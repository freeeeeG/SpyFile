using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Token: 0x02000B75 RID: 2933
public class T17OverTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	// Token: 0x06003BAB RID: 15275 RVA: 0x0011C7BC File Offset: 0x0011ABBC
	private void Awake()
	{
	}

	// Token: 0x06003BAC RID: 15276 RVA: 0x0011C7BE File Offset: 0x0011ABBE
	private void OnDisable()
	{
		if (this.m_bOver)
		{
			this.Leave();
		}
	}

	// Token: 0x06003BAD RID: 15277 RVA: 0x0011C7D1 File Offset: 0x0011ABD1
	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		this.Enter();
	}

	// Token: 0x06003BAE RID: 15278 RVA: 0x0011C7D9 File Offset: 0x0011ABD9
	public virtual void OnPointerExit(PointerEventData eventData)
	{
		this.Leave();
	}

	// Token: 0x06003BAF RID: 15279 RVA: 0x0011C7E1 File Offset: 0x0011ABE1
	private void Enter()
	{
		this.m_bOver = true;
		if (this.m_OnEnter != null)
		{
			this.m_OnEnter.Invoke();
		}
	}

	// Token: 0x06003BB0 RID: 15280 RVA: 0x0011C800 File Offset: 0x0011AC00
	private void Leave()
	{
		this.m_bOver = false;
		if (this.m_OnLeave != null)
		{
			this.m_OnLeave.Invoke();
		}
	}

	// Token: 0x04003080 RID: 12416
	public UnityEvent m_OnEnter;

	// Token: 0x04003081 RID: 12417
	public UnityEvent m_OnLeave;

	// Token: 0x04003082 RID: 12418
	private bool m_bOver;
}
