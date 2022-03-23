﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace tuan4_buithanhtung.Models
{
    public class giohang
    {
        MyDataDataContext data = new MyDataDataContext();
        public int masach { get; set; }

        [Display(Name = "Tên sách")]
        public string tensach { get; set; }
        [Display(Name = "Ảnh bìa")]
        public string hinh { get; set; }
        [Display(Name = "Giá bán")]
        public Double giaban { get; set; }
        [Display(Name = "Số lượng")]
        public int isoluong { get; set; }
        [Display(Name = "Thành tiền")]
        public Double dThanhtien
        {
            get { return isoluong * giaban; }
        }
        public giohang(int id)
        {
            masach = id;
            Sach sach = data.Saches.Single(n => n.masach == masach);
            tensach = sach.tensach;
            hinh = sach.hinh;
            giaban = Convert.ToDouble(sach.giaban);
            isoluong = 1;
        }


    }
}