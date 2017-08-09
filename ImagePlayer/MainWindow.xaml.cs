using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImagePlayer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Main.DataContext = this;
            for (double i = 0.25; i < 8; i*=2)
            {
                ComboBoxSpeed.Items.Add(i);
            }
            
            
        }

        private string[] _fileNames;
        private List<ImageSource> _imageSources;

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

            _imageSources = new List<ImageSource>();
            foreach (var t in _fileNames)
            {
                _imageSources.Add(new BitmapImage(new Uri(t)));
            }
            ImageSourceList = _imageSources;
        }

        private List<ImageSource> _imageSourceList;

        public List<ImageSource> ImageSourceList
        {
            get { return _imageSourceList; }
            set
            {
                _imageSourceList = value;
                OnPropertyChanged();
            }
        }

        private bool _startPlay;

        public bool StartPlay
        {
            get { return _startPlay; }
            set
            {
                _startPlay = value;
                OnPropertyChanged();
            }
        }

        private int _currentFrameIndex;

        public int CurrentFrameIndex
        {
            get { return _currentFrameIndex; }
            set
            {
                _currentFrameIndex = value;
                OnPropertyChanged();
            }
        }

        private int _totalFrameCount;

        public int TotalFrameCount
        {
            get { return _totalFrameCount; }
            set
            {
                _totalFrameCount = value;
                OnPropertyChanged();
            }
        }

        private double _playSpeed=1.0;

        public double PlaySpeed
        {
            get { return _playSpeed; }
            set
            {
                _playSpeed = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonPlay_OnClick(object sender, RoutedEventArgs e)
        {
            StartPlay = true;
        }

        private void ButtonStop_OnClick(object sender, RoutedEventArgs e)
        {
            StartPlay = false;
        }

        private void ButtonPrev_OnClick(object sender, RoutedEventArgs e)
        {
            if(CurrentFrameIndex>1)
                CurrentFrameIndex--;

        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            if(CurrentFrameIndex<TotalFrameCount)
                CurrentFrameIndex++;

        }
    }
}