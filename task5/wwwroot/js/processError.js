function displayError(message) {
    const errorMessage = document.getElementById('error-message');
    errorMessage.textContent = message;
}

function hideError() {
    const errorMessage = document.getElementById('error-message');
    errorMessage.textContent = '';
}

window.displayError = displayError;
window.hideError = hideError;