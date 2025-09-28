var pageModal = null;

function openModal(type, id) {
    let link, modalElement;

    if (type === "category") {
        link = document.getElementById("deleteCategoryAction");
        link.href = "/Category/Delete/" + id;
        modalElement = document.getElementById("categoryDeleteModal");
    }
    else if (type === "product") {
        link = document.getElementById("deleteProductAction");
        link.href = "/Product/Delete/" + id;
        modalElement = document.getElementById("productDeleteModal");
    }

    if (modalElement) {
        pageModal = new bootstrap.Modal(modalElement, { keyboard: false });
        pageModal.show();
    }
}

function closeModal() {
    if (pageModal != null) {
        pageModal.hide();
        pageModal = null;
    }
}
