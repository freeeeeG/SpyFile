using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x0200001B RID: 27
public class TransformTweenMixerBehaviour : PlayableBehaviour
{
	// Token: 0x06000042 RID: 66 RVA: 0x00002FCC File Offset: 0x000011CC
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		Transform transform = playerData as Transform;
		if (transform == null)
		{
			return;
		}
		Vector3 position = transform.position;
		Quaternion rotation = transform.rotation;
		int inputCount = playable.GetInputCount<Playable>();
		float num = 0f;
		float num2 = 0f;
		Vector3 vector = Vector3.zero;
		Quaternion quaternion = new Quaternion(0f, 0f, 0f, 0f);
		for (int i = 0; i < inputCount; i++)
		{
			ScriptPlayable<TransformTweenBehaviour> playable2 = (ScriptPlayable<T>)playable.GetInput(i);
			TransformTweenBehaviour behaviour = playable2.GetBehaviour();
			if (!(behaviour.endLocation == null))
			{
				float inputWeight = playable.GetInputWeight(i);
				if (!this.m_FirstFrameHappened && !behaviour.startLocation)
				{
					behaviour.startingPosition = position;
					behaviour.startingRotation = rotation;
				}
				float time = (float)(playable2.GetTime<ScriptPlayable<TransformTweenBehaviour>>() / playable2.GetDuration<ScriptPlayable<TransformTweenBehaviour>>());
				float t = behaviour.EvaluateCurrentCurve(time);
				if (behaviour.tweenPosition)
				{
					num += inputWeight;
					vector += Vector3.Lerp(behaviour.startingPosition, behaviour.endLocation.position, t) * inputWeight;
				}
				if (behaviour.tweenRotation)
				{
					num2 += inputWeight;
					Quaternion quaternion2 = Quaternion.Lerp(behaviour.startingRotation, behaviour.endLocation.rotation, t);
					quaternion2 = TransformTweenMixerBehaviour.NormalizeQuaternion(quaternion2);
					if (Quaternion.Dot(quaternion, quaternion2) < 0f)
					{
						quaternion2 = TransformTweenMixerBehaviour.ScaleQuaternion(quaternion2, -1f);
					}
					quaternion2 = TransformTweenMixerBehaviour.ScaleQuaternion(quaternion2, inputWeight);
					quaternion = TransformTweenMixerBehaviour.AddQuaternions(quaternion, quaternion2);
				}
			}
		}
		vector += position * (1f - num);
		Quaternion second = TransformTweenMixerBehaviour.ScaleQuaternion(rotation, 1f - num2);
		quaternion = TransformTweenMixerBehaviour.AddQuaternions(quaternion, second);
		transform.position = vector;
		transform.rotation = quaternion;
		this.m_FirstFrameHappened = true;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000031A8 File Offset: 0x000013A8
	public override void OnPlayableDestroy(Playable playable)
	{
		this.m_FirstFrameHappened = false;
	}

	// Token: 0x06000044 RID: 68 RVA: 0x000031B4 File Offset: 0x000013B4
	private static Quaternion AddQuaternions(Quaternion first, Quaternion second)
	{
		first.w += second.w;
		first.x += second.x;
		first.y += second.y;
		first.z += second.z;
		return first;
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00003206 File Offset: 0x00001406
	private static Quaternion ScaleQuaternion(Quaternion rotation, float multiplier)
	{
		rotation.w *= multiplier;
		rotation.x *= multiplier;
		rotation.y *= multiplier;
		rotation.z *= multiplier;
		return rotation;
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00003239 File Offset: 0x00001439
	private static float QuaternionMagnitude(Quaternion rotation)
	{
		return Mathf.Sqrt(Quaternion.Dot(rotation, rotation));
	}

	// Token: 0x06000047 RID: 71 RVA: 0x00003248 File Offset: 0x00001448
	private static Quaternion NormalizeQuaternion(Quaternion rotation)
	{
		float num = TransformTweenMixerBehaviour.QuaternionMagnitude(rotation);
		if (num > 0f)
		{
			return TransformTweenMixerBehaviour.ScaleQuaternion(rotation, 1f / num);
		}
		Debug.LogWarning("Cannot normalize a quaternion with zero magnitude.");
		return Quaternion.identity;
	}

	// Token: 0x04000045 RID: 69
	private bool m_FirstFrameHappened;
}
