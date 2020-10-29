﻿using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfRopeDetectionModel;
using WpfRopeDetectionModel.Models;

namespace WpfRopeDetectionApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string projectDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "../../../"));

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var file = lst_img.SelectedItem as Image;
                string file_name = file.Name + ".jpg";
                string imagesRelativePath = System.IO.Path.Combine(projectDirectory, "TestImages", file_name);
                string file_path = ((BitmapFrame)file.Source).Decoder.ToString();

                // Add input data
                var input = new ModelInput
                {
                    ImagePath = imagesRelativePath,
                    Label = file_name,
                    Image = File.ReadAllBytes(imagesRelativePath)
                };
                // Measure #1 prediction execution time.
                var watch = System.Diagnostics.Stopwatch.StartNew();

                MessageBox.Show("Пожалуйста, подождите...");
                // Load model and predict output of sample data
                ModelOutput result = ConsumeModel.PredictSingleImage(input);
     
                // Stop measuring time.
                watch.Stop();

                var elapsedMs = watch.ElapsedMilliseconds;
                var seconds = TimeSpan.FromMilliseconds(elapsedMs).Seconds;
                MessageBox.Show("Время первого анализа заняло: " + seconds + " секунд");
                OutputPrediction(result);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static void OutputPrediction(ModelOutput prediction)
        {
            string imageName = System.IO.Path.GetFileName(prediction.ImagePath);
            string predictedValue = (prediction.PredictedLabel == "CD") ? "есть дефект" : "дефекта нет";
            MessageBox.Show($"Изображение: {imageName} | Наличие дефекта: {predictedValue}");
        }
    }
}
