using System;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000922 RID: 2338
	public interface Synchroniser
	{
		// Token: 0x06002DF0 RID: 11760
		void StartSynchronising(Component synchronisedObject);

		// Token: 0x06002DF1 RID: 11761
		void StopSynchronising();

		// Token: 0x06002DF2 RID: 11762
		bool IsSynchronising();

		// Token: 0x06002DF3 RID: 11763
		void UpdateSynchronising();

		// Token: 0x06002DF4 RID: 11764
		EntityType GetEntityType();

		// Token: 0x06002DF5 RID: 11765
		void SetSynchronisedComponent(Component component);

		// Token: 0x06002DF6 RID: 11766
		Component GetSynchronisedComponent();
	}
}
