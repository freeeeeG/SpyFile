using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000608 RID: 1544
public class ServerFlamethrowerSpray : ServerSprayingUtensil
{
	// Token: 0x06001D4A RID: 7498 RVA: 0x0008FC03 File Offset: 0x0008E003
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_FlamethrowerSpray = (FlamethrowerSpray)synchronisedObject;
	}

	// Token: 0x06001D4B RID: 7499 RVA: 0x0008FC18 File Offset: 0x0008E018
	public override void UpdateSynchronising()
	{
		if (base.IsSpraying())
		{
			IEnumerable<ServerCookingHandler> cookingHandlers = ServerCookingHandler.GetCookingHandlers();
			foreach (ServerCookingHandler serverCookingHandler in cookingHandlers)
			{
				if (base.IsInSpray(serverCookingHandler.transform))
				{
					this.Cook(serverCookingHandler);
				}
			}
		}
		Generic<bool, KeyValuePair<AttachStation, ServerFlamethrowerSpray.StationSmouldering>> shouldRemove = delegate(KeyValuePair<AttachStation, ServerFlamethrowerSpray.StationSmouldering> _kvPair)
		{
			_kvPair.Value.LifeTime -= TimeManager.GetDeltaTime(base.gameObject);
			if (_kvPair.Value.LifeTime < 0f)
			{
				this.EndSmoulder(_kvPair.Value);
				return true;
			}
			return false;
		};
		ServerFlamethrowerSpray.m_smoulderLookup.RemoveAll(shouldRemove);
	}

	// Token: 0x06001D4C RID: 7500 RVA: 0x0008FCA8 File Offset: 0x0008E0A8
	private void EndSmoulder(ServerFlamethrowerSpray.StationSmouldering _smoulder)
	{
		UnityEngine.Object.Destroy(_smoulder.Smoulder);
		UnityEngine.Object.Destroy(_smoulder.Cooker);
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x0008FCC0 File Offset: 0x0008E0C0
	private void Cook(ServerCookingHandler _handler)
	{
		if (_handler.Cook(this.m_FlamethrowerSpray.m_cookingRate * TimeManager.GetDeltaTime(base.gameObject)))
		{
			AttachStation attachStation = _handler.gameObject.RequestComponentUpwardsRecursive<AttachStation>();
			if (attachStation != null)
			{
				if (attachStation.gameObject.RequestComponent<CookingStation>() == null)
				{
					ServerFlamethrowerSpray.StationSmouldering stationSmouldering = new ServerFlamethrowerSpray.StationSmouldering();
					stationSmouldering.LifeTime = this.m_FlamethrowerSpray.m_smoulderTime;
					stationSmouldering.Cooker = attachStation.gameObject.AddComponent<CookingStation>();
					stationSmouldering.Cooker.m_stationType = CookingStationType.Flamethrower;
					stationSmouldering.Cooker.m_attachRestrictions = false;
					stationSmouldering.Smoulder = this.m_FlamethrowerSpray.m_smoulderEffect.InstantiateOnParent(attachStation.GetAttachPoint(_handler.gameObject), true);
					ServerFlamethrowerSpray.m_smoulderLookup[attachStation] = stationSmouldering;
				}
				else if (ServerFlamethrowerSpray.m_smoulderLookup.ContainsKey(attachStation))
				{
					ServerFlamethrowerSpray.m_smoulderLookup[attachStation].LifeTime = this.m_FlamethrowerSpray.m_smoulderTime;
				}
			}
			if (_handler.IsBurning())
			{
				ServerFlammable serverFlammable = _handler.gameObject.RequestComponentUpwardsRecursive<ServerFlammable>();
				if (serverFlammable != null)
				{
					serverFlammable.Ignite();
				}
			}
		}
	}

	// Token: 0x040016B8 RID: 5816
	protected FlamethrowerSpray m_FlamethrowerSpray;

	// Token: 0x040016B9 RID: 5817
	private static Dictionary<AttachStation, ServerFlamethrowerSpray.StationSmouldering> m_smoulderLookup = new Dictionary<AttachStation, ServerFlamethrowerSpray.StationSmouldering>();

	// Token: 0x02000609 RID: 1545
	private class StationSmouldering
	{
		// Token: 0x040016BA RID: 5818
		public CookingStation Cooker;

		// Token: 0x040016BB RID: 5819
		public GameObject Smoulder;

		// Token: 0x040016BC RID: 5820
		public float LifeTime;
	}
}
