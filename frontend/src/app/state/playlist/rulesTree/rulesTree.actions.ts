import * as React from 'react';
import { PlaylistAction } from '../playlist.reducer';
import { Rule, RuleOrRuleGroup } from '~/app/types/rule';
import { createRule, createRuleGroup } from '~/app/types/rule.factory';
import { TreeNodeData } from '~/common/components/TreeView/types/tree';

export type RulesTreeActions = {
    addRule(node: TreeNodeData): void;
    addRuleEntity(node: TreeNodeData, entity: Rule): void;
    addRuleGroup(node: TreeNodeData): void;
    removeRule(node: TreeNodeData): void;
    changeExpand(node: TreeNodeData<RuleOrRuleGroup>, isExpanded: boolean): void;
};

export const createRulesTreeActions = (
    dispatcher: React.Dispatch<PlaylistAction>,
): RulesTreeActions => {
    return {
        addRule: (node: TreeNodeData) => {
            dispatcher({
                type: 'ruleTree:add',
                entity: createRule(),
                treeNode: node,
            });
        },
        addRuleEntity: (node: TreeNodeData, entity: Rule) => {
            dispatcher({
                type: 'ruleTree:add',
                entity: entity,
                treeNode: node,
            });
        },
        addRuleGroup: (node: TreeNodeData) => {
            dispatcher({
                type: 'ruleTree:addGroup',
                entity: createRuleGroup(),
                childEntity: createRule(),
                treeNode: node,
            });
        },
        removeRule: (node: TreeNodeData) => {
            dispatcher({
                type: 'ruleTree:remove',
                treeNode: node,
            });
        },
        changeExpand: (node: TreeNodeData<RuleOrRuleGroup>, isExpanded: boolean) => {
            dispatcher({
                type: 'ruleTree:changeExpand',
                treeNode: node,
                isExpanded: isExpanded,
            });
        },
    };
};
