using System;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000925 RID: 2341
	public class ClientSynchroniserBase : SynchroniserBase, ClientSynchroniser, Synchroniser
	{
		// Token: 0x06002E08 RID: 11784 RVA: 0x00027E17 File Offset: 0x00026217
		public override void StartSynchronising(Component synchronisedObject)
		{
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x00027E19 File Offset: 0x00026219
		public override void UpdateSynchronising()
		{
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x00027E1B File Offset: 0x0002621B
		public override EntityType GetEntityType()
		{
			return EntityType.Unknown;
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x00027E1E File Offset: 0x0002621E
		public virtual bool IsValidServerUpdateSequenceNumber(uint uSequence)
		{
			return uSequence == uint.MaxValue || Mailbox.CheckSequenced(this.m_iLastSequenceNumberProcessed, (int)uSequence);
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x00027E35 File Offset: 0x00026235
		public virtual void SetLastServerUpdateSequenceNumber(uint uSequence)
		{
			this.m_iLastSequenceNumberProcessed = (int)uSequence;
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x00027E3E File Offset: 0x0002623E
		public virtual bool IsValidLastUpdateTimeStamp(float timeStamp, float diff)
		{
			return timeStamp - this.m_lastUpdateTimeStamp > diff;
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x00027E4B File Offset: 0x0002624B
		public virtual void SetLastUpdateTimeStamp(float timeStamp)
		{
			this.m_lastUpdateTimeStamp = timeStamp;
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x00027E54 File Offset: 0x00026254
		public virtual void ApplyServerUpdate(Serialisable serialisable)
		{
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x00027E56 File Offset: 0x00026256
		public virtual void ApplyServerEvent(Serialisable serialisable)
		{
		}

		// Token: 0x06002E11 RID: 11793 RVA: 0x00027E58 File Offset: 0x00026258
		protected virtual void OnDestroy()
		{
			EntitySerialisationRegistry.UnregisterObject(base.gameObject);
		}

		// Token: 0x0400250E RID: 9486
		private int m_iLastSequenceNumberProcessed = -1;

		// Token: 0x0400250F RID: 9487
		private float m_lastUpdateTimeStamp;
	}
}
