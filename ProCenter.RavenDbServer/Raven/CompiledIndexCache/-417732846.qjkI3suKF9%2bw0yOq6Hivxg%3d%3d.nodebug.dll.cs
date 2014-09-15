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


public class Index_RavenCommitByDate : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_RavenCommitByDate()
	{
		this.ViewText = @"docs.RavenCommits.Select(c => new {
    CommitStamp = c.CommitStamp,
    Partition = ((string)(c.Partition ?? null))
})";
		this.ForEntityNames.Add("RavenCommits");
		this.AddMapDefinition(docs => docs.Where(__document => string.Equals(__document["@metadata"]["Raven-Entity-Name"], "RavenCommits", System.StringComparison.InvariantCultureIgnoreCase)).Select((Func<dynamic, dynamic>)(c => new {
			CommitStamp = c.CommitStamp,
			Partition = ((string)(this.__dynamic_null != c.Partition ? c.Partition : null)),
			__document_id = c.__document_id
		})));
		this.AddField("CommitStamp");
		this.AddField("Partition");
		this.AddField("__document_id");
	}
}
