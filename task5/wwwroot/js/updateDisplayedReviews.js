document.addEventListener('DOMContentLoaded', function () {
    const avgReviewsSlider = document.getElementById('avg-reviews-slider');
    const avgReviewsValue = document.getElementById('avg-reviews-value');
    avgReviewsValue.textContent = parseFloat(avgReviewsSlider.value).toFixed(1);
    avgReviewsSlider.addEventListener('input', function () {
        avgReviewsValue.textContent = parseFloat(avgReviewsSlider.value).toFixed(1);
    });
});