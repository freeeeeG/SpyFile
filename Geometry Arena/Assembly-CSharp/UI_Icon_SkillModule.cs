using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000099 RID: 153
public class UI_Icon_SkillModule : UI_Icon
{
	// Token: 0x170000E0 RID: 224
	// (get) Token: 0x06000557 RID: 1367 RVA: 0x0001F069 File Offset: 0x0001D269
	private SkillModule TheSkillModule
	{
		get
		{
			return DataBase.Inst.DataPlayerModels[this.jobID].skillModules[this.skillModuleID];
		}
	}

	// Token: 0x170000E1 RID: 225
	// (get) Token: 0x06000558 RID: 1368 RVA: 0x0001F088 File Offset: 0x0001D288
	// (set) Token: 0x06000559 RID: 1369 RVA: 0x0001F0B8 File Offset: 0x0001D2B8
	public bool OpenFlag
	{
		get
		{
			int effectID = this.TheSkillModule.effectID;
			return TempData.inst.skillModuleFlags[TempData.inst.jobId][effectID];
		}
		set
		{
			int effectID = this.TheSkillModule.effectID;
			TempData.inst.skillModuleFlags[TempData.inst.jobId][effectID] = value;
		}
	}

	// Token: 0x0600055A RID: 1370 RVA: 0x0001F0EC File Offset: 0x0001D2EC
	public void Init(int ID)
	{
		this.jobID = TempData.inst.jobId;
		this.skillModuleID = ID;
		Sprite spriteWithId = ResourceLibrary.Inst.SpLists_Icon_SkillModules[this.jobID].GetSpriteWithId(this.skillModuleID);
		Color white = Color.white;
		this.imageIcon.sprite = spriteWithId;
		this.imageIcon.color = white;
		this.UpdateOutline();
		base.UpdateLockIcon();
	}

	// Token: 0x0600055B RID: 1371 RVA: 0x0001F158 File Offset: 0x0001D358
	private void UpdateOutline()
	{
		EnumSceneType currentSceneType = TempData.inst.currentSceneType;
		if (currentSceneType != EnumSceneType.MAINMENU)
		{
			if (currentSceneType != EnumSceneType.BATTLE)
			{
				return;
			}
			this.OutlineNew_Close();
			return;
		}
		else
		{
			if (this.OpenFlag)
			{
				this.OutlineNew_Show_Selected();
				return;
			}
			this.OutlineNew_Close();
			return;
		}
	}

	// Token: 0x0600055C RID: 1372 RVA: 0x0001F194 File Offset: 0x0001D394
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (TempData.inst.currentSceneType != EnumSceneType.MAINMENU)
		{
			return;
		}
		if (!this.ifUnlocked())
		{
			return;
		}
		this.OpenFlag = !this.OpenFlag;
		UI_FloatTextControl.inst.Special_SkillModule(this.TheSkillModule, this.OpenFlag);
		UI_ToolTip.inst.TryClose();
		MainCanvas.inst.Panel_NewGame_Update();
		MainCanvas.inst.Obj_Preview_UpdateColor();
		if (this.jobID == 2 && this.TheSkillModule.effectID == 4)
		{
			MainCanvas.inst.Obj_Preview_UpdateAll();
		}
		if (this.jobID == 9 && this.TheSkillModule.effectID == 1)
		{
			MainCanvas.inst.Obj_Preview_UpdateAll();
		}
		this.UpdateOutline();
	}

	// Token: 0x0600055D RID: 1373 RVA: 0x0001F243 File Offset: 0x0001D443
	public override void OnPointerEnter(PointerEventData eventData)
	{
		UI_ToolTip.inst.ShowWithString(UI_ToolTipInfo.GetInfo_SkillModule(this.jobID, this.skillModuleID, this.ifUnlocked(), this.OpenFlag));
		if (!this.OpenFlag)
		{
			this.OutlineNew_Show_Above();
		}
	}

	// Token: 0x0600055E RID: 1374 RVA: 0x0001F27A File Offset: 0x0001D47A
	public override void OnPointerExit(PointerEventData eventData)
	{
		UI_ToolTip.inst.Close();
		if (!this.OpenFlag)
		{
			this.OutlineNew_Close();
		}
	}

	// Token: 0x0600055F RID: 1375 RVA: 0x0001F294 File Offset: 0x0001D494
	protected override bool ifUnlocked()
	{
		int num = 0;
		Skill.SkillCurrent(ref num);
		int num2 = (TempData.inst.jobId <= 8) ? 3 : 1;
		if (num < num2)
		{
			return false;
		}
		int levelNeed = this.TheSkillModule.levelNeed;
		return GameData.inst.jobs[this.jobID].mastery.GetRank() >= levelNeed;
	}

	// Token: 0x0400045E RID: 1118
	[SerializeField]
	private int jobID;

	// Token: 0x0400045F RID: 1119
	public int skillModuleID;

	// Token: 0x04000460 RID: 1120
	public Image imageIcon;
}
