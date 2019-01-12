using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FIGapi.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }

        public List<Player> Players {get; set;} 

    }

    public class Player
    {
        private int _teamId;

        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        //Foriegn Key
        public int TeamId { get => _teamId; set => _teamId = value; }
        // Navigation property
        public Team Team { get; set; }
    }
}
