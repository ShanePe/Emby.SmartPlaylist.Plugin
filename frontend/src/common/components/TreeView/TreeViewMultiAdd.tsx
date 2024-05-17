import * as React from 'react';
import { Modal } from '~/emby/components/Modal';
import { AppContext } from '~/app/state/app.context';
import { Select } from '~/common/components/Select';
import { CriteriaValue, ListMapValue, ListValue, Period, Rule } from '../../../app/types/rule';
import { createRule } from '../../../app/types/rule.factory';
import { Inline } from '~/common/components/Inline';
import { UserListInput } from '../../../app/components/RuleEditor/ValueInputs/UserList';
import './TreeViewMultiAdd.css'
import { tryParseDate } from '~/common/helpers/date';
import { Guid } from '~/common/helpers/guid';
import { tryParseInt, showError } from '~/common/helpers/utils';

type PlaylistEditorMultiAddProps = {
    onClose(): void;
    onConfirm(rules: Rule[]): void;
};

export const TreeViewMultiAdd: React.FC<PlaylistEditorMultiAddProps> = (props) => {
    const appContext = React.useContext(AppContext);
    let [rule, updateRule] = React.useState(createRule())
    let [values, updateValues] = React.useState("");

    const criteriaDefs = appContext.getRulesCriteriaDefinitions();
    const criteriaNames = criteriaDefs.map(x => x.name);
    const operators = appContext.getRuleCriteriaOperators(rule.criteria.name);
    const criteriaDef = appContext.getRulesCriteriaDefinition(rule.criteria.name);

    const getPipedValues = (value: string, expected: Number): string[] => {
        let splitValues = value.split('|');
        if (splitValues.length != expected)
            throw Error('Value: ' + value + ' not pipe separated for multiple values.')
        return splitValues;
    }

    const getCriteriaValue = (rule: Rule, newValue: string): CriteriaValue => {
        switch (rule.criteria.value.kind) {
            case 'date':
                return {
                    ...rule.criteria.value,
                    value: tryParseDate(newValue)
                };
            case 'dateRange':
                let dateValues = getPipedValues(newValue, 2);
                return {
                    ...rule.criteria.value,
                    from: tryParseDate(dateValues[0]),
                    to: tryParseDate(dateValues[1])
                };
            case 'empty':
                return {
                    ...rule.criteria.value,
                    value: 'empty'
                };
            case 'lastPeriod':
                let lastPeriodValues = getPipedValues(newValue, 2);
                return {
                    ...rule.criteria.value,
                    value: tryParseInt(lastPeriodValues[0]),
                    periodType: lastPeriodValues[1] as Period
                }
            case 'listMapValue':
                var listValuesMapCriteria = appContext.getRulesCriteriaDefinition(rule.criteria.name);
                var listMapValue = listValuesMapCriteria.values.find(x => (x as ListMapValue).value.toLowerCase() === newValue.toLowerCase());
                if (listMapValue === undefined)
                    throw Error(newValue + ' is not a valid value for list. Available values are: [' +
                        listValuesMapCriteria.values.map(x => (x as ListMapValue).value).join(", ") + "]");
                return {
                    ...rule.criteria.value,
                    ...listMapValue
                }
            case 'listValue':
                var listValuesCriteria = appContext.getRulesCriteriaDefinition(rule.criteria.name);
                var listValue = listValuesCriteria.values.find(x => (x as ListValue).value.toLowerCase() === newValue.toLowerCase());
                if (listValue === undefined)
                    throw Error(newValue + ' is not a valid value for list. Available values are: [' +
                        listValuesCriteria.values.map(x => (x as ListValue).value).join(", ") + "]");
                return {
                    ...rule.criteria.value,
                    ...listValue
                }
            case 'number':
                return {
                    ...rule.criteria.value,
                    value: tryParseInt(newValue)
                }
            case 'numberRange':
                let numberRangeValues = getPipedValues(newValue, 2);
                return {
                    ...rule.criteria.value,
                    from: tryParseInt(numberRangeValues[0]),
                    to: tryParseInt(numberRangeValues[1])
                }
            case 'string':
                return {
                    ...rule.criteria.value,
                    value: newValue
                }
            default:
                throw Error('Invalid value type found.')
        }
    }

    return (
        <Modal
            onClose={() => props.onClose()}
            onConfirm={() => {
                try {
                    var rules: Rule[] = []
                    if (values === undefined || values === '')
                        throw Error('Not values supplied.');

                    for (let item of [...new Set(values.split(','))]) {
                        rules.push({
                            ...rule,
                            criteria: {
                                ...rule.criteria,
                                value: getCriteriaValue(rule, item.trim())
                            },
                            id: Guid.newGuid()
                        })
                    }

                    updateRule(createRule());
                    updateValues('');

                    props.onConfirm(rules);

                } catch (e) {
                    var msg = e instanceof Error ? e.message : e;

                    showError({ label: 'Error adding multiple values', content: e, timeout: 5000, modal: true });
                }

            }
            }
            confirmLabel='Add'
            title='Add multiple values'
            small={true}
        >
            <div className='main-container'>
                <Inline>
                    <Select
                        values={criteriaNames}
                        value={rule.criteria.name}
                        label="Criteria"
                        maxWidth={true}
                        onChange={newVal => {

                            const operator = appContext.getRuleCriteriaOperators(newVal)[0];
                            const newCritDef = appContext.getRulesCriteriaDefinition(newVal);

                            updateRule({
                                ...rule,
                                criteria: {
                                    ...rule.criteria,
                                    name: newCritDef.name,
                                    operator: operator,
                                    value: operator.defaultValue
                                }
                            });
                        }}
                    />
                </Inline>
                <Inline>
                    <Select
                        values={operators.map(x => x.name)}
                        value={rule.criteria.operator.name}
                        label='Operator'
                        maxWidth={true}
                        onChange={newVal => {
                            const operator = appContext.getRuleCriteriaOperators(rule.criteria.name).find(x => x.name === newVal);

                            updateRule({
                                ...rule,
                                criteria: {
                                    ...rule.criteria,
                                    operator: operator,
                                    value: operator.defaultValue
                                }
                            })
                        }
                        }
                    />
                </Inline>
                {criteriaDef.isUserSpecific && (
                    <>
                        <Inline>
                            <UserListInput
                                userId={rule.criteria.userId}
                                label='In context of User'
                                maxWidth={true}
                                onChange={newVal => updateRule({
                                    ...rule,
                                    criteria: {
                                        ...rule.criteria,
                                        userId: newVal
                                    }
                                })}
                            />
                        </Inline>
                    </>
                )}
                <Inline>
                    <div className='text-container selectContainer padding-lr max-width'>
                        <label htmlFor='txt_values' className='selectLabel selectLabelText'>Comma separated value list: (e.g. Value 1,Value 2,Value 3 etc., Ranges and multi-value fields must be separated by a pipe e.g. 0|1, 2|10, 3|7 etc. )
                        </label>
                        <div>
                            <textarea
                                id='txt_values'
                                onBlur={e => {
                                    updateValues(e.target.value)
                                }}
                            />
                        </div>
                    </div>
                </Inline>
            </div>
        </Modal >
    )
}


