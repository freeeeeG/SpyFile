using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000019 RID: 25
[Serializable]
public class TransformTweenBehaviour : PlayableBehaviour
{
	// Token: 0x0600003B RID: 59 RVA: 0x00002D26 File Offset: 0x00000F26
	public override void PrepareFrame(Playable playable, FrameData info)
	{
		if (this.startLocation)
		{
			this.startingPosition = this.startLocation.position;
			this.startingRotation = this.startLocation.rotation;
		}
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002D58 File Offset: 0x00000F58
	public float EvaluateCurrentCurve(float time)
	{
		if (this.tweenType == TransformTweenBehaviour.TweenType.Custom && !this.IsCustomCurveNormalised())
		{
			Debug.LogError("Custom Curve is not normalised.  Curve must start at 0,0 and end at 1,1.");
			return 0f;
		}
		switch (this.tweenType)
		{
		case TransformTweenBehaviour.TweenType.Linear:
			return this.m_LinearCurve.Evaluate(time);
		case TransformTweenBehaviour.TweenType.Deceleration:
			return this.m_DecelerationCurve.Evaluate(time);
		case TransformTweenBehaviour.TweenType.Harmonic:
			return this.m_HarmonicCurve.Evaluate(time);
		default:
			return this.customCurve.Evaluate(time);
		}
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002DD4 File Offset: 0x00000FD4
	private bool IsCustomCurveNormalised()
	{
		return Mathf.Approximately(this.customCurve[0].time, 0f) && Mathf.Approximately(this.customCurve[0].value, 0f) && Mathf.Approximately(this.customCurve[this.customCurve.length - 1].time, 1f) && Mathf.Approximately(this.customCurve[this.customCurve.length - 1].value, 1f);
	}

	// Token: 0x04000036 RID: 54
	public Transform startLocation;

	// Token: 0x04000037 RID: 55
	public Transform endLocation;

	// Token: 0x04000038 RID: 56
	public bool tweenPosition = true;

	// Token: 0x04000039 RID: 57
	public bool tweenRotation = true;

	// Token: 0x0400003A RID: 58
	public TransformTweenBehaviour.TweenType tweenType;

	// Token: 0x0400003B RID: 59
	public AnimationCurve customCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400003C RID: 60
	public Vector3 startingPosition;

	// Token: 0x0400003D RID: 61
	public Quaternion startingRotation = Quaternion.identity;

	// Token: 0x0400003E RID: 62
	private AnimationCurve m_LinearCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

	// Token: 0x0400003F RID: 63
	private AnimationCurve m_DecelerationCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f, -1.5707964f, 1.5707964f),
		new Keyframe(1f, 1f, 0f, 0f)
	});

	// Token: 0x04000040 RID: 64
	private AnimationCurve m_HarmonicCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

	// Token: 0x04000041 RID: 65
	private const float k_RightAngleInRads = 1.5707964f;

	// Token: 0x020000F2 RID: 242
	public enum TweenType
	{
		// Token: 0x04000376 RID: 886
		Linear,
		// Token: 0x04000377 RID: 887
		Deceleration,
		// Token: 0x04000378 RID: 888
		Harmonic,
		// Token: 0x04000379 RID: 889
		Custom
	}
}
