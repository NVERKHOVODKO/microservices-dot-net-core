﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Services.Interfaces;
using TestApplication.DTO;

namespace ProductApi.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [Authorize(Roles = "Admin, SuperAdmin, Support")]
    [HttpPost("products")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        var productId = await _productService.CreateProductAsync(request);
        return Ok(productId);
    }

    [Authorize]
    [HttpGet("products")]
    public async Task<IActionResult> GetProducts()
    {
        var product = _productService.GetProducts();
        return Ok(product);
    }

    [Authorize]
    [HttpGet("products/{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await _productService.GetProduct(id);
        return Ok(product);
    }

    [Authorize(Roles = "Admin, SuperAdmin")]
    [HttpPatch("products/edit-name")]
    public async Task<IActionResult> EditName(EditNameRequest request)
    {
        await _productService.UpdateName(request);
        return Ok();
    }

    [Authorize(Roles = "Admin, SuperAdmin")]
    [HttpPatch("products/edit-description")]
    public async Task<IActionResult> EditDescription(EditDescriptionRequest request)
    {
        await _productService.UpdateDescription(request);
        return Ok();
    }

    [Authorize(Roles = "Admin, SuperAdmin")]
    [HttpPatch("products/edit-price")]
    public async Task<IActionResult> EditPrice(EditPriceRequest request)
    {
        await _productService.UpdatePrice(request);
        return Ok();
    }

    [Authorize(Roles = "Admin, SuperAdmin")]
    [HttpPatch("products/edit-availability")]
    public async Task<IActionResult> EditAvailability(EditAvailabilityRequest request)
    {
        await _productService.UpdateAvailability(request);
        return Ok();
    }

    [Authorize(Roles = "Admin, SuperAdmin")]
    [HttpDelete("products/{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await _productService.DeleteProductAsync(id);
        return Ok();
    }
}