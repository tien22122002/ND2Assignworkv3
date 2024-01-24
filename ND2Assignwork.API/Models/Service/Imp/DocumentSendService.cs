using Microsoft.EntityFrameworkCore;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using System.Collections.Generic;
using System.Linq;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class DocumentSendService : IDocumentSendService
    {
        private readonly DataContext _context;

        public DocumentSendService(DataContext context)
        {
            this._context = context;
        }
        public async Task<IEnumerable<Document_SendDTO>> GetAll()
        {
            return await _context.Document_Send.Select(p => new Document_SendDTO
            {
                Document_Send_Id = p.Document_Send_Id,
                Document_Send_Title = p.Document_Send_Title,
                Document_Send_Content = p.Document_Send_Content,
                Document_Send_Time = p.Document_Send_Time,
                Document_Send_TimeStart = p.Document_Send_TimeStart,
                Document_Send_Deadline = p.Document_Send_Deadline,
                Document_Send_UserSend = p.Document_Send_UserSend,
                Document_Send_Catagory = p.Document_Send_Catagory,
                Document_Send_State = p.Document_Send_State,
                Document_Send_Comment = p.Document_Send_Comment,
            }).ToListAsync();
        }
        public async Task<Document_Send> GetDocSendFullByDocIdAsync(string docId)
        {
            var docSend = await _context.Document_Send
                .Include(d => d.UserAccount).ThenInclude(d => d.Department)
                .Include(d => d.ReceivedByUsers).ThenInclude(u => u.User_Account)
                .Include(d => d.ReceivedByUsers).ThenInclude(d => d.Department)
                .Include(d => d.Category)
                .Include(d => d.Document_Send_Files).ThenInclude(f => f.File)
                .FirstOrDefaultAsync(d => d.Document_Send_Id == docId);
            return docSend;
        }

        public Document_SendDTO GetById(string id)
        {
            var p = _context.Document_Send.Find(id);
            if (p == null)
            {

            }
            return new Document_SendDTO
            {
                Document_Send_Id = p.Document_Send_Id,
                Document_Send_Title = p.Document_Send_Title,
                Document_Send_Content = p.Document_Send_Content,
                Document_Send_Time = p.Document_Send_Time,
                Document_Send_TimeStart = p.Document_Send_TimeStart,
                Document_Send_Deadline = p.Document_Send_Deadline,
                Document_Send_UserSend = p.Document_Send_UserSend,
                Document_Send_Catagory = p.Document_Send_Catagory,
                Document_Send_State = p.Document_Send_State,
                Document_Send_Comment = p.Document_Send_Comment,
            };
        }
        public IEnumerable<Document_SendDTO> GetDocumentList(string user_id, bool isReceived)
        {
            var userDocIds = _context.Document_Send
                .Where(up => up.Document_Send_UserSend == user_id && up.Document_Send_Public == true)
                .Select(up => up.Document_Send_Id)
                .ToList();

            var userReceiveDocSendIds = _context.User_Receive_Document
                .Where(d => userDocIds.Contains(d.Document_Send_Id))
                .Select(d => d.Document_Send_Id)
                .ToList();

            var DocSendList = isReceived
                ? _context.Document_Send
                    .Where(up => userDocIds.Contains(up.Document_Send_Id) && userReceiveDocSendIds.Contains(up.Document_Send_Id))
                    .OrderByDescending(p => p.Document_Send_Time)
                    .Select(p => new Document_SendDTO
                    {
                        Document_Send_Id = p.Document_Send_Id,
                        Document_Send_Title = p.Document_Send_Title,
                        Document_Send_Content = p.Document_Send_Content,
                        Document_Send_Time = p.Document_Send_Time,
                        Document_Send_TimeStart = p.Document_Send_TimeStart,
                        Document_Send_Deadline = p.Document_Send_Deadline,
                        Document_Send_UserSend = p.Document_Send_UserSend,
                        Document_Send_Catagory = p.Document_Send_Catagory,
                        Document_Send_State = p.Document_Send_State,
                        Document_Send_Comment = p.Document_Send_Comment,
                    })
                    .ToList()
                : _context.Document_Send
                    .Where(up => userDocIds.Contains(up.Document_Send_Id) && !userReceiveDocSendIds.Contains(up.Document_Send_Id))
                    .OrderByDescending(p => p.Document_Send_Time)
                    .Select(p => new Document_SendDTO
                    {
                        Document_Send_Id = p.Document_Send_Id,
                        Document_Send_Title = p.Document_Send_Title,
                        Document_Send_Content = p.Document_Send_Content,
                        Document_Send_Time = p.Document_Send_Time,
                        Document_Send_TimeStart = p.Document_Send_TimeStart,
                        Document_Send_Deadline = p.Document_Send_Deadline,
                        Document_Send_UserSend = p.Document_Send_UserSend,
                        Document_Send_Catagory = p.Document_Send_Catagory,
                        Document_Send_State = p.Document_Send_State,
                        Document_Send_Comment = p.Document_Send_Comment,
                    })
                    .ToList();

            return DocSendList;
        }

        public IEnumerable<Document_SendDTO> GetAllListPrivateUserSend(string user_id)
        {
            var userDocInEntities = _context.Document_Send
                   .Where(up => up.Document_Send_UserSend == user_id && up.Document_Send_Public == false)
                   .OrderByDescending(p => p.Document_Send_Time);
            var userDocDTOs = userDocInEntities
                .Select(p => new Document_SendDTO
                {
                    Document_Send_Id = p.Document_Send_Id,
                    Document_Send_Title = p.Document_Send_Title,
                    Document_Send_Content = p.Document_Send_Content,
                    Document_Send_Time = p.Document_Send_Time,
                    Document_Send_TimeStart = p.Document_Send_TimeStart,
                    Document_Send_Deadline = p.Document_Send_Deadline,
                    Document_Send_UserSend = p.Document_Send_UserSend,
                    Document_Send_Catagory = p.Document_Send_Catagory,
                    Document_Send_State = p.Document_Send_State,
                    Document_Send_Comment = p.Document_Send_Comment,
                })
                .ToList();

            return userDocDTOs;
        }
        
        public (IEnumerable<Document_SendDTO>, int) GetListByUserSendLimitNumberPage(string user_id, int limit, int numberPage)
        {
            var userDocInEntities = _context.Document_Send
                   .Where(up => up.Document_Send_UserSend == user_id && up.Document_Send_Public == true)
                   .OrderByDescending(p => p.Document_Send_Time);
            int totalNumberPage = (userDocInEntities.Count() + limit - 1) / limit;

            int offset = (numberPage - 1) * limit;

            var userDocDTOs = userDocInEntities
                .Skip(offset)
                .Take(limit)
                .Select(p => new Document_SendDTO
                {
                    Document_Send_Id = p.Document_Send_Id,
                    Document_Send_Title = p.Document_Send_Title,
                    Document_Send_Content = p.Document_Send_Content,
                    Document_Send_Time = p.Document_Send_Time,
                    Document_Send_TimeStart = p.Document_Send_TimeStart,
                    Document_Send_Deadline = p.Document_Send_Deadline,
                    Document_Send_UserSend = p.Document_Send_UserSend,
                    Document_Send_Catagory = p.Document_Send_Catagory,
                    Document_Send_State = p.Document_Send_State,
                    Document_Send_Comment = p.Document_Send_Comment,
                })
                .ToList();

            return (userDocDTOs, totalNumberPage);
        }
        public IEnumerable<Document_SendDTO> GetAllListUserReceive(string user_id)
        {
            var userReceive = _context.User_Receive_Document.Where(u => u.User_Id == user_id).ToList();
            var userDocInEntities = new List<Document_Send>();

            bool hasPermissionFour = _context.User_Permission.Any(p => p.User_Id == user_id && p.Permission_Id == 4);
            
            
            foreach (var item in userReceive)
            {
                if (hasPermissionFour)
                {
                    var document = _context.Document_Send.FirstOrDefault(up => up.Document_Send_Id == item.Document_Send_Id);
                    if (document != null)
                    {
                        userDocInEntities.Add(document);
                    }
                }
                else
                {
                    var document = _context.Document_Send.FirstOrDefault(up => up.Document_Send_Id == item.Document_Send_Id && up.Document_Send_State >= 3);
                    if (document != null)
                    {
                        userDocInEntities.Add(document);
                    }
                }
            }
            userDocInEntities = userDocInEntities.OrderByDescending(p => p.Document_Send_Time).ToList();

            var userDocDTOs = userDocInEntities
                .Select(p => new Document_SendDTO
                {
                    Document_Send_Id = p.Document_Send_Id,
                    Document_Send_Title = p.Document_Send_Title,
                    Document_Send_Content = p.Document_Send_Content,
                    Document_Send_Time = p.Document_Send_Time,
                    Document_Send_TimeStart = p.Document_Send_TimeStart,
                    Document_Send_Deadline = p.Document_Send_Deadline,
                    Document_Send_UserSend = p.Document_Send_UserSend,
                    Document_Send_Catagory = p.Document_Send_Catagory,
                    Document_Send_State = p.Document_Send_State,
                    Document_Send_Comment = p.Document_Send_Comment,
                })
                .ToList();

            return userDocDTOs;
        }
        public IEnumerable<Document_SendDTO> GetAllListUserReceiveIsSeen(string user_id)
        {
            var userReceive = _context.User_Receive_Document.Where(u => u.User_Id == user_id && u.Document_Send_IsSeen == false).ToList();
            var userDocInEntities = new List<Document_Send>();

            //bool hasPermissionFour = _context.User_Permission.Any(p => p.User_Id == user_id && p.Permission_Id == 4);


            foreach (var item in userReceive)
            {
                /*if (hasPermissionFour)
                {
                    var document = _context.Document_Send.FirstOrDefault(up => up.Document_Send_Id == item.Document_Send_Id && up.Document_Send_State == 0);
                    if (document != null)
                    {
                        userDocInEntities.Add(document);
                    }
                }
                else
                {*/
                    var document = _context.Document_Send.FirstOrDefault(up => up.Document_Send_Id == item.Document_Send_Id && up.Document_Send_State == 3);
                    if (document != null)
                    {
                        userDocInEntities.Add(document);
                    }
                //}
            }
            var document_Sends = _context.Document_Send.Where(d => d.Document_Send_UserSend == user_id && d.Document_Send_State == 4).ToList();
            foreach(var document in document_Sends)
            {
                userDocInEntities.Add(document);
            }

            userDocInEntities = userDocInEntities.OrderByDescending(p => p.Document_Send_Time).ToList();

            var userDocDTOs = userDocInEntities
                .Select(p => new Document_SendDTO
                {
                    Document_Send_Id = p.Document_Send_Id,
                    Document_Send_Title = p.Document_Send_Title,
                    Document_Send_Content = p.Document_Send_Content,
                    Document_Send_Time = p.Document_Send_Time,
                    Document_Send_TimeStart = p.Document_Send_TimeStart,
                    Document_Send_Deadline = p.Document_Send_Deadline,
                    Document_Send_UserSend = p.Document_Send_UserSend,
                    Document_Send_Catagory = p.Document_Send_Catagory,
                    Document_Send_State = p.Document_Send_State,
                    Document_Send_Comment = p.Document_Send_Comment,
                    Document_Send_TimeUpdate = p.Document_Send_TimeUpdate,
                })
                .ToList();

            return userDocDTOs;
        }
        public int GetNumberNotification(string userId)
        {
            var userReceive = _context.User_Receive_Document.Where(u => u.User_Id == userId && u.Document_Send_IsSeen == false).ToList();
            var userDocInEntities = new List<Document_Send>();

            //bool hasPermissionFour = _context.User_Permission.Any(p => p.User_Id == user_id && p.Permission_Id == 4);

            foreach (var item in userReceive)
            {
                /*if (hasPermissionFour)
                {
                    var document = _context.Document_Send.FirstOrDefault(up => up.Document_Send_Id == item.Document_Send_Id && up.Document_Send_State == 0);
                    if (document != null)
                    {
                        userDocInEntities.Add(document);
                    }
                }
                else
                {*/
                var document = _context.Document_Send.FirstOrDefault(up => up.Document_Send_Id == item.Document_Send_Id && up.Document_Send_State == 3);
                if (document != null)
                {
                    userDocInEntities.Add(document);
                }
                //}
            }
            var document_Sends = _context.Document_Send.Where(d => d.Document_Send_UserSend == userId && d.Document_Send_State == 4).ToList();
            foreach (var document in document_Sends)
            {
                userDocInEntities.Add(document);
            }
            userDocInEntities = userDocInEntities.OrderByDescending(p => p.Document_Send_Time).ToList();

            return userDocInEntities.Count();
        }

        public (IEnumerable<Document_SendDTO>, int) GetListByUserReceiveLimitNumberPage(string user_id, int limit, int numberPage)
        {
            var userReceive = _context.Document_Send_File.Where(u => u.User_Id == user_id).ToList();
            var userDocInEntities = new List<Document_Send>();
            bool hasPermissionFour = _context.User_Permission.Any(p => p.User_Id == user_id && p.Permission_Id == 4);

            foreach (var item in userReceive)
            {
                if (hasPermissionFour)
                {
                    var document = _context.Document_Send.FirstOrDefault(up => up.Document_Send_Id == item.Document_Send_Id);
                    if (document != null)
                    {
                        userDocInEntities.Add(document);
                    }
                }
                else
                {
                    var document = _context.Document_Send.FirstOrDefault(up => up.Document_Send_Id == item.Document_Send_Id && up.Document_Send_State >= 3);
                    if (document != null)
                    {
                        userDocInEntities.Add(document);
                    }
                }
            }
            userDocInEntities = userDocInEntities.OrderByDescending(p => p.Document_Send_Time).ToList();

            int totalNumberPage = (userDocInEntities.Count() + limit - 1) / limit;

            int offset = (numberPage - 1) * limit;

            var userDocDTOs = userDocInEntities
                .Skip(offset)
                .Take(limit)
                .Select(p => new Document_SendDTO
                {
                    Document_Send_Id = p.Document_Send_Id,
                    Document_Send_Title = p.Document_Send_Title,
                    Document_Send_Content = p.Document_Send_Content,
                    Document_Send_Time = p.Document_Send_Time,
                    Document_Send_TimeStart = p.Document_Send_TimeStart,
                    Document_Send_Deadline = p.Document_Send_Deadline,
                    Document_Send_UserSend = p.Document_Send_UserSend,
                    Document_Send_Catagory = p.Document_Send_Catagory,
                    Document_Send_State = p.Document_Send_State,
                    Document_Send_Comment = p.Document_Send_Comment,
                })
                .ToList();

            return (userDocDTOs, totalNumberPage);
        }
        public bool Create(Document_SendDTO p)
        {
            var documentSendEntity = new Domain.Document_Send
            {
                Document_Send_Id = p.Document_Send_Id,
                Document_Send_Title = p.Document_Send_Title,
                Document_Send_Content = p.Document_Send_Content,
                Document_Send_Time = p.Document_Send_Time,
                Document_Send_TimeStart = p.Document_Send_TimeStart,
                Document_Send_Deadline = p.Document_Send_Deadline,
                Document_Send_UserSend = p.Document_Send_UserSend,
                Document_Send_Catagory = p.Document_Send_Catagory,
                Document_Send_State = p.Document_Send_State,
                Document_Send_Public = p.Document_Send_Public,
                
            };
            _context.Document_Send.Add(documentSendEntity);

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
        public bool Update(Document_SendDTO document_SendDTO)
        {
            var documentSendEntity = _context.Document_Send.Find(document_SendDTO.Document_Send_Id);
            if (documentSendEntity == null)
            {
                return false;
            }
            documentSendEntity.Document_Send_Title = document_SendDTO.Document_Send_Title;
            documentSendEntity.Document_Send_Content = document_SendDTO.Document_Send_Content;
            documentSendEntity.Document_Send_Time = document_SendDTO.Document_Send_Time;
            documentSendEntity.Document_Send_TimeStart = document_SendDTO.Document_Send_TimeStart;
            documentSendEntity.Document_Send_Deadline = document_SendDTO.Document_Send_Deadline;
            documentSendEntity.Document_Send_Catagory = document_SendDTO.Document_Send_Catagory;
            documentSendEntity.Document_Send_State = document_SendDTO.Document_Send_State;

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
        public bool CommentEditAndStateByDocId(string docId, int state,string comment)
        {
            var documentSendEntity = _context.Document_Send.Find(docId);
            if (documentSendEntity == null)
            {
                return false;
            }
            documentSendEntity.Document_Send_State = state;
            documentSendEntity.Document_Send_Comment = comment;
            documentSendEntity.Document_Send_TimeUpdate = DateTime.Now;
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
        public bool UpdateState(string doc_id, int state)
        {
            var documentSendEntity = _context.Document_Send.Find(doc_id);
            if (documentSendEntity == null)
                return false;
            documentSendEntity.Document_Send_State = state;
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
        public bool UpdateSeen(string doc_id, string userId)
        {
            var documentSendEntity = _context.User_Receive_Document.Where(d => d.Document_Send_Id == doc_id && d.User_Id == userId).FirstOrDefault();
            if (documentSendEntity == null)
                return false;


            documentSendEntity.Document_Send_IsSeen = true;
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

        public bool Delete(string id)
        {
            var documentSendEntity = _context.Document_Send.Find(id);
            if (documentSendEntity == null)
            {
                return false;
            }
            _context.Document_Send.Remove(documentSendEntity);

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
    }
}
