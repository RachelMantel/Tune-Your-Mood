using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuneYourMood.Core.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime DateRegistration { get; set; }

        public List<SongEntity> SongList { get; set; }

        public ICollection<RoleEntity>? Roles { get; set; }

        public UserEntity()
        {

        }
    }
}
