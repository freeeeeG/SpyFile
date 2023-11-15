using System;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000056 RID: 86
	[AddComponentMenu("Pathfinding/Navmesh/RecastTileUpdate")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_recast_tile_update.php")]
	public class RecastTileUpdate : MonoBehaviour
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000420 RID: 1056 RVA: 0x00014D14 File Offset: 0x00012F14
		// (remove) Token: 0x06000421 RID: 1057 RVA: 0x00014D48 File Offset: 0x00012F48
		public static event Action<Bounds> OnNeedUpdates;

		// Token: 0x06000422 RID: 1058 RVA: 0x00014D7B File Offset: 0x00012F7B
		private void Start()
		{
			this.ScheduleUpdate();
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00014D83 File Offset: 0x00012F83
		private void OnDestroy()
		{
			this.ScheduleUpdate();
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x00014D8C File Offset: 0x00012F8C
		public void ScheduleUpdate()
		{
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				if (RecastTileUpdate.OnNeedUpdates != null)
				{
					RecastTileUpdate.OnNeedUpdates(component.bounds);
					return;
				}
			}
			else if (RecastTileUpdate.OnNeedUpdates != null)
			{
				RecastTileUpdate.OnNeedUpdates(new Bounds(base.transform.position, Vector3.zero));
			}
		}
	}
}
