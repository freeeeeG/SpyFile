using System;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000D8 RID: 216
	public class BurnOnWalk : MonoBehaviour
	{
		// Token: 0x06000691 RID: 1681 RVA: 0x0001DAA5 File Offset: 0x0001BCA5
		private void Start()
		{
			this.BS = BurnSystem.SharedInstance;
			this._lastPos = base.transform.position;
			this._currPos = base.transform.position;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x0001DAE0 File Offset: 0x0001BCE0
		private void Update()
		{
			this._lastPos = this._currPos;
			this._currPos = base.transform.position;
			Vector2 vector = this._lastPos - this._currPos;
			this._distanceCtr += vector.magnitude;
			if (this._distanceCtr >= this.distanceToCast)
			{
				this._distanceCtr -= this.distanceToCast;
				this.particles.Play();
				this.soundFX.Play(null);
				Collider2D[] array = Physics2D.OverlapCircleAll(base.transform.position, this.range);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].gameObject.tag == "Enemy")
					{
						this.BS.Burn(array[i].gameObject, this.burnDamage);
						FlashSprite componentInChildren = array[i].gameObject.GetComponentInChildren<FlashSprite>();
						if (componentInChildren != null)
						{
							componentInChildren.Flash();
						}
					}
				}
			}
		}

		// Token: 0x0400045C RID: 1116
		[SerializeField]
		private ParticleSystem particles;

		// Token: 0x0400045D RID: 1117
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x0400045E RID: 1118
		[SerializeField]
		private float range;

		// Token: 0x0400045F RID: 1119
		[SerializeField]
		private float distanceToCast;

		// Token: 0x04000460 RID: 1120
		[SerializeField]
		private int burnDamage;

		// Token: 0x04000461 RID: 1121
		private BurnSystem BS;

		// Token: 0x04000462 RID: 1122
		private float _distanceCtr;

		// Token: 0x04000463 RID: 1123
		private Vector2 _lastPos;

		// Token: 0x04000464 RID: 1124
		private Vector2 _currPos;
	}
}
