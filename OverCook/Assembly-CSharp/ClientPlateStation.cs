using System;
using System.Collections;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000552 RID: 1362
public class ClientPlateStation : ClientSynchroniserBase
{
	// Token: 0x060019B7 RID: 6583 RVA: 0x00080B73 File Offset: 0x0007EF73
	public override EntityType GetEntityType()
	{
		return EntityType.PlateStation;
	}

	// Token: 0x060019B8 RID: 6584 RVA: 0x00080B76 File Offset: 0x0007EF76
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_ClientAttachStation = base.GetComponent<ClientAttachStation>();
		this.m_ClientAttachStation.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
	}

	// Token: 0x060019B9 RID: 6585 RVA: 0x00080BA4 File Offset: 0x0007EFA4
	private void OnScoreTipNotification(DataStore.Id id, object data)
	{
		TeamTip teamTip = (TeamTip)data;
		if (teamTip.m_tip > 0 && teamTip.m_team == this.m_plateStation.m_teamId && teamTip.m_station == this)
		{
			GameObject obj = GameUtils.InstantiateHoverIconUIController(this.m_plateStation.m_tipsFloatingNumberUI, this.GetAttachPoint(base.gameObject), "HoverIconCanvas", default(Vector3));
			DisplayIntUIController displayIntUIController = obj.RequireComponent<DisplayIntUIController>();
			displayIntUIController.Value = teamTip.m_tip;
		}
	}

	// Token: 0x060019BA RID: 6586 RVA: 0x00080C30 File Offset: 0x0007F030
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		PlateStationMessage plateStationMessage = (PlateStationMessage)serialisable;
		if (plateStationMessage.m_success)
		{
			ClientPlate plate = plateStationMessage.m_delivered.RequireComponent<ClientPlate>();
			this.DeliverPlate(plate);
		}
		else
		{
			this.FailedToDeliver(plateStationMessage.m_delivered);
		}
	}

	// Token: 0x060019BB RID: 6587 RVA: 0x00080C73 File Offset: 0x0007F073
	public override void UpdateSynchronising()
	{
		if (this.m_needsPlateCooldownTimer > 0f)
		{
			this.m_needsPlateCooldownTimer -= TimeManager.GetDeltaTime(base.gameObject);
		}
	}

	// Token: 0x060019BC RID: 6588 RVA: 0x00080CA0 File Offset: 0x0007F0A0
	private void DeliverPlate(ClientPlate _plate)
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.ServiceBell, base.gameObject.layer);
		base.StartCoroutine(this.DeliverySequence(_plate, this.m_plateStation.m_deliveryEffects));
		if (this.m_plateStation.m_onFoodDeliveredTrigger != string.Empty)
		{
			base.gameObject.SendTrigger(this.m_plateStation.m_onFoodDeliveredTrigger);
		}
	}

	// Token: 0x060019BD RID: 6589 RVA: 0x00080D0C File Offset: 0x0007F10C
	private IEnumerator DeliverySequence(ClientPlate plate, PlateStation.DeliveryFX deliveryFx)
	{
		this.m_waitForPfxDelay = new WaitForSeconds(deliveryFx.m_pfxToFadeDelayTime);
		foreach (Collider collider in plate.gameObject.RequestComponentsRecursive<Collider>())
		{
			collider.enabled = false;
		}
		Rigidbody rigidBody = plate.gameObject.RequestComponent<Rigidbody>();
		if (rigidBody != null)
		{
			rigidBody.isKinematic = true;
		}
		if (deliveryFx.m_deliverPFXPrefab != null)
		{
			GameObject pfx = deliveryFx.m_deliverPFXPrefab.InstantiateOnParent(base.transform, true);
			pfx.transform.SetParent(null);
			yield return this.m_waitForPfxDelay;
		}
		MeshRenderer[] allRenderers = plate.gameObject.RequestComponentsRecursive<MeshRenderer>();
		for (int j = 0; j < allRenderers.Length; j++)
		{
			MeshRenderer meshRenderer = allRenderers[j];
			if (!meshRenderer.material.HasProperty("_Mode"))
			{
				Material material = new Material(meshRenderer.material)
				{
					shader = deliveryFx.m_fadeOutShader
				};
				meshRenderer.material = material;
			}
			else
			{
				meshRenderer.material.SetFloat("_Mode", 2f);
				StandardShaderHelper.SetupMaterialWithBlendMode(allRenderers[j].material, StandardShaderHelper.BlendMode.Fade);
			}
		}
		bool errored = false;
		float progress = 0f;
		while (progress < 1f)
		{
			foreach (MeshRenderer meshRenderer2 in allRenderers)
			{
				if (meshRenderer2 == null)
				{
					if (!errored)
					{
						errored = true;
					}
				}
				else if (meshRenderer2.material.HasProperty("_Alpha"))
				{
					float value = 1f - progress;
					meshRenderer2.material.SetFloat("_Alpha", value);
				}
			}
			progress += TimeManager.GetDeltaTime(plate.gameObject) / deliveryFx.m_fadeTime;
			yield return null;
		}
		UnityEngine.Object.Destroy(plate.gameObject);
		yield break;
	}

	// Token: 0x060019BE RID: 6590 RVA: 0x00080D38 File Offset: 0x0007F138
	private void FailedToDeliver(GameObject _object)
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.UIBack, base.gameObject.layer);
		if (this.m_needsPlateCooldownTimer <= 0f && _object != null && _object.GetComponent<Plate>() == null)
		{
			GameUtils.InstantiateHoverIconUIController(this.m_plateStation.m_needsPlateFloatingUI, this.m_attachStation.GetAttachPoint(_object), "HoverIconCanvas", default(Vector3));
			this.m_needsPlateCooldownTimer = 1f;
		}
	}

	// Token: 0x060019BF RID: 6591 RVA: 0x00080DBB File Offset: 0x0007F1BB
	public Transform GetAttachPoint(GameObject gameObject)
	{
		return this.m_attachStation.GetAttachPoint(gameObject);
	}

	// Token: 0x060019C0 RID: 6592 RVA: 0x00080DCC File Offset: 0x0007F1CC
	private void Awake()
	{
		this.m_attachStation = base.gameObject.GetComponent<AttachStation>();
		this.m_plateStation = base.gameObject.RequireComponent<PlateStation>();
		this.m_dataStore = GameUtils.RequireManager<DataStore>();
		this.m_dataStore.Register(ClientPlateStation.k_scoreTipId, new DataStore.OnChangeNotification(this.OnScoreTipNotification));
	}

	// Token: 0x060019C1 RID: 6593 RVA: 0x00080E24 File Offset: 0x0007F224
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (null != this.m_ClientAttachStation)
		{
			this.m_ClientAttachStation.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
		}
		if (this.m_dataStore != null)
		{
			this.m_dataStore.Unregister(ClientPlateStation.k_scoreTipId, new DataStore.OnChangeNotification(this.OnScoreTipNotification));
		}
	}

	// Token: 0x060019C2 RID: 6594 RVA: 0x00080E8C File Offset: 0x0007F28C
	private bool CanAddItem(GameObject _object, PlacementContext _context)
	{
		return _object.GetComponent<ClientPlate>() != null;
	}

	// Token: 0x0400145F RID: 5215
	private const float NEEDS_PLATE_COOLDOWN_TIME = 1f;

	// Token: 0x04001460 RID: 5216
	private PlateStation m_plateStation;

	// Token: 0x04001461 RID: 5217
	private ClientAttachStation m_ClientAttachStation;

	// Token: 0x04001462 RID: 5218
	private float m_needsPlateCooldownTimer = -1f;

	// Token: 0x04001463 RID: 5219
	private DataStore m_dataStore;

	// Token: 0x04001464 RID: 5220
	private static readonly DataStore.Id k_scoreTipId = new DataStore.Id("score.tip");

	// Token: 0x04001465 RID: 5221
	private PlateStationMessage m_data = new PlateStationMessage();

	// Token: 0x04001466 RID: 5222
	private AttachStation m_attachStation;

	// Token: 0x04001467 RID: 5223
	private WaitForSeconds m_waitForPfxDelay;
}
