using System;
using UnityEngine;

namespace Level
{
	// Token: 0x0200048D RID: 1165
	public class AdventurerRewardSlot : MonoBehaviour
	{
		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001631 RID: 5681 RVA: 0x00045933 File Offset: 0x00043B33
		public Vector3 displayPosition
		{
			get
			{
				return this._displayPosition.position;
			}
		}

		// Token: 0x04001371 RID: 4977
		[NonSerialized]
		public DroppedGear droppedGear;

		// Token: 0x04001372 RID: 4978
		[SerializeField]
		private Transform _displayPosition;
	}
}
