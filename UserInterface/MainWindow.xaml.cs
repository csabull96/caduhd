using Caduhd.UserInterface.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

namespace Caduhd.UserInterface
{
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _mainViewModel = Resources["MainViewModel"] as MainViewModel;
        }

        private void Connect(object sender, RoutedEventArgs e)
        {
            _mainViewModel.ConnectToDrone();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyEvent(e);
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            HandleKeyEvent(e);
        }

        private void HandleKeyEvent(KeyEventArgs keyEventArgs)
        {
            keyEventArgs.Handled = true;

            // if a key is held down, this (PreviewKeyDown) will keep being raised
            // not sure this makes any sense with the PreviewKeyUp event
            if (keyEventArgs.IsRepeat) return;

            _mainViewModel.HandleKeyEvent(keyEventArgs);
        }

        private void StartStreamingDroneVideo(object sender, RoutedEventArgs e)
        {
            _mainViewModel.StartStreamingDroneVideo();
        }

        private void StopStreamingDroneVideo(object sender, RoutedEventArgs e)
        {
            _mainViewModel.StopStreamingDroneVideo();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _mainViewModel.Dispose();
        }
    }
}
