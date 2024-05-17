import * as React from 'react';

export type IconProps = {
    type: 'add' | 'remove' | 'group' | 'collapsed' | 'expanded' | 'checklist' | 'account_circle' | 'library_add' | 'psychology_alt' | 'help';
};

export const Icon: React.FC<IconProps> = props => {
    return (
        <>
            {props.type === 'add' && '+'}
            {props.type === 'remove' && '-'}
            {props.type === 'group' && '#'}
            {props.type === 'expanded' && 'V'}
            {props.type === 'collapsed' && '>'}
            {props.type === 'checklist' && 'checklist'}
            {props.type === 'account_circle' && 'account_circle'}
            {props.type === 'library_add' && 'library_add'}
            {props.type === 'psychology_alt' && 'psychology_alt'}
            {props.type === 'help' && 'help'}
        </>
    );
};
