﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Data;
using SAPbobsCOM;

namespace B1Base.DAO
{
    public class BaseDAO<T> where T : Model.BaseModel
    {
        
        protected virtual string TableName
        {
            get
            {
                return this.GetType().Name.Replace("DAO", "").ToUpper();
            }
        }

        public T Get(int code)
        {
            Type type = typeof(T);

            var props = type.GetProperties().Where(r => r.Name != "Changed");

            T model = (T)Activator.CreateInstance(type);

            UserTable userTable = (UserTable)Controller.ConnectionController.Instance.Company.UserTables.Item(TableName);
            try
            {
                if (userTable.GetByKey(code.ToString()))
                {
                    foreach (var prop in props)
                    {
                        Model.BaseModel.NonDB nonDB = prop.GetCustomAttribute(typeof(Model.BaseModel.NonDB)) as Model.BaseModel.NonDB;

                        if (nonDB == null)
                        {
                            if (prop.PropertyType == typeof(Boolean))
                            {
                                prop.SetValue(model, userTable.UserFields.Fields.Item("U_" + prop.Name).Value.ToString().Equals("Y"), null);
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
                    }

                    return model as T;
                }
                else
                {
                    foreach (var prop in props)
                    {
                        Model.BaseModel.NonDB nonDB = prop.GetCustomAttribute(typeof(Model.BaseModel.NonDB)) as Model.BaseModel.NonDB;

                        if (nonDB == null)
                        {
                            if (prop.PropertyType == typeof(Boolean))
                            {
                                prop.SetValue(model, userTable.UserFields.Fields.Item("U_" + prop.Name).Value.ToString().Equals("Y"), null);
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
                    }

                    return model as T;
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

            var props = type.GetProperties().Where(r => r.Name != "Changed" && r.Name != "Code");

            UserTable userTable = (UserTable)Controller.ConnectionController.Instance.Company.UserTables.Item(TableName);
            try
            {
                if (userTable.GetByKey(model.Code.ToString()))
                {
                    foreach (var prop in props)
                    {
                        Model.BaseModel.NonDB nonDB = prop.GetCustomAttribute(typeof(Model.BaseModel.NonDB)) as Model.BaseModel.NonDB;

                        if (nonDB == null)
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
                    }

                    userTable.Update();
                }
                else
                {                    
                    foreach (var prop in props)
                    {
                        Model.BaseModel.NonDB nonDB = prop.GetCustomAttribute(typeof(Model.BaseModel.NonDB)) as Model.BaseModel.NonDB;

                        if (nonDB == null)
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
                    }

                    if (model.Code == 0)
                    {
                        Type seqDAOType = Type.GetType(type.AssemblyQualifiedName.Replace("Model", "DAO").Replace(type.Name.Replace("Model", "DAO"), "ConfigSeqDAO"));

                        if (seqDAOType == null)
                            model.Code = Controller.ConnectionController.Instance.ExecuteSqlForObject<int>("GetLastCode", TableName, ConfigSeqDAO.AddOnSequenceTableName);
                        else
                        {
                            var dao = (DAO.ConfigSeqDAO)Activator.CreateInstance(seqDAOType);

                            model.Code = Controller.ConnectionController.Instance.ExecuteSqlForObject<int>("GetLastCode", TableName, dao.TableName);
                        }

                    }

                    userTable.UserFields.Fields.Item("U_Code").Value = model.Code;
                    userTable.Code = model.Code.ToString();
                    userTable.Name = model.Code.ToString();

                    userTable.Add();
                }

                Controller.ConnectionController.Instance.VerifyBussinesObjectSuccess();
            }
            finally
            {
                Marshal.ReleaseComObject(userTable);
                GC.Collect();
            }
        }

        public void Delete(T model)
        {
            Type type = typeof(T);

            UserTable userTable = (UserTable)Controller.ConnectionController.Instance.Company.UserTables.Item(TableName);
            try
            {
                if (userTable.GetByKey(model.Code.ToString()))
                {
                    userTable.Remove();
                }

                Controller.ConnectionController.Instance.VerifyBussinesObjectSuccess();
            }
            finally
            {
                Marshal.ReleaseComObject(userTable);
                GC.Collect();
            }
        }

        public void UpdateField(int code, string field, object value)
        {
            UserTable userTable = (UserTable)Controller.ConnectionController.Instance.Company.UserTables.Item(TableName);
            try
            {
                if (userTable.GetByKey(code.ToString()))
                {
                    userTable.UserFields.Fields.Item("U_" + field).Value = value;

                    userTable.Update();

                    Controller.ConnectionController.Instance.VerifyBussinesObjectSuccess();
                }
            }
            finally
            {
                Marshal.ReleaseComObject(userTable);
                GC.Collect();
            }            
        }
    }
}
