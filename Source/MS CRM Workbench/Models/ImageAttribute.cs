using System;


namespace PZone.Models
{
    public class ImageAttribute : Attribute
    {
        public string Path { get; set; }


        public ImageAttribute(string path)
        {
            Path = path;
        }
    }
}