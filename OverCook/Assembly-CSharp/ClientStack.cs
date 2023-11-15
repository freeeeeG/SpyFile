using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200056D RID: 1389
public class ClientStack : ClientSynchroniserBase
{
	// Token: 0x06001A26 RID: 6694 RVA: 0x00082F48 File Offset: 0x00081348
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_stack = (Stack)synchronisedObject;
		this.m_boxCollider = base.gameObject.GetComponent<BoxCollider>();
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.CompareTag(this.m_stack.m_itemTag))
			{
				this.m_stackItems.Add(base.transform.GetChild(i).gameObject);
			}
		}
		this.m_stack.RefreshStackTransforms(ref this.m_stackItems);
		this.m_stack.RefreshStackCollider(ref this.m_boxCollider, this.m_stackItems.Count);
	}

	// Token: 0x06001A27 RID: 6695 RVA: 0x00083004 File Offset: 0x00081404
	public void AddToStack(GameObject _item)
	{
		if (this.m_stackItems.Contains(_item))
		{
			return;
		}
		if (_item.RequestInterface<IClientAttachment>() == null)
		{
			_item.transform.SetParent(this.m_stack.GetAttachPoint(_item));
		}
		this.m_stackItems.Add(_item);
		this.m_stack.RefreshStackTransforms(ref this.m_stackItems);
		this.m_stack.RefreshStackCollider(ref this.m_boxCollider, this.m_stackItems.Count);
	}

	// Token: 0x06001A28 RID: 6696 RVA: 0x00083080 File Offset: 0x00081480
	public GameObject RemoveFromStack()
	{
		if (this.m_stackItems.Count > 0)
		{
			GameObject gameObject = this.m_stackItems[this.m_stackItems.Count - 1];
			this.m_stackItems.Remove(gameObject);
			if (gameObject.RequestInterface<IClientAttachment>() == null)
			{
				gameObject.transform.SetParent(null);
			}
			this.m_stack.RefreshStackTransforms(ref this.m_stackItems);
			this.m_stack.RefreshStackCollider(ref this.m_boxCollider, this.m_stackItems.Count);
			return gameObject;
		}
		return null;
	}

	// Token: 0x06001A29 RID: 6697 RVA: 0x0008310D File Offset: 0x0008150D
	public int GetSize()
	{
		return this.m_stackItems.Count;
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x0008311A File Offset: 0x0008151A
	public GameObject InspectTopOfStack()
	{
		if (this.m_stackItems.Count > 0)
		{
			return this.m_stackItems[this.m_stackItems.Count - 1];
		}
		return null;
	}

	// Token: 0x040014C3 RID: 5315
	private Stack m_stack;

	// Token: 0x040014C4 RID: 5316
	private List<GameObject> m_stackItems = new List<GameObject>();

	// Token: 0x040014C5 RID: 5317
	private BoxCollider m_boxCollider;
}
