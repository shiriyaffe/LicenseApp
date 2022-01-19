using System;
using System.Collections.Generic;
using System.Text;
using LicenseApp.Services;

namespace LicenseApp.Models
{
    public partial class Instructor
    {
        public string ImgSource
        {
            get
            {
                LicenseAPIProxy proxy = LicenseAPIProxy.CreateProxy();
                //Create a source with cache busting!
                Random r = new Random();
                string source = $"{proxy.GetBasePhotoUri()}/{this.InstructorId}.jpg?{r.Next()}";
                return source;
            }
        }
    }
}
