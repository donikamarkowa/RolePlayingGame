using System.Runtime.CompilerServices;

namespace RolePlayingGame.Entities
{
    public class Mage : Entity
    {
        public Mage() 
        {
            this.Strenght = 2;
            this.Agility = 1;
            this.Intelligence = 3;
            this.Range = 3;
            this.Symbol = '*';

            base.SetUp();   
        }
    }
}
