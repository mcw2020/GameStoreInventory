using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using MySql.Data.MySqlClient;

namespace GameStoreDatabase
{
    public class Backstage
    {
        //Called on MainWindow
        public void CreateDatabase()
        {
            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;password=progenitus";
            string createDatabaseCommand = "create database  if not exists magiccardinventory;";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            var cmd = new MySqlCommand(createDatabaseCommand, conn);
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Dispose();

            //Threw a fit whenever I tried to mix these commands up above, so I separated them
            string createTable = $"create table if not exists set_names (id int AUTO_INCREMENT PRIMARY KEY, set_name varchar(255));";
            dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            cmd = new MySqlCommand(createTable, conn);
            cmd.ExecuteNonQuery();

            cmd.Dispose();
            conn.Dispose();
        }

        //Called in InStockWindow
        public List<String> GetAllSets()
        {
            List<String> allSets = new List<String>();

            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            string sql = $"select set_name from set_names;";
            var cmd = new MySqlCommand(sql, conn);

            MySqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                allSets.Add(rdr.GetString(0));
            }

            rdr.Dispose();
            cmd.Dispose();
            conn.Dispose();

            return allSets;
        }

        public List<String> GetCardNamesBasedOnSet(string setName)
        {
            List<String> allSets = new List<String>();
            string setID = "";
            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";

            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            //Find the set id and use that to find the table
            string sql = $"select id from set_names where set_name = \"{setName}\";";
            var cmd = new MySqlCommand(sql, conn);

            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                setID = rdr.GetString(0);
            }

            rdr.Dispose();

            sql = $"select card_name from {setID}a;";

            cmd = new MySqlCommand(sql, conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string str = rdr.GetString(0);
                allSets.Add(str);
            }
            rdr.Dispose();

            cmd.Dispose();
            conn.Dispose();

