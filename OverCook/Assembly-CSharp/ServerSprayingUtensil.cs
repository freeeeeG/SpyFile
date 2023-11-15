using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000616 RID: 1558
public class ServerSprayingUtensil : ServerSynchroniserBase, ICarryNotified, ITriggerReceiver
{
	// Token: 0x06001D7B RID: 7547 RVA: 0x0008E9D4 File Offset: 0x0008CDD4
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_SprayingUtensil = (SprayingUtensil)synchronisedObject;
		this.m_UtensilCollider = this.m_SprayingUtensil.gameObject.RequireComponent<Collider>();
		this.m_MaxSprayDistance = this.m_SprayingUtensil.m_sprayDistance;
		base.GetComponent<ClientInteractable>().SetStickyInteractionCallback(() => false);
	}

	// Token: 0x06001D7C RID: 7548 RVA: 0x0008EA3C File Offset: 0x0008CE3C
	public override EntityType GetEntityType()
	{
		return EntityType.SprayingUtensil;
	}

	// Token: 0x06001D7D RID: 7549 RVA: 0x0008EA3F File Offset: 0x0008CE3F
	public override void UpdateSynchronising()
	{
		if (this.IsSpraying())
		{
			this.UpdateSprayDistance();
		}
	}

	// Token: 0x06001D7E RID: 7550 RVA: 0x0008EA52 File Offset: 0x0008CE52
	public override void OnDestroy()
	{
		this.StopSpray();
		if (this.m_carrier)
		{
			this.OnCarryEnded(null);
		}
		base.GetComponent<ClientInteractable>().SetStickyInteractionCallback(null);
		base.OnDestroy();
	}

	// Token: 0x1700026B RID: 619
	// (get) Token: 0x06001D7F RID: 7551 RVA: 0x0008EA83 File Offset: 0x0008CE83
	public GameObject Carrier
	{
		get
		{
			return this.m_carrier;
		}
	}

	// Token: 0x06001D80 RID: 7552 RVA: 0x0008EA8B File Offset: 0x0008CE8B
	public virtual void OnCarryBegun(ICarrier _carrier)
	{
		this.m_carrier = (_carrier as MonoBehaviour).gameObject;
	}

	// Token: 0x06001D81 RID: 7553 RVA: 0x0008EAA0 File Offset: 0x0008CEA0
	public virtual void OnCarryEnded(ICarrier _carrier)
	{
		if (this.m_carrier)
		{
			PlayerControls playerControls = this.m_carrier.RequireComponent<PlayerControls>();
			playerControls.SetMovementScale(1f);
		}
	}

	// Token: 0x06001D82 RID: 7554 RVA: 0x0008EAD4 File Offset: 0x0008CED4
	protected virtual void StartSpray()
	{
		if (!this.m_ServerData.m_bSpraying)
		{
			this.m_ServerData.m_bSpraying = true;
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_carrier);
			if (entry != null)
			{
				this.m_ServerData.m_Carrier = entry.m_Header.m_uEntityID;
			}
			this.SendServerEvent(this.m_ServerData);
		}
	}

	// Token: 0x06001D83 RID: 7555 RVA: 0x0008EB31 File Offset: 0x0008CF31
	protected virtual void StopSpray()
	{
		if (this.m_ServerData.m_bSpraying)
		{
			this.m_ServerData.m_bSpraying = false;
			this.m_ServerData.m_Carrier = 0U;
			this.SendServerEvent(this.m_ServerData);
		}
	}

	// Token: 0x06001D84 RID: 7556 RVA: 0x0008EB67 File Offset: 0x0008CF67
	public void OnTrigger(string _trigger)
	{
		if (this.m_SprayingUtensil.m_startSprayTrigger == _trigger)
		{
			this.StartSpray();
		}
		else if (this.m_SprayingUtensil.m_stopSprayTrigger == _trigger)
		{
			this.StopSpray();
		}
	}

	// Token: 0x06001D85 RID: 7557 RVA: 0x0008EBA8 File Offset: 0x0008CFA8
	private void UpdateSprayDistance()
	{
		this.m_SprayingUtensil.m_sprayDistance = this.m_MaxSprayDistance;
		Ray ray = new Ray(this.m_SprayingUtensil.m_effectAttachPoint.position - this.m_SprayingUtensil.m_effectAttachPoint.forward, this.m_SprayingUtensil.m_effectAttachPoint.forward);
		int num = Physics.RaycastNonAlloc(ray, this.m_RaycastHits, this.m_SprayingUtensil.m_sprayDistance, this.m_SprayingUtensil.m_CollisionLayerMask, QueryTriggerInteraction.Ignore);
		for (int i = 0; i < num; i++)
		{
			if (this.m_RaycastHits[i].collider != this.m_UtensilCollider && this.m_RaycastHits[i].distance < this.m_SprayingUtensil.m_sprayDistance)
			{
				this.m_SprayingUtensil.m_sprayDistance = this.m_RaycastHits[i].distance;
			}
		}
	}

	// Token: 0x06001D86 RID: 7558 RVA: 0x0008EC9C File Offset: 0x0008D09C
	protected bool IsInSpray(Transform _t)
	{
		Transform transform = this.m_carrier.transform;
		Vector3 position = transform.position;
		Vector3 position2 = _t.position;
		Vector3 lhs = position2 - position;
		float num = (this.m_SprayingUtensil.m_sprayDistance + 0.6f) * (this.m_SprayingUtensil.m_sprayDistance + 0.6f);
		if (lhs.sqrMagnitude < num)
		{
			float num2 = Vector3.Dot(lhs, transform.forward);
			if (num2 > 0f && num2 < this.m_SprayingUtensil.m_sprayDistance + 0.6f)
			{
				float num3 = num2 * Mathf.Sin(0.017453292f * this.m_SprayingUtensil.m_sprayAngleInDegrees * 0.5f);
				Vector3 b = transform.position + transform.forward * num2;
				float num4 = (num3 + 0.6f) * (num3 + 0.6f);
				if (num4 > (position2 - b).sqrMagnitude)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06001D87 RID: 7559 RVA: 0x0008ED99 File Offset: 0x0008D199
	public bool IsSpraying()
	{
		return this.m_ServerData.m_bSpraying;
	}

	// Token: 0x040016CF RID: 5839
	private SprayingUtensilMessage m_ServerData = new SprayingUtensilMessage();

	// Token: 0x040016D0 RID: 5840
	private GameObject m_carrier;

	// Token: 0x040016D1 RID: 5841
	private RaycastHit[] m_RaycastHits = new RaycastHit[16];

	// Token: 0x040016D2 RID: 5842
	private Collider m_UtensilCollider;

	// Token: 0x040016D3 RID: 5843
	private float m_MaxSprayDistance;

	// Token: 0x040016D4 RID: 5844
	private SprayingUtensil m_SprayingUtensil;
}
