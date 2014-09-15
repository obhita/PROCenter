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


public class Index_RavenSnapshotByStreamIdAndRevision : Raven.Database.Linq.AbstractViewGenerator
{
	public Index_RavenSnapshotByStreamIdAndRevision()
	{
		this.ViewText = @"docs.RavenSnapshots.Select(s => new {
    StreamId = s.StreamId,
    StreamRevision = s.StreamRevision,
    Partition = ((string)(s.Partition ?? null))
})";
		this.ForEntityNames.Add("RavenSnapshots");
		this.AddMapDefinition(docs => docs.Where(__document => string.Equals(__document["@metadata"]["Raven-Entity-Name"], "RavenSnapshots", System.StringComparison.InvariantCultureIgnoreCase)).Select((Func<dynamic, dynamic>)(s => new {
			StreamId = s.StreamId,
			StreamRevision = s.StreamRevision,
			Partition = ((string)(this.__dynamic_null != s.Partition ? s.Partition : null)),
			__document_id = s.__document_id
		})));
		this.AddField("StreamId");
		this.AddField("StreamRevision");
		this.AddField("Partition");
		this.AddField("__document_id");
	}
}
