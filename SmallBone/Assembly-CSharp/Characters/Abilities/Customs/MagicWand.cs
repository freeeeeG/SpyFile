using System;
using FX;
using Level;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D66 RID: 3430
	[Serializable]
	public sealed class MagicWand : Ability
	{
		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06004526 RID: 17702 RVA: 0x000C8CF8 File Offset: 0x000C6EF8
		// (set) Token: 0x06004527 RID: 17703 RVA: 0x000C8D05 File Offset: 0x000C6F05
		public int stack
		{
			get
			{
				return this._instance.stack;
			}
			set
			{
				this._instance.stack = value;
			}
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x000C8D14 File Offset: 0x000C6F14
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this._instance = new MagicWand.Instance(owner, this);
		}

		// Token: 0x040034A5 RID: 13477
		[SerializeField]
		private CharacterTypeBoolArray _targetType;

		// Token: 0x040034A6 RID: 13478
		[SerializeField]
		private Collider2D _findRange;

		// Token: 0x040034A7 RID: 13479
		[SerializeField]
		private GameObject _summonPrefab;

		// Token: 0x040034A8 RID: 13480
		[SerializeField]
		private EffectInfo _effect;

		// Token: 0x040034A9 RID: 13481
		[SerializeField]
		private int _maxStack;

		// Token: 0x040034AA RID: 13482
		private MagicWand.Instance _instance;

		// Token: 0x02000D67 RID: 3431
		public class Instance : AbilityInstance<MagicWand>
		{
			// Token: 0x0600452A RID: 17706 RVA: 0x000C8D31 File Offset: 0x000C6F31
			public Instance(Character owner, MagicWand ability) : base(owner, ability)
			{
			}

			// Token: 0x17000E5F RID: 3679
			// (get) Token: 0x0600452B RID: 17707 RVA: 0x000C8D3B File Offset: 0x000C6F3B
			// (set) Token: 0x0600452C RID: 17708 RVA: 0x000C8D43 File Offset: 0x000C6F43
			public int stack { get; set; }

			// Token: 0x0600452D RID: 17709 RVA: 0x000C8D4C File Offset: 0x000C6F4C
			protected override void OnAttach()
			{
				this.owner.playerComponents.inventory.weapon.onSwap += this.Weapon_onSwap;
			}

			// Token: 0x0600452E RID: 17710 RVA: 0x000C8D74 File Offset: 0x000C6F74
			private void Weapon_onSwap()
			{
				int stack = this.stack;
				this.stack = stack + 1;
				if (this.stack >= this.ability._maxStack)
				{
					this.ChangeToDummy();
					this.stack = 0;
				}
			}

			// Token: 0x0600452F RID: 17711 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnDetach()
			{
			}

			// Token: 0x06004530 RID: 17712 RVA: 0x000C8DB4 File Offset: 0x000C6FB4
			private void ChangeToDummy()
			{
				Character randomTarget = TargetFinder.GetRandomTarget(this.ability._findRange, 1024);
				if (randomTarget == null)
				{
					return;
				}
				Vector3 position = randomTarget.transform.position;
				randomTarget.health.Kill();
				UnityEngine.Object.Instantiate<GameObject>(this.ability._summonPrefab, Map.Instance.waveContainer.summonWave.transform).transform.position = position;
				this.ability._effect.Spawn(position, 0f, 1f);
			}
		}
	}
}
