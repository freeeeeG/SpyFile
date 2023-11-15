using System;
using UnityEngine;

// Token: 0x02000206 RID: 518
public class RecipeRefactor : RefactorTurret
{
	// Token: 0x06000CF5 RID: 3317 RVA: 0x00021754 File Offset: 0x0001F954
	protected override void UndoUnSwitching()
	{
		base.UndoUnSwitching();
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Challenge && TechSelectPanel.PlacingChoice)
		{
			TechSelectPanel.PlacingChoice = false;
			Singleton<ObjectPool>.Instance.UnSpawn(this);
			Singleton<GameManager>.Instance.ShowChoices(true, false, false);
			return;
		}
		this.Strategy.UndoStrategy();
	}

	// Token: 0x06000CF6 RID: 3318 RVA: 0x000217AA File Offset: 0x0001F9AA
	protected override void ContentLandedCheck(Collider2D col)
	{
		if (Singleton<LevelManager>.Instance.CurrentLevel.ModeType == ModeType.Challenge && TechSelectPanel.PlacingChoice)
		{
			Singleton<GameManager>.Instance.ConfirmChoice();
			TechSelectPanel.PlacingChoice = false;
		}
		base.ContentLandedCheck(col);
	}
}
