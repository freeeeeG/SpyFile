using System;
using UnityEngine;

namespace Team17.Online.Multiplayer.Messaging
{
	// Token: 0x020008FA RID: 2298
	public class PhysicsObjectLerp : EmptyLerp
	{
		// Token: 0x06002CC1 RID: 11457 RVA: 0x000D329C File Offset: 0x000D169C
		public void Start()
		{
			Lerp[] components = base.GetComponents<Lerp>();
			for (int i = 0; i < components.Length; i++)
			{
				MonoBehaviour monoBehaviour = components[i] as MonoBehaviour;
				if (monoBehaviour != this)
				{
					components[i] = null;
					UnityEngine.Object.Destroy(monoBehaviour);
				}
			}
		}

		// Token: 0x06002CC2 RID: 11458 RVA: 0x000D32E3 File Offset: 0x000D16E3
		public override void StartSynchronising(Component synchronisedObject)
		{
		}

		// Token: 0x06002CC3 RID: 11459 RVA: 0x000D32E5 File Offset: 0x000D16E5
		public virtual void ApplyLerpInfo(Vector3 targetPosition, Quaternion targetRotation)
		{
		}

		// Token: 0x06002CC4 RID: 11460 RVA: 0x000D32E7 File Offset: 0x000D16E7
		public override void Reparented()
		{
		}

		// Token: 0x06002CC5 RID: 11461 RVA: 0x000D32E9 File Offset: 0x000D16E9
		public override void UpdateLerp(float _delta)
		{
		}

		// Token: 0x06002CC6 RID: 11462 RVA: 0x000D32EB File Offset: 0x000D16EB
		public void Update_PositionLerp(float _lerp)
		{
		}

		// Token: 0x06002CC7 RID: 11463 RVA: 0x000D32ED File Offset: 0x000D16ED
		public void Update_RotationLerp(float _lerp)
		{
		}

		// Token: 0x06002CC8 RID: 11464 RVA: 0x000D32EF File Offset: 0x000D16EF
		public override void ReceiveServerUpdate(Vector3 localPosition, Quaternion localRotation)
		{
		}

		// Token: 0x06002CC9 RID: 11465 RVA: 0x000D32F1 File Offset: 0x000D16F1
		public override void ReceiveServerEvent(Vector3 localPosition, Quaternion localRotation)
		{
		}

		// Token: 0x06002CCA RID: 11466 RVA: 0x000D32F3 File Offset: 0x000D16F3
		public void ClientChefCollisionEnter()
		{
		}

		// Token: 0x06002CCB RID: 11467 RVA: 0x000D32F5 File Offset: 0x000D16F5
		public void ClientChefCollisionExit()
		{
		}

		// Token: 0x06002CCC RID: 11468 RVA: 0x000D32F7 File Offset: 0x000D16F7
		public void ClientChefOnCillisionStay()
		{
		}

		// Token: 0x06002CCD RID: 11469 RVA: 0x000D32F9 File Offset: 0x000D16F9
		private void ActivateLerp()
		{
		}

		// Token: 0x06002CCE RID: 11470 RVA: 0x000D32FB File Offset: 0x000D16FB
		private void ControlLocally()
		{
		}
	}
}
