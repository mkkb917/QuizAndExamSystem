﻿using ExamSystem.Data.Static;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamSystem.Models
{
    public class Uploads : BaseEntity
    {
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? FileName { get; set; }
        public string? FileThumbnail { get; set; }
        public UploadsCategory? FileType { get; set; }

        

    }
}
