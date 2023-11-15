using System;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02000526 RID: 1318
[ExecutionDependency(typeof(PlayerControls))]
public class MultiplayerCamera : MonoBehaviour
{
	// Token: 0x060018A8 RID: 6312 RVA: 0x0007D274 File Offset: 0x0007B674
	private void Awake()
	{
		this.m_camera = base.gameObject.RequireComponentRecursive<Camera>();
		PlayerIDProvider.OnPlayerIDProviderDestroyed = (GenericVoid)Delegate.Combine(PlayerIDProvider.OnPlayerIDProviderDestroyed, new GenericVoid(this.OnPlayerIDProviderDestroyed));
		this.SetupAvatars();
		this.m_basePos = base.transform.position + base.transform.forward * this.GetAverageDistance();
		base.transform.position = this.GetIdealLocation();
		this.m_bStarted = true;
	}

	// Token: 0x060018A9 RID: 6313 RVA: 0x0007D2FC File Offset: 0x0007B6FC
	private void OnDestroy()
	{
		PlayerIDProvider.OnPlayerIDProviderDestroyed = (GenericVoid)Delegate.Remove(PlayerIDProvider.OnPlayerIDProviderDestroyed, new GenericVoid(this.OnPlayerIDProviderDestroyed));
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x0007D31E File Offset: 0x0007B71E
	private void OnPlayerIDProviderDestroyed()
	{
		if (this.m_bStarted)
		{
			this.SetupAvatars();
		}
	}

	// Token: 0x060018AB RID: 6315 RVA: 0x0007D334 File Offset: 0x0007B734
	private void SetupAvatars()
	{
		int count = PlayerIDProvider.s_AllProviders.Count;
		this.m_avatars = new Transform[count];
		for (int i = 0; i < count; i++)
		{
			this.m_avatars[i] = PlayerIDProvider.s_AllProviders._items[i].transform;
		}
	}

	// Token: 0x060018AC RID: 6316 RVA: 0x0007D384 File Offset: 0x0007B784
	private float GetAverageDistance()
	{
		Camera camera = this.m_camera;
		Vector3 position = base.transform.position;
		Vector3 forward = base.transform.forward;
		float num = 0f;
		if (this.m_avatars != null)
		{
			for (int i = 0; i < this.m_avatars.Length; i++)
			{
				Vector3 lhs = this.m_avatars[i].position - position;
				num += Vector3.Dot(lhs, forward);
			}
			return num / (float)this.m_avatars.Length;
		}
		return 0f;
	}

	// Token: 0x060018AD RID: 6317 RVA: 0x0007D410 File Offset: 0x0007B810
	private Vector3 GetCentralizedCameraPosition()
	{
		if (this.m_avatars != null)
		{
			Camera camera = this.m_camera;
			Vector3 up = base.transform.up;
			float num = (float)this.m_avatars.Length;
			float num2 = 0f;
			for (int i = 0; i < this.m_avatars.Length; i++)
			{
				num2 += Vector3.Dot(this.m_avatars[i].position - this.m_basePos, up);
			}
			float num3 = 1f / num * num2;
			Vector3 right = base.transform.right;
			float num4 = 0f;
			for (int j = 0; j < this.m_avatars.Length; j++)
			{
				num4 += Vector3.Dot(this.m_avatars[j].position - this.m_basePos, right);
			}
			float num5 = 1f / num * num4;
			num3 = Mathf.Clamp(num3, -this.m_maxUDistance, this.m_maxUDistance);
			num5 = Mathf.Clamp(num5, -this.m_maxRDistance, this.m_maxRDistance);
			return this.m_basePos + up * num3 + right * num5;
		}
		return Vector3.zero;
	}

	// Token: 0x060018AE RID: 6318 RVA: 0x0007D54C File Offset: 0x0007B94C
	private float GetIdealDistance(Vector3 _centralisedCamera)
	{
		if (this.m_avatars != null)
		{
			Camera camera = this.m_camera;
			float fieldOfView = camera.fieldOfView;
			float aspect = camera.aspect;
			Vector3 right = base.transform.right;
			Vector3 up = base.transform.up;
			Vector3 forward = base.transform.forward;
			float num = 0f;
			float num2 = Mathf.Tan(0.5f * fieldOfView);
			for (int i = 0; i < this.m_avatars.Length; i++)
			{
				Vector3 position = this.m_avatars[i].position;
				Vector3 lhs = position - _centralisedCamera;
				float num3 = Vector3.Dot(lhs, right) / (2f * aspect * num2 * (0.5f - this.m_rightEdgeBuffer));
				float num4 = -Vector3.Dot(lhs, right) / (2f * aspect * num2 * (0.5f - this.m_leftEdgeBuffer));
				float num5 = Vector3.Dot(lhs, up) / (2f * num2 * (0.5f - this.m_topEdgeBuffer));
				float num6 = -Vector3.Dot(lhs, up) / (2f * num2 * (0.5f - this.m_bottomEdgeBuffer));
				float num7 = Vector3.Dot(this.m_basePos - position, forward);
				num3 += num7;
				num4 += num7;
				num5 += num7;
				num6 += num7;
				num = Mathf.Max(num3, num);
				num = Mathf.Max(num4, num);
				num = Mathf.Max(num5, num);
				num = Mathf.Max(num6, num);
			}
			return num;
		}
		return 0f;
	}

	// Token: 0x060018AF RID: 6319 RVA: 0x0007D6DC File Offset: 0x0007BADC
	private Vector3 GetIdealLocation()
	{
		Vector3 centralizedCameraPosition = this.GetCentralizedCameraPosition();
		float d = Mathf.Clamp(this.GetIdealDistance(centralizedCameraPosition), this.m_minDistance, this.m_maxDistance);
		return centralizedCameraPosition - d * base.transform.forward;
	}

	// Token: 0x060018B0 RID: 6320 RVA: 0x0007D720 File Offset: 0x0007BB20
	private void FixedUpdate()
	{
		if (this.m_bStarted)
		{
			Vector3 idealLocation = this.GetIdealLocation();
			float magnitude = (idealLocation - base.transform.position).magnitude;
			float fixedDeltaTime = TimeManager.GetFixedDeltaTime(base.gameObject);
			MathUtils.AdvanceToTarget_Sinusoidal(ref magnitude, ref this.m_currentGradient, 0f, this.m_gradientLimit, this.m_timeToMax, fixedDeltaTime);
			base.transform.position = idealLocation - (idealLocation - base.transform.position).SafeNormalised(Vector3.zero) * magnitude;
		}
	}

	// Token: 0x040013CC RID: 5068
	private Transform[] m_avatars;

	// Token: 0x040013CD RID: 5069
	[SerializeField]
	private float m_gradientLimit = 0.5f;

	// Token: 0x040013CE RID: 5070
	[SerializeField]
	private float m_timeToMax = 0.5f;

	// Token: 0x040013CF RID: 5071
	[SerializeField]
	[FormerlySerializedAs("m_xEdgeBuffer")]
	[Range(0f, 0.5f)]
	private float m_leftEdgeBuffer = 0.2f;

	// Token: 0x040013D0 RID: 5072
	[SerializeField]
	[FormerlySerializedAs("m_xEdgeBuffer")]
	[Range(0f, 0.5f)]
	private float m_rightEdgeBuffer = 0.2f;

	// Token: 0x040013D1 RID: 5073
	[SerializeField]
	[FormerlySerializedAs("m_yEdgeBuffer")]
	[Range(0f, 0.5f)]
	private float m_topEdgeBuffer = 0.2f;

	// Token: 0x040013D2 RID: 5074
	[SerializeField]
	[FormerlySerializedAs("m_yEdgeBuffer")]
	[Range(0f, 0.5f)]
	private float m_bottomEdgeBuffer = 0.2f;

	// Token: 0x040013D3 RID: 5075
	[SerializeField]
	private float m_minDistance = 5f;

	// Token: 0x040013D4 RID: 5076
	[SerializeField]
	private float m_maxDistance = 50f;

	// Token: 0x040013D5 RID: 5077
	[SerializeField]
	private float m_maxUDistance = 2f;

	// Token: 0x040013D6 RID: 5078
	[SerializeField]
	private float m_maxRDistance = 2f;

	// Token: 0x040013D7 RID: 5079
	private Vector3 m_basePos;

	// Token: 0x040013D8 RID: 5080
	private float m_currentGradient;

	// Token: 0x040013D9 RID: 5081
	private Camera m_camera;

	// Token: 0x040013DA RID: 5082
	private bool m_bStarted;
}
