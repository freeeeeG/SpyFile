using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200016F RID: 367
public class ServerTriggerAnimatorSetVariable : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x06000672 RID: 1650 RVA: 0x0002CEC6 File Offset: 0x0002B2C6
	public override EntityType GetEntityType()
	{
		return EntityType.AnimatorVariable;
	}

	// Token: 0x06000673 RID: 1651 RVA: 0x0002CECA File Offset: 0x0002B2CA
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerAnimatorVariable = (TriggerAnimatorSetVariable)synchronisedObject;
	}

	// Token: 0x06000674 RID: 1652 RVA: 0x0002CEE0 File Offset: 0x0002B2E0
	private void SendTriggerMessage()
	{
		if (this.m_triggerAnimatorVariable.m_randomValue)
		{
			AnimatorVariableType variableType = this.m_triggerAnimatorVariable.m_variableType;
			if (variableType != AnimatorVariableType.Bool)
			{
				if (variableType != AnimatorVariableType.Float)
				{
					if (variableType == AnimatorVariableType.Int)
					{
						this.m_data.InitRandomInt(this.m_triggerAnimatorVariable.m_minIntValue, this.m_triggerAnimatorVariable.m_maxIntValue);
					}
				}
				else
				{
					this.m_data.InitRandomFloat(this.m_triggerAnimatorVariable.m_minFloatValue, this.m_triggerAnimatorVariable.m_maxFloatValue);
				}
			}
			else
			{
				this.m_data.InitRandomBool();
			}
		}
		else
		{
			this.m_data.Initialise();
		}
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06000675 RID: 1653 RVA: 0x0002CF9C File Offset: 0x0002B39C
	public override void UpdateSynchronising()
	{
		if (this.m_triggerAnimatorVariable == null || !this.m_triggerAnimatorVariable.enabled)
		{
			return;
		}
		if (this.m_triggerAnimatorVariable.m_onAwake)
		{
			this.m_triggerAnimatorVariable.m_onAwake = false;
			this.SendTriggerMessage();
		}
	}

	// Token: 0x06000676 RID: 1654 RVA: 0x0002CFED File Offset: 0x0002B3ED
	public void OnTrigger(string _trigger)
	{
		if (this.m_triggerAnimatorVariable.enabled && this.m_triggerAnimatorVariable.m_triggerToReceive == _trigger)
		{
			this.SendTriggerMessage();
		}
	}

	// Token: 0x0400055F RID: 1375
	private TriggerAnimatorSetVariable m_triggerAnimatorVariable;

	// Token: 0x04000560 RID: 1376
	private TriggerAnimatorVariableMessage m_data = new TriggerAnimatorVariableMessage();
}
