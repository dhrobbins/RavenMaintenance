using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using Raven.Abstractions.Indexing;

namespace RavenLib.Data
{
    public class AllDocsByTypeIndex : AbstractIndexCreationTask
    {

        public override IndexDefinition CreateIndexDefinition()
        {
            var index = new IndexDefinition();
            index.Name = this.IndexName;
            index.Map = @"from doc in docs 
let DocumentType = ((dynamic)doc)[""@metadata""][""Raven-Entity-Name""]
let Id = doc[""@metadata""][""Id""]
let LastModified = doc[""@metadata""][""Last-Modified""]
select new {DocumentType = ((dynamic)doc)[""@metadata""][""Raven-Entity-Name""], Id, LastModified}";

            index.TransformResults = @"from result in results
            select new {Id = result.Id, DocumentType = result.DocumentType, LastModified = result.LastModified}";

            return index;
        }
    }
}
