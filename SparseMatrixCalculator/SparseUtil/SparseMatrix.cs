using System;

namespace SparseMatrixCalculator.SparseUtil
{
    /// <summary>
    /// Represents a Sparse matrix.
    /// Provides methods to add, subtract, sort, multiply and transpose Sparse matrices.
    /// </summary>
    /// <typeparam name="T">The type of elements in matrix.</typeparam>
    public class SparseMatrix
    {
        /// <summary>
        /// Count of non-zero elements in matrix.
        /// </summary>
        public readonly int elementsCount;

        /// <summary>
        /// Count of rows in the original matrix.
        /// </summary>
        public readonly int originalRowsCount;

        /// <summary>
        /// Count of columns in the original matrix.
        /// </summary>
        public readonly int originalColumnsCount;

        /// <summary>
        /// Contains index of each non-zero element in the original matrix.
        /// First column indicates original row and second column indicates original column.
        /// </summary>
        public readonly int[,] indexes;

        /// <summary>
        /// Contains non-zero elements of matrix.
        /// </summary>
        public readonly double[] elements;

        /// <summary>
        /// Initializes a new instance of the <c>SparseMatrix</c> class
        /// that has the specified capacity.
        /// </summary>
        /// <param name="elementsCount">Count of non-zero elements.</param>
        /// <param name="originalRowsCount">Count of rows in the original matrix.</param>
        /// <param name="originalColumnsCount">Count of columns in the original matrix.</param>
        /// <exception cref="ArgumentException"></exception>
        public SparseMatrix(int elementsCount, int originalRowsCount, int originalColumnsCount)
        {
            if (elementsCount > originalRowsCount * originalColumnsCount)
            {
                throw new ArgumentException("elementsCount should not be " +
                    "more than totalRows multiplied by totalColumns.", "elementsCount");
            }
            if (elementsCount < 0)
            {
                throw new ArgumentException("elementsCount should not be less than zero.", "elementsCount");
            }
            if (originalRowsCount < 0)
            {
                throw new ArgumentException("totalRows should not be less than zero.", "totalRows");
            }
            if (originalColumnsCount < 0)
            {
                throw new ArgumentException("totalColumns should not be less than zero.", "totalColumns");
            }

            this.elementsCount = elementsCount;
            this.originalRowsCount = originalRowsCount;
            this.originalColumnsCount = originalColumnsCount;
            indexes = new int[elementsCount, 2];
            elements = new double[elementsCount];
        }

