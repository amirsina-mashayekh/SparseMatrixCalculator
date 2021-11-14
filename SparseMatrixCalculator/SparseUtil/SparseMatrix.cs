using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparseMatrixCalculator.SparseUtil
{
    /// <summary>
    /// Represents a Sparse matrix.
    /// Provides methods to add, subtract, sort, multiply and transpose Sparse matrices.
    /// </summary>
    /// <typeparam name="T">The type of elements in matrix.</typeparam>
    public class SparseMatrix
    {
        public readonly int elementsCount;

        public readonly int totalRows;

        public readonly int totalColumns;

        public readonly int[,] indexes;

        public readonly double[] elements;

        public SparseMatrix(int elementsCount, int totalRows, int totalColumns)
        {
            if (elementsCount > totalRows * totalColumns)
            {
                throw new ArgumentException("elementsCount should not be " +
                    "more than totalRows multiplied by totalColumns.", "elementsCount");
            }
            if (elementsCount < 0)
            {
                throw new ArgumentException("elementsCount should not be less than zero.", "elementsCount");
            }
            if (totalRows < 0)
            {
                throw new ArgumentException("totalRows should not be less than zero.", "totalRows");
            }
            if (totalColumns < 0)
            {
                throw new ArgumentException("totalColumns should not be less than zero.", "totalColumns");
            }

            this.elementsCount = elementsCount;
            this.totalRows = totalRows;
            this.totalColumns = totalColumns;
            indexes = new int[elementsCount, 2];
            elements = new double[elementsCount];
        }

        public SparseMatrix(double[,] matrix)
        {
            totalRows = matrix.GetLength(0);
            totalColumns = matrix.GetLength(1);
            int[,] tmpIndexes = new int[totalRows * totalColumns, 2];
            double[] tmpValues = new double[totalRows * totalColumns];
            elementsCount = -1;

            for (int i = 0; i < totalRows; i++)
            {
                for (int j = 0; j < totalColumns; j++)
                {
                    double element = matrix[i, j];
                    if (element != 0)
                    {
                        elementsCount++;
                        tmpIndexes[elementsCount, 0] = i;
                        tmpIndexes[elementsCount, 1] = j;
                        tmpValues[elementsCount] = element;
                    }
                }
            }

            elementsCount++;

            indexes = new int[elementsCount, 2];
            elements = new double[elementsCount];

            for (int i = 0; i < elementsCount; i++)
            {
                indexes[i, 0] = tmpIndexes[i, 0];
                indexes[i, 1] = tmpIndexes[i, 1];
                elements[i] = tmpValues[i];
            }
        }

        public double Get(int row, int col)
        {
            int len = indexes.GetLength(0);
            for (int i = 0; i < len; i++)
            {
                if (indexes[i, 0] == row && indexes[i, 1] == col)
                {
                    return elements[i];
                }
            }

            throw new ArgumentException();
        }

        public void Sort()
        {
            for (int i = 0; i < elementsCount; i++)
            {
                int currentRow = indexes[i, 0];
                int currentColumn = indexes[i, 1];
                int lowestValueIndex = i;

                for (int j = i + 1; j < elementsCount; j++)
                {
                    int cmpRow = indexes[j, 0];
                    int cmpColumn = indexes[j, 1];

                    if (cmpRow < currentRow ||
                        (cmpRow == currentRow && cmpColumn < currentColumn))
                    {
                        lowestValueIndex = j;
                        currentRow = cmpRow;
                        currentColumn = cmpColumn;
                    }
                }

                if (lowestValueIndex != i)
                {
                    // Swap indexes
                    indexes[lowestValueIndex, 0] = indexes[i, 0];
                    indexes[lowestValueIndex, 1] = indexes[i, 1];
                    indexes[i, 0] = currentRow;
                    indexes[i, 1] = currentColumn;

                    // Swap elements
                    double tmp = elements[i];
                    elements[i] = elements[lowestValueIndex];
                    elements[lowestValueIndex] = tmp;
                }
            }
        }

        public static SparseMatrix Transpose(SparseMatrix matrix)
        {
            if (matrix.elementsCount == 0) { return matrix; }

            SparseMatrix transposed = new SparseMatrix(matrix.elementsCount, matrix.totalRows, matrix.totalColumns);

            int[] rowSize = new int[matrix.totalRows];
            for (int i = 0; i < matrix.elementsCount; i++)
            {
                rowSize[matrix.indexes[i, 0]]++;
            }

            int[] startOfRow = new int[matrix.totalRows];
            for (int i = 1; i < matrix.totalRows; i++)
            {
                startOfRow[i] = startOfRow[i - 1] + rowSize[i - 1];
            }

            for (int i = 0; i < matrix.elementsCount; i++)
            {
                int SOR_Index = matrix.indexes[i, 0];
                int SOR = startOfRow[SOR_Index];

                transposed.indexes[SOR, 1] = matrix.indexes[i, 0];
                transposed.indexes[SOR, 0] = matrix.indexes[i, 1];
                transposed.elements[SOR] = matrix.elements[i];
                startOfRow[SOR_Index]++;
            }
            return transposed;
        }
    }
}
