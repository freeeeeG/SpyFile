using System;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x020002BD RID: 701
	public class ETFXMouseOrbit : MonoBehaviour
	{
		// Token: 0x0600112D RID: 4397 RVA: 0x00030D1C File Offset: 0x0002EF1C
		private void Start()
		{
			Vector3 eulerAngles = base.transform.eulerAngles;
			this.rotationYAxis = eulerAngles.y;
			this.rotationXAxis = eulerAngles.x;
			if (base.GetComponent<Rigidbody>())
			{
				base.GetComponent<Rigidbody>().freezeRotation = true;
			}
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00030D68 File Offset: 0x0002EF68
		private void LateUpdate()
		{
			if (this.target)
			{
				if (Input.GetMouseButton(1))
				{
					this.velocityX += this.xSpeed * Input.GetAxis("Mouse X") * this.distance * 0.02f;
					this.velocityY += this.ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
				}
				this.rotationYAxis += this.velocityX;
				this.rotationXAxis -= this.velocityY;
				this.rotationXAxis = ETFXMouseOrbit.ClampAngle(this.rotationXAxis, this.yMinLimit, this.yMaxLimit);
				Quaternion rotation = Quaternion.Euler(this.rotationXAxis, this.rotationYAxis, 0f);
				this.distance = Mathf.Clamp(this.distance - Input.GetAxis("Mouse ScrollWheel") * 5f, this.distanceMin, this.distanceMax);
				RaycastHit raycastHit;
				if (Physics.Linecast(this.target.position, base.transform.position, out raycastHit))
				{
					this.distance -= raycastHit.distance;
				}
				Vector3 point = new Vector3(0f, 0f, -this.distance);
				Vector3 position = rotation * point + this.target.position;
				base.transform.rotation = rotation;
				base.transform.position = position;
				this.velocityX = Mathf.Lerp(this.velocityX, 0f, Time.deltaTime * this.smoothTime);
				this.velocityY = Mathf.Lerp(this.velocityY, 0f, Time.deltaTime * this.smoothTime);
			}
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x00030F23 File Offset: 0x0002F123
		public static float ClampAngle(float angle, float min, float max)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
			return Mathf.Clamp(angle, min, max);
		}

		// Token: 0x04000970 RID: 2416
		public Transform target;

		// Token: 0x04000971 RID: 2417
		public float distance = 5f;

		// Token: 0x04000972 RID: 2418
		public float xSpeed = 120f;

		// Token: 0x04000973 RID: 2419
		public float ySpeed = 120f;

		// Token: 0x04000974 RID: 2420
		public float yMinLimit = -20f;

		// Token: 0x04000975 RID: 2421
		public float yMaxLimit = 80f;

		// Token: 0x04000976 RID: 2422
		public float distanceMin = 0.5f;

		// Token: 0x04000977 RID: 2423
		public float distanceMax = 15f;

		// Token: 0x04000978 RID: 2424
		public float smoothTime = 2f;

		// Token: 0x04000979 RID: 2425
		private float rotationYAxis;

		// Token: 0x0400097A RID: 2426
		private float rotationXAxis;

		// Token: 0x0400097B RID: 2427
		private float velocityX;

		// Token: 0x0400097C RID: 2428
		private float velocityY;
	}
}
