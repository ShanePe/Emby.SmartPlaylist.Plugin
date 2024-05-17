import * as React from 'react';
import { Playlist, ServerResponse } from '~/app/types/playlist';
import { createPlaylist, duplicatePlaylist } from '~/app/types/playlist.factory';
import { AppAction, AppState } from '~/app/state/app.reducer';
import { AppData, AppPlaylist } from '~/app/types/appData';
import { saveAppPlaylist, deletePlaylist, viewPlaylist } from '~/app/app.data';
import { getAppEditPlaylist, getAppPlaylistForPlaylist, getAppSortJobPlaylist } from '~/app/state/app.selectors';
import { ConfirmDeletePlaylist, DeleteData } from '~/emby/components/Confirmation';
import { showError, showInfo } from '~/common/helpers/utils';


export type AppActions = {
    addNewPlaylist(): void;
    editPlaylist(plalist: Playlist): void;
    duplicatePlaylist(plalist: Playlist): void;
    editSortJob(plalist: Playlist): void;
    updatePlaylist(plalist: Playlist): void;
    savePlaylist(): void;
    saveSortJob(): void;
    deletePlaylist(plalist: Playlist, keep: boolean): void;
    discardPlaylist(): void;
    loadAppData(appData: AppData): void;
    viewPlaylist(plalist: Playlist): void;
    executePlaylist(plalist: Playlist): void;
    confirmDeletePlaylist(plalist: Playlist): void;
    reset(): void;
};

export function handleSaveResponse(response: ServerResponse<Playlist>, dis) {
    if (response.success) {
        dis({
            type: 'app:savePlaylist',
            playlist: response.response
        });
    }
    else {
        showError({ label: "Error saving playlist", content: response.error, modal: false })
    }
}

export const createAppActions = (
    dispatcher: React.Dispatch<AppAction>,
    state: AppState,
): AppActions => {
    return {
        addNewPlaylist: () => {
            dispatcher({
                type: 'app:addNewPlaylist',
                playlist: createPlaylist(),
            });
        },
        editPlaylist: (playlist: Playlist) => {
            dispatcher({
                type: 'app:editPlaylist',
                playlist: playlist,
            });
        },
        duplicatePlaylist: (playlist: Playlist) => {
            dispatcher({
                type: 'app:addNewPlaylist',
                playlist: duplicatePlaylist(playlist),
            });
        },
        editSortJob: (playlist: Playlist) => {
            dispatcher({
                type: 'app:editSortJob',
                playlist: playlist,
            });
        },
        updatePlaylist: async (playlist: Playlist) => {
            await saveAppPlaylist(getAppPlaylistForPlaylist(playlist), false);
            dispatcher({
                type: 'app:updatePlaylist',
                playlist: playlist
            });
        },
        savePlaylist: async () => {
            handleSaveResponse(await saveAppPlaylist(getAppEditPlaylist(state), false) as ServerResponse<Playlist>, dispatcher);

        },
        saveSortJob: async () => {
            handleSaveResponse(await saveAppPlaylist(getAppSortJobPlaylist(state), true) as ServerResponse<Playlist>, dispatcher);
        },

        deletePlaylist: async (plalist: Playlist, keep: boolean) => {
            await deletePlaylist(plalist.id, keep);
            dispatcher({
                type: 'app:removePlaylist',
                playlist: plalist,
                keep: keep
            });
        },
        viewPlaylist: async (plalist: Playlist) => {
            dispatcher({
                type: 'app:loadPlaylistInfo',
                playlistInfo: await viewPlaylist(plalist.id, false)
            });
        },
        executePlaylist: async (plalist: Playlist) => {
            showInfo('Executing playlist: ' + plalist.name, false);
            viewPlaylist(plalist.id, true)
        },
        discardPlaylist: () => {
            dispatcher({
                type: 'app:discardPlaylist',
            });
        },
        confirmDeletePlaylist: (plalist: Playlist) => {
            dispatcher({
                type: 'app:confirmDeletePlaylist',
                confirmationProps: {
                    title: 'Confirm playlist removal',
                    data: { playlist: plalist, keep: false } as DeleteData,
                    question: `Are you sure you want to delete the playlist: "${plalist.name}" ?`,
                    control: ConfirmDeletePlaylist,
                    onNo: (data => { }),
                    onYes: (data => {
                        deletePlaylist(data.playlist.id, data.keep).finally(() => {
                            dispatcher({
                                type: 'app:removePlaylist',
                                playlist: data.playlist,
                                keep: data.keep
                            })
                        })
                    })
                }
            })
        },
        loadAppData: (appData: AppData) => {
            dispatcher({
                type: 'app:loadSettings',
                settings: appData,
            });
        },
        reset: () => {
            dispatcher({
                type: 'app:reset'
            });
        }
    };
};
