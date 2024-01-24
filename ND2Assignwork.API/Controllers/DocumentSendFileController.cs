using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.Service;

namespace ND2Assignwork.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController, Authorize]
    public class DocumentSendFileController : ControllerBase
    {
        private readonly IDocumentSendFileService _DocSendFileService;
        public DocumentSendFileController(IDocumentSendFileService documentSendFile)
        {
            _DocSendFileService = documentSendFile;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_DocSendFileService.GetAllDocumentSendFile());
        }

        //[HttpGet("GetByDocId/{doc_id}")]
        public IActionResult GetById(string doc_id)
        {
            var documentSendFileDTOs = _DocSendFileService.GetDocSendFileByDocId(doc_id);
            if (documentSendFileDTOs == null)
            {
                return NotFound();
            }
            return Ok(documentSendFileDTOs);
        }
        // GET: 
        //[HttpGet("{file_id}/{doc_id}")]
        public IActionResult GetById(string file_id, string doc_id)
        {
            var documentSendFileDTO = _DocSendFileService.GetOneDocSendFile(doc_id, file_id);
            if (documentSendFileDTO == null)
            {
                return NotFound();
            }
            return Ok(documentSendFileDTO);
        }

        // POST:
        [HttpPost]
        public IActionResult CreateDocSendFile([FromBody] Document_Send_FileDTO documentSendFileDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_DocSendFileService.CreateDocSendFile(documentSendFileDTO))
            {
                return CreatedAtAction(nameof(GetById), new { file_id = documentSendFileDTO.File_Id, doc_id = documentSendFileDTO.Document_Send_Id }, documentSendFileDTO);
            }
            return BadRequest("Lỗi khi thêm DocSend File");

        }


        // DELETE: https://localhost:7147/api/userpermissions/5
        //[Authorize(Roles = "Admin")]
        //[HttpDelete("{file_id}/{doc_id}")]
        public IActionResult DeleteUserPermission([FromRoute] string file_id, string doc_id)
        {
            if (_DocSendFileService.DeleteDocSendFile(doc_id, file_id))
            {
                return BadRequest("Đã xóa thành công !");
            }
            else { return BadRequest("Lỗi khi xóa DocSendFile"); }

        }
    }
}
