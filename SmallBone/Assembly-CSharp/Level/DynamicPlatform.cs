using System;
using System.Collections.Generic;
using Characters.Movements;
using UnityEngine;

namespace Level
{
	// Token: 0x020004D6 RID: 1238
	public class DynamicPlatform : MonoBehaviour
	{
		// Token: 0x0600181F RID: 6175 RVA: 0x0004BB28 File Offset: 0x00049D28
		private void Awake()
		{
			this._positionBefore = base.transform.position;
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0004BB3C File Offset: 0x00049D3C
		private void Update()
		{
			if (this._controllers.Count > 0)
			{
				Vector3 v = base.transform.position - this._positionBefore;
				Physics2D.SyncTransforms();
				for (int i = 0; i < this._controllers.Count; i++)
				{
					this._controllers[i].Move(v);
				}
			}
			this._positionBefore = base.transform.position;
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x0004BBB2 File Offset: 0x00049DB2
		public void Attach(CharacterController2D controller)
		{
			this._controllers.Add(controller);
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0004BBC0 File Offset: 0x00049DC0
		public bool Detach(CharacterController2D controller)
		{
			return this._controllers.Remove(controller);
		}

		// Token: 0x04001507 RID: 5383
		private readonly List<CharacterController2D> _controllers = new List<CharacterController2D>();

		// Token: 0x04001508 RID: 5384
		private Vector3 _positionBefore;
	}
}
