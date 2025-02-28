using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class TribeViewModel : TribeViewModelBase, IViewModel<Tribe>
    {
        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Tribe Get(int entityId)
        {
            using (TribeManager mgr = new TribeManager())
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
                return Entity;
            }
        }

        public void HandleRequest()
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            using (TribeManager mgr = new TribeManager())
            {
                try
                {
                    //TEMP
                    if (Entity.IsAccepted)
                    {
                        Entity.AcceptedID = 0;
                    }
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
            using (TribeManager mgr = new TribeManager())
            {
                try
                {
                    DataCollection = new Collection<Tribe>(mgr.Search(SearchEntity));
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
            using (TribeManager mgr = new TribeManager())
            {
                try
                {
                    DataCollection = new Collection<Tribe>(mgr.SearchFolderItems(SearchEntity));
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

        public List<Tribe> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            using (TribeManager mgr = new TribeManager())
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
        public override bool Validate()
        {
            bool validated = true;

            if (String.IsNullOrEmpty(Entity.TribeName))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The tribe name is required." });
            }

            if (String.IsNullOrEmpty(Entity.FamilyName))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "You must specify a family." });
            }

            if (ValidationMessages.Count > 0)
                validated = false;

            return validated;
        }
    }
}
