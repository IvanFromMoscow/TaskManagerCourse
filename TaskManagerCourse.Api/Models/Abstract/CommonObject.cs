﻿using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Models
{
    public class CommonObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[]? Photo { get; set; }

        public CommonObject()
        {
            CreationDate = DateTime.Now;
        }
        public CommonObject(CommonModel model)
        {
            Name = model.Name;
            Description = model.Description;
            Photo = model.Photo;
            CreationDate = DateTime.Now;
        }
    }
}
