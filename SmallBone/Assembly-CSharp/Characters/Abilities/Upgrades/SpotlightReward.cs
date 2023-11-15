using System;
using Data;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000B07 RID: 2823
	[Serializable]
	public class SpotlightReward : Ability
	{
		// Token: 0x0600397C RID: 14716 RVA: 0x000A98F8 File Offset: 0x000A7AF8
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new SpotlightReward.Instance(owner, this);
		}

		// Token: 0x04002D9B RID: 11675
		[SerializeField]
		private CharacterTypeBoolArray _characterTypes;

		// Token: 0x02000B08 RID: 2824
		public class Instance : AbilityInstance<SpotlightReward>
		{
			// Token: 0x0600397E RID: 14718 RVA: 0x000A9901 File Offset: 0x000A7B01
			internal Instance(Character owner, SpotlightReward ability) : base(owner, ability)
			{
			}

			// Token: 0x0600397F RID: 14719 RVA: 0x000A990B File Offset: 0x000A7B0B
			protected override void OnAttach()
			{
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
			}

			// Token: 0x06003980 RID: 14720 RVA: 0x000A9934 File Offset: 0x000A7B34
			private void HandleOnKilled(ITarget target, ref Damage damage)
			{
				Character character = target.character;
				if (character == null)
				{
					return;
				}
				if (!this.ability._characterTypes[character.type])
				{
					return;
				}
				GameData.Currency.Type type = MMMaths.RandomBool() ? GameData.Currency.Type.Bone : GameData.Currency.Type.Gold;
				int amount = UnityEngine.Random.Range(Singleton<Service>.Instance.levelManager.currentChapter.currentStage.goldrewardAmount.x, Singleton<Service>.Instance.levelManager.currentChapter.currentStage.goldrewardAmount.y);
				Singleton<Service>.Instance.levelManager.DropCurrencyBag(type, Rarity.Unique, amount, 16, character.transform.position);
			}

			// Token: 0x06003981 RID: 14721 RVA: 0x000A99DE File Offset: 0x000A7BDE
			protected override void OnDetach()
			{
				Character owner = this.owner;
				owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.HandleOnKilled));
			}
		}
	}
}
