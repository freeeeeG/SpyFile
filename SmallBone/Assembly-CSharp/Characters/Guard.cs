using System;
using System.Collections;
using Characters.Operations;
using FX;
using FX.SpriteEffects;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace Characters
{
	// Token: 0x02000745 RID: 1861
	public sealed class Guard : MonoBehaviour
	{
		// Token: 0x060025DE RID: 9694 RVA: 0x000723BC File Offset: 0x000705BC
		public static Guard Create(AssetReference reference)
		{
			return UnityEngine.Object.Instantiate<Guard>(Addressables.LoadAssetAsync<GameObject>(reference).WaitForCompletion().GetComponent<Guard>());
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060025DF RID: 9695 RVA: 0x000723E1 File Offset: 0x000705E1
		// (set) Token: 0x060025E0 RID: 9696 RVA: 0x000723EC File Offset: 0x000705EC
		public bool active
		{
			get
			{
				return this._active;
			}
			set
			{
				this._active = value;
				if (this._attachPosition && this._owner != null)
				{
					Guard.Poisition[] positionInfos = this._positionInfos;
					for (int i = 0; i < positionInfos.Length; i++)
					{
						positionInfos[i].TryAttach(this._owner, base.transform);
					}
				}
				if (this._spriteEffectStack != null && this._spriteEffectStack.mainRenderer != null && this._owner != null)
				{
					float d = this._spriteSize[this._owner.sizeForEffect];
					this._spriteEffectStack.mainRenderer.transform.localScale = Vector2.one * d;
				}
				base.gameObject.SetActive(this._active);
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060025E1 RID: 9697 RVA: 0x000724BB File Offset: 0x000706BB
		// (set) Token: 0x060025E2 RID: 9698 RVA: 0x000724C3 File Offset: 0x000706C3
		public float currentDurability
		{
			get
			{
				return this._currentDurability;
			}
			set
			{
				this._currentDurability = value;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060025E3 RID: 9699 RVA: 0x000724CC File Offset: 0x000706CC
		// (set) Token: 0x060025E4 RID: 9700 RVA: 0x000724D4 File Offset: 0x000706D4
		public float durability
		{
			get
			{
				return this._durability;
			}
			set
			{
				this._durability = value;
			}
		}

		// Token: 0x060025E5 RID: 9701 RVA: 0x000724DD File Offset: 0x000706DD
		public void Initialize(Character owner)
		{
			this._owner = owner;
			base.transform.SetParent(owner.attachWithFlip.transform);
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}

		// Token: 0x060025E6 RID: 9702 RVA: 0x0007251C File Offset: 0x0007071C
		public void GuardUp()
		{
			this._currentDurability = this._durability;
			this._owner.health.onTakeDamage.Add(int.MinValue, new TakeDamageDelegate(this.Block));
			this.active = true;
			if (this._lifeTime > 0f)
			{
				this._cexpireReference = this.StartCoroutineWithReference(this.CExpire());
			}
		}

		// Token: 0x060025E7 RID: 9703 RVA: 0x00072584 File Offset: 0x00070784
		public void GuardDown()
		{
			this._cexpireReference.Stop();
			if (this._owner != null)
			{
				this._owner.health.onTakeDamage.Remove(new TakeDamageDelegate(this.Block));
			}
			this.active = false;
		}

		// Token: 0x060025E8 RID: 9704 RVA: 0x000725D3 File Offset: 0x000707D3
		private void BreakGuard(Damage damage)
		{
			damage.stoppingPower = this._stoppingPowerOnBroken;
			this._onBreak.Run(this._owner);
			if (this._cancelActionOnBroken)
			{
				this._owner.CancelAction();
			}
			this.GuardDown();
		}

		// Token: 0x060025E9 RID: 9705 RVA: 0x0007260C File Offset: 0x0007080C
		private IEnumerator CExpire()
		{
			yield return this._owner.chronometer.master.WaitForSeconds(this._lifeTime);
			this.GuardDown();
			yield break;
		}

		// Token: 0x060025EA RID: 9706 RVA: 0x0007261C File Offset: 0x0007081C
		private bool Block(ref Damage damage)
		{
			Attacker attacker = damage.attacker;
			if (damage.attackType == Damage.AttackType.Additional)
			{
				return false;
			}
			Vector3 position = base.transform.position;
			if (!this.IsBlocked(position, damage))
			{
				return false;
			}
			this._currentDurability -= (float)damage.amount;
			if (this._breakable && this._currentDurability <= 0f)
			{
				this.BreakGuard(damage);
				return true;
			}
			damage.stoppingPower = 0f;
			if (damage.attackType == Damage.AttackType.Melee)
			{
				if (Time.time - this._lastHitTime < this._chronoEffectUniqueTime)
				{
					return true;
				}
				this._lastHitTime = Time.time;
				this._onHitToOwnerChronoInfo.ApplyGlobe();
				this._onHitToOwner.gameObject.SetActive(true);
				this._onHitToOwner.Run(this._owner);
				this._onHitToTarget.gameObject.SetActive(true);
				this._onHitToTarget.Run(attacker.character);
				if (this._spriteEffectStack != null)
				{
					this._spriteEffectStack.Add(new GuardHit(0, 0.2f));
				}
			}
			else if (damage.attackType == Damage.AttackType.Ranged || damage.attackType == Damage.AttackType.Projectile)
			{
				this._onHitToOwnerFromRangeAttack.gameObject.SetActive(true);
				this._onHitToOwnerFromRangeAttack.Run(this._owner);
			}
			return true;
		}

		// Token: 0x060025EB RID: 9707 RVA: 0x0007277C File Offset: 0x0007097C
		private bool IsBlocked(Vector2 center, Damage damage)
		{
			Vector3 vector = damage.attacker.transform.position;
			if (damage.attackType == Damage.AttackType.Projectile)
			{
				vector = damage.hitPoint;
			}
			return !this._frontOnly || (this._owner.lookingDirection == Character.LookingDirection.Right && center.x < vector.x) || (this._owner.lookingDirection == Character.LookingDirection.Left && center.x > vector.x);
		}

		// Token: 0x04002025 RID: 8229
		[SerializeField]
		private bool _frontOnly;

		// Token: 0x04002026 RID: 8230
		[SerializeField]
		private bool _cancelActionOnBroken;

		// Token: 0x04002027 RID: 8231
		[SerializeField]
		private bool _breakable;

		// Token: 0x04002028 RID: 8232
		[Tooltip("breakable일 때 체력")]
		[SerializeField]
		private float _durability;

		// Token: 0x04002029 RID: 8233
		[SerializeField]
		private float _lifeTime;

		// Token: 0x0400202A RID: 8234
		[SerializeField]
		private float _chronoEffectUniqueTime = 0.2f;

		// Token: 0x0400202B RID: 8235
		[SerializeField]
		private float _stoppingPowerOnBroken = 2f;

		// Token: 0x0400202C RID: 8236
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		[Space]
		private OperationInfos _onHitToOwner;

		// Token: 0x0400202D RID: 8237
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onHitToOwnerFromRangeAttack;

		// Token: 0x0400202E RID: 8238
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onHitToTarget;

		// Token: 0x0400202F RID: 8239
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onBreak;

		// Token: 0x04002030 RID: 8240
		[Space]
		[SerializeField]
		private ChronoInfo _onHitToOwnerChronoInfo;

		// Token: 0x04002031 RID: 8241
		[SerializeField]
		private ChronoInfo _onHitToTargetChronoInfo;

		// Token: 0x04002032 RID: 8242
		[SerializeField]
		private SpriteEffectStack _spriteEffectStack;

		// Token: 0x04002033 RID: 8243
		[SerializeField]
		private bool _attachPosition;

		// Token: 0x04002034 RID: 8244
		[SerializeField]
		private Guard.Poisition[] _positionInfos;

		// Token: 0x04002035 RID: 8245
		[SerializeField]
		private EnumArray<Character.SizeForEffect, float> _spriteSize;

		// Token: 0x04002036 RID: 8246
		private Character _owner;

		// Token: 0x04002037 RID: 8247
		private float _currentDurability;

		// Token: 0x04002038 RID: 8248
		private float _lastHitTime;

		// Token: 0x04002039 RID: 8249
		private CoroutineReference _cexpireReference;

		// Token: 0x0400203A RID: 8250
		private bool _active;

		// Token: 0x02000746 RID: 1862
		[Serializable]
		private class Poisition
		{
			// Token: 0x060025ED RID: 9709 RVA: 0x00072810 File Offset: 0x00070A10
			public void TryAttach(Character owner, Transform target)
			{
				if (this._size == owner.sizeForEffect)
				{
					this._positionInfo.Attach(owner, target);
				}
			}

			// Token: 0x0400203B RID: 8251
			[SerializeField]
			private Character.SizeForEffect _size;

			// Token: 0x0400203C RID: 8252
			[SerializeField]
			private PositionInfo _positionInfo;
		}
	}
}
