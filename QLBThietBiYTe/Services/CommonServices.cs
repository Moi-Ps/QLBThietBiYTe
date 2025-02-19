using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace QLBThietBiYTe.Services
{
    public static class CommonServices
    {
        private static IWebHostEnvironment _hostingEnvironment;
        public static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _hostingEnvironment = hostingEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }
        public static string ConvertViewToString(ControllerContext controllerContext, PartialViewResult pvr, ICompositeViewEngine _viewEngine)
        {
            using (StringWriter writer = new StringWriter())
            {
                var viewResult = _viewEngine.FindView(controllerContext, pvr.ViewName, false);
                if (!viewResult.Success)
                {
                    throw new InvalidOperationException($"Không tìm thấy view '{pvr.ViewName}'.");
                }
                var viewContext = new ViewContext(controllerContext, viewResult.View, pvr.ViewData, pvr.TempData, writer, new HtmlHelperOptions());

                viewResult.View.RenderAsync(viewContext).GetAwaiter().GetResult();

                return writer.GetStringBuilder().ToString();
            }
        }

        //Phân trang
        public static async Task<dynamic> getModelsWithNumberPageAndModels(int pageNumber, IQueryable<object> query, int pageSize = 12)
        {
            // Tính toán tổng số bản ghi
            int totalRecords = await query.CountAsync();
            // Tính toán số trang
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (pageNumber == -1)
            {
                pageNumber = totalPages;
            }
            if (pageSize == 0)
            {
                pageSize = totalRecords;
            }

            // Kiểm tra xem có trang trước đó không
            bool hasPreviousPage = pageNumber > 1;

            // Kiểm tra xem có trang tiếp theo không
            bool hasNextPage = pageNumber < totalPages;

            // Sử dụng Skip và Take để thực hiện phân trang
            int skip = (pageNumber - 1) * pageSize;
            var result = await query.Skip(skip).Take(pageSize).ToListAsync();
            return new
            {
                prePage = hasPreviousPage ? pageNumber - 1 : 0,
                nextPage = hasNextPage ? pageNumber + 1 : 0,
                result = result,
            };
        }
        public static async Task<dynamic> getModelsWithNumberPageAndQuantityAndModelsForDB(int pageSize, int pageNumber, IEnumerable<object> query)
        {
            // Tính toán tổng số bản ghi
            int totalRecords = query.Count();

            // Tính toán số trang
            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            if (pageNumber == -1)
            {
                pageNumber = totalPages;
            }
            if (pageSize == 0)
            {
                pageSize = totalRecords;
            }

            // Kiểm tra xem có trang trước đó không
            bool hasPreviousPage = pageNumber > 1;

            // Kiểm tra xem có trang tiếp theo không
            bool hasNextPage = pageNumber < totalPages;

            // Sử dụng Skip và Take để thực hiện phân trang
            int skip = (pageNumber - 1) * pageSize;
            var result = query.Skip(skip).Take(pageSize).ToList();
            return new
            {
                prePage = hasPreviousPage ? pageNumber - 1 : 0,
                nextPage = hasNextPage ? pageNumber + 1 : 0,
                result = result,
            };
        }
    }
}
