using System;
using UnityEngine;

// Token: 0x02000B0F RID: 2831
[RequireComponent(typeof(GraphBase))]
[AddComponentMenu("KMonoBehaviour/scripts/GraphLayer")]
public class GraphLayer : KMonoBehaviour
{
	// Token: 0x17000669 RID: 1641
	// (get) Token: 0x0600574F RID: 22351 RVA: 0x001FECE7 File Offset: 0x001FCEE7
	public GraphBase graph
	{
		get
		{
			if (this.graph_base == null)
			{
				this.graph_base = base.GetComponent<GraphBase>();
			}
			return this.graph_base;
		}
	}

	// Token: 0x04003AF7 RID: 15095
	[MyCmpReq]
	private GraphBase graph_base;
}
