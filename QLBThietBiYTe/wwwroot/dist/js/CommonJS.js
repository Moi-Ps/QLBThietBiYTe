// Hàm format số tiền VND
function formatCurrency(amount) {
    return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + "VND";
}
function formatCurrencyInt(amount) {
    return amount.toLocaleString('vi-VN') + ' VND';
}
// Hàm định dạng ngày (dd/mm/yyyy)
function formatDate(dateString) {
    var date = new Date(dateString);
    var day = ('0' + date.getDate()).slice(-2);
    var month = ('0' + (date.getMonth() + 1)).slice(-2);
    var year = date.getFullYear();
    return day + '/' + month + '/' + year;
}
// Hàm chuyển từ dd/MM/yyyy -> yyyy-MM-ddTHH:mm:ss
function convertDateToISO(ddmmyyyy) {
    if (!ddmmyyyy) return null;
    var parts = ddmmyyyy.split('/');
    if (parts.length === 3) {
        var day = parts[0];
        var month = parts[1];
        var year = parts[2];
        return `${year}-${month}-${day}T00:00:00`;
    }
    return ddmmyyyy;
}