using System;
using System.Drawing;

namespace GameGalievKamil
{
    class BaseObject
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;
        protected string URLImage;
        public BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;
        }
        public virtual void Draw()
        {
            URLImage = @"stars.png";
            Game.Buffer.Graphics.DrawImage(Image.FromFile(URLImage), Pos.X, Pos.Y, Size.Width+20, Size.Height+20);
            Game.Buffer.Graphics.DrawEllipse(Pens.Brown, Pos.X - 10, Pos.Y * 3, Size.Width, Size.Height);
            
        }

        public virtual void DrawStart()
        {
            URLImage = @"stars.png";
            SplashScreen.Buffer.Graphics.DrawImage(Image.FromFile(URLImage), Pos.X, Pos.Y, Size.Width + 20, Size.Height + 20);
            SplashScreen.Buffer.Graphics.DrawEllipse(Pens.Brown, Pos.X - 10, Pos.Y * 3, Size.Width, Size.Height);

        }
        public virtual void Update()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > Game.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > Game.Height) Dir.Y = -Dir.Y;
        }
        public virtual void UpdateStart()
        {
            Pos.X = Pos.X + Dir.X;
            Pos.Y = Pos.Y + Dir.Y;
            if (Pos.X < 0) Dir.X = -Dir.X;
            if (Pos.X > SplashScreen.Width) Dir.X = -Dir.X;
            if (Pos.Y < 0) Dir.Y = -Dir.Y;
            if (Pos.Y > SplashScreen.Height) Dir.Y = -Dir.Y;
        }
    }
}
