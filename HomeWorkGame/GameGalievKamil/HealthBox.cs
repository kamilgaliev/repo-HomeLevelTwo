using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameGalievKamil
{
    class HealthBox : BaseObject
    {
        private int _getHealth = 1;
        public int GetHealth => _getHealth;
        protected Image img = Image.FromFile(@"health.png");
        public HealthBox (Point pos, Point dir, Size size) : base(pos, dir, size)
        {

        }
        public override void Draw()
        {
            Game.Buffer.Graphics.DrawImage(img, Pos.X, Pos.Y, Size.Width, Size.Height);
        }

        public override void Update()
        {
            Pos.X = Pos.X + Dir.X;
            if (Pos.X < 0) Pos.X = Game.Width + Size.Width;
        }
    }
}
