using System;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x02001132 RID: 4402
	public class EnergyCropsContainer : MonoBehaviour
	{
		// Token: 0x06005596 RID: 21910 RVA: 0x000FF5CB File Offset: 0x000FD7CB
		private void OnEnable()
		{
			this.Generate();
		}

		// Token: 0x06005597 RID: 21911 RVA: 0x00002191 File Offset: 0x00000391
		private void Generate()
		{
		}

		// Token: 0x04004493 RID: 17555
		[SerializeField]
		private Transform _leftBottom;

		// Token: 0x04004494 RID: 17556
		[SerializeField]
		private GameObject _orb;

		// Token: 0x04004495 RID: 17557
		[SerializeField]
		private int _width;

		// Token: 0x04004496 RID: 17558
		[SerializeField]
		private int _height;

		// Token: 0x04004497 RID: 17559
		[SerializeField]
		private float _distance;

		// Token: 0x04004498 RID: 17560
		[SerializeField]
		private float _noise;
	}
}
