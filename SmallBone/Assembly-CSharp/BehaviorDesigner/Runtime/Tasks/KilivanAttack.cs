using System;
using Characters;
using Characters.Actions;
using Characters.AI.Hero;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001462 RID: 5218
	[TaskDescription("커스텀 : 검은 초대용사 킬리반 공격")]
	public sealed class KilivanAttack : BehaviorDesigner.Runtime.Tasks.Action
	{
		// Token: 0x060065EC RID: 26092 RVA: 0x00126674 File Offset: 0x00124874
		public override void OnStart()
		{
			Character value = this._target.Value;
			Character value2 = this._character.Value;
			RaycastHit2D raycastHit2D;
			value.movement.TryBelowRayCast(262144, out raycastHit2D, 100f);
			Vector2 direction = raycastHit2D.point - value2.transform.position;
			this._destination = this._kilivanFinish.Fire(direction);
		}

		// Token: 0x060065ED RID: 26093 RVA: 0x001266E3 File Offset: 0x001248E3
		public override TaskStatus OnUpdate()
		{
			if (!this._throw.running)
			{
				this._throw.TryStart();
			}
			if (!this._kilivanFinish.UpdateMove(Chronometer.global.deltaTime, this._destination))
			{
				return TaskStatus.Success;
			}
			return TaskStatus.Running;
		}

		// Token: 0x040051D8 RID: 20952
		[SerializeField]
		private SharedCharacter _character;

		// Token: 0x040051D9 RID: 20953
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x040051DA RID: 20954
		[SerializeField]
		private KilivanFinish _kilivanFinish;

		// Token: 0x040051DB RID: 20955
		[SerializeField]
		private Characters.Actions.Action _throw;

		// Token: 0x040051DC RID: 20956
		private Vector2 _destination;
	}
}
