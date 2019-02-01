﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Base.Model
{
    public class DocumentInstallmentModel
    {
        public int DocEntry { get; set; }
        public EnumObjType ObjType { get; set; }        
        public double InsTotal { get; set; }
        public double InstPrcnt { get; set; }
        public DateTime DueDate { get; set; }

        public Dictionary<string, dynamic> UserFields { get; set; }
    }
}
