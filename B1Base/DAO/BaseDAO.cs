﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SAPbobsCOM;

namespace B1Base.DAO
{
    public class BaseDAO<T> where T : Model.BaseModel
    {
        protected virtual string TableName
        {
            get
            {
                return this.GetType().Name.Replace("Model", "");
            }
        }

        public T Get(int code)
        {
            Type type = typeof(T);

            var props = type.GetProperties();

            T model = (T)Activator.CreateInstance(type);

            UserTable userTable = (UserTable)AddOn.Instance.ConnectionController.Company.UserTables.Item(TableName);
            try
            {
                if (userTable.GetByKey(code.ToString()))
                {
                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(Boolean))
                        {
                            prop.SetValue(model, userTable.UserFields.Fields.Item("U_" + prop.Name).Value.ToString().Equals("Y") ? true : false, null);
                        }
                        else if (prop.PropertyType.IsEnum)
                        {
                            prop.SetValue(model, Convert.ChangeType(userTable.UserFields.Fields.Item("U_" + prop.Name).Value, Enum.GetUnderlyingType(prop.PropertyType)), null);
                        }
                        else
                        {
                            prop.SetValue(model, userTable.UserFields.Fields.Item("U_" + prop.Name).Value);
                        }
                    }

                    return model as T;
                }
                else
                {
                    return (T)Activator.CreateInstance(type);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(userTable);
            }
            
        }

        public void Save(T model)
        {
            Type type = typeof(T);

            var props = type.GetProperties();

            UserTable userTable = (UserTable)AddOn.Instance.ConnectionController.Company.UserTables.Item(TableName);
            try
            {
                if (userTable.GetByKey(model.Code.ToString()))
                {
                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(Boolean))
                        {
                            userTable.UserFields.Fields.Item("U_" + prop.Name).Value = (bool)prop.GetValue(model) ? "Y" : "N";
                        }
                        else if (prop.PropertyType.IsEnum)
                        {
                            userTable.UserFields.Fields.Item("U_" + prop.Name).Value = (int)prop.GetValue(model);
                        }
                        else
                        {
                            userTable.UserFields.Fields.Item("U_" + prop.Name).Value = prop.GetValue(model);
                        }
                    }

                    userTable.Update();
                }
                else
                {
                    userTable.Code = model.Code.ToString();
                    userTable.Name = model.Code.ToString();

                    foreach (var prop in props)
                    {
                        if (prop.PropertyType == typeof(Boolean))
                        {
                            userTable.UserFields.Fields.Item("U_" + prop.Name).Value = (bool)prop.GetValue(model) ? "Y" : "N";
                        }
                        else
                        {
                            userTable.UserFields.Fields.Item("U_" + prop.Name).Value = prop.GetValue(model);
                        }
                    }

                    userTable.Add();
                }

                AddOn.Instance.ConnectionController.VerifyBussinesObjectSuccess();
            }
            finally
            {
                Marshal.ReleaseComObject(userTable);
            }
        }
    }
}
