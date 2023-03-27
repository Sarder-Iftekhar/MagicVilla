using AutoMapper;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    //[Route("api/[Controller]")] you can write this way also
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        protected APIResponse _response;
        private readonly IVillaNumberRepository _villaNumberRepository;
        // IVillaRepository for checking Valid villa no in Villa Table(Villa no exist or not in villa table)
        private readonly IVillaRepository _villaRepository;
        private readonly IMapper _mapper;
        public VillaNumberAPIController(ApplicationDbContext db,IMapper mapper, IVillaNumberRepository villaNumberRepository, IVillaRepository villaRepository)
        {
            _db = db;
            _mapper = mapper;
            _villaNumberRepository = villaNumberRepository;
            _villaRepository = villaRepository;
            this._response= new();  
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {


                IEnumerable<VillaNumber> villaNumberList = await _villaNumberRepository.GetAll();
                _response.Data = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess= false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
            //return    Ok(await _db.Villas.ToListAsync()); 
        }
        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumberById(int id)
        {
            try { 
            if(id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            var villaNumber = await _villaNumberRepository.GetById(x => x.VillaNo == id);
            if(villaNumber == null)
            {
                 _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }
            _response.Data = _mapper.Map<VillaNumberDTO>(villaNumber);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody]VillaNumberCreateDTO createDTO)
        {
            try { 
                    if (await _villaNumberRepository.GetById(x => x.VillaNo == createDTO.VillaNo)!=null)
                    {
                        ModelState.AddModelError(" ", "Villa number already exist!!!!!!!!!!!!!");
                        return BadRequest(ModelState);

                    }
                    if(await _villaRepository.GetById(x=>x.Id == createDTO.VillaID)==null)
                    {
                            ModelState.AddModelError(" ", "Villa Id is Invalid");   
                            return BadRequest(ModelState);
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
                    VillaNumber model = _mapper.Map<VillaNumber>(createDTO);
            
                    await _villaNumberRepository.Create(model); 

                    _response.Data = _mapper.Map<VillaNumberDTO>(model);
                    //here we are using created rather than ok
                    _response.StatusCode = HttpStatusCode.Created;
                    return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)] 
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try { 
            if(id==0)
            {
                return BadRequest();
            }
            var villaNumber = await _villaNumberRepository.GetById(x=>x.VillaNo == id);
            if (villaNumber == null)
            {
                return NotFound();
            }
   
             await _villaNumberRepository.Delete(villaNumber);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess= true;
            return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }

        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody]VillaNumberUpdateDTO updateDTO)
        {
            try
            {

            if(updateDTO == null || id!= updateDTO.VillaNo)
            {
                return BadRequest();
            }
            if (await _villaRepository.GetById(x => x.Id == updateDTO.VillaID) == null)
            {
                ModelState.AddModelError("Custom Error ", "Villa Id is Invalid");
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
                VillaNumber model = _mapper.Map<VillaNumber>(updateDTO); 
            await _villaNumberRepository.Edit(model); 
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess= true;
            return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return _response;

        }


        //ther is a problem in api testing, test further later
        [HttpPatch("id")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialVillaNumber(int id, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest();
            }
            var villaNumber = _villaNumberRepository.GetById(x => x.VillaNo==id,tracked:false);

            VillaNumberUpdateDTO villaDTO = _mapper.Map<VillaNumberUpdateDTO>(patchDTO);

            if (villaNumber == null )
            {
                return BadRequest();
            }
            patchDTO.ApplyTo(villaDTO, ModelState);
            VillaNumber model = _mapper.Map<VillaNumber>(villaDTO);
            await  _villaNumberRepository.Edit(model); 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return NoContent();

        }
    }
}
