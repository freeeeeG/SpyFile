using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000605 RID: 1541
public class ServerFireExtinguishSpray : ServerSprayingUtensil
{
	// Token: 0x06001D43 RID: 7491 RVA: 0x0008FA90 File Offset: 0x0008DE90
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_FireExtinguishSpray = (FireExtinguishSpray)synchronisedObject;
	}

	// Token: 0x06001D44 RID: 7492 RVA: 0x0008FAA8 File Offset: 0x0008DEA8
	public override void OnCarryBegun(ICarrier _carrier)
	{
		base.OnCarryBegun(_carrier);
		ServerFlammable serverFlammable = base.Carrier.RequestComponent<ServerFlammable>();
		if (serverFlammable != null)
		{
			serverFlammable.SetCanCatchFire(false);
		}
	}

	// Token: 0x06001D45 RID: 7493 RVA: 0x0008FADC File Offset: 0x0008DEDC
	public override void OnCarryEnded(ICarrier _carrier)
	{
		if (base.Carrier != null)
		{
			ServerFlammable serverFlammable = base.Carrier.RequestComponent<ServerFlammable>();
			if (serverFlammable != null)
			{
				serverFlammable.SetCanCatchFire(true);
			}
		}
		base.OnCarryEnded(_carrier);
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x0008FB20 File Offset: 0x0008DF20
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (base.IsSpraying())
		{
			List<ServerFlammable> list = new List<ServerFlammable>();
			foreach (ServerFlammable serverFlammable in ServerFlammable.GetAllOnFire())
			{
				if (base.IsInSpray(serverFlammable.transform))
				{
					list.Add(serverFlammable);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				ServerFlammable serverFlammable2 = list[i];
				serverFlammable2.FightFire(this.m_FireExtinguishSpray.m_exinguishTime, TimeManager.GetDeltaTime(base.gameObject), true);
			}
		}
	}

	// Token: 0x040016B6 RID: 5814
	protected FireExtinguishSpray m_FireExtinguishSpray;
}
