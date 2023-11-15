using System;
using Characters.Abilities.CharacterStat;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D13 RID: 3347
	public class GhoulConsumeStatAttacher : AbilityAttacher
	{
		// Token: 0x06004385 RID: 17285 RVA: 0x00002191 File Offset: 0x00000391
		public override void OnIntialize()
		{
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x000C4ABF File Offset: 0x000C2CBF
		public override void StartAttach()
		{
			Character owner = base.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x000C4AE8 File Offset: 0x000C2CE8
		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (target.character == null)
			{
				return;
			}
			if (!this._motionTypeFilter[gaveDamage.motionType])
			{
				return;
			}
			if (!this._attackTypeFilter[gaveDamage.attackType])
			{
				return;
			}
			if (!this._characterTypeFilter[target.character.type])
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(gaveDamage.key))
			{
				return;
			}
			foreach (GhoulConsumeStatAttacher.KeyMap keyMap in this._keyMaps.values)
			{
				if (gaveDamage.key.Equals(keyMap.key, StringComparison.OrdinalIgnoreCase))
				{
					base.owner.ability.Add(keyMap.statBonus.ability);
					return;
				}
			}
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x000C4BA4 File Offset: 0x000C2DA4
		public override void StopAttach()
		{
			if (base.owner == null)
			{
				return;
			}
			Character owner = base.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			foreach (GhoulConsumeStatAttacher.KeyMap keyMap in this._keyMaps.values)
			{
				base.owner.ability.Remove(keyMap.statBonus.ability);
			}
		}

		// Token: 0x04003394 RID: 13204
		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		// Token: 0x04003395 RID: 13205
		[SerializeField]
		private AttackTypeBoolArray _attackTypeFilter;

		// Token: 0x04003396 RID: 13206
		[SerializeField]
		private CharacterTypeBoolArray _characterTypeFilter = new CharacterTypeBoolArray(new bool[]
		{
			true,
			true,
			true,
			true,
			true,
			false,
			false,
			false
		});

		// Token: 0x04003397 RID: 13207
		[Space]
		[SerializeField]
		private GhoulConsumeStatAttacher.KeyMap.Reorderable _keyMaps;

		// Token: 0x02000D14 RID: 3348
		[Serializable]
		private class KeyMap
		{
			// Token: 0x04003398 RID: 13208
			public string key;

			// Token: 0x04003399 RID: 13209
			[UnityEditor.Subcomponent(typeof(StackableStatBonusComponent))]
			public StackableStatBonusComponent statBonus;

			// Token: 0x02000D15 RID: 3349
			[Serializable]
			public class Reorderable : ReorderableArray<GhoulConsumeStatAttacher.KeyMap>
			{
			}
		}
	}
}
