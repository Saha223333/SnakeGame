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
using System.ComponentModel;

namespace SnakeGame
{
    /*
     * Интерфейс создает связь-соглашение (contract), которому
    должны соответствовать все взаимозаменяемые классы.
    Если интерфейс заявляет, что класс, реализующий его, имеет метод A и свойство B, тогда каждый класс,
    реализующий этот интерфейс, должен иметь метод A и свойство B   
     */
    public class DotClass
    {
        public int CoordX;
        public int CoordY;
        public int Score;
        int GridKoef = 20;//множитель определяющий величину ячеек условной сетки

        public MainWindow obj_MainWindow;

        public Ellipse DotShape;
        //переопределяем конструктор точки чтобы сразу в нём задавать параметры
        public DotClass(MainWindow p_MainWindow)
        {
            obj_MainWindow = p_MainWindow;

            Random rand = new Random();

            CoordX = rand.Next(1, 15) * GridKoef;
            CoordY = rand.Next(1, 15) * GridKoef;
            Score = SnakeClass.Length + 1;//кол-во очков, хранящихся в точке зависит от длины змеи
            //описываем эллипс
            //Invoke выполняет указанный делегат в том потоке, которому принадлежит базовый дескриптор окна элемента управления.
            //видимо не получалось добавить эллипс на грид, т.к. он создавался без Invoke т.е. не в потоке где содержится MainWindow
            //а в потоке движения змеи, но пытался добавится в поток главного окна, поэтому вылетала ошибка что владелец другой
            obj_MainWindow.Dispatcher.Invoke(new Action(delegate { DotShape = new Ellipse(); }));
            /*
             Делегат позволяет передать именованный или анонимный код в качестве параметра. Также делегаты выполняют функцию
             * подписки методов на события. Так работают все обработчики событий - через делегты. Они определяют интерфейс
             * взаимодействия между событиями и подписанными на них методами
             */
            obj_MainWindow.Dispatcher.Invoke(new Action(delegate { DotShape.Stroke = System.Windows.Media.Brushes.Black; }));
            obj_MainWindow.Dispatcher.Invoke(new Action(delegate { DotShape.Fill = System.Windows.Media.Brushes.DarkBlue; }));
            obj_MainWindow.Dispatcher.Invoke(new Action(delegate { DotShape.HorizontalAlignment = HorizontalAlignment.Center; }));
            obj_MainWindow.Dispatcher.Invoke(new Action(delegate { DotShape.VerticalAlignment = VerticalAlignment.Center; }));
            obj_MainWindow.Dispatcher.Invoke(new Action(delegate { DotShape.Width = 20; }));
            obj_MainWindow.Dispatcher.Invoke(new Action(delegate { DotShape.Height = 20; }));
            obj_MainWindow.Dispatcher.Invoke(new Action(delegate { DotShape.Margin = new Thickness(CoordX, CoordY, CoordX, CoordY); }));
            //рисуем эллипс
            obj_MainWindow.Dispatcher.Invoke(new Action(delegate { obj_MainWindow.MyCanvas.Children.Add(DotShape); }));
        }
    }
}
