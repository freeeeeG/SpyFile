using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020005C5 RID: 1477
public class CarePackage : StateMachineComponent<CarePackage.SMInstance>
{
	// Token: 0x06002466 RID: 9318 RVA: 0x000C67B0 File Offset: 0x000C49B0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.info != null)
		{
			this.SetAnimToInfo();
		}
		this.reactable = this.CreateReactable();
	}

	// Token: 0x06002467 RID: 9319 RVA: 0x000C67E0 File Offset: 0x000C49E0
	public Reactable CreateReactable()
	{
		return new EmoteReactable(base.gameObject, "UpgradeFX", Db.Get().ChoreTypes.Emote, 15, 8, 0f, 20f, float.PositiveInfinity, 0f).SetEmote(Db.Get().Emotes.Minion.Cheer);
	}

	// Token: 0x06002468 RID: 9320 RVA: 0x000C6841 File Offset: 0x000C4A41
	protected override void OnCleanUp()
	{
		this.reactable.Cleanup();
		base.OnCleanUp();
	}

	// Token: 0x06002469 RID: 9321 RVA: 0x000C6854 File Offset: 0x000C4A54
	public void SetInfo(CarePackageInfo info)
	{
		this.info = info;
		this.SetAnimToInfo();
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x000C6863 File Offset: 0x000C4A63
	public void SetFacade(string facadeID)
	{
		this.facadeID = facadeID;
		this.SetAnimToInfo();
	}

	// Token: 0x0600246B RID: 9323 RVA: 0x000C6874 File Offset: 0x000C4A74
	private void SetAnimToInfo()
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Meter".ToTag()), base.gameObject, null);
		GameObject prefab = Assets.GetPrefab(this.info.id);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController component2 = prefab.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component3 = prefab.GetComponent<SymbolOverrideController>();
		KBatchedAnimController component4 = gameObject.GetComponent<KBatchedAnimController>();
		component4.transform.SetLocalPosition(Vector3.forward);
		component4.AnimFiles = component2.AnimFiles;
		component4.isMovable = true;
		component4.animWidth = component2.animWidth;
		component4.animHeight = component2.animHeight;
		if (component3 != null)
		{
			SymbolOverrideController symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
			foreach (SymbolOverrideController.SymbolEntry symbolEntry in component3.GetSymbolOverrides)
			{
				symbolOverrideController.AddSymbolOverride(symbolEntry.targetSymbol, symbolEntry.sourceSymbol, 0);
			}
		}
		component4.initialAnim = component2.initialAnim;
		component4.initialMode = KAnim.PlayMode.Loop;
		if (!string.IsNullOrEmpty(this.facadeID))
		{
			component4.SwapAnims(new KAnimFile[]
			{
				Db.GetEquippableFacades().Get(this.facadeID).AnimFile
			});
			base.GetComponentsInChildren<KBatchedAnimController>()[1].SetSymbolVisiblity("object", false);
		}
		KBatchedAnimTracker component5 = gameObject.GetComponent<KBatchedAnimTracker>();
		component5.controller = component;
		component5.symbol = new HashedString("snapTO_object");
		component5.offset = new Vector3(0f, 0.5f, 0f);
		gameObject.SetActive(true);
		component.SetSymbolVisiblity("snapTO_object", false);
		new KAnimLink(component, component4);
	}

	// Token: 0x0600246C RID: 9324 RVA: 0x000C6A14 File Offset: 0x000C4C14
	private void SpawnContents()
	{
		if (this.info == null)
		{
			global::Debug.LogWarning("CarePackage has no data to spawn from. Probably a save from before the CarePackage info data was serialized.");
			return;
		}
		GameObject gameObject = null;
		GameObject prefab = Assets.GetPrefab(this.info.id);
		Element element = ElementLoader.GetElement(this.info.id.ToTag());
		Vector3 position = base.transform.position + Vector3.up / 2f;
		if (element == null && prefab != null)
		{
			int num = 0;
			while ((float)num < this.info.quantity)
			{
				gameObject = Util.KInstantiate(prefab, position);
				if (gameObject != null)
				{
					if (!this.facadeID.IsNullOrWhiteSpace())
					{
						EquippableFacade.AddFacadeToEquippable(gameObject.GetComponent<Equippable>(), this.facadeID);
					}
					gameObject.SetActive(true);
				}
				num++;
			}
		}
		else if (element != null)
		{
			float quantity = this.info.quantity;
			gameObject = element.substance.SpawnResource(position, quantity, element.defaultValues.temperature, byte.MaxValue, 0, false, true, false);
		}
		else
		{
			global::Debug.LogWarning("Can't find spawnable thing from tag " + this.info.id);
		}
		if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
	}

	// Token: 0x040014DF RID: 5343
	[Serialize]
	public CarePackageInfo info;

	// Token: 0x040014E0 RID: 5344
	private string facadeID;

	// Token: 0x040014E1 RID: 5345
	private Reactable reactable;

	// Token: 0x02001252 RID: 4690
	public class SMInstance : GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.GameInstance
	{
		// Token: 0x06007CB6 RID: 31926 RVA: 0x002E20ED File Offset: 0x002E02ED
		public SMInstance(CarePackage master) : base(master)
		{
		}

		// Token: 0x04005F24 RID: 24356
		public List<Chore> activeUseChores;
	}

	// Token: 0x02001253 RID: 4691
	public class States : GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage>
	{
		// Token: 0x06007CB7 RID: 31927 RVA: 0x002E20F8 File Offset: 0x002E02F8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.spawn;
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			this.spawn.PlayAnim("portalbirth").OnAnimQueueComplete(this.open).ParamTransition<bool>(this.spawnedContents, this.pst, GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.IsTrue);
			this.open.PlayAnim("portalbirth_pst").QueueAnim("object_idle_loop", false, null).Exit(delegate(CarePackage.SMInstance smi)
			{
				smi.master.SpawnContents();
				this.spawnedContents.Set(true, smi, false);
			}).ScheduleGoTo(1f, this.pst);
			this.pst.PlayAnim("object_idle_pst").ScheduleGoTo(5f, this.destroy);
			this.destroy.Enter(delegate(CarePackage.SMInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x04005F25 RID: 24357
		public StateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.BoolParameter spawnedContents;

		// Token: 0x04005F26 RID: 24358
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State spawn;

		// Token: 0x04005F27 RID: 24359
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State open;

		// Token: 0x04005F28 RID: 24360
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State pst;

		// Token: 0x04005F29 RID: 24361
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State destroy;
	}
}
