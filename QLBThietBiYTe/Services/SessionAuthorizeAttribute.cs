using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace QLBThietBiYTe.Services
{
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var user = session.GetString("User");

            
            if (string.IsNullOrEmpty(user))
            {
                context.Result = new RedirectToActionResult("Index", "DangNhap", null);
                return;
            }

            
            var loginTimeStr = session.GetString("LoginTime");
            if (!string.IsNullOrEmpty(loginTimeStr) && DateTime.TryParse(loginTimeStr, out DateTime loginTime))
            {
                if ((DateTime.Now - loginTime).TotalMinutes > 10)
                {
                    session.Clear(); 
                    context.Result = new RedirectToActionResult("Index", "DangNhap", null);
                    return;
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
