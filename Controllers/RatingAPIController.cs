using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoStopAPI.Models.SQL;
using System.Collections.Generic;

namespace AutoStopAPI.Controllers
{
    [Route("api/rating")]
    [ApiController]
    public class RatingAPIController : ControllerBase
    {
        private readonly SQLComment _sqlComment;

        public RatingAPIController()
        {
            _sqlComment = new SQLComment();
        }

        [HttpGet]
        public IActionResult GetRating(string phone)
        {
            try
            {
                List<Rating> ratings = _sqlComment.GetRatings(phone);
                if (ratings == null || ratings.Count == 0)
                {
                    return NotFound("No ratings found for the given phone number.");
                }
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult PostRating([FromBody] Rating rating)
        {
            if (rating == null || string.IsNullOrEmpty(rating.phoneGet) || string.IsNullOrEmpty(rating.phoneSend))
            {
                return BadRequest("Invalid rating data.");
            }

            try
            {
                int idComment = _sqlComment.AddRating(rating);
                if (idComment > 0)
                {
                    return Ok(idComment);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Error adding rating.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding rating: {ex.Message}");
            }
        }

        [HttpDelete]
        public IActionResult DeleteRating(int idComment)
        {
            try
            {
                bool isDeleted = _sqlComment.DeleteRating(idComment);
                if (isDeleted)
                {
                    return Ok("Rating deleted successfully.");
                }
                else
                {
                    return NotFound("Rating not found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting rating: {ex.Message}");
            }
        }
    }
}
