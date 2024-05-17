import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { version } from '~/emby/app.data';
import { Guid } from './guid';

export function utils_configure() {
    toast.configure({ containerId: "smartPlaylistToast" })
}

export type ErrorProps = {
    label: string,
    content: any,
    modal: boolean,
    timeout?: number
}

export function showError(errorProps: ErrorProps): void {
    var m = `${errorProps.label}: ${(errorProps.content instanceof Error ? errorProps.content.message : errorProps.content)}`;
    toast.error(m, {
        containerId: errorProps.modal ? "smartPlaylistToast-modalToast" : "smartPlaylistToast-appToast",
        autoClose: errorProps.timeout ? errorProps.timeout : false,
        position: 'top-center',
        bodyStyle: {
            zIndex: 1000
        }
    });
}

export function showInfo(msg: string, modal: boolean) {
    toast(msg, {
        containerId: modal ? "smartPlaylistToast-modalToast" : "smartPlaylistToast-appToast"
    });
}

export function tryParseInt(i: any) {
    if (isNaN(i))
        throw Error(i + ' is not a valid number.');

    return parseInt(i);
}

export function openUrl(baseUrl: string, newWindow: boolean) {
    const url = `${baseUrl}?v=${version}&X-Emby-Client=${window.ApiClient.appName()}&X-Emby-Device-Name=${window.ApiClient.deviceName()}&X-Emby-Device-Id=${window.ApiClient.deviceId()}&X-Emby-Client-Version=${window.ApiClient.appVersion()}&X-Emby-Token=${window.ApiClient.accessToken()}`;
    if (newWindow)
        window.open(url, Guid.newGuid());
    else
        window.location.href = url;
}

export const toBase64 = file => new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = () => resolve(reader.result);
    reader.onerror = error => reject(error);
});

export const loadLog = (id) => {
    try {
        openUrl(`../smartplaylist/log/${id}`, true)
    } catch (e) {
        var msg = e instanceof Response ? "Log file for playlist does not exist yet" :
            e instanceof Error ? e.message : e;

        showError({ label: "Error loading playlist log", content: msg, modal: true });
    }
}

var hoverTimer;
export const showHoverToast = (content, delay = 2000) => {
    clearTimeout(hoverTimer);
    if (!content || content === '')
        return;

    hoverTimer = setTimeout(() => {
        toast.info(
            content, {
            toastId: 'smartPlaylistToast-notes-toast',
            position: "bottom-right",
            autoClose: false
        });
    }, delay);
};

export const dismissToast = () => {
    clearTimeout(hoverTimer);
    toast.dismiss();
}