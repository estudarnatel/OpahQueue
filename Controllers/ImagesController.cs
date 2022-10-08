
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpahQueue.Data;
using OpahQueue.Models;

namespace OpahQueue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly DBContext _context;

        public ImagesController(DBContext context)
        {
            _context = context;
        }

        [HttpPost("user/{id}")]
        public ActionResult StoreImage(int id)
        {
            var image = _context.Images.SingleOrDefault(pic => pic.UserId == id);
            if (image != null)
            {
                _context.Images.Remove(image);
                _context.SaveChanges();
            }
            Image img = new Image();
            img.UserId = id;
            foreach (var file in Request.Form.Files)
            {
                img.ImageTitle = file.FileName;
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                img.ImageData = ms.ToArray();
                ms.Close();
                ms.Dispose();
                _context.Images.Add(img);
                _context.SaveChanges();
            }
            return Ok(img);
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.UserId == id);
        }
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutImage(int id)
        {
            var image = _context.Images.SingleOrDefault(pic => pic.UserId == id);
            foreach (var file in Request.Form.Files)
            {
                // image.ImageTitle = file.FileName;
                image!.ImageTitle = file.FileName;
                MemoryStream ms = new MemoryStream();
                file.CopyTo(ms);
                image.ImageData = ms.ToArray();
                ms.Close();
                ms.Dispose();
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            // string imageBase64Data = Convert.ToBase64String(image.ImageData);
            string imageBase64Data = Convert.ToBase64String(image!.ImageData);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            return Ok(new { userId = image.UserId, imageSRCcode = imageDataURL});
        }

        [HttpGet()]
        public ActionResult GetImagesCodes()
        {
            List<Image> meus = _context.Images.ToList();
            List<ImageCode> imgback = new List<ImageCode>();
            foreach (Image one in meus)
            {
                string imageBase64Data = Convert.ToBase64String(one.ImageData);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                var nova = new ImageCode
                {
                    UserId = one.UserId,
                    imageSRCcode = imageDataURL
                };
                imgback.Add(nova);
            }
            return Ok(imgback);
        }

        [HttpGet("by/{id}")]
        public ActionResult RetrieveImage(int id)
        {
            // Image img = _context.Images.Find(id);
            Image img = _context.Images.Find(id)!;
            string imageBase64Data = Convert.ToBase64String(img.ImageData);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            // return Ok(imageBase64Data); // DADOS DA IMAGEM
            // return Ok(imageDataURL); // COLOCAR ESSA STRING NO SRC DA TAG IMG // SE COLOCAR NA URL DO NAVEGADOR TAMBÉM FUNCIONA.
            // return Ok(new { src = imageDataURL});
            return Ok(new { userId = img.UserId, imageSRCcode = imageDataURL});
            // BASEADO NESSE VIDEO DO YOUTUBE E NO TUTORIAL OFICIAL DO ASPNET MVC DA MICROSOFT:
            // https://www.youtube.com/watch?v=FcdXA6EYWyM
            // https://www.youtube.com/watch?v=FcdXA6EYWyM
            // https://www.youtube.com/watch?v=FcdXA6EYWyM
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
