using System;
using Data;
using UnityEngine;

namespace Characters.Gear.Upgrades
{
	// Token: 0x0200084F RID: 2127
	[Serializable]
	public class Upgrade
	{
		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06002C42 RID: 11330 RVA: 0x00018EC5 File Offset: 0x000170C5
		public GameData.Currency.Type currencyTypeByDiscard
		{
			get
			{
				return GameData.Currency.Type.Gold;
			}
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06002C43 RID: 11331 RVA: 0x0008784B File Offset: 0x00085A4B
		public int maxLevel
		{
			get
			{
				return this._abilityByLevel.components.Length;
			}
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x0008785A File Offset: 0x00085A5A
		public UpgradeAbility GetAbility(int level)
		{
			if (level == 0)
			{
				return null;
			}
			return this._abilityByLevel[level - 1];
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x0008786F File Offset: 0x00085A6F
		public void Attach(Character character)
		{
			this._owner = character;
			this._passive.Attach(character);
			this._abilityByLevel[0].Attach(character);
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x00087898 File Offset: 0x00085A98
		public void LevelUp(int level)
		{
			if (level > this.maxLevel || this.maxLevel == 0)
			{
				Debug.Log("대상의 레벨이 최대 레벨을 초과했습니다.");
				return;
			}
			this._abilityByLevel.DetachAll();
			this._abilityByLevel[level - 1].Attach(this._owner);
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x000878E5 File Offset: 0x00085AE5
		public void Detach()
		{
			this._passive.DetachAll();
			this._abilityByLevel.DetachAll();
		}

		// Token: 0x04002568 RID: 9576
		[UpgradeAbility.SubcomponentAttribute]
		[SerializeField]
		private UpgradeAbility.Subcomponents _passive;

		// Token: 0x04002569 RID: 9577
		[UpgradeAbility.SubcomponentAttribute]
		[SerializeField]
		private UpgradeAbility.Subcomponents _abilityByLevel;

		// Token: 0x0400256A RID: 9578
		private Character _owner;
	}
}
