using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandBook.DAL
{
    class SqliteHelperTrans
    { 
        public static void Refresh(BindingSource bsWord)
        {
            List<WordFull>  _list = new List<WordFull>();
            List<WordFull>  list = DAL.SqliteHelperTrans.GetWords();
            if (list != null && list.Count > 0)
            {
                _list.AddRange(list);
                bsWord.DataSource = _list;
                bsWord.ResetBindings(false);
            }           
        }
        public static void UpdateWord(int id, string word, string translation)
        {
            using (SQLiteConnection connection = new SQLiteConnection(@"DataSource=C:\\Users\sotni\\OneDrive\\Рабочий стол\\dataBase.db"))
            {
                connection.Open();
                string query = $"UPDATE words set word = '{word}', translation = '{translation}' where id = '{id}'";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public static void DeleteWord(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(@"DataSource=C:\\Users\sotni\\OneDrive\\Рабочий стол\\dataBase.db"))
            {
                connection.Open();
                string query = $"delete from words where id = '{id}'";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public static void AddWord(string word, string translation)
        {
            using (SQLiteConnection connection = new SQLiteConnection(@"DataSource=C:\\Users\sotni\\OneDrive\\Рабочий стол\\dataBase.db"))
            {
                connection.Open();
                string query = $"insert into words (word, translation) values('{word}', '{translation}')";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }



        internal static List<WordFull> GetWords()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(@"DataSource=C:\\Users\sotni\\OneDrive\\Рабочий стол\\dataBase.db"))
                {
                    connection.Open();
                    string query = @"SELECT * from words";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        { 
                            List<WordFull> words = new List<WordFull>();
                            //int i = 1;
                            while (reader.Read())
                            {

                                words.Add(new WordFull
                                {
                                    Id = reader.GetInt32(0),
                                    Word = reader.GetString(1),
                                    Translation = reader.GetString(2)

                                });
                                //i++;
                            }

                            return words;
                        }
                      
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return null;
        }
         public static void Search(DataGridView dgw, BindingSource bsWord, string searchString)
         {
            dgw.Rows.Clear();
            using (SQLiteConnection connection = new SQLiteConnection(@"DataSource=C:\\Users\sotni\\OneDrive\\Рабочий стол\\dataBase.db"))
            {
                connection.Open();
                string querystring = $"select * from words where word like '%{searchString}%'";
                using (SQLiteCommand command = new SQLiteCommand(querystring, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        List<DAL.WordFull> words = new List<DAL.WordFull>();
                        while (reader.Read())
                        {
                            words.Add(new DAL.WordFull
                            {
                                Id = reader.GetInt32(0),
                                Word = reader.GetString(1),
                                Translation = reader.GetString(2)

                            });

                            List<DAL.WordFull> list = words;
                            if (list != null && list.Count > 0)
                            {
                                bsWord.DataSource = list;
                                bsWord.ResetBindings(false);
                            }
                        }
                    }
                }

            }
        }
    }
}
