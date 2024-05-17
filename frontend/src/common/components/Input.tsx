import * as React from 'react';
import { useInput } from '~/common/hooks/useInput';

export type InputProps = { label?: string } & React.InputHTMLAttributes<HTMLInputElement> & BaseProps;

export const Input: React.FC<InputProps> = props => {
    const inputValueProps = useInput(props.value.toString());
    const inputType = props.type || 'text';
    return <input type={inputType} {...props} {...inputValueProps} />;
};
