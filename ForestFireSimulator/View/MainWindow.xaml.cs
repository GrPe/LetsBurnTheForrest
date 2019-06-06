using ForestFireSimulator.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ForestFireSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller controller;
        Thread thread;

        public MainWindow()
        {
            InitializeComponent();
            controller = new Controller();
            thread = new Thread(new ThreadStart(RenderImage));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] data = { FileTextBox.Text, SelfTextBox.Text, NeighborTextBox.Text, RenewTextBox.Text };
            if (!controller.ParseData(data))
            {
                MessageBox.Show("Invalid input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (thread.IsAlive)
            {
                thread.Abort();
            }
            else
            {
                thread = new Thread(new ThreadStart(RenderImage));
                thread.Start();
            }
        }

        private void RenderImage()
        {

            foreach (System.Drawing.Image img in controller.StartSimulation())
            {
                using (var ms = new MemoryStream())
                {
                    this.Dispatcher.Invoke(delegate
                    {
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;
                        var bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.StreamSource = ms;
                        bi.EndInit();
                        IMG.Source = bi;
                    });
                }
                Thread.Sleep(400);
            }

        }
    }

}
