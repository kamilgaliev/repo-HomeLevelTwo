using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GameGalievKamil
{
    class Game
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        static BaseObject[] _objs;
        static Timer timer;
        private static Bullet _bullet;
        private static Asteroid[] _asteroids;

        // Свойства
        // Ширина и высота игрового поля
        public static int Width { get; set; }
        public static int Height { get; set; }
        static Game()
        {
        }
        public static void Init(Form form)
        {
            // Графическое устройство для вывода графики            
            Graphics g;
            // Предоставляет доступ к главному буферу графического контекста для текущего приложения
            _context = BufferedGraphicsManager.Current;
            g = form.CreateGraphics();
            // Создаем объект (поверхность рисования) и связываем его с формой
            // Запоминаем размеры формы
            Width = form.ClientSize.Width;
            Height = form.ClientSize.Height;
            form.FormClosing += Form_FormClosing;
            // Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            Load();
            timer = new Timer { Interval = 100 };
            timer.Start();
            timer.Tick += Timer_Tick;
        }

        private static void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
        }

        public static void Load()
        {
            _objs = new BaseObject[30];
            _bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(4, 1));
            _asteroids = new Asteroid[60];
            var rnd = new Random();
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
            }
            for (var i = 0; i < _asteroids.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _asteroids[i] = new Asteroid(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r));
            }

        }

        public static void Draw()
        {
            
            Buffer.Graphics.Clear(Color.Black);
            
            Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(650, 10, 100, 100));
            //Buffer.Render();

            //Buffer.Graphics.Clear(Color.Black);
            Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (Asteroid obj in _asteroids)
                obj.Draw();
            _bullet.Draw();
            

            Buffer.Render();
        }
        
        public static void Update()
        {
            foreach (BaseObject obj in _objs)
                obj.Update();
            var rnd = new Random();

            foreach (Asteroid a in _asteroids)
            {
                
                a.Update();
                //Проверка попадания снаряда по астероиду
                if (a.Collision(_bullet)) 
                { 
                    //Если снарад попал то Воспроизводим системный звук и создаем новый снаряд
                    System.Media.SystemSounds.Hand.Play();
                    _bullet = new Bullet(new Point(0, 200), new Point(5, 0), new Size(4, 1));
                    //После попадания снаряда, астероид уничтожается и создается заново с новыми координатами и размром
                    for (int i = 0; i < _asteroids.Length; i++)
                        if (_asteroids[i] == a)
                        {
                            int r = rnd.Next(5, 50);
                            _asteroids[i] = new Asteroid(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r / 5, r), new Size(r, r));
                        }
                }
            }
            _bullet.Update();
        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }
}
