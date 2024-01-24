using Microsoft.EntityFrameworkCore;
using ND2Assignwork.API.Data;
using ND2Assignwork.API.Models.DTO;

namespace ND2Assignwork.API.Models.Service.Imp
{
    public class FileService : IFileService
    {

        private readonly DataContext _context;

        public FileService(DataContext context)
        {
            this._context = context;
        }

        public IEnumerable<FileDTO> GetAll()
        {
            return _context.File.Select(p => new FileDTO
            {
                File_Id = p.File_Id,
                File_Name = p.File_Name,
                ContentType = p.ContentType,
            });
        }
        public async Task<IEnumerable<FileDTO>> GetFileByDocInIdAsync(string id)
        {
            var files =await _context.Document_Incomming_File
                .Include(f => f.File)
                .Where(df => df.Document_Incomming_Id == id)
                .ToListAsync();
            var fileDTO = files.Select(f => new FileDTO
            {
                File_Id = f.File_Id,
                File_Name = f.File.File_Name,
                ContentType = f.File.ContentType
            }).ToList();
            return fileDTO;
        }
        public async Task<IEnumerable<FileDTO>> GetFileByDocSendIdAsync(string id)
        {
            var files = await _context.Document_Send_File
                .Include(f => f.File)
                .Where(df => df.Document_Send_Id == id)
                .ToListAsync();
            var fileDTO = files.Select(f => new FileDTO
            {
                File_Id = f.File_Id,
                File_Name = f.File.File_Name,
                ContentType = f.File.ContentType
            }).ToList();
            return fileDTO;
        }
        public async Task<IEnumerable<FileDTO>> GetFileByTaskIdAsync(string id)
        {
            var files = await _context.Task_File
                .Include(f => f.File)
                .Where(df => df.Task_Id == id)
                .ToListAsync();
            var fileDTO = files.Select(f => new FileDTO
            {
                File_Id = f.File_Id,
                File_Name = f.File.File_Name,
                ContentType = f.File.ContentType
            }).ToList();
            return fileDTO;
        }
        public async Task<IEnumerable<FileDTO>> GetFileByDocSendIdComfirmAsync(string id)
        {
            var files = await _context.Document_Send_File
                .Include(f => f.File)
                .Where(df => df.Document_Send_Id == id && df.User_Id != null)
                .ToListAsync();
            var fileDTO = files.Select(f => new FileDTO
            {
                File_Id = f.File_Id,
                File_Name = f.File.File_Name,
                ContentType = f.File.ContentType
            }).ToList();
            return fileDTO;
        }
        public async Task<IEnumerable<FileDTO>> GetFileByTaskIdCofirmAsync(string id)
        {
            var files = await _context.Task_File
                .Include(f => f.File)
                .Where(df => df.Task_Id == id && df.User_Id != null)
                .ToListAsync();
            var fileDTO = files.Select(f => new FileDTO
            {
                File_Id = f.File_Id,
                File_Name = f.File.File_Name,
                ContentType = f.File.ContentType
            }).ToList();
            return fileDTO;
        }
        public IEnumerable<FileDTO> GetFileByDocId(string id)
        {
            var files = _context.Document_Incomming_File
                .Where(df => df.Document_Incomming_Id == id)
                .Select(df => new
                {
                    df.File_Id,
                })
                .Join(_context.File,
                    df => df.File_Id,
                    f => f.File_Id,
                    (df, f) => new FileDTO
                    {
                        File_Id = f.File_Id,
                        File_Name = f.File_Name,
                        ContentType = f.ContentType,
                    })
                .ToList();

            return files;
        }

        public IEnumerable<FileDTO> GetFileByDocSendId(string id)
        {
            var files = _context.Document_Send_File
                .Where(df => df.Document_Send_Id == id && df.User_Id == null)
                .Select(df => new
                {
                    df.File_Id,
                })
                .Join(_context.File,
                    df => df.File_Id,
                    f => f.File_Id,
                    (df, f) => new FileDTO
                    {
                        File_Id = f.File_Id,
                        File_Name = f.File_Name,
                        ContentType = f.ContentType,
                    })
                .ToList();

            return files;
        }
        public IEnumerable<FileDTO> GetFileByDocSendIdComfirm(string id)
        {
            var files = _context.Document_Send_File
                .Where(df => df.Document_Send_Id == id && df.User_Id != null)
                .Select(df => new
                {
                    df.File_Id,
                })
                .Join(_context.File,
                    df => df.File_Id,
                    f => f.File_Id,
                    (df, f) => new FileDTO
                    {
                        File_Id = f.File_Id,
                        File_Name = f.File_Name,
                        ContentType = f.ContentType,
                    })
                .ToList();

            return files;
        }


        public IEnumerable<FileDTO> GetFileByTaskId(string id)
        {
            var files = _context.Task_File
                .Where(df => df.Task_Id == id && df.User_Id == null)
                .Select(df => new
                {
                    df.File_Id,
                    
                })
                .Join(_context.File,
                    df => df.File_Id,
                    f => f.File_Id,
                    (df, f) => new FileDTO
                    {
                        File_Id = f.File_Id,
                        File_Name = f.File_Name,
                        ContentType = f.ContentType,
                        
                    })
                .ToList();

            return files;
        }
        public IEnumerable<FileDTO> GetFileByTaskIdcComfirm(string id)
        {
            var files = _context.Task_File
                .Where(df => df.Task_Id == id && df.User_Id != null)
                .Select(df => new
                {
                    df.File_Id,

                })
                .Join(_context.File,
                    df => df.File_Id,
                    f => f.File_Id,
                    (df, f) => new FileDTO
                    {
                        File_Id = f.File_Id,
                        File_Name = f.File_Name,
                        ContentType = f.ContentType,

                    })
                .ToList();

            return files;
        }

