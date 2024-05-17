import * as React from 'react';

export const AutoSize: React.FC<{children?:any}> = props => {
    return <div style={{ width: 'auto' }}>{props.children}</div>;
};
