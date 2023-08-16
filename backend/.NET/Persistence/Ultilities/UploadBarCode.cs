using System;
using System.Drawing;
using System.IO;
using Domain.Entities;
using QRCoder;

namespace Persistence.Ultilities
{
    public class UploadBarCode
    {       
        public String generateAndUploadQRCode(String content)
        {
            try
            {
                QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);

                Bitmap qrCodeBitmap = qrCode.GetGraphic(20); // Kích thước của mã vạch

                string filePath = uploadImages(qrCodeBitmap, Guid.NewGuid().ToString()+ ".png");
                return filePath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public string uploadImages(Bitmap image, string filename)
        {
            try
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "QRProduct");
                var filePath = Path.Combine(uploadPath, filename);

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.Save(fileStream, System.Drawing.Imaging.ImageFormat.Png);
                }               
                var path = Path.Combine("QRProduct", filename);
                return path;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }



    }
}
