using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangasMVVM.Model
{
    public class Manga
    {
        public string Titulo { get; set; } = "";
        public string Genero { get; set; } = "";
        public string Autor { get; set; } = "";
        public int NumTomos { get; set; } 
        public string Sinopsis { get; set; } = "";
        public string Imagen { get; set; } = "";
         

    }
}