        public FileDTO GetFileDocInById(string id, string userId)
        {
            // Tìm tệp tin dựa trên id được cung cấp
            var fileEntity = _context.File.Find(id);
            if (fileEntity == null)
                return null;

            // Lấy danh sách các Document_Incomming_Id mà người gửi hoặc người nhận liên quan đến userId
            var associatedDocumentIds = _context.Document_Incomming
                .Where(d => d.Document_Incomming_UserSend == userId || d.Document_Incomming_UserReceive == userId)
                .Select(d => d.Document_Incomming_Id)
                .ToList();

            // Kiểm tra xem có liên kết giữa tệp tin và các Document_Incomming_Id mà người dùng có quan hệ không
            var isFileAssociatedWithUser = _context.Document_Incomming_File
                .Any(df => df.File_Id == id && associatedDocumentIds.Contains(df.Document_Incomming_Id));

            if (!isFileAssociatedWithUser)
                return null;

            return new FileDTO
            {
                File_Id = fileEntity.File_Id,
                File_Name = fileEntity.File_Name,
                File_Data = fileEntity.FileData,
                ContentType = fileEntity.ContentType,
            };
        }
        public FileDTO GetFileDocSendById(string id, string userId)
        {
            // Tìm tệp tin dựa trên id được cung cấp
            var fileEntity = _context.File.Find(id);
            if (fileEntity == null)
                return null;

            /*// Lấy danh sách các Document_Incomming_Id mà người gửi hoặc người nhận liên quan đến userId
            var associatedDocumentIds = _context.Document_Send
                .Where(d => d.Document_Send_UserSend == userId)
                .Select(d => d.Document_Send_Id)
                .ToList();
            
            // Kiểm tra xem có liên kết giữa tệp tin và các Document_Incomming_Id mà người dùng có quan hệ không
            var isFileAssociatedWithUser = _context.Document_Send_File
                .Any(df => df.File_Id == id && associatedDocumentIds.Contains(df.Document_Send_Id));

            if (!isFileAssociatedWithUser)
                return null;*/

            return new FileDTO
            {
                File_Id = fileEntity.File_Id,
                File_Name = fileEntity.File_Name,
                File_Data = fileEntity.FileData,
                ContentType = fileEntity.ContentType,
            };
        }
        public FileDTO GetFileTaskById(string id, string userId)
        {
            // Tìm tệp tin dựa trên id được cung cấp
            var fileEntity = _context.File.Find(id);
            if (fileEntity == null)
                return null;

            // Lấy danh sách các Document_Incomming_Id mà người gửi hoặc người nhận liên quan đến userId
            var associatedDocumentIds = _context.Task
                .Where(d => d.Task_Person_Send == userId || d.Task_Person_Receive == userId)
                .Select(d => d.Task_Id)
                .ToList();

            // Kiểm tra xem có liên kết giữa tệp tin và các Document_Incomming_Id mà người dùng có quan hệ không
            var isFileAssociatedWithUser = _context.Task_File
                .Any(df => df.File_Id == id && associatedDocumentIds.Contains(df.Task_Id));

            if (!isFileAssociatedWithUser)
                return null;

            return new FileDTO
            {
                File_Id = fileEntity.File_Id,
                File_Name = fileEntity.File_Name,
                File_Data = fileEntity.FileData,
                ContentType = fileEntity.ContentType,
            };
        }

        public bool Create(FileDTO fileDTO)
        {

            var fileEntity = new Models.Domain.File
            {
                File_Id = fileDTO.File_Id,
                File_Name = fileDTO.File_Name,
                FileData = fileDTO.File_Data,
                ContentType = fileDTO.ContentType,
            };
            _context.File.Add(fileEntity);

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
        public async Task<bool> CreateAsync(FileDTO fileDTO)
        {
            var fileEntity = new Models.Domain.File
            {
                File_Id = fileDTO.File_Id,
                File_Name = fileDTO.File_Name,
                FileData = fileDTO.File_Data,
                ContentType = fileDTO.ContentType,
            };
            await _context.File.AddAsync(fileEntity);

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

        public bool Update(FileDTO fileDTO)
        {
            var fileEntity = _context.File.Find(fileDTO.File_Id);
            fileEntity.File_Name = fileDTO.File_Name;
            fileEntity.FileData = fileDTO.File_Data;
            fileEntity.ContentType = fileDTO.ContentType;

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
            try
            {
                var fileEntity = _context.File.Find(id);
                _context.File.Remove(fileEntity);

                int recordsAffected = _context.SaveChanges();
                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public async Task<List<Domain.File>> FileNotConect()
        {
            var filesWithoutForeignKey =await _context.File
                .Where(f => !f.Document_Incomming_File.Any(d => d.File_Id == f.File_Id) 
                            && !f.Task_File.Any(t => t.File_Id == f.File_Id)
                            && !f.Document_Send_File.Any(s => s.File_Id == f.File_Id))
                .ToListAsync();
            return filesWithoutForeignKey;
        }
        public bool DeleteListFileNotConnect(List<Domain.File> fileIds)
        {
            foreach (var id in fileIds)
            {
                var fileEntity = _context.File.Find(id.File_Id);
                _context.File.Remove(fileEntity);
            }
            int recordsAffected = _context.SaveChanges();
            return recordsAffected > 0;
        }


        public async Task<FileDTO> GetFileById(string id)
        {
            var fileEntity = await _context.File.FindAsync(id);
            if (fileEntity == null)
                return null;

            return new FileDTO
            {
                File_Id = fileEntity.File_Id,
                File_Name = fileEntity.File_Name,
                File_Data = fileEntity.FileData,
                ContentType = fileEntity.ContentType,
            };
        }
    }
}
