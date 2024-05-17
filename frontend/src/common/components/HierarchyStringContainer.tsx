import * as React from 'react';
import { HierarchyString } from '~/app/types/appData';
import './HierarchyStringContainer.css';

export type HierarchyStringProps = {
    value: HierarchyString
}

export const HierarchyStringContainer: React.FC<HierarchyStringProps> = props => {


    const getContainer = (hs: HierarchyString): React.ReactElement => {
        return (
            <>
                <div className='hier-str' style={{ paddingLeft: (hs.level * 10) + 'px' }}>
                    {hs.value}
                </div>

                {hs.children.map((x) => getContainer(x))}
            </>
        )
    }

    return getContainer(props.value);
};
