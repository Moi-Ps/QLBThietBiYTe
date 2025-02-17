$(document).ready(function () {
    loadHoaDon();
    loadChiTietHoaDon();

    $('#addHD').on('click', function (e) {
        e.preventDefault();

        $('#hoaDonForm')[0].reset();
        $('#maHoaDon').val('');
        $('#hoaDonModalLabel').text('Lập hóa đơn');
        isUpdate = false;
    });

    $('#hoaDonForm').on('submit', function (e) {
        e.preventDefault();

        const hoaDonForm = {
            maHoaDon: $('#maHoaDon').val(),
            tenKhachHang: $('#tenKhachHang').val(),
            ngayLap: $('#ngayLap').val(),
            tongTien: $('#tongTien').val(),
            chitiethoadons: getChiTietHoaDons()
        };

        $.ajax({
            url: 'QuanLy/QL_HoaDon/CreateHoaDon',
            type: 'POST',
            data: JSON.stringify(hoaDonForm),
            contentType: 'application/json',
            success: function () {
                $('#hoaDonForm')[0].reset();
                $('#hoaDonModal').modal('hide');
                loadHoaDon();
                isUpdate = false;
            },
            error: function () {
                alert('Thất bại!');
            }
        });
    });

    function getChiTietHoaDons() {
        let chiTietHoaDons = [];
        $('#ctHoaDonTable tr').each(function () {
            let chiTiet = {
                maChiTiet: $(this).find('.maChiTiet').text(),
                maThietBi: $(this).find('.maThietBi').text(),
                soLuong: $(this).find('.soLuong').text(),
                giaTien: $(this).find('.giaTien').text(),
                thanhTien: $(this).find('.thanhTien').text()
            };
            chiTietHoaDons.push(chiTiet);
        });
        return chiTietHoaDons;
    }
});

function updateHoaDon(maHoaDon) {
    $.ajax({
        url: '/QuanLy/QL_HoaDon/getHoaDon',
        type: 'POST',
        data: { maHoaDon: maHoaDon },
        success: function (response) {
            $('#maHoaDon').val(response.maHoaDon);
            $('#tenKhachHang').val(response.tenKhachHang);
            $('#ngayLap').val(response.ngayLap);
            $('#tongTien').val(response.tongTien);

            // Hiển thị chi tiết hóa đơn
            loadChiTietHoaDons(response.chiTietHoaDons);

            $('#hoaDonModalLabel').text('Sửa hóa đơn');
            $('#hoaDonModal').modal('show');
            isUpdate = true;
        },
        error: function () {
            alert('Thất bại!');
        }
    });
}

function deleteHoaDon(maHoaDon) {
    $('#modal-danger').modal('show');

    $('#btnDanger').off('click').on('click', function () {
        $.ajax({
            url: '/QuanLy/QL_HoaDon/DeleteHoaDon',
            type: 'POST',
            data: { maHoaDon: maHoaDon },
            success: function () {
                $('#modal-danger').modal('hide');
                loadHoaDon();
                alert('Xóa thành công!');
            },
            error: function () {
                alert('Thất bại!');
            }
        });
    });
}

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
                        <button class="bg-transparent border-0" onclick="addChiTietHoaDon('${hoaDon.maHoaDon}')">
                           <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-plus-square-dotted" viewBox="0 0 16 16">
                              <path d="M2.5 0q-.25 0-.487.048l.194.98A1.5 1.5 0 0 1 2.5 1h.458V0zm2.292 0h-.917v1h.917zm1.833 0h-.917v1h.917zm1.833 0h-.916v1h.916zm1.834 0h-.917v1h.917zm1.833 0h-.917v1h.917zM13.5 0h-.458v1h.458q.151 0 .293.029l.194-.981A2.5 2.5 0 0 0 13.5 0m2.079 1.11a2.5 2.5 0 0 0-.69-.689l-.556.831q.248.167.415.415l.83-.556zM1.11.421a2.5 2.5 0 0 0-.689.69l.831.556c.11-.164.251-.305.415-.415zM16 2.5q0-.25-.048-.487l-.98.194q.027.141.028.293v.458h1zM.048 2.013A2.5 2.5 0 0 0 0 2.5v.458h1V2.5q0-.151.029-.293zM0 3.875v.917h1v-.917zm16 .917v-.917h-1v.917zM0 5.708v.917h1v-.917zm16 .917v-.917h-1v.917zM0 7.542v.916h1v-.916zm15 .916h1v-.916h-1zM0 9.375v.917h1v-.917zm16 .917v-.917h-1v.917zm-16 .916v.917h1v-.917zm16 .917v-.917h-1v.917zm-16 .917v.458q0 .25.048.487l.98-.194A1.5 1.5 0 0 1 1 13.5v-.458zm16 .458v-.458h-1v.458q0 .151-.029.293l.981.194Q16 13.75 16 13.5M.421 14.89c.183.272.417.506.69.689l.556-.831a1.5 1.5 0 0 1-.415-.415zm14.469.689c.272-.183.506-.417.689-.69l-.831-.556c-.11.164-.251.305-.415.415l.556.83zm-12.877.373Q2.25 16 2.5 16h.458v-1H2.5q-.151 0-.293-.029zM13.5 16q.25 0 .487-.048l-.194-.98A1.5 1.5 0 0 1 13.5 15h-.458v1zm-9.625 0h.917v-1h-.917zm1.833 0h.917v-1h-.917zm1.834-1v1h.916v-1zm1.833 1h.917v-1h-.917zm1.833 0h.917v-1h-.917zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3z"/>
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

