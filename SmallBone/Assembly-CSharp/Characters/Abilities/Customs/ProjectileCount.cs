using System;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D7C RID: 3452
	[Serializable]
	public sealed class ProjectileCount : Ability
	{
		// Token: 0x0600458A RID: 17802 RVA: 0x000C99BD File Offset: 0x000C7BBD
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new ProjectileCount.Instance(owner, this);
		}

		// Token: 0x040034D8 RID: 13528
		[SerializeField]
		private Projectile[] _projectilesToCount;

		// Token: 0x02000D7D RID: 3453
		public sealed class Instance : AbilityInstance<ProjectileCount>
		{
			// Token: 0x17000E76 RID: 3702
			// (get) Token: 0x0600458C RID: 17804 RVA: 0x000C99C8 File Offset: 0x000C7BC8
			public override Sprite icon
			{
				get
				{
					int num = 0;
					foreach (Projectile projectile in this.ability._projectilesToCount)
					{
						num += projectile.reusable.spawnedCount;
					}
					if (num <= 0)
					{
						return null;
					}
					return this.ability.defaultIcon;
				}
			}

			// Token: 0x17000E77 RID: 3703
			// (get) Token: 0x0600458D RID: 17805 RVA: 0x000C9A14 File Offset: 0x000C7C14
			public override int iconStacks
			{
				get
				{
					int num = 0;
					foreach (Projectile projectile in this.ability._projectilesToCount)
					{
						num += projectile.reusable.spawnedCount;
					}
					this._totalCache = num;
					return num;
				}
			}

			// Token: 0x0600458E RID: 17806 RVA: 0x000C9A57 File Offset: 0x000C7C57
			public Instance(Character owner, ProjectileCount ability) : base(owner, ability)
			{
			}

			// Token: 0x0600458F RID: 17807 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x06004590 RID: 17808 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}

			// Token: 0x040034D9 RID: 13529
			private int _totalCache;
		}
	}
}
