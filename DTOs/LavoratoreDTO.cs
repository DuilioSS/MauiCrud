using CommunityToolkit.Mvvm.ComponentModel;

namespace mauicrud.DTOs
{
    public partial class LavoratoreDTO: ObservableObject
    {
        [ObservableProperty]

        public int idlavoratore;

        [ObservableProperty]

        public string nomecompleto;

        [ObservableProperty]

        public string email;

        [ObservableProperty]

        public decimal stipendio;

        [ObservableProperty]

        public DateTime datacontratto;
    }
}
