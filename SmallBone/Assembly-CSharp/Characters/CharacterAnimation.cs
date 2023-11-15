using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006B0 RID: 1712
	[RequireComponent(typeof(Animator))]
	public class CharacterAnimation : MonoBehaviour
	{
		// Token: 0x17000723 RID: 1827
		// (get) Token: 0x06002262 RID: 8802 RVA: 0x000673A8 File Offset: 0x000655A8
		public AnimationClip walkClip
		{
			get
			{
				return this._walkClip;
			}
		}

		// Token: 0x17000724 RID: 1828
		// (get) Token: 0x06002263 RID: 8803 RVA: 0x000673B0 File Offset: 0x000655B0
		// (set) Token: 0x06002264 RID: 8804 RVA: 0x000673B8 File Offset: 0x000655B8
		public CharacterAnimation.Parameter parameter { get; protected set; }

		// Token: 0x17000725 RID: 1829
		// (get) Token: 0x06002265 RID: 8805 RVA: 0x000673C1 File Offset: 0x000655C1
		// (set) Token: 0x06002266 RID: 8806 RVA: 0x000673CE File Offset: 0x000655CE
		public float speed
		{
			get
			{
				return this._animator.speed;
			}
			set
			{
				this._animator.speed = value;
			}
		}

		// Token: 0x17000726 RID: 1830
		// (get) Token: 0x06002267 RID: 8807 RVA: 0x000673DC File Offset: 0x000655DC
		public string key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06002268 RID: 8808 RVA: 0x000673E4 File Offset: 0x000655E4
		public SpriteRenderer spriteRenderer
		{
			get
			{
				return this._spriteRenderer;
			}
		}

		// Token: 0x17000728 RID: 1832
		// (get) Token: 0x06002269 RID: 8809 RVA: 0x000673EC File Offset: 0x000655EC
		public AnimatorParameter state
		{
			get
			{
				int tagHash = this._animator.GetCurrentAnimatorStateInfo(0).tagHash;
				if (tagHash == CharacterAnimation.action.hash)
				{
					return CharacterAnimation.action;
				}
				if (tagHash == CharacterAnimation.ground.hash)
				{
					return CharacterAnimation.ground;
				}
				if (tagHash == CharacterAnimation.air.hash)
				{
					return CharacterAnimation.air;
				}
				return null;
			}
		}

		// Token: 0x0600226A RID: 8810 RVA: 0x00067448 File Offset: 0x00065648
		private void OnDestroy()
		{
			if (this._defaultOverrider != null)
			{
				this._defaultOverrider.Dispose();
				this._defaultOverrider = null;
			}
			foreach (AnimationClipOverrider animationClipOverrider in this._overriders)
			{
				animationClipOverrider.Dispose();
			}
			this._overriders.Clear();
			this._idleClip = null;
			this._walkClip = null;
			this._jumpClip = null;
			this._fallClip = null;
			this._fallRepeatClip = null;
			this._actionClip = null;
			this._animator = null;
			if (this._spriteRenderer != null)
			{
				this._spriteRenderer.sprite = null;
			}
			this._spriteRenderer = null;
		}

		// Token: 0x0600226B RID: 8811 RVA: 0x00067510 File Offset: 0x00065710
		public void Initialize()
		{
			if (this._defaultOverrider == null)
			{
				this._defaultOverrider = new AnimationClipOverrider(this._animator.runtimeAnimatorController);
				this.AttachOverrider(this._defaultOverrider);
			}
			this._defaultOverrider.Override("EmptyIdle", this._idleClip);
			this._defaultOverrider.Override("EmptyWalk", this._walkClip);
			this._defaultOverrider.Override("EmptyJumpUp", this._jumpClip);
			this._defaultOverrider.Override("EmptyJumpDown", this._fallClip);
			this._defaultOverrider.Override("EmptyJumpDownLoop", this._fallRepeatClip);
			this.parameter = new CharacterAnimation.Parameter(this._animator);
		}

		// Token: 0x0600226C RID: 8812 RVA: 0x000675C8 File Offset: 0x000657C8
		public void Play(AnimationClip clip, float speed)
		{
			this._actionClip = clip;
			this._overriders.Last<AnimationClipOverrider>().Override("EmptyAction", clip);
			AnimationEvent animationEvent = clip.events.SingleOrDefault((AnimationEvent @event) => @event.functionName.Equals("CycleOffset"));
			AnimationEvent animationEvent2 = clip.events.SingleOrDefault((AnimationEvent @event) => @event.functionName.Equals("Repeat"));
			if (animationEvent == null)
			{
				this._cycleOffset = 0f;
			}
			else
			{
				if (animationEvent2 == null)
				{
					clip.AddEvent(new AnimationEvent
					{
						functionName = "Repeat",
						time = clip.length
					});
				}
				this._cycleOffset = animationEvent.time / clip.length;
			}
			this.Play(speed);
		}

		// Token: 0x0600226D RID: 8813 RVA: 0x00067699 File Offset: 0x00065899
		public void Play(float speed)
		{
			this.parameter.actionSpeed.Value = speed;
			this.Play();
		}

		// Token: 0x0600226E RID: 8814 RVA: 0x000676B2 File Offset: 0x000658B2
		public void Play()
		{
			this._animator.Play(CharacterAnimation.action.hash, 0, 0f);
		}

		// Token: 0x0600226F RID: 8815 RVA: 0x000676CF File Offset: 0x000658CF
		public void Stun()
		{
			this.Play(this._idleClip, 1.5f);
		}

		// Token: 0x06002270 RID: 8816 RVA: 0x000676E4 File Offset: 0x000658E4
		public void Stop()
		{
			if (!this._animator.isActiveAndEnabled)
			{
				return;
			}
			if (this._animator.GetCurrentAnimatorStateInfo(0).shortNameHash == CharacterAnimation.action.hash)
			{
				this._animator.Play(CharacterAnimation.idle.hash, 0, 0f);
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x0006773A File Offset: 0x0006593A
		public void Repeat()
		{
			this._animator.Play(CharacterAnimation.action.hash, 0, this._cycleOffset);
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x00002191 File Offset: 0x00000391
		public void CycleOffset()
		{
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x00067758 File Offset: 0x00065958
		public void SetIdle(AnimationClip clip)
		{
			if (this._defaultOverrider == null)
			{
				this._defaultOverrider = new AnimationClipOverrider(this._animator.runtimeAnimatorController);
				this.AttachOverrider(this._defaultOverrider);
			}
			this._defaultOverrider.Override("EmptyIdle", clip);
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x00067795 File Offset: 0x00065995
		public void SetWalk(AnimationClip clip)
		{
			if (this._defaultOverrider == null)
			{
				this._defaultOverrider = new AnimationClipOverrider(this._animator.runtimeAnimatorController);
				this.AttachOverrider(this._defaultOverrider);
			}
			this._defaultOverrider.Override("EmptyWalk", clip);
		}

		// Token: 0x06002275 RID: 8821 RVA: 0x000677D2 File Offset: 0x000659D2
		public void AttachOverrider(AnimationClipOverrider overrider)
		{
			if (this._overriders.Contains(overrider))
			{
				return;
			}
			this._overriders.Add(overrider);
			this._animator.runtimeAnimatorController = this._overriders.Last<AnimationClipOverrider>().animatorController;
		}

		// Token: 0x06002276 RID: 8822 RVA: 0x0006780A File Offset: 0x00065A0A
		public void DetachOverrider(AnimationClipOverrider overrider)
		{
			this._overriders.Remove(overrider);
			this._animator.runtimeAnimatorController = this._overriders.Last<AnimationClipOverrider>().animatorController;
		}

		// Token: 0x04001D33 RID: 7475
		public const string idleClipName = "EmptyIdle";

		// Token: 0x04001D34 RID: 7476
		public const string walkClipName = "EmptyWalk";

		// Token: 0x04001D35 RID: 7477
		public const string jumpUpClipName = "EmptyJumpUp";

		// Token: 0x04001D36 RID: 7478
		public const string fallClipName = "EmptyJumpDown";

		// Token: 0x04001D37 RID: 7479
		public const string fallRepeatClipName = "EmptyJumpDownLoop";

		// Token: 0x04001D38 RID: 7480
		public const string actionClipName = "EmptyAction";

		// Token: 0x04001D39 RID: 7481
		public static readonly AnimatorParameter action = new AnimatorParameter("Action");

		// Token: 0x04001D3A RID: 7482
		public static readonly AnimatorParameter idle = new AnimatorParameter("Idle");

		// Token: 0x04001D3B RID: 7483
		public static readonly AnimatorParameter ground = new AnimatorParameter("Ground");

		// Token: 0x04001D3C RID: 7484
		public static readonly AnimatorParameter air = new AnimatorParameter("Air");

		// Token: 0x04001D3D RID: 7485
		[GetComponent]
		[SerializeField]
		protected Animator _animator;

		// Token: 0x04001D3E RID: 7486
		protected AnimationClipOverrider _defaultOverrider;

		// Token: 0x04001D3F RID: 7487
		[GetComponent]
		[SerializeField]
		protected SpriteRenderer _spriteRenderer;

		// Token: 0x04001D40 RID: 7488
		[SerializeField]
		[CharacterAnimationController.KeyAttribute]
		private string _key;

		// Token: 0x04001D41 RID: 7489
		[SerializeField]
		private AnimationClip _idleClip;

		// Token: 0x04001D42 RID: 7490
		[SerializeField]
		private AnimationClip _walkClip;

		// Token: 0x04001D43 RID: 7491
		[SerializeField]
		private AnimationClip _jumpClip;

		// Token: 0x04001D44 RID: 7492
		[SerializeField]
		private AnimationClip _fallClip;

		// Token: 0x04001D45 RID: 7493
		[SerializeField]
		private AnimationClip _fallRepeatClip;

		// Token: 0x04001D46 RID: 7494
		private AnimationClip _actionClip;

		// Token: 0x04001D47 RID: 7495
		private float _cycleOffset;

		// Token: 0x04001D48 RID: 7496
		private readonly List<AnimationClipOverrider> _overriders = new List<AnimationClipOverrider>();

		// Token: 0x020006B1 RID: 1713
		public class Parameter
		{
			// Token: 0x06002279 RID: 8825 RVA: 0x00067888 File Offset: 0x00065A88
			internal Parameter(Animator animator)
			{
				this.animator = animator;
				this.walk = new AnimatorBool(animator, "Walk");
				this.movementSpeed = new AnimatorFloat(animator, "MovementSpeed");
				this.ySpeed = new AnimatorFloat(animator, "YSpeed");
				this.actionSpeed = new AnimatorFloat(animator, "ActionSpeed");
				this.grounded = new AnimatorBool(animator, "Grounded");
			}

			// Token: 0x04001D4A RID: 7498
			private readonly Animator animator;

			// Token: 0x04001D4B RID: 7499
			public readonly AnimatorBool walk;

			// Token: 0x04001D4C RID: 7500
			public readonly AnimatorBool grounded;

			// Token: 0x04001D4D RID: 7501
			public readonly AnimatorFloat movementSpeed;

			// Token: 0x04001D4E RID: 7502
			public readonly AnimatorFloat ySpeed;

			// Token: 0x04001D4F RID: 7503
			public readonly AnimatorFloat actionSpeed;
		}
	}
}
