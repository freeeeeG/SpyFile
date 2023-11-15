using System;
using System.Collections;
using System.Collections.Generic;

// Token: 0x0200025D RID: 605
public class AcyclicGraph<NodeContents, LinkContents> : IEnumerable where NodeContents : class where LinkContents : class
{
	// Token: 0x06000B2E RID: 2862 RVA: 0x0003C037 File Offset: 0x0003A437
	public AcyclicGraph()
	{
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x0003C055 File Offset: 0x0003A455
	public AcyclicGraph(NodeContents _head)
	{
		this.FindOrAddNode(_head);
	}

	// Token: 0x06000B30 RID: 2864 RVA: 0x0003C07C File Offset: 0x0003A47C
	public AcyclicGraph<NodeContents, LinkContents>.LinkKey AddLink(NodeContents t1, NodeContents t2, LinkContents _linkData)
	{
		AcyclicGraph<NodeContents, LinkContents>.Node node = this.FindOrAddNode(t1);
		AcyclicGraph<NodeContents, LinkContents>.Node node2 = this.FindOrAddNode(t2);
		AcyclicGraph<NodeContents, LinkContents>.Link link = new AcyclicGraph<NodeContents, LinkContents>.Link(node2, _linkData);
		node.m_connected.Add(link);
		AcyclicGraph<NodeContents, LinkContents>.Link link2 = new AcyclicGraph<NodeContents, LinkContents>.Link(node, _linkData);
		node2.m_connected.Add(link2);
		AcyclicGraph<NodeContents, LinkContents>.LinkKey linkKey = new AcyclicGraph<NodeContents, LinkContents>.LinkKey();
		this.m_keyLinkLookup[linkKey] = new AcyclicGraph<NodeContents, LinkContents>.TwowayLink(link, link2);
		return linkKey;
	}

	// Token: 0x06000B31 RID: 2865 RVA: 0x0003C0E0 File Offset: 0x0003A4E0
	public static AcyclicGraph<NodeContents, LinkContents> Merge(AcyclicGraph<NodeContents, LinkContents> _graph1, AcyclicGraph<NodeContents, LinkContents> _graph2)
	{
		AcyclicGraph<NodeContents, LinkContents> acyclicGraph = new AcyclicGraph<NodeContents, LinkContents>();
		AcyclicGraphEnumerator<NodeContents, LinkContents> enumerator = _graph1.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				NodeContents nodeContents = enumerator.Current;
				AcyclicGraph<NodeContents, LinkContents>.Node node = _graph1.GetNode(nodeContents);
				foreach (AcyclicGraph<NodeContents, LinkContents>.Link link in node.m_connected)
				{
					acyclicGraph.AddLink(nodeContents, link.m_target.m_value, link.m_data);
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		AcyclicGraphEnumerator<NodeContents, LinkContents> enumerator3 = _graph2.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				NodeContents nodeContents2 = enumerator3.Current;
				AcyclicGraph<NodeContents, LinkContents>.Node node2 = _graph2.GetNode(nodeContents2);
				foreach (AcyclicGraph<NodeContents, LinkContents>.Link link2 in node2.m_connected)
				{
					acyclicGraph.AddLink(nodeContents2, link2.m_target.m_value, link2.m_data);
				}
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator3 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		return acyclicGraph;
	}

	// Token: 0x06000B32 RID: 2866 RVA: 0x0003C260 File Offset: 0x0003A660
	private AcyclicGraph<NodeContents, LinkContents>.Node FindOrAddNode(NodeContents _value)
	{
		AcyclicGraph<NodeContents, LinkContents>.Node node = this.m_nodes.Find((AcyclicGraph<NodeContents, LinkContents>.Node n) => n.m_value == _value);
		if (node == null)
		{
			node = new AcyclicGraph<NodeContents, LinkContents>.Node();
			node.m_value = _value;
			this.m_nodes.Add(node);
		}
		return node;
	}

	// Token: 0x06000B33 RID: 2867 RVA: 0x0003C2B8 File Offset: 0x0003A6B8
	public void GetNodesFromLink(AcyclicGraph<NodeContents, LinkContents>.LinkKey _key, out AcyclicGraph<NodeContents, LinkContents>.Node _node1, out AcyclicGraph<NodeContents, LinkContents>.Node _node2)
	{
		AcyclicGraph<NodeContents, LinkContents>.TwowayLink twowayLink = this.m_keyLinkLookup[_key];
		_node1 = twowayLink.m_link1.m_target;
		_node2 = twowayLink.m_link2.m_target;
	}

	// Token: 0x06000B34 RID: 2868 RVA: 0x0003C2EC File Offset: 0x0003A6EC
	public void RemoveLink(AcyclicGraph<NodeContents, LinkContents>.LinkKey _key)
	{
		AcyclicGraph<NodeContents, LinkContents>.TwowayLink twoWayLink = this.m_keyLinkLookup[_key];
		foreach (AcyclicGraph<NodeContents, LinkContents>.Node node2 in this.m_nodes)
		{
			node2.m_connected.RemoveAll((AcyclicGraph<NodeContents, LinkContents>.Link link) => link == twoWayLink.m_link1);
			node2.m_connected.RemoveAll((AcyclicGraph<NodeContents, LinkContents>.Link link) => link == twoWayLink.m_link2);
		}
		this.m_nodes.RemoveAll((AcyclicGraph<NodeContents, LinkContents>.Node node) => node.m_connected.Count == 0);
	}

	// Token: 0x06000B35 RID: 2869 RVA: 0x0003C3B4 File Offset: 0x0003A7B4
	public AcyclicGraph<NodeContents, LinkContents>.Node GetNode(NodeContents _value)
	{
		return this.m_nodes.Find((AcyclicGraph<NodeContents, LinkContents>.Node n) => n.m_value == _value);
	}

	// Token: 0x06000B36 RID: 2870 RVA: 0x0003C3E5 File Offset: 0x0003A7E5
	IEnumerator IEnumerable.GetEnumerator()
	{
		return this.GetEnumerator();
	}

	// Token: 0x06000B37 RID: 2871 RVA: 0x0003C3ED File Offset: 0x0003A7ED
	public AcyclicGraphEnumerator<NodeContents, LinkContents> GetEnumerator()
	{
		return new AcyclicGraphEnumerator<NodeContents, LinkContents>(this.m_nodes);
	}

	// Token: 0x040008C7 RID: 2247
	private Dictionary<AcyclicGraph<NodeContents, LinkContents>.LinkKey, AcyclicGraph<NodeContents, LinkContents>.TwowayLink> m_keyLinkLookup = new Dictionary<AcyclicGraph<NodeContents, LinkContents>.LinkKey, AcyclicGraph<NodeContents, LinkContents>.TwowayLink>();

	// Token: 0x040008C8 RID: 2248
	private List<AcyclicGraph<NodeContents, LinkContents>.Node> m_nodes = new List<AcyclicGraph<NodeContents, LinkContents>.Node>();

	// Token: 0x0200025E RID: 606
	public class Node
	{
		// Token: 0x040008CA RID: 2250
		public NodeContents m_value;

		// Token: 0x040008CB RID: 2251
		public List<AcyclicGraph<NodeContents, LinkContents>.Link> m_connected = new List<AcyclicGraph<NodeContents, LinkContents>.Link>();
	}

	// Token: 0x0200025F RID: 607
	public class Link
	{
		// Token: 0x06000B3A RID: 2874 RVA: 0x0003C41D File Offset: 0x0003A81D
		public Link(AcyclicGraph<NodeContents, LinkContents>.Node n, LinkContents data)
		{
			this.m_target = n;
			this.m_data = data;
		}

		// Token: 0x040008CC RID: 2252
		public AcyclicGraph<NodeContents, LinkContents>.Node m_target;

		// Token: 0x040008CD RID: 2253
		public LinkContents m_data;
	}

	// Token: 0x02000260 RID: 608
	public class LinkKey
	{
	}

	// Token: 0x02000261 RID: 609
	private class TwowayLink
	{
		// Token: 0x06000B3C RID: 2876 RVA: 0x0003C43B File Offset: 0x0003A83B
		public TwowayLink(AcyclicGraph<NodeContents, LinkContents>.Link _link1, AcyclicGraph<NodeContents, LinkContents>.Link _link2)
		{
			this.m_link1 = _link1;
			this.m_link2 = _link2;
		}

		// Token: 0x040008CE RID: 2254
		public AcyclicGraph<NodeContents, LinkContents>.Link m_link1;

		// Token: 0x040008CF RID: 2255
		public AcyclicGraph<NodeContents, LinkContents>.Link m_link2;
	}
}
