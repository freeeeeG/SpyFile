using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000B79 RID: 2937
[AddComponentMenu("T17_UI/Slider", 32)]
public class T17Slider : Slider, IT17EventHelper
{
	// Token: 0x06003BD7 RID: 15319 RVA: 0x0011E03E File Offset: 0x0011C43E
	public void SetEventSystem(T17EventSystem gamersEventSystem)
	{
		this.m_EventSystem = gamersEventSystem;
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x0011E048 File Offset: 0x0011C448
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

	// Token: 0x06003BD9 RID: 15321 RVA: 0x0011E098 File Offset: 0x0011C498
	public override void Select()
	{
		if (this.m_EventSystem.alreadySelecting)
		{
			return;
		}
		this.m_EventSystem.SetSelectedGameObject(base.gameObject);
	}

	// Token: 0x06003BDA RID: 15322 RVA: 0x0011E0BC File Offset: 0x0011C4BC
	public T17EventSystem GetDomain()
	{
		return this.m_EventSystem;
	}

	// Token: 0x06003BDB RID: 15323 RVA: 0x0011E0C4 File Offset: 0x0011C4C4
	public GameObject GetGameobject()
	{
		return base.gameObject;
	}

	// Token: 0x06003BDC RID: 15324 RVA: 0x0011E0CC File Offset: 0x0011C4CC
	public bool CanReselectOnMouseDisable()
	{
		return true;
	}

	// Token: 0x06003BDD RID: 15325 RVA: 0x0011E0CF File Offset: 0x0011C4CF
	public bool ReleaseSelectionOnPointerClickOrExit()
	{
		return true;
	}

	// Token: 0x0400309A RID: 12442
	private T17EventSystem m_EventSystem;
}
