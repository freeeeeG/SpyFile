using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.Abilities.Constraints;
using Characters.Operations;
using Characters.Operations.Fx;
using FX;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions.FairyTaleSummon
{
	// Token: 0x020008CB RID: 2251
	public class Oberon : MonoBehaviour
	{
		// Token: 0x06002FE7 RID: 12263 RVA: 0x0008F9C0 File Offset: 0x0008DBC0
		private void Awake()
		{
			this._attackDetectRange.enabled = false;
			this._thunderDetectRange.enabled = false;
			this._bombDetectRange.enabled = false;
			this._groundFinder = new RayCaster
			{
				direction = Vector2.down,
				distance = 5f
			};
			this._groundFinder.contactFilter.SetLayerMask(Layers.groundMask);
			this._remainAttackCooldown = this._attackCooldown;
			this._remainThunderCooldown = this._thunderCooldown;
			this._remainBombCooldown = this._bombCooldown;
			this._bombScreenFlash.Initialize();
			Dictionary<string, AnimationClip> dictionary = this._animator.runtimeAnimatorController.animationClips.ToDictionary((AnimationClip clip) => clip.name);
			this._idleLength = dictionary["Idle"].length;
			this._introLength = dictionary["Intro"].length;
			this._bombEndLength = dictionary["SpiritBomb_End"].length;
			this._bombLoopLength = dictionary["SpiritBomb_Loop"].length;
			this._bombReadyLength = dictionary["SpiritBomb_Ready"].length;
			this._thunderLength = dictionary["SpiritThunder"].length;
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x0008FB10 File Offset: 0x0008DD10
		public void Initialize(Character owner, Transform slot)
		{
			this._owner = owner;
			this._slot = slot;
			this.ResetPosition();
			base.StartCoroutine(this.CCooldown());
			base.StartCoroutine(this.CRun());
			Singleton<Service>.Instance.levelManager.onMapLoaded += this.ResetPosition;
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x0008FB66 File Offset: 0x0008DD66
		private void OnDestroy()
		{
			Singleton<Service>.Instance.levelManager.onMapLoaded -= this.ResetPosition;
		}

		// Token: 0x06002FEA RID: 12266 RVA: 0x0008FB84 File Offset: 0x0008DD84
		private void ResetPosition()
		{
			if (this._slot == null)
			{
				return;
			}
			base.transform.position = (this._position = this._slot.transform.position);
		}

		// Token: 0x06002FEB RID: 12267 RVA: 0x0008FBC4 File Offset: 0x0008DDC4
		private void Move(float deltaTime)
		{
			if (this._slot == null)
			{
				return;
			}
			this._position = Vector3.Lerp(this._position, this._slot.transform.position, deltaTime * this._trackSpeed);
			this._floatingTime += deltaTime;
			Vector3 position = this._position;
			position.y += Mathf.Sin(this._floatingTime * 3.1415927f * this._floatFrequency) * this._floatAmplitude;
			base.transform.position = position;
			this._spriteRenderer.flipX = (this._slot.transform.position.x - this._position.x < 0f);
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x0008FC87 File Offset: 0x0008DE87
		private IEnumerator CCooldown()
		{
			for (;;)
			{
				yield return null;
				if (this._constraints.components.Pass())
				{
					this._remainAttackCooldown -= this._owner.chronometer.master.deltaTime;
					this._remainThunderCooldown -= this._owner.chronometer.master.deltaTime;
					this._remainBombCooldown -= this._owner.chronometer.master.deltaTime;
				}
			}
			yield break;
		}

		// Token: 0x06002FED RID: 12269 RVA: 0x0008FC96 File Offset: 0x0008DE96
		private IEnumerator CPlayAnimation(int hash, float length)
		{
			this._animator.Play(hash);
			this._animator.enabled = false;
			float remain = length;
			while (remain > 1E-45f)
			{
				float deltaTime = Chronometer.global.deltaTime;
				this._animator.Update(deltaTime);
				remain -= deltaTime;
				yield return null;
			}
			this._animator.enabled = true;
			yield break;
		}

		// Token: 0x06002FEE RID: 12270 RVA: 0x0008FCB3 File Offset: 0x0008DEB3
		private IEnumerator CRun()
		{
			this._introEffect.Spawn(base.transform.position, 0f, 1f);
			yield return this.CPlayAnimation(Oberon._introHash, this._introLength);
			for (;;)
			{
				this._animator.Play(Oberon._idleHash);
				yield return null;
				this.Move(this._owner.chronometer.master.deltaTime);
				if (this._remainAttackCooldown < 0f)
				{
					Target target;
					if (this.FindAttackTarget(out target, this._attackDetectRange))
					{
						this._remainAttackCooldown = this._attackCooldown;
						this._animator.Play(Oberon._attackHash);
						this._attackEffect.Spawn(base.transform.position, 0f, 1f);
						yield return this.CSpawnAttackOperationRunner(target);
					}
					else
					{
						this._remainAttackCooldown = 0.5f;
					}
				}
				if (this._remainThunderCooldown < 0f)
				{
					Vector3 position;
					if (this.FindThunderPosition(out position))
					{
						this._remainThunderCooldown = this._thunderCooldown;
						this._animator.Play(Oberon._thunderHash);
						this._thunderEffect.Spawn(base.transform.position, 0f, 1f);
						yield return this.CSpawnThunderOperationRunner(position);
					}
					else
					{
						this._remainThunderCooldown = 0.5f;
					}
				}
				if (this._remainBombCooldown < 0f)
				{
					Target target2;
					if (this.FindAttackTarget(out target2, this._bombDetectRange))
					{
						this._remainBombCooldown = this._bombCooldown;
						yield return this.CPlayAnimation(Oberon._bombReadyHash, this._bombReadyLength);
						this._animator.Play(Oberon._bombLoopHash);
						this._bombEffect.Spawn(base.transform.position, 0f, 1f);
						this._bombScreenFlash.Run(this._owner);
						yield return this.CSpawnBombOperationRunner();
						yield return this.CPlayAnimation(Oberon._bombEndHash, this._bombEndLength);
					}
					else
					{
						this._remainBombCooldown = 0.5f;
					}
				}
			}
			yield break;
		}

		// Token: 0x06002FEF RID: 12271 RVA: 0x0008FCC4 File Offset: 0x0008DEC4
		private bool FindAttackTarget(out Target target, Collider2D collider)
		{
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(this._owner.gameObject));
			collider.enabled = true;
			this._overlapper.OverlapCollider(collider);
			collider.enabled = false;
			List<Target> components = this._overlapper.results.GetComponents(true);
			if (components.Count == 0)
			{
				target = null;
				return false;
			}
			target = components[0];
			return true;
		}

		// Token: 0x06002FF0 RID: 12272 RVA: 0x0008FD3C File Offset: 0x0008DF3C
		private bool FindThunderPosition(out Vector3 position)
		{
			position = Vector3.zero;
			this._overlapper.contactFilter.SetLayerMask(this._layer.Evaluate(this._owner.gameObject));
			this._thunderDetectRange.enabled = true;
			this._overlapper.OverlapCollider(this._thunderDetectRange);
			this._thunderDetectRange.enabled = false;
			List<Target> components = this._overlapper.results.GetComponents(true);
			if (components.Count == 0)
			{
				return false;
			}
			Target target = components[0];
			this._groundFinder.origin = target.transform.position;
			RaycastHit2D hit = this._groundFinder.SingleCast();
			if (!hit)
			{
				return false;
			}
			position = hit.point;
			return true;
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x0008FE0C File Offset: 0x0008E00C
		private IEnumerator CSpawnAttackOperationRunner(Target target)
		{
			Vector3 vector = target.collider.bounds.center - base.transform.position;
			float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
			OperationInfos spawnedOperationInfos = this._attackOperationRunner.Spawn().operationInfos;
			spawnedOperationInfos.transform.SetPositionAndRotation(base.transform.position, Quaternion.Euler(0f, 0f, z));
			spawnedOperationInfos.Run(this._owner);
			while (spawnedOperationInfos.gameObject.activeSelf)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x0008FE22 File Offset: 0x0008E022
		private IEnumerator CSpawnThunderOperationRunner(Vector3 position)
		{
			OperationInfos spawnedOperationInfos = this._thunderOperationRunner.Spawn().operationInfos;
			spawnedOperationInfos.transform.SetPositionAndRotation(position, Quaternion.identity);
			spawnedOperationInfos.Run(this._owner);
			while (spawnedOperationInfos.gameObject.activeSelf)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x0008FE38 File Offset: 0x0008E038
		private IEnumerator CSpawnBombOperationRunner()
		{
			OperationInfos spawnedOperationInfos = this._bombOperationRunner.Spawn().operationInfos;
			spawnedOperationInfos.transform.SetPositionAndRotation(this._bombSpawnPosition.position, Quaternion.identity);
			spawnedOperationInfos.Run(this._owner);
			while (spawnedOperationInfos.gameObject.activeSelf)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x0400278B RID: 10123
		private const string _attackName = "Attack";

		// Token: 0x0400278C RID: 10124
		private static readonly int _attackHash = Animator.StringToHash("Attack");

		// Token: 0x0400278D RID: 10125
		private float _attackLength;

		// Token: 0x0400278E RID: 10126
		private const string _idleName = "Idle";

		// Token: 0x0400278F RID: 10127
		private static readonly int _idleHash = Animator.StringToHash("Idle");

		// Token: 0x04002790 RID: 10128
		private float _idleLength;

		// Token: 0x04002791 RID: 10129
		private const string _introName = "Intro";

		// Token: 0x04002792 RID: 10130
		private static readonly int _introHash = Animator.StringToHash("Intro");

		// Token: 0x04002793 RID: 10131
		private float _introLength;

		// Token: 0x04002794 RID: 10132
		private const string _bombEndName = "SpiritBomb_End";

		// Token: 0x04002795 RID: 10133
		private static readonly int _bombEndHash = Animator.StringToHash("SpiritBomb_End");

		// Token: 0x04002796 RID: 10134
		private float _bombEndLength;

		// Token: 0x04002797 RID: 10135
		private const string _bombLoopName = "SpiritBomb_Loop";

		// Token: 0x04002798 RID: 10136
		private static readonly int _bombLoopHash = Animator.StringToHash("SpiritBomb_Loop");

		// Token: 0x04002799 RID: 10137
		private float _bombLoopLength;

		// Token: 0x0400279A RID: 10138
		private const string _bombReadyName = "SpiritBomb_Ready";

		// Token: 0x0400279B RID: 10139
		private static readonly int _bombReadyHash = Animator.StringToHash("SpiritBomb_Ready");

		// Token: 0x0400279C RID: 10140
		private float _bombReadyLength;

		// Token: 0x0400279D RID: 10141
		private const string _thunderName = "SpiritThunder";

		// Token: 0x0400279E RID: 10142
		private static readonly int _thunderHash = Animator.StringToHash("SpiritThunder");

		// Token: 0x0400279F RID: 10143
		private float _thunderLength;

		// Token: 0x040027A0 RID: 10144
		private Character _owner;

		// Token: 0x040027A1 RID: 10145
		[Constraint.SubcomponentAttribute]
		[SerializeField]
		private Constraint.Subcomponents _constraints;

		// Token: 0x040027A2 RID: 10146
		[Header("Movement")]
		private Transform _slot;

		// Token: 0x040027A3 RID: 10147
		[SerializeField]
		private float _trackSpeed = 2.5f;

		// Token: 0x040027A4 RID: 10148
		[SerializeField]
		private float _floatAmplitude = 0.5f;

		// Token: 0x040027A5 RID: 10149
		[SerializeField]
		private float _floatFrequency = 1f;

		// Token: 0x040027A6 RID: 10150
		[Header("Graphic")]
		[SerializeField]
		private Animator _animator;

		// Token: 0x040027A7 RID: 10151
		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		// Token: 0x040027A8 RID: 10152
		[SerializeField]
		private EffectInfo _introEffect;

		// Token: 0x040027A9 RID: 10153
		[SerializeField]
		[Header("Galaxy Beam")]
		private Collider2D _attackDetectRange;

		// Token: 0x040027AA RID: 10154
		[SerializeField]
		private EffectInfo _attackEffect;

		// Token: 0x040027AB RID: 10155
		[SerializeField]
		private OperationRunner _attackOperationRunner;

		// Token: 0x040027AC RID: 10156
		[SerializeField]
		private float _attackCooldown;

		// Token: 0x040027AD RID: 10157
		private float _remainAttackCooldown;

		// Token: 0x040027AE RID: 10158
		[Header("Spirit Thunder")]
		[SerializeField]
		private Collider2D _thunderDetectRange;

		// Token: 0x040027AF RID: 10159
		[SerializeField]
		private EffectInfo _thunderEffect;

		// Token: 0x040027B0 RID: 10160
		[SerializeField]
		private OperationRunner _thunderOperationRunner;

		// Token: 0x040027B1 RID: 10161
		[SerializeField]
		private float _thunderCooldown;

		// Token: 0x040027B2 RID: 10162
		private float _remainThunderCooldown;

		// Token: 0x040027B3 RID: 10163
		[SerializeField]
		[Header("Spirit Nemesis")]
		private Collider2D _bombDetectRange;

		// Token: 0x040027B4 RID: 10164
		[SerializeField]
		private EffectInfo _bombEffect;

		// Token: 0x040027B5 RID: 10165
		[SerializeField]
		private Characters.Operations.Fx.ScreenFlash _bombScreenFlash;

		// Token: 0x040027B6 RID: 10166
		[SerializeField]
		private OperationRunner _bombOperationRunner;

		// Token: 0x040027B7 RID: 10167
		[SerializeField]
		private float _bombCooldown;

		// Token: 0x040027B8 RID: 10168
		[SerializeField]
		private Transform _bombSpawnPosition;

		// Token: 0x040027B9 RID: 10169
		private float _remainBombCooldown;

		// Token: 0x040027BA RID: 10170
		private TargetLayer _layer = new TargetLayer(0, false, true, false, false);

		// Token: 0x040027BB RID: 10171
		private NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);

		// Token: 0x040027BC RID: 10172
		private RayCaster _groundFinder;

		// Token: 0x040027BD RID: 10173
		private Vector3 _position;

		// Token: 0x040027BE RID: 10174
		private float _floatingTime;
	}
}
