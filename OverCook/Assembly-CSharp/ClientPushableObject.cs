using System;
using UnityEngine;

// Token: 0x0200055B RID: 1371
public class ClientPushableObject : ClientSessionInteractable
{
	// Token: 0x060019D7 RID: 6615 RVA: 0x000819F1 File Offset: 0x0007FDF1
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_PushableObject = (PushableObject)synchronisedObject;
		if (this.m_PushableObject.m_fakePlayerCollider != null)
		{
			this.m_PushableObject.m_fakePlayerCollider.enabled = false;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x060019D8 RID: 6616 RVA: 0x00081A30 File Offset: 0x0007FE30
	public Vector2 CosmeticMovementDirection
	{
		get
		{
			if (base.Session != null)
			{
				ClientPushableObject.UserSession userSession = base.Session as ClientPushableObject.UserSession;
				return userSession.CosmeticMovementDirection;
			}
			return Vector2.zero;
		}
	}

	// Token: 0x060019D9 RID: 6617 RVA: 0x00081A60 File Offset: 0x0007FE60
	protected override ClientSessionInteractable.SessionBase BuildSession(GameObject _interacter)
	{
		return new ClientPushableObject.UserSession(this, _interacter, this.m_PushableObject);
	}

	// Token: 0x0400148A RID: 5258
	private PushableObject m_PushableObject;

	// Token: 0x0200055C RID: 1372
	private class UserSession : ClientSessionInteractable.SessionBase
	{
		// Token: 0x060019DA RID: 6618 RVA: 0x00081A70 File Offset: 0x0007FE70
		public UserSession(ClientPushableObject _self, GameObject _avatar, PushableObject _pushableObject) : base(_self, _avatar)
		{
			this.m_PushableObject = _pushableObject;
			this.m_PilotMovement = _self.gameObject.RequireComponent<PilotMovement>();
			this.m_ClientPilotMovement = _self.gameObject.RequireComponent<ClientPilotMovement>();
			this.m_ClientPilotMovement.AssignAvatar(_avatar);
			DynamicLandscapeParenting dynamicLandscapeParenting = base.PlayerControls.gameObject.RequestComponent<DynamicLandscapeParenting>();
			if (dynamicLandscapeParenting != null)
			{
				dynamicLandscapeParenting.enabled = false;
			}
			if (this.m_PushableObject.m_fakePlayerCollider != null)
			{
				this.m_PushableObject.m_fakePlayerCollider.transform.position = base.PlayerControls.transform.position;
				this.m_PushableObject.m_fakePlayerCollider.enabled = true;
			}
			this.m_lastPosition = this.m_ClientPilotMovement.transform.position;
		}

		// Token: 0x060019DB RID: 6619 RVA: 0x00081B40 File Offset: 0x0007FF40
		public override void Update()
		{
			base.Update();
			Transform transform = base.PlayerControls.transform;
			if (!this.m_PlayerAttached && this.m_PushableObject.IsAttached(transform))
			{
				this.m_PlayerAttached = true;
				if (this.m_PushableObject.m_UseAttachPoints)
				{
					transform.localPosition = Vector3.zero;
					transform.localRotation = Quaternion.identity;
				}
				else
				{
					Quaternion rotation = default(Quaternion);
					rotation.SetLookRotation(this.m_PushableObject.m_CentrePoint.transform.position - transform.position);
					transform.rotation = rotation;
				}
			}
			if (this.m_PushableObject.m_fakePlayerCollider != null)
			{
				this.m_PushableObject.m_fakePlayerCollider.transform.position = transform.position;
			}
			if (!this.m_PushableObject.m_UseAttachPoints && base.PlayerControls.ControlScheme != null)
			{
				Vector3 a = transform.position - this.m_PushableObject.m_CentrePoint.transform.position;
				float sqrMagnitude = a.sqrMagnitude;
				if (Mathf.Abs(sqrMagnitude - this.m_PushableObject.m_idealPlayerDistance * this.m_PushableObject.m_idealPlayerDistance) > 0.0001f)
				{
					float num = Mathf.Sqrt(sqrMagnitude);
					if (sqrMagnitude > 0.0001f)
					{
						a /= num;
					}
					else
					{
						a = -base.PlayerControls.transform.forward;
					}
					float deltaTime = TimeManager.GetDeltaTime(this.m_PushableObject.gameObject.layer);
					float num2 = this.m_PushableObject.m_idealPlayerDistance - num;
					float d = (num2 >= 0f) ? Mathf.Min(num2, this.m_PushableObject.m_lerpPlayerSpeed * deltaTime) : Mathf.Max(num2, -this.m_PushableObject.m_lerpPlayerSpeed * deltaTime);
					transform.position += a * d;
				}
			}
			this.ToggleDraggingAudio(this.CosmeticMovementDirection.sqrMagnitude > 0f);
		}

		// Token: 0x060019DC RID: 6620 RVA: 0x00081D64 File Offset: 0x00080164
		private void ToggleDraggingAudio(bool _enabled)
		{
			if (!this.m_PushableObject.m_playDraggingAudio)
			{
				return;
			}
			if (_enabled == this.m_playingAudio)
			{
				return;
			}
			if (_enabled)
			{
				GameUtils.StartAudio(this.m_PushableObject.m_draggingAudioTag, this, this.m_PushableObject.gameObject.layer);
			}
			else
			{
				GameUtils.StopAudio(this.m_PushableObject.m_draggingAudioTag, this);
			}
			this.m_playingAudio = _enabled;
		}

		// Token: 0x060019DD RID: 6621 RVA: 0x00081DD4 File Offset: 0x000801D4
		public override void OnSessionEnded()
		{
			this.m_ClientPilotMovement.AssignAvatar(null);
			base.OnSessionEnded();
			DynamicLandscapeParenting dynamicLandscapeParenting = base.PlayerControls.gameObject.RequestComponent<DynamicLandscapeParenting>();
			if (dynamicLandscapeParenting != null)
			{
				dynamicLandscapeParenting.enabled = true;
			}
			if (this.m_PushableObject.m_fakePlayerCollider != null)
			{
				this.m_PushableObject.m_fakePlayerCollider.enabled = false;
			}
			this.ToggleDraggingAudio(false);
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060019DE RID: 6622 RVA: 0x00081E48 File Offset: 0x00080248
		public Vector2 CosmeticMovementDirection
		{
			get
			{
				if (base.PlayerControls.ControlScheme != null)
				{
					return new Vector2(base.PlayerControls.ControlScheme.m_moveX.GetValue(), -base.PlayerControls.ControlScheme.m_moveY.GetValue());
				}
				Vector3 position = this.m_ClientPilotMovement.transform.position;
				float deltaTime = TimeManager.GetDeltaTime(this.m_ClientPilotMovement.gameObject.layer);
				if (deltaTime > 0.001f)
				{
					Vector3 a = (position - this.m_lastPosition) / deltaTime;
					float num = Mathf.Min(10f * deltaTime, 1f);
					this.m_velocityAverage = num * a + (1f - num) * this.m_velocityAverage;
					this.m_lastPosition = position;
				}
				return this.ClampByMagnitude(this.m_velocityAverage.XZ() / this.m_PilotMovement.MoveSpeed, 1f);
			}
		}

		// Token: 0x060019DF RID: 6623 RVA: 0x00081F40 File Offset: 0x00080340
		private Vector2 ClampByMagnitude(Vector2 _input, float _maxMagnitude)
		{
			float sqrMagnitude = _input.sqrMagnitude;
			if (sqrMagnitude > _maxMagnitude * _maxMagnitude)
			{
				return _input.normalized * _maxMagnitude;
			}
			return _input;
		}

		// Token: 0x0400148B RID: 5259
		private PushableObject m_PushableObject;

		// Token: 0x0400148C RID: 5260
		private ClientPilotMovement m_ClientPilotMovement;

		// Token: 0x0400148D RID: 5261
		private bool m_PlayerAttached;

		// Token: 0x0400148E RID: 5262
		private bool m_playingAudio;

		// Token: 0x0400148F RID: 5263
		private PilotMovement m_PilotMovement;

		// Token: 0x04001490 RID: 5264
		private Vector3 m_lastPosition;

		// Token: 0x04001491 RID: 5265
		private Vector3 m_velocityAverage;
	}
}
