using System;
using System.Collections.Generic;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x0200094A RID: 2378
public class RotPile : StateMachineComponent<RotPile.StatesInstance>
{
	// Token: 0x0600454B RID: 17739 RVA: 0x00186F31 File Offset: 0x00185131
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x0600454C RID: 17740 RVA: 0x00186F39 File Offset: 0x00185139
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x0600454D RID: 17741 RVA: 0x00186F4C File Offset: 0x0018514C
	protected void ConvertToElement()
	{
		PrimaryElement component = base.smi.master.GetComponent<PrimaryElement>();
		float mass = component.Mass;
		float temperature = component.Temperature;
		if (mass <= 0f)
		{
			Util.KDestroyGameObject(base.gameObject);
			return;
		}
		SimHashes hash = SimHashes.ToxicSand;
		GameObject gameObject = ElementLoader.FindElementByHash(hash).substance.SpawnResource(base.smi.master.transform.GetPosition(), mass, temperature, byte.MaxValue, 0, false, false, false);
		PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, ElementLoader.FindElementByHash(hash).name, gameObject.transform, 1.5f, false);
		Util.KDestroyGameObject(base.smi.gameObject);
	}

	// Token: 0x0600454E RID: 17742 RVA: 0x00187000 File Offset: 0x00185200
	private static string OnRottenTooltip(List<Notification> notifications, object data)
	{
		string text = "";
		foreach (Notification notification in notifications)
		{
			if (notification.tooltipData != null)
			{
				text = text + "\n• " + (string)notification.tooltipData + " ";
			}
		}
		return string.Format(MISC.NOTIFICATIONS.FOODROT.TOOLTIP, text);
	}

	// Token: 0x0600454F RID: 17743 RVA: 0x00187084 File Offset: 0x00185284
	public void TryClearNotification()
	{
		if (this.notification != null)
		{
			base.gameObject.AddOrGet<Notifier>().Remove(this.notification);
		}
	}

	// Token: 0x06004550 RID: 17744 RVA: 0x001870A4 File Offset: 0x001852A4
	public void TryCreateNotification()
	{
		WorldContainer myWorld = base.smi.master.GetMyWorld();
		if (myWorld != null && myWorld.worldInventory.IsReachable(base.smi.master.gameObject.GetComponent<Pickupable>()))
		{
			this.notification = new Notification(MISC.NOTIFICATIONS.FOODROT.NAME, NotificationType.BadMinor, new Func<List<Notification>, object, string>(RotPile.OnRottenTooltip), null, true, 0f, null, null, null, true, false, false);
			this.notification.tooltipData = base.smi.master.gameObject.GetProperName();
			base.gameObject.AddOrGet<Notifier>().Add(this.notification, "");
		}
	}

	// Token: 0x04002E01 RID: 11777
	private Notification notification;

	// Token: 0x0200179C RID: 6044
	public class StatesInstance : GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.GameInstance
	{
		// Token: 0x06008EFC RID: 36604 RVA: 0x00320E73 File Offset: 0x0031F073
		public StatesInstance(RotPile master) : base(master)
		{
		}

		// Token: 0x04006F62 RID: 28514
		public AttributeModifier baseDecomposeRate;
	}

	// Token: 0x0200179D RID: 6045
	public class States : GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile>
	{
		// Token: 0x06008EFD RID: 36605 RVA: 0x00320E7C File Offset: 0x0031F07C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.decomposing;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.decomposing.Enter(delegate(RotPile.StatesInstance smi)
			{
				smi.master.TryCreateNotification();
			}).Exit(delegate(RotPile.StatesInstance smi)
			{
				smi.master.TryClearNotification();
			}).ParamTransition<float>(this.decompositionAmount, this.convertDestroy, (RotPile.StatesInstance smi, float p) => p >= 600f).Update("Decomposing", delegate(RotPile.StatesInstance smi, float dt)
			{
				this.decompositionAmount.Delta(dt, smi);
			}, UpdateRate.SIM_200ms, false);
			this.convertDestroy.Enter(delegate(RotPile.StatesInstance smi)
			{
				smi.master.ConvertToElement();
			});
		}

		// Token: 0x04006F63 RID: 28515
		public GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State decomposing;

		// Token: 0x04006F64 RID: 28516
		public GameStateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.State convertDestroy;

		// Token: 0x04006F65 RID: 28517
		public StateMachine<RotPile.States, RotPile.StatesInstance, RotPile, object>.FloatParameter decompositionAmount;
	}
}
