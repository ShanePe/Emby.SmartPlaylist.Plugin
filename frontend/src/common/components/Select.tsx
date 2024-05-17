import * as React from 'react';

export type SelectProps = {
    values: string[];
    value: string;
    label?: string;
    setItemLabel?(item: string): string;
    labelPos?: 'left' | 'top';
    onChange(value: string): void;
} & React.SelectHTMLAttributes<HTMLSelectElement> &
    BaseProps;

export const Select: React.FC<SelectProps> = props => {

    return (
        <select {...props} onChange={e => props.onChange(e.target.value)} value={props.value}>
            {props.values.map(item => (
                <option key={item} value={item}>
                    {props.setItemLabel ? props.setItemLabel(item) : item}
                </option>
            ))}
        </select>
    );
};
