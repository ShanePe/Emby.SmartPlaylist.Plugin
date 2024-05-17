import { RuleOrRuleGroup } from '~/app/types/rule';
import { TreeNodeData, TreeViewData } from '~/common/components/TreeView/types/tree';
import { PlaylistState } from '~/app/state/playlist/playlist.reducer';
import { isShuffleUpdateType, PlaylistBasicData } from '~/app/types/playlist';

export type PlaylistSelectors = {
    getRulesTree(): TreeViewData<RuleOrRuleGroup>;
    getBasicData(): PlaylistBasicData;
    isShuffleUpdateType(): boolean;
    getPlaylist(): PlaylistState;
    getLastTreeNode(): TreeNodeData;
};

export const createPlaylistSelectors = (state: PlaylistState): PlaylistSelectors => {
    return {
        getRulesTree: (): TreeViewData<RuleOrRuleGroup> => {
            return state.rulesTree;
        },
        getBasicData: (): PlaylistBasicData => {
            const { rulesTree, ...basicData } = state;
            return basicData;
        },
        isShuffleUpdateType: (): boolean => {
            return isShuffleUpdateType(state.updateType);
        },
        getPlaylist: (): PlaylistState => {
            return state;
        },
        getLastTreeNode: (): TreeNodeData => {
            let keys = Object.keys(state.rulesTree.byId);
            if (keys.length !== 0)
                return state.rulesTree.byId[keys[keys.length - 1]];

            keys = Object.keys(state.rulesTree.rootIds);
            return state.rulesTree.rootIds[keys[keys.length - 1]];
        }
    };
};
