using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using AutoStopAPI.Models.SQL;

namespace AutoStopAPI.Controllers
{
    [Route("api/img")]
    [ApiController]
    public class ImgAPIController : ControllerBase
    {
        private readonly SQLImg _sqlImg;

        public ImgAPIController()
        {
            _sqlImg = new SQLImg();
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateImage(string phone, IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return BadRequest("No image file provided.");
            }

            byte[] imageData;
            using (var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                imageData = ms.ToArray();
            }

            bool updateSuccess = _sqlImg.UpdateUserImage(phone, imageData);
            if (updateSuccess)
            {
                return Ok("Image updated successfully.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating image.");
            }
        }

        [HttpGet("{phone}")]
        public IActionResult GetImage(string phone)
        {
            var imageData = _sqlImg.GetUserImage(phone);
            if (imageData != null)
            {
                return File(imageData, "image/jpeg"); // Assuming the image format is jpeg
            }
            else
            {
                return NotFound("Image not found.");
            }
        }
    }
}
