using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Models.Extensions
{
    public static class CommonModelExtention
    {
        public static BitmapImage LoadImage(this CommonModel model)
        {
            if (model.Photo == null || model.Photo.Length == 0)
            {
                return null;
            }
            var image = new BitmapImage();
            using (var memstr = new MemoryStream(model.Photo))
            {
                memstr.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = memstr;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
    }

