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
    /// Interaction logic for UpdateSetWindow.xaml
    /// </summary>
    public partial class UpdateSetWindow : Window
    {
        List<String> ListOfSets = new List<String>();
        List<String> CardsInSet = new List<String>(); 
        Backstage backstage = new Backstage();
        public UpdateSetWindow()
        {
            InitializeComponent();

            ListOfSets = backstage.GetAllSets();
            ListOfSets.Sort();
            SetListcmb.ItemsSource = ListOfSets;
            PreSetscmb.ItemsSource = ListOfSets;
            ChangeCardNameSetNamescmb.ItemsSource = ListOfSets;
            InsertCardSetNamescmb.ItemsSource = ListOfSets;

        }

        private void OnClick_LookupCard(object sender, RoutedEventArgs e)
        {
            string inStock = backstage.GetInStockBasedOnCardAndSet(SetListcmb.SelectedItem.ToString(), CardNametxt.Text);
            if(inStock.Equals(""))
            {
                MessageBox.Show("Please make sure the card name and set is correct.");
            }
            else
            {
                AmountInInventorytxt.Text = inStock;
            }
        }

        private void OnClick_AddToInventory(object sender, RoutedEventArgs e)
        {
            if (AmountInInventorytxt.Text.Equals(""))
            {
                MessageBox.Show("Please look up a card to get an in_stock value.");
            }
            else if (AmountToAddtxt.Text.Equals(""))
            {
                MessageBox.Show("Please enter an amount to add.");
            }
            else
            {
                int amountToAdd = int.Parse(AmountToAddtxt.Text);

                backstage.AddToInventory(amountToAdd, SetListcmb.SelectedItem.ToString(), CardNametxt.Text);
            }

            AmountToAddtxt.Text = "";

            string inStock = backstage.GetInStockBasedOnCardAndSet(SetListcmb.SelectedItem.ToString(), CardNametxt.Text);
            AmountInInventorytxt.Text = inStock;
        }

        private void OnClick_SubtractFromInventory(object sender, RoutedEventArgs e)
        {
            if(AmountInInventorytxt.Text.Equals(""))
            {
                MessageBox.Show("Please look up a card to get an in_stock value.");
            }
            else if (AmountToRemovetxt.Text.Equals(""))
            {
                MessageBox.Show("Please enter an amount to subtract.");
            }
            else if (int.Parse(AmountToRemovetxt.Text) > int.Parse(AmountInInventorytxt.Text))
            {
                MessageBox.Show("You're trying to subtract more than you have in stock.");
            }
            else
            {
                int amountToSubtract = int.Parse(AmountToRemovetxt.Text);

                backstage.SubtractFromInventory(amountToSubtract, SetListcmb.SelectedItem.ToString(), CardNametxt.Text);
            }

            AmountToRemovetxt.Text = "";

            string inStock = backstage.GetInStockBasedOnCardAndSet(SetListcmb.SelectedItem.ToString(), CardNametxt.Text);
            AmountInInventorytxt.Text = inStock;
        }

        private void OnClick_ManuallyUpdateInventory(object sender, RoutedEventArgs e)
        {
            if (AmountInInventorytxt.Text.Equals(""))
            {
                MessageBox.Show("Please look up a card to get an in_stock value.");
            }
            else if (Manualtxt.Text.Equals(""))
            {
                MessageBox.Show("Please enter an amount to set inventory to.");
            }
            else if (int.Parse(Manualtxt.Text) < 0)
            {
                MessageBox.Show("You're trying to set a negative value.");
            }
            else
            {
                int amountToSet = int.Parse(Manualtxt.Text);

                backstage.SetInventoryAmount(amountToSet, SetListcmb.SelectedItem.ToString(), CardNametxt.Text);
            }

            Manualtxt.Text = "";

            string inStock = backstage.GetInStockBasedOnCardAndSet(SetListcmb.SelectedItem.ToString(), CardNametxt.Text);
            AmountInInventorytxt.Text = inStock;
        }

        private void OnClick_ChangeSetName(object sender, RoutedEventArgs e)
        {
            if(PreSetscmb.SelectedItem.ToString().Equals("") || NewSetstxt.Text.Equals("")) {
                MessageBox.Show("Please choose a set and decide what to rename it.");
            }
            else
            {
                backstage.ChangeSetName(PreSetscmb.SelectedItem.ToString(), NewSetstxt.Text);
                NewSetstxt.Text = "";
                ListOfSets = backstage.GetAllSets();
                ListOfSets.Sort();
                PreSetscmb.ItemsSource = ListOfSets;
            }

        }

        private void OnSelection_UpdateCardsInSet(object sender, SelectionChangedEventArgs e)
        {
            if(ChangeCardNameSetNamescmb.SelectedItem != null)
            {
                CardsInSet = backstage.GetCardNamesBasedOnSet(ChangeCardNameSetNamescmb.SelectedItem.ToString());
                ChangeCardNameCardNamescmb.ItemsSource = CardsInSet;
            }
        }

        private void OnClick_ChangeCardName(object sender, RoutedEventArgs e)
        {
           if(ChangeCardNameCardNamescmb.SelectedItem != null && !ChangeCardNameNewNametxt.Text.Equals(""))
            {
                backstage.ChangeCardName(ChangeCardNameSetNamescmb.SelectedItem.ToString(),ChangeCardNameCardNamescmb.SelectedItem.ToString(), ChangeCardNameNewNametxt.Text);
                ChangeCardNameNewNametxt.Text = "";
                ChangeCardNameSetNamescmb.ItemsSource = null;
                ChangeCardNameSetNamescmb.ItemsSource = ListOfSets;
                ChangeCardNameCardNamescmb.ItemsSource = null;
            }
        }

        private void OnClick_InsertNewCard(object sender, RoutedEventArgs e)
        {
            if(InsertCardSetNamescmb.SelectedItem != null && !InsertCardNametxt.Equals(""))
            {
                backstage.InsertCardIntoSet(InsertCardSetNamescmb.SelectedItem.ToString(), InsertCardNametxt.Text);
                InsertCardNametxt.Text = "";
            }
        }

        private void OnClick_CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
