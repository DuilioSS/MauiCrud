using mauicrud.logicaSchede;

namespace mauicrud.Views;

public partial class Lavoratorepage : ContentPage
{
	public Lavoratorepage(Lavoratorescheda scheda)
	{
		
        InitializeComponent();
        BindingContext = scheda;
    }
}