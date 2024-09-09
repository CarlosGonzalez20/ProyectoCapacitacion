using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Odbc;

namespace CapaControlador
{
    public class Consultas
    {
        Conexion con = new Conexion();
        // obtener datos de una tabla CAPA MODELO
        public OdbcDataAdapter llenarTbl(string tabla)// metodo  que obtinene el contenio de una tabla
        {
            //string para almacenar los campos de OBTENERCAMPOS y utilizar el 1ro
            string sql = "SELECT * FROM " + tabla + "  ;";
            OdbcDataAdapter dataTable = new OdbcDataAdapter(sql, con.conexion());
            return dataTable;
        }
        ///Modelo///
        public string[] llenarCmb(string tabla, string campo1, string campo2)
        {

            string[] Campos = new string[300];
            string[] auto = new string[300];
            int i = 0;
            string sql = "SELECT " + campo1 + "," + campo2 + " FROM " + tabla + " where estado = 1 ;";

            try
            {
                OdbcCommand command = new OdbcCommand(sql, con.conexion());
                OdbcDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    Campos[i] = reader.GetValue(0).ToString() + "-" + reader.GetValue(1).ToString();
                    i++;


                }




            }
            catch (Exception ex) { Console.WriteLine(ex.Message.ToString() + " \nError en asignarCombo, revise los parametros \n -" + tabla + "\n -" + campo1); }


            return Campos;



        }

        /// Modelo 2 //

        public DataTable obtener(string tabla, string campo1, string campo2)
        {

            string sql = "SELECT " + campo1 + "," + campo2 + " FROM " + tabla + " where estado = 1  ;";

            OdbcCommand command = new OdbcCommand(sql, con.conexion());
            OdbcDataAdapter adaptador = new OdbcDataAdapter(command);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);


            return dt;
        }
        public bool InsertarEmpleado(int codigoEmpleado, string nombreCompleto, string puesto, string departamento, int estado)
        {
            OdbcConnection connection = null;
            try
            {
                connection = con.conexion(); // Obtener la conexión
                using (OdbcCommand command = new OdbcCommand("INSERT INTO empleados (codigo_empleado, nombre_completo, puesto, departamento, estado) VALUES (?, ?, ?, ?, ?)", connection))
                {
                    // Agregar parámetros a la consulta
                    command.Parameters.Add("@codigo_empleado", OdbcType.Int).Value = codigoEmpleado;
                    command.Parameters.Add("@nombre_completo", OdbcType.VarChar).Value = nombreCompleto;
                    command.Parameters.Add("@puesto", OdbcType.VarChar).Value = puesto;
                    command.Parameters.Add("@departamento", OdbcType.VarChar).Value = departamento;
                    command.Parameters.Add("@estado", OdbcType.Int).Value = estado;

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Console.WriteLine("Error al insertar empleado: " + ex.Message);
                return false;
            }
            finally
            {
                if (connection != null)
                {
                    con.desconexion(connection); // Asegúrate de cerrar la conexión
                }
            }
        }
    }
}
