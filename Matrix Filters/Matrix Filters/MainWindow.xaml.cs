using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Matrix_Filters.Annotations;
using Microsoft.Win32;

namespace Matrix_Filters {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged {
        public WriteableBitmap ProcessedBitmap { get; set; }

        private WriteableBitmap _lastLoadedBitmap;

        public ObservableCollection<Filter> DefinedFilters { get; set; }

        private Filter _activeFilter;

        public Filter ActiveFilter {
            get { return _activeFilter; }
            set {
                _activeFilter = value;
                OnPropertyChanged();
            }
        }

        public Histogram RedHistogram { get; set; }
        public Histogram GreenHistogram { get; set; }
        public Histogram BlueHistogram { get; set; }

        public Stack<WriteableBitmap> PreviousBitmaps { get; set; }

        public MainWindow() {
            DefinedFilters = new ObservableCollection<Filter>
            {
                new Filter
                {
                    Name = "Define new filter",
                    Divisor = 1,
                    Shift = 0,
                    FilterMatrix = new Matrix3X3(0, 0, 0, 0, 0, 0, 0, 0, 0)
                },
                new Filter
                {
                    Name = "Blur",
                    Divisor = 9,
                    Shift = 0,
                    FilterMatrix = new Matrix3X3(1, 1, 1, 1, 1, 1, 1, 1, 1)
                },
                new Filter
                {
                    Name = "Gaussian blur",
                    Divisor = 8,
                    Shift = 0,
                    FilterMatrix = new Matrix3X3(0, 1, 0, 1, 4, 1, 0, 1, 0)
                },
                new Filter
                {
                    Name = "Edge detection",
                    Divisor = 1,
                    Shift = 127,
                    FilterMatrix =  new Matrix3X3(-1, -1, -1, -1, 8, -1, -1, -1, -1)
                },
                new Filter
                {
                    Name = "Mean removal",
                    Divisor = 1,
                    Shift = 0,
                    FilterMatrix = new Matrix3X3(-1, -1, -1, -1, 9, -1, -1, -1, -1)
                },
                new Filter
                {
                    Name = "Emboss",
                    Divisor = 1,
                    Shift = 128,
                    FilterMatrix = new Matrix3X3(-1, -1, 0, -1, 0, 1, 0, 1, 1)
                }
            };
            ActiveFilter = new Filter(DefinedFilters[0]);
            InitializeComponent();
            FilterComboBox.SelectedIndex = 0;

            PreviousBitmaps = new Stack<WriteableBitmap>();
            InitHistograms();
            LoadSampleImage();
        }

        private void InitHistograms() {
            RedHistogram = new Histogram(Colors.Red);
            RedBorder.Child = RedHistogram;

            GreenHistogram = new Histogram(Colors.Green);
            GreenBorder.Child = GreenHistogram;

            BlueHistogram = new Histogram(Colors.Blue);
            BlueBorder.Child = BlueHistogram;
        }

        private void UpdateHistograms() {
            RedHistogram.UpdateHistogram(ProcessedBitmap);
            GreenHistogram.UpdateHistogram(ProcessedBitmap);
            BlueHistogram.UpdateHistogram(ProcessedBitmap);
        }

        private void LoadImageButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            LoadImageFromFile();
        }

        private void LoadImageFromFile() {
            var fileDialog = new OpenFileDialog {
                Filter = "All files (*.*)|*.*|PNG files (*.png)|*.png|JPG files (*.jpg)|(*.jpg)|JPEG files (*.jpeg)|(*.jpeg)|BMP files (*bmp)|(*.bmp)"
            };

            var dialogResult = fileDialog.ShowDialog();

            if (dialogResult != null && !dialogResult.Value) return;

            var filePath = fileDialog.FileName;
            var loadedBitmapImage = new BitmapImage(new Uri(filePath));
            ProcessedBitmap = new WriteableBitmap(loadedBitmapImage);
            _lastLoadedBitmap = new WriteableBitmap(loadedBitmapImage);
            SetDisplayedImage(ProcessedBitmap);
        }

        private void SetDisplayedImage(BitmapSource loadedBitmapImage) {
            if (loadedBitmapImage == null) throw new ArgumentNullException("loadedBitmapImage");
            DisplayedImage.Source = loadedBitmapImage;
            UpdateHistograms();
        }

        private void SaveImageButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            var saveFileDialog = new SaveFileDialog { FileName = "file.bmp", DefaultExt = ".bmp" };

            var dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult != null && !dialogResult.Value) return;

            var filePath = saveFileDialog.FileName;
            SaveBitmapToFile(filePath, ProcessedBitmap);
        }

        private void SaveBitmapToFile(string filename, BitmapSource bitmap) {
            if (filename == string.Empty) return;

            using (var fileStream = new FileStream(filename, FileMode.Create)) {
                var encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmap));
                encoder.Save(fileStream);
                fileStream.Close();
            }
        }

        private void ApplyFilterButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            var destinationBitmap = ProcessedBitmap.Convolute(ActiveFilter);

            PreviousBitmaps.Push(ProcessedBitmap);
            ProcessedBitmap = destinationBitmap;

            SetDisplayedImage(ProcessedBitmap);
        }

        private void ReloadImageButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            ProcessedBitmap = new WriteableBitmap(_lastLoadedBitmap);
            SetDisplayedImage(_lastLoadedBitmap);
        }

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var selectedFilter = FilterComboBox.SelectedItem as Filter;
            ActiveFilter = new Filter(selectedFilter);
        }

        private void SaveFilterButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DefinedFilters.Any(definedFilter => definedFilter.Name == ActiveFilter.Name))
            {
                return;
            }
            DefinedFilters.Add(ActiveFilter);
            ActiveFilter = ActiveFilter.Clone();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void LoadSampleImageButton_Click(object sender, System.Windows.RoutedEventArgs e) {
            LoadSampleImage();
        }
        private void LoadSampleImage() {
            BitmapSource source = new BitmapImage(new Uri("pack://application:,,,/Matrix Filters;component/Resources/Lenna.png", UriKind.Absolute));
            ProcessedBitmap = new WriteableBitmap(source);
            _lastLoadedBitmap = new WriteableBitmap(source);
            SetDisplayedImage(ProcessedBitmap);
        }

        private void UndoFilterButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (PreviousBitmaps.Count == 0) return;
            var lastBitmap = PreviousBitmaps.Pop();
            ProcessedBitmap = lastBitmap;
            SetDisplayedImage(lastBitmap);
        }
    }
}
