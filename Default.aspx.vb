Imports System.Data
Imports System.Data.SqlClient

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarEstadisticas()
        End If
    End Sub

    Private Sub CargarEstadisticas()
        Try
            Using connection As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("BDGanaderiaConnectionString").ConnectionString)
                connection.Open()
                
                Dim sb As New System.Text.StringBuilder()
                
                ' Total de animales
                Try
                    Dim queryAnimales As String = "SELECT COUNT(*) FROM Animal"
                    Dim totalAnimales As Object = Nothing
                    Using cmd As New SqlCommand(queryAnimales, connection)
                        totalAnimales = cmd.ExecuteScalar()
                    End Using
                    If totalAnimales IsNot Nothing Then
                        sb.AppendLine("document.getElementById('totalAnimales').textContent = '" & totalAnimales.ToString() & "';")
                    End If
                Catch
                End Try
                
                ' Total de pesajes registrados
                Try
                    Dim queryPesajes As String = "SELECT COUNT(*) FROM Animal_Pesaje"
                    Dim pesajes As Object = Nothing
                    Using cmd As New SqlCommand(queryPesajes, connection)
                        pesajes = cmd.ExecuteScalar()
                    End Using
                    If pesajes IsNot Nothing Then
                        sb.AppendLine("document.getElementById('animalesActivos').textContent = '" & pesajes.ToString() & "';")
                    End If
                Catch
                End Try
                
                ' Razas registradas
                Try
                    Dim queryRazas As String = "SELECT COUNT(*) FROM Raza"
                    Dim razas As Object = Nothing
                    Using cmd As New SqlCommand(queryRazas, connection)
                        razas = cmd.ExecuteScalar()
                    End Using
                    If razas IsNot Nothing Then
                        sb.AppendLine("document.getElementById('razasRegistradas').textContent = '" & razas.ToString() & "';")
                    End If
                Catch
                End Try
                
                ' Controles realizados
                Try
                    Dim queryControles As String = "SELECT COUNT(*) FROM Control_Lechero"
                    Dim controles As Object = Nothing
                    Using cmd As New SqlCommand(queryControles, connection)
                        controles = cmd.ExecuteScalar()
                    End Using
                    If controles IsNot Nothing Then
                        sb.AppendLine("document.getElementById('controlesRealizados').textContent = '" & controles.ToString() & "';")
                    End If
                Catch
                End Try
                
                ' Total de vacunas
                Try
                    Dim queryVacunas As String = "SELECT COUNT(*) FROM Vacuna"
                    Dim totalVacunas As Object = Nothing
                    Using cmd As New SqlCommand(queryVacunas, connection)
                        totalVacunas = cmd.ExecuteScalar()
                    End Using
                    If totalVacunas IsNot Nothing Then
                        sb.AppendLine("document.getElementById('litrosTotales').textContent = '" & totalVacunas.ToString() & "';")
                    End If
                Catch
                End Try
                
                ' Tratamientos registrados
                Try
                    Dim queryTratamientos As String = "SELECT COUNT(*) FROM Animal_Tratamiento"
                    Dim tratamientos As Object = Nothing
                    Using cmd As New SqlCommand(queryTratamientos, connection)
                        tratamientos = cmd.ExecuteScalar()
                    End Using
                    If tratamientos IsNot Nothing Then
                        sb.AppendLine("document.getElementById('tratamientosRegistrados').textContent = '" & tratamientos.ToString() & "';")
                    End If
                Catch
                End Try
                
                ' Total de empleados
                Try
                    Dim queryEmpleados As String = "SELECT COUNT(*) FROM Empleado"
                    Dim empleados As Object = Nothing
                    Using cmd As New SqlCommand(queryEmpleados, connection)
                        empleados = cmd.ExecuteScalar()
                    End Using
                    If empleados IsNot Nothing Then
                        sb.AppendLine("document.getElementById('vacunasAplicadas').textContent = '" & empleados.ToString() & "';")
                    End If
                Catch
                End Try
                
                ' Raciones asignadas este mes
                Try
                    Dim queryRaciones As String = "SELECT COUNT(*) FROM Animal_Racion WHERE MONTH(Fecha_Asignacion) = MONTH(GETDATE()) AND YEAR(Fecha_Asignacion) = YEAR(GETDATE())"
                    Dim raciones As Object = Nothing
                    Using cmd As New SqlCommand(queryRaciones, connection)
                        raciones = cmd.ExecuteScalar()
                    End Using
                    If raciones IsNot Nothing Then
                        sb.AppendLine("document.getElementById('racionesAsignadas').textContent = '" & raciones.ToString() & "';")
                    End If
                Catch
                End Try
                
                ' Tipos de alimento
                Try
                    Dim queryAlimentos As String = "SELECT COUNT(*) FROM Alimento"
                    Dim alimentos As Object = Nothing
                    Using cmd As New SqlCommand(queryAlimentos, connection)
                        alimentos = cmd.ExecuteScalar()
                    End Using
                    If alimentos IsNot Nothing Then
                        sb.AppendLine("document.getElementById('tiposAlimento').textContent = '" & alimentos.ToString() & "';")
                    End If
                Catch
                End Try
                
                ' Ejecutar script
                If sb.Length > 0 Then
                    ClientScript.RegisterStartupScript(Me.GetType(), "UpdateStats", sb.ToString(), True)
                End If
            End Using
        Catch ex As Exception
            ' En caso de error, mantener los valores por defecto
        End Try
    End Sub
End Class
