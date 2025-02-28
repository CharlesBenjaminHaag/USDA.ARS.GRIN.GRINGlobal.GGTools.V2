using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class SysDynamicQueryManager: GRINGlobalDataManagerBase
    {
        public DataTable Search(SysDynamicQuerySearch searchEntity)
        {
            DataTable dt = new DataTable();
            SQL = searchEntity.SQLStatement;
            using (IDataReader rdr = GetDataReader())
            {
                dt.Load(rdr);

                //for (int index = 0; index < rdr.FieldCount; index++)
                //{
                //    // Get name from data reader
                //    var name = rdr.GetName(index);

                //}
                //var resultList = new List<Dictionary<string, dynamic>>();
                //while (rdr.Read())
                //{
                //    var t = new Dictionary<string, dynamic>();
                //    for (var i = 0; i < rdr.FieldCount; i++)
                //    {
                //        t[rdr.GetName(i)] = rdr[i];
                //    }
                //    resultList.Add(t);
                //}
                rdr.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        string DEBUG = dr[i].ToString();
                    }
                }

            }
            return dt;
        }
        
    }
}
