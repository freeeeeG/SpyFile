using System;
using System.Collections.Generic;
using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014B6 RID: 5302
	[TaskDescription("콜라이더 범위 내에서 설정된 캐릭터들을 찾습니다.아무것도 찾지 못하면 Fail를 반환하고 하나 이상이라도 찾으면 Success를 반환 합니다.")]
	public sealed class IsFoundCharactersInRange : Conditional
	{
		// Token: 0x06006730 RID: 26416 RVA: 0x0012AC4C File Offset: 0x00128E4C
		public override void OnAwake()
		{
			this._overlapper = new NonAllocOverlapper(31);
			this._ownerValue = this._owner.Value;
			this._rangeValue = this._range.Value;
		}

		// Token: 0x06006731 RID: 26417 RVA: 0x0012AC80 File Offset: 0x00128E80
		public override TaskStatus OnUpdate()
		{
			this._rangeValue.enabled = true;
			this._overlapper.contactFilter.SetLayerMask(this._targetLayer.Evaluate(this._ownerValue.gameObject));
			List<Character> components = this._overlapper.OverlapCollider(this._rangeValue).GetComponents<Character>(true);
			if (!this._inculdeOwner)
			{
				components.Remove(this._ownerValue);
			}
			if (components.Count == 0)
			{
				return TaskStatus.Failure;
			}
			if (this._targetsList != null)
			{
				this._targetsList.SetValue(components);
			}
			return TaskStatus.Success;
		}

		// Token: 0x0400533C RID: 21308
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x0400533D RID: 21309
		[SerializeField]
		private SharedCollider _range;

		// Token: 0x0400533E RID: 21310
		[SerializeField]
		private TargetLayer _targetLayer = new TargetLayer(0, false, true, false, false);

		// Token: 0x0400533F RID: 21311
		[SerializeField]
		private bool _inculdeOwner;

		// Token: 0x04005340 RID: 21312
		[SerializeField]
		private SharedCharacterList _targetsList;

		// Token: 0x04005341 RID: 21313
		private NonAllocOverlapper _overlapper;

		// Token: 0x04005342 RID: 21314
		private Character _ownerValue;

		// Token: 0x04005343 RID: 21315
		private Collider2D _rangeValue;
	}
}
