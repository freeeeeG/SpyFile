using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200056C RID: 1388
public class ServerStack : ServerSynchroniserBase
{
	// Token: 0x06001A20 RID: 6688 RVA: 0x00082D18 File Offset: 0x00081118
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_stack = (synchronisedObject as Stack);
		this.m_boxCollider = base.gameObject.GetComponent<BoxCollider>();
		Transform transform = base.transform;
		int childCount = transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);
			if (child.gameObject.tag == this.m_stack.m_itemTag)
			{
				this.m_stackItems.Add(child.gameObject);
			}
		}
		this.m_stack.RefreshStackTransforms(ref this.m_stackItems);
		this.m_stack.RefreshStackCollider(ref this.m_boxCollider, this.m_stackItems.Count);
	}

	// Token: 0x06001A21 RID: 6689 RVA: 0x00082DD0 File Offset: 0x000811D0
	public void AddToStack(GameObject _item)
	{
		if (this.m_stackItems.Contains(_item))
		{
			return;
		}
		IAttachment attachment = _item.RequestInterface<IAttachment>();
		if (attachment != null)
		{
			attachment.Attach(this.m_stack);
		}
		else
		{
			_item.transform.SetParent(this.m_stack.GetAttachPoint(_item));
		}
		this.m_stackItems.Add(_item);
		this.m_stack.RefreshStackTransforms(ref this.m_stackItems);
		this.m_stack.RefreshStackCollider(ref this.m_boxCollider, this.m_stackItems.Count);
	}

	// Token: 0x06001A22 RID: 6690 RVA: 0x00082E60 File Offset: 0x00081260
	public GameObject RemoveFromStack()
	{
		if (this.m_stackItems.Count > 0)
		{
			GameObject gameObject = this.m_stackItems[this.m_stackItems.Count - 1];
			this.m_stackItems.Remove(gameObject);
			IAttachment attachment = gameObject.RequestInterface<IAttachment>();
			if (attachment != null)
			{
				attachment.Detach();
			}
			else
			{
				gameObject.transform.SetParent(null);
			}
			this.m_stack.RefreshStackTransforms(ref this.m_stackItems);
			this.m_stack.RefreshStackCollider(ref this.m_boxCollider, this.m_stackItems.Count);
			return gameObject;
		}
		return null;
	}

	// Token: 0x06001A23 RID: 6691 RVA: 0x00082EF8 File Offset: 0x000812F8
	public int GetSize()
	{
		return this.m_stackItems.Count;
	}

	// Token: 0x06001A24 RID: 6692 RVA: 0x00082F05 File Offset: 0x00081305
	public GameObject InspectTopOfStack()
	{
		if (this.m_stackItems.Count > 0)
		{
			return this.m_stackItems[this.m_stackItems.Count - 1];
		}
		return null;
	}

	// Token: 0x040014C0 RID: 5312
	private Stack m_stack;

	// Token: 0x040014C1 RID: 5313
	private List<GameObject> m_stackItems = new List<GameObject>();

	// Token: 0x040014C2 RID: 5314
	private BoxCollider m_boxCollider;
}
