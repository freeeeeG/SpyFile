using System;
using UnityEngine;

namespace DinoPinBall
{
	// Token: 0x020001C1 RID: 449
	public class Func_RandomRotationOnEnable : MonoBehaviour
	{
		// Token: 0x06000BCB RID: 3019 RVA: 0x0002E48C File Offset: 0x0002C68C
		private void OnEnable()
		{
			Vector3 eulerAngles = base.transform.localRotation.eulerAngles;
			Vector3 euler = Random.insideUnitSphere * 360f;
			if (!this.x_Axis)
			{
				euler.x = eulerAngles.x;
			}
			if (!this.y_Axis)
			{
				euler.y = eulerAngles.y;
			}
			if (!this.z_Axis)
			{
				euler.z = eulerAngles.z;
			}
			base.transform.localRotation = Quaternion.Euler(euler);
		}

		// Token: 0x04000966 RID: 2406
		[SerializeField]
		private bool x_Axis = true;

		// Token: 0x04000967 RID: 2407
		[SerializeField]
		private bool y_Axis = true;

		// Token: 0x04000968 RID: 2408
		[SerializeField]
		private bool z_Axis = true;
	}
}
