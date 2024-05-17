import { PeriodValues, RuleMatchTypes as GroupMatchModes } from '~/app/app.const';

export type Rule = {
    kind: 'rule';
    criteria: RuleCriteriaValue;
    id: string;
};

export type RuleGroup = {
    kind: 'ruleGroup';
    id: string;
    matchMode: GroupMatchMode;
};

export type RuleOrRuleGroup = Rule | RuleGroup;

export type CriteriaValue =
    | StringValue
    | DateValue
    | LastPeriodValue
    | DateRangeValue
    | ListValue
    | ListMapValue
    | NumberValue
    | NumberRangeValue
    | ListValueRange
    | EmptyValue
    | RegexValue;

export type RuleCriteriaValue = {
    name: string;
    operator: RuleCriteriaOperator;
    value: CriteriaValue;
    userId: string;
};

export type RuleCriteriaDefinition = {
    name: string;
    type: RuleCriteriaDefinitionType;
    values: CriteriaValue[];
    isUserSpecific: boolean;
    userId?: string;
};

export type RuleCriteriaOperator = {
    name: string;
    type: OperatorType;
    defaultValue: CriteriaValue;
};

export type CriteriaType = 'string' | 'date' | 'number' | 'listValue' | 'bool' | 'empty' | 'regex';
export type OperatorType =
    | 'string'
    | 'date'
    | 'lastPeriod'
    | 'dateRange'
    | 'listValue'
    | 'listMapValue'
    | 'number'
    | 'numberRange'
    | 'listValueRange'
    | 'bool'
    | 'empty'
    | 'regex';

export type RuleCriteriaDefinitionType = {
    name: CriteriaType;
    operators: RuleCriteriaOperator[];
};

export type StringValue = {
    kind: 'string';
    value: string;
};

export type RegexValue = {
    kind: 'regex',
    value: string,
    caseSensitive: boolean
}

export type NumberValue = {
    kind: 'number';
    value: number;
};

export type DateValue = {
    kind: 'date';
    value: Date;
};

export type LastPeriodValue = {
    kind: 'lastPeriod';
    value: number;
    periodType: Period;
};

export type DateRangeValue = {
    kind: 'dateRange';
    from: Date;
    to: Date;
};

export type NumberRangeValue = {
    kind: 'numberRange';
    from: number;
    to: number;
};

export type ListValue = {
    kind: 'listValue';
    value: string;
    numValue: number;
};

export type ListMapValue = {
    kind: 'listMapValue';
    value: string;
    map: string;
    numValue: number;
};

export type BoolValue = {
    kind: 'bool';
    value: boolean;
};

export type EmptyValue = {
    kind: 'empty';
    value: any
}

export type ListValueRange = {
    kind: 'listValueRange';
    from: ListValue;
    to: ListValue;
};

export type Period = typeof PeriodValues[number];
export type GroupMatchMode = typeof GroupMatchModes[number];
