namespace Caduhd.UserInterface
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using Caduhd.UserInterface.ViewModel;

    /// <summary>
    /// The main window of the application.
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel mainViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.mainViewModel = this.Resources["MainViewModel"] as MainViewModel;
        }

        private void Connect(object sender, RoutedEventArgs e)
        {
            this.mainViewModel.ConnectToDrone();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            this.HandleKeyEvent(e);
        }

        private void Window_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            this.HandleKeyEvent(e);
        }

        private void HandleKeyEvent(KeyEventArgs keyEventArgs)
        {
            keyEventArgs.Handled = true;

            // if a key is held down, this (PreviewKeyDown) will keep being raised
            if (keyEventArgs.IsRepeat)
            {
                return;
            }

            this.mainViewModel.HandleKeyEvent(keyEventArgs);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.mainViewModel.Dispose();
        }
    }
}
