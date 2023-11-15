using System;
using UnityEngine;

// Token: 0x02000202 RID: 514
public class NonRecipeRefactor : RefactorTurret
{
	// Token: 0x17000499 RID: 1177
	// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x000213BB File Offset: 0x0001F5BB
	public override bool CanEquip
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x000213BE File Offset: 0x0001F5BE
	public override void ContentLanded()
	{
		base.ContentLanded();
		base.m_GameTile.tag = StaticData.UndropablePoint;
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x000213D8 File Offset: 0x0001F5D8
	protected override void UndoUnSwitching()
	{
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Challenge && TechSelectPanel.PlacingChoice)
		{
			TechSelectPanel.PlacingChoice = false;
			Singleton<ObjectPool>.Instance.UnSpawn(this);
			Singleton<GameManager>.Instance.ShowChoices(true, false, false);
		}
		else
		{
			Singleton<GameManager>.Instance.ShowTechSelect(true, false);
			Singleton<GameManager>.Instance.RemoveTech(TechSelectPanel.SelectingTech);
			TechSelectPanel.SelectingTech = null;
		}
		base.UndoUnSwitching();
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x00021445 File Offset: 0x0001F645
	protected override void ContentLandedCheck(Collider2D col)
	{
		if (!base.IsSwitching)
		{
			Singleton<GameManager>.Instance.ConfirmTechSelect();
		}
		base.ContentLandedCheck(col);
	}
}
