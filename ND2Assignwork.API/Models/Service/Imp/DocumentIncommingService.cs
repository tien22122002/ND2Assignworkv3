using Microsoft.EntityFrameworkCore;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using System.Collections.Generic;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class DocumentIncommingService : IDocumentIncommingService
    {
        private readonly DataContext _context;

        public DocumentIncommingService(DataContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Document_IncommingDTO>> GetAll()
        {
            return await _context.Document_Incomming.Select(p => new Document_IncommingDTO
            {
                Document_Incomming_Id = p.Document_Incomming_Id,
                Document_Incomming_Title = p.Document_Incomming_Title,
                Document_Incomming_Content = p.Document_Incomming_Content,
                Document_Incomming_Time = p.Document_Incomming_Time,
                Document_Incomming_UserSend = p.Document_Incomming_UserSend,
                Document_Incomming_UserReceive = p.Document_Incomming_UserReceive,
                Document_Incomming_State = p.Document_Incomming_State,
                Document_Incomming_Comment = p.Document_Incomming_Comment,
                Document_Incomming_Id_Forward = p.Document_Incomming_Id_Forward,
            }).ToListAsync();
        }
        public async Task<IEnumerable<Document_Incomming>> GetAllListUserSend(string user_id)
        {
            /*bool hasPermissionFour = _context.User_Permission.Any(p => p.User_Id == user_id && p.Permission_Id == 4);
            IQueryable<Document_Incomming> userDocInEntities;
            if (hasPermissionFour)
            {
                userDocInEntities = _context.Document_Incomming;
            }
            else
            {
                
            }*/
            var userDocInEntities = await _context.Document_Incomming
                .Include(d => d.ForwardDocument)
                .Include(u => u.Receiver).ThenInclude(u => u.Department)
                    .Where(up => up.Document_Incomming_UserSend == user_id)
                    .OrderByDescending(p => p.Document_Incomming_Time)
                    .ToListAsync();
            
            return userDocInEntities;
        }
        public async Task<IEnumerable<Document_Incomming>> GetListDocUserReceiveInDepartment(string user_id)
        {
            var listTest = await _context.Document_Incomming
                .Include(u => u.Sender).ThenInclude(u => u.Department).ThenInclude(u => u.Users)
                .Include(r => r.Receiver)
                .Where(up => up.Document_Incomming_UserReceive == user_id && up.Sender.Department.Users.Any(u => u.User_Department == up.Receiver.User_Department))
                .OrderByDescending(p => p.Document_Incomming_Time)
                .ToListAsync();
            return listTest;
        }
        /*public IEnumerable<Document_IncommingDTO> GetListDocUserReceiveInDepartments(string user_id)
        {
            var department_id = _context.User_Account.Where(u => u.User_Id == user_id).FirstOrDefault().User_Department;
            var lstUserDepartment = _context.User_Account.Where(u => u.User_Department == department_id).ToList();

            var allUserDocs = _context.Document_Incomming
                    .Where(up => up.Document_Incomming_UserReceive == user_id)
                    .ToList();

            var userDocInEntities = allUserDocs
                                .Where(up => lstUserDepartment.Any(u => u.User_Id == up.Document_Incomming_UserSend))
                                .OrderByDescending(p => p.Document_Incomming_Time)
                                .ToList();
            var userDocDTOs = userDocInEntities
                .Select(p => new Document_IncommingDTO
                {
                    Document_Incomming_Id = p.Document_Incomming_Id,
                    Document_Incomming_Title = p.Document_Incomming_Title,
                    Document_Incomming_Content = p.Document_Incomming_Content,
                    Document_Incomming_Time = p.Document_Incomming_Time,
                    Document_Incomming_UserSend = p.Document_Incomming_UserSend,
                    Document_Incomming_UserReceive = p.Document_Incomming_UserReceive,
                    Document_Incomming_State = p.Document_Incomming_State,
                    Document_Incomming_Comment = p.Document_Incomming_Comment,
                    Document_Incomming_Id_Forward = p.Document_Incomming_Id_Forward,
                })
                .ToList();

            return userDocDTOs;
        }*/
        public async Task<IEnumerable<Document_Incomming>> GetListDocUserReceiveOutDepartment(string user_id)
        {
            var listTest = await _context.Document_Incomming
                .Include(u => u.Sender).ThenInclude(u => u.Department).ThenInclude(u => u.Users)
                .Include(r => r.Receiver)
                .Where(up => up.Document_Incomming_UserReceive == user_id && up.Sender.Department.Users.Any(u => u.User_Department == up.Receiver.User_Department))
                .OrderByDescending(p => p.Document_Incomming_Time)
                .ToListAsync();
            return listTest;
        }
        public IEnumerable<Document_IncommingDTO> GetListDocUserReceiveOutDepartments(string user_id)
        {
            var department_id = _context.User_Account.Where(u => u.User_Id == user_id).FirstOrDefault().User_Department;
            var lstUserDepartment = _context.User_Account.Where(u => u.User_Department == department_id).ToList();

            var allUserDocs = _context.Document_Incomming
                    .Where(up => up.Document_Incomming_UserReceive == user_id)
                    .ToList();

            var userDocInEntities = allUserDocs
                                .Where(up => !lstUserDepartment.Any(u => u.User_Id == up.Document_Incomming_UserSend))
                                .OrderByDescending(p => p.Document_Incomming_Time)
                                .ToList();
            var userDocDTOs = userDocInEntities
                .Select(p => new Document_IncommingDTO
                {
                    Document_Incomming_Id = p.Document_Incomming_Id,
                    Document_Incomming_Title = p.Document_Incomming_Title,
                    Document_Incomming_Content = p.Document_Incomming_Content,
                    Document_Incomming_Time = p.Document_Incomming_Time,
                    Document_Incomming_UserSend = p.Document_Incomming_UserSend,
                    Document_Incomming_UserReceive = p.Document_Incomming_UserReceive,
                    Document_Incomming_State = p.Document_Incomming_State,
                    Document_Incomming_Comment = p.Document_Incomming_Comment,
                    Document_Incomming_Id_Forward = p.Document_Incomming_Id_Forward,
                })
                .ToList();

            return userDocDTOs;
        }
        public async Task<IEnumerable<Document_Incomming>> GetListByUserReceiveIsSeen(string user_id)
        {
            var ListDocInIsSeen = await _context.Document_Incomming
                                        .Include(u => u.Sender).ThenInclude(u => u.Department)
                                        .Include(u => u.Receiver).ThenInclude(u => u.Department)
                                        .Where(d => d.Document_Incomming_IsSeen == false &&
                                                    ((d.Document_Incomming_UserReceive == user_id &&
                                                      (d.Document_Incomming_State == 0 ||
                                                       d.Document_Incomming_State == 21 ||
                                                       d.Document_Incomming_State == 6)
                                                      )
                                                     ||
                                                     (d.Document_Incomming_UserSend == user_id &&
                                                      !(d.Document_Incomming_State == 0 ||
                                                       d.Document_Incomming_State == 21 ||
                                                       d.Document_Incomming_State == 6)
                                                      )
                                                    )
                                              )
                                        .OrderByDescending(d => d.Document_Incomming_Time)
                                        .ToListAsync();

            return (ListDocInIsSeen);
            

            /*var allUserDocs = _context.Document_Incomming
                    .Where(up => up.Document_Incomming_UserReceive == user_id && up.Document_Incomming_IsSeen == false && up.Document_Incomming_State == 0)
                    .ToList();
            var docincommingUserSend = _context.Document_Incomming.Where(d => d.Document_Incomming_UserSend == user_id && d.Document_Incomming_IsSeen == false && d.Document_Incomming_State > 0).ToList();

            foreach( var d in docincommingUserSend )
            {
                allUserDocs.Add(d);
            }

            var userDocInEntities = allUserDocs
                                .OrderByDescending(p => p.Document_Incomming_Time)
                                .ToList();
            var userDocDTOs = userDocInEntities
                .Select(p => new Document_IncommingDTO
                {
                    Document_Incomming_Id = p.Document_Incomming_Id,
                    Document_Incomming_Title = p.Document_Incomming_Title,
                    Document_Incomming_Content = p.Document_Incomming_Content,
                    Document_Incomming_Time = p.Document_Incomming_Time,
                    Document_Incomming_UserSend = p.Document_Incomming_UserSend,
                    Document_Incomming_UserReceive = p.Document_Incomming_UserReceive,
                    Document_Incomming_State = p.Document_Incomming_State,
                    Document_Incomming_Comment = p.Document_Incomming_Comment,
                    Document_Incomming_Id_Forward = p.Document_Incomming_Id_Forward,
                    Document_Incomming_TimeUpdate = p.Document_Incomming_TimeUpdate,
                })
                .ToList();
*/
        }
        public int getNumberNotification(string userId)
        {
            var allUserDocs = _context.Document_Incomming
                    .Where(up => up.Document_Incomming_UserReceive == userId && up.Document_Incomming_IsSeen == false && up.Document_Incomming_State == 0)
                    .ToList();
            var docincommingUserSend = _context.Document_Incomming.Where(d => d.Document_Incomming_UserSend == userId && d.Document_Incomming_IsSeen == false && d.Document_Incomming_State > 0).ToList();

            foreach (var d in docincommingUserSend)
            {
                allUserDocs.Add(d);
            }
            if (allUserDocs.Count() <= 0 || allUserDocs == null)
                return 0;
            return allUserDocs.Count();
        }

        public (IEnumerable<Document_IncommingDTO>, int) GetListByUserSendLimitNumberPage(string user_id, int limit, int numberPage)
        {
            
            var userDocInEntities = _context.Document_Incomming
                   .Where(up => up.Document_Incomming_UserSend == user_id)
                   .OrderByDescending(p => p.Document_Incomming_Time);
            int totalNumberPage = (userDocInEntities.Count() + limit - 1) / limit;

            int offset = (numberPage - 1) * limit;

            var userDocDTOs = userDocInEntities
                .Skip(offset)
                .Take(limit)
                .Select(p => new Document_IncommingDTO
                {
                    Document_Incomming_Id = p.Document_Incomming_Id,
                    Document_Incomming_Title = p.Document_Incomming_Title,
                    Document_Incomming_Content = p.Document_Incomming_Content,
                    Document_Incomming_Time = p.Document_Incomming_Time,
                    Document_Incomming_UserSend = p.Document_Incomming_UserSend,
                    Document_Incomming_UserReceive = p.Document_Incomming_UserReceive,
                    Document_Incomming_State = p.Document_Incomming_State,
                    Document_Incomming_Comment = p.Document_Incomming_Comment,
                    Document_Incomming_Id_Forward = p.Document_Incomming_Id_Forward,
                })
                .ToList();

            return (userDocDTOs, totalNumberPage);
        }
        public (IEnumerable<Document_IncommingDTO>, int) GetListByUserReceiveLimitNumberPage(string user_id, int limit, int numberPage)
        {
            var department_id = _context.User_Account.Where(u => u.User_Id == user_id).FirstOrDefault().User_Department;
            var lstUserDepartment = _context.User_Account.Where(u => u.User_Department == department_id).ToList();

            var allUserDocs = _context.Document_Incomming
                    .Where(up => up.Document_Incomming_UserReceive == user_id)
                    .ToList();

            var userDocInEntities = allUserDocs
                                .Where(up => lstUserDepartment.Any(u => u.User_Id == up.Document_Incomming_UserSend))
                                .OrderByDescending(p => p.Document_Incomming_Time)
                                .ToList();
            int totalNumberPage = (userDocInEntities.Count() + limit - 1) / limit;

            int offset = (numberPage - 1) * limit;

            var userDocDTOs = userDocInEntities
                .Skip(offset)
                .Take(limit)
                .Select(p => new Document_IncommingDTO
                {
                    Document_Incomming_Id = p.Document_Incomming_Id,
                    Document_Incomming_Title = p.Document_Incomming_Title,
                    Document_Incomming_Content = p.Document_Incomming_Content,
                    Document_Incomming_Time = p.Document_Incomming_Time,
                    Document_Incomming_UserSend = p.Document_Incomming_UserSend,
                    Document_Incomming_UserReceive = p.Document_Incomming_UserReceive,
                    Document_Incomming_State = p.Document_Incomming_State,
                    Document_Incomming_Comment = p.Document_Incomming_Comment,
                    Document_Incomming_Id_Forward = p.Document_Incomming_Id_Forward,
                })
                .ToList();

            return (userDocDTOs, totalNumberPage);
        }



        public async Task<Document_IncommingDTO> GetById(string id)
        {
            var p = await _context.Document_Incomming.FirstOrDefaultAsync(d => d.Document_Incomming_Id == id);
            if (p == null)
            {
                return null;
            }
            return new Document_IncommingDTO
            {
                Document_Incomming_Id = p.Document_Incomming_Id,
                Document_Incomming_Title = p.Document_Incomming_Title,
                Document_Incomming_Content = p.Document_Incomming_Content,
                Document_Incomming_Time = p.Document_Incomming_Time,
                Document_Incomming_UserSend = p.Document_Incomming_UserSend,
                Document_Incomming_UserReceive = p.Document_Incomming_UserReceive,
                Document_Incomming_State = p.Document_Incomming_State,
                Document_Incomming_Comment = p.Document_Incomming_Comment,
                Document_Incomming_Id_Forward = p.Document_Incomming_Id_Forward,
                Document_Incomming_TimeUpdate = p.Document_Incomming_TimeUpdate
            };
        }
        public async Task<Document_Incomming> GetDocInByDocInIdAsync(string docId)
        {
            var docIn = await _context.Document_Incomming
                            .Include(u => u.Sender).ThenInclude(d => d.Department)
                            .Include(u => u.Receiver).ThenInclude(d => d.Department)
                            .FirstOrDefaultAsync(doc => doc.Document_Incomming_Id == docId);
            return docIn;
        }


        public async Task<List<Document_Incomming>> GetAllDocForwardByDocId(string docId)
        {
           

            var DocReceive = _context.Document_Incomming.Find(docId);
            if(DocReceive == null)
                return null;

            var DocReceiveForwardEntity = await _context.Document_Incomming
                .Include(u => u.Sender).ThenInclude(u => u.Department)
                .Include(u => u.Receiver).ThenInclude(u => u.Department)
                .FirstOrDefaultAsync(d => d.Document_Incomming_Id_Forward == docId);
            if (DocReceiveForwardEntity == null)
                return new List<Document_Incomming>();

            var DocReceiveForward = DocReceiveForwardEntity;
            
            var DocReceiveForwardList = new List<Document_Incomming>{DocReceiveForward};
            while (DocReceiveForwardEntity != null)
            {
                while (DocReceiveForwardEntity != null)
                {
                    DocReceiveForwardEntity = await _context.Document_Incomming
                        .Include(u => u.Sender).ThenInclude(u => u.Department)
                        .Include(d => d.Receiver).ThenInclude(d => d.Department)
                        .FirstOrDefaultAsync(d => d.Document_Incomming_Id_Forward == DocReceiveForward.Document_Incomming_Id);
                    if (DocReceiveForwardEntity != null)
                    {

                        DocReceiveForward = DocReceiveForwardEntity;
                        DocReceiveForwardList.Add(DocReceiveForward);
                    }
                }
            }
            return DocReceiveForwardList;
        }

        
        public async Task<Document_Incomming> GetDocForwardByDocIdAsync(string docId)
        {
            var DocReceiveForwardEntity = await _context.Document_Incomming
                .Include(u => u.Receiver).ThenInclude(u => u.Department)
                .FirstOrDefaultAsync(d => d.Document_Incomming_Id_Forward == docId);
            if (DocReceiveForwardEntity == null)
                return new Document_Incomming();

            var DocReceiveForward = DocReceiveForwardEntity;
            while (DocReceiveForwardEntity != null)
            {
                DocReceiveForwardEntity = await _context.Document_Incomming
                    .Include(d => d.Receiver).ThenInclude(d => d.Department)
                    .FirstOrDefaultAsync(d => d.Document_Incomming_Id_Forward == DocReceiveForward.Document_Incomming_Id);
                if(DocReceiveForwardEntity != null)
                {
                    DocReceiveForward = DocReceiveForwardEntity;
                }
            }
            return DocReceiveForward;
        }

        public async Task<bool> Create(Document_IncommingDTO p)
        {

            var documentSendEntity = new Domain.Document_Incomming
            {
                Document_Incomming_Id = p.Document_Incomming_Id,
                Document_Incomming_Title = p.Document_Incomming_Title,
                Document_Incomming_Content = p.Document_Incomming_Content,
                Document_Incomming_Time = p.Document_Incomming_Time,
                Document_Incomming_UserSend = p.Document_Incomming_UserSend,
                Document_Incomming_UserReceive = p.Document_Incomming_UserReceive,
                Document_Incomming_State = 0,
                Document_Incomming_Id_Forward = p.Document_Incomming_Id_Forward,
                Document_Incomming_IsSeen = false,
                Document_Incomming_TimeUpdate = null,
                Document_Incomming_Comment = p.Document_Incomming_Comment,

            };
            await _context.Document_Incomming.AddAsync(documentSendEntity);

            try
            {
                int recordsAffected = await _context.SaveChangesAsync();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }
        /*public bool SetHandOverDocument(string doc_id ,List<string> user_id)
        {
            var p = _context.Document_Incomming.Find(doc_id);
            if (p == null)
            {
                return false;
            }
            var timenow = DateTime.Now;
            foreach(var u in user_id)
            {
                var userReceive = new User_Receive_Document
                {
                    User_Id = u,
                    Document_Send_Id = doc_id,
                };
                _context.User_Receive_Document.Add(userReceive);
            }
            try
            {
                int recordsAffected = _context.SaveChanges();
                UpdateState(doc_id, 2);
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }*/
        public bool UpdateHandOverDocument(string doc_id, List<string> user_id)
        {
            var p = _context.Document_Incomming.Find(doc_id);
            if (p == null)
            {
                return false;
            }
            
            var userReceiveDocIn = _context.User_Receive_Document
                .Where(u => u.Document_Send_Id == doc_id)
                .ToList();

            
            // Thêm mới User_Receive_Document nếu không tồn tại trong danh sách
            foreach (var u in user_id)
            {
                if (userReceiveDocIn.Any(userId => userId.User_Id != u))
                {
                    var userReceive = new User_Receive_Document
                    {
                        User_Id = u,
                        Document_Send_Id = doc_id,
                    };
                    _context.User_Receive_Document.Add(userReceive);
                };
                
            }
            // Loại bỏ User_Receive_Document nếu không tồn tại trong danh sách user_id
            foreach (var u in userReceiveDocIn)
            {
                if(user_id.Any(user => user != u.User_Id))
                {
                    var UserReciveDocument = _context.User_Receive_Document
                        .Where(ud => ud.User_Id == u.User_Id && ud.Document_Send_Id == doc_id)
                        .FirstOrDefault();
                    if (UserReciveDocument != null)
                    {
                        _context.User_Receive_Document.Remove(UserReciveDocument);
                    }
                    
                }
            }
            try
            {
                int recordsAffected = _context.SaveChanges();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }
        
        
        public async Task<bool> Update(Document_IncommingDTO Document_IncommingDTO)
        {
            var documentSendEntity =await _context.Document_Incomming.FirstOrDefaultAsync(u => u.Document_Incomming_Id == Document_IncommingDTO.Document_Incomming_Id);
            if (documentSendEntity == null)
                return false;
            if (documentSendEntity.Document_Incomming_State != 0)
            {
                return false;
            }
            documentSendEntity.Document_Incomming_Title = Document_IncommingDTO.Document_Incomming_Title;
            documentSendEntity.Document_Incomming_Content = Document_IncommingDTO.Document_Incomming_Content;
            documentSendEntity.Document_Incomming_TimeUpdate = Document_IncommingDTO.Document_Incomming_TimeUpdate;
            documentSendEntity.Document_Incomming_State = Document_IncommingDTO.Document_Incomming_State;
            documentSendEntity.Document_Incomming_IsSeen = Document_IncommingDTO.Document_Incomming_IsSeen;
            

            try
            {
                int recordsAffected = await _context.SaveChangesAsync();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }
        /*public bool CommentEditByDocId(string docId, string comment)
        {
            var documentSendEntity = _context.Document_Send.Find(docId);
            if (documentSendEntity == null)
            {
                return false;
            }
            documentSendEntity.Document_Send_Comment = comment;
            try
            {
                int recordsAffected = _context.SaveChanges();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }*/
        public async Task<bool> UpdateStateComment(string doc_id, int state, string comment)
        {
            var documentSendEntity = await _context.Document_Incomming.FirstOrDefaultAsync(d => d.Document_Incomming_Id == doc_id);
            if (documentSendEntity == null)
                return false;
            documentSendEntity.Document_Incomming_State = state;
            documentSendEntity.Document_Incomming_Comment = comment;
            documentSendEntity.Document_Incomming_IsSeen = false;
            documentSendEntity.Document_Incomming_TimeUpdate = DateTime.Now;
            try
            {
                int recordsAffected = await _context.SaveChangesAsync();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> UpdateState(string doc_id, int state)
        {
            var documentSendEntity = await _context.Document_Incomming.FirstOrDefaultAsync(d => d.Document_Incomming_Id == doc_id);
            if (documentSendEntity == null)
                return false;
            documentSendEntity.Document_Incomming_State = state;
            documentSendEntity.Document_Incomming_IsSeen= false;
            documentSendEntity.Document_Incomming_TimeUpdate = DateTime.Now;
            try
            {
                int recordsAffected = await _context.SaveChangesAsync();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }
        public async Task<bool> UpdateSeenTrue(string doc_id)
        {
            var documentSendEntity = await _context.Document_Incomming.FirstOrDefaultAsync(d => d.Document_Incomming_Id == doc_id);
            if (documentSendEntity == null)
                return false;
            documentSendEntity.Document_Incomming_IsSeen = true;
            try
            {
                int recordsAffected = await _context.SaveChangesAsync();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi cập nhập dữ liệu: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(string id)
        {
            var documentSendEntity = await _context.Document_Incomming.FirstOrDefaultAsync(d => d.Document_Incomming_Id == id);
            if (documentSendEntity == null)
            {
                return false;
            }
            _context.Document_Incomming.Remove(documentSendEntity);

            try
            {
                int recordsAffected = await _context.SaveChangesAsync();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }

        }
        public bool isExist(string id)
        {
            bool exists = _context.Document_Incomming.Any(d => d.Document_Incomming_Id == id);
            return exists;
        }
    }
}
