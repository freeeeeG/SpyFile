using System;
using UnityEngine;

// Token: 0x02000A2C RID: 2604
public class WaterTrapGuide : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06004E03 RID: 19971 RVA: 0x001B57BD File Offset: 0x001B39BD
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.parentController = this.parent.GetComponent<KBatchedAnimController>();
		this.guideController = base.GetComponent<KBatchedAnimController>();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06004E04 RID: 19972 RVA: 0x001B57EE File Offset: 0x001B39EE
	private void RefreshTint()
	{
		this.guideController.TintColour = this.parentController.TintColour;
	}

	// Token: 0x06004E05 RID: 19973 RVA: 0x001B5806 File Offset: 0x001B3A06
	public void RefreshPosition()
	{
		if (this.guideController != null && this.guideController.IsMoving)
		{
			this.guideController.SetDirty();
		}
	}

	// Token: 0x06004E06 RID: 19974 RVA: 0x001B5830 File Offset: 0x001B3A30
	private void RefreshDepthAvailable()
	{
		int depthAvailable = WaterTrapGuide.GetDepthAvailable(Grid.PosToCell(this), this.parent);
		if (depthAvailable != this.previousDepthAvailable)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			if (depthAvailable == 0)
			{
				component.enabled = false;
			}
			else
			{
				component.enabled = true;
				component.Play(new HashedString("place_pipe" + depthAvailable.ToString()), KAnim.PlayMode.Once, 1f, 0f);
			}
			if (this.occupyTiles)
			{
				WaterTrapGuide.OccupyArea(this.parent, depthAvailable);
			}
			this.previousDepthAvailable = depthAvailable;
		}
	}

	// Token: 0x06004E07 RID: 19975 RVA: 0x001B58B4 File Offset: 0x001B3AB4
	public void RenderEveryTick(float dt)
	{
		this.RefreshPosition();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06004E08 RID: 19976 RVA: 0x001B58C8 File Offset: 0x001B3AC8
	public static void OccupyArea(GameObject go, int depth_available)
	{
		int cell = Grid.PosToCell(go.transform.GetPosition());
		for (int i = 1; i <= 4; i++)
		{
			int key = Grid.OffsetCell(cell, 0, -i);
			if (i <= depth_available)
			{
				Grid.ObjectLayers[1][key] = go;
			}
			else if (Grid.ObjectLayers[1].ContainsKey(key) && Grid.ObjectLayers[1][key] == go)
			{
				Grid.ObjectLayers[1][key] = null;
			}
		}
	}

	// Token: 0x06004E09 RID: 19977 RVA: 0x001B5948 File Offset: 0x001B3B48
	public static int GetDepthAvailable(int root_cell, GameObject pump)
	{
		int num = 4;
		int result = 0;
		for (int i = 1; i <= num; i++)
		{
			int num2 = Grid.OffsetCell(root_cell, 0, -i);
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || (Grid.ObjectLayers[1].ContainsKey(num2) && !(Grid.ObjectLayers[1][num2] == null) && !(Grid.ObjectLayers[1][num2] == pump)))
			{
				break;
			}
			result = i;
		}
		return result;
	}

	// Token: 0x040032E2 RID: 13026
	private int previousDepthAvailable = -1;

	// Token: 0x040032E3 RID: 13027
	public GameObject parent;

	// Token: 0x040032E4 RID: 13028
	public bool occupyTiles;

	// Token: 0x040032E5 RID: 13029
	private KBatchedAnimController parentController;

	// Token: 0x040032E6 RID: 13030
	private KBatchedAnimController guideController;
}
