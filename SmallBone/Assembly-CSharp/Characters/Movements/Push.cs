using System;
using Characters.Projectiles;
using UnityEngine;

namespace Characters.Movements
{
	// Token: 0x0200080C RID: 2060
	public sealed class Push
	{
		// Token: 0x14000072 RID: 114
		// (add) Token: 0x06002A4A RID: 10826 RVA: 0x00082798 File Offset: 0x00080998
		// (remove) Token: 0x06002A4B RID: 10827 RVA: 0x000827D0 File Offset: 0x000809D0
		public event Push.OnSmashEndDelegate onEnd;

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06002A4C RID: 10828 RVA: 0x00082805 File Offset: 0x00080A05
		// (set) Token: 0x06002A4D RID: 10829 RVA: 0x0008280D File Offset: 0x00080A0D
		public bool ignoreOtherForce { get; private set; }

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06002A4E RID: 10830 RVA: 0x00082816 File Offset: 0x00080A16
		// (set) Token: 0x06002A4F RID: 10831 RVA: 0x0008281E File Offset: 0x00080A1E
		public bool expireOnGround { get; private set; }

		// Token: 0x17000899 RID: 2201
		// (get) Token: 0x06002A50 RID: 10832 RVA: 0x00082827 File Offset: 0x00080A27
		// (set) Token: 0x06002A51 RID: 10833 RVA: 0x0008282F File Offset: 0x00080A2F
		public bool smash { get; private set; }

		// Token: 0x1700089A RID: 2202
		// (get) Token: 0x06002A52 RID: 10834 RVA: 0x00082838 File Offset: 0x00080A38
		// (set) Token: 0x06002A53 RID: 10835 RVA: 0x00082840 File Offset: 0x00080A40
		public bool expired { get; private set; } = true;

		// Token: 0x1700089B RID: 2203
		// (get) Token: 0x06002A54 RID: 10836 RVA: 0x00082849 File Offset: 0x00080A49
		// (set) Token: 0x06002A55 RID: 10837 RVA: 0x00082851 File Offset: 0x00080A51
		public Vector2 direction { get; private set; }

		// Token: 0x1700089C RID: 2204
		// (get) Token: 0x06002A56 RID: 10838 RVA: 0x0008285A File Offset: 0x00080A5A
		// (set) Token: 0x06002A57 RID: 10839 RVA: 0x00082862 File Offset: 0x00080A62
		public Vector2 totalForce { get; private set; }

		// Token: 0x1700089D RID: 2205
		// (get) Token: 0x06002A58 RID: 10840 RVA: 0x0008286B File Offset: 0x00080A6B
		// (set) Token: 0x06002A59 RID: 10841 RVA: 0x00082873 File Offset: 0x00080A73
		public float duration { get; private set; }

		// Token: 0x06002A5A RID: 10842 RVA: 0x0008287C File Offset: 0x00080A7C
		internal Push(Character owner)
		{
			this._owner = owner;
		}

		// Token: 0x06002A5B RID: 10843 RVA: 0x00082894 File Offset: 0x00080A94
		private void Intialize()
		{
			this._time = 0f;
			this._amountBefore = new ValueTuple<float, float>(0f, 0f);
			this.direction = (this._force.Item1 + this._force.Item2).normalized;
			this.totalForce = this._force.Item1 * this._curve.Item1.valueMultiplier + this._force.Item2 * this._curve.Item2.valueMultiplier;
			this.expired = false;
			this.duration = Mathf.Max(this._curve.Item1.duration, this._curve.Item2.duration);
		}

		// Token: 0x06002A5C RID: 10844 RVA: 0x00082968 File Offset: 0x00080B68
		public bool ApplySmash(Character from, Vector2 force, Curve curve, bool ignoreOtherForce = false, bool expireOnGround = false, Push.OnSmashEndDelegate onEnd = null)
		{
			if (this.smash && !this.expired)
			{
				return false;
			}
			this._from = from;
			this._force = new ValueTuple<Vector2, Vector2>(force, Vector2.zero);
			this._curve = new ValueTuple<Curve, Curve>(curve, Curve.empty);
			this.ignoreOtherForce = ignoreOtherForce;
			this.expireOnGround = expireOnGround;
			this.onEnd = onEnd;
			this.smash = true;
			this.Intialize();
			return true;
		}

		// Token: 0x06002A5D RID: 10845 RVA: 0x000829D8 File Offset: 0x00080BD8
		public void ApplySmash(Character from, PushInfo info, Push.OnSmashEndDelegate onEnd = null)
		{
			this._from = from;
			this._force = info.Evaluate(from, new TargetStruct(this._owner));
			this._curve = new ValueTuple<Curve, Curve>(info.curve1, info.curve2);
			this.ignoreOtherForce = info.ignoreOtherForce;
			this.expireOnGround = info.expireOnGround;
			this.onEnd = onEnd;
			this.smash = true;
			this.Intialize();
		}

		// Token: 0x06002A5E RID: 10846 RVA: 0x00082A4C File Offset: 0x00080C4C
		public void ApplySmash(Character from, Transform forceFrom, PushInfo info, Push.OnSmashEndDelegate onEnd = null)
		{
			this._from = from;
			this._force = info.Evaluate(forceFrom, new TargetStruct(this._owner));
			this._curve = new ValueTuple<Curve, Curve>(info.curve1, info.curve2);
			this.ignoreOtherForce = info.ignoreOtherForce;
			this.expireOnGround = info.expireOnGround;
			this.onEnd = onEnd;
			this.smash = true;
			this.Intialize();
		}

