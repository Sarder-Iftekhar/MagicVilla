﻿using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    //[Route("api/[Controller]")] you can write this way also
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Villa> GetVillas()
        {
            //List<Villa> x =new List<Villa>
            // {

            // }
            //return x;

            return new List<Villa>
            {
                new Villa{ Id = 1, Name ="Pool View" },
                new Villa{ Id = 2, Name ="Beach View"}
            };
           

        }
    }
}