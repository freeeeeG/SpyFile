using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C9 RID: 201
public class UI_Panel_Main_SkillModule : UI_Panel_Main_IconList
{
	// Token: 0x060006E8 RID: 1768 RVA: 0x00026A40 File Offset: 0x00024C40
	public override void InitIcons(Transform transformParent = null)
	{
		if (TempData.inst.currentSceneType == EnumSceneType.MAINMENU)
		{
			this.text_Title.text = LanguageText.Inst.skillModule.skillModule_Title;
		}
		if (this.IconNum() == 0)
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		base.InitIcons(null);
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x00026A9C File Offset: 0x00024C9C
	protected override int IconNum()
	{
		int jobId = TempData.inst.jobId;
		return DataBase.Inst.DataPlayerModels[jobId].skillModules.Length;
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x00026AC8 File Offset: 0x00024CC8
	protected override bool IfAvailable(int ID)
	{
		EnumSceneType currentSceneType = TempData.inst.currentSceneType;
		if (currentSceneType == EnumSceneType.MAINMENU)
		{
			return true;
		}
		if (currentSceneType != EnumSceneType.BATTLE)
		{
			return true;
		}
		SkillModule skillModule = DataBase.Inst.DataPlayerModels[TempData.inst.jobId].skillModules[ID];
		if (TempData.inst.skillModuleFlags.Length <= skillModule.effectID)
		{
			Debug.LogError("Error_SkillModuleFlagOut!");
			return false;
		}
		return TempData.inst.skillModuleFlags[TempData.inst.jobId][skillModule.effectID];
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x00026B46 File Offset: 0x00024D46
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Icon_SkillModule>().Init(ID);
	}

	// Token: 0x040005BF RID: 1471
	[SerializeField]
	private Text text_Title;
}
