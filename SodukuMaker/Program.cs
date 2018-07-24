using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SodukuMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = false;
            var count = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var puz = new Puzzle();
            while (true)
            {
                try
                {
                    puz = Generator.makePuzzle();
                    if (puz != null)
                    {
                        puz.print();
                    }
                    
                    break;
                } catch (Exception exe)
                {
                }
                count++; 
            }
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);


            var playPuzzle = DeepClone(puz);


            playPuzzle.print();
            Console.WriteLine("Valid Board: " + playPuzzle.isValidBoard());

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("BEGIN SOLVER");
            var finalResult = GeneratePuzzles(puz);
            finalResult.outputToFile(@"Q:\finaloutput.txt");
            puz.outputToFile(@"Q:\final.txt");

            Console.WriteLine("DONE");
            Console.ReadLine();
        }

        public static Puzzle GeneratePuzzles(Puzzle masterCopy)
        {
            var playCopy = DeepClone(masterCopy);
            var maxCount = 4;

            while (maxCount > 0)
            {
                var squareToClear = Generator.GetRandomSquare(playCopy);
                var oldValue = squareToClear.value;
                if (oldValue != null) {
                    squareToClear.value = null;

                    var minusCopy = DeepClone(playCopy);
                    var plusCopy = DeepClone(playCopy);


                    var positiveResult = SolvePositive(plusCopy, 0, 0);
                    var negativeResult = SolveNegative(minusCopy, 0, 0);


                    if (masterCopy.ComparePuzzles(positiveResult) && masterCopy.ComparePuzzles(negativeResult))
                    {
                        Console.WriteLine("WE GOOD");
                        maxCount = 4;
                    }
                    else
                    {
                        Console.WriteLine("We Bad");
                        squareToClear.value = oldValue;
                        maxCount--;
                    }
                }
               
            }
            playCopy.print();
            return playCopy;

        }


        public static Puzzle SolvePositive(Puzzle puz, int x, int y)
        {
            var nullSquares = puz.getAllNullSquares();
            solveRe(puz, nullSquares, 0);
            return puz;
        }


        public static Puzzle SolveNegative(Puzzle puz, int x, int y)
        {
            var nullSquares = puz.getAllNullSquares();
            solveRe(puz, nullSquares, 0);
            return puz;
        }

        public static bool solveRe(Puzzle puz, List<Square> list, int listPostion)
        {
            if (listPostion >= list.Count)
            {
                return true;
            }

            var ourSquare = list[listPostion];
            for (int i = 1; i < 10; i++)
            {
                ourSquare.value = i;
                if (puz.isValidBoard())
                {
                    if (solveRe(puz, list, ++listPostion))
                    {
                        return true;
                    } 
                }
                
            }
            ourSquare.value = null;

            return false;
        }

        public static bool solveMinus(Puzzle puz, List<Square> list, int listPostion)
        {
            if (listPostion >= list.Count)
            {
                return true;
            }

            var ourSquare = list[listPostion];
            for (int i = 1; i < 10; i++)
            {
                ourSquare.value = i;
                if (puz.isValidBoard())
                {
                    if (solveMinus(puz, list, ++listPostion))
                    {
                        return true;
                    }
                }

            }
            ourSquare.value = null;

            return false;
        }

        public static void tempMethod(List<Square> list)
        {
            list.RemoveAt(0);
            Console.WriteLine(list.Count);
        }

        public static T DeepClone<T>(T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }

        public static void printListOfNumbers(List<int> list)
        {
            string printout = "";
            foreach (var item in list)
            {
                printout += item + ",";

            }
            Console.WriteLine(printout);
        }
    }
}
