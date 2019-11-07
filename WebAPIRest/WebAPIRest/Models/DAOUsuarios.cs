using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;


namespace WebAPIRest.Models
{
    public class DAOUsuarios
    {
        string conectionDB = "SERVER=localhost; DATABASE=Usuarios; UID=root; PWD=!@#Samsamuel200";

        MySqlConnection conexao = null;
        MySqlCommand comando;

        public DataTable ExibirDados()
        {
            try
            {
                conexao = new MySqlConnection(conectionDB);
                comando = new MySqlCommand("SELECT * FROM usuarios", conexao);

                MySqlDataAdapter Da = new MySqlDataAdapter();
                Da.SelectCommand = comando;

                

            }catch(Exception e)
            {
                throw e;
            }
            return null;
        }
    }
}