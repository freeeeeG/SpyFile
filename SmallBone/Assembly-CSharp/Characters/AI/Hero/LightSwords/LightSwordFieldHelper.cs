using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.Hero.LightSwords
{
	// Token: 0x0200127E RID: 4734
	public class LightSwordFieldHelper : MonoBehaviour
	{
		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x06005DE1 RID: 24033 RVA: 0x001141F8 File Offset: 0x001123F8
		public List<LightSword> swords
		{
			get
			{
				return this._swords;
			}
		}

		// Token: 0x06005DE2 RID: 24034 RVA: 0x00114200 File Offset: 0x00112400
		public void Fire()
		{
			base.StartCoroutine(this.CFire());
		}

		// Token: 0x06005DE3 RID: 24035 RVA: 0x0011420F File Offset: 0x0011240F
		public IEnumerator CFire()
		{
			if (this._intervals == null)
			{
				this.MakeInterval();
			}
			this._swords = this._pool.Get();
			this._platform = this._owner.movement.controller.collisionState.lastStandingCollider.bounds;
			int count = this._swords.Count - 1;
			int intervalIndex = 0;
			int num;
			do
			{
				Vector2 destination = new Vector2(UnityEngine.Random.Range(this._intervals[intervalIndex].left, this._intervals[intervalIndex].right), this._platform.max.y);
				float degree = UnityEngine.Random.Range(this._fireRange.x, this._fireRange.y);
				Vector2 source = this.CalculateFirePosition(destination, degree);
				this._swords[count].Fire(this._owner, source, destination);
				intervalIndex = (intervalIndex + 1) % this._intervalCount;
				yield return Chronometer.global.WaitForSeconds(0.1f);
				num = count - 1;
				count = num;
			}
			while (num >= 0);
			yield break;
		}

		// Token: 0x06005DE4 RID: 24036 RVA: 0x0011421E File Offset: 0x0011241E
		public void Sign(Character owner)
		{
			this._swords.ForEach(delegate(LightSword sword)
			{
				if (sword.active)
				{
					sword.Sign();
				}
			});
			base.StartCoroutine(this.CDraw(owner));
		}

		// Token: 0x06005DE5 RID: 24037 RVA: 0x00114258 File Offset: 0x00112458
		private IEnumerator CDraw(Character owner)
		{
			yield return Chronometer.global.WaitForSeconds(0.5f);
			this.Draw(owner);
			yield break;
		}

		// Token: 0x06005DE6 RID: 24038 RVA: 0x00114270 File Offset: 0x00112470
		public void Draw(Character owner)
		{
			this._swords.ForEach(delegate(LightSword sword)
			{
				if (sword.active)
				{
					sword.Draw(owner);
				}
			});
		}

		// Token: 0x06005DE7 RID: 24039 RVA: 0x001142A1 File Offset: 0x001124A1
		public int GetActivatedSwordCount()
		{
			return this._swords.Count((LightSword sword) => sword.active);
		}

		// Token: 0x06005DE8 RID: 24040 RVA: 0x001142D0 File Offset: 0x001124D0
		public LightSword GetClosestFromPlayer()
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			float num = float.PositiveInfinity;
			LightSword lightSword = null;
			if (this._swords == null)
			{
				return null;
			}
			foreach (LightSword lightSword2 in this._swords)
			{
				if (lightSword2.active)
				{
					if (lightSword == null)
					{
						lightSword = lightSword2;
						num = Mathf.Abs(player.transform.position.x - lightSword2.GetStuckPosition().x);
					}
					else
					{
						Vector3 stuckPosition = lightSword2.GetStuckPosition();
						float num2 = Mathf.Abs(player.transform.position.x - stuckPosition.x);
						if (num2 < num)
						{
							lightSword = lightSword2;
							num = num2;
						}
					}
				}
			}
			return lightSword;
		}

		// Token: 0x06005DE9 RID: 24041 RVA: 0x001143B0 File Offset: 0x001125B0
		public LightSword GetBehindPlayer()
		{
			float x = Singleton<Service>.Instance.levelManager.player.transform.position.x;
			float x2 = this._owner.transform.position.x;
			LightSword lightSword = null;
			foreach (LightSword lightSword2 in this._swords)
			{
				if (lightSword2.active)
				{
					float x3 = lightSword2.GetStuckPosition().x;
					float num = x3 - x;
					float num2 = x3 - x2;
					if ((x < x2 || (num2 >= 0f && num >= 0f)) && (x > x2 || (num2 <= 0f && num <= 0f)))
					{
						if (lightSword == null)
						{
							lightSword = lightSword2;
						}
						else
						{
							float num3 = Mathf.Abs(lightSword.GetStuckPosition().x - x);
							if (Mathf.Abs(num) < num3)
							{
								lightSword = lightSword2;
							}
						}
					}
				}
			}
			return lightSword;
		}

		// Token: 0x06005DEA RID: 24042 RVA: 0x001144B8 File Offset: 0x001126B8
		public LightSword GetFarthestFromHero()
		{
			float x = this._owner.transform.position.x;
			float num = float.NegativeInfinity;
			LightSword lightSword = null;
			foreach (LightSword lightSword2 in this._swords)
			{
				if (lightSword2.active)
				{
					if (lightSword == null)
					{
						lightSword = lightSword2;
					}
					else
					{
						float num2 = Mathf.Abs(lightSword2.GetStuckPosition().x - x);
						if (num2 > num)
						{
							lightSword = lightSword2;
							num = num2;
						}
					}
				}
			}
			return lightSword;
		}

		// Token: 0x06005DEB RID: 24043 RVA: 0x0011455C File Offset: 0x0011275C
		private void MakeInterval()
		{
			this._intervals = new List<LightSwordFieldHelper.Interval>();
			Bounds bounds = this._owner.movement.controller.collisionState.lastStandingCollider.bounds;
			float num = 1f;
			float num2 = (bounds.size.x - num) / (float)this._intervalCount;
			float num3 = bounds.min.x + num;
			float num4 = num3 + num2;
			for (int i = 0; i < this._intervalCount; i++)
			{
				this._intervals.Add(new LightSwordFieldHelper.Interval(num3, num4));
				num3 = num4;
				num4 = num3 + num2;
			}
			this._intervals.Shuffle<LightSwordFieldHelper.Interval>();
		}

		// Token: 0x06005DEC RID: 24044 RVA: 0x00114600 File Offset: 0x00112800
		private Vector2 CalculateFirePosition(Vector2 destination, float degree)
		{
			Vector2 vector = Vector2.right * this._fireDistance;
			float f = degree * 0.017453292f;
			float x = vector.x * Mathf.Cos(f) - vector.y * Mathf.Sin(f);
			float y = vector.x * Mathf.Sin(f) + vector.y * Mathf.Cos(f);
			return new Vector2(x, y) + destination;
		}

		// Token: 0x04004B6D RID: 19309
		[SerializeField]
		private Character _owner;

		// Token: 0x04004B6E RID: 19310
		[SerializeField]
		[MinMaxSlider(0f, 180f)]
		private Vector2 _fireRange;

		// Token: 0x04004B6F RID: 19311
		[SerializeField]
		private float _fireDistance;

		// Token: 0x04004B70 RID: 19312
		[SerializeField]
		private int _intervalCount;

		// Token: 0x04004B71 RID: 19313
		[SerializeField]
		private LightSwordPool _pool;

		// Token: 0x04004B72 RID: 19314
		private List<LightSword> _swords;

		// Token: 0x04004B73 RID: 19315
		private List<LightSwordFieldHelper.Interval> _intervals;

		// Token: 0x04004B74 RID: 19316
		private Bounds _platform;

		// Token: 0x0200127F RID: 4735
		private class Interval
		{
			// Token: 0x06005DEE RID: 24046 RVA: 0x00114669 File Offset: 0x00112869
			internal Interval(float left, float right)
			{
				this.left = left;
				this.right = right;
			}

			// Token: 0x04004B75 RID: 19317
			internal float left;

			// Token: 0x04004B76 RID: 19318
			internal float right;
		}
	}
}
