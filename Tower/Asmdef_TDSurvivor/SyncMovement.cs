using System;
using UnityEngine;

// Token: 0x020001A7 RID: 423
public class SyncMovement : MonoBehaviour
{
	// Token: 0x06000B67 RID: 2919 RVA: 0x0002C8B7 File Offset: 0x0002AAB7
	private void Start()
	{
		if (this.targetObject == null)
		{
			Debug.LogError("SyncMovement: 請設定目標物件。");
			return;
		}
		this._lastTargetPosition = this.targetObject.transform.position;
	}

	// Token: 0x06000B68 RID: 2920 RVA: 0x0002C8E8 File Offset: 0x0002AAE8
	private void Update()
	{
		Vector3 b = this.targetObject.transform.position - this._lastTargetPosition;
		base.transform.position += b;
		this._lastTargetPosition = this.targetObject.transform.position;
	}

	// Token: 0x04000918 RID: 2328
	[SerializeField]
	private GameObject targetObject;

	// Token: 0x04000919 RID: 2329
	private Vector3 _lastTargetPosition;
}
