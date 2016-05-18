﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Descripción breve de GestorHotel
/// una masa Jumbotronn
/// </summary>
public class GestorHotel
{   //TODO aca instanciar la BD
    //ACÁ SE INSTANCIA LA CADENA DE CONEXIÓN, UNA ÚNICA VEZ
    public static string CadenaConexion = @"Data Source=NICO-PC\sqlexpress;Integrated Security=SSPI;Initial Catalog=4K1_62726";
    //public static string CadenaConexion = @"Data Source=DAVID-PC\SQLEXPRESS;Initial Catalog=aaa;Integrated Security=True";
    //public static string CadenaConexion = "Data Source=MAQUIS;Initial Catalog=4K1_62726;User ID=avisuales2;Password=avisuales2";


    public static List<Hotel> BuscarPordescripcion(string descripcion, string orden)
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
            cmd.CommandText = " select h.id,h.descripcion,h.capacidad,d.descripcion destino_descripcion from Hotel h left join Destino d on h.destino = d.id where h.eliminado is NULL AND h.descripcion like @descripcion order by " + orden;
            cmd.Parameters.Add(new SqlParameter("@descripcion", "%" + descripcion + "%"));
            //cmd.CommandType = CommandType.Text; // es necesario setear esta propiedad el valor por defecto es  CommandType.Text
            SqlDataReader dr = cmd.ExecuteReader();

            // con el resultado cargar una entidad
            while (dr.Read())
            {

                h2 = new Hotel();
                h2.descripcion = (string)dr["descripcion"];
                h2.descripcion = dr["descripcion"].ToString();
                h2.id = (int)dr["id"];
                // esto para cada atributo que acepte valores nulos
                if (dr["capacidad"] != DBNull.Value)
                {
                    h2.capacidad = (int)(dr["capacidad"]);
                }
                if (dr["destino_descripcion"] != DBNull.Value)
                {
                    h2.destino_descripcion = (string)(dr["destino_descripcion"]);
                }
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

    public static Hotel buscarPorId(int id)
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
            cmd.CommandText = "select * from Hotel where id = @id";
            cmd.Parameters.Add(new SqlParameter("@id", id));

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                h = new Hotel();
                h.id = (int)dr["id"];
                h.descripcion = dr["descripcion"].ToString();
                h.destino = (int)dr["destino"];
                h.capacidad = (int)dr["capacidad"];
                h.cuit = (int)dr["cuit"];
                h.aceptaMascota = (Boolean)dr["aceptaMascota"];
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
        Hotel h1 = buscarPorId(h.id);
        if (accion)
            if (h.id == -1)//que grabe de forma autonumérica. 
                sql = @"insert  into Hotel (descripcion, capacidad, destino, cuit, aceptaMascota, eliminado) values(@descripcion, @capacidad, @destino, @cuit, @aceptaMascota, @eliminado);";
            else //que grabe imponiendo un id
                sql = @"set identity_insert dbo.Hotel on 
                                  insert  into Hotel (id, descripcion, capacidad, destino, cuit, aceptaMascota, eliminado) values(@id, @descripcion, @capacidad, @destino, @cuit, @aceptaMascota, @eliminado)
                                  set identity_insert dbo.Hotel off";


        else
            sql = @"update Hotel set descripcion=@descripcion , capacidad=@capacidad, destino=@destino, cuit=@cuit, aceptaMascota=@aceptaMascota, eliminado=@eliminado where id=@id;";

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
            cmd.Parameters.Add(new SqlParameter("@aceptaMascota", h.aceptaMascota));
            cmd.Parameters.Add(new SqlParameter("@eliminado", DBNull.Value));
            //cmd.CommandType = CommandType.Text; // es necesario setear esta propiedad el valor por defecto es  CommandType.Text
            int filasAfetadas = cmd.ExecuteNonQuery();
            if (filasAfetadas == 0)
            {
                throw new Exception("El registro ya existe en la base");
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

    public static bool existe(int id)
    {
        SqlConnection cn = new SqlConnection(GestorHotel.CadenaConexion);
        try
        {
            cn.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.Parameters.Clear();
            cmd.Connection = cn;
            cmd.CommandText = @"select * from Hotel where id=@id";
            cmd.Parameters.Add(new SqlParameter("@id", id));

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
                return true;
            else
                return false;
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
    }
}


