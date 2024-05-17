import * as React from 'react';
import { EmbyProps, parseEmbyProps } from '~/emby/components/embyProps';

export const Inline: React.FC<EmbyProps> = props => {

    const embyProps = parseEmbyProps(props);
    return <div style={{ width: '100%', display: 'flex' }} {...embyProps}>{props.children}</div>;
};
