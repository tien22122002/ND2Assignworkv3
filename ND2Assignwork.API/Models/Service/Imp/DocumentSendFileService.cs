using ND2Assignwork.API.Controllers;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.Domain;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class DocumentSendFileService : IDocumentSendFileService
    {
        private readonly DataContext _context;
        public DocumentSendFileService(DataContext context)
        {
            this._context = context;
        }

        public IEnumerable<Document_Send_FileDTO> GetAllDocumentSendFile()
        {
            return _context.Document_Send_File.Select(u => new Document_Send_FileDTO
            {
                File_Id = u.File_Id,
                Document_Send_Id = u.Document_Send_Id,
            }).ToList();
        }
        public Document_Send_FileDTO GetOneDocSendFile(string doc_id, string file_id)
        {
            var documentSendFileEntity = _context.Document_Send_File.Find(file_id, doc_id);
            if (documentSendFileEntity == null)
            {
                return null;
            }

            return new Document_Send_FileDTO
            {
                File_Id = documentSendFileEntity.File_Id,
                Document_Send_Id = documentSendFileEntity.Document_Send_Id,
            };
        }
        public bool CreateDocSendFile(Document_Send_FileDTO document_Send_FileDTO)
        {
            var documentSendFileEntity = new Document_Send_File
            {
                File_Id = document_Send_FileDTO.File_Id,
                Document_Send_Id = document_Send_FileDTO.Document_Send_Id,
            };

            _context.Document_Send_File.Add(documentSendFileEntity);
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
        public bool DeleteDocSendFile(string doc_id, string file_id)
        {
            var documentSendFileEntity = _context.Document_Send_File.Find(file_id, doc_id);
            if (documentSendFileEntity == null)
            {
                throw new ArgumentException("Document Send File not found");
            }

            _context.Document_Send_File.Remove(documentSendFileEntity);

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
        public bool DeleteDocSendFilesByDocId(string doc_id)
        {
            var documentSendFileEntities = _context.Document_Send_File.Where(dsf => dsf.Document_Send_Id == doc_id).ToList();

            if (documentSendFileEntities.Count == 0)
            {
                return false; // Không có liên kết nào cho doc_id này
            }

            _context.Document_Send_File.RemoveRange(documentSendFileEntities);

            try
            {
                int recordsAffected = _context.SaveChanges();
                if (recordsAffected > 0)
                {
                    foreach (var docFileEntity in documentSendFileEntities)
                    {
                        var fileEntity = _context.File.Find(docFileEntity.File_Id);

                        if (fileEntity != null)
                        {
                            var countFileIdOccurrences = _context.Document_Send_File
                                .Count(dif => dif.File_Id == docFileEntity.File_Id);

                            var isLinkedToOtherTables = false;

                            // Kiểm tra liên kết trong các bảng khác
                            var linkedTable1 = _context.Document_Incomming_File.FirstOrDefault(t1 => t1.File_Id == fileEntity.File_Id);
                            var linkedTable2 = _context.Task_File.FirstOrDefault(t2 => t2.File_Id == fileEntity.File_Id);

                            if (linkedTable1 != null || linkedTable2 != null)
                            {
                                isLinkedToOtherTables = true;
                            }

                            if (!isLinkedToOtherTables && countFileIdOccurrences == 1)
                            {
                                _context.File.Remove(fileEntity);
                            }
                        }
                    }

                    return _context.SaveChanges() > 0;

                }
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi lưu dữ liệu: " + ex.Message);
                return false;
            }
        }
        public bool DeleteDocSendFilesByFileId(string file_id)
        {
            var documentSendFileEntities = _context.Document_Send_File.Where(dsf => dsf.File_Id == file_id).ToList();

            if (documentSendFileEntities.Count == 0)
            {
                return false; // Không có liên kết nào cho file_id này
            }

            _context.Document_Send_File.RemoveRange(documentSendFileEntities);

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

        public IEnumerable<Document_Send_FileDTO> GetDocSendFileByDocId(string doc_id)
        {
            var documentSendFileEntity = _context.Document_Send_File
                .Where(up => up.Document_Send_Id == doc_id)
                .ToList();

            if (documentSendFileEntity == null || documentSendFileEntity.Count == 0)
            {
                return null;
            }

            var documentSendFileDTO = documentSendFileEntity
                .Select(up => new Document_Send_FileDTO
                {
                    File_Id = up.File_Id,
                    Document_Send_Id = up.Document_Send_Id,
                });

            return documentSendFileDTO;
        }
        public bool isExistDocSend(string doc_id)
        {

            bool exists = _context.Document_Send_File.Any(d => d.Document_Send_Id == doc_id);
            return exists;

        }
        public bool isExistFile(string file_id)
        {
            bool exists = _context.Document_Send_File.Any(d => d.File_Id == file_id);
            return exists;
        }
    }
}
