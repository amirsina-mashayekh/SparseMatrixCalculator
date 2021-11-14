using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparseMatrixCalculator.SparseUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparseMatrixCalculator.SparseUtil.Tests
{
    [TestClass()]
    public class SparseMatrixTests
    {
        [TestMethod()]
        public void SparseMatrixFromMatrixTest()
        {
            double[,] matrix =
            {
                {1, 2, 3},
                {0, 5, 6},
                {5, 0, 7},
                {6, 7, 0},
                {7, 8, 9}
            };

            SparseMatrix sparseMatrix = new SparseMatrix(matrix);

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        _ = Assert.ThrowsException<ArgumentException>(() => sparseMatrix.Get(i, j));
                    }
                    else
                    {
                        Assert.AreEqual(matrix[i,j], sparseMatrix.Get(i, j));
                    }
                }
            }
        }

        [TestMethod()]
        public void TransposeTest()
        {
            double[,] matrix =
            {
                {1, 2, 3},
                {0, 5, 6},
                {5, 0, 7},
                {6, 7, 0},
                {7, 8, 9}
            };

            SparseMatrix sparseMatrix = SparseMatrix.Transpose(new SparseMatrix(matrix));

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        _ = Assert.ThrowsException<ArgumentException>(() => sparseMatrix.Get(j, i));
                    }
                    else
                    {
                        Assert.AreEqual(matrix[i, j], sparseMatrix.Get(j, i));
                    }
                }
            }
        }
    }
}