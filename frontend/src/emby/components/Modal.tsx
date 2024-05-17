import * as React from 'react';
import { ToastContainer } from 'react-toastify';
import { Button } from '~/emby/components/Button';
import './Modal.css';

export type ModalProps = {
    title?: string;
    confirmLabel: string;
    confirmDisable?: boolean;
    onClose(): void;
    onConfirm(): void;
    small?: boolean;
    children: any;
};

export const Modal: React.FC<ModalProps> = props => {
    if (props.small === undefined) props.small = false;
    return (
        <>

            <div className="dialogBackdrop dialogBackdropOpened" />
            <div className="dialogContainer">
                <ToastContainer containerId="modalToast" />
                <div
                    className={"focuscontainer dialog " + (props.small ? "dialog-small" : "dialog-fixedSize") + " dialog-medium-tall formDialog opened"}
                    data-lockscroll="true"
                    data-history="true"
                    data-autofocus="true"
                    data-removeonclose="true"
                >
                    <div className="formDialogHeader">
                        <button is="paper-icon-button-light" onClick={_ => props.onClose()}>
                            <i className="md-icon"></i>
                        </button>
                        <h3 className="formDialogHeaderTitle">{props.title}</h3>
                    </div>
                    <div className="formDialogContent scrollY">
                        <div className="dialogContentInner">{props.children}</div>
                        <div className="formDialogFooter">
                            <Button
                                type="submit"
                                onClick={_ => props.onConfirm()}
                                isBlock={true}
                                class="formDialogFooterItem"
                                disabled={props.confirmDisable}
                            >
                                {props.confirmLabel}
                            </Button>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};
