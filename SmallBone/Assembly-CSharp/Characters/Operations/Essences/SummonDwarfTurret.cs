using System;
using Characters.Abilities.Essences;
using Characters.Gear.Quintessences;
using Characters.Minions;
using UnityEngine;

namespace Characters.Operations.Essences
{
	// Token: 0x02000EB3 RID: 3763
	public sealed class SummonDwarfTurret : CharacterOperation
	{
		// Token: 0x06004A0A RID: 18954 RVA: 0x000D83E4 File Offset: 0x000D65E4
		public override void Run(Character owner)
		{
			if (owner.playerComponents == null)
			{
				return;
			}
			if (this._spawnPositions.Length == 0)
			{
				this.Summon(owner, owner.transform.position);
				return;
			}
			foreach (Transform transform in this._spawnPositions)
			{
				this.Summon(owner, transform.position);
			}
		}

		// Token: 0x06004A0B RID: 18955 RVA: 0x000D843C File Offset: 0x000D663C
		private void Summon(Character owner, Vector3 position)
		{
			owner.playerComponents.minionLeader.Summon(this._dwarfTurret.minion, position, this._overrideSetting);
		}

		// Token: 0x0400393F RID: 14655
		[SerializeField]
		private DwarfComponent _dwarf;

		// Token: 0x04003940 RID: 14656
		[SerializeField]
		private DwarfTurret _dwarfTurret;

		// Token: 0x04003941 RID: 14657
		[SerializeField]
		[Information("비워둘 경우 Default 설정 값을 적용", InformationAttribute.InformationType.Info, false)]
		private MinionSetting _overrideSetting;

		// Token: 0x04003942 RID: 14658
		[SerializeField]
		[Information("비워둘 경우 플레이어 위치에 1마리 소환, 그 외에는 지정된 위치마다 소환됨", InformationAttribute.InformationType.Info, false)]
		private Transform[] _spawnPositions;
	}
}
