$(document).ready(function () {

    loadHoaDon();
    loadCTHoaDon(maHoaDon);
    loadThietBi();

    $('#soLuong, #giaTien').on('input', function () {
        var soLuong = parseFloat($('#soLuong').val()) || 0;
        var giaTien = parseFloat($('#giaTien').val()) || 0;
        var thanhTien = soLuong * giaTien;
        $('#thanhTien').val(thanhTien.toFixed(2));
    });
});

function loadHoaDon() {
    $.ajax({
        url: '/QuanLy/QL_HoaDon/read',
        type: 'POST',
        success: function (response) {
            const data = response;
            let rows = '';
            data.forEach(hoaDon => {
                rows += `
                <tr>
                    <td class="text-secondary">${hoaDon.id}</td>
                    <td class="text-secondary">${hoaDon.maHoaDon}</td>
                    <td class="text-secondary">${hoaDon.tenKhachHang}</td>
                    <td class="text-secondary">${formatDate(hoaDon.ngayLap)}</td>
                    <td class="text-secondary">${hoaDon.tongTien}</td>
                    <td>
                        <button class="bg-transparent border-0" onclick="loadCTHoaDon('${hoaDon.maHoaDon}')">
                               <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-plus-square-dotted" viewBox="0 0 16 16">
                                  <path d="M2.5 0q-.25 0-.487.048l.194.98A1.5 1.5 0 0 1 2.5 1h.458V0zm2.292 0h-.917v1h.917zm1.833 0h-.917v1h.917zm1.833 0h-.916v1h.916zm1.834 0h-.917v1h.917zm1.833 0h-.917v1h.917zM13.5 0h-.458v1h.458q.151 0 .293.029l.194-.981A2.5 2.5 0 0 0 13.5 0m2.079 1.11a2.5 2.5 0 0 0-.69-.689l-.556.831q.248.167.415.415l.83-.556zM1.11.421a2.5 2.5 0 0 0-.689.69l.831.556c.11-.164.251-.305.415-.415zM16 2.5q0-.25-.048-.487l-.98.194q.027.141.028.293v.458h1zM.048 2.013A2.5 2.5 0 0 0 0 2.5v.458h1V2.5q0-.151.029-.293zM0 3.875v.917h1v-.917zm16 .917v-.917h-1v.917zM0 5.708v.917h1v-.917zm16 .917v-.917h-1v.917zM0 7.542v.916h1v-.916zm15 .916h1v-.916h-1zM0 9.375v.917h1v-.917zm16 .917v-.917h-1v.917zm-16 .916v.917h1v-.917zm16 .917v-.917h-1v.917zm-16 .917v.458q0 .25.048.487l.98-.194A1.5 1.5 0 0 1 1 13.5v-.458zm16 .458v-.458h-1v.458q0 .151-.029.293l.981.194Q16 13.75 16 13.5M.421 14.89c.183.272.417.506.69.689l.556-.831a1.5 1.5 0 0 1-.415-.415zm14.469.689c.272-.183.506-.417.689-.69l-.831-.556c-.11.164-.251.305-.415.415l.556.83zm-12.877.373Q2.25 16 2.5 16h.458v-1H2.5q-.151 0-.293-.029zM13.5 16q.25 0 .487-.048l-.194-.98A1.5 1.5 0 0 1 13.5 15h-.458v1zm-9.625 0h.917v-1h-.917zm1.833 0h.917v-1h-.917zm1.834-1v1h.916v-1zm1.833 1h.917v-1h-.917zm1.833 0h.917v-1h-.917zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3z"/>
                                </svg>
                        </button>
                        <button class="bg-transparent border-0" onclick="updateHoaDon('${hoaDon.maHoaDon}')">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">
                                <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                                <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"/>
                            </svg>
                        </button>
                        <button class="bg-transparent border-0" onclick="deleteHoaDon('${hoaDon.maHoaDon}')">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                                <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
                            </svg>
                        </button>
                    </td>
                </tr>
            `;
            });
            $('#hoaDonTable').html(rows);
        },
        error: function (xhr) {
            alert('Lỗi khi tải dữ liệu: ' + xhr.responseText);
        }
    });
}
function loadCTHoaDon(maHoaDon) {
    $.ajax({
        url: '/QuanLy/QL_HoaDon/getHoaDon',
        type: 'POST',
        data: { maHoaDon: maHoaDon },
        success: function (response) {
            const data = response;
            let listItems = '';
            if (Array.isArray(data.chiTietHoaDon)) {
                data.chiTietHoaDon.forEach((item, index) => {
                    listItems += `
                        <li class="list-group-item" onclick="fillForm('${item.maChiTiet}', '${item.tenThietBi}', '${item.soLuong}', '${item.giaTien}', '${item.thanhTien}')">
                            <div class="row">
                                <div class="col-md-4">
                                    <strong>Mã chi tiết hóa đơn:</strong> ${item.maChiTiet}
                                </div>
                                <div class="col-md-4">
                                    <strong>Tên thiết bị:</strong> ${item.tenThietBi}
                                </div>
                                <div class="col-md-4">
                                    <strong>Số lượng:</strong> ${item.soLuong}
                                </div>
                                <div class="col-md-4">
                                    <strong>Giá tiền:</strong> ${item.giaTien}
                                </div>
                                <div class="col-md-4">
                                    <strong>Thành tiền:</strong> ${item.thanhTien}
                                </div>
                            </div>
                        </li>
                    `;
                });
            } else {
                alert('Dữ liệu chi tiết hóa đơn không hợp lệ');
            }

            $('#loadCTHoaDonList').html(listItems);
            $('#ctHoaDonModal').modal('show');
        },
        error: function (xhr) {
            alert('Lỗi khi tải chi tiết hóa đơn: ' + xhr.responseText);
        }
    });
}

function fillForm(maChiTiet, tenThietBi, soLuong, giaTien, thanhTien) {
    $('#maChiTiet').val(maChiTiet);
    $('#maThietBi').val(tenThietBi);
    $('#soLuong').val(soLuong);
    $('#giaTien').val(giaTien);
    $('#thanhTien').val(thanhTien);
}

function formatDate(dateString) {
    let date = new Date(dateString);
    let day = ('0' + date.getDate()).slice(-2);
    let month = ('0' + (date.getMonth() + 1)).slice(-2);
    let year = date.getFullYear();
    return `${day}/${month}/${year}`;
}

