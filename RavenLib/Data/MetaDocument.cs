using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RavenLib.Data
{
    public class MetaDocument
    {
        public string Id { get; set; }
        public string DocumentType { get; set; }
        public string LastModified { get; set; }
    }
}
