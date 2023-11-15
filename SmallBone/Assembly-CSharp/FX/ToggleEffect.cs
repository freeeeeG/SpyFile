using System;
using Characters;
using UnityEngine;

namespace FX
{
	// Token: 0x02000266 RID: 614
	public class ToggleEffect : MonoBehaviour
	{
		// Token: 0x06000C05 RID: 3077 RVA: 0x000211E4 File Offset: 0x0001F3E4
		private void Start()
		{
			Character componentInParent = base.GetComponentInParent<Character>();
			if (componentInParent != null)
			{
				switch (this._chronometerType)
				{
				case ToggleEffect.Type.effect:
					this._chronometer = componentInParent.chronometer.effect;
					return;
				case ToggleEffect.Type.master:
					this._chronometer = componentInParent.chronometer.master;
					return;
				case ToggleEffect.Type.projectile:
					this._chronometer = componentInParent.chronometer.projectile;
					return;
				case ToggleEffect.Type.animation:
					this._chronometer = componentInParent.chronometer.animation;
					return;
				default:
					this._chronometer = componentInParent.chronometer.effect;
					break;
				}
			}
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x00021279 File Offset: 0x0001F479
		private void Update()
		{
			this._animator.speed = ((this._chronometer == null) ? Chronometer.global.timeScale : this._chronometer.timeScale) / Time.timeScale;
		}

		// Token: 0x04000A07 RID: 2567
		[SerializeField]
		[GetComponent]
		private Animator _animator;

		// Token: 0x04000A08 RID: 2568
		[SerializeField]
		private ToggleEffect.Type _chronometerType;

		// Token: 0x04000A09 RID: 2569
		private Chronometer _chronometer;

		// Token: 0x02000267 RID: 615
		private enum Type
		{
			// Token: 0x04000A0B RID: 2571
			effect,
			// Token: 0x04000A0C RID: 2572
			master,
			// Token: 0x04000A0D RID: 2573
			projectile,
			// Token: 0x04000A0E RID: 2574
			animation
		}
	}
}
