using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.Service;
using ND2Assignwork.API.Models.SignalRHub;
using System.Collections.Concurrent;

namespace ND2Assignwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class DocumentIncommingController : ControllerBase
    {
        private readonly IDocumentIncommingService _documentIncommingService;
        private readonly IFileService _fileService;
        private readonly IDocumentIncommingFileService _documentIncommingFileService;
        private readonly IDepartmentService _departmentService;
        private readonly IUserAccountService _userAccountService;
        public DocumentIncommingController(IDocumentIncommingService documentIncommingService, IFileService fileService, 
            IDocumentIncommingFileService documentIncommingFileService,
            IDepartmentService departmentService, IUserAccountService userAccountService)
        {
            _documentIncommingService = documentIncommingService;
            _fileService = fileService;
            _documentIncommingFileService = documentIncommingFileService;
            _departmentService = departmentService;
            _userAccountService = userAccountService;
        }
        // GET ALL Document
        [HttpGet,Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _documentIncommingService.GetAll());
        }

        
        [HttpGet("GetDocByDocId/{doc_id}")]
        public async Task<IActionResult> GetDocByDocId([FromRoute] string doc_id)
        {
            var document_IncommingDTO = await _documentIncommingService.GetDocInByDocInIdAsync(doc_id);
            if (document_IncommingDTO == null)
            {
                return BadRequest("Không tìm thấy Document Incomming !");
            }
            
            var fileIds =await _fileService.GetFileByDocInIdAsync(doc_id);
            if(fileIds != null)
            {
                _ = fileIds.Select(f => new
                {
                    f.File_Id,
                    f.File_Name,
                    f.ContentType,
                });
            }
            var fileCofirm = await _fileService.GetFileByDocSendIdComfirmAsync(doc_id);
            if (fileCofirm != null)
            {
                _ = fileCofirm.Select(f => new
                {
                    f.File_Id,
                    f.File_Name,
                    f.ContentType,
                });
            }

            return Ok(new
            {
                DocumentIncomming = new
                {
                    document_IncommingDTO.Document_Incomming_Id,
                    document_IncommingDTO.Document_Incomming_Title,
                    document_IncommingDTO.Document_Incomming_Content,
                    Document_Incomming_Time = document_IncommingDTO.Document_Incomming_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    document_IncommingDTO.Document_Incomming_UserSend,
                    Document_Incomming_UserSend_FullName = document_IncommingDTO.Sender.User_FullName,
                    Deparment_NameSend = document_IncommingDTO.Sender.Department.Department_Name,
                    document_IncommingDTO.Document_Incomming_UserReceive,
                    Document_Incomming_UserReceive_FullName = document_IncommingDTO.Receiver.User_FullName,
                    Deparment_NameReceive = document_IncommingDTO.Receiver.Department.Department_Name,
                    document_IncommingDTO.Document_Incomming_State,
                    document_IncommingDTO.Document_Incomming_Comment,
                    document_IncommingDTO.Document_Incomming_Id_Forward,
                    Document_Incomming_TimeUpdate = document_IncommingDTO.Document_Incomming_TimeUpdate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                },
                FileIds = fileIds,
                FileCofirm = fileCofirm,
            });
        }

        [HttpGet("GetListDocForwardByDocId")]
        public async Task<IActionResult> GetListDocForward(string DocId)
        {
            var documentIds = await _documentIncommingService.GetAllDocForwardByDocId(DocId);
            if (documentIds == null)
                return BadRequest("Không tồn tại Document Incomming!");
            return Ok(documentIds
                .Select(d => new
                {
                    d.Document_Incomming_Id,
                    d.Document_Incomming_Title,
                    d.Document_Incomming_Content,
                    Document_Incomming_Time = d.Document_Incomming_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    d.Document_Incomming_UserSend,
                    Document_Incomming_UserSend_FullName = d.Sender.User_FullName,
                    Deparment_NameSend = d.Sender.Department.Department_Name,
                    d.Document_Incomming_UserReceive,
                    Document_Incomming_UserReceive_FullName = d.Receiver.User_FullName,
                    Deparment_NameReceive = d.Receiver.Department.Department_Name,
                    d.Document_Incomming_State,
                    d.Document_Incomming_Comment,
                    d.Document_Incomming_Id_Forward,
                }));
        }

        [HttpGet("GetAllDocSendByUserLogin")]
        public async Task<IActionResult> GetAllDocUserLogin()
        {

            var document_Incomming = await _documentIncommingService.GetAllListUserSend(User.Identity.Name);
            if (document_Incomming == null)
            {
                return BadRequest("Không có Document Incomming !");
            }
            var documentDTOs = new List<DocumentInDTO>();

            foreach (var d in document_Incomming)
            {
                var documentDTO = new DocumentInDTO
                {
                    Document_Incomming_Id = d.Document_Incomming_Id,
                    Document_Incomming_Title = d.Document_Incomming_Title,
                    Document_Incomming_Content = d.Document_Incomming_Content,
                    Document_Incomming_Time = d.Document_Incomming_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    Document_Incomming_UserSend = d.Document_Incomming_UserSend,
                    Deparment_NameReceive = d.Receiver.Department.Department_Name,
                    Document_Incomming_UserReceive = d.Document_Incomming_UserReceive,
                    Document_Incomming_State = d.Document_Incomming_State,
                    Document_Incomming_Comment = d.Document_Incomming_Comment,
                    Document_Incomming_Id_Forward = d.Document_Incomming_Id_Forward,
                    Department_Location = ""
                };

                var nameDep = await _documentIncommingService.GetDocForwardByDocIdAsync(d.Document_Incomming_Id);
                documentDTO.Department_Location = nameDep?.Receiver?.Department?.Department_Name;

                documentDTOs.Add(documentDTO);
            }

            return Ok(documentDTOs);
        }

        [HttpGet("GetAllDocReceiveInDepartment")]
        public async Task<IActionResult> GetAllDocReceiveUserLogin()
        {

            var document_IncommingDTO = await _documentIncommingService.GetListDocUserReceiveInDepartment(User.Identity.Name);
            if (document_IncommingDTO == null)
            {
                return BadRequest("Không có Document Incomming !");
            }
            return Ok(document_IncommingDTO.Select(d => new
            {
                d.Document_Incomming_Id,
                d.Document_Incomming_Title,
                d.Document_Incomming_Content,
                Document_Incomming_Time = d.Document_Incomming_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                d.Document_Incomming_UserSend,
                Document_Incomming_UserSend_FullName = d.Sender.User_FullName,
                d.Document_Incomming_UserReceive,
                d.Document_Incomming_State,
                d.Document_Incomming_Comment,
                d.Document_Incomming_Id_Forward,
            }));
        }
        [HttpGet("GetAllDocReceiveOutDepartment")]
        public async Task<IActionResult> GetAllDocReceiveUserLoginOut()
        {

            var document_IncommingDTO = await _documentIncommingService.GetListDocUserReceiveOutDepartment(User.Identity.Name);
            if (document_IncommingDTO == null)
            {
                return BadRequest("Không có Document Incomming !");
            }
            return Ok(document_IncommingDTO.Select(d => new
            {
                d.Document_Incomming_Id,
                d.Document_Incomming_Title,
                d.Document_Incomming_Content,
                Document_Incomming_Time = d.Document_Incomming_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                d.Document_Incomming_UserSend,
                Document_Incomming_UserSend_FullName = d.Sender.User_FullName,
                Deparment_NameSend = d.Sender.Department.Department_Name,
                d.Document_Incomming_UserReceive,
                d.Document_Incomming_State,
                d.Document_Incomming_Comment,
                d.Document_Incomming_Id_Forward,
            }));
        }

        /*[HttpGet("GetListDocSendByUserLimitNumberPage/{limit:int}/{numberPage:int}")]
        public IActionResult GetListByUserLimitNumberPage(int limit, int numberPage)
        {
            var (documentList, totalNumberPage) = _documentIncommingService.GetListByUserSendLimitNumberPage(User.Identity.Name, limit, numberPage);

            if (documentList == null)
            {
                return BadRequest("Không có Document Incomming !");
            }
            var result = new
            {
                Documents = documentList.Select(d => new
                {
                    d.Document_Incomming_Id,
                    d.Document_Incomming_Title,
                    d.Document_Incomming_Content,
                    Document_Incomming_Time = d.Document_Incomming_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    d.Document_Incomming_UserSend,
                    Document_Incomming_UserSend_FullName = _userAccountService.GetUserAccountByUserId(d.Document_Incomming_UserSend).User_FullName,
                    Deparment_NameReceive = _departmentService.GetUserByDepartmentId(_userAccountService.GetUserAccountByUserId(d.Document_Incomming_UserReceive).User_Department).Department_Name,
                    d.Document_Incomming_UserReceive,
                    d.Document_Incomming_State,
                    d.Document_Incomming_Comment,
                }),
                TotalPages = totalNumberPage    
            };

            return Ok(result);
        }*/
        [HttpGet("GetListDocReceiveByUserNotification")]
        public async Task<IActionResult> GetListByUserReceiveIsSeen()
        {
            var documentList = await _documentIncommingService.GetListByUserReceiveIsSeen(User.Identity.Name);

            if (documentList == null || documentList.Count() == 0)
            {
                return BadRequest("Không có Document Incomming !");
            }

            var bag = new ConcurrentBag<object>();

            Parallel.ForEach(documentList, d =>
            {
                bag.Add(new
                {
                    d.Document_Incomming_Id,
                    d.Document_Incomming_Title,
                    Document_Incomming_Time = d.Document_Incomming_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    d.Document_Incomming_UserSend,
                    Document_Incomming_UserSend_FullName = checkState(d.Document_Incomming_State) ? d.Sender.User_FullName : d.Receiver.User_FullName,
                    DepartmentSend_Name = checkInDep(d.Sender.Department.Department_ID, d.Receiver.Department.Department_ID) ? null : (checkState(d.Document_Incomming_State) ? d.Sender.Department.Department_Name : d.Receiver.Department.Department_Name),
                    d.Document_Incomming_UserReceive,
                    d.Document_Incomming_State,
                    Document_Incomming_TimeUpdate = d.Document_Incomming_TimeUpdate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                });
            });
            
            return Ok(new
            {
                DocumentIncomming = bag
            });
        }
        private bool checkState(int state)
        {
            return (state == 0 || state == 21 || state == 6);
        }
        private bool checkInDep(string dep1, string dep2)
        {
            return dep1.Equals(dep2);
        }


        /*private string GetDepartmentName(string userId)
        {
            if (userId == null)
            {
                return null;
            }
            return _departmentService.GetDepartmentById(_userAccountService.GetUserAccountByUserId(userId).User_Department).Department_Name;
        }
        private string GetDepartmentIsHead(string userId)
        {
            if(userId == null)
            {
                return null;
            }
            if (_departmentService.isHead(userId))
            {
                return GetDepartmentName(userId);
            }
            return null;
        }*/

        /*[HttpGet("GetListDocReceiveByUserLimitNumberPage/{limit:int}/{numberPage:int}")]
        public IActionResult GetListByUserReceiveLimitNumberPage(int limit, int numberPage) 
        {
            var (documentList, totalNumberPage) = _documentIncommingService.GetListByUserReceiveLimitNumberPage(User.Identity.Name, limit, numberPage);

            if (documentList == null)
            {
                return BadRequest("Không có Document Incomming !");
            }
            var result = new
            {
                Documents = documentList.Select(d => new
                {
                    d.Document_Incomming_Id,
                    d.Document_Incomming_Title,
                    d.Document_Incomming_Content,
                    Document_Incomming_Time = d.Document_Incomming_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    d.Document_Incomming_UserSend,
                    Document_Incomming_UserSend_FullName = _userAccountService.GetUserAccountByUserId(d.Document_Incomming_UserSend).User_FullName,
                    d.Document_Incomming_UserReceive,
                    d.Document_Incomming_State,
                    d.Document_Incomming_Comment,
                    d.Document_Incomming_Id_Forward,
                }),
                TotalPages = totalNumberPage
            };

            return Ok(result);
        }*/

        // Thêm mới document
        [HttpPost("SendDepartmentHead")]
        public async Task<IActionResult> CreateDocumentIncomming(string Title, string Content, [FromForm] List<IFormFile> files)
        {
            if (Title == null || Content == null)
            {
                return BadRequest("Thông tin không đầy đủ");
            }
            var DepartmentEntity =await _departmentService.GetDepartmentByUserIdAsync(User.Identity.Name);
            if (DepartmentEntity == null)
            {
                return BadRequest("Không tồn department !");
            }
            if(DepartmentEntity.Department_Head == null)
            {
                return BadRequest("Không có department head!");
            }
            if (await _departmentService.IsHeadyAsync(User.Identity.Name))
                return BadRequest("Trưởng phòng không sử dụng được api này !");
            DateTime currentTime = DateTime.UtcNow;
            string formattedDateTime = currentTime.ToString("yyMMddhhmmssffff");
            string doc_id = "DocI" + formattedDateTime;
            var document_InDTO = new Document_IncommingDTO
            {
                Document_Incomming_Id = doc_id,
                Document_Incomming_Title = Title,
                Document_Incomming_Content = Content,
                Document_Incomming_Time = currentTime,
                Document_Incomming_UserSend = User.Identity.Name,
                Document_Incomming_UserReceive = DepartmentEntity.Department_Head,
            };

            if (await _documentIncommingService.Create(document_InDTO))
            {   
                if(files != null)
                {
                    foreach (var file in files)
                    {
                        var fileContent = new FileContent();
                        var dto = fileContent.ToFile(file);
                        if (!await _fileService.CreateAsync(dto))

                            return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
                        var DocFile = new Document_Incomming_FileDTO
                        {
                            Document_Incomming_Id = doc_id,
                            File_Id = dto.File_Id,
                        };
                        if (!await _documentIncommingFileService.CreateDocFile(DocFile))
                            return BadRequest("Lỗi khi gửi file: " + dto.File_Name);
                    }
                }
                return Ok("Đã thêm Document Incomming thành công !");
            }
            return BadRequest("Lỗi khi thêm Document Incomming !");
        }

        // Duyệt chuyển tiếp document
        [HttpPost("CreateSendByDepartmentId")]
        public async Task<IActionResult> ForwardDocumentIncomming(string Title, string Content, string Comment, string DepartmentIdReceive, string?  DocIdForward, [FromForm] List<IFormFile> files)
        {
            if (DepartmentIdReceive == null || Title == null || Content == null)
                return BadRequest("Thông tin không đầy đủ");
            var DepartmentEntity = _departmentService.GetDepartmentById(DepartmentIdReceive);
            if (DepartmentEntity == null)
                return BadRequest("Không tồn tại department !");
            if (DepartmentEntity.Department_Head == null)
                return BadRequest("Không có department head!");
            if (_departmentService.GetDepartmentById(_userAccountService.GetUserAccountByUserId(User.Identity.Name).User_Department).Department_Head != User.Identity.Name)
                return BadRequest("Không phải trưởng phòng không sử dụng được api này !");
            if (DocIdForward != null)
            {
                var docIn = await _documentIncommingService.GetById(DocIdForward);
                if (docIn.Document_Incomming_State != 3)
                    return BadRequest("Document không ở trong trạng thái đã duyệt !");
            }
    
            DateTime currentTime = DateTime.UtcNow;    
            string formattedDateTime = currentTime.ToString("yyMMddhhmmssffff");
            string doc_id = "DocI" + formattedDateTime;
            var fileForwards = new List<string>();
            if(DocIdForward != null)
            {
                var filesFW = await _documentIncommingFileService.GetDocFileByDocId(DocIdForward);
                fileForwards = filesFW?.Select(d => d.File_Id).ToList() ?? new List<string>();
            }
            var document_InDTO = new Document_IncommingDTO
            {
                Document_Incomming_Id = doc_id,
                Document_Incomming_Title = Title,
                Document_Incomming_Content = Content,
                Document_Incomming_Time = currentTime,
                Document_Incomming_UserSend = User.Identity.Name,
                Document_Incomming_UserReceive = DepartmentEntity.Department_Head,
                Document_Incomming_Id_Forward = DocIdForward,
                Document_Incomming_Comment = Comment,
                
            };

            if (await _documentIncommingService.Create(document_InDTO))
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var fileContent = new FileContent();
                        var dto = fileContent.ToFile(file);
                        if (!_fileService.Create(dto))

                            return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
                        fileForwards.Add(dto.File_Id);
                    }
                }
                if (fileForwards != null)
                {
                    foreach(var fileId in fileForwards)
                    {
                        var DocFile = new Document_Incomming_FileDTO
                        {
                            Document_Incomming_Id = doc_id,
                            File_Id = fileId,
                        };
                        if (!await _documentIncommingFileService.CreateDocFile(DocFile))
                            return BadRequest("Lỗi khi gửi file: " + fileId);
                    }
                }
                if(DocIdForward == null)
                {
                    return Ok("Đã gửi Document Incomming thành công !");
                }
                if(await _documentIncommingService.UpdateState(DocIdForward, 4)){ // cập nhập trạng thái đã chuyển tiếp
                    //object message = $"Trạng thái tài liệu {DocIdForward} đã được cập nhật thành {4}";
                    //_hubContext.Clients.Group(_documentIncommingService.GetById(DocIdForward).Document_Incomming_UserReceive).SendAsync("ReceiveUpdateNotification", message);
                    return Ok("Đã chuyển tiếp Document Incomming thành công !");
                }
                //return BadRequest("Lỗi khi cập nhật trạng thái doc forward !");
            }
            return BadRequest("Lỗi khi chuyển tiếp Document Incomming !");
        }


        /*// Bàn giao xuống phòng ban ở trạng thái đã duyệt
        [HttpPost("SetHandOver"), Authorize(Roles = "SuperAdmin, Admin, SendAllDoc")]
        public IActionResult SetHandOverDocument(string doc_id ,[FromForm] List<string> user_id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var docIncomming = _documentIncommingService.GetById(doc_id);
            //Chỉ đang trạng thái 1(đã duyệt)
            if (docIncomming.Document_Incomming_State == 1)
            {
                if (_documentIncommingService.SetHandOverDocument(doc_id, user_id))
                    return BadRequest("Bàn giao thành công !");
                return BadRequest("Lỗi khi bàn giao Document Incomming !");
            }
            return BadRequest("Documment Incomming này không ở trong trạng thái đã duyệt !");
        }  
         
         */

        /*// Sửa danh sách bàn giao ở trạng thái đã duyệt trong 15 phút đầu bàn giao
        [HttpPut("UpdateHandOver"), Authorize(Roles = "SuperAdmin, Admin, SendAllDoc")]
        public IActionResult UpdateHandOverDocument(string doc_id, [FromForm] List<string> user_id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var docIncomming = _documentIncommingService.GetById(doc_id);
            // chỉ đang ở trang thái 2(bàn giao)
            if (docIncomming.Document_Incomming_State == 2)
            {   
                var timeReceive = _userReceiveDocumentService.GetUserReceiceByDocId(doc_id).FirstOrDefault().Document_Incomming_TimeSend;
                //giới hạn 15 phút đầu
                if (timeReceive.AddMinutes(15) < DateTime.Now)
                    return BadRequest("Đã vượt thời gian quy định sửa danh sách bàn giao !"); ;
                if (_documentIncommingService.UpdateHandOverDocument(doc_id, user_id))
                    return BadRequest("Update bàn giao thành công !");
                return BadRequest("Lỗi khi sửa bàn giao Document Incomming !");
            }
            return BadRequest("Documment Incomming này không ở trong trạng thái bàn giao !");
        }*/

        [HttpPut("UpdateDocIncomming")]
        public async Task<IActionResult> UpdateDocumentIncomming(string doc_id, string Title, string Content,
                 [FromForm] List<IFormFile> files,[FromForm] List<string> fileOlds)
        {
            if (doc_id == null || Title == null || Content == null)
            {
                return BadRequest("Thông tin không đầy đủ");
            }
            var docState = await _documentIncommingService.GetById(doc_id);
            if (docState.Document_Incomming_UserSend != User.Identity.Name)
            {
                return BadRequest("Chỉ người gửi mới được quyền sửa !");
            }
            if (docState.Document_Incomming_State != 0 && docState.Document_Incomming_State != 2)
            {
                return BadRequest("Document không được chỉnh sửa !");
            }
            var document_InDTO = new Document_IncommingDTO
            {
                Document_Incomming_Id = doc_id,
                Document_Incomming_Title = Title,
                Document_Incomming_Content = Content,
                Document_Incomming_State = 21,
                Document_Incomming_IsSeen = false,
                Document_Incomming_TimeUpdate = DateTime.UtcNow,
            };
            if (await _documentIncommingService.Update(document_InDTO))
            {
                if (await _documentIncommingFileService.DeleteDocFilesByDocId(doc_id))
                {
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            var fileContent = new FileContent();
                            var dto = fileContent.ToFile(file);
                            if (!_fileService.Create(dto))
                                return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
                            fileOlds.Add(dto.File_Id);
                        }
                    }
                    if (fileOlds != null)
                    {
                        foreach (var fileId in fileOlds)
                        {
                            var DocFile = new Document_Incomming_FileDTO
                            {
                                Document_Incomming_Id = doc_id,
                                File_Id = fileId,
                            };
                            if (!await _documentIncommingFileService.CreateDocFile(DocFile))
                                return BadRequest("Lỗi khi gửi file: " + fileId);
                        }
                    }
                    return Ok("Cập nhật Document Incomming thành công !");
                }
                return BadRequest("Lỗi khi xóa document Incomming file !");
            }
            return BadRequest("Lỗi khi cập nhật document Incomming !");
        }

        // PUT: cập nhật trạng thái
        [HttpPut("UpdateState")]
        public async Task<IActionResult> UpdateStateDocIncomming(string docId,int state, string comment)
        {
            
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (await _documentIncommingService.UpdateStateComment(docId, state, comment))
            {
                return Ok("Đã cập nhập trạng thái Document Incomming!");
            }    
            return BadRequest("Lỗi khi cập nhật document Incomming !");
        }

        [HttpPut("UpdateSeenTrue/{id}")]
        public async Task<IActionResult> UpdateSeenTrueDocIncomming(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (await _documentIncommingService.UpdateSeenTrue(id))
            {
                return Ok("Cập nhập đã xem thành công ");
            }
            return BadRequest("Lỗi khi cập nhật document Incomming !");
        }

        /* // PUT: cập nhật trạng thái
         [HttpPut("UpdateState/{id}/{state:int}")]
         public IActionResult UpdateStateDocIncomming([FromRoute] string id,int state)
         {
             if (!ModelState.IsValid)
                 return BadRequest(ModelState);

             if (_documentIncommingService.UpdateState(id, state))
                 return Ok("Update State Document Incomming thành công !");
             return BadRequest("Lỗi khi cập nhật state document Incomming !");
         }*/

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocumentIncomming([FromRoute] string id)
        {
            var document_IncommingDTO = await _documentIncommingService.GetById(id);
            if (document_IncommingDTO == null)
                return BadRequest("Document Incomming không tồn tại !");
            if (document_IncommingDTO.Document_Incomming_State >= 3)
                return BadRequest("Không thể xóa document Incomming đã duyệt !");
            if (await _documentIncommingFileService.DeleteDocFilesByDocId(id))
            {
                if (await _documentIncommingService.Delete(id))
                    return Ok("Đã xóa document Incomming thành công !");
                return BadRequest("Lỗi khi xóa document Incomming !");
            }
            return BadRequest("Lỗi khi xóa document Incomming File !");

        }
    }
}
