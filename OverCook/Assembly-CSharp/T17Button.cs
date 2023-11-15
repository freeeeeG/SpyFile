using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B60 RID: 2912
[AddComponentMenu("T17_UI/T17BUtton", 30)]
public class T17Button : Button, IT17EventHelper
{
	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x06003B23 RID: 15139 RVA: 0x00119BCC File Offset: 0x00117FCC
	// (set) Token: 0x06003B24 RID: 15140 RVA: 0x00119BD4 File Offset: 0x00117FD4
	public GameOneShotAudioTag SelectAudioTag
	{
		get
		{
			return this.m_SelectAudioTag;
		}
		set
		{
			this.m_SelectAudioTag = value;
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06003B25 RID: 15141 RVA: 0x00119BDD File Offset: 0x00117FDD
	// (set) Token: 0x06003B26 RID: 15142 RVA: 0x00119BE5 File Offset: 0x00117FE5
	public GameOneShotAudioTag SubmitAudioTag
	{
		get
		{
			return this.m_SubmitAudioTag;
		}
		set
		{
			this.m_SubmitAudioTag = value;
		}
	}

	// Token: 0x06003B27 RID: 15143 RVA: 0x00119BF0 File Offset: 0x00117FF0
	public override Selectable FindSelectableOnDown()
	{
		Selectable selectable = base.FindSelectableOnDown();
		if (selectable != null && (!selectable.isActiveAndEnabled || !selectable.IsInteractable()))
		{
			return selectable.FindSelectableOnDown();
		}
		return selectable;
	}

	// Token: 0x06003B28 RID: 15144 RVA: 0x00119C30 File Offset: 0x00118030
	public override Selectable FindSelectableOnLeft()
	{
		Selectable selectable = base.FindSelectableOnLeft();
		if (selectable != null && (!selectable.isActiveAndEnabled || !selectable.IsInteractable()))
		{
			return selectable.FindSelectableOnLeft();
		}
		return selectable;
	}

	// Token: 0x06003B29 RID: 15145 RVA: 0x00119C70 File Offset: 0x00118070
	public override Selectable FindSelectableOnRight()
	{
		Selectable selectable = base.FindSelectableOnRight();
		if (selectable != null && (!selectable.isActiveAndEnabled || !selectable.IsInteractable()))
		{
			return selectable.FindSelectableOnRight();
		}
		return selectable;
	}

	// Token: 0x06003B2A RID: 15146 RVA: 0x00119CB0 File Offset: 0x001180B0
	public override Selectable FindSelectableOnUp()
	{
		Selectable selectable = base.FindSelectableOnUp();
		if (selectable != null && (!selectable.isActiveAndEnabled || !selectable.IsInteractable()))
		{
			return selectable.FindSelectableOnUp();
		}
		return selectable;
	}

	// Token: 0x06003B2B RID: 15147 RVA: 0x00119CEE File Offset: 0x001180EE
	protected override void Start()
	{
		base.Start();
		this.m_ButtonText = base.GetComponentInChildren<Text>(true);
		this.m_audioLayer = LayerMask.NameToLayer("Administration");
	}

	// Token: 0x06003B2C RID: 15148 RVA: 0x00119D13 File Offset: 0x00118113
	public void SetText(string text)
	{
		if (this.m_ButtonText != null)
		{
			this.m_ButtonText.text = text;
		}
	}

	// Token: 0x06003B2D RID: 15149 RVA: 0x00119D34 File Offset: 0x00118134
	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		if (this.OnButtonSelect != null)
		{
			this.OnButtonSelect(this);
		}
		if (this.m_bPlaySound && !(eventData is PointerEventData))
		{
			GameUtils.TriggerAudio(this.m_SelectAudioTag, this.m_audioLayer);
		}
		if (this.m_bShowTooltip && T17TooltipManager.Instance != null)
		{
			T17TooltipManager.Instance.Show(this.m_TooltipTag, !this.m_bLocalizeTooltip);
		}
	}

	// Token: 0x06003B2E RID: 15150 RVA: 0x00119DBB File Offset: 0x001181BB
	public override void OnSubmit(BaseEventData eventData)
	{
		base.OnSubmit(eventData);
		if (this.m_bPlaySound)
		{
			GameUtils.TriggerAudio(this.m_SubmitAudioTag, this.m_audioLayer);
		}
	}

	// Token: 0x06003B2F RID: 15151 RVA: 0x00119DE4 File Offset: 0x001181E4
	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		if (this.OnButtonDeselect != null)
		{
			this.OnButtonDeselect(this);
		}
		if (this.m_bShowTooltip && T17TooltipManager.Instance != null)
		{
			T17TooltipManager.Instance.Show(string.Empty, true);
		}
	}

	// Token: 0x06003B30 RID: 15152 RVA: 0x00119E3A File Offset: 0x0011823A
	public override void OnPointerDown(PointerEventData eventData)
	{
		base.OnPointerDown(eventData);
		if (this.m_bPlaySound)
		{
			GameUtils.TriggerAudio(this.m_SubmitAudioTag, this.m_audioLayer);
		}
	}

