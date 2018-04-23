Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Namespace Sample.Models
	Public NotInheritable Class Helper
		Private Sub New()
		End Sub
		Public Shared Property Files() As Dictionary(Of Object, Byte())
			Get
				If HttpContext.Current.Session("files") Is Nothing Then
					HttpContext.Current.Session("files") = New Dictionary(Of Object, Byte())()
				End If
				Return TryCast(HttpContext.Current.Session("files"), Dictionary(Of Object, Byte()))
			End Get
			Set(ByVal value As Dictionary(Of Object, Byte()))
				HttpContext.Current.Session("files") = value
			End Set
		End Property
		Public Shared Function GetData() As IEnumerable(Of TestModel)
			Return Enumerable.Range(0, 10).Select(Function(i) New TestModel() With {.ID = i, .PersonName = "Name " & i, .FileName = ""})
		End Function
	End Class



	Public Class TestModel
		Private privateID As Integer
		Public Property ID() As Integer
			Get
				Return privateID
			End Get
			Set(ByVal value As Integer)
				privateID = value
			End Set
		End Property
		Private privatePersonName As String
		Public Property PersonName() As String
			Get
				Return privatePersonName
			End Get
			Set(ByVal value As String)
				privatePersonName = value
			End Set
		End Property
		Private privateFileName As String
		Public Property FileName() As String
			Get
				Return privateFileName
			End Get
			Set(ByVal value As String)
				privateFileName = value
			End Set
		End Property
		Private privateFile As Byte()
		Public Property File() As Byte()
			Get
				Return privateFile
			End Get
			Set(ByVal value As Byte())
				privateFile = value
			End Set
		End Property
	End Class

End Namespace