using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x0200009B RID: 155
public class UI_Icon_Talent : UI_Icon
{
	// Token: 0x06000563 RID: 1379 RVA: 0x0001F33C File Offset: 0x0001D53C
	[SerializeField]
	public void Init(int jobID, int talentID)
	{
		this.jobID = jobID;
		this.talentID = talentID;
		LanguageText inst = LanguageText.Inst;
		this.talent = DataBase.Inst.DataPlayerModels[jobID].talents[talentID];
		if (talentID < 8)
		{
			this.textName.text = inst.GetFactorNameAbb(this.talent.facs[1].type);
		}
		else
		{
			this.textName.text = inst.factorInfinity[this.talent.facs[1].type];
		}
		int num = GameData.inst.jobs[jobID].TalentLevels[talentID];
		int maxLevel = DataBase.Inst.DataPlayerModels[jobID].talents[talentID].maxLevel;
		this.textLevel.text = inst.talent_Level + " " + num;
		int num2 = this.talent.CostWithCurLevel(num);
		long star = GameData.inst.Star;
		if (num == maxLevel)
		{
			this.textPrice.text = "MAX";
		}
		else
		{
			this.textPrice.text = num2.ToString();
		}
		if (num >= maxLevel)
		{
			this.imagePlus.gameObject.SetActive(false);
			this.imageStar.gameObject.SetActive(true);
		}
		else
		{
			this.imagePlus.gameObject.SetActive(true);
			this.imageStar.gameObject.SetActive(false);
			if ((long)num2 <= star)
			{
				this.imagePlus.color = UI_Setting.Inst.myGreen;
			}
			else
			{
				this.imagePlus.color = Color.grey;
			}
		}
		this.Outline_Close();
		base.TextSetNormal();
	}

	// Token: 0x06000564 RID: 1380 RVA: 0x0001F4D8 File Offset: 0x0001D6D8
	public bool TalentUpgrade()
	{
		int num = GameData.inst.jobs[this.jobID].TalentLevels[this.talentID];
		int num2 = this.talent.CostWithCurLevel(num);
		float num3 = (float)this.talent.maxLevel;
		if (GameData.inst.Star < (long)num2)
		{
			UI_FloatTextControl.inst.Special_TalentLackOfStar();
			return false;
		}
		if ((float)num >= num3)
		{
			UI_FloatTextControl.inst.Special_TalentLevelMax(this.jobID, this.talentID);
			return false;
		}
		GameData.inst.UseStar(num2);
		int[] talentLevels = GameData.inst.jobs[this.jobID].TalentLevels;
		talentLevels[this.talentID]++;
		GameData.inst.jobs[this.jobID].SetTalentLevels(talentLevels);
		UI_FloatTextControl.inst.Special_TalentLevelUp(this.jobID, this.talentID);
		UI_ToolTip.inst.TryClose();
		MainCanvas.inst.Panel_NewGame_Update();
		MainCanvas.inst.Obj_Preview_UpdateAll();
		SaveFile.SaveByJson(false);
		return true;
	}

	// Token: 0x06000565 RID: 1381 RVA: 0x0001F5D9 File Offset: 0x0001D7D9
	private IEnumerator TalentUpgrade_Repeat10Times()
	{
		int num;
		for (int i = 0; i < 10; i = num + 1)
		{
			if (i != 0)
			{
				SoundEffects.Inst.ui_ButtonClick.PlayRandom();
			}
			if (!this.TalentUpgrade())
			{
				break;
			}
			yield return new WaitForSeconds(0.06f);
			num = i;
		}
		yield break;
	}

	// Token: 0x06000566 RID: 1382 RVA: 0x0001F5E8 File Offset: 0x0001D7E8
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (MyInput.KeyShiftHold())
		{
			MainCanvas.inst.StartCoroutine(this.TalentUpgrade_Repeat10Times());
			return;
		}
		this.TalentUpgrade();
	}

	// Token: 0x06000567 RID: 1383 RVA: 0x0001F60A File Offset: 0x0001D80A
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithTalent(this);
		base.TextSetHighlight();
		this.Outline_Show_Above();
	}

	// Token: 0x06000568 RID: 1384 RVA: 0x0001F623 File Offset: 0x0001D823
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		base.TextSetNormal();
		this.Outline_Close();
	}

	// Token: 0x06000569 RID: 1385 RVA: 0x0001F63B File Offset: 0x0001D83B
	public void SetBold(bool flag)
	{
		this.textName.fontStyle = (flag ? FontStyle.Bold : FontStyle.Normal);
		this.textLevel.fontStyle = (flag ? FontStyle.Bold : FontStyle.Normal);
	}

	// Token: 0x0600056A RID: 1386 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x04000464 RID: 1124
	public int jobID = -1;

	// Token: 0x04000465 RID: 1125
	public int talentID = -1;

	// Token: 0x04000466 RID: 1126
	public Text textName;

	// Token: 0x04000467 RID: 1127
	public Text textLevel;

	// Token: 0x04000468 RID: 1128
	[SerializeField]
	private Text textPrice;

	// Token: 0x04000469 RID: 1129
	[SerializeField]
	private Image imagePlus;

	// Token: 0x0400046A RID: 1130
	[SerializeField]
	private Image imageStar;

	// Token: 0x0400046B RID: 1131
	private Talent talent;
}
