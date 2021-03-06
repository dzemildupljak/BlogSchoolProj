﻿using BlogSchoolProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSchoolProj.ViewModels
{
    public class IndexViewModel
    {
        public List<Post> Posts { get; set; }
        public int PageNumber { get; set; }
        public bool NextPage { get; set; }
        public string Category { get; set; }
        public int PageCount { get; set; }
    }
}
