using Actividad.Database;
using Actividad.Models;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Actividad.ViewModels
{
    public class CrearEditarViewModel : BaseViewModel
    {
        private string _botonGuardar;
        private string _informacion;
        private Persona _persona;

        public CrearEditarViewModel(Persona persona)
        {
            if (persona is null || persona.Id.Equals(0))
            {
                this.Limpiar();
            }
            else
            {
                this.Persona = persona;
                this.BotonGuardar = "Editar";
            }

            this.LimpiarCommand = new Command(this.Limpiar);
            this.GuardarCommand = new Command(this.Guardar);
        }

        public CrearEditarViewModel() { }

        public Persona Persona
        {
            get => this._persona;
            set
            {
                this._persona = value;
                this.OnPropertyChanged();
            }
        }

        public string Informacion
        {
            get => this._informacion;
            set
            {
                this._informacion = value;
                this.OnPropertyChanged();
            }
        }

        public string BotonGuardar
        {
            get => this._botonGuardar;
            set
            {
                this._botonGuardar = value;
                this.OnPropertyChanged();
            }
        }

        public ICommand LimpiarCommand { get; set; }
        public ICommand GuardarCommand { get; set; }

        private async void Guardar()
        {
            try
            {
                if
                (
                    String.IsNullOrWhiteSpace(this.Persona.Nombre) ||
                    String.IsNullOrWhiteSpace(this.Persona.Correo) ||
                    String.IsNullOrWhiteSpace(this.Persona.Telefono)
                )
                {
                    this.Informacion = "Debe llenar todos los campos";
                    return;
                }

                if (await BaseDeDatos.NoExisteAsync(this.Persona))
                {
                    if (await BaseDeDatos.GuardarAsync(this.Persona) > 0)
                    {
                        if (this.BotonGuardar.Equals("Crear"))
                        {
                            this.Limpiar();
                            this.Informacion = "Persona creada";
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Aviso", "Persona editada", "Entendido");
                            await Application.Current.MainPage.Navigation.PopAsync();
                        }
                    }
                    else
                    {
                        this.Informacion = "No se puedo guardar a la persona";
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Duplicado", "Ya hay una persona con ese Nombre o Correo.", "Entendido");
                }
            }
            catch (Exception excepcion)
            {
                this.Informacion = excepcion.Message;
            }
        }

        private void Limpiar()
        {
            this.Persona = new Persona
            {
                Id = this.Persona?.Id ?? 0,
                Nombre = String.Empty,
                Correo = String.Empty,
                Telefono = String.Empty
            };

            this.Informacion = String.Empty;
            this.BotonGuardar = this.Persona.Id.Equals(0) ? "Crear" : "Editar";
        }
    }
}
