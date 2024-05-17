import * as React from 'react';
import { ListMapValue } from '~/app/types/rule';
import { Select } from '~/common/components/Select';

type MapValueInputProps = {
    value: ListMapValue;
    values: ListMapValue[];
    onChange(value: ListMapValue): void;
};

export const ListMapValueInput: React.FC<MapValueInputProps> = props => {
    const { onChange } = props;
    const values = props.values.map(x => x.value);
    const value = props.value.value;
    const listMapValue = props.value;

    return (
        <Select
            values={values}
            value={value}
            onChange={newVal =>
                onChange({
                    ...listMapValue,
                    ...props.values.find(x => x.value === newVal),
                    value: newVal,
                })
            }
        />
    );
};
