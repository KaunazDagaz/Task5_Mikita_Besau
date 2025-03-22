let isLoading = false;
function initialize() {
    hideError();
    const seedInput = document.getElementById('seed-input');
    const avgLikesSlider = document.getElementById('avg-likes-slider');
    const avgReviewsSlider = document.getElementById('avg-reviews-slider');
    const avgLikesValue = document.getElementById('avg-likes-value');
    const avgReviewsValue = document.getElementById('avg-reviews-value');

    if (!seedInput || !avgLikesSlider || !avgReviewsSlider || !avgLikesValue || !avgReviewsValue) {
        displayError('One or more elements are not found in the DOM.');
        return;
    }

    const storedSeed = localStorage.getItem('seed');
    const storedAvgLikes = localStorage.getItem('avgLikes');
    const storedAvgReviews = localStorage.getItem('avgReviews');

    if (storedSeed !== null) {
        seedInput.value = storedSeed;
    } else {
        const randomSeed = Math.floor(Math.random() * 10000);
        seedInput.value = randomSeed;
        localStorage.setItem('seed', randomSeed);
    }

    if (storedAvgLikes !== null) {
        avgLikesSlider.value = storedAvgLikes;
        avgLikesValue.textContent = storedAvgLikes;
    } else {
        avgLikesSlider.value = avgLikes;
        avgLikesValue.textContent = avgLikes;
        localStorage.setItem('avgLikes', avgLikes);
    }

    if (storedAvgReviews !== null) {
        avgReviewsSlider.value = storedAvgReviews;
        avgReviewsValue.textContent = storedAvgReviews;
    } else {
        avgReviewsSlider.value = avgReviews;
        avgReviewsValue.textContent = avgReviews;
        localStorage.setItem('avgReviews', avgReviews);
    }

    seedInput.addEventListener('input', () => {
        localStorage.setItem('seed', seedInput.value);
        debouncedHandleInputChange();
    });
    avgLikesSlider.addEventListener('input', () => {
        localStorage.setItem('avgLikes', avgLikesSlider.value);
        avgLikesValue.textContent = avgLikesSlider.value;
        debouncedHandleInputChange();
    });
    avgReviewsSlider.addEventListener('input', () => {
        localStorage.setItem('avgReviews', avgReviewsSlider.value);
        avgReviewsValue.textContent = avgReviewsSlider.value;
        debouncedHandleInputChange();
    });

    setupInfiniteScroll();
    fetchBooks();
}

function setupInfiniteScroll() {
    const sentinel = document.createElement('div');
    sentinel.id = 'scroll-sentinel';
    document.body.appendChild(sentinel);

    const observer = new IntersectionObserver(entries => {
        if (entries[0].isIntersecting && !isLoading) {
            isLoading = true;
            fetchBooks().finally(() => {
                isLoading = false;
            });
        }
    });

    observer.observe(sentinel);
}

function generateRandomSeed() {
    const randomSeed = Math.floor(Math.random() * 1000000);
    document.getElementById('seed-input').value = randomSeed;
    localStorage.setItem('seed', randomSeed);
    debouncedHandleInputChange();
}

function handleInputChange() {
    page = 0;
    seed = null;
    rowNumber = 1;
    fetchBooks();
}
const debouncedHandleInputChange = debounce(handleInputChange, 1000);

document.addEventListener('DOMContentLoaded', initialize);