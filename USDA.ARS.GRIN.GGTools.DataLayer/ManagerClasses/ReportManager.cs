using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class ReportManager: GRINGlobalDataManagerBase
    {
        public DataTable GetReport(string code)
        {
            DataTable dtReportData = new DataTable();

            switch (code)
            {
                case "WORORD":
                    SQL = "SELECT * FROM vw_GRINGlobal_Rpt_Web_Order_Request_Order_Request WHERE YEAR(OrderDate) = 2022";
                    break;
               
                case "NRRPOFF":
                    SQL = "SELECT TOP 50 * FROM vw_GRINGlobal_Rpt_NRR_Past_Offenders ORDER BY OrderDate DESC";
                    break;
                case "NRRPREV":
                    SQL = "SELECT * FROM vw_GRINGlobal_Rpt_NRR_Prevented ";
                    break;
                
            }

            using (IDataReader rdr = GetDataReader())
            {
                dtReportData.Load(rdr);

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

                foreach (DataRow dr in dtReportData.Rows)
                {
                    for (var i = 0; i < dtReportData.Columns.Count; i++)
                    {
                        string DEBUG = dr[i].ToString();
                    }
                }

            }
            return dtReportData;
        }
    }
}
