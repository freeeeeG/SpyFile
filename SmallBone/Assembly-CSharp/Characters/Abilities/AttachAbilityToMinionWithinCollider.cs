using System;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x020009C7 RID: 2503
	[Serializable]
	public sealed class AttachAbilityToMinionWithinCollider : Ability
	{
		// Token: 0x06003556 RID: 13654 RVA: 0x0009E079 File Offset: 0x0009C279
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new AttachAbilityToMinionWithinCollider.Instance(owner, this);
		}

		// Token: 0x04002AF4 RID: 10996
		[SerializeField]
		[Tooltip("이 주기(초)마다 콜라이더 내에 있는 캐릭터들을 검사합니다. 낮을수록 정밀도가 올라가지만 연산량이 많아집니다.")]
		[Range(0.1f, 1f)]
		private float _checkInterval = 0.33f;

		// Token: 0x04002AF5 RID: 10997
		[SerializeField]
		private Key[] _targetKeys;

		// Token: 0x04002AF6 RID: 10998
		[SerializeField]
		[Header("Collider")]
		private Collider2D _collider;

		// Token: 0x04002AF7 RID: 10999
		[Tooltip("콜라이더 최적화 여부, Composite Collider등 특별한 경우가 아니면 true로 유지")]
		[SerializeField]
		private bool _optimizedCollider = true;

		// Token: 0x04002AF8 RID: 11000
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		[Space]
		[Header("Abilities")]
		private AbilityComponent.Subcomponents _abilityComponents;

		// Token: 0x020009C8 RID: 2504
		[SerializeField]
		public enum ChronometerType
		{
			// Token: 0x04002AFA RID: 11002
			Master,
			// Token: 0x04002AFB RID: 11003
			Animation,
			// Token: 0x04002AFC RID: 11004
			Effect,
			// Token: 0x04002AFD RID: 11005
			Projectile
		}

		// Token: 0x020009C9 RID: 2505
		public class Instance : AbilityInstance<AttachAbilityToMinionWithinCollider>
		{
			// Token: 0x06003557 RID: 13655 RVA: 0x0009E084 File Offset: 0x0009C284
			public Instance(Character owner, AttachAbilityToMinionWithinCollider ability) : base(owner, ability)
			{
				if (ability._optimizedCollider)
				{
					ability._collider.enabled = false;
				}
				this._charactersWithinCollider = new DoubleBuffered<List<Character>>(new List<Character>(AttachAbilityToMinionWithinCollider.Instance._sharedOverlapper.capacity), new List<Character>(AttachAbilityToMinionWithinCollider.Instance._sharedOverlapper.capacity));
				ability._abilityComponents.Initialize();
			}

			// Token: 0x06003558 RID: 13656 RVA: 0x00002191 File Offset: 0x00000391
			protected override void OnAttach()
			{
			}

			// Token: 0x06003559 RID: 13657 RVA: 0x0009E0E4 File Offset: 0x0009C2E4
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

			// Token: 0x0600355A RID: 13658 RVA: 0x0009E15A File Offset: 0x0009C35A
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

			// Token: 0x0600355B RID: 13659 RVA: 0x0009E198 File Offset: 0x0009C398
			private void Check()
			{
				this.ability._collider.enabled = true;
				AttachAbilityToMinionWithinCollider.Instance._sharedOverlapper.contactFilter.SetLayerMask(512);
				AttachAbilityToMinionWithinCollider.Instance._sharedOverlapper.OverlapCollider(this.ability._collider);
				if (this.ability._optimizedCollider)
				{
					this.ability._collider.enabled = false;
				}
				for (int i = 0; i < AttachAbilityToMinionWithinCollider.Instance._sharedOverlapper.results.Count; i++)
				{
					Minion component = AttachAbilityToMinionWithinCollider.Instance._sharedOverlapper.results[i].GetComponent<Minion>();
					if (!(component == null))
					{
						bool flag = false;
						foreach (Key key in this.ability._targetKeys)
						{
							if (component.character.key == key)
							{
								flag = true;
							}
						}
						if (flag)
						{
							this._charactersWithinCollider.Next.Add(component.character);
							int num = this._charactersWithinCollider.Current.IndexOf(component.character);
							if (num >= 0)
							{
								this._charactersWithinCollider.Current.RemoveAt(num);
							}
							else
							{
								for (int k = 0; k < this.ability._abilityComponents.components.Length; k++)
								{
									component.character.ability.Add(this.ability._abilityComponents.components[k].ability);
								}
							}
						}
					}
				}
				for (int l = 0; l < this._charactersWithinCollider.Current.Count; l++)
				{
					Character character = this._charactersWithinCollider.Current[l];
					if (!(character == null))
					{
						for (int m = 0; m < this.ability._abilityComponents.components.Length; m++)
						{
							character.ability.Remove(this.ability._abilityComponents.components[m].ability);
						}
					}
				}
				this._charactersWithinCollider.Current.Clear();
				this._charactersWithinCollider.Swap();
			}

			// Token: 0x04002AFE RID: 11006
			private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(99);

			// Token: 0x04002AFF RID: 11007
			private DoubleBuffered<List<Character>> _charactersWithinCollider;

			// Token: 0x04002B00 RID: 11008
			private float _remainCheckTime;
		}
	}
}
