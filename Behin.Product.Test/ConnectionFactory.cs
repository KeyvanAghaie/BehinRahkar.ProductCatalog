using DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Behin.Product.Test
{
    public class ConnectionFactory : IDisposable
    {
        private bool disposedValue = false;

        public ApplicationDbContext CreateContextForInMemory()
        {
            var option = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DAB_TestDataBase").Options;

            var context = new ApplicationDbContext(option);
            if (context != null)
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
            return context;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
                disposedValue = true;
        }

    }
}
