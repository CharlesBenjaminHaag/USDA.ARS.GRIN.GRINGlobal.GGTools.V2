using System;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.Taxonomy.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using System.Collections.Generic;

namespace USDA.ARS.GRIN.GGTools.Taxonomy.ViewModelLayer
{
    public class FamilyViewModel: FamilyViewModelBase, IViewModel<Family>
    {
        public void Delete()
        {
            try
            {
                using (FamilyManager mgr = new FamilyManager())
                {
                    mgr.Delete(TableName, Entity.ID);
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public Family Get(int entityId)
        {
            try
            {
                using (FamilyManager mgr = new FamilyManager())
                {
                    try
                    {
                        SearchEntity.ID = entityId;
                        Entity = mgr.Get(entityId);

                        // Set web vis. Boolean flag used by UI.
                        if (Entity.IsWebVisible == "Y")
                        {
                            IsWebVisibleSelector = true;
                        }
                        else 
                        {
                            IsWebVisibleSelector = false;
                        }

                        // If not accepted, retrieve accepted-fam data.
                        if (Entity.IsAcceptedName == "N")
                        {
                            Family familyAccepted = new Family();
                            familyAccepted = mgr.Get(Entity.AcceptedID);
                            Entity.AcceptedName = familyAccepted.AssembledName;
                        }
                        RowsAffected = mgr.RowsAffected;
                    }
                    catch (Exception ex)
                    {
                        PublishException(ex);
                        throw ex;
                    }
                }

                if (Entity.TypeGenusID > 0)
                {
                    using (GenusManager genusMgr = new GenusManager())
                    {
                        Genus typeGenus = new Genus();
                        typeGenus = genusMgr.Get(Entity.TypeGenusID);
                        Entity.TypeGenusName = typeGenus.AssembledName;
                    }
                }

                return Entity;
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public void GetSynonyms(int entityId)
        {
            using (FamilyManager mgr = new FamilyManager())
            {
                try
                {
                    DataCollectionSynonyms = new Collection<Family>(mgr.GetSynonyms(entityId));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }
        
        public void GetSubdivisions(string familyName)
        {
            using (FamilyManager mgr = new FamilyManager())
            {
                try
                {
                    DataCollectionSubdivisions = new Collection<Family>(mgr.GetSubdivisions(familyName));
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
            }
        }

        public void GetFolderItems(int sysFolderId)
        {
            using (FamilyManager mgr = new FamilyManager())
            {
                try
                {
                    DataCollection = new Collection<Family>(mgr.GetFolderItems(sysFolderId));
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
            try
            {
                using (FamilyManager mgr = new FamilyManager())
                {
                    Entity.ID = mgr.Insert(Entity);
                    return Entity.ID;
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public int Update()
        {
            try
            {
                using (FamilyManager mgr = new FamilyManager())
                {
                    Entity.ID = mgr.Update(Entity);
                    return Entity.ID;
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
        }

        public void Search()
        {
            try
            {
                using (FamilyManager mgr = new FamilyManager())
                {
                    try
                    {
                        DataCollection = new Collection<Family>(mgr.Search(SearchEntity));
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
            
            catch (Exception ex)
            {
                PublishException(ex);
                throw ex;
            }
}
    }
}
