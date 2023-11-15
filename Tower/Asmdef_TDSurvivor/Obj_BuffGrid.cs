using System;
using System.Collections;
using UnityEngine;

// Token: 0x020000C1 RID: 193
[SelectionBase]
public class Obj_BuffGrid : APowerGrid
{
	// Token: 0x06000472 RID: 1138 RVA: 0x00011FF5 File Offset: 0x000101F5
	public override void OnTetrisPlaced(Obj_TetrisBlock tetris)
	{
		base.OnTetrisPlaced(tetris);
	}

	// Token: 0x06000473 RID: 1139 RVA: 0x00011FFE File Offset: 0x000101FE
	protected override void OnTetrisRemoved(Obj_TetrisBlock tetris)
	{
		base.OnTetrisRemoved(tetris);
		base.transform.position = base.transform.position.WithY(0.05f);
	}

	// Token: 0x06000474 RID: 1140 RVA: 0x00012027 File Offset: 0x00010227
	protected override IEnumerator CR_PlaceTetrisProc(Obj_TetrisBlock tetris)
	{
		yield return new WaitForSeconds(0.33f);
		this.particle_TetrisPlaced.Play();
		this.animator.SetBool("isOn", false);
		SoundManager.PlaySound("Block", "BuffGrid_TetrisOn", -1f, -1f, -1f);
		yield return new WaitForSeconds(0.1f);
		base.transform.position = base.transform.position.WithY(1f);
		this.animator.SetBool("isOn", true);
		yield break;
	}

	// Token: 0x06000475 RID: 1141 RVA: 0x00012038 File Offset: 0x00010238
	protected override void ApplyEffectToTower(ABaseTower tower)
	{
		this.particle_TowerPlaced.Play();
		SoundManager.PlaySound("Block", "BuffGrid_TowerOn", -1f, -1f, -1f);
		if (tower.SettingData.TowerSizeType == eTowerSizeType._2x2)
		{
			TowerStats towerStats = new TowerStats(this.buffStat);
			towerStats.OverrideByMultiplier(0.25f);
			tower.SettingData.AddBuffMultiplier(towerStats);
			return;
		}
		tower.SettingData.AddBuffMultiplier(this.buffStat);
	}

	// Token: 0x06000476 RID: 1142 RVA: 0x000120B2 File Offset: 0x000102B2
	protected override void RemoveEffectFromTower(ABaseTower tower)
	{
		tower.SettingData.RemoveBuffMultiplier(this.buffStat);
	}

	// Token: 0x06000477 RID: 1143 RVA: 0x000120C8 File Offset: 0x000102C8
	public override string GetLocStatsString()
	{
		return LocalizationManager.Instance.GetString("PowerGrid", base.SettingData.PowerGridType.ToString() + "_EFFECT", new object[]
		{
			this.buffStat.GetSingleModifierValueLocString(true)
		});
	}

	// Token: 0x04000456 RID: 1110
	[SerializeField]
	private ParticleSystem particle_TetrisPlaced;

	// Token: 0x04000457 RID: 1111
	[SerializeField]
	private ParticleSystem particle_TowerPlaced;

	// Token: 0x04000458 RID: 1112
	[SerializeField]
	private TowerStats buffStat;
}
