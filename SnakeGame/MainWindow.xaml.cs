using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
           
    public partial class MainWindow : Window
    {
        SnakeClass MySnake;      

        public MainWindow()
        {
            InitializeComponent();

            //создаем экземпляр первой точки
            DotClass FirstDot = new DotClass(this);
            //создаем экземпляр змеи
            MySnake = new SnakeClass(this, FirstDot);
            //поток движения змеи
            Thread MoveThread = new Thread(MySnake.Move);
            MoveThread.SetApartmentState(ApartmentState.STA);
            MoveThread.Start();
            //запускаем змею
            SnakeClass.Running = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
           MySnake.ChangeDirection(this, e);        
        }
    }
}
