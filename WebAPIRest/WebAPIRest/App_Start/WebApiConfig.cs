using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MySql.Data.MySqlClient;

namespace WebAPIRest
{
    public static class WebApiConfig
    {
        public static MySqlConnection connecting()
        {
            string conect = "server=localhost;port=3306;database=Usuarios;username=root;password=!@#Samsamuel200;";

            MySqlConnection connecting = new MySqlConnection(conect);

            return connecting;
        }
        public static void Register(HttpConfiguration config)
        {
            // Serviços e configuração da API da Web

            // Rotas da API da Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.Indent = true;
        }
    }
}
