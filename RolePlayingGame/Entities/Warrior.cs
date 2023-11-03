namespace RolePlayingGame.Entities
{
    public class Warrior : Entity
    {
        public Warrior() 
        {
            this.Strenght = 3;
            this.Agility = 3;
            this.Intelligence = 0;
            this.Range = 1;
            this.Symbol = '@';

            base.SetUp();   
        }
    }
}
