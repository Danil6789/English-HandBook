﻿using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace HandBook.DAL
{
    public class WordFull
    {
        [Browsable(false)]
        public int Id { get; internal set; }
        
        public string Word { get; internal set; }

        public string Translation { get; internal set; }
    }
}