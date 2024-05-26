using AutoStopAPI.Models;
using AutoStopAPI.Models.SQL;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AutoStopAPI.Controllers
{
    [ApiController]
    [Route("api/car")]
    public class CarController : Controller
    {
        [HttpGet]
        public IActionResult GetCar([FromQuery] string phone)
        {
            SQLCar sqlCar = new SQLCar();
            List<Car> cars = sqlCar.GetCarsByUser(phone);
            if (cars.Count == 0)
            {
                return NotFound(new { message = "No cars found for this user" });
            }

            return Ok(cars);
        }

        [HttpPost]
        public IActionResult PostCar([FromBody] Car car)
        {
            SQLCar sqlCar = new SQLCar();
            bool result = sqlCar.AddCar(car);
            if (result == false)
            {
                // Возвращаем 400, если добавление не удалось
                return BadRequest(new { message = "Failed to add car" });
            }

            return Ok(car);
        }

        [HttpDelete]
        public IActionResult DeleteCar([FromBody] Car car)
        {
            SQLCar sqlCar = new SQLCar();
            bool result = sqlCar.DeleteCar(car.PhoneUser, car.GRZ);
            if (result == false)
            {
                // Возвращаем 404, если удаление не удалось
                return NotFound(new { message = "Car not found or failed to delete" });
            }

            return Ok(new { message = "Car successfully deleted" });
        }

        [HttpPatch]
        public IActionResult PatchCar([FromBody] Car car)
        {
            SQLCar sqlCar = new SQLCar();
            bool result = sqlCar.UpdateCar(car);
            if (result == false)
            {
                // Возвращаем 400, если обновление не удалось
                return BadRequest(new { message = "Failed to update car" });
            }

            return Ok(new { message = "Car update" });
        }
    }
}
