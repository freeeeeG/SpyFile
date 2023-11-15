using System;
using Services;
using Singletons;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B1F RID: 2847
	[Serializable]
	public sealed class OnActivateMapReward : Trigger
	{
		// Token: 0x060039C4 RID: 14788 RVA: 0x000AAA22 File Offset: 0x000A8C22
		public override void Attach(Character character)
		{
			Singleton<Service>.Instance.levelManager.onActivateMapReward += base.Invoke;
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x000AAA3F File Offset: 0x000A8C3F
		public override void Detach()
		{
			Singleton<Service>.Instance.levelManager.onActivateMapReward -= base.Invoke;
		}
	}
}
