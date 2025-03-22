let loadedBooks = [];
let exportButtonInitialized = false;
let isExporting = false;

function addBooksToLoadedList(books) {
    if (Array.isArray(books)) {
        loadedBooks = loadedBooks.concat(books);
    }
}

function resetLoadedBooks() {
    loadedBooks = [];
}

function exportBooksToCSV(event) {
    if (isExporting) {
        return false;
    }
    isExporting = true;
    debounce(() => { isExporting = false; }, 1000);
    try {
        if (loadedBooks.length === 0) {
            displayError("No books to export. Please load books first.");
            return;
        }
        const booksForExport = loadedBooks.map(book => ({
            ISBN: book.isbn,
            Title: book.title,
            Author: book.author,
            Publisher: book.publisher,
            Likes: book.likes,
            ReviewCount: book.reviews ? book.reviews.length : 0
        }));
        const date = new Date();
        const formattedDate = `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
        const filename = `books_export_${formattedDate}_${Date.now()}.csv`;
        const csv = Papa.unparse(booksForExport);
        downloadCSV(csv, filename);

    } catch (error) {
        displayError("Error exporting books: " + error.message);
    }
    return false;
}

function downloadCSV(csvContent, filename) {
    const blob = new Blob([csvContent], { type: "text/csv;charset=utf-8;" });
    const link = document.createElement("a");
    if (navigator.msSaveBlob) {
        navigator.msSaveBlob(blob, filename);
    } else {
        const url = URL.createObjectURL(blob);
        link.href = url;
        link.setAttribute("download", filename);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    }
}

function initializeExportButton() {
    const exportButton = document.getElementById('export-csv-btn');
    exportButton.addEventListener('click', exportBooksToCSV);
}

document.addEventListener('DOMContentLoaded', () => {
    if (!exportButtonInitialized) {
        initializeExportButton();
    }
});

window.addBooksToLoadedList = addBooksToLoadedList;
window.resetLoadedBooks = resetLoadedBooks;