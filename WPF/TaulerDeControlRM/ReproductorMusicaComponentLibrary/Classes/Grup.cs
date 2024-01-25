﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReproductorMusicaComponentLibrary.Classes
{
    public class Grup
    {
        public string Nom { get; set; }
        public int? Any { get; set; }
        public List<Music> LMusics { get; set; }
        public List<Tocar> LTocar { get; set; }
    }
}