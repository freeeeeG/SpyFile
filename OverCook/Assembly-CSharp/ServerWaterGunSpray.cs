using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000623 RID: 1571
public class ServerWaterGunSpray : ServerFireExtinguishSpray, IWindSource
{
	// Token: 0x06001DC1 RID: 7617 RVA: 0x000907F3 File Offset: 0x0008EBF3
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_waterGunSpray = (WaterGunSpray)synchronisedObject;
	}

	// Token: 0x06001DC2 RID: 7618 RVA: 0x00090808 File Offset: 0x0008EC08
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (base.IsSpraying())
		{
			List<ServerWashable> allWashable = ServerWashable.GetAllWashable();
			List<WindAccumulator> allWindReceivers = WindAccumulator.GetAllWindReceivers();
			this.UpdateSprayableCollection<ServerWashable>(ref this.m_activeWashables, allWashable, new GenericVoid<ServerWashable>(this.StartWashing), new GenericVoid<ServerWashable>(this.StopWashing));
			this.UpdateSprayableCollection<WindAccumulator>(ref this.m_activeWindReceivers, allWindReceivers, new GenericVoid<WindAccumulator>(this.StartKnockback), new GenericVoid<WindAccumulator>(this.StopKnockback));
		}
		else
		{
			if (this.m_activeWashables.Count > 0)
			{
				this.ClearSprayableCollection<ServerWashable>(ref this.m_activeWashables, new GenericVoid<ServerWashable>(this.StopWashing));
			}
			if (this.m_activeWindReceivers.Count > 0)
			{
				this.ClearSprayableCollection<WindAccumulator>(ref this.m_activeWindReceivers, new GenericVoid<WindAccumulator>(this.StopKnockback));
			}
		}
	}

	// Token: 0x06001DC3 RID: 7619 RVA: 0x000908D4 File Offset: 0x0008ECD4
	private void UpdateSprayableCollection<T>(ref List<T> _activeSprayables, List<T> _possibleSprayables, GenericVoid<T> _enterCallback, GenericVoid<T> _exitCallback) where T : Component
	{
		for (int i = 0; i < _possibleSprayables.Count; i++)
		{
			T t = _possibleSprayables[i];
			int num = _activeSprayables.IndexOf(t);
			bool flag = base.IsInSpray(t.transform);
			bool flag2 = num >= 0;
			if (!flag2 && flag)
			{
				_activeSprayables.Add(t);
				_enterCallback(_possibleSprayables[i]);
			}
			else if (flag2 && !flag)
			{
				_activeSprayables.RemoveAt(num);
				_exitCallback(t);
			}
		}
	}

	// Token: 0x06001DC4 RID: 7620 RVA: 0x0009096C File Offset: 0x0008ED6C
	private void ClearSprayableCollection<T>(ref List<T> _activeSprayables, GenericVoid<T> _removeCallback)
	{
		if (_activeSprayables.Count > 0)
		{
			for (int i = 0; i < _activeSprayables.Count; i++)
			{
				_removeCallback(_activeSprayables[i]);
			}
			_activeSprayables.Clear();
		}
	}

	// Token: 0x06001DC5 RID: 7621 RVA: 0x000909B3 File Offset: 0x0008EDB3
	private void StartWashing(ServerWashable _washable)
	{
		_washable.StartWashing(this, this.m_waterGunSpray.m_washSpeed);
	}

	// Token: 0x06001DC6 RID: 7622 RVA: 0x000909C7 File Offset: 0x0008EDC7
	private void StopWashing(ServerWashable _washable)
	{
		_washable.StopWashing(this);
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x000909D0 File Offset: 0x0008EDD0
	private void StartKnockback(WindAccumulator _windReceiver)
	{
		_windReceiver.AddWindSource(this);
	}

	// Token: 0x06001DC8 RID: 7624 RVA: 0x000909D9 File Offset: 0x0008EDD9
	private void StopKnockback(WindAccumulator _windReceiver)
	{
		_windReceiver.RemoveWindSource(this);
	}

	// Token: 0x06001DC9 RID: 7625 RVA: 0x000909E2 File Offset: 0x0008EDE2
	public Vector3 GetVelocity()
	{
		return base.transform.forward * this.m_waterGunSpray.m_knockbackForce;
	}

	// Token: 0x040016F5 RID: 5877
	private WaterGunSpray m_waterGunSpray;

	// Token: 0x040016F6 RID: 5878
	private List<ServerWashable> m_activeWashables = new List<ServerWashable>();

	// Token: 0x040016F7 RID: 5879
	private List<WindAccumulator> m_activeWindReceivers = new List<WindAccumulator>();
}
