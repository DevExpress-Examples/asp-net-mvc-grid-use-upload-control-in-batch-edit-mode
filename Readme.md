<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128549571/22.1.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T191714)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

# GridView for ASP.NET MVC - How to use the upload control in batch edit mode

This example illustrates how to use the upload control in batch edit mode. Note that all files are stored in memory until you call the update method.

## Overview

Follow the steps below:

1. Place an upload control to a column's edit item template:

    ```cs
    c.SetEditItemTemplateContent(container => {
        Html.DevExpress().UploadControl(ucSettings => {
            ucSettings.Name = "uc";
            ucSettings.CallbackRouteValues = new { Controller = "Home", Action = "UploadControlUploadFile" };
            ucSettings.UploadMode = UploadControlUploadMode.Advanced;
            ucSettings.Width = Unit.Percentage(100);
            ucSettings.ClientSideEvents.TextChanged = "OnTextChanged";
            ucSettings.ClientSideEvents.FileUploadComplete = "OnFileUploadComplete";
        }).Render();
    });
    ```

2. Handle the grid's client-side [BatchEditStartEditing](https://docs.devexpress.com/AspNet/js-ASPxClientGridView.BatchEditStartEditing) event to set the grid's cell values to the upload control. Use the [rowValues](https://docs.devexpress.com/AspNet/js-ASPxClientGridViewBatchEditStartEditingEventArgs.rowValues) argument property to get the focused cell value:

    ```js
    function OnBatchStartEditing(s, e) {
        browseClicked = false;
        $("#hf").val(e.visibleIndex);
        var fileNameColumn = s.GetColumnByField("FileName");
        if (!e.rowValues.hasOwnProperty(fileNameColumn.index))
            return;
        var cellInfo = e.rowValues[fileNameColumn.index];
    
        setTimeout(function () {
            SetUCText(cellInfo.value);
        }, 0);
    }
    ```

3. Handle the grid's client-side [BatchEditConfirmShowing](https://docs.devexpress.com/AspNet/js-ASPxClientGridView.BatchEditConfirmShowing) event to prevent data loss on the upload control's callbacks:

    ```js
    function OnBatchConfirm(s, e) {
        e.cancel = browseClicked;
    }
    ```

4. Handle the upload control's client-side `TextChanged` and `FileUploadComplete` events to automatically upload the selected file and update the cell value after:

    ```js
    function OnFileUploadComplete(s, e) {
        gridView.batchEditApi.SetCellValue($("#hf").val(), "FileName", e.callbackData);
        gridView.batchEditApi.EndEdit();
    }
    function OnTextChanged(s, e) {
        if (s.GetText()) {
            browseClicked = true;
            s.Upload();
        }
    }
    ```

5. Handle the upload control's `FileUploadComplete` event to store the uploaded file in the session:

    ```cs
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
    ```

Now you can access all the uploaded files in the batch action method. Clear the session storage after you have updated the files.

## Files to Review

* [HomeController.cs](./CS/Sample/Controllers/HomeController.cs) (VB: [HomeController.vb](./VB/Sample/Controllers/HomeController.vb))
* [Model.cs](./CS/Sample/Models/Model.cs) (VB: [Model.vb](./VB/Sample/Models/Model.vb))
* [GridViewPartial.cshtml](./CS/Sample/Views/Home/GridViewPartial.cshtml)
* [Index.cshtml](./CS/Sample/Views/Home/Index.cshtml)

## More Examples

* [ASPxGridView - Batch Edit - How to use the upload control in Batch Edit mode](https://github.com/DevExpress-Examples/aspxgridview-batch-edit-how-to-use-the-upload-control-in-batch-edit-mode-t191652)

