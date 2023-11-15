using System;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x0200091C RID: 2332
	public class ServerClientPhysicsObjectSynchroniser : ClientWorldObjectSynchroniser
	{
		// Token: 0x06002DB7 RID: 11703 RVA: 0x000D91AF File Offset: 0x000D75AF
		public override EntityType GetEntityType()
		{
			return EntityType.PhysicsObject;
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x000D91B3 File Offset: 0x000D75B3
		public override void Pause()
		{
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x000D91B5 File Offset: 0x000D75B5
		public override void Resume()
		{
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x000D91B7 File Offset: 0x000D75B7
		public override void OnResumeDataReceived(Serialisable _data)
		{
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x000D91B9 File Offset: 0x000D75B9
		public override bool IsReadyToResume()
		{
			return true;
		}
	}
}
