using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiPrimerWebApi.Contexts;
using MiPrimerWebApi.Entities;
using MiPrimerWebApi.Helpers;
using MiPrimerWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApi.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    public class AutoresController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("/listarAutores")] //GET /listarAutores 
        [HttpGet("listarAutores")]  //GET /api/autores/listarAutores
        [HttpGet]
        [ServiceFilter(typeof(MiFiltroDeAccion))]
        /* Puedo ignorar la ruta definida por defecto arriba (api/controller), agregando una barra adelante del nombre definido en cada metodo, 
         * Ej: /listarAutores eliminaria el localhost:4444/api/autores/listarAutores y quedaria definido directamente como localhost:4444/listarAutores.
         Y si queremos ambos endpoints solo agregamos ambos HttpGet de las dos maneras comentadas. */

        public async Task<ActionResult<IEnumerable<AutorDTO>>> GetAsync()
        {
            var autores = await context.Autores.ToListAsync();
            var autoresDTO = mapper.Map<List<AutorDTO>>(autores);
            return autoresDTO;

            //return context.Autores.Include(x=>x.Libros).ToList();
        }


        
        [HttpGet("{id}/{nombre?}", Name="ObtenerAutor")] //GET /api/autores/5/juan?
        /*Puedo enviar tantos parametros como desee por la URL (opcionales o no).
        Si quiero ponerle un valor por defecto solo hago {nombre=valor}
        */
        public async Task<ActionResult<AutorDTO>> Get(int id, string nombre)
        {
            var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            var autorDTO = mapper.Map<AutorDTO>(autor); //AUTOMAPPER MAPEANDO AutorDTO. (TAMBIEN SE ENCARGA DE MAPEAR LAS LISTAS ENTIDADES QUE ESTEN DENTRO DE LA ENTIDAD)

            return autorDTO;
        }
        
        [HttpPost]        
        public ActionResult Post([FromBody] Autor autor)
        {
            if(ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            context.Autores.Add(autor);
            context.SaveChanges();
            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autor);
        
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Autor value)
        {
            // Esto no es necesario en asp.net core 2.1
            // if (ModelState.IsValid){

            // }

            if (id != value.Id)
            {
                return BadRequest();
            }

            context.Entry(value).State = EntityState.Modified;
            context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Autor> Delete(int id)
        {
            var autor = context.Autores.FirstOrDefault(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }

            context.Autores.Remove(autor);
            context.SaveChanges();
            return autor;
        }

        [HttpGet("primerAutor")]
        public ActionResult<Autor> GetPrimerAutor()
        {
            return context.Autores.FirstOrDefault();
        }
    }
}
