using Raven.Abstractions;
using Raven.Database.Linq;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System;
using Raven.Database.Linq.PrivateExtensions;
using Lucene.Net.Documents;
using System.Globalization;
using System.Text.RegularExpressions;
using Raven.Database.Indexing;


public class Index_EventStoreDocumentsByEntityName : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_EventStoreDocumentsByEntityName()
	{
		this.ViewText = @"from doc in docs 
                        let Tag = doc[""@metadata""][""Raven-Entity-Name""]
                        where  Tag != null 
                        select new { Tag, LastModified = (DateTime)doc[""@metadata""][""Last-Modified""], Partition = doc.Partition ?? null };";
		this.AddMapDefinition(docs => 
			from doc in docs
			let Tag = doc["@metadata"]["Raven-Entity-Name"]
			where Tag != null
			select new {
				Tag,
				LastModified = (DateTime)doc["@metadata"]["Last-Modified"],
				Partition = this.__dynamic_null != doc.Partition ? doc.Partition : null,
				__document_id = doc.__document_id
			});
		this.AddField("LastModified");
		this.AddField("Partition");
		this.AddField("__document_id");
		this.AddField("Tag");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("__document_id");
	}
}
