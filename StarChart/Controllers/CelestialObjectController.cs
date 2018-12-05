using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:id}",Name="GetById")]
        public IActionResult GetById(int id)
        {
            var celestial = _context.CelestialObjects.Find(id);
            if (celestial ==null)
            {
                return NotFound();
            }
            celestial.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id).ToList();
            return Ok(celestial);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestiaobjects = _context.CelestialObjects.Where(o=>o.Name==name).ToList();
            if (!celestiaobjects.Any())
            {
                return NotFound();
            }          
            foreach (var item in celestiaobjects)
            {
                item.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == item.Id).ToList();
            }          
            return Ok(celestiaobjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestiaobjects = _context.CelestialObjects.ToList();
            foreach (var item in celestiaobjects)
            {
                item.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == item.OrbitedObjectId).ToList();
            }
            return Ok(celestiaobjects);
        }
    }
}
