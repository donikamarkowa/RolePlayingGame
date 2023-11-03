namespace RolePlayingGame.Entities
{
    public class Monster : Entity
    {
        public Monster() 
        { 
            this.Strenght = new Random().Next(1, 4);
            this.Agility = new Random().Next(1, 4);
            this.Intelligence = new Random().Next(1, 4);
            this.Range = 1;
            this.Symbol = '!';

            base.SetUp();   
        }
    }
}
