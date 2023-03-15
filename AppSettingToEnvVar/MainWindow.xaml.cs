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
using AppSettingToEnvVar.services;

namespace AppSettingToEnvVar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IConvertJsonToEnvVarsService _convertJsonToEnvVarsService;

        public MainWindow(IConvertJsonToEnvVarsService convertJsonToEnvVarsService)
        {
            _convertJsonToEnvVarsService = convertJsonToEnvVarsService;
            InitializeComponent();
        }

        private async void ButtonConvert_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                EnvVarText.Text = await _convertJsonToEnvVarsService.Convert(AppSettingsText.Text);
                EnvVarText.Foreground=Brushes.Black;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                EnvVarText.Text = exception.Message;
                EnvVarText.Foreground=Brushes.DarkRed;
            }
        }

        private void ButtonCopyToClipboard_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(EnvVarText.Text);
        }
    }
}