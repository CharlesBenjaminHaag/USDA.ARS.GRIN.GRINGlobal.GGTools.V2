using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;


namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class SubfamilyViewModel : SubfamilyViewModelBase, IViewModel<Subfamily>
    {
        public SubfamilyViewModel()
        {
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Subfamily Get(int entityId)
        {
            using (SubfamilyManager mgr = new SubfamilyManager())
            {
                try
                {
                    SearchEntity.ID = entityId; 
                    Search();
                    if (Entity.IsAcceptedName == "Y")
                        Entity.SetAccepted = true;
                    else
                        Entity.SetAccepted = false;
                    //DataCollectionInfrafamilialTaxa = new Collection<InfrafamilialTaxon>(mgr.GetInfrafamilialTaxa(SearchEntity.ID));
                    //DataCollectionCitations = new Collection<Citation>(mgr.GetCitations(SearchEntity.ID));
                    
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
            using (SubfamilyManager mgr = new SubfamilyManager())
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
            //using (FamilyManager mgr = new FamilyManager())
            //{
            //    try
            //    {
            //        DataCollection = new Collection<FamilyMap>(mgr.Search(SearchEntity));
            //        RowsAffected = mgr.RowsAffected;
            //        if (RowsAffected == 1)
            //        {
            //            Entity = DataCollection[0];
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        PublishException(ex);
            //        throw ex;
            //    }
            //}
        }

        public void SearchFolderItems()
        {
            using (SubfamilyManager mgr = new SubfamilyManager())
            {
                try
                {
                    //DataCollection = new Collection<Subfamily>(mgr.SearchFolderItems(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        //Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public List<FamilyMap> AcceptedNameSearch(string searchText)
        {
            List<FamilyMap> infraFamilyList = new List<FamilyMap>();
            using (SubfamilyManager mgr = new SubfamilyManager())
            {
                SearchEntity.SubFamilyName = searchText;
                DataCollectionInfraFamilies = new Collection<FamilyMap>(mgr.AcceptedNameSearch(searchText));
            }
            return infraFamilyList;
        }
        
        public int Update()
        {
            using (SubfamilyManager mgr = new SubfamilyManager())
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

            if (String.IsNullOrEmpty(Entity.SubfamilyName))
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The subfamily name is required." });
            }

            if (Entity.FamilyID == 0)
            {
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "You must specify a family." });
            }

            if (ValidationMessages.Count > 0)
                validated = false;

            return validated;
        }

        public List<Subfamily> SearchNotes(string searchText)
        {
            throw new NotImplementedException();
        }
    }
}
