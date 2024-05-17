import * as React from 'react';
import {
    CriteriaValue,
    GroupMatchMode,
    Rule,
    RuleCriteriaOperator,
    RuleCriteriaValue,
    RuleGroup,
} from '~/app/types/rule';
import { PlaylistAction, PlaylistState } from '~/app/state/playlist/playlist.reducer';
import { isShuffleUpdateType, PlaylistBasicData } from '~/app/types/playlist';
import { nameLimitOrderBy, randomLimitOrderBy } from '~/app/app.const';
import { AppContextProps } from '~/app/state/app.context';

export type PlaylistActions = {
    changeMatchMode(ruleGroup: RuleGroup, matchMode: GroupMatchMode): void;
    updateBasicData(playlistBasicData: Partial<PlaylistBasicData>): void;
    changeCriteriaDef(rule: Rule, criteriaName: string): void;
    changeOperator(rule: Rule, operatorName: string): void;
    updateCriteriaValue(rule: Rule, value: CriteriaValue): void;
    updateCriteriaUser(rule: Rule, userId: string): void;
};

export const createPlaylistActions = (
    dispatcher: React.Dispatch<PlaylistAction>,
    state: PlaylistState,
    appContext: AppContextProps,
): PlaylistActions => {
    return {
        updateCriteriaUser: (rule: Rule, userId: string): void => {
            updateCriteriaState(
                dispatcher,
                {
                    userId: userId,
                },
                rule,
            );
        },
        updateCriteriaValue: (rule: Rule, value: CriteriaValue): void => {
            updateCriteriaState(
                dispatcher,
                {
                    value: value,
                },
                rule,
            );
        },
        changeOperator: (rule: Rule, operatorName: string): void => {
            const newOperator = appContext
                .getRuleCriteriaOperators(rule.criteria.name)
                .find(x => x.name === operatorName);

            updateCriteriaState(
                dispatcher,
                {
                    operator: newOperator,
                    value: getOperatorValue(newOperator, rule.criteria),
                },
                rule,
            );
        },
        changeCriteriaDef: (rule: Rule, criteriaName: string): void => {
            const criteria = rule.criteria;
            const operator = appContext.getRuleCriteriaOperators(criteriaName)[0];
            const newCriteriaDef = appContext.getRulesCriteriaDefinition(criteriaName);
            const criteriaDef = appContext.getRulesCriteriaDefinition(rule.criteria.name);

            const valuesAreValid =
                newCriteriaDef.values.length === 0 ||
                (newCriteriaDef.values.length > 0 &&
                    newCriteriaDef.values.find(x => x === criteria.value));

            if (newCriteriaDef.type.name === criteriaDef.type.name && valuesAreValid) {
                updateCriteriaState(
                    dispatcher,
                    {
                        name: criteriaName,
                    },
                    rule,
                );
            } else {
                updateCriteriaState(
                    dispatcher,
                    {
                        name: criteriaName,
                        operator: operator,
                        value: valuesAreValid
                            ? getOperatorValue(operator, criteria)
                            : operator.defaultValue,
                    },
                    rule,
                );
            }
        },
        changeMatchMode: (ruleGroup: RuleGroup, matchMode: GroupMatchMode) => {
            dispatcher({
                type: 'ruleEntity:update',
                entity: {
                    matchMode: matchMode,
                },
                ruleId: ruleGroup.id,
            });
        },
        updateBasicData: (playlistBasicData: Partial<PlaylistBasicData>) => {
            if (playlistBasicData.updateType) {
                if (isShuffleUpdateType(playlistBasicData.updateType)) {
                    playlistBasicData.limit = {
                        ...state.limit,
                        orderBy: randomLimitOrderBy,
                    };
                } else if (isShuffleUpdateType(state.updateType)) {
                    playlistBasicData.limit = {
                        ...state.limit,
                        orderBy: nameLimitOrderBy,
                    };
                }
            }
            dispatcher({
                type: 'playlist:updateData',
                data: playlistBasicData,
            });
        },
    };
};

const updateCriteriaState = (
    dispatcher: React.Dispatch<PlaylistAction>,
    criteria: Partial<RuleCriteriaValue>,
    rule: Rule,
) => {
    dispatcher({
        type: 'ruleEntity:update',
        entity: {
            criteria: {
                ...rule.criteria,
                ...criteria,
            },
        },
        ruleId: rule.id,
    });
};

const getOperatorValue = (operator: RuleCriteriaOperator, criteria: RuleCriteriaValue) => {
    const isSameValueType = operator.type === criteria.operator.type;
    return isSameValueType ? criteria.value : operator.defaultValue;
};
