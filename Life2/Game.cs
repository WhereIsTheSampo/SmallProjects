using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Life2
{
    public class Game
    {
        private Int32 _width;
        private Int32 _height;
        private Byte[,] _world;

        public Game(Int32 worldWidth, Int32 worldHeight)
        {
            _width = worldWidth;
            _height = worldHeight;

            _world = new Byte[_width, _height];
        }

        public event EventHandler WorldUpdated;

        public Int32 Width
        {
            get { return _width; }
        }

        public Int32 Height
        {
            get { return _height; }
        }

        public Byte[,] World
        {
            get { return _world; }
        }

        public Boolean IsRunning { get; private set; }

        private CancellationTokenSource _ctSource;
        private CancellationToken _ct;

        public void Start()
        {
            _ctSource = new CancellationTokenSource();
            _ct = _ctSource.Token;

            var factory = new TaskFactory();
            factory.StartNew(GameRunner);
        }

        public void Stop()
        {
            _ctSource.Cancel();
        }

        private void GameRunner()
        {
            IsRunning = true;

            while(!_ct.IsCancellationRequested)
            {
                Iterate();
                if (WorldUpdated != null)
                    WorldUpdated(this, new EventArgs());

                Thread.Sleep(50);
            }

            IsRunning = false;
        }

        public Int64 Iteration { get; private set; }

        public void Iterate()
        {
            Iteration += 1;

            Int32[,] neighbors = ComputeNeighbors(_world);
            Byte[,] nextIteration = new Byte[_width, _height];

            for (Int32 j = 0; j < _height; j++)
                for (Int32 i = 0; i < _width; i++)
                    nextIteration[i, j] = ApplyRules(i, j, _world, neighbors);

            _world = nextIteration;
        }

        private Byte ApplyRules(Int32 x, Int32 y, Byte[,] world, Int32[,] neighbors)
        {
            Byte newCellValue;

            switch (world[x, y])
            {
                case 0:
                    newCellValue = neighbors[x, y] == 3 ? (Byte)1 : (Byte)0;
                    break;
                case 1:
                    newCellValue = neighbors[x, y] == 2 || neighbors[x, y] == 3 ? (byte)1 : (Byte)0;
                    break;
                default:
                    throw new InvalidOperationException("Expecting 0 or 1 for the workd cell value");
            }

            return newCellValue;
        }

        private Int32[,] ComputeNeighbors(Byte[,] world)
        {
            Int32[,] neighbors = new Int32[_width, _height];

            for (Int32 j = 0; j < _height; j++)
                for (Int32 i = 0; i < _width; i++)
                    neighbors[i, j] = CountNeigbors(i, j, world);

            return neighbors;
        }

        private Int32 CountNeigbors(Int32 x, Int32 y, Byte[,] world)
        {
            Int32 neighborCount = 0;

            for (Int32 j = -1; j <= 1; j++)
            {
                for (Int32 i = -1; i <= 1; i++)
                {
                    Int32 nx = x + i;
                    Int32 ny = y + j;

                    if (nx < 0 || nx >= _width || ny < 0 || ny >= _height)
                        continue;
                    if (nx == x && ny == y)
                        continue;

                    neighborCount += world[nx, ny];
                }
            }

            return neighborCount;
        }


        public void Reset()
        {
            Seed(_world);
        }

        private void Seed(Byte[,] world)
        {
            Random random = new Random();

            for (Int32 j = 0; j < _height; j++)
                for (Int32 i = 0; i < _width; i++)
                    world[i, j] = random.Next(3) == 0 ? (Byte)1 : (Byte)0;
        }

    }
}
