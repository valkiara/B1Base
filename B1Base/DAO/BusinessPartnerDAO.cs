﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SAPbobsCOM;

namespace B1Base.DAO
{
    public class BusinessPartnerDAO
    {
        public void Save(Model.BusinessPartnerModel businessPartnerModel)
        {
            BusinessPartners businessPartner = AddOn.Instance.ConnectionController.Company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
            try
            {
                if (businessPartner.GetByKey(businessPartnerModel.CardCode))
                {
                    if (businessPartnerModel.SlpCode == 0)
                        businessPartner.SalesPersonCode = -1;
                    else
                        businessPartner.SalesPersonCode = businessPartnerModel.SlpCode;                    

                    businessPartner.Update();

                    AddOn.Instance.ConnectionController.VerifyBussinesObjectSuccess();

                    AddOn.Instance.ConnectionController.ExecuteStatement("UpdateBusinessPartnerAgentCode", businessPartnerModel.AgentCode, businessPartnerModel.CardCode);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(businessPartner);
                GC.Collect();
            }
        }
    }
}
