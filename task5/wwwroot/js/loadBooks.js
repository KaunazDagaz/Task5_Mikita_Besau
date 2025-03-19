let page = 0;
let seed = null;

const fetchBooks = async () => {
    if (seed === null) {
        const seedInput = document.getElementById('seed-input');
        seed = seedInput.value.trim();

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
        const response = await fetch(`/Book/LoadBooks?count=20&seed=${seed+page}`);
        const newBooks = await response.json();

        const bookList = document.getElementById('book-list');

        newBooks.forEach(book => {
            const bookRow = document.createElement('tr');
            bookRow.innerHTML = `
                    <td class="fw-bold">${rowNumber++}</td>
                    <td>${book.isbn}</td>
                    <td>${book.title}</td>
                    <td>${book.author}</td>
                    <td>${book.publisher}</td>
                `;
            bookList.appendChild(bookRow);
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
        seed = null;
        fetchBooks();
    });

    window.addEventListener('scroll', () => {
        if (window.innerHeight + window.scrollY >= document.body.offsetHeight - 2) {
            fetchBooks();
        }
    });
};

initialize();