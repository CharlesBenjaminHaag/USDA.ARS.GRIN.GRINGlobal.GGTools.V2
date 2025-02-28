using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer
{
    public class ReferenceManager : GRINGlobalDataManagerBase
    {
        public List<CodeValue> SearchNotes(ReferenceSearch searchEntity)
        {
            // Create SQL to search for rows
            SQL = "SELECT Value, Description FROM vw_GRINGlobal_Taxonomy_Note ";
            SQL += " WHERE (@Note      IS NULL      OR Description     LIKE     '%' + @Note + '%') ";
            SQL += " AND   (Value      =            @TableName) ";

            var parameters = new List<IDbDataParameter> {
                CreateParameter("TableName", (object)searchEntity.TableName ?? DBNull.Value, true),
                CreateParameter("Note", (object)searchEntity.SearchText ?? DBNull.Value, true),
            };
            List<CodeValue> codeValues = GetRecords<CodeValue>(SQL, parameters.ToArray());
            RowsAffected = codeValues.Count;
            return codeValues;
        }
    }
}
