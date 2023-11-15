using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000195 RID: 405
public class ServerTriggerTimer : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x060006E8 RID: 1768 RVA: 0x0002DDFC File Offset: 0x0002C1FC
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_triggerTimer = (TriggerTimer)synchronisedObject;
		if ((this.m_triggerTimer.m_triggerAtStart || this.m_triggerTimer.m_startTiming) && base.gameObject.activeInHierarchy)
		{
			this.m_iFlowController = GameUtils.GetFlowController();
			if (this.m_iFlowController != null)
			{
				this.m_iFlowController.RoundActivatedCallback += this.OnRoundBegun;
				this.m_intitialised = false;
			}
		}
		else
		{
			this.m_intitialised = true;
		}
	}

	// Token: 0x060006E9 RID: 1769 RVA: 0x0002DE8C File Offset: 0x0002C28C
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_iFlowController != null)
		{
			this.m_iFlowController.RoundActivatedCallback -= this.OnRoundBegun;
		}
	}

	// Token: 0x060006EA RID: 1770 RVA: 0x0002DEB6 File Offset: 0x0002C2B6
	private void OnRoundBegun()
	{
		this.m_intitialised = true;
	}

	// Token: 0x060006EB RID: 1771 RVA: 0x0002DEBF File Offset: 0x0002C2BF
	protected override void OnEnable()
	{
		base.OnEnable();
		this.m_triggeredAtStart = false;
		this.m_startedTiming = false;
	}

	// Token: 0x060006EC RID: 1772 RVA: 0x0002DED5 File Offset: 0x0002C2D5
	public void OnTrigger(string _trigger)
	{
		if (this.m_triggerTimer.m_startTrigger == _trigger)
		{
			this.m_timer = this.m_triggerTimer.m_time;
		}
	}

	// Token: 0x060006ED RID: 1773 RVA: 0x0002DF00 File Offset: 0x0002C300
	public override void UpdateSynchronising()
	{
		if (this.m_triggerTimer == null || !this.m_triggerTimer.enabled)
		{
			return;
		}
		if (this.m_intitialised)
		{
			if (this.m_triggerTimer.m_startTiming && !this.m_startedTiming)
			{
				this.m_timer = this.m_triggerTimer.m_time;
				this.m_startedTiming = true;
			}
			if (this.m_triggerTimer.m_triggerAtStart && !this.m_triggeredAtStart)
			{
				base.gameObject.SendTrigger(this.m_triggerTimer.m_completeTrigger);
				this.m_triggeredAtStart = true;
			}
		}
		if (this.m_timer > 0f)
		{
			this.m_timer -= TimeManager.GetDeltaTime(base.gameObject);
			if (this.m_timer <= 0f)
			{
				base.gameObject.SendTrigger(this.m_triggerTimer.m_completeTrigger);
			}
		}
	}

	// Token: 0x040005BA RID: 1466
	private TriggerTimer m_triggerTimer;

	// Token: 0x040005BB RID: 1467
	private IFlowController m_iFlowController;

	// Token: 0x040005BC RID: 1468
	private bool m_intitialised;

	// Token: 0x040005BD RID: 1469
	private float m_timer;

	// Token: 0x040005BE RID: 1470
	private bool m_triggeredAtStart;

	// Token: 0x040005BF RID: 1471
	private bool m_startedTiming;
}
