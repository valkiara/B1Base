﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using SAPbobsCOM;

namespace B1Base.DAO
{
    class AttachmentDAO
    {
        public Model.AttachmentModel Get(int atcEntry, int line)
        {
            Model.AttachmentModel result = new Model.AttachmentModel();

             Attachments2 attachment = (Attachments2)AddOn.Instance.ConnectionController.Company.GetBusinessObject(BoObjectTypes.oAttachments2);
             try
             {
                 if (attachment.GetByKey(atcEntry))
                 {
                     attachment.Lines.SetCurrentLine(line - 1);

                     result.AbsEntry = attachment.AbsoluteEntry;
                     result.Line = line;
                     result.Path = Path.ChangeExtension(Path.Combine(GetAttachmentFolder(), attachment.Lines.FileName), attachment.Lines.FileExtension);
                 }
             }
             finally
             {
                 Marshal.ReleaseComObject(attachment);
             }

            return result;
        }

        public void Insert(Model.AttachmentModel attachmentModel)
        {
            Attachments2 attachment = (Attachments2)AddOn.Instance.ConnectionController.Company.GetBusinessObject(BoObjectTypes.oAttachments2);
            try
            {
                if (attachment.GetByKey(attachmentModel.AbsEntry))
                {
                    attachment.Lines.Add();

                    attachment.Lines.SetCurrentLine(attachment.Lines.Count - 1);

                    attachment.Lines.SourcePath = Path.GetDirectoryName(attachmentModel.Path);
                    attachment.Lines.FileName = Path.GetFileNameWithoutExtension(attachmentModel.Path);
                    attachment.Lines.FileExtension = Path.GetExtension(attachmentModel.Path).Replace(".", "");
                    attachment.Lines.Override = BoYesNoEnum.tYES;

                    attachment.Update();

                    AddOn.Instance.ConnectionController.VerifyBussinesObjectSuccess();
                }
                else
                {
                    attachment.Lines.SourcePath = Path.GetDirectoryName(attachmentModel.Path);
                    attachment.Lines.FileName = Path.GetFileNameWithoutExtension(attachmentModel.Path);
                    attachment.Lines.FileExtension = Path.GetExtension(attachmentModel.Path).Replace(".", "");
                    attachment.Lines.Override = BoYesNoEnum.tYES;

                    attachment.Add();

                    AddOn.Instance.ConnectionController.VerifyBussinesObjectSuccess();

                    attachmentModel.AbsEntry = Convert.ToInt32(AddOn.Instance.ConnectionController.Company.GetNewObjectKey());
                }                
            }
            finally
            {
                Marshal.ReleaseComObject(attachment);
            }
        }

        public void Delete(Model.AttachmentModel attachmentModel)
        {
            Attachments2 attachment = (Attachments2)AddOn.Instance.ConnectionController.Company.GetBusinessObject(BoObjectTypes.oAttachments2);
            try
            {
                if (attachment.GetByKey(attachmentModel.AbsEntry))
                {
                    attachment.Lines.SetCurrentLine(attachmentModel.Line - 1);

                    attachment.Lines.SourcePath = string.Empty;
                    attachment.Lines.FileName = string.Empty;
                    attachment.Lines.FileExtension = string.Empty;

                    attachment.Update();

                    AddOn.Instance.ConnectionController.VerifyBussinesObjectSuccess();
                }               
            }
            finally
            {
                Marshal.ReleaseComObject(attachment);
            }
        }

        public string GetAttachmentFolder()
        {
            return AddOn.Instance.ConnectionController.ExecuteSqlForObject<string>("GetAttachmentFolder");
        }

        public string GetImageFolder()
        {
            return AddOn.Instance.ConnectionController.ExecuteSqlForObject<string>("GetImageFolder");            
        }
    }
}