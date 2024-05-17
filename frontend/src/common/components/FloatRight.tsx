import * as React from 'react';
export const FloatRight: React.FC<{children?:any}> = props => {
    return <div style={{ marginLeft: 'auto', display: 'flex' }}>{props.children}</div>;
};
