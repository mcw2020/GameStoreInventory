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
    /// Interaction logic for CreateTableWindow.xaml
    /// </summary>
    public partial class CreateMagicSetWindow : Window
    {
        public Backstage _backstage = new Backstage();
        public CreateMagicSetWindow()
        {
            InitializeComponent();
        }

        private void OnClick_CreateSet(object sender, RoutedEventArgs e)
        {
            //The front end logic for creating a new table in the database will be here.
            //There will be two Message Boxes. The first will make sure the set name is correct. A different prompt will come up if the set name is blank
            //The second will let the user know the table is created.
            //TODO: There needs to be logic to both save the set name, but allow it to be the table name (Ikoria: Lair needs to keep the colon, but Mysql throws a fit if it's the table name)
            //Probably going to use a list and then use that list to find table names. For testing I'll still to alphanumeric
            if(SetNametxt.Text != "")
            {
                MessageBoxResult result = MessageBox.Show("Are you sure the set name is correct?", "", MessageBoxButton.YesNo);
                switch(result)
                {
                    case MessageBoxResult.Yes:
                        _backstage.CreateNewSet(SetNametxt.Text, CreateCardsInSet());
                        MessageBox.Show("The set has been created! You can update it in the Set Update page.");
                        SetNametxt.Text = "";
                        CardNamestxt.Text = "";
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("Please enter the name that you want for the set and then submit again.");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("Please enter a set name.");
            }
        }

        private List<String> CreateCardsInSet()
        {
            List<String> CardsInSet = new List<String>();

            string[] lines = CardNamestxt.Text.Split
            (
                new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries
            );

            foreach (var line in lines)
            {
                CardsInSet.Add(line);
            }

            return CardsInSet;
        }


        private void OnClick_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
