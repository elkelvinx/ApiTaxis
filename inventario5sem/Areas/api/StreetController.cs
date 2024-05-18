using Entidades;
using Entidades.Arrays;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class StreetController : ApiController
    {
        // GET: api/Street

        public HttpResponseMessage Get()
        {
            var entidad = new listStreet();
            ServicioStreet srv = new ServicioStreet();
            entidad = srv.consultarCalles();
            var response = Request.CreateResponse<IEnumerable<Street>>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }
        public HttpResponseMessage Get(string val)
        {
            var entidad = new listStreet();
            ServicioStreet srv = new ServicioStreet();
            if (val.Equals("n"))
            {
                entidad = srv.consultarNombre();
            }

            var response = Request.CreateResponse<IEnumerable<Street>>(System.Net.HttpStatusCode.Created, entidad);
            return response;

        }
        public string Post(Street obj)
        {
            var srv = new ServicioStreet();
            string respuesta = "ok";
            try
            {
                respuesta = srv.insertar(obj);
            }
            catch (Exception ex) { respuesta = ex.ToString(); }


            return respuesta;
        }
        public string Put(Street obj)
        {
            var srv = new ServicioStreet();
            string res = "ok";
            try
            {
                res = srv.Actualizar(obj);
            }
            catch (Exception ex) { res = ex.ToString(); }
            return res;
        }
        public string Delete(int id)
        {
            var res = "ok";
            var srv = new ServicioStreet();
            try
            {
                srv.eliminar(id);
            }
            catch (Exception ex) { res = ex.ToString(); }
             return res;
        }

    }
}
