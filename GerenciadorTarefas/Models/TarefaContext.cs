using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorTarefas.Models
{
    class TarefaContext : DbContext
    {
        public DbSet<Tarefa> tarefa { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\.."));                        
            var dbPath = Path.Combine(rootPath, "Data");

            if (!Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);

            optionsBuilder.UseSqlite($"Data Source={Path.Combine(dbPath, "Banco.db")}");
        }
    }
}
