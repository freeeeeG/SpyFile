using System;
using Characters;
using FX.Connections;
using UnityEngine;

namespace Level.Objects
{
	// Token: 0x02000575 RID: 1397
	public class DivineShieldEffect : MonoBehaviour
	{
		// Token: 0x06001B71 RID: 7025 RVA: 0x00055428 File Offset: 0x00053628
		public void Activate(Character target)
		{
			base.gameObject.SetActive(true);
			Vector2 endOffset = new Vector2(0f, target.collider.size.y * 0.5f);
			this._connection.Connect(base.transform, Vector2.zero, target.transform, endOffset);
		}

		// Token: 0x0400179B RID: 6043
		[SerializeField]
		private Connection _connection;
	}
}
