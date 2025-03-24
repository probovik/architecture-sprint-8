using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] // Требуем аутентификации
    public class ReportsController : ControllerBase
    {
        // GET: api/reports
        [HttpGet]
        public ActionResult GetReports()
        {
            if (!CheckUserRole(User))
            {
                return Forbid(); // 403 Forbidden, если у пользователя нет роли
            }
            var file = System.IO.File.ReadAllBytes("1.png");
            HttpContext.Response.Headers.ContentDisposition = "inline;filename=1.png";
            return File(file, "application/pdf", "1.png");
        }

        public bool CheckUserRole(ClaimsPrincipal user)
        {
            // Ищем утверждение с realm_access
            var realmAccessClaim = user.Claims.FirstOrDefault(c => c.Type == "realm_access");
            if (realmAccessClaim == null)
            {
                Console.WriteLine("realm_access claim not found");
                return false;
            }
            return realmAccessClaim.Value.Contains("prothetic_user");
        }
    }
}