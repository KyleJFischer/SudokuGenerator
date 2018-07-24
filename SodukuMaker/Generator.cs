using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SodukuMaker
{
    class Generator
    {
        private static Random rng = new Random();
        public static ThreeByThree ThreeByThreeGenerator()
        {
            var result = new ThreeByThree();
            List<int> remainingNumbers = new List<int>();
            for (int i = 1; i < 10; i++)
            {
                remainingNumbers.Add(i);
            }

            foreach(var item in result.grid)
            {

                int index = rng.Next(0, remainingNumbers.Count );
                int value = remainingNumbers[index];
                remainingNumbers.RemoveAt(index);


                item.value = value;
            }
            return result;
        }

        public static void RowGenerator(Puzzle puz)
        {
            
        }

        public static Puzzle makePuzzle()
        {
            var puz = new Puzzle();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    populateSquare(puz, i, j);
                }
            }


            return puz;
        }

        public static void populateSquare(Puzzle puz, int x, int y)
        {
            puz.ConvertToOurGrid(x, out int internalRow, out int enternalRow);
            puz.ConvertToOurGrid(y, out int internalCol, out int enternalCol);

            var numbersForSquare = PopulateList();
            var numbersForRow = PopulateList();
            var numbersForCol = PopulateList();

            numbersForSquare.AddRange(puz.grid[internalRow, internalCol].GetAllNumbers());
            numbersForRow.AddRange(puz.getRowOfNumbers(x));
            numbersForCol.AddRange(puz.getColOfNumbers(y));

            numbersForSquare = RemoveDuplicates(numbersForSquare);
            numbersForRow = RemoveDuplicates(numbersForRow);
            numbersForCol = RemoveDuplicates(numbersForCol);

            var condensedList = CombineList(numbersForSquare, numbersForRow, numbersForCol);


            var finalList = condensedList.Distinct().ToList();
            if (finalList.Count() == 0)
            {
                throw new Exception("We have no possible out come");
            }

            var index = rng.Next(0, finalList.Count());
            var square = puz.GetSquare(x, y);
            square.x = x;
            square.y = y;
            square.value = finalList[index];
            
        }

        private static List<int> CombineList(List<int> numbersForSquare, List<int> numbersForRow, List<int> numbersForCol)
        {
            var masterList = new List<int>();
            foreach(var item in numbersForRow)
            {
                if (numbersForSquare.Contains(item) && numbersForCol.Contains(item))
                {
                    masterList.Add(item);
                }
            }

            return masterList;
        }

        public static List<int> PopulateList()
        {
            var list = new List<int>();
            for (int i = 1; i < 10; i++)
            {
                list.Add(i);
            }
            return list;
        }

        public static List<int> RemoveDuplicates(List<int> list)
        {
            var duplicateKeys = list.GroupBy(x => x)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key);

            foreach(var item in duplicateKeys)
            {
                list.RemoveAll(m => m.Equals(item));
            }

            return list;
        }

        public static Square GetRandomSquare(Puzzle puz)
        {
            int x = rng.Next(0, 10);
            int y = rng.Next(0, 10);
            return puz.GetSquare(x, y);
        }

    }
}
