using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using static SparseMatrixCalculator.SparseUtil.SparseMatrix;

namespace SparseMatrixCalculator.SparseUtil.Tests
{
    [TestClass()]
    public class SparseMatrixTests
    {
        private static readonly double[,] matrix1 =
        {
            {1, 2, 3},
            {0, 5, 6},
            {5, 0, 7},
            {6, 7, 0},
            {7, 8, 9}
        };

        private static readonly SparseMatrix sparseMatrix1;

        private static readonly double[,] matrix2 =
        {
            {7, 8, 9},
            {1, 2, 3},
            {0, 5, 6},
            {5, 0, 7},
            {6, 7, 0}
        };

        private static readonly SparseMatrix sparseMatrix2;

        private static readonly double[,] matrix3 =
        {
            {7, 8, 9, 4},
            {1, 2, 3, 8},
            {0, 5, 6, 3}
        };

        private static readonly SparseMatrix sparseMatrix3;

        private static readonly double[,] matrix1plus2;

        private static readonly double[,] matrix1minus2;

        private static readonly double[,] matrix1mul3;

        static SparseMatrixTests()
        {
            int m1r = matrix1.GetLength(0);
            int m1c = matrix1.GetLength(1);
            int m3c = matrix3.GetLength(1);

            matrix1plus2 = new double[m1r, m1c];
            for (int i = 0; i < m1r; i++)
            {
                for (int j = 0; j < m1c; j++)
                {
                    matrix1plus2[i, j] = matrix1[i, j] + matrix2[i, j];
                }
            }

            matrix1minus2 = new double[m1r, m1c];
            for (int i = 0; i < m1r; i++)
            {
                for (int j = 0; j < m1c; j++)
                {
                    matrix1minus2[i, j] = matrix1[i, j] - matrix2[i, j];
                }
            }

            matrix1mul3 = new double[m1r, m3c];
            for (int i = 0; i < m1r; i++)
            {
                for (int j = 0; j < m3c; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < m1c; k++)
                    {
                        sum += matrix1[i, k] * matrix3[k, j];
                    }
                    matrix1mul3[i, j] = sum;
                }
            }

            sparseMatrix1 = new SparseMatrix(matrix1);
            sparseMatrix2 = new SparseMatrix(matrix2);
            sparseMatrix3 = new SparseMatrix(matrix3);
        }

        [TestMethod()]
        public void SparseMatrixFromMatrixTest()
        {
            SparseMatrix sparseMatrix = new SparseMatrix(matrix1);

            int m1r = matrix1.GetLength(0);
            int m1c = matrix1.GetLength(1);
            for (int i = 0; i < m1r; i++)
            {
                for (int j = 0; j < m1c; j++)
                {
                    Assert.AreEqual(matrix1[i, j], sparseMatrix.GetElementAt(i, j));
                }
            }
        }

        [TestMethod()]
        public void TransposeTest()
        {
            SparseMatrix sparseMatrix = SparseMatrix.Transpose(new SparseMatrix(matrix1));

            int m1r = matrix1.GetLength(0);
            int m1c = matrix1.GetLength(1);
            for (int i = 0; i < m1r; i++)
            {
                for (int j = 0; j < m1c; j++)
                {
                    Assert.AreEqual(matrix1[i, j], sparseMatrix.GetElementAt(j, i));
                }
            }
        }

        [TestMethod()]
        public void AddTest()
        {
            _ = Assert.ThrowsException<ArgumentException>(() => Add(sparseMatrix1, sparseMatrix3));
            SparseMatrix sum = Add(sparseMatrix1, sparseMatrix2);

            int m1r = matrix1.GetLength(0);
            int m1c = matrix1.GetLength(1);
            for (int i = 0; i < m1r; i++)
            {
                for (int j = 0; j < m1c; j++)
                {
                    Assert.AreEqual(matrix1plus2[i, j], sum.GetElementAt(i, j));
                }
            }
        }

        [TestMethod()]
        public void SubtractTest()
        {
            _ = Assert.ThrowsException<ArgumentException>(() => Subtract(sparseMatrix1, sparseMatrix3));
            SparseMatrix dif = Subtract(sparseMatrix1, sparseMatrix2);

            int m1r = matrix1.GetLength(0);
            int m1c = matrix1.GetLength(1);
            for (int i = 0; i < m1r; i++)
            {
                for (int j = 0; j < m1c; j++)
                {
                    Assert.AreEqual(matrix1minus2[i, j], dif.GetElementAt(i, j));
                }
            }
        }

        [TestMethod()]
        public void MultiplyTest()
        {
            _ = Assert.ThrowsException<ArgumentException>(() => Multiply(sparseMatrix1, sparseMatrix2));
            SparseMatrix mul = Multiply(sparseMatrix1, sparseMatrix3);

            int m3r = matrix3.GetLength(0);
            int m3c = matrix3.GetLength(1);
            for (int i = 0; i < m3r; i++)
            {
                for (int j = 0; j < m3c; j++)
                {
                    Assert.AreEqual(matrix1mul3[i, j], mul.GetElementAt(i, j));
                }
            }
        }
    }
}