$(document).ready(function () {

    loadTaiKhoan();

    $('#addTK').on('click', function (e) {
        e.preventDefault();

        $('#taiKhoanForm')[0].reset();
        $('#maTaiKhoan').val('');
        $('#taiKhoanModalLabel').text('Thêm mới tài khoản');
        isUpdate = false;
    });

    $('#taiKhoanForm').on('submit', function (e) {
        e.preventDefault();

        const taiKhoanForm = {
            maTaiKhoan: $('#maTaiKhoan').val(),
            userName: $('#userName').val(),
            passWord: $('#passWord').val(),
            role: $('#role').val(),
        };


        $.ajax({
            url: isUpdate ? '/QuanLy/QL_TaiKhoan/UpdateTaiKhoan' : '/QuanLy/QL_TaiKhoan/CreateTaiKhoan',
            type: 'POST',
            data: taiKhoanForm,
            success: function () {
                //ShowToast('success', isUpdate ? 'Cập nhật thành công!' : 'Thêm thành công!');
                $('#taiKhoanForm')[0].reset();

                $('#taiKhoanModal').modal('hide');
                loadTaiKhoan();
                isUpdate = false;
            },
            error: function () {
                ShowToast('error', 'Thất bại!');
            }
        });
    });
});

function updateTaiKhoan(maTK) {
    $.ajax({
        url: '/QuanLy/QL_TaiKhoan/getTaiKhoan',
        type: 'POST',
        data: { maTK: maTK },
        success: function (response) {
            $('#maTaiKhoan').val(response.maTaiKhoan);
            $('#userName').val(response.userName);
            $('#passWord').val(response.passWord);
            $('#role').val(response.role);
            $('#taiKhoanModalLabel').text('Sửa tài khoản');
            $('#taiKhoanModal').modal('show');

            isUpdate = true;
        },
        error: function () {
            ShowToast('error', 'Thất bại!');
        }
    });
}
function deleteTaiKhoan(maTK) {
    $('#modal-danger').modal('show');

    $('#btnDanger').off('click').on('click', function () {
        $.ajax({
            url: '/QuanLy/QL_TaiKhoan/DeleteTaiKhoan',
            type: 'POST',
            data: { maTK: maTK },
            success: function () {
                $('#modal-danger').modal('hide');
                loadTaiKhoan();
                //ShowToast('success', 'Xóa thành công!');
            },
            error: function () {
                ShowToast('error', 'Thất bại!');
            }
        });
    });
}
// Hàm tải danh sách loại thiết bị
function loadTaiKhoan() {
    $.ajax({
        url: '/QuanLy/QL_TaiKhoan/read',
        type: 'POST',
        success: function (response) {
            const data = response;
            let rows = '';
            data.forEach(taiKhoan => {
                rows += `
                <tr>
                    <td class="text-secondary">${taiKhoan.id}</td>
                    <td class="text-secondary">${taiKhoan.maTaiKhoan}</td>
                    <td class="text-secondary">${taiKhoan.userName}</td>
                    <td class="text-secondary">${taiKhoan.passWord}</td>
                    <td class="text-secondary">${taiKhoan.role}</td>
                    <td>
                        <button class="bg-transparent border-0" onclick="updateTaiKhoan('${taiKhoan.maTaiKhoan}')">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-pencil-square" viewBox="0 0 16 16">
                                    <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                                    <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"/>
                                </svg>
                        </button>
                        <button class="bg-transparent border-0" onclick="deleteTaiKhoan('${taiKhoan.maTaiKhoan}')">
                            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" class="bi bi-trash" viewBox="0 0 16 16">
                                <path d="M5.5 5.5A.5.5 0 0 1 6 6v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m2.5 0a.5.5 0 0 1 .5.5v6a.5.5 0 0 1-1 0V6a.5.5 0 0 1 .5-.5m3 .5a.5.5 0 0 0-1 0v6a.5.5 0 0 0 1 0z"/>
                                <path d="M14.5 3a1 1 0 0 1-1 1H13v9a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V4h-.5a1 1 0 0 1-1-1V2a1 1 0 0 1 1-1H6a1 1 0 0 1 1-1h2a1 1 0 0 1 1 1h3.5a1 1 0 0 1 1 1zM4.118 4 4 4.059V13a1 1 0 0 0 1 1h6a1 1 0 0 0 1-1V4.059L11.882 4zM2.5 3h11V2h-11z"/>
                            </svg>
                        </button>
                    </td>
                </tr>
            `;
            });
            $('#taiKhoanTable').html(rows);
        },
        error: function (xhr) {
            alert('Lỗi khi tải dữ liệu: ' + xhr.responseText);
        }
    });
}

