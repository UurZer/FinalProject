﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            //Dependency chain -0-0-0
            var result = _productService.GetAll();
            if (result.Success)
            {
                return Ok(result);//Datayı alır
            }
            return BadRequest(result);//Message alır
        }
        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            //Dependency chain -0-0-0
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);//Datayı alır
            }
            return BadRequest(result);//Message alır
        }
        [HttpPost("add")]
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("transaction")]
        public IActionResult TransactionTest(Product product)
        {
             var result = _productService.AddTransactionalTest(product);
            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    }
}
