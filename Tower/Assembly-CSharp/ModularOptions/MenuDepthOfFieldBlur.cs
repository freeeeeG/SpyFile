using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace ModularOptions
{
	// Token: 0x02000090 RID: 144
	[AddComponentMenu("Modular Options/Misc/Menu Depth Of Field Blur")]
	public sealed class MenuDepthOfFieldBlur : MonoBehaviour
	{
		// Token: 0x06000204 RID: 516 RVA: 0x00008987 File Offset: 0x00006B87
		private void Awake()
		{
			if (!this.postProcessingProfile.TryGet<DepthOfField>(out this.dof))
			{
				this.dof = this.postProcessingProfile.Add<DepthOfField>(true);
				this.dof.active = false;
			}
		}

		// Token: 0x06000205 RID: 517 RVA: 0x000089BC File Offset: 0x00006BBC
		private void OnEnable()
		{
			this.normalFocusDistance = this.dof.focusDistance.value;
			this.dof.focusDistance.value = this.focusDistance;
			this.dofActive = this.dof.active;
			this.dof.active = true;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x00008A12 File Offset: 0x00006C12
		private void OnDisable()
		{
			this.dof.active = this.dofActive;
			this.dof.focusDistance.value = this.normalFocusDistance;
		}

		// Token: 0x040001C9 RID: 457
		[Tooltip("Reference to global baseline profile.")]
		public VolumeProfile postProcessingProfile;

		// Token: 0x040001CA RID: 458
		[Range(0.01f, 9f)]
		public float focusDistance = 0.7f;

		// Token: 0x040001CB RID: 459
		private DepthOfField dof;

		// Token: 0x040001CC RID: 460
		private float normalFocusDistance;

		// Token: 0x040001CD RID: 461
		private bool dofActive;
	}
}
