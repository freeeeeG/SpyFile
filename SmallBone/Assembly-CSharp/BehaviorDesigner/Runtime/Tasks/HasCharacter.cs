using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x02001469 RID: 5225
	[TaskDescription("Allows multiple action tasks to be added to a single node.")]
	[TaskIcon("{SkinColor}TurnOnEdge.png")]
	public sealed class HasCharacter : Action
	{
		// Token: 0x06006601 RID: 26113 RVA: 0x00126C44 File Offset: 0x00124E44
		public override TaskStatus OnUpdate()
		{
			Character value = this._target.Value;
			if (value == null)
			{
				return TaskStatus.Failure;
			}
			if (this._onSamePlatform)
			{
				Collider2D lastStandingCollider = value.movement.controller.collisionState.lastStandingCollider;
				if (lastStandingCollider == null)
				{
					return TaskStatus.Failure;
				}
				if (this._owner.Value.movement.controller.collisionState.lastStandingCollider != lastStandingCollider)
				{
					return TaskStatus.Failure;
				}
			}
			return TaskStatus.Success;
		}

		// Token: 0x040051FA RID: 20986
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x040051FB RID: 20987
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x040051FC RID: 20988
		[SerializeField]
		private bool _onSamePlatform;
	}
}
