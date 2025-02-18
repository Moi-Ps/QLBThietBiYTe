
$(document).ready(function () {
    loadHoaDon();
    loadThietBi();
    // Tính toán thành tiền khi nhập số lượng và giá tiền
    $('#soLuong, #giaTien').on('input', function () {
        var soLuong = parseFloat($('#soLuong').val()) || 0;
        var giaTien = parseFloat($('#giaTien').val()) || 0;
        var thanhTien = soLuong * giaTien;
        $('#thanhTien').val(thanhTien.toFixed(2));
    });

    // Submit form hóa đơn
    $('#hoaDonForm').on('submit', function (e) {
        e.preventDefault();
        var $modal = $('#hoaDonModal');
        var maHoaDon = $modal.find('#maHoaDon').val();
        var tenKhachHang = $modal.find('#tenKhachHang').val();
        var ngayLap = $modal.find('#ngayLap').val();

        var invoiceData = {
            hoDon: {
                maHoaDon: maHoaDon,
                tenKhachHang: tenKhachHang,
                ngayLap: ngayLap,
                tongTien: 0
            },
            chiTietHoaDon: []
        };

        $.ajax({
            url: '/QuanLy/QL_HoaDon/CreateHoaDon',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(invoiceData),
            success: function (response) {
                alert(response.message);
                $modal.modal('hide');
                loadHoaDon();
            },
            error: function (xhr) {
                alert('Error: ' + xhr.responseText);
            }
        });
    });

    // Submit form chi tiết hóa đơn
    $('#ctHoaDonForm').on('submit', function (e) {
        e.preventDefault();
        var $ctModal = $('#ctHoaDonModal');
        var maHoaDon = $ctModal.find('#maHoaDonDetail').val();
        var maChiTiet = $ctModal.find('#maChiTiet').val();
        var maThietBi = $ctModal.find('#maThietBi').val();
        var soLuong = parseFloat($ctModal.find('#soLuong').val()) || 0;
        var giaTien = parseFloat($ctModal.find('#giaTien').val()) || 0;
        var thanhTien = parseFloat($ctModal.find('#thanhTien').val()) || 0;

        // Lấy dữ liệu hóa đơn hiện tại từ server (bao gồm chi tiết)
        $.ajax({
            url: '/QuanLy/QL_HoaDon/getHoaDon',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ maHoaDon: maHoaDon }),
            success: function (response) {
                var invoice = response;
                var details = invoice.chiTietHoaDon || [];

                // Kiểm tra xem chi tiết đã tồn tại (sửa) hay chưa (thêm mới)
                var existingIndex = details.findIndex(function (item) {
                    return item.maChiTiet === maChiTiet;
                });
                var newDetail = {
                    maChiTiet: maChiTiet ? maChiTiet : generateUniqueId(),
                    maHoaDon: maHoaDon,
                    maThietBi: maThietBi,
                    soLuong: soLuong,
                    giaTien: giaTien,
                    thanhTien: thanhTien
                };
                if (existingIndex !== -1) {
                    details[existingIndex] = newDetail;
                } else {
                    details.push(newDetail);
                }

                // Tính lại tổng tiền dựa trên các chi tiết
                var tongTien = details.reduce(function (sum, item) {
                    return sum + (item.thanhTien || 0);
                }, 0);

                var invoiceRequest = {
                    hoDon: {
                        maHoaDon: invoice.maHoaDon,
                        tenKhachHang: invoice.tenKhachHang,
                        ngayLap: invoice.ngayLap,
                        tongTien: tongTien
                    },
                    chiTietHoaDon: details
                };

                $.ajax({
                    url: '/QuanLy/QL_HoaDon/CreateHoaDon',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(invoiceRequest),
                    success: function (resp) {
                        alert(resp.message);
                        //$('#ctHoaDonForm')[0].reset();
                        $ctModal.find('#maChiTiet').val('');
                        $ctModal.find('#maThietBi').val('');
                        $ctModal.find('#soLuong').val('');
                        $ctModal.find('#giaTien').val('');
                        $ctModal.find('#thanhTien').val('');

                        loadCTHoaDonList(invoice.maHoaDon);
                        loadHoaDon();
                    },
                    error: function (xhr) {
                        alert('Error: ' + xhr.responseText);
                    }
                });
            },
            error: function (xhr) {
                alert('Error: ' + xhr.responseText);
            }
        });
    });

    // Xử lý nút xóa chi tiết
    $('#deleteCTDetail').on('click', function (e) {
        e.preventDefault();
        var maChiTiet = $('#maChiTiet').val();
        if (maChiTiet) {
            deleteCTHoaDon(maChiTiet);
        } else {
            alert("Vui lòng nhập mã chi tiết hóa đơn cần xóa.");
        }
    });
});
function loadThietBi() {
    $.ajax({
        url: '/QuanLy/QL_ThietBi/getDSThietBi', 
        type: 'POST',
        success: function (response) {
            var options = '<option value="" disabled selected>Chọn thiết bị</option>';
            response.forEach(function (item) {
                options += `<option value="${item.maThietBi}">${item.tenThietBi}</option>`;
            });
            $('#maThietBi').html(options);
        },
        error: function (xhr) {
            alert("Lỗi khi tải dữ liệu thiết bị: " + xhr.responseText);
        }
    });
}

// Hàm tạo ID mới cho chi tiết nếu chưa có
function generateUniqueId() {
    return 'CT' + new Date().getTime();
}

