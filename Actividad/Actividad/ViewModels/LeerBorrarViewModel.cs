using Actividad.Database;
using Actividad.Extensions;
using Actividad.Models;
using Actividad.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Actividad.ViewModels
{
    public class LeerBorrarViewModel : BaseViewModel
    {
        private string _buscado;
        private string _informacion;
        private ObservableCollection<Persona> _personas;
        private Persona _seleccionada;

        public LeerBorrarViewModel()
        {
            this.AgregarCommand = new Command(this.Agregar);
            this.BorrarCommand = new Command<Persona>(this.Borrar);
            this.EditarCommand = new Command<Persona>(this.Editar);
            this.BuscarCommand = new Command<string>(this.Buscar);
        }

        public ObservableCollection<Persona> Personas
        {
            get => this._personas;
            set
            {
                this._personas = value;
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

        public Persona Seleccionada
        {
            get => this._seleccionada;
            set
            {
                this._seleccionada = value;
                if (value != null) this.Editar(value);
            }
        }

        public string Buscado
        {
            get => this._buscado;
            set
            {
                this._buscado = value;
                if (String.IsNullOrWhiteSpace(value)) this.Buscar(value);
                this.OnPropertyChanged();
            }
        }

        public ICommand AgregarCommand { get; set; }
        public ICommand BorrarCommand { get; set; }
        public ICommand EditarCommand { get; set; }
        public ICommand BuscarCommand { get; set; }

        public async void ListarPersonas()
        {
            try
            {
                List<Persona> personas = await BaseDeDatos.ListarAsync<Persona>();

                this.Personas = new ObservableCollection<Persona>();

                if ((personas != null) && (personas.Count > 0))
                {
                    personas
                        .OrderBy(p => p.Nombre)
                        .ThenBy(p => p.Correo)
                        .ToList()
                        .ForEach(p => this.Personas.Add(new Persona
                        {
                            Id = p.Id,
                            Nombre = p.Nombre,
                            Correo = p.Correo,
                            Telefono = p.Telefono
                        }));

                    this.Informacion = $"{personas.Count} {(personas.Count.Equals(1) ? "persona encontrada" : "personas encontradas")}";
                }
                else
                {
                    this.Informacion = "No hay personas, agrégue una";
                }
            }

            catch (Exception excepcion)
            {
                this.Informacion = excepcion.Message;
            }
        }

        private async void Agregar() => await Application.Current.MainPage.Navigation.PushAsync(new CrearEditarPage(null));

        private async void Borrar(Persona persona)
        {
            if (!await Application.Current.MainPage.DisplayAlert("Borrar", $"¿Realmente desea borrar a {persona.Nombre}?", "Sí", "No")) return;

            try
            {
                if (await BaseDeDatos.BorrarAsync(persona) != 1) this.Informacion = "No se puedo borrar a la persona";

                this.ListarPersonas();
            }

            catch (Exception excepcion)
            {
                this.Informacion = excepcion.Message;
            }
        }

        private async void Editar(Persona p) => await Application.Current.MainPage.Navigation.PushAsync(new CrearEditarPage(p));

        private async void Buscar(string consulta)
        {
            if (String.IsNullOrWhiteSpace(consulta))
            {
                this.ListarPersonas();
            }
            else
            {
                consulta = consulta.RemoverDiacriticos().ToLower();

                this.Personas = new ObservableCollection<Persona>
                (
                    (await BaseDeDatos.ListarAsync<Persona>())
                    .Where
                    (p =>
                        p.Nombre.RemoverDiacriticos().ToLower().Contains(consulta) ||
                        p.Correo.RemoverDiacriticos().ToLower().Contains(consulta) ||
                        p.Telefono.RemoverDiacriticos().ToLower().Contains(consulta)
                    )
                    .OrderBy(p => p.Nombre)
                    .ThenBy(p => p.Correo)
                    .ToList()
                );

                this.Informacion = $"{this.Personas.Count} {(this.Personas.Count.Equals(1) ? "persona encontrada" : "personas encontradas")}";
            }
        }
    }
}
