using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HandBook.DAL
{
    public class Slang
    {
        [Browsable(false)]
        public int Id { get; internal set; }

        [DisplayName("Slang")]
        public string Word { get; internal set; }

        public string Meaning { get; internal set; }
    }
}
