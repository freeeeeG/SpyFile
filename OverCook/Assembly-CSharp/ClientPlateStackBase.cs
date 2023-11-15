using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200054E RID: 1358
public class ClientPlateStackBase : ClientSynchroniserBase
{
	// Token: 0x06001995 RID: 6549 RVA: 0x00070141 File Offset: 0x0006E541
	public void RegisterOnPlateAdded(GenericVoid<GameObject> _added)
	{
		this.m_plateAdded = (GenericVoid<GameObject>)Delegate.Combine(this.m_plateAdded, _added);
	}

	// Token: 0x06001996 RID: 6550 RVA: 0x0007015A File Offset: 0x0006E55A
	public void UnregisterOnPlateAdded(GenericVoid<GameObject> _added)
	{
		this.m_plateAdded = (GenericVoid<GameObject>)Delegate.Remove(this.m_plateAdded, _added);
	}

	// Token: 0x06001997 RID: 6551 RVA: 0x00070173 File Offset: 0x0006E573
	public void RegisterOnPlateRemoved(GenericVoid<GameObject> _removed)
	{
		this.m_plateRemoved = (GenericVoid<GameObject>)Delegate.Combine(this.m_plateRemoved, _removed);
	}

	// Token: 0x06001998 RID: 6552 RVA: 0x0007018C File Offset: 0x0006E58C
	public void UnregisterOnPlateRemoved(GenericVoid<GameObject> _removed)
	{
		this.m_plateRemoved = (GenericVoid<GameObject>)Delegate.Remove(this.m_plateRemoved, _removed);
	}

	// Token: 0x06001999 RID: 6553 RVA: 0x000701A5 File Offset: 0x0006E5A5
	public override EntityType GetEntityType()
	{
		return EntityType.PlateStack;
	}

	// Token: 0x0600199A RID: 6554 RVA: 0x000701AC File Offset: 0x0006E5AC
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_plateStack = (PlateStackBase)synchronisedObject;
		this.m_stack = base.gameObject.RequireComponent<ClientStack>();
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_plateStack.m_platePrefab, new VoidGeneric<GameObject>(this.PlateSpawned));
	}

	// Token: 0x0600199B RID: 6555 RVA: 0x00070200 File Offset: 0x0006E600
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		this.PlateRemoved();
	}

	// Token: 0x0600199C RID: 6556 RVA: 0x00070208 File Offset: 0x0006E608
	protected virtual void PlateSpawned(GameObject _object)
	{
		this.m_plateAdded(_object);
	}

	// Token: 0x0600199D RID: 6557 RVA: 0x00070216 File Offset: 0x0006E616
	protected virtual void PlateRemoved()
	{
		this.m_plateRemoved(null);
	}

	// Token: 0x0600199E RID: 6558 RVA: 0x00070224 File Offset: 0x0006E624
	protected void NotifyPlateAdded(GameObject _plate)
	{
		this.m_plateAdded(_plate);
	}

	// Token: 0x0600199F RID: 6559 RVA: 0x00070232 File Offset: 0x0006E632
	protected void NotifyPlateRemoved(GameObject _plate)
	{
		this.m_plateRemoved(_plate);
	}

	// Token: 0x060019A0 RID: 6560 RVA: 0x00070240 File Offset: 0x0006E640
	public int GetCount()
	{
		if (this.m_stack != null)
		{
			return this.m_stack.GetSize();
		}
		return 0;
	}

	// Token: 0x0400144E RID: 5198
	protected PlateStackBase m_plateStack;

	// Token: 0x0400144F RID: 5199
	private GenericVoid<GameObject> m_plateAdded = delegate(GameObject _plate)
	{
	};

	// Token: 0x04001450 RID: 5200
	private GenericVoid<GameObject> m_plateRemoved = delegate(GameObject _plate)
	{
	};

	// Token: 0x04001451 RID: 5201
	protected ClientStack m_stack;
}
