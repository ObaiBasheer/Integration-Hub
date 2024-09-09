using Integration_Hub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Integration_Hub.Controllers
{
    public class IntegrationController : Controller
    {
        private readonly AppDbContext _context;
        public IntegrationController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Integration/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var integrations = await _context.Integrations
                .Select(i => new IntegrationViewModel
                {
                    Id = i.Id,
                    Name = i.Name,
                    Type = i.Source.Type.ToString() + " to " + i.Destination.Type.ToString(),
                    IsActive = i.IsActive,
                    LastRunTime = i.LastRunTime
                })
                .ToListAsync();

            return View(integrations);
        }
    }
}
}
