using System;
using System.Collections.Generic;
using UnityEngine;

namespace CameraShake
{
	// Token: 0x02000055 RID: 85
	public class CameraShaker : MonoBehaviour
	{
		// Token: 0x06000424 RID: 1060 RVA: 0x00015F58 File Offset: 0x00014158
		public static void Shake(ICameraShake shake)
		{
			if (CameraShaker.IsInstanceNull())
			{
				return;
			}
			CameraShaker.Instance.RegisterShake(shake);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x00015F6D File Offset: 0x0001416D
		public void RegisterShake(ICameraShake shake)
		{
			shake.Initialize(this.cameraTransform.position, this.cameraTransform.rotation);
			this.activeShakes.Add(shake);
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x00015F97 File Offset: 0x00014197
		public void SetCameraTransform(Transform cameraTransform)
		{
			cameraTransform.localPosition = Vector3.zero;
			cameraTransform.localEulerAngles = Vector3.zero;
			this.cameraTransform = cameraTransform;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00015FB6 File Offset: 0x000141B6
		private void Awake()
		{
			CameraShaker.Instance = this;
			this.ShakePresets = new CameraShakePresets(this);
			CameraShaker.Presets = this.ShakePresets;
			if (this.cameraTransform == null)
			{
				this.cameraTransform = base.transform;
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x00015FF0 File Offset: 0x000141F0
		private void Update()
		{
			if (this.cameraTransform == null || !CameraShaker.ShakeOn)
			{
				return;
			}
			Displacement displacement = Displacement.Zero;
			for (int i = this.activeShakes.Count - 1; i >= 0; i--)
			{
				if (this.activeShakes[i].IsFinished)
				{
					this.activeShakes.RemoveAt(i);
				}
				else
				{
					this.activeShakes[i].Update(Time.deltaTime, this.cameraTransform.position, this.cameraTransform.rotation);
					displacement += this.activeShakes[i].CurrentDisplacement;
				}
			}
			this.cameraTransform.localPosition = this.StrengthMultiplier * displacement.position;
			this.cameraTransform.localRotation = Quaternion.Euler(this.StrengthMultiplier * displacement.eulerAngles);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x000160D3 File Offset: 0x000142D3
		private static bool IsInstanceNull()
		{
			if (CameraShaker.Instance == null)
			{
				Debug.LogError("CameraShaker Instance is missing!");
				return true;
			}
			return false;
		}

		// Token: 0x04000227 RID: 551
		public static CameraShaker Instance;

		// Token: 0x04000228 RID: 552
		public static CameraShakePresets Presets;

		// Token: 0x04000229 RID: 553
		public static bool ShakeOn;

		// Token: 0x0400022A RID: 554
		private readonly List<ICameraShake> activeShakes = new List<ICameraShake>();

		// Token: 0x0400022B RID: 555
		[Tooltip("Transform which will be affected by the shakes.\n\nCameraShaker will set this transform's local position and rotation.")]
		[SerializeField]
		private Transform cameraTransform;

		// Token: 0x0400022C RID: 556
		[Tooltip("Scales the strength of all shakes.")]
		[Range(0f, 1f)]
		[SerializeField]
		public float StrengthMultiplier = 1f;

		// Token: 0x0400022D RID: 557
		public CameraShakePresets ShakePresets;
	}
}
