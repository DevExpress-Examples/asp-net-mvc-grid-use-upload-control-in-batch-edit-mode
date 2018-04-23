using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.ASPxUploadControl;
using DevExpress.Web.Mvc;
using Sample.Models;

namespace Sample.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult GridViewPartial() {
            return PartialView(Helper.GetData());
        }

        public ActionResult GridViewBatchUpdate(MVCxGridViewBatchUpdateValues<TestModel, int> updateValues) {
            throw new Exception("Online data modification is not supported. Download the example and implement your logic in the GridViewBatchUpdate action method.");
            //update the datasource here using the uploaded files
            //clear the dictionary after
            Helper.Files.Clear();
            return PartialView("BatchEditingPartial", Helper.GetData());
        }

        public ActionResult UploadControlUploadFile() {            
            var visibleIndex = Convert.ToInt32(Request.Params["hf"]);
            UploadControlExtension.GetUploadedFiles("uc", null, (s,e) => {
                var name = e.UploadedFile.FileName;
                e.CallbackData = name;

                if (Helper.Files.ContainsKey(visibleIndex))
                    Helper.Files[visibleIndex] = e.UploadedFile.FileBytes;
                else
                    Helper.Files.Add(visibleIndex, e.UploadedFile.FileBytes);
            });
            return null;
        }
    }
}