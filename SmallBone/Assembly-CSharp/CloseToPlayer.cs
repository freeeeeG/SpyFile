using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class CloseToPlayer : MonoBehaviour
{
	// Token: 0x06000072 RID: 114 RVA: 0x00003A95 File Offset: 0x00001C95
	private void Start()
	{
		this._player = Singleton<Service>.Instance.levelManager.player.transform;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00003AB4 File Offset: 0x00001CB4
	private void Update()
	{
		float num = Mathf.Sign(this._player.position.x - base.transform.position.x);
		base.transform.Translate(new Vector2(num * this._owner.chronometer.master.deltaTime * this._speed, 0f));
	}

	// Token: 0x0400006B RID: 107
	[SerializeField]
	private Character _owner;

	// Token: 0x0400006C RID: 108
	[SerializeField]
	private float _speed;

	// Token: 0x0400006D RID: 109
	private Transform _player;
}
