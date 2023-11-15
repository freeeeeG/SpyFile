using System;
using UnityEngine;

// Token: 0x02000007 RID: 7
public class ActivateOnEnterZone : MonoBehaviour
{
	// Token: 0x06000013 RID: 19 RVA: 0x00002A6D File Offset: 0x00000C6D
	private void OnTriggerEnter2D(Collider2D collision)
	{
		this._target.gameObject.SetActive(true);
		UnityEngine.Object.Destroy(this._collider);
		UnityEngine.Object.Destroy(this._target, 10f);
	}

	// Token: 0x04000014 RID: 20
	[SerializeField]
	private GameObject _target;

	// Token: 0x04000015 RID: 21
	[GetComponent]
	[SerializeField]
	private Collider2D _collider;
}
