using System;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x0200090B RID: 2315
	public class ClientChefSynchroniser : ClientWorldObjectSynchroniser
	{
		// Token: 0x06002D4B RID: 11595 RVA: 0x000D6201 File Offset: 0x000D4601
		public Vector3 GetDesiredPosition()
		{
			return this.m_DesiredPosition;
		}

		// Token: 0x06002D4C RID: 11596 RVA: 0x000D620C File Offset: 0x000D460C
		public Vector3 GetServerGlobalPosition()
		{
			if (this.m_DisableDynamicReparenting)
			{
				if (this.m_Transform.parent != null)
				{
					return this.m_Transform.parent.position + this.m_Transform.parent.rotation * this.m_ServerLocalPosition;
				}
			}
			else if (this.m_ServerParent != null)
			{
				return this.m_ServerParent.transform.position + this.m_ServerParent.transform.rotation * this.m_ServerLocalPosition;
			}
			return this.m_ServerLocalPosition;
		}

		// Token: 0x06002D4D RID: 11597 RVA: 0x000D62B8 File Offset: 0x000D46B8
		public Vector3 GetServerVelocity()
		{
			return this.m_ServerVelocity;
		}

		// Token: 0x06002D4E RID: 11598 RVA: 0x000D62C0 File Offset: 0x000D46C0
		protected Vector3 GetLastServerGlobalPosition()
		{
			if (this.m_DisableDynamicReparenting)
			{
				if (this.m_Transform.parent != null)
				{
					return MathUtils.MultiplyByMatrix(this.m_Transform.parent.localToWorldMatrix, this.m_ServerLastLocalPosition);
				}
			}
			else if (this.m_ServerLastParent != null)
			{
				return MathUtils.MultiplyByMatrix(this.m_ServerLastParent.localToWorldMatrix, this.m_ServerLastLocalPosition);
			}
			return this.m_ServerLastLocalPosition;
		}

		// Token: 0x06002D4F RID: 11599 RVA: 0x000D6340 File Offset: 0x000D4740
		public override void Awake()
		{
			base.Awake();
			this.m_DisableDynamicReparenting = GameUtils.GetLevelConfig().m_disableDynamicParenting;
			Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
			this.m_Rigidbody = base.GetComponent<Rigidbody>();
			this.m_MultiPlayerController = GameUtils.RequireManager<MultiplayerController>();
		}

		// Token: 0x06002D50 RID: 11600 RVA: 0x000D6392 File Offset: 0x000D4792
		protected override void OnDestroy()
		{
			Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
			base.OnDestroy();
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x000D63B4 File Offset: 0x000D47B4
		private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
		{
			GameStateMessage gameStateMessage = (GameStateMessage)message;
			if (gameStateMessage.m_State == GameState.StartEntities)
			{
				this.ChefSetup();
				if (this.m_bLocallyControlled)
				{
					this.m_ChefLerp = this.m_Transform.Find("Chef").gameObject.AddComponent<ChefLerp>();
					this.m_ChefLerp.Setup(this.m_Rigidbody);
				}
				else
				{
					this.m_Lerper = base.gameObject.AddComponent<BasicLerp>();
					this.m_Lerper.StartSynchronising(this.GetSynchronisedComponent());
				}
			}
			if (gameStateMessage.m_State == GameState.InLevel)
			{
				this.m_Go = true;
			}
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x000D6454 File Offset: 0x000D4854
		private void ChefSetup()
		{
			PlayerIDProvider playerIDProvider = base.gameObject.RequireComponent<PlayerIDProvider>();
			this.m_bLocallyControlled = playerIDProvider.IsLocallyControlled();
			if (this.m_bLocallyControlled)
			{
				this.m_CapsuleCollision = base.gameObject.AddComponent<CapsuleCollisionHelper>();
				this.m_PositionRecorder = base.gameObject.RequireComponent<PositionRecorder>();
				this.m_PlayerControls = base.GetComponent<PlayerControls>();
				this.m_ControlScheme = this.m_PlayerControls.ControlScheme;
				this.m_PositionRecorder.Setup(this.GetLocalCurrentParent());
			}
			else
			{
				this.m_PositionRecorder = null;
				this.m_RemoteChefPositionRecorder = base.gameObject.AddComponent<RemoteChefPositionRecorder>();
				this.m_Rigidbody.isKinematic = true;
			}
		}

		// Token: 0x06002D53 RID: 11603 RVA: 0x000D64FD File Offset: 0x000D48FD
		private Transform GetLocalCurrentParent()
		{
			if (this.m_DisableDynamicReparenting)
			{
				return this.m_Transform.parent;
			}
			return this.m_LocalCurrentParent;
		}

		// Token: 0x06002D54 RID: 11604 RVA: 0x000D651C File Offset: 0x000D491C
		private void OnGroundChanged(Collider _collider)
		{
			if (_collider != null)
			{
				IParentable parentable = _collider.gameObject.RequestInterfaceUpwardsRecursive<IParentable>();
				if (parentable != null)
				{
					this.m_LocalCurrentParent = parentable.GetAttachPoint(base.gameObject);
				}
				else
				{
					this.m_LocalCurrentParent = null;
				}
			}
			else
			{
				this.m_LocalCurrentParent = null;
			}
		}

		// Token: 0x06002D55 RID: 11605 RVA: 0x000D6574 File Offset: 0x000D4974
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			ClientChefSynchroniser.BuildDebugString();
			if (!this.m_DisableDynamicReparenting)
			{
				GroundCast component = base.GetComponent<GroundCast>();
				component.RegisterGroundChangedCallback(new VoidGeneric<Collider>(this.OnGroundChanged));
				this.OnGroundChanged(component.GetGroundCollider());
			}
		}

		// Token: 0x06002D56 RID: 11606 RVA: 0x000D65BD File Offset: 0x000D49BD
		public override EntityType GetEntityType()
		{
			return EntityType.Chef;
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x000D65C0 File Offset: 0x000D49C0
		public override void ApplyServerUpdate(Serialisable serialisable)
		{
			this.HandleMessage((ChefPositionMessage)serialisable, false);
			this.m_bHasEverReceived = true;
		}

		// Token: 0x06002D58 RID: 11608 RVA: 0x000D65D6 File Offset: 0x000D49D6
		public override void ApplyServerEvent(Serialisable serialisable)
		{
			this.HandleMessage((ChefPositionMessage)serialisable, true);
			this.m_bHasEverReceived = true;
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x000D65EC File Offset: 0x000D49EC
		private void HandleMessage(ChefPositionMessage dataReceived, bool bSetStationary = false)
		{
			if (this.m_bLocallyControlled)
			{
				this.LocalChefReceiveData(dataReceived);
			}
			else if (!this.m_bPaused)
			{
				this.m_RemoteServerTime = dataReceived.NetworkTime;
				if (this.m_RemoteResumeTime <= this.m_RemoteServerTime)
				{
					if (bSetStationary)
					{
						this.m_ServerVelocity.Set(0f, 0f, 0f);
					}
					else
					{
						this.m_ServerVelocity = dataReceived.Velocity;
					}
					base.ApplyServerUpdate(dataReceived.WorldObject);
					if (this.m_RemoteChefPositionRecorder != null)
					{
						Vector3 vector = dataReceived.WorldObject.LocalPosition;
						if (this.m_Transform.parent != null)
						{
							vector += this.m_Transform.parent.position;
						}
						this.m_RemoteChefPositionRecorder.RecordData(Time.time, vector, this.m_Rigidbody.velocity);
					}
				}
			}
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x000D66DC File Offset: 0x000D4ADC
		private void LocalChefReceiveData(ChefPositionMessage dataReceived)
		{
			if (this.m_PositionRecorder != null && !this.m_bPaused)
			{
				WorldObjectMessage worldObject = dataReceived.WorldObject;
				this.m_ServerLastLocalPosition = this.m_ServerLocalPosition;
				this.m_ServerLocalPosition = worldObject.LocalPosition;
				this.m_ServerLastParent = this.m_ServerParent;
				if (!this.m_DisableDynamicReparenting)
				{
					base.ParentingLogic(worldObject);
					this.m_ServerParent = this.m_Transform.parent;
				}
				if (this.m_ServerParent != this.m_PreviousServerParent)
				{
					Matrix4x4 rhs = Matrix4x4.identity;
					Matrix4x4 lhs = Matrix4x4.identity;
					if (this.m_PreviousServerParent != null)
					{
						rhs = this.m_PreviousServerParent.localToWorldMatrix;
					}
					if (this.m_ServerParent != null)
					{
						lhs = this.m_ServerParent.worldToLocalMatrix;
					}
					this.m_QueuedServerLocalPosition = MathUtils.MultiplyByMatrix(lhs * rhs, this.m_QueuedServerLocalPosition);
					this.m_PreviousServerParent = this.m_ServerParent;
				}
				if (this.m_ServerParent != null)
				{
					this.m_ParentPreviousPosition = this.m_ServerParent.position;
					this.m_ParentPreviousRotation = this.m_ServerParent.rotation;
					this.m_QueuedServerGlobalPosition = MathUtils.MultiplyByMatrix(this.m_ServerParent.localToWorldMatrix, this.m_QueuedServerLocalPosition);
				}
				this.m_ServerLastGlobalPosition = this.m_QueuedServerGlobalPosition;
				this.m_ServerGlobalPosition = this.GetServerGlobalPosition();
				this.m_ServerTime = dataReceived.NetworkTime;
				this.m_LastReceivedTimeStamp = this.m_QueuedReceivedTimeStamp;
				this.m_ReceivedTimeStamp = dataReceived.ClientTimeStamp;
				this.m_bParentHasMoved = false;
			}
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000D6868 File Offset: 0x000D4C68
		public override void OnResumeDataReceived(Serialisable _data)
		{
			ChefPositionMessage other = (ChefPositionMessage)_data;
			ChefPositionMessage chefPositionMessage = new ChefPositionMessage();
			chefPositionMessage.Copy(other);
			this.m_PendingResumeData = chefPositionMessage;
			this.m_bDoCorrection = true;
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000D6898 File Offset: 0x000D4C98
		protected override void ApplyResumeData(Serialisable _data)
		{
			ChefPositionMessage chefPositionMessage = (ChefPositionMessage)_data;
			this.m_bPaused = false;
			if (this.m_bLocallyControlled)
			{
				this.LocalChefReceiveData(chefPositionMessage);
				if (this.m_PositionRecorder != null)
				{
					this.m_PositionRecorder.Clear(this.GetLocalCurrentParent());
					this.m_QueuedServerGlobalPosition = this.GetServerGlobalPosition();
					this.m_QueuedReceivedTimeStamp = chefPositionMessage.ClientTimeStamp;
					if (this.m_CapsuleCollision.CheckCapsule(this.m_QueuedServerGlobalPosition))
					{
					}
				}
			}
			else
			{
				this.m_RemoteResumeTime = chefPositionMessage.NetworkTime;
				this.m_RemoteServerTime = chefPositionMessage.NetworkTime;
				this.m_ServerVelocity.Set(0f, 0f, 0f);
				this.m_Rigidbody.velocity = this.m_ServerVelocity;
				base.ApplyResumeData(chefPositionMessage.WorldObject);
			}
			this.m_bDoCorrection = true;
			this.m_ResumeTime = Time.time;
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x000D697B File Offset: 0x000D4D7B
		public virtual void FixedUpdate()
		{
			if (this.m_bLocallyControlled)
			{
				this.RunCorrection();
			}
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000D6990 File Offset: 0x000D4D90
		private bool ControlsMovingPlayer()
		{
			return this.m_ControlScheme != null && this.m_PlayerControls != null && (this.m_ControlScheme.m_moveX.GetValue() != 0f || this.m_ControlScheme.m_moveY.GetValue() != 0f || this.m_PlayerControls.IsDashing());
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x000D6A00 File Offset: 0x000D4E00
		private void RunCorrection()
		{
			Transform localCurrentParent = this.GetLocalCurrentParent();
			if (!this.m_bDoCorrection)
			{
				if (this.ControlsMovingPlayer())
				{
					this.m_bDoCorrection = true;
				}
				else if (!this.m_bPaused)
				{
					if (!this.m_DisableDynamicReparenting)
					{
						this.m_Transform.parent = this.m_ServerParent;
					}
					this.m_Transform.localPosition = this.m_ServerLocalPosition;
					this.m_PositionRecorder.TakeSample(TimeManager.GetFixedDeltaTime(base.gameObject), localCurrentParent, this.m_ReceivedTimeStamp);
					return;
				}
			}
			if (this.m_Go && !this.m_bPaused && this.m_bLocallyControlled && this.m_PositionRecorder != null && this.m_ChefLerp != null && this.m_Transform != null && this.m_bHasEverReceived)
			{
				this.m_PositionRecorder.TakeSample(TimeManager.GetFixedDeltaTime(base.gameObject), localCurrentParent, this.m_ReceivedTimeStamp);
				if (this.m_ResumeTime <= this.m_ReceivedTimeStamp)
				{
					Vector3 serverGlobalPosition = this.GetServerGlobalPosition();
					if (this.m_ServerParent != null && ((this.m_ServerParent.position - this.m_ParentPreviousPosition).magnitude > 1E-45f || this.m_ServerParent.rotation != this.m_ParentPreviousRotation))
					{
						this.m_ParentPreviousPosition = this.m_ServerParent.position;
						this.m_ParentPreviousRotation = this.m_ServerParent.rotation;
						this.m_QueuedServerGlobalPosition = MathUtils.MultiplyByMatrix(this.m_ServerParent.localToWorldMatrix, this.m_QueuedServerLocalPosition);
						this.m_bParentHasMoved = true;
					}
					bool flag = this.m_CapsuleCollision.CheckCapsule(serverGlobalPosition);
					Vector3 vector = serverGlobalPosition;
					Vector3 b = this.m_PositionRecorder.CalculateLocalPosition(this.m_Transform.position, localCurrentParent);
					float num = this.m_ReceivedTimeStamp;
					Vector3 lastServerGlobalPosition = this.GetLastServerGlobalPosition();
					bool flag2 = false;
					if (this.m_bParentHasMoved)
					{
						flag2 = this.m_CapsuleCollision.CheckCapsule(lastServerGlobalPosition);
						if (flag2)
						{
							vector = lastServerGlobalPosition;
							num = this.m_ReceivedTimeStamp;
						}
					}
					if (!flag2)
					{
						float magnitude = (serverGlobalPosition - lastServerGlobalPosition).magnitude;
						float b2 = 0.03f / magnitude;
						float num2 = Mathf.Max(0.05f, b2);
						float num3 = Mathf.Max(1f - num2, 0.1f);
						while (flag && num3 >= 0f)
						{
							vector = lastServerGlobalPosition + num3 * (serverGlobalPosition - lastServerGlobalPosition);
							flag = this.m_CapsuleCollision.CheckCapsule(vector);
							num = this.m_LastReceivedTimeStamp + num3 * (this.m_ReceivedTimeStamp - this.m_LastReceivedTimeStamp);
							num2 *= 2f;
							num3 -= num2;
						}
						if (num3 < 0f)
						{
							num3 = 0f;
							vector = lastServerGlobalPosition + num3 * (serverGlobalPosition - lastServerGlobalPosition);
							flag = this.m_CapsuleCollision.CheckCapsule(vector);
							num = this.m_LastReceivedTimeStamp + num3 * (this.m_ReceivedTimeStamp - this.m_LastReceivedTimeStamp);
						}
						if (this.m_CapsuleCollision.CheckCapsule(vector))
						{
							vector = lastServerGlobalPosition + 1.05f * (serverGlobalPosition - lastServerGlobalPosition);
							num = this.m_LastReceivedTimeStamp + 1.05f * (this.m_ReceivedTimeStamp - this.m_LastReceivedTimeStamp);
							if (this.m_CapsuleCollision.CheckCapsule(vector))
							{
								vector = serverGlobalPosition;
								num = this.m_ReceivedTimeStamp;
							}
						}
					}
					this.m_QueuedServerGlobalPosition = vector;
					if (this.m_ServerParent != null)
					{
						this.m_QueuedServerLocalPosition = MathUtils.MultiplyByMatrix(this.m_ServerParent.worldToLocalMatrix, vector);
					}
					this.m_QueuedReceivedTimeStamp = num;
					if (flag)
					{
					}
					Vector3 lagCompensatedPositionDeltaParents = this.m_PositionRecorder.GetLagCompensatedPositionDeltaParents(num, vector, ref this.m_DesiredPosition);
					this.m_DesiredLocalPosition = this.m_PositionRecorder.CalculateLocalPosition(this.m_DesiredPosition, localCurrentParent);
					Quaternion rotation = Quaternion.identity;
					if (localCurrentParent != null)
					{
						rotation = localCurrentParent.localToWorldMatrix.rotation;
					}
					bool flag3 = this.m_CapsuleCollision.CheckCapsule(this.m_DesiredPosition);
					if (flag3)
					{
					}
					Vector3 a = this.m_DesiredLocalPosition - b;
					if (!this.m_bLerpRigidBody)
					{
						float y = a.y;
						float num4 = y * y;
						float x = a.x;
						float z = a.z;
						float num5 = x * x + z * z;
						RaycastHit[] array = Physics.SphereCastAll(this.m_Transform.position, 0.1f, this.m_Transform.up * -1f, 2f, 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("SlopedGround"));
						if (num4 > num5 + 0.1f && array.Length == 0)
						{
							this.m_bLerpRigidBody = true;
							this.m_Rigidbody.velocity = Vector3.zero;
							Transform transform;
							if (this.m_DisableDynamicReparenting)
							{
								transform = this.m_Transform.parent;
							}
							else
							{
								this.m_Transform.parent = this.m_ServerParent;
								transform = this.m_ServerParent;
							}
							if (transform != null)
							{
								this.m_DesiredPosition = MathUtils.MultiplyByMatrix(transform.localToWorldMatrix, this.m_ServerLocalPosition);
							}
							else
							{
								this.m_DesiredPosition = this.m_ServerLocalPosition;
							}
						}
						else if (a.sqrMagnitude > 6.25f && !flag3)
						{
							this.m_bLerpRigidBody = true;
							this.m_Rigidbody.velocity = Vector3.zero;
						}
						else
						{
							if (a.magnitude <= 0.1f && !this.ControlsMovingPlayer())
							{
								this.m_Rigidbody.velocity = Vector3.zero;
								this.m_bLerpRigidBody = true;
							}
							else
							{
								float num6 = Mathf.Lerp(1f, 2f, (a.magnitude - 1.25f) / 1.25f);
								Vector3 vector2 = ClientChefSynchroniser.CorrectionForce * num6 * a;
								this.m_Rigidbody.velocity += rotation * vector2;
								this.m_PositionRecorder.AddAjustedVelocity(vector2);
							}
							this.m_bLerpRigidBody = false;
							if (Vector3.Dot(this.m_Transform.forward, a.normalized) > -0.950002f && this.m_CapsuleCollision.CheckPathToPoint(this.m_Transform.position, this.m_DesiredPosition))
							{
								if (!flag3)
								{
									this.m_bLerpRigidBody = true;
								}
							}
						}
					}
					if (this.m_bLerpRigidBody)
					{
						this.m_PositionRecorder.Teleport(this.m_DesiredPosition, localCurrentParent);
						this.m_bDoCorrection = false;
						this.m_bLerpRigidBody = false;
					}
				}
			}
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x000D70C8 File Offset: 0x000D54C8
		public static void BuildDebugString()
		{
			ClientChefSynchroniser.DEBUGSTRING = ClientPhysicsObjectSynchroniser.GetDebugString();
		}

		// Token: 0x04002444 RID: 9284
		private Vector3 m_ServerVelocity = Vector3.zero;

		// Token: 0x04002445 RID: 9285
		private Rigidbody m_Rigidbody;

		// Token: 0x04002446 RID: 9286
		private PositionRecorder m_PositionRecorder;

		// Token: 0x04002447 RID: 9287
		private RemoteChefPositionRecorder m_RemoteChefPositionRecorder;

		// Token: 0x04002448 RID: 9288
		private bool m_bLocallyControlled;

		// Token: 0x04002449 RID: 9289
		private PlayerControls m_PlayerControls;

		// Token: 0x0400244A RID: 9290
		private PlayerControls.ControlSchemeData m_ControlScheme;

		// Token: 0x0400244B RID: 9291
		private ChefLerp m_ChefLerp;

		// Token: 0x0400244C RID: 9292
		private Vector3 m_ServerLocalPosition = default(Vector3);

		// Token: 0x0400244D RID: 9293
		private Vector3 m_ServerLastLocalPosition = default(Vector3);

		// Token: 0x0400244E RID: 9294
		private Vector3 m_ServerLastGlobalPosition = default(Vector3);

		// Token: 0x0400244F RID: 9295
		private Vector3 m_ServerTrueLastGlobalPosition = default(Vector3);

		// Token: 0x04002450 RID: 9296
		private Vector3 m_ServerGlobalPosition = default(Vector3);

		// Token: 0x04002451 RID: 9297
		private Vector3 m_QueuedServerGlobalPosition = default(Vector3);

		// Token: 0x04002452 RID: 9298
		private Vector3 m_QueuedServerLocalPosition = default(Vector3);

		// Token: 0x04002453 RID: 9299
		private Transform m_ServerParent;

		// Token: 0x04002454 RID: 9300
		private Vector3 m_ParentPreviousPosition = default(Vector3);

		// Token: 0x04002455 RID: 9301
		private Quaternion m_ParentPreviousRotation = default(Quaternion);

		// Token: 0x04002456 RID: 9302
		private Transform m_PreviousServerParent;

		// Token: 0x04002457 RID: 9303
		private float m_ServerTime;

		// Token: 0x04002458 RID: 9304
		private float m_ReceivedTimeStamp;

		// Token: 0x04002459 RID: 9305
		private float m_QueuedReceivedTimeStamp;

		// Token: 0x0400245A RID: 9306
		private float m_LastReceivedTimeStamp;

		// Token: 0x0400245B RID: 9307
		private float m_TrueLastReceivedTimeStamp;

		// Token: 0x0400245C RID: 9308
		private bool m_Go;

		// Token: 0x0400245D RID: 9309
		public CapsuleCollisionHelper m_CapsuleCollision;

		// Token: 0x0400245E RID: 9310
		public static float CorrectionForce = 4f;

		// Token: 0x0400245F RID: 9311
		private Vector3 m_DesiredPosition = default(Vector3);

		// Token: 0x04002460 RID: 9312
		private Vector3 m_DesiredLocalPosition = default(Vector3);

		// Token: 0x04002461 RID: 9313
		private bool m_bLerpRigidBody;

		// Token: 0x04002462 RID: 9314
		private MultiplayerController m_MultiPlayerController;

		// Token: 0x04002463 RID: 9315
		private bool m_DisableDynamicReparenting;

		// Token: 0x04002464 RID: 9316
		private float m_ResumeTime;

		// Token: 0x04002465 RID: 9317
		private float m_RemoteResumeTime;

		// Token: 0x04002466 RID: 9318
		public float m_RemoteServerTime;

		// Token: 0x04002467 RID: 9319
		private Transform m_ServerLastParent;

		// Token: 0x04002468 RID: 9320
		private bool m_bParentHasMoved;

		// Token: 0x04002469 RID: 9321
		private bool m_bDoCorrection = true;

		// Token: 0x0400246A RID: 9322
		private Transform m_LocalCurrentParent;

		// Token: 0x0400246B RID: 9323
		public static string DEBUGSTRING = string.Empty;
	}
}
