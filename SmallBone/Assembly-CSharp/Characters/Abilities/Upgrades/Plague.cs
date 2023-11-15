using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000AFD RID: 2813
	[Serializable]
	public class Plague : Ability
	{
		// Token: 0x06003956 RID: 14678 RVA: 0x000A92AD File Offset: 0x000A74AD
		public override void Initialize()
		{
			base.Initialize();
			this._plagueAbility.Initialize();
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x000A92C0 File Offset: 0x000A74C0
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Plague.Instance(owner, this);
		}

		// Token: 0x04002D84 RID: 11652
		[SerializeField]
		private CharacterTypeBoolArray _characterType;

		// Token: 0x04002D85 RID: 11653
		[AbilityComponent.SubcomponentAttribute]
		[SerializeField]
		private AbilityComponent _plagueAbility;

		// Token: 0x02000AFE RID: 2814
		public class Instance : AbilityInstance<Plague>
		{
			// Token: 0x06003959 RID: 14681 RVA: 0x000A92C9 File Offset: 0x000A74C9
			public Instance(Character owner, Plague ability) : base(owner, ability)
			{
			}

			// Token: 0x0600395A RID: 14682 RVA: 0x000A92D3 File Offset: 0x000A74D3
			protected override void OnAttach()
			{
				this._targets = new HashSet<Character>();
				if (this.owner.playerComponents == null)
				{
					return;
				}
				Singleton<Service>.Instance.levelManager.onMapLoaded += this.HandleOnMapLoaded;
				this.HandleOnMapLoaded();
			}

			// Token: 0x0600395B RID: 14683 RVA: 0x000A930F File Offset: 0x000A750F
			private void HandleOnMapLoaded()
			{
				this._applyReference.Stop();
				if (this.owner != null)
				{
					this._applyReference = this.owner.StartCoroutineWithReference(this.Apply());
				}
			}

			// Token: 0x0600395C RID: 14684 RVA: 0x000A9341 File Offset: 0x000A7541
			private IEnumerator Apply()
			{
				this._targets.Clear();
				for (;;)
				{
					foreach (Character character in Map.Instance.waveContainer.GetAllEnemies())
					{
						if (!this._targets.Contains(character) && !character.health.dead && this.ability._characterType[character.type] && !(character.ability == null))
						{
							character.ability.Add(this.ability._plagueAbility.ability);
							this._targets.Add(character);
						}
					}
					yield return Chronometer.global.WaitForSeconds(0.1f);
				}
				yield break;
			}

			// Token: 0x0600395D RID: 14685 RVA: 0x000A9350 File Offset: 0x000A7550
			protected override void OnDetach()
			{
				Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= this.HandleOnMapLoaded;
			}

			// Token: 0x04002D86 RID: 11654
			private HashSet<Character> _targets;

			// Token: 0x04002D87 RID: 11655
			private CoroutineReference _applyReference;
		}
	}
}
