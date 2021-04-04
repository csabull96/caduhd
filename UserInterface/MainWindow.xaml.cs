using Caduhd.UserInterface.ViewModel;
using System;
using System.Windows;
using System.Windows.Input;

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

            m_mainViewModel.HandleKeyEvent(keyEventArgs);
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
