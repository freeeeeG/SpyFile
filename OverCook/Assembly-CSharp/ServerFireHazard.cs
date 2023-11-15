using System;
using UnityEngine;

// Token: 0x02000492 RID: 1170
public class ServerFireHazard : ServerHazardBase
{
	// Token: 0x060015E7 RID: 5607 RVA: 0x00075144 File Offset: 0x00073544
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_hazard = (FireHazard)synchronisedObject;
		this.m_flammable = base.gameObject.RequireComponent<ServerFlammable>();
		this.m_flammable.RegisterIgnitionCallback(new ServerFlammable.IgnitionCallback(this.OnIgnitionChange));
	}

	// Token: 0x060015E8 RID: 5608 RVA: 0x00075184 File Offset: 0x00073584
	public override void UpdateSynchronising()
	{
		if (this.m_flammable == null)
		{
			return;
		}
		if (this.m_hazard.m_lifetime > 0f)
		{
			this.m_flammable.FightFire(this.m_hazard.m_lifetime, TimeManager.GetDeltaTime(base.gameObject), false);
		}
		if (this.m_destroyTimer > 0f)
		{
			this.m_destroyTimer -= TimeManager.GetDeltaTime(base.gameObject);
			if (this.m_destroyTimer <= 0f)
			{
				NetworkUtils.DestroyObject(base.gameObject);
			}
		}
	}

	// Token: 0x060015E9 RID: 5609 RVA: 0x0007521D File Offset: 0x0007361D
	public void Conflagrate()
	{
		this.m_flammable.Ignite();
		this.m_destroyTimer = 0f;
	}

	// Token: 0x060015EA RID: 5610 RVA: 0x00075235 File Offset: 0x00073635
	private void Shutdown()
	{
		this.m_destroyTimer = this.m_hazard.m_destroyTime;
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x00075248 File Offset: 0x00073648
	public override void HandleTransfer(GridIndex _index, GameObject _object)
	{
		if (_object.RequestComponent<FireHazard>() == null)
		{
			this.m_flammable.Extinguish();
		}
	}

	// Token: 0x060015EC RID: 5612 RVA: 0x00075266 File Offset: 0x00073666
	private void OnIgnitionChange(bool _state)
	{
		if (!_state)
		{
			this.Shutdown();
		}
	}

	// Token: 0x0400108F RID: 4239
	private FireHazard m_hazard;

	// Token: 0x04001090 RID: 4240
	private ServerFlammable m_flammable;

	// Token: 0x04001091 RID: 4241
	private float m_destroyTimer;
}
