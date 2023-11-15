using System;
using UnityEngine;

namespace BT
{
	// Token: 0x0200141F RID: 5151
	public class TranslateTowards : Node
	{
		// Token: 0x0600653D RID: 25917 RVA: 0x0012533E File Offset: 0x0012353E
		protected override void OnInitialize()
		{
			this._speedXValue = this._speedX.value;
			this._speedYValue = this._speedY.value;
			base.OnInitialize();
		}

		// Token: 0x0600653E RID: 25918 RVA: 0x00125368 File Offset: 0x00123568
		protected override NodeState UpdateDeltatime(Context context)
		{
			Transform transform = context.Get<Transform>(Key.OwnerTransform);
			if (transform == null)
			{
				Debug.LogError("OwnerTransform is null");
				return NodeState.Fail;
			}
			float deltaTime = context.deltaTime;
			Vector2 a = Vector2.zero;
			a += (MMMaths.Chance(this._rightChance) ? Vector2.right : Vector2.left) * this._speedXValue;
			a += (MMMaths.Chance(this._upChance) ? Vector2.up : Vector2.down) * this._speedYValue;
			transform.Translate(a * deltaTime);
			return NodeState.Success;
		}

		// Token: 0x0400518D RID: 20877
		[SerializeField]
		private CustomFloat _speedX;

		// Token: 0x0400518E RID: 20878
		[SerializeField]
		private CustomFloat _speedY;

		// Token: 0x0400518F RID: 20879
		[Range(0f, 1f)]
		[SerializeField]
		private float _rightChance;

		// Token: 0x04005190 RID: 20880
		[Range(0f, 1f)]
		[SerializeField]
		private float _upChance;

		// Token: 0x04005191 RID: 20881
		private float _speedXValue;

		// Token: 0x04005192 RID: 20882
		private float _speedYValue;
	}
}
