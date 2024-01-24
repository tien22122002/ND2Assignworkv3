using Microsoft.EntityFrameworkCore;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class DocumentIncommingFileService : IDocumentIncommingFileService
    {
        private readonly DataContext _context;
        public DocumentIncommingFileService(DataContext context)
        {
            this._context = context;
        }


        public IEnumerable<Document_Incomming_FileDTO> GetAllDocumentFile()
        {
            return _context.Document_Incomming_File.Select(u => new Document_Incomming_FileDTO
            {
                File_Id = u.File_Id,
                Document_Incomming_Id = u.Document_Incomming_Id,
            }).ToList();
        }
        public IEnumerable<Document_Incomming_FileDTO> GetAllDocByDocId(string doc_id)
        {
            return _context.Document_Incomming_File
                .Where(dif => dif.Document_Incomming_Id == doc_id)
                .Select(d => new Document_Incomming_FileDTO
                {
                    File_Id = d.File_Id,
                    Document_Incomming_Id = d.Document_Incomming_Id,
                })
                .ToList();
        }
        public Document_Incomming_FileDTO GetOneDocFile(string doc_id, string file_id)
        {
            var documentIncommingFileEntity = _context.Document_Incomming_File.Find(file_id, doc_id);
            if (documentIncommingFileEntity == null)
            {
                return null;
            }

            return new Document_Incomming_FileDTO
            {
                File_Id = documentIncommingFileEntity.File_Id,
                Document_Incomming_Id = documentIncommingFileEntity.Document_Incomming_Id,
            };
        }
        public async Task<bool> CreateDocFile(Document_Incomming_FileDTO document_Incomming_FileDTO)
        {
            var documentIncommingFileEntity = new Document_Incomming_File
            {
                File_Id = document_Incomming_FileDTO.File_Id,
                Document_Incomming_Id = document_Incomming_FileDTO.Document_Incomming_Id,
            };

            await _context.Document_Incomming_File.AddAsync(documentIncommingFileEntity);
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
        public bool DeleteDocFile(string doc_id, string file_id)
        {
            var documentIncommingFileEntity = _context.Document_Incomming_File.Find(file_id, doc_id);
            if (documentIncommingFileEntity == null)
            {
                throw new ArgumentException("Document Incomming File not found");
            }

            _context.Document_Incomming_File.Remove(documentIncommingFileEntity);

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
        public async Task<bool> DeleteDocFilesByDocId(string doc_id)
        {
            var documentIncommingEntity =await _context.Document_Incomming.Include(d => d.Document_Incomming_File).FirstOrDefaultAsync(d => d.Document_Incomming_Id == doc_id);
            if (documentIncommingEntity == null || documentIncommingEntity.Document_Incomming_State >= 3)
            {
                return false; // Không thể xóa do trạng thái >= 3
            }

            try
            {
                // Xóa các bản ghi từ bảng Document_Incomming_File
                _context.Document_Incomming_File.RemoveRange(documentIncommingEntity.Document_Incomming_File);
                int recordsAffected = _context.SaveChanges();

                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }

        public bool DeleteDocFilesByFileId(string file_id)
        {
            var documentIncommingFileEntities = _context.Document_Incomming_File.Where(dif => dif.File_Id == file_id).ToList();

            if (documentIncommingFileEntities.Count == 0)
            {
                return false; // Không có liên kết nào cho file_id này
            }

            _context.Document_Incomming_File.RemoveRange(documentIncommingFileEntities);

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


        public async Task<IEnumerable<Document_Incomming_FileDTO>> GetDocFileByDocId(string doc_id)
        {
            var documentIncommingFileEntity =await _context.Document_Incomming_File
                .Where(up => up.Document_Incomming_Id == doc_id)
                .ToListAsync();

            if (documentIncommingFileEntity == null || documentIncommingFileEntity.Count == 0)
            {
                return null;
            }

            var documentIncommingFileDTO = documentIncommingFileEntity
                .Select(up => new Document_Incomming_FileDTO
                {
                    File_Id = up.File_Id,
                    Document_Incomming_Id = up.Document_Incomming_Id,
                });

            return documentIncommingFileDTO;
        }
        public bool isExistDocIn(string doc_id)
        {
            bool exists = _context.Document_Incomming_File.Any(d => d.Document_Incomming_Id == doc_id);
            return exists;

        }
        public bool isExistFile(string file_id)
        {
            bool exists = _context.Document_Incomming_File.Any(d => d.File_Id == file_id);
            return exists;
        }
    }
}
