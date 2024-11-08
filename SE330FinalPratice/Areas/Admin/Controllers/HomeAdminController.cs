﻿using BaiTap4.Models;
using BaiTap4.Models.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BaiTap4.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/home")]
    public class HomeAdminController : Controller
    {
        QlbanVaLiContext db = new QlbanVaLiContext();
        [Route("")]
        [Route("index")]
        [Authentication]
        public IActionResult Index()
        {
            return View();
        }
        [Route("danhmucsanpham")]
        [Authentication]
        public IActionResult DanhMucSanPham(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstsanpham = db.TDanhMucSps.AsNoTracking().OrderBy(x => x.TenSp);
            PagedList<TDanhMucSp> lst = new PagedList<TDanhMucSp>(lstsanpham, pageNumber, pageSize);
            return View(lst);
        }

        [Route("danhmucnguoidung")]
        [Authentication]
        public IActionResult DanhMucNguoiDung(int? page)
        {

            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstNguoiDung = db.TUsers.Where(u => u.LoaiUser == 0).OrderBy(x => x.Username);
            PagedList<TUser> lst = new PagedList<TUser>(lstNguoiDung, pageNumber, pageSize);
            return View(lst);
        }

        [Route("ThemSanPhamMoi")]
        [HttpGet]
        public IActionResult ThemSanPhamMoi()
        {
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            return View();
        }
        [Route("ThemSanPhamMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPhamMoi(TDanhMucSp SanPham)
        {
            if (ModelState.IsValid)
            {
                db.TDanhMucSps.Add(SanPham);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham");
            }
            return View(SanPham);
        }

        [Route("SuaSanPham")]
        [Authentication]
        [HttpGet]
        public IActionResult SuaSanPham(String maSanPham)
        {
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");
            var sanPham = db.TDanhMucSps.Find(maSanPham);
            return View(sanPham);
        }
        [Route("SuaSanPham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(TDanhMucSp SanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(SanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham", "HomeAdmin");
            }
            return View(SanPham);
        }

        [Route("XoaSanPham")]
        [HttpGet]
        public IActionResult XoaSanPham(String maSanPham)
        {
            TempData["Message"] = "";
            var chiTietSanPham = db.TChiTietSanPhams.Where(x => x.MaSp == maSanPham).ToList();
            if (chiTietSanPham.Count > 0)
            {
                TempData["Message"] = "Không thể xóa sản phẩm này vì đã có chi tiết sản phẩm";
                return RedirectToAction("DanhMucSanPham", "HomeAdmin");
            }
            var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSanPham).ToList();
            if (anhSanPham.Any()) db.RemoveRange(anhSanPham);
            db.Remove(db.TDanhMucSps.Find(maSanPham));
            db.SaveChanges();
            TempData["Message"] = "Xóa sản phẩm thành công";
            return RedirectToAction("DanhMucSanPham", "HomeAdmin");

        }

        [Route("ChiTietSanPham")]
        [HttpGet]
        public IActionResult ChiTietSanPham(String maSanPham)
        {
            var sanpham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == maSanPham);
            var anhSanPham = db.TAnhSps.Where(x => x.MaSp == maSanPham).ToList();
            ViewBag.anhSanPham = anhSanPham;
            return View(sanpham);
        }

        [Route("ThemNguoiDung")]
        [HttpGet]
        [Authentication]
        public IActionResult ThemNguoiDung()
        {
            return View();
        }

        [Route("ThemNguoiDung")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemNguoiDung(TUser user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = db.TUsers.FirstOrDefault(u => u.Username == user.Username);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                        return View(user);
                    }

                    if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
                    {
                        ModelState.AddModelError("", "Username and Password are required");
                        return View(user);
                    }

                    user.LoaiUser = 0;
                    db.TUsers.Add(user);
                    db.SaveChanges();

                    TempData["Message"] = "Thêm người dùng thành công";
                    return RedirectToAction("DanhMucNguoiDung");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error occurred while creating user: " + ex.Message);
            }
            return View(user);
        }

        [Route("SuaNguoiDung")]
        [HttpGet]
        [Authentication]
        public IActionResult SuaNguoiDung(string username)
        {
            var user = db.TUsers.Find(username);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Route("SuaNguoiDung")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaNguoiDung(TUser user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Cập nhật người dùng thành công";
                    return RedirectToAction("DanhMucNguoiDung");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!db.TUsers.Any(e => e.Username == user.Username))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(user);
        }

        [Route("XoaNguoiDung")]
        [HttpGet]
        [Authentication]
        public IActionResult XoaNguoiDung(string username)
        {
            var user = db.TUsers.Find(username);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                db.TUsers.Remove(user);
                db.SaveChanges();
                TempData["Message"] = "Xóa người dùng thành công";
            }
            catch (Exception)
            {
                TempData["Message"] = "Không thể xóa người dùng này";
            }
            return RedirectToAction("DanhMucNguoiDung");
        }
    }
}
