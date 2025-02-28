using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;
using System.Diagnostics;
using System.Data;
using System.Text.RegularExpressions;
using GrinGlobal.Core;
using System.Security.Authentication;
using System.IO;
using System.Security.Cryptography;
using GrinGlobal.Business.SearchSvc;
using System.Timers;
using System.Web;
using System.Drawing;
using System.Net;
using System.Drawing.Imaging;

using GrinGlobal.Business.DataTriggers;
using GrinGlobal.Interface.DataTriggers;
using GrinGlobal.Interface.Dataviews;
using System.Threading;
using System.Runtime.InteropServices;
using System.DirectoryServices;

using GrinGlobal.DatabaseInspector;
using System.Xml;
using GrinGlobal.Interface;
using System.Data.SqlClient;
using System.ComponentModel;

namespace GrinGlobal.Business {


	internal delegate Permission[] PermissionCallback();

	public partial class SecureData : IDisposable {


		// This is the method template you MUST use when creating a new method in this class.
		// It handles all initialization and tear down properly to respect the suppressExceptions 
		// flag the caller specified.  Also guarantees return value is in correct format.
		protected DataSet __MethodTemplate__() {
            DataSet dsReturn = createReturnDataSet();
			try {

				using (DataManager dm = BeginProcessing(true)) {
					// put code specific to the method here

				}

			} catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
					throw;
				}
			} finally {
				FinishProcessing(dsReturn, _languageID);
			}
			return dsReturn;
		}

        /// <summary>
        /// Signifies the action the trigger is currently handling
        /// </summary>
        protected enum TriggerMode {
            Saving,
            RowSaving,
            RowSaved,
            RowSaveFailed,
            Saved,
        }


		#region Exception


		protected Exception _lastException;
        /// <summary>
        /// Gets the last exception that occurred.  Set by the AddExceptionToTable method.
        /// </summary>
		public Exception LastException {
			get { return _lastException; }
		}
		
        /// <summary>
        /// Creates the skeleton DataSet that is returned by almost all methods in SecureData.
        /// </summary>
        /// <returns></returns>
		[DebuggerStepThrough()]
		protected DataSet createReturnDataSet() {
			return createReturnDataSet(null);
		}

        /// <summary>
        /// Creates the skeleton DataSet and populates the ExceptionTable with one record representing the given Exception.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
		protected DataSet createReturnDataSet(Exception ex) {
			DataSet ds = new DataSet("SecureDataDataSet");

            // see http://www.eggheadcafe.com/articles/20030205.asp for info as to why the following could make things faster...
            // ds.EnforceConstraints = false;

			// add a table for holding exceptions
			DataTable dtException = new DataTable("ExceptionTable");
			dtException.Columns.Add("ExceptionIndex", typeof(int));
			dtException.Columns.Add("ExceptionType", typeof(string));
			dtException.Columns.Add("Data", typeof(string));
			dtException.Columns.Add("Message", typeof(string));
			dtException.Columns.Add("Source", typeof(string));
			dtException.Columns.Add("StackTrace", typeof(string));
			dtException.Columns.Add("InnerException", typeof(string));
			ds.Tables.Add(dtException);
			if (ex != null) {
				AddExceptionToTable(ex, false, ds);
			}

			return ds;
		}

        /// <summary>
        /// Creates the skeleton DataSet that is returned by almost all methods in SecureData.
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough()]
        public DataSet CreateReturnDataSet()
        {
            return createReturnDataSet(null);
        }

        /// <summary>
        /// Adds a row to the ExceptionTable DataTable in the given dsReturn DataSet.  Also sets the LastException variable.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="markAsHandled"></param>
        /// <param name="dsReturn"></param>
        /// <returns></returns>
		public int AddExceptionToTable(Exception ex, bool markAsHandled, DataSet dsReturn) {

			_lastException = ex;

            DataTable dtException = GetExceptionTable(dsReturn);

			DataRow dr = dtException.NewRow();
            int index = dtException.Rows.Count + 1;
			dr["ExceptionIndex"] = index;
			dr["ExceptionType"] = ex.GetType().ToString();
			dr["Data"] = Library.DataToString(ex.Data);
			dr["Message"] = ex.Message;
			dr["Source"] = ex.Source;
			dr["StackTrace"] = ex.StackTrace;
			if (ex.InnerException != null) {
				dr["InnerException"] = ex.InnerException.ToString();
			}
            dtException.Rows.Add(dr);

            if (markAsHandled) {
                markExceptionAsHandled(ex);
            }

			return index;
		}

        /// <summary>
        /// Adds a flag to the given Exception object so AddExceptionToTable can be called multiple times for the given exception as it bubbles out of a call stack, yet only be logged in the table one time.
        /// </summary>
        /// <param name="ex"></param>
        protected void markExceptionAsHandled(Exception ex) {
            // tell the exception we already logged it (to prevent duplicate exception reports)
            if (!ex.Data.Contains("SecureDataExceptionHandled")) {
                ex.Data.Add("SecureDataExceptionHandled", true);
            }
        }

        /// <summary>
        /// Returns true if the given exception has the flag set by markExceptionAsHandled().
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected bool alreadyLoggedException(Exception ex) {
			return ex.Data.Contains("SecureDataExceptionHandled");
        }

        /// <summary>
        /// Logs the exception if needed and returns true if the exception should be thrown
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        protected bool LogException(Exception ex, DataSet dsReturn) {
            return LogException(ex, dsReturn, true);
        }

        /// <summary>
        /// Logs the exception if needed and returns true if the exception should be thrown
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="dsReturn"></param>
        /// <param name="writeToEventLog"></param>
        /// <returns></returns>
		protected bool LogException(Exception ex, DataSet dsReturn, bool writeToEventLog) {
            if (alreadyLoggedException(ex)){
				// this has been handled once, which means we should neither
				// log it nor research into which method threw it.
				return !_suppressExceptions;
			}

            if (dsReturn != null) {
                AddExceptionToTable(ex, false, dsReturn);
            }


			// log where exception was coming from.  Helloooooo stack frames.
			StackTrace st = new StackTrace(ex);
			StackFrame[] frames = st.GetFrames();
			int i = frames.Length - 1;

			MethodInfo mi = null;
			bool found = false;
			while (!found && i > -1) {
				mi = (MethodInfo)frames[i--].GetMethod();
				switch (mi.Name.ToUpper()) {
					case "INIT":
					case "HANDLEEXCEPTION":
					case "PROCESS":
						break;
					default:
						if (mi.Name.StartsWith("<")) {
							// probably an anonymous method. ignore it and continue
						} else {
							found = true;
						}
						break;
				}
			}
			StringBuilder sb = new StringBuilder();
			if (mi == null) {
				sb.Append("(unknown method, stackframe lookup yielded no match)");
			} else {
				sb.Append(mi.ReturnParameter.ParameterType.Name + " ");
				sb.Append(mi.ReflectedType.FullName + "." + mi.Name + "(");

				ParameterInfo[] pis = mi.GetParameters();
				foreach (ParameterInfo pi in pis) {
					sb.Append(pi.ParameterType.Name + " " + pi.Name + ", ");
				}
				sb.Remove(sb.Length - 2, 2);
				sb.Append(")");
			}

			string fullMethodSignature = sb.ToString();

			//MethodBody mb = mi.GetMethodBody();
			//foreach (LocalVariableInfo lvi in mb.LocalVariables) {
			//    if (lvi.LocalType is Exception) {
			//        Debug.WriteLine(lvi.LocalType.FullName);
			//        frames[i].
			//    }
			//}
			Logger.LogText(fullMethodSignature, ex, true, writeToEventLog, true);

            markExceptionAsHandled(ex);


			return !_suppressExceptions;

		}

		#endregion Exception



        #region Properties

        private DataConnectionSpec _dataConnectionSpec;
        /// <summary>
        /// Gets the database connection information this instance of SecureData is using.
        /// </summary>
        public DataConnectionSpec DataConnectionSpec {
            get {
                return _dataConnectionSpec;
            }
        }

        private bool _suppressExceptions;
        /// <summary>
        /// Gets or sets the flag denoting how to handle exceptions should they occur during normal processing.  Note this only includes exceptions that occur within this class or other classes it calls -- i.e. IIS errors such as 404, timeouts, bad configuration files, etc can still occur and still cause exceptions to be thrown.
        /// </summary>
		public bool SuppressExceptions {
			[DebuggerStepThrough()]
			get { return _suppressExceptions; }
			[DebuggerStepThrough()]
			set { _suppressExceptions = value; }
		}

        //private DataSet _defaultDataSet;
        ///// <summary>
        ///// Instance-level dataset
        ///// </summary>
        //public DataSet DefaultDataSet {
        //    get {
        //        return _defaultDataSet;
        //    }
        //}

        /// <summary>
        /// Returns the ExceptionTable DataTable from the given DataSet
        /// </summary>
        /// <param name="dsSource"></param>
        /// <returns></returns>
		public DataTable GetExceptionTable(DataSet dsSource) {
            if (dsSource == null || dsSource.Tables["ExceptionTable"] == null) {
                throw Library.CreateBusinessException(getDisplayMember("GetExceptionTable", "DataSet not properly initialized."));
            }
			return dsSource.Tables["ExceptionTable"];
		}

        /// <summary>
        /// Returns true if LastException is not null
        /// </summary>
		public bool HasExceptions {
			[DebuggerStepThrough()]
			get {
				return _lastException != null;
			}
		}


		private int _sysUserID;
        /// <summary>
        /// Gets the primary key id for the current system user (sys_user.sys_user_id).
        /// </summary>
		public int SysUserID {
			[DebuggerStepThrough()]
			get {
				return _sysUserID;
			}
		}

        private int _webUserID;
        /// <summary>
        /// Gets the primary key id for the current web user (web_user.web_user_id).
        /// </summary>
        public int WebUserID {
            [DebuggerStepThrough()]
            get {
                return _webUserID;
            }
        }

		private int _cooperatorID;
        /// <summary>
        /// Gets the primary key id for the current cooperator (cooperator.cooperator_id).
        /// </summary>
		public int CooperatorID {
			[DebuggerStepThrough()]
			get {
				return _cooperatorID;
			}
		}

        private int _webCooperatorID;
        /// <summary>
        /// Gets the primary key id for the current web cooperator (web_cooperator.web_cooperator_id).
        /// </summary>
        public int WebCooperatorID {
            [DebuggerStepThrough()]
            get {
                return _webCooperatorID;
            }
        }

		private DateTime _lastLoginDate;
        /// <summary>
        /// Returns the UTC date of the last time the current web user successfully logged in.
        /// </summary>
		public DateTime LastLoginDate {
			[DebuggerStepThrough()]
			get {
				return _lastLoginDate;
			}
		}

        private string _remoteIP;
        /// <summary>
        /// Gets the remote IP address of the current user.  Note this may not be their actual IP, as intermediary NAT and HTTP Proxies may exist.
        /// </summary>
        public string RemoteIP {
            [DebuggerStepThrough()]
            get {
                return _remoteIP;
            }
        }

		private int _languageID;
        /// <summary>
        /// Gets or sets the primary key id for the language (sys_lang.sys_lang_id) for the current user.
        /// </summary>
		public int LanguageID {
			[DebuggerStepThrough()]
			get {
				return _languageID;
			}
            [DebuggerStepThrough()]
            set {
                _languageID = value;
            }
		}

        public string _languageDirection;
        /// <summary>
        /// Gets the text describing the direction the current user's language reads -- "RTL" means right-to-left, otherwise left-to-right should be assumed.
        /// </summary>
        public string LanguageDirection {
            [DebuggerStepThrough()]
            get {
                return _languageDirection;
            }
        }


        private string _webUserName;
        /// <summary>
        /// Gets the user name for the current web user (web_user.user_name).
        /// </summary>
        public string WebUserName {
            [DebuggerStepThrough()]
            get {
                return _webUserName;
            }
        }

		private string _sysUserName;
        /// <summary>
        /// Gets the user name for the current system user (sys_user.user_name)
        /// </summary>
		public string SysUserName {
			[DebuggerStepThrough()]
			get {
				return _sysUserName;
			}
		}

		private string _password;
        /// <summary>
        /// Sets the system password for the current user.
        /// </summary>
		public string Password {
			[DebuggerStepThrough()]
			set { _password = value; }
		}

        /// <summary>
        /// Gets the list of groups to which the current user belongs.  ASPNET nomenclature uses "roles", GRIN-Global schema and Admin Tool nomenclature is "groups".  They are semantically equivalent.
        /// </summary>
        public string[] Roles { get; private set; }

		#endregion Properties

		#region Boilerplate

        /// <summary>
        /// Logs the given text to a file if the application setting of "LogFile" is set, and the windows application event log if "EventLogSourceName" application setting is set.
        /// </summary>
        /// <param name="text"></param>
        protected void log(string text) {
            Logger.LogText("SecurityLog: User '" + SysUserName + "' " + text);
        }


		/// <summary>
		/// Initializes a DataManager object and the defaultDataSet DataSet variable if needed.  Inspect __MethodTemplate__() method for proper usage with FinishProcessing()
		/// </summary>
        /// <param name="needDataManager"></param>
        /// <param name="forceDataSetInitialization">sets defaultDataSet = createBaseDataSet(), even if defaultDataSet != null</param>
		/// <returns></returns>
		public DataManager BeginProcessing(bool needDataManager){
			if (needDataManager){
                if (_dataConnectionSpec == null) {
                    // use whatever connection info is specified in the config file under 'DataManager' connectionString
                    return DataManager.Create();
                } else {
                    // use given connection info
                    return DataManager.Create(_dataConnectionSpec);
                }
			} else {
				return null;
			}
		}

        public DataManager BeginProcessing(bool needDataManager,  bool isWebQuery)
        {
            if (needDataManager && isWebQuery)
            {
                if (_dataConnectionSpec == null)
                {
                    return DataManager.CreateWeb();
                }
                else
                {
                    // use given connection info
                    return DataManager.Create(_dataConnectionSpec);
                }
            }
            else if (needDataManager )
            {
                if (_dataConnectionSpec == null)
                {
                    // use whatever connection info is specified in the config file under 'DataManager' connectionString
                    return DataManager.Create();
                }
                else
                {
                    // use given connection info
                    return DataManager.Create(_dataConnectionSpec);
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Writes final table mapping properties to the DefaultDataSet.  Intended to be called from a finally {} block.
        /// </summary>
        public void FinishProcessing(DataSet ds, int languageID) {
			if (ds == null) {
				return;
			}

            using (DataManager dm = DataManager.Create(_dataConnectionSpec)) {
                foreach (DataTable dt in ds.Tables) {
                    if (dt.TableName.ToUpper() != "EXCEPTIONTABLE" && dt.TableName.ToUpper() != "VALIDATE_LOGIN") {
                        // look up the dataview name mapping to the friendly names from the database
                        var fmdv = Dataview.Fill(dt.TableName, languageID, dm);


                        // some tables are generated by the system and don't require mapping.  validate_login is a good example.
                        // since the property format in the dataset xml is large, we suppress them whenever we can to minimize necessary bandwidth.
                        //if (fmdv != null && !fmdv.IsPropertySuppressed) {
                        if (fmdv != null) {

                            if (dt.ExtendedProperties.Count == 0) {

                                // setting the extended properties can be somewhat costly, so we only do it here
                                // if it's not already done once.

                                // the CT and middle tier uses extended properties extensively to convey the various dataview settings
                                // and field settings that a dataview definition consists of.  i.e. the metadata (information about the actual GG data)
                                // is all stored in extended properties.

                                Debug.WriteLine("getting dv " + fmdv.DataViewName);
                                dt.ExtendedProperties.Add("title", fmdv.Title);
                                dt.ExtendedProperties.Add("description", fmdv.Description);
                                dt.ExtendedProperties.Add("is_transform", fmdv.IsTransform ? "Y" : "N");
                                dt.ExtendedProperties.Add("transform_by_fields", String.Join(",", fmdv.TransformByFields));
                                dt.ExtendedProperties.Add("transform_field_for_names", fmdv.TransformFieldForNames);
                                dt.ExtendedProperties.Add("transform_field_for_values", fmdv.TransformFieldForValues);
                                dt.ExtendedProperties.Add("transform_field_for_captions", fmdv.TransformFieldForCaptions);
                                dt.ExtendedProperties.Add("is_readonly", fmdv.IsReadOnly ? "Y" : "N");
                                dt.ExtendedProperties.Add("configuration_options", fmdv.ConfigurationOptions);
                                dt.ExtendedProperties.Add("script_direction", LanguageDirection);
                                var pkCols = fmdv.GetPrimaryKeyColumns(dt);
                                try {
                                    dt.PrimaryKey = pkCols;
                                } catch (ArgumentException) {
                                    var colNames = new List<string>();
                                    foreach(var pkCol in pkCols){
                                        colNames.Add(pkCol.ColumnName);
                                    }
                                    throw Library.CreateBusinessException(getDisplayMember("FinishProcessing{invalidpk}", "Invalid primary key defined on dataview '{0}'.  Duplicate data exists. The following are defined as primary key columns: {1}", fmdv.DataViewName , String.Join(", ", colNames.ToArray())));
                                }
                            }

                            foreach (Field fm in fmdv.Mappings) {
                                if (dt.Columns.Contains(fm.DataviewFieldName)) {
                                    DataColumn dc = dt.Columns[fm.DataviewFieldName];
                                    dc.Caption = fm.FriendlyFieldName;
                                    bool readOnly = fm.IsReadOnly || fmdv.IsReadOnly || (fm.Table != null && fm.Table.IsReadOnly);
                                    dc.ReadOnly = readOnly;

                                    if (dc.ExtendedProperties.Count == 0 || !dc.ExtendedProperties.ContainsKey("gui_hint")){
                                        // a dataview may pull in the same field from a given table more than once.
                                        // so we only fill it once to avoid errors / performance hit

                                        dc.ExtendedProperties.Add("default_value", fm.DefaultValue);
                                        
                                        //2009-12-08 brock@circaware.com
                                        // a field is now marked with is_primary_key only if it is both a primary key in sys_table_field and sys_dataview_field
                                        // previously it had to be marked in one or the other (causing us dual primary keys for dataviews mapping to 2 different tables)

                                        //2010-01-08 brock@circaware.com
                                        // changed restriction to match on the fields dataview name instead of the table field name.
                                        // See DataView.Fill() method -- it maps PrimaryKeyNames with the dataview field name, so it should match here as well.
                                        // if (fm.IsPrimaryKey && fmdv.PrimaryKeyNames.Contains(fm.TableFieldName)) {

                                        if (fm.IsPrimaryKey && fmdv.PrimaryKeyNames.Contains(fm.DataviewFieldName)) {
                                            dc.ExtendedProperties.Add("is_primary_key", "Y");
                                        } else {
                                            dc.ExtendedProperties.Add("is_primary_key", "N");
                                        }
                                        //dc.ExtendedProperties.Add("is_primary_key", fm.IsPrimaryKey ? "Y" : "N");


                                        dc.ExtendedProperties.Add("table_name", fm.TableName);
                                        dc.ExtendedProperties.Add("table_field_name", fm.TableFieldName);
                                        dc.ExtendedProperties.Add("table_field_data_type_string", fm.DataTypeString);
                                        dc.ExtendedProperties.Add("dataview_name", fm.DataviewName);
                                        dc.ExtendedProperties.Add("dataview_field_name", fm.DataviewFieldName);


                                        // NOTE: We can't enforce DataTable-level primary keys (by setting the PrimaryKey property)
                                        //       because a datatable may represent more than one table and not include the primary key
                                        //       as part of its selected fields.
                                        //if (fm.IsPrimaryKey) {
                                        //    if (dc.Table.PrimaryKey == null || dc.Table.PrimaryKey.Length == 0) {
                                        //        // none exist yet, init the PrimaryKey array
                                        //        dc.Table.PrimaryKey = new DataColumn[1] { dc };
                                        //    } else {
                                        //        // add one (by copying out the current and replacing with a new one that has the new pk tacked on the end)
                                        //        DataColumn[] pks = new DataColumn[dc.Table.PrimaryKey.Length + 1];
                                        //        Array.Copy(dc.Table.PrimaryKey, pks, dc.Table.PrimaryKey.Length);
                                        //        pks[pks.Length - 1] = dc;
                                        //        dc.Table.PrimaryKey = pks;
                                        //    }
                                        //}

                                        if (fm.Table != null) {
                                            dc.ExtendedProperties.Add("alternate_key_fields", fm.Table.UniqueKeyFields);
                                        }

                                        dc.ExtendedProperties.Add("is_foreign_key", fm.IsForeignKey ? "Y" : "N");
                                        dc.ExtendedProperties.Add("foreign_key_dataview_name", fm.ForeignKeyDataviewName);
                                        dc.ExtendedProperties.Add("foreign_key_table_field_name", fm.ForeignKeyTableFieldName);
                                        dc.ExtendedProperties.Add("foreign_key_field_name", fm.ForeignKeyTableFieldName);
                                        dc.ExtendedProperties.Add("foreign_key_dataview_param", fm.ForeignKeyDataviewParam);

                                        dc.ExtendedProperties.Add("is_nullable", fm.IsNullable ? "Y" : "N");
                                        dc.ExtendedProperties.Add("gui_hint", fm.GuiHint);
                                        dc.ExtendedProperties.Add("is_visible", fm.IsVisible ? "Y" : "N");
                                        dc.ExtendedProperties.Add("table_alias_name", fm.Table == null ? "" : fm.Table.AliasName.ToLower());


                                        dc.ExtendedProperties.Add("is_autoincrement", fm.IsAutoIncrement ? "Y" : "N");
                                        dc.ExtendedProperties.Add("is_readonly", readOnly ? "Y" : "N");
                                        dc.ExtendedProperties.Add("is_readonly_on_insert", readOnly || fm.IsReadOnlyOnInsert ? "Y" : "N");
                                        if (fm.MaximumLength > 0 && dc.DataType == typeof(string)) {
                                            if (!fm.DataviewFieldName.ToLower().StartsWith("is_")) {
                                                // assume boolean fields can't be set incorrectly...
                                                dc.MaxLength = fm.MaximumLength;
                                            }
                                        }
                                        dc.ExtendedProperties.Add("max_length", fm.MaximumLength.ToString());

                                        // 2010-06-25 brock@circaware.com
                                        // changed following to coincide with new db schema naming convention
                                        //dc.ExtendedProperties.Add("friendly_field_name", fm.FriendlyFieldName);
                                        //dc.ExtendedProperties.Add("friendly_field_description", fm.FriendlyDescription);
                                        dc.ExtendedProperties.Add("title", fm.FriendlyFieldName);
                                        dc.ExtendedProperties.Add("description", fm.FriendlyDescription);

                                        dc.ExtendedProperties.Add("configuration_options", fm.ConfigurationOptions);

                                        //dc.ExtendedProperties.Add("code_group_code", fm.GroupName);
                                        dc.ExtendedProperties.Add("group_name", fm.GroupName);

                                    }
                                } else {
                                    // column is not mapped in sys_dataview_field to sys_table_field.
                                    // we can't determine the extra attributes, so just ignore them.
                                    Debug.WriteLine("could not map dataview field=" + fm.DataviewFieldName + " to a table/field for dataview=" + fm.DataviewName);
                                }
                            }
                        }
                    }
                }

                // This is needed for Oracle support...
                if (dm.DataConnectionSpec.EngineName.ToUpper() == "ORACLE")
                {
                    scrubDataSetSchema(ds);
                }
            }
		}

        /// <summary>
        /// Corrects inconsistencies in the DataSet's DataTable schema(s) by comparing actual Column DataType(s) 
        /// to the desired Column DataType presented in the "table_field_data_type_string" ExtendedProperty
        /// </summary>
        /// <param name="ds"></param>
        private static void scrubDataSetSchema(DataSet ds)
        {
            DataSet scrubbedDataSet = new DataSet();
            foreach (DataTable dt in ds.Tables)
            {
                if (dt.Columns.Count > 0 &&
                    dt.Columns[0].ExtendedProperties.Contains("table_field_data_type_string"))
                {
                    // Start out assuming all columns are correctly typed...
                    bool tableNeedsScrubbing = false;
                    // Create an empty copy of the original table just in case the columns have inconsistent data types...
                    DataTable scrubbedDT = dt.Clone();

                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (dc.ExtendedProperties["table_field_data_type_string"] != null)
                        {
                            switch (dc.ExtendedProperties["table_field_data_type_string"].ToString().ToUpper())
                            {
                                case "INTEGER":
                                    if (dc.DataType != typeof(int))
                                    {
                                        // Found a column that is supposed to be an integer but is not
                                        // So first modify the column type in the empty 'scrubbed' datatable...
                                        scrubbedDT.Columns[dc.ColumnName].DataType = typeof(int);
                                        // And the set the bool for later intensive processing...
                                        tableNeedsScrubbing = true;
                                    }
                                    break;
                                case "DECIMAL":
                                    if (dc.DataType != typeof(decimal))
                                    {
                                        // Found a column that is supposed to be a decimal but is not
                                        // So first modify the column type in the empty 'scrubbed' datatable...
                                        scrubbedDT.Columns[dc.ColumnName].DataType = typeof(decimal);
                                        // And the set the bool for later intensive processing...
                                        tableNeedsScrubbing = true;
                                    }
                                    break;
                                case "STRING":
                                    if (dc.DataType != typeof(string))
                                    {
                                        // Found a column that is supposed to be a string but is not
                                        // So first modify the column type in the empty 'scrubbed' datatable...
                                        scrubbedDT.Columns[dc.ColumnName].DataType = typeof(string);
                                        // And the set the bool for later intensive processing...
                                        tableNeedsScrubbing = true;
                                    }
                                    break;
                                case "DATETIME":
                                    if (dc.DataType != typeof(DateTime))
                                    {
                                        // Found a column that is supposed to be a datetime but is not
                                        // So first modify the column type in the empty 'scrubbed' datatable...
                                        scrubbedDT.Columns[dc.ColumnName].DataType = typeof(DateTime);
                                        // And the set the bool for later intensive processing...
                                        tableNeedsScrubbing = true;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    // Now do the really heavy processing if the field types were inconsistent...
                    if (tableNeedsScrubbing)
                    {
                        try
                        {
                            // Depending on how big the datatable is, this could take a while, so 
                            // suspend relational DB integrity...
                            scrubbedDT.BeginLoadData();
                            // Load the data in a stream...
                            scrubbedDT.Load(dt.CreateDataReader());
                            // Re-apply relational DB constraints...
                            scrubbedDT.EndLoadData();
                            // Make sure there are no pending changes to the table...
                            scrubbedDT.AcceptChanges();

                            // Now add the scrubbed table to the scrubbed dataset...
                            scrubbedDataSet.Tables.Add(scrubbedDT);
                        }
                        catch (Exception)
                        {
                            // Something went wrong during the conversion process (most likely data that could not be converted
                            // to the desired datatype so for right now we will keep the original datatable in the dataset
                            // and pass the whole mess back to the calling program for them to deal with... 
                        }
                    }
                }
            }

            // Finally replace all of the tables that need to have their schema scrubed in the original dataset...
            foreach (DataTable dt in scrubbedDataSet.Tables)
            {
                if (ds.Tables.Contains(dt.TableName))
                {
                    // Remove the original datatable with the broken schema...
                    ds.Tables.Remove(dt.TableName);
                    // Add the new table with the scrubbed schema...
                    ds.Tables.Add(dt.Copy());
                }
            }

            // Release the memory for the temp scrubbedDataset...
            scrubbedDataSet.Dispose();
        }

        /// <summary>
        /// Initializes a new instance of SecureData.
        /// </summary>
        /// <param name="suppressExceptions"></param>
		protected SecureData(bool suppressExceptions) {
//			_defaultDataSet = createBaseDataSet();
			_suppressExceptions = suppressExceptions;
		}

        /// <summary>
        /// Initializes a new instance of SecureData.  Throws an exception if loginToken is null or empty.
        /// </summary>
        /// <param name="suppressExceptions"></param>
        /// <param name="loginToken"></param>
		public SecureData(bool suppressExceptions, string loginToken) : this(suppressExceptions, loginToken, null) {
            if (String.IsNullOrEmpty(loginToken) || this._sysUserID == 0){
                throw new InvalidCredentialException(getDisplayMember("InvalidLoginToken", "Login token is invalid.  Please generate a new token using ValidateLogin()."));
            }
            
        }

        /// <summary>
        /// Returns the EngineName of the current database engine.  Possible values are "oracle", "sqlserver", "mysql", "postgresql", or "sqlite".
        /// </summary>
        /// <returns></returns>
        public static string CurrentDatabaseEngine() {
            using (var dm = DataManager.Create()) {
                return dm.DataConnectionSpec.EngineName;
            }
        }

        /// <summary>
        /// Initializes a new instance of SecureData.  Parses the given loginToken into the various properties (SysUserID, CooperatorID, SysUserName, WebUserID, WebCooperatorID, LanguageID, LanguageDirection, RemoteIP, and Roles)
        /// </summary>
        /// <param name="suppressExceptions"></param>
        /// <param name="loginToken"></param>
        /// <param name="dcs"></param>
        public SecureData(bool suppressExceptions, string loginToken, DataConnectionSpec dcs) : this(suppressExceptions) {

            // use the given login token to authorize request

            _dataConnectionSpec = dcs;

            var tok = LoginToken.Parse(loginToken);
            if (tok != null) {
                _sysUserID = tok.UserID;
                _cooperatorID = tok.CooperatorID;
                _sysUserName = tok.UserName;

                _webUserID = tok.WebUserID;
                _webCooperatorID = tok.WebCooperatorID;
                _webUserName = tok.WebUserName;

                LanguageID = tok.LanguageID;
                _languageDirection = tok.LanguageDirection;

                _remoteIP = tok.RemoteIP;
                Roles = tok.Roles;
            }
		}

        //public int GetSystemCooperatorID() {
        //    var cm = CacheManager.Get("GenericSettings");
        //    var langDir = cm[languageID.ToString()] as string;
        //    if (String.IsNullOrEmpty(langDir)) {
        //        using (var dm = BeginProcessing(true)) {
        //            try {
        //                langDir = "" + dm.ReadValue("select script_direction from sys_lang where sys_lang_id = :langid", new DataParameters(":langid", languageID));
        //            } catch {
        //                // assume left to right if the lookup fails for some reason
        //                langDir = "LTR";
        //            }
        //            cm[languageID.ToString()] = langDir;
        //        }
        //    }
        //    return langDir;
        //}

        /// <summary>
        /// Fetches the sys_lang.script_direction value from the database for the given languageID (sys_lang_id).  Assumes "LTR" (left-to-right) on failure.  "RTL" means right-to-left.
        /// </summary>
        /// <param name="languageID"></param>
        /// <returns></returns>
        private string getLanguageDirection(int languageID) {
            var cm = CacheManager.Get("GenericSettings");
            var langDir = cm["Language" + languageID] as string;
            if (String.IsNullOrEmpty(langDir)) {
                using (var dm = BeginProcessing(true)) {
                    try {
                        langDir = "" + dm.ReadValue("select script_direction from sys_lang where sys_lang_id = :langid", new DataParameters(":langid", languageID));
                    } catch {
                        // assume left to right if the lookup fails for some reason
                        langDir = "LTR";
                    }
                    cm["Language" + languageID] = langDir;
                }
            }
            return langDir;
        }

//		[DebuggerStepThrough()]
        /// <summary>
        /// Clears the local cache for the current SysUserName.
        /// </summary>
		public void Logout() {
			CacheManager cm = CacheManager.Get("Login");
			cm[SysUserName] = null;
			cm[SysUserName + "_dataset"] = null;
		}

        /// <summary>
        /// Gets the cached login information for the current SysUserName.
        /// </summary>
        /// <returns></returns>
		public DataSet GetLoginInfo() {
			CacheManager cm = CacheManager.Get("Login");
			DataSet ds = cm[SysUserName + "_dataset"] as DataSet;
			return ds;
		}

        /// <summary>
        /// Verifies the login credentials stored within the GG database's sys_user table.  Assumes the 'password' parameter is a SHA-1 hash of the actual password.
        /// </summary>
        /// <param name="suppressExceptions"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static DataSet TestLogin(bool suppressExceptions, string userName, string password) {
            using (var sd = new SecureData(suppressExceptions)) {
                return sd.softLogin(userName, password);
            }
        }

        /// <summary>
        /// Connects to the database and attempts to verify given userName/password against data in sys_user table.  Failed logins do not throw exceptions (only logged to the return DataSet), but database connectivity issues will throw exceptions.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private DataSet softLogin(string userName, string password) {

            // since doLogin will throw an exception regardless of suppressExceptions,
            // softLogin just wraps it and respects the suppressExceptions flag.

            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    doLogin(userName, password, dm, dsReturn);

                    // Log the login
                    Logger.LogText("Successfull login by: " + userName);
                }
            } catch (Exception ex){

                // non-login failed errors should ALWAYS throw exceptions (i.e. can't get to db)
                if (LogException(ex, dsReturn)) {
                    throw;
                }

            } finally {
                FinishProcessing(dsReturn, _languageID);
            }
            return dsReturn;
        }

//		[DebuggerStepThrough()]
        /// <summary>
        /// Verifies the given userName/password against values in the sys_user table.  Returns the loginToken value on success, null on failure.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Login(string userName, string password) {
            return Login(userName, password, null);
        }

        /// <summary>
        /// Returns non-empty string if database connectivity is successful, null / empty string otherwise.  String value typically consists of database engine version info, but this is not guaranteed across all database engines.
        /// </summary>
        /// <param name="dcs"></param>
        /// <returns></returns>
        public static string TestDatabaseConnection(DataConnectionSpec dcs) {
            try {
                using (SecureData sd = new SecureData(false, null, dcs)) {
                    using (var dm = sd.BeginProcessing(true)) {
                        return dm.TestLogin();
                    }
                }
            } catch (Exception ex) {
                Debug.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        /// <summary>
        /// Verifies the given userName/password against the values in sys_user.  Throws exception on failure.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dcs"></param>
        /// <returns></returns>
		public static string Login(string userName, string password, DataConnectionSpec dcs) {
            // we pass false so we never suppress exceptions
			using (SecureData sd = new SecureData(false)) {
                DataSet dsReturn = sd.createReturnDataSet();
				try {
                    sd._dataConnectionSpec = dcs;
                    using (DataManager dm = sd.BeginProcessing(true)) {
                        return sd.doLogin(userName, password, dm, dsReturn);
                    }
				} catch (Exception ex) {
					if (sd.LogException(ex, dsReturn)) {
						throw ex;
					} else {
						return null;
					}
				} finally {
                    sd.FinishProcessing(dsReturn, sd.LanguageID);
				}
			}
		}


        /// <summary>
        /// Using given userName and SHA-1 hashing the given password, compares those values against values stored in sys_user table.  Returns loginToken on success, throws InvalidCredentialException on failure.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dm"></param>
        /// <param name="dsReturn"></param>
        /// <returns></returns>
        private string doLogin(string userName, string password, DataManager dm, DataSet dsReturn) {
            string loginToken = null;

            // check for account lockout either permanent or temporary
            if (existTempAccountLock(userName)) {
                //TODO: change to unhelpful "InvalidUserNamePassword", "Invalid user name / password combination."
                throw new InvalidCredentialException(getDisplayMember("LockedUser", "Account temporarily locked."));
            }

            // get info on account 
            dm.Limit = 1;
            dm.Read(@"
SELECT 
    su.sys_user_id,
    su.user_name,
    su.password,
    su.is_enabled,
    su.cooperator_id,
    su.created_date,
    su.created_by,
    COALESCE(su.modified_date,su.created_date) AS modified_date,
    su.modified_by,
    su.owned_date,
    su.owned_by,
    co.sys_lang_id,
    co.site_id,
    s.site_short_name as site,
    sl.script_direction,
    '' as groups,
    '' as login_token,
    '' as warning
  FROM sys_user su
    LEFT JOIN cooperator co ON su.cooperator_id = co.cooperator_id
    LEFT JOIN site        s ON co.site_id = s.site_id
    LEFT JOIN sys_lang   sl ON sl.sys_lang_id = co.sys_lang_id
  WHERE (su.user_name = :username OR su.user_name LIKE :username + '@%') AND su.is_enabled = 'Y'
", dsReturn, "validate_login", new DataParameters(":username", userName, DbType.String));

            DataTable dt = dsReturn.Tables["validate_login"];
            if (dt.Rows.Count < 1) {
                createTempAccountLock(userName);
                Logger.LogText("Failed username from " + System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                throw new InvalidCredentialException(getDisplayMember("InvalidUserNamePassword", "Invalid user name / password combination."));
            }

            // get stored password hash, then remove from dataset
            DataRow dr = dt.Rows[0];
            string storedHash = dr["password"].ToString();
            dt.Columns.Remove("password");

            string LdapDomain = Toolkit.GetSetting("SysUserLdapDomain", "");
            string passwordHash = Crypto.HashText(password);

            // First, try password match against sys_user password field
            if (comparePasswordHash(password, storedHash) || comparePasswordHash(passwordHash, storedHash)) {

                // generate a login token
                loginToken = generateLoginToken(dt.Rows[0], userName, false);

                // Check password age
                int maxPassAge = Toolkit.GetSetting("SysUserPasswordMaximumAge", 60);
                int maxPassWarn = Toolkit.GetSetting("SysUserPasswordWarning", 14);
                int maxPassLock = Toolkit.GetSetting("SysUserPasswordExpireLock", 30);
                DateTime modifiedDate = DateTime.Parse((dr["modified_date"]).ToString());
                double passAge = (DateTime.UtcNow - modifiedDate).TotalDays;
                if (maxPassAge > 0 && passAge > maxPassAge) {
                    if (maxPassLock != -1 && passAge > (maxPassAge + maxPassLock)) {
                        // remove all user info on failure
                        dsReturn.Tables.Remove("validate_login");
                        Logger.LogText("Expired password lock for user " + userName + " from " + System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                        throw new InvalidCredentialException(getDisplayMember("LoginExpiredPasswordLocked", "Account locked due to password expiration."));
                    }
                    // remove all user info on failure
                    dsReturn.Tables.Remove("validate_login");
                    Logger.LogText("Expired password for user " + userName + " from " + System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
                    throw new InvalidCredentialException(getDisplayMember("LoginExpiredPassword", "Password expired."));
                } else if (maxPassWarn > 0 && passAge > (maxPassAge - maxPassWarn)) {
                    dr["warning"] = getDisplayMember("LoginPasswordWarning", "Your password will expire in {0} days.", (Math.Truncate(maxPassAge - passAge)).ToString());
                }
                dt.AcceptChanges();

                return loginToken;

            // Try authenticating password against LDAP if doamin specified
            } else if (!String.IsNullOrEmpty(LdapDomain)) {
                var cm = CacheManager.Get("ldaplogin");
                
                string loginKey = userName + ":" + passwordHash;
                string ldapResult = cm[loginKey] as string;

                // if LDAP result not already cached, check ahaingst LDAP server
                if (String.IsNullOrEmpty(ldapResult)) {
                    ldapResult = "unauthorized";
                    if (LdapAuthenticate(userName, password, LdapDomain)) ldapResult = "authorized";
                    //Logger.LogText("SecureData checking LDAP for " + userName  + " result: " + ldapResult);  //KFEdebug
                    cm[loginKey] = ldapResult;  // cache result
                }

                if (ldapResult == "authorized") {
                    // generate a login token
                    loginToken = generateLoginToken(dt.Rows[0], userName, false);
                    dt.AcceptChanges();
                    return loginToken;
                }
            }

            // validation failed, posibly lock account then remove all user data
            createTempAccountLock(userName);
            dsReturn.Tables.Remove("validate_login");

            //KMK 06/14/2017  "Test connection" button on the Admin Tool Login was failing if the log in information is not correctly filled it.  It threw an unhandled exception.  
            if (password != null) {
                Logger.LogText("Failed password for user " + userName + " from " + System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            }
            throw new InvalidCredentialException(getDisplayMember("InvalidUserNamePassword", "Invalid user name / password combination."));
        }


        /// <summary>
        /// Authenticates login password against Active Directory by LDAP
        /// </summary>
        /// <param name="userNamer"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private bool LdapAuthenticate(string userName, string password, string domain) {
            bool authenticated = false;

            // trim domain email suffix if present
            string suffix = Toolkit.GetSetting("SysUserLdapIgnoreSuffix", "");
            if (!string.IsNullOrEmpty(suffix) && userName.EndsWith(suffix)) {
                userName = userName.Substring(0, userName.Length - suffix.Length);
            }

            // use server's root domain if requested
            if (string.IsNullOrEmpty(domain) || domain == "LDAP://RootDSE") {
                using (DirectoryEntry rootDSE = new DirectoryEntry("LDAP://RootDSE")) {
                    domain = rootDSE.Properties["defaultNamingContext"].Value.ToString();
                }
            }

            try {
                if (!domain.StartsWith("LDAP://")) domain = "LDAP://" + domain;
                DirectoryEntry entry = new DirectoryEntry(domain, userName, password);
                object nativeObject = entry.NativeObject;
                authenticated = true;
                //Logger.LogText("KFE Debug LdapAuthenticate - user: "+ userName +" authenticated to domain: "+domain);
            } catch (DirectoryServicesCOMException) {
                //Logger.LogText("KFE Debug LdapAuthenticate - authentication failed for user: "+userName+" domain: "+domain);
            }

            return authenticated;
        }

        /// <summary>
        /// Compares plaintext password with stored hashed password. Returns bool true on success.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        private bool comparePasswordHash(string password, string storedSaltHash) {
            string crypt;            
            string salt;
            string storedHash;

            // parse the stored password field for hash type, salt, and hashed password
            string[] hashes = storedSaltHash.Split(':');
            string[] passField = hashes[0].Split('$');
            if (passField.Length == 1) {
                // original format of SHA1 hash with no salt
                crypt = "SHA1";
                salt = "";
                storedHash = passField[0];
            } else if (passField.Length == 2) {
                // two fields means salt and hash
                crypt = "SHA256";
                salt = passField[0];
                storedHash = passField[1];
            } else if (passField.Length == 3) {
                // with three fields the first is the hash type
                crypt = passField[0];
                salt = passField[1];
                storedHash = passField[2];
            } else {
                // can't figure out what is stored in the hash field
                return false;
            }

            string hashedPassword;
            if (crypt == "SHA1") {
                hashedPassword = Crypto.HashText(salt+password);
            } else if (crypt == "SHA256") {
                hashedPassword = Crypto.HashTextSHA256(salt+password);
            } else {
                // don't understand the hash type
                return false;
            }

            // Finally we test whether it is a match
            if (hashedPassword == storedHash) return true;

            return false;
        }


        private class tempLockInfo {
            public string userName { get; set; }
            public DateTime failTime { get; set; }
            public int failCount { get; set; }
        }

        /// <summary>
        /// Check for the existance of an account lock. Returns bool true if lock exists
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private bool existTempAccountLock(string userName) {
            int lockPeriod = Toolkit.GetSetting("SysUserLoginLockPeriod", 900);
            int loginTries = Toolkit.GetSetting("SysUserMaxLoginFailures", 5);
            if (loginTries <= 1) return false;
            string cacheName =  "UserLoginLockCache";
            string cacheKey = userName;
            CacheManager cache = CacheManager.Get(cacheName);
            var userlock = (tempLockInfo)(cache[cacheKey]);
            if (userlock == null) {
                return false;
            } else {
                if ((DateTime.UtcNow - userlock.failTime).TotalSeconds < lockPeriod) {
                    if (userlock.failCount > loginTries) return true;
                } else {
                    cache.Remove(cacheKey);
                }
            }
            return false;
        }


        /// <summary>
        /// create or add to account lock record
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private void createTempAccountLock(string userName) {
            int failPeriod = Toolkit.GetSetting("SysUserLoginFailPeriod", 900);
            string cacheName =  "UserLoginLockCache";
            string cacheKey = userName;
            CacheManager cache = CacheManager.Get(cacheName);
            var userlock = (tempLockInfo)(cache[cacheKey]);
            if (userlock != null) {
                // reset if lock not recent
                if ((DateTime.UtcNow - userlock.failTime).TotalSeconds > failPeriod) {
                    userlock.failTime = DateTime.UtcNow;
                    userlock.failCount = 0;
                }
                userlock.failCount += 1;
                cache[cacheKey] = userlock;
            } else {
                userlock = new tempLockInfo();
                userlock.failTime = DateTime.UtcNow;
                userlock.failCount = 1;
                cache[cacheKey] = userlock;
            }
        }



        /// <summary>
        /// Verifies the given username/password against the web_user table. Optionally ignores password.  Returns loginToken on success, throws InvalidCredentialException on failure.
        /// </summary>
        /// <param name="webUserName"></param>
        /// <param name="webPassword"></param>
        /// <param name="ignorePassword"></param>
        /// <param name="appliedEncryption"></param>
        /// <returns></returns>
        public static string WebLogin(string webUserName, string webPassword, bool ignorePassword, bool appliedEncryption) {
            return WebLogin(webUserName, webPassword, null, ignorePassword, appliedEncryption);
        }

        /// <summary>
        /// Verifies the given username/password against the web_user table. Optionally ignores password.  Returns loginToken on success, throws InvalidCredentialException on failure.
        /// </summary>
        /// <param name="webUserName"></param>
        /// <param name="webPassword"></param>
        /// <param name="dcs"></param>
        /// <param name="ignorePassword"></param>
        /// <param name="appliedEncryption"></param>
        /// <returns></returns>
        public static string WebLogin(string webUserName, string webPassword, DataConnectionSpec dcs, bool ignorePassword, bool appliedEncryption) {
            // we pass false so we never suppress exceptions
            using (SecureData sd = new SecureData(false)) {
                DataSet dsReturn = sd.createReturnDataSet();
                try {
                    sd._dataConnectionSpec = dcs;
                    using (DataManager dm = sd.BeginProcessing(true)) {

                        var doublyHashedPassword = "";
                        if (appliedEncryption)
                        {
                            doublyHashedPassword = Crypto.HashText(webPassword);
                           
                        }
                        else
                        {
                            var hashedPassword = Crypto.HashText(webPassword);
                            doublyHashedPassword = Crypto.HashText(hashedPassword);
                        }
                        // Logger.LogText("attempting WebLogin for user '" + webUserName + "', encpw='" + encryptedPassword + "' or hashpw='" + hashedPassword + "'");

                        dm.Limit = 1;
                        dm.Read(@"
select 
	wu.web_user_id,
	wu.user_name,
	wu.is_enabled,
	wu.web_cooperator_id,
	wu.created_date,
	wu.modified_date,
wu.sys_lang_id,
sl.script_direction
from 
	web_user wu left join sys_lang sl 
        on sl.sys_lang_id = wu.sys_lang_id
where 
	wu.user_name = :username 
    and (wu.password = :hashpw or 1 = :ignorepw)
	and wu.is_enabled = 'Y'
", dsReturn, "validate_web_login", new DataParameters(
     ":username", webUserName, DbType.String,
     ":hashpw", doublyHashedPassword, DbType.String,
     ":ignorepw", (ignorePassword ? 1 : 0), DbType.Int32));



                        DataTable dt = dsReturn.Tables["validate_web_login"];
                        if (dt.Rows.Count > 0) {

                            return sd.generateLoginToken(dt.Rows[0], webUserName, true);

                        } else {
                            throw new InvalidCredentialException(getDisplayMember("InvalidUserNamePassword", "Invalid user name / password combination."));
                        }
                    }
                } catch (Exception ex) {
                    if (sd.LogException(ex, dsReturn)) {
                        throw ex;
                    } else {
                        return null;
                    }
                } finally {
                    sd.FinishProcessing(dsReturn, sd.LanguageID);
                }
            }
        }

        /// <summary>
        /// Generates a new login token based on SecureData object properties
        /// </summary>
        /// <returns></returns>
        public string RegenerateLoginToken() {
            var loginToken = new LoginToken(_sysUserID, _cooperatorID, _sysUserName, _webUserID, _webCooperatorID, _webUserName, _languageID, _languageDirection, Roles);
            return loginToken.ToString();
        }

        /// <summary>
        /// Generates a new login token based on the given DataRow.
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="userName"></param>
        /// <param name="isWebUser"></param>
        /// <returns></returns>
        private string generateLoginToken(DataRow dr, string userName, bool isWebUser) {
            // generate a login token based on their cooperatorid .... and maybe current time ???

            if (isWebUser){

                _webUserID = Toolkit.ToInt32(dr["web_user_id"], -1);
                _webCooperatorID = Toolkit.ToInt32(dr["web_cooperator_id"], -1);
                _webUserName = userName;

                _sysUserID = getSysUserIDForUser(_webCooperatorID);
                if (_sysUserID == -1) {
                    _sysUserID = GetSysUserIDForGuestWebUser();
                    _sysUserName = Toolkit.GetSetting("AnonymousUserName", "guest");
                } else {
                    _sysUserName = getSysUserNameForUser(_webCooperatorID);
                }

                _cooperatorID = getCooperatorIDForUser(_webCooperatorID);
                if (_cooperatorID == -1) {
                    _cooperatorID = GetCooperatorIDForGuestWebUser();
                }


                _languageID = Toolkit.ToInt32(dr["sys_lang_id"], -1);
                _languageDirection = dr["script_direction"].ToString(); // getLanguageDirection(_languageID);
                var ctx = HttpContext.Current;
                if (ctx != null) {
                    if (ctx.Request != null) {
                        if (Toolkit.GetSetting("AllowCookies", true)) {
                            var cookie = ctx.Request.Cookies["language"];
                            if (cookie != null) {
                                _languageID = Toolkit.ToInt32(cookie.Value, _languageID);
                            }
                        }
                    }
                }

            } else {

                _sysUserID = Toolkit.ToInt32(dr["sys_user_id"], -1);
                _cooperatorID = Toolkit.ToInt32(dr["cooperator_id"], -1);
                _sysUserName = userName;

                _webUserID = -1;
                _webCooperatorID = -1;
                _webUserName = null;

                _languageID = Toolkit.ToInt32(dr["sys_lang_id"], -1);
                _languageDirection = dr["script_direction"].ToString(); // getLanguageDirection(_languageID);

            }

            Roles = GetRoles(userName, _sysUserID).ToArray();

            if (dr.Table.Columns.Contains("groups")) {
                dr["groups"] = String.Join("\t", Roles);
            }


            var loginToken = new LoginToken(_sysUserID, _cooperatorID, _sysUserName, _webUserID, _webCooperatorID, _webUserName, _languageID, _languageDirection, Roles);
            if (loginToken == null) {
                return null;
            } else {
                var tok = loginToken.ToString();
                //Console.WriteLine("logintoken=" + tok);
                if (dr.Table.Columns.Contains("login_token")) {
                    dr["login_token"] = tok;
                }
                return tok;
            }
        }

		#endregion Boilerplate



		#region List Management
        /// <summary>
        /// Deletes the given list / tab for given cooperator from the app_user_item_list table.  Not currently used by CT.
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="tabName"></param>
        /// <param name="cooperatorID"></param>
        /// <returns></returns>
		public virtual DataSet DeleteList(string listName, string tabName, int cooperatorID) {
            DataSet dsReturn = createReturnDataSet();
            try {
				using (DataManager dm = BeginProcessing(true)) {
                    if (!CanDelete(dm, "app_user_item_list", null, null)) {
                        throw Library.DataPermissionException(this.SysUserName, "app_user_item_list", Permission.Delete, null);
                    }
					dm.Write(@"
delete from
	app_user_item_list
where
	cooperator_id = :cooperatorid
	and list_name = :listname
	and tab_name = :tabname
", new DataParameters(":cooperatorid", cooperatorID, DbType.Int32, ":listname", listName, ":tabname", tabName));
				}
			} catch (Exception ex) {
				if (LogException(ex, dsReturn)) {
					throw ex;
				}
			} finally {
                FinishProcessing(dsReturn, _languageID);
			}
			return dsReturn;
		}

        /// <summary>
        /// Updates the list and tab names for the given list/tab names.  Not currently used by CT.
        /// </summary>
        /// <param name="existingListName"></param>
        /// <param name="newListName"></param>
        /// <param name="existingTabName"></param>
        /// <param name="newTabName"></param>
        /// <param name="cooperatorID"></param>
        /// <returns></returns>
        public DataSet RenameList(string existingListName, string newListName, string existingTabName, string newTabName, int cooperatorID) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    if (!CanUpdate(dm, "app_user_item_list", null, null)) {
                        throw Library.DataPermissionException(this.SysUserName, "app_user_item_list", Permission.Update, null);
                    }
                    dm.Write(@"
update
	app_user_item_list
set
	list_name = :newlistname,
	tab_name = :newtabname,
	modified_date = :modifieddate,
	modified_by = :modifiedby
where
	list_name = :existinlistname
	and tab_name = :existingtabname
	and cooperator_id = :cooperatorid
", new DataParameters(
             new DataParameter(":newlistname", newListName),
             new DataParameter(":newtabname", newTabName),
             new DataParameter(":modifieddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2),
             new DataParameter(":modifiedby", CooperatorID, DbType.Int32),
             new DataParameter(":existinlistname", existingListName),
             new DataParameter(":existingtabname", existingTabName),
             new DataParameter(":cooperatorid", cooperatorID, DbType.Int32)));
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, _languageID);
            }
            return dsReturn ;

        }


        /// <summary>
        /// Updates the tab name for the given tab.  Not currently used by CT.
        /// </summary>
        /// <param name="existingTabName"></param>
        /// <param name="newTabName"></param>
        /// <param name="cooperatorID"></param>
        /// <returns></returns>
        public virtual DataSet RenameTab(string existingTabName, string newTabName, int cooperatorID) {

            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    if (!CanUpdate(dm, "app_user_item_list", null, null)) {
                        throw Library.DataPermissionException(this.SysUserName, "app_user_item_list", Permission.Update, null);
                    }
                    dm.Write(@"
update
	app_user_item_list
set
	tab_name = :newtabname,
	modified_date = :modifieddate,
	modified_by = :modifiedby
where
	tab_name = :existingtabname
	and cooperator_id = :cooperatorid
", new DataParameters(
                         new DataParameter(":newtabname", newTabName),
                         new DataParameter(":modifieddate", DateTime.Now.ToUniversalTime(), DbType.DateTime2),
                         new DataParameter(":modifiedby", CooperatorID, DbType.Int32),
                         new DataParameter(":existingtabname", existingTabName),
                         new DataParameter(":cooperatorid", cooperatorID, DbType.Int32)));
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, _languageID);
            }
            return dsReturn;

        }


        #endregion List Management



        #region Get Data

        #endregion Get Data



        #region Miscellaneous
        /// <summary>
        /// Changes the password for the given system user.  Expects the password in plain text.  Affects the sys_user table.
        /// </summary>
        /// <param name="suppressExceptions"></param>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="targetUserName"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public static DataSet ChangePassword(bool suppressExceptions, string userName, string oldPassword, string targetUserName, string newPassword) {
            using (var sd = new SecureData(suppressExceptions)) {
                return sd.ChangePassword(userName, oldPassword, targetUserName, newPassword);
            }
        }

        /// <summary>
        /// Changes the password for the given system user.  Expects the password in plain text.  Affects the sys_user table.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="oldPassword"></param>
        /// <param name="targetUserName"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        private DataSet ChangePassword(string userName, string oldPassword, string targetUserName, string newPassword) {
            DataSet dsReturn = createReturnDataSet();
            string mustSsl = Toolkit.GetSetting("SysChangePasswordRequiresSSL", "No");

            if (mustSsl.ToLower() == "true" && System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"] != "443") {
                throw Library.CreateBusinessException(getDisplayMember("ChangePasswordPort", "Must connect on secure port 443."));
            }

            if (oldPassword.Length == 28 && oldPassword.Substring(27) == "=") throw Library.CreateBusinessException(getDisplayMember("ChangePasswordHash", "Plain text required for password change. Upgrade your CT."));


            try {
                using (DataManager dm = BeginProcessing(true)) {
                    try {
                        doLogin(userName, oldPassword, dm, dsReturn);
                    } catch (InvalidCredentialException ice) {
                        // it's OK to ignore password expiration when changing password
                        if (ice.Message != "Password expired.") throw;
                    }
                                
                    int minPassDiffs = Toolkit.GetSetting("SysUserPasswordMinDiffs", 3);
                    int diffCount = numberOfDifferences(oldPassword, newPassword);
                    if (diffCount < minPassDiffs) {
                        throw Library.CreateBusinessException(getDisplayMember("ChangePasswordDiffFail", "New password only differs by {0} characters. A difference of {1} is required.", diffCount.ToString(), minPassDiffs.ToString()));
                    }
            
                    // proceed with the password change
                    return ChangePassword(targetUserName, newPassword);
                }
            } catch (Exception ex) {
                // non-login failed errors should ALWAYS throw exceptions (i.e. can't get to db)
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, _languageID);
            }
            return dsReturn;
        }

        /// <summary>
        /// Compares two strings and returns the number of differences
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        static int numberOfDifferences(string s1, string s2) {
            if (String.IsNullOrEmpty(s1)) return s2.Length;
            if (String.IsNullOrEmpty(s2)) return s1.Length;
            string shorter;
            string longer;
            if (s1.Length < s2.Length) {
                shorter = s1;
                longer = s2;
            } else {
                shorter = s2;
                longer = s1;
            }

            for (int patternLen = shorter.Length; patternLen > 0; patternLen--) {
                for (int patternStartPos = 0; patternStartPos + patternLen <= shorter.Length; patternStartPos++) {
                    string pattern = shorter.Substring(patternStartPos, patternLen);
                    int matchPos = longer.IndexOf(pattern);
                    if (matchPos != -1) {
                        //throw Library.CreateBusinessException(getDisplayMember("DEBUGdiff", "Found match of length {0} at short {1} and long {2}.", patternLen.ToString(), patternStartPos.ToString(), matchPos.ToString() ));

                        //before
                        int bDiff;
                        if (patternStartPos == 0) {
                            bDiff = matchPos;
                        } else if (matchPos == 0) {
                            bDiff = patternStartPos;
                        } else {
                            bDiff = numberOfDifferences(shorter.Substring(0, patternStartPos), longer.Substring(0, matchPos));
                        }
                        //after
                        int aDiff;
                        if (patternStartPos + patternLen == shorter.Length) {
                            aDiff = longer.Length - (matchPos + patternLen);
                        } else if (matchPos + patternLen == longer.Length) {
                            aDiff = shorter.Length - (patternStartPos + patternLen);
                        } else {
                            aDiff = numberOfDifferences(shorter.Substring(patternStartPos + patternLen), longer.Substring(matchPos + patternLen));
                        }
                        return bDiff + aDiff;
                    }
                }
            }

            //If no match at all the difference is the size of the longer string
            return longer.Length;
        }

        /// <summary>
        /// Changes the password for the given system user.  Expects the password in SHA-1 hashed format.  Affects the sys_user table.
        /// </summary>
        /// <param name="targetUserName"></param>
        /// <param name="newHashedPassword"></param>
        /// <returns></returns>
        public DataSet ChangePassword(string targetUserName, string newPassword) {

            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {

                    if (SysUserName != targetUserName) {

                        checkUserHasAdminEnabled();

                        // throw Library.CreateBusinessException("You do not have rights to change the password for user '" + targetUserName + "'.");

                    } else {
                        //some checks for non admins

                        if (isTooSoonToChangePassword(targetUserName, dm)) {
                            throw Library.CreateBusinessException(getDisplayMember("ChangePasswordTooSoon", "It is too soon to change your password."));
                        }

                        if (!isPasswordComplexEnough(newPassword)) {
                            throw Library.CreateBusinessException(getDisplayMember("ChangePasswordComplexity", "New password is not complex enough."));
                        }

                        if (isPasswordReused(targetUserName, newPassword, dm)) {
                            throw Library.CreateBusinessException(getDisplayMember("ChangePasswordReuse", "Cannot reuse old password."));
                        }
                    }

                    if (newPassword.Length == 28 && newPassword.Substring(27) == "=") throw Library.CreateBusinessException(getDisplayMember("ChangePasswordHash", "Plain text required for password change. Upgrade your CT."));

                    setPassword(targetUserName, Crypto.HashText(newPassword), CooperatorID, dm);

                    log("changed the password for user '" + targetUserName + "'.");
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

                /// <summary>
        /// Tests Password complexity requirements. Returns false on failure.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="storedHash"></param>
        /// <returns></returns>
        private bool isPasswordComplexEnough(string password) {

            int passMinLength = Toolkit.GetSetting("SysUserPasswordMinLength", 12);
            if (password.Length < passMinLength) {
                throw Library.CreateBusinessException(getDisplayMember("ChangePasswordMinLength", "New password is not long enough, must be at least {0} characters.", passMinLength.ToString()));
                //return false;
            }

            int passMaxLength = Toolkit.GetSetting("SysUserPasswordMaxLength", 255);
            if (password.Length > passMaxLength) {
                throw Library.CreateBusinessException(getDisplayMember("ChangePasswordMaxLength", "New password is too long, {0} character maximum.", passMaxLength.ToString()));
                //return false;
            }

            string passReqPatt1 = Toolkit.GetSetting("SysUserPasswordReqDigit", @"\p{Nd}");
            int passReqCnt1 = Toolkit.GetSetting("SysUserPasswordReqPatt1Ccnt", 1);
            if (passReqCnt1 > 0 && Regex.Matches(password, passReqPatt1).Count < passReqCnt1) {
                throw Library.CreateBusinessException(getDisplayMember("ChangePasswordReqDigit", "New password must contain at least {0} digit.", passReqCnt1.ToString()));
                //return false;
            }
            
            string passReqPatt2 = Toolkit.GetSetting("SysUserPasswordReqLower", @"\p{Ll}");
            int passReqCnt2 = Toolkit.GetSetting("SysUserPasswordReqPatt2Ccnt", 1);
            if (passReqCnt2 > 0 && Regex.Matches(password, passReqPatt2).Count < passReqCnt2) {
                throw Library.CreateBusinessException(getDisplayMember("ChangePasswordReqLower", "New password must contain at least {0} lower case letter.", passReqCnt2.ToString()));
                //return false;
            }

            string passReqPatt3 = Toolkit.GetSetting("SysUserPasswordReqUpper", @"\p{Lu}");
            int passReqCnt3 = Toolkit.GetSetting("SysUserPasswordReqPatt3Ccnt", 1);
            if (passReqCnt3 > 0 && Regex.Matches(password, passReqPatt3).Count < passReqCnt3) {
                throw Library.CreateBusinessException(getDisplayMember("ChangePasswordReqUpper", "New password must contain at least {0} upper case letter.", passReqCnt2.ToString()));
                //return false;
            }

            string passReqPatt4 = Toolkit.GetSetting("SysUserPasswordReqSpecial", @"[\p{S}\p{P}\p{Z}\p{C}]");
            int passReqCnt4 = Toolkit.GetSetting("SysUserPasswordReqPatt4Ccnt", 1);
            if (passReqCnt4 > 0 && Regex.Matches(password, passReqPatt4).Count < passReqCnt4) {
                throw Library.CreateBusinessException(getDisplayMember("ChangePasswordReqSpecial", "New password must contain at least {0} special character.", passReqCnt2.ToString()));
                //return false;
            }
            return true;
            
        }

        /// <summary>
        /// Test if password is too new to change
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="dm"></param>
        /// <returns></returns>
        private bool isTooSoonToChangePassword(string userName, DataManager dm) {
            int minPassAge = Toolkit.GetSetting("SysUserPasswordMinimumAge", 1);
            if (minPassAge < 1) return false;
            string modDate = "" + dm.ReadValue("SELECT COALESCE(modified_date, created_date) AS modified_date FROM sys_user WHERE user_name = :username OR user_name LIKE :username + '@%'"
                , new DataParameters(":username", userName, DbType.String));
            DateTime modifiedDate = DateTime.Parse(modDate);
            double passAge = (DateTime.UtcNow - modifiedDate).TotalDays;
            if (passAge < minPassAge) return true;
            return false;
        }

        /// <summary>
        /// Test if new password matches an of the stored old password hashes.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="dm"></param>
        /// <returns></returns>
        private bool isPasswordReused(string userName, string password, DataManager dm) {
            int passHistory = Toolkit.GetSetting("SysUserPasswordHistory", 24);
            if (passHistory < 1) return false;
            string[] oldHash = getOldPasswordHashes(userName, dm);
            if (oldHash == null) return false;

            for (int i = 0; i < oldHash.Length; i++) {
                if (i >= passHistory) return false;
                if (comparePasswordHash(password, oldHash[i]) || comparePasswordHash(Crypto.HashText(password), oldHash[i])) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Retreive old password hashes for a user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="dm"></param>
        /// <returns></returns>
        private static string[] getOldPasswordHashes(string userName, DataManager dm) {
            string storedHash = "" + dm.ReadValue("SELECT password FROM sys_user WHERE user_name = :username", new DataParameters(":username", userName, DbType.String));
            return storedHash.Split(':');
        }

        /// <summary>
        /// Sets the password for the given system user without requiring them to be an administrator or know the previous password.  Must be a local connection and only called from an exe (not via the web). newPassword is the plaintext password.  NOT the first-hashed password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        /// <param name="dcs"></param>
        /// <returns></returns>
        public static string SetPasswordForcefully(string userName, string newPassword, DataConnectionSpec dcs) {
            try {

                if (String.IsNullOrEmpty(dcs.ServerName)){
                    return "No servername provided.";
                } else {
                    if (dcs.ServerName.Contains("127.0.0.1") || dcs.ServerName.Contains("localhost") || dcs.ServerName.Contains(Dns.GetHostName())) {
                        using (var dm = DataManager.Create(dcs)) {
                            var sysCoopID = -1;
                            var dt = dm.Read("select cooperator_id from cooperator where last_name = 'SYSTEM'");
                            if (dt.Rows.Count < 1) {
                                sysCoopID = -1;
                            } else {
                                sysCoopID = Toolkit.ToInt32(dt.Rows[0]["cooperator_id"], -1);
                            }

                            // assumes input is completely plaintext -- not encrypted or hashed.
                            // this means we need to hash it before passing it in to setPassword()
                            // because setPassword() will hash it again then store it
                            var hashed = Crypto.HashText(newPassword);
                            setPassword(userName, hashed, sysCoopID, dm);
                        }

                        return null;
                    } else {
                        return "Forcefully setting a password requires the database connection string to include 127.0.0.1, localhost, or the same DNS name as the current computer.\r\nIn essence this functionality is available only when running this application on the same physical computer as the database engine.";
                    }
                }
            } catch (Exception ex) {
                return ex.Message;
            }
        }

        /// <summary>
        /// Does the actual update to the sys_user table for given user/password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="hashedPassword"></param>
        /// <param name="cooperatorID"></param>
        /// <param name="dm"></param>
        private static void setPassword(string userName, string hashedPassword, int cooperatorID, DataManager dm){
                    // we get here, either:
                    // a) they're updating their own password (which is ok regardless of rights)
                    // b) they do have permission to update the sys_user table

            // string doublyHashedPassword = Crypto.HashText(hashedPassword);
            string doublyHashedPassword = saltAndHash(hashedPassword);

            // Store hashes of old passwords as a colon separated list behind the new password hash
            int passHistory = Toolkit.GetSetting("SysUserPasswordHistory", 24);
            if (passHistory >= 1) {
                string[] oldHash = getOldPasswordHashes(userName, dm);
                if (oldHash != null && oldHash.Length > 0) {
                    if (oldHash.Length > passHistory) {
                        doublyHashedPassword += ":" + String.Join(":", oldHash, 0, passHistory);
                    } else {
                        doublyHashedPassword += ":" + String.Join(":", oldHash);
                    }
                    // trim
                    int fieldSize = Toolkit.ToInt32(dm.ReadValue(@"SELECT max_length FROM sys_table_field stf INNER JOIN sys_table st ON stf.sys_table_id = st.sys_table_id WHERE st.table_name = 'sys_user' AND stf.field_name = 'password'"),-1);
                    if (fieldSize > 0 && doublyHashedPassword.Length > fieldSize) {
                        doublyHashedPassword = doublyHashedPassword.Substring(0,fieldSize);
                    }
                }
            }

            dm.Write(@"
UPDATE sys_user
SET
	password = :pw,
	modified_date = :modifieddate,
	modified_by = :modifiedby
WHERE
	user_name = :username
",
                            new DataParameters(
                            new DataParameter(":pw", doublyHashedPassword),
                            new DataParameter(":modifieddate", DateTime.UtcNow, DbType.DateTime2),
                            new DataParameter(":modifiedby", cooperatorID, DbType.Int32),
                            new DataParameter(":username", userName)));

        }

        /// <summary>
        /// Generates salt and hash string given a plaintext password
        /// </summary>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        private static string saltAndHash(string password) {
            int saltSize = Toolkit.GetSetting("SysUserPasswordSaltBytes", 6);
            string salt = Crypto.SaltText(saltSize);
            //string hash = Crypto.HashText(salt + password);
            string hash = Crypto.HashTextSHA256(salt + password);
            return salt + "$" + hash;
        }


        /// <summary>
        /// Sets the password for the given web user without requiring them to be an administrator or know the previous password.  Must be a local connection and only called from an exe (not via the web). newPassword is the plaintext password.  NOT the first-hashed password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        /// <param name="dcs"></param>
        /// <returns></returns>
        public static string SetWebPasswordForcefully(string userName, string newPassword, DataConnectionSpec dcs) {
            try {

                if (String.IsNullOrEmpty(dcs.ServerName)) {
                    return "No servername provided.";
                } else {
                    if (dcs.ServerName.Contains("127.0.0.1") || dcs.ServerName.Contains("localhost") || dcs.ServerName.Contains(Dns.GetHostName())) {
                        using (var dm = DataManager.Create(dcs)) {
                            // assumes input is completely plaintext -- not encrypted or hashed.
                            // this means we need to hash it before passing it in to setWebPassword()
                            // because setWebPassword() will hash it again then store it
                            var hashed = Crypto.HashText(newPassword);
                            setWebPassword(userName, hashed, dm);
                        }

                        return null;
                    } else {
                        return "Forcefully setting a password requires the database connection string to include 127.0.0.1, localhost, or the same DNS name as the current computer.\r\nIn essence this functionality is available only when running this application on the same physical computer as the database engine.";
                    }
                }
            } catch (Exception ex) {
                return ex.Message;
            }
        }


        /// <summary>
        /// Does the actual update to the web_user table for given user/password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="hashedPassword"></param>
        /// <param name="dm"></param>
        private static void setWebPassword(string userName, string hashedPassword, DataManager dm) {
            // we get here, either:
            // a) they're updating their own password (which is ok regardless of rights)
            // b) they do have permission to update the sys_user table

            string doublyHashedPassword = Crypto.HashText(hashedPassword);

            dm.Write(@"
update
	web_user
set
	password = :pw,
	modified_date = :modifieddate
where
	user_name = :username
",
                new DataParameters(
                    new DataParameter(":pw", doublyHashedPassword),
                    new DataParameter(":modifieddate", DateTime.UtcNow, DbType.DateTime2),
                    new DataParameter(":username", userName)));

        }


        /// <summary>
        /// Updates the cooperator table for current user, pointing sys_lang_id at a different sys_lang record (language).
        /// </summary>
        /// <param name="newLanguageID"></param>
        /// <returns></returns>
        public DataSet ChangeLanguage(int newLanguageID) {
            DataSet dsReturn = createReturnDataSet();
			try {
				using (DataManager dm = BeginProcessing(true)) {
					dm.Write(@"
update
	cooperator
set
	sys_lang_id = :langid,
	modified_date = :modifieddate,
	modified_by = :modifiedby
where
	cooperator_id = :cooperatorid
", new DataParameters(
         ":langid", newLanguageID, DbType.Int32,
		 ":modifieddate", DateTime.UtcNow, DbType.DateTime2,
         ":modifiedby", CooperatorID, DbType.Int32,
         ":cooperatorid", CooperatorID, DbType.Int32
		 ));
                    LanguageID = newLanguageID;

                    _languageDirection = getLanguageDirection(LanguageID);

				}
			} catch (Exception ex) {
				if (LogException(ex, dsReturn)) {
					throw ex;
				}
			} finally {
                FinishProcessing(dsReturn, _languageID);
			}
            return dsReturn;
		}

        /// <summary>
        /// Spins through given DataSet and transfers ownership _only for records owned by the current user_ to the given cooperator.  Optionally recursively traverses down the relationship hierarchy as defined by sys_table_relationship records with type_code = 'OWNER_PARENT'.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="newOwnerCooperatorID"></param>
        /// <param name="includeDescendents"></param>
        /// <returns></returns>
        public DataSet TransferOwnership(DataSet ds, int newOwnerCooperatorID, bool includeDescendents) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = BeginProcessing(true)) {

                    dm.BeginTran();
                    // pull all cooperator id's for current user...
                    var coopIDs = new List<int>();

                    // if the "current_cooperator_id" value isn't mapped right, make sure we at least include the actual current one for the requesting user.
                    coopIDs.Add(_cooperatorID);

                    var dtCoop = dm.Read(@"
select
    cooperator_id
from
    cooperator
where
    current_cooperator_id = :coopid
", new DataParameters(":coopid", _cooperatorID, DbType.Int32));

                    foreach (DataRow drCoop in dtCoop.Rows) {
                        coopIDs.Add(Toolkit.ToInt32(drCoop["cooperator_id"], -1));
                    }

                    // for each table that maps to a dataview name, mark the newOwnerCooperatorID as the new owner
                    foreach (DataTable dt in ds.Tables) {
                        var dv = Dataview.Map(dt.TableName, _languageID, dm);
                        if (dv != null) {
                            // table is named same as a valid dataview...
                            var pkNames = dv.PrimaryKeyNames;
                            if (pkNames.Count > 0){
                                var pkName = pkNames[0];
                                DataTable dtChildTables = null;
                                if (includeDescendents) {
                                    dtChildTables = getChildTables(dv.PrimaryKeyTableName, dm);
                                }
                                assignNewOwner(dm, dt, dv.PrimaryKeyTableName, pkName, newOwnerCooperatorID, coopIDs, dtChildTables);
                            }
                        }
                    }

                    dm.Commit();

                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, _languageID);
            }
            return dsReturn;

        }

        /// <summary>
        /// Returns all tables that are specified as children based on OWNER_PARENT relationship definition in sys_table_relationship.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="dm"></param>
        /// <returns></returns>
        private DataTable getChildTables(string tableName, DataManager dm) {

            var dt = dm.Read(@"
select
    st.table_name as child_table_name,
    (select stfpk.field_name from sys_table_field stfpk where stfpk.sys_table_id = stf.sys_table_id and stfpk.is_primary_key = 'Y') as child_primary_key_name,
    stf.field_name as child_foreign_key_name,
    stf.sys_table_id as child_table_id,
    stf.sys_table_field_id as child_field_id,
    strel.relationship_type_tag,
    st2.table_name as parent_table_name,
    stf2.field_name as parent_primary_key_name,
    stf2.sys_table_id as parent_table_id,
    stf2.sys_table_field_id as parent_field_id
from
    sys_table_relationship strel
    inner join sys_table_field stf
        on strel.sys_table_field_id = stf.sys_table_field_id
    inner join sys_table st
        on stf.sys_table_id = st.sys_table_id
    inner join sys_table_field stf2
        on strel.other_table_field_id = stf2.sys_table_field_id
    inner join sys_table st2
        on stf2.sys_table_id = st2.sys_table_id
where
    st2.table_name = :parentTable
    and strel.relationship_type_tag = 'OWNER_PARENT'
",
                        new DataParameters(":parentTable", tableName, DbType.String));
            return dt;
        }

        /// <summary>
        /// Performs the actual update to transfer ownership to another cooperator.  Recursive if need be.
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        /// <param name="primaryKeyName"></param>
        /// <param name="newOwnerCooperatorID"></param>
        /// <param name="validCooperatorIDs"></param>
        /// <param name="dtChildTables"></param>
        private void assignNewOwner(DataManager dm, DataTable dt, string tableName, string primaryKeyName, int newOwnerCooperatorID, List<int> validCooperatorIDs, DataTable dtChildTables) {


            // first, assign new owner to all records in given table
            foreach (DataRow dr in dt.Rows) {

                // we issue the update but notice the where clause -- it enforces
                // not only for the row id to match, but the current owner coop id must
                // belong to the user who called this method.  Doesn't have to necessarily be their current
                // coop id, but it does have to be one of the ones that were valid at one time for them.
                dm.Write(String.Format(@"
update
    {0}
set
    owned_by = :newowner,
    owned_date = :now
where
    owned_by in (:curowner)
    and {1} = :pkvalue
", tableName, primaryKeyName), new DataParameters(
":newowner", newOwnerCooperatorID, DbType.Int32,
":now", DateTime.UtcNow, DbType.DateTime2,
":curowner", validCooperatorIDs, DbPseudoType.IntegerCollection,
":pkvalue", dr[primaryKeyName], DbType.Int32));
            }

            // then assign all children if needed...
            if (dt.Rows.Count > 0 && dtChildTables != null && dtChildTables.Rows.Count > 0) {

                // (1) look up 'children' from sys_table_relationship
                // (2) for each table, pull all child records for current id
                // (3) assign owner, recursing indefinitely

                foreach (DataRow drChildTable in dtChildTables.Rows) {

                    // by looping on the child tables first, we prevent a LOT of SQL processing since having the rows 
                    // on the outside of the child tables would result in looking up the child tables repeatedly.

                    var dtGrandChildTables = getChildTables(drChildTable["child_table_name"].ToString(), dm);

                    foreach (DataRow dr in dt.Rows) {

                        // now, each row represents a child of the parent one passed into this method.
                        // pull all pk values whose FK matches the current parent one

                        var dtChildData = dm.Read(String.Format(@"
select
    {0}
from
    {1}
where
    {2} = :id", drChildTable["child_primary_key_name"].ToString(), drChildTable["child_table_name"].ToString(), drChildTable["child_foreign_key_name"].ToString()),
               new DataParameters(":id", dr[primaryKeyName], DbType.Int32));

                        // and if there are any rows found, process them (and possibly their children)
                        if (dtChildData.Rows.Count > 0) {
                            assignNewOwner(dm, dtChildData, drChildTable["child_table_name"].ToString(), drChildTable["child_primary_key_name"].ToString(), newOwnerCooperatorID, validCooperatorIDs, dtGrandChildTables);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Clears the local cache.  Optionally requires caller to be admin.
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="mustBeAdmin"></param>
        /// <returns></returns>
        public bool ClearCache(string cacheName, bool mustBeAdmin) {
            if (mustBeAdmin) {
                checkIsAdmin();
            }
            return clearCache(cacheName);
        }

        /// <summary>
        /// Clears the local cache.  No admin check.
        /// </summary>
        /// <param name="cacheName"></param>
        /// <returns></returns>
		protected bool clearCache(string cacheName) {
            try {
				if (String.IsNullOrEmpty(cacheName)) {
					CacheManager.ClearAll();
				} else {
					CacheManager.Get(cacheName).Clear();
				}
                return true;
			} catch (Exception ex) {
                // eat all cache clearing problems
                Debug.WriteLine(ex.Message);
                return false;
			}
        }

        /// <summary>
        /// Not used, can be removed.
        /// </summary>
        /// <param name="usageData"></param>
        /// <returns></returns>
        public DataSet LogUsage(string usageData) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    foreach (string line in usageData.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
                        string[] data = line.Split('\t');

                        if (data.Length >= 3) {

                            dm.Write(@"
insert into
	sys_usage_log
(user_id, date_occurred, log_date, thread_id, message)
values
(:userid, :dateoccurred, :logdate, :threadid, :message)
", new DataParameters(
            new DataParameter(":userid", SysUserID, DbType.Int32),
             new DataParameter(":dateoccurred", Toolkit.ToDateTime(data[0], DateTime.Now.ToUniversalTime()), DbType.DateTime2),
             new DataParameter(":logdate", DateTime.Now.ToUniversalTime(), DbType.DateTime2),
             new DataParameter(":threadid", Toolkit.ToInt32(data[1], -1), DbType.Int32),
             new DataParameter(":message", String.Join("\t", data, 2, 50))));
                        }
                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, _languageID);
            }
            return dsReturn;
        }

        /// <summary>
        /// throws an exception is the user is not in the administrators group
        /// </summary>
        protected void checkIsAdmin() {
            if (!isCurrentUserAdministrator()) {
                throw Library.CreateBusinessException(getDisplayMember("checkIsAdmin", "'{0}' is requesting to do an administrative function but is not configured with administrative privileges.", this.SysUserName));
            }
        }

        /// <summary>
        /// Returns true if the user is in the administrators group, false otherwise
        /// </summary>
        /// <returns></returns>
        protected bool isCurrentUserAdministrator() {
            return this.Roles.Contains("admins");
        }

        /// <summary>
        /// Returns true if the user is in the group with the "ADMINS" tag applied to it.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="dm"></param>
        /// <returns></returns>
        protected bool isAdministrator(int userID, DataManager dm) {

            var cm = CacheManager.Get("admins");
            var isadmin = (bool?)cm[userID.ToString()];
            if (isadmin != null) {
                return (bool)isadmin;
            }

            var count = Toolkit.ToInt32(dm.ReadValue(@"
select
    count(*)
from
    sys_group sg
    inner join sys_group_user_map sgum
        on sg.sys_group_id = sgum.sys_group_id
where
    sg.group_tag = 'ADMINS'
    and sgum.sys_user_id = :userid
", new DataParameters(":userid", userID, DbType.Int32)), 0);

            var ret = count > 0;
            cm[userID.ToString()] = ret;
            return ret;

        }

        /// <summary>
        /// Returns true if EnableAdminViaWeb config setting is true and the current user is part of the "ADMINS" group.
        /// </summary>
        protected void checkUserHasAdminEnabled() {
            if (HttpContext.Current != null && !Toolkit.GetSetting("EnableAdminViaWeb", false)) {
                throw Library.CreateBusinessException(getDisplayMember("checkUserHasAdminEnabled", "Administration functionality via web services is disabled."));
            }

            checkIsAdmin();
        }

        /// <summary>
        /// Lists all tables which are related to the given dataview or table.
        /// </summary>
        /// <param name="dataviewID"></param>
        /// <param name="tableID"></param>
        /// <param name="parentTableFieldID"></param>
        /// <param name="relationshipTypes"></param>
        /// <param name="dm"></param>
        /// <param name="dsReturn"></param>
        protected void listRelatedTables(int dataviewID, int tableID, int parentTableFieldID, string[] relationshipTypes, DataManager dm, DataSet dsReturn) {
            // added memoization...
            var cm = CacheManager.Get("table");
            var cacheKey = "list_related_tables-" + dataviewID + "/" + tableID + ";" + parentTableFieldID + "|" + String.Join(":", relationshipTypes);
            var rv = cm[cacheKey] as DataTable;
            if (rv == null) {

                rv = dm.Read(@"
select
    st.sys_table_id,
    st.table_name,
    st.is_enabled,
    st.is_readonly,
    st.audits_created,
    st.audits_modified,
    st.audits_owned,
    st.database_area_code,
    st.created_date,
    st.created_by,
    st.modified_date,
    st.modified_by,
    st.owned_date,
    st.owned_by
from
    sys_table_field stf
    inner join sys_table_relationship strel
        on stf.sys_table_field_id = strel.sys_table_field_id
           and strel.relationship_type_tag IN (:reltype1)
    inner join sys_table_field stfother
        on strel.other_table_field_id = stfother.sys_table_field_id
    inner join sys_table st
        on stfother.sys_table_id = st.sys_table_id
where
    stf.sys_table_id = :tblid

UNION

select
    st.sys_table_id,
    st.table_name,
    st.is_enabled,
    st.is_readonly,
    st.audits_created,
    st.audits_modified,
    st.audits_owned,
    st.database_area_code,
    st.created_date,
    st.created_by,
    st.modified_date,
    st.modified_by,
    st.owned_date,
    st.owned_by
from
    sys_dataview sd inner join sys_dataview_field sdf
        on sd.sys_dataview_id = sdf.sys_dataview_id
    inner join sys_table_field stf
        on sdf.sys_table_field_id = stf.sys_table_field_id
    inner join sys_table_relationship strel
        on stf.sys_table_field_id = strel.sys_table_field_id
           and strel.relationship_type_tag IN (:reltype2)
    inner join sys_table_field stfother
        on strel.other_table_field_id = stfother.sys_table_field_id
    inner join sys_table st
        on stfother.sys_table_id = st.sys_table_id
where
    sd.sys_dataview_id = :dvid

UNION

select
    st.sys_table_id,
    st.table_name,
    st.is_enabled,
    st.is_readonly,
    st.audits_created,
    st.audits_modified,
    st.audits_owned,
    st.database_area_code,
    st.created_date,
    st.created_by,
    st.modified_date,
    st.modified_by,
    st.owned_date,
    st.owned_by
from
    sys_table st inner join sys_table_field stf
        on st.sys_table_id = stf.sys_table_id
where
    stf.sys_table_field_id = :tblfid
order by
    table_name
", "list_related_tables", new DataParameters(":reltype1", relationshipTypes, DbPseudoType.StringCollection, ":tblid", tableID, DbType.Int32, ":reltype2", relationshipTypes, DbPseudoType.StringCollection, ":dvid", dataviewID, DbType.Int32, ":tblfid", parentTableFieldID, DbType.Int32));

                rv.DataSet.Tables.Remove(rv);
                cm[cacheKey] = rv;

            }

            dsReturn.Tables.Add(rv.Copy());

        }

        /// <summary>
        /// Returns the first table marked as either OWNER_PARENT or PARENT for the given tableName depending on mustBeOwnerParent parameter.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="mustBeOwnerParent"></param>
        /// <param name="foreignKeyField"></param>
        /// <param name="dm"></param>
        /// <returns></returns>
        private string getParentTable(string tableName, bool mustBeOwnerParent, ref string foreignKeyField, DataManager dm) {
            if (String.IsNullOrEmpty(tableName)) {
                return null;
            }

            var dt = dm.Read(@"
select
    stparent.table_name as parent_table_name,
    stfparent.field_name as parent_primary_key_field,
    stf.field_name as foreign_key_field
from
    sys_table stchild
    inner join sys_table_field stf
        on stchild.sys_table_id = stf.sys_table_id
    inner join sys_table_relationship strel
        on stf.sys_table_field_id = strel.sys_table_field_id
           and strel.relationship_type_tag IN (:reltype1)
    inner join sys_table_field stfparent
        on strel.other_table_field_id = stfparent.sys_table_field_id
    inner join sys_table stparent
        on stfparent.sys_table_id = stparent.sys_table_id
where
    stchild.table_name = :tablename
", new DataParameters(":reltype1", (mustBeOwnerParent ? "OWNER_PARENT" : "PARENT"), DbType.String,
     ":tablename", tableName, DbType.String));

            if (dt.Rows.Count > 0) {
                foreignKeyField = dt.Rows[0]["foreign_key_field"].ToString();
                return dt.Rows[0]["parent_table_name"].ToString();
            }

            return null;
        }

        /// <summary>
        /// Returns the effective permissions for the given system user id (sys_user_id) and dataview / table.  
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="dataviewName"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public Permission[] GetEffectivePermissions(int userID, string dataviewName, string tableName) {
            try {
                using (DataManager dm = BeginProcessing(true)) {
                    // make sure user has proper permissions
                    // checkUserHasAdminEnabled();

                    var perms = getPermissions(dm, userID, tableName, dataviewName, null, null);
                    return perms;
                }
            } catch (Exception ex) {
                if (LogException(ex, null)) {
                    throw ex;
                } else {
                    return null;
                }
            }
        }




        #endregion Miscellaneous

        #region Search
        /// <summary>
        /// Calls out to the search engine (via WCF or custom TCP class) to tell it to perform realtime updates to one or more of its indexes.
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="rows"></param>
        /// <param name="bindingType"></param>
        /// <param name="bindingUrl"></param>
        internal void UpdateSearchIndex(ITable fmt, List<GrinGlobal.Interface.UpdateRow> rows, string bindingType, string bindingUrl) {
            try {
                BeginProcessing(false);

                try {
                    using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                        c.UpdateIndex(fmt.TableName, rows);
                    }
                } catch (Exception ex) {
                    if (LogException(ex, null)) {
                        throw ex;
                    }
                }

            } catch (Exception ex) {
                if (LogException(ex, null, false)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(null, LanguageID);
            }
        }

        /// <summary>
        /// Calls out to the search engine (via WCF or custom TCP class) to test connectivity only
        /// </summary>
        /// <returns></returns>
        public bool PingSearchEngine() {
            using(var c = new ClientSearchEngineRequest(null, null)){
                return c.Ping();
            }
        }

        /// <summary>
        /// Calls to the search engine (via WCF or custom TCP class) to run the given query in the specified indexes, resolving to the given resolver.  Then takes outputted IDs from the search engine and passes them to the given dataviewName, and returns the results from the GetData() call for that dataview.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ignoreCase"></param>
        /// <param name="autoAndConsecutiveLiterals"></param>
        /// <param name="indexList"></param>
        /// <param name="resolverName"></param>
        /// <param name="searchOffset"></param>
        /// <param name="searchLimit"></param>
        /// <param name="databaseOffset"></param>
        /// <param name="databaseLimit"></param>
        /// <param name="dataviewName"></param>
        /// <param name="options"></param>
        /// <param name="bindingType"></param>
        /// <param name="bindingUrl"></param>
        /// <returns></returns>
        public DataSet Search(string query, bool ignoreCase, bool autoAndConsecutiveLiterals, string indexList, string resolverName, int searchOffset, int searchLimit, int databaseOffset, int databaseLimit, string dataviewName, string options, string bindingType, string bindingUrl) {

            DataSet dsReturn = createReturnDataSet();
            try {

                //BeginProcessing(false);
                DataManager dm = BeginProcessing(true);

                using (var sr = new SearchRequest(this, dm))
                {
//DataSet ds = new DataSet();
//ds = sr.FindPKeys(resolverName + "_id", query, autoAndConsecutiveLiterals?"AND":"OR");
//// If the search engine was successful, a SearchResult table should be return - so add it to this method's return dataset...
//if (ds != null &&
//    ds.Tables.Count > 0 &&
//    ds.Tables.Contains("SearchResult"))
//{
//    dsReturn.Tables.Add(ds.Tables["SearchResult"].Copy());
//}

                    // KFE
                    string seType = "";
                    if (Regex.Match(options, @"SearchEngineType\s*=\s*'dynamic'", RegexOptions.IgnoreCase).Success) {
                        seType = "dynamic";
                    } else if (Regex.Match(options, @"SearchEngineType\s*=\s*'static'", RegexOptions.IgnoreCase).Success) {
                        seType = "static";
                    }

                    if (Regex.Match(options, @"OrMultipleLines\s*=\s*true", RegexOptions.IgnoreCase).Success)
                    {
                        dsReturn = sr.FindPKeys(resolverName + "_id", query, "LIST", seType, searchLimit);
                    }
                    else
                    {
                        dsReturn = sr.FindPKeys(resolverName + "_id", query, autoAndConsecutiveLiterals ? "AND" : "OR", seType, searchLimit);
                    }
                    
                    // If the search engine was successful, a SearchResult table should be return - so add it to this method's return dataset...
                    if (!String.IsNullOrEmpty(dataviewName) &&
                        dsReturn != null &&
                        dsReturn.Tables.Count > 0 &&
                        dsReturn.Tables.Contains("SearchResult"))
                    {
                        dsReturn = GetData(dataviewName, ":idlist=" + Toolkit.Join(dsReturn.Tables["SearchResult"], "id", ",", ""), databaseOffset, databaseLimit, options);
                    }
                }

//// Commented out to try new search engine...
//                if (!String.IsNullOrEmpty(resolverName)) {
//                    resolverName = resolverName.ToLower().Replace("accessions", "accession").Replace("orders", "order_request");
//                }

//                // intercept the languageid option, change it to the current user language id if needed
//                var opts = Toolkit.ParsePairs<string>(("" + options).ToLower());
//                var lang = string.Empty;
//                if (opts.TryGetValue("languageid", out lang)) {
//                    var langID = Toolkit.ToInt32(lang, -1);
//                    if (langID < 0) {
//                        opts["languageid"] = LanguageID.ToString();
//                    }
//                } else {
//                    opts["languageid"] = LanguageID.ToString();
//                }
//                options = Toolkit.ConcatPairs(opts);

//                var httpCtx = HttpContext.Current;

//                List<string> indexNames = (indexList == null ? new List<string>() : indexList.Split(new string[] { ",", " ", ";" }, true).ToList());
//                //// go get the results from the search engine

//                using (var c = GetSearchRequest(bindingType, bindingUrl)) {

//                    //if (httpCtx != null) {
//                    //    // perform an async call here so we can poll the IsClientConnected property
//                    //    // this lets us cancel the query to the search engine if the user disconnects from the website.
//                    //    var iasync = client.BeginSearch(query, ignoreCase, autoAndConsecutiveLiterals, indexNames, resolverName, searchOffset, searchLimit, new AsyncCallback(asyncCallbackForSearch), null);
//                    //    bool done = false;
//                    //    try {
//                    //        while (!iasync.IsCompleted && !done) {
//                    //            httpCtx.Response.Write(' ');
//                    //            httpCtx.Response.Flush();
//                    //            if (!httpCtx.Response.IsClientConnected) {
//                    //                // user disconnected from website, let's disconnect from search query
//                    //                done = true;
//                    //                client.Abort();
//                    //            }
//                    //        }
//                    //        if (!done) {
//                    //            // thread completed successfully.
//                    //            result = client.EndSearch(iasync);
//                    //        }
//                    //    } catch (Exception ex1) {
//                    //        Debug.WriteLine(ex1.Message);
//                    //    }
//                    //} else {
//                    var inputXml = Toolkit.DataSetToXml(dsReturn);
//                    dsReturn = c.Search(query, ignoreCase, autoAndConsecutiveLiterals, indexNames, resolverName, searchOffset, searchLimit, inputXml, options);
//                }

//                if (!String.IsNullOrEmpty(dataviewName)) {
//                    dsReturn = GetData(dataviewName, ":idlist=" + Toolkit.Join(dsReturn.Tables["SearchResult"], "id", ",", ""), databaseOffset, databaseLimit, options);
//                }

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw ex;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        /// <summary>
        /// Unused, but stub must be here
        /// </summary>
        /// <param name="res"></param>
        private void asyncCallbackForSearch(IAsyncResult res) {
            // not used, but necessary
        }

        //public List<ResolvedHitData> SearchForHits(string query, bool ignoreCase, bool autoAndConsecutiveLiterals, string indexList, string resolverName, bool returnHitsWithNoResolvedIDs, int searchOffset, int searchLimit, string bindingType, string bindingUrl) {

        //    SearchSvc.SearchHostClient client = null;
        //    DataSet dsReturn = createReturnDataSet();
        //    List<ResolvedHitData> result = null;
        //    try {

        //        BeginProcessing(false);

        //        List<string> indexNames = (indexList == null ? new List<string>() : indexList.Split(new string[] { ",", " " }, true).ToList());
        //        client = GetSearchHost(bindingType, bindingUrl);
        //        try {
        //            //// go get the results from the search engine
        //            result = client.SearchForHits(query, ignoreCase, autoAndConsecutiveLiterals, indexNames, resolverName, returnHitsWithNoResolvedIDs, searchOffset, searchLimit);
        //        } catch (Exception ex) {
        //            if (LogException(ex, dsReturn)) {
        //                throw ex;
        //            }
        //        }
        //    } catch (Exception ex) {
        //        if (LogException(ex, dsReturn)) {
        //            throw ex;
        //        }
        //    } finally {
        //        CloseSearchHost(client);
        //        FinishProcessing(dsReturn, LanguageID);
        //    }

        //    return result;


        //}

        /// <summary>
        /// Initializes a new instance of the search engine request class (for communicating to the search engine service)
        /// </summary>
        /// <param name="bindingType"></param>
        /// <param name="bindingUrl"></param>
        /// <returns></returns>
        protected ClientSearchEngineRequest GetSearchRequest(string bindingType, string bindingUrl) {
            var c = new ClientSearchEngineRequest(bindingType, bindingUrl);
            return c;
        }

        /// <summary>
        /// Creates the UpdateRow that will eventually be passed to the search engine for realtime update purposes
        /// </summary>
        /// <param name="fmt"></param>
        /// <param name="mode"></param>
        /// <param name="dr"></param>
        /// <param name="primaryKeyID"></param>
        /// <returns></returns>
        internal GrinGlobal.Interface.UpdateRow QueueSearchUpdate(ITable fmt, SaveMode mode, DataRow dr, int primaryKeyID) {
            GrinGlobal.Interface.UpdateRow row = new GrinGlobal.Interface.UpdateRow();
            row.Values = new List<GrinGlobal.Interface.FieldValue>();
            row.ID = primaryKeyID;
            for (int i = 0; i < dr.Table.Columns.Count; i++) {
                var fv = new GrinGlobal.Interface.FieldValue { FieldName = dr.Table.Columns[i].ColumnName };
                switch (mode) {
                    case SaveMode.Insert:
                        // don't try to pull original values on a new record, that will cause an error
                        fv.NewValue = dr[i, DataRowVersion.Current];
                        if (fmt.PrimaryKeyFieldName.ToLower() == fv.FieldName.ToLower()) {
                            fv.NewValue = primaryKeyID;
                        }
                        row.Mode = GrinGlobal.Interface.UpdateMode.Add;
                        break;
                    case SaveMode.Update:
                        if (dr.RowState == DataRowState.Modified) {
                            fv.OriginalValue = dr[i, DataRowVersion.Original];
                        } else {
                            fv.OriginalValue = DBNull.Value;
                        }
                        fv.NewValue = dr[i, DataRowVersion.Current];

                        if (fmt.PrimaryKeyFieldName.ToLower() == fv.FieldName.ToLower()) {
                            fv.OriginalValue = primaryKeyID;
                            fv.NewValue = primaryKeyID;
                        }
                        row.Mode = GrinGlobal.Interface.UpdateMode.Replace;
                        break;
                    case SaveMode.Delete:
                        // don't try to pull current values on a deleted record, that will cause an error.
                        fv.OriginalValue = dr[i, DataRowVersion.Original];
                        if (fmt.PrimaryKeyFieldName.ToLower() == fv.FieldName.ToLower()) {
                            fv.OriginalValue = primaryKeyID;
                            fv.NewValue = null;
                        }
                        row.Mode = GrinGlobal.Interface.UpdateMode.Subtract;
                        break;
                }
                row.Values.Add(fv);
            }
            return row;
        }

        /// <summary>
        /// Calls to the search engine to get basic configuration information
        /// </summary>
        /// <param name="enabledIndexesOnly"></param>
        /// <param name="bindingType"></param>
        /// <param name="bindingUrl"></param>
        /// <returns></returns>
        public DataSet GetSearchEngineInfo(bool enabledIndexesOnly, string bindingType, string bindingUrl) {
            //// go get the results from the search engine
            using (var c = GetSearchRequest(bindingType, bindingUrl)) {
                return c.GetInfo(enabledIndexesOnly);
            }
        }


        #endregion Search

        #region File
        /// <summary>
        /// Gets file information about all files in the sys_file_group table for the given groupID.  Used by Admin Tool.
        /// </summary>
        /// <param name="groupID"></param>
        /// <param name="onlyLatest"></param>
        /// <returns></returns>
        public DataSet GetFileInfo(int groupID, bool onlyLatest) {

            string groupName = null;
            string versionName = null;

            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = this.BeginProcessing(true)) {

                    var dt = dm.Read(@"
select
    group_name,
    version_name
from
    sys_file_group
where
    sys_file_group_id = :groupid
", new DataParameters(":groupid", groupID, DbType.Int32));

                    if (dt != null && dt.Rows.Count > 0) {
                        groupName = dt.Rows[0]["group_name"].ToString();
                        versionName = dt.Rows[0]["version_name"].ToString();
                    }
                }


                dsReturn = GetFileInfo(groupName, versionName, false, onlyLatest);

            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;

        }

        /// <summary>
        /// Gets file information for all files in the sys_file_group table for the given group name / version / etc.  Used by Updater.
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="groupVersionName"></param>
        /// <param name="onlyAvailable"></param>
        /// <param name="onlyLatest"></param>
        /// <returns></returns>
        public DataSet GetFileInfo(string groupName, string groupVersionName, bool onlyAvailable, bool onlyLatest) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = this.BeginProcessing(true)) {

                    // grabbing only latest means get only the first row from sys_file_group
                    dm.Limit = onlyLatest ? 1 : 0;

                    dm.Read(@"
select
    sfg.version_name,
    sfg.group_name,
    sfg.sys_file_group_id,
    sfg.is_enabled,
    coalesce(sfg.modified_date, sfg.created_date) as last_touched_date,
    coalesce(su_m.sys_user_id, su_c.sys_user_id) as last_touched_by_id,
    coalesce(su_m.user_name, su_c.user_name) as last_touched_by
from
    sys_file_group sfg 
    left join sys_user su_m
        on sfg.modified_by = su_m.cooperator_id
    left join sys_user su_c
        on sfg.created_by = su_c.cooperator_id
where
    sfg.group_name = coalesce(:groupname, sfg.group_name)
    and sfg.version_name = coalesce(:versionname, sfg.version_name)
    and coalesce(:enabled, sfg.is_enabled, '') = coalesce(sfg.is_enabled, '')
order by
    1 desc,
    2
", dsReturn, "file_group_info", new DataParameters(
     ":groupname", groupName,
     ":versionname", groupVersionName,
     ":enabled", (onlyAvailable ? "Y" : null)));


                    int? groupID = null;

                    // if they specified only the latest, restrict by groupid as well...
                    if (onlyLatest && dsReturn.Tables["file_group_info"].Rows.Count > 0) {
                        groupID = Toolkit.ToInt32(dsReturn.Tables["file_group_info"].Rows[0]["sys_file_group_id"], -1);
                    }

                    // we always grab all files for the given group(s)
                    dm.Limit = 0;

                    dm.Read(@"
select
    sfg.version_name,
    sfg.group_name,
    sfl.title,
    sfg.sys_file_group_id,
    sfg.is_enabled as group_is_enabled,
    sf.sys_file_id
      ,sf.is_enabled
      ,sf.virtual_file_path
      ,sf.file_name
      ,sf.file_version
      ,sf.file_size
      ,sf.display_name
      ,sf.created_date
      ,sf.created_by
      ,sf.modified_date
      ,sf.modified_by
      ,sf.owned_date
      ,sf.owned_by,
    coalesce(sf.modified_date, sf.created_date) as last_touched_date,
    coalesce(su_m.sys_user_id, su_c.sys_user_id) as last_touched_by_id,
    coalesce(su_m.user_name, su_c.user_name) as last_touched_by
from
    sys_file sf left join sys_file_group_map sfgm
        on sf.sys_file_id = sfgm.sys_file_id
    left join sys_file_group sfg 
        on sfgm.sys_file_group_id = sfg.sys_file_group_id
    left join sys_user su_m
        on sf.modified_by = su_m.cooperator_id
    left join sys_user su_c
        on sf.created_by = su_c.cooperator_id
    left join sys_file_lang sfl
        on sf.sys_file_id = sfl.sys_file_id
where
    coalesce(sfg.group_name, '') = coalesce(:groupname, sfg.group_name, '')
    and coalesce(sfg.version_name,'') = coalesce(:versionname, sfg.version_name, '')
    and coalesce(:fileavailable, sf.is_enabled, '') = coalesce(sf.is_enabled, '')
    and coalesce(:groupavailable, sfg.is_enabled, '') = coalesce(sfg.is_enabled, '')
    and coalesce(sfg.sys_file_group_id, -1) = coalesce(:groupid, sfg.sys_file_group_id, -1)
order by
    1 desc,
    2,
    3
", dsReturn, "file_info", new DataParameters(
     ":groupname", groupName,
     ":versionname", groupVersionName,
     ":fileavailable", (onlyAvailable ? "Y" : null),
     ":groupavailable", (onlyAvailable ? "Y" : null),
     ":groupid", groupID, DbType.Int32
     ));


                    // we always overwrite version and date info with actual file data...
                    var dt = dsReturn.Tables["file_info"];
                    foreach (DataRow dr in dt.Rows) {
                        var dtInfo = GetDirectoryOrFileInfo(dr["virtual_file_path"].ToString()).Tables["dir_file_info"];
                        foreach (DataRow dr2 in dtInfo.Rows) {
                            dr["file_version"] = dr2["version"];
                            dr["file_size"] = dr2["size"];
                        }
                    }


                    //dsReturn.Relations.Add(new DataRelation("fk_group_to_file", dsReturn.Tables["file_group_info"].Columns["sys_file_group_id"], dsReturn.Tables["file_info"].Columns["sys_file_group_id"]));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;
        }

        /// <summary>
        /// Gets file information from sys_file table for the given fileID.  Used by Admin Tool.
        /// </summary>
        /// <param name="fileID"></param>
        /// <returns></returns>
        public DataSet GetFileInfo(int fileID) {
            DataSet dsReturn = createReturnDataSet();
            try {

                using (DataManager dm = this.BeginProcessing(true)) {

                    dm.Read(@"
select
    sys_file_id
      ,is_enabled
      ,virtual_file_path
      ,file_name
      ,file_version
      ,file_size
      ,display_name
      ,created_date
      ,created_by
      ,modified_date
      ,modified_by
      ,owned_date
      ,owned_by
from
    sys_file
where
    sys_file_id = :fileid
", dsReturn, "file_info", new DataParameters(":fileid", fileID, DbType.Int32));

                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }

            return dsReturn;

        }

        /// <summary>
        /// Gets information about the given virtual path.  Looks up data from the sys_file table as well as the file system itself.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public DataSet GetDirectoryOrFileInfo(string virtualPath) {
            DataSet dsReturn = createReturnDataSet();
            try {
                using (var dm = BeginProcessing(true)) {

                    DataTable dt = new DataTable("dir_file_info");
                    dt.Columns.Add("type", typeof(string));
                    dt.Columns.Add("virtualpath", typeof(string));
                    dt.Columns.Add("extension", typeof(string));
                    dt.Columns.Add("size", typeof(long));
                    dt.Columns.Add("created", typeof(DateTime));
                    dt.Columns.Add("modified", typeof(DateTime));
                    dt.Columns.Add("version", typeof(string));
                    dt.Columns.Add("ismapped", typeof(bool));
                    dt.Columns.Add("subfoldercount", typeof(int));
                    dt.Columns.Add("filecount", typeof(int));

                    dsReturn.Tables.Add(dt);

                    var dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    dr["virtualpath"] = virtualPath;

                    dr["ismapped"] = Toolkit.ToInt32(dm.ReadValue("select count(*) from sys_file where virtual_file_path = :virt", new DataParameters(":virt", virtualPath)), 0) > 0;

                    string physPath = null;

                    try {
                        if (HttpContext.Current != null && HttpContext.Current.Server != null) {
                            physPath = HttpContext.Current.Server.MapPath(virtualPath);
                        } else {
                            physPath = Toolkit.GetIISPhysicalPath("gringlobal");
                        }
                    } catch {
                        // eat all errors here, we're using this primarily for context detection
                    }
                    if (File.Exists(physPath) || Directory.Exists(physPath)) {
                        var fa = File.GetAttributes(physPath);
                        if ((fa & FileAttributes.Directory) == FileAttributes.Directory) {
                            // is a directory
                            var di = new DirectoryInfo(physPath);
                            dr["type"] = "directory";
                            dr["extension"] = "";
                            dr["size"] = 0;
                            dr["created"] = di.CreationTimeUtc;
                            dr["modified"] = di.LastWriteTimeUtc;
                            dr["version"] = "";
                            dr["subfoldercount"] = Directory.GetDirectories(physPath).Length;
                            dr["filecount"] = Directory.GetFiles(physPath).Length;
                        } else {
                            // is a file
                            dr["type"] = "file";
                            var fi = new FileInfo(physPath);
                            dr["extension"] = fi.Extension;
                            dr["size"] = fi.Length;
                            dr["created"] = fi.CreationTimeUtc;
                            dr["modified"] = fi.LastWriteTimeUtc;
                            dr["version"] = getDownloadFileInfo(fi, virtualPath, dm);
                            dr["subfoldercount"] = 0;
                            dr["filecount"] = 0;
                        }
                    }
                }
            } catch (Exception ex) {
                if (LogException(ex, dsReturn)) {
                    throw;
                }
            } finally {
                FinishProcessing(dsReturn, LanguageID);
            }
            return dsReturn;

        }

        /// <summary>
        /// Gets information needed to determine if the file needs to be downloaded.  Auto-generates .version files as needed.  Inspects .msi files to get ProductVersion instead of just the FileVersion attribute.  Used by Updater.
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="virtualPath"></param>
        /// <param name="dm"></param>
        /// <returns></returns>
        protected string getDownloadFileInfo(FileInfo fi, string virtualPath, DataManager dm) {

            var version = "";
            var writeNewVersionInfo = false;
            var versionFile = fi.FullName + ".version";
            if (File.Exists(versionFile)) {
                var fi2 = new FileInfo(versionFile);
                if (fi.CreationTimeUtc > fi2.CreationTimeUtc || fi.LastWriteTimeUtc > fi2.LastWriteTimeUtc) {
                    // the 'version' file is outdated because it is older than the file we're keeping track of.  needs to be rewritten.
                    writeNewVersionInfo = true;
                    try {
                        File.Delete(versionFile);
                    } catch {
                    }
                } else {
                    version = File.ReadAllText(versionFile) + "";
                }
            } else {
                if (".exe.cab.msi".Contains(fi.Extension.ToLower())) {
                    // we write a version file only for files that take a long time to calculate their version info or take a long time to download
                    writeNewVersionInfo = true;
                }
            }

            if (String.IsNullOrEmpty(version)) {
                // we get here if no version file exists or the version file was outdated and needs to be recreated
                if (fi.Extension == ".msi") {
                    version = getProductVersion(fi.FullName);
                } else {
                    FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(fi.FullName);
                    version = fvi.ProductVersion ?? fi.CreationTimeUtc.Year.ToString().PadLeft(4, '0') + "." +
                        fi.CreationTimeUtc.Month.ToString().PadLeft(2, '0') + fi.CreationTimeUtc.Day.ToString().PadLeft(2, '0') + "." +
                        fi.CreationTimeUtc.Hour.ToString().PadLeft(2, '0') + fi.CreationTimeUtc.Minute.ToString().PadLeft(2, '0');
                }
            }

            if (writeNewVersionInfo) {
                // if we're pulling from a CD or other read-only folder, the writing the version file may fail.  Silently eat this and continue
                // as not caching version info in that case is ok since it's only a single user environment
                try {

                    // be sure to update the database table as well
                    dm.Write(@"
update sys_file
set
   file_version = :fileversion,
   file_size = :filesize,
   modified_date = :now,
  modified_by = :coop
where
   virtual_file_path = :vpath
", new DataParameters(
     ":fileversion", version,
     ":filesize", fi.Length, DbType.Decimal,
     ":vpath", virtualPath,
     ":now", DateTime.UtcNow, DbType.DateTime2,
     ":coop", CooperatorID, DbType.Int32));


                    File.WriteAllText(versionFile, version);
                } catch {
                }
            }
            return version.Trim();
        }

        /// <summary>
        /// Performs the actual retrieval of the product version information from a .msi file.  Also calls GC.Collect() due to how the WindowsInstaller COM object behaves.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected string getProductVersion(string fileName) {
            try {
                Type type = Type.GetTypeFromProgID("WindowsInstaller.Installer");
                WindowsInstaller.Installer installer = null;
                WindowsInstaller.Database db = null;
                WindowsInstaller.View dv = null;
                try {
                    installer = (WindowsInstaller.Installer)Activator.CreateInstance(type);
                    db = installer.OpenDatabase(fileName, 0);
                    dv = db.OpenView("SELECT `Value` FROM `Property` WHERE `Property`='ProductVersion'");
                    WindowsInstaller.Record record = null;
                    dv.Execute(record);
                    record = dv.Fetch();
                    string productVersion = record.get_StringData(1).ToString();
                    return productVersion;
                } finally {

                    // since the installer objects are COM objects,
                    // we need to be extra careful to clean up after ourselves

                    if (db != null) {
                        Marshal.FinalReleaseComObject(db);
                        db = null;
                    }
                    if (dv != null) {
                        dv.Close();
                        Marshal.FinalReleaseComObject(dv);
                        dv = null;
                    }

                    // DO NOT REMOVE THIS!!!
                    // the OpenDatabase call leaves a file descriptor open against the .msi file.
                    // The only way I could find to clean this up properly was to force garbage collection
                    // since there is no Close() or Dispose() or CloseDatabase() or whatever in the installer API.
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            } catch (Exception) {
                // couldn't deduce the 'real' version, just use the file date/time
                return new FileInfo(fileName).CreationTimeUtc.ToString("yyyy.MM.dd");
            }
        }



        #endregion File
        #region Image

        /// <summary>
        /// returns true if url is not absolute, root relative, or app relative.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool isDocumentRelativeUrl(string url) {
            return !isAbsoluteUrl(url) && !isRootRelativeUrl(url) && !isAppRelativeUrl(url);
        }

        /// <summary>
        ///  returns true if the url starts with http:, https:, \\, *: (2nd char is colon)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool isAbsoluteUrl(string url) {
            // absolute means it starts with:
            //    http:
            //    https:
            //    \\
            //    *:      (2nd char is colon)


            url = url.ToLower();
            return url.StartsWith("http:") || url.StartsWith("https:") || url.StartsWith(@"\\") || Toolkit.Cut(url, 1) == ":";
        }

        /// <summary>
        ///  returns true if url starts with "/"
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool isRootRelativeUrl(string url) {
            return url.ToLower().StartsWith("/");
        }

        /// <summary>
        /// Returns true if url starts with "~/"
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private bool isAppRelativeUrl(string url) {
            return url.ToLower().StartsWith("~/");
        }

        /// <summary>
        /// Deletes a file from the ~/uploads/images directory of the website.  No security checks are currently performed.
        /// </summary>
        /// <param name="imageFileName"></param>
        /// <param name="isThumbnail"></param>
        /// <returns></returns>
        public bool DeleteImage(string imageFileName) {

            string rootVPath = "~/uploads/images/";

            if (isRootRelativeUrl(imageFileName) || isAbsoluteUrl(imageFileName) || (isAppRelativeUrl(imageFileName) && !imageFileName.ToLower().StartsWith(rootVPath))) {
                throw Library.CreateBusinessException(getDisplayMember("DeleteImage", "Only document-relative file names (image.jpg or path/to/image.jpg) and app-relative file names (" + rootVPath + ") are valid for deleting via this method.  Absolute file names (http://servername/path/to/image.jpg) or root-relative file names (/path/to/image.jpg) are not allowed.  Also, app-relative file names that do not begin with '{0}' are also not allowed.", rootVPath));
            }

            string vpath = rootVPath + imageFileName;
            string path = null;
            if (HttpContext.Current == null) {
                path = Toolkit.ResolveFilePath(vpath, false);
            } else {
                path = HttpContext.Current.Server.MapPath(vpath);
            }
            if (File.Exists(path)) {
                File.Delete(path);
                return true;
            } else {
                return false;
            }
        }

        /// <summary>
        /// Unused but necessary for GetThumbnailImage() call to work properly
        /// </summary>
        /// <returns></returns>
        private bool dummyThumbnailCallback() {
            // do not delete, required for GetThumbnailImage() call
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the app-relative path to the new file.  Optionally auto-creates corresponding thumbnail image.  Note filename may be different than given if that file name already exists and overwrite is false.
        /// </summary>
        /// <param name="imageFileName"></param>
        /// <param name="imageBytes"></param>
        /// <param name="isThumbnail"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public string SaveImage(string imageFileName, byte[] imageBytes, bool createThumbnail, bool overwrite) {

            string newVPath = "~/uploads/images/";
            string path = null;

            // make sure it's not in windows-style path separators
            imageFileName = imageFileName.Replace(@"\", "/");

            // make doc-relative into app-relative
            if (isDocumentRelativeUrl(imageFileName)) {
                imageFileName = newVPath + imageFileName;
            }

            // ... and if it's not absolute, force it to be app-relative
            if (imageFileName.StartsWith("/")) {
                imageFileName = "~" + imageFileName;
            }

            // make sure it's not root-relative -- if it is, coerce it into app-relative
            if (imageFileName.StartsWith("~/")) {
                if (!imageFileName.StartsWith(newVPath)) {
                    imageFileName = imageFileName.Replace("~/", newVPath);
                }
            }


            if (isRootRelativeUrl(imageFileName) || isAbsoluteUrl(imageFileName) || (isAppRelativeUrl(imageFileName) && !imageFileName.ToLower().StartsWith(newVPath))) {
                throw Library.CreateBusinessException(getDisplayMember("SaveImage", "Only document-relative file names (image.jpg or path/to/image.jpg) and app-relative file names (" + newVPath + ") are valid for saving via this method.  Absolute file names (http://servername/path/to/image.jpg) or root-relative file names (/path/to/image.jpg) are not allowed.  Also, app-relative file names that do not begin with '{0}' are not allowed.", newVPath));
            } else {

                //if (!imageFileName.StartsWith("~/")) {
                //    imageFileName = newVPath + imageFileName;
                //}

                if (HttpContext.Current == null) {
                    path = imageFileName;
                } else {
                    path = HttpContext.Current.Server.MapPath(imageFileName);
                }

                // create directories recursively if needed
                path = Toolkit.ResolveFilePath(path, true);

                // change name until that file doesn't exist
                int i = 1;
                string ext = new FileInfo(path).Extension;
                string prevExt = ext;
                while (File.Exists(path) && !overwrite) {
                    string newExt = "_" + i + ext;
                    path = path.Replace(prevExt, newExt);
                    prevExt = newExt;
                    i++;
                }
                File.WriteAllBytes(path, imageBytes);

                if (createThumbnail) {
                    Image img = Image.FromStream(new MemoryStream(imageBytes, false));

                    // remember format before we start monkeying with it
                    ImageFormat fmt = img.RawFormat;


                    if (img.Width < 100 || img.Height < 100) {
                        // for 'small' images, don't scruch down to unusably tiny images.
                        img.Save(path.Replace(ext, "_thumbnail" + ext), fmt);
                    } else {

                        // HACK: To force ignore EXIF thumbnail, essentially rotate image 360 deg so it's a bitmap (drops EXIF info)
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);

                        // generate a new img that is 1/10th the size of the original (so we maintain aspect ratio)
                        Image thumbnail = img.GetThumbnailImage(img.Width / 10, img.Height / 10, new Image.GetThumbnailImageAbort(dummyThumbnailCallback), IntPtr.Zero);

                        //ImageFormat fmt = ImageFormat.Jpeg;
                        //ext = ext.ToLower();
                        //if (ext == ".gif") {
                        //    fmt = ImageFormat.Gif;
                        //} else if (ext == ".jpg" || ext == ".jpeg") {
                        //    fmt = ImageFormat.Jpeg;
                        //} else if (ext == ".png") {
                        //    fmt = ImageFormat.Png;
                        //} else if (ext == ".tiff" || ext == ".tif") {
                        //    fmt = ImageFormat.Tiff;
                        //}

                        thumbnail.Save(path.Replace(ext, "_thumbnail" + ext), fmt);
                    }
                }

                // chop off the physical file path part, replace with our vpath path, make sure we return web-style path separators
                string imagesPath = Toolkit.ResolveDirectoryPath(newVPath, false);
                string ret = path.Replace(imagesPath, newVPath).Replace(@"\", "/");
                return ret;
            }

        }

        /// <summary>
        /// Returns a byte array consisting of the actual data for the given file.  Path can be absolute, app-relative, or doc relative.  If absolute and the file is on a different server, performs a web request to retrieve it.
        /// </summary>
        /// <param name="appOrDocRelativePath"></param>
        /// <returns></returns>
        public byte[] GetImage(string appOrDocRelativePath) {
            byte[] ret = null;
            try {

                if (String.IsNullOrEmpty(appOrDocRelativePath)) {
                    // image not found.  return null.
                    ret = null;
                } else {
                    // this is the vpath to the image.

                    string path = appOrDocRelativePath;

                    if (isAbsoluteUrl(appOrDocRelativePath)) {

                        // TODO: cache image?  use same pathing from orig source, just tack on ~/uploads/images/ in front of it???


                        // if absolute, do a web request to retrieve it

                        List<byte> bytes = new List<byte>();
                        var req = WebRequest.Create(appOrDocRelativePath);
                        
                        // TODO: http proxy... we need to initialize it here.  But that code is in GrinGlobal.InstallHelper.  Add a reference to it from SecureData?  Doesn't seem like a good idea but I can't put my finger on it as to why
                        // Utility.InitProxySettings(req);

                        using (var resp = req.GetResponse()) {
                            using (var s = resp.GetResponseStream()) {
                                byte[] buf = new byte[4096];
                                int read = 0;
                                while ((read = s.Read(buf, 0, 4096)) > 0) {
                                    bytes.AddRange(buf.Take(read));
                                    // 0 out the array??? 
                                    // buf = new byte[4096];
                                }
                            }
                        }
                        ret = bytes.ToArray();

                    } else {
                        // otherwise load as normal
                        if (HttpContext.Current != null) {
                            // this can be app-relative, root-relative, or document-relative...
                            if (isDocumentRelativeUrl(appOrDocRelativePath)) {
                                // resolve the image down to the proper folder...
                                appOrDocRelativePath = "~/uploads/images/" + appOrDocRelativePath;
                            } else {
                                // they're specifying root-relatieve or app relative.  assume they know what is best and just pull it.
                            }
                            path = HttpContext.Current.Server.MapPath(appOrDocRelativePath);
                        } else {
                            // no http context. assume relative to current directory.
                            path = Toolkit.ResolveFilePath(appOrDocRelativePath, false);
                        }
                        if (!File.Exists(path)) {
                            throw new FileNotFoundException(getDisplayMember("GetImage{nofile}", "Could not find image {0} on the server.", appOrDocRelativePath));
                        } 
                        ret = File.ReadAllBytes(path);

                    }

                }
            } catch (Exception ex){
                if (LogException(ex, null)) {
                    throw;
                }
            } finally {
                FinishProcessing(null, _languageID);
            }
            return ret;
        }
        #endregion Image

        #region IDisposable Members

        public void Dispose() {

		}

		#endregion

        /// <summary>
        /// Loads language-friendly text on demand for messages / errors / etc.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="substitutes"></param>
        /// <returns></returns>
        private static string getDisplayMember(string resourceName, string defaultValue, params string[] substitutes) {
            return ResourceHelper.GetDisplayMember(null, "MiddleTier", "SecureData", resourceName, null, defaultValue, substitutes);
        }
    }
}
