using System;
using Characters.Gear;

namespace GameResources
{
	// Token: 0x0200017C RID: 380
	[Serializable]
	public class EssenceReference : GearReference
	{
		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x000147BD File Offset: 0x000129BD
		public override Gear.Type type
		{
			get
			{
				return Gear.Type.Quintessence;
			}
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x00017EE8 File Offset: 0x000160E8
		public new EssenceRequest LoadAsync()
		{
			return new EssenceRequest(this.path);
		}
	}
}
