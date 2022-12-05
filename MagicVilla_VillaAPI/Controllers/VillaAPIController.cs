using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Controllers
{
    //[Route("api/[Controller]")] you can write this way also
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        public VillaAPIController(ApplicationDbContext db,IMapper mapper,IVillaRepository villaRepository)
        {
            _db = db;
            _mapper = mapper;
            _villaRepository=villaRepository;

        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            IEnumerable<Villa> villaList = await _villaRepository.GetAll();
            return Ok(_mapper.Map<List<VillaDTO>>(villaList)); 
            //return  Ok(await _db.Villas.ToListAsync());
           
        }
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVillaById(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = await _villaRepository.GetById(x => x.Id == id);
            if(villa == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<VillaDTO>(villa));
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody]VillaCreateDTO createDTO)
        {
            if (await _villaRepository.GetById(x => x.Name.ToLower() == createDTO.Name.ToLower())!=null)
            {
                ModelState.AddModelError(" ", "Villa already exist!!!!!!!!!!!!!");
                return BadRequest();

            }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            //villaDTO.Id = VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1; no need this when use database
            //it cannot implicitly convert villaDto to villa ,so we have to do a manual conversion
            //like this we will create a new villa here 
            //Villa model = new Villa()
            //{
            //    Amenity = createDTO.Amenity,
            //    Details = createDTO.Details,
            //    ImageUrl = createDTO.ImageUrl,
            //    Name = createDTO.Name,
            //    Occupancy = createDTO.Occupancy,
            //    Rate = createDTO.Rate,
            //    Sqft = createDTO.Sqft,
            //};
            //but we dont need above conversion , instead use mapping
            Villa model = _mapper.Map<Villa>(createDTO);
            await _villaRepository.Create(model);
            return Ok(createDTO);
        }

        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)] 
        public async Task<ActionResult<VillaDTO>> Delete(int id)
        {
            if(id==0)
            {
                return BadRequest();
            }
            var villa = await _villaRepository.GetById(x=>x.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            //we do not have remove async
             _villaRepository.Delete(villa);
            await _db.SaveChangesAsync();
            //you can use NoContent orOk whatever you want
            //return NoContent();
            return Ok();

        }

        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody]VillaUpdateDTO updateDTO)
        {
            if(updateDTO == null || id!= updateDTO.Id)
            {
                return BadRequest();
            }
            //Villa model = new Villa()
            //{
            //    Amenity = updateDTO.Amenity,
            //    Details = updateDTO.Details,
            //    Id = updateDTO.Id,
            //    ImageUrl = updateDTO.ImageUrl,
            //    Name = updateDTO.Name,
            //    Occupancy = updateDTO.Occupancy,
            //    Rate = updateDTO.Rate,
            //    Sqft = updateDTO.Sqft,
            //};
            //but instead above mapping, we can add online code which is auto mapper
            Villa model = _mapper.Map<Villa>(updateDTO); 
            await _villaRepository.Edit(model);
            return NoContent();
    
        }


        //ther is a problem in api testing, test further later
        [HttpPatch("id")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villa = _villaRepository.GetById(x => x.Id==id,tracked:false);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(patchDTO);

            if (villa == null )
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);
            Villa model = _mapper.Map<Villa>(villaDTO);
            await  _villaRepository.Edit(model); 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }
    }
}
