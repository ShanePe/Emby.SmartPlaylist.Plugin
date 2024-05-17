import * as React from 'react';
import Popup from 'reactjs-popup';
import './Menu.css';
import { PopupPosition } from 'reactjs-popup/dist/types';

export type MenuItem = {
    label: string;
    icon?: string;
    hidden?: boolean;
    onClick(): void;
}

export type MenuItemToggle = {
    label: string,
    onToggle(check: boolean): void;
    value: boolean
}

export type MenuProps = {
    menuItems: MenuItem[]
    open?: boolean;
    position?: PopupPosition;
} & React.HtmlHTMLAttributes<HTMLDivElement>

export const Menu: React.FC<MenuProps> = props => {
    const pos = props.position ? props.position : "left top";
    return (
        <Popup
            trigger={<div className='menu-container' {...props}>
                <i className="md-icon">pending</i>
            </div>}
            position={pos}
            open={props.open}
            on='hover'
            closeOnDocumentClick
            mouseLeaveDelay={300}
            mouseEnterDelay={0}
            contentStyle={{ padding: '0px', border: 'none' }}
            arrow={false}
        >
            <div className='menu'>
                {props.menuItems.map((m, i) => {
                    if (m.hidden) {
                        return ''
                    }
                    else {
                        return <div
                            className='menu-item'
                            key={i}
                            onClick={() => m.onClick()} >
                            {m.icon && (
                                <i className="md-icon button-icon button-icon-left">{m.icon}</i>
                            )}
                            <span>{m.label}</span>
                        </div>
                    }
                })}
            </div>
        </Popup>
    )
}