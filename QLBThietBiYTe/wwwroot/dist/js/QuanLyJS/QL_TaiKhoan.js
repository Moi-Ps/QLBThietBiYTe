$(document).ready(function () {

    loadLoaiThietBi();

    $('#addLTB').on('click', function (e) {
        e.preventDefault();

        $('#loaiThietBiForm')[0].reset();
        $('#maLoai').val('');
        $('#loaiThietBiModalLabel').text('Thêm mới loại thiết bị');
        isUpdate = false;
    });

    $('#loaiThietBiForm').on('submit', function (e) {
        e.preventDefault();

        const loaiThietBiForm = {
            maLoai: $('#maLoai').val(),
            tenLoaiThietBi: $('#tenLoaiThietBi').val(),
        };


        $.ajax({
            url: isUpdate ? '/QuanLy/QL_LoaiThietBi/updateLoaiThietBi' : '/QuanLy/QL_LoaiThietBi/CreateLoaiThietBi',
            type: 'POST',
            data: loaiThietBiForm,
            success: function () {
                //ShowToast('success', isUpdate ? 'Cập nhật thành công!' : 'Thêm thành công!');
                $('#loaiThietBiForm')[0].reset();

                $('#loaiThietBiModal').modal('hide');
                loadLoaiThietBi();
                isUpdate = false;
            },
            error: function () {
                ShowToast('error', 'Thất bại!');
            }
        });
    });
});

function updateLoaiThietBi(maloai) {
    $.ajax({
        url: '/QuanLy/QL_LoaiThietBi/getMaLoai',
        type: 'POST',
        data: { maLTB: maloai },
        success: function (response) {
            $('#maLoai').val(response.maLoai);
            $('#tenLoaiThietBi').val(response.tenLoaiThietBi);
            $('#loaiThietBiModalLabel').text('Sửa loại thiết bị');
            $('#loaiThietBiModal').modal('show');

            isUpdate = true;
        },
        error: function () {
            ShowToast('error', 'Thất bại!');
        }
    });
}
function deleteLoaiThietBi(maloai) {
    $('#modal-danger').modal('show');

    $('#btnDanger').off('click').on('click', function () {
        $.ajax({
            url: '/QuanLy/QL_LoaiThietBi/DeleteLTB',
            type: 'POST',
            data: { maLTB: maloai },
            success: function () {
                $('#modal-danger').modal('hide');
                loadLoaiThietBi();
                ShowToast('success', 'Xóa thành công!');
            },
            error: function () {
                ShowToast('error', 'Thất bại!');
            }
        });
    });
}
// Hàm tải danh sách loại thiết bị
function loadLoaiThietBi() {
    $.ajax({
        url: '/QuanLy/QL_LoaiThietBi/getDSLTB',
        type: 'POST',
        success: function (response) {
            const data = response;
            let rows = '';
            data.forEach(loaiThietBi => {
                rows += `
                <tr>
                    <td class="text-secondary">${loaiThietBi.id}</td>
                    <td class="text-secondary">${loaiThietBi.maLoai}</td>
                    <td class="text-secondary">${loaiThietBi.tenLoaiThietBi}</td>
                    <td>
                        <button class="bg-transparent border-0" onclick="updateLoaiThietBi('${loaiThietBi.maLoai}')">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">
                                    <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                                    <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"/>
                                </svg>
                        </button>
                        <button class="bg-transparent border-0" onclick="deleteLoaiThietBi('${loaiThietBi.maLoai}')">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                                <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
                            </svg>
                        </button>
                    </td>
                </tr>
            `;
            });
            $('#loaiThietBiTable').html(rows);
        },
        error: function (xhr) {
            alert('Lỗi khi tải dữ liệu: ' + xhr.responseText);
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
