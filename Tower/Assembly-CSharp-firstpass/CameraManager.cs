using System;
using UnityEngine;
using UnityEngine.UI;

namespace Highlighters
{
	// Token: 0x0200000E RID: 14
	public class CameraManager : MonoBehaviour
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00002A48 File Offset: 0x00000C48
		private void Start()
		{
			this.rotationSlider.onValueChanged.AddListener(delegate(float <p0>)
			{
				this.RotationValueChange();
			});
			this.distanceSlider.onValueChanged.AddListener(delegate(float <p0>)
			{
				this.DistanceValueChange();
			});
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002A84 File Offset: 0x00000C84
		public void RotationValueChange()
		{
			if (this.m_Locked)
			{
				this.yRot = this.rotationSlider.value;
				base.transform.position = this.target.position + this.transformv + Quaternion.Euler(this.xRot, this.yRot, 0f) * (this.distance * -Vector3.back);
				base.transform.LookAt(this.target.position + this.transformv, Vector3.up);
				return;
			}
			this.yRot = this.rotationSlider.value;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002B3C File Offset: 0x00000D3C
		public void DistanceValueChange()
		{
			if (this.m_Locked)
			{
				this.distance = this.distanceSlider.value;
				base.transform.position = this.target.position + this.transformv + Quaternion.Euler(this.xRot, this.yRot, 0f) * (this.distance * -Vector3.back);
				base.transform.LookAt(this.target.position + this.transformv, Vector3.up);
				return;
			}
			this.distance = this.distanceSlider.value;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002BF4 File Offset: 0x00000DF4
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				this.LockRotation();
			}
			if (Input.GetKeyDown(KeyCode.M))
			{
				this.yRot = this.RestartRotation;
			}
			if (this.m_Locked)
			{
				return;
			}
			this.yRot += this.sensitivity * Time.deltaTime;
			base.transform.position = this.target.position + this.transformv + Quaternion.Euler(this.xRot, this.yRot, 0f) * (this.distance * -Vector3.back);
			base.transform.LookAt(this.target.position + this.transformv, Vector3.up);
			if (this.yRot >= 360f)
			{
				this.yRot = 0f;
			}
			this.UiManager();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002CE1 File Offset: 0x00000EE1
		private void UiManager()
		{
			this.rotationSlider.value = this.yRot;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002CF4 File Offset: 0x00000EF4
		public void LockRotation()
		{
			this.m_Locked = !this.m_Locked;
		}

		// Token: 0x04000022 RID: 34
		public Transform target;

		// Token: 0x04000023 RID: 35
		public Vector3 transformv;

		// Token: 0x04000024 RID: 36
		public float xRot;

		// Token: 0x04000025 RID: 37
		public float RestartRotation;

		// Token: 0x04000026 RID: 38
		public float yRot;

		// Token: 0x04000027 RID: 39
		public float distance = 5f;

		// Token: 0x04000028 RID: 40
		public float sensitivity = 10f;

		// Token: 0x04000029 RID: 41
		public Slider rotationSlider;

		// Token: 0x0400002A RID: 42
		public Slider distanceSlider;

		// Token: 0x0400002B RID: 43
		private bool m_Locked;
	}
}
