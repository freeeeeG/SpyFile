using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000090 RID: 144
public class UI_Icon_ModeIcon : UI_Icon
{
	// Token: 0x170000DC RID: 220
	// (get) Token: 0x06000507 RID: 1287 RVA: 0x0001D86F File Offset: 0x0001BA6F
	public bool OpenFlag
	{
		get
		{
			if (this.modeID == 3)
			{
				return TempData.inst.daily_Open;
			}
			return TempData.inst.modeType == (EnumModeType)this.modeID;
		}
	}

	// Token: 0x06000508 RID: 1288 RVA: 0x0001D898 File Offset: 0x0001BA98
	private void Update()
	{
		if (this.mouseAbove && this.modeID == 3)
		{
			if (this.mouseAbove_UpdateLeft > 0f)
			{
				this.mouseAbove_UpdateLeft -= Time.unscaledDeltaTime;
			}
			else
			{
				this.mouseAbove_UpdateLeft = 0.5f;
				UI_ToolTip.inst.ShowWithIconInfinity(this.modeID, this.ifUnlocked(), this.OpenFlag);
			}
		}
		if (this.modeID == 3)
		{
			this.textDay.text = NetworkTime.GetString_DayWithDayIndex(NetworkTime.Inst.dayIndex);
		}
	}

	// Token: 0x06000509 RID: 1289 RVA: 0x0001D924 File Offset: 0x0001BB24
	public void Init(int modeID)
	{
		this.modeID = modeID;
		Sprite spriteWithId = ResourceLibrary.Inst.Splist_Icon_Modes.GetSpriteWithId(modeID);
		this.image.sprite = spriteWithId;
		this.OutlineNew_Close();
		this.UpdateColorAndOutline();
		base.UpdateLockIcon();
		if (modeID == 3)
		{
			this.textDay.text = NetworkTime.GetString_DayWithDayIndex(NetworkTime.Inst.dayIndex);
		}
	}

	// Token: 0x0600050A RID: 1290 RVA: 0x0001D988 File Offset: 0x0001BB88
	private void UpdateColorAndOutline()
	{
		if (this.modeID == 3)
		{
			this.image.color = Color.white;
		}
		else
		{
			this.image.color = ResourceLibrary.Inst.colorSet_UI.GetColorWithHue((float)this.modeID / 3f);
		}
		if (this.OpenFlag)
		{
			this.OutlineNew_Show_Selected();
			return;
		}
		this.OutlineNew_Close();
	}

	// Token: 0x0600050B RID: 1291 RVA: 0x0001D9EC File Offset: 0x0001BBEC
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (!this.ifUnlocked())
		{
			if (this.modeID == 3 && this.OpenFlag)
			{
				Debug.LogWarning("Warning_开着每日挑战却没解锁，重置返回");
				TempData.inst.DailyChallenge_Close(true);
				UI_Panel_Main_NewGame.inst.UpdatePanel();
				this.OnPointerEnter(eventData);
			}
			return;
		}
		int num = this.modeID;
		if (num > 2)
		{
			if (num == 3)
			{
				if (!this.OpenFlag)
				{
					TempData.inst.DailyChallenge_UpdateWithTodayIndex(true);
				}
				else
				{
					TempData.inst.DailyChallenge_Close(true);
				}
			}
		}
		else
		{
			if (TempData.inst.daily_Open)
			{
				UI_FloatTextControl.inst.NewFloatText(LanguageText.Inst.floatText.dailyChallenge_LockMode);
				return;
			}
			if (!this.OpenFlag)
			{
				TempData.inst.modeType = (EnumModeType)this.modeID;
			}
		}
		UI_Panel_Main_NewGame.inst.UpdatePanel();
		this.OnPointerEnter(eventData);
	}

	// Token: 0x0600050C RID: 1292 RVA: 0x0001DABC File Offset: 0x0001BCBC
	public override void OnPointerEnter(PointerEventData eventData)
	{
		this.mouseAbove = true;
		this.mouseAbove_UpdateLeft = 0.5f;
		if (!this.OpenFlag)
		{
			this.OutlineNew_Show_Above();
		}
		int num = this.modeID;
		if (num <= 3)
		{
			UI_ToolTip.inst.ShowWithIconInfinity(this.modeID, this.ifUnlocked(), this.OpenFlag);
			return;
		}
		Debug.LogError("ModeError");
	}

	// Token: 0x0600050D RID: 1293 RVA: 0x0001DB1B File Offset: 0x0001BD1B
	public override void OnPointerExit(PointerEventData eventData)
	{
		this.mouseAbove = false;
		UI_ToolTip.inst.Close();
		if (!this.OpenFlag)
		{
			this.OutlineNew_Close();
		}
	}

	// Token: 0x0600050E RID: 1294 RVA: 0x0001DB3C File Offset: 0x0001BD3C
	protected override bool ifUnlocked()
	{
		switch (this.modeID)
		{
		case 0:
			return true;
		case 1:
			return !GameParameters.Inst.ifDemo && GameData.inst.ifFinished;
		case 2:
			return !GameParameters.Inst.ifDemo && GameData.inst.maxEndless >= 40;
		case 3:
			if (GameParameters.Inst.ifDemo)
			{
				return false;
			}
			if (GameData.inst.maxEndless < 20)
			{
				return false;
			}
			for (int i = 0; i < 11; i++)
			{
				if (!GameData.inst.IfJobUnlocked(i))
				{
					return false;
				}
			}
			return true;
		default:
			Debug.LogError("ModeError");
			return false;
		}
	}

	// Token: 0x0400042B RID: 1067
	[SerializeField]
	private int modeID;

	// Token: 0x0400042C RID: 1068
	[SerializeField]
	private bool mouseAbove;

	// Token: 0x0400042D RID: 1069
	[SerializeField]
	private float mouseAbove_UpdateLeft;

	// Token: 0x0400042E RID: 1070
	public Image image;

	// Token: 0x0400042F RID: 1071
	[SerializeField]
	private Text textDay;
}
