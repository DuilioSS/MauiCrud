using mauicrud.classe;
using mauicrud.flussodati;
using Microsoft.EntityFrameworkCore;

namespace mauicrud.confgdata
{
    public class LavoratoreDb : DbContext
    {
       
        public DbSet<Lavoratore> Lavoratori { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conessioneDB = $"Filename ={ConessioneDB.ritonastrada("laboratori.db")}";
            optionsBuilder.UseSqlite(conessioneDB);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Lavoratore>(entity =>
            {
                entity.HasKey(col => col.IdLavoratore);
                entity.Property(col => col.IdLavoratore).IsRequired().ValueGeneratedOnAdd();
            });
        }
    }
}
