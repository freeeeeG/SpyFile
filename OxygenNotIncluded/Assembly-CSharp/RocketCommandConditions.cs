using System;

// Token: 0x020009B2 RID: 2482
public class RocketCommandConditions : KMonoBehaviour
{
	// Token: 0x060049ED RID: 18925 RVA: 0x001A043C File Offset: 0x0019E63C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		RocketModule component = base.GetComponent<RocketModule>();
		this.reachable = (ConditionDestinationReachable)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionDestinationReachable(base.GetComponent<RocketModule>()));
		this.allModulesComplete = (ConditionAllModulesComplete)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionAllModulesComplete(base.GetComponent<ILaunchableRocket>()));
		if (base.GetComponent<ILaunchableRocket>().registerType == LaunchableRocketRegisterType.Spacecraft)
		{
			this.destHasResources = (ConditionHasMinimumMass)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionHasMinimumMass(base.GetComponent<CommandModule>()));
			this.hasAstronaut = (ConditionHasAstronaut)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionHasAstronaut(base.GetComponent<CommandModule>()));
			this.hasSuit = (ConditionHasAtmoSuit)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new ConditionHasAtmoSuit(base.GetComponent<CommandModule>()));
			this.cargoEmpty = (CargoBayIsEmpty)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketStorage, new CargoBayIsEmpty(base.GetComponent<CommandModule>()));
		}
		else if (base.GetComponent<ILaunchableRocket>().registerType == LaunchableRocketRegisterType.Clustercraft)
		{
			this.hasEngine = (ConditionHasEngine)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionHasEngine(base.GetComponent<ILaunchableRocket>()));
			this.hasNosecone = (ConditionHasNosecone)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionHasNosecone(base.GetComponent<LaunchableRocketCluster>()));
			this.hasControlStation = (ConditionHasControlStation)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionHasControlStation(base.GetComponent<RocketModuleCluster>()));
			this.pilotOnBoard = (ConditionPilotOnBoard)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketBoard, new ConditionPilotOnBoard(base.GetComponent<PassengerRocketModule>()));
			this.passengersOnBoard = (ConditionPassengersOnBoard)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketBoard, new ConditionPassengersOnBoard(base.GetComponent<PassengerRocketModule>()));
			this.noExtraPassengers = (ConditionNoExtraPassengers)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketBoard, new ConditionNoExtraPassengers(base.GetComponent<PassengerRocketModule>()));
			this.onLaunchPad = (ConditionOnLaunchPad)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketPrep, new ConditionOnLaunchPad(base.GetComponent<RocketModuleCluster>().CraftInterface));
		}
		int bufferWidth = 1;
		if (DlcManager.FeatureClusterSpaceEnabled())
		{
			bufferWidth = 0;
		}
		this.flightPathIsClear = (ConditionFlightPathIsClear)component.AddModuleCondition(ProcessCondition.ProcessConditionType.RocketFlight, new ConditionFlightPathIsClear(base.gameObject, bufferWidth));
	}

	// Token: 0x0400308C RID: 12428
	public ConditionDestinationReachable reachable;

	// Token: 0x0400308D RID: 12429
	public ConditionHasAstronaut hasAstronaut;

	// Token: 0x0400308E RID: 12430
	public ConditionPilotOnBoard pilotOnBoard;

	// Token: 0x0400308F RID: 12431
	public ConditionPassengersOnBoard passengersOnBoard;

	// Token: 0x04003090 RID: 12432
	public ConditionNoExtraPassengers noExtraPassengers;

	// Token: 0x04003091 RID: 12433
	public ConditionHasAtmoSuit hasSuit;

	// Token: 0x04003092 RID: 12434
	public CargoBayIsEmpty cargoEmpty;

	// Token: 0x04003093 RID: 12435
	public ConditionHasMinimumMass destHasResources;

	// Token: 0x04003094 RID: 12436
	public ConditionAllModulesComplete allModulesComplete;

	// Token: 0x04003095 RID: 12437
	public ConditionHasControlStation hasControlStation;

	// Token: 0x04003096 RID: 12438
	public ConditionHasEngine hasEngine;

	// Token: 0x04003097 RID: 12439
	public ConditionHasNosecone hasNosecone;

	// Token: 0x04003098 RID: 12440
	public ConditionOnLaunchPad onLaunchPad;

	// Token: 0x04003099 RID: 12441
	public ConditionFlightPathIsClear flightPathIsClear;
}
