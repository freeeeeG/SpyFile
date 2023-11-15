using System;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x02000919 RID: 2329
	public class ServerPhysicsObjectSynchroniser : ServerWorldObjectSynchroniser
	{
		// Token: 0x06002DA5 RID: 11685 RVA: 0x000D8B60 File Offset: 0x000D6F60
		public override void Awake()
		{
			base.Awake();
			IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
			this.m_SessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
			this.m_PlayerLayer = LayerMask.NameToLayer("Players");
			this.m_RigidBody = base.gameObject.RequireComponent<Rigidbody>();
			this.m_CollisionRecorder = base.gameObject.AddComponent<CollisionRecorder>();
			this.m_CollisionRecorder.SetFilter(new Generic<bool, Collision>(this.CollisionFilter));
			ServerPhysicsObjectSynchroniser.ms_ServerPhysicsObjectSytnchroniserTransforms.Add(new ServerPhysicsObjectSynchroniser.SerialisationEntryTransformPair(this.m_Transform, EntitySerialisationRegistry.GetEntry(base.gameObject)));
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000D8BF0 File Offset: 0x000D6FF0
		public override void OnDestroy()
		{
			for (int i = 0; i < ServerPhysicsObjectSynchroniser.ms_ServerPhysicsObjectSytnchroniserTransforms.Count; i++)
			{
				if (ServerPhysicsObjectSynchroniser.ms_ServerPhysicsObjectSytnchroniserTransforms[i].m_Transform == this.m_Transform)
				{
					ServerPhysicsObjectSynchroniser.ms_ServerPhysicsObjectSytnchroniserTransforms.RemoveAt(i);
					break;
				}
			}
			base.OnDestroy();
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000D8C4E File Offset: 0x000D704E
		public static List<ServerPhysicsObjectSynchroniser.SerialisationEntryTransformPair> GetAllSynchroniserSerialisationEntryTransformPairs()
		{
			return ServerPhysicsObjectSynchroniser.ms_ServerPhysicsObjectSytnchroniserTransforms;
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000D8C58 File Offset: 0x000D7058
		public override void StartSynchronising(Component synchronisedObject)
		{
			this.m_RigidBody = base.GetComponent<Rigidbody>();
			base.StartSynchronising(synchronisedObject);
			this.m_PhysicsObjectData.WorldObject = base.GetMessageData();
			ServerWorldObjectSynchroniser[] components = base.GetComponents<ServerWorldObjectSynchroniser>();
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i].GetType() != typeof(ServerPhysicsObjectSynchroniser))
				{
					UnityEngine.Object.Destroy(components[i]);
				}
			}
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x000D8CC3 File Offset: 0x000D70C3
		public override EntityType GetEntityType()
		{
			return EntityType.PhysicsObject;
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x000D8CC8 File Offset: 0x000D70C8
		public override Serialisable GetServerUpdate()
		{
			if (this.Serialising)
			{
				base.GetServerUpdate();
				if (this.m_RigidBody == null)
				{
					this.m_RigidBody = base.GetComponent<Rigidbody>();
				}
				if (this.m_PhysicsObjectData.Velocity != this.m_RigidBody.velocity)
				{
					this.m_PhysicsObjectData.Velocity = this.m_RigidBody.velocity;
					this.m_bPhysicsActive = true;
				}
				else
				{
					this.m_bPhysicsActive = false;
				}
				this.StoreContactInformation();
			}
			return null;
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x000D8D54 File Offset: 0x000D7154
		private bool IsPhysicsObjectActive()
		{
			return this.m_bPhysicsActive || base.IsWorldObjectActive();
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x000D8D6A File Offset: 0x000D716A
		public override bool HasTargetedServerUpdates()
		{
			return this.Serialising;
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000D8D72 File Offset: 0x000D7172
		public override Serialisable GetServerUpdateForRecipient(IOnlineMultiplayerSessionUserId recipient)
		{
			if (recipient == null)
			{
				return null;
			}
			if (!this.Serialising)
			{
				return null;
			}
			if (this.IsPhysicsObjectActive())
			{
				return this.m_PhysicsObjectData;
			}
			return null;
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000D8D9C File Offset: 0x000D719C
		public override void SendServerEvent(Serialisable message)
		{
			if (this.Serialising && this.m_SessionCoordinator != null)
			{
				IOnlineMultiplayerSessionUserId[] array = this.m_SessionCoordinator.Members();
				if (array != null)
				{
					this.m_PhysicsObjectData.Velocity.Set(0f, 0f, 0f);
					this.StoreContactInformation();
					for (int i = 0; i < array.Length; i++)
					{
						this.SendServerEventToRecipient(array[i], this.m_PhysicsObjectData);
					}
				}
			}
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x000D8E1C File Offset: 0x000D721C
		private void StoreContactInformation()
		{
			for (int i = this.m_CollidingChefs.Count - 1; i >= 0; i--)
			{
				ServerPhysicsObjectSynchroniser.CollidingPlayer collidingPlayer = this.m_CollidingChefs._items[i];
				if (collidingPlayer.Transform == null)
				{
					this.m_CollidingChefs.RemoveAt(i);
				}
			}
			this.m_PhysicsObjectData.ContactCount = (uint)this.m_CollidingChefs.Count;
			for (int j = 0; j < this.m_CollidingChefs.Count; j++)
			{
				ServerPhysicsObjectSynchroniser.CollidingPlayer collidingPlayer2 = this.m_CollidingChefs._items[j];
				this.m_PhysicsObjectData.Contacts[j] = collidingPlayer2.ID;
				this.m_PhysicsObjectData.RelativePositions[j] = this.m_Transform.position - collidingPlayer2.Transform.position;
				this.m_PhysicsObjectData.ContactTimes[j] = collidingPlayer2.LastCollisionTime;
				this.m_PhysicsObjectData.ContactVelocitys[j] = collidingPlayer2.ContactVelocity;
			}
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000D8F28 File Offset: 0x000D7328
		private bool CollisionFilter(Collision _collision)
		{
			if (_collision.gameObject.layer != this.m_PlayerLayer)
			{
				return false;
			}
			Rigidbody rigidbody = _collision.rigidbody;
			return !(rigidbody == null) && rigidbody.velocity.sqrMagnitude > 0.001f;
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000D8F80 File Offset: 0x000D7380
		public override void UpdateSynchronising()
		{
			List<Collision> recentCollisions = this.m_CollisionRecorder.GetRecentCollisions();
			float num = ClientTime.Time();
			for (int i = 0; i < recentCollisions.Count; i++)
			{
				Collision collision = recentCollisions[i];
				int instanceID = collision.gameObject.GetInstanceID();
				bool flag = true;
				for (int j = 0; j < this.m_CollidingChefs.Count; j++)
				{
					ServerPhysicsObjectSynchroniser.CollidingPlayer collidingPlayer = this.m_CollidingChefs._items[j];
					if (instanceID == collidingPlayer.InstanceID)
					{
						flag = false;
						collidingPlayer.LastCollisionTime = num;
						break;
					}
				}
				if (flag)
				{
					EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(collision.gameObject);
					if (entry != null)
					{
						this.m_CollidingChefs.Add(new ServerPhysicsObjectSynchroniser.CollidingPlayer(entry.m_Header.m_uEntityID, collision.transform, instanceID, num, collision.gameObject.GetComponent<Rigidbody>().velocity));
					}
				}
			}
			float num2 = num - 0.2f;
			for (int k = this.m_CollidingChefs.Count - 1; k >= 0; k--)
			{
				ServerPhysicsObjectSynchroniser.CollidingPlayer collidingPlayer2 = this.m_CollidingChefs._items[k];
				if (collidingPlayer2.LastCollisionTime < num2)
				{
					this.m_CollidingChefs.RemoveAt(k);
				}
			}
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000D90C4 File Offset: 0x000D74C4
		protected override bool SendResumeData(IOnlineMultiplayerSessionUserId sessionUserId)
		{
			base.RefreshParent();
			this.m_PhysicsObjectData.WorldObject.LocalPosition = this.m_Transform.localPosition;
			this.m_PhysicsObjectData.WorldObject.LocalRotation = this.m_Transform.localRotation;
			this.m_PhysicsObjectData.Velocity = this.m_RigidBody.velocity;
			this.StoreContactInformation();
			ServerMessenger.ResumePhysicsObjectSync(base.gameObject, this.m_PhysicsObjectData, sessionUserId);
			return true;
		}

		// Token: 0x040024A3 RID: 9379
		public bool Serialising = true;

		// Token: 0x040024A4 RID: 9380
		private static List<ServerPhysicsObjectSynchroniser.SerialisationEntryTransformPair> ms_ServerPhysicsObjectSytnchroniserTransforms = new List<ServerPhysicsObjectSynchroniser.SerialisationEntryTransformPair>();

		// Token: 0x040024A5 RID: 9381
		private IOnlineMultiplayerSessionCoordinator m_SessionCoordinator;

		// Token: 0x040024A6 RID: 9382
		private Rigidbody m_RigidBody;

		// Token: 0x040024A7 RID: 9383
		private PhysicsObjectMessage m_PhysicsObjectData = new PhysicsObjectMessage();

		// Token: 0x040024A8 RID: 9384
		private FastList<ServerPhysicsObjectSynchroniser.CollidingPlayer> m_CollidingChefs = new FastList<ServerPhysicsObjectSynchroniser.CollidingPlayer>();

		// Token: 0x040024A9 RID: 9385
		private CollisionRecorder m_CollisionRecorder;

		// Token: 0x040024AA RID: 9386
		private int m_PlayerLayer;

		// Token: 0x040024AB RID: 9387
		private const float kCollisionLingerTime = 0.2f;

		// Token: 0x040024AC RID: 9388
		private bool m_bPhysicsActive;

		// Token: 0x0200091A RID: 2330
		public class SerialisationEntryTransformPair
		{
			// Token: 0x06002DB4 RID: 11700 RVA: 0x000D9149 File Offset: 0x000D7549
			public SerialisationEntryTransformPair(Transform _transform, EntitySerialisationEntry _entry)
			{
				this.m_Transform = _transform;
				this.m_Entry = _entry;
			}

			// Token: 0x040024AD RID: 9389
			public Transform m_Transform;

			// Token: 0x040024AE RID: 9390
			public EntitySerialisationEntry m_Entry;
		}

		// Token: 0x0200091B RID: 2331
		private class CollidingPlayer
		{
			// Token: 0x06002DB5 RID: 11701 RVA: 0x000D9160 File Offset: 0x000D7560
			public CollidingPlayer(uint _id, Transform _transform, int _instanceID, float _collisionTime, Vector3 _contactVelocity)
			{
				this.ID = _id;
				this.Transform = _transform;
				this.InstanceID = _instanceID;
				this.LastCollisionTime = _collisionTime;
				this.ContactVelocity = _contactVelocity;
			}

			// Token: 0x040024AF RID: 9391
			public uint ID;

			// Token: 0x040024B0 RID: 9392
			public Transform Transform;

			// Token: 0x040024B1 RID: 9393
			public int InstanceID;

			// Token: 0x040024B2 RID: 9394
			public float LastCollisionTime;

			// Token: 0x040024B3 RID: 9395
			public Vector3 ContactVelocity = default(Vector3);
		}
	}
}