	// Token: 0x06003B31 RID: 15153 RVA: 0x00119E60 File Offset: 0x00118260
	public override void Select()
	{
		if (this.IsThereAnEventSystem())
		{
			if (this.m_EventSystem.alreadySelecting)
			{
				return;
			}
			this.m_EventSystem.SetSelectedGameObject(base.gameObject);
		}
	}

	// Token: 0x06003B32 RID: 15154 RVA: 0x00119E90 File Offset: 0x00118290
	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (this.OnButtonPointerEnter != null)
		{
			this.OnButtonPointerEnter(this);
		}
		if (this.m_bPlaySound)
		{
			GameUtils.TriggerAudio(this.m_SelectAudioTag, this.m_audioLayer);
		}
		if (this.m_bShowTooltip && T17TooltipManager.Instance != null)
		{
			T17TooltipManager.Instance.Show(this.m_TooltipTag, !this.m_bLocalizeTooltip);
		}
	}

	// Token: 0x06003B33 RID: 15155 RVA: 0x00119F0C File Offset: 0x0011830C
	public override void OnPointerExit(PointerEventData eventData)
	{
		base.OnPointerExit(eventData);
		if (this.OnButtonPointerExit != null)
		{
			this.OnButtonPointerExit(this);
		}
		if (this.m_bShowTooltip && T17TooltipManager.Instance != null)
		{
			T17TooltipManager.Instance.Show(string.Empty, true);
		}
	}

	// Token: 0x06003B34 RID: 15156 RVA: 0x00119F64 File Offset: 0x00118364
	public override void OnMove(AxisEventData eventData)
	{
		base.OnMove(eventData);
		if (this.OnButtonMove == null)
		{
			return;
		}
		Selectable to = null;
		switch (eventData.moveDir)
		{
		case MoveDirection.Left:
			to = base.navigation.selectOnLeft;
			break;
		case MoveDirection.Up:
			to = base.navigation.selectOnUp;
			break;
		case MoveDirection.Right:
			to = base.navigation.selectOnRight;
			break;
		case MoveDirection.Down:
			to = base.navigation.selectOnDown;
			break;
		}
		this.OnButtonMove(this, to, eventData.moveDir);
	}

	// Token: 0x06003B35 RID: 15157 RVA: 0x0011A00D File Offset: 0x0011840D
	public T17EventSystem GetDomain()
	{
		return this.m_EventSystem;
	}

	// Token: 0x06003B36 RID: 15158 RVA: 0x0011A015 File Offset: 0x00118415
	public GameObject GetGameobject()
	{
		return base.gameObject;
	}

	// Token: 0x06003B37 RID: 15159 RVA: 0x0011A01D File Offset: 0x0011841D
	public bool IsThereAnEventSystem()
	{
		return this.m_EventSystem != null;
	}

	// Token: 0x06003B38 RID: 15160 RVA: 0x0011A033 File Offset: 0x00118433
	public void SetEventSystem(T17EventSystem gamersEventSystem = null)
	{
		this.m_EventSystem = gamersEventSystem;
	}

	// Token: 0x0400301E RID: 12318
	private Text m_ButtonText;

	// Token: 0x0400301F RID: 12319
	public T17Button.T17ButtonDelegate OnButtonSelect;

	// Token: 0x04003020 RID: 12320
	public T17Button.T17ButtonDelegate OnButtonDeselect;

	// Token: 0x04003021 RID: 12321
	public T17Button.T17ButtonMoveDelegate OnButtonMove;

	// Token: 0x04003022 RID: 12322
	public T17Button.T17ButtonDelegate OnButtonPointerEnter;

	// Token: 0x04003023 RID: 12323
	public T17Button.T17ButtonDelegate OnButtonPointerExit;

	// Token: 0x04003024 RID: 12324
	public bool m_bPlaySound = true;

	// Token: 0x04003025 RID: 12325
	private int m_audioLayer;

	// Token: 0x04003026 RID: 12326
	public bool m_bShowTooltip;

	// Token: 0x04003027 RID: 12327
	public bool m_bLocalizeTooltip = true;

	// Token: 0x04003028 RID: 12328
	public string m_TooltipTag = "Text.UI.ButtonTooltip";

	// Token: 0x04003029 RID: 12329
	private T17EventSystem m_EventSystem;

	// Token: 0x0400302A RID: 12330
	private GamepadUser m_GamepadUser;

	// Token: 0x0400302B RID: 12331
	public T17Image m_lockImage;

	// Token: 0x0400302C RID: 12332
	[SerializeField]
	private GameOneShotAudioTag m_SelectAudioTag = GameOneShotAudioTag.UIHighlight;

	// Token: 0x0400302D RID: 12333
	[SerializeField]
	private GameOneShotAudioTag m_SubmitAudioTag = GameOneShotAudioTag.UISelect;

	// Token: 0x02000B61 RID: 2913
	// (Invoke) Token: 0x06003B3A RID: 15162
	public delegate void T17ButtonDelegate(T17Button sender);

	// Token: 0x02000B62 RID: 2914
	// (Invoke) Token: 0x06003B3E RID: 15166
	public delegate void T17ButtonMoveDelegate(Selectable from, Selectable to, MoveDirection direction);
}
