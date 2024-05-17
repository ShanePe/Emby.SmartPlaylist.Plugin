import * as React from 'react';
import { EmbyProps } from '~/emby/components/embyProps';
import './TagList.css';

export type TagListProps = {
    Items: string[]
} & React.AllHTMLAttributes<HTMLDivElement> & BaseProps & EmbyProps;


export const TagList: React.FC<TagListProps> = props => {
    return (
        <div className='tag-list'>
            {props.Items.map(x =>
                <div className='raised emby-button tag-item'>{x}</div>
            )}
        </div>
    )
};