using System;
using System.Web.Mvc;
using USDA.ARS.GRIN.GGTools.DataLayer;
using USDA.ARS.GRIN.GGTools.ViewModelLayer;
using NLog;

namespace USDA.ARS.GRIN.GGTools.WebUI.Controllers
{
    [GrinGlobalAuthentication]
    public class OrderRequestController : BaseController, IController<OrderRequestViewModel>
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public ActionResult Delete(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Edit(int entityId)
        {
            OrderRequestViewModel viewModel = new OrderRequestViewModel();

            try
            {
                viewModel.Get(entityId);
                ViewBag.PageTitle = String.Format("Order Request [{0}]: {1}", viewModel.Entity.ID, viewModel.Entity.RequestorCooperatorName);
                return View("~/Views/OrderRequest/Edit.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Edit(OrderRequestViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public ActionResult Index()
        {
            try
            {
                OrderRequestViewModel viewModel = new OrderRequestViewModel();
                viewModel.PageTitle = "Order Request Search";
                return View("~/Views/OrderRequest/Index.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return RedirectToAction("InternalServerError", "Error");
            }
        }

        [HttpPost]
        public ActionResult Search(OrderRequestViewModel viewModel)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult _Get(int entityId = 0, int webOrderRequestId = 0)
        {
            OrderRequestViewModel viewModel = new OrderRequestViewModel();

            try
            {
                if (entityId > 0)
                {
                    viewModel.Get(entityId);
                }
                else 
                { 
                    if (webOrderRequestId > 0)
                    {
                        viewModel.SearchEntity.WebOrderRequestID = webOrderRequestId;
                        viewModel.Search();
                    }
                }
                return PartialView("~/Views/OrderRequest/_Detail.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _List(int orderRequestId = 0, int webOrderRequestId = 0)
        {
            try
            {
                OrderRequestViewModel viewModel = new OrderRequestViewModel();
                if (webOrderRequestId > 0)
                {
                    viewModel.SearchEntity.WebOrderRequestID = webOrderRequestId;
                }
                viewModel.Search();
                return PartialView("~/Views/OrderRequest/_List.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public PartialViewResult _ListItems(int orderRequestId)
        {
            try
            {
                OrderRequestViewModel viewModel = new OrderRequestViewModel();
                viewModel.GetItems(orderRequestId);
                return PartialView("~/Views/OrderRequest/_ListItems.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListActions(int orderRequestId)
        {
            try
            {
                OrderRequestViewModel viewModel = new OrderRequestViewModel();
                viewModel.GetActions(orderRequestId);
                return PartialView("~/Views/OrderRequest/_ListActions.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListAttachments(int orderRequestId)
        {
            try
            {
                OrderRequestViewModel viewModel = new OrderRequestViewModel();
                viewModel.GetAttachments(orderRequestId);
                return PartialView("~/Views/OrderRequest/_ListAttachments.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListPhytoLog(int orderRequestId)
        {
            try
            {
                OrderRequestViewModel viewModel = new OrderRequestViewModel();
                viewModel.GetPhytoLog(orderRequestId);
                return PartialView("~/Views/OrderRequest/_ListPhytoLog.cshtml", viewModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }

        public PartialViewResult _ListFolderItems(int sysFolderId)
        {
            try
            {
                return PartialView("~/Views/Shared/_UnderConstruction.cshtml");
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                return PartialView("~/Views/Error/_InternalServerError.cshtml");
            }
        }
        
        public JsonResult Add(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        public ActionResult Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public PartialViewResult FolderItems(FormCollection formCollection)
        {
            throw new NotImplementedException();
        }

        
    }
}