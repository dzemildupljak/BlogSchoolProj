﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSchoolProj.Models.Comment
{
    public class SubComment : Comment
    {
        public int MainCommentId { get; set; }
    }
}