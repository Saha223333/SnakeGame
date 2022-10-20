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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace SnakeGame
{
    public class SnakeClass
    {
        //экземпляр главного окна, класса главного окна
        MainWindow o_MainWnd;
        //экземпляр точки, класса точки
        DotClass o_Dot;
        DoubleAnimation DA;

        Label LooseLbl;
        public static Boolean Running; //движется ли змея
        public static int Length = 1; //длина
        private int Speed;      //скорость
        public int Scores;      //количество очков
        private double HeadSize = 20; //это размер точки головы. Нужно чтобы в случае изменения размера, поменять
        //только в одном месте
        //структура, описывающая элементы тела змеи
        public enum Directions { DUp, DDown, DLeft, DRight }   //список направлений движения элемента
        public struct SnakeBody
        {
            public double CoordX, CoordY;
            public double OldCoordX, OldCoordY;

            public Ellipse BShape;
            public Directions Direction; //само направление элемента 
        }
        //массив элементов змеи.
        public SnakeBody[] Body = new SnakeBody[0];
        //создаем свой конструктор. У конструктора такое же имя, как и у его класса, а с точки зрения синтаксиса он подобен методу.
        //параметрами являются ссылка на главное окно и ссылка на экземпляр первой точки
        public SnakeClass(MainWindow p_MainWnd, DotClass p_FirstDot)
        {   //экземпляру главного окна присваиваем параметр, который подается при вызове конструктора
            o_MainWnd = p_MainWnd;
            //экземпляру точки присваиваем параметр, который подается при вызове конструктора (первая точка)
            o_Dot = p_FirstDot;
            //задаем длину массива (изначально 1 т.е. только голова)
            Array.Resize(ref Body, Length);
            //Начальные координаты первого элемента (головы) и направление
            Body[0].CoordX = 0; Body[0].CoordY = 0; Body[0].Direction = Directions.DDown;
            //начальная скорость
            Speed = 200;
            //описываем эллипс первого элемента (головы)
            o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[0].BShape = new Ellipse(); }));
            o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[0].BShape.Stroke = System.Windows.Media.Brushes.Black; }));
            o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[0].BShape.Fill = System.Windows.Media.Brushes.Orange; }));
            o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[0].BShape.HorizontalAlignment = HorizontalAlignment.Center; }));
            o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[0].BShape.VerticalAlignment = VerticalAlignment.Center; }));
            o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[0].BShape.Width = HeadSize; }));
            o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[0].BShape.Height = HeadSize; }));
            //рисуем эллипс гпервого элемента (головы)
            o_MainWnd.Dispatcher.Invoke(new Action(delegate { o_MainWnd.MyCanvas.Children.Add(Body[0].BShape); }));
            //создаем анимацию
            o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA = new DoubleAnimation(); }));
            //начальное время анимации. Должно корректироваться по мере ускорения змейки
            o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.Duration = TimeSpan.FromSeconds(0.2); }));
        }
        //метод изменения направления движения
        public void ChangeDirection(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {  //перед тем как задать новое направление, нужно запомнить старое
                case Key.Left: if (Body[0].Direction != Directions.DRight) { Body[0].Direction = Directions.DLeft; }  break;
                case Key.Right: if (Body[0].Direction != Directions.DLeft) { Body[0].Direction = Directions.DRight; } break;
                case Key.Up: if (Body[0].Direction != Directions.DDown) { Body[0].Direction = Directions.DUp; } break;
                case Key.Down: if (Body[0].Direction != Directions.DUp) { Body[0].Direction = Directions.DDown; } break;
                case Key.Escape: SnakeClass.Running = false; break;
            }
        }

        public void Move(object sender)
        {
            while (Running == true)
            {
                Body[0].OldCoordY = Body[0].CoordY; Body[0].OldCoordX = Body[0].CoordX;

                switch (Body[0].Direction)
                {
                    case Directions.DUp:
                        //перед анимацией нужно запомнить предыдущие и последующие координаты
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.From = Body[0].CoordY; }));
                        Body[0].CoordY -= HeadSize;
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.To = Body[0].CoordY; }));
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[0].BShape.BeginAnimation(Canvas.TopProperty, DA); }));
                        break;
                    case Directions.DDown:
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.From = Body[0].CoordY; }));
                        Body[0].CoordY += HeadSize;
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.To = Body[0].CoordY; }));
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[0].BShape.BeginAnimation(Canvas.TopProperty, DA); }));
                        break;
                    case Directions.DLeft:
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.From = Body[0].CoordX; }));
                        Body[0].CoordX -= HeadSize;
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.To = Body[0].CoordX; }));
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[0].BShape.BeginAnimation(Canvas.LeftProperty, DA); }));
                        break;
                    case Directions.DRight:
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.From = Body[0].CoordX; }));
                        Body[0].CoordX += HeadSize;
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.To = Body[0].CoordX; }));
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[0].BShape.BeginAnimation(Canvas.LeftProperty, DA); }));
                        break;
                }
                //здесь описано как змея съедает точку
                if (o_Dot.CoordX == Body[0].CoordX && o_Dot.CoordY == Body[0].CoordY)
                {
                    //перед созданием новой точки надо сначала удалить старую
                    DotClass tmpDot = o_Dot;
                    //удаляем сам шейп
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { o_MainWnd.MyCanvas.Children.Remove(o_Dot.DotShape); }));
                    //удаляем ссылку на точку чтобы сборщик мусора потом прибрался
                    o_Dot = null;

                    Scores = Scores + tmpDot.Score; //увеличиваются очки на величину очков, хранящихся в точке   
                    Speed -= 5; //увеличивается скорость
                    //нужно подстраивать значение длительности анимации под скорость потока чтобы точка не дергалась при анимации
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.Duration -= TimeSpan.FromSeconds(0.005); }));
                    //увеличивается длина змеи
                    Length += 1;
                    //меняем длину массива элементов тела
                    Array.Resize(ref Body, Length);
                    //добавляем новый элемент тела
                    Body[Length - 1] = new SnakeBody();
                    //
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[Length - 1].CoordX = tmpDot.CoordX; }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[Length - 1].CoordY = tmpDot.CoordY; }));
                    //здесь опять описывается эллипс элемента тела как и элемент головы (хорошо бы создать шаблон, чтобы  не описывать вручную
                    //каждый раз
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[Length - 1].BShape = new Ellipse(); }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[Length - 1].BShape.Stroke = System.Windows.Media.Brushes.Black; }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[Length - 1].BShape.Fill = System.Windows.Media.Brushes.Red; }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[Length - 1].BShape.HorizontalAlignment = HorizontalAlignment.Center; }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[Length - 1].BShape.VerticalAlignment = VerticalAlignment.Center; }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[Length - 1].BShape.Width = HeadSize; }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[Length - 1].BShape.Height = HeadSize; }));
                    //рисуем эллипс элемента
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { o_MainWnd.MyCanvas.Children.Add(Body[Length - 1].BShape); }));
                    //сразу ставим его на место съеденной точки, чтобы он сразу побежал за последним элементом
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { Canvas.SetLeft(Body[Length - 1].BShape, Body[Length - 1].OldCoordX); }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { Canvas.SetTop(Body[Length - 1].BShape, Body[Length - 1].OldCoordY); }));

                    DotClass NewDot = new DotClass(o_MainWnd);
                    o_Dot = NewDot;

                    //Invoke выполняет указанный делегат в том потоке, которому принадлежит базовый дескриптор окна элемента управления.
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { o_MainWnd.SnakeScoresLabel.Content = Scores; }));
                }
                //здесь описано достижение головой змейки краёв канваса
                o_MainWnd.Dispatcher.Invoke(new Action(delegate
                {
                    //если касается правой границы
                    if (Body[0].CoordX == o_MainWnd.MyCanvas.Width - HeadSize && Body[0].Direction == Directions.DRight)
                    {
                        Body[0].CoordX = 0; //RRight = true;
                    }
                    //если касается нижней границы
                    if (Body[0].CoordY == o_MainWnd.MyCanvas.Height - HeadSize && Body[0].Direction == Directions.DDown)
                    {
                        Body[0].CoordY = 0; //RDown = true;
                    }
                    //если касается левой границы
                    if (Body[0].CoordX == 0 && Body[0].Direction == Directions.DLeft)
                    {
                        Body[0].CoordX = o_MainWnd.MyCanvas.Width - HeadSize; //RLeft = true;
                    }
                    //если касается верхней границы
                    if (Body[0].CoordY == 0 && Body[0].Direction == Directions.DUp)
                    {
                        Body[0].CoordY = o_MainWnd.MyCanvas.Height - HeadSize; //RUp = true;
                    }
                }));
                Thread.Sleep(Speed);
                Repaint();
            }

        }

        public void Repaint()
        {
            for (int i = 1; i < Body.Length; i++)
            {
                //Body[i].Direction = Body[i - 1].Direction;   
                //прежде чем двигать запоминаем текущие координаты
                Body[i].OldCoordX = Body[i].CoordX; Body[i].OldCoordY = Body[i].CoordY;
                //назначаем старые координаты ведущего узла
                Body[i].CoordX = Body[i - 1].OldCoordX; Body[i].CoordY = Body[i - 1].OldCoordY;
                o_MainWnd.Dispatcher.Invoke(new Action(delegate { Canvas.SetLeft(Body[i].BShape, Body[i].CoordX); }));
                o_MainWnd.Dispatcher.Invoke(new Action(delegate { Canvas.SetTop(Body[i].BShape, Body[i].CoordY); }));
                //проверка не съела ли змея саму себя
                if (Body[0].CoordX == Body[i].CoordX && Body[0].CoordY == Body[i].CoordY)                 
                {
                   Running = false;
                    /*
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { LooseLbl = new Label(); }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { LooseLbl.Height = 40; }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { LooseLbl.Width = 40; }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { LooseLbl.Name = "Label1"; }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { LooseLbl.Content = "Лабел"; }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { LooseLbl.Margin = new Thickness(0, 0, 0, 0); }));
                    o_MainWnd.Dispatcher.Invoke(new Action(delegate { LooseLbl.Visibility = Visibility.Visible; }));
                     
                     */
                }
                //анимация
               /* 
                switch (Body[i].Direction)
                {
                    case Directions.DUp:
                        
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.From = Body[i].OldCoordY; }));

                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.To = Body[i].CoordY; }));
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[i].BShape.BeginAnimation(Canvas.TopProperty, DA); }));

                        break;
                    case Directions.DDown:
                       
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.From = Body[i].OldCoordY; }));
 
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.To = Body[i].CoordY; }));
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[i].BShape.BeginAnimation(Canvas.TopProperty, DA); }));

                        break;
                    case Directions.DLeft:
                        
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.From = Body[i].OldCoordX; }));

                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.To = Body[i].CoordX; }));
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[i].BShape.BeginAnimation(Canvas.LeftProperty, DA); }));

                        break;
                    case Directions.DRight:
                      
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.From = Body[i].CoordX; }));

                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { DA.To = Body[i - 1].CoordX; }));
                        o_MainWnd.Dispatcher.Invoke(new Action(delegate { Body[i].BShape.BeginAnimation(Canvas.LeftProperty, DA); }));

                        break;
                }
               */ 
                // Body[i].OldCoordX = Body[i - 1].CoordX; Body[i].OldCoordY = Body[i - 1].CoordY;
                  //Body[i].Direction = Body[i - 1].Direction;    
            }

        }
    }
}
