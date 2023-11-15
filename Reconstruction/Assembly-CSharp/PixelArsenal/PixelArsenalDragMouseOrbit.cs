using System;
using UnityEngine;

namespace PixelArsenal
{
	// Token: 0x020002AD RID: 685
	public class PixelArsenalDragMouseOrbit : MonoBehaviour
	{
		// Token: 0x060010E7 RID: 4327 RVA: 0x0002F558 File Offset: 0x0002D758
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

		// Token: 0x060010E8 RID: 4328 RVA: 0x0002F5A4 File Offset: 0x0002D7A4
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
				this.rotationXAxis = PixelArsenalDragMouseOrbit.ClampAngle(this.rotationXAxis, this.yMinLimit, this.yMaxLimit);
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

		// Token: 0x060010E9 RID: 4329 RVA: 0x0002F75F File Offset: 0x0002D95F
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

		// Token: 0x04000915 RID: 2325
		public Transform target;

		// Token: 0x04000916 RID: 2326
		public float distance = 5f;

		// Token: 0x04000917 RID: 2327
		public float xSpeed = 120f;

		// Token: 0x04000918 RID: 2328
		public float ySpeed = 120f;

		// Token: 0x04000919 RID: 2329
		public float yMinLimit = -20f;

		// Token: 0x0400091A RID: 2330
		public float yMaxLimit = 80f;

		// Token: 0x0400091B RID: 2331
		public float distanceMin = 0.5f;

		// Token: 0x0400091C RID: 2332
		public float distanceMax = 15f;

		// Token: 0x0400091D RID: 2333
		public float smoothTime = 2f;

		// Token: 0x0400091E RID: 2334
		private float rotationYAxis;

		// Token: 0x0400091F RID: 2335
		private float rotationXAxis;

		// Token: 0x04000920 RID: 2336
		private float velocityX;

		// Token: 0x04000921 RID: 2337
		private float velocityY;
	}
}
