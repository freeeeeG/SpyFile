using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000084 RID: 132
	[Serializable]
	public class NavmeshUpdates
	{
		// Token: 0x06000695 RID: 1685 RVA: 0x00027AB8 File Offset: 0x00025CB8
		internal void OnEnable()
		{
			NavmeshClipper.AddEnableCallback(new Action<NavmeshClipper>(this.HandleOnEnableCallback), new Action<NavmeshClipper>(this.HandleOnDisableCallback));
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00027AD7 File Offset: 0x00025CD7
		internal void OnDisable()
		{
			NavmeshClipper.RemoveEnableCallback(new Action<NavmeshClipper>(this.HandleOnEnableCallback), new Action<NavmeshClipper>(this.HandleOnDisableCallback));
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00027AF8 File Offset: 0x00025CF8
		public void DiscardPending()
		{
			for (int i = 0; i < NavmeshClipper.allEnabled.Count; i++)
			{
				NavmeshClipper.allEnabled[i].NotifyUpdated();
			}
			NavGraph[] graphs = AstarPath.active.graphs;
			for (int j = 0; j < graphs.Length; j++)
			{
				NavmeshBase navmeshBase = graphs[j] as NavmeshBase;
				if (navmeshBase != null)
				{
					navmeshBase.navmeshUpdateData.forcedReloadRects.Clear();
				}
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00027B60 File Offset: 0x00025D60
		private void HandleOnEnableCallback(NavmeshClipper obj)
		{
			NavGraph[] graphs = AstarPath.active.graphs;
			for (int i = 0; i < graphs.Length; i++)
			{
				NavmeshBase navmeshBase = graphs[i] as NavmeshBase;
				if (navmeshBase != null)
				{
					navmeshBase.navmeshUpdateData.AddClipper(obj);
				}
			}
			obj.ForceUpdate();
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00027BA4 File Offset: 0x00025DA4
		private void HandleOnDisableCallback(NavmeshClipper obj)
		{
			NavGraph[] graphs = AstarPath.active.graphs;
			for (int i = 0; i < graphs.Length; i++)
			{
				NavmeshBase navmeshBase = graphs[i] as NavmeshBase;
				if (navmeshBase != null)
				{
					navmeshBase.navmeshUpdateData.RemoveClipper(obj);
				}
			}
			this.lastUpdateTime = float.NegativeInfinity;
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00027BF0 File Offset: 0x00025DF0
		internal void Update()
		{
			if (AstarPath.active.isScanning)
			{
				return;
			}
			bool flag = false;
			NavGraph[] graphs = AstarPath.active.graphs;
			for (int i = 0; i < graphs.Length; i++)
			{
				NavmeshBase navmeshBase = graphs[i] as NavmeshBase;
				if (navmeshBase != null)
				{
					navmeshBase.navmeshUpdateData.Refresh(false);
					flag = (navmeshBase.navmeshUpdateData.forcedReloadRects.Count > 0);
				}
			}
			if ((this.updateInterval >= 0f && Time.realtimeSinceStartup - this.lastUpdateTime > this.updateInterval) || flag)
			{
				this.ForceUpdate();
			}
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00027C80 File Offset: 0x00025E80
		public void ForceUpdate()
		{
			this.lastUpdateTime = Time.realtimeSinceStartup;
			List<NavmeshClipper> list = null;
			NavGraph[] graphs = AstarPath.active.graphs;
			for (int i = 0; i < graphs.Length; i++)
			{
				NavmeshBase navmeshBase = graphs[i] as NavmeshBase;
				if (navmeshBase != null)
				{
					navmeshBase.navmeshUpdateData.Refresh(false);
					TileHandler handler = navmeshBase.navmeshUpdateData.handler;
					if (handler != null)
					{
						List<IntRect> forcedReloadRects = navmeshBase.navmeshUpdateData.forcedReloadRects;
						GridLookup<NavmeshClipper>.Root allItems = handler.cuts.AllItems;
						if (forcedReloadRects.Count == 0)
						{
							bool flag = false;
							for (GridLookup<NavmeshClipper>.Root root = allItems; root != null; root = root.next)
							{
								if (root.obj.RequiresUpdate())
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								goto IL_16A;
							}
						}
						handler.StartBatchLoad();
						for (int j = 0; j < forcedReloadRects.Count; j++)
						{
							handler.ReloadInBounds(forcedReloadRects[j]);
						}
						forcedReloadRects.ClearFast<IntRect>();
						if (list == null)
						{
							list = ListPool<NavmeshClipper>.Claim();
						}
						for (GridLookup<NavmeshClipper>.Root root2 = allItems; root2 != null; root2 = root2.next)
						{
							if (root2.obj.RequiresUpdate())
							{
								handler.ReloadInBounds(root2.previousBounds);
								Rect bounds = root2.obj.GetBounds(handler.graph.transform);
								IntRect touchingTilesInGraphSpace = handler.graph.GetTouchingTilesInGraphSpace(bounds);
								handler.cuts.Move(root2.obj, touchingTilesInGraphSpace);
								handler.ReloadInBounds(touchingTilesInGraphSpace);
								list.Add(root2.obj);
							}
						}
						handler.EndBatchLoad();
					}
				}
				IL_16A:;
			}
			if (list != null)
			{
				for (int k = 0; k < list.Count; k++)
				{
					list[k].NotifyUpdated();
				}
				ListPool<NavmeshClipper>.Release(ref list);
			}
		}

		// Token: 0x040003DC RID: 988
		public float updateInterval;

		// Token: 0x040003DD RID: 989
		private float lastUpdateTime = float.NegativeInfinity;

		// Token: 0x02000152 RID: 338
		internal class NavmeshUpdateSettings
		{
			// Token: 0x06000B4E RID: 2894 RVA: 0x00047B15 File Offset: 0x00045D15
			public NavmeshUpdateSettings(NavmeshBase graph)
			{
				this.graph = graph;
			}

			// Token: 0x06000B4F RID: 2895 RVA: 0x00047B30 File Offset: 0x00045D30
			public void Refresh(bool forceCreate = false)
			{
				if (!this.graph.enableNavmeshCutting)
				{
					if (this.handler != null)
					{
						this.handler.cuts.Clear();
						this.handler.ReloadInBounds(new IntRect(int.MinValue, int.MinValue, int.MaxValue, int.MaxValue));
						AstarPath.active.FlushGraphUpdates();
						AstarPath.active.FlushWorkItems();
						this.forcedReloadRects.ClearFast<IntRect>();
						this.handler = null;
						return;
					}
				}
				else if ((this.handler == null && (forceCreate || NavmeshClipper.allEnabled.Count > 0)) || (this.handler != null && !this.handler.isValid))
				{
					this.handler = new TileHandler(this.graph);
					for (int i = 0; i < NavmeshClipper.allEnabled.Count; i++)
					{
						this.AddClipper(NavmeshClipper.allEnabled[i]);
					}
					this.handler.CreateTileTypesFromGraph();
					this.forcedReloadRects.Add(new IntRect(int.MinValue, int.MinValue, int.MaxValue, int.MaxValue));
				}
			}

			// Token: 0x06000B50 RID: 2896 RVA: 0x00047C43 File Offset: 0x00045E43
			public void OnRecalculatedTiles(NavmeshTile[] tiles)
			{
				this.Refresh(false);
				if (this.handler != null)
				{
					this.handler.OnRecalculatedTiles(tiles);
				}
			}

			// Token: 0x06000B51 RID: 2897 RVA: 0x00047C60 File Offset: 0x00045E60
			public void AddClipper(NavmeshClipper obj)
			{
				if (!obj.graphMask.Contains((int)this.graph.graphIndex))
				{
					return;
				}
				this.Refresh(true);
				if (this.handler == null)
				{
					return;
				}
				Rect bounds = obj.GetBounds(this.handler.graph.transform);
				IntRect touchingTilesInGraphSpace = this.handler.graph.GetTouchingTilesInGraphSpace(bounds);
				this.handler.cuts.Add(obj, touchingTilesInGraphSpace);
			}

			// Token: 0x06000B52 RID: 2898 RVA: 0x00047CD4 File Offset: 0x00045ED4
			public void RemoveClipper(NavmeshClipper obj)
			{
				this.Refresh(false);
				if (this.handler == null)
				{
					return;
				}
				GridLookup<NavmeshClipper>.Root root = this.handler.cuts.GetRoot(obj);
				if (root != null)
				{
					this.forcedReloadRects.Add(root.previousBounds);
					this.handler.cuts.Remove(obj);
				}
			}

			// Token: 0x040007BA RID: 1978
			public TileHandler handler;

			// Token: 0x040007BB RID: 1979
			public readonly List<IntRect> forcedReloadRects = new List<IntRect>();

			// Token: 0x040007BC RID: 1980
			private readonly NavmeshBase graph;
		}
	}
}
