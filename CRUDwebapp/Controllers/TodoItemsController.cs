using CRUDwebapp.Data;
using CRUDwebapp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDwebapp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;

        public TodoItemsController(AppDbContext context , IWebHostEnvironment webHostEnvironment)

        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        [Route("GetItems")]
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _context.itemsDetails.ToListAsync();
            return Ok(items);
        }


        [Route("CreateItem")]
        [HttpPost]
        public async Task<IActionResult> CreateItem(itemsDetails data)
        {
            if (ModelState.IsValid)
            {
                await _context.itemsDetails.AddAsync(data);
                await _context.SaveChangesAsync();
                var folderPath = System.IO.Path.Combine(_webHostEnvironment.ContentRootPath, "imgs");
                if (!System.IO.Directory.Exists(folderPath))
                {
                    System.IO.Directory.CreateDirectory(folderPath);
                }
                System.IO.File.WriteAllBytes(Path.Combine(folderPath, data.String), Convert.FromBase64String(data.ImageBase64));
            }
                return Ok("Items Added Sucessfully");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {

            var item = await _context.itemsDetails.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, itemsDetails item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }
            var existItem = await _context.itemsDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (existItem == null)
            {
                return NotFound();
            }
            existItem.Name = item.Name;
            existItem.Date = item.Date;
            existItem.String = item.String;
            existItem.ImageBase64 = item.ImageBase64;
            await _context.SaveChangesAsync();
            return Ok("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id, itemsDetails item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }
            var existItem = await _context.itemsDetails.FirstOrDefaultAsync(x => x.Id == id);
            if (existItem == null)
            {
                return NotFound();
            }
            _context.Remove(existItem);
            await _context.SaveChangesAsync();
            return Ok("Deleted Successfully");
        }

        [Route("GetImagePath")]
         [HttpGet]
         public ActionResult GetImagePath(string ImgStr, string ImgName)
         {
            var folderPath = System.IO.Path.Combine(_webHostEnvironment.ContentRootPath, "imgs");
            if (!System.IO.Directory.Exists(folderPath))
             {
                 System.IO.Directory.CreateDirectory(folderPath);
             }
             string imageName = ImgName + ".jpg";
             string imgPath = Path.Combine(folderPath, imageName);
             return Ok(imgPath);
         }

    }
    }
