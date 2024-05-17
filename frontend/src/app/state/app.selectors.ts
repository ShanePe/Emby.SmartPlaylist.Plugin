import { Playlist, PlaylistInfo, SourceType } from '~/app/types/playlist';
import {
    RuleCriteriaDefinition,
    RuleCriteriaOperator,
    ListValueRange,
    ListValue,
    ListMapValue
} from '~/app/types/rule';
import { AppPlaylistState, AppState } from '~/app/state/app.reducer';
import { AppData, AppPlaylist, Source, User } from '~/app/types/appData';
import { TreeViewData } from '~/common/components/TreeView/types/tree';
import { ConfirmationProps } from '~/emby/components/Confirmation';
import { SourceTypes, UpdateTypes } from '../app.const';

export type AppSelectors = {
    getPlaylists(): Playlist[];
    getEditedPlaylist(): Playlist;
    getSortJobPlaylist(): Playlist;

    isNewPlaylist(id: string): boolean;
    getRuleCriteriaOperators(criteriaName: string): RuleCriteriaOperator[];
    getRulesCriteriaDefinitions(): RuleCriteriaDefinition[];
    getUsers(): User[];
    getRulesCriteriaDefinition(criteriaName: string): RuleCriteriaDefinition;
    getAppData(): AppData;
    getOrdersBy(): string[];
    getViewPlaylist(): PlaylistInfo;
    getConfirmation(): ConfirmationProps;
    getSourcesFor(type: string): Source[];
    isLoaded(): boolean;
};

export const createAppSelectors = (state: AppState): AppSelectors => {
    return {
        getPlaylists: (): Playlist[] => {
            return state.playlists.names.map(name => state.playlists.byId[name]);
        },
        getViewPlaylist: (): PlaylistInfo => {
            return state.viewPlaylist;
        },
        getEditedPlaylist: (): Playlist => {
            return state.editedPlaylist;
        },
        getSortJobPlaylist: (): Playlist => {
            return state.sortJobPlaylist;
        },
        isNewPlaylist: (id: string): boolean => {
            return !Object.keys(state.playlists.byId).includes(id);
        },
        getRuleCriteriaOperators: (criteriaName: string): RuleCriteriaOperator[] => {
            const ruleCritDef = state.rulesCriteriaDefinitions.find(x => x.name === criteriaName);
            if (ruleCritDef && ruleCritDef.type.operators) {
                return ruleCritDef.type.operators;
            } else {
                return [];
            }
        },
        getRulesCriteriaDefinitions: (): RuleCriteriaDefinition[] => {
            return state.rulesCriteriaDefinitions;
        },
        getRulesCriteriaDefinition: (criteriaName: string): RuleCriteriaDefinition => {
            return state.rulesCriteriaDefinitions.find(x => x.name === criteriaName);
        },
        getAppData: (): AppData => {
            return getAppData(state);
        },
        getOrdersBy: (): string[] => {
            return state.limitOrdersBy;
        },
        getConfirmation: (): ConfirmationProps => {
            return state.confirmation;
        },
        getSourcesFor: (type: SourceType): Source[] => {
            if (type === SourceTypes[3]) {
                return state.playlists.names.map(name => state.playlists.byId[name])
                    .filter(p => p.enabled && p.updateType !== UpdateTypes[0])
                    .map(p => {
                        return { id: p.id, name: p.name, type: SourceTypes[3] } as Source
                    }).sort((a, b) => a.name.localeCompare(b.name));
            }
            else
                return state.sources.filter(s => s.type === type);
        },
        isLoaded: (): boolean => {
            return state.loadedPlaylist
        },
        getUsers: (): User[] => {
            return state.users;
        }
    };
};

export const getAppData = (state: AppState): AppData => {
    return {
        ...state,
        playlists: convertToAppPlaylists(state.playlists),
        sources: state.sources,
        users: state.users
    };
};

export const getAppEditPlaylist = (state: AppState): AppPlaylist => {
    return getAppPlaylistForPlaylist(state.editedPlaylist);
};

export const getAppSortJobPlaylist = (state: AppState): AppPlaylist => {
    return getAppPlaylistForPlaylist(state.sortJobPlaylist);
}

export const getAppPlaylistForPlaylist = (playlist: Playlist): AppPlaylist => {
    return {
        ...playlist,
        rulesTree: getOrderedNodeIds(playlist.rulesTree.rootIds, playlist.rulesTree).map(
            id => playlist.rulesTree.byId[id],
        ),
    };
}

const convertToAppPlaylists = (playlistState: AppPlaylistState): AppPlaylist[] => {
    return playlistState.names
        .map(x => playlistState.byId[x])
        .map(x => ({
            ...x,
            rulesTree: getOrderedNodeIds(x.rulesTree.rootIds, x.rulesTree).map(
                id => x.rulesTree.byId[id],
            ),
        }));
};

const getOrderedNodeIds = (ids: string[], treeViewData: TreeViewData): string[] => {
    const arr: string[] = [];

    ids.forEach(id => {
        arr.push(id);
        const node = treeViewData.byId[id];
        if (node.children.length > 0) {
            getOrderedNodeIds(node.children, treeViewData).map(i => arr.push(i));
        }
    });

    return arr;
};
