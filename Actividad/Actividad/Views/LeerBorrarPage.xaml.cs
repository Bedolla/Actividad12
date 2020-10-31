using Actividad.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Actividad.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeerBorrarPage : ContentPage
    {
        public LeerBorrarPage()
        {
            this.InitializeComponent();

            this.LeerBorrarViewModel = new LeerBorrarViewModel();
            this.BindingContext = this.LeerBorrarViewModel;
        }

        private LeerBorrarViewModel LeerBorrarViewModel { get; }

        protected override void OnAppearing() => this.LeerBorrarViewModel.ListarPersonas();
    }
}
