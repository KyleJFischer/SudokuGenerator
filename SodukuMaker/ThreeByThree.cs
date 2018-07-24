using System;
using System.Collections.Generic;
using System.Text;

namespace SodukuMaker
{
    [Serializable]
    class ThreeByThree
    {
        public Square[,] grid;


        public ThreeByThree()
        {
            grid = new Square[3,3];
            populateGrid();
        }

        public void populateGrid()
        {
            if (grid != null)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        grid[i,j] = new Square();
                    }
                }
            }
        }

        public bool isValid()
        {
            var numbers = Generator.PopulateList();

            int runningSum = 0;
            if (grid == null)
            {
                return false;
            }
            foreach(var vert in grid)
            {
                

                var value = (vert.value != null) ? (int)vert.value : 0;
                runningSum += value;
                if (value != 0)
                {
                    if (!numbers.Contains((int)vert.value))
                    {
                        return false;
                    }
                    numbers.Remove((int)vert.value);
                }
                
            }
    
            return true;
        }

        public void print()
        {
            for (int i = 0; i < 3; i++)
            {
                string printout = "";
                for (int j = 0; j < 3; j++)
                {
                    printout += (grid[i,j].value + ",");
                }
                Console.WriteLine(printout);
            }
        }

        public List<int> GetAllNumbers()
        {
            var masterList = new List<int>();
            for (int i = 0; i < 3; i++)
            {

                for (int j = 0; j < 3; j++)
                {
                    int? val = grid[i, j].value;
                    if (val != null)
                    {
                        masterList.Add((int)grid[i, j].value);
                    }
                    
                }

            }
            return masterList;
        }

        public List<int> GetNumbersForRow(int row)
        {
            var results = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                int? val = grid[row, i].value;
                if (val != null)
                {
                    results.Add((int)val);
                }
               
            }
            return results;
        }

        public List<int> GetNumbersForRowWithNulls(int row)
        {
            var results = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                int? val = grid[row, i].value;
                if (val == null)
                {
                    val = -1;
                }
                results.Add((int)val);
                
            }
            return results;
        }

        public List<Square> GetSquaresForRow(int row)
        {
            var results = new List<Square>();
            for (int i = 0; i < 3; i++)
            {
                results.Add(grid[row, i]);
            }
            return results;
        }

        public List<int> GetNumbersForColumn(int Column)
        {
            var results = new List<int>();
            for (int i = 0; i < 3; i++)
            {
                int? val = grid[i, Column].value;
                if (val != null)
                {
                    results.Add((int)val);
                }
            }
            return results;
        }

        public List<Square> GetSquaresForColumn(int Column)
        {
            var results = new List<Square>();
            for (int i = 0; i < 3; i++)
            {
                results.Add(grid[i, Column]);
            }
            return results;
        }
    }
}
