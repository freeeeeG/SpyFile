using System;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006F8 RID: 1784
	[Serializable]
	public class Force
	{
		// Token: 0x06002400 RID: 9216 RVA: 0x0006C4A8 File Offset: 0x0006A6A8
		public Vector2 Evaluate(Character character, float extraPower = 0f)
		{
			float value = this._angle.value;
			Vector2 result = new Vector2(Mathf.Cos(value * 0.017453292f), Mathf.Sin(value * 0.017453292f)) * (this._power.value + extraPower);
			if (this._method == Force.Method.LookingDirection && character.lookingDirection != Character.LookingDirection.Right)
			{
				result.x *= -1f;
			}
			return result;
		}

		// Token: 0x04001EC2 RID: 7874
		[SerializeField]
		private CustomFloat _angle = new CustomFloat(0f);

		// Token: 0x04001EC3 RID: 7875
		[SerializeField]
		private CustomFloat _power = new CustomFloat(0f);

		// Token: 0x04001EC4 RID: 7876
		[SerializeField]
		private Force.Method _method;

		// Token: 0x020006F9 RID: 1785
		public enum Method
		{
			// Token: 0x04001EC6 RID: 7878
			LookingDirection,
			// Token: 0x04001EC7 RID: 7879
			Constant
		}
	}
}
