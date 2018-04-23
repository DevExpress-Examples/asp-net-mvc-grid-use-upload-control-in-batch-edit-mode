Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports DevExpress.Web.ASPxUploadControl
Imports DevExpress.Web.Mvc
Imports Sample.Models

Namespace Sample.Controllers
	Public Class HomeController
		Inherits Controller
		Public Function Index() As ActionResult
			Return View()
		End Function

		Public Function GridViewPartial() As ActionResult
			Return PartialView(Helper.GetData())
		End Function

		Public Function GridViewBatchUpdate(ByVal updateValues As MVCxGridViewBatchUpdateValues(Of TestModel, Integer)) As ActionResult
			Throw New Exception("Online data modification is not supported. Download the example and implement your logic in the GridViewBatchUpdate action method.")
			'update the datasource here using the uploaded files
			'clear the dictionary after
			Helper.Files.Clear()
			Return PartialView("BatchEditingPartial", Helper.GetData())
		End Function

		Public Function UploadControlUploadFile() As ActionResult
			Dim visibleIndex = Convert.ToInt32(Request.Params("hf"))
            UploadControlExtension.GetUploadedFiles("uc", Nothing, Function(s, e)
                                                                                Dim name = e.UploadedFile.FileName
                                                                                e.CallbackData = name

                                                                                If Helper.Files.ContainsKey(visibleIndex) Then
                                                                                    Helper.Files(visibleIndex) = e.UploadedFile.FileBytes
                                                                                Else
                                                                                    Helper.Files.Add(visibleIndex, e.UploadedFile.FileBytes)
                                                                                End If

                                                                            End Function)
			Return Nothing
        End Function
	End Class
End Namespace