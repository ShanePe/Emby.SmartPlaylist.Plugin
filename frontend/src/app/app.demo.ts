import { RuleCriteriaDefinition } from '~/app/types/rule';
import { AppData, AppPlaylist } from '~/app/types/appData';
import { createTreeNodeData } from '~/common/components/TreeView/types/tree.factory';
import { Guid } from '~/common/helpers/guid';
import { createRule, createRuleGroup } from '~/app/types/rule.factory';
import {
    dateCriteriaDefinitionType,
    defaultPlaylistLimit,
    defaultRuleCriteriaDefinition,
    stringCriteriaDefinitionType,
    defaultUpdateType,
    defaultSmartType,
    defaultCollectionMode,
    defaultNewItemOrder,
    defaultSortJob,
    defaultUISections
} from '~/app/app.const';
import { PlaylistInfo, ServerResponse } from './types/playlist';

export const demoRulesCritDefinitions: RuleCriteriaDefinition[] = [
    defaultRuleCriteriaDefinition,
    {
        name: 'Artist',
        values: [],
        type: stringCriteriaDefinitionType,
        isUserSpecific: false
    },
    {
        name: 'DateLastSaved',
        type: dateCriteriaDefinitionType,
        values: [],
        isUserSpecific: false
    },
];

export const demoAppPlaylists: AppPlaylist[] = [
    {
        id: Guid.newGuid(),
        name: 'Playlist1',
        limit: defaultPlaylistLimit,
        updateType: defaultUpdateType,
        smartType: defaultSmartType,
        collectionMode: defaultCollectionMode,
        priorNames: [],
        enabled: true,
        newItemOrder: defaultNewItemOrder,
        sourceType: 'Media Items',
        source: undefined,
        rulesTree: [
            createTreeNodeData({
                isRoot: true,
                data: createRuleGroup(),
                children: ['child1'],
            }),
            createTreeNodeData({
                id: 'child1',
                data: createRule(),
                level: 1,
            }),
        ],
        sortJob: defaultSortJob,
        monitorMode: false,
        uiSections: defaultUISections,
        notes: ''
    },
];

export const demoAppPlaylistView: PlaylistInfo = {
    id: Guid.newGuid(),
    name: 'Playlist1',
    limit: defaultPlaylistLimit,
    updateType: defaultUpdateType,
    smartType: defaultSmartType,
    collectionMode: defaultCollectionMode,
    priorNames: [],
    enabled: true,
    items: [],
    ruleCount: 0,
    newItemOrder: defaultNewItemOrder,
    sourceType: 'Media Items',
    source: undefined,
    rulesTree: {
        byId: {},
        rootIds: []
    },
    sortJob: defaultSortJob,
    monitorMode: false,
    uiSections: defaultUISections,
    notes: ''
}

export const demoLimitOrdersBy = ['Album', 'Artist'];

export const demoAppData: AppData = {
    appId: Guid.newGuid(),
    playlists: demoAppPlaylists,
    rulesCriteriaDefinitions: demoRulesCritDefinitions,
    limitOrdersBy: demoLimitOrdersBy,
    sources: [],
    users: []
};

export const demoServerResponse: ServerResponse<string> = {
    success: true,
    response: ''
}
