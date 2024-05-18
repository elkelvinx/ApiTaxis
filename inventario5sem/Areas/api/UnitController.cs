using Entidades;
using Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class UnitController : ApiController
    {
        // GET: api/Admin
        
        public HttpResponseMessage Get()    
        {
            var entidad = new listUnit();
            var srv = new ServicioUnit();
            entidad = srv.consultarUnits();
            var response = Request.CreateResponse<IEnumerable<Unit>>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }
        
        // GET: api/Admin/5
        public HttpResponseMessage Get(int id)
        {
            var entidad = new Unit();
            var srv = new ServicioUnit();
            entidad = srv.consultarUnit(id);
            var response = Request.CreateResponse<Unit>(System.Net.HttpStatusCode.Created, entidad);
            return response;
        }

        // POST: api/Admin

     
        public string Post(Unit ent)
        {
            var srv = new ServicioUnit();
            string respuesta = "ok";
            try
            {
                respuesta = srv.insertar(ent);
            }
            catch(Exception ex) { respuesta = ex.ToString(); }
           
            
            return respuesta;
        }


        // PUT: api/Admin/5
        public string Put(Unit unit)
        {
            var srv = new ServicioUnit();
            string res = "ok";
            try 
            {
                res = srv.Actualizar(unit);
            }
            catch (Exception ex) { res = ex.ToString(); }
            return res;
        }

        // DELETE: api/Admin/5
        public string Delete(int id)
        {
            var res = "ok";
            var srv = new ServicioAdmin();
            try
            {
                srv.eliminar(id);
            }
            catch(Exception ex) { res = ex.ToString(); }
            return res;
        }

    }
}
