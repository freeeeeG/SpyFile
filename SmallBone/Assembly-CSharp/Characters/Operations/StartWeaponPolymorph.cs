using System;
using System.Collections.Generic;
using Characters.Controllers;
using Characters.Gear.Weapons;
using Services;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E3C RID: 3644
	public class StartWeaponPolymorph : CharacterOperation
	{
		// Token: 0x06004897 RID: 18583 RVA: 0x000D3570 File Offset: 0x000D1770
		private void Awake()
		{
			this._weaponInstance = UnityEngine.Object.Instantiate<Weapon>(this._polymorphWeapon);
			this._weaponInstance.transform.parent = null;
			this._weaponInstance.name = this._polymorphWeapon.name;
			this._weaponInstance.Initialize();
			this._weaponInstance.gameObject.SetActive(false);
			if (this._followSkillOrder)
			{
				this._matchedSkillInfos = new List<SkillInfo>(this._weaponInstance.currentSkills.Capacity);
			}
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x000D35F4 File Offset: 0x000D17F4
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (Service.quitting)
			{
				return;
			}
			UnityEngine.Object.Destroy(this._weaponInstance.gameObject);
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x000D3614 File Offset: 0x000D1814
		public override void Run(Character target)
		{
			this._before = target.playerComponents.inventory.weapon.current;
			target.playerComponents.inventory.weapon.Polymorph(this._weaponInstance);
			this.ApplySkillMap();
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x000D3654 File Offset: 0x000D1854
		private void ApplySkillMap()
		{
			if (this._skillMaps.values.Length == 0)
			{
				return;
			}
			if (this._weaponInstance.currentSkills != null)
			{
				this._weaponInstance.currentSkills.Clear();
			}
			if (this._followSkillOrder)
			{
				this._matchedSkillInfos.Clear();
			}
			foreach (StartWeaponPolymorph.SkillMap skillMap in this._skillMaps.values)
			{
				SkillInfo[] skills = this._weaponInstance.skills;
				int j = 0;
				while (j < skills.Length)
				{
					SkillInfo skillInfo = skills[j];
					if (skillMap.originalSkill.action.button != Button.None && skillInfo.key.Equals(skillMap.polymorphSkillKey, StringComparison.OrdinalIgnoreCase))
					{
						if (skillMap.copyCooldown)
						{
							skillInfo.action.cooldown.CopyCooldown(skillMap.originalSkill.action.cooldown);
						}
						this._weaponInstance.currentSkills.Add(skillInfo);
						if (this._followSkillOrder)
						{
							this._matchedSkillInfos.Add(skillMap.originalSkill);
							break;
						}
						break;
					}
					else
					{
						j++;
					}
				}
			}
			if (this._followSkillOrder)
			{
				string key = this._before.currentSkills[0].key;
				string key2 = this._matchedSkillInfos[0].key;
				if (!key.Equals(key2, StringComparison.OrdinalIgnoreCase))
				{
					this._weaponInstance.SwapSkillOrder();
				}
			}
			this._weaponInstance.SetSkillButtons();
		}

		// Token: 0x040037AD RID: 14253
		[SerializeField]
		private bool _followSkillOrder;

		// Token: 0x040037AE RID: 14254
		[SerializeField]
		private Weapon _polymorphWeapon;

		// Token: 0x040037AF RID: 14255
		[SerializeField]
		private StartWeaponPolymorph.SkillMap.Reorderable _skillMaps;

		// Token: 0x040037B0 RID: 14256
		private Weapon _weaponInstance;

		// Token: 0x040037B1 RID: 14257
		private Weapon _before;

		// Token: 0x040037B2 RID: 14258
		private List<SkillInfo> _matchedSkillInfos;

		// Token: 0x02000E3D RID: 3645
		[Serializable]
		private class SkillMap
		{
			// Token: 0x040037B3 RID: 14259
			public SkillInfo originalSkill;

			// Token: 0x040037B4 RID: 14260
			public string polymorphSkillKey;

			// Token: 0x040037B5 RID: 14261
			public bool copyCooldown;

			// Token: 0x02000E3E RID: 3646
			[Serializable]
			public class Reorderable : ReorderableArray<StartWeaponPolymorph.SkillMap>
			{
			}
		}
	}
}
