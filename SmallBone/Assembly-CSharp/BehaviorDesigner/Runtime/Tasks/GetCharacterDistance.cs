using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001466 RID: 5222
	[TaskDescription("현재 챕터의 Stage Index를 저장합니다.")]
	public sealed class GetCharacterDistance : Action
	{
		// Token: 0x060065FA RID: 26106 RVA: 0x00126B06 File Offset: 0x00124D06
		public override void OnAwake()
		{
			this._ownerValue = this._owner.Value;
		}

		// Token: 0x060065FB RID: 26107 RVA: 0x00126B1C File Offset: 0x00124D1C
		public override TaskStatus OnUpdate()
		{
			if (this._ownerValue == null || this._target.Value == null)
			{
				return TaskStatus.Failure;
			}
			float num;
			if (!this._compareOnlyXDistance)
			{
				num = Vector2.Distance(this._target.Value.transform.position, this._ownerValue.transform.position);
			}
			else
			{
				num = Mathf.Abs(this._target.Value.transform.position.x - this._ownerValue.transform.position.x);
			}
			this._storedDistance.SetValue(num);
			return TaskStatus.Success;
		}

		// Token: 0x040051F2 RID: 20978
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x040051F3 RID: 20979
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x040051F4 RID: 20980
		[SerializeField]
		private SharedFloat _storedDistance;

		// Token: 0x040051F5 RID: 20981
		[SerializeField]
		private bool _compareOnlyXDistance;

		// Token: 0x040051F6 RID: 20982
		private Character _ownerValue;
	}
}
