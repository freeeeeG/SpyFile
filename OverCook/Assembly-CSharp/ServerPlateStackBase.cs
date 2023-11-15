using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200054D RID: 1357
public class ServerPlateStackBase : ServerSynchroniserBase, IAddToStack
{
	// Token: 0x0600198E RID: 6542 RVA: 0x0006FE01 File Offset: 0x0006E201
	public override EntityType GetEntityType()
	{
		return EntityType.PlateStack;
	}

	// Token: 0x0600198F RID: 6543 RVA: 0x0006FE05 File Offset: 0x0006E205
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_plateStack = (PlateStackBase)synchronisedObject;
		this.m_stack = base.gameObject.RequireComponent<ServerStack>();
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_plateStack.m_platePrefab);
	}

	// Token: 0x06001990 RID: 6544 RVA: 0x0006FE44 File Offset: 0x0006E244
	protected virtual GameObject RemoveFromStack()
	{
		GameObject result = this.m_stack.RemoveFromStack();
		this.SendServerEvent(this.m_data);
		return result;
	}

	// Token: 0x06001991 RID: 6545 RVA: 0x0006FE6C File Offset: 0x0006E26C
	public virtual void AddToStack()
	{
		GameObject item = NetworkUtils.ServerSpawnPrefab(base.gameObject, this.m_plateStack.m_platePrefab);
		this.m_stack.AddToStack(item);
	}

	// Token: 0x06001992 RID: 6546 RVA: 0x0006FE9C File Offset: 0x0006E29C
	public int GetSize()
	{
		return this.m_stack.GetSize();
	}

	// Token: 0x06001993 RID: 6547 RVA: 0x0006FEA9 File Offset: 0x0006E2A9
	public PlatingStepData GetPlatingStep()
	{
		return this.m_plateStack.GetPlatingStep();
	}

	// Token: 0x0400144B RID: 5195
	protected PlateStackBase m_plateStack;

	// Token: 0x0400144C RID: 5196
	private PlateStackMessage m_data = new PlateStackMessage();

	// Token: 0x0400144D RID: 5197
	protected ServerStack m_stack;
}
