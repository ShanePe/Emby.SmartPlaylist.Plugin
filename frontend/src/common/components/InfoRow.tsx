import * as React from 'react';
import { EmbyProps } from '~/emby/components/embyProps';
import { InfoLabel } from './InfoLabel';
import { Inline } from './Inline';

export type InfoItem = {
    label: string,
    text: string,
    visible: boolean
}

export type InfoRowProps = {
    InfoItems: InfoItem[]
} & React.AllHTMLAttributes<HTMLDivElement> & BaseProps & EmbyProps;


export const InfoRow: React.FC<InfoRowProps> = props => {
    return (
        <Inline {...props}>
            {props.InfoItems.map((i, key) =>
                <InfoLabel
                    key={key}
                    label={i.label}
                    text={i.text}
                    visible={i.visible}
                />
            )}
        </Inline>
    );
};