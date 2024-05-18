using Entidades;
using Entidades.cars;
using Servicios;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class BrandController : ApiController
    {
        // GET: api/Street
        public HttpResponseMessage Get()
        {
            var entidad = new listBrand();
            var srv = new ServicioCar();
            entidad = srv.consultarBrands();
            var response = Request.CreateResponse<IEnumerable<Brand>>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }
        public string Post(Brand obj)
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
        public string Put(Brand obj)
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
                srv.eliminar(id);
            }
            catch (Exception ex) { res = ex.ToString(); }
            return res;
        }

    }
}
