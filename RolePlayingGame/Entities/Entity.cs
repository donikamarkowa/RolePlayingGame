namespace RolePlayingGame.Entities
{
    public abstract class Entity
    {
        public int Strenght { get; set; }
        public int Agility { get; set; }
        public int Intelligence { get; set; }
        public int Range { get; set; }
        public char Symbol { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
        public int Damage { get; set; }

        public void SetUp()
        {
            this.Health = this.Strenght * 5;
            this.Mana = this.Intelligence * 3;
            this.Damage = this.Agility * 2;
        }
    }
}
