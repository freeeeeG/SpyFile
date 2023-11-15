using System;
using UnityEngine;

// Token: 0x020001EB RID: 491
public class BuildingContent : RefactorTurret
{
	// Token: 0x17000478 RID: 1144
	// (get) Token: 0x06000C83 RID: 3203 RVA: 0x000209D0 File Offset: 0x0001EBD0
	public override bool CanEquip
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x000209D3 File Offset: 0x0001EBD3
	public override void ContentLanded()
	{
		base.ContentLanded();
		base.m_GameTile.tag = StaticData.UndropablePoint;
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x000209EB File Offset: 0x0001EBEB
	protected override void RotateTowards()
	{
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x000209F0 File Offset: 0x0001EBF0
	protected override void ContentLandedCheck(Collider2D col)
	{
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Challenge && TechSelectPanel.PlacingChoice)
		{
			Singleton<GameManager>.Instance.ConfirmChoice();
			TechSelectPanel.PlacingChoice = false;
		}
		else if (!base.IsSwitching)
		{
			Singleton<GameManager>.Instance.ConfirmTechSelect();
		}
		base.ContentLandedCheck(col);
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x00020A44 File Offset: 0x0001EC44
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
}
