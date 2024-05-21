using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using mauicrud.confgdata;
using mauicrud.DTOs;
using mauicrud.flussodati;

using mauicrud.classe;
namespace mauicrud.logicaSchede
{
    public partial class Lavoratorescheda : ObservableObject, IQueryAttributable
    {
        private readonly LavoratoreDb _dbcontesto;
        [ObservableProperty]

        private LavoratoreDTO lavoratoreDto = new LavoratoreDTO();

        [ObservableProperty]
        private string titolopag;

        private int Idlavoratore;

        [ObservableProperty]
        private bool loadingvisibile = false;

        public Lavoratorescheda(LavoratoreDb contesto)
        {
            _dbcontesto = contesto;
            LavoratoreDto.Datacontratto = DateTime.Now;
        }



        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse(query["id"].ToString());
            Idlavoratore = id;

            if (Idlavoratore == 0)
            {
                Titolopag = "Nuovo lavoratore";
            }
            else
            {
                Titolopag = "Modifica lavoratore";
                Loadingvisibile = true;

                await Task.Run(async () =>
                {
                    var trovato = await _dbcontesto.Lavoratori.FirstAsync(e => e.IdLavoratore == Idlavoratore);
                    LavoratoreDto.Idlavoratore = trovato.IdLavoratore;
                    LavoratoreDto.Nomecompleto = trovato.Nomecompleto;
                    LavoratoreDto.Email = trovato.Email;
                    LavoratoreDto.Stipendio = trovato.Stipendio;
                    LavoratoreDto.Datacontratto = trovato.Datacontratto;

                    MainThread.BeginInvokeOnMainThread(() => { Loadingvisibile = false; });
                });
            }
        }

        [RelayCommand]

        private async Task Salva()
        {
            Loadingvisibile = true;
            Lavoratoremessaggio messaggio = new Lavoratoremessaggio();
            bool isValid = true;
            string errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(LavoratoreDto.Nomecompleto))
            {
                isValid = false;
                errorMessage += "Nome completo is required.\n";
            }
            if (string.IsNullOrWhiteSpace(LavoratoreDto.Email) || !LavoratoreDto.Email.Contains('@'))
            {
                isValid = false;
                errorMessage += "Email is required and must be a valid one.\n";
            }
            if (LavoratoreDto.Stipendio <= 0)
            {
                isValid = false;
                errorMessage += "Stipendio must be greater than zero.\n";
            }
            if (LavoratoreDto.Datacontratto == default)
            {
                isValid = false;
                errorMessage += "Data contratto is required.\n";
            }

            if (!isValid)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Loadingvisibile = false;
                    Application.Current.MainPage.DisplayAlert("Validation Error", errorMessage, "OK");
                });
                return;
            }

            await Task.Run(async () =>
            {
                if (Idlavoratore == 0)
                {
                    var tblavoratore = new Lavoratore
                    {
                        Nomecompleto = LavoratoreDto.Nomecompleto,
                        Email = LavoratoreDto.Email,
                        Stipendio = LavoratoreDto.Stipendio,
                        Datacontratto = LavoratoreDto.Datacontratto,
                    };

                    _dbcontesto.Lavoratori.Add(tblavoratore);
                    await _dbcontesto.SaveChangesAsync();

                    LavoratoreDto.Idlavoratore = tblavoratore.IdLavoratore;
                    messaggio = new Lavoratoremessaggio
                    {
                        Percreare = true,
                        LavoratoreDto = LavoratoreDto
                    };
                }
                else
                {
                    var trovato = await _dbcontesto.Lavoratori.FirstOrDefaultAsync(e => e.IdLavoratore == Idlavoratore);
                    if (trovato != null)
                    {
                        trovato.Nomecompleto = LavoratoreDto.Nomecompleto;
                        trovato.Email = LavoratoreDto.Email;
                        trovato.Stipendio = LavoratoreDto.Stipendio;
                        trovato.Datacontratto = LavoratoreDto.Datacontratto;

                        await _dbcontesto.SaveChangesAsync();

                        messaggio = new Lavoratoremessaggio
                        {
                            Percreare = false,
                            LavoratoreDto = LavoratoreDto
                        };
                    }
                    else
                    {
                        isValid = false;
                        errorMessage = "Lavoratore not found.";
                    }
                }

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    Loadingvisibile = false;
                    if (isValid)
                    {
                        WeakReferenceMessenger.Default.Send(new Lavoratoreposta(messaggio));
                        await Shell.Current.Navigation.PopAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", errorMessage, "OK");
                    }
                });
            });
        }
    }
}
