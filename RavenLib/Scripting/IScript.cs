using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RavenLib.Scripting
{
    public interface IScript
    {
        void Process(Dictionary<string, object> parameters);
    }
}
