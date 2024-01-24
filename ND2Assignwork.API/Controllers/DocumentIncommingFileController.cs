using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.Service;

namespace ND2Assignwork.API.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController, Authorize]
    public class DocumentIncommingFileController : ControllerBase
    {
        private readonly IDocumentIncommingFileService _docInFileService;
        public DocumentIncommingFileController(IDocumentIncommingFileService docInFileService)
        {
            _docInFileService = docInFileService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_docInFileService.GetAllDocumentFile());
        }

        //[HttpGet("GetByDocId/{doc_id}")]
        public async Task<IActionResult> GetById(string doc_id)
        {
            var documentSendFileDTOs =await _docInFileService.GetDocFileByDocId(doc_id);
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
            var documentSendFileDTO = _docInFileService.GetOneDocFile(doc_id, file_id);
            if (documentSendFileDTO == null)
            {
                return NotFound();
            }
            return Ok(documentSendFileDTO);
        }

        // POST:
        [HttpPost]
        public async Task<IActionResult> CreateDocSendFile([FromBody] Document_Incomming_FileDTO document_Incomming_FileDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _docInFileService.CreateDocFile(document_Incomming_FileDTO))
            {
                return CreatedAtAction(nameof(GetById), new { file_id = document_Incomming_FileDTO.File_Id, doc_id = document_Incomming_FileDTO.Document_Incomming_Id }, document_Incomming_FileDTO);
            }
            return BadRequest("Lỗi khi thêm DocIncomming File");

        }


        // DELETE: https://localhost:7147/api/userpermissions/5
        //[Authorize(Roles = "Admin")]
        //[HttpDelete("{file_id}/{doc_id}")]
        public IActionResult DeleteUserPermission([FromRoute] string file_id, string doc_id)
        {
            if (_docInFileService.DeleteDocFile(doc_id, file_id))
            {
                return BadRequest("Đã xóa thành công !");
            }
            else { return BadRequest("Lỗi khi xóa DocIncomming File"); }

        }
    }
}
