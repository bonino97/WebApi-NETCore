using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApi.Model
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int AutorId { get; set; }
    }
}
