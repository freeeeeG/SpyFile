using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B77 RID: 2935
[AddComponentMenu("T17_UI/Scrollbar", 31)]
public class T17Scrollbar : Scrollbar, IT17EventHelper, IScrollHandler, IEventSystemHandler
{
	// Token: 0x06003BB3 RID: 15283 RVA: 0x0011C83A File Offset: 0x0011AC3A
	public void SetEventSystem(T17EventSystem gamersEventSystem = null)
	{
		this.m_EventSystem = gamersEventSystem;
	}

	// Token: 0x06003BB4 RID: 15284 RVA: 0x0011C844 File Offset: 0x0011AC44
	public override void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		if (this.IsInteractable() && base.navigation.mode != Navigation.Mode.None && this.m_EventSystem != null)
		{
			this.m_EventSystem.SetSelectedGameObject(base.gameObject, eventData);
		}
		base.OnPointerDown(eventData);
	}

	// Token: 0x06003BB5 RID: 15285 RVA: 0x0011C8A5 File Offset: 0x0011ACA5
	public override void Select()
	{
		if (this.m_EventSystem.alreadySelecting)
		{
			return;
		}
		this.m_EventSystem.SetSelectedGameObject(base.gameObject);
	}

	// Token: 0x06003BB6 RID: 15286 RVA: 0x0011C8C9 File Offset: 0x0011ACC9
	public T17EventSystem GetDomain()
	{
		return this.m_EventSystem;
	}

	// Token: 0x06003BB7 RID: 15287 RVA: 0x0011C8D1 File Offset: 0x0011ACD1
	public GameObject GetGameobject()
	{
		return base.gameObject;
	}

	// Token: 0x06003BB8 RID: 15288 RVA: 0x0011C8D9 File Offset: 0x0011ACD9
	public bool CanReselectOnMouseDisable()
	{
		return true;
	}

	// Token: 0x06003BB9 RID: 15289 RVA: 0x0011C8DC File Offset: 0x0011ACDC
	public bool ReleaseSelectionOnPointerClickOrExit()
	{
		return true;
	}

	// Token: 0x06003BBA RID: 15290 RVA: 0x0011C8DF File Offset: 0x0011ACDF
	private void Update()
	{
		this.m_bReceivedScrollEvent = false;
	}

	// Token: 0x06003BBB RID: 15291 RVA: 0x0011C8E8 File Offset: 0x0011ACE8
	public void OnScroll(PointerEventData eventData)
	{
		if (!this.IsActive() || this.m_bReceivedScrollEvent)
		{
			return;
		}
		this.m_bReceivedScrollEvent = true;
		Vector2 scrollDelta = eventData.scrollDelta;
		if (Mathf.Abs(scrollDelta.x) > Mathf.Abs(scrollDelta.y))
		{
			scrollDelta.y = scrollDelta.x;
		}
		scrollDelta.x = 0f;
		base.value += scrollDelta.y * this.m_ScrollSensitivity * Mathf.Lerp(1f, 10f, base.size);
	}

	// Token: 0x04003083 RID: 12419
	[Range(0.01f, 0.2f)]
	public float m_ScrollSensitivity = 0.05f;

	// Token: 0x04003084 RID: 12420
	private T17EventSystem m_EventSystem;

	// Token: 0x04003085 RID: 12421
	private bool m_bReceivedScrollEvent;
}
