using System;
using BT.SharedValues;
using UnityEngine;

namespace BT
{
	// Token: 0x02001426 RID: 5158
	public class SetOwnerTransform : MonoBehaviour
	{
		// Token: 0x06006551 RID: 25937 RVA: 0x00125696 File Offset: 0x00123896
		private void Awake()
		{
			this._runner.context.Set<Transform>(Key.OwnerTransform, new SharedValue<Transform>(this._transform));
		}

		// Token: 0x0400519D RID: 20893
		[SerializeField]
		private BehaviourTreeRunner _runner;

		// Token: 0x0400519E RID: 20894
		[SerializeField]
		private Transform _transform;
	}
}
