using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SodukuMaker
{
    [Serializable]
    class Puzzle
    {
        public ThreeByThree[,] grid = new ThreeByThree[3, 3];

        public Puzzle()
        {
            populateGrid();
        }

        public void populateGrid()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    grid[i, j] = new ThreeByThree();
                }
            }
        }

        public List<int> getRowOfNumbers(int row)
        {
            int internalRow = 0;
            int enInternalRow = row % 3;


            if (row < 9)
            {
                internalRow = 2;
            }
            if (row < 6)
            {
                internalRow = 1;
            }
            if (row < 3)
            {
                internalRow = 0;
            }

            var masterList = new List<int>();

            for (int x = 0; x < 3; x++)
            {
                var listOfNumbers = grid[internalRow, x].GetNumbersForRow(enInternalRow);
                masterList.AddRange(listOfNumbers);
            }



            return masterList;
        }

        public List<int> getColOfNumbers(int col)
        {
            int internalCol = 0;
            int enInternalCol = col % 3;


            if (col < 9)
            {
                internalCol = 2;
            }
            if (col < 6)
            {
                internalCol = 1;
            }
            if (col < 3)
            {
                internalCol = 0;
            }

            var masterList = new List<int>();

            for (int x = 0; x < 3; x++)
            {
                var listOfNumbers = grid[x, internalCol].GetNumbersForColumn(enInternalCol);
                masterList.AddRange(listOfNumbers);
            }



            return masterList;
        }

        public List<Square> getColOfSquares(int col)
        {
            int internalCol = 0;
            int enInternalCol = col % 3;


            if (col < 9)
            {
                internalCol = 2;
            }
            if (col < 6)
            {
                internalCol = 1;
            }
            if (col < 3)
            {
                internalCol = 0;
            }

            var masterList = new List<Square>();

            for (int x = 0; x < 3; x++)
            {
                var listOfSquares = grid[x, internalCol].GetSquaresForColumn(enInternalCol);
                masterList.AddRange(listOfSquares);
            }
            return masterList;
        }

        public List<Square> getRowOfSquares(int row)
        {
            int internalRow = 0;
            int enInternalRow = row % 3;


            if (row < 9)
            {
                internalRow = 2;
            }
            if (row < 6)
            {
                internalRow = 1;
            }
            if (row < 3)
            {
                internalRow = 0;
            }

            var masterList = new List<Square>();

            for (int x = 0; x < 3; x++)
            {
                var listOfSquares = grid[internalRow, x].GetSquaresForRow(enInternalRow);
                masterList.AddRange(listOfSquares);
            }



            return masterList;
        }

        public Square GetSquare(int x, int y)
        {
            ConvertToOurGrid(x, out int internalRow, out int enInternalRow);
            ConvertToOurGrid(y, out int internalCol, out int enInternalCol);
            return grid[internalRow, internalCol].grid[enInternalRow, enInternalCol];
        }

        public ThreeByThree GetThreeByThree(int x, int y)
        {
            ConvertToOurGrid(x, out int internalRow, out int enInternalRow);
            ConvertToOurGrid(y, out int internalCol, out int enInternalCol);
            return grid[internalRow, internalCol];
        }

        public void ConvertToOurGrid(int row, out int inter, out int enInternal)
        {
            inter = 0;
            enInternal = row % 3;


            if (row < 9)
            {
                inter = 2;
            }
            if (row < 6)
            {
                inter = 1;
            }
            if (row < 3)
            {
                inter = 0;
            }
        }

        public bool CheckAllRows()
        {
            for (int i = 0; i < 9; i++)
            {
                var numbers = Generator.PopulateList();
                var list = getRowOfSquares(i);
                foreach (var vert in list)
                {
                    var value = (vert.value != null) ? (int)vert.value : 0;

                    if (value != 0)
                    {
                        if (!numbers.Contains((int)vert.value))
                        {
                            return false;
                        }
                        numbers.Remove((int)vert.value);
                    }

                }
            }
            return true;
        }
        public bool CheckAllCols()
        {

            for (int i = 0; i < 9; i++)
            {
                var numbers = Generator.PopulateList();
                var list = getColOfSquares(i);
                foreach (var vert in list)
                {
                    var value = (vert.value != null) ? (int)vert.value : 0;

                    if (value != 0)
                    {
                        if (!numbers.Contains((int)vert.value))
                        {
                            return false;
                        }
                        numbers.Remove((int)vert.value);
                    }

                }
            }
            return true;
        }

        public bool checkListForAllNumbers(List<int> list)
        {
            for (int i = 1; i < 10; i++)
            {
                if (!list.Contains(i))
                {
                    return false;
                }
            }
            return true;
        }

        public void print()
        {
            for (int i = 0; i < 3; i++)
            {

                for (int j = 0; j < 3; j++)
                {
                    string printout = "";
                    for (int x = 0; x < 3; x++)
                    {
                        var listOfNumbers = grid[i, x].GetNumbersForRowWithNulls(j);
                        foreach (var num in listOfNumbers)
                        {
                            if (num < 0)
                            {
                                printout += "_" + ",";
                            } else
                            {
                                printout += num + ",";
                            }
                            
                        }
                        printout += "|";
                    }
                    Console.WriteLine(printout);

                }
                Console.WriteLine("--------------------------");


            }
        }

        public void clearSquare(Square square)
        {
            square.value = null;
            square.cleared = true;
        }

        public bool isValidMove(int value, int x, int y)
        {
            Square sq = GetSquare(x, y);
            int? originalValue = sq.value;
            sq.value = value;
            var three = GetThreeByThree(x, y);
            if (CheckAllRows() && CheckAllCols() && three.isValid())
            {
                sq.value = originalValue;
                return true;
            }
            sq.value = originalValue;
            return false;

        }
        public bool isValidBoard()
        {
            foreach (var three in grid)
            {
                if (!three.isValid())
                {
                    return false;
                }
            }

            return CheckAllRows() && CheckAllCols();
        }

        public void makeMove(int value, int x, int y)
        {
            Square sq = GetSquare(x, y);
            sq.value = value;
        }

        public List<Square> getAllNullSquares()
        {
            var masterList = new List<Square>();
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    var sq = GetSquare(x, y);
                    if (sq.value == null)
                    {
                        masterList.Add(sq);
                    }

                }
            }

            return masterList;
        }

        public List<Square> getAllSquares()
        {
            var masterList = new List<Square>();
            for (int x = 0; x < 9; x++)
            {
                for (int y = 0; y < 9; y++)
                {
                    masterList.Add(GetSquare(x, y));
                }
            }


            return masterList;
        }


        public bool ComparePuzzles(Puzzle puz2)
        {
            var squares = getAllSquares();

            foreach (var sq in squares)
            {
                int x = sq.x;
                int y = sq.y;
                var puz2Sq = puz2.GetSquare(x, y);

                if (puz2Sq.value != sq.value)
                {
                    return false;
                }

            }
            return true;
        }


        public void outputToFile(String fileName)
        {

            using (System.IO.StreamWriter file =
          new System.IO.StreamWriter(fileName))
            {
                for (int i = 0; i < 3; i++)
                {

                    for (int j = 0; j < 3; j++)
                    {
                        string printout = "";
                        for (int x = 0; x < 3; x++)
                        {
                            var listOfNumbers = grid[i, x].GetNumbersForRowWithNulls(j);
                            foreach (var num in listOfNumbers)
                            {
                                if (num < 0)
                                {
                                    printout += "_" + ",";
                                }
                                else
                                {
                                    printout += num + ",";
                                }

                            }
                            printout += "|";
                        }
                        file.WriteLine(printout);

                    }
                    file.WriteLine("--------------------------");


                }
            }

            
        }
    }
}
