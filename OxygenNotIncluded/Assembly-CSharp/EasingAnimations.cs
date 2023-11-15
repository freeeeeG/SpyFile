using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000AA3 RID: 2723
public class EasingAnimations : MonoBehaviour
{
	// Token: 0x17000607 RID: 1543
	// (get) Token: 0x06005320 RID: 21280 RVA: 0x001DCB1B File Offset: 0x001DAD1B
	public bool IsPlaying
	{
		get
		{
			return this.animationCoroutine != null;
		}
	}

	// Token: 0x06005321 RID: 21281 RVA: 0x001DCB26 File Offset: 0x001DAD26
	private void Start()
	{
		if (this.animationMap == null || this.animationMap.Count == 0)
		{
			this.Initialize();
		}
	}

	// Token: 0x06005322 RID: 21282 RVA: 0x001DCB44 File Offset: 0x001DAD44
	private void Initialize()
	{
		this.animationMap = new Dictionary<string, EasingAnimations.AnimationScales>();
		foreach (EasingAnimations.AnimationScales animationScales in this.scales)
		{
			this.animationMap.Add(animationScales.name, animationScales);
		}
	}

	// Token: 0x06005323 RID: 21283 RVA: 0x001DCB8C File Offset: 0x001DAD8C
	public void PlayAnimation(string animationName, float delay = 0f)
	{
		if (this.animationMap == null || this.animationMap.Count == 0)
		{
			this.Initialize();
		}
		if (!this.animationMap.ContainsKey(animationName))
		{
			return;
		}
		if (this.animationCoroutine != null)
		{
			base.StopCoroutine(this.animationCoroutine);
		}
		this.currentAnimation = this.animationMap[animationName];
		this.currentAnimation.currentScale = this.currentAnimation.startScale;
		base.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
		this.animationCoroutine = base.StartCoroutine(this.ExecuteAnimation(delay));
	}

	// Token: 0x06005324 RID: 21284 RVA: 0x001DCC32 File Offset: 0x001DAE32
	private IEnumerator ExecuteAnimation(float delay)
	{
		float startTime = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < startTime + delay)
		{
			yield return SequenceUtil.WaitForNextFrame;
		}
		startTime = Time.realtimeSinceStartup;
		bool keepAnimating = true;
		while (keepAnimating)
		{
			float num = Time.realtimeSinceStartup - startTime;
			this.currentAnimation.currentScale = this.GetEasing(num * this.currentAnimation.easingMultiplier);
			if (this.currentAnimation.endScale > this.currentAnimation.startScale)
			{
				keepAnimating = (this.currentAnimation.currentScale < this.currentAnimation.endScale - 0.025f);
			}
			else
			{
				keepAnimating = (this.currentAnimation.currentScale > this.currentAnimation.endScale + 0.025f);
			}
			if (!keepAnimating)
			{
				this.currentAnimation.currentScale = this.currentAnimation.endScale;
			}
			base.transform.localScale = Vector3.one * this.currentAnimation.currentScale;
			yield return SequenceUtil.WaitForEndOfFrame;
		}
		this.animationCoroutine = null;
		if (this.OnAnimationDone != null)
		{
			this.OnAnimationDone(this.currentAnimation.name);
		}
		yield break;
	}

	// Token: 0x06005325 RID: 21285 RVA: 0x001DCC48 File Offset: 0x001DAE48
	private float GetEasing(float t)
	{
		EasingAnimations.AnimationScales.AnimationType type = this.currentAnimation.type;
		if (type == EasingAnimations.AnimationScales.AnimationType.EaseOutBack)
		{
			return this.EaseOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
		}
		if (type == EasingAnimations.AnimationScales.AnimationType.EaseInBack)
		{
			return this.EaseInBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
		}
		return this.EaseInOutBack(this.currentAnimation.currentScale, this.currentAnimation.endScale, t);
	}

	// Token: 0x06005326 RID: 21286 RVA: 0x001DCCC4 File Offset: 0x001DAEC4
	public float EaseInOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value /= 0.5f;
		if (value < 1f)
		{
			num *= 1.525f;
			return end * 0.5f * (value * value * ((num + 1f) * value - num)) + start;
		}
		value -= 2f;
		num *= 1.525f;
		return end * 0.5f * (value * value * ((num + 1f) * value + num) + 2f) + start;
	}

	// Token: 0x06005327 RID: 21287 RVA: 0x001DCD40 File Offset: 0x001DAF40
	public float EaseInBack(float start, float end, float value)
	{
		end -= start;
		value /= 1f;
		float num = 1.70158f;
		return end * value * value * ((num + 1f) * value - num) + start;
	}

	// Token: 0x06005328 RID: 21288 RVA: 0x001DCD74 File Offset: 0x001DAF74
	public float EaseOutBack(float start, float end, float value)
	{
		float num = 1.70158f;
		end -= start;
		value -= 1f;
		return end * (value * value * ((num + 1f) * value + num) + 1f) + start;
	}

	// Token: 0x04003750 RID: 14160
	public EasingAnimations.AnimationScales[] scales;

	// Token: 0x04003751 RID: 14161
	private EasingAnimations.AnimationScales currentAnimation;

	// Token: 0x04003752 RID: 14162
	private Coroutine animationCoroutine;

	// Token: 0x04003753 RID: 14163
	private Dictionary<string, EasingAnimations.AnimationScales> animationMap;

	// Token: 0x04003754 RID: 14164
	public Action<string> OnAnimationDone;

	// Token: 0x020019B7 RID: 6583
	[Serializable]
	public struct AnimationScales
	{
		// Token: 0x04007714 RID: 30484
		public string name;

		// Token: 0x04007715 RID: 30485
		public float startScale;

		// Token: 0x04007716 RID: 30486
		public float endScale;

		// Token: 0x04007717 RID: 30487
		public EasingAnimations.AnimationScales.AnimationType type;

		// Token: 0x04007718 RID: 30488
		public float easingMultiplier;

		// Token: 0x04007719 RID: 30489
		[HideInInspector]
		public float currentScale;

		// Token: 0x02002228 RID: 8744
		public enum AnimationType
		{
			// Token: 0x040098C8 RID: 39112
			EaseInOutBack,
			// Token: 0x040098C9 RID: 39113
			EaseOutBack,
			// Token: 0x040098CA RID: 39114
			EaseInBack
		}
	}
}
