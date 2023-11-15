using System;
using System.Collections;
using System.Collections.Generic;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000B04 RID: 2820
	[Serializable]
	public sealed class RiskyDarkEnemy : Ability
	{
		// Token: 0x0600396F RID: 14703 RVA: 0x000A970E File Offset: 0x000A790E
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new RiskyDarkEnemy.Instance(owner, this);
		}

		// Token: 0x04002D95 RID: 11669
		[SerializeField]
		[AbilityComponent.SubcomponentAttribute]
		private AbilityComponent _targetAbility;

		// Token: 0x02000B05 RID: 2821
		public sealed class Instance : AbilityInstance<RiskyDarkEnemy>
		{
			// Token: 0x06003971 RID: 14705 RVA: 0x000A9717 File Offset: 0x000A7917
			public Instance(Character owner, RiskyDarkEnemy ability) : base(owner, ability)
			{
			}

			// Token: 0x06003972 RID: 14706 RVA: 0x000A9724 File Offset: 0x000A7924
			protected override void OnAttach()
			{
				this.ability._targetAbility.Initialize();
				this._targets = new HashSet<Character>();
				if (this.owner.playerComponents == null)
				{
					return;
				}
				this.owner.ability.Add(this.ability._targetAbility.ability);
				Singleton<Service>.Instance.levelManager.onMapLoaded += this.HandleOnMapLoaded;
			}

			// Token: 0x06003973 RID: 14707 RVA: 0x000A9796 File Offset: 0x000A7996
			private void HandleOnMapLoaded()
			{
				this._applyReference.Stop();
				this._applyReference = this.owner.StartCoroutineWithReference(this.Apply());
			}

			// Token: 0x06003974 RID: 14708 RVA: 0x000A97BA File Offset: 0x000A79BA
			private IEnumerator Apply()
			{
				this._targets.Clear();
				for (;;)
				{
					foreach (Character character in Map.Instance.waveContainer.GetAllSpawnedEnemies())
					{
						if (!this._targets.Contains(character) && !character.health.dead && character.type == Character.Type.Named)
						{
							character.ability.Add(this.ability._targetAbility.ability);
							this._targets.Add(character);
						}
					}
					yield return Chronometer.global.WaitForSeconds(0.1f);
				}
				yield break;
			}

			// Token: 0x06003975 RID: 14709 RVA: 0x000A97C9 File Offset: 0x000A79C9
			protected override void OnDetach()
			{
				Singleton<Service>.Instance.levelManager.onMapLoadedAndFadedIn -= this.HandleOnMapLoaded;
			}

			// Token: 0x04002D96 RID: 11670
			private HashSet<Character> _targets;

			// Token: 0x04002D97 RID: 11671
			private CoroutineReference _applyReference;
		}
	}
}
