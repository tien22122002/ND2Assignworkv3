using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.Service;
using ND2Assignwork.API.Models.Service.Imp;

namespace ND2Assignwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class DocumentSendController : ControllerBase
    {
        private readonly IDocumentSendService _documentSendService;
        private readonly IDocumentSendFileService _documentSendFileService;
        private readonly IUserReceiveDocumentService _userReceiveDocumentService;
        private readonly IFileService _fileService;
        private readonly IDepartmentService _departmentService;
        private readonly IUserAccountService _userAccountService;
        private readonly IDocumentIncommingService _documentIncommingService;
        private readonly ITaskService _taskService;
        private readonly ITaskCategoryService _taskCategoryService;
        public DocumentSendController(IDocumentSendService documentSendService,
            IDocumentSendFileService documentSendFileService, IUserReceiveDocumentService userReceiveDocumentService,
            IFileService fileService, IDepartmentService departmentService, IUserAccountService userAccountService,
            IDocumentIncommingService documentIncommingService, ITaskService taskService, ITaskCategoryService taskCategoryService)
        {
            _documentSendService = documentSendService;
            _fileService = fileService;
            _documentSendFileService = documentSendFileService;
            _userReceiveDocumentService = userReceiveDocumentService;
            _departmentService = departmentService;
            _userAccountService = userAccountService;
            _documentIncommingService = documentIncommingService;
            _taskService = taskService;
            _taskCategoryService = taskCategoryService;
        }
        // GET ALL Document
        [HttpGet, Authorize(Roles ="SuperAdmin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _documentSendService.GetAll());
        }


        [HttpGet("GetDocByDocId/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var document_SendDTO =await _documentSendService.GetDocSendFullByDocIdAsync(id);
            if (document_SendDTO == null)
            {
                return BadRequest("Không tìm thấy Document Send !");
            }
            /*var fileIds = await _fileService.GetFileByDocSendIdAsync(id);
            if (fileIds != null)
            {
                _ = fileIds.Select(f => new
                {
                    f.File_Id,
                    f.File_Name,
                    f.ContentType,
                });
            }
            var UserReceiceEntity = _userReceiveDocumentService.GetUserReceiceByDocId(id);
            if (UserReceiceEntity != null)
            {
                _ = UserReceiceEntity.Select(u => new
                {
                    UserIdReceive = u.User_Id,
                    _userAccountService.GetUserAccountByUserId(u.User_Id).User_FullName,
                }).ToList();
            }*/

            return Ok(new
            {
                DocumentSend = new
                {
                    document_SendDTO.Document_Send_Id,
                    document_SendDTO.Document_Send_Title,
                    document_SendDTO.Document_Send_Content,
                    Document_Send_Time = document_SendDTO.Document_Send_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    Document_Send_TimeStart = document_SendDTO.Document_Send_TimeStart?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                    Document_Send_Deadline = document_SendDTO.Document_Send_Deadline?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                    document_SendDTO.Document_Send_UserSend,
                    UserSend_FullName = document_SendDTO.UserAccount.User_FullName,
                    Department_Name_Send = document_SendDTO.UserAccount.Department.Department_Name,
                    Catagory_Name = document_SendDTO.Category.Category_Name,
                    document_SendDTO.Document_Send_Comment,
                    document_SendDTO.Document_Send_State,
                    Document_Send_TimeUpdate = document_SendDTO.Document_Send_TimeUpdate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    Document_Send_Percent = _taskService.GetPercentTaskDocSendId(document_SendDTO.Document_Send_Id)

                },
                FileIds = document_SendDTO.Document_Send_Files?.Select(f => new
                {
                    f.File_Id,
                    f.File.File_Name,
                    f.File.ContentType
                }),
                UserIdReceive = document_SendDTO.ReceivedByUsers?.Select(u => new
                {
                    u.User_Id,
                    u.User_Account.User_FullName,
                    u.Department?.Department_Name
                }),
            });
        }

        [HttpGet("GetAllDocSendPublicByUserLogin")]
        public IActionResult GetAllDocPublicUserLogin()
        {

            var document_SendDTO = _documentSendService.GetDocumentList(User.Identity.Name, false);
            if (document_SendDTO == null)
            {
                return BadRequest("Không có Document Send !");
            }
            var result = document_SendDTO.Select(d => new
            {
                d.Document_Send_Id,
                d.Document_Send_Title,
                d.Document_Send_Content,
                Document_Send_Time = d.Document_Send_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Document_Send_TimeStart = d.Document_Send_TimeStart?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                Document_Send_Deadline = d.Document_Send_Deadline?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                d.Document_Send_UserSend,
                Catagory_Name = _taskCategoryService.GetTaskCategoryById(d.Document_Send_Catagory).Category_Name,
                d.Document_Send_Comment,
                d.Document_Send_State,
                UserReceive = _userReceiveDocumentService.GetUserReceiceByDocId(d.Document_Send_Id)
                    ?.Select(u => new
                    {
                        u.User_Id,
                        _userAccountService.GetUserAccountById(u.User_Id)?.User_FullName,
                        _departmentService.GetDepartmentById(u.Department_Id)?.Department_Name,
                    }).ToList()
                    .Cast<object>() // Chuyển đổi thành List<object>
                    .ToList(),

                Document_Send_Percent = _taskService.GetPercentTaskDocSendId(d.Document_Send_Id)
            }).ToList();

            return Ok(result);
        }
        [HttpGet("GetAllDocSendTransferByUserLogin")]
        public IActionResult GetAllDocSendTransferByUserLogin()
        {

            var document_SendDTO = _documentSendService.GetDocumentList(User.Identity.Name, true);
            if (document_SendDTO == null)
            {
                return BadRequest("Không có Document Send !");
            }
            var result = document_SendDTO.Select(d => new
            {
                d.Document_Send_Id,
                d.Document_Send_Title,
                d.Document_Send_Content,
                Document_Send_Time = d.Document_Send_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Document_Send_TimeStart = d.Document_Send_TimeStart?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                Document_Send_Deadline = d.Document_Send_Deadline?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                d.Document_Send_UserSend,
                Catagory_Name = _taskCategoryService.GetTaskCategoryById(d.Document_Send_Catagory).Category_Name,
                d.Document_Send_Comment,
                d.Document_Send_State,
                UserReceive = _userReceiveDocumentService.GetUserReceiceByDocId(d.Document_Send_Id)
                    ?.Select(u => new
                    {
                        u.User_Id,
                        _userAccountService.GetUserAccountById(u.User_Id)?.User_FullName,
                        _departmentService.GetDepartmentById(u.Department_Id)?.Department_Name,
                    }).ToList()
                    .Cast<object>() // Chuyển đổi thành List<object>
                    .ToList(),
                Document_Send_Percent = _taskService.GetPercentTaskDocSendId(d.Document_Send_Id)
            }).ToList();

            return Ok(result);
        }

        [HttpGet("GetAllDocSendPrivateByUserLogin")]
        public IActionResult GetAllDocPrivateUserLogin()
        {

            var document_SendDTO = _documentSendService.GetAllListPrivateUserSend(User.Identity.Name);
            if (document_SendDTO == null)
            {
                return BadRequest("Không có Document Send !");
            }
            var result = document_SendDTO.Select(d => new
            {
                d.Document_Send_Id,
                d.Document_Send_Title,
                d.Document_Send_Content,
                Document_Send_Time = d.Document_Send_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Document_Send_TimeStart = d.Document_Send_TimeStart?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                Document_Send_Deadline = d.Document_Send_Deadline?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                d.Document_Send_UserSend,
                Catagory_Name = _taskCategoryService.GetTaskCategoryById(d.Document_Send_Catagory).Category_Name,
                d.Document_Send_Comment,
                d.Document_Send_State,
                Document_Send_Percent = _taskService.GetPercentTaskDocSendId(d.Document_Send_Id)
            }).ToList();

            return Ok(result);
        }

        [HttpGet("GetAllDocReceiveByUserLogin")]
        public IActionResult GetAllDocReceiveUserLogin()
        {

            var document_SendDTO = _documentSendService.GetAllListUserReceive(User.Identity.Name);
            if (document_SendDTO == null)
            {
                return BadRequest("Không có Document Send !");
            }
            var result = document_SendDTO.Select(d => new
            {
                d.Document_Send_Id,
                d.Document_Send_Title,
                d.Document_Send_Content,
                Document_Send_Time = d.Document_Send_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Document_Send_TimeStart = d.Document_Send_TimeStart?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                Document_Send_Deadline = d.Document_Send_Deadline?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                d.Document_Send_UserSend,
                UserSend_FullName = _userAccountService.GetUserAccountById(d.Document_Send_UserSend).User_FullName,
                Department_Name_Send = _departmentService.GetDepartmentById(_userAccountService.GetUserAccountById(d.Document_Send_UserSend).User_Department).Department_Name,
                Catagory_Name = _taskCategoryService.GetTaskCategoryById(d.Document_Send_Catagory).Category_Name,
                d.Document_Send_Comment,
                d.Document_Send_State,
                Document_Send_Percent = _taskService.GetPercentTaskDocSendId(d.Document_Send_Id)

            }).ToList();

            return Ok(result);
        }
        [HttpGet("GetListDocReceiveByUserNotification")]
        public IActionResult GetListDocReceiveByUserNotification()
        {

            var document_SendDTO = _documentSendService.GetAllListUserReceiveIsSeen(User.Identity.Name);
            if (document_SendDTO == null)
            {
                return BadRequest("Không có Document Send !");
            }

            return Ok(new
            {
                DocumentSend  = document_SendDTO.Select(d => new
                {
                    d.Document_Send_Id,
                    d.Document_Send_Title,
                    d.Document_Send_Content,
                    Document_Send_Time = d.Document_Send_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    Document_Send_TimeStart = d.Document_Send_TimeStart?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                    Document_Send_Deadline = d.Document_Send_Deadline?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                    d.Document_Send_UserSend,
                    UserSend_FullName = _userAccountService.GetUserAccountById(d.Document_Send_UserSend).User_FullName,
                    Department_Name_Receive = _departmentService.GetDepartmentById(_userAccountService.GetUserAccountById(d.Document_Send_UserSend).User_Department).Department_Name,
                    Catagory_Name = _taskCategoryService.GetTaskCategoryById(d.Document_Send_Catagory).Category_Name,
                    d.Document_Send_Comment,
                    d.Document_Send_State,
                    Document_Send_TimeUpdate = d.Document_Send_TimeUpdate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                }).ToList()
                });
        }

        /*[HttpGet("GetListDocSendPublicByUserLimitNumberPage/{limit:int}/{numberPage:int}")]
        public IActionResult GetListByUserLimitNumberPage(int limit, int numberPage)
        {
            var (documentList, totalNumberPage) = _documentSendService.GetListByUserSendLimitNumberPage(User.Identity.Name, limit, numberPage);

            if (documentList == null)
            {
                return BadRequest("Không có Document Send !");
            }
            
            var result = documentList.Select(d => new
            {
                d.Document_Send_Id,
                d.Document_Send_Title,
                d.Document_Send_Content,
                Document_Send_Time = d.Document_Send_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Document_Send_TimeStart = d.Document_Send_TimeStart?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                Document_Send_Deadline = d.Document_Send_Deadline?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                d.Document_Send_UserSend,
                d.Document_Send_Catagory,
                d.Document_Send_Comment,
                d.Document_Send_State,
                UserReceive = _userReceiveDocumentService.GetUserReceiceByDocId(d.Document_Send_Id)
                    ?.Select(u => new
                    {
                        u.User_Id,
                        _userAccountService.GetUserAccountById(u.User_Id)?.User_FullName,
                    }).ToList()
                    .Cast<object>() // Chuyển đổi thành List<object>
                    .ToList(),
            }).ToList();
            var results = new
            {
                Documents = result,
                TotalPages = totalNumberPage
            };
            return Ok(results);
        }

        [HttpGet("GetListDocReceiveByUserLimitNumberPage/{limit:int}/{numberPage:int}")]
        public IActionResult GetListByUserReceiveLimitNumberPage(int limit, int numberPage)
        {
            var (documentList, totalNumberPage) = _documentSendService.GetListByUserReceiveLimitNumberPage(User.Identity.Name, limit, numberPage);

            if (documentList == null)
            {
                return BadRequest("Không có Document Send !");
            }
            var result = documentList.Select(d => new
            {
                d.Document_Send_Id,
                d.Document_Send_Title,
                d.Document_Send_Content,
                Document_Send_Time = d.Document_Send_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Document_Send_TimeStart = d.Document_Send_TimeStart?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                Document_Send_Deadline = d.Document_Send_Deadline?.ToString("yyyy-MM-ddTHH:mm:ssZ") ?? null,
                d.Document_Send_UserSend,
                d.Document_Send_Catagory,
                d.Document_Send_Comment,
                d.Document_Send_State,
                UserReceive = _userReceiveDocumentService.GetUserReceiceByDocId(d.Document_Send_Id)
                   ?.Select(u => new
                   {
                       u.User_Id,
                       _userAccountService.GetUserAccountById(u.User_Id)?.User_FullName,
                   }).ToList()
                   .Cast<object>() // Chuyển đổi thành List<object>
                   .ToList(),
            }).ToList();
            var results = new
            {
                Documents = result,
                TotalPages = totalNumberPage
            };

            return Ok(results);
        }
*/

        [HttpPost("CreateDocSendPublic")]
        public async Task<IActionResult> CreateDocumentSend
            (string Title, string Content, DateTime? TimeStart, DateTime? Deadline, int CatagoryId,
            [FromForm] List<IFormFile> files, [FromForm] List<string> DepartmentIds, [FromForm] List<string> UserReceives)
        {
            if (CatagoryId == 0 || Title == null || Content == null )
            {
                return BadRequest("Thông tin không đầy đủ");
            }
            if (UserReceives.Count == 0 && DepartmentIds.Count == 0)
            {
                return BadRequest("Vui lòng thêm người nhận hoặc phòng ban !");
            }
            if (!await _departmentService.isHeadAsync(User.Identity.Name))
            {
                return BadRequest("Không phải là trưởng phòng không sử dụng được chức năng này !");
            }
            DateTime currentTime = DateTime.UtcNow;
            string formattedDateTime = currentTime.ToString("yyMMddhhmmssffff");
            string doc_id = "DocS" + formattedDateTime;
            var document_SendDTO = new Document_SendDTO
            {
                Document_Send_Id = doc_id,
                Document_Send_Title = Title,
                Document_Send_Content = Content,
                Document_Send_Time = currentTime,
                Document_Send_TimeStart = TimeStart,
                Document_Send_Deadline = Deadline,
                Document_Send_UserSend = User.Identity.Name,
                Document_Send_Catagory = CatagoryId,
                Document_Send_State = 3,
                Document_Send_Public = true,
            };

            if (_documentSendService.Create(document_SendDTO))
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var fileContent = new FileContent();
                        var dto = fileContent.ToFile(file);
                        if (!_fileService.Create(dto))

                            return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
                        var DocFile = new Document_Send_FileDTO
                        {
                            Document_Send_Id = doc_id,
                            File_Id = dto.File_Id,
                        };
                        if (!_documentSendFileService.CreateDocSendFile(DocFile))
                            return BadRequest("Lỗi khi gửi file: " + dto.File_Name);
                    }
                }
                var UserDepartments = new List<string>();
                if(DepartmentIds != null)
                {
                    foreach (var depId in DepartmentIds)
                    {
                        var departmentEnity = _departmentService.GetDepartmentById(depId);
                        if (departmentEnity != null)
                        {
                            if(departmentEnity.Department_Head != null)
                            {
                                UserDepartments.Add(departmentEnity.Department_Head);
                                var userReceive = new User_Receive_DocumentDTO
                                {
                                    Document_Send_Id = doc_id,
                                    User_Id = departmentEnity.Department_Head,
                                    Department_Id = depId,
                                };
                                if (!_userReceiveDocumentService.CreateUserReceice(userReceive))
                                    return BadRequest("Lỗi khi gửi Doc cho department: " + departmentEnity.Department_Name);
                            }
                        }
                    }
                }
                if(UserReceives!=null)
                {
                    foreach (var user in UserReceives)
                    {
                        if (!UserDepartments.Contains(user))
                        {
                            var userReceive = new User_Receive_DocumentDTO
                            {
                                Document_Send_Id = doc_id,
                                User_Id = user,
                                Department_Id = null,
                            };
                            if (!_userReceiveDocumentService.CreateUserReceice(userReceive))
                                return BadRequest("Lỗi khi gửi Doc cho user: " + user);
                        }
                    }
                }
                return Ok("Đã thêm Document Send thành công !");
            }
            return BadRequest("Lỗi khi thêm Document Send !");
            
        }

        [HttpPost("CreateDocSendPrivate")]
        public async Task<IActionResult> CreateDocumentSendPrivate
            ( string Title, string Content, DateTime? TimeStart, DateTime? Deadline, int CatagoryId,
            [FromForm] List<IFormFile> files)
        {
            if (CatagoryId == 0 || Title == null || Content == null)
            {
                return BadRequest("Thông tin không đầy đủ");
            }

            if (!await _departmentService.isHeadAsync(User.Identity.Name))
            {
                return BadRequest("Không phải là trưởng phòng không sử dụng được chức năng này !");
            }
            DateTime currentTime = DateTime.UtcNow;
            string formattedDateTime = currentTime.ToString("yyMMddhhmmssffff");
            string doc_id = "DocS" + formattedDateTime;
            var document_SendDTO = new Document_SendDTO
            {
                Document_Send_Id = doc_id,
                Document_Send_Title = Title,
                Document_Send_Content = Content,
                Document_Send_Time = currentTime,
                Document_Send_TimeStart = TimeStart,
                Document_Send_Deadline = Deadline,
                Document_Send_UserSend = User.Identity.Name,
                Document_Send_Catagory = CatagoryId,
                Document_Send_State = 3,
                Document_Send_Public = false,
            };

            if (_documentSendService.Create(document_SendDTO))
            {
                if (files != null)
                {
                    var filesDocIn = new List<string>();
                    foreach (var file in files)
                    {
                        var fileContent = new FileContent();
                        var dto = fileContent.ToFile(file);
                        if (!_fileService.Create(dto))

                            return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
                        filesDocIn.Add(dto.File_Id);
                    }
                    if (filesDocIn.Count > 0)
                    {
                        foreach (var file in filesDocIn)
                        {
                            var DocFile = new Document_Send_FileDTO
                            {
                                Document_Send_Id = doc_id,
                                File_Id = file,
                            };
                            if (!_documentSendFileService.CreateDocSendFile(DocFile))
                                return BadRequest("Lỗi khi gửi file: " + file);
                        }
                    }
                }
                return Ok("Đã thêm Document Send thành công !");
            }
            return BadRequest("Lỗi khi thêm Document Send !");

        }

        [HttpPost("CreateDocSendPrivateByDocIn")]
        public async Task<IActionResult> CreateDocumentSendByDocIncomming
            (string DocIncommingId, string Title, string Content, DateTime? TimeStart, DateTime? Deadline, int CatagoryId,
            [FromForm] List<IFormFile> files, [FromForm] List<string> filesDocIn)
        {
            if (CatagoryId == 0 || Title == null || Content == null || DocIncommingId == null)
            {
                return BadRequest("Thông tin không đầy đủ");
            }
            if (!await _departmentService.isHeadAsync(User.Identity.Name))
            {
                return BadRequest("Không phải là trưởng phòng không sử dụng được chức năng này !");
            }
            var docIn = await _documentIncommingService.GetById(DocIncommingId);
            if (docIn.Document_Incomming_State != 3)
            {
                return BadRequest("Document Incomming không ở trong trạng thái đã duyệt");
            }

            DateTime currentTime = DateTime.UtcNow;
            //string formattedDateTime = currentTime.ToString("yyMMddhhmmssffff");
            //string doc_id = "DocS" + formattedDateTime;
            var document_SendDTO = new Document_SendDTO
            {
                Document_Send_Id = DocIncommingId,
                Document_Send_Title = Title,
                Document_Send_Content = Content,
                Document_Send_Time = currentTime,
                Document_Send_TimeStart = TimeStart,
                Document_Send_Deadline = Deadline,
                Document_Send_UserSend = User.Identity.Name,
                Document_Send_Catagory = CatagoryId,
                Document_Send_State = 3,
                Document_Send_Public = false,
            };

            if (_documentSendService.Create(document_SendDTO))
            {
                if (files != null)
                {   
                    foreach (var file in files)
                    {
                        var fileContent = new FileContent();
                        var dto = fileContent.ToFile(file);
                        if (!_fileService.Create(dto))

                            return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
                        filesDocIn.Add(dto.File_Id);
                      }
                }
                if(filesDocIn.Count > 0)
                {
                    foreach (var file in filesDocIn)
                    {
                        var DocFile = new Document_Send_FileDTO
                        {
                            Document_Send_Id = DocIncommingId,
                            File_Id = file,
                        };
                        if (!_documentSendFileService.CreateDocSendFile(DocFile))
                            return BadRequest("Lỗi khi gửi file: " + file);
                    }    
                }
                if (!await _documentIncommingService.UpdateState(DocIncommingId, 5))
                    return BadRequest("Lỗi khi cập nhật trạng thái Doc");
                return Ok("Đã thêm Document Send thành công !");
            }
            return BadRequest("Lỗi khi thêm Document Send !");

        }


        [HttpPost("CreateDocSendPublicByDocIn")]
        public async Task<IActionResult> CreateDocSendPublicByDocIn
            (string DocIncommingId, string Title, string Content, DateTime? TimeStart, DateTime? Deadline, int CatagoryId,
            [FromForm] List<IFormFile> files, [FromForm] List<string> filesDocIn, [FromForm] List<string> DepartmentIds)
        {
            if (CatagoryId == 0 || Title == null || Content == null || DocIncommingId == null)
            {
                return BadRequest("Thông tin không đầy đủ");
            }
            if (!_departmentService.isHead(User.Identity.Name))
            {
                return BadRequest("Không phải là trưởng phòng không sử dụng được chức năng này !");
            }
            var docIn = await _documentIncommingService.GetById(DocIncommingId);
            if (docIn.Document_Incomming_State != 3)
            {
                return BadRequest("Document Incomming không ở trong trạng thái đã duyệt");
            }

            DateTime currentTime = DateTime.UtcNow;
            //string formattedDateTime = currentTime.ToString("yyMMddhhmmssffff");
            //string doc_id = "DocS" + formattedDateTime;
            var document_SendDTO = new Document_SendDTO
            {
                Document_Send_Id = DocIncommingId,
                Document_Send_Title = Title,
                Document_Send_Content = Content,
                Document_Send_Time = currentTime,
                Document_Send_TimeStart = TimeStart,
                Document_Send_Deadline = Deadline,
                Document_Send_UserSend = User.Identity.Name,
                Document_Send_Catagory = CatagoryId,
                Document_Send_State = 3,
                Document_Send_Public = true,
            };

            if (_documentSendService.Create(document_SendDTO))
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var fileContent = new FileContent();
                        var dto = fileContent.ToFile(file);
                        if (!_fileService.Create(dto))

                            return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
                        filesDocIn.Add(dto.File_Id);
                    }
                }
                if (filesDocIn.Count > 0)
                {
                    foreach (var file in filesDocIn)
                    {
                        var DocFile = new Document_Send_FileDTO
                        {
                            Document_Send_Id = DocIncommingId,
                            File_Id = file,
                        };
                        if (!_documentSendFileService.CreateDocSendFile(DocFile))
                            return BadRequest("Lỗi khi gửi file: " + file);
                    }
                }
                if (DepartmentIds != null)
                {
                    foreach (var depId in DepartmentIds)
                    {
                        var departmentEnity = _departmentService.GetDepartmentById(depId);
                        if (departmentEnity != null)
                        {
                            if (departmentEnity.Department_Head != null)
                            {
                                var userReceive = new User_Receive_DocumentDTO
                                {
                                    Document_Send_Id = DocIncommingId,
                                    User_Id = departmentEnity.Department_Head,
                                    Department_Id = depId,
                                };
                                if (!_userReceiveDocumentService.CreateUserReceice(userReceive))
                                    return BadRequest("Lỗi khi gửi Doc cho department: " + departmentEnity.Department_Name);
                            }
                        }
                    }
                }
                if (!await _documentIncommingService.UpdateState(DocIncommingId, 5))
                    return BadRequest("Lỗi khi cập nhật trạng thái Doc");
                return Ok("Đã thêm Document Send thành công !");
            }
            return BadRequest("Lỗi khi thêm Document Send !");
        }
        [HttpPut("CompleteProposal")]
        public async Task<IActionResult> CompleteProposal
            (string DocSendId, 
            [FromForm] List<IFormFile> files, [FromForm] List<string> filesDocIn)
        {
            if (DocSendId == null)
            {
                return BadRequest("Thông tin không đầy đủ");
            }
            if (!_departmentService.isHead(User.Identity.Name))
            {
                return BadRequest("Không phải là trưởng phòng không sử dụng được chức năng này !");
            }
            var docIn = await _documentIncommingService.GetById(DocSendId);
            if(docIn == null || docIn.Document_Incomming_State != 5)
            {
                return BadRequest("Document Incomming không tồn tại hoặc không ở trong trạng thái đang xử lý !");
            }
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileContent = new FileContent();
                    var dto = fileContent.ToFile(file);
                    if (!_fileService.Create(dto))

                        return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
                    filesDocIn.Add(dto.File_Id);
                }
            }
            if (filesDocIn.Count > 0)
            {
                foreach (var file in filesDocIn)
                {
                    var DocFile = new Document_Send_FileDTO
                    {
                        Document_Send_Id = DocSendId,
                        File_Id = file,
                        User_Id = User.Identity.Name,
                    };
                    if (!_documentSendFileService.CreateDocSendFile(DocFile))
                        return BadRequest("Lỗi khi gửi file: " + file);
                }
            }
            if (!await _documentIncommingService.UpdateState(DocSendId, 6))
                return BadRequest("Lỗi khi cập nhật trạng thái Doc In");
            if (!_documentSendService.UpdateState(DocSendId, 4))
                return BadRequest("Lỗi khi cập nhật trạng thái Doc Send");
            return Ok("Đã xử lý hoàn thành đề xuất thành công !");
           
        }
        [HttpPut("CompleteTheWork")]
        public IActionResult CompleteTheWork
            (string DocSendId,
            [FromForm] List<IFormFile> files, [FromForm] List<string> filesDocIn)
        {
            if (DocSendId == null)
            {
                return BadRequest("Thông tin không đầy đủ");
            }
            if (!_departmentService.isHead(User.Identity.Name))
            {
                return BadRequest("Không phải là trưởng phòng không sử dụng được chức năng này !");
            }
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileContent = new FileContent();
                    var dto = fileContent.ToFile(file);
                    if (!_fileService.Create(dto))

                        return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
                    filesDocIn.Add(dto.File_Id);
                }
            }
            if (filesDocIn.Count > 0)
            {
                foreach (var file in filesDocIn)
                {
                    var DocFile = new Document_Send_FileDTO
                    {
                        Document_Send_Id = DocSendId,
                        File_Id = file,
                        User_Id = User.Identity.Name,
                    };
                    if (!_documentSendFileService.CreateDocSendFile(DocFile))
                        return BadRequest("Lỗi khi gửi file: " + file);
                }
            }
            if (!_documentSendService.UpdateState(DocSendId, 4))
                return BadRequest("Lỗi khi cập nhật trạng thái Doc Send");                     
            return Ok("Đã xử lý hoàn thành công việc bàn giao thành công !");

        }

        [HttpPut("UpdateDocSend")]
        public IActionResult UpdateDocumentSend
            (string doc_id, string Title, string Content, DateTime TimeStart, DateTime Deadline, int CatagoryId, [FromForm] List<IFormFile> files,
            [FromForm] List<string> fileOlds, [FromForm] List<string> DepartmentIds, [FromForm] List<string> UserReceives)
        {
            if (doc_id == null || Title == null || Content == null || CatagoryId == 0)
            {
                return BadRequest("Thông tin không đầy đủ");
            }
            var docState = _documentSendService.GetById(doc_id).Document_Send_State;
            if (docState != 0 && docState != 2)
            {
                return BadRequest("Document không được chỉnh sửa !");
            }
            var document_SendDTO = new Document_SendDTO
            {
                Document_Send_Id = doc_id,
                Document_Send_Title = Title,
                Document_Send_Content = Content,
                Document_Send_Time = DateTime.UtcNow,
                Document_Send_TimeStart = TimeStart,
                Document_Send_Deadline = Deadline,
                Document_Send_Catagory = CatagoryId,
                Document_Send_State = 3,
            };
            if (_documentSendService.Update(document_SendDTO))
            {
                _documentSendFileService.DeleteDocSendFilesByDocId(doc_id);
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
                        var DocFile = new Document_Send_FileDTO
                        {
                            Document_Send_Id = doc_id,
                            File_Id = fileId,
                        };
                        if (!_documentSendFileService.CreateDocSendFile(DocFile))
                            return BadRequest("Lỗi khi gửi file: " + fileId);
                    }
                }

                _userReceiveDocumentService.DeleteUserReceivesByDocId(doc_id);
                var UserDepartments = new List<string>();
                if (DepartmentIds != null)
                {
                    foreach (var depId in DepartmentIds)
                    {
                        var departmentEnity = _departmentService.GetDepartmentById(depId);
                        if (departmentEnity != null)
                        {
                            if (departmentEnity.Department_Head != null)
                            {
                                UserDepartments.Add(departmentEnity.Department_Head);
                                var userReceive = new User_Receive_DocumentDTO
                                {
                                    Document_Send_Id = doc_id,
                                    User_Id = departmentEnity.Department_Head,
                                    Department_Id = depId,
                                };
                                if (!_userReceiveDocumentService.CreateUserReceice(userReceive))
                                    return BadRequest("Lỗi khi gửi Doc cho department: " + departmentEnity.Department_Name);
                            }
                        }
                    }
                }
                if (UserReceives != null)
                {
                    foreach (var user in UserReceives)
                    {
                        if (!UserDepartments.Contains(user))
                        {
                            var userReceive = new User_Receive_DocumentDTO
                            {
                                Document_Send_Id = doc_id,
                                User_Id = user,
                                Department_Id = null,
                            };
                            if (!_userReceiveDocumentService.CreateUserReceice(userReceive))
                                return BadRequest("Lỗi khi gửi Doc cho user: " + user);
                        }
                    }
                }
                return Ok("Cập nhật Document Send thành công !");
            }
            else
            {
                return BadRequest("Lỗi khi cập nhật DocumentSend !");
            }

        }

        // PUT: trả về để chỉnh sửa
        [HttpPut("UpdateState")]
        public IActionResult UpdateStateDocSend(string DocId, string Comment, int State)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_documentSendService.CommentEditAndStateByDocId(DocId, State, Comment))
                
                return Ok("Đã cập nhật state Document Send !");
            return BadRequest("Lỗi khi cập nhật document Send !");
        }

        [HttpPut("UpdateSeenTrue/{id}")]
        public IActionResult UpdateSeenDocSend(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_documentSendService.UpdateSeen(id, User.Identity.Name))
                return Ok("Đã xem Document Send !");
            return BadRequest("Lỗi khi cập nhật document Send !");
        }

        // PUT: cập nhật trạng thái
        /*[HttpPut("UpdateState/{id}/{state:int}")]
        public IActionResult UpdateStateDocSend([FromRoute] string id, int state)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_documentSendService.UpdateState(id, state))
                return Ok("Update State Document Send thành công !");
            return BadRequest("Lỗi khi cập nhật state document Send !");
        }*/

        [HttpDelete("{id}")]
        public IActionResult DeleteDocumentSend([FromRoute] string id)
        {
            var document_SendDTO = _documentSendService.GetById(id);
            if (document_SendDTO == null)
            {
                return BadRequest("DocumentSend không tồn tại !");
            }
            if (_documentSendService.Delete(id))
            {
                return Ok("Đã xóa DocumentSend thành công !");
            }
            else
            {
                return BadRequest("Lỗi khi xóa documentSend !");
            }
        }
    }
}
