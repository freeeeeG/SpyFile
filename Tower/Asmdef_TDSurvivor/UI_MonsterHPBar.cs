using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000171 RID: 369
public class UI_MonsterHPBar : MonoBehaviour
{
	// Token: 0x170000AB RID: 171
	// (get) Token: 0x060009C3 RID: 2499 RVA: 0x00024D13 File Offset: 0x00022F13
	public float Width
	{
		get
		{
			return this.width;
		}
	}

	// Token: 0x060009C4 RID: 2500 RVA: 0x00024D1C File Offset: 0x00022F1C
	public void AttachUI(AMonsterBase target)
	{
		this.targetMonster = target;
		AMonsterBase amonsterBase = this.targetMonster;
		amonsterBase.OnMonsterDespawn = (Action<AMonsterBase>)Delegate.Combine(amonsterBase.OnMonsterDespawn, new Action<AMonsterBase>(this.OnMonsterDisable));
		AMonsterBase amonsterBase2 = this.targetMonster;
		amonsterBase2.OnMonsterKilled = (Action<AMonsterBase>)Delegate.Combine(amonsterBase2.OnMonsterKilled, new Action<AMonsterBase>(this.OnMonsterDisable));
		AMonsterBase amonsterBase3 = this.targetMonster;
		amonsterBase3.OnMonsterDamageDebuffChange = (Action)Delegate.Combine(amonsterBase3.OnMonsterDamageDebuffChange, new Action(this.OnMonsterDamageDebuffChange));
		this.UpdateBarColor();
	}

	// Token: 0x060009C5 RID: 2501 RVA: 0x00024DAB File Offset: 0x00022FAB
	public void DetachUI()
	{
		this.targetMonster = null;
		Singleton<PrefabManager>.Instance.DespawnPrefab(base.gameObject, 0f);
	}

	// Token: 0x060009C6 RID: 2502 RVA: 0x00024DCC File Offset: 0x00022FCC
	private void OnMonsterDisable(AMonsterBase monster)
	{
		AMonsterBase amonsterBase = this.targetMonster;
		amonsterBase.OnMonsterDespawn = (Action<AMonsterBase>)Delegate.Remove(amonsterBase.OnMonsterDespawn, new Action<AMonsterBase>(this.OnMonsterDisable));
		AMonsterBase amonsterBase2 = this.targetMonster;
		amonsterBase2.OnMonsterKilled = (Action<AMonsterBase>)Delegate.Remove(amonsterBase2.OnMonsterKilled, new Action<AMonsterBase>(this.OnMonsterDisable));
		AMonsterBase amonsterBase3 = this.targetMonster;
		amonsterBase3.OnMonsterDamageDebuffChange = (Action)Delegate.Remove(amonsterBase3.OnMonsterDamageDebuffChange, new Action(this.OnMonsterDamageDebuffChange));
		this.DetachUI();
	}

	// Token: 0x060009C7 RID: 2503 RVA: 0x00024E54 File Offset: 0x00023054
	private void OnMonsterDamageDebuffChange()
	{
		this.UpdateBarColor();
	}

	// Token: 0x060009C8 RID: 2504 RVA: 0x00024E5C File Offset: 0x0002305C
	private void UpdateBarColor()
	{
		if (this.targetMonster.IsPoisoned())
		{
			this.image_Bar.color = this.color_Poison;
			return;
		}
		if (this.targetMonster.IsBurning())
		{
			this.image_Bar.color = this.color_Burning;
			return;
		}
		if (this.targetMonster.IsFrozen())
		{
			this.image_Bar.color = this.color_Freeze;
			return;
		}
		this.image_Bar.color = this.color_Normal;
	}

	// Token: 0x060009C9 RID: 2505 RVA: 0x00024ED8 File Offset: 0x000230D8
	private void Update()
	{
		if (this.targetMonster != null && this.targetMonster.IsAttackable())
		{
			this.monsterHPPercentage = this.targetMonster.GetHPPercentage();
			if (this.monsterHPPercentage >= 1f)
			{
				this.canvasGroup.alpha = 0f;
				return;
			}
			this.canvasGroup.alpha = 1f;
			this.image_Bar.fillAmount = this.targetMonster.GetHPPercentage();
			base.transform.position = Singleton<CameraManager>.Instance.Calculate2DPosFrom3DPos(this.targetMonster.HeadWorldPosition);
			base.transform.localPosition += this.localOffset;
		}
	}

	// Token: 0x0400079B RID: 1947
	[SerializeField]
	private CanvasGroup canvasGroup;

	// Token: 0x0400079C RID: 1948
	[SerializeField]
	private Image image_Bar;

	// Token: 0x0400079D RID: 1949
	[SerializeField]
	private Vector3 localOffset;

	// Token: 0x0400079E RID: 1950
	[SerializeField]
	private Color color_Normal;

	// Token: 0x0400079F RID: 1951
	[SerializeField]
	private Color color_Poison;

	// Token: 0x040007A0 RID: 1952
	[SerializeField]
	private Color color_Burning;

	// Token: 0x040007A1 RID: 1953
	[SerializeField]
	private Color color_Freeze;

	// Token: 0x040007A2 RID: 1954
	[SerializeField]
	private float width = 1f;

	// Token: 0x040007A3 RID: 1955
	private AMonsterBase targetMonster;

	// Token: 0x040007A4 RID: 1956
	private float monsterHPPercentage;
}
