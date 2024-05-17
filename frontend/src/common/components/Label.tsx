import * as React from 'react';

export const Label: React.FC<{children?:any}> = props => {
    return <div style={{ margin: 'auto 0', padding: '0 .25em' }}>{props.children}</div>;
};
