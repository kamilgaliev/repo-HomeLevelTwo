using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameGalievKamil
{
    public partial class SplashScreen : Form
    {
        private static BufferedGraphicsContext _context;
        public static BufferedGraphics Buffer;
        static BaseObject[] _objs;

        public static int Width { get; set; }
        public static int Height { get; set; }

        public SplashScreen()
        {
            InitializeComponent();

            
        }


        public static void DrawStart()
        {

            Buffer.Graphics.Clear(Color.Black);

            Buffer.Graphics.FillEllipse(Brushes.Wheat, new Rectangle(650, 10, 100, 100));
            Buffer.Render();

            //Buffer.Graphics.Clear(Color.Black);
            //foreach (BaseObject obj in _objs)
            //    obj.DrawStart();
            //Buffer.Render();
        }

        public static void UpdateStart()
        {
            //foreach (BaseObject obj in _objs)
            //    obj.UpdateStart();
        }
        private static void Timer_Tick(object sender, EventArgs e)
        {
            DrawStart();
            UpdateStart();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            
            Form form = new Form();
            form.Width = 800;
            form.Height = 600;
            Game.Init(form);
            form.Show();
            Game.Draw();
            form.Hide();
            form.ShowDialog();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            //// Графическое устройство для вывода графики            
            //Graphics g;
            //// Предоставляет доступ к главному буферу графического контекста для текущего приложения
            //_context = BufferedGraphicsManager.Current;
            //g = this.CreateGraphics();
            //// Создаем объект (поверхность рисования) и связываем его с формой
            //// Запоминаем размеры формы
            //Width = this.Size.Width;
            //Height = this.Size.Height;
            //// Связываем буфер в памяти с графическим объектом, чтобы рисовать в буфере
            //Buffer = _context.Allocate(g, new Rectangle(0, 0, Width, Height));
            
            

            //_objs = new BaseObject[20];
            //for (int i = 0; i < _objs.Length; i++)
            //    _objs[i] = new BaseObject(new Point(600, i * 20), new Point(-i, -i), new Size(10, 10));
         

            //Timer timer = new Timer { Interval = 100 };
            //timer.Start();
            //timer.Tick += Timer_Tick;
        }
    }
}
