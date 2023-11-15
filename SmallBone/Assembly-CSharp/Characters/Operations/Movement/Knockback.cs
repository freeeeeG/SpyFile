using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E5A RID: 3674
	public class Knockback : TargetedCharacterOperation
	{
		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x060048F5 RID: 18677 RVA: 0x000D4C42 File Offset: 0x000D2E42
		public PushInfo pushInfo
		{
			get
			{
				return this._pushInfo;
			}
		}

		// Token: 0x060048F6 RID: 18678 RVA: 0x000D4C4C File Offset: 0x000D2E4C
		public override void Run(Character owner, Character target)
		{
			if (target == null || !target.liveAndActive)
			{
				return;
			}
			if (this._transfromOverride == null)
			{
				target.movement.push.ApplyKnockback(owner, this._pushInfo);
				return;
			}
			target.movement.push.ApplyKnockback(this._transfromOverride, this._pushInfo);
		}

		// Token: 0x04003810 RID: 14352
		[SerializeField]
		[Information("이 값을 지정해주면 오퍼레이션 소유 캐릭터 대신 해당 트랜스폼을 기준으로 넉백합니다.", InformationAttribute.InformationType.Info, false)]
		private Transform _transfromOverride;

		// Token: 0x04003811 RID: 14353
		[SerializeField]
		private PushInfo _pushInfo = new PushInfo(false, false);
	}
}
