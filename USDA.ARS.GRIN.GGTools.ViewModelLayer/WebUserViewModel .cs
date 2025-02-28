using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using USDA.ARS.GRIN.Common.Library.Email;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{

    public class WebUserViewModel : WebUserViewModelBase
    {
        public WebUser Get(int entityId, string environment = "")
        {
            using (WebUserManager mgr = new WebUserManager())
            {
                Entity = mgr.Get(entityId);
            }
            return Entity;
        }
        
        public int Search()
        {
            using (WebUserManager mgr = new WebUserManager())
            {
                try
                {
                    DataCollection = new Collection<WebUser>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected > 0)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return mgr.RowsAffected;
            }
        }

        public int Insert()
        {
            using (WebUserManager mgr = new WebUserManager())
            {
                try
                {
                    Entity.WebUserPassword = GetSecurePassword(Entity.WebUserPassword);
                    Entity.ID = mgr.Insert(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return Entity.ID;
            }
        }
        public int Update()
        {
            using (WebUserManager mgr = new WebUserManager())
            {
                try
                {
                    Entity.ID = mgr.Update(Entity);
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw ex;
                }
                return Entity.ID;
            }
        }
        public void Copy()
        {
            using (WebUserManager mgr = new WebUserManager())
            {
                Entity.ID = mgr.Copy(SysUserID, Entity.WebCooperatorID);
            }
        }
        public override bool Validate()
        {
            if (String.IsNullOrEmpty(Entity.WebUserName))
            {
                UserMessage = "Please enter your user name.";
                ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "Please enter your user name." });
                return false;
            }
                
            //if (Entity.Password != Entity.we)
            //{
            //    UserMessage = "The passwords that you have entered do not match.";
            //    ValidationMessages.Add(new Common.Library.ValidationMessage { Message = "The passwords that you have entered do not match." });
            //    return false;
            //}
           return true;
        }

        public string GetSecurePassword(string password)
        {
            password = Crypto.HashText(password);
            password = SaltAndHash(password);
            return password;
        }
        private static string SaltAndHash(string password)
        {
            int saltSize = 6;
            string salt = Crypto.SaltText(saltSize);
            string hash = Crypto.HashTextSHA256(salt + password);
            return salt + "$" + hash;
        }
    }
}

