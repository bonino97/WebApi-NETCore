using Microsoft.EntityFrameworkCore;
using MiPrimerWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApi.Contexts
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet <Autor> Autores { get; set; }
        public DbSet <Libro> Libros { get; set; }
    }
}
