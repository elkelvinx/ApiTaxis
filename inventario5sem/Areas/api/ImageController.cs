using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Servicios;
using System;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace TaxisTeodoro.Areas.api
{
    public class ImageController : ApiController
    {
        [HttpPost]
        [Route("api/upload/document")]
        public async Task<IHttpActionResult> UploadDocument()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;

                // Validar archivo
                if (httpRequest.Files.Count == 0)
                    return BadRequest("No file uploaded.");

                var file = httpRequest.Files[0];
                if (file == null || file.ContentLength == 0)
                    return BadRequest("Empty file.");

                // Datos extra desde form-data
                var entityType = httpRequest.Form["EntityType"];
                var entityId = httpRequest.Form["EntityId"];
                var documentTypeCode = httpRequest.Form["DocumentTypeCode"];
                var description = httpRequest.Form["Description"] ?? "";

                // Configuración desde web.config
                var connectionString = ConfigurationManager.AppSettings["AzureBlobConnection"];
                var containerName = ConfigurationManager.AppSettings["AzureBlobContainer"];

                // Subir archivo a Azure Blob
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();

                var originalFileName = Path.GetFileName(file.FileName);
                var extension = Path.GetExtension(originalFileName);
                var storedFileName = Guid.NewGuid().ToString() + extension; // nombre único
                var blockBlob = container.GetBlockBlobReference(storedFileName);

                using (var stream = file.InputStream)
                {
                    await blockBlob.UploadFromStreamAsync(stream);
                }

                var fileUrl = blockBlob.Uri.ToString();


                // Guardar metadata en BD


                using (SqlConnection conn = ServiciosBD.ObtenerConexion())
                {
                    await conn.OpenAsync();
                    var query = @"
                        INSERT INTO Documents 
                        (OriginalFileName, StoredFileName, FilePath, FileType, DocumentTypeCode, Description, FileSize, UploadDate, UploadedBy, RelatedEntityType, RelatedEntityId) 
                        VALUES (@OriginalFileName, @StoredFileName, @FilePath, @FileType, @DocumentTypeCode, @Description, @FileSize, GETDATE(), @UploadedBy, @RelatedEntityType, @RelatedEntityId);
                        SELECT SCOPE_IDENTITY();
                    ";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@OriginalFileName", originalFileName);
                        cmd.Parameters.AddWithValue("@StoredFileName", storedFileName);
                        cmd.Parameters.AddWithValue("@FilePath", fileUrl);
                        cmd.Parameters.AddWithValue("@FileType", extension.TrimStart('.'));
                        cmd.Parameters.AddWithValue("@DocumentTypeCode", documentTypeCode);
                        cmd.Parameters.AddWithValue("@Description", description);
                        cmd.Parameters.AddWithValue("@FileSize", file.ContentLength);
                        cmd.Parameters.AddWithValue("@UploadedBy", "admin"); // luego puedes usar el usuario real
                        cmd.Parameters.AddWithValue("@RelatedEntityType", entityType);
                        cmd.Parameters.AddWithValue("@RelatedEntityId", string.IsNullOrEmpty(entityId) ? (object)DBNull.Value : Convert.ToInt32(entityId));

                        var insertedId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        return Ok(new
                        {
                            DocumentId = insertedId,
                            Url = fileUrl,  
                            Message = "File uploaded successfully"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                
                return InternalServerError(ex);
            }
        }
    }
}
