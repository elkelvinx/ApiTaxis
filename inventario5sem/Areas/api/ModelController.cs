using Entidades;
using Entidades.Cars;
using Servicios;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Data.SqlClient;

namespace TaxistTeodoro.Areas.api
{
    //[Authorize]
    public class ModelController : ApiController
    {
        // GET: api/Street
        public HttpResponseMessage Get()
        {
            var entidad = new listModelsCar();
            var srv = new ServicioCar();
            entidad = srv.consultarModels();
            var response = Request.CreateResponse<IEnumerable<ModelsCar>>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }
        public string Post(ModelsCar obj)
        {
            var srv = new ServicioCar();
            string respuesta = "ok";
            try
            {
                respuesta = srv.insertar(obj);
            }
            catch (Exception ex) { respuesta = ex.ToString(); }
            return respuesta;
        }
        public string Put(ModelsCar obj)
        {
            var srv = new ServicioCar();
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
            var srv = new ServicioCar();
            try
            {
                srv.eliminarModel(id);
            }
            catch (Exception ex) { res = ex.ToString(); }
            return res;
        }

    }
}
