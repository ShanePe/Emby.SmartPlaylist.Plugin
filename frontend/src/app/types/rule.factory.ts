import { Rule, RuleCriteriaValue, RuleGroup } from '~/app/types/rule';
import { Guid } from '~/common/helpers/guid';
import { defaultRuleCriteriaDefinition } from '~/app/app.const';

export const createRuleGroup = (template?: Partial<RuleGroup>): RuleGroup => {
    let newRuleGroup: RuleGroup = {
        id: Guid.newGuid(),
        kind: 'ruleGroup',
        matchMode: 'All',
    };

    newRuleGroup = { ...newRuleGroup, ...template };

    return newRuleGroup;
};

export const createRule = (template?: Partial<Rule>): Rule => {
    let newRule: Rule = {
        id: Guid.newGuid(),
        kind: 'rule',
        criteria: createRuleCriteriaValue(),
    };
    newRule = { ...newRule, ...template };

    return newRule;
};

export const createRuleCriteriaValue = (): RuleCriteriaValue => {
    const criteriaDefinition = defaultRuleCriteriaDefinition;
    return {
        name: criteriaDefinition.name,
        operator: criteriaDefinition.type.operators[0],
        value: criteriaDefinition.type.operators[0].defaultValue,
        userId: criteriaDefinition.userId
    };
};
