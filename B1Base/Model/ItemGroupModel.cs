﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Base.Model
{
    public class ItemGroupModel
    {
        public int GroupCode { get; set; }
        public string GroupName { get; set; }
        public Dictionary<string, dynamic> UserFields { get; set; }

        public ItemGroupModel()
        {
            UserFields = new Dictionary<string, dynamic>();
        }
    }
}
