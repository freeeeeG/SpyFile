using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000143 RID: 323
[SelectionBase]
public class UI_Obj_TalentButton : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x170000A4 RID: 164
	// (get) Token: 0x06000857 RID: 2135 RVA: 0x0001F9A1 File Offset: 0x0001DBA1
	public Transform Node_Shake
	{
		get
		{
			return this.node_Shake;
		}
	}

	// Token: 0x170000A5 RID: 165
	// (get) Token: 0x06000858 RID: 2136 RVA: 0x0001F9A9 File Offset: 0x0001DBA9
	// (set) Token: 0x06000859 RID: 2137 RVA: 0x0001F9B1 File Offset: 0x0001DBB1
	public Vector2Int Coordinate
	{
		get
		{
			return this.coordintate;
		}
		set
		{
			this.coordintate = value;
		}
	}

	// Token: 0x170000A6 RID: 166
	// (get) Token: 0x0600085A RID: 2138 RVA: 0x0001F9BA File Offset: 0x0001DBBA
	public UI_Obj_TalentButton.eState State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x170000A7 RID: 167
	// (get) Token: 0x0600085B RID: 2139 RVA: 0x0001F9C2 File Offset: 0x0001DBC2
	public int Index
	{
		get
		{
			return this.index;
		}
	}

	// Token: 0x0600085C RID: 2140 RVA: 0x0001F9CC File Offset: 0x0001DBCC
	public void SetupContent(int index, TalentSetting data, UI_TalentPage_Popup ref_TalentPage)
	{
		this.index = index;
		this.talentData = data;
		this.ref_TalentPage = ref_TalentPage;
		this.image_Icon.sprite = data.Icon;
		int exp = GameDataManager.instance.Playerdata.Exp;
		this.text_Cost.text = data.ExpCost.ToString();
		this.text_Cost.color = ((this.talentData.ExpCost > exp) ? this.Color_Text_Unpuchaseable : this.Color_Text_Puchaseable);
		if (data.LockInDemoVersion)
		{
			this.node_LockInDemoVersion.gameObject.SetActive(true);
		}
	}

	// Token: 0x0600085D RID: 2141 RVA: 0x0001FA68 File Offset: 0x0001DC68
	private void OnEnable()
	{
		UI_HoldableButton ui_HoldableButton = this.button;
		ui_HoldableButton.OnHoldButton = (Action)Delegate.Combine(ui_HoldableButton.OnHoldButton, new Action(this.OnHoldButton));
		UI_HoldableButton ui_HoldableButton2 = this.button;
		ui_HoldableButton2.OnButtonUp = (Action)Delegate.Combine(ui_HoldableButton2.OnButtonUp, new Action(this.OnButtonUp));
		UI_HoldableButton ui_HoldableButton3 = this.button;
		ui_HoldableButton3.OnButtonDown = (Action)Delegate.Combine(ui_HoldableButton3.OnButtonDown, new Action(this.OnButtonDown));
		EventMgr.Register<int>(eGameEvents.OnExpChanged, new Action<int>(this.OnExpChanged));
	}

	// Token: 0x0600085E RID: 2142 RVA: 0x0001FB04 File Offset: 0x0001DD04
	private void OnDisable()
	{
		UI_HoldableButton ui_HoldableButton = this.button;
		ui_HoldableButton.OnHoldButton = (Action)Delegate.Remove(ui_HoldableButton.OnHoldButton, new Action(this.OnHoldButton));
		UI_HoldableButton ui_HoldableButton2 = this.button;
		ui_HoldableButton2.OnButtonUp = (Action)Delegate.Remove(ui_HoldableButton2.OnButtonUp, new Action(this.OnButtonUp));
		UI_HoldableButton ui_HoldableButton3 = this.button;
		ui_HoldableButton3.OnButtonDown = (Action)Delegate.Remove(ui_HoldableButton3.OnButtonDown, new Action(this.OnButtonDown));
		EventMgr.Remove<int>(eGameEvents.OnExpChanged, new Action<int>(this.OnExpChanged));
	}

	// Token: 0x0600085F RID: 2143 RVA: 0x0001FB9E File Offset: 0x0001DD9E
	private void OnExpChanged(int value)
	{
		this.text_Cost.color = ((this.talentData.ExpCost > value) ? this.Color_Text_Unpuchaseable : this.Color_Text_Puchaseable);
	}

	// Token: 0x06000860 RID: 2144 RVA: 0x0001FBC8 File Offset: 0x0001DDC8
	private void OnButtonDown()
	{
		if (this.ref_TalentPage.IsPressingTalentButton)
		{
			return;
		}
		if (this.state == UI_Obj_TalentButton.eState.UNAVALIABLE)
		{
			this.PlayUnavailableAnimation();
			return;
		}
		if (this.talentData.ExpCost > GameDataManager.instance.Playerdata.Exp)
		{
			this.PlayUnavailableAnimation();
			return;
		}
		this.isHoldButton = true;
		this.animator.SetBool("doShake", true);
		SoundManager.PlaySound("UI", "Talent_HoldButton", 0.8f, -1f, -1f);
		base.transform.SetAsLastSibling();
		Action<UI_Obj_TalentButton> buttonDownCallback = this.ButtonDownCallback;
		if (buttonDownCallback == null)
		{
			return;
		}
		buttonDownCallback(this);
	}

	// Token: 0x06000861 RID: 2145 RVA: 0x0001FC68 File Offset: 0x0001DE68
	private void OnButtonUp()
	{
		if (!this.isHoldButton)
		{
			return;
		}
		this.isHoldButton = false;
		this.animator.SetBool("doShake", false);
		Action<UI_Obj_TalentButton> buttonUpCallback = this.ButtonUpCallback;
		if (buttonUpCallback == null)
		{
			return;
		}
		buttonUpCallback(this);
	}

	// Token: 0x06000862 RID: 2146 RVA: 0x0001FC9C File Offset: 0x0001DE9C
	private void Update()
	{
		if (this.isHoldButton)
		{
			this.energyFillLevel = Mathf.Clamp01(this.energyFillLevel + Time.deltaTime / this.HOLD_BUTTON_MAX_TIME);
			this.image_EnergyFill.fillAmount = this.energyFillLevel;
			this.node_Shake.localScale = Vector3.one * (1f + 0.5f * Easing.GetEasingFunction(Easing.Type.EaseOutCubic, this.energyFillLevel));
			if (this.energyFillLevel >= 1f)
			{
				this.energyFillLevel = 0f;
				Action<UI_Obj_TalentButton, int, TalentSetting> fillFullCallback = this.FillFullCallback;
				if (fillFullCallback != null)
				{
					fillFullCallback(this, this.index, this.talentData);
				}
				this.animator.SetBool("doShake", false);
			}
			this.soundPlayTimer += Time.deltaTime;
			if (this.soundPlayTimer >= 0.1f)
			{
				this.soundPlayTimer = 0f;
				float pitch = 0.8f + Easing.GetEasingFunction(Easing.Type.EaseOutSine, this.button.PressedTime / this.HOLD_BUTTON_MAX_TIME) * 0.8f;
				SoundManager.PlaySound("UI", "Talent_HoldButton", pitch, -1f, -1f);
				return;
			}
		}
		else
		{
			if (this.energyFillLevel >= 0f)
			{
				this.energyFillLevel = Mathf.Clamp01(this.energyFillLevel - Time.deltaTime * 5f);
			}
			this.image_EnergyFill.fillAmount = this.energyFillLevel;
			this.node_Shake.localScale = Vector3.one * (1f + 0.5f * Easing.GetEasingFunction(Easing.Type.EaseOutCubic, this.energyFillLevel));
			this.soundPlayTimer = 0f;
		}
	}

	// Token: 0x06000863 RID: 2147 RVA: 0x0001FE39 File Offset: 0x0001E039
	private void OnHoldButton()
	{
	}

	// Token: 0x06000864 RID: 2148 RVA: 0x0001FE3B File Offset: 0x0001E03B
	public void PlayLearnTalentAnimation()
	{
		SoundManager.PlaySound("UI", "Talent_Learned", -1f, -1f, -1f);
		this.animator.SetTrigger("talentLearned");
	}

	// Token: 0x06000865 RID: 2149 RVA: 0x0001FE6C File Offset: 0x0001E06C
	public void PlaySmallBounceAnimation(float delay)
	{
		base.StartCoroutine(this.CR_PlaySmallBounceAnimation(delay));
	}

	// Token: 0x06000866 RID: 2150 RVA: 0x0001FE7C File Offset: 0x0001E07C
	public void PlayUnavailableAnimation()
	{
		this.animator.SetTrigger("unavailable");
		SoundManager.PlaySound("UI", "Talent_Unavailable", -1f, -1f, -1f);
	}

	// Token: 0x06000867 RID: 2151 RVA: 0x0001FEAD File Offset: 0x0001E0AD
	private IEnumerator CR_PlaySmallBounceAnimation(float delay)
	{
		yield return new WaitForSeconds(delay);
		this.animator.SetTrigger("smallBounce");
		yield break;
	}

	// Token: 0x06000868 RID: 2152 RVA: 0x0001FEC4 File Offset: 0x0001E0C4
	public void SwitchState(UI_Obj_TalentButton.eState targetState)
	{
		this.state = targetState;
		switch (this.state)
		{
		case UI_Obj_TalentButton.eState.NOT_INITIALIZED:
			break;
		case UI_Obj_TalentButton.eState.UNAVALIABLE:
			this.image_Frame.sprite = this.sprite_Frame_Unavaliable;
			this.image_Icon.enabled = !this.talentData.LockInDemoVersion;
			this.text_Cost.enabled = false;
			this.button.interactable = false;
			this.animator.SetBool("isAvaliable", false);
			return;
		case UI_Obj_TalentButton.eState.AVALIABLE:
			this.image_Frame.sprite = this.sprite_Frame_Avaliable;
			this.image_Icon.enabled = !this.talentData.LockInDemoVersion;
			this.text_Cost.enabled = true;
			this.button.interactable = !this.talentData.LockInDemoVersion;
			this.animator.SetBool("isAvaliable", true);
			return;
		case UI_Obj_TalentButton.eState.LEARNED:
			this.image_Frame.sprite = this.sprite_Frame_Learned;
			this.text_Cost.enabled = false;
			this.button.interactable = false;
			this.animator.SetBool("isAvaliable", true);
			break;
		default:
			return;
		}
	}

	// Token: 0x06000869 RID: 2153 RVA: 0x0001FFE7 File Offset: 0x0001E1E7
	public void ToggleButton(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
	}

	// Token: 0x0600086A RID: 2154 RVA: 0x0001FFFA File Offset: 0x0001E1FA
	public void OnPointerEnter(PointerEventData eventData)
	{
		Action<UI_Obj_TalentButton, TalentSetting> buttonMouseInCallback = this.ButtonMouseInCallback;
		if (buttonMouseInCallback == null)
		{
			return;
		}
		buttonMouseInCallback(this, this.talentData);
	}

	// Token: 0x0600086B RID: 2155 RVA: 0x00020014 File Offset: 0x0001E214
	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.isHoldButton)
		{
			this.isHoldButton = false;
			this.animator.SetBool("doShake", false);
			Action<UI_Obj_TalentButton> buttonUpCallback = this.ButtonUpCallback;
			if (buttonUpCallback != null)
			{
				buttonUpCallback(this);
			}
		}
		Action<UI_Obj_TalentButton> buttonMouseOutCallback = this.ButtonMouseOutCallback;
		if (buttonMouseOutCallback == null)
		{
			return;
		}
		buttonMouseOutCallback(this);
	}

	// Token: 0x040006BD RID: 1725
	[SerializeField]
	private Animator animator;

	// Token: 0x040006BE RID: 1726
	[SerializeField]
	private UI_HoldableButton button;

	// Token: 0x040006BF RID: 1727
	[SerializeField]
	private Image image_Icon;

	// Token: 0x040006C0 RID: 1728
	[SerializeField]
	private Image image_Frame;

	// Token: 0x040006C1 RID: 1729
	[SerializeField]
	private Image image_EnergyFill;

	// Token: 0x040006C2 RID: 1730
	[SerializeField]
	private Sprite sprite_Frame_Learned;

	// Token: 0x040006C3 RID: 1731
	[SerializeField]
	private Sprite sprite_Frame_Avaliable;

	// Token: 0x040006C4 RID: 1732
	[SerializeField]
	private Sprite sprite_Frame_Unavaliable;

	// Token: 0x040006C5 RID: 1733
	[SerializeField]
	private TMP_Text text_Cost;

	// Token: 0x040006C6 RID: 1734
	[SerializeField]
	private Transform node_Shake;

	// Token: 0x040006C7 RID: 1735
	[SerializeField]
	private Transform node_LockInDemoVersion;

	// Token: 0x040006C8 RID: 1736
	[SerializeField]
	private Color Color_Text_Puchaseable;

	// Token: 0x040006C9 RID: 1737
	[SerializeField]
	private Color Color_Text_Unpuchaseable;

	// Token: 0x040006CA RID: 1738
	[SerializeField]
	private Vector2Int coordintate;

	// Token: 0x040006CB RID: 1739
	[SerializeField]
	private UI_Obj_TalentButton.eState state = UI_Obj_TalentButton.eState.NOT_INITIALIZED;

	// Token: 0x040006CC RID: 1740
	private UI_TalentPage_Popup ref_TalentPage;

	// Token: 0x040006CD RID: 1741
	private readonly float HOLD_BUTTON_MAX_TIME = 1f;

	// Token: 0x040006CE RID: 1742
	private float energyFillLevel;

	// Token: 0x040006CF RID: 1743
	public Action<UI_Obj_TalentButton, int, TalentSetting> FillFullCallback;

	// Token: 0x040006D0 RID: 1744
	public Action<UI_Obj_TalentButton> ButtonDownCallback;

	// Token: 0x040006D1 RID: 1745
	public Action<UI_Obj_TalentButton> ButtonUpCallback;

	// Token: 0x040006D2 RID: 1746
	public Action<UI_Obj_TalentButton, TalentSetting> ButtonMouseInCallback;

	// Token: 0x040006D3 RID: 1747
	public Action<UI_Obj_TalentButton> ButtonMouseOutCallback;

	// Token: 0x040006D4 RID: 1748
	private bool isHoldButton;

	// Token: 0x040006D5 RID: 1749
	private int index = -1;

	// Token: 0x040006D6 RID: 1750
	private TalentSetting talentData;

	// Token: 0x040006D7 RID: 1751
	private float soundPlayTimer;

	// Token: 0x02000280 RID: 640
	public enum eState
	{
		// Token: 0x04000BE7 RID: 3047
		NOT_INITIALIZED = -1,
		// Token: 0x04000BE8 RID: 3048
		UNAVALIABLE,
		// Token: 0x04000BE9 RID: 3049
		AVALIABLE,
		// Token: 0x04000BEA RID: 3050
		LEARNED
	}
}
