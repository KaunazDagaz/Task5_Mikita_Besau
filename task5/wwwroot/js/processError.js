function displayError(message) {
    const errorElement = document.getElementById('error-message');
    if (errorElement) {
        errorElement.textContent = message;
    }
}

function hideError() {
    const errorElement = document.getElementById('error-message');
    if (errorElement) {
        errorElement.textContent = "";
    }
}

function getLocalizedString(key) {
    const localizationElement = document.getElementById('js-localization');
    return localizationElement.getAttribute('data-error-' + key);
}

window.displayError = displayError;
window.hideError = hideError;
window.getLocalizedString = getLocalizedString;