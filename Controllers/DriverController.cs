using AutoStopAPI.Models.SQL;
using AutoStopAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AutoStopAPI.Controllers
{
    [ApiController]
    [Route("api/driver")]
    public class DriverController : ControllerBase
    {
        //Проверяем является ли User водителем
        [HttpGet]
        public IActionResult GetDriver([FromQuery] string phone)
        {
            SQLDriver sqlDriver = new SQLDriver();
            var driver = sqlDriver.SerchDrive(phone);

            if (driver == null)
            {
                // Возвращаем 404, если запись не найдена
                return NotFound(new { message = "Driver not found" });
            }

            return Ok(driver);
        }

        [HttpPost]
        public IActionResult AddDriver([FromBody] Driver driver)
        {
            SQLDriver sqlDriver = new SQLDriver();
            bool result = sqlDriver.AddDriver(driver);
            if (result == false)
            {
                // Возвращаем 404, если запись не найдена
                return NotFound(new { message = "Driver not found" });
            }

            return Ok(driver);
        }
    }
}
