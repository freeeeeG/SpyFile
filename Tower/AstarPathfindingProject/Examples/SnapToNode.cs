using System;
using UnityEngine;

namespace Pathfinding.Examples
{
	// Token: 0x020000F5 RID: 245
	[ExecuteInEditMode]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_snap_to_node.php")]
	public class SnapToNode : MonoBehaviour
	{
		// Token: 0x06000A16 RID: 2582 RVA: 0x00042360 File Offset: 0x00040560
		private void Update()
		{
			if (base.transform.hasChanged && AstarPath.active != null)
			{
				GraphNode node = AstarPath.active.GetNearest(base.transform.position, NNConstraint.None).node;
				if (node != null)
				{
					base.transform.position = (Vector3)node.position;
					base.transform.hasChanged = false;
				}
			}
		}
	}
}
