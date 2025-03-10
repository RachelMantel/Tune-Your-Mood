//using Microsoft.AspNetCore.Mvc;
//using TuneYourMood.Core.Entities;
//using TuneYourMood.Core.InterfaceRepository;

//// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace TuneYourMood.Api.Controllers
//{
//    using Microsoft.AspNetCore.Mvc;
//    using System;
//    using System.IO;
//    using System.Threading.Tasks;
//    using Microsoft.AspNetCore.Http;
//    using Microsoft.AspNetCore.Hosting;
//    using TuneYourMood.Core.Entities;
//    using TuneYourMood.Core.InterfaceRepository;

//    [ApiController]
//    [Route("api/files")]
//    public class FileController : ControllerBase
//    {
//        private readonly ISongRepository _songRepository;
//        private readonly IWebHostEnvironment _env;

//        public FileController(ISongRepository songRepository, IWebHostEnvironment env)
//        {
//            _songRepository = songRepository;
//            _env = env;
//        }

//        // 1. העלאת קובץ
//        [HttpPost("upload")]
//        [Consumes("multipart/form-data")]
//        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, [FromForm] string songName, [FromForm] string artist, [FromForm] int userId)
//        {
//            if (file == null || file.Length == 0)
//                return BadRequest("No file uploaded.");

//            // שימוש ב-ContentRootPath אם WebRootPath ריק
//            string uploadsFolder = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot", "uploads");

//            if (!Directory.Exists(uploadsFolder))
//                Directory.CreateDirectory(uploadsFolder);

//            // יצירת שם ייחודי לקובץ
//            string uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
//            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

//            using (var stream = new FileStream(filePath, FileMode.Create))
//            {
//                await file.CopyToAsync(stream);
//            }

//            var song = new SongEntity
//            {
//                SongName = songName,
//                Artist = artist,
//                FilePath = $"/uploads/{uniqueFileName}",  // שמירת הנתיב היחסי
//                UserId = userId,
//                DateAdding = DateTime.UtcNow
//            };

//            await _songRepository.UploadSongAsync(song);
//            return Ok(song);
//        }

//        // 2. הורדת קובץ
//        [HttpGet("download/{songId}")]
//        public async Task<IActionResult> DownloadFile(int songId)
//        {
//            var filePath = await _songRepository.GetSongUrlAsync(songId);
//            if (string.IsNullOrEmpty(filePath))
//                return NotFound("Song not found.");

//            // חיבור נכון של הנתיב המלא
//            string fullPath = Path.Combine(_env.WebRootPath ?? _env.ContentRootPath, "wwwroot", filePath.TrimStart('/'));

//            if (!System.IO.File.Exists(fullPath))
//                return NotFound("File not found.");

//            var fileBytes = await System.IO.File.ReadAllBytesAsync(fullPath);
//            return File(fileBytes, "application/octet-stream", Path.GetFileName(fullPath));
//        }
//    }


//}
