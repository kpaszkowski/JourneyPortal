using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using JourneyPortal.Models;

namespace JourneyPortal.Helpers
{
    public static class ImageHelper
    {
        public static Image PrepareImage(HttpPostedFileBase file)
        {
            byte[] fileData = null;
            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                fileData = binaryReader.ReadBytes(file.ContentLength);
            }

            Image image = new Image();
            var fileName = Path.GetFileName(file.FileName);
            var ext = Path.GetExtension(file.FileName);
            string name = Path.GetFileNameWithoutExtension(fileName);
            string myfile = name + "_" + image.Name + ext;
            var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images"), myfile);
            image.Name = Guid.NewGuid().ToString();
            image.ImageUrl = path;
            image.Binary = fileData;
            return image;
        }
    }
}