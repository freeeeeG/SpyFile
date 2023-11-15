using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000809 RID: 2057
public class ServerLimitedQuantityItem : ServerSynchroniserBase
{
	// Token: 0x06002750 RID: 10064 RVA: 0x000B93A4 File Offset: 0x000B77A4
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_baseObject = (LimitedQuantityItem)synchronisedObject;
		LimitedQuantityItemManager limitedQuantityItemManager = GameUtils.RequireManager<LimitedQuantityItemManager>();
		this.m_Manager = limitedQuantityItemManager.GetComponent<ServerLimitedQuantityItemManager>();
		this.m_Manager.AddItemToList(this);
		base.StartSynchronising(synchronisedObject);
	}

	// Token: 0x06002751 RID: 10065 RVA: 0x000B93E2 File Offset: 0x000B77E2
	public bool IsInvincible()
	{
		return this.m_InvincibilityConditions.CallForResult(true);
	}

	// Token: 0x06002752 RID: 10066 RVA: 0x000B93F0 File Offset: 0x000B77F0
	public void AddInvincibilityCondition(Generic<bool> isInvincible)
	{
		this.m_InvincibilityConditions.Add(isInvincible);
	}

	// Token: 0x06002753 RID: 10067 RVA: 0x000B93FE File Offset: 0x000B77FE
	public void RemoveInvincibilityCondition(Generic<bool> isInvincible)
	{
		this.m_InvincibilityConditions.Remove(isInvincible);
	}

	// Token: 0x06002754 RID: 10068 RVA: 0x000B940D File Offset: 0x000B780D
	public void RegisterImpendingDestructionNotification(ImpendingDestructionCallback func)
	{
		this.m_baseObject.RegisterImpendingDestructionNotification(func);
	}

	// Token: 0x06002755 RID: 10069 RVA: 0x000B941B File Offset: 0x000B781B
	public void UnregisterImpendingDestructionNotification(ImpendingDestructionCallback func)
	{
		this.m_baseObject.UnregisterImpendingDestructionNotification(func);
	}

	// Token: 0x06002756 RID: 10070 RVA: 0x000B9429 File Offset: 0x000B7829
	public void AddDestructionScoreModifier(Generic<float> modifier)
	{
		this.m_ScoreModifiers.Add(modifier);
	}

	// Token: 0x06002757 RID: 10071 RVA: 0x000B9437 File Offset: 0x000B7837
	public void RemoveDestructionScoreModifier(Generic<float> modifier)
	{
		this.m_ScoreModifiers.Remove(modifier);
	}

	// Token: 0x06002758 RID: 10072 RVA: 0x000B9448 File Offset: 0x000B7848
	public float GetDestructionScore()
	{
		float num = ClientTime.Time() - this.m_fLastActivity;
		float num2 = 0f;
		for (int i = 0; i < this.m_ScoreModifiers.Count; i++)
		{
			num2 += this.m_ScoreModifiers[i]();
		}
		return num + num2;
	}

	// Token: 0x06002759 RID: 10073 RVA: 0x000B949B File Offset: 0x000B789B
	public void Start()
	{
		this.Touch();
	}

	// Token: 0x0600275A RID: 10074 RVA: 0x000B94A3 File Offset: 0x000B78A3
	public override void OnDestroy()
	{
		this.m_InvincibilityConditions = null;
		if (this.m_Manager != null)
		{
			this.m_Manager.RemoveItemFromList(this);
		}
		base.OnDestroy();
	}

	// Token: 0x0600275B RID: 10075 RVA: 0x000B94CF File Offset: 0x000B78CF
	public void Touch()
	{
		this.m_fLastActivity = ClientTime.Time();
	}

	// Token: 0x04001EE4 RID: 7908
	private LimitedQuantityItem m_baseObject;

	// Token: 0x04001EE5 RID: 7909
	private ServerLimitedQuantityItemManager m_Manager;

	// Token: 0x04001EE6 RID: 7910
	private float m_fLastActivity;

	// Token: 0x04001EE7 RID: 7911
	private List<Generic<bool>> m_InvincibilityConditions = new List<Generic<bool>>();

	// Token: 0x04001EE8 RID: 7912
	private List<Generic<float>> m_ScoreModifiers = new List<Generic<float>>();
}
