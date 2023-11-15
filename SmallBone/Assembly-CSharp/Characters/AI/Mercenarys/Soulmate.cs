using System;
using System.Collections;
using Characters.Actions;
using Characters.Operations;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.Mercenarys
{
	// Token: 0x020011D8 RID: 4568
	public class Soulmate : MonoBehaviour
	{
		// Token: 0x060059A3 RID: 22947 RVA: 0x0010A9EC File Offset: 0x00108BEC
		private void Start()
		{
			if (WitchBonus.instance.soul.fatalMind.level == 0)
			{
				return;
			}
			UnityEngine.Object.DontDestroyOnLoad(this._teleportDestination);
			this._teleportDestination.SetParent(null);
		}

		// Token: 0x060059A4 RID: 22948 RVA: 0x0010AA1C File Offset: 0x00108C1C
		private void OnEnable()
		{
			base.StartCoroutine(this.CProcess());
		}

		// Token: 0x060059A5 RID: 22949 RVA: 0x0010AA2B File Offset: 0x00108C2B
		private IEnumerator CBuff()
		{
			for (;;)
			{
				if (this._hidden)
				{
					yield return null;
				}
				else
				{
					this._buffElapsed += this._owner.chronometer.master.deltaTime;
					if (this._buffElapsed >= this._buffInterval)
					{
						this._buffs.Random<AttachAbility>().Run(this._owner);
						this._buffElapsed = 0f;
					}
					yield return null;
				}
			}
			yield break;
		}

		// Token: 0x060059A6 RID: 22950 RVA: 0x0010AA3A File Offset: 0x00108C3A
		private IEnumerator CSetOwner()
		{
			while (this._owner == null)
			{
				yield return null;
				this._owner = Singleton<Service>.Instance.levelManager.player;
			}
			yield break;
		}

		// Token: 0x060059A7 RID: 22951 RVA: 0x0010AA49 File Offset: 0x00108C49
		private IEnumerator CProcess()
		{
			yield return this.CSetOwner();
			base.StartCoroutine(this.CMove());
			base.StartCoroutine(this.CBuff());
			yield break;
		}

		// Token: 0x060059A8 RID: 22952 RVA: 0x0010AA58 File Offset: 0x00108C58
		private IEnumerator CMove()
		{
			for (;;)
			{
				if (this._hidden)
				{
					yield return null;
				}
				else
				{
					Collider2D lastStandingCollider = this._owner.movement.controller.collisionState.lastStandingCollider;
					if (lastStandingCollider == null)
					{
						yield return null;
					}
					else
					{
						Collider2D lastStandingCollider2 = this._character.movement.controller.collisionState.lastStandingCollider;
						if (lastStandingCollider2 == null)
						{
							yield return null;
						}
						else
						{
							if (lastStandingCollider != lastStandingCollider2)
							{
								this._timeAwayfromOwner += this._owner.chronometer.master.deltaTime;
								if (this._timeAwayfromOwner > this._timeToChase)
								{
									yield return this.CTeleport();
									this._timeAwayfromOwner = 0f;
								}
							}
							else
							{
								this._timeAwayfromOwner = 0f;
								float f = this._owner.transform.position.x - this._character.transform.position.x;
								if (Mathf.Abs(f) > this._minimumDistance)
								{
									this._character.movement.MoveHorizontal(new Vector2(Mathf.Sign(f), 0f));
								}
							}
							yield return null;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x060059A9 RID: 22953 RVA: 0x0010AA67 File Offset: 0x00108C67
		private IEnumerator CTeleport()
		{
			this._teleportDestination.position = this._owner.transform.position;
			this._teleport.TryStart();
			while (this._teleport.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060059AA RID: 22954 RVA: 0x0010AA78 File Offset: 0x00108C78
		public void Hide()
		{
			if (WitchBonus.instance.soul.fatalMind.level == 0)
			{
				return;
			}
			this._hidden = true;
			this._character.gameObject.SetActive(false);
			this._buffElapsed = 0f;
			this._timeAwayfromOwner = 0f;
		}

		// Token: 0x060059AB RID: 22955 RVA: 0x0010AACA File Offset: 0x00108CCA
		public IEnumerator CAppearance()
		{
			yield return this.CSetOwner();
			Collider2D lastStandingCollider;
			for (;;)
			{
				lastStandingCollider = this._owner.movement.controller.collisionState.lastStandingCollider;
				if (!(lastStandingCollider == null))
				{
					break;
				}
				yield return null;
			}
			this._teleportDestination.position = new Vector2(this._owner.transform.position.x - 1f, lastStandingCollider.bounds.max.y);
			this._character.gameObject.SetActive(true);
			this._teleport.TryStart();
			while (this._teleport.running)
			{
				yield return null;
			}
			this._hidden = false;
			yield break;
		}

		// Token: 0x0400486E RID: 18542
		[SerializeField]
		private Character _character;

		// Token: 0x0400486F RID: 18543
		[Header("Movement")]
		[SerializeField]
		private float _timeToChase = 2f;

		// Token: 0x04004870 RID: 18544
		[SerializeField]
		private float _minimumDistance = 1.5f;

		// Token: 0x04004871 RID: 18545
		[SerializeField]
		private Characters.Actions.Action _teleport;

		// Token: 0x04004872 RID: 18546
		[SerializeField]
		private Transform _teleportDestination;

		// Token: 0x04004873 RID: 18547
		[SerializeField]
		[Header("Buff")]
		private float _buffInterval = 30f;

		// Token: 0x04004874 RID: 18548
		[SerializeField]
		private AttachAbility[] _buffs;

		// Token: 0x04004875 RID: 18549
		private Character _owner;

		// Token: 0x04004876 RID: 18550
		private float _timeAwayfromOwner;

		// Token: 0x04004877 RID: 18551
		private bool _hidden;

		// Token: 0x04004878 RID: 18552
		private float _buffElapsed;
	}
}
