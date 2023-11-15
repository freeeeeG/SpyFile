using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000091 RID: 145
public class UI_Icon_Proficiency : UI_Icon
{
	// Token: 0x06000510 RID: 1296 RVA: 0x0001DBFC File Offset: 0x0001BDFC
	[SerializeField]
	public void Init(int jobID)
	{
		this.jobID = jobID;
		LanguageText inst = LanguageText.Inst;
		Mastery mastery = GameData.inst.jobs[jobID].mastery;
		this.textName.text = inst.factorAbbOthers[2];
		this.textLevel.text = inst.talent_Level + " " + mastery.GetRank().ToString();
		this.textProcess.text = mastery.GetString_Progress();
		base.TextSetNormal();
		this.imageStar.gameObject.SetActive(mastery.GetRank() == 10);
	}

	// Token: 0x06000511 RID: 1297 RVA: 0x000051D0 File Offset: 0x000033D0
	public override void OnPointerClick(PointerEventData eventData)
	{
	}

	// Token: 0x06000512 RID: 1298 RVA: 0x0001DC95 File Offset: 0x0001BE95
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithStringAndPlace(UI_ToolTipInfo.GetInfo_Proficiency(TempData.inst.jobId), 1);
		base.TextSetHighlight();
	}

	// Token: 0x06000513 RID: 1299 RVA: 0x0001C672 File Offset: 0x0001A872
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		base.TextSetNormal();
	}

	// Token: 0x06000514 RID: 1300 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x04000430 RID: 1072
	public int jobID = -1;

	// Token: 0x04000431 RID: 1073
	public Text textName;

	// Token: 0x04000432 RID: 1074
	public Text textLevel;

	// Token: 0x04000433 RID: 1075
	public Text textProcess;

	// Token: 0x04000434 RID: 1076
	[SerializeField]
	private Image imageStar;
}
