using SparseMatrixCalculator.SparseUtil;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SparseMatrixCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        double[,] MatrixA
        {
            get
            {
                return (MatrixAPage.NavigationService.Content as MatrixInput).GetMatrix();
            }
        }

        double[,] MatrixB
        {
            get
            {
                return (MatrixBPage.NavigationService.Content as MatrixInput).GetMatrix();
            }
        }


        private TextBox NewResultCell(double value)
        {
            return new TextBox()
            {
                Text = value.ToString(),
                IsReadOnly = true,
                TextAlignment = TextAlignment.Center
            };
        }

        private void ShowResult(SparseMatrix result)
        {
            ResultSparse.Children.Clear();
            ResultSparse.RowDefinitions.Clear();

            ResultSparse.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(1, GridUnitType.Auto)
            });

            ResultSparse.Children.Add(new TextBlock()
            {
                Text = "Row",
                TextAlignment = TextAlignment.Center
            });
            ResultSparse.Children.Add(new TextBlock()
            {
                Text = "Column",
                TextAlignment = TextAlignment.Center
            });
            ResultSparse.Children.Add(new TextBlock()
            {
                Text = "Value",
                TextAlignment = TextAlignment.Center
            });

            for (int i = 0; i < 3; i++)
            {
                Grid.SetColumn(ResultSparse.Children[i], i);
                Grid.SetRow(ResultSparse.Children[i], 0);
            }

            for (int i = 0; i < result.elementsCount; i++)
            {
                ResultSparse.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(20)
                });

                TextBox row = NewResultCell(result.indexes[i, 0] + 1);

                TextBox col = NewResultCell(result.indexes[i, 1] + 1);

                TextBox val = NewResultCell(result.elements[i]);

                ResultSparse.Children.Add(row);
                ResultSparse.Children.Add(col);
                ResultSparse.Children.Add(val);

                Grid.SetRow(row, i + 1);
                Grid.SetRow(col, i + 1);
                Grid.SetRow(val, i + 1);
                Grid.SetColumn(row, 0);
                Grid.SetColumn(col, 1);
                Grid.SetColumn(val, 2);
            }


            double[,] resultMatrix = result.ToMatrix();
            ResultMatrix.Children.Clear();
            ResultMatrix.RowDefinitions.Clear();
            ResultMatrix.ColumnDefinitions.Clear();
            for (int i = 0; i < result.originalColumnsCount; i++)
            {
                ResultMatrix.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(40)
                });
            }
            for (int i = 0; i < result.originalRowsCount; i++)
            {
                ResultMatrix.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(20)
                });

                for (int j = 0; j < result.originalColumnsCount; j++)
                {
                    TextBox cell = NewResultCell(resultMatrix[i, j]);
                    ResultMatrix.Children.Add(cell);
                    Grid.SetRow(cell, i);
                    Grid.SetColumn(cell, j);
                }
            }
        }

        private void SparseA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowResult(new SparseMatrix(MatrixA));
            }
            catch (Exception ex) { _ = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void SparseB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowResult(new SparseMatrix(MatrixB));
            }
            catch (Exception ex) { _ = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void TransposeA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowResult(SparseMatrix.Transpose(new SparseMatrix(MatrixA)));
            }
            catch (Exception ex) { _ = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void TransposeB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowResult(SparseMatrix.Transpose(new SparseMatrix(MatrixB)));
            }
            catch (Exception ex) { _ = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void APlusB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowResult(SparseMatrix.Add(new SparseMatrix(MatrixA), new SparseMatrix(MatrixB)));
            }
            catch (Exception ex) { _ = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void AMinusB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowResult(SparseMatrix.Subtract(new SparseMatrix(MatrixA), new SparseMatrix(MatrixB)));
            }
            catch (Exception ex) { _ = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void BMinusA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowResult(SparseMatrix.Subtract(new SparseMatrix(MatrixB), new SparseMatrix(MatrixA)));
            }
            catch (Exception ex) { _ = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void AMulB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowResult(SparseMatrix.Multiply(new SparseMatrix(MatrixA), new SparseMatrix(MatrixB)));
            }
            catch (Exception ex) { _ = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void BMulA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowResult(SparseMatrix.Multiply(new SparseMatrix(MatrixB), new SparseMatrix(MatrixA)));
            }
            catch (Exception ex) { _ = MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); }
        }
    }
}