            return allSets;

        }

        public List<String> GetInventoryBasedOnCardName(string cardName)
        {
            List<Set> Sets = new List<Set>();
            List<String> Inventory = new List<String>();

            Inventory.Add(cardName);

            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            //Get the ids of all the sets
            string sql = $"select * from set_names;";
            var cmd = new MySqlCommand(sql, conn);

            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Set set = new Set(rdr.GetString(1), rdr.GetString(0));
                Sets.Add(set);
            }

            rdr.Dispose();


            //Use each set id to search each table for the card in question
            foreach(var set in Sets)
            {
                sql = $"select in_stock from {set.Id}a where card_name like '{cardName}' ";
                cmd = new MySqlCommand(sql, conn);

                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    string str = $"{set.Name}\nIn Stock: {rdr.GetString(0)}\n";
                    Inventory.Add(str);
                }
                rdr.Dispose();
            }

            cmd.Dispose();
            conn.Dispose();
            return Inventory;
        }

        //Called in InStockWindow
        public List<String> GetInventoryBasedOnSet(string setName)
        {
            List<String> Inventory = new List<String>();
            List<String> SetIds = new List<String>();
            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            //Find the set id and use that to find the table
            string sql = $"select id from set_names where set_name = \"{setName}\";";
            var cmd = new MySqlCommand(sql, conn);

            MySqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                SetIds.Add(rdr.GetString(0));
            }

            rdr.Dispose();

            foreach(var item in SetIds)
            {
                sql = $"select * from {item}a;";
                cmd = new MySqlCommand(sql, conn);
                rdr = cmd.ExecuteReader();

                while(rdr.Read())
                {
                    string str = rdr.GetString(1) + " " + rdr.GetString(2);
                    Inventory.Add(str);
                }
                rdr.Dispose();
            }

            cmd.Dispose();
            conn.Dispose();

            return Inventory;

        }

        //Called in InStockWindow
        public List<String> GetInventoryBasedOnCardAndSet (string set, string card)
        {
            List<String> Inventory = new List<String>();
            string setId = "";

            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            //Find the set in question
            string sql = $"select id from set_names where set_name = \"{set}\";";

            var cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                setId = rdr.GetString(0);
            }

            rdr.Dispose();

            //Search the table for the card
            sql = $"select in_stock from {setId}a where card_name like \"{card}\";";

            cmd = new MySqlCommand(sql, conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                string str = $"{card} from {set}\nIn Stock: {rdr.GetString(0)}\n";
                Inventory.Add(str);
            }

            rdr.Dispose();
            cmd.Dispose();
            conn.Dispose();
            return Inventory;
        }

        public string GetInStockBasedOnCardAndSet(string set, string card)
        {
            string inStock = "";
            string setId = "";

            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            //Find the set in question
            string sql = $"select id from set_names where set_name = \"{set}\";";

            var cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                setId = rdr.GetString(0);
            }

            rdr.Dispose();

            //Search the table for the card
            sql = $"select in_stock from {setId}a where card_name like \"{card}\";";

            cmd = new MySqlCommand(sql, conn);
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                inStock = rdr.GetString(0);
            }

            rdr.Dispose();
            cmd.Dispose();
            conn.Dispose();
            return inStock;
        }


        //Called in CreateMagicSetWindow
        public void CreateNewSet(string setName, List<String> cardsInSet)
        {
           
            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            //Inserts new set into set_names
            string sql = $"insert into set_names (set_name) select * from (select \"{setName}\") as temp where not exists (select set_name from set_names where set_name = \"{setName}\") limit 1;";

            var cmd = new MySqlCommand(sql, conn);

            cmd.ExecuteNonQuery();
            
            
            
            sql = $"select id from set_names where set_name = \"{setName}\";";
            cmd = new MySqlCommand(sql, conn);

            MySqlDataReader rdr = cmd.ExecuteReader();

            string tableName = "";
            while(rdr.Read())
            {
                //Note the tableName + a. The a (or any other letter) has to be there as part of the name because of naming conventions
                tableName = rdr.GetString(0) + "a";
            }

            string createTable = $"create table if not exists {tableName} (id int AUTO_INCREMENT PRIMARY KEY, card_name varchar(255), in_stock int default 0);";

            rdr.Close();

            //This will create the table. After this we'll input the values;
            cmd = new MySqlCommand(createTable, conn);
            cmd.ExecuteNonQuery();

            foreach(var card in cardsInSet)
            {
                string temp = card.TrimEnd(' ', '\t', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.');
                string insertInto = $"insert into {tableName} (card_name) select * from (select \"{temp}\") as temp where not exists (select card_name from {tableName} where card_name = \"{card}\") limit 1;";
                cmd = new MySqlCommand(insertInto, conn);
                cmd.ExecuteNonQuery();
            }



            cmd.Dispose();
            conn.Dispose();
        }

        //This will add a specified number to a card's in_stock
        public void AddToInventory(int amount, string set, string card)
        {
            string setId = "";

            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            //Find the set in question
            string sql = $"select id from set_names where set_name = \"{set}\";";

            var cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                setId = rdr.GetString(0);
            }

            rdr.Dispose();       

            //Search the table for the card
            sql = $"update {setId}a set in_stock = in_stock + {amount} where card_name like \"{card}\";";

            cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            rdr.Dispose();
            cmd.Dispose();
            conn.Dispose();
        }

        public void SubtractFromInventory(int amount, string set, string card)
        {
            string setId = "";

            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            //Find the set in question
            string sql = $"select id from set_names where set_name = \"{set}\";";

            var cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                setId = rdr.GetString(0);
            }

            rdr.Dispose();

            //Search the table for the card
            sql = $"update {setId}a set in_stock = in_stock - {amount} where card_name like \"{card}\";";

            cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            rdr.Dispose();
            cmd.Dispose();
            conn.Dispose();
        }

        public void SetInventoryAmount(int amount, string set, string card)
        {
            string setId = "";

            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            //Find the set in question
            string sql = $"select id from set_names where set_name = \"{set}\";";

            var cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                setId = rdr.GetString(0);
            }

            rdr.Dispose();

            //Search the table for the card
            sql = $"update {setId}a set in_stock = {amount} where card_name like \"{card}\";";

            cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            rdr.Dispose();
            cmd.Dispose();
            conn.Dispose();
        }

        public void ChangeSetName(string oldName, string newName)
        {
            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            //Find the set in question
            string sql = $"update set_names set set_name = \"{newName}\" where set_name = \"{oldName}\";";

            var cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
                     
            cmd.Dispose();
            conn.Dispose();
        }

        public void ChangeCardName(string setName, string oldName, string newName)
        {
            string setId = "";

            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            //Find the set in question
            string sql = $"select id from set_names where set_name = \"{setName}\";";

            var cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                setId = rdr.GetString(0);
            }

            rdr.Dispose();

            //Search the table for the card
            sql = $"update {setId}a set card_name = \"{newName}\" where card_name like \"{oldName}\";";

            cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            rdr.Dispose();
            cmd.Dispose();
            conn.Dispose();
        }

        public void InsertCardIntoSet(string setName, string cardName)
        {
            string setId = "";

            string dbConnectionString = "server=localhost;user=root;database=magiccardinventory;port=3306;password=progenitus";
            var conn = new MySqlConnection(dbConnectionString);
            conn.Open();

            //Find the set in question
            string sql = $"select id from set_names where set_name = \"{setName}\";";

            var cmd = new MySqlCommand(sql, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                setId = rdr.GetString(0);
            }

            rdr.Dispose();

            //Search the table for the card
            sql = $"insert into {setId}a (card_name) values (\"{cardName}\");";

            cmd = new MySqlCommand(sql, conn);
            cmd.ExecuteNonQuery();

            rdr.Dispose();
            cmd.Dispose();
            conn.Dispose();
        }
    }
}
