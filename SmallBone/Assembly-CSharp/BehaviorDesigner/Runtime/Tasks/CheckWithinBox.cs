using System;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014A1 RID: 5281
	[TaskDescription("Allows multiple action tasks to be added to a single node.")]
	[TaskIcon("Assets/Behavior Designer/Icon/CheckWithinSight.png")]
	public sealed class CheckWithinBox : Conditional
	{
		// Token: 0x060066FE RID: 26366 RVA: 0x00129E22 File Offset: 0x00128022
		public override void OnAwake()
		{
			this._ownerValue = this._owner.Value;
		}

		// Token: 0x060066FF RID: 26367 RVA: 0x00129E38 File Offset: 0x00128038
		public override TaskStatus OnUpdate()
		{
			Character character = this.FindTarget();
			if (character == null)
			{
				return TaskStatus.Failure;
			}
			this._target.SetValue(character);
			return TaskStatus.Success;
		}

		// Token: 0x06006700 RID: 26368 RVA: 0x00129E64 File Offset: 0x00128064
		private Character FindTarget()
		{
			CheckWithinBox._overlapper.contactFilter.SetLayerMask(this._targetLayer.Evaluate(this._ownerValue.gameObject));
			return TargetFinder.FindClosestTarget(CheckWithinBox._overlapper, this._ownerValue.transform.position + this._boxOffset, this._boxSize, this._boxAngle);
		}

		// Token: 0x040052DB RID: 21211
		private static NonAllocOverlapper _overlapper = new NonAllocOverlapper(15);

		// Token: 0x040052DC RID: 21212
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x040052DD RID: 21213
		[SerializeField]
		private SharedCharacter _target;

		// Token: 0x040052DE RID: 21214
		[SerializeField]
		private Vector2 _boxOffset;

		// Token: 0x040052DF RID: 21215
		[SerializeField]
		private Vector2 _boxSize;

		// Token: 0x040052E0 RID: 21216
		[SerializeField]
		private float _boxAngle;

		// Token: 0x040052E1 RID: 21217
		[SerializeField]
		private TargetLayer _targetLayer = new TargetLayer(0, false, true, false, false);

		// Token: 0x040052E2 RID: 21218
		private Character _ownerValue;
	}
}
