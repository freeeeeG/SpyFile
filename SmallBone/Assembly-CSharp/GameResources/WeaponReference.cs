using System;
using Characters.Gear;
using Characters.Gear.Weapons;

namespace GameResources
{
	// Token: 0x02000192 RID: 402
	[Serializable]
	public class WeaponReference : GearReference
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00018EC5 File Offset: 0x000170C5
		public override Gear.Type type
		{
			get
			{
				return Gear.Type.Weapon;
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00018EC8 File Offset: 0x000170C8
		public new WeaponRequest LoadAsync()
		{
			return new WeaponRequest(this.path);
		}

		// Token: 0x040006F1 RID: 1777
		public Weapon.Category category;
	}
}
