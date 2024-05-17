import * as React from 'react';
import {
    CriteriaValue,
    DateRangeValue,
    DateValue,
    LastPeriodValue,
    ListMapValue,
    ListValue,
    ListValueRange,
    NumberRangeValue,
    NumberValue,
    OperatorType,
    RegexValue,
    StringValue,
} from '~/app/types/rule';
import { SingleDate } from '~/app/components/RuleEditor/ValueInputs/SingleDate';
import { LastPeriod } from '~/app/components/RuleEditor/ValueInputs/LastPeriod';
import { DateRange } from '~/app/components/RuleEditor/ValueInputs/DateRange';
import { StringValueInput } from '~/app/components/RuleEditor/ValueInputs/String';
import { ListValueInput } from '~/app/components/RuleEditor/ValueInputs/ListValue';
import { NumberValueInput } from '~/app/components/RuleEditor/ValueInputs/NumberValue';
import { NumberRangeValueInput } from '~/app/components/RuleEditor/ValueInputs/NumberRange';
import { ListValueRangeInput } from '~/app/components/RuleEditor/ValueInputs/ListValueRange';
import { ListMapValueInput } from '~/app/components/RuleEditor/ValueInputs/ListMapValue';
import { RegexValueInput } from './ValueInputs/Regex';

type ValueInputProps = {
    type: OperatorType;
    value: CriteriaValue;
    values: CriteriaValue[];
    onChange(value: CriteriaValue): void;
};

export const ValueInput: React.FC<ValueInputProps> = props => {
    const { value, type, onChange, values } = props;

    const renderValueInput = () => {

        switch (type) {
            case 'string':
                return (
                    <StringValueInput
                        value={value as StringValue}
                        onChange={newVal => onChange(newVal)}
                    />
                );
            case 'date':
                return (
                    <SingleDate value={value as DateValue} onChange={newVal => onChange(newVal)} />
                );
            case 'lastPeriod':
                return (
                    <LastPeriod
                        value={value as LastPeriodValue}
                        onChange={newVal => onChange(newVal)}
                    />
                );
            case 'dateRange':
                return (
                    <DateRange
                        value={value as DateRangeValue}
                        onChange={newVal => onChange(newVal)}
                    />
                );
            case 'listValue':
                return (
                    <ListValueInput
                        value={value as ListValue}
                        values={values as ListValue[]}
                        onChange={newVal => onChange(newVal)}
                    />
                );
            case 'listMapValue':
                return (
                    <ListMapValueInput
                        value={value as ListMapValue}
                        values={values as ListMapValue[]}
                        onChange={newVal => onChange(newVal)}
                    />
                );
            case 'number':
                return (
                    <NumberValueInput
                        value={value as NumberValue}
                        onChange={newVal => onChange(newVal)}
                    />
                );
            case 'numberRange':
                return (
                    <NumberRangeValueInput
                        value={value as NumberRangeValue}
                        onChange={newVal => onChange(newVal)}
                    />
                );
            case 'listValueRange':
                return (
                    <ListValueRangeInput
                        value={value as ListValueRange}
                        values={values as ListValue[]}
                        onChange={newVal => onChange(newVal)}
                    />
                );
            case 'regex':
                return (
                    <RegexValueInput
                        value={value as RegexValue}
                        onChange={newVal => onChange(newVal)}
                    />
                )
            default:
                return <></>;
        }

    };

    return <>{renderValueInput()}</>;
};
