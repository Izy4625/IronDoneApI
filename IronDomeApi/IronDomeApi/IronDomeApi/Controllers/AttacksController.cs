using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IronDomeApi.Controllers;
using IronDomeApi.Services;
using IronDomeApi.Models;
namespace IronDomeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttacksController : ControllerBase
    {

        
            [HttpGet]
            public IActionResult GetAttacks()
            {
                return StatusCode(
                    StatusCodes.Status200OK,
                    new
                    {
                        success = true,
                        attacks = DbService.AttackList.ToArray()
                    }
                    );
            }


            [HttpPost]
            [Produces("application/json")]
            [ProducesResponseType(StatusCodes.Status201Created)]
            public IActionResult CreateAttack(Attack attack)
            {
                attack.Id = Guid.NewGuid();
                attack.Status = "Pending";
                DbService.AttackList.Add(attack);
                return StatusCode(
                    StatusCodes.Status201Created,
                    new { success = true, attack = attack }
                    );
                    }
        [HttpPost("{Id}")]
        [Produces("application/json")]
        
        public IActionResult StartAttack(Guid Id)
        {
            {
                Attack attack = DbService.AttackList.FirstOrDefault(att => att.Id == Id);
                
               
                if (attack.Status == "Completed")
                {
                    return StatusCode(
                        400,
                        new
                        {
                            error = "Cannot start an attack that has already been completed."
                        }
                    );
                }
                Task attackTask = Task.Run(() =>
                {
                    Task.Delay(10000);
                    attack.Status = "In Progress";
                    attack.launched = DateTime.Now;
                });
                return StatusCode(
                    StatusCodes.Status200OK,
                    new { message = "Attack Started.", TaskId = attackTask.Id }
                );

            }
        }
        [HttpGet("{Id}")]
        [Produces("application/json")]
        public IActionResult GetAttackStatus(Guid Id)
        {
            Attack attack = DbService.AttackList.FirstOrDefault(att => att.Id == Id);
            string _status = attack.Status;
            switch (_status)
            {
                case "Pending":
                case "Completed":  
                case "In Progress":
                    return StatusCode(
                StatusCodes.Status200OK,
                new {Id = attack.Id , Status = _status ,StartedAt = attack.launched});
                    default:
                    return StatusCode(
             StatusCodes.Status200OK,
             new { Status = "error fetching the status" });
                   


            }
        }
    }
}
