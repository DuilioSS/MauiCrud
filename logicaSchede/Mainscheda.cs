using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using mauicrud.confgdata;
using mauicrud.DTOs;
using mauicrud.flussodati;
using mauicrud.logicaSchede;
using mauicrud.classe;
using mauicrud.Views;
using System.Collections.ObjectModel;



namespace mauicrud.logicaSchede
{
    public partial class Mainscheda : ObservableObject
    {
        private readonly LavoratoreDb _dbcontesto;
        [ObservableProperty]
        private ObservableCollection<LavoratoreDTO> listalavoratore = new ObservableCollection<LavoratoreDTO>();

        
        public Mainscheda(LavoratoreDb contesto)
        {
            _dbcontesto = contesto;

            MainThread.BeginInvokeOnMainThread(new Action(async () => await Prendere()));

            WeakReferenceMessenger.Default.Register<Lavoratoreposta>(this, (r, m) => {
                Lavoratoremsaggioricevuto(m.Value);
            });
        }

        public async Task Prendere()
        {
            var lista = await _dbcontesto.Lavoratori.ToListAsync();
            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    Listalavoratore.Add(new LavoratoreDTO
                    {
                        Idlavoratore = item.IdLavoratore,
                        Nomecompleto = item.Nomecompleto,
                        Email = item.Email,
                        Stipendio = item.Stipendio,
                        Datacontratto = item.Datacontratto,

                    });
                }
            }
        }


        private void Lavoratoremsaggioricevuto(Lavoratoremessaggio lavoratoremessaggio)
        {
            var lavoratoreDto = lavoratoremessaggio.LavoratoreDto;

            if (lavoratoremessaggio.Percreare)
            {
                Listalavoratore.Add(lavoratoreDto);
            }
            else
            {
                var trovato = Listalavoratore.First(e => e.Idlavoratore == lavoratoreDto.Idlavoratore);

                trovato.Nomecompleto = lavoratoreDto.Nomecompleto;
                trovato.Email = lavoratoreDto.Email;
                trovato.Stipendio = lavoratoreDto.Stipendio;
                trovato.Datacontratto = lavoratoreDto.Datacontratto;

            }



        }
        [RelayCommand]
        private async Task Crea()
        {
            var uri = $"{nameof(Lavoratorepage)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }


        [RelayCommand]
        private async Task Modifica(LavoratoreDTO lavoratoreDto)
        {
            var uri = $"{nameof(Lavoratorepage)}?id={lavoratoreDto.Idlavoratore}";
            await Shell.Current.GoToAsync(uri);
        }


        [RelayCommand]
        private async Task Elimina(LavoratoreDTO lavoratoreDto)
        {
            bool domanda = await Shell.Current.DisplayAlert("mesaggio", "Vuoi eliminare il lavoratore ?", "si", "no");
            if (domanda)
            {
                var trovato = await _dbcontesto.Lavoratori.FirstAsync(e => e.IdLavoratore == lavoratoreDto.Idlavoratore);

                _dbcontesto.Lavoratori.Remove(trovato);
                await _dbcontesto.SaveChangesAsync();
                Listalavoratore.Remove(lavoratoreDto);
            }
        }
    }
}
