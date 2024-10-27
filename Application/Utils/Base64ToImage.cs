using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Utils
{
    public class Base64ToImage
    {
        public Image ConvertBase64ToImage(string base64String)
        {
            // Convert Base64 string to byte array
            byte[] imageBytes = Convert.FromBase64String(base64String);

            Image image;
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }
    }
}
