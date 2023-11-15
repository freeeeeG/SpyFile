using System;
using UnityEngine;

namespace Highlighters
{
	// Token: 0x02000010 RID: 16
	public class FloatingObject : MonoBehaviour
	{
		// Token: 0x06000045 RID: 69 RVA: 0x0000303B File Offset: 0x0000123B
		private void Start()
		{
			this.startPos = base.transform.position;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003050 File Offset: 0x00001250
		private void Update()
		{
			float y = this.amplitude * Mathf.Sin(Time.time * this.frequency);
			base.transform.position = this.startPos + new Vector3(0f, y, 0f);
		}

		// Token: 0x04000034 RID: 52
		public float amplitude = 0.5f;

		// Token: 0x04000035 RID: 53
		public float frequency = 1f;

		// Token: 0x04000036 RID: 54
		private Vector3 startPos;
	}
}
