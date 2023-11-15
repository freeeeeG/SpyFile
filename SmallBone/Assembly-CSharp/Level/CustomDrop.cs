using System;
using System.Collections;
using Services;
using Singletons;
using UnityEngine;

namespace Level
{
	// Token: 0x020004AA RID: 1194
	public class CustomDrop : MonoBehaviour
	{
		// Token: 0x060016F1 RID: 5873 RVA: 0x0004842A File Offset: 0x0004662A
		private void Awake()
		{
			this._levelManager = Singleton<Service>.Instance.levelManager;
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x0004843C File Offset: 0x0004663C
		private void OnEnable()
		{
			this._levelManager.RegisterDrop(this._poolObject);
			base.StartCoroutine(this.CDespawnAfterLifetime());
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x0004845C File Offset: 0x0004665C
		private void OnDisable()
		{
			if (Service.quitting)
			{
				return;
			}
			this._levelManager.DeregisterDrop(this._poolObject);
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00048477 File Offset: 0x00046677
		private IEnumerator CDespawnAfterLifetime()
		{
			yield return Chronometer.global.WaitForSeconds(this._lifetime);
			this._poolObject.Despawn();
			yield break;
		}

		// Token: 0x0400141C RID: 5148
		[SerializeField]
		private PoolObject _poolObject;

		// Token: 0x0400141D RID: 5149
		[SerializeField]
		private float _lifetime = 30f;

		// Token: 0x0400141E RID: 5150
		private LevelManager _levelManager;
	}
}
