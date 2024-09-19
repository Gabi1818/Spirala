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

namespace Al_01_Spirala
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum Direction { LeftUp_RightUp, RightUp_RightDown, RigthDown_LeftDown, LeftDown_LeftUp }
        public MainWindow()
        {
            InitializeComponent();
            isInitialized = true;
        }

        private bool isInitialized = false;

        int l;
        int d;

        int nextL;
        bool[,] spiral;
        Direction direction;
        int startX;
        int startY;

        private void TextBox_TextChangedL(object sender, TextChangedEventArgs e)
        {
            if (isInitialized)
            {
                TextBox pressedBtn = (TextBox)sender;
                string pressedNum = pressedBtn.Text.ToString();


                try
                {
                    l = int.Parse(pressedNum);
                    ErrorBlock.Background = null;
                    ErrorBlock.Text = null;
                }
                catch
                {
                    //není celé číslo
                    ErrorBlock.Background = Brushes.White;
                    ErrorBlock.Text = "Špatně vyplněná hodnota l.";
                }
            }
        }

        private void TextBox_TextChangedD(object sender, TextChangedEventArgs e)
        {
            if (isInitialized)
            {
                TextBox pressedBtn = (TextBox)sender;
                string pressedNum = pressedBtn.Text.ToString();


                try
                {
                    d = int.Parse(pressedNum);
                    ErrorBlock.Background = null;
                    ErrorBlock.Text = null;
                }
                catch
                {
                    //není celé číslo
                    ErrorBlock.Background = Brushes.White;
                    ErrorBlock.Text = "Špatně vyplněná hodnota d.";
                }
            }
        }

        private void Btn_click(object sender, RoutedEventArgs e)
        {
            //není možné vykreslit
            if (l < d || l > 101 || l < 1 || d < 1)
            {
                ErrorBlock.Background = Brushes.White;
                ErrorBlock.Text = "Není možné vykreslit";

                return;
            }



            //mřížku (čtverec) se stranou l
            spiral = new bool[l, l];

            //levý horní roh -> pravý horní roh
            direction = Direction.LeftUp_RightUp;

            //začátek v levém horním rohu
            startX = 0;
            startY = 0;

            nextL = l;


            //1. řádek
            DrawL();

            int stepNum = 0;

            //2. řádek
            //int stepNum = 1;
            //Turn90();
            //CalculateGap(stepNum);
            //DrawL();


            //Stopwatch stopwatchRecursive = new Stopwatch();
            //stopwatchRecursive.Start();
            SpiralToField(stepNum);  
            //stopwatchRecursive.Stop();
            //long elapsedTimeRecursive = stopwatchRecursive.ElapsedMilliseconds;

            //Stopwatch stopwatchLoop = new Stopwatch();
            //stopwatchLoop.Start();
            //SpiralToFieldUsingWhile(stepNum);  
            //stopwatchLoop.Stop();
            //long elapsedTimeLoop = stopwatchLoop.ElapsedMilliseconds;

            //vykresli celé pole
            DrawSpiral();
        }

        private void SpiralToField(int stepNum)
        {
            //"otočit" o 90, odebrat d od l, vykreslit zbytek l
            Turn90();
            CalculateGap(stepNum);
            DrawL();

            //dokud je nextL > d
            if (d >= nextL - d)
                return;

            stepNum++;

            SpiralToField(stepNum);
        }

        private void SpiralToFieldUsingWhile(int stepNum)
        {
            while (true)
            {
                Turn90();
                CalculateGap(stepNum);
                DrawL();

                if (d >= nextL - d)
                    break;

                stepNum++;
            }
        }

        private void Turn90()
        {
            //otoč směr spirály

            //enum Direction { LeftUp_RightUp, RightUp_RightDown, RigthDown_LeftDown, LeftDown_LeftUp }
            if (direction == Direction.LeftUp_RightUp)
                direction = Direction.RightUp_RightDown;
            else if (direction == Direction.RightUp_RightDown)
                direction = Direction.RigthDown_LeftDown;
            else if (direction == Direction.RigthDown_LeftDown)
                direction = Direction.LeftDown_LeftUp;
            else if (direction == Direction.LeftDown_LeftUp)
                direction = Direction.LeftUp_RightUp;
            else
                throw new Exception();
        }

        private void CalculateGap(int stepNum)
        {
            //každý druhý zkrátit o mezeru + 1 (z minulého kola)
            if (stepNum % 2 == 0 && stepNum > 0)
                nextL -= d + 1;
        }

        private void DrawL()
        {
            //projdi pole spiral, označ 'čáru' délky nextL jako true, se startem ve start, ve směru direction 

            switch (direction)
            {
                case Direction.LeftUp_RightUp:
                    for (int x = startX; x < startX + nextL; x++)
                    {
                        spiral[startY, x] = true;
                    }
                    startX += nextL - 1; 
                    break;

                case Direction.RightUp_RightDown:
                    for (int y = startY; y < startY + nextL; y++)
                    {
                        spiral[y, startX] = true;
                    }
                    startY += nextL - 1; 
                    break;

                case Direction.RigthDown_LeftDown:
                    for (int x = startX; x > startX - nextL; x--)
                    {
                        spiral[startY, x] = true;
                    }
                    startX -= nextL - 1;
                    break;

                case Direction.LeftDown_LeftUp:
                    for (int y = startY; y > startY - nextL; y--)
                    {
                        spiral[y, startX] = true;
                    }
                    startY -= nextL - 1; 
                    break;
            }
        }

        private void DrawSpiral()
        {
            //do SpiralGrid udělej mřížku (čtverec se stranou l) a vykresli pole spiral

            SpiralGrid.Children.Clear();
            SpiralGrid.RowDefinitions.Clear();
            SpiralGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < l; i++)
            {
                SpiralGrid.RowDefinitions.Add(new RowDefinition());
                SpiralGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int y = 0; y < l; y++)
            {
                for (int x = 0; x < l; x++)
                {
                    var rectangle = new Border();
                    rectangle.Background = spiral[y, x] ? Brushes.Black : Brushes.White;

                    rectangle.BorderBrush = Brushes.Gray;
                    rectangle.BorderThickness = new Thickness(0.5);

                    Grid.SetRow(rectangle, y);
                    Grid.SetColumn(rectangle, x);
                    SpiralGrid.Children.Add(rectangle);
                }
            }
        }
    }
}