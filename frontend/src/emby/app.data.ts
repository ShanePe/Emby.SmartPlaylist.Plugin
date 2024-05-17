import { AppData, AppPlaylist, HierarchyString, Upload } from '~/app/types/appData';
import camelcaseKeys = require('camelcase-keys');
import { parseDate } from '~/common/helpers/date';
import { convertObjectPropValues } from '~/common/helpers/object';
import { Playlist, PlaylistInfo, ServerResponse } from '~/app/types/playlist';
import { toBase64 } from '~/common/helpers/utils';

type ApiClient = {
    getPluginConfiguration<TConfig>(pluginId: string): Promise<TConfig>;
    updatePluginConfiguration<TConfig>(pluginId: string, config: TConfig): Promise<any>;
    serverId(): string;
    deviceId(): string;
    accessToken(): string;
    appVersion(): string;
    deviceName(): string;
    appName(): string;
    ajax<T = any>(request: any): Promise<T>;
};

export const version = "2.5.1.4850";

declare global {
    // tslint:disable-next-line:interface-name
    interface Window {
        Dashboard: any;
        ApiClient: ApiClient;
    }
}

export function convertResponse<T>(data: T): T {
    data = camelcaseKeys(data, {
        deep: true,
    }) as T;

    convertObjectPropValues(data, o => parseDate(o));
    return data;
}

function getContextPath() {
    var ctx = window.location.pathname,
        path = '/' !== ctx ? ctx.substring(0, ctx.indexOf('/', 1) + 1) : ctx;
    return path + (/\/$/.test(path) ? '' : '/');
}

export const loadAppData = async (appId: string): Promise<AppData> => {
    let appData = await window.ApiClient.ajax<AppData>(
        {
            url: `../smartplaylist/appData?v=${version}`,
            type: 'GET',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
            },
            contentType: 'application/json',
            dataType: 'json',
        }
    );

    return new Promise<AppData>(res => {
        res({
            appId: appId,
            ...convertResponse<AppData>(appData),
        });
    });
};

export const saveAppPlaylist = async (playlist: AppPlaylist, sortJobSave: boolean): Promise<ServerResponse<Playlist>> => {
    let response = await window.ApiClient.ajax<ServerResponse<Playlist>>(
        {
            url: `../smartplaylist${sortJobSave ? "/sort" : ""}?v=${version}`,
            type: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
            },
            data: JSON.stringify(playlist),
            contentType: 'application/json',
            dataType: 'json',
        }
    );
    return new Promise<ServerResponse<Playlist>>(res => {
        res(convertResponse<ServerResponse<Playlist>>(response))
    })
};

export const deletePlaylist = async (playlistId: string, keep: boolean): Promise<any> => {
    return window.ApiClient.ajax(
        {
            url: `../smartplaylist/${playlistId}/${keep}?v=${version}`,
            type: 'DELETE',
        }
    );
};

export const viewPlaylist = async (playlistId: string, execute: boolean): Promise<PlaylistInfo> => {
    let playlistInfo = await window.ApiClient.ajax<PlaylistInfo>(
        {
            url: `../smartplaylist/info/${playlistId}?v=${version}`,
            type: execute ? 'POST' : 'GET',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
            },
            contentType: 'application/json',
            dataType: 'json',
        }
    );

    return new Promise<PlaylistInfo>(res => {
        res(convertResponse<PlaylistInfo>(playlistInfo));
    });
};

export const viewPlaylistLog = (playlistId: string): Promise<string> => {
    return window.ApiClient.ajax(
        {
            url: `../smartplaylist/log/${playlistId}/?v=${version}`,
            type: 'GET',
            contentType: 'text/plain',
            dataType: 'text'
        }
    );
}

export const explainPlaylistRules = async (playlist: AppPlaylist): Promise<ServerResponse<HierarchyString>> => {
    let response = await window.ApiClient.ajax(
        {
            url: `../smartplaylist/explain_rules?v=${version}`,
            type: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
            },
            data: JSON.stringify(playlist),
            contentType: 'application/json',
            dataType: 'json',
        }
    )

    return new Promise<ServerResponse<HierarchyString>>(res => {
        res(convertResponse<ServerResponse<HierarchyString>>(response));
    });
}

export const importFile = async (props: Upload): Promise<ServerResponse<string>> => {

    const payload = {
        "type": props.type,
        "uploadFile": await toBase64(props.uploadFile)
    }

    let response = await window.ApiClient.ajax(
        {
            url: `../smartplaylist/import?v=${version}`,
            type: 'POST',
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json',
            },
            data: JSON.stringify(payload),
            contentType: 'application/json',
            dataType: 'json',
        }
    );

    return new Promise<ServerResponse<string>>(res => {
        res(convertResponse<ServerResponse<string>>(response));
    });
}