using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02000C30 RID: 3120
public class MonumentSideScreen : SideScreenContent
{
	// Token: 0x060062C0 RID: 25280 RVA: 0x00247724 File Offset: 0x00245924
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MonumentPart>() != null;
	}

	// Token: 0x060062C1 RID: 25281 RVA: 0x00247734 File Offset: 0x00245934
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.debugVictoryButton.onClick += delegate()
		{
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Thriving.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
			GameScheduler.Instance.Schedule("ForceCheckAchievements", 0.1f, delegate(object data)
			{
				Game.Instance.Trigger(395452326, null);
			}, null, null);
		};
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPartResource.Part.Top);
		this.flipButton.onClick += delegate()
		{
			this.target.GetComponent<Rotatable>().Rotate();
		};
	}

	// Token: 0x060062C2 RID: 25282 RVA: 0x002477B0 File Offset: 0x002459B0
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target = target.GetComponent<MonumentPart>();
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPartResource.Part.Top);
		this.GenerateStateButtons();
	}

	// Token: 0x060062C3 RID: 25283 RVA: 0x00247800 File Offset: 0x00245A00
	public void GenerateStateButtons()
	{
		for (int i = this.buttons.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.buttons[i]);
		}
		this.buttons.Clear();
		using (List<MonumentPartResource>.Enumerator enumerator = Db.GetMonumentParts().GetParts(this.target.part).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MonumentPartResource state = enumerator.Current;
				GameObject gameObject = Util.KInstantiateUI(this.stateButtonPrefab, this.buttonContainer.gameObject, true);
				string state2 = state.State;
				string symbolName = state.SymbolName;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.target.SetState(state.Id);
				};
				this.buttons.Add(gameObject);
				gameObject.GetComponent<KButton>().fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(state.AnimFile, state2, false, symbolName);
			}
		}
	}

	// Token: 0x04004350 RID: 17232
	private MonumentPart target;

	// Token: 0x04004351 RID: 17233
	public KButton debugVictoryButton;

	// Token: 0x04004352 RID: 17234
	public KButton flipButton;

	// Token: 0x04004353 RID: 17235
	public GameObject stateButtonPrefab;

	// Token: 0x04004354 RID: 17236
	private List<GameObject> buttons = new List<GameObject>();

	// Token: 0x04004355 RID: 17237
	[SerializeField]
	private RectTransform buttonContainer;
}
