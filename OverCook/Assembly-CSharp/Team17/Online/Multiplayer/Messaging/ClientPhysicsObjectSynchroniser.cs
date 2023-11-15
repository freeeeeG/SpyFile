using System;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x0200091D RID: 2333
	public class ClientPhysicsObjectSynchroniser : ClientWorldObjectSynchroniser
	{
		// Token: 0x06002DBD RID: 11709 RVA: 0x000D9258 File Offset: 0x000D7658
		public override void Awake()
		{
			base.Awake();
			if (ClientPhysicsObjectSynchroniser.Tweekables == null)
			{
				ClientPhysicsObjectSynchroniser.Tweekables = GameUtils.RequireManager<MultiplayerController>().m_NetworkPredictionTweekables;
			}
			this.m_MultiplayerController = GameUtils.RequireManager<MultiplayerController>();
			this.m_PlayerLayer = LayerMask.NameToLayer("Players");
			for (int i = 0; i < this.m_ContactingChefs.Length; i++)
			{
				ClientPhysicsObjectSynchroniser.ContactingChef contactingChef = new ClientPhysicsObjectSynchroniser.ContactingChef();
				contactingChef.EntityID = 0U;
				contactingChef.ChefGameObject = null;
				contactingChef.LocallyControlled = false;
				this.m_ContactingChefs[i] = contactingChef;
			}
			this.m_RigidBodyMotion = base.GetComponent<RigidbodyMotion>();
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x000D92EE File Offset: 0x000D76EE
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x000D92F8 File Offset: 0x000D76F8
		public override void StartSynchronising(Component synchronisedObject)
		{
			base.StartSynchronising(synchronisedObject);
			this.m_PhysicalAttachment = ((PhysicsObjectSynchroniser)synchronisedObject).GetPhysicalAttachment();
			this.m_Colliders = this.m_PhysicalAttachment.GetComponentsInChildren<Collider>(true);
			for (int i = 0; i < this.m_Colliders.Length; i++)
			{
				if (this.m_Colliders[i].isTrigger)
				{
					this.m_Colliders[i] = null;
				}
			}
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x000D9363 File Offset: 0x000D7763
		public override EntityType GetEntityType()
		{
			return EntityType.PhysicsObject;
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x000D9368 File Offset: 0x000D7768
		public override void ApplyServerUpdate(Serialisable serialisable)
		{
			PhysicsObjectMessage physicsObjectMessage = (PhysicsObjectMessage)serialisable;
			base.ApplyServerUpdate(physicsObjectMessage.WorldObject);
			this.ReceiveServerUpdate(physicsObjectMessage);
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x000D9390 File Offset: 0x000D7790
		public override void ApplyServerEvent(Serialisable serialisable)
		{
			PhysicsObjectMessage physicsObjectMessage = (PhysicsObjectMessage)serialisable;
			base.ApplyServerEvent(physicsObjectMessage.WorldObject);
			this.ReceiveServerUpdate(physicsObjectMessage);
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000D93B8 File Offset: 0x000D77B8
		private void ReceiveServerUpdate(PhysicsObjectMessage receivedData)
		{
			this.m_ServerLocalPosition = receivedData.WorldObject.LocalPosition;
			this.m_ServerLocalRotation = receivedData.WorldObject.LocalRotation;
			this.m_TimeLastMessageReceived = Time.time;
			if (this.m_bFirstNetworkMessage && this.m_meshLerper != null)
			{
				this.m_bFirstNetworkMessage = false;
				this.m_meshLerper.SnapToTarget(MeshLerper.Target.ServerPosition);
			}
			this.m_ContactChefCount = (int)receivedData.ContactCount;
			for (int i = 0; i < this.m_ContactChefCount; i++)
			{
				uint num = receivedData.Contacts[i];
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(num);
				ClientPhysicsObjectSynchroniser.ContactingChef contactingChef = this.m_ContactingChefs[i];
				if (entry != null)
				{
					contactingChef.EntityID = num;
					contactingChef.ChefGameObject = entry.m_GameObject;
					contactingChef.ChefPositionRecorder = entry.m_GameObject.GetComponent<PositionRecorder>();
					contactingChef.ChefSynchroniser = entry.m_GameObject.GetComponent<ClientChefSynchroniser>();
					contactingChef.ServerContactTime = receivedData.ContactTimes[i];
					contactingChef.RelativePosition = receivedData.RelativePositions[i];
					contactingChef.ContactVelocity = receivedData.ContactVelocitys[i];
					contactingChef.LocallyControlled = entry.m_GameObject.GetComponent<PlayerIDProvider>().IsLocallyControlled();
					if (contactingChef.LocallyControlled)
					{
						this.m_WaitingOnRemoteCollisionInfo = false;
					}
				}
			}
			this.m_ServerPreviousVelocity = this.m_ServerVelocity;
			this.m_ServerVelocity = receivedData.Velocity;
			if (!this.m_bPaused)
			{
				if (this.m_ContactChefCount > 0 && this.m_ChildAttachedTime + 0.1f >= Time.time)
				{
					this.m_SnapRigidbodyOnDataReceive = true;
					this.ChangeState(ClientPhysicsObjectSynchroniser.State.Line);
					this.m_TimeEnteredPreLineState = Time.time;
					this.m_meshLerper.SetTargets(MeshLerper.Target.CurrentPosition, MeshLerper.Target.Line, 0.012f);
				}
				if (this.m_SnapRigidbodyOnDataReceive && this.m_meshLerper != null)
				{
					float num2 = 0.5f;
					Vector3 vector = base.GetGlobalServerPosition();
					for (int j = 0; j < this.m_ContactChefCount; j++)
					{
						ClientPhysicsObjectSynchroniser.ContactingChef contactingChef2 = this.m_ContactingChefs[j];
						Vector3 position = contactingChef2.ChefGameObject.transform.position;
						if (Vector3.Distance(position, vector) < num2)
						{
							vector = position + (vector - position).normalized * num2;
							break;
						}
					}
					if (this.m_PendingSetKinematicState && this.m_CurrentState == ClientPhysicsObjectSynchroniser.State.ServerPosition)
					{
						this.m_PendingSetKinematicState = false;
						this.m_RigidBodyMotion.SetKinematic(true);
					}
					this.m_Transform.position = vector;
					this.m_Transform.localRotation = this.m_ServerLocalRotation;
					this.m_RigidBodyMotion.SetVelocity(this.m_ServerVelocity);
				}
				base.ParentingLogic(receivedData.WorldObject);
			}
			if (this.m_meshLerper != null)
			{
				this.m_meshLerper.SetServerPosition(base.GetGlobalServerPosition(), this.GetServerRotation());
			}
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x000D9694 File Offset: 0x000D7A94
		public override void Pause()
		{
			this.m_bPaused = true;
			base.Pause();
			if (this.m_meshLerper != null)
			{
				this.m_meshLerper.SetTargets(MeshLerper.Target.CurrentPosition, MeshLerper.Target.RigidBody, 0.2f);
			}
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x000D96C8 File Offset: 0x000D7AC8
		public override void OnResumeDataReceived(Serialisable _data)
		{
			PhysicsObjectMessage other = (PhysicsObjectMessage)_data;
			PhysicsObjectMessage physicsObjectMessage = new PhysicsObjectMessage();
			physicsObjectMessage.Copy(other);
			this.m_PendingResumeData = physicsObjectMessage;
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000D96F0 File Offset: 0x000D7AF0
		protected override void ApplyResumeData(Serialisable _data)
		{
			PhysicsObjectMessage physicsObjectMessage = (PhysicsObjectMessage)_data;
			this.m_bPaused = false;
			this.ChangeState(ClientPhysicsObjectSynchroniser.State.ServerPosition);
			this.m_SnapRigidbodyOnDataReceive = true;
			base.ApplyResumeData(physicsObjectMessage.WorldObject);
			this.ReceiveServerUpdate(physicsObjectMessage);
			if (this.m_meshLerper != null)
			{
				this.m_meshLerper.SnapToTarget(MeshLerper.Target.ServerPosition);
			}
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000D974C File Offset: 0x000D7B4C
		public virtual void FixedUpdate()
		{
			float fixedDeltaTime = Time.fixedDeltaTime;
			if (this.m_meshLerper == null && this.m_PhysicalAttachment != null)
			{
				this.m_meshLerper = this.m_PhysicalAttachment.m_meshLerper;
				if (this.m_meshLerper != null)
				{
					this.m_meshLerper.Initialise(this, base.GetComponent<Rigidbody>(), this.m_Transform, this.m_PhysicalAttachment);
					this.m_bFirstNetworkMessage = true;
				}
				if (this.m_ServerPositionTransform != null)
				{
					this.m_ServerPositionTransform.parent = this.m_Transform.parent;
				}
			}
			if (this.m_Transform != null && this.m_meshLerper != null)
			{
				this.RunChefRelativePrediction(fixedDeltaTime);
			}
			if (this.m_bPreviousFrameCollision && !this.m_CollidingWithLocalChef)
			{
				this.m_TimeLastLocalCollision = Time.time;
				this.m_TimeAnyCollision = this.m_TimeLastLocalCollision;
			}
			this.m_bPreviousFrameCollision = this.m_CollidingWithLocalChef;
			this.m_CollidingWithLocalChef = false;
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000D9858 File Offset: 0x000D7C58
		private void RunChefRelativePrediction(float _delta)
		{
			if (this.m_bPaused)
			{
				return;
			}
			float latencyTime = this.GetLatencyTime();
			float time = Time.time;
			float roundTripTime = this.GetRoundTripTime();
			Vector3 globalServerPosition = base.GetGlobalServerPosition();
			Vector3 globalServerPosition2 = base.GetGlobalServerPosition();
			bool flag = false;
			bool flag2 = false;
			Vector3 lhs = default(Vector3);
			float num = (!this.m_CollidingWithLocalChef) ? 0f : this.m_LocalCollidingChefRigidbody.velocity.magnitude;
			Vector3 vector = default(Vector3);
			Vector3 vector2 = default(Vector3);
			this.m_bRenderLineSphere = false;
			for (int i = 0; i < this.m_ContactChefCount; i++)
			{
				ClientPhysicsObjectSynchroniser.ContactingChef contactingChef = this.m_ContactingChefs[i];
				if (contactingChef.LocallyControlled)
				{
					this.m_TimeLastServerCollision = time;
					flag2 = true;
					Vector3 desiredPosition = contactingChef.ChefSynchroniser.GetDesiredPosition();
					lhs = contactingChef.ChefGameObject.GetComponent<Rigidbody>().velocity.normalized;
					Vector3 position = this.m_Transform.position;
					Vector3 forward = contactingChef.ChefGameObject.transform.forward;
					Vector3 relativePosition = contactingChef.RelativePosition;
					vector = relativePosition;
					vector2 = contactingChef.ContactVelocity;
					Vector3 input = default(Vector3);
					if (this.m_bNewLine)
					{
						input = contactingChef.RelativePosition + desiredPosition;
						this.m_PreviousRelativeTarget = contactingChef.RelativePosition;
					}
					else
					{
						input = Vector3.Lerp(this.m_PreviousRelativeTarget, contactingChef.RelativePosition, 0.5f) + desiredPosition;
						this.m_PreviousRelativeTarget = contactingChef.RelativePosition;
					}
					Vector2 a = position.XZ();
					Vector2 vector3 = relativePosition.XZ();
					Vector2 vector4 = input.XZ();
					Vector2 normalized = vector3.normalized;
					Vector2 b = vector4;
					Vector2 vector5 = a - b;
					float num2 = Vector3.Dot(forward, relativePosition.normalized);
					float num3 = (num2 <= 0f) ? 0f : (1f - num2);
					num3 *= latencyTime;
					float num4 = vector5.x * normalized.x + vector5.y * normalized.y;
					flag = (num4 < 0f);
					num4 = Mathf.Max(0f, num4);
					globalServerPosition = new Vector3(b.x - num3 * normalized.x, position.y, b.y - num3 * normalized.y);
					globalServerPosition2 = new Vector3(b.x + 0.1f * normalized.x, position.y, b.y + 0.1f * normalized.y);
					this.m_LinePosition = globalServerPosition;
					break;
				}
			}
			if (!flag2)
			{
				this.m_bNewLine = true;
				if (this.m_CurrentState == ClientPhysicsObjectSynchroniser.State.Line)
				{
					this.m_TimeAnyCollision = time;
				}
			}
			else if (!this.m_RemoteCollisionData)
			{
				this.m_TimeStartedReceivingCollision = time;
			}
			this.m_RemoteCollisionData = flag2;
			if (this.m_CurrentState == ClientPhysicsObjectSynchroniser.State.Line)
			{
				this.m_TimeinLine += _delta;
			}
			else
			{
				this.m_TimeinLine -= _delta;
			}
			this.m_TimeinLine = Mathf.Clamp(this.m_TimeinLine, 0f, 0.4f);
			this.m_RemoteCollisionConformationTimer += _delta;
			if (this.m_RemoteCollisionConformationTimer > 0.11f)
			{
				this.m_RecentlyCollided = false;
			}
			switch (this.m_CurrentState)
			{
			case ClientPhysicsObjectSynchroniser.State.ServerPosition:
				if (this.m_CollidersEnabled)
				{
					this.m_CollidersEnabled = false;
					for (int j = 0; j < this.m_ContactedChefColliders.Count; j++)
					{
						for (int k = 0; k < this.m_Colliders.Length; k++)
						{
							if (this.m_Colliders[k] != null)
							{
								Physics.IgnoreCollision(this.m_ContactedChefColliders[j], this.m_Colliders[k], false);
							}
						}
					}
					this.m_ContactedChefColliders.Clear();
				}
				this.m_SnapRigidbodyOnDataReceive = true;
				this.m_PenetrationOccured = false;
				if (this.m_CollidingWithLocalChef)
				{
					this.ChangeState(ClientPhysicsObjectSynchroniser.State.PreLine);
					this.m_TimeEnteredPreLineState = time;
					this.m_SnapRigidbodyOnDataReceive = false;
				}
				break;
			case ClientPhysicsObjectSynchroniser.State.PreLine:
			{
				Vector3 right = this.m_LocalCollidingChefRigidbody.transform.right;
				float num5 = Vector3.Dot(right, (this.m_meshLerper.transform.position - this.m_LocalCollidingChefRigidbody.position).normalized);
				float num6 = Vector3.Distance(this.m_LocalCollidingChefRigidbody.position, this.m_meshLerper.transform.position);
				bool flag3 = (num5 > ClientPhysicsObjectSynchroniser.Tweekables.PenetrationAngle || num5 < -ClientPhysicsObjectSynchroniser.Tweekables.PenetrationAngle) && num6 < 1f * ClientPhysicsObjectSynchroniser.Tweekables.ChefRadius;
				bool flag4 = num6 < 0.7f * ClientPhysicsObjectSynchroniser.Tweekables.ChefRadius;
				bool flag5 = num > ClientPhysicsObjectSynchroniser.Tweekables.PenitrationMinSpeed;
				bool flag6 = flag3 || flag4 || flag5;
				if (!this.m_PenetrationOccured && flag6 && num > 2.5f)
				{
					this.m_PenetrationOccured = true;
					this.m_meshLerper.SetTargets(MeshLerper.Target.ServerPosition, MeshLerper.Target.RigidBody, latencyTime * 3.5f);
				}
				if (flag2)
				{
					if (Vector3.Dot(vector2.normalized, this.m_ServerVelocity.normalized) > 0.8f)
					{
						float num7 = Vector3.Dot(lhs, vector.normalized);
						if (num7 >= 0.5f)
						{
							this.ChangeState(ClientPhysicsObjectSynchroniser.State.Line);
							this.m_meshLerper.SetTargets(MeshLerper.Target.CurrentPosition, MeshLerper.Target.Line, 0.2f);
							this.m_TimeEnteredLineState = time;
							this.m_SnapRigidbodyOnDataReceive = false;
						}
						else
						{
							this.ChangeState(ClientPhysicsObjectSynchroniser.State.ServerPosition);
							this.m_meshLerper.SetTargets(MeshLerper.Target.CurrentPosition, MeshLerper.Target.ServerPosition, 0.2f);
							this.m_SnapRigidbodyOnDataReceive = true;
						}
					}
					else
					{
						this.ChangeState(ClientPhysicsObjectSynchroniser.State.ServerPosition);
						this.m_meshLerper.SetTargets(MeshLerper.Target.CurrentPosition, MeshLerper.Target.ServerPosition, 0.2f);
						this.m_SnapRigidbodyOnDataReceive = true;
					}
				}
				else if (this.m_TimeEnteredPreLineState + roundTripTime * 2f < time)
				{
					this.m_SnapRigidbodyOnDataReceive = true;
					this.m_WaitingOnRemoteCollisionInfo = false;
					this.ChangeState(ClientPhysicsObjectSynchroniser.State.ServerPosition);
					this.m_meshLerper.SetTargets(MeshLerper.Target.CurrentPosition, MeshLerper.Target.ServerPosition, 0.2f);
					float d = Mathf.Min((this.m_Transform.localPosition - this.m_ServerLocalPosition).magnitude * 1f, 1f);
					this.m_Transform.localPosition = (this.m_ServerLocalPosition - this.m_Transform.localPosition) * d + this.m_Transform.localPosition;
					this.m_Transform.localRotation = this.m_ServerLocalRotation;
				}
				else
				{
					this.m_SnapRigidbodyOnDataReceive = false;
				}
				break;
			}
			case ClientPhysicsObjectSynchroniser.State.Line:
			{
				bool flag7 = false;
				if (!flag2)
				{
					flag7 = true;
				}
				else
				{
					this.m_SnapRigidbodyOnDataReceive = true;
					this.m_bRenderLineSphere = true;
					vector.y = 0f;
					lhs.y = 0f;
					float num8 = Vector3.Dot(lhs, vector.normalized);
					if (num8 < ClientPhysicsObjectSynchroniser.Tweekables.ChefMovingTowardsUsAngle)
					{
						flag7 = true;
					}
					if (!flag7)
					{
						if (flag)
						{
							Vector3 additionalVelocity = (globalServerPosition2 - this.m_Transform.position) * ClientPhysicsObjectSynchroniser.kCorrectiveForce;
							this.m_Transform.localRotation = this.m_ServerLocalRotation;
							this.m_RigidBodyMotion.AddVelocity(additionalVelocity);
						}
						else
						{
							Vector3 additionalVelocity2 = (globalServerPosition2 - this.m_Transform.position) * ClientPhysicsObjectSynchroniser.kCorrectiveForce;
							this.m_Transform.localRotation = this.m_ServerLocalRotation;
							this.m_RigidBodyMotion.AddVelocity(additionalVelocity2);
						}
					}
				}
				if (flag7)
				{
					this.m_TimeLeftLineState = time;
					if (this.m_TimeinLine < 0.2f)
					{
						this.ChangeState(ClientPhysicsObjectSynchroniser.State.ServerPosition);
						this.m_meshLerper.SetTargets(MeshLerper.Target.CurrentPosition, MeshLerper.Target.ServerPosition, 0.1f);
						this.m_SnapRigidbodyOnDataReceive = true;
					}
					else
					{
						this.ChangeState(ClientPhysicsObjectSynchroniser.State.PostLine);
						this.m_CollidersEnabled = true;
						for (int l = 0; l < this.m_ContactedChefColliders.Count; l++)
						{
							for (int m = 0; m < this.m_Colliders.Length; m++)
							{
								if (this.m_Colliders[m] != null)
								{
									Physics.IgnoreCollision(this.m_ContactedChefColliders[l], this.m_Colliders[m], true);
								}
							}
						}
						this.m_TimeEnteredPostLineState = time;
						this.m_SnapRigidbodyOnDataReceive = true;
						this.m_meshLerper.SetTargets(MeshLerper.Target.CurrentPosition, MeshLerper.Target.ServerPosition, 0.1f);
					}
				}
				break;
			}
			case ClientPhysicsObjectSynchroniser.State.PostLine:
				this.m_SnapRigidbodyOnDataReceive = false;
				if (time > this.m_TimeEnteredPostLineState + 0.2f)
				{
					this.ChangeState(ClientPhysicsObjectSynchroniser.State.ServerPosition);
				}
				if (this.m_CollidingWithLocalChef)
				{
					this.ChangeState(ClientPhysicsObjectSynchroniser.State.PreLine);
					this.m_TimeEnteredPreLineState = time;
				}
				break;
			}
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x000DA11C File Offset: 0x000D851C
		private void ChangeState(ClientPhysicsObjectSynchroniser.State _state)
		{
			this.m_CurrentState = _state;
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x000DA125 File Offset: 0x000D8525
		public void ChildRemoved(ClientWorldObjectSynchroniser _synchroniser)
		{
			if (this.m_PhysicalAttachment != null)
			{
				this.m_PhysicalAttachment.UseStaticPositioning();
			}
			this.m_RigidBodyMotion.SetKinematic(true);
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x000DA14F File Offset: 0x000D854F
		public void ChildAttached(ClientWorldObjectSynchroniser _synchroniser)
		{
			this.m_ChildAttachedTime = Time.time;
			if (this.m_PhysicalAttachment != null)
			{
				this.m_PhysicalAttachment.UseMeshLerp();
			}
			this.m_RigidBodyMotion.SetKinematic(true);
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x000DA184 File Offset: 0x000D8584
		private void OnCollisionStay(Collision collision)
		{
			if (this.m_bPaused)
			{
				return;
			}
			GameObject gameObject = collision.collider.gameObject;
			PlayerIDProvider playerIDProvider = gameObject.RequestComponent<PlayerIDProvider>();
			if (gameObject.layer == this.m_PlayerLayer && playerIDProvider != null && playerIDProvider.IsLocallyControlled())
			{
				this.m_LocalCollidingChefRigidbody = gameObject.GetComponent<Rigidbody>();
				if (!this.m_PendingSetKinematicState)
				{
					this.m_RigidBodyMotion.SetKinematic(false);
					this.m_PendingSetKinematicState = true;
				}
				if (!this.m_CollidingWithLocalChef && !this.m_bPreviousFrameCollision && this.m_LocalCollidingChefRigidbody.velocity.magnitude > 0.2f)
				{
					if (!this.m_WaitingOnRemoteCollisionInfo)
					{
						this.m_WaitingOnRemoteCollisionInfo = true;
					}
					this.m_ChefLocalDirectionOnCollision = this.m_RigidBodyMotion.GetVelocity().normalized;
				}
				if (this.m_RemoteCollisionConformationTimer < 0.1f)
				{
					this.m_RecentlyCollided = true;
				}
				this.m_RemoteCollisionConformationTimer = 0f;
				this.m_CollidingWithLocalChef = true;
				if (!this.m_ContactedChefColliders.Contains(collision.collider))
				{
					this.m_ContactedChefColliders.Add(collision.collider);
				}
			}
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000DA2B0 File Offset: 0x000D86B0
		public float GetRoundTripTime()
		{
			return this.m_MultiplayerController.GetClientConnectionStats(false).m_fLatency * 2f;
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x000DA2D8 File Offset: 0x000D86D8
		public float GetLatencyTime()
		{
			return this.m_MultiplayerController.GetClientConnectionStats(false).m_fLatency;
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x000DA2F9 File Offset: 0x000D86F9
		public Vector3 GetLinePosition()
		{
			return this.m_LinePosition;
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x000DA301 File Offset: 0x000D8701
		public Vector3 GetServerVelocity()
		{
			return this.m_ServerVelocity;
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x000DA309 File Offset: 0x000D8709
		public Vector3 ServerPreviousVelocity()
		{
			return this.m_ServerPreviousVelocity;
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x000DA314 File Offset: 0x000D8714
		public Vector3 GetServerPosition()
		{
			if (this.m_CurrentState != ClientPhysicsObjectSynchroniser.State.Line && this.m_TimeinLine > 0.2f)
			{
				return base.GetGlobalServerPosition() + this.m_ServerVelocity * this.GetLatencyTime() * 0.75f * Mathf.Clamp((this.m_TimeinLine - 0.2f) / 0.2f, 0f, 1f);
			}
			return base.GetGlobalServerPosition();
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x000DA390 File Offset: 0x000D8790
		public Vector3 GetExtrapolatedServerPosition()
		{
			return base.GetGlobalServerPosition() + this.m_ServerVelocity * (Time.time - this.m_TimeLastMessageReceived);
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x000DA3B4 File Offset: 0x000D87B4
		public Quaternion GetServerRotation()
		{
			return this.m_ServerLocalRotation;
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x000DA3BC File Offset: 0x000D87BC
		public static string GetDebugString()
		{
			return string.Empty;
		}

		// Token: 0x040024B4 RID: 9396
		private PhysicalAttachment m_PhysicalAttachment;

		// Token: 0x040024B5 RID: 9397
		private RigidbodyMotion m_RigidBodyMotion;

		// Token: 0x040024B6 RID: 9398
		private Vector3 m_ServerLocalPosition = Vector3.zero;

		// Token: 0x040024B7 RID: 9399
		private Vector3 m_ServerVelocity = Vector3.zero;

		// Token: 0x040024B8 RID: 9400
		private Vector3 m_ServerPreviousVelocity = Vector3.zero;

		// Token: 0x040024B9 RID: 9401
		private Quaternion m_ServerLocalRotation = Quaternion.identity;

		// Token: 0x040024BA RID: 9402
		private int m_PlayerLayer;

		// Token: 0x040024BB RID: 9403
		private Vector3 m_ChefLocalDirectionOnCollision = default(Vector3);

		// Token: 0x040024BC RID: 9404
		private bool m_CollidingWithLocalChef;

		// Token: 0x040024BD RID: 9405
		private bool m_PenetrationOccured;

		// Token: 0x040024BE RID: 9406
		private Rigidbody m_LocalCollidingChefRigidbody;

		// Token: 0x040024BF RID: 9407
		private bool m_WaitingOnRemoteCollisionInfo;

		// Token: 0x040024C0 RID: 9408
		private const float kRemoteCollisionWaitTime = 2f;

		// Token: 0x040024C1 RID: 9409
		private Collider[] m_Colliders;

		// Token: 0x040024C2 RID: 9410
		private List<Collider> m_ContactedChefColliders = new List<Collider>(2);

		// Token: 0x040024C3 RID: 9411
		private bool m_CollidersEnabled;

		// Token: 0x040024C4 RID: 9412
		private float m_RemoteCollisionConformationTimer;

		// Token: 0x040024C5 RID: 9413
		private bool m_RecentlyCollided;

		// Token: 0x040024C6 RID: 9414
		private Vector3 m_LinePosition = default(Vector3);

		// Token: 0x040024C7 RID: 9415
		public int m_ContactChefCount;

		// Token: 0x040024C8 RID: 9416
		public ClientPhysicsObjectSynchroniser.ContactingChef[] m_ContactingChefs = new ClientPhysicsObjectSynchroniser.ContactingChef[4];

		// Token: 0x040024C9 RID: 9417
		private MeshLerper m_meshLerper;

		// Token: 0x040024CA RID: 9418
		private bool m_SnapRigidbodyOnDataReceive = true;

		// Token: 0x040024CB RID: 9419
		private Transform m_ServerPositionTransform;

		// Token: 0x040024CC RID: 9420
		private MultiplayerController m_MultiplayerController;

		// Token: 0x040024CD RID: 9421
		private float m_TimeLastLocalCollision;

		// Token: 0x040024CE RID: 9422
		private float m_TimeLastServerCollision;

		// Token: 0x040024CF RID: 9423
		private float m_TimeStartedReceivingCollision;

		// Token: 0x040024D0 RID: 9424
		private float m_TimeEnteredPreLineState;

		// Token: 0x040024D1 RID: 9425
		private float m_TimeinLine;

		// Token: 0x040024D2 RID: 9426
		private float m_TimeLeftLineState;

		// Token: 0x040024D3 RID: 9427
		private float m_TimeEnteredLineState;

		// Token: 0x040024D4 RID: 9428
		private float m_TimeEnteredPostLineState;

		// Token: 0x040024D5 RID: 9429
		private float m_TimeAnyCollision;

		// Token: 0x040024D6 RID: 9430
		private float m_TimeLineStateEnd;

		// Token: 0x040024D7 RID: 9431
		private float m_TimeLastMessageReceived;

		// Token: 0x040024D8 RID: 9432
		private bool m_RemoteCollisionData;

		// Token: 0x040024D9 RID: 9433
		private bool m_bPreviousFrameCollision;

		// Token: 0x040024DA RID: 9434
		private bool m_bFirstNetworkMessage = true;

		// Token: 0x040024DB RID: 9435
		private float m_ChildAttachedTime;

		// Token: 0x040024DC RID: 9436
		private bool m_PendingSetKinematicState;

		// Token: 0x040024DD RID: 9437
		private ClientPhysicsObjectSynchroniser.State m_CurrentState;

		// Token: 0x040024DE RID: 9438
		private Vector3 m_PreviousRelativeTarget = default(Vector3);

		// Token: 0x040024DF RID: 9439
		private bool m_bNewLine = true;

		// Token: 0x040024E0 RID: 9440
		private bool m_bRenderLineSphere;

		// Token: 0x040024E1 RID: 9441
		private static NetworkPredictionTweekables Tweekables;

		// Token: 0x040024E2 RID: 9442
		public static float kCorrectiveForce = 0.2f;

		// Token: 0x0200091E RID: 2334
		public class ContactingChef
		{
			// Token: 0x040024E3 RID: 9443
			public uint EntityID;

			// Token: 0x040024E4 RID: 9444
			public GameObject ChefGameObject;

			// Token: 0x040024E5 RID: 9445
			public ClientChefSynchroniser ChefSynchroniser;

			// Token: 0x040024E6 RID: 9446
			public PositionRecorder ChefPositionRecorder;

			// Token: 0x040024E7 RID: 9447
			public Vector3 RelativePosition;

			// Token: 0x040024E8 RID: 9448
			public Vector3 ContactVelocity;

			// Token: 0x040024E9 RID: 9449
			public bool LocallyControlled;

			// Token: 0x040024EA RID: 9450
			public float ServerContactTime;
		}

		// Token: 0x0200091F RID: 2335
		private enum State
		{
			// Token: 0x040024EC RID: 9452
			ServerPosition,
			// Token: 0x040024ED RID: 9453
			PreLine,
			// Token: 0x040024EE RID: 9454
			Line,
			// Token: 0x040024EF RID: 9455
			PostLine
		}
	}
}
