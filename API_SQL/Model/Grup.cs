using System.ComponentModel.DataAnnotations;

namespace dymj.ReproductorMusica {
    public partial class Grup {
        [Key]
        public string Nom{get; set;}
        public ICollection<Music> Musics{get; set;}
    }
}