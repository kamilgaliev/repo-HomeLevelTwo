﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace GameGalievKamil
{
    class Game
    {
        /// <summary>
        /// ActionMes - вывод в консоль
        /// </summary>
        public static event Action<string> ActionMes;
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        static BaseObject[] _objs;
        private static Timer _timer = new Timer();
        public static int score = 0;
        public static Random Rnd = new Random();
        private static Bullet _bullet;
        private static HealthBox _health;
        private static Asteroid[] _asteroids;
        private static Ship _ship = new Ship(new Point(10, 400), new Point(5, 5), new Size(30, 30));

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
            ActionMes += Game_ActionMes;
            Load();
            _timer.Interval= 100;
            _timer.Start();
            _timer.Tick += Timer_Tick;
            form.KeyDown += Form_KeyDown;
            Ship.MessageDie += Finish;
            
        }

        

        /// <summary>
        /// Управление кораблем
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey) _bullet = new Bullet(new Point(_ship.Rect.X + 15, _ship.Rect.Y + 15), new Point(4, 0), new Size(4, 1));
            if (e.KeyCode == Keys.Up) _ship.Up();
            if (e.KeyCode == Keys.Down) _ship.Down();
            if (e.KeyCode == Keys.Right) _ship.Right();
            if (e.KeyCode == Keys.Left) _ship.Left();

        }

        private static void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            ActionMes?.Invoke($"{DateTime.Now} - Форма закрыта");
            _timer.Stop();
        }

        public static void Load()
        {
            _objs = new BaseObject[30];
            //_bullet = new Bullet(new Point(0, 415), new Point(5, 0), new Size(4, 1));
            
            _asteroids = new Asteroid[10];
            var rnd = new Random();
            _health = new HealthBox(new Point(1000, rnd.Next(0, Game.Height)), new Point(-10, 10), new Size(40, 40));
            for (var i = 0; i < _objs.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _objs[i] = new Star(new Point(1000, rnd.Next(0, Game.Height)), new Point(-r, r), new Size(3, 3));
            }
            for (var i = 0; i < _asteroids.Length; i++)
            {
                int r = rnd.Next(5, 50);
                _asteroids[i] = new Asteroid(new Point(1000, rnd.Next(0, Game.Height - r)), new Point(-r / 5, r), new Size(r, r));
            }
            ActionMes?.Invoke($"{DateTime.Now} - Запуск игры");
        }

        public static void Draw()
        {
            
            Buffer.Graphics.Clear(Color.Black);
            
            Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(650, 10, 100, 100));
            //Buffer.Render();

            //Buffer.Graphics.Clear(Color.Black);
            //Buffer.Graphics.Clear(Color.Black);
            foreach (BaseObject obj in _objs)
                obj.Draw();
            foreach (Asteroid obj in _asteroids)
                obj?.Draw();
            _bullet?.Draw();
            _health?.Draw();
            _ship?.Draw();
            if (_ship != null)
                Buffer.Graphics.DrawString($"Energy:   {_ship.Energy}   \nScore:  {score}", SystemFonts.DefaultFont, Brushes.White, 0, 0);
            
            

            Buffer.Render();
        }
        
        public static void Update()
        {
            foreach (BaseObject obj in _objs) obj.Update();
            _bullet?.Update();
            _health.Update();
            for (var i = 0; i < _asteroids.Length; i++)
            {
                var rnd = new Random();
                if (_asteroids[i] == null) 
                    continue;
                _asteroids[i].Update();
                if (_bullet != null && _bullet.Collision(_asteroids[i]))
                {
                    System.Media.SystemSounds.Hand.Play();
                    score++;
                    ActionMes?.Invoke($"{DateTime.Now} - Ракета уничтожила астероид. Счет: {score}");
                    int r = rnd.Next(5, 50);
                    _asteroids[i] = new Asteroid(new Point(1000, rnd.Next(0, Game.Height-r)), new Point(-r / 5, r), new Size(r, r));
                    _bullet = null;
                    continue;
                }
                if (!_ship.Collision(_asteroids[i]))
                    continue;
                else
                {
                    _ship?.EnergyLow(rnd.Next(1, 10));
                    ActionMes?.Invoke($"{DateTime.Now} - Корабль получил урон. Энергия: {_ship.Energy}");
                    System.Media.SystemSounds.Asterisk.Play();
                    int r = rnd.Next(5, 50);
                    _asteroids[i] = new Asteroid(new Point(1000, rnd.Next(0, Game.Height - r)), new Point(-r / 5, r), new Size(r, r));
                    if (_ship.Energy <= 0) _ship?.Die();

                }
            }
            UpdateHealth();


        }
        /// <summary>
        /// Получение аптечки
        /// </summary>
        private static void UpdateHealth()
        {
            var rnd = new Random();
            if (_bullet != null && _bullet.Collision(_health))
            {
                System.Media.SystemSounds.Hand.Play();
                _ship?.EnergyUp(_health.GetHealth);
                ActionMes?.Invoke($"{DateTime.Now} - Ракета попала в аптечку. Энергия: {_ship.Energy}");
                _health = new HealthBox(new Point(1000, rnd.Next(0, Game.Height - 40)), new Point(-10, 10), new Size(40, 40));

                _bullet = null;
            }
            if (_ship.Collision(_health))
            {
                System.Media.SystemSounds.Hand.Play();
                _ship?.EnergyUp(_health.GetHealth);
                ActionMes?.Invoke($"{DateTime.Now} - Корабль задел аптечку. Энергия: {_ship.Energy}");
                _health = new HealthBox(new Point(1000, rnd.Next(0, Game.Height - 40)), new Point(-10, 10), new Size(40, 40));
            }
        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
        /// <summary>
        /// Событие при уничтожении корабля астероидами
        /// </summary>
        public static void Finish()
        {
            _timer.Stop();
            Buffer.Graphics.DrawString("Вы проиграли!", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 100, 200);
            ActionMes?.Invoke($"{DateTime.Now} - Игра окончена");
            Buffer.Render();
        }
        /// <summary>
        /// Обработка для события ActionMes
        /// </summary>
        /// <param name="obj">Текст</param>
        private static void Game_ActionMes(string obj)
        {
            Console.WriteLine(obj);
            StreamWriter sw = File.AppendText("log.txt");
            sw.WriteLine(obj);
            sw.Close();
        }
    }
}
