export function base64ToArray(base64) {
    const binaryString = window.atob(base64);
    const bytes = [];

    for (let i = 0; i < binaryString.length; i++) {
        bytes[i] = binaryString.charCodeAt(i);
    }

    return bytes;
}