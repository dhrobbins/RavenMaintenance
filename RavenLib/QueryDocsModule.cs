using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nancy;
using Raven.Client;
using Raven.Client.Document;
using RavenLib.Data;
using Raven.Json.Linq;

namespace RavenLib
{
    public class QueryDocsModule : RavenModuleBase
    {
        public QueryDocsModule()
            : base("QueryDocs")
        {
            Post["/Query/AllDocsByType/"] = parameters =>
                {
                    var docs = DocumentSession.Query<MetaDocument>("AllDocsByTypeIndex")
                                                .ToList();
                                                

                    return Response.AsJson(docs);
                };

            Post["/Query/JsonForDoc/{id}"] = parameters =>
                {
                    RavenJObject jsonDoc = DocumentSession.Load<RavenJObject>(parameters.id);                    

                    return Response.AsJson(jsonDoc);
                };

            Get["/Query/QueryTool/"] = x =>
                {
                    return View["QueryTool.htm"];
                };
            
        }
    }
}
