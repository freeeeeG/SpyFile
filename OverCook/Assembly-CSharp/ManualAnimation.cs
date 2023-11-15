using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200051E RID: 1310
[Serializable]
public class ManualAnimation
{
	// Token: 0x0600186A RID: 6250 RVA: 0x0007C054 File Offset: 0x0007A454
	public IEnumerator Run(GameObject obj, Transform attachPoint, Vector3 offset)
	{
		float time = 0f;
		float length = this.m_CurveTime;
		float endTime = time + length;
		Vector3 startingScale = obj.transform.localScale;
		if (this.m_bUseStartScale)
		{
			startingScale = this.m_StartScaleValue;
		}
		Vector3 startingPosition = obj.transform.position;
		if (this.m_bUseStartPosition)
		{
			startingPosition = attachPoint.rotation * this.m_StartPositionValue + attachPoint.position;
		}
		Vector3 targetScale = this.m_TargetScaleValue;
		Vector3 targetPosition = attachPoint.rotation * this.m_TargetPositionValue + attachPoint.position + offset;
		while (time < endTime)
		{
			float position = 1f - (endTime - time) / length;
			obj.transform.localScale = Vector3.Lerp(startingScale, targetScale, this.m_ScaleCurve.Evaluate(position));
			obj.transform.position = Vector3.Lerp(startingPosition, targetPosition, this.m_PositionCurve.Evaluate(position));
			time += TimeManager.GetDeltaTime(obj);
			yield return null;
		}
		obj.transform.localScale = targetScale;
		obj.transform.position = targetPosition;
		yield break;
	}

	// Token: 0x040013A2 RID: 5026
	public AnimationCurve m_ScaleCurve;

	// Token: 0x040013A3 RID: 5027
	public AnimationCurve m_PositionCurve;

	// Token: 0x040013A4 RID: 5028
	public bool m_bUseStartPosition;

	// Token: 0x040013A5 RID: 5029
	[HideInInspectorTest("m_bUseStartPosition", true)]
	public Vector3 m_StartPositionValue = default(Vector3);

	// Token: 0x040013A6 RID: 5030
	public Vector3 m_TargetPositionValue = default(Vector3);

	// Token: 0x040013A7 RID: 5031
	public bool m_bUseStartScale;

	// Token: 0x040013A8 RID: 5032
	[HideInInspectorTest("m_bUseStartPosition", true)]
	public Vector3 m_StartScaleValue = default(Vector3);

	// Token: 0x040013A9 RID: 5033
	public Vector3 m_TargetScaleValue = default(Vector3);

	// Token: 0x040013AA RID: 5034
	public float m_CurveTime = 1f;
}
