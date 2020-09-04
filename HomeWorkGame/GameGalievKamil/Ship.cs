using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameGalievKamil
{
    class Ship : BaseObject
    {
        private int _energy = 100;
        public int Energy => _energy;

       
        protected Image img = Image.FromFile(@"ship.png");

        /// <summary>
        /// При столкновении с астероидом, энергия снижается
        /// </summary>
        public void EnergyLow(int n)
        {
            _energy -= n;
        }
        /// <summary>
        /// После получения аптечки, энергия добавляется
        /// </summary>
        /// <param name="n"></param>
        public void EnergyUp(int n)
        {
            if(_energy < 100)
                _energy += n;
        }
        public Ship(Point pos, Point dir, Size size) : base(pos, dir, size)
        {
        }
        /// <summary>
        /// Управление кораблем. При нажатии стрелки верх, корабль поднимается
        /// </summary>
        public void Up()
        {
            if (Pos.Y > 0) Pos.Y = Pos.Y - Dir.Y;
        }
        /// <summary>
        /// Управление кораблем. При нажатии стрелки вниз, корабль снижается
        /// </summary>
        public void Down()
        {
            if (Pos.Y < Game.Height - Size.Height) Pos.Y = Pos.Y + Dir.Y;
        }
        /// <summary>
        /// Управление кораблем. При нажатии стрелки ->, корабль идет вперед-направо
        /// </summary>
        public void Right()
        {
            if (Pos.X < (Game.Width - Size.Width)) Pos.X = Pos.X + Dir.X;
        }
        /// <summary>
        /// Управление кораблем. При нажатии стрелки <-, корабль идет назад-налево
        /// </summary>
        public void Left()
        {
            if (Pos.X > 0) Pos.X = Pos.X - Dir.X;
        }
        public static event Message MessageDie;
        public void Die()
        {
            MessageDie?.Invoke();
        }


        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(img, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            
        }
    }
}
