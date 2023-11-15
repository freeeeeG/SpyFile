using System;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014A6 RID: 5286
	[TaskDescription("캐릭터간 거리 비교")]
	public sealed class ComapreCharacterDistance : Conditional
	{
		// Token: 0x06006714 RID: 26388 RVA: 0x0012A314 File Offset: 0x00128514
		public override TaskStatus OnUpdate()
		{
			Character value = this._from.Value;
			Character value2 = this._to.Value;
			float value3 = this._distance.Value;
			float num = 0f;
			switch (this._axis)
			{
			case ComapreCharacterDistance.Axis.XAxis:
				num = Mathf.Abs(value.transform.position.x - value2.transform.position.x);
				break;
			case ComapreCharacterDistance.Axis.YAxis:
				num = Mathf.Abs(value.transform.position.y - value2.transform.position.y);
				break;
			case ComapreCharacterDistance.Axis.XYAxis:
				num = Vector2.Distance(value.transform.position, value2.transform.position);
				break;
			}
			ComapreCharacterDistance.Compare comparer = this._comparer;
			if (comparer != ComapreCharacterDistance.Compare.GreatherThan)
			{
				if (comparer != ComapreCharacterDistance.Compare.LessThan)
				{
					return TaskStatus.Failure;
				}
				if (num <= value3)
				{
					return TaskStatus.Success;
				}
				return TaskStatus.Failure;
			}
			else
			{
				if (num >= value3)
				{
					return TaskStatus.Success;
				}
				return TaskStatus.Failure;
			}
		}

		// Token: 0x040052FE RID: 21246
		[SerializeField]
		private SharedCharacter _from;

		// Token: 0x040052FF RID: 21247
		[SerializeField]
		private SharedCharacter _to;

		// Token: 0x04005300 RID: 21248
		[SerializeField]
		private ComapreCharacterDistance.Axis _axis;

		// Token: 0x04005301 RID: 21249
		[SerializeField]
		private SharedFloat _distance;

		// Token: 0x04005302 RID: 21250
		[SerializeField]
		private ComapreCharacterDistance.Compare _comparer;

		// Token: 0x020014A7 RID: 5287
		private enum Compare
		{
			// Token: 0x04005304 RID: 21252
			GreatherThan,
			// Token: 0x04005305 RID: 21253
			LessThan
		}

		// Token: 0x020014A8 RID: 5288
		private enum Axis
		{
			// Token: 0x04005307 RID: 21255
			XAxis,
			// Token: 0x04005308 RID: 21256
			YAxis,
			// Token: 0x04005309 RID: 21257
			XYAxis
		}
	}
}
