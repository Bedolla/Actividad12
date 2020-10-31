using Actividad.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Actividad.Database
{
    public static class BaseDeDatos
    {
        private static SQLiteAsyncConnection Contexto =>
            new Lazy<SQLiteAsyncConnection>
            (
                () => new SQLiteAsyncConnection
                (
                    Path.Combine
                    (
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Personas.db"
                    ),
                    SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache
                )
            ).Value;

        public static async Task InicializarAsync()
        {
            if (BaseDeDatos.Contexto.TableMappings.All(m => m.MappedType.Name != nameof(Persona)))
                await BaseDeDatos.Contexto
                                 .CreateTablesAsync(CreateFlags.None, typeof(Persona)).ContinueWith(async t =>
                                 {
                                     if ((await BaseDeDatos.ListarAsync<Persona>()).Count == 0)
                                         await BaseDeDatos.Contexto.InsertAllAsync(new List<Persona>
                                         {
                                             new Persona
                                             {
                                                 Nombre = "María Guadalupe Alvarez Negrete",
                                                 Correo = "Lupita@Ucol.mx",
                                                 Telefono = "(312) 000 0001"
                                             },
                                             new Persona
                                             {
                                                 Nombre = "María Enriqueta Rego Rodriguez",
                                                 Correo = "Enri@Ucol.mx",
                                                 Telefono = "(312) 000 0002"
                                             },
                                             new Persona
                                             {
                                                 Nombre = "Fernando Bedolla Valencia",
                                                 Correo = "Fer@Ucol.mx",
                                                 Telefono = "(312) 000 0003"
                                             }
                                         });
                                 });
        }

        public static async Task<List<T>> ListarAsync<T>() where T : new() => await BaseDeDatos.Contexto.Table<T>().ToListAsync();

        public static async Task<int> BorrarAsync<T>(T entidad) => await BaseDeDatos.Contexto.DeleteAsync(entidad);

        public static async Task<int> GuardarAsync<T>(T entidad) where T : Base =>
            entidad.Id.Equals(0) ?
                await BaseDeDatos.Contexto.InsertAsync(entidad) :
                await BaseDeDatos.Contexto.UpdateAsync(entidad);

        public static async Task<bool> NoExisteAsync(Persona persona) =>
            await BaseDeDatos.Contexto.Table<Persona>().FirstOrDefaultAsync
            (p =>
                (
                    (p.Nombre.ToLower() == persona.Nombre.ToLower()) ||
                    (p.Correo.ToLower() == persona.Correo.ToLower())
                )
                &&
                (
                    p.Id != persona.Id
                )
            ) is null;
    }
}
