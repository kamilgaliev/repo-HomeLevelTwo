using System;
using System.Windows.Forms;

namespace GameGalievKamil
{
    class Program
    {
        //Галиев Камиль 
        //Переделать виртуальный метод Update в BaseObject в абстрактный и реализовать его в наследниках.
        //Сделать так, чтобы при столкновении пули с астероидом они регенерировались в разных концах экрана.

        static void Main(string[] args)
        {
            Form form = new Form();
            form.Width = 800;
            form.Height = 600;
            Game.Init(form);
            form.Show();
            Game.Draw();
            Application.Run(form);

        }
    }
}
