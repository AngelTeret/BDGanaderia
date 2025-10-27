Imports System.Web.Services
Imports System.ComponentModel
Imports System.Data
Imports System.Collections.Generic
Imports System.Linq

Partial Public Class Reportes
    Inherits System.Web.UI.Page
    
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' El CSS ya está incluido inline en el .aspx
    End Sub
    
    <WebMethod()>
    Public Shared Function GetReportData(storedProcedureName As String, parameters As Dictionary(Of String, Object)) As Object
        Try
            Dim dt = Reports.ReportsRepository.ExecuteReport(storedProcedureName, parameters)
            Dim rows As New List(Of Object)()
            
            For Each row As DataRow In dt.Rows
                Dim rowData As New Dictionary(Of String, Object)()
                For Each col As DataColumn In dt.Columns
                    Dim value As Object = row(col.ColumnName)
                    ' Convertir fechas a formato legible
                    If value IsNot Nothing AndAlso TypeOf value Is DateTime Then
                        Dim dateValue As DateTime = DirectCast(value, DateTime)
                        rowData(col.ColumnName) = dateValue.ToString("dd/MM/yyyy")
                    ElseIf value Is DBNull.Value Then
                        rowData(col.ColumnName) = Nothing
                    Else
                        rowData(col.ColumnName) = value
                    End If
                Next
                rows.Add(rowData)
            Next
            
            Return New With {
                .success = True,
                .data = rows,
                .columns = dt.Columns.Cast(Of DataColumn).Select(Function(c) c.ColumnName).ToList()
            }
        Catch ex As Exception
            Return New With {
                .success = False,
                .message = ex.Message
            }
        End Try
    End Function
    
    <WebMethod()>
    Public Shared Function GetDropdownOptions(spName As String, textField As String, valueField As String) As Object
        Try
            Dim options = Reports.ReportsRepository.GetDropdownData(spName, textField, valueField)
            Return New With {
                .success = True,
                .data = options.Select(Function(kvp) New With {.value = kvp.Key, .text = kvp.Value}).ToList()
            }
        Catch ex As Exception
            Return New With {
                .success = False,
                .message = ex.Message
            }
        End Try
    End Function
End Class

