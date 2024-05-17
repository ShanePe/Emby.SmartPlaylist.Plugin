import * as React from 'react';
import { Inline } from '~/common/components/Inline';
import { showError } from '~/common/helpers/utils';
import { importFile } from '~/emby/app.data';
import { Modal } from '~/emby/components/Modal';
import { ServerResponse } from '../types/playlist';
import Dropzone from 'react-dropzone';
import './Import.css';
import { BeatLoader } from 'react-spinners';

export type ImportProps = {
    onClose(): void;
    onConfirm(response: ServerResponse<string>): void;
} & React.HtmlHTMLAttributes<HTMLDivElement>

export const Import: React.FC<ImportProps> = props => {
    const [uploadFiles, setFileUploads] = React.useState([]);
    const [isUploading, setIsUploading] = React.useState(false);

    React.useEffect(() => {
        if (isUploading && uploadFiles.length > 0) {
            importFile({
                uploadFile: uploadFiles[0],
                type: 'import'
            }).then((response) => {
                props.onConfirm(response);
            }).catch((e) => {
                props.onConfirm({
                    success: false,
                    error: e instanceof Error ? e.message : (e as string),
                    response: ''
                })
            }).finally(() => {

                setIsUploading(false);
                setFileUploads([])
            });
        }
    }, [isUploading])


    return (
        <Modal
            onClose={() => props.onClose()}
            onConfirm={() => setIsUploading(true)}
            title="Import playlists"
            small={true}
            confirmLabel='Import'
            confirmDisable={uploadFiles.length === 0}
        >
            {isUploading && (
                <>
                    <div className='import-loader'>
                        <div>Uploading ...</div>
                        <div>
                            <BeatLoader loading={true} color='green' />
                        </div>
                    </div>
                </>
            )}

            {!isUploading && (
                <>
                    <Inline>
                        <div className='import-head'>NOTE:!! When uploading, if the playlist already exists, it will be overwritten.</div>
                    </Inline>

                    <Dropzone
                        onDrop={(accepted, rejected) => {
                            if (rejected.length != 0) {
                                showError({ label: 'Upload failed', content: 'Only a single zip file not bigger than 1mb is allowed', modal: true, timeout: 3000 })
                            }
                            else {
                                setFileUploads(Object.values(accepted))
                            }
                        }}
                        maxSize={1048576}
                        maxFiles={1}
                        accept={{ 'application/zip': [] }}
                    >
                        {({ getRootProps, getInputProps }) => (
                            <div className='import-drop-zone'>
                                <div {...getRootProps()}>
                                    <input {...getInputProps()} />
                                    <p style={{ cursor: 'pointer' }}>Drag 'n' drop your import file here, or <u>click</u> to select your import files</p>
                                </div>
                            </div>
                        )}
                    </Dropzone>

                </>
            )}

            {(uploadFiles.length > 0 && !isUploading) && (
                <>
                    <br />
                    <div>
                        File to be imported:
                    </div>
                    <ul>
                        <li>{(uploadFiles[0] as File).name}</li>
                    </ul>
                </>
            )}
        </Modal>
    )
}