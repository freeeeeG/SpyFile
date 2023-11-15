using System;
using UnityEngine;

namespace Characters.Minions
{
	// Token: 0x02000816 RID: 2070
	[CreateAssetMenu(fileName = "MinionSetting", menuName = "ScriptableObjects/MinionSetting", order = 1)]
	public sealed class MinionSetting : ScriptableObject
	{
		// Token: 0x04002443 RID: 9283
		public int maxCount = int.MaxValue;

		// Token: 0x04002444 RID: 9284
		public float lifeTime = float.MaxValue;

		// Token: 0x04002445 RID: 9285
		[Space]
		public bool despawnOnMapChanged = true;

		// Token: 0x04002446 RID: 9286
		public bool despawnOnSwap;

		// Token: 0x04002447 RID: 9287
		public bool despawnOnWeaponDropped;

		// Token: 0x04002448 RID: 9288
		public bool despawnOnEssenceChanged;

		// Token: 0x04002449 RID: 9289
		[Space]
		public bool triggerOnKilled = true;

		// Token: 0x0400244A RID: 9290
		public bool triggerOnGiveDamage;

		// Token: 0x0400244B RID: 9291
		public bool triggerOnGaveDamage;

		// Token: 0x0400244C RID: 9292
		public bool triggerOnGaveStatus;
	}
}
