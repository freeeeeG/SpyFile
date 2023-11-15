using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000628 RID: 1576
public class ServerWashable : ServerSynchroniserBase
{
	// Token: 0x06001DD2 RID: 7634 RVA: 0x00090B86 File Offset: 0x0008EF86
	public override EntityType GetEntityType()
	{
		return EntityType.Washable;
	}

	// Token: 0x06001DD3 RID: 7635 RVA: 0x00090B8A File Offset: 0x0008EF8A
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_washable = (Washable)synchronisedObject;
		this.m_washDuration = (float)this.m_washable.m_duration;
		this.m_durationMultiplier = this.m_washable.GetWashTimeMultiplier();
	}

	// Token: 0x06001DD4 RID: 7636 RVA: 0x00090BC2 File Offset: 0x0008EFC2
	private void SynchroniseWashingProgress(float _progress, float _target)
	{
		this.m_data.Initialise_Progress(_progress, _target);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x00090BDD File Offset: 0x0008EFDD
	private void SynchroniseWashingRate(float _rate)
	{
		this.m_data.Initialise_Rate(_rate, this.m_progress);
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001DD6 RID: 7638 RVA: 0x00090BFD File Offset: 0x0008EFFD
	public static List<ServerWashable> GetAllWashable()
	{
		return ServerWashable.s_allWashables;
	}

	// Token: 0x06001DD7 RID: 7639 RVA: 0x00090C04 File Offset: 0x0008F004
	public void RegisterAllowWashCallback(Generic<bool> _callback)
	{
		this.m_allowWashCallbacks.Add(_callback);
	}

	// Token: 0x06001DD8 RID: 7640 RVA: 0x00090C12 File Offset: 0x0008F012
	public void UnregisterAllowWashCallback(Generic<bool> _callback)
	{
		this.m_allowWashCallbacks.Remove(_callback);
	}

	// Token: 0x06001DD9 RID: 7641 RVA: 0x00090C21 File Offset: 0x0008F021
	public void RegisterFinishedCallback(CallbackVoid _callback)
	{
		this.m_finishedCallback = (CallbackVoid)Delegate.Combine(this.m_finishedCallback, _callback);
	}

	// Token: 0x06001DDA RID: 7642 RVA: 0x00090C3A File Offset: 0x0008F03A
	public void UnregisterFinishedCallback(CallbackVoid _callback)
	{
		this.m_finishedCallback = (CallbackVoid)Delegate.Remove(this.m_finishedCallback, _callback);
	}

	// Token: 0x06001DDB RID: 7643 RVA: 0x00090C54 File Offset: 0x0008F054
	private void WashingFinishedAchievement()
	{
		for (int i = 0; i < this.m_activeWashers.Count; i++)
		{
			ServerWashable.WashData washData = this.m_activeWashers[i];
			if (washData.Source != null && washData.Source.GetType() == typeof(ServerWaterGunSpray))
			{
				ServerWaterGunSpray serverWaterGunSpray = (ServerWaterGunSpray)washData.Source;
				GameObject carrier = serverWaterGunSpray.Carrier;
				if (carrier != null)
				{
					ServerStack serverStack = base.gameObject.RequestComponent<ServerStack>();
					ServerMessenger.Achievement(carrier, 101, (!(serverStack != null)) ? 1 : serverStack.GetSize());
				}
			}
		}
	}

	// Token: 0x06001DDC RID: 7644 RVA: 0x00090D04 File Offset: 0x0008F104
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (!this.HasFinished())
		{
			float num = this.m_washDuration * (float)this.m_durationMultiplier;
			float num2 = this.CalculateWashRate();
			if (num2 > 0f || num2 < 0f)
			{
				this.m_progress += num2 * TimeManager.GetDeltaTime(base.gameObject.layer);
				this.m_progress = Mathf.Clamp(this.m_progress, 0f, num);
			}
			if (this.m_progress >= num)
			{
				this.m_finishedCallback();
				this.m_hasFinished = true;
				this.SynchroniseWashingProgress(this.m_progress, this.m_washDuration);
				this.WashingFinishedAchievement();
			}
			else if (num2 != this.m_previousWashRate)
			{
				this.SynchroniseWashingRate(num2);
				this.m_previousWashRate = num2;
			}
		}
	}

	// Token: 0x06001DDD RID: 7645 RVA: 0x00090DDC File Offset: 0x0008F1DC
	private float CalculateWashRate()
	{
		if (!this.m_allowWashCallbacks.CallForResult(false))
		{
			float num = 0f;
			for (int i = 0; i < this.m_activeWashers.Count; i++)
			{
				num += this.m_activeWashers[i].Rate;
			}
			return num;
		}
		return 0f;
	}

	// Token: 0x06001DDE RID: 7646 RVA: 0x00090E3C File Offset: 0x0008F23C
	public void StartWashing(object _source, float _rate)
	{
		if (!this.m_activeWashers.Exists((ServerWashable.WashData x) => x.Source == _source))
		{
			this.m_activeWashers.Add(new ServerWashable.WashData
			{
				Source = _source,
				Rate = _rate
			});
		}
	}

	// Token: 0x06001DDF RID: 7647 RVA: 0x00090E9C File Offset: 0x0008F29C
	public void StopWashing(object _source)
	{
		this.m_activeWashers.RemoveAll((ServerWashable.WashData x) => x.Source == _source);
	}

	// Token: 0x06001DE0 RID: 7648 RVA: 0x00090ECE File Offset: 0x0008F2CE
	public void SetDuration(float _duration)
	{
		if (this.m_washDuration != _duration)
		{
			this.m_washDuration = _duration;
			this.SynchroniseWashingProgress(this.m_progress, this.m_washDuration);
		}
	}

	// Token: 0x06001DE1 RID: 7649 RVA: 0x00090EF5 File Offset: 0x0008F2F5
	public bool HasFinished()
	{
		return this.m_progress >= this.m_washDuration * (float)this.m_durationMultiplier;
	}

	// Token: 0x06001DE2 RID: 7650 RVA: 0x00090F10 File Offset: 0x0008F310
	private void Awake()
	{
		Mailbox.Server.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		if (!ServerWashable.s_allWashables.Contains(this))
		{
			ServerWashable.s_allWashables.Add(this);
		}
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x00090F45 File Offset: 0x0008F345
	public override void OnDestroy()
	{
		base.OnDestroy();
		Mailbox.Server.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		ServerWashable.s_allWashables.Remove(this);
	}

	// Token: 0x06001DE4 RID: 7652 RVA: 0x00090F74 File Offset: 0x0008F374
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.m_durationMultiplier = this.m_washable.GetWashTimeMultiplier();
		}
	}

	// Token: 0x04001702 RID: 5890
	private Washable m_washable;

	// Token: 0x04001703 RID: 5891
	private WashableMessage m_data = new WashableMessage();

	// Token: 0x04001704 RID: 5892
	private static List<ServerWashable> s_allWashables = new List<ServerWashable>();

	// Token: 0x04001705 RID: 5893
	private float m_washDuration;

	// Token: 0x04001706 RID: 5894
	private int m_durationMultiplier = 1;

	// Token: 0x04001707 RID: 5895
	private List<ServerWashable.WashData> m_activeWashers = new List<ServerWashable.WashData>();

	// Token: 0x04001708 RID: 5896
	private float m_progress;

	// Token: 0x04001709 RID: 5897
	private bool m_hasFinished;

	// Token: 0x0400170A RID: 5898
	private float m_previousWashRate;

	// Token: 0x0400170B RID: 5899
	private List<Generic<bool>> m_allowWashCallbacks = new List<Generic<bool>>();

	// Token: 0x0400170C RID: 5900
	private CallbackVoid m_finishedCallback = delegate()
	{
	};

	// Token: 0x02000629 RID: 1577
	private struct WashData
	{
		// Token: 0x0400170E RID: 5902
		public object Source;

		// Token: 0x0400170F RID: 5903
		public float Rate;
	}
}
