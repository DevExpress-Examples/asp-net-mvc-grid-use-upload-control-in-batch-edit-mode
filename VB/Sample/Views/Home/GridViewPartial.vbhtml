@Html.DevExpress().GridView(
    Sub(settings)
            settings.Name = "gridView"
            settings.CallbackRouteValues = New With {.Controller = "Home", .Action = "GridViewPartial"}
            settings.SettingsEditing.BatchUpdateRouteValues = New With {.Controller = "Home", .Action = "GridViewBatchUpdate"}

            settings.KeyFieldName = "ID"
            settings.SettingsEditing.Mode = GridViewEditingMode.Batch

            settings.Columns.Add("ID").ReadOnly = True
            settings.Columns.Add("PersonName")
            settings.Columns.Add(
                Sub(c)
                        c.FieldName = "FileName"
                        c.Width = 200
                        c.SetEditItemTemplateContent(
                            Sub(container)
                                    Html.DevExpress().UploadControl(
                                        Sub(ucSettings)
                                                ucSettings.Name = "uc"
                                                ucSettings.CallbackRouteValues = New With {.Controller = "Home", .Action = "UploadControlUploadFile"}
                                                ucSettings.UploadMode = UploadControlUploadMode.Advanced
                                                ucSettings.Width = Unit.Percentage(100)
                                                ucSettings.ClientSideEvents.TextChanged = "OnTextChanged"
                                                ucSettings.ClientSideEvents.FileUploadComplete = "OnFileUploadComplete"
                                        End Sub).Render()
                            End Sub)
                End Sub)


            settings.BeforeGetCallbackResult = Function(s, e)
                                                        Sample.Models.Helper.Files.Clear()
                                                End Function

            settings.ClientSideEvents.BatchEditStartEditing = "OnBatchStartEditing"
            settings.ClientSideEvents.BatchEditConfirmShowing = "OnBatchConfirm"
            settings.ClientSideEvents.BatchEditEndEditing = "OnBatchEditEndEditing"
    
    End Sub).Bind(Model).GetHtml()