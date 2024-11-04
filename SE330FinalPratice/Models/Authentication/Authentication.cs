using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BaiTap4.Models.Authentication
{
	public class Authentication : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			if (context.HttpContext.Session.GetString("Username") == null)
			{
				context.Result = new RedirectToRouteResult(
					new RouteValueDictionary
					{
							{ "Controller", "Access" },
							{ "Action", "Login" },
							{ "Area", "" }
					});
				return;
			}

			string? userRole = context.HttpContext.Session.GetString("LoaiUser");
			string? currentArea = context.RouteData.Values["area"]?.ToString()?.ToLower();

			Console.WriteLine($"UserRole: {userRole}");
			Console.WriteLine($"CurrentArea: {currentArea}");

			if (userRole == "0")
			{
				if (currentArea == "admin")
				{
					context.Result = new RedirectToRouteResult(
						new RouteValueDictionary
						{
								{ "Controller", "Home" },
								{ "Action", "Index" },
								{ "Area", "" }
						});
					return;
				}
			}
			else if (userRole == "1")
			{
				if (currentArea != "admin")
				{
					context.Result = new RedirectToRouteResult(
						new RouteValueDictionary
						{
								{ "Controller", "Home" },
								{ "Action", "Index" },
								{ "Area", "admin" }
						});
					return;
				}
			}
		}
	}
}