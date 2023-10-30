using Microsoft.EntityFrameworkCore;

namespace Otus.Teaching.Concurrency.Queue.Access
{
    public class DataContext
        : DbContext
    {
        public DbSet<MessageQueueItem> MessageQueue { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=OtusTeachingConcurrency.sqlite");
        }
    }
}