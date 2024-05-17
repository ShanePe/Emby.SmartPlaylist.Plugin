import * as React from 'react';
import { getIconsForPlayList, Playlist } from '~/app/types/playlist';
import { Toggle } from './Toggle';
import { Menu } from '~/app/components/Menu';
import { dismissToast, loadLog, showHoverToast } from '~/common/helpers/utils';


export type ListItemProps = {
    onEditClick(): void;
    onDuplicateClick(): void;
    onSortJobClick(): void;
    onDeleteClick(): void;
    onViewClick(): void;
    onExecuteClick(): void;
    onUpdateDataClick(playlist: Partial<Playlist>): void;
    onOpenClick(): void;
    playList: Playlist;
} & React.AllHTMLAttributes<HTMLDivElement> &
    BaseProps;

export const ListItem: React.FC<ListItemProps> = props => {
    const mainStyle = "listItem listItem-border emby-button plist-row" + (props.playList.enabled ? "" : " plist-row-disabled");
    const sub = props.playList.sourceType + (props.playList.sourceType === "Media Items" ? "" : " (" + props.playList.source.name + ")");

    const notes = !props.playList.notes || props.playList.notes === '' ? '' : <div dangerouslySetInnerHTML={{ __html: props.playList.notes }} />

    return (
        <div className={mainStyle} data-ripple="false"
            onMouseEnter={() => showHoverToast(notes)}
            onMouseLeave={() => dismissToast()}
        >
            <div className="plist-icon-container">
                <span title={props.playList.smartType + ' ' + props.playList.updateType} className="plist-icon md-icon listItemIcon-transparent">{getIconsForPlayList(props.playList)}</span>
            </div>
            <div className="listItemBody">
                <div className="listItemBodyText" onClick={() => props.onEditClick()}>{props.playList.name}</div>
                <div className="listItemBodyTextSub" onClick={() => props.onEditClick()}>{sub}</div>
            </div>
            <div className='popper'>
                <span className={`tooltiptext`}>
                    <Toggle title='Enabled' id={'toggle-' + props.playList.id} checked={props.playList.enabled} onToggled={(checked: boolean) => {
                        props.onUpdateDataClick({
                            enabled: checked
                        });
                    }
                    } />
                </span>
                <button type="button" disabled={props.playList.updateType === 'Live' ? true : undefined} title={props.playList.updateType === 'Live' ? "Cannot execute 'Live' update types" : 'Execute'} is="paper-icon-button-light" className="paper-icon-button-light icon-button-conditionalfocuscolor" onClick={() => props.onExecuteClick()}><i className="md-icon sp-icon">play_arrow</i></button>
                <button type="button" title='Edit' is="paper-icon-button-light" className="paper-icon-button-light icon-button-conditionalfocuscolor" onClick={() => props.onEditClick()}><i className="md-icon sp-icon">edit</i></button>
                <button type="button" title='Delete' is="paper-icon-button-light" className="paper-icon-button-light icon-button-conditionalfocuscolor" onClick={() => props.onDeleteClick()}><i className="md-icon sp-icon">delete</i></button>
            </div>
            <div className='playlist-menu'>
                <Menu
                    open={false}
                    position='left center'
                    menuItems={[
                        { label: 'Details', icon: 'quick_reference', onClick: () => props.onViewClick() },
                        { label: 'Duplicate', icon: 'content_copy', onClick: () => props.onDuplicateClick() },
                        { label: 'Sort Job', icon: 'sort_by_alpha', onClick: () => props.onSortJobClick(), hidden: props.playList.smartType !== "Playlist" },
                        { label: 'Show Log', icon: 'grading', onClick: () => loadLog(props.playList.id) },
                        { label: 'Open', icon: 'open_in_new', onClick: () => props.onOpenClick() }

                    ]}
                />
            </div>
        </div>
    )

}