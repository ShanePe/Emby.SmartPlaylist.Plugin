import * as React from 'react';
import { Select as SelectBase, SelectProps } from '~/common/components/Select';
import './Select.css';
import './Base.css';
import { EmbyProps, parseEmbyProps } from '~/emby/components/embyProps';

type EmbySelectProps = SelectProps & EmbyProps;

export const Select: React.FC<EmbySelectProps> = props => {
    let wrapperClass = 'selectContainer padding-lr';
    if (!props.label) {
        wrapperClass += ' inline';
    }
    if (props.maxWidth) {
        wrapperClass += ' max-width';
    }
    return (
        <div className={wrapperClass}>
            <SelectBase is="emby-select" {...parseEmbyProps<EmbySelectProps>(props)} />
        </div>
    );
};
