using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000054 RID: 84
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_local_space_graph.php")]
	public class LocalSpaceGraph : VersionedMonoBehaviour
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x00014B08 File Offset: 0x00012D08
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x00014B10 File Offset: 0x00012D10
		public GraphTransform transformation { get; private set; }

		// Token: 0x0600041B RID: 1051 RVA: 0x00014B19 File Offset: 0x00012D19
		private void Start()
		{
			this.originalMatrix = base.transform.worldToLocalMatrix;
			base.transform.hasChanged = true;
			this.Refresh();
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00014B3E File Offset: 0x00012D3E
		public void Refresh()
		{
			if (base.transform.hasChanged)
			{
				this.transformation = new GraphTransform(base.transform.localToWorldMatrix * this.originalMatrix);
				base.transform.hasChanged = false;
			}
		}

		// Token: 0x04000275 RID: 629
		private Matrix4x4 originalMatrix;
	}
}
