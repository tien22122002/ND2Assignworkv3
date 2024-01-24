using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ND2Assignwork.API.Models.Domain;

namespace ND2Assignwork.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions dbContextOptions) : base(dbContextOptions) 
        {
        }
        public DbSet<Department> Department { get; set; }
        public DbSet<Discuss> Discuss { get; set; }
        public DbSet<Document_Incomming> Document_Incomming { get; set; }
        public DbSet<Document_Incomming_File> Document_Incomming_File { get; set; }

        public DbSet<Document_Send> Document_Send { get; set; }
        public DbSet<Document_Send_File> Document_Send_File { get; set; }
        public DbSet<Models.Domain.File> File { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Models.Domain.Task> Task { get; set; }
        public DbSet<Task_Category> Task_Category { get; set; }
        public DbSet<Task_File> Task_File { get; set; }
        public DbSet<User_Account> User_Account { get; set; }
        public DbSet<User_Permission> User_Permission { get; set; }
        public DbSet<User_Receive_Document> User_Receive_Document { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ 1:N giữa User and Position
            modelBuilder.Entity<User_Account>()
                .HasOne(u => u.Position)
                .WithMany(p => p.Users)
                .HasForeignKey(u => u.User_Position)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình quan hệ 1:N giữa Department và User_Account thông qua User_Department
            modelBuilder.Entity<User_Account>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.User_Department)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.UserAccount)
                .WithOne(u => u.DepartmentOne)
                .HasForeignKey<Department>(d => d.Department_Head)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false); // Cho phép Department_Head có thể null

            // Cấu hình quan hệ 1:N giữa  User_Account và DocumentSend thông qua Document_Send_UserSend
            modelBuilder.Entity<Document_Send>()
                .HasOne(u => u.UserAccount)
                .WithMany(d => d.Document_Send)
                .HasForeignKey(u => u.Document_Send_UserSend)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Document_Send>()
                .HasOne(u => u.Category)
                .WithMany(d => d.Document_Send)
                .HasForeignKey(u => u.Document_Send_Catagory)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình quan hệ 1:N giữa User_Account và Document_Incomming thông qua Document_Incomming_UserSend
            modelBuilder.Entity<Document_Incomming>()
                .HasOne(t => t.Sender)
                .WithMany(u => u.DocumentIncommingSend)
                .HasForeignKey(t => t.Document_Incomming_UserSend)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Document_Incomming>()
                .HasOne(t => t.Receiver)
                .WithMany(u => u.DocumentIncommingReceived)
                .HasForeignKey(t => t.Document_Incomming_UserReceive)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Document_Incomming>()
                .HasOne(d => d.ForwardDocument)
                .WithMany()
                .HasForeignKey(d => d.Document_Incomming_Id_Forward)
                .OnDelete(DeleteBehavior.Restrict);



            // Cấu hình quan hệ 1:N giữa User_Account và Task thông qua Task_Person_Send
            modelBuilder.Entity<Models.Domain.Task>()
                .HasOne(t => t.Sender)
                .WithMany(u => u.TasksSend)
                .HasForeignKey(t => t.Task_Person_Send)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Models.Domain.Task>()
                .HasOne(t => t.Receiver)
                .WithMany(u => u.TasksReceive)
                .HasForeignKey(t => t.Task_Person_Receive)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Task_File>()
               .HasOne(u => u.Account)
               .WithMany(d => d.Task_File)
               .HasForeignKey(u => u.User_Id)
               .OnDelete(DeleteBehavior.NoAction)
               .IsRequired(false); // Cho phép User_Id có thể null

            // Cấu hình quan hệ 1:N giữa Task_Category và Task thông qua field Task_Category
            modelBuilder.Entity<Models.Domain.Task>()
                .HasOne(t => t.Category)
                .WithMany(u => u.LstTaskCategory)
                .HasForeignKey(t => t.Task_Category)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình quan hệ 1:N giữa Document_Incomming và Task thông qua field Document_Incomming_Id
            modelBuilder.Entity<Models.Domain.Task>()
                .HasOne(t => t.Document_Send)
                .WithMany(u => u.ListTask)
                .HasForeignKey(t => t.Document_Send_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Discuss>()
                .HasOne(d => d.Task)
                .WithMany(t => t.Discusses)
                .HasForeignKey(d => d.Discuss_Task)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Discuss>()
                .HasOne(d => d.UserAccount)
                .WithMany(u => u.Discusses)
                .HasForeignKey(d => d.Discuss_User)
                .OnDelete(DeleteBehavior.NoAction);

            // Cấu hình quan hệ N:N giữa User_Account và Permission thông qua User_Permission
            modelBuilder.Entity<User_Permission>()
                .HasOne(up => up.UserAccount)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.User_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User_Permission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.Permission_Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User_Receive_Document>()
               .HasOne(urd => urd.User_Account)
               .WithMany(ua => ua.ReceivedDocuments)
               .HasForeignKey(urd => urd.User_Id)
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User_Receive_Document>()
                .HasOne(urd => urd.Document_Send)
                .WithMany(ds => ds.ReceivedByUsers)
                .HasForeignKey(urd => urd.Document_Send_Id)
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<User_Receive_Document>()
                .HasOne(urd => urd.Department)
                .WithMany(ds => ds.User_Receive_Document)
                .HasForeignKey(urd => urd.Department_Id)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired(false); // Cho phép Department_ID có thể null


            


            modelBuilder.Entity<User_Receive_Document>()
                .HasKey(up => new { up.User_Id, up.Document_Send_Id });

            modelBuilder.Entity<User_Permission>()
                .HasKey(up => new { up.User_Id, up.Permission_Id });

            modelBuilder.Entity<Discuss>()
                .HasKey(d => new { d.Discuss_Task, d.Discuss_User, d.Discuss_Time });

            modelBuilder.Entity<Document_Incomming_File>()
                .HasKey(df => new { df.File_Id, df.Document_Incomming_Id });


            modelBuilder.Entity<Document_Send_File>()
                .HasKey(df => new { df.File_Id, df.Document_Send_Id });

            modelBuilder.Entity<Task_File>()
                .HasKey(df => new { df.File_Id, df.Task_Id });



        }


    }
}
