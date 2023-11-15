using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x02001142 RID: 4418
	public class YggdrasillAnimationController : MonoBehaviour
	{
		// Token: 0x060055F2 RID: 22002 RVA: 0x00100099 File Offset: 0x000FE299
		private void Awake()
		{
			this.CreateMapper(this._phase1Container, this._phase1Mapper);
			this.CreateMapper(this._phase2Container, this._phase2Mapper);
		}

		// Token: 0x060055F3 RID: 22003 RVA: 0x001000C0 File Offset: 0x000FE2C0
		private void CreateMapper(Transform transform, Dictionary<YggdrasillAnimation.Tag, CharacterAnimationController.AnimationInfo> mapper)
		{
			foreach (YggdrasillAnimation yggdrasillAnimation in transform.GetComponentsInChildren<YggdrasillAnimation>())
			{
				mapper.Add(yggdrasillAnimation.tag, yggdrasillAnimation.info);
			}
		}

		// Token: 0x060055F4 RID: 22004 RVA: 0x001000F8 File Offset: 0x000FE2F8
		public IEnumerator CPlayAndWaitAnimation(YggdrasillAnimation.Tag tag)
		{
			CharacterAnimationController.AnimationInfo animationInfo;
			if (!this._phase1Mapper.TryGetValue(tag, out animationInfo) && !this._phase2Mapper.TryGetValue(tag, out animationInfo))
			{
				yield break;
			}
			this._owner.animationController.Play(animationInfo, this._speed);
			yield return this._owner.chronometer.animation.WaitForSeconds(animationInfo.dictionary["Behind"].length);
			yield break;
		}

		// Token: 0x060055F5 RID: 22005 RVA: 0x0010010E File Offset: 0x000FE30E
		public void PlayCutSceneAnimation()
		{
			base.StartCoroutine(this.<PlayCutSceneAnimation>g__CLoop|10_0());
		}

		// Token: 0x060055F7 RID: 22007 RVA: 0x00100146 File Offset: 0x000FE346
		[CompilerGenerated]
		private IEnumerator <PlayCutSceneAnimation>g__CLoop|10_0()
		{
			for (;;)
			{
				yield return this.CPlayAndWaitAnimation(YggdrasillAnimation.Tag.P1_Idle_CutScene);
			}
			yield break;
		}

		// Token: 0x04004502 RID: 17666
		[SerializeField]
		private Character _owner;

		// Token: 0x04004503 RID: 17667
		[SerializeField]
		private Transform _phase1Container;

		// Token: 0x04004504 RID: 17668
		[SerializeField]
		private Transform _phase2Container;

		// Token: 0x04004505 RID: 17669
		private Dictionary<YggdrasillAnimation.Tag, CharacterAnimationController.AnimationInfo> _phase1Mapper = new Dictionary<YggdrasillAnimation.Tag, CharacterAnimationController.AnimationInfo>();

		// Token: 0x04004506 RID: 17670
		private Dictionary<YggdrasillAnimation.Tag, CharacterAnimationController.AnimationInfo> _phase2Mapper = new Dictionary<YggdrasillAnimation.Tag, CharacterAnimationController.AnimationInfo>();

		// Token: 0x04004507 RID: 17671
		private float _speed = 1f;

		// Token: 0x04004508 RID: 17672
		private const string _referenceAnimationTag = "Behind";
	}
}
