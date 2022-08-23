using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;

namespace ImageConverter
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String filePath = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void fileChooseBut_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("Выберите файл для конвертации", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            

            Stream imageStreamSource = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            PngBitmapDecoder decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            FormatConvertedBitmap newFormatedBitmapSource = new FormatConvertedBitmap();
            newFormatedBitmapSource.BeginInit();
            newFormatedBitmapSource.Source = bitmapSource;
            newFormatedBitmapSource.DestinationFormat = PixelFormats.Gray32Float;
            newFormatedBitmapSource.EndInit();


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "image"; // Default file name
            saveFileDialog.DefaultExt = ".png"; // Default file extension
            saveFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png"; // Filter files by extension
                                                        // Show save file dialog box
            // Process save file dialog box results
            if (saveFileDialog.ShowDialog() == true)
            {
                // Save document
                string filename = saveFileDialog.FileName;
                FileStream stream = new FileStream(filename, FileMode.Create);
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Interlace = PngInterlaceOption.On;
                encoder.Frames.Add(BitmapFrame.Create(newFormatedBitmapSource));
                encoder.Save(stream);
            }
        }

        private void fileChooseBut_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == true)
                {
                    filePath = openFileDialog.FileName;
                    pathBox.Text = filePath;
                }
            }
        }
    }
}
