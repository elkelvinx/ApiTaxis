using Entidades;
using Entidades.Arrays;
using Entidades.cars;
using Entidades.ModelsCar;
using Servicios;
using Servicios.Relation;
using Servicios.Monolitico;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace TaxistTeodoro.Areas.api
{
    public class DataArrayController : ApiController
    {
        public HttpResponseMessage Get(string val)
        {
           
            var entidad2 =new listModelsCar();
            var entidad = new listArray();
            var response = Request.CreateResponse<IEnumerable<Arrays>>(System.Net.HttpStatusCode.Created, entidad);
            var response2 = Request.CreateResponse<IEnumerable<ModelsCar>>(System.Net.HttpStatusCode.Created, entidad2);
            ServicioArrayCR srv = new ServicioArrayCR();
            ServicioRelation serv = new ServicioRelation();
            switch (val)
            {
                case "brandCar":
                    entidad = srv.consultarBrandCar();
                     response = Request.CreateResponse<IEnumerable<Arrays>>(System.Net.HttpStatusCode.Created, entidad);
                    break;
                case "modelCar":
                    entidad2 = srv.consultarModelCar();
                     response = Request.CreateResponse<IEnumerable<ModelsCar>>(System.Net.HttpStatusCode.Created, entidad2);
                    break;
                case "adminName":
                 entidad = srv.consultarNombreAdmin();
                     response = Request.CreateResponse<IEnumerable<Arrays>>(System.Net.HttpStatusCode.Created, entidad);
                    break;
                case "driverName":
                    entidad = srv.consultarNombreDriver();
                    response = Request.CreateResponse<IEnumerable<Arrays>>(System.Net.HttpStatusCode.Created, entidad);
                    break;
                case "relationShip":
                    entidad = serv.consultaRelations();
                    response = Request.CreateResponse<IEnumerable<Arrays>>(System.Net.HttpStatusCode.Created, entidad);
                    break;
                case "status":
                    entidad = srv.consultarStatus();
                    response = Request.CreateResponse<IEnumerable<Arrays>>(System.Net.HttpStatusCode.Created, entidad);
                    break;
                case "insuranceName":
                    entidad = srv.consultarNombreInsurance();
                    response = Request.CreateResponse<IEnumerable<Arrays>>(System.Net.HttpStatusCode.Created, entidad);
                    break;
                case "sinisterType":
                    entidad = srv.consultarTypeInsurance();
                    response = Request.CreateResponse<IEnumerable<Arrays>>(System.Net.HttpStatusCode.Created, entidad);
                    break;
            }           
            return response;

        }
        public string Post(string metodo, Arrays entidad)
        {
            Arrays data = new Arrays();
            data.name = entidad.name;
            var srv = new ServicioArrayCR();
            var srelation = new ServicioRelation();
            string res = "ok";
            try
            {
                switch (metodo)
                {
                    case "relationShip":
                        res = srelation.insertar(data);
                        break;
                    case "status":
                        res = srv.insertarStatus(data);
                        break;

                    case "sinisterType":
                        res = srv.insertarSinisterType(data);
                        break;
                    case "insuranceName":
                        res = srv.insertarInsuranceName(data);
                        break;
                }

            }
            catch (Exception ex) { res = ex.ToString(); }
            return res;
        }

        public string Put(string metodo, Arrays data)
        {
            var srv = new ServicioArrayUD();
            var srelation = new ServicioRelation();
            string res = "ok";
            try
            {
                switch(metodo)
                {
                    case "relationShip":
                            res = srelation.actualizar(data);
                        break;
                    case "status":
                        res = srv.ActualizarStatus(data);
                        break;
                   
                    case "sinisterType":
                        res = srv.ActualizarSinisterType(data);
                        break;
                    case "insuranceName":
                        res = srv.ActualizarInsuranceName(data);
                        break;
                }    

            }
            catch (Exception ex) { res = ex.ToString(); }
            return res;
        }
        public string Delete(String metodo,int id)
        {
            var res = "ok";
            var srv = new ServicioArrayUD();
            var srelation = new ServicioRelation();
            try
            {
                switch (metodo)
                {
                    case "relationShip":
                        res = srelation.eliminar(id);
                        break;
                    case "status":
                        res = srv.eliminarStatus(id);
                        break;

                    case "sinisterType":
                        res = srv.eliminarSinisterType(id);
                        break;
                    case "insuranceName":
                        res = srv.eliminarInsuranceName(id);
                        break;
                }

            }
            catch (Exception ex) { res = ex.ToString(); }
            return res;
        }
    }
}
