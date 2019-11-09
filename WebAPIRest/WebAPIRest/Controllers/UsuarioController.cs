using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIRest.Models;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebAPIRest.Controllers
{
    [RoutePrefix("api/usuario")] // Aqui é definido o caminho do webservice, ou seja onde ficarão os metodos CRUD 
                                // ex: http://localhost:port/api/usuario...
    public class UsuarioController : ApiController // Define as propriedades e os metodos padrões de uma API
    {
        private static HashSet<UsuarioModel> listaUsuarios = new HashSet<UsuarioModel>(); //Lista estática para inserção dos dados dos
                                                                                    //usuarios

          


        //HttpPost
        [AcceptVerbs("POST")] //AcceptVerbs define qual metodo de requisição http será utilizado
        [Route("CadastrarUsuario")] //Route define a rota do metodo e os parametros, caso haja
        public string CadastrarUsuario(UsuarioModel usuario) // Método para cadastrar usuario, ele recebe um usuario
        {
            foreach (UsuarioModel x in listaUsuarios) //Foreach para percorrer a lista toda
            {
                if (usuario.Codigo == x.Codigo) // Aqui um IF  para verificar em cada item da lista se o código existe
                {
                    return ("Usuario não permitido"); // Caso ja exista, não é inserido e retorna uma mensagem de erro

                }else if (usuario.Login.Equals(x.Login)) // Metodo if para verificar em cada item da lista se o Login ja existe
                {                                        // na lista

                    return ("Usuario não permitido"); // Caso ja exista também, não cadastra e retorna um erro
                }
            }
                listaUsuarios.Add(usuario); // Se não houver impedimentos, cadastra o usuario e retorna uma mensagem de sucesso
                return "Usuário cadastrado com sucesso!";
        }

        //HttpPost
        [AcceptVerbs("POST")] //AcceptVerbs define qual metodo de requisição jhttp será utilizado
        [Route("CadastrarUsuarioBanco")] //Route define a rota do metodo e os parametros, caso haja
        public string CadastrarUsuarioBanco(UsuarioModel usuario) // Método para cadastrar usuario, ele recebe um usuario
        {
            MySqlConnection connection = WebApiConfig.connecting(); // Metodo para realizar a conexão no banco de Dados, criado na class API.Config

            try
            {
                //abre a conexão
                connection.Open();

                //Comando para inserir dados no banco
                MySqlCommand objCmd = new MySqlCommand("insert into usuarios (Codigo, Nome, Login) values(null, ?, ?)", connection);

                objCmd.Parameters.Clear(); // Limpa Os parametros ( ?, ? )

                objCmd.Parameters.Add("@Nome", MySqlDbType.VarChar, 50).Value = usuario.Nome; // Adiciona o valor do parametro para Nome
                objCmd.Parameters.Add("@Login", MySqlDbType.VarChar, 35).Value = usuario.Login; // Adiciona o valor do parametro para Login

                // Executar a query, ou seja o comando
                objCmd.ExecuteNonQuery();

                connection.Close(); // Fecha a conexão
            }
            catch (Exception erro)
            {
                throw erro ;
            }

            return "Usuário cadastrado com sucesso!";
        }

        //HttpPut
        [AcceptVerbs("PUT")]
        [Route("AlterarUsuario")]
        public string AlterarUsuario(UsuarioModel usuario) {

            foreach (UsuarioModel x in listaUsuarios) //Foreach para percorrer a lista toda
            {
                if (usuario.Codigo == x.Codigo) // Aqui um IF  para verificar em cada item da lista se o código existe
                {
                    listaUsuarios.Where(n => n.Codigo == usuario.Codigo) 
                .Select(s =>
                {                                // Caso exista, ele altera os dados
                   s.Codigo = usuario.Codigo;
                   s.Login = usuario.Login;
                   s.Nome = usuario.Nome;

                   return s;
               }).ToList(); // Reenvia os dados para a lista

                    return "Usuario alterado com sucesso!"; // mensagem de sucesso, se tudo certo
                }
            }

                return "Codigo não encontrado"; //caso o código não for encontrado ele retorna um erro

             

        }

        //HttpPut
        [AcceptVerbs("PUT")]
        [Route("AlterarUsuarioBanco")]
        public string AlterarUsuarioBanco(UsuarioModel usuario)
        {
            MySqlConnection connection = WebApiConfig.connecting(); // Inicia a conexão 
            
            try
            {
                //abre a conexão
                connection.Open();

                MySqlCommand objCmd = new MySqlCommand("UPDATE usuarios SET Nome = ?, Login = ? WHERE Codigo = ?", connection);
                objCmd.Parameters.Clear();

                objCmd.Parameters.Add("@Nome", MySqlDbType.VarChar, 50).Value = usuario.Nome;
                objCmd.Parameters.Add("@Login", MySqlDbType.VarChar, 35).Value = usuario.Login;
                objCmd.Parameters.Add("@Codigo", MySqlDbType.Int32).Value = usuario.Codigo;

                objCmd.CommandType = CommandType.Text;
                objCmd.ExecuteNonQuery();



                //fecha a conexão
                connection.Close();

                return "Dados alterados com sucesso!";

            }catch (Exception erro)
            {
                return "Falha ao Atualizar!" + erro;
            }

        }

        //HttpDelete
        [AcceptVerbs("DELETE")]
        [Route("ExcluirUsuario/{codigo}")] // URI, trazendo o parametro
        public string ExcluirUsuario(int codigo) {
            foreach(UsuarioModel x in listaUsuarios) //Foreach para percorrer a lista toda
            {
                if(x.Codigo == codigo) // Aqui um IF  para verificar em cada item da lista se o código existe
                                       // caso encontrado, ele executa a função de apagar o registro
                {
                    UsuarioModel usuario = listaUsuarios.Where(n => n.Codigo == codigo)
                .Select(n => n).First();

                    listaUsuarios.Remove(usuario); //remove o registro da lista

                    return "Registro excluido com sucesso!";

                }
            }

            return "Registro não encontrado!"; //Se o código não for encontrado ele retorna um erro 

        }

        //HttpDelete
        [AcceptVerbs("DELETE")]
        [Route("ExcluirUsuarioBanco/{codigo}")]
        public string ExcluirUsuarioBanco(int codigo)
        {
            MySqlConnection connection = WebApiConfig.connecting();

            try
            {
                connection.Open();
                MySqlCommand objCmd = new MySqlCommand("Delete from usuarios where Codigo = ?", connection);
                objCmd.Parameters.Clear();
                objCmd.Parameters.Add("@Codigo", MySqlDbType.Int32).Value = codigo;

                objCmd.CommandType = CommandType.Text;
                objCmd.ExecuteNonQuery();
                connection.Close();


            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                throw;

            };


            return "Excluido com sucesso!"; //Se o código não for encontrado ele retorna um erro 

        }

        //HttpGet
        [AcceptVerbs("GET")]
        [Route("ConsultarUsuarioPorCodigoBanco/{codigo}")]
        public HashSet<UsuarioModel> ConsultarUsuarioPorCodigo(int codigo) { // recebe um código por parametro

            MySqlConnection connection = WebApiConfig.connecting();

            try
            {
                connection.Open(); //Abre o Banco de Dados

                //Comando do MySQL com os devidos parametros
                MySqlCommand objCmd = new MySqlCommand("SELECT Codigo, Nome, Login FROM usuarios WHERE Codigo = ?", connection);
                objCmd.Parameters.Clear();
                objCmd.Parameters.Add("@Codigo", MySqlDbType.Int32).Value = codigo;

                // Executa o comando
                objCmd.CommandType = CommandType.Text;

                //Recebe o conteudo buscado no banco
                MySqlDataReader dr;

                dr = objCmd.ExecuteReader();

                // Faz a leitura do que veio, para retornar
                while(dr.Read())
                {
                    listaUsuarios.Add(new UsuarioModel(dr.GetInt32("Codigo"), dr["Nome"].ToString(), dr["Login"].ToString()));
                }
                               
            }catch (Exception)
            {
                throw;
            }

            return listaUsuarios;
        }

        //HttpGet
        [AcceptVerbs("GET")]
        [Route("ConsultarUsuarios")]
        public HashSet<UsuarioModel> ConsultarUsuarios() {

            return listaUsuarios; // Retorna todos os usuarios cadastrados
        }

        //HttpGet
        [AcceptVerbs("GET")]
        [Route("ConsultarUsuariosTotal")]
        public HashSet<UsuarioModel> ConsultarUsuariosTotal()
        {
            MySqlConnection connection = WebApiConfig.connecting();

            MySqlCommand query = connection.CreateCommand();

            query.CommandText = "SELECT Codigo, Nome, Login FROM usuarios";

            try
            {
                connection.Open();

            }catch (MySql.Data.MySqlClient.MySqlException)
            {
                throw;

            }

            MySqlDataReader fetch_query = query.ExecuteReader();

            while (fetch_query.Read())
            {
                listaUsuarios.Add(new UsuarioModel(fetch_query.GetInt32("Codigo"), fetch_query["Nome"].ToString(), fetch_query["Login"].ToString()));

            }

            return listaUsuarios;

        }

    }
}
