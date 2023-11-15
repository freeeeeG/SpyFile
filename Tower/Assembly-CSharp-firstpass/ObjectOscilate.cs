using System;
using UnityEngine;

namespace Highlighters
{
	// Token: 0x02000011 RID: 17
	public class ObjectOscilate : MonoBehaviour
	{
		// Token: 0x06000048 RID: 72 RVA: 0x000030BC File Offset: 0x000012BC
		private void Update()
		{
			float t = Mathf.PingPong(Time.time * this.speed, 1f);
			float x = Mathf.Lerp(this.minX, this.maxX, t);
			base.transform.localPosition = new Vector3(x, base.transform.localPosition.y, base.transform.localPosition.z);
		}

		// Token: 0x04000037 RID: 55
		public float minX;

		// Token: 0x04000038 RID: 56
		public float maxX;

		// Token: 0x04000039 RID: 57
		public float speed = 1f;
	}
}
