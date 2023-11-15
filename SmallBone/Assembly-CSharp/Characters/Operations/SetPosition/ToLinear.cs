using System;
using UnityEngine;

namespace Characters.Operations.SetPosition
{
	// Token: 0x02000EE9 RID: 3817
	public sealed class ToLinear : Policy
	{
		// Token: 0x06004ADB RID: 19163 RVA: 0x000D950E File Offset: 0x000D770E
		public override Vector2 GetPosition(Character owner)
		{
			return this.GetPosition();
		}

		// Token: 0x06004ADC RID: 19164 RVA: 0x000DBACA File Offset: 0x000D9CCA
		public override Vector2 GetPosition()
		{
			return Vector2.Lerp(this._from.position, this._to.position, this._value);
		}

		// Token: 0x04003A0C RID: 14860
		[Range(0f, 1f)]
		[SerializeField]
		private float _value;

		// Token: 0x04003A0D RID: 14861
		[SerializeField]
		private Transform _from;

		// Token: 0x04003A0E RID: 14862
		[SerializeField]
		private Transform _to;
	}
}
