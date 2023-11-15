using System;
using System.Collections;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs.DarkHero
{
	// Token: 0x0200101F RID: 4127
	public sealed class BrutalCharge : CharacterOperation
	{
		// Token: 0x06004F99 RID: 20377 RVA: 0x000EF96D File Offset: 0x000EDB6D
		public override void Run(Character owner)
		{
			this._owner = owner;
			base.StartCoroutine(this.CAttack(owner));
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x000EF984 File Offset: 0x000EDB84
		private IEnumerator CAttack(Character owner)
		{
			int count = (int)this._count.value;
			if (count >= this._points.Length)
			{
				count = this._points.Length;
			}
			Collider2D platform = owner.movement.controller.collisionState.lastStandingCollider;
			if (platform == null)
			{
				owner.movement.TryGetClosestBelowCollider(out platform, 262144, 100f);
			}
			this._lowCount = 2;
			this._lookRight = (owner.lookingDirection == Character.LookingDirection.Right);
			this._points[0].position = owner.transform.position;
			float wallToWallCount;
			if (owner.movement.isGrounded)
			{
				if (count % 2 == 1)
				{
					wallToWallCount = 3f;
				}
				else
				{
					wallToWallCount = 2f;
				}
				this._last = BrutalCharge.Point.Ground;
			}
			else if (owner.transform.position.x <= platform.bounds.center.x)
			{
				if (count % 2 == 0)
				{
					wallToWallCount = 3f;
				}
				else
				{
					wallToWallCount = 2f;
				}
				this._last = BrutalCharge.Point.LeftWall;
			}
			else
			{
				if (count % 2 == 0)
				{
					wallToWallCount = 3f;
				}
				else
				{
					wallToWallCount = 2f;
				}
				this._last = BrutalCharge.Point.RightWall;
			}
			Vector2 beforePoint = this._points[0].position;
			int num3;
			for (int i = 1; i < count; i = num3 + 1)
			{
				float x = 0f;
				float y = 0f;
				switch (this._last)
				{
				case BrutalCharge.Point.Ground:
					if (this._lookRight)
					{
						this.MoveToRightWall(platform, out x, out y);
					}
					else
					{
						this.MoveToLeftWall(platform, out x, out y);
					}
					break;
				case BrutalCharge.Point.LeftWall:
					this._lookRight = true;
					if (wallToWallCount > 0f)
					{
						float num = wallToWallCount;
						wallToWallCount = num - 1f;
						this.MoveToRightWall(platform, out x, out y);
					}
					else
					{
						this.MoveToGround(platform, out x, out y);
					}
					break;
				case BrutalCharge.Point.RightWall:
					this._lookRight = false;
					if (wallToWallCount > 0f)
					{
						float num = wallToWallCount;
						wallToWallCount = num - 1f;
						this.MoveToLeftWall(platform, out x, out y);
					}
					else
					{
						this.MoveToGround(platform, out x, out y);
					}
					break;
				}
				Vector2 vector = new Vector2(x, y);
				this._points[i].position = vector;
				Vector2 vector2 = vector - beforePoint;
				float num2 = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
				if (num2 < 0f)
				{
					num2 += 360f;
				}
				this._points[i - 1].rotation = Quaternion.Euler(0f, 0f, num2);
				this._sign.Spawn(this._points[i - 1].position, owner, num2, 1f);
				beforePoint = vector;
				yield return owner.chronometer.master.WaitForSeconds(0.1f);
				num3 = i;
			}
			yield return owner.chronometer.master.WaitForSeconds(0.5f);
			for (int i = 0; i < count - 1; i = num3 + 1)
			{
				float num4 = Vector2.Distance(MMMaths.Vector3ToVector2(this._points[i].position), MMMaths.Vector3ToVector2(this._points[i + 1].position));
				this._attack.scaleX = new CustomFloat(num4 / 23f);
				this._attack.Spawn(this._points[i].position, owner, this._points[i].rotation.eulerAngles.z, 1f);
				this._smoke.Spawn(this._points[i].position, owner, this._points[i].rotation.eulerAngles.z, 1f);
				this._flame.Spawn(this._points[i].position, owner, this._points[i].rotation.eulerAngles.z, 1f);
				yield return owner.chronometer.master.WaitForSeconds(0.1f);
				num3 = i;
			}
			yield break;
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x000EF99C File Offset: 0x000EDB9C
		private void MoveToGround(Collider2D platform, out float x, out float y)
		{
			if (this._lookRight)
			{
				x = UnityEngine.Random.Range(platform.bounds.center.x + 2.5f, platform.bounds.max.x - 1f);
			}
			else
			{
				x = UnityEngine.Random.Range(platform.bounds.min.x + 1f, platform.bounds.center.x - 2.5f);
			}
			y = platform.bounds.max.y;
			this._last = BrutalCharge.Point.Ground;
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x000EFA44 File Offset: 0x000EDC44
		private void MoveToLeftWall(Collider2D platform, out float x, out float y)
		{
			x = platform.bounds.min.x + 1.35f;
			if (this._lowCount > 0)
			{
				y = Singleton<Service>.Instance.levelManager.player.collider.bounds.center.y;
				this._lowCount--;
			}
			else
			{
				y = platform.bounds.max.y + this._heightRange.value;
			}
			this._last = BrutalCharge.Point.LeftWall;
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x000EFAD8 File Offset: 0x000EDCD8
		private void MoveToRightWall(Collider2D platform, out float x, out float y)
		{
			x = platform.bounds.max.x - 1.35f;
			if (this._lowCount > 0)
			{
				y = Singleton<Service>.Instance.levelManager.player.collider.bounds.center.y;
				this._lowCount--;
			}
			else
			{
				y = platform.bounds.max.y + this._heightRange.value;
			}
			this._last = BrutalCharge.Point.RightWall;
		}

		// Token: 0x04003FDC RID: 16348
		[SerializeField]
		private CustomFloat _heightRange;

		// Token: 0x04003FDD RID: 16349
		[SerializeField]
		private CustomFloat _count;

		// Token: 0x04003FDE RID: 16350
		[SerializeField]
		private Transform[] _points;

		// Token: 0x04003FDF RID: 16351
		[SerializeField]
		private EffectInfo _sign;

		// Token: 0x04003FE0 RID: 16352
		[SerializeField]
		private EffectInfo _smoke;

		// Token: 0x04003FE1 RID: 16353
		[SerializeField]
		private EffectInfo _attack;

		// Token: 0x04003FE2 RID: 16354
		[SerializeField]
		private EffectInfo _flame;

		// Token: 0x04003FE3 RID: 16355
		private int _lowCount;

		// Token: 0x04003FE4 RID: 16356
		private bool _lookRight;

		// Token: 0x04003FE5 RID: 16357
		private BrutalCharge.Point _last;

		// Token: 0x04003FE6 RID: 16358
		private Character _owner;

		// Token: 0x04003FE7 RID: 16359
		private const float _effectXLength = 23f;

		// Token: 0x04003FE8 RID: 16360
		private const float _extents = 1.35f;

		// Token: 0x02001020 RID: 4128
		private enum Point
		{
			// Token: 0x04003FEA RID: 16362
			Ground,
			// Token: 0x04003FEB RID: 16363
			LeftWall,
			// Token: 0x04003FEC RID: 16364
			RightWall
		}
	}
}
