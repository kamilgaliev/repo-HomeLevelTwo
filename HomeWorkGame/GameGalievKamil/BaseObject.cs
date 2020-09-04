using System;
using System.Drawing;

namespace GameGalievKamil
{
    public delegate void Message();
    abstract class BaseObject : ICollision
    {
        protected Point Pos;
        protected Point Dir;
        protected Size Size;

        protected BaseObject()
        {
        }

        protected BaseObject(Point pos, Point dir, Size size)
        {
            Pos = pos;
            Dir = dir;
            Size = size;

        }

        public Rectangle Rect => new Rectangle(Pos, Size);

        public bool Collision(ICollision o)
        {
           return o.Rect.IntersectsWith(this.Rect);
        }

        public abstract void Draw();

        public abstract void Update();
    }
}
