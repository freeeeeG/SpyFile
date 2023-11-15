using System;

namespace STRINGS
{
	// Token: 0x02000DAC RID: 3500
	public class BUILDING
	{
		// Token: 0x02001DDF RID: 7647
		public class STATUSITEMS
		{
			// Token: 0x020027D0 RID: 10192
			public class SPECIALCARGOBAYCLUSTERCRITTERSTORED
			{
				// Token: 0x0400AE80 RID: 44672
				public static LocString NAME = "Contents: {0}";

				// Token: 0x0400AE81 RID: 44673
				public static LocString TOOLTIP = "";
			}

			// Token: 0x020027D1 RID: 10193
			public class GEOTUNER_NEEDGEYSER
			{
				// Token: 0x0400AE82 RID: 44674
				public static LocString NAME = "No Geyser Selected";

				// Token: 0x0400AE83 RID: 44675
				public static LocString TOOLTIP = "Select an analyzed geyser to increase its output";
			}

			// Token: 0x020027D2 RID: 10194
			public class GEOTUNER_CHARGE_REQUIRED
			{
				// Token: 0x0400AE84 RID: 44676
				public static LocString NAME = "Experimentation Needed";

				// Token: 0x0400AE85 RID: 44677
				public static LocString TOOLTIP = "This building requires a Duplicant to produce amplification data through experimentation";
			}

			// Token: 0x020027D3 RID: 10195
			public class GEOTUNER_CHARGING
			{
				// Token: 0x0400AE86 RID: 44678
				public static LocString NAME = "Compiling Data";

				// Token: 0x0400AE87 RID: 44679
				public static LocString TOOLTIP = "Compiling amplification data through experimentation";
			}

			// Token: 0x020027D4 RID: 10196
			public class GEOTUNER_CHARGED
			{
				// Token: 0x0400AE88 RID: 44680
				public static LocString NAME = "Data Remaining: {0}";

				// Token: 0x0400AE89 RID: 44681
				public static LocString TOOLTIP = "This building consumes amplification data while boosting a geyser\n\nTime remaining: {0} ({1} data per second)";
			}

			// Token: 0x020027D5 RID: 10197
			public class GEOTUNER_GEYSER_STATUS
			{
				// Token: 0x0400AE8A RID: 44682
				public static LocString NAME = "";

				// Token: 0x0400AE8B RID: 44683
				public static LocString NAME_ERUPTING = "Target is Erupting";

				// Token: 0x0400AE8C RID: 44684
				public static LocString NAME_DORMANT = "Target is Not Erupting";

				// Token: 0x0400AE8D RID: 44685
				public static LocString NAME_IDLE = "Target is Not Erupting";

				// Token: 0x0400AE8E RID: 44686
				public static LocString TOOLTIP = "";

				// Token: 0x0400AE8F RID: 44687
				public static LocString TOOLTIP_ERUPTING = "The selected geyser is erupting and will receive stored amplification data";

				// Token: 0x0400AE90 RID: 44688
				public static LocString TOOLTIP_DORMANT = "The selected geyser is not erupting\n\nIt will not receive stored amplification data in this state";

				// Token: 0x0400AE91 RID: 44689
				public static LocString TOOLTIP_IDLE = "The selected geyser is not erupting\n\nIt will not receive stored amplification data in this state";
			}

			// Token: 0x020027D6 RID: 10198
			public class GEYSER_GEOTUNED
			{
				// Token: 0x0400AE92 RID: 44690
				public static LocString NAME = "Geotuned ({0}/{1})";

				// Token: 0x0400AE93 RID: 44691
				public static LocString TOOLTIP = "This geyser is being boosted by {0} out {1} of " + UI.PRE_KEYWORD + "Geotuners" + UI.PST_KEYWORD;
			}

			// Token: 0x020027D7 RID: 10199
			public class RADIATOR_ENERGY_CURRENT_EMISSION_RATE
			{
				// Token: 0x0400AE94 RID: 44692
				public static LocString NAME = "Currently Emitting: {ENERGY_RATE}";

				// Token: 0x0400AE95 RID: 44693
				public static LocString TOOLTIP = "Currently Emitting: {ENERGY_RATE}";
			}

			// Token: 0x020027D8 RID: 10200
			public class NOTLINKEDTOHEAD
			{
				// Token: 0x0400AE96 RID: 44694
				public static LocString NAME = "Not Linked";

				// Token: 0x0400AE97 RID: 44695
				public static LocString TOOLTIP = "This building must be built adjacent to a {headBuilding} or another {linkBuilding} in order to function";
			}

			// Token: 0x020027D9 RID: 10201
			public class BAITED
			{
				// Token: 0x0400AE98 RID: 44696
				public static LocString NAME = "{0} Bait";

				// Token: 0x0400AE99 RID: 44697
				public static LocString TOOLTIP = "This lure is baited with {0}\n\nBait material is set during the construction of the building";
			}

			// Token: 0x020027DA RID: 10202
			public class NOCOOLANT
			{
				// Token: 0x0400AE9A RID: 44698
				public static LocString NAME = "No Coolant";

				// Token: 0x0400AE9B RID: 44699
				public static LocString TOOLTIP = "This building needs coolant";
			}

			// Token: 0x020027DB RID: 10203
			public class ANGERDAMAGE
			{
				// Token: 0x0400AE9C RID: 44700
				public static LocString NAME = "Damage: Duplicant Tantrum";

				// Token: 0x0400AE9D RID: 44701
				public static LocString TOOLTIP = "A stressed Duplicant is damaging this building";

				// Token: 0x0400AE9E RID: 44702
				public static LocString NOTIFICATION = "Building Damage: Duplicant Tantrum";

				// Token: 0x0400AE9F RID: 44703
				public static LocString NOTIFICATION_TOOLTIP = "Stressed Duplicants are damaging these buildings:\n\n{0}";
			}

			// Token: 0x020027DC RID: 10204
			public class PIPECONTENTS
			{
				// Token: 0x0400AEA0 RID: 44704
				public static LocString EMPTY = "Empty";

				// Token: 0x0400AEA1 RID: 44705
				public static LocString CONTENTS = "{0} of {1} at {2}";

				// Token: 0x0400AEA2 RID: 44706
				public static LocString CONTENTS_WITH_DISEASE = "\n  {0}";
			}

			// Token: 0x020027DD RID: 10205
			public class CONVEYOR_CONTENTS
			{
				// Token: 0x0400AEA3 RID: 44707
				public static LocString EMPTY = "Empty";

				// Token: 0x0400AEA4 RID: 44708
				public static LocString CONTENTS = "{0} of {1} at {2}";

				// Token: 0x0400AEA5 RID: 44709
				public static LocString CONTENTS_WITH_DISEASE = "\n  {0}";
			}

			// Token: 0x020027DE RID: 10206
			public class ASSIGNEDTO
			{
				// Token: 0x0400AEA6 RID: 44710
				public static LocString NAME = "Assigned to: {Assignee}";

				// Token: 0x0400AEA7 RID: 44711
				public static LocString TOOLTIP = "Only {Assignee} can use this amenity";
			}

			// Token: 0x020027DF RID: 10207
			public class ASSIGNEDPUBLIC
			{
				// Token: 0x0400AEA8 RID: 44712
				public static LocString NAME = "Assigned to: Public";

				// Token: 0x0400AEA9 RID: 44713
				public static LocString TOOLTIP = "Any Duplicant can use this amenity";
			}

			// Token: 0x020027E0 RID: 10208
			public class ASSIGNEDTOROOM
			{
				// Token: 0x0400AEAA RID: 44714
				public static LocString NAME = "Assigned to: {0}";

				// Token: 0x0400AEAB RID: 44715
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Any Duplicant assigned to this ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" can use this amenity"
				});
			}

			// Token: 0x020027E1 RID: 10209
			public class AWAITINGSEEDDELIVERY
			{
				// Token: 0x0400AEAC RID: 44716
				public static LocString NAME = "Awaiting Delivery";

				// Token: 0x0400AEAD RID: 44717
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;
			}

			// Token: 0x020027E2 RID: 10210
			public class AWAITINGBAITDELIVERY
			{
				// Token: 0x0400AEAE RID: 44718
				public static LocString NAME = "Awaiting Bait";

				// Token: 0x0400AEAF RID: 44719
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Bait" + UI.PST_KEYWORD;
			}

			// Token: 0x020027E3 RID: 10211
			public class CLINICOUTSIDEHOSPITAL
			{
				// Token: 0x0400AEB0 RID: 44720
				public static LocString NAME = "Medical building outside Hospital";

