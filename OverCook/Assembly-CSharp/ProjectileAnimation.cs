using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000557 RID: 1367
[Serializable]
public class ProjectileAnimation
{
	// Token: 0x060019CB RID: 6603 RVA: 0x00081368 File Offset: 0x0007F768
	public IEnumerator Run(GameObject obj, Transform target)
	{
		float time = 0f;
		float length = this.m_CurveTime;
		float endTime = time + length;
		Quaternion startingRotation = obj.transform.rotation;
		if (this.m_bUseStartRotation)
		{
			startingRotation = Quaternion.Euler(this.m_StartRotationValue);
		}
		Vector3 targetPosition = target.position;
		Quaternion targetRotation = Quaternion.Euler(target.eulerAngles + this.m_TargetRotationValue);
		Vector3 startingPosition = obj.transform.position;
		float startingHeight = obj.transform.position.y;
		float maxHeight = startingHeight + this.m_MaxHeightValue;
		while (time < endTime)
		{
			if (obj == null)
			{
				break;
			}
			float position = 1f - (endTime - time) / length;
			obj.transform.rotation = Quaternion.Lerp(startingRotation, targetRotation, this.m_RotationCurve.Evaluate(position));
			Vector3 newPosition = Vector3.Lerp(startingPosition, targetPosition, position);
			if (position < 0.5f)
			{
				newPosition.y = Mathf.Lerp(startingHeight, maxHeight, this.m_HeightCurve.Evaluate(position));
			}
			else
			{
				newPosition.y = Mathf.Lerp(targetPosition.y, maxHeight, this.m_HeightCurve.Evaluate(position));
			}
			obj.transform.position = newPosition;
			time += TimeManager.GetDeltaTime(obj);
			yield return null;
		}
		if (obj != null)
		{
			obj.transform.rotation = targetRotation;
			obj.transform.position = targetPosition;
		}
		yield break;
	}

	// Token: 0x04001479 RID: 5241
	public AnimationCurve m_RotationCurve;

	// Token: 0x0400147A RID: 5242
	public AnimationCurve m_HeightCurve;

	// Token: 0x0400147B RID: 5243
	public bool m_bUseStartRotation;

	// Token: 0x0400147C RID: 5244
	[HideInInspectorTest("m_bUseStartRotation", true)]
	public Vector3 m_StartRotationValue = default(Vector3);

	// Token: 0x0400147D RID: 5245
	public Vector3 m_TargetRotationValue = default(Vector3);

	// Token: 0x0400147E RID: 5246
	public float m_MaxHeightValue = 30f;

	// Token: 0x0400147F RID: 5247
	public float m_CurveTime = 1f;
}
