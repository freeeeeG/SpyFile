using System;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000086 RID: 134
	[Obsolete("Use AstarPath.navmeshUpdates instead. You can safely remove this component.")]
	[HelpURL("http://arongranberg.com/astar/documentation/stable/class_pathfinding_1_1_tile_handler_helper.php")]
	public class TileHandlerHelper : VersionedMonoBehaviour
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x00028096 File Offset: 0x00026296
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x000280A7 File Offset: 0x000262A7
		public float updateInterval
		{
			get
			{
				return AstarPath.active.navmeshUpdates.updateInterval;
			}
			set
			{
				AstarPath.active.navmeshUpdates.updateInterval = value;
			}
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000280B9 File Offset: 0x000262B9
		[Obsolete("All navmesh/recast graphs now use navmesh cutting")]
		public void UseSpecifiedHandler(TileHandler newHandler)
		{
			throw new Exception("All navmesh/recast graphs now use navmesh cutting");
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000280C5 File Offset: 0x000262C5
		public void DiscardPending()
		{
			AstarPath.active.navmeshUpdates.DiscardPending();
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000280D6 File Offset: 0x000262D6
		public void ForceUpdate()
		{
			AstarPath.active.navmeshUpdates.ForceUpdate();
		}
	}
}
