using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

/// <summary>
/// Descripción breve de GestorHotel
/// una masa Jumbotronn
/// </summary>
public class GestorHotel
{   //TODO aca instanciar la BD
    //ACÁ SE INSTANCIA LA CADENA DE CONEXIÓN, UNA ÚNICA VEZ
    public static string CadenaConexion = ConfigurationManager.ConnectionStrings["DBDavid"].ConnectionString;
    //public static string CadenaConexion = @"Data Source=DAVID-PC\SQLEXPRESS;Initial Catalog=PAV2;Integrated Security=True";
    //public static string CadenaConexion = "Data Source=MAQUIS;Initial Catalog=4K1_62726;User ID=avisuales2;Password=avisuales2";


    public static List<Hotel> BuscarPordescripcion(string descripcion, string orden, bool eliminados)
    {
        List<Hotel> listaHotel = new List<Hotel>();
        SqlConnection cn = new SqlConnection(GestorHotel.CadenaConexion);
        Hotel h2 = null;

        try
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cn;
            cmd.Parameters.Clear();
            cmd.Connection = cn;
            string sql="";
            if (eliminados)//TODO 04: Refactorizar! hay un metodo q busca la descripcion en las tablas, no es necesario el join para obtener el destino
                sql = "select h.id,h.descripcion, h.cuit, h.capacidad,d.descripcion destino_descripcion from Hotel h left join Destino d on h.destino = d.id where eliminado=1 AND h.descripcion like @descripcion order by " + orden;
            else
                sql = "select h.id,h.descripcion, h.cuit, h.capacidad,d.descripcion destino_descripcion from Hotel h left join Destino d on h.destino = d.id where (eliminado is NULL OR eliminado=0) AND h.descripcion like @descripcion order by " + orden;
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@descripcion", "%" + descripcion + "%"));
            //cmd.Parameters.Add(new SqlParameter("@eliminado", 0));
            //cmd.CommandType = CommandType.Text; // es necesario setear esta propiedad el valor por defecto es  CommandType.Text
            SqlDataReader dr = cmd.ExecuteReader();

            // con el resultado cargar una entidad
            while (dr.Read())
            {

                h2 = new Hotel();
                h2.descripcion = (string)dr["descripcion"];
                h2.descripcion = dr["descripcion"].ToString();
                h2.id = (int)dr["id"];
                h2.cuit = (long)dr["cuit"];
                // esto para cada atributo que acepte valores nulos
                if (dr["capacidad"] != DBNull.Value)
                    h2.capacidad = (int)(dr["capacidad"]);
                if (dr["destino_descripcion"] != DBNull.Value)
                    h2.destino_descripcion = (string)(dr["destino_descripcion"]);

                listaHotel.Add(h2);
            }
            dr.Close();

        }

        catch (Exception)
        {
            throw;
        }

        finally
        {
            if (cn != null && cn.State == ConnectionState.Open)
                cn.Close();
        }

