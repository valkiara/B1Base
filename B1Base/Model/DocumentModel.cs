﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B1Base.Model
{
    public class DocumentModel
    {
        public int DocEntry { get; set; }
        public string CardCode { get; set; }
        public DateTime DocDate { get; set; }
        public int SlpCode { get; set; }        
        public EnumObjType ObjType { get; set; }

        public List<Model.DocumentItemModel> DocumentItemList { get; set; }
        
        public Dictionary<string, dynamic> UserFields { get; set; }

        public DocumentModel()
        {
            DocEntry = 0;
            CardCode = string.Empty;
            DocDate = DateTime.Now;
            SlpCode = 0;
            ObjType = EnumObjType.None;

            DocumentItemList = new List<DocumentItemModel>();           

            UserFields = new Dictionary<string, dynamic>();
        }
    }
}
