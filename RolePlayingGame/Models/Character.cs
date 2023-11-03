using System.ComponentModel.DataAnnotations;

namespace RolePlayingGame.Models
{
    public class Character
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public int Strenght { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int Range { get; set; }
        public char Symbol { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Damage { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
