using System;
using Characters;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom
{
	// Token: 0x02001651 RID: 5713
	[TaskDescription("스테이지별로 보정된 모험가 CastingBreakDamage를 반환합니다")]
	public sealed class GetAdventurerCastingBreakDamage : Action
	{
		// Token: 0x06006CEC RID: 27884 RVA: 0x001375C0 File Offset: 0x001357C0
		public override void OnAwake()
		{
			this._originValue = this._storeResult.Value;
		}

		// Token: 0x06006CED RID: 27885 RVA: 0x001375D4 File Offset: 0x001357D4
		public override TaskStatus OnUpdate()
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			float num = this._originValue * currentChapter.currentStage.adventurerCastingBreakDamageMultiplier;
			if (GameData.HardmodeProgress.hardmode)
			{
				float castingBreakDamageMultiplier = HardmodeLevelInfo.instance.GetEnemyStatInfoByType(Character.Type.Adventurer).castingBreakDamageMultiplier;
				num *= castingBreakDamageMultiplier;
			}
			this._storeResult.SetValue(num);
			return TaskStatus.Success;
		}

		// Token: 0x040058B1 RID: 22705
		[SerializeField]
		private SharedFloat _storeResult;

		// Token: 0x040058B2 RID: 22706
		private float _originValue;
	}
}
