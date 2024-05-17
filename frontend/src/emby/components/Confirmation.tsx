import * as React from 'react';
import { Button } from '~/emby/components/Button';
import './Modal.css';
import './Confirmation.css'
import { CheckBox } from './CheckBox';
import { Playlist } from '~/app/types/playlist';

export type ConfirmationProps = {
    title?: string;
    question: string;
    data?: any;
    control?: React.FC<ControlProps>;
    onYes(data: any): void;
    onNo(data: any): void;
    children?:any
};

type ControlProps = {
    raiseStateChanged(d: any);
}

export const Confirmation: React.FC<ConfirmationProps> = props => {
    const [state, setData] = React.useState(props.data);

    const inner = React.createElement(props.control, {
        raiseStateChanged: (d) => {
            setData({ ...state, ...d });
        }
    });

    return (
        <>
            <div className="dialogBackdrop dialogBackdropOpened" />
            <div className="dialogContainer">
                <div
                    className="focuscontainer dialog formDialog opened"
                    data-lockscroll="true"
                    data-history="true"
                    data-autofocus="true"
                    data-removeonclose="true"
                >
                    <div className="formDialogHeader">
                        <h3 className="formDialogHeaderTitle">{props.title}</h3>
                    </div>
                    <div className="formDialogContent scrollY">
                        <div className="confirm-text"><span>{props.question}</span></div>
                        {inner}
                        <div className="confirm-footer">
                            <Button
                                type="submit"
                                onClick={_ => props.onYes(state)}
                                class="formDialogFooterItem"
                            >
                                Yes
                            </Button>
                            <Button
                                type="submit"
                                onClick={_ => props.onNo(state)}
                                class="formDialogFooterItem"
                            >No
                            </Button>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export type DeleteData = { playlist: Playlist, keep: boolean };
export const ConfirmDeletePlaylist: React.FC<ControlProps> = props => {
    return (
        <div className='deletePlaylist'>
            <CheckBox
                label='Keep generated Playlist/Collection ?'
                onChange={(e) => {
                    props.raiseStateChanged({ keep: e.target.checked });
                }}
            />
        </div>
    )
}
