using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Life
{
    class Program
    {
        private const Int32 ITERATIONS = Int32.MaxValue;
        private const Int32 WORLD_WIDTH = 111;
        private const Int32 WORLD_HEIGHT = 72;
        private const Int32 ITERATION_INTERVAL = 50;

        private static Random s_random = new Random();


        static void Main(string[] args)
        {
            Console.SetWindowSize(Console.LargestWindowWidth - 3, Console.LargestWindowHeight - 3);
            Console.SetBufferSize(Console.LargestWindowWidth - 3, Console.LargestWindowHeight - 3);

            Maximize();

            Grid world = new Grid(WORLD_WIDTH, WORLD_HEIGHT);
            Seed(world);

            for (Int32 iteration = 1; iteration < ITERATIONS; iteration++)
            {
                world = Iterate(world);
                Draw(iteration, world);
                Thread.Sleep(ITERATION_INTERVAL);
            }

            Console.ReadLine();
        }

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);

        private static void Maximize()
        {
            Process p = Process.GetCurrentProcess();
            ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3
        }

        private static void Seed(Grid world)
        {
            for (Int32 j = 0; j < world.Height; j++)
                for (Int32 i = 0; i < world.Width; i++)
                    world.Cells[i, j] = s_random.Next(3) == 0 ? 1 : 0;
        }

        private static void Draw(Int32 iteration, Grid world)
        {
            Grid neighborGrid = ComputeNeighborGrid(world);

            StringBuilder builder = new StringBuilder();

            for (Int32 j = 0; j < world.Height; j++)
            {
                builder.Append(" ");
                for (Int32 i = 0; i < world.Width; i++)
                {
                    builder.Append(world.Cells[i, j] == 1 ? "#" : " ");
                }

                builder.Append(" | ");

                for (Int32 i = 0; i < neighborGrid.Width; i++)
                {
                    builder.Append(neighborGrid.Cells[i, j]);
                }

                builder.AppendLine();
            }

            builder.AppendLine();

            Console.Clear();
            Console.WriteLine("===== Iteration {0:00000} =====\n", iteration);
            Console.Write(builder.ToString());
        }

        private static Grid ComputeNeighborGrid(Grid world)
        {
            Grid neighborGrid = new Grid(world.Width, world.Height);

            for (Int32 j = 0; j < world.Height; j++)
                for (Int32 i = 0; i < world.Width; i++)
                    neighborGrid.Cells[i, j] = CountNeigbors(i, j, world);

            return neighborGrid;
        }

        private static Grid Iterate(Grid world)
        {
            Grid neigbors = ComputeNeighborGrid(world);
            Grid nextIteration = new Grid(world.Width, world.Height);

            for (Int32 j = 0; j < world.Height; j++)
                for (Int32 i = 0; i < world.Width; i++)
                    nextIteration.Cells[i, j] = ApplyRules(i, j, world, neigbors);

            return nextIteration;
        }

        private static Int32 ApplyRules(Int32 x, Int32 y, Grid world, Grid neighbors)
        {
            Int32 newCellValue;

            switch(world.Cells[x,y])
            {
                case 0:
                    newCellValue = neighbors.Cells[x,y] == 3 || neighbors.Cells[x,y] == 6 ? 1 : 0;
                    break;
                case 1:
                    newCellValue = neighbors.Cells[x, y] == 2 || neighbors.Cells[x, y] == 3 ? 1 : 0;
                    break;
                default:
                    throw new InvalidOperationException("Expecting 0 or 1 for the workd cell value");
            }

            return newCellValue;
        }

        private static Int32 CountNeigbors(Int32 x, Int32 y, Grid grid)
        {
            Int32 neighborCount = 0;

            for (Int32 j = -1; j <= 1; j++)
            {
                for (Int32 i = -1; i <= 1; i++)
                {
                    Int32 nx = x + i;
                    Int32 ny = y + j;

                    if (nx < 0 || nx >= grid.Width || ny < 0 || ny >= grid.Height)
                        continue;
                    if (nx == x && ny == y)
                        continue;

                    neighborCount += grid.Cells[nx, ny];
                }
            }

            return neighborCount;
        }

        private static Grid CopyGrid(Grid original)
        {
            Grid copy = new Grid(original.Width, original.Height);

            for (Int32 j = 0; j < original.Height; j++)
                for (Int32 i = 0; i < original.Width; i++)
                    copy.Cells[i, j] = original.Cells[i, j];

            return copy;
        }
    }
}
