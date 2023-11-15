using System;
using System.Collections.Generic;
using Pathfinding.Util;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x02000082 RID: 130
	public abstract class NavmeshClipper : VersionedMonoBehaviour
	{
		// Token: 0x0600067B RID: 1659 RVA: 0x00026F46 File Offset: 0x00025146
		public static void AddEnableCallback(Action<NavmeshClipper> onEnable, Action<NavmeshClipper> onDisable)
		{
			NavmeshClipper.OnEnableCallback = (Action<NavmeshClipper>)Delegate.Combine(NavmeshClipper.OnEnableCallback, onEnable);
			NavmeshClipper.OnDisableCallback = (Action<NavmeshClipper>)Delegate.Combine(NavmeshClipper.OnDisableCallback, onDisable);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00026F72 File Offset: 0x00025172
		public static void RemoveEnableCallback(Action<NavmeshClipper> onEnable, Action<NavmeshClipper> onDisable)
		{
			NavmeshClipper.OnEnableCallback = (Action<NavmeshClipper>)Delegate.Remove(NavmeshClipper.OnEnableCallback, onEnable);
			NavmeshClipper.OnDisableCallback = (Action<NavmeshClipper>)Delegate.Remove(NavmeshClipper.OnDisableCallback, onDisable);
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600067D RID: 1661 RVA: 0x00026F9E File Offset: 0x0002519E
		public static List<NavmeshClipper> allEnabled
		{
			get
			{
				return NavmeshClipper.all;
			}
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00026FA5 File Offset: 0x000251A5
		protected virtual void OnEnable()
		{
			if (NavmeshClipper.OnEnableCallback != null)
			{
				NavmeshClipper.OnEnableCallback(this);
			}
			this.listIndex = NavmeshClipper.all.Count;
			NavmeshClipper.all.Add(this);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00026FD4 File Offset: 0x000251D4
		protected virtual void OnDisable()
		{
			NavmeshClipper.all[this.listIndex] = NavmeshClipper.all[NavmeshClipper.all.Count - 1];
			NavmeshClipper.all[this.listIndex].listIndex = this.listIndex;
			NavmeshClipper.all.RemoveAt(NavmeshClipper.all.Count - 1);
			this.listIndex = -1;
			if (NavmeshClipper.OnDisableCallback != null)
			{
				NavmeshClipper.OnDisableCallback(this);
			}
		}

		// Token: 0x06000680 RID: 1664
		internal abstract void NotifyUpdated();

		// Token: 0x06000681 RID: 1665
		public abstract Rect GetBounds(GraphTransform transform);

		// Token: 0x06000682 RID: 1666
		public abstract bool RequiresUpdate();

		// Token: 0x06000683 RID: 1667
		public abstract void ForceUpdate();

		// Token: 0x040003C2 RID: 962
		private static Action<NavmeshClipper> OnEnableCallback;

		// Token: 0x040003C3 RID: 963
		private static Action<NavmeshClipper> OnDisableCallback;

		// Token: 0x040003C4 RID: 964
		private static readonly List<NavmeshClipper> all = new List<NavmeshClipper>();

		// Token: 0x040003C5 RID: 965
		private int listIndex = -1;

		// Token: 0x040003C6 RID: 966
		public GraphMask graphMask = GraphMask.everything;
	}
}
