using System;
using UnityEngine;

// Token: 0x0200044E RID: 1102
public class KBatchedAnimTracker : MonoBehaviour
{
	// Token: 0x0600180B RID: 6155 RVA: 0x0007C1D4 File Offset: 0x0007A3D4
	private void Start()
	{
		if (this.controller == null)
		{
			Transform parent = base.transform.parent;
			while (parent != null)
			{
				this.controller = parent.GetComponent<KBatchedAnimController>();
				if (this.controller != null)
				{
					break;
				}
				parent = parent.parent;
			}
		}
		if (this.controller == null)
		{
			global::Debug.Log("Controller Null for tracker on " + base.gameObject.name, base.gameObject);
			base.enabled = false;
			return;
		}
		this.controller.onAnimEnter += this.OnAnimStart;
		this.controller.onAnimComplete += this.OnAnimStop;
		this.controller.onLayerChanged += this.OnLayerChanged;
		this.forceUpdate = true;
		if (this.myAnim != null)
		{
			return;
		}
		this.myAnim = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController kbatchedAnimController = this.myAnim;
		kbatchedAnimController.getPositionDataFunctionInUse = (Func<Vector4>)Delegate.Combine(kbatchedAnimController.getPositionDataFunctionInUse, new Func<Vector4>(this.MyAnimGetPosition));
	}

	// Token: 0x0600180C RID: 6156 RVA: 0x0007C2EC File Offset: 0x0007A4EC
	private Vector4 MyAnimGetPosition()
	{
		if (this.controller.transform == this.myAnim.transform.parent)
		{
			Vector3 pivotSymbolPosition = this.myAnim.GetPivotSymbolPosition();
			return new Vector4(pivotSymbolPosition.x - this.controller.Offset.x, pivotSymbolPosition.y - this.controller.Offset.y, pivotSymbolPosition.x, pivotSymbolPosition.y);
		}
		return base.transform.GetPosition();
	}

	// Token: 0x0600180D RID: 6157 RVA: 0x0007C378 File Offset: 0x0007A578
	private void OnDestroy()
	{
		if (this.controller != null)
		{
			this.controller.onAnimEnter -= this.OnAnimStart;
			this.controller.onAnimComplete -= this.OnAnimStop;
			this.controller.onLayerChanged -= this.OnLayerChanged;
			this.controller = null;
		}
		if (this.myAnim != null)
		{
			KBatchedAnimController kbatchedAnimController = this.myAnim;
			kbatchedAnimController.getPositionDataFunctionInUse = (Func<Vector4>)Delegate.Remove(kbatchedAnimController.getPositionDataFunctionInUse, new Func<Vector4>(this.MyAnimGetPosition));
		}
		this.myAnim = null;
	}

	// Token: 0x0600180E RID: 6158 RVA: 0x0007C41C File Offset: 0x0007A61C
	private void LateUpdate()
	{
		if (this.controller != null && (this.controller.IsVisible() || this.forceAlwaysVisible || this.forceUpdate))
		{
			this.UpdateFrame();
		}
		if (!this.alive)
		{
			base.enabled = false;
		}
	}

	// Token: 0x0600180F RID: 6159 RVA: 0x0007C469 File Offset: 0x0007A669
	public void SetAnimControllers(KBatchedAnimController controller, KBatchedAnimController parentController)
	{
		this.myAnim = controller;
		this.controller = parentController;
	}

