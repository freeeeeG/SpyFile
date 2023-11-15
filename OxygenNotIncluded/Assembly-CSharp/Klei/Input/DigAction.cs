using System;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02000E1B RID: 3611
	[ActionType("InterfaceTool", "Dig", true)]
	public abstract class DigAction
	{
		// Token: 0x06006EAA RID: 28330 RVA: 0x002B84F4 File Offset: 0x002B66F4
		public void Uproot(int cell)
		{
			ListPool<ScenePartitionerEntry, GameScenePartitioner>.PooledList pooledList = ListPool<ScenePartitionerEntry, GameScenePartitioner>.Allocate();
			int x_bottomLeft;
			int y_bottomLeft;
			Grid.CellToXY(cell, out x_bottomLeft, out y_bottomLeft);
			GameScenePartitioner.Instance.GatherEntries(x_bottomLeft, y_bottomLeft, 1, 1, GameScenePartitioner.Instance.plants, pooledList);
			if (pooledList.Count > 0)
			{
				this.Uproot((pooledList[0].obj as Component).GetComponent<Uprootable>());
			}
			pooledList.Recycle();
		}

		// Token: 0x06006EAB RID: 28331
		public abstract void Dig(int cell, int distFromOrigin);

		// Token: 0x06006EAC RID: 28332
		protected abstract void Uproot(Uprootable uprootable);
	}
}
