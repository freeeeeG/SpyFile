using System;
using TUNING;

namespace STRINGS
{
	// Token: 0x02000DA4 RID: 3492
	public class BUILDINGS
	{
		// Token: 0x02001CCD RID: 7373
		public class PREFABS
		{
			// Token: 0x02002264 RID: 8804
			public class HEADQUARTERSCOMPLETE
			{
				// Token: 0x040099B3 RID: 39347
				public static LocString NAME = UI.FormatAsLink("Printing Pod", "HEADQUARTERS");

				// Token: 0x040099B4 RID: 39348
				public static LocString UNIQUE_POPTEXT = "A clone of the cloning machine? What a novel thought.\n\nAlas, it won't work.";
			}

			// Token: 0x02002265 RID: 8805
			public class EXOBASEHEADQUARTERS
			{
				// Token: 0x040099B5 RID: 39349
				public static LocString NAME = UI.FormatAsLink("Mini-Pod", "EXOBASEHEADQUARTERS");

				// Token: 0x040099B6 RID: 39350
				public static LocString DESC = "A quick and easy substitute, though it'll never live up to the original.";

				// Token: 0x040099B7 RID: 39351
				public static LocString EFFECT = "A portable bioprinter that produces new Duplicants or care packages containing resources.\n\nOnly one Printing Pod or Mini-Pod is permitted per Planetoid.";
			}

			// Token: 0x02002266 RID: 8806
			public class AIRCONDITIONER
			{
				// Token: 0x040099B8 RID: 39352
				public static LocString NAME = UI.FormatAsLink("Thermo Regulator", "AIRCONDITIONER");

				// Token: 0x040099B9 RID: 39353
				public static LocString DESC = "A thermo regulator doesn't remove heat, but relocates it to a new area.";

				// Token: 0x040099BA RID: 39354
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Cools the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" piped through it, but outputs ",
					UI.FormatAsLink("Heat", "HEAT"),
					" in its immediate vicinity."
				});
			}

			// Token: 0x02002267 RID: 8807
			public class STATERPILLAREGG
			{
				// Token: 0x040099BB RID: 39355
				public static LocString NAME = UI.FormatAsLink("Slug Egg", "STATERPILLAREGG");

				// Token: 0x040099BC RID: 39356
				public static LocString DESC = "The electrifying egg of the " + UI.FormatAsLink("Plug Slug", "STATERPILLAR") + ".";

				// Token: 0x040099BD RID: 39357
				public static LocString EFFECT = "Slug Eggs can be connected to a " + UI.FormatAsLink("Power", "POWER") + " circuit as an energy source.";
			}

			// Token: 0x02002268 RID: 8808
			public class STATERPILLARGENERATOR
			{
				// Token: 0x040099BE RID: 39358
				public static LocString NAME = UI.FormatAsLink("Plug Slug", "STATERPILLAR");

				// Token: 0x02002F6C RID: 12140
				public class MODIFIERS
				{
					// Token: 0x0400C1C4 RID: 49604
					public static LocString WILD = "Wild!";

					// Token: 0x0400C1C5 RID: 49605
					public static LocString HUNGRY = "Hungry!";
				}
			}

			// Token: 0x02002269 RID: 8809
			public class BEEHIVE
			{
				// Token: 0x040099BF RID: 39359
				public static LocString NAME = UI.FormatAsLink("Beeta Hive", "BEEHIVE");

				// Token: 0x040099C0 RID: 39360
				public static LocString DESC = string.Concat(new string[]
				{
					"A moderately ",
					UI.FormatAsLink("Radioactive", "RADIATION"),
					" nest made by ",
					UI.FormatAsLink("Beetas", "BEE"),
					".\n\nConverts ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					" into ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" when worked by a Beeta.\nWill not function if ground below has been destroyed."
				});

				// Token: 0x040099C1 RID: 39361
				public static LocString EFFECT = "The cozy home of a Beeta.";
			}

			// Token: 0x0200226A RID: 8810
			public class ETHANOLDISTILLERY
			{
				// Token: 0x040099C2 RID: 39362
				public static LocString NAME = UI.FormatAsLink("Ethanol Distiller", "ETHANOLDISTILLERY");

				// Token: 0x040099C3 RID: 39363
				public static LocString DESC = string.Concat(new string[]
				{
					"Ethanol distillers convert ",
					ITEMS.INDUSTRIAL_PRODUCTS.WOOD.NAME,
					" into burnable ",
					ELEMENTS.ETHANOL.NAME,
					" fuel."
				});

				// Token: 0x040099C4 RID: 39364
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Refines ",
					UI.FormatAsLink("Lumber", "WOOD"),
					" into ",
					UI.FormatAsLink("Ethanol", "ETHANOL"),
					"."
				});
			}

			// Token: 0x0200226B RID: 8811
			public class ALGAEDISTILLERY
			{
				// Token: 0x040099C5 RID: 39365
				public static LocString NAME = UI.FormatAsLink("Algae Distiller", "ALGAEDISTILLERY");

				// Token: 0x040099C6 RID: 39366
				public static LocString DESC = "Algae distillers convert disease-causing slime into algae for oxygen production.";

				// Token: 0x040099C7 RID: 39367
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Refines ",
					UI.FormatAsLink("Slime", "SLIMEMOLD"),
					" into ",
					UI.FormatAsLink("Algae", "ALGAE"),
					"."
				});
			}

			// Token: 0x0200226C RID: 8812
			public class OXYLITEREFINERY
			{
				// Token: 0x040099C8 RID: 39368
				public static LocString NAME = UI.FormatAsLink("Oxylite Refinery", "OXYLITEREFINERY");

				// Token: 0x040099C9 RID: 39369
				public static LocString DESC = "Oxylite is a solid and easily transportable source of consumable oxygen.";

				// Token: 0x040099CA RID: 39370
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Synthesizes ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" using ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and a small amount of ",
					UI.FormatAsLink("Gold", "GOLD"),
					"."
				});
			}

			// Token: 0x0200226D RID: 8813
			public class FERTILIZERMAKER
			{
				// Token: 0x040099CB RID: 39371
				public static LocString NAME = UI.FormatAsLink("Fertilizer Synthesizer", "FERTILIZERMAKER");

				// Token: 0x040099CC RID: 39372
				public static LocString DESC = "Fertilizer synthesizers convert polluted dirt into fertilizer for domestic plants.";

				// Token: 0x040099CD RID: 39373
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" and ",
					UI.FormatAsLink("Phosphorite", "PHOSPHORITE"),
					" to produce ",
					UI.FormatAsLink("Fertilizer", "FERTILIZER"),
					"."
				});
			}

			// Token: 0x0200226E RID: 8814
			public class ALGAEHABITAT
			{
				// Token: 0x040099CE RID: 39374
				public static LocString NAME = UI.FormatAsLink("Algae Terrarium", "ALGAEHABITAT");

				// Token: 0x040099CF RID: 39375
				public static LocString DESC = "Algae colony, Duplicant colony... we're more alike than we are different.";

				// Token: 0x040099D0 RID: 39376
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Consumes ",
					UI.FormatAsLink("Algae", "ALGAE"),
					" to produce ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and remove some ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					".\n\nGains a 10% efficiency boost in direct ",
					UI.FormatAsLink("Light", "LIGHT"),
					"."
				});

				// Token: 0x040099D1 RID: 39377
				public static LocString SIDESCREEN_TITLE = "Empty " + UI.FormatAsLink("Polluted Water", "DIRTYWATER") + " Threshold";
			}

			// Token: 0x0200226F RID: 8815
			public class BATTERY
			{
				// Token: 0x040099D2 RID: 39378
				public static LocString NAME = UI.FormatAsLink("Battery", "BATTERY");

				// Token: 0x040099D3 RID: 39379
				public static LocString DESC = "Batteries allow power from generators to be stored for later.";

				// Token: 0x040099D4 RID: 39380
				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Power", "POWER") + " from generators, then provides that power to buildings.\n\nLoses charge over time.";

				// Token: 0x040099D5 RID: 39381
				public static LocString CHARGE_LOSS = "{Battery} charge loss";
			}

			// Token: 0x02002270 RID: 8816
			public class FLYINGCREATUREBAIT
			{
				// Token: 0x040099D6 RID: 39382
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Bait", "FLYINGCREATUREBAIT");

				// Token: 0x040099D7 RID: 39383
				public static LocString DESC = "The type of critter attracted by critter bait depends on the construction material.";

				// Token: 0x040099D8 RID: 39384
				public static LocString EFFECT = "Attracts one type of airborne critter.\n\nSingle use.";
			}

			// Token: 0x02002271 RID: 8817
			public class WATERTRAP
			{
				// Token: 0x040099D9 RID: 39385
				public static LocString NAME = "Fish Trap";

				// Token: 0x040099DA RID: 39386
				public static LocString DESC = "Trapped fish will automatically be bagged for transport.";

				// Token: 0x040099DB RID: 39387
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Attracts and traps swimming ",
					UI.FormatAsLink("Pacu", "PACU"),
					".\n\nOnly Duplicants with the ",
					UI.FormatAsLink("Critter Ranching I", "RANCHING1"),
					" skill can arm this trap. It's reusable!"
				});
			}

			// Token: 0x02002272 RID: 8818
			public class REUSABLETRAP
			{
				// Token: 0x040099DC RID: 39388
				public static LocString LOGIC_PORT = "Trap Occupied";

				// Token: 0x040099DD RID: 39389
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when a critter has been trapped";

				// Token: 0x040099DE RID: 39390
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x040099DF RID: 39391
				public static LocString INPUT_LOGIC_PORT = "Trap Setter";

				// Token: 0x040099E0 RID: 39392
				public static LocString INPUT_LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set trap";

				// Token: 0x040099E1 RID: 39393
				public static LocString INPUT_LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disarm and empty trap";
			}

			// Token: 0x02002273 RID: 8819
			public class CREATUREAIRTRAP
			{
				// Token: 0x040099E2 RID: 39394
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Trap", "FLYINGCREATUREBAIT");

				// Token: 0x040099E3 RID: 39395
				public static LocString DESC = "It needs to be armed prior to use.";

				// Token: 0x040099E4 RID: 39396
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Attracts and captures airborne ",
					UI.FormatAsLink("Critters", "CREATURES"),
					".\n\nOnly Duplicants with the ",
					UI.FormatAsLink("Critter Ranching I", "RANCHING1"),
					" skill can arm this trap. It's reusable!"
				});
			}

			// Token: 0x02002274 RID: 8820
			public class AIRBORNECREATURELURE
			{
				// Token: 0x040099E5 RID: 39397
				public static LocString NAME = UI.FormatAsLink("Airborne Critter Lure", "AIRBORNECREATURELURE");

				// Token: 0x040099E6 RID: 39398
				public static LocString DESC = "Lures can relocate Pufts or Shine Bugs to specific locations in my colony.";

				// Token: 0x040099E7 RID: 39399
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Attracts one type of airborne critter at a time.\n\nMust be baited with ",
					UI.FormatAsLink("Slime", "SLIMEMOLD"),
					" or ",
					UI.FormatAsLink("Phosphorite", "PHOSPHORITE"),
					"."
				});
			}

			// Token: 0x02002275 RID: 8821
			public class BATTERYMEDIUM
			{
				// Token: 0x040099E8 RID: 39400
				public static LocString NAME = UI.FormatAsLink("Jumbo Battery", "BATTERYMEDIUM");

				// Token: 0x040099E9 RID: 39401
				public static LocString DESC = "Larger batteries hold more power and keep systems running longer before recharging.";

				// Token: 0x040099EA RID: 39402
				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Power", "POWER") + " from generators, then provides that power to buildings.\n\nSlightly loses charge over time.";
			}

			// Token: 0x02002276 RID: 8822
			public class BATTERYSMART
			{
				// Token: 0x040099EB RID: 39403
				public static LocString NAME = UI.FormatAsLink("Smart Battery", "BATTERYSMART");

				// Token: 0x040099EC RID: 39404
				public static LocString DESC = "Smart batteries send a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when they require charging.";

				// Token: 0x040099ED RID: 39405
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Power", "POWER"),
					" from generators, then provides that power to buildings.\n\nSends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the configuration of the Logic Activation Parameters.\n\nVery slightly loses charge over time."
				});

				// Token: 0x040099EE RID: 39406
				public static LocString LOGIC_PORT = "Charge Parameters";

				// Token: 0x040099EF RID: 39407
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when battery is less than <b>Low Threshold</b> charged, until <b>High Threshold</b> is reached again";

				// Token: 0x040099F0 RID: 39408
				public static LocString LOGIC_PORT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when the battery is more than <b>High Threshold</b> charged, until <b>Low Threshold</b> is reached again";

				// Token: 0x040099F1 RID: 39409
				public static LocString ACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when battery is less than <b>{0}%</b> charged, until it is <b>{1}% (High Threshold)</b> charged";

				// Token: 0x040099F2 RID: 39410
				public static LocString DEACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when battery is <b>{0}%</b> charged, until it is less than <b>{1}% (Low Threshold)</b> charged";

				// Token: 0x040099F3 RID: 39411
				public static LocString SIDESCREEN_TITLE = "Logic Activation Parameters";

				// Token: 0x040099F4 RID: 39412
				public static LocString SIDESCREEN_ACTIVATE = "Low Threshold:";

				// Token: 0x040099F5 RID: 39413
				public static LocString SIDESCREEN_DEACTIVATE = "High Threshold:";
			}

			// Token: 0x02002277 RID: 8823
			public class BED
			{
				// Token: 0x040099F6 RID: 39414
				public static LocString NAME = UI.FormatAsLink("Cot", "BED");

				// Token: 0x040099F7 RID: 39415
				public static LocString DESC = "Duplicants without a bed will develop sore backs from sleeping on the floor.";

				// Token: 0x040099F8 RID: 39416
				public static LocString EFFECT = "Gives one Duplicant a place to sleep.\n\nDuplicants will automatically return to their cots to sleep at night.";

				// Token: 0x02002F6D RID: 12141
				public class FACADES
				{
					// Token: 0x020031F8 RID: 12792
					public class DEFAULT_BED
					{
						// Token: 0x0400C852 RID: 51282
						public static LocString NAME = UI.FormatAsLink("Cot", "BED");

						// Token: 0x0400C853 RID: 51283
						public static LocString DESC = "A safe place to sleep.";
					}

					// Token: 0x020031F9 RID: 12793
					public class STARCURTAIN
					{
						// Token: 0x0400C854 RID: 51284
						public static LocString NAME = UI.FormatAsLink("Stargazer Cot", "BED");

						// Token: 0x0400C855 RID: 51285
						public static LocString DESC = "Now Duplicants can sleep beneath the stars without wearing an Atmo Suit to bed.";
					}

					// Token: 0x020031FA RID: 12794
					public class SCIENCELAB
					{
						// Token: 0x0400C856 RID: 51286
						public static LocString NAME = UI.FormatAsLink("Lab Cot", "BED");

						// Token: 0x0400C857 RID: 51287
						public static LocString DESC = "For the Duplicant who dreams of scientific discoveries.";
					}

					// Token: 0x020031FB RID: 12795
					public class STAYCATION
					{
						// Token: 0x0400C858 RID: 51288
						public static LocString NAME = UI.FormatAsLink("Staycation Cot", "BED");

						// Token: 0x0400C859 RID: 51289
						public static LocString DESC = "Like a weekend away, except... not.";
					}

					// Token: 0x020031FC RID: 12796
					public class CREAKY
					{
						// Token: 0x0400C85A RID: 51290
						public static LocString NAME = UI.FormatAsLink("Camping Cot", "BED");

						// Token: 0x0400C85B RID: 51291
						public static LocString DESC = "It's sturdier than it looks.";
					}
				}
			}

			// Token: 0x02002278 RID: 8824
			public class BOTTLEEMPTIER
			{
				// Token: 0x040099F9 RID: 39417
				public static LocString NAME = UI.FormatAsLink("Bottle Emptier", "BOTTLEEMPTIER");

				// Token: 0x040099FA RID: 39418
				public static LocString DESC = "A bottle emptier's Element Filter can be used to designate areas for specific liquid storage.";

				// Token: 0x040099FB RID: 39419
				public static LocString EFFECT = "Empties bottled " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + " back into the world.";
			}

			// Token: 0x02002279 RID: 8825
			public class BOTTLEEMPTIERGAS
			{
				// Token: 0x040099FC RID: 39420
				public static LocString NAME = UI.FormatAsLink("Canister Emptier", "BOTTLEEMPTIERGAS");

				// Token: 0x040099FD RID: 39421
				public static LocString DESC = "A canister emptier's Element Filter can designate areas for specific gas storage.";

				// Token: 0x040099FE RID: 39422
				public static LocString EFFECT = "Empties " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " canisters back into the world.";
			}

			// Token: 0x0200227A RID: 8826
			public class ARTIFACTCARGOBAY
			{
				// Token: 0x040099FF RID: 39423
				public static LocString NAME = UI.FormatAsLink("Artifact Transport Module", "ARTIFACTCARGOBAY");

				// Token: 0x04009A00 RID: 39424
				public static LocString DESC = "Holds artifacts found in space.";

				// Token: 0x04009A01 RID: 39425
				public static LocString EFFECT = "Allows Duplicants to store any artifacts they uncover during space missions.\n\nArtifacts become available to the colony upon the rocket's return. \n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".";
			}

			// Token: 0x0200227B RID: 8827
			public class CARGOBAY
			{
				// Token: 0x04009A02 RID: 39426
				public static LocString NAME = UI.FormatAsLink("Cargo Bay", "CARGOBAY");

				// Token: 0x04009A03 RID: 39427
				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				// Token: 0x04009A04 RID: 39428
				public static LocString EFFECT = "Allows Duplicants to store any " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			// Token: 0x0200227C RID: 8828
			public class CARGOBAYCLUSTER
			{
				// Token: 0x04009A05 RID: 39429
				public static LocString NAME = UI.FormatAsLink("Large Cargo Bay", "CARGOBAY");

				// Token: 0x04009A06 RID: 39430
				public static LocString DESC = "Holds more than a regular cargo bay.";

				// Token: 0x04009A07 RID: 39431
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store most of the ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x0200227D RID: 8829
			public class SOLIDCARGOBAYSMALL
			{
				// Token: 0x04009A08 RID: 39432
				public static LocString NAME = UI.FormatAsLink("Cargo Bay", "SOLIDCARGOBAYSMALL");

				// Token: 0x04009A09 RID: 39433
				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				// Token: 0x04009A0A RID: 39434
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store some of the ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x0200227E RID: 8830
			public class SPECIALCARGOBAY
			{
				// Token: 0x04009A0B RID: 39435
				public static LocString NAME = UI.FormatAsLink("Biological Cargo Bay", "SPECIALCARGOBAY");

				// Token: 0x04009A0C RID: 39436
				public static LocString DESC = "Biological cargo bays allow Duplicants to retrieve alien plants and wildlife from space.";

				// Token: 0x04009A0D RID: 39437
				public static LocString EFFECT = "Allows Duplicants to store unusual or organic resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			// Token: 0x0200227F RID: 8831
			public class SPECIALCARGOBAYCLUSTER
			{
				// Token: 0x04009A0E RID: 39438
				public static LocString NAME = UI.FormatAsLink("Critter Cargo Bay", "SPECIALCARGOBAY");

				// Token: 0x04009A0F RID: 39439
				public static LocString DESC = "Critters do not require feeding during transit.";

				// Token: 0x04009A10 RID: 39440
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to transport ",
					UI.CODEX.CATEGORYNAMES.CREATURES,
					" through space.\n\nSpecimens can be released into the colony upon the rocket's return.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});

				// Token: 0x04009A11 RID: 39441
				public static LocString RELEASE_BTN = "Release Critter";

				// Token: 0x04009A12 RID: 39442
				public static LocString RELEASE_BTN_TOOLTIP = "Release the critter stored inside";
			}

			// Token: 0x02002280 RID: 8832
			public class COMMANDMODULE
			{
				// Token: 0x04009A13 RID: 39443
				public static LocString NAME = UI.FormatAsLink("Command Capsule", "COMMANDMODULE");

				// Token: 0x04009A14 RID: 39444
				public static LocString DESC = "At least one astronaut must be assigned to the command module to pilot a rocket.";

				// Token: 0x04009A15 RID: 39445
				public static LocString EFFECT = "Contains passenger seating for Duplicant " + UI.FormatAsLink("Astronauts", "ASTRONAUTING1") + ".\n\nA Command Capsule must be the last module installed at the top of a rocket.";

				// Token: 0x04009A16 RID: 39446
				public static LocString LOGIC_PORT_READY = "Rocket Checklist";

				// Token: 0x04009A17 RID: 39447
				public static LocString LOGIC_PORT_READY_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket launch checklist is complete";

				// Token: 0x04009A18 RID: 39448
				public static LocString LOGIC_PORT_READY_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x04009A19 RID: 39449
				public static LocString LOGIC_PORT_LAUNCH = "Launch Rocket";

				// Token: 0x04009A1A RID: 39450
				public static LocString LOGIC_PORT_LAUNCH_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket";

				// Token: 0x04009A1B RID: 39451
				public static LocString LOGIC_PORT_LAUNCH_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Awaits launch command";
			}

			// Token: 0x02002281 RID: 8833
			public class CLUSTERCOMMANDMODULE
			{
				// Token: 0x04009A1C RID: 39452
				public static LocString NAME = UI.FormatAsLink("Command Capsule", "CLUSTERCOMMANDMODULE");

				// Token: 0x04009A1D RID: 39453
				public static LocString DESC = "";

				// Token: 0x04009A1E RID: 39454
				public static LocString EFFECT = "";

				// Token: 0x04009A1F RID: 39455
				public static LocString LOGIC_PORT_READY = "Rocket Checklist";

				// Token: 0x04009A20 RID: 39456
				public static LocString LOGIC_PORT_READY_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket launch checklist is complete";

				// Token: 0x04009A21 RID: 39457
				public static LocString LOGIC_PORT_READY_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x04009A22 RID: 39458
				public static LocString LOGIC_PORT_LAUNCH = "Launch Rocket";

				// Token: 0x04009A23 RID: 39459
				public static LocString LOGIC_PORT_LAUNCH_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket";

				// Token: 0x04009A24 RID: 39460
				public static LocString LOGIC_PORT_LAUNCH_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Awaits launch command";
			}

			// Token: 0x02002282 RID: 8834
			public class CLUSTERCRAFTINTERIORDOOR
			{
				// Token: 0x04009A25 RID: 39461
				public static LocString NAME = UI.FormatAsLink("Interior Hatch", "CLUSTERCRAFTINTERIORDOOR");

				// Token: 0x04009A26 RID: 39462
				public static LocString DESC = "A hatch for getting in and out of the rocket.";

				// Token: 0x04009A27 RID: 39463
				public static LocString EFFECT = "Warning: Do not open mid-flight.";
			}

			// Token: 0x02002283 RID: 8835
			public class ROCKETCONTROLSTATION
			{
				// Token: 0x04009A28 RID: 39464
				public static LocString NAME = UI.FormatAsLink("Rocket Control Station", "ROCKETCONTROLSTATION");

				// Token: 0x04009A29 RID: 39465
				public static LocString DESC = "Someone needs to be around to jiggle the controls when the screensaver comes on.";

				// Token: 0x04009A2A RID: 39466
				public static LocString EFFECT = "Allows Duplicants to use pilot-operated rockets and control access to interior buildings.\n\nAssigned Duplicants must have the " + UI.FormatAsLink("Rocket Piloting", "ROCKETPILOTING1") + " skill.";

				// Token: 0x04009A2B RID: 39467
				public static LocString LOGIC_PORT = "Restrict Building Usage";

				// Token: 0x04009A2C RID: 39468
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Restrict access to interior buildings";

				// Token: 0x04009A2D RID: 39469
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Unrestrict access to interior buildings";
			}

			// Token: 0x02002284 RID: 8836
			public class RESEARCHMODULE
			{
				// Token: 0x04009A2E RID: 39470
				public static LocString NAME = UI.FormatAsLink("Research Module", "RESEARCHMODULE");

				// Token: 0x04009A2F RID: 39471
				public static LocString DESC = "Data banks can be used at virtual planetariums to produce additional research.";

				// Token: 0x04009A30 RID: 39472
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Completes one ",
					UI.FormatAsLink("Research Task", "RESEARCH"),
					" per space mission.\n\nProduces a small Data Bank regardless of mission destination.\n\nGenerated ",
					UI.FormatAsLink("Research Points", "RESEARCH"),
					" become available upon the rocket's return."
				});
			}

			// Token: 0x02002285 RID: 8837
			public class TOURISTMODULE
			{
				// Token: 0x04009A31 RID: 39473
				public static LocString NAME = UI.FormatAsLink("Sight-Seeing Module", "TOURISTMODULE");

				// Token: 0x04009A32 RID: 39474
				public static LocString DESC = "An astronaut must accompany sight seeing Duplicants on rocket flights.";

				// Token: 0x04009A33 RID: 39475
				public static LocString EFFECT = "Allows one non-Astronaut Duplicant to visit space.\n\nSight-Seeing Rocket flights decrease " + UI.FormatAsLink("Stress", "STRESS") + ".";
			}

			// Token: 0x02002286 RID: 8838
			public class SCANNERMODULE
			{
				// Token: 0x04009A34 RID: 39476
				public static LocString NAME = UI.FormatAsLink("Cartographic Module", "SCANNERMODULE");

				// Token: 0x04009A35 RID: 39477
				public static LocString DESC = "Allows Duplicants to boldly go where other Duplicants haven't been yet.";

				// Token: 0x04009A36 RID: 39478
				public static LocString EFFECT = "Automatically analyzes adjacent space while on a voyage. \n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".";
			}

			// Token: 0x02002287 RID: 8839
			public class HABITATMODULESMALL
			{
				// Token: 0x04009A37 RID: 39479
				public static LocString NAME = UI.FormatAsLink("Solo Spacefarer Nosecone", "HABITATMODULESMALL");

				// Token: 0x04009A38 RID: 39480
				public static LocString DESC = "One lucky Duplicant gets the best view from the whole rocket.";

				// Token: 0x04009A39 RID: 39481
				public static LocString EFFECT = "Functions as a Command Module and a Nosecone.\n\nHolds one Duplicant traveller.\n\nOne Command Module may be installed per rocket.\n\nMust be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ". \n\nMust be built at the top of a rocket.";
			}

			// Token: 0x02002288 RID: 8840
			public class HABITATMODULEMEDIUM
			{
				// Token: 0x04009A3A RID: 39482
				public static LocString NAME = UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM");

				// Token: 0x04009A3B RID: 39483
				public static LocString DESC = "Allows Duplicants to survive space travel... Hopefully.";

				// Token: 0x04009A3C RID: 39484
				public static LocString EFFECT = "Functions as a Command Module.\n\nHolds up to ten Duplicant travellers.\n\nOne Command Module may be installed per rocket. \n\nEngine must be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".";
			}

			// Token: 0x02002289 RID: 8841
			public class NOSECONEBASIC
			{
				// Token: 0x04009A3D RID: 39485
				public static LocString NAME = UI.FormatAsLink("Basic Nosecone", "NOSECONEBASIC");

				// Token: 0x04009A3E RID: 39486
				public static LocString DESC = "Every rocket requires a nosecone to fly.";

				// Token: 0x04009A3F RID: 39487
				public static LocString EFFECT = "Protects a rocket during takeoff and entry, enabling space travel.\n\nEngine must be built via " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ". \n\nMust be built at the top of a rocket.";
			}

			// Token: 0x0200228A RID: 8842
			public class NOSECONEHARVEST
			{
				// Token: 0x04009A40 RID: 39488
				public static LocString NAME = UI.FormatAsLink("Drillcone", "NOSECONEHARVEST");

				// Token: 0x04009A41 RID: 39489
				public static LocString DESC = "Harvests resources from the universe.";

				// Token: 0x04009A42 RID: 39490
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Enables a rocket to drill into interstellar debris and collect ",
					UI.FormatAsLink("gas", "ELEMENTS_GAS"),
					", ",
					UI.FormatAsLink("liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("solid", "ELEMENTS_SOLID"),
					" resources from space.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nMust be built at the top of a rocket with ",
					UI.FormatAsLink("gas", "ELEMENTS_GAS"),
					", ",
					UI.FormatAsLink("liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("solid", "ELEMENTS_SOLID"),
					" Cargo Module attached to store the appropriate resources."
				});
			}

			// Token: 0x0200228B RID: 8843
			public class CO2ENGINE
			{
				// Token: 0x04009A43 RID: 39491
				public static LocString NAME = UI.FormatAsLink("Carbon Dioxide Engine", "CO2ENGINE");

				// Token: 0x04009A44 RID: 39492
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x04009A45 RID: 39493
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses pressurized ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" to propel rockets for short range space exploration.\n\nCarbon Dioxide Engines are relatively fast engine for their size but with limited height restrictions.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x0200228C RID: 8844
			public class KEROSENEENGINE
			{
				// Token: 0x04009A46 RID: 39494
				public static LocString NAME = UI.FormatAsLink("Petroleum Engine", "KEROSENEENGINE");

				// Token: 0x04009A47 RID: 39495
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x04009A48 RID: 39496
				public static LocString EFFECT = "Burns " + UI.FormatAsLink("Petroleum", "PETROLEUM") + " to propel rockets for mid-range space exploration.\n\nPetroleum Engines have generous height restrictions, ideal for hauling many modules.\n\nThe engine must be built first before more rocket modules can be added.";
			}

			// Token: 0x0200228D RID: 8845
			public class KEROSENEENGINECLUSTER
			{
				// Token: 0x04009A49 RID: 39497
				public static LocString NAME = UI.FormatAsLink("Petroleum Engine", "KEROSENEENGINECLUSTER");

				// Token: 0x04009A4A RID: 39498
				public static LocString DESC = "More powerful rocket engines can propel heavier burdens.";

				// Token: 0x04009A4B RID: 39499
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" to propel rockets for mid-range space exploration.\n\nPetroleum Engines have generous height restrictions, ideal for hauling many modules.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x0200228E RID: 8846
			public class KEROSENEENGINECLUSTERSMALL
			{
				// Token: 0x04009A4C RID: 39500
				public static LocString NAME = UI.FormatAsLink("Small Petroleum Engine", "KEROSENEENGINECLUSTERSMALL");

				// Token: 0x04009A4D RID: 39501
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x04009A4E RID: 39502
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" to propel rockets for mid-range space exploration.\n\nSmall Petroleum Engines possess the same speed as a ",
					UI.FormatAsLink("Petroleum Engines", "KEROSENEENGINE"),
					" but have smaller height restrictions.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x0200228F RID: 8847
			public class HYDROGENENGINE
			{
				// Token: 0x04009A4F RID: 39503
				public static LocString NAME = UI.FormatAsLink("Hydrogen Engine", "HYDROGENENGINE");

				// Token: 0x04009A50 RID: 39504
				public static LocString DESC = "Hydrogen engines can propel rockets further than steam or petroleum engines.";

				// Token: 0x04009A51 RID: 39505
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN"),
					" to propel rockets for long-range space exploration.\n\nHydrogen Engines have the same generous height restrictions as ",
					UI.FormatAsLink("Petroleum Engines", "KEROSENEENGINE"),
					" but are slightly faster.\n\nThe engine must be built first before more rocket modules can be added."
				});
			}

			// Token: 0x02002290 RID: 8848
			public class HYDROGENENGINECLUSTER
			{
				// Token: 0x04009A52 RID: 39506
				public static LocString NAME = UI.FormatAsLink("Hydrogen Engine", "HYDROGENENGINE");

				// Token: 0x04009A53 RID: 39507
				public static LocString DESC = "Hydrogen engines can propel rockets further than steam or petroleum engines.";

				// Token: 0x04009A54 RID: 39508
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Liquid Hydrogen", "LIQUIDHYDROGEN"),
					" to propel rockets for long-range space exploration.\n\nHydrogen Engines have the same generous height restrictions as ",
					UI.FormatAsLink("Petroleum Engines", "KEROSENEENGINE"),
					" but are slightly faster.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					".\n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x02002291 RID: 8849
			public class SUGARENGINE
			{
				// Token: 0x04009A55 RID: 39509
				public static LocString NAME = UI.FormatAsLink("Sugar Engine", "SUGARENGINE");

				// Token: 0x04009A56 RID: 39510
				public static LocString DESC = "Not the most stylish way to travel space, but certainly the tastiest.";

				// Token: 0x04009A57 RID: 39511
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Sucrose", "SUCROSE"),
					" to propel rockets for short range space exploration.\n\nSugar Engines have higher height restrictions than ",
					UI.FormatAsLink("Carbon Dioxide Engines", "CO2ENGINE"),
					", but move slower.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x02002292 RID: 8850
			public class HEPENGINE
			{
				// Token: 0x04009A58 RID: 39512
				public static LocString NAME = UI.FormatAsLink("Radbolt Engine", "HEPENGINE");

				// Token: 0x04009A59 RID: 39513
				public static LocString DESC = "Radbolt-fueled rockets support few modules, but travel exceptionally far.";

				// Token: 0x04009A5A RID: 39514
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Injects ",
					UI.FormatAsLink("Radbolts", "RADIATION"),
					" into a reaction chamber to propel rockets for long-range space exploration.\n\nRadbolt Engines are faster than ",
					UI.FormatAsLink("Hydrogen Engines", "HYDROGENENGINE"),
					" but with a more restrictive height allowance.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});

				// Token: 0x04009A5B RID: 39515
				public static LocString LOGIC_PORT_STORAGE = "Radbolt Storage";

				// Token: 0x04009A5C RID: 39516
				public static LocString LOGIC_PORT_STORAGE_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its Radbolt Storage is full";

				// Token: 0x04009A5D RID: 39517
				public static LocString LOGIC_PORT_STORAGE_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002293 RID: 8851
			public class ORBITALCARGOMODULE
			{
				// Token: 0x04009A5E RID: 39518
				public static LocString NAME = UI.FormatAsLink("Orbital Cargo Module", "ORBITALCARGOMODULE");

				// Token: 0x04009A5F RID: 39519
				public static LocString DESC = "It's a generally good idea to pack some supplies when exploring unknown worlds.";

				// Token: 0x04009A60 RID: 39520
				public static LocString EFFECT = "Delivers cargo to the surface of Planetoids that do not yet have a " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ". \n\nMust be built via Rocket Platform.";
			}

			// Token: 0x02002294 RID: 8852
			public class BATTERYMODULE
			{
				// Token: 0x04009A61 RID: 39521
				public static LocString NAME = UI.FormatAsLink("Battery Module", "BATTERYMODULE");

				// Token: 0x04009A62 RID: 39522
				public static LocString DESC = "Charging a battery module before takeoff makes it easier to power buildings during flight.";

				// Token: 0x04009A63 RID: 39523
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores the excess ",
					UI.FormatAsLink("Power", "POWER"),
					" generated by a Rocket Engine or ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					".\n\nProvides stored power to ",
					UI.FormatAsLink("Interior Rocket Outlets", "ROCKETINTERIORPOWERPLUG"),
					".\n\nLoses charge over time. \n\nMust be built via Rocket Platform."
				});
			}

			// Token: 0x02002295 RID: 8853
			public class PIONEERMODULE
			{
				// Token: 0x04009A64 RID: 39524
				public static LocString NAME = UI.FormatAsLink("Trailblazer Module", "PIONEERMODULE");

				// Token: 0x04009A65 RID: 39525
				public static LocString DESC = "That's one small step for Dupekind.";

				// Token: 0x04009A66 RID: 39526
				public static LocString EFFECT = "Enables travel to Planetoids that do not yet have a " + BUILDINGS.PREFABS.LAUNCHPAD.NAME + ".\n\nCan hold one Duplicant traveller.\n\nDeployment is available while in a Starmap hex adjacent to a Planetoid. \n\nMust be built via Rocket Platform.";
			}

			// Token: 0x02002296 RID: 8854
			public class SOLARPANELMODULE
			{
				// Token: 0x04009A67 RID: 39527
				public static LocString NAME = UI.FormatAsLink("Solar Panel Module", "SOLARPANELMODULE");

				// Token: 0x04009A68 RID: 39528
				public static LocString DESC = "Collect solar energy before takeoff and during flight.";

				// Token: 0x04009A69 RID: 39529
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					" for use on rockets.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nMust be exposed to space."
				});
			}

			// Token: 0x02002297 RID: 8855
			public class SCOUTMODULE
			{
				// Token: 0x04009A6A RID: 39530
				public static LocString NAME = UI.FormatAsLink("Rover's Module", "SCOUTMODULE");

				// Token: 0x04009A6B RID: 39531
				public static LocString DESC = "Rover can conduct explorations of planetoids that don't have rocket platforms built.";

				// Token: 0x04009A6C RID: 39532
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Deploys one ",
					UI.FormatAsLink("Rover Bot", "SCOUT"),
					" for remote Planetoid exploration.\n\nDeployment is available while in a Starmap hex adjacent to a Planetoid. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x02002298 RID: 8856
			public class PIONEERLANDER
			{
				// Token: 0x04009A6D RID: 39533
				public static LocString NAME = UI.FormatAsLink("Trailblazer Lander", "PIONEERLANDER");

				// Token: 0x04009A6E RID: 39534
				public static LocString DESC = "Lands a Duplicant on a Planetoid from an orbiting " + BUILDINGS.PREFABS.PIONEERMODULE.NAME + ".";
			}

			// Token: 0x02002299 RID: 8857
			public class SCOUTLANDER
			{
				// Token: 0x04009A6F RID: 39535
				public static LocString NAME = UI.FormatAsLink("Rover's Lander", "SCOUTLANDER");

				// Token: 0x04009A70 RID: 39536
				public static LocString DESC = string.Concat(new string[]
				{
					"Lands ",
					UI.FormatAsLink("Rover", "SCOUT"),
					" on a Planetoid when ",
					BUILDINGS.PREFABS.SCOUTMODULE.NAME,
					" is in orbit."
				});
			}

			// Token: 0x0200229A RID: 8858
			public class GANTRY
			{
				// Token: 0x04009A71 RID: 39537
				public static LocString NAME = UI.FormatAsLink("Gantry", "GANTRY");

				// Token: 0x04009A72 RID: 39538
				public static LocString DESC = "A gantry can be built over rocket pieces where ladders and tile cannot.";

				// Token: 0x04009A73 RID: 39539
				public static LocString EFFECT = "Provides scaffolding across rocket modules to allow Duplicant access.";

				// Token: 0x04009A74 RID: 39540
				public static LocString LOGIC_PORT = "Extend/Retract";

				// Token: 0x04009A75 RID: 39541
				public static LocString LOGIC_PORT_ACTIVE = "<b>Extends gantry</b> when a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " signal is received";

				// Token: 0x04009A76 RID: 39542
				public static LocString LOGIC_PORT_INACTIVE = "<b>Retracts gantry</b> when a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " signal is received";
			}

			// Token: 0x0200229B RID: 8859
			public class ROCKETINTERIORPOWERPLUG
			{
				// Token: 0x04009A77 RID: 39543
				public static LocString NAME = UI.FormatAsLink("Power Outlet Fitting", "ROCKETINTERIORPOWERPLUG");

				// Token: 0x04009A78 RID: 39544
				public static LocString DESC = "Outlets conveniently power buildings inside a cockpit using their rocket's power stores.";

				// Token: 0x04009A79 RID: 39545
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Provides ",
					UI.FormatAsLink("Power", "POWER"),
					" to connected buildings.\n\nPulls power from ",
					UI.FormatAsLink("Battery Modules", "BATTERYMODULE"),
					" and Rocket Engines.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x0200229C RID: 8860
			public class ROCKETINTERIORLIQUIDINPUT
			{
				// Token: 0x04009A7A RID: 39546
				public static LocString NAME = UI.FormatAsLink("Liquid Intake Fitting", "ROCKETINTERIORLIQUIDINPUT");

				// Token: 0x04009A7B RID: 39547
				public static LocString DESC = "Begone, foul waters!";

				// Token: 0x04009A7C RID: 39548
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to be pumped into rocket storage via ",
					UI.FormatAsLink("Pipes", "LIQUIDCONDUIT"),
					".\n\nSends liquid to the first Rocket Module with available space.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x0200229D RID: 8861
			public class ROCKETINTERIORLIQUIDOUTPUT
			{
				// Token: 0x04009A7D RID: 39549
				public static LocString NAME = UI.FormatAsLink("Liquid Output Fitting", "ROCKETINTERIORLIQUIDOUTPUT");

				// Token: 0x04009A7E RID: 39550
				public static LocString DESC = "Now if only we had some water balloons...";

				// Token: 0x04009A7F RID: 39551
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to be drawn from rocket storage via ",
					UI.FormatAsLink("Pipes", "LIQUIDCONDUIT"),
					".\n\nDraws liquid from the first Rocket Module with the requested material.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x0200229E RID: 8862
			public class ROCKETINTERIORGASINPUT
			{
				// Token: 0x04009A80 RID: 39552
				public static LocString NAME = UI.FormatAsLink("Gas Intake Fitting", "ROCKETINTERIORGASINPUT");

				// Token: 0x04009A81 RID: 39553
				public static LocString DESC = "It's basically central-vac.";

				// Token: 0x04009A82 RID: 39554
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" to be pumped into rocket storage via ",
					UI.FormatAsLink("Pipes", "GASCONDUIT"),
					".\n\nSends gas to the first Rocket Module with available space.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x0200229F RID: 8863
			public class ROCKETINTERIORGASOUTPUT
			{
				// Token: 0x04009A83 RID: 39555
				public static LocString NAME = UI.FormatAsLink("Gas Output Fitting", "ROCKETINTERIORGASOUTPUT");

				// Token: 0x04009A84 RID: 39556
				public static LocString DESC = "Refreshing breezes, on-demand.";

				// Token: 0x04009A85 RID: 39557
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" to be drawn from rocket storage via ",
					UI.FormatAsLink("Pipes", "GASCONDUIT"),
					".\n\nDraws gas from the first Rocket Module with the requested material.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x020022A0 RID: 8864
			public class ROCKETINTERIORSOLIDINPUT
			{
				// Token: 0x04009A86 RID: 39558
				public static LocString NAME = UI.FormatAsLink("Conveyor Receptacle Fitting", "ROCKETINTERIORSOLIDINPUT");

				// Token: 0x04009A87 RID: 39559
				public static LocString DESC = "Why organize your shelves when you can just shove everything in here?";

				// Token: 0x04009A88 RID: 39560
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" to be moved into rocket storage via ",
					UI.FormatAsLink("Conveyor Rails", "SOLIDCONDUIT"),
					".\n\nSends solid material to the first Rocket Module with available space.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x020022A1 RID: 8865
			public class ROCKETINTERIORSOLIDOUTPUT
			{
				// Token: 0x04009A89 RID: 39561
				public static LocString NAME = UI.FormatAsLink("Conveyor Loader Fitting", "ROCKETINTERIORSOLIDOUTPUT");

				// Token: 0x04009A8A RID: 39562
				public static LocString DESC = "For accessing your stored luggage mid-flight.";

				// Token: 0x04009A8B RID: 39563
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" to be moved out of rocket storage via ",
					UI.FormatAsLink("Conveyor Rails", "SOLIDCONDUIT"),
					".\n\nDraws solid material from the first Rocket Module with the requested material.\n\nMust be built within the interior of a Rocket Module."
				});
			}

			// Token: 0x020022A2 RID: 8866
			public class WATERCOOLER
			{
				// Token: 0x04009A8C RID: 39564
				public static LocString NAME = UI.FormatAsLink("Water Cooler", "WATERCOOLER");

				// Token: 0x04009A8D RID: 39565
				public static LocString DESC = "Chatting with friends improves Duplicants' moods and reduces their stress.";

				// Token: 0x04009A8E RID: 39566
				public static LocString EFFECT = "Provides a gathering place for Duplicants during Downtime.\n\nImproves Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";

				// Token: 0x02002F6E RID: 12142
				public class OPTION_TOOLTIPS
				{
					// Token: 0x0400C1C6 RID: 49606
					public static LocString WATER = ELEMENTS.WATER.NAME + "\nPlain potable water";

					// Token: 0x0400C1C7 RID: 49607
					public static LocString MILK = ELEMENTS.MILK.NAME + "\nA salty, green-hued beverage";
				}

				// Token: 0x02002F6F RID: 12143
				public class FACADES
				{
					// Token: 0x020031FD RID: 12797
					public class DEFAULT_WATERCOOLER
					{
						// Token: 0x0400C85C RID: 51292
						public static LocString NAME = UI.FormatAsLink("Water Cooler", "WATERCOOLER");

						// Token: 0x0400C85D RID: 51293
						public static LocString DESC = "Where Duplicants sip and socialize.";
					}

					// Token: 0x020031FE RID: 12798
					public class ROUND_BODY
					{
						// Token: 0x0400C85E RID: 51294
						public static LocString NAME = UI.FormatAsLink("Elegant Water Cooler", "WATERCOOLER");

						// Token: 0x0400C85F RID: 51295
						public static LocString DESC = "It really classes up a breakroom.";
					}
				}
			}

			// Token: 0x020022A3 RID: 8867
			public class ARCADEMACHINE
			{
				// Token: 0x04009A8F RID: 39567
				public static LocString NAME = UI.FormatAsLink("Arcade Cabinet", "ARCADEMACHINE");

				// Token: 0x04009A90 RID: 39568
				public static LocString DESC = "Komet Kablam-O!\nFor up to two players.";

				// Token: 0x04009A91 RID: 39569
				public static LocString EFFECT = "Allows Duplicants to play video games on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x020022A4 RID: 8868
			public class SINGLEPLAYERARCADE
			{
				// Token: 0x04009A92 RID: 39570
				public static LocString NAME = UI.FormatAsLink("Single Player Arcade", "SINGLEPLAYERARCADE");

				// Token: 0x04009A93 RID: 39571
				public static LocString DESC = "Space Brawler IV! For one player.";

				// Token: 0x04009A94 RID: 39572
				public static LocString EFFECT = "Allows a Duplicant to play video games solo on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x020022A5 RID: 8869
			public class PHONOBOX
			{
				// Token: 0x04009A95 RID: 39573
				public static LocString NAME = UI.FormatAsLink("Jukebot", "PHONOBOX");

				// Token: 0x04009A96 RID: 39574
				public static LocString DESC = "Dancing helps Duplicants get their innermost feelings out.";

				// Token: 0x04009A97 RID: 39575
				public static LocString EFFECT = "Plays music for Duplicants to dance to on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x020022A6 RID: 8870
			public class JUICER
			{
				// Token: 0x04009A98 RID: 39576
				public static LocString NAME = UI.FormatAsLink("Juicer", "JUICER");

				// Token: 0x04009A99 RID: 39577
				public static LocString DESC = "Fruity juice can really brighten a Duplicant's breaktime";

				// Token: 0x04009A9A RID: 39578
				public static LocString EFFECT = "Provides refreshment for Duplicants on their breaks.\n\nDrinking juice increases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x020022A7 RID: 8871
			public class ESPRESSOMACHINE
			{
				// Token: 0x04009A9B RID: 39579
				public static LocString NAME = UI.FormatAsLink("Espresso Machine", "ESPRESSOMACHINE");

				// Token: 0x04009A9C RID: 39580
				public static LocString DESC = "A shot of espresso helps Duplicants relax after a long day.";

				// Token: 0x04009A9D RID: 39581
				public static LocString EFFECT = "Provides refreshment for Duplicants on their breaks.\n\nIncreases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";
			}

			// Token: 0x020022A8 RID: 8872
			public class TELEPHONE
			{
				// Token: 0x04009A9E RID: 39582
				public static LocString NAME = UI.FormatAsLink("Party Line Phone", "TELEPHONE");

				// Token: 0x04009A9F RID: 39583
				public static LocString DESC = "You never know who you'll meet on the other line.";

				// Token: 0x04009AA0 RID: 39584
				public static LocString EFFECT = "Can be used by one Duplicant to chat with themselves or with other Duplicants in different locations.\n\nChatting increases Duplicant " + UI.FormatAsLink("Morale", "MORALE") + ".";

				// Token: 0x04009AA1 RID: 39585
				public static LocString EFFECT_BABBLE = "{attrib}: {amount} (No One)";

				// Token: 0x04009AA2 RID: 39586
				public static LocString EFFECT_BABBLE_TOOLTIP = "Duplicants will gain {amount} {attrib} if they chat only with themselves.";

				// Token: 0x04009AA3 RID: 39587
				public static LocString EFFECT_CHAT = "{attrib}: {amount} (At least one Duplicant)";

				// Token: 0x04009AA4 RID: 39588
				public static LocString EFFECT_CHAT_TOOLTIP = "Duplicants will gain {amount} {attrib} if they chat with at least one other Duplicant.";

				// Token: 0x04009AA5 RID: 39589
				public static LocString EFFECT_LONG_DISTANCE = "{attrib}: {amount} (At least one Duplicant across space)";

				// Token: 0x04009AA6 RID: 39590
				public static LocString EFFECT_LONG_DISTANCE_TOOLTIP = "Duplicants will gain {amount} {attrib} if they chat with at least one other Duplicant across space.";
			}

			// Token: 0x020022A9 RID: 8873
			public class MODULARLIQUIDINPUT
			{
				// Token: 0x04009AA7 RID: 39591
				public static LocString NAME = UI.FormatAsLink("Liquid Input Hub", "MODULARLIQUIDINPUT");

				// Token: 0x04009AA8 RID: 39592
				public static LocString DESC = "A hub from which to input " + UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID") + ".";
			}

			// Token: 0x020022AA RID: 8874
			public class MODULARSOLIDINPUT
			{
				// Token: 0x04009AA9 RID: 39593
				public static LocString NAME = UI.FormatAsLink("Solid Input Hub", "MODULARSOLIDINPUT");

				// Token: 0x04009AAA RID: 39594
				public static LocString DESC = "A hub from which to input " + UI.FormatAsLink("Solids", "ELEMENTS_SOLID") + ".";
			}

			// Token: 0x020022AB RID: 8875
			public class MODULARGASINPUT
			{
				// Token: 0x04009AAB RID: 39595
				public static LocString NAME = UI.FormatAsLink("Gas Input Hub", "MODULARGASINPUT");

				// Token: 0x04009AAC RID: 39596
				public static LocString DESC = "A hub from which to input " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + ".";
			}

			// Token: 0x020022AC RID: 8876
			public class MECHANICALSURFBOARD
			{
				// Token: 0x04009AAD RID: 39597
				public static LocString NAME = UI.FormatAsLink("Mechanical Surfboard", "MECHANICALSURFBOARD");

				// Token: 0x04009AAE RID: 39598
				public static LocString DESC = "Mechanical waves make for radical relaxation time.";

				// Token: 0x04009AAF RID: 39599
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nSome ",
					UI.FormatAsLink("Water", "WATER"),
					" gets splashed on the floor during use."
				});

				// Token: 0x04009AB0 RID: 39600
				public static LocString WATER_REQUIREMENT = "{element}: {amount}";

				// Token: 0x04009AB1 RID: 39601
				public static LocString WATER_REQUIREMENT_TOOLTIP = "This building must be filled with {amount} {element} in order to function.";

				// Token: 0x04009AB2 RID: 39602
				public static LocString LEAK_REQUIREMENT = "Spillage: {amount}";

				// Token: 0x04009AB3 RID: 39603
				public static LocString LEAK_REQUIREMENT_TOOLTIP = "This building will spill {amount} of its contents on to the floor during use, which must be replenished.";
			}

			// Token: 0x020022AD RID: 8877
			public class SAUNA
			{
				// Token: 0x04009AB4 RID: 39604
				public static LocString NAME = UI.FormatAsLink("Sauna", "SAUNA");

				// Token: 0x04009AB5 RID: 39605
				public static LocString DESC = "A steamy sauna soothes away all the aches and pains.";

				// Token: 0x04009AB6 RID: 39606
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Steam", "STEAM"),
					" to create a relaxing atmosphere.\n\nIncreases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			// Token: 0x020022AE RID: 8878
			public class BEACHCHAIR
			{
				// Token: 0x04009AB7 RID: 39607
				public static LocString NAME = UI.FormatAsLink("Beach Chair", "BEACHCHAIR");

				// Token: 0x04009AB8 RID: 39608
				public static LocString DESC = "Soak up some relaxing sun rays.";

				// Token: 0x04009AB9 RID: 39609
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Duplicants can relax by lounging in ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					".\n\nIncreases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x04009ABA RID: 39610
				public static LocString LIGHTEFFECT_LOW = "{attrib}: {amount} (Dim Light)";

				// Token: 0x04009ABB RID: 39611
				public static LocString LIGHTEFFECT_LOW_TOOLTIP = "Duplicants will gain {amount} {attrib} if this building is in light dimmer than {lux}.";

				// Token: 0x04009ABC RID: 39612
				public static LocString LIGHTEFFECT_HIGH = "{attrib}: {amount} (Bright Light)";

				// Token: 0x04009ABD RID: 39613
				public static LocString LIGHTEFFECT_HIGH_TOOLTIP = "Duplicants will gain {amount} {attrib} if this building is in at least {lux} light.";
			}

			// Token: 0x020022AF RID: 8879
			public class SUNLAMP
			{
				// Token: 0x04009ABE RID: 39614
				public static LocString NAME = UI.FormatAsLink("Sun Lamp", "SUNLAMP");

				// Token: 0x04009ABF RID: 39615
				public static LocString DESC = "An artificial ray of sunshine.";

				// Token: 0x04009AC0 RID: 39616
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Gives off ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					" level Lux.\n\nCan be paired with ",
					UI.FormatAsLink("Beach Chairs", "BEACHCHAIR"),
					"."
				});
			}

			// Token: 0x020022B0 RID: 8880
			public class VERTICALWINDTUNNEL
			{
				// Token: 0x04009AC1 RID: 39617
				public static LocString NAME = UI.FormatAsLink("Vertical Wind Tunnel", "VERTICALWINDTUNNEL");

				// Token: 0x04009AC2 RID: 39618
				public static LocString DESC = "Duplicants love the feeling of high-powered wind through their hair.";

				// Token: 0x04009AC3 RID: 39619
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Must be connected to a ",
					UI.FormatAsLink("Power Source", "POWER"),
					". To properly function, the area under this building must be left vacant.\n\nIncreases Duplicants ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x04009AC4 RID: 39620
				public static LocString DISPLACEMENTEFFECT = "Gas Displacement: {amount}";

				// Token: 0x04009AC5 RID: 39621
				public static LocString DISPLACEMENTEFFECT_TOOLTIP = "This building will displace {amount} Gas while in use.";
			}

			// Token: 0x020022B1 RID: 8881
			public class TELEPORTALPAD
			{
				// Token: 0x04009AC6 RID: 39622
				public static LocString NAME = UI.FormatAsLink("Teleporter Pad", "TELEPORTALPAD");

				// Token: 0x04009AC7 RID: 39623
				public static LocString DESC = "Duplicants are just atoms as far as the pad's concerned.";

				// Token: 0x04009AC8 RID: 39624
				public static LocString EFFECT = "Instantly transports Duplicants and items to another portal with the same portal code.";

				// Token: 0x04009AC9 RID: 39625
				public static LocString LOGIC_PORT = "Portal Code Input";

				// Token: 0x04009ACA RID: 39626
				public static LocString LOGIC_PORT_ACTIVE = "1";

				// Token: 0x04009ACB RID: 39627
				public static LocString LOGIC_PORT_INACTIVE = "0";
			}

			// Token: 0x020022B2 RID: 8882
			public class CHECKPOINT
			{
				// Token: 0x04009ACC RID: 39628
				public static LocString NAME = UI.FormatAsLink("Duplicant Checkpoint", "CHECKPOINT");

				// Token: 0x04009ACD RID: 39629
				public static LocString DESC = "Checkpoints can be connected to automated sensors to determine when it's safe to enter.";

				// Token: 0x04009ACE RID: 39630
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to pass when receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					".\n\nPrevents Duplicants from passing when receiving a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					"."
				});

				// Token: 0x04009ACF RID: 39631
				public static LocString LOGIC_PORT = "Duplicant Stop/Go";

				// Token: 0x04009AD0 RID: 39632
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow Duplicant passage";

				// Token: 0x04009AD1 RID: 39633
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent Duplicant passage";
			}

			// Token: 0x020022B3 RID: 8883
			public class FIREPOLE
			{
				// Token: 0x04009AD2 RID: 39634
				public static LocString NAME = UI.FormatAsLink("Fire Pole", "FIREPOLE");

				// Token: 0x04009AD3 RID: 39635
				public static LocString DESC = "Build these in addition to ladders for efficient upward and downward movement.";

				// Token: 0x04009AD4 RID: 39636
				public static LocString EFFECT = "Allows rapid Duplicant descent.\n\nSignificantly slows upward climbing.";
			}

			// Token: 0x020022B4 RID: 8884
			public class FLOORSWITCH
			{
				// Token: 0x04009AD5 RID: 39637
				public static LocString NAME = UI.FormatAsLink("Weight Plate", "FLOORSWITCH");

				// Token: 0x04009AD6 RID: 39638
				public static LocString DESC = "Weight plates can be used to turn on amenities only when Duplicants pass by.";

				// Token: 0x04009AD7 RID: 39639
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when an object or Duplicant is placed atop of it.\n\nCannot be triggered by ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" or ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					"."
				});

				// Token: 0x04009AD8 RID: 39640
				public static LocString LOGIC_PORT_DESC = UI.FormatAsLink("Active", "LOGIC") + "/" + UI.FormatAsLink("Inactive", "LOGIC");
			}

			// Token: 0x020022B5 RID: 8885
			public class KILN
			{
				// Token: 0x04009AD9 RID: 39641
				public static LocString NAME = UI.FormatAsLink("Kiln", "KILN");

				// Token: 0x04009ADA RID: 39642
				public static LocString DESC = "Kilns can also be used to refine coal into pure carbon.";

				// Token: 0x04009ADB RID: 39643
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Fires ",
					UI.FormatAsLink("Clay", "CLAY"),
					" to produce ",
					UI.FormatAsLink("Ceramic", "CERAMIC"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			// Token: 0x020022B6 RID: 8886
			public class LIQUIDFUELTANK
			{
				// Token: 0x04009ADC RID: 39644
				public static LocString NAME = UI.FormatAsLink("Liquid Fuel Tank", "LIQUIDFUELTANK");

				// Token: 0x04009ADD RID: 39645
				public static LocString DESC = "Storing additional fuel increases the distance a rocket can travel before returning.";

				// Token: 0x04009ADE RID: 39646
				public static LocString EFFECT = "Stores the " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " fuel piped into it to supply rocket engines.\n\nThe stored fuel type is determined by the rocket engine it is built upon.";
			}

			// Token: 0x020022B7 RID: 8887
			public class LIQUIDFUELTANKCLUSTER
			{
				// Token: 0x04009ADF RID: 39647
				public static LocString NAME = UI.FormatAsLink("Large Liquid Fuel Tank", "LIQUIDFUELTANKCLUSTER");

				// Token: 0x04009AE0 RID: 39648
				public static LocString DESC = "Storing additional fuel increases the distance a rocket can travel before returning.";

				// Token: 0x04009AE1 RID: 39649
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" fuel piped into it to supply rocket engines.\n\nThe stored fuel type is determined by the rocket engine it is built upon. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020022B8 RID: 8888
			public class LANDING_POD
			{
				// Token: 0x04009AE2 RID: 39650
				public static LocString NAME = "Spacefarer Deploy Pod";

				// Token: 0x04009AE3 RID: 39651
				public static LocString DESC = "Geronimo!";

				// Token: 0x04009AE4 RID: 39652
				public static LocString EFFECT = "Contains a Duplicant deployed from orbit.\n\nPod will disintegrate on arrival.";
			}

			// Token: 0x020022B9 RID: 8889
			public class ROCKETPOD
			{
				// Token: 0x04009AE5 RID: 39653
				public static LocString NAME = UI.FormatAsLink("Trailblazer Deploy Pod", "ROCKETPOD");

				// Token: 0x04009AE6 RID: 39654
				public static LocString DESC = "The Duplicant inside is equal parts nervous and excited.";

				// Token: 0x04009AE7 RID: 39655
				public static LocString EFFECT = "Contains a Duplicant deployed from orbit by a " + BUILDINGS.PREFABS.PIONEERMODULE.NAME + ".\n\nPod will disintegrate on arrival.";
			}

			// Token: 0x020022BA RID: 8890
			public class SCOUTROCKETPOD
			{
				// Token: 0x04009AE8 RID: 39656
				public static LocString NAME = UI.FormatAsLink("Rover's Doghouse", "SCOUTROCKETPOD");

				// Token: 0x04009AE9 RID: 39657
				public static LocString DESC = "Good luck out there, boy!";

				// Token: 0x04009AEA RID: 39658
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Contains a ",
					UI.FormatAsLink("Rover", "SCOUT"),
					" deployed from an orbiting ",
					BUILDINGS.PREFABS.SCOUTMODULE.NAME,
					".\n\nPod will disintegrate on arrival."
				});
			}

			// Token: 0x020022BB RID: 8891
			public class ROCKETCOMMANDCONSOLE
			{
				// Token: 0x04009AEB RID: 39659
				public static LocString NAME = UI.FormatAsLink("Rocket Cockpit", "ROCKETCOMMANDCONSOLE");

				// Token: 0x04009AEC RID: 39660
				public static LocString DESC = "Looks kinda fun.";

				// Token: 0x04009AED RID: 39661
				public static LocString EFFECT = "Allows a Duplicant to pilot a rocket.\n\nCargo rockets must possess a Rocket Cockpit in order to function.";
			}

			// Token: 0x020022BC RID: 8892
			public class ROCKETENVELOPETILE
			{
				// Token: 0x04009AEE RID: 39662
				public static LocString NAME = UI.FormatAsLink("Rocket", "ROCKETENVELOPETILE");

				// Token: 0x04009AEF RID: 39663
				public static LocString DESC = "Keeps the space out.";

				// Token: 0x04009AF0 RID: 39664
				public static LocString EFFECT = "The walls of a rocket.";
			}

			// Token: 0x020022BD RID: 8893
			public class ROCKETENVELOPEWINDOWTILE
			{
				// Token: 0x04009AF1 RID: 39665
				public static LocString NAME = UI.FormatAsLink("Rocket Window", "ROCKETENVELOPEWINDOWTILE");

				// Token: 0x04009AF2 RID: 39666
				public static LocString DESC = "I can see my asteroid from here!";

				// Token: 0x04009AF3 RID: 39667
				public static LocString EFFECT = "The window of a rocket.";
			}

			// Token: 0x020022BE RID: 8894
			public class ROCKETWALLTILE
			{
				// Token: 0x04009AF4 RID: 39668
				public static LocString NAME = UI.FormatAsLink("Rocket Wall", "ROCKETENVELOPETILE");

				// Token: 0x04009AF5 RID: 39669
				public static LocString DESC = "Keeps the space out.";

				// Token: 0x04009AF6 RID: 39670
				public static LocString EFFECT = "The walls of a rocket.";
			}

			// Token: 0x020022BF RID: 8895
			public class SMALLOXIDIZERTANK
			{
				// Token: 0x04009AF7 RID: 39671
				public static LocString NAME = UI.FormatAsLink("Small Solid Oxidizer Tank", "SMALLOXIDIZERTANK");

				// Token: 0x04009AF8 RID: 39672
				public static LocString DESC = "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";

				// Token: 0x04009AF9 RID: 39673
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Fertilizer", "Fertilizer"),
					" and ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" for burning rocket fuels. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});

				// Token: 0x04009AFA RID: 39674
				public static LocString UI_FILTER_CATEGORY = "Accepted Oxidizers";
			}

			// Token: 0x020022C0 RID: 8896
			public class OXIDIZERTANK
			{
				// Token: 0x04009AFB RID: 39675
				public static LocString NAME = UI.FormatAsLink("Solid Oxidizer Tank", "OXIDIZERTANK");

				// Token: 0x04009AFC RID: 39676
				public static LocString DESC = "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";

				// Token: 0x04009AFD RID: 39677
				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Oxylite", "OXYROCK") + " and other oxidizers for burning rocket fuels.";

				// Token: 0x04009AFE RID: 39678
				public static LocString UI_FILTER_CATEGORY = "Accepted Oxidizers";
			}

			// Token: 0x020022C1 RID: 8897
			public class OXIDIZERTANKCLUSTER
			{
				// Token: 0x04009AFF RID: 39679
				public static LocString NAME = UI.FormatAsLink("Large Solid Oxidizer Tank", "OXIDIZERTANKCLUSTER");

				// Token: 0x04009B00 RID: 39680
				public static LocString DESC = "Solid oxidizers allows rocket fuel to be efficiently burned in the vacuum of space.";

				// Token: 0x04009B01 RID: 39681
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" and other oxidizers for burning rocket fuels.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});

				// Token: 0x04009B02 RID: 39682
				public static LocString UI_FILTER_CATEGORY = "Accepted Oxidizers";
			}

			// Token: 0x020022C2 RID: 8898
			public class OXIDIZERTANKLIQUID
			{
				// Token: 0x04009B03 RID: 39683
				public static LocString NAME = UI.FormatAsLink("Liquid Oxidizer Tank", "LIQUIDOXIDIZERTANK");

				// Token: 0x04009B04 RID: 39684
				public static LocString DESC = "Liquid oxygen improves the thrust-to-mass ratio of rocket fuels.";

				// Token: 0x04009B05 RID: 39685
				public static LocString EFFECT = "Stores " + UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN") + " for burning rocket fuels.";
			}

			// Token: 0x020022C3 RID: 8899
			public class OXIDIZERTANKLIQUIDCLUSTER
			{
				// Token: 0x04009B06 RID: 39686
				public static LocString NAME = UI.FormatAsLink("Liquid Oxidizer Tank", "LIQUIDOXIDIZERTANKCLUSTER");

				// Token: 0x04009B07 RID: 39687
				public static LocString DESC = "Liquid oxygen improves the thrust-to-mass ratio of rocket fuels.";

				// Token: 0x04009B08 RID: 39688
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Liquid Oxygen", "LIQUIDOXYGEN"),
					" for burning rocket fuels. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020022C4 RID: 8900
			public class LIQUIDCONDITIONER
			{
				// Token: 0x04009B09 RID: 39689
				public static LocString NAME = UI.FormatAsLink("Thermo Aquatuner", "LIQUIDCONDITIONER");

				// Token: 0x04009B0A RID: 39690
				public static LocString DESC = "A thermo aquatuner cools liquid and outputs the heat elsewhere.";

				// Token: 0x04009B0B RID: 39691
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Cools the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" piped through it, but outputs ",
					UI.FormatAsLink("Heat", "HEAT"),
					" in its immediate vicinity."
				});
			}

			// Token: 0x020022C5 RID: 8901
			public class LIQUIDCARGOBAY
			{
				// Token: 0x04009B0C RID: 39692
				public static LocString NAME = UI.FormatAsLink("Liquid Cargo Tank", "LIQUIDCARGOBAY");

				// Token: 0x04009B0D RID: 39693
				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				// Token: 0x04009B0E RID: 39694
				public static LocString EFFECT = "Allows Duplicants to store any " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			// Token: 0x020022C6 RID: 8902
			public class LIQUIDCARGOBAYCLUSTER
			{
				// Token: 0x04009B0F RID: 39695
				public static LocString NAME = UI.FormatAsLink("Large Liquid Cargo Tank", "LIQUIDCARGOBAY");

				// Token: 0x04009B10 RID: 39696
				public static LocString DESC = "Holds more than a regular cargo tank.";

				// Token: 0x04009B11 RID: 39697
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store most of the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020022C7 RID: 8903
			public class LIQUIDCARGOBAYSMALL
			{
				// Token: 0x04009B12 RID: 39698
				public static LocString NAME = UI.FormatAsLink("Liquid Cargo Tank", "LIQUIDCARGOBAYSMALL");

				// Token: 0x04009B13 RID: 39699
				public static LocString DESC = "Duplicants will fill cargo tanks with whatever resources they find during space missions.";

				// Token: 0x04009B14 RID: 39700
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store some of the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020022C8 RID: 8904
			public class LUXURYBED
			{
				// Token: 0x04009B15 RID: 39701
				public static LocString NAME = UI.FormatAsLink("Comfy Bed", "LUXURYBED");

				// Token: 0x04009B16 RID: 39702
				public static LocString DESC = "Duplicants prefer comfy beds to cots and wake up more rested after sleeping in them.";

				// Token: 0x04009B17 RID: 39703
				public static LocString EFFECT = "Provides a sleeping area for one Duplicant and restores additional stamina.\n\nDuplicants will automatically sleep in their assigned beds at night.";

				// Token: 0x02002F70 RID: 12144
				public class FACADES
				{
					// Token: 0x020031FF RID: 12799
					public class DEFAULT_LUXURYBED
					{
						// Token: 0x0400C860 RID: 51296
						public static LocString NAME = UI.FormatAsLink("Comfy Bed", "LUXURYBED");

						// Token: 0x0400C861 RID: 51297
						public static LocString DESC = "Much comfier than a cot.";
					}

					// Token: 0x02003200 RID: 12800
					public class GRANDPRIX
					{
						// Token: 0x0400C862 RID: 51298
						public static LocString NAME = UI.FormatAsLink("Grand Prix Bed", "LUXURYBED");

						// Token: 0x0400C863 RID: 51299
						public static LocString DESC = "Where every Duplicant wakes up a winner.";
					}

					// Token: 0x02003201 RID: 12801
					public class BOAT
					{
						// Token: 0x0400C864 RID: 51300
						public static LocString NAME = UI.FormatAsLink("Dreamboat Bed", "LUXURYBED");

						// Token: 0x0400C865 RID: 51301
						public static LocString DESC = "Ahoy! Set sail for zzzzz's.";
					}

					// Token: 0x02003202 RID: 12802
					public class ROCKET_BED
					{
						// Token: 0x0400C866 RID: 51302
						public static LocString NAME = UI.FormatAsLink("S.S. Napmaster Bed", "LUXURYBED");

						// Token: 0x0400C867 RID: 51303
						public static LocString DESC = "Launches sleepy Duplicants into a deep-space slumber.";
					}

					// Token: 0x02003203 RID: 12803
					public class BOUNCY_BED
					{
						// Token: 0x0400C868 RID: 51304
						public static LocString NAME = UI.FormatAsLink("Bouncy Castle Bed", "LUXURYBED");

						// Token: 0x0400C869 RID: 51305
						public static LocString DESC = "An inflatable party prop makes a surprisingly good bed.";
					}

					// Token: 0x02003204 RID: 12804
					public class PUFT_BED
					{
						// Token: 0x0400C86A RID: 51306
						public static LocString NAME = UI.FormatAsLink("Puft Bed", "LUXURYBED");

						// Token: 0x0400C86B RID: 51307
						public static LocString DESC = "A comfy, if somewhat 'fragrant', place to sleep.";
					}

					// Token: 0x02003205 RID: 12805
					public class HAND
					{
						// Token: 0x0400C86C RID: 51308
						public static LocString NAME = UI.FormatAsLink("Cradled Bed", "LUXURYBED");

						// Token: 0x0400C86D RID: 51309
						public static LocString DESC = "It's so nice to be held.";
					}

					// Token: 0x02003206 RID: 12806
					public class RUBIKS
					{
						// Token: 0x0400C86E RID: 51310
						public static LocString NAME = UI.FormatAsLink("Puzzle Cube Bed", "LUXURYBED");

						// Token: 0x0400C86F RID: 51311
						public static LocString DESC = "A little pattern recognition at bedtime soothes the mind.";
					}
				}
			}

			// Token: 0x020022C9 RID: 8905
			public class LADDERBED
			{
				// Token: 0x04009B18 RID: 39704
				public static LocString NAME = UI.FormatAsLink("Ladder Bed", "LADDERBED");

				// Token: 0x04009B19 RID: 39705
				public static LocString DESC = "Duplicant's sleep will be interrupted if another Duplicant uses the ladder.";

				// Token: 0x04009B1A RID: 39706
				public static LocString EFFECT = "Provides a sleeping area for one Duplicant and also functions as a ladder.\n\nDuplicants will automatically sleep in their assigned beds at night.";
			}

			// Token: 0x020022CA RID: 8906
			public class MEDICALCOT
			{
				// Token: 0x04009B1B RID: 39707
				public static LocString NAME = UI.FormatAsLink("Triage Cot", "MEDICALCOT");

				// Token: 0x04009B1C RID: 39708
				public static LocString DESC = "Duplicants use triage cots to recover from physical injuries and receive aid from peers.";

				// Token: 0x04009B1D RID: 39709
				public static LocString EFFECT = "Accelerates " + UI.FormatAsLink("Health", "HEALTH") + " restoration and the healing of physical injuries.\n\nRevives incapacitated Duplicants.";
			}

			// Token: 0x020022CB RID: 8907
			public class DOCTORSTATION
			{
				// Token: 0x04009B1E RID: 39710
				public static LocString NAME = UI.FormatAsLink("Sick Bay", "DOCTORSTATION");

				// Token: 0x04009B1F RID: 39711
				public static LocString DESC = "Sick bays can be placed in hospital rooms to decrease the likelihood of disease spreading.";

				// Token: 0x04009B20 RID: 39712
				public static LocString EFFECT = "Allows Duplicants to administer basic treatments to sick Duplicants.\n\nDuplicants must possess the Bedside Manner " + UI.FormatAsLink("Skill", "ROLES") + " to treat peers.";
			}

			// Token: 0x020022CC RID: 8908
			public class ADVANCEDDOCTORSTATION
			{
				// Token: 0x04009B21 RID: 39713
				public static LocString NAME = UI.FormatAsLink("Disease Clinic", "ADVANCEDDOCTORSTATION");

				// Token: 0x04009B22 RID: 39714
				public static LocString DESC = "Disease clinics require power, but treat more serious illnesses than sick bays alone.";

				// Token: 0x04009B23 RID: 39715
				public static LocString EFFECT = "Allows Duplicants to administer powerful treatments to sick Duplicants.\n\nDuplicants must possess the Advanced Medical Care " + UI.FormatAsLink("Skill", "ROLES") + " to treat peers.";
			}

			// Token: 0x020022CD RID: 8909
			public class MASSAGETABLE
			{
				// Token: 0x04009B24 RID: 39716
				public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

				// Token: 0x04009B25 RID: 39717
				public static LocString DESC = "Massage tables quickly reduce extreme stress, at the cost of power production.";

				// Token: 0x04009B26 RID: 39718
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Rapidly reduces ",
					UI.FormatAsLink("Stress", "STRESS"),
					" for the Duplicant user.\n\nDuplicants will automatically seek a massage table when ",
					UI.FormatAsLink("Stress", "STRESS"),
					" exceeds breaktime range."
				});

				// Token: 0x04009B27 RID: 39719
				public static LocString ACTIVATE_TOOLTIP = "Duplicants must take a massage break when their " + UI.FormatAsKeyWord("Stress") + " reaches {0}%";

				// Token: 0x04009B28 RID: 39720
				public static LocString DEACTIVATE_TOOLTIP = "Breaktime ends when " + UI.FormatAsKeyWord("Stress") + " is reduced to {0}%";

				// Token: 0x02002F71 RID: 12145
				public class FACADES
				{
					// Token: 0x02003207 RID: 12807
					public class DEFAULT_MASSAGETABLE
					{
						// Token: 0x0400C870 RID: 51312
						public static LocString NAME = UI.FormatAsLink("Massage Table", "MASSAGETABLE");

						// Token: 0x0400C871 RID: 51313
						public static LocString DESC = "Massage tables quickly reduce extreme stress, at the cost of power production.";
					}

					// Token: 0x02003208 RID: 12808
					public class SHIATSU
					{
						// Token: 0x0400C872 RID: 51314
						public static LocString NAME = UI.FormatAsLink("Shiatsu Table", "MASSAGETABLE");

						// Token: 0x0400C873 RID: 51315
						public static LocString DESC = "Deep pressure for deep-seated stress.";
					}
				}
			}

			// Token: 0x020022CE RID: 8910
			public class CEILINGLIGHT
			{
				// Token: 0x04009B29 RID: 39721
				public static LocString NAME = UI.FormatAsLink("Ceiling Light", "CEILINGLIGHT");

				// Token: 0x04009B2A RID: 39722
				public static LocString DESC = "Light reduces Duplicant stress and is required to grow certain plants.";

				// Token: 0x04009B2B RID: 39723
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Provides ",
					UI.FormatAsLink("Light", "LIGHT"),
					" when ",
					UI.FormatAsLink("Powered", "POWER"),
					".\n\nIncreases Duplicant workspeed within light radius."
				});

				// Token: 0x02002F72 RID: 12146
				public class FACADES
				{
					// Token: 0x02003209 RID: 12809
					public class DEFAULT_CEILINGLIGHT
					{
						// Token: 0x0400C874 RID: 51316
						public static LocString NAME = UI.FormatAsLink("Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400C875 RID: 51317
						public static LocString DESC = "It does not go on the floor.";
					}

					// Token: 0x0200320A RID: 12810
					public class LABFLASK
					{
						// Token: 0x0400C876 RID: 51318
						public static LocString NAME = UI.FormatAsLink("Lab Flask Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400C877 RID: 51319
						public static LocString DESC = "For best results, do not fill with liquids.";
					}

					// Token: 0x0200320B RID: 12811
					public class FAUXPIPE
					{
						// Token: 0x0400C878 RID: 51320
						public static LocString NAME = UI.FormatAsLink("Faux Pipe Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400C879 RID: 51321
						public static LocString DESC = "The height of plumbing-inspired interior design.";
					}

					// Token: 0x0200320C RID: 12812
					public class MINING
					{
						// Token: 0x0400C87A RID: 51322
						public static LocString NAME = UI.FormatAsLink("Mining Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400C87B RID: 51323
						public static LocString DESC = "The protective cage makes it the safest choice for underground parties.";
					}

					// Token: 0x0200320D RID: 12813
					public class BLOSSOM
					{
						// Token: 0x0400C87C RID: 51324
						public static LocString NAME = UI.FormatAsLink("Blossom Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400C87D RID: 51325
						public static LocString DESC = "For Duplicants who can't keep real plants alive.";
					}

					// Token: 0x0200320E RID: 12814
					public class POLKADOT
					{
						// Token: 0x0400C87E RID: 51326
						public static LocString NAME = UI.FormatAsLink("Polka Dot Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400C87F RID: 51327
						public static LocString DESC = "A fun lampshade for fun spaces.";
					}

					// Token: 0x0200320F RID: 12815
					public class RUBIKS
					{
						// Token: 0x0400C880 RID: 51328
						public static LocString NAME = UI.FormatAsLink("Puzzle Cube Ceiling Light", "CEILINGLIGHT");

						// Token: 0x0400C881 RID: 51329
						public static LocString DESC = "The initials E.R. are sewn into the lampshade.";
					}
				}
			}

			// Token: 0x020022CF RID: 8911
			public class AIRFILTER
			{
				// Token: 0x04009B2C RID: 39724
				public static LocString NAME = UI.FormatAsLink("Deodorizer", "AIRFILTER");

				// Token: 0x04009B2D RID: 39725
				public static LocString DESC = "Oh! Citrus scented!";

				// Token: 0x04009B2E RID: 39726
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Sand", "SAND"),
					" to filter ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					" from the air, reducing ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" spread."
				});
			}

			// Token: 0x020022D0 RID: 8912
			public class ARTIFACTANALYSISSTATION
			{
				// Token: 0x04009B2F RID: 39727
				public static LocString NAME = UI.FormatAsLink("Artifact Analysis Station", "ARTIFACTANALYSISSTATION");

				// Token: 0x04009B30 RID: 39728
				public static LocString DESC = "Discover the mysteries of the past.";

				// Token: 0x04009B31 RID: 39729
				public static LocString EFFECT = "Analyses and extracts " + UI.FormatAsLink("Neutronium", "UNOBTANIUM") + " from artifacts of interest.";

				// Token: 0x04009B32 RID: 39730
				public static LocString PAYLOAD_DROP_RATE = ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME + " drop chance: {chance}";

				// Token: 0x04009B33 RID: 39731
				public static LocString PAYLOAD_DROP_RATE_TOOLTIP = "This artifact has a {chance} to drop a " + ITEMS.INDUSTRIAL_PRODUCTS.GENE_SHUFFLER_RECHARGE.NAME + " when analyzed at the " + BUILDINGS.PREFABS.ARTIFACTANALYSISSTATION.NAME;
			}

			// Token: 0x020022D1 RID: 8913
			public class CANVAS
			{
				// Token: 0x04009B34 RID: 39732
				public static LocString NAME = UI.FormatAsLink("Blank Canvas", "CANVAS");

				// Token: 0x04009B35 RID: 39733
				public static LocString DESC = "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";

				// Token: 0x04009B36 RID: 39734
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be painted by a Duplicant."
				});

				// Token: 0x04009B37 RID: 39735
				public static LocString POORQUALITYNAME = "Crude Painting";

				// Token: 0x04009B38 RID: 39736
				public static LocString AVERAGEQUALITYNAME = "Mediocre Painting";

				// Token: 0x04009B39 RID: 39737
				public static LocString EXCELLENTQUALITYNAME = "Masterpiece";

				// Token: 0x02002F73 RID: 12147
				public class FACADES
				{
					// Token: 0x02003210 RID: 12816
					public class ART_A
					{
						// Token: 0x0400C882 RID: 51330
						public static LocString NAME = UI.FormatAsLink("Doodle Dee Duplicant", "ART_A");

						// Token: 0x0400C883 RID: 51331
						public static LocString DESC = "A sweet, amateurish interpretation of the Duplicant form.";
					}

					// Token: 0x02003211 RID: 12817
					public class ART_B
					{
						// Token: 0x0400C884 RID: 51332
						public static LocString NAME = UI.FormatAsLink("Midnight Meal", "ART_B");

						// Token: 0x0400C885 RID: 51333
						public static LocString DESC = "The fast-food equivalent of high art.";
					}

					// Token: 0x02003212 RID: 12818
					public class ART_C
					{
						// Token: 0x0400C886 RID: 51334
						public static LocString NAME = UI.FormatAsLink("Dupa Leesa", "ART_C");

						// Token: 0x0400C887 RID: 51335
						public static LocString DESC = "Some viewers swear they've seen it blink.";
					}

					// Token: 0x02003213 RID: 12819
					public class ART_D
					{
						// Token: 0x0400C888 RID: 51336
						public static LocString NAME = UI.FormatAsLink("The Screech", "ART_D");

						// Token: 0x0400C889 RID: 51337
						public static LocString DESC = "If art could speak, this piece would be far less popular.";
					}

					// Token: 0x02003214 RID: 12820
					public class ART_E
					{
						// Token: 0x0400C88A RID: 51338
						public static LocString NAME = UI.FormatAsLink("Fridup Kallo", "ART_E");

						// Token: 0x0400C88B RID: 51339
						public static LocString DESC = "Scratching and sniffing the flower yields no scent.";
					}

					// Token: 0x02003215 RID: 12821
					public class ART_F
					{
						// Token: 0x0400C88C RID: 51340
						public static LocString NAME = UI.FormatAsLink("Moopoleon Bonafarte", "ART_F");

						// Token: 0x0400C88D RID: 51341
						public static LocString DESC = "Portrait of a leader astride their mighty steed.";
					}

					// Token: 0x02003216 RID: 12822
					public class ART_G
					{
						// Token: 0x0400C88E RID: 51342
						public static LocString NAME = UI.FormatAsLink("Expressive Genius", "ART_G");

						// Token: 0x0400C88F RID: 51343
						public static LocString DESC = "The raw emotion conveyed here often renders viewers speechless.";
					}

					// Token: 0x02003217 RID: 12823
					public class ART_H
					{
						// Token: 0x0400C890 RID: 51344
						public static LocString NAME = UI.FormatAsLink("The Smooch", "ART_H");

						// Token: 0x0400C891 RID: 51345
						public static LocString DESC = "A candid moment of affection between two organisms.";
					}

					// Token: 0x02003218 RID: 12824
					public class ART_I
					{
						// Token: 0x0400C892 RID: 51346
						public static LocString NAME = UI.FormatAsLink("Self-Self-Self Portrait", "ART_I");

						// Token: 0x0400C893 RID: 51347
						public static LocString DESC = "A multi-layered exploration of the artist as a subject.";
					}

					// Token: 0x02003219 RID: 12825
					public class ART_J
					{
						// Token: 0x0400C894 RID: 51348
						public static LocString NAME = UI.FormatAsLink("Nikola Devouring His Mush Bar", "ART_J");

						// Token: 0x0400C895 RID: 51349
						public static LocString DESC = "A painting that captures the true nature of hunger.";
					}

					// Token: 0x0200321A RID: 12826
					public class ART_K
					{
						// Token: 0x0400C896 RID: 51350
						public static LocString NAME = UI.FormatAsLink("Sketchy Fungi", "ART_K");

						// Token: 0x0400C897 RID: 51351
						public static LocString DESC = "The perfect painting for dark, dank spaces.";
					}

					// Token: 0x0200321B RID: 12827
					public class ART_L
					{
						// Token: 0x0400C898 RID: 51352
						public static LocString NAME = UI.FormatAsLink("Post-Ear Era", "ART_L");

						// Token: 0x0400C899 RID: 51353
						public static LocString DESC = "The furry hat helped keep the artist's bandage on.";
					}

					// Token: 0x0200321C RID: 12828
					public class ART_M
					{
						// Token: 0x0400C89A RID: 51354
						public static LocString NAME = UI.FormatAsLink("Maternal Gaze", "ART_M");

						// Token: 0x0400C89B RID: 51355
						public static LocString DESC = "She's not angry, just disappointed.";
					}

					// Token: 0x0200321D RID: 12829
					public class ART_O
					{
						// Token: 0x0400C89C RID: 51356
						public static LocString NAME = UI.FormatAsLink("Hands-On", "ART_O");

						// Token: 0x0400C89D RID: 51357
						public static LocString DESC = "It's all about cooperation, really.";
					}

					// Token: 0x0200321E RID: 12830
					public class ART_N
					{
						// Token: 0x0400C89E RID: 51358
						public static LocString NAME = UI.FormatAsLink("Always Hope", "ART_N");

						// Token: 0x0400C89F RID: 51359
						public static LocString DESC = "Most Duplicants believe that the balloon in this image is about to be caught.";
					}

					// Token: 0x0200321F RID: 12831
					public class ART_P
					{
						// Token: 0x0400C8A0 RID: 51360
						public static LocString NAME = UI.FormatAsLink("Pour Soul", "ART_P");

						// Token: 0x0400C8A1 RID: 51361
						public static LocString DESC = "It is a cruel guest who does not RSVP.";
					}
				}
			}

			// Token: 0x020022D2 RID: 8914
			public class CANVASWIDE
			{
				// Token: 0x04009B3A RID: 39738
				public static LocString NAME = UI.FormatAsLink("Landscape Canvas", "CANVASWIDE");

				// Token: 0x04009B3B RID: 39739
				public static LocString DESC = "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";

				// Token: 0x04009B3C RID: 39740
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be painted by a Duplicant."
				});

				// Token: 0x04009B3D RID: 39741
				public static LocString POORQUALITYNAME = "Crude Painting";

				// Token: 0x04009B3E RID: 39742
				public static LocString AVERAGEQUALITYNAME = "Mediocre Painting";

				// Token: 0x04009B3F RID: 39743
				public static LocString EXCELLENTQUALITYNAME = "Masterpiece";

				// Token: 0x02002F74 RID: 12148
				public class FACADES
				{
					// Token: 0x02003220 RID: 12832
					public class ART_WIDE_A
					{
						// Token: 0x0400C8A2 RID: 51362
						public static LocString NAME = UI.FormatAsLink("The Twins", "ART_WIDE_A");

						// Token: 0x0400C8A3 RID: 51363
						public static LocString DESC = "The effort is admirable, though the execution is not.";
					}

					// Token: 0x02003221 RID: 12833
					public class ART_WIDE_B
					{
						// Token: 0x0400C8A4 RID: 51364
						public static LocString NAME = UI.FormatAsLink("Ground Zero", "ART_WIDE_B");

						// Token: 0x0400C8A5 RID: 51365
						public static LocString DESC = "Every story has its origin.";
					}

					// Token: 0x02003222 RID: 12834
					public class ART_WIDE_C
					{
						// Token: 0x0400C8A6 RID: 51366
						public static LocString NAME = UI.FormatAsLink("Still Life with Barbeque and Frost Bun", "ART_WIDE_C");

						// Token: 0x0400C8A7 RID: 51367
						public static LocString DESC = "Food this good deserves to be immortalized.";
					}

					// Token: 0x02003223 RID: 12835
					public class ART_WIDE_D
					{
						// Token: 0x0400C8A8 RID: 51368
						public static LocString NAME = UI.FormatAsLink("Composition with Three Colors", "ART_WIDE_D");

						// Token: 0x0400C8A9 RID: 51369
						public static LocString DESC = "All the other colors in the artist's palette had dried up.";
					}

					// Token: 0x02003224 RID: 12836
					public class ART_WIDE_E
					{
						// Token: 0x0400C8AA RID: 51370
						public static LocString NAME = UI.FormatAsLink("Behold, A Fork", "ART_WIDE_E");

						// Token: 0x0400C8AB RID: 51371
						public static LocString DESC = "Each tine represents a branch of science.";
					}

					// Token: 0x02003225 RID: 12837
					public class ART_WIDE_F
					{
						// Token: 0x0400C8AC RID: 51372
						public static LocString NAME = UI.FormatAsLink("The Astronomer at Home", "ART_WIDE_F");

						// Token: 0x0400C8AD RID: 51373
						public static LocString DESC = "Its companion piece, \"The Astronomer at Work\" was lost in a meteor shower.";
					}

					// Token: 0x02003226 RID: 12838
					public class ART_WIDE_G
					{
						// Token: 0x0400C8AE RID: 51374
						public static LocString NAME = UI.FormatAsLink("Iconic Iteration", "ART_WIDE_G");

						// Token: 0x0400C8AF RID: 51375
						public static LocString DESC = "For the art collector who doesn't mind a bit of repetition.";
					}

					// Token: 0x02003227 RID: 12839
					public class ART_WIDE_H
					{
						// Token: 0x0400C8B0 RID: 51376
						public static LocString NAME = UI.FormatAsLink("La Belle Meep", "ART_WIDE_H");

						// Token: 0x0400C8B1 RID: 51377
						public static LocString DESC = "A daring piece, guaranteed to cause a stir.";
					}

					// Token: 0x02003228 RID: 12840
					public class ART_WIDE_I
					{
						// Token: 0x0400C8B2 RID: 51378
						public static LocString NAME = UI.FormatAsLink("Glorious Vole", "ART_WIDE_I");

						// Token: 0x0400C8B3 RID: 51379
						public static LocString DESC = "A moody study of the renowned tunneler.";
					}

					// Token: 0x02003229 RID: 12841
					public class ART_WIDE_J
					{
						// Token: 0x0400C8B4 RID: 51380
						public static LocString NAME = UI.FormatAsLink("The Swell Swell", "ART_WIDE_J");

						// Token: 0x0400C8B5 RID: 51381
						public static LocString DESC = "As far as wave-themed art goes, it's great.";
					}

					// Token: 0x0200322A RID: 12842
					public class ART_WIDE_K
					{
						// Token: 0x0400C8B6 RID: 51382
						public static LocString NAME = UI.FormatAsLink("Flight of the Slicksters", "ART_WIDE_K");

						// Token: 0x0400C8B7 RID: 51383
						public static LocString DESC = "The delight on the subjects' faces is contagious.";
					}

					// Token: 0x0200322B RID: 12843
					public class ART_WIDE_L
					{
						// Token: 0x0400C8B8 RID: 51384
						public static LocString NAME = UI.FormatAsLink("The Shiny Night", "ART_WIDE_L");

						// Token: 0x0400C8B9 RID: 51385
						public static LocString DESC = "A dreamy abundance of swirls, whirls and whorls.";
					}

					// Token: 0x0200322C RID: 12844
					public class ART_WIDE_M
					{
						// Token: 0x0400C8BA RID: 51386
						public static LocString NAME = UI.FormatAsLink("Hot Afternoon", "ART_WIDE_M");

						// Token: 0x0400C8BB RID: 51387
						public static LocString DESC = "Things get a bit melty if they're forgotten in the sun.";
					}
				}
			}

			// Token: 0x020022D3 RID: 8915
			public class CANVASTALL
			{
				// Token: 0x04009B40 RID: 39744
				public static LocString NAME = UI.FormatAsLink("Portrait Canvas", "CANVASTALL");

				// Token: 0x04009B41 RID: 39745
				public static LocString DESC = "Once built, a Duplicant can paint a blank canvas to produce a decorative painting.";

				// Token: 0x04009B42 RID: 39746
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be painted by a Duplicant."
				});

				// Token: 0x04009B43 RID: 39747
				public static LocString POORQUALITYNAME = "Crude Painting";

				// Token: 0x04009B44 RID: 39748
				public static LocString AVERAGEQUALITYNAME = "Mediocre Painting";

				// Token: 0x04009B45 RID: 39749
				public static LocString EXCELLENTQUALITYNAME = "Masterpiece";

				// Token: 0x02002F75 RID: 12149
				public class FACADES
				{
					// Token: 0x0200322D RID: 12845
					public class ART_TALL_A
					{
						// Token: 0x0400C8BC RID: 51388
						public static LocString NAME = UI.FormatAsLink("Ode to O2", "ART_TALL_A");

						// Token: 0x0400C8BD RID: 51389
						public static LocString DESC = "Even amateur art is essential to life.";
					}

					// Token: 0x0200322E RID: 12846
					public class ART_TALL_B
					{
						// Token: 0x0400C8BE RID: 51390
						public static LocString NAME = UI.FormatAsLink("A Cool Wheeze", "ART_TALL_B");

						// Token: 0x0400C8BF RID: 51391
						public static LocString DESC = "It certainly is colorful.";
					}

					// Token: 0x0200322F RID: 12847
					public class ART_TALL_C
					{
						// Token: 0x0400C8C0 RID: 51392
						public static LocString NAME = UI.FormatAsLink("Luxe Splatter", "ART_TALL_C");

						// Token: 0x0400C8C1 RID: 51393
						public static LocString DESC = "Chaotic, yet compelling.";
					}

					// Token: 0x02003230 RID: 12848
					public class ART_TALL_D
					{
						// Token: 0x0400C8C2 RID: 51394
						public static LocString NAME = UI.FormatAsLink("Pickled Meal Lice II", "ART_TALL_D");

						// Token: 0x0400C8C3 RID: 51395
						public static LocString DESC = "It doesn't have to taste good, it's art.";
					}

					// Token: 0x02003231 RID: 12849
					public class ART_TALL_E
					{
						// Token: 0x0400C8C4 RID: 51396
						public static LocString NAME = UI.FormatAsLink("Fruit Face", "ART_TALL_E");

						// Token: 0x0400C8C5 RID: 51397
						public static LocString DESC = "Rumor has it that the model was self-conscious about their uneven eyebrows.";
					}

					// Token: 0x02003232 RID: 12850
					public class ART_TALL_F
					{
						// Token: 0x0400C8C6 RID: 51398
						public static LocString NAME = UI.FormatAsLink("Girl with the Blue Scarf", "ART_TALL_F");

						// Token: 0x0400C8C7 RID: 51399
						public static LocString DESC = "The earring is nice too.";
					}

					// Token: 0x02003233 RID: 12851
					public class ART_TALL_G
					{
						// Token: 0x0400C8C8 RID: 51400
						public static LocString NAME = UI.FormatAsLink("A Farewell at Sunrise", "ART_TALL_G");

						// Token: 0x0400C8C9 RID: 51401
						public static LocString DESC = "A poetic ink painting depicting the beginning of an end.";
					}

					// Token: 0x02003234 RID: 12852
					public class ART_TALL_H
					{
						// Token: 0x0400C8CA RID: 51402
						public static LocString NAME = UI.FormatAsLink("Conqueror of Clusters", "ART_TALL_H");

						// Token: 0x0400C8CB RID: 51403
						public static LocString DESC = "The type of painting that ambitious Duplicants gravitate to.";
					}

					// Token: 0x02003235 RID: 12853
					public class ART_TALL_I
					{
						// Token: 0x0400C8CC RID: 51404
						public static LocString NAME = UI.FormatAsLink("Pei Phone", "ART_TALL_I");

						// Token: 0x0400C8CD RID: 51405
						public static LocString DESC = "When the future calls, Duplicants answer.";
					}

					// Token: 0x02003236 RID: 12854
					public class ART_TALL_J
					{
						// Token: 0x0400C8CE RID: 51406
						public static LocString NAME = UI.FormatAsLink("Duplicants of the Galaxy", "ART_TALL_J");

						// Token: 0x0400C8CF RID: 51407
						public static LocString DESC = "A poster for a blockbuster film that was never made.";
					}

					// Token: 0x02003237 RID: 12855
					public class ART_TALL_K
					{
						// Token: 0x0400C8D0 RID: 51408
						public static LocString NAME = UI.FormatAsLink("Cubist Loo", "ART_TALL_K");

						// Token: 0x0400C8D1 RID: 51409
						public static LocString DESC = "The glass and frame are hydrophobic, for easy cleaning.";
					}

					// Token: 0x02003238 RID: 12856
					public class ART_TALL_M
					{
						// Token: 0x0400C8D2 RID: 51410
						public static LocString NAME = UI.FormatAsLink("Do Not Disturb", "ART_TALL_M");

						// Token: 0x0400C8D3 RID: 51411
						public static LocString DESC = "No one likes being interrupted when they're waiting for inspiration to strike.";
					}

					// Token: 0x02003239 RID: 12857
					public class ART_TALL_L
					{
						// Token: 0x0400C8D4 RID: 51412
						public static LocString NAME = UI.FormatAsLink("Mirror Ball", "ART_TALL_L");

						// Token: 0x0400C8D5 RID: 51413
						public static LocString DESC = "Nearby, a companion animal waited for the object to be thrown.";
					}
				}
			}

			// Token: 0x020022D4 RID: 8916
			public class CO2SCRUBBER
			{
				// Token: 0x04009B46 RID: 39750
				public static LocString NAME = UI.FormatAsLink("Carbon Skimmer", "CO2SCRUBBER");

				// Token: 0x04009B47 RID: 39751
				public static LocString DESC = "Skimmers remove large amounts of carbon dioxide, but produce no breathable air.";

				// Token: 0x04009B48 RID: 39752
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Water", "WATER"),
					" to filter ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" from the air."
				});
			}

			// Token: 0x020022D5 RID: 8917
			public class COMPOST
			{
				// Token: 0x04009B49 RID: 39753
				public static LocString NAME = UI.FormatAsLink("Compost", "COMPOST");

				// Token: 0x04009B4A RID: 39754
				public static LocString DESC = "Composts safely deal with biological waste, producing fresh dirt.";

				// Token: 0x04009B4B RID: 39755
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reduces ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" and other compostables down into ",
					UI.FormatAsLink("Dirt", "DIRT"),
					"."
				});
			}

			// Token: 0x020022D6 RID: 8918
			public class COOKINGSTATION
			{
				// Token: 0x04009B4C RID: 39756
				public static LocString NAME = UI.FormatAsLink("Electric Grill", "COOKINGSTATION");

				// Token: 0x04009B4D RID: 39757
				public static LocString DESC = "Proper cooking eliminates foodborne disease and produces tasty, stress-relieving meals.";

				// Token: 0x04009B4E RID: 39758
				public static LocString EFFECT = "Cooks a wide variety of improved " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x020022D7 RID: 8919
			public class CRYOTANK
			{
				// Token: 0x04009B4F RID: 39759
				public static LocString NAME = UI.FormatAsLink("Cryotank 3000", "CRYOTANK");

				// Token: 0x04009B50 RID: 39760
				public static LocString DESC = "The tank appears impossibly old, but smells crisp and brand new.\n\nA silhouette just barely visible through the frost of the glass.";

				// Token: 0x04009B51 RID: 39761
				public static LocString DEFROSTBUTTON = "Defrost Friend";

				// Token: 0x04009B52 RID: 39762
				public static LocString DEFROSTBUTTONTOOLTIP = "A new pal is just an icebreaker away";
			}

			// Token: 0x020022D8 RID: 8920
			public class GOURMETCOOKINGSTATION
			{
				// Token: 0x04009B53 RID: 39763
				public static LocString NAME = UI.FormatAsLink("Gas Range", "GOURMETCOOKINGSTATION");

				// Token: 0x04009B54 RID: 39764
				public static LocString DESC = "Luxury meals increase Duplicants' morale and prevents them from becoming stressed.";

				// Token: 0x04009B55 RID: 39765
				public static LocString EFFECT = "Cooks a wide variety of quality " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x020022D9 RID: 8921
			public class DININGTABLE
			{
				// Token: 0x04009B56 RID: 39766
				public static LocString NAME = UI.FormatAsLink("Mess Table", "DININGTABLE");

				// Token: 0x04009B57 RID: 39767
				public static LocString DESC = "Duplicants prefer to dine at a table, rather than eat off the floor.";

				// Token: 0x04009B58 RID: 39768
				public static LocString EFFECT = "Gives one Duplicant a place to eat.\n\nDuplicants will automatically eat at their assigned table when hungry.";
			}

			// Token: 0x020022DA RID: 8922
			public class DOOR
			{
				// Token: 0x04009B59 RID: 39769
				public static LocString NAME = UI.FormatAsLink("Pneumatic Door", "DOOR");

				// Token: 0x04009B5A RID: 39770
				public static LocString DESC = "Door controls can be used to prevent Duplicants from entering restricted areas.";

				// Token: 0x04009B5B RID: 39771
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Encloses areas without blocking ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});

				// Token: 0x04009B5C RID: 39772
				public static LocString PRESSURE_SUIT_REQUIRED = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT") + " required {0}";

				// Token: 0x04009B5D RID: 39773
				public static LocString PRESSURE_SUIT_NOT_REQUIRED = UI.FormatAsLink("Atmo Suit", "ATMO_SUIT") + " not required {0}";

				// Token: 0x04009B5E RID: 39774
				public static LocString ABOVE = "above";

				// Token: 0x04009B5F RID: 39775
				public static LocString BELOW = "below";

				// Token: 0x04009B60 RID: 39776
				public static LocString LEFT = "on left";

				// Token: 0x04009B61 RID: 39777
				public static LocString RIGHT = "on right";

				// Token: 0x04009B62 RID: 39778
				public static LocString LOGIC_OPEN = "Open/Close";

				// Token: 0x04009B63 RID: 39779
				public static LocString LOGIC_OPEN_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Open";

				// Token: 0x04009B64 RID: 39780
				public static LocString LOGIC_OPEN_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Close and lock";

				// Token: 0x02002F76 RID: 12150
				public static class CONTROL_STATE
				{
					// Token: 0x0200323A RID: 12858
					public class OPEN
					{
						// Token: 0x0400C8D6 RID: 51414
						public static LocString NAME = "Open";

						// Token: 0x0400C8D7 RID: 51415
						public static LocString TOOLTIP = "This door will remain open";
					}

					// Token: 0x0200323B RID: 12859
					public class CLOSE
					{
						// Token: 0x0400C8D8 RID: 51416
						public static LocString NAME = "Lock";

						// Token: 0x0400C8D9 RID: 51417
						public static LocString TOOLTIP = "Nothing may pass through";
					}

					// Token: 0x0200323C RID: 12860
					public class AUTO
					{
						// Token: 0x0400C8DA RID: 51418
						public static LocString NAME = "Auto";

						// Token: 0x0400C8DB RID: 51419
						public static LocString TOOLTIP = "Duplicants open and close this door as needed";
					}
				}
			}

			// Token: 0x020022DB RID: 8923
			public class ELECTROLYZER
			{
				// Token: 0x04009B65 RID: 39781
				public static LocString NAME = UI.FormatAsLink("Electrolyzer", "ELECTROLYZER");

				// Token: 0x04009B66 RID: 39782
				public static LocString DESC = "Water goes in one end, life sustaining oxygen comes out the other.";

				// Token: 0x04009B67 RID: 39783
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Water", "WATER"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			// Token: 0x020022DC RID: 8924
			public class RUSTDEOXIDIZER
			{
				// Token: 0x04009B68 RID: 39784
				public static LocString NAME = UI.FormatAsLink("Rust Deoxidizer", "RUSTDEOXIDIZER");

				// Token: 0x04009B69 RID: 39785
				public static LocString DESC = "Rust and salt goes in, oxygen comes out.";

				// Token: 0x04009B6A RID: 39786
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Rust", "RUST"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Liquid Chlorine", "CHLORINE"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			// Token: 0x020022DD RID: 8925
			public class DESALINATOR
			{
				// Token: 0x04009B6B RID: 39787
				public static LocString NAME = UI.FormatAsLink("Desalinator", "DESALINATOR");

				// Token: 0x04009B6C RID: 39788
				public static LocString DESC = "Salt can be refined into table salt for a mealtime morale boost.";

				// Token: 0x04009B6D RID: 39789
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Removes ",
					UI.FormatAsLink("Salt", "SALT"),
					" from ",
					UI.FormatAsLink("Brine", "BRINE"),
					" or ",
					UI.FormatAsLink("Salt Water", "SALTWATER"),
					", producing ",
					UI.FormatAsLink("Water", "WATER"),
					"."
				});
			}

			// Token: 0x020022DE RID: 8926
			public class POWERTRANSFORMERSMALL
			{
				// Token: 0x04009B6E RID: 39790
				public static LocString NAME = UI.FormatAsLink("Power Transformer", "POWERTRANSFORMERSMALL");

				// Token: 0x04009B6F RID: 39791
				public static LocString DESC = "Limiting the power drawn by wires prevents them from incurring overload damage.";

				// Token: 0x04009B70 RID: 39792
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Limits ",
					UI.FormatAsLink("Power", "POWER"),
					" flowing through the Transformer to 1000 W.\n\nConnect ",
					UI.FormatAsLink("Batteries", "BATTERY"),
					" on the large side to act as a valve and prevent ",
					UI.FormatAsLink("Wires", "WIRE"),
					" from drawing more than 1000 W.\n\nCan be rotated before construction."
				});
			}

			// Token: 0x020022DF RID: 8927
			public class POWERTRANSFORMER
			{
				// Token: 0x04009B71 RID: 39793
				public static LocString NAME = UI.FormatAsLink("Large Power Transformer", "POWERTRANSFORMER");

				// Token: 0x04009B72 RID: 39794
				public static LocString DESC = "It's a power transformer, but larger.";

				// Token: 0x04009B73 RID: 39795
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Limits ",
					UI.FormatAsLink("Power", "POWER"),
					" flowing through the Transformer to 4 kW.\n\nConnect ",
					UI.FormatAsLink("Batteries", "BATTERY"),
					" on the large side to act as a valve and prevent ",
					UI.FormatAsLink("Wires", "WIRE"),
					" from drawing more than 4 kW.\n\nCan be rotated before construction."
				});
			}

			// Token: 0x020022E0 RID: 8928
			public class FLOORLAMP
			{
				// Token: 0x04009B74 RID: 39796
				public static LocString NAME = UI.FormatAsLink("Lamp", "FLOORLAMP");

				// Token: 0x04009B75 RID: 39797
				public static LocString DESC = "Any building's light emitting radius can be viewed in the light overlay.";

				// Token: 0x04009B76 RID: 39798
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Provides ",
					UI.FormatAsLink("Light", "LIGHT"),
					" when ",
					UI.FormatAsLink("Powered", "POWER"),
					".\n\nIncreases Duplicant workspeed within light radius."
				});

				// Token: 0x02002F77 RID: 12151
				public class FACADES
				{
				}
			}

			// Token: 0x020022E1 RID: 8929
			public class FLOWERVASE
			{
				// Token: 0x04009B77 RID: 39799
				public static LocString NAME = UI.FormatAsLink("Flower Pot", "FLOWERVASE");

				// Token: 0x04009B78 RID: 39800
				public static LocString DESC = "Flower pots allow decorative plants to be moved to new locations.";

				// Token: 0x04009B79 RID: 39801
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x02002F78 RID: 12152
				public class FACADES
				{
					// Token: 0x0200323D RID: 12861
					public class DEFAULT_FLOWERVASE
					{
						// Token: 0x0400C8DC RID: 51420
						public static LocString NAME = UI.FormatAsLink("Flower Pot", "FLOWERVASE");

						// Token: 0x0400C8DD RID: 51421
						public static LocString DESC = "The original container for plants on the move.";
					}

					// Token: 0x0200323E RID: 12862
					public class RETRO_SUNNY
					{
						// Token: 0x0400C8DE RID: 51422
						public static LocString NAME = UI.FormatAsLink("Sunny Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400C8DF RID: 51423
						public static LocString DESC = "A funky yellow flower pot for plants on the move.";
					}

					// Token: 0x0200323F RID: 12863
					public class RETRO_BOLD
					{
						// Token: 0x0400C8E0 RID: 51424
						public static LocString NAME = UI.FormatAsLink("Bold Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400C8E1 RID: 51425
						public static LocString DESC = "A funky red flower pot for plants on the move.";
					}

					// Token: 0x02003240 RID: 12864
					public class RETRO_BRIGHT
					{
						// Token: 0x0400C8E2 RID: 51426
						public static LocString NAME = UI.FormatAsLink("Bright Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400C8E3 RID: 51427
						public static LocString DESC = "A funky green flower pot for plants on the move.";
					}

					// Token: 0x02003241 RID: 12865
					public class RETRO_DREAMY
					{
						// Token: 0x0400C8E4 RID: 51428
						public static LocString NAME = UI.FormatAsLink("Dreamy Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400C8E5 RID: 51429
						public static LocString DESC = "A funky blue flower pot for plants on the move.";
					}

					// Token: 0x02003242 RID: 12866
					public class RETRO_ELEGANT
					{
						// Token: 0x0400C8E6 RID: 51430
						public static LocString NAME = UI.FormatAsLink("Elegant Retro Flower Pot", "FLOWERVASE");

						// Token: 0x0400C8E7 RID: 51431
						public static LocString DESC = "A funky white flower pot for plants on the move.";
					}
				}
			}

			// Token: 0x020022E2 RID: 8930
			public class FLOWERVASEWALL
			{
				// Token: 0x04009B7A RID: 39802
				public static LocString NAME = UI.FormatAsLink("Wall Pot", "FLOWERVASEWALL");

				// Token: 0x04009B7B RID: 39803
				public static LocString DESC = "Placing a plant in a wall pot can add a spot of Decor to otherwise bare walls.";

				// Token: 0x04009B7C RID: 39804
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be hung from a wall."
				});

				// Token: 0x02002F79 RID: 12153
				public class FACADES
				{
					// Token: 0x02003243 RID: 12867
					public class DEFAULT_FLOWERVASEWALL
					{
						// Token: 0x0400C8E8 RID: 51432
						public static LocString NAME = UI.FormatAsLink("Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400C8E9 RID: 51433
						public static LocString DESC = "Facilitates vertical plant displays.";
					}

					// Token: 0x02003244 RID: 12868
					public class RETRO_GREEN
					{
						// Token: 0x0400C8EA RID: 51434
						public static LocString NAME = UI.FormatAsLink("Bright Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400C8EB RID: 51435
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					// Token: 0x02003245 RID: 12869
					public class RETRO_YELLOW
					{
						// Token: 0x0400C8EC RID: 51436
						public static LocString NAME = UI.FormatAsLink("Sunny Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400C8ED RID: 51437
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					// Token: 0x02003246 RID: 12870
					public class RETRO_RED
					{
						// Token: 0x0400C8EE RID: 51438
						public static LocString NAME = UI.FormatAsLink("Bold Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400C8EF RID: 51439
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					// Token: 0x02003247 RID: 12871
					public class RETRO_BLUE
					{
						// Token: 0x0400C8F0 RID: 51440
						public static LocString NAME = UI.FormatAsLink("Dreamy Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400C8F1 RID: 51441
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}

					// Token: 0x02003248 RID: 12872
					public class RETRO_WHITE
					{
						// Token: 0x0400C8F2 RID: 51442
						public static LocString NAME = UI.FormatAsLink("Elegant Retro Wall Pot", "FLOWERVASEWALL");

						// Token: 0x0400C8F3 RID: 51443
						public static LocString DESC = "Vertical gardens are pretty nifty.";
					}
				}
			}

			// Token: 0x020022E3 RID: 8931
			public class FLOWERVASEHANGING
			{
				// Token: 0x04009B7D RID: 39805
				public static LocString NAME = UI.FormatAsLink("Hanging Pot", "FLOWERVASEHANGING");

				// Token: 0x04009B7E RID: 39806
				public static LocString DESC = "Hanging pots can add some Decor to a room, without blocking buildings on the floor.";

				// Token: 0x04009B7F RID: 39807
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be hung from a ceiling."
				});

				// Token: 0x02002F7A RID: 12154
				public class FACADES
				{
					// Token: 0x02003249 RID: 12873
					public class RETRO_RED
					{
						// Token: 0x0400C8F4 RID: 51444
						public static LocString NAME = UI.FormatAsLink("Bold Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400C8F5 RID: 51445
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x0200324A RID: 12874
					public class RETRO_GREEN
					{
						// Token: 0x0400C8F6 RID: 51446
						public static LocString NAME = UI.FormatAsLink("Bright Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400C8F7 RID: 51447
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x0200324B RID: 12875
					public class RETRO_BLUE
					{
						// Token: 0x0400C8F8 RID: 51448
						public static LocString NAME = UI.FormatAsLink("Dreamy Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400C8F9 RID: 51449
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x0200324C RID: 12876
					public class RETRO_YELLOW
					{
						// Token: 0x0400C8FA RID: 51450
						public static LocString NAME = UI.FormatAsLink("Sunny Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400C8FB RID: 51451
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x0200324D RID: 12877
					public class RETRO_WHITE
					{
						// Token: 0x0400C8FC RID: 51452
						public static LocString NAME = UI.FormatAsLink("Elegant Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400C8FD RID: 51453
						public static LocString DESC = "Suspended vessels really elevate a plant display.";
					}

					// Token: 0x0200324E RID: 12878
					public class BEAKER
					{
						// Token: 0x0400C8FE RID: 51454
						public static LocString NAME = UI.FormatAsLink("Beaker Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400C8FF RID: 51455
						public static LocString DESC = "A measured approach to indoor plant decor.";
					}

					// Token: 0x0200324F RID: 12879
					public class RUBIKS
					{
						// Token: 0x0400C900 RID: 51456
						public static LocString NAME = UI.FormatAsLink("Puzzle Cube Hanging Pot", "FLOWERVASEHANGING");

						// Token: 0x0400C901 RID: 51457
						public static LocString DESC = "The real puzzle is how to keep indoor plants alive.";
					}
				}
			}

			// Token: 0x020022E4 RID: 8932
			public class FLOWERVASEHANGINGFANCY
			{
				// Token: 0x04009B80 RID: 39808
				public static LocString NAME = UI.FormatAsLink("Aero Pot", "FLOWERVASEHANGINGFANCY");

				// Token: 0x04009B81 RID: 39809
				public static LocString DESC = "Aero pots can be hung from the ceiling and have extremely high Decor.";

				// Token: 0x04009B82 RID: 39810
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Houses a single ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" when sown with a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be hung from a ceiling."
				});

				// Token: 0x02002F7B RID: 12155
				public class FACADES
				{
				}
			}

			// Token: 0x020022E5 RID: 8933
			public class FLUSHTOILET
			{
				// Token: 0x04009B83 RID: 39811
				public static LocString NAME = UI.FormatAsLink("Lavatory", "FLUSHTOILET");

				// Token: 0x04009B84 RID: 39812
				public static LocString DESC = "Lavatories transmit fewer germs to Duplicants' skin and require no emptying.";

				// Token: 0x04009B85 RID: 39813
				public static LocString EFFECT = "Gives Duplicants a place to relieve themselves.\n\nSpreads very few " + UI.FormatAsLink("Germs", "DISEASE") + ".";
			}

			// Token: 0x020022E6 RID: 8934
			public class SHOWER
			{
				// Token: 0x04009B86 RID: 39814
				public static LocString NAME = UI.FormatAsLink("Shower", "SHOWER");

				// Token: 0x04009B87 RID: 39815
				public static LocString DESC = "Regularly showering will prevent Duplicants spreading germs to the things they touch.";

				// Token: 0x04009B88 RID: 39816
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Improves Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					" and removes surface ",
					UI.FormatAsLink("Germs", "DISEASE"),
					"."
				});
			}

			// Token: 0x020022E7 RID: 8935
			public class CONDUIT
			{
				// Token: 0x02002F7C RID: 12156
				public class STATUS_ITEM
				{
					// Token: 0x0400C1C8 RID: 49608
					public static LocString NAME = "Marked for Emptying";

					// Token: 0x0400C1C9 RID: 49609
					public static LocString TOOLTIP = "Awaiting a " + UI.FormatAsLink("Plumber", "PLUMBER") + " to clear this pipe";
				}
			}

			// Token: 0x020022E8 RID: 8936
			public class FOSSILDIG
			{
				// Token: 0x04009B89 RID: 39817
				public static LocString NAME = "Ancient Specimen";

				// Token: 0x04009B8A RID: 39818
				public static LocString DESC = "It's not from around here.";

				// Token: 0x04009B8B RID: 39819
				public static LocString EFFECT = "Contains a partial " + UI.FormatAsLink("Fossil", "FOSSIL") + " left behind by a giant critter.\n\nStudying the full skeleton could yield the information required to access a valuable new resource.";
			}

			// Token: 0x020022E9 RID: 8937
			public class FOSSILDIG_COMPLETED
			{
				// Token: 0x04009B8C RID: 39820
				public static LocString NAME = "Fossil Quarry";

				// Token: 0x04009B8D RID: 39821
				public static LocString DESC = "There sure are a lot of old bones in this area.";

				// Token: 0x04009B8E RID: 39822
				public static LocString EFFECT = "Contains a deep cache of harvestable " + UI.FormatAsLink("Fossils", "FOSSIL") + ".";
			}

			// Token: 0x020022EA RID: 8938
			public class GAMMARAYOVEN
			{
				// Token: 0x04009B8F RID: 39823
				public static LocString NAME = UI.FormatAsLink("Gamma Ray Oven", "GAMMARAYOVEN");

				// Token: 0x04009B90 RID: 39824
				public static LocString DESC = "Nuke your food.";

				// Token: 0x04009B91 RID: 39825
				public static LocString EFFECT = "Cooks a variety of " + UI.FormatAsLink("Foods", "FOOD") + ".\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x020022EB RID: 8939
			public class GASCARGOBAY
			{
				// Token: 0x04009B92 RID: 39826
				public static LocString NAME = UI.FormatAsLink("Gas Cargo Canister", "GASCARGOBAY");

				// Token: 0x04009B93 RID: 39827
				public static LocString DESC = "Duplicants will fill cargo bays with any resources they find during space missions.";

				// Token: 0x04009B94 RID: 39828
				public static LocString EFFECT = "Allows Duplicants to store any " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.";
			}

			// Token: 0x020022EC RID: 8940
			public class GASCARGOBAYCLUSTER
			{
				// Token: 0x04009B95 RID: 39829
				public static LocString NAME = UI.FormatAsLink("Large Gas Cargo Canister", "GASCARGOBAY");

				// Token: 0x04009B96 RID: 39830
				public static LocString DESC = "Holds more than a typical gas cargo canister.";

				// Token: 0x04009B97 RID: 39831
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store most of the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return.\n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020022ED RID: 8941
			public class GASCARGOBAYSMALL
			{
				// Token: 0x04009B98 RID: 39832
				public static LocString NAME = UI.FormatAsLink("Gas Cargo Canister", "GASCARGOBAYSMALL");

				// Token: 0x04009B99 RID: 39833
				public static LocString DESC = "Duplicants fill cargo canisters with any resources they find during space missions.";

				// Token: 0x04009B9A RID: 39834
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to store some of the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources found during space missions.\n\nStored resources become available to the colony upon the rocket's return. \n\nMust be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					"."
				});
			}

			// Token: 0x020022EE RID: 8942
			public class GASCONDUIT
			{
				// Token: 0x04009B9B RID: 39835
				public static LocString NAME = UI.FormatAsLink("Gas Pipe", "GASCONDUIT");

				// Token: 0x04009B9C RID: 39836
				public static LocString DESC = "Gas pipes are used to connect the inputs and outputs of ventilated buildings.";

				// Token: 0x04009B9D RID: 39837
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" between ",
					UI.FormatAsLink("Outputs", "GASPIPING"),
					" and ",
					UI.FormatAsLink("Intakes", "GASPIPING"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x020022EF RID: 8943
			public class GASCONDUITBRIDGE
			{
				// Token: 0x04009B9E RID: 39838
				public static LocString NAME = UI.FormatAsLink("Gas Bridge", "GASCONDUITBRIDGE");

				// Token: 0x04009B9F RID: 39839
				public static LocString DESC = "Separate pipe systems prevent mingled contents from causing building damage.";

				// Token: 0x04009BA0 RID: 39840
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Gas Pipe", "GASPIPING") + " section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x020022F0 RID: 8944
			public class GASCONDUITPREFERENTIALFLOW
			{
				// Token: 0x04009BA1 RID: 39841
				public static LocString NAME = UI.FormatAsLink("Priority Gas Flow", "GASCONDUITPREFERENTIALFLOW");

				// Token: 0x04009BA2 RID: 39842
				public static LocString DESC = "Priority flows ensure important buildings are filled first when on a system with other buildings.";

				// Token: 0x04009BA3 RID: 39843
				public static LocString EFFECT = "Diverts " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " to a secondary input when its primary input overflows.";
			}

			// Token: 0x020022F1 RID: 8945
			public class LIQUIDCONDUITPREFERENTIALFLOW
			{
				// Token: 0x04009BA4 RID: 39844
				public static LocString NAME = UI.FormatAsLink("Priority Liquid Flow", "LIQUIDCONDUITPREFERENTIALFLOW");

				// Token: 0x04009BA5 RID: 39845
				public static LocString DESC = "Priority flows ensure important buildings are filled first when on a system with other buildings.";

				// Token: 0x04009BA6 RID: 39846
				public static LocString EFFECT = "Diverts " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " to a secondary input when its primary input overflows.";
			}

			// Token: 0x020022F2 RID: 8946
			public class GASCONDUITOVERFLOW
			{
				// Token: 0x04009BA7 RID: 39847
				public static LocString NAME = UI.FormatAsLink("Gas Overflow Valve", "GASCONDUITOVERFLOW");

				// Token: 0x04009BA8 RID: 39848
				public static LocString DESC = "Overflow valves can be used to prioritize which buildings should receive precious resources first.";

				// Token: 0x04009BA9 RID: 39849
				public static LocString EFFECT = "Fills a secondary" + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " output only when its primary output is blocked.";
			}

			// Token: 0x020022F3 RID: 8947
			public class LIQUIDCONDUITOVERFLOW
			{
				// Token: 0x04009BAA RID: 39850
				public static LocString NAME = UI.FormatAsLink("Liquid Overflow Valve", "LIQUIDCONDUITOVERFLOW");

				// Token: 0x04009BAB RID: 39851
				public static LocString DESC = "Overflow valves can be used to prioritize which buildings should receive precious resources first.";

				// Token: 0x04009BAC RID: 39852
				public static LocString EFFECT = "Fills a secondary" + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " output only when its primary output is blocked.";
			}

			// Token: 0x020022F4 RID: 8948
			public class LAUNCHPAD
			{
				// Token: 0x04009BAD RID: 39853
				public static LocString NAME = UI.FormatAsLink("Rocket Platform", "LAUNCHPAD");

				// Token: 0x04009BAE RID: 39854
				public static LocString DESC = "A platform from which rockets can be launched and on which they can land.";

				// Token: 0x04009BAF RID: 39855
				public static LocString EFFECT = "Precursor to construction of all other Rocket modules.\n\nAllows Rockets to launch from or land on the host Planetoid.\n\nAutomatically links up to " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + UI.FormatAsLink("s", "MODULARLAUNCHPADPORTSOLID") + " built to either side of the platform.";

				// Token: 0x04009BB0 RID: 39856
				public static LocString LOGIC_PORT_READY = "Rocket Checklist";

				// Token: 0x04009BB1 RID: 39857
				public static LocString LOGIC_PORT_READY_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket is ready for flight";

				// Token: 0x04009BB2 RID: 39858
				public static LocString LOGIC_PORT_READY_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x04009BB3 RID: 39859
				public static LocString LOGIC_PORT_LANDED_ROCKET = "Landed Rocket";

				// Token: 0x04009BB4 RID: 39860
				public static LocString LOGIC_PORT_LANDED_ROCKET_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its rocket is on the " + BUILDINGS.PREFABS.LAUNCHPAD.NAME;

				// Token: 0x04009BB5 RID: 39861
				public static LocString LOGIC_PORT_LANDED_ROCKET_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x04009BB6 RID: 39862
				public static LocString LOGIC_PORT_LAUNCH = "Launch Rocket";

				// Token: 0x04009BB7 RID: 39863
				public static LocString LOGIC_PORT_LAUNCH_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Launch rocket";

				// Token: 0x04009BB8 RID: 39864
				public static LocString LOGIC_PORT_LAUNCH_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Cancel launch";
			}

			// Token: 0x020022F5 RID: 8949
			public class GASFILTER
			{
				// Token: 0x04009BB9 RID: 39865
				public static LocString NAME = UI.FormatAsLink("Gas Filter", "GASFILTER");

				// Token: 0x04009BBA RID: 39866
				public static LocString DESC = "All gases are sent into the building's output pipe, except the gas chosen for filtering.";

				// Token: 0x04009BBB RID: 39867
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sieves one ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" from the air, sending it into a dedicated ",
					UI.FormatAsLink("Pipe", "GASPIPING"),
					"."
				});

				// Token: 0x04009BBC RID: 39868
				public static LocString STATUS_ITEM = "Filters: {0}";

				// Token: 0x04009BBD RID: 39869
				public static LocString ELEMENT_NOT_SPECIFIED = "Not Specified";
			}

			// Token: 0x020022F6 RID: 8950
			public class SOLIDFILTER
			{
				// Token: 0x04009BBE RID: 39870
				public static LocString NAME = UI.FormatAsLink("Solid Filter", "SOLIDFILTER");

				// Token: 0x04009BBF RID: 39871
				public static LocString DESC = "All solids are sent into the building's output conveyor, except the solid chosen for filtering.";

				// Token: 0x04009BC0 RID: 39872
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Separates one ",
					UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID"),
					" from the conveyor, sending it into a dedicated ",
					BUILDINGS.PREFABS.SOLIDCONDUIT.NAME,
					"."
				});

				// Token: 0x04009BC1 RID: 39873
				public static LocString STATUS_ITEM = "Filters: {0}";

				// Token: 0x04009BC2 RID: 39874
				public static LocString ELEMENT_NOT_SPECIFIED = "Not Specified";
			}

			// Token: 0x020022F7 RID: 8951
			public class GASPERMEABLEMEMBRANE
			{
				// Token: 0x04009BC3 RID: 39875
				public static LocString NAME = UI.FormatAsLink("Airflow Tile", "GASPERMEABLEMEMBRANE");

				// Token: 0x04009BC4 RID: 39876
				public static LocString DESC = "Building with airflow tile promotes better gas circulation within a colony.";

				// Token: 0x04009BC5 RID: 39877
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nBlocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" flow without obstructing ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x020022F8 RID: 8952
			public class DEVPUMPGAS
			{
				// Token: 0x04009BC6 RID: 39878
				public static LocString NAME = UI.FormatAsLink("Dev Pump Gas", "DEVPUMPGAS");

				// Token: 0x04009BC7 RID: 39879
				public static LocString DESC = "Piping a pump's output to a building's intake will send gas to that building.";

				// Token: 0x04009BC8 RID: 39880
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					".\n\nMust be immersed in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x020022F9 RID: 8953
			public class GASPUMP
			{
				// Token: 0x04009BC9 RID: 39881
				public static LocString NAME = UI.FormatAsLink("Gas Pump", "GASPUMP");

				// Token: 0x04009BCA RID: 39882
				public static LocString DESC = "Piping a pump's output to a building's intake will send gas to that building.";

				// Token: 0x04009BCB RID: 39883
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					".\n\nMust be immersed in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x020022FA RID: 8954
			public class GASMINIPUMP
			{
				// Token: 0x04009BCC RID: 39884
				public static LocString NAME = UI.FormatAsLink("Mini Gas Pump", "GASMINIPUMP");

				// Token: 0x04009BCD RID: 39885
				public static LocString DESC = "Mini pumps are useful for moving small quantities of gas with minimum power.";

				// Token: 0x04009BCE RID: 39886
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in a small amount of ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					".\n\nMust be immersed in ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					"."
				});
			}

			// Token: 0x020022FB RID: 8955
			public class GASVALVE
			{
				// Token: 0x04009BCF RID: 39887
				public static LocString NAME = UI.FormatAsLink("Gas Valve", "GASVALVE");

				// Token: 0x04009BD0 RID: 39888
				public static LocString DESC = "Valves control the amount of gas that moves through pipes, preventing waste.";

				// Token: 0x04009BD1 RID: 39889
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Controls the ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" volume permitted through ",
					UI.FormatAsLink("Pipes", "GASPIPING"),
					"."
				});
			}

			// Token: 0x020022FC RID: 8956
			public class GASLOGICVALVE
			{
				// Token: 0x04009BD2 RID: 39890
				public static LocString NAME = UI.FormatAsLink("Gas Shutoff", "GASLOGICVALVE");

				// Token: 0x04009BD3 RID: 39891
				public static LocString DESC = "Automated piping saves power and time by removing the need for Duplicant input.";

				// Token: 0x04009BD4 RID: 39892
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow on or off."
				});

				// Token: 0x04009BD5 RID: 39893
				public static LocString LOGIC_PORT = "Open/Close";

				// Token: 0x04009BD6 RID: 39894
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow gas flow";

				// Token: 0x04009BD7 RID: 39895
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent gas flow";
			}

			// Token: 0x020022FD RID: 8957
			public class GASLIMITVALVE
			{
				// Token: 0x04009BD8 RID: 39896
				public static LocString NAME = UI.FormatAsLink("Gas Meter Valve", "GASLIMITVALVE");

				// Token: 0x04009BD9 RID: 39897
				public static LocString DESC = "Meter Valves let an exact amount of gas pass through before shutting off.";

				// Token: 0x04009BDA RID: 39898
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow off when the specified amount has passed through it."
				});

				// Token: 0x04009BDB RID: 39899
				public static LocString LOGIC_PORT_OUTPUT = "Limit Reached";

				// Token: 0x04009BDC RID: 39900
				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if limit has been reached";

				// Token: 0x04009BDD RID: 39901
				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x04009BDE RID: 39902
				public static LocString LOGIC_PORT_RESET = "Reset Meter";

				// Token: 0x04009BDF RID: 39903
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the amount";

				// Token: 0x04009BE0 RID: 39904
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x020022FE RID: 8958
			public class GASVENT
			{
				// Token: 0x04009BE1 RID: 39905
				public static LocString NAME = UI.FormatAsLink("Gas Vent", "GASVENT");

				// Token: 0x04009BE2 RID: 39906
				public static LocString DESC = "Vents are an exit point for gases from ventilation systems.";

				// Token: 0x04009BE3 RID: 39907
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Releases ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" from ",
					UI.FormatAsLink("Gas Pipes", "GASPIPING"),
					"."
				});
			}

			// Token: 0x020022FF RID: 8959
			public class GASVENTHIGHPRESSURE
			{
				// Token: 0x04009BE4 RID: 39908
				public static LocString NAME = UI.FormatAsLink("High Pressure Gas Vent", "GASVENTHIGHPRESSURE");

				// Token: 0x04009BE5 RID: 39909
				public static LocString DESC = "High pressure vents can expel gas into more highly pressurized environments.";

				// Token: 0x04009BE6 RID: 39910
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Releases ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" from ",
					UI.FormatAsLink("Gas Pipes", "GASPIPING"),
					" into high pressure locations."
				});
			}

			// Token: 0x02002300 RID: 8960
			public class GASBOTTLER
			{
				// Token: 0x04009BE7 RID: 39911
				public static LocString NAME = UI.FormatAsLink("Canister Filler", "GASBOTTLER");

				// Token: 0x04009BE8 RID: 39912
				public static LocString DESC = "Canisters allow Duplicants to manually deliver gases from place to place.";

				// Token: 0x04009BE9 RID: 39913
				public static LocString EFFECT = "Automatically stores piped " + UI.FormatAsLink("Gases", "ELEMENTS_GAS") + " into canisters for manual transport.";
			}

			// Token: 0x02002301 RID: 8961
			public class GENERATOR
			{
				// Token: 0x04009BEA RID: 39914
				public static LocString NAME = UI.FormatAsLink("Coal Generator", "GENERATOR");

				// Token: 0x04009BEB RID: 39915
				public static LocString DESC = "Burning coal produces more energy than manual power, but emits heat and exhaust.";

				// Token: 0x04009BEC RID: 39916
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Coal", "CARBON"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					"."
				});

				// Token: 0x04009BED RID: 39917
				public static LocString OVERPRODUCTION = "{Generator} overproduction";
			}

			// Token: 0x02002302 RID: 8962
			public class GENETICANALYSISSTATION
			{
				// Token: 0x04009BEE RID: 39918
				public static LocString NAME = UI.FormatAsLink("Botanical Analyzer", "GENETICANALYSISSTATION");

				// Token: 0x04009BEF RID: 39919
				public static LocString DESC = "Would a mutated rose still smell as sweet?";

				// Token: 0x04009BF0 RID: 39920
				public static LocString EFFECT = "Identifies new " + UI.FormatAsLink("Seed", "PLANTS") + " subspecies.";
			}

			// Token: 0x02002303 RID: 8963
			public class DEVGENERATOR
			{
				// Token: 0x04009BF1 RID: 39921
				public static LocString NAME = "Dev Generator";

				// Token: 0x04009BF2 RID: 39922
				public static LocString DESC = "Runs on coffee.";

				// Token: 0x04009BF3 RID: 39923
				public static LocString EFFECT = "Generates testing power for late nights.";
			}

			// Token: 0x02002304 RID: 8964
			public class DEVLIFESUPPORT
			{
				// Token: 0x04009BF4 RID: 39924
				public static LocString NAME = "Dev Life Support";

				// Token: 0x04009BF5 RID: 39925
				public static LocString DESC = "Keeps Duplicants cozy and breathing.";

				// Token: 0x04009BF6 RID: 39926
				public static LocString EFFECT = "Generates warm, oxygen-rich air.";
			}

			// Token: 0x02002305 RID: 8965
			public class DEVLIGHTGENERATOR
			{
				// Token: 0x04009BF7 RID: 39927
				public static LocString NAME = "Dev Light Source";

				// Token: 0x04009BF8 RID: 39928
				public static LocString DESC = "Brightens up a dev's darkest hours.";

				// Token: 0x04009BF9 RID: 39929
				public static LocString EFFECT = "Generates dimmable light on demand.";

				// Token: 0x04009BFA RID: 39930
				public static LocString FALLOFF_LABEL = "Falloff Rate";

				// Token: 0x04009BFB RID: 39931
				public static LocString BRIGHTNESS_LABEL = "Brightness";

				// Token: 0x04009BFC RID: 39932
				public static LocString RANGE_LABEL = "Range";
			}

			// Token: 0x02002306 RID: 8966
			public class DEVRADIATIONGENERATOR
			{
				// Token: 0x04009BFD RID: 39933
				public static LocString NAME = "Dev Radiation Emitter";

				// Token: 0x04009BFE RID: 39934
				public static LocString DESC = "That's some <i>strong</i> coffee.";

				// Token: 0x04009BFF RID: 39935
				public static LocString EFFECT = "Generates on-demand radiation to keep you cozy.";
			}

			// Token: 0x02002307 RID: 8967
			public class GENERICFABRICATOR
			{
				// Token: 0x04009C00 RID: 39936
				public static LocString NAME = UI.FormatAsLink("Omniprinter", "GENERICFABRICATOR");

				// Token: 0x04009C01 RID: 39937
				public static LocString DESC = "Omniprinters are incapable of printing organic matter.";

				// Token: 0x04009C02 RID: 39938
				public static LocString EFFECT = "Converts " + UI.FormatAsLink("Raw Mineral", "RAWMINERAL") + " into unique materials and objects.";
			}

			// Token: 0x02002308 RID: 8968
			public class GEOTUNER
			{
				// Token: 0x04009C03 RID: 39939
				public static LocString NAME = UI.FormatAsLink("Geotuner", "GEOTUNER");

				// Token: 0x04009C04 RID: 39940
				public static LocString DESC = "The targeted geyser receives stored amplification data when it is erupting.";

				// Token: 0x04009C05 RID: 39941
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases the ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" and output of an analyzed ",
					UI.FormatAsLink("Geyser", "GEYSERS"),
					".\n\nMultiple Geotuners can be directed at a single ",
					UI.FormatAsLink("Geyser", "GEYSERS"),
					" anywhere on an asteroid."
				});

				// Token: 0x04009C06 RID: 39942
				public static LocString LOGIC_PORT = "Geyser Eruption Monitor";

				// Token: 0x04009C07 RID: 39943
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when geyser is erupting";

				// Token: 0x04009C08 RID: 39944
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002309 RID: 8969
			public class GRAVE
			{
				// Token: 0x04009C09 RID: 39945
				public static LocString NAME = UI.FormatAsLink("Tasteful Memorial", "GRAVE");

				// Token: 0x04009C0A RID: 39946
				public static LocString DESC = "Burying dead Duplicants reduces health hazards and stress on the colony.";

				// Token: 0x04009C0B RID: 39947
				public static LocString EFFECT = "Provides a final resting place for deceased Duplicants.\n\nLiving Duplicants will automatically place an unburied corpse inside.";
			}

			// Token: 0x0200230A RID: 8970
			public class HEADQUARTERS
			{
				// Token: 0x04009C0C RID: 39948
				public static LocString NAME = UI.FormatAsLink("Printing Pod", "HEADQUARTERS");

				// Token: 0x04009C0D RID: 39949
				public static LocString DESC = "New Duplicants come out here, but thank goodness, they never go back in.";

				// Token: 0x04009C0E RID: 39950
				public static LocString EFFECT = "An exceptionally advanced bioprinter of unknown origin.\n\nIt periodically produces new Duplicants or care packages containing resources.";
			}

			// Token: 0x0200230B RID: 8971
			public class HYDROGENGENERATOR
			{
				// Token: 0x04009C0F RID: 39951
				public static LocString NAME = UI.FormatAsLink("Hydrogen Generator", "HYDROGENGENERATOR");

				// Token: 0x04009C10 RID: 39952
				public static LocString DESC = "Hydrogen generators are extremely efficient, emitting next to no waste.";

				// Token: 0x04009C11 RID: 39953
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					"."
				});
			}

			// Token: 0x0200230C RID: 8972
			public class METHANEGENERATOR
			{
				// Token: 0x04009C12 RID: 39954
				public static LocString NAME = UI.FormatAsLink("Natural Gas Generator", "METHANEGENERATOR");

				// Token: 0x04009C13 RID: 39955
				public static LocString DESC = "Natural gas generators leak polluted water and are best built above a waste reservoir.";

				// Token: 0x04009C14 RID: 39956
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Natural Gas", "METHANE"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"."
				});
			}

			// Token: 0x0200230D RID: 8973
			public class NUCLEARREACTOR
			{
				// Token: 0x04009C15 RID: 39957
				public static LocString NAME = UI.FormatAsLink("Research Reactor", "NUCLEARREACTOR");

				// Token: 0x04009C16 RID: 39958
				public static LocString DESC = "Radbolt generators and reflectors make radiation useable by other buildings.";

				// Token: 0x04009C17 RID: 39959
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" to produce ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" for Radbolt production.\n\nGenerates a massive amount of ",
					UI.FormatAsLink("Heat", "HEAT"),
					". Overheating will result in an explosive meltdown."
				});

				// Token: 0x04009C18 RID: 39960
				public static LocString LOGIC_PORT = "Fuel Delivery Control";

				// Token: 0x04009C19 RID: 39961
				public static LocString INPUT_PORT_ACTIVE = "Fuel Delivery Enabled";

				// Token: 0x04009C1A RID: 39962
				public static LocString INPUT_PORT_INACTIVE = "Fuel Delivery Disabled";
			}

			// Token: 0x0200230E RID: 8974
			public class WOODGASGENERATOR
			{
				// Token: 0x04009C1B RID: 39963
				public static LocString NAME = UI.FormatAsLink("Wood Burner", "WOODGASGENERATOR");

				// Token: 0x04009C1C RID: 39964
				public static LocString DESC = "Wood burners are small and easy to maintain, but produce a fair amount of heat.";

				// Token: 0x04009C1D RID: 39965
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Lumber", "WOOD"),
					" to produce electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					UI.FormatAsLink("Heat", "HEAT"),
					"."
				});
			}

			// Token: 0x0200230F RID: 8975
			public class PETROLEUMGENERATOR
			{
				// Token: 0x04009C1E RID: 39966
				public static LocString NAME = UI.FormatAsLink("Petroleum Generator", "PETROLEUMGENERATOR");

				// Token: 0x04009C1F RID: 39967
				public static LocString DESC = "Petroleum generators have a high energy output but produce a great deal of waste.";

				// Token: 0x04009C20 RID: 39968
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts either ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" or ",
					UI.FormatAsLink("Ethanol", "ETHANOL"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nProduces ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					" and ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"."
				});
			}

			// Token: 0x02002310 RID: 8976
			public class HYDROPONICFARM
			{
				// Token: 0x04009C21 RID: 39969
				public static LocString NAME = UI.FormatAsLink("Hydroponic Farm", "HYDROPONICFARM");

				// Token: 0x04009C22 RID: 39970
				public static LocString DESC = "Hydroponic farms reduce Duplicant traffic by automating irrigating crops.";

				// Token: 0x04009C23 RID: 39971
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Grows one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" from a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nCan be used as floor tile and rotated before construction.\n\nMust be irrigated through ",
					UI.FormatAsLink("Liquid Piping", "LIQUIDPIPING"),
					"."
				});
			}

			// Token: 0x02002311 RID: 8977
			public class INSULATEDGASCONDUIT
			{
				// Token: 0x04009C24 RID: 39972
				public static LocString NAME = UI.FormatAsLink("Insulated Gas Pipe", "INSULATEDGASCONDUIT");

				// Token: 0x04009C25 RID: 39973
				public static LocString DESC = "Pipe insulation prevents gas contents from significantly changing temperature in transit.";

				// Token: 0x04009C26 RID: 39974
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" with minimal change in ",
					UI.FormatAsLink("Temperature", "HEAT"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002312 RID: 8978
			public class GASCONDUITRADIANT
			{
				// Token: 0x04009C27 RID: 39975
				public static LocString NAME = UI.FormatAsLink("Radiant Gas Pipe", "GASCONDUITRADIANT");

				// Token: 0x04009C28 RID: 39976
				public static LocString DESC = "Radiant pipes pumping cold gas can be run through hot areas to help cool them down.";

				// Token: 0x04009C29 RID: 39977
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					", allowing extreme ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" exchange with the surrounding environment.\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002313 RID: 8979
			public class INSULATEDLIQUIDCONDUIT
			{
				// Token: 0x04009C2A RID: 39978
				public static LocString NAME = UI.FormatAsLink("Insulated Liquid Pipe", "INSULATEDLIQUIDCONDUIT");

				// Token: 0x04009C2B RID: 39979
				public static LocString DESC = "Pipe insulation prevents liquid contents from significantly changing temperature in transit.";

				// Token: 0x04009C2C RID: 39980
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" with minimal change in ",
					UI.FormatAsLink("Temperature", "HEAT"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002314 RID: 8980
			public class LIQUIDCONDUITRADIANT
			{
				// Token: 0x04009C2D RID: 39981
				public static LocString NAME = UI.FormatAsLink("Radiant Liquid Pipe", "LIQUIDCONDUITRADIANT");

				// Token: 0x04009C2E RID: 39982
				public static LocString DESC = "Radiant pipes pumping cold liquid can be run through hot areas to help cool them down.";

				// Token: 0x04009C2F RID: 39983
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", allowing extreme ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" exchange with the surrounding environment.\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002315 RID: 8981
			public class CONTACTCONDUCTIVEPIPEBRIDGE
			{
				// Token: 0x04009C30 RID: 39984
				public static LocString NAME = "Conduction Panel";

				// Token: 0x04009C31 RID: 39985
				public static LocString DESC = "It can transfer heat effectively even if no liquid is passing through.";

				// Token: 0x04009C32 RID: 39986
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", allowing extreme ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" exchange with overlapping buildings.\n\nCan function in a vacuum.\n\nCan be run through wall and floor tiles."
				});
			}

			// Token: 0x02002316 RID: 8982
			public class INSULATEDWIRE
			{
				// Token: 0x04009C33 RID: 39987
				public static LocString NAME = UI.FormatAsLink("Insulated Wire", "INSULATEDWIRE");

				// Token: 0x04009C34 RID: 39988
				public static LocString DESC = "This stuff won't go melting if things get heated.";

				// Token: 0x04009C35 RID: 39989
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects buildings to ",
					UI.FormatAsLink("Power", "POWER"),
					" sources in extreme ",
					UI.FormatAsLink("Heat", "HEAT"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002317 RID: 8983
			public class INSULATIONTILE
			{
				// Token: 0x04009C36 RID: 39990
				public static LocString NAME = UI.FormatAsLink("Insulated Tile", "INSULATIONTILE");

				// Token: 0x04009C37 RID: 39991
				public static LocString DESC = "The low thermal conductivity of insulated tiles slows any heat passing through them.";

				// Token: 0x04009C38 RID: 39992
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nReduces " + UI.FormatAsLink("Heat", "HEAT") + " transfer between walls, retaining ambient heat in an area.";
			}

			// Token: 0x02002318 RID: 8984
			public class EXTERIORWALL
			{
				// Token: 0x04009C39 RID: 39993
				public static LocString NAME = UI.FormatAsLink("Drywall", "EXTERIORWALL");

				// Token: 0x04009C3A RID: 39994
				public static LocString DESC = "Drywall can be used in conjunction with tiles to build airtight rooms on the surface.";

				// Token: 0x04009C3B RID: 39995
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Prevents ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" loss in space.\n\nBuilds an insulating backwall behind buildings."
				});

				// Token: 0x02002F7D RID: 12157
				public class FACADES
				{
					// Token: 0x02003250 RID: 12880
					public class DEFAULT_EXTERIORWALL
					{
						// Token: 0x0400C902 RID: 51458
						public static LocString NAME = UI.FormatAsLink("Drywall", "EXTERIORWALL");

						// Token: 0x0400C903 RID: 51459
						public static LocString DESC = "It gets the job done.";
					}

					// Token: 0x02003251 RID: 12881
					public class BALM_LILY
					{
						// Token: 0x0400C904 RID: 51460
						public static LocString NAME = UI.FormatAsLink("Balm Lily Print", "EXTERIORWALL");

						// Token: 0x0400C905 RID: 51461
						public static LocString DESC = "A mellow floral wallpaper.";
					}

					// Token: 0x02003252 RID: 12882
					public class CLOUDS
					{
						// Token: 0x0400C906 RID: 51462
						public static LocString NAME = UI.FormatAsLink("Cloud Print", "EXTERIORWALL");

						// Token: 0x0400C907 RID: 51463
						public static LocString DESC = "A soft, fluffy wallpaper.";
					}

					// Token: 0x02003253 RID: 12883
					public class MUSHBAR
					{
						// Token: 0x0400C908 RID: 51464
						public static LocString NAME = UI.FormatAsLink("Mush Bar Print", "EXTERIORWALL");

						// Token: 0x0400C909 RID: 51465
						public static LocString DESC = "A gag-inducing wallpaper.";
					}

					// Token: 0x02003254 RID: 12884
					public class PLAID
					{
						// Token: 0x0400C90A RID: 51466
						public static LocString NAME = UI.FormatAsLink("Aqua Plaid Print", "EXTERIORWALL");

						// Token: 0x0400C90B RID: 51467
						public static LocString DESC = "A cozy flannel wallpaper.";
					}

					// Token: 0x02003255 RID: 12885
					public class RAIN
					{
						// Token: 0x0400C90C RID: 51468
						public static LocString NAME = UI.FormatAsLink("Rainy Print", "EXTERIORWALL");

						// Token: 0x0400C90D RID: 51469
						public static LocString DESC = "A precipitation-themed wallpaper.";
					}

					// Token: 0x02003256 RID: 12886
					public class AQUATICMOSAIC
					{
						// Token: 0x0400C90E RID: 51470
						public static LocString NAME = UI.FormatAsLink("Aquatic Mosaic", "EXTERIORWALL");

						// Token: 0x0400C90F RID: 51471
						public static LocString DESC = "A multi-hued blue wallpaper.";
					}

					// Token: 0x02003257 RID: 12887
					public class RAINBOW
					{
						// Token: 0x0400C910 RID: 51472
						public static LocString NAME = UI.FormatAsLink("Rainbow Stripe", "EXTERIORWALL");

						// Token: 0x0400C911 RID: 51473
						public static LocString DESC = "A wallpaper with <i>all</i> the colors.";
					}

					// Token: 0x02003258 RID: 12888
					public class SNOW
					{
						// Token: 0x0400C912 RID: 51474
						public static LocString NAME = UI.FormatAsLink("Snowflake Print", "EXTERIORWALL");

						// Token: 0x0400C913 RID: 51475
						public static LocString DESC = "A wallpaper as unique as my colony.";
					}

					// Token: 0x02003259 RID: 12889
					public class SUN
					{
						// Token: 0x0400C914 RID: 51476
						public static LocString NAME = UI.FormatAsLink("Sunshine Print", "EXTERIORWALL");

						// Token: 0x0400C915 RID: 51477
						public static LocString DESC = "A UV-free wallpaper.";
					}

					// Token: 0x0200325A RID: 12890
					public class COFFEE
					{
						// Token: 0x0400C916 RID: 51478
						public static LocString NAME = UI.FormatAsLink("Cafe Print", "EXTERIORWALL");

						// Token: 0x0400C917 RID: 51479
						public static LocString DESC = "A caffeine-themed wallpaper.";
					}

					// Token: 0x0200325B RID: 12891
					public class PASTELPOLKA
					{
						// Token: 0x0400C918 RID: 51480
						public static LocString NAME = UI.FormatAsLink("Pastel Polka Print", "EXTERIORWALL");

						// Token: 0x0400C919 RID: 51481
						public static LocString DESC = "A soothing, dotted wallpaper.";
					}

					// Token: 0x0200325C RID: 12892
					public class PASTELBLUE
					{
						// Token: 0x0400C91A RID: 51482
						public static LocString NAME = UI.FormatAsLink("Pastel Blue", "EXTERIORWALL");

						// Token: 0x0400C91B RID: 51483
						public static LocString DESC = "A soothing blue wallpaper.";
					}

					// Token: 0x0200325D RID: 12893
					public class PASTELGREEN
					{
						// Token: 0x0400C91C RID: 51484
						public static LocString NAME = UI.FormatAsLink("Pastel Green", "EXTERIORWALL");

						// Token: 0x0400C91D RID: 51485
						public static LocString DESC = "A soothing green wallpaper.";
					}

					// Token: 0x0200325E RID: 12894
					public class PASTELPINK
					{
						// Token: 0x0400C91E RID: 51486
						public static LocString NAME = UI.FormatAsLink("Pastel Pink", "EXTERIORWALL");

						// Token: 0x0400C91F RID: 51487
						public static LocString DESC = "A soothing pink wallpaper.";
					}

					// Token: 0x0200325F RID: 12895
					public class PASTELPURPLE
					{
						// Token: 0x0400C920 RID: 51488
						public static LocString NAME = UI.FormatAsLink("Pastel Purple", "EXTERIORWALL");

						// Token: 0x0400C921 RID: 51489
						public static LocString DESC = "A soothing purple wallpaper.";
					}

					// Token: 0x02003260 RID: 12896
					public class PASTELYELLOW
					{
						// Token: 0x0400C922 RID: 51490
						public static LocString NAME = UI.FormatAsLink("Pastel Yellow", "EXTERIORWALL");

						// Token: 0x0400C923 RID: 51491
						public static LocString DESC = "A soothing yellow wallpaper.";
					}

					// Token: 0x02003261 RID: 12897
					public class BASIC_WHITE
					{
						// Token: 0x0400C924 RID: 51492
						public static LocString NAME = UI.FormatAsLink("Fresh White", "EXTERIORWALL");

						// Token: 0x0400C925 RID: 51493
						public static LocString DESC = "It's just so fresh and so clean.";
					}

					// Token: 0x02003262 RID: 12898
					public class DIAGONAL_RED_DEEP_WHITE
					{
						// Token: 0x0400C926 RID: 51494
						public static LocString NAME = UI.FormatAsLink("Magma Diagonal", "EXTERIORWALL");

						// Token: 0x0400C927 RID: 51495
						public static LocString DESC = "A red wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003263 RID: 12899
					public class DIAGONAL_ORANGE_SATSUMA_WHITE
					{
						// Token: 0x0400C928 RID: 51496
						public static LocString NAME = UI.FormatAsLink("Bright Diagonal", "EXTERIORWALL");

						// Token: 0x0400C929 RID: 51497
						public static LocString DESC = "An orange wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003264 RID: 12900
					public class DIAGONAL_YELLOW_LEMON_WHITE
					{
						// Token: 0x0400C92A RID: 51498
						public static LocString NAME = UI.FormatAsLink("Yellowcake Diagonal", "EXTERIORWALL");

						// Token: 0x0400C92B RID: 51499
						public static LocString DESC = "A radiation-free wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003265 RID: 12901
					public class DIAGONAL_GREEN_KELLY_WHITE
					{
						// Token: 0x0400C92C RID: 51500
						public static LocString NAME = UI.FormatAsLink("Algae Diagonal", "EXTERIORWALL");

						// Token: 0x0400C92D RID: 51501
						public static LocString DESC = "A slippery wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003266 RID: 12902
					public class DIAGONAL_BLUE_COBALT_WHITE
					{
						// Token: 0x0400C92E RID: 51502
						public static LocString NAME = UI.FormatAsLink("H2O Diagonal", "EXTERIORWALL");

						// Token: 0x0400C92F RID: 51503
						public static LocString DESC = "A damp wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003267 RID: 12903
					public class DIAGONAL_PINK_FLAMINGO_WHITE
					{
						// Token: 0x0400C930 RID: 51504
						public static LocString NAME = UI.FormatAsLink("Petal Diagonal", "EXTERIORWALL");

						// Token: 0x0400C931 RID: 51505
						public static LocString DESC = "A pink wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003268 RID: 12904
					public class DIAGONAL_GREY_CHARCOAL_WHITE
					{
						// Token: 0x0400C932 RID: 51506
						public static LocString NAME = UI.FormatAsLink("Charcoal Diagonal", "EXTERIORWALL");

						// Token: 0x0400C933 RID: 51507
						public static LocString DESC = "A sleek wallpaper with a diagonal stripe.";
					}

					// Token: 0x02003269 RID: 12905
					public class CIRCLE_RED_DEEP_WHITE
					{
						// Token: 0x0400C934 RID: 51508
						public static LocString NAME = UI.FormatAsLink("Magma Wedge", "EXTERIORWALL");

						// Token: 0x0400C935 RID: 51509
						public static LocString DESC = "It can be arranged into giant red polka dots.";
					}

					// Token: 0x0200326A RID: 12906
					public class CIRCLE_ORANGE_SATSUMA_WHITE
					{
						// Token: 0x0400C936 RID: 51510
						public static LocString NAME = UI.FormatAsLink("Bright Wedge", "EXTERIORWALL");

						// Token: 0x0400C937 RID: 51511
						public static LocString DESC = "It can be arranged into giant orange polka dots.";
					}

					// Token: 0x0200326B RID: 12907
					public class CIRCLE_YELLOW_LEMON_WHITE
					{
						// Token: 0x0400C938 RID: 51512
						public static LocString NAME = UI.FormatAsLink("Yellowcake Wedge", "EXTERIORWALL");

						// Token: 0x0400C939 RID: 51513
						public static LocString DESC = "A radiation-free pattern that can be arranged into giant polka dots.";
					}

					// Token: 0x0200326C RID: 12908
					public class CIRCLE_GREEN_KELLY_WHITE
					{
						// Token: 0x0400C93A RID: 51514
						public static LocString NAME = UI.FormatAsLink("Algae Wedge", "EXTERIORWALL");

						// Token: 0x0400C93B RID: 51515
						public static LocString DESC = "It can be arranged into giant green polka dots.";
					}

					// Token: 0x0200326D RID: 12909
					public class CIRCLE_BLUE_COBALT_WHITE
					{
						// Token: 0x0400C93C RID: 51516
						public static LocString NAME = UI.FormatAsLink("H2O Wedge", "EXTERIORWALL");

						// Token: 0x0400C93D RID: 51517
						public static LocString DESC = "It can be arranged into giant blue polka dots.";
					}

					// Token: 0x0200326E RID: 12910
					public class CIRCLE_PINK_FLAMINGO_WHITE
					{
						// Token: 0x0400C93E RID: 51518
						public static LocString NAME = UI.FormatAsLink("Petal Wedge", "EXTERIORWALL");

						// Token: 0x0400C93F RID: 51519
						public static LocString DESC = "It can be arranged into giant pink polka dots.";
					}

					// Token: 0x0200326F RID: 12911
					public class CIRCLE_GREY_CHARCOAL_WHITE
					{
						// Token: 0x0400C940 RID: 51520
						public static LocString NAME = UI.FormatAsLink("Charcoal Wedge", "EXTERIORWALL");

						// Token: 0x0400C941 RID: 51521
						public static LocString DESC = "It can be arranged into giant shadowy polka dots.";
					}

					// Token: 0x02003270 RID: 12912
					public class BASIC_BLUE_COBALT
					{
						// Token: 0x0400C942 RID: 51522
						public static LocString NAME = UI.FormatAsLink("Solid Cobalt", "EXTERIORWALL");

						// Token: 0x0400C943 RID: 51523
						public static LocString DESC = "It doesn't cure the blues, so much as emphasize them.";
					}

					// Token: 0x02003271 RID: 12913
					public class BASIC_GREEN_KELLY
					{
						// Token: 0x0400C944 RID: 51524
						public static LocString NAME = UI.FormatAsLink("Spring Green", "EXTERIORWALL");

						// Token: 0x0400C945 RID: 51525
						public static LocString DESC = "It's cheaper than having a garden.";
					}

					// Token: 0x02003272 RID: 12914
					public class BASIC_GREY_CHARCOAL
					{
						// Token: 0x0400C946 RID: 51526
						public static LocString NAME = UI.FormatAsLink("Solid Charcoal", "EXTERIORWALL");

						// Token: 0x0400C947 RID: 51527
						public static LocString DESC = "An elevated take on \"gray\".";
					}

					// Token: 0x02003273 RID: 12915
					public class BASIC_ORANGE_SATSUMA
					{
						// Token: 0x0400C948 RID: 51528
						public static LocString NAME = UI.FormatAsLink("Solid Satsuma", "EXTERIORWALL");

						// Token: 0x0400C949 RID: 51529
						public static LocString DESC = "Less fruit-forward, but just as fresh.";
					}

					// Token: 0x02003274 RID: 12916
					public class BASIC_PINK_FLAMINGO
					{
						// Token: 0x0400C94A RID: 51530
						public static LocString NAME = UI.FormatAsLink("Solid Pink", "EXTERIORWALL");

						// Token: 0x0400C94B RID: 51531
						public static LocString DESC = "A bold statement, for bold Duplicants.";
					}

					// Token: 0x02003275 RID: 12917
					public class BASIC_RED_DEEP
					{
						// Token: 0x0400C94C RID: 51532
						public static LocString NAME = UI.FormatAsLink("Chili Red", "EXTERIORWALL");

						// Token: 0x0400C94D RID: 51533
						public static LocString DESC = "It really spices up dull walls.";
					}

					// Token: 0x02003276 RID: 12918
					public class BASIC_YELLOW_LEMON
					{
						// Token: 0x0400C94E RID: 51534
						public static LocString NAME = UI.FormatAsLink("Canary Yellow", "EXTERIORWALL");

						// Token: 0x0400C94F RID: 51535
						public static LocString DESC = "The original coal-mine chic.";
					}

					// Token: 0x02003277 RID: 12919
					public class BLUEBERRIES
					{
						// Token: 0x0400C950 RID: 51536
						public static LocString NAME = UI.FormatAsLink("Juicy Blueberry", "EXTERIORWALL");

						// Token: 0x0400C951 RID: 51537
						public static LocString DESC = "It stains the fingers.";
					}

					// Token: 0x02003278 RID: 12920
					public class GRAPES
					{
						// Token: 0x0400C952 RID: 51538
						public static LocString NAME = UI.FormatAsLink("Grape Escape", "EXTERIORWALL");

						// Token: 0x0400C953 RID: 51539
						public static LocString DESC = "It's seedless, if that matters.";
					}

					// Token: 0x02003279 RID: 12921
					public class LEMON
					{
						// Token: 0x0400C954 RID: 51540
						public static LocString NAME = UI.FormatAsLink("Sour Lemon", "EXTERIORWALL");

						// Token: 0x0400C955 RID: 51541
						public static LocString DESC = "A bitter yet refreshing style.";
					}

					// Token: 0x0200327A RID: 12922
					public class LIME
					{
						// Token: 0x0400C956 RID: 51542
						public static LocString NAME = UI.FormatAsLink("Juicy Lime", "EXTERIORWALL");

						// Token: 0x0400C957 RID: 51543
						public static LocString DESC = "Contains no actual vitamin C.";
					}

					// Token: 0x0200327B RID: 12923
					public class SATSUMA
					{
						// Token: 0x0400C958 RID: 51544
						public static LocString NAME = UI.FormatAsLink("Satsuma Slice", "EXTERIORWALL");

						// Token: 0x0400C959 RID: 51545
						public static LocString DESC = "Adds some much-needed zest to the room.";
					}

					// Token: 0x0200327C RID: 12924
					public class STRAWBERRY
					{
						// Token: 0x0400C95A RID: 51546
						public static LocString NAME = UI.FormatAsLink("Strawberry Speckle", "EXTERIORWALL");

						// Token: 0x0400C95B RID: 51547
						public static LocString DESC = "Fruity freckles for naturally sweet spaces.";
					}

					// Token: 0x0200327D RID: 12925
					public class WATERMELON
					{
						// Token: 0x0400C95C RID: 51548
						public static LocString NAME = UI.FormatAsLink("Juicy Watermelon", "EXTERIORWALL");

						// Token: 0x0400C95D RID: 51549
						public static LocString DESC = "Far more practical than gluing real fruit on a wall.";
					}

					// Token: 0x0200327E RID: 12926
					public class TROPICAL
					{
						// Token: 0x0400C95E RID: 51550
						public static LocString NAME = UI.FormatAsLink("Sporechid Print", "EXTERIORWALL");

						// Token: 0x0400C95F RID: 51551
						public static LocString DESC = "The original scratch-and-sniff version was immediately recalled.";
					}

					// Token: 0x0200327F RID: 12927
					public class TOILETPAPER
					{
						// Token: 0x0400C960 RID: 51552
						public static LocString NAME = UI.FormatAsLink("De-loo-xe", "EXTERIORWALL");

						// Token: 0x0400C961 RID: 51553
						public static LocString DESC = "Softly undulating lines create an undeniable air of loo-xury.";
					}

					// Token: 0x02003280 RID: 12928
					public class PLUNGER
					{
						// Token: 0x0400C962 RID: 51554
						public static LocString NAME = UI.FormatAsLink("Plunger Print", "EXTERIORWALL");

						// Token: 0x0400C963 RID: 51555
						public static LocString DESC = "Unclogs one's creative impulses.";
					}

					// Token: 0x02003281 RID: 12929
					public class STRIPES_BLUE
					{
						// Token: 0x0400C964 RID: 51556
						public static LocString NAME = UI.FormatAsLink("Blue Awning Stripe", "EXTERIORWALL");

						// Token: 0x0400C965 RID: 51557
						public static LocString DESC = "Thick stripes in alternating shades of blue.";
					}

					// Token: 0x02003282 RID: 12930
					public class STRIPES_DIAGONAL_BLUE
					{
						// Token: 0x0400C966 RID: 51558
						public static LocString NAME = UI.FormatAsLink("Blue Regimental Stripe", "EXTERIORWALL");

						// Token: 0x0400C967 RID: 51559
						public static LocString DESC = "Inspired by the ties worn during intraoffice sports.";
					}

					// Token: 0x02003283 RID: 12931
					public class STRIPES_CIRCLE_BLUE
					{
						// Token: 0x0400C968 RID: 51560
						public static LocString NAME = UI.FormatAsLink("Blue Circle Stripe", "EXTERIORWALL");

						// Token: 0x0400C969 RID: 51561
						public static LocString DESC = "A stripe that curves to the right.";
					}

					// Token: 0x02003284 RID: 12932
					public class SQUARES_RED_DEEP_WHITE
					{
						// Token: 0x0400C96A RID: 51562
						public static LocString NAME = UI.FormatAsLink("Magma Checkers", "EXTERIORWALL");

						// Token: 0x0400C96B RID: 51563
						public static LocString DESC = "They're so hot right now!";
					}

					// Token: 0x02003285 RID: 12933
					public class SQUARES_ORANGE_SATSUMA_WHITE
					{
						// Token: 0x0400C96C RID: 51564
						public static LocString NAME = UI.FormatAsLink("Bright Checkers", "EXTERIORWALL");

						// Token: 0x0400C96D RID: 51565
						public static LocString DESC = "Every tile feels like four tiles!";
					}

					// Token: 0x02003286 RID: 12934
					public class SQUARES_YELLOW_LEMON_WHITE
					{
						// Token: 0x0400C96E RID: 51566
						public static LocString NAME = UI.FormatAsLink("Yellowcake Checkers", "EXTERIORWALL");

						// Token: 0x0400C96F RID: 51567
						public static LocString DESC = "Any brighter, and they'd be radioactive!";
					}

					// Token: 0x02003287 RID: 12935
					public class SQUARES_GREEN_KELLY_WHITE
					{
						// Token: 0x0400C970 RID: 51568
						public static LocString NAME = UI.FormatAsLink("Algae Checkers", "EXTERIORWALL");

						// Token: 0x0400C971 RID: 51569
						public static LocString DESC = "Now with real simulated algae color!";
					}

					// Token: 0x02003288 RID: 12936
					public class SQUARES_BLUE_COBALT_WHITE
					{
						// Token: 0x0400C972 RID: 51570
						public static LocString NAME = UI.FormatAsLink("H2O Checkers", "EXTERIORWALL");

						// Token: 0x0400C973 RID: 51571
						public static LocString DESC = "Drink it all in!";
					}

					// Token: 0x02003289 RID: 12937
					public class SQUARES_PINK_FLAMINGO_WHITE
					{
						// Token: 0x0400C974 RID: 51572
						public static LocString NAME = UI.FormatAsLink("Petal Checkers", "EXTERIORWALL");

						// Token: 0x0400C975 RID: 51573
						public static LocString DESC = "Fiercely fluorescent floral-inspired pink!";
					}

					// Token: 0x0200328A RID: 12938
					public class SQUARES_GREY_CHARCOAL_WHITE
					{
						// Token: 0x0400C976 RID: 51574
						public static LocString NAME = UI.FormatAsLink("Charcoal Checkers", "EXTERIORWALL");

						// Token: 0x0400C977 RID: 51575
						public static LocString DESC = "So retro!";
					}

					// Token: 0x0200328B RID: 12939
					public class KITCHEN_RETRO1
					{
						// Token: 0x0400C978 RID: 51576
						public static LocString NAME = UI.FormatAsLink("Cafeteria Kitsch", "EXTERIORWALL");

						// Token: 0x0400C979 RID: 51577
						public static LocString DESC = "Some diners find it nostalgic.";
					}

					// Token: 0x0200328C RID: 12940
					public class PLUS_RED_DEEP_WHITE
					{
						// Token: 0x0400C97A RID: 51578
						public static LocString NAME = UI.FormatAsLink("Digital Chili", "EXTERIORWALL");

						// Token: 0x0400C97B RID: 51579
						public static LocString DESC = "A pixelated red-and-white print.";
					}

					// Token: 0x0200328D RID: 12941
					public class PLUS_ORANGE_SATSUMA_WHITE
					{
						// Token: 0x0400C97C RID: 51580
						public static LocString NAME = UI.FormatAsLink("Digital Satsuma", "EXTERIORWALL");

						// Token: 0x0400C97D RID: 51581
						public static LocString DESC = "A pixelated orange-and-white print.";
					}

					// Token: 0x0200328E RID: 12942
					public class PLUS_YELLOW_LEMON_WHITE
					{
						// Token: 0x0400C97E RID: 51582
						public static LocString NAME = UI.FormatAsLink("Digital Lemon", "EXTERIORWALL");

						// Token: 0x0400C97F RID: 51583
						public static LocString DESC = "A pixelated yellow-and-white print.";
					}

					// Token: 0x0200328F RID: 12943
					public class PLUS_GREEN_KELLY_WHITE
					{
						// Token: 0x0400C980 RID: 51584
						public static LocString NAME = UI.FormatAsLink("Digital Lawn", "EXTERIORWALL");

						// Token: 0x0400C981 RID: 51585
						public static LocString DESC = "A pixelated green-and-white print.";
					}

					// Token: 0x02003290 RID: 12944
					public class PLUS_BLUE_COBALT_WHITE
					{
						// Token: 0x0400C982 RID: 51586
						public static LocString NAME = UI.FormatAsLink("Digital Cobalt", "EXTERIORWALL");

						// Token: 0x0400C983 RID: 51587
						public static LocString DESC = "A pixelated blue-and-white print.";
					}

					// Token: 0x02003291 RID: 12945
					public class PLUS_PINK_FLAMINGO_WHITE
					{
						// Token: 0x0400C984 RID: 51588
						public static LocString NAME = UI.FormatAsLink("Digital Pink", "EXTERIORWALL");

						// Token: 0x0400C985 RID: 51589
						public static LocString DESC = "A pixelated pink-and-white print.";
					}

					// Token: 0x02003292 RID: 12946
					public class PLUS_GREY_CHARCOAL_WHITE
					{
						// Token: 0x0400C986 RID: 51590
						public static LocString NAME = UI.FormatAsLink("Digital Charcoal", "EXTERIORWALL");

						// Token: 0x0400C987 RID: 51591
						public static LocString DESC = "It's futuristic, so it must be good.";
					}
				}
			}

			// Token: 0x02002319 RID: 8985
			public class FARMTILE
			{
				// Token: 0x04009C3C RID: 39996
				public static LocString NAME = UI.FormatAsLink("Farm Tile", "FARMTILE");

				// Token: 0x04009C3D RID: 39997
				public static LocString DESC = "Duplicants can deliver fertilizer and liquids to farm tiles, accelerating plant growth.";

				// Token: 0x04009C3E RID: 39998
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Grows one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" from a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					".\n\nCan be used as floor tile and rotated before construction."
				});
			}

			// Token: 0x0200231A RID: 8986
			public class LADDER
			{
				// Token: 0x04009C3F RID: 39999
				public static LocString NAME = UI.FormatAsLink("Ladder", "LADDER");

				// Token: 0x04009C40 RID: 40000
				public static LocString DESC = "(That means they climb it.)";

				// Token: 0x04009C41 RID: 40001
				public static LocString EFFECT = "Enables vertical mobility for Duplicants.";
			}

			// Token: 0x0200231B RID: 8987
			public class LADDERFAST
			{
				// Token: 0x04009C42 RID: 40002
				public static LocString NAME = UI.FormatAsLink("Plastic Ladder", "LADDERFAST");

				// Token: 0x04009C43 RID: 40003
				public static LocString DESC = "Plastic ladders are mildly antiseptic and can help limit the spread of germs in a colony.";

				// Token: 0x04009C44 RID: 40004
				public static LocString EFFECT = "Increases Duplicant climbing speed.";
			}

			// Token: 0x0200231C RID: 8988
			public class LIQUIDCONDUIT
			{
				// Token: 0x04009C45 RID: 40005
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe", "LIQUIDCONDUIT");

				// Token: 0x04009C46 RID: 40006
				public static LocString DESC = "Liquid pipes are used to connect the inputs and outputs of plumbed buildings.";

				// Token: 0x04009C47 RID: 40007
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" between ",
					UI.FormatAsLink("Outputs", "LIQUIDPIPING"),
					" and ",
					UI.FormatAsLink("Intakes", "LIQUIDPIPING"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x0200231D RID: 8989
			public class LIQUIDCONDUITBRIDGE
			{
				// Token: 0x04009C48 RID: 40008
				public static LocString NAME = UI.FormatAsLink("Liquid Bridge", "LIQUIDCONDUITBRIDGE");

				// Token: 0x04009C49 RID: 40009
				public static LocString DESC = "Separate pipe systems help prevent building damage caused by mingled pipe contents.";

				// Token: 0x04009C4A RID: 40010
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Liquid Pipe", "LIQUIDPIPING") + " section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x0200231E RID: 8990
			public class ICECOOLEDFAN
			{
				// Token: 0x04009C4B RID: 40011
				public static LocString NAME = UI.FormatAsLink("Ice-E Fan", "ICECOOLEDFAN");

				// Token: 0x04009C4C RID: 40012
				public static LocString DESC = "A Duplicant can work an Ice-E fan to temporarily cool small areas as needed.";

				// Token: 0x04009C4D RID: 40013
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Ice", "ICEORE"),
					" to dissipate a small amount of the ",
					UI.FormatAsLink("Heat", "HEAT"),
					"."
				});
			}

			// Token: 0x0200231F RID: 8991
			public class ICEMACHINE
			{
				// Token: 0x04009C4E RID: 40014
				public static LocString NAME = UI.FormatAsLink("Ice Maker", "ICEMACHINE");

				// Token: 0x04009C4F RID: 40015
				public static LocString DESC = "Ice makers can be used as a small renewable source of ice.";

				// Token: 0x04009C50 RID: 40016
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Water", "WATER"),
					" into ",
					UI.FormatAsLink("Ice", "ICE"),
					"."
				});
			}

			// Token: 0x02002320 RID: 8992
			public class LIQUIDCOOLEDFAN
			{
				// Token: 0x04009C51 RID: 40017
				public static LocString NAME = UI.FormatAsLink("Hydrofan", "LIQUIDCOOLEDFAN");

				// Token: 0x04009C52 RID: 40018
				public static LocString DESC = "A Duplicant can work a hydrofan to temporarily cool small areas as needed.";

				// Token: 0x04009C53 RID: 40019
				public static LocString EFFECT = "Dissipates a small amount of the " + UI.FormatAsLink("Heat", "HEAT") + ".";
			}

			// Token: 0x02002321 RID: 8993
			public class CREATURETRAP
			{
				// Token: 0x04009C54 RID: 40020
				public static LocString NAME = UI.FormatAsLink("Critter Trap", "CREATURETRAP");

				// Token: 0x04009C55 RID: 40021
				public static LocString DESC = "Critter traps cannot catch swimming or flying critters.";

				// Token: 0x04009C56 RID: 40022
				public static LocString EFFECT = "Captures a living " + UI.FormatAsLink("Critter", "CREATURES") + " for transport.\n\nSingle use.";
			}

			// Token: 0x02002322 RID: 8994
			public class CREATUREGROUNDTRAP
			{
				// Token: 0x04009C57 RID: 40023
				public static LocString NAME = UI.FormatAsLink("Critter Trap", "CREATURETRAP");

				// Token: 0x04009C58 RID: 40024
				public static LocString DESC = "It's designed for land critters, but flopping fish sometimes find their way in too.";

				// Token: 0x04009C59 RID: 40025
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Captures a living ",
					UI.FormatAsLink("Critter", "CREATURES"),
					" for transport.\n\nOnly Duplicants with the ",
					UI.FormatAsLink("Critter Ranching I", "RANCHING1"),
					" skill can arm this trap. It's reusable!"
				});
			}

			// Token: 0x02002323 RID: 8995
			public class CREATUREDELIVERYPOINT
			{
				// Token: 0x04009C5A RID: 40026
				public static LocString NAME = UI.FormatAsLink("Critter Drop-Off", "CREATUREDELIVERYPOINT");

				// Token: 0x04009C5B RID: 40027
				public static LocString DESC = "Duplicants automatically bring captured critters to these relocation points for release.";

				// Token: 0x04009C5C RID: 40028
				public static LocString EFFECT = "Releases trapped " + UI.FormatAsLink("Critters", "CREATURES") + " back into the world.\n\nCan be used multiple times.";
			}

			// Token: 0x02002324 RID: 8996
			public class LIQUIDFILTER
			{
				// Token: 0x04009C5D RID: 40029
				public static LocString NAME = UI.FormatAsLink("Liquid Filter", "LIQUIDFILTER");

				// Token: 0x04009C5E RID: 40030
				public static LocString DESC = "All liquids are sent into the building's output pipe, except the liquid chosen for filtering.";

				// Token: 0x04009C5F RID: 40031
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sieves one ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" out of a mix, sending it into a dedicated ",
					UI.FormatAsLink("Filtered Output Pipe", "LIQUIDPIPING"),
					".\n\nCan only filter one liquid type at a time."
				});
			}

			// Token: 0x02002325 RID: 8997
			public class DEVPUMPLIQUID
			{
				// Token: 0x04009C60 RID: 40032
				public static LocString NAME = UI.FormatAsLink("Dev Pump Liquid", "DEVPUMPLIQUID");

				// Token: 0x04009C61 RID: 40033
				public static LocString DESC = "Piping a pump's output to a building's intake will send liquid to that building.";

				// Token: 0x04009C62 RID: 40034
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					".\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			// Token: 0x02002326 RID: 8998
			public class LIQUIDPUMP
			{
				// Token: 0x04009C63 RID: 40035
				public static LocString NAME = UI.FormatAsLink("Liquid Pump", "LIQUIDPUMP");

				// Token: 0x04009C64 RID: 40036
				public static LocString DESC = "Piping a pump's output to a building's intake will send liquid to that building.";

				// Token: 0x04009C65 RID: 40037
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					".\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			// Token: 0x02002327 RID: 8999
			public class LIQUIDMINIPUMP
			{
				// Token: 0x04009C66 RID: 40038
				public static LocString NAME = UI.FormatAsLink("Mini Liquid Pump", "LIQUIDMINIPUMP");

				// Token: 0x04009C67 RID: 40039
				public static LocString DESC = "Mini pumps are useful for moving small quantities of liquid with minimum power.";

				// Token: 0x04009C68 RID: 40040
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in a small amount of ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and runs it through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					".\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			// Token: 0x02002328 RID: 9000
			public class LIQUIDPUMPINGSTATION
			{
				// Token: 0x04009C69 RID: 40041
				public static LocString NAME = UI.FormatAsLink("Pitcher Pump", "LIQUIDPUMPINGSTATION");

				// Token: 0x04009C6A RID: 40042
				public static LocString DESC = "Pitcher pumps allow Duplicants to bottle and deliver liquids from place to place.";

				// Token: 0x04009C6B RID: 40043
				public static LocString EFFECT = "Manually pumps " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " into bottles for transport.\n\nDuplicants can only carry liquids that are bottled.";
			}

			// Token: 0x02002329 RID: 9001
			public class LIQUIDVALVE
			{
				// Token: 0x04009C6C RID: 40044
				public static LocString NAME = UI.FormatAsLink("Liquid Valve", "LIQUIDVALVE");

				// Token: 0x04009C6D RID: 40045
				public static LocString DESC = "Valves control the amount of liquid that moves through pipes, preventing waste.";

				// Token: 0x04009C6E RID: 40046
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Controls the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" volume permitted through ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					"."
				});
			}

			// Token: 0x0200232A RID: 9002
			public class LIQUIDLOGICVALVE
			{
				// Token: 0x04009C6F RID: 40047
				public static LocString NAME = UI.FormatAsLink("Liquid Shutoff", "LIQUIDLOGICVALVE");

				// Token: 0x04009C70 RID: 40048
				public static LocString DESC = "Automated piping saves power and time by removing the need for Duplicant input.";

				// Token: 0x04009C71 RID: 40049
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" flow on or off."
				});

				// Token: 0x04009C72 RID: 40050
				public static LocString LOGIC_PORT = "Open/Close";

				// Token: 0x04009C73 RID: 40051
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow Liquid flow";

				// Token: 0x04009C74 RID: 40052
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent Liquid flow";
			}

			// Token: 0x0200232B RID: 9003
			public class LIQUIDLIMITVALVE
			{
				// Token: 0x04009C75 RID: 40053
				public static LocString NAME = UI.FormatAsLink("Liquid Meter Valve", "LIQUIDLIMITVALVE");

				// Token: 0x04009C76 RID: 40054
				public static LocString DESC = "Meter Valves let an exact amount of liquid pass through before shutting off.";

				// Token: 0x04009C77 RID: 40055
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" flow off when the specified amount has passed through it."
				});

				// Token: 0x04009C78 RID: 40056
				public static LocString LOGIC_PORT_OUTPUT = "Limit Reached";

				// Token: 0x04009C79 RID: 40057
				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if limit has been reached";

				// Token: 0x04009C7A RID: 40058
				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x04009C7B RID: 40059
				public static LocString LOGIC_PORT_RESET = "Reset Meter";

				// Token: 0x04009C7C RID: 40060
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the amount";

				// Token: 0x04009C7D RID: 40061
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x0200232C RID: 9004
			public class LIQUIDVENT
			{
				// Token: 0x04009C7E RID: 40062
				public static LocString NAME = UI.FormatAsLink("Liquid Vent", "LIQUIDVENT");

				// Token: 0x04009C7F RID: 40063
				public static LocString DESC = "Vents are an exit point for liquids from plumbing systems.";

				// Token: 0x04009C80 RID: 40064
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Releases ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" from ",
					UI.FormatAsLink("Liquid Pipes", "LIQUIDPIPING"),
					"."
				});
			}

			// Token: 0x0200232D RID: 9005
			public class MANUALGENERATOR
			{
				// Token: 0x04009C81 RID: 40065
				public static LocString NAME = UI.FormatAsLink("Manual Generator", "MANUALGENERATOR");

				// Token: 0x04009C82 RID: 40066
				public static LocString DESC = "Watching Duplicants run on it is adorable... the electrical power is just an added bonus.";

				// Token: 0x04009C83 RID: 40067
				public static LocString EFFECT = "Converts manual labor into electrical " + UI.FormatAsLink("Power", "POWER") + ".";
			}

			// Token: 0x0200232E RID: 9006
			public class MANUALPRESSUREDOOR
			{
				// Token: 0x04009C84 RID: 40068
				public static LocString NAME = UI.FormatAsLink("Manual Airlock", "MANUALPRESSUREDOOR");

				// Token: 0x04009C85 RID: 40069
				public static LocString DESC = "Airlocks can quarter off dangerous areas and prevent gases from seeping into the colony.";

				// Token: 0x04009C86 RID: 40070
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Blocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow, maintaining pressure between areas.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});
			}

			// Token: 0x0200232F RID: 9007
			public class MESHTILE
			{
				// Token: 0x04009C87 RID: 40071
				public static LocString NAME = UI.FormatAsLink("Mesh Tile", "MESHTILE");

				// Token: 0x04009C88 RID: 40072
				public static LocString DESC = "Mesh tile can be used to make Duplicant pathways in areas where liquid flows.";

				// Token: 0x04009C89 RID: 40073
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nDoes not obstruct ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow."
				});
			}

			// Token: 0x02002330 RID: 9008
			public class PLASTICTILE
			{
				// Token: 0x04009C8A RID: 40074
				public static LocString NAME = UI.FormatAsLink("Plastic Tile", "PLASTICTILE");

				// Token: 0x04009C8B RID: 40075
				public static LocString DESC = "Plastic tile is mildly antiseptic and can help limit the spread of germs in a colony.";

				// Token: 0x04009C8C RID: 40076
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nSignificantly increases Duplicant runspeed.";
			}

			// Token: 0x02002331 RID: 9009
			public class GLASSTILE
			{
				// Token: 0x04009C8D RID: 40077
				public static LocString NAME = UI.FormatAsLink("Window Tile", "GLASSTILE");

				// Token: 0x04009C8E RID: 40078
				public static LocString DESC = "Window tiles provide a barrier against liquid and gas and are completely transparent.";

				// Token: 0x04009C8F RID: 40079
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nAllows ",
					UI.FormatAsLink("Light", "LIGHT"),
					" and ",
					UI.FormatAsLink("Decor", "DECOR"),
					" to pass through."
				});
			}

			// Token: 0x02002332 RID: 9010
			public class METALTILE
			{
				// Token: 0x04009C90 RID: 40080
				public static LocString NAME = UI.FormatAsLink("Metal Tile", "METALTILE");

				// Token: 0x04009C91 RID: 40081
				public static LocString DESC = "Heat travels much more quickly through metal tile than other types of flooring.";

				// Token: 0x04009C92 RID: 40082
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nSignificantly increases Duplicant runspeed.";
			}

			// Token: 0x02002333 RID: 9011
			public class BUNKERTILE
			{
				// Token: 0x04009C93 RID: 40083
				public static LocString NAME = UI.FormatAsLink("Bunker Tile", "BUNKERTILE");

				// Token: 0x04009C94 RID: 40084
				public static LocString DESC = "Bunker tile can build strong shelters in otherwise dangerous environments.";

				// Token: 0x04009C95 RID: 40085
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nCan withstand extreme pressures and impacts.";
			}

			// Token: 0x02002334 RID: 9012
			public class CARPETTILE
			{
				// Token: 0x04009C96 RID: 40086
				public static LocString NAME = UI.FormatAsLink("Carpeted Tile", "CARPETTILE");

				// Token: 0x04009C97 RID: 40087
				public static LocString DESC = "Soft on little Duplicant toesies.";

				// Token: 0x04009C98 RID: 40088
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			// Token: 0x02002335 RID: 9013
			public class MOULDINGTILE
			{
				// Token: 0x04009C99 RID: 40089
				public static LocString NAME = UI.FormatAsLink("Trimming Tile", "MOUDLINGTILE");

				// Token: 0x04009C9A RID: 40090
				public static LocString DESC = "Trimming is used as purely decorative lining for walls and structures.";

				// Token: 0x04009C9B RID: 40091
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to build the walls and floors of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			// Token: 0x02002336 RID: 9014
			public class MONUMENTBOTTOM
			{
				// Token: 0x04009C9C RID: 40092
				public static LocString NAME = UI.FormatAsLink("Monument Base", "MONUMENTBOTTOM");

				// Token: 0x04009C9D RID: 40093
				public static LocString DESC = "The base of a monument must be constructed first.";

				// Token: 0x04009C9E RID: 40094
				public static LocString EFFECT = "Builds the bottom section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";
			}

			// Token: 0x02002337 RID: 9015
			public class MONUMENTMIDDLE
			{
				// Token: 0x04009C9F RID: 40095
				public static LocString NAME = UI.FormatAsLink("Monument Midsection", "MONUMENTMIDDLE");

				// Token: 0x04009CA0 RID: 40096
				public static LocString DESC = "Customized sections of a Great Monument can be mixed and matched.";

				// Token: 0x04009CA1 RID: 40097
				public static LocString EFFECT = "Builds the middle section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";
			}

			// Token: 0x02002338 RID: 9016
			public class MONUMENTTOP
			{
				// Token: 0x04009CA2 RID: 40098
				public static LocString NAME = UI.FormatAsLink("Monument Top", "MONUMENTTOP");

				// Token: 0x04009CA3 RID: 40099
				public static LocString DESC = "Building a Great Monument will declare to the universe that this hunk of rock is your own.";

				// Token: 0x04009CA4 RID: 40100
				public static LocString EFFECT = "Builds the top section of a Great Monument.\n\nCan be customized.\n\nA Great Monument must be built to achieve the Colonize Imperative.";
			}

			// Token: 0x02002339 RID: 9017
			public class MICROBEMUSHER
			{
				// Token: 0x04009CA5 RID: 40101
				public static LocString NAME = UI.FormatAsLink("Microbe Musher", "MICROBEMUSHER");

				// Token: 0x04009CA6 RID: 40102
				public static LocString DESC = "Musher recipes will keep Duplicants fed, but may impact health and morale over time.";

				// Token: 0x04009CA7 RID: 40103
				public static LocString EFFECT = "Produces low quality " + UI.FormatAsLink("Food", "FOOD") + " using common ingredients.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x0200233A RID: 9018
			public class MINERALDEOXIDIZER
			{
				// Token: 0x04009CA8 RID: 40104
				public static LocString NAME = UI.FormatAsLink("Oxygen Diffuser", "MINERALDEOXIDIZER");

				// Token: 0x04009CA9 RID: 40105
				public static LocString DESC = "Oxygen diffusers are inefficient, but output enough oxygen to keep a colony breathing.";

				// Token: 0x04009CAA RID: 40106
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts large amounts of ",
					UI.FormatAsLink("Algae", "ALGAE"),
					" into ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			// Token: 0x0200233B RID: 9019
			public class SUBLIMATIONSTATION
			{
				// Token: 0x04009CAB RID: 40107
				public static LocString NAME = UI.FormatAsLink("Sublimation Station", "SUBLIMATIONSTATION");

				// Token: 0x04009CAC RID: 40108
				public static LocString DESC = "Sublimation is the sublime process by which solids convert directly into gas.";

				// Token: 0x04009CAD RID: 40109
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Speeds up the conversion of ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					" into ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					".\n\nBecomes idle when the area reaches maximum pressure capacity."
				});
			}

			// Token: 0x0200233C RID: 9020
			public class ORESCRUBBER
			{
				// Token: 0x04009CAE RID: 40110
				public static LocString NAME = UI.FormatAsLink("Ore Scrubber", "ORESCRUBBER");

				// Token: 0x04009CAF RID: 40111
				public static LocString DESC = "Scrubbers sanitize freshly mined materials before they're brought into the colony.";

				// Token: 0x04009CB0 RID: 40112
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Kills a significant amount of ",
					UI.FormatAsLink("Germs", "DISEASE"),
					" present on ",
					UI.FormatAsLink("Raw Ore", "RAWMINERAL"),
					"."
				});
			}

			// Token: 0x0200233D RID: 9021
			public class OUTHOUSE
			{
				// Token: 0x04009CB1 RID: 40113
				public static LocString NAME = UI.FormatAsLink("Outhouse", "OUTHOUSE");

				// Token: 0x04009CB2 RID: 40114
				public static LocString DESC = "The colony that eats together, excretes together.";

				// Token: 0x04009CB3 RID: 40115
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Gives Duplicants a place to relieve themselves.\n\nRequires no ",
					UI.FormatAsLink("Piping", "LIQUIDPIPING"),
					".\n\nMust be periodically emptied of ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					"."
				});
			}

			// Token: 0x0200233E RID: 9022
			public class APOTHECARY
			{
				// Token: 0x04009CB4 RID: 40116
				public static LocString NAME = UI.FormatAsLink("Apothecary", "APOTHECARY");

				// Token: 0x04009CB5 RID: 40117
				public static LocString DESC = "Some medications help prevent diseases, while others aim to alleviate existing illness.";

				// Token: 0x04009CB6 RID: 40118
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Medicine", "MEDICINE"),
					" to cure most basic ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nDuplicants must possess the Medicine Compounding ",
					UI.FormatAsLink("Skill", "ROLES"),
					" to fabricate medicines.\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			// Token: 0x0200233F RID: 9023
			public class ADVANCEDAPOTHECARY
			{
				// Token: 0x04009CB7 RID: 40119
				public static LocString NAME = UI.FormatAsLink("Nuclear Apothecary", "ADVANCEDAPOTHECARY");

				// Token: 0x04009CB8 RID: 40120
				public static LocString DESC = "Some medications help prevent diseases, while others aim to alleviate existing illness.";

				// Token: 0x04009CB9 RID: 40121
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Medicine", "MEDICINE"),
					" to cure most basic ",
					UI.FormatAsLink("Diseases", "DISEASE"),
					".\n\nDuplicants must possess the Medicine Compounding ",
					UI.FormatAsLink("Skill", "ROLES"),
					" to fabricate medicines.\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			// Token: 0x02002340 RID: 9024
			public class PLANTERBOX
			{
				// Token: 0x04009CBA RID: 40122
				public static LocString NAME = UI.FormatAsLink("Planter Box", "PLANTERBOX");

				// Token: 0x04009CBB RID: 40123
				public static LocString DESC = "Domestically grown seeds mature more quickly than wild plants.";

				// Token: 0x04009CBC RID: 40124
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Grows one ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" from a ",
					UI.FormatAsLink("Seed", "PLANTS"),
					"."
				});

				// Token: 0x02002F7E RID: 12158
				public class FACADES
				{
					// Token: 0x02003293 RID: 12947
					public class DEFAULT_PLANTERBOX
					{
						// Token: 0x0400C988 RID: 51592
						public static LocString NAME = UI.FormatAsLink("Planter Box", "PLANTERBOX");

						// Token: 0x0400C989 RID: 51593
						public static LocString DESC = "Domestically grown seeds mature more quickly than wild plants.";
					}

					// Token: 0x02003294 RID: 12948
					public class MEALWOOD
					{
						// Token: 0x0400C98A RID: 51594
						public static LocString NAME = UI.FormatAsLink("Mealy Teal Planter Box", "PLANTERBOX");

						// Token: 0x0400C98B RID: 51595
						public static LocString DESC = "Inspired by genetically modified nature.";
					}

					// Token: 0x02003295 RID: 12949
					public class BRISTLEBLOSSOM
					{
						// Token: 0x0400C98C RID: 51596
						public static LocString NAME = UI.FormatAsLink("Bristly Green Planter Box", "PLANTERBOX");

						// Token: 0x0400C98D RID: 51597
						public static LocString DESC = "The interior is lined with tiny barbs.";
					}

					// Token: 0x02003296 RID: 12950
					public class WHEEZEWORT
					{
						// Token: 0x0400C98E RID: 51598
						public static LocString NAME = UI.FormatAsLink("Wheezy Whorl Planter Box", "PLANTERBOX");

						// Token: 0x0400C98F RID: 51599
						public static LocString DESC = "For the dreamy agriculturalist.";
					}

					// Token: 0x02003297 RID: 12951
					public class SLEETWHEAT
					{
						// Token: 0x0400C990 RID: 51600
						public static LocString NAME = UI.FormatAsLink("Sleet Blue Planter Box", "PLANTERBOX");

						// Token: 0x0400C991 RID: 51601
						public static LocString DESC = "The thick paint drips are invisible from a distance.";
					}

					// Token: 0x02003298 RID: 12952
					public class SALMON_PINK
					{
						// Token: 0x0400C992 RID: 51602
						public static LocString NAME = UI.FormatAsLink("Flashy Planter Box", "PLANTERBOX");

						// Token: 0x0400C993 RID: 51603
						public static LocString DESC = "It's not exactly a subtle color.";
					}
				}
			}

			// Token: 0x02002341 RID: 9025
			public class PRESSUREDOOR
			{
				// Token: 0x04009CBD RID: 40125
				public static LocString NAME = UI.FormatAsLink("Mechanized Airlock", "PRESSUREDOOR");

				// Token: 0x04009CBE RID: 40126
				public static LocString DESC = "Mechanized airlocks open and close more quickly than other types of door.";

				// Token: 0x04009CBF RID: 40127
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Blocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow, maintaining pressure between areas.\n\nFunctions as a ",
					UI.FormatAsLink("Manual Airlock", "MANUALPRESSUREDOOR"),
					" when no ",
					UI.FormatAsLink("Power", "POWER"),
					" is available.\n\nWild ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" cannot pass through doors."
				});
			}

			// Token: 0x02002342 RID: 9026
			public class BUNKERDOOR
			{
				// Token: 0x04009CC0 RID: 40128
				public static LocString NAME = UI.FormatAsLink("Bunker Door", "BUNKERDOOR");

				// Token: 0x04009CC1 RID: 40129
				public static LocString DESC = "A massive, slow-moving door which is nearly indestructible.";

				// Token: 0x04009CC2 RID: 40130
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Blocks ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" flow, maintaining pressure between areas.\n\nCan withstand extremely high pressures and impacts."
				});
			}

			// Token: 0x02002343 RID: 9027
			public class RATIONBOX
			{
				// Token: 0x04009CC3 RID: 40131
				public static LocString NAME = UI.FormatAsLink("Ration Box", "RATIONBOX");

				// Token: 0x04009CC4 RID: 40132
				public static LocString DESC = "Ration boxes keep food safe from hungry critters, but don't slow food spoilage.";

				// Token: 0x04009CC5 RID: 40133
				public static LocString EFFECT = "Stores a small amount of " + UI.FormatAsLink("Food", "FOOD") + ".\n\nFood must be delivered to boxes by Duplicants.";
			}

			// Token: 0x02002344 RID: 9028
			public class PARKSIGN
			{
				// Token: 0x04009CC6 RID: 40134
				public static LocString NAME = UI.FormatAsLink("Park Sign", "PARKSIGN");

				// Token: 0x04009CC7 RID: 40135
				public static LocString DESC = "Passing through parks will increase Duplicant Morale.";

				// Token: 0x04009CC8 RID: 40136
				public static LocString EFFECT = "Classifies an area as a Park or Nature Reserve.";
			}

			// Token: 0x02002345 RID: 9029
			public class RADIATIONLIGHT
			{
				// Token: 0x04009CC9 RID: 40137
				public static LocString NAME = UI.FormatAsLink("Radiation Lamp", "RADIATIONLIGHT");

				// Token: 0x04009CCA RID: 40138
				public static LocString DESC = "Duplicants can become sick if exposed to radiation without protection.";

				// Token: 0x04009CCB RID: 40139
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Emits ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" when ",
					UI.FormatAsLink("Powered", "POWER"),
					" that can be collected by a ",
					UI.FormatAsLink("Radbolt Generator", "HIGHENERGYPARTICLESPAWNER"),
					"."
				});
			}

			// Token: 0x02002346 RID: 9030
			public class REFRIGERATOR
			{
				// Token: 0x04009CCC RID: 40140
				public static LocString NAME = UI.FormatAsLink("Refrigerator", "REFRIGERATOR");

				// Token: 0x04009CCD RID: 40141
				public static LocString DESC = "Food spoilage can be slowed by ambient conditions as well as by refrigerators.";

				// Token: 0x04009CCE RID: 40142
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Food", "FOOD"),
					" at an ideal ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" to prevent spoilage."
				});

				// Token: 0x04009CCF RID: 40143
				public static LocString LOGIC_PORT = "Full/Not Full";

				// Token: 0x04009CD0 RID: 40144
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when full";

				// Token: 0x04009CD1 RID: 40145
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002347 RID: 9031
			public class ROLESTATION
			{
				// Token: 0x04009CD2 RID: 40146
				public static LocString NAME = UI.FormatAsLink("Skills Board", "ROLESTATION");

				// Token: 0x04009CD3 RID: 40147
				public static LocString DESC = "A skills board can teach special skills to Duplicants they can't learn on their own.";

				// Token: 0x04009CD4 RID: 40148
				public static LocString EFFECT = "Allows Duplicants to spend Skill Points to learn new " + UI.FormatAsLink("Skills", "JOBS") + ".";
			}

			// Token: 0x02002348 RID: 9032
			public class RESETSKILLSSTATION
			{
				// Token: 0x04009CD5 RID: 40149
				public static LocString NAME = UI.FormatAsLink("Skill Scrubber", "RESETSKILLSSTATION");

				// Token: 0x04009CD6 RID: 40150
				public static LocString DESC = "Erase skills from a Duplicant's mind, returning them to their default abilities.";

				// Token: 0x04009CD7 RID: 40151
				public static LocString EFFECT = "Refunds a Duplicant's Skill Points for reassignment.\n\nDuplicants will lose all assigned skills in the process.";
			}

			// Token: 0x02002349 RID: 9033
			public class RESEARCHCENTER
			{
				// Token: 0x04009CD8 RID: 40152
				public static LocString NAME = UI.FormatAsLink("Research Station", "RESEARCHCENTER");

				// Token: 0x04009CD9 RID: 40153
				public static LocString DESC = "Research stations are necessary for unlocking all research tiers.";

				// Token: 0x04009CDA RID: 40154
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Novice Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Dirt", "DIRT"),
					"."
				});
			}

			// Token: 0x0200234A RID: 9034
			public class ADVANCEDRESEARCHCENTER
			{
				// Token: 0x04009CDB RID: 40155
				public static LocString NAME = UI.FormatAsLink("Super Computer", "ADVANCEDRESEARCHCENTER");

				// Token: 0x04009CDC RID: 40156
				public static LocString DESC = "Super computers unlock higher technology tiers than research stations alone.";

				// Token: 0x04009CDD RID: 40157
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Advanced Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Water", "WATER"),
					".\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Advanced Research", "RESEARCHING1"),
					" skill."
				});
			}

			// Token: 0x0200234B RID: 9035
			public class NUCLEARRESEARCHCENTER
			{
				// Token: 0x04009CDE RID: 40158
				public static LocString NAME = UI.FormatAsLink("Materials Study Terminal", "NUCLEARRESEARCHCENTER");

				// Token: 0x04009CDF RID: 40159
				public static LocString DESC = "Comes with a few ions thrown in, free of charge.";

				// Token: 0x04009CE0 RID: 40160
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Materials Science Research", "RESEARCHDLC1"),
					" to unlock new technologies.\n\nConsumes Radbolts.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Applied Sciences Research", "ATOMICRESEARCH"),
					" skill."
				});
			}

			// Token: 0x0200234C RID: 9036
			public class ORBITALRESEARCHCENTER
			{
				// Token: 0x04009CE1 RID: 40161
				public static LocString NAME = UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER");

				// Token: 0x04009CE2 RID: 40162
				public static LocString DESC = "Orbital Data Collection Labs record data while orbiting a Planetoid and write it to a " + UI.FormatAsLink("Data Bank", "ORBITALRESEARCHDATABANK") + ". ";

				// Token: 0x04009CE3 RID: 40163
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Creates ",
					UI.FormatAsLink("Data Banks", "ORBITALRESEARCHDATABANK"),
					" that can be consumed at a ",
					UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
					" and ",
					UI.FormatAsLink("Power", "POWER"),
					"."
				});
			}

			// Token: 0x0200234D RID: 9037
			public class COSMICRESEARCHCENTER
			{
				// Token: 0x04009CE4 RID: 40164
				public static LocString NAME = UI.FormatAsLink("Virtual Planetarium", "COSMICRESEARCHCENTER");

				// Token: 0x04009CE5 RID: 40165
				public static LocString DESC = "Planetariums allow the simulated exploration of locations discovered with a telescope.";

				// Token: 0x04009CE6 RID: 40166
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Interstellar Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes data from ",
					UI.FormatAsLink("Telescopes", "TELESCOPE"),
					" and ",
					UI.FormatAsLink("Research Modules", "RESEARCHMODULE"),
					".\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Astronomy", "ASTRONOMY"),
					" skill."
				});
			}

			// Token: 0x0200234E RID: 9038
			public class DLC1COSMICRESEARCHCENTER
			{
				// Token: 0x04009CE7 RID: 40167
				public static LocString NAME = UI.FormatAsLink("Virtual Planetarium", "DLC1COSMICRESEARCHCENTER");

				// Token: 0x04009CE8 RID: 40168
				public static LocString DESC = "Planetariums allow the simulated exploration of locations recorded in " + UI.FormatAsLink("Data Banks", "ORBITALRESEARCHDATABANK") + ".";

				// Token: 0x04009CE9 RID: 40169
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Conducts ",
					UI.FormatAsLink("Data Analysis Research", "RESEARCH"),
					" to unlock new technologies.\n\nConsumes ",
					UI.FormatAsLink("Data Banks", "ORBITALRESEARCHDATABANK"),
					" generated by exploration."
				});
			}

			// Token: 0x0200234F RID: 9039
			public class TELESCOPE
			{
				// Token: 0x04009CEA RID: 40170
				public static LocString NAME = UI.FormatAsLink("Telescope", "TELESCOPE");

				// Token: 0x04009CEB RID: 40171
				public static LocString DESC = "Telescopes are necessary for learning starmaps and conducting rocket missions.";

				// Token: 0x04009CEC RID: 40172
				public static LocString EFFECT = "Maps Starmap destinations.\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Field Research", "RESEARCHING2") + " skill.\n\nBuilding must be exposed to space to function.";

				// Token: 0x04009CED RID: 40173
				public static LocString REQUIREMENT_TOOLTIP = "A steady {0} supply is required to sustain working Duplicants.";
			}

			// Token: 0x02002350 RID: 9040
			public class CLUSTERTELESCOPE
			{
				// Token: 0x04009CEE RID: 40174
				public static LocString NAME = UI.FormatAsLink("Telescope", "CLUSTERTELESCOPE");

				// Token: 0x04009CEF RID: 40175
				public static LocString DESC = "Telescopes are necessary for studying space, allowing rocket travel to other worlds.";

				// Token: 0x04009CF0 RID: 40176
				public static LocString EFFECT = "Reveals visitable Planetoids in space.\n\nAssigned Duplicants must possess the " + UI.FormatAsLink("Astronomy", "ASTRONOMY") + " skill.\n\nBuilding must be exposed to space to function.";

				// Token: 0x04009CF1 RID: 40177
				public static LocString REQUIREMENT_TOOLTIP = "A steady {0} supply is required to sustain working Duplicants.";
			}

			// Token: 0x02002351 RID: 9041
			public class CLUSTERTELESCOPEENCLOSED
			{
				// Token: 0x04009CF2 RID: 40178
				public static LocString NAME = UI.FormatAsLink("Enclosed Telescope", "CLUSTERTELESCOPEENCLOSED");

				// Token: 0x04009CF3 RID: 40179
				public static LocString DESC = "Telescopes are necessary for studying space, allowing rocket travel to other worlds.";

				// Token: 0x04009CF4 RID: 40180
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reveals visitable Planetoids in space... in comfort!\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Astronomy", "ASTRONOMY"),
					" skill.\n\nExcellent sunburn protection (100%), partial ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" protection (",
					GameUtil.GetFormattedPercent(FIXEDTRAITS.COSMICRADIATION.TELESCOPE_RADIATION_SHIELDING * 100f, GameUtil.TimeSlice.None),
					") .\n\nBuilding must be exposed to space to function."
				});

				// Token: 0x04009CF5 RID: 40181
				public static LocString REQUIREMENT_TOOLTIP = "A steady {0} supply is required to sustain working Duplicants.";
			}

			// Token: 0x02002352 RID: 9042
			public class MISSIONCONTROL
			{
				// Token: 0x04009CF6 RID: 40182
				public static LocString NAME = UI.FormatAsLink("Mission Control Station", "MISSIONCONTROL");

				// Token: 0x04009CF7 RID: 40183
				public static LocString DESC = "Like a backseat driver who actually does know better.";

				// Token: 0x04009CF8 RID: 40184
				public static LocString EFFECT = "Provides guidance data to rocket pilots, to improve rocket speed.\n\nMust be operated by a Duplicant with the " + UI.FormatAsLink("Astronomy", "ASTRONOMY") + " skill.\n\nRequires a clear line of sight to space in order to function.";
			}

			// Token: 0x02002353 RID: 9043
			public class MISSIONCONTROLCLUSTER
			{
				// Token: 0x04009CF9 RID: 40185
				public static LocString NAME = UI.FormatAsLink("Mission Control Station", "MISSIONCONTROL");

				// Token: 0x04009CFA RID: 40186
				public static LocString DESC = "Like a backseat driver who actually does know better.";

				// Token: 0x04009CFB RID: 40187
				public static LocString EFFECT = "Provides guidance data to rocket pilots within range, to improve rocket speed.\n\nMust be operated by a Duplicant with the " + UI.FormatAsLink("Astronomy", "ASTRONOMY") + " skill.\n\nRequires a clear line of sight to space in order to function.";
			}

			// Token: 0x02002354 RID: 9044
			public class SCULPTURE
			{
				// Token: 0x04009CFC RID: 40188
				public static LocString NAME = UI.FormatAsLink("Large Sculpting Block", "SCULPTURE");

				// Token: 0x04009CFD RID: 40189
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x04009CFE RID: 40190
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Moderately increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x04009CFF RID: 40191
				public static LocString POORQUALITYNAME = "\"Abstract\" Sculpture";

				// Token: 0x04009D00 RID: 40192
				public static LocString AVERAGEQUALITYNAME = "Mediocre Sculpture";

				// Token: 0x04009D01 RID: 40193
				public static LocString EXCELLENTQUALITYNAME = "Genius Sculpture";

				// Token: 0x02002F7F RID: 12159
				public class FACADES
				{
					// Token: 0x02003299 RID: 12953
					public class SCULPTURE_GOOD_1
					{
						// Token: 0x0400C994 RID: 51604
						public static LocString NAME = UI.FormatAsLink("O Cupid, My Cupid", "SCULPTURE_GOOD_1");

						// Token: 0x0400C995 RID: 51605
						public static LocString DESC = "Ode to the bow and arrow, love's equivalent to a mining gun...but for hearts.";
					}

					// Token: 0x0200329A RID: 12954
					public class SCULPTURE_CRAP_1
					{
						// Token: 0x0400C996 RID: 51606
						public static LocString NAME = UI.FormatAsLink("Inexplicable", "SCULPTURE_CRAP_1");

						// Token: 0x0400C997 RID: 51607
						public static LocString DESC = "A valiant attempt at art.";
					}

					// Token: 0x0200329B RID: 12955
					public class SCULPTURE_AMAZING_2
					{
						// Token: 0x0400C998 RID: 51608
						public static LocString NAME = UI.FormatAsLink("Plate Chucker", "SCULPTURE_AMAZING_2");

						// Token: 0x0400C999 RID: 51609
						public static LocString DESC = "A masterful portrayal of an athlete who's been banned from the communal kitchen.";
					}

					// Token: 0x0200329C RID: 12956
					public class SCULPTURE_AMAZING_3
					{
						// Token: 0x0400C99A RID: 51610
						public static LocString NAME = UI.FormatAsLink("Before Battle", "SCULPTURE_AMAZING_3");

						// Token: 0x0400C99B RID: 51611
						public static LocString DESC = "A masterful portrayal of a slingshot-wielding hero.";
					}

					// Token: 0x0200329D RID: 12957
					public class SCULPTURE_AMAZING_4
					{
						// Token: 0x0400C99C RID: 51612
						public static LocString NAME = UI.FormatAsLink("Grandiose Grub-Grub", "SCULPTURE_AMAZING_4");

						// Token: 0x0400C99D RID: 51613
						public static LocString DESC = "A masterful portrayal of a gentle, plant-tending critter.";
					}

					// Token: 0x0200329E RID: 12958
					public class SCULPTURE_AMAZING_1
					{
						// Token: 0x0400C99E RID: 51614
						public static LocString NAME = UI.FormatAsLink("The Hypothesizer", "SCULPTURE_AMAZING_1");

						// Token: 0x0400C99F RID: 51615
						public static LocString DESC = "A masterful portrayal of a scientist lost in thought.";
					}

					// Token: 0x0200329F RID: 12959
					public class SCULPTURE_AMAZING_5
					{
						// Token: 0x0400C9A0 RID: 51616
						public static LocString NAME = UI.FormatAsLink("Vertical Cosmos", "SCULPTURE_AMAZING_5");

						// Token: 0x0400C9A1 RID: 51617
						public static LocString DESC = "It contains multitudes.";
					}

					// Token: 0x020032A0 RID: 12960
					public class SCULPTURE_AMAZING_6
					{
						// Token: 0x0400C9A2 RID: 51618
						public static LocString NAME = UI.FormatAsLink("Into the Voids", "SCULPTURE_AMAZING_6");

						// Token: 0x0400C9A3 RID: 51619
						public static LocString DESC = "No amount of material success will ever fill the void within.";
					}
				}
			}

			// Token: 0x02002355 RID: 9045
			public class ICESCULPTURE
			{
				// Token: 0x04009D02 RID: 40194
				public static LocString NAME = UI.FormatAsLink("Ice Block", "ICESCULPTURE");

				// Token: 0x04009D03 RID: 40195
				public static LocString DESC = "Prone to melting.";

				// Token: 0x04009D04 RID: 40196
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x04009D05 RID: 40197
				public static LocString POORQUALITYNAME = "\"Abstract\" Ice Sculpture";

				// Token: 0x04009D06 RID: 40198
				public static LocString AVERAGEQUALITYNAME = "Mediocre Ice Sculpture";

				// Token: 0x04009D07 RID: 40199
				public static LocString EXCELLENTQUALITYNAME = "Genius Ice Sculpture";

				// Token: 0x02002F80 RID: 12160
				public class FACADES
				{
					// Token: 0x020032A1 RID: 12961
					public class ICESCULPTURE_CRAP
					{
						// Token: 0x0400C9A4 RID: 51620
						public static LocString NAME = UI.FormatAsLink("Cubi I", "ICESCULPTURE_CRAP");

						// Token: 0x0400C9A5 RID: 51621
						public static LocString DESC = "It's structurally unsound, but otherwise not entirely terrible.";
					}

					// Token: 0x020032A2 RID: 12962
					public class ICESCULPTURE_AMAZING_1
					{
						// Token: 0x0400C9A6 RID: 51622
						public static LocString NAME = UI.FormatAsLink("Exquisite Chompers", "ICESCULPTURE_AMAZING_1");

						// Token: 0x0400C9A7 RID: 51623
						public static LocString DESC = "These incisors are the stuff of dental legend.";
					}

					// Token: 0x020032A3 RID: 12963
					public class ICESCULPTURE_AMAZING_2
					{
						// Token: 0x0400C9A8 RID: 51624
						public static LocString NAME = UI.FormatAsLink("Frosty Crustacean", "ICESCULPTURE_AMAZING_2");

						// Token: 0x0400C9A9 RID: 51625
						public static LocString DESC = "A charming depiction of the mighty Pokeshell in mid-rampage.";
					}

					// Token: 0x020032A4 RID: 12964
					public class ICESCULPTURE_AMAZING_3
					{
						// Token: 0x0400C9AA RID: 51626
						public static LocString NAME = UI.FormatAsLink("The Chase", "ICESCULPTURE_AMAZING_3");

						// Token: 0x0400C9AB RID: 51627
						public static LocString DESC = "Some aquarists posit that Pacus are the original creators of the game now known as \"Tag.\"";
					}
				}
			}

			// Token: 0x02002356 RID: 9046
			public class MARBLESCULPTURE
			{
				// Token: 0x04009D08 RID: 40200
				public static LocString NAME = UI.FormatAsLink("Marble Block", "MARBLESCULPTURE");

				// Token: 0x04009D09 RID: 40201
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x04009D0A RID: 40202
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x04009D0B RID: 40203
				public static LocString POORQUALITYNAME = "\"Abstract\" Marble Sculpture";

				// Token: 0x04009D0C RID: 40204
				public static LocString AVERAGEQUALITYNAME = "Mediocre Marble Sculpture";

				// Token: 0x04009D0D RID: 40205
				public static LocString EXCELLENTQUALITYNAME = "Genius Marble Sculpture";

				// Token: 0x02002F81 RID: 12161
				public class FACADES
				{
					// Token: 0x020032A5 RID: 12965
					public class SCULPTURE_MARBLE_CRAP_1
					{
						// Token: 0x0400C9AC RID: 51628
						public static LocString NAME = UI.FormatAsLink("Lumpy Fungus", "SCULPTURE_MARBLE_CRAP_1");

						// Token: 0x0400C9AD RID: 51629
						public static LocString DESC = "The artist was a very fungi.";
					}

					// Token: 0x020032A6 RID: 12966
					public class SCULPTURE_MARBLE_GOOD_1
					{
						// Token: 0x0400C9AE RID: 51630
						public static LocString NAME = UI.FormatAsLink("Unicorn Bust", "SCULPTURE_MARBLE_GOOD_1");

						// Token: 0x0400C9AF RID: 51631
						public static LocString DESC = "It has real \"mane\" character energy.";
					}

					// Token: 0x020032A7 RID: 12967
					public class SCULPTURE_MARBLE_AMAZING_1
					{
						// Token: 0x0400C9B0 RID: 51632
						public static LocString NAME = UI.FormatAsLink("The Large-ish Mermaid", "SCULPTURE_MARBLE_AMAZING_1");

						// Token: 0x0400C9B1 RID: 51633
						public static LocString DESC = "She's not afraid to take up space.";
					}

					// Token: 0x020032A8 RID: 12968
					public class SCULPTURE_MARBLE_AMAZING_2
					{
						// Token: 0x0400C9B2 RID: 51634
						public static LocString NAME = UI.FormatAsLink("Grouchy Beast", "SCULPTURE_MARBLE_AMAZING_2");

						// Token: 0x0400C9B3 RID: 51635
						public static LocString DESC = "The artist took great pleasure in conveying their displeasure.";
					}

					// Token: 0x020032A9 RID: 12969
					public class SCULPTURE_MARBLE_AMAZING_3
					{
						// Token: 0x0400C9B4 RID: 51636
						public static LocString NAME = UI.FormatAsLink("The Guardian", "SCULPTURE_MARBLE_AMAZING_3");

						// Token: 0x0400C9B5 RID: 51637
						public static LocString DESC = "Will not play fetch.";
					}

					// Token: 0x020032AA RID: 12970
					public class SCULPTURE_MARBLE_AMAZING_4
					{
						// Token: 0x0400C9B6 RID: 51638
						public static LocString NAME = UI.FormatAsLink("Truly A-Moo-Zing", "SCULPTURE_MARBLE_AMAZING_4");

						// Token: 0x0400C9B7 RID: 51639
						public static LocString DESC = "A masterful celebration of one of the universe's most mysterious - and flatulent - organisms.";
					}

					// Token: 0x020032AB RID: 12971
					public class SCULPTURE_MARBLE_AMAZING_5
					{
						// Token: 0x0400C9B8 RID: 51640
						public static LocString NAME = UI.FormatAsLink("Green Goddess", "SCULPTURE_MARBLE_AMAZING_5");

						// Token: 0x0400C9B9 RID: 51641
						public static LocString DESC = "A masterful celebration of the deep bond between a horticulturalist and her prize Bristle Blossom.";
					}
				}
			}

			// Token: 0x02002357 RID: 9047
			public class METALSCULPTURE
			{
				// Token: 0x04009D0E RID: 40206
				public static LocString NAME = UI.FormatAsLink("Metal Block", "METALSCULPTURE");

				// Token: 0x04009D0F RID: 40207
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x04009D10 RID: 40208
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Majorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x04009D11 RID: 40209
				public static LocString POORQUALITYNAME = "\"Abstract\" Metal Sculpture";

				// Token: 0x04009D12 RID: 40210
				public static LocString AVERAGEQUALITYNAME = "Mediocre Metal Sculpture";

				// Token: 0x04009D13 RID: 40211
				public static LocString EXCELLENTQUALITYNAME = "Genius Metal Sculpture";

				// Token: 0x02002F82 RID: 12162
				public class FACADES
				{
					// Token: 0x020032AC RID: 12972
					public class SCULPTURE_METAL_CRAP_1
					{
						// Token: 0x0400C9BA RID: 51642
						public static LocString NAME = UI.FormatAsLink("Unnatural Beauty", "SCULPTURE_METAL_CRAP_1");

						// Token: 0x0400C9BB RID: 51643
						public static LocString DESC = "Actually, it's a very good likeness.";
					}

					// Token: 0x020032AD RID: 12973
					public class SCULPTURE_METAL_GOOD_1
					{
						// Token: 0x0400C9BC RID: 51644
						public static LocString NAME = UI.FormatAsLink("Beautiful Biohazard", "SCULPTURE_METAL_GOOD_1");

						// Token: 0x0400C9BD RID: 51645
						public static LocString DESC = "The Morb's eye is mounted on a swivel that activates at random intervals.";
					}

					// Token: 0x020032AE RID: 12974
					public class SCULPTURE_METAL_AMAZING_1
					{
						// Token: 0x0400C9BE RID: 51646
						public static LocString NAME = UI.FormatAsLink("Insatiable Appetite", "SCULPTURE_METAL_AMAZING_1");

						// Token: 0x0400C9BF RID: 51647
						public static LocString DESC = "It's quite lovely, until someone stubs their toe on it in the dark.";
					}

					// Token: 0x020032AF RID: 12975
					public class SCULPTURE_METAL_AMAZING_2
					{
						// Token: 0x0400C9C0 RID: 51648
						public static LocString NAME = UI.FormatAsLink("Agape", "SCULPTURE_METAL_AMAZING_2");

						// Token: 0x0400C9C1 RID: 51649
						public static LocString DESC = "Not quite expressionist, but undeniably expressive.";
					}

					// Token: 0x020032B0 RID: 12976
					public class SCULPTURE_METAL_AMAZING_3
					{
						// Token: 0x0400C9C2 RID: 51650
						public static LocString NAME = UI.FormatAsLink("Friendly Flier", "SCULPTURE_METAL_AMAZING_3");

						// Token: 0x0400C9C3 RID: 51651
						public static LocString DESC = "It emits no light, but it sure does brighten up a room.";
					}

					// Token: 0x020032B1 RID: 12977
					public class SCULPTURE_METAL_AMAZING_4
					{
						// Token: 0x0400C9C4 RID: 51652
						public static LocString NAME = UI.FormatAsLink("Whatta Pip", "SCULPTURE_METAL_AMAZING_4");

						// Token: 0x0400C9C5 RID: 51653
						public static LocString DESC = "A masterful likeness of the mischievous critter that Duplicants love to love.";
					}

					// Token: 0x020032B2 RID: 12978
					public class SCULPTURE_METAL_AMAZING_5
					{
						// Token: 0x0400C9C6 RID: 51654
						public static LocString NAME = UI.FormatAsLink("Phrenologist's Dream", "SCULPTURE_METAL_AMAZING_5");

						// Token: 0x0400C9C7 RID: 51655
						public static LocString DESC = "What if the entire head is one big bump?";
					}
				}
			}

			// Token: 0x02002358 RID: 9048
			public class SMALLSCULPTURE
			{
				// Token: 0x04009D14 RID: 40212
				public static LocString NAME = UI.FormatAsLink("Sculpting Block", "SMALLSCULPTURE");

				// Token: 0x04009D15 RID: 40213
				public static LocString DESC = "Duplicants who have learned art skills can produce more decorative sculptures.";

				// Token: 0x04009D16 RID: 40214
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Minorly increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nMust be sculpted by a Duplicant."
				});

				// Token: 0x04009D17 RID: 40215
				public static LocString POORQUALITYNAME = "\"Abstract\" Sculpture";

				// Token: 0x04009D18 RID: 40216
				public static LocString AVERAGEQUALITYNAME = "Mediocre Sculpture";

				// Token: 0x04009D19 RID: 40217
				public static LocString EXCELLENTQUALITYNAME = "Genius Sculpture";

				// Token: 0x02002F83 RID: 12163
				public class FACADES
				{
					// Token: 0x020032B3 RID: 12979
					public class SCULPTURE_1x2_GOOD
					{
						// Token: 0x0400C9C8 RID: 51656
						public static LocString NAME = UI.FormatAsLink("Lunar Slice", "SCULPTURE_1x2_GOOD");

						// Token: 0x0400C9C9 RID: 51657
						public static LocString DESC = "It must be a moon, because there are no bananas in space.";
					}

					// Token: 0x020032B4 RID: 12980
					public class SCULPTURE_1x2_CRAP
					{
						// Token: 0x0400C9CA RID: 51658
						public static LocString NAME = UI.FormatAsLink("Unrequited", "SCULPTURE_1x2_CRAP");

						// Token: 0x0400C9CB RID: 51659
						public static LocString DESC = "It's a heavy heart.";
					}

					// Token: 0x020032B5 RID: 12981
					public class SCULPTURE_1x2_AMAZING_1
					{
						// Token: 0x0400C9CC RID: 51660
						public static LocString NAME = UI.FormatAsLink("Not a Funnel", "SCULPTURE_1x2_AMAZING_1");

						// Token: 0x0400C9CD RID: 51661
						public static LocString DESC = "<i>Ceci n'est pas un entonnoir.</i>";
					}

					// Token: 0x020032B6 RID: 12982
					public class SCULPTURE_1x2_AMAZING_2
					{
						// Token: 0x0400C9CE RID: 51662
						public static LocString NAME = UI.FormatAsLink("Equilibrium", "SCULPTURE_1x2_AMAZING_2");

						// Token: 0x0400C9CF RID: 51663
						public static LocString DESC = "Part of a well-balanced exhibit.";
					}

					// Token: 0x020032B7 RID: 12983
					public class SCULPTURE_1x2_AMAZING_3
					{
						// Token: 0x0400C9D0 RID: 51664
						public static LocString NAME = UI.FormatAsLink("Opaque Orb", "SCULPTURE_1x2_AMAZING_3");

						// Token: 0x0400C9D1 RID: 51665
						public static LocString DESC = "It lacks transparency.";
					}

					// Token: 0x020032B8 RID: 12984
					public class SCULPTURE_1x2_AMAZING_4
					{
						// Token: 0x0400C9D2 RID: 51666
						public static LocString NAME = UI.FormatAsLink("Employee of the Month", "SCULPTURE_1x2_AMAZING_4");

						// Token: 0x0400C9D3 RID: 51667
						public static LocString DESC = "A masterful celebration of the Sweepy's unbeatable work ethic and cheerful, can-clean attitude.";
					}

					// Token: 0x020032B9 RID: 12985
					public class SCULPTURE_1x2_AMAZING_5
					{
						// Token: 0x0400C9D4 RID: 51668
						public static LocString NAME = UI.FormatAsLink("Pointy Impossibility", "SCULPTURE_1x2_AMAZING_5");

						// Token: 0x0400C9D5 RID: 51669
						public static LocString DESC = "A three-dimensional rebellion against the rules of Euclidean space.";
					}

					// Token: 0x020032BA RID: 12986
					public class SCULPTURE_1x2_AMAZING_6
					{
						// Token: 0x0400C9D6 RID: 51670
						public static LocString NAME = UI.FormatAsLink("Fireball", "SCULPTURE_1x2_AMAZING_6");

						// Token: 0x0400C9D7 RID: 51671
						public static LocString DESC = "Tribute to the artist's friend, who once attempted to catch a meteor with their bare hands.";
					}
				}
			}

			// Token: 0x02002359 RID: 9049
			public class SHEARINGSTATION
			{
				// Token: 0x04009D1A RID: 40218
				public static LocString NAME = UI.FormatAsLink("Shearing Station", "SHEARINGSTATION");

				// Token: 0x04009D1B RID: 40219
				public static LocString DESC = "Those critters aren't gonna shear themselves.";

				// Token: 0x04009D1C RID: 40220
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Shearing stations allow ",
					UI.FormatAsLink("Dreckos", "DRECKO"),
					" and ",
					UI.FormatAsLink("Delecta Voles", "MOLEDELICACY"),
					" to be safely sheared for useful raw materials."
				});
			}

			// Token: 0x0200235A RID: 9050
			public class OXYGENMASKSTATION
			{
				// Token: 0x04009D1D RID: 40221
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask Station", "OXYGENMASKSTATION");

				// Token: 0x04009D1E RID: 40222
				public static LocString DESC = "Duplicants can't pass by a station if it lacks enough oxygen to fill a mask.";

				// Token: 0x04009D1F RID: 40223
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses designated ",
					UI.FormatAsLink("Metal Ores", "METAL"),
					" from filter settings to create ",
					UI.FormatAsLink("Oxygen Masks", "OXYGENMASK"),
					".\n\nAutomatically draws in ambient ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" to fill masks.\n\nMarks a threshold where Duplicants must put on or take off a mask.\n\nCan be rotated before construction."
				});
			}

			// Token: 0x0200235B RID: 9051
			public class SWEEPBOTSTATION
			{
				// Token: 0x04009D20 RID: 40224
				public static LocString NAME = UI.FormatAsLink("Sweepy's Dock", "SWEEPBOTSTATION");

				// Token: 0x04009D21 RID: 40225
				public static LocString NAMEDSTATION = "{0}'s Dock";

				// Token: 0x04009D22 RID: 40226
				public static LocString DESC = "The cute little face comes pre-installed.";

				// Token: 0x04009D23 RID: 40227
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Deploys an automated ",
					UI.FormatAsLink("Sweepy Bot", "SWEEPBOT"),
					" to sweep up ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					" debris and ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" spills.\n\nDock stores ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" and ",
					UI.FormatAsLink("Solids", "ELEMENTS_SOLID"),
					" gathered by the Sweepy.\n\nUses ",
					UI.FormatAsLink("Power", "POWER"),
					" to recharge the Sweepy.\n\nDuplicants will empty Dock storage into available storage bins."
				});
			}

			// Token: 0x0200235C RID: 9052
			public class OXYGENMASKMARKER
			{
				// Token: 0x04009D24 RID: 40228
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask Checkpoint", "OXYGENMASKMARKER");

				// Token: 0x04009D25 RID: 40229
				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				// Token: 0x04009D26 RID: 40230
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must put on or take off an ",
					UI.FormatAsLink("Oxygen Mask", "OXYGEN_MASK"),
					".\n\nMust be built next to an ",
					UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER"),
					".\n\nCan be rotated before construction."
				});
			}

			// Token: 0x0200235D RID: 9053
			public class OXYGENMASKLOCKER
			{
				// Token: 0x04009D27 RID: 40231
				public static LocString NAME = UI.FormatAsLink("Oxygen Mask Dock", "OXYGENMASKLOCKER");

				// Token: 0x04009D28 RID: 40232
				public static LocString DESC = "An oxygen mask dock will store and refill masks while they're not in use.";

				// Token: 0x04009D29 RID: 40233
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Oxygen Masks", "OXYGEN_MASK"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nBuild next to an ",
					UI.FormatAsLink("Oxygen Mask Checkpoint", "OXYGENMASKMARKER"),
					" to make Duplicants put on masks when passing by."
				});
			}

			// Token: 0x0200235E RID: 9054
			public class SUITMARKER
			{
				// Token: 0x04009D2A RID: 40234
				public static LocString NAME = UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER");

				// Token: 0x04009D2B RID: 40235
				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				// Token: 0x04009D2C RID: 40236
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must change into or out of ",
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					".\n\nMust be built next to an ",
					UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER"),
					".\n\nCan be rotated before construction."
				});
			}

			// Token: 0x0200235F RID: 9055
			public class SUITLOCKER
			{
				// Token: 0x04009D2D RID: 40237
				public static LocString NAME = UI.FormatAsLink("Atmo Suit Dock", "SUITLOCKER");

				// Token: 0x04009D2E RID: 40238
				public static LocString DESC = "An atmo suit dock will empty atmo suits of waste, but only one suit can charge at a time.";

				// Token: 0x04009D2F RID: 40239
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Atmo Suits", "ATMO_SUIT"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nEmpties suits of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					".\n\nBuild next to an ",
					UI.FormatAsLink("Atmo Suit Checkpoint", "SUITMARKER"),
					" to make Duplicants change into suits when passing by."
				});
			}

			// Token: 0x02002360 RID: 9056
			public class JETSUITMARKER
			{
				// Token: 0x04009D30 RID: 40240
				public static LocString NAME = UI.FormatAsLink("Jet Suit Checkpoint", "JETSUITMARKER");

				// Token: 0x04009D31 RID: 40241
				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				// Token: 0x04009D32 RID: 40242
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must change into or out of ",
					UI.FormatAsLink("Jet Suits", "JET_SUIT"),
					".\n\nMust be built next to a ",
					UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER"),
					".\n\nCan be rotated before construction."
				});
			}

			// Token: 0x02002361 RID: 9057
			public class JETSUITLOCKER
			{
				// Token: 0x04009D33 RID: 40243
				public static LocString NAME = UI.FormatAsLink("Jet Suit Dock", "JETSUITLOCKER");

				// Token: 0x04009D34 RID: 40244
				public static LocString DESC = "Jet suit docks can refill jet suits with air and fuel, or empty them of waste.";

				// Token: 0x04009D35 RID: 40245
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Jet Suits", "JET_SUIT"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					" and ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					".\n\nEmpties suits of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					".\n\nBuild next to a ",
					UI.FormatAsLink("Jet Suit Checkpoint", "JETSUITMARKER"),
					" to make Duplicants change into suits when passing by."
				});
			}

			// Token: 0x02002362 RID: 9058
			public class LEADSUITMARKER
			{
				// Token: 0x04009D36 RID: 40246
				public static LocString NAME = UI.FormatAsLink("Lead Suit Checkpoint", "LEADSUITMARKER");

				// Token: 0x04009D37 RID: 40247
				public static LocString DESC = "A checkpoint must have a correlating dock built on the opposite side its arrow faces.";

				// Token: 0x04009D38 RID: 40248
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Marks a threshold where Duplicants must change into or out of ",
					UI.FormatAsLink("Lead Suits", "LEAD_SUIT"),
					".\n\nMust be built next to a ",
					UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER"),
					"\n\nCan be rotated before construction."
				});
			}

			// Token: 0x02002363 RID: 9059
			public class LEADSUITLOCKER
			{
				// Token: 0x04009D39 RID: 40249
				public static LocString NAME = UI.FormatAsLink("Lead Suit Dock", "LEADSUITLOCKER");

				// Token: 0x04009D3A RID: 40250
				public static LocString DESC = "Lead suit docks can refill lead suits with air and empty them of waste.";

				// Token: 0x04009D3B RID: 40251
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores ",
					UI.FormatAsLink("Lead Suits", "LEAD_SUIT"),
					" and refuels them with ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nEmpties suits of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					".\n\nBuild next to a ",
					UI.FormatAsLink("Lead Suit Checkpoint", "LEADSUITMARKER"),
					" to make Duplicants change into suits when passing by."
				});
			}

			// Token: 0x02002364 RID: 9060
			public class CRAFTINGTABLE
			{
				// Token: 0x04009D3C RID: 40252
				public static LocString NAME = UI.FormatAsLink("Crafting Station", "CRAFTINGTABLE");

				// Token: 0x04009D3D RID: 40253
				public static LocString DESC = "Crafting stations allow Duplicants to make oxygen masks to wear in low breathability areas.";

				// Token: 0x04009D3E RID: 40254
				public static LocString EFFECT = "Produces items and equipment for Duplicant use.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x02002365 RID: 9061
			public class SUITFABRICATOR
			{
				// Token: 0x04009D3F RID: 40255
				public static LocString NAME = UI.FormatAsLink("Exosuit Forge", "SUITFABRICATOR");

				// Token: 0x04009D40 RID: 40256
				public static LocString DESC = "Exosuits can be filled with oxygen to allow Duplicants to safely enter hazardous areas.";

				// Token: 0x04009D41 RID: 40257
				public static LocString EFFECT = "Forges protective " + UI.FormatAsLink("Exosuits", "EXOSUIT") + " for Duplicants to wear.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x02002366 RID: 9062
			public class CLOTHINGALTERATIONSTATION
			{
				// Token: 0x04009D42 RID: 40258
				public static LocString NAME = UI.FormatAsLink("Clothing Refashionator", "CLOTHINGALTERATIONSTATION");

				// Token: 0x04009D43 RID: 40259
				public static LocString DESC = "Allows skilled Duplicants to add extra personal pizzazz to their wardrobe.";

				// Token: 0x04009D44 RID: 40260
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Upgrades ",
					UI.FormatAsLink("Snazzy Suits", "FUNKY_VEST"),
					" into ",
					UI.FormatAsLink("Primo Garb", "CUSTOM_CLOTHING"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});
			}

			// Token: 0x02002367 RID: 9063
			public class CLOTHINGFABRICATOR
			{
				// Token: 0x04009D45 RID: 40261
				public static LocString NAME = UI.FormatAsLink("Textile Loom", "CLOTHINGFABRICATOR");

				// Token: 0x04009D46 RID: 40262
				public static LocString DESC = "A textile loom can be used to spin Reed Fiber into wearable Duplicant clothing.";

				// Token: 0x04009D47 RID: 40263
				public static LocString EFFECT = "Tailors Duplicant " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " items.\n\nDuplicants will not fabricate items unless recipes are queued.";
			}

			// Token: 0x02002368 RID: 9064
			public class SOLIDBOOSTER
			{
				// Token: 0x04009D48 RID: 40264
				public static LocString NAME = UI.FormatAsLink("Solid Fuel Thruster", "SOLIDBOOSTER");

				// Token: 0x04009D49 RID: 40265
				public static LocString DESC = "Additional thrusters allow rockets to reach far away space destinations.";

				// Token: 0x04009D4A RID: 40266
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Burns ",
					UI.FormatAsLink("Iron", "IRON"),
					" and ",
					UI.FormatAsLink("Oxylite", "OXYROCK"),
					" to increase rocket exploration distance."
				});
			}

			// Token: 0x02002369 RID: 9065
			public class SPACEHEATER
			{
				// Token: 0x04009D4B RID: 40267
				public static LocString NAME = UI.FormatAsLink("Space Heater", "SPACEHEATER");

				// Token: 0x04009D4C RID: 40268
				public static LocString DESC = "A space heater will radiate heat for as long as it's powered.";

				// Token: 0x04009D4D RID: 40269
				public static LocString EFFECT = "Radiates a moderate amount of " + UI.FormatAsLink("Heat", "HEAT") + ".";
			}

			// Token: 0x0200236A RID: 9066
			public class SPICEGRINDER
			{
				// Token: 0x04009D4E RID: 40270
				public static LocString NAME = UI.FormatAsLink("Spice Grinder", "SPICEGRINDER");

				// Token: 0x04009D4F RID: 40271
				public static LocString DESC = "Crushed seeds and other edibles make excellent meal-enhancing additives.";

				// Token: 0x04009D50 RID: 40272
				public static LocString EFFECT = "Produces ingredients that add benefits to " + UI.FormatAsLink("foods", "FOOD") + " prepared at skilled cooking stations.";

				// Token: 0x04009D51 RID: 40273
				public static LocString INGREDIENTHEADER = "Ingredients per 1000kcal:";
			}

			// Token: 0x0200236B RID: 9067
			public class STORAGELOCKER
			{
				// Token: 0x04009D52 RID: 40274
				public static LocString NAME = UI.FormatAsLink("Storage Bin", "STORAGELOCKER");

				// Token: 0x04009D53 RID: 40275
				public static LocString DESC = "Resources left on the floor become \"debris\" and lower decor when not put away.";

				// Token: 0x04009D54 RID: 40276
				public static LocString EFFECT = "Stores the " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " of your choosing.";

				// Token: 0x02002F84 RID: 12164
				public class FACADES
				{
					// Token: 0x020032BB RID: 12987
					public class DEFAULT_STORAGELOCKER
					{
						// Token: 0x0400C9D8 RID: 51672
						public static LocString NAME = UI.FormatAsLink("Storage Bin", "STORAGELOCKER");

						// Token: 0x0400C9D9 RID: 51673
						public static LocString DESC = "Resources left on the floor become \"debris\" and lower decor when not put away.";
					}

					// Token: 0x020032BC RID: 12988
					public class GREEN_MUSH
					{
						// Token: 0x0400C9DA RID: 51674
						public static LocString NAME = UI.FormatAsLink("Mush Green Storage Bin", "STORAGELOCKER");

						// Token: 0x0400C9DB RID: 51675
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x020032BD RID: 12989
					public class RED_ROSE
					{
						// Token: 0x0400C9DC RID: 51676
						public static LocString NAME = UI.FormatAsLink("Puce Pink Storage Bin", "STORAGELOCKER");

						// Token: 0x0400C9DD RID: 51677
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x020032BE RID: 12990
					public class BLUE_BABYTEARS
					{
						// Token: 0x0400C9DE RID: 51678
						public static LocString NAME = UI.FormatAsLink("Weepy Blue Storage Bin", "STORAGELOCKER");

						// Token: 0x0400C9DF RID: 51679
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x020032BF RID: 12991
					public class PURPLE_BRAINFAT
					{
						// Token: 0x0400C9E0 RID: 51680
						public static LocString NAME = UI.FormatAsLink("Faint Purple Storage Bin", "STORAGELOCKER");

						// Token: 0x0400C9E1 RID: 51681
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}

					// Token: 0x020032C0 RID: 12992
					public class YELLOW_TARTAR
					{
						// Token: 0x0400C9E2 RID: 51682
						public static LocString NAME = UI.FormatAsLink("Ick Yellow Storage Bin", "STORAGELOCKER");

						// Token: 0x0400C9E3 RID: 51683
						public static LocString DESC = "Color-coded storage makes things easier to find.";
					}
				}
			}

			// Token: 0x0200236C RID: 9068
			public class STORAGELOCKERSMART
			{
				// Token: 0x04009D55 RID: 40277
				public static LocString NAME = UI.FormatAsLink("Smart Storage Bin", "STORAGELOCKERSMART");

				// Token: 0x04009D56 RID: 40278
				public static LocString DESC = "Smart storage bins can automate resource organization based on type and mass.";

				// Token: 0x04009D57 RID: 40279
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores the ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" of your choosing.\n\nSends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when bin is full."
				});

				// Token: 0x04009D58 RID: 40280
				public static LocString LOGIC_PORT = "Full/Not Full";

				// Token: 0x04009D59 RID: 40281
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when full";

				// Token: 0x04009D5A RID: 40282
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x0200236D RID: 9069
			public class OBJECTDISPENSER
			{
				// Token: 0x04009D5B RID: 40283
				public static LocString NAME = UI.FormatAsLink("Automatic Dispenser", "OBJECTDISPENSER");

				// Token: 0x04009D5C RID: 40284
				public static LocString DESC = "Automatic dispensers will store and drop resources in small quantities.";

				// Token: 0x04009D5D RID: 40285
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Stores any ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" delivered to it by Duplicants.\n\nDumps stored materials back into the world when it receives a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"."
				});

				// Token: 0x04009D5E RID: 40286
				public static LocString LOGIC_PORT = "Dump Trigger";

				// Token: 0x04009D5F RID: 40287
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Dump all stored materials";

				// Token: 0x04009D60 RID: 40288
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Store materials";
			}

			// Token: 0x0200236E RID: 9070
			public class LIQUIDRESERVOIR
			{
				// Token: 0x04009D61 RID: 40289
				public static LocString NAME = UI.FormatAsLink("Liquid Reservoir", "LIQUIDRESERVOIR");

				// Token: 0x04009D62 RID: 40290
				public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";

				// Token: 0x04009D63 RID: 40291
				public static LocString EFFECT = "Stores any " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " resources piped into it.";
			}

			// Token: 0x0200236F RID: 9071
			public class GASRESERVOIR
			{
				// Token: 0x04009D64 RID: 40292
				public static LocString NAME = UI.FormatAsLink("Gas Reservoir", "GASRESERVOIR");

				// Token: 0x04009D65 RID: 40293
				public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";

				// Token: 0x04009D66 RID: 40294
				public static LocString EFFECT = "Stores any " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " resources piped into it.";

				// Token: 0x02002F85 RID: 12165
				public class FACADES
				{
					// Token: 0x020032C1 RID: 12993
					public class DEFAULT_GASRESERVOIR
					{
						// Token: 0x0400C9E4 RID: 51684
						public static LocString NAME = UI.FormatAsLink("Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400C9E5 RID: 51685
						public static LocString DESC = "Reservoirs cannot receive manually delivered resources.";
					}

					// Token: 0x020032C2 RID: 12994
					public class LIGHTGOLD
					{
						// Token: 0x0400C9E6 RID: 51686
						public static LocString NAME = UI.FormatAsLink("Golden Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400C9E7 RID: 51687
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x020032C3 RID: 12995
					public class PEAGREEN
					{
						// Token: 0x0400C9E8 RID: 51688
						public static LocString NAME = UI.FormatAsLink("Greenpea Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400C9E9 RID: 51689
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x020032C4 RID: 12996
					public class LIGHTCOBALT
					{
						// Token: 0x0400C9EA RID: 51690
						public static LocString NAME = UI.FormatAsLink("Bluemoon Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400C9EB RID: 51691
						public static LocString DESC = "A colorful reservoir keeps gases neatly organized.";
					}

					// Token: 0x020032C5 RID: 12997
					public class POLKA_DARKPURPLERESIN
					{
						// Token: 0x0400C9EC RID: 51692
						public static LocString NAME = UI.FormatAsLink("Mod Dot Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400C9ED RID: 51693
						public static LocString DESC = "It sports the cheeriest of paint jobs. What a gas!";
					}

					// Token: 0x020032C6 RID: 12998
					public class POLKA_DARKNAVYNOOKGREEN
					{
						// Token: 0x0400C9EE RID: 51694
						public static LocString NAME = UI.FormatAsLink("Party Dot Gas Reservoir", "GASRESERVOIR");

						// Token: 0x0400C9EF RID: 51695
						public static LocString DESC = "Safe gas storage doesn't have to be dull.";
					}
				}
			}

			// Token: 0x02002370 RID: 9072
			public class SMARTRESERVOIR
			{
				// Token: 0x04009D67 RID: 40295
				public static LocString LOGIC_PORT = "Refill Parameters";

				// Token: 0x04009D68 RID: 40296
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when reservoir is less than <b>Low Threshold</b> full, until <b>High Threshold</b> is reached again";

				// Token: 0x04009D69 RID: 40297
				public static LocString LOGIC_PORT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when reservoir is <b>High Threshold</b> full, until <b>Low Threshold</b> is reached again";

				// Token: 0x04009D6A RID: 40298
				public static LocString ACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when reservoir is less than <b>{0}%</b> full, until it is <b>{1}% (High Threshold)</b> full";

				// Token: 0x04009D6B RID: 40299
				public static LocString DEACTIVATE_TOOLTIP = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " when reservoir is <b>{0}%</b> full, until it is less than <b>{1}% (Low Threshold)</b> full";

				// Token: 0x04009D6C RID: 40300
				public static LocString SIDESCREEN_TITLE = "Logic Activation Parameters";

				// Token: 0x04009D6D RID: 40301
				public static LocString SIDESCREEN_ACTIVATE = "Low Threshold:";

				// Token: 0x04009D6E RID: 40302
				public static LocString SIDESCREEN_DEACTIVATE = "High Threshold:";
			}

			// Token: 0x02002371 RID: 9073
			public class LIQUIDHEATER
			{
				// Token: 0x04009D6F RID: 40303
				public static LocString NAME = UI.FormatAsLink("Liquid Tepidizer", "LIQUIDHEATER");

				// Token: 0x04009D70 RID: 40304
				public static LocString DESC = "Tepidizers heat liquid which can kill waterborne germs.";

				// Token: 0x04009D71 RID: 40305
				public static LocString EFFECT = "Warms large bodies of " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + ".\n\nMust be fully submerged.";
			}

			// Token: 0x02002372 RID: 9074
			public class SWITCH
			{
				// Token: 0x04009D72 RID: 40306
				public static LocString NAME = UI.FormatAsLink("Switch", "SWITCH");

				// Token: 0x04009D73 RID: 40307
				public static LocString DESC = "Switches can only affect buildings that come after them on a circuit.";

				// Token: 0x04009D74 RID: 40308
				public static LocString EFFECT = "Turns " + UI.FormatAsLink("Power", "POWER") + " on or off.\n\nDoes not affect circuitry preceding the switch.";

				// Token: 0x04009D75 RID: 40309
				public static LocString SIDESCREEN_TITLE = "Switch";

				// Token: 0x04009D76 RID: 40310
				public static LocString TURN_ON = "Turn On";

				// Token: 0x04009D77 RID: 40311
				public static LocString TURN_ON_TOOLTIP = "Turn On {Hotkey}";

				// Token: 0x04009D78 RID: 40312
				public static LocString TURN_OFF = "Turn Off";

				// Token: 0x04009D79 RID: 40313
				public static LocString TURN_OFF_TOOLTIP = "Turn Off {Hotkey}";
			}

			// Token: 0x02002373 RID: 9075
			public class LOGICPOWERRELAY
			{
				// Token: 0x04009D7A RID: 40314
				public static LocString NAME = UI.FormatAsLink("Power Shutoff", "LOGICPOWERRELAY");

				// Token: 0x04009D7B RID: 40315
				public static LocString DESC = "Automated systems save power and time by removing the need for Duplicant input.";

				// Token: 0x04009D7C RID: 40316
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off.\n\nDoes not affect circuitry preceding the switch."
				});

				// Token: 0x04009D7D RID: 40317
				public static LocString LOGIC_PORT = "Kill Power";

				// Token: 0x04009D7E RID: 40318
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					": Allow ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" through connected circuits"
				});

				// Token: 0x04009D7F RID: 40319
				public static LocString LOGIC_PORT_INACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					": Prevent ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from flowing through connected circuits"
				});
			}

			// Token: 0x02002374 RID: 9076
			public class LOGICINTERASTEROIDSENDER
			{
				// Token: 0x04009D80 RID: 40320
				public static LocString NAME = UI.FormatAsLink("Automation Broadcaster", "LOGICINTERASTEROIDSENDER");

				// Token: 0x04009D81 RID: 40321
				public static LocString DESC = "Sends automation signals into space.";

				// Token: 0x04009D82 RID: 40322
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to an ",
					UI.FormatAsLink("Automation Receiver", "LOGICINTERASTEROIDRECEIVER"),
					" over vast distances in space.\n\nBoth the Automation Broadcaster and the Automation Receiver must be exposed to space to function."
				});

				// Token: 0x04009D83 RID: 40323
				public static LocString DEFAULTNAME = "Unnamed Broadcaster";

				// Token: 0x04009D84 RID: 40324
				public static LocString LOGIC_PORT = "Broadcasting Signal";

				// Token: 0x04009D85 RID: 40325
				public static LocString LOGIC_PORT_ACTIVE = "Broadcasting: " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x04009D86 RID: 40326
				public static LocString LOGIC_PORT_INACTIVE = "Broadcasting: " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002375 RID: 9077
			public class LOGICINTERASTEROIDRECEIVER
			{
				// Token: 0x04009D87 RID: 40327
				public static LocString NAME = UI.FormatAsLink("Automation Receiver", "LOGICINTERASTEROIDRECEIVER");

				// Token: 0x04009D88 RID: 40328
				public static LocString DESC = "Receives automation signals from space.";

				// Token: 0x04009D89 RID: 40329
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Receives a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" from an ",
					UI.FormatAsLink("Automation Broadcaster", "LOGICINTERASTEROIDSENDER"),
					" over vast distances in space.\n\nBoth the Automation Receiver and the Automation Broadcaster must be exposed to space to function."
				});

				// Token: 0x04009D8A RID: 40330
				public static LocString LOGIC_PORT = "Receiving Signal";

				// Token: 0x04009D8B RID: 40331
				public static LocString LOGIC_PORT_ACTIVE = "Receiving: " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x04009D8C RID: 40332
				public static LocString LOGIC_PORT_INACTIVE = "Receiving: " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x02002376 RID: 9078
			public class TEMPERATURECONTROLLEDSWITCH
			{
				// Token: 0x04009D8D RID: 40333
				public static LocString NAME = UI.FormatAsLink("Thermo Switch", "TEMPERATURECONTROLLEDSWITCH");

				// Token: 0x04009D8E RID: 40334
				public static LocString DESC = "Automated switches can be used to manage circuits in areas where Duplicants cannot enter.";

				// Token: 0x04009D8F RID: 40335
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically turns ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off using ambient ",
					UI.FormatAsLink("Temperature", "HEAT"),
					".\n\nDoes not affect circuitry preceding the switch."
				});
			}

			// Token: 0x02002377 RID: 9079
			public class PRESSURESWITCHLIQUID
			{
				// Token: 0x04009D90 RID: 40336
				public static LocString NAME = UI.FormatAsLink("Hydro Switch", "PRESSURESWITCHLIQUID");

				// Token: 0x04009D91 RID: 40337
				public static LocString DESC = "A hydro switch shuts off power when the liquid pressure surrounding it surpasses the set threshold.";

				// Token: 0x04009D92 RID: 40338
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically turns ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off using ambient ",
					UI.FormatAsLink("Liquid Pressure", "PRESSURE"),
					".\n\nDoes not affect circuitry preceding the switch.\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});
			}

			// Token: 0x02002378 RID: 9080
			public class PRESSURESWITCHGAS
			{
				// Token: 0x04009D93 RID: 40339
				public static LocString NAME = UI.FormatAsLink("Atmo Switch", "PRESSURESWITCHGAS");

				// Token: 0x04009D94 RID: 40340
				public static LocString DESC = "An atmo switch shuts off power when the air pressure surrounding it surpasses the set threshold.";

				// Token: 0x04009D95 RID: 40341
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automatically turns ",
					UI.FormatAsLink("Power", "POWER"),
					" on or off using ambient ",
					UI.FormatAsLink("Gas Pressure", "PRESSURE"),
					" .\n\nDoes not affect circuitry preceding the switch."
				});
			}

			// Token: 0x02002379 RID: 9081
			public class TILE
			{
				// Token: 0x04009D96 RID: 40342
				public static LocString NAME = UI.FormatAsLink("Tile", "TILE");

				// Token: 0x04009D97 RID: 40343
				public static LocString DESC = "Tile can be used to bridge gaps and get to unreachable areas.";

				// Token: 0x04009D98 RID: 40344
				public static LocString EFFECT = "Used to build the walls and floors of rooms.\n\nIncreases Duplicant runspeed.";
			}

			// Token: 0x0200237A RID: 9082
			public class WALLTOILET
			{
				// Token: 0x04009D99 RID: 40345
				public static LocString NAME = UI.FormatAsLink("Wall Toilet", "WALLTOILET");

				// Token: 0x04009D9A RID: 40346
				public static LocString DESC = "Wall Toilets transmit fewer germs to Duplicants and require no emptying.";

				// Token: 0x04009D9B RID: 40347
				public static LocString EFFECT = "Gives Duplicants a place to relieve themselves. Empties directly on the other side of the wall.\n\nSpreads very few " + UI.FormatAsLink("Germs", "DISEASE") + ".";
			}

			// Token: 0x0200237B RID: 9083
			public class WATERPURIFIER
			{
				// Token: 0x04009D9C RID: 40348
				public static LocString NAME = UI.FormatAsLink("Water Sieve", "WATERPURIFIER");

				// Token: 0x04009D9D RID: 40349
				public static LocString DESC = "Sieves cannot kill germs and pass any they receive into their waste and water output.";

				// Token: 0x04009D9E RID: 40350
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces clean ",
					UI.FormatAsLink("Water", "WATER"),
					" from ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" using ",
					UI.FormatAsLink("Sand", "SAND"),
					".\n\nProduces ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					"."
				});
			}

			// Token: 0x0200237C RID: 9084
			public class DISTILLATIONCOLUMN
			{
				// Token: 0x04009D9F RID: 40351
				public static LocString NAME = UI.FormatAsLink("Distillation Column", "DISTILLATIONCOLUMN");

				// Token: 0x04009DA0 RID: 40352
				public static LocString DESC = "Gets hot and steamy.";

				// Token: 0x04009DA1 RID: 40353
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Separates any ",
					UI.FormatAsLink("Contaminated Water", "DIRTYWATER"),
					" piped through it into ",
					UI.FormatAsLink("Steam", "STEAM"),
					" and ",
					UI.FormatAsLink("Polluted Dirt", "TOXICSAND"),
					"."
				});
			}

			// Token: 0x0200237D RID: 9085
			public class WIRE
			{
				// Token: 0x04009DA2 RID: 40354
				public static LocString NAME = UI.FormatAsLink("Wire", "WIRE");

				// Token: 0x04009DA3 RID: 40355
				public static LocString DESC = "Electrical wire is used to connect generators, batteries, and buildings.";

				// Token: 0x04009DA4 RID: 40356
				public static LocString EFFECT = "Connects buildings to " + UI.FormatAsLink("Power", "POWER") + " sources.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x0200237E RID: 9086
			public class WIREBRIDGE
			{
				// Token: 0x04009DA5 RID: 40357
				public static LocString NAME = UI.FormatAsLink("Wire Bridge", "WIREBRIDGE");

				// Token: 0x04009DA6 RID: 40358
				public static LocString DESC = "Splitting generators onto separate grids can prevent overloads and wasted electricity.";

				// Token: 0x04009DA7 RID: 40359
				public static LocString EFFECT = "Runs one wire section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x0200237F RID: 9087
			public class HIGHWATTAGEWIRE
			{
				// Token: 0x04009DA8 RID: 40360
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE");

				// Token: 0x04009DA9 RID: 40361
				public static LocString DESC = "Higher wattage wire is used to avoid power overloads, particularly for strong generators.";

				// Token: 0x04009DAA RID: 40362
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than regular ",
					UI.FormatAsLink("Wire", "WIRE"),
					" without overloading.\n\nCannot be run through wall and floor tile."
				});
			}

			// Token: 0x02002380 RID: 9088
			public class WIREBRIDGEHIGHWATTAGE
			{
				// Token: 0x04009DAB RID: 40363
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Joint Plate", "WIREBRIDGEHIGHWATTAGE");

				// Token: 0x04009DAC RID: 40364
				public static LocString DESC = "Joint plates can run Heavi-Watt wires through walls without leaking gas or liquid.";

				// Token: 0x04009DAD RID: 40365
				public static LocString EFFECT = "Allows " + UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE") + " to be run through wall and floor tile.\n\nFunctions as regular tile.";
			}

			// Token: 0x02002381 RID: 9089
			public class WIREREFINED
			{
				// Token: 0x04009DAE RID: 40366
				public static LocString NAME = UI.FormatAsLink("Conductive Wire", "WIREREFINED");

				// Token: 0x04009DAF RID: 40367
				public static LocString DESC = "My Duplicants prefer the look of conductive wire to the regular raggedy stuff.";

				// Token: 0x04009DB0 RID: 40368
				public static LocString EFFECT = "Connects buildings to " + UI.FormatAsLink("Power", "POWER") + " sources.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x02002382 RID: 9090
			public class WIREREFINEDBRIDGE
			{
				// Token: 0x04009DB1 RID: 40369
				public static LocString NAME = UI.FormatAsLink("Conductive Wire Bridge", "WIREREFINEDBRIDGE");

				// Token: 0x04009DB2 RID: 40370
				public static LocString DESC = "Splitting generators onto separate systems can prevent overloads and wasted electricity.";

				// Token: 0x04009DB3 RID: 40371
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than a regular ",
					UI.FormatAsLink("Wire Bridge", "WIREBRIDGE"),
					" without overloading.\n\nRuns one wire section over another without joining them.\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x02002383 RID: 9091
			public class WIREREFINEDHIGHWATTAGE
			{
				// Token: 0x04009DB4 RID: 40372
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Conductive Wire", "WIREREFINEDHIGHWATTAGE");

				// Token: 0x04009DB5 RID: 40373
				public static LocString DESC = "Higher wattage wire is used to avoid power overloads, particularly for strong generators.";

				// Token: 0x04009DB6 RID: 40374
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than regular ",
					UI.FormatAsLink("Wire", "WIRE"),
					" without overloading.\n\nCannot be run through wall and floor tile."
				});
			}

			// Token: 0x02002384 RID: 9092
			public class WIREREFINEDBRIDGEHIGHWATTAGE
			{
				// Token: 0x04009DB7 RID: 40375
				public static LocString NAME = UI.FormatAsLink("Heavi-Watt Conductive Joint Plate", "WIREREFINEDBRIDGEHIGHWATTAGE");

				// Token: 0x04009DB8 RID: 40376
				public static LocString DESC = "Joint plates can run Heavi-Watt wires through walls without leaking gas or liquid.";

				// Token: 0x04009DB9 RID: 40377
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Carries more ",
					UI.FormatAsLink("Wattage", "POWER"),
					" than a regular ",
					UI.FormatAsLink("Heavi-Watt Joint Plate", "WIREBRIDGEHIGHWATTAGE"),
					" without overloading.\n\nAllows ",
					UI.FormatAsLink("Heavi-Watt Wire", "HIGHWATTAGEWIRE"),
					" to be run through wall and floor tile."
				});
			}

			// Token: 0x02002385 RID: 9093
			public class HANDSANITIZER
			{
				// Token: 0x04009DBA RID: 40378
				public static LocString NAME = UI.FormatAsLink("Hand Sanitizer", "HANDSANITIZER");

				// Token: 0x04009DBB RID: 40379
				public static LocString DESC = "Hand sanitizers kill germs more effectively than wash basins.";

				// Token: 0x04009DBC RID: 40380
				public static LocString EFFECT = "Removes most " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Hand Sanitizers when passing by in the selected direction.";
			}

			// Token: 0x02002386 RID: 9094
			public class WASHBASIN
			{
				// Token: 0x04009DBD RID: 40381
				public static LocString NAME = UI.FormatAsLink("Wash Basin", "WASHBASIN");

				// Token: 0x04009DBE RID: 40382
				public static LocString DESC = "Germ spread can be reduced by building these where Duplicants often get dirty.";

				// Token: 0x04009DBF RID: 40383
				public static LocString EFFECT = "Removes some " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Wash Basins when passing by in the selected direction.";
			}

			// Token: 0x02002387 RID: 9095
			public class WASHSINK
			{
				// Token: 0x04009DC0 RID: 40384
				public static LocString NAME = UI.FormatAsLink("Sink", "WASHSINK");

				// Token: 0x04009DC1 RID: 40385
				public static LocString DESC = "Sinks are plumbed and do not need to be manually emptied or refilled.";

				// Token: 0x04009DC2 RID: 40386
				public static LocString EFFECT = "Removes " + UI.FormatAsLink("Germs", "DISEASE") + " from Duplicants.\n\nGerm-covered Duplicants use Sinks when passing by in the selected direction.";
			}

			// Token: 0x02002388 RID: 9096
			public class DECONTAMINATIONSHOWER
			{
				// Token: 0x04009DC3 RID: 40387
				public static LocString NAME = UI.FormatAsLink("Decontamination Shower", "DECONTAMINATIONSHOWER");

				// Token: 0x04009DC4 RID: 40388
				public static LocString DESC = "Don't forget to decontaminate behind your ears.";

				// Token: 0x04009DC5 RID: 40389
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Water", "WATER"),
					" to remove ",
					UI.FormatAsLink("Germs", "DISEASE"),
					" and ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					".\n\nDecontaminates both Duplicants and their ",
					UI.FormatAsLink("Clothing", "EQUIPMENT"),
					"."
				});
			}

			// Token: 0x02002389 RID: 9097
			public class TILEPOI
			{
				// Token: 0x04009DC6 RID: 40390
				public static LocString NAME = UI.FormatAsLink("Tile", "TILEPOI");

				// Token: 0x04009DC7 RID: 40391
				public static LocString DESC = "";

				// Token: 0x04009DC8 RID: 40392
				public static LocString EFFECT = "Used to build the walls and floor of rooms.";
			}

			// Token: 0x0200238A RID: 9098
			public class POLYMERIZER
			{
				// Token: 0x04009DC9 RID: 40393
				public static LocString NAME = UI.FormatAsLink("Polymer Press", "POLYMERIZER");

				// Token: 0x04009DCA RID: 40394
				public static LocString DESC = "Plastic can be used to craft unique buildings and goods.";

				// Token: 0x04009DCB RID: 40395
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" into raw ",
					UI.FormatAsLink("Plastic", "POLYPROPYLENE"),
					"."
				});
			}

			// Token: 0x0200238B RID: 9099
			public class DIRECTIONALWORLDPUMPLIQUID
			{
				// Token: 0x04009DCC RID: 40396
				public static LocString NAME = UI.FormatAsLink("Liquid Channel", "DIRECTIONALWORLDPUMPLIQUID");

				// Token: 0x04009DCD RID: 40397
				public static LocString DESC = "Channels move more volume than pumps and require no power, but need sufficient pressure to function.";

				// Token: 0x04009DCE RID: 40398
				public static LocString EFFECT = "Directionally moves large volumes of " + UI.FormatAsLink("LIQUID", "ELEMENTS_LIQUID") + " through a channel.\n\nCan be used as floor tile and rotated before construction.";
			}

			// Token: 0x0200238C RID: 9100
			public class STEAMTURBINE
			{
				// Token: 0x04009DCF RID: 40399
				public static LocString NAME = UI.FormatAsLink("[DEPRECATED] Steam Turbine", "STEAMTURBINE");

				// Token: 0x04009DD0 RID: 40400
				public static LocString DESC = "Useful for converting the geothermal energy of magma into usable power.";

				// Token: 0x04009DD1 RID: 40401
				public static LocString EFFECT = string.Concat(new string[]
				{
					"THIS BUILDING HAS BEEN DEPRECATED AND CANNOT BE BUILT.\n\nGenerates exceptional electrical ",
					UI.FormatAsLink("Power", "POWER"),
					" using pressurized, ",
					UI.FormatAsLink("Scalding", "HEAT"),
					" ",
					UI.FormatAsLink("Steam", "STEAM"),
					".\n\nOutputs significantly cooler ",
					UI.FormatAsLink("Steam", "STEAM"),
					" than it receives.\n\nAir pressure beneath this building must be higher than pressure above for air to flow."
				});
			}

			// Token: 0x0200238D RID: 9101
			public class STEAMTURBINE2
			{
				// Token: 0x04009DD2 RID: 40402
				public static LocString NAME = UI.FormatAsLink("Steam Turbine", "STEAMTURBINE2");

				// Token: 0x04009DD3 RID: 40403
				public static LocString DESC = "Useful for converting the geothermal energy into usable power.";

				// Token: 0x04009DD4 RID: 40404
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Draws in ",
					UI.FormatAsLink("Steam", "STEAM"),
					" from the tiles directly below the machine's foundation and uses it to generate electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nOutputs ",
					UI.FormatAsLink("Water", "WATER"),
					"."
				});

				// Token: 0x04009DD5 RID: 40405
				public static LocString HEAT_SOURCE = "Power Generation Waste";
			}

			// Token: 0x0200238E RID: 9102
			public class STEAMENGINE
			{
				// Token: 0x04009DD6 RID: 40406
				public static LocString NAME = UI.FormatAsLink("Steam Engine", "STEAMENGINE");

				// Token: 0x04009DD7 RID: 40407
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x04009DD8 RID: 40408
				public static LocString EFFECT = "Utilizes " + UI.FormatAsLink("Steam", "STEAM") + " to propel rockets for space exploration.\n\nThe engine of a rocket must be built first before more rocket modules may be added.";
			}

			// Token: 0x0200238F RID: 9103
			public class STEAMENGINECLUSTER
			{
				// Token: 0x04009DD9 RID: 40409
				public static LocString NAME = UI.FormatAsLink("Steam Engine", "STEAMENGINECLUSTER");

				// Token: 0x04009DDA RID: 40410
				public static LocString DESC = "Rockets can be used to send Duplicants into space and retrieve rare resources.";

				// Token: 0x04009DDB RID: 40411
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Utilizes ",
					UI.FormatAsLink("Steam", "STEAM"),
					" to propel rockets for space exploration.\n\nEngine must be built via ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					". \n\nOnce the engine has been built, more rocket modules can be added."
				});
			}

			// Token: 0x02002390 RID: 9104
			public class SOLARPANEL
			{
				// Token: 0x04009DDC RID: 40412
				public static LocString NAME = UI.FormatAsLink("Solar Panel", "SOLARPANEL");

				// Token: 0x04009DDD RID: 40413
				public static LocString DESC = "Solar panels convert high intensity sunlight into power and produce zero waste.";

				// Token: 0x04009DDE RID: 40414
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Sunlight", "LIGHT"),
					" into electrical ",
					UI.FormatAsLink("Power", "POWER"),
					".\n\nMust be exposed to space."
				});
			}

			// Token: 0x02002391 RID: 9105
			public class COMETDETECTOR
			{
				// Token: 0x04009DDF RID: 40415
				public static LocString NAME = UI.FormatAsLink("Space Scanner", "COMETDETECTOR");

				// Token: 0x04009DE0 RID: 40416
				public static LocString DESC = "Networks of many scanners will scan more efficiently than one on its own.";

				// Token: 0x04009DE1 RID: 40417
				public static LocString EFFECT = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to its automation circuit when it detects incoming objects from space.\n\nCan be configured to detect incoming meteor showers or returning space rockets.";
			}

			// Token: 0x02002392 RID: 9106
			public class OILREFINERY
			{
				// Token: 0x04009DE2 RID: 40418
				public static LocString NAME = UI.FormatAsLink("Oil Refinery", "OILREFINERY");

				// Token: 0x04009DE3 RID: 40419
				public static LocString DESC = "Petroleum can only be produced from the refinement of crude oil.";

				// Token: 0x04009DE4 RID: 40420
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Converts ",
					UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
					" into ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					" and ",
					UI.FormatAsLink("Natural Gas", "METHANE"),
					"."
				});
			}

			// Token: 0x02002393 RID: 9107
			public class OILWELLCAP
			{
				// Token: 0x04009DE5 RID: 40421
				public static LocString NAME = UI.FormatAsLink("Oil Well", "OILWELLCAP");

				// Token: 0x04009DE6 RID: 40422
				public static LocString DESC = "Water pumped into an oil reservoir cannot be recovered.";

				// Token: 0x04009DE7 RID: 40423
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Extracts ",
					UI.FormatAsLink("Crude Oil", "CRUDEOIL"),
					" using clean ",
					UI.FormatAsLink("Water", "WATER"),
					".\n\nMust be built atop an ",
					UI.FormatAsLink("Oil Reservoir", "OIL_WELL"),
					"."
				});
			}

			// Token: 0x02002394 RID: 9108
			public class METALREFINERY
			{
				// Token: 0x04009DE8 RID: 40424
				public static LocString NAME = UI.FormatAsLink("Metal Refinery", "METALREFINERY");

				// Token: 0x04009DE9 RID: 40425
				public static LocString DESC = "Refined metals are necessary to build advanced electronics and technologies.";

				// Token: 0x04009DEA RID: 40426
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Refined Metals", "REFINEDMETAL"),
					" from raw ",
					UI.FormatAsLink("Metal Ore", "RAWMETAL"),
					".\n\nSignificantly ",
					UI.FormatAsLink("Heats", "HEAT"),
					" and outputs the ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" piped into it.\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x04009DEB RID: 40427
				public static LocString RECIPE_DESCRIPTION = "Extracts pure {0} from {1}.";
			}

			// Token: 0x02002395 RID: 9109
			public class MISSILEFABRICATOR
			{
				// Token: 0x04009DEC RID: 40428
				public static LocString NAME = UI.FormatAsLink("Blastshot Maker", "MISSILEFABRICATOR");

				// Token: 0x04009DED RID: 40429
				public static LocString DESC = "Blastshot shells are an effective defense against incoming meteor showers.";

				// Token: 0x04009DEE RID: 40430
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Blastshot", "MISSILELAUNCHER"),
					" from ",
					UI.FormatAsLink("Refined Metals", "REFINEDMETAL"),
					" combined with ",
					UI.FormatAsLink("Petroleum", "PETROLEUM"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x04009DEF RID: 40431
				public static LocString RECIPE_DESCRIPTION = "Produces {0} from {1} and {2}.";
			}

			// Token: 0x02002396 RID: 9110
			public class GLASSFORGE
			{
				// Token: 0x04009DF0 RID: 40432
				public static LocString NAME = UI.FormatAsLink("Glass Forge", "GLASSFORGE");

				// Token: 0x04009DF1 RID: 40433
				public static LocString DESC = "Glass can be used to construct window tile.";

				// Token: 0x04009DF2 RID: 40434
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Molten Glass", "MOLTENGLASS"),
					" from raw ",
					UI.FormatAsLink("Sand", "SAND"),
					".\n\nOutputs ",
					UI.FormatAsLink("High Temperature", "HEAT"),
					" ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					".\n\nDuplicants will not fabricate items unless recipes are queued."
				});

				// Token: 0x04009DF3 RID: 40435
				public static LocString RECIPE_DESCRIPTION = "Extracts pure {0} from {1}.";
			}

			// Token: 0x02002397 RID: 9111
			public class ROCKCRUSHER
			{
				// Token: 0x04009DF4 RID: 40436
				public static LocString NAME = UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER");

				// Token: 0x04009DF5 RID: 40437
				public static LocString DESC = "Rock Crushers loosen nuggets from raw ore and can process many different resources.";

				// Token: 0x04009DF6 RID: 40438
				public static LocString EFFECT = "Inefficiently produces refined materials from raw resources.\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x04009DF7 RID: 40439
				public static LocString RECIPE_DESCRIPTION = "Crushes {0} into {1}.";

				// Token: 0x04009DF8 RID: 40440
				public static LocString METAL_RECIPE_DESCRIPTION = "Crushes {1} into " + UI.FormatAsLink("Sand", "SAND") + " and pure {0}.";

				// Token: 0x04009DF9 RID: 40441
				public static LocString LIME_RECIPE_DESCRIPTION = "Crushes {1} into {0}";

				// Token: 0x04009DFA RID: 40442
				public static LocString LIME_FROM_LIMESTONE_RECIPE_DESCRIPTION = "Crushes {0} into {1} and a small amount of pure {2}";

				// Token: 0x02002F86 RID: 12166
				public class FACADES
				{
					// Token: 0x020032C7 RID: 12999
					public class DEFAULT_ROCKCRUSHER
					{
						// Token: 0x0400C9F0 RID: 51696
						public static LocString NAME = UI.FormatAsLink("Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400C9F1 RID: 51697
						public static LocString DESC = "Rock Crushers loosen nuggets from raw ore and can process many different resources.";
					}

					// Token: 0x020032C8 RID: 13000
					public class HANDS
					{
						// Token: 0x0400C9F2 RID: 51698
						public static LocString NAME = UI.FormatAsLink("Punchy Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400C9F3 RID: 51699
						public static LocString DESC = "Smashy smashy!";
					}

					// Token: 0x020032C9 RID: 13001
					public class TEETH
					{
						// Token: 0x0400C9F4 RID: 51700
						public static LocString NAME = UI.FormatAsLink("Toothy Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400C9F5 RID: 51701
						public static LocString DESC = "Not designed to handle overcooked food waste.";
					}

					// Token: 0x020032CA RID: 13002
					public class ROUNDSTAMP
					{
						// Token: 0x0400C9F6 RID: 51702
						public static LocString NAME = UI.FormatAsLink("Smooth Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400C9F7 RID: 51703
						public static LocString DESC = "Inspired by the traditional mortar and pestle.";
					}

					// Token: 0x020032CB RID: 13003
					public class SPIKEBEDS
					{
						// Token: 0x0400C9F8 RID: 51704
						public static LocString NAME = UI.FormatAsLink("Spiked Rock Crusher", "ROCKCRUSHER");

						// Token: 0x0400C9F9 RID: 51705
						public static LocString DESC = "Mashes rocks into oblivion.";
					}
				}
			}

			// Token: 0x02002398 RID: 9112
			public class SLUDGEPRESS
			{
				// Token: 0x04009DFB RID: 40443
				public static LocString NAME = UI.FormatAsLink("Sludge Press", "SLUDGEPRESS");

				// Token: 0x04009DFC RID: 40444
				public static LocString DESC = "What Duplicant doesn't love playing with mud?";

				// Token: 0x04009DFD RID: 40445
				public static LocString EFFECT = "Separates " + UI.FormatAsLink("Mud", "MUD") + " and other sludges into their base elements.\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x04009DFE RID: 40446
				public static LocString RECIPE_DESCRIPTION = "Separates {0} into its base elements.";
			}

			// Token: 0x02002399 RID: 9113
			public class SUPERMATERIALREFINERY
			{
				// Token: 0x04009DFF RID: 40447
				public static LocString NAME = UI.FormatAsLink("Molecular Forge", "SUPERMATERIALREFINERY");

				// Token: 0x04009E00 RID: 40448
				public static LocString DESC = "Rare materials can be procured through rocket missions into space.";

				// Token: 0x04009E01 RID: 40449
				public static LocString EFFECT = "Processes " + UI.FormatAsLink("Rare Materials", "RAREMATERIALS") + " into advanced industrial goods.\n\nRare materials can be retrieved from space missions.\n\nDuplicants will not fabricate items unless recipes are queued.";

				// Token: 0x04009E02 RID: 40450
				public static LocString SUPERCOOLANT_RECIPE_DESCRIPTION = "Super Coolant is an industrial-grade " + UI.FormatAsLink("Fullerene", "FULLERENE") + " coolant.";

				// Token: 0x04009E03 RID: 40451
				public static LocString SUPERINSULATOR_RECIPE_DESCRIPTION = string.Concat(new string[]
				{
					"Insulation reduces ",
					UI.FormatAsLink("Heat Transfer", "HEAT"),
					" and is composed of recrystallized ",
					UI.FormatAsLink("Abyssalite", "KATAIRITE"),
					"."
				});

				// Token: 0x04009E04 RID: 40452
				public static LocString TEMPCONDUCTORSOLID_RECIPE_DESCRIPTION = "Thermium is an industrial metal alloy formulated to maximize " + UI.FormatAsLink("Heat Transfer", "HEAT") + " and thermal dispersion.";

				// Token: 0x04009E05 RID: 40453
				public static LocString VISCOGEL_RECIPE_DESCRIPTION = "Visco-Gel Fluid is a " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " polymer with high surface tension.";

				// Token: 0x04009E06 RID: 40454
				public static LocString YELLOWCAKE_RECIPE_DESCRIPTION = "Yellowcake is a " + UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID") + " used in uranium enrichment.";

				// Token: 0x04009E07 RID: 40455
				public static LocString FULLERENE_RECIPE_DESCRIPTION = string.Concat(new string[]
				{
					"Fullerene is a ",
					UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID"),
					" used in the production of ",
					UI.FormatAsLink("Super Coolant", "SUPERCOOLANT"),
					"."
				});

				// Token: 0x04009E08 RID: 40456
				public static LocString HARDPLASTIC_RECIPE_DESCRIPTION = "Plastium is a highly heat-resistant, plastic-like " + UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID") + " used for space buildings.";
			}

			// Token: 0x0200239A RID: 9114
			public class THERMALBLOCK
			{
				// Token: 0x04009E09 RID: 40457
				public static LocString NAME = UI.FormatAsLink("Tempshift Plate", "THERMALBLOCK");

				// Token: 0x04009E0A RID: 40458
				public static LocString DESC = "The thermal properties of construction materials determine their heat retention.";

				// Token: 0x04009E0B RID: 40459
				public static LocString EFFECT = "Accelerates or buffers " + UI.FormatAsLink("Heat", "HEAT") + " dispersal based on the construction material used.\n\nHas a small area of effect.";
			}

			// Token: 0x0200239B RID: 9115
			public class POWERCONTROLSTATION
			{
				// Token: 0x04009E0C RID: 40460
				public static LocString NAME = UI.FormatAsLink("Power Control Station", "POWERCONTROLSTATION");

				// Token: 0x04009E0D RID: 40461
				public static LocString DESC = "Only one Duplicant may be assigned to a station at a time.";

				// Token: 0x04009E0E RID: 40462
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Microchip", "POWER_STATION_TOOLS"),
					" to increase the ",
					UI.FormatAsLink("Power", "POWER"),
					" output of generators.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Tune Up", "TECHNICALS2"),
					" trait.\n\nThis building is a necessary component of the Power Plant room."
				});
			}

			// Token: 0x0200239C RID: 9116
			public class FARMSTATION
			{
				// Token: 0x04009E0F RID: 40463
				public static LocString NAME = UI.FormatAsLink("Farm Station", "FARMSTATION");

				// Token: 0x04009E10 RID: 40464
				public static LocString DESC = "This station only has an effect on crops grown within the same room.";

				// Token: 0x04009E11 RID: 40465
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Produces ",
					UI.FormatAsLink("Micronutrient Fertilizer", "FARM_STATION_TOOLS"),
					" to increase ",
					UI.FormatAsLink("Plant", "PLANTS"),
					" growth rates.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Crop Tending", "FARMING2"),
					" trait.\n\nThis building is a necessary component of the Greenhouse room."
				});
			}

			// Token: 0x0200239D RID: 9117
			public class FISHDELIVERYPOINT
			{
				// Token: 0x04009E12 RID: 40466
				public static LocString NAME = UI.FormatAsLink("Fish Release", "FISHDELIVERYPOINT");

				// Token: 0x04009E13 RID: 40467
				public static LocString DESC = "A fish release must be built above liquid to prevent released fish from suffocating.";

				// Token: 0x04009E14 RID: 40468
				public static LocString EFFECT = "Releases trapped " + UI.FormatAsLink("Pacu", "PACU") + " back into the world.\n\nCan be used multiple times.";
			}

			// Token: 0x0200239E RID: 9118
			public class FISHFEEDER
			{
				// Token: 0x04009E15 RID: 40469
				public static LocString NAME = UI.FormatAsLink("Fish Feeder", "FISHFEEDER");

				// Token: 0x04009E16 RID: 40470
				public static LocString DESC = "Build this feeder above a body of water to feed the fish within.";

				// Token: 0x04009E17 RID: 40471
				public static LocString EFFECT = "Automatically dispenses stored " + UI.FormatAsLink("Critter", "CREATURES") + " food into the area below.\n\nDispenses continuously as food is consumed.";
			}

			// Token: 0x0200239F RID: 9119
			public class FISHTRAP
			{
				// Token: 0x04009E18 RID: 40472
				public static LocString NAME = UI.FormatAsLink("Fish Trap", "FISHTRAP");

				// Token: 0x04009E19 RID: 40473
				public static LocString DESC = "Trapped fish will automatically be bagged for transport.";

				// Token: 0x04009E1A RID: 40474
				public static LocString EFFECT = "Attracts and traps swimming " + UI.FormatAsLink("Pacu", "PACU") + ".\n\nSingle use.";
			}

			// Token: 0x020023A0 RID: 9120
			public class RANCHSTATION
			{
				// Token: 0x04009E1B RID: 40475
				public static LocString NAME = UI.FormatAsLink("Grooming Station", "RANCHSTATION");

				// Token: 0x04009E1C RID: 40476
				public static LocString DESC = "Grooming critters make them look nice, smell pretty, feel happy, and produce more.";

				// Token: 0x04009E1D RID: 40477
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows the assigned ",
					UI.FormatAsLink("Rancher", "RANCHER"),
					" to care for ",
					UI.FormatAsLink("Critters", "CREATURES"),
					".\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Critter Ranching", "RANCHING1"),
					" skill.\n\nThis building is a necessary component of the Stable room."
				});
			}

			// Token: 0x020023A1 RID: 9121
			public class MACHINESHOP
			{
				// Token: 0x04009E1E RID: 40478
				public static LocString NAME = UI.FormatAsLink("Mechanics Station", "MACHINESHOP");

				// Token: 0x04009E1F RID: 40479
				public static LocString DESC = "Duplicants will only improve the efficiency of buildings in the same room as this station.";

				// Token: 0x04009E20 RID: 40480
				public static LocString EFFECT = "Allows the assigned " + UI.FormatAsLink("Engineer", "MACHINE_TECHNICIAN") + " to improve building production efficiency.\n\nThis building is a necessary component of the Machine Shop room.";
			}

			// Token: 0x020023A2 RID: 9122
			public class LOGICWIRE
			{
				// Token: 0x04009E21 RID: 40481
				public static LocString NAME = UI.FormatAsLink("Automation Wire", "LOGICWIRE");

				// Token: 0x04009E22 RID: 40482
				public static LocString DESC = "Automation wire is used to connect building ports to automation gates.";

				// Token: 0x04009E23 RID: 40483
				public static LocString EFFECT = "Connects buildings to " + UI.FormatAsLink("Sensors", "LOGIC") + ".\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x020023A3 RID: 9123
			public class LOGICRIBBON
			{
				// Token: 0x04009E24 RID: 40484
				public static LocString NAME = UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON");

				// Token: 0x04009E25 RID: 40485
				public static LocString DESC = "Logic ribbons use significantly less space to carry multiple automation signals.";

				// Token: 0x04009E26 RID: 40486
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A 4-Bit ",
					BUILDINGS.PREFABS.LOGICWIRE.NAME,
					" which can carry up to four automation signals.\n\nUse a ",
					UI.FormatAsLink("Ribbon Writer", "LOGICRIBBONWRITER"),
					" to output to multiple Bits, and a ",
					UI.FormatAsLink("Ribbon Reader", "LOGICRIBBONREADER"),
					" to input from multiple Bits."
				});
			}

			// Token: 0x020023A4 RID: 9124
			public class LOGICWIREBRIDGE
			{
				// Token: 0x04009E27 RID: 40487
				public static LocString NAME = UI.FormatAsLink("Automation Wire Bridge", "LOGICWIREBRIDGE");

				// Token: 0x04009E28 RID: 40488
				public static LocString DESC = "Wire bridges allow multiple automation grids to exist in a small area without connecting.";

				// Token: 0x04009E29 RID: 40489
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Automation Wire", "LOGICWIRE") + " section over another without joining them.\n\nCan be run through wall and floor tile.";

				// Token: 0x04009E2A RID: 40490
				public static LocString LOGIC_PORT = "Transmit Signal";

				// Token: 0x04009E2B RID: 40491
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Pass through the " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x04009E2C RID: 40492
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Pass through the " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023A5 RID: 9125
			public class LOGICRIBBONBRIDGE
			{
				// Token: 0x04009E2D RID: 40493
				public static LocString NAME = UI.FormatAsLink("Automation Ribbon Bridge", "LOGICRIBBONBRIDGE");

				// Token: 0x04009E2E RID: 40494
				public static LocString DESC = "Wire bridges allow multiple automation grids to exist in a small area without connecting.";

				// Token: 0x04009E2F RID: 40495
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON") + " section over another without joining them.\n\nCan be run through wall and floor tile.";

				// Token: 0x04009E30 RID: 40496
				public static LocString LOGIC_PORT = "Transmit Signal";

				// Token: 0x04009E31 RID: 40497
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Pass through the " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x04009E32 RID: 40498
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Pass through the " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023A6 RID: 9126
			public class LOGICGATEAND
			{
				// Token: 0x04009E33 RID: 40499
				public static LocString NAME = UI.FormatAsLink("AND Gate", "LOGICGATEAND");

				// Token: 0x04009E34 RID: 40500
				public static LocString DESC = "This gate outputs a Green Signal when both its inputs are receiving Green Signals at the same time.";

				// Token: 0x04009E35 RID: 40501
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when both Input A <b>AND</b> Input B are receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when even one Input is receiving ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					"."
				});

				// Token: 0x04009E36 RID: 40502
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x04009E37 RID: 40503
				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if both Inputs are receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x04009E38 RID: 40504
				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if any Input is receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);
			}

			// Token: 0x020023A7 RID: 9127
			public class LOGICGATEOR
			{
				// Token: 0x04009E39 RID: 40505
				public static LocString NAME = UI.FormatAsLink("OR Gate", "LOGICGATEOR");

				// Token: 0x04009E3A RID: 40506
				public static LocString DESC = "This gate outputs a Green Signal if receiving one or more Green Signals.";

				// Token: 0x04009E3B RID: 40507
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if at least one of Input A <b>OR</b> Input B is receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when neither Input A or Input B are receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					"."
				});

				// Token: 0x04009E3C RID: 40508
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x04009E3D RID: 40509
				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if any Input is receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x04009E3E RID: 40510
				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if both Inputs are receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);
			}

			// Token: 0x020023A8 RID: 9128
			public class LOGICGATENOT
			{
				// Token: 0x04009E3F RID: 40511
				public static LocString NAME = UI.FormatAsLink("NOT Gate", "LOGICGATENOT");

				// Token: 0x04009E40 RID: 40512
				public static LocString DESC = "This gate reverses automation signals, turning a Green Signal into a Red Signal and vice versa.";

				// Token: 0x04009E41 RID: 40513
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the Input is receiving a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when its Input is receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"."
				});

				// Token: 0x04009E42 RID: 40514
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x04009E43 RID: 40515
				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if receiving " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

				// Token: 0x04009E44 RID: 40516
				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);
			}

			// Token: 0x020023A9 RID: 9129
			public class LOGICGATEXOR
			{
				// Token: 0x04009E45 RID: 40517
				public static LocString NAME = UI.FormatAsLink("XOR Gate", "LOGICGATEXOR");

				// Token: 0x04009E46 RID: 40518
				public static LocString DESC = "This gate outputs a Green Signal if exactly one of its Inputs is receiving a Green Signal.";

				// Token: 0x04009E47 RID: 40519
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if exactly one of its Inputs is receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					".\n\nOutputs a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" if both or neither Inputs are receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					"."
				});

				// Token: 0x04009E48 RID: 40520
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x04009E49 RID: 40521
				public static LocString OUTPUT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if exactly one of its Inputs is receiving " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x04009E4A RID: 40522
				public static LocString OUTPUT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if both Input signals match (any color)";
			}

			// Token: 0x020023AA RID: 9130
			public class LOGICGATEBUFFER
			{
				// Token: 0x04009E4B RID: 40523
				public static LocString NAME = UI.FormatAsLink("BUFFER Gate", "LOGICGATEBUFFER");

				// Token: 0x04009E4C RID: 40524
				public static LocString DESC = "This gate continues outputting a Green Signal for a short time after the gate stops receiving a Green Signal.";

				// Token: 0x04009E4D RID: 40525
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Outputs a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if the Input is receiving a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					".\n\nContinues sending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for an amount of buffer time after the Input receives a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					"."
				});

				// Token: 0x04009E4E RID: 40526
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x04009E4F RID: 40527
				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" while receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					". After receiving ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					", will continue sending ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" until the timer has expired"
				});

				// Token: 0x04009E50 RID: 40528
				public static LocString OUTPUT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".";
			}

			// Token: 0x020023AB RID: 9131
			public class LOGICGATEFILTER
			{
				// Token: 0x04009E51 RID: 40529
				public static LocString NAME = UI.FormatAsLink("FILTER Gate", "LOGICGATEFILTER");

				// Token: 0x04009E52 RID: 40530
				public static LocString DESC = "This gate only lets a Green Signal through if its Input has received a Green Signal that lasted longer than the selected filter time.";

				// Token: 0x04009E53 RID: 40531
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Only lets a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" through if the Input has received a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" for longer than the selected filter time.\n\nWill continue outputting a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" if the ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" did not last long enough."
				});

				// Token: 0x04009E54 RID: 40532
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x04009E55 RID: 40533
				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" after receiving ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" for longer than the selected filter timer"
				});

				// Token: 0x04009E56 RID: 40534
				public static LocString OUTPUT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ".";
			}

			// Token: 0x020023AC RID: 9132
			public class LOGICMEMORY
			{
				// Token: 0x04009E57 RID: 40535
				public static LocString NAME = UI.FormatAsLink("Memory Toggle", "LOGICMEMORY");

				// Token: 0x04009E58 RID: 40536
				public static LocString DESC = "A Memory stores a Green Signal received in the Set Port (S) until the Reset Port (R) receives a Green Signal.";

				// Token: 0x04009E59 RID: 40537
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Contains an internal Memory, and will output whatever signal is stored in that Memory.\n\nSignals sent to the Inputs <i>only</i> affect the Memory, and do not pass through to the Output. \n\nSending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to the Set Port (S) will set the memory to ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					". \n\nSending a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to the Reset Port (R) will reset the memory back to ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					"."
				});

				// Token: 0x04009E5A RID: 40538
				public static LocString STATUS_ITEM_VALUE = "Current Value: {0}";

				// Token: 0x04009E5B RID: 40539
				public static LocString READ_PORT = "MEMORY OUTPUT";

				// Token: 0x04009E5C RID: 40540
				public static LocString READ_PORT_ACTIVE = "Outputs a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the internal Memory is set to " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x04009E5D RID: 40541
				public static LocString READ_PORT_INACTIVE = "Outputs a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if the internal Memory is set to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

				// Token: 0x04009E5E RID: 40542
				public static LocString SET_PORT = "SET PORT (S)";

				// Token: 0x04009E5F RID: 40543
				public static LocString SET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Set the internal Memory to " + UI.FormatAsAutomationState("Green", UI.AutomationState.Active);

				// Token: 0x04009E60 RID: 40544
				public static LocString SET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": No effect";

				// Token: 0x04009E61 RID: 40545
				public static LocString RESET_PORT = "RESET PORT (R)";

				// Token: 0x04009E62 RID: 40546
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the internal Memory to " + UI.FormatAsAutomationState("Red", UI.AutomationState.Standby);

				// Token: 0x04009E63 RID: 40547
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": No effect";
			}

			// Token: 0x020023AD RID: 9133
			public class LOGICGATEMULTIPLEXER
			{
				// Token: 0x04009E64 RID: 40548
				public static LocString NAME = UI.FormatAsLink("Signal Selector", "LOGICGATEMULTIPLEXER");

				// Token: 0x04009E65 RID: 40549
				public static LocString DESC = "Signal Selectors can be used to select which automation signal is relevant to pass through to a given circuit";

				// Token: 0x04009E66 RID: 40550
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Select which one of four Input signals should be sent out the Output, using Control Inputs.\n\nSend a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to the two Control Inputs to determine which Input is selected."
				});

				// Token: 0x04009E67 RID: 40551
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x04009E68 RID: 40552
				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Receives a ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" or ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					" signal from the selected input"
				});

				// Token: 0x04009E69 RID: 40553
				public static LocString OUTPUT_INACTIVE = "Nothing";
			}

			// Token: 0x020023AE RID: 9134
			public class LOGICGATEDEMULTIPLEXER
			{
				// Token: 0x04009E6A RID: 40554
				public static LocString NAME = UI.FormatAsLink("Signal Distributor", "LOGICGATEDEMULTIPLEXER");

				// Token: 0x04009E6B RID: 40555
				public static LocString DESC = "Signal Distributors can be used to choose which circuit should receive a given automation signal.";

				// Token: 0x04009E6C RID: 40556
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Route a single Input signal out one of four possible Outputs, based on the selection made by the Control Inputs.\n\nSend a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to the two Control Inputs to determine which Output is selected."
				});

				// Token: 0x04009E6D RID: 40557
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x04009E6E RID: 40558
				public static LocString OUTPUT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" or ",
					UI.FormatAsAutomationState("Red", UI.AutomationState.Standby),
					" signal to the selected output"
				});

				// Token: 0x04009E6F RID: 40559
				public static LocString OUTPUT_INACTIVE = "Nothing";
			}

			// Token: 0x020023AF RID: 9135
			public class LOGICSWITCH
			{
				// Token: 0x04009E70 RID: 40560
				public static LocString NAME = UI.FormatAsLink("Signal Switch", "LOGICSWITCH");

				// Token: 0x04009E71 RID: 40561
				public static LocString DESC = "Signal switches don't turn grids on and off like power switches, but add an extra signal.";

				// Token: 0x04009E72 RID: 40562
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" on an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid."
				});

				// Token: 0x04009E73 RID: 40563
				public static LocString SIDESCREEN_TITLE = "Signal Switch";

				// Token: 0x04009E74 RID: 40564
				public static LocString LOGIC_PORT = "Signal Toggle";

				// Token: 0x04009E75 RID: 40565
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if toggled ON";

				// Token: 0x04009E76 RID: 40566
				public static LocString LOGIC_PORT_INACTIVE = "Sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " if toggled OFF";
			}

			// Token: 0x020023B0 RID: 9136
			public class LOGICPRESSURESENSORGAS
			{
				// Token: 0x04009E77 RID: 40567
				public static LocString NAME = UI.FormatAsLink("Atmo Sensor", "LOGICPRESSURESENSORGAS");

				// Token: 0x04009E78 RID: 40568
				public static LocString DESC = "Atmo sensors can be used to prevent excess oxygen production and overpressurization.";

				// Token: 0x04009E79 RID: 40569
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" pressure enters the chosen range."
				});

				// Token: 0x04009E7A RID: 40570
				public static LocString LOGIC_PORT = UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Pressure";

				// Token: 0x04009E7B RID: 40571
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if Gas pressure is within the selected range";

				// Token: 0x04009E7C RID: 40572
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023B1 RID: 9137
			public class LOGICPRESSURESENSORLIQUID
			{
				// Token: 0x04009E7D RID: 40573
				public static LocString NAME = UI.FormatAsLink("Hydro Sensor", "LOGICPRESSURESENSORLIQUID");

				// Token: 0x04009E7E RID: 40574
				public static LocString DESC = "A hydro sensor can tell a pump to refill its basin as soon as it contains too little liquid.";

				// Token: 0x04009E7F RID: 40575
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" pressure enters the chosen range.\n\nMust be submerged in ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					"."
				});

				// Token: 0x04009E80 RID: 40576
				public static LocString LOGIC_PORT = UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Pressure";

				// Token: 0x04009E81 RID: 40577
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if Liquid pressure is within the selected range";

				// Token: 0x04009E82 RID: 40578
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023B2 RID: 9138
			public class LOGICTEMPERATURESENSOR
			{
				// Token: 0x04009E83 RID: 40579
				public static LocString NAME = UI.FormatAsLink("Thermo Sensor", "LOGICTEMPERATURESENSOR");

				// Token: 0x04009E84 RID: 40580
				public static LocString DESC = "Thermo sensors can disable buildings when they approach dangerous temperatures.";

				// Token: 0x04009E85 RID: 40581
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ambient ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" enters the chosen range."
				});

				// Token: 0x04009E86 RID: 40582
				public static LocString LOGIC_PORT = "Ambient " + UI.FormatAsLink("Temperature", "HEAT");

				// Token: 0x04009E87 RID: 40583
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if ambient ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" is within the selected range"
				});

				// Token: 0x04009E88 RID: 40584
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023B3 RID: 9139
			public class LOGICLIGHTSENSOR
			{
				// Token: 0x04009E89 RID: 40585
				public static LocString NAME = UI.FormatAsLink("Light Sensor", "LOGICLIGHTSENSOR");

				// Token: 0x04009E8A RID: 40586
				public static LocString DESC = "Light sensors can tell surface bunker doors above solar panels to open or close based on solar light levels.";

				// Token: 0x04009E8B RID: 40587
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ambient ",
					UI.FormatAsLink("Brightness", "LIGHT"),
					" enters the chosen range."
				});

				// Token: 0x04009E8C RID: 40588
				public static LocString LOGIC_PORT = "Ambient " + UI.FormatAsLink("Brightness", "LIGHT");

				// Token: 0x04009E8D RID: 40589
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if ambient ",
					UI.FormatAsLink("Brightness", "LIGHT"),
					" is within the selected range"
				});

				// Token: 0x04009E8E RID: 40590
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023B4 RID: 9140
			public class LOGICWATTAGESENSOR
			{
				// Token: 0x04009E8F RID: 40591
				public static LocString NAME = UI.FormatAsLink("Wattage Sensor", "LOGICWATTSENSOR");

				// Token: 0x04009E90 RID: 40592
				public static LocString DESC = "Wattage sensors can send a signal when a building has switched on or off.";

				// Token: 0x04009E91 RID: 40593
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ",
					UI.FormatAsLink("Wattage", "POWER"),
					" consumed enters the chosen range."
				});

				// Token: 0x04009E92 RID: 40594
				public static LocString LOGIC_PORT = "Consumed " + UI.FormatAsLink("Wattage", "POWER");

				// Token: 0x04009E93 RID: 40595
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if current ",
					UI.FormatAsLink("Wattage", "POWER"),
					" is within the selected range"
				});

				// Token: 0x04009E94 RID: 40596
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023B5 RID: 9141
			public class LOGICHEPSENSOR
			{
				// Token: 0x04009E95 RID: 40597
				public static LocString NAME = UI.FormatAsLink("Radbolt Sensor", "LOGICHEPSENSOR");

				// Token: 0x04009E96 RID: 40598
				public static LocString DESC = "Radbolt sensors can send a signal when a Radbolt passes over them.";

				// Token: 0x04009E97 RID: 40599
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when Radbolts detected enters the chosen range."
				});

				// Token: 0x04009E98 RID: 40600
				public static LocString LOGIC_PORT = "Detected Radbolts";

				// Token: 0x04009E99 RID: 40601
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if detected Radbolts are within the selected range";

				// Token: 0x04009E9A RID: 40602
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023B6 RID: 9142
			public class LOGICTIMEOFDAYSENSOR
			{
				// Token: 0x04009E9B RID: 40603
				public static LocString NAME = UI.FormatAsLink("Cycle Sensor", "LOGICTIMEOFDAYSENSOR");

				// Token: 0x04009E9C RID: 40604
				public static LocString DESC = "Cycle sensors ensure systems always turn on at the same time, day or night, every cycle.";

				// Token: 0x04009E9D RID: 40605
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sets an automatic ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" and ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" schedule within one day-night cycle."
				});

				// Token: 0x04009E9E RID: 40606
				public static LocString LOGIC_PORT = "Cycle Time";

				// Token: 0x04009E9F RID: 40607
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if current time is within the selected ",
					UI.FormatAsAutomationState("Green", UI.AutomationState.Active),
					" range"
				});

				// Token: 0x04009EA0 RID: 40608
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023B7 RID: 9143
			public class LOGICTIMERSENSOR
			{
				// Token: 0x04009EA1 RID: 40609
				public static LocString NAME = UI.FormatAsLink("Timer Sensor", "LOGICTIMERSENSOR");

				// Token: 0x04009EA2 RID: 40610
				public static LocString DESC = "Timer sensors create automation schedules for very short or very long periods of time.";

				// Token: 0x04009EA3 RID: 40611
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Creates a timer to send ",
					UI.FormatAsAutomationState("Green Signals", UI.AutomationState.Active),
					" and ",
					UI.FormatAsAutomationState("Red Signals", UI.AutomationState.Standby),
					" for specific amounts of time."
				});

				// Token: 0x04009EA4 RID: 40612
				public static LocString LOGIC_PORT = "Timer Schedule";

				// Token: 0x04009EA5 RID: 40613
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " for the selected amount of Green time";

				// Token: 0x04009EA6 RID: 40614
				public static LocString LOGIC_PORT_INACTIVE = "Then, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " for the selected amount of Red time";
			}

			// Token: 0x020023B8 RID: 9144
			public class LOGICCRITTERCOUNTSENSOR
			{
				// Token: 0x04009EA7 RID: 40615
				public static LocString NAME = UI.FormatAsLink("Critter Sensor", "LOGICCRITTERCOUNTSENSOR");

				// Token: 0x04009EA8 RID: 40616
				public static LocString DESC = "Detecting critter populations can help adjust their automated feeding and care regimens.";

				// Token: 0x04009EA9 RID: 40617
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the number of eggs and critters in a room."
				});

				// Token: 0x04009EAA RID: 40618
				public static LocString LOGIC_PORT = "Critter Count";

				// Token: 0x04009EAB RID: 40619
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Critters and Eggs in the Room is greater than the selected threshold.";

				// Token: 0x04009EAC RID: 40620
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x04009EAD RID: 40621
				public static LocString SIDESCREEN_TITLE = "Critter Sensor";

				// Token: 0x04009EAE RID: 40622
				public static LocString COUNT_CRITTER_LABEL = "Count Critters";

				// Token: 0x04009EAF RID: 40623
				public static LocString COUNT_EGG_LABEL = "Count Eggs";
			}

			// Token: 0x020023B9 RID: 9145
			public class LOGICCLUSTERLOCATIONSENSOR
			{
				// Token: 0x04009EB0 RID: 40624
				public static LocString NAME = UI.FormatAsLink("Starmap Location Sensor", "LOGICCLUSTERLOCATIONSENSOR");

				// Token: 0x04009EB1 RID: 40625
				public static LocString DESC = "Starmap Location sensors can signal when a spacecraft is at a certain location";

				// Token: 0x04009EB2 RID: 40626
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Send ",
					UI.FormatAsAutomationState("Green Signals", UI.AutomationState.Active),
					" at the chosen Starmap locations and ",
					UI.FormatAsAutomationState("Red Signals", UI.AutomationState.Standby),
					" everywhere else."
				});

				// Token: 0x04009EB3 RID: 40627
				public static LocString LOGIC_PORT = "Starmap Location Sensor";

				// Token: 0x04009EB4 RID: 40628
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + "when a spacecraft is at the chosen Starmap locations";

				// Token: 0x04009EB5 RID: 40629
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023BA RID: 9146
			public class LOGICDUPLICANTSENSOR
			{
				// Token: 0x04009EB6 RID: 40630
				public static LocString NAME = UI.FormatAsLink("Duplicant Motion Sensor", "DUPLICANTSENSOR");

				// Token: 0x04009EB7 RID: 40631
				public static LocString DESC = "Motion sensors save power by only enabling buildings when Duplicants are nearby.";

				// Token: 0x04009EB8 RID: 40632
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on whether a Duplicant is in the sensor's range."
				});

				// Token: 0x04009EB9 RID: 40633
				public static LocString LOGIC_PORT = "Duplicant Motion Sensor";

				// Token: 0x04009EBA RID: 40634
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " while a Duplicant is in the sensor's tile range";

				// Token: 0x04009EBB RID: 40635
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023BB RID: 9147
			public class LOGICDISEASESENSOR
			{
				// Token: 0x04009EBC RID: 40636
				public static LocString NAME = UI.FormatAsLink("Germ Sensor", "LOGICDISEASESENSOR");

				// Token: 0x04009EBD RID: 40637
				public static LocString DESC = "Detecting germ populations can help block off or clean up dangerous areas.";

				// Token: 0x04009EBE RID: 40638
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on quantity of surrounding ",
					UI.FormatAsLink("Germs", "DISEASE"),
					"."
				});

				// Token: 0x04009EBF RID: 40639
				public static LocString LOGIC_PORT = UI.FormatAsLink("Germ", "DISEASE") + " Count";

				// Token: 0x04009EC0 RID: 40640
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs is within the selected range";

				// Token: 0x04009EC1 RID: 40641
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023BC RID: 9148
			public class LOGICELEMENTSENSORGAS
			{
				// Token: 0x04009EC2 RID: 40642
				public static LocString NAME = UI.FormatAsLink("Gas Element Sensor", "LOGICELEMENTSENSORGAS");

				// Token: 0x04009EC3 RID: 40643
				public static LocString DESC = "These sensors can detect the presence of a specific gas and alter systems accordingly.";

				// Token: 0x04009EC4 RID: 40644
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" is detected on this sensor's tile.\n\nSends a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when the selected ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" is not present."
				});

				// Token: 0x04009EC5 RID: 40645
				public static LocString LOGIC_PORT = "Specific " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Presence";

				// Token: 0x04009EC6 RID: 40646
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the selected Gas is detected";

				// Token: 0x04009EC7 RID: 40647
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023BD RID: 9149
			public class LOGICELEMENTSENSORLIQUID
			{
				// Token: 0x04009EC8 RID: 40648
				public static LocString NAME = UI.FormatAsLink("Liquid Element Sensor", "LOGICELEMENTSENSORLIQUID");

				// Token: 0x04009EC9 RID: 40649
				public static LocString DESC = "These sensors can detect the presence of a specific liquid and alter systems accordingly.";

				// Token: 0x04009ECA RID: 40650
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" is detected on this sensor's tile.\n\nSends a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when the selected ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" is not present."
				});

				// Token: 0x04009ECB RID: 40651
				public static LocString LOGIC_PORT = "Specific " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Presence";

				// Token: 0x04009ECC RID: 40652
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the selected Liquid is detected";

				// Token: 0x04009ECD RID: 40653
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023BE RID: 9150
			public class LOGICRADIATIONSENSOR
			{
				// Token: 0x04009ECE RID: 40654
				public static LocString NAME = UI.FormatAsLink("Radiation Sensor", "LOGICRADIATIONSENSOR");

				// Token: 0x04009ECF RID: 40655
				public static LocString DESC = "Radiation sensors can disable buildings when they detect dangerous levels of radiation.";

				// Token: 0x04009ED0 RID: 40656
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when ambient ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" enters the chosen range."
				});

				// Token: 0x04009ED1 RID: 40657
				public static LocString LOGIC_PORT = "Ambient " + UI.FormatAsLink("Radiation", "RADIATION");

				// Token: 0x04009ED2 RID: 40658
				public static LocString LOGIC_PORT_ACTIVE = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" if ambient ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" is within the selected range"
				});

				// Token: 0x04009ED3 RID: 40659
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023BF RID: 9151
			public class GASCONDUITDISEASESENSOR
			{
				// Token: 0x04009ED4 RID: 40660
				public static LocString NAME = UI.FormatAsLink("Gas Pipe Germ Sensor", "GASCONDUITDISEASESENSOR");

				// Token: 0x04009ED5 RID: 40661
				public static LocString DESC = "Germ sensors can help control automation behavior in the presence of germs.";

				// Token: 0x04009ED6 RID: 40662
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the internal ",
					UI.FormatAsLink("Germ", "DISEASE"),
					" count of the pipe."
				});

				// Token: 0x04009ED7 RID: 40663
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count";

				// Token: 0x04009ED8 RID: 40664
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs in the pipe is within the selected range";

				// Token: 0x04009ED9 RID: 40665
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023C0 RID: 9152
			public class LIQUIDCONDUITDISEASESENSOR
			{
				// Token: 0x04009EDA RID: 40666
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe Germ Sensor", "LIQUIDCONDUITDISEASESENSOR");

				// Token: 0x04009EDB RID: 40667
				public static LocString DESC = "Germ sensors can help control automation behavior in the presence of germs.";

				// Token: 0x04009EDC RID: 40668
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the internal ",
					UI.FormatAsLink("Germ", "DISEASE"),
					" count of the pipe."
				});

				// Token: 0x04009EDD RID: 40669
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count";

				// Token: 0x04009EDE RID: 40670
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs in the pipe is within the selected range";

				// Token: 0x04009EDF RID: 40671
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023C1 RID: 9153
			public class SOLIDCONDUITDISEASESENSOR
			{
				// Token: 0x04009EE0 RID: 40672
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail Germ Sensor", "SOLIDCONDUITDISEASESENSOR");

				// Token: 0x04009EE1 RID: 40673
				public static LocString DESC = "Germ sensors can help control automation behavior in the presence of germs.";

				// Token: 0x04009EE2 RID: 40674
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" based on the internal ",
					UI.FormatAsLink("Germ", "DISEASE"),
					" count of the object on the rail."
				});

				// Token: 0x04009EE3 RID: 40675
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Germ", "DISEASE") + " Count";

				// Token: 0x04009EE4 RID: 40676
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the number of Germs on the object on the rail is within the selected range";

				// Token: 0x04009EE5 RID: 40677
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023C2 RID: 9154
			public class GASCONDUITELEMENTSENSOR
			{
				// Token: 0x04009EE6 RID: 40678
				public static LocString NAME = UI.FormatAsLink("Gas Pipe Element Sensor", "GASCONDUITELEMENTSENSOR");

				// Token: 0x04009EE7 RID: 40679
				public static LocString DESC = "Element sensors can be used to detect the presence of a specific gas in a pipe.";

				// Token: 0x04009EE8 RID: 40680
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" is detected within a pipe."
				});

				// Token: 0x04009EE9 RID: 40681
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " Presence";

				// Token: 0x04009EEA RID: 40682
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured Gas is detected";

				// Token: 0x04009EEB RID: 40683
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023C3 RID: 9155
			public class LIQUIDCONDUITELEMENTSENSOR
			{
				// Token: 0x04009EEC RID: 40684
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe Element Sensor", "LIQUIDCONDUITELEMENTSENSOR");

				// Token: 0x04009EED RID: 40685
				public static LocString DESC = "Element sensors can be used to detect the presence of a specific liquid in a pipe.";

				// Token: 0x04009EEE RID: 40686
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" when the selected ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" is detected within a pipe."
				});

				// Token: 0x04009EEF RID: 40687
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " Presence";

				// Token: 0x04009EF0 RID: 40688
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured Liquid is detected within the pipe";

				// Token: 0x04009EF1 RID: 40689
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023C4 RID: 9156
			public class SOLIDCONDUITELEMENTSENSOR
			{
				// Token: 0x04009EF2 RID: 40690
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail Element Sensor", "SOLIDCONDUITELEMENTSENSOR");

				// Token: 0x04009EF3 RID: 40691
				public static LocString DESC = "Element sensors can be used to detect the presence of a specific item on a rail.";

				// Token: 0x04009EF4 RID: 40692
				public static LocString EFFECT = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the selected item is detected on a rail.";

				// Token: 0x04009EF5 RID: 40693
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Item", "ELEMENTS_LIQUID") + " Presence";

				// Token: 0x04009EF6 RID: 40694
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the configured item is detected on the rail";

				// Token: 0x04009EF7 RID: 40695
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023C5 RID: 9157
			public class GASCONDUITTEMPERATURESENSOR
			{
				// Token: 0x04009EF8 RID: 40696
				public static LocString NAME = UI.FormatAsLink("Gas Pipe Thermo Sensor", "GASCONDUITTEMPERATURESENSOR");

				// Token: 0x04009EF9 RID: 40697
				public static LocString DESC = "Thermo sensors disable buildings when their pipe contents reach a certain temperature.";

				// Token: 0x04009EFA RID: 40698
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when pipe contents enter the chosen ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" range."
				});

				// Token: 0x04009EFB RID: 40699
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Gas", "ELEMENTS_GAS") + " " + UI.FormatAsLink("Temperature", "HEAT");

				// Token: 0x04009EFC RID: 40700
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained Gas is within the selected Temperature range";

				// Token: 0x04009EFD RID: 40701
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023C6 RID: 9158
			public class LIQUIDCONDUITTEMPERATURESENSOR
			{
				// Token: 0x04009EFE RID: 40702
				public static LocString NAME = UI.FormatAsLink("Liquid Pipe Thermo Sensor", "LIQUIDCONDUITTEMPERATURESENSOR");

				// Token: 0x04009EFF RID: 40703
				public static LocString DESC = "Thermo sensors disable buildings when their pipe contents reach a certain temperature.";

				// Token: 0x04009F00 RID: 40704
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when pipe contents enter the chosen ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" range."
				});

				// Token: 0x04009F01 RID: 40705
				public static LocString LOGIC_PORT = "Internal " + UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID") + " " + UI.FormatAsLink("Temperature", "HEAT");

				// Token: 0x04009F02 RID: 40706
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained Liquid is within the selected Temperature range";

				// Token: 0x04009F03 RID: 40707
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023C7 RID: 9159
			public class SOLIDCONDUITTEMPERATURESENSOR
			{
				// Token: 0x04009F04 RID: 40708
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail Thermo Sensor", "SOLIDCONDUITTEMPERATURESENSOR");

				// Token: 0x04009F05 RID: 40709
				public static LocString DESC = "Thermo sensors disable buildings when their rail contents reach a certain temperature.";

				// Token: 0x04009F06 RID: 40710
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" when rail contents enter the chosen ",
					UI.FormatAsLink("Temperature", "HEAT"),
					" range."
				});

				// Token: 0x04009F07 RID: 40711
				public static LocString LOGIC_PORT = "Internal Item " + UI.FormatAsLink("Temperature", "HEAT");

				// Token: 0x04009F08 RID: 40712
				public static LocString LOGIC_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if the contained item is within the selected Temperature range";

				// Token: 0x04009F09 RID: 40713
				public static LocString LOGIC_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023C8 RID: 9160
			public class LOGICCOUNTER
			{
				// Token: 0x04009F0A RID: 40714
				public static LocString NAME = UI.FormatAsLink("Signal Counter", "LOGICCOUNTER");

				// Token: 0x04009F0B RID: 40715
				public static LocString DESC = "For numbers higher than ten connect multiple counters together.";

				// Token: 0x04009F0C RID: 40716
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Counts how many times a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" has been received up to a chosen number.\n\nWhen the chosen number is reached it sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" until it receives another ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					", when it resets automatically and begins counting again."
				});

				// Token: 0x04009F0D RID: 40717
				public static LocString LOGIC_PORT = "Internal Counter Value";

				// Token: 0x04009F0E RID: 40718
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Increase counter by one";

				// Token: 0x04009F0F RID: 40719
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";

				// Token: 0x04009F10 RID: 40720
				public static LocString LOGIC_PORT_RESET = "Reset Counter";

				// Token: 0x04009F11 RID: 40721
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset counter";

				// Token: 0x04009F12 RID: 40722
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";

				// Token: 0x04009F13 RID: 40723
				public static LocString LOGIC_PORT_OUTPUT = "Number Reached";

				// Token: 0x04009F14 RID: 40724
				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when the counter matches the selected value";

				// Token: 0x04009F15 RID: 40725
				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023C9 RID: 9161
			public class LOGICALARM
			{
				// Token: 0x04009F16 RID: 40726
				public static LocString NAME = UI.FormatAsLink("Automated Notifier", "LOGICALARM");

				// Token: 0x04009F17 RID: 40727
				public static LocString DESC = "Sends a notification when it receives a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".";

				// Token: 0x04009F18 RID: 40728
				public static LocString EFFECT = "Attach to sensors to send a notification when certain conditions are met.\n\nNotifications can be customized.";

				// Token: 0x04009F19 RID: 40729
				public static LocString LOGIC_PORT = "Notification";

				// Token: 0x04009F1A RID: 40730
				public static LocString INPUT_NAME = "INPUT";

				// Token: 0x04009F1B RID: 40731
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Push notification";

				// Token: 0x04009F1C RID: 40732
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x020023CA RID: 9162
			public class PIXELPACK
			{
				// Token: 0x04009F1D RID: 40733
				public static LocString NAME = UI.FormatAsLink("Pixel Pack", "PIXELPACK");

				// Token: 0x04009F1E RID: 40734
				public static LocString DESC = "Four pixels which can be individually designated different colors.";

				// Token: 0x04009F1F RID: 40735
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Pixels can be designated a color when it receives a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" and a different color when it receives a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					".\n\nInput from an ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE"),
					" controls the whole strip. Input from an ",
					UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON"),
					" can control individual pixels on the strip."
				});

				// Token: 0x04009F20 RID: 40736
				public static LocString LOGIC_PORT = "Color Selection";

				// Token: 0x04009F21 RID: 40737
				public static LocString INPUT_NAME = "RIBBON INPUT";

				// Token: 0x04009F22 RID: 40738
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Display the configured " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " pixels";

				// Token: 0x04009F23 RID: 40739
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Display the configured " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " pixels";

				// Token: 0x04009F24 RID: 40740
				public static LocString SIDESCREEN_TITLE = "Pixel Pack";
			}

			// Token: 0x020023CB RID: 9163
			public class LOGICHAMMER
			{
				// Token: 0x04009F25 RID: 40741
				public static LocString NAME = UI.FormatAsLink("Hammer", "LOGICHAMMER");

				// Token: 0x04009F26 RID: 40742
				public static LocString DESC = "The hammer makes neat sounds when it strikes buildings.";

				// Token: 0x04009F27 RID: 40743
				public static LocString EFFECT = "In its default orientation, the hammer strikes the building to the left when it receives a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ".\n\nEach building has a unique sound when struck by the hammer.\n\nThe hammer does no damage when it strikes.";

				// Token: 0x04009F28 RID: 40744
				public static LocString LOGIC_PORT = "Resonating Buildings";

				// Token: 0x04009F29 RID: 40745
				public static LocString INPUT_NAME = "INPUT";

				// Token: 0x04009F2A RID: 40746
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Hammer strikes once";

				// Token: 0x04009F2B RID: 40747
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x020023CC RID: 9164
			public class LOGICRIBBONWRITER
			{
				// Token: 0x04009F2C RID: 40748
				public static LocString NAME = UI.FormatAsLink("Ribbon Writer", "LOGICRIBBONWRITER");

				// Token: 0x04009F2D RID: 40749
				public static LocString DESC = "Translates the signal from an " + UI.FormatAsLink("Automation Wire", "LOGICWIRE") + " to a single Bit in an " + UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON");

				// Token: 0x04009F2E RID: 40750
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Writes a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to the specified Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME,
					"\n\n",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME,
					" must be used as the output wire to avoid overloading."
				});

				// Token: 0x04009F2F RID: 40751
				public static LocString LOGIC_PORT = "1-Bit Input";

				// Token: 0x04009F30 RID: 40752
				public static LocString INPUT_NAME = "INPUT";

				// Token: 0x04009F31 RID: 40753
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Receives " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to be written to selected Bit";

				// Token: 0x04009F32 RID: 40754
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Receives " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " to to be written selected Bit";

				// Token: 0x04009F33 RID: 40755
				public static LocString LOGIC_PORT_OUTPUT = "Bit Writing";

				// Token: 0x04009F34 RID: 40756
				public static LocString OUTPUT_NAME = "RIBBON OUTPUT";

				// Token: 0x04009F35 RID: 40757
				public static LocString OUTPUT_PORT_ACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					": Writes a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to selected Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME
				});

				// Token: 0x04009F36 RID: 40758
				public static LocString OUTPUT_PORT_INACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					": Writes a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to selected Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME
				});
			}

			// Token: 0x020023CD RID: 9165
			public class LOGICRIBBONREADER
			{
				// Token: 0x04009F37 RID: 40759
				public static LocString NAME = UI.FormatAsLink("Ribbon Reader", "LOGICRIBBONREADER");

				// Token: 0x04009F38 RID: 40760
				public static LocString DESC = string.Concat(new string[]
				{
					"Inputs the signal from a single Bit in an ",
					UI.FormatAsLink("Automation Ribbon", "LOGICRIBBON"),
					" into an ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE"),
					"."
				});

				// Token: 0x04009F39 RID: 40761
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Reads a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" or a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" from the specified Bit of an ",
					BUILDINGS.PREFABS.LOGICRIBBON.NAME,
					" onto an ",
					BUILDINGS.PREFABS.LOGICWIRE.NAME,
					"."
				});

				// Token: 0x04009F3A RID: 40762
				public static LocString LOGIC_PORT = "4-Bit Input";

				// Token: 0x04009F3B RID: 40763
				public static LocString INPUT_NAME = "RIBBON INPUT";

				// Token: 0x04009F3C RID: 40764
				public static LocString INPUT_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reads a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " from selected Bit";

				// Token: 0x04009F3D RID: 40765
				public static LocString INPUT_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Reads a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + " from selected Bit";

				// Token: 0x04009F3E RID: 40766
				public static LocString LOGIC_PORT_OUTPUT = "Bit Reading";

				// Token: 0x04009F3F RID: 40767
				public static LocString OUTPUT_NAME = "OUTPUT";

				// Token: 0x04009F40 RID: 40768
				public static LocString OUTPUT_PORT_ACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					": Sends a ",
					UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
					" to attached ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE")
				});

				// Token: 0x04009F41 RID: 40769
				public static LocString OUTPUT_PORT_INACTIVE = string.Concat(new string[]
				{
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					": Sends a ",
					UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
					" to attached ",
					UI.FormatAsLink("Automation Wire", "LOGICWIRE")
				});
			}

			// Token: 0x020023CE RID: 9166
			public class TRAVELTUBEENTRANCE
			{
				// Token: 0x04009F42 RID: 40770
				public static LocString NAME = UI.FormatAsLink("Transit Tube Access", "TRAVELTUBEENTRANCE");

				// Token: 0x04009F43 RID: 40771
				public static LocString DESC = "Duplicants require access points to enter tubes, but not to exit them.";

				// Token: 0x04009F44 RID: 40772
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants to enter the connected ",
					UI.FormatAsLink("Transit Tube", "TRAVELTUBE"),
					" system.\n\nStops drawing ",
					UI.FormatAsLink("Power", "POWER"),
					" once fully charged."
				});
			}

			// Token: 0x020023CF RID: 9167
			public class TRAVELTUBE
			{
				// Token: 0x04009F45 RID: 40773
				public static LocString NAME = UI.FormatAsLink("Transit Tube", "TRAVELTUBE");

				// Token: 0x04009F46 RID: 40774
				public static LocString DESC = "Duplicants will only exit a transit tube when a safe landing area is available beneath it.";

				// Token: 0x04009F47 RID: 40775
				public static LocString EFFECT = "Quickly transports Duplicants from a " + UI.FormatAsLink("Transit Tube Access", "TRAVELTUBEENTRANCE") + " to the tube's end.\n\nOnly transports Duplicants.";
			}

			// Token: 0x020023D0 RID: 9168
			public class TRAVELTUBEWALLBRIDGE
			{
				// Token: 0x04009F48 RID: 40776
				public static LocString NAME = UI.FormatAsLink("Transit Tube Crossing", "TRAVELTUBEWALLBRIDGE");

				// Token: 0x04009F49 RID: 40777
				public static LocString DESC = "Tube crossings can run transit tubes through walls without leaking gas or liquid.";

				// Token: 0x04009F4A RID: 40778
				public static LocString EFFECT = "Allows " + UI.FormatAsLink("Transit Tubes", "TRAVELTUBE") + " to be run through wall and floor tile.\n\nFunctions as regular tile.";
			}

			// Token: 0x020023D1 RID: 9169
			public class SOLIDCONDUIT
			{
				// Token: 0x04009F4B RID: 40779
				public static LocString NAME = UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT");

				// Token: 0x04009F4C RID: 40780
				public static LocString DESC = "Rails move materials where they'll be needed most, saving Duplicants the walk.";

				// Token: 0x04009F4D RID: 40781
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Transports ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" on a track between ",
					UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX"),
					" and ",
					UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX"),
					".\n\nCan be run through wall and floor tile."
				});
			}

			// Token: 0x020023D2 RID: 9170
			public class SOLIDCONDUITINBOX
			{
				// Token: 0x04009F4E RID: 40782
				public static LocString NAME = UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX");

				// Token: 0x04009F4F RID: 40783
				public static LocString DESC = "Material filters can be used to determine what resources are sent down the rail.";

				// Token: 0x04009F50 RID: 40784
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" onto ",
					UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT"),
					" for transport.\n\nOnly loads the resources of your choosing."
				});
			}

			// Token: 0x020023D3 RID: 9171
			public class SOLIDCONDUITOUTBOX
			{
				// Token: 0x04009F51 RID: 40785
				public static LocString NAME = UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX");

				// Token: 0x04009F52 RID: 40786
				public static LocString DESC = "When materials reach the end of a rail they enter a receptacle to be used by Duplicants.";

				// Token: 0x04009F53 RID: 40787
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" from a ",
					UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT"),
					" into storage."
				});
			}

			// Token: 0x020023D4 RID: 9172
			public class SOLIDTRANSFERARM
			{
				// Token: 0x04009F54 RID: 40788
				public static LocString NAME = UI.FormatAsLink("Auto-Sweeper", "SOLIDTRANSFERARM");

				// Token: 0x04009F55 RID: 40789
				public static LocString DESC = "An auto-sweeper's range can be viewed at any time by " + UI.CLICK(UI.ClickType.clicking) + " on the building.";

				// Token: 0x04009F56 RID: 40790
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Automates ",
					UI.FormatAsLink("Sweeping", "CHORES"),
					" and ",
					UI.FormatAsLink("Supplying", "CHORES"),
					" errands by sucking up all nearby ",
					UI.FormatAsLink("Debris", "DECOR"),
					".\n\nMaterials are automatically delivered to any ",
					UI.FormatAsLink("Conveyor Loader", "SOLIDCONDUITINBOX"),
					", ",
					UI.FormatAsLink("Conveyor Receptacle", "SOLIDCONDUITOUTBOX"),
					", storage, or buildings within range."
				});
			}

			// Token: 0x020023D5 RID: 9173
			public class SOLIDCONDUITBRIDGE
			{
				// Token: 0x04009F57 RID: 40791
				public static LocString NAME = UI.FormatAsLink("Conveyor Bridge", "SOLIDCONDUITBRIDGE");

				// Token: 0x04009F58 RID: 40792
				public static LocString DESC = "Separating rail systems helps ensure materials go to the intended destinations.";

				// Token: 0x04009F59 RID: 40793
				public static LocString EFFECT = "Runs one " + UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT") + " section over another without joining them.\n\nCan be run through wall and floor tile.";
			}

			// Token: 0x020023D6 RID: 9174
			public class SOLIDVENT
			{
				// Token: 0x04009F5A RID: 40794
				public static LocString NAME = UI.FormatAsLink("Conveyor Chute", "SOLIDVENT");

				// Token: 0x04009F5B RID: 40795
				public static LocString DESC = "When materials reach the end of a rail they are dropped back into the world.";

				// Token: 0x04009F5C RID: 40796
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID"),
					" from a ",
					UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT"),
					" onto the floor."
				});
			}

			// Token: 0x020023D7 RID: 9175
			public class SOLIDLOGICVALVE
			{
				// Token: 0x04009F5D RID: 40797
				public static LocString NAME = UI.FormatAsLink("Conveyor Shutoff", "SOLIDLOGICVALVE");

				// Token: 0x04009F5E RID: 40798
				public static LocString DESC = "Automated conveyors save power and time by removing the need for Duplicant input.";

				// Token: 0x04009F5F RID: 40799
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Connects to an ",
					UI.FormatAsLink("Automation", "LOGIC"),
					" grid to automatically turn ",
					UI.FormatAsLink("Solid Material", "ELEMENTS_SOLID"),
					" transport on or off."
				});

				// Token: 0x04009F60 RID: 40800
				public static LocString LOGIC_PORT = "Open/Close";

				// Token: 0x04009F61 RID: 40801
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow material transport";

				// Token: 0x04009F62 RID: 40802
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Prevent material transport";
			}

			// Token: 0x020023D8 RID: 9176
			public class SOLIDLIMITVALVE
			{
				// Token: 0x04009F63 RID: 40803
				public static LocString NAME = UI.FormatAsLink("Conveyor Meter", "SOLIDLIMITVALVE");

				// Token: 0x04009F64 RID: 40804
				public static LocString DESC = "Conveyor Meters let an exact amount of materials pass through before shutting off.";

				// Token: 0x04009F65 RID: 40805
				public static LocString EFFECT = "Connects to an " + UI.FormatAsLink("Automation", "LOGIC") + " grid to automatically turn material transfer off when the specified amount has passed through it.";

				// Token: 0x04009F66 RID: 40806
				public static LocString LOGIC_PORT_OUTPUT = "Limit Reached";

				// Token: 0x04009F67 RID: 40807
				public static LocString OUTPUT_PORT_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " if limit has been reached";

				// Token: 0x04009F68 RID: 40808
				public static LocString OUTPUT_PORT_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x04009F69 RID: 40809
				public static LocString LOGIC_PORT_RESET = "Reset Meter";

				// Token: 0x04009F6A RID: 40810
				public static LocString RESET_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Reset the amount";

				// Token: 0x04009F6B RID: 40811
				public static LocString RESET_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Nothing";
			}

			// Token: 0x020023D9 RID: 9177
			public class DEVPUMPSOLID
			{
				// Token: 0x04009F6C RID: 40812
				public static LocString NAME = UI.FormatAsLink("Dev Pump Solid", "DEVPUMPSOLID");

				// Token: 0x04009F6D RID: 40813
				public static LocString DESC = "Piping a pump's output to a building's intake will send solids to that building.";

				// Token: 0x04009F6E RID: 40814
				public static LocString EFFECT = "Generates chosen " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " and runs it through " + UI.FormatAsLink("Conveyor Rail", "SOLIDCONDUIT");
			}

			// Token: 0x020023DA RID: 9178
			public class AUTOMINER
			{
				// Token: 0x04009F6F RID: 40815
				public static LocString NAME = UI.FormatAsLink("Robo-Miner", "AUTOMINER");

				// Token: 0x04009F70 RID: 40816
				public static LocString DESC = "A robo-miner's range can be viewed at any time by selecting the building.";

				// Token: 0x04009F71 RID: 40817
				public static LocString EFFECT = "Automatically digs out all materials in a set range.";
			}

			// Token: 0x020023DB RID: 9179
			public class CREATUREFEEDER
			{
				// Token: 0x04009F72 RID: 40818
				public static LocString NAME = UI.FormatAsLink("Critter Feeder", "CREATUREFEEDER");

				// Token: 0x04009F73 RID: 40819
				public static LocString DESC = "Critters tend to stay close to their food source and wander less when given a feeder.";

				// Token: 0x04009F74 RID: 40820
				public static LocString EFFECT = "Automatically dispenses food for hungry " + UI.FormatAsLink("Critters", "CREATURES") + ".";
			}

			// Token: 0x020023DC RID: 9180
			public class GRAVITASPEDESTAL
			{
				// Token: 0x04009F75 RID: 40821
				public static LocString NAME = UI.FormatAsLink("Pedestal", "ITEMPEDESTAL");

				// Token: 0x04009F76 RID: 40822
				public static LocString DESC = "Perception can be drastically changed by a bit of thoughtful presentation.";

				// Token: 0x04009F77 RID: 40823
				public static LocString EFFECT = "Displays a single object, doubling its " + UI.FormatAsLink("Decor", "DECOR") + " value.\n\nObjects with negative Decor will gain some positive Decor when displayed.";

				// Token: 0x04009F78 RID: 40824
				public static LocString DISPLAYED_ITEM_FMT = "Displayed {0}";
			}

			// Token: 0x020023DD RID: 9181
			public class ITEMPEDESTAL
			{
				// Token: 0x04009F79 RID: 40825
				public static LocString NAME = UI.FormatAsLink("Pedestal", "ITEMPEDESTAL");

				// Token: 0x04009F7A RID: 40826
				public static LocString DESC = "Perception can be drastically changed by a bit of thoughtful presentation.";

				// Token: 0x04009F7B RID: 40827
				public static LocString EFFECT = "Displays a single object, doubling its " + UI.FormatAsLink("Decor", "DECOR") + " value.\n\nObjects with negative Decor will gain some positive Decor when displayed.";

				// Token: 0x04009F7C RID: 40828
				public static LocString DISPLAYED_ITEM_FMT = "Displayed {0}";

				// Token: 0x02002F87 RID: 12167
				public class FACADES
				{
					// Token: 0x020032CC RID: 13004
					public class DEFAULT_ITEMPEDESTAL
					{
						// Token: 0x0400C9FA RID: 51706
						public static LocString NAME = UI.FormatAsLink("Pedestal", "ITEMPEDESTAL");

						// Token: 0x0400C9FB RID: 51707
						public static LocString DESC = "Perception can be drastically changed by a bit of thoughtful presentation.";
					}

					// Token: 0x020032CD RID: 13005
					public class HAND
					{
						// Token: 0x0400C9FC RID: 51708
						public static LocString NAME = UI.FormatAsLink("Hand of Dupe Pedestal", "ITEMPEDESTAL");

						// Token: 0x0400C9FD RID: 51709
						public static LocString DESC = "This pedestal cradles precious objects in the palm of its hand.";
					}
				}
			}

			// Token: 0x020023DE RID: 9182
			public class CROWNMOULDING
			{
				// Token: 0x04009F7D RID: 40829
				public static LocString NAME = UI.FormatAsLink("Crown Moulding", "CROWNMOULDING");

				// Token: 0x04009F7E RID: 40830
				public static LocString DESC = "Crown moulding is used as purely decorative trim for ceilings.";

				// Token: 0x04009F7F RID: 40831
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to decorate the ceilings of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x02002F88 RID: 12168
				public class FACADES
				{
				}
			}

			// Token: 0x020023DF RID: 9183
			public class CORNERMOULDING
			{
				// Token: 0x04009F80 RID: 40832
				public static LocString NAME = UI.FormatAsLink("Corner Moulding", "CORNERMOULDING");

				// Token: 0x04009F81 RID: 40833
				public static LocString DESC = "Corner moulding is used as purely decorative trim for ceiling corners.";

				// Token: 0x04009F82 RID: 40834
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Used to decorate the ceiling corners of rooms.\n\nIncreases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x02002F89 RID: 12169
				public class FACADES
				{
				}
			}

			// Token: 0x020023E0 RID: 9184
			public class EGGINCUBATOR
			{
				// Token: 0x04009F83 RID: 40835
				public static LocString NAME = UI.FormatAsLink("Incubator", "EGGINCUBATOR");

				// Token: 0x04009F84 RID: 40836
				public static LocString DESC = "Incubators can maintain the ideal internal conditions for several species of critter egg.";

				// Token: 0x04009F85 RID: 40837
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Incubates ",
					UI.FormatAsLink("Critter", "CREATURES"),
					" eggs until ready to hatch.\n\nAssigned Duplicants must possess the ",
					UI.FormatAsLink("Critter Ranching", "RANCHING1"),
					" skill."
				});
			}

			// Token: 0x020023E1 RID: 9185
			public class EGGCRACKER
			{
				// Token: 0x04009F86 RID: 40838
				public static LocString NAME = UI.FormatAsLink("Egg Cracker", "EGGCRACKER");

				// Token: 0x04009F87 RID: 40839
				public static LocString DESC = "Raw eggs are an ingredient in certain high quality food recipes.";

				// Token: 0x04009F88 RID: 40840
				public static LocString EFFECT = "Converts viable " + UI.FormatAsLink("Critter", "CREATURES") + " eggs into cooking ingredients.\n\nCracked Eggs cannot hatch.\n\nDuplicants will not crack eggs unless tasks are queued.";

				// Token: 0x04009F89 RID: 40841
				public static LocString RECIPE_DESCRIPTION = "Turns {0} into {1}.";

				// Token: 0x04009F8A RID: 40842
				public static LocString RESULT_DESCRIPTION = "Cracked {0}";

				// Token: 0x02002F8A RID: 12170
				public class FACADES
				{
					// Token: 0x020032CE RID: 13006
					public class DEFAULT_EGGCRACKER
					{
						// Token: 0x0400C9FE RID: 51710
						public static LocString NAME = UI.FormatAsLink("Egg Cracker", "EGGCRACKER");

						// Token: 0x0400C9FF RID: 51711
						public static LocString DESC = "It cracks eggs.";
					}

					// Token: 0x020032CF RID: 13007
					public class BEAKER
					{
						// Token: 0x0400CA00 RID: 51712
						public static LocString NAME = UI.FormatAsLink("Beaker Cracker", "EGGCRACKER");

						// Token: 0x0400CA01 RID: 51713
						public static LocString DESC = "A practical exercise in physics.";
					}

					// Token: 0x020032D0 RID: 13008
					public class FLOWER
					{
						// Token: 0x0400CA02 RID: 51714
						public static LocString NAME = UI.FormatAsLink("Blossom Cracker", "EGGCRACKER");

						// Token: 0x0400CA03 RID: 51715
						public static LocString DESC = "Now with EZ-clean petals.";
					}

					// Token: 0x020032D1 RID: 13009
					public class HANDS
					{
						// Token: 0x0400CA04 RID: 51716
						public static LocString NAME = UI.FormatAsLink("Handy Cracker", "EGGCRACKER");

						// Token: 0x0400CA05 RID: 51717
						public static LocString DESC = "Just like Mi-Ma used to have.";
					}
				}
			}

			// Token: 0x020023E2 RID: 9186
			public class URANIUMCENTRIFUGE
			{
				// Token: 0x04009F8B RID: 40843
				public static LocString NAME = UI.FormatAsLink("Uranium Centrifuge", "URANIUMCENTRIFUGE");

				// Token: 0x04009F8C RID: 40844
				public static LocString DESC = "Enriched uranium is a specialized substance that can be used to fuel powerful research reactors.";

				// Token: 0x04009F8D RID: 40845
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Extracts ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" from ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					".\n\nOutputs ",
					UI.FormatAsLink("Depleted Uranium", "DEPLETEDURANIUM"),
					" in molten form."
				});

				// Token: 0x04009F8E RID: 40846
				public static LocString RECIPE_DESCRIPTION = "Convert Uranium ore to Molten Uranium and Enriched Uranium";
			}

			// Token: 0x020023E3 RID: 9187
			public class HIGHENERGYPARTICLEREDIRECTOR
			{
				// Token: 0x04009F8F RID: 40847
				public static LocString NAME = UI.FormatAsLink("Radbolt Reflector", "HIGHENERGYPARTICLEREDIRECTOR");

				// Token: 0x04009F90 RID: 40848
				public static LocString DESC = "We were all out of mirrors.";

				// Token: 0x04009F91 RID: 40849
				public static LocString EFFECT = "Receives and redirects Radbolts from " + UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER") + ".";

				// Token: 0x04009F92 RID: 40850
				public static LocString LOGIC_PORT = "Ignore incoming Radbolts";

				// Token: 0x04009F93 RID: 40851
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Allow incoming Radbolts";

				// Token: 0x04009F94 RID: 40852
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Ignore incoming Radbolts";
			}

			// Token: 0x020023E4 RID: 9188
			public class MANUALHIGHENERGYPARTICLESPAWNER
			{
				// Token: 0x04009F95 RID: 40853
				public static LocString NAME = UI.FormatAsLink("Manual Radbolt Generator", "MANUALHIGHENERGYPARTICLESPAWNER");

				// Token: 0x04009F96 RID: 40854
				public static LocString DESC = "Radbolts are necessary for producing Materials Science research.";

				// Token: 0x04009F97 RID: 40855
				public static LocString EFFECT = "Refines radioactive ores to generate Radbolts.\n\nEmits generated Radbolts in the direction of your choosing.";

				// Token: 0x04009F98 RID: 40856
				public static LocString RECIPE_DESCRIPTION = "Creates " + UI.FormatAsLink("Radbolts", "RADIATION") + " by processing {0}. Also creates {1} as a byproduct.";
			}

			// Token: 0x020023E5 RID: 9189
			public class HIGHENERGYPARTICLESPAWNER
			{
				// Token: 0x04009F99 RID: 40857
				public static LocString NAME = UI.FormatAsLink("Radbolt Generator", "HIGHENERGYPARTICLESPAWNER");

				// Token: 0x04009F9A RID: 40858
				public static LocString DESC = "Radbolts are necessary for producing Materials Science research.";

				// Token: 0x04009F9B RID: 40859
				public static LocString EFFECT = "Attracts nearby " + UI.FormatAsLink("Radiation", "RADIATION") + " to generate Radbolts.\n\nEmits generated Radbolts in the direction of your choosing when the set Radbolt threshold is reached.\n\nRadbolts collected will rapidly decay while this building is disabled.";

				// Token: 0x04009F9C RID: 40860
				public static LocString LOGIC_PORT = "Do not emit Radbolts";

				// Token: 0x04009F9D RID: 40861
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Emit Radbolts";

				// Token: 0x04009F9E RID: 40862
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Do not emit Radbolts";
			}

			// Token: 0x020023E6 RID: 9190
			public class DEVHEPSPAWNER
			{
				// Token: 0x04009F9F RID: 40863
				public static LocString NAME = "Dev Radbolt Generator";

				// Token: 0x04009FA0 RID: 40864
				public static LocString DESC = "Radbolts are necessary for producing Materials Science research.";

				// Token: 0x04009FA1 RID: 40865
				public static LocString EFFECT = "Generates Radbolts.";

				// Token: 0x04009FA2 RID: 40866
				public static LocString LOGIC_PORT = "Do not emit Radbolts";

				// Token: 0x04009FA3 RID: 40867
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Emit Radbolts";

				// Token: 0x04009FA4 RID: 40868
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Do not emit Radbolts";
			}

			// Token: 0x020023E7 RID: 9191
			public class HEPBATTERY
			{
				// Token: 0x04009FA5 RID: 40869
				public static LocString NAME = UI.FormatAsLink("Radbolt Chamber", "HEPBATTERY");

				// Token: 0x04009FA6 RID: 40870
				public static LocString DESC = "Particles packed up and ready to go.";

				// Token: 0x04009FA7 RID: 40871
				public static LocString EFFECT = "Stores Radbolts in a high-energy state, ready for transport.\n\nRequires a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " to release radbolts from storage when the Radbolt threshold is reached.\n\nRadbolts in storage will rapidly decay while this building is disabled.";

				// Token: 0x04009FA8 RID: 40872
				public static LocString LOGIC_PORT = "Do not emit Radbolts";

				// Token: 0x04009FA9 RID: 40873
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Emit Radbolts";

				// Token: 0x04009FAA RID: 40874
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Do not emit Radbolts";

				// Token: 0x04009FAB RID: 40875
				public static LocString LOGIC_PORT_STORAGE = "Radbolt Storage";

				// Token: 0x04009FAC RID: 40876
				public static LocString LOGIC_PORT_STORAGE_ACTIVE = "Sends a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + " when its Radbolt Storage is full";

				// Token: 0x04009FAD RID: 40877
				public static LocString LOGIC_PORT_STORAGE_INACTIVE = "Otherwise, sends a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020023E8 RID: 9192
			public class HEPBRIDGETILE
			{
				// Token: 0x04009FAE RID: 40878
				public static LocString NAME = UI.FormatAsLink("Radbolt Joint Plate", "HEPBRIDGETILE");

				// Token: 0x04009FAF RID: 40879
				public static LocString DESC = "Allows Radbolts to pass through walls.";

				// Token: 0x04009FB0 RID: 40880
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Receives ",
					UI.FormatAsLink("Radbolts", "RADIATION"),
					" from ",
					UI.FormatAsLink("Radbolt Generators", "HIGHENERGYPARTICLESPAWNER"),
					" and directs them through walls. All other materials and elements will be blocked from passage."
				});
			}

			// Token: 0x020023E9 RID: 9193
			public class ASTRONAUTTRAININGCENTER
			{
				// Token: 0x04009FB1 RID: 40881
				public static LocString NAME = UI.FormatAsLink("Space Cadet Centrifuge", "ASTRONAUTTRAININGCENTER");

				// Token: 0x04009FB2 RID: 40882
				public static LocString DESC = "Duplicants must complete astronaut training in order to pilot space rockets.";

				// Token: 0x04009FB3 RID: 40883
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Trains Duplicants to become ",
					UI.FormatAsLink("Astronaut", "ROCKETPILOTING1"),
					".\n\nDuplicants must possess the ",
					UI.FormatAsLink("Astronaut", "ROCKETPILOTING1"),
					" trait to receive training."
				});
			}

			// Token: 0x020023EA RID: 9194
			public class HOTTUB
			{
				// Token: 0x04009FB4 RID: 40884
				public static LocString NAME = UI.FormatAsLink("Hot Tub", "HOTTUB");

				// Token: 0x04009FB5 RID: 40885
				public static LocString DESC = "Relaxes Duplicants with massaging jets of heated liquid.";

				// Token: 0x04009FB6 RID: 40886
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Requires ",
					UI.FormatAsLink("Pipes", "LIQUIDPIPING"),
					" to and from tub and ",
					UI.FormatAsLink("Power", "POWER"),
					" to run the jets.\n\nWater must be a comfortable temperature and will cool rapidly.\n\nIncreases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});

				// Token: 0x04009FB7 RID: 40887
				public static LocString WATER_REQUIREMENT = "{element}: {amount}";

				// Token: 0x04009FB8 RID: 40888
				public static LocString WATER_REQUIREMENT_TOOLTIP = "This building must be filled with {amount} {element} in order to function.";

				// Token: 0x04009FB9 RID: 40889
				public static LocString TEMPERATURE_REQUIREMENT = "Minimum {element} Temperature: {temperature}";

				// Token: 0x04009FBA RID: 40890
				public static LocString TEMPERATURE_REQUIREMENT_TOOLTIP = "The Hot Tub will only be usable if supplied with {temperature} {element}. If the {element} gets too cold, the Hot Tub will drain and require refilling with {element}.";
			}

			// Token: 0x020023EB RID: 9195
			public class SODAFOUNTAIN
			{
				// Token: 0x04009FBB RID: 40891
				public static LocString NAME = UI.FormatAsLink("Soda Fountain", "SODAFOUNTAIN");

				// Token: 0x04009FBC RID: 40892
				public static LocString DESC = "Sparkling water puts a twinkle in a Duplicant's eye.";

				// Token: 0x04009FBD RID: 40893
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Creates soda from ",
					UI.FormatAsLink("Water", "WATER"),
					" and ",
					UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE"),
					".\n\nConsuming soda water increases Duplicant ",
					UI.FormatAsLink("Morale", "MORALE"),
					"."
				});
			}

			// Token: 0x020023EC RID: 9196
			public class UNCONSTRUCTEDROCKETMODULE
			{
				// Token: 0x04009FBE RID: 40894
				public static LocString NAME = "Empty Rocket Module";

				// Token: 0x04009FBF RID: 40895
				public static LocString DESC = "Something useful could be put here someday";

				// Token: 0x04009FC0 RID: 40896
				public static LocString EFFECT = "Can be changed into a different rocket module";
			}

			// Token: 0x020023ED RID: 9197
			public class MILKFATSEPARATOR
			{
				// Token: 0x04009FC1 RID: 40897
				public static LocString NAME = UI.FormatAsLink("Brackwax Gleaner", "MILKFATSEPARATOR");

				// Token: 0x04009FC2 RID: 40898
				public static LocString DESC = "Duplicants can slather up with brackwax to increase their travel speed in transit tubes.";

				// Token: 0x04009FC3 RID: 40899
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Refines ",
					ELEMENTS.MILK.NAME,
					" into ",
					ELEMENTS.BRINE.NAME,
					" and ",
					ELEMENTS.MILKFAT.NAME,
					", and emits ",
					ELEMENTS.CARBONDIOXIDE.NAME,
					"."
				});
			}

			// Token: 0x020023EE RID: 9198
			public class MILKFEEDER
			{
				// Token: 0x04009FC4 RID: 40900
				public static LocString NAME = UI.FormatAsLink("Critter Fountain", "MILKFEEDER");

				// Token: 0x04009FC5 RID: 40901
				public static LocString DESC = "It's easier to tolerate overcrowding when you're all hopped up on brackene.";

				// Token: 0x04009FC6 RID: 40902
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Dispenses ",
					ELEMENTS.MILK.NAME,
					" to a wide variety of ",
					UI.CODEX.CATEGORYNAMES.CREATURES,
					".\n\nAccessing the fountain significantly improves ",
					UI.CODEX.CATEGORYNAMES.CREATURES,
					"' moods."
				});
			}

			// Token: 0x020023EF RID: 9199
			public class MILKINGSTATION
			{
				// Token: 0x04009FC7 RID: 40903
				public static LocString NAME = UI.FormatAsLink("Milking Station", "MILKINGSTATION");

				// Token: 0x04009FC8 RID: 40904
				public static LocString DESC = "The harvested liquid is basically the equivalent of soda for critters.";

				// Token: 0x04009FC9 RID: 40905
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows Duplicants with the ",
					UI.FormatAsLink("Critter Ranching II", "RANCHING2"),
					" skill to milk ",
					UI.FormatAsLink("Gassy Moos", "MOO"),
					" for ",
					ELEMENTS.MILK.NAME,
					".\n\n",
					ELEMENTS.MILK.NAME,
					" can be used to refill the ",
					BUILDINGS.PREFABS.MILKFEEDER.NAME,
					"."
				});
			}

			// Token: 0x020023F0 RID: 9200
			public class MODULARLAUNCHPADPORT
			{
				// Token: 0x04009FCA RID: 40906
				public static LocString NAME = UI.FormatAsLink("Rocket Port", "MODULARLAUNCHPADPORTSOLID");

				// Token: 0x04009FCB RID: 40907
				public static LocString NAME_PLURAL = UI.FormatAsLink("Rocket Ports", "MODULARLAUNCHPADPORTSOLID");
			}

			// Token: 0x020023F1 RID: 9201
			public class MODULARLAUNCHPADPORTGAS
			{
				// Token: 0x04009FCC RID: 40908
				public static LocString NAME = UI.FormatAsLink("Gas Rocket Port Loader", "MODULARLAUNCHPADPORTGAS");

				// Token: 0x04009FCD RID: 40909
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x04009FCE RID: 40910
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" to the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the gas filters set on the rocket's cargo bays."
				});
			}

			// Token: 0x020023F2 RID: 9202
			public class MODULARLAUNCHPADPORTLIQUID
			{
				// Token: 0x04009FCF RID: 40911
				public static LocString NAME = UI.FormatAsLink("Liquid Rocket Port Loader", "MODULARLAUNCHPADPORTLIQUID");

				// Token: 0x04009FD0 RID: 40912
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x04009FD1 RID: 40913
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the liquid filters set on the rocket's cargo bays."
				});
			}

			// Token: 0x020023F3 RID: 9203
			public class MODULARLAUNCHPADPORTSOLID
			{
				// Token: 0x04009FD2 RID: 40914
				public static LocString NAME = UI.FormatAsLink("Solid Rocket Port Loader", "MODULARLAUNCHPADPORTSOLID");

				// Token: 0x04009FD3 RID: 40915
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x04009FD4 RID: 40916
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Loads ",
					UI.FormatAsLink("Solids", "ELEMENTS_SOLID"),
					" to the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the solid material filters set on the rocket's cargo bays."
				});
			}

			// Token: 0x020023F4 RID: 9204
			public class MODULARLAUNCHPADPORTGASUNLOADER
			{
				// Token: 0x04009FD5 RID: 40917
				public static LocString NAME = UI.FormatAsLink("Gas Rocket Port Unloader", "MODULARLAUNCHPADPORTGASUNLOADER");

				// Token: 0x04009FD6 RID: 40918
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x04009FD7 RID: 40919
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Gases", "ELEMENTS_GAS"),
					" from the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the gas filters set on this unloader."
				});
			}

			// Token: 0x020023F5 RID: 9205
			public class MODULARLAUNCHPADPORTLIQUIDUNLOADER
			{
				// Token: 0x04009FD8 RID: 40920
				public static LocString NAME = UI.FormatAsLink("Liquid Rocket Port Unloader", "MODULARLAUNCHPADPORTLIQUIDUNLOADER");

				// Token: 0x04009FD9 RID: 40921
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x04009FDA RID: 40922
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" from the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the liquid filters set on this unloader."
				});
			}

			// Token: 0x020023F6 RID: 9206
			public class MODULARLAUNCHPADPORTSOLIDUNLOADER
			{
				// Token: 0x04009FDB RID: 40923
				public static LocString NAME = UI.FormatAsLink("Solid Rocket Port Unloader", "MODULARLAUNCHPADPORTSOLIDUNLOADER");

				// Token: 0x04009FDC RID: 40924
				public static LocString DESC = "Rockets must be landed to load or unload resources.";

				// Token: 0x04009FDD RID: 40925
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unloads ",
					UI.FormatAsLink("Solids", "ELEMENTS_SOLID"),
					" from the storage of a linked rocket.\n\nAutomatically links when built to the side of a ",
					BUILDINGS.PREFABS.LAUNCHPAD.NAME,
					" or another ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					".\n\nUses the solid material filters set on this unloader."
				});
			}

			// Token: 0x020023F7 RID: 9207
			public class STICKERBOMB
			{
				// Token: 0x04009FDE RID: 40926
				public static LocString NAME = UI.FormatAsLink("Sticker Bomb", "STICKERBOMB");

				// Token: 0x04009FDF RID: 40927
				public static LocString DESC = "Surprise decor sneak attacks a Duplicant's gloomy day.";
			}

			// Token: 0x020023F8 RID: 9208
			public class HEATCOMPRESSOR
			{
				// Token: 0x04009FE0 RID: 40928
				public static LocString NAME = UI.FormatAsLink("Liquid Heatquilizer", "HEATCOMPRESSOR");

				// Token: 0x04009FE1 RID: 40929
				public static LocString DESC = "\"Room temperature\" is relative, really.";

				// Token: 0x04009FE2 RID: 40930
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Heats or cools ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					" to match ambient ",
					UI.FormatAsLink("Air Temperature", "HEAT"),
					"."
				});
			}

			// Token: 0x020023F9 RID: 9209
			public class PARTYCAKE
			{
				// Token: 0x04009FE3 RID: 40931
				public static LocString NAME = UI.FormatAsLink("Triple Decker Cake", "PARTYCAKE");

				// Token: 0x04009FE4 RID: 40932
				public static LocString DESC = "Any way you slice it, that's a good looking cake.";

				// Token: 0x04009FE5 RID: 40933
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Increases ",
					UI.FormatAsLink("Decor", "DECOR"),
					", contributing to ",
					UI.FormatAsLink("Morale", "MORALE"),
					".\n\nAdds a ",
					UI.FormatAsLink("Morale", "MORALE"),
					" bonus to Duplicants' parties."
				});
			}

			// Token: 0x020023FA RID: 9210
			public class RAILGUN
			{
				// Token: 0x04009FE6 RID: 40934
				public static LocString NAME = UI.FormatAsLink("Interplanetary Launcher", "RAILGUN");

				// Token: 0x04009FE7 RID: 40935
				public static LocString DESC = "It's tempting to climb inside but trust me... don't.";

				// Token: 0x04009FE8 RID: 40936
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Launches ",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" between Planetoids.\n\nPayloads can contain ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", or ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" materials.\n\nCannot transport Duplicants."
				});

				// Token: 0x04009FE9 RID: 40937
				public static LocString SIDESCREEN_HEP_REQUIRED = "Launch cost: {current} / {required} radbolts";

				// Token: 0x04009FEA RID: 40938
				public static LocString LOGIC_PORT = "Launch Toggle";

				// Token: 0x04009FEB RID: 40939
				public static LocString LOGIC_PORT_ACTIVE = UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ": Enable payload launching.";

				// Token: 0x04009FEC RID: 40940
				public static LocString LOGIC_PORT_INACTIVE = UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ": Disable payload launching.";
			}

			// Token: 0x020023FB RID: 9211
			public class RAILGUNPAYLOADOPENER
			{
				// Token: 0x04009FED RID: 40941
				public static LocString NAME = UI.FormatAsLink("Payload Opener", "RAILGUNPAYLOADOPENER");

				// Token: 0x04009FEE RID: 40942
				public static LocString DESC = "Payload openers can be hooked up to conveyors, plumbing and ventilation for improved sorting.";

				// Token: 0x04009FEF RID: 40943
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Unpacks ",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" delivered by Duplicants.\n\nAutomatically separates ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" materials and distributes them to the appropriate systems."
				});
			}

			// Token: 0x020023FC RID: 9212
			public class LANDINGBEACON
			{
				// Token: 0x04009FF0 RID: 40944
				public static LocString NAME = UI.FormatAsLink("Targeting Beacon", "LANDINGBEACON");

				// Token: 0x04009FF1 RID: 40945
				public static LocString DESC = "Microtarget where your " + UI.FormatAsLink("Interplanetary Payload", "RAILGUNPAYLOAD") + " lands on a Planetoid surface.";

				// Token: 0x04009FF2 RID: 40946
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Guides ",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" and ",
					UI.FormatAsLink("Orbital Cargo Modules", "ORBITALCARGOMODULE"),
					" to land nearby.\n\n",
					UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
					" must be launched from a ",
					UI.FormatAsLink("Interplanetary Launcher", "RAILGUN"),
					"."
				});
			}

			// Token: 0x020023FD RID: 9213
			public class DIAMONDPRESS
			{
				// Token: 0x04009FF3 RID: 40947
				public static LocString NAME = UI.FormatAsLink("Diamond Press", "DIAMONDPRESS");

				// Token: 0x04009FF4 RID: 40948
				public static LocString DESC = "Crushes refined carbon into diamond.";

				// Token: 0x04009FF5 RID: 40949
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					UI.FormatAsLink("Power", "POWER"),
					" and ",
					UI.FormatAsLink("Radbolts", "RADIATION"),
					" to crush ",
					UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
					" into ",
					UI.FormatAsLink("Diamond", "DIAMOND"),
					".\n\nDuplicants will not fabricate items unless recipes are queued and ",
					UI.FormatAsLink("Refined Carbon", "REFINEDCARBON"),
					" has been discovered."
				});

				// Token: 0x04009FF6 RID: 40950
				public static LocString REFINED_CARBON_RECIPE_DESCRIPTION = "Converts {1} to {0}";
			}

			// Token: 0x020023FE RID: 9214
			public class ESCAPEPOD
			{
				// Token: 0x04009FF7 RID: 40951
				public static LocString NAME = UI.FormatAsLink("Escape Pod", "ESCAPEPOD");

				// Token: 0x04009FF8 RID: 40952
				public static LocString DESC = "Delivers a Duplicant from a stranded rocket to the nearest Planetoid.";
			}

			// Token: 0x020023FF RID: 9215
			public class ROCKETINTERIORLIQUIDOUTPUTPORT
			{
				// Token: 0x04009FF9 RID: 40953
				public static LocString NAME = UI.FormatAsLink("Liquid Spacefarer Output Port", "ROCKETINTERIORLIQUIDOUTPUTPORT");

				// Token: 0x04009FFA RID: 40954
				public static LocString DESC = "A direct attachment to the input port on the exterior of a rocket.";

				// Token: 0x04009FFB RID: 40955
				public static LocString EFFECT = "Allows a direct conduit connection into the " + UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM") + " of a rocket.";
			}

			// Token: 0x02002400 RID: 9216
			public class ROCKETINTERIORLIQUIDINPUTPORT
			{
				// Token: 0x04009FFC RID: 40956
				public static LocString NAME = UI.FormatAsLink("Liquid Spacefarer Input Port", "ROCKETINTERIORLIQUIDINPUTPORT");

				// Token: 0x04009FFD RID: 40957
				public static LocString DESC = "A direct attachment to the output port on the exterior of a rocket.";

				// Token: 0x04009FFE RID: 40958
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows a direct conduit connection out of the ",
					UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM"),
					" of a rocket.\nCan be used to vent ",
					UI.FormatAsLink("Liquids", "ELEMENTS_LIQUID"),
					" to space during flight."
				});
			}

			// Token: 0x02002401 RID: 9217
			public class ROCKETINTERIORGASOUTPUTPORT
			{
				// Token: 0x04009FFF RID: 40959
				public static LocString NAME = UI.FormatAsLink("Gas Spacefarer Output Port", "ROCKETINTERIORGASOUTPUTPORT");

				// Token: 0x0400A000 RID: 40960
				public static LocString DESC = "A direct attachment to the input port on the exterior of a rocket.";

				// Token: 0x0400A001 RID: 40961
				public static LocString EFFECT = "Allows a direct conduit connection into the " + UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM") + " of a rocket.";
			}

			// Token: 0x02002402 RID: 9218
			public class ROCKETINTERIORGASINPUTPORT
			{
				// Token: 0x0400A002 RID: 40962
				public static LocString NAME = UI.FormatAsLink("Gas Spacefarer Input Port", "ROCKETINTERIORGASINPUTPORT");

				// Token: 0x0400A003 RID: 40963
				public static LocString DESC = "A direct attachment leading to the output port on the exterior of the rocket.";

				// Token: 0x0400A004 RID: 40964
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Allows a direct conduit connection out of the ",
					UI.FormatAsLink("Spacefarer Module", "HABITATMODULEMEDIUM"),
					" of the rocket.\nCan be used to vent ",
					UI.FormatAsLink("Gasses", "ELEMENTS_GAS"),
					" to space during flight."
				});
			}

			// Token: 0x02002403 RID: 9219
			public class MISSILELAUNCHER
			{
				// Token: 0x0400A005 RID: 40965
				public static LocString NAME = "Meteor Blaster";

				// Token: 0x0400A006 RID: 40966
				public static LocString DESC = "Some meteors drop harvestable resources when they're blown to smithereens.";

				// Token: 0x0400A007 RID: 40967
				public static LocString EFFECT = "Fires " + UI.FormatAsLink("Blastshot", "MISSILELAUNCHER") + " shells at meteor showers to defend the colony from impact-related damage.\n\nRange: 16 tiles horizontally, 32 tiles vertically.";

				// Token: 0x0400A008 RID: 40968
				public static LocString TARGET_SELECTION_HEADER = "Target Selection";

				// Token: 0x02002F8B RID: 12171
				public class BODY
				{
					// Token: 0x0400C1CA RID: 49610
					public static LocString CONTAINER1 = "Fires " + UI.FormatAsLink("Blastshot", "MISSILELAUNCHER") + " shells at meteor showers to defend the colony from impact-related damage.\n\nRange: 16 tiles horizontally, 32 tiles vertically.\n\nMeteors that have been blown to smithereens leave behind no harvestable resources.";
				}
			}

			// Token: 0x02002404 RID: 9220
			public class MASSIVEHEATSINK
			{
				// Token: 0x0400A009 RID: 40969
				public static LocString NAME = UI.FormatAsLink("Anti Entropy Thermo-Nullifier", "MASSIVEHEATSINK");

				// Token: 0x0400A00A RID: 40970
				public static LocString DESC = "";

				// Token: 0x0400A00B RID: 40971
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A self-sustaining machine powered by what appears to be refined ",
					UI.FormatAsLink("Neutronium", "UNOBTANIUM"),
					".\n\nAbsorbs and neutralizes ",
					UI.FormatAsLink("Heat", "HEAT"),
					" energy when provided with piped ",
					UI.FormatAsLink("Hydrogen Gas", "HYDROGEN"),
					"."
				});
			}

			// Token: 0x02002405 RID: 9221
			public class MEGABRAINTANK
			{
				// Token: 0x0400A00C RID: 40972
				public static LocString NAME = UI.FormatAsLink("Somnium Synthesizer", "MEGABRAINTANK");

				// Token: 0x0400A00D RID: 40973
				public static LocString DESC = "";

				// Token: 0x0400A00E RID: 40974
				public static LocString EFFECT = string.Concat(new string[]
				{
					"An organic multi-cortex repository and processing system fuelled by ",
					UI.FormatAsLink("Oxygen", "OXYGEN"),
					".\n\nAnalyzes ",
					UI.FormatAsLink("Dream Journals", "DREAMJOURNAL"),
					" produced by Duplicants wearing ",
					UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS"),
					".\n\nProvides a sustainable boost to Duplicant skills and abilities throughout the colony."
				});
			}

			// Token: 0x02002406 RID: 9222
			public class GRAVITASCREATUREMANIPULATOR
			{
				// Token: 0x0400A00F RID: 40975
				public static LocString NAME = UI.FormatAsLink("Critter Flux-O-Matic", "GRAVITASCREATUREMANIPULATOR");

				// Token: 0x0400A010 RID: 40976
				public static LocString DESC = "";

				// Token: 0x0400A011 RID: 40977
				public static LocString EFFECT = "An experimental DNA manipulator.\n\nAnalyzes " + UI.FormatAsLink("Critters", "CREATURES") + " to transform base morphs into random variants of their species.";
			}

			// Token: 0x02002407 RID: 9223
			public class FACILITYBACKWALLWINDOW
			{
				// Token: 0x0400A012 RID: 40978
				public static LocString NAME = UI.FormatAsLink("Window", "FACILITYBACKWALLWINDOW");

				// Token: 0x0400A013 RID: 40979
				public static LocString DESC = "";

				// Token: 0x0400A014 RID: 40980
				public static LocString EFFECT = "A tall, thin window.";
			}

			// Token: 0x02002408 RID: 9224
			public class POIBUNKEREXTERIORDOOR
			{
				// Token: 0x0400A015 RID: 40981
				public static LocString NAME = UI.FormatAsLink("Security Door", "POIBUNKEREXTERIORDOOR");

				// Token: 0x0400A016 RID: 40982
				public static LocString EFFECT = "A strong door with a sophisticated genetic lock.";

				// Token: 0x0400A017 RID: 40983
				public static LocString DESC = "";
			}

			// Token: 0x02002409 RID: 9225
			public class POIDOORINTERNAL
			{
				// Token: 0x0400A018 RID: 40984
				public static LocString NAME = UI.FormatAsLink("Security Door", "POIDOORINTERNAL");

				// Token: 0x0400A019 RID: 40985
				public static LocString EFFECT = "A strong door with a sophisticated genetic lock.";

				// Token: 0x0400A01A RID: 40986
				public static LocString DESC = "";
			}

			// Token: 0x0200240A RID: 9226
			public class POIFACILITYDOOR
			{
				// Token: 0x0400A01B RID: 40987
				public static LocString NAME = UI.FormatAsLink("Lobby Doors", "FACILITYDOOR");

				// Token: 0x0400A01C RID: 40988
				public static LocString EFFECT = "Large double doors that were once the main entrance to a large facility.";

				// Token: 0x0400A01D RID: 40989
				public static LocString DESC = "";
			}

			// Token: 0x0200240B RID: 9227
			public class VENDINGMACHINE
			{
				// Token: 0x0400A01E RID: 40990
				public static LocString NAME = "Vending Machine";

				// Token: 0x0400A01F RID: 40991
				public static LocString DESC = "A pristine " + UI.FormatAsLink("Nutrient Bar", "FIELDRATION") + " dispenser.";
			}

			// Token: 0x0200240C RID: 9228
			public class GENESHUFFLER
			{
				// Token: 0x0400A020 RID: 40992
				public static LocString NAME = "Neural Vacillator";

				// Token: 0x0400A021 RID: 40993
				public static LocString DESC = "A massive synthetic brain, suspended in saline solution.\n\nThere is a chair attached to the device with room for one person.";
			}

			// Token: 0x0200240D RID: 9229
			public class PROPTALLPLANT
			{
				// Token: 0x0400A022 RID: 40994
				public static LocString NAME = "Potted Plant";

				// Token: 0x0400A023 RID: 40995
				public static LocString DESC = "Looking closely, it appears to be fake.";
			}

			// Token: 0x0200240E RID: 9230
			public class PROPTABLE
			{
				// Token: 0x0400A024 RID: 40996
				public static LocString NAME = "Table";

				// Token: 0x0400A025 RID: 40997
				public static LocString DESC = "A table and some chairs.";
			}

			// Token: 0x0200240F RID: 9231
			public class PROPDESK
			{
				// Token: 0x0400A026 RID: 40998
				public static LocString NAME = "Computer Desk";

				// Token: 0x0400A027 RID: 40999
				public static LocString DESC = "An intact office desk, decorated with several personal belongings and a barely functioning computer.";
			}

			// Token: 0x02002410 RID: 9232
			public class PROPFACILITYCHAIR
			{
				// Token: 0x0400A028 RID: 41000
				public static LocString NAME = "Lobby Chair";

				// Token: 0x0400A029 RID: 41001
				public static LocString DESC = "A chair where visitors can comfortably wait before their appointments.";
			}

			// Token: 0x02002411 RID: 9233
			public class PROPFACILITYCOUCH
			{
				// Token: 0x0400A02A RID: 41002
				public static LocString NAME = "Lobby Couch";

				// Token: 0x0400A02B RID: 41003
				public static LocString DESC = "A couch where visitors can comfortably wait before their appointments.";
			}

			// Token: 0x02002412 RID: 9234
			public class PROPFACILITYDESK
			{
				// Token: 0x0400A02C RID: 41004
				public static LocString NAME = "Director's Desk";

				// Token: 0x0400A02D RID: 41005
				public static LocString DESC = "A spotless desk filled with impeccably organized office supplies.\n\nA photo peeks out from beneath the desk pad, depicting two beaming young women in caps and gowns.\n\nThe photo is quite old.";
			}

			// Token: 0x02002413 RID: 9235
			public class PROPFACILITYTABLE
			{
				// Token: 0x0400A02E RID: 41006
				public static LocString NAME = "Coffee Table";

				// Token: 0x0400A02F RID: 41007
				public static LocString DESC = "A low coffee table that may have once held old science magazines.";
			}

			// Token: 0x02002414 RID: 9236
			public class PROPFACILITYSTATUE
			{
				// Token: 0x0400A030 RID: 41008
				public static LocString NAME = "Gravitas Monument";

				// Token: 0x0400A031 RID: 41009
				public static LocString DESC = "A large, modern sculpture that sits in the center of the lobby.\n\nIt's an artistic cross between an hourglass shape and a double helix.";
			}

			// Token: 0x02002415 RID: 9237
			public class PROPFACILITYCHANDELIER
			{
				// Token: 0x0400A032 RID: 41010
				public static LocString NAME = "Chandelier";

				// Token: 0x0400A033 RID: 41011
				public static LocString DESC = "A large chandelier that hangs from the ceiling.\n\nIt does not appear to function.";
			}

			// Token: 0x02002416 RID: 9238
			public class PROPFACILITYGLOBEDROORS
			{
				// Token: 0x0400A034 RID: 41012
				public static LocString NAME = "Filing Cabinet";

				// Token: 0x0400A035 RID: 41013
				public static LocString DESC = "A filing cabinet for storing hard copy employee records.\n\nThe contents have been shredded.";
			}

			// Token: 0x02002417 RID: 9239
			public class PROPFACILITYDISPLAY1
			{
				// Token: 0x0400A036 RID: 41014
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400A037 RID: 41015
				public static LocString DESC = "An electronic display projecting the blueprint of a familiar device.\n\nIt looks like a Printing Pod.";
			}

			// Token: 0x02002418 RID: 9240
			public class PROPFACILITYDISPLAY2
			{
				// Token: 0x0400A038 RID: 41016
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400A039 RID: 41017
				public static LocString DESC = "An electronic display projecting the blueprint of a familiar device.\n\nIt looks like a Mining Gun.";
			}

			// Token: 0x02002419 RID: 9241
			public class PROPFACILITYDISPLAY3
			{
				// Token: 0x0400A03A RID: 41018
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400A03B RID: 41019
				public static LocString DESC = "An electronic display projecting the blueprint of a strange device.\n\nPerhaps these displays were used to entice visitors.";
			}

			// Token: 0x0200241A RID: 9242
			public class PROPFACILITYTALLPLANT
			{
				// Token: 0x0400A03C RID: 41020
				public static LocString NAME = "Office Plant";

				// Token: 0x0400A03D RID: 41021
				public static LocString DESC = "It's survived the vacuum of space by virtue of being plastic.";
			}

			// Token: 0x0200241B RID: 9243
			public class PROPFACILITYLAMP
			{
				// Token: 0x0400A03E RID: 41022
				public static LocString NAME = "Light Fixture";

				// Token: 0x0400A03F RID: 41023
				public static LocString DESC = "A long light fixture that hangs from the ceiling.\n\nIt does not appear to function.";
			}

			// Token: 0x0200241C RID: 9244
			public class PROPFACILITYWALLDEGREE
			{
				// Token: 0x0400A040 RID: 41024
				public static LocString NAME = "Doctorate Degree";

				// Token: 0x0400A041 RID: 41025
				public static LocString DESC = "Certification in Applied Physics, awarded in recognition of one \"Jacquelyn A. Stern\".";
			}

			// Token: 0x0200241D RID: 9245
			public class PROPFACILITYPAINTING
			{
				// Token: 0x0400A042 RID: 41026
				public static LocString NAME = "Landscape Portrait";

				// Token: 0x0400A043 RID: 41027
				public static LocString DESC = "A painting featuring a copse of fir trees and a magnificent mountain range on the horizon.\n\nThe air in the room prickles with the sensation that I'm not meant to be here.";
			}

			// Token: 0x0200241E RID: 9246
			public class PROPRECEPTIONDESK
			{
				// Token: 0x0400A044 RID: 41028
				public static LocString NAME = "Reception Desk";

				// Token: 0x0400A045 RID: 41029
				public static LocString DESC = "A full coffee cup and a note abandoned mid sentence sit behind the desk.\n\nIt gives me an eerie feeling, as if the receptionist has stepped out and will return any moment.";
			}

			// Token: 0x0200241F RID: 9247
			public class PROPELEVATOR
			{
				// Token: 0x0400A046 RID: 41030
				public static LocString NAME = "Broken Elevator";

				// Token: 0x0400A047 RID: 41031
				public static LocString DESC = "Out of service.\n\nThe buttons inside indicate it went down more than a dozen floors at one point in time.";
			}

			// Token: 0x02002420 RID: 9248
			public class SETLOCKER
			{
				// Token: 0x0400A048 RID: 41032
				public static LocString NAME = "Locker";

				// Token: 0x0400A049 RID: 41033
				public static LocString DESC = "A basic metal locker.\n\nIt contains an assortment of personal effects.";
			}

			// Token: 0x02002421 RID: 9249
			public class PROPLIGHT
			{
				// Token: 0x0400A04A RID: 41034
				public static LocString NAME = "Light Fixture";

				// Token: 0x0400A04B RID: 41035
				public static LocString DESC = "An elegant ceiling lamp, slightly worse for wear.";
			}

			// Token: 0x02002422 RID: 9250
			public class PROPLADDER
			{
				// Token: 0x0400A04C RID: 41036
				public static LocString NAME = "Ladder";

				// Token: 0x0400A04D RID: 41037
				public static LocString DESC = "A hard plastic ladder.";
			}

			// Token: 0x02002423 RID: 9251
			public class PROPSKELETON
			{
				// Token: 0x0400A04E RID: 41038
				public static LocString NAME = "Model Skeleton";

				// Token: 0x0400A04F RID: 41039
				public static LocString DESC = "A detailed anatomical model.\n\nIt appears to be made of resin.";
			}

			// Token: 0x02002424 RID: 9252
			public class PROPSURFACESATELLITE1
			{
				// Token: 0x0400A050 RID: 41040
				public static LocString NAME = "Crashed Satellite";

				// Token: 0x0400A051 RID: 41041
				public static LocString DESC = "All that remains of a once peacefully orbiting satellite.";
			}

			// Token: 0x02002425 RID: 9253
			public class PROPSURFACESATELLITE2
			{
				// Token: 0x0400A052 RID: 41042
				public static LocString NAME = "Wrecked Satellite";

				// Token: 0x0400A053 RID: 41043
				public static LocString DESC = "All that remains of a once peacefully orbiting satellite.";
			}

			// Token: 0x02002426 RID: 9254
			public class PROPSURFACESATELLITE3
			{
				// Token: 0x0400A054 RID: 41044
				public static LocString NAME = "Crushed Satellite";

				// Token: 0x0400A055 RID: 41045
				public static LocString DESC = "All that remains of a once peacefully orbiting satellite.";
			}

			// Token: 0x02002427 RID: 9255
			public class PROPCLOCK
			{
				// Token: 0x0400A056 RID: 41046
				public static LocString NAME = "Clock";

				// Token: 0x0400A057 RID: 41047
				public static LocString DESC = "A simple wall clock.\n\nIt is no longer ticking.";
			}

			// Token: 0x02002428 RID: 9256
			public class PROPGRAVITASDECORATIVEWINDOW
			{
				// Token: 0x0400A058 RID: 41048
				public static LocString NAME = "Window";

				// Token: 0x0400A059 RID: 41049
				public static LocString DESC = "A tall, thin window which once pointed to a courtyard.";
			}

			// Token: 0x02002429 RID: 9257
			public class PROPGRAVITASLABWINDOW
			{
				// Token: 0x0400A05A RID: 41050
				public static LocString NAME = "Lab Window";

				// Token: 0x0400A05B RID: 41051
				public static LocString DESC = "";

				// Token: 0x0400A05C RID: 41052
				public static LocString EFFECT = "A lab window. Formerly a portal to the outside world.";
			}

			// Token: 0x0200242A RID: 9258
			public class PROPGRAVITASLABWINDOWHORIZONTAL
			{
				// Token: 0x0400A05D RID: 41053
				public static LocString NAME = "Lab Window";

				// Token: 0x0400A05E RID: 41054
				public static LocString DESC = "";

				// Token: 0x0400A05F RID: 41055
				public static LocString EFFECT = "A lab window.\n\nSomeone once stared out of this, contemplating the results of an experiment.";
			}

			// Token: 0x0200242B RID: 9259
			public class PROPGRAVITASLABWALL
			{
				// Token: 0x0400A060 RID: 41056
				public static LocString NAME = "Lab Wall";

				// Token: 0x0400A061 RID: 41057
				public static LocString DESC = "";

				// Token: 0x0400A062 RID: 41058
				public static LocString EFFECT = "A regular wall that once existed in a working lab.";
			}

			// Token: 0x0200242C RID: 9260
			public class GRAVITASCONTAINER
			{
				// Token: 0x0400A063 RID: 41059
				public static LocString NAME = "Pajama Cubby";

				// Token: 0x0400A064 RID: 41060
				public static LocString DESC = "";

				// Token: 0x0400A065 RID: 41061
				public static LocString EFFECT = "A clothing storage unit.\n\nIt contains ultra-soft sleepwear.";
			}

			// Token: 0x0200242D RID: 9261
			public class GRAVITASLABLIGHT
			{
				// Token: 0x0400A066 RID: 41062
				public static LocString NAME = "LED Light";

				// Token: 0x0400A067 RID: 41063
				public static LocString DESC = "";

				// Token: 0x0400A068 RID: 41064
				public static LocString EFFECT = "An overhead light therapy lamp designed to soothe the minds.";
			}

			// Token: 0x0200242E RID: 9262
			public class GRAVITASDOOR
			{
				// Token: 0x0400A069 RID: 41065
				public static LocString NAME = "Gravitas Door";

				// Token: 0x0400A06A RID: 41066
				public static LocString DESC = "";

				// Token: 0x0400A06B RID: 41067
				public static LocString EFFECT = "An office door to an office that no longer exists.";
			}

			// Token: 0x0200242F RID: 9263
			public class PROPGRAVITASWALL
			{
				// Token: 0x0400A06C RID: 41068
				public static LocString NAME = "Wall";

				// Token: 0x0400A06D RID: 41069
				public static LocString DESC = "";

				// Token: 0x0400A06E RID: 41070
				public static LocString EFFECT = "The wall of a once-great scientific facility.";
			}

			// Token: 0x02002430 RID: 9264
			public class PROPGRAVITASDISPLAY4
			{
				// Token: 0x0400A06F RID: 41071
				public static LocString NAME = "Electronic Display";

				// Token: 0x0400A070 RID: 41072
				public static LocString DESC = "An electronic display projecting the blueprint of a robotic device.\n\nIt looks like a ceiling robot.";
			}

			// Token: 0x02002431 RID: 9265
			public class PROPGRAVITASCEILINGROBOT
			{
				// Token: 0x0400A071 RID: 41073
				public static LocString NAME = "Ceiling Robot";

				// Token: 0x0400A072 RID: 41074
				public static LocString DESC = "Non-functioning robotic arms that once assisted lab technicians.";
			}

			// Token: 0x02002432 RID: 9266
			public class PROPGRAVITASFLOORROBOT
			{
				// Token: 0x0400A073 RID: 41075
				public static LocString NAME = "Robotic Arm";

				// Token: 0x0400A074 RID: 41076
				public static LocString DESC = "The grasping robotic claw designed to assist technicians in a lab.";
			}

			// Token: 0x02002433 RID: 9267
			public class PROPGRAVITASJAR1
			{
				// Token: 0x0400A075 RID: 41077
				public static LocString NAME = "Big Brain Jar";

				// Token: 0x0400A076 RID: 41078
				public static LocString DESC = "An abnormally large brain floating in embalming liquid to prevent decomposition.";
			}

			// Token: 0x02002434 RID: 9268
			public class PROPGRAVITASCREATUREPOSTER
			{
				// Token: 0x0400A077 RID: 41079
				public static LocString NAME = "Anatomy Poster";

				// Token: 0x0400A078 RID: 41080
				public static LocString DESC = "An anatomical illustration of the very first " + UI.FormatAsLink("Hatch", "HATCH") + " ever produced.\n\nWhile the ratio of egg sac to brain may appear outlandish, it is in fact to scale.";
			}

			// Token: 0x02002435 RID: 9269
			public class PROPGRAVITASDESKPODIUM
			{
				// Token: 0x0400A079 RID: 41081
				public static LocString NAME = "Computer Podium";

				// Token: 0x0400A07A RID: 41082
				public static LocString DESC = "A clutter-proof desk to minimize distractions.\n\nThere appears to be something stored in the computer.";
			}

			// Token: 0x02002436 RID: 9270
			public class PROPGRAVITASFIRSTAIDKIT
			{
				// Token: 0x0400A07B RID: 41083
				public static LocString NAME = "First Aid Kit";

				// Token: 0x0400A07C RID: 41084
				public static LocString DESC = "It looks like it's been used a lot.";
			}

			// Token: 0x02002437 RID: 9271
			public class PROPGRAVITASHANDSCANNER
			{
				// Token: 0x0400A07D RID: 41085
				public static LocString NAME = "Hand Scanner";

				// Token: 0x0400A07E RID: 41086
				public static LocString DESC = "A sophisticated security device.\n\nIt appears to use a method other than fingerprints to verify an individual's identity.";
			}

			// Token: 0x02002438 RID: 9272
			public class PROPGRAVITASLABTABLE
			{
				// Token: 0x0400A07F RID: 41087
				public static LocString NAME = "Lab Desk";

				// Token: 0x0400A080 RID: 41088
				public static LocString DESC = "The quaint research desk of a departed lab technician.\n\nPerhaps the computer stores something of interest.";
			}

			// Token: 0x02002439 RID: 9273
			public class PROPGRAVITASROBTICTABLE
			{
				// Token: 0x0400A081 RID: 41089
				public static LocString NAME = "Robotics Research Desk";

				// Token: 0x0400A082 RID: 41090
				public static LocString DESC = "The work space of an extinct robotics technician who left behind some unfinished prototypes.";
			}

			// Token: 0x0200243A RID: 9274
			public class PROPGRAVITASSHELF
			{
				// Token: 0x0400A083 RID: 41091
				public static LocString NAME = "Shelf";

				// Token: 0x0400A084 RID: 41092
				public static LocString DESC = "A shelf holding jars just out of reach for a short person.";
			}

			// Token: 0x0200243B RID: 9275
			public class PROPGRAVITASJAR2
			{
				// Token: 0x0400A085 RID: 41093
				public static LocString NAME = "Sample Jar";

				// Token: 0x0400A086 RID: 41094
				public static LocString DESC = "The corpse of a proto-hatch creature meticulously preserved in a jar.";
			}

			// Token: 0x0200243C RID: 9276
			public class WARPCONDUITRECEIVER
			{
				// Token: 0x0400A087 RID: 41095
				public static LocString NAME = "Supply Teleporter Output";

				// Token: 0x0400A088 RID: 41096
				public static LocString DESC = "The tubes at the back disappear into nowhere.";

				// Token: 0x0400A089 RID: 41097
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A machine capable of teleporting ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources to another asteroid.\n\nIt can be activated by a Duplicant with the ",
					UI.FormatAsLink("Field Research", "RESEARCHING2"),
					" skill.\n\nThis is the receiving side."
				});
			}

			// Token: 0x0200243D RID: 9277
			public class WARPCONDUITSENDER
			{
				// Token: 0x0400A08A RID: 41098
				public static LocString NAME = "Supply Teleporter Input";

				// Token: 0x0400A08B RID: 41099
				public static LocString DESC = "The tubes at the back disappear into nowhere.";

				// Token: 0x0400A08C RID: 41100
				public static LocString EFFECT = string.Concat(new string[]
				{
					"A machine capable of teleporting ",
					UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
					", ",
					UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
					", and ",
					UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
					" resources to another asteroid.\n\nIt can be activated by a Duplicant with the ",
					UI.FormatAsLink("Field Research", "RESEARCHING2"),
					" skill.\n\nThis is the transmitting side."
				});
			}

			// Token: 0x0200243E RID: 9278
			public class WARPPORTAL
			{
				// Token: 0x0400A08D RID: 41101
				public static LocString NAME = "Teleporter Transmitter";

				// Token: 0x0400A08E RID: 41102
				public static LocString DESC = "The functional remnants of an intricate teleportation system.\n\nThis is the outgoing side, and has one pre-programmed destination.";
			}

			// Token: 0x0200243F RID: 9279
			public class WARPRECEIVER
			{
				// Token: 0x0400A08F RID: 41103
				public static LocString NAME = "Teleporter Receiver";

				// Token: 0x0400A090 RID: 41104
				public static LocString DESC = "The functional remnants of an intricate teleportation system.\n\nThis is the incoming side.";
			}

			// Token: 0x02002440 RID: 9280
			public class TEMPORALTEAROPENER
			{
				// Token: 0x0400A091 RID: 41105
				public static LocString NAME = "Temporal Tear Opener";

				// Token: 0x0400A092 RID: 41106
				public static LocString DESC = "Infinite possibilities, with a complimentary side of meteor showers.";

				// Token: 0x0400A093 RID: 41107
				public static LocString EFFECT = "A powerful mechanism capable of tearing through the fabric of reality.";

				// Token: 0x02002F8C RID: 12172
				public class SIDESCREEN
				{
					// Token: 0x0400C1CB RID: 49611
					public static LocString TEXT = "Fire!";

					// Token: 0x0400C1CC RID: 49612
					public static LocString TOOLTIP = "The big red button.";
				}
			}

			// Token: 0x02002441 RID: 9281
			public class LONELYMINIONHOUSE
			{
				// Token: 0x0400A094 RID: 41108
				public static LocString NAME = UI.FormatAsLink("Gravitas Shipping Container", "LONELYMINIONHOUSE");

				// Token: 0x0400A095 RID: 41109
				public static LocString DESC = "Its occupant has been alone for so long, he's forgotten what friendship feels like.";

				// Token: 0x0400A096 RID: 41110
				public static LocString EFFECT = "A large transport unit from the facility's sub-sub-basement.\n\nIt has been modified into a crude yet functional temporary shelter.";
			}

			// Token: 0x02002442 RID: 9282
			public class LONELYMINIONHOUSE_COMPLETE
			{
				// Token: 0x0400A097 RID: 41111
				public static LocString NAME = UI.FormatAsLink("Gravitas Shipping Container", "LONELYMINIONHOUSE_COMPLETE");

				// Token: 0x0400A098 RID: 41112
				public static LocString DESC = "Someone lived inside it for a while.";

				// Token: 0x0400A099 RID: 41113
				public static LocString EFFECT = "A super-spacious container for the " + UI.FormatAsLink("Solid Materials", "ELEMENTS_SOLID") + " of your choosing.";
			}

			// Token: 0x02002443 RID: 9283
			public class LONELYMAILBOX
			{
				// Token: 0x0400A09A RID: 41114
				public static LocString NAME = UI.FormatAsLink("Mailbox", "LONELYMAILBOX");

				// Token: 0x0400A09B RID: 41115
				public static LocString DESC = "There's nothing quite like receiving homemade gifts in the mail.";

				// Token: 0x0400A09C RID: 41116
				public static LocString EFFECT = "Displays a single edible object.";
			}

			// Token: 0x02002444 RID: 9284
			public class CHLORINATOR
			{
				// Token: 0x0400A09D RID: 41117
				public static LocString NAME = UI.FormatAsLink("Bleach Stone Hopper", "CHLORINATOR");

				// Token: 0x0400A09E RID: 41118
				public static LocString DESC = "Bleach stone is useful for sanitation and geotuning.";

				// Token: 0x0400A09F RID: 41119
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Uses ",
					ELEMENTS.SALT.NAME,
					" and ",
					ELEMENTS.GOLD.NAME,
					" to produce ",
					ELEMENTS.BLEACHSTONE.NAME,
					"."
				});
			}

			// Token: 0x02002445 RID: 9285
			public class MILKPRESS
			{
				// Token: 0x0400A0A0 RID: 41120
				public static LocString NAME = UI.FormatAsLink("Plant Pulverizer", "MILKPRESS");

				// Token: 0x0400A0A1 RID: 41121
				public static LocString DESC = "For Duplicants who are too squeamish to milk critters.";

				// Token: 0x0400A0A2 RID: 41122
				public static LocString EFFECT = string.Concat(new string[]
				{
					"Crushes ",
					CREATURES.SPECIES.SEEDS.COLDWHEAT.NAME,
					" or ",
					ITEMS.FOOD.SPICENUT.NAME,
					" to extract ",
					ELEMENTS.MILK.NAME,
					".\n\n",
					ELEMENTS.MILK.NAME,
					" can be used to refill the ",
					BUILDINGS.PREFABS.MILKFEEDER.NAME,
					"."
				});

				// Token: 0x0400A0A3 RID: 41123
				public static LocString WHEAT_MILK_RECIPE_DESCRIPTION = "Converts {0} to {1}";

				// Token: 0x0400A0A4 RID: 41124
				public static LocString NUT_MILK_RECIPE_DESCRIPTION = "Converts {0} to {1}";
			}
		}

		// Token: 0x02001CCE RID: 7374
		public static class DAMAGESOURCES
		{
			// Token: 0x04008349 RID: 33609
			public static LocString NOTIFICATION_TOOLTIP = "A {0} sustained damage from {1}";

			// Token: 0x0400834A RID: 33610
			public static LocString CONDUIT_CONTENTS_FROZE = "pipe contents becoming too cold";

			// Token: 0x0400834B RID: 33611
			public static LocString CONDUIT_CONTENTS_BOILED = "pipe contents becoming too hot";

			// Token: 0x0400834C RID: 33612
			public static LocString BUILDING_OVERHEATED = "overheating";

			// Token: 0x0400834D RID: 33613
			public static LocString CORROSIVE_ELEMENT = "corrosive element";

			// Token: 0x0400834E RID: 33614
			public static LocString BAD_INPUT_ELEMENT = "receiving an incorrect substance";

			// Token: 0x0400834F RID: 33615
			public static LocString MINION_DESTRUCTION = "an angry Duplicant. Rude!";

			// Token: 0x04008350 RID: 33616
			public static LocString LIQUID_PRESSURE = "neighboring liquid pressure";

			// Token: 0x04008351 RID: 33617
			public static LocString CIRCUIT_OVERLOADED = "an overloaded circuit";

			// Token: 0x04008352 RID: 33618
			public static LocString LOGIC_CIRCUIT_OVERLOADED = "an overloaded logic circuit";

			// Token: 0x04008353 RID: 33619
			public static LocString MICROMETEORITE = "micrometeorite";

			// Token: 0x04008354 RID: 33620
			public static LocString COMET = "falling space rocks";

			// Token: 0x04008355 RID: 33621
			public static LocString ROCKET = "rocket engine";
		}

		// Token: 0x02001CCF RID: 7375
		public static class AUTODISINFECTABLE
		{
			// Token: 0x02002446 RID: 9286
			public static class ENABLE_AUTODISINFECT
			{
				// Token: 0x0400A0A5 RID: 41125
				public static LocString NAME = "Enable Disinfect";

				// Token: 0x0400A0A6 RID: 41126
				public static LocString TOOLTIP = "Automatically disinfect this building when it becomes contaminated";
			}

			// Token: 0x02002447 RID: 9287
			public static class DISABLE_AUTODISINFECT
			{
				// Token: 0x0400A0A7 RID: 41127
				public static LocString NAME = "Disable Disinfect";

				// Token: 0x0400A0A8 RID: 41128
				public static LocString TOOLTIP = "Do not automatically disinfect this building";
			}

			// Token: 0x02002448 RID: 9288
			public static class NO_DISEASE
			{
				// Token: 0x0400A0A9 RID: 41129
				public static LocString TOOLTIP = "This building is clean";
			}
		}

		// Token: 0x02001CD0 RID: 7376
		public static class DISINFECTABLE
		{
			// Token: 0x02002449 RID: 9289
			public static class ENABLE_DISINFECT
			{
				// Token: 0x0400A0AA RID: 41130
				public static LocString NAME = "Disinfect";

				// Token: 0x0400A0AB RID: 41131
				public static LocString TOOLTIP = "Mark this building for disinfection";
			}

			// Token: 0x0200244A RID: 9290
			public static class DISABLE_DISINFECT
			{
				// Token: 0x0400A0AC RID: 41132
				public static LocString NAME = "Cancel Disinfect";

				// Token: 0x0400A0AD RID: 41133
				public static LocString TOOLTIP = "Cancel this disinfect order";
			}

			// Token: 0x0200244B RID: 9291
			public static class NO_DISEASE
			{
				// Token: 0x0400A0AE RID: 41134
				public static LocString TOOLTIP = "This building is already clean";
			}
		}

		// Token: 0x02001CD1 RID: 7377
		public static class REPAIRABLE
		{
			// Token: 0x0200244C RID: 9292
			public static class ENABLE_AUTOREPAIR
			{
				// Token: 0x0400A0AF RID: 41135
				public static LocString NAME = "Enable Autorepair";

				// Token: 0x0400A0B0 RID: 41136
				public static LocString TOOLTIP = "Automatically repair this building when damaged";
			}

			// Token: 0x0200244D RID: 9293
			public static class DISABLE_AUTOREPAIR
			{
				// Token: 0x0400A0B1 RID: 41137
				public static LocString NAME = "Disable Autorepair";

				// Token: 0x0400A0B2 RID: 41138
				public static LocString TOOLTIP = "Only repair this building when ordered";
			}
		}

		// Token: 0x02001CD2 RID: 7378
		public static class AUTOMATABLE
		{
			// Token: 0x0200244E RID: 9294
			public static class ENABLE_AUTOMATIONONLY
			{
				// Token: 0x0400A0B3 RID: 41139
				public static LocString NAME = "Disable Manual";

				// Token: 0x0400A0B4 RID: 41140
				public static LocString TOOLTIP = "This building's storage may be accessed by Auto-Sweepers only\n\nDuplicants will not be permitted to add or remove materials from this building";
			}

			// Token: 0x0200244F RID: 9295
			public static class DISABLE_AUTOMATIONONLY
			{
				// Token: 0x0400A0B5 RID: 41141
				public static LocString NAME = "Enable Manual";

				// Token: 0x0400A0B6 RID: 41142
				public static LocString TOOLTIP = "This building's storage may be accessed by both Duplicants and Auto-Sweeper buildings";
			}
		}
	}
}
