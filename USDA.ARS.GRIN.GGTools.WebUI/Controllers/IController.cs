using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace USDA.ARS.GRIN.GGTools.WebUI
{
    public interface IController<T>
    {
        [HttpPost]
        ActionResult Search(T viewModel);
        ActionResult Index();
        ActionResult Edit(int entityId);
        [HttpPost]
        ActionResult Edit(T viewModel);
        //T LoadFromSession(T viewModel);
        [HttpPost]
        ActionResult Delete(FormCollection formCollection);
        //PartialViewResult _ListFolderItems(int sysFolderId);
    }
}
