using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000144 RID: 324
public class UI_TalentPage_Popup : APopupWindow
{
	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x0600086D RID: 2157 RVA: 0x00020085 File Offset: 0x0001E285
	public bool IsPressingTalentButton
	{
		get
		{
			return this.isPressingTalentButton;
		}
	}

	// Token: 0x0600086E RID: 2158 RVA: 0x00020090 File Offset: 0x0001E290
	protected override void ShowWindowProc()
	{
		this.text_Title.text = "";
		this.text_Description.text = "";
		this.Initialize();
		base.StartCoroutine(this.CR_StartEffect());
		this.animator.SetBool("isOn", true);
		SoundManager.PlaySound("UI", "Talent_Show_1", -1f, -1f, -1f);
		SoundManager.PlaySound("UI", "Talent_Show_2", -1f, -1f, 0.25f);
	}

	// Token: 0x0600086F RID: 2159 RVA: 0x0002011F File Offset: 0x0001E31F
	protected override void CloseWindowProc()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x06000870 RID: 2160 RVA: 0x00020134 File Offset: 0x0001E334
	private void OnEnable()
	{
		foreach (UI_Obj_TalentButton ui_Obj_TalentButton in this.list_TalentButton)
		{
			ui_Obj_TalentButton.FillFullCallback = (Action<UI_Obj_TalentButton, int, TalentSetting>)Delegate.Combine(ui_Obj_TalentButton.FillFullCallback, new Action<UI_Obj_TalentButton, int, TalentSetting>(this.OnTalentButtonFillFull));
			ui_Obj_TalentButton.ButtonDownCallback = (Action<UI_Obj_TalentButton>)Delegate.Combine(ui_Obj_TalentButton.ButtonDownCallback, new Action<UI_Obj_TalentButton>(this.OnTalentButtonDown));
			ui_Obj_TalentButton.ButtonUpCallback = (Action<UI_Obj_TalentButton>)Delegate.Combine(ui_Obj_TalentButton.ButtonUpCallback, new Action<UI_Obj_TalentButton>(this.OnTalentButtonUp));
			ui_Obj_TalentButton.ButtonMouseInCallback = (Action<UI_Obj_TalentButton, TalentSetting>)Delegate.Combine(ui_Obj_TalentButton.ButtonMouseInCallback, new Action<UI_Obj_TalentButton, TalentSetting>(this.OnTalentButtonMouseIn));
			ui_Obj_TalentButton.ButtonMouseOutCallback = (Action<UI_Obj_TalentButton>)Delegate.Combine(ui_Obj_TalentButton.ButtonMouseOutCallback, new Action<UI_Obj_TalentButton>(this.OnTalentButtonMouseOut));
		}
		this.button_Leave.onClick.AddListener(new UnityAction(this.OnClickLeave));
		this.button_Reset.onClick.AddListener(new UnityAction(this.OnClickReset));
	}

