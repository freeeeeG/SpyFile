using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities.Items
{
	// Token: 0x02000CD1 RID: 3281
	[Serializable]
	public sealed class HealthScreeningsMachine : Ability
	{
		// Token: 0x06004273 RID: 17011 RVA: 0x000C17E2 File Offset: 0x000BF9E2
		public override void Initialize()
		{
			base.Initialize();
			this._overlapper = new NonAllocOverlapper(128);
			this._overlapper.contactFilter.SetLayerMask(1024);
		}

		// Token: 0x06004274 RID: 17012 RVA: 0x000C1814 File Offset: 0x000BFA14
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new HealthScreeningsMachine.Instance(owner, this);
		}

		// Token: 0x040032DE RID: 13022
		[SerializeField]
		private float _checkInterval = 0.33f;

		// Token: 0x040032DF RID: 13023
		[SerializeField]
		private Collider2D _range;

		// Token: 0x040032E0 RID: 13024
		[SerializeField]
		private int _maxStack;

		// Token: 0x040032E1 RID: 13025
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x040032E2 RID: 13026
		[SerializeField]
		private CharacterStatusKindBoolArray _statusKinds;

		// Token: 0x040032E3 RID: 13027
		[SerializeField]
		private CharacterTypeBoolArray _characterTypes;

		// Token: 0x040032E4 RID: 13028
		private int _stack;

		// Token: 0x040032E5 RID: 13029
		private NonAllocOverlapper _overlapper;

		// Token: 0x02000CD2 RID: 3282
		public class Instance : AbilityInstance<HealthScreeningsMachine>
		{
			// Token: 0x17000DDB RID: 3547
			// (get) Token: 0x06004276 RID: 17014 RVA: 0x000C1830 File Offset: 0x000BFA30
			public override Sprite icon
			{
				get
				{
					if (this.ability._stack <= 0)
					{
						return null;
					}
					return base.icon;
				}
			}

			// Token: 0x17000DDC RID: 3548
			// (get) Token: 0x06004277 RID: 17015 RVA: 0x000C1848 File Offset: 0x000BFA48
			public override int iconStacks
			{
				get
				{
					return this.ability._stack;
				}
			}

			// Token: 0x06004278 RID: 17016 RVA: 0x000C1855 File Offset: 0x000BFA55
			public Instance(Character owner, HealthScreeningsMachine ability) : base(owner, ability)
			{
			}

			// Token: 0x06004279 RID: 17017 RVA: 0x000C185F File Offset: 0x000BFA5F
			protected override void OnAttach()
			{
				this._stat = this.ability._statPerStack.Clone();
				this.owner.stat.AttachValues(this._stat);
				this.owner.StartCoroutine(this.CCheckWithinRange());
			}

			// Token: 0x0600427A RID: 17018 RVA: 0x000C189F File Offset: 0x000BFA9F
			protected override void OnDetach()
			{
				this.owner.stat.DetachValues(this._stat);
			}

			// Token: 0x0600427B RID: 17019 RVA: 0x000C18B7 File Offset: 0x000BFAB7
			public override void Refresh()
			{
				base.Refresh();
				this.ability._stack++;
			}

			// Token: 0x0600427C RID: 17020 RVA: 0x000B7067 File Offset: 0x000B5267
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
			}

			// Token: 0x0600427D RID: 17021 RVA: 0x000C18D4 File Offset: 0x000BFAD4
			public void UpdateStack()
			{
				for (int i = 0; i < this._stat.values.Length; i++)
				{
					this._stat.values[i].value = this.ability._statPerStack.values[i].GetStackedValue((double)this.ability._stack);
				}
				this.owner.stat.SetNeedUpdate();
			}

			// Token: 0x0600427E RID: 17022 RVA: 0x000C193E File Offset: 0x000BFB3E
			private IEnumerator CCheckWithinRange()
			{
				while (base.attached)
				{
					using (new UsingCollider(this.ability._range, true))
					{
						this.ability._overlapper.OverlapCollider(this.ability._range);
					}
					IEnumerable<Collider2D> source = this.ability._overlapper.results.Where(delegate(Collider2D result)
					{
						Character component = result.GetComponent<Character>();
						return !(component == null) && this.ability._characterTypes[component.type] && component.status.hasAny;
					});
					this.ability._stack = Mathf.Min(this.ability._maxStack, source.Count<Collider2D>());
					this.UpdateStack();
					yield return Chronometer.global.WaitForSeconds(this.ability._checkInterval);
				}
				yield break;
			}

			// Token: 0x040032E6 RID: 13030
			private Stat.Values _stat;
		}
	}
}
