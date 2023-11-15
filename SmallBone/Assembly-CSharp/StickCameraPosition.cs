using System;
using UnityEngine;

// Token: 0x0200002D RID: 45
public class StickCameraPosition : MonoBehaviour
{
	// Token: 0x060000A9 RID: 169 RVA: 0x000044BA File Offset: 0x000026BA
	public void Start()
	{
		this._camera = Camera.main;
	}

	// Token: 0x060000AA RID: 170 RVA: 0x000044C7 File Offset: 0x000026C7
	public void Update()
	{
		this._camera.transform.position = base.transform.position + this._offset;
	}

	// Token: 0x04000098 RID: 152
	[SerializeField]
	private Vector3 _offset;

	// Token: 0x04000099 RID: 153
	private Camera _camera;
}
