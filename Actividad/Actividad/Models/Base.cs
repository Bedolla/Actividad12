using SQLite;

namespace Actividad.Models
{
    public class Base
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
    }
}