function loadChiTietHoaDon() {
    $.ajax({
        url: '/QuanLy/QL_HoaDon/getHoaDon',
        type: 'POST',
        success: function (response) {
            const data = response;
            let rows = '';
            data.forEach(ctHoaDon => {
                rows += `
                <tr>
                    <td class="text-secondary">${ctHoaDon.id}</td>
                    <td class="text-secondary">${ctHoaDon.maChiTiet}</td>
                    <td class="text-secondary">${ctHoaDon.tenThietBi}</td>
                    <td class="text-secondary">${ctHoaDon.soLuong}</td>
                    <td class="text-secondary">${ctHoaDon.giaTien}</td>
                    <td class="text-secondary">${ctHoaDon.thanhTien}</td>
                    <td>
                        <button class="bg-transparent border-0" onclick="deleteChiTietHoaDon('${ctHoaDon.maChiTiet}')">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                                <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
                            </svg>
                        </button>
                    </td>
                </tr>
            `;
            });
            $('#ctHoaDonTable').html(rows);
        },
        error: function (xhr) {
            alert('Lỗi khi tải dữ liệu: ' + xhr.responseText);
        }
    });
}

function formatDate(dateString) {
    let date = new Date(dateString);
    let day = ('0' + date.getDate()).slice(-2);
    let month = ('0' + (date.getMonth() + 1)).slice(-2);
    let year = date.getFullYear();
    return `${day}/${month}/${year}`;
}

function deleteChiTietHoaDon(maChiTiet) {
    $.ajax({
        url: '/QuanLy/QL_HoaDon/DeleteChiTietHoaDon',
        type: 'POST',
        data: { maChiTiet: maChiTiet },
        success: function () {
            loadChiTietHoaDon();
            alert('Xóa chi tiết hóa đơn thành công!');
        },
        error: function () {
            alert('Thất bại!');
        }
    });
}

function loadChiTietHoaDons(chiTietHoaDons) {
    let rows = '';
    chiTietHoaDons.forEach(ctHoaDon => {
        rows += `
        <tr>
            <td class="text-secondary">${ctHoaDon.id}</td>
            <td class="text-secondary maChiTiet">${ctHoaDon.maChiTiet}</td>
            <td class="text-secondary maThietBi">${ctHoaDon.tenThietBi}</td>
            <td class="text-secondary soLuong">${ctHoaDon.soLuong}</td>
            <td class="text-secondary giaTien">${ctHoaDon.giaTien}</td>
            <td class="text-secondary thanhTien">${ctHoaDon.thanhTien}</td>
            <td>
                <button class="bg-transparent border-0" onclick="deleteChiTietHoaDon('${ctHoaDon.maChiTiet}')">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                        <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                        <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
                    </svg>
                </button>
            </td>
        </tr>
        `;
    });
    $('#ctHoaDonTable').html(rows);
}