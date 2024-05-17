import * as React from 'react';
import { Playlist, PlaylistInfo } from '~/app/types/playlist';
import { RuleCriteriaDefinition } from '~/app/types/rule';
import { PlaylistAction, playlistReducer } from '~/app/state/playlist/playlist.reducer';
import { normalizeArray } from '~/common/helpers/array';
import { createTreeViewData } from '~/common/components/TreeView/types/tree.factory';
import { AppData, Source, User } from '~/app/types/appData';
import { defaultPlaylistLimit, defaultUISections } from '~/app/app.const';
import { ConfirmationProps } from '~/emby/components/Confirmation';
import { getAppPlaylistForPlaylist } from './app.selectors';
import { TreeNodeData } from '~/common/components/TreeView/types/tree';
import { Guid } from '~/common/helpers/guid';

export type AppPlaylistState = {
    byId: {
        [Key: string]: Playlist;
    };
    names: string[];
};

export type AppState = {
    appId: string;
    loadedPlaylist: boolean,
    playlists: AppPlaylistState;
    rulesCriteriaDefinitions: RuleCriteriaDefinition[];
    limitOrdersBy: string[];
    editedPlaylist?: Playlist;
    viewPlaylist?: PlaylistInfo,
    confirmation?: ConfirmationProps,
    sources: Source[],
    sortJobPlaylist?: Playlist,
    users: User[]

};

export const initAppState: AppState = {
    appId: '',
    loadedPlaylist: false,
    playlists: {
        byId: {},
        names: [],
    },
    rulesCriteriaDefinitions: [],
    limitOrdersBy: [],
    editedPlaylist: undefined,
    viewPlaylist: undefined,
    sources: [],
    users: [],
    sortJobPlaylist: undefined
};

export type AppAction =
    | { type: 'app:loadSettings'; settings: AppData }
    | { type: 'app:addNewPlaylist'; playlist: Playlist }
    | { type: 'app:editPlaylist'; playlist: Playlist }
    | { type: 'app:editSortJob'; playlist: Playlist }
    | { type: 'app:discardPlaylist' }
    | { type: 'app:updatePlaylist'; playlist: Playlist }
    | { type: 'app:savePlaylist'; playlist: Playlist }
    | { type: 'app:removePlaylist'; playlist: Playlist, keep: boolean }
    | { type: 'app:loadPlaylistInfo'; playlistInfo: PlaylistInfo }
    | { type: 'app:confirmDeletePlaylist'; confirmationProps: ConfirmationProps }
    | { type: 'app:reset' };

export const appReducer: React.Reducer<AppState, AppAction | PlaylistAction> = (state, action) => {
    switch (action.type) {
        case 'app:loadSettings': {
            const playlists = action.settings.playlists.map(x => ({
                limit: defaultPlaylistLimit,
                ...x,
                rulesTree: createTreeViewData(x.rulesTree),
            }));

            return {
                ...action.settings,
                loadedPlaylist: true,
                playlists: {
                    byId: normalizeArray(playlists, 'id'),
                    names: playlists.map(x => x.id),
                },
            };
        }
        case 'app:addNewPlaylist': {
            return {
                ...state,
                editedPlaylist: { ...action.playlist },
                viewPlaylist: undefined,
                confirmation: undefined,
                sortJobPlaylist: undefined
            };
        }
        case 'app:editPlaylist': {
            return {
                ...state,
                editedPlaylist: { ...state.playlists.byId[action.playlist.id] },
                viewPlaylist: undefined,
                confirmation: undefined,
                sortJobPlaylist: undefined
            };
        }
        case 'app:editSortJob': {
            return {
                ...state,
                editedPlaylist: undefined,
                viewPlaylist: undefined,
                confirmation: undefined,
                sortJobPlaylist: { ...state.playlists.byId[action.playlist.id] }
            };
        }
        case 'app:discardPlaylist': {
            return {
                ...state,
                editedPlaylist: undefined,
                viewPlaylist: undefined,
                confirmation: undefined,
                sortJobPlaylist: undefined
            };
        }
        case 'app:removePlaylist': {
            const { [action.playlist.id]: deleted, ...byId } = state.playlists.byId;
            return {
                ...state,
                editedPlaylist: undefined,
                viewPlaylist: undefined,
                confirmation: undefined,
                sortJobPlaylist: undefined,
                playlists: {
                    ...state.playlists,
                    byId: byId,
                    names: state.playlists.names.filter(x => x !== action.playlist.id),
                }
            };
        }
        case 'app:reset': {
            return {
                ...initAppState
            }
        }
        case 'app:loadPlaylistInfo': {
            return {
                ...state,
                viewPlaylist: action.playlistInfo,
                editedPlaylist: undefined
            };
        }
        case 'app:updatePlaylist': {
            let names = state.playlists.names;
            if (!names.includes(action.playlist.id)) {
                names = [...names, action.playlist.id];
            }
            return {
                ...state,
                playlists: {
                    ...state.playlists,
                    byId: {
                        ...state.playlists.byId,
                        [action.playlist.id]: {
                            ...action.playlist,
                        },
                    },
                    names: names,
                }
            };
        }
        case 'app:savePlaylist': {
            return refreshPlaylist(action.playlist);
        }
        case 'app:confirmDeletePlaylist': {
            return {
                ...state,
                confirmation: action.confirmationProps
            }
        }

        default: {
            return state.editedPlaylist ?
                {
                    ...state,
                    editedPlaylist: {
                        ...state.editedPlaylist,
                        ...playlistReducer(state.editedPlaylist, action as PlaylistAction)
                    },
                    sortJobPlaylist: undefined
                } :
                state.sortJobPlaylist ? {
                    ...state,
                    editedPlaylist: undefined,
                    sortJobPlaylist: {
                        ...state.sortJobPlaylist,
                        ...playlistReducer(state.sortJobPlaylist, action as PlaylistAction)
                    }

                } : {
                    ...state
                };
        }
    }

    function refreshPlaylist(playlist: Playlist) {
        let names = state.playlists.names;
        if (!names.includes(playlist.id)) {
            names = [...names, playlist.id];
        }

        if (playlist.rulesTree instanceof Array) {
            playlist.rulesTree = createTreeViewData(playlist.rulesTree as TreeNodeData[])
        }

        return {
            ...state,
            playlists: {
                ...state.playlists,
                byId: {
                    ...state.playlists.byId,
                    [playlist.id]: {
                        ...playlist,
                    },
                },
                names: names,
            },
            editedPlaylist: undefined,
            sortJobPlaylist: undefined
        };
    }
};

