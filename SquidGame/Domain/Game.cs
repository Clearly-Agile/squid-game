using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SquidGame.Domain
{
    public class Game
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset StartDateTime { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<Player> Players { get; set; } = new Collection<Player>();
    }
}
