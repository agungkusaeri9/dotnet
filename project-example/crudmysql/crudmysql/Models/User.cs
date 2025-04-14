using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace crudmysql.Models
{
    public class User
    {
        [Key]
        [Column("id")] // Mapping ke kolom "id"
        public int Id { get; set; }

        [MaxLength(100)]
        [Column("name")] // Kolom "name"
        public string Name { get; set; }

        [MaxLength(100)]

        [Column("email")] // Kolom "email"
        public string Email { get; set; }

        [MaxLength(100)]
        [Column("password")] // Kolom "password"
        public string Password { get; set; }
    }
}
