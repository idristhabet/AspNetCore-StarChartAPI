using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestial = _context.CelestialObjects.Find(id);
            if (celestial == null)
            {
                return NotFound();
            }
            celestial.Satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id).ToList();
            return Ok(celestial);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestiaobjects = _context.CelestialObjects.Where(o => o.Name == name).ToList();
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

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialobject)
        {
            _context.CelestialObjects.Add(celestialobject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialobject.Id }, celestialobject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id,CelestialObject celestiaobject)
        {
            var celestia = _context.CelestialObjects.Find(id) ;
            if (celestia ==null )
            {
                return NotFound();
            }

            celestia.Name = celestiaobject.Name;
            celestia.OrbitalPeriod = celestiaobject.OrbitalPeriod;
            celestia.OrbitedObjectId = celestiaobject.OrbitedObjectId;
            _context.CelestialObjects.Update(celestia);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id,string name)
        {
            var objectdesired= _context.CelestialObjects.Find(id);
            if (objectdesired ==null)
            {
                return NotFound(); 
            }

            objectdesired.Name = name;
            _context.CelestialObjects.Update(objectdesired);
            _context.SaveChanges();return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existobject = _context.CelestialObjects.Where(p => p.Id == id|| p.OrbitedObjectId==id).ToList();
            if (!existobject.Any())
            {
                return NotFound(); 
            }
           
                _context.CelestialObjects.RemoveRange(existobject);
            _context.SaveChanges();
            return NoContent(); 
            
        }
    }
}
