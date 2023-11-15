using System;
using Services;
using Singletons;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B33 RID: 2867
	[Serializable]
	public class OnEnterMap : Trigger
	{
		// Token: 0x060039EB RID: 14827 RVA: 0x000AB0AD File Offset: 0x000A92AD
		public override void Attach(Character character)
		{
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn += base.Invoke;
		}

		// Token: 0x060039EC RID: 14828 RVA: 0x000AB0CA File Offset: 0x000A92CA
		public override void Detach()
		{
			Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= base.Invoke;
		}
	}
}
