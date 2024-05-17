import * as React from 'react';
import { Inline } from '~/common/components/Inline';
import { Toggle } from '~/common/components/Toggle';
import { Modal } from '~/emby/components/Modal';
import { PlaylistBasicData } from '../types/playlist';

export type ExportProps = {
    playlists: PlaylistBasicData[];
    onClose(): void;
    onConfirm(ids: string[]): void;
} & React.HtmlHTMLAttributes<HTMLDivElement>

export const Export: React.FC<ExportProps> = props => {
    const [ids, setIds] = React.useState([]);
    React.useEffect(() => {
        setIds(props.playlists.map(x => x.id))
    }, []);

    return (
        <Modal
            onClose={() => props.onClose()}
            onConfirm={() => props.onConfirm(ids)}
            title="Export playlists"
            small={true}
            confirmLabel='Export'
            confirmDisable={ids.length === 0}
        >
            <div style={{ 'paddingTop': '20px' }}>
                Please the playlists you'd like to export.
            </div>
            <ul>
                {props.playlists.map(x =>
                    <li>
                        <Toggle
                            id={x.id}
                            checked={ids.includes(x.id)}
                            label={x.name}
                            labelPos='right'
                            onToggled={(val) => {
                                if (val && !ids.includes(x.id)) {
                                    setIds((preIds) => [
                                        ...preIds,
                                        x.id
                                    ])
                                }
                                else if (!val && ids.includes(x.id)) {
                                    setIds(ids.filter(i => i !== x.id))
                                }
                            }}
                        />
                    </li>
                )}
            </ul>
        </Modal>
    )
}