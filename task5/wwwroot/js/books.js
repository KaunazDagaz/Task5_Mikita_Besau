﻿let page = 0;
let seed = null;
let rowNumber = 1;
let avgLikes = 50.0;
let avgReviews = 5.0;

async function fetchBooks() {
    hideError();
    if (seed === null) {
        const seedInput = document.getElementById('seed-input');
        seed = seedInput.value.trim();
        if (!seed) {
            displayError('Please enter a valid seed value.');
            return;
        }
        document.getElementById('book-list').innerHTML = '';
        page = 0;
        rowNumber = 1;
    }

    const avgLikesSlider = document.getElementById('avg-likes-slider');
    const avgReviewsSlider = document.getElementById('avg-reviews-slider');
    const avgLikes = avgLikesSlider ? avgLikesSlider.value : avgLikes;
    const avgReviews = avgReviewsSlider ? avgReviewsSlider.value : avgReviews;

    const loading = document.getElementById('loading');
    loading.classList.remove('d-none');

    try {
        const response = await fetch(`/Book/LoadBooks?count=20&seed=${seed + page}&avgLikes=${avgLikes}&avgReviews=${avgReviews}`);
        const newBooks = await response.json();
        const bookList = document.getElementById('book-list');

        newBooks.forEach(book => {
            const bookRow = document.createElement('tr');
            bookRow.classList.add('book-row', 'table-light');

            bookRow.innerHTML = `
                <td class="fw-bold text-center">${rowNumber++}</td>
                <td>${book.isbn}</td>
                <td class="fw-semibold">${book.title}</td>
                <td>${book.author}</td>
                <td>${book.publisher}</td>
            `;

            const detailsRow = document.createElement('tr');
            detailsRow.classList.add('details-row', 'bg-light');
            detailsRow.style.display = 'none';

            detailsRow.innerHTML = `
                <td colspan="5">
                    <div class="card shadow-sm p-2 p-md-3">
                        <div class="row g-3">
                            <div class="col-6 col-sm-4 col-md-3 col-lg-2 mx-auto mx-sm-0">
                                <img src="${book.coverImageUrl}" class="img-fluid rounded" alt="Cover Image"/>
                            </div>
                            <div class="col-12 col-sm-8 col-md-9 col-lg-10">
                                <h5 class="fw-bold mb-1">${book.title}</h5>
                                <h6 class="mb-2">${book.author}</h6>
                                <div class="d-flex flex-wrap gap-3 mb-2">
                                    <div>
                                        <strong>ISBN:</strong> ${book.isbn}
                                    </div>
                                    <div class="text-secondary">
                                        ${book.publisher}
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <i class="fa-solid fa-heart text-danger"></i> ${book.likes}
                                </div>
                                <hr class="my-2">
                                <div class="reviews-section">
                                    <h6 class="mb-2">Reviews:</h6>
                                    <ul class="list-unstyled mb-0">
                                        ${book.reviews.map(review => `
                                            <li class="mb-2 pb-2 border-bottom border-light">
                                                <strong class="d-block">${review.username}:</strong>
                                                <span class="text-break">${review.comment}</span>
                                            </li>
                                        `).join('')}
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
            `;

            bookRow.addEventListener('click', () => {
                detailsRow.style.display = detailsRow.style.display === '' ? 'none' : '';
            });

            bookList.appendChild(bookRow);
            bookList.appendChild(detailsRow);
        });

        page++;
    } catch (error) {
        displayError('Error fetching books:', error);
    } finally {
        loading.classList.add('d-none');
    }
}


window.fetchBooks = fetchBooks;