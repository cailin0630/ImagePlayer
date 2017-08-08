using ImagePlayer.Annotations;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImagePlayer
{
    /// <summary>
    /// ImagePlayer.xaml 的交互逻辑
    /// </summary>
    public partial class MyImagePlayer : UserControl, INotifyPropertyChanged
    {
        public MyImagePlayer()
        {
            InitializeComponent();
            MyImagePlayerTitle.DataContext = this;
            for (int i = 1; i < 10; i++)
            {
                ComboBoxSpeed.Items.Add(i);
            }
            ComboBoxSpeed.SelectedIndex = 0;
        }

        private string[] _fileNames;
        private ImageSource[] _imageSources;
        private int _speed;
        private bool _running;

        private void ButtonOpen_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "图片文件|*.jpg;*.bmp;*.png;",
                Multiselect = true
            };
            if (!openFileDialog.ShowDialog() ?? false)
                return;
            if (openFileDialog.FileNames == null)
                return;
            _fileNames = openFileDialog.FileNames;

            _imageSources = new ImageSource[_fileNames.Length];
            for (var i = 0; i < _fileNames.Length; i++)
            {
                _imageSources.SetValue(new BitmapImage(new Uri(_fileNames[i])), i);
            }
            TotalCount = _imageSources.Length;
            CurrentCount = 1;
            ImageSingle.Source = _imageSources[CurrentCount];
        }

        private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
        {
            _running = true;

            Task.Factory.StartNew(() =>
            {
                while (_running)
                {
                    var currentIndex = CurrentCount = CurrentCount == TotalCount ? 0 : CurrentCount - 1;
                    for (var i = currentIndex; i < _imageSources.Length; i++)

                    {
                        Dispatcher.Invoke(() =>
                        {
                            ImageSingle.Source = _imageSources[i];
                            CurrentCount = i + 1;
                            _speed = int.Parse(ComboBoxSpeed.SelectedValue.ToString());
                        });
                        Thread.Sleep(1000 / _speed);
                        if (!_running)
                            break;
                    }
                }
            });
        }

        private int _currentCount;

        public int CurrentCount
        {
            get { return _currentCount; }
            set
            {
                _currentCount = value;
                OnPropertyChanged();
            }
        }

        private int _totalCount;

        public int TotalCount
        {
            get { return _totalCount; }
            set
            {
                _totalCount = value;
                OnPropertyChanged();
            }
        }

        private void ButtonPause_OnClick(object sender, RoutedEventArgs e)
        {
            _running = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonPrev_OnClick(object sender, RoutedEventArgs e)
        {
            if (CurrentCount <= 1)
                return;
            CurrentCount--;
            ImageSingle.Source = _imageSources[CurrentCount - 1];
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            if (CurrentCount >= TotalCount)
                return;
            CurrentCount++;
            ImageSingle.Source = _imageSources[CurrentCount - 1];
        }
    }
}