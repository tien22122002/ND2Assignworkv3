using Microsoft.EntityFrameworkCore;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;
using System.Collections.Generic;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class UserReceiceDocumentService : IUserReceiveDocumentService
    {
        private readonly DataContext _context;

        public UserReceiceDocumentService(DataContext context)
        {
            this._context = context;
        }
        public IEnumerable<User_Receive_DocumentDTO> GetAllUserReceice()
        {
            return _context.User_Receive_Document.Select(u => new User_Receive_DocumentDTO
            {

                User_Id = u.User_Id,
                Document_Send_Id = u.Document_Send_Id,
                Department_Id = u.Department_Id,
            }).ToList();
        }
        public User_Receive_DocumentDTO GetOneUserReceice(string user_id, string doc_id)
        {
            var userReceiveEntities = _context.User_Receive_Document.Find(user_id, doc_id);
            if (userReceiveEntities == null)
            {
                return null;
            }

            return new User_Receive_DocumentDTO
            {
                User_Id = userReceiveEntities.User_Id,
                Document_Send_Id = userReceiveEntities.Document_Send_Id,
                Department_Id = userReceiveEntities.Department_Id
            };
        }
        public bool CreateUserReceice(User_Receive_DocumentDTO user_Receive_DocumentDTO)
        {
            var userReceiveEntities = new User_Receive_Document
            {
                User_Id = user_Receive_DocumentDTO.User_Id,
                Document_Send_Id = user_Receive_DocumentDTO.Document_Send_Id,
                Department_Id = user_Receive_DocumentDTO.Department_Id,
                Document_Send_IsSeen = false,
            };

            _context.User_Receive_Document.Add(userReceiveEntities);
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
        public bool DeleteUserReceice(string user_id, string doc_id)
        {
            var userReceiveEntities = _context.User_Receive_Document.Find(user_id, doc_id);
            if (userReceiveEntities == null)
            {
                throw new ArgumentException("User Receice Document not found");
            }

            _context.User_Receive_Document.Remove(userReceiveEntities);

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
        public bool DeleteUserReceivesByDocId(string doc_id)
        {
            var userReceiveEntities = _context.User_Receive_Document.Where(urd => urd.Document_Send_Id == doc_id).ToList();

            if (userReceiveEntities.Count == 0)
            {
                return false; // Không có liên kết nào cho doc_id này
            }

            _context.User_Receive_Document.RemoveRange(userReceiveEntities);

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

        public IEnumerable<User_Receive_DocumentDTO> GetUserReceiceByUserId(string user_id)
        {
            var userReceiveEntities = _context.User_Receive_Document
                .Where(up => up.User_Id == user_id)
                .ToList();

            if (userReceiveEntities == null || userReceiveEntities.Count == 0)
            {
                return null;
            }

            var userReceiveDTO = userReceiveEntities
                .Select(up => new User_Receive_DocumentDTO
                {
                    User_Id = up.User_Id,
                    Document_Send_Id = up.Document_Send_Id,
                    Department_Id = up.Department_Id
                });

            return userReceiveDTO;
        }
        public IEnumerable<User_Receive_DocumentDTO> GetUserReceiceByDocId(string doc_id)
        {
            var userReceiveEntities = _context.User_Receive_Document
                .Where(up => up.Document_Send_Id == doc_id)
                .ToList();

            if (userReceiveEntities == null || userReceiveEntities.Count == 0)
            {
                return null;
            }

            var userReceiveDTO = userReceiveEntities
                .Select(up => new User_Receive_DocumentDTO
                {
                    User_Id = up.User_Id,
                    Document_Send_Id = up.Document_Send_Id,
                    Department_Id = up.Department_Id,
                }).ToList();

            return userReceiveDTO;
        }
        public bool isExistDocSend(string doc_id)
        {

            bool exists = _context.User_Receive_Document.Any(d => d.Document_Send_Id == doc_id);
            return exists;

        }
        public bool isExistUser(string user_id)
        {
            bool exists = _context.User_Receive_Document.Any(d => d.User_Id == user_id);
            return exists;
        }
    }
}
