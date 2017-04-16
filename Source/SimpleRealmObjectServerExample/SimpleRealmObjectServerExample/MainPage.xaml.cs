using SimpleRealmObjectServerExample.ViewModels;
using Xamarin.Forms;

namespace SimpleRealmObjectServerExample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            BindingContext = new MainPageViewModel();
            InitializeComponent();
        }
    }
}