				// Token: 0x0400AEB1 RID: 44721
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Rebuild this medical equipment in a ",
					UI.PRE_KEYWORD,
					"Hospital",
					UI.PST_KEYWORD,
					" to more effectively quarantine sick Duplicants"
				});
			}

			// Token: 0x020027E4 RID: 10212
			public class BOTTLE_EMPTIER
			{
				// Token: 0x0200314C RID: 12620
				public static class ALLOWED
				{
					// Token: 0x0400C5D9 RID: 50649
					public static LocString NAME = "Auto-Bottle: On";

					// Token: 0x0400C5DA RID: 50650
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may specifically fetch ",
						UI.PRE_KEYWORD,
						"Liquid",
						UI.PST_KEYWORD,
						" from a ",
						BUILDINGS.PREFABS.LIQUIDPUMPINGSTATION.NAME,
						" to bring to this location"
					});
				}

				// Token: 0x0200314D RID: 12621
				public static class DENIED
				{
					// Token: 0x0400C5DB RID: 50651
					public static LocString NAME = "Auto-Bottle: Off";

					// Token: 0x0400C5DC RID: 50652
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may not specifically fetch ",
						UI.PRE_KEYWORD,
						"Liquid",
						UI.PST_KEYWORD,
						" from a ",
						BUILDINGS.PREFABS.LIQUIDPUMPINGSTATION.NAME,
						" to bring to this location"
					});
				}
			}

			// Token: 0x020027E5 RID: 10213
			public class CANISTER_EMPTIER
			{
				// Token: 0x0200314E RID: 12622
				public static class ALLOWED
				{
					// Token: 0x0400C5DD RID: 50653
					public static LocString NAME = "Auto-Bottle: On";

					// Token: 0x0400C5DE RID: 50654
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may specifically fetch ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" from a ",
						BUILDINGS.PREFABS.GASBOTTLER.NAME,
						" to bring to this location"
					});
				}

				// Token: 0x0200314F RID: 12623
				public static class DENIED
				{
					// Token: 0x0400C5DF RID: 50655
					public static LocString NAME = "Auto-Bottle: Off";

					// Token: 0x0400C5E0 RID: 50656
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may not specifically fetch ",
						UI.PRE_KEYWORD,
						"Gas",
						UI.PST_KEYWORD,
						" from a ",
						BUILDINGS.PREFABS.GASBOTTLER.NAME,
						" to bring to this location"
					});
				}
			}

			// Token: 0x020027E6 RID: 10214
			public class BROKEN
			{
				// Token: 0x0400AEB2 RID: 44722
				public static LocString NAME = "Broken";

				// Token: 0x0400AEB3 RID: 44723
				public static LocString TOOLTIP = "This building received damage from <b>{DamageInfo}</b>\n\nIt will not function until it receives repairs";
			}

			// Token: 0x020027E7 RID: 10215
			public class CHANGEDOORCONTROLSTATE
			{
				// Token: 0x0400AEB4 RID: 44724
				public static LocString NAME = "Pending Door State Change: {ControlState}";

				// Token: 0x0400AEB5 RID: 44725
				public static LocString TOOLTIP = "Waiting for a Duplicant to change control state";
			}

			// Token: 0x020027E8 RID: 10216
			public class DISPENSEREQUESTED
			{
				// Token: 0x0400AEB6 RID: 44726
				public static LocString NAME = "Dispense Requested";

				// Token: 0x0400AEB7 RID: 44727
				public static LocString TOOLTIP = "Waiting for a Duplicant to dispense the item";
			}

			// Token: 0x020027E9 RID: 10217
			public class SUIT_LOCKER
			{
				// Token: 0x02003150 RID: 12624
				public class NEED_CONFIGURATION
				{
					// Token: 0x0400C5E1 RID: 50657
					public static LocString NAME = "Current Status: Needs Configuration";

					// Token: 0x0400C5E2 RID: 50658
					public static LocString TOOLTIP = "Set this dock to store a suit or leave it empty";
				}

				// Token: 0x02003151 RID: 12625
				public class READY
				{
					// Token: 0x0400C5E3 RID: 50659
					public static LocString NAME = "Current Status: Empty";

					// Token: 0x0400C5E4 RID: 50660
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock is ready to receive a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD,
						", either by manual delivery or from a Duplicant returning the suit they're wearing"
					});
				}

				// Token: 0x02003152 RID: 12626
				public class SUIT_REQUESTED
				{
					// Token: 0x0400C5E5 RID: 50661
					public static LocString NAME = "Current Status: Awaiting Delivery";

					// Token: 0x0400C5E6 RID: 50662
					public static LocString TOOLTIP = "Waiting for a Duplicant to deliver a " + UI.PRE_KEYWORD + "Suit" + UI.PST_KEYWORD;
				}

				// Token: 0x02003153 RID: 12627
				public class CHARGING
				{
					// Token: 0x0400C5E7 RID: 50663
					public static LocString NAME = "Current Status: Charging Suit";

					// Token: 0x0400C5E8 RID: 50664
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD,
						" is docked and refueling"
					});
				}

				// Token: 0x02003154 RID: 12628
				public class NO_OXYGEN
				{
					// Token: 0x0400C5E9 RID: 50665
					public static LocString NAME = "Current Status: No Oxygen";

					// Token: 0x0400C5EA RID: 50666
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.OXYGEN.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x02003155 RID: 12629
				public class NO_FUEL
				{
					// Token: 0x0400C5EB RID: 50667
					public static LocString NAME = "Current Status: No Fuel";

					// Token: 0x0400C5EC RID: 50668
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.PETROLEUM.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x02003156 RID: 12630
				public class NO_COOLANT
				{
					// Token: 0x0400C5ED RID: 50669
					public static LocString NAME = "Current Status: No Coolant";

					// Token: 0x0400C5EE RID: 50670
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This dock does not contain enough ",
						ELEMENTS.WATER.NAME,
						" to refill a ",
						UI.PRE_KEYWORD,
						"Suit",
						UI.PST_KEYWORD
					});
				}

				// Token: 0x02003157 RID: 12631
				public class NOT_OPERATIONAL
				{
					// Token: 0x0400C5EF RID: 50671
					public static LocString NAME = "Current Status: Offline";

					// Token: 0x0400C5F0 RID: 50672
					public static LocString TOOLTIP = "This dock requires " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
				}

				// Token: 0x02003158 RID: 12632
				public class FULLY_CHARGED
				{
					// Token: 0x0400C5F1 RID: 50673
					public static LocString NAME = "Current Status: Full Fueled";

					// Token: 0x0400C5F2 RID: 50674
					public static LocString TOOLTIP = "This suit is fully refueled and ready for use";
				}
			}

			// Token: 0x020027EA RID: 10218
			public class SUITMARKERTRAVERSALONLYWHENROOMAVAILABLE
			{
				// Token: 0x0400AEB8 RID: 44728
				public static LocString NAME = "Clearance: Vacancy Only";

				// Token: 0x0400AEB9 RID: 44729
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Suited Duplicants may pass only if there is room in a ",
					UI.PRE_KEYWORD,
					"Dock",
					UI.PST_KEYWORD,
					" to store their ",
					UI.PRE_KEYWORD,
					"Suit",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020027EB RID: 10219
			public class SUITMARKERTRAVERSALANYTIME
			{
				// Token: 0x0400AEBA RID: 44730
				public static LocString NAME = "Clearance: Always Permitted";

				// Token: 0x0400AEBB RID: 44731
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Suited Duplicants may pass even if there is no room to store their ",
					UI.PRE_KEYWORD,
					"Suits",
					UI.PST_KEYWORD,
					"\n\nWhen all available docks are full, Duplicants will unequip their ",
					UI.PRE_KEYWORD,
					"Suits",
					UI.PST_KEYWORD,
					" and drop them on the floor"
				});
			}

			// Token: 0x020027EC RID: 10220
			public class SUIT_LOCKER_NEEDS_CONFIGURATION
			{
				// Token: 0x0400AEBC RID: 44732
				public static LocString NAME = "Not Configured";

				// Token: 0x0400AEBD RID: 44733
				public static LocString TOOLTIP = "Dock settings not configured";
			}

			// Token: 0x020027ED RID: 10221
			public class CURRENTDOORCONTROLSTATE
			{
				// Token: 0x0400AEBE RID: 44734
				public static LocString NAME = "Current State: {ControlState}";

				// Token: 0x0400AEBF RID: 44735
				public static LocString TOOLTIP = "Current State: {ControlState}\n\nAuto: Duplicants open and close this door as needed\nLocked: Nothing may pass through\nOpen: This door will remain open";

				// Token: 0x0400AEC0 RID: 44736
				public static LocString OPENED = "Opened";

				// Token: 0x0400AEC1 RID: 44737
				public static LocString AUTO = "Auto";

				// Token: 0x0400AEC2 RID: 44738
				public static LocString LOCKED = "Locked";
			}

			// Token: 0x020027EE RID: 10222
			public class CONDUITBLOCKED
			{
				// Token: 0x0400AEC3 RID: 44739
				public static LocString NAME = "Pipe Blocked";

				// Token: 0x0400AEC4 RID: 44740
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x020027EF RID: 10223
			public class OUTPUTTILEBLOCKED
			{
				// Token: 0x0400AEC5 RID: 44741
				public static LocString NAME = "Output Blocked";

				// Token: 0x0400AEC6 RID: 44742
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x020027F0 RID: 10224
			public class CONDUITBLOCKEDMULTIPLES
			{
				// Token: 0x0400AEC7 RID: 44743
				public static LocString NAME = "Pipe Blocked";

				// Token: 0x0400AEC8 RID: 44744
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x020027F1 RID: 10225
			public class SOLIDCONDUITBLOCKEDMULTIPLES
			{
				// Token: 0x0400AEC9 RID: 44745
				public static LocString NAME = "Conveyor Rail Blocked";

				// Token: 0x0400AECA RID: 44746
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Output ",
					UI.PRE_KEYWORD,
					"Conveyor Rail",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x020027F2 RID: 10226
			public class OUTPUTPIPEFULL
			{
				// Token: 0x0400AECB RID: 44747
				public static LocString NAME = "Output Pipe Full";

				// Token: 0x0400AECC RID: 44748
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Unable to flush contents, output ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" is blocked"
				});
			}

			// Token: 0x020027F3 RID: 10227
			public class CONSTRUCTIONUNREACHABLE
			{
				// Token: 0x0400AECD RID: 44749
				public static LocString NAME = "Unreachable Build";

				// Token: 0x0400AECE RID: 44750
				public static LocString TOOLTIP = "Duplicants cannot reach this construction site";
			}

			// Token: 0x020027F4 RID: 10228
			public class MOPUNREACHABLE
			{
				// Token: 0x0400AECF RID: 44751
				public static LocString NAME = "Unreachable Mop";

				// Token: 0x0400AED0 RID: 44752
				public static LocString TOOLTIP = "Duplicants cannot reach this area";
			}

			// Token: 0x020027F5 RID: 10229
			public class DEADREACTORCOOLINGOFF
			{
				// Token: 0x0400AED1 RID: 44753
				public static LocString NAME = "Cooling ({CyclesRemaining} cycles remaining)";

				// Token: 0x0400AED2 RID: 44754
				public static LocString TOOLTIP = "The radiation coming from this reactor is diminishing";
			}

			// Token: 0x020027F6 RID: 10230
			public class DIGUNREACHABLE
			{
				// Token: 0x0400AED3 RID: 44755
				public static LocString NAME = "Unreachable Dig";

				// Token: 0x0400AED4 RID: 44756
				public static LocString TOOLTIP = "Duplicants cannot reach this area";
			}

			// Token: 0x020027F7 RID: 10231
			public class STORAGEUNREACHABLE
			{
				// Token: 0x0400AED5 RID: 44757
				public static LocString NAME = "Unreachable Storage";

				// Token: 0x0400AED6 RID: 44758
				public static LocString TOOLTIP = "Duplicants cannot reach this storage unit";
			}

			// Token: 0x020027F8 RID: 10232
			public class PASSENGERMODULEUNREACHABLE
			{
				// Token: 0x0400AED7 RID: 44759
				public static LocString NAME = "Unreachable Module";

				// Token: 0x0400AED8 RID: 44760
				public static LocString TOOLTIP = "Duplicants cannot reach this rocket module";
			}

			// Token: 0x020027F9 RID: 10233
			public class CONSTRUCTABLEDIGUNREACHABLE
			{
				// Token: 0x0400AED9 RID: 44761
				public static LocString NAME = "Unreachable Dig";

				// Token: 0x0400AEDA RID: 44762
				public static LocString TOOLTIP = "This construction site contains cells that cannot be dug out";
			}

			// Token: 0x020027FA RID: 10234
			public class EMPTYPUMPINGSTATION
			{
				// Token: 0x0400AEDB RID: 44763
				public static LocString NAME = "Empty";

				// Token: 0x0400AEDC RID: 44764
				public static LocString TOOLTIP = "This pumping station cannot access any " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;
			}

			// Token: 0x020027FB RID: 10235
			public class ENTOMBED
			{
				// Token: 0x0400AEDD RID: 44765
				public static LocString NAME = "Entombed";

				// Token: 0x0400AEDE RID: 44766
				public static LocString TOOLTIP = "Must be dug out by a Duplicant";

				// Token: 0x0400AEDF RID: 44767
				public static LocString NOTIFICATION_NAME = "Building entombment";

				// Token: 0x0400AEE0 RID: 44768
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are entombed and need to be dug out:";
			}

			// Token: 0x020027FC RID: 10236
			public class FABRICATORACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400AEE1 RID: 44769
				public static LocString NAME = "Fabricator accepts mutant seeds";

				// Token: 0x0400AEE2 RID: 44770
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This fabricator is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as recipe ingredients"
				});
			}

			// Token: 0x020027FD RID: 10237
			public class FISHFEEDERACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400AEE3 RID: 44771
				public static LocString NAME = "Fish Feeder accepts mutant seeds";

				// Token: 0x0400AEE4 RID: 44772
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This fish feeder is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as fish food"
				});
			}

			// Token: 0x020027FE RID: 10238
			public class INVALIDPORTOVERLAP
			{
				// Token: 0x0400AEE5 RID: 44773
				public static LocString NAME = "Invalid Port Overlap";

				// Token: 0x0400AEE6 RID: 44774
				public static LocString TOOLTIP = "Ports on this building overlap those on another building\n\nThis building must be rebuilt in a valid location";

				// Token: 0x0400AEE7 RID: 44775
				public static LocString NOTIFICATION_NAME = "Building has overlapping ports";

				// Token: 0x0400AEE8 RID: 44776
				public static LocString NOTIFICATION_TOOLTIP = "These buildings must be rebuilt with non-overlapping ports:";
			}

			// Token: 0x020027FF RID: 10239
			public class GENESHUFFLECOMPLETED
			{
				// Token: 0x0400AEE9 RID: 44777
				public static LocString NAME = "Vacillation Complete";

				// Token: 0x0400AEEA RID: 44778
				public static LocString TOOLTIP = "The Duplicant has completed the neural vacillation process and is ready to be released";
			}

			// Token: 0x02002800 RID: 10240
			public class OVERHEATED
			{
				// Token: 0x0400AEEB RID: 44779
				public static LocString NAME = "Damage: Overheating";

				// Token: 0x0400AEEC RID: 44780
				public static LocString TOOLTIP = "This building is taking damage and will break down if not cooled";
			}

			// Token: 0x02002801 RID: 10241
			public class OVERLOADED
			{
				// Token: 0x0400AEED RID: 44781
				public static LocString NAME = "Damage: Overloading";

				// Token: 0x0400AEEE RID: 44782
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Wire",
					UI.PST_KEYWORD,
					" is taking damage because there are too many buildings pulling ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from this circuit\n\nSplit this ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" circuit into multiple circuits, or use higher quality ",
					UI.PRE_KEYWORD,
					"Wires",
					UI.PST_KEYWORD,
					" to prevent overloading"
				});
			}

			// Token: 0x02002802 RID: 10242
			public class LOGICOVERLOADED
			{
				// Token: 0x0400AEEF RID: 44783
				public static LocString NAME = "Damage: Overloading";

				// Token: 0x0400AEF0 RID: 44784
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Logic Wire",
					UI.PST_KEYWORD,
					" is taking damage\n\nLimit the output to one Bit, or replace it with ",
					UI.PRE_KEYWORD,
					"Logic Ribbon",
					UI.PST_KEYWORD,
					" to prevent further damage"
				});
			}

			// Token: 0x02002803 RID: 10243
			public class OPERATINGENERGY
			{
				// Token: 0x0400AEF1 RID: 44785
				public static LocString NAME = "Heat Production: {0}/s";

				// Token: 0x0400AEF2 RID: 44786
				public static LocString TOOLTIP = "This building is producing <b>{0}</b> per second\n\nSources:\n{1}";

				// Token: 0x0400AEF3 RID: 44787
				public static LocString LINEITEM = "    • {0}: {1}\n";

				// Token: 0x0400AEF4 RID: 44788
				public static LocString OPERATING = "Normal operation";

				// Token: 0x0400AEF5 RID: 44789
				public static LocString EXHAUSTING = "Excess produced";

				// Token: 0x0400AEF6 RID: 44790
				public static LocString PIPECONTENTS_TRANSFER = "Transferred from pipes";

				// Token: 0x0400AEF7 RID: 44791
				public static LocString FOOD_TRANSFER = "Internal Cooling";
			}

			// Token: 0x02002804 RID: 10244
			public class FLOODED
			{
				// Token: 0x0400AEF8 RID: 44792
				public static LocString NAME = "Building Flooded";

				// Token: 0x0400AEF9 RID: 44793
				public static LocString TOOLTIP = "Building cannot function at current saturation";

				// Token: 0x0400AEFA RID: 44794
				public static LocString NOTIFICATION_NAME = "Flooding";

				// Token: 0x0400AEFB RID: 44795
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are flooded:";
			}

			// Token: 0x02002805 RID: 10245
			public class GASVENTOBSTRUCTED
			{
				// Token: 0x0400AEFC RID: 44796
				public static LocString NAME = "Gas Vent Obstructed";

				// Token: 0x0400AEFD RID: 44797
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" has been obstructed and is preventing ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" flow to this vent"
				});
			}

			// Token: 0x02002806 RID: 10246
			public class GASVENTOVERPRESSURE
			{
				// Token: 0x0400AEFE RID: 44798
				public static LocString NAME = "Gas Vent Overpressure";

				// Token: 0x0400AEFF RID: 44799
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" pressure in this area is preventing further ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" emission\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x02002807 RID: 10247
			public class DIRECTION_CONTROL
			{
				// Token: 0x0400AF00 RID: 44800
				public static LocString NAME = "Use Direction: {Direction}";

				// Token: 0x0400AF01 RID: 44801
				public static LocString TOOLTIP = "Duplicants will only use this building when walking by it\n\nCurrently allowed direction: <b>{Direction}</b>";

				// Token: 0x02003159 RID: 12633
				public static class DIRECTIONS
				{
					// Token: 0x0400C5F3 RID: 50675
					public static LocString LEFT = "Left";

					// Token: 0x0400C5F4 RID: 50676
					public static LocString RIGHT = "Right";

					// Token: 0x0400C5F5 RID: 50677
					public static LocString BOTH = "Both";
				}
			}

			// Token: 0x02002808 RID: 10248
			public class WATTSONGAMEOVER
			{
				// Token: 0x0400AF02 RID: 44802
				public static LocString NAME = "Colony Lost";

				// Token: 0x0400AF03 RID: 44803
				public static LocString TOOLTIP = "All Duplicants are dead or incapacitated";
			}

			// Token: 0x02002809 RID: 10249
			public class INVALIDBUILDINGLOCATION
			{
				// Token: 0x0400AF04 RID: 44804
				public static LocString NAME = "Invalid Building Location";

				// Token: 0x0400AF05 RID: 44805
				public static LocString TOOLTIP = "Cannot construct a building in this location";
			}

			// Token: 0x0200280A RID: 10250
			public class LIQUIDVENTOBSTRUCTED
			{
				// Token: 0x0400AF06 RID: 44806
				public static LocString NAME = "Liquid Vent Obstructed";

				// Token: 0x0400AF07 RID: 44807
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Pipe",
					UI.PST_KEYWORD,
					" has been obstructed and is preventing ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" flow to this vent"
				});
			}

			// Token: 0x0200280B RID: 10251
			public class LIQUIDVENTOVERPRESSURE
			{
				// Token: 0x0400AF08 RID: 44808
				public static LocString NAME = "Liquid Vent Overpressure";

				// Token: 0x0400AF09 RID: 44809
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" or ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" pressure in this area is preventing further ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" emission\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x0200280C RID: 10252
			public class MANUALLYCONTROLLED
			{
				// Token: 0x0400AF0A RID: 44810
				public static LocString NAME = "Manually Controlled";

				// Token: 0x0400AF0B RID: 44811
				public static LocString TOOLTIP = "This Duplicant is under my control";
			}

			// Token: 0x0200280D RID: 10253
			public class LIMITVALVELIMITREACHED
			{
				// Token: 0x0400AF0C RID: 44812
				public static LocString NAME = "Limit Reached";

				// Token: 0x0400AF0D RID: 44813
				public static LocString TOOLTIP = "No more Mass can be transferred";
			}

			// Token: 0x0200280E RID: 10254
			public class LIMITVALVELIMITNOTREACHED
			{
				// Token: 0x0400AF0E RID: 44814
				public static LocString NAME = "Amount remaining: {0}";

				// Token: 0x0400AF0F RID: 44815
				public static LocString TOOLTIP = "This building will stop transferring Mass when the amount remaining reaches 0";
			}

			// Token: 0x0200280F RID: 10255
			public class MATERIALSUNAVAILABLE
			{
				// Token: 0x0400AF10 RID: 44816
				public static LocString NAME = "Insufficient Resources\n{ItemsRemaining}";

				// Token: 0x0400AF11 RID: 44817
				public static LocString TOOLTIP = "Crucial materials for this building are beyond reach or unavailable";

				// Token: 0x0400AF12 RID: 44818
				public static LocString NOTIFICATION_NAME = "Building lacks resources";

				// Token: 0x0400AF13 RID: 44819
				public static LocString NOTIFICATION_TOOLTIP = "Crucial materials are unavailable or beyond reach for these buildings:";

				// Token: 0x0400AF14 RID: 44820
				public static LocString LINE_ITEM_MASS = "• {0}: {1}";

				// Token: 0x0400AF15 RID: 44821
				public static LocString LINE_ITEM_UNITS = "• {0}";
			}

			// Token: 0x02002810 RID: 10256
			public class MATERIALSUNAVAILABLEFORREFILL
			{
				// Token: 0x0400AF16 RID: 44822
				public static LocString NAME = "Resources Low\n{ItemsRemaining}";

				// Token: 0x0400AF17 RID: 44823
				public static LocString TOOLTIP = "This building will soon require materials that are unavailable";

				// Token: 0x0400AF18 RID: 44824
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x02002811 RID: 10257
			public class MELTINGDOWN
			{
				// Token: 0x0400AF19 RID: 44825
				public static LocString NAME = "Breaking Down";

				// Token: 0x0400AF1A RID: 44826
				public static LocString TOOLTIP = "This building is collapsing";

				// Token: 0x0400AF1B RID: 44827
				public static LocString NOTIFICATION_NAME = "Building breakdown";

				// Token: 0x0400AF1C RID: 44828
				public static LocString NOTIFICATION_TOOLTIP = "These buildings are collapsing:";
			}

			// Token: 0x02002812 RID: 10258
			public class MISSINGFOUNDATION
			{
				// Token: 0x0400AF1D RID: 44829
				public static LocString NAME = "Missing Tile";

				// Token: 0x0400AF1E RID: 44830
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Build ",
					UI.PRE_KEYWORD,
					"Tile",
					UI.PST_KEYWORD,
					" beneath this building to regain function\n\nTile can be found in the ",
					UI.FormatAsBuildMenuTab("Base Tab", global::Action.Plan1),
					" of the Build Menu"
				});
			}

			// Token: 0x02002813 RID: 10259
			public class NEUTRONIUMUNMINABLE
			{
				// Token: 0x0400AF1F RID: 44831
				public static LocString NAME = "Cannot Mine";

				// Token: 0x0400AF20 RID: 44832
				public static LocString TOOLTIP = "This resource cannot be mined by Duplicant tools";
			}

			// Token: 0x02002814 RID: 10260
			public class NEEDGASIN
			{
				// Token: 0x0400AF21 RID: 44833
				public static LocString NAME = "No Gas Intake\n{GasRequired}";

				// Token: 0x0400AF22 RID: 44834
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Gas Intake",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" connected"
				});

				// Token: 0x0400AF23 RID: 44835
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x02002815 RID: 10261
			public class NEEDGASOUT
			{
				// Token: 0x0400AF24 RID: 44836
				public static LocString NAME = "No Gas Output";

				// Token: 0x0400AF25 RID: 44837
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Gas Output",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.GASCONDUIT.NAME,
					" connected"
				});
			}

			// Token: 0x02002816 RID: 10262
			public class NEEDLIQUIDIN
			{
				// Token: 0x0400AF26 RID: 44838
				public static LocString NAME = "No Liquid Intake\n{LiquidRequired}";

				// Token: 0x0400AF27 RID: 44839
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Liquid Intake",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" connected"
				});

				// Token: 0x0400AF28 RID: 44840
				public static LocString LINE_ITEM = "• {0}";
			}

			// Token: 0x02002817 RID: 10263
			public class NEEDLIQUIDOUT
			{
				// Token: 0x0400AF29 RID: 44841
				public static LocString NAME = "No Liquid Output";

				// Token: 0x0400AF2A RID: 44842
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building's ",
					UI.PRE_KEYWORD,
					"Liquid Output",
					UI.PST_KEYWORD,
					" does not have a ",
					BUILDINGS.PREFABS.LIQUIDCONDUIT.NAME,
					" connected"
				});
			}

			// Token: 0x02002818 RID: 10264
			public class LIQUIDPIPEEMPTY
			{
				// Token: 0x0400AF2B RID: 44843
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400AF2C RID: 44844
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is no ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" in this pipe"
				});
			}

			// Token: 0x02002819 RID: 10265
			public class LIQUIDPIPEOBSTRUCTED
			{
				// Token: 0x0400AF2D RID: 44845
				public static LocString NAME = "Not Pumping";

				// Token: 0x0400AF2E RID: 44846
				public static LocString TOOLTIP = "This pump is not active";
			}

			// Token: 0x0200281A RID: 10266
			public class GASPIPEEMPTY
			{
				// Token: 0x0400AF2F RID: 44847
				public static LocString NAME = "Empty Pipe";

				// Token: 0x0400AF30 RID: 44848
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is no ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" in this pipe"
				});
			}

			// Token: 0x0200281B RID: 10267
			public class GASPIPEOBSTRUCTED
			{
				// Token: 0x0400AF31 RID: 44849
				public static LocString NAME = "Not Pumping";

				// Token: 0x0400AF32 RID: 44850
				public static LocString TOOLTIP = "This pump is not active";
			}

			// Token: 0x0200281C RID: 10268
			public class NEEDSOLIDIN
			{
				// Token: 0x0400AF33 RID: 44851
				public static LocString NAME = "No Conveyor Loader";

				// Token: 0x0400AF34 RID: 44852
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material cannot be fed onto this Conveyor system for transport\n\nEnter the ",
					UI.FormatAsBuildMenuTab("Shipping Tab", global::Action.Plan13),
					" of the Build Menu to build and connect a ",
					UI.PRE_KEYWORD,
					"Conveyor Loader",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200281D RID: 10269
			public class NEEDSOLIDOUT
			{
				// Token: 0x0400AF35 RID: 44853
				public static LocString NAME = "No Conveyor Receptacle";

				// Token: 0x0400AF36 RID: 44854
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material cannot be offloaded from this Conveyor system and will backup the rails\n\nEnter the ",
					UI.FormatAsBuildMenuTab("Shipping Tab", global::Action.Plan13),
					" of the Build Menu to build and connect a ",
					UI.PRE_KEYWORD,
					"Conveyor Receptacle",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200281E RID: 10270
			public class SOLIDPIPEOBSTRUCTED
			{
				// Token: 0x0400AF37 RID: 44855
				public static LocString NAME = "Conveyor Rail Backup";

				// Token: 0x0400AF38 RID: 44856
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Conveyor Rail",
					UI.PST_KEYWORD,
					" cannot carry anymore material\n\nRemove material from the ",
					UI.PRE_KEYWORD,
					"Conveyor Receptacle",
					UI.PST_KEYWORD,
					" to free space for more objects"
				});
			}

			// Token: 0x0200281F RID: 10271
			public class NEEDPLANT
			{
				// Token: 0x0400AF39 RID: 44857
				public static LocString NAME = "No Seeds";

				// Token: 0x0400AF3A RID: 44858
				public static LocString TOOLTIP = "Uproot wild plants to obtain seeds";
			}

			// Token: 0x02002820 RID: 10272
			public class NEEDSEED
			{
				// Token: 0x0400AF3B RID: 44859
				public static LocString NAME = "No Seed Selected";

				// Token: 0x0400AF3C RID: 44860
				public static LocString TOOLTIP = "Uproot wild plants to obtain seeds";
			}

			// Token: 0x02002821 RID: 10273
			public class NEEDPOWER
			{
				// Token: 0x0400AF3D RID: 44861
				public static LocString NAME = "No Power";

				// Token: 0x0400AF3E RID: 44862
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"All connected ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" sources have lost charge"
				});
			}

			// Token: 0x02002822 RID: 10274
			public class NOTENOUGHPOWER
			{
				// Token: 0x0400AF3F RID: 44863
				public static LocString NAME = "Insufficient Power";

				// Token: 0x0400AF40 RID: 44864
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building does not have enough stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" to run"
				});
			}

			// Token: 0x02002823 RID: 10275
			public class POWERLOOPDETECTED
			{
				// Token: 0x0400AF41 RID: 44865
				public static LocString NAME = "Power Loop Detected";

				// Token: 0x0400AF42 RID: 44866
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.PRE_KEYWORD,
					"Transformer's",
					UI.PST_KEYWORD,
					" ",
					UI.PRE_KEYWORD,
					"Power Output",
					UI.PST_KEYWORD,
					" has been connected back to its own ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002824 RID: 10276
			public class NEEDRESOURCE
			{
				// Token: 0x0400AF43 RID: 44867
				public static LocString NAME = "Resource Required";

				// Token: 0x0400AF44 RID: 44868
				public static LocString TOOLTIP = "This building is missing required materials";
			}

			// Token: 0x02002825 RID: 10277
			public class NEWDUPLICANTSAVAILABLE
			{
				// Token: 0x0400AF45 RID: 44869
				public static LocString NAME = "Printables Available";

				// Token: 0x0400AF46 RID: 44870
				public static LocString TOOLTIP = "I am ready to print a new colony member or care package";

				// Token: 0x0400AF47 RID: 44871
				public static LocString NOTIFICATION_NAME = "New Printables are available";

				// Token: 0x0400AF48 RID: 44872
				public static LocString NOTIFICATION_TOOLTIP = "The Printing Pod " + UI.FormatAsHotKey(global::Action.Plan1) + " is ready to print a new Duplicant or care package.\nI'll need to select a blueprint:";
			}

			// Token: 0x02002826 RID: 10278
			public class NOAPPLICABLERESEARCHSELECTED
			{
				// Token: 0x0400AF49 RID: 44873
				public static LocString NAME = "Inapplicable Research";

				// Token: 0x0400AF4A RID: 44874
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building cannot produce the correct ",
					UI.PRE_KEYWORD,
					"Research Type",
					UI.PST_KEYWORD,
					" for the current ",
					UI.FormatAsLink("Research Focus", "TECH")
				});

				// Token: 0x0400AF4B RID: 44875
				public static LocString NOTIFICATION_NAME = UI.FormatAsLink("Research Center", "ADVANCEDRESEARCHCENTER") + " idle";

				// Token: 0x0400AF4C RID: 44876
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These buildings cannot produce the correct ",
					UI.PRE_KEYWORD,
					"Research Type",
					UI.PST_KEYWORD,
					" for the selected ",
					UI.FormatAsLink("Research Focus", "TECH"),
					":"
				});
			}

			// Token: 0x02002827 RID: 10279
			public class NOAPPLICABLEANALYSISSELECTED
			{
				// Token: 0x0400AF4D RID: 44877
				public static LocString NAME = "No Analysis Focus Selected";

				// Token: 0x0400AF4E RID: 44878
				public static LocString TOOLTIP = "Select an unknown destination from the " + UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap) + " to begin analysis";

				// Token: 0x0400AF4F RID: 44879
				public static LocString NOTIFICATION_NAME = UI.FormatAsLink("Telescope", "TELESCOPE") + " idle";

				// Token: 0x0400AF50 RID: 44880
				public static LocString NOTIFICATION_TOOLTIP = "These buildings require an analysis focus:";
			}

			// Token: 0x02002828 RID: 10280
			public class NOAVAILABLESEED
			{
				// Token: 0x0400AF51 RID: 44881
				public static LocString NAME = "No Seed Available";

				// Token: 0x0400AF52 RID: 44882
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The selected ",
					UI.PRE_KEYWORD,
					"Seed",
					UI.PST_KEYWORD,
					" is not available"
				});
			}

			// Token: 0x02002829 RID: 10281
			public class NOSTORAGEFILTERSET
			{
				// Token: 0x0400AF53 RID: 44883
				public static LocString NAME = "Filters Not Designated";

				// Token: 0x0400AF54 RID: 44884
				public static LocString TOOLTIP = "No resources types are marked for storage in this building";
			}

			// Token: 0x0200282A RID: 10282
			public class NOSUITMARKER
			{
				// Token: 0x0400AF55 RID: 44885
				public static LocString NAME = "No Checkpoint";

				// Token: 0x0400AF56 RID: 44886
				public static LocString TOOLTIP = "Docks must be placed beside a " + BUILDINGS.PREFABS.CHECKPOINT.NAME + ", opposite the side the checkpoint faces";
			}

			// Token: 0x0200282B RID: 10283
			public class SUITMARKERWRONGSIDE
			{
				// Token: 0x0400AF57 RID: 44887
				public static LocString NAME = "Invalid Checkpoint";

				// Token: 0x0400AF58 RID: 44888
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has been built on the wrong side of a ",
					BUILDINGS.PREFABS.CHECKPOINT.NAME,
					"\n\nDocks must be placed beside a ",
					BUILDINGS.PREFABS.CHECKPOINT.NAME,
					", opposite the side the checkpoint faces"
				});
			}

			// Token: 0x0200282C RID: 10284
			public class NOFILTERELEMENTSELECTED
			{
				// Token: 0x0400AF59 RID: 44889
				public static LocString NAME = "No Filter Selected";

				// Token: 0x0400AF5A RID: 44890
				public static LocString TOOLTIP = "Select a resource to filter";
			}

			// Token: 0x0200282D RID: 10285
			public class NOLUREELEMENTSELECTED
			{
				// Token: 0x0400AF5B RID: 44891
				public static LocString NAME = "No Bait Selected";

				// Token: 0x0400AF5C RID: 44892
				public static LocString TOOLTIP = "Select a resource to use as bait";
			}

			// Token: 0x0200282E RID: 10286
			public class NOFISHABLEWATERBELOW
			{
				// Token: 0x0400AF5D RID: 44893
				public static LocString NAME = "No Fishable Water";

				// Token: 0x0400AF5E RID: 44894
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There are no edible ",
					UI.PRE_KEYWORD,
					"Fish",
					UI.PST_KEYWORD,
					" beneath this structure"
				});
			}

			// Token: 0x0200282F RID: 10287
			public class NOPOWERCONSUMERS
			{
				// Token: 0x0400AF5F RID: 44895
				public static LocString NAME = "No Power Consumers";

				// Token: 0x0400AF60 RID: 44896
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"No buildings are connected to this ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" source"
				});
			}

			// Token: 0x02002830 RID: 10288
			public class NOWIRECONNECTED
			{
				// Token: 0x0400AF61 RID: 44897
				public static LocString NAME = "No Power Wire Connected";

				// Token: 0x0400AF62 RID: 44898
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has not been connected to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" grid"
				});
			}

			// Token: 0x02002831 RID: 10289
			public class PENDINGDECONSTRUCTION
			{
				// Token: 0x0400AF63 RID: 44899
				public static LocString NAME = "Deconstruction Errand";

				// Token: 0x0400AF64 RID: 44900
				public static LocString TOOLTIP = "Building will be deconstructed once a Duplicant is available";
			}

			// Token: 0x02002832 RID: 10290
			public class PENDINGDEMOLITION
			{
				// Token: 0x0400AF65 RID: 44901
				public static LocString NAME = "Demolition Errand";

				// Token: 0x0400AF66 RID: 44902
				public static LocString TOOLTIP = "Object will be permanently demolished once a Duplicant is available";
			}

			// Token: 0x02002833 RID: 10291
			public class PENDINGFISH
			{
				// Token: 0x0400AF67 RID: 44903
				public static LocString NAME = "Fishing Errand";

				// Token: 0x0400AF68 RID: 44904
				public static LocString TOOLTIP = "Spot will be fished once a Duplicant is available";
			}

			// Token: 0x02002834 RID: 10292
			public class PENDINGHARVEST
			{
				// Token: 0x0400AF69 RID: 44905
				public static LocString NAME = "Harvest Errand";

				// Token: 0x0400AF6A RID: 44906
				public static LocString TOOLTIP = "Plant will be harvested once a Duplicant is available";
			}

			// Token: 0x02002835 RID: 10293
			public class PENDINGUPROOT
			{
				// Token: 0x0400AF6B RID: 44907
				public static LocString NAME = "Uproot Errand";

				// Token: 0x0400AF6C RID: 44908
				public static LocString TOOLTIP = "Plant will be uprooted once a Duplicant is available";
			}

			// Token: 0x02002836 RID: 10294
			public class PENDINGREPAIR
			{
				// Token: 0x0400AF6D RID: 44909
				public static LocString NAME = "Repair Errand";

				// Token: 0x0400AF6E RID: 44910
				public static LocString TOOLTIP = "Building will be repaired once a Duplicant is available\nReceived damage from {DamageInfo}";
			}

			// Token: 0x02002837 RID: 10295
			public class PENDINGSWITCHTOGGLE
			{
				// Token: 0x0400AF6F RID: 44911
				public static LocString NAME = "Settings Errand";

				// Token: 0x0400AF70 RID: 44912
				public static LocString TOOLTIP = "Settings will be changed once a Duplicant is available";
			}

			// Token: 0x02002838 RID: 10296
			public class PENDINGWORK
			{
				// Token: 0x0400AF71 RID: 44913
				public static LocString NAME = "Work Errand";

				// Token: 0x0400AF72 RID: 44914
				public static LocString TOOLTIP = "Building will be operated once a Duplicant is available";
			}

			// Token: 0x02002839 RID: 10297
			public class POWERBUTTONOFF
			{
				// Token: 0x0400AF73 RID: 44915
				public static LocString NAME = "Function Suspended";

				// Token: 0x0400AF74 RID: 44916
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has been toggled off\nPress ",
					UI.PRE_KEYWORD,
					"Enable Building",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ToggleEnabled),
					" to resume its use"
				});
			}

			// Token: 0x0200283A RID: 10298
			public class PUMPINGSTATION
			{
				// Token: 0x0400AF75 RID: 44917
				public static LocString NAME = "Liquid Available: {Liquids}";

				// Token: 0x0400AF76 RID: 44918
				public static LocString TOOLTIP = "This pumping station has access to: {Liquids}";
			}

			// Token: 0x0200283B RID: 10299
			public class PRESSUREOK
			{
				// Token: 0x0400AF77 RID: 44919
				public static LocString NAME = "Max Gas Pressure";

				// Token: 0x0400AF78 RID: 44920
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"High ambient ",
					UI.PRE_KEYWORD,
					"Gas Pressure",
					UI.PST_KEYWORD,
					" is preventing this building from emitting gas\n\nReduce pressure by pumping ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" away or clearing more space"
				});
			}

			// Token: 0x0200283C RID: 10300
			public class UNDERPRESSURE
			{
				// Token: 0x0400AF79 RID: 44921
				public static LocString NAME = "Low Air Pressure";

				// Token: 0x0400AF7A RID: 44922
				public static LocString TOOLTIP = "A minimum atmospheric pressure of <b>{TargetPressure}</b> is needed for this building to operate";
			}

			// Token: 0x0200283D RID: 10301
			public class STORAGELOCKER
			{
				// Token: 0x0400AF7B RID: 44923
				public static LocString NAME = "Storing: {Stored} / {Capacity} {Units}";

				// Token: 0x0400AF7C RID: 44924
				public static LocString TOOLTIP = "This container is storing <b>{Stored}{Units}</b> of a maximum <b>{Capacity}{Units}</b>";
			}

			// Token: 0x0200283E RID: 10302
			public class SKILL_POINTS_AVAILABLE
			{
				// Token: 0x0400AF7D RID: 44925
				public static LocString NAME = "Skill Points Available";

				// Token: 0x0400AF7E RID: 44926
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant has ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					" available"
				});
			}

			// Token: 0x0200283F RID: 10303
			public class TANNINGLIGHTSUFFICIENT
			{
				// Token: 0x0400AF7F RID: 44927
				public static LocString NAME = "Tanning Light Available";

				// Token: 0x0400AF80 RID: 44928
				public static LocString TOOLTIP = "There is sufficient " + UI.FormatAsLink("Light", "LIGHT") + " here to create pleasing skin crisping";
			}

			// Token: 0x02002840 RID: 10304
			public class TANNINGLIGHTINSUFFICIENT
			{
				// Token: 0x0400AF81 RID: 44929
				public static LocString NAME = "Insufficient Tanning Light";

				// Token: 0x0400AF82 RID: 44930
				public static LocString TOOLTIP = "The " + UI.FormatAsLink("Light", "LIGHT") + " here is not bright enough for that Sunny Day feeling";
			}

			// Token: 0x02002841 RID: 10305
			public class UNASSIGNED
			{
				// Token: 0x0400AF83 RID: 44931
				public static LocString NAME = "Unassigned";

				// Token: 0x0400AF84 RID: 44932
				public static LocString TOOLTIP = "Assign a Duplicant to use this amenity";
			}

			// Token: 0x02002842 RID: 10306
			public class UNDERCONSTRUCTION
			{
				// Token: 0x0400AF85 RID: 44933
				public static LocString NAME = "Under Construction";

				// Token: 0x0400AF86 RID: 44934
				public static LocString TOOLTIP = "This building is currently being built";
			}

			// Token: 0x02002843 RID: 10307
			public class UNDERCONSTRUCTIONNOWORKER
			{
				// Token: 0x0400AF87 RID: 44935
				public static LocString NAME = "Construction Errand";

				// Token: 0x0400AF88 RID: 44936
				public static LocString TOOLTIP = "Building will be constructed once a Duplicant is available";
			}

			// Token: 0x02002844 RID: 10308
			public class WAITINGFORMATERIALS
			{
				// Token: 0x0400AF89 RID: 44937
				public static LocString NAME = "Awaiting Delivery\n{ItemsRemaining}";

				// Token: 0x0400AF8A RID: 44938
				public static LocString TOOLTIP = "These materials will be delivered once a Duplicant is available";

				// Token: 0x0400AF8B RID: 44939
				public static LocString LINE_ITEM_MASS = "• {0}: {1}";

				// Token: 0x0400AF8C RID: 44940
				public static LocString LINE_ITEM_UNITS = "• {0}";
			}

			// Token: 0x02002845 RID: 10309
			public class WAITINGFORRADIATION
			{
				// Token: 0x0400AF8D RID: 44941
				public static LocString NAME = "Awaiting Radbolts";

				// Token: 0x0400AF8E RID: 44942
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires Radbolts to function\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay"),
					" ",
					UI.FormatAsHotKey(global::Action.Overlay15),
					" to view this building's radiation port"
				});
			}

			// Token: 0x02002846 RID: 10310
			public class WAITINGFORREPAIRMATERIALS
			{
				// Token: 0x0400AF8F RID: 44943
				public static LocString NAME = "Awaiting Repair Delivery\n{ItemsRemaining}\n";

				// Token: 0x0400AF90 RID: 44944
				public static LocString TOOLTIP = "These materials must be delivered before this building can be repaired";

				// Token: 0x0400AF91 RID: 44945
				public static LocString LINE_ITEM = string.Concat(new string[]
				{
					"• ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					": <b>{1}</b>"
				});
			}

			// Token: 0x02002847 RID: 10311
			public class MISSINGGANTRY
			{
				// Token: 0x0400AF92 RID: 44946
				public static LocString NAME = "Missing Gantry";

				// Token: 0x0400AF93 RID: 44947
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A ",
					UI.FormatAsLink("Gantry", "GANTRY"),
					" must be built below ",
					UI.FormatAsLink("Command Capsules", "COMMANDMODULE"),
					" and ",
					UI.FormatAsLink("Sight-Seeing Modules", "TOURISTMODULE"),
					" for Duplicant access"
				});
			}

			// Token: 0x02002848 RID: 10312
			public class DISEMBARKINGDUPLICANT
			{
				// Token: 0x0400AF94 RID: 44948
				public static LocString NAME = "Waiting To Disembark";

				// Token: 0x0400AF95 RID: 44949
				public static LocString TOOLTIP = "The Duplicant inside this rocket can't come out until the " + UI.FormatAsLink("Gantry", "GANTRY") + " is extended";
			}

			// Token: 0x02002849 RID: 10313
			public class REACTORMELTDOWN
			{
				// Token: 0x0400AF96 RID: 44950
				public static LocString NAME = "Reactor Meltdown";

				// Token: 0x0400AF97 RID: 44951
				public static LocString TOOLTIP = "This reactor is spilling dangerous radioactive waste and cannot be stopped";
			}

			// Token: 0x0200284A RID: 10314
			public class ROCKETNAME
			{
				// Token: 0x0400AF98 RID: 44952
				public static LocString NAME = "Parent Rocket: {0}";

				// Token: 0x0400AF99 RID: 44953
				public static LocString TOOLTIP = "This module belongs to the rocket: " + UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD;
			}

			// Token: 0x0200284B RID: 10315
			public class HASGANTRY
			{
				// Token: 0x0400AF9A RID: 44954
				public static LocString NAME = "Has Gantry";

				// Token: 0x0400AF9B RID: 44955
				public static LocString TOOLTIP = "Duplicants may now enter this section of the rocket";
			}

			// Token: 0x0200284C RID: 10316
			public class NORMAL
			{
				// Token: 0x0400AF9C RID: 44956
				public static LocString NAME = "Normal";

				// Token: 0x0400AF9D RID: 44957
				public static LocString TOOLTIP = "Nothing out of the ordinary here";
			}

			// Token: 0x0200284D RID: 10317
			public class MANUALGENERATORCHARGINGUP
			{
				// Token: 0x0400AF9E RID: 44958
				public static LocString NAME = "Charging Up";

				// Token: 0x0400AF9F RID: 44959
				public static LocString TOOLTIP = "This power source is being charged";
			}

			// Token: 0x0200284E RID: 10318
			public class MANUALGENERATORRELEASINGENERGY
			{
				// Token: 0x0400AFA0 RID: 44960
				public static LocString NAME = "Powering";

				// Token: 0x0400AFA1 RID: 44961
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This generator is supplying energy to ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumers"
				});
			}

			// Token: 0x0200284F RID: 10319
			public class GENERATOROFFLINE
			{
				// Token: 0x0400AFA2 RID: 44962
				public static LocString NAME = "Generator Idle";

				// Token: 0x0400AFA3 RID: 44963
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" source is idle"
				});
			}

			// Token: 0x02002850 RID: 10320
			public class PIPE
			{
				// Token: 0x0400AFA4 RID: 44964
				public static LocString NAME = "Contents: {Contents}";

				// Token: 0x0400AFA5 RID: 44965
				public static LocString TOOLTIP = "This pipe is delivering {Contents}";
			}

			// Token: 0x02002851 RID: 10321
			public class CONVEYOR
			{
				// Token: 0x0400AFA6 RID: 44966
				public static LocString NAME = "Contents: {Contents}";

				// Token: 0x0400AFA7 RID: 44967
				public static LocString TOOLTIP = "This conveyor is delivering {Contents}";
			}

			// Token: 0x02002852 RID: 10322
			public class FABRICATORIDLE
			{
				// Token: 0x0400AFA8 RID: 44968
				public static LocString NAME = "No Fabrications Queued";

				// Token: 0x0400AFA9 RID: 44969
				public static LocString TOOLTIP = "Select a recipe to begin fabrication";
			}

			// Token: 0x02002853 RID: 10323
			public class FABRICATOREMPTY
			{
				// Token: 0x0400AFAA RID: 44970
				public static LocString NAME = "Waiting For Materials";

				// Token: 0x0400AFAB RID: 44971
				public static LocString TOOLTIP = "Fabrication will begin once materials have been delivered";
			}

			// Token: 0x02002854 RID: 10324
			public class FABRICATORLACKSHEP
			{
				// Token: 0x0400AFAC RID: 44972
				public static LocString NAME = "Waiting For Radbolts ({CurrentHEP}/{HEPRequired})";

				// Token: 0x0400AFAD RID: 44973
				public static LocString TOOLTIP = "A queued recipe requires more Radbolts than are currently stored.\n\nCurrently stored: {CurrentHEP}\nRequired for recipe: {HEPRequired}";
			}

			// Token: 0x02002855 RID: 10325
			public class TOILET
			{
				// Token: 0x0400AFAE RID: 44974
				public static LocString NAME = "{FlushesRemaining} \"Visits\" Remaining";

				// Token: 0x0400AFAF RID: 44975
				public static LocString TOOLTIP = "{FlushesRemaining} more Duplicants can use this amenity before it requires maintenance";
			}

			// Token: 0x02002856 RID: 10326
			public class TOILETNEEDSEMPTYING
			{
				// Token: 0x0400AFB0 RID: 44976
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400AFB1 RID: 44977
				public static LocString TOOLTIP = "This amenity cannot be used while full\n\nEmptying it will produce " + UI.FormatAsLink("Polluted Dirt", "TOXICSAND");
			}

			// Token: 0x02002857 RID: 10327
			public class DESALINATORNEEDSEMPTYING
			{
				// Token: 0x0400AFB2 RID: 44978
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400AFB3 RID: 44979
				public static LocString TOOLTIP = "This building needs to be emptied of " + UI.FormatAsLink("Salt", "SALT") + " to resume function";
			}

			// Token: 0x02002858 RID: 10328
			public class MILKSEPARATORNEEDSEMPTYING
			{
				// Token: 0x0400AFB4 RID: 44980
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400AFB5 RID: 44981
				public static LocString TOOLTIP = "This building needs to be emptied of " + UI.FormatAsLink("Brackwax", "MILKFAT") + " to resume function";
			}

			// Token: 0x02002859 RID: 10329
			public class HABITATNEEDSEMPTYING
			{
				// Token: 0x0400AFB6 RID: 44982
				public static LocString NAME = "Requires Emptying";

				// Token: 0x0400AFB7 RID: 44983
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.FormatAsLink("Algae Terrarium", "ALGAEHABITAT"),
					" needs to be emptied of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					"\n\n",
					UI.FormatAsLink("Bottle Emptiers", "BOTTLEEMPTIER"),
					" can be used to transport and dispose of ",
					UI.FormatAsLink("Polluted Water", "DIRTYWATER"),
					" in designated areas"
				});
			}

			// Token: 0x0200285A RID: 10330
			public class UNUSABLE
			{
				// Token: 0x0400AFB8 RID: 44984
				public static LocString NAME = "Out of Order";

				// Token: 0x0400AFB9 RID: 44985
				public static LocString TOOLTIP = "This amenity requires maintenance";
			}

			// Token: 0x0200285B RID: 10331
			public class NORESEARCHSELECTED
			{
				// Token: 0x0400AFBA RID: 44986
				public static LocString NAME = "No Research Focus Selected";

				// Token: 0x0400AFBB RID: 44987
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Open the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" to select a new ",
					UI.FormatAsLink("Research", "TECH"),
					" project"
				});

				// Token: 0x0400AFBC RID: 44988
				public static LocString NOTIFICATION_NAME = "No " + UI.FormatAsLink("Research Focus", "TECH") + " selected";

				// Token: 0x0400AFBD RID: 44989
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"Open the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" to select a new ",
					UI.FormatAsLink("Research", "TECH"),
					" project"
				});
			}

			// Token: 0x0200285C RID: 10332
			public class NORESEARCHORDESTINATIONSELECTED
			{
				// Token: 0x0400AFBE RID: 44990
				public static LocString NAME = "No Research Focus or Starmap Destination Selected";

				// Token: 0x0400AFBF RID: 44991
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Select a ",
					UI.FormatAsLink("Research", "TECH"),
					" project in the ",
					UI.FormatAsManagementMenu("Research Tree", global::Action.ManageResearch),
					" or a Destination in the ",
					UI.FormatAsManagementMenu("Starmap", global::Action.ManageStarmap)
				});

				// Token: 0x0400AFC0 RID: 44992
				public static LocString NOTIFICATION_NAME = "No " + UI.FormatAsLink("Research Focus", "TECH") + " or Starmap destination selected";

				// Token: 0x0400AFC1 RID: 44993
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"Select a ",
					UI.FormatAsLink("Research", "TECH"),
					" project in the ",
					UI.FormatAsManagementMenu("Research Tree", "[R]"),
					" or a Destination in the ",
					UI.FormatAsManagementMenu("Starmap", "[Z]")
				});
			}

			// Token: 0x0200285D RID: 10333
			public class RESEARCHING
			{
				// Token: 0x0400AFC2 RID: 44994
				public static LocString NAME = "Current " + UI.FormatAsLink("Research", "TECH") + ": {Tech}";

				// Token: 0x0400AFC3 RID: 44995
				public static LocString TOOLTIP = "Research produced at this station will be invested in {Tech}";
			}

			// Token: 0x0200285E RID: 10334
			public class TINKERING
			{
				// Token: 0x0400AFC4 RID: 44996
				public static LocString NAME = "Tinkering: {0}";

				// Token: 0x0400AFC5 RID: 44997
				public static LocString TOOLTIP = "This Duplicant is creating {0} to use somewhere else";
			}

			// Token: 0x0200285F RID: 10335
			public class VALVE
			{
				// Token: 0x0400AFC6 RID: 44998
				public static LocString NAME = "Max Flow Rate: {MaxFlow}";

				// Token: 0x0400AFC7 RID: 44999
				public static LocString TOOLTIP = "This valve is allowing flow at a volume of <b>{MaxFlow}</b>";
			}

			// Token: 0x02002860 RID: 10336
			public class VALVEREQUEST
			{
				// Token: 0x0400AFC8 RID: 45000
				public static LocString NAME = "Requested Flow Rate: {QueuedMaxFlow}";

				// Token: 0x0400AFC9 RID: 45001
				public static LocString TOOLTIP = "Waiting for a Duplicant to adjust flow rate";
			}

			// Token: 0x02002861 RID: 10337
			public class EMITTINGLIGHT
			{
				// Token: 0x0400AFCA RID: 45002
				public static LocString NAME = "Emitting Light";

				// Token: 0x0400AFCB RID: 45003
				public static LocString TOOLTIP = "Open the " + UI.FormatAsOverlay("Light Overlay", global::Action.Overlay5) + " to view this light's visibility radius";
			}

			// Token: 0x02002862 RID: 10338
			public class RATIONBOXCONTENTS
			{
				// Token: 0x0400AFCC RID: 45004
				public static LocString NAME = "Storing: {Stored}";

				// Token: 0x0400AFCD RID: 45005
				public static LocString TOOLTIP = "This box contains <b>{Stored}</b> of " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02002863 RID: 10339
			public class EMITTINGELEMENT
			{
				// Token: 0x0400AFCE RID: 45006
				public static LocString NAME = "Emitting {ElementType}: {FlowRate}";

				// Token: 0x0400AFCF RID: 45007
				public static LocString TOOLTIP = "Producing {ElementType} at " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002864 RID: 10340
			public class EMITTINGCO2
			{
				// Token: 0x0400AFD0 RID: 45008
				public static LocString NAME = "Emitting CO<sub>2</sub>: {FlowRate}";

				// Token: 0x0400AFD1 RID: 45009
				public static LocString TOOLTIP = "Producing " + ELEMENTS.CARBONDIOXIDE.NAME + " at " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002865 RID: 10341
			public class EMITTINGOXYGENAVG
			{
				// Token: 0x0400AFD2 RID: 45010
				public static LocString NAME = "Emitting " + UI.FormatAsLink("Oxygen", "OXYGEN") + ": {FlowRate}";

				// Token: 0x0400AFD3 RID: 45011
				public static LocString TOOLTIP = "Producing " + ELEMENTS.OXYGEN.NAME + " at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002866 RID: 10342
			public class EMITTINGGASAVG
			{
				// Token: 0x0400AFD4 RID: 45012
				public static LocString NAME = "Emitting {Element}: {FlowRate}";

				// Token: 0x0400AFD5 RID: 45013
				public static LocString TOOLTIP = "Producing {Element} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002867 RID: 10343
			public class EMITTINGBLOCKEDHIGHPRESSURE
			{
				// Token: 0x0400AFD6 RID: 45014
				public static LocString NAME = "Not Emitting: Overpressure";

				// Token: 0x0400AFD7 RID: 45015
				public static LocString TOOLTIP = "Ambient pressure is too high for {Element} to be released";
			}

			// Token: 0x02002868 RID: 10344
			public class EMITTINGBLOCKEDLOWTEMPERATURE
			{
				// Token: 0x0400AFD8 RID: 45016
				public static LocString NAME = "Not Emitting: Too Cold";

				// Token: 0x0400AFD9 RID: 45017
				public static LocString TOOLTIP = "Temperature is too low for {Element} to be released";
			}

			// Token: 0x02002869 RID: 10345
			public class PUMPINGLIQUIDORGAS
			{
				// Token: 0x0400AFDA RID: 45018
				public static LocString NAME = "Average Flow Rate: {FlowRate}";

				// Token: 0x0400AFDB RID: 45019
				public static LocString TOOLTIP = "This building is pumping an average volume of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x0200286A RID: 10346
			public class WIRECIRCUITSTATUS
			{
				// Token: 0x0400AFDC RID: 45020
				public static LocString NAME = "Current Load: {CurrentLoadAndColor} / {MaxLoad}";

				// Token: 0x0400AFDD RID: 45021
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The current ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" load on this wire\n\nOverloading a wire will cause damage to the wire over time and cause it to break"
				});
			}

			// Token: 0x0200286B RID: 10347
			public class WIREMAXWATTAGESTATUS
			{
				// Token: 0x0400AFDE RID: 45022
				public static LocString NAME = "Potential Load: {TotalPotentialLoadAndColor} / {MaxLoad}";

				// Token: 0x0400AFDF RID: 45023
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"How much wattage this network will draw if all ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumers on the network become active at once"
				});
			}

			// Token: 0x0200286C RID: 10348
			public class NOLIQUIDELEMENTTOPUMP
			{
				// Token: 0x0400AFE0 RID: 45024
				public static LocString NAME = "Pump Not In Liquid";

				// Token: 0x0400AFE1 RID: 45025
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This pump must be submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x0200286D RID: 10349
			public class NOGASELEMENTTOPUMP
			{
				// Token: 0x0400AFE2 RID: 45026
				public static LocString NAME = "Pump Not In Gas";

				// Token: 0x0400AFE3 RID: 45027
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This pump must be submerged in ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x0200286E RID: 10350
			public class INVALIDMASKSTATIONCONSUMPTIONSTATE
			{
				// Token: 0x0400AFE4 RID: 45028
				public static LocString NAME = "Station Not In Oxygen";

				// Token: 0x0400AFE5 RID: 45029
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This station must be submerged in ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" to work"
				});
			}

			// Token: 0x0200286F RID: 10351
			public class PIPEMAYMELT
			{
				// Token: 0x0400AFE6 RID: 45030
				public static LocString NAME = "High Melt Risk";

				// Token: 0x0400AFE7 RID: 45031
				public static LocString TOOLTIP = "This pipe is in danger of melting at the current " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x02002870 RID: 10352
			public class ELEMENTEMITTEROUTPUT
			{
				// Token: 0x0400AFE8 RID: 45032
				public static LocString NAME = "Emitting {ElementTypes}: {FlowRate}";

				// Token: 0x0400AFE9 RID: 45033
				public static LocString TOOLTIP = "This object is releasing {ElementTypes} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002871 RID: 10353
			public class ELEMENTCONSUMER
			{
				// Token: 0x0400AFEA RID: 45034
				public static LocString NAME = "Consuming {ElementTypes}: {FlowRate}";

				// Token: 0x0400AFEB RID: 45035
				public static LocString TOOLTIP = "This object is utilizing ambient {ElementTypes} from the environment";
			}

			// Token: 0x02002872 RID: 10354
			public class SPACECRAFTREADYTOLAND
			{
				// Token: 0x0400AFEC RID: 45036
				public static LocString NAME = "Spacecraft ready to land";

				// Token: 0x0400AFED RID: 45037
				public static LocString TOOLTIP = "A spacecraft is ready to land";

				// Token: 0x0400AFEE RID: 45038
				public static LocString NOTIFICATION = "Space mission complete";

				// Token: 0x0400AFEF RID: 45039
				public static LocString NOTIFICATION_TOOLTIP = "Spacecrafts have completed their missions";
			}

			// Token: 0x02002873 RID: 10355
			public class CONSUMINGFROMSTORAGE
			{
				// Token: 0x0400AFF0 RID: 45040
				public static LocString NAME = "Consuming {ElementTypes}: {FlowRate}";

				// Token: 0x0400AFF1 RID: 45041
				public static LocString TOOLTIP = "This building is consuming {ElementTypes} from storage";
			}

			// Token: 0x02002874 RID: 10356
			public class ELEMENTCONVERTEROUTPUT
			{
				// Token: 0x0400AFF2 RID: 45042
				public static LocString NAME = "Emitting {ElementTypes}: {FlowRate}";

				// Token: 0x0400AFF3 RID: 45043
				public static LocString TOOLTIP = "This building is releasing {ElementTypes} at a rate of " + UI.FormatAsPositiveRate("{FlowRate}");
			}

			// Token: 0x02002875 RID: 10357
			public class ELEMENTCONVERTERINPUT
			{
				// Token: 0x0400AFF4 RID: 45044
				public static LocString NAME = "Using {ElementTypes}: {FlowRate}";

				// Token: 0x0400AFF5 RID: 45045
				public static LocString TOOLTIP = "This building is using {ElementTypes} from storage at a rate of " + UI.FormatAsNegativeRate("{FlowRate}");
			}

			// Token: 0x02002876 RID: 10358
			public class AWAITINGCOMPOSTFLIP
			{
				// Token: 0x0400AFF6 RID: 45046
				public static LocString NAME = "Requires Flipping";

				// Token: 0x0400AFF7 RID: 45047
				public static LocString TOOLTIP = "Compost must be flipped periodically to produce " + UI.FormatAsLink("Dirt", "DIRT");
			}

			// Token: 0x02002877 RID: 10359
			public class AWAITINGWASTE
			{
				// Token: 0x0400AFF8 RID: 45048
				public static LocString NAME = "Awaiting Compostables";

				// Token: 0x0400AFF9 RID: 45049
				public static LocString TOOLTIP = "More waste material is required to begin the composting process";
			}

			// Token: 0x02002878 RID: 10360
			public class BATTERIESSUFFICIENTLYFULL
			{
				// Token: 0x0400AFFA RID: 45050
				public static LocString NAME = "Batteries Sufficiently Full";

				// Token: 0x0400AFFB RID: 45051
				public static LocString TOOLTIP = "All batteries are above the refill threshold";
			}

			// Token: 0x02002879 RID: 10361
			public class NEEDRESOURCEMASS
			{
				// Token: 0x0400AFFC RID: 45052
				public static LocString NAME = "Insufficient Resources\n{ResourcesRequired}";

				// Token: 0x0400AFFD RID: 45053
				public static LocString TOOLTIP = "The mass of material that was delivered to this building was too low\n\nDeliver more material to run this building";

				// Token: 0x0400AFFE RID: 45054
				public static LocString LINE_ITEM = "• <b>{0}</b>";
			}

			// Token: 0x0200287A RID: 10362
			public class JOULESAVAILABLE
			{
				// Token: 0x0400AFFF RID: 45055
				public static LocString NAME = "Power Available: {JoulesAvailable} / {JoulesCapacity}";

				// Token: 0x0400B000 RID: 45056
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"<b>{JoulesAvailable}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" available for use"
				});
			}

			// Token: 0x0200287B RID: 10363
			public class WATTAGE
			{
				// Token: 0x0400B001 RID: 45057
				public static LocString NAME = "Wattage: {Wattage}";

				// Token: 0x0400B002 RID: 45058
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200287C RID: 10364
			public class SOLARPANELWATTAGE
			{
				// Token: 0x0400B003 RID: 45059
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400B004 RID: 45060
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This panel is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200287D RID: 10365
			public class MODULESOLARPANELWATTAGE
			{
				// Token: 0x0400B005 RID: 45061
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400B006 RID: 45062
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This panel is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x0200287E RID: 10366
			public class WATTSON
			{
				// Token: 0x0400B007 RID: 45063
				public static LocString NAME = "Next Print: {TimeRemaining}";

				// Token: 0x0400B008 RID: 45064
				public static LocString TOOLTIP = "The Printing Pod can print out new Duplicants and useful resources over time.\nThe next print will be ready in <b>{TimeRemaining}</b>";

				// Token: 0x0400B009 RID: 45065
				public static LocString UNAVAILABLE = "UNAVAILABLE";
			}

			// Token: 0x0200287F RID: 10367
			public class FLUSHTOILET
			{
				// Token: 0x0400B00A RID: 45066
				public static LocString NAME = "{toilet} Ready";

				// Token: 0x0400B00B RID: 45067
				public static LocString TOOLTIP = "This bathroom is ready to receive visitors";
			}

			// Token: 0x02002880 RID: 10368
			public class FLUSHTOILETINUSE
			{
				// Token: 0x0400B00C RID: 45068
				public static LocString NAME = "{toilet} In Use";

				// Token: 0x0400B00D RID: 45069
				public static LocString TOOLTIP = "This bathroom is occupied";
			}

			// Token: 0x02002881 RID: 10369
			public class WIRECONNECTED
			{
				// Token: 0x0400B00E RID: 45070
				public static LocString NAME = "Wire Connected";

				// Token: 0x0400B00F RID: 45071
				public static LocString TOOLTIP = "This wire is connected to a network";
			}

			// Token: 0x02002882 RID: 10370
			public class WIRENOMINAL
			{
				// Token: 0x0400B010 RID: 45072
				public static LocString NAME = "Wire Nominal";

				// Token: 0x0400B011 RID: 45073
				public static LocString TOOLTIP = "This wire is able to handle the wattage it is receiving";
			}

			// Token: 0x02002883 RID: 10371
			public class WIREDISCONNECTED
			{
				// Token: 0x0400B012 RID: 45074
				public static LocString NAME = "Wire Disconnected";

				// Token: 0x0400B013 RID: 45075
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This wire is not connecting a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" consumer to a ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" generator"
				});
			}

			// Token: 0x02002884 RID: 10372
			public class COOLING
			{
				// Token: 0x0400B014 RID: 45076
				public static LocString NAME = "Cooling";

				// Token: 0x0400B015 RID: 45077
				public static LocString TOOLTIP = "This building is cooling the surrounding area";
			}

			// Token: 0x02002885 RID: 10373
			public class COOLINGSTALLEDHOTENV
			{
				// Token: 0x0400B016 RID: 45078
				public static LocString NAME = "Gas Too Hot";

				// Token: 0x0400B017 RID: 45079
				public static LocString TOOLTIP = "Incoming pipe contents cannot be cooled more than <b>{2}</b> below the surrounding environment\n\nEnvironment: {0}\nCurrent Pipe Contents: {1}";
			}

			// Token: 0x02002886 RID: 10374
			public class COOLINGSTALLEDCOLDGAS
			{
				// Token: 0x0400B018 RID: 45080
				public static LocString NAME = "Gas Too Cold";

				// Token: 0x0400B019 RID: 45081
				public static LocString TOOLTIP = "This building cannot cool incoming pipe contents below <b>{0}</b>\n\nCurrent Pipe Contents: {0}";
			}

			// Token: 0x02002887 RID: 10375
			public class COOLINGSTALLEDHOTLIQUID
			{
				// Token: 0x0400B01A RID: 45082
				public static LocString NAME = "Liquid Too Hot";

				// Token: 0x0400B01B RID: 45083
				public static LocString TOOLTIP = "Incoming pipe contents cannot be cooled more than <b>{2}</b> below the surrounding environment\n\nEnvironment: {0}\nCurrent Pipe Contents: {1}";
			}

			// Token: 0x02002888 RID: 10376
			public class COOLINGSTALLEDCOLDLIQUID
			{
				// Token: 0x0400B01C RID: 45084
				public static LocString NAME = "Liquid Too Cold";

				// Token: 0x0400B01D RID: 45085
				public static LocString TOOLTIP = "This building cannot cool incoming pipe contents below <b>{0}</b>\n\nCurrent Pipe Contents: {0}";
			}

			// Token: 0x02002889 RID: 10377
			public class CANNOTCOOLFURTHER
			{
				// Token: 0x0400B01E RID: 45086
				public static LocString NAME = "Minimum Temperature Reached";

				// Token: 0x0400B01F RID: 45087
				public static LocString TOOLTIP = "This building cannot cool the surrounding environment below <b>{0}</b>";
			}

			// Token: 0x0200288A RID: 10378
			public class HEATINGSTALLEDHOTENV
			{
				// Token: 0x0400B020 RID: 45088
				public static LocString NAME = "Target Temperature Reached";

				// Token: 0x0400B021 RID: 45089
				public static LocString TOOLTIP = "This building cannot heat the surrounding environment beyond <b>{0}</b>";
			}

			// Token: 0x0200288B RID: 10379
			public class HEATINGSTALLEDLOWMASS_GAS
			{
				// Token: 0x0400B022 RID: 45090
				public static LocString NAME = "Insufficient Atmosphere";

				// Token: 0x0400B023 RID: 45091
				public static LocString TOOLTIP = "This building cannot operate in a vacuum";
			}

			// Token: 0x0200288C RID: 10380
			public class HEATINGSTALLEDLOWMASS_LIQUID
			{
				// Token: 0x0400B024 RID: 45092
				public static LocString NAME = "Not Submerged In Liquid";

				// Token: 0x0400B025 RID: 45093
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be submerged in ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" to function"
				});
			}

			// Token: 0x0200288D RID: 10381
			public class BUILDINGDISABLED
			{
				// Token: 0x0400B026 RID: 45094
				public static LocString NAME = "Building Disabled";

				// Token: 0x0400B027 RID: 45095
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Press ",
					UI.PRE_KEYWORD,
					"Enable Building",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.ToggleEnabled),
					" to resume use"
				});
			}

			// Token: 0x0200288E RID: 10382
			public class MISSINGREQUIREMENTS
			{
				// Token: 0x0400B028 RID: 45096
				public static LocString NAME = "Missing Requirements";

				// Token: 0x0400B029 RID: 45097
				public static LocString TOOLTIP = "There are some problems that need to be fixed before this building is operational";
			}

			// Token: 0x0200288F RID: 10383
			public class GETTINGREADY
			{
				// Token: 0x0400B02A RID: 45098
				public static LocString NAME = "Getting Ready";

				// Token: 0x0400B02B RID: 45099
				public static LocString TOOLTIP = "This building will soon be ready to use";
			}

			// Token: 0x02002890 RID: 10384
			public class WORKING
			{
				// Token: 0x0400B02C RID: 45100
				public static LocString NAME = "Nominal";

				// Token: 0x0400B02D RID: 45101
				public static LocString TOOLTIP = "This building is working as intended";
			}

			// Token: 0x02002891 RID: 10385
			public class GRAVEEMPTY
			{
				// Token: 0x0400B02E RID: 45102
				public static LocString NAME = "Empty";

				// Token: 0x0400B02F RID: 45103
				public static LocString TOOLTIP = "This memorial honors no one.";
			}

			// Token: 0x02002892 RID: 10386
			public class GRAVE
			{
				// Token: 0x0400B030 RID: 45104
				public static LocString NAME = "RIP {DeadDupe}";

				// Token: 0x0400B031 RID: 45105
				public static LocString TOOLTIP = "{Epitaph}";
			}

			// Token: 0x02002893 RID: 10387
			public class AWAITINGARTING
			{
				// Token: 0x0400B032 RID: 45106
				public static LocString NAME = "Incomplete Artwork";

				// Token: 0x0400B033 RID: 45107
				public static LocString TOOLTIP = "This building requires a Duplicant's artistic touch";
			}

			// Token: 0x02002894 RID: 10388
			public class LOOKINGUGLY
			{
				// Token: 0x0400B034 RID: 45108
				public static LocString NAME = "Crude";

				// Token: 0x0400B035 RID: 45109
				public static LocString TOOLTIP = "Honestly, Morbs could've done better";
			}

			// Token: 0x02002895 RID: 10389
			public class LOOKINGOKAY
			{
				// Token: 0x0400B036 RID: 45110
				public static LocString NAME = "Quaint";

				// Token: 0x0400B037 RID: 45111
				public static LocString TOOLTIP = "Duplicants find this art piece quite charming";
			}

			// Token: 0x02002896 RID: 10390
			public class LOOKINGGREAT
			{
				// Token: 0x0400B038 RID: 45112
				public static LocString NAME = "Masterpiece";

				// Token: 0x0400B039 RID: 45113
				public static LocString TOOLTIP = "This poignant piece stirs something deep within each Duplicant's soul";
			}

			// Token: 0x02002897 RID: 10391
			public class EXPIRED
			{
				// Token: 0x0400B03A RID: 45114
				public static LocString NAME = "Depleted";

				// Token: 0x0400B03B RID: 45115
				public static LocString TOOLTIP = "This building has no more use";
			}

			// Token: 0x02002898 RID: 10392
			public class EXCAVATOR_BOMB
			{
				// Token: 0x0200315A RID: 12634
				public class UNARMED
				{
					// Token: 0x0400C5F6 RID: 50678
					public static LocString NAME = "Unarmed";

					// Token: 0x0400C5F7 RID: 50679
					public static LocString TOOLTIP = "This explosive is currently inactive";
				}

				// Token: 0x0200315B RID: 12635
				public class ARMED
				{
					// Token: 0x0400C5F8 RID: 50680
					public static LocString NAME = "Armed";

					// Token: 0x0400C5F9 RID: 50681
					public static LocString TOOLTIP = "Stand back, this baby's ready to blow!";
				}

				// Token: 0x0200315C RID: 12636
				public class COUNTDOWN
				{
					// Token: 0x0400C5FA RID: 50682
					public static LocString NAME = "Countdown: {0}";

					// Token: 0x0400C5FB RID: 50683
					public static LocString TOOLTIP = "<b>{0}</b> seconds until detonation";
				}

				// Token: 0x0200315D RID: 12637
				public class DUPE_DANGER
				{
					// Token: 0x0400C5FC RID: 50684
					public static LocString NAME = "Duplicant Preservation Override";

					// Token: 0x0400C5FD RID: 50685
					public static LocString TOOLTIP = "Explosive disabled due to close Duplicant proximity";
				}

				// Token: 0x0200315E RID: 12638
				public class EXPLODING
				{
					// Token: 0x0400C5FE RID: 50686
					public static LocString NAME = "Exploding";

					// Token: 0x0400C5FF RID: 50687
					public static LocString TOOLTIP = "Kaboom!";
				}
			}

			// Token: 0x02002899 RID: 10393
			public class BURNER
			{
				// Token: 0x0200315F RID: 12639
				public class BURNING_FUEL
				{
					// Token: 0x0400C600 RID: 50688
					public static LocString NAME = "Consuming Fuel: {0}";

					// Token: 0x0400C601 RID: 50689
					public static LocString TOOLTIP = "<b>{0}</b> fuel remaining";
				}

				// Token: 0x02003160 RID: 12640
				public class HAS_FUEL
				{
					// Token: 0x0400C602 RID: 50690
					public static LocString NAME = "Fueled: {0}";

					// Token: 0x0400C603 RID: 50691
					public static LocString TOOLTIP = "<b>{0}</b> fuel remaining";
				}
			}

			// Token: 0x0200289A RID: 10394
			public class CREATURE_REUSABLE_TRAP
			{
				// Token: 0x02003161 RID: 12641
				public class NEEDS_ARMING
				{
					// Token: 0x0400C604 RID: 50692
					public static LocString NAME = "Waiting to be Armed";

					// Token: 0x0400C605 RID: 50693
					public static LocString TOOLTIP = "Waiting for a Duplicant to arm this trap\n\nOnly Duplicants with the " + DUPLICANTS.ROLES.RANCHER.NAME + " skill can arm traps";
				}

				// Token: 0x02003162 RID: 12642
				public class READY
				{
					// Token: 0x0400C606 RID: 50694
					public static LocString NAME = "Armed";

					// Token: 0x0400C607 RID: 50695
					public static LocString TOOLTIP = "This trap has been armed and is ready to catch a " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
				}

				// Token: 0x02003163 RID: 12643
				public class SPRUNG
				{
					// Token: 0x0400C608 RID: 50696
					public static LocString NAME = "Sprung";

					// Token: 0x0400C609 RID: 50697
					public static LocString TOOLTIP = "This trap has caught a {0}!";
				}
			}

			// Token: 0x0200289B RID: 10395
			public class CREATURE_TRAP
			{
				// Token: 0x02003164 RID: 12644
				public class NEEDSBAIT
				{
					// Token: 0x0400C60A RID: 50698
					public static LocString NAME = "Needs Bait";

					// Token: 0x0400C60B RID: 50699
					public static LocString TOOLTIP = "This trap needs to be baited before it can be set";
				}

				// Token: 0x02003165 RID: 12645
				public class READY
				{
					// Token: 0x0400C60C RID: 50700
					public static LocString NAME = "Set";

					// Token: 0x0400C60D RID: 50701
					public static LocString TOOLTIP = "This trap has been set and is ready to catch a " + UI.PRE_KEYWORD + "Critter" + UI.PST_KEYWORD;
				}

				// Token: 0x02003166 RID: 12646
				public class SPRUNG
				{
					// Token: 0x0400C60E RID: 50702
					public static LocString NAME = "Sprung";

					// Token: 0x0400C60F RID: 50703
					public static LocString TOOLTIP = "This trap has caught a {0}!";
				}
			}

			// Token: 0x0200289C RID: 10396
			public class ACCESS_CONTROL
			{
				// Token: 0x02003167 RID: 12647
				public class ACTIVE
				{
					// Token: 0x0400C610 RID: 50704
					public static LocString NAME = "Access Restrictions";

					// Token: 0x0400C611 RID: 50705
					public static LocString TOOLTIP = "Some Duplicants are prohibited from passing through this door by the current " + UI.PRE_KEYWORD + "Access Permissions" + UI.PST_KEYWORD;
				}

				// Token: 0x02003168 RID: 12648
				public class OFFLINE
				{
					// Token: 0x0400C612 RID: 50706
					public static LocString NAME = "Access Control Offline";

					// Token: 0x0400C613 RID: 50707
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"This door has granted Emergency ",
						UI.PRE_KEYWORD,
						"Access Permissions",
						UI.PST_KEYWORD,
						"\n\nAll Duplicants are permitted to pass through it until ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						" is restored"
					});
				}
			}

			// Token: 0x0200289D RID: 10397
			public class REQUIRESSKILLPERK
			{
				// Token: 0x0400B03C RID: 45116
				public static LocString NAME = "Skill-Required Operation";

				// Token: 0x0400B03D RID: 45117
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Only Duplicants with one of the following ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					" can operate this building:\n{Skills}"
				});
			}

			// Token: 0x0200289E RID: 10398
			public class DIGREQUIRESSKILLPERK
			{
				// Token: 0x0400B03E RID: 45118
				public static LocString NAME = "Skill-Required Dig";

				// Token: 0x0400B03F RID: 45119
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Only Duplicants with one of the following ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					" can mine this material:\n{Skills}"
				});
			}

			// Token: 0x0200289F RID: 10399
			public class COLONYLACKSREQUIREDSKILLPERK
			{
				// Token: 0x0400B040 RID: 45120
				public static LocString NAME = "Colony Lacks {Skills} Skill";

				// Token: 0x0400B041 RID: 45121
				public static LocString TOOLTIP = "{Skills} Skill required to operate\n\nOpen the " + UI.FormatAsManagementMenu("Skills Panel", global::Action.ManageSkills) + " to teach {Skills} to a Duplicant";
			}

			// Token: 0x020028A0 RID: 10400
			public class CLUSTERCOLONYLACKSREQUIREDSKILLPERK
			{
				// Token: 0x0400B042 RID: 45122
				public static LocString NAME = "Local Colony Lacks {Skills} Skill";

				// Token: 0x0400B043 RID: 45123
				public static LocString TOOLTIP = BUILDING.STATUSITEMS.COLONYLACKSREQUIREDSKILLPERK.TOOLTIP + ", or bring a Duplicant with the skill from another " + UI.CLUSTERMAP.PLANETOID;
			}

			// Token: 0x020028A1 RID: 10401
			public class WORKREQUIRESMINION
			{
				// Token: 0x0400B044 RID: 45124
				public static LocString NAME = "Duplicant Operation Required";

				// Token: 0x0400B045 RID: 45125
				public static LocString TOOLTIP = "A Duplicant must be present to complete this operation";
			}

			// Token: 0x020028A2 RID: 10402
			public class SWITCHSTATUSACTIVE
			{
				// Token: 0x0400B046 RID: 45126
				public static LocString NAME = "Active";

				// Token: 0x0400B047 RID: 45127
				public static LocString TOOLTIP = "This switch is currently toggled <b>On</b>";
			}

			// Token: 0x020028A3 RID: 10403
			public class SWITCHSTATUSINACTIVE
			{
				// Token: 0x0400B048 RID: 45128
				public static LocString NAME = "Inactive";

				// Token: 0x0400B049 RID: 45129
				public static LocString TOOLTIP = "This switch is currently toggled <b>Off</b>";
			}

			// Token: 0x020028A4 RID: 10404
			public class LOGICSWITCHSTATUSACTIVE
			{
				// Token: 0x0400B04A RID: 45130
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400B04B RID: 45131
				public static LocString TOOLTIP = "This switch is currently sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);
			}

			// Token: 0x020028A5 RID: 10405
			public class LOGICSWITCHSTATUSINACTIVE
			{
				// Token: 0x0400B04C RID: 45132
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400B04D RID: 45133
				public static LocString TOOLTIP = "This switch is currently sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020028A6 RID: 10406
			public class LOGICSENSORSTATUSACTIVE
			{
				// Token: 0x0400B04E RID: 45134
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);

				// Token: 0x0400B04F RID: 45135
				public static LocString TOOLTIP = "This sensor is currently sending a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active);
			}

			// Token: 0x020028A7 RID: 10407
			public class LOGICSENSORSTATUSINACTIVE
			{
				// Token: 0x0400B050 RID: 45136
				public static LocString NAME = "Sending a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);

				// Token: 0x0400B051 RID: 45137
				public static LocString TOOLTIP = "This sensor is currently sending " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby);
			}

			// Token: 0x020028A8 RID: 10408
			public class PLAYERCONTROLLEDTOGGLESIDESCREEN
			{
				// Token: 0x0400B052 RID: 45138
				public static LocString NAME = "Pending Toggle on Unpause";

				// Token: 0x0400B053 RID: 45139
				public static LocString TOOLTIP = "This will be toggled when time is unpaused";
			}

			// Token: 0x020028A9 RID: 10409
			public class FOOD_CONTAINERS_OUTSIDE_RANGE
			{
				// Token: 0x0400B054 RID: 45140
				public static LocString NAME = "Unreachable food";

				// Token: 0x0400B055 RID: 45141
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Recuperating Duplicants must have ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" available within <b>{0}</b> cells"
				});
			}

			// Token: 0x020028AA RID: 10410
			public class TOILETS_OUTSIDE_RANGE
			{
				// Token: 0x0400B056 RID: 45142
				public static LocString NAME = "Unreachable restroom";

				// Token: 0x0400B057 RID: 45143
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Recuperating Duplicants must have ",
					UI.PRE_KEYWORD,
					"Toilets",
					UI.PST_KEYWORD,
					" available within <b>{0}</b> cells"
				});
			}

			// Token: 0x020028AB RID: 10411
			public class BUILDING_DEPRECATED
			{
				// Token: 0x0400B058 RID: 45144
				public static LocString NAME = "Building Deprecated";

				// Token: 0x0400B059 RID: 45145
				public static LocString TOOLTIP = "This building is from an older version of the game and its use is not intended";
			}

			// Token: 0x020028AC RID: 10412
			public class TURBINE_BLOCKED_INPUT
			{
				// Token: 0x0400B05A RID: 45146
				public static LocString NAME = "All Inputs Blocked";

				// Token: 0x0400B05B RID: 45147
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This turbine's ",
					UI.PRE_KEYWORD,
					"Input Vents",
					UI.PST_KEYWORD,
					" are blocked, so it can't intake any ",
					ELEMENTS.STEAM.NAME,
					".\n\nThe ",
					UI.PRE_KEYWORD,
					"Input Vents",
					UI.PST_KEYWORD,
					" are located directly below the foundation ",
					UI.PRE_KEYWORD,
					"Tile",
					UI.PST_KEYWORD,
					" this building is resting on."
				});
			}

			// Token: 0x020028AD RID: 10413
			public class TURBINE_PARTIALLY_BLOCKED_INPUT
			{
				// Token: 0x0400B05C RID: 45148
				public static LocString NAME = "{Blocked}/{Total} Inputs Blocked";

				// Token: 0x0400B05D RID: 45149
				public static LocString TOOLTIP = "<b>{Blocked}</b> of this turbine's <b>{Total}</b> inputs have been blocked, resulting in reduced throughput";
			}

			// Token: 0x020028AE RID: 10414
			public class TURBINE_TOO_HOT
			{
				// Token: 0x0400B05E RID: 45150
				public static LocString NAME = "Turbine Too Hot";

				// Token: 0x0400B05F RID: 45151
				public static LocString TOOLTIP = "This turbine must be below <b>{Overheat_Temperature}</b> to properly process {Src_Element} into {Dest_Element}";
			}

			// Token: 0x020028AF RID: 10415
			public class TURBINE_BLOCKED_OUTPUT
			{
				// Token: 0x0400B060 RID: 45152
				public static LocString NAME = "Output Blocked";

				// Token: 0x0400B061 RID: 45153
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A blocked ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" has stopped this turbine from functioning"
				});
			}

			// Token: 0x020028B0 RID: 10416
			public class TURBINE_INSUFFICIENT_MASS
			{
				// Token: 0x0400B062 RID: 45154
				public static LocString NAME = "Not Enough {Src_Element}";

				// Token: 0x0400B063 RID: 45155
				public static LocString TOOLTIP = "The {Src_Element} present below this turbine must be at least <b>{Min_Mass}</b> in order to turn the turbine";
			}

			// Token: 0x020028B1 RID: 10417
			public class TURBINE_INSUFFICIENT_TEMPERATURE
			{
				// Token: 0x0400B064 RID: 45156
				public static LocString NAME = "{Src_Element} Temperature Below {Active_Temperature}";

				// Token: 0x0400B065 RID: 45157
				public static LocString TOOLTIP = "This turbine requires {Src_Element} that is a minimum of <b>{Active_Temperature}</b> in order to produce power";
			}

			// Token: 0x020028B2 RID: 10418
			public class TURBINE_ACTIVE_WATTAGE
			{
				// Token: 0x0400B066 RID: 45158
				public static LocString NAME = "Current Wattage: {Wattage}";

				// Token: 0x0400B067 RID: 45159
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This turbine is generating ",
					UI.FormatAsPositiveRate("{Wattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nIt is running at <b>{Efficiency}</b> of full capacity\n\nIncrease {Src_Element} ",
					UI.PRE_KEYWORD,
					"Mass",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" to improve output"
				});
			}

			// Token: 0x020028B3 RID: 10419
			public class TURBINE_SPINNING_UP
			{
				// Token: 0x0400B068 RID: 45160
				public static LocString NAME = "Spinning Up";

				// Token: 0x0400B069 RID: 45161
				public static LocString TOOLTIP = "This turbine is currently spinning up\n\nSpinning up allows a turbine to continue running for a short period if the pressure it needs to run becomes unavailable";
			}

			// Token: 0x020028B4 RID: 10420
			public class TURBINE_ACTIVE
			{
				// Token: 0x0400B06A RID: 45162
				public static LocString NAME = "Active";

				// Token: 0x0400B06B RID: 45163
				public static LocString TOOLTIP = "This turbine is running at <b>{0}RPM</b>";
			}

			// Token: 0x020028B5 RID: 10421
			public class WELL_PRESSURIZING
			{
				// Token: 0x0400B06C RID: 45164
				public static LocString NAME = "Backpressure: {0}";

				// Token: 0x0400B06D RID: 45165
				public static LocString TOOLTIP = "Well pressure increases with each use and must be periodically relieved to prevent shutdown";
			}

			// Token: 0x020028B6 RID: 10422
			public class WELL_OVERPRESSURE
			{
				// Token: 0x0400B06E RID: 45166
				public static LocString NAME = "Overpressure";

				// Token: 0x0400B06F RID: 45167
				public static LocString TOOLTIP = "This well can no longer function due to excessive backpressure";
			}

			// Token: 0x020028B7 RID: 10423
			public class NOTINANYROOM
			{
				// Token: 0x0400B070 RID: 45168
				public static LocString NAME = "Outside of room";

				// Token: 0x0400B071 RID: 45169
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be built inside a ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" for full functionality\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x020028B8 RID: 10424
			public class NOTINREQUIREDROOM
			{
				// Token: 0x0400B072 RID: 45170
				public static LocString NAME = "Outside of {0}";

				// Token: 0x0400B073 RID: 45171
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building must be built inside a {0} for full functionality\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x020028B9 RID: 10425
			public class NOTINRECOMMENDEDROOM
			{
				// Token: 0x0400B074 RID: 45172
				public static LocString NAME = "Outside of {0}";

				// Token: 0x0400B075 RID: 45173
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"It is recommended to build this building inside a {0}\n\nOpen the ",
					UI.FormatAsOverlay("Room Overlay", global::Action.Overlay11),
					" to view full ",
					UI.PRE_KEYWORD,
					"Room",
					UI.PST_KEYWORD,
					" status"
				});
			}

			// Token: 0x020028BA RID: 10426
			public class RELEASING_PRESSURE
			{
				// Token: 0x0400B076 RID: 45174
				public static LocString NAME = "Releasing Pressure";

				// Token: 0x0400B077 RID: 45175
				public static LocString TOOLTIP = "Pressure buildup is being safely released";
			}

			// Token: 0x020028BB RID: 10427
			public class LOGIC_FEEDBACK_LOOP
			{
				// Token: 0x0400B078 RID: 45176
				public static LocString NAME = "Feedback Loop";

				// Token: 0x0400B079 RID: 45177
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Feedback loops prevent automation grids from functioning\n\nFeedback loops occur when the ",
					UI.PRE_KEYWORD,
					"Output",
					UI.PST_KEYWORD,
					" of an automated building connects back to its own ",
					UI.PRE_KEYWORD,
					"Input",
					UI.PST_KEYWORD,
					" through the Automation grid"
				});
			}

			// Token: 0x020028BC RID: 10428
			public class ENOUGH_COOLANT
			{
				// Token: 0x0400B07A RID: 45178
				public static LocString NAME = "Awaiting Coolant";

				// Token: 0x0400B07B RID: 45179
				public static LocString TOOLTIP = "<b>{1}</b> of {0} must be present in storage to begin production";
			}

			// Token: 0x020028BD RID: 10429
			public class ENOUGH_FUEL
			{
				// Token: 0x0400B07C RID: 45180
				public static LocString NAME = "Awaiting Fuel";

				// Token: 0x0400B07D RID: 45181
				public static LocString TOOLTIP = "<b>{1}</b> of {0} must be present in storage to begin production";
			}

			// Token: 0x020028BE RID: 10430
			public class LOGIC
			{
				// Token: 0x0400B07E RID: 45182
				public static LocString LOGIC_CONTROLLED_ENABLED = "Enabled by Automation Grid";

				// Token: 0x0400B07F RID: 45183
				public static LocString LOGIC_CONTROLLED_DISABLED = "Disabled by Automation Grid";
			}

			// Token: 0x020028BF RID: 10431
			public class GANTRY
			{
				// Token: 0x0400B080 RID: 45184
				public static LocString AUTOMATION_CONTROL = "Automation Control: {0}";

				// Token: 0x0400B081 RID: 45185
				public static LocString MANUAL_CONTROL = "Manual Control: {0}";

				// Token: 0x0400B082 RID: 45186
				public static LocString EXTENDED = "Extended";

				// Token: 0x0400B083 RID: 45187
				public static LocString RETRACTED = "Retracted";
			}

			// Token: 0x020028C0 RID: 10432
			public class OBJECTDISPENSER
			{
				// Token: 0x0400B084 RID: 45188
				public static LocString AUTOMATION_CONTROL = "Automation Control: {0}";

				// Token: 0x0400B085 RID: 45189
				public static LocString MANUAL_CONTROL = "Manual Control: {0}";

				// Token: 0x0400B086 RID: 45190
				public static LocString OPENED = "Opened";

				// Token: 0x0400B087 RID: 45191
				public static LocString CLOSED = "Closed";
			}

			// Token: 0x020028C1 RID: 10433
			public class TOO_COLD
			{
				// Token: 0x0400B088 RID: 45192
				public static LocString NAME = "Too Cold";

				// Token: 0x0400B089 RID: 45193
				public static LocString TOOLTIP = "Either this building or its surrounding environment is too cold to operate";
			}

			// Token: 0x020028C2 RID: 10434
			public class CHECKPOINT
			{
				// Token: 0x0400B08A RID: 45194
				public static LocString LOGIC_CONTROLLED_OPEN = "Clearance: Permitted";

				// Token: 0x0400B08B RID: 45195
				public static LocString LOGIC_CONTROLLED_CLOSED = "Clearance: Not Permitted";

				// Token: 0x0400B08C RID: 45196
				public static LocString LOGIC_CONTROLLED_DISCONNECTED = "No Automation";

				// Token: 0x02003169 RID: 12649
				public class TOOLTIPS
				{
					// Token: 0x0400C614 RID: 50708
					public static LocString LOGIC_CONTROLLED_OPEN = "Automated Checkpoint is receiving a " + UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby) + ", preventing Duplicants from passing";

					// Token: 0x0400C615 RID: 50709
					public static LocString LOGIC_CONTROLLED_CLOSED = "Automated Checkpoint is receiving a " + UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active) + ", allowing Duplicants to pass";

					// Token: 0x0400C616 RID: 50710
					public static LocString LOGIC_CONTROLLED_DISCONNECTED = string.Concat(new string[]
					{
						"This Checkpoint has not been connected to an ",
						UI.PRE_KEYWORD,
						"Automation",
						UI.PST_KEYWORD,
						" grid"
					});
				}
			}

			// Token: 0x020028C3 RID: 10435
			public class HIGHENERGYPARTICLEREDIRECTOR
			{
				// Token: 0x0400B08D RID: 45197
				public static LocString LOGIC_CONTROLLED_STANDBY = "Incoming Radbolts: Ignore";

				// Token: 0x0400B08E RID: 45198
				public static LocString LOGIC_CONTROLLED_ACTIVE = "Incoming Radbolts: Redirect";

				// Token: 0x0400B08F RID: 45199
				public static LocString NORMAL = "Normal";

				// Token: 0x0200316A RID: 12650
				public class TOOLTIPS
				{
					// Token: 0x0400C617 RID: 50711
					public static LocString LOGIC_CONTROLLED_STANDBY = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Reflector"),
						" is receiving a ",
						UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
						", ignoring incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C618 RID: 50712
					public static LocString LOGIC_CONTROLLED_ACTIVE = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Reflector"),
						" is receiving a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						", accepting incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C619 RID: 50713
					public static LocString NORMAL = "Incoming Radbolts will be accepted and redirected";
				}
			}

			// Token: 0x020028C4 RID: 10436
			public class HIGHENERGYPARTICLESPAWNER
			{
				// Token: 0x0400B090 RID: 45200
				public static LocString LOGIC_CONTROLLED_STANDBY = "Launch Radbolt: Off";

				// Token: 0x0400B091 RID: 45201
				public static LocString LOGIC_CONTROLLED_ACTIVE = "Launch Radbolt: On";

				// Token: 0x0400B092 RID: 45202
				public static LocString NORMAL = "Normal";

				// Token: 0x0200316B RID: 12651
				public class TOOLTIPS
				{
					// Token: 0x0400C61A RID: 50714
					public static LocString LOGIC_CONTROLLED_STANDBY = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Generator"),
						" is receiving a ",
						UI.FormatAsAutomationState("Red Signal", UI.AutomationState.Standby),
						", ignoring incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C61B RID: 50715
					public static LocString LOGIC_CONTROLLED_ACTIVE = string.Concat(new string[]
					{
						UI.FormatAsKeyWord("Radbolt Generator"),
						" is receiving a ",
						UI.FormatAsAutomationState("Green Signal", UI.AutomationState.Active),
						", accepting incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C61C RID: 50716
					public static LocString NORMAL = string.Concat(new string[]
					{
						"Incoming ",
						UI.PRE_KEYWORD,
						"Radbolts",
						UI.PST_KEYWORD,
						" will be accepted and redirected"
					});
				}
			}

			// Token: 0x020028C5 RID: 10437
			public class AWAITINGFUEL
			{
				// Token: 0x0400B093 RID: 45203
				public static LocString NAME = "Awaiting Fuel: {0}";

				// Token: 0x0400B094 RID: 45204
				public static LocString TOOLTIP = "This building requires <b>{1}</b> of {0} to operate";
			}

			// Token: 0x020028C6 RID: 10438
			public class FOSSILHUNT
			{
				// Token: 0x0200316C RID: 12652
				public class PENDING_EXCAVATION
				{
					// Token: 0x0400C61D RID: 50717
					public static LocString NAME = "Awaiting Excavation";

					// Token: 0x0400C61E RID: 50718
					public static LocString TOOLTIP = "Currently awaiting excavation by a Duplicant";
				}

				// Token: 0x0200316D RID: 12653
				public class EXCAVATING
				{
					// Token: 0x0400C61F RID: 50719
					public static LocString NAME = "Excavation In Progress";

					// Token: 0x0400C620 RID: 50720
					public static LocString TOOLTIP = "Currently being excavated by a Duplicant";
				}
			}

			// Token: 0x020028C7 RID: 10439
			public class MEGABRAINTANK
			{
				// Token: 0x0200316E RID: 12654
				public class PROGRESS
				{
					// Token: 0x02003414 RID: 13332
					public class PROGRESSIONRATE
					{
						// Token: 0x0400CCB7 RID: 52407
						public static LocString NAME = "Dream Journals: {ActivationProgress}";

						// Token: 0x0400CCB8 RID: 52408
						public static LocString TOOLTIP = "Currently awaiting the Dream Journals necessary to restore this building to full functionality";
					}

					// Token: 0x02003415 RID: 13333
					public class DREAMANALYSIS
					{
						// Token: 0x0400CCB9 RID: 52409
						public static LocString NAME = "Analyzing Dreams: {TimeToComplete}s";

						// Token: 0x0400CCBA RID: 52410
						public static LocString TOOLTIP = "Maximum Aptitude effect sustained while dream analysis continues";
					}
				}

				// Token: 0x0200316F RID: 12655
				public class COMPLETE
				{
					// Token: 0x0400C621 RID: 50721
					public static LocString NAME = "Fully Restored";

					// Token: 0x0400C622 RID: 50722
					public static LocString TOOLTIP = "This building is functioning at full capacity";
				}
			}

			// Token: 0x020028C8 RID: 10440
			public class MEGABRAINNOTENOUGHOXYGEN
			{
				// Token: 0x0400B095 RID: 45205
				public static LocString NAME = "Lacks Oxygen";

				// Token: 0x0400B096 RID: 45206
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building needs ",
					UI.PRE_KEYWORD,
					"Oxygen",
					UI.PST_KEYWORD,
					" in order to function"
				});
			}

			// Token: 0x020028C9 RID: 10441
			public class NOLOGICWIRECONNECTED
			{
				// Token: 0x0400B097 RID: 45207
				public static LocString NAME = "No Automation Wire Connected";

				// Token: 0x0400B098 RID: 45208
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has not been connected to an ",
					UI.PRE_KEYWORD,
					"Automation",
					UI.PST_KEYWORD,
					" grid"
				});
			}

			// Token: 0x020028CA RID: 10442
			public class NOTUBECONNECTED
			{
				// Token: 0x0400B099 RID: 45209
				public static LocString NAME = "No Tube Connected";

				// Token: 0x0400B09A RID: 45210
				public static LocString TOOLTIP = "The first section of tube extending from a " + BUILDINGS.PREFABS.TRAVELTUBEENTRANCE.NAME + " must connect directly upward";
			}

			// Token: 0x020028CB RID: 10443
			public class NOTUBEEXITS
			{
				// Token: 0x0400B09B RID: 45211
				public static LocString NAME = "No Landing Available";

				// Token: 0x0400B09C RID: 45212
				public static LocString TOOLTIP = "Duplicants can only exit a tube when there is somewhere for them to land within <b>two tiles</b>";
			}

			// Token: 0x020028CC RID: 10444
			public class STOREDCHARGE
			{
				// Token: 0x0400B09D RID: 45213
				public static LocString NAME = "Charge Available: {0}/{1}";

				// Token: 0x0400B09E RID: 45214
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building has <b>{0}</b> of stored ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					"\n\nIt consumes ",
					UI.FormatAsNegativeRate("{2}"),
					" per use"
				});
			}

			// Token: 0x020028CD RID: 10445
			public class NEEDEGG
			{
				// Token: 0x0400B09F RID: 45215
				public static LocString NAME = "No Egg Selected";

				// Token: 0x0400B0A0 RID: 45216
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Collect ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					" from ",
					UI.FormatAsLink("Critters", "CREATURES"),
					" to incubate"
				});
			}

			// Token: 0x020028CE RID: 10446
			public class NOAVAILABLEEGG
			{
				// Token: 0x0400B0A1 RID: 45217
				public static LocString NAME = "No Egg Available";

				// Token: 0x0400B0A2 RID: 45218
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The selected ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" is not currently available"
				});
			}

			// Token: 0x020028CF RID: 10447
			public class AWAITINGEGGDELIVERY
			{
				// Token: 0x0400B0A3 RID: 45219
				public static LocString NAME = "Awaiting Delivery";

				// Token: 0x0400B0A4 RID: 45220
				public static LocString TOOLTIP = "Awaiting delivery of selected " + UI.PRE_KEYWORD + "Egg" + UI.PST_KEYWORD;
			}

			// Token: 0x020028D0 RID: 10448
			public class INCUBATORPROGRESS
			{
				// Token: 0x0400B0A5 RID: 45221
				public static LocString NAME = "Incubating: {Percent}";

				// Token: 0x0400B0A6 RID: 45222
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Egg",
					UI.PST_KEYWORD,
					" incubating cozily\n\nIt will hatch when ",
					UI.PRE_KEYWORD,
					"Incubation",
					UI.PST_KEYWORD,
					" reaches <b>100%</b>"
				});
			}

			// Token: 0x020028D1 RID: 10449
			public class NETWORKQUALITY
			{
				// Token: 0x0400B0A7 RID: 45223
				public static LocString NAME = "Scan Network Quality: {TotalQuality}";

				// Token: 0x0400B0A8 RID: 45224
				public static LocString TOOLTIP = "This scanner network is scanning at <b>{TotalQuality}</b> effectiveness\n\nIt will detect incoming objects <b>{WorstTime}</b> to <b>{BestTime}</b> before they arrive\n\nBuild multiple " + BUILDINGS.PREFABS.COMETDETECTOR.NAME + "s to increase surface coverage and improve network quality\n\n    • Surface Coverage: <b>{Coverage}</b>";
			}

			// Token: 0x020028D2 RID: 10450
			public class DETECTORSCANNING
			{
				// Token: 0x0400B0A9 RID: 45225
				public static LocString NAME = "Scanning";

				// Token: 0x0400B0AA RID: 45226
				public static LocString TOOLTIP = "This scanner is currently scouring space for anything of interest";
			}

			// Token: 0x020028D3 RID: 10451
			public class INCOMINGMETEORS
			{
				// Token: 0x0400B0AB RID: 45227
				public static LocString NAME = "Incoming Object Detected";

				// Token: 0x0400B0AC RID: 45228
				public static LocString TOOLTIP = "Warning!\n\nHigh velocity objects on approach!";
			}

			// Token: 0x020028D4 RID: 10452
			public class SPACE_VISIBILITY_NONE
			{
				// Token: 0x0400B0AD RID: 45229
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400B0AE RID: 45230
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space\n    • Efficiency: <b>{VISIBILITY}</b>";
			}

			// Token: 0x020028D5 RID: 10453
			public class SPACE_VISIBILITY_REDUCED
			{
				// Token: 0x0400B0AF RID: 45231
				public static LocString NAME = "Reduced Visibility";

				// Token: 0x0400B0B0 RID: 45232
				public static LocString TOOLTIP = "This building has a partially obstructed view of space\n\nTo operate at maximum speed, this building requires an unblocked view of space\n    • Efficiency: <b>{VISIBILITY}</b>";
			}

			// Token: 0x020028D6 RID: 10454
			public class LANDEDROCKETLACKSPASSENGERMODULE
			{
				// Token: 0x0400B0B1 RID: 45233
				public static LocString NAME = "Rocket lacks spacefarer module";

				// Token: 0x0400B0B2 RID: 45234
				public static LocString TOOLTIP = "A rocket must have a spacefarer module";
			}

			// Token: 0x020028D7 RID: 10455
			public class PATH_NOT_CLEAR
			{
				// Token: 0x0400B0B3 RID: 45235
				public static LocString NAME = "Launch Path Blocked";

				// Token: 0x0400B0B4 RID: 45236
				public static LocString TOOLTIP = "There are obstructions in the launch trajectory of this rocket:\n    • {0}\n\nThis rocket requires a clear flight path for launch";

				// Token: 0x0400B0B5 RID: 45237
				public static LocString TILE_FORMAT = "Solid {0}";
			}

			// Token: 0x020028D8 RID: 10456
			public class RAILGUN_PATH_NOT_CLEAR
			{
				// Token: 0x0400B0B6 RID: 45238
				public static LocString NAME = "Launch Path Blocked";

				// Token: 0x0400B0B7 RID: 45239
				public static LocString TOOLTIP = "There are obstructions in the launch trajectory of this " + UI.FormatAsLink("Interplanetary Launcher", "RAILGUN") + "\n\nThis launcher requires a clear path to launch payloads";
			}

			// Token: 0x020028D9 RID: 10457
			public class RAILGUN_NO_DESTINATION
			{
				// Token: 0x0400B0B8 RID: 45240
				public static LocString NAME = "No Delivery Destination";

				// Token: 0x0400B0B9 RID: 45241
				public static LocString TOOLTIP = "A delivery destination has not been set";
			}

			// Token: 0x020028DA RID: 10458
			public class NOSURFACESIGHT
			{
				// Token: 0x0400B0BA RID: 45242
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400B0BB RID: 45243
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x020028DB RID: 10459
			public class ROCKETRESTRICTIONACTIVE
			{
				// Token: 0x0400B0BC RID: 45244
				public static LocString NAME = "Access: Restricted";

				// Token: 0x0400B0BD RID: 45245
				public static LocString TOOLTIP = "This building cannot be operated while restricted, though it can be filled\n\nControlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x020028DC RID: 10460
			public class ROCKETRESTRICTIONINACTIVE
			{
				// Token: 0x0400B0BE RID: 45246
				public static LocString NAME = "Access: Not Restricted";

				// Token: 0x0400B0BF RID: 45247
				public static LocString TOOLTIP = "This building's operation is not restricted\n\nControlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x020028DD RID: 10461
			public class NOROCKETRESTRICTION
			{
				// Token: 0x0400B0C0 RID: 45248
				public static LocString NAME = "Not Controlled";

				// Token: 0x0400B0C1 RID: 45249
				public static LocString TOOLTIP = "This building is not controlled by a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME;
			}

			// Token: 0x020028DE RID: 10462
			public class BROADCASTEROUTOFRANGE
			{
				// Token: 0x0400B0C2 RID: 45250
				public static LocString NAME = "Broadcaster Out of Range";

				// Token: 0x0400B0C3 RID: 45251
				public static LocString TOOLTIP = "This receiver is too far from the selected broadcaster to get signal updates";
			}

			// Token: 0x020028DF RID: 10463
			public class LOSINGRADBOLTS
			{
				// Token: 0x0400B0C4 RID: 45252
				public static LocString NAME = "Radbolt Decay";

				// Token: 0x0400B0C5 RID: 45253
				public static LocString TOOLTIP = "This building is unable to maintain the integrity of the radbolts it is storing";
			}

			// Token: 0x020028E0 RID: 10464
			public class TOP_PRIORITY_CHORE
			{
				// Token: 0x0400B0C6 RID: 45254
				public static LocString NAME = "Top Priority";

				// Token: 0x0400B0C7 RID: 45255
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This errand has been set to ",
					UI.PRE_KEYWORD,
					"Top Priority",
					UI.PST_KEYWORD,
					"\n\nThe colony will be in ",
					UI.PRE_KEYWORD,
					"Yellow Alert",
					UI.PST_KEYWORD,
					" until this task is completed"
				});

				// Token: 0x0400B0C8 RID: 45256
				public static LocString NOTIFICATION_NAME = "Yellow Alert";

				// Token: 0x0400B0C9 RID: 45257
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The following errands have been set to ",
					UI.PRE_KEYWORD,
					"Top Priority",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x020028E1 RID: 10465
			public class HOTTUBWATERTOOCOLD
			{
				// Token: 0x0400B0CA RID: 45258
				public static LocString NAME = "Water Too Cold";

				// Token: 0x0400B0CB RID: 45259
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub's ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" is below <b>{temperature}</b>\n\nIt is draining so it can be refilled with warmer ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x020028E2 RID: 10466
			public class HOTTUBTOOHOT
			{
				// Token: 0x0400B0CC RID: 45260
				public static LocString NAME = "Building Too Hot";

				// Token: 0x0400B0CD RID: 45261
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub's ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" is above <b>{temperature}</b>\n\nIt needs to cool before it can safely be used"
				});
			}

			// Token: 0x020028E3 RID: 10467
			public class HOTTUBFILLING
			{
				// Token: 0x0400B0CE RID: 45262
				public static LocString NAME = "Filling Up: ({fullness})";

				// Token: 0x0400B0CF RID: 45263
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This tub is currently filling with ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					"\n\nIt will be available to use when the ",
					UI.PRE_KEYWORD,
					"Water",
					UI.PST_KEYWORD,
					" level reaches <b>100%</b>"
				});
			}

			// Token: 0x020028E4 RID: 10468
			public class WINDTUNNELINTAKE
			{
				// Token: 0x0400B0D0 RID: 45264
				public static LocString NAME = "Intake Requires Gas";

				// Token: 0x0400B0D1 RID: 45265
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A wind tunnel requires ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					" at the top and bottom intakes in order to operate\n\nThe intakes for this wind tunnel don't have enough gas to operate"
				});
			}

			// Token: 0x020028E5 RID: 10469
			public class TEMPORAL_TEAR_OPENER_NO_TARGET
			{
				// Token: 0x0400B0D2 RID: 45266
				public static LocString NAME = "Temporal Tear not revealed";

				// Token: 0x0400B0D3 RID: 45267
				public static LocString TOOLTIP = "This machine is meant to target something in space, but the target has not yet been revealed";
			}

			// Token: 0x020028E6 RID: 10470
			public class TEMPORAL_TEAR_OPENER_NO_LOS
			{
				// Token: 0x0400B0D4 RID: 45268
				public static LocString NAME = "Line of Sight: Obstructed";

				// Token: 0x0400B0D5 RID: 45269
				public static LocString TOOLTIP = "This device needs a clear view of space to operate";
			}

			// Token: 0x020028E7 RID: 10471
			public class TEMPORAL_TEAR_OPENER_INSUFFICIENT_COLONIES
			{
				// Token: 0x0400B0D6 RID: 45270
				public static LocString NAME = "Too few Printing Pods {progress}";

				// Token: 0x0400B0D7 RID: 45271
				public static LocString TOOLTIP = "To open the Temporal Tear, this device relies on a network of activated Printing Pods {progress}";
			}

			// Token: 0x020028E8 RID: 10472
			public class TEMPORAL_TEAR_OPENER_PROGRESS
			{
				// Token: 0x0400B0D8 RID: 45272
				public static LocString NAME = "Charging Progress: {progress}";

				// Token: 0x0400B0D9 RID: 45273
				public static LocString TOOLTIP = "This device must be charged with a high number of Radbolts\n\nOperation can commence once this device is fully charged";
			}

			// Token: 0x020028E9 RID: 10473
			public class TEMPORAL_TEAR_OPENER_READY
			{
				// Token: 0x0400B0DA RID: 45274
				public static LocString NOTIFICATION = "Temporal Tear Opener fully charged";

				// Token: 0x0400B0DB RID: 45275
				public static LocString NOTIFICATION_TOOLTIP = "Push the red button to activate";
			}

			// Token: 0x020028EA RID: 10474
			public class WARPPORTALCHARGING
			{
				// Token: 0x0400B0DC RID: 45276
				public static LocString NAME = "Recharging: {charge}";

				// Token: 0x0400B0DD RID: 45277
				public static LocString TOOLTIP = "This teleporter will be ready for use in {cycles} cycles";
			}

			// Token: 0x020028EB RID: 10475
			public class WARPCONDUITPARTNERDISABLED
			{
				// Token: 0x0400B0DE RID: 45278
				public static LocString NAME = "Teleporter Disabled ({x}/2)";

				// Token: 0x0400B0DF RID: 45279
				public static LocString TOOLTIP = "This teleporter cannot be used until both the transmitting and receiving sides have been activated";
			}

			// Token: 0x020028EC RID: 10476
			public class COLLECTINGHEP
			{
				// Token: 0x0400B0E0 RID: 45280
				public static LocString NAME = "Collecting Radbolts ({x}/cycle)";

				// Token: 0x0400B0E1 RID: 45281
				public static LocString TOOLTIP = "Collecting Radbolts from ambient radiation";
			}

			// Token: 0x020028ED RID: 10477
			public class INORBIT
			{
				// Token: 0x0400B0E2 RID: 45282
				public static LocString NAME = "In Orbit: {Destination}";

				// Token: 0x0400B0E3 RID: 45283
				public static LocString TOOLTIP = "This rocket is currently in orbit around {Destination}";
			}

			// Token: 0x020028EE RID: 10478
			public class INFLIGHT
			{
				// Token: 0x0400B0E4 RID: 45284
				public static LocString NAME = "In Flight To {Destination_Asteroid}: {ETA}";

				// Token: 0x0400B0E5 RID: 45285
				public static LocString TOOLTIP = "This rocket is currently traveling to {Destination_Pad} on {Destination_Asteroid}\n\nIt will arrive in {ETA}";

				// Token: 0x0400B0E6 RID: 45286
				public static LocString TOOLTIP_NO_PAD = "This rocket is currently traveling to {Destination_Asteroid}\n\nIt will arrive in {ETA}";
			}

			// Token: 0x020028EF RID: 10479
			public class DESTINATIONOUTOFRANGE
			{
				// Token: 0x0400B0E7 RID: 45287
				public static LocString NAME = "Destination Out Of Range";

				// Token: 0x0400B0E8 RID: 45288
				public static LocString TOOLTIP = "This rocket lacks the range to reach its destination\n\nRocket Range: {Range}\nDestination Distance: {Distance}";
			}

			// Token: 0x020028F0 RID: 10480
			public class ROCKETSTRANDED
			{
				// Token: 0x0400B0E9 RID: 45289
				public static LocString NAME = "Stranded";

				// Token: 0x0400B0EA RID: 45290
				public static LocString TOOLTIP = "This rocket has run out of fuel and cannot move";
			}

			// Token: 0x020028F1 RID: 10481
			public class SPACEPOIHARVESTING
			{
				// Token: 0x0400B0EB RID: 45291
				public static LocString NAME = "Extracting Resources: {0}";

				// Token: 0x0400B0EC RID: 45292
				public static LocString TOOLTIP = "Resources are being mined from this space debris";
			}

			// Token: 0x020028F2 RID: 10482
			public class SPACEPOIWASTING
			{
				// Token: 0x0400B0ED RID: 45293
				public static LocString NAME = "Cannot store resources: {0}";

				// Token: 0x0400B0EE RID: 45294
				public static LocString TOOLTIP = "Some resources being mined from this space debris cannot be stored in this rocket";
			}

			// Token: 0x020028F3 RID: 10483
			public class RAILGUNPAYLOADNEEDSEMPTYING
			{
				// Token: 0x0400B0EF RID: 45295
				public static LocString NAME = "Ready To Unpack";

				// Token: 0x0400B0F0 RID: 45296
				public static LocString TOOLTIP = "This payload has reached its destination and is ready to be unloaded\n\nIt can be marked for unpacking manually, or automatically unpacked on arrival using a " + BUILDINGS.PREFABS.RAILGUNPAYLOADOPENER.NAME;
			}

			// Token: 0x020028F4 RID: 10484
			public class MISSIONCONTROLASSISTINGROCKET
			{
				// Token: 0x0400B0F1 RID: 45297
				public static LocString NAME = "Guidance Signal: {0}";

				// Token: 0x0400B0F2 RID: 45298
				public static LocString TOOLTIP = "Once transmission is complete, Mission Control will boost targeted rocket's speed";
			}

			// Token: 0x020028F5 RID: 10485
			public class MISSIONCONTROLBOOSTED
			{
				// Token: 0x0400B0F3 RID: 45299
				public static LocString NAME = "Mission Control Speed Boost: {0}";

				// Token: 0x0400B0F4 RID: 45300
				public static LocString TOOLTIP = "Mission Control has given this rocket a {0} speed boost\n\n{1} remaining";
			}

			// Token: 0x020028F6 RID: 10486
			public class TRANSITTUBEENTRANCEWAXREADY
			{
				// Token: 0x0400B0F5 RID: 45301
				public static LocString NAME = "Smooth Ride Ready";

				// Token: 0x0400B0F6 RID: 45302
				public static LocString TOOLTIP = "This building is stocked with speed-boosting " + ELEMENTS.MILKFAT.NAME + "\n\n{0} per use ({1} remaining)";
			}

			// Token: 0x020028F7 RID: 10487
			public class NOROCKETSTOMISSIONCONTROLBOOST
			{
				// Token: 0x0400B0F7 RID: 45303
				public static LocString NAME = "No Eligible Rockets in Range";

				// Token: 0x0400B0F8 RID: 45304
				public static LocString TOOLTIP = "Rockets must be mid-flight and not targeted by another Mission Control Station, or already boosted";
			}

			// Token: 0x020028F8 RID: 10488
			public class NOROCKETSTOMISSIONCONTROLCLUSTERBOOST
			{
				// Token: 0x0400B0F9 RID: 45305
				public static LocString NAME = "No Eligible Rockets in Range";

				// Token: 0x0400B0FA RID: 45306
				public static LocString TOOLTIP = "Rockets must be mid-flight, within {0} tiles, and not targeted by another Mission Control Station or already boosted";
			}

			// Token: 0x020028F9 RID: 10489
			public class AWAITINGEMPTYBUILDING
			{
				// Token: 0x0400B0FB RID: 45307
				public static LocString NAME = "Empty Errand";

				// Token: 0x0400B0FC RID: 45308
				public static LocString TOOLTIP = "Building will be emptied once a Duplicant is available";
			}

			// Token: 0x020028FA RID: 10490
			public class DUPLICANTACTIVATIONREQUIRED
			{
				// Token: 0x0400B0FD RID: 45309
				public static LocString NAME = "Activation Required";

				// Token: 0x0400B0FE RID: 45310
				public static LocString TOOLTIP = "A Duplicant is required to bring this building online";
			}

			// Token: 0x020028FB RID: 10491
			public class PILOTNEEDED
			{
				// Token: 0x0400B0FF RID: 45311
				public static LocString NAME = "Switching to Autopilot";

				// Token: 0x0400B100 RID: 45312
				public static LocString TOOLTIP = "Autopilot will engage in {timeRemaining} if a Duplicant pilot does not assume control";
			}

			// Token: 0x020028FC RID: 10492
			public class AUTOPILOTACTIVE
			{
				// Token: 0x0400B101 RID: 45313
				public static LocString NAME = "Autopilot Engaged";

				// Token: 0x0400B102 RID: 45314
				public static LocString TOOLTIP = "This rocket has entered autopilot mode and will fly at reduced speed\n\nIt can resume full speed once a Duplicant pilot takes over";
			}

			// Token: 0x020028FD RID: 10493
			public class ROCKETCHECKLISTINCOMPLETE
			{
				// Token: 0x0400B103 RID: 45315
				public static LocString NAME = "Launch Checklist Incomplete";

				// Token: 0x0400B104 RID: 45316
				public static LocString TOOLTIP = "Critical launch tasks uncompleted\n\nRefer to the Launch Checklist in the status panel";
			}

			// Token: 0x020028FE RID: 10494
			public class ROCKETCARGOEMPTYING
			{
				// Token: 0x0400B105 RID: 45317
				public static LocString NAME = "Unloading Cargo";

				// Token: 0x0400B106 RID: 45318
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Rocket cargo is being unloaded into the ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					"\n\nLoading of new cargo will begin once unloading is complete"
				});
			}

			// Token: 0x020028FF RID: 10495
			public class ROCKETCARGOFILLING
			{
				// Token: 0x0400B107 RID: 45319
				public static LocString NAME = "Loading Cargo";

				// Token: 0x0400B108 RID: 45320
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Cargo is being loaded onto the rocket from the ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					"\n\nRocket cargo will be ready for launch once loading is complete"
				});
			}

			// Token: 0x02002900 RID: 10496
			public class ROCKETCARGOFULL
			{
				// Token: 0x0400B109 RID: 45321
				public static LocString NAME = "Platform Ready";

				// Token: 0x0400B10A RID: 45322
				public static LocString TOOLTIP = "All cargo operations are complete";
			}

			// Token: 0x02002901 RID: 10497
			public class FLIGHTALLCARGOFULL
			{
				// Token: 0x0400B10B RID: 45323
				public static LocString NAME = "All cargo bays are full";

				// Token: 0x0400B10C RID: 45324
				public static LocString TOOLTIP = "Rocket cannot store any more materials";
			}

			// Token: 0x02002902 RID: 10498
			public class FLIGHTCARGOREMAINING
			{
				// Token: 0x0400B10D RID: 45325
				public static LocString NAME = "Cargo capacity remaining: {0}";

				// Token: 0x0400B10E RID: 45326
				public static LocString TOOLTIP = "Rocket can store up to {0} more materials";
			}

			// Token: 0x02002903 RID: 10499
			public class ROCKET_PORT_IDLE
			{
				// Token: 0x0400B10F RID: 45327
				public static LocString NAME = "Idle";

				// Token: 0x0400B110 RID: 45328
				public static LocString TOOLTIP = "This port is idle because there is no rocket on the connected " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;
			}

			// Token: 0x02002904 RID: 10500
			public class ROCKET_PORT_UNLOADING
			{
				// Token: 0x0400B111 RID: 45329
				public static LocString NAME = "Unloading Rocket";

				// Token: 0x0400B112 RID: 45330
				public static LocString TOOLTIP = "Resources are being unloaded from the rocket into the local network";
			}

			// Token: 0x02002905 RID: 10501
			public class ROCKET_PORT_LOADING
			{
				// Token: 0x0400B113 RID: 45331
				public static LocString NAME = "Loading Rocket";

				// Token: 0x0400B114 RID: 45332
				public static LocString TOOLTIP = "Resources are being loaded from the local network into the rocket's storage";
			}

			// Token: 0x02002906 RID: 10502
			public class ROCKET_PORT_LOADED
			{
				// Token: 0x0400B115 RID: 45333
				public static LocString NAME = "Cargo Transfer Complete";

				// Token: 0x0400B116 RID: 45334
				public static LocString TOOLTIP = "The connected rocket has either reached max capacity for this resource type, or lacks appropriate storage modules";
			}

			// Token: 0x02002907 RID: 10503
			public class CONNECTED_ROCKET_PORT
			{
				// Token: 0x0400B117 RID: 45335
				public static LocString NAME = "Port Network Attached";

				// Token: 0x0400B118 RID: 45336
				public static LocString TOOLTIP = "This module has been connected to a " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + " and can now load and unload cargo";
			}

			// Token: 0x02002908 RID: 10504
			public class CONNECTED_ROCKET_WRONG_PORT
			{
				// Token: 0x0400B119 RID: 45337
				public static LocString NAME = "Incorrect Port Network";

				// Token: 0x0400B11A RID: 45338
				public static LocString TOOLTIP = "The attached " + BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME + " is not the correct type for this cargo module";
			}

			// Token: 0x02002909 RID: 10505
			public class CONNECTED_ROCKET_NO_PORT
			{
				// Token: 0x0400B11B RID: 45339
				public static LocString NAME = "No Rocket Ports";

				// Token: 0x0400B11C RID: 45340
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Rocket Platform",
					UI.PST_KEYWORD,
					" has no ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME,
					" attached\n\n",
					UI.PRE_KEYWORD,
					"Solid",
					UI.PST_KEYWORD,
					", ",
					UI.PRE_KEYWORD,
					"Gas",
					UI.PST_KEYWORD,
					", and ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" ",
					BUILDINGS.PREFABS.MODULARLAUNCHPADPORT.NAME_PLURAL,
					" can be attached to load and unload cargo from a landed rocket's modules"
				});
			}

			// Token: 0x0200290A RID: 10506
			public class CLUSTERTELESCOPEALLWORKCOMPLETE
			{
				// Token: 0x0400B11D RID: 45341
				public static LocString NAME = "Area Complete";

				// Token: 0x0400B11E RID: 45342
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This ",
					UI.PRE_KEYWORD,
					"Telescope",
					UI.PST_KEYWORD,
					" has analyzed all the space visible from its current location"
				});
			}

			// Token: 0x0200290B RID: 10507
			public class ROCKETPLATFORMCLOSETOCEILING
			{
				// Token: 0x0400B11F RID: 45343
				public static LocString NAME = "Low Clearance: {distance} Tiles";

				// Token: 0x0400B120 RID: 45344
				public static LocString TOOLTIP = "Tall rockets may not be able to land on this " + UI.PRE_KEYWORD + "Rocket Platform" + UI.PST_KEYWORD;
			}

			// Token: 0x0200290C RID: 10508
			public class MODULEGENERATORNOTPOWERED
			{
				// Token: 0x0400B121 RID: 45345
				public static LocString NAME = "Thrust Generation: {ActiveWattage}/{MaxWattage}";

				// Token: 0x0400B122 RID: 45346
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Engine will generate ",
					UI.FormatAsPositiveRate("{MaxWattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" once traveling through space\n\nRight now, it's not doing much of anything"
				});
			}

			// Token: 0x0200290D RID: 10509
			public class MODULEGENERATORPOWERED
			{
				// Token: 0x0400B123 RID: 45347
				public static LocString NAME = "Thrust Generation: {ActiveWattage}/{MaxWattage}";

				// Token: 0x0400B124 RID: 45348
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Engine is extracting ",
					UI.FormatAsPositiveRate("{MaxWattage}"),
					" of ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" from the thruster\n\nIt will continue generating power as long as it travels through space"
				});
			}

			// Token: 0x0200290E RID: 10510
			public class INORBITREQUIRED
			{
				// Token: 0x0400B125 RID: 45349
				public static LocString NAME = "Grounded";

				// Token: 0x0400B126 RID: 45350
				public static LocString TOOLTIP = "This building cannot operate from the surface of a " + UI.CLUSTERMAP.PLANETOID_KEYWORD + " and must be in space to function";
			}

			// Token: 0x0200290F RID: 10511
			public class REACTORREFUELDISABLED
			{
				// Token: 0x0400B127 RID: 45351
				public static LocString NAME = "Refuel Disabled";

				// Token: 0x0400B128 RID: 45352
				public static LocString TOOLTIP = "This building will not be refueled once its active fuel has been consumed";
			}

			// Token: 0x02002910 RID: 10512
			public class RAILGUNCOOLDOWN
			{
				// Token: 0x0400B129 RID: 45353
				public static LocString NAME = "Cleaning Rails: {timeleft}";

				// Token: 0x0400B12A RID: 45354
				public static LocString TOOLTIP = "This building automatically performs routine maintenance every {x} launches";
			}

			// Token: 0x02002911 RID: 10513
			public class FRIDGECOOLING
			{
				// Token: 0x0400B12B RID: 45355
				public static LocString NAME = "Cooling Contents: {UsedPower}";

				// Token: 0x0400B12C RID: 45356
				public static LocString TOOLTIP = "{UsedPower} of {MaxPower} are being used to cool the contents of this food storage";
			}

			// Token: 0x02002912 RID: 10514
			public class FRIDGESTEADY
			{
				// Token: 0x0400B12D RID: 45357
				public static LocString NAME = "Energy Saver: {UsedPower}";

				// Token: 0x0400B12E RID: 45358
				public static LocString TOOLTIP = "The contents of this food storage are at refrigeration temperatures\n\nEnergy Saver mode has been automatically activated using only {UsedPower} of {MaxPower}";
			}

			// Token: 0x02002913 RID: 10515
			public class TELEPHONE
			{
				// Token: 0x02003170 RID: 12656
				public class BABBLE
				{
					// Token: 0x0400C623 RID: 50723
					public static LocString NAME = "Babbling to no one.";

					// Token: 0x0400C624 RID: 50724
					public static LocString TOOLTIP = "{Duplicant} just needed to vent to into the void.";
				}

				// Token: 0x02003171 RID: 12657
				public class CONVERSATION
				{
					// Token: 0x0400C625 RID: 50725
					public static LocString TALKING_TO = "Talking to {Duplicant} on {Asteroid}";

					// Token: 0x0400C626 RID: 50726
					public static LocString TALKING_TO_NUM = "Talking to {0} friends.";
				}
			}

			// Token: 0x02002914 RID: 10516
			public class CREATUREMANIPULATORPROGRESS
			{
				// Token: 0x0400B12F RID: 45359
				public static LocString NAME = "Collected Species Data {0}/{1}";

				// Token: 0x0400B130 RID: 45360
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building requires data from multiple ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" species to unlock its genetic manipulator\n\nSpecies scanned:"
				});

				// Token: 0x0400B131 RID: 45361
				public static LocString NO_DATA = "No species scanned";
			}

			// Token: 0x02002915 RID: 10517
			public class CREATUREMANIPULATORMORPHMODELOCKED
			{
				// Token: 0x0400B132 RID: 45362
				public static LocString NAME = "Current Status: Offline";

				// Token: 0x0400B133 RID: 45363
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building cannot operate until it collects more ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" DNA"
				});
			}

			// Token: 0x02002916 RID: 10518
			public class CREATUREMANIPULATORMORPHMODE
			{
				// Token: 0x0400B134 RID: 45364
				public static LocString NAME = "Current Status: Online";

				// Token: 0x0400B135 RID: 45365
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is ready to manipulate ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" DNA"
				});
			}

			// Token: 0x02002917 RID: 10519
			public class CREATUREMANIPULATORWAITING
			{
				// Token: 0x0400B136 RID: 45366
				public static LocString NAME = "Waiting for a Critter";

				// Token: 0x0400B137 RID: 45367
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is waiting for a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" to get sucked into its scanning area"
				});
			}

			// Token: 0x02002918 RID: 10520
			public class CREATUREMANIPULATORWORKING
			{
				// Token: 0x0400B138 RID: 45368
				public static LocString NAME = "Poking and Prodding Critter";

				// Token: 0x0400B139 RID: 45369
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This building is extracting genetic information from a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" "
				});
			}

			// Token: 0x02002919 RID: 10521
			public class SPICEGRINDERNOSPICE
			{
				// Token: 0x0400B13A RID: 45370
				public static LocString NAME = "No Spice Selected";

				// Token: 0x0400B13B RID: 45371
				public static LocString TOOLTIP = "Select a recipe to begin fabrication";
			}

			// Token: 0x0200291A RID: 10522
			public class SPICEGRINDERACCEPTSMUTANTSEEDS
			{
				// Token: 0x0400B13C RID: 45372
				public static LocString NAME = "Spice Grinder accepts mutant seeds";

				// Token: 0x0400B13D RID: 45373
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This spice grinder is allowed to use ",
					UI.PRE_KEYWORD,
					"Mutant Seeds",
					UI.PST_KEYWORD,
					" as recipe ingredients"
				});
			}

			// Token: 0x0200291B RID: 10523
			public class MISSILELAUNCHER_NOSURFACESIGHT
			{
				// Token: 0x0400B13E RID: 45374
				public static LocString NAME = "No Line of Sight";

				// Token: 0x0400B13F RID: 45375
				public static LocString TOOLTIP = "This building has no view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x0200291C RID: 10524
			public class MISSILELAUNCHER_PARTIALLYBLOCKED
			{
				// Token: 0x0400B140 RID: 45376
				public static LocString NAME = "Limited Line of Sight";

				// Token: 0x0400B141 RID: 45377
				public static LocString TOOLTIP = "This building has a partially obstructed view of space\n\nTo properly function, this building requires an unblocked view of space";
			}

			// Token: 0x0200291D RID: 10525
			public class COMPLEXFABRICATOR
			{
				// Token: 0x02003172 RID: 12658
				public class COOKING
				{
					// Token: 0x0400C627 RID: 50727
					public static LocString NAME = "Cooking {Item}";

					// Token: 0x0400C628 RID: 50728
					public static LocString TOOLTIP = "This building is currently whipping up a batch of {Item}";
				}

				// Token: 0x02003173 RID: 12659
				public class PRODUCING
				{
					// Token: 0x0400C629 RID: 50729
					public static LocString NAME = "Producing {Item}";

					// Token: 0x0400C62A RID: 50730
					public static LocString TOOLTIP = "This building is carrying out its current production orders";
				}

				// Token: 0x02003174 RID: 12660
				public class RESEARCHING
				{
					// Token: 0x0400C62B RID: 50731
					public static LocString NAME = "Researching {Item}";

					// Token: 0x0400C62C RID: 50732
					public static LocString TOOLTIP = "This building is currently conducting important research";
				}

				// Token: 0x02003175 RID: 12661
				public class ANALYZYINGn
				{
					// Token: 0x0400C62D RID: 50733
					public static LocString NAME = "Analyzing {Item}";

					// Token: 0x0400C62E RID: 50734
					public static LocString TOOLTIP = "This building is currently analyzing a fascinating artifact";
				}

				// Token: 0x02003176 RID: 12662
				public class UNTRAINING
				{
					// Token: 0x0400C62F RID: 50735
					public static LocString NAME = "Untraining {Duplicant}";

					// Token: 0x0400C630 RID: 50736
					public static LocString TOOLTIP = "Restoring {Duplicant} to a blissfully ignorant state";
				}

				// Token: 0x02003177 RID: 12663
				public class TELESCOPE
				{
					// Token: 0x0400C631 RID: 50737
					public static LocString NAME = "Studying Space";

					// Token: 0x0400C632 RID: 50738
					public static LocString TOOLTIP = "This building is currently investigating the mysteries of space";
				}

				// Token: 0x02003178 RID: 12664
				public class CLUSTERTELESCOPEMETEOR
				{
					// Token: 0x0400C633 RID: 50739
					public static LocString NAME = "Studying Meteor";

					// Token: 0x0400C634 RID: 50740
					public static LocString TOOLTIP = "This building is currently studying a meteor";
				}
			}
		}

		// Token: 0x02001DE0 RID: 7648
		public class DETAILS
		{
			// Token: 0x04008923 RID: 35107
			public static LocString USE_COUNT = "Uses: {0}";

			// Token: 0x04008924 RID: 35108
			public static LocString USE_COUNT_TOOLTIP = "This building has been used {0} times";
		}
	}
}
