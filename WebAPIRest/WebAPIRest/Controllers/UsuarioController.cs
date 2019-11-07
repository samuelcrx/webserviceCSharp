using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIRest.Models;

namespace WebAPIRest.Controllers
{
    [RoutePrefix("api/usuario")] // Aqui é definido o caminho do webservice, ou seja onde ficarão os metodos CRUD 
                                // ex: http://localhost:port/api/usuario...
    public class UsuarioController : ApiController // Define as propriedades e os metodos padrões de uma API
    {
        private static List<UsuarioModel> listaUsuarios = new List<UsuarioModel>(); //Lista estática para inserção dos dados dos
                                                                                    //usuarios


        //HttpPost
        [AcceptVerbs("POST")] //AcceptVerbs define qual metodo de requisição jhttp será utilizado
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

        //HttpDelete
        [AcceptVerbs("DELETE")]
        [Route("ExcluirUsuario/{codigo}")]
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

        //HttpGet
        [AcceptVerbs("GET")]
        [Route("ConsultarUsuarioPorCodigo/{codigo}")]
        public UsuarioModel ConsultarUsuarioPorCodigo(int codigo) { // recebe um código por parametro

            UsuarioModel usuario = listaUsuarios.Where(n => n.Codigo == codigo)
                .Select(n => n)
                .FirstOrDefault(); //Procura o usuario que possui o código passado por parametro

            return usuario; //Retorna os dados do usuario, se encontrado, se não retorna null
        }

        //HttpGet
        [AcceptVerbs("GET")]
        [Route("ConsultarUsuarios")]
        public List<UsuarioModel> ConsultarUsuarios() {

            return listaUsuarios; // Retorna todos os usuarios cadastrados
        }

    }
}
