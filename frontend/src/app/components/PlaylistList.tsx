import * as React from 'react';
import { AppContext } from '~/app/state/app.context';
import './PlaylistList.css'
import { ListItem } from '~/common/components/ListItem';
import { Playlist } from '../types/playlist';


type PlaylistListProps = {};

export const PlaylistList: React.FC<PlaylistListProps> = () => {
    const appContext = React.useContext(AppContext);

    const { getPlaylists, editPlaylist, updatePlaylist, confirmDeletePlaylist, viewPlaylist, executePlaylist, editSortJob, duplicatePlaylist } = appContext;

    return (
        <>
            {getPlaylists().map(playlist => (

                <ListItem
                    onDeleteClick={() => confirmDeletePlaylist(playlist)}
                    onEditClick={() => editPlaylist(playlist)}
                    onDuplicateClick={() => duplicatePlaylist(playlist)}
                    onViewClick={() => viewPlaylist(playlist)}
                    onExecuteClick={() => executePlaylist(playlist)}
                    onSortJobClick={() => editSortJob(playlist)}
                    onOpenClick={() => { window.open('index.html#!/item?id=' + playlist.internalId + "&serverId=" + window.ApiClient.serverId(), playlist.name) }}
                    onUpdateDataClick={(pl: Partial<Playlist>) => {
                        updatePlaylist({
                            ...playlist,
                            ...pl
                        });

                    }}
                    playList={playlist}
                />
            ))}
        </>
    );
};
