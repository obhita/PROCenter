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


public class Index_AllDocuments : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_AllDocuments()
	{
		this.ViewText = @"from doc in docs select new {doc}";
		this.AddMapDefinition(docs => 
			from doc in docs
			select new {
				doc,
				__document_id = doc.__document_id
			});
		this.AddField("__document_id");
		this.AddField("doc");
		this.AddQueryParameterForMap("__document_id");
		this.AddQueryParameterForReduce("__document_id");
	}
}
