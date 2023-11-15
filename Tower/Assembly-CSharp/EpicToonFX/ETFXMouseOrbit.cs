using System;
using System.Collections;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x02000064 RID: 100
	public class ETFXMouseOrbit : MonoBehaviour
	{
		// Token: 0x0600014A RID: 330 RVA: 0x00006674 File Offset: 0x00004874
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

		// Token: 0x0600014B RID: 331 RVA: 0x000066C0 File Offset: 0x000048C0
		private void Update()
		{
			if (this.target)
			{
				if (Input.GetMouseButton(1))
				{
					this.velocityX += this.xSpeed * Input.GetAxis("Mouse X") * this.distance * 0.02f;
					this.velocityY += this.ySpeed * Input.GetAxis("Mouse Y") * 0.02f;
					if (this.isAutoRotating)
					{
						this.StopAutoRotation();
					}
				}
				this.distance = Mathf.Clamp(this.distance - Input.GetAxis("Mouse ScrollWheel") * 15f, this.distanceMin, this.distanceMax);
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00006770 File Offset: 0x00004970
		private void FixedUpdate()
		{
			if (this.target)
			{
				this.rotationYAxis += this.velocityX;
				this.rotationXAxis -= this.velocityY;
				this.rotationXAxis = ETFXMouseOrbit.ClampAngle(this.rotationXAxis, this.yMinLimit, this.yMaxLimit);
				Quaternion rotation = Quaternion.Euler(this.rotationXAxis, this.rotationYAxis, 0f);
				RaycastHit raycastHit;
				if (Physics.Linecast(this.target.position, base.transform.position, out raycastHit))
				{
					this.distance -= raycastHit.distance;
				}
				Vector3 point = new Vector3(0f, 0f, -this.distance);
				Vector3 position = Vector3.Lerp(base.transform.position, rotation * point + this.target.position, 0.6f);
				base.transform.rotation = rotation;
				base.transform.position = position;
				this.velocityX = Mathf.Lerp(this.velocityX, 0f, Time.deltaTime * this.smoothTime);
				this.velocityY = Mathf.Lerp(this.velocityY, 0f, Time.deltaTime * this.smoothTime);
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x000068BB File Offset: 0x00004ABB
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

		// Token: 0x0600014E RID: 334 RVA: 0x000068E7 File Offset: 0x00004AE7
		public void InitializeAutoRotation()
		{
			this.isAutoRotating = true;
			base.StartCoroutine(this.AutoRotate());
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000068FD File Offset: 0x00004AFD
		public void SetAutoRotationSpeed(float rotationSpeed)
		{
			this.maxVelocityX = rotationSpeed;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00006908 File Offset: 0x00004B08
		private void StopAutoRotation()
		{
			if (this.etfxEffectController != null)
			{
				this.etfxEffectController.autoRotation = false;
			}
			if (this.etfxEffectControllerPooled != null)
			{
				this.etfxEffectControllerPooled.autoRotation = false;
			}
			this.isAutoRotating = false;
			base.StopAllCoroutines();
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006956 File Offset: 0x00004B56
		private IEnumerator AutoRotate()
		{
			int lerpSteps = 0;
			while (lerpSteps < 30)
			{
				this.velocityX = Mathf.Lerp(this.velocityX, this.maxVelocityX, this.autoRotationSmoothing);
				yield return new WaitForFixedUpdate();
			}
			while (this.isAutoRotating)
			{
				this.velocityX = this.maxVelocityX;
				yield return new WaitForFixedUpdate();
			}
			yield break;
		}

		// Token: 0x04000154 RID: 340
		public Transform target;

		// Token: 0x04000155 RID: 341
		public float distance = 12f;

		// Token: 0x04000156 RID: 342
		public float xSpeed = 120f;

		// Token: 0x04000157 RID: 343
		public float ySpeed = 120f;

		// Token: 0x04000158 RID: 344
		public float yMinLimit = -20f;

		// Token: 0x04000159 RID: 345
		public float yMaxLimit = 80f;

		// Token: 0x0400015A RID: 346
		public float distanceMin = 8f;

		// Token: 0x0400015B RID: 347
		public float distanceMax = 15f;

		// Token: 0x0400015C RID: 348
		public float smoothTime = 2f;

		// Token: 0x0400015D RID: 349
		private float rotationYAxis;

		// Token: 0x0400015E RID: 350
		private float rotationXAxis;

		// Token: 0x0400015F RID: 351
		private float velocityX;

		// Token: 0x04000160 RID: 352
		private float maxVelocityX = 0.1f;

		// Token: 0x04000161 RID: 353
		private float velocityY;

		// Token: 0x04000162 RID: 354
		private readonly float autoRotationSmoothing = 0.02f;

		// Token: 0x04000163 RID: 355
		[HideInInspector]
		public bool isAutoRotating;

		// Token: 0x04000164 RID: 356
		[HideInInspector]
		public ETFXEffectController etfxEffectController;

		// Token: 0x04000165 RID: 357
		[HideInInspector]
		public ETFXEffectControllerPooled etfxEffectControllerPooled;
	}
}
