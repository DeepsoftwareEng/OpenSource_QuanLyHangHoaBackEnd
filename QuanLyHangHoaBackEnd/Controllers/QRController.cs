using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using QuanLyHangHoaEsteel_BackEnd.Models;
using QuanLyHangHoaEsteel_BackEnd.Repository;
using QRCoder;
using System.Drawing;

namespace QuanLyHangHoaEsteel_BackEnd.Controllers
{
    [ApiController]
    [Route("api/qrcode")]
    public class QRController : Controller
    {
        private readonly IProductRepository productRepository;

        public QRController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
       
        [HttpDelete("delete-qr")]
        public IActionResult Delete(string productCode)
        {
            // qRCodeRepository.DeleteQRCode(qr.Id);
            var prod = productRepository.GetById(productCode);
            prod.hasCode = 0;
            var del = productRepository.UpdateProduct(prod);
            if (!del)
            {
                return Ok(new
                {
                    response_code = 00,
                    response_data = "Lỗi khi xóa mã"
                });
            }
            return Ok(new
            {
                response_code = 200,
                response_data = "Xóa mã thành công"
            });
        }
        [HttpGet("save-by-prod")]
        public IActionResult SaveByProd(string ProductCode)
        {
            var prod = productRepository.GetById(ProductCode);
            try
            {
                byte[] content = new byte[0];
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(prod.ProductCode, QRCodeGenerator.ECCLevel.M);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                string filepath = "D:\\MediaDatabase\\QuanLySanPhamEsteel\\QRImage\\qr_" + prod.ProductCode + ".png";
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    qrCodeImage.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                }
                content = System.IO.File.ReadAllBytes(filepath);
                var temp = new
                {
                    name = prod.ProductName,
                    data = content.ToArray()
                };
            return Ok(new
            {
                response_code = 200,
                response_data = temp
            });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    response_code = 300,
                    response_data = "Lỗi khi xử lý mã"
                });
            }
        }
        [HttpGet("save-all")]
        public IActionResult SaveAllQr()
        {
            var prod = productRepository.GetAllCoded();
            List<dynamic> imageData = new List<dynamic>();
            foreach(var i in prod)
            {
                try
                {
                    byte[] content = new byte[0];
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(i.ProductCode, QRCodeGenerator.ECCLevel.M);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);
                    string filepath = "D:\\MediaDatabase\\QuanLySanPhamEsteel\\QRImage\\qr_" + i.ProductCode + ".png";
                    if(System.IO.File.Exists(filepath))
                    {
                        System.IO.File.Delete(filepath);
                    }
                    using(FileStream fs = new FileStream(filepath, FileMode.Create))
                    {
                        qrCodeImage.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
                    }
                    content = System.IO.File.ReadAllBytes(filepath);
                    var temp = new
                    {
                        name = i.ProductName,
                        data = content.ToArray()
                    };
                    imageData.Add(temp);
                }
                catch (Exception ex)
                {
                    return Ok(new
                    {
                        response_code = 300,
                        response_data = "Lỗi khi xử lý mã"
                    });
                }
               
            }
            return Ok(new
            {
                response_code = 200,
                response_data = imageData
            });
        }
    }
}
