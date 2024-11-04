using BaiTap4.Models;
using BaiTap4.Models.Authentication;
using BaiTap4.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace BaiTap4.Controllers
{
	public class HomeController : Controller
	{
		QlbanVaLiContext db = new QlbanVaLiContext();
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		[Authentication]
		public IActionResult Index(int? page)
		{
			int pageSize = 8;
			int pageNumber = page == null || page < 0 ? 1 : page.Value;
			var lstsanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
			PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
			return View(lst);
		}
		[Authentication]

		public IActionResult SanPhamTheoLoai(String maLoai, int? page)
		{
			int pageSize = 8;
			int pageNumber = page == null || page < 0 ? 1 : page.Value;
			var lstsanpham = db.TDanhMucSps.AsNoTracking().Where(x => x.MaLoai == maLoai).OrderBy(x => x.TenSp);
			PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
			ViewBag.maLoai = maLoai;
			return View(lst);
		}
		[Authentication]
		public IActionResult ChiTietSanPham(String maSp)
		{
			var sanpham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
			var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
			ViewBag.anhSanPham = anhSanPham;
			return View(sanpham);
		}
		[Authentication]
		public IActionResult ProductDetail(String maSp)
		{
			var sanpham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSp);
			var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSp).ToList();
			var homeProductDetailViewModel = new HomeProductDetailViewModel
			{
				danhMucSp = sanpham,
				anhSps = anhSanPham
			};
			return View(homeProductDetailViewModel);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