	// Token: 0x06001810 RID: 6160 RVA: 0x0007C47C File Offset: 0x0007A67C
	private void UpdateFrame()
	{
		this.forceUpdate = false;
		bool flag = false;
		if (this.controller.CurrentAnim != null)
		{
			Matrix2x3 symbolLocalTransform = this.controller.GetSymbolLocalTransform(this.symbol, out flag);
			Vector3 position = this.controller.transform.GetPosition();
			if (flag && (this.previousMatrix != symbolLocalTransform || position != this.previousPosition || (this.useTargetPoint && this.targetPoint != this.previousTargetPoint) || (this.matchParentOffset && this.myAnim.Offset != this.controller.Offset)))
			{
				this.previousMatrix = symbolLocalTransform;
				this.previousPosition = position;
				Matrix2x3 overrideTransformMatrix = ((this.useTargetPoint || this.myAnim == null) ? this.controller.GetTransformMatrix() : this.controller.GetTransformMatrix(new Vector2(this.myAnim.animWidth * this.myAnim.animScale, -this.myAnim.animHeight * this.myAnim.animScale))) * symbolLocalTransform;
				float z = base.transform.GetPosition().z;
				base.transform.SetPosition(overrideTransformMatrix.MultiplyPoint(this.offset));
				if (this.useTargetPoint)
				{
					this.previousTargetPoint = this.targetPoint;
					Vector3 position2 = base.transform.GetPosition();
					position2.z = 0f;
					Vector3 vector = this.targetPoint - position2;
					float num = Vector3.Angle(vector, Vector3.right);
					if (vector.y < 0f)
					{
						num = 360f - num;
					}
					base.transform.localRotation = Quaternion.identity;
					base.transform.RotateAround(position2, new Vector3(0f, 0f, 1f), num);
					float sqrMagnitude = vector.sqrMagnitude;
					this.myAnim.GetBatchInstanceData().SetClipRadius(base.transform.GetPosition().x, base.transform.GetPosition().y, sqrMagnitude, true);
				}
				else
				{
					Vector3 v = this.controller.FlipX ? Vector3.left : Vector3.right;
					Vector3 v2 = this.controller.FlipY ? Vector3.down : Vector3.up;
					base.transform.up = overrideTransformMatrix.MultiplyVector(v2);
					base.transform.right = overrideTransformMatrix.MultiplyVector(v);
					if (this.myAnim != null)
					{
						KBatchedAnimInstanceData batchInstanceData = this.myAnim.GetBatchInstanceData();
						if (batchInstanceData != null)
						{
							batchInstanceData.SetOverrideTransformMatrix(overrideTransformMatrix);
						}
					}
				}
				base.transform.SetPosition(new Vector3(base.transform.GetPosition().x, base.transform.GetPosition().y, z));
				if (this.matchParentOffset)
				{
					this.myAnim.Offset = this.controller.Offset;
				}
				this.myAnim.SetDirty();
			}
		}
		if (this.myAnim != null && flag != this.myAnim.enabled)
		{
			this.myAnim.enabled = flag;
		}
	}

	// Token: 0x06001811 RID: 6161 RVA: 0x0007C7B6 File Offset: 0x0007A9B6
	[ContextMenu("ForceAlive")]
	private void OnAnimStart(HashedString name)
	{
		this.alive = true;
		base.enabled = true;
		this.forceUpdate = true;
	}

	// Token: 0x06001812 RID: 6162 RVA: 0x0007C7CD File Offset: 0x0007A9CD
	private void OnAnimStop(HashedString name)
	{
		if (!this.forceAlwaysAlive)
		{
			this.alive = false;
		}
	}

	// Token: 0x06001813 RID: 6163 RVA: 0x0007C7DE File Offset: 0x0007A9DE
	private void OnLayerChanged(int layer)
	{
		this.myAnim.SetLayer(layer);
	}

	// Token: 0x06001814 RID: 6164 RVA: 0x0007C7EC File Offset: 0x0007A9EC
	public void SetTarget(Vector3 target)
	{
		this.targetPoint = target;
		this.targetPoint.z = 0f;
	}

	// Token: 0x04000D41 RID: 3393
	public KBatchedAnimController controller;

	// Token: 0x04000D42 RID: 3394
	public Vector3 offset = Vector3.zero;

	// Token: 0x04000D43 RID: 3395
	public HashedString symbol;

	// Token: 0x04000D44 RID: 3396
	public Vector3 targetPoint = Vector3.zero;

	// Token: 0x04000D45 RID: 3397
	public Vector3 previousTargetPoint;

	// Token: 0x04000D46 RID: 3398
	public bool useTargetPoint;

	// Token: 0x04000D47 RID: 3399
	public bool fadeOut = true;

	// Token: 0x04000D48 RID: 3400
	public bool forceAlwaysVisible;

	// Token: 0x04000D49 RID: 3401
	public bool matchParentOffset;

	// Token: 0x04000D4A RID: 3402
	public bool forceAlwaysAlive;

	// Token: 0x04000D4B RID: 3403
	private bool alive = true;

	// Token: 0x04000D4C RID: 3404
	private bool forceUpdate;

	// Token: 0x04000D4D RID: 3405
	private Matrix2x3 previousMatrix;

	// Token: 0x04000D4E RID: 3406
	private Vector3 previousPosition;

	// Token: 0x04000D4F RID: 3407
	[SerializeField]
	private KBatchedAnimController myAnim;
}
