using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Raven.Client;

namespace RavenLib
{
    public abstract class RavenModuleBase : NancyModule
    {
        public RavenModuleBase() { }

        public RavenModuleBase(string modulePath)
            : base(modulePath)
        {
        }

        public IDocumentSession DocumentSession
        {
            get { return Context.Items[Conventions.RavenSession] as IDocumentSession; }
        }
    }
}
