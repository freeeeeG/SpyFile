using System;
using UnityEngine;

namespace ModularOptions
{
	// Token: 0x020000A1 RID: 161
	[AddComponentMenu("Modular Options/Misc/First Person Camera Rotation")]
	public class FirstPersonCameraRotation : MonoBehaviour
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00008FD3 File Offset: 0x000071D3
		// (set) Token: 0x06000230 RID: 560 RVA: 0x00008FDB File Offset: 0x000071DB
		public float Sensitivity
		{
			get
			{
				return this.sensitivity;
			}
			set
			{
				this.sensitivity = value;
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00008FE4 File Offset: 0x000071E4
		private void Update()
		{
			this.rotation.x = this.rotation.x + Input.GetAxis("Mouse X") * this.sensitivity;
			this.rotation.y = this.rotation.y + Input.GetAxis("Mouse Y") * this.sensitivity;
			this.rotation.y = Mathf.Clamp(this.rotation.y, -this.yRotationLimit, this.yRotationLimit);
			Quaternion lhs = Quaternion.AngleAxis(this.rotation.x, Vector3.up);
			Quaternion rhs = Quaternion.AngleAxis(this.rotation.y, Vector3.left);
			base.transform.localRotation = lhs * rhs;
		}

		// Token: 0x040001E0 RID: 480
		[Range(0.1f, 9f)]
		[SerializeField]
		private float sensitivity = 2f;

		// Token: 0x040001E1 RID: 481
		[Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
		[Range(0f, 90f)]
		[SerializeField]
		private float yRotationLimit = 88f;

		// Token: 0x040001E2 RID: 482
		private Vector2 rotation = Vector2.zero;

		// Token: 0x040001E3 RID: 483
		private const string xAxis = "Mouse X";

		// Token: 0x040001E4 RID: 484
		private const string yAxis = "Mouse Y";
	}
}