        return listaHotel;
    }

    public static Hotel buscarPorId(int id, bool eliminados)
    {
        // procedimiento almacenado

        SqlConnection cn = new SqlConnection(GestorHotel.CadenaConexion);
        Hotel h = null;
        try
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cn;
            cmd.Parameters.Clear();
            cmd.Connection = cn;
            string sql = "";
            if (eliminados)
                sql = "select * from Hotel where id = @id AND eliminado=1";
            else
                sql = "select * from Hotel where id = @id AND (eliminado is NULL OR eliminado=0)";
            cmd.CommandText = sql;
            cmd.Parameters.Add(new SqlParameter("@id", id));
            //cmd.Parameters.Add(new SqlParameter("@eliminado", 0));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                h = new Hotel();
                h.id = (int)dr["id"];
                h.descripcion = dr["descripcion"].ToString();
                h.destino = (int)dr["destino"];
                h.capacidad = (int)dr["capacidad"];
                h.cuit = (long)dr["cuit"];
                h.aceptaMascota = (Boolean)dr["aceptaMascota"];
                h.inicioActividad = (DateTime)dr["inicioActividad"];
            }
            dr.Close();
        }

        catch (Exception)
        {
            throw;
        }

        finally
        {
            if (cn != null && cn.State == ConnectionState.Open)
                cn.Close();
        }

        return h;
    }

    public static void Eliminar(int id)
    {
        SqlConnection cn = new SqlConnection(GestorHotel.CadenaConexion);
        try
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cn;
            cmd.Parameters.Clear();
            cmd.Connection = cn;
            cmd.CommandText = @"update Hotel set eliminado=@eliminado where id=@id";
            cmd.Parameters.Add(new SqlParameter("@eliminado", 1));
            cmd.Parameters.Add(new SqlParameter("@id", id));
            //cmd.CommandType = CommandType.Text; // es necesario setear esta propiedad el valor por defecto es  CommandType.Text
            int filasAfetadas = cmd.ExecuteNonQuery();
            if (filasAfetadas == 0)
            {
                throw new Exception("El registro ya no existe en la base");
            }
        }

        catch (Exception)
        {
            throw;
        }

        finally
        {
            if (cn != null && cn.State == ConnectionState.Open)
                cn.Close();
        }
    }
    //NO GRABA MASCOTA
    public static void Grabar(Hotel h, Boolean accion)
    {
        string sql = "";
        SqlConnection cn = new SqlConnection(GestorHotel.CadenaConexion);
        //Hotel h1 = buscarPorId(h.id, false);
        if (accion)
                sql = @"insert  into Hotel (descripcion, capacidad, destino, cuit, aceptaMascota, eliminado, inicioActividad) values(@descripcion, @capacidad, @destino, @cuit, @aceptaMascota, @eliminado, @inicioActividad);";

        else
            sql = @"update Hotel set descripcion=@descripcion , capacidad=@capacidad, destino=@destino, cuit=@cuit, aceptaMascota=@aceptaMascota, eliminado=@eliminado, inicioActividad=@inicioActividad where id=@id;";

        try
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cn;
            cmd.Parameters.Clear();
            cmd.Connection = cn;
            cmd.CommandText = sql; //descripcion del procedimiento q debe ir a buscar
            cmd.Parameters.Add(new SqlParameter("@descripcion", h.descripcion));
            cmd.Parameters.Add(new SqlParameter("@capacidad", h.capacidad));
            cmd.Parameters.Add(new SqlParameter("@destino", h.destino.ToString()));
            cmd.Parameters.Add(new SqlParameter("@id", h.id));
            cmd.Parameters.Add(new SqlParameter("@cuit", h.cuit));
            cmd.Parameters.Add(new SqlParameter("@eliminado", DBNull.Value));
            cmd.Parameters.Add(new SqlParameter("@aceptaMascota", h.aceptaMascota));
            cmd.Parameters.Add(new SqlParameter("@inicioActividad", h.inicioActividad));
           
            int filasAfetadas = cmd.ExecuteNonQuery();
            if (filasAfetadas == 0)
            {
                throw new Exception();
            }

        }

        catch (Exception)
        {
            throw;
        }

        finally
        {
            if (cn != null && cn.State == ConnectionState.Open)
                cn.Close();
        }


    }

    //podria unirse con elmetodo  public static bool existe(int id)
    // verifica si existe un registro en la BD con ese cuit
    public static bool existeCuit(long cuit)
    {
       string sql = "";
        SqlConnection cn = new SqlConnection(GestorHotel.CadenaConexion);
        sql = @"SELECT  *  FROM  Hotel WHERE  cuit = @cuit;";
        try
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cn;
            cmd.Parameters.Clear();
            cmd.Connection = cn;
            cmd.CommandText = sql; //descripcion del procedimiento q debe ir a buscar

            cmd.Parameters.Add(new SqlParameter("@cuit", cuit));

            SqlDataReader dr = cmd.ExecuteReader();

            // con el resultado cargar una entidad
            while (dr.Read())
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            throw;
        }

        finally
        {
            if (cn != null && cn.State == ConnectionState.Open)
                cn.Close();
        }

    }

    public static DataTable ObtenerTodas()
    {
        SqlConnection cn = new SqlConnection(GestorHotel.CadenaConexion);
        DataTable dt = new DataTable();

        try
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.Connection = cn;
            cmd.Parameters.Clear();
            cmd.Connection = cn;
            cmd.CommandText = "select * from Hotel order by descripcion;";
            dt.Load(cmd.ExecuteReader());

        }
        catch (Exception)
        {
            throw;
        }

        finally
        {
            if (cn != null && cn.State == ConnectionState.Open)
                cn.Close();
        }

        return dt;
    }
}


