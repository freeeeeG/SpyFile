using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009D1 RID: 2513
	[Serializable]
	public class AttachAbilityWithinCollider : Ability
	{
		// Token: 0x0600356F RID: 13679 RVA: 0x0009EA82 File Offset: 0x0009CC82
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AttachAbilityWithinCollider.Instance(owner, this);
		}

		// Token: 0x04002B0B RID: 11019
		[Tooltip("이 주기(초)마다 콜라이더 내에 있는 캐릭터들을 검사합니다. 낮을수록 정밀도가 올라가지만 연산량이 많아집니다.")]
		[SerializeField]
		[Range(0.1f, 1f)]
		private float _checkInterval = 0.33f;

		// Token: 0x04002B0C RID: 11020
		[SerializeField]
		[Header("Filter")]
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x04002B0D RID: 11021
		[SerializeField]
		private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			true,
			true,
			true
		});

		// Token: 0x04002B0E RID: 11022
		[SerializeField]
		[Header("Collider")]
		private Collider2D _collider;

		// Token: 0x04002B0F RID: 11023
		[SerializeField]
		[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
		private bool _optimizedCollider = true;

		// Token: 0x04002B10 RID: 11024
		[Header("Abilities")]
		[Space]
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent.Subcomponents _abilityComponents;

		// Token: 0x020009D2 RID: 2514
		public enum ChronometerType
		{
			// Token: 0x04002B12 RID: 11026
			Master,
			// Token: 0x04002B13 RID: 11027
			Animation,
			// Token: 0x04002B14 RID: 11028
			Effect,
			// Token: 0x04002B15 RID: 11029
			Projectile
		}

		// Token: 0x020009D3 RID: 2515
		public class Instance : AbilityInstance<AttachAbilityWithinCollider>
		{
			// Token: 0x06003570 RID: 13680 RVA: 0x0009EA8C File Offset: 0x0009CC8C
			public Instance(Character owner, AttachAbilityWithinCollider ability) : base(owner, ability)
			{
				if (ability._optimizedCollider)
				{
					ability._collider.enabled = false;
				}
				this._charactersWithinCollider = new DoubleBuffered<List<Character>>(new List<Character>(AttachAbilityWithinCollider.Instance._sharedOverlapper.capacity), new List<Character>(AttachAbilityWithinCollider.Instance._sharedOverlapper.capacity));
				ability._abilityComponents.Initialize();
			}

			// Token: 0x06003571 RID: 13681 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x06003572 RID: 13682 RVA: 0x0009EAEC File Offset: 0x0009CCEC
			protected override void OnDetach()
			{
				for (int i = 0; i < this._charactersWithinCollider.Current.Count; i++)
				{
					Character character = this._charactersWithinCollider.Current[i];
					AbilityComponent[] components = this.ability._abilityComponents.components;
					if (!(character == null))
					{
						for (int j = 0; j < components.Length; j++)
						{
							character.ability.Remove(components[j].ability);
						}
					}
				}
			}

			// Token: 0x06003573 RID: 13683 RVA: 0x0009EB62 File Offset: 0x0009CD62
			public override void UpdateTime(float deltaTime)
			{
				base.UpdateTime(deltaTime);
				this._remainCheckTime -= deltaTime;
				if (this._remainCheckTime < 0f)
				{
					this._remainCheckTime = this.ability._checkInterval;
					this.Check();
				}
			}

			// Token: 0x06003574 RID: 13684 RVA: 0x0009EBA0 File Offset: 0x0009CDA0
			private void Check()
			{
				this.ability._collider.enabled = true;
				AttachAbilityWithinCollider.Instance._sharedOverlapper.contactFilter.SetLayerMask(this.ability._layer.Evaluate(this.owner.gameObject));
				AttachAbilityWithinCollider.Instance._sharedOverlapper.OverlapCollider(this.ability._collider);
				if (this.ability._optimizedCollider)
				{
					this.ability._collider.enabled = false;
				}
				int i = 0;
				while (i < AttachAbilityWithinCollider.Instance._sharedOverlapper.results.Count)
				{
					Target component = AttachAbilityWithinCollider.Instance._sharedOverlapper.results[i].GetComponent<Target>();
					Character character;
					if (component == null)
					{
						Minion component2 = AttachAbilityWithinCollider.Instance._sharedOverlapper.results[i].GetComponent<Minion>();
						if (!(component2 == null) && this.ability._characterTypeFilter[Character.Type.PlayerMinion])
						{
							character = component2.character;
							goto IL_111;
						}
					}
					else if (!(component.character == null) && this.ability._characterTypeFilter[component.character.type])
					{
						character = component.character;
						goto IL_111;
					}
					IL_190:
					i++;
					continue;
					IL_111:
					this._charactersWithinCollider.Next.Add(character);
					int num = this._charactersWithinCollider.Current.IndexOf(character);
					if (num >= 0)
					{
						this._charactersWithinCollider.Current.RemoveAt(num);
						goto IL_190;
					}
					for (int j = 0; j < this.ability._abilityComponents.components.Length; j++)
					{
						character.ability.Add(this.ability._abilityComponents.components[j].ability);
					}
					goto IL_190;
				}
				for (int k = 0; k < this._charactersWithinCollider.Current.Count; k++)
				{
					Character character2 = this._charactersWithinCollider.Current[k];
					if (!(character2 == null))
					{
						for (int l = 0; l < this.ability._abilityComponents.components.Length; l++)
						{
							character2.ability.Remove(this.ability._abilityComponents.components[l].ability);
						}
					}
				}
				this._charactersWithinCollider.Current.Clear();
				this._charactersWithinCollider.Swap();
			}

			// Token: 0x04002B16 RID: 11030
			private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(99);

			// Token: 0x04002B17 RID: 11031
			private DoubleBuffered<List<Character>> _charactersWithinCollider;

			// Token: 0x04002B18 RID: 11032
			private float _remainCheckTime;
		}
	}
}
