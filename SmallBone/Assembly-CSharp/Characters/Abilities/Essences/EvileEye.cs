using System;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Essences
{
	// Token: 0x02000BDF RID: 3039
	[Serializable]
	public class EvileEye : Ability
	{
		// Token: 0x06003E81 RID: 16001 RVA: 0x000B5AD1 File Offset: 0x000B3CD1
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new EvileEye.Instance(owner, this);
		}

		// Token: 0x0400303B RID: 12347
		[SerializeField]
		private float attackDamage;

		// Token: 0x0400303C RID: 12348
		[SerializeField]
		private SoundInfo soundInfo;

		// Token: 0x0400303D RID: 12349
		[SerializeField]
		private HitInfo hitInfo;

		// Token: 0x02000BE0 RID: 3040
		public class Instance : AbilityInstance<EvileEye>
		{
			// Token: 0x17000D3D RID: 3389
			// (get) Token: 0x06003E83 RID: 16003 RVA: 0x000B5ADA File Offset: 0x000B3CDA
			public override int iconStacks
			{
				get
				{
					return this._stacks;
				}
			}

			// Token: 0x17000D3E RID: 3390
			// (get) Token: 0x06003E84 RID: 16004 RVA: 0x000B0EA4 File Offset: 0x000AF0A4
			public override Sprite icon
			{
				get
				{
					return SavableAbilityResource.instance.curseIcon;
				}
			}

			// Token: 0x06003E85 RID: 16005 RVA: 0x000B5AE2 File Offset: 0x000B3CE2
			public Instance(Character owner, EvileEye ability) : base(owner, ability)
			{
			}

			// Token: 0x06003E86 RID: 16006 RVA: 0x000B5AF7 File Offset: 0x000B3CF7
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._elapsed += deltaTime;
				if (this._elapsed >= this._interval)
				{
					this.TakeDamage();
					this._elapsed -= this._interval;
				}
			}

			// Token: 0x06003E87 RID: 16007 RVA: 0x000B5B35 File Offset: 0x000B3D35
			protected override void OnAttach()
			{
				this._elapsed = 0f;
			}

			// Token: 0x06003E88 RID: 16008 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}

			// Token: 0x06003E89 RID: 16009 RVA: 0x000B5B42 File Offset: 0x000B3D42
			public override void Refresh()
			{
				base.Refresh();
				this._elapsed = 0f;
			}

			// Token: 0x06003E8A RID: 16010 RVA: 0x000B5B58 File Offset: 0x000B3D58
			private void TakeDamage()
			{
				Character player = Singleton<Service>.Instance.levelManager.player;
				Damage damage = player.stat.GetDamage((double)this.ability.attackDamage, this.owner.transform.position, this.ability.hitInfo);
				player.Attack(this.owner, ref damage);
				PersistentSingleton<SoundManager>.Instance.PlaySound(this.ability.soundInfo, this.owner.transform.position);
			}

			// Token: 0x0400303E RID: 12350
			private float _interval = 0.5f;

			// Token: 0x0400303F RID: 12351
			private float _elapsed;

			// Token: 0x04003040 RID: 12352
			private int _stacks;
		}
	}
}
