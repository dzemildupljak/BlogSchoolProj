using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogSchoolProj.Data.FileManager
{
    public interface IFileManager
    {
        FileStream ImageStream(string image);
        Task<String> SaveImage(IFormFile image);
    }
}
