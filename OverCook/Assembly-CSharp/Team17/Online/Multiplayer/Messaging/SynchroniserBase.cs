using System;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000924 RID: 2340
	public abstract class SynchroniserBase : MonoBehaviour, Synchroniser
	{
		// Token: 0x06002DFE RID: 11774
		public abstract void StartSynchronising(Component synchronisedObject);

		// Token: 0x06002DFF RID: 11775 RVA: 0x00027AD7 File Offset: 0x00025ED7
		public virtual void StopSynchronising()
		{
		}

		// Token: 0x06002E00 RID: 11776 RVA: 0x00027AD9 File Offset: 0x00025ED9
		public bool IsSynchronising()
		{
			return this.ActiveAndEnabled;
		}

		// Token: 0x06002E01 RID: 11777
		public abstract void UpdateSynchronising();

		// Token: 0x06002E02 RID: 11778
		public abstract EntityType GetEntityType();

		// Token: 0x06002E03 RID: 11779 RVA: 0x00027AE1 File Offset: 0x00025EE1
		public virtual void SetSynchronisedComponent(Component component)
		{
			this.m_Component = component;
		}

		// Token: 0x06002E04 RID: 11780 RVA: 0x00027AEA File Offset: 0x00025EEA
		protected virtual void OnEnable()
		{
			this.ActiveAndEnabled = true;
		}

		// Token: 0x06002E05 RID: 11781 RVA: 0x00027AF3 File Offset: 0x00025EF3
		protected virtual void OnDisable()
		{
			this.ActiveAndEnabled = false;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x00027AFC File Offset: 0x00025EFC
		public virtual Component GetSynchronisedComponent()
		{
			return this.m_Component;
		}

		// Token: 0x0400250C RID: 9484
		public bool ActiveAndEnabled;

		// Token: 0x0400250D RID: 9485
		private Component m_Component;
	}
}
