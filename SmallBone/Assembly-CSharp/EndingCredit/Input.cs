using System;
using UnityEngine;
using UserInput;

namespace EndingCredit
{
	// Token: 0x0200019A RID: 410
	public class Input : MonoBehaviour
	{
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060008DD RID: 2269 RVA: 0x000193E3 File Offset: 0x000175E3
		public float speed
		{
			get
			{
				return this._speed;
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x000193EB File Offset: 0x000175EB
		private void Start()
		{
			this._startSpeed = this._speed;
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x000193FC File Offset: 0x000175FC
		private void Update()
		{
			if (KeyMapper.Map.Attack.IsPressed || KeyMapper.Map.Jump.IsPressed || KeyMapper.Map.Submit.IsPressed || KeyMapper.Map.Down.IsPressed)
			{
				this._speed = this._startSpeed * this._accelerationValue;
			}
			if (KeyMapper.Map.Attack.WasReleased || KeyMapper.Map.Jump.WasReleased || KeyMapper.Map.Submit.WasReleased || KeyMapper.Map.Down.WasReleased)
			{
				this._speed = this._startSpeed;
			}
		}

		// Token: 0x04000712 RID: 1810
		[SerializeField]
		private float _speed;

		// Token: 0x04000713 RID: 1811
		[SerializeField]
		private float _accelerationValue;

		// Token: 0x04000714 RID: 1812
		private float _startSpeed;
	}
}
