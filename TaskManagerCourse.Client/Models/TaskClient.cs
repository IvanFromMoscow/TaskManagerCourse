using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TaskManagerCourse.Client.Models.Extensions;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Models
{
    public class TaskClient
    {
        public TaskModel Model { get; set; }
        public UserModel? Creator { get; set; }
        public UserModel? Executor { get; set; }

        public BitmapImage Image
        {
            get =>  Model.LoadImage();
        }
        public bool IsHaveFile 
        {
            get => Model?.File != null;
        }
        public TaskClient(TaskModel model)
        {
            Model = model;
        }
    }
}
