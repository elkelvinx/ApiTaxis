using Entidades;
using Entidades.LogIn;
using Servicios;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class SettlementController : ApiController
    {
        // GET: api/Cliente

        public HttpResponseMessage Get()
        {
            var entidad = new listSettle();
            ServicioSettlement srv = new ServicioSettlement();
            entidad = srv.consultarColonias();
            var response = Request.CreateResponse<IEnumerable<settlement>>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }
        public HttpResponseMessage Get(string val)
        {
            var entidad = new listSettle();
            ServicioSettlement srv = new ServicioSettlement();
            if (val.Equals("n"))
            {
                entidad = srv.consultarNombre();
            }

            var response = Request.CreateResponse<IEnumerable<settlement>>(System.Net.HttpStatusCode.Created, entidad);
            return response;

        }
        public string Post(UserData obj)
        {
            var srv = new ServicioUser();
            string respuesta = "ok";
            try
            {
                respuesta = srv.Insertar(obj);
            }
            catch (Exception ex) { respuesta = ex.ToString(); }


            return respuesta;
        }
        public string Put(settlement obj)
        {
            var srv = new ServicioSettlement();
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
            var srv = new ServicioSettlement();
            try
            {
                srv.eliminar(id);
            }
            catch (Exception ex) { res = ex.ToString(); }
            return res;
        }
    }
}
