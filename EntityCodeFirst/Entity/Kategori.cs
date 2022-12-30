﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityCodeFirst.Entity
{
    public class Kategori
    {
        [Key]
        public int KategoriId { get; set; }
        public string KategoriAdi { get; set; }
        public ICollection<Urunler> Urunlers { get; set; } // Bir kategori birden fazla üründe yer alabilir
    }
}
