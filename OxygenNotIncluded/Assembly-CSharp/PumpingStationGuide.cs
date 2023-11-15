using System;
using UnityEngine;

// Token: 0x020004F0 RID: 1264
[AddComponentMenu("KMonoBehaviour/scripts/PumpingStationGuide")]
public class PumpingStationGuide : KMonoBehaviour, IRenderEveryTick
{
	// Token: 0x06001DA3 RID: 7587 RVA: 0x0009DAC4 File Offset: 0x0009BCC4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.parentController = this.parent.GetComponent<KBatchedAnimController>();
		this.guideController = base.GetComponent<KBatchedAnimController>();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06001DA4 RID: 7588 RVA: 0x0009DAF5 File Offset: 0x0009BCF5
	public void RefreshPosition()
	{
		if (this.guideController != null && this.guideController.IsMoving)
		{
			this.guideController.SetDirty();
		}
	}

	// Token: 0x06001DA5 RID: 7589 RVA: 0x0009DB1D File Offset: 0x0009BD1D
	private void RefreshTint()
	{
		this.guideController.TintColour = this.parentController.TintColour;
	}

	// Token: 0x06001DA6 RID: 7590 RVA: 0x0009DB38 File Offset: 0x0009BD38
	private void RefreshDepthAvailable()
	{
		int depthAvailable = PumpingStationGuide.GetDepthAvailable(Grid.PosToCell(this), this.parent);
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
				PumpingStationGuide.OccupyArea(this.parent, depthAvailable);
			}
			this.previousDepthAvailable = depthAvailable;
		}
	}

	// Token: 0x06001DA7 RID: 7591 RVA: 0x0009DBBC File Offset: 0x0009BDBC
	public void RenderEveryTick(float dt)
	{
		this.RefreshPosition();
		this.RefreshTint();
		this.RefreshDepthAvailable();
	}

	// Token: 0x06001DA8 RID: 7592 RVA: 0x0009DBD0 File Offset: 0x0009BDD0
	public static void OccupyArea(GameObject go, int depth_available)
	{
		int cell = Grid.PosToCell(go.transform.GetPosition());
		for (int i = 1; i <= 4; i++)
		{
			int key = Grid.OffsetCell(cell, 0, -i);
			int key2 = Grid.OffsetCell(cell, 1, -i);
			if (i <= depth_available)
			{
				Grid.ObjectLayers[1][key] = go;
				Grid.ObjectLayers[1][key2] = go;
			}
			else
			{
				if (Grid.ObjectLayers[1].ContainsKey(key) && Grid.ObjectLayers[1][key] == go)
				{
					Grid.ObjectLayers[1][key] = null;
				}
				if (Grid.ObjectLayers[1].ContainsKey(key2) && Grid.ObjectLayers[1][key2] == go)
				{
					Grid.ObjectLayers[1][key2] = null;
				}
			}
		}
	}

	// Token: 0x06001DA9 RID: 7593 RVA: 0x0009DCA0 File Offset: 0x0009BEA0
	public static int GetDepthAvailable(int root_cell, GameObject pump)
	{
		int num = 4;
		int result = 0;
		for (int i = 1; i <= num; i++)
		{
			int num2 = Grid.OffsetCell(root_cell, 0, -i);
			int num3 = Grid.OffsetCell(root_cell, 1, -i);
			if (!Grid.IsValidCell(num2) || Grid.Solid[num2] || !Grid.IsValidCell(num3) || Grid.Solid[num3] || (Grid.ObjectLayers[1].ContainsKey(num2) && !(Grid.ObjectLayers[1][num2] == null) && !(Grid.ObjectLayers[1][num2] == pump)) || (Grid.ObjectLayers[1].ContainsKey(num3) && !(Grid.ObjectLayers[1][num3] == null) && !(Grid.ObjectLayers[1][num3] == pump)))
			{
				break;
			}
			result = i;
		}
		return result;
	}

	// Token: 0x04001077 RID: 4215
	private int previousDepthAvailable = -1;

	// Token: 0x04001078 RID: 4216
	public GameObject parent;

	// Token: 0x04001079 RID: 4217
	public bool occupyTiles;

	// Token: 0x0400107A RID: 4218
	private KBatchedAnimController parentController;

	// Token: 0x0400107B RID: 4219
	private KBatchedAnimController guideController;
}
