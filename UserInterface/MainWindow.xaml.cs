using Caduhd.Input.Keyboard;
using Caduhd.UserInterface.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Caduhd.UserInterface
{
    public partial class MainWindow : Window
    {
        private MainViewModel m_mainViewModel;

        public MainWindow()
        {
            InitializeComponent();
            m_mainViewModel = Resources["MainViewModel"] as MainViewModel;
        }

        private void Connect(object sender, RoutedEventArgs e)
        {
            m_mainViewModel.ConnectToDrone();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            m_mainViewModel.HandleKeyEvent(e.Key, KeyState.Down);

        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            m_mainViewModel.HandleKeyEvent(e.Key, KeyState.Up);
        }

        private void StartStreamingDroneVideo(object sender, RoutedEventArgs e)
        {
            m_mainViewModel.StartStreamingDroneVideo();
        }

        private void StopStreamingDroneVideo(object sender, RoutedEventArgs e)
        {
            m_mainViewModel.StopStreamingDroneVideo();
        }

        private void TurnOnWebCamera(object sender, RoutedEventArgs e)
        {
            m_mainViewModel.TurnOnWebCamera();
        }

        private void TurnOffWebCamera(object sender, RoutedEventArgs e)
        {
            m_mainViewModel.TurnOffWebCamera();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            m_mainViewModel.Closed();
        }
    }
}
