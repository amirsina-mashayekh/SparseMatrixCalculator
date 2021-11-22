using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SparseMatrixCalculator
{
    /// <summary>
    /// Interaction logic for MatrixInput.xaml
    /// </summary>
    public partial class MatrixInput : Page
    {
        public MatrixInput()
        {
            InitializeComponent();
            Loaded += MatrixInput_Loaded;
        }

        private void MatrixInput_Loaded(object sender, RoutedEventArgs e)
        {
            if (MatrixGrid.Children.Count == 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    AddMatrixRow();
                    AddMatrixCol();
                }
            }
        }

        private TextBox NewMatrixElement()
        {
            TextBox element = new TextBox()
            {
                Text = "0",
                Width = 40,
                Height = 20,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(2)
            };
            element.GotFocus += MatrixElements_GotFocus;

            return element;
        }

        private void AddMatrixRow()
        {
            MatrixGrid.RowDefinitions.Add(new RowDefinition()
            { Height = new GridLength(1, GridUnitType.Auto) });

            int lastRow = MatrixGrid.RowDefinitions.Count - 1;
            int colsCount = MatrixGrid.ColumnDefinitions.Count;
            for (int i = 0; i < colsCount; i++)
            {
                TextBox textBox = NewMatrixElement();
                MatrixGrid.Children.Add(textBox);
                Grid.SetRow(textBox, lastRow);
                Grid.SetColumn(textBox, i);
            }
            SetMatrixElementsTabIndex();
            RowsCount.Text = MatrixGrid.RowDefinitions.Count.ToString();
        }

        private void AddMatrixCol()
        {
            MatrixGrid.ColumnDefinitions.Add(new ColumnDefinition()
            { Width = new GridLength(1, GridUnitType.Auto) });

            int lastCol = MatrixGrid.ColumnDefinitions.Count - 1;
            int rowsCount = MatrixGrid.RowDefinitions.Count;
            for (int i = 0; i < rowsCount; i++)
            {
                TextBox textBox = NewMatrixElement();
                MatrixGrid.Children.Add(textBox);
                Grid.SetRow(textBox, i);
                Grid.SetColumn(textBox, lastCol);
            }
            SetMatrixElementsTabIndex();
            ColsCount.Text = MatrixGrid.ColumnDefinitions.Count.ToString();
        }

        private void SetMatrixElementsTabIndex()
        {
            int colsCount = MatrixGrid.ColumnDefinitions.Count;
            foreach (TextBox element in MatrixGrid.Children)
            {
                int row = Grid.GetRow(element);
                int col = Grid.GetColumn(element);
                element.TabIndex = row * colsCount + col;
            }
        }

        private void AddColButton_Click(object sender, RoutedEventArgs e)
        {
            AddMatrixCol();
        }

        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            AddMatrixRow();
        }

        private void RemoveColButton_Click(object sender, RoutedEventArgs e)
        {
            int lastCol = MatrixGrid.ColumnDefinitions.Count - 1;
            if (lastCol == 0) { return; }

            int elementsCount = MatrixGrid.Children.Count;
            for (int i = 0; i < elementsCount; i++)
            {
                if (Grid.GetColumn(MatrixGrid.Children[i]) == lastCol)
                {
                    MatrixGrid.Children.RemoveAt(i);
                    i--;
                    elementsCount--;
                }
            }
            MatrixGrid.ColumnDefinitions.RemoveAt(lastCol);
            SetMatrixElementsTabIndex();
        }

        private void RemoveRowButton_Click(object sender, RoutedEventArgs e)
        {
            int lastRow = MatrixGrid.RowDefinitions.Count - 1;
            if (lastRow == 0) { return; }

            int elementsCount = MatrixGrid.Children.Count;
            for (int i = 0; i < elementsCount; i++)
            {
                if (Grid.GetRow(MatrixGrid.Children[i]) == lastRow)
                {
                    MatrixGrid.Children.RemoveAt(i);
                    i--;
                    elementsCount--;
                }
            }
            MatrixGrid.RowDefinitions.RemoveAt(lastRow);
            SetMatrixElementsTabIndex();
        }

        private void MatrixElements_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        public double[,] GetMatrix()
        {
            int rowsCouunt = MatrixGrid.RowDefinitions.Count;
            int colsCount = MatrixGrid.ColumnDefinitions.Count;
            double[,] result = new double[rowsCouunt, colsCount];

            for (int i = 0; i < rowsCouunt; i++)
            {
                for (int j = 0; j < colsCount; j++)
                {
                    TextBox element = MatrixGrid.Children.Cast<TextBox>()
                        .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);

                    result[i, j] = double.Parse(element.Text);
                }
            }

            return result;
        }
    }
}
