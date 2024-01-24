using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.DTO.DTO_Identity;
using ND2Assignwork.API.Models.Service;
using ND2Assignwork.API.Models.Service.Imp;
using System.Net.Mime;

namespace ND2Assignwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        public FileController(IFileService fileService, IServiceScopeFactory serviceScopeFactory)
        {
            this._fileService = fileService;
            //_timer = new Timer(DeleteFileNotConnect, null, TimeSpan.Zero, TimeSpan.FromHours(24));
            _scopeFactory = serviceScopeFactory;

        }
        
        // GET ALL FILE
        /*[HttpGet, Authorize(Roles = "SuperAdmin")]
        public IActionResult GetAll()
        {
            return Ok(_fileService.GetAll());
        }*/


        [HttpGet("GetFileById/{fileId}")]
        public async Task<IActionResult> GetFileDocInById([FromRoute] string fileId)
        {
            var fileDTO = await _fileService.GetFileById(fileId);
            if (fileDTO == null)
            {
                return BadRequest("Không tìm thấy File !");
            }

            string fileName = fileDTO.File_Name;

            ContentDisposition contentDisposition = new ContentDisposition
            {
                FileName = fileName,
                Inline = false,
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            string contextType = fileDTO.ContentType.ToString();
            var stream = new MemoryStream(fileDTO.File_Data);
            return File(stream, contextType);
        }



        /*[HttpGet("GetFileDocIn/{fileId}")]
        public IActionResult GetFileDocInById([FromRoute] string fileId)
        {
            string userId = User.Identity.Name;
            var fileDTO = _fileService.GetFileDocInById(fileId, userId);
            if (fileDTO == null)
            {
                return BadRequest("Không tìm thấy File !");
            }

            string fileName = fileDTO.File_Name;

            ContentDisposition contentDisposition = new ContentDisposition
            {
                FileName = fileName,
                Inline = false,
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            string contextType = fileDTO.ContentType.ToString();
            var stream = new MemoryStream(fileDTO.File_Data);
            return File(stream, contextType);
        }


        [HttpGet("GetFileDocSend/{fileId}")]
        public IActionResult GetFileDocSendById([FromRoute] string fileId)
        {
            string userId = User.Identity.Name;
            var fileDTO = _fileService.GetFileDocSendById(fileId, userId);
            if (fileDTO == null)
            {
                return BadRequest("Không tìm thấy File !");
            }

            string fileName = fileDTO.File_Name;

            ContentDisposition contentDisposition = new ContentDisposition
            {
                FileName = fileName,
                Inline = false,
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            string contextType = fileDTO.ContentType.ToString();
            var stream = new MemoryStream(fileDTO.File_Data);
            return File(stream, contextType);
        }


        [HttpGet("GetFileTask/{fileId}")]
        public IActionResult GetFileTaskById([FromRoute] string fileId)
        {
            string userId = User.Identity.Name;
            var fileDTO = _fileService.GetFileTaskById(fileId, userId);
            if (fileDTO == null)
            {
                return BadRequest("Không tìm thấy File !");
            }

            string fileName = fileDTO.File_Name;

            ContentDisposition contentDisposition = new ContentDisposition
            {
                FileName = fileName,
                Inline = false,
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            string contextType = fileDTO.ContentType.ToString();
            var stream = new MemoryStream(fileDTO.File_Data);
            return File(stream, contextType);
        }*/
        ////////////////////////////////////////////////////////////////////////

        /*[HttpGet("GetFileDocSend/{fileId}")]
        public IActionResult GetFileDocSendById([FromRoute] string fileId)
        {
            string userId = User.Identity.Name;
            var fileDTO = _fileService.GetFileDocSendById(fileId, userId);
            if (fileDTO == null)
            {
                return BadRequest("Không tìm thấy File !");
            }

            string fileName = fileDTO.File_Name;

            ContentDisposition contentDisposition = new ContentDisposition
            {
                FileName = fileName,
                Inline = false,
            };
            Response.Headers.Add("Content-Disposition", contentDisposition.ToString());

            string contextType = fileDTO.ContentType.ToString();
            var stream = new MemoryStream(fileDTO.File_Data);
            return File(stream, contextType);
        }*/


        /*[HttpPost]
        public IActionResult CreateFiles([FromForm] List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var fileContent = new FileContent();
                var dto = fileContent.ToFile(file);
                if (!_fileService.Create(dto))
                    return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
            }
            return Ok("Tạo file thành công!");
        }


        [HttpPut("{id}")]
        public IActionResult UpdateFile([FromRoute] string id, [FromBody] FileDTO fileDTO)
        {
            if (id != fileDTO.File_Id)
                return BadRequest("Vui lòng nhập đúng id !");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_fileService.Update(fileDTO))
                return Ok(fileDTO);
            return BadRequest("Lỗi khi cập nhật file !");
        }*/

        /*[HttpDelete("{id}")]
        public IActionResult DeleteFile([FromRoute] string id)
        {
            string userId = User.Identity.Name;
            var fileDTO = _fileService.GetFileByDocId(id);
            if (fileDTO == null)
            {
                return BadRequest("File không tồn tại !");
            }
            if (_fileService.Delete(id))
            {
                return BadRequest("Đã xóa file thành công !");
            }
            else
            {
                return BadRequest("Lỗi khi xóa file !");
            }
        }*/
        [HttpGet("GetListFileNotConnect"), Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> GetFileNotConnect()
        {
            var file = await _fileService.FileNotConect();
            return Ok(file.Select(f => new
            {
                f.File_Id,
                f.File_Name,
                f.ContentType
            }).ToList());
        }
        [HttpDelete("DeleteListFileNotConnect"), Authorize(Roles = "SuperAdmin")]
        public IActionResult DeleteFileNotConnect()
        {
            return Ok("Công việc xóa file không kết nối sẽ được thực hiện tự động theo lịch trình!");
        }

        // Auto xóa file rác
        /*private void DeleteFileNotConnect(object state)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                context.File.RemoveRange(context.File
                .Where(file => !context.Document_Incomming_File.Any(di => di.File_Id == file.File_Id) &&
                               !context.Document_Send_File.Any(ds => ds.File_Id == file.File_Id) &&
                               !context.Task_File.Any(tf => tf.File_Id == file.File_Id))
                .ToList());

                if (!fileIds.Any())
                {
                    Console.WriteLine("No file not connect !");
                }
                else
                {
                    foreach (var id in fileIds)
                    {
                        var fileEntity = context.File.Find(id.File_Id);
                        context.File.Remove(fileEntity);
                    }
                    int recordsAffected = context.SaveChanges();
                    if (recordsAffected > 0)
                    {
                        Console.WriteLine("Xóa File Not Connect thành công!");
                    }
                    Console.WriteLine("Lỗi khi xóa File Not Connect!");
                }
            }
            
        }*/
    }
}
