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
using System.Windows.Shapes;

namespace GameStoreDatabase
{
    /// <summary>
    /// Interaction logic for InStockWindow.xaml
    /// </summary>
    public partial class InStockWindow : Window
    {
        Backstage _backstage = new Backstage();
        List<String> ListOfSets = new List<String>();
        List<String> CardsInInventory = new List<String>();
        public InStockWindow()
        {
            InitializeComponent();
            ListOfSets = _backstage.GetAllSets();
            ListOfSets.Sort();
            ListOfSetscb.ItemsSource = ListOfSets;
        }

        private void OnClick_SubmitQuery(object sender, RoutedEventArgs e)
        {
            ListOfInventorylst.ItemsSource = null;
            ListOfInventorylst.Items.Clear();
            if(ListOfSetscb.SelectedItem != null && CardNametxt.Text == "")
            {
                CardsInInventory = _backstage.GetInventoryBasedOnSet(ListOfSetscb.Text);
                ListOfInventorylst.ItemsSource = CardsInInventory;
            }
            else if(ListOfSetscb.Text == "" && CardNametxt.Text != "")
            {
                CardsInInventory = _backstage.GetInventoryBasedOnCardName(CardNametxt.Text);
                ListOfInventorylst.ItemsSource = CardsInInventory;
                if(CardsInInventory.Count == 1)
                {
                    MessageBox.Show("You may have misspelled the card.");
                }
            }
            else if(ListOfSetscb.Text == "" && CardNametxt.Text == "")
            {
                MessageBox.Show("Please enter a card name or choose a set.");
            }
            else {
                CardsInInventory = _backstage.GetInventoryBasedOnCardAndSet(ListOfSetscb.Text, CardNametxt.Text);
                ListOfInventorylst.ItemsSource = CardsInInventory;
            }
        }

        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
