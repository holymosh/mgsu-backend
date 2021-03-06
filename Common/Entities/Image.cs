﻿using System;
using System.IO;

namespace Common.Entities
{
    public class Image
    {
        public Image(){}

        public Image(string bigPhotoUri, string smallPhotoUri)
        {
            Original = bigPhotoUri;
            Small = smallPhotoUri;
        }

        public string Original { get; set; }
        public string Small { get; set; }

        public string Role { get; set; }
    }
}