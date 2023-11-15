using System;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FD1 RID: 4049
	public class ResizeBoundsX : Operation
	{
		// Token: 0x06004E5B RID: 20059 RVA: 0x000EAAA8 File Offset: 0x000E8CA8
		public override void Run()
		{
			float num = Mathf.Abs(this._end.position.x - this._start.position.x);
			float value = this._extraSizeX.value;
			float value2 = this._extraSizeY.value;
			this._collider.size = new Vector2(num + value, this._collider.size.y + value2);
			this._collider.offset = new Vector2(-num / 2f, this._collider.offset.y);
		}

		// Token: 0x04003E64 RID: 15972
		[SerializeField]
		private BoxCollider2D _collider;

		// Token: 0x04003E65 RID: 15973
		[SerializeField]
		private Transform _start;

		// Token: 0x04003E66 RID: 15974
		[SerializeField]
		private Transform _end;

		// Token: 0x04003E67 RID: 15975
		[SerializeField]
		private CustomFloat _extraSizeX = new CustomFloat(0f);

		// Token: 0x04003E68 RID: 15976
		[SerializeField]
		private CustomFloat _extraSizeY = new CustomFloat(0f);
	}
}
