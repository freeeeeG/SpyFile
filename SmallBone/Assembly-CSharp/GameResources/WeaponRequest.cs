using System;
using Characters.Gear.Weapons;

namespace GameResources
{
	// Token: 0x0200018E RID: 398
	public sealed class WeaponRequest : Request<Weapon>
	{
		// Token: 0x060008B2 RID: 2226 RVA: 0x00018EA1 File Offset: 0x000170A1
		public WeaponRequest(string path) : base(path)
		{
		}
	}
}
