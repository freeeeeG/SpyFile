using System;
using UnityEngine;

// Token: 0x02000034 RID: 52
public class Floater : MonoBehaviour
{
	// Token: 0x060000D1 RID: 209 RVA: 0x00004C40 File Offset: 0x00002E40
	private void Start()
	{
		this._originalPosition = base.transform.position;
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00004C54 File Offset: 0x00002E54
	private void Update()
	{
		this._floatingPosition = this._originalPosition;
		this._floatingPosition.y = this._floatingPosition.y + Mathf.Sin(Time.fixedTime * 3.1415927f * this._frequency) * this._amplitude;
		base.transform.position = this._floatingPosition;
	}

	// Token: 0x040000B8 RID: 184
	[SerializeField]
	private float _amplitude = 0.2f;

	// Token: 0x040000B9 RID: 185
	[SerializeField]
	private float _frequency = 1f;

	// Token: 0x040000BA RID: 186
	private Vector3 _originalPosition;

	// Token: 0x040000BB RID: 187
	private Vector3 _floatingPosition;
}
