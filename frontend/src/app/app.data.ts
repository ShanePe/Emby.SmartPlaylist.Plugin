import { AppData, AppPlaylist } from '~/app/types/appData';
import { demoAppData, demoAppPlaylistView, demoServerResponse } from '~/app/app.demo';
import { Playlist, PlaylistInfo, ServerResponse } from './types/playlist';

export const loadAppData = (appId: string): Promise<AppData> => {
    return new Promise<AppData>(res => {
        res({
            appId: appId,
            ...demoAppData,
        });
    });
};

export const saveAppPlaylist = (playlist: AppPlaylist, saveSortJob: boolean): Promise<ServerResponse<Playlist> | void> => {
    // tslint:disable-next-line:no-console
    return new Promise<ServerResponse<Playlist> | void>(res => {
        res();
    });
};

export const deletePlaylist = (playlistId: string, remove: boolean): Promise<void> => {
    return new Promise<void>(res => {
        res();
    });
};

export const viewPlaylist = (playlistId: string, execute: boolean): Promise<PlaylistInfo> => {
    return new Promise<PlaylistInfo>(res => {
        res({
            ...demoAppPlaylistView
        });
    });
};

export const viewPlaylistLog = (playlistId: string): Promise<string> => {
    return new Promise<string>(res => {
        res('');
    });
};

export const importPlaylists = (uploadFile: File): Promise<ServerResponse<string>> => {
    return new Promise<ServerResponse<string>>(res => {
        res({
            ...demoServerResponse
        })
    })
}