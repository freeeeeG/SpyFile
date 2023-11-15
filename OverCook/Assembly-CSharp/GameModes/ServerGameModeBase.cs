using System;

namespace GameModes
{
	// Token: 0x020006B1 RID: 1713
	public abstract class ServerGameModeBase : IServerMode
	{
		// Token: 0x0600206C RID: 8300 RVA: 0x0009C74F File Offset: 0x0009AB4F
		public ServerGameModeBase(Config config)
		{
		}

		// Token: 0x0600206D RID: 8301 RVA: 0x0009C757 File Offset: 0x0009AB57
		public virtual void Setup(ServerContext context, SessionConfig config, ref ServerSetupData setupData)
		{
		}

		// Token: 0x0600206E RID: 8302 RVA: 0x0009C759 File Offset: 0x0009AB59
		public virtual void Begin()
		{
		}

		// Token: 0x0600206F RID: 8303 RVA: 0x0009C75B File Offset: 0x0009AB5B
		public virtual void Update()
		{
		}

		// Token: 0x06002070 RID: 8304 RVA: 0x0009C75D File Offset: 0x0009AB5D
		public virtual void End()
		{
		}
	}
}
