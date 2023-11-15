using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000B9 RID: 185
	[CreateAssetMenu(fileName = "GunData", menuName = "GunData", order = 1)]
	public class GunData : ScriptableObject
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0001BF9D File Offset: 0x0001A19D
		public string nameString
		{
			get
			{
				return LocalizationSystem.GetLocalizedValue(this.nameStringID.key);
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600060A RID: 1546 RVA: 0x0001BFAF File Offset: 0x0001A1AF
		public string description
		{
			get
			{
				return LocalizationSystem.GetLocalizedValue(this.descriptionStringID.key);
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x0001BFC1 File Offset: 0x0001A1C1
		public string bulletOPTag
		{
			get
			{
				return this.bullet.name;
			}
		}

		// Token: 0x040003CC RID: 972
		public LocalizedString nameStringID;

		// Token: 0x040003CD RID: 973
		public LocalizedString descriptionStringID;

		// Token: 0x040003CE RID: 974
		public GameObject model;

		// Token: 0x040003CF RID: 975
		public Sprite icon;

		// Token: 0x040003D0 RID: 976
		public SoundEffectSO gunshotSFX;

		// Token: 0x040003D1 RID: 977
		public SoundEffectSO reloadSFXOverride;

		// Token: 0x040003D2 RID: 978
		public float damage;

		// Token: 0x040003D3 RID: 979
		public float shotCooldown;

		// Token: 0x040003D4 RID: 980
		public int maxAmmo;

		// Token: 0x040003D5 RID: 981
		public float reloadDuration;

		// Token: 0x040003D6 RID: 982
		public int numOfProjectiles;

		// Token: 0x040003D7 RID: 983
		public float spread;

		// Token: 0x040003D8 RID: 984
		public float knockback;

		// Token: 0x040003D9 RID: 985
		public float projectileSpeed;

		// Token: 0x040003DA RID: 986
		public int bounce;

		// Token: 0x040003DB RID: 987
		public int piercing;

		// Token: 0x040003DC RID: 988
		public float inaccuracy = 10f;

		// Token: 0x040003DD RID: 989
		public GameObject bullet;

		// Token: 0x040003DE RID: 990
		public bool isSummonGun;

		// Token: 0x040003DF RID: 991
		public bool disableManualReload;

		// Token: 0x040003E0 RID: 992
		public List<GunEvolution> gunEvolutions;
	}
}
