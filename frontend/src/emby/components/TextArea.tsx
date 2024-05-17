import * as React from 'react';
import './TextArea.css'
import ContentEditable from 'react-contenteditable';
import * as sanitizeHtml from 'sanitize-html';
import { dismissToast } from '~/common/helpers/utils';

type OnValueChange = (html: string) => void;
type OnError = (errorMessage: string) => void;

export type TextAreaProps = { html: string, maxChar?: number, onValueChange: OnValueChange, onError?: OnError }

export const TextArea: React.FC<TextAreaProps> = props => {
    if (!props.maxChar) {
        props.maxChar = 256;
    }

    return <ContentEditable
        className='textarea textarea-mono emby-textarea'
        html={props.html}
        disabled={false}
        onChange={(e) => { }}
        onFocus={(e) => { dismissToast(); }}
        onBlur={(e) => {
            try {
                e.stopPropagation();
                const d = $(e.target);
                if (d.text().length >= props.maxChar) {
                    throw new Error("Size limit " + props.maxChar + " exceeded");
                }

                props.onValueChange(sanitizeHtml(d.text().trim() === "" ? "" : d.html(), {
                    allowedTags: ["b", "i", "em", "strong", "a", "p", "h1", "br", "div", "ul", "ol", "li"],
                    allowedAttributes: { a: ["href"] }
                }));

            } catch (e) {
                if (props.onError) {
                    props.onError(e);
                } else {
                    throw e;
                }
            }
        }}

    />
};
