using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class SubtribeViewModel : SubtribeViewModelBase, IViewModel<Subtribe>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Subtribe Get(int entityId)
        {
            using (SubtribeManager mgr = new SubtribeManager())
            {
                try
                {
                    SearchEntity.ID = entityId;
                    Search();
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
            return Entity;
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (SubtribeManager mgr = new SubtribeManager())
            {
                try
                {
                    Entity.ID = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }

        public void Search()
        {
            using (SubtribeManager mgr = new SubtribeManager())
            {
                try
                {
                    DataCollection = new Collection<Subtribe>(mgr.Search(SearchEntity));
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
        public void SearchFolderItems()
        {
            using (SubtribeManager mgr = new SubtribeManager())
            {
                try
                {
                    DataCollection = new Collection<Subtribe>(mgr.SearchFolderItems(SearchEntity));
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

        public List<Subtribe> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            using (SubtribeManager mgr = new SubtribeManager())
            {
                try
                {
                    RowsAffected = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return RowsAffected;
            }
        }

    }
}