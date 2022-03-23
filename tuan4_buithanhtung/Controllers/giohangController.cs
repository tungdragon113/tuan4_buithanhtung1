using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using tuan4_buithanhtung.Models;

namespace tuan4_buithanhtung.Controllers
{
    public class giohangController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        public List<giohang> Laygiohang()
        {
            List<giohang> lstgiohang = Session["giohang"] as List<giohang>;
            if (lstgiohang == null)
            {
                lstgiohang = new List<giohang>();
                Session["giohang"] = lstgiohang;
            }
            return lstgiohang;
        }
        public ActionResult Themgiohang(int id, string strURL)
        {
            List<giohang> lstgiohang = Laygiohang();
            giohang sanpham = lstgiohang.Find(n => n.masach == id);
            if (sanpham == null)
            {
                sanpham = new giohang(id);
                lstgiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.isoluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<giohang> lstgiohang = Session["giohang"] as List<giohang>;
            if (lstgiohang != null)
            {
                tsl = lstgiohang.Sum(n => n.isoluong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<giohang> lstgiohang = Session["giohang"] as List<giohang>;
            if (lstgiohang != null)
            {
                tsl = lstgiohang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<giohang> Lstgiohang = Session["giohang"] as List<giohang>;
            if (Lstgiohang != null)
            {
                tt = Lstgiohang.Sum(n => n.dThanhtien);
            }
            return tt;
        }
        public ActionResult giohang()
        {
            List<giohang> Lstgiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(Lstgiohang);
        }
        public ActionResult giohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }
        public ActionResult Xoagiohang(int id)
        {
            List<giohang> lstgiohang = Laygiohang();
            giohang sanpham = lstgiohang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                lstgiohang.RemoveAll(n => n.masach == id);
                return RedirectToAction("giohang");
            }
            return RedirectToAction("giohang");
        }
        public ActionResult Capnhatgiohang(int id, FormCollection collection)
        {
            List<giohang> lstgiohang = Laygiohang();
            giohang sanpham = lstgiohang.SingleOrDefault(n => n.masach == id);
            if (sanpham != null)
            {
                sanpham.isoluong = int.Parse(collection["txtSoluong"].ToString());
            }
            return RedirectToAction("giohang");
        }
        public ActionResult XoaTatCagiohang()
        {
            List<giohang> lstgiohang = Laygiohang();
            lstgiohang.Clear();
            return RedirectToAction("giohang");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["giohang"] == null)
            {
                return RedirectToAction("Index", "Sach");
            }
            List<giohang> lstgiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(lstgiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            Sach s = new Sach();
            List<giohang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;
            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.masach = item.masach;
                ctdh.soluong = item.isoluong;
                ctdh.gia = (decimal)item.giaban;
                s = data.Saches.Single(n => n.masach == item.masach);
                s.soluongton = ctdh.soluong;
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["giohang"] = null;
            return RedirectToAction("xacnhandonhang", "giohang");
        }
        public ActionResult xacnhandonhang()
        {
            return View();
        }
    }
}