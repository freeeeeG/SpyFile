using System;

namespace GameModes
{
	// Token: 0x020006B2 RID: 1714
	public abstract class ClientGameModeBase : IClientMode
	{
		// Token: 0x06002071 RID: 8305 RVA: 0x0009C8E7 File Offset: 0x0009ACE7
		public ClientGameModeBase(Config config)
		{
		}

		// Token: 0x06002072 RID: 8306 RVA: 0x0009C8EF File Offset: 0x0009ACEF
		public virtual void Setup(ClientContext context, SessionConfig config, ref ClientSetupData setupData)
		{
		}

		// Token: 0x06002073 RID: 8307 RVA: 0x0009C8F1 File Offset: 0x0009ACF1
		public virtual void Begin()
		{
		}

		// Token: 0x06002074 RID: 8308 RVA: 0x0009C8F3 File Offset: 0x0009ACF3
		public virtual void Update()
		{
		}

		// Token: 0x06002075 RID: 8309 RVA: 0x0009C8F5 File Offset: 0x0009ACF5
		public virtual void End()
		{
		}
	}
}
