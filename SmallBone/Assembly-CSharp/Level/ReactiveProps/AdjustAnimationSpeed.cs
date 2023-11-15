using System;
using UnityEngine;

namespace Level.ReactiveProps
{
	// Token: 0x02000568 RID: 1384
	public class AdjustAnimationSpeed : MonoBehaviour
	{
		// Token: 0x06001B37 RID: 6967 RVA: 0x00054968 File Offset: 0x00052B68
		private void Start()
		{
			for (int i = 0; i < this._animators.Length; i++)
			{
				this._animators[i].speed = UnityEngine.Random.Range(this._animationSpeedRange.x, this._animationSpeedRange.y);
			}
		}

		// Token: 0x04001764 RID: 5988
		[SerializeField]
		[MinMaxSlider(0f, 5f)]
		private Vector2 _animationSpeedRange;

		// Token: 0x04001765 RID: 5989
		[SerializeField]
		private Animator[] _animators;
	}
}