	// Token: 0x06000871 RID: 2161 RVA: 0x00020268 File Offset: 0x0001E468
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			base.CloseWindow();
		}
	}

	// Token: 0x06000872 RID: 2162 RVA: 0x0002027C File Offset: 0x0001E47C
	private void OnDisable()
	{
		foreach (UI_Obj_TalentButton ui_Obj_TalentButton in this.list_TalentButton)
		{
			ui_Obj_TalentButton.FillFullCallback = (Action<UI_Obj_TalentButton, int, TalentSetting>)Delegate.Remove(ui_Obj_TalentButton.FillFullCallback, new Action<UI_Obj_TalentButton, int, TalentSetting>(this.OnTalentButtonFillFull));
			ui_Obj_TalentButton.ButtonDownCallback = (Action<UI_Obj_TalentButton>)Delegate.Remove(ui_Obj_TalentButton.ButtonDownCallback, new Action<UI_Obj_TalentButton>(this.OnTalentButtonDown));
			ui_Obj_TalentButton.ButtonUpCallback = (Action<UI_Obj_TalentButton>)Delegate.Remove(ui_Obj_TalentButton.ButtonUpCallback, new Action<UI_Obj_TalentButton>(this.OnTalentButtonUp));
			ui_Obj_TalentButton.ButtonMouseInCallback = (Action<UI_Obj_TalentButton, TalentSetting>)Delegate.Remove(ui_Obj_TalentButton.ButtonMouseInCallback, new Action<UI_Obj_TalentButton, TalentSetting>(this.OnTalentButtonMouseIn));
			ui_Obj_TalentButton.ButtonMouseOutCallback = (Action<UI_Obj_TalentButton>)Delegate.Remove(ui_Obj_TalentButton.ButtonMouseOutCallback, new Action<UI_Obj_TalentButton>(this.OnTalentButtonMouseOut));
		}
		this.button_Leave.onClick.RemoveListener(new UnityAction(this.OnClickLeave));
		this.button_Reset.onClick.RemoveListener(new UnityAction(this.OnClickReset));
	}

	// Token: 0x06000873 RID: 2163 RVA: 0x000203B0 File Offset: 0x0001E5B0
	private void OnClickLeave()
	{
		base.CloseWindow();
	}

	// Token: 0x06000874 RID: 2164 RVA: 0x000203B8 File Offset: 0x0001E5B8
	private void OnClickReset()
	{
		APopupWindow.CreateWindow<UI_Window_YesNo_Popup>(APopupWindow.ePopupWindowLayer.TOP, null, false).SetupContent("TALENT_RESET_CONFIRM", "TALENT_RESET_CONFIRM_YES", "TALENT_RESET_CONFIRM_NO", new Action<bool>(this.ResetConfirmCallback));
	}

	// Token: 0x06000875 RID: 2165 RVA: 0x000203E2 File Offset: 0x0001E5E2
	private void ResetConfirmCallback(bool doReset)
	{
		if (!doReset)
		{
			return;
		}
		this.ResetAllTalents();
	}

	// Token: 0x06000876 RID: 2166 RVA: 0x000203F0 File Offset: 0x0001E5F0
	private void ResetAllTalents()
	{
		int num = 0;
		for (int i = 0; i < this.list_TalentButton.Count; i++)
		{
			TalentSetting talentSettingByIndex = this.data.GetTalentSettingByIndex(i);
			if (GameDataManager.instance.Playerdata.IsTalentLearned(talentSettingByIndex.TalentType))
			{
				num += talentSettingByIndex.ExpCost;
			}
		}
		Debug.Log(string.Format("重置天賦, 返還經驗值: {0}", num));
		EventMgr.SendEvent(eGameEvents.RequestResetTalent);
		EventMgr.SendEvent<int>(eGameEvents.RequestAddExp, num);
		foreach (UI_Obj_TalentButton ui_Obj_TalentButton in this.list_TalentButton)
		{
			ui_Obj_TalentButton.SwitchState(UI_Obj_TalentButton.eState.UNAVALIABLE);
		}
		this.ResetAllButtonState();
		this.UpdateAllButtonState();
		GameDataManager.instance.SaveData();
	}

	// Token: 0x06000877 RID: 2167 RVA: 0x000204CC File Offset: 0x0001E6CC
	private void OnTalentButtonMouseIn(UI_Obj_TalentButton button, TalentSetting talentData)
	{
		string @string;
		string string2;
		if (talentData.LockInDemoVersion)
		{
			@string = LocalizationManager.Instance.GetString("UI", "TALENT_PAGE_UNLOCK_IN_FULL_VERSION_TITLE", Array.Empty<object>());
			string2 = LocalizationManager.Instance.GetString("UI", "TALENT_PAGE_UNLOCK_IN_FULL_VERSION_DESC", Array.Empty<object>());
		}
		else
		{
			@string = LocalizationManager.Instance.GetString("Talents", string.Format("{0}", talentData.TalentType), Array.Empty<object>());
			string2 = LocalizationManager.Instance.GetString("Talents", string.Format("{0}_DESC", talentData.TalentType), Array.Empty<object>());
		}
		this.text_Title.text = @string;
		this.text_Description.text = string2;
		SoundManager.PlaySound("UI", "Talent_MouseOverButton", -1f, -1f, -1f);
		button.transform.DOScale(Vector3.one * 1.2f, 0.1f).SetEase(Ease.OutCubic);
		button.transform.SetAsLastSibling();
		this.node_SelectionFrame.transform.SetParent(button.Node_Shake);
		this.node_SelectionFrame.transform.localPosition = Vector3.zero;
		this.node_SelectionFrame.transform.localScale = Vector3.one;
		this.node_SelectionFrame.gameObject.SetActive(true);
	}

	// Token: 0x06000878 RID: 2168 RVA: 0x00020634 File Offset: 0x0001E834
	private void OnTalentButtonMouseOut(UI_Obj_TalentButton button)
	{
		this.text_Title.text = "";
		this.text_Description.text = "";
		button.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutCubic);
		this.node_SelectionFrame.gameObject.SetActive(false);
	}

	// Token: 0x06000879 RID: 2169 RVA: 0x00020690 File Offset: 0x0001E890
	private void OnTalentButtonDown(UI_Obj_TalentButton button)
	{
		this.currentButtonIndex = button.Index;
		this.particle_ChargeEffect.transform.position = button.transform.position + button.transform.forward * -5f;
		this.particle_ChargeEffect.Play(true);
	}

	// Token: 0x0600087A RID: 2170 RVA: 0x000206EA File Offset: 0x0001E8EA
	private void OnTalentButtonUp(UI_Obj_TalentButton button)
	{
		if (button.Index != this.currentButtonIndex)
		{
			return;
		}
		this.currentButtonIndex = -1;
		this.particle_ChargeEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
	}

	// Token: 0x0600087B RID: 2171 RVA: 0x00020710 File Offset: 0x0001E910
	private void OnTalentButtonFillFull(UI_Obj_TalentButton button, int index, TalentSetting talentData)
	{
		if (talentData.ExpCost > GameDataManager.instance.Playerdata.Exp)
		{
			Debug.LogError("收到新天賦的要求, 但EXP不夠");
			return;
		}
		button.PlayLearnTalentAnimation();
		button.SwitchState(UI_Obj_TalentButton.eState.LEARNED);
		EventMgr.SendEvent<eTalentType>(eGameEvents.RequestLearnTalent, talentData.TalentType);
		int expCost = talentData.ExpCost;
		EventMgr.SendEvent<int>(eGameEvents.RequestAddExp, -1 * expCost);
		this.UpdateAllButtonState();
		this.animator.SetTrigger("learnTalent");
		this.particle_ChargeFullEffect.transform.position = button.transform.position + button.transform.forward * -5f;
		this.particle_ChargeFullEffect.Play();
		this.list_playedButtons = new List<Vector2Int>();
		this.list_playedButtons.Add(button.Coordinate);
		base.StartCoroutine(this.TriggerSmallBounceAnimRecursive(button.transform.position, button.Coordinate, 0f));
		GameDataManager.instance.SaveData();
	}

	// Token: 0x0600087C RID: 2172 RVA: 0x00020815 File Offset: 0x0001EA15
	private IEnumerator TriggerSmallBounceAnimRecursive(Vector3 origin, Vector2Int fromCoord, float delay)
	{
		List<UI_Obj_TalentButton> neighborButtons = this.GetNeighborButtons(fromCoord);
		yield return new WaitForSeconds(delay);
		yield return new WaitForSeconds(0.05f);
		using (List<UI_Obj_TalentButton>.Enumerator enumerator = neighborButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UI_Obj_TalentButton ui_Obj_TalentButton = enumerator.Current;
				if (!this.list_playedButtons.Contains(ui_Obj_TalentButton.Coordinate))
				{
					ui_Obj_TalentButton.transform.DOPunchPosition((ui_Obj_TalentButton.transform.position - origin).normalized * 25f, 2f, 3, 0f, false);
					ui_Obj_TalentButton.PlaySmallBounceAnimation(0f);
					this.list_playedButtons.Add(ui_Obj_TalentButton.Coordinate);
					base.StartCoroutine(this.TriggerSmallBounceAnimRecursive(origin, ui_Obj_TalentButton.Coordinate, 0f));
				}
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x0600087D RID: 2173 RVA: 0x0002083C File Offset: 0x0001EA3C
	private void Initialize()
	{
		this.dic_TalentButton = new Dictionary<Vector2Int, UI_Obj_TalentButton>();
		for (int i = 0; i < this.list_TalentButton.Count; i++)
		{
			this.dic_TalentButton.Add(this.list_TalentButton[i].Coordinate, this.list_TalentButton[i]);
			TalentSetting talentSettingByIndex = this.data.GetTalentSettingByIndex(i);
			this.list_TalentButton[i].SetupContent(i, talentSettingByIndex, this);
		}
		this.ResetAllButtonState();
		this.UpdateAllButtonState();
	}

	// Token: 0x0600087E RID: 2174 RVA: 0x000208C0 File Offset: 0x0001EAC0
	private void ResetAllButtonState()
	{
		this.GetButtonByCoord(new Vector2Int(2, 2)).SwitchState(UI_Obj_TalentButton.eState.AVALIABLE);
		this.GetButtonByCoord(new Vector2Int(2, 3)).SwitchState(UI_Obj_TalentButton.eState.AVALIABLE);
		this.GetButtonByCoord(new Vector2Int(3, 2)).SwitchState(UI_Obj_TalentButton.eState.AVALIABLE);
		this.GetButtonByCoord(new Vector2Int(3, 3)).SwitchState(UI_Obj_TalentButton.eState.AVALIABLE);
		this.UpdateAllButtonState();
	}

	// Token: 0x0600087F RID: 2175 RVA: 0x00020920 File Offset: 0x0001EB20
	private void UpdateAllButtonState()
	{
		int i = 0;
		while (i < this.list_TalentButton.Count)
		{
			TalentSetting talentSettingByIndex = this.data.GetTalentSettingByIndex(i);
			if (GameDataManager.instance.Playerdata.IsTalentLearned(talentSettingByIndex.TalentType))
			{
				this.list_TalentButton[i].SwitchState(UI_Obj_TalentButton.eState.LEARNED);
				using (List<UI_Obj_TalentButton>.Enumerator enumerator = this.GetNeighborButtons(this.list_TalentButton[i].Coordinate).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						UI_Obj_TalentButton ui_Obj_TalentButton = enumerator.Current;
						if (ui_Obj_TalentButton.State != UI_Obj_TalentButton.eState.LEARNED)
						{
							ui_Obj_TalentButton.SwitchState(UI_Obj_TalentButton.eState.AVALIABLE);
						}
					}
					goto IL_B3;
				}
				goto IL_8D;
			}
			goto IL_8D;
			IL_B3:
			i++;
			continue;
			IL_8D:
			if (this.list_TalentButton[i].State != UI_Obj_TalentButton.eState.AVALIABLE)
			{
				this.list_TalentButton[i].SwitchState(UI_Obj_TalentButton.eState.UNAVALIABLE);
				goto IL_B3;
			}
			goto IL_B3;
		}
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x00020A08 File Offset: 0x0001EC08
	private IEnumerator CR_StartEffect()
	{
		int num;
		for (int i = 0; i < this.list_TalentButton.Count; i = num + 1)
		{
			this.list_TalentButton[i].ToggleButton(true);
			yield return new WaitForSecondsRealtime(0.01f);
			num = i;
		}
		yield break;
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x00020A18 File Offset: 0x0001EC18
	private void SetButtonStateByData()
	{
		foreach (UI_Obj_TalentButton ui_Obj_TalentButton in this.list_TalentButton)
		{
		}
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00020A64 File Offset: 0x0001EC64
	private List<UI_Obj_TalentButton> GetNeighborButtons(Vector2Int coord)
	{
		List<UI_Obj_TalentButton> list = new List<UI_Obj_TalentButton>();
		UI_Obj_TalentButton buttonByCoord = this.GetButtonByCoord(coord + Vector2Int.up);
		UI_Obj_TalentButton buttonByCoord2 = this.GetButtonByCoord(coord + Vector2Int.down);
		UI_Obj_TalentButton buttonByCoord3 = this.GetButtonByCoord(coord + Vector2Int.left);
		UI_Obj_TalentButton buttonByCoord4 = this.GetButtonByCoord(coord + Vector2Int.right);
		if (buttonByCoord != null)
		{
			list.Add(buttonByCoord);
		}
		if (buttonByCoord2 != null)
		{
			list.Add(buttonByCoord2);
		}
		if (buttonByCoord3 != null)
		{
			list.Add(buttonByCoord3);
		}
		if (buttonByCoord4 != null)
		{
			list.Add(buttonByCoord4);
		}
		return list;
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x00020B03 File Offset: 0x0001ED03
	private UI_Obj_TalentButton GetButtonByCoord(Vector2Int vector2Int)
	{
		if (this.dic_TalentButton.ContainsKey(vector2Int))
		{
			return this.dic_TalentButton[vector2Int];
		}
		return null;
	}

	// Token: 0x040006D8 RID: 1752
	[SerializeField]
	private TalentSettingData data;

	// Token: 0x040006D9 RID: 1753
	[SerializeField]
	private TMP_Text text_Title;

	// Token: 0x040006DA RID: 1754
	[SerializeField]
	private TMP_Text text_Description;

	// Token: 0x040006DB RID: 1755
	[SerializeField]
	private Button button_Leave;

	// Token: 0x040006DC RID: 1756
	[SerializeField]
	private Button button_Reset;

	// Token: 0x040006DD RID: 1757
	[SerializeField]
	private Transform node_SelectionFrame;

	// Token: 0x040006DE RID: 1758
	[SerializeField]
	private ParticleSystem particle_ChargeEffect;

	// Token: 0x040006DF RID: 1759
	[SerializeField]
	private ParticleSystem particle_ChargeFullEffect;

	// Token: 0x040006E0 RID: 1760
	[SerializeField]
	private List<UI_Obj_TalentButton> list_TalentButton;

	// Token: 0x040006E1 RID: 1761
	[SerializeField]
	private Dictionary<Vector2Int, UI_Obj_TalentButton> dic_TalentButton;

	// Token: 0x040006E2 RID: 1762
	private bool isPressingTalentButton;

	// Token: 0x040006E3 RID: 1763
	private int currentButtonIndex = -1;

	// Token: 0x040006E4 RID: 1764
	private int curExpValue = -1;

	// Token: 0x040006E5 RID: 1765
	private List<Vector2Int> list_playedButtons;
}
