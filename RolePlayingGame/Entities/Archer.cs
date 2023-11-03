namespace RolePlayingGame.Entities
{
    public class Archer : Entity
    {
        public Archer() 
        {
            this.Strenght = 2;
            this.Agility = 4;
            this.Intelligence = 0;
            this.Range = 2;
            this.Symbol = '#';

            base.SetUp();   
        }
    }
}
