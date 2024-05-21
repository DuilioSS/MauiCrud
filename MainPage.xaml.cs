using mauicrud.logicaSchede;

namespace mauicrud
{
    public partial class MainPage : ContentPage
    {
        

        public MainPage(Mainscheda sccheda)
        {
            InitializeComponent();
            BindingContext = sccheda;
        }

        
    }

}
