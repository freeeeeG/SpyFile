using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008FB RID: 2299
public class PositionRecorder : MonoBehaviour
{
	// Token: 0x06002CD0 RID: 11472 RVA: 0x000D3357 File Offset: 0x000D1757
	private void Awake()
	{
		this.m_Transform = base.transform;
		this.m_RigidBody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06002CD1 RID: 11473 RVA: 0x000D3374 File Offset: 0x000D1774
	public void TakeSample(float _deltaTime, Transform _currentParent, float _serverPositionTime)
	{
		PositionRecorder.MovementDelta movementDelta = this.m_MovementHistory[this.m_CurrentMovementPosition++];
		movementDelta.NetworkTime = Time.time;
		if (this.m_PreviousMovementDelta != null)
		{
			float networkTime = this.m_PreviousMovementDelta.NetworkTime;
		}
		else
		{
			float networkTime = movementDelta.NetworkTime;
		}
		movementDelta.FrameTime = _deltaTime;
		Vector3 vector = this.CalculateLocalPosition(this.m_Transform.position, _currentParent);
		Vector3 position = this.m_Transform.position;
		if (_currentParent != this.m_ParentLastFrame)
		{
			this.m_PositionLastFrame = this.CalculateLocalPosition(this.m_PositionGlobalLastFrame, _currentParent);
		}
		movementDelta.PositionDelta = vector - this.m_PositionLastFrame - this.m_AdjustedAditionalVelocity * movementDelta.FrameTime;
		movementDelta.Parent = _currentParent;
		this.m_AdjustedAditionalVelocity.Set(0f, 0f, 0f);
		this.m_ParentLastFrame = _currentParent;
		this.m_PositionLastFrame = vector;
		this.m_PositionGlobalLastFrame = position;
		this.m_PreviousMovementDelta = movementDelta;
		if (this.m_CurrentMovementPosition == 100)
		{
			this.m_CurrentMovementPosition = 0;
		}
		movementDelta.OriginalDelta = movementDelta.PositionDelta;
		PositionRecorder.MovementDelta segment = this.parentCache.GetSegment();
		if (segment == null)
		{
			this.CleanSpaces(_serverPositionTime);
			segment = this.parentCache.GetSegment();
		}
		if (segment == null)
		{
			this.CleanOldest();
			segment = this.parentCache.GetSegment();
		}
		if (segment != null)
		{
			segment.CopyData(movementDelta);
			this.AddToQueue(segment);
		}
	}

	// Token: 0x06002CD2 RID: 11474 RVA: 0x000D34F8 File Offset: 0x000D18F8
	private bool NeedToAddNewSpace(Transform aTransform)
	{
		return this.mySpaces.Count == 0 || !(this.mySpaces[this.mySpaces.Count - 1].Parent == aTransform);
	}

	// Token: 0x06002CD3 RID: 11475 RVA: 0x000D3538 File Offset: 0x000D1938
	private bool DoPotentialSpaceTransition(PositionRecorder.MovementDelta delta)
	{
		if (this.NeedToAddNewSpace(delta.Parent))
		{
			if (this.mySpaces.Count != 0)
			{
				if (delta.Parent != null)
				{
					if (this.mySpaces[this.mySpaces.Count - 1].Parent != null)
					{
						this.mySpaces[this.mySpaces.Count - 1].exitTransform = delta.Parent.worldToLocalMatrix * this.mySpaces[this.mySpaces.Count - 1].Parent.worldToLocalMatrix.inverse;
					}
					else
					{
						this.mySpaces[this.mySpaces.Count - 1].exitTransform = delta.Parent.worldToLocalMatrix;
					}
				}
				else if (this.mySpaces[this.mySpaces.Count - 1].Parent != null)
				{
					this.mySpaces[this.mySpaces.Count - 1].exitTransform = this.mySpaces[this.mySpaces.Count - 1].Parent.worldToLocalMatrix.inverse;
				}
				else
				{
					this.mySpaces[this.mySpaces.Count - 1].exitTransform = Matrix4x4.identity;
				}
			}
			PositionRecorder.ParentSpace space = this.parentCache.GetSpace();
			space.Setup(delta.Parent);
			this.mySpaces.Add(space);
			space.Segments.Add(delta);
			return true;
		}
		return false;
	}

	// Token: 0x06002CD4 RID: 11476 RVA: 0x000D36F4 File Offset: 0x000D1AF4
	private void AddToQueue(PositionRecorder.MovementDelta delta)
	{
		if (!this.DoPotentialSpaceTransition(delta))
		{
			PositionRecorder.ParentSpace parentSpace = this.mySpaces[this.mySpaces.Count - 1];
			PositionRecorder.MovementDelta movementDelta = parentSpace.Segments[parentSpace.Segments.Count - 1];
			this.mySpaces[this.mySpaces.Count - 1].Segments.Add(delta);
		}
	}

	// Token: 0x06002CD5 RID: 11477 RVA: 0x000D3764 File Offset: 0x000D1B64
	public void Setup(Transform _currentParent)
	{
		this.m_bActive = true;
		this.m_PositionLastFrame = this.CalculateLocalPosition(this.m_Transform.position, _currentParent);
		this.m_PositionGlobalLastFrame = this.m_Transform.position;
		this.m_ParentLastFrame = _currentParent;
		this.m_CapsuleColliderHelper = base.gameObject.RequireComponent<CapsuleCollisionHelper>();
		for (int i = 0; i < 100; i++)
		{
			this.m_MovementHistory[i] = new PositionRecorder.MovementDelta();
		}
	}

	// Token: 0x06002CD6 RID: 11478 RVA: 0x000D37DC File Offset: 0x000D1BDC
	public void Clear(Transform _currentParent)
	{
		this.m_PositionLastFrame = this.CalculateLocalPosition(this.m_Transform.position, _currentParent);
		this.m_PositionGlobalLastFrame = this.m_Transform.position;
		this.m_ParentLastFrame = _currentParent;
		this.m_CurrentMovementPosition = 0;
		this.m_PreviousMovementDelta = null;
		this.m_AdjustedAditionalVelocity.Set(0f, 0f, 0f);
		for (int i = 0; i < 100; i++)
		{
			this.m_MovementHistory[i].Reset();
		}
	}

	// Token: 0x06002CD7 RID: 11479 RVA: 0x000D3864 File Offset: 0x000D1C64
	public Vector3 GetLagCompensatedPositionDelta(float serverPositionTime, Vector3 position)
	{
		Vector3 b = position;
		if (this.m_bActive)
		{
			bool option = DebugManager.Instance.GetOption("Chef VS Chef Prediction");
			this.m_CapsuleColliderHelper.UpdateCollisionMask(option);
			Vector3 position2 = this.m_RigidBody.position;
			Vector3 velocity = this.m_RigidBody.velocity;
			if (option)
			{
				this.m_RigidBody.position += Vector3.one * 100f;
				RemoteChefPositionRecorder.SetChefRestorePoint();
			}
			int num = this.m_CurrentMovementPosition;
			if (num < 0)
			{
				num = 100;
			}
			bool flag = true;
			int num2 = this.m_CurrentMovementPosition;
			while (num2 != num || flag)
			{
				flag = false;
				if (num2 == 100)
				{
					num2 = 0;
					if (num2 == num)
					{
						break;
					}
				}
				PositionRecorder.MovementDelta movementDelta = this.m_MovementHistory[num2];
				if (serverPositionTime <= movementDelta.NetworkTime)
				{
					float d = 1f;
					if (serverPositionTime > movementDelta.NetworkTime - movementDelta.FrameTime)
					{
						d = (movementDelta.NetworkTime - serverPositionTime) / movementDelta.FrameTime;
					}
					if (option)
					{
						RemoteChefPositionRecorder.SetRemoteChefsToTime(movementDelta.NetworkTime);
					}
					this.m_CapsuleColliderHelper.CastPositionForward(ref position, movementDelta.PositionDelta * d, option);
				}
				num2++;
			}
			if (option)
			{
				this.m_RigidBody.position = position2;
				this.m_RigidBody.velocity = velocity;
				RemoteChefPositionRecorder.RestoreChefsPositions();
			}
		}
		return position - b;
	}

	// Token: 0x06002CD8 RID: 11480 RVA: 0x000D39D8 File Offset: 0x000D1DD8
	public Vector3 GetLagCompensatedPositionDeltaParents(float serverPositionTime, Vector3 position, ref Vector3 globalPosition)
	{
		Vector3 b = position;
		if (this.m_bActive)
		{
			bool option = DebugManager.Instance.GetOption("Chef VS Chef Prediction");
			this.m_CapsuleColliderHelper.UpdateCollisionMask(option);
			Vector3 position2 = this.m_RigidBody.position;
			Vector3 velocity = this.m_RigidBody.velocity;
			if (option)
			{
				this.m_RigidBody.position += Vector3.one * 100f;
				RemoteChefPositionRecorder.SetChefRestorePoint();
			}
			this.CleanSpaces(serverPositionTime);
			Vector3 vector = position;
			Vector3 position3 = Vector3.zero;
			for (int i = 0; i < this.mySpaces.Count; i++)
			{
				PositionRecorder.ParentSpace parentSpace = this.mySpaces[i];
				Vector3 vector2;
				if (i == 0)
				{
					vector2 = vector;
				}
				else
				{
					vector2 = parentSpace.MeToWorld(position3);
				}
				position = vector2;
				for (int j = 0; j < parentSpace.Segments.Count; j++)
				{
					PositionRecorder.MovementDelta movementDelta = parentSpace.Segments[j];
					if (serverPositionTime <= movementDelta.NetworkTime)
					{
						float d = 1f;
						if (serverPositionTime > movementDelta.NetworkTime - movementDelta.FrameTime)
						{
							d = (movementDelta.NetworkTime - serverPositionTime) / movementDelta.FrameTime;
						}
						if (option)
						{
							RemoteChefPositionRecorder.SetRemoteChefsToTime(movementDelta.NetworkTime);
						}
						if (movementDelta.Parent == null)
						{
							this.m_CapsuleColliderHelper.CastPositionForward(ref position, movementDelta.PositionDelta * d, option);
						}
						else
						{
							this.m_CapsuleColliderHelper.CastPositionForward(ref position, movementDelta.Parent.localToWorldMatrix.rotation * movementDelta.PositionDelta * d, option);
						}
					}
				}
				Vector3 vector3 = parentSpace.FromWorldToMe(position);
				position3 = parentSpace.exitTransform * new Vector4(vector3.x, vector3.y, vector3.z, 1f);
			}
			if (option)
			{
				this.m_RigidBody.position = position2;
				this.m_RigidBody.velocity = velocity;
				RemoteChefPositionRecorder.RestoreChefsPositions();
			}
		}
		globalPosition = position;
		return position - b;
	}

	// Token: 0x06002CD9 RID: 11481 RVA: 0x000D3C0C File Offset: 0x000D200C
	private void CleanSpaces(float currentServerTime)
	{
		for (int i = this.mySpaces.Count - 1; i >= 0; i--)
		{
			PositionRecorder.ParentSpace parentSpace = this.mySpaces[i];
			for (int j = parentSpace.Segments.Count - 1; j >= 0; j--)
			{
				PositionRecorder.MovementDelta movementDelta = parentSpace.Segments[j];
				if (currentServerTime > movementDelta.NetworkTime)
				{
					this.parentCache.ReturnSegment(parentSpace.Segments[j]);
					parentSpace.Segments.RemoveAt(j);
				}
			}
			if (parentSpace.Segments.Count == 0)
			{
				this.parentCache.ReturnSpace(this.mySpaces[i]);
				this.mySpaces.RemoveAt(i);
			}
		}
	}

	// Token: 0x06002CDA RID: 11482 RVA: 0x000D3CD4 File Offset: 0x000D20D4
	private void CleanOldest()
	{
		if (this.mySpaces.Count > 0 && this.mySpaces[0].Segments.Count > 0)
		{
			this.parentCache.ReturnSegment(this.mySpaces[0].Segments[0]);
			this.mySpaces[0].Segments.RemoveAt(0);
			if (this.mySpaces[0].Segments.Count == 0)
			{
				this.parentCache.ReturnSpace(this.mySpaces[0]);
				this.mySpaces.RemoveAt(0);
			}
		}
	}

	// Token: 0x06002CDB RID: 11483 RVA: 0x000D3D85 File Offset: 0x000D2185
	public void AddAjustedVelocity(Vector3 additionalVelocity)
	{
		this.m_AdjustedAditionalVelocity += additionalVelocity;
	}

	// Token: 0x06002CDC RID: 11484 RVA: 0x000D3D9C File Offset: 0x000D219C
	public void Teleport(Vector3 newGlobalPosition, Transform currentParent)
	{
		Vector3 velocity = this.m_RigidBody.velocity;
		this.m_Transform.position = newGlobalPosition;
		this.m_RigidBody.velocity = velocity;
		this.m_PositionGlobalLastFrame = this.m_Transform.position;
		this.m_PositionLastFrame = this.CalculateLocalPosition(this.m_Transform.position, currentParent);
	}

	// Token: 0x06002CDD RID: 11485 RVA: 0x000D3DF6 File Offset: 0x000D21F6
	public Vector3 CalculateLocalPosition(Vector3 _globalPosition, Transform _currentParent)
	{
		if (_currentParent != null)
		{
			return _currentParent.worldToLocalMatrix * new Vector4(_globalPosition.x, _globalPosition.y, _globalPosition.z, 1f);
		}
		return _globalPosition;
	}

	// Token: 0x04002400 RID: 9216
	private List<PositionRecorder.ParentSpace> mySpaces = new List<PositionRecorder.ParentSpace>();

	// Token: 0x04002401 RID: 9217
	private PositionRecorder.ParentSpaceCache parentCache = new PositionRecorder.ParentSpaceCache();

	// Token: 0x04002402 RID: 9218
	private bool m_bActive;

	// Token: 0x04002403 RID: 9219
	private const int kDeltaHistory = 100;

	// Token: 0x04002404 RID: 9220
	private PositionRecorder.MovementDelta[] m_MovementHistory = new PositionRecorder.MovementDelta[100];

	// Token: 0x04002405 RID: 9221
	private int m_CurrentMovementPosition;

	// Token: 0x04002406 RID: 9222
	private Vector3 m_PositionLastFrame = Vector3.zero;

	// Token: 0x04002407 RID: 9223
	private Vector3 m_PositionGlobalLastFrame = Vector3.zero;

	// Token: 0x04002408 RID: 9224
	private Transform m_ParentLastFrame;

	// Token: 0x04002409 RID: 9225
	private Transform m_Transform;

	// Token: 0x0400240A RID: 9226
	private Rigidbody m_RigidBody;

	// Token: 0x0400240B RID: 9227
	private PositionRecorder.MovementDelta m_PreviousMovementDelta;

	// Token: 0x0400240C RID: 9228
	private Vector3 m_AdjustedAditionalVelocity = Vector3.zero;

	// Token: 0x0400240D RID: 9229
	private CapsuleCollisionHelper m_CapsuleColliderHelper;

	// Token: 0x020008FC RID: 2300
	private class MovementDelta
	{
		// Token: 0x06002CDE RID: 11486 RVA: 0x000D3E38 File Offset: 0x000D2238
		public MovementDelta(PositionRecorder.MovementDelta delta)
		{
			this.PositionDelta = delta.PositionDelta;
			this.FrameTime = delta.FrameTime;
			this.NetworkTime = delta.NetworkTime;
			this.Parent = delta.Parent;
			this.OriginalDelta = delta.OriginalDelta;
		}

		// Token: 0x06002CDF RID: 11487 RVA: 0x000D3EB0 File Offset: 0x000D22B0
		public MovementDelta()
		{
			this.Reset();
		}

		// Token: 0x06002CE0 RID: 11488 RVA: 0x000D3EF2 File Offset: 0x000D22F2
		public void CopyData(PositionRecorder.MovementDelta delta)
		{
			this.PositionDelta = delta.PositionDelta;
			this.FrameTime = delta.FrameTime;
			this.NetworkTime = delta.NetworkTime;
			this.Parent = delta.Parent;
			this.OriginalDelta = delta.OriginalDelta;
		}

		// Token: 0x06002CE1 RID: 11489 RVA: 0x000D3F30 File Offset: 0x000D2330
		public void Reset()
		{
			this.NetworkTime = float.MinValue;
			this.FrameTime = 0f;
			this.PositionDelta.Set(0f, 0f, 0f);
			this.OriginalDelta.Set(0f, 0f, 0f);
			this.Parent = null;
		}

		// Token: 0x0400240E RID: 9230
		public Vector3 PositionDelta = default(Vector3);

		// Token: 0x0400240F RID: 9231
		public Vector3 OriginalDelta = default(Vector3);

		// Token: 0x04002410 RID: 9232
		public float FrameTime;

		// Token: 0x04002411 RID: 9233
		public float NetworkTime = float.MinValue;

		// Token: 0x04002412 RID: 9234
		public Transform Parent;
	}

	// Token: 0x020008FD RID: 2301
	private class ParentSpace
	{
		// Token: 0x06002CE3 RID: 11491 RVA: 0x000D3FAC File Offset: 0x000D23AC
		public Vector3 FromWorldToMe(Vector3 position)
		{
			if (this.Parent == null)
			{
				return position;
			}
			return this.Parent.worldToLocalMatrix * new Vector4(position.x, position.y, position.z, 1f);
		}

		// Token: 0x06002CE4 RID: 11492 RVA: 0x000D4000 File Offset: 0x000D2400
		public Vector3 MeToWorld(Vector3 position)
		{
			if (this.Parent == null)
			{
				return position;
			}
			return this.Parent.localToWorldMatrix * new Vector4(position.x, position.y, position.z, 1f);
		}

		// Token: 0x06002CE5 RID: 11493 RVA: 0x000D4054 File Offset: 0x000D2454
		public void Setup(Transform parent)
		{
			this.Parent = parent;
			this.Segments.Clear();
			this.exitTransform = Matrix4x4.identity;
		}

		// Token: 0x04002413 RID: 9235
		public Transform Parent;

		// Token: 0x04002414 RID: 9236
		public List<PositionRecorder.MovementDelta> Segments = new List<PositionRecorder.MovementDelta>();

		// Token: 0x04002415 RID: 9237
		public Matrix4x4 exitTransform = Matrix4x4.identity;
	}

	// Token: 0x020008FE RID: 2302
	private class ParentSpaceCache
	{
		// Token: 0x06002CE6 RID: 11494 RVA: 0x000D4074 File Offset: 0x000D2474
		public ParentSpaceCache()
		{
			for (int i = 0; i < 100; i++)
			{
				this.segments.Push(new PositionRecorder.MovementDelta());
			}
			for (int j = 0; j < 5; j++)
			{
				this.parentSpaces.Push(new PositionRecorder.ParentSpace());
			}
		}

		// Token: 0x06002CE7 RID: 11495 RVA: 0x000D40E2 File Offset: 0x000D24E2
		public PositionRecorder.ParentSpace GetSpace()
		{
			if (this.parentSpaces.Count == 0)
			{
				this.parentSpaces.Push(new PositionRecorder.ParentSpace());
			}
			return this.parentSpaces.Pop();
		}

		// Token: 0x06002CE8 RID: 11496 RVA: 0x000D410F File Offset: 0x000D250F
		public void ReturnSpace(PositionRecorder.ParentSpace space)
		{
			this.parentSpaces.Push(space);
		}

		// Token: 0x06002CE9 RID: 11497 RVA: 0x000D411D File Offset: 0x000D251D
		public PositionRecorder.MovementDelta GetSegment()
		{
			if (this.segments.Count == 0)
			{
				return null;
			}
			return this.segments.Pop();
		}

		// Token: 0x06002CEA RID: 11498 RVA: 0x000D413C File Offset: 0x000D253C
		public void ReturnSegment(PositionRecorder.MovementDelta delta)
		{
			this.segments.Push(delta);
		}

		// Token: 0x04002416 RID: 9238
		private Stack<PositionRecorder.ParentSpace> parentSpaces = new Stack<PositionRecorder.ParentSpace>();

		// Token: 0x04002417 RID: 9239
		private Stack<PositionRecorder.MovementDelta> segments = new Stack<PositionRecorder.MovementDelta>();
	}
}
