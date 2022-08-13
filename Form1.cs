using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandBook
{
    public partial class Form1 : Form
    {
        public void Clear()
        {
            textBoxWord.Text = "";
            textBoxTrans.Text = "";
        }

        private List<DAL.WordFull> _list = new List<DAL.WordFull>();
        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = true;
            

            bsWord.DataSource = _list;
            List<DAL.WordFull> list = DAL.SqliteHelper.GetWords();
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
            DAL.SqliteHelper.AddWord(textBoxWord.Text, textBoxTrans.Text);

            DAL.SqliteHelper.Refresh(bsWord);
            Clear();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var wordFul = (DAL.WordFull)bsWord.Current;
            int id = wordFul.Id;
            DAL.SqliteHelper.DeleteWord(id);
            DAL.SqliteHelper.Refresh(bsWord);
            Clear();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            var wordFul = (DAL.WordFull)bsWord.Current;
            int id = wordFul.Id;
            string translation = textBoxTrans.Text;
            string word = textBoxWord.Text;
            DAL.SqliteHelper.UpdateWord(id, word, translation);
           
            DAL.SqliteHelper.Refresh(bsWord);
            Clear();
        }
     
        private void textBox_search_TextChanged(object sender, EventArgs e)
        {
            DAL.SqliteHelper.Search(dataGridView1, bsWord, textBox_search.Text);
        }
    }
}
