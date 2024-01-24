using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.Service;
using ND2Assignwork.API.Models.Service.Imp;
using System.Net.WebSockets;

namespace ND2Assignwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController,Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IDiscussService _discussService;
        private readonly ITaskFileService _taskFileService;
        private readonly IFileService _fileService;
        private readonly IDocumentSendService _documentSendService;
        private readonly IUserAccountService _userAccountService;
        private readonly IDepartmentService _departmentService;
        private readonly ITaskCategoryService _taskCategoryService;

        public TaskController(ITaskService taskService, IDiscussService discussService, ITaskFileService taskFileService,
            IFileService fileService, IDocumentSendService documentSendService, IUserAccountService userAccountService,
            IDepartmentService departmentService, ITaskCategoryService taskCategoryService)
        {
            _taskService = taskService;
            _discussService = discussService;
            _taskFileService = taskFileService;
            _fileService = fileService;
            _documentSendService = documentSendService;
            _userAccountService = userAccountService;
            _departmentService = departmentService;
            _taskCategoryService = taskCategoryService;
        }

        // GET ALL Task
        [HttpGet, Authorize(Roles = "SuperAdmin")]

        public IActionResult GetAll()
        {
            return Ok(_taskService.GetAll());
        }

        [HttpGet("GetByTaskId/{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var taskDTO = _taskService.GetById(id);
            if (taskDTO == null)
            {
                return BadRequest("Không tìm thấy Task !");
            }
            var fileIds = await _fileService.GetFileByTaskIdAsync(id);
            if (fileIds != null)
            {
                _ = fileIds.Select(f => new
                {
                    f.File_Id,
                    f.File_Name,
                    f.ContentType,
                });
            }
            var fileComfirm = await _fileService.GetFileByTaskIdCofirmAsync(id);
            if (fileComfirm != null)
            {
                _ = fileIds.Select(f => new
                {
                    f.File_Id,
                    f.File_Name,
                    f.ContentType,
                });
            }

            return Ok(new
            {
                Task =  new
                {
                    taskDTO.Task_Id,
                    taskDTO.Task_Title,
                    taskDTO.Task_Content,
                    Task_Catagory_Name = _taskCategoryService.GetTaskCategoryById(taskDTO.Task_Category).Category_Name,
                    Task_DateSend = taskDTO.Task_DateSend.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    Task_DateStart = taskDTO.Task_DateStart?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    Task_DateEnd = taskDTO.Task_DateEnd?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    taskDTO.Task_Person_Send,
                    UserSend_FullName = _userAccountService.GetUserAccountById(taskDTO.Task_Person_Send).User_FullName,
                    taskDTO.Task_Person_Receive,
                    UserReceive_FullName = _userAccountService.GetUserAccountById(taskDTO.Task_Person_Receive).User_FullName,
                    taskDTO.Task_State,
                    taskDTO.Document_Send_Id,
                    Task_TimeUpdate = taskDTO.Task_TimeUpdate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                },
                FileIds = fileIds,
                FileComfirms = fileComfirm

            });
        }
        [HttpGet("GetListTaskByDocSendId/{docSendId}")]
        public IActionResult GetListTaskByDocSendId([FromRoute] string docSendId)
        {
            var taskDTO = _taskService.GetAllTaskDocSendId(docSendId);
            if (!taskDTO.Any())
            {
                return Ok(new List<string>());
            }
            return Ok(taskDTO.Select(t => new
            {
                t.Task_Id,
                t.Task_Title,
                t.Task_Content,
                Task_Catagory_Name = _taskCategoryService.GetTaskCategoryById(t.Task_Category).Category_Name,
                Task_DateSend = t.Task_DateSend.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Task_DateStart = t.Task_DateStart?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Task_DateEnd = t.Task_DateEnd?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                t.Task_Person_Send,
                UserSend_FullName = _userAccountService.GetUserAccountById(t.Task_Person_Send).User_FullName,
                t.Task_Person_Receive,
                UserReceive_FullName = _userAccountService.GetUserAccountById(t.Task_Person_Receive).User_FullName,
                t.Task_State,
                t.Document_Send_Id,
                
            }));
        }
        [HttpGet("GetListTaskSendUserLogin")]
        public IActionResult GetListTaskSendUserLogin()
        {
            var taskDTO = _taskService.GetAllTaskUserSend(User.Identity.Name);
            if (taskDTO == null)
            {
                return Ok(new List<string>());
            }
            return Ok(taskDTO.Select(t => new
            {
                t.Task_Id,
                t.Task_Title,
                t.Task_Content,
                Task_Catagory_Name = _taskCategoryService.GetTaskCategoryById(t.Task_Category).Category_Name,
                Task_DateSend = t.Task_DateSend.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Task_DateStart = t.Task_DateStart?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Task_DateEnd = t.Task_DateEnd?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                t.Task_Person_Send,
                UserSend_FullName = _userAccountService.GetUserAccountById(t.Task_Person_Send)?.User_FullName,
                t.Task_Person_Receive,
                UserReceive_FullName = _userAccountService.GetUserAccountById(t.Task_Person_Receive)?.User_FullName,
                t.Task_State,
                t.Document_Send_Id,

            }));
        }
        [HttpGet("GetListTaskReceiveUserLogin")]
        public IActionResult GetListTaskReceiveUserLogin()
        {
            var taskDTO = _taskService.GetAllTaskUserReceive(User.Identity.Name);
            if (!taskDTO.Any())
            {
                return Ok(new List<string>());
            }
            return Ok(taskDTO.Select(t => new
            {
                t.Task_Id,
                t.Task_Title,
                t.Task_Content,
                Task_Catagory_Name = _taskCategoryService.GetTaskCategoryById(t.Task_Category).Category_Name,
                Task_DateSend = t.Task_DateSend.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Task_DateStart = t.Task_DateStart?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Task_DateEnd = t.Task_DateEnd?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                t.Task_Person_Send,
                UserSend_FullName = _userAccountService.GetUserAccountById(t.Task_Person_Send).User_FullName,
                t.Task_Person_Receive,
                UserReceive_FullName = _userAccountService.GetUserAccountById(t.Task_Person_Receive).User_FullName,
                t.Task_State,
                t.Document_Send_Id,

            }));
        }

        [HttpGet("GetListTaskReceiveCurrentMonth")]
        public IActionResult GetListTaskReceiveCurrentMonth()
        {
            var date = DateTime.Now;
            var taskDTO = _taskService.GetAllTaskUserReceiveMonth(User.Identity.Name, date.Month, date.Year);
            if (!taskDTO.Any())
            {
                return Ok(new List<string>());
            }
            return Ok(taskDTO.Select(t => new
            {
                t.Task_Id,
                t.Task_Title,
                t.Task_Content,
                Task_Catagory_Name = _taskCategoryService.GetTaskCategoryById(t.Task_Category).Category_Name,
                Task_DateSend = t.Task_DateSend.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Task_DateStart = t.Task_DateStart?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Task_DateEnd = t.Task_DateEnd?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                t.Task_Person_Send,
                UserSend_FullName = _userAccountService.GetUserAccountById(t.Task_Person_Send).User_FullName,
                t.Task_Person_Receive,
                UserReceive_FullName = _userAccountService.GetUserAccountById(t.Task_Person_Receive).User_FullName,
                t.Task_State,
                t.Document_Send_Id,

            }));
        }
        [HttpGet("GetListTaskReceiveMonth")]
        public IActionResult GetListTaskReceiveMonth(int Month, int Year)
        {
            if(Month < 1 || Month > 12 || Year < 2000 || Year > 3000)
            {
                BadRequest("Tham số không hợp lệ !");
            }
            var taskDTO = _taskService.GetAllTaskUserReceiveMonth(User.Identity.Name, Month, Year);
            if (!taskDTO.Any())
            {
                return Ok(new List<string>());
            }
            return Ok(taskDTO.Select(t => new
            {
                t.Task_Id,
                t.Task_Title,
                t.Task_Content,
                Task_Catagory_Name = _taskCategoryService.GetTaskCategoryById(t.Task_Category).Category_Name,
                Task_DateSend = t.Task_DateSend.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Task_DateStart = t.Task_DateStart?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Task_DateEnd = t.Task_DateEnd?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                t.Task_Person_Send,
                UserSend_FullName = _userAccountService.GetUserAccountById(t.Task_Person_Send).User_FullName,
                t.Task_Person_Receive,
                UserReceive_FullName = _userAccountService.GetUserAccountById(t.Task_Person_Receive).User_FullName,
                t.Task_State,
                t.Document_Send_Id,

            }));
        }

        [HttpGet("GetListTaskUserReceiveNotification")]
        public IActionResult GetListTaskUserReceiveNotification()
        {
            var taskDTO = _taskService.GetAllTaskUserReceiveNotification(User.Identity.Name);
            if (taskDTO == null || !taskDTO.Any())
            {
                return Ok(new
                {
                    Task = new List<string>()
                });
            }
            return Ok(new
            {
                Task = taskDTO.Select(t => new
                {
                    t.Task_Id,
                    t.Task_Title,
                    Task_Catagory_Name = _taskCategoryService.GetTaskCategoryById(t.Task_Category).Category_Name,
                    Task_DateSend = t.Task_DateSend.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    t.Task_Person_Send,
                    UserSend_FullName = _userAccountService.GetUserAccountById(t.Task_Person_Send).User_FullName,
                    t.Task_State,
                    t.Document_Send_Id,
                    Task_TimeUpdate = t.Task_TimeUpdate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                }).ToList()
            });
        }

        [HttpGet("GetListDiscussByTaskId/{TaskId}")]
        public IActionResult GetByTaskId([FromRoute] string TaskId)
        {
            var discussDTO = _discussService.GetByTaskId(TaskId, User.Identity.Name);
            if (discussDTO == null || !discussDTO.Any())
            {
                return Ok(new List<string>());
            }

            var result = discussDTO.Select(d => new
            {
                d.Discuss_Task,
                d.Discuss_User,
                UserSend_Fullname = _userAccountService.GetUserAccountById(d.Discuss_User)?.User_FullName,
                d.Discuss_Content,
                Discuss_Time = d.Discuss_Time.ToString("yyyy-MM-ddTHH:mm:ssZ")
            }).ToList();

            return Ok(result);

        }
        [HttpGet("GetListDiscussByUsserNotification")]
        public IActionResult GetListDiscussByUsserNotification()
        {
            var discussDTO = _discussService.GetListDiscussByUsserNotification(User.Identity.Name);

            var discussList = discussDTO?.Select(d => new
            {
                d.Discuss_Task,
                d.Discuss_User,
                UserSend_Fullname = _userAccountService.GetUserAccountById(d.Discuss_User)?.User_FullName,
                d.Discuss_Content,
                Discuss_Time = d.Discuss_Time.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            }).ToList();

            var discussObjectList = discussList?.Cast<object>().ToList() ?? new List<object>();

            return Ok(new
            {
                Discuss = discussObjectList 
            });
        }


        [HttpPost("CreateTaskByDocSendId")]
        public IActionResult CreateTask( string DocSendId, string UserReceive,string Title, string Content, DateTime? TimeStart,
            DateTime? Deadline, int CatagoryId, [FromForm] List<IFormFile> files, [FromForm] List<string> fileDocSends)
        {
            if (!_departmentService.isHead(User.Identity.Name))
                return BadRequest("Chỉ trưởng phòng mới sử dụng chức năng này được !");
            if (DocSendId == null || UserReceive == null || Title == null || Content == null || CatagoryId <= 0)
                return BadRequest("Thông tin không đầy đủ !");
            var docSend = _documentSendService.GetById(DocSendId);
            if (docSend == null) 
                return BadRequest("Không tìm thấy DocumentSend !");
            if (docSend.Document_Send_State <3 || docSend.Document_Send_State > 4)
                return BadRequest("Document Send chưa duyệt hoặc đã hoàn thành !");
            DateTime currentTime = DateTime.UtcNow;
            if (TimeStart == null)
            {
                if(docSend.Document_Send_TimeStart != null && docSend.Document_Send_TimeStart >= currentTime)
                {
                    TimeStart = docSend.Document_Send_TimeStart;
                }
                else
                {
                    TimeStart = currentTime;
                }
            }
            if (Deadline == null)
            {
                if(docSend.Document_Send_Deadline != null)
                {
                    Deadline = docSend.Document_Send_Deadline;
                }
            }
            if (Deadline != null)
            {
                if (TimeStart >= Deadline)
                    return BadRequest("Thời gian bắt đầu và thời gian kết thúc không hợp lệ !");
            }
            string formattedDateTime = currentTime.ToString("yyMMddhhmmssffff");
            string task_id = "Task" + formattedDateTime;
            var taskDTO = new TaskDTO
            {
                Task_Id = task_id,
                Task_Title = Title,
                Task_Content = Content,
                Task_Category = CatagoryId,
                Task_DateSend = currentTime,
                Task_DateStart = TimeStart,
                Task_DateEnd = Deadline,
                Task_Person_Send = User.Identity.Name,
                Task_Person_Receive = UserReceive,
                Task_State = 3,
                Document_Send_Id = DocSendId,
            };
            if (_taskService.Create(taskDTO))
            {
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var fileContent = new FileContent();
                        var dto = fileContent.ToFile(file);
                        if (!_fileService.Create(dto))

                            return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
                        fileDocSends.Add(dto.File_Id);
                        
                    }
                }
                if(fileDocSends.Count > 0){
                    foreach (var fileid in fileDocSends){
                        var DocFile = new Task_FileDTO
                        {
                            Task_Id = task_id,
                            File_Id = fileid,
                        };
                        if (!_taskFileService.CreateTaskFile(DocFile))
                            return BadRequest("Lỗi khi gửi file: " + fileid);
                    }
                }
                
                return Ok("Đã thêm task !");
            }
            else
            {
                return BadRequest("Lỗi khi thêm task !");
            }
        }


        [HttpPost("CreateSendDiscuss")]
        public IActionResult CreateSendDiscuss(string TaskId, string Content)
        {
            if (TaskId == null || Content == null)
                return BadRequest("Thông tin không đầy đủ !");
            if (_taskService.GetById(TaskId) == null)
                return BadRequest("Task không tồn tại !");
            var disDTO = new DiscussDTO
            {
                Discuss_Task = TaskId,
                Discuss_Content = Content,
                Discuss_User = User.Identity.Name,
                Discuss_Time = DateTime.Now,
                Discuss_IsSeen = false,
            };
            if (_discussService.Create(disDTO))
                return Ok("Thêm discuss thành công !");
            return BadRequest("Lỗi khi thêm discuss !");
        }

        [HttpPut("ConfirmCompletionTask")]
        public IActionResult ConfirmCompletionTask(string taskId, [FromForm] List<IFormFile> files)
        {
            var fileDocSends = new List<string>();
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileContent = new FileContent();
                    var dto = fileContent.ToFile(file);
                    if (!_fileService.Create(dto))

                        return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
                    fileDocSends.Add(dto.File_Id);

                }
            }
            if (fileDocSends.Count > 0)
            {
                foreach (var fileid in fileDocSends)
                {
                    var DocFile = new Task_FileDTO
                    {
                        Task_Id = taskId,
                        File_Id = fileid,
                        User_Id = User.Identity.Name,
                    };
                    if (!_taskFileService.CreateTaskFile(DocFile))
                        return BadRequest("Lỗi khi gửi file: " + fileid);
                }
            }
            _taskService.UpdateState(taskId, 5);
            return Ok("Đã xác nhận thành công !");
            
        }
        [HttpPut("UpdateSendTaskTrue")]
        public IActionResult UpdateSendTaskTrue(string TaskId)
        {
            if (_taskService.UpdateIsSeen(TaskId))
            {
                return Ok(" Cập nhật thành công !");
            }
            return BadRequest("Lỗi khi cập nhật trạng thái đã xem !");
        }
        [HttpPut("UpdateSendDiscussTrue")]
        public IActionResult UpdateSendDiscussTrue( string TaskId)
        {
            var taskDTO = _taskService.GetById(TaskId);
            if (taskDTO == null)
            {
                return BadRequest("Task không tồn tại !");
            }
            var disDTO = new DiscussDTO
            {
                Discuss_Task = TaskId,
                Discuss_User = taskDTO.Task_Person_Send,
            };
            if (_discussService.Update(disDTO))
            {
                return Ok(" Cập nhật thành công !");
            }
            return BadRequest("Lỗi khi cập nhật trạng thái đã xem !");
        }
        [HttpPut("UpdateTask")]
        public IActionResult UpdateTask(string TaskId, string Title, string Content, DateTime? TimeStart,
            DateTime? Deadline, int CatagoryId, [FromForm] List<IFormFile> files, [FromForm] List<string> fileOld)
        {
            if (TaskId == null || Title == null || Content == null || CatagoryId == 0)
            {
                return BadRequest("Vui lòng điền đủ thông tin !");
            }
            var task = _taskService.GetById(TaskId);
            if(task == null)
            {
                return BadRequest("Task ID không tồn tại !");
            }
            if (!task.Task_Person_Send.Equals(User.Identity.Name))
            {
                return BadRequest("tài khoản này không có quyền chỉnh sửa !");
            }
            if (task.Task_State < 3 && task.Task_State > 4)
            {
                return BadRequest("Task không ở trạng thái 3 hoặc 4 để có thể chỉnh sửa !");
            }
            var taskDTO = new TaskDTO 
            {
                Task_Id = TaskId,
                Task_Title = Title,
                Task_Content = Content,
                Task_DateSend = DateTime.Now,
                Task_DateStart = TimeStart,
                Task_DateEnd = Deadline,
                Task_Category = CatagoryId,
                Task_State = 4,
                Task_IsSeen = false,
            };

            if (_taskService.Update(taskDTO))
            {
                if (fileOld.Count > 0)
                {
                    if(_taskService.DeleteFileTask(TaskId, fileOld))
                    {
                        return BadRequest("Lỗi khi xóa file cũ !");
                    }
                }
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var fileContent = new FileContent();
                        var dto = fileContent.ToFile(file);
                        if (!_fileService.Create(dto))

                            return BadRequest("Lỗi khi thêm file: " + dto.File_Name);
                        var DocFile = new Task_FileDTO
                        {
                            Task_Id = TaskId,
                            File_Id = dto.File_Id,
                        };
                        if (!_taskFileService.CreateTaskFile(DocFile))
                            return BadRequest("Lỗi khi gửi file: " + dto.File_Id);
                    }
                }
                return Ok("Update task thành công !");
            }
            else
            {
                return BadRequest("Lỗi khi cập nhật task !");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask([FromRoute] string id)
        {
            var taskDTO = _taskService.GetById(id);
            if (taskDTO == null)
            {
                return BadRequest("Task không tồn tại !");
            }
            if(_taskFileService.DeleteTaskFilesByTaskId(id) && _discussService.DeleteAllDiscussByTaskId(id))
            {
                if (_taskService.Delete(id))
                {
                    return Ok("Đã xóa task thành công !");
                }
                else
                {
                    return BadRequest("Lỗi khi xóa task !");
                }
            }
            else
            {
                return BadRequest("Lỗi khi xóa task !");
            }
            
        }

    }
}
