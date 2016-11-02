using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashBoard.Web.Models
{
    public class ADViewModel
    {
    }
    public class groupViewModel
    {
        public IEnumerable<string> ObjectAsString { get; set; }
    }

    public class groupViewModelClass
    {
        public string Name { set; get; }
        public string Description { get; set; }
    }
    
}