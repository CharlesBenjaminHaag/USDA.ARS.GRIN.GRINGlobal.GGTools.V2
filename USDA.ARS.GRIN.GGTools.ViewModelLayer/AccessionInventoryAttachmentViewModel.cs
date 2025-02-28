using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Collections.ObjectModel;
using USDA.ARS.GRIN.Common.Library;
using USDA.ARS.GRIN.Common.Library.Exceptions;
using USDA.ARS.GRIN.Common.Library.Security;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.AppLayer;

namespace USDA.ARS.GRIN.GGTools.ViewModelLayer
{
    public class AccessionInventoryAttachmentViewModel : AccessionInventoryAttachmentViewModelBase, IViewModel<AccessionInventoryAttachment>
    {
        public HttpPostedFileBase VirtualPathUpload { get; set; }
        public HttpPostedFileBase ThumbnailVirtualPathUpload { get; set; }

        public void Delete()
        {
            throw new NotImplementedException();
        }
        public void DeleteAll()
        { }

        public AccessionInventoryAttachment Get(int entityId)
        {
            using (AccessionInventoryAttachmentManager mgr = new AccessionInventoryAttachmentManager())
            {
                try
                {
                    Entity = mgr.Get(entityId);
                    RowsAffected = mgr.RowsAffected;
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
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
            throw new NotImplementedException();
        }

        public void Search()
        {
            using (AccessionInventoryAttachmentManager mgr = new AccessionInventoryAttachmentManager())
            {
                try
                {
                    DataCollection = new Collection<AccessionInventoryAttachment>(mgr.Search(SearchEntity));
                    RowsAffected = mgr.RowsAffected;
                    if (RowsAffected == 1)
                    {
                        Entity = DataCollection[0];
                    }
                }
                catch (Exception ex)
                {
                    PublishException(ex);
                    throw (ex);
                }
            }
        }
       
        public int Update()
        {
            using (AccessionInventoryAttachmentManager mgr = new AccessionInventoryAttachmentManager())
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
            }
            return RowsAffected;
        }

        [HttpPost]
        public void ValidateAll()
        {
            try
            {
                foreach (var id in ItemIDList.Split(','))
                {
                    using (AccessionInventoryAttachmentManager accessionInventoryAttachmentManager = new AccessionInventoryAttachmentManager())
                    {
                        int accessionInventoryAttachmentId = Int32.Parse(id);
                        Entity = accessionInventoryAttachmentManager.Get(accessionInventoryAttachmentId);
                        Entity.IsVirtualPathValid = "Y";
                        Entity.IsThumbnailVirtualPathValid = "Y";

                        if (!String.IsNullOrEmpty(Entity.VirtualPath))
                        {
                            if (!Entity.VirtualPath.Contains("http"))
                            {
                                Entity.VirtualPath = "https://npgsweb.ars-grin.gov/gringlobal/uploads/images/" + Entity.VirtualPath;
                            }

                            FileMetaData fileMetaData = new FileMetaData();
                            fileMetaData = GetFileMetaData(Entity.VirtualPath);

                            if (fileMetaData.ResultCode == "NotFound")
                            {
                                Entity.IsVirtualPathValid = "N";
                            }
                        }

                        if (!String.IsNullOrEmpty(Entity.ThumbnailVirtualPath))
                        {
                            if (!Entity.ThumbnailVirtualPath.Contains("http"))
                            {
                                Entity.ThumbnailVirtualPath = "https://npgsweb.ars-grin.gov/gringlobal/uploads/images/" + Entity.ThumbnailVirtualPath;
                            }

                            FileMetaData thumbnailFileMetaData = new FileMetaData();
                            thumbnailFileMetaData = GetFileMetaData(Entity.ThumbnailVirtualPath);

                            if (thumbnailFileMetaData.ResultCode == "NotFound")
                            {
                                Entity.IsThumbnailVirtualPathValid = "N";
                            }
                        }
                        Update();
                    }
                    RowsAffected++;
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw (ex);
            }
        }

        public FileMetaData GetFileMetaData(string url)
        {
            FileMetaData fileMetaData = new FileMetaData();

            try
            {
                WebRequest webRequest = WebRequest.Create(url);
                WebResponse webResponse = webRequest.GetResponse();

                //if (webResponse == HttpStatusCode.OK)
                //{
                //    fileMetaData.ResultCode = response.StatusCode.ToString();
                //    fileMetaData.FileSize = response.ContentLength;
                //    fileMetaData.LastModified = response.LastModified;
                //}
                //else
                //{
                //    fileMetaData.ResultCode = response.StatusCode.ToString();
                //    fileMetaData.ResultMessage = response.StatusDescription;
                //}
            }
            catch (WebException webEx)
            {
                WebExceptionStatus status = webEx.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)webEx.Response;
                    fileMetaData.ResultCode = httpResponse.StatusCode.ToString();
                    fileMetaData.ResultMessage = webEx.Message;
                }
            }
            catch (Exception ex)
            {
                fileMetaData.ResultCode = "ERR";
                fileMetaData.ResultMessage = ex.Message;
            }
            return fileMetaData;
        }

        public void SearchFolderItems()
        {
            using (AccessionInventoryAttachmentManager mgr = new AccessionInventoryAttachmentManager())
            {
                try
                {
                    DataCollection = new Collection<AccessionInventoryAttachment>(mgr.SearchFolderItems(SearchEntity));
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
    }
}
