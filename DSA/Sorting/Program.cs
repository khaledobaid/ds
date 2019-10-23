using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorting
{
    class Program
    {

        static void Main(string[] args)
        {
            TestUpdates();

            Console.ReadKey();
        }

        static void TestDistricts()
        {
            // Test Case 1
            //int rows = 5;
            //int columns = 4;
            //int[,] grid = new int[,] {
            //    { 1, 1, 0, 0 },
            //    { 0, 0, 1, 0 },
            //    { 0, 0, 0, 0 },
            //    { 1, 0, 1, 1 },
            //    { 1, 1, 1, 1 } };

            // Test Case 2
            //int rows = 4;
            //int columns = 4;
            //int[,] grid = new int[,] { 
            //    { 1, 1, 0, 0 }, 
            //    { 0, 0, 0, 0 }, 
            //    { 0, 0, 1, 1 }, 
            //    { 0, 0, 0, 0 } };

            // Test Case 3
            int rows = 7;
            int columns = 7;
            int[,] grid = new int[,] {
                { 1, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 0, 0, 0, 0, 0 },
                { 0, 0, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 0, 0, 0 },
                { 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 1 } };

            Districts districts = new Districts();

            Console.WriteLine("{0}", districts.GetNumberOfPossibleDistricts(grid, rows, columns));
        }

        static void TestUpdates()
        {
            // Test Case 1
            int rows = 4;
            int columns = 5;
            int[,] grid = new int[,] {
                { 0, 1, 1, 0, 1 },
                { 0, 1, 0, 1, 0 },
                { 0, 0, 0, 0, 1 },
                { 0, 1, 0, 0, 0 } };

            //// Test Case 2
            //int rows = 4;
            //int columns = 5;
            //int[,] grid = new int[,] {
            //    { 1, 1, 1, 1, 1 },
            //    { 1, 1, 1, 1, 1 },
            //    { 1, 1, 1, 1, 1 },
            //    { 1, 1, 1, 1, 1 } };

            //// Test Case 3
            //int rows = 5;
            //int columns = 6;
            //int[,] grid = new int[,] {
            //    { 0, 0, 1, 0, 0, 0 },
            //    { 0, 0, 0, 0, 0, 0 },
            //    { 0, 0, 0, 0, 0, 1 },
            //    { 0, 0, 0, 0, 0, 0 },
            //    { 0, 1, 0, 0, 0, 0 } };


            ServerUpdate serverUpdate = new ServerUpdate();

            Console.WriteLine(serverUpdate.GetMinimumDay(grid, rows, columns));

        }
    }

    public class ServerUpdate
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        // Left, Top, Right, Down
        int[] nRows = { 0, -1, 0, 1 };
        int[] nCols = { -1, 0, 1, 0 };

        public int GetMinimumDay(int[,] grid, int rows, int columns)
        {
            Rows = rows;
            Columns = columns;

            int DaysCounter = 0;

            // Internal Variables
            int[,] clone = new int[rows, columns];
            bool atLeastOneUpdateOccured = false;
            bool atLeastOneConsidered;
            bool isMoreDaysRequired = true;
            int outdatedCounter;
            

            while (isMoreDaysRequired)
            {
                outdatedCounter = 0;
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        clone[i, j] = grid[i, j];

                        if (grid[i, j] == 0)
                        {
                            // Check if any other can update it
                            atLeastOneConsidered = false;
                            for (int r = 0; r < nRows.Length; r++)
                            {
                                if (IsConsidered(grid, i + nRows[r], j + nCols[r]))
                                {
                                    atLeastOneUpdateOccured = true;
                                    atLeastOneConsidered = true;
                                    break;
                                }
                            }

                            if (atLeastOneConsidered)
                                clone[i, j] = 1;
                            else
                                outdatedCounter++;
                        }
                    }
                }

                // Update the grid matrix
                for (int i = 0; i < rows; i++) for (int j = 0; j <columns; j++) grid[i, j] = clone[i, j];

                if (atLeastOneUpdateOccured)
                    DaysCounter++;

                if (outdatedCounter == 0)
                    isMoreDaysRequired = false;
            }


            return DaysCounter;
        }

        private bool IsConsidered(int[,] grid, int row, int col)
        {
            if (row >= 0 && row < Rows && col >= 0 && col < Columns && grid[row, col] == 1)
                return true;
            return false;
        }

    }

    public class Districts
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }


        // Left, Top, Right, Down
        int[] nRows = { 0, -1, 0, 1 };
        int[] nCols = { -1, 0, 1, 0 };

        // Each building can have 4 neighbors
        private bool[,] visitied;

        public int GetNumberOfPossibleDistricts(int[,] grid, int rows, int columns)
        {
            this.Rows = rows;
            this.Columns = columns;

            int totalDistricts = 0;

            // initilize visitied array to store visited values
            visitied = new bool[rows, columns];

            // Linear Traverse Matrix
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (grid[i, j] == 1 && !visitied[i, j])
                    {
                        // If this item is building and it is not discovered yet, then new district is found
                        totalDistricts++;

                        // Now traverse neigbors, and marke visited adjacency
                        Traverse(grid, i, j);
                    }
                }
            }

            return totalDistricts;
        }

        private void Traverse(int[,] grid, int row, int col)
        {
            // First : Mark item as visited.
            visitied[row, col] = true;

            // Second : Using DFS traverse all neighbors (but only neigbors that can be considered within district)
            for (int i = 0; i < nRows.Length; i++)
            {
                // If item is considered, then traverese it also since it is connected.
                if (IsConsidered(grid, row + nRows[i], col + nCols[i]))
                    Traverse(grid, row + nRows[i], col + nCols[i]);
            }
        }

        /// This function will check if specific item can be considered within the cluster
        /// 1. It is not visited
        /// 2. It is not out of the matrix
        /// 3. It's value is 1
        private bool IsConsidered(int[,] grid, int row, int col)
        {
            if (row >= 0 && row < Rows && col >= 0 && col < Columns && grid[row, col] == 1 && !visitied[row, col])
                return true;
            return false;
        }
    }
}
