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

/*function updateHoaDon(maHoaDon) {
    $.ajax({
        url: '/QuanLy/QL_HoaDon/getMaHoaDon',
        type: 'POST',
        data: { maHoaDon: maHoaDon },
        success: function (response) {
            $('#maHoaDon').val(response.maHoaDon);
            $('#tenKhachHang').val(response.tenKhachHang);
            $('#ngayLap').val(response.ngayLap);
            $('#tongTien').val(response.tongTien);
            loadChiTietHoaDons(response.chitiethoadons);
            $('#hoaDonModalLabel').text('Sửa hóa đơn');
            $('#hoaDonModal').modal('show');
            isUpdate = true;
        },
        error: function () {
            alert('Thất bại!');
        }
    });
}*/
function updateHoaDon(maHoaDon) {
    $.ajax({
        url: '/QuanLy/QL_HoaDon/getMaHoaDon',
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