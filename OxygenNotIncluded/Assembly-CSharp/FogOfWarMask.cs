using System;
using UnityEngine;

// Token: 0x020007C9 RID: 1993
[AddComponentMenu("KMonoBehaviour/scripts/FogOfWarMask")]
public class FogOfWarMask : KMonoBehaviour
{
	// Token: 0x0600375D RID: 14173 RVA: 0x0012BAFD File Offset: 0x00129CFD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Grid.OnReveal = (Action<int>)Delegate.Combine(Grid.OnReveal, new Action<int>(this.OnReveal));
	}

	// Token: 0x0600375E RID: 14174 RVA: 0x0012BB25 File Offset: 0x00129D25
	private void OnReveal(int cell)
	{
		if (Grid.PosToCell(this) == cell)
		{
			Grid.OnReveal = (Action<int>)Delegate.Remove(Grid.OnReveal, new Action<int>(this.OnReveal));
			base.gameObject.DeleteObject();
		}
	}

	// Token: 0x0600375F RID: 14175 RVA: 0x0012BB5C File Offset: 0x00129D5C
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		GameUtil.FloodCollectCells(Grid.PosToCell(this), delegate(int cell)
		{
			Grid.Visible[cell] = 0;
			Grid.PreventFogOfWarReveal[cell] = true;
			return !Grid.Solid[cell];
		}, 300, null, true);
		GameUtil.FloodCollectCells(Grid.PosToCell(this), delegate(int cell)
		{
			bool flag = Grid.PreventFogOfWarReveal[cell];
			if (Grid.Solid[cell] && Grid.Foundation[cell])
			{
				Grid.PreventFogOfWarReveal[cell] = true;
				Grid.Visible[cell] = 0;
				GameObject gameObject = Grid.Objects[cell, 1];
				if (gameObject != null && gameObject.GetComponent<KPrefabID>().PrefabTag.ToString() == "POIBunkerExteriorDoor")
				{
					Grid.PreventFogOfWarReveal[cell] = false;
					Grid.Visible[cell] = byte.MaxValue;
				}
			}
			return flag || Grid.Foundation[cell];
		}, 300, null, true);
	}

	// Token: 0x06003760 RID: 14176 RVA: 0x0012BBD3 File Offset: 0x00129DD3
	public static void ClearMask(int cell)
	{
		if (Grid.PreventFogOfWarReveal[cell])
		{
			GameUtil.FloodCollectCells(cell, new Func<int, bool>(FogOfWarMask.RevealFogOfWarMask), 300, null, true);
		}
	}

	// Token: 0x06003761 RID: 14177 RVA: 0x0012BBFC File Offset: 0x00129DFC
	public static bool RevealFogOfWarMask(int cell)
	{
		bool flag = Grid.PreventFogOfWarReveal[cell];
		if (flag)
		{
			Grid.PreventFogOfWarReveal[cell] = false;
			Grid.Reveal(cell, byte.MaxValue, false);
		}
		return flag;
	}
}
