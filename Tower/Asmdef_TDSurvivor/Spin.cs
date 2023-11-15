using System;
using UnityEngine;

// Token: 0x020001A6 RID: 422
public class Spin : MonoBehaviour
{
	// Token: 0x06000B62 RID: 2914 RVA: 0x0002C79F File Offset: 0x0002A99F
	private void Start()
	{
		this.mTrans = base.transform;
	}

	// Token: 0x06000B63 RID: 2915 RVA: 0x0002C7AD File Offset: 0x0002A9AD
	private void Update()
	{
		if (this.mRb == null)
		{
			this.ApplyDelta(this.ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime);
		}
	}

	// Token: 0x06000B64 RID: 2916 RVA: 0x0002C7D7 File Offset: 0x0002A9D7
	private void FixedUpdate()
	{
	}

	// Token: 0x06000B65 RID: 2917 RVA: 0x0002C7DC File Offset: 0x0002A9DC
	public void ApplyDelta(float delta)
	{
		delta *= 360f;
		Quaternion rhs = Quaternion.Euler(this.rotationsPerSecond * delta);
		if (this.mRb == null)
		{
			if (this.isLocalCoordinate)
			{
				this.mTrans.localRotation = this.mTrans.localRotation * rhs;
				return;
			}
			this.mTrans.rotation = this.mTrans.rotation * rhs;
			return;
		}
		else
		{
			if (this.isLocalCoordinate)
			{
				this.mRb.MoveRotation(this.mTrans.localRotation * rhs);
				return;
			}
			this.mRb.MoveRotation(this.mTrans.rotation * rhs);
			return;
		}
	}

	// Token: 0x04000913 RID: 2323
	[Header("旋轉速度 (圈數/每秒)")]
	public Vector3 rotationsPerSecond = new Vector3(0f, 0f, 0f);

	// Token: 0x04000914 RID: 2324
	[Header("是否受遊戲速度影響 (除非特殊狀況不然都不勾)")]
	public bool ignoreTimeScale;

	// Token: 0x04000915 RID: 2325
	[Header("是不是Local座標 (不勾的話就用世界座標)")]
	public bool isLocalCoordinate;

	// Token: 0x04000916 RID: 2326
	private Rigidbody mRb;

	// Token: 0x04000917 RID: 2327
	private Transform mTrans;
}
