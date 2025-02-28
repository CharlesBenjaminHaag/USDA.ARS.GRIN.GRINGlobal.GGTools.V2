using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using USDA.ARS.GRIN.Common.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.DataLayer
{
    public class WebOrderRequestManager : GRINGlobalDataManagerBase, IManager<WebOrderRequest, WebOrderRequestSearch>
    {
        public void BuildInsertUpdateParameters()
        {
            throw new NotImplementedException();
        }

        public int Delete(WebOrderRequest entity)
        {
            throw new NotImplementedException();
        }

        public WebOrderRequest Get(int entityId)
        {
            SQL = "usp_GRINGlobal_Web_Order_Request_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("web_order_request_id", (object)entityId, false)
            };
            WebOrderRequest webOrderRequest = GetRecord<WebOrderRequest>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return webOrderRequest;
        }
        
        public int Insert(WebOrderRequest entity)
        {
            throw new NotImplementedException();
        }
        
        public int InsertWebOrderRequestAction(WebOrderRequestAction entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<WebOrderRequestAction>(entity);

            SQL = "usp_GRINGlobal_Web_Order_Request_Action_Insert";

            AddParameter("web_order_request_id", entity.WebOrderRequestID == 0 ? DBNull.Value : (object)entity.WebOrderRequestID, true);
            AddParameter("action_code", String.IsNullOrEmpty(entity.ActionCode) ? DBNull.Value : (object)entity.ActionCode, true);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);
            AddParameter("created_by", (object)entity.CreatedByWebUserID ?? DBNull.Value, true);

            AddParameter("@out_web_order_request_action_id", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            entity.ID = GetParameterValue<int>("@out_web_order_request_action_id", -1);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }
        
        public List<WebOrderRequest> Search(WebOrderRequestSearch searchEntity)
        {
            List<WebOrderRequest> results = new List<WebOrderRequest>();

            SQL = " SELECT * FROM vw_GRINGlobal_Web_Order_Request";
            SQL += " WHERE (@FirstName              IS NULL     OR      WebCooperatorFirstName          LIKE        '%' + @FirstName + '%')";
            SQL += " AND (@ID                       IS NULL     OR      ID                              =           @ID)";
            SQL += " AND (@OwnedByWebUserID         IS NULL     OR      OwnedByWebUserID                =           @OwnedByWebUserID)";
            SQL += " AND (@LastName                 IS NULL     OR      WebCooperatorLastName           LIKE        '%' + @LastName + '%')";
            SQL += " AND (@EmailAddress             IS NULL     OR      WebCooperatorEmail              LIKE        '%' + @EmailAddress + '%')";
            SQL += " AND (@Organization             IS NULL     OR      WebCooperatorOrganization       LIKE        '%' + @Organization + '%')";
            SQL += " AND (@WebCooperatorAddressCountryDescription       IS NULL                         OR          WebCooperatorAddressCountryDescription     =   @WebCooperatorAddressCountryDescription )";
            SQL += " AND (@IntendedUseCode          IS NULL     OR      IntendedUseCode                 =           @IntendedUseCode)";
            SQL += " AND (@StatusCode               IS NULL     OR      StatusCode                      =           @StatusCode)";
            SQL += " AND (@MostRecentWebOrderAction IS NULL     OR      MostRecentWebOrderAction        =           @MostRecentWebOrderAction)";
            SQL += " AND (@Year                     IS NULL     OR      YEAR(CreatedDate)               =           @Year)";

            if (searchEntity.IsLocked == "Y")
            {
                if (SQL.Contains("WHERE"))
                {
                    SQL += " AND ";
                }
                else
                {
                    SQL += " WHERE ";
                }
                SQL += "  (@IsLocked IS NULL OR IsLocked = 1)";
            }

            if (!String.IsNullOrEmpty(searchEntity.TimeFrame))
            {
                if (SQL.Contains("WHERE"))
                {
                    SQL += " AND ";
                }
                else
                {
                    SQL += " WHERE ";
                }
                switch (searchEntity.TimeFrame)
            {
                case "1D":
                    SQL += "  (CONVERT(date, OrderDate) = CONVERT(date, GETDATE()))";
                    break;
                case "3D":
                    SQL += "  OrderDate >= DATEADD(day,-3, GETDATE())";
                    break;
                case "7D":
                    SQL += "  OrderDate >= DATEADD(day,-7, GETDATE())";
                    break;
                case "30D":
                    SQL += "  OrderDate >= DATEADD(day,-30, GETDATE())";
                    break;
                case "60D":
                    SQL += "  OrderDate >= DATEADD(day,-60, GETDATE())";
                    break;
                case "90D":
                    SQL += "  OrderDate >= DATEADD(day,-90, GETDATE())";
                    break;
                case "180D":
                    SQL += "  OrderDate >= DATEADD(day,-180, GETDATE())";
                    break;
                    case "1Y":
                    SQL += "  DATEPART(year, OrderDate) = DATEPART(year, GETDATE())";
                    break;
            }
            }

            if (!String.IsNullOrEmpty(searchEntity.StatusList))
            {
                searchEntity.StatusList = String.Join(",", Array.ConvertAll(searchEntity.StatusList.Split(','), z => "'" + z + "'"));

                if (SQL.Contains("WHERE"))
                {
                    SQL += " AND ";
                }
                else
                {
                    SQL += " WHERE ";
                }

                SQL += "  StatusCode IN (" + searchEntity.StatusList + ")";
            }

            if (!String.IsNullOrEmpty(searchEntity.MostRecentActionList))
            {
                searchEntity.MostRecentActionList = String.Join(",", Array.ConvertAll(searchEntity.MostRecentActionList.Split(','), z => "'" + z + "'"));

                if (SQL.Contains("WHERE"))
                {
                    SQL += " AND ";
                }
                else
                {
                    SQL += " WHERE ";
                }

                SQL += "  MostRecentWebOrderAction IN (" + searchEntity.MostRecentActionList + ")";
            }

            //if (!String.IsNullOrEmpty(searchEntity.WebUserList))
            //{
            //    searchEntity.WebUserList = String.Join(",", Array.ConvertAll(searchEntity.WebUserList.Split(','), z + "'"));
            //    SQL += " AND IntendedUseCode IN (" + searchEntity.IntendedUseList + ")";
            //}

            SQL += " ORDER BY OrderDate DESC";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("ID", searchEntity.ID > 0 ? (object)searchEntity.ID : DBNull.Value, true),
                CreateParameter("OwnedByWebUserID", searchEntity.OwnedByWebUserID > 0 ? (object)searchEntity.OwnedByWebUserID : DBNull.Value, true),
                CreateParameter("FirstName", (object)searchEntity.WebCooperatorFirstName ?? DBNull.Value, true),
                CreateParameter("LastName", (object)searchEntity.WebCooperatorLastName ?? DBNull.Value, true),
                CreateParameter("Organization", (object)searchEntity.WebCooperatorOrganization ?? DBNull.Value, true),
                CreateParameter("WebCooperatorAddressCountryDescription", (object)searchEntity.WebCooperatorAddressCountryDescription ?? DBNull.Value, true),
                CreateParameter("EmailAddress", (object)searchEntity.WebCooperatorEmailAddress ?? DBNull.Value, true),
                CreateParameter("IntendedUseCode", (object)searchEntity.IntendedUseCode ?? DBNull.Value, true),
                CreateParameter("StatusCode", (object)searchEntity.StatusCode ?? DBNull.Value, true),
                CreateParameter("MostRecentWebOrderAction", (object)searchEntity.MostRecentAction ?? DBNull.Value, true),
                CreateParameter("Year", searchEntity.Year > 0 ? (object)searchEntity.Year : DBNull.Value, true),
                CreateParameter("IsLocked", (object)searchEntity.IsLocked ?? DBNull.Value, true),
            };

            results = GetRecords<WebOrderRequest>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public List<WebOrderRequestItem> SearchWebOrderRequestItems(WebOrderRequestSearch searchEntity)
        {
            List<WebOrderRequestItem> results = new List<WebOrderRequestItem>();

            SQL = " SELECT * FROM vw_GRINGlobal_Web_Order_Request_Item ";
            SQL += " WHERE WebOrderRequestID IN (" + searchEntity.IDList + ")";
            
            var parameters = new List<IDbDataParameter> {
           };

            results = GetRecords<WebOrderRequestItem>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public List<WebOrderRequestAction> SearchWebOrderRequestActions(WebOrderRequestSearch searchEntity)
        {
            List<WebOrderRequestAction> results = new List<WebOrderRequestAction>();

            SQL = " SELECT * FROM vw_GRINGlobal_Web_Order_Request_Action ";
            SQL += " WHERE WebOrderRequestID IN (" + searchEntity.IDList + ")";

            var parameters = new List<IDbDataParameter>
            {
            };

            results = GetRecords<WebOrderRequestAction>(SQL, parameters.ToArray());
            RowsAffected = results.Count;
            return results;
        }

        public List<WebOrderRequestItem> GetWebOrderRequestItems(int entityId)
        {
            SQL = "usp_GRINGlobal_Web_Order_Request_Items_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("web_order_request_id", (object)entityId, false)
            };
            List<WebOrderRequestItem> webOrderRequestItems = GetRecords<WebOrderRequestItem>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return webOrderRequestItems;
        }
        
        public List<WebOrderRequestAction> GetWebOrderRequestActions(int? entityId)
        {
            SQL = "usp_GRINGlobal_Web_Order_Request_Actions_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("web_order_request_id", (object)entityId, false)
            };
            List<WebOrderRequestAction> webOrderRequestActions = GetRecords<WebOrderRequestAction>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return webOrderRequestActions;
        }
        
        public CodeValue GetWebOrderRequestEmailAddresses(int entityId)
        {
            SQL = "usp_GRINGlobal_Web_Order_Request_EmailAddresses_Select";
            var parameters = new List<IDbDataParameter> {
                CreateParameter("web_order_request_id", (object)entityId, false)
            };
            //AddParameter("out_email_recipients", -1, true, System.Data.DbType.String, System.Data.ParameterDirection.Output);
            CodeValue siteEmailDetail = GetRecord<CodeValue>(SQL, CommandType.StoredProcedure, parameters.ToArray());
            return siteEmailDetail;
        }
        
        public int Update(WebOrderRequest entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<WebOrderRequest>(entity);

            SQL = "usp_GRINGlobal_Web_Order_Request_Update";

            BuildInsertUpdateParameters(entity);
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);
            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }

            RowsAffected = ExecuteNonQuery();
            return RowsAffected;
        }

        public int UpdateLock(WebOrderRequest entity)
        {
            Reset(CommandType.StoredProcedure);
            Validate<WebOrderRequest>(entity);

            SQL = "usp_GRINGlobal_Web_Order_Request_Lock_Update";

            AddParameter("web_order_request_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            AddParameter("web_user_id", (object)entity.WebUserID, true);
            AddParameter("is_locked", (object)entity.IsLocked, true);
            
            AddParameter("@out_error_number", -1, true, System.Data.DbType.Int32, System.Data.ParameterDirection.Output);

            RowsAffected = ExecuteNonQuery();

            int errorNumber = GetParameterValue<int>("@out_error_number", -1);

            if (errorNumber > 0)
            {
                throw new Exception(errorNumber.ToString());
            }
            return RowsAffected;
        }
        
        public void BuildInsertUpdateParameters(WebOrderRequest entity)
        {
            if (entity.ID > 0)
            {
                AddParameter("web_order_request_id", entity.ID == 0 ? DBNull.Value : (object)entity.ID, true);
            }
            AddParameter("web_user_id", entity.WebUserID == 0 ? DBNull.Value : (object)entity.WebUserID, true);
            AddParameter("status_code", String.IsNullOrEmpty(entity.StatusCode) ? DBNull.Value : (object)entity.StatusCode, true);
            AddParameter("note", (object)entity.Note ?? DBNull.Value, true);
        }
        
        public List<CodeValue> GetTimeFrameOptions()
        {
            List<CodeValue> timeFrameOptions = new List<CodeValue>();
            timeFrameOptions.Add(new CodeValue { Value = "1D", Title = "Today" });
            timeFrameOptions.Add(new CodeValue { Value = "3D", Title = "The last 3 days" });
            timeFrameOptions.Add(new CodeValue { Value = "7D", Title = "This Week" });
            timeFrameOptions.Add(new CodeValue { Value = "30D", Title = "This Month" });
            timeFrameOptions.Add(new CodeValue { Value = "60D", Title = "The Last 2 Months" });
            timeFrameOptions.Add(new CodeValue { Value = "90D", Title = "The Last 3 Months" });
            return timeFrameOptions;
        }

        public List<WebCooperator> GetWebCooperators()
        {
            SQL = "usp_GRINGlobal_Web_Cooperators_Select";
            List<WebCooperator> webCooperators = GetRecords<WebCooperator>(SQL, CommandType.StoredProcedure);
            RowsAffected = webCooperators.Count;
            return webCooperators;
        }

        public List<CodeValue> GetWebOrderRequestStatuses()
        {
            List<CodeValue> codeValues = null;

            SQL = "SELECT cv.Code, cv.CodeTitle FROM vw_GRINGlobal_Code_Value cv WHERE cv.GroupName = 'WEB_ORDER_REQUEST_STATUS'";
            var parameters = new List<IDbDataParameter> {
            };
            codeValues = GetRecords<CodeValue>(SQL, CommandType.Text, parameters.ToArray());
            return codeValues;
        }

        public List<CodeValue> GetWebOrderRequestActions()
        {
            List<CodeValue> codeValues = null;

            SQL = "SELECT cv.Code, cv.CodeTitle FROM vw_GRINGlobal_Code_Value cv WHERE cv.GroupName = 'WEB_ORDER_REQUEST_ACTION' AND cv.Code NOT LIKE '%FLAG%'";
            var parameters = new List<IDbDataParameter>
            {
            };
            codeValues = GetRecords<CodeValue>(SQL, CommandType.Text, parameters.ToArray());
            return codeValues;
        }
    }
}
