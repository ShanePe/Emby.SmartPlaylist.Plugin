import * as React from 'react';
import Popup from 'reactjs-popup';
import { MenuItem, MenuItemToggle } from '~/app/components/Menu';
import { Inline } from './Inline';
import { Input, InputProps } from './Input';
import './MenuInput.css';
import { Toggle } from './Toggle';
import '../../app/components/Menu.css'

type MenuTypeInputProps = {
    menuItems: (MenuItem | MenuItemToggle)[]
} & InputProps

export const MenuInput: React.FC<MenuTypeInputProps> = props => {
    function isItem(item: MenuItem | MenuItemToggle): item is MenuItem {
        return (item as MenuItem).onClick !== undefined;
    }

    function isToggle(item: MenuItem | MenuItemToggle): item is MenuItemToggle {
        return (item as MenuItemToggle).onToggle !== undefined;
    }

    const renderMenuItem = (m: MenuItem | MenuItemToggle, i: number) => {
        if (isItem(m)) {
            return (
                <div
                    className='menu-item'
                    key={i}
                    onClick={() => m.onClick()} >
                    {m.icon && (
                        <i className="md-icon button-icon button-icon-left">{m.icon}</i>
                    )}
                    <span>{m.label}</span>
                </div>
            )
        }
        else if (isToggle(m)) {
            return (
                <div
                    className='menu-item'
                    key={i}>
                    <Toggle
                        id={'toggle_menu_' + i}
                        onToggled={(check) => m.onToggle(check)}
                        label={m.label}
                        labelPos={'right'}
                        checked={m.value}
                    />
                </div>
            )
        }
    }

    return (
        <Inline>
            <div className='row-container'>
                <Input {...props} />
                <Popup
                    trigger={<div className='menu-container-input' {...props}>
                        <i className="md-icon">pending</i>
                    </div>}
                    position='top right'
                    on='hover'
                    closeOnDocumentClick
                    mouseLeaveDelay={300}
                    mouseEnterDelay={0}
                    contentStyle={{ backgroundColor: "var(--theme-background)", zIndex: 9999999, 'borderRadius': '5px', padding: '10px', boxShadow: '0px 0px 10px rgba(169, 169, 169, 0.6)' }}
                    arrow={false}
                >
                    {props.menuItems.map((m, i) => renderMenuItem(m, i))}
                </Popup>
            </div>
        </Inline>
    )
}