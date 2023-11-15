using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008FF RID: 2303
public class RemoteChefPositionRecorder : MonoBehaviour
{
	// Token: 0x06002CEC RID: 11500 RVA: 0x000D418C File Offset: 0x000D258C
	public virtual void Awake()
	{
		for (int i = 0; i < 100; i++)
		{
			this.m_PositionHistory[i] = new RemoteChefPositionRecorder.PositionData();
		}
		RemoteChefPositionRecorder.ms_AllRemoteChefPositionRecorders.Add(this);
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06002CED RID: 11501 RVA: 0x000D41D0 File Offset: 0x000D25D0
	public virtual void OnDestroy()
	{
		RemoteChefPositionRecorder.ms_AllRemoteChefPositionRecorders.Remove(this);
	}

	// Token: 0x06002CEE RID: 11502 RVA: 0x000D41E0 File Offset: 0x000D25E0
	public static void SetRemoteChefsToTime(float _time)
	{
		for (int i = 0; i < RemoteChefPositionRecorder.ms_AllRemoteChefPositionRecorders.Count; i++)
		{
			RemoteChefPositionRecorder.ms_AllRemoteChefPositionRecorders[i].InternalSetChefToTime(_time);
		}
	}

	// Token: 0x06002CEF RID: 11503 RVA: 0x000D421C File Offset: 0x000D261C
	public static void SetChefRestorePoint()
	{
		for (int i = 0; i < RemoteChefPositionRecorder.ms_AllRemoteChefPositionRecorders.Count; i++)
		{
			RemoteChefPositionRecorder.ms_AllRemoteChefPositionRecorders[i].InternalSetChefRestorePoint();
		}
	}

	// Token: 0x06002CF0 RID: 11504 RVA: 0x000D4254 File Offset: 0x000D2654
	public static void RestoreChefsPositions()
	{
		for (int i = 0; i < RemoteChefPositionRecorder.ms_AllRemoteChefPositionRecorders.Count; i++)
		{
			RemoteChefPositionRecorder.ms_AllRemoteChefPositionRecorders[i].InternalRestorePosition();
		}
	}

	// Token: 0x06002CF1 RID: 11505 RVA: 0x000D428C File Offset: 0x000D268C
	private void InternalSetChefRestorePoint()
	{
		this.m_RestorePosition = this.m_Rigidbody.position;
		this.m_RestoreVelocity = this.m_Rigidbody.velocity;
	}

	// Token: 0x06002CF2 RID: 11506 RVA: 0x000D42B0 File Offset: 0x000D26B0
	private void InternalRestorePosition()
	{
		this.m_Rigidbody.position = this.m_RestorePosition;
		this.m_Rigidbody.velocity = this.m_RestoreVelocity;
	}

	// Token: 0x06002CF3 RID: 11507 RVA: 0x000D42D4 File Offset: 0x000D26D4
	private void InternalSetChefToTime(float _time)
	{
		this.m_Rigidbody.position = this.GetChefPosition(_time);
	}

	// Token: 0x06002CF4 RID: 11508 RVA: 0x000D42E8 File Offset: 0x000D26E8
	public void RecordData(float _time, Vector3 _position, Vector3 _velocity)
	{
		RemoteChefPositionRecorder.PositionData positionData = this.m_PositionHistory[this.m_CurrentIndex++];
		if (this.m_CurrentIndex == 100)
		{
			this.m_CurrentIndex = 0;
		}
		positionData.Time = _time;
		positionData.Position = _position;
		positionData.Velocity = _velocity;
	}

	// Token: 0x06002CF5 RID: 11509 RVA: 0x000D4338 File Offset: 0x000D2738
	public Vector3 GetChefPosition(float _time)
	{
		float num = 0f;
		int num2 = 0;
		bool flag = false;
		for (int i = 0; i < 100; i++)
		{
			float time = this.m_PositionHistory[i].Time;
			if (time < _time && time > num)
			{
				num2 = i;
				num = time;
				flag = true;
			}
		}
		if (!flag)
		{
			return this.m_Rigidbody.position;
		}
		RemoteChefPositionRecorder.PositionData positionData = this.m_PositionHistory[num2];
		int num3 = num2 + 1;
		if (num3 == 100)
		{
			num3 = 0;
		}
		RemoteChefPositionRecorder.PositionData positionData2 = this.m_PositionHistory[num3];
		if (positionData2.Time < positionData.Time)
		{
			return positionData.Position + positionData.Velocity * (_time - positionData.Time);
		}
		float num4 = positionData2.Time - positionData.Time;
		float num5 = _time - positionData.Time;
		return Vector3.Lerp(positionData.Position, positionData2.Position, num5 / num4);
	}

	// Token: 0x04002418 RID: 9240
	private static List<RemoteChefPositionRecorder> ms_AllRemoteChefPositionRecorders = new List<RemoteChefPositionRecorder>();

	// Token: 0x04002419 RID: 9241
	private const int kPositionDataHistory = 100;

	// Token: 0x0400241A RID: 9242
	private RemoteChefPositionRecorder.PositionData[] m_PositionHistory = new RemoteChefPositionRecorder.PositionData[100];

	// Token: 0x0400241B RID: 9243
	private int m_CurrentIndex;

	// Token: 0x0400241C RID: 9244
	private Vector3 m_RestorePosition = default(Vector3);

	// Token: 0x0400241D RID: 9245
	private Vector3 m_RestoreVelocity = default(Vector3);

	// Token: 0x0400241E RID: 9246
	private Rigidbody m_Rigidbody;

	// Token: 0x02000900 RID: 2304
	private class PositionData
	{
		// Token: 0x06002CF7 RID: 11511 RVA: 0x000D443C File Offset: 0x000D283C
		public PositionData()
		{
			this.Time = 0f;
			this.Position = default(Vector3);
			this.Velocity = default(Vector3);
		}

		// Token: 0x0400241F RID: 9247
		public float Time;

		// Token: 0x04002420 RID: 9248
		public Vector3 Position;

		// Token: 0x04002421 RID: 9249
		public Vector3 Velocity;
	}
}
