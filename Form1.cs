using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace HandBook
{
    public partial class Form1 : Form
    {
        public void ClearTrans()
        {
            textBoxWord.Text = "";
            textBoxTrans.Text = "";
        }

        public void ClearMean()
        {
            textBoxWord2.Text = "";
            textBoxMean.Text = "";
        }


        private List<DAL.Slang> _list2 = new List<DAL.Slang>();
        private List<DAL.WordFull> _list = new List<DAL.WordFull>();
        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = true;
            dataGridView2.AutoGenerateColumns = true;

            bsSlang.DataSource = _list2;
            List<DAL.Slang> list2 = DAL.SqliteHelperMean.GetSlangs();
            if (list2 != null && list2.Count > 0)
            {
                _list2.AddRange(list2);
                bsSlang.ResetBindings(false);
            }
            //--------------------------------------------------------------------------------

            bsWord.DataSource = _list;
            List<DAL.WordFull> list = DAL.SqliteHelperTrans.GetWords();
            if (list != null && list.Count > 0)
            {
                _list.AddRange(list);
                bsWord.ResetBindings(false);
            }
            //dataGridView1.Columns["Id"].Visible = false;
        }



        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (!textBox_search.Focused)
            {
                var wordFul = (DAL.WordFull)bsWord.Current;
                textBoxWord.Text = wordFul.Word;
                textBoxTrans.Text = wordFul.Translation;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DAL.SqliteHelperTrans.AddWord(textBoxWord.Text, textBoxTrans.Text);

            DAL.SqliteHelperTrans.Refresh(bsWord);
            ClearTrans();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var wordFul = (DAL.WordFull)bsWord.Current;
            int id = wordFul.Id;
            DAL.SqliteHelperTrans.DeleteWord(id);
            DAL.SqliteHelperTrans.Refresh(bsWord);
            ClearTrans();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            var wordFul = (DAL.WordFull)bsWord.Current;
            int id = wordFul.Id;
            string translation = textBoxTrans.Text;
            string word = textBoxWord.Text;
            DAL.SqliteHelperTrans.UpdateWord(id, word, translation);

            DAL.SqliteHelperTrans.Refresh(bsWord);
            ClearTrans();
        }

        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            DAL.SqliteHelperTrans.Search(dataGridView1, bsWord, textBox_search.Text);
        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (!textBox_search2.Focused)
            {
                var wordSlang = (DAL.Slang)bsSlang.Current;
                textBoxWord2.Text = wordSlang.Word;
                textBoxMean.Text = wordSlang.Meaning;
            }
        }

        private void btnAdd2_Click(object sender, EventArgs e)
        {
            DAL.SqliteHelperMean.AddWord(textBoxWord2.Text, textBoxMean.Text);

            DAL.SqliteHelperMean.Refresh(bsSlang);
            ClearMean();
        }

        private void textBox_search2_TextChanged(object sender, EventArgs e)
        {
            DAL.SqliteHelperMean.Search(dataGridView2, bsSlang, textBox_search2.Text);
        }

        private void btnDelete2_Click(object sender, EventArgs e)
        {
            var wordSlang = (DAL.Slang)bsSlang.Current;
            int id = wordSlang.Id;
            DAL.SqliteHelperMean.DeleteWord(id);
            DAL.SqliteHelperMean.Refresh(bsSlang);
            ClearMean();
        }

        private void btnChange2_Click(object sender, EventArgs e)
        {
            var wordSlang = (DAL.Slang)bsSlang.Current;
            int id = wordSlang.Id;
            string word = textBoxWord2.Text;
            string meaning = textBoxMean.Text;

            DAL.SqliteHelperMean.UpdateWord(id, word, meaning);

            DAL.SqliteHelperMean.Refresh(bsSlang);
            ClearMean();

        }
       
        List<DAL.WordFull> revenge = DAL.SqliteHelperTrans.GetWords();
        public List<DAL.WordFull> ChangeRevenge(List<DAL.WordFull> revenge)
        {
            revenge.Reverse();
            return revenge;
        }
        private void pictureBoxVoice_Click(object sender, EventArgs e)
        {
            try
            {
                string word = textBoxWord.Text;
                WebClient client = new WebClient();
                var str = client.DownloadString($"https://api.dictionaryapi.dev/api/v2/entries/en/{word}");

                var englishResponse = JsonConvert.DeserializeObject<Root[]>(str);



                WindowsMediaPlayer player = new WindowsMediaPlayer();
                if (englishResponse[0].phonetics[0].audio != "")
                {
                    player.URL = $"{englishResponse[0].phonetics[0].audio}";
                    player.controls.play();
                }
                else
                {
                    player.URL = $"{englishResponse[0].phonetics[1].audio}";
                    player.controls.play();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Голос этого слова не найден", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           

            
        }
        public string changeText(string word, List<DAL.WordFull> list)
        {
            foreach (var item in list)
            {
                string value = item.Word.Trim(' ');
                if (word == value)
                {
                    string result = item.Translation.Trim(' ');
                    return result;
                }
              
            }
            return "";
        }

        private void textBoxWord_TextChanged(object sender, EventArgs e)
        {
            string word = textBoxWord.Text;
            List<DAL.WordFull> list = DAL.SqliteHelperTrans.GetWords2();

            string translation = changeText(word, list);
            textBoxTrans.Text = translation;
        }
    }
}
