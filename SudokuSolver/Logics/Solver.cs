using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SudokuSolver.Logics
{
    public class Solver
    {
        //https://www.c-sharpcorner.com/blogs/sudoku-solver
        //https://github.com/Aghost/SolveSudokuFunction/blob/main/SudokuSolver/Logics/Solver.cs
        //https://www.sudokudragon.com/guideguess.htm

        int Iterations;

        public int[][] Solve(int[][] sudoku)
        {
            Iterations = 0;

            do
            {
                FillSudoku(sudoku);
            } while (FillSudoku(sudoku) == false);

            return sudoku;
        }

        public int[][] Create(int[][] sudoku)
        {
            do
            {
                FillSudokuRandom(sudoku);
            } while (FillSudokuRandom(sudoku) == false);

            RemoveRandomNumbers(sudoku);

            return sudoku;
        }
        public bool FillSudokuRandom(int[][] sudoku)
        {
            Random Random = new Random();

            for (int Row = 0; Row < 9; Row++)
            {
                for (int Column = 0; Column < 9; Column++)
                {
                    if (sudoku[Row][Column] == 0)
                    {
                        for (int Num = 1; Num <= 9; Num++)
                        {
                            int RandomNum = Random.Next(1, 10);
                            if (ValidateCell(sudoku, Row, Column, RandomNum) == true)
                            {
                                sudoku[Row][Column] = RandomNum;

                                if (FillSudoku(sudoku) == true)
                                    return true;
                                else
                                    sudoku[Row][Column] = 0;
                            }
                        }
                        return false;
                    }
                }
            }
            if (AllCellsFilled(sudoku) == true)
                return true;
            else
                return FillSudoku(sudoku);
        }
        public int[][] RemoveRandomNumbers(int[][] sudoku)
        {
            Random Random = new Random();
            int AmountCellsDeleted = Random.Next(55, 70);

            for (int i = 0; i < AmountCellsDeleted; i++)
            {
                for (int x = 0; x < 9; x++)
                {
                    for (int y = 0; y < 9; y++)
                    {
                        if (Random.Next(100) < 50 && sudoku[x][y] != 0)
                        {
                            sudoku[x][y] = 0;
                            AmountCellsDeleted -= 1;

                            if (AmountCellsDeleted == 0)
                                return sudoku;
                        }
                    }
                }
            }
            return sudoku;
        }
        public bool FillSudoku(int[][] sudoku)
        {
            Iterations += 1;
            for (int Row = 0; Row < 9; Row++)
            {
                for (int Column = 0; Column < 9; Column++)
                {
                    if (sudoku[Row][Column] == 0)
                    {
                        if (Iterations < 200)
                        {
                            var OnePossibility = OnePossiblityInCell(sudoku, Row, Column);
                            if (OnePossibility != 0)
                            {
                                sudoku[Row][Column] = OnePossibility;
                                continue;
                            }
                        }

                        if (Iterations >= 200)
                        {
                            for (int Num = 1; Num <= 9; Num++)
                            {
                                if (ValidateCell(sudoku, Row, Column, Num) == true)
                                {
                                    sudoku[Row][Column] = Num;

                                    if (FillSudoku(sudoku) == true)
                                        return true;
                                    else
                                        sudoku[Row][Column] = 0;
                                }
                            }
                            return false;
                        }
                    }
                }
            }
            if (AllCellsFilled(sudoku) == true)
                return true;
            else
                return FillSudoku(sudoku);
        }
        public bool ValidateCell(int[][] sudoku, int row, int column, int num)
        {
            for (int i = 0; i < 9; i++)
                if (sudoku[row][i] == num || sudoku[i][column] == num)
                    return false;

            int rowStart = (row / 3) * 3;
            int colStart = (column / 3) * 3;

            for (int k = rowStart; k < (rowStart + 3); ++k)
            {
                for (int j = colStart; j < (colStart + 3); ++j)
                {
                    if (sudoku[k][j] == num)
                        return false;
                }
            }

            return true;
        }
        public bool AllCellsFilled(int[][] sudoku)
        {
            if (sudoku.Any(s => s.Any(w => w == 0)))
                return false;
            else
                return true;
        }
        public int OnePossiblityInCell(int[][] sudoku, int Row, int Column)
        {
            HashSet<int> PossibleNums = new HashSet<int>();

            for (int Num = 1; Num <= 9; Num++)
            {
                if (ValidateCell(sudoku, Row, Column, Num) == true)
                {
                    PossibleNums.Add(Num);
                }
                if (PossibleNums.Count() == 1 && Num == 9)
                {
                    return PossibleNums.First();
                }
            }
            PossibleNums.Clear();

            return 0;
        }
    }
}