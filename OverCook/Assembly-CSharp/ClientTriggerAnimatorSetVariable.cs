using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000170 RID: 368
public class ClientTriggerAnimatorSetVariable : ClientSynchroniserBase
{
	// Token: 0x06000678 RID: 1656 RVA: 0x0002D023 File Offset: 0x0002B423
	public override EntityType GetEntityType()
	{
		return EntityType.AnimatorVariable;
	}

	// Token: 0x06000679 RID: 1657 RVA: 0x0002D027 File Offset: 0x0002B427
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerAnimatorVariable = (TriggerAnimatorSetVariable)synchronisedObject;
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0002D03C File Offset: 0x0002B43C
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		TriggerAnimatorVariableMessage triggerAnimatorVariableMessage = (TriggerAnimatorVariableMessage)serialisable;
		if (triggerAnimatorVariableMessage != null)
		{
			AnimatorUtils.SetValue(this.m_triggerAnimatorVariable.m_targetAnimator, this.m_triggerAnimatorVariable.m_variableNameHash, this.m_triggerAnimatorVariable.m_variableType, this.GetValue(triggerAnimatorVariableMessage));
		}
	}

	// Token: 0x0600067B RID: 1659 RVA: 0x0002D084 File Offset: 0x0002B484
	public object GetValue(TriggerAnimatorVariableMessage _data)
	{
		AnimatorVariableType variableType = this.m_triggerAnimatorVariable.m_variableType;
		if (variableType != AnimatorVariableType.Bool)
		{
			if (variableType != AnimatorVariableType.Int)
			{
				if (variableType != AnimatorVariableType.Float)
				{
					return null;
				}
				if (this.m_triggerAnimatorVariable.m_randomValue && _data.Type != TriggerAnimatorVariableMessage.RandomValueType.None)
				{
					return _data.m_randomValue;
				}
				return this.m_triggerAnimatorVariable.m_floatValue;
			}
			else
			{
				if (this.m_triggerAnimatorVariable.m_randomValue && _data.Type != TriggerAnimatorVariableMessage.RandomValueType.None)
				{
					return _data.m_randomValue;
				}
				return this.m_triggerAnimatorVariable.m_intValue;
			}
		}
		else
		{
			if (this.m_triggerAnimatorVariable.m_randomValue && _data.Type != TriggerAnimatorVariableMessage.RandomValueType.None)
			{
				return _data.m_randomValue;
			}
			return this.m_triggerAnimatorVariable.m_boolValue;
		}
	}

	// Token: 0x04000561 RID: 1377
	private TriggerAnimatorSetVariable m_triggerAnimatorVariable;
}