		// Token: 0x06002A5F RID: 10847 RVA: 0x00082AC4 File Offset: 0x00080CC4
		public bool ApplyKnockback(Character from, Vector2 force, Curve curve, bool ignoreOtherForce = false, bool expireOnGround = false)
		{
			if (this.smash && !this.expired)
			{
				return false;
			}
			this._from = from;
			this._force = new ValueTuple<Vector2, Vector2>(force, Vector2.zero);
			this._curve = new ValueTuple<Curve, Curve>(curve, Curve.empty);
			this.ignoreOtherForce = ignoreOtherForce;
			this.expireOnGround = expireOnGround;
			this.smash = false;
			this.Intialize();
			return true;
		}

		// Token: 0x06002A60 RID: 10848 RVA: 0x00082B2C File Offset: 0x00080D2C
		public bool ApplyKnockback(Transform from, PushInfo info)
		{
			if (this.smash && !this.expired)
			{
				return false;
			}
			this._from = null;
			this._force = info.Evaluate(from, new TargetStruct(this._owner));
			this._curve = new ValueTuple<Curve, Curve>(info.curve1, info.curve2);
			this.ignoreOtherForce = info.ignoreOtherForce;
			this.expireOnGround = info.expireOnGround;
			this.smash = false;
			this.Intialize();
			return true;
		}

		// Token: 0x06002A61 RID: 10849 RVA: 0x00082BAC File Offset: 0x00080DAC
		public bool ApplyKnockback(IProjectile from, PushInfo info)
		{
			if (this.smash && !this.expired)
			{
				return false;
			}
			this._from = from.owner;
			this._force = info.Evaluate(from, new TargetStruct(this._owner));
			this._curve = new ValueTuple<Curve, Curve>(info.curve1, info.curve2);
			this.ignoreOtherForce = info.ignoreOtherForce;
			this.expireOnGround = info.expireOnGround;
			this.smash = false;
			this.Intialize();
			return true;
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x00082C34 File Offset: 0x00080E34
		public bool ApplyKnockback(Character from, PushInfo info)
		{
			if (this.smash && !this.expired)
			{
				return false;
			}
			this._from = from;
			this._force = info.Evaluate(from, new TargetStruct(this._owner));
			this._curve = new ValueTuple<Curve, Curve>(info.curve1, info.curve2);
			this.ignoreOtherForce = info.ignoreOtherForce;
			this.expireOnGround = info.expireOnGround;
			this.smash = false;
			this.Intialize();
			return true;
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x00082CB4 File Offset: 0x00080EB4
		private bool UpdateForce(ref Vector2 forceVector, Vector2 force, Curve curve, ref float amountBefore)
		{
			float num = curve.Evaluate(this._time);
			forceVector += force * (num - amountBefore);
			amountBefore = num;
			return this._time > curve.duration || curve.duration == 0f;
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x00082D0C File Offset: 0x00080F0C
		internal void Update(out Vector2 vector, float deltaTime)
		{
			vector = Vector2.zero;
			if (this.duration == 0f)
			{
				vector = this._force.Item1 + this._force.Item2;
				this.expired = true;
				Push.OnSmashEndDelegate onSmashEndDelegate = this.onEnd;
				if (onSmashEndDelegate == null)
				{
					return;
				}
				onSmashEndDelegate(this, this._from, this._owner, Push.SmashEndType.Expire, null, Movement.CollisionDirection.None);
				return;
			}
			else
			{
				bool flag = this.UpdateForce(ref vector, this._force.Item1, this._curve.Item1, ref this._amountBefore.Item1);
				bool flag2 = this.UpdateForce(ref vector, this._force.Item2, this._curve.Item2, ref this._amountBefore.Item2);
				this._time += deltaTime;
				if (!flag || !flag2)
				{
					return;
				}
				this.expired = true;
				Push.OnSmashEndDelegate onSmashEndDelegate2 = this.onEnd;
				if (onSmashEndDelegate2 == null)
				{
					return;
				}
				onSmashEndDelegate2(this, this._from, this._owner, Push.SmashEndType.Expire, null, Movement.CollisionDirection.None);
				return;
			}
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x00082E16 File Offset: 0x00081016
		internal void CollideWith(RaycastHit2D raycastHit, Movement.CollisionDirection direction)
		{
			if (this.expired)
			{
				return;
			}
			this.expired = true;
			Push.OnSmashEndDelegate onSmashEndDelegate = this.onEnd;
			if (onSmashEndDelegate == null)
			{
				return;
			}
			onSmashEndDelegate(this, this._from, this._owner, Push.SmashEndType.Collide, new RaycastHit2D?(raycastHit), direction);
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x00082E4D File Offset: 0x0008104D
		internal void Expire()
		{
			this.expired = true;
		}

		// Token: 0x04002419 RID: 9241
		private readonly Character _owner;

		// Token: 0x0400241A RID: 9242
		private Character _from;

		// Token: 0x0400241B RID: 9243
		private ValueTuple<Vector2, Vector2> _force;

		// Token: 0x0400241C RID: 9244
		private ValueTuple<Curve, Curve> _curve;

		// Token: 0x0400241D RID: 9245
		private float _time;

		// Token: 0x0400241E RID: 9246
		private ValueTuple<float, float> _amountBefore;

		// Token: 0x0200080D RID: 2061
		// (Invoke) Token: 0x06002A68 RID: 10856
		public delegate void OnSmashEndDelegate(Push push, Character from, Character to, Push.SmashEndType endType, RaycastHit2D? raycastHit, Movement.CollisionDirection direction);

		// Token: 0x0200080E RID: 2062
		public enum SmashEndType
		{
			// Token: 0x04002427 RID: 9255
			Expire,
			// Token: 0x04002428 RID: 9256
			Collide,
			// Token: 0x04002429 RID: 9257
			Cancel
		}
	}
}
