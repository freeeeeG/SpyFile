using System;
using UnityEngine;

namespace Characters.AI
{
	// Token: 0x020010D8 RID: 4312
	public class MagicianBallAssigner : MonoBehaviour
	{
		// Token: 0x060053C3 RID: 21443 RVA: 0x000FB230 File Offset: 0x000F9430
		private void Awake()
		{
			this.Assign();
		}

		// Token: 0x060053C4 RID: 21444 RVA: 0x000FB238 File Offset: 0x000F9438
		private void Assign()
		{
			if (base.transform.childCount <= 0)
			{
				Debug.LogError(base.name + " has no child");
				return;
			}
			int num = 360 / base.transform.childCount;
			Vector3 vector = Vector2.up * this._radius;
			for (int i = 0; i < base.transform.childCount; i++)
			{
				base.transform.GetChild(i).position = base.transform.position + vector;
				base.transform.GetChild(i).rotation = Quaternion.Euler(0f, 0f, (float)(num * i + 90));
				vector = Quaternion.Euler(0f, 0f, (float)num) * vector;
			}
		}

		// Token: 0x0400434C RID: 17228
		[SerializeField]
		private float _radius;
	}
}
