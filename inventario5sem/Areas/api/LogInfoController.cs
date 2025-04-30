using Entidades;
using Entidades.Arrays;
using Entidades.LogIn;
using Entidades.Logs;
using Servicios;
using Servicios.Logs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static Entidades.Logs.historyLogIn;

namespace TaxistTeodoro.Areas.api
{
    public class LogInfocontroller : ApiController
    {
        private readonly ServicioHistoryLogIn _historyLogIn;
        private readonly ServicioChangeLog _changeLog;
        private readonly ServicioErrorLogs _errorLog;
        public LogInfocontroller()
        {
            _historyLogIn = new ServicioHistoryLogIn();
            _changeLog= new ServicioChangeLog();
            _errorLog= new ServicioErrorLogs();
        }
        [Route("api/login/historyLogIn")]
        public HttpResponseMessage GetHistoryLogIn()
        {
            var info = new ApiResponse<List<historyLogIn>>();
            try
            {
                info = _historyLogIn.GetAll();
                if (!info.Success)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, info.ErrorMessage);
                }
                var response = Request.CreateResponse<IEnumerable<historyLogIn>>(System.Net.HttpStatusCode.OK, info.Data);
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
        [Route("api/login/changeLog")]
        public HttpResponseMessage GetChangeLog()
        {
            var info = new ApiResponse<listChangeLogs>();
            try
            {
                info = _changeLog.GetAllNuevo();
                if (!info.Success)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, info.ErrorMessage);
                }
                var response = Request.CreateResponse<IEnumerable<ChangeLog>>(HttpStatusCode.OK, info.Data);
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [Route("api/login/errorLog")]
        public HttpResponseMessage GetErrorLog()
        {
            var info = new ApiResponse<listErrorLogs>();
            try
            {
                info = _errorLog.GetAll();
                if (!info.Success)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, info.ErrorMessage);
                }
                var response = Request.CreateResponse<IEnumerable<ErrorLog>>(HttpStatusCode.OK, info.Data);
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
