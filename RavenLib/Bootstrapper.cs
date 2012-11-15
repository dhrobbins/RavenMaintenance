using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Nancy.Conventions;
using Raven.Client;
using Nancy.Session;
using Nancy.Bootstrapper;
using RavenLib;
using RavenLib.Data;
using Raven.Client.Indexes;

namespace WebConfiguration
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoC.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {   
            base.ApplicationStartup(container, pipelines);
            CookieBasedSessions.Enable(pipelines);

            Raven.Client.Indexes.IndexCreation.CreateIndexes(typeof(AllDocsByTypeIndex).Assembly, 
                                                                RavenSessionProvider.DocumentStore);
        }
        
        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Scripts"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Styles"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Templates"));
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(x => x.NancyModuleBuilder = typeof (RavenAwareModuleBuilder)); 
            }
        }
    }
}
