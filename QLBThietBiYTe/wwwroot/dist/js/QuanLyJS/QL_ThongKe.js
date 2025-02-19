$(document).ready(function () {
    if (window.Litepicker) {
        function initLitepicker(elementId) {
            new Litepicker({
                element: document.getElementById(elementId),
                format: 'DD/MM/YYYY',
                buttonText: {
                    previousMonth: `<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="icon icon-1"><path d="M15 6l-6 6l6 6" /></svg>`,
                    nextMonth: `<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="icon icon-1"><path d="M9 6l6 6l-6 6" /></svg>`
                },
                onSelect: function () {
                    loadPDF();
                }
            });
        }
        initLitepicker('tuNgay');
        initLitepicker('denNgay');
    }
    $("#btnFilter").on("click", function () {
        loadPDF();
    });
});

function loadPDF() {
    var tuNgay = $("#tuNgay").val();
    var denNgay = $("#denNgay").val();
    console.log("loadPDF: ", tuNgay, denNgay);
    if (!tuNgay || !denNgay) {
        console.log("Không có giá trị ngày!");
        return;
    }
    $.ajax({
        url: "/QuanLy/QL_ThongKe/PDFtkDoanhThu",
        type: "POST",
        data: {
            tuNgay: tuNgay,
            denNgay: denNgay
        },
        xhrFields: { responseType: "blob" },
        success: function (response) {
            console.log("Ajax success, nhận blob PDF", response);
            var blob = new Blob([response], { type: "application/pdf" });
            var url = URL.createObjectURL(blob);
            console.log("PDF URL: ", url);
            $("#pdfViewer").attr("src", url);
        },
        error: function () {
            alert("Thất bại khi tải PDF!");
        }
    });
}