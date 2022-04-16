export function formErrorMessage(response) {
    return response.json().then(data => {
        if (!data.errors) {
            throw new Error(data);
        } else {
            let errorString = "";
    
            for (const error of Object.values(data.errors)) {
                errorString += `${error[0]}\n`;
            }
    
            throw new Error(errorString);
        }
    });
}