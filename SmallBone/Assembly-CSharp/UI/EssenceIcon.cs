using System;
using Characters.Gear.Quintessences.Constraints;
using GameResources;

namespace UI
{
	// Token: 0x0200039D RID: 925
	public class EssenceIcon : IconWithCooldown
	{
		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001100 RID: 4352 RVA: 0x000323C2 File Offset: 0x000305C2
		// (set) Token: 0x06001101 RID: 4353 RVA: 0x000323CA File Offset: 0x000305CA
		public Constraint.Subcomponents constraints { get; set; }

		// Token: 0x06001102 RID: 4354 RVA: 0x000323D4 File Offset: 0x000305D4
		protected override void Update()
		{
			if (this.constraints == null)
			{
				return;
			}
			base.Update();
			base.icon.material = ((this.constraints.components.Pass() && base.cooldown.canUse) ? null : MaterialResource.ui_grayScale);
		}
	}
}
