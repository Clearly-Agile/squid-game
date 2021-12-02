using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SquidGame.Domain
{
    public class Player
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Number { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Game> GamesPlayed { get; set; } = new Collection<Game>();
    }
}
