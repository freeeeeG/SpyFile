using System;

// Token: 0x0200006B RID: 107
public enum eGameEvents
{
	// Token: 0x040001E3 RID: 483
	None,
	// Token: 0x040001E4 RID: 484
	OnWorldChange,
	// Token: 0x040001E5 RID: 485
	UpdateEnvSceneBindings,
	// Token: 0x040001E6 RID: 486
	MoveCameraToOffset,
	// Token: 0x040001E7 RID: 487
	RequestCameraShake,
	// Token: 0x040001E8 RID: 488
	OnLanguageChanged,
	// Token: 0x040001E9 RID: 489
	OnGameInitReady,
	// Token: 0x040001EA RID: 490
	RequestChangeGameState,
	// Token: 0x040001EB RID: 491
	GameStateChanged,
	// Token: 0x040001EC RID: 492
	OnPauseGame,
	// Token: 0x040001ED RID: 493
	RequestEndPause,
	// Token: 0x040001EE RID: 494
	RequestChangeBattleState,
	// Token: 0x040001EF RID: 495
	OnUpdateRoundTimer,
	// Token: 0x040001F0 RID: 496
	OnRoundStart,
	// Token: 0x040001F1 RID: 497
	OnBattleStart,
	// Token: 0x040001F2 RID: 498
	OnBattleEnd,
	// Token: 0x040001F3 RID: 499
	OnRoundEnd,
	// Token: 0x040001F4 RID: 500
	UI_ToggleRoundTimerUI,
	// Token: 0x040001F5 RID: 501
	OnPlayerVictory,
	// Token: 0x040001F6 RID: 502
	OnPlayerDefeat,
	// Token: 0x040001F7 RID: 503
	RequestModifySystemGameSpeed,
	// Token: 0x040001F8 RID: 504
	RequestModifyBattleGameSpeed,
	// Token: 0x040001F9 RID: 505
	RequestModifyBasicGameSpeed,
	// Token: 0x040001FA RID: 506
	RequestModifyDebugGameSpeed,
	// Token: 0x040001FB RID: 507
	RequestDiscoverReward,
	// Token: 0x040001FC RID: 508
	OnDiscoverRewardComplete,
	// Token: 0x040001FD RID: 509
	OnDiscoverRewardSelected,
	// Token: 0x040001FE RID: 510
	UI_ShowDiscoverReward,
	// Token: 0x040001FF RID: 511
	RequestSetTutorialStageCompleted,
	// Token: 0x04000200 RID: 512
	QueueTutorialForGameStart,
	// Token: 0x04000201 RID: 513
	QueueTutorialForEvent,
	// Token: 0x04000202 RID: 514
	RequestStartQueuedTutorial,
	// Token: 0x04000203 RID: 515
	RequestTutorial,
	// Token: 0x04000204 RID: 516
	RequestSetTutorialFinished,
	// Token: 0x04000205 RID: 517
	RequestRecordTowerBuilt,
	// Token: 0x04000206 RID: 518
	RequestAddGem,
	// Token: 0x04000207 RID: 519
	RequestSetGem,
	// Token: 0x04000208 RID: 520
	OnGemChanged,
	// Token: 0x04000209 RID: 521
	RequestAddExp,
	// Token: 0x0400020A RID: 522
	RequestSetExp,
	// Token: 0x0400020B RID: 523
	OnExpChanged,
	// Token: 0x0400020C RID: 524
	RequestLearnTalent,
	// Token: 0x0400020D RID: 525
	RequestResetTalent,
	// Token: 0x0400020E RID: 526
	OnTalentChanged,
	// Token: 0x0400020F RID: 527
	RequestAddCoin,
	// Token: 0x04000210 RID: 528
	RequestSetCoin,
	// Token: 0x04000211 RID: 529
	OnCoinChanged,
	// Token: 0x04000212 RID: 530
	RequestAddEnergy,
	// Token: 0x04000213 RID: 531
	RequestSetEnergy,
	// Token: 0x04000214 RID: 532
	OnEnergyChanged,
	// Token: 0x04000215 RID: 533
	RequestOverrideMapHP,
	// Token: 0x04000216 RID: 534
	RequestAddHP,
	// Token: 0x04000217 RID: 535
	RequestSetHP,
	// Token: 0x04000218 RID: 536
	OnHpChanged,
	// Token: 0x04000219 RID: 537
	RequestAddTowerCardLimit,
	// Token: 0x0400021A RID: 538
	RequestAddItemCardLimit,
	// Token: 0x0400021B RID: 539
	RequestResetDrawCardCost,
	// Token: 0x0400021C RID: 540
	OnDrawCardCostChanged,
	// Token: 0x0400021D RID: 541
	RequestAddCardToStorage,
	// Token: 0x0400021E RID: 542
	RequestRemoveCardFromStorage,
	// Token: 0x0400021F RID: 543
	OnStorageChanged,
	// Token: 0x04000220 RID: 544
	RequestAddCardToDeck,
	// Token: 0x04000221 RID: 545
	RequestRemoveCardFromDeck,
	// Token: 0x04000222 RID: 546
	RequestAddDrawCardCount,
	// Token: 0x04000223 RID: 547
	OnDeckChanged,
	// Token: 0x04000224 RID: 548
	RequestDrawCard,
	// Token: 0x04000225 RID: 549
	RequestShuffleDeck,
	// Token: 0x04000226 RID: 550
	RequestAddCardToHand,
	// Token: 0x04000227 RID: 551
	RequestAddCardToHandFromPosition,
	// Token: 0x04000228 RID: 552
	RequestRemoveCardFromHand,
	// Token: 0x04000229 RID: 553
	RequestRemoveExcessHandCard,
	// Token: 0x0400022A RID: 554
	RequestCombineCard,
	// Token: 0x0400022B RID: 555
	RequestRedrawCards,
	// Token: 0x0400022C RID: 556
	RequestDiscardAllCardsFromHand,
	// Token: 0x0400022D RID: 557
	OnHandCardChanged,
	// Token: 0x0400022E RID: 558
	AddCardToHand,
	// Token: 0x0400022F RID: 559
	RequestClearAllTowerCard,
	// Token: 0x04000230 RID: 560
	RequestAddTowerCard,
	// Token: 0x04000231 RID: 561
	RequestReplaceTowerCard,
	// Token: 0x04000232 RID: 562
	RequestRemoveTowerCard,
	// Token: 0x04000233 RID: 563
	RequestLevelUpTowerCard,
	// Token: 0x04000234 RID: 564
	UI_ForceUpdateAllTowerCard,
	// Token: 0x04000235 RID: 565
	OnTowerCardChanged,
	// Token: 0x04000236 RID: 566
	RequestOverrideTowerLoadout,
	// Token: 0x04000237 RID: 567
	RequestStartPlacement,
	// Token: 0x04000238 RID: 568
	ConfirmPlacement,
	// Token: 0x04000239 RID: 569
	CancelPlacement,
	// Token: 0x0400023A RID: 570
	RequestRotatePlacement,
	// Token: 0x0400023B RID: 571
	OnTetrisPlaced,
	// Token: 0x0400023C RID: 572
	OnTowerPlaced,
	// Token: 0x0400023D RID: 573
	RegisterDynamicPlacementObject,
	// Token: 0x0400023E RID: 574
	UnregisterDynamicPlacementObject,
	// Token: 0x0400023F RID: 575
	ToggleRangeIndicator,
	// Token: 0x04000240 RID: 576
	SetupRangeIndicator,
	// Token: 0x04000241 RID: 577
	LockRangeIndicator,
	// Token: 0x04000242 RID: 578
	RequestGiveBuffToTower,
	// Token: 0x04000243 RID: 579
	RequestStartBuffSelection,
	// Token: 0x04000244 RID: 580
	ConfirmBuffSelection,
	// Token: 0x04000245 RID: 581
	CancelBuffSelection,
	// Token: 0x04000246 RID: 582
	RequestUpgradeTower,
	// Token: 0x04000247 RID: 583
	RequestSellTower,
	// Token: 0x04000248 RID: 584
	RequestStartRandomPlacement,
	// Token: 0x04000249 RID: 585
	OnGridObjectChanged,
	// Token: 0x0400024A RID: 586
	OnGraphUpdated,
	// Token: 0x0400024B RID: 587
	OnFloodPathUpdate,
	// Token: 0x0400024C RID: 588
	RequestSceneTimeChange,
	// Token: 0x0400024D RID: 589
	StartSceneTimeChange,
	// Token: 0x0400024E RID: 590
	RoundTimeFastForwardToNight,
	// Token: 0x0400024F RID: 591
	MonsterSpawn,
	// Token: 0x04000250 RID: 592
	MonsterImpendingDeath,
	// Token: 0x04000251 RID: 593
	MonsterKilled,
	// Token: 0x04000252 RID: 594
	MonsterDespawn,
	// Token: 0x04000253 RID: 595
	MonsterDealDamageToPlayer,
	// Token: 0x04000254 RID: 596
	RequestStartNextWave,
	// Token: 0x04000255 RID: 597
	OnWaveIndexChanged,
	// Token: 0x04000256 RID: 598
	RequestSpawnMonster,
	// Token: 0x04000257 RID: 599
	SetNextWaveSpawnIndex,
	// Token: 0x04000258 RID: 600
	UI_UpdateNextWaveMonster,
	// Token: 0x04000259 RID: 601
	UI_ToggleNextWaveMonsterUI,
	// Token: 0x0400025A RID: 602
	OnClickTowerOnField,
	// Token: 0x0400025B RID: 603
	OnClickTetrisOnField,
	// Token: 0x0400025C RID: 604
	AchievementUnlock,
	// Token: 0x0400025D RID: 605
	UI_ShowAchievementUnlock,
	// Token: 0x0400025E RID: 606
	UI_ShowDamageNumber,
	// Token: 0x0400025F RID: 607
	UI_ShowBuffApplyText,
	// Token: 0x04000260 RID: 608
	UI_ShowSettingPage,
	// Token: 0x04000261 RID: 609
	UI_ShowStageAnnounce,
	// Token: 0x04000262 RID: 610
	UI_TogglePauseMenu,
	// Token: 0x04000263 RID: 611
	UI_ToggleMouseTooltip,
	// Token: 0x04000264 RID: 612
	UI_SetMouseTooltipTarget,
	// Token: 0x04000265 RID: 613
	UI_SetMouseTooltipContent,
	// Token: 0x04000266 RID: 614
	UI_ToggleControlTip,
	// Token: 0x04000267 RID: 615
	UI_ToggleControlTipError_PathBlocked,
	// Token: 0x04000268 RID: 616
	UI_ToggleCraftTowerUI,
	// Token: 0x04000269 RID: 617
	UI_RequestUpdateCraftTowerTooltip,
	// Token: 0x0400026A RID: 618
	UI_OnCardRemoveFromCraftTowerUI,
	// Token: 0x0400026B RID: 619
	UI_TriggerTransition_Show,
	// Token: 0x0400026C RID: 620
	UI_TriggerTransition_Hide,
	// Token: 0x0400026D RID: 621
	UI_SetPlacementPointerArrowTarget,
	// Token: 0x0400026E RID: 622
	UI_TogglePlacementPointerArrow,
	// Token: 0x0400026F RID: 623
	UI_WaveClearUI_Show,
	// Token: 0x04000270 RID: 624
	UI_VictoryUICompleted,
	// Token: 0x04000271 RID: 625
	UI_ShowWelcomeToDemoUI,
	// Token: 0x04000272 RID: 626
	TriggerNotification,
	// Token: 0x04000273 RID: 627
	RequestAddOutline,
	// Token: 0x04000274 RID: 628
	RequestAddOutlineByList,
	// Token: 0x04000275 RID: 629
	RequestRemoveOutline,
	// Token: 0x04000276 RID: 630
	RequestRemoveOutlineByList,
	// Token: 0x04000277 RID: 631
	RequestRemoveOutlineByListAndType,
	// Token: 0x04000278 RID: 632
	PhysicsInteraction_Explosion,
	// Token: 0x04000279 RID: 633
	PhysicsInteraction_Flame
}
