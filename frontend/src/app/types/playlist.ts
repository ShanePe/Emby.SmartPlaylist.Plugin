import { TreeViewData } from '~/common/components/TreeView/types/tree';
import { RuleOrRuleGroup } from '~/app/types/rule';
import { LimitOrderByValues, CollectionModes, SmartTypes, UpdateTypes, SourceTypes } from '~/app/app.const';
import { Source } from './appData';

export type PlaylistRulesTree<T> = {
    rulesTree: T;
};

export type PlaylistBasicData = {
    id: string;
    name: string;
    limit: PlaylistLimit;
    updateType: UpdateType;
    smartType: SmartType;
    internalId?: number;
    collectionMode: CollectionMode;
    priorNames: [];
    enabled: boolean;
    newItemOrder: NewItemOrder;
    sourceType: SourceType,
    source: Source | undefined,
    sortJob: SortJob | undefined,
    monitorMode: boolean | undefined,
    uiSections: UISections | undefined,
    notes: string | undefined
};

export type PlaylistLimit = {
    hasLimit: boolean;
    maxItems: number;
    orderBy: string;
};

export type NewItemOrder = {
    hasSort: boolean;
    orderBy: string;
}

export type SortJob = {
    enabled: boolean;
    updateType: UpdateType;
    orderBy: string,
    syncCount?: number,
    lastSyncDuration?: number,
    status?: string,
    nextUpdate?: Date,
    lastUpdated?: Date,
    lastRan?: Date,
    lastDurationStr?: string,
    thenBys?: string[];
}

export type UISections = {
    setup: boolean,
    sort: boolean,
    rules: boolean,
    notes: boolean
}

export type PlaylistViewData = PlaylistBasicData & {
    internalId?: number,
    lastShuffleUpdate?: Date,
    lastUpdated?: Date,
    lastSync?: Date,
    syncCount?: number,
    lastDuration?: number,
    status?: string,
    lastDurationStr?: string
    items: string[],
    ruleCount: number
}
export type Playlist = PlaylistBasicData & PlaylistRulesTree<TreeViewData<RuleOrRuleGroup>>;
export type PlaylistInfo = PlaylistViewData & PlaylistRulesTree<TreeViewData<RuleOrRuleGroup>>;

export type UpdateType = typeof UpdateTypes[number];

export type SourceType = typeof SourceTypes[number];

export type LimitOrderBy = typeof LimitOrderByValues[number];

export type SmartType = typeof SmartTypes[number];

export type CollectionMode = typeof CollectionModes[number];

export type ServerResponse<T> = {
    success: boolean;
    error?: string;
    response?: T
}

export const isShuffleUpdateType = (updateType: UpdateType) => {
    return (
        updateType === 'ShuffleDaily' ||
        updateType === 'ShuffleMonthly' ||
        updateType === 'ShuffleWeekly'
    );
};

export const getIconsForPlayList = (playList: Playlist) => {
    var icons: string[] = []

    icons.push(playList.smartType == 'Collection' ? 'video_library' : 'featured_play_list');

    if (playList.updateType == 'Daily' || playList.updateType == 'ShuffleDaily')
        icons.push('today');
    else if (playList.updateType == 'Weekly' || playList.updateType == 'ShuffleWeekly')
        icons.push('date_range');
    else if (playList.updateType == 'Monthly' || playList.updateType == 'ShuffleMonthly')
        icons.push('event_note');
    else if (playList.updateType == 'Live')
        icons.push('directions_run');
    else if (playList.updateType == 'Manual')
        icons.push('accessibility')

    if (isShuffleUpdateType(playList.updateType))
        icons.push('shuffle');

    return icons.join('');
}
