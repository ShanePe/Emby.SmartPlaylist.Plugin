import * as React from 'react';
import { ListValue, ListValueRange } from '~/app/types/rule';
import { Select } from '~/common/components/Select';
import { Label } from '~/common/components/Label';

type ListValueRangeInputProps = {
    value: ListValueRange;
    values: ListValue[];
    onChange(value: ListValueRange): void;
};

export const ListValueRangeInput: React.FC<ListValueRangeInputProps> = props => {
    const { onChange } = props;
    const values = props.values.map(x => x.value);
    const listValue = props.value;

    return (
        <>
            <Select
                values={values}
                value={listValue.from.value}
                onChange={newVal =>
                    onChange({

                        ...listValue,
                        from: props.values.find(x => x.value === newVal),
                    })
                }
            />
            <Label>to:</Label>
            <Select
                values={values}
                value={listValue.to.value}
                onChange={newVal =>
                    onChange({
                        ...listValue,
                        to: props.values.find(x => x.value === newVal),
                    })
                }
            />
        </>
    );
};
