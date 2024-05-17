import { Rule, RuleCriteriaDefinition, RuleGroup, RuleOrRuleGroup } from '~/app/types/rule';
import { TreeNodeData } from '~/common/components/TreeView/types/tree';
import { createTreeNodeData } from '~/common/components/TreeView/types/tree.factory';
import {
    addChildNode,
    addNode,
    changeExpand,
    removeNode,
} from '~/common/components/TreeView/actions/treeViewData.actions';
import { Playlist, PlaylistInfo } from '~/app/types/playlist';

export type RulesCriteriaDefinitionsState = {
    byId: {
        [Key: string]: RuleCriteriaDefinition;
    };
    names: string[];
};

export type PlaylistState = Playlist | PlaylistInfo;

export type PlaylistAction =
    | { type: 'ruleTree:add'; entity: Rule; treeNode: TreeNodeData }
    | {
        type: 'ruleTree:addGroup';
        entity: RuleGroup;
        childEntity: Rule;
        treeNode: TreeNodeData;
    }
    | { type: 'ruleTree:remove'; treeNode: TreeNodeData }
    | {
        type: 'ruleTree:changeExpand';
        treeNode: TreeNodeData;
        isExpanded: boolean;
    }
    | { type: 'ruleEntity:update'; entity: Partial<RuleOrRuleGroup>; ruleId: string }
    | {
        type: 'playlist:updateData';
        data: Partial<Playlist>;
    };

export const playlistReducer: React.Reducer<PlaylistState, PlaylistAction> = (state, action) => {
    switch (action.type) {
        case 'ruleTree:add': {
            const newNode = createTreeNodeData({ id: action.entity.id, data: action.entity });

            return {
                ...state,
                rulesTree: {
                    ...addNode(state.rulesTree, action.treeNode, newNode),
                },
            };
        }

        case 'ruleTree:addGroup': {
            const newChildNode = createTreeNodeData({
                id: action.childEntity.id,
                data: action.childEntity,
            });
            const newParentNode = createTreeNodeData({ id: action.entity.id, data: action.entity });

            let treeWithMewNode = {
                ...addNode(state.rulesTree, action.treeNode, newParentNode),
            };
            treeWithMewNode = {
                ...addChildNode(treeWithMewNode, newParentNode, newChildNode),
            };

            return {
                ...state,
                rulesTree: {
                    ...treeWithMewNode,
                },
            };
        }

        case 'ruleTree:remove': {
            return {
                ...state,
                rulesTree: {
                    ...removeNode(state.rulesTree, action.treeNode),
                },
            };
        }

        case 'ruleTree:changeExpand': {
            return {
                ...state,
                rulesTree: {
                    ...changeExpand(state.rulesTree, action.treeNode, action.isExpanded),
                },
            };
        }

        case 'playlist:updateData': {
            return {
                ...state,
                ...action.data,
            };
        }

        case 'ruleEntity:update': {
            const ruleTreeDict = state.rulesTree.byId;
            const nodeId = Object.keys(ruleTreeDict).find(
                x => ruleTreeDict[x].data.id === action.ruleId,
            );
            const node = ruleTreeDict[nodeId];
            return {
                ...state,
                rulesTree: {
                    ...state.rulesTree,
                    byId: {
                        ...state.rulesTree.byId,
                        [node.id]: {
                            ...node,
                            data: {
                                ...node.data,
                                ...action.entity,
                            },
                        },
                    },
                },
            };
        }

        default:
            throw new Error();
    }
};
