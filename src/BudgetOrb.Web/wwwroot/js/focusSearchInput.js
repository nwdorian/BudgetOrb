const searchField = document.getElementById("search-term");

function focusInput() {
    if (searchField && searchField.value.trim().length > 0) {
        const len = searchField.value.length;
        searchField.focus();
        searchField.setSelectionRange(len, len);
    }
}

document.addEventListener("DOMContentLoaded", focusInput);
