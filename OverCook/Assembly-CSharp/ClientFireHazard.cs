using System;
using UnityEngine;

// Token: 0x02000493 RID: 1171
public class ClientFireHazard : ClientHazardBase
{
	// Token: 0x060015EE RID: 5614 RVA: 0x00075284 File Offset: 0x00073684
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_hazard = (FireHazard)synchronisedObject;
		this.m_flammable = base.gameObject.RequireComponent<ClientFlammable>();
		this.m_flammable.RegisterIgnitionCallback(new ClientFlammable.IgnitionCallback(this.OnIgnitionChange));
	}

	// Token: 0x060015EF RID: 5615 RVA: 0x000752C1 File Offset: 0x000736C1
	protected virtual void Awake()
	{
		this.m_collider = base.gameObject.RequireComponent<Collider>();
		this.m_collider.enabled = false;
	}

	// Token: 0x060015F0 RID: 5616 RVA: 0x000752E0 File Offset: 0x000736E0
	private void SetHazardActive(bool _active)
	{
		if (this.m_collider != null)
		{
			this.m_collider.enabled = _active;
		}
	}

	// Token: 0x060015F1 RID: 5617 RVA: 0x000752FF File Offset: 0x000736FF
	private void OnIgnitionChange(bool _state)
	{
		this.SetHazardActive(_state);
	}

	// Token: 0x04001092 RID: 4242
	private FireHazard m_hazard;

	// Token: 0x04001093 RID: 4243
	private ClientFlammable m_flammable;

	// Token: 0x04001094 RID: 4244
	private Collider m_collider;
}
