import * as React from 'react';
import { EmbyProps } from "~/emby/components/embyProps";

export type InfoLabelProps = {
    key?: number,
    label: string,
    text: string,
    visible: boolean
} & React.AllHTMLAttributes<HTMLDivElement> & BaseProps & EmbyProps;

export const InfoLabel: React.FC<InfoLabelProps> = props => {
    return props.visible && (
        <div {...props} className='info-row' key={props.key}>
            <div className='info-row-label'>{props.label}</div>
            <div className='info-row-text'>{props.text}</div>
        </div>
    );
};