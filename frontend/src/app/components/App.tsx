
import { PlaylistEditor } from '~/app/components/PlaylistEditor';
import { SortJobEditor } from '~/app/components/SortJobEditor';
import * as React from 'react';
import { createPlaylistContextValue, PlaylistContext } from '~/app/state/playlist/playlist.context';
import { appReducer, initAppState } from '~/app/state/app.reducer';
import { AppContext, createAppContextValue } from '~/app/state/app.context';
import { loadAppData } from '~/app/app.data';
import { AppData } from '~/app/types/appData';
import { PlaylistList } from '~/app/components/PlaylistList';
import { PlaylistDetail } from '~/app/components/PlaylistDetail';
import './App.css';
import { Confirmation } from '~/emby/components/Confirmation';
import { Modal } from '~/emby/components/Modal';
import BeatLoader from "react-spinners/BeatLoader"
import { dismissToast, openUrl, showError, showInfo, utils_configure } from '~/common/helpers/utils';
import { Menu } from './Menu';
import { Export } from './Export';
import { Inline } from '~/common/components/Inline';
import { Import } from './Import';
import { ToastContainer } from 'react-toastify';

export type AppProps = {
    appId: string;
};

enum UIFlags {
    None,
    Export,
    Import
}

utils_configure();

export const App: React.FC<AppProps> = props => {

    const [appState, appDispatcher] = React.useReducer(appReducer, {
        ...initAppState,
    });

    const appContext = createAppContextValue(appState, appDispatcher);

    React.useEffect(() => {
        loadPlaylists();
    }, [props.appId]);

    const {
        addNewPlaylist,
        discardPlaylist,
        savePlaylist,
        saveSortJob,
        getEditedPlaylist,
        isNewPlaylist,
        getViewPlaylist,
        getConfirmation,
        isLoaded,
        getSortJobPlaylist,
        reset
    } = appContext;

    function loadPlaylists() {
        loadAppData(props.appId).then((appData: AppData) => {
            try {
                appContext.loadAppData(appData);
            } catch (e) {
                showError({ label: "Error loading playlists", content: e, modal: false });
            }
        });
    }

    const editedPlaylist = getEditedPlaylist();
    const viewPlaylistInfo = getViewPlaylist();
    const confirmation = getConfirmation();
    const loaded = isLoaded()
    const sortJobPlaylist = getSortJobPlaylist();

    const [uiFlag, setUIFlag] = React.useState(UIFlags.None);

    const doUIFlagResponse = (func, error: string) => {
        try {
            func();
        } catch (e) {
            showError({ label: error, content: e, modal: false });
        } finally {
            setUIFlag(UIFlags.None)
        }
    }

    return (
        <>
            <AppContext.Provider value={appContext}>
                {loaded && (
                    <Menu
                        open={appContext.getPlaylists().length === 0}
                        menuItems={[
                            { label: 'Add Smart Playlist', icon: 'add', onClick: () => addNewPlaylist() },
                            { label: 'Export Playlists', icon: 'arrow_circle_down', onClick: () => setUIFlag(UIFlags.Export) },
                            { label: 'Import Playlists', icon: 'arrow_circle_up', onClick: () => setUIFlag(UIFlags.Import) }
                        ]}
                    />
                )}

                <ToastContainer containerId='appToast' />

                {(loaded && appContext.getPlaylists().length === 0) && (
                    <>
                        <Inline>
                            <b><u>Welcome to Smart Playlist!</u></b>
                        </Inline>
                        <Inline>
                            <p>You currently do not have any smart playlists or collections added,
                                to do so, select <b>'Add New Playlist'</b> from the menu on the right.
                                <i className="md-icon" style={{ fontSize: "x-large" }}>pending</i></p>
                        </Inline>
                    </>
                )}

                {!loaded && (
                    <div className='app-container'>
                        <BeatLoader loading={true} color='green' />
                    </div>
                )}

                <div className="verticalSection verticalSection-extrabottompadding app-container">
                    <PlaylistList />
                </div>

                {editedPlaylist && (
                    <Modal
                        confirmLabel="Save"
                        onClose={() => { dismissToast(); discardPlaylist(); }}
                        title={isNewPlaylist(editedPlaylist.id) ? 'Add Playlist' : 'Edit Playlist'}
                        onConfirm={() => { dismissToast(); savePlaylist(); }}
                    >
                        <PlaylistContext.Provider
                            value={createPlaylistContextValue(
                                editedPlaylist,
                                appDispatcher,
                                appContext,
                            )}
                        >
                            <PlaylistEditor />
                        </PlaylistContext.Provider>
                    </Modal>
                )}

                {viewPlaylistInfo && (
                    <Modal
                        confirmLabel='Close'
                        title={`Playlist detail for ${viewPlaylistInfo.name}`}
                        onClose={() => discardPlaylist()}
                        onConfirm={() => discardPlaylist()}
                    >
                        <PlaylistDetail
                            playlist={viewPlaylistInfo}
                        />
                    </Modal>
                )}

                {sortJobPlaylist && (
                    <Modal
                        confirmLabel="Save"
                        onClose={() => discardPlaylist()}
                        title={`Edit Sort Job for ${sortJobPlaylist.name}`}
                        onConfirm={() => saveSortJob()}
                    >
                        <PlaylistContext.Provider
                            value={createPlaylistContextValue(
                                sortJobPlaylist,
                                appDispatcher,
                                appContext,
                            )}
                        >
                            <SortJobEditor />
                        </PlaylistContext.Provider>
                    </Modal>
                )}

                {confirmation && (
                    <Confirmation
                        {...confirmation}
                        onNo={(data) => discardPlaylist()}
                        onYes={(data) => {
                            confirmation.onYes(data);
                        }}
                    >
                    </Confirmation>
                )}

                {uiFlag === UIFlags.Export && (
                    <Export
                        playlists={appContext.getPlaylists()}
                        onClose={() => setUIFlag(UIFlags.None)}
                        onConfirm={(ids) => doUIFlagResponse(() => {
                            if (ids.length !== 0)
                                openUrl(`../smartplaylist/export/${window.btoa(ids.join(','))}`, false);
                        }, 'Error exporting')}
                    />
                )}

                {uiFlag === UIFlags.Import && (
                    <Import
                        onClose={() => setUIFlag(UIFlags.None)}
                        onConfirm={(response) => doUIFlagResponse(() => {
                            if (response.success) {
                                new Promise<void>((res) => {
                                    showInfo('Import successful: ' + response.response, false);
                                    res();
                                }).then(() => {
                                    reset();
                                    loadPlaylists();
                                });
                            } else
                                throw Error(response.error);

                        }, 'Error importing')}
                    />
                )}
            </AppContext.Provider>
        </>
    );
};
