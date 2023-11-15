using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000AA4 RID: 2724
public class ExpandRevealUIContent : MonoBehaviour
{
	// Token: 0x0600532A RID: 21290 RVA: 0x001DCDB8 File Offset: 0x001DAFB8
	private void OnDisable()
	{
		if (this.BGChildFitter)
		{
			this.BGChildFitter.WidthScale = (this.BGChildFitter.HeightScale = 0f);
		}
		if (this.MaskChildFitter)
		{
			if (this.MaskChildFitter.fitWidth)
			{
				this.MaskChildFitter.WidthScale = 0f;
			}
			if (this.MaskChildFitter.fitHeight)
			{
				this.MaskChildFitter.HeightScale = 0f;
			}
		}
		if (this.BGRectStretcher)
		{
			this.BGRectStretcher.XStretchFactor = (this.BGRectStretcher.YStretchFactor = 0f);
			this.BGRectStretcher.UpdateStretching();
		}
		if (this.MaskRectStretcher)
		{
			this.MaskRectStretcher.XStretchFactor = (this.MaskRectStretcher.YStretchFactor = 0f);
			this.MaskRectStretcher.UpdateStretching();
		}
	}

	// Token: 0x0600532B RID: 21291 RVA: 0x001DCEA4 File Offset: 0x001DB0A4
	public void Expand(Action<object> completeCallback)
	{
		if (this.MaskChildFitter && this.MaskRectStretcher)
		{
			global::Debug.LogWarning("ExpandRevealUIContent has references to both a MaskChildFitter and a MaskRectStretcher. It should have only one or the other. ChildFitter to match child size, RectStretcher to match parent size.");
		}
		if (this.BGChildFitter && this.BGRectStretcher)
		{
			global::Debug.LogWarning("ExpandRevealUIContent has references to both a BGChildFitter and a BGRectStretcher . It should have only one or the other.  ChildFitter to match child size, RectStretcher to match parent size.");
		}
		if (this.activeRoutine != null)
		{
			base.StopCoroutine(this.activeRoutine);
		}
		this.CollapsedImmediate();
		this.activeRoutineCompleteCallback = completeCallback;
		this.activeRoutine = base.StartCoroutine(this.ExpandRoutine(null));
	}

	// Token: 0x0600532C RID: 21292 RVA: 0x001DCF30 File Offset: 0x001DB130
	public void Collapse(Action<object> completeCallback)
	{
		if (this.activeRoutine != null)
		{
			if (this.activeRoutineCompleteCallback != null)
			{
				this.activeRoutineCompleteCallback(null);
			}
			base.StopCoroutine(this.activeRoutine);
		}
		this.activeRoutineCompleteCallback = completeCallback;
		if (base.gameObject.activeInHierarchy)
		{
			this.activeRoutine = base.StartCoroutine(this.CollapseRoutine(completeCallback));
			return;
		}
		this.activeRoutine = null;
		if (completeCallback != null)
		{
			completeCallback(null);
		}
	}

	// Token: 0x0600532D RID: 21293 RVA: 0x001DCF9E File Offset: 0x001DB19E
	private IEnumerator ExpandRoutine(Action<object> completeCallback)
	{
		this.Collapsing = false;
		this.Expanding = true;
		float num = 0f;
		foreach (Keyframe keyframe in this.expandAnimation.keys)
		{
			if (keyframe.time > num)
			{
				num = keyframe.time;
			}
		}
		float duration = num / this.speedScale;
		for (float remaining = duration; remaining >= 0f; remaining -= Time.unscaledDeltaTime * this.speedScale)
		{
			this.SetStretch(this.expandAnimation.Evaluate(duration - remaining));
			yield return null;
		}
		this.SetStretch(this.expandAnimation.Evaluate(duration));
		if (completeCallback != null)
		{
			completeCallback(null);
		}
		this.activeRoutine = null;
		this.Expanding = false;
		yield break;
	}

	// Token: 0x0600532E RID: 21294 RVA: 0x001DCFB4 File Offset: 0x001DB1B4
	private void SetStretch(float value)
	{
		if (this.BGRectStretcher)
		{
			if (this.BGRectStretcher.StretchX)
			{
				this.BGRectStretcher.XStretchFactor = value;
			}
			if (this.BGRectStretcher.StretchY)
			{
				this.BGRectStretcher.YStretchFactor = value;
			}
		}
		if (this.MaskRectStretcher)
		{
			if (this.MaskRectStretcher.StretchX)
			{
				this.MaskRectStretcher.XStretchFactor = value;
			}
			if (this.MaskRectStretcher.StretchY)
			{
				this.MaskRectStretcher.YStretchFactor = value;
			}
		}
		if (this.BGChildFitter)
		{
			if (this.BGChildFitter.fitWidth)
			{
				this.BGChildFitter.WidthScale = value;
			}
			if (this.BGChildFitter.fitHeight)
			{
				this.BGChildFitter.HeightScale = value;
			}
		}
		if (this.MaskChildFitter)
		{
			if (this.MaskChildFitter.fitWidth)
			{
				this.MaskChildFitter.WidthScale = value;
			}
			if (this.MaskChildFitter.fitHeight)
			{
				this.MaskChildFitter.HeightScale = value;
			}
		}
	}

	// Token: 0x0600532F RID: 21295 RVA: 0x001DD0BD File Offset: 0x001DB2BD
	private IEnumerator CollapseRoutine(Action<object> completeCallback)
	{
		this.Expanding = false;
		this.Collapsing = true;
		float num = 0f;
		foreach (Keyframe keyframe in this.collapseAnimation.keys)
		{
			if (keyframe.time > num)
			{
				num = keyframe.time;
			}
		}
		float duration = num;
		for (float remaining = duration; remaining >= 0f; remaining -= Time.unscaledDeltaTime)
		{
			this.SetStretch(this.collapseAnimation.Evaluate(duration - remaining));
			yield return null;
		}
		this.SetStretch(this.collapseAnimation.Evaluate(duration));
		if (completeCallback != null)
		{
			completeCallback(null);
		}
		this.activeRoutine = null;
		this.Collapsing = false;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06005330 RID: 21296 RVA: 0x001DD0D4 File Offset: 0x001DB2D4
	public void CollapsedImmediate()
	{
		float time = (float)this.collapseAnimation.length;
		this.SetStretch(this.collapseAnimation.Evaluate(time));
	}

	// Token: 0x04003755 RID: 14165
	private Coroutine activeRoutine;

	// Token: 0x04003756 RID: 14166
	private Action<object> activeRoutineCompleteCallback;

	// Token: 0x04003757 RID: 14167
	public AnimationCurve expandAnimation;

	// Token: 0x04003758 RID: 14168
	public AnimationCurve collapseAnimation;

	// Token: 0x04003759 RID: 14169
	public KRectStretcher MaskRectStretcher;

	// Token: 0x0400375A RID: 14170
	public KRectStretcher BGRectStretcher;

	// Token: 0x0400375B RID: 14171
	public KChildFitter MaskChildFitter;

	// Token: 0x0400375C RID: 14172
	public KChildFitter BGChildFitter;

	// Token: 0x0400375D RID: 14173
	public float speedScale = 1f;

	// Token: 0x0400375E RID: 14174
	public bool Collapsing;

	// Token: 0x0400375F RID: 14175
	public bool Expanding;
}
