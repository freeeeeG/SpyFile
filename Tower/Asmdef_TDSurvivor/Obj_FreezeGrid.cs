using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000C5 RID: 197
[SelectionBase]
public class Obj_FreezeGrid : APowerGrid
{
	// Token: 0x0600048D RID: 1165 RVA: 0x00012641 File Offset: 0x00010841
	protected override IEnumerator CR_PlaceTetrisProc(Obj_TetrisBlock tetris)
	{
		yield return new WaitForSeconds(0.33f);
		if (tetris.IsFrozen())
		{
			yield break;
		}
		tetris.FreezeBlock();
		yield break;
	}

	// Token: 0x0600048E RID: 1166 RVA: 0x00012650 File Offset: 0x00010850
	public override string GetLocStatsString()
	{
		return LocalizationManager.Instance.GetString("PowerGrid", base.SettingData.PowerGridType.ToString() + "_EFFECT", Array.Empty<object>());
	}

	// Token: 0x0400046D RID: 1133
	[SerializeField]
	private ParticleSystem particle_TetrisPlaced;
}
