$(document).ready(function () {

    $('a[href="#tabs-bckho-5"]').on('click', function (e) {
        loadPDF();
    })

    // PDF download and load
    $("#btnDownloadPDF").on("click", function () {
        printPDF();
    });

    // Form thêm kho
    $("#khoForm").on("submit", function (e) {
        e.preventDefault();
        const khoForm = {
            maThietBi: $("#maThietBi").val(),
            soLuong: $("#soLuong").val(),
        };

        $.ajax({
            url: "/QuanLy/Kho/createKho",
            type: "POST",
            data: khoForm,
            success: function () {
                ShowToast("success", "Thêm thành công!");
                loadKho();
                loadLichSuKho();
                $("#khoForm")[0].reset();
                $("#khoModal").modal("hide");
                $(".modal-backdrop").remove();
                $("body").removeClass("modal-open");
            },
            error: function () {
                ShowToast("error", "Thất bại!");
            },
        });
    });

    // load data
    loadKho();
});
function loadKho() {
    $.ajax({
        url: "/QuanLy/QL_Kho/getDSKho",
        type: "POST",
        success: function (response) {
            const data = response;
            let rows = "";
            data.forEach((kho) => {
                rows += `
                            <tr>
                                <td class="text-secondary">${kho.tenThietBi}</td>
                                <td class="text-secondary">${kho.tongSoLuong}</td>
                                <td  class="">
                                    <button class="bg-transparent border-0" onclick="updateLsKho()">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">
                                            <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                                            <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"/>
                                        </svg>
                                    </button>
                                    <button class="bg-transparent border-0" onclick="deleteLsKho()">
                                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                            <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                                            <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
                                        </svg>
                                    </button>
                                </td>
                           </tr> 
                        `;
            });
            $("#khoTable").html(rows);
        },
        error: function () {
            ShowToast("error", "Thất bại!");
        },
    });
}
// Hàm Load PDF into iframe
function loadPDF() {
    $.ajax({
        url: "/QuanLy/QL_Kho/pdfKho",
        type: "POST",
        xhrFields: { responseType: "blob" },
        success: function (response) {
            var blob = new Blob([response], { type: "application/pdf" });
            var url = URL.createObjectURL(blob);
            $("#pdfViewer").attr("src", url);
        },
        error: function () {
            ShowToast("error", "Thất bại!");
        },
    });
}

// Hàm Print PDF
function printPDF() {
    $.ajax({
        url: '/QuanLy/QL_Kho/pdfKho',
        type: 'POST',
        xhrFields: { responseType: 'blob' },
        success: function (response) {
            var blob = new Blob([response], { type: 'application/pdf' });
            var url = URL.createObjectURL(blob);
            var iframe = document.createElement('iframe');
            iframe.style.display = 'none';
            iframe.src = url;
            document.body.appendChild(iframe);
            iframe.onload = function () {
                iframe.contentWindow.print();
            };
        },
        error: function () {
            ShowToast('error', 'Thất bại!');
        }
    });
}
// Format date to dd/mm/yyyy
function formatDate(dateString) {
    let date = new Date(dateString);
    let day = ("0" + date.getDate()).slice(-2);
    let month = ("0" + (date.getMonth() + 1)).slice(-2);
    let year = date.getFullYear();
    return `${day}/${month}/${year}`;
}
