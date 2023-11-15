using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001465 RID: 5221
	[TaskDescription("Character의 Chronometer.Time을 가져옵니다.")]
	public sealed class GetCharacterChronometer : Action
	{
		// Token: 0x060065F7 RID: 26103 RVA: 0x00126A50 File Offset: 0x00124C50
		public override void OnAwake()
		{
			this._characterValue = this._character.Value;
			if (!this._characterValue.gameObject.activeInHierarchy)
			{
				return;
			}
			this._time = new ChronometerTime(this._characterValue.chronometer.master, this._characterValue);
		}

		// Token: 0x060065F8 RID: 26104 RVA: 0x00126AA4 File Offset: 0x00124CA4
		public override TaskStatus OnUpdate()
		{
			if (this._characterValue == null)
			{
				return TaskStatus.Failure;
			}
			if (this._time == null)
			{
				this._time = new ChronometerTime(this._characterValue.chronometer.master, this._characterValue);
			}
			this.storeResult.SetValue(this._time.time);
			return TaskStatus.Success;
		}

		// Token: 0x040051EE RID: 20974
		[SerializeField]
		private SharedCharacter _character;

		// Token: 0x040051EF RID: 20975
		[SerializeField]
		private SharedFloat storeResult;

		// Token: 0x040051F0 RID: 20976
		private Character _characterValue;

		// Token: 0x040051F1 RID: 20977
		private ChronometerTime _time;
	}
}
