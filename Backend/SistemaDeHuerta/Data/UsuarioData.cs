using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using SistemaDeHuerta.Models;

namespace SistemaDeHuerta.Data
{
    public class UsuarioData
    {
        private static object objEst;

        // Método de prueba para verificar la conexión
        public static string PruebaConexion()
        {
            ConexionBD oConexion = new ConexionBD();
            string resultado = "";
            
            try
            {
                // Prueba simple: contar usuarios
                string sentencia = "SELECT COUNT(*) as total FROM Usuario";
                
                if (oConexion.ConsultarValorUnico(sentencia, false))
                {
                    resultado = "Conexión OK. Total usuarios en BD: " + oConexion.ValorUnico;
                }
                else
                {
                    resultado = "Error en consulta: " + oConexion.Error;
                }
            }
            catch (Exception ex)
            {
                resultado = "Error en PruebaConexion: " + ex.Message;
            }
            
            oConexion.CerrarConexion();
            return resultado;
        }

        public static bool RegistrarUsuario(usuario oUsuario)
        {
            ConexionBD oConexion = new ConexionBD();

            string sentencia = "EXECUTE RegistrarUsuario '" + oUsuario.nombre + "', '" + oUsuario.tipoUsuario + "', '" +
                oUsuario.cedula + "', '" + oUsuario.correo + "', '" + oUsuario.contraseña + "'";

            bool success = oConexion.EjecutarSentencia(sentencia, false);
            objEst = null;
            return success;
        }

        public static int ActualizarUsuario(usuario oUsuario)
        {
            ConexionBD oConexion = new ConexionBD();
            
            // Crear comando con parámetros
            SqlCommand cmd = new SqlCommand("ActualizarUsuario");
            cmd.CommandType = CommandType.StoredProcedure;

            // Agregar parámetros de entrada
            cmd.Parameters.AddWithValue("@idUsuario", oUsuario.idUsuario);
            cmd.Parameters.AddWithValue("@nombre", oUsuario.nombre ?? "");
            cmd.Parameters.AddWithValue("@tipoUsuario", oUsuario.tipoUsuario.ToString());
            cmd.Parameters.AddWithValue("@cedula", oUsuario.cedula ?? "");
            cmd.Parameters.AddWithValue("@correo", oUsuario.correo ?? "");
            cmd.Parameters.AddWithValue("@contraseña", oUsuario.contraseña ?? "");

            // Agregar parámetro de salida
            SqlParameter resultadoParam = new SqlParameter("@Resultado", SqlDbType.Int);
            resultadoParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(resultadoParam);

            try
            {
                oConexion.EjecutarComando(cmd);
                int resultado = (int)resultadoParam.Value;
                return resultado;
            }
            catch (Exception ex)
            {
                oConexion.Error = ex.Message;
                return -1;
            }
        }
        
        public static List<usuario> ListarUsuarios()
        {
            List<usuario> listaUsuarios = new List<usuario>();
            ConexionBD oConexion = new ConexionBD();
            
            try
            {
                // Usar consulta SQL directa en lugar de procedimiento almacenado
                string sentencia = "SELECT idUsuario, nombre, tipoUsuario, cedula, correo, contraseña FROM Usuario ORDER BY nombre ASC";
                
                if (oConexion.Consultar(sentencia, false))
                {
                    SqlDataReader reader = oConexion.Reader;
                    
                    while (reader.Read())
                    {
                        try
                        {
                            usuario oUsuario = new usuario
                            {
                                idUsuario = Convert.ToInt32(reader["idUsuario"]),
                                nombre = reader["nombre"] != DBNull.Value ? reader["nombre"].ToString() : "",
                                tipoUsuario = reader["tipoUsuario"] != DBNull.Value ? 
                                    (TipoUsuario)Enum.Parse(typeof(TipoUsuario), reader["tipoUsuario"].ToString()) : 
                                    TipoUsuario.Invitado,
                                cedula = reader["cedula"] != DBNull.Value ? reader["cedula"].ToString() : "",
                                correo = reader["correo"] != DBNull.Value ? reader["correo"].ToString() : "",
                                contraseña = reader["contraseña"] != DBNull.Value ? reader["contraseña"].ToString() : ""
                            };
                            listaUsuarios.Add(oUsuario);
                            System.Diagnostics.Debug.WriteLine($"Usuario cargado: {oUsuario.nombre}");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Error mapeando usuario: " + ex.Message);
                        }
                    }
                    reader.Close();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Error al consultar: " + oConexion.Error);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en ListarUsuarios: " + ex.Message);
            }
            finally
            {
                oConexion.CerrarConexion();
            }
            
            System.Diagnostics.Debug.WriteLine($"Total usuarios retornados: {listaUsuarios.Count}");
            return listaUsuarios;
        }

    }
}