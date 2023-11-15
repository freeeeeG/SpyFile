using System;
using Characters.Abilities;
using Characters.Abilities.Weapons;
using Characters.Gear.Weapons.Gauges;
using Level;
using UnityEngine;

namespace CutScenes.Shots.Events.Customs
{
	// Token: 0x0200021C RID: 540
	public sealed class DropScrollAndCells : Event
	{
		// Token: 0x06000AAE RID: 2734 RVA: 0x0001D06C File Offset: 0x0001B26C
		public override void Run()
		{
			Vector3 position = base.transform.position;
			position.y += 0.2f;
			UnityEngine.Object.Instantiate<PrisonerSkillScroll>(this._skillScroll, position, Quaternion.identity, Map.Instance.transform).SetSkillInfo(this._chest.weapon, this._chest.skills, this._chest.skillInfo);
			float value = this._cellCount.value;
			ValueGauge gauge = (ValueGauge)this._chest.weapon.gauge;
			this.ApplyBuff();
			int num = 0;
			while ((float)num < value)
			{
				this._cellPrefab.Spawn(base.transform.position, gauge);
				num++;
			}
		}

		// Token: 0x06000AAF RID: 2735 RVA: 0x0001D124 File Offset: 0x0001B324
		private void ApplyBuff()
		{
			if (this._buff == null)
			{
				return;
			}
			AbilityBuff abilityBuff = UnityEngine.Object.Instantiate<AbilityBuff>(this._buff);
			abilityBuff.name = this._buff.name;
			abilityBuff.Loot(this._chest.weapon.owner);
		}

		// Token: 0x040008B9 RID: 2233
		[SerializeField]
		private PrisonerChest _chest;

		// Token: 0x040008BA RID: 2234
		[SerializeField]
		private PrisonerSkillScroll _skillScroll;

		// Token: 0x040008BB RID: 2235
		[SerializeField]
		private DroppedCell _cellPrefab;

		// Token: 0x040008BC RID: 2236
		[SerializeField]
		private CustomFloat _cellCount;

		// Token: 0x040008BD RID: 2237
		[SerializeField]
		[Header("부여할 버프, 비워두면 부여하지 않음")]
		private AbilityBuff _buff;
	}
}
