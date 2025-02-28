using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class SysTableViewModel : SysTableViewModelBase, IViewModel<SysTable>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public SysTable Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public CodeValue GetRecordCount(string sysTableName)
        {
            using (SysTableManager mgr = new SysTableManager())
            {
                return mgr.GetRecordCount(sysTableName);
            }
        }

        public void GetSysTablesTaxonomy(bool loadChildData = false)
        {
            using (SysTableManager mgr = new SysTableManager())
            {
                try
                {
                    DataCollection = new Collection<SysTable>(mgr.GetSysTablesTaxonomy(loadChildData));
                    RowsAffected = mgr.RowsAffected;

                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }

                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public int Insert()
        {
            throw new NotImplementedException();
        }

        public void Search()
        {
            using (SysTableManager mgr = new SysTableManager())
            {
                try
                {
                    DataCollection = new Collection<SysTable>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;

                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }

                    //String DEBUG = SerializeToXml<CitationSearch>(SearchEntity);
                    //CitationSearch DEBUG2 = Deserialize<CitationSearch>(DEBUG);

                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public int Update()
        {
            throw new NotImplementedException();
        }

        public void TransferOwnership(string idList, string sysTableName, int recipientCooperatorId)
        {
            try
            {
                using (SysTableManager mgr = new SysTableManager())
                {
                    string[] idArray = idList.Split(',');

                    foreach (var idToken in idArray)
                    {
                        mgr.TransferOwnership(Int32.Parse(idToken), sysTableName, recipientCooperatorId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
