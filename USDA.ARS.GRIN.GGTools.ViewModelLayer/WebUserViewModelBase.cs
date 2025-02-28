using System;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.AppLayer;
using USDA.ARS.GRIN.GGTools.DataLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class WebUserViewModelBase: AuthenticatedViewModelBase 
    {
        public WebUserViewModelBase() {}

        private int _SysUserID; 
        private string _PasswordResetToken;
        private WebUser _Entity = new WebUser();
        private WebUserSearch _SearchEntity = new WebUserSearch();
        private Collection<WebUser> _DataCollection = new Collection<WebUser>();
        private Collection<SysGroupUserMap> _DataCollectionGroups = new Collection<SysGroupUserMap>();

        public int SysUserID
        {
            get { return _SysUserID; }
            set { _SysUserID = value; }
        }
        public string PasswordResetToken 
        {
            get { return _PasswordResetToken; }
            set { _PasswordResetToken = value; }
        }

        public WebUser Entity
        {
            get { return _Entity; }
            set { _Entity = value; }
        }

        public WebUserSearch SearchEntity
        {
            get { return _SearchEntity; }
            set { _SearchEntity = value; }
        }

        public Collection<WebUser> DataCollection
        {
            get { return _DataCollection; }
            set { _DataCollection = value; }
        }
        public Collection<SysGroupUserMap> DataCollectionGroups
        {
            get { return _DataCollectionGroups; }
            set { _DataCollectionGroups = value; }
        }

        protected bool validateHashedPassword(string password, string storedSaltHash)
        {
            string crypt;
            string salt;
            string storedHash;

            // parse the stored password field for hash type, salt, and hashed password
            string[] hashes = storedSaltHash.Split(':');
            string[] passField = hashes[0].Split('$');
            if (passField.Length == 1)
            {
                // original format of SHA1 hash with no salt
                crypt = "SHA1";
                salt = "";
                storedHash = passField[0];
            }
            else if (passField.Length == 2)
            {
                // two fields means salt and hash
                crypt = "SHA256";
                salt = passField[0];
                storedHash = passField[1];
            }
            else if (passField.Length == 3)
            {
                // with three fields the first is the hash type
                crypt = passField[0];
                salt = passField[1];
                storedHash = passField[2];
            }
            else
            {
                // can't figure out what is stored in the hash field
                return false;
            }

            string hashedPassword;
            if (crypt == "SHA1")
            {
                hashedPassword = Crypto.HashText(salt + password);
            }
            else if (crypt == "SHA256")
            {
                hashedPassword = Crypto.HashTextSHA256(salt + password);
            }
            else
            {
                // don't understand the hash type
                return false;
            }

            // Finally we test whether it is a match
            if (hashedPassword == storedHash) return true;

            return false;
        }
    }
}
