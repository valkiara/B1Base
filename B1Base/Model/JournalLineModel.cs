﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Base.Model
{
    class JournalLineModel
    {
        public string Account { get; set; }        
        public double Credit { get; set; }
        public double Debit { get; set; }

        public JournalLineModel()
        {
            Account = string.Empty;
            Credit = 0;
            Debit = 0;
        }
    }
}