        /// <summary>
        /// Creates new instance of the <c>SparseMatrix</c> class
        /// from a matrix.
        /// </summary>
        /// <param name="matrix">A two dimentional <c>double</c> array which contains matrix data.</param>
        public SparseMatrix(double[,] matrix)
        {
            originalRowsCount = matrix.GetLength(0);
            originalColumnsCount = matrix.GetLength(1);
            int[,] tmpIndexes = new int[originalRowsCount * originalColumnsCount, 2];
            double[] tmpValues = new double[originalRowsCount * originalColumnsCount];
            elementsCount = -1;

            for (int i = 0; i < originalRowsCount; i++)
            {
                for (int j = 0; j < originalColumnsCount; j++)
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

        /// <summary>
        /// Gets element at a specific index in matrix.
        /// </summary>
        /// <param name="row">The row of element.</param>
        /// <param name="col">The column of element.</param>
        /// <returns>
        /// The element at specified index.
        /// If element isn't defined in this <c>SparseMatrix</c>
        /// but is in range of original matrix, returns zero.
        /// </returns>
        /// <exception cref="ArgumentException">Index is out of range of original matrix.</exception>
        public double GetElementAt(int row, int col)
        {
            if (row < 0 || row >= originalRowsCount ||
                col < 0 || col >= originalColumnsCount)
            {
                throw new ArgumentException("Index is out of range of original matrix.");
            }

            int len = indexes.GetLength(0);
            for (int i = 0; i < len; i++)
            {
                if (indexes[i, 0] == row && indexes[i, 1] == col)
                {
                    return elements[i];
                }
            }

            return 0;
        }

        /// <summary>
        /// Converts this instance of <c>SparseMatrix</c> to a normal matrix.
        /// </summary>
        /// <returns>A two dimentional <c>double</c> array which contains matrix data.</returns>
        public double[,] ToMatrix()
        {
            double[,] result = new double[originalRowsCount, originalColumnsCount];

            for (int i = 0; i < originalRowsCount; i++)
            {
                for (int j = 0; j < originalColumnsCount; j++)
                {
                    result[i, j] = GetElementAt(i, j);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the Transpose of matrix.
        /// </summary>
        /// <param name="matrix">An instance of <c>SparseMatrix</c> class.</param>
        /// <returns>
        /// An instance of <c>SparseMatrix</c> class which indicates transpose of <paramref name="matrix"/>.
        /// </returns>
        public static SparseMatrix Transpose(SparseMatrix matrix)
        {
            if (matrix.elementsCount == 0) { return matrix; }

            SparseMatrix transposed = new SparseMatrix(matrix.elementsCount, matrix.originalColumnsCount, matrix.originalRowsCount);

            int[] rowSize = new int[matrix.originalRowsCount];
            for (int i = 0; i < matrix.elementsCount; i++)
            {
                rowSize[matrix.indexes[i, 0]]++;
            }

            int[] startOfRow = new int[matrix.originalRowsCount];
            for (int i = 1; i < matrix.originalRowsCount; i++)
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

        /// <summary>
        /// Adds two matrices and returns the result.
        /// </summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>Sum of two matrices.</returns>
        /// <exception cref="ArgumentException">Matrices are not of same size.</exception>
        public static SparseMatrix Add(SparseMatrix left, SparseMatrix right)
        {
            if (left.originalRowsCount != right.originalRowsCount
                || left.originalColumnsCount != right.originalColumnsCount)
            {
                throw new ArgumentException("Matrices are not of same size.");
            }

            double[,] result = new double[left.originalRowsCount, left.originalColumnsCount];

            for (int i = 0; i < left.elementsCount; i++)
            {
                result[left.indexes[i, 0], left.indexes[i, 1]] = left.elements[i];
            }

            for (int i = 0; i < right.elementsCount; i++)
            {
                result[right.indexes[i, 0], right.indexes[i, 1]] += right.elements[i];
            }

            return new SparseMatrix(result);
        }

        /// <summary>
        /// Subtracts one matrix from another and returns the result.
        /// </summary>
        /// <param name="left">The matrix to be subtracted.</param>
        /// <param name="right">The matrix to subtract.</param>
        /// <returns>The result of subtracting <paramref name="right"/> from <paramref name="right"/></returns>
        /// <exception cref="ArgumentException">Matrices are not of same size.</exception>
        public static SparseMatrix Subtract(SparseMatrix left, SparseMatrix right)
        {
            if (left.originalRowsCount != right.originalRowsCount
                || left.originalColumnsCount != right.originalColumnsCount)
            {
                throw new ArgumentException("Matrices are not of same size.");
            }

            double[,] result = new double[left.originalRowsCount, left.originalColumnsCount];

            for (int i = 0; i < left.elementsCount; i++)
            {
                result[left.indexes[i, 0], left.indexes[i, 1]] = left.elements[i];
            }

            for (int i = 0; i < right.elementsCount; i++)
            {
                result[right.indexes[i, 0], right.indexes[i, 1]] -= right.elements[i];
            }

            return new SparseMatrix(result);
        }

        /// <summary>
        /// Returns the product of two matrices.
        /// </summary>
        /// <param name="left">The first matrix.</param>
        /// <param name="right">The second matrix.</param>
        /// <returns>The product of the <paramref name="left"/> and <paramref name="right"/> parameters.</returns>
        /// <exception cref="ArgumentException">
        /// The number of columns in the <paramref name="left"/> are not
        /// equal to the number of rows in the <paramref name="right"/>.
        /// </exception>
        public static SparseMatrix Multiply(SparseMatrix left, SparseMatrix right)
        {
            if (left.originalColumnsCount != right.originalRowsCount)
            {
                throw new ArgumentException("The number of columns in the left matrix are not " +
                    "equal to the number of rows in the right matrix");
            }

            double[,] result = new double[left.originalRowsCount, right.originalColumnsCount];
            for (int i = 0; i < left.elementsCount; i++)
            {
                int row = left.indexes[i, 0];
                int col = left.indexes[i, 1];
                for (int j = 0; j < right.originalColumnsCount; j++)
                {
                    result[row, j] += left.elements[i] * right.GetElementAt(col, j);
                }
            }

            return new SparseMatrix(result);
        }
    }
}
