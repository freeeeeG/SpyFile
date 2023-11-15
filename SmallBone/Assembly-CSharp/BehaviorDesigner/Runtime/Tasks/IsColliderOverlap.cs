using System;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014B4 RID: 5300
	[TaskDescription("특정 레이어 마스크의 콜라이더에 오버랩 되는것이 있는지 확인합니다.하나라도 있으면 성공 없으면 실패를 반환합니다.")]
	public sealed class IsColliderOverlap : Conditional
	{
		// Token: 0x0600672B RID: 26411 RVA: 0x0012AAC1 File Offset: 0x00128CC1
		public override TaskStatus OnUpdate()
		{
			NonAllocOverlapper nonAllocOverlapper = new NonAllocOverlapper(31);
			nonAllocOverlapper.contactFilter.SetLayerMask(this._layerMask);
			if (nonAllocOverlapper.OverlapCollider(this._range.Value).results.Count == 0)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x04005336 RID: 21302
		[SerializeField]
		private SharedCollider _range;

		// Token: 0x04005337 RID: 21303
		[SerializeField]
		private LayerMask _layerMask;
	}
}
