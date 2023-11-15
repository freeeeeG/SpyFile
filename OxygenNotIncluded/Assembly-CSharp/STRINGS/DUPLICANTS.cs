using System;
using TUNING;

namespace STRINGS
{
	// Token: 0x02000DB1 RID: 3505
	public class DUPLICANTS
	{
		// Token: 0x04005127 RID: 20775
		public static LocString RACE_PREFIX = "Species: {0}";

		// Token: 0x04005128 RID: 20776
		public static LocString RACE = "Duplicant";

		// Token: 0x04005129 RID: 20777
		public static LocString NAMETITLE = "Name: ";

		// Token: 0x0400512A RID: 20778
		public static LocString GENDERTITLE = "Gender: ";

		// Token: 0x0400512B RID: 20779
		public static LocString ARRIVALTIME = "Age: ";

		// Token: 0x0400512C RID: 20780
		public static LocString ARRIVALTIME_TOOLTIP = "This {1} was printed on <b>Cycle {0}</b>";

		// Token: 0x0400512D RID: 20781
		public static LocString DESC_TOOLTIP = "About {0}s";

		// Token: 0x02001DF4 RID: 7668
		public class GENDER
		{
			// Token: 0x02002AB1 RID: 10929
			public class MALE
			{
				// Token: 0x0400B500 RID: 46336
				public static LocString NAME = "M";

				// Token: 0x020031D3 RID: 12755
				public class PLURALS
				{
					// Token: 0x0400C701 RID: 50945
					public static LocString ONE = "he";

					// Token: 0x0400C702 RID: 50946
					public static LocString TWO = "his";
				}
			}

			// Token: 0x02002AB2 RID: 10930
			public class FEMALE
			{
				// Token: 0x0400B501 RID: 46337
				public static LocString NAME = "F";

				// Token: 0x020031D4 RID: 12756
				public class PLURALS
				{
					// Token: 0x0400C703 RID: 50947
					public static LocString ONE = "she";

					// Token: 0x0400C704 RID: 50948
					public static LocString TWO = "her";
				}
			}

			// Token: 0x02002AB3 RID: 10931
			public class NB
			{
				// Token: 0x0400B502 RID: 46338
				public static LocString NAME = "X";

				// Token: 0x020031D5 RID: 12757
				public class PLURALS
				{
					// Token: 0x0400C705 RID: 50949
					public static LocString ONE = "they";

					// Token: 0x0400C706 RID: 50950
					public static LocString TWO = "their";
				}
			}
		}

		// Token: 0x02001DF5 RID: 7669
		public class STATS
		{
			// Token: 0x02002AB4 RID: 10932
			public class SUBJECTS
			{
				// Token: 0x0400B503 RID: 46339
				public static LocString DUPLICANT = "Duplicant";

				// Token: 0x0400B504 RID: 46340
				public static LocString DUPLICANT_POSSESSIVE = "Duplicant's";

				// Token: 0x0400B505 RID: 46341
				public static LocString DUPLICANT_PLURAL = "Duplicants";

				// Token: 0x0400B506 RID: 46342
				public static LocString CREATURE = "critter";

				// Token: 0x0400B507 RID: 46343
				public static LocString CREATURE_POSSESSIVE = "critter's";

				// Token: 0x0400B508 RID: 46344
				public static LocString CREATURE_PLURAL = "critters";

				// Token: 0x0400B509 RID: 46345
				public static LocString PLANT = "plant";

				// Token: 0x0400B50A RID: 46346
				public static LocString PLANT_POSESSIVE = "plant's";

				// Token: 0x0400B50B RID: 46347
				public static LocString PLANT_PLURAL = "plants";
			}

			// Token: 0x02002AB5 RID: 10933
			public class BREATH
			{
				// Token: 0x0400B50C RID: 46348
				public static LocString NAME = "Breath";

				// Token: 0x0400B50D RID: 46349
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A Duplicant with zero remaining ",
					UI.PRE_KEYWORD,
					"Breath",
					UI.PST_KEYWORD,
					" will begin suffocating"
				});
			}

			// Token: 0x02002AB6 RID: 10934
			public class STAMINA
			{
				// Token: 0x0400B50E RID: 46350
				public static LocString NAME = "Stamina";

				// Token: 0x0400B50F RID: 46351
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will pass out from fatigue when ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					" reaches zero"
				});
			}

			// Token: 0x02002AB7 RID: 10935
			public class CALORIES
			{
				// Token: 0x0400B510 RID: 46352
				public static LocString NAME = "Calories";

				// Token: 0x0400B511 RID: 46353
				public static LocString TOOLTIP = "This {1} can burn <b>{0}</b> before starving";
			}

			// Token: 0x02002AB8 RID: 10936
			public class TEMPERATURE
			{
				// Token: 0x0400B512 RID: 46354
				public static LocString NAME = "Body Temperature";

				// Token: 0x0400B513 RID: 46355
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A healthy Duplicant's ",
					UI.PRE_KEYWORD,
					"Body Temperature",
					UI.PST_KEYWORD,
					" is <b>{1}</b>"
				});

				// Token: 0x0400B514 RID: 46356
				public static LocString TOOLTIP_DOMESTICATEDCRITTER = string.Concat(new string[]
				{
					"This critter's ",
					UI.PRE_KEYWORD,
					"Body Temperature",
					UI.PST_KEYWORD,
					" is <b>{1}</b>"
				});
			}

			// Token: 0x02002AB9 RID: 10937
			public class EXTERNALTEMPERATURE
			{
				// Token: 0x0400B515 RID: 46357
				public static LocString NAME = "External Temperature";

				// Token: 0x0400B516 RID: 46358
				public static LocString TOOLTIP = "This Duplicant's environment is <b>{0}</b>";
			}

			// Token: 0x02002ABA RID: 10938
			public class DECOR
			{
				// Token: 0x0400B517 RID: 46359
				public static LocString NAME = "Decor";

				// Token: 0x0400B518 RID: 46360
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants become stressed in areas with ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" lower than their expectations\n\nOpen the ",
					UI.FormatAsOverlay("Decor Overlay", global::Action.Overlay8),
					" to view current ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values"
				});

				// Token: 0x0400B519 RID: 46361
				public static LocString TOOLTIP_CURRENT = "\n\nCurrent Environmental Decor: <b>{0}</b>";

				// Token: 0x0400B51A RID: 46362
				public static LocString TOOLTIP_AVERAGE_TODAY = "\nAverage Decor This Cycle: <b>{0}</b>";

				// Token: 0x0400B51B RID: 46363
				public static LocString TOOLTIP_AVERAGE_YESTERDAY = "\nAverage Decor Last Cycle: <b>{0}</b>";
			}

			// Token: 0x02002ABB RID: 10939
			public class STRESS
			{
				// Token: 0x0400B51C RID: 46364
				public static LocString NAME = "Stress";

				// Token: 0x0400B51D RID: 46365
				public static LocString TOOLTIP = "Duplicants exhibit their Stress Reactions at one hundred percent " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02002ABC RID: 10940
			public class RADIATIONBALANCE
			{
				// Token: 0x0400B51E RID: 46366
				public static LocString NAME = "Absorbed Rad Dose";

				// Token: 0x0400B51F RID: 46367
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants accumulate Rads in areas with ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" and recover when using the toilet\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay", global::Action.Overlay15),
					" to view current ",
					UI.PRE_KEYWORD,
					"Rad",
					UI.PST_KEYWORD,
					" readings"
				});

				// Token: 0x0400B520 RID: 46368
				public static LocString TOOLTIP_CURRENT_BALANCE = string.Concat(new string[]
				{
					"Duplicants accumulate Rads in areas with ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" and recover when using the toilet\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay", global::Action.Overlay15),
					" to view current ",
					UI.PRE_KEYWORD,
					"Rad",
					UI.PST_KEYWORD,
					" readings"
				});

				// Token: 0x0400B521 RID: 46369
				public static LocString CURRENT_EXPOSURE = "Current Exposure: {0}/cycle";

				// Token: 0x0400B522 RID: 46370
				public static LocString CURRENT_REJUVENATION = "Current Rejuvenation: {0}/cycle";
			}

			// Token: 0x02002ABD RID: 10941
			public class BLADDER
			{
				// Token: 0x0400B523 RID: 46371
				public static LocString NAME = "Bladder";

				// Token: 0x0400B524 RID: 46372
				public static LocString TOOLTIP = "Duplicants make \"messes\" if no toilets are available at one hundred percent " + UI.PRE_KEYWORD + "Bladder" + UI.PST_KEYWORD;
			}

			// Token: 0x02002ABE RID: 10942
			public class HITPOINTS
			{
				// Token: 0x0400B525 RID: 46373
				public static LocString NAME = "Health";

				// Token: 0x0400B526 RID: 46374
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"When Duplicants reach zero ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					" they become incapacitated and require rescuing\n\nWhen critters reach zero ",
					UI.PRE_KEYWORD,
					"Health",
					UI.PST_KEYWORD,
					", they will die immediately"
				});
			}

			// Token: 0x02002ABF RID: 10943
			public class SKIN_THICKNESS
			{
				// Token: 0x0400B527 RID: 46375
				public static LocString NAME = "Skin Thickness";
			}

			// Token: 0x02002AC0 RID: 10944
			public class SKIN_DURABILITY
			{
				// Token: 0x0400B528 RID: 46376
				public static LocString NAME = "Skin Durability";
			}

			// Token: 0x02002AC1 RID: 10945
			public class DISEASERECOVERYTIME
			{
				// Token: 0x0400B529 RID: 46377
				public static LocString NAME = "Disease Recovery";
			}

			// Token: 0x02002AC2 RID: 10946
			public class TRUNKHEALTH
			{
				// Token: 0x0400B52A RID: 46378
				public static LocString NAME = "Trunk Health";

				// Token: 0x0400B52B RID: 46379
				public static LocString TOOLTIP = "Tree branches will die if they do not have a healthy trunk to grow from";
			}
		}

		// Token: 0x02001DF6 RID: 7670
		public class DEATHS
		{
			// Token: 0x02002AC3 RID: 10947
			public class GENERIC
			{
				// Token: 0x0400B52C RID: 46380
				public static LocString NAME = "Death";

				// Token: 0x0400B52D RID: 46381
				public static LocString DESCRIPTION = "{Target} has died.";
			}

			// Token: 0x02002AC4 RID: 10948
			public class FROZEN
			{
				// Token: 0x0400B52E RID: 46382
				public static LocString NAME = "Frozen";

				// Token: 0x0400B52F RID: 46383
				public static LocString DESCRIPTION = "{Target} has frozen to death.";
			}

			// Token: 0x02002AC5 RID: 10949
			public class SUFFOCATION
			{
				// Token: 0x0400B530 RID: 46384
				public static LocString NAME = "Suffocation";

				// Token: 0x0400B531 RID: 46385
				public static LocString DESCRIPTION = "{Target} has suffocated to death.";
			}

			// Token: 0x02002AC6 RID: 10950
			public class STARVATION
			{
				// Token: 0x0400B532 RID: 46386
				public static LocString NAME = "Starvation";

				// Token: 0x0400B533 RID: 46387
				public static LocString DESCRIPTION = "{Target} has starved to death.";
			}

			// Token: 0x02002AC7 RID: 10951
			public class OVERHEATING
			{
				// Token: 0x0400B534 RID: 46388
				public static LocString NAME = "Overheated";

				// Token: 0x0400B535 RID: 46389
				public static LocString DESCRIPTION = "{Target} overheated to death.";
			}

			// Token: 0x02002AC8 RID: 10952
			public class DROWNED
			{
				// Token: 0x0400B536 RID: 46390
				public static LocString NAME = "Drowned";

				// Token: 0x0400B537 RID: 46391
				public static LocString DESCRIPTION = "{Target} has drowned.";
			}

			// Token: 0x02002AC9 RID: 10953
			public class EXPLOSION
			{
				// Token: 0x0400B538 RID: 46392
				public static LocString NAME = "Explosion";

				// Token: 0x0400B539 RID: 46393
				public static LocString DESCRIPTION = "{Target} has died in an explosion.";
			}

			// Token: 0x02002ACA RID: 10954
			public class COMBAT
			{
				// Token: 0x0400B53A RID: 46394
				public static LocString NAME = "Slain";

				// Token: 0x0400B53B RID: 46395
				public static LocString DESCRIPTION = "{Target} succumbed to their wounds after being incapacitated.";
			}

			// Token: 0x02002ACB RID: 10955
			public class FATALDISEASE
			{
				// Token: 0x0400B53C RID: 46396
				public static LocString NAME = "Succumbed to Disease";

				// Token: 0x0400B53D RID: 46397
				public static LocString DESCRIPTION = "{Target} has died of a fatal illness.";
			}

			// Token: 0x02002ACC RID: 10956
			public class RADIATION
			{
				// Token: 0x0400B53E RID: 46398
				public static LocString NAME = "Irradiated";

				// Token: 0x0400B53F RID: 46399
				public static LocString DESCRIPTION = "{Target} perished from excessive radiation exposure.";
			}

			// Token: 0x02002ACD RID: 10957
			public class HITBYHIGHENERGYPARTICLE
			{
				// Token: 0x0400B540 RID: 46400
				public static LocString NAME = "Struck by Radbolt";

				// Token: 0x0400B541 RID: 46401
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"{Target} was struck by a radioactive ",
					UI.PRE_KEYWORD,
					"Radbolt",
					UI.PST_KEYWORD,
					" and perished."
				});
			}
		}

		// Token: 0x02001DF7 RID: 7671
		public class CHORES
		{
			// Token: 0x040089CB RID: 35275
			public static LocString NOT_EXISTING_TASK = "Not Existing";

			// Token: 0x040089CC RID: 35276
			public static LocString IS_DEAD_TASK = "Dead";

			// Token: 0x02002ACE RID: 10958
			public class THINKING
			{
				// Token: 0x0400B542 RID: 46402
				public static LocString NAME = "Ponder";

				// Token: 0x0400B543 RID: 46403
				public static LocString STATUS = "Pondering";

				// Token: 0x0400B544 RID: 46404
				public static LocString TOOLTIP = "This Duplicant is mulling over what they should do next";
			}

			// Token: 0x02002ACF RID: 10959
			public class ASTRONAUT
			{
				// Token: 0x0400B545 RID: 46405
				public static LocString NAME = "Space Mission";

				// Token: 0x0400B546 RID: 46406
				public static LocString STATUS = "On space mission";

				// Token: 0x0400B547 RID: 46407
				public static LocString TOOLTIP = "This Duplicant is exploring the vast universe";
			}

			// Token: 0x02002AD0 RID: 10960
			public class DIE
			{
				// Token: 0x0400B548 RID: 46408
				public static LocString NAME = "Die";

				// Token: 0x0400B549 RID: 46409
				public static LocString STATUS = "Dying";

				// Token: 0x0400B54A RID: 46410
				public static LocString TOOLTIP = "Fare thee well, brave soul";
			}

			// Token: 0x02002AD1 RID: 10961
			public class ENTOMBED
			{
				// Token: 0x0400B54B RID: 46411
				public static LocString NAME = "Entombed";

				// Token: 0x0400B54C RID: 46412
				public static LocString STATUS = "Entombed";

				// Token: 0x0400B54D RID: 46413
				public static LocString TOOLTIP = "Entombed Duplicants are at risk of suffocating and must be dug out by others in the colony";
			}

			// Token: 0x02002AD2 RID: 10962
			public class BEINCAPACITATED
			{
				// Token: 0x0400B54E RID: 46414
				public static LocString NAME = "Incapacitated";

				// Token: 0x0400B54F RID: 46415
				public static LocString STATUS = "Dying";

				// Token: 0x0400B550 RID: 46416
				public static LocString TOOLTIP = "This Duplicant will die soon if they do not receive assistance";
			}

			// Token: 0x02002AD3 RID: 10963
			public class GENESHUFFLE
			{
				// Token: 0x0400B551 RID: 46417
				public static LocString NAME = "Use Neural Vacillator";

				// Token: 0x0400B552 RID: 46418
				public static LocString STATUS = "Using Neural Vacillator";

				// Token: 0x0400B553 RID: 46419
				public static LocString TOOLTIP = "This Duplicant is being experimented on!";
			}

			// Token: 0x02002AD4 RID: 10964
			public class MIGRATE
			{
				// Token: 0x0400B554 RID: 46420
				public static LocString NAME = "Use Teleporter";

				// Token: 0x0400B555 RID: 46421
				public static LocString STATUS = "Using Teleporter";

				// Token: 0x0400B556 RID: 46422
				public static LocString TOOLTIP = "This Duplicant's molecules are hurtling through the air!";
			}

			// Token: 0x02002AD5 RID: 10965
			public class DEBUGGOTO
			{
				// Token: 0x0400B557 RID: 46423
				public static LocString NAME = "DebugGoTo";

				// Token: 0x0400B558 RID: 46424
				public static LocString STATUS = "DebugGoTo";
			}

			// Token: 0x02002AD6 RID: 10966
			public class DISINFECT
			{
				// Token: 0x0400B559 RID: 46425
				public static LocString NAME = "Disinfect";

				// Token: 0x0400B55A RID: 46426
				public static LocString STATUS = "Going to disinfect";

				// Token: 0x0400B55B RID: 46427
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Buildings can be disinfected to remove contagious ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" from their surface"
				});
			}

			// Token: 0x02002AD7 RID: 10967
			public class EQUIPPINGSUIT
			{
				// Token: 0x0400B55C RID: 46428
				public static LocString NAME = "Equip Exosuit";

				// Token: 0x0400B55D RID: 46429
				public static LocString STATUS = "Equipping exosuit";

				// Token: 0x0400B55E RID: 46430
				public static LocString TOOLTIP = "This Duplicant is putting on protective gear";
			}

			// Token: 0x02002AD8 RID: 10968
			public class STRESSIDLE
			{
				// Token: 0x0400B55F RID: 46431
				public static LocString NAME = "Antsy";

				// Token: 0x0400B560 RID: 46432
				public static LocString STATUS = "Antsy";

				// Token: 0x0400B561 RID: 46433
				public static LocString TOOLTIP = "This Duplicant is a workaholic and gets stressed when they have nothing to do";
			}

			// Token: 0x02002AD9 RID: 10969
			public class MOVETO
			{
				// Token: 0x0400B562 RID: 46434
				public static LocString NAME = "Move to";

				// Token: 0x0400B563 RID: 46435
				public static LocString STATUS = "Moving to location";

				// Token: 0x0400B564 RID: 46436
				public static LocString TOOLTIP = "This Duplicant was manually directed to move to a specific location";
			}

			// Token: 0x02002ADA RID: 10970
			public class ROCKETENTEREXIT
			{
				// Token: 0x0400B565 RID: 46437
				public static LocString NAME = "Rocket Recrewing";

				// Token: 0x0400B566 RID: 46438
				public static LocString STATUS = "Recrewing Rocket";

				// Token: 0x0400B567 RID: 46439
				public static LocString TOOLTIP = "This Duplicant is getting into (or out of) their assigned rocket";
			}

			// Token: 0x02002ADB RID: 10971
			public class DROPUNUSEDINVENTORY
			{
				// Token: 0x0400B568 RID: 46440
				public static LocString NAME = "Drop Inventory";

				// Token: 0x0400B569 RID: 46441
				public static LocString STATUS = "Dropping unused inventory";

				// Token: 0x0400B56A RID: 46442
				public static LocString TOOLTIP = "This Duplicant is dropping carried items they no longer need";
			}

			// Token: 0x02002ADC RID: 10972
			public class PEE
			{
				// Token: 0x0400B56B RID: 46443
				public static LocString NAME = "Relieve Self";

				// Token: 0x0400B56C RID: 46444
				public static LocString STATUS = "Relieving self";

				// Token: 0x0400B56D RID: 46445
				public static LocString TOOLTIP = "This Duplicant didn't find a toilet in time. Oops";
			}

			// Token: 0x02002ADD RID: 10973
			public class BREAK_PEE
			{
				// Token: 0x0400B56E RID: 46446
				public static LocString NAME = "Downtime: Use Toilet";

				// Token: 0x0400B56F RID: 46447
				public static LocString STATUS = "Downtime: Going to use toilet";

				// Token: 0x0400B570 RID: 46448
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has scheduled ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" and is using their break to go to the toilet\n\nDuplicants have to use the toilet at least once per day"
				});
			}

			// Token: 0x02002ADE RID: 10974
			public class STRESSVOMIT
			{
				// Token: 0x0400B571 RID: 46449
				public static LocString NAME = "Stress Vomit";

				// Token: 0x0400B572 RID: 46450
				public static LocString STATUS = "Stress vomiting";

				// Token: 0x0400B573 RID: 46451
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Some people deal with ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" better than others"
				});
			}

			// Token: 0x02002ADF RID: 10975
			public class UGLY_CRY
			{
				// Token: 0x0400B574 RID: 46452
				public static LocString NAME = "Ugly Cry";

				// Token: 0x0400B575 RID: 46453
				public static LocString STATUS = "Ugly crying";

				// Token: 0x0400B576 RID: 46454
				public static LocString TOOLTIP = "This Duplicant is having a healthy cry to alleviate their " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02002AE0 RID: 10976
			public class BINGE_EAT
			{
				// Token: 0x0400B577 RID: 46455
				public static LocString NAME = "Binge Eat";

				// Token: 0x0400B578 RID: 46456
				public static LocString STATUS = "Binge eating";

				// Token: 0x0400B579 RID: 46457
				public static LocString TOOLTIP = "This Duplicant is attempting to eat their emotions due to " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02002AE1 RID: 10977
			public class BANSHEE_WAIL
			{
				// Token: 0x0400B57A RID: 46458
				public static LocString NAME = "Banshee Wail";

				// Token: 0x0400B57B RID: 46459
				public static LocString STATUS = "Wailing";

				// Token: 0x0400B57C RID: 46460
				public static LocString TOOLTIP = "This Duplicant is emitting ear-piercing shrieks to relieve pent-up " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02002AE2 RID: 10978
			public class EMOTEHIGHPRIORITY
			{
				// Token: 0x0400B57D RID: 46461
				public static LocString NAME = "Express Themselves";

				// Token: 0x0400B57E RID: 46462
				public static LocString STATUS = "Expressing themselves";

				// Token: 0x0400B57F RID: 46463
				public static LocString TOOLTIP = "This Duplicant needs a moment to express their feelings, then they'll be on their way";
			}

			// Token: 0x02002AE3 RID: 10979
			public class HUG
			{
				// Token: 0x0400B580 RID: 46464
				public static LocString NAME = "Hug";

				// Token: 0x0400B581 RID: 46465
				public static LocString STATUS = "Hugging";

				// Token: 0x0400B582 RID: 46466
				public static LocString TOOLTIP = "This Duplicant is enjoying a big warm hug";
			}

			// Token: 0x02002AE4 RID: 10980
			public class FLEE
			{
				// Token: 0x0400B583 RID: 46467
				public static LocString NAME = "Flee";

				// Token: 0x0400B584 RID: 46468
				public static LocString STATUS = "Fleeing";

				// Token: 0x0400B585 RID: 46469
				public static LocString TOOLTIP = "Run away!";
			}

			// Token: 0x02002AE5 RID: 10981
			public class RECOVERBREATH
			{
				// Token: 0x0400B586 RID: 46470
				public static LocString NAME = "Recover Breath";

				// Token: 0x0400B587 RID: 46471
				public static LocString STATUS = "Recovering breath";

				// Token: 0x0400B588 RID: 46472
				public static LocString TOOLTIP = "";
			}

			// Token: 0x02002AE6 RID: 10982
			public class MOVETOQUARANTINE
			{
				// Token: 0x0400B589 RID: 46473
				public static LocString NAME = "Move to Quarantine";

				// Token: 0x0400B58A RID: 46474
				public static LocString STATUS = "Moving to quarantine";

				// Token: 0x0400B58B RID: 46475
				public static LocString TOOLTIP = "This Duplicant will isolate themselves to keep their illness away from the colony";
			}

			// Token: 0x02002AE7 RID: 10983
			public class ATTACK
			{
				// Token: 0x0400B58C RID: 46476
				public static LocString NAME = "Attack";

				// Token: 0x0400B58D RID: 46477
				public static LocString STATUS = "Attacking";

				// Token: 0x0400B58E RID: 46478
				public static LocString TOOLTIP = "Chaaaarge!";
			}

			// Token: 0x02002AE8 RID: 10984
			public class CAPTURE
			{
				// Token: 0x0400B58F RID: 46479
				public static LocString NAME = "Wrangle";

				// Token: 0x0400B590 RID: 46480
				public static LocString STATUS = "Wrangling";

				// Token: 0x0400B591 RID: 46481
				public static LocString TOOLTIP = "Duplicants that possess the Critter Ranching Skill can wrangle most critters without traps";
			}

			// Token: 0x02002AE9 RID: 10985
			public class SINGTOEGG
			{
				// Token: 0x0400B592 RID: 46482
				public static LocString NAME = "Sing To Egg";

				// Token: 0x0400B593 RID: 46483
				public static LocString STATUS = "Singing to egg";

				// Token: 0x0400B594 RID: 46484
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A gentle lullaby from a supportive Duplicant encourages developing ",
					UI.PRE_KEYWORD,
					"Eggs",
					UI.PST_KEYWORD,
					"\n\nIncreases ",
					UI.PRE_KEYWORD,
					"Incubation Rate",
					UI.PST_KEYWORD,
					"\n\nDuplicants must possess the ",
					DUPLICANTS.ROLES.RANCHER.NAME,
					" skill to sing to an egg"
				});
			}

			// Token: 0x02002AEA RID: 10986
			public class USETOILET
			{
				// Token: 0x0400B595 RID: 46485
				public static LocString NAME = "Use Toilet";

				// Token: 0x0400B596 RID: 46486
				public static LocString STATUS = "Going to use toilet";

				// Token: 0x0400B597 RID: 46487
				public static LocString TOOLTIP = "Duplicants have to use the toilet at least once per day";
			}

			// Token: 0x02002AEB RID: 10987
			public class WASHHANDS
			{
				// Token: 0x0400B598 RID: 46488
				public static LocString NAME = "Wash Hands";

				// Token: 0x0400B599 RID: 46489
				public static LocString STATUS = "Washing hands";

				// Token: 0x0400B59A RID: 46490
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Good hygiene removes ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" and prevents the spread of ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002AEC RID: 10988
			public class CHECKPOINT
			{
				// Token: 0x0400B59B RID: 46491
				public static LocString NAME = "Wait at Checkpoint";

				// Token: 0x0400B59C RID: 46492
				public static LocString STATUS = "Waiting at Checkpoint";

				// Token: 0x0400B59D RID: 46493
				public static LocString TOOLTIP = "This Duplicant is waiting for permission to pass";
			}

			// Token: 0x02002AED RID: 10989
			public class TRAVELTUBEENTRANCE
			{
				// Token: 0x0400B59E RID: 46494
				public static LocString NAME = "Enter Transit Tube";

				// Token: 0x0400B59F RID: 46495
				public static LocString STATUS = "Entering Transit Tube";

				// Token: 0x0400B5A0 RID: 46496
				public static LocString TOOLTIP = "Nyoooom!";
			}

			// Token: 0x02002AEE RID: 10990
			public class SCRUBORE
			{
				// Token: 0x0400B5A1 RID: 46497
				public static LocString NAME = "Scrub Ore";

				// Token: 0x0400B5A2 RID: 46498
				public static LocString STATUS = "Scrubbing ore";

				// Token: 0x0400B5A3 RID: 46499
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Material ore can be scrubbed to remove ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" present on its surface"
				});
			}

			// Token: 0x02002AEF RID: 10991
			public class EAT
			{
				// Token: 0x0400B5A4 RID: 46500
				public static LocString NAME = "Eat";

				// Token: 0x0400B5A5 RID: 46501
				public static LocString STATUS = "Going to eat";

				// Token: 0x0400B5A6 RID: 46502
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants eat to replenish their ",
					UI.PRE_KEYWORD,
					"Calorie",
					UI.PST_KEYWORD,
					" stores"
				});
			}

			// Token: 0x02002AF0 RID: 10992
			public class VOMIT
			{
				// Token: 0x0400B5A7 RID: 46503
				public static LocString NAME = "Vomit";

				// Token: 0x0400B5A8 RID: 46504
				public static LocString STATUS = "Vomiting";

				// Token: 0x0400B5A9 RID: 46505
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Vomiting produces ",
					ELEMENTS.DIRTYWATER.NAME,
					" and can spread ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002AF1 RID: 10993
			public class RADIATIONPAIN
			{
				// Token: 0x0400B5AA RID: 46506
				public static LocString NAME = "Radiation Aches";

				// Token: 0x0400B5AB RID: 46507
				public static LocString STATUS = "Feeling radiation aches";

				// Token: 0x0400B5AC RID: 46508
				public static LocString TOOLTIP = "Radiation Aches are a symptom of " + DUPLICANTS.DISEASES.RADIATIONSICKNESS.NAME;
			}

			// Token: 0x02002AF2 RID: 10994
			public class COUGH
			{
				// Token: 0x0400B5AD RID: 46509
				public static LocString NAME = "Cough";

				// Token: 0x0400B5AE RID: 46510
				public static LocString STATUS = "Coughing";

				// Token: 0x0400B5AF RID: 46511
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Coughing is a symptom of ",
					DUPLICANTS.DISEASES.SLIMESICKNESS.NAME,
					" and spreads airborne ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002AF3 RID: 10995
			public class SLEEP
			{
				// Token: 0x0400B5B0 RID: 46512
				public static LocString NAME = "Sleep";

				// Token: 0x0400B5B1 RID: 46513
				public static LocString STATUS = "Sleeping";

				// Token: 0x0400B5B2 RID: 46514
				public static LocString TOOLTIP = "Zzzzzz...";
			}

			// Token: 0x02002AF4 RID: 10996
			public class NARCOLEPSY
			{
				// Token: 0x0400B5B3 RID: 46515
				public static LocString NAME = "Narcoleptic Nap";

				// Token: 0x0400B5B4 RID: 46516
				public static LocString STATUS = "Narcoleptic napping";

				// Token: 0x0400B5B5 RID: 46517
				public static LocString TOOLTIP = "Zzzzzz...";
			}

			// Token: 0x02002AF5 RID: 10997
			public class FLOORSLEEP
			{
				// Token: 0x0400B5B6 RID: 46518
				public static LocString NAME = "Sleep on Floor";

				// Token: 0x0400B5B7 RID: 46519
				public static LocString STATUS = "Sleeping on floor";

				// Token: 0x0400B5B8 RID: 46520
				public static LocString TOOLTIP = "Zzzzzz...\n\nSleeping on the floor will give Duplicants a " + DUPLICANTS.MODIFIERS.SOREBACK.NAME;
			}

			// Token: 0x02002AF6 RID: 10998
			public class TAKEMEDICINE
			{
				// Token: 0x0400B5B9 RID: 46521
				public static LocString NAME = "Take Medicine";

				// Token: 0x0400B5BA RID: 46522
				public static LocString STATUS = "Taking medicine";

				// Token: 0x0400B5BB RID: 46523
				public static LocString TOOLTIP = "This Duplicant is taking a dose of medicine to ward off " + UI.PRE_KEYWORD + "Disease" + UI.PST_KEYWORD;
			}

			// Token: 0x02002AF7 RID: 10999
			public class GETDOCTORED
			{
				// Token: 0x0400B5BC RID: 46524
				public static LocString NAME = "Visit Doctor";

				// Token: 0x0400B5BD RID: 46525
				public static LocString STATUS = "Visiting doctor";

				// Token: 0x0400B5BE RID: 46526
				public static LocString TOOLTIP = "This Duplicant is visiting a doctor to receive treatment";
			}

			// Token: 0x02002AF8 RID: 11000
			public class DOCTOR
			{
				// Token: 0x0400B5BF RID: 46527
				public static LocString NAME = "Treat Patient";

				// Token: 0x0400B5C0 RID: 46528
				public static LocString STATUS = "Treating patient";

				// Token: 0x0400B5C1 RID: 46529
				public static LocString TOOLTIP = "This Duplicant is trying to make one of their peers feel better";
			}

			// Token: 0x02002AF9 RID: 11001
			public class DELIVERFOOD
			{
				// Token: 0x0400B5C2 RID: 46530
				public static LocString NAME = "Deliver Food";

				// Token: 0x0400B5C3 RID: 46531
				public static LocString STATUS = "Delivering food";

				// Token: 0x0400B5C4 RID: 46532
				public static LocString TOOLTIP = "Under thirty minutes or it's free";
			}

			// Token: 0x02002AFA RID: 11002
			public class SHOWER
			{
				// Token: 0x0400B5C5 RID: 46533
				public static LocString NAME = "Shower";

				// Token: 0x0400B5C6 RID: 46534
				public static LocString STATUS = "Showering";

				// Token: 0x0400B5C7 RID: 46535
				public static LocString TOOLTIP = "This Duplicant is having a refreshing shower";
			}

			// Token: 0x02002AFB RID: 11003
			public class SIGH
			{
				// Token: 0x0400B5C8 RID: 46536
				public static LocString NAME = "Sigh";

				// Token: 0x0400B5C9 RID: 46537
				public static LocString STATUS = "Sighing";

				// Token: 0x0400B5CA RID: 46538
				public static LocString TOOLTIP = "Ho-hum.";
			}

			// Token: 0x02002AFC RID: 11004
			public class RESTDUETODISEASE
			{
				// Token: 0x0400B5CB RID: 46539
				public static LocString NAME = "Rest";

				// Token: 0x0400B5CC RID: 46540
				public static LocString STATUS = "Resting";

				// Token: 0x0400B5CD RID: 46541
				public static LocString TOOLTIP = "This Duplicant isn't feeling well and is taking a rest";
			}

			// Token: 0x02002AFD RID: 11005
			public class HEAL
			{
				// Token: 0x0400B5CE RID: 46542
				public static LocString NAME = "Heal";

				// Token: 0x0400B5CF RID: 46543
				public static LocString STATUS = "Healing";

				// Token: 0x0400B5D0 RID: 46544
				public static LocString TOOLTIP = "This Duplicant is taking some time to recover from their wounds";
			}

			// Token: 0x02002AFE RID: 11006
			public class STRESSACTINGOUT
			{
				// Token: 0x0400B5D1 RID: 46545
				public static LocString NAME = "Lash Out";

				// Token: 0x0400B5D2 RID: 46546
				public static LocString STATUS = "Lashing out";

				// Token: 0x0400B5D3 RID: 46547
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is having a ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					"-induced tantrum"
				});
			}

			// Token: 0x02002AFF RID: 11007
			public class RELAX
			{
				// Token: 0x0400B5D4 RID: 46548
				public static LocString NAME = "Relax";

				// Token: 0x0400B5D5 RID: 46549
				public static LocString STATUS = "Relaxing";

				// Token: 0x0400B5D6 RID: 46550
				public static LocString TOOLTIP = "This Duplicant is taking it easy";
			}

			// Token: 0x02002B00 RID: 11008
			public class STRESSHEAL
			{
				// Token: 0x0400B5D7 RID: 46551
				public static LocString NAME = "De-Stress";

				// Token: 0x0400B5D8 RID: 46552
				public static LocString STATUS = "De-stressing";

				// Token: 0x0400B5D9 RID: 46553
				public static LocString TOOLTIP = "This Duplicant taking some time to recover from their " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B01 RID: 11009
			public class EQUIP
			{
				// Token: 0x0400B5DA RID: 46554
				public static LocString NAME = "Equip";

				// Token: 0x0400B5DB RID: 46555
				public static LocString STATUS = "Moving to equip";

				// Token: 0x0400B5DC RID: 46556
				public static LocString TOOLTIP = "This Duplicant is putting on a piece of equipment";
			}

			// Token: 0x02002B02 RID: 11010
			public class LEARNSKILL
			{
				// Token: 0x0400B5DD RID: 46557
				public static LocString NAME = "Learn Skill";

				// Token: 0x0400B5DE RID: 46558
				public static LocString STATUS = "Learning skill";

				// Token: 0x0400B5DF RID: 46559
				public static LocString TOOLTIP = "This Duplicant is learning a new " + UI.PRE_KEYWORD + "Skill" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B03 RID: 11011
			public class UNLEARNSKILL
			{
				// Token: 0x0400B5E0 RID: 46560
				public static LocString NAME = "Unlearn Skills";

				// Token: 0x0400B5E1 RID: 46561
				public static LocString STATUS = "Unlearning skills";

				// Token: 0x0400B5E2 RID: 46562
				public static LocString TOOLTIP = "This Duplicant is unlearning " + UI.PRE_KEYWORD + "Skills" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B04 RID: 11012
			public class RECHARGE
			{
				// Token: 0x0400B5E3 RID: 46563
				public static LocString NAME = "Recharge Equipment";

				// Token: 0x0400B5E4 RID: 46564
				public static LocString STATUS = "Recharging equipment";

				// Token: 0x0400B5E5 RID: 46565
				public static LocString TOOLTIP = "This Duplicant is recharging their equipment";
			}

			// Token: 0x02002B05 RID: 11013
			public class UNEQUIP
			{
				// Token: 0x0400B5E6 RID: 46566
				public static LocString NAME = "Unequip";

				// Token: 0x0400B5E7 RID: 46567
				public static LocString STATUS = "Moving to unequip";

				// Token: 0x0400B5E8 RID: 46568
				public static LocString TOOLTIP = "This Duplicant is removing a piece of their equipment";
			}

			// Token: 0x02002B06 RID: 11014
			public class MOURN
			{
				// Token: 0x0400B5E9 RID: 46569
				public static LocString NAME = "Mourn";

				// Token: 0x0400B5EA RID: 46570
				public static LocString STATUS = "Mourning";

				// Token: 0x0400B5EB RID: 46571
				public static LocString TOOLTIP = "This Duplicant is mourning the loss of a friend";
			}

			// Token: 0x02002B07 RID: 11015
			public class WARMUP
			{
				// Token: 0x0400B5EC RID: 46572
				public static LocString NAME = "Warm Up";

				// Token: 0x0400B5ED RID: 46573
				public static LocString STATUS = "Going to warm up";

				// Token: 0x0400B5EE RID: 46574
				public static LocString TOOLTIP = "This Duplicant got too cold and is going somewhere to warm up";
			}

			// Token: 0x02002B08 RID: 11016
			public class COOLDOWN
			{
				// Token: 0x0400B5EF RID: 46575
				public static LocString NAME = "Cool Off";

				// Token: 0x0400B5F0 RID: 46576
				public static LocString STATUS = "Going to cool off";

				// Token: 0x0400B5F1 RID: 46577
				public static LocString TOOLTIP = "This Duplicant got too hot and is going somewhere to cool off";
			}

			// Token: 0x02002B09 RID: 11017
			public class EMPTYSTORAGE
			{
				// Token: 0x0400B5F2 RID: 46578
				public static LocString NAME = "Empty Storage";

				// Token: 0x0400B5F3 RID: 46579
				public static LocString STATUS = "Going to empty storage";

				// Token: 0x0400B5F4 RID: 46580
				public static LocString TOOLTIP = "This Duplicant is taking items out of storage";
			}

			// Token: 0x02002B0A RID: 11018
			public class ART
			{
				// Token: 0x0400B5F5 RID: 46581
				public static LocString NAME = "Decorate";

				// Token: 0x0400B5F6 RID: 46582
				public static LocString STATUS = "Going to decorate";

				// Token: 0x0400B5F7 RID: 46583
				public static LocString TOOLTIP = "This Duplicant is going to work on their art";
			}

			// Token: 0x02002B0B RID: 11019
			public class MOP
			{
				// Token: 0x0400B5F8 RID: 46584
				public static LocString NAME = "Mop";

				// Token: 0x0400B5F9 RID: 46585
				public static LocString STATUS = "Going to mop";

				// Token: 0x0400B5FA RID: 46586
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Mopping removes ",
					UI.PRE_KEYWORD,
					"Liquid",
					UI.PST_KEYWORD,
					" from the floor and bottles them for transport"
				});
			}

			// Token: 0x02002B0C RID: 11020
			public class RELOCATE
			{
				// Token: 0x0400B5FB RID: 46587
				public static LocString NAME = "Relocate";

				// Token: 0x0400B5FC RID: 46588
				public static LocString STATUS = "Going to relocate";

				// Token: 0x0400B5FD RID: 46589
				public static LocString TOOLTIP = "This Duplicant is moving a building to a new location";
			}

			// Token: 0x02002B0D RID: 11021
			public class TOGGLE
			{
				// Token: 0x0400B5FE RID: 46590
				public static LocString NAME = "Change Setting";

				// Token: 0x0400B5FF RID: 46591
				public static LocString STATUS = "Going to change setting";

				// Token: 0x0400B600 RID: 46592
				public static LocString TOOLTIP = "This Duplicant is going to change the settings on a building";
			}

			// Token: 0x02002B0E RID: 11022
			public class RESCUEINCAPACITATED
			{
				// Token: 0x0400B601 RID: 46593
				public static LocString NAME = "Rescue Friend";

				// Token: 0x0400B602 RID: 46594
				public static LocString STATUS = "Rescuing friend";

				// Token: 0x0400B603 RID: 46595
				public static LocString TOOLTIP = "This Duplicant is rescuing another Duplicant that has been incapacitated";
			}

			// Token: 0x02002B0F RID: 11023
			public class REPAIR
			{
				// Token: 0x0400B604 RID: 46596
				public static LocString NAME = "Repair";

				// Token: 0x0400B605 RID: 46597
				public static LocString STATUS = "Going to repair";

				// Token: 0x0400B606 RID: 46598
				public static LocString TOOLTIP = "This Duplicant is fixing a broken building";
			}

			// Token: 0x02002B10 RID: 11024
			public class DECONSTRUCT
			{
				// Token: 0x0400B607 RID: 46599
				public static LocString NAME = "Deconstruct";

				// Token: 0x0400B608 RID: 46600
				public static LocString STATUS = "Going to deconstruct";

				// Token: 0x0400B609 RID: 46601
				public static LocString TOOLTIP = "This Duplicant is deconstructing a building";
			}

			// Token: 0x02002B11 RID: 11025
			public class RESEARCH
			{
				// Token: 0x0400B60A RID: 46602
				public static LocString NAME = "Research";

				// Token: 0x0400B60B RID: 46603
				public static LocString STATUS = "Going to research";

				// Token: 0x0400B60C RID: 46604
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is working on the current ",
					UI.PRE_KEYWORD,
					"Research",
					UI.PST_KEYWORD,
					" focus"
				});
			}

			// Token: 0x02002B12 RID: 11026
			public class ANALYZEARTIFACT
			{
				// Token: 0x0400B60D RID: 46605
				public static LocString NAME = "Artifact Analysis";

				// Token: 0x0400B60E RID: 46606
				public static LocString STATUS = "Going to analyze artifacts";

				// Token: 0x0400B60F RID: 46607
				public static LocString TOOLTIP = "This Duplicant is analyzing " + UI.PRE_KEYWORD + "Artifacts" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B13 RID: 11027
			public class ANALYZESEED
			{
				// Token: 0x0400B610 RID: 46608
				public static LocString NAME = "Seed Analysis";

				// Token: 0x0400B611 RID: 46609
				public static LocString STATUS = "Going to analyze seeds";

				// Token: 0x0400B612 RID: 46610
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is analyzing ",
					UI.PRE_KEYWORD,
					"Seeds",
					UI.PST_KEYWORD,
					" to find mutations"
				});
			}

			// Token: 0x02002B14 RID: 11028
			public class RETURNSUIT
			{
				// Token: 0x0400B613 RID: 46611
				public static LocString NAME = "Dock Exosuit";

				// Token: 0x0400B614 RID: 46612
				public static LocString STATUS = "Docking exosuit";

				// Token: 0x0400B615 RID: 46613
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is plugging an ",
					UI.PRE_KEYWORD,
					"Exosuit",
					UI.PST_KEYWORD,
					" in for refilling"
				});
			}

			// Token: 0x02002B15 RID: 11029
			public class GENERATEPOWER
			{
				// Token: 0x0400B616 RID: 46614
				public static LocString NAME = "Generate Power";

				// Token: 0x0400B617 RID: 46615
				public static LocString STATUS = "Going to generate power";

				// Token: 0x0400B618 RID: 46616
				public static LocString TOOLTIP = "This Duplicant is producing electrical " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B16 RID: 11030
			public class HARVEST
			{
				// Token: 0x0400B619 RID: 46617
				public static LocString NAME = "Harvest";

				// Token: 0x0400B61A RID: 46618
				public static LocString STATUS = "Going to harvest";

				// Token: 0x0400B61B RID: 46619
				public static LocString TOOLTIP = "This Duplicant is harvesting usable materials from a mature " + UI.PRE_KEYWORD + "Plant" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B17 RID: 11031
			public class UPROOT
			{
				// Token: 0x0400B61C RID: 46620
				public static LocString NAME = "Uproot";

				// Token: 0x0400B61D RID: 46621
				public static LocString STATUS = "Going to uproot";

				// Token: 0x0400B61E RID: 46622
				public static LocString TOOLTIP = "This Duplicant is uprooting a plant to retrieve a " + UI.PRE_KEYWORD + "Seed" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B18 RID: 11032
			public class CLEANTOILET
			{
				// Token: 0x0400B61F RID: 46623
				public static LocString NAME = "Clean Outhouse";

				// Token: 0x0400B620 RID: 46624
				public static LocString STATUS = "Going to clean";

				// Token: 0x0400B621 RID: 46625
				public static LocString TOOLTIP = "This Duplicant is cleaning out the " + BUILDINGS.PREFABS.OUTHOUSE.NAME;
			}

			// Token: 0x02002B19 RID: 11033
			public class EMPTYDESALINATOR
			{
				// Token: 0x0400B622 RID: 46626
				public static LocString NAME = "Empty Desalinator";

				// Token: 0x0400B623 RID: 46627
				public static LocString STATUS = "Going to clean";

				// Token: 0x0400B624 RID: 46628
				public static LocString TOOLTIP = "This Duplicant is emptying out the " + BUILDINGS.PREFABS.DESALINATOR.NAME;
			}

			// Token: 0x02002B1A RID: 11034
			public class LIQUIDCOOLEDFAN
			{
				// Token: 0x0400B625 RID: 46629
				public static LocString NAME = "Use Fan";

				// Token: 0x0400B626 RID: 46630
				public static LocString STATUS = "Going to use fan";

				// Token: 0x0400B627 RID: 46631
				public static LocString TOOLTIP = "This Duplicant is attempting to cool down the area";
			}

			// Token: 0x02002B1B RID: 11035
			public class ICECOOLEDFAN
			{
				// Token: 0x0400B628 RID: 46632
				public static LocString NAME = "Use Fan";

				// Token: 0x0400B629 RID: 46633
				public static LocString STATUS = "Going to use fan";

				// Token: 0x0400B62A RID: 46634
				public static LocString TOOLTIP = "This Duplicant is attempting to cool down the area";
			}

			// Token: 0x02002B1C RID: 11036
			public class PROCESSCRITTER
			{
				// Token: 0x0400B62B RID: 46635
				public static LocString NAME = "Process Critter";

				// Token: 0x0400B62C RID: 46636
				public static LocString STATUS = "Going to process critter";

				// Token: 0x0400B62D RID: 46637
				public static LocString TOOLTIP = "This Duplicant is processing " + UI.PRE_KEYWORD + "Critters" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B1D RID: 11037
			public class COOK
			{
				// Token: 0x0400B62E RID: 46638
				public static LocString NAME = "Cook";

				// Token: 0x0400B62F RID: 46639
				public static LocString STATUS = "Going to cook";

				// Token: 0x0400B630 RID: 46640
				public static LocString TOOLTIP = "This Duplicant is cooking " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B1E RID: 11038
			public class COMPOUND
			{
				// Token: 0x0400B631 RID: 46641
				public static LocString NAME = "Compound Medicine";

				// Token: 0x0400B632 RID: 46642
				public static LocString STATUS = "Going to compound medicine";

				// Token: 0x0400B633 RID: 46643
				public static LocString TOOLTIP = "This Duplicant is fabricating " + UI.PRE_KEYWORD + "Medicine" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B1F RID: 11039
			public class TRAIN
			{
				// Token: 0x0400B634 RID: 46644
				public static LocString NAME = "Train";

				// Token: 0x0400B635 RID: 46645
				public static LocString STATUS = "Training";

				// Token: 0x0400B636 RID: 46646
				public static LocString TOOLTIP = "This Duplicant is busy training";
			}

			// Token: 0x02002B20 RID: 11040
			public class MUSH
			{
				// Token: 0x0400B637 RID: 46647
				public static LocString NAME = "Mush";

				// Token: 0x0400B638 RID: 46648
				public static LocString STATUS = "Going to mush";

				// Token: 0x0400B639 RID: 46649
				public static LocString TOOLTIP = "This Duplicant is producing " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B21 RID: 11041
			public class COMPOSTWORKABLE
			{
				// Token: 0x0400B63A RID: 46650
				public static LocString NAME = "Compost";

				// Token: 0x0400B63B RID: 46651
				public static LocString STATUS = "Going to compost";

				// Token: 0x0400B63C RID: 46652
				public static LocString TOOLTIP = "This Duplicant is dropping off organic material at the " + BUILDINGS.PREFABS.COMPOST.NAME;
			}

			// Token: 0x02002B22 RID: 11042
			public class FLIPCOMPOST
			{
				// Token: 0x0400B63D RID: 46653
				public static LocString NAME = "Flip";

				// Token: 0x0400B63E RID: 46654
				public static LocString STATUS = "Going to flip compost";

				// Token: 0x0400B63F RID: 46655
				public static LocString TOOLTIP = BUILDINGS.PREFABS.COMPOST.NAME + "s need to be flipped in order for their contents to compost";
			}

			// Token: 0x02002B23 RID: 11043
			public class DEPRESSURIZE
			{
				// Token: 0x0400B640 RID: 46656
				public static LocString NAME = "Depressurize Well";

				// Token: 0x0400B641 RID: 46657
				public static LocString STATUS = "Going to depressurize well";

				// Token: 0x0400B642 RID: 46658
				public static LocString TOOLTIP = BUILDINGS.PREFABS.OILWELLCAP.NAME + "s need to be periodically depressurized to function";
			}

			// Token: 0x02002B24 RID: 11044
			public class FABRICATE
			{
				// Token: 0x0400B643 RID: 46659
				public static LocString NAME = "Fabricate";

				// Token: 0x0400B644 RID: 46660
				public static LocString STATUS = "Going to fabricate";

				// Token: 0x0400B645 RID: 46661
				public static LocString TOOLTIP = "This Duplicant is crafting something";
			}

			// Token: 0x02002B25 RID: 11045
			public class BUILD
			{
				// Token: 0x0400B646 RID: 46662
				public static LocString NAME = "Build";

				// Token: 0x0400B647 RID: 46663
				public static LocString STATUS = "Going to build";

				// Token: 0x0400B648 RID: 46664
				public static LocString TOOLTIP = "This Duplicant is constructing a new building";
			}

			// Token: 0x02002B26 RID: 11046
			public class BUILDDIG
			{
				// Token: 0x0400B649 RID: 46665
				public static LocString NAME = "Construction Dig";

				// Token: 0x0400B64A RID: 46666
				public static LocString STATUS = "Going to construction dig";

				// Token: 0x0400B64B RID: 46667
				public static LocString TOOLTIP = "This Duplicant is making room for a planned construction task by performing this dig";
			}

			// Token: 0x02002B27 RID: 11047
			public class DIG
			{
				// Token: 0x0400B64C RID: 46668
				public static LocString NAME = "Dig";

				// Token: 0x0400B64D RID: 46669
				public static LocString STATUS = "Going to dig";

				// Token: 0x0400B64E RID: 46670
				public static LocString TOOLTIP = "This Duplicant is digging out a tile";
			}

			// Token: 0x02002B28 RID: 11048
			public class FETCH
			{
				// Token: 0x0400B64F RID: 46671
				public static LocString NAME = "Deliver";

				// Token: 0x0400B650 RID: 46672
				public static LocString STATUS = "Delivering";

				// Token: 0x0400B651 RID: 46673
				public static LocString TOOLTIP = "This Duplicant is delivering materials where they need to go";

				// Token: 0x0400B652 RID: 46674
				public static LocString REPORT_NAME = "Deliver to {0}";
			}

			// Token: 0x02002B29 RID: 11049
			public class JOYREACTION
			{
				// Token: 0x0400B653 RID: 46675
				public static LocString NAME = "Joy Reaction";

				// Token: 0x0400B654 RID: 46676
				public static LocString STATUS = "Overjoyed";

				// Token: 0x0400B655 RID: 46677
				public static LocString TOOLTIP = "This Duplicant is taking a moment to relish in their own happiness";

				// Token: 0x0400B656 RID: 46678
				public static LocString REPORT_NAME = "Overjoyed Reaction";
			}

			// Token: 0x02002B2A RID: 11050
			public class ROCKETCONTROL
			{
				// Token: 0x0400B657 RID: 46679
				public static LocString NAME = "Rocket Control";

				// Token: 0x0400B658 RID: 46680
				public static LocString STATUS = "Controlling rocket";

				// Token: 0x0400B659 RID: 46681
				public static LocString TOOLTIP = "This Duplicant is keeping their spacecraft on course";

				// Token: 0x0400B65A RID: 46682
				public static LocString REPORT_NAME = "Rocket Control";
			}

			// Token: 0x02002B2B RID: 11051
			public class STORAGEFETCH
			{
				// Token: 0x0400B65B RID: 46683
				public static LocString NAME = "Store Materials";

				// Token: 0x0400B65C RID: 46684
				public static LocString STATUS = "Storing materials";

				// Token: 0x0400B65D RID: 46685
				public static LocString TOOLTIP = "This Duplicant is moving materials into storage for later use";

				// Token: 0x0400B65E RID: 46686
				public static LocString REPORT_NAME = "Store {0}";
			}

			// Token: 0x02002B2C RID: 11052
			public class EQUIPMENTFETCH
			{
				// Token: 0x0400B65F RID: 46687
				public static LocString NAME = "Store Equipment";

				// Token: 0x0400B660 RID: 46688
				public static LocString STATUS = "Storing equipment";

				// Token: 0x0400B661 RID: 46689
				public static LocString TOOLTIP = "This Duplicant is transporting equipment for storage";

				// Token: 0x0400B662 RID: 46690
				public static LocString REPORT_NAME = "Store {0}";
			}

			// Token: 0x02002B2D RID: 11053
			public class REPAIRFETCH
			{
				// Token: 0x0400B663 RID: 46691
				public static LocString NAME = "Repair Supply";

				// Token: 0x0400B664 RID: 46692
				public static LocString STATUS = "Supplying repair materials";

				// Token: 0x0400B665 RID: 46693
				public static LocString TOOLTIP = "This Duplicant is delivering materials to where they'll be needed to repair buildings";
			}

			// Token: 0x02002B2E RID: 11054
			public class RESEARCHFETCH
			{
				// Token: 0x0400B666 RID: 46694
				public static LocString NAME = "Research Supply";

				// Token: 0x0400B667 RID: 46695
				public static LocString STATUS = "Supplying research materials";

				// Token: 0x0400B668 RID: 46696
				public static LocString TOOLTIP = "This Duplicant is delivering materials where they'll be needed to conduct " + UI.PRE_KEYWORD + "Research" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B2F RID: 11055
			public class EXCAVATEFOSSIL
			{
				// Token: 0x0400B669 RID: 46697
				public static LocString NAME = "Excavate Fossil";

				// Token: 0x0400B66A RID: 46698
				public static LocString STATUS = "Excavating a fossil";

				// Token: 0x0400B66B RID: 46699
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is excavating a ",
					UI.PRE_KEYWORD,
					"Fossil",
					UI.PST_KEYWORD,
					" site"
				});
			}

			// Token: 0x02002B30 RID: 11056
			public class ARMTRAP
			{
				// Token: 0x0400B66C RID: 46700
				public static LocString NAME = "Arm Trap";

				// Token: 0x0400B66D RID: 46701
				public static LocString STATUS = "Arming a trap";

				// Token: 0x0400B66E RID: 46702
				public static LocString TOOLTIP = "This Duplicant is arming a trap";
			}

			// Token: 0x02002B31 RID: 11057
			public class FARMFETCH
			{
				// Token: 0x0400B66F RID: 46703
				public static LocString NAME = "Farming Supply";

				// Token: 0x0400B670 RID: 46704
				public static LocString STATUS = "Supplying farming materials";

				// Token: 0x0400B671 RID: 46705
				public static LocString TOOLTIP = "This Duplicant is delivering farming materials where they're needed to tend " + UI.PRE_KEYWORD + "Crops" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B32 RID: 11058
			public class FETCHCRITICAL
			{
				// Token: 0x0400B672 RID: 46706
				public static LocString NAME = "Life Support Supply";

				// Token: 0x0400B673 RID: 46707
				public static LocString STATUS = "Supplying critical materials";

				// Token: 0x0400B674 RID: 46708
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is delivering materials required to perform ",
					UI.PRE_KEYWORD,
					"Life Support",
					UI.PST_KEYWORD,
					" Errands"
				});

				// Token: 0x0400B675 RID: 46709
				public static LocString REPORT_NAME = "Life Support Supply to {0}";
			}

			// Token: 0x02002B33 RID: 11059
			public class MACHINEFETCH
			{
				// Token: 0x0400B676 RID: 46710
				public static LocString NAME = "Operational Supply";

				// Token: 0x0400B677 RID: 46711
				public static LocString STATUS = "Supplying operational materials";

				// Token: 0x0400B678 RID: 46712
				public static LocString TOOLTIP = "This Duplicant is delivering materials to where they'll be needed for machine operation";

				// Token: 0x0400B679 RID: 46713
				public static LocString REPORT_NAME = "Operational Supply to {0}";
			}

			// Token: 0x02002B34 RID: 11060
			public class COOKFETCH
			{
				// Token: 0x0400B67A RID: 46714
				public static LocString NAME = "Cook Supply";

				// Token: 0x0400B67B RID: 46715
				public static LocString STATUS = "Supplying cook ingredients";

				// Token: 0x0400B67C RID: 46716
				public static LocString TOOLTIP = "This Duplicant is delivering materials required to cook " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B35 RID: 11061
			public class DOCTORFETCH
			{
				// Token: 0x0400B67D RID: 46717
				public static LocString NAME = "Medical Supply";

				// Token: 0x0400B67E RID: 46718
				public static LocString STATUS = "Supplying medical resources";

				// Token: 0x0400B67F RID: 46719
				public static LocString TOOLTIP = "This Duplicant is delivering the materials that will be needed to treat sick patients";

				// Token: 0x0400B680 RID: 46720
				public static LocString REPORT_NAME = "Medical Supply to {0}";
			}

			// Token: 0x02002B36 RID: 11062
			public class FOODFETCH
			{
				// Token: 0x0400B681 RID: 46721
				public static LocString NAME = "Store Food";

				// Token: 0x0400B682 RID: 46722
				public static LocString STATUS = "Storing food";

				// Token: 0x0400B683 RID: 46723
				public static LocString TOOLTIP = "This Duplicant is moving edible resources into proper storage";

				// Token: 0x0400B684 RID: 46724
				public static LocString REPORT_NAME = "Store {0}";
			}

			// Token: 0x02002B37 RID: 11063
			public class POWERFETCH
			{
				// Token: 0x0400B685 RID: 46725
				public static LocString NAME = "Power Supply";

				// Token: 0x0400B686 RID: 46726
				public static LocString STATUS = "Supplying power materials";

				// Token: 0x0400B687 RID: 46727
				public static LocString TOOLTIP = "This Duplicant is delivering materials to where they'll be needed for " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;

				// Token: 0x0400B688 RID: 46728
				public static LocString REPORT_NAME = "Power Supply to {0}";
			}

			// Token: 0x02002B38 RID: 11064
			public class FABRICATEFETCH
			{
				// Token: 0x0400B689 RID: 46729
				public static LocString NAME = "Fabrication Supply";

				// Token: 0x0400B68A RID: 46730
				public static LocString STATUS = "Supplying fabrication materials";

				// Token: 0x0400B68B RID: 46731
				public static LocString TOOLTIP = "This Duplicant is delivering materials required to fabricate new objects";

				// Token: 0x0400B68C RID: 46732
				public static LocString REPORT_NAME = "Fabrication Supply to {0}";
			}

			// Token: 0x02002B39 RID: 11065
			public class BUILDFETCH
			{
				// Token: 0x0400B68D RID: 46733
				public static LocString NAME = "Construction Supply";

				// Token: 0x0400B68E RID: 46734
				public static LocString STATUS = "Supplying construction materials";

				// Token: 0x0400B68F RID: 46735
				public static LocString TOOLTIP = "This delivery will provide materials to a planned construction site";
			}

			// Token: 0x02002B3A RID: 11066
			public class FETCHCREATURE
			{
				// Token: 0x0400B690 RID: 46736
				public static LocString NAME = "Relocate Critter";

				// Token: 0x0400B691 RID: 46737
				public static LocString STATUS = "Relocating critter";

				// Token: 0x0400B692 RID: 46738
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is moving a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" to a new location"
				});
			}

			// Token: 0x02002B3B RID: 11067
			public class FETCHRANCHING
			{
				// Token: 0x0400B693 RID: 46739
				public static LocString NAME = "Ranching Supply";

				// Token: 0x0400B694 RID: 46740
				public static LocString STATUS = "Supplying ranching materials";

				// Token: 0x0400B695 RID: 46741
				public static LocString TOOLTIP = "This Duplicant is delivering materials for ranching activities";
			}

			// Token: 0x02002B3C RID: 11068
			public class TRANSPORT
			{
				// Token: 0x0400B696 RID: 46742
				public static LocString NAME = "Sweep";

				// Token: 0x0400B697 RID: 46743
				public static LocString STATUS = "Going to sweep";

				// Token: 0x0400B698 RID: 46744
				public static LocString TOOLTIP = "Moving debris off the ground and into storage improves colony " + UI.PRE_KEYWORD + "Decor" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B3D RID: 11069
			public class MOVETOSAFETY
			{
				// Token: 0x0400B699 RID: 46745
				public static LocString NAME = "Find Safe Area";

				// Token: 0x0400B69A RID: 46746
				public static LocString STATUS = "Finding safer area";

				// Token: 0x0400B69B RID: 46747
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is ",
					UI.PRE_KEYWORD,
					"Idle",
					UI.PST_KEYWORD,
					" and looking for somewhere safe and comfy to chill"
				});
			}

			// Token: 0x02002B3E RID: 11070
			public class PARTY
			{
				// Token: 0x0400B69C RID: 46748
				public static LocString NAME = "Party";

				// Token: 0x0400B69D RID: 46749
				public static LocString STATUS = "Partying";

				// Token: 0x0400B69E RID: 46750
				public static LocString TOOLTIP = "This Duplicant is partying hard";
			}

			// Token: 0x02002B3F RID: 11071
			public class POWER_TINKER
			{
				// Token: 0x0400B69F RID: 46751
				public static LocString NAME = "Tinker";

				// Token: 0x0400B6A0 RID: 46752
				public static LocString STATUS = "Tinkering";

				// Token: 0x0400B6A1 RID: 46753
				public static LocString TOOLTIP = "Tinkering with buildings improves their functionality";
			}

			// Token: 0x02002B40 RID: 11072
			public class RANCH
			{
				// Token: 0x0400B6A2 RID: 46754
				public static LocString NAME = "Ranch";

				// Token: 0x0400B6A3 RID: 46755
				public static LocString STATUS = "Ranching";

				// Token: 0x0400B6A4 RID: 46756
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is tending to a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"'s well-being"
				});

				// Token: 0x0400B6A5 RID: 46757
				public static LocString REPORT_NAME = "Deliver to {0}";
			}

			// Token: 0x02002B41 RID: 11073
			public class CROP_TEND
			{
				// Token: 0x0400B6A6 RID: 46758
				public static LocString NAME = "Tend";

				// Token: 0x0400B6A7 RID: 46759
				public static LocString STATUS = "Tending plant";

				// Token: 0x0400B6A8 RID: 46760
				public static LocString TOOLTIP = "Tending to plants increases their " + UI.PRE_KEYWORD + "Growth Rate" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B42 RID: 11074
			public class DEMOLISH
			{
				// Token: 0x0400B6A9 RID: 46761
				public static LocString NAME = "Demolish";

				// Token: 0x0400B6AA RID: 46762
				public static LocString STATUS = "Demolishing object";

				// Token: 0x0400B6AB RID: 46763
				public static LocString TOOLTIP = "Demolishing an object removes it permanently";
			}

			// Token: 0x02002B43 RID: 11075
			public class IDLE
			{
				// Token: 0x0400B6AC RID: 46764
				public static LocString NAME = "Idle";

				// Token: 0x0400B6AD RID: 46765
				public static LocString STATUS = "Idle";

				// Token: 0x0400B6AE RID: 46766
				public static LocString TOOLTIP = "This Duplicant cannot reach any pending " + UI.PRE_KEYWORD + "Errands" + UI.PST_KEYWORD;
			}

			// Token: 0x02002B44 RID: 11076
			public class PRECONDITIONS
			{
				// Token: 0x0400B6AF RID: 46767
				public static LocString HEADER = "The selected {Selected} could:";

				// Token: 0x0400B6B0 RID: 46768
				public static LocString SUCCESS_ROW = "{Duplicant} -- {Rank}";

				// Token: 0x0400B6B1 RID: 46769
				public static LocString CURRENT_ERRAND = "Current Errand";

				// Token: 0x0400B6B2 RID: 46770
				public static LocString RANK_FORMAT = "#{0}";

				// Token: 0x0400B6B3 RID: 46771
				public static LocString FAILURE_ROW = "{Duplicant} -- {Reason}";

				// Token: 0x0400B6B4 RID: 46772
				public static LocString CONTAINS_OXYGEN = "Not enough Oxygen";

				// Token: 0x0400B6B5 RID: 46773
				public static LocString IS_PREEMPTABLE = "Already assigned to {Assignee}";

				// Token: 0x0400B6B6 RID: 46774
				public static LocString HAS_URGE = "No current need";

				// Token: 0x0400B6B7 RID: 46775
				public static LocString IS_VALID = "Invalid";

				// Token: 0x0400B6B8 RID: 46776
				public static LocString IS_PERMITTED = "Not permitted";

				// Token: 0x0400B6B9 RID: 46777
				public static LocString IS_ASSIGNED_TO_ME = "Not assigned to {Selected}";

				// Token: 0x0400B6BA RID: 46778
				public static LocString IS_IN_MY_WORLD = "Outside world";

				// Token: 0x0400B6BB RID: 46779
				public static LocString IS_CELL_NOT_IN_MY_WORLD = "Already there";

				// Token: 0x0400B6BC RID: 46780
				public static LocString IS_IN_MY_ROOM = "Outside {Selected}'s room";

				// Token: 0x0400B6BD RID: 46781
				public static LocString IS_PREFERRED_ASSIGNABLE = "Not preferred assignment";

				// Token: 0x0400B6BE RID: 46782
				public static LocString IS_PREFERRED_ASSIGNABLE_OR_URGENT_BLADDER = "Not preferred assignment";

				// Token: 0x0400B6BF RID: 46783
				public static LocString HAS_SKILL_PERK = "Requires learned skill";

				// Token: 0x0400B6C0 RID: 46784
				public static LocString IS_MORE_SATISFYING = "Low priority";

				// Token: 0x0400B6C1 RID: 46785
				public static LocString CAN_CHAT = "Unreachable";

				// Token: 0x0400B6C2 RID: 46786
				public static LocString IS_NOT_RED_ALERT = "Unavailable in Red Alert";

				// Token: 0x0400B6C3 RID: 46787
				public static LocString NO_DEAD_BODIES = "Unburied Duplicant";

				// Token: 0x0400B6C4 RID: 46788
				public static LocString NOT_A_ROBOT = "Unavailable to Robots";

				// Token: 0x0400B6C5 RID: 46789
				public static LocString VALID_MOURNING_SITE = "Nowhere to mourn";

				// Token: 0x0400B6C6 RID: 46790
				public static LocString HAS_PLACE_TO_STAND = "Nowhere to stand";

				// Token: 0x0400B6C7 RID: 46791
				public static LocString IS_SCHEDULED_TIME = "Not allowed by schedule";

				// Token: 0x0400B6C8 RID: 46792
				public static LocString CAN_MOVE_TO = "Unreachable";

				// Token: 0x0400B6C9 RID: 46793
				public static LocString CAN_PICKUP = "Cannot pickup";

				// Token: 0x0400B6CA RID: 46794
				public static LocString IS_AWAKE = "{Selected} is sleeping";

				// Token: 0x0400B6CB RID: 46795
				public static LocString IS_STANDING = "{Selected} must stand";

				// Token: 0x0400B6CC RID: 46796
				public static LocString IS_MOVING = "{Selected} is not moving";

				// Token: 0x0400B6CD RID: 46797
				public static LocString IS_OFF_LADDER = "{Selected} is busy climbing";

				// Token: 0x0400B6CE RID: 46798
				public static LocString NOT_IN_TUBE = "{Selected} is busy in transit";

				// Token: 0x0400B6CF RID: 46799
				public static LocString HAS_TRAIT = "Missing required trait";

				// Token: 0x0400B6D0 RID: 46800
				public static LocString IS_OPERATIONAL = "Not operational";

				// Token: 0x0400B6D1 RID: 46801
				public static LocString IS_MARKED_FOR_DECONSTRUCTION = "Being deconstructed";

				// Token: 0x0400B6D2 RID: 46802
				public static LocString IS_NOT_BURROWED = "Is not burrowed";

				// Token: 0x0400B6D3 RID: 46803
				public static LocString IS_CREATURE_AVAILABLE_FOR_RANCHING = "No Critters Available";

				// Token: 0x0400B6D4 RID: 46804
				public static LocString IS_CREATURE_AVAILABLE_FOR_FIXED_CAPTURE = "Pen Status OK";

				// Token: 0x0400B6D5 RID: 46805
				public static LocString IS_MARKED_FOR_DISABLE = "Building Disabled";

				// Token: 0x0400B6D6 RID: 46806
				public static LocString IS_FUNCTIONAL = "Not functioning";

				// Token: 0x0400B6D7 RID: 46807
				public static LocString IS_OVERRIDE_TARGET_NULL_OR_ME = "DebugIsOverrideTargetNullOrMe";

				// Token: 0x0400B6D8 RID: 46808
				public static LocString NOT_CHORE_CREATOR = "DebugNotChoreCreator";

				// Token: 0x0400B6D9 RID: 46809
				public static LocString IS_GETTING_MORE_STRESSED = "{Selected}'s stress is decreasing";

				// Token: 0x0400B6DA RID: 46810
				public static LocString IS_ALLOWED_BY_AUTOMATION = "Automated";

				// Token: 0x0400B6DB RID: 46811
				public static LocString CAN_DO_RECREATION = "Not Interested";

				// Token: 0x0400B6DC RID: 46812
				public static LocString DOES_SUIT_NEED_RECHARGING_IDLE = "Suit is currently charged";

				// Token: 0x0400B6DD RID: 46813
				public static LocString DOES_SUIT_NEED_RECHARGING_URGENT = "Suit is currently charged";

				// Token: 0x0400B6DE RID: 46814
				public static LocString HAS_SUIT_MARKER = "No Suit Checkpoint";

				// Token: 0x0400B6DF RID: 46815
				public static LocString ALLOWED_TO_DEPRESSURIZE = "Not currently overpressure";

				// Token: 0x0400B6E0 RID: 46816
				public static LocString IS_STRESS_ABOVE_ACTIVATION_RANGE = "{Selected} is not stressed right now";

				// Token: 0x0400B6E1 RID: 46817
				public static LocString IS_NOT_ANGRY = "{Selected} is too angry";

				// Token: 0x0400B6E2 RID: 46818
				public static LocString IS_NOT_BEING_ATTACKED = "{Selected} is in combat";

				// Token: 0x0400B6E3 RID: 46819
				public static LocString IS_CONSUMPTION_PERMITTED = "Disallowed by consumable permissions";

				// Token: 0x0400B6E4 RID: 46820
				public static LocString CAN_CURE = "No applicable illness";

				// Token: 0x0400B6E5 RID: 46821
				public static LocString TREATMENT_AVAILABLE = "No treatable illness";

				// Token: 0x0400B6E6 RID: 46822
				public static LocString DOCTOR_AVAILABLE = "No doctors available\n(Duplicants cannot treat themselves)";

				// Token: 0x0400B6E7 RID: 46823
				public static LocString IS_OKAY_TIME_TO_SLEEP = "No current need";

				// Token: 0x0400B6E8 RID: 46824
				public static LocString IS_NARCOLEPSING = "{Selected} is currently napping";

				// Token: 0x0400B6E9 RID: 46825
				public static LocString IS_FETCH_TARGET_AVAILABLE = "No pending deliveries";

				// Token: 0x0400B6EA RID: 46826
				public static LocString EDIBLE_IS_NOT_NULL = "Consumable Permission not allowed";

				// Token: 0x0400B6EB RID: 46827
				public static LocString HAS_MINGLE_CELL = "Nowhere to Mingle";

				// Token: 0x0400B6EC RID: 46828
				public static LocString EXCLUSIVELY_AVAILABLE = "Building Already Busy";

				// Token: 0x0400B6ED RID: 46829
				public static LocString BLADDER_FULL = "Bladder isn't full";

				// Token: 0x0400B6EE RID: 46830
				public static LocString BLADDER_NOT_FULL = "Bladder too full";

				// Token: 0x0400B6EF RID: 46831
				public static LocString CURRENTLY_PEEING = "Currently Peeing";

				// Token: 0x0400B6F0 RID: 46832
				public static LocString HAS_BALLOON_STALL_CELL = "Has a location for a Balloon Stall";

				// Token: 0x0400B6F1 RID: 46833
				public static LocString IS_MINION = "Must be a Duplicant";

				// Token: 0x0400B6F2 RID: 46834
				public static LocString IS_ROCKET_TRAVELLING = "Rocket must be travelling";
			}
		}

		// Token: 0x02001DF8 RID: 7672
		public class SKILLGROUPS
		{
			// Token: 0x02002B45 RID: 11077
			public class MINING
			{
				// Token: 0x0400B6F3 RID: 46835
				public static LocString NAME = "Digger";
			}

			// Token: 0x02002B46 RID: 11078
			public class BUILDING
			{
				// Token: 0x0400B6F4 RID: 46836
				public static LocString NAME = "Builder";
			}

			// Token: 0x02002B47 RID: 11079
			public class FARMING
			{
				// Token: 0x0400B6F5 RID: 46837
				public static LocString NAME = "Farmer";
			}

			// Token: 0x02002B48 RID: 11080
			public class RANCHING
			{
				// Token: 0x0400B6F6 RID: 46838
				public static LocString NAME = "Rancher";
			}

			// Token: 0x02002B49 RID: 11081
			public class COOKING
			{
				// Token: 0x0400B6F7 RID: 46839
				public static LocString NAME = "Cooker";
			}

			// Token: 0x02002B4A RID: 11082
			public class ART
			{
				// Token: 0x0400B6F8 RID: 46840
				public static LocString NAME = "Decorator";
			}

			// Token: 0x02002B4B RID: 11083
			public class RESEARCH
			{
				// Token: 0x0400B6F9 RID: 46841
				public static LocString NAME = "Researcher";
			}

			// Token: 0x02002B4C RID: 11084
			public class SUITS
			{
				// Token: 0x0400B6FA RID: 46842
				public static LocString NAME = "Suit Wearer";
			}

			// Token: 0x02002B4D RID: 11085
			public class HAULING
			{
				// Token: 0x0400B6FB RID: 46843
				public static LocString NAME = "Supplier";
			}

			// Token: 0x02002B4E RID: 11086
			public class TECHNICALS
			{
				// Token: 0x0400B6FC RID: 46844
				public static LocString NAME = "Operator";
			}

			// Token: 0x02002B4F RID: 11087
			public class MEDICALAID
			{
				// Token: 0x0400B6FD RID: 46845
				public static LocString NAME = "Doctor";
			}

			// Token: 0x02002B50 RID: 11088
			public class BASEKEEPING
			{
				// Token: 0x0400B6FE RID: 46846
				public static LocString NAME = "Tidier";
			}

			// Token: 0x02002B51 RID: 11089
			public class ROCKETRY
			{
				// Token: 0x0400B6FF RID: 46847
				public static LocString NAME = "Pilot";
			}
		}

		// Token: 0x02001DF9 RID: 7673
		public class CHOREGROUPS
		{
			// Token: 0x02002B52 RID: 11090
			public class ART
			{
				// Token: 0x0400B700 RID: 46848
				public static LocString NAME = "Decorating";

				// Token: 0x0400B701 RID: 46849
				public static LocString DESC = string.Concat(new string[]
				{
					"Sculpt or paint to improve colony ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400B702 RID: 46850
				public static LocString ARCHETYPE_NAME = "Decorator";
			}

			// Token: 0x02002B53 RID: 11091
			public class COMBAT
			{
				// Token: 0x0400B703 RID: 46851
				public static LocString NAME = "Attacking";

				// Token: 0x0400B704 RID: 46852
				public static LocString DESC = "Fight wild " + UI.FormatAsLink("Critters", "CREATURES") + ".";

				// Token: 0x0400B705 RID: 46853
				public static LocString ARCHETYPE_NAME = "Attacker";
			}

			// Token: 0x02002B54 RID: 11092
			public class LIFESUPPORT
			{
				// Token: 0x0400B706 RID: 46854
				public static LocString NAME = "Life Support";

				// Token: 0x0400B707 RID: 46855
				public static LocString DESC = string.Concat(new string[]
				{
					"Maintain ",
					BUILDINGS.PREFABS.ALGAEHABITAT.NAME,
					"s, ",
					BUILDINGS.PREFABS.AIRFILTER.NAME,
					"s, and ",
					BUILDINGS.PREFABS.WATERPURIFIER.NAME,
					"s to support colony life."
				});

				// Token: 0x0400B708 RID: 46856
				public static LocString ARCHETYPE_NAME = "Life Supporter";
			}

			// Token: 0x02002B55 RID: 11093
			public class TOGGLE
			{
				// Token: 0x0400B709 RID: 46857
				public static LocString NAME = "Toggling";

				// Token: 0x0400B70A RID: 46858
				public static LocString DESC = "Enable or disable buildings, adjust building settings, and set or flip switches and sensors.";

				// Token: 0x0400B70B RID: 46859
				public static LocString ARCHETYPE_NAME = "Toggler";
			}

			// Token: 0x02002B56 RID: 11094
			public class COOK
			{
				// Token: 0x0400B70C RID: 46860
				public static LocString NAME = "Cooking";

				// Token: 0x0400B70D RID: 46861
				public static LocString DESC = string.Concat(new string[]
				{
					"Operate ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" preparation buildings."
				});

				// Token: 0x0400B70E RID: 46862
				public static LocString ARCHETYPE_NAME = "Cooker";
			}

			// Token: 0x02002B57 RID: 11095
			public class RESEARCH
			{
				// Token: 0x0400B70F RID: 46863
				public static LocString NAME = "Researching";

				// Token: 0x0400B710 RID: 46864
				public static LocString DESC = string.Concat(new string[]
				{
					"Use ",
					UI.PRE_KEYWORD,
					"Research Stations",
					UI.PST_KEYWORD,
					" to unlock new technologies."
				});

				// Token: 0x0400B711 RID: 46865
				public static LocString ARCHETYPE_NAME = "Researcher";
			}

			// Token: 0x02002B58 RID: 11096
			public class REPAIR
			{
				// Token: 0x0400B712 RID: 46866
				public static LocString NAME = "Repairing";

				// Token: 0x0400B713 RID: 46867
				public static LocString DESC = "Repair damaged buildings.";

				// Token: 0x0400B714 RID: 46868
				public static LocString ARCHETYPE_NAME = "Repairer";
			}

			// Token: 0x02002B59 RID: 11097
			public class FARMING
			{
				// Token: 0x0400B715 RID: 46869
				public static LocString NAME = "Farming";

				// Token: 0x0400B716 RID: 46870
				public static LocString DESC = string.Concat(new string[]
				{
					"Gather crops from mature ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400B717 RID: 46871
				public static LocString ARCHETYPE_NAME = "Farmer";
			}

			// Token: 0x02002B5A RID: 11098
			public class RANCHING
			{
				// Token: 0x0400B718 RID: 46872
				public static LocString NAME = "Ranching";

				// Token: 0x0400B719 RID: 46873
				public static LocString DESC = "Tend to domesticated " + UI.FormatAsLink("Critters", "CREATURES") + ".";

				// Token: 0x0400B71A RID: 46874
				public static LocString ARCHETYPE_NAME = "Rancher";
			}

			// Token: 0x02002B5B RID: 11099
			public class BUILD
			{
				// Token: 0x0400B71B RID: 46875
				public static LocString NAME = "Building";

				// Token: 0x0400B71C RID: 46876
				public static LocString DESC = "Construct new buildings.";

				// Token: 0x0400B71D RID: 46877
				public static LocString ARCHETYPE_NAME = "Builder";
			}

			// Token: 0x02002B5C RID: 11100
			public class HAULING
			{
				// Token: 0x0400B71E RID: 46878
				public static LocString NAME = "Supplying";

				// Token: 0x0400B71F RID: 46879
				public static LocString DESC = "Run resources to critical buildings and urgent storage.";

				// Token: 0x0400B720 RID: 46880
				public static LocString ARCHETYPE_NAME = "Supplier";
			}

			// Token: 0x02002B5D RID: 11101
			public class STORAGE
			{
				// Token: 0x0400B721 RID: 46881
				public static LocString NAME = "Storing";

				// Token: 0x0400B722 RID: 46882
				public static LocString DESC = "Fill storage buildings with resources when no other errands are available.";

				// Token: 0x0400B723 RID: 46883
				public static LocString ARCHETYPE_NAME = "Storer";
			}

			// Token: 0x02002B5E RID: 11102
			public class RECREATION
			{
				// Token: 0x0400B724 RID: 46884
				public static LocString NAME = "Relaxing";

				// Token: 0x0400B725 RID: 46885
				public static LocString DESC = "Use leisure facilities, chat with other Duplicants, and relieve Stress.";

				// Token: 0x0400B726 RID: 46886
				public static LocString ARCHETYPE_NAME = "Relaxer";
			}

			// Token: 0x02002B5F RID: 11103
			public class BASEKEEPING
			{
				// Token: 0x0400B727 RID: 46887
				public static LocString NAME = "Tidying";

				// Token: 0x0400B728 RID: 46888
				public static LocString DESC = "Sweep, mop, and disinfect objects within the colony.";

				// Token: 0x0400B729 RID: 46889
				public static LocString ARCHETYPE_NAME = "Tidier";
			}

			// Token: 0x02002B60 RID: 11104
			public class DIG
			{
				// Token: 0x0400B72A RID: 46890
				public static LocString NAME = "Digging";

				// Token: 0x0400B72B RID: 46891
				public static LocString DESC = "Mine raw resources.";

				// Token: 0x0400B72C RID: 46892
				public static LocString ARCHETYPE_NAME = "Digger";
			}

			// Token: 0x02002B61 RID: 11105
			public class MEDICALAID
			{
				// Token: 0x0400B72D RID: 46893
				public static LocString NAME = "Doctoring";

				// Token: 0x0400B72E RID: 46894
				public static LocString DESC = "Treat sick and injured Duplicants.";

				// Token: 0x0400B72F RID: 46895
				public static LocString ARCHETYPE_NAME = "Doctor";
			}

			// Token: 0x02002B62 RID: 11106
			public class MASSAGE
			{
				// Token: 0x0400B730 RID: 46896
				public static LocString NAME = "Relaxing";

				// Token: 0x0400B731 RID: 46897
				public static LocString DESC = "Take breaks for massages.";

				// Token: 0x0400B732 RID: 46898
				public static LocString ARCHETYPE_NAME = "Relaxer";
			}

			// Token: 0x02002B63 RID: 11107
			public class MACHINEOPERATING
			{
				// Token: 0x0400B733 RID: 46899
				public static LocString NAME = "Operating";

				// Token: 0x0400B734 RID: 46900
				public static LocString DESC = "Operating machinery for production, fabrication, and utility purposes.";

				// Token: 0x0400B735 RID: 46901
				public static LocString ARCHETYPE_NAME = "Operator";
			}

			// Token: 0x02002B64 RID: 11108
			public class SUITS
			{
				// Token: 0x0400B736 RID: 46902
				public static LocString ARCHETYPE_NAME = "Suit Wearer";
			}

			// Token: 0x02002B65 RID: 11109
			public class ROCKETRY
			{
				// Token: 0x0400B737 RID: 46903
				public static LocString NAME = "Rocketry";

				// Token: 0x0400B738 RID: 46904
				public static LocString DESC = "Pilot rockets";

				// Token: 0x0400B739 RID: 46905
				public static LocString ARCHETYPE_NAME = "Pilot";
			}
		}

		// Token: 0x02001DFA RID: 7674
		public class STATUSITEMS
		{
			// Token: 0x02002B66 RID: 11110
			public class WAXEDFORTRANSITTUBE
			{
				// Token: 0x0400B73A RID: 46906
				public static LocString NAME = "Smooth Rider";

				// Token: 0x0400B73B RID: 46907
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slapped on some ",
					ELEMENTS.MILKFAT.NAME,
					" before starting their commute\n\nThis boosts their ",
					BUILDINGS.PREFABS.TRAVELTUBE.NAME,
					" travel speed by {0}"
				});
			}

			// Token: 0x02002B67 RID: 11111
			public class ARMINGTRAP
			{
				// Token: 0x0400B73C RID: 46908
				public static LocString NAME = "Arming trap";

				// Token: 0x0400B73D RID: 46909
				public static LocString TOOLTIP = "This Duplicant is arming a trap";
			}

			// Token: 0x02002B68 RID: 11112
			public class GENERIC_DELIVER
			{
				// Token: 0x0400B73E RID: 46910
				public static LocString NAME = "Delivering resources to {Target}";

				// Token: 0x0400B73F RID: 46911
				public static LocString TOOLTIP = "This Duplicant is transporting materials to <b>{Target}</b>";
			}

			// Token: 0x02002B69 RID: 11113
			public class COUGHING
			{
				// Token: 0x0400B740 RID: 46912
				public static LocString NAME = "Yucky Lungs Coughing";

				// Token: 0x0400B741 RID: 46913
				public static LocString TOOLTIP = "Hey! Do that into your elbow\n• Coughing fit was caused by " + DUPLICANTS.MODIFIERS.CONTAMINATEDLUNGS.NAME;
			}

			// Token: 0x02002B6A RID: 11114
			public class WEARING_PAJAMAS
			{
				// Token: 0x0400B742 RID: 46914
				public static LocString NAME = "Wearing " + UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS");

				// Token: 0x0400B743 RID: 46915
				public static LocString TOOLTIP = "This Duplicant can now produce " + UI.FormatAsLink("Dream Journals", "DREAMJOURNAL") + " when sleeping";
			}

			// Token: 0x02002B6B RID: 11115
			public class DREAMING
			{
				// Token: 0x0400B744 RID: 46916
				public static LocString NAME = "Dreaming";

				// Token: 0x0400B745 RID: 46917
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is adventuring through their own subconscious\n\nDreams are caused by wearing ",
					UI.FormatAsLink("Pajamas", "SLEEP_CLINIC_PAJAMAS"),
					"\n\n",
					UI.FormatAsLink("Dream Journal", "DREAMJOURNAL"),
					" will be ready in {time}"
				});
			}

			// Token: 0x02002B6C RID: 11116
			public class FOSSILHUNT
			{
				// Token: 0x020031D6 RID: 12758
				public class WORKEREXCAVATING
				{
					// Token: 0x0400C707 RID: 50951
					public static LocString NAME = "Excavating Fossil";

					// Token: 0x0400C708 RID: 50952
					public static LocString TOOLTIP = "This Duplicant is carefully uncovering a " + UI.FormatAsLink("Fossil", "FOSSIL");
				}
			}

			// Token: 0x02002B6D RID: 11117
			public class SLEEPING
			{
				// Token: 0x0400B746 RID: 46918
				public static LocString NAME = "Sleeping";

				// Token: 0x0400B747 RID: 46919
				public static LocString TOOLTIP = "This Duplicant is recovering stamina";

				// Token: 0x0400B748 RID: 46920
				public static LocString TOOLTIP_DISTURBER = "\n\nThey were sleeping peacefully until they were disturbed by <b>{Disturber}</b>";
			}

			// Token: 0x02002B6E RID: 11118
			public class SLEEPINGPEACEFULLY
			{
				// Token: 0x0400B749 RID: 46921
				public static LocString NAME = "Sleeping peacefully";

				// Token: 0x0400B74A RID: 46922
				public static LocString TOOLTIP = "This Duplicant is getting well-deserved, quality sleep\n\nAt this rate they're sure to feel " + UI.FormatAsLink("Well Rested", "SLEEP") + " tomorrow morning";
			}

			// Token: 0x02002B6F RID: 11119
			public class SLEEPINGBADLY
			{
				// Token: 0x0400B74B RID: 46923
				public static LocString NAME = "Sleeping badly";

				// Token: 0x0400B74C RID: 46924
				public static LocString TOOLTIP = "This Duplicant's having trouble falling asleep due to noise from <b>{Disturber}</b>\n\nThey're going to feel a bit " + UI.FormatAsLink("Unrested", "SLEEP") + " tomorrow morning";
			}

			// Token: 0x02002B70 RID: 11120
			public class SLEEPINGTERRIBLY
			{
				// Token: 0x0400B74D RID: 46925
				public static LocString NAME = "Can't sleep";

				// Token: 0x0400B74E RID: 46926
				public static LocString TOOLTIP = "This Duplicant was woken up by noise from <b>{Disturber}</b> and can't get back to sleep\n\nThey're going to feel " + UI.FormatAsLink("Dead Tired", "SLEEP") + " tomorrow morning";
			}

			// Token: 0x02002B71 RID: 11121
			public class SLEEPINGINTERRUPTEDBYLIGHT
			{
				// Token: 0x0400B74F RID: 46927
				public static LocString NAME = "Interrupted Sleep: Bright Light";

				// Token: 0x0400B750 RID: 46928
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant can't sleep because the ",
					UI.PRE_KEYWORD,
					"Lights",
					UI.PST_KEYWORD,
					" are still on"
				});
			}

			// Token: 0x02002B72 RID: 11122
			public class SLEEPINGINTERRUPTEDBYNOISE
			{
				// Token: 0x0400B751 RID: 46929
				public static LocString NAME = "Interrupted Sleep: Snoring Friend";

				// Token: 0x0400B752 RID: 46930
				public static LocString TOOLTIP = "This Duplicant is having trouble sleeping thanks to a certain noisy someone";
			}

			// Token: 0x02002B73 RID: 11123
			public class SLEEPINGINTERRUPTEDBYFEAROFDARK
			{
				// Token: 0x0400B753 RID: 46931
				public static LocString NAME = "Interrupted Sleep: Afraid of Dark";

				// Token: 0x0400B754 RID: 46932
				public static LocString TOOLTIP = "This Duplicant is having trouble sleeping because of their fear of the dark";
			}

			// Token: 0x02002B74 RID: 11124
			public class SLEEPINGINTERRUPTEDBYMOVEMENT
			{
				// Token: 0x0400B755 RID: 46933
				public static LocString NAME = "Interrupted Sleep: Bed Jostling";

				// Token: 0x0400B756 RID: 46934
				public static LocString TOOLTIP = "This Duplicant was woken up because their bed was moved";
			}

			// Token: 0x02002B75 RID: 11125
			public class REDALERT
			{
				// Token: 0x0400B757 RID: 46935
				public static LocString NAME = "Red Alert!";

				// Token: 0x0400B758 RID: 46936
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The colony is in a state of ",
					UI.PRE_KEYWORD,
					"Red Alert",
					UI.PST_KEYWORD,
					". Duplicants will not eat, sleep, use the bathroom, or engage in leisure activities while the ",
					UI.PRE_KEYWORD,
					"Red Alert",
					UI.PST_KEYWORD,
					" is active"
				});
			}

			// Token: 0x02002B76 RID: 11126
			public class ROLE
			{
				// Token: 0x0400B759 RID: 46937
				public static LocString NAME = "{Role}: {Progress} Mastery";

				// Token: 0x0400B75A RID: 46938
				public static LocString TOOLTIP = "This Duplicant is working as a <b>{Role}</b>\n\nThey have <b>{Progress}</b> mastery of this job";
			}

			// Token: 0x02002B77 RID: 11127
			public class LOWOXYGEN
			{
				// Token: 0x0400B75B RID: 46939
				public static LocString NAME = "Oxygen low";

				// Token: 0x0400B75C RID: 46940
				public static LocString TOOLTIP = "This Duplicant is working in a low breathability area";

				// Token: 0x0400B75D RID: 46941
				public static LocString NOTIFICATION_NAME = "Low " + ELEMENTS.OXYGEN.NAME + " area entered";

				// Token: 0x0400B75E RID: 46942
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are working in areas with low " + ELEMENTS.OXYGEN.NAME + ":";
			}

			// Token: 0x02002B78 RID: 11128
			public class SEVEREWOUNDS
			{
				// Token: 0x0400B75F RID: 46943
				public static LocString NAME = "Severely injured";

				// Token: 0x0400B760 RID: 46944
				public static LocString TOOLTIP = "This Duplicant is badly hurt";

				// Token: 0x0400B761 RID: 46945
				public static LocString NOTIFICATION_NAME = "Severely injured";

				// Token: 0x0400B762 RID: 46946
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are badly hurt and require medical attention";
			}

			// Token: 0x02002B79 RID: 11129
			public class INCAPACITATED
			{
				// Token: 0x0400B763 RID: 46947
				public static LocString NAME = "Incapacitated: {CauseOfIncapacitation}\nTime until death: {TimeUntilDeath}\n";

				// Token: 0x0400B764 RID: 46948
				public static LocString TOOLTIP = "This Duplicant is near death!\n\nAssign them to a Triage Cot for rescue";

				// Token: 0x0400B765 RID: 46949
				public static LocString NOTIFICATION_NAME = "Incapacitated";

				// Token: 0x0400B766 RID: 46950
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are near death.\nA " + BUILDINGS.PREFABS.MEDICALCOT.NAME + " is required for rescue:";
			}

			// Token: 0x02002B7A RID: 11130
			public class BEDUNREACHABLE
			{
				// Token: 0x0400B767 RID: 46951
				public static LocString NAME = "Cannot reach bed";

				// Token: 0x0400B768 RID: 46952
				public static LocString TOOLTIP = "This Duplicant cannot reach their bed";

				// Token: 0x0400B769 RID: 46953
				public static LocString NOTIFICATION_NAME = "Unreachable bed";

				// Token: 0x0400B76A RID: 46954
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot sleep because their ",
					UI.PRE_KEYWORD,
					"Beds",
					UI.PST_KEYWORD,
					" are beyond their reach:"
				});
			}

			// Token: 0x02002B7B RID: 11131
			public class COLD
			{
				// Token: 0x0400B76B RID: 46955
				public static LocString NAME = "Chilly surroundings";

				// Token: 0x0400B76C RID: 46956
				public static LocString TOOLTIP = "This Duplicant cannot retain enough heat to stay warm and may be under insulated for this area\n\nStress: <b>{StressModification}</b>\n\nCurrent Environmental Exchange: <b>{currentTransferWattage}</b>\n\nInsulation Thickness: {conductivityBarrier}";
			}

			// Token: 0x02002B7C RID: 11132
			public class DAILYRATIONLIMITREACHED
			{
				// Token: 0x0400B76D RID: 46957
				public static LocString NAME = "Daily calorie limit reached";

				// Token: 0x0400B76E RID: 46958
				public static LocString TOOLTIP = "This Duplicant has consumed their allotted " + UI.FormatAsLink("Rations", "FOOD") + " for the day";

				// Token: 0x0400B76F RID: 46959
				public static LocString NOTIFICATION_NAME = "Daily calorie limit reached";

				// Token: 0x0400B770 RID: 46960
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants have consumed their allotted " + UI.FormatAsLink("Rations", "FOOD") + " for the day:";
			}

			// Token: 0x02002B7D RID: 11133
			public class DOCTOR
			{
				// Token: 0x0400B771 RID: 46961
				public static LocString NAME = "Treating Patient";

				// Token: 0x0400B772 RID: 46962
				public static LocString STATUS = "This Duplicant is going to administer medical care to an ailing friend";
			}

			// Token: 0x02002B7E RID: 11134
			public class HOLDINGBREATH
			{
				// Token: 0x0400B773 RID: 46963
				public static LocString NAME = "Holding breath";

				// Token: 0x0400B774 RID: 46964
				public static LocString TOOLTIP = "This Duplicant cannot breathe in their current location";
			}

			// Token: 0x02002B7F RID: 11135
			public class RECOVERINGBREATH
			{
				// Token: 0x0400B775 RID: 46965
				public static LocString NAME = "Recovering breath";

				// Token: 0x0400B776 RID: 46966
				public static LocString TOOLTIP = "This Duplicant held their breath too long and needs a moment";
			}

			// Token: 0x02002B80 RID: 11136
			public class HOT
			{
				// Token: 0x0400B777 RID: 46967
				public static LocString NAME = "Toasty surroundings";

				// Token: 0x0400B778 RID: 46968
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant cannot let off enough ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" to stay cool and may be over insulated for this area\n\nStress Modification: <b>{StressModification}</b>\n\nCurrent Environmental Exchange: <b>{currentTransferWattage}</b>\n\nInsulation Thickness: {conductivityBarrier}"
				});
			}

			// Token: 0x02002B81 RID: 11137
			public class HUNGRY
			{
				// Token: 0x0400B779 RID: 46969
				public static LocString NAME = "Hungry";

				// Token: 0x0400B77A RID: 46970
				public static LocString TOOLTIP = "This Duplicant would really like something to eat";
			}

			// Token: 0x02002B82 RID: 11138
			public class POORDECOR
			{
				// Token: 0x0400B77B RID: 46971
				public static LocString NAME = "Drab decor";

				// Token: 0x0400B77C RID: 46972
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is depressed by the lack of ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" in this area"
				});
			}

			// Token: 0x02002B83 RID: 11139
			public class POORQUALITYOFLIFE
			{
				// Token: 0x0400B77D RID: 46973
				public static LocString NAME = "Low Morale";

				// Token: 0x0400B77E RID: 46974
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The bad in this Duplicant's life is starting to outweigh the good\n\nImproved amenities and additional ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" would help improve their ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002B84 RID: 11140
			public class POOR_FOOD_QUALITY
			{
				// Token: 0x0400B77F RID: 46975
				public static LocString NAME = "Lousy Meal";

				// Token: 0x0400B780 RID: 46976
				public static LocString TOOLTIP = "The last meal this Duplicant ate didn't quite meet their expectations";
			}

			// Token: 0x02002B85 RID: 11141
			public class GOOD_FOOD_QUALITY
			{
				// Token: 0x0400B781 RID: 46977
				public static LocString NAME = "Decadent Meal";

				// Token: 0x0400B782 RID: 46978
				public static LocString TOOLTIP = "The last meal this Duplicant ate exceeded their expectations!";
			}

			// Token: 0x02002B86 RID: 11142
			public class NERVOUSBREAKDOWN
			{
				// Token: 0x0400B783 RID: 46979
				public static LocString NAME = "Nervous breakdown";

				// Token: 0x0400B784 RID: 46980
				public static LocString TOOLTIP = UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD + " has completely eroded this Duplicant's ability to function";

				// Token: 0x0400B785 RID: 46981
				public static LocString NOTIFICATION_NAME = "Nervous breakdown";

				// Token: 0x0400B786 RID: 46982
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have cracked under the ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" and need assistance:"
				});
			}

			// Token: 0x02002B87 RID: 11143
			public class STRESSED
			{
				// Token: 0x0400B787 RID: 46983
				public static LocString NAME = "Stressed";

				// Token: 0x0400B788 RID: 46984
				public static LocString TOOLTIP = "This Duplicant is feeling the pressure";

				// Token: 0x0400B789 RID: 46985
				public static LocString NOTIFICATION_NAME = "High stress";

				// Token: 0x0400B78A RID: 46986
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants are ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" and need to unwind:"
				});
			}

			// Token: 0x02002B88 RID: 11144
			public class NORATIONSAVAILABLE
			{
				// Token: 0x0400B78B RID: 46987
				public static LocString NAME = "No food available";

				// Token: 0x0400B78C RID: 46988
				public static LocString TOOLTIP = "There's nothing in the colony for this Duplicant to eat";

				// Token: 0x0400B78D RID: 46989
				public static LocString NOTIFICATION_NAME = "No food available";

				// Token: 0x0400B78E RID: 46990
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants have nothing to eat:";
			}

			// Token: 0x02002B89 RID: 11145
			public class QUARANTINEAREAUNREACHABLE
			{
				// Token: 0x0400B78F RID: 46991
				public static LocString NAME = "Cannot reach quarantine";

				// Token: 0x0400B790 RID: 46992
				public static LocString TOOLTIP = "This Duplicant cannot reach their quarantine zone";

				// Token: 0x0400B791 RID: 46993
				public static LocString NOTIFICATION_NAME = "Unreachable quarantine";

				// Token: 0x0400B792 RID: 46994
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants cannot reach their assigned quarantine zones:";
			}

			// Token: 0x02002B8A RID: 11146
			public class QUARANTINED
			{
				// Token: 0x0400B793 RID: 46995
				public static LocString NAME = "Quarantined";

				// Token: 0x0400B794 RID: 46996
				public static LocString TOOLTIP = "This Duplicant has been isolated from the colony";
			}

			// Token: 0x02002B8B RID: 11147
			public class RATIONSUNREACHABLE
			{
				// Token: 0x0400B795 RID: 46997
				public static LocString NAME = "Cannot reach food";

				// Token: 0x0400B796 RID: 46998
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There is ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" in the colony that this Duplicant cannot reach"
				});

				// Token: 0x0400B797 RID: 46999
				public static LocString NOTIFICATION_NAME = "Unreachable food";

				// Token: 0x0400B798 RID: 47000
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot access the colony's ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02002B8C RID: 11148
			public class RATIONSNOTPERMITTED
			{
				// Token: 0x0400B799 RID: 47001
				public static LocString NAME = "Food Type Not Permitted";

				// Token: 0x0400B79A RID: 47002
				public static LocString TOOLTIP = "This Duplicant is not allowed to eat any of the " + UI.FormatAsLink("Food", "FOOD") + " in their reach\n\nEnter the <color=#833A5FFF>CONSUMABLES</color> <color=#F44A47><b>[F]</b></color> to adjust their food permissions";

				// Token: 0x0400B79B RID: 47003
				public static LocString NOTIFICATION_NAME = "Unpermitted food";

				// Token: 0x0400B79C RID: 47004
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants' <color=#833A5FFF>CONSUMABLES</color> <color=#F44A47><b>[F]</b></color> permissions prevent them from eating any of the " + UI.FormatAsLink("Food", "FOOD") + " within their reach:";
			}

			// Token: 0x02002B8D RID: 11149
			public class ROTTEN
			{
				// Token: 0x0400B79D RID: 47005
				public static LocString NAME = "Rotten";

				// Token: 0x0400B79E RID: 47006
				public static LocString TOOLTIP = "Gross!";
			}

			// Token: 0x02002B8E RID: 11150
			public class STARVING
			{
				// Token: 0x0400B79F RID: 47007
				public static LocString NAME = "Starving";

				// Token: 0x0400B7A0 RID: 47008
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is about to die and needs ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					"!"
				});

				// Token: 0x0400B7A1 RID: 47009
				public static LocString NOTIFICATION_NAME = "Starvation";

				// Token: 0x0400B7A2 RID: 47010
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants are starving and will die if they can't find ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02002B8F RID: 11151
			public class STRESS_SIGNAL_AGGRESIVE
			{
				// Token: 0x0400B7A3 RID: 47011
				public static LocString NAME = "Frustrated";

				// Token: 0x0400B7A4 RID: 47012
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is trying to keep their cool\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they destroy something to let off steam"
				});
			}

			// Token: 0x02002B90 RID: 11152
			public class STRESS_SIGNAL_BINGE_EAT
			{
				// Token: 0x0400B7A5 RID: 47013
				public static LocString NAME = "Stress Cravings";

				// Token: 0x0400B7A6 RID: 47014
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is consumed by hunger\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they eat all the colony's ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" stores"
				});
			}

			// Token: 0x02002B91 RID: 11153
			public class STRESS_SIGNAL_UGLY_CRIER
			{
				// Token: 0x0400B7A7 RID: 47015
				public static LocString NAME = "Misty Eyed";

				// Token: 0x0400B7A8 RID: 47016
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is trying and failing to swallow their emotions\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they have a good ugly cry"
				});
			}

			// Token: 0x02002B92 RID: 11154
			public class STRESS_SIGNAL_VOMITER
			{
				// Token: 0x0400B7A9 RID: 47017
				public static LocString NAME = "Stress Burp";

				// Token: 0x0400B7AA RID: 47018
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Sort of like having butterflies in your stomach, except they're burps\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they start to stress vomit"
				});
			}

			// Token: 0x02002B93 RID: 11155
			public class STRESS_SIGNAL_BANSHEE
			{
				// Token: 0x0400B7AB RID: 47019
				public static LocString NAME = "Suppressed Screams";

				// Token: 0x0400B7AC RID: 47020
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is fighting the urge to scream\n\nImprove this Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" before they start wailing uncontrollably"
				});
			}

			// Token: 0x02002B94 RID: 11156
			public class ENTOMBEDCHORE
			{
				// Token: 0x0400B7AD RID: 47021
				public static LocString NAME = "Entombed";

				// Token: 0x0400B7AE RID: 47022
				public static LocString TOOLTIP = "This Duplicant needs someone to help dig them out!";

				// Token: 0x0400B7AF RID: 47023
				public static LocString NOTIFICATION_NAME = "Entombed";

				// Token: 0x0400B7B0 RID: 47024
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are trapped:";
			}

			// Token: 0x02002B95 RID: 11157
			public class EARLYMORNING
			{
				// Token: 0x0400B7B1 RID: 47025
				public static LocString NAME = "Early Bird";

				// Token: 0x0400B7B2 RID: 47026
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is jazzed to start the day\n• All ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" <b>+2</b> in the morning"
				});
			}

			// Token: 0x02002B96 RID: 11158
			public class NIGHTTIME
			{
				// Token: 0x0400B7B3 RID: 47027
				public static LocString NAME = "Night Owl";

				// Token: 0x0400B7B4 RID: 47028
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is more efficient on a nighttime ",
					UI.PRE_KEYWORD,
					"Schedule",
					UI.PST_KEYWORD,
					"\n• All ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" <b>+3</b> at night"
				});
			}

			// Token: 0x02002B97 RID: 11159
			public class METEORPHILE
			{
				// Token: 0x0400B7B5 RID: 47029
				public static LocString NAME = "Rock Fan";

				// Token: 0x0400B7B6 RID: 47030
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is <i>really</i> into meteor showers\n• All ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					" <b>+3</b> during meteor showers"
				});
			}

			// Token: 0x02002B98 RID: 11160
			public class SUFFOCATING
			{
				// Token: 0x0400B7B7 RID: 47031
				public static LocString NAME = "Suffocating";

				// Token: 0x0400B7B8 RID: 47032
				public static LocString TOOLTIP = "This Duplicant cannot breathe!";

				// Token: 0x0400B7B9 RID: 47033
				public static LocString NOTIFICATION_NAME = "Suffocating";

				// Token: 0x0400B7BA RID: 47034
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants cannot breathe:";
			}

			// Token: 0x02002B99 RID: 11161
			public class TIRED
			{
				// Token: 0x0400B7BB RID: 47035
				public static LocString NAME = "Tired";

				// Token: 0x0400B7BC RID: 47036
				public static LocString TOOLTIP = "This Duplicant could use a nice nap";
			}

			// Token: 0x02002B9A RID: 11162
			public class IDLE
			{
				// Token: 0x0400B7BD RID: 47037
				public static LocString NAME = "Idle";

				// Token: 0x0400B7BE RID: 47038
				public static LocString TOOLTIP = "This Duplicant cannot reach any pending errands";

				// Token: 0x0400B7BF RID: 47039
				public static LocString NOTIFICATION_NAME = "Idle";

				// Token: 0x0400B7C0 RID: 47040
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot reach any pending ",
					UI.PRE_KEYWORD,
					"Errands",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02002B9B RID: 11163
			public class FIGHTING
			{
				// Token: 0x0400B7C1 RID: 47041
				public static LocString NAME = "In combat";

				// Token: 0x0400B7C2 RID: 47042
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is attacking a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"!"
				});

				// Token: 0x0400B7C3 RID: 47043
				public static LocString NOTIFICATION_NAME = "Combat!";

				// Token: 0x0400B7C4 RID: 47044
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have engaged a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					" in combat:"
				});
			}

			// Token: 0x02002B9C RID: 11164
			public class FLEEING
			{
				// Token: 0x0400B7C5 RID: 47045
				public static LocString NAME = "Fleeing";

				// Token: 0x0400B7C6 RID: 47046
				public static LocString TOOLTIP = "This Duplicant is trying to escape something scary!";

				// Token: 0x0400B7C7 RID: 47047
				public static LocString NOTIFICATION_NAME = "Fleeing!";

				// Token: 0x0400B7C8 RID: 47048
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are trying to escape:";
			}

			// Token: 0x02002B9D RID: 11165
			public class DEAD
			{
				// Token: 0x0400B7C9 RID: 47049
				public static LocString NAME = "Dead: {Death}";

				// Token: 0x0400B7CA RID: 47050
				public static LocString TOOLTIP = "This Duplicant definitely isn't sleeping";
			}

			// Token: 0x02002B9E RID: 11166
			public class LASHINGOUT
			{
				// Token: 0x0400B7CB RID: 47051
				public static LocString NAME = "Lashing out";

				// Token: 0x0400B7CC RID: 47052
				public static LocString TOOLTIP = "This Duplicant is breaking buildings to relieve their " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400B7CD RID: 47053
				public static LocString NOTIFICATION_NAME = "Lashing out";

				// Token: 0x0400B7CE RID: 47054
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants broke buildings to relieve their ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					":"
				});
			}

			// Token: 0x02002B9F RID: 11167
			public class MOVETOSUITNOTREQUIRED
			{
				// Token: 0x0400B7CF RID: 47055
				public static LocString NAME = "Exiting Exosuit area";

				// Token: 0x0400B7D0 RID: 47056
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is leaving an area where a ",
					UI.PRE_KEYWORD,
					"Suit",
					UI.PST_KEYWORD,
					" was required"
				});
			}

			// Token: 0x02002BA0 RID: 11168
			public class NOROLE
			{
				// Token: 0x0400B7D1 RID: 47057
				public static LocString NAME = "No Job";

				// Token: 0x0400B7D2 RID: 47058
				public static LocString TOOLTIP = "This Duplicant does not have a Job Assignment\n\nEnter the " + UI.FormatAsManagementMenu("Jobs Panel", "[J]") + " to view all available Jobs";
			}

			// Token: 0x02002BA1 RID: 11169
			public class DROPPINGUNUSEDINVENTORY
			{
				// Token: 0x0400B7D3 RID: 47059
				public static LocString NAME = "Dropping objects";

				// Token: 0x0400B7D4 RID: 47060
				public static LocString TOOLTIP = "This Duplicant is dropping what they're holding";
			}

			// Token: 0x02002BA2 RID: 11170
			public class MOVINGTOSAFEAREA
			{
				// Token: 0x0400B7D5 RID: 47061
				public static LocString NAME = "Moving to safe area";

				// Token: 0x0400B7D6 RID: 47062
				public static LocString TOOLTIP = "This Duplicant is finding a less dangerous place";
			}

			// Token: 0x02002BA3 RID: 11171
			public class TOILETUNREACHABLE
			{
				// Token: 0x0400B7D7 RID: 47063
				public static LocString NAME = "Unreachable toilet";

				// Token: 0x0400B7D8 RID: 47064
				public static LocString TOOLTIP = "This Duplicant cannot reach a functioning " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " or " + UI.FormatAsLink("Lavatory", "FLUSHTOILET");

				// Token: 0x0400B7D9 RID: 47065
				public static LocString NOTIFICATION_NAME = "Unreachable toilet";

				// Token: 0x0400B7DA RID: 47066
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants cannot reach a functioning ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					" or ",
					UI.FormatAsLink("Lavatory", "FLUSHTOILET"),
					":"
				});
			}

			// Token: 0x02002BA4 RID: 11172
			public class NOUSABLETOILETS
			{
				// Token: 0x0400B7DB RID: 47067
				public static LocString NAME = "Toilet out of order";

				// Token: 0x0400B7DC RID: 47068
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The only ",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" or ",
					UI.FormatAsLink("Lavatories", "FLUSHTOILET"),
					" in this Duplicant's reach are out of order"
				});

				// Token: 0x0400B7DD RID: 47069
				public static LocString NOTIFICATION_NAME = "Toilet out of order";

				// Token: 0x0400B7DE RID: 47070
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants want to use an ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					" or ",
					UI.FormatAsLink("Lavatory", "FLUSHTOILET"),
					" that is out of order:"
				});
			}

			// Token: 0x02002BA5 RID: 11173
			public class NOTOILETS
			{
				// Token: 0x0400B7DF RID: 47071
				public static LocString NAME = "No Outhouses";

				// Token: 0x0400B7E0 RID: 47072
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"There are no ",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" available for this Duplicant\n\n",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5)
				});

				// Token: 0x0400B7E1 RID: 47073
				public static LocString NOTIFICATION_NAME = "No Outhouses built";

				// Token: 0x0400B7E2 RID: 47074
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5),
					".\n\nThese Duplicants are in need of an ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					":"
				});
			}

			// Token: 0x02002BA6 RID: 11174
			public class FULLBLADDER
			{
				// Token: 0x0400B7E3 RID: 47075
				public static LocString NAME = "Full bladder";

				// Token: 0x0400B7E4 RID: 47076
				public static LocString TOOLTIP = "This Duplicant would really appreciate an " + UI.FormatAsLink("Outhouse", "OUTHOUSE") + " or " + UI.FormatAsLink("Lavatory", "FLUSHTOILET");
			}

			// Token: 0x02002BA7 RID: 11175
			public class STRESSFULLYEMPTYINGBLADDER
			{
				// Token: 0x0400B7E5 RID: 47077
				public static LocString NAME = "Making a mess";

				// Token: 0x0400B7E6 RID: 47078
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This poor Duplicant couldn't find an ",
					UI.FormatAsLink("Outhouse", "OUTHOUSE"),
					" in time and is super embarrassed\n\n",
					UI.FormatAsLink("Outhouses", "OUTHOUSE"),
					" can be built from the ",
					UI.FormatAsBuildMenuTab("Plumbing Tab", global::Action.Plan5)
				});

				// Token: 0x0400B7E7 RID: 47079
				public static LocString NOTIFICATION_NAME = "Made a mess";

				// Token: 0x0400B7E8 RID: 47080
				public static LocString NOTIFICATION_TOOLTIP = "The " + UI.FormatAsTool("Mop Tool", global::Action.Mop) + " can be used to clean up Duplicant-related \"spills\"\n\nThese Duplicants made messes that require cleaning up:\n";
			}

			// Token: 0x02002BA8 RID: 11176
			public class WASHINGHANDS
			{
				// Token: 0x0400B7E9 RID: 47081
				public static LocString NAME = "Washing hands";

				// Token: 0x0400B7EA RID: 47082
				public static LocString TOOLTIP = "This Duplicant is washing their hands";
			}

			// Token: 0x02002BA9 RID: 11177
			public class SHOWERING
			{
				// Token: 0x0400B7EB RID: 47083
				public static LocString NAME = "Showering";

				// Token: 0x0400B7EC RID: 47084
				public static LocString TOOLTIP = "This Duplicant is gonna be squeaky clean";
			}

			// Token: 0x02002BAA RID: 11178
			public class RELAXING
			{
				// Token: 0x0400B7ED RID: 47085
				public static LocString NAME = "Relaxing";

				// Token: 0x0400B7EE RID: 47086
				public static LocString TOOLTIP = "This Duplicant's just taking it easy";
			}

			// Token: 0x02002BAB RID: 11179
			public class VOMITING
			{
				// Token: 0x0400B7EF RID: 47087
				public static LocString NAME = "Throwing up";

				// Token: 0x0400B7F0 RID: 47088
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has unceremoniously hurled as the result of a ",
					UI.FormatAsLink("Disease", "DISEASE"),
					"\n\nDuplicant-related \"spills\" can be cleaned up using the ",
					UI.PRE_KEYWORD,
					"Mop Tool",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.Mop)
				});

				// Token: 0x0400B7F1 RID: 47089
				public static LocString NOTIFICATION_NAME = "Throwing up";

				// Token: 0x0400B7F2 RID: 47090
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop),
					" can be used to clean up Duplicant-related \"spills\"\n\nA ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" has caused these Duplicants to throw up:"
				});
			}

			// Token: 0x02002BAC RID: 11180
			public class STRESSVOMITING
			{
				// Token: 0x0400B7F3 RID: 47091
				public static LocString NAME = "Stress vomiting";

				// Token: 0x0400B7F4 RID: 47092
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is relieving their ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" all over the floor\n\nDuplicant-related \"spills\" can be cleaned up using the ",
					UI.PRE_KEYWORD,
					"Mop Tool",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.Mop)
				});

				// Token: 0x0400B7F5 RID: 47093
				public static LocString NOTIFICATION_NAME = "Stress vomiting";

				// Token: 0x0400B7F6 RID: 47094
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"The ",
					UI.FormatAsTool("Mop Tool", global::Action.Mop),
					" can used to clean up Duplicant-related \"spills\"\n\nThese Duplicants became so ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" they threw up:"
				});
			}

			// Token: 0x02002BAD RID: 11181
			public class RADIATIONVOMITING
			{
				// Token: 0x0400B7F7 RID: 47095
				public static LocString NAME = "Radiation vomiting";

				// Token: 0x0400B7F8 RID: 47096
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is sick due to ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" poisoning.\n\nDuplicant-related \"spills\" can be cleaned up using the ",
					UI.PRE_KEYWORD,
					"Mop Tool",
					UI.PST_KEYWORD,
					" ",
					UI.FormatAsHotKey(global::Action.Mop)
				});

				// Token: 0x0400B7F9 RID: 47097
				public static LocString NOTIFICATION_NAME = "Radiation vomiting";

				// Token: 0x0400B7FA RID: 47098
				public static LocString NOTIFICATION_TOOLTIP = "The " + UI.FormatAsTool("Mop Tool", global::Action.Mop) + " can clean up Duplicant-related \"spills\"\n\nRadiation Sickness caused these Duplicants to throw up:";
			}

			// Token: 0x02002BAE RID: 11182
			public class HASDISEASE
			{
				// Token: 0x0400B7FB RID: 47099
				public static LocString NAME = "Feeling ill";

				// Token: 0x0400B7FC RID: 47100
				public static LocString TOOLTIP = "This Duplicant has contracted a " + UI.FormatAsLink("Disease", "DISEASE") + " and requires recovery time at a " + UI.FormatAsLink("Sick Bay", "DOCTORSTATION");

				// Token: 0x0400B7FD RID: 47101
				public static LocString NOTIFICATION_NAME = "Illness";

				// Token: 0x0400B7FE RID: 47102
				public static LocString NOTIFICATION_TOOLTIP = string.Concat(new string[]
				{
					"These Duplicants have contracted a ",
					UI.FormatAsLink("Disease", "DISEASE"),
					" and require recovery time at a ",
					UI.FormatAsLink("Sick Bay", "DOCTORSTATION"),
					":"
				});
			}

			// Token: 0x02002BAF RID: 11183
			public class BODYREGULATINGHEATING
			{
				// Token: 0x0400B7FF RID: 47103
				public static LocString NAME = "Regulating temperature at: {TempDelta}";

				// Token: 0x0400B800 RID: 47104
				public static LocString TOOLTIP = "This Duplicant is regulating their internal " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x02002BB0 RID: 11184
			public class BODYREGULATINGCOOLING
			{
				// Token: 0x0400B801 RID: 47105
				public static LocString NAME = "Regulating temperature at: {TempDelta}";

				// Token: 0x0400B802 RID: 47106
				public static LocString TOOLTIP = "This Duplicant is regulating their internal " + UI.PRE_KEYWORD + "Temperature" + UI.PST_KEYWORD;
			}

			// Token: 0x02002BB1 RID: 11185
			public class BREATHINGO2
			{
				// Token: 0x0400B803 RID: 47107
				public static LocString NAME = "Inhaling {ConsumptionRate} O<sub>2</sub>";

				// Token: 0x0400B804 RID: 47108
				public static LocString TOOLTIP = "Duplicants require " + UI.FormatAsLink("Oxygen", "OXYGEN") + " to live";
			}

			// Token: 0x02002BB2 RID: 11186
			public class EMITTINGCO2
			{
				// Token: 0x0400B805 RID: 47109
				public static LocString NAME = "Exhaling {EmittingRate} CO<sub>2</sub>";

				// Token: 0x0400B806 RID: 47110
				public static LocString TOOLTIP = "Duplicants breathe out " + UI.FormatAsLink("Carbon Dioxide", "CARBONDIOXIDE");
			}

			// Token: 0x02002BB3 RID: 11187
			public class PICKUPDELIVERSTATUS
			{
				// Token: 0x0400B807 RID: 47111
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B808 RID: 47112
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BB4 RID: 11188
			public class STOREDELIVERSTATUS
			{
				// Token: 0x0400B809 RID: 47113
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B80A RID: 47114
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BB5 RID: 11189
			public class CLEARDELIVERSTATUS
			{
				// Token: 0x0400B80B RID: 47115
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B80C RID: 47116
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BB6 RID: 11190
			public class STOREFORBUILDDELIVERSTATUS
			{
				// Token: 0x0400B80D RID: 47117
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B80E RID: 47118
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BB7 RID: 11191
			public class STOREFORBUILDPRIORITIZEDDELIVERSTATUS
			{
				// Token: 0x0400B80F RID: 47119
				public static LocString NAME = "Allocating {Item} to {Target}";

				// Token: 0x0400B810 RID: 47120
				public static LocString TOOLTIP = "This Duplicant is delivering materials to a <b>{Target}</b> construction errand";
			}

			// Token: 0x02002BB8 RID: 11192
			public class BUILDDELIVERSTATUS
			{
				// Token: 0x0400B811 RID: 47121
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B812 RID: 47122
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BB9 RID: 11193
			public class BUILDPRIORITIZEDSTATUS
			{
				// Token: 0x0400B813 RID: 47123
				public static LocString NAME = "Building {Target}";

				// Token: 0x0400B814 RID: 47124
				public static LocString TOOLTIP = "This Duplicant is constructing a <b>{Target}</b>";
			}

			// Token: 0x02002BBA RID: 11194
			public class FABRICATEDELIVERSTATUS
			{
				// Token: 0x0400B815 RID: 47125
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B816 RID: 47126
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BBB RID: 11195
			public class USEITEMDELIVERSTATUS
			{
				// Token: 0x0400B817 RID: 47127
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B818 RID: 47128
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BBC RID: 11196
			public class STOREPRIORITYDELIVERSTATUS
			{
				// Token: 0x0400B819 RID: 47129
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B81A RID: 47130
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BBD RID: 11197
			public class STORECRITICALDELIVERSTATUS
			{
				// Token: 0x0400B81B RID: 47131
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B81C RID: 47132
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BBE RID: 11198
			public class COMPOSTFLIPSTATUS
			{
				// Token: 0x0400B81D RID: 47133
				public static LocString NAME = "Going to flip compost";

				// Token: 0x0400B81E RID: 47134
				public static LocString TOOLTIP = "This Duplicant is going to flip the " + BUILDINGS.PREFABS.COMPOST.NAME;
			}

			// Token: 0x02002BBF RID: 11199
			public class DECONSTRUCTDELIVERSTATUS
			{
				// Token: 0x0400B81F RID: 47135
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B820 RID: 47136
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BC0 RID: 11200
			public class TOGGLEDELIVERSTATUS
			{
				// Token: 0x0400B821 RID: 47137
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B822 RID: 47138
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BC1 RID: 11201
			public class EMPTYSTORAGEDELIVERSTATUS
			{
				// Token: 0x0400B823 RID: 47139
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B824 RID: 47140
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BC2 RID: 11202
			public class HARVESTDELIVERSTATUS
			{
				// Token: 0x0400B825 RID: 47141
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B826 RID: 47142
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BC3 RID: 11203
			public class SLEEPDELIVERSTATUS
			{
				// Token: 0x0400B827 RID: 47143
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B828 RID: 47144
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BC4 RID: 11204
			public class EATDELIVERSTATUS
			{
				// Token: 0x0400B829 RID: 47145
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B82A RID: 47146
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BC5 RID: 11205
			public class WARMUPDELIVERSTATUS
			{
				// Token: 0x0400B82B RID: 47147
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B82C RID: 47148
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BC6 RID: 11206
			public class REPAIRDELIVERSTATUS
			{
				// Token: 0x0400B82D RID: 47149
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B82E RID: 47150
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BC7 RID: 11207
			public class REPAIRWORKSTATUS
			{
				// Token: 0x0400B82F RID: 47151
				public static LocString NAME = "Repairing {Target}";

				// Token: 0x0400B830 RID: 47152
				public static LocString TOOLTIP = "This Duplicant is fixing the <b>{Target}</b>";
			}

			// Token: 0x02002BC8 RID: 11208
			public class BREAKDELIVERSTATUS
			{
				// Token: 0x0400B831 RID: 47153
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B832 RID: 47154
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BC9 RID: 11209
			public class BREAKWORKSTATUS
			{
				// Token: 0x0400B833 RID: 47155
				public static LocString NAME = "Breaking {Target}";

				// Token: 0x0400B834 RID: 47156
				public static LocString TOOLTIP = "This Duplicant is going totally bananas on the <b>{Target}</b>!";
			}

			// Token: 0x02002BCA RID: 11210
			public class EQUIPDELIVERSTATUS
			{
				// Token: 0x0400B835 RID: 47157
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B836 RID: 47158
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BCB RID: 11211
			public class COOKDELIVERSTATUS
			{
				// Token: 0x0400B837 RID: 47159
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B838 RID: 47160
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BCC RID: 11212
			public class MUSHDELIVERSTATUS
			{
				// Token: 0x0400B839 RID: 47161
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B83A RID: 47162
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BCD RID: 11213
			public class PACIFYDELIVERSTATUS
			{
				// Token: 0x0400B83B RID: 47163
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B83C RID: 47164
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BCE RID: 11214
			public class RESCUEDELIVERSTATUS
			{
				// Token: 0x0400B83D RID: 47165
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B83E RID: 47166
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BCF RID: 11215
			public class RESCUEWORKSTATUS
			{
				// Token: 0x0400B83F RID: 47167
				public static LocString NAME = "Rescuing {Target}";

				// Token: 0x0400B840 RID: 47168
				public static LocString TOOLTIP = "This Duplicant is saving <b>{Target}</b> from certain peril!";
			}

			// Token: 0x02002BD0 RID: 11216
			public class MOPDELIVERSTATUS
			{
				// Token: 0x0400B841 RID: 47169
				public static LocString NAME = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.NAME;

				// Token: 0x0400B842 RID: 47170
				public static LocString TOOLTIP = DUPLICANTS.STATUSITEMS.GENERIC_DELIVER.TOOLTIP;
			}

			// Token: 0x02002BD1 RID: 11217
			public class DIGGING
			{
				// Token: 0x0400B843 RID: 47171
				public static LocString NAME = "Digging";

				// Token: 0x0400B844 RID: 47172
				public static LocString TOOLTIP = "This Duplicant is excavating raw resources";
			}

			// Token: 0x02002BD2 RID: 11218
			public class EATING
			{
				// Token: 0x0400B845 RID: 47173
				public static LocString NAME = "Eating {Target}";

				// Token: 0x0400B846 RID: 47174
				public static LocString TOOLTIP = "This Duplicant is having a meal";
			}

			// Token: 0x02002BD3 RID: 11219
			public class CLEANING
			{
				// Token: 0x0400B847 RID: 47175
				public static LocString NAME = "Cleaning {Target}";

				// Token: 0x0400B848 RID: 47176
				public static LocString TOOLTIP = "This Duplicant is cleaning the <b>{Target}</b>";
			}

			// Token: 0x02002BD4 RID: 11220
			public class LIGHTWORKEFFICIENCYBONUS
			{
				// Token: 0x0400B849 RID: 47177
				public static LocString NAME = "Lit Workspace";

				// Token: 0x0400B84A RID: 47178
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Better visibility from the ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" is allowing this Duplicant to work faster:\n    {0}"
				});

				// Token: 0x0400B84B RID: 47179
				public static LocString NO_BUILDING_WORK_ATTRIBUTE = "{0} Speed";
			}

			// Token: 0x02002BD5 RID: 11221
			public class LABORATORYWORKEFFICIENCYBONUS
			{
				// Token: 0x0400B84C RID: 47180
				public static LocString NAME = "Lab Workspace";

				// Token: 0x0400B84D RID: 47181
				public static LocString TOOLTIP = "Working in a Laboratory is allowing this Duplicant to work faster:\n    {0}";

				// Token: 0x0400B84E RID: 47182
				public static LocString NO_BUILDING_WORK_ATTRIBUTE = "{0} Speed";
			}

			// Token: 0x02002BD6 RID: 11222
			public class PICKINGUP
			{
				// Token: 0x0400B84F RID: 47183
				public static LocString NAME = "Picking up {Target}";

				// Token: 0x0400B850 RID: 47184
				public static LocString TOOLTIP = "This Duplicant is retrieving <b>{Target}</b>";
			}

			// Token: 0x02002BD7 RID: 11223
			public class MOPPING
			{
				// Token: 0x0400B851 RID: 47185
				public static LocString NAME = "Mopping";

				// Token: 0x0400B852 RID: 47186
				public static LocString TOOLTIP = "This Duplicant is cleaning up a nasty spill";
			}

			// Token: 0x02002BD8 RID: 11224
			public class ARTING
			{
				// Token: 0x0400B853 RID: 47187
				public static LocString NAME = "Decorating";

				// Token: 0x0400B854 RID: 47188
				public static LocString TOOLTIP = "This Duplicant is hard at work on their art";
			}

			// Token: 0x02002BD9 RID: 11225
			public class MUSHING
			{
				// Token: 0x0400B855 RID: 47189
				public static LocString NAME = "Mushing {Item}";

				// Token: 0x0400B856 RID: 47190
				public static LocString TOOLTIP = "This Duplicant is cooking a <b>{Item}</b>";
			}

			// Token: 0x02002BDA RID: 11226
			public class COOKING
			{
				// Token: 0x0400B857 RID: 47191
				public static LocString NAME = "Cooking {Item}";

				// Token: 0x0400B858 RID: 47192
				public static LocString TOOLTIP = "This Duplicant is cooking up a tasty <b>{Item}</b>";
			}

			// Token: 0x02002BDB RID: 11227
			public class RESEARCHING
			{
				// Token: 0x0400B859 RID: 47193
				public static LocString NAME = "Researching {Tech}";

				// Token: 0x0400B85A RID: 47194
				public static LocString TOOLTIP = "This Duplicant is intently researching <b>{Tech}</b> technology";
			}

			// Token: 0x02002BDC RID: 11228
			public class MISSIONCONTROLLING
			{
				// Token: 0x0400B85B RID: 47195
				public static LocString NAME = "Mission Controlling";

				// Token: 0x0400B85C RID: 47196
				public static LocString TOOLTIP = "This Duplicant is guiding a " + UI.PRE_KEYWORD + "Rocket" + UI.PST_KEYWORD;
			}

			// Token: 0x02002BDD RID: 11229
			public class STORING
			{
				// Token: 0x0400B85D RID: 47197
				public static LocString NAME = "Storing {Item}";

				// Token: 0x0400B85E RID: 47198
				public static LocString TOOLTIP = "This Duplicant is putting <b>{Item}</b> away in <b>{Target}</b>";
			}

			// Token: 0x02002BDE RID: 11230
			public class BUILDING
			{
				// Token: 0x0400B85F RID: 47199
				public static LocString NAME = "Building {Target}";

				// Token: 0x0400B860 RID: 47200
				public static LocString TOOLTIP = "This Duplicant is constructing a <b>{Target}</b>";
			}

			// Token: 0x02002BDF RID: 11231
			public class EQUIPPING
			{
				// Token: 0x0400B861 RID: 47201
				public static LocString NAME = "Equipping {Target}";

				// Token: 0x0400B862 RID: 47202
				public static LocString TOOLTIP = "This Duplicant is equipping a <b>{Target}</b>";
			}

			// Token: 0x02002BE0 RID: 11232
			public class WARMINGUP
			{
				// Token: 0x0400B863 RID: 47203
				public static LocString NAME = "Warming up";

				// Token: 0x0400B864 RID: 47204
				public static LocString TOOLTIP = "This Duplicant got too cold and is trying to warm up";
			}

			// Token: 0x02002BE1 RID: 11233
			public class GENERATINGPOWER
			{
				// Token: 0x0400B865 RID: 47205
				public static LocString NAME = "Generating power";

				// Token: 0x0400B866 RID: 47206
				public static LocString TOOLTIP = "This Duplicant is using the <b>{Target}</b> to produce electrical " + UI.PRE_KEYWORD + "Power" + UI.PST_KEYWORD;
			}

			// Token: 0x02002BE2 RID: 11234
			public class HARVESTING
			{
				// Token: 0x0400B867 RID: 47207
				public static LocString NAME = "Harvesting {Target}";

				// Token: 0x0400B868 RID: 47208
				public static LocString TOOLTIP = "This Duplicant is gathering resources from a <b>{Target}</b>";
			}

			// Token: 0x02002BE3 RID: 11235
			public class UPROOTING
			{
				// Token: 0x0400B869 RID: 47209
				public static LocString NAME = "Uprooting {Target}";

				// Token: 0x0400B86A RID: 47210
				public static LocString TOOLTIP = "This Duplicant is digging up a <b>{Target}</b>";
			}

			// Token: 0x02002BE4 RID: 11236
			public class EMPTYING
			{
				// Token: 0x0400B86B RID: 47211
				public static LocString NAME = "Emptying {Target}";

				// Token: 0x0400B86C RID: 47212
				public static LocString TOOLTIP = "This Duplicant is removing materials from the <b>{Target}</b>";
			}

			// Token: 0x02002BE5 RID: 11237
			public class TOGGLING
			{
				// Token: 0x0400B86D RID: 47213
				public static LocString NAME = "Change {Target} setting";

				// Token: 0x0400B86E RID: 47214
				public static LocString TOOLTIP = "This Duplicant is changing the <b>{Target}</b>'s setting";
			}

			// Token: 0x02002BE6 RID: 11238
			public class DECONSTRUCTING
			{
				// Token: 0x0400B86F RID: 47215
				public static LocString NAME = "Deconstructing {Target}";

				// Token: 0x0400B870 RID: 47216
				public static LocString TOOLTIP = "This Duplicant is deconstructing the <b>{Target}</b>";
			}

			// Token: 0x02002BE7 RID: 11239
			public class DEMOLISHING
			{
				// Token: 0x0400B871 RID: 47217
				public static LocString NAME = "Demolishing {Target}";

				// Token: 0x0400B872 RID: 47218
				public static LocString TOOLTIP = "This Duplicant is demolishing the <b>{Target}</b>";
			}

			// Token: 0x02002BE8 RID: 11240
			public class DISINFECTING
			{
				// Token: 0x0400B873 RID: 47219
				public static LocString NAME = "Disinfecting {Target}";

				// Token: 0x0400B874 RID: 47220
				public static LocString TOOLTIP = "This Duplicant is disinfecting <b>{Target}</b>";
			}

			// Token: 0x02002BE9 RID: 11241
			public class FABRICATING
			{
				// Token: 0x0400B875 RID: 47221
				public static LocString NAME = "Fabricating {Item}";

				// Token: 0x0400B876 RID: 47222
				public static LocString TOOLTIP = "This Duplicant is crafting a <b>{Item}</b>";
			}

			// Token: 0x02002BEA RID: 11242
			public class PROCESSING
			{
				// Token: 0x0400B877 RID: 47223
				public static LocString NAME = "Refining {Item}";

				// Token: 0x0400B878 RID: 47224
				public static LocString TOOLTIP = "This Duplicant is refining <b>{Item}</b>";
			}

			// Token: 0x02002BEB RID: 11243
			public class SPICING
			{
				// Token: 0x0400B879 RID: 47225
				public static LocString NAME = "Spicing Food";

				// Token: 0x0400B87A RID: 47226
				public static LocString TOOLTIP = "This Duplicant is making a tasty meal even tastier";
			}

			// Token: 0x02002BEC RID: 11244
			public class CLEARING
			{
				// Token: 0x0400B87B RID: 47227
				public static LocString NAME = "Sweeping {Target}";

				// Token: 0x0400B87C RID: 47228
				public static LocString TOOLTIP = "This Duplicant is sweeping away <b>{Target}</b>";
			}

			// Token: 0x02002BED RID: 11245
			public class STUDYING
			{
				// Token: 0x0400B87D RID: 47229
				public static LocString NAME = "Analyzing";

				// Token: 0x0400B87E RID: 47230
				public static LocString TOOLTIP = "This Duplicant is conducting a field study of a Natural Feature";
			}

			// Token: 0x02002BEE RID: 11246
			public class SOCIALIZING
			{
				// Token: 0x0400B87F RID: 47231
				public static LocString NAME = "Socializing";

				// Token: 0x0400B880 RID: 47232
				public static LocString TOOLTIP = "This Duplicant is using their break to hang out";
			}

			// Token: 0x02002BEF RID: 11247
			public class MINGLING
			{
				// Token: 0x0400B881 RID: 47233
				public static LocString NAME = "Mingling";

				// Token: 0x0400B882 RID: 47234
				public static LocString TOOLTIP = "This Duplicant is using their break to chat with friends";
			}

			// Token: 0x02002BF0 RID: 11248
			public class NOISEPEACEFUL
			{
				// Token: 0x0400B883 RID: 47235
				public static LocString NAME = "Peace and Quiet";

				// Token: 0x0400B884 RID: 47236
				public static LocString TOOLTIP = "This Duplicant has found a quiet place to concentrate";
			}

			// Token: 0x02002BF1 RID: 11249
			public class NOISEMINOR
			{
				// Token: 0x0400B885 RID: 47237
				public static LocString NAME = "Loud Noises";

				// Token: 0x0400B886 RID: 47238
				public static LocString TOOLTIP = "This area is a bit too loud for comfort";
			}

			// Token: 0x02002BF2 RID: 11250
			public class NOISEMAJOR
			{
				// Token: 0x0400B887 RID: 47239
				public static LocString NAME = "Cacophony!";

				// Token: 0x0400B888 RID: 47240
				public static LocString TOOLTIP = "It's very, very loud in here!";
			}

			// Token: 0x02002BF3 RID: 11251
			public class LOWIMMUNITY
			{
				// Token: 0x0400B889 RID: 47241
				public static LocString NAME = "Under the Weather";

				// Token: 0x0400B88A RID: 47242
				public static LocString TOOLTIP = "This Duplicant has a weakened immune system and will become ill if it reaches zero";

				// Token: 0x0400B88B RID: 47243
				public static LocString NOTIFICATION_NAME = "Low Immunity";

				// Token: 0x0400B88C RID: 47244
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants are at risk of becoming sick:";
			}

			// Token: 0x02002BF4 RID: 11252
			public abstract class TINKERING
			{
				// Token: 0x0400B88D RID: 47245
				public static LocString NAME = "Tinkering";

				// Token: 0x0400B88E RID: 47246
				public static LocString TOOLTIP = "This Duplicant is making functional improvements to a building";
			}

			// Token: 0x02002BF5 RID: 11253
			public class CONTACTWITHGERMS
			{
				// Token: 0x0400B88F RID: 47247
				public static LocString NAME = "Contact with {Sickness} Germs";

				// Token: 0x0400B890 RID: 47248
				public static LocString TOOLTIP = "This Duplicant has encountered {Sickness} Germs and is at risk of dangerous exposure if contact continues\n\n<i>" + UI.CLICK(UI.ClickType.Click) + " to jump to last contact location</i>";
			}

			// Token: 0x02002BF6 RID: 11254
			public class EXPOSEDTOGERMS
			{
				// Token: 0x0400B891 RID: 47249
				public static LocString TIER1 = "Mild Exposure";

				// Token: 0x0400B892 RID: 47250
				public static LocString TIER2 = "Medium Exposure";

				// Token: 0x0400B893 RID: 47251
				public static LocString TIER3 = "Exposure";

				// Token: 0x0400B894 RID: 47252
				public static readonly LocString[] EXPOSURE_TIERS = new LocString[]
				{
					DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.TIER1,
					DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.TIER2,
					DUPLICANTS.STATUSITEMS.EXPOSEDTOGERMS.TIER3
				};

				// Token: 0x0400B895 RID: 47253
				public static LocString NAME = "{Severity} to {Sickness} Germs";

				// Token: 0x0400B896 RID: 47254
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has been exposed to a concentration of {Sickness} Germs and is at risk of waking up sick on their next shift\n\nExposed {Source}\n\nRate of Contracting {Sickness}: {Chance}\n\nResistance Rating: {Total}\n    • Base {Sickness} Resistance: {Base}\n    • ",
					DUPLICANTS.ATTRIBUTES.GERMRESISTANCE.NAME,
					": {Dupe}\n    • {Severity} Exposure: {ExposureLevelBonus}\n\n<i>",
					UI.CLICK(UI.ClickType.Click),
					" to jump to last exposure location</i>"
				});
			}

			// Token: 0x02002BF7 RID: 11255
			public class GASLIQUIDEXPOSURE
			{
				// Token: 0x0400B897 RID: 47255
				public static LocString NAME_MINOR = "Eye Irritation";

				// Token: 0x0400B898 RID: 47256
				public static LocString NAME_MAJOR = "Major Eye Irritation";

				// Token: 0x0400B899 RID: 47257
				public static LocString TOOLTIP = "Ah, it stings!\n\nThis poor Duplicant got a faceful of an irritating gas or liquid";

				// Token: 0x0400B89A RID: 47258
				public static LocString TOOLTIP_EXPOSED = "Current exposure to {element} is {rate} eye irritation";

				// Token: 0x0400B89B RID: 47259
				public static LocString TOOLTIP_RATE_INCREASE = "increasing";

				// Token: 0x0400B89C RID: 47260
				public static LocString TOOLTIP_RATE_DECREASE = "decreasing";

				// Token: 0x0400B89D RID: 47261
				public static LocString TOOLTIP_RATE_STAYS = "maintaining";

				// Token: 0x0400B89E RID: 47262
				public static LocString TOOLTIP_EXPOSURE_LEVEL = "Time Remaining: {time}";
			}

			// Token: 0x02002BF8 RID: 11256
			public class BEINGPRODUCTIVE
			{
				// Token: 0x0400B89F RID: 47263
				public static LocString NAME = "Super Focused";

				// Token: 0x0400B8A0 RID: 47264
				public static LocString TOOLTIP = "This Duplicant is focused on being super productive right now";
			}

			// Token: 0x02002BF9 RID: 11257
			public class BALLOONARTISTPLANNING
			{
				// Token: 0x0400B8A1 RID: 47265
				public static LocString NAME = "Balloon Artist";

				// Token: 0x0400B8A2 RID: 47266
				public static LocString TOOLTIP = "This Duplicant is planning to hand out balloons in their downtime";
			}

			// Token: 0x02002BFA RID: 11258
			public class BALLOONARTISTHANDINGOUT
			{
				// Token: 0x0400B8A3 RID: 47267
				public static LocString NAME = "Balloon Artist";

				// Token: 0x0400B8A4 RID: 47268
				public static LocString TOOLTIP = "This Duplicant is handing out balloons to other Duplicants";
			}

			// Token: 0x02002BFB RID: 11259
			public class EXPELLINGRADS
			{
				// Token: 0x0400B8A5 RID: 47269
				public static LocString NAME = "Cleansing Rads";

				// Token: 0x0400B8A6 RID: 47270
				public static LocString TOOLTIP = "This Duplicant is, uh... \"expelling\" absorbed radiation from their system";
			}

			// Token: 0x02002BFC RID: 11260
			public class ANALYZINGGENES
			{
				// Token: 0x0400B8A7 RID: 47271
				public static LocString NAME = "Analyzing Plant Genes";

				// Token: 0x0400B8A8 RID: 47272
				public static LocString TOOLTIP = "This Duplicant is peering deep into the genetic code of an odd seed";
			}

			// Token: 0x02002BFD RID: 11261
			public class ANALYZINGARTIFACT
			{
				// Token: 0x0400B8A9 RID: 47273
				public static LocString NAME = "Analyzing Artifact";

				// Token: 0x0400B8AA RID: 47274
				public static LocString TOOLTIP = "This Duplicant is studying an artifact";
			}

			// Token: 0x02002BFE RID: 11262
			public class RANCHING
			{
				// Token: 0x0400B8AB RID: 47275
				public static LocString NAME = "Ranching";

				// Token: 0x0400B8AC RID: 47276
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is tending to a ",
					UI.PRE_KEYWORD,
					"Critter",
					UI.PST_KEYWORD,
					"'s well-being"
				});
			}
		}

		// Token: 0x02001DFB RID: 7675
		public class DISEASES
		{
			// Token: 0x040089CD RID: 35277
			public static LocString CURED_POPUP = "Cured of {0}";

			// Token: 0x040089CE RID: 35278
			public static LocString INFECTED_POPUP = "Became infected by {0}";

			// Token: 0x040089CF RID: 35279
			public static LocString ADDED_POPFX = "{0}: {1} Germs";

			// Token: 0x040089D0 RID: 35280
			public static LocString NOTIFICATION_TOOLTIP = "{0} contracted {1} from: {2}";

			// Token: 0x040089D1 RID: 35281
			public static LocString GERMS = "Germs";

			// Token: 0x040089D2 RID: 35282
			public static LocString GERMS_CONSUMED_DESCRIPTION = "A count of the number of germs this Duplicant is host to";

			// Token: 0x040089D3 RID: 35283
			public static LocString RECUPERATING = "Recuperating";

			// Token: 0x040089D4 RID: 35284
			public static LocString INFECTION_MODIFIER = "Recently consumed {0} ({1})";

			// Token: 0x040089D5 RID: 35285
			public static LocString INFECTION_MODIFIER_SOURCE = "Fighting off {0} from {1}";

			// Token: 0x040089D6 RID: 35286
			public static LocString INFECTED_MODIFIER = "Suppressed immune system";

			// Token: 0x040089D7 RID: 35287
			public static LocString LEGEND_POSTAMBLE = "\n•  Select an infected object for more details";

			// Token: 0x040089D8 RID: 35288
			public static LocString ATTRIBUTE_MODIFIER_SYMPTOMS = "{0}: {1}";

			// Token: 0x040089D9 RID: 35289
			public static LocString ATTRIBUTE_MODIFIER_SYMPTOMS_TOOLTIP = "Modifies {0} by {1}";

			// Token: 0x040089DA RID: 35290
			public static LocString DEATH_SYMPTOM = "Death in {0} if untreated";

			// Token: 0x040089DB RID: 35291
			public static LocString DEATH_SYMPTOM_TOOLTIP = "Without medical treatment, this Duplicant will die of their illness in {0}";

			// Token: 0x040089DC RID: 35292
			public static LocString RESISTANCES_PANEL_TOOLTIP = "{0}";

			// Token: 0x040089DD RID: 35293
			public static LocString IMMUNE_FROM_MISSING_REQUIRED_TRAIT = "Immune: Does not have {0}";

			// Token: 0x040089DE RID: 35294
			public static LocString IMMUNE_FROM_HAVING_EXLCLUDED_TRAIT = "Immune: Has {0}";

			// Token: 0x040089DF RID: 35295
			public static LocString IMMUNE_FROM_HAVING_EXCLUDED_EFFECT = "Immunity: Has {0}";

			// Token: 0x040089E0 RID: 35296
			public static LocString CONTRACTION_PROBABILITY = "{0} of {1}'s exposures to these germs will result in {2}";

			// Token: 0x02002BFF RID: 11263
			public class STATUS_ITEM_TOOLTIP
			{
				// Token: 0x0400B8AD RID: 47277
				public static LocString TEMPLATE = "{InfectionSource}{Duration}{Doctor}{Fatality}{Cures}{Bedrest}\n\n\n{Symptoms}";

				// Token: 0x0400B8AE RID: 47278
				public static LocString DESCRIPTOR = "<b>{0} {1}</b>\n";

				// Token: 0x0400B8AF RID: 47279
				public static LocString SYMPTOMS = "{0}\n";

				// Token: 0x0400B8B0 RID: 47280
				public static LocString INFECTION_SOURCE = "Contracted by: {0}\n";

				// Token: 0x0400B8B1 RID: 47281
				public static LocString DURATION = "Time to recovery: {0}\n";

				// Token: 0x0400B8B2 RID: 47282
				public static LocString CURES = "Remedies taken: {0}\n";

				// Token: 0x0400B8B3 RID: 47283
				public static LocString NOMEDICINETAKEN = "Remedies taken: None\n";

				// Token: 0x0400B8B4 RID: 47284
				public static LocString FATALITY = "Fatal if untreated in: {0}\n";

				// Token: 0x0400B8B5 RID: 47285
				public static LocString BEDREST = "Sick Bay assignment will allow faster recovery\n";

				// Token: 0x0400B8B6 RID: 47286
				public static LocString DOCTOR_REQUIRED = "Sick Bay assignment required for recovery\n";

				// Token: 0x0400B8B7 RID: 47287
				public static LocString DOCTORED = "Received medical treatment, recovery speed is increased\n";
			}

			// Token: 0x02002C00 RID: 11264
			public class MEDICINE
			{
				// Token: 0x0400B8B8 RID: 47288
				public static LocString SELF_ADMINISTERED_BOOSTER = "Self-Administered: Anytime";

				// Token: 0x0400B8B9 RID: 47289
				public static LocString SELF_ADMINISTERED_BOOSTER_TOOLTIP = "Duplicants can give themselves this medicine, whether they are currently sick or not";

				// Token: 0x0400B8BA RID: 47290
				public static LocString SELF_ADMINISTERED_CURE = "Self-Administered: Sick Only";

				// Token: 0x0400B8BB RID: 47291
				public static LocString SELF_ADMINISTERED_CURE_TOOLTIP = "Duplicants can give themselves this medicine, but only while they are sick";

				// Token: 0x0400B8BC RID: 47292
				public static LocString DOCTOR_ADMINISTERED_BOOSTER = "Doctor Administered: Anytime";

				// Token: 0x0400B8BD RID: 47293
				public static LocString DOCTOR_ADMINISTERED_BOOSTER_TOOLTIP = "Duplicants can receive this medicine at a {Station}, whether they are currently sick or not\n\nThey cannot give it to themselves and must receive it from a friend with " + UI.PRE_KEYWORD + "Doctoring Skills" + UI.PST_KEYWORD;

				// Token: 0x0400B8BE RID: 47294
				public static LocString DOCTOR_ADMINISTERED_CURE = "Doctor Administered: Sick Only";

				// Token: 0x0400B8BF RID: 47295
				public static LocString DOCTOR_ADMINISTERED_CURE_TOOLTIP = "Duplicants can receive this medicine at a {Station}, but only while they are sick\n\nThey cannot give it to themselves and must receive it from a friend with " + UI.PRE_KEYWORD + "Doctoring Skills" + UI.PST_KEYWORD;

				// Token: 0x0400B8C0 RID: 47296
				public static LocString BOOSTER = UI.FormatAsLink("Immune Booster", "IMMUNE SYSTEM");

				// Token: 0x0400B8C1 RID: 47297
				public static LocString BOOSTER_TOOLTIP = "Boosters can be taken by both healthy and sick Duplicants to prevent potential disease";

				// Token: 0x0400B8C2 RID: 47298
				public static LocString CURES_ANY = "Alleviates " + UI.FormatAsLink("All Diseases", "DISEASE");

				// Token: 0x0400B8C3 RID: 47299
				public static LocString CURES_ANY_TOOLTIP = string.Concat(new string[]
				{
					"This is a nonspecific ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" treatment that can be taken by any sick Duplicant"
				});

				// Token: 0x0400B8C4 RID: 47300
				public static LocString CURES = "Alleviates {0}";

				// Token: 0x0400B8C5 RID: 47301
				public static LocString CURES_TOOLTIP = "This medicine is used to treat {0} and can only be taken by sick Duplicants";
			}

			// Token: 0x02002C01 RID: 11265
			public class SEVERITY
			{
				// Token: 0x0400B8C6 RID: 47302
				public static LocString BENIGN = "Benign";

				// Token: 0x0400B8C7 RID: 47303
				public static LocString MINOR = "Minor";

				// Token: 0x0400B8C8 RID: 47304
				public static LocString MAJOR = "Major";

				// Token: 0x0400B8C9 RID: 47305
				public static LocString CRITICAL = "Critical";
			}

			// Token: 0x02002C02 RID: 11266
			public class TYPE
			{
				// Token: 0x0400B8CA RID: 47306
				public static LocString PATHOGEN = "Illness";

				// Token: 0x0400B8CB RID: 47307
				public static LocString AILMENT = "Ailment";

				// Token: 0x0400B8CC RID: 47308
				public static LocString INJURY = "Injury";
			}

			// Token: 0x02002C03 RID: 11267
			public class TRIGGERS
			{
				// Token: 0x0400B8CD RID: 47309
				public static LocString EATCOMPLETEEDIBLE = "May cause {Diseases}";

				// Token: 0x020031D7 RID: 12759
				public class TOOLTIPS
				{
					// Token: 0x0400C709 RID: 50953
					public static LocString EATCOMPLETEEDIBLE = "May cause {Diseases}";
				}
			}

			// Token: 0x02002C04 RID: 11268
			public class INFECTIONSOURCES
			{
				// Token: 0x0400B8CE RID: 47310
				public static LocString INTERNAL_TEMPERATURE = "Extreme internal temperatures";

				// Token: 0x0400B8CF RID: 47311
				public static LocString TOXIC_AREA = "Exposure to toxic areas";

				// Token: 0x0400B8D0 RID: 47312
				public static LocString FOOD = "Eating a germ-covered {0}";

				// Token: 0x0400B8D1 RID: 47313
				public static LocString AIR = "Breathing germ-filled {0}";

				// Token: 0x0400B8D2 RID: 47314
				public static LocString SKIN = "Skin contamination";

				// Token: 0x0400B8D3 RID: 47315
				public static LocString UNKNOWN = "Unknown source";
			}

			// Token: 0x02002C05 RID: 11269
			public class DESCRIPTORS
			{
				// Token: 0x020031D8 RID: 12760
				public class INFO
				{
					// Token: 0x0400C70A RID: 50954
					public static LocString FOODBORNE = "Contracted via ingestion\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400C70B RID: 50955
					public static LocString FOODBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" by ingesting ",
						UI.PRE_KEYWORD,
						"Food",
						UI.PST_KEYWORD,
						" contaminated with these ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C70C RID: 50956
					public static LocString AIRBORNE = "Contracted via inhalation\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400C70D RID: 50957
					public static LocString AIRBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" by breathing ",
						ELEMENTS.OXYGEN.NAME,
						" containing these ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C70E RID: 50958
					public static LocString SKINBORNE = "Contracted via physical contact\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400C70F RID: 50959
					public static LocString SKINBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" by touching objects contaminated with these ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD
					});

					// Token: 0x0400C710 RID: 50960
					public static LocString SUNBORNE = "Contracted via environmental exposure\n" + UI.HORIZONTAL_RULE;

					// Token: 0x0400C711 RID: 50961
					public static LocString SUNBORNE_TOOLTIP = string.Concat(new string[]
					{
						"Duplicants may contract this ",
						UI.PRE_KEYWORD,
						"Disease",
						UI.PST_KEYWORD,
						" through exposure to hazardous environments"
					});

					// Token: 0x0400C712 RID: 50962
					public static LocString GROWS_ON = "Multiplies in:";

					// Token: 0x0400C713 RID: 50963
					public static LocString GROWS_ON_TOOLTIP = string.Concat(new string[]
					{
						"These substances allow ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" to spread and reproduce"
					});

					// Token: 0x0400C714 RID: 50964
					public static LocString NEUTRAL_ON = "Survives in:";

					// Token: 0x0400C715 RID: 50965
					public static LocString NEUTRAL_ON_TOOLTIP = UI.PRE_KEYWORD + "Germs" + UI.PST_KEYWORD + " will survive contact with these substances, but will not reproduce";

					// Token: 0x0400C716 RID: 50966
					public static LocString DIES_SLOWLY_ON = "Inhibited by:";

					// Token: 0x0400C717 RID: 50967
					public static LocString DIES_SLOWLY_ON_TOOLTIP = string.Concat(new string[]
					{
						"Contact with these substances will slowly reduce ",
						UI.PRE_KEYWORD,
						"Germ",
						UI.PST_KEYWORD,
						" numbers"
					});

					// Token: 0x0400C718 RID: 50968
					public static LocString DIES_ON = "Killed by:";

					// Token: 0x0400C719 RID: 50969
					public static LocString DIES_ON_TOOLTIP = string.Concat(new string[]
					{
						"Contact with these substances kills ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" over time"
					});

					// Token: 0x0400C71A RID: 50970
					public static LocString DIES_QUICKLY_ON = "Disinfected by:";

					// Token: 0x0400C71B RID: 50971
					public static LocString DIES_QUICKLY_ON_TOOLTIP = "Contact with these substances will quickly kill these " + UI.PRE_KEYWORD + "Germs" + UI.PST_KEYWORD;

					// Token: 0x0400C71C RID: 50972
					public static LocString GROWS = "Multiplies";

					// Token: 0x0400C71D RID: 50973
					public static LocString GROWS_TOOLTIP = "Doubles germ count every {0}";

					// Token: 0x0400C71E RID: 50974
					public static LocString NEUTRAL = "Survives";

					// Token: 0x0400C71F RID: 50975
					public static LocString NEUTRAL_TOOLTIP = "Germ count remains static";

					// Token: 0x0400C720 RID: 50976
					public static LocString DIES_SLOWLY = "Inhibited";

					// Token: 0x0400C721 RID: 50977
					public static LocString DIES_SLOWLY_TOOLTIP = "Halves germ count every {0}";

					// Token: 0x0400C722 RID: 50978
					public static LocString DIES = "Dies";

					// Token: 0x0400C723 RID: 50979
					public static LocString DIES_TOOLTIP = "Halves germ count every {0}";

					// Token: 0x0400C724 RID: 50980
					public static LocString DIES_QUICKLY = "Disinfected";

					// Token: 0x0400C725 RID: 50981
					public static LocString DIES_QUICKLY_TOOLTIP = "Halves germ count every {0}";

					// Token: 0x0400C726 RID: 50982
					public static LocString GROWTH_FORMAT = "    • {0}";

					// Token: 0x0400C727 RID: 50983
					public static LocString TEMPERATURE_RANGE = "Temperature range: {0} to {1}";

					// Token: 0x0400C728 RID: 50984
					public static LocString TEMPERATURE_RANGE_TOOLTIP = string.Concat(new string[]
					{
						"These ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" can survive ",
						UI.PRE_KEYWORD,
						"Temperatures",
						UI.PST_KEYWORD,
						" between <b>{0}</b> and <b>{1}</b>\n\nThey thrive in ",
						UI.PRE_KEYWORD,
						"Temperatures",
						UI.PST_KEYWORD,
						" between <b>{2}</b> and <b>{3}</b>"
					});

					// Token: 0x0400C729 RID: 50985
					public static LocString PRESSURE_RANGE = "Pressure range: {0} to {1}\n";

					// Token: 0x0400C72A RID: 50986
					public static LocString PRESSURE_RANGE_TOOLTIP = string.Concat(new string[]
					{
						"These ",
						UI.PRE_KEYWORD,
						"Germs",
						UI.PST_KEYWORD,
						" can survive between <b>{0}</b> and <b>{1}</b> of pressure\n\nThey thrive in pressures between <b>{2}</b> and <b>{3}</b>"
					});
				}
			}

			// Token: 0x02002C06 RID: 11270
			public class ALLDISEASES
			{
				// Token: 0x0400B8D4 RID: 47316
				public static LocString NAME = "All Diseases";
			}

			// Token: 0x02002C07 RID: 11271
			public class NODISEASES
			{
				// Token: 0x0400B8D5 RID: 47317
				public static LocString NAME = "NO";
			}

			// Token: 0x02002C08 RID: 11272
			public class FOODPOISONING
			{
				// Token: 0x0400B8D6 RID: 47318
				public static LocString NAME = UI.FormatAsLink("Food Poisoning", "FOODPOISONING");

				// Token: 0x0400B8D7 RID: 47319
				public static LocString LEGEND_HOVERTEXT = "Food Poisoning Germs present\n";

				// Token: 0x0400B8D8 RID: 47320
				public static LocString DESC = "Food and drinks tainted with Food Poisoning germs are unsafe to consume, as they cause vomiting and other...bodily unpleasantness.";
			}

			// Token: 0x02002C09 RID: 11273
			public class SLIMELUNG
			{
				// Token: 0x0400B8D9 RID: 47321
				public static LocString NAME = UI.FormatAsLink("Slimelung", "SLIMELUNG");

				// Token: 0x0400B8DA RID: 47322
				public static LocString LEGEND_HOVERTEXT = "Slimelung Germs present\n";

				// Token: 0x0400B8DB RID: 47323
				public static LocString DESC = string.Concat(new string[]
				{
					"Slimelung germs are found in ",
					UI.FormatAsLink("Slime", "SLIMEMOLD"),
					" and ",
					UI.FormatAsLink("Polluted Oxygen", "CONTAMINATEDOXYGEN"),
					". Inhaling these germs can cause Duplicants to cough and struggle to breathe."
				});
			}

			// Token: 0x02002C0A RID: 11274
			public class POLLENGERMS
			{
				// Token: 0x0400B8DC RID: 47324
				public static LocString NAME = UI.FormatAsLink("Floral Scent", "POLLENGERMS");

				// Token: 0x0400B8DD RID: 47325
				public static LocString LEGEND_HOVERTEXT = "Floral Scent allergens present\n";

				// Token: 0x0400B8DE RID: 47326
				public static LocString DESC = "Floral Scent allergens trigger excessive sneezing fits in Duplicants who possess the Allergies trait.";
			}

			// Token: 0x02002C0B RID: 11275
			public class ZOMBIESPORES
			{
				// Token: 0x0400B8DF RID: 47327
				public static LocString NAME = UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES");

				// Token: 0x0400B8E0 RID: 47328
				public static LocString LEGEND_HOVERTEXT = "Zombie Spores present\n";

				// Token: 0x0400B8E1 RID: 47329
				public static LocString DESC = "Zombie Spores are a parasitic brain fungus released by " + UI.FormatAsLink("Sporechids", "EVIL_FLOWER") + ". Duplicants who touch or inhale the spores risk becoming infected and temporarily losing motor control.";
			}

			// Token: 0x02002C0C RID: 11276
			public class RADIATIONPOISONING
			{
				// Token: 0x0400B8E2 RID: 47330
				public static LocString NAME = UI.FormatAsLink("Radioactive Contamination", "RADIATIONPOISONING");

				// Token: 0x0400B8E3 RID: 47331
				public static LocString LEGEND_HOVERTEXT = "Radioactive contamination present\n";

				// Token: 0x0400B8E4 RID: 47332
				public static LocString DESC = string.Concat(new string[]
				{
					"Items tainted with Radioactive Contaminants emit low levels of ",
					UI.FormatAsLink("Radiation", "RADIATION"),
					" that can cause ",
					UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
					". They are unaffected by pressure or temperature, but do degrade over time."
				});
			}

			// Token: 0x02002C0D RID: 11277
			public class FOODSICKNESS
			{
				// Token: 0x0400B8E5 RID: 47333
				public static LocString NAME = UI.FormatAsLink("Food Poisoning", "FOODSICKNESS");

				// Token: 0x0400B8E6 RID: 47334
				public static LocString DESCRIPTION = "This Duplicant's last meal wasn't exactly food safe";

				// Token: 0x0400B8E7 RID: 47335
				public static LocString VOMIT_SYMPTOM = "Vomiting";

				// Token: 0x0400B8E8 RID: 47336
				public static LocString VOMIT_SYMPTOM_TOOLTIP = string.Concat(new string[]
				{
					"Duplicants periodically vomit throughout the day, producing additional ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD,
					" and losing ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD
				});

				// Token: 0x0400B8E9 RID: 47337
				public static LocString DESCRIPTIVE_SYMPTOMS = "Nonlethal. A Duplicant's body \"purges\" from both ends, causing extreme fatigue.";

				// Token: 0x0400B8EA RID: 47338
				public static LocString DISEASE_SOURCE_DESCRIPTOR = "Currently infected with {2}.\n\nThis Duplicant will produce {1} when vomiting.";

				// Token: 0x0400B8EB RID: 47339
				public static LocString DISEASE_SOURCE_DESCRIPTOR_TOOLTIP = "This Duplicant will vomit approximately every <b>{0}</b>\n\nEach time they vomit, they will release <b>{1}</b> and lose " + UI.PRE_KEYWORD + "Calories" + UI.PST_KEYWORD;
			}

			// Token: 0x02002C0E RID: 11278
			public class SLIMESICKNESS
			{
				// Token: 0x0400B8EC RID: 47340
				public static LocString NAME = UI.FormatAsLink("Slimelung", "SLIMESICKNESS");

				// Token: 0x0400B8ED RID: 47341
				public static LocString DESCRIPTION = "This Duplicant's chest congestion is making it difficult to breathe";

				// Token: 0x0400B8EE RID: 47342
				public static LocString COUGH_SYMPTOM = "Coughing";

				// Token: 0x0400B8EF RID: 47343
				public static LocString COUGH_SYMPTOM_TOOLTIP = string.Concat(new string[]
				{
					"Duplicants periodically cough up ",
					ELEMENTS.CONTAMINATEDOXYGEN.NAME,
					", producing additional ",
					UI.PRE_KEYWORD,
					"Germs",
					UI.PST_KEYWORD
				});

				// Token: 0x0400B8F0 RID: 47344
				public static LocString DESCRIPTIVE_SYMPTOMS = "Lethal without medical treatment. Duplicants experience coughing and shortness of breath.";

				// Token: 0x0400B8F1 RID: 47345
				public static LocString DISEASE_SOURCE_DESCRIPTOR = "Currently infected with {2}.\n\nThis Duplicant will produce <b>{1}</b> when coughing.";

				// Token: 0x0400B8F2 RID: 47346
				public static LocString DISEASE_SOURCE_DESCRIPTOR_TOOLTIP = "This Duplicant will cough approximately every <b>{0}</b>\n\nEach time they cough, they will release <b>{1}</b>";
			}

			// Token: 0x02002C0F RID: 11279
			public class ZOMBIESICKNESS
			{
				// Token: 0x0400B8F3 RID: 47347
				public static LocString NAME = UI.FormatAsLink("Zombie Spores", "ZOMBIESICKNESS");

				// Token: 0x0400B8F4 RID: 47348
				public static LocString DESCRIPTIVE_SYMPTOMS = "Duplicants lose much of their motor control and experience extreme discomfort.";

				// Token: 0x0400B8F5 RID: 47349
				public static LocString DESCRIPTION = "Fungal spores have infiltrated the Duplicant's head and are sending unnatural electrical impulses to their brain";

				// Token: 0x0400B8F6 RID: 47350
				public static LocString LEGEND_HOVERTEXT = "Area Causes Zombie Spores\n";
			}

			// Token: 0x02002C10 RID: 11280
			public class ALLERGIES
			{
				// Token: 0x0400B8F7 RID: 47351
				public static LocString NAME = UI.FormatAsLink("Allergic Reaction", "ALLERGIES");

				// Token: 0x0400B8F8 RID: 47352
				public static LocString DESCRIPTIVE_SYMPTOMS = "Allergens cause excessive sneezing fits";

				// Token: 0x0400B8F9 RID: 47353
				public static LocString DESCRIPTION = "Pollen and other irritants are causing this poor Duplicant's immune system to overreact, resulting in needless sneezing and congestion";
			}

			// Token: 0x02002C11 RID: 11281
			public class COLDSICKNESS
			{
				// Token: 0x0400B8FA RID: 47354
				public static LocString NAME = UI.FormatAsLink("Hypothermia", "COLDSICKNESS");

				// Token: 0x0400B8FB RID: 47355
				public static LocString DESCRIPTIVE_SYMPTOMS = "Nonlethal. Duplicants experience extreme body heat loss causing chills and discomfort.";

				// Token: 0x0400B8FC RID: 47356
				public static LocString DESCRIPTION = "This Duplicant's thought processes have been slowed to a crawl from extreme cold exposure";

				// Token: 0x0400B8FD RID: 47357
				public static LocString LEGEND_HOVERTEXT = "Area Causes Hypothermia\n";
			}

			// Token: 0x02002C12 RID: 11282
			public class SUNBURNSICKNESS
			{
				// Token: 0x0400B8FE RID: 47358
				public static LocString NAME = UI.FormatAsLink("Sunburn", "SUNBURNSICKNESS");

				// Token: 0x0400B8FF RID: 47359
				public static LocString DESCRIPTION = "Extreme sun exposure has given this Duplicant a nasty burn.";

				// Token: 0x0400B900 RID: 47360
				public static LocString LEGEND_HOVERTEXT = "Area Causes Sunburn\n";

				// Token: 0x0400B901 RID: 47361
				public static LocString SUNEXPOSURE = "Sun Exposure";

				// Token: 0x0400B902 RID: 47362
				public static LocString DESCRIPTIVE_SYMPTOMS = "Nonlethal. Duplicants experience temporary discomfort due to dermatological damage.";
			}

			// Token: 0x02002C13 RID: 11283
			public class HEATSICKNESS
			{
				// Token: 0x0400B903 RID: 47363
				public static LocString NAME = UI.FormatAsLink("Heat Stroke", "HEATSICKNESS");

				// Token: 0x0400B904 RID: 47364
				public static LocString DESCRIPTIVE_SYMPTOMS = "Nonlethal. Duplicants experience high fever and discomfort.";

				// Token: 0x0400B905 RID: 47365
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"This Duplicant's thought processes have short circuited from extreme ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" exposure"
				});

				// Token: 0x0400B906 RID: 47366
				public static LocString LEGEND_HOVERTEXT = "Area Causes Heat Stroke\n";
			}

			// Token: 0x02002C14 RID: 11284
			public class RADIATIONSICKNESS
			{
				// Token: 0x0400B907 RID: 47367
				public static LocString NAME = UI.FormatAsLink("Radioactive Contaminants", "RADIATIONSICKNESS");

				// Token: 0x0400B908 RID: 47368
				public static LocString DESCRIPTIVE_SYMPTOMS = "Extremely lethal. This Duplicant is not expected to survive.";

				// Token: 0x0400B909 RID: 47369
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"This Duplicant is leaving a trail of ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" behind them."
				});

				// Token: 0x0400B90A RID: 47370
				public static LocString LEGEND_HOVERTEXT = "Area Causes Radiation Sickness\n";

				// Token: 0x0400B90B RID: 47371
				public static LocString DESC = DUPLICANTS.DISEASES.RADIATIONPOISONING.DESC;
			}

			// Token: 0x02002C15 RID: 11285
			public class PUTRIDODOUR
			{
				// Token: 0x0400B90C RID: 47372
				public static LocString NAME = UI.FormatAsLink("Trench Stench", "PUTRIDODOUR");

				// Token: 0x0400B90D RID: 47373
				public static LocString DESCRIPTION = "\nThe pungent odor wafting off this Duplicant is nauseating to their peers";

				// Token: 0x0400B90E RID: 47374
				public static LocString CRINGE_EFFECT = "Smelled a putrid odor";

				// Token: 0x0400B90F RID: 47375
				public static LocString LEGEND_HOVERTEXT = "Trench Stench Germs Present\n";
			}
		}

		// Token: 0x02001DFC RID: 7676
		public class MODIFIERS
		{
			// Token: 0x040089E1 RID: 35297
			public static LocString MODIFIER_FORMAT = UI.PRE_KEYWORD + "{0}" + UI.PST_KEYWORD + ": {1}";

			// Token: 0x040089E2 RID: 35298
			public static LocString TIME_REMAINING = "Time Remaining: {0}";

			// Token: 0x040089E3 RID: 35299
			public static LocString TIME_TOTAL = "\nDuration: {0}";

			// Token: 0x040089E4 RID: 35300
			public static LocString EFFECT_HEADER = UI.PRE_POS_MODIFIER + "Effects:" + UI.PST_POS_MODIFIER;

			// Token: 0x02002C16 RID: 11286
			public class SKILLLEVEL
			{
				// Token: 0x0400B910 RID: 47376
				public static LocString NAME = "Skill Level";
			}

			// Token: 0x02002C17 RID: 11287
			public class ROOMPARK
			{
				// Token: 0x0400B911 RID: 47377
				public static LocString NAME = "Park";

				// Token: 0x0400B912 RID: 47378
				public static LocString TOOLTIP = "This Duplicant recently passed through a Park\n\nWow, nature sure is neat!";
			}

			// Token: 0x02002C18 RID: 11288
			public class ROOMNATURERESERVE
			{
				// Token: 0x0400B913 RID: 47379
				public static LocString NAME = "Nature Reserve";

				// Token: 0x0400B914 RID: 47380
				public static LocString TOOLTIP = "This Duplicant recently passed through a splendid Nature Reserve\n\nWow, nature sure is neat!";
			}

			// Token: 0x02002C19 RID: 11289
			public class ROOMLATRINE
			{
				// Token: 0x0400B915 RID: 47381
				public static LocString NAME = "Latrine";

				// Token: 0x0400B916 RID: 47382
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant used an ",
					BUILDINGS.PREFABS.OUTHOUSE.NAME,
					" in a ",
					UI.PRE_KEYWORD,
					"Latrine",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002C1A RID: 11290
			public class ROOMBATHROOM
			{
				// Token: 0x0400B917 RID: 47383
				public static LocString NAME = "Washroom";

				// Token: 0x0400B918 RID: 47384
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant used a ",
					BUILDINGS.PREFABS.FLUSHTOILET.NAME,
					" in a ",
					UI.PRE_KEYWORD,
					"Washroom",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002C1B RID: 11291
			public class ROOMBARRACKS
			{
				// Token: 0x0400B919 RID: 47385
				public static LocString NAME = "Barracks";

				// Token: 0x0400B91A RID: 47386
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slept in the ",
					UI.PRE_KEYWORD,
					"Barracks",
					UI.PST_KEYWORD,
					" last night and feels refreshed"
				});
			}

			// Token: 0x02002C1C RID: 11292
			public class ROOMBEDROOM
			{
				// Token: 0x0400B91B RID: 47387
				public static LocString NAME = "Luxury Barracks";

				// Token: 0x0400B91C RID: 47388
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slept in a ",
					UI.PRE_KEYWORD,
					"Luxury Barracks",
					UI.PST_KEYWORD,
					" last night and feels extra refreshed"
				});
			}

			// Token: 0x02002C1D RID: 11293
			public class ROOMPRIVATEBEDROOM
			{
				// Token: 0x0400B91D RID: 47389
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400B91E RID: 47390
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant slept in a ",
					UI.PRE_KEYWORD,
					"Private Bedroom",
					UI.PST_KEYWORD,
					" last night and feels super refreshed"
				});
			}

			// Token: 0x02002C1E RID: 11294
			public class BEDHEALTH
			{
				// Token: 0x0400B91F RID: 47391
				public static LocString NAME = "Bed Rest";

				// Token: 0x0400B920 RID: 47392
				public static LocString TOOLTIP = "This Duplicant will incrementally heal over while on " + UI.PRE_KEYWORD + "Bed Rest" + UI.PST_KEYWORD;
			}

			// Token: 0x02002C1F RID: 11295
			public class BEDSTAMINA
			{
				// Token: 0x0400B921 RID: 47393
				public static LocString NAME = "Sleeping in a cot";

				// Token: 0x0400B922 RID: 47394
				public static LocString TOOLTIP = "This Duplicant's sleeping arrangements are adequate";
			}

			// Token: 0x02002C20 RID: 11296
			public class LUXURYBEDSTAMINA
			{
				// Token: 0x0400B923 RID: 47395
				public static LocString NAME = "Sleeping in a comfy bed";

				// Token: 0x0400B924 RID: 47396
				public static LocString TOOLTIP = "This Duplicant loves their snuggly bed";
			}

			// Token: 0x02002C21 RID: 11297
			public class BARRACKSSTAMINA
			{
				// Token: 0x0400B925 RID: 47397
				public static LocString NAME = "Barracks";

				// Token: 0x0400B926 RID: 47398
				public static LocString TOOLTIP = "This Duplicant shares sleeping quarters with others";
			}

			// Token: 0x02002C22 RID: 11298
			public class LADDERBEDSTAMINA
			{
				// Token: 0x0400B927 RID: 47399
				public static LocString NAME = "Sleeping in a ladder bed";

				// Token: 0x0400B928 RID: 47400
				public static LocString TOOLTIP = "This Duplicant's sleeping arrangements are adequate";
			}

			// Token: 0x02002C23 RID: 11299
			public class BEDROOMSTAMINA
			{
				// Token: 0x0400B929 RID: 47401
				public static LocString NAME = "Private Bedroom";

				// Token: 0x0400B92A RID: 47402
				public static LocString TOOLTIP = "This lucky Duplicant has their own private bedroom";
			}

			// Token: 0x02002C24 RID: 11300
			public class ROOMMESSHALL
			{
				// Token: 0x0400B92B RID: 47403
				public static LocString NAME = "Mess Hall";

				// Token: 0x0400B92C RID: 47404
				public static LocString TOOLTIP = "This Duplicant's most recent meal was eaten in a " + UI.PRE_KEYWORD + "Mess Hall" + UI.PST_KEYWORD;
			}

			// Token: 0x02002C25 RID: 11301
			public class ROOMGREATHALL
			{
				// Token: 0x0400B92D RID: 47405
				public static LocString NAME = "Great Hall";

				// Token: 0x0400B92E RID: 47406
				public static LocString TOOLTIP = "This Duplicant's most recent meal was eaten in a fancy " + UI.PRE_KEYWORD + "Great Hall" + UI.PST_KEYWORD;
			}

			// Token: 0x02002C26 RID: 11302
			public class ENTITLEMENT
			{
				// Token: 0x0400B92F RID: 47407
				public static LocString NAME = "Entitlement";

				// Token: 0x0400B930 RID: 47408
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants will demand better ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" and accommodations with each Expertise level they gain"
				});
			}

			// Token: 0x02002C27 RID: 11303
			public class BASEDUPLICANT
			{
				// Token: 0x0400B931 RID: 47409
				public static LocString NAME = "Duplicant";
			}

			// Token: 0x02002C28 RID: 11304
			public class HOMEOSTASIS
			{
				// Token: 0x0400B932 RID: 47410
				public static LocString NAME = "Homeostasis";
			}

			// Token: 0x02002C29 RID: 11305
			public class WARMAIR
			{
				// Token: 0x0400B933 RID: 47411
				public static LocString NAME = "Warm Air";
			}

			// Token: 0x02002C2A RID: 11306
			public class COLDAIR
			{
				// Token: 0x0400B934 RID: 47412
				public static LocString NAME = "Cold Air";
			}

			// Token: 0x02002C2B RID: 11307
			public class CLAUSTROPHOBIC
			{
				// Token: 0x0400B935 RID: 47413
				public static LocString NAME = "Claustrophobic";

				// Token: 0x0400B936 RID: 47414
				public static LocString TOOLTIP = "This Duplicant recently found themselves in an upsettingly cramped space";

				// Token: 0x0400B937 RID: 47415
				public static LocString CAUSE = "This Duplicant got so good at their job that they became claustrophobic";
			}

			// Token: 0x02002C2C RID: 11308
			public class VERTIGO
			{
				// Token: 0x0400B938 RID: 47416
				public static LocString NAME = "Vertigo";

				// Token: 0x0400B939 RID: 47417
				public static LocString TOOLTIP = "This Duplicant had to climb a tall ladder that left them dizzy and unsettled";

				// Token: 0x0400B93A RID: 47418
				public static LocString CAUSE = "This Duplicant got so good at their job they became bad at ladders";
			}

			// Token: 0x02002C2D RID: 11309
			public class UNCOMFORTABLEFEET
			{
				// Token: 0x0400B93B RID: 47419
				public static LocString NAME = "Aching Feet";

				// Token: 0x0400B93C RID: 47420
				public static LocString TOOLTIP = "This Duplicant recently walked across floor without tile, much to their chagrin";

				// Token: 0x0400B93D RID: 47421
				public static LocString CAUSE = "This Duplicant got so good at their job that their feet became sensitive";
			}

			// Token: 0x02002C2E RID: 11310
			public class PEOPLETOOCLOSEWHILESLEEPING
			{
				// Token: 0x0400B93E RID: 47422
				public static LocString NAME = "Personal Bubble Burst";

				// Token: 0x0400B93F RID: 47423
				public static LocString TOOLTIP = "This Duplicant had to sleep too close to others and it was awkward for them";

				// Token: 0x0400B940 RID: 47424
				public static LocString CAUSE = "This Duplicant got so good at their job that they stopped being comfortable sleeping near other people";
			}

			// Token: 0x02002C2F RID: 11311
			public class RESTLESS
			{
				// Token: 0x0400B941 RID: 47425
				public static LocString NAME = "Restless";

				// Token: 0x0400B942 RID: 47426
				public static LocString TOOLTIP = "This Duplicant went a few minutes without working and is now completely awash with guilt";

				// Token: 0x0400B943 RID: 47427
				public static LocString CAUSE = "This Duplicant got so good at their job that they forgot how to be comfortable doing anything else";
			}

			// Token: 0x02002C30 RID: 11312
			public class UNFASHIONABLECLOTHING
			{
				// Token: 0x0400B944 RID: 47428
				public static LocString NAME = "Fashion Crime";

				// Token: 0x0400B945 RID: 47429
				public static LocString TOOLTIP = "This Duplicant had to wear something that was an affront to fashion";

				// Token: 0x0400B946 RID: 47430
				public static LocString CAUSE = "This Duplicant got so good at their job that they became incapable of tolerating unfashionable clothing";
			}

			// Token: 0x02002C31 RID: 11313
			public class BURNINGCALORIES
			{
				// Token: 0x0400B947 RID: 47431
				public static LocString NAME = "Homeostasis";
			}

			// Token: 0x02002C32 RID: 11314
			public class EATINGCALORIES
			{
				// Token: 0x0400B948 RID: 47432
				public static LocString NAME = "Eating";
			}

			// Token: 0x02002C33 RID: 11315
			public class TEMPEXCHANGE
			{
				// Token: 0x0400B949 RID: 47433
				public static LocString NAME = "Environmental Exchange";
			}

			// Token: 0x02002C34 RID: 11316
			public class CLOTHING
			{
				// Token: 0x0400B94A RID: 47434
				public static LocString NAME = "Clothing";
			}

			// Token: 0x02002C35 RID: 11317
			public class CRYFACE
			{
				// Token: 0x0400B94B RID: 47435
				public static LocString NAME = "Cry Face";

				// Token: 0x0400B94C RID: 47436
				public static LocString TOOLTIP = "This Duplicant recently had a crying fit and it shows";

				// Token: 0x0400B94D RID: 47437
				public static LocString CAUSE = string.Concat(new string[]
				{
					"Obtained from the ",
					UI.PRE_KEYWORD,
					"Ugly Crier",
					UI.PST_KEYWORD,
					" stress reaction"
				});
			}

			// Token: 0x02002C36 RID: 11318
			public class DUPLICANTGOTMILK
			{
				// Token: 0x0400B94E RID: 47438
				public static LocString NAME = "Extra Hydrated";

				// Token: 0x0400B94F RID: 47439
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently drank ",
					UI.PRE_KEYWORD,
					"Brackene",
					UI.PST_KEYWORD,
					". It's helping them relax"
				});
			}

			// Token: 0x02002C37 RID: 11319
			public class SOILEDSUIT
			{
				// Token: 0x0400B950 RID: 47440
				public static LocString NAME = "Soiled Suit";

				// Token: 0x0400B951 RID: 47441
				public static LocString TOOLTIP = "This Duplicant's suit needs to be emptied of waste\n\n(Preferably soon)";

				// Token: 0x0400B952 RID: 47442
				public static LocString CAUSE = "Obtained when a Duplicant wears a suit filled with... \"fluids\"";
			}

			// Token: 0x02002C38 RID: 11320
			public class SHOWERED
			{
				// Token: 0x0400B953 RID: 47443
				public static LocString NAME = "Showered";

				// Token: 0x0400B954 RID: 47444
				public static LocString TOOLTIP = "This Duplicant recently had a shower and feels squeaky clean!";
			}

			// Token: 0x02002C39 RID: 11321
			public class SOREBACK
			{
				// Token: 0x0400B955 RID: 47445
				public static LocString NAME = "Sore Back";

				// Token: 0x0400B956 RID: 47446
				public static LocString TOOLTIP = "This Duplicant feels achy from sleeping on the floor last night and would like a bed";

				// Token: 0x0400B957 RID: 47447
				public static LocString CAUSE = "Obtained by sleeping on the ground";
			}

			// Token: 0x02002C3A RID: 11322
			public class GOODEATS
			{
				// Token: 0x0400B958 RID: 47448
				public static LocString NAME = "Soul Food";

				// Token: 0x0400B959 RID: 47449
				public static LocString TOOLTIP = "This Duplicant had a yummy home cooked meal and is totally stuffed";

				// Token: 0x0400B95A RID: 47450
				public static LocString CAUSE = "Obtained by eating a hearty home cooked meal";

				// Token: 0x0400B95B RID: 47451
				public static LocString DESCRIPTION = "Duplicants find this home cooked meal is emotionally comforting";
			}

			// Token: 0x02002C3B RID: 11323
			public class HOTSTUFF
			{
				// Token: 0x0400B95C RID: 47452
				public static LocString NAME = "Hot Stuff";

				// Token: 0x0400B95D RID: 47453
				public static LocString TOOLTIP = "This Duplicant had an extremely spicy meal and is both exhilarated and a little " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;

				// Token: 0x0400B95E RID: 47454
				public static LocString CAUSE = "Obtained by eating a very spicy meal";

				// Token: 0x0400B95F RID: 47455
				public static LocString DESCRIPTION = "Duplicants find this spicy meal quite invigorating";
			}

			// Token: 0x02002C3C RID: 11324
			public class SEAFOODRADIATIONRESISTANCE
			{
				// Token: 0x0400B960 RID: 47456
				public static LocString NAME = "Radiation Resistant: Aquatic Diet";

				// Token: 0x0400B961 RID: 47457
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant ate sea-grown foods, which boost ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" resistance"
				});

				// Token: 0x0400B962 RID: 47458
				public static LocString CAUSE = "Obtained by eating sea-grown foods like fish or lettuce";

				// Token: 0x0400B963 RID: 47459
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Eating this improves ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" resistance"
				});
			}

			// Token: 0x02002C3D RID: 11325
			public class RECENTLYPARTIED
			{
				// Token: 0x0400B964 RID: 47460
				public static LocString NAME = "Partied Hard";

				// Token: 0x0400B965 RID: 47461
				public static LocString TOOLTIP = "This Duplicant recently attended a great party!";
			}

			// Token: 0x02002C3E RID: 11326
			public class NOFUNALLOWED
			{
				// Token: 0x0400B966 RID: 47462
				public static LocString NAME = "Fun Interrupted";

				// Token: 0x0400B967 RID: 47463
				public static LocString TOOLTIP = "This Duplicant is upset a party was rejected";
			}

			// Token: 0x02002C3F RID: 11327
			public class CONTAMINATEDLUNGS
			{
				// Token: 0x0400B968 RID: 47464
				public static LocString NAME = "Yucky Lungs";

				// Token: 0x0400B969 RID: 47465
				public static LocString TOOLTIP = "This Duplicant got a big nasty lungful of " + ELEMENTS.CONTAMINATEDOXYGEN.NAME;
			}

			// Token: 0x02002C40 RID: 11328
			public class MINORIRRITATION
			{
				// Token: 0x0400B96A RID: 47466
				public static LocString NAME = "Minor Eye Irritation";

				// Token: 0x0400B96B RID: 47467
				public static LocString TOOLTIP = "A gas or liquid made this Duplicant's eyes sting a little";

				// Token: 0x0400B96C RID: 47468
				public static LocString CAUSE = "Obtained by exposure to a harsh liquid or gas";
			}

			// Token: 0x02002C41 RID: 11329
			public class MAJORIRRITATION
			{
				// Token: 0x0400B96D RID: 47469
				public static LocString NAME = "Major Eye Irritation";

				// Token: 0x0400B96E RID: 47470
				public static LocString TOOLTIP = "Woah, something really messed up this Duplicant's eyes!\n\nCaused by exposure to a harsh liquid or gas";

				// Token: 0x0400B96F RID: 47471
				public static LocString CAUSE = "Obtained by exposure to a harsh liquid or gas";
			}

			// Token: 0x02002C42 RID: 11330
			public class FRESH_AND_CLEAN
			{
				// Token: 0x0400B970 RID: 47472
				public static LocString NAME = "Refreshingly Clean";

				// Token: 0x0400B971 RID: 47473
				public static LocString TOOLTIP = "This Duplicant took a warm shower and it was great!";

				// Token: 0x0400B972 RID: 47474
				public static LocString CAUSE = "Obtained by taking a comfortably heated shower";
			}

			// Token: 0x02002C43 RID: 11331
			public class BURNED_BY_SCALDING_WATER
			{
				// Token: 0x0400B973 RID: 47475
				public static LocString NAME = "Scalded";

				// Token: 0x0400B974 RID: 47476
				public static LocString TOOLTIP = "Ouch! This Duplicant showered or was doused in water that was way too hot";

				// Token: 0x0400B975 RID: 47477
				public static LocString CAUSE = "Obtained by exposure to hot water";
			}

			// Token: 0x02002C44 RID: 11332
			public class STRESSED_BY_COLD_WATER
			{
				// Token: 0x0400B976 RID: 47478
				public static LocString NAME = "Numb";

				// Token: 0x0400B977 RID: 47479
				public static LocString TOOLTIP = "Brr! This Duplicant was showered or doused in water that was way too cold";

				// Token: 0x0400B978 RID: 47480
				public static LocString CAUSE = "Obtained by exposure to icy water";
			}

			// Token: 0x02002C45 RID: 11333
			public class SMELLEDSTINKY
			{
				// Token: 0x0400B979 RID: 47481
				public static LocString NAME = "Smelled Stinky";

				// Token: 0x0400B97A RID: 47482
				public static LocString TOOLTIP = "This Duplicant got a whiff of a certain somebody";
			}

			// Token: 0x02002C46 RID: 11334
			public class STRESSREDUCTION
			{
				// Token: 0x0400B97B RID: 47483
				public static LocString NAME = "Receiving Massage";

				// Token: 0x0400B97C RID: 47484
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is just melting away"
				});
			}

			// Token: 0x02002C47 RID: 11335
			public class STRESSREDUCTION_CLINIC
			{
				// Token: 0x0400B97D RID: 47485
				public static LocString NAME = "Receiving Clinic Massage";

				// Token: 0x0400B97E RID: 47486
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Clinical facilities are improving the effectiveness of this massage\n\nThis Duplicant's ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" is just melting away"
				});
			}

			// Token: 0x02002C48 RID: 11336
			public class UGLY_CRYING
			{
				// Token: 0x0400B97F RID: 47487
				public static LocString NAME = "Ugly Crying";

				// Token: 0x0400B980 RID: 47488
				public static LocString TOOLTIP = "This Duplicant is having a cathartic ugly cry as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400B981 RID: 47489
				public static LocString NOTIFICATION_NAME = "Ugly Crying";

				// Token: 0x0400B982 RID: 47490
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they broke down crying:";
			}

			// Token: 0x02002C49 RID: 11337
			public class BINGE_EATING
			{
				// Token: 0x0400B983 RID: 47491
				public static LocString NAME = "Insatiable Hunger";

				// Token: 0x0400B984 RID: 47492
				public static LocString TOOLTIP = "This Duplicant is stuffing their face as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400B985 RID: 47493
				public static LocString NOTIFICATION_NAME = "Binge Eating";

				// Token: 0x0400B986 RID: 47494
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they began overeating:";
			}

			// Token: 0x02002C4A RID: 11338
			public class BANSHEE_WAILING
			{
				// Token: 0x0400B987 RID: 47495
				public static LocString NAME = "Deafening Shriek";

				// Token: 0x0400B988 RID: 47496
				public static LocString TOOLTIP = "This Duplicant is wailing at the top of their lungs as a result of " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;

				// Token: 0x0400B989 RID: 47497
				public static LocString NOTIFICATION_NAME = "Banshee Wailing";

				// Token: 0x0400B98A RID: 47498
				public static LocString NOTIFICATION_TOOLTIP = "These Duplicants became so " + UI.FormatAsLink("Stressed", "STRESS") + " they began wailing:";
			}

			// Token: 0x02002C4B RID: 11339
			public class BANSHEE_WAILING_RECOVERY
			{
				// Token: 0x0400B98B RID: 47499
				public static LocString NAME = "Guzzling Air";

				// Token: 0x0400B98C RID: 47500
				public static LocString TOOLTIP = "This Duplicant needs a little extra oxygen to catch their breath";
			}

			// Token: 0x02002C4C RID: 11340
			public class METABOLISM_CALORIE_MODIFIER
			{
				// Token: 0x0400B98D RID: 47501
				public static LocString NAME = "Metabolism";

				// Token: 0x0400B98E RID: 47502
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					UI.PRE_KEYWORD,
					"Metabolism",
					UI.PST_KEYWORD,
					" determines how quickly a critter burns ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002C4D RID: 11341
			public class WORKING
			{
				// Token: 0x0400B98F RID: 47503
				public static LocString NAME = "Working";

				// Token: 0x0400B990 RID: 47504
				public static LocString TOOLTIP = "This Duplicant is working up a sweat";
			}

			// Token: 0x02002C4E RID: 11342
			public class UNCOMFORTABLESLEEP
			{
				// Token: 0x0400B991 RID: 47505
				public static LocString NAME = "Sleeping Uncomfortably";

				// Token: 0x0400B992 RID: 47506
				public static LocString TOOLTIP = "This Duplicant collapsed on the floor from sheer exhaustion";
			}

			// Token: 0x02002C4F RID: 11343
			public class MANAGERIALDUTIES
			{
				// Token: 0x0400B993 RID: 47507
				public static LocString NAME = "Managerial Duties";

				// Token: 0x0400B994 RID: 47508
				public static LocString TOOLTIP = "Being a manager is stressful";
			}

			// Token: 0x02002C50 RID: 11344
			public class MANAGEDCOLONY
			{
				// Token: 0x0400B995 RID: 47509
				public static LocString NAME = "Managed Colony";

				// Token: 0x0400B996 RID: 47510
				public static LocString TOOLTIP = "A Duplicant is in the colony manager job";
			}

			// Token: 0x02002C51 RID: 11345
			public class FLOORSLEEP
			{
				// Token: 0x0400B997 RID: 47511
				public static LocString NAME = "Sleeping On Floor";

				// Token: 0x0400B998 RID: 47512
				public static LocString TOOLTIP = "This Duplicant is uncomfortably recovering " + UI.PRE_KEYWORD + "Stamina" + UI.PST_KEYWORD;
			}

			// Token: 0x02002C52 RID: 11346
			public class PASSEDOUTSLEEP
			{
				// Token: 0x0400B999 RID: 47513
				public static LocString NAME = "Exhausted";

				// Token: 0x0400B99A RID: 47514
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Lack of rest depleted this Duplicant's ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					"\n\nThey passed out from the fatigue"
				});
			}

			// Token: 0x02002C53 RID: 11347
			public class SLEEP
			{
				// Token: 0x0400B99B RID: 47515
				public static LocString NAME = "Sleeping";

				// Token: 0x0400B99C RID: 47516
				public static LocString TOOLTIP = "This Duplicant is recovering " + UI.PRE_KEYWORD + "Stamina" + UI.PST_KEYWORD;
			}

			// Token: 0x02002C54 RID: 11348
			public class SLEEPCLINIC
			{
				// Token: 0x0400B99D RID: 47517
				public static LocString NAME = "Nodding Off";

				// Token: 0x0400B99E RID: 47518
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is losing ",
					UI.PRE_KEYWORD,
					"Stamina",
					UI.PST_KEYWORD,
					" because of their ",
					UI.PRE_KEYWORD,
					"Pajamas",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002C55 RID: 11349
			public class RESTFULSLEEP
			{
				// Token: 0x0400B99F RID: 47519
				public static LocString NAME = "Sleeping Peacefully";

				// Token: 0x0400B9A0 RID: 47520
				public static LocString TOOLTIP = "This Duplicant is getting a good night's rest";
			}

			// Token: 0x02002C56 RID: 11350
			public class SLEEPY
			{
				// Token: 0x0400B9A1 RID: 47521
				public static LocString NAME = "Sleepy";

				// Token: 0x0400B9A2 RID: 47522
				public static LocString TOOLTIP = "This Duplicant is getting tired";
			}

			// Token: 0x02002C57 RID: 11351
			public class HUNGRY
			{
				// Token: 0x0400B9A3 RID: 47523
				public static LocString NAME = "Hungry";

				// Token: 0x0400B9A4 RID: 47524
				public static LocString TOOLTIP = "This Duplicant is ready for lunch";
			}

			// Token: 0x02002C58 RID: 11352
			public class STARVING
			{
				// Token: 0x0400B9A5 RID: 47525
				public static LocString NAME = "Starving";

				// Token: 0x0400B9A6 RID: 47526
				public static LocString TOOLTIP = "This Duplicant needs to eat something, soon";
			}

			// Token: 0x02002C59 RID: 11353
			public class HOT
			{
				// Token: 0x0400B9A7 RID: 47527
				public static LocString NAME = "Hot";

				// Token: 0x0400B9A8 RID: 47528
				public static LocString TOOLTIP = "This Duplicant is uncomfortably warm";
			}

			// Token: 0x02002C5A RID: 11354
			public class COLD
			{
				// Token: 0x0400B9A9 RID: 47529
				public static LocString NAME = "Cold";

				// Token: 0x0400B9AA RID: 47530
				public static LocString TOOLTIP = "This Duplicant is uncomfortably cold";
			}

			// Token: 0x02002C5B RID: 11355
			public class CARPETFEET
			{
				// Token: 0x0400B9AB RID: 47531
				public static LocString NAME = "Tickled Tootsies";

				// Token: 0x0400B9AC RID: 47532
				public static LocString TOOLTIP = "Walking on carpet has made this Duplicant's day a little more luxurious";
			}

			// Token: 0x02002C5C RID: 11356
			public class WETFEET
			{
				// Token: 0x0400B9AD RID: 47533
				public static LocString NAME = "Soggy Feet";

				// Token: 0x0400B9AE RID: 47534
				public static LocString TOOLTIP = "This Duplicant recently stepped in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

				// Token: 0x0400B9AF RID: 47535
				public static LocString CAUSE = "Obtained by walking through liquid";
			}

			// Token: 0x02002C5D RID: 11357
			public class SOAKINGWET
			{
				// Token: 0x0400B9B0 RID: 47536
				public static LocString NAME = "Sopping Wet";

				// Token: 0x0400B9B1 RID: 47537
				public static LocString TOOLTIP = "This Duplicant was recently submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;

				// Token: 0x0400B9B2 RID: 47538
				public static LocString CAUSE = "Obtained from submergence in liquid";
			}

			// Token: 0x02002C5E RID: 11358
			public class POPPEDEARDRUMS
			{
				// Token: 0x0400B9B3 RID: 47539
				public static LocString NAME = "Popped Eardrums";

				// Token: 0x0400B9B4 RID: 47540
				public static LocString TOOLTIP = "This Duplicant was exposed to an over-pressurized area that popped their eardrums";
			}

			// Token: 0x02002C5F RID: 11359
			public class ANEWHOPE
			{
				// Token: 0x0400B9B5 RID: 47541
				public static LocString NAME = "New Hope";

				// Token: 0x0400B9B6 RID: 47542
				public static LocString TOOLTIP = "This Duplicant feels pretty optimistic about their new home";
			}

			// Token: 0x02002C60 RID: 11360
			public class MEGABRAINTANKBONUS
			{
				// Token: 0x0400B9B7 RID: 47543
				public static LocString NAME = "Maximum Aptitude";

				// Token: 0x0400B9B8 RID: 47544
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is smarter and stronger than usual thanks to the ",
					UI.PRE_KEYWORD,
					"Somnium Synthesizer",
					UI.PST_KEYWORD,
					" "
				});
			}

			// Token: 0x02002C61 RID: 11361
			public class PRICKLEFRUITDAMAGE
			{
				// Token: 0x0400B9B9 RID: 47545
				public static LocString NAME = "Ouch!";

				// Token: 0x0400B9BA RID: 47546
				public static LocString TOOLTIP = "This Duplicant ate a raw " + UI.FormatAsLink("Bristle Berry", "PRICKLEFRUIT") + " and it gave their mouth ouchies";
			}

			// Token: 0x02002C62 RID: 11362
			public class NOOXYGEN
			{
				// Token: 0x0400B9BB RID: 47547
				public static LocString NAME = "No Oxygen";

				// Token: 0x0400B9BC RID: 47548
				public static LocString TOOLTIP = "There is no breathable air in this area";
			}

			// Token: 0x02002C63 RID: 11363
			public class LOWOXYGEN
			{
				// Token: 0x0400B9BD RID: 47549
				public static LocString NAME = "Low Oxygen";

				// Token: 0x0400B9BE RID: 47550
				public static LocString TOOLTIP = "The air is thin in this area";
			}

			// Token: 0x02002C64 RID: 11364
			public class MOURNING
			{
				// Token: 0x0400B9BF RID: 47551
				public static LocString NAME = "Mourning";

				// Token: 0x0400B9C0 RID: 47552
				public static LocString TOOLTIP = "This Duplicant is grieving the loss of a friend";
			}

			// Token: 0x02002C65 RID: 11365
			public class NARCOLEPTICSLEEP
			{
				// Token: 0x0400B9C1 RID: 47553
				public static LocString NAME = "Narcoleptic Nap";

				// Token: 0x0400B9C2 RID: 47554
				public static LocString TOOLTIP = "This Duplicant just needs to rest their eyes for a second";
			}

			// Token: 0x02002C66 RID: 11366
			public class BADSLEEP
			{
				// Token: 0x0400B9C3 RID: 47555
				public static LocString NAME = "Unrested: Too Bright";

				// Token: 0x0400B9C4 RID: 47556
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant tossed and turned all night because a ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" was left on where they were trying to sleep"
				});
			}

			// Token: 0x02002C67 RID: 11367
			public class BADSLEEPAFRAIDOFDARK
			{
				// Token: 0x0400B9C5 RID: 47557
				public static LocString NAME = "Unrested: Afraid of Dark";

				// Token: 0x0400B9C6 RID: 47558
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant didn't get much sleep because they were too anxious about the lack of ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" to relax"
				});
			}

			// Token: 0x02002C68 RID: 11368
			public class BADSLEEPMOVEMENT
			{
				// Token: 0x0400B9C7 RID: 47559
				public static LocString NAME = "Unrested: Bed Jostling";

				// Token: 0x0400B9C8 RID: 47560
				public static LocString TOOLTIP = "This Duplicant was woken up when a friend climbed on their ladder bed";
			}

			// Token: 0x02002C69 RID: 11369
			public class TERRIBLESLEEP
			{
				// Token: 0x0400B9C9 RID: 47561
				public static LocString NAME = "Dead Tired: Snoring Friend";

				// Token: 0x0400B9CA RID: 47562
				public static LocString TOOLTIP = "This Duplicant didn't get any shuteye last night because of all the racket from a friend's snoring";
			}

			// Token: 0x02002C6A RID: 11370
			public class PEACEFULSLEEP
			{
				// Token: 0x0400B9CB RID: 47563
				public static LocString NAME = "Well Rested";

				// Token: 0x0400B9CC RID: 47564
				public static LocString TOOLTIP = "This Duplicant had a blissfully quiet sleep last night";
			}

			// Token: 0x02002C6B RID: 11371
			public class CENTEROFATTENTION
			{
				// Token: 0x0400B9CD RID: 47565
				public static LocString NAME = "Center of Attention";

				// Token: 0x0400B9CE RID: 47566
				public static LocString TOOLTIP = "This Duplicant feels like someone's watching over them...";
			}

			// Token: 0x02002C6C RID: 11372
			public class INSPIRED
			{
				// Token: 0x0400B9CF RID: 47567
				public static LocString NAME = "Inspired";

				// Token: 0x0400B9D0 RID: 47568
				public static LocString TOOLTIP = "This Duplicant has had a creative vision!";
			}

			// Token: 0x02002C6D RID: 11373
			public class NEWCREWARRIVAL
			{
				// Token: 0x0400B9D1 RID: 47569
				public static LocString NAME = "New Friend";

				// Token: 0x0400B9D2 RID: 47570
				public static LocString TOOLTIP = "This Duplicant is happy to see a new face in the colony";
			}

			// Token: 0x02002C6E RID: 11374
			public class UNDERWATER
			{
				// Token: 0x0400B9D3 RID: 47571
				public static LocString NAME = "Underwater";

				// Token: 0x0400B9D4 RID: 47572
				public static LocString TOOLTIP = "This Duplicant's movement is slowed";
			}

			// Token: 0x02002C6F RID: 11375
			public class NIGHTMARES
			{
				// Token: 0x0400B9D5 RID: 47573
				public static LocString NAME = "Nightmares";

				// Token: 0x0400B9D6 RID: 47574
				public static LocString TOOLTIP = "This Duplicant was visited by something in the night";
			}

			// Token: 0x02002C70 RID: 11376
			public class WASATTACKED
			{
				// Token: 0x0400B9D7 RID: 47575
				public static LocString NAME = "Recently assailed";

				// Token: 0x0400B9D8 RID: 47576
				public static LocString TOOLTIP = "This Duplicant is stressed out after having been attacked";
			}

			// Token: 0x02002C71 RID: 11377
			public class LIGHTWOUNDS
			{
				// Token: 0x0400B9D9 RID: 47577
				public static LocString NAME = "Light Wounds";

				// Token: 0x0400B9DA RID: 47578
				public static LocString TOOLTIP = "This Duplicant sustained injuries that are a bit uncomfortable";
			}

			// Token: 0x02002C72 RID: 11378
			public class MODERATEWOUNDS
			{
				// Token: 0x0400B9DB RID: 47579
				public static LocString NAME = "Moderate Wounds";

				// Token: 0x0400B9DC RID: 47580
				public static LocString TOOLTIP = "This Duplicant sustained injuries that are affecting their ability to work";
			}

			// Token: 0x02002C73 RID: 11379
			public class SEVEREWOUNDS
			{
				// Token: 0x0400B9DD RID: 47581
				public static LocString NAME = "Severe Wounds";

				// Token: 0x0400B9DE RID: 47582
				public static LocString TOOLTIP = "This Duplicant sustained serious injuries that are impacting their work and well-being";
			}

			// Token: 0x02002C74 RID: 11380
			public class SANDBOXMORALEADJUSTMENT
			{
				// Token: 0x0400B9DF RID: 47583
				public static LocString NAME = "Sandbox Morale Adjustment";

				// Token: 0x0400B9E0 RID: 47584
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had their ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" temporarily adjusted using the Sandbox tools"
				});
			}

			// Token: 0x02002C75 RID: 11381
			public class ROTTEMPERATURE
			{
				// Token: 0x0400B9E1 RID: 47585
				public static LocString UNREFRIGERATED = "Unrefrigerated";

				// Token: 0x0400B9E2 RID: 47586
				public static LocString REFRIGERATED = "Refrigerated";

				// Token: 0x0400B9E3 RID: 47587
				public static LocString FROZEN = "Frozen";
			}

			// Token: 0x02002C76 RID: 11382
			public class ROTATMOSPHERE
			{
				// Token: 0x0400B9E4 RID: 47588
				public static LocString CONTAMINATED = "Contaminated Air";

				// Token: 0x0400B9E5 RID: 47589
				public static LocString NORMAL = "Normal Atmosphere";

				// Token: 0x0400B9E6 RID: 47590
				public static LocString STERILE = "Sterile Atmosphere";
			}

			// Token: 0x02002C77 RID: 11383
			public class BASEROT
			{
				// Token: 0x0400B9E7 RID: 47591
				public static LocString NAME = "Base Decay Rate";
			}

			// Token: 0x02002C78 RID: 11384
			public class FULLBLADDER
			{
				// Token: 0x0400B9E8 RID: 47592
				public static LocString NAME = "Full Bladder";

				// Token: 0x0400B9E9 RID: 47593
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant's ",
					UI.PRE_KEYWORD,
					"Bladder",
					UI.PST_KEYWORD,
					" is full"
				});
			}

			// Token: 0x02002C79 RID: 11385
			public class DIARRHEA
			{
				// Token: 0x0400B9EA RID: 47594
				public static LocString NAME = "Diarrhea";

				// Token: 0x0400B9EB RID: 47595
				public static LocString TOOLTIP = "This Duplicant's gut is giving them some trouble";

				// Token: 0x0400B9EC RID: 47596
				public static LocString CAUSE = "Obtained by eating a disgusting meal";

				// Token: 0x0400B9ED RID: 47597
				public static LocString DESCRIPTION = "Most Duplicants experience stomach upset from this meal";
			}

			// Token: 0x02002C7A RID: 11386
			public class STRESSFULYEMPTYINGBLADDER
			{
				// Token: 0x0400B9EE RID: 47598
				public static LocString NAME = "Making a mess";

				// Token: 0x0400B9EF RID: 47599
				public static LocString TOOLTIP = "This Duplicant had no choice but to empty their " + UI.PRE_KEYWORD + "Bladder" + UI.PST_KEYWORD;
			}

			// Token: 0x02002C7B RID: 11387
			public class REDALERT
			{
				// Token: 0x0400B9F0 RID: 47600
				public static LocString NAME = "Red Alert!";

				// Token: 0x0400B9F1 RID: 47601
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The ",
					UI.PRE_KEYWORD,
					"Red Alert",
					UI.PST_KEYWORD,
					" is stressing this Duplicant out"
				});
			}

			// Token: 0x02002C7C RID: 11388
			public class FUSSY
			{
				// Token: 0x0400B9F2 RID: 47602
				public static LocString NAME = "Fussy";

				// Token: 0x0400B9F3 RID: 47603
				public static LocString TOOLTIP = "This Duplicant is hard to please";
			}

			// Token: 0x02002C7D RID: 11389
			public class WARMINGUP
			{
				// Token: 0x0400B9F4 RID: 47604
				public static LocString NAME = "Warming Up";

				// Token: 0x0400B9F5 RID: 47605
				public static LocString TOOLTIP = "This Duplicant is trying to warm back up";
			}

			// Token: 0x02002C7E RID: 11390
			public class COOLINGDOWN
			{
				// Token: 0x0400B9F6 RID: 47606
				public static LocString NAME = "Cooling Down";

				// Token: 0x0400B9F7 RID: 47607
				public static LocString TOOLTIP = "This Duplicant is trying to cool back down";
			}

			// Token: 0x02002C7F RID: 11391
			public class DARKNESS
			{
				// Token: 0x0400B9F8 RID: 47608
				public static LocString NAME = "Darkness";

				// Token: 0x0400B9F9 RID: 47609
				public static LocString TOOLTIP = "Eep! This Duplicant doesn't like being in the dark!";
			}

			// Token: 0x02002C80 RID: 11392
			public class STEPPEDINCONTAMINATEDWATER
			{
				// Token: 0x0400B9FA RID: 47610
				public static LocString NAME = "Stepped in polluted water";

				// Token: 0x0400B9FB RID: 47611
				public static LocString TOOLTIP = "Gross! This Duplicant stepped in something yucky";
			}

			// Token: 0x02002C81 RID: 11393
			public class WELLFED
			{
				// Token: 0x0400B9FC RID: 47612
				public static LocString NAME = "Well fed";

				// Token: 0x0400B9FD RID: 47613
				public static LocString TOOLTIP = "This Duplicant feels satisfied after having a big meal";
			}

			// Token: 0x02002C82 RID: 11394
			public class STALEFOOD
			{
				// Token: 0x0400B9FE RID: 47614
				public static LocString NAME = "Bad leftovers";

				// Token: 0x0400B9FF RID: 47615
				public static LocString TOOLTIP = "This Duplicant is in a bad mood from having to eat stale " + UI.PRE_KEYWORD + "Food" + UI.PST_KEYWORD;
			}

			// Token: 0x02002C83 RID: 11395
			public class SMELLEDPUTRIDODOUR
			{
				// Token: 0x0400BA00 RID: 47616
				public static LocString NAME = "Smelled a putrid odor";

				// Token: 0x0400BA01 RID: 47617
				public static LocString TOOLTIP = "This Duplicant got a whiff of something unspeakably foul";
			}

			// Token: 0x02002C84 RID: 11396
			public class VOMITING
			{
				// Token: 0x0400BA02 RID: 47618
				public static LocString NAME = "Vomiting";

				// Token: 0x0400BA03 RID: 47619
				public static LocString TOOLTIP = "Better out than in, as they say";
			}

			// Token: 0x02002C85 RID: 11397
			public class BREATHING
			{
				// Token: 0x0400BA04 RID: 47620
				public static LocString NAME = "Breathing";
			}

			// Token: 0x02002C86 RID: 11398
			public class HOLDINGBREATH
			{
				// Token: 0x0400BA05 RID: 47621
				public static LocString NAME = "Holding breath";
			}

			// Token: 0x02002C87 RID: 11399
			public class RECOVERINGBREATH
			{
				// Token: 0x0400BA06 RID: 47622
				public static LocString NAME = "Recovering breath";
			}

			// Token: 0x02002C88 RID: 11400
			public class ROTTING
			{
				// Token: 0x0400BA07 RID: 47623
				public static LocString NAME = "Rotting";
			}

			// Token: 0x02002C89 RID: 11401
			public class DEAD
			{
				// Token: 0x0400BA08 RID: 47624
				public static LocString NAME = "Dead";
			}

			// Token: 0x02002C8A RID: 11402
			public class TOXICENVIRONMENT
			{
				// Token: 0x0400BA09 RID: 47625
				public static LocString NAME = "Toxic environment";
			}

			// Token: 0x02002C8B RID: 11403
			public class RESTING
			{
				// Token: 0x0400BA0A RID: 47626
				public static LocString NAME = "Resting";
			}

			// Token: 0x02002C8C RID: 11404
			public class INTRAVENOUS_NUTRITION
			{
				// Token: 0x0400BA0B RID: 47627
				public static LocString NAME = "Intravenous Feeding";
			}

			// Token: 0x02002C8D RID: 11405
			public class CATHETERIZED
			{
				// Token: 0x0400BA0C RID: 47628
				public static LocString NAME = "Catheterized";

				// Token: 0x0400BA0D RID: 47629
				public static LocString TOOLTIP = "Let's leave it at that";
			}

			// Token: 0x02002C8E RID: 11406
			public class NOISEPEACEFUL
			{
				// Token: 0x0400BA0E RID: 47630
				public static LocString NAME = "Peace and Quiet";

				// Token: 0x0400BA0F RID: 47631
				public static LocString TOOLTIP = "This Duplicant has found a quiet place to concentrate";
			}

			// Token: 0x02002C8F RID: 11407
			public class NOISEMINOR
			{
				// Token: 0x0400BA10 RID: 47632
				public static LocString NAME = "Loud Noises";

				// Token: 0x0400BA11 RID: 47633
				public static LocString TOOLTIP = "This area is a bit too loud for comfort";
			}

			// Token: 0x02002C90 RID: 11408
			public class NOISEMAJOR
			{
				// Token: 0x0400BA12 RID: 47634
				public static LocString NAME = "Cacophony!";

				// Token: 0x0400BA13 RID: 47635
				public static LocString TOOLTIP = "It's very, very loud in here!";
			}

			// Token: 0x02002C91 RID: 11409
			public class MEDICALCOT
			{
				// Token: 0x0400BA14 RID: 47636
				public static LocString NAME = "Triage Cot Rest";

				// Token: 0x0400BA15 RID: 47637
				public static LocString TOOLTIP = "Bedrest is improving this Duplicant's physical recovery time";
			}

			// Token: 0x02002C92 RID: 11410
			public class MEDICALCOTDOCTORED
			{
				// Token: 0x0400BA16 RID: 47638
				public static LocString NAME = "Receiving treatment";

				// Token: 0x0400BA17 RID: 47639
				public static LocString TOOLTIP = "This Duplicant is receiving treatment for their physical injuries";
			}

			// Token: 0x02002C93 RID: 11411
			public class DOCTOREDOFFCOTEFFECT
			{
				// Token: 0x0400BA18 RID: 47640
				public static LocString NAME = "Runaway Patient";

				// Token: 0x0400BA19 RID: 47641
				public static LocString TOOLTIP = "Tsk tsk!\nThis Duplicant cannot receive treatment while out of their medical bed!";
			}

			// Token: 0x02002C94 RID: 11412
			public class POSTDISEASERECOVERY
			{
				// Token: 0x0400BA1A RID: 47642
				public static LocString NAME = "Feeling better";

				// Token: 0x0400BA1B RID: 47643
				public static LocString TOOLTIP = "This Duplicant is up and about, but they still have some lingering effects from their " + UI.PRE_KEYWORD + "Disease" + UI.PST_KEYWORD;

				// Token: 0x0400BA1C RID: 47644
				public static LocString ADDITIONAL_EFFECTS = "This Duplicant has temporary immunity to diseases from having beaten an infection";
			}

			// Token: 0x02002C95 RID: 11413
			public class IMMUNESYSTEMOVERWHELMED
			{
				// Token: 0x0400BA1D RID: 47645
				public static LocString NAME = "Immune System Overwhelmed";

				// Token: 0x0400BA1E RID: 47646
				public static LocString TOOLTIP = "This Duplicant's immune system is slowly being overwhelmed by a high concentration of germs";
			}

			// Token: 0x02002C96 RID: 11414
			public class MEDICINE_GENERICPILL
			{
				// Token: 0x0400BA1F RID: 47647
				public static LocString NAME = "Placebo";

				// Token: 0x0400BA20 RID: 47648
				public static LocString TOOLTIP = ITEMS.PILLS.PLACEBO.DESC;

				// Token: 0x0400BA21 RID: 47649
				public static LocString EFFECT_DESC = string.Concat(new string[]
				{
					"Applies the ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" effect"
				});
			}

			// Token: 0x02002C97 RID: 11415
			public class MEDICINE_BASICBOOSTER
			{
				// Token: 0x0400BA22 RID: 47650
				public static LocString NAME = ITEMS.PILLS.BASICBOOSTER.NAME;

				// Token: 0x0400BA23 RID: 47651
				public static LocString TOOLTIP = ITEMS.PILLS.BASICBOOSTER.DESC;
			}

			// Token: 0x02002C98 RID: 11416
			public class MEDICINE_INTERMEDIATEBOOSTER
			{
				// Token: 0x0400BA24 RID: 47652
				public static LocString NAME = ITEMS.PILLS.INTERMEDIATEBOOSTER.NAME;

				// Token: 0x0400BA25 RID: 47653
				public static LocString TOOLTIP = ITEMS.PILLS.INTERMEDIATEBOOSTER.DESC;
			}

			// Token: 0x02002C99 RID: 11417
			public class MEDICINE_BASICRADPILL
			{
				// Token: 0x0400BA26 RID: 47654
				public static LocString NAME = ITEMS.PILLS.BASICRADPILL.NAME;

				// Token: 0x0400BA27 RID: 47655
				public static LocString TOOLTIP = ITEMS.PILLS.BASICRADPILL.DESC;
			}

			// Token: 0x02002C9A RID: 11418
			public class MEDICINE_INTERMEDIATERADPILL
			{
				// Token: 0x0400BA28 RID: 47656
				public static LocString NAME = ITEMS.PILLS.INTERMEDIATERADPILL.NAME;

				// Token: 0x0400BA29 RID: 47657
				public static LocString TOOLTIP = ITEMS.PILLS.INTERMEDIATERADPILL.DESC;
			}

			// Token: 0x02002C9B RID: 11419
			public class SUNLIGHT_PLEASANT
			{
				// Token: 0x0400BA2A RID: 47658
				public static LocString NAME = "Bright and Cheerful";

				// Token: 0x0400BA2B RID: 47659
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The strong natural ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" is making this Duplicant feel light on their feet"
				});
			}

			// Token: 0x02002C9C RID: 11420
			public class SUNLIGHT_BURNING
			{
				// Token: 0x0400BA2C RID: 47660
				public static LocString NAME = "Intensely Bright";

				// Token: 0x0400BA2D RID: 47661
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The bright ",
					UI.PRE_KEYWORD,
					"Light",
					UI.PST_KEYWORD,
					" is significantly improving this Duplicant's mood, but prolonged exposure may result in burning"
				});
			}

			// Token: 0x02002C9D RID: 11421
			public class TOOKABREAK
			{
				// Token: 0x0400BA2E RID: 47662
				public static LocString NAME = "Downtime";

				// Token: 0x0400BA2F RID: 47663
				public static LocString TOOLTIP = "This Duplicant has a bit of time off from work to attend to personal matters";
			}

			// Token: 0x02002C9E RID: 11422
			public class SOCIALIZED
			{
				// Token: 0x0400BA30 RID: 47664
				public static LocString NAME = "Socialized";

				// Token: 0x0400BA31 RID: 47665
				public static LocString TOOLTIP = "This Duplicant had some free time to hang out with buddies";
			}

			// Token: 0x02002C9F RID: 11423
			public class GOODCONVERSATION
			{
				// Token: 0x0400BA32 RID: 47666
				public static LocString NAME = "Pleasant Chitchat";

				// Token: 0x0400BA33 RID: 47667
				public static LocString TOOLTIP = "This Duplicant recently had a chance to chat with a friend";
			}

			// Token: 0x02002CA0 RID: 11424
			public class WORKENCOURAGED
			{
				// Token: 0x0400BA34 RID: 47668
				public static LocString NAME = "Appreciated";

				// Token: 0x0400BA35 RID: 47669
				public static LocString TOOLTIP = "Someone saw how hard this Duplicant was working and gave them a compliment\n\nThis Duplicant feels great about themselves now!";
			}

			// Token: 0x02002CA1 RID: 11425
			public class ISSTICKERBOMBING
			{
				// Token: 0x0400BA36 RID: 47670
				public static LocString NAME = "Sticker Bombing";

				// Token: 0x0400BA37 RID: 47671
				public static LocString TOOLTIP = "This Duplicant is slapping stickers onto everything!\n\nEveryone's gonna love these";
			}

			// Token: 0x02002CA2 RID: 11426
			public class ISSPARKLESTREAKER
			{
				// Token: 0x0400BA38 RID: 47672
				public static LocString NAME = "Sparkle Streaking";

				// Token: 0x0400BA39 RID: 47673
				public static LocString TOOLTIP = "This Duplicant is currently Sparkle Streaking!\n\nBaa-ling!";
			}

			// Token: 0x02002CA3 RID: 11427
			public class SAWSPARKLESTREAKER
			{
				// Token: 0x0400BA3A RID: 47674
				public static LocString NAME = "Sparkle Flattered";

				// Token: 0x0400BA3B RID: 47675
				public static LocString TOOLTIP = "A Sparkle Streaker's sparkles dazzled this Duplicant\n\nThis Duplicant has a spring in their step now!";
			}

			// Token: 0x02002CA4 RID: 11428
			public class ISJOYSINGER
			{
				// Token: 0x0400BA3C RID: 47676
				public static LocString NAME = "Yodeling";

				// Token: 0x0400BA3D RID: 47677
				public static LocString TOOLTIP = "This Duplicant is currently Yodeling!\n\nHow melodious!";
			}

			// Token: 0x02002CA5 RID: 11429
			public class HEARDJOYSINGER
			{
				// Token: 0x0400BA3E RID: 47678
				public static LocString NAME = "Serenaded";

				// Token: 0x0400BA3F RID: 47679
				public static LocString TOOLTIP = "A Yodeler's singing thrilled this Duplicant\n\nThis Duplicant works at a higher tempo now!";
			}

			// Token: 0x02002CA6 RID: 11430
			public class HASBALLOON
			{
				// Token: 0x0400BA40 RID: 47680
				public static LocString NAME = "Balloon Buddy";

				// Token: 0x0400BA41 RID: 47681
				public static LocString TOOLTIP = "A Balloon Artist gave this Duplicant a balloon!\n\nThis Duplicant feels super crafty now!";
			}

			// Token: 0x02002CA7 RID: 11431
			public class GREETING
			{
				// Token: 0x0400BA42 RID: 47682
				public static LocString NAME = "Saw Friend";

				// Token: 0x0400BA43 RID: 47683
				public static LocString TOOLTIP = "This Duplicant recently saw a friend in the halls and got to say \"hi\"\n\nIt wasn't even awkward!";
			}

			// Token: 0x02002CA8 RID: 11432
			public class HUGGED
			{
				// Token: 0x0400BA44 RID: 47684
				public static LocString NAME = "Hugged";

				// Token: 0x0400BA45 RID: 47685
				public static LocString TOOLTIP = "This Duplicant recently received a hug from a friendly critter\n\nIt was so fluffy!";
			}

			// Token: 0x02002CA9 RID: 11433
			public class ARCADEPLAYING
			{
				// Token: 0x0400BA46 RID: 47686
				public static LocString NAME = "Gaming";

				// Token: 0x0400BA47 RID: 47687
				public static LocString TOOLTIP = "This Duplicant is playing a video game\n\nIt looks like fun!";
			}

			// Token: 0x02002CAA RID: 11434
			public class PLAYEDARCADE
			{
				// Token: 0x0400BA48 RID: 47688
				public static LocString NAME = "Played Video Games";

				// Token: 0x0400BA49 RID: 47689
				public static LocString TOOLTIP = "This Duplicant recently played video games and is feeling like a champ";
			}

			// Token: 0x02002CAB RID: 11435
			public class DANCING
			{
				// Token: 0x0400BA4A RID: 47690
				public static LocString NAME = "Dancing";

				// Token: 0x0400BA4B RID: 47691
				public static LocString TOOLTIP = "This Duplicant is showing off their best moves.";
			}

			// Token: 0x02002CAC RID: 11436
			public class DANCED
			{
				// Token: 0x0400BA4C RID: 47692
				public static LocString NAME = "Recently Danced";

				// Token: 0x0400BA4D RID: 47693
				public static LocString TOOLTIP = "This Duplicant had a chance to cut loose!\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CAD RID: 11437
			public class JUICER
			{
				// Token: 0x0400BA4E RID: 47694
				public static LocString NAME = "Drank Juice";

				// Token: 0x0400BA4F RID: 47695
				public static LocString TOOLTIP = "This Duplicant had delicious fruity drink!\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CAE RID: 11438
			public class ESPRESSO
			{
				// Token: 0x0400BA50 RID: 47696
				public static LocString NAME = "Drank Espresso";

				// Token: 0x0400BA51 RID: 47697
				public static LocString TOOLTIP = "This Duplicant had delicious drink!\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CAF RID: 11439
			public class MECHANICALSURFBOARD
			{
				// Token: 0x0400BA52 RID: 47698
				public static LocString NAME = "Stoked";

				// Token: 0x0400BA53 RID: 47699
				public static LocString TOOLTIP = "This Duplicant had a rad experience on a surfboard.\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CB0 RID: 11440
			public class MECHANICALSURFING
			{
				// Token: 0x0400BA54 RID: 47700
				public static LocString NAME = "Surfin'";

				// Token: 0x0400BA55 RID: 47701
				public static LocString TOOLTIP = "This Duplicant is surfin' some artificial waves!";
			}

			// Token: 0x02002CB1 RID: 11441
			public class SAUNA
			{
				// Token: 0x0400BA56 RID: 47702
				public static LocString NAME = "Steam Powered";

				// Token: 0x0400BA57 RID: 47703
				public static LocString TOOLTIP = "This Duplicant just had a relaxing time in a sauna\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CB2 RID: 11442
			public class SAUNARELAXING
			{
				// Token: 0x0400BA58 RID: 47704
				public static LocString NAME = "Relaxing";

				// Token: 0x0400BA59 RID: 47705
				public static LocString TOOLTIP = "This Duplicant is relaxing in a sauna";
			}

			// Token: 0x02002CB3 RID: 11443
			public class HOTTUB
			{
				// Token: 0x0400BA5A RID: 47706
				public static LocString NAME = "Hot Tubbed";

				// Token: 0x0400BA5B RID: 47707
				public static LocString TOOLTIP = "This Duplicant recently unwound in a Hot Tub\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CB4 RID: 11444
			public class HOTTUBRELAXING
			{
				// Token: 0x0400BA5C RID: 47708
				public static LocString NAME = "Relaxing";

				// Token: 0x0400BA5D RID: 47709
				public static LocString TOOLTIP = "This Duplicant is unwinding in a hot tub\n\nThey sure look relaxed";
			}

			// Token: 0x02002CB5 RID: 11445
			public class SODAFOUNTAIN
			{
				// Token: 0x0400BA5E RID: 47710
				public static LocString NAME = "Soda Filled";

				// Token: 0x0400BA5F RID: 47711
				public static LocString TOOLTIP = "This Duplicant just enjoyed a bubbly beverage\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CB6 RID: 11446
			public class VERTICALWINDTUNNELFLYING
			{
				// Token: 0x0400BA60 RID: 47712
				public static LocString NAME = "Airborne";

				// Token: 0x0400BA61 RID: 47713
				public static LocString TOOLTIP = "This Duplicant is having an exhilarating time in the wind tunnel\n\nWhoosh!";
			}

			// Token: 0x02002CB7 RID: 11447
			public class VERTICALWINDTUNNEL
			{
				// Token: 0x0400BA62 RID: 47714
				public static LocString NAME = "Wind Swept";

				// Token: 0x0400BA63 RID: 47715
				public static LocString TOOLTIP = "This Duplicant recently had an exhilarating wind tunnel experience\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CB8 RID: 11448
			public class BEACHCHAIRRELAXING
			{
				// Token: 0x0400BA64 RID: 47716
				public static LocString NAME = "Totally Chill";

				// Token: 0x0400BA65 RID: 47717
				public static LocString TOOLTIP = "This Duplicant is totally chillin' in a beach chair";
			}

			// Token: 0x02002CB9 RID: 11449
			public class BEACHCHAIRLIT
			{
				// Token: 0x0400BA66 RID: 47718
				public static LocString NAME = "Sun Kissed";

				// Token: 0x0400BA67 RID: 47719
				public static LocString TOOLTIP = "This Duplicant had an amazing experience at the Beach\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CBA RID: 11450
			public class BEACHCHAIRUNLIT
			{
				// Token: 0x0400BA68 RID: 47720
				public static LocString NAME = "Passably Relaxed";

				// Token: 0x0400BA69 RID: 47721
				public static LocString TOOLTIP = "This Duplicant just had a mediocre beach experience\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CBB RID: 11451
			public class TELEPHONECHAT
			{
				// Token: 0x0400BA6A RID: 47722
				public static LocString NAME = "Full of Gossip";

				// Token: 0x0400BA6B RID: 47723
				public static LocString TOOLTIP = "This Duplicant chatted on the phone with at least one other Duplicant\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CBC RID: 11452
			public class TELEPHONEBABBLE
			{
				// Token: 0x0400BA6C RID: 47724
				public static LocString NAME = "Less Anxious";

				// Token: 0x0400BA6D RID: 47725
				public static LocString TOOLTIP = "This Duplicant got some things off their chest by talking to themselves on the phone\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CBD RID: 11453
			public class TELEPHONELONGDISTANCE
			{
				// Token: 0x0400BA6E RID: 47726
				public static LocString NAME = "Sociable";

				// Token: 0x0400BA6F RID: 47727
				public static LocString TOOLTIP = "This Duplicant is feeling sociable after chatting on the phone with someone across space\n\nLeisure activities increase Duplicants' " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;
			}

			// Token: 0x02002CBE RID: 11454
			public class EDIBLEMINUS3
			{
				// Token: 0x0400BA70 RID: 47728
				public static LocString NAME = "Grisly Meal";

				// Token: 0x0400BA71 RID: 47729
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Grisly",
					UI.PST_KEYWORD,
					"\n\nThey hope their next meal will be better"
				});
			}

			// Token: 0x02002CBF RID: 11455
			public class EDIBLEMINUS2
			{
				// Token: 0x0400BA72 RID: 47730
				public static LocString NAME = "Terrible Meal";

				// Token: 0x0400BA73 RID: 47731
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Terrible",
					UI.PST_KEYWORD,
					"\n\nThey hope their next meal will be better"
				});
			}

			// Token: 0x02002CC0 RID: 11456
			public class EDIBLEMINUS1
			{
				// Token: 0x0400BA74 RID: 47732
				public static LocString NAME = "Poor Meal";

				// Token: 0x0400BA75 RID: 47733
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Poor",
					UI.PST_KEYWORD,
					"\n\nThey hope their next meal will be a little better"
				});
			}

			// Token: 0x02002CC1 RID: 11457
			public class EDIBLE0
			{
				// Token: 0x0400BA76 RID: 47734
				public static LocString NAME = "Standard Meal";

				// Token: 0x0400BA77 RID: 47735
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Average",
					UI.PST_KEYWORD,
					"\n\nThey thought it was sort of okay"
				});
			}

			// Token: 0x02002CC2 RID: 11458
			public class EDIBLE1
			{
				// Token: 0x0400BA78 RID: 47736
				public static LocString NAME = "Good Meal";

				// Token: 0x0400BA79 RID: 47737
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Good",
					UI.PST_KEYWORD,
					"\n\nThey thought it was pretty good!"
				});
			}

			// Token: 0x02002CC3 RID: 11459
			public class EDIBLE2
			{
				// Token: 0x0400BA7A RID: 47738
				public static LocString NAME = "Great Meal";

				// Token: 0x0400BA7B RID: 47739
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Great",
					UI.PST_KEYWORD,
					"\n\nThey thought it was pretty good!"
				});
			}

			// Token: 0x02002CC4 RID: 11460
			public class EDIBLE3
			{
				// Token: 0x0400BA7C RID: 47740
				public static LocString NAME = "Superb Meal";

				// Token: 0x0400BA7D RID: 47741
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Superb",
					UI.PST_KEYWORD,
					"\n\nThey thought it was really good!"
				});
			}

			// Token: 0x02002CC5 RID: 11461
			public class EDIBLE4
			{
				// Token: 0x0400BA7E RID: 47742
				public static LocString NAME = "Ambrosial Meal";

				// Token: 0x0400BA7F RID: 47743
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"The food this Duplicant last ate was ",
					UI.PRE_KEYWORD,
					"Ambrosial",
					UI.PST_KEYWORD,
					"\n\nThey thought it was super tasty!"
				});
			}

			// Token: 0x02002CC6 RID: 11462
			public class DECORMINUS1
			{
				// Token: 0x0400BA80 RID: 47744
				public static LocString NAME = "Last Cycle's Decor: Ugly";

				// Token: 0x0400BA81 RID: 47745
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was downright depressing"
				});
			}

			// Token: 0x02002CC7 RID: 11463
			public class DECOR0
			{
				// Token: 0x0400BA82 RID: 47746
				public static LocString NAME = "Last Cycle's Decor: Poor";

				// Token: 0x0400BA83 RID: 47747
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was quite poor"
				});
			}

			// Token: 0x02002CC8 RID: 11464
			public class DECOR1
			{
				// Token: 0x0400BA84 RID: 47748
				public static LocString NAME = "Last Cycle's Decor: Mediocre";

				// Token: 0x0400BA85 RID: 47749
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant had no strong opinions about the colony's ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday"
				});
			}

			// Token: 0x02002CC9 RID: 11465
			public class DECOR2
			{
				// Token: 0x0400BA86 RID: 47750
				public static LocString NAME = "Last Cycle's Decor: Average";

				// Token: 0x0400BA87 RID: 47751
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was pretty alright"
				});
			}

			// Token: 0x02002CCA RID: 11466
			public class DECOR3
			{
				// Token: 0x0400BA88 RID: 47752
				public static LocString NAME = "Last Cycle's Decor: Nice";

				// Token: 0x0400BA89 RID: 47753
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was quite nice!"
				});
			}

			// Token: 0x02002CCB RID: 11467
			public class DECOR4
			{
				// Token: 0x0400BA8A RID: 47754
				public static LocString NAME = "Last Cycle's Decor: Charming";

				// Token: 0x0400BA8B RID: 47755
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was downright charming!"
				});
			}

			// Token: 0x02002CCC RID: 11468
			public class DECOR5
			{
				// Token: 0x0400BA8C RID: 47756
				public static LocString NAME = "Last Cycle's Decor: Gorgeous";

				// Token: 0x0400BA8D RID: 47757
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant thought the overall ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" yesterday was fantastic\n\nThey love what I've done with the place!"
				});
			}

			// Token: 0x02002CCD RID: 11469
			public class BREAK1
			{
				// Token: 0x0400BA8E RID: 47758
				public static LocString NAME = "One Shift Break";

				// Token: 0x0400BA8F RID: 47759
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had one ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shift in the last cycle"
				});
			}

			// Token: 0x02002CCE RID: 11470
			public class BREAK2
			{
				// Token: 0x0400BA90 RID: 47760
				public static LocString NAME = "Two Shift Break";

				// Token: 0x0400BA91 RID: 47761
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had two ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts in the last cycle"
				});
			}

			// Token: 0x02002CCF RID: 11471
			public class BREAK3
			{
				// Token: 0x0400BA92 RID: 47762
				public static LocString NAME = "Three Shift Break";

				// Token: 0x0400BA93 RID: 47763
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had three ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts in the last cycle"
				});
			}

			// Token: 0x02002CD0 RID: 11472
			public class BREAK4
			{
				// Token: 0x0400BA94 RID: 47764
				public static LocString NAME = "Four Shift Break";

				// Token: 0x0400BA95 RID: 47765
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had four ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts in the last cycle"
				});
			}

			// Token: 0x02002CD1 RID: 11473
			public class BREAK5
			{
				// Token: 0x0400BA96 RID: 47766
				public static LocString NAME = "Five Shift Break";

				// Token: 0x0400BA97 RID: 47767
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant has had five ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" shifts in the last cycle"
				});
			}

			// Token: 0x02002CD2 RID: 11474
			public class POWERTINKER
			{
				// Token: 0x0400BA98 RID: 47768
				public static LocString NAME = "Engie's Tune-Up";

				// Token: 0x0400BA99 RID: 47769
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A skilled Duplicant has improved this generator's ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" output efficiency\n\nApplying this effect consumed one ",
					UI.PRE_KEYWORD,
					ITEMS.INDUSTRIAL_PRODUCTS.POWER_STATION_TOOLS.NAME,
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002CD3 RID: 11475
			public class FARMTINKER
			{
				// Token: 0x0400BA9A RID: 47770
				public static LocString NAME = "Farmer's Touch";

				// Token: 0x0400BA9B RID: 47771
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A skilled Duplicant has encouraged this ",
					UI.PRE_KEYWORD,
					"Plant",
					UI.PST_KEYWORD,
					" to grow a little bit faster\n\nApplying this effect consumed one dose of ",
					UI.PRE_KEYWORD,
					ITEMS.INDUSTRIAL_PRODUCTS.FARM_STATION_TOOLS.NAME,
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002CD4 RID: 11476
			public class MACHINETINKER
			{
				// Token: 0x0400BA9C RID: 47772
				public static LocString NAME = "Engie's Jerry Rig";

				// Token: 0x0400BA9D RID: 47773
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A skilled Duplicant has jerry rigged this ",
					UI.PRE_KEYWORD,
					"Generator",
					UI.PST_KEYWORD,
					" to temporarily run faster"
				});
			}

			// Token: 0x02002CD5 RID: 11477
			public class SPACETOURIST
			{
				// Token: 0x0400BA9E RID: 47774
				public static LocString NAME = "Visited Space";

				// Token: 0x0400BA9F RID: 47775
				public static LocString TOOLTIP = "This Duplicant went on a trip to space and saw the wonders of the universe";
			}

			// Token: 0x02002CD6 RID: 11478
			public class SUDDENMORALEHELPER
			{
				// Token: 0x0400BAA0 RID: 47776
				public static LocString NAME = "Morale Upgrade Helper";

				// Token: 0x0400BAA1 RID: 47777
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant will receive a temporary ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" bonus to buffer the new ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" system introduction"
				});
			}

			// Token: 0x02002CD7 RID: 11479
			public class EXPOSEDTOFOODGERMS
			{
				// Token: 0x0400BAA2 RID: 47778
				public static LocString NAME = "Food Poisoning Exposure";

				// Token: 0x0400BAA3 RID: 47779
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was exposed to ",
					DUPLICANTS.DISEASES.FOODPOISONING.NAME,
					" Germs and is at risk of developing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002CD8 RID: 11480
			public class EXPOSEDTOSLIMEGERMS
			{
				// Token: 0x0400BAA4 RID: 47780
				public static LocString NAME = "Slimelung Exposure";

				// Token: 0x0400BAA5 RID: 47781
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was exposed to ",
					DUPLICANTS.DISEASES.SLIMELUNG.NAME,
					" and is at risk of developing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002CD9 RID: 11481
			public class EXPOSEDTOZOMBIESPORES
			{
				// Token: 0x0400BAA6 RID: 47782
				public static LocString NAME = "Zombie Spores Exposure";

				// Token: 0x0400BAA7 RID: 47783
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant was exposed to ",
					DUPLICANTS.DISEASES.ZOMBIESPORES.NAME,
					" and is at risk of developing the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002CDA RID: 11482
			public class FEELINGSICKFOODGERMS
			{
				// Token: 0x0400BAA8 RID: 47784
				public static LocString NAME = "Contracted: Food Poisoning";

				// Token: 0x0400BAA9 RID: 47785
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant contracted ",
					DUPLICANTS.DISEASES.FOODSICKNESS.NAME,
					" after a recent ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					" exposure and will begin exhibiting symptoms shortly"
				});
			}

			// Token: 0x02002CDB RID: 11483
			public class FEELINGSICKSLIMEGERMS
			{
				// Token: 0x0400BAAA RID: 47786
				public static LocString NAME = "Contracted: Slimelung";

				// Token: 0x0400BAAB RID: 47787
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant contracted ",
					DUPLICANTS.DISEASES.SLIMESICKNESS.NAME,
					" after a recent ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					" exposure and will begin exhibiting symptoms shortly"
				});
			}

			// Token: 0x02002CDC RID: 11484
			public class FEELINGSICKZOMBIESPORES
			{
				// Token: 0x0400BAAC RID: 47788
				public static LocString NAME = "Contracted: Zombie Spores";

				// Token: 0x0400BAAD RID: 47789
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant contracted ",
					DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME,
					" after a recent ",
					UI.PRE_KEYWORD,
					"Germ",
					UI.PST_KEYWORD,
					" exposure and will begin exhibiting symptoms shortly"
				});
			}

			// Token: 0x02002CDD RID: 11485
			public class SMELLEDFLOWERS
			{
				// Token: 0x0400BAAE RID: 47790
				public static LocString NAME = "Smelled Flowers";

				// Token: 0x0400BAAF RID: 47791
				public static LocString TOOLTIP = "A pleasant " + DUPLICANTS.DISEASES.POLLENGERMS.NAME + " wafted over this Duplicant and brightened their day";
			}

			// Token: 0x02002CDE RID: 11486
			public class HISTAMINESUPPRESSION
			{
				// Token: 0x0400BAB0 RID: 47792
				public static LocString NAME = "Antihistamines";

				// Token: 0x0400BAB1 RID: 47793
				public static LocString TOOLTIP = "This Duplicant's allergic reactions have been suppressed by medication";
			}

			// Token: 0x02002CDF RID: 11487
			public class FOODSICKNESSRECOVERY
			{
				// Token: 0x0400BAB2 RID: 47794
				public static LocString NAME = "Food Poisoning Antibodies";

				// Token: 0x0400BAB3 RID: 47795
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently recovered from ",
					DUPLICANTS.DISEASES.FOODSICKNESS.NAME,
					" and is temporarily immune to the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002CE0 RID: 11488
			public class SLIMESICKNESSRECOVERY
			{
				// Token: 0x0400BAB4 RID: 47796
				public static LocString NAME = "Slimelung Antibodies";

				// Token: 0x0400BAB5 RID: 47797
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently recovered from ",
					DUPLICANTS.DISEASES.SLIMESICKNESS.NAME,
					" and is temporarily immune to the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002CE1 RID: 11489
			public class ZOMBIESICKNESSRECOVERY
			{
				// Token: 0x0400BAB6 RID: 47798
				public static LocString NAME = "Zombie Spores Antibodies";

				// Token: 0x0400BAB7 RID: 47799
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant recently recovered from ",
					DUPLICANTS.DISEASES.ZOMBIESICKNESS.NAME,
					" and is temporarily immune to the ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002CE2 RID: 11490
			public class MESSTABLESALT
			{
				// Token: 0x0400BAB8 RID: 47800
				public static LocString NAME = "Salted Food";

				// Token: 0x0400BAB9 RID: 47801
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant had the luxury of using ",
					UI.PRE_KEYWORD,
					ITEMS.INDUSTRIAL_PRODUCTS.TABLE_SALT.NAME,
					UI.PST_KEYWORD,
					" with their last meal at a ",
					BUILDINGS.PREFABS.DININGTABLE.NAME
				});
			}

			// Token: 0x02002CE3 RID: 11491
			public class RADIATIONEXPOSUREMINOR
			{
				// Token: 0x0400BABA RID: 47802
				public static LocString NAME = "Minor Radiation Sickness";

				// Token: 0x0400BABB RID: 47803
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"A bit of ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has made this Duplicant feel sluggish"
				});
			}

			// Token: 0x02002CE4 RID: 11492
			public class RADIATIONEXPOSUREMAJOR
			{
				// Token: 0x0400BABC RID: 47804
				public static LocString NAME = "Major Radiation Sickness";

				// Token: 0x0400BABD RID: 47805
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Significant ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has left this Duplicant totally exhausted"
				});
			}

			// Token: 0x02002CE5 RID: 11493
			public class RADIATIONEXPOSUREEXTREME
			{
				// Token: 0x0400BABE RID: 47806
				public static LocString NAME = "Extreme Radiation Sickness";

				// Token: 0x0400BABF RID: 47807
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Dangerously high ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure is making this Duplicant wish they'd never been printed"
				});
			}

			// Token: 0x02002CE6 RID: 11494
			public class RADIATIONEXPOSUREDEADLY
			{
				// Token: 0x0400BAC0 RID: 47808
				public static LocString NAME = "Deadly Radiation Sickness";

				// Token: 0x0400BAC1 RID: 47809
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Extreme ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" exposure has incapacitated this Duplicant"
				});
			}

			// Token: 0x02002CE7 RID: 11495
			public class CHARGING
			{
				// Token: 0x0400BAC2 RID: 47810
				public static LocString NAME = "Charging";

				// Token: 0x0400BAC3 RID: 47811
				public static LocString TOOLTIP = "This lil bot is charging its internal battery";
			}

			// Token: 0x02002CE8 RID: 11496
			public class BOTSWEEPING
			{
				// Token: 0x0400BAC4 RID: 47812
				public static LocString NAME = "Sweeping";

				// Token: 0x0400BAC5 RID: 47813
				public static LocString TOOLTIP = "This lil bot is picking up debris from the floor";
			}

			// Token: 0x02002CE9 RID: 11497
			public class BOTMOPPING
			{
				// Token: 0x0400BAC6 RID: 47814
				public static LocString NAME = "Mopping";

				// Token: 0x0400BAC7 RID: 47815
				public static LocString TOOLTIP = "This lil bot is clearing liquids from the ground";
			}

			// Token: 0x02002CEA RID: 11498
			public class SCOUTBOTCHARGING
			{
				// Token: 0x0400BAC8 RID: 47816
				public static LocString NAME = "Charging";

				// Token: 0x0400BAC9 RID: 47817
				public static LocString TOOLTIP = ROBOTS.MODELS.SCOUT.NAME + " is happily charging inside " + BUILDINGS.PREFABS.SCOUTMODULE.NAME;
			}

			// Token: 0x02002CEB RID: 11499
			public class CRYOFRIEND
			{
				// Token: 0x0400BACA RID: 47818
				public static LocString NAME = "Motivated By Friend";

				// Token: 0x0400BACB RID: 47819
				public static LocString TOOLTIP = "This Duplicant feels motivated after meeting a long lost friend";
			}

			// Token: 0x02002CEC RID: 11500
			public class BONUSDREAM1
			{
				// Token: 0x0400BACC RID: 47820
				public static LocString NAME = "Good Dream";

				// Token: 0x0400BACD RID: 47821
				public static LocString TOOLTIP = "This Duplicant had a good dream and is feeling psyched!";
			}

			// Token: 0x02002CED RID: 11501
			public class BONUSDREAM2
			{
				// Token: 0x0400BACE RID: 47822
				public static LocString NAME = "Really Good Dream";

				// Token: 0x0400BACF RID: 47823
				public static LocString TOOLTIP = "This Duplicant had a really good dream and is full of possibilities!";
			}

			// Token: 0x02002CEE RID: 11502
			public class BONUSDREAM3
			{
				// Token: 0x0400BAD0 RID: 47824
				public static LocString NAME = "Great Dream";

				// Token: 0x0400BAD1 RID: 47825
				public static LocString TOOLTIP = "This Duplicant had a great dream last night and periodically remembers another great moment they previously forgot";
			}

			// Token: 0x02002CEF RID: 11503
			public class BONUSDREAM4
			{
				// Token: 0x0400BAD2 RID: 47826
				public static LocString NAME = "Dream Inspired";

				// Token: 0x0400BAD3 RID: 47827
				public static LocString TOOLTIP = "This Duplicant is inspired from all the unforgettable dreams they had";
			}

			// Token: 0x02002CF0 RID: 11504
			public class BONUSRESEARCH
			{
				// Token: 0x0400BAD4 RID: 47828
				public static LocString NAME = "Inspired Learner";

				// Token: 0x0400BAD5 RID: 47829
				public static LocString TOOLTIP = "This Duplicant is looking forward to some learning";
			}

			// Token: 0x02002CF1 RID: 11505
			public class BONUSTOILET1
			{
				// Token: 0x0400BAD6 RID: 47830
				public static LocString NAME = "Small Comforts";

				// Token: 0x0400BAD7 RID: 47831
				public static LocString TOOLTIP = "This Duplicant visited the {building} and appreciated the small comforts";
			}

			// Token: 0x02002CF2 RID: 11506
			public class BONUSTOILET2
			{
				// Token: 0x0400BAD8 RID: 47832
				public static LocString NAME = "Greater Comforts";

				// Token: 0x0400BAD9 RID: 47833
				public static LocString TOOLTIP = "This Duplicant used a " + BUILDINGS.PREFABS.OUTHOUSE.NAME + "and liked how comfortable it felt";
			}

			// Token: 0x02002CF3 RID: 11507
			public class BONUSTOILET3
			{
				// Token: 0x0400BADA RID: 47834
				public static LocString NAME = "Small Luxury";

				// Token: 0x0400BADB RID: 47835
				public static LocString TOOLTIP = "This Duplicant visited a " + ROOMS.TYPES.LATRINE.NAME + " and feels they could get used to this luxury";
			}

			// Token: 0x02002CF4 RID: 11508
			public class BONUSTOILET4
			{
				// Token: 0x0400BADC RID: 47836
				public static LocString NAME = "Luxurious";

				// Token: 0x0400BADD RID: 47837
				public static LocString TOOLTIP = "This Duplicant feels endless luxury from the " + ROOMS.TYPES.PRIVATE_BATHROOM.NAME;
			}

			// Token: 0x02002CF5 RID: 11509
			public class BONUSDIGGING1
			{
				// Token: 0x0400BADE RID: 47838
				public static LocString NAME = "Hot Diggity!";

				// Token: 0x0400BADF RID: 47839
				public static LocString TOOLTIP = "This Duplicant did a lot of excavating and is really digging digging";
			}

			// Token: 0x02002CF6 RID: 11510
			public class BONUSSTORAGE
			{
				// Token: 0x0400BAE0 RID: 47840
				public static LocString NAME = "Something in Store";

				// Token: 0x0400BAE1 RID: 47841
				public static LocString TOOLTIP = "This Duplicant stored something in a " + BUILDINGS.PREFABS.STORAGELOCKER.NAME + " and is feeling organized";
			}

			// Token: 0x02002CF7 RID: 11511
			public class BONUSBUILDER
			{
				// Token: 0x0400BAE2 RID: 47842
				public static LocString NAME = "Accomplished Builder";

				// Token: 0x0400BAE3 RID: 47843
				public static LocString TOOLTIP = "This Duplicant has built many buildings and has a sense of accomplishment!";
			}

			// Token: 0x02002CF8 RID: 11512
			public class BONUSOXYGEN
			{
				// Token: 0x0400BAE4 RID: 47844
				public static LocString NAME = "Fresh Air";

				// Token: 0x0400BAE5 RID: 47845
				public static LocString TOOLTIP = "This Duplicant breathed in some fresh air and is feeling refreshed";
			}

			// Token: 0x02002CF9 RID: 11513
			public class BONUSGENERATOR
			{
				// Token: 0x0400BAE6 RID: 47846
				public static LocString NAME = "Exercised";

				// Token: 0x0400BAE7 RID: 47847
				public static LocString TOOLTIP = "This Duplicant ran in a Generator and has benefited from the exercise";
			}

			// Token: 0x02002CFA RID: 11514
			public class BONUSDOOR
			{
				// Token: 0x0400BAE8 RID: 47848
				public static LocString NAME = "Open and Shut";

				// Token: 0x0400BAE9 RID: 47849
				public static LocString TOOLTIP = "This Duplicant closed a door and appreciates the privacy";
			}

			// Token: 0x02002CFB RID: 11515
			public class BONUSHITTHEBOOKS
			{
				// Token: 0x0400BAEA RID: 47850
				public static LocString NAME = "Hit the Books";

				// Token: 0x0400BAEB RID: 47851
				public static LocString TOOLTIP = "This Duplicant did some research and is feeling smarter";
			}

			// Token: 0x02002CFC RID: 11516
			public class BONUSLITWORKSPACE
			{
				// Token: 0x0400BAEC RID: 47852
				public static LocString NAME = "Lit";

				// Token: 0x0400BAED RID: 47853
				public static LocString TOOLTIP = "This Duplicant was in a well-lit environment and is feeling lit";
			}

			// Token: 0x02002CFD RID: 11517
			public class BONUSTALKER
			{
				// Token: 0x0400BAEE RID: 47854
				public static LocString NAME = "Talker";

				// Token: 0x0400BAEF RID: 47855
				public static LocString TOOLTIP = "This Duplicant engaged in small talk with a coworker and is feeling connected";
			}

			// Token: 0x02002CFE RID: 11518
			public class THRIVER
			{
				// Token: 0x0400BAF0 RID: 47856
				public static LocString NAME = "Clutchy";

				// Token: 0x0400BAF1 RID: 47857
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" and has kicked into hyperdrive"
				});
			}

			// Token: 0x02002CFF RID: 11519
			public class LONER
			{
				// Token: 0x0400BAF2 RID: 47858
				public static LocString NAME = "Alone";

				// Token: 0x0400BAF3 RID: 47859
				public static LocString TOOLTIP = "This Duplicant is more feeling focused now that they're alone";
			}

			// Token: 0x02002D00 RID: 11520
			public class STARRYEYED
			{
				// Token: 0x0400BAF4 RID: 47860
				public static LocString NAME = "Starry Eyed";

				// Token: 0x0400BAF5 RID: 47861
				public static LocString TOOLTIP = "This Duplicant loves being in space!";
			}

			// Token: 0x02002D01 RID: 11521
			public class WAILEDAT
			{
				// Token: 0x0400BAF6 RID: 47862
				public static LocString NAME = "Disturbed by Wailing";

				// Token: 0x0400BAF7 RID: 47863
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"This Duplicant is feeling ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" by someone's Banshee Wail"
				});
			}
		}

		// Token: 0x02001DFD RID: 7677
		public class CONGENITALTRAITS
		{
			// Token: 0x02002D02 RID: 11522
			public class NONE
			{
				// Token: 0x0400BAF8 RID: 47864
				public static LocString NAME = "None";

				// Token: 0x0400BAF9 RID: 47865
				public static LocString DESC = "This Duplicant seems pretty average overall";
			}

			// Token: 0x02002D03 RID: 11523
			public class JOSHUA
			{
				// Token: 0x0400BAFA RID: 47866
				public static LocString NAME = "Cheery Disposition";

				// Token: 0x0400BAFB RID: 47867
				public static LocString DESC = "This Duplicant brightens others' days wherever he goes";
			}

			// Token: 0x02002D04 RID: 11524
			public class ELLIE
			{
				// Token: 0x0400BAFC RID: 47868
				public static LocString NAME = "Fastidious";

				// Token: 0x0400BAFD RID: 47869
				public static LocString DESC = "This Duplicant needs things done in a very particular way";
			}

			// Token: 0x02002D05 RID: 11525
			public class LIAM
			{
				// Token: 0x0400BAFE RID: 47870
				public static LocString NAME = "Germaphobe";

				// Token: 0x0400BAFF RID: 47871
				public static LocString DESC = "This Duplicant has an all-consuming fear of bacteria";
			}

			// Token: 0x02002D06 RID: 11526
			public class BANHI
			{
				// Token: 0x0400BB00 RID: 47872
				public static LocString NAME = "";

				// Token: 0x0400BB01 RID: 47873
				public static LocString DESC = "";
			}

			// Token: 0x02002D07 RID: 11527
			public class STINKY
			{
				// Token: 0x0400BB02 RID: 47874
				public static LocString NAME = "Stinkiness";

				// Token: 0x0400BB03 RID: 47875
				public static LocString DESC = "This Duplicant is genetically cursed by a pungent bodily odor";
			}
		}

		// Token: 0x02001DFE RID: 7678
		public class TRAITS
		{
			// Token: 0x040089E5 RID: 35301
			public static LocString TRAIT_DESCRIPTION_LIST_ENTRY = "\n• ";

			// Token: 0x040089E6 RID: 35302
			public static LocString ATTRIBUTE_MODIFIERS = "{0}: {1}";

			// Token: 0x040089E7 RID: 35303
			public static LocString CANNOT_DO_TASK = "Cannot do <b>{0} Errands</b>";

			// Token: 0x040089E8 RID: 35304
			public static LocString CANNOT_DO_TASK_TOOLTIP = "{0}: {1}";

			// Token: 0x040089E9 RID: 35305
			public static LocString REFUSES_TO_DO_TASK = "Cannot do <b>{0} Errands</b>";

			// Token: 0x040089EA RID: 35306
			public static LocString IGNORED_EFFECTS = "Immune to <b>{0}</b>";

			// Token: 0x040089EB RID: 35307
			public static LocString IGNORED_EFFECTS_TOOLTIP = "{0}: {1}";

			// Token: 0x040089EC RID: 35308
			public static LocString GRANTED_SKILL_SHARED_NAME = "Skilled: ";

			// Token: 0x040089ED RID: 35309
			public static LocString GRANTED_SKILL_SHARED_DESC = string.Concat(new string[]
			{
				"This Duplicant begins with a pre-learned ",
				UI.FormatAsKeyWord("Skill"),
				", but does not have increased ",
				UI.FormatAsKeyWord(DUPLICANTS.NEEDS.QUALITYOFLIFE.NAME),
				".\n\n{0}\n{1}"
			});

			// Token: 0x040089EE RID: 35310
			public static LocString GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP = "This Duplicant receives a free " + UI.FormatAsKeyWord("Skill") + " without the drawback of increased " + UI.FormatAsKeyWord(DUPLICANTS.NEEDS.QUALITYOFLIFE.NAME);

			// Token: 0x02002D08 RID: 11528
			public class CHATTY
			{
				// Token: 0x0400BB04 RID: 47876
				public static LocString NAME = "Charismatic";

				// Token: 0x0400BB05 RID: 47877
				public static LocString DESC = string.Concat(new string[]
				{
					"This Duplicant's so charming, chatting with them is sometimes enough to trigger an ",
					UI.PRE_KEYWORD,
					"Overjoyed",
					UI.PST_KEYWORD,
					" response"
				});
			}

			// Token: 0x02002D09 RID: 11529
			public class NEEDS
			{
				// Token: 0x020031D9 RID: 12761
				public class CLAUSTROPHOBIC
				{
					// Token: 0x0400C72B RID: 50987
					public static LocString NAME = "Claustrophobic";

					// Token: 0x0400C72C RID: 50988
					public static LocString DESC = "This Duplicant feels suffocated in spaces fewer than four tiles high or three tiles wide";
				}

				// Token: 0x020031DA RID: 12762
				public class FASHIONABLE
				{
					// Token: 0x0400C72D RID: 50989
					public static LocString NAME = "Fashionista";

					// Token: 0x0400C72E RID: 50990
					public static LocString DESC = "This Duplicant dies a bit inside when forced to wear unstylish clothing";
				}

				// Token: 0x020031DB RID: 12763
				public class CLIMACOPHOBIC
				{
					// Token: 0x0400C72F RID: 50991
					public static LocString NAME = "Vertigo Prone";

					// Token: 0x0400C730 RID: 50992
					public static LocString DESC = "Climbing ladders more than four tiles tall makes this Duplicant's stomach do flips";
				}

				// Token: 0x020031DC RID: 12764
				public class SOLITARYSLEEPER
				{
					// Token: 0x0400C731 RID: 50993
					public static LocString NAME = "Solitary Sleeper";

					// Token: 0x0400C732 RID: 50994
					public static LocString DESC = "This Duplicant prefers to sleep alone";
				}

				// Token: 0x020031DD RID: 12765
				public class PREFERSWARMER
				{
					// Token: 0x0400C733 RID: 50995
					public static LocString NAME = "Skinny";

					// Token: 0x0400C734 RID: 50996
					public static LocString DESC = string.Concat(new string[]
					{
						"This Duplicant doesn't have much ",
						UI.PRE_KEYWORD,
						"Insulation",
						UI.PST_KEYWORD,
						", so they are more ",
						UI.PRE_KEYWORD,
						"Temperature",
						UI.PST_KEYWORD,
						" sensitive than others"
					});
				}

				// Token: 0x020031DE RID: 12766
				public class PREFERSCOOLER
				{
					// Token: 0x0400C735 RID: 50997
					public static LocString NAME = "Pudgy";

					// Token: 0x0400C736 RID: 50998
					public static LocString DESC = string.Concat(new string[]
					{
						"This Duplicant has some extra ",
						UI.PRE_KEYWORD,
						"Insulation",
						UI.PST_KEYWORD,
						", so the room ",
						UI.PRE_KEYWORD,
						"Temperature",
						UI.PST_KEYWORD,
						" affects them a little less"
					});
				}

				// Token: 0x020031DF RID: 12767
				public class SENSITIVEFEET
				{
					// Token: 0x0400C737 RID: 50999
					public static LocString NAME = "Delicate Feetsies";

					// Token: 0x0400C738 RID: 51000
					public static LocString DESC = "This Duplicant is a sensitive sole and would rather walk on tile than raw bedrock";
				}

				// Token: 0x020031E0 RID: 12768
				public class WORKAHOLIC
				{
					// Token: 0x0400C739 RID: 51001
					public static LocString NAME = "Workaholic";

					// Token: 0x0400C73A RID: 51002
					public static LocString DESC = "This Duplicant gets antsy when left idle";
				}
			}

			// Token: 0x02002D0A RID: 11530
			public class ANCIENTKNOWLEDGE
			{
				// Token: 0x0400BB06 RID: 47878
				public static LocString NAME = "Ancient Knowledge";

				// Token: 0x0400BB07 RID: 47879
				public static LocString DESC = "This Duplicant has knowledge from the before times\n• Starts with 3 skill points";
			}

			// Token: 0x02002D0B RID: 11531
			public class CANTRESEARCH
			{
				// Token: 0x0400BB08 RID: 47880
				public static LocString NAME = "Yokel";

				// Token: 0x0400BB09 RID: 47881
				public static LocString DESC = "This Duplicant isn't the brightest star in the solar system";
			}

			// Token: 0x02002D0C RID: 11532
			public class CANTBUILD
			{
				// Token: 0x0400BB0A RID: 47882
				public static LocString NAME = "Unconstructive";

				// Token: 0x0400BB0B RID: 47883
				public static LocString DESC = "This Duplicant is incapable of building even the most basic of structures";
			}

			// Token: 0x02002D0D RID: 11533
			public class CANTCOOK
			{
				// Token: 0x0400BB0C RID: 47884
				public static LocString NAME = "Gastrophobia";

				// Token: 0x0400BB0D RID: 47885
				public static LocString DESC = "This Duplicant has a deep-seated distrust of the culinary arts";
			}

			// Token: 0x02002D0E RID: 11534
			public class CANTDIG
			{
				// Token: 0x0400BB0E RID: 47886
				public static LocString NAME = "Trypophobia";

				// Token: 0x0400BB0F RID: 47887
				public static LocString DESC = "This Duplicant's fear of holes makes it impossible for them to dig";
			}

			// Token: 0x02002D0F RID: 11535
			public class HEMOPHOBIA
			{
				// Token: 0x0400BB10 RID: 47888
				public static LocString NAME = "Squeamish";

				// Token: 0x0400BB11 RID: 47889
				public static LocString DESC = "This Duplicant is of delicate disposition and cannot tend to the sick";
			}

			// Token: 0x02002D10 RID: 11536
			public class BEDSIDEMANNER
			{
				// Token: 0x0400BB12 RID: 47890
				public static LocString NAME = "Caregiver";

				// Token: 0x0400BB13 RID: 47891
				public static LocString DESC = "This Duplicant has good bedside manner and a healing touch";
			}

			// Token: 0x02002D11 RID: 11537
			public class MOUTHBREATHER
			{
				// Token: 0x0400BB14 RID: 47892
				public static LocString NAME = "Mouth Breather";

				// Token: 0x0400BB15 RID: 47893
				public static LocString DESC = "This Duplicant sucks up way more than their fair share of " + ELEMENTS.OXYGEN.NAME;
			}

			// Token: 0x02002D12 RID: 11538
			public class FUSSY
			{
				// Token: 0x0400BB16 RID: 47894
				public static LocString NAME = "Fussy";

				// Token: 0x0400BB17 RID: 47895
				public static LocString DESC = "Nothing's ever quite good enough for this Duplicant";
			}

			// Token: 0x02002D13 RID: 11539
			public class TWINKLETOES
			{
				// Token: 0x0400BB18 RID: 47896
				public static LocString NAME = "Twinkletoes";

				// Token: 0x0400BB19 RID: 47897
				public static LocString DESC = "This Duplicant is light as a feather on their feet";
			}

			// Token: 0x02002D14 RID: 11540
			public class STRONGARM
			{
				// Token: 0x0400BB1A RID: 47898
				public static LocString NAME = "Buff";

				// Token: 0x0400BB1B RID: 47899
				public static LocString DESC = "This Duplicant has muscles on their muscles";
			}

			// Token: 0x02002D15 RID: 11541
			public class NOODLEARMS
			{
				// Token: 0x0400BB1C RID: 47900
				public static LocString NAME = "Noodle Arms";

				// Token: 0x0400BB1D RID: 47901
				public static LocString DESC = "This Duplicant's arms have all the tensile strength of overcooked linguine";
			}

			// Token: 0x02002D16 RID: 11542
			public class AGGRESSIVE
			{
				// Token: 0x0400BB1E RID: 47902
				public static LocString NAME = "Destructive";

				// Token: 0x0400BB1F RID: 47903
				public static LocString DESC = "This Duplicant handles stress by taking their frustrations out on defenseless machines";

				// Token: 0x0400BB20 RID: 47904
				public static LocString NOREPAIR = "• Will not repair buildings while above 60% " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D17 RID: 11543
			public class UGLYCRIER
			{
				// Token: 0x0400BB21 RID: 47905
				public static LocString NAME = "Ugly Crier";

				// Token: 0x0400BB22 RID: 47906
				public static LocString DESC = string.Concat(new string[]
				{
					"If this Duplicant gets too ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" it won't be pretty"
				});
			}

			// Token: 0x02002D18 RID: 11544
			public class BINGEEATER
			{
				// Token: 0x0400BB23 RID: 47907
				public static LocString NAME = "Binge Eater";

				// Token: 0x0400BB24 RID: 47908
				public static LocString DESC = "This Duplicant will dangerously overeat when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D19 RID: 11545
			public class ANXIOUS
			{
				// Token: 0x0400BB25 RID: 47909
				public static LocString NAME = "Anxious";

				// Token: 0x0400BB26 RID: 47910
				public static LocString DESC = "This Duplicant collapses when put under too much pressure";
			}

			// Token: 0x02002D1A RID: 11546
			public class STRESSVOMITER
			{
				// Token: 0x0400BB27 RID: 47911
				public static LocString NAME = "Vomiter";

				// Token: 0x0400BB28 RID: 47912
				public static LocString DESC = "This Duplicant is liable to puke everywhere when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D1B RID: 11547
			public class BANSHEE
			{
				// Token: 0x0400BB29 RID: 47913
				public static LocString NAME = "Banshee";

				// Token: 0x0400BB2A RID: 47914
				public static LocString DESC = "This Duplicant wails uncontrollably when " + UI.PRE_KEYWORD + "Stressed" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D1C RID: 11548
			public class BALLOONARTIST
			{
				// Token: 0x0400BB2B RID: 47915
				public static LocString NAME = "Balloon Artist";

				// Token: 0x0400BB2C RID: 47916
				public static LocString DESC = "This Duplicant hands out balloons when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D1D RID: 11549
			public class SPARKLESTREAKER
			{
				// Token: 0x0400BB2D RID: 47917
				public static LocString NAME = "Sparkle Streaker";

				// Token: 0x0400BB2E RID: 47918
				public static LocString DESC = "This Duplicant leaves a trail of happy sparkles when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D1E RID: 11550
			public class STICKERBOMBER
			{
				// Token: 0x0400BB2F RID: 47919
				public static LocString NAME = "Sticker Bomber";

				// Token: 0x0400BB30 RID: 47920
				public static LocString DESC = "This Duplicant will spontaneously redecorate a room when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D1F RID: 11551
			public class SUPERPRODUCTIVE
			{
				// Token: 0x0400BB31 RID: 47921
				public static LocString NAME = "Super Productive";

				// Token: 0x0400BB32 RID: 47922
				public static LocString DESC = "This Duplicant is super productive when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D20 RID: 11552
			public class HAPPYSINGER
			{
				// Token: 0x0400BB33 RID: 47923
				public static LocString NAME = "Yodeler";

				// Token: 0x0400BB34 RID: 47924
				public static LocString DESC = "This Duplicant belts out catchy tunes when they are " + UI.PRE_KEYWORD + "Overjoyed" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D21 RID: 11553
			public class IRONGUT
			{
				// Token: 0x0400BB35 RID: 47925
				public static LocString NAME = "Iron Gut";

				// Token: 0x0400BB36 RID: 47926
				public static LocString DESC = "This Duplicant can eat just about anything without getting sick";

				// Token: 0x0400BB37 RID: 47927
				public static LocString SHORT_DESC = "Immune to <b>" + DUPLICANTS.DISEASES.FOODSICKNESS.NAME + "</b>";

				// Token: 0x0400BB38 RID: 47928
				public static LocString SHORT_DESC_TOOLTIP = "Eating food contaminated with " + DUPLICANTS.DISEASES.FOODSICKNESS.NAME + " Germs will not affect this Duplicant";
			}

			// Token: 0x02002D22 RID: 11554
			public class STRONGIMMUNESYSTEM
			{
				// Token: 0x0400BB39 RID: 47929
				public static LocString NAME = "Germ Resistant";

				// Token: 0x0400BB3A RID: 47930
				public static LocString DESC = "This Duplicant's immune system bounces back faster than most";
			}

			// Token: 0x02002D23 RID: 11555
			public class SCAREDYCAT
			{
				// Token: 0x0400BB3B RID: 47931
				public static LocString NAME = "Pacifist";

				// Token: 0x0400BB3C RID: 47932
				public static LocString DESC = "This Duplicant abhors violence";
			}

			// Token: 0x02002D24 RID: 11556
			public class ALLERGIES
			{
				// Token: 0x0400BB3D RID: 47933
				public static LocString NAME = "Allergies";

				// Token: 0x0400BB3E RID: 47934
				public static LocString DESC = "This Duplicant will sneeze uncontrollably when exposed to the pollen present in " + DUPLICANTS.DISEASES.POLLENGERMS.NAME;

				// Token: 0x0400BB3F RID: 47935
				public static LocString SHORT_DESC = "Allergic reaction to <b>" + DUPLICANTS.DISEASES.POLLENGERMS.NAME + "</b>";

				// Token: 0x0400BB40 RID: 47936
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.DISEASES.ALLERGIES.DESCRIPTIVE_SYMPTOMS;
			}

			// Token: 0x02002D25 RID: 11557
			public class WEAKIMMUNESYSTEM
			{
				// Token: 0x0400BB41 RID: 47937
				public static LocString NAME = "Biohazardous";

				// Token: 0x0400BB42 RID: 47938
				public static LocString DESC = "All the vitamin C in space couldn't stop this Duplicant from getting sick";
			}

			// Token: 0x02002D26 RID: 11558
			public class IRRITABLEBOWEL
			{
				// Token: 0x0400BB43 RID: 47939
				public static LocString NAME = "Irritable Bowel";

				// Token: 0x0400BB44 RID: 47940
				public static LocString DESC = "This Duplicant needs a little extra time to \"do their business\"";
			}

			// Token: 0x02002D27 RID: 11559
			public class CALORIEBURNER
			{
				// Token: 0x0400BB45 RID: 47941
				public static LocString NAME = "Bottomless Stomach";

				// Token: 0x0400BB46 RID: 47942
				public static LocString DESC = "This Duplicant might actually be several black holes in a trench coat";
			}

			// Token: 0x02002D28 RID: 11560
			public class SMALLBLADDER
			{
				// Token: 0x0400BB47 RID: 47943
				public static LocString NAME = "Small Bladder";

				// Token: 0x0400BB48 RID: 47944
				public static LocString DESC = string.Concat(new string[]
				{
					"This Duplicant has a tiny, pea-sized ",
					UI.PRE_KEYWORD,
					"Bladder",
					UI.PST_KEYWORD,
					". Adorable!"
				});
			}

			// Token: 0x02002D29 RID: 11561
			public class ANEMIC
			{
				// Token: 0x0400BB49 RID: 47945
				public static LocString NAME = "Anemic";

				// Token: 0x0400BB4A RID: 47946
				public static LocString DESC = "This Duplicant has trouble keeping up with the others";
			}

			// Token: 0x02002D2A RID: 11562
			public class GREASEMONKEY
			{
				// Token: 0x0400BB4B RID: 47947
				public static LocString NAME = "Grease Monkey";

				// Token: 0x0400BB4C RID: 47948
				public static LocString DESC = "This Duplicant likes to throw a wrench into the colony's plans... in a good way";
			}

			// Token: 0x02002D2B RID: 11563
			public class MOLEHANDS
			{
				// Token: 0x0400BB4D RID: 47949
				public static LocString NAME = "Mole Hands";

				// Token: 0x0400BB4E RID: 47950
				public static LocString DESC = "They're great for tunneling, but finding good gloves is a nightmare";
			}

			// Token: 0x02002D2C RID: 11564
			public class FASTLEARNER
			{
				// Token: 0x0400BB4F RID: 47951
				public static LocString NAME = "Quick Learner";

				// Token: 0x0400BB50 RID: 47952
				public static LocString DESC = "This Duplicant's sharp as a tack and learns new skills with amazing speed";
			}

			// Token: 0x02002D2D RID: 11565
			public class SLOWLEARNER
			{
				// Token: 0x0400BB51 RID: 47953
				public static LocString NAME = "Slow Learner";

				// Token: 0x0400BB52 RID: 47954
				public static LocString DESC = "This Duplicant's a little slow on the uptake, but gosh do they try";
			}

			// Token: 0x02002D2E RID: 11566
			public class DIVERSLUNG
			{
				// Token: 0x0400BB53 RID: 47955
				public static LocString NAME = "Diver's Lungs";

				// Token: 0x0400BB54 RID: 47956
				public static LocString DESC = "This Duplicant could have been a talented opera singer in another life";
			}

			// Token: 0x02002D2F RID: 11567
			public class FLATULENCE
			{
				// Token: 0x0400BB55 RID: 47957
				public static LocString NAME = "Flatulent";

				// Token: 0x0400BB56 RID: 47958
				public static LocString DESC = "Some Duplicants are just full of it";

				// Token: 0x0400BB57 RID: 47959
				public static LocString SHORT_DESC = "Farts frequently";

				// Token: 0x0400BB58 RID: 47960
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant will periodically \"output\" " + ELEMENTS.METHANE.NAME;
			}

			// Token: 0x02002D30 RID: 11568
			public class SNORER
			{
				// Token: 0x0400BB59 RID: 47961
				public static LocString NAME = "Loud Sleeper";

				// Token: 0x0400BB5A RID: 47962
				public static LocString DESC = "In space, everyone can hear you snore";

				// Token: 0x0400BB5B RID: 47963
				public static LocString SHORT_DESC = "Snores loudly";

				// Token: 0x0400BB5C RID: 47964
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant's snoring will rudely awake nearby friends";
			}

			// Token: 0x02002D31 RID: 11569
			public class NARCOLEPSY
			{
				// Token: 0x0400BB5D RID: 47965
				public static LocString NAME = "Narcoleptic";

				// Token: 0x0400BB5E RID: 47966
				public static LocString DESC = "This Duplicant can and will fall asleep anytime, anyplace";

				// Token: 0x0400BB5F RID: 47967
				public static LocString SHORT_DESC = "Falls asleep periodically";

				// Token: 0x0400BB60 RID: 47968
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant's work will be periodically interrupted by naps";
			}

			// Token: 0x02002D32 RID: 11570
			public class INTERIORDECORATOR
			{
				// Token: 0x0400BB61 RID: 47969
				public static LocString NAME = "Interior Decorator";

				// Token: 0x0400BB62 RID: 47970
				public static LocString DESC = "\"Move it a little to the left...\"";
			}

			// Token: 0x02002D33 RID: 11571
			public class UNCULTURED
			{
				// Token: 0x0400BB63 RID: 47971
				public static LocString NAME = "Uncultured";

				// Token: 0x0400BB64 RID: 47972
				public static LocString DESC = "This Duplicant has simply no appreciation for the arts";
			}

			// Token: 0x02002D34 RID: 11572
			public class EARLYBIRD
			{
				// Token: 0x0400BB65 RID: 47973
				public static LocString NAME = "Early Bird";

				// Token: 0x0400BB66 RID: 47974
				public static LocString DESC = "This Duplicant always wakes up feeling fresh and efficient!";

				// Token: 0x0400BB67 RID: 47975
				public static LocString EXTENDED_DESC = string.Concat(new string[]
				{
					"• Morning: <b>{0}</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: 5 Schedule Blocks"
				});

				// Token: 0x0400BB68 RID: 47976
				public static LocString SHORT_DESC = "Gains morning Attribute bonuses";

				// Token: 0x0400BB69 RID: 47977
				public static LocString SHORT_DESC_TOOLTIP = string.Concat(new string[]
				{
					"Morning: <b>+2</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: 5 Schedule Blocks"
				});
			}

			// Token: 0x02002D35 RID: 11573
			public class NIGHTOWL
			{
				// Token: 0x0400BB6A RID: 47978
				public static LocString NAME = "Night Owl";

				// Token: 0x0400BB6B RID: 47979
				public static LocString DESC = "This Duplicant does their best work when they'd ought to be sleeping";

				// Token: 0x0400BB6C RID: 47980
				public static LocString EXTENDED_DESC = string.Concat(new string[]
				{
					"• Nighttime: <b>{0}</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: All Night"
				});

				// Token: 0x0400BB6D RID: 47981
				public static LocString SHORT_DESC = "Gains nighttime Attribute bonuses";

				// Token: 0x0400BB6E RID: 47982
				public static LocString SHORT_DESC_TOOLTIP = string.Concat(new string[]
				{
					"Nighttime: <b>+3</b> bonus to all ",
					UI.PRE_KEYWORD,
					"Attributes",
					UI.PST_KEYWORD,
					"\n• Duration: All Night"
				});
			}

			// Token: 0x02002D36 RID: 11574
			public class METEORPHILE
			{
				// Token: 0x0400BB6F RID: 47983
				public static LocString NAME = "Rock Fan";

				// Token: 0x0400BB70 RID: 47984
				public static LocString DESC = "Meteor showers get this Duplicant really, really hyped";

				// Token: 0x0400BB71 RID: 47985
				public static LocString EXTENDED_DESC = "• During meteor showers: <b>{0}</b> bonus to all " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD;

				// Token: 0x0400BB72 RID: 47986
				public static LocString SHORT_DESC = "Gains Attribute bonuses during meteor showers.";

				// Token: 0x0400BB73 RID: 47987
				public static LocString SHORT_DESC_TOOLTIP = "During meteor showers: <b>+3</b> bonus to all " + UI.PRE_KEYWORD + "Attributes" + UI.PST_KEYWORD;
			}

			// Token: 0x02002D37 RID: 11575
			public class REGENERATION
			{
				// Token: 0x0400BB74 RID: 47988
				public static LocString NAME = "Regenerative";

				// Token: 0x0400BB75 RID: 47989
				public static LocString DESC = "This robust Duplicant is constantly regenerating health";
			}

			// Token: 0x02002D38 RID: 11576
			public class DEEPERDIVERSLUNGS
			{
				// Token: 0x0400BB76 RID: 47990
				public static LocString NAME = "Deep Diver's Lungs";

				// Token: 0x0400BB77 RID: 47991
				public static LocString DESC = "This Duplicant has a frankly impressive ability to hold their breath";
			}

			// Token: 0x02002D39 RID: 11577
			public class SUNNYDISPOSITION
			{
				// Token: 0x0400BB78 RID: 47992
				public static LocString NAME = "Sunny Disposition";

				// Token: 0x0400BB79 RID: 47993
				public static LocString DESC = "This Duplicant has an unwaveringly positive outlook on life";
			}

			// Token: 0x02002D3A RID: 11578
			public class ROCKCRUSHER
			{
				// Token: 0x0400BB7A RID: 47994
				public static LocString NAME = "Beefsteak";

				// Token: 0x0400BB7B RID: 47995
				public static LocString DESC = "This Duplicant's got muscles on their muscles!";
			}

			// Token: 0x02002D3B RID: 11579
			public class SIMPLETASTES
			{
				// Token: 0x0400BB7C RID: 47996
				public static LocString NAME = "Shrivelled Tastebuds";

				// Token: 0x0400BB7D RID: 47997
				public static LocString DESC = "This Duplicant could lick a Puft's backside and taste nothing";
			}

			// Token: 0x02002D3C RID: 11580
			public class FOODIE
			{
				// Token: 0x0400BB7E RID: 47998
				public static LocString NAME = "Gourmet";

				// Token: 0x0400BB7F RID: 47999
				public static LocString DESC = "This Duplicant's refined palate demands only the most luxurious dishes the colony can offer";
			}

			// Token: 0x02002D3D RID: 11581
			public class ARCHAEOLOGIST
			{
				// Token: 0x0400BB80 RID: 48000
				public static LocString NAME = "Relic Hunter";

				// Token: 0x0400BB81 RID: 48001
				public static LocString DESC = "This Duplicant was never taught the phrase \"take only pictures, leave only footprints\"";
			}

			// Token: 0x02002D3E RID: 11582
			public class DECORUP
			{
				// Token: 0x0400BB82 RID: 48002
				public static LocString NAME = "Innately Stylish";

				// Token: 0x0400BB83 RID: 48003
				public static LocString DESC = "This Duplicant's radiant self-confidence makes even the rattiest outfits look trendy";
			}

			// Token: 0x02002D3F RID: 11583
			public class DECORDOWN
			{
				// Token: 0x0400BB84 RID: 48004
				public static LocString NAME = "Shabby Dresser";

				// Token: 0x0400BB85 RID: 48005
				public static LocString DESC = "This Duplicant's clearly never heard of ironing";
			}

			// Token: 0x02002D40 RID: 11584
			public class THRIVER
			{
				// Token: 0x0400BB86 RID: 48006
				public static LocString NAME = "Duress to Impress";

				// Token: 0x0400BB87 RID: 48007
				public static LocString DESC = "This Duplicant kicks into hyperdrive when the stress is on";

				// Token: 0x0400BB88 RID: 48008
				public static LocString SHORT_DESC = "Attribute bonuses while stressed";

				// Token: 0x0400BB89 RID: 48009
				public static LocString SHORT_DESC_TOOLTIP = "More than 60% Stress: <b>+7</b> bonus to all " + UI.FormatAsKeyWord("Attributes");
			}

			// Token: 0x02002D41 RID: 11585
			public class LONER
			{
				// Token: 0x0400BB8A RID: 48010
				public static LocString NAME = "Loner";

				// Token: 0x0400BB8B RID: 48011
				public static LocString DESC = "This Duplicant prefers solitary pursuits";

				// Token: 0x0400BB8C RID: 48012
				public static LocString SHORT_DESC = "Attribute bonuses while alone";

				// Token: 0x0400BB8D RID: 48013
				public static LocString SHORT_DESC_TOOLTIP = "Only Duplicant on a world: <b>+4</b> bonus to all " + UI.FormatAsKeyWord("Attributes");
			}

			// Token: 0x02002D42 RID: 11586
			public class STARRYEYED
			{
				// Token: 0x0400BB8E RID: 48014
				public static LocString NAME = "Starry Eyed";

				// Token: 0x0400BB8F RID: 48015
				public static LocString DESC = "This Duplicant loves being in space";

				// Token: 0x0400BB90 RID: 48016
				public static LocString SHORT_DESC = "Morale bonus while in space";

				// Token: 0x0400BB91 RID: 48017
				public static LocString SHORT_DESC_TOOLTIP = "In outer space: <b>+10</b> " + UI.FormatAsKeyWord("Morale");
			}

			// Token: 0x02002D43 RID: 11587
			public class GLOWSTICK
			{
				// Token: 0x0400BB92 RID: 48018
				public static LocString NAME = "Glow Stick";

				// Token: 0x0400BB93 RID: 48019
				public static LocString DESC = "This Duplicant is positively glowing";

				// Token: 0x0400BB94 RID: 48020
				public static LocString SHORT_DESC = "Emits low amounts of rads and light";

				// Token: 0x0400BB95 RID: 48021
				public static LocString SHORT_DESC_TOOLTIP = "Emits low amounts of rads and light";
			}

			// Token: 0x02002D44 RID: 11588
			public class RADIATIONEATER
			{
				// Token: 0x0400BB96 RID: 48022
				public static LocString NAME = "Radiation Eater";

				// Token: 0x0400BB97 RID: 48023
				public static LocString DESC = "This Duplicant eats radiation for breakfast (and dinner)";

				// Token: 0x0400BB98 RID: 48024
				public static LocString SHORT_DESC = "Converts radiation exposure into calories";

				// Token: 0x0400BB99 RID: 48025
				public static LocString SHORT_DESC_TOOLTIP = "Converts radiation exposure into calories";
			}

			// Token: 0x02002D45 RID: 11589
			public class NIGHTLIGHT
			{
				// Token: 0x0400BB9A RID: 48026
				public static LocString NAME = "Nyctophobic";

				// Token: 0x0400BB9B RID: 48027
				public static LocString DESC = "This Duplicant will imagine scary shapes in the dark all night if no one leaves a light on";

				// Token: 0x0400BB9C RID: 48028
				public static LocString SHORT_DESC = "Requires light to sleep";

				// Token: 0x0400BB9D RID: 48029
				public static LocString SHORT_DESC_TOOLTIP = "This Duplicant can't sleep in complete darkness";
			}

			// Token: 0x02002D46 RID: 11590
			public class GREENTHUMB
			{
				// Token: 0x0400BB9E RID: 48030
				public static LocString NAME = "Green Thumb";

				// Token: 0x0400BB9F RID: 48031
				public static LocString DESC = "This Duplicant regards every plant as a potential friend";
			}

			// Token: 0x02002D47 RID: 11591
			public class CONSTRUCTIONUP
			{
				// Token: 0x0400BBA0 RID: 48032
				public static LocString NAME = "Handy";

				// Token: 0x0400BBA1 RID: 48033
				public static LocString DESC = "This Duplicant is a swift and skilled builder";
			}

			// Token: 0x02002D48 RID: 11592
			public class RANCHINGUP
			{
				// Token: 0x0400BBA2 RID: 48034
				public static LocString NAME = "Animal Lover";

				// Token: 0x0400BBA3 RID: 48035
				public static LocString DESC = "The fuzzy snoots! The little claws! The chitinous exoskeletons! This Duplicant's never met a critter they didn't like";
			}

			// Token: 0x02002D49 RID: 11593
			public class CONSTRUCTIONDOWN
			{
				// Token: 0x0400BBA4 RID: 48036
				public static LocString NAME = "Building Impaired";

				// Token: 0x0400BBA5 RID: 48037
				public static LocString DESC = "This Duplicant has trouble constructing anything besides meaningful friendships";
			}

			// Token: 0x02002D4A RID: 11594
			public class RANCHINGDOWN
			{
				// Token: 0x0400BBA6 RID: 48038
				public static LocString NAME = "Critter Aversion";

				// Token: 0x0400BBA7 RID: 48039
				public static LocString DESC = "This Duplicant just doesn't trust those beady little eyes";
			}

			// Token: 0x02002D4B RID: 11595
			public class DIGGINGDOWN
			{
				// Token: 0x0400BBA8 RID: 48040
				public static LocString NAME = "Undigging";

				// Token: 0x0400BBA9 RID: 48041
				public static LocString DESC = "This Duplicant couldn't dig themselves out of a paper bag";
			}

			// Token: 0x02002D4C RID: 11596
			public class MACHINERYDOWN
			{
				// Token: 0x0400BBAA RID: 48042
				public static LocString NAME = "Luddite";

				// Token: 0x0400BBAB RID: 48043
				public static LocString DESC = "This Duplicant always invites friends over just to make them hook up their electronics";
			}

			// Token: 0x02002D4D RID: 11597
			public class COOKINGDOWN
			{
				// Token: 0x0400BBAC RID: 48044
				public static LocString NAME = "Kitchen Menace";

				// Token: 0x0400BBAD RID: 48045
				public static LocString DESC = "This Duplicant could probably figure out a way to burn ice cream";
			}

			// Token: 0x02002D4E RID: 11598
			public class ARTDOWN
			{
				// Token: 0x0400BBAE RID: 48046
				public static LocString NAME = "Unpracticed Artist";

				// Token: 0x0400BBAF RID: 48047
				public static LocString DESC = "This Duplicant proudly proclaims they \"can't even draw a stick figure\"";
			}

			// Token: 0x02002D4F RID: 11599
			public class CARINGDOWN
			{
				// Token: 0x0400BBB0 RID: 48048
				public static LocString NAME = "Unempathetic";

				// Token: 0x0400BBB1 RID: 48049
				public static LocString DESC = "This Duplicant's lack of bedside manner makes it difficult for them to nurse peers back to health";
			}

			// Token: 0x02002D50 RID: 11600
			public class BOTANISTDOWN
			{
				// Token: 0x0400BBB2 RID: 48050
				public static LocString NAME = "Plant Murderer";

				// Token: 0x0400BBB3 RID: 48051
				public static LocString DESC = "Never ask this Duplicant to watch your ferns when you go on vacation";
			}

			// Token: 0x02002D51 RID: 11601
			public class GRANTSKILL_MINING1
			{
				// Token: 0x0400BBB4 RID: 48052
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_MINER.NAME;

				// Token: 0x0400BBB5 RID: 48053
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_MINER.DESCRIPTION;

				// Token: 0x0400BBB6 RID: 48054
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400BBB7 RID: 48055
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D52 RID: 11602
			public class GRANTSKILL_MINING2
			{
				// Token: 0x0400BBB8 RID: 48056
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MINER.NAME;

				// Token: 0x0400BBB9 RID: 48057
				public static LocString DESC = DUPLICANTS.ROLES.MINER.DESCRIPTION;

				// Token: 0x0400BBBA RID: 48058
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400BBBB RID: 48059
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D53 RID: 11603
			public class GRANTSKILL_MINING3
			{
				// Token: 0x0400BBBC RID: 48060
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_MINER.NAME;

				// Token: 0x0400BBBD RID: 48061
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_MINER.DESCRIPTION;

				// Token: 0x0400BBBE RID: 48062
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400BBBF RID: 48063
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D54 RID: 11604
			public class GRANTSKILL_MINING4
			{
				// Token: 0x0400BBC0 RID: 48064
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MASTER_MINER.NAME;

				// Token: 0x0400BBC1 RID: 48065
				public static LocString DESC = DUPLICANTS.ROLES.MASTER_MINER.DESCRIPTION;

				// Token: 0x0400BBC2 RID: 48066
				public static LocString SHORT_DESC = "Starts with a Tier 4 <b>Skill</b>";

				// Token: 0x0400BBC3 RID: 48067
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D55 RID: 11605
			public class GRANTSKILL_BUILDING1
			{
				// Token: 0x0400BBC4 RID: 48068
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_BUILDER.NAME;

				// Token: 0x0400BBC5 RID: 48069
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_BUILDER.DESCRIPTION;

				// Token: 0x0400BBC6 RID: 48070
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400BBC7 RID: 48071
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D56 RID: 11606
			public class GRANTSKILL_BUILDING2
			{
				// Token: 0x0400BBC8 RID: 48072
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.BUILDER.NAME;

				// Token: 0x0400BBC9 RID: 48073
				public static LocString DESC = DUPLICANTS.ROLES.BUILDER.DESCRIPTION;

				// Token: 0x0400BBCA RID: 48074
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400BBCB RID: 48075
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D57 RID: 11607
			public class GRANTSKILL_BUILDING3
			{
				// Token: 0x0400BBCC RID: 48076
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_BUILDER.NAME;

				// Token: 0x0400BBCD RID: 48077
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_BUILDER.DESCRIPTION;

				// Token: 0x0400BBCE RID: 48078
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400BBCF RID: 48079
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D58 RID: 11608
			public class GRANTSKILL_FARMING1
			{
				// Token: 0x0400BBD0 RID: 48080
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_FARMER.NAME;

				// Token: 0x0400BBD1 RID: 48081
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_FARMER.DESCRIPTION;

				// Token: 0x0400BBD2 RID: 48082
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400BBD3 RID: 48083
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D59 RID: 11609
			public class GRANTSKILL_FARMING2
			{
				// Token: 0x0400BBD4 RID: 48084
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.FARMER.NAME;

				// Token: 0x0400BBD5 RID: 48085
				public static LocString DESC = DUPLICANTS.ROLES.FARMER.DESCRIPTION;

				// Token: 0x0400BBD6 RID: 48086
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400BBD7 RID: 48087
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D5A RID: 11610
			public class GRANTSKILL_FARMING3
			{
				// Token: 0x0400BBD8 RID: 48088
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_FARMER.NAME;

				// Token: 0x0400BBD9 RID: 48089
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_FARMER.DESCRIPTION;

				// Token: 0x0400BBDA RID: 48090
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400BBDB RID: 48091
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D5B RID: 11611
			public class GRANTSKILL_RANCHING1
			{
				// Token: 0x0400BBDC RID: 48092
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.RANCHER.NAME;

				// Token: 0x0400BBDD RID: 48093
				public static LocString DESC = DUPLICANTS.ROLES.RANCHER.DESCRIPTION;

				// Token: 0x0400BBDE RID: 48094
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400BBDF RID: 48095
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D5C RID: 11612
			public class GRANTSKILL_RANCHING2
			{
				// Token: 0x0400BBE0 RID: 48096
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_RANCHER.NAME;

				// Token: 0x0400BBE1 RID: 48097
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_RANCHER.DESCRIPTION;

				// Token: 0x0400BBE2 RID: 48098
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400BBE3 RID: 48099
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D5D RID: 11613
			public class GRANTSKILL_RESEARCHING1
			{
				// Token: 0x0400BBE4 RID: 48100
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_RESEARCHER.NAME;

				// Token: 0x0400BBE5 RID: 48101
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_RESEARCHER.DESCRIPTION;

				// Token: 0x0400BBE6 RID: 48102
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400BBE7 RID: 48103
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D5E RID: 11614
			public class GRANTSKILL_RESEARCHING2
			{
				// Token: 0x0400BBE8 RID: 48104
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.RESEARCHER.NAME;

				// Token: 0x0400BBE9 RID: 48105
				public static LocString DESC = DUPLICANTS.ROLES.RESEARCHER.DESCRIPTION;

				// Token: 0x0400BBEA RID: 48106
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400BBEB RID: 48107
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D5F RID: 11615
			public class GRANTSKILL_RESEARCHING3
			{
				// Token: 0x0400BBEC RID: 48108
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_RESEARCHER.NAME;

				// Token: 0x0400BBED RID: 48109
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_RESEARCHER.DESCRIPTION;

				// Token: 0x0400BBEE RID: 48110
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400BBEF RID: 48111
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D60 RID: 11616
			public class GRANTSKILL_RESEARCHING4
			{
				// Token: 0x0400BBF0 RID: 48112
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.NAME;

				// Token: 0x0400BBF1 RID: 48113
				public static LocString DESC = DUPLICANTS.ROLES.NUCLEAR_RESEARCHER.DESCRIPTION;

				// Token: 0x0400BBF2 RID: 48114
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400BBF3 RID: 48115
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D61 RID: 11617
			public class GRANTSKILL_COOKING1
			{
				// Token: 0x0400BBF4 RID: 48116
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_COOK.NAME;

				// Token: 0x0400BBF5 RID: 48117
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_COOK.DESCRIPTION;

				// Token: 0x0400BBF6 RID: 48118
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400BBF7 RID: 48119
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D62 RID: 11618
			public class GRANTSKILL_COOKING2
			{
				// Token: 0x0400BBF8 RID: 48120
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.COOK.NAME;

				// Token: 0x0400BBF9 RID: 48121
				public static LocString DESC = DUPLICANTS.ROLES.COOK.DESCRIPTION;

				// Token: 0x0400BBFA RID: 48122
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400BBFB RID: 48123
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D63 RID: 11619
			public class GRANTSKILL_ARTING1
			{
				// Token: 0x0400BBFC RID: 48124
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_ARTIST.NAME;

				// Token: 0x0400BBFD RID: 48125
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_ARTIST.DESCRIPTION;

				// Token: 0x0400BBFE RID: 48126
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400BBFF RID: 48127
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D64 RID: 11620
			public class GRANTSKILL_ARTING2
			{
				// Token: 0x0400BC00 RID: 48128
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.ARTIST.NAME;

				// Token: 0x0400BC01 RID: 48129
				public static LocString DESC = DUPLICANTS.ROLES.ARTIST.DESCRIPTION;

				// Token: 0x0400BC02 RID: 48130
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400BC03 RID: 48131
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D65 RID: 11621
			public class GRANTSKILL_ARTING3
			{
				// Token: 0x0400BC04 RID: 48132
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MASTER_ARTIST.NAME;

				// Token: 0x0400BC05 RID: 48133
				public static LocString DESC = DUPLICANTS.ROLES.MASTER_ARTIST.DESCRIPTION;

				// Token: 0x0400BC06 RID: 48134
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400BC07 RID: 48135
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D66 RID: 11622
			public class GRANTSKILL_HAULING1
			{
				// Token: 0x0400BC08 RID: 48136
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.HAULER.NAME;

				// Token: 0x0400BC09 RID: 48137
				public static LocString DESC = DUPLICANTS.ROLES.HAULER.DESCRIPTION;

				// Token: 0x0400BC0A RID: 48138
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400BC0B RID: 48139
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D67 RID: 11623
			public class GRANTSKILL_HAULING2
			{
				// Token: 0x0400BC0C RID: 48140
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MATERIALS_MANAGER.NAME;

				// Token: 0x0400BC0D RID: 48141
				public static LocString DESC = DUPLICANTS.ROLES.MATERIALS_MANAGER.DESCRIPTION;

				// Token: 0x0400BC0E RID: 48142
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400BC0F RID: 48143
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D68 RID: 11624
			public class GRANTSKILL_SUITS1
			{
				// Token: 0x0400BC10 RID: 48144
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SUIT_EXPERT.NAME;

				// Token: 0x0400BC11 RID: 48145
				public static LocString DESC = DUPLICANTS.ROLES.SUIT_EXPERT.DESCRIPTION;

				// Token: 0x0400BC12 RID: 48146
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400BC13 RID: 48147
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D69 RID: 11625
			public class GRANTSKILL_TECHNICALS1
			{
				// Token: 0x0400BC14 RID: 48148
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MACHINE_TECHNICIAN.NAME;

				// Token: 0x0400BC15 RID: 48149
				public static LocString DESC = DUPLICANTS.ROLES.MACHINE_TECHNICIAN.DESCRIPTION;

				// Token: 0x0400BC16 RID: 48150
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400BC17 RID: 48151
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D6A RID: 11626
			public class GRANTSKILL_TECHNICALS2
			{
				// Token: 0x0400BC18 RID: 48152
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.POWER_TECHNICIAN.NAME;

				// Token: 0x0400BC19 RID: 48153
				public static LocString DESC = DUPLICANTS.ROLES.POWER_TECHNICIAN.DESCRIPTION;

				// Token: 0x0400BC1A RID: 48154
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400BC1B RID: 48155
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D6B RID: 11627
			public class GRANTSKILL_ENGINEERING1
			{
				// Token: 0x0400BC1C RID: 48156
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.NAME;

				// Token: 0x0400BC1D RID: 48157
				public static LocString DESC = DUPLICANTS.ROLES.MECHATRONIC_ENGINEER.DESCRIPTION;

				// Token: 0x0400BC1E RID: 48158
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400BC1F RID: 48159
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D6C RID: 11628
			public class GRANTSKILL_BASEKEEPING1
			{
				// Token: 0x0400BC20 RID: 48160
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.HANDYMAN.NAME;

				// Token: 0x0400BC21 RID: 48161
				public static LocString DESC = DUPLICANTS.ROLES.HANDYMAN.DESCRIPTION;

				// Token: 0x0400BC22 RID: 48162
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400BC23 RID: 48163
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D6D RID: 11629
			public class GRANTSKILL_BASEKEEPING2
			{
				// Token: 0x0400BC24 RID: 48164
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.PLUMBER.NAME;

				// Token: 0x0400BC25 RID: 48165
				public static LocString DESC = DUPLICANTS.ROLES.PLUMBER.DESCRIPTION;

				// Token: 0x0400BC26 RID: 48166
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400BC27 RID: 48167
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D6E RID: 11630
			public class GRANTSKILL_ASTRONAUTING1
			{
				// Token: 0x0400BC28 RID: 48168
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.ASTRONAUTTRAINEE.NAME;

				// Token: 0x0400BC29 RID: 48169
				public static LocString DESC = DUPLICANTS.ROLES.ASTRONAUTTRAINEE.DESCRIPTION;

				// Token: 0x0400BC2A RID: 48170
				public static LocString SHORT_DESC = "Starts with a Tier 4 <b>Skill</b>";

				// Token: 0x0400BC2B RID: 48171
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D6F RID: 11631
			public class GRANTSKILL_ASTRONAUTING2
			{
				// Token: 0x0400BC2C RID: 48172
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.ASTRONAUT.NAME;

				// Token: 0x0400BC2D RID: 48173
				public static LocString DESC = DUPLICANTS.ROLES.ASTRONAUT.DESCRIPTION;

				// Token: 0x0400BC2E RID: 48174
				public static LocString SHORT_DESC = "Starts with a Tier 5 <b>Skill</b>";

				// Token: 0x0400BC2F RID: 48175
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D70 RID: 11632
			public class GRANTSKILL_MEDICINE1
			{
				// Token: 0x0400BC30 RID: 48176
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.JUNIOR_MEDIC.NAME;

				// Token: 0x0400BC31 RID: 48177
				public static LocString DESC = DUPLICANTS.ROLES.JUNIOR_MEDIC.DESCRIPTION;

				// Token: 0x0400BC32 RID: 48178
				public static LocString SHORT_DESC = "Starts with a Tier 1 <b>Skill</b>";

				// Token: 0x0400BC33 RID: 48179
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D71 RID: 11633
			public class GRANTSKILL_MEDICINE2
			{
				// Token: 0x0400BC34 RID: 48180
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.MEDIC.NAME;

				// Token: 0x0400BC35 RID: 48181
				public static LocString DESC = DUPLICANTS.ROLES.MEDIC.DESCRIPTION;

				// Token: 0x0400BC36 RID: 48182
				public static LocString SHORT_DESC = "Starts with a Tier 2 <b>Skill</b>";

				// Token: 0x0400BC37 RID: 48183
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D72 RID: 11634
			public class GRANTSKILL_MEDICINE3
			{
				// Token: 0x0400BC38 RID: 48184
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.SENIOR_MEDIC.NAME;

				// Token: 0x0400BC39 RID: 48185
				public static LocString DESC = DUPLICANTS.ROLES.SENIOR_MEDIC.DESCRIPTION;

				// Token: 0x0400BC3A RID: 48186
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400BC3B RID: 48187
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}

			// Token: 0x02002D73 RID: 11635
			public class GRANTSKILL_PYROTECHNICS
			{
				// Token: 0x0400BC3C RID: 48188
				public static LocString NAME = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_NAME + DUPLICANTS.ROLES.PYROTECHNIC.NAME;

				// Token: 0x0400BC3D RID: 48189
				public static LocString DESC = DUPLICANTS.ROLES.PYROTECHNIC.DESCRIPTION;

				// Token: 0x0400BC3E RID: 48190
				public static LocString SHORT_DESC = "Starts with a Tier 3 <b>Skill</b>";

				// Token: 0x0400BC3F RID: 48191
				public static LocString SHORT_DESC_TOOLTIP = DUPLICANTS.TRAITS.GRANTED_SKILL_SHARED_SHORT_DESC_TOOLTIP;
			}
		}

		// Token: 0x02001DFF RID: 7679
		public class PERSONALITIES
		{
			// Token: 0x02002D74 RID: 11636
			public class CATALINA
			{
				// Token: 0x0400BC40 RID: 48192
				public static LocString NAME = "Catalina";

				// Token: 0x0400BC41 RID: 48193
				public static LocString DESC = "A {0} is admired by all for her seemingly tireless work ethic. Little do people know, she's dying on the inside.";
			}

			// Token: 0x02002D75 RID: 11637
			public class NISBET
			{
				// Token: 0x0400BC42 RID: 48194
				public static LocString NAME = "Nisbet";

				// Token: 0x0400BC43 RID: 48195
				public static LocString DESC = "This {0} likes to punch people to show her affection. Everyone's too afraid of her to tell her it hurts.";
			}

			// Token: 0x02002D76 RID: 11638
			public class ELLIE
			{
				// Token: 0x0400BC44 RID: 48196
				public static LocString NAME = "Ellie";

				// Token: 0x0400BC45 RID: 48197
				public static LocString DESC = "Nothing makes an {0} happier than a big tin of glitter and a pack of unicorn stickers.";
			}

			// Token: 0x02002D77 RID: 11639
			public class RUBY
			{
				// Token: 0x0400BC46 RID: 48198
				public static LocString NAME = "Ruby";

				// Token: 0x0400BC47 RID: 48199
				public static LocString DESC = "This {0} asks the pressing questions, like \"Where can I get a leather jacket in space?\"";
			}

			// Token: 0x02002D78 RID: 11640
			public class LEIRA
			{
				// Token: 0x0400BC48 RID: 48200
				public static LocString NAME = "Leira";

				// Token: 0x0400BC49 RID: 48201
				public static LocString DESC = "{0}s just want everyone to be happy.";
			}

			// Token: 0x02002D79 RID: 11641
			public class BUBBLES
			{
				// Token: 0x0400BC4A RID: 48202
				public static LocString NAME = "Bubbles";

				// Token: 0x0400BC4B RID: 48203
				public static LocString DESC = "This {0} is constantly challenging others to fight her, regardless of whether or not she can actually take them.";
			}

			// Token: 0x02002D7A RID: 11642
			public class MIMA
			{
				// Token: 0x0400BC4C RID: 48204
				public static LocString NAME = "Mi-Ma";

				// Token: 0x0400BC4D RID: 48205
				public static LocString DESC = "Ol' {0} here can't stand lookin' at people's knees.";
			}

			// Token: 0x02002D7B RID: 11643
			public class NAILS
			{
				// Token: 0x0400BC4E RID: 48206
				public static LocString NAME = "Nails";

				// Token: 0x0400BC4F RID: 48207
				public static LocString DESC = "People often expect a Duplicant named \"{0}\" to be tough, but they're all pretty huge wimps.";
			}

			// Token: 0x02002D7C RID: 11644
			public class MAE
			{
				// Token: 0x0400BC50 RID: 48208
				public static LocString NAME = "Mae";

				// Token: 0x0400BC51 RID: 48209
				public static LocString DESC = "There's nothing a {0} can't do if she sets her mind to it.";
			}

			// Token: 0x02002D7D RID: 11645
			public class GOSSMANN
			{
				// Token: 0x0400BC52 RID: 48210
				public static LocString NAME = "Gossmann";

				// Token: 0x0400BC53 RID: 48211
				public static LocString DESC = "{0}s are major goofballs who can make anyone laugh.";
			}

			// Token: 0x02002D7E RID: 11646
			public class MARIE
			{
				// Token: 0x0400BC54 RID: 48212
				public static LocString NAME = "Marie";

				// Token: 0x0400BC55 RID: 48213
				public static LocString DESC = "This {0} is positively glowing! What's her secret? Radioactive isotopes, of course.";
			}

			// Token: 0x02002D7F RID: 11647
			public class LINDSAY
			{
				// Token: 0x0400BC56 RID: 48214
				public static LocString NAME = "Lindsay";

				// Token: 0x0400BC57 RID: 48215
				public static LocString DESC = "A {0} is a charming woman, unless you make the mistake of messing with one of her friends.";
			}

			// Token: 0x02002D80 RID: 11648
			public class DEVON
			{
				// Token: 0x0400BC58 RID: 48216
				public static LocString NAME = "Devon";

				// Token: 0x0400BC59 RID: 48217
				public static LocString DESC = "This {0} dreams of owning their own personal computer so they can start a blog full of pictures of toast.";
			}

			// Token: 0x02002D81 RID: 11649
			public class REN
			{
				// Token: 0x0400BC5A RID: 48218
				public static LocString NAME = "Ren";

				// Token: 0x0400BC5B RID: 48219
				public static LocString DESC = "Every {0} has this unshakable feeling that his life's already happened and he's just watching it unfold from a memory.";
			}

			// Token: 0x02002D82 RID: 11650
			public class FRANKIE
			{
				// Token: 0x0400BC5C RID: 48220
				public static LocString NAME = "Frankie";

				// Token: 0x0400BC5D RID: 48221
				public static LocString DESC = "There's nothing {0}s are more proud of than their thick, dignified eyebrows.";
			}

			// Token: 0x02002D83 RID: 11651
			public class BANHI
			{
				// Token: 0x0400BC5E RID: 48222
				public static LocString NAME = "Banhi";

				// Token: 0x0400BC5F RID: 48223
				public static LocString DESC = "The \"cool loner\" vibes that radiate off a {0} never fail to make the colony swoon.";
			}

			// Token: 0x02002D84 RID: 11652
			public class ADA
			{
				// Token: 0x0400BC60 RID: 48224
				public static LocString NAME = "Ada";

				// Token: 0x0400BC61 RID: 48225
				public static LocString DESC = "{0}s enjoy writing poetry in their downtime. Dark poetry.";
			}

			// Token: 0x02002D85 RID: 11653
			public class HASSAN
			{
				// Token: 0x0400BC62 RID: 48226
				public static LocString NAME = "Hassan";

				// Token: 0x0400BC63 RID: 48227
				public static LocString DESC = "If someone says something nice to a {0} he'll think about it nonstop for no less than three weeks.";
			}

			// Token: 0x02002D86 RID: 11654
			public class STINKY
			{
				// Token: 0x0400BC64 RID: 48228
				public static LocString NAME = "Stinky";

				// Token: 0x0400BC65 RID: 48229
				public static LocString DESC = "This {0} has never been invited to a party, which is a shame. His dance moves are incredible.";
			}

			// Token: 0x02002D87 RID: 11655
			public class JOSHUA
			{
				// Token: 0x0400BC66 RID: 48230
				public static LocString NAME = "Joshua";

				// Token: 0x0400BC67 RID: 48231
				public static LocString DESC = "{0}s are precious goobers. Other Duplicants are strangely incapable of cursing in a {0}'s presence.";
			}

			// Token: 0x02002D88 RID: 11656
			public class LIAM
			{
				// Token: 0x0400BC68 RID: 48232
				public static LocString NAME = "Liam";

				// Token: 0x0400BC69 RID: 48233
				public static LocString DESC = "No matter how much this {0} scrubs, he can never truly feel clean.";
			}

			// Token: 0x02002D89 RID: 11657
			public class ABE
			{
				// Token: 0x0400BC6A RID: 48234
				public static LocString NAME = "Abe";

				// Token: 0x0400BC6B RID: 48235
				public static LocString DESC = "{0}s are sweet, delicate flowers. They need to be treated gingerly, with great consideration for their feelings.";
			}

			// Token: 0x02002D8A RID: 11658
			public class BURT
			{
				// Token: 0x0400BC6C RID: 48236
				public static LocString NAME = "Burt";

				// Token: 0x0400BC6D RID: 48237
				public static LocString DESC = "This {0} always feels great after a bubble bath and a good long cry.";
			}

			// Token: 0x02002D8B RID: 11659
			public class TRAVALDO
			{
				// Token: 0x0400BC6E RID: 48238
				public static LocString NAME = "Travaldo";

				// Token: 0x0400BC6F RID: 48239
				public static LocString DESC = "A {0}'s monotonous voice and lack of facial expression makes it impossible for others to tell when he's messing with them.";
			}

			// Token: 0x02002D8C RID: 11660
			public class HAROLD
			{
				// Token: 0x0400BC70 RID: 48240
				public static LocString NAME = "Harold";

				// Token: 0x0400BC71 RID: 48241
				public static LocString DESC = "Get a bunch of {0}s together in a room, and you'll have... a bunch of {0}s together in a room.";
			}

			// Token: 0x02002D8D RID: 11661
			public class MAX
			{
				// Token: 0x0400BC72 RID: 48242
				public static LocString NAME = "Max";

				// Token: 0x0400BC73 RID: 48243
				public static LocString DESC = "At any given moment a {0} is viscerally reliving ten different humiliating memories.";
			}

			// Token: 0x02002D8E RID: 11662
			public class ROWAN
			{
				// Token: 0x0400BC74 RID: 48244
				public static LocString NAME = "Rowan";

				// Token: 0x0400BC75 RID: 48245
				public static LocString DESC = "{0}s have exceptionally large hearts and express their emotions most efficiently by yelling.";
			}

			// Token: 0x02002D8F RID: 11663
			public class OTTO
			{
				// Token: 0x0400BC76 RID: 48246
				public static LocString NAME = "Otto";

				// Token: 0x0400BC77 RID: 48247
				public static LocString DESC = "{0}s always insult people by accident and generally exist in a perpetual state of deep regret.";
			}

			// Token: 0x02002D90 RID: 11664
			public class TURNER
			{
				// Token: 0x0400BC78 RID: 48248
				public static LocString NAME = "Turner";

				// Token: 0x0400BC79 RID: 48249
				public static LocString DESC = "This {0} is paralyzed by the knowledge that others have memories and perceptions of them they can't control.";
			}

			// Token: 0x02002D91 RID: 11665
			public class NIKOLA
			{
				// Token: 0x0400BC7A RID: 48250
				public static LocString NAME = "Nikola";

				// Token: 0x0400BC7B RID: 48251
				public static LocString DESC = "This {0} once claimed he could build a laser so powerful it would rip the colony in half. No one asked him to prove it.";
			}

			// Token: 0x02002D92 RID: 11666
			public class MEEP
			{
				// Token: 0x0400BC7C RID: 48252
				public static LocString NAME = "Meep";

				// Token: 0x0400BC7D RID: 48253
				public static LocString DESC = "{0}s have a face only a two tonne Printing Pod could love.";
			}

			// Token: 0x02002D93 RID: 11667
			public class ARI
			{
				// Token: 0x0400BC7E RID: 48254
				public static LocString NAME = "Ari";

				// Token: 0x0400BC7F RID: 48255
				public static LocString DESC = "{0}s tend to space out from time to time, but they always pay attention when it counts.";
			}

			// Token: 0x02002D94 RID: 11668
			public class JEAN
			{
				// Token: 0x0400BC80 RID: 48256
				public static LocString NAME = "Jean";

				// Token: 0x0400BC81 RID: 48257
				public static LocString DESC = "Just because {0}s are a little slow doesn't mean they can't suffer from soul-crushing existential crises.";
			}

			// Token: 0x02002D95 RID: 11669
			public class CAMILLE
			{
				// Token: 0x0400BC82 RID: 48258
				public static LocString NAME = "Camille";

				// Token: 0x0400BC83 RID: 48259
				public static LocString DESC = "This {0} loves anything that makes her feel nostalgic, including things that haven't aged well.";
			}

			// Token: 0x02002D96 RID: 11670
			public class ASHKAN
			{
				// Token: 0x0400BC84 RID: 48260
				public static LocString NAME = "Ashkan";

				// Token: 0x0400BC85 RID: 48261
				public static LocString DESC = "{0}s have what can only be described as a \"seriously infectious giggle\".";
			}

			// Token: 0x02002D97 RID: 11671
			public class STEVE
			{
				// Token: 0x0400BC86 RID: 48262
				public static LocString NAME = "Steve";

				// Token: 0x0400BC87 RID: 48263
				public static LocString DESC = "This {0} is convinced that he has psychic powers. And he knows exactly what his friends think about that.";
			}

			// Token: 0x02002D98 RID: 11672
			public class AMARI
			{
				// Token: 0x0400BC88 RID: 48264
				public static LocString NAME = "Amari";

				// Token: 0x0400BC89 RID: 48265
				public static LocString DESC = "{0}s likes to keep the peace. Ironically, they're a riot at parties.";
			}

			// Token: 0x02002D99 RID: 11673
			public class PEI
			{
				// Token: 0x0400BC8A RID: 48266
				public static LocString NAME = "Pei";

				// Token: 0x0400BC8B RID: 48267
				public static LocString DESC = "Every {0} spends at least half the day pretending that they remember what they came into this room for.";
			}

			// Token: 0x02002D9A RID: 11674
			public class QUINN
			{
				// Token: 0x0400BC8C RID: 48268
				public static LocString NAME = "Quinn";

				// Token: 0x0400BC8D RID: 48269
				public static LocString DESC = "This {0}'s favorite genre of music is \"festive power ballad\".";
			}

			// Token: 0x02002D9B RID: 11675
			public class JORGE
			{
				// Token: 0x0400BC8E RID: 48270
				public static LocString NAME = "Jorge";

				// Token: 0x0400BC8F RID: 48271
				public static LocString DESC = "{0} loves his new colony, even if their collective body odor makes his eyes water.";
			}
		}

		// Token: 0x02001E00 RID: 7680
		public class NEEDS
		{
			// Token: 0x02002D9C RID: 11676
			public class DECOR
			{
				// Token: 0x0400BC90 RID: 48272
				public static LocString NAME = "Decor Expectation";

				// Token: 0x0400BC91 RID: 48273
				public static LocString PROFESSION_NAME = "Critic";

				// Token: 0x0400BC92 RID: 48274
				public static LocString OBSERVED_DECOR = "Current Surroundings";

				// Token: 0x0400BC93 RID: 48275
				public static LocString EXPECTATION_TOOLTIP = string.Concat(new string[]
				{
					"Most objects have ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values that alter Duplicants' opinions of their surroundings.\nThis Duplicant desires ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values of <b>{0}</b> or higher, and becomes ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" in areas with lower ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400BC94 RID: 48276
				public static LocString EXPECTATION_MOD_NAME = "Job Tier Request";
			}

			// Token: 0x02002D9D RID: 11677
			public class FOOD_QUALITY
			{
				// Token: 0x0400BC95 RID: 48277
				public static LocString NAME = "Food Quality";

				// Token: 0x0400BC96 RID: 48278
				public static LocString PROFESSION_NAME = "Gourmet";

				// Token: 0x0400BC97 RID: 48279
				public static LocString EXPECTATION_TOOLTIP = string.Concat(new string[]
				{
					"Each Duplicant has a minimum quality of ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" they'll tolerate eating.\nThis Duplicant desires <b>Tier {0}<b> or better ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					", and becomes ",
					UI.PRE_KEYWORD,
					"Stressed",
					UI.PST_KEYWORD,
					" when they eat meals of lower quality."
				});

				// Token: 0x0400BC98 RID: 48280
				public static LocString BAD_FOOD_MOD = "Food Quality";

				// Token: 0x0400BC99 RID: 48281
				public static LocString NORMAL_FOOD_MOD = "Food Quality";

				// Token: 0x0400BC9A RID: 48282
				public static LocString GOOD_FOOD_MOD = "Food Quality";

				// Token: 0x0400BC9B RID: 48283
				public static LocString EXPECTATION_MOD_NAME = "Job Tier Request";

				// Token: 0x0400BC9C RID: 48284
				public static LocString ADJECTIVE_FORMAT_POSITIVE = "{0} [{1}]";

				// Token: 0x0400BC9D RID: 48285
				public static LocString ADJECTIVE_FORMAT_NEGATIVE = "{0} [{1}]";

				// Token: 0x0400BC9E RID: 48286
				public static LocString FOODQUALITY = "\nFood Quality Score of {0}";

				// Token: 0x0400BC9F RID: 48287
				public static LocString FOODQUALITY_EXPECTATION = string.Concat(new string[]
				{
					"\nThis Duplicant is content to eat ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					" with a ",
					UI.PRE_KEYWORD,
					"Food Quality",
					UI.PST_KEYWORD,
					" of <b>{0}</b> or higher"
				});

				// Token: 0x0400BCA0 RID: 48288
				public static int ADJECTIVE_INDEX_OFFSET = -1;

				// Token: 0x020031E1 RID: 12769
				public class ADJECTIVES
				{
					// Token: 0x0400C73B RID: 51003
					public static LocString MINUS_1 = "Grisly";

					// Token: 0x0400C73C RID: 51004
					public static LocString ZERO = "Terrible";

					// Token: 0x0400C73D RID: 51005
					public static LocString PLUS_1 = "Poor";

					// Token: 0x0400C73E RID: 51006
					public static LocString PLUS_2 = "Standard";

					// Token: 0x0400C73F RID: 51007
					public static LocString PLUS_3 = "Good";

					// Token: 0x0400C740 RID: 51008
					public static LocString PLUS_4 = "Great";

					// Token: 0x0400C741 RID: 51009
					public static LocString PLUS_5 = "Superb";

					// Token: 0x0400C742 RID: 51010
					public static LocString PLUS_6 = "Ambrosial";
				}
			}

			// Token: 0x02002D9E RID: 11678
			public class QUALITYOFLIFE
			{
				// Token: 0x0400BCA1 RID: 48289
				public static LocString NAME = "Morale Requirements";

				// Token: 0x0400BCA2 RID: 48290
				public static LocString EXPECTATION_TOOLTIP = string.Concat(new string[]
				{
					"The more responsibilities and stressors a Duplicant has, the more they will desire additional leisure time and improved amenities.\n\nFailing to keep a Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" at or above their ",
					UI.PRE_KEYWORD,
					"Morale Need",
					UI.PST_KEYWORD,
					" means they will not be able to unwind, causing them ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" over time."
				});

				// Token: 0x0400BCA3 RID: 48291
				public static LocString EXPECTATION_MOD_NAME = "Skills Learned";

				// Token: 0x0400BCA4 RID: 48292
				public static LocString APTITUDE_SKILLS_MOD_NAME = "Interested Skills Learned";

				// Token: 0x0400BCA5 RID: 48293
				public static LocString TOTAL_SKILL_POINTS = "Total Skill Points: {0}";

				// Token: 0x0400BCA6 RID: 48294
				public static LocString GOOD_MODIFIER = "High Morale";

				// Token: 0x0400BCA7 RID: 48295
				public static LocString NEUTRAL_MODIFIER = "Sufficient Morale";

				// Token: 0x0400BCA8 RID: 48296
				public static LocString BAD_MODIFIER = "Low Morale";
			}

			// Token: 0x02002D9F RID: 11679
			public class NOISE
			{
				// Token: 0x0400BCA9 RID: 48297
				public static LocString NAME = "Noise Expectation";
			}
		}

		// Token: 0x02001E01 RID: 7681
		public class ATTRIBUTES
		{
			// Token: 0x040089EF RID: 35311
			public static LocString VALUE = "{0}: {1}";

			// Token: 0x040089F0 RID: 35312
			public static LocString TOTAL_VALUE = "\n\nTotal <b>{1}</b>: {0}";

			// Token: 0x040089F1 RID: 35313
			public static LocString BASE_VALUE = "\nBase: {0}";

			// Token: 0x040089F2 RID: 35314
			public static LocString MODIFIER_ENTRY = "\n    • {0}: {1}";

			// Token: 0x040089F3 RID: 35315
			public static LocString UNPROFESSIONAL_NAME = "Lump";

			// Token: 0x040089F4 RID: 35316
			public static LocString UNPROFESSIONAL_DESC = "This Duplicant has no discernible skills";

			// Token: 0x040089F5 RID: 35317
			public static LocString PROFESSION_DESC = string.Concat(new string[]
			{
				"Expertise is determined by a Duplicant's highest ",
				UI.PRE_KEYWORD,
				"Attribute",
				UI.PST_KEYWORD,
				"\n\nDuplicants develop higher expectations as their Expertise level increases"
			});

			// Token: 0x040089F6 RID: 35318
			public static LocString STORED_VALUE = "Stored value";

			// Token: 0x02002DA0 RID: 11680
			public class CONSTRUCTION
			{
				// Token: 0x0400BCAA RID: 48298
				public static LocString NAME = "Construction";

				// Token: 0x0400BCAB RID: 48299
				public static LocString DESC = "Determines a Duplicant's building Speed.";

				// Token: 0x0400BCAC RID: 48300
				public static LocString SPEEDMODIFIER = "{0} Construction Speed";
			}

			// Token: 0x02002DA1 RID: 11681
			public class SCALDINGTHRESHOLD
			{
				// Token: 0x0400BCAD RID: 48301
				public static LocString NAME = "Scalding Threshold";

				// Token: 0x0400BCAE RID: 48302
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines the ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" at which a Duplicant will get burned."
				});
			}

			// Token: 0x02002DA2 RID: 11682
			public class DIGGING
			{
				// Token: 0x0400BCAF RID: 48303
				public static LocString NAME = "Excavation";

				// Token: 0x0400BCB0 RID: 48304
				public static LocString DESC = "Determines a Duplicant's mining speed.";

				// Token: 0x0400BCB1 RID: 48305
				public static LocString SPEEDMODIFIER = "{0} Digging Speed";

				// Token: 0x0400BCB2 RID: 48306
				public static LocString ATTACK_MODIFIER = "{0} Attack Damage";
			}

			// Token: 0x02002DA3 RID: 11683
			public class MACHINERY
			{
				// Token: 0x0400BCB3 RID: 48307
				public static LocString NAME = "Machinery";

				// Token: 0x0400BCB4 RID: 48308
				public static LocString DESC = "Determines how quickly a Duplicant uses machines.";

				// Token: 0x0400BCB5 RID: 48309
				public static LocString SPEEDMODIFIER = "{0} Machine Operation Speed";

				// Token: 0x0400BCB6 RID: 48310
				public static LocString TINKER_EFFECT_MODIFIER = "{0} Engie's Tune-Up Effect Duration";
			}

			// Token: 0x02002DA4 RID: 11684
			public class LIFESUPPORT
			{
				// Token: 0x0400BCB7 RID: 48311
				public static LocString NAME = "Life Support";

				// Token: 0x0400BCB8 RID: 48312
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how efficiently a Duplicant maintains ",
					BUILDINGS.PREFABS.ALGAEHABITAT.NAME,
					"s, ",
					BUILDINGS.PREFABS.AIRFILTER.NAME,
					"s, and ",
					BUILDINGS.PREFABS.WATERPURIFIER.NAME,
					"s"
				});
			}

			// Token: 0x02002DA5 RID: 11685
			public class TOGGLE
			{
				// Token: 0x0400BCB9 RID: 48313
				public static LocString NAME = "Toggle";

				// Token: 0x0400BCBA RID: 48314
				public static LocString DESC = "Determines how efficiently a Duplicant tunes machinery, flips switches, and sets sensors.";
			}

			// Token: 0x02002DA6 RID: 11686
			public class ATHLETICS
			{
				// Token: 0x0400BCBB RID: 48315
				public static LocString NAME = "Athletics";

				// Token: 0x0400BCBC RID: 48316
				public static LocString DESC = "Determines a Duplicant's default runspeed.";

				// Token: 0x0400BCBD RID: 48317
				public static LocString SPEEDMODIFIER = "{0} Runspeed";
			}

			// Token: 0x02002DA7 RID: 11687
			public class TRANSITTUBETRAVELSPEED
			{
				// Token: 0x0400BCBE RID: 48318
				public static LocString NAME = "Transit Speed";

				// Token: 0x0400BCBF RID: 48319
				public static LocString DESC = "Determines a Duplicant's default " + BUILDINGS.PREFABS.TRAVELTUBE.NAME + " travel speed.";

				// Token: 0x0400BCC0 RID: 48320
				public static LocString SPEEDMODIFIER = "{0} Transit Tube Travel Speed";
			}

			// Token: 0x02002DA8 RID: 11688
			public class DOCTOREDLEVEL
			{
				// Token: 0x0400BCC1 RID: 48321
				public static LocString NAME = UI.FormatAsLink("Treatment Received", "MEDICINE") + " Effect";

				// Token: 0x0400BCC2 RID: 48322
				public static LocString DESC = string.Concat(new string[]
				{
					"Duplicants who receive medical care while in a ",
					BUILDINGS.PREFABS.DOCTORSTATION.NAME,
					" or ",
					BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME,
					" will gain the ",
					UI.PRE_KEYWORD,
					"Treatment Received",
					UI.PST_KEYWORD,
					" effect\n\nThis effect reduces the severity of ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" symptoms"
				});
			}

			// Token: 0x02002DA9 RID: 11689
			public class SNEEZYNESS
			{
				// Token: 0x0400BCC3 RID: 48323
				public static LocString NAME = "Sneeziness";

				// Token: 0x0400BCC4 RID: 48324
				public static LocString DESC = "Determines how frequently a Duplicant sneezes.";
			}

			// Token: 0x02002DAA RID: 11690
			public class GERMRESISTANCE
			{
				// Token: 0x0400BCC5 RID: 48325
				public static LocString NAME = "Germ Resistance";

				// Token: 0x0400BCC6 RID: 48326
				public static LocString DESC = string.Concat(new string[]
				{
					"Duplicants with a higher ",
					UI.PRE_KEYWORD,
					"Germ Resistance",
					UI.PST_KEYWORD,
					" rating are less likely to contract germ-based ",
					UI.PRE_KEYWORD,
					"Diseases",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x020031E2 RID: 12770
				public class MODIFIER_DESCRIPTORS
				{
					// Token: 0x0400C743 RID: 51011
					public static LocString NEGATIVE_LARGE = "{0} (Large Loss)";

					// Token: 0x0400C744 RID: 51012
					public static LocString NEGATIVE_MEDIUM = "{0} (Medium Loss)";

					// Token: 0x0400C745 RID: 51013
					public static LocString NEGATIVE_SMALL = "{0} (Small Loss)";

					// Token: 0x0400C746 RID: 51014
					public static LocString NONE = "No Effect";

					// Token: 0x0400C747 RID: 51015
					public static LocString POSITIVE_SMALL = "{0} (Small Boost)";

					// Token: 0x0400C748 RID: 51016
					public static LocString POSITIVE_MEDIUM = "{0} (Medium Boost)";

					// Token: 0x0400C749 RID: 51017
					public static LocString POSITIVE_LARGE = "{0} (Large Boost)";
				}
			}

			// Token: 0x02002DAB RID: 11691
			public class LEARNING
			{
				// Token: 0x0400BCC7 RID: 48327
				public static LocString NAME = "Science";

				// Token: 0x0400BCC8 RID: 48328
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant conducts ",
					UI.PRE_KEYWORD,
					"Research",
					UI.PST_KEYWORD,
					" and gains ",
					UI.PRE_KEYWORD,
					"Skill Points",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400BCC9 RID: 48329
				public static LocString SPEEDMODIFIER = "{0} Skill Leveling";

				// Token: 0x0400BCCA RID: 48330
				public static LocString RESEARCHSPEED = "{0} Research Speed";

				// Token: 0x0400BCCB RID: 48331
				public static LocString GEOTUNER_SPEED_MODIFIER = "{0} Geotuning Speed";
			}

			// Token: 0x02002DAC RID: 11692
			public class COOKING
			{
				// Token: 0x0400BCCC RID: 48332
				public static LocString NAME = "Cuisine";

				// Token: 0x0400BCCD RID: 48333
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant prepares ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400BCCE RID: 48334
				public static LocString SPEEDMODIFIER = "{0} Cooking Speed";
			}

			// Token: 0x02002DAD RID: 11693
			public class HAPPINESSDELTA
			{
				// Token: 0x0400BCCF RID: 48335
				public static LocString NAME = "Happiness";

				// Token: 0x0400BCD0 RID: 48336
				public static LocString DESC = "Contented " + UI.FormatAsLink("Critters", "CREATURES") + " produce usable materials with increased frequency.";
			}

			// Token: 0x02002DAE RID: 11694
			public class RADIATIONBALANCEDELTA
			{
				// Token: 0x0400BCD1 RID: 48337
				public static LocString NAME = "Absorbed Radiation Dose";

				// Token: 0x0400BCD2 RID: 48338
				public static LocString TOOLTIP = string.Concat(new string[]
				{
					"Duplicants accumulate Rads in areas with ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" and recover at very slow rates\n\nOpen the ",
					UI.FormatAsOverlay("Radiation Overlay", global::Action.Overlay15),
					" to view current ",
					UI.PRE_KEYWORD,
					"Rad",
					UI.PST_KEYWORD,
					" readings"
				});
			}

			// Token: 0x02002DAF RID: 11695
			public class INSULATION
			{
				// Token: 0x0400BCD3 RID: 48339
				public static LocString NAME = "Insulation";

				// Token: 0x0400BCD4 RID: 48340
				public static LocString DESC = string.Concat(new string[]
				{
					"Highly ",
					UI.PRE_KEYWORD,
					"Insulated",
					UI.PST_KEYWORD,
					" Duplicants retain body heat easily, while low ",
					UI.PRE_KEYWORD,
					"Insulation",
					UI.PST_KEYWORD,
					" Duplicants are easier to keep cool."
				});

				// Token: 0x0400BCD5 RID: 48341
				public static LocString SPEEDMODIFIER = "{0} Temperature Retention";
			}

			// Token: 0x02002DB0 RID: 11696
			public class STRENGTH
			{
				// Token: 0x0400BCD6 RID: 48342
				public static LocString NAME = "Strength";

				// Token: 0x0400BCD7 RID: 48343
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines a Duplicant's ",
					UI.PRE_KEYWORD,
					"Carrying Capacity",
					UI.PST_KEYWORD,
					" and cleaning speed."
				});

				// Token: 0x0400BCD8 RID: 48344
				public static LocString CARRYMODIFIER = "{0} " + DUPLICANTS.ATTRIBUTES.CARRYAMOUNT.NAME;

				// Token: 0x0400BCD9 RID: 48345
				public static LocString SPEEDMODIFIER = "{0} Tidying Speed";
			}

			// Token: 0x02002DB1 RID: 11697
			public class CARING
			{
				// Token: 0x0400BCDA RID: 48346
				public static LocString NAME = "Medicine";

				// Token: 0x0400BCDB RID: 48347
				public static LocString DESC = "Determines a Duplicant's ability to care for sick peers.";

				// Token: 0x0400BCDC RID: 48348
				public static LocString SPEEDMODIFIER = "{0} Treatment Speed";

				// Token: 0x0400BCDD RID: 48349
				public static LocString FABRICATE_SPEEDMODIFIER = "{0} Medicine Fabrication Speed";
			}

			// Token: 0x02002DB2 RID: 11698
			public class IMMUNITY
			{
				// Token: 0x0400BCDE RID: 48350
				public static LocString NAME = "Immunity";

				// Token: 0x0400BCDF RID: 48351
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines a Duplicant's ",
					UI.PRE_KEYWORD,
					"Disease",
					UI.PST_KEYWORD,
					" susceptibility and recovery time."
				});

				// Token: 0x0400BCE0 RID: 48352
				public static LocString BOOST_MODIFIER = "{0} Immunity Regen";

				// Token: 0x0400BCE1 RID: 48353
				public static LocString BOOST_STAT = "Immunity Attribute";
			}

			// Token: 0x02002DB3 RID: 11699
			public class BOTANIST
			{
				// Token: 0x0400BCE2 RID: 48354
				public static LocString NAME = "Agriculture";

				// Token: 0x0400BCE3 RID: 48355
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly and efficiently a Duplicant cultivates ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400BCE4 RID: 48356
				public static LocString HARVEST_SPEED_MODIFIER = "{0} Harvesting Speed";

				// Token: 0x0400BCE5 RID: 48357
				public static LocString TINKER_MODIFIER = "{0} Tending Speed";

				// Token: 0x0400BCE6 RID: 48358
				public static LocString BONUS_SEEDS = "{0} Seed Chance";

				// Token: 0x0400BCE7 RID: 48359
				public static LocString TINKER_EFFECT_MODIFIER = "{0} Farmer's Touch Effect Duration";
			}

			// Token: 0x02002DB4 RID: 11700
			public class RANCHING
			{
				// Token: 0x0400BCE8 RID: 48360
				public static LocString NAME = "Husbandry";

				// Token: 0x0400BCE9 RID: 48361
				public static LocString DESC = "Determines how efficiently a Duplicant tends " + UI.FormatAsLink("Critters", "CREATURES") + ".";

				// Token: 0x0400BCEA RID: 48362
				public static LocString EFFECTMODIFIER = "{0} Groom Effect Duration";

				// Token: 0x0400BCEB RID: 48363
				public static LocString CAPTURABLESPEED = "{0} Wrangling Speed";
			}

			// Token: 0x02002DB5 RID: 11701
			public class ART
			{
				// Token: 0x0400BCEC RID: 48364
				public static LocString NAME = "Creativity";

				// Token: 0x0400BCED RID: 48365
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant produces ",
					UI.PRE_KEYWORD,
					"Artwork",
					UI.PST_KEYWORD,
					"."
				});

				// Token: 0x0400BCEE RID: 48366
				public static LocString SPEEDMODIFIER = "{0} Decorating Speed";
			}

			// Token: 0x02002DB6 RID: 11702
			public class DECOR
			{
				// Token: 0x0400BCEF RID: 48367
				public static LocString NAME = "Decor";

				// Token: 0x0400BCF0 RID: 48368
				public static LocString DESC = string.Concat(new string[]
				{
					"Affects a Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" and their opinion of their surroundings."
				});
			}

			// Token: 0x02002DB7 RID: 11703
			public class THERMALCONDUCTIVITYBARRIER
			{
				// Token: 0x0400BCF1 RID: 48369
				public static LocString NAME = "Insulation Thickness";

				// Token: 0x0400BCF2 RID: 48370
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant retains or loses body ",
					UI.PRE_KEYWORD,
					"Heat",
					UI.PST_KEYWORD,
					" in any given area.\n\nIt is the sum of a Duplicant's ",
					UI.PRE_KEYWORD,
					"Equipment",
					UI.PST_KEYWORD,
					" and their natural ",
					UI.PRE_KEYWORD,
					"Insulation",
					UI.PST_KEYWORD,
					" values."
				});
			}

			// Token: 0x02002DB8 RID: 11704
			public class DECORRADIUS
			{
				// Token: 0x0400BCF3 RID: 48371
				public static LocString NAME = "Decor Radius";

				// Token: 0x0400BCF4 RID: 48372
				public static LocString DESC = string.Concat(new string[]
				{
					"The influence range of an object's ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" value."
				});
			}

			// Token: 0x02002DB9 RID: 11705
			public class DECOREXPECTATION
			{
				// Token: 0x0400BCF5 RID: 48373
				public static LocString NAME = "Decor Morale Bonus";

				// Token: 0x0400BCF6 RID: 48374
				public static LocString DESC = string.Concat(new string[]
				{
					"A Decor Morale Bonus allows Duplicants to receive ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" boosts from lower ",
					UI.PRE_KEYWORD,
					"Decor",
					UI.PST_KEYWORD,
					" values.\n\nMaintaining high ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" will allow Duplicants to learn more ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02002DBA RID: 11706
			public class FOODEXPECTATION
			{
				// Token: 0x0400BCF7 RID: 48375
				public static LocString NAME = "Food Morale Bonus";

				// Token: 0x0400BCF8 RID: 48376
				public static LocString DESC = string.Concat(new string[]
				{
					"A Food Morale Bonus allows Duplicants to receive ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" boosts from lower quality ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					".\n\nMaintaining high ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" will allow Duplicants to learn more ",
					UI.PRE_KEYWORD,
					"Skills",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02002DBB RID: 11707
			public class QUALITYOFLIFEEXPECTATION
			{
				// Token: 0x0400BCF9 RID: 48377
				public static LocString NAME = "Morale Need";

				// Token: 0x0400BCFA RID: 48378
				public static LocString DESC = string.Concat(new string[]
				{
					"Dictates how high a Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" must be kept to prevent them from gaining ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002DBC RID: 11708
			public class HYGIENE
			{
				// Token: 0x0400BCFB RID: 48379
				public static LocString NAME = "Hygiene";

				// Token: 0x0400BCFC RID: 48380
				public static LocString DESC = "Affects a Duplicant's sense of cleanliness.";
			}

			// Token: 0x02002DBD RID: 11709
			public class CARRYAMOUNT
			{
				// Token: 0x0400BCFD RID: 48381
				public static LocString NAME = "Carrying Capacity";

				// Token: 0x0400BCFE RID: 48382
				public static LocString DESC = "Determines the maximum weight that a Duplicant can carry.";
			}

			// Token: 0x02002DBE RID: 11710
			public class SPACENAVIGATION
			{
				// Token: 0x0400BCFF RID: 48383
				public static LocString NAME = "Piloting";

				// Token: 0x0400BD00 RID: 48384
				public static LocString DESC = "Determines how long it takes a Duplicant to complete a space mission.";

				// Token: 0x0400BD01 RID: 48385
				public static LocString DLC1_DESC = "Determines how much of a speed bonus a Duplicant provides to a rocket they are piloting.";

				// Token: 0x0400BD02 RID: 48386
				public static LocString SPEED_MODIFIER = "{0} Rocket Speed";
			}

			// Token: 0x02002DBF RID: 11711
			public class QUALITYOFLIFE
			{
				// Token: 0x0400BD03 RID: 48387
				public static LocString NAME = "Morale";

				// Token: 0x0400BD04 RID: 48388
				public static LocString DESC = string.Concat(new string[]
				{
					"A Duplicant's ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" must exceed their ",
					UI.PRE_KEYWORD,
					"Morale Need",
					UI.PST_KEYWORD,
					", or they'll begin to accumulate ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					".\n\n",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" can be increased by providing Duplicants higher quality ",
					UI.PRE_KEYWORD,
					"Food",
					UI.PST_KEYWORD,
					", allotting more ",
					UI.PRE_KEYWORD,
					"Downtime",
					UI.PST_KEYWORD,
					" in\nthe colony schedule, or building better ",
					UI.PRE_KEYWORD,
					"Bathrooms",
					UI.PST_KEYWORD,
					" and ",
					UI.PRE_KEYWORD,
					"Bedrooms",
					UI.PST_KEYWORD,
					" for them to live in."
				});

				// Token: 0x0400BD05 RID: 48389
				public static LocString DESC_FORMAT = "{0} / {1}";

				// Token: 0x0400BD06 RID: 48390
				public static LocString TOOLTIP_EXPECTATION = "Total <b>Morale Need</b>: {0}\n    • Skills Learned: +{0}";

				// Token: 0x0400BD07 RID: 48391
				public static LocString TOOLTIP_EXPECTATION_OVER = "This Duplicant has sufficiently high " + UI.PRE_KEYWORD + "Morale" + UI.PST_KEYWORD;

				// Token: 0x0400BD08 RID: 48392
				public static LocString TOOLTIP_EXPECTATION_UNDER = string.Concat(new string[]
				{
					"This Duplicant's low ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" will cause ",
					UI.PRE_KEYWORD,
					"Stress",
					UI.PST_KEYWORD,
					" over time"
				});
			}

			// Token: 0x02002DC0 RID: 11712
			public class AIRCONSUMPTIONRATE
			{
				// Token: 0x0400BD09 RID: 48393
				public static LocString NAME = "Air Consumption Rate";

				// Token: 0x0400BD0A RID: 48394
				public static LocString DESC = "Air Consumption determines how much " + ELEMENTS.OXYGEN.NAME + " a Duplicant requires per minute to live.";
			}

			// Token: 0x02002DC1 RID: 11713
			public class RADIATIONRESISTANCE
			{
				// Token: 0x0400BD0B RID: 48395
				public static LocString NAME = "Radiation Resistance";

				// Token: 0x0400BD0C RID: 48396
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how easily a Duplicant repels ",
					UI.PRE_KEYWORD,
					"Radiation Sickness",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02002DC2 RID: 11714
			public class RADIATIONRECOVERY
			{
				// Token: 0x0400BD0D RID: 48397
				public static LocString NAME = "Radiation Absorption";

				// Token: 0x0400BD0E RID: 48398
				public static LocString DESC = string.Concat(new string[]
				{
					"The rate at which ",
					UI.PRE_KEYWORD,
					"Radiation",
					UI.PST_KEYWORD,
					" is neutralized within a Duplicant body."
				});
			}

			// Token: 0x02002DC3 RID: 11715
			public class STRESSDELTA
			{
				// Token: 0x0400BD0F RID: 48399
				public static LocString NAME = "Stress";

				// Token: 0x0400BD10 RID: 48400
				public static LocString DESC = "Determines how quickly a Duplicant gains or reduces " + UI.PRE_KEYWORD + "Stress" + UI.PST_KEYWORD;
			}

			// Token: 0x02002DC4 RID: 11716
			public class BREATHDELTA
			{
				// Token: 0x0400BD11 RID: 48401
				public static LocString NAME = "Breath";

				// Token: 0x0400BD12 RID: 48402
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant gains or reduces ",
					UI.PRE_KEYWORD,
					"Breath",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02002DC5 RID: 11717
			public class BLADDERDELTA
			{
				// Token: 0x0400BD13 RID: 48403
				public static LocString NAME = "Bladder";

				// Token: 0x0400BD14 RID: 48404
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant's ",
					UI.PRE_KEYWORD,
					"Bladder",
					UI.PST_KEYWORD,
					" fills or depletes."
				});
			}

			// Token: 0x02002DC6 RID: 11718
			public class CALORIESDELTA
			{
				// Token: 0x0400BD15 RID: 48405
				public static LocString NAME = "Calories";

				// Token: 0x0400BD16 RID: 48406
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines how quickly a Duplicant burns or stores ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02002DC7 RID: 11719
			public class STAMINADELTA
			{
				// Token: 0x0400BD17 RID: 48407
				public static LocString NAME = "Stamina";

				// Token: 0x0400BD18 RID: 48408
				public static LocString DESC = "";
			}

			// Token: 0x02002DC8 RID: 11720
			public class TOXICITYDELTA
			{
				// Token: 0x0400BD19 RID: 48409
				public static LocString NAME = "Toxicity";

				// Token: 0x0400BD1A RID: 48410
				public static LocString DESC = "";
			}

			// Token: 0x02002DC9 RID: 11721
			public class IMMUNELEVELDELTA
			{
				// Token: 0x0400BD1B RID: 48411
				public static LocString NAME = "Immunity";

				// Token: 0x0400BD1C RID: 48412
				public static LocString DESC = "";
			}

			// Token: 0x02002DCA RID: 11722
			public class TOILETEFFICIENCY
			{
				// Token: 0x0400BD1D RID: 48413
				public static LocString NAME = "Bathroom Use Speed";

				// Token: 0x0400BD1E RID: 48414
				public static LocString DESC = "Determines how long a Duplicant needs to do their \"business\".";

				// Token: 0x0400BD1F RID: 48415
				public static LocString SPEEDMODIFIER = "{0} Bathroom Use Speed";
			}

			// Token: 0x02002DCB RID: 11723
			public class METABOLISM
			{
				// Token: 0x0400BD20 RID: 48416
				public static LocString NAME = "Critter Metabolism";

				// Token: 0x0400BD21 RID: 48417
				public static LocString DESC = string.Concat(new string[]
				{
					"Affects the rate at which a critter burns ",
					UI.PRE_KEYWORD,
					"Calories",
					UI.PST_KEYWORD,
					"."
				});
			}

			// Token: 0x02002DCC RID: 11724
			public class ROOMTEMPERATUREPREFERENCE
			{
				// Token: 0x0400BD22 RID: 48418
				public static LocString NAME = "Temperature Preference";

				// Token: 0x0400BD23 RID: 48419
				public static LocString DESC = string.Concat(new string[]
				{
					"Determines the minimum body ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" a Duplicant prefers to maintain."
				});
			}

			// Token: 0x02002DCD RID: 11725
			public class MAXUNDERWATERTRAVELCOST
			{
				// Token: 0x0400BD24 RID: 48420
				public static LocString NAME = "Underwater Movement";

				// Token: 0x0400BD25 RID: 48421
				public static LocString DESC = "Determines a Duplicant's runspeed when submerged in " + UI.PRE_KEYWORD + "Liquid" + UI.PST_KEYWORD;
			}

			// Token: 0x02002DCE RID: 11726
			public class OVERHEATTEMPERATURE
			{
				// Token: 0x0400BD26 RID: 48422
				public static LocString NAME = "Overheat Temperature";

				// Token: 0x0400BD27 RID: 48423
				public static LocString DESC = string.Concat(new string[]
				{
					"A building at Overheat ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" will take damage and break down if not cooled"
				});
			}

			// Token: 0x02002DCF RID: 11727
			public class FATALTEMPERATURE
			{
				// Token: 0x0400BD28 RID: 48424
				public static LocString NAME = "Break Down Temperature";

				// Token: 0x0400BD29 RID: 48425
				public static LocString DESC = string.Concat(new string[]
				{
					"A building at break down ",
					UI.PRE_KEYWORD,
					"Temperature",
					UI.PST_KEYWORD,
					" will lose functionality and take damage"
				});
			}

			// Token: 0x02002DD0 RID: 11728
			public class HITPOINTSDELTA
			{
				// Token: 0x0400BD2A RID: 48426
				public static LocString NAME = UI.FormatAsLink("Health", "HEALTH");

				// Token: 0x0400BD2B RID: 48427
				public static LocString DESC = "Health regeneration is increased when another Duplicant provides medical care to the patient";
			}

			// Token: 0x02002DD1 RID: 11729
			public class DISEASECURESPEED
			{
				// Token: 0x0400BD2C RID: 48428
				public static LocString NAME = UI.FormatAsLink("Disease", "DISEASE") + " Recovery Speed Bonus";

				// Token: 0x0400BD2D RID: 48429
				public static LocString DESC = "Recovery speed bonus is increased when another Duplicant provides medical care to the patient";
			}

			// Token: 0x02002DD2 RID: 11730
			public abstract class MACHINERYSPEED
			{
				// Token: 0x0400BD2E RID: 48430
				public static LocString NAME = "Machinery Speed";

				// Token: 0x0400BD2F RID: 48431
				public static LocString DESC = "Speed Bonus";
			}

			// Token: 0x02002DD3 RID: 11731
			public abstract class GENERATOROUTPUT
			{
				// Token: 0x0400BD30 RID: 48432
				public static LocString NAME = "Power Output";
			}

			// Token: 0x02002DD4 RID: 11732
			public abstract class ROCKETBURDEN
			{
				// Token: 0x0400BD31 RID: 48433
				public static LocString NAME = "Burden";
			}

			// Token: 0x02002DD5 RID: 11733
			public abstract class ROCKETENGINEPOWER
			{
				// Token: 0x0400BD32 RID: 48434
				public static LocString NAME = "Engine Power";
			}

			// Token: 0x02002DD6 RID: 11734
			public abstract class FUELRANGEPERKILOGRAM
			{
				// Token: 0x0400BD33 RID: 48435
				public static LocString NAME = "Range";
			}

			// Token: 0x02002DD7 RID: 11735
			public abstract class HEIGHT
			{
				// Token: 0x0400BD34 RID: 48436
				public static LocString NAME = "Height";
			}

			// Token: 0x02002DD8 RID: 11736
			public class WILTTEMPRANGEMOD
			{
				// Token: 0x0400BD35 RID: 48437
				public static LocString NAME = "Viable Temperature Range";

				// Token: 0x0400BD36 RID: 48438
				public static LocString DESC = "Variance growth temperature relative to the base crop";
			}

			// Token: 0x02002DD9 RID: 11737
			public class YIELDAMOUNT
			{
				// Token: 0x0400BD37 RID: 48439
				public static LocString NAME = "Yield Amount";

				// Token: 0x0400BD38 RID: 48440
				public static LocString DESC = "Plant production relative to the base crop";
			}

			// Token: 0x02002DDA RID: 11738
			public class HARVESTTIME
			{
				// Token: 0x0400BD39 RID: 48441
				public static LocString NAME = "Harvest Duration";

				// Token: 0x0400BD3A RID: 48442
				public static LocString DESC = "Time it takes an unskilled Duplicant to harvest this plant";
			}

			// Token: 0x02002DDB RID: 11739
			public class DECORBONUS
			{
				// Token: 0x0400BD3B RID: 48443
				public static LocString NAME = "Decor Bonus";

				// Token: 0x0400BD3C RID: 48444
				public static LocString DESC = "Change in Decor value relative to the base crop";
			}

			// Token: 0x02002DDC RID: 11740
			public class MINLIGHTLUX
			{
				// Token: 0x0400BD3D RID: 48445
				public static LocString NAME = "Light";

				// Token: 0x0400BD3E RID: 48446
				public static LocString DESC = "Minimum lux this plant requires for growth";
			}

			// Token: 0x02002DDD RID: 11741
			public class FERTILIZERUSAGEMOD
			{
				// Token: 0x0400BD3F RID: 48447
				public static LocString NAME = "Fertilizer Usage";

				// Token: 0x0400BD40 RID: 48448
				public static LocString DESC = "Fertilizer and irrigation amounts this plant requires relative to the base crop";
			}

			// Token: 0x02002DDE RID: 11742
			public class MINRADIATIONTHRESHOLD
			{
				// Token: 0x0400BD41 RID: 48449
				public static LocString NAME = "Minimum Radiation";

				// Token: 0x0400BD42 RID: 48450
				public static LocString DESC = "Smallest amount of ambient Radiation required for this plant to grow";
			}

			// Token: 0x02002DDF RID: 11743
			public class MAXRADIATIONTHRESHOLD
			{
				// Token: 0x0400BD43 RID: 48451
				public static LocString NAME = "Maximum Radiation";

				// Token: 0x0400BD44 RID: 48452
				public static LocString DESC = "Largest amount of ambient Radiation this plant can tolerate";
			}
		}

		// Token: 0x02001E02 RID: 7682
		public class ROLES
		{
			// Token: 0x02002DE0 RID: 11744
			public class GROUPS
			{
				// Token: 0x0400BD45 RID: 48453
				public static LocString APTITUDE_DESCRIPTION = string.Concat(new string[]
				{
					"This Duplicant will gain <b>{1}</b> ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when learning ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" Skills"
				});

				// Token: 0x0400BD46 RID: 48454
				public static LocString APTITUDE_DESCRIPTION_CHOREGROUP = string.Concat(new string[]
				{
					"{2}\n\nThis Duplicant will gain <b>+{1}</b> ",
					UI.PRE_KEYWORD,
					"Morale",
					UI.PST_KEYWORD,
					" when learning ",
					UI.PRE_KEYWORD,
					"{0}",
					UI.PST_KEYWORD,
					" Skills"
				});

				// Token: 0x0400BD47 RID: 48455
				public static LocString SUITS = "Suit Wearing";
			}

			// Token: 0x02002DE1 RID: 11745
			public class NO_ROLE
			{
				// Token: 0x0400BD48 RID: 48456
				public static LocString NAME = UI.FormatAsLink("Unemployed", "NO_ROLE");

				// Token: 0x0400BD49 RID: 48457
				public static LocString DESCRIPTION = "No job assignment";
			}

			// Token: 0x02002DE2 RID: 11746
			public class JUNIOR_ARTIST
			{
				// Token: 0x0400BD4A RID: 48458
				public static LocString NAME = UI.FormatAsLink("Art Fundamentals", "ARTING1");

				// Token: 0x0400BD4B RID: 48459
				public static LocString DESCRIPTION = "Teaches the most basic level of art skill";
			}

			// Token: 0x02002DE3 RID: 11747
			public class ARTIST
			{
				// Token: 0x0400BD4C RID: 48460
				public static LocString NAME = UI.FormatAsLink("Aesthetic Design", "ARTING2");

				// Token: 0x0400BD4D RID: 48461
				public static LocString DESCRIPTION = "Allows moderately attractive art to be created";
			}

			// Token: 0x02002DE4 RID: 11748
			public class MASTER_ARTIST
			{
				// Token: 0x0400BD4E RID: 48462
				public static LocString NAME = UI.FormatAsLink("Masterworks", "ARTING3");

				// Token: 0x0400BD4F RID: 48463
				public static LocString DESCRIPTION = "Enables the painting and sculpting of masterpieces";
			}

			// Token: 0x02002DE5 RID: 11749
			public class JUNIOR_BUILDER
			{
				// Token: 0x0400BD50 RID: 48464
				public static LocString NAME = UI.FormatAsLink("Improved Construction I", "BUILDING1");

				// Token: 0x0400BD51 RID: 48465
				public static LocString DESCRIPTION = "Marginally improves a Duplicant's construction speeds";
			}

			// Token: 0x02002DE6 RID: 11750
			public class BUILDER
			{
				// Token: 0x0400BD52 RID: 48466
				public static LocString NAME = UI.FormatAsLink("Improved Construction II", "BUILDING2");

				// Token: 0x0400BD53 RID: 48467
				public static LocString DESCRIPTION = "Further increases a Duplicant's construction speeds";
			}

			// Token: 0x02002DE7 RID: 11751
			public class SENIOR_BUILDER
			{
				// Token: 0x0400BD54 RID: 48468
				public static LocString NAME = UI.FormatAsLink("Demolition", "BUILDING3");

				// Token: 0x0400BD55 RID: 48469
				public static LocString DESCRIPTION = "Enables a Duplicant to deconstruct Gravitas buildings";
			}

			// Token: 0x02002DE8 RID: 11752
			public class JUNIOR_RESEARCHER
			{
				// Token: 0x0400BD56 RID: 48470
				public static LocString NAME = UI.FormatAsLink("Advanced Research", "RESEARCHING1");

				// Token: 0x0400BD57 RID: 48471
				public static LocString DESCRIPTION = "Allows Duplicants to perform research using a " + BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME;
			}

			// Token: 0x02002DE9 RID: 11753
			public class RESEARCHER
			{
				// Token: 0x0400BD58 RID: 48472
				public static LocString NAME = UI.FormatAsLink("Field Research", "RESEARCHING2");

				// Token: 0x0400BD59 RID: 48473
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Duplicants can perform studies on ",
					UI.PRE_KEYWORD,
					"Geysers",
					UI.PST_KEYWORD,
					", ",
					UI.CLUSTERMAP.PLANETOID_KEYWORD,
					", and other geographical phenomena"
				});
			}

			// Token: 0x02002DEA RID: 11754
			public class SENIOR_RESEARCHER
			{
				// Token: 0x0400BD5A RID: 48474
				public static LocString NAME = UI.FormatAsLink("Astronomy", "ASTRONOMY");

				// Token: 0x0400BD5B RID: 48475
				public static LocString DESCRIPTION = "Enables Duplicants to study outer space using the " + BUILDINGS.PREFABS.CLUSTERTELESCOPE.NAME;
			}

			// Token: 0x02002DEB RID: 11755
			public class NUCLEAR_RESEARCHER
			{
				// Token: 0x0400BD5C RID: 48476
				public static LocString NAME = UI.FormatAsLink("Applied Sciences Research", "ATOMICRESEARCH");

				// Token: 0x0400BD5D RID: 48477
				public static LocString DESCRIPTION = "Enables Duplicants to study matter using the " + BUILDINGS.PREFABS.NUCLEARRESEARCHCENTER.NAME;
			}

			// Token: 0x02002DEC RID: 11756
			public class SPACE_RESEARCHER
			{
				// Token: 0x0400BD5E RID: 48478
				public static LocString NAME = UI.FormatAsLink("Data Analysis Researcher", "SPACERESEARCH");

				// Token: 0x0400BD5F RID: 48479
				public static LocString DESCRIPTION = "Enables Duplicants to conduct research using the " + BUILDINGS.PREFABS.DLC1COSMICRESEARCHCENTER.NAME;
			}

			// Token: 0x02002DED RID: 11757
			public class JUNIOR_COOK
			{
				// Token: 0x0400BD60 RID: 48480
				public static LocString NAME = UI.FormatAsLink("Grilling", "COOKING1");

				// Token: 0x0400BD61 RID: 48481
				public static LocString DESCRIPTION = "Allows Duplicants to cook using the " + BUILDINGS.PREFABS.COOKINGSTATION.NAME;
			}

			// Token: 0x02002DEE RID: 11758
			public class COOK
			{
				// Token: 0x0400BD62 RID: 48482
				public static LocString NAME = UI.FormatAsLink("Grilling II", "COOKING2");

				// Token: 0x0400BD63 RID: 48483
				public static LocString DESCRIPTION = "Improves a Duplicant's cooking speed";
			}

			// Token: 0x02002DEF RID: 11759
			public class JUNIOR_MEDIC
			{
				// Token: 0x0400BD64 RID: 48484
				public static LocString NAME = UI.FormatAsLink("Medicine Compounding", "MEDICINE1");

				// Token: 0x0400BD65 RID: 48485
				public static LocString DESCRIPTION = "Allows Duplicants to produce medicines at the " + BUILDINGS.PREFABS.APOTHECARY.NAME;
			}

			// Token: 0x02002DF0 RID: 11760
			public class MEDIC
			{
				// Token: 0x0400BD66 RID: 48486
				public static LocString NAME = UI.FormatAsLink("Bedside Manner", "MEDICINE2");

				// Token: 0x0400BD67 RID: 48487
				public static LocString DESCRIPTION = "Trains Duplicants to administer medicine at the " + BUILDINGS.PREFABS.DOCTORSTATION.NAME;
			}

			// Token: 0x02002DF1 RID: 11761
			public class SENIOR_MEDIC
			{
				// Token: 0x0400BD68 RID: 48488
				public static LocString NAME = UI.FormatAsLink("Advanced Medical Care", "MEDICINE3");

				// Token: 0x0400BD69 RID: 48489
				public static LocString DESCRIPTION = "Trains Duplicants to operate the " + BUILDINGS.PREFABS.ADVANCEDDOCTORSTATION.NAME;
			}

			// Token: 0x02002DF2 RID: 11762
			public class MACHINE_TECHNICIAN
			{
				// Token: 0x0400BD6A RID: 48490
				public static LocString NAME = UI.FormatAsLink("Improved Tinkering", "TECHNICALS1");

				// Token: 0x0400BD6B RID: 48491
				public static LocString DESCRIPTION = "Marginally improves a Duplicant's tinkering speeds";
			}

			// Token: 0x02002DF3 RID: 11763
			public class OIL_TECHNICIAN
			{
				// Token: 0x0400BD6C RID: 48492
				public static LocString NAME = UI.FormatAsLink("Oil Engineering", "OIL_TECHNICIAN");

				// Token: 0x0400BD6D RID: 48493
				public static LocString DESCRIPTION = "Allows the extraction and refinement of " + ELEMENTS.CRUDEOIL.NAME;
			}

			// Token: 0x02002DF4 RID: 11764
			public class HAULER
			{
				// Token: 0x0400BD6E RID: 48494
				public static LocString NAME = UI.FormatAsLink("Improved Carrying I", "HAULING1");

				// Token: 0x0400BD6F RID: 48495
				public static LocString DESCRIPTION = "Minorly increase a Duplicant's strength and carrying capacity";
			}

			// Token: 0x02002DF5 RID: 11765
			public class MATERIALS_MANAGER
			{
				// Token: 0x0400BD70 RID: 48496
				public static LocString NAME = UI.FormatAsLink("Improved Carrying II", "HAULING2");

				// Token: 0x0400BD71 RID: 48497
				public static LocString DESCRIPTION = "Further increases a Duplicant's strength and carrying capacity for even swifter deliveries";
			}

			// Token: 0x02002DF6 RID: 11766
			public class JUNIOR_FARMER
			{
				// Token: 0x0400BD72 RID: 48498
				public static LocString NAME = UI.FormatAsLink("Improved Farming I", "FARMING1");

				// Token: 0x0400BD73 RID: 48499
				public static LocString DESCRIPTION = "Minorly increase a Duplicant's farming skills, increasing their chances of harvesting new plant " + UI.PRE_KEYWORD + "Seeds" + UI.PST_KEYWORD;
			}

			// Token: 0x02002DF7 RID: 11767
			public class FARMER
			{
				// Token: 0x0400BD74 RID: 48500
				public static LocString NAME = UI.FormatAsLink("Crop Tending", "FARMING2");

				// Token: 0x0400BD75 RID: 48501
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Enables tending ",
					UI.PRE_KEYWORD,
					"Plants",
					UI.PST_KEYWORD,
					", which will increase their growth speed"
				});
			}

			// Token: 0x02002DF8 RID: 11768
			public class SENIOR_FARMER
			{
				// Token: 0x0400BD76 RID: 48502
				public static LocString NAME = UI.FormatAsLink("Improved Farming II", "FARMING3");

				// Token: 0x0400BD77 RID: 48503
				public static LocString DESCRIPTION = "Further increases a Duplicant's farming skills";
			}

			// Token: 0x02002DF9 RID: 11769
			public class JUNIOR_MINER
			{
				// Token: 0x0400BD78 RID: 48504
				public static LocString NAME = UI.FormatAsLink("Hard Digging", "MINING1");

				// Token: 0x0400BD79 RID: 48505
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows the excavation of ",
					UI.PRE_KEYWORD,
					ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.VERYFIRM,
					UI.PST_KEYWORD,
					" materials such as ",
					ELEMENTS.GRANITE.NAME
				});
			}

			// Token: 0x02002DFA RID: 11770
			public class MINER
			{
				// Token: 0x0400BD7A RID: 48506
				public static LocString NAME = UI.FormatAsLink("Superhard Digging", "MINING2");

				// Token: 0x0400BD7B RID: 48507
				public static LocString DESCRIPTION = "Allows the excavation of the element " + ELEMENTS.KATAIRITE.NAME;
			}

			// Token: 0x02002DFB RID: 11771
			public class SENIOR_MINER
			{
				// Token: 0x0400BD7C RID: 48508
				public static LocString NAME = UI.FormatAsLink("Super-Duperhard Digging", "MINING3");

				// Token: 0x0400BD7D RID: 48509
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows the excavation of ",
					UI.PRE_KEYWORD,
					ELEMENTS.HARDNESS.HARDNESS_DESCRIPTOR.NEARLYIMPENETRABLE,
					UI.PST_KEYWORD,
					" elements, including ",
					ELEMENTS.DIAMOND.NAME,
					" and ",
					ELEMENTS.OBSIDIAN.NAME
				});
			}

			// Token: 0x02002DFC RID: 11772
			public class MASTER_MINER
			{
				// Token: 0x0400BD7E RID: 48510
				public static LocString NAME = UI.FormatAsLink("Hazmat Digging", "MINING4");

				// Token: 0x0400BD7F RID: 48511
				public static LocString DESCRIPTION = "Allows the excavation of dangerous materials like " + ELEMENTS.CORIUM.NAME;
			}

			// Token: 0x02002DFD RID: 11773
			public class SUIT_DURABILITY
			{
				// Token: 0x0400BD80 RID: 48512
				public static LocString NAME = UI.FormatAsLink("Suit Sustainability Training", "SUITDURABILITY");

				// Token: 0x0400BD81 RID: 48513
				public static LocString DESCRIPTION = "Suits equipped by this Duplicant lose durability " + GameUtil.GetFormattedPercent(EQUIPMENT.SUITS.SUIT_DURABILITY_SKILL_BONUS * 100f, GameUtil.TimeSlice.None) + " slower.";
			}

			// Token: 0x02002DFE RID: 11774
			public class SUIT_EXPERT
			{
				// Token: 0x0400BD82 RID: 48514
				public static LocString NAME = UI.FormatAsLink("Exosuit Training", "SUITS1");

				// Token: 0x0400BD83 RID: 48515
				public static LocString DESCRIPTION = "Eliminates the runspeed loss experienced while wearing exosuits";
			}

			// Token: 0x02002DFF RID: 11775
			public class POWER_TECHNICIAN
			{
				// Token: 0x0400BD84 RID: 48516
				public static LocString NAME = UI.FormatAsLink("Electrical Engineering", "TECHNICALS2");

				// Token: 0x0400BD85 RID: 48517
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Enables generator ",
					UI.PRE_KEYWORD,
					"Tune-Up",
					UI.PST_KEYWORD,
					", which will temporarily provide improved ",
					UI.PRE_KEYWORD,
					"Power",
					UI.PST_KEYWORD,
					" output"
				});
			}

			// Token: 0x02002E00 RID: 11776
			public class MECHATRONIC_ENGINEER
			{
				// Token: 0x0400BD86 RID: 48518
				public static LocString NAME = UI.FormatAsLink("Mechatronics Engineering", "ENGINEERING1");

				// Token: 0x0400BD87 RID: 48519
				public static LocString DESCRIPTION = "Allows the construction and maintenance of " + BUILDINGS.PREFABS.SOLIDCONDUIT.NAME + " systems";
			}

			// Token: 0x02002E01 RID: 11777
			public class HANDYMAN
			{
				// Token: 0x0400BD88 RID: 48520
				public static LocString NAME = UI.FormatAsLink("Improved Strength", "BASEKEEPING1");

				// Token: 0x0400BD89 RID: 48521
				public static LocString DESCRIPTION = "Minorly improves a Duplicant's physical strength";
			}

			// Token: 0x02002E02 RID: 11778
			public class PLUMBER
			{
				// Token: 0x0400BD8A RID: 48522
				public static LocString NAME = UI.FormatAsLink("Plumbing", "BASEKEEPING2");

				// Token: 0x0400BD8B RID: 48523
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows a Duplicant to empty ",
					UI.PRE_KEYWORD,
					"Pipes",
					UI.PST_KEYWORD,
					" without making a mess"
				});
			}

			// Token: 0x02002E03 RID: 11779
			public class PYROTECHNIC
			{
				// Token: 0x0400BD8C RID: 48524
				public static LocString NAME = UI.FormatAsLink("Pyrotechnics", "PYROTECHNICS");

				// Token: 0x0400BD8D RID: 48525
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Allows a Duplicant to make ",
					UI.PRE_KEYWORD,
					"Blastshot",
					UI.PST_KEYWORD,
					" for the ",
					UI.PRE_KEYWORD,
					"Meteor Blaster",
					UI.PST_KEYWORD
				});
			}

			// Token: 0x02002E04 RID: 11780
			public class RANCHER
			{
				// Token: 0x0400BD8E RID: 48526
				public static LocString NAME = UI.FormatAsLink("Critter Ranching I", "RANCHING1");

				// Token: 0x0400BD8F RID: 48527
				public static LocString DESCRIPTION = "Allows a Duplicant to handle and care for " + UI.FormatAsLink("Critters", "CREATURES");
			}

			// Token: 0x02002E05 RID: 11781
			public class SENIOR_RANCHER
			{
				// Token: 0x0400BD90 RID: 48528
				public static LocString NAME = UI.FormatAsLink("Critter Ranching II", "RANCHING2");

				// Token: 0x0400BD91 RID: 48529
				public static LocString DESCRIPTION = string.Concat(new string[]
				{
					"Improves a Duplicant's ",
					UI.PRE_KEYWORD,
					"Ranching",
					UI.PST_KEYWORD,
					" skills"
				});
			}

			// Token: 0x02002E06 RID: 11782
			public class ASTRONAUTTRAINEE
			{
				// Token: 0x0400BD92 RID: 48530
				public static LocString NAME = UI.FormatAsLink("Rocket Piloting", "ASTRONAUTING1");

				// Token: 0x0400BD93 RID: 48531
				public static LocString DESCRIPTION = "Allows a Duplicant to operate a " + BUILDINGS.PREFABS.COMMANDMODULE.NAME + " to pilot a rocket ship";
			}

			// Token: 0x02002E07 RID: 11783
			public class ASTRONAUT
			{
				// Token: 0x0400BD94 RID: 48532
				public static LocString NAME = UI.FormatAsLink("Rocket Navigation", "ASTRONAUTING2");

				// Token: 0x0400BD95 RID: 48533
				public static LocString DESCRIPTION = "Improves the speed that space missions are completed";
			}

			// Token: 0x02002E08 RID: 11784
			public class ROCKETPILOT
			{
				// Token: 0x0400BD96 RID: 48534
				public static LocString NAME = UI.FormatAsLink("Rocket Piloting", "ROCKETPILOTING1");

				// Token: 0x0400BD97 RID: 48535
				public static LocString DESCRIPTION = "Allows a Duplicant to operate a " + BUILDINGS.PREFABS.ROCKETCONTROLSTATION.NAME + " and pilot rockets";
			}

			// Token: 0x02002E09 RID: 11785
			public class SENIOR_ROCKETPILOT
			{
				// Token: 0x0400BD98 RID: 48536
				public static LocString NAME = UI.FormatAsLink("Rocket Piloting II", "ROCKETPILOTING2");

				// Token: 0x0400BD99 RID: 48537
				public static LocString DESCRIPTION = "Allows Duplicants to pilot rockets at faster speeds";
			}

			// Token: 0x02002E0A RID: 11786
			public class USELESSSKILL
			{
				// Token: 0x0400BD9A RID: 48538
				public static LocString NAME = "W.I.P. Skill";

				// Token: 0x0400BD9B RID: 48539
				public static LocString DESCRIPTION = "This skill doesn't really do anything right now.";
			}
		}

		// Token: 0x02001E03 RID: 7683
		public class THOUGHTS
		{
			// Token: 0x02002E0B RID: 11787
			public class STARVING
			{
				// Token: 0x0400BD9C RID: 48540
				public static LocString TOOLTIP = "Starving";
			}

			// Token: 0x02002E0C RID: 11788
			public class HOT
			{
				// Token: 0x0400BD9D RID: 48541
				public static LocString TOOLTIP = "Hot";
			}

			// Token: 0x02002E0D RID: 11789
			public class COLD
			{
				// Token: 0x0400BD9E RID: 48542
				public static LocString TOOLTIP = "Cold";
			}

			// Token: 0x02002E0E RID: 11790
			public class BREAKBLADDER
			{
				// Token: 0x0400BD9F RID: 48543
				public static LocString TOOLTIP = "Washroom Break";
			}

			// Token: 0x02002E0F RID: 11791
			public class FULLBLADDER
			{
				// Token: 0x0400BDA0 RID: 48544
				public static LocString TOOLTIP = "Full Bladder";
			}

			// Token: 0x02002E10 RID: 11792
			public class HAPPY
			{
				// Token: 0x0400BDA1 RID: 48545
				public static LocString TOOLTIP = "Happy";
			}

			// Token: 0x02002E11 RID: 11793
			public class UNHAPPY
			{
				// Token: 0x0400BDA2 RID: 48546
				public static LocString TOOLTIP = "Unhappy";
			}

			// Token: 0x02002E12 RID: 11794
			public class POORDECOR
			{
				// Token: 0x0400BDA3 RID: 48547
				public static LocString TOOLTIP = "Poor Decor";
			}

			// Token: 0x02002E13 RID: 11795
			public class POOR_FOOD_QUALITY
			{
				// Token: 0x0400BDA4 RID: 48548
				public static LocString TOOLTIP = "Lousy Meal";
			}

			// Token: 0x02002E14 RID: 11796
			public class GOOD_FOOD_QUALITY
			{
				// Token: 0x0400BDA5 RID: 48549
				public static LocString TOOLTIP = "Delicious Meal";
			}

			// Token: 0x02002E15 RID: 11797
			public class SLEEPY
			{
				// Token: 0x0400BDA6 RID: 48550
				public static LocString TOOLTIP = "Sleepy";
			}

			// Token: 0x02002E16 RID: 11798
			public class DREAMY
			{
				// Token: 0x0400BDA7 RID: 48551
				public static LocString TOOLTIP = "Dreaming";
			}

			// Token: 0x02002E17 RID: 11799
			public class SUFFOCATING
			{
				// Token: 0x0400BDA8 RID: 48552
				public static LocString TOOLTIP = "Suffocating";
			}

			// Token: 0x02002E18 RID: 11800
			public class ANGRY
			{
				// Token: 0x0400BDA9 RID: 48553
				public static LocString TOOLTIP = "Angry";
			}

			// Token: 0x02002E19 RID: 11801
			public class RAGING
			{
				// Token: 0x0400BDAA RID: 48554
				public static LocString TOOLTIP = "Raging";
			}

			// Token: 0x02002E1A RID: 11802
			public class GOTINFECTED
			{
				// Token: 0x0400BDAB RID: 48555
				public static LocString TOOLTIP = "Got Infected";
			}

			// Token: 0x02002E1B RID: 11803
			public class PUTRIDODOUR
			{
				// Token: 0x0400BDAC RID: 48556
				public static LocString TOOLTIP = "Smelled Something Putrid";
			}

			// Token: 0x02002E1C RID: 11804
			public class NOISY
			{
				// Token: 0x0400BDAD RID: 48557
				public static LocString TOOLTIP = "Loud Area";
			}

			// Token: 0x02002E1D RID: 11805
			public class NEWROLE
			{
				// Token: 0x0400BDAE RID: 48558
				public static LocString TOOLTIP = "New Skill";
			}

			// Token: 0x02002E1E RID: 11806
			public class CHATTY
			{
				// Token: 0x0400BDAF RID: 48559
				public static LocString TOOLTIP = "Greeting";
			}

			// Token: 0x02002E1F RID: 11807
			public class ENCOURAGE
			{
				// Token: 0x0400BDB0 RID: 48560
				public static LocString TOOLTIP = "Encouraging";
			}

			// Token: 0x02002E20 RID: 11808
			public class CONVERSATION
			{
				// Token: 0x0400BDB1 RID: 48561
				public static LocString TOOLTIP = "Chatting";
			}

			// Token: 0x02002E21 RID: 11809
			public class CATCHYTUNE
			{
				// Token: 0x0400BDB2 RID: 48562
				public static LocString TOOLTIP = "WHISTLING";
			}
		}
	}
}
