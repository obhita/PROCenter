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


public class Index_RavenStreamHeadBySnapshotAge : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_RavenStreamHeadBySnapshotAge()
	{
		this.ViewText = @"docs.RavenStreamHeads.Select(s => new {
    SnapshotAge = s.HeadRevision - s.SnapshotRevision,
    Partition = ((string)(s.Partition ?? null))
})";
		this.ForEntityNames.Add("RavenStreamHeads");
		this.AddMapDefinition(docs => docs.Where(__document => string.Equals(__document["@metadata"]["Raven-Entity-Name"], "RavenStreamHeads", System.StringComparison.InvariantCultureIgnoreCase)).Select((Func<dynamic, dynamic>)(s => new {
			SnapshotAge = s.HeadRevision - s.SnapshotRevision,
			Partition = ((string)(this.__dynamic_null != s.Partition ? s.Partition : null)),
			__document_id = s.__document_id
		})));
		this.AddField("SnapshotAge");
		this.AddField("Partition");
		this.AddField("__document_id");
	}
}
