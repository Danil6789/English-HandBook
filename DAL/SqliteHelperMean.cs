using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace HandBook.DAL
{
    class SqliteHelperMean
    {
        public static void UpdateWord(int id, string word, string meaning)
        {
            using (SQLiteConnection connection = new SQLiteConnection(@"DataSource=C:\\Users\sotni\\OneDrive\\Рабочий стол\\dataBase.db"))
            {
                connection.Open();
                string query = $"UPDATE slangs set word = '{word}', meaning = '{meaning}' where id = '{id}'";
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
                string query = $"delete from slangs where id = '{id}'";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }



        public static void Search(DataGridView dgw, BindingSource bsSlang, string searchString)
        {
            dgw.Rows.Clear();
            using (SQLiteConnection connection = new SQLiteConnection(@"DataSource=C:\\Users\sotni\\OneDrive\\Рабочий стол\\dataBase.db"))
            {
                connection.Open();
                string querystring = $"select * from slangs where word like '%{searchString}%'";
                using (SQLiteCommand command = new SQLiteCommand(querystring, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        List<Slang> slangs = new List<Slang>();
                        while (reader.Read())
                        {
                            slangs.Add(new Slang
                            {
                                Id = reader.GetInt32(0),
                                Word = reader.GetString(1),
                                Meaning = reader.GetString(2)

                            });

                            List<Slang> list = slangs;
                            if (list != null && list.Count > 0)
                            {
                                bsSlang.DataSource = list;
                                bsSlang.ResetBindings(false);
                            }
                        }
                    }
                }

            }
        }

        public static void Refresh(BindingSource bsSlang)
        {
            List<Slang> _list = new List<Slang>();
            List<Slang> list = DAL.SqliteHelperMean.GetSlangs();
            if (list != null && list.Count > 0)
            {
                _list.AddRange(list);
                bsSlang.DataSource = _list;
                bsSlang.ResetBindings(false);
            }
        }


        public static void AddWord(string word, string meaning)
        {
            using (SQLiteConnection connection = new SQLiteConnection(@"DataSource=C:\\Users\sotni\\OneDrive\\Рабочий стол\\dataBase.db"))
            {
                connection.Open();
                string query = $"insert into slangs (word, meaning) values('{word}', '{meaning}')";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        internal static List<Slang> GetSlangs()
        {
           
                using (SQLiteConnection connection = new SQLiteConnection(@"DataSource=C:\\Users\sotni\\OneDrive\\Рабочий стол\\dataBase.db"))
                {
                    connection.Open();
                    string query = @"SELECT * from slangs";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            List<Slang> slangs = new List<Slang>();
                            //int i = 1;
                            while (reader.Read())
                            {

                                slangs.Add(new Slang
                                {
                                    Id = reader.GetInt32(0),
                                    Word = reader.GetString(1),
                                    Meaning = reader.GetString(2)

                                });
                                //i++;
                            }

                            return slangs;
                        }

                    }
                }

        }
    }
}
