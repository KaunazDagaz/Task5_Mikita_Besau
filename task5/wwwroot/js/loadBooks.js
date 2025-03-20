let page = 0;
let seed = null;
let rowNumber = 1;
let avgLikes = 50.0;
let avgReviews = 5.0;

const fetchBooks = async () => {
    if (seed === null) {
        const seedInput = document.getElementById('seed-input');
        const avgLikesSlider = document.getElementById('avg-likes-slider');
        const avgReviewsSlider = document.getElementById('avg-reviews-slider');
        seed = seedInput.value.trim();
        avgLikes = parseFloat(avgLikesSlider.value);
        avgReviews = parseFloat(avgReviewsSlider.value);

        if (!seed) {
            alert('Please enter a seed value.');
            return;
        }

        document.getElementById('book-list').innerHTML = '';
        page = 0;
        rowNumber = 1;
    }

    const loading = document.getElementById('loading');
    loading.style.display = 'block';

    try {
        const response = await fetch(`/Book/LoadBooks?count=20&seed=${seed + page}&avgLikes=${avgLikes}&avgReviews=${avgReviews}`);
        const newBooks = await response.json();

        const bookList = document.getElementById('book-list');

        newBooks.forEach(book => {
            const bookRow = document.createElement('tr');
            bookRow.classList.add('book-row');

            bookRow.innerHTML = `
                <td class="fw-bold">${rowNumber++}</td>
                <td>${book.isbn}</td>
                <td>${book.title}</td>
                <td>${book.author}</td>
                <td>${book.publisher}</td>
            `;

            const detailsRow = document.createElement('tr');
            detailsRow.classList.add('details-row');
            detailsRow.style.display = 'none';

            detailsRow.innerHTML = `
                <td colspan="5">
                    <div class="p-3 bg-light">
                        <p><strong>ISBN:</strong> ${book.isbn}</p>
                        <p><strong>Title:</strong> ${book.title}</p>
                        <p><strong>Author:</strong> ${book.author}</p>
                        <p><strong>Publisher:</strong> ${book.publisher}</p>
                        <p><strong>Likes:</strong> ${book.likes}</p>
                        <div>
                            <strong>Reviews:</strong>
                            <ul class="reviews-list">Loading...</ul>
                        </div>
                    </div>
                </td>
            `;

            bookRow.addEventListener('click', async () => {
                const isVisible = detailsRow.style.display === '';
                if (!isVisible) {
                    detailsRow.style.display = '';

                    // Fetch reviews dynamically
                    const reviewsList = detailsRow.querySelector('.reviews-list');
                    reviewsList.innerHTML = '<li>Loading reviews...</li>';

                    try {
                        const reviewsResponse = await fetch(`/Review/GetReviews?avgReviews=${avgReviews}`);
                        const reviews = await reviewsResponse.json();

                        if (reviews.length === 0) {
                            reviewsList.innerHTML = '<li>No reviews found.</li>';
                        } else {
                            reviewsList.innerHTML = reviews
                                .map(review => `<li><strong>${review.username}:</strong> ${review.comment}</li>`)
                                .join('');
                        }
                    } catch (error) {
                        console.error('Error fetching reviews:', error);
                        reviewsList.innerHTML = '<li>Error loading reviews.</li>';
                    }
                } else {
                    detailsRow.style.display = 'none';
                }
            });

            bookList.appendChild(bookRow);
            bookList.appendChild(detailsRow);
        });

        page++;
    } catch (error) {
        console.error('Error fetching books:', error);
    } finally {
        loading.style.display = 'none';
    }
};

const initialize = () => {
    document.getElementById('load-books-button').addEventListener('click', () => {
        page = 0;
        seed = null;
        rowNumber = 1;
        fetchBooks();
    });

    window.addEventListener('scroll', () => {
        if (window.innerHeight + window.scrollY >= document.body.offsetHeight - 2) {
            fetchBooks();
        }
    });
};

initialize();
