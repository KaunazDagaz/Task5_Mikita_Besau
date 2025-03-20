document.addEventListener('DOMContentLoaded', function () {
    const avgLikesSlider = document.getElementById('avg-likes-slider');
    const avgLikesValue = document.getElementById('avg-likes-value');
    avgLikesValue.textContent = parseFloat(avgLikesSlider.value).toFixed(1);
    avgLikesSlider.addEventListener('input', function () {
        avgLikesValue.textContent = parseFloat(avgLikesSlider.value).toFixed(1);
    });
});