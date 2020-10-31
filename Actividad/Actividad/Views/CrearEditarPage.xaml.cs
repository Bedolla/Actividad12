using Actividad.Models;
using Actividad.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Actividad.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrearEditarPage : ContentPage
    {
        public CrearEditarPage(Persona persona)
        {
            this.InitializeComponent();

            this.BindingContext = new CrearEditarViewModel(persona);
        }
    }
}
