using System;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x0200092A RID: 2346
	public class ClientWorldObjectSynchroniser : ClientSynchroniserBase
	{
		// Token: 0x06002E35 RID: 11829 RVA: 0x000D4FD6 File Offset: 0x000D33D6
		public virtual void Awake()
		{
			this.m_Transform = base.transform;
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x000D4FE4 File Offset: 0x000D33E4
		public override void StartSynchronising(Component synchronisedObject)
		{
			if (this.m_Lerper == null)
			{
				this.m_Lerper = base.gameObject.GetComponent<Lerp>();
			}
			if (this.m_Lerper != null)
			{
				this.m_Lerper.StartSynchronising(synchronisedObject);
			}
			this.m_Transform = base.transform;
			this.m_PhysicalAttachment = base.GetComponent<PhysicalAttachment>();
			this.m_bHasParent = (this.m_Transform.parent != null);
			this.m_Parent = null;
			this.m_ParentEntityID = 0U;
			if (this.m_Transform.parent != null)
			{
				IParentable parentable = this.m_Transform.parent.gameObject.RequestInterfaceUpwardsRecursive<IParentable>();
				if (parentable != null)
				{
					this.m_Transform.SetParent(parentable.GetAttachPoint(base.gameObject), true);
					this.OnParentChanged();
					EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(((MonoBehaviour)parentable).gameObject);
					if (entry != null)
					{
						this.m_ParentEntityID = entry.m_Header.m_uEntityID;
					}
				}
			}
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x000D50D9 File Offset: 0x000D34D9
		public override EntityType GetEntityType()
		{
			return EntityType.WorldObject;
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x000D50DC File Offset: 0x000D34DC
		public override void ApplyServerUpdate(Serialisable serialisable)
		{
			if (!this.m_bPaused)
			{
				WorldObjectMessage worldObjectMessage = (WorldObjectMessage)serialisable;
				bool flag = this.ParentingLogic(worldObjectMessage);
				if (worldObjectMessage.HasPositions)
				{
					this.m_ServerPosition = worldObjectMessage.LocalPosition;
					if (flag && this.m_Lerper != null)
					{
						this.m_Lerper.ReceiveServerUpdate(worldObjectMessage.LocalPosition, worldObjectMessage.LocalRotation);
					}
				}
				this.m_bHasEverReceived = true;
			}
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x000D514C File Offset: 0x000D354C
		public override void ApplyServerEvent(Serialisable serialisable)
		{
			if (!this.m_bPaused)
			{
				WorldObjectMessage worldObjectMessage = (WorldObjectMessage)serialisable;
				bool flag = this.ParentingLogic(worldObjectMessage);
				if (worldObjectMessage.HasPositions)
				{
					this.m_ServerPosition = worldObjectMessage.LocalPosition;
					if (flag && this.m_Lerper != null)
					{
						this.m_Lerper.ReceiveServerEvent(worldObjectMessage.LocalPosition, worldObjectMessage.LocalRotation);
					}
				}
				this.m_bHasEverReceived = true;
			}
		}

		// Token: 0x06002E3A RID: 11834 RVA: 0x000D51BC File Offset: 0x000D35BC
		public Vector3 GetGlobalServerPosition()
		{
			if (this.m_Transform.parent != null)
			{
				return this.m_Transform.parent.position + this.m_Transform.parent.rotation * this.m_ServerPosition;
			}
			return this.m_ServerPosition;
		}

		// Token: 0x06002E3B RID: 11835 RVA: 0x000D5218 File Offset: 0x000D3618
		public void CorrectScale()
		{
			Vector3 lossyScale = this.m_PhysicalAttachment.transform.lossyScale;
			Vector3 b = new Vector3(1f / lossyScale.x, 1f / lossyScale.y, 1f / lossyScale.z);
			this.m_PhysicalAttachment.transform.localScale = this.m_PhysicalAttachment.transform.localScale.MultipliedBy(b);
		}

		// Token: 0x06002E3C RID: 11836 RVA: 0x000D528C File Offset: 0x000D368C
		public bool ParentingLogic(WorldObjectMessage dataReceived)
		{
			bool result = true;
			if (this.m_bHasParent != dataReceived.HasParent || this.m_ParentEntityID != dataReceived.ParentEntityID)
			{
				IParentable parent = this.m_Parent;
				this.m_bHasParent = dataReceived.HasParent;
				this.m_ParentEntityID = dataReceived.ParentEntityID;
				this.m_Parent = null;
				if (this.m_bHasParent)
				{
					if (this.m_ParentEntityID != 0U)
					{
						EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_ParentEntityID);
						if (entry != null)
						{
							this.m_Parent = entry.m_GameObject.RequestInterface<IParentable>();
							result = this.DoReparenting(parent, this.m_Parent);
						}
					}
				}
				else
				{
					result = this.DoReparenting(parent, this.m_Parent);
				}
			}
			return result;
		}

		// Token: 0x06002E3D RID: 11837 RVA: 0x000D5340 File Offset: 0x000D3740
		public virtual bool DoReparenting(IParentable _oldParent, IParentable _newParent)
		{
			if (this.m_Transform.parent != null)
			{
				ClientPhysicsObjectSynchroniser component = this.m_Transform.parent.gameObject.GetComponent<ClientPhysicsObjectSynchroniser>();
				if (component != null)
				{
					component.ChildRemoved(this);
				}
			}
			if (_newParent != null)
			{
				ObjectContainer objectContainer = _newParent as ObjectContainer;
				if (objectContainer != null)
				{
					ClientPhysicsObjectSynchroniser component2 = objectContainer.GetComponent<ClientPhysicsObjectSynchroniser>();
					if (component2 != null)
					{
						component2.ChildAttached(this);
					}
				}
			}
			if (null == this.m_Transform)
			{
				this.m_Transform = base.transform;
			}
			if (_newParent != null)
			{
				this.m_Transform.SetParent(_newParent.GetAttachPoint(base.gameObject));
				this.OnParentChanged();
				if (this.m_PhysicalAttachment != null && this.m_PhysicalAttachment.m_container != null)
				{
					this.m_PhysicalAttachment.m_container.position = base.transform.position;
					this.m_PhysicalAttachment.m_container.rotation = base.transform.rotation;
				}
			}
			else
			{
				this.m_Transform.SetParent(null, true);
				this.OnParentChanged();
			}
			bool flag = _oldParent == null || !_oldParent.HasClientSidePrediction() || _newParent == null || !_newParent.HasClientSidePrediction();
			if (this.m_Lerper != null && flag)
			{
				this.m_Lerper.Reparented();
			}
			return flag;
		}

		// Token: 0x06002E3E RID: 11838 RVA: 0x000D54B8 File Offset: 0x000D38B8
		private void OnParentChanged()
		{
			if (this.m_parentChanged != null)
			{
				this.m_parentChanged();
			}
		}

		// Token: 0x06002E3F RID: 11839 RVA: 0x000D54D0 File Offset: 0x000D38D0
		public void RegisterOnParentChanged(GenericVoid _callback)
		{
			this.m_parentChanged = (GenericVoid)Delegate.Combine(this.m_parentChanged, _callback);
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x000D54E9 File Offset: 0x000D38E9
		public void UnregisterOnParentChanged(GenericVoid _callback)
		{
			this.m_parentChanged = (GenericVoid)Delegate.Remove(this.m_parentChanged, _callback);
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x000D5504 File Offset: 0x000D3904
		public override void UpdateSynchronising()
		{
			if (!this.m_bPaused && (this.m_Parent == null || !this.m_Parent.HasClientSidePrediction()) && this.m_Lerper != null)
			{
				this.m_Lerper.UpdateLerp(TimeManager.GetDeltaTime(base.gameObject));
			}
		}

		// Token: 0x06002E42 RID: 11842 RVA: 0x000D5558 File Offset: 0x000D3958
		public virtual void Pause()
		{
			this.m_bPaused = true;
		}

		// Token: 0x06002E43 RID: 11843 RVA: 0x000D5561 File Offset: 0x000D3961
		public virtual void Resume()
		{
			if (this.m_bPaused)
			{
				this.m_bPaused = false;
				this.ApplyResumeData(this.m_PendingResumeData);
			}
			this.m_PendingResumeData = null;
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x000D5588 File Offset: 0x000D3988
		protected virtual void ApplyResumeData(Serialisable _data)
		{
			WorldObjectMessage worldObjectMessage = (WorldObjectMessage)_data;
			this.ParentingLogic(worldObjectMessage);
			if (worldObjectMessage.HasPositions)
			{
				this.m_ServerPosition = worldObjectMessage.LocalPosition;
				this.m_Transform.localPosition = worldObjectMessage.LocalPosition;
				this.m_Transform.localRotation = worldObjectMessage.LocalRotation;
			}
			if (this.m_Lerper != null)
			{
				this.m_Lerper.Reset();
			}
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x000D55F4 File Offset: 0x000D39F4
		public virtual void OnResumeDataReceived(Serialisable _data)
		{
			WorldObjectMessage other = (WorldObjectMessage)_data;
			WorldObjectMessage worldObjectMessage = new WorldObjectMessage();
			worldObjectMessage.Copy(other);
			this.m_PendingResumeData = worldObjectMessage;
		}

		// Token: 0x06002E46 RID: 11846 RVA: 0x000D561C File Offset: 0x000D3A1C
		public virtual bool IsReadyToResume()
		{
			return this.m_PendingResumeData != null;
		}

		// Token: 0x0400251F RID: 9503
		protected Transform m_Transform;

		// Token: 0x04002520 RID: 9504
		protected IParentable m_Parent;

		// Token: 0x04002521 RID: 9505
		protected uint m_ParentEntityID;

		// Token: 0x04002522 RID: 9506
		protected bool m_bHasParent;

		// Token: 0x04002523 RID: 9507
		private Vector3 m_ServerPosition = default(Vector3);

		// Token: 0x04002524 RID: 9508
		protected Lerp m_Lerper;

		// Token: 0x04002525 RID: 9509
		protected bool m_bPaused;

		// Token: 0x04002526 RID: 9510
		protected Serialisable m_PendingResumeData;

		// Token: 0x04002527 RID: 9511
		private PhysicalAttachment m_PhysicalAttachment;

		// Token: 0x04002528 RID: 9512
		private GenericVoid m_parentChanged;

		// Token: 0x04002529 RID: 9513
		public bool m_bHasEverReceived;
	}
}
