using System;
using System.Linq;
using ListifyApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ListifyApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ListifyController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll([FromQuery] ListifyFilterModel filter)
        {
            if(!ValidateRequest(null, filter))
                return BadRequest();

            try
            {
                var minValue = filter.MinValue ?? 0;
                var maxValue = filter.MaxValue ?? minValue + 100;

                var list = new Listify(minValue, maxValue);

                return Ok(list.AsEnumerable());
            }
            catch (IndexOutOfRangeException iorException)
            {
                return BadRequest(iorException.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromQuery] ListifyFilterModel filter)
        {
            if (!ValidateRequest(id, filter))
                return BadRequest();

            try
            {
                var minValue = filter.MinValue ?? 0;
                var maxValue = filter.MaxValue ?? minValue + id + 100;

                var list = new Listify(minValue, maxValue);

                return Ok(list[id]);
            }
            catch(IndexOutOfRangeException iorException)
            {
                return BadRequest(iorException.Message);
            }
        }

        private bool ValidateRequest(int? id, ListifyFilterModel filter)
        {
            if ((!filter.MinValue.HasValue && !filter.MaxValue.HasValue) || (filter.MinValue.HasValue && filter.MaxValue.HasValue && filter.MaxValue > filter.MinValue))
            {
                if (id.HasValue && ((filter.MinValue.HasValue && filter.MaxValue.HasValue && id.Value > (filter.MaxValue - filter.MinValue))))
                    return false;
                else
                    return true;
            }
            else
                return false;
        }
    }

    public class ListifyFilterModel
    {
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }
}