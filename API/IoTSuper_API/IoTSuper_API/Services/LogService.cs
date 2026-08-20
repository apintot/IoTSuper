using IoTSuper_API.Data;
using IoTSuper_API.Models;
using IoTSuper_API.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace IoTSuper_API.Services
{
    public class LogService : ILogService
    {
        private readonly AppDBContext _context;

        public LogService(AppDBContext context)
        {
            _context = context;
        }

        public async Task LogAsync(string info)
        {
            Log log = new Log
            {
                Info = info,
                CreatedAt = DateTime.Now
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
