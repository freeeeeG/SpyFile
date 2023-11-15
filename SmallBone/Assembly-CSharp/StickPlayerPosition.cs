using System;
using Services;
using Singletons;
using UnityEngine;

// Token: 0x0200002E RID: 46
public class StickPlayerPosition : MonoBehaviour
{
	// Token: 0x060000AC RID: 172 RVA: 0x000044EF File Offset: 0x000026EF
	public void Start()
	{
		this._playerObject = Singleton<Service>.Instance.levelManager.player.gameObject;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x0000450B File Offset: 0x0000270B
	public void Update()
	{
		base.transform.position = this._playerObject.transform.position + this._offset;
	}

	// Token: 0x0400009A RID: 154
	[SerializeField]
	private Vector3 _offset;

	// Token: 0x0400009B RID: 155
	private GameObject _playerObject;
}
