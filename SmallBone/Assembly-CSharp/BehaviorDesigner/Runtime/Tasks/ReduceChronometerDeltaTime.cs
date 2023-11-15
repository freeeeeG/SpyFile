using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001473 RID: 5235
	[TaskDescription("Time에서 Owner의 Master Chronometer의 DeltaTime만큼 감소시킴")]
	[TaskIcon("Assets/Behavior Designer/Icon/ReduceChronometerDeltaTime.png")]
	public sealed class ReduceChronometerDeltaTime : Action
	{
		// Token: 0x0600661B RID: 26139 RVA: 0x00127244 File Offset: 0x00125444
		public override TaskStatus OnUpdate()
		{
			Character value = this._owner.Value;
			this._time.SetValue(this._time.Value - value.chronometer.master.deltaTime);
			return TaskStatus.Success;
		}

		// Token: 0x04005217 RID: 21015
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x04005218 RID: 21016
		[SerializeField]
		private SharedFloat _time;
	}
}
