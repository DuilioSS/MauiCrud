using System.ComponentModel.DataAnnotations;

namespace mauicrud.classe
{
    public class Lavoratore
    {
        [Key]

        public int IdLavoratore { get; set; }

        public string Nomecompleto { get; set; }

        public string Email { get; set; }

        public decimal Stipendio { get; set; }

        public DateTime Datacontratto { get; set; }
    }
}
