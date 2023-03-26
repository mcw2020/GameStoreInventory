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

namespace GameStoreDatabase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Backstage _backstage = new Backstage();
        public MainWindow()
        {
            InitializeComponent();
            _backstage.CreateDatabase();
        }

        private void OnClick_OpenCreateMagicSetWindow(object sender, RoutedEventArgs e)
        {
            CreateMagicSetWindow createMagicSetWindow = new CreateMagicSetWindow();
            createMagicSetWindow.Owner = this;
            createMagicSetWindow.ShowDialog();
        }

        private void OnClick_OpenInStockWindow(object sender, RoutedEventArgs e)
        {
            InStockWindow inStockWindow = new InStockWindow();
            inStockWindow.Owner = this;
            inStockWindow.ShowDialog();
        }

        private void OnClick_OpenCreateUpdateSetWindow(object sender, RoutedEventArgs e)
        {
            UpdateSetWindow updateSetWindow = new UpdateSetWindow();
            updateSetWindow.Owner = this;
            updateSetWindow.ShowDialog();
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
