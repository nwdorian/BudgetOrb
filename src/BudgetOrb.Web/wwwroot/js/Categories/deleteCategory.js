let deleteModal;
const deleteContainer = document.getElementById("delete-category-modal-placeholder");

async function loadDeleteModal(id) {
    try {
        const response = await fetch(`/Categories/Delete/${id}`);
        if (!response.ok) {
            console.error("There was an error loading the modal: ", response.error);
        }
        const html = await response.text();
        deleteContainer.innerHTML = html;

        deleteModal = new bootstrap.Modal(document.getElementById("delete-category-modal"));
        deleteModal.show();
    } catch (error) {
        console.error("There was an unexpected error loading the delete modal:", error);
    }
}

async function submitDelete(form, event) {
    event.preventDefault();

    try {
        const response = await fetch(form.action, {
            method: "POST",
            body: new FormData(form),
        });

        if (!response.ok) {
            window.location.href = "/Errors/Error";
            return;
        }

        if (response.status !== 204) {
            const errorHtml = await response.text();
            const errorDiv = document.createElement("div");
            errorDiv.innerHTML = errorHtml;

            const errorContent = errorDiv.querySelector(".modal-content").innerHTML;
            document.querySelector("#delete-category-modal .modal-content").innerHTML = errorContent;
            return;
        }

        deleteModal.hide();
        window.location.reload();
    } catch (error) {
        console.error("There was an unexpected error deleting a category: ", error);
    }
}
