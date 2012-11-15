using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy.Routing;
using Nancy.ViewEngines;
using Nancy;
using Nancy.ModelBinding;
using RavenLib.Data;
using Raven.Client;
using Nancy.Responses.Negotiation;
using Nancy.Validation;
using Nancy.Extensions;

namespace RavenLib
{
    public class RavenAwareModuleBuilder : INancyModuleBuilder
    {
        private readonly IViewFactory viewFactory;
        private readonly IResponseFormatterFactory responseFormatterFactory;
        private readonly IModelBinderLocator modelBinderLocator;
        private readonly IModelValidatorLocator validatorLocator;
        private readonly IRavenSessionProvider ravenSessionProvider;

        public RavenAwareModuleBuilder(IViewFactory viewFactory, IResponseFormatterFactory responseFormatterFactory, 
                                         IModelBinderLocator modelBinderLocator, 
                                        IModelValidatorLocator validatorLocator, 
                                        IRavenSessionProvider ravenSessionProvider)
        {
            this.viewFactory = viewFactory;
            this.responseFormatterFactory = responseFormatterFactory;
            this.modelBinderLocator = modelBinderLocator;
            this.validatorLocator = validatorLocator;
            this.ravenSessionProvider = ravenSessionProvider;
        }

        public NancyModule BuildModule(NancyModule module, NancyContext context)
        {
            context.NegotiationContext = new NegotiationContext
            {
                ModuleName = module.GetModuleName(),
                ModulePath = module.ModulePath,
            };

            module.Context = context;
            module.Response = this.responseFormatterFactory.Create(context);
            module.ViewFactory = this.viewFactory;
            module.ModelBinderLocator = this.modelBinderLocator;
            module.ValidatorLocator = this.validatorLocator;
            context.Items.Add(Conventions.RavenSession, this.ravenSessionProvider.GetSession());

            module.After.AddItemToStartOfPipeline(ctx =>
            {
                var session =
                    ctx.Items[Conventions.RavenSession] as IDocumentSession;

                if (session == null)
                {
                    return;
                }
                
                session.SaveChanges();
                session.Dispose();
            });
            return module;
        }
    }
}
