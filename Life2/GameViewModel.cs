using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Life2
{
    public class GameViewModel : ViewModelBase
    {
        private Game _game;

        public GameViewModel(Game game)
        {
            _game = game;

            ResetCommand = new RelayCommand(ExecuteReset, CanReset);
            IterateCommand = new RelayCommand(ExecuteIterate, CanIterate);
            StartCommand = new RelayCommand(ExecuteStart, CanStart);
            StopCommand = new RelayCommand(ExecuteStop, CanStop);

            _game.WorldUpdated += (sender, e) => UpdateVmState();
        }

        public ICommand StopCommand { get; private set; }

        private void ExecuteStop()
        {
            _game.Stop();
        }

        private Boolean CanStop()
        {
            return _game.IsRunning;
        }

        public ICommand StartCommand { get; private set; }

        private void ExecuteStart()
        {
            _game.Start();
        }

        private Boolean CanStart()
        {
            return !_game.IsRunning;
        }

        public ICommand ResetCommand { get; private set; }

        private void ExecuteReset()
        {
            _game.Reset();
            UpdateVmState();
        }

        private Boolean CanReset()
        {
            return true;
        }

        public ICommand IterateCommand { get; private set; }

        private void ExecuteIterate()
        {
            _game.Iterate();
            UpdateVmState();
        }

        private Boolean CanIterate()
        {
            return true;
        }

        public ImageSource WorldImage { get; private set; }


        public String Iteration
        {
            get { return String.Format("Iteration: {0:000000}", _game.Iteration); }
        }

        private void UpdateVmState()
        {
            if (!Application.Current.Dispatcher.CheckAccess())
            {
                Application.Current.Dispatcher.Invoke(UpdateVmState);
            }
            else
            {
                Byte[] merged = new Byte[_game.World.Length];
                Buffer.BlockCopy(_game.World, 0, merged, 0, _game.World.Length);

                for (Int32 i = 0; i < merged.Length; i++)
                {
                    merged[i] = merged[i] == 1 ? (Byte)0 : (Byte)200;
                }

                BitmapSource bitmapSource = BitmapSource.Create(
                    _game.Width, _game.Height, 96, 96,
                    PixelFormats.Indexed8,
                    BitmapPalettes.Gray256,
                    merged,
                    _game.Width);

                WorldImage = bitmapSource;
                RaisePropertyChanged(null);
            }
        }
    }
}
