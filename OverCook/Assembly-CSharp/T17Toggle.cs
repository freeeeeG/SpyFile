using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B7F RID: 2943
[AddComponentMenu("T17_UI/Toggle", 32)]
public class T17Toggle : Toggle, IT17EventHelper
{
	// Token: 0x06003C10 RID: 15376 RVA: 0x0011F952 File Offset: 0x0011DD52
	protected override void Start()
	{
		base.Start();
		this.m_audioLayer = LayerMask.NameToLayer("Administration");
	}

	// Token: 0x06003C11 RID: 15377 RVA: 0x0011F96A File Offset: 0x0011DD6A
	public void SetEventSystem(T17EventSystem gamersEventSystem)
	{
		this.m_EventSystem = gamersEventSystem;
	}

	// Token: 0x06003C12 RID: 15378 RVA: 0x0011F974 File Offset: 0x0011DD74
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		if (this.IsInteractable() && base.navigation.mode != Navigation.Mode.None)
		{
			this.m_EventSystem.SetSelectedGameObject(base.gameObject, eventData);
		}
		base.OnPointerDown(eventData);
	}

	// Token: 0x06003C13 RID: 15379 RVA: 0x0011F9C4 File Offset: 0x0011DDC4
	public override void Select()
	{
		if (this.m_EventSystem.alreadySelecting)
		{
			return;
		}
		this.m_EventSystem.SetSelectedGameObject(base.gameObject);
	}

	// Token: 0x06003C14 RID: 15380 RVA: 0x0011F9E8 File Offset: 0x0011DDE8
	public T17EventSystem GetDomain()
	{
		return this.m_EventSystem;
	}

	// Token: 0x06003C15 RID: 15381 RVA: 0x0011F9F0 File Offset: 0x0011DDF0
	public GameObject GetGameobject()
	{
		return base.gameObject;
	}

	// Token: 0x06003C16 RID: 15382 RVA: 0x0011F9F8 File Offset: 0x0011DDF8
	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		if (this.m_bPlaySound)
		{
			GameUtils.TriggerAudio(this.m_selectAudioTag, this.m_audioLayer);
		}
	}

	// Token: 0x06003C17 RID: 15383 RVA: 0x0011FA1E File Offset: 0x0011DE1E
	public override void OnPointerClick(PointerEventData eventData)
	{
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		if (this.m_bPlaySound)
		{
			GameUtils.TriggerAudio(this.m_submitAudioTag, this.m_audioLayer);
		}
		base.OnPointerClick(eventData);
	}

	// Token: 0x06003C18 RID: 15384 RVA: 0x0011FA5B File Offset: 0x0011DE5B
	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		if (this.m_bPlaySound)
		{
			GameUtils.TriggerAudio(this.m_selectAudioTag, this.m_audioLayer);
		}
	}

	// Token: 0x06003C19 RID: 15385 RVA: 0x0011FA81 File Offset: 0x0011DE81
	public override void OnSubmit(BaseEventData eventData)
	{
		if (!this.IsActive() || !this.IsInteractable())
		{
			return;
		}
		if (this.m_bPlaySound)
		{
			GameUtils.TriggerAudio(this.m_submitAudioTag, this.m_audioLayer);
		}
		base.OnSubmit(eventData);
	}

	// Token: 0x040030D0 RID: 12496
	[SerializeField]
	private GameOneShotAudioTag m_selectAudioTag = GameOneShotAudioTag.UIHighlight;

	// Token: 0x040030D1 RID: 12497
	[SerializeField]
	private GameOneShotAudioTag m_submitAudioTag = GameOneShotAudioTag.UISelect;

	// Token: 0x040030D2 RID: 12498
	public bool m_bPlaySound = true;

	// Token: 0x040030D3 RID: 12499
	private T17EventSystem m_EventSystem;

	// Token: 0x040030D4 RID: 12500
	private int m_audioLayer;
}
