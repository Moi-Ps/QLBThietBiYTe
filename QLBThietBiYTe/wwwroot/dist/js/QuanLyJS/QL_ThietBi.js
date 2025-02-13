$(document).ready(function () {

    loadThietBi();
    loadLoaiThietBi();
    loadNhaCungCap();

    $('#addTB').on('click', function (e) {
        e.preventDefault();

        $('#thietBiForm')[0].reset();
        $('#maThietBi').val('');
        $('#thietBiModalLabel').text('Thêm mới thiết bị');
        isUpdate = false;
    });

    $('#thietBiForm').on('submit', function (e) {
        e.preventDefault();

        const thietBiForm = {
            maThietBi: $('#maThietBi').val(),
            tenThietBi: $('#tenThietBi').val(),
            namSanXuat: $('#namSanXuat').val(),
            maLoai: $('#maLoai').val(),
            maNCC: $('#maNCC').val(),
            giaMua: $('#giaMua').val(),
            giaBan: $('#giaBan').val(),
            
        };


        $.ajax({
            url: isUpdate ? '/QuanLy/QL_ThietBi/UpdateThietBi' : '/QuanLy/QL_ThietBi/CreateThietBi',
            type: 'POST',
            data: thietBiForm,
            success: function () {
                //ShowToast('success', isUpdate ? 'Cập nhật thành công!' : 'Thêm thành công!');
                $('#thietBiForm')[0].reset();

                $('#thietBiModal').modal('hide');
                loadThietBi();
                isUpdate = false;
            },
            error: function () {
                ShowToast('error', 'Thất bại!');
            }
        });
    });
});

function updateThietBi(matb) {
    $.ajax({
        url: '/QuanLy/QL_ThietBi/GetMaThietBi',
        type: 'POST',
        data: { maTB: matb },
        success: function (response) {
            $('#maThietBi').val(response.maThietBi);
            $('#tenThietBi').val(response.tenThietBi);
            $('#namSanXuat').val(response.namSanXuat);
            $('#maLoai').val(response.maLoai);
            $('#maNCC').val(response.maNCC);
            $('#giaMua').val(response.giaMua);
            $('#giaBan').val(response.giaBan);
            $('#thietBiModalLabel').text('Sửa thiết bị');
            $('#thietBiModal').modal('show');

            isUpdate = true;
        },
        error: function () {
            ShowToast('error', 'Thất bại!');
        }
    });
}
function deleteThietBi(matb) {
    $('#modal-danger').modal('show');

    $('#btnDanger').off('click').on('click', function () {
        $.ajax({
            url: '/QuanLy/QL_ThietBi/DeleteThietBi',
            type: 'POST',
            data: { maTB: matb },
            success: function () {
                $('#modal-danger').modal('hide');
                loadThietBi();
                ShowToast('success', 'Xóa thành công!');
            },
            error: function () {
                ShowToast('error', 'Thất bại!');
            }
        });
    });
}
// Hàm tải danh sách thiết bị
function loadThietBi() {
    $.ajax({
        url: '/QuanLy/QL_ThietBi/getDSThietBi',
        type: 'POST',
        success: function (response) {
            const data = response;
            let rows = '';
            data.forEach(thietBi => {
                rows += `
                <tr>
                    <td class="text-secondary">${thietBi.id}</td>
                    <td class="text-secondary">${thietBi.maThietBi}</td>
                    <td class="text-secondary">${formatDate(thietBi.namSanXuat)}</td>
                    <td class="text-secondary">${thietBi.tenThietBi}</td>
                    <td class="text-secondary">${thietBi.tenLoaiThietBi}</td>
                    <td class="text-secondary">${thietBi.tenNhaCungCap}</td>
                    <td class="text-secondary">${thietBi.giaMua}</td>
                    <td class="text-secondary">${thietBi.giaBan}</td>            
                    <td>
                        <button class="bg-transparent border-0" onclick="updateThietBi('${thietBi.maThietBi}')">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">
                                    <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                                    <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"/>
                                </svg>
                        </button>
                        <button class="bg-transparent border-0" onclick="deleteThietBi('${thietBi.maThietBi}')">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                                <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
                            </svg>
                        </button>
                    </td>
                </tr>
            `;
            });
            $('#thietBiTable').html(rows);
        },
        error: function (xhr) {
            alert('Lỗi khi tải dữ liệu: ' + xhr.responseText);
        }
    });
}
// Hàm tải danh sách loại thiết bị
function loadLoaiThietBi() {
    $.ajax({
        url: '/QuanLy/QL_LoaiThietBi/getDSLTB',
        type: 'POST',
        success: function (data) {
            let options = '<option value="" disabled selected>Chọn loại thiết bị</option>';
            data.forEach(loaiThietBi => {
                options += `<option value="${loaiThietBi.maLoai}">${loaiThietBi.tenLoaiThietBi}</option>`;
            });
            $('#maLoai').html(options);
        },
        error: function (xhr) {
            alert('Lỗi khi tải danh sách: ' + xhr.responseText);
        }
    });
}
// Hàm tải danh sách nhà cung cấp
function loadNhaCungCap() {
    $.ajax({
        url: '/QuanLy/QL_NhaCungCap/getDSNCC',
        type: 'POST',
        success: function (data) {
            let options = '<option value="" disabled selected>Chọn nhà cung cấp</option>';
            data.forEach(nhaCungCap => {
                options += `<option value="${nhaCungCap.maNCC}">${nhaCungCap.tenNhaCungCap}</option>`;
            });
            $('#maNCC').html(options);
        },
        error: function (xhr) {
            alert('Lỗi khi tải danh sách: ' + xhr.responseText);
        }
    });
}


// Format date to dd/mm/yyyy
function formatDate(dateString) {
    let date = new Date(dateString);
    let day = ('0' + date.getDate()).slice(-2);
    let month = ('0' + (date.getMonth() + 1)).slice(-2);
    let year = date.getFullYear();
    return `${day}/${month}/${year}`;
}
