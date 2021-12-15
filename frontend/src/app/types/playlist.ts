import { TreeViewData } from '~/common/components/TreeView/types/tree';
import { RuleOrRuleGroup } from '~/app/types/rule';
import { LimitOrderByValues, CollectionModes, SmartTypes, UpdateTypes } from '~/app/app.const';

export type PlaylistRulesTree<T> = {
    rulesTree: T;
};

export type PlaylistBasicData = {
    id: string;
    name: string;
    limit: PlaylistLimit;
    updateType: UpdateType;
    smartType: SmartType;
    collectionMode: CollectionMode;
    priorNames: [];
    mustUpdate: boolean;
};

export type PlaylistLimit = {
    hasLimit: boolean;
    maxItems: number;
    orderBy: LimitOrderBy;
};

export type Playlist = PlaylistBasicData & PlaylistRulesTree<TreeViewData<RuleOrRuleGroup>>;

export type UpdateType = typeof UpdateTypes[number];

export type LimitOrderBy = typeof LimitOrderByValues[number];

export type SmartType = typeof SmartTypes[number];

export type CollectionMode = typeof CollectionModes[number];

export const isShuffleUpdateType = (updateType: UpdateType) => {
    return (
        updateType === 'ShuffleDaily' ||
        updateType === 'ShuffleMonthly' ||
        updateType === 'ShuffleWeekly'
    );
};