// Hàm load danh sách hóa đơn
function loadHoaDon() {
    $.ajax({
        url: '/QuanLy/QL_HoaDon/read',
        type: 'POST',
        success: function (response) {
            var data = response;
            var rows = '';
            data.forEach(function (hoaDon) {
                rows += `
              <tr>
                <td class="text-secondary">${hoaDon.maHoaDon}</td>
                <td class="text-secondary">${hoaDon.tenKhachHang}</td>
                <td class="text-secondary">${formatDate(hoaDon.ngayLap)}</td>
                <td class="text-secondary">${formatCurrency(hoaDon.tongTien)}</td>
                <td>
                  <button class="bg-transparent border-0" onclick="addCTHoaDon('${hoaDon.maHoaDon}')">
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

// Mở modal chi tiết hóa đơn và load danh sách chi tiết cho hóa đơn được chọn
function addCTHoaDon(maHoaDon) {
    $.ajax({
        url: '/QuanLy/QL_HoaDon/getHoaDon',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ maHoaDon: maHoaDon }),
        success: function (response) {
            $('#ctHoaDonModal #maHoaDonDetail').val(response.maHoaDon);
            loadCTHoaDonList(response.maHoaDon);
            $('#ctHoaDonModal').modal('show');
        },
        error: function (xhr) {
            alert('Lỗi khi tải chi tiết hóa đơn: ' + xhr.responseText);
        }
    });
}

// Load danh sách chi tiết hóa đơn
function loadCTHoaDonList(maHoaDon) {
    $.ajax({
        url: '/QuanLy/QL_HoaDon/getHoaDon',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ maHoaDon: maHoaDon }),
        success: function (response) {
            var data = response;
            var listItems = '';
            if (Array.isArray(data.chiTietHoaDon)) {
                data.chiTietHoaDon.forEach(function (item) {
                    listItems += `
                <li class="list-group-item" onclick="fillForm('${item.maChiTiet}', '${item.maThietBi}', '${item.soLuong}', '${item.giaTien}', '${item.thanhTien}')">
                  <div class="row">
                    <div class="col-md-4">
                      <strong>Mã chi tiết:</strong> ${item.maChiTiet}
                    </div>
                    <div class="col-md-4">
                      <strong>Mã thiết bị:</strong> ${item.maThietBi}
                    </div>
                    <div class="col-md-4">
                      <strong>Số lượng:</strong> ${item.soLuong}
                    </div>
                    <div class="col-md-4">
                      <strong>Giá tiền:</strong> ${formatCurrency(item.giaTien)}
                    </div>
                    <div class="col-md-4">
                      <strong>Thành tiền:</strong> ${formatCurrency(item.thanhTien)}
                    </div>
                  </div>
                </li>
              `;
                });
            } else {
                listItems = '<li class="list-group-item">Không có chi tiết hóa đơn</li>';
            }
            $('#loadCTHoaDonList').html(listItems);
        },
        error: function (xhr) {
            alert('Error: ' + xhr.responseText);
        }
    });
}

// Khi click vào một mục chi tiết, điền dữ liệu vào form để sửa
function fillForm(maChiTiet, maThietBi, soLuong, giaTien, thanhTien) {
    var $ctModal = $('#ctHoaDonModal');
    $ctModal.find('#maChiTiet').val(maChiTiet);
    $ctModal.find('#maThietBi').val(maThietBi);
    $ctModal.find('#soLuong').val(soLuong);
    $ctModal.find('#giaTien').val(giaTien);
    $ctModal.find('#thanhTien').val(thanhTien);
}
// Hàm định dạng ngày (dd/mm/yyyy)
function formatDate(dateString) {
    var date = new Date(dateString);
    var day = ('0' + date.getDate()).slice(-2);
    var month = ('0' + (date.getMonth() + 1)).slice(-2);
    var year = date.getFullYear();
    return day + '/' + month + '/' + year;
}

// Hàm xóa hóa đơn
function deleteHoaDon(maHoaDon) {
    $('#modal-danger').modal('show');
    $('#btnDanger').off('click').on('click', function () {
        $.ajax({
            url: '/QuanLy/QL_HoaDon/deleteHoaDon',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ maHoaDon: maHoaDon }),
            success: function (response) {
                $('#modal-danger').modal('hide');
                loadHoaDon();
            },
            error: function (xhr) {
                alert('Error: ' + xhr.responseText);
            }
        });
    });         
}

// Hàm xóa chi tiết hóa đơn
function deleteCTHoaDon(maChiTiet) {
    $('#modal-danger').modal('show');    

    $('#btnDanger').off('click').on('click', function () {
        $.ajax({
            url: '/QuanLy/QL_HoaDon/deleteChiTietHoaDon',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ maChiTiet: maChiTiet }),
            success: function () {
                $('#modal-danger').modal('hide');
                var currentMaHoaDon = $('#ctHoaDonModal #maHoaDonDetail').val();
                loadCTHoaDonList(currentMaHoaDon);
                loadHoaDon();
            },
            error: function () {
                ShowToast('error', 'Thất bại!');
            }
        });
    });
}

// Hàm cập nhật hóa đơn
function updateHoaDon(maHoaDon) {
    $.ajax({
        url: '/QuanLy/QL_HoaDon/getHoaDon',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ maHoaDon: maHoaDon }),
        success: function (response) {
            $('#hoaDonModal').modal('show');
            $('#maHoaDon').val(response.maHoaDon);
            $('#tenKhachHang').val(response.tenKhachHang);
            $('#ngayLap').val(formatDate(response.ngayLap));
            $('#tongTien').val(response.tongTien);
        },
        error: function (xhr) {
            alert('Error: ' + xhr.responseText);
        }
    });
}
function formatCurrency(amount) {
    return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + "VND";
}

