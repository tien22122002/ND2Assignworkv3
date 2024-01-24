
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ND2Assignwork.API.Models.DTO;
using ND2Assignwork.API.Models.DTO.DTO_Identity;
using ND2Assignwork.API.Models.Service;

namespace ND2Assignwork.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class PositionController : ControllerBase
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            this._positionService = positionService;
        }

        // GET: https://localhost:7147/api/positions
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_positionService.GetAllPositions());
        }

        // GET: https://localhost:7147/api/positions/5
        [HttpGet("{id:int}")]
        public IActionResult GetById([FromRoute] int id)
        {
            var positionDTO = _positionService.GetPositionById(id);
            if (positionDTO == null)
            {
                return NotFound();
            }
            return Ok(positionDTO);
        }

        // POST: https://localhost:7147/api/positions
        [HttpPost, Authorize(Roles ="SuperAdmin")]
        public IActionResult CreatePosition([FromBody] PositionDTO_Identity positionDTO_Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int id_new = _positionService.CreatePosition(positionDTO_Id);
            if (id_new == 0)
            {
                return BadRequest("Lỗi khi thêm position !");
            }
            else
            {
                var positionDTO = _positionService.GetPositionById(id_new);
                return CreatedAtAction("GetById", new { id = id_new }, positionDTO);
            }
            
        }

        // PUT: https://localhost:7147/api/positions/5
        [HttpPut("{id:int}"), Authorize(Roles = "SuperAdmin")]
        public IActionResult UpdatePosition([FromRoute] int id, [FromBody] PositionDTO positionDTO)
        {
            if (id != positionDTO.Position_Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_positionService.UpdatePosition(positionDTO))
            {
                return Ok(positionDTO);
            }
            else
            {
                return BadRequest("Lỗi khi cập nhật Position !");
            }
        }

        // DELETE: https://localhost:7147/api/positions/5
        [HttpDelete("{id:int}"), Authorize(Roles = "SuperAdmin")]
        public IActionResult DeletePosition([FromRoute] int id)
        {
            if (_positionService.DeletePosition(id))
            {
                return Ok("Xóa position thành công !");
            }
            return BadRequest("Lỗi khi xóa position !");
        }
    }
}
