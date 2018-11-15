<!-- default file list -->
*Files to look at*:

* [HomeController.cs](./CS/Sample/Controllers/HomeController.cs) (VB: [HomeController.vb](./VB/Sample/Controllers/HomeController.vb))
* [Model.cs](./CS/Sample/Models/Model.cs) (VB: [Model.vb](./VB/Sample/Models/Model.vb))
* [GridViewPartial.cshtml](./CS/Sample/Views/Home/GridViewPartial.cshtml)
* **[Index.cshtml](./CS/Sample/Views/Home/Index.cshtml)**
<!-- default file list end -->
# GridView - Batch Edit - How to use the upload control in Batch Edit mode


This example illustrates how to use the upload control in Batch edit mode. Note that all files are <strong>stored in memory</strong> until you call the update method.<br /><br />1) Place UploadControl into the column's EditItem template:<br />


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


<br /><br />2) Handle the grid's client-side <a href="https://help.devexpress.com/#AspNet/DevExpressWebASPxGridViewScriptsASPxClientGridView_BatchEditStartEditingtopic">BatchEditStartEditing</a> event to set the grid's cell values to the upload control. It is possible to get the focused cell value using the <a href="https://help.devexpress.com/#AspNet/DevExpressWebASPxGridViewScriptsASPxClientGridViewBatchEditStartEditingEventArgs_rowValuestopic">e.rowValues</a> property:<br />


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


<br />3) Handle the grid's client-side <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebScriptsASPxClientGridView_BatchEditConfirmShowingtopic">BatchEditConfirmShowing</a> event to prevent data loss on the upload control's callbacks:<br />


```js
function OnBatchConfirm(s, e) {
    e.cancel = browseClicked;
}
```


This "browseClicked" flag is set to true when the upload control starts uploading a file and to false when the file has been uploaded or the user starts editing another cell.<br /><br />4) Handle the upload control's client-side <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebScriptsASPxClientUploadControl_TextChangedtopic">TextChanged</a> and <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebScriptsASPxClientUploadControl_FileUploadCompletetopic">FileUploadComplete</a> events to automatically upload the selected file and update the cell value after:<br />


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


<br />5) Handle the upload control's <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxUploadControl_FileUploadCompletetopic">FileUploadComplete</a> event to store the uploaded file in the session:<br />


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


<br />Now you can access all the uploaded files in the Batch action method. Clear the session storage after you have updated the files.<br /><br /><strong>See also:</strong><br /><a href="https://www.devexpress.com/Support/Center/p/T191652">T191652: ASPxGridView - Batch Edit - How to use the upload control in Batch Edit mode</a>

<br/>


