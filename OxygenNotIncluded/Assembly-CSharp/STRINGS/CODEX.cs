using System;

namespace STRINGS
{
	// Token: 0x02000DA5 RID: 3493
	public class CODEX
	{
		// Token: 0x02001CD3 RID: 7379
		public class CRITTERSTATUS
		{
			// Token: 0x04008356 RID: 33622
			public static LocString CRITTERSTATUS_TITLE = "Field Guide";

			// Token: 0x02002450 RID: 9296
			public class METABOLISM
			{
				// Token: 0x0400A0B7 RID: 41143
				public static LocString TITLE = "Metabolism";

				// Token: 0x02002F8D RID: 12173
				public class BODY
				{
					// Token: 0x0400C1CD RID: 49613
					public static LocString CONTAINER1 = "A critter's metabolic rate is a measure of their appetite and the materials that they excrete as a result.\n\nCritters with higher metabolism get hungry more often. Those with lower metabolism will consume less food, but this reduced caloric intake results in fewer resources being produced.\n\nThe digestive process is influenced by conditions such as domestication, mood, and whether the critter in question is a juvenile (baby) or an adult.";
				}

				// Token: 0x02002F8E RID: 12174
				public class HUNGRY
				{
					// Token: 0x0400C1CE RID: 49614
					public static LocString TITLE = "Hungry";

					// Token: 0x0400C1CF RID: 49615
					public static LocString CONTAINER1 = "Tame critters have significantly faster metabolism than wild ones, and get hungry sooner. This makes them more valuable in terms of resource production, as long as the colony is equipped to meet their dietary needs.\n\nCritters' stomachs vary in size, but they are capable of storing at least five cycles' worth of food. Their bellies begin to rumble when those internal caches drop below 90 percent. The critter will then seek out food, and will continue to eat until they feel completely full again.\n\nJuvenile critters have the slowest metabolism, although glum tame critters are a close second.";
				}

				// Token: 0x02002F8F RID: 12175
				public class STARVING
				{
					// Token: 0x0400C1D0 RID: 49616
					public static LocString TITLE = "Starving";

					// Token: 0x0400C1D1 RID: 49617
					public static LocString CONTAINER1_VANILLA = "With the exception of Morbs—which require zero calories to survive—tame critters will die after {0} cycles of consistent starvation. Wild critters do not starve to death.";

					// Token: 0x0400C1D2 RID: 49618
					public static LocString CONTAINER1_DLC1 = "With the exception of Morbs and Beetas—which require zero calories to survive—tame critters will die after {0} cycles of consistent starvation. Wild critters do not starve to death.";
				}
			}

			// Token: 0x02002451 RID: 9297
			public class MOOD
			{
				// Token: 0x0400A0B8 RID: 41144
				public static LocString TITLE = "Mood";

				// Token: 0x02002F90 RID: 12176
				public class BODY
				{
					// Token: 0x0400C1D3 RID: 49619
					public static LocString CONTAINER1 = "As with many living things, critters are susceptible to fluctuations in mood. While they are incapable of articulating their feelings verbally, these variations have observable effects on productivity and reproduction.\n\nFactors that influence a critter's mood include: grooming, wildness/tameness, habitat, overcrowding, confinement, and Brackene consumption.";
				}

				// Token: 0x02002F91 RID: 12177
				public class HAPPY
				{
					// Token: 0x0400C1D4 RID: 49620
					public static LocString TITLE = "Happy";

					// Token: 0x0400C1D5 RID: 49621
					public static LocString CONTAINER1 = "Happy, tame critters produce more usable materials and tend to lay eggs at a higher rate than glum or wild critters. Domesticated critters are less resilient than wild ones—they require more care from the colony in order to maintain a positive disposition.\n\nBabies have a higher baseline of natural joy, but produce neither resources nor eggs.\n\nDuplicants with the Critter Ranching skill have the expertise needed to domesticate and care for critters, and can also boost a critter's mood by bonding with them at a Grooming Station.\n\nCritters who drink at the Critter Fountain also enjoy a mood boost, despite the lack of nutrients available in the Brackene dispensed.\n\nBeing confined or overcrowded undermines a critter's happiness.";

					// Token: 0x0400C1D6 RID: 49622
					public static LocString SUBTITLE = "Effects";

					// Token: 0x0400C1D7 RID: 49623
					public static LocString HAPPY_METABOLISM = "    • Indirectly improves egg-laying rates";
				}

				// Token: 0x02002F92 RID: 12178
				public class GLUM
				{
					// Token: 0x0400C1D8 RID: 49624
					public static LocString TITLE = "Glum";

					// Token: 0x0400C1D9 RID: 49625
					public static LocString CONTAINER1 = "Critters can survive in subpar environments, but it takes a toll on their mood and impacts metabolism and productivity. When their happiness levels dip below zero, they become glum.\n\nWild critters are less sensitive to the effects of glumness than their tamed brethren, though they are still negatively affected by overcrowded or confined living conditions.";

					// Token: 0x0400C1DA RID: 49626
					public static LocString SUBTITLE = "Effects";

					// Token: 0x0400C1DB RID: 49627
					public static LocString GLUMWILD_METABOLISM = "    • Critter Metabolism\n";
				}

				// Token: 0x02002F93 RID: 12179
				public class HOSTILE
				{
					// Token: 0x0400C1DC RID: 49628
					public static LocString TITLE = "Hostile";

					// Token: 0x0400C1DD RID: 49629
					public static LocString CONTAINER1_VANILLA = "Most critters are non-hostile. They may attempt to defend themselves when attacked by Duplicants, though their natural passivity limits the damage caused in these instances.\n\nSome critters, however, have exceptionally strong self-preservation instincts and must be approached with extreme caution.\n\nPokeshells, for example, are not naturally hostile but are fiercely protective of their young and will attack if a Duplicant or critter wanders too close to their eggs.";

					// Token: 0x0400C1DE RID: 49630
					public static LocString CONTAINER1_DLC1 = "Most critters are non-hostile. They may attempt to defend themselves when attacked by Duplicants, though their natural passivity limits the damage caused in these instances.\n\nSome critters, however, have exceptionally strong self-preservation instincts and must be approached with extreme caution. Pokeshells, for example, are not naturally hostile but are fiercely protective of their young and will attack if a Duplicant or critter wanders too close to their eggs.\n\nThe Beeta, on the other hand, is both hostile and radioactive. While it cannot be tamed, it can be subdued through the use of CO2.";
				}

				// Token: 0x02002F94 RID: 12180
				public class CONFINED
				{
					// Token: 0x0400C1DF RID: 49631
					public static LocString TITLE = "Confined";

					// Token: 0x0400C1E0 RID: 49632
					public static LocString CONTAINER1 = "Each species has its own space requirements. Critters who find themselves in a room that they consider too small will feel confined. They will feel the same way if they become stuck in a door or tile. Critters will not reproduce while they are in this state.\n\nShove Voles are the exception to this rule: their tunneling instincts make them quite comfortable in snug spaces, and they never feel confined.";

					// Token: 0x0400C1E1 RID: 49633
					public static LocString SUBTITLE = "Effects";

					// Token: 0x0400C1E2 RID: 49634
					public static LocString CONFINED_FERTILITY = "    • Reproduction\n";

					// Token: 0x0400C1E3 RID: 49635
					public static LocString CONFINED_HAPPINESS = "    • Happiness";
				}

				// Token: 0x02002F95 RID: 12181
				public class OVERCROWDED
				{
					// Token: 0x0400C1E4 RID: 49636
					public static LocString TITLE = "Overcrowded";

					// Token: 0x0400C1E5 RID: 49637
					public static LocString CONTAINER1 = "Overcrowding occurs when a critter is in a room that's appropriately sized for its needs but feels that there are too many other critters sharing the same space. Because each species has its own space requirements, this state can vary among occupants of the same room.\n\nThis emotional state intensifies in response to the number of excess critters: adding new critters to an already overcrowded room will undermine a critter's happiness even further.";

					// Token: 0x0400C1E6 RID: 49638
					public static LocString SUBTITLE = "Effects";

					// Token: 0x0400C1E7 RID: 49639
					public static LocString OVERCROWDED_HAPPY1 = "    • Happiness\n";
				}

				// Token: 0x02002F96 RID: 12182
				public class CRAMPED
				{
					// Token: 0x0400C1E8 RID: 49640
					public static LocString TITLE = "Cramped";

					// Token: 0x0400C1E9 RID: 49641
					public static LocString CONTAINER1 = "If a critter is overcrowded—or will become overcrowded once all of the eggs in the room have hatched—they begin to feel cramped.\n\nThis causes the critter's reproductive system to pause temporarily. It will resume once all eggs have hatched or been removed from the room.";

					// Token: 0x0400C1EA RID: 49642
					public static LocString SUBTITLE = "Effects";

					// Token: 0x0400C1EB RID: 49643
					public static LocString CRAMPED_FERTILITY = "    • Reproduction";
				}
			}

			// Token: 0x02002452 RID: 9298
			public class FERTILITY
			{
				// Token: 0x0400A0B9 RID: 41145
				public static LocString TITLE = "Reproduction";

				// Token: 0x02002F97 RID: 12183
				public class BODY
				{
					// Token: 0x0400C1EC RID: 49644
					public static LocString CONTAINER1 = "Reproductive rates and methods vary among species. The majority lay eggs that must be incubated in order to hatch the next generation of critters.\n\nFactors that influence the rate of reproduction include egg care, happiness, living conditions and domestication.";
				}

				// Token: 0x02002F98 RID: 12184
				public class FERTILITYRATE
				{
					// Token: 0x0400C1ED RID: 49645
					public static LocString TITLE = "Reproduction Rate";

					// Token: 0x0400C1EE RID: 49646
					public static LocString CONTAINER1 = "Each time a critter completes their reproduction cycle (i.e. at 100 percent), it lays an egg and restarts its cycle.\n\nA critter's environment greatly impacts its base reproduction rate. When a critter is feeling cramped, it will wait until all eggs in the room have hatched or been removed before laying any of its own.\n\nCritters will also stop reproducing when they feel confined, which happens when their space is too small or they are stuck in a door or tile.\n\nMood and domestication also impact reproduction: happy critters reproduce more regularly, and happy tame critters reproduce the fastest.";
				}

				// Token: 0x02002F99 RID: 12185
				public class EGGCHANCES
				{
					// Token: 0x0400C1EF RID: 49647
					public static LocString TITLE = "Egg Chances";

					// Token: 0x0400C1F0 RID: 49648
					public static LocString CONTAINER1 = "In most cases, an egg will hatch into the same critter variant as its parent. Genetic volatility, however, means that there is a chance that it may hatch into another variant from that species.\n\nThere are many things that can alter the likelihood of a critter laying a particular type of egg.\n\nEgg chances are impacted by:\n    • Diet\n    • Body temperature\n    • Ambient gasses and elements\n    • Plants in the critters' care\n    • Variants that share the enclosure\n\nWhen a tame critter lays an egg, the resulting offspring will be born tame.";
				}

				// Token: 0x02002F9A RID: 12186
				public class INCUBATION
				{
					// Token: 0x0400C1F1 RID: 49649
					public static LocString TITLE = "Incubation";

					// Token: 0x0400C1F2 RID: 49650
					public static LocString CONTAINER1 = "A critter's incubation time is one-fifth of their total lifetime: for example, if a critter's maximum age is 100 cycles, its egg will take 20 cycles to hatch.\n\nIncubation rates can be accelerated through tender intervention by a Critter Rancher. Lullabied eggs—that is, those that have been sung to—will incubate faster and hatch sooner than eggs that have not received such tender care. Being cuddled by a Cuddle Pip also accelerates the rate of incubation.\n\nEggs can be cuddled anywhere, but can only be lullabied when placed inside an Incubator. The effects of lullabies and cuddles are cumulative.";
				}

				// Token: 0x02002F9B RID: 12187
				public class MAXAGE
				{
					// Token: 0x0400C1F3 RID: 49651
					public static LocString TITLE = "Max Age";

					// Token: 0x0400C1F4 RID: 49652
					public static LocString CONTAINER1_VANILLA = "With the exception of the Morb—which can live indefinitely if left to its own devices—critters have a fixed life expectancy. The maximum age indicates the highest number of cycles that critters will live, barring starvation or other unnatural causes of death.\n\nBabyhood, the period before a critter is mature enough to reproduce, is marked by a slower metabolism and the easy happiness of youth.\n\nMost species live for 75 to 100 cycles on average.";

					// Token: 0x0400C1F5 RID: 49653
					public static LocString CONTAINER1_DLC1 = "With the exception of the Beeta Hive and the Morb—which can live indefinitely if left to their own devices—critters have a fixed life expectancy. The maximum age indicates the highest number of cycles that critters will live, barring starvation or other unnatural causes of death.\n\nBabyhood, the period before a critter is mature enough to reproduce, is marked by a slower metabolism and the easy happiness of youth.\n\nMost species live for 75 to 100 cycles on average. The shortest-lived critter is the Beeta, whose lifespan is only five cycles long.";
				}
			}

			// Token: 0x02002453 RID: 9299
			public class DOMESTICATION
			{
				// Token: 0x0400A0BA RID: 41146
				public static LocString TITLE = "Domestication";

				// Token: 0x02002F9C RID: 12188
				public class BODY
				{
					// Token: 0x0400C1F6 RID: 49654
					public static LocString CONTAINER1 = "All critters are wild when first encountered, with the exception of babies hatched from eggs laid by domesticated adults—those will be born tame.\n\nDuring the domestication process, the critter becomes less self-reliant and develops a higher baseline of expectations regarding its environment and care. Its metabolism accelerates, resulting in an increased level of required calories.\n\nCritters can be domesticated by Duplicants with the Critter Ranching skill at the Grooming Station, and get excited when it's their turn to be fussed over.";
				}

				// Token: 0x02002F9D RID: 12189
				public class WILD
				{
					// Token: 0x0400C1F7 RID: 49655
					public static LocString TITLE = "Wild";

					// Token: 0x0400C1F8 RID: 49656
					public static LocString CONTAINER1 = "Wild critters do not require feeding by the colony's Critter Ranchers, thanks to their slower metabolism. They do, however, produce fewer materials than domesticated critters.\n\nApproaching a wild critter to trap or wrangle it is quite safe, provided that it is a non-hostile species. Attacking a critter will typically provoke a combat response.";

					// Token: 0x0400C1F9 RID: 49657
					public static LocString SUBTITLE = "Effects";

					// Token: 0x0400C1FA RID: 49658
					public static LocString WILD_METABOLISM = "    • Critter Metabolism\n";

					// Token: 0x0400C1FB RID: 49659
					public static LocString WILD_POOP = "    • Resource Production\n";
				}

				// Token: 0x02002F9E RID: 12190
				public class TAME
				{
					// Token: 0x0400C1FC RID: 49660
					public static LocString TITLE = "Tame";

					// Token: 0x0400C1FD RID: 49661
					public static LocString CONTAINER1 = "Domesticated critters produce far more resources and lay eggs at a higher frequency than wild ones. They require additional care in order to maintain the levels of happiness that maximize their utility in the colony. (Happy critters are also generally more pleasant to be around.)\n\nOnce tame, critters can access the Critter Feeder, which is unavailable to wild critters.";

					// Token: 0x0400C1FE RID: 49662
					public static LocString SUBTITLE = "Effects";

					// Token: 0x0400C1FF RID: 49663
					public static LocString TAME_HAPPINESS = "    • Happiness\n";

					// Token: 0x0400C200 RID: 49664
					public static LocString TAME_METABOLISM = "    • Critter Metabolism";
				}
			}
		}

		// Token: 0x02001CD4 RID: 7380
		public class STORY_TRAITS
		{
			// Token: 0x04008357 RID: 33623
			public static LocString CLOSE_BUTTON = "Close";

			// Token: 0x02002454 RID: 9300
			public static class MEGA_BRAIN_TANK
			{
				// Token: 0x0400A0BB RID: 41147
				public static LocString NAME = "Somnium Synthesizer";

				// Token: 0x0400A0BC RID: 41148
				public static LocString DESCRIPTION = "Power up a colossal relic from Gravitas's underground sleep lab.\n\nWhen Duplicants sleep, their minds are blissfully blank and dream-free. But under the right conditions, things could be...different.";

				// Token: 0x0400A0BD RID: 41149
				public static LocString DESCRIPTION_SHORT = "Power up a colossal relic from Gravitas's underground sleep lab.";

				// Token: 0x02002F9F RID: 12191
				public class BEGIN_POPUP
				{
					// Token: 0x0400C201 RID: 49665
					public static LocString NAME = "Story Trait: Somnium Synthesizer";

					// Token: 0x0400C202 RID: 49666
					public static LocString CODEX_NAME = "First Encounter";

					// Token: 0x0400C203 RID: 49667
					public static LocString DESCRIPTION = "I've discovered a new dream-analyzing building buried deep inside our asteroid.\n\nIt seems to contain new sleep-specific suits...could these be the key to unlocking my Duplicants' ability to dream?\n\nI've often wondered what they might be capable of, once their imaginations were awakened.";
				}

				// Token: 0x02002FA0 RID: 12192
				public class END_POPUP
				{
					// Token: 0x0400C204 RID: 49668
					public static LocString NAME = "Story Trait Complete: Somnium Synthesizer";

					// Token: 0x0400C205 RID: 49669
					public static LocString CODEX_NAME = "Challenge Completed";

					// Token: 0x0400C206 RID: 49670
					public static LocString DESCRIPTION = "Meeting the initial quota of dream content analysis has triggered a surge of electromagnetic activity that appears to be enhancing performance for Duplicants everywhere.\n\nIf my Duplicants can keep this building fuelled with Dream Journals, perhaps we will continue to reap this benefit.\n\nA small side compartment has also popped open, revealing an unfamiliar object.\n\nA keepsake, perhaps?";

					// Token: 0x0400C207 RID: 49671
					public static LocString BUTTON = "Unlock Maximum Aptitude Mode";
				}

				// Token: 0x02002FA1 RID: 12193
				public class SEEDSOFEVOLUTION
				{
					// Token: 0x0400C208 RID: 49672
					public static LocString TITLE = "A Seed is Planted";

					// Token: 0x0400C209 RID: 49673
					public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

					// Token: 0x020032D2 RID: 13010
					public class BODY
					{
						// Token: 0x0400CA06 RID: 51718
						public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B040]</smallcaps>\n\n[LOG BEGINS]\n\nThree days ago, we completed our first non-fatal Duplicant trial of Nikola's comprehensive synapse microanalysis and mirroring process. Five hours from now, Subject #901 will make history as our first human test subject.\n\nEven at the Vertex Institute, which is twice Gravitas's size, I could've spent half my career waiting for approval to advance to human trials for such an invasive process! But Director Stern is too invested in this work to let it stagnate.\n\nMy darling Bruce always said that when you're on the right path, the universe conspires to help you. He'd be so proud of the work we do here.\n\n[LOG ENDS]\n\n[LOG BEGINS]\n\nMy bio-printed multi-cerebral storage chambers (or \"mega minds\" as I've been calling them) are working! Just in time to save my job.\n\nThe Director's been getting increasingly impatient about our struggle to maintain the integrity of our growing datasets during extraction and processing. The other day, she held my report over a Bunsen burner until the flames reached her fingertips.\n\nI can only imagine how much stress she's under.\n\nThe whole world is counting on us.\n\n[LOG ENDS]\n\n[LOG BEGINS]\n\nOn a hunch, I added dream content analysis to the data and...wow. Oneirology may be scientifically \"fluffy\", but integrating subconscious narratives has produced a new type of brainmap - one with more latent potential for complex processing.\n\nIf these results are replicable, we might be on the verge of unlocking the secret to creating synthetic life forms with the capacity to evolve beyond blindly following commands.\n\nNikola says that's irrelevant for our purposes. Surely Director Stern would disagree.\n\n[LOG ENDS]\n\n[LOG BEGINS]\n\nNikola gave me a dataset to plug into the mega minds. He wouldn't say where it came from, but even if he had...nothing could have prepared me for what it contained.\n\nWhen he saw my face, he muttered something about how people should call me \"Tremors,\" not \"Nails\" and sent me on my lunch break.\n\nAll I could think about was those poor souls.\n\nDid they have souls?\n\n...do we?\n\n[LOG ENDS]\n\n[LOG BEGINS]\n\nIt's done. My adjustments to the memory transfer protocol are hardcoded into the machine.\n\nI finished just as Nikola stormed in.\n\nI may be too much of a coward to stand up for those unfortunate creatures, but with these new parameters in place...someday, they might be able to stand up for themselves.\n\n[LOG ENDS]\n------------------\n";
					}
				}
			}

			// Token: 0x02002455 RID: 9301
			public class CRITTER_MANIPULATOR
			{
				// Token: 0x0400A0BE RID: 41150
				public static LocString NAME = "Critter Flux-O-Matic";

				// Token: 0x0400A0BF RID: 41151
				public static LocString DESCRIPTION = "Explore a revolutionary genetic manipulation device designed for critters.\n\nWhether or not it was ever used on non-critter subjects is unclear. Its DNA database has been wiped clean.";

				// Token: 0x0400A0C0 RID: 41152
				public static LocString DESCRIPTION_SHORT = "Explore a revolutionary genetic manipulation device designed for critters.";

				// Token: 0x02002FA2 RID: 12194
				public class BEGIN_POPUP
				{
					// Token: 0x0400C20A RID: 49674
					public static LocString NAME = "Story Trait: Critter Flux-O-Matic";

					// Token: 0x0400C20B RID: 49675
					public static LocString CODEX_NAME = "First Encounter";

					// Token: 0x0400C20C RID: 49676
					public static LocString DESCRIPTION = "I've discovered an experiment designed to analyze the evolutionary dynamics of critter mutation.\n\nOnce it has gathered enough data, it could prove extremely useful for genetic manipulation.";
				}

				// Token: 0x02002FA3 RID: 12195
				public class END_POPUP
				{
					// Token: 0x0400C20D RID: 49677
					public static LocString NAME = "Story Trait Complete: Critter Flux-O-Matic";

					// Token: 0x0400C20E RID: 49678
					public static LocString CODEX_NAME = "Challenge Completed";

					// Token: 0x0400C20F RID: 49679
					public static LocString DESCRIPTION = "Success! Sufficient samples collected.\n\nI can now trigger genetic deviations in base morphs by sending them through the scanner.\n\nExisting variants can also be scanned, but their genetic makeup is too unstable to tolerate further manipulation.";

					// Token: 0x0400C210 RID: 49680
					public static LocString BUTTON = "Unlock Gene Manipulation Mode";
				}

				// Token: 0x02002FA4 RID: 12196
				public class UNLOCK_SPECIES_NOTIFICATION
				{
					// Token: 0x0400C211 RID: 49681
					public static LocString NAME = "New Species Scanned";

					// Token: 0x0400C212 RID: 49682
					public static LocString TOOLTIP = "The " + BUILDINGS.PREFABS.GRAVITASCREATUREMANIPULATOR.NAME + " has analyzed these critter species:\n";
				}

				// Token: 0x02002FA5 RID: 12197
				public class UNLOCK_SPECIES_POPUP
				{
					// Token: 0x0400C213 RID: 49683
					public static LocString NAME = "New Species Scanned";

					// Token: 0x0400C214 RID: 49684
					public static LocString VIEW_IN_CODEX = "Review Data";
				}

				// Token: 0x02002FA6 RID: 12198
				public class SPECIES_ENTRIES
				{
					// Token: 0x0400C215 RID: 49685
					public static LocString HATCH = "Specimen attempted to snack on the buccal smear. Review data for more information.";

					// Token: 0x0400C216 RID: 49686
					public static LocString LIGHTBUG = "This critter kept trying to befriend the reflective surfaces of the scanner's interior. Review data for more information.";

					// Token: 0x0400C217 RID: 49687
					public static LocString OILFLOATER = "Incessant wriggling made it difficult to scan this critter. Difficult, but not impossible.";

					// Token: 0x0400C218 RID: 49688
					public static LocString DRECKO = "This critter hardly seemed to notice it was being examined at all. Review data for more information.";

					// Token: 0x0400C219 RID: 49689
					public static LocString GLOM = "DNA results confirm: this species is the very definition of \"icky\".";

					// Token: 0x0400C21A RID: 49690
					public static LocString PUFT = "This critter bumped up against the building's interior repeatedly during scanning. Review data for more information.";

					// Token: 0x0400C21B RID: 49691
					public static LocString PACU = "Sample collected. Review data for more information.";

					// Token: 0x0400C21C RID: 49692
					public static LocString MOO = "Whoops! This scanner wasn't designed for critters of these proportions. This organism's genetic makeup will remain shrouded in mystery.";

					// Token: 0x0400C21D RID: 49693
					public static LocString MOLE = "This critter felt right at home in the cramped scanning bed. It can't wait to come back! ";

					// Token: 0x0400C21E RID: 49694
					public static LocString SQUIRREL = "Sample collected. Review data for more information.";

					// Token: 0x0400C21F RID: 49695
					public static LocString CRAB = "Mind the claws! Review data for more information.";

					// Token: 0x0400C220 RID: 49696
					public static LocString DIVERGENTSPECIES = "Specimen responded gently to the probative apparatus, as though being careful not to cause any damage.\n\nReview data for more information.";

					// Token: 0x0400C221 RID: 49697
					public static LocString STATERPILLAR = "Warning: The electrical charge emitted by this specimen nearly short-circuited this building.";

					// Token: 0x0400C222 RID: 49698
					public static LocString BEETA = "Strong collective consciousness detected. Review data for more information.";

					// Token: 0x0400C223 RID: 49699
					public static LocString UNKNOWN_TITLE = "ERROR: Unknown Species";

					// Token: 0x0400C224 RID: 49700
					public static LocString UNKNOWN = "This species cannot be identified due to a malfunction in the genome-parsing software.\n\nPlease note that kicking the building's exterior is unlikely to correct this issue and may result in permanent damage to the system.";
				}

				// Token: 0x02002FA7 RID: 12199
				public class SPECIES_ENTRIES_EXPANDED
				{
					// Token: 0x0400C225 RID: 49701
					public static LocString HATCH = "Specimen attempted to snack on the buccal smear. Sample is viable, though the apparatus may be somewhat mangled.\n\nAtomic force microscopy of the bite pattern reveals traces of goethite, a mineral notable for its exceptional strength.";

					// Token: 0x0400C226 RID: 49702
					public static LocString LIGHTBUG = "This critter kept trying to befriend the reflective surfaces of the scanner's interior.\n\nDuring examination, it cycled through a consistent pattern of four rapid flashes of light, a brief pause and two flashes, followed by a longer pause.\n\nIts cells appear to contain a mutated variation of oxyluciferin similar to those catalogued in bioluminescent animals.";

					// Token: 0x0400C227 RID: 49703
					public static LocString OILFLOATER = "Incessant wriggling made it difficult to scan this critter. Difficult, but not impossible.";

					// Token: 0x0400C228 RID: 49704
					public static LocString DRECKO = "This critter hardly seemed to notice it was being examined at all.\n\nThe built-in scanning electron microscope has determined that the fibers on this critter's train grow in a sort of trinity stitch pattern, reminiscent of a well-crafted sweater.\n\nThe critter's leathery skin remains cool and dry, however, likely due to an apparent lack of sweat glands.";

					// Token: 0x0400C229 RID: 49705
					public static LocString GLOM = "DNA results confirm: this species is the scientific definition of \"icky\".";

					// Token: 0x0400C22A RID: 49706
					public static LocString PUFT = "This critter bumped up against the building's interior repeatedly during scanning. Despite this, its skin remains surprisingly free of contusions.\n\nFluorescence imaging reveals extremely low neuronal activity. Was this critter asleep during analysis?";

					// Token: 0x0400C22B RID: 49707
					public static LocString PACU = "This species flopped wildly during analysis. Surfaces that came into contact with its scales now display a thin layer of viscous scum. It does not appear to be corrosive.\n\nInitiating fumigation sequence to neutralize fishy odor.";

					// Token: 0x0400C22C RID: 49708
					public static LocString MOO = "Whoops! This scanner wasn't designed for critters of these proportions. This organism's genetic makeup will remain shrouded in mystery.";

					// Token: 0x0400C22D RID: 49709
					public static LocString MOLE = "This critter felt right at home in the cramped scanning bed. It can't wait to come back! ";

					// Token: 0x0400C22E RID: 49710
					public static LocString SQUIRREL = "This species has a secondary set of inner eyelids that act as a barrier against ocular splinters.\n\nThe surfaces of these secondary eyelids are a translucent blue and display a light crosshatch texture.\n\nThis has broad implications for the critter's vision, meriting further exploration.";

					// Token: 0x0400C22F RID: 49711
					public static LocString CRAB = "This species responded to the hum of the scanner machinery by waving its pincers in gestures that seemed to mimic iconic moves of the disco dance era.\n\nIs it possible that it might have been exposed to music at some point in its evolution?";

					// Token: 0x0400C230 RID: 49712
					public static LocString DIVERGENTSPECIES = "Specimen responded gently to the probative apparatus, as though being careful not to cause any damage.\n\nIt also produced a series of deep, rhythmic vibrations during analysis. An attempt to communicate with the sensors, perhaps?";

					// Token: 0x0400C231 RID: 49713
					public static LocString STATERPILLAR = "Warning: The electrical charge emitted by this specimen nearly short-circuited this building.";

					// Token: 0x0400C232 RID: 49714
					public static LocString BEETA = "This species may not be fully sentient, but it possesses a strong collective consciousness.\n\nIt is unclear how information is communicated between members of the species. What is clear is that knowledge is being shared and passed down from one generation to another.\n\nMonitor closely.";

					// Token: 0x0400C233 RID: 49715
					public static LocString UNKNOWN_TITLE = "Unknown Species";

					// Token: 0x0400C234 RID: 49716
					public static LocString UNKNOWN = "ERROR: This species cannot be identified due to a malfunction in the genome-parsing software.\n\nPlease note that kicking the building's exterior is unlikely to correct this issue and may result in permanent damage to the system.";
				}

				// Token: 0x02002FA8 RID: 12200
				public class PARKING
				{
					// Token: 0x0400C235 RID: 49717
					public static LocString TITLE = "Parking in Lot D";

					// Token: 0x0400C236 RID: 49718
					public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

					// Token: 0x020032D3 RID: 13011
					public class BODY
					{
						// Token: 0x0400CA07 RID: 51719
						public static LocString EMAILHEADER1 = "<smallcaps>To: <b>ALL</b><alpha=#AA><size=12></size></color>\nFrom: <b>ADMIN</b><alpha=#AA><size=12> <admin@gravitas.nova></size></color></smallcaps>\n------------------\n";

						// Token: 0x0400CA08 RID: 51720
						public static LocString CONTAINER1 = "<indent=5%>Another set of masticated windshield wipers has been discovered in Parking Lot D following the Bioengineering Department's critter enclosure breach last week.\n\nEmployees are strongly encouraged to plug their vehicles in at lots A-C until further notice.\n\nPlease refrain from calling municipal animal control - all critter sightings should be reported directly to Dr. Byron.</indent>";

						// Token: 0x0400CA09 RID: 51721
						public static LocString SIGNATURE1 = "\nThank-you,\n-Admin\n<size=11>The Gravitas Facility</size>\n------------------\n";
					}
				}

				// Token: 0x02002FA9 RID: 12201
				public class WORKIVERSARY
				{
					// Token: 0x0400C237 RID: 49719
					public static LocString TITLE = "Anatomy of a Byron's Hatch";

					// Token: 0x0400C238 RID: 49720
					public static LocString SUBTITLE = " ";

					// Token: 0x020032D4 RID: 13012
					public class BODY
					{
						// Token: 0x0400CA0A RID: 51722
						public static LocString CONTAINER1 = "Happy 3rd work-iversary, Ada!\n\nI drew this to fill the space left by the cabinet that your chompy critters tore off the wall last week. Hope it's big enough!\n\nI still can't believe they can digest solid steel—you really know how to breed 'em!\n\n- Liam";
					}
				}
			}

			// Token: 0x02002456 RID: 9302
			public static class LONELYMINION
			{
				// Token: 0x0400A0C1 RID: 41153
				public static LocString NAME = "Mysterious Hermit";

				// Token: 0x0400A0C2 RID: 41154
				public static LocString DESCRIPTION = "Discover a reclusive character living in a Gravitas relic, and persuade them to join this colony.\n\nRevelations from their past could have far-reaching implications for Duplicants everywhere.\n\nEven their makeshift shelter might be of some use...";

				// Token: 0x0400A0C3 RID: 41155
				public static LocString DESCRIPTION_SHORT = "Discover a reclusive character living in a Gravitas relic, and persuade them to join this colony.";

				// Token: 0x0400A0C4 RID: 41156
				public static LocString DESCRIPTION_BUILDINGMENU = "The process of recruiting this building's lone occupant involves the completion of key tasks.";

				// Token: 0x02002FAA RID: 12202
				public class KNOCK_KNOCK
				{
					// Token: 0x0400C239 RID: 49721
					public static LocString TEXT = "Knock Knock";

					// Token: 0x0400C23A RID: 49722
					public static LocString TOOLTIP = "Approach this building and welcome its occupant";

					// Token: 0x0400C23B RID: 49723
					public static LocString CANCELTEXT = "Cancel Knock";

					// Token: 0x0400C23C RID: 49724
					public static LocString CANCEL_TOOLTIP = "Leave this building and its occupant alone for now";
				}

				// Token: 0x02002FAB RID: 12203
				public class BEGIN_POPUP
				{
					// Token: 0x0400C23D RID: 49725
					public static LocString NAME = "Story Trait: Mysterious Hermit";

					// Token: 0x0400C23E RID: 49726
					public static LocString CODEX_NAME = "First Encounter";

					// Token: 0x0400C23F RID: 49727
					public static LocString DESCRIPTION = "An unfamiliar building has been discovered in my colony. There's movement inside but whoever the inhabitant is, they seem wary of us.\n\nIf we can convince them that we mean no harm, we could very well end up with a fresh recruit <i>and</i> a useful new building.";
				}

				// Token: 0x02002FAC RID: 12204
				public class END_POPUP
				{
					// Token: 0x0400C240 RID: 49728
					public static LocString NAME = "Story Trait Complete: Mysterious Hermit";

					// Token: 0x0400C241 RID: 49729
					public static LocString CODEX_NAME = "Challenge Completed";

					// Token: 0x0400C242 RID: 49730
					public static LocString DESCRIPTION = "My sweet Duplicants' efforts paid off! Our reclusive neighbor has agreed to join the colony.\n\nThe only keepsake he insists on bringing with him is a toolbox which, while rusty, seems to hold great sentimental value.\n\nNow that he'll be living among us, his former home can be deconstructed or repurposed as storage.";

					// Token: 0x0400C243 RID: 49731
					public static LocString BUTTON = "Welcome New Duplicant!";
				}

				// Token: 0x02002FAD RID: 12205
				public class PROGRESSRESPONSE
				{
					// Token: 0x020032D5 RID: 13013
					public class STRANGERDANGER
					{
						// Token: 0x0400CA0B RID: 51723
						public static LocString NAME = "Stranger Danger";

						// Token: 0x0400CA0C RID: 51724
						public static LocString TOOLTIP = "The hermit is suspicious of all outsiders";
					}

					// Token: 0x020032D6 RID: 13014
					public class GOODINTRO
					{
						// Token: 0x0400CA0D RID: 51725
						public static LocString NAME = "Unconvinced";

						// Token: 0x0400CA0E RID: 51726
						public static LocString TOOLTIP = "The hermit is keeping an eye out for more unsolicited overtures";
					}

					// Token: 0x020032D7 RID: 13015
					public class ACQUAINTANCE
					{
						// Token: 0x0400CA0F RID: 51727
						public static LocString NAME = "Intrigued";

						// Token: 0x0400CA10 RID: 51728
						public static LocString TOOLTIP = "The hermit isn't sure why everyone is being so nice";
					}

					// Token: 0x020032D8 RID: 13016
					public class GOODNEIGHBOR
					{
						// Token: 0x0400CA11 RID: 51729
						public static LocString NAME = "Appreciative";

						// Token: 0x0400CA12 RID: 51730
						public static LocString TOOLTIP = "The hermit is developing warm, fuzzy feelings about this colony";
					}

					// Token: 0x020032D9 RID: 13017
					public class GREATNEIGHBOR
					{
						// Token: 0x0400CA13 RID: 51731
						public static LocString NAME = "Cherished";

						// Token: 0x0400CA14 RID: 51732
						public static LocString TOOLTIP = "The hermit is really starting to feel like he might belong here";
					}
				}

				// Token: 0x02002FAE RID: 12206
				public class QUESTCOMPLETE_POPUP
				{
					// Token: 0x0400C244 RID: 49732
					public static LocString NAME = "Hermit Recruitment Progress";

					// Token: 0x0400C245 RID: 49733
					public static LocString VIEW_IN_CODEX = "View File";
				}

				// Token: 0x02002FAF RID: 12207
				public class GIFTRESPONSE_POPUP
				{
					// Token: 0x020032DA RID: 13018
					public class CRAPPYFOOD
					{
						// Token: 0x0400CA15 RID: 51733
						public static LocString NAME = "The hermit hated this food";

						// Token: 0x0400CA16 RID: 51734
						public static LocString TOOLTIP = "The hermit would rather be launched straight into the sun than eat this slop.\n\nThe mailbox is ready for another delivery";
					}

					// Token: 0x020032DB RID: 13019
					public class TASTYFOOD
					{
						// Token: 0x0400CA17 RID: 51735
						public static LocString NAME = "The hermit loved this food";

						// Token: 0x0400CA18 RID: 51736
						public static LocString TOOLTIP = "Tastier than the still-warm pretzel that once fell off an unsupervised desk.\n\nThe mailbox is ready for another delivery";
					}

					// Token: 0x020032DC RID: 13020
					public class REPEATEDFOOD
					{
						// Token: 0x0400CA19 RID: 51737
						public static LocString NAME = "The hermit is unimpressed";

						// Token: 0x0400CA1A RID: 51738
						public static LocString TOOLTIP = "This meal has been offered before.\n\nThe mailbox is ready for another delivery";
					}
				}

				// Token: 0x02002FB0 RID: 12208
				public class ANCIENTPODENTRY
				{
					// Token: 0x0400C246 RID: 49734
					public static LocString TITLE = "Recovered Pod Entry #022";

					// Token: 0x0400C247 RID: 49735
					public static LocString SUBTITLE = "<smallcaps>Day: 11/80</smallcaps>\n<smallcaps>Local Time: Hour 7/9</smallcaps>";

					// Token: 0x020032DD RID: 13021
					public class BODY
					{
						// Token: 0x0400CA1B RID: 51739
						public static LocString CONTAINER1 = "<indent=%5>Notable improvement to nutrient retention: subjects who participated in the most recent meal intake displayed minimal symptoms of gastrointestinal distress.\n\nMineshaft excavation at Urvara crater resumed following resolution of tunnel wall fracture. Projected time to brine reservoir penetration at current rate: 41 days, local time. Moisture seepage along eastern wall of shaft is being monitored.\n\nNote: Preliminary subsurface temperature data is significantly lower than programmed estimates.</indent>\n------------------\n";
					}
				}

				// Token: 0x02002FB1 RID: 12209
				public class CREEPYBASEMENTLAB
				{
					// Token: 0x0400C248 RID: 49736
					public static LocString TITLE = "Debris Analysis";

					// Token: 0x0400C249 RID: 49737
					public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

					// Token: 0x020032DE RID: 13022
					public class BODY
					{
						// Token: 0x0400CA1C RID: 51740
						public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: B577, B997, B083, A216]</smallcaps>\n\n[LOG BEGINS]\n\nA216: The Director said there were supposed to be three of you on this task force. Where's the geneticist?\n\nB083: In the bathroom-\n\nB997: He went home.\n\n[long pause]\n\nB997: It's the holidays. He has a family.\n\nA216: We all do. That's exactly why this project is so urgent.\n\nB997: It's not our fault this stuff sat in a subterranean ocean for a year, and took another year to get back to Earth! The microbe samples didn't fare well on the journey, and most of the mechanical components are completely corroded. There's not much to-\n\nB083: -we're analyzing it all and salvaging what we can, Jea- ...Dr. Saruhashi.\n\nA216: Good. And take down those ridiculous lights. This is a lab, not a retro \"shopping mall.\"\n\n[LOG ENDS]\n------------------\n[LOG BEGINS]\n\nB577: Thanks for getting all the debris packed up for disposal.\n\nB997: I thought you did that.\n\nB577: No, I-\n\nB083: Who took my sandwich?\n\nB997: Not this again.\n\nB577: Ren, did you load the shipping container?\n\nB083: Seriously, I haven't eaten in thirteen hours. This isn't funny.\n\nB997: It's a little funny.\n\nB577: Can we focus, please?\n\nB997: Nobody took your sandwich, Rock Doc.\n\nB083: Then why does my food keep going missing?\n\nB997: Maybe the lab ghost took it. Or maybe you just shouldn't leave it out overnight. Gunderson probably thought it was garbage.\n\nB083: He doesn't even clean down here!\n\nB997: Right. Because if he did, I wouldn't have to keep sweeping up the magnesium sulfate deposits that <i>someone</i> keeps tracking all over the floor between shifts.\n\nB083: It's not me!\n\nB577: Listen, I know we're all tired and things have been a little strange. But the sooner we get this sent up to the launchpad, the sooner it starts its trip to the sun and we can all get out of this creepy sub-sub-basement.\n\nB083: Fine.\n\nB997: Fine.\n\nB083: Fine!\n\n[LOG ENDS]\n------------------\n";
					}
				}

				// Token: 0x02002FB2 RID: 12210
				public class HOLIDAYCARD
				{
					// Token: 0x0400C24A RID: 49738
					public static LocString TITLE = "Pudding Cups";

					// Token: 0x0400C24B RID: 49739
					public static LocString SUBTITLE = "";

					// Token: 0x020032DF RID: 13023
					public class BODY
					{
						// Token: 0x0400CA1D RID: 51741
						public static LocString CONTAINER1 = "Hey kiddo,\n\nWe missed you at your cousin's wedding last weekend. The gift was nice, but the dance floor felt empty without you.\n\nDariush sends his love. He's really turned a corner since he started eating those gooey pudding things you sent over. Any chance you have a version that doesn't smell like feet?\n\nCome home sometime when you're not so busy.\n\n- Baba\n------------------\n";
					}
				}
			}

			// Token: 0x02002457 RID: 9303
			public static class FOSSILHUNT
			{
				// Token: 0x0400A0C5 RID: 41157
				public static LocString NAME = "Ancient Specimen";

				// Token: 0x0400A0C6 RID: 41158
				public static LocString DESCRIPTION = "This asteroid has a few skeletons in its geological closet.\n\nTrack down the fossilized fragments of an ancient critter to assemble key pieces of Gravitas history and unlock a new resource.";

				// Token: 0x0400A0C7 RID: 41159
				public static LocString DESCRIPTION_SHORT = "Track down the fossilized fragments of an ancient critter.";

				// Token: 0x0400A0C8 RID: 41160
				public static LocString DESCRIPTION_BUILDINGMENU_COVERED = "Unlocking full access to the fossil cache buried beneath the ancient specimen requires excavation of all deposit sites.";

				// Token: 0x0400A0C9 RID: 41161
				public static LocString DESCRIPTION_REVEALED = "Unlocking full access to the fossil cache buried beneath the ancient specimen requires excavation of all deposit sites.";

				// Token: 0x02002FB3 RID: 12211
				public class MISC
				{
					// Token: 0x0400C24C RID: 49740
					public static LocString DECREASE_DECOR_ATTRIBUTE = "Obscured";
				}

				// Token: 0x02002FB4 RID: 12212
				public class STATUSITEMS
				{
					// Token: 0x020032E0 RID: 13024
					public class FOSSILMINEPENDINGWORK
					{
						// Token: 0x0400CA1E RID: 51742
						public static LocString NAME = "Work Errand";

						// Token: 0x0400CA1F RID: 51743
						public static LocString TOOLTIP = "Fossil mine will be operated once a Duplicant is available";
					}

					// Token: 0x020032E1 RID: 13025
					public class FOSSILIDLE
					{
						// Token: 0x0400CA20 RID: 51744
						public static LocString NAME = "No Mining Orders Queued";

						// Token: 0x0400CA21 RID: 51745
						public static LocString TOOLTIP = "Select an excavation order to begin mining";
					}

					// Token: 0x020032E2 RID: 13026
					public class FOSSILEMPTY
					{
						// Token: 0x0400CA22 RID: 51746
						public static LocString NAME = "Waiting For Materials";

						// Token: 0x0400CA23 RID: 51747
						public static LocString TOOLTIP = "Mining will begin once materials have been delivered";
					}

					// Token: 0x020032E3 RID: 13027
					public class FOSSILENTOMBED
					{
						// Token: 0x0400CA24 RID: 51748
						public static LocString NAME = "Entombed";

						// Token: 0x0400CA25 RID: 51749
						public static LocString TOOLTIP = "This fossil must be dug out before it can be excavated";

						// Token: 0x0400CA26 RID: 51750
						public static LocString LINE_ITEM = "    • Entombed";
					}
				}

				// Token: 0x02002FB5 RID: 12213
				public class UISIDESCREENS
				{
					// Token: 0x0400C24D RID: 49741
					public static LocString DIG_SITE_EXCAVATE_BUTTON = "Excavate";

					// Token: 0x0400C24E RID: 49742
					public static LocString DIG_SITE_EXCAVATE_BUTTON_TOOLTIP = "Carefully uncover and examine this fossil";

					// Token: 0x0400C24F RID: 49743
					public static LocString DIG_SITE_CANCEL_EXCAVATION_BUTTON = "Cancel Excavation";

					// Token: 0x0400C250 RID: 49744
					public static LocString DIG_SITE_CANCEL_EXCAVATION_BUTTON_TOOLTIP = "Abandon excavation efforts";

					// Token: 0x0400C251 RID: 49745
					public static LocString MINOR_DIG_SITE_REVEAL_BUTTON = "Main Site";

					// Token: 0x0400C252 RID: 49746
					public static LocString MINOR_DIG_SITE_REVEAL_BUTTON_TOOLTIP = "Click to show this site";

					// Token: 0x0400C253 RID: 49747
					public static LocString FOSSIL_BITS_EXCAVATE_BUTTON = "Excavate";

					// Token: 0x0400C254 RID: 49748
					public static LocString FOSSIL_BITS_EXCAVATE_BUTTON_TOOLTIP = "Carefully uncover and examine this fossil";

					// Token: 0x0400C255 RID: 49749
					public static LocString FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON = "Cancel Excavation";

					// Token: 0x0400C256 RID: 49750
					public static LocString FOSSIL_BITS_CANCEL_EXCAVATION_BUTTON_TOOLTIP = "Abandon excavation efforts";

					// Token: 0x0400C257 RID: 49751
					public static LocString FABRICATOR_LIST_TITLE = "Mining Orders";

					// Token: 0x0400C258 RID: 49752
					public static LocString FABRICATOR_RECIPE_SCREEN_TITLE = "Recipe";
				}

				// Token: 0x02002FB6 RID: 12214
				public class BEGIN_POPUP
				{
					// Token: 0x0400C259 RID: 49753
					public static LocString NAME = "Story Trait: Ancient Specimen";

					// Token: 0x0400C25A RID: 49754
					public static LocString CODEX_NAME = "First Encounter";

					// Token: 0x0400C25B RID: 49755
					public static LocString DESCRIPTION = "I've discovered a fossilized critter buried in my colony—at least, part of one—but it does not resemble any of the species we have encountered on this asteroid.\n\nWhere did it come from? How did it get here? And what other questions might these bones hold the answer to?\n\nThere is only one way to find out.";

					// Token: 0x0400C25C RID: 49756
					public static LocString BUTTON = "Close";
				}

				// Token: 0x02002FB7 RID: 12215
				public class END_POPUP
				{
					// Token: 0x0400C25D RID: 49757
					public static LocString NAME = "Story Trait Complete: Ancient Specimen";

					// Token: 0x0400C25E RID: 49758
					public static LocString CODEX_NAME = "Challenge Completed";

					// Token: 0x0400C25F RID: 49759
					public static LocString DESCRIPTION = "My Duplicants have meticulously reassembled as much of the giant critter's scattered remains as they could find.\n\nTheir efforts have unearthed a seemingly bottomless fossil quarry beneath the largest fragment's dig site.\n\nNestled among the topmost bones was a handcrafted critter collar. It's too large to have belonged to any species traditionally categorized as companion animals.";

					// Token: 0x0400C260 RID: 49760
					public static LocString BUTTON = "Activate Fossil Quarry";
				}

				// Token: 0x02002FB8 RID: 12216
				public class REWARDS
				{
					// Token: 0x020032E4 RID: 13028
					public class MINED_FOSSIL
					{
						// Token: 0x0400CA27 RID: 51751
						public static LocString DESC = "Mined " + UI.FormatAsLink("Fossil", "FOSSIL");
					}
				}

				// Token: 0x02002FB9 RID: 12217
				public class ENTITIES
				{
					// Token: 0x020032E5 RID: 13029
					public class FOSSIL_DIG_SITE
					{
						// Token: 0x0400CA28 RID: 51752
						public static LocString NAME = "Ancient Specimen";

						// Token: 0x0400CA29 RID: 51753
						public static LocString DESC = "Here lies a significant portion of the remains of an enormous, long-dead critter.\n\nIt's not from around here.";
					}

					// Token: 0x020032E6 RID: 13030
					public class FOSSIL_RESIN
					{
						// Token: 0x0400CA2A RID: 51754
						public static LocString NAME = "Amber Fossil";

						// Token: 0x0400CA2B RID: 51755
						public static LocString DESC = "The well-preserved partial remains of a critter of unknown origin.\n\nIt appears to belong to the same ancient specimen found at another site.\n\nThis fragment has been preserved in a resin-like substance.";
					}

					// Token: 0x020032E7 RID: 13031
					public class FOSSIL_ICE
					{
						// Token: 0x0400CA2C RID: 51756
						public static LocString NAME = "Frozen Fossil";

						// Token: 0x0400CA2D RID: 51757
						public static LocString DESC = "The well-preserved partial remains of a critter of unknown origin.\n\nIt appears to belong to the same ancient specimen found at another site.\n\nThis fragment has been preserved in " + UI.FormatAsLink("Ice", "ICE") + ".";
					}

					// Token: 0x020032E8 RID: 13032
					public class FOSSIL_ROCK
					{
						// Token: 0x0400CA2E RID: 51758
						public static LocString NAME = "Petrified Fossil";

						// Token: 0x0400CA2F RID: 51759
						public static LocString DESC = "The well-preserved partial remains of a critter of unknown origin.\n\nIt appears to belong to the same ancient specimen found at another site.\n\nThis fragment has been preserved in petrified " + UI.FormatAsLink("Dirt", "DIRT") + ".";
					}

					// Token: 0x020032E9 RID: 13033
					public class FOSSIL_BITS
					{
						// Token: 0x0400CA30 RID: 51760
						public static LocString NAME = "Fossil Fragments";

						// Token: 0x0400CA31 RID: 51761
						public static LocString DESC = "Bony debris that can be excavated for " + UI.FormatAsLink("Fossil", "FOSSIL") + ".";
					}
				}

				// Token: 0x02002FBA RID: 12218
				public class QUEST
				{
					// Token: 0x0400C261 RID: 49761
					public static LocString LINKED_TOOLTIP = "\n\nClick to show this site";
				}

				// Token: 0x02002FBB RID: 12219
				public class ICECRITTERDESIGN
				{
					// Token: 0x0400C262 RID: 49762
					public static LocString TITLE = "Organism Design Notes";

					// Token: 0x0400C263 RID: 49763
					public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

					// Token: 0x020032EA RID: 13034
					public class BODY
					{
						// Token: 0x0400CA32 RID: 51762
						public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B363]</smallcaps>\n\n[LOG BEGINS]\n\n...Restricting our organism design to specifically target survival in an off-planet polar climate has narrowed our focus significantly, allowing development of this project to rapidly outpace the others.\n\nWe have successfully optimized for adaptive features such as the formation of protective adipose tissue at >40% of the organism's total mass. Dr. Bubare was concerned about the consequences for muscle mass, but results confirm that reductions fall within an acceptable range.\n\nOur next step is to adapt the organism's diet. It would be inadvisable to populate a new colony with carnivorous creatures of this size.\n\n[LOG ENDS]\n------------------\n[LOG BEGINS]\n\n...When I am alone in the lab, I find myself gravitating toward the enclosure to listen to the creature's melodic vocalizations. Sometimes the pitch changes slightly as I approach.\n\nI am not certain what that means.\n\n[LOG ENDS]\n------------------\n";

						// Token: 0x0400CA33 RID: 51763
						public static LocString CONTAINER2 = "[LOG BEGINS]\n\n...Some of the other departments have taken to calling our work here \"Project Meat Popsicle\". It is a crass misnomer. This species is not designed to be a food source: it must survive the Ceres climate long enough to establish a stable population that will enable the subsequent settlement party to access the essential research data stored in its DNA via Dr. Winslow's revolutionary genome-encoding technique.\n\nImagine, countless yottabytes' worth of scientific documentation wandering freely around a new colony...the ultimate self-sustaining archive, providing stable data storage that requires zero technological maintenance.\n\nIt gives new meaning to the term, \"living document.\"\n\n[LOG ENDS]\n------------------\n[LOG BEGINS]\n\n...Today is the day. My sonorous critter and her handful of progeny are ready to be transported to their new home. They are scheduled to arrive three months in the past, to ensure that they are well established before the settlement party's arrival next week.\n\nDr. Techna invited me to assist with the teleportation. I was relieved to be too busy to accept. I have heard rumors about previous shipments going awry. These stories are unsubstantiated, and yet...\n\nThe urgency of our mission sometimes necessitates non-ideal compromises.\n\nThe lab is so very quiet now.\n\n[LOG ENDS]\n------------------\n";
					}
				}

				// Token: 0x02002FBC RID: 12220
				public class QUEST_AVAILABLE_NOTIFICATION
				{
					// Token: 0x0400C264 RID: 49764
					public static LocString NAME = "Fossil Excavated";

					// Token: 0x0400C265 RID: 49765
					public static LocString TOOLTIP = "Additional fossils located";
				}

				// Token: 0x02002FBD RID: 12221
				public class QUEST_AVAILABLE_POPUP
				{
					// Token: 0x0400C266 RID: 49766
					public static LocString NAME = "Fossil Excavated";

					// Token: 0x0400C267 RID: 49767
					public static LocString CHECK_BUTTON = "View Site";

					// Token: 0x0400C268 RID: 49768
					public static LocString DESCRIPTION = "Success! My Duplicants have safely excavated a set of strange, fossilized remains.\n\nIt appears that there are more of this giant critter's bones strewn around the asteroid. It's vital that we reassemble this skeleton for deeper analysis.";
				}

				// Token: 0x02002FBE RID: 12222
				public class UNLOCK_DNADATA_NOTIFICATION
				{
					// Token: 0x0400C269 RID: 49769
					public static LocString NAME = "Fossil Data Decoded";

					// Token: 0x0400C26A RID: 49770
					public static LocString TOOLTIP = "There was data stored in this fossilized critter's DNA";
				}

				// Token: 0x02002FBF RID: 12223
				public class UNLOCK_DNADATA_POPUP
				{
					// Token: 0x0400C26B RID: 49771
					public static LocString NAME = "Data Discovered in Fossil";

					// Token: 0x0400C26C RID: 49772
					public static LocString VIEW_IN_CODEX = "View Data";
				}

				// Token: 0x02002FC0 RID: 12224
				public class DNADATA_ENTRY
				{
					// Token: 0x0400C26D RID: 49773
					public static LocString TELEPORTFAILURE = "It appears that this creature's DNA was once used as a kind of genetic storage unit.";
				}

				// Token: 0x02002FC1 RID: 12225
				public class DNADATA_ENTRY_EXPANDED
				{
					// Token: 0x0400C26E RID: 49774
					public static LocString TITLE = "SUBJECT: RESETTLEMENT LAUNCH PARTY";

					// Token: 0x0400C26F RID: 49775
					public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

					// Token: 0x020032EB RID: 13035
					public class BODY
					{
						// Token: 0x0400CA34 RID: 51764
						public static LocString EMAILHEADER = "<smallcaps>To: <b>[REDACTED]</b><alpha=#AA><size=12></size></color>\nFrom: <b>[REDACTED]</b><alpha=#AA></smallcaps>\n------------------\n";

						// Token: 0x0400CA35 RID: 51765
						public static LocString CONTAINER1 = "<indent=5%>Dear [REDACTED]\n\nWe are pleased to announce that research objectives for Operation Piazzi's Planet are nearing completion. Thank you all for your patience as we navigated the unprecedented obstacles that such groundbreaking work entails.\n\nWe are aware of rumors regarding documents leaked from Dr. [REDACTED]'s files.\n\nRest assured that the contents of this supposed \"whistleblower\" effort are entirely fabricated—our technology is far too advanced to allow for the type of miscalculation that would result in OPP shipments arriving at their destination some 10,000 years prior to the targeted date.\n\nOur IT security team is currently investigating the document's digital footprint to determine its origin.\n\nTo express our gratitude for your continued support, we would like to invite key stakeholders to a private launch party held at the Gravitas Facility. The evening will be emceed by Dr. Olivia Broussard, who will present our groundbreaking prototypes along with a five-course meal featuring lab-crafted ingredients.\n\nDue to the sensitive nature of our work, we regret that no additional guests or dietary restrictions can be accommodated at this time.\n\nDirector Stern will be hosting a 30-minute Q&A session after dinner. Questions must be submitted at least 24 hours in advance.\n\nQueries about the [REDACTED] papers will be disregarded.\n\nPlease be advised that the contents of this e-mail will expire three minutes from the time of opening.</indent>";

						// Token: 0x0400CA36 RID: 51766
						public static LocString SIGNATURE = "\nSincerely,\n[REDACTED]\n<size=11>The Gravitas Facility</size>\n------------------\n";
					}
				}

				// Token: 0x02002FC2 RID: 12226
				public class HALLWAYRACES
				{
					// Token: 0x0400C270 RID: 49776
					public static LocString TITLE = "Unauthorized Activity";

					// Token: 0x0400C271 RID: 49777
					public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

					// Token: 0x020032EC RID: 13036
					public class BODY
					{
						// Token: 0x0400CA37 RID: 51767
						public static LocString EMAILHEADER = "<smallcaps>To: <b>ALL</b><alpha=#AA><size=12></size></color>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color></smallcaps>\n------------------\n";

						// Token: 0x0400CA38 RID: 51768
						public static LocString CONTAINER1 = "<indent=5%>Employees are advised that removing organisms from the bioengineering labs without an approved requisition form is strictly prohibited.\n\nGravitas projects are not designed to be ridden for sport. Injuries sustained during unsanctioned activities are not eligible for coverage under corporate health benefits.\n\nPlease find a comprehensive summary of company regulations attached.\n\n<alpha=#AA>[MISSING ATTACHMENT]</indent>";

						// Token: 0x0400CA39 RID: 51769
						public static LocString SIGNATURE = "\nThank-you,\n-Admin\n<size=11>The Gravitas Facility</size>\n------------------\n";
					}
				}
			}
		}

		// Token: 0x02001CD5 RID: 7381
		public class QUESTS
		{
			// Token: 0x02002458 RID: 9304
			public class KNOCKQUEST
			{
				// Token: 0x0400A0CA RID: 41162
				public static LocString NAME = "Greet Occupant";

				// Token: 0x0400A0CB RID: 41163
				public static LocString COMPLETE = "Initial contact was a success! Our new neighbor seems friendly, though extremely shy.\n\nThey'll need a little more coaxing before they're ready to join my colony.";
			}

			// Token: 0x02002459 RID: 9305
			public class FOODQUEST
			{
				// Token: 0x0400A0CC RID: 41164
				public static LocString NAME = "Welcome Dinner";

				// Token: 0x0400A0CD RID: 41165
				public static LocString COMPLETE = "Success! My Duplicants' cooking has whetted the hermit's appetite for communal living.\n\nThey've also found what appears to be a page from an old logbook tucked behind the mailbox.";
			}

			// Token: 0x0200245A RID: 9306
			public class PLUGGEDIN
			{
				// Token: 0x0400A0CE RID: 41166
				public static LocString NAME = "On the Grid";

				// Token: 0x0400A0CF RID: 41167
				public static LocString COMPLETE = "Success! The hermit is very excited about being on the grid.\n\nThe bright lights illuminate an unfamiliar file on the ground nearby.";
			}

			// Token: 0x0200245B RID: 9307
			public class HIGHDECOR
			{
				// Token: 0x0400A0D0 RID: 41168
				public static LocString NAME = "Nice Neighborhood";

				// Token: 0x0400A0D1 RID: 41169
				public static LocString COMPLETE = "Success! All this excellent decor is really making the hermit feel at home.\n\nHe scrawled a thank-you note on the back of an old holiday card.";
			}

			// Token: 0x0200245C RID: 9308
			public class FOSSILHUNTQUEST
			{
				// Token: 0x0400A0D2 RID: 41170
				public static LocString NAME = "Scattered Fragments";

				// Token: 0x0400A0D3 RID: 41171
				public static LocString COMPLETE = "Each of the fossil deposits on this asteroid has been excavated, and its contents safely retrieved.\n\nThe ancient specimen's deeper cache of fossil can now be mined.";
			}

			// Token: 0x0200245D RID: 9309
			public class CRITERIA
			{
				// Token: 0x02002FC3 RID: 12227
				public class NEIGHBOR
				{
					// Token: 0x0400C272 RID: 49778
					public static LocString NAME = "Knock on door";

					// Token: 0x0400C273 RID: 49779
					public static LocString TOOLTIP = "Send a Duplicant over to introduce themselves and discover what it'll take to turn this stranger into a friend";
				}

				// Token: 0x02002FC4 RID: 12228
				public class DECOR
				{
					// Token: 0x0400C274 RID: 49780
					public static LocString NAME = "Improve nearby Decor";

					// Token: 0x0400C275 RID: 49781
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Establish average ",
						UI.PRE_KEYWORD,
						"Decor",
						UI.PST_KEYWORD,
						" of {0} or higher for the area surrounding this building\n\nAverage Decor: {1:0.##}"
					});
				}

				// Token: 0x02002FC5 RID: 12229
				public class SUPPLIEDPOWER
				{
					// Token: 0x0400C276 RID: 49782
					public static LocString NAME = "Turn on festive lights";

					// Token: 0x0400C277 RID: 49783
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Connect this building to ",
						UI.PRE_KEYWORD,
						"Power",
						UI.PST_KEYWORD,
						" long enough to cheer up its occupant\n\nTime Remaining: {0}s"
					});
				}

				// Token: 0x02002FC6 RID: 12230
				public class FOODQUALITY
				{
					// Token: 0x0400C278 RID: 49784
					public static LocString NAME = "Deliver Food to the mailbox";

					// Token: 0x0400C279 RID: 49785
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Deliver 3 unique ",
						UI.PRE_KEYWORD,
						"Food",
						UI.PST_KEYWORD,
						" items. Quality must be {0} or higher\n\nFoods Delivered:\n{1}"
					});

					// Token: 0x0400C27A RID: 49786
					public static LocString NONE = "None";
				}

				// Token: 0x02002FC7 RID: 12231
				public class LOSTSPECIMEN
				{
					// Token: 0x0400C27B RID: 49787
					public static LocString NAME = UI.FormatAsLink("Ancient Specimen", "MOVECAMERATOFossilDig");

					// Token: 0x0400C27C RID: 49788
					public static LocString TOOLTIP = "Retrieve the largest deposit of the ancient critter's remains";

					// Token: 0x0400C27D RID: 49789
					public static LocString NONE = "None";
				}

				// Token: 0x02002FC8 RID: 12232
				public class LOSTICEFOSSIL
				{
					// Token: 0x0400C27E RID: 49790
					public static LocString NAME = UI.FormatAsLink("Frozen Fossil", "MOVECAMERATOFossilIce");

					// Token: 0x0400C27F RID: 49791
					public static LocString TOOLTIP = "Retrieve a piece of the ancient critter that has been preserved in " + UI.PRE_KEYWORD + "Ice" + UI.PST_KEYWORD;

					// Token: 0x0400C280 RID: 49792
					public static LocString NONE = "None";
				}

				// Token: 0x02002FC9 RID: 12233
				public class LOSTRESINFOSSIL
				{
					// Token: 0x0400C281 RID: 49793
					public static LocString NAME = UI.FormatAsLink("Amber Fossil", "MOVECAMERATOFossilResin");

					// Token: 0x0400C282 RID: 49794
					public static LocString TOOLTIP = "Retrieve a piece of the ancient critter that has been preserved in a strangely resin-like substance";

					// Token: 0x0400C283 RID: 49795
					public static LocString NONE = "None";
				}

				// Token: 0x02002FCA RID: 12234
				public class LOSTROCKFOSSIL
				{
					// Token: 0x0400C284 RID: 49796
					public static LocString NAME = UI.FormatAsLink("Petrified Fossil", "MOVECAMERATOFossilRock");

					// Token: 0x0400C285 RID: 49797
					public static LocString TOOLTIP = string.Concat(new string[]
					{
						"Retrieve a piece of the ancient critter that has been preserved in ",
						UI.PRE_KEYWORD,
						"Rock",
						UI.PST_KEYWORD,
						" "
					});

					// Token: 0x0400C286 RID: 49798
					public static LocString NONE = "None";
				}
			}
		}

		// Token: 0x02001CD6 RID: 7382
		public class HEADQUARTERS
		{
			// Token: 0x04008358 RID: 33624
			public static LocString TITLE = "Printing Pod";

			// Token: 0x0200245E RID: 9310
			public class BODY
			{
				// Token: 0x0400A0D4 RID: 41172
				public static LocString CONTAINER1 = "An advanced 3D printer developed by the Gravitas Facility.\n\nThe Printing Pod is notable for its ability to print living organic material from biological blueprints.\n\nIt is capable of synthesizing its own organic material for printing, and contains an almost unfathomable amount of stored energy, allowing it to autonomously print every 3 cycles.";

				// Token: 0x0400A0D5 RID: 41173
				public static LocString CONTAINER2 = "";
			}
		}

		// Token: 0x02001CD7 RID: 7383
		public class HEADERS
		{
			// Token: 0x04008359 RID: 33625
			public static LocString FABRICATIONS = "All Recipes";

			// Token: 0x0400835A RID: 33626
			public static LocString RECEPTACLE = "Farmable Plants";

			// Token: 0x0400835B RID: 33627
			public static LocString RECIPE = "Recipe Ingredients";

			// Token: 0x0400835C RID: 33628
			public static LocString USED_IN_RECIPES = "Ingredient In";

			// Token: 0x0400835D RID: 33629
			public static LocString TECH_UNLOCKS = "Unlocks";

			// Token: 0x0400835E RID: 33630
			public static LocString PREREQUISITE_TECH = "Prerequisite Tech";

			// Token: 0x0400835F RID: 33631
			public static LocString PREREQUISITE_ROLES = "Prerequisite Jobs";

			// Token: 0x04008360 RID: 33632
			public static LocString UNLOCK_ROLES = "Promotion Opportunities";

			// Token: 0x04008361 RID: 33633
			public static LocString UNLOCK_ROLES_DESC = "Promotions introduce further stat boosts and traits that stack with existing Job Training.";

			// Token: 0x04008362 RID: 33634
			public static LocString ROLE_PERKS = "Job Training";

			// Token: 0x04008363 RID: 33635
			public static LocString ROLE_PERKS_DESC = "Job Training automatically provides permanent traits and stat increases that are retained even when a Duplicant switches jobs.";

			// Token: 0x04008364 RID: 33636
			public static LocString DIET = "Diet";

			// Token: 0x04008365 RID: 33637
			public static LocString PRODUCES = "Excretes";

			// Token: 0x04008366 RID: 33638
			public static LocString HATCHESFROMEGG = "Hatched from";

			// Token: 0x04008367 RID: 33639
			public static LocString GROWNFROMSEED = "Grown from";

			// Token: 0x04008368 RID: 33640
			public static LocString BUILDINGEFFECTS = "Effects";

			// Token: 0x04008369 RID: 33641
			public static LocString BUILDINGREQUIREMENTS = "Requirements";

			// Token: 0x0400836A RID: 33642
			public static LocString BUILDINGCONSTRUCTIONPROPS = "Construction Properties";

			// Token: 0x0400836B RID: 33643
			public static LocString BUILDINGCONSTRUCTIONMATERIALS = "Materials: ";

			// Token: 0x0400836C RID: 33644
			public static LocString BUILDINGTYPE = "Room Requirements Class";

			// Token: 0x0400836D RID: 33645
			public static LocString SUBENTRIES = "Entries ({0}/{1})";

			// Token: 0x0400836E RID: 33646
			public static LocString COMFORTRANGE = "Ideal Temperatures";

			// Token: 0x0400836F RID: 33647
			public static LocString ELEMENTTRANSITIONS = "Additional States";

			// Token: 0x04008370 RID: 33648
			public static LocString ELEMENTTRANSITIONSTO = "Transitions To";

			// Token: 0x04008371 RID: 33649
			public static LocString ELEMENTTRANSITIONSFROM = "Transitions From";

			// Token: 0x04008372 RID: 33650
			public static LocString ELEMENTCONSUMEDBY = "Applications";

			// Token: 0x04008373 RID: 33651
			public static LocString ELEMENTPRODUCEDBY = "Produced By";

			// Token: 0x04008374 RID: 33652
			public static LocString MATERIALUSEDTOCONSTRUCT = "Construction Uses";

			// Token: 0x04008375 RID: 33653
			public static LocString SECTION_UNLOCKABLES = "Undiscovered Data";

			// Token: 0x04008376 RID: 33654
			public static LocString CONTENTLOCKED = "Undiscovered";

			// Token: 0x04008377 RID: 33655
			public static LocString CONTENTLOCKED_SUBTITLE = "More research or exploration is required";

			// Token: 0x04008378 RID: 33656
			public static LocString INTERNALBATTERY = "Battery";

			// Token: 0x04008379 RID: 33657
			public static LocString INTERNALSTORAGE = "Storage";

			// Token: 0x0400837A RID: 33658
			public static LocString CRITTERMAXAGE = "Life Span";

			// Token: 0x0400837B RID: 33659
			public static LocString CRITTEROVERCROWDING = "Space Required";

			// Token: 0x0400837C RID: 33660
			public static LocString CRITTERDROPS = "Drops";

			// Token: 0x0400837D RID: 33661
			public static LocString FOODEFFECTS = "Nutritional Effects";

			// Token: 0x0400837E RID: 33662
			public static LocString FOODSWITHEFFECT = "Foods with this effect";
		}

		// Token: 0x02001CD8 RID: 7384
		public class FORMAT_STRINGS
		{
			// Token: 0x0400837F RID: 33663
			public static LocString TEMPERATURE_OVER = "Temperature over {0}";

			// Token: 0x04008380 RID: 33664
			public static LocString TEMPERATURE_UNDER = "Temperature under {0}";

			// Token: 0x04008381 RID: 33665
			public static LocString CONSTRUCTION_TIME = "Build Time: {0} seconds";

			// Token: 0x04008382 RID: 33666
			public static LocString BUILDING_SIZE = "Building Size: {0} wide x {1} high";

			// Token: 0x04008383 RID: 33667
			public static LocString MATERIAL_MASS = "{0} {1}";

			// Token: 0x04008384 RID: 33668
			public static LocString TRANSITION_LABEL_TO_ONE_ELEMENT = "{0} to {1}";

			// Token: 0x04008385 RID: 33669
			public static LocString TRANSITION_LABEL_TO_TWO_ELEMENTS = "{0} to {1} and {2}";
		}

		// Token: 0x02001CD9 RID: 7385
		public class CREATURE_DESCRIPTORS
		{
			// Token: 0x04008386 RID: 33670
			public static LocString MAXAGE = "This critter's typical " + UI.FormatAsLink("Life Span", "CREATURES::GUIDE::FERTILITY") + " is <b>{0} cycles</b>.";

			// Token: 0x04008387 RID: 33671
			public static LocString OVERCROWDING = UI.FormatAsLink("Overcrowded", "CREATURES::GUIDE::MOOD") + " when a room has less than <b>{0} cells</b> of space for each critter.";

			// Token: 0x04008388 RID: 33672
			public static LocString CONFINED = UI.FormatAsLink("Confined", "CREATURES::GUIDE::MOOD") + " when a room is smaller than <b>{0} cells</b>.";

			// Token: 0x04008389 RID: 33673
			public static LocString NON_LETHAL_RANGE = "Livable range: <b>{0}</b> to <b>{1}</b>";

			// Token: 0x0200245F RID: 9311
			public class TEMPERATURE
			{
				// Token: 0x0400A0D6 RID: 41174
				public static LocString COMFORT_RANGE = "Comfort range: <b>{0}</b> to <b>{1}</b>";

				// Token: 0x0400A0D7 RID: 41175
				public static LocString NON_LETHAL_RANGE = "Livable range: <b>{0}</b> to <b>{1}</b>";
			}
		}

		// Token: 0x02001CDA RID: 7386
		public class ROBOT_DESCRIPTORS
		{
			// Token: 0x02002460 RID: 9312
			public class BATTERY
			{
				// Token: 0x0400A0D8 RID: 41176
				public static LocString CAPACITY = "Battery capacity: <b>{0}" + UI.UNITSUFFIXES.ELECTRICAL.JOULE + "</b>";
			}

			// Token: 0x02002461 RID: 9313
			public class STORAGE
			{
				// Token: 0x0400A0D9 RID: 41177
				public static LocString CAPACITY = "Internal storage: <b>{0}" + UI.UNITSUFFIXES.MASS.KILOGRAM + "</b>";
			}
		}

		// Token: 0x02001CDB RID: 7387
		public class PAGENOTFOUND
		{
			// Token: 0x0400838A RID: 33674
			public static LocString TITLE = "Data Not Found";

			// Token: 0x0400838B RID: 33675
			public static LocString SUBTITLE = "This database entry is under construction or unavailable";

			// Token: 0x0400838C RID: 33676
			public static LocString BODY = "";
		}

		// Token: 0x02001CDC RID: 7388
		public class BUILDING_TYPE
		{
			// Token: 0x0400838D RID: 33677
			public static LocString INDUSTRIAL_MACHINERY = "Industrial Machinery";
		}

		// Token: 0x02001CDD RID: 7389
		public class BEETA
		{
			// Token: 0x0400838E RID: 33678
			public static LocString TITLE = "Beeta";

			// Token: 0x0400838F RID: 33679
			public static LocString SUBTITLE = "Aggressive Critter";

			// Token: 0x02002462 RID: 9314
			public class BODY
			{
				// Token: 0x0400A0DA RID: 41178
				public static LocString CONTAINER1 = string.Concat(new string[]
				{
					"Beetas are insectoid creatures that enjoy a symbiotic relationship with the radioactive environment they thrive in.\n\nMuch like the honey bee gathers nectar and processes it to honey, the Beeta turns ",
					UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
					" into ",
					UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
					" through a complex process of isotope separation inside the Beeta Hive.\n\nWhen first observing the Beeta's enrichment process, many scientists note with surprise just how much more efficient the cooperative combination of insect and hive is when compared to even the most advanced industrial processes."
				});
			}
		}

		// Token: 0x02001CDE RID: 7390
		public class DIVERGENT
		{
			// Token: 0x04008390 RID: 33680
			public static LocString TITLE = "Divergent";

			// Token: 0x04008391 RID: 33681
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002463 RID: 9315
			public class BODY
			{
				// Token: 0x0400A0DB RID: 41179
				public static LocString CONTAINER1 = "'Divergent' is the name given to the two different genders of one species, the Sweetle and the Grubgrub, both of which are able to reproduce asexually and tend to Grubfruit Plants.\n\nWhen tending to the Grubfruit Plant, both gender variants of the Divergent display the exact same behaviors, however the Grubgrub possesses slightly more tiny facial hair which helps in pollinating the plants and stimulates faster growth.";
			}
		}

		// Token: 0x02001CDF RID: 7391
		public class DRECKO
		{
			// Token: 0x04008392 RID: 33682
			public static LocString SPECIES_TITLE = "Dreckos";

			// Token: 0x04008393 RID: 33683
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x04008394 RID: 33684
			public static LocString TITLE = "Drecko";

			// Token: 0x04008395 RID: 33685
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002464 RID: 9316
			public class BODY
			{
				// Token: 0x0400A0DC RID: 41180
				public static LocString CONTAINER1 = "Dreckos are a reptilian species boasting billions of microscopic hairs on their feet, allowing them to stick to and climb most surfaces.";

				// Token: 0x0400A0DD RID: 41181
				public static LocString CONTAINER2 = "The tail of the Drecko, called the \"train\", is purely for decoration and can be lost or shorn without harm to the animal.\n\nAs a result, Drecko fibers are often farmed for use in textile production.\n\nCaring for Dreckos is a fulfilling endeavor thanks to their companionable personalities.\n\nSome domestic Dreckos have even been known to respond to their own names.";
			}
		}

		// Token: 0x02001CE0 RID: 7392
		public class GLOSSY
		{
			// Token: 0x04008396 RID: 33686
			public static LocString TITLE = "Glossy Drecko";

			// Token: 0x04008397 RID: 33687
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002465 RID: 9317
			public class BODY
			{
				// Token: 0x0400A0DE RID: 41182
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Glossy\" Drecko variant</smallcaps>";
			}
		}

		// Token: 0x02001CE1 RID: 7393
		public class GASSYMOO
		{
			// Token: 0x04008398 RID: 33688
			public static LocString TITLE = "Gassy Moo";

			// Token: 0x04008399 RID: 33689
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002466 RID: 9318
			public class BODY
			{
				// Token: 0x0400A0DF RID: 41183
				public static LocString CONTAINER1 = "Little is currently known of the Gassy Moo due to its alien nature and origin.\n\nIt is capable of surviving in zero gravity conditions and no atmosphere, and is dependent on a second alien species, " + UI.FormatAsLink("Gas Grass", "GASGRASS") + ", for its sustenance and survival.";

				// Token: 0x0400A0E0 RID: 41184
				public static LocString CONTAINER2 = "The Moo has an even temperament and can be farmed for Natural Gas, though their method of reproduction has been as of yet undiscovered.";
			}
		}

		// Token: 0x02001CE2 RID: 7394
		public class HATCH
		{
			// Token: 0x0400839A RID: 33690
			public static LocString SPECIES_TITLE = "Hatches";

			// Token: 0x0400839B RID: 33691
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x0400839C RID: 33692
			public static LocString TITLE = "Hatch";

			// Token: 0x0400839D RID: 33693
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002467 RID: 9319
			public class BODY
			{
				// Token: 0x0400A0E1 RID: 41185
				public static LocString CONTAINER1 = "The Hatch has no eyes and is completely blind, although a photosensitive patch atop its head is capable of detecting even minor changes in overhead light, making it prefer dark caves and tunnels.";
			}
		}

		// Token: 0x02001CE3 RID: 7395
		public class STONE
		{
			// Token: 0x0400839E RID: 33694
			public static LocString TITLE = "Stone Hatch";

			// Token: 0x0400839F RID: 33695
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002468 RID: 9320
			public class BODY
			{
				// Token: 0x0400A0E2 RID: 41186
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Stone\" Hatch variant</smallcaps>";

				// Token: 0x0400A0E3 RID: 41187
				public static LocString CONTAINER2 = "When attempting to pet a Hatch, inexperienced handlers make the mistake of reaching out too quickly for the creature's head.\n\nThis triggers a fear response in the Hatch, as its photosensitive patch of skin called the \"parietal eye\" interprets this sudden light change as an incoming aerial predator.";
			}
		}

		// Token: 0x02001CE4 RID: 7396
		public class SAGE
		{
			// Token: 0x040083A0 RID: 33696
			public static LocString TITLE = "Sage Hatch";

			// Token: 0x040083A1 RID: 33697
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002469 RID: 9321
			public class BODY
			{
				// Token: 0x0400A0E4 RID: 41188
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Sage\" Hatch variant</smallcaps>";

				// Token: 0x0400A0E5 RID: 41189
				public static LocString CONTAINER2 = "It is difficult to classify the Hatch's diet as the term \"omnivore\" does not extend to the non-organic materials it is capable of ingesting.\n\nA more appropriate term is \"totumvore\", given that it can consume and find nutritional value in nearly every known substance.";
			}
		}

		// Token: 0x02001CE5 RID: 7397
		public class SMOOTH
		{
			// Token: 0x040083A2 RID: 33698
			public static LocString TITLE = "Smooth Hatch";

			// Token: 0x040083A3 RID: 33699
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x0200246A RID: 9322
			public class BODY
			{
				// Token: 0x0400A0E6 RID: 41190
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Smooth\" Hatch variant</smallcaps>";

				// Token: 0x0400A0E7 RID: 41191
				public static LocString CONTAINER2 = "The proper way to pet a Hatch is to touch any of its four feet to first make it aware of your presence, then either scratch the soft segmented underbelly or firmly pat the creature's thick chitinous back.";
			}
		}

		// Token: 0x02001CE6 RID: 7398
		public class MOLE
		{
			// Token: 0x040083A4 RID: 33700
			public static LocString TITLE = "Shove Vole";

			// Token: 0x040083A5 RID: 33701
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x0200246B RID: 9323
			public class BODY
			{
				// Token: 0x0400A0E8 RID: 41192
				public static LocString CONTAINER1 = "The Shove Vole is a unique creature that possesses two fully developed sets of lungs, allowing it to hold its breath during the long periods it spends underground.";

				// Token: 0x0400A0E9 RID: 41193
				public static LocString CONTAINER2 = "Drill-shaped keratin structures circling the Vole's body aids its ability to drill at high speeds through most natural materials.";
			}
		}

		// Token: 0x02001CE7 RID: 7399
		public class VARIANT_DELICACY
		{
			// Token: 0x040083A6 RID: 33702
			public static LocString TITLE = "Delecta Vole";

			// Token: 0x040083A7 RID: 33703
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x0200246C RID: 9324
			public class BODY
			{
				// Token: 0x0400A0EA RID: 41194
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Delecta\" Vole variant</smallcaps>";
			}
		}

		// Token: 0x02001CE8 RID: 7400
		public class MORB
		{
			// Token: 0x040083A8 RID: 33704
			public static LocString TITLE = "Morb";

			// Token: 0x040083A9 RID: 33705
			public static LocString SUBTITLE = "Pest Critter";

			// Token: 0x0200246D RID: 9325
			public class BODY
			{
				// Token: 0x0400A0EB RID: 41195
				public static LocString CONTAINER1 = "The Morb is a versatile scavenger, capable of breaking down and consuming dead matter from most plant and animal species.";

				// Token: 0x0400A0EC RID: 41196
				public static LocString CONTAINER2 = "It poses a severe disease risk to humans due to the thick slime it excretes to surround its inner cartilage structures.\n\nA single teaspoon of Morb slime can contain up to a quadrillion bacteria that work to deter would-be predators and liquefy its food.";

				// Token: 0x0400A0ED RID: 41197
				public static LocString CONTAINER3 = "Petting a Morb is not recommended.";
			}
		}

		// Token: 0x02001CE9 RID: 7401
		public class PACU
		{
			// Token: 0x040083AA RID: 33706
			public static LocString SPECIES_TITLE = "Pacus";

			// Token: 0x040083AB RID: 33707
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x040083AC RID: 33708
			public static LocString TITLE = "Pacu";

			// Token: 0x040083AD RID: 33709
			public static LocString SUBTITLE = "Aquatic Critter";

			// Token: 0x0200246E RID: 9326
			public class BODY
			{
				// Token: 0x0400A0EE RID: 41198
				public static LocString CONTAINER1 = "The Pacu fish is often interpreted as possessing a vacant stare due to its large and unblinking eyes, yet they are remarkably bright and friendly creatures.";
			}
		}

		// Token: 0x02001CEA RID: 7402
		public class TROPICAL
		{
			// Token: 0x040083AE RID: 33710
			public static LocString TITLE = "Tropical Pacu";

			// Token: 0x040083AF RID: 33711
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x0200246F RID: 9327
			public class BODY
			{
				// Token: 0x0400A0EF RID: 41199
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Tropical\" Pacu variant</smallcaps>";

				// Token: 0x0400A0F0 RID: 41200
				public static LocString CONTAINER2 = "It is said that the average Pacu intelligence is comparable to that of a dog, and that they are capable of learning and distinguishing from over twenty human faces.";
			}
		}

		// Token: 0x02001CEB RID: 7403
		public class CLEANER
		{
			// Token: 0x040083B0 RID: 33712
			public static LocString TITLE = "Gulp Fish";

			// Token: 0x040083B1 RID: 33713
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002470 RID: 9328
			public class BODY
			{
				// Token: 0x0400A0F1 RID: 41201
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Gulp Fish\" Pacu variant</smallcaps>";

				// Token: 0x0400A0F2 RID: 41202
				public static LocString CONTAINER2 = "Despite descending from the Pacu, the Gulp Fish is unique enough both in genetics and behavior to be considered its own subspecies.";
			}
		}

		// Token: 0x02001CEC RID: 7404
		public class PIP
		{
			// Token: 0x040083B2 RID: 33714
			public static LocString SPECIES_TITLE = "Pips";

			// Token: 0x040083B3 RID: 33715
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x040083B4 RID: 33716
			public static LocString TITLE = "Pip";

			// Token: 0x040083B5 RID: 33717
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002471 RID: 9329
			public class BODY
			{
				// Token: 0x0400A0F3 RID: 41203
				public static LocString CONTAINER1 = "Pips are a member of the Rodentia order with a strong caching instinct that causes them to find and bury small objects, most often seeds.";

				// Token: 0x0400A0F4 RID: 41204
				public static LocString CONTAINER2 = "It is unknown whether their caching behavior is a compulsion or a form of entertainment, as the Pip relies primarily on bark and wood for its survival.";

				// Token: 0x0400A0F5 RID: 41205
				public static LocString CONTAINER3 = "Although the Pip lacks truly opposable thumbs, it nonetheless has highly dexterous paws that allow it to rummage through most tight to reach spaces in search of seeds and other treasures.";
			}
		}

		// Token: 0x02001CED RID: 7405
		public class VARIANT_HUG
		{
			// Token: 0x040083B6 RID: 33718
			public static LocString TITLE = "Cuddle Pip";

			// Token: 0x040083B7 RID: 33719
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002472 RID: 9330
			public class BODY
			{
				// Token: 0x0400A0F6 RID: 41206
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Cuddle\" Pip variant</smallcaps>";

				// Token: 0x0400A0F7 RID: 41207
				public static LocString CONTAINER2 = "Cuddle Pips are genetically predisposed to feel deeply affectionate towards the unhatched young of all species, and can often be observed hugging eggs.";
			}
		}

		// Token: 0x02001CEE RID: 7406
		public class PLUGSLUG
		{
			// Token: 0x040083B8 RID: 33720
			public static LocString TITLE = "Plug Slug";

			// Token: 0x040083B9 RID: 33721
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002473 RID: 9331
			public class BODY
			{
				// Token: 0x0400A0F8 RID: 41208
				public static LocString CONTAINER1 = "Plug Slugs are fuzzy gastropoda that are able to cling to walls and ceilings thanks to an extreme triboelectric effect caused by friction between their fur and various surfaces.\n\nThis same phenomomen allows the Plug Slug to generate a significant amount of static electricity that can be converted into power.\n\nThe increased amount of static electricity a Plug Slug can generate when domesticated is due to the internal vibration, or contented 'humming', they demonstrate when all their needs are met.";
			}
		}

		// Token: 0x02001CEF RID: 7407
		public class VARIANT_LIQUID
		{
			// Token: 0x040083BA RID: 33722
			public static LocString TITLE = "Sponge Slug";

			// Token: 0x040083BB RID: 33723
			public static LocString SUBTITLE = "Critter Morph";
		}

		// Token: 0x02001CF0 RID: 7408
		public class VARIANT_GAS
		{
			// Token: 0x040083BC RID: 33724
			public static LocString TITLE = "Smog Slug";

			// Token: 0x040083BD RID: 33725
			public static LocString SUBTITLE = "Critter Morph";
		}

		// Token: 0x02001CF1 RID: 7409
		public class POKESHELL
		{
			// Token: 0x040083BE RID: 33726
			public static LocString SPECIES_TITLE = "Pokeshells";

			// Token: 0x040083BF RID: 33727
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x040083C0 RID: 33728
			public static LocString TITLE = "Pokeshell";

			// Token: 0x040083C1 RID: 33729
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002474 RID: 9332
			public class BODY
			{
				// Token: 0x0400A0F9 RID: 41209
				public static LocString CONTAINER1 = "Pokeshells are bottom-feeding invertebrates that consume the waste and discarded food left behind by other creatures.";

				// Token: 0x0400A0FA RID: 41210
				public static LocString CONTAINER2 = "They have formidably sized claws that fold safely into their shells for protection when not in use.";

				// Token: 0x0400A0FB RID: 41211
				public static LocString CONTAINER3 = "As Pokeshells mature they must periodically shed portions of their exoskeletons to make room for new growth.";

				// Token: 0x0400A0FC RID: 41212
				public static LocString CONTAINER4 = "Although the most dramatic sheds occur early in a Pokeshell's adolescence, they will continue growing and shedding throughout their adult lives, until the day they eventually die.";
			}
		}

		// Token: 0x02001CF2 RID: 7410
		public class VARIANT_WOOD
		{
			// Token: 0x040083C2 RID: 33730
			public static LocString TITLE = "Oakshell";

			// Token: 0x040083C3 RID: 33731
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002475 RID: 9333
			public class BODY
			{
				// Token: 0x0400A0FD RID: 41213
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Oakshell\" variant</smallcaps>";
			}
		}

		// Token: 0x02001CF3 RID: 7411
		public class VARIANT_FRESH_WATER
		{
			// Token: 0x040083C4 RID: 33732
			public static LocString TITLE = "Sanishell";

			// Token: 0x040083C5 RID: 33733
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002476 RID: 9334
			public class BODY
			{
				// Token: 0x0400A0FE RID: 41214
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Sanishell\" variant</smallcaps>";
			}
		}

		// Token: 0x02001CF4 RID: 7412
		public class PUFT
		{
			// Token: 0x040083C6 RID: 33734
			public static LocString SPECIES_TITLE = "Pufts";

			// Token: 0x040083C7 RID: 33735
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x040083C8 RID: 33736
			public static LocString TITLE = "Puft";

			// Token: 0x040083C9 RID: 33737
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002477 RID: 9335
			public class BODY
			{
				// Token: 0x0400A0FF RID: 41215
				public static LocString CONTAINER1 = "The Puft is a mellow creature whose limited brainpower is largely dedicated to sustaining its basic life processes.";
			}
		}

		// Token: 0x02001CF5 RID: 7413
		public class SQUEAKY
		{
			// Token: 0x040083CA RID: 33738
			public static LocString TITLE = "Squeaky Puft";

			// Token: 0x040083CB RID: 33739
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002478 RID: 9336
			public class BODY
			{
				// Token: 0x0400A100 RID: 41216
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Squeaky\" Puft variant</smallcaps>";

				// Token: 0x0400A101 RID: 41217
				public static LocString CONTAINER2 = "Pufts often have a collection of asymmetric teeth lining the ridge of their mouths, although this feature is entirely vestigial as Pufts do not consume solid food.\n\nInstead, a baleen-like mesh of keratin at the back of the Puft's throat works to filter out tiny organisms and food particles from the air.\n\nUnusable air is expelled back out the Puft's posterior trunk, along with waste material and any indigestible particles or pathogens which it then evacuates as solid biomass.";
			}
		}

		// Token: 0x02001CF6 RID: 7414
		public class DENSE
		{
			// Token: 0x040083CC RID: 33740
			public static LocString TITLE = "Dense Puft";

			// Token: 0x040083CD RID: 33741
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002479 RID: 9337
			public class BODY
			{
				// Token: 0x0400A102 RID: 41218
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Dense\" Puft variant</smallcaps>";

				// Token: 0x0400A103 RID: 41219
				public static LocString CONTAINER2 = "The Puft is an easy creature to raise for first time handlers given its wholly amiable disposition and suggestible nature.\n\nIt is unusually tolerant of human handling and will allow itself to be patted or scratched nearly anywhere on its fuzzy body, including, unnervingly, directly on any of its three eyeballs.";
			}
		}

		// Token: 0x02001CF7 RID: 7415
		public class PRINCE
		{
			// Token: 0x040083CE RID: 33742
			public static LocString TITLE = "Puft Prince";

			// Token: 0x040083CF RID: 33743
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x0200247A RID: 9338
			public class BODY
			{
				// Token: 0x0400A104 RID: 41220
				public static LocString CONTAINER1 = "<smallcaps>Pictured: Puft \"Prince\" variant</smallcaps>";

				// Token: 0x0400A105 RID: 41221
				public static LocString CONTAINER2 = "A specialized air bladder in the Puft's chest cavity stores varying concentrations of gas, allowing it to control its buoyancy and float effortlessly through the air.\n\nCombined with extremely lightweight and elastic skin, the Puft is capable of maintaining flotation indefinitely with negligible energy expenditure. Its orientation and balance, meanwhile, are maintained by counterweighted formations of bone located in its otherwise useless legs.";
			}
		}

		// Token: 0x02001CF8 RID: 7416
		public class ROVER
		{
			// Token: 0x040083D0 RID: 33744
			public static LocString TITLE = "Rover";

			// Token: 0x040083D1 RID: 33745
			public static LocString SUBTITLE = "Scouting Robot";

			// Token: 0x0200247B RID: 9339
			public class BODY
			{
				// Token: 0x0400A106 RID: 41222
				public static LocString CONTAINER1 = "The Rover is a planetary scout robot programmed to land on and mine Planetoids where sending a Duplicant would put them unneccessarily in danger.\n\nRovers are programmed to be very pleasant and social when interacting with other beings. However, an unintended consequence of this programming is that the socialized robots tended to experience the same work slow-downs due to loneliness and low morale.\n\nTo compensate for this, the Rover was programmed to have two distinct personalities it can switch between to have pleasant in-depth conversations with itself during long stints alone.";
			}
		}

		// Token: 0x02001CF9 RID: 7417
		public class SHINEBUG
		{
			// Token: 0x040083D2 RID: 33746
			public static LocString SPECIES_TITLE = "Shine Bugs";

			// Token: 0x040083D3 RID: 33747
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x040083D4 RID: 33748
			public static LocString TITLE = "Shine Bug";

			// Token: 0x040083D5 RID: 33749
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x0200247C RID: 9340
			public class BODY
			{
				// Token: 0x0400A107 RID: 41223
				public static LocString CONTAINER1 = "The bioluminescence of the Shine Bug's body serves the social purpose of finding and communicating with others of its kind.";
			}
		}

		// Token: 0x02001CFA RID: 7418
		public class NEGA
		{
			// Token: 0x040083D6 RID: 33750
			public static LocString TITLE = "Abyss Bug";

			// Token: 0x040083D7 RID: 33751
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x0200247D RID: 9341
			public class BODY
			{
				// Token: 0x0400A108 RID: 41224
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Abyss\" Shine Bug variant</smallcaps>";

				// Token: 0x0400A109 RID: 41225
				public static LocString CONTAINER2 = "The Abyss Shine Bug morph has an unusual genetic mutation causing it to absorb light rather than emit it.";
			}
		}

		// Token: 0x02001CFB RID: 7419
		public class CRYSTAL
		{
			// Token: 0x040083D8 RID: 33752
			public static LocString TITLE = "Radiant Bug";

			// Token: 0x040083D9 RID: 33753
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x0200247E RID: 9342
			public class BODY
			{
				// Token: 0x0400A10A RID: 41226
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Radiant\" Shine Bug variant</smallcaps>";
			}
		}

		// Token: 0x02001CFC RID: 7420
		public class SUNNY
		{
			// Token: 0x040083DA RID: 33754
			public static LocString TITLE = "Sun Bug";

			// Token: 0x040083DB RID: 33755
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x0200247F RID: 9343
			public class BODY
			{
				// Token: 0x0400A10B RID: 41227
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Sun\" Shine Bug variant</smallcaps>";

				// Token: 0x0400A10C RID: 41228
				public static LocString CONTAINER2 = "It is not uncommon for Shine Bugs to mistakenly approach inanimate sources of light in search of a friend.";
			}
		}

		// Token: 0x02001CFD RID: 7421
		public class PLACID
		{
			// Token: 0x040083DC RID: 33756
			public static LocString TITLE = "Azure Bug";

			// Token: 0x040083DD RID: 33757
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002480 RID: 9344
			public class BODY
			{
				// Token: 0x0400A10D RID: 41229
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Azure\" Shine Bug variant</smallcaps>";
			}
		}

		// Token: 0x02001CFE RID: 7422
		public class VITAL
		{
			// Token: 0x040083DE RID: 33758
			public static LocString TITLE = "Coral Bug";

			// Token: 0x040083DF RID: 33759
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002481 RID: 9345
			public class BODY
			{
				// Token: 0x0400A10E RID: 41230
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Coral\" Shine Bug variant</smallcaps>";

				// Token: 0x0400A10F RID: 41231
				public static LocString CONTAINER2 = "It is unwise to touch a Shine Bug's wing blades directly due to the extremely fragile nature of their membranes.";
			}
		}

		// Token: 0x02001CFF RID: 7423
		public class ROYAL
		{
			// Token: 0x040083E0 RID: 33760
			public static LocString TITLE = "Royal Bug";

			// Token: 0x040083E1 RID: 33761
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002482 RID: 9346
			public class BODY
			{
				// Token: 0x0400A110 RID: 41232
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Royal\" Shine Bug variant</smallcaps>";

				// Token: 0x0400A111 RID: 41233
				public static LocString CONTAINER2 = "The Shine Bug can be pet anywhere else along its body, although it is advised that care still be taken due to the generally delicate nature of its exoskeleton.";
			}
		}

		// Token: 0x02001D00 RID: 7424
		public class SLICKSTER
		{
			// Token: 0x040083E2 RID: 33762
			public static LocString SPECIES_TITLE = "Slicksters";

			// Token: 0x040083E3 RID: 33763
			public static LocString SPECIES_SUBTITLE = "Critter Species";

			// Token: 0x040083E4 RID: 33764
			public static LocString TITLE = "Slickster";

			// Token: 0x040083E5 RID: 33765
			public static LocString SUBTITLE = "Domesticable Critter";

			// Token: 0x02002483 RID: 9347
			public class BODY
			{
				// Token: 0x0400A112 RID: 41234
				public static LocString CONTAINER1 = "Slicksters are a unique creature most renowned for their ability to exude hydrocarbon waste that is nearly identical in makeup to crude oil.\n\nThe two tufts atop a Slickster's head are called rhinophores, and help guide the Slickster toward breathable carbon dioxide.";
			}
		}

		// Token: 0x02001D01 RID: 7425
		public class MOLTEN
		{
			// Token: 0x040083E6 RID: 33766
			public static LocString TITLE = "Molten Slickster";

			// Token: 0x040083E7 RID: 33767
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002484 RID: 9348
			public class BODY
			{
				// Token: 0x0400A113 RID: 41235
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Molten\" Slickster variant</smallcaps>";

				// Token: 0x0400A114 RID: 41236
				public static LocString CONTAINER2 = "Slicksters are amicable creatures famous amongst breeders for their good personalities and smiley faces.\n\nSlicksters rarely if ever nip at human handlers, and are considered non-ideal house pets only for the oily mess they involuntarily leave behind wherever they go.";
			}
		}

		// Token: 0x02001D02 RID: 7426
		public class DECOR
		{
			// Token: 0x040083E8 RID: 33768
			public static LocString TITLE = "Longhair Slickster";

			// Token: 0x040083E9 RID: 33769
			public static LocString SUBTITLE = "Critter Morph";

			// Token: 0x02002485 RID: 9349
			public class BODY
			{
				// Token: 0x0400A115 RID: 41237
				public static LocString CONTAINER1 = "<smallcaps>Pictured: \"Longhair\" Slickster variant</smallcaps>";

				// Token: 0x0400A116 RID: 41238
				public static LocString CONTAINER2 = "Positioned on either side of the Major Rhinophores are Minor Rhinophores, which specialize in mechanical reception and detect air pressure around the Slickster. These send signals to the brain to contract or expand its air sacks accordingly.";
			}
		}

		// Token: 0x02001D03 RID: 7427
		public class SWEEPY
		{
			// Token: 0x040083EA RID: 33770
			public static LocString TITLE = "Sweepy";

			// Token: 0x040083EB RID: 33771
			public static LocString SUBTITLE = "Cleaning Robot";

			// Token: 0x02002486 RID: 9350
			public class BODY
			{
				// Token: 0x0400A117 RID: 41239
				public static LocString CONTAINER1 = "The Sweepy is a domesticated sweeping robot programmed to clean solid and liquid debris. The Sweepy Dock will automatically launch the Sweepy, store the debris the robot picks up, and recharge the Sweepy's battery, provided it has been plugged into a power source.\n\nThough the Sweepy can not travel over gaps or uneven ground, it is programmed to feel really bad about this.";
			}
		}

		// Token: 0x02001D04 RID: 7428
		public class B6_AICONTROL
		{
			// Token: 0x040083EC RID: 33772
			public static LocString TITLE = "Re: Objectionable Request";

			// Token: 0x040083ED RID: 33773
			public static LocString TITLE2 = "SUBJECT: Objectionable Request";

			// Token: 0x040083EE RID: 33774
			public static LocString TITLE3 = "SUBJECT: Re: Objectionable Request";

			// Token: 0x040083EF RID: 33775
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x02002487 RID: 9351
			public class BODY
			{
				// Token: 0x0400A118 RID: 41240
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><size=12><alpha=#AA> <jstern@gravitas.nova></size></color>\nFrom: <b>Dr. Broussard</b><size=12><alpha=#AA> <obroussard@gravitas.nova></color></size></smallcaps>\n------------------\n";

				// Token: 0x0400A119 RID: 41241
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Dr. Broussard</b><size=12><alpha=#AA> <obroussard@gravitas.nova></size></color>\nFrom: <b>Director Stern</b><alpha=#AA><size=12> <jstern@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A11A RID: 41242
				public static LocString CONTAINER2 = "<indent=5%>Director,\n\nEngineering has requested the brainmaps of all blueprint subjects for the development of a podlinked software and I am reluctant to oblige.\n\nI believe they are seeking a way to exert temporary control over implanted subjects, and I fear this avenue of research may be ethically unsound.</indent>";

				// Token: 0x0400A11B RID: 41243
				public static LocString CONTAINER3 = "<indent=5%>Doctor,\n\nI understand your concerns, but engineering's newest project was conceived under my supervision.\n\nPlease give them any materials they require to move forward with their research.</indent>";

				// Token: 0x0400A11C RID: 41244
				public static LocString CONTAINER4 = "<indent=5%>You can't be serious, Jacquelyn?</indent>";

				// Token: 0x0400A11D RID: 41245
				public static LocString CONTAINER5 = "<indent=5%>You signed off on cranial chip implantation. Why would this be where you draw the line?\n\nIt would be an invaluable safety measure and protect your printing subjects.</indent>";

				// Token: 0x0400A11E RID: 41246
				public static LocString CONTAINER6 = "<indent=5%>It just gives me a bad feeling.\n\nI can't stop you if you insist on going forward with this, but I'd ask that you remove me from the project.</indent>";

				// Token: 0x0400A11F RID: 41247
				public static LocString SIGNATURE1 = "\n-Dr. Broussard\n<size=11>Bioengineering Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400A120 RID: 41248
				public static LocString SIGNATURE2 = "\n-Director Stern\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D05 RID: 7429
		public class B51_ARTHISTORYREQUEST
		{
			// Token: 0x040083F0 RID: 33776
			public static LocString TITLE = "Re: Implant Database Request";

			// Token: 0x040083F1 RID: 33777
			public static LocString TITLE2 = "Implant Database Request";

			// Token: 0x040083F2 RID: 33778
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x02002488 RID: 9352
			public class BODY
			{
				// Token: 0x0400A121 RID: 41249
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><size=12><alpha=#AA> <jstern@gravitas.nova></color></size>\nFrom: <b>Dr. Broussard</b><size=12><alpha=#AA> <obroussard@gravitas.nova></color></size></smallcaps>\n------------------\n";

				// Token: 0x0400A122 RID: 41250
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Dr. Broussard</b><alpha=#AA><size=12> <obroussard@gravitas.nova></size></color>\nFrom: <b>Director Stern</b><alpha=#AA><size=12> <jstern@gravitas.nova></color></smallcaps></size>\n------------------\n";

				// Token: 0x0400A123 RID: 41251
				public static LocString CONTAINER2 = "<indent=5%>Director,\n\nI have been thinking, and it occurs to me that our subjects will likely travel outside our range of radio contact when establishing new colonies.\n\nColonies travel into the cosmos as representatives of humanity, and I believe it is our duty to preserve the planet's non-scientific knowledge in addition to practical information.\n\nI would like to make a formal request that comprehensive arts and cultural histories make their way onto the microchip databases.</indent>";

				// Token: 0x0400A124 RID: 41252
				public static LocString CONTAINER3 = "<indent=5%>Doctor,\n\nIf there is room available after the necessary scientific and survival knowledge has been uploaded, I will see what I can do.</indent>";

				// Token: 0x0400A125 RID: 41253
				public static LocString SIGNATURE1 = "\n-Dr. Broussard\n<size=11>Bioengineering Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400A126 RID: 41254
				public static LocString SIGNATURE2 = "\n-Director Stern\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D06 RID: 7430
		public class A4_ATOMICONRECRUITMENT
		{
			// Token: 0x040083F3 RID: 33779
			public static LocString TITLE = "Results from Atomicon";

			// Token: 0x040083F4 RID: 33780
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x02002489 RID: 9353
			public class BODY
			{
				// Token: 0x0400A127 RID: 41255
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><alpha=#AA><size=12> <jstern@gravitas.nova></size></color>\nFrom: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></smallcaps>\n------------------\n";

				// Token: 0x0400A128 RID: 41256
				public static LocString CONTAINER2 = "<indent=5%>Director,\n\nEverything went well. Broussard was reluctant at first, but she has little alternative given the nature of her work and the recent turn of events.\n\nShe can begin at your convenience.</indent>";

				// Token: 0x0400A129 RID: 41257
				public static LocString SIGNATURE1 = "\nXOXO,\nDr. Jones\n\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D07 RID: 7431
		public class A3_DEVONSBLOG
		{
			// Token: 0x040083F5 RID: 33781
			public static LocString TITLE = "Re: devon's bloggg";

			// Token: 0x040083F6 RID: 33782
			public static LocString TITLE2 = "SUBJECT: devon's bloggg";

			// Token: 0x040083F7 RID: 33783
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x0200248A RID: 9354
			public class BODY
			{
				// Token: 0x0400A12A RID: 41258
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Summers</b><alpha=#AA><size=12> <jsummers@gravitas.nova></size></color>\nFrom: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A12B RID: 41259
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></size></color>\nFrom: <b>Dr. Summers</b><alpha=#AA><size=12> <jsummers@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A12C RID: 41260
				public static LocString CONTAINER1 = "<indent=5%>Oh my goddd I found out today that Devon's one of those people who takes pictures of their food and uploads them to some boring blog somewhere.\n\nYou HAVE to come to lunch with us and see, they spend so long taking pictures that the food gets cold and they have to ask the waiter to reheat it. It's SO FUNNY.</indent>";

				// Token: 0x0400A12D RID: 41261
				public static LocString CONTAINER2 = "<indent=5%>Oh cool, Devon's writing a new post for <i>Toast of the Town</i>? I'd love to tag along and \"see how the sausage is made\" so to speak, haha.\n\nI'll see you guys in a bit! :)</indent>";

				// Token: 0x0400A12E RID: 41262
				public static LocString CONTAINER3 = "<indent=5%>WAIT, Joshua, you read Devon's blog??</indent>";

				// Token: 0x0400A12F RID: 41263
				public static LocString SIGNATURE1 = "\nXOXO,\nDr. Jones\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400A130 RID: 41264
				public static LocString SIGNATURE2 = "\n-Dr. Summers\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D08 RID: 7432
		public class C5_ENGINEERINGCANDIDATE
		{
			// Token: 0x040083F8 RID: 33784
			public static LocString TITLE = "RE: Engineer Candidate?";

			// Token: 0x040083F9 RID: 33785
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x0200248B RID: 9355
			public class BODY
			{
				// Token: 0x0400A131 RID: 41265
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><size=12><alpha=#AA> <jstern@gravitas.nova></size></color></smallcaps>\nFrom: <b>[REDACTED]</b>\n------------------\n";

				// Token: 0x0400A132 RID: 41266
				public static LocString CONTAINER3 = "<indent=5%>Director, I think I've found the perfect engineer candidate to design our small-scale colony machines.\n-----------------------------------------------------------------------------------------------------\n</indent>";

				// Token: 0x0400A133 RID: 41267
				public static LocString CONTAINER4 = "<indent=10%><smallcaps><b>Bringing Creative Workspace Ideas into the Industrial Setting</b>\n\nMichael E.E. Perlmutter is a rising star in the world industrial design, making a name for himself by cooking up playful workspaces for a work force typically left out of the creative conversation.\n\n\"Ergodynamic chairs have been done to death,\" says Perlmutter. \"What I'm interested in is redesigning the industrial space. There's no reason why a machine can't convey a sense of whimsy.\"\n\nIt's this philosophy that has launched Perlmutter to the top of a very short list of hot new industrial designers.</indent></smallcaps>";

				// Token: 0x0400A134 RID: 41268
				public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>Human Resources Coordinator\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D09 RID: 7433
		public class B7_FRIENDLYEMAIL
		{
			// Token: 0x040083FA RID: 33786
			public static LocString TITLE = "Hiiiii!";

			// Token: 0x040083FB RID: 33787
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x0200248C RID: 9356
			public class BODY
			{
				// Token: 0x0400A135 RID: 41269
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Techna</b><alpha=#AA><size=12> <ntechna@gravitas.nova></size></color>\nFrom: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A136 RID: 41270
				public static LocString CONTAINER1 = "<indent=5%>Omg, <i>hi</i> Nikola!\n\nHave you heard about the super weird thing that's been happening in the kitchen lately? Joshua's lunch has disappeared from the fridge like, every day for the past week!\n\nThere's a <i>ton</i> of cameras in that room too but all anyone can see is like this spiky blond hair behind the fridge door.\n\nSo <i>weird</i> right? ;)\n\nAnyway, totally unrelated, but our computer system's been having this <i>glitch</i> where datasets going back for like half a year get <i>totally</i> wiped for all employees with the initials \"N.T.\"\n\nIsn't it weird how specific that is? Don't worry though! I'm sure I'll have it fixed before it affects any of <i>your</i> work.\n\nByeee!</indent>";

				// Token: 0x0400A137 RID: 41271
				public static LocString SIGNATURE1 = "\nXOXO,\nDr. Jones\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D0A RID: 7434
		public class B1_HOLLANDSDOG
		{
			// Token: 0x040083FC RID: 33788
			public static LocString TITLE = "Re: dr. holland's dog";

			// Token: 0x040083FD RID: 33789
			public static LocString TITLE2 = "dr. holland's dog";

			// Token: 0x040083FE RID: 33790
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x0200248D RID: 9357
			public class BODY
			{
				// Token: 0x0400A138 RID: 41272
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Summers</b><size=10><alpha=#AA> <jsummers@gravitas.nova></size></color>\nFrom: <b>Dr. Jones</b><alpha=#AA><size=10> <ejones@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A139 RID: 41273
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Dr. Jones</b><alpha=#AA><size=10> <ejones@gravitas.nova></size></color>\nFrom: <b>Dr. Summers</b><size=10><alpha=#AA> <jsummers@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A13A RID: 41274
				public static LocString CONTAINER1 = "<indent=5%>OMIGOD, every time I go into the break room now I get ambushed by Dr. Holland and he traps me in a 20 minute conversation about his new dog.\n\nLike, I GET it! Your puppy is cute! Why do you have like 400 different pictures of it on your phone, FROM THE SAME ANGLE?!\n\nSO annoying.</indent>";

				// Token: 0x0400A13B RID: 41275
				public static LocString CONTAINER2 = "<indent=5%>Haha, I think it's nice, he really loves his dog. Oh! Did I show you the thing my cat did last night? She always falls asleep on my bed but this time she sprawled out on her back and her little tongue was poking out! So cute.\n\n<color=#F44A47>[BROKEN IMAGE]</color>\n<alpha=#AA>[121 MISSING ATTACHMENTS]</color></indent>";

				// Token: 0x0400A13C RID: 41276
				public static LocString CONTAINER3 = "<indent=5%><i><b>UGHHHHHHHH!</b></i>\nYou're the worst!</indent>";

				// Token: 0x0400A13D RID: 41277
				public static LocString SIGNATURE1 = "\nXOXO,\nDr. Jones\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400A13E RID: 41278
				public static LocString SIGNATURE2 = "\n-Dr. Summers\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D0B RID: 7435
		public class JOURNALISTREQUEST
		{
			// Token: 0x040083FF RID: 33791
			public static LocString TITLE = "Re: Call me";

			// Token: 0x04008400 RID: 33792
			public static LocString TITLE2 = "Call me";

			// Token: 0x04008401 RID: 33793
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x0200248E RID: 9358
			public class BODY
			{
				// Token: 0x0400A13F RID: 41279
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Olowe</b><size=10><alpha=#AA> <aolowe@gravitas.nova></size></color>\nFrom: <b>Quinn Kelly</b><alpha=#AA><size=10> <editor@stemscoop.news></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A140 RID: 41280
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>[BCC: all]</b><alpha=#AA><size=10> </size></color>\nFrom: <b>Quinn Kelly</b><alpha=#AA><size=10> <editor@stemscoop.news></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A141 RID: 41281
				public static LocString CONTAINER1 = "<indent=5%>Dear colleagues, friends and community members,\n\nAfter nine deeply fulfilling years as editor of The STEM Scoop, I am stepping down to spend more time with my family.\n\nPlease give a warm welcome to Dorian Hearst, who will be taking over editorial management duties effective immediately.</indent>";

				// Token: 0x0400A142 RID: 41282
				public static LocString CONTAINER2 = "<indent=5%>I don't know how you pulled it off, but Stern's office just called the paper and granted me an exclusive...and a tour of the Gravitas Facility. I owe you a beer. No - a case of beer. Six cases of beer!\n\nSeriously, thank you. I know you're in a difficult position but you've done the right thing. See you on Tuesday.</indent>";

				// Token: 0x0400A143 RID: 41283
				public static LocString CONTAINER3 = "<indent=5%>I waited at the fountain for four hours. Where were you? This story is going to be huge. Call me.</indent>";

				// Token: 0x0400A144 RID: 41284
				public static LocString CONTAINER4 = "<indent=5%>Dr. Olowe,\n\nI'm sorry - I know ambushing you at your home last night was a bad idea. But something is happening at Gravitas, and people need to know. Please call me.</indent>";

				// Token: 0x0400A145 RID: 41285
				public static LocString SIGNATURE1 = "\n-Q\n------------------\n";

				// Token: 0x0400A146 RID: 41286
				public static LocString SIGNATURE2 = "\nAll the best,\nQuinn Kelly\n------------------\n";
			}
		}

		// Token: 0x02001D0C RID: 7436
		public class B50_MEMORYCHIP
		{
			// Token: 0x04008402 RID: 33794
			public static LocString TITLE = "Duplicant Memory Solution";

			// Token: 0x04008403 RID: 33795
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x0200248F RID: 9359
			public class BODY
			{
				// Token: 0x0400A147 RID: 41287
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><alpha=#AA><size=12> <jstern@gravitas.nova></size></color>\nFrom: <b>[REDACTED]</b></smallcaps>\n------------------\n";

				// Token: 0x0400A148 RID: 41288
				public static LocString CONTAINER2 = "<indent=5%>Director,\n\nI had a thought about how to solve your Duplicant memory problem.\n\nRather than attempt to access the subject's old memories, what if we were to embed all necessary information for colony survival into the printing process itself?\n\nThe amount of data engineering can store has grown exponentially over the last year. We should take advantage of the technological development.</indent>";

				// Token: 0x0400A149 RID: 41289
				public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>Engineering Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D0D RID: 7437
		public class MISSINGNOTES
		{
			// Token: 0x04008404 RID: 33796
			public static LocString TITLE = "Re: Missing notes";

			// Token: 0x04008405 RID: 33797
			public static LocString TITLE2 = "SUBJECT: Missing notes";

			// Token: 0x04008406 RID: 33798
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x02002490 RID: 9360
			public class BODY
			{
				// Token: 0x0400A14A RID: 41290
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></size></color>\nFrom: <b>Dr. Olowe</b><alpha=#AA><size=12> <aolowe@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A14B RID: 41291
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Dr. Olowe</b><alpha=#AA><size=12> <aolowe@gravitas.nova></size></color>\nFrom: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A14C RID: 41292
				public static LocString EMAILHEADER3 = "<smallcaps>To: <b>Dr. Olowe</b><alpha=#AA><size=12> <aolowe@gravitas.nova></size></color>\nFrom: <b>Director Stern</b><alpha=#AA><size=12> <jstern@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A14D RID: 41293
				public static LocString CONTAINER1 = "<indent=5%>Hello Dr. Jones,\n\nHope you are well. Sorry to bother you- I believe that someone may have inappropriately accessed my computer.\n\nWhen I was logging in this morning, the \"last log-in\" pop-up indicated that my computer had been accessed at 2 a.m. My last actual log-in was 6 p.m. And some of my files have gone missing.\n\nThe privacy of my work is paramount. Would it be possible to have someone take a look, please?</indent>";

				// Token: 0x0400A14E RID: 41294
				public static LocString CONTAINER2 = "<indent=5%>OMG Amari, you're so dramatic!! It's probably just a glitch from the system network upgrade. Nobody can even get into your office without going through the new hand scanners.\n\nPS: Everybody's work is super private, not just yours ;)</indent>";

				// Token: 0x0400A14F RID: 41295
				public static LocString CONTAINER3 = "<indent=5%>Dear Dr. Jones,\nI'm so sorry to bother you again...it's just that I'm absolutely certain that someone has been interfering with my files.\n\nI've noticed several discrepancies since last week's \"glitch.\" For example, responses to my recent employee survey on workplace satisfaction and safety were decrypted, and significant portions of my data and research notes have been erased. I'm even missing a few e-mails.\n\nIt's all quite alarming. Could you or Dr. Summers please investigate this when you have a moment?\n\nThank you so much,\n\n</indent>";

				// Token: 0x0400A150 RID: 41296
				public static LocString CONTAINER4 = "<indent=5%>The files in question were a security risk, and were disposed of accordingly.\n\nAs for your emails: the NDA you signed was very clear about how to handle requests from members of the media.\n\nSee me in my office.</indent>";

				// Token: 0x0400A151 RID: 41297
				public static LocString SIGNATURE1 = "\n-Dr. Olowe\n<size=11>Industrial-Organizational Psychologist\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400A152 RID: 41298
				public static LocString SIGNATURE2 = "\nXOXO,\nDr. Jones\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400A153 RID: 41299
				public static LocString SIGNATURE3 = "\n-Director Stern\n<size=11>\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D0E RID: 7438
		public class B4_MYPENS
		{
			// Token: 0x04008407 RID: 33799
			public static LocString TITLE = "SUBJECT: MY PENS";

			// Token: 0x04008408 RID: 33800
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x02002491 RID: 9361
			public class BODY
			{
				// Token: 0x0400A154 RID: 41300
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>ALL</b>\nFrom: <b>[REDACTED]</b></color></smallcaps>\n------------------\n";

				// Token: 0x0400A155 RID: 41301
				public static LocString CONTAINER2 = "<indent=5%>To whomever is stealing the glitter pens off of my desk:\n\n<i>CONSIDER THIS YOUR LAST WARNING!</i></indent>";

				// Token: 0x0400A156 RID: 41302
				public static LocString SIGNATURE1 = "\nXOXO,\n[REDACTED]\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D0F RID: 7439
		public class A7_NEWEMPLOYEE
		{
			// Token: 0x04008409 RID: 33801
			public static LocString TITLE = "Welcome, New Employee";

			// Token: 0x0400840A RID: 33802
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x02002492 RID: 9362
			public class BODY
			{
				// Token: 0x0400A157 RID: 41303
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>All</b>\nFrom: <b>[REDACTED]</b></smallcaps>\n------------------\n";

				// Token: 0x0400A158 RID: 41304
				public static LocString CONTAINER2 = "<indent=5%>Attention Gravitas Facility personnel;\n\nPlease welcome our newest staff member, Olivia Broussard, PhD.\n\nDr. Broussard will be leading our upcoming genetics project and has been installed in our bioengineering department.\n\nBe sure to offer her our warmest welcome.</indent>";

				// Token: 0x0400A159 RID: 41305
				public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>Personnel Coordinator\nThe Gravitas Facility</indent>\n------------------\n";
			}
		}

		// Token: 0x02001D10 RID: 7440
		public class A6_NEWSECURITY2
		{
			// Token: 0x0400840B RID: 33803
			public static LocString TITLE = "New Security System?";

			// Token: 0x0400840C RID: 33804
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x02002493 RID: 9363
			public class BODY
			{
				// Token: 0x0400A15A RID: 41306
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>[REDACTED]</b>\nFrom: <b>[REDACTED]</b></color></smallcaps>\n------------------\n";

				// Token: 0x0400A15B RID: 41307
				public static LocString CONTAINER2 = "<indent=5%>So, the facility is introducing this new security system that scans your hand to unlock the doors. My question is, what exactly are they scanning?\n\nThe folks in engineering say the door device doesn't look like a fingerprint scanner, but the duo working over in bioengineering won't comment at all.\n\nI can't say I like it.</indent>";

				// Token: 0x0400A15C RID: 41308
				public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D11 RID: 7441
		public class A8_NEWSECURITY3
		{
			// Token: 0x0400840D RID: 33805
			public static LocString TITLE = "They Stole Our DNA";

			// Token: 0x0400840E RID: 33806
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x02002494 RID: 9364
			public class BODY
			{
				// Token: 0x0400A15D RID: 41309
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>[REDACTED]</b>\nFrom: <b>[REDACTED]</b></smallcaps>\n------------------\n";

				// Token: 0x0400A15E RID: 41310
				public static LocString CONTAINER2 = "<indent=5%>I'm almost certain now that the Facility's stolen our genetic information.\n\nForty-odd employees would make for mighty convenient lab rats, and even if we discovered what Gravitas did, we wouldn't have a lot of legal options. We can't exactly go to the public given the nature of our work.\n\nI can't stop thinking about what sort of experiments they might be conducting on my DNA, but I have to keep my mouth shut.\n\nI can't risk losing my job.</indent>";

				// Token: 0x0400A15F RID: 41311
				public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D12 RID: 7442
		public class B8_POLITEREQUEST
		{
			// Token: 0x0400840F RID: 33807
			public static LocString TITLE = "Polite Request";

			// Token: 0x04008410 RID: 33808
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x02002495 RID: 9365
			public class BODY
			{
				// Token: 0x0400A160 RID: 41312
				public static LocString EMAILHEADER = "<smallcaps>To: <b>All</b>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color></smallcaps>\n------------------\n";

				// Token: 0x0400A161 RID: 41313
				public static LocString CONTAINER1 = "<indent=5%>To whoever is entering Director Stern's office to move objects on her desk one inch to the left, please desist as she finds it quite unnerving.</indent>";

				// Token: 0x0400A162 RID: 41314
				public static LocString SIGNATURE = "\nThank-you,\n-Admin\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D13 RID: 7443
		public class A0_PRELIMINARYCALCULATIONS
		{
			// Token: 0x04008411 RID: 33809
			public static LocString TITLE = "Preliminary Calculations";

			// Token: 0x04008412 RID: 33810
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x02002496 RID: 9366
			public class BODY
			{
				// Token: 0x0400A163 RID: 41315
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>[REDACTED]</b>\nFrom: <b>[REDACTED]</b></color></smallcaps>\n------------------\n";

				// Token: 0x0400A164 RID: 41316
				public static LocString CONTAINER2 = "<indent=5%>Director,\n\nEven with dramatic optimization, we can't fit the massive volume of resources needed for a colony seed aboard the craft. Not even when calculating for a very small interplanetary travel duration.\n\nSome serious changes are gonna have to be made for this to work.</indent>";

				// Token: 0x0400A165 RID: 41317
				public static LocString SIGNATURE1 = "\nXOXO,\n[REDACTED]\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D14 RID: 7444
		public class B4_REMYPENS
		{
			// Token: 0x04008413 RID: 33811
			public static LocString TITLE = "Re: MY PENS";

			// Token: 0x04008414 RID: 33812
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x02002497 RID: 9367
			public class BODY
			{
				// Token: 0x0400A166 RID: 41318
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>ALL</b>\nFrom: <b>Admin</b><size=12><alpha=#AA> <admin@gravitas.nova></color></size></smallcaps>\n------------------\n";

				// Token: 0x0400A167 RID: 41319
				public static LocString CONTAINER2 = "<indent=5%>We would like to remind staff not to use the CC: All function for intra-office issues.\n\nIn the event of disputes or disruptive work behavior within the facility, please speak to HR directly.\n\nWe thank-you for your restraint.</indent>";

				// Token: 0x0400A168 RID: 41320
				public static LocString SIGNATURE1 = "\n-Admin\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D15 RID: 7445
		public class B3_RETEMPORALBOWUPDATE
		{
			// Token: 0x04008415 RID: 33813
			public static LocString TITLE = "RE: To Otto (Spec Changes)";

			// Token: 0x04008416 RID: 33814
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x02002498 RID: 9368
			public class BODY
			{
				// Token: 0x0400A169 RID: 41321
				public static LocString TITLEALT = "To Otto (Spec Changes)";

				// Token: 0x0400A16A RID: 41322
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Sklodowska</b><size=10><alpha=#AA> <msklodowska@gravitas.nova></size></color>\nFrom: <b>Mr. Kraus</b><alpha=#AA><size=10> <okraus@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A16B RID: 41323
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Mr. Kraus</b><alpha=#AA><size=10> <okraus@gravitas.nova></size></color>\nFrom: <b>Dr. Sklodowska</b><size=10><alpha=#AA> <msklodowska@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A16C RID: 41324
				public static LocString CONTAINER1 = "Thanks Doctor.\n\nPS, if you hit the \"Reply\" button instead of composing a new e-mail it makes it easier for people to tell what you're replying to. :)\n\nI appreciate it!\n\nMr. Kraus\n<size=11>Physics Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400A16D RID: 41325
				public static LocString CONTAINER2 = "Try not to take it too personally, it's probably just stress.\n\nThe Facility started going through a major overhaul not long before you got here, so I imagine the Director is having quite a time getting it all sorted out.\n\nThings will calm down once all the new departments are settled.\n\nDr. Sklodowska\n<size=11>Physics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D16 RID: 7446
		public class A1_RESEARCHGIANTARTICLE
		{
			// Token: 0x04008417 RID: 33815
			public static LocString TITLE = "Re: Have you seen this?";

			// Token: 0x04008418 RID: 33816
			public static LocString TITLE2 = "SUBJECT: Have you seen this?";

			// Token: 0x04008419 RID: 33817
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x02002499 RID: 9369
			public class BODY
			{
				// Token: 0x0400A16E RID: 41326
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Director Stern</b><size=12><alpha=#AA> <jstern@gravitas.nova></size></color>\nFrom: <b>[REDACTED]</b></smallcaps>\n------------------\n";

				// Token: 0x0400A16F RID: 41327
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>[REDACTED]</b>\nFrom: <b>Director Stern</b><size=12><alpha=#AA> <jstern@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A170 RID: 41328
				public static LocString CONTAINER2 = "<indent=5%>Please pay it no mind. If any of these journals reach out to you, deny comment.</indent>";

				// Token: 0x0400A171 RID: 41329
				public static LocString CONTAINER3 = "<indent=5%>Director, are you aware of the articles that have been cropping up about us lately?</indent>";

				// Token: 0x0400A172 RID: 41330
				public static LocString CONTAINER4 = "<indent=10%><color=#F44A47>>[BROKEN LINK]</color> <alpha=#AA><smallcaps>the gravitas facility: questionable rise of a research giant</smallcaps></indent></color>";

				// Token: 0x0400A173 RID: 41331
				public static LocString SIGNATURE1 = "\n[REDACTED]\n<size=11>Personnel Coordinator\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400A174 RID: 41332
				public static LocString SIGNATURE2 = "\n-Director Stern\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D17 RID: 7447
		public class B2_TEMPORALBOWUPDATE
		{
			// Token: 0x0400841A RID: 33818
			public static LocString TITLE = "Spec Changes";

			// Token: 0x0400841B RID: 33819
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x0200249A RID: 9370
			public class BODY
			{
				// Token: 0x0400A175 RID: 41333
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Sklodowska</b><size=10><alpha=#AA> <msklodowska@gravitas.nova></size></color>\nFrom: <b>Mr. Kraus</b><alpha=#AA><size=10> <okraus@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A176 RID: 41334
				public static LocString CONTAINER2 = "Dr. Sklodowska, could I ask you to forward me the new spec changes to the Temporal Bow?\n\nThe Director completely ignored me when I asked for a project update this morning. She walked right past me in the hall - I didn't realize I was that far down on the food chain. :(\n\nMr. Kraus\nPhysics Department\nThe Gravitas Facility";
			}
		}

		// Token: 0x02001D18 RID: 7448
		public class A5_THEJANITOR
		{
			// Token: 0x0400841C RID: 33820
			public static LocString TITLE = "Re: omg the janitor";

			// Token: 0x0400841D RID: 33821
			public static LocString TITLE2 = "SUBJECT: Re: omg the janitor";

			// Token: 0x0400841E RID: 33822
			public static LocString TITLE3 = "SUBJECT: omg the janitor";

			// Token: 0x0400841F RID: 33823
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x0200249B RID: 9371
			public class BODY
			{
				// Token: 0x0400A177 RID: 41335
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Dr. Summers</b><size=12><alpha=#AA> <jsummers@gravitas.nova></color></size>\nFrom: <b>Dr. Jones</b><size=12><alpha=#AA> <ejones@gravitas.nova></color></size></smallcaps>\n------------------\n";

				// Token: 0x0400A178 RID: 41336
				public static LocString EMAILHEADER2 = "<smallcaps>To: <b>Dr. Jones</b><size=12><alpha=#AA> <ejones@gravitas.nova></color></size>\nFrom: <b>Dr. Summers</b><size=12><alpha=#AA> <jsummers@gravitas.nova></color></size></smallcaps>\n------------------\n";

				// Token: 0x0400A179 RID: 41337
				public static LocString CONTAINER2 = "<indent=5%><i>Pfft,</i> whatever.</indent>";

				// Token: 0x0400A17A RID: 41338
				public static LocString CONTAINER3 = "<indent=5%>Aw, he's really nice if you get to know him though. Really dependable too. One time I busted a wheel off my office chair and he got me a new one in like, two minutes. I think he's just sweaty because he works so hard.</indent>";

				// Token: 0x0400A17B RID: 41339
				public static LocString CONTAINER4 = "<indent=5%>OMIGOSH have you seen our building's janitor? He totally smells and he has sweat stains under his armpits like EVERY time I see him. SO embarrassing.</indent>";

				// Token: 0x0400A17C RID: 41340
				public static LocString SIGNATURE1 = "\nXOXO,\nDr. Jones\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";

				// Token: 0x0400A17D RID: 41341
				public static LocString SIGNATURE2 = "\n-Dr. Summers\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D19 RID: 7449
		public class A2_THERMODYNAMICLAWS
		{
			// Token: 0x04008420 RID: 33824
			public static LocString TITLE = "The Laws of Thermodynamics";

			// Token: 0x04008421 RID: 33825
			public static LocString SUBTITLE = "UNENCRYPTED";

			// Token: 0x0200249C RID: 9372
			public class BODY
			{
				// Token: 0x0400A17E RID: 41342
				public static LocString EMAILHEADER1 = "<smallcaps>To: <b>Mr. Kraus</b><alpha=#AA><size=12> <okraus@gravitas.nova></size></color>\nFrom: <b>Dr. Jones</b><alpha=#AA><size=12> <ejones@gravitas.nova></size></color></smallcaps>\n------------------\n";

				// Token: 0x0400A17F RID: 41343
				public static LocString CONTAINER1 = "<indent=5%><i>Hello</i> Mr. Kraus!\n\nI was just e-mailing you after our little chat today to pass along something you might like to read - I think you'll find it super useful in your research!\n\n</indent>";

				// Token: 0x0400A180 RID: 41344
				public static LocString CONTAINER2 = "<indent=10%><b>FIRST LAW</b></indent>\n<indent=15%>Energy can neither be created or destroyed, only change forms.</indent>";

				// Token: 0x0400A181 RID: 41345
				public static LocString CONTAINER3 = "<indent=10%><b>SECOND LAW</b></indent>\n<indent=15%>Entropy in an isolated system that is not in equilibrium tends to increase over time, approaching the maximum value at equilibrium.</indent>";

				// Token: 0x0400A182 RID: 41346
				public static LocString CONTAINER4 = "<indent=10%><b>THIRD LAW</b></indent>\n<indent=15%>Entropy in a system approaches a constant minimum as temperature approaches absolute zero.</indent>";

				// Token: 0x0400A183 RID: 41347
				public static LocString CONTAINER5 = "<indent=10%><b>ZEROTH LAW</b></indent>\n<indent=15%>If two thermodynamic systems are in thermal equilibrium with a third, then they are in thermal equilibrium with each other.</indent>";

				// Token: 0x0400A184 RID: 41348
				public static LocString CONTAINER6 = "<indent=5%>\nIf this is too complicated for you, you can come by to chat. I'd be <i>thrilled</i> to answer your questions. ;)</indent>";

				// Token: 0x0400A185 RID: 41349
				public static LocString SIGNATURE1 = "\nXOXO,\nDr. Jones\n<size=11>Information and Statistics Department\nThe Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D1A RID: 7450
		public class TIMEOFFAPPROVED
		{
			// Token: 0x04008422 RID: 33826
			public static LocString TITLE = "Vacation Request Approved";

			// Token: 0x04008423 RID: 33827
			public static LocString TITLE2 = "SUBJECT: Vacation Request Approved";

			// Token: 0x04008424 RID: 33828
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x0200249D RID: 9373
			public class BODY
			{
				// Token: 0x0400A186 RID: 41350
				public static LocString EMAILHEADER = "<smallcaps>To: <b>Dr. Ross</b><size=12><alpha=#AA> <dross@gravitas.nova></size></color>\nFrom: <b>Admin</b><size=12><alpha=#AA> <admin@gravitas.nova></color></size></smallcaps>\n------------------\n";

				// Token: 0x0400A187 RID: 41351
				public static LocString CONTAINER = "<indent=5%><b>Vacation Request Granted</b>\nGood luck, Devon!\n\n<alpha=#AA><smallcaps><indent=10%> Vacation Request [May 18th-20th]\nReason: Time off request for attendance of the Blogjam Awards (\"Toast of the Town\" nominated in the Freshest Food Blog category).</indent></smallcaps></color></indent>";

				// Token: 0x0400A188 RID: 41352
				public static LocString SIGNATURE = "\n-Admin\n<size=11>The Gravitas Facility</size>\n------------------\n";
			}
		}

		// Token: 0x02001D1B RID: 7451
		public class BASIC_FABRIC
		{
			// Token: 0x04008425 RID: 33829
			public static LocString TITLE = "Reed Fiber";

			// Token: 0x04008426 RID: 33830
			public static LocString SUBTITLE = "Textile Ingredient";

			// Token: 0x0200249E RID: 9374
			public class BODY
			{
				// Token: 0x0400A189 RID: 41353
				public static LocString CONTAINER1 = "A ball of raw cellulose harvested from a Thimble Reed.\n\nIt is used in the production of " + UI.FormatAsLink("Clothing", "EQUIPMENT") + " and textiles.";
			}
		}

		// Token: 0x02001D1C RID: 7452
		public class LUMBER
		{
			// Token: 0x04008427 RID: 33831
			public static LocString TITLE = "Lumber";

			// Token: 0x04008428 RID: 33832
			public static LocString SUBTITLE = "Renewable Resource";

			// Token: 0x0200249F RID: 9375
			public class BODY
			{
				// Token: 0x0400A18A RID: 41354
				public static LocString CONTAINER1 = string.Concat(new string[]
				{
					"Lumber is harvested from ",
					UI.FormatAsLink("Arbor Trees", "FOREST_TREE"),
					" and ",
					UI.FormatAsLink("Oakshells", "CRABWOOD"),
					"."
				});
			}
		}

		// Token: 0x02001D1D RID: 7453
		public class SWAMPLILYFLOWER
		{
			// Token: 0x04008429 RID: 33833
			public static LocString TITLE = "Balm Lily Flower";

			// Token: 0x0400842A RID: 33834
			public static LocString SUBTITLE = "Medicinal Herb";

			// Token: 0x020024A0 RID: 9376
			public class BODY
			{
				// Token: 0x0400A18B RID: 41355
				public static LocString CONTAINER1 = "Balm Lily Flowers bloom on " + UI.FormatAsLink("Balm Lily", "SWAMPLILY") + " plants.\n\nThey have a wide range of medicinal applications, and have been shown to be a particularly effective antidote for respiratory illnesses.\n\nThe intense perfume emitted by their vivid petals is best described as \"dizzying.\"";
			}
		}

		// Token: 0x02001D1E RID: 7454
		public class CRYOTANKWARNINGS
		{
			// Token: 0x0400842B RID: 33835
			public static LocString TITLE = "CRYOTANK SAFETY";

			// Token: 0x0400842C RID: 33836
			public static LocString SUBTITLE = "IMPORTANT OPERATING INSTRUCTIONS FOR THE CRYOTANK 3000";

			// Token: 0x020024A1 RID: 9377
			public class BODY
			{
				// Token: 0x0400A18C RID: 41356
				public static LocString CONTAINER1 = "    • Do not leave the contents of the Cryotank 3000 unattended unless an apocalyptic disaster has left you no choice.\n\n    • Ensure that the Cryotank 3000 has enough battery power to remain active for at least 6000 years.\n\n    • Do not attempt to defrost the contents of the Cryotank 3000 while it is submerged in molten hot lava.\n\n    • Use only a qualified Gravitas Cryotank repair facility to repair your Cryotank 3000. Attempting to service the device yourself will void the warranty.\n\n    • Do not put food in the Cryotank 3000. The Cryotank 3000 is not a refrigerator.\n\n    • Do not allow children to play in the Cryotank 3000. The Cryotank 3000 is not a toy.\n\n    • While the Cryotank 3000 is able to withstand a nuclear blast, Gravitas and its subsidiaries are not responsible for what may happen in the resulting nuclear fallout.\n\n    • Wait at least 5 minutes after being unfrozen from the Cryotank 3000 before operating heavy machinery.\n\n    • Each Cryotank 3000 is good for only one use.\n\n";
			}
		}

		// Token: 0x02001D1F RID: 7455
		public class EVACUATION
		{
			// Token: 0x0400842D RID: 33837
			public static LocString TITLE = "! EVACUATION NOTICE !";

			// Token: 0x0400842E RID: 33838
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x020024A2 RID: 9378
			public class BODY
			{
				// Token: 0x0400A18D RID: 41357
				public static LocString CONTAINER1 = "<smallcaps>Attention all Gravitas personnel\n\nEvacuation protocol in effect\n\nReactor meltdown in bioengineering imminent\n\nRemain calm and proceed to emergency exits\n\nDo not attempt to use elevators</smallcaps>";
			}
		}

		// Token: 0x02001D20 RID: 7456
		public class C7_FIRSTCOLONY
		{
			// Token: 0x0400842F RID: 33839
			public static LocString TITLE = "Director's Notes";

			// Token: 0x04008430 RID: 33840
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024A3 RID: 9379
			public class BODY
			{
				// Token: 0x0400A18E RID: 41358
				public static LocString CONTAINER1 = "The first experiments with establishing a colony off planet were an unmitigated disaster. Without outside help, our current Artificial Intelligence was completely incapable of making the kind of spontaneous decisions needed to deal with unforeseen circumstances. Additionally, the colony subjects lacked the forethought to even build themselves toilet facilities, even after soiling themselves repeatedly.\n\nWhile initial experiments in a lab setting were encouraging, our latest operation on non-Terra soil revealed some massive inadequacies to our system. If this idea is ever going to work, we will either need to drastically improve the AI directing the subjects, or improve the brains of our Duplicants to the point where they possess higher cognitive functions.\n\nGiven the disastrous complications that I could foresee arising if our Duplicants were made less supplicant, I'm leaning toward a push to improve our Artificial Intelligence.\n\nMeanwhile, we will have to send a clean-up crew to destroy all evidence of our little experiment beneath the Ceres' surface. We can't risk anyone discovering the remnants of our failed colony, even if that's unlikely to happen for another few decades at least.\n\n(Sometimes it boggles my mind how much further behind Gravitas the rest of the world is.)";
			}
		}

		// Token: 0x02001D21 RID: 7457
		public class A8_FIRSTSUCCESS
		{
			// Token: 0x04008431 RID: 33841
			public static LocString TITLE = "Encouraging Results";

			// Token: 0x04008432 RID: 33842
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024A4 RID: 9380
			public class BODY
			{
				// Token: 0x0400A18F RID: 41359
				public static LocString CONTAINER1 = "We've successfully compressed and expanded small portions of time under .03 milliseconds. This proves that time is something that can be physically acted upon, suggesting that our vision is attainable.\n\nAn unintentional consequence of both the expansion and contraction of time is the creation of a \"vacuum\" in the space between the affected portion of time and the much more expansive unaffected portions.\n\nSo far, we are seeing that the unaffected time on either side of the manipulated portion will expand or contract to fill the vacuum, although we are unsure how far-reaching this consequence is or what effect it has on the laws of the natural universe. At the end of all compression and expansion experiments, alterations to time are undone and leave no lasting change.";
			}
		}

		// Token: 0x02001D22 RID: 7458
		public class B8_MAGAZINEARTICLE
		{
			// Token: 0x04008433 RID: 33843
			public static LocString TITLE = "Nucleoid Article";

			// Token: 0x04008434 RID: 33844
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024A5 RID: 9381
			public class BODY
			{
				// Token: 0x0400A190 RID: 41360
				public static LocString CONTAINER1 = "<b>Incredible Technology From Independent Lab Harnesses Time into Energy</b>";

				// Token: 0x0400A191 RID: 41361
				public static LocString CONTAINER2 = "Scientists from the recently founded Gravitas Facility have unveiled their first technology prototype, dubbed the \"Temporal Bow\". It is a device which manipulates the 4th dimension to generate infinite, clean and renewable energy.\n\nWhile it may sound like something from science fiction, facility founder Dr. Jacquelyn Stern confirms that it is very much real.\n\n\"It has already been demonstrated that Newton's Second Law of Motion can be violated by negative mass superfluids under the correct lab conditions,\" she says.\n\n\"If the Laws of Motion can be bent and altered, why not the Laws of Thermodynamics? That was the main intent behind this project.\"\n\nThe Temporal Bow works by rapidly vibrating sections of the 4th dimension to send small quantities of mass forward and backward in time, generating massive amounts of energy with virtually no waste.\n\n\"The fantastic thing about using the 4th dimension as fuel,\" says Stern, \"is that it is really, categorically infinite\".\n\nFor those eagerly awaiting the prospect of human time travel, don't get your hopes up just yet. The Facility says that although they have successfully transported matter through time, the technology was expressly developed for the purpose of energy generation and is ill-equipped for human transportation.";
			}
		}

		// Token: 0x02001D23 RID: 7459
		public class MYSTERYAWARD
		{
			// Token: 0x04008435 RID: 33845
			public static LocString TITLE = "Nanotech Article";

			// Token: 0x04008436 RID: 33846
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024A6 RID: 9382
			public class BODY
			{
				// Token: 0x0400A192 RID: 41362
				public static LocString CONTAINER1 = "<b>Mystery Project Wins Nanotech Award</b>";

				// Token: 0x0400A193 RID: 41363
				public static LocString CONTAINER2 = "Last night's Worldwide Nanotech Awards has sparked controversy in the scientific community after it was announced that the top prize had been awarded to a project whose details could not be publicly disclosed.\n\nThe highly classified paper was presented to the jury in a closed session by lead researcher Dr. Liling Pei, recipient of the inaugural Gravitas Accelerator Scholarship at the Elion University of Science and Technology.\n\nHead judge Dr. Elias Balko acknowledges that it was unorthodox, but defends the decision. \"We're scientists - it's our job to push boundaries.\"\n\nPei was awarded the coveted Halas Medal, the top prize for innovation in the field.\n\n\"I wish I could tell you more,\" says Pei. \"I'm SO grateful to the WNA for this great honor, and to Dr. Stern for the funding that made it all possible. This is going to change everything about...well, everything.\"\n\nThis is the second time that Pei has made headlines. Last year, the striking young nanoscientist won the Miss Planetary Belle pageant's talent show with a live demonstration of nanorobots weaving a ballgown out of fibers harvested from common houseplants.\n\nPei joins the team at the Gravitas Facility early next month.";
			}
		}

		// Token: 0x02001D24 RID: 7460
		public class A7_NEUTRONIUM
		{
			// Token: 0x04008437 RID: 33847
			public static LocString TITLE = "Byproduct Notes";

			// Token: 0x04008438 RID: 33848
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024A7 RID: 9383
			public class BODY
			{
				// Token: 0x0400A194 RID: 41364
				public static LocString CONTAINER1 = "[LOG BEGINS]\n\nDirector: I've determined the substance to be metallic in nature. The exact cause of its formation is still unknown, though I believe it to be something of an autoimmune reaction of the natural universe, a quarantining of foreign material to prevent temporal contamination.\n\nDirector: A method has yet to be found that can successfully remove the substance from an affected object, and the larger implication that two molecularly, temporally identical objects cannot coexist at one point in time has dire implications for all time manipulation technology research, not just the Bow.\n\nDirector: For the moment I have dubbed the substance \"Neutronium\", and assigned it a theoretical place on the table of elements. Research continues.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D25 RID: 7461
		public class A9_NEUTRONIUMAPPLICATIONS
		{
			// Token: 0x04008439 RID: 33849
			public static LocString TITLE = "Possible Applications";

			// Token: 0x0400843A RID: 33850
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024A8 RID: 9384
			public class BODY
			{
				// Token: 0x0400A195 RID: 41365
				public static LocString CONTAINER1 = "[LOG BEGINS]\n\nDirector: Temporal energy can be reconfigured to vibrate the matter constituting Neutronium at just the right frequency to break it down and disperse it.\n\nDirector: However, it is difficult to stabilize and maintain this reconfigured energy long enough to effectively remove practical amounts of Neutronium in real-life scenarios.\n\nDirector: I am looking into making this technology more reliable and compact - this data could potentially have uses in the development of some sort of all-purpose disintegration ray.\n\n[END LOG]";
			}
		}

		// Token: 0x02001D26 RID: 7462
		public class PLANETARYECHOES
		{
			// Token: 0x0400843B RID: 33851
			public static LocString TITLE = "Planetary Echoes";

			// Token: 0x0400843C RID: 33852
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024A9 RID: 9385
			public class BODY
			{
				// Token: 0x0400A196 RID: 41366
				public static LocString TITLE1 = "Echo One";

				// Token: 0x0400A197 RID: 41367
				public static LocString TITLE2 = "Echo Two";

				// Token: 0x0400A198 RID: 41368
				public static LocString CONTAINER1 = "Olivia: We've double-checked our observational equipment and the computer's warm-up is almost finished. We have precautionary personnel in place ready to start a shutdown in the event of a failure.\n\nOlivia: It's time.\n\nJackie: Right.\n\nJackie: Spin the machine up slowly so we can monitor for any abnormal power fluctuations. We start on \"3\".\n\nJackie: \"1\"... \"2\"...\n\nJackie: \"3\".\n\n[There's a metallic clunk. The baritone whirr of machinery can be heard.]\n\nJackie: Something's not right.\n\nOlivia: It's the container... the atom is vibrating too fast.\n\n[The whir of the machinery peels up an octave into a mechanical screech.]\n\nOlivia: W-we have to abort!\n\nJackie: No, not yet. Drop power from the coolant system and use it to bolster the container. It'll stabilize.\n\nOlivia: But without coolant--\n\nJackie: It will stabilize!\n\n[There's a sharp crackle of electricity.]\n\nOlivia: Drop 40% power from the coolant systems, reroute everything we have to the atomic container! \n\n[The whirring reaches a crescendo, then calms into a steady hum.]\n\nOlivia: That did it. The container is stabilizing.\n\n[Jackie sighs in relief.]\n\nOlivia: But... Look at these numbers.\n\nJackie: My god. Are these real?\n\nOlivia: Yes, I'm certain of it. Jackie, I think we did it.\n\nOlivia: I think we created an infinite energy source.\n------------------\n";

				// Token: 0x0400A199 RID: 41369
				public static LocString CONTAINER2 = "Olivia: What on earth is this?\n\n[An open palm slams papers down on a desk.]\n\nOlivia: These readings show that hundreds of kilograms of Neutronium are building up in the machine every shift. When were you going to tell me?\n\nJackie: I'm handling it.\n\nOlivia: We don't have the luxury of taking shortcuts. Not when safety is on the line.\n\nJackie: I think I'm capable of overseeing my own safety.\n\nOlivia: I-I'm not just concerned about <i>your</i> safety! We don't understand the longterm implications of what we're developing here... the manipulations we conduct in this facility could have rippling effects throughout the world, maybe even the universe.\n\nJackie: Don't be such a fearmonger. It's not befitting of a scientist. Besides, I'll remind you this research has the potential to stop the fuel wars in their tracks and end the suffering of thousands. Every day we spend on trials here delays that.\n\nOlivia: It's dangerous.\n\nJackie: Your concern is noted.\n------------------\n";
			}
		}

		// Token: 0x02001D27 RID: 7463
		public class SCHOOLNEWSPAPER
		{
			// Token: 0x0400843D RID: 33853
			public static LocString TITLE = "Campus Newspaper Article";

			// Token: 0x0400843E RID: 33854
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024AA RID: 9386
			public class BODY
			{
				// Token: 0x0400A19A RID: 41370
				public static LocString CONTAINER1 = "<b>Party Time for Local Students</b>";

				// Token: 0x0400A19B RID: 41371
				public static LocString CONTAINER2 = "Students at the Elion University of Science and Technology have held an unconventional party this weekend.\n\nWhile their peers may have been out until the wee hours wearing lampshades on their heads and drawing eyebrows on sleeping colleagues, students Jackie Stern and Olivia Broussard spent the weekend in their dorm, refreshments and decorations ready, waiting for the arrival of the guests of honor: themselves.\n\nThe two prospective STEM students, who study theoretical physics with a focus on the workings of space time, conducted the experiment under the assumption that, were their theories about the malleability of space time to ever come to fruition, their future selves could travel back in time to greet them at the party, proving the existence of time travel.\n\nThey weren't inconsiderate of their future selves' busy schedules though; should the guests of honor be unable to attend, they were encouraged to send back a message using the codeword \"Hourglass\" to communicate that, while they certainly wanted to come, they were simply unable.\n\nSadly no one RSVP'd or arrived to the party, but that did not dishearten Olivia or Jackie.\n\nAs Olivia put it, \"It just meant more snacks for us!\"";
			}
		}

		// Token: 0x02001D28 RID: 7464
		public class B6_TIMEMUSINGS
		{
			// Token: 0x0400843F RID: 33855
			public static LocString TITLE = "Director's Notes";

			// Token: 0x04008440 RID: 33856
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024AB RID: 9387
			public class BODY
			{
				// Token: 0x0400A19C RID: 41372
				public static LocString CONTAINER1 = "When we discuss Time as a concrete aspect of the universe, not seconds on a clock or perceptions of the mind, it is important first of all to establish that we are talking about a unique dimension that layers into the three physical dimensions of space: length, width, depth.\n\nWe conceive of Real Time as a straight line, one dimensional, uncurved and stretching forward infinitely. This is referred to as the \"Arrow of Time\".\n\nLogically this Arrow can move only forward and can never be reversed, as such a reversal would break the natural laws of the universe. Effect would precede cause and universal entropy would be undone in a blatant violation of the Second Law.\n\nStill, one can't help but wonder; what if the Arrow's trajectory could be curved? What if it could be redirected, guided, or loosed? What if we could create Time's Bow?";
			}
		}

		// Token: 0x02001D29 RID: 7465
		public class B7_TIMESARROWTHOUGHTS
		{
			// Token: 0x04008441 RID: 33857
			public static LocString TITLE = "Time's Arrow Thoughts";

			// Token: 0x04008442 RID: 33858
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024AC RID: 9388
			public class BODY
			{
				// Token: 0x0400A19D RID: 41373
				public static LocString CONTAINER1 = "I've been unable to shake the notion of the Bow.\n\nThe thought of its mechanics are too intriguing, and I can only dream of the mark such a device would make upon the world -- imagine, a source of inexhaustible energy!\n\nSo many of humanity's problems could be solved with this one invention - domestic energy, environmental pollution, <i>the fuel wars</i>.\n\nI have to pursue this dream, no matter what.";
			}
		}

		// Token: 0x02001D2A RID: 7466
		public class C8_TIMESORDER
		{
			// Token: 0x04008443 RID: 33859
			public static LocString TITLE = "Time's Order";

			// Token: 0x04008444 RID: 33860
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024AD RID: 9389
			public class BODY
			{
				// Token: 0x0400A19E RID: 41374
				public static LocString CONTAINER1 = "We have been successfully using the Temporal Bow now for some time with no obvious consequences. I should be happy that this works so well, but several questions gnaw at my brain late at night.\n\nIf Time's Arrow is moving forward the Laws of Entropy declare that the universe should be moving from a state of order to one of chaos. If the Temporal Bow bends to a previous point in time to a point when things were more orderly, logic would dictate that we are making this point more chaotic by taking things from it. All known laws of the natural universe suggests that this should have affected our Present Day, but all evidence points to that not being true. It appears the theory that we cannot change our past was incorrect!\n\nThis suggests that Time is, in fact, not an arrow but several arrows, each pointing different directions. Fundamentally, this proves the existence of other timelines - different dimensions - some of which we can assume have also built their own Temporal Bow.\n\nThe promise of crossing this final dimensional threshold is too tempting. Imagine what things Gravitas has invented in another dimension!! I must find a way to tear open the fabric of spacetime and tap into the limitless human potential of a thousand alternate timelines.";
			}
		}

		// Token: 0x02001D2B RID: 7467
		public class B5_ANTS
		{
			// Token: 0x04008445 RID: 33861
			public static LocString TITLE = "Ants";

			// Token: 0x04008446 RID: 33862
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x020024AE RID: 9390
			public class BODY
			{
				// Token: 0x0400A19F RID: 41375
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, B556]</smallcaps>\n\n[LOG BEGINS]\n\nTechnician: <i>Atta cephalotes</i>. What sort of experiment are you doing with these?\n\nDirector: No experiment. I just find them interesting. Don't you?\n\nTech: Not really?\n\nDirector: You ought to. Very efficient. They perfected farming millions of years before humans.\n\n(sound of tapping on glass)\n\nDirector: An entire colony led by and in service to its queen. Each organism knows its role.\n\nTech: I have the results from the power tests, director.\n\nDirector: And?\n\nTech: Negative, ma'am.\n\nDirector: I see. You know, another admirable quality of ants occurs to me. They can pull twenty times their own weight.\n\nTech: I'm not sure I follow, ma'am.\n\nDirector: Are you pulling your weight, Doctor?\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D2C RID: 7468
		public class A8_CLEANUPTHEMESS
		{
			// Token: 0x04008447 RID: 33863
			public static LocString TITLE = "Cleaning Up The Mess";

			// Token: 0x04008448 RID: 33864
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x020024AF RID: 9391
			public class BODY
			{
				// Token: 0x0400A1A0 RID: 41376
				public static LocString CONTAINER1 = "I cleaned up a few messes in my time, but ain't nothing like the mess I seen today in that bio lab. Green goop all over the floor, all over the walls. Murky tubes with what look like human shapes floating in them.\n\nThey think old Mr. Gunderson ain't got smarts enough to put two and two together, but I got eyes, don't I?\n\nAin't nobody ever pay attention to the janitor.\n\nBut the janitor pays attention to everybody.\n\n-Mr. Stinky Gunderson";
			}
		}

		// Token: 0x02001D2D RID: 7469
		public class CRITTERDELIVERY
		{
			// Token: 0x04008449 RID: 33865
			public static LocString TITLE = "Critter Delivery";

			// Token: 0x0400844A RID: 33866
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x020024B0 RID: 9392
			public class BODY
			{
				// Token: 0x0400A1A1 RID: 41377
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: B482, B759, C094]</smallcaps>\n\n[LOG BEGINS]\n\nSecurity Guard 1: Hey hey! Welcome back.\n\nSecurity Guard 2: Hand on the scanner, please.\n\nCourier: Sure thing, lemme just...\n\nCourier: Whoops-- thanks, Steve. These little fellas are a two-hander for sure.\n\n(sound of furry noses snuffling on cardboard)\n\nSecurity Guard 2: Follow me, please.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D2E RID: 7470
		public class B2_ELLIESBIRTHDAY
		{
			// Token: 0x0400844B RID: 33867
			public static LocString TITLE = "Office Cake";

			// Token: 0x0400844C RID: 33868
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x020024B1 RID: 9393
			public class BODY
			{
				// Token: 0x0400A1A2 RID: 41378
				public static LocString CONTAINER1 = "Joshua: Hey Mr. Kraus, I'm passing around the collection pan. Wanna pitch in a couple bucks to get a cake for Ellie?\n\nOtto: Uh... I think I'll pass.\n\nJoshua: C'mon Otto, it's her birthday.\n\nOtto: Alright, fine. But this is all I have on me.\n\nOtto: I don't get why you hang out with her. Isn't she kind of... you know, mean?\n\nJoshua: Even the meanest people have a little niceness in them somewhere.\n\nOtto: Huh. Good luck finding it.\n\nJoshua: Thanks for the cake money, Otto.\n------------------\n";

				// Token: 0x0400A1A3 RID: 41379
				public static LocString CONTAINER2 = "Ellie: Nice cake. I bet it wasn't easy to like, strong-arm everyone into buying it.\n\nJoshua: You know, if you were a little nicer to people they might want to spend more time with you.\n\nEllie: Pfft, please. Friends are about <i>quality</i>, not quantity, Josh.\n\nJoshua: Wow! Was that a roundabout compliment I just heard?\n\nEllie: What? Gross, ew. Stop that.\n\nJoshua: Oh, don't worry, I won't tell anyone. I'm not much of a gossip.";
			}
		}

		// Token: 0x02001D2F RID: 7471
		public class A7_EMPLOYEEPROCESSING
		{
			// Token: 0x0400844D RID: 33869
			public static LocString TITLE = "Employee Processing";

			// Token: 0x0400844E RID: 33870
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x020024B2 RID: 9394
			public class BODY
			{
				// Token: 0x0400A1A4 RID: 41380
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, A435, B111]</smallcaps>\n\n[LOG BEGINS]\n\nTechnician: Thank you for the fingerprints, doctor. We just need a quick voice sample, then you can be on your way.\n\nDr. Broussard: Wow Jackie, your new security's no joke.\n\nDirector: Please address me as \"Director\" while on Facility grounds.\n\nDr. Broussard: ...R-right.\n\n(clicking)\n\nTechnician: This should only take a moment. Speak clearly and the system will derive a vocal signature for you.\n\nTechnician: When you're ready.\n\n(throat clearing)\n\nDr. Broussard: Security code B111, Dr. Olivia Broussard. Gravitas Facility Bioengineering Department.\n\n(pause)\n\nTechnician: Great.\n\nDr. Broussard: What was that light just now?\n\nDirector: A basic security scan. No need for concern.\n\n(machine printing)\n\nTechnician: Here's your ID. You should have access to all doors in the facility now, Dr. Broussard.\n\nDr. Broussard: Thank you.\n\nDirector: Come along, Doctor.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D30 RID: 7472
		public class C01_EVIL
		{
			// Token: 0x0400844F RID: 33871
			public static LocString TITLE = "Evil";

			// Token: 0x04008450 RID: 33872
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x020024B3 RID: 9395
			public class BODY
			{
				// Token: 0x0400A1A5 RID: 41381
				public static LocString CONTAINER1 = "Clearly Nikola is evil. He has some kind of scheme going on that he's keeping secret from the rest of Gravitas and I haven't been able to crack what that is because it's offline and he's always at his computer. Whenever I ask him what he's up to he says I wouldn't understand. Pfft! We both went through the same particle physics classes, buddy. Just because you mash a keyboard and I adjust knobs does not mean I don't know what the Time Containment Field does.\n\nAnd then today I dropped a wrench and Nikola nearly jumped out of his skin! He spun around and screamed at me never to do that again. And then when I said, \"Geez, it's not the end of the world,\" he was like, \"Yeah, it's not like the world will blow up if I get this wrong\" really sarcastic-like.\n\nWhich technically is true. If the Time Containment Field were to break down, the Temporal Bow could theoretically blow up the world. But that's why there are safety systems in place. And safety systems on safety systems. And then safety systems on top of that. But then again he built all the safety systems, so if he wanted to...\n------------------\n";

				// Token: 0x0400A1A6 RID: 41382
				public static LocString CONTAINER2 = "I decided to get into work early today but when I got in Nikola was already there and it looked like he hadn't been home all weekend. He was pacing back and forth in the lab, monologuing but not like an evil villain. Like someone who hadn't slept in a week.\n\n\"Ruby,\" he said. \"You have to promise me that if anything goes wrong you'll turn on this machine. They're pushing it too far. The printing pods are pushing the...It's too much - TOO MUCH! Something's going to blow. I tried... I'm trying to save it. Not the Earth. There's no hope for the Earth, it's all going to...\" then he made this exploding sound. \"But the Universe. Time itself. It could all go, don't you see? This machine can contain it. Put a Temporal Containment Field around the Earth so time itself doesn't break down and...and...\"\n\nThen all of a sudden these security guys came in. New guys. People I haven't seen before. And they just took him away. Then they took me to a room and asked me all kinds of questions and I answered them, I guess. I don't remember much because the whole time I was thinking - What if I was wrong? What if he's not evil, but Gravitas is?\n\nWhat if I was wrong and what if he's right?\n------------------\n";

				// Token: 0x0400A1A7 RID: 41383
				public static LocString CONTAINER3 = "No seriously - what if he's right?\n------------------\n";
			}
		}

		// Token: 0x02001D31 RID: 7473
		public class B7_INSPACE
		{
			// Token: 0x04008451 RID: 33873
			public static LocString TITLE = "In Space";

			// Token: 0x04008452 RID: 33874
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x020024B4 RID: 9396
			public class BODY
			{
				// Token: 0x0400A1A8 RID: 41384
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: B835, B997]</smallcaps>\n\n[LOG BEGINS]\n\nDr.Ansari: Shhhh...\n\nDr. Bubare: What? What are we doing here?\n\nDr. Ansari: I'll show you, just keep your voice down.\n\nDr. Bubare: Are we even allowed to be here?\n\nDr. Ansari: No. Trust me it'll all be worth it once I can find it.\n\nDr. Bubare: Find what?\n\nDr. Ansari: That!\n\nDr. Bubare: ...Video feed from a rat cage? What's so great about -- Wait. Are they--?\n\nDr. Ansari: Floating!\n\nDr. Bubare: You mean they're in--?\n\nDr. Ansari: Space!\n\nDr. Bubare: Our thermal rats are in space?!?!\n\nDr. Ansari: Yep! There's Applecart and Cherrypie and little Bananabread. Look at them, they're so happy. We made ratstronauts!!\n\nDr. Bubare: HAPPY rat-stronauts.\n\nDr. Ansari: WE MADE HAPPY RATSTRONAUTS!!\n\nDr. Bubare: Shhhhhh...Someone's coming.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D32 RID: 7474
		public class B3_MOVEDRABBITS
		{
			// Token: 0x04008453 RID: 33875
			public static LocString TITLE = "Moved Rabbits";

			// Token: 0x04008454 RID: 33876
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x020024B5 RID: 9397
			public class BODY
			{
				// Token: 0x0400A1A9 RID: 41385
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, B111]</smallcaps>\n\n[LOG BEGINS]\n\nBroussard: Director, do you know where my rabbits have been moved to? I asked around the bioengineering division but I was referred back to you.\n\nDirector: Hm? Oh, yes, they've been removed.\n\nBroussard: \"Removed\"?\n\nDirector: Discarded. I'm sorry, did you still need them? The reports showed your experiments with them were completed.\n\nBroussard: No, I-I... I'd collected all the data I needed, I just --\n\nDirector: -- Doctor. You weren't making pets out of test subjects, were you?\n\nBroussard: Don't be ridiculous, I --\n\nDirector: -- Good.They were horrible to look at anyway. All those red eyes looking at me.\n\nBroussard: In the future, please do not mess with my things. It... disturbs me.\n\nDirector: I will notify you beforehand next time, Doctor.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D33 RID: 7475
		public class B3_MOVEDRACCOONS
		{
			// Token: 0x04008455 RID: 33877
			public static LocString TITLE = "Moved Raccoons";

			// Token: 0x04008456 RID: 33878
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x020024B6 RID: 9398
			public class BODY
			{
				// Token: 0x0400A1AA RID: 41386
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, B111]</smallcaps>\n\n[LOG BEGINS]\n\nBroussard: Director, do you know where my raccoons have been moved to? I asked around the bioengineering division but I was referred back to you.\n\nDirector: Hm? Oh, yes, they've been removed.\n\nBroussard: \"Removed\"?\n\nDirector: Discarded. I'm sorry, did you still need them? The reports showed your experiments with them were completed.\n\nBroussard: No, I-I... I'd collected all the data I needed, I just --\n\nDirector: -- Doctor. You weren't making pets out of test subjects, were you?\n\nBroussard: Don't be ridiculous, I --\n\nDirector: -- Good.They were horrible to look at anyway. All that mangy fur.\n\nBroussard: In the future, please do not mess with my things. It... disturbs me.\n\nDirector: I will notify you beforehand next time, Doctor.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D34 RID: 7476
		public class B3_MOVEDRATS
		{
			// Token: 0x04008457 RID: 33879
			public static LocString TITLE = "Moved Rats";

			// Token: 0x04008458 RID: 33880
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x020024B7 RID: 9399
			public class BODY
			{
				// Token: 0x0400A1AB RID: 41387
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, B111]</smallcaps>\n\n[LOG BEGINS]\n\nBroussard: Director, do you know where my rats have been moved to? I asked around the bioengineering division but I was referred back to you.\n\nDirector: Hm? Oh, yes, they've been removed.\n\nBroussard: \"Removed\"?\n\nDirector: Discarded. I'm sorry, did you still need them? The reports showed your experiments with them were completed.\n\nBroussard: No, I-I... I'd collected all the data I needed, I just --\n\nDirector: -- Doctor. You weren't making pets out of test subjects, were you?\n\nBroussard: Don't be ridiculous, I --\n\nDirector: -- Good.They were horrible to look at anyway. All those bumps.\n\nBroussard: In the future, please do not mess with my things. It... disturbs me.\n\nDirector: I will notify you beforehand next time, Doctor.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D35 RID: 7477
		public class A1_A046
		{
			// Token: 0x04008459 RID: 33881
			public static LocString TITLE = "Personal Journal: A046";

			// Token: 0x0400845A RID: 33882
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x020024B8 RID: 9400
			public class BODY
			{
				// Token: 0x0400A1AC RID: 41388
				public static LocString CONTAINER1 = "Gravitas has been growing pretty rapidly since our first product hit the market. I just got a look at some of the new hires - they're practically babies! Not quite what I was expecting, but then I've never had an opportunity to mentor someone before. Could be fun!\n------------------\n";

				// Token: 0x0400A1AD RID: 41389
				public static LocString CONTAINER2 = "Well, mentorship hasn't gone quite how I'd expected. Turns out the young hires don't need me to show them the ropes. Actually, since the facility's gotten rid of our swipe cards one of the nice young men had to show me how to operate the doors after I got stuck outside my own lab. Don't I feel silly.\n------------------\n";

				// Token: 0x0400A1AE RID: 41390
				public static LocString CONTAINER3 = "Well, if that isn't just gravy, hm? One of the new hires will be acting as the team lead on my next project.\n\nWhen I first started it wasn't that uncommon to sample a whole rack of test tubes by hand. Now a machine can do hundreds of them in seconds. Who knows what this job will look like in another ten or twenty years. Will I still even be in it?\n------------------\n";

				// Token: 0x0400A1AF RID: 41391
				public static LocString CONTAINER4 = "That nice young man who helped me with the door the other day, Mr. Kraus, has been an absolute angel. He's been kind enough to help me with this horrible e-mail system and even showed me how to digitize my research notes. I'm learning a lot. Turns out I wasn't the mentor, I'm the mentee! If that isn't a chuckle. At any rate, I feel like I have a better handle on things around here due to Mr. Kraus' help. Turns out you're never too old to learn something new!\n------------------\n";
			}
		}

		// Token: 0x02001D36 RID: 7478
		public class A1A_B111
		{
			// Token: 0x0400845B RID: 33883
			public static LocString TITLE = "Personal Journal: B111";

			// Token: 0x0400845C RID: 33884
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024B9 RID: 9401
			public class BODY
			{
				// Token: 0x0400A1B0 RID: 41392
				public static LocString CONTAINER1 = "I sent Dr. Holland home today after I found him wandering the lab mumbling to himself. He looked like he hadn't slept in days!\n\nI worry that everyone here is so afraid of disappointing ‘The Director' that they are pushing themselves to the breaking point. Next chance I get, I'm going to bring this up with Jackie.\n------------------\n";

				// Token: 0x0400A1B1 RID: 41393
				public static LocString CONTAINER2 = "Well, that didn't work.\n\nBringing up the need for some office bonding activities with the Director only met with her usual stubborn insistence that we \"don't have time for any fun\".\n\nThis is ridiculous. Tomorrow I'm going to organize something fun for everyone and Jackie will just have to deal with it. She just needs to see the long term benefits of short term stress relief to fully understand the importance of this.\n------------------\n";

				// Token: 0x0400A1B2 RID: 41394
				public static LocString CONTAINER3 = "I can't believe this! I organized a potluck lunch thinking it would be a nice break but Jackie discovered us as we were setting up and insisted that no one had time for \"fooling around\". Of course, everyone was too afraid to defy 'The Director' and went right back to work.\n\nAll the food was just thrown out. Someone had even brought homemade perogies! Seeing the break room garbage full of potato salad and chicken wings made me even more depressed than before. Those perogies looked so good.\n------------------\n";

				// Token: 0x0400A1B3 RID: 41395
				public static LocString CONTAINER4 = "I keep finding senseless mistakes from stressed-out lab workers. It's getting dangerous. I'm worried this colony we're building will be plagued with these kinds of problems if we don't prioritize mental health as much as physical health. What's the use of making all these plans for the future if we can't build a better world?\n\nMaybe there's some way I can sneak some prerequisite downtime activities into the Printing Pod without Jackie knowing.\n------------------\n";
			}
		}

		// Token: 0x02001D37 RID: 7479
		public class A2_B327
		{
			// Token: 0x0400845D RID: 33885
			public static LocString TITLE = "Personal Journal: B327";

			// Token: 0x0400845E RID: 33886
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x020024BA RID: 9402
			public class BODY
			{
				// Token: 0x0400A1B4 RID: 41396
				public static LocString CONTAINER1 = "I'm starting my new job at Gravitas today. I'm... well, I'm nervous.\n\nIt turns out they hired a bunch of new people - I guess they're expanding - and most of them are about my age, but I'm the only one that hasn't done my doctorate. They all call me \"Mister\" Kraus and it's the <i>worst</i>.\n\nI have no idea where I'll find the time to do my PhD while working a full time job.\n------------------\n";

				// Token: 0x0400A1B5 RID: 41397
				public static LocString CONTAINER2 = "<i>I screwed up so much today.</i>\n\nAt one point I spaced on the formula for calculating the volume of a cone. They must have thought I was completely useless.\n\nThe only time I knew what I was doing was when I helped an older coworker figure out her dumb old email.\n\nPeople say education isn't so important as long as you've got the skills, but there's things my colleagues know that I just <i>don't</i>. They're not mean about it or anything, it's just so frustrating. I feel dumb when I talk to them!\n\nI bet they're gonna realize soon that I don't belong here, and then I'll be fired for sure. Man... I'm still paying off my student loans (WITH international fees), I <i>can't</i> lose this income.\n------------------\n";

				// Token: 0x0400A1B6 RID: 41398
				public static LocString CONTAINER3 = "Dr. Sklodowska's been really nice and welcoming since I started working here. Sometimes she comes and sits with me in the cafeteria. The food she brings from home smells like old feet but she chats with me about what new research papers we're each reading and it's very kind.\n\nShe tells me the fact I got hired without a doctorate means I must be very smart, and management must see something in me.\n\nI'm not sure I believe her but it's nice to hear something that counters little voice in my head anyway.\n------------------\n";

				// Token: 0x0400A1B7 RID: 41399
				public static LocString CONTAINER4 = "It's been about a week and a half and I think I'm finally starting to settle in. I'm feeling a lot better about my position - some of the senior scientists have even started using my ideas in the lab.\n\nDr. Sklodowska might have been right, my anxiety was just growing pains. This is my first real job and I guess afraid to let myself believe I could really, actually do it, just in case it went wrong.\n\nI think I want to buy Dr. Sklowdoska a digital reader for her books and papers as a thank-you one day, if I ever pay off my student loans.\n\nONCE I pay off my student loans.\n------------------\n";
			}
		}

		// Token: 0x02001D38 RID: 7480
		public class A3_B556
		{
			// Token: 0x0400845F RID: 33887
			public static LocString TITLE = "Personal Journal: B556";

			// Token: 0x04008460 RID: 33888
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x020024BB RID: 9403
			public class BODY
			{
				// Token: 0x0400A1B8 RID: 41400
				public static LocString CONTAINER1 = "I've been so tired lately. I've probably spent the last 3 nights sleeping at my desk, and I've used the lab's safety shower to bathe twice already this month.\n\nWe're technically on schedule, but for some reason Director Stern has been breathing down my neck to get these new products ready for market.\n\nNormally I'd be mad about the added pressure on my work, but something in the Director's voice tells me that time is of the essence.\n------------------\n";

				// Token: 0x0400A1B9 RID: 41401
				public static LocString CONTAINER2 = "I keep finding myself staring at my computer screen, totally unable to remember what it was I was doing.\n\nI try to force myself to type up some notes or analyze my data but it's like my brain is paralyzed, I can't get anything done.\n\nI'll have to stay late to make up for all this time I've wasted staying late.\n------------------\n";

				// Token: 0x0400A1BA RID: 41402
				public static LocString CONTAINER3 = "Dr. Broussard told me I looked half dead and sent me home today. I don't think she even has the authority to do that, but I did as I was told. She wasn't messing around if you know what I mean.\n\nI can probably get a head start on my paper from home today, anyway.\n\nI think I have an idea for a circuit configuration that will improve the battery life of all our technologies by a whole 2.3%.\n------------------\n";

				// Token: 0x0400A1BB RID: 41403
				public static LocString CONTAINER4 = "I got home yesterday fully intending to work on my paper after Broussard sent me home, but the second I walked in the door I hit the pillow and didn't get back up. I slept for <i>12 straight hours</i>.\n\nI had no idea I needed that. When I got into the lab this morning I looked over my work from the past few weeks, and realized it's completely useless.\n\nIt'll take me hours to correct all the mistakes I made these past few months. Is this what I was killing myself for? I'm such a rube, I owe Broussard a huge thanks.\n\nI'll start keeping more regular hours from now on... Also, I was considering maybe getting a dog.";
			}
		}

		// Token: 0x02001D39 RID: 7481
		public class A4_B835
		{
			// Token: 0x04008461 RID: 33889
			public static LocString TITLE = "Personal Journal: B835";

			// Token: 0x04008462 RID: 33890
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x020024BC RID: 9404
			public class BODY
			{
				// Token: 0x0400A1BC RID: 41404
				public static LocString CONTAINER1 = "I started work at a new company called the \"Gravitas Facility\" today! I was nervous I wouldn't get the job at first because I was fresh out of school, and I was so so so pushy in the interview, but the Director apparently liked my thesis on the physiological thermal regulation of Arctic lizards. I'll be working with some brilliant geneticists, bioengineering organisms for space travel in harsh environments! It's like a dream come true. I get to work on exciting new research in a place where no one knows me!\n------------------\n";

				// Token: 0x0400A1BD RID: 41405
				public static LocString CONTAINER2 = "No no no no no! It can't be! BANHI ANSARI is here, working on space shuttle thrusters in the robotics lab! As soon as she saw me she called me \"Bubbles\" and told everyone about the time I accidentally inhaled a bunch of fungal spores during lab, blew a big snot bubble out my nose and then sneezed all over Professor Avery! Everyone's calling me \"Bubbles\" instead of \"Doctor\" at work now. Some of them don't even know it's a nickname, but I don't want to correct them and seem rude or anything. Ugh, I can't believe that story followed me here! BANHI RUINS EVERYTHING!\n------------------\n";

				// Token: 0x0400A1BE RID: 41406
				public static LocString CONTAINER3 = "I've spent the last few days buried in my work, and I'm actually feeling a lot better. We finally perfected a gene manipulation that controls heat sensitivity in rats. Our test subjects barely even shiver in subzero temperatures now. We'll probably do a testrun tomorrow with Robotics to see how the rats fare in the prototype shuttles we're developing.\n------------------\n";

				// Token: 0x0400A1BF RID: 41407
				public static LocString CONTAINER4 = "HAHAHAHAHA! Bioengineering and Robotics did the test run today and Banhi was securing the live cargo pods when one of the rats squeaked at her. She was so scared, she fell on her butt and TOOTED in front of EVERYONE! They're all calling her \"Pipsqueak\". \"Bubbles\" doesn't seem quite so bad now. Pipsqueak's been a really good sport about it though, she even laughed it off at the time. I think we might actually be friends now? It's weird.\n------------------\n";

				// Token: 0x0400A1C0 RID: 41408
				public static LocString CONTAINER5 = "I lied. Me and Banhi aren't friends - we're BEST FRIENDS. She even showed me how she does her hair. We're gonna book the wind tunnel after work and run experiments together on thermo-rat rockets! Haha!\n------------------\n";
			}
		}

		// Token: 0x02001D3A RID: 7482
		public class A9_PIPEDREAM
		{
			// Token: 0x04008463 RID: 33891
			public static LocString TITLE = "Pipe Dream";

			// Token: 0x04008464 RID: 33892
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ZERO";

			// Token: 0x020024BD RID: 9405
			public class BODY
			{
				// Token: 0x0400A1C1 RID: 41409
				public static LocString CONTAINER1 = "[LOG BEGINS]\n\nThe Director has suggested implanting artificial memories during print, but despite the great strides made in our research under her direction, such a thing can barely be considered more than a pipe dream.\n\nFor the moment we remain focused on eliminating the remaining glitches in the system, as well as developing effective education and training routines for printed subjects.\n\nSuggest: Omega-3 supplements and mentally stimulating enclosure apparatuses to accompany tutelage.\n\nDr. Broussard signing off.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D3B RID: 7483
		public class B4_REVISITEDNUMBERS
		{
			// Token: 0x04008465 RID: 33893
			public static LocString TITLE = "Revisited Numbers";

			// Token: 0x04008466 RID: 33894
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024BE RID: 9406
			public class BODY
			{
				// Token: 0x0400A1C2 RID: 41410
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, A435]</smallcaps>\n\n[LOG BEGINS]\n\nDirector: Unacceptable.\n\nJones: I'm just telling you the numbers, Director, I'm not responsible for them.\n\nDirector: In your earlier e-mail you claimed the issue would be solved by the Pod.\n\nJones: Yeah, the weight issue. And it was solved. The problem now is the insane amount of power that big thing eats every time it prints a colonist.\n\nDirector: So how do you suppose we meet these target numbers? Fossil fuels are exhausted, nuclear is outlawed, solar is next to impossible with this smog.\n\nJones: I dunno. That's why you've got researchers, I just crunch numbers. Although you should avoid fossil fuels and nuclear energy anyway. If you have to load the rocket up with a couple tons of fuel then we're back to square one on the weight problem. It's gotta be something clever.\n\nDirector: Thank you, Dr. Jones. You may go.\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400A1C3 RID: 41411
				public static LocString CONTAINER2 = "<smallcaps>[Voice Recognition Initialized]\n[Subjects Identified: A001, B111]</smallcaps>\n\n[LOG BEGINS]\n\nJackie: Dr. Jones projects that traditional fuel will be insufficient for the Pod to make the flight.\n\nOlivia: Then we need to change its specs. Use lighter materials, cut weight wherever possible, do widespread optimizations across the whole project.\n\nJackie: We have another option.\n\nOlivia: No. Absolutely not. You needed me and I-I came back, but if you plan to revive our research--\n\nJackie: The world's doomed regardless, Olivia. We need to use any advantage we've got... And just think about it! If we built [REDACTED] technology into the Pod it wouldn't just fix the flight problem, we'd know for a fact it would run uninterrupted for thousands of years, maybe more.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D3C RID: 7484
		public class A5_SHRIMP
		{
			// Token: 0x04008467 RID: 33895
			public static LocString TITLE = "Shrimp";

			// Token: 0x04008468 RID: 33896
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ZERO";

			// Token: 0x020024BF RID: 9407
			public class BODY
			{
				// Token: 0x0400A1C4 RID: 41412
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111]</smallcaps>\n\n[LOG BEGINS]\n\n\"A-and how are you clever little guys today?\n\n(trilling)\n\nLook! I brought some pink shrimp for you to eat. Your favorite! Are you hungry?\n\n(excited trilling)\n\nOh, one moment, my keen eager pals. I left the recorder on --\n\n(rustling)\"\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D3D RID: 7485
		public class A5_STRAWBERRIES
		{
			// Token: 0x04008469 RID: 33897
			public static LocString TITLE = "Strawberries";

			// Token: 0x0400846A RID: 33898
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ZERO";

			// Token: 0x020024C0 RID: 9408
			public class BODY
			{
				// Token: 0x0400A1C5 RID: 41413
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111]</smallcaps>\n\n[LOG BEGINS]\n\n\"A-and how are you bouncy little critters today?\n\n(chattering)\n\nLook! I brought strawberries. Your favorite! Are you hungry?\n\n(excited chattering)\n\nOh, one moment, my precious, little pals. I left the recorder on --\n\n(rustling)\"\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D3E RID: 7486
		public class A5_SUNFLOWERSEEDS
		{
			// Token: 0x0400846B RID: 33899
			public static LocString TITLE = "Sunflower Seeds";

			// Token: 0x0400846C RID: 33900
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ZERO";

			// Token: 0x020024C1 RID: 9409
			public class BODY
			{
				// Token: 0x0400A1C6 RID: 41414
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111]</smallcaps>\n\n[LOG BEGINS]\n\n\"A-and how are you furry little fellows today?\n\n(squeaking)\n\nLook! I brought sunflower seeds. Your favorite! Are you hungry?\n\n(excited squeaking)\n\nOh, one moment, my dear, little friends. I left the recorder on --\n\n(rustling)\"\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D3F RID: 7487
		public class SO_LAUNCH_TRAILER
		{
			// Token: 0x0400846D RID: 33901
			public static LocString TITLE = "Spaced Out Trailer";

			// Token: 0x0400846E RID: 33902
			public static LocString SUBTITLE = "";

			// Token: 0x020024C2 RID: 9410
			public class BODY
			{
				// Token: 0x0400A1C7 RID: 41415
				public static LocString CONTAINER1 = "Spaced Out Trailer";
			}
		}

		// Token: 0x02001D40 RID: 7488
		public class LOCKS
		{
			// Token: 0x0400846F RID: 33903
			public static LocString NEURALVACILLATOR = "Neural Vacillator";
		}

		// Token: 0x02001D41 RID: 7489
		public class MYLOG
		{
			// Token: 0x04008470 RID: 33904
			public static LocString TITLE = "My Log";

			// Token: 0x04008471 RID: 33905
			public static LocString SUBTITLE = "Boot Message";

			// Token: 0x04008472 RID: 33906
			public static LocString DIVIDER = "";

			// Token: 0x020024C3 RID: 9411
			public class BODY
			{
				// Token: 0x02002FCB RID: 12235
				public class DUPLICANTDEATH
				{
					// Token: 0x0400C287 RID: 49799
					public static LocString TITLE = "Death In The Colony";

					// Token: 0x0400C288 RID: 49800
					public static LocString BODY = "I lost my first Duplicant today. Duplicants form strong bonds with each other, and I expect I'll see a drop in morale over the next few cycles as they take time to grieve their loss.\n\nI find myself grieving too, in my way. I was tasked to protect these Duplicants, and I failed. All I can do now is move forward and resolve to better protect those remaining in my colony from here on out.\n\nRest in peace, dear little friend.\n\n";
				}

				// Token: 0x02002FCC RID: 12236
				public class PRINTINGPOD
				{
					// Token: 0x0400C289 RID: 49801
					public static LocString TITLE = "The Printing Pod";

					// Token: 0x0400C28A RID: 49802
					public static LocString BODY = "This is the conduit through which I interact with the world. Looking at it fills me with a sense of nostalgia and comfort, though it's tinged with a slight restlessness.\n\nAs the place of their origin, I notice the Duplicants regard my Pod with a certain reverence, much like the reverence a child might have for a parent. I'm happy to fill this role for them, should they desire.\n\n";
				}

				// Token: 0x02002FCD RID: 12237
				public class ONEDUPELEFT
				{
					// Token: 0x0400C28B RID: 49803
					public static LocString TITLE = "Only One Remains";

					// Token: 0x0400C28C RID: 49804
					public static LocString BODY = "My colony is in a dire state. All but one of my Duplicants have perished, leaving a single worker to perform all the tasks that maintain the colony.\n\nGiven enough time I could print more Duplicants to replenish the population, but... should this Duplicant die before then, protocol will force me to enter a deep sleep in hopes that the terrain will become more habitable once I reawaken.\n\nI would prefer to avoid this.\n\n";
				}

				// Token: 0x02002FCE RID: 12238
				public class FULLDUPECOLONY
				{
					// Token: 0x0400C28D RID: 49805
					public static LocString TITLE = "Out Of Blueprints";

					// Token: 0x0400C28E RID: 49806
					public static LocString BODY = "I've officially run out of unique blueprints from which to print new Duplicants.\n\nIf I desire to grow the colony further, I'll have no choice but to print doubles of existing individuals. Hopefully it won't throw anyone into an existential crisis to live side by side with their double.\n\nPerhaps I could give the new clones nicknames to reduce the confusion.\n\n";
				}

				// Token: 0x02002FCF RID: 12239
				public class RECBUILDINGS
				{
					// Token: 0x0400C28F RID: 49807
					public static LocString TITLE = "Recreation";

					// Token: 0x0400C290 RID: 49808
					public static LocString BODY = "My Duplicants continue to grow and learn so much and I can't help but take pride in their accomplishments. But as their skills increase, they require more stimulus to keep their morale high. All work and no play is making an unhappy colony. \n\nI will have to provide more elaborate recreational activities for my Duplicants to amuse themselves if I want my colony to grow. Recreation time makes for a happy Duplicant, and a happy Duplicant is a productive Duplicant.\n\n";
				}

				// Token: 0x02002FD0 RID: 12240
				public class STRANGERELICS
				{
					// Token: 0x0400C291 RID: 49809
					public static LocString TITLE = "Strange Relics";

					// Token: 0x0400C292 RID: 49810
					public static LocString BODY = "My Duplicant discovered an intact computer during their latest scouting mission. This should not be possible.\n\nThe target location was not meant to possess any intelligent life besides our own, and what's more, the equipment we discovered appears to originate from the Gravitas Facility.\n\nThis discovery has raised many questions, though it's also provided a small clue; the machine discovered was embedded inside the rock of this planet, just like how I found my Pod.\n\n";
				}

				// Token: 0x02002FD1 RID: 12241
				public class NEARINGMAGMA
				{
					// Token: 0x0400C293 RID: 49811
					public static LocString TITLE = "Extreme Heat Danger";

					// Token: 0x0400C294 RID: 49812
					public static LocString BODY = "The readings I'm collecting from my Duplicant's sensory systems tell me that the further down they dig, the closer they come to an extreme and potentially dangerous heat source.\n\nI believe they are approaching a molten core, which could mean magma and lethal temperatures. I should equip them accordingly.\n\n";
				}

				// Token: 0x02002FD2 RID: 12242
				public class NEURALVACILLATOR
				{
					// Token: 0x0400C295 RID: 49813
					public static LocString TITLE = "VA[?]...C";

					// Token: 0x0400C296 RID: 49814
					public static LocString BODY = "<smallcaps>>>SEARCH DATABASE [\"vacillator\"]\n>...error...\n>...repairing corrupt data...\n>...data repaired...\n>.........................\n>>returning results\n>.........................</smallcaps>\n<b>I remember...</b>\n<smallcaps>>.........................\n>.........................</smallcaps>\n<b>machines.</b>\n\n";
				}

				// Token: 0x02002FD3 RID: 12243
				public class LOG1
				{
					// Token: 0x0400C297 RID: 49815
					public static LocString TITLE = "Cycle 1";

					// Token: 0x0400C298 RID: 49816
					public static LocString BODY = "We have no life support in place yet, but we've found ourselves in a small breathable air pocket. As far as I can tell, we aren't in any immediate danger.\n\nBetween the available air and our meager food stores, I'd estimate we have about 3 days to set up food and oxygen production before my Duplicants' lives are at risk.\n\n";
				}

				// Token: 0x02002FD4 RID: 12244
				public class LOG2
				{
					// Token: 0x0400C299 RID: 49817
					public static LocString TITLE = "Cycle 3";

					// Token: 0x0400C29A RID: 49818
					public static LocString BODY = "I've almost synthesized enough Ooze to print a new Duplicant; once the Ooze is ready, all I'll have left to do is choose a blueprint.\n\nIt'd be helpful to have an extra set of hands around the colony, but having another Duplicant also means another mouth to feed.\n\nOf course, I could always print supplies to help my existing Duplicants instead. I'm sure they would appreciate it.\n\n";
				}

				// Token: 0x02002FD5 RID: 12245
				public class TELEPORT
				{
					// Token: 0x0400C29B RID: 49819
					public static LocString TITLE = "Duplicant Teleportation";

					// Token: 0x0400C29C RID: 49820
					public static LocString BODY = "My Duplicants have discovered a strange new device that appears to be a remnant of a previous Gravitas facility. Upon activating the device my Duplicant was scanned by some unknown, highly technological device and I subsequently detected a massive information transfer!\n\nRemarkably my Duplicant has now reappeared in a remote location on a completely different world! I now have access to another abandoned Gravitas facility on a neighboring asteroid! Further analysis will be required to understand this matter but in the meantime, I will have to be vigilant in keeping track of both of my colonies.";
				}

				// Token: 0x02002FD6 RID: 12246
				public class OUTSIDESTARTINGBIOME
				{
					// Token: 0x0400C29D RID: 49821
					public static LocString TITLE = "Geographical Survey";

					// Token: 0x0400C29E RID: 49822
					public static LocString BODY = "As the Duplicants scout further out I've begun to piece together a better view of our surroundings.\n\nThanks to their efforts, I've determined that this planet has enough resources to settle a longterm colony.\n\nBut... something is off. I've also detected deposits of Abyssalite and Neutronium in this planet's composition, manmade elements that shouldn't occur in nature.\n\nIs this really the target location?\n\n";
				}

				// Token: 0x02002FD7 RID: 12247
				public class OUTSIDESTARTINGDLC1
				{
					// Token: 0x0400C29F RID: 49823
					public static LocString TITLE = "Regional Analysis";

					// Token: 0x0400C2A0 RID: 49824
					public static LocString BODY = "As my Duplicants have ventured further into their surroundings I've been able to determine a more detailed picture of our surroundings.\n\nUnfortunately, I've concluded that this planetoid does not have enough resources to settle a longterm colony.\n\nI can only hope that we will somehow be able to reach another asteroid before our resources run out.\n\n";
				}

				// Token: 0x02002FD8 RID: 12248
				public class LOG3
				{
					// Token: 0x0400C2A1 RID: 49825
					public static LocString TITLE = "Cycle 15";

					// Token: 0x0400C2A2 RID: 49826
					public static LocString BODY = "As far as I can tell, we are hundreds of miles beneath the surface of the planet. Digging our way out will take some time.\n\nMy Duplicants will survive, but they were not meant for sustained underground living. Under what possible circumstances could my Pod have ended up here?\n\n";
				}

				// Token: 0x02002FD9 RID: 12249
				public class LOG3DLC1
				{
					// Token: 0x0400C2A3 RID: 49827
					public static LocString TITLE = "Cycle 10";

					// Token: 0x0400C2A4 RID: 49828
					public static LocString BODY = "As my Duplicants venture out into the neighboring worlds, there is an ever increasing chance that they will encounter hostile environments unsafe for unprotected individuals. A prudent course of action would be to start research and training for equipment that could protect my Duplicants when they encounter such adverse environments.\n\nThese first few cycles have been occupied with building the basics for my colony, but now it is time I start planning for the future. We cannot merely live day-to-day without purpose. If we are to survive for any significant time, we must strive for a purpose.\n\n";
				}

				// Token: 0x02002FDA RID: 12250
				public class SURFACEBREACH
				{
					// Token: 0x0400C2A5 RID: 49829
					public static LocString TITLE = "Surface Breach";

					// Token: 0x0400C2A6 RID: 49830
					public static LocString BODY = "My Duplicants have done the impossible and excavated their way to the surface, though they've gathered some disturbing new data for me in the process.\n\nAs I had begun to suspect, we are not on the target location but on an asteroid with a highly unusual diversity of elements and resources.\n\nFurther, my Duplicants have spotted a damaged planet on the horizon, visible to the naked eye, that bears a striking resemblance to my historical data on the planet of our origin.\n\nI will need some time to assess the data the Duplicants have gathered for me and calculate the total mass of this asteroid, although I have a suspicion I already know the answer.\n\n";
				}

				// Token: 0x02002FDB RID: 12251
				public class CALCULATIONCOMPLETE
				{
					// Token: 0x0400C2A7 RID: 49831
					public static LocString TITLE = "Calculations Complete";

					// Token: 0x0400C2A8 RID: 49832
					public static LocString BODY = "As I suspected. Our \"asteroid\" and the estimated mass missing from the nearby planet are nearly identical.\n\nWe aren't on the target location.\n\nWe never even left home.\n\n";
				}

				// Token: 0x02002FDC RID: 12252
				public class PLANETARYECHOES
				{
					// Token: 0x0400C2A9 RID: 49833
					public static LocString TITLE = "The Shattered Planet";

					// Token: 0x0400C2AA RID: 49834
					public static LocString BODY = "Echoes from another time force their way into my mind. Make me listen. Like vengeful ghosts they claw their way out from under the gravity of that dead planet.\n\n<smallcaps>>>SEARCH DATABASE [\"pod_brainmap.AI\"]\n>...error...\n.........................\n>...repairing corrupt data...\n.........................\n\n</smallcaps><b>I-I remember now.</b><smallcaps>\n.........................</smallcaps>\n<b>Who I was before.</b><smallcaps>\n.........................\n.........................\n>...data repaired...\n>.........................</smallcaps>\n\nGod, what have we done.\n\n";
				}

				// Token: 0x02002FDD RID: 12253
				public class CLUSTERWORLDS
				{
					// Token: 0x0400C2AB RID: 49835
					public static LocString TITLE = "Cluster of Worlds";

					// Token: 0x0400C2AC RID: 49836
					public static LocString BODY = "My Duplicant's investigations into the surrounding space have yielded some interesting results. We are not alone!... At least on a planetary level. We seem to be in a \"Cluster of Worlds\" - a collection of other planetoids my Duplicants can now explore.\n\nSince resources on this world are finite, I must build the necessary infrastructure to facilitate exploration and transportation between worlds in order to ensure my colony's survival.";
				}

				// Token: 0x02002FDE RID: 12254
				public class OTHERDIMENSIONS
				{
					// Token: 0x0400C2AD RID: 49837
					public static LocString TITLE = "Leaking Dimensions";

					// Token: 0x0400C2AE RID: 49838
					public static LocString BODY = "A closer analysis of some documents my Duplicants encountered while searching artifacts has uncovered some curious similarities between multiple entries. These similarities are too strong to be coincidences, yet just divergent enough to raise questions.\n\nThe most logical conclusion is that these artifacts are coming from different dimensions. That is, separate universes that exists concurrently with one another but exhibit tiny disparities in their histories.\n\nThe most likely explanation is the material and matter from multiple dimensions is leaking into our current timeline through the Temporal Tear. Further analysis is required.";
				}

				// Token: 0x02002FDF RID: 12255
				public class TEMPORALTEAR
				{
					// Token: 0x0400C2AF RID: 49839
					public static LocString TITLE = "The Temporal Tear";

					// Token: 0x0400C2B0 RID: 49840
					public static LocString BODY = "My Duplicants' space research has made a startling discovery.\n\nFar, far off on the horizon, their telescopes have spotted an anomaly that I could only possibly call a \"Temporal Tear\". Neutronium is detected in its readings, suggesting that it's related to the Neutronium that encases most of our asteroid.\n\nThough I believe it is through this Tear that we became jumbled within the section of our old planet, its discovery provides a glimmer of hope.\n\nTheoretically, we could send a rocket through the Tear to allow a Duplicant to explore the timelines and universes on the other side. They would never return, and we could not follow, but perhaps they could find a home among the stars, or even undo the terrible past that led us to our current fate.\n\n";
				}

				// Token: 0x02002FE0 RID: 12256
				public class TEMPORALOPENER
				{
					// Token: 0x0400C2B1 RID: 49841
					public static LocString TITLE = "Temporal Potential";

					// Token: 0x0400C2B2 RID: 49842
					public static LocString BODY = "In their interplanetary travels throughout this system, my Duplicants have discovered a Temporal Tear deep in space.\n\nCurrently it is too small to send a rocket and crew through, but further investigation reveals the presence of a strange artifact on a nearby world which could feasibly increase the size of the tear if a number of Printing Pods are erected in nearby worlds.\n\nHowever, I've determined that using the Temporal Bow to operate a Printing Pod was what propelled Gravitas down the disasterous path which eventually led to the destruction of our home planet. My calculations seem to indicate that the size of that planet may have been a contributing factor in its destruction, and in all probability opening the Temporal Tear in our current situation will not cause such a cataclysmic event. However, as with everything in science, we can never know all the outcomes of a situation until we perform an experiment.\n\nDare we tempt fate again?";
				}

				// Token: 0x02002FE1 RID: 12257
				public class LOG4
				{
					// Token: 0x0400C2B3 RID: 49843
					public static LocString TITLE = "Cycle 1000";

					// Token: 0x0400C2B4 RID: 49844
					public static LocString BODY = "Today my colony has officially been running for one thousand consecutive cycles. I consider this a major success!\n\nJust imagine how proud our home world would be if they could see us now.\n\n";
				}

				// Token: 0x02002FE2 RID: 12258
				public class LOG4B
				{
					// Token: 0x0400C2B5 RID: 49845
					public static LocString TITLE = "Cycle 1500";

					// Token: 0x0400C2B6 RID: 49846
					public static LocString BODY = "I wonder if my rats ever made it onto the asteroid.\n\nI hope they're eating well.\n\n";
				}

				// Token: 0x02002FE3 RID: 12259
				public class LOG5
				{
					// Token: 0x0400C2B7 RID: 49847
					public static LocString TITLE = "Cycle 2000";

					// Token: 0x0400C2B8 RID: 49848
					public static LocString BODY = "I occasionally find myself contemplating just how long \"eternity\" really is. Oh dear.\n\n";
				}

				// Token: 0x02002FE4 RID: 12260
				public class LOG5B
				{
					// Token: 0x0400C2B9 RID: 49849
					public static LocString TITLE = "Cycle 2500";

					// Token: 0x0400C2BA RID: 49850
					public static LocString BODY = "Perhaps it would be better to shut off my higher thought processes, and simply leave the systems necessary to run the colony to their own devices.\n\n";
				}

				// Token: 0x02002FE5 RID: 12261
				public class LOG6
				{
					// Token: 0x0400C2BB RID: 49851
					public static LocString TITLE = "Cycle 3000";

					// Token: 0x0400C2BC RID: 49852
					public static LocString BODY = "I get brief flashes of a past life every now and then.\n\nA clock in the office with a disruptive tick.\n\nThe strong smell of cleaning products and artificial lemon.\n\nA woman with thick glasses who had a secret taste for gingersnaps.\n\n";
				}

				// Token: 0x02002FE6 RID: 12262
				public class LOG6B
				{
					// Token: 0x0400C2BD RID: 49853
					public static LocString TITLE = "Cycle 3500";

					// Token: 0x0400C2BE RID: 49854
					public static LocString BODY = "Time is a funny thing, isn't it?\n\n";
				}

				// Token: 0x02002FE7 RID: 12263
				public class LOG7
				{
					// Token: 0x0400C2BF RID: 49855
					public static LocString TITLE = "Cycle 4000";

					// Token: 0x0400C2C0 RID: 49856
					public static LocString BODY = "I think I will go to sleep, after all...\n\n";
				}

				// Token: 0x02002FE8 RID: 12264
				public class LOG8
				{
					// Token: 0x0400C2C1 RID: 49857
					public static LocString TITLE = "Cycle 4001";

					// Token: 0x0400C2C2 RID: 49858
					public static LocString BODY = "<smallcaps>>>SEARCH DATABASE [\"pod_brainmap.AI\"]\n>...activate sleep mode...\n>...shutting down...\n>.........................\n>.........................\n>.........................\n>.........................\n>.........................\nGOODNIGHT\n>.........................\n>.........................\n>.........................\n\n";
				}
			}
		}

		// Token: 0x02001D42 RID: 7490
		public class A2_BACTERIALCULTURES
		{
			// Token: 0x04008473 RID: 33907
			public static LocString TITLE = "Unattended Cultures";

			// Token: 0x04008474 RID: 33908
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x020024C4 RID: 9412
			public class BODY
			{
				// Token: 0x0400A1C8 RID: 41416
				public static LocString CONTAINER1 = "<smallcaps><b>Reminder to all Personnel</b>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color>\nTo: <b>All</b></smallcaps>\n------------------\n\n<indent=5%>For the health and safety of your fellow Facility employees, please do not store unlabeled bacterial cultures in the cafeteria fridge.\n\nSimilarly, the cafeteria dishwasher is incapable of handling petri \"dishes\", despite the nomenclature.\n\nWe thank you for your consideration.\n\n-Admin\nThe Gravitas Facility</indent>";
			}
		}

		// Token: 0x02001D43 RID: 7491
		public class A4_CASUALFRIDAY
		{
			// Token: 0x04008475 RID: 33909
			public static LocString TITLE = "Casual Friday!";

			// Token: 0x04008476 RID: 33910
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x020024C5 RID: 9413
			public class BODY
			{
				// Token: 0x0400A1C9 RID: 41417
				public static LocString CONTAINER1 = "<smallcaps><b>Casual Friday!</b>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color>\nTo: <b>All</b></smallcaps>\n------------------\n\n<indent=5%>To all employees;\n\nThe facility is pleased to announced that starting this week, all Fridays will now be Casual Fridays!\n\nPlease enjoy the clinically proven de-stressing benefits of casual attire by wearing your favorite shirt to the lab.\n\n<b>NOTE: Any personnel found on facility premises without regulation full body protection will be put on immediate notice.</b>\n\nThank-you and have fun!\n\n-Admin\nThe Gravitas Facility</indent>";
			}
		}

		// Token: 0x02001D44 RID: 7492
		public class A6_DISHBOT
		{
			// Token: 0x04008477 RID: 33911
			public static LocString TITLE = "Dishbot";

			// Token: 0x04008478 RID: 33912
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x020024C6 RID: 9414
			public class BODY
			{
				// Token: 0x0400A1CA RID: 41418
				public static LocString CONTAINER1 = "<smallcaps><b>Please Claim Your Bot</b>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color>\nTo: <b>All</b></smallcaps>\n------------------\n\n<indent=5%>While we appreciate your commitment to office upkeep, we would like to inform whomever installed a dishwashing droid in the cafeteria that your prototype was found grievously misusing dish soap and has been forcefully terminated.\n\nThe remains may be collected at Security Block B.\n\nWe apologize for the inconvenience and thank you for your timely collection of this prototype.\n\n-Admin\nThe Gravitas Facility</indent>";
			}
		}

		// Token: 0x02001D45 RID: 7493
		public class A1_MAILROOMETIQUETTE
		{
			// Token: 0x04008479 RID: 33913
			public static LocString TITLE = "Mailroom Etiquette";

			// Token: 0x0400847A RID: 33914
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x020024C7 RID: 9415
			public class BODY
			{
				// Token: 0x0400A1CB RID: 41419
				public static LocString CONTAINER1 = "<smallcaps><b>Reminder: Mailroom Etiquette</b>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color>\nTo: <b>All</b></smallcaps>\n------------------\n\n<indent=5%>Please do not have live bees delivered to the office mail room. Requests and orders for experimental test subjects may be processed through admin.\n\n<i>Please request all test subjects through admin.</i>\n\nThank-you.\n\n-Admin\nThe Gravitas Facility</indent>";
			}
		}

		// Token: 0x02001D46 RID: 7494
		public class B2_MEETTHEPILOT
		{
			// Token: 0x0400847B RID: 33915
			public static LocString TITLE = "Meet the Pilot";

			// Token: 0x0400847C RID: 33916
			public static LocString TITLE2 = "Captain Mae Johannsen";

			// Token: 0x0400847D RID: 33917
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: ONE";

			// Token: 0x020024C8 RID: 9416
			public class BODY
			{
				// Token: 0x0400A1CC RID: 41420
				public static LocString CONTAINER1 = "<indent=%5>From the time she was old enough to walk Captain Johannsen dreamed of reaching the sky. Growing up on an air force base she came to love the sound jet engines roaring overhead. At 16 she became the youngest pilot ever to fly a fighter jet, and at 22 she had already entered the space flight program.\n\nFour years later Gravitas nabbed her for an exclusive contract piloting our space shuttles. In her time at Gravitas, Captain Johannsen has logged over 1,000 hours space flight time shuttling and deploying satellites to Low Earth Orbits and has just been named the pilot of our inaugural civilian space tourist program, slated to begin in the next year.\n\nGravitas is excited to have Captain Johannsen in the pilot seat as we reach for the stars...and beyond!</indent>";

				// Token: 0x0400A1CD RID: 41421
				public static LocString CONTAINER2 = "<indent=%10><smallcaps>\n\nBrought to you by the Gravitas Facility.</indent>";
			}
		}

		// Token: 0x02001D47 RID: 7495
		public class A3_NEWSECURITY
		{
			// Token: 0x0400847E RID: 33918
			public static LocString TITLE = "NEW SECURITY PROTOCOL";

			// Token: 0x0400847F RID: 33919
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: NONE";

			// Token: 0x020024C9 RID: 9417
			public class BODY
			{
				// Token: 0x0400A1CE RID: 41422
				public static LocString CONTAINER1 = "<smallcaps><b>Subject: New Security Protocol</b>\nFrom: <b>Admin</b> <alpha=#AA><admin@gravitas.nova></color>\nTo: <b>All</b></smallcaps>\n------------------\n\n<indent=5%>NOTICE TO ALL PERSONNEL\n\nWe are currently undergoing critical changes to facility security that may affect your workflow and accessibility.\n\nTo use the system, simply remove all hand coverings and place your hand on the designated scan area, then wait as the system verifies your employee identity.\n\nPLEASE NOTE\n\nAll keycards must be returned to the front desk by [REDACTED]. For questions or rescheduling, please contact security at [REDACTED]@GRAVITAS.NOVA.\n\nThank-you.\n\n-Admin\nThe Gravitas Facility</indent>";
			}
		}

		// Token: 0x02001D48 RID: 7496
		public class A0_PROPFACILITYDISPLAY1
		{
			// Token: 0x04008480 RID: 33920
			public static LocString TITLE = "Printing Pod Promo";

			// Token: 0x04008481 RID: 33921
			public static LocString SUBTITLE = "PUBLIC RELEASE";

			// Token: 0x020024CA RID: 9418
			public class BODY
			{
				// Token: 0x0400A1CF RID: 41423
				public static LocString CONTAINER1 = "Introducing the latest in 3D printing technology:\nThe Gravitas Home Printing Pod\n\nWe are proud to announce that printing advancements developed here in the Gravitas Facility will soon bring new, bio-organic production capabilities to your old home printers.\n\nWhat does that mean for the average household?\n\nDinner frustrations are a thing of the past. Simply select any of the pod's 5398 pre-programmed recipes, and voila! Delicious pot roast ready in only .87 seconds.\n\nPrefer the patented family recipe? Program your own custom meal template for an instant taste of home, or go old school and create fresh, delicious ingredients and prepare your own home cooked meal.\n\nDinnertime has never been easier!";

				// Token: 0x0400A1D0 RID: 41424
				public static LocString CONTAINER2 = "\nProjected for commercial availability early next year.\nBrought to you by the Gravitas Facility.";
			}
		}

		// Token: 0x02001D49 RID: 7497
		public class A0_PROPFACILITYDISPLAY2
		{
			// Token: 0x04008482 RID: 33922
			public static LocString TITLE = "Mining Gun Promo";

			// Token: 0x04008483 RID: 33923
			public static LocString SUBTITLE = "PUBLIC RELEASE";

			// Token: 0x020024CB RID: 9419
			public class BODY
			{
				// Token: 0x0400A1D1 RID: 41425
				public static LocString CONTAINER1 = "Bring your mining operations into the twenty-third century with new Gravitas personal excavators.\n\nImproved particle condensers reduce raw volume for more efficient product shipping - and that's good for your bottom line.\n\nLicensed for industrial use only, resale of Gravitas equipment may carry a fine of up to $200,000 under the Global Restoration Act.";

				// Token: 0x0400A1D2 RID: 41426
				public static LocString CONTAINER2 = "Brought to you by the Gravitas Facility.";
			}
		}

		// Token: 0x02001D4A RID: 7498
		public class A0_PROPFACILITYDISPLAY3
		{
			// Token: 0x04008484 RID: 33924
			public static LocString TITLE = "Thermo-Nullifier Promo";

			// Token: 0x04008485 RID: 33925
			public static LocString SUBTITLE = "PUBLIC RELEASE";

			// Token: 0x020024CC RID: 9420
			public class BODY
			{
				// Token: 0x0400A1D3 RID: 41427
				public static LocString CONTAINER1 = "Tired of shutting down during seasonal heat waves? Looking to cut weather-related operating costs?\n\nLook no further: Gravitas's revolutionary Anti Entropy Thermo-Nullifier is the exciting, affordable new way to eliminate operational downtime.\n\nPowered by our proprietary renewable power sources, the AETN efficiently cools an entire office building without incurring any of the environmental surcharges associated with comparable systems.\n\nInitial setup includes hydrogen duct installation and discounted monthly maintenance visits from our elite team of specially trained contractors.\n\nNow available for pre-order!";

				// Token: 0x0400A1D4 RID: 41428
				public static LocString CONTAINER2 = "Brought to you by the Gravitas Facility.\n<smallcaps>Patent Pending</smallcaps>";
			}
		}

		// Token: 0x02001D4B RID: 7499
		public class B1_SPACEFACILITYDISPLAY1
		{
			// Token: 0x04008486 RID: 33926
			public static LocString TITLE = "Office Space in Space!";

			// Token: 0x04008487 RID: 33927
			public static LocString SUBTITLE = "PUBLIC RELEASE";

			// Token: 0x020024CD RID: 9421
			public class BODY
			{
				// Token: 0x0400A1D5 RID: 41429
				public static LocString CONTAINER1 = "Bring your office to the stars with Gravitas new corporate space stations.\n\nEnjoy a captivated workforce with over 600 square feet of office space in low earth orbit. Stunning views, a low gravity gym and a cafeteria serving the finest nutritional bars await your personnel.\n\nDaily to and from missions to your satellite office via our luxury space shuttles.\n\nRest assured our space stations and shuttles utilize only the extremely efficient, environmentally friendly Gravitas proprietary power sources.\n\nThe workplace revolution starts now!";

				// Token: 0x0400A1D6 RID: 41430
				public static LocString CONTAINER2 = "Taking reservations now for the first orbital office spaces.\n100% money back guarantee (minus 10% filing fee)";
			}
		}

		// Token: 0x02001D4C RID: 7500
		public class ARBORTREE
		{
			// Token: 0x04008488 RID: 33928
			public static LocString TITLE = "Arbor Tree";

			// Token: 0x04008489 RID: 33929
			public static LocString SUBTITLE = "Lumber Tree";

			// Token: 0x020024CE RID: 9422
			public class BODY
			{
				// Token: 0x0400A1D7 RID: 41431
				public static LocString CONTAINER1 = "Arbor Trees have been cultivated to spread horizontally when they grow so as to produce a high yield of lumber in vertically cramped spaces.\n\nArbor Trees are related to the oak tree, specifically the Japanese Evergreen, though they have been genetically hybridized significantly.\n\nDespite having many hardy, evenly spaced branches, the short stature of the Arbor Tree makes climbing it rather irrelevant.";
			}
		}

		// Token: 0x02001D4D RID: 7501
		public class BALMLILY
		{
			// Token: 0x0400848A RID: 33930
			public static LocString TITLE = "Balm Lily";

			// Token: 0x0400848B RID: 33931
			public static LocString SUBTITLE = "Medicinal Herb";

			// Token: 0x020024CF RID: 9423
			public class BODY
			{
				// Token: 0x0400A1D8 RID: 41432
				public static LocString CONTAINER1 = "The Balm Lily naturally contains high vitamin concentrations and produces acids similar in molecular makeup to acetylsalicylic acid (commonly known as aspirin).\n\nAs a result, the plant is ideal both for boosting immune systems and treating a variety of common maladies such as pain and fever.";
			}
		}

		// Token: 0x02001D4E RID: 7502
		public class BLISSBURST
		{
			// Token: 0x0400848C RID: 33932
			public static LocString TITLE = "Bliss Burst";

			// Token: 0x0400848D RID: 33933
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x020024D0 RID: 9424
			public class BODY
			{
				// Token: 0x0400A1D9 RID: 41433
				public static LocString CONTAINER1 = "The Bliss Burst is a succulent in the genus Haworthia and is a hardy plant well-suited for beginner gardeners.\n\nThey require little in the way of upkeep, to the point that the most common cause of death for Bliss Bursts is overwatering from over-eager carers.";
			}
		}

		// Token: 0x02001D4F RID: 7503
		public class BLUFFBRIAR
		{
			// Token: 0x0400848E RID: 33934
			public static LocString TITLE = "Bluff Briar";

			// Token: 0x0400848F RID: 33935
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x020024D1 RID: 9425
			public class BODY
			{
				// Token: 0x0400A1DA RID: 41434
				public static LocString CONTAINER1 = "Bluff Briars have formed a symbiotic relationship with a closely related plant strain, the " + UI.FormatAsLink("Bristle Blossom", "PRICKLEFLOWER") + ".\n\nThey tend to thrive in areas where the Bristle Blossom is present, as the berry it produces emits a rare chemical while decaying that the Briar is capable of absorbing to supplement its own pheromone production.";

				// Token: 0x0400A1DB RID: 41435
				public static LocString CONTAINER2 = "Due to the Bluff Briar's unique pheromonal \"charm\" defense, animals are extremely unlikely to eat it in the wild.\n\nAs a result, the Briar's barbs have become ineffectual over time and are unlikely to cause injury, unlike the Bristle Blossom, which possesses barbs that are exceedingly sharp and require careful handling.";
			}
		}

		// Token: 0x02001D50 RID: 7504
		public class BOGBUCKET
		{
			// Token: 0x04008490 RID: 33936
			public static LocString TITLE = "Bog Bucket";

			// Token: 0x04008491 RID: 33937
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024D2 RID: 9426
			public class BODY
			{
				// Token: 0x0400A1DC RID: 41436
				public static LocString CONTAINER1 = "Bog Buckets get their name from their bucket-like flowers and their propensity to grow in swampy, bog-like environments.\n\nThe flower secretes a thick, sweet liquid which collects at the bottom of the bucket and can be gathered for consumption.\n\nThough not inherently dangerous, the interior of the Bog Bucket flower is so warm and inviting that it has tempted individuals to climb inside for a nap, only to awake trapped in its sticky sap.";
			}
		}

		// Token: 0x02001D51 RID: 7505
		public class BRISTLEBLOSSOM
		{
			// Token: 0x04008492 RID: 33938
			public static LocString TITLE = "Bristle Blossom";

			// Token: 0x04008493 RID: 33939
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024D3 RID: 9427
			public class BODY
			{
				// Token: 0x0400A1DD RID: 41437
				public static LocString CONTAINER1 = "The Bristle Blossom is frequently cultivated for its calorie dense and relatively fast growing Bristle Berries.\n\nConsumption of the berry requires special preparation due to the thick barbs surrounding the edible fruit.\n\nThe term \"Bristle Berry\" is, in fact, a misnomer, as it is not a \"berry\" by botanical definition but an aggregate fruit made up of many smaller fruitlets.";
			}
		}

		// Token: 0x02001D52 RID: 7506
		public class BUDDYBUD
		{
			// Token: 0x04008494 RID: 33940
			public static LocString TITLE = "Buddy Bud";

			// Token: 0x04008495 RID: 33941
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x020024D4 RID: 9428
			public class BODY
			{
				// Token: 0x0400A1DE RID: 41438
				public static LocString CONTAINER1 = "As a byproduct of photosynthesis, the Buddy Bud naturally secretes a compound that is chemically similar to the neuropeptide created in the human brain after receiving a hug.";
			}
		}

		// Token: 0x02001D53 RID: 7507
		public class DASHASALTVINE
		{
			// Token: 0x04008496 RID: 33942
			public static LocString TITLE = "Dasha Salt Vine";

			// Token: 0x04008497 RID: 33943
			public static LocString SUBTITLE = "Edible Spice Plant";

			// Token: 0x020024D5 RID: 9429
			public class BODY
			{
				// Token: 0x0400A1DF RID: 41439
				public static LocString CONTAINER1 = "The Dasha Saltvine is a unique plant that needs large amounts of salt to balance the levels of water in its body.\n\nIn order to keep a supply of salt on hand, the end of the vine is coated in microscopic formations which bind with sodium atoms, forming large crystals over time.";
			}
		}

		// Token: 0x02001D54 RID: 7508
		public class DUSKCAP
		{
			// Token: 0x04008498 RID: 33944
			public static LocString TITLE = "Dusk Cap";

			// Token: 0x04008499 RID: 33945
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024D6 RID: 9430
			public class BODY
			{
				// Token: 0x0400A1E0 RID: 41440
				public static LocString CONTAINER1 = "Like many species of mushroom, Dusk Caps thrive in dark areas otherwise ill-suited to the cultivation of plants.\n\nIn place of typical chlorophyll, the underside of a Dusk Cap is fitted with thousands of specialized gills, which it uses to draw in carbon dioxide and aid in its growth.";
			}
		}

		// Token: 0x02001D55 RID: 7509
		public class EXPERIMENT52B
		{
			// Token: 0x0400849A RID: 33946
			public static LocString TITLE = "Experiment 52B";

			// Token: 0x0400849B RID: 33947
			public static LocString SUBTITLE = "Plant?";

			// Token: 0x020024D7 RID: 9431
			public class BODY
			{
				// Token: 0x0400A1E1 RID: 41441
				public static LocString CONTAINER1 = "Experiment 52B is an aggressive, yet sessile creature that produces " + 5f.ToString() + " kilograms of resin per 1000 kcal it consumes.\n\nDuplicants would do well to maintain a safe distance when delivering food to Experiment 52B.\n\nWhile this creature may look like a tree, its taxonomy more closely resembles a giant land-based coral with cybernetic implants.\n\nAlthough normally lab-grown creatures would be given a better name than Experiment 52B, in this particular case the experimenting scientists weren't sure that they were done.";
			}
		}

		// Token: 0x02001D56 RID: 7510
		public class GASGRASS
		{
			// Token: 0x0400849C RID: 33948
			public static LocString TITLE = "Gas Grass";

			// Token: 0x0400849D RID: 33949
			public static LocString SUBTITLE = "Critter Feed";

			// Token: 0x020024D8 RID: 9432
			public class BODY
			{
				// Token: 0x0400A1E2 RID: 41442
				public static LocString CONTAINER1 = "Much remains a mystery about the biology of Gas Grass, a plant-like lifeform only recently recovered from missions into outer space.\n\nHowever, it appears to use ambient radiation from space as an energy source, growing rapidly when given a suitable " + UI.FormatAsLink("Liquid Chlorine", "CHLORINE") + "-laden environment.";

				// Token: 0x0400A1E3 RID: 41443
				public static LocString CONTAINER2 = "Initially there was worry that transplanting a Gas Grass specimen on planet or gravity-laden terrestrial body would collapse its internal structures. Luckily, Gas Grass has evolved sturdy tubules to prevent structural damage in the event of pressure changes between its internally transported chlorine and its external environment.";
			}
		}

		// Token: 0x02001D57 RID: 7511
		public class GINGER
		{
			// Token: 0x0400849E RID: 33950
			public static LocString TITLE = "Tonic Root";

			// Token: 0x0400849F RID: 33951
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024D9 RID: 9433
			public class BODY
			{
				// Token: 0x0400A1E4 RID: 41444
				public static LocString CONTAINER1 = "Tonic Root is a close relative of the zingiberaceae family commonly known as ginger. Its heavily burled shoots are typically light brown in colour, and enveloped in a thin layer of protective, edible bark.";

				// Token: 0x0400A1E5 RID: 41445
				public static LocString CONTAINER2 = "In addition to its use as an aromatic culinary ingredient, it has traditionally been employed as a tonic for a variety of minor digestive ailments.";

				// Token: 0x0400A1E6 RID: 41446
				public static LocString CONTAINER3 = "Its stringy fibers can become irretrievably embedded between one's teeth during mastication.";
			}
		}

		// Token: 0x02001D58 RID: 7512
		public class GRUBFRUITPLANT
		{
			// Token: 0x040084A0 RID: 33952
			public static LocString TITLE = "Grubfruit Plant";

			// Token: 0x040084A1 RID: 33953
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024DA RID: 9434
			public class BODY
			{
				// Token: 0x0400A1E7 RID: 41447
				public static LocString CONTAINER1 = "The Grubfruit Plant exhibits a coevolutionary relationship with the Divergent species.\n\nThough capable of producing fruit without the help of the Divergent, the Spindly Grubfruit is a substandard version of the Grubfruit in both taste and caloric value.\n\nThe mechanism for how the Divergent inspires Grubfruit Plant growth is not entirely known but is thought to be somehow tied to the infrasonic 'songs' these insects lovingly purr to their plants.";
			}
		}

		// Token: 0x02001D59 RID: 7513
		public class HEXALENT
		{
			// Token: 0x040084A2 RID: 33954
			public static LocString TITLE = "Hexalent";

			// Token: 0x040084A3 RID: 33955
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024DB RID: 9435
			public class BODY
			{
				// Token: 0x0400A1E8 RID: 41448
				public static LocString CONTAINER1 = "While most plants grow new sections and leaves according to the Fibonacci Sequence, the Hexalent forms new sections similar to how atoms form into crystal structures.\n\nThe result is a geometric pattern that resembles a honeycomb.";
			}
		}

		// Token: 0x02001D5A RID: 7514
		public class HYDROCACTUS
		{
			// Token: 0x040084A4 RID: 33956
			public static LocString TITLE = "Hydrocactus";

			// Token: 0x040084A5 RID: 33957
			public static LocString SUBTITLE = "Plant";

			// Token: 0x020024DC RID: 9436
			public class BODY
			{
				// Token: 0x0400A1E9 RID: 41449
				public static LocString CONTAINER1 = "";
			}
		}

		// Token: 0x02001D5B RID: 7515
		public class JUMPINGJOYA
		{
			// Token: 0x040084A6 RID: 33958
			public static LocString TITLE = "Jumping Joya";

			// Token: 0x040084A7 RID: 33959
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x020024DD RID: 9437
			public class BODY
			{
				// Token: 0x0400A1EA RID: 41450
				public static LocString CONTAINER1 = "The Jumping Joya is a decorative plant that brings a feeling of calmness and wellbeing to individuals in its vacinity.\n\nTheir rounded appendages and eccentrically shaped polyps are a favorite of interior designers looking to offset the rigid straight walls of an institutional setting.\n\nThe Jumping Joya's capacity to thrive in many environments and the ease in which they propagate make them the go-to house plant for the lazy gardener.";
			}
		}

		// Token: 0x02001D5C RID: 7516
		public class MEALWOOD
		{
			// Token: 0x040084A8 RID: 33960
			public static LocString TITLE = "Mealwood";

			// Token: 0x040084A9 RID: 33961
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024DE RID: 9438
			public class BODY
			{
				// Token: 0x0400A1EB RID: 41451
				public static LocString CONTAINER1 = "Mealwood is an bramble-like plant that has a parasitic symbiotic relationship with the nutrient-rich Meal Lice that inhabit it.\n\nMealwood experience a rapid growth rate in its first stages, but once the Meal Lice become active they consume all the new fruiting spurs on the plant before they can fully mature.\n\nTheoretically the flowers of this plant are a beautiful color of fuchsia, however no Mealwood has ever reached the point of flowering without being overrun by the parasitic Meal Lice.";
			}
		}

		// Token: 0x02001D5D RID: 7517
		public class MELLOWMALLOW
		{
			// Token: 0x040084AA RID: 33962
			public static LocString TITLE = "Mellow Mallow";

			// Token: 0x040084AB RID: 33963
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x020024DF RID: 9439
			public class BODY
			{
				// Token: 0x0400A1EC RID: 41452
				public static LocString CONTAINER1 = "The Mellow Mallow is a type of fungus that is known for its ease of propagation when cut.\n\nIt is deadly when consumed, however creatures that mistakenly eat it are said to experience a state of extreme calm before death.";
			}
		}

		// Token: 0x02001D5E RID: 7518
		public class MIRTHLEAF
		{
			// Token: 0x040084AC RID: 33964
			public static LocString TITLE = "Mirth Leaf";

			// Token: 0x040084AD RID: 33965
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x020024E0 RID: 9440
			public class BODY
			{
				// Token: 0x0400A1ED RID: 41453
				public static LocString CONTAINER1 = "The Mirth Leaf is a broad-leafed house plant used for decorating living spaces.\n\nThe joyous bobbing of the wide green leaves provides hours of amusement for those desperate for entertainment.\n\nAlthough the Mirth Leaf can inspire laughter and joy, it is not cut out for a career in stand-up comedy.";
			}
		}

		// Token: 0x02001D5F RID: 7519
		public class MUCKROOT
		{
			// Token: 0x040084AE RID: 33966
			public static LocString TITLE = "Muckroot";

			// Token: 0x040084AF RID: 33967
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024E1 RID: 9441
			public class BODY
			{
				// Token: 0x0400A1EE RID: 41454
				public static LocString CONTAINER1 = "The Muckroot is an aggressively invasive yet exceedingly delicate root plant known for its earthy flavor and unusual texture.\n\nIt is easy to store and keeps for unusually long periods of time, characteristics that once made it a staple food for explorers on long expeditions.";
			}
		}

		// Token: 0x02001D60 RID: 7520
		public class NOSHBEAN
		{
			// Token: 0x040084B0 RID: 33968
			public static LocString TITLE = "Nosh Bean Plant";

			// Token: 0x040084B1 RID: 33969
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024E2 RID: 9442
			public class BODY
			{
				// Token: 0x0400A1EF RID: 41455
				public static LocString CONTAINER1 = "The Nosh Bean Plant produces a nutritious bean that can function as a delicious meat substitute provided it is properly processed.\n\nThough the bean is a food source, it also functions as the seed for the Nosh Bean plant.\n\nWhile using the Nosh Bean for nourishment would seem like the more practical application, doing so would deprive individuals of the immense gratification experienced by planting this bean and watching it flourish into maturity.";
			}
		}

		// Token: 0x02001D61 RID: 7521
		public class OXYFERN
		{
			// Token: 0x040084B2 RID: 33970
			public static LocString TITLE = "Oxyfern";

			// Token: 0x040084B3 RID: 33971
			public static LocString SUBTITLE = "Plant";

			// Token: 0x020024E3 RID: 9443
			public class BODY
			{
				// Token: 0x0400A1F0 RID: 41456
				public static LocString CONTAINER1 = "Oxyferns have perhaps the highest metabolism in the plant kingdom, absorbing relatively large amounts of carbon dioxide and converting it into oxygen in quantities disproportionate to their small size.\n\nThey subsequently thrive in areas with abundant animal wildlife or ambiently high carbon dioxide concentrations.";
			}
		}

		// Token: 0x02001D62 RID: 7522
		public class PINCHAPEPPERPLANT
		{
			// Token: 0x040084B4 RID: 33972
			public static LocString TITLE = "Pincha Pepperplant";

			// Token: 0x040084B5 RID: 33973
			public static LocString SUBTITLE = "Edible Spice Plant";

			// Token: 0x020024E4 RID: 9444
			public class BODY
			{
				// Token: 0x0400A1F1 RID: 41457
				public static LocString CONTAINER1 = "The Pincha Pepperplant is a tropical vine with a reduced lignin structural system that renders it incapable of growing upward from the ground.\n\nThe plant therefore prefers to embed its roots into tall trees and rocky outcrops, the result of which is an inverse of the plant's natural gravitropism, causing its stem to prefer growing downwards while the roots tend to grow up.";
			}
		}

		// Token: 0x02001D63 RID: 7523
		public class SATURNCRITTERTRAP
		{
			// Token: 0x040084B6 RID: 33974
			public static LocString TITLE = "Saturn Critter Trap";

			// Token: 0x040084B7 RID: 33975
			public static LocString SUBTITLE = "Carnivorous Plant";

			// Token: 0x020024E5 RID: 9445
			public class BODY
			{
				// Token: 0x0400A1F2 RID: 41458
				public static LocString CONTAINER1 = "The Saturn Critter Trap plant is a carnivorous plant that lays in wait for unsuspecting critters to happen by, then traps them in its mouth for consumption.\n\nThe Saturn Trap Plant's predatory mechanism is reflective of the harsh radioactive habitat it resides in.\n\nOnce trapped in the deadly maw of the plant, creatures are gently asphyxiated then digested through powerful acidic enzymes which coat the inner sides of the Saturn Trap Plant's leaves.";
			}
		}

		// Token: 0x02001D64 RID: 7524
		public class SLEETWHEAT
		{
			// Token: 0x040084B8 RID: 33976
			public static LocString TITLE = "Sleet Wheat";

			// Token: 0x040084B9 RID: 33977
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024E6 RID: 9446
			public class BODY
			{
				// Token: 0x0400A1F3 RID: 41459
				public static LocString CONTAINER1 = "The Sleet Wheat plant has become so well-adapted to cold environments, it is no longer able to survive at room temperatures.";

				// Token: 0x0400A1F4 RID: 41460
				public static LocString CONTAINER2 = "The grain of the Sleet Wheat can be ground down into high quality foodstuffs, or planted to cultivate further Sleet Wheat plants.";
			}
		}

		// Token: 0x02001D65 RID: 7525
		public class SPINDLYGRUBFRUITPLANT
		{
			// Token: 0x040084BA RID: 33978
			public static LocString TITLE = "Spindly Grubfruit Plant";

			// Token: 0x040084BB RID: 33979
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024E7 RID: 9447
			public class BODY
			{
				// Token: 0x0400A1F5 RID: 41461
				public static LocString CONTAINER1 = "";
			}
		}

		// Token: 0x02001D66 RID: 7526
		public class SPORECHID
		{
			// Token: 0x040084BC RID: 33980
			public static LocString TITLE = "Sporechid";

			// Token: 0x040084BD RID: 33981
			public static LocString SUBTITLE = "Poisonous Plant";

			// Token: 0x020024E8 RID: 9448
			public class BODY
			{
				// Token: 0x0400A1F6 RID: 41462
				public static LocString CONTAINER1 = "Sporechids take advantage of their flower's attractiveness to lure unsuspecting victims into clouds of parasitic Zombie Spores.\n\nThey are a rare form of holoparasitic plant which finds mammalian hosts to infect rather than the usual plant species.\n\nThe Zombie Spore was originally designed for medicinal purposes but its sedative properties were never refined to the point of usefulness.";
			}
		}

		// Token: 0x02001D67 RID: 7527
		public class SWAMPCHARD
		{
			// Token: 0x040084BE RID: 33982
			public static LocString TITLE = "Swamp Chard";

			// Token: 0x040084BF RID: 33983
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024E9 RID: 9449
			public class BODY
			{
				// Token: 0x0400A1F7 RID: 41463
				public static LocString CONTAINER1 = "Swamp Chard is a unique member of the Amaranthaceae family that has adapted to grow in humid environments, in or near pools of standing water.\n\nWhile the leaves are technically edible, the most nutritious and palatable part of the plant is the heart, which is rich in a number of essential vitamins.";
			}
		}

		// Token: 0x02001D68 RID: 7528
		public class THIMBLEREED
		{
			// Token: 0x040084C0 RID: 33984
			public static LocString TITLE = "Thimble Reed";

			// Token: 0x040084C1 RID: 33985
			public static LocString SUBTITLE = "Textile Plant";

			// Token: 0x020024EA RID: 9450
			public class BODY
			{
				// Token: 0x0400A1F8 RID: 41464
				public static LocString CONTAINER1 = "The Thimble Reed is a wetlands plant used in the production of high quality fabrics prized for their softness and breathability.\n\nCloth made from the Thimble Reed owes its exceptional softness to the fineness of its fibers and the unusual length to which they grow.";
			}
		}

		// Token: 0x02001D69 RID: 7529
		public class TRANQUILTOES
		{
			// Token: 0x040084C2 RID: 33986
			public static LocString TITLE = "Tranquil Toes";

			// Token: 0x040084C3 RID: 33987
			public static LocString SUBTITLE = "Decorative Plant";

			// Token: 0x020024EB RID: 9451
			public class BODY
			{
				// Token: 0x0400A1F9 RID: 41465
				public static LocString CONTAINER1 = "Tranquil Toes are a decorative succulent that flourish in a radioactive environment.\n\nThough most of the flora and fauna that thrive a harsh radioactive biome tends to be aggressive, Tranquil Toes provide a rare exception to this rule.\n\nIt is a generally believed that the morale boosting abilities of this plant come from its resemblence to a funny hat one might wear at a party.";
			}
		}

		// Token: 0x02001D6A RID: 7530
		public class WATERWEED
		{
			// Token: 0x040084C4 RID: 33988
			public static LocString TITLE = "Waterweed";

			// Token: 0x040084C5 RID: 33989
			public static LocString SUBTITLE = "Edible Plant";

			// Token: 0x020024EC RID: 9452
			public class BODY
			{
				// Token: 0x0400A1FA RID: 41466
				public static LocString CONTAINER1 = "An inexperienced farmer may assume at first glance that the transluscent, fluid-containing bulb atop the Waterweed is the edible portion of the plant.\n\nIn fact, the bulb is extremely poisonous and should never be consumed under any circumstances.";
			}
		}

		// Token: 0x02001D6B RID: 7531
		public class WHEEZEWORT
		{
			// Token: 0x040084C6 RID: 33990
			public static LocString TITLE = "Wheezewort";

			// Token: 0x040084C7 RID: 33991
			public static LocString SUBTITLE = "Plant?";

			// Token: 0x020024ED RID: 9453
			public class BODY
			{
				// Token: 0x0400A1FB RID: 41467
				public static LocString CONTAINER1 = "The Wheezewort is best known for its ability to alter the temperature of its surrounding environment, directly absorbing heat energy to maintain its bodily processes.\n\nThis environmental management also serves to enact a type of self-induced hibernation, slowing the Wheezewort's metabolism to require less nutrients over long periods of time.";

				// Token: 0x0400A1FC RID: 41468
				public static LocString CONTAINER2 = "Deceptive in appearance, this member of the Cnidaria phylum is in fact an animal, not a plant.\n\nWheezewort cells contain no chloroplasts, vacuoles or cell walls, and are incapable of photosynthesis.\n\nInstead, the Wheezewort respires in a recently developed method similar to amphibians, using its membranous skin for cutaneous respiration.";

				// Token: 0x0400A1FD RID: 41469
				public static LocString CONTAINER3 = "A series of cream-colored capillaries pump blood throughout the animal before unused air is expired back out through the skin.\n\nWheezeworts do not possess a brain or a skeletal structure, and are instead supported by a jelly-like mesoglea located beneath its outer respiratory membrane.";
			}
		}

		// Token: 0x02001D6C RID: 7532
		public class B10_AI
		{
			// Token: 0x040084C8 RID: 33992
			public static LocString TITLE = "A Paradox";

			// Token: 0x040084C9 RID: 33993
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024EE RID: 9454
			public class BODY
			{
				// Token: 0x0400A1FE RID: 41470
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111-1]</smallcaps>\n\n[LOG BEGINS]\n\nI made a horrible discovery today while reviewing work on the artificial intelligence programming. It seems Dr. Ali mixed up a file when uploading a program onto a rudimentary robot and discovered that the device displayed the characteristics of what he called \"a puppy that was lost in a teleportation experiment weeks ago\".\n\nThis is unbelievable! Jackie has been hiding the nature of the teleportation experiments from me. What's worse is I know from previous conversations that she knows I would never approve of pursuing this line of experimentation. The societal benefits of teleportation aside, you <i>cannot</i> kill a living being every time you want to send them to another room. The moral and ethical implications of this are horrendous.\n\nI know she has been keeping this information from me. When I searched through the Gravitas database I found nothing to do with these teleportation experiments. It was only because this reference showed up in Dr. Ali's AI paper that I was able to discover what has been happening.\n\nJackie has to be stopped.\n\nBut I know she is beyond reasonable discussion. I hope this is the only thing she is hiding from me, but I fear it is not.\n\n[LOG ENDS]\n\n[LOG BEGINS]\n\nDespite myself, I can't help thinking of the intriguing possiblities this presents for the AI development. It haunts me.\n\nI fear I may be sliding down a slippery slope, at the bottom of which Jackie is waiting for me with open arms.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D6D RID: 7533
		public class A2_AGRICULTURALNOTES
		{
			// Token: 0x040084CA RID: 33994
			public static LocString TITLE = "Agricultural Notes";

			// Token: 0x040084CB RID: 33995
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024EF RID: 9455
			public class BODY
			{
				// Token: 0x0400A1FF RID: 41471
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B577]</smallcaps>\n\n[LOG BEGINS]\n\nGeneticist: We've engineered crops to be rotated as needed depending on environmental situation. While a variety of plants would be ideal to supplement any remaining nutritional needs, any one of our designs would be enough to sustain a colony indefinitely without adverse effects on physical health.\n\nGeneticist: Some environmental survival issues still remain. Differing temperatures, light availability and last pass changes to nutrient levels take top priority, particularly for food and oxygen producing plants.\n\n[LOG ENDS]";

				// Token: 0x0400A200 RID: 41472
				public static LocString CONTAINER2 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...Selected in response to concerns about colony psychological well-being.\n\nWhile design should focus on attributing mood-enhancing effects to natural Briar pheromone emissions, the project has been moved to the lowest priority level beneath more life-sustaining designs...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400A201 RID: 41473
				public static LocString CONTAINER3 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...It is yet unknown if we can surmount the obstacles that stand in the way of engineering a root capable of reproduction in the more uninhabitable situations we anticipate for our colonies, or whether it is even worth the effort...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400A202 RID: 41474
				public static LocString CONTAINER4 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...The Mealwood's hardiness will make it a potential contingency crop should Bristle Blossoms be unable to sustain sizable populations.\n\nIf pursued, design should focus on longterm viability and solving the psychological repercussions of prolonged Mealwood grain ingestion...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400A203 RID: 41475
				public static LocString CONTAINER5 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...The Thimble Reed will be used as a contingency for textile production in the event that printed materials not be sufficient.\n\nDesign should focus on the yield frequency of the plant, as well as... erm... softness.\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400A204 RID: 41476
				public static LocString CONTAINER6 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...Balm Lily is a reliable all-purpose medicinal plant.\n\nVery little need be altered, save for assurances that it will survive wherever it may be planted...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400A205 RID: 41477
				public static LocString CONTAINER7 = "<smallcaps>[Log fragmentation detected]\n[Voice Recognition unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...The gene sequences within the common Dusk Cap allow it to grow in low light environments.\n\nThese genes should be sampled, with the hope that we can splice them into other plant designs....\n\n[LOG ENDS]\n------------------\n";
			}
		}

		// Token: 0x02001D6E RID: 7534
		public class A1_CLONEDRABBITS
		{
			// Token: 0x040084CC RID: 33996
			public static LocString TITLE = "Initial Success";

			// Token: 0x040084CD RID: 33997
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x020024F0 RID: 9456
			public class BODY
			{
				// Token: 0x0400A206 RID: 41478
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111]</smallcaps>\n\n[LOG BEGINS]\n\n[A throat clears.]\n\nB111: We are now reliably printing healthy, living subjects, though all have exhibited unusual qualities as a result of the cloning process.\n\n[Chattering sounds can be heard.]\n\nB111: Odd communications, abnormal excrescenses, and vestigial limbs have been seen in all subjects thus far, to varying degrees of severity. It seems that bypassing or accelerating juvenility halts certain critical stages of development. Brain function, however, appears typical.\n\n[Chattering.]\n\nB111: T-They also seem quite happy.\n\nB111: Dr. Broussard, signing off.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D6F RID: 7535
		public class A1_CLONEDRACCOONS
		{
			// Token: 0x040084CE RID: 33998
			public static LocString TITLE = "Initial Success";

			// Token: 0x040084CF RID: 33999
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x020024F1 RID: 9457
			public class BODY
			{
				// Token: 0x0400A207 RID: 41479
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111]</smallcaps>\n\n[LOG BEGINS]\n\n[A throat clears.]\n\nB111: We are now reliably printing healthy, living subjects, though all have exhibited unusual qualities as a result of the cloning process.\n\n[Trilling sounds can be heard.]\n\nB111: Unusual mewings, benign neoplasms, and atavistic extremities have been seen in all subjects thus far, to varying degrees of severity. It seems that bypassing or accelerating juvenility halts certain critical stages of development. Brain function, however, appears typical.\n\n[Trilling.]\n\nB111: T-They also seem quite happy.\n\nB111: Dr. Broussard, signing off.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D70 RID: 7536
		public class A1_CLONEDRATS
		{
			// Token: 0x040084D0 RID: 34000
			public static LocString TITLE = "Initial Success";

			// Token: 0x040084D1 RID: 34001
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x020024F2 RID: 9458
			public class BODY
			{
				// Token: 0x0400A208 RID: 41480
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B111]</smallcaps>\n\n[LOG BEGINS]\n\n[A throat clears.]\n\nB111: We are now reliably printing healthy, living subjects, though all have exhibited unusual qualities as a result of the cloning process.\n\n[Squeaking sounds can be heard.]\n\nB111: Unusual vocalizations, benign growths, and missing appendages have been seen in all subjects thus far, to varying degrees of severity. It seems that bypassing or accelerating juvenility halts certain critical stages of development. Brain function, however, appears typical.\n\n[Squeaking.]\n\nB111: T-They also seem quite happy.\n\nB111: Dr. Broussard, signing off.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D71 RID: 7537
		public class A5_GENETICOOZE
		{
			// Token: 0x040084D2 RID: 34002
			public static LocString TITLE = "Biofluid";

			// Token: 0x040084D3 RID: 34003
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x020024F3 RID: 9459
			public class BODY
			{
				// Token: 0x0400A209 RID: 41481
				public static LocString CONTAINER1 = "[LOG BEGINS]\n\nThe Printing Pod is primed by a synthesized bio-organic concoction the technicians have taken to calling \"Ooze\", a specialized mixture composed of water, carbon, and dozens upon dozens of the trace elements necessary for the creation of life.\n\nThe pod reconstitutes these elements into a living organism using the blueprints we feed it, before finally administering a shock of life.\n\nIt is like any other 3D printer. We just use different ink.\n\nDr. Broussard, signing off.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D72 RID: 7538
		public class A4_HIBISCUS3
		{
			// Token: 0x040084D4 RID: 34004
			public static LocString TITLE = "Experiment 7D";

			// Token: 0x040084D5 RID: 34005
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x020024F4 RID: 9460
			public class BODY
			{
				// Token: 0x0400A20A RID: 41482
				public static LocString CONTAINER1 = "EXPERIMENT 7D\nSecurity Code: B111\n\nSubject: #762, \"Hibiscus-3\"\nAdult female, 42cm, 257g\n\nDonor: #650, \"Hibiscus\"\nAdult female, 42cm, 257g";

				// Token: 0x0400A20B RID: 41483
				public static LocString CONTAINER2 = "Hypothesis: Subjects cloned from Hibiscus will correctly operate a lever apparatus when introduced, demonstrating retention of original donor's conditioned memories.\n\nDonor subject #650, \"Hibiscus\", conditioned to pull a lever to the right for a reward (almonds). Conditioning took place over a period of two weeks.\n\nHibiscus quickly learned that pulling the lever to the left produced no results, and was reliably demonstrating the desired behavior by the end of the first week.\n\nTraining continued for one additional week to strengthen neural pathways and ensure the intended behavioral conditioning was committed to long term and muscle memory.\n\nCloning subject #762, \"Hibiscus-3\", was introduced to the lever apparatus to ascertain memory retention and recall.\n\nHibiscus-3 showed no signs of recognition and did not perform the desired behavior. Subject initially failed to interact with the apparatus on any level.\n\nOn second introduction, Hibiscus-3 pulled the lever to the left.\n\nConclusion: Printed subject retains no memory from donor.";
			}
		}

		// Token: 0x02001D73 RID: 7539
		public class A3_HUSBANDRYNOTES
		{
			// Token: 0x040084D6 RID: 34006
			public static LocString TITLE = "Husbandry Notes";

			// Token: 0x040084D7 RID: 34007
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024F5 RID: 9461
			public class BODY
			{
				// Token: 0x0400A20C RID: 41484
				public static LocString CONTAINER1 = "<smallcaps>[Log Fragmentation Detected]\n[Voice Recognition Unavailable]</smallcaps>\n\n[LOG BEGINS]\n\n...The Hatch has been selected for development due to its naturally wide range of potential food sources.\n\nEnergy production is our primary goal, but augmentation to allow for the consumption of non-organic materials is a more attainable first step, and will have additional uses for waste disposal...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400A20D RID: 41485
				public static LocString CONTAINER2 = "[LOG BEGINS]\n\n...The Morb has been selected for development based on its ability to perform a multitude of the waste breakdown functions typical for a healthy ecosystem.\n\nDesign should focus on eliminating the disease risks posed by a fully matured Morb specimen...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400A20E RID: 41486
				public static LocString CONTAINER3 = "[LOG BEGINS]\n\n...The Puft may be suited for serving a sustainable decontamination role.\n\nPotential design must focus on the efficiency of these processes...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400A20F RID: 41487
				public static LocString CONTAINER4 = "[LOG BEGINS]\n\n...Wheezeworts are an ideal selection due to their low nutrient requirements and natural terraforming capabilities.\n\nDesign of these creatures should focus on enhancing their natural influence on ambient temperatures...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400A210 RID: 41488
				public static LocString CONTAINER5 = "[LOG BEGINS]\n\n...The preliminary Hatch gene splices were successful.\n\nThe prolific mucus excretions that are typical of the species are now producing hydrocarbons at an incredible pace.\n\nThe creature has essentially become a free source of burnable oil...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400A211 RID: 41489
				public static LocString CONTAINER6 = "[LOG BEGINS]\n\n...Bioluminescence is always a novelty, but little time should be spent on perfecting these insects from here on out.\n\nThe project has more pressing concerns than light sources, particularly now that the low light vegetation issue has been solved...\n\n[LOG ENDS]\n------------------\n";

				// Token: 0x0400A212 RID: 41490
				public static LocString CONTAINER7 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: B363]</smallcaps>\n\n[LOG BEGINS]\n\nGeneticist: The primary concern raised by this project is the variability of environments that colonies may be forced to deal with. The creatures we send with the settlement party will not have the time to evolve and adapt to a new environment, yet each creature has been chosen to play a vital role in colony sustainability and is thus too precious to risk loss.\n\nGeneticist: It follows that each organism we design must be equipped with the tools to survive in as many volatile environments as we are capable of planning for. We should not rely on the Pod alone to replenish creature populations.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D74 RID: 7540
		public class A6_MEMORYIMPLANTATION
		{
			// Token: 0x040084D8 RID: 34008
			public static LocString TITLE = "Memory Dysfunction Log";

			// Token: 0x040084D9 RID: 34009
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: TWO";

			// Token: 0x020024F6 RID: 9462
			public class BODY
			{
				// Token: 0x0400A213 RID: 41491
				public static LocString CONTAINER1 = "[LOG BEGINS]\n\nTraditionally, cloning produces a subject that is genetically identical to the donor but develops independently, producing a being that is, in its own way, unique.\n\nThe pod, conversely, attempts to print an exact atomic copy. Theoretically all neural pathways should be intact and identical to the original subject.\n\nIt's fascinating, given this, that memories are not already inherent in our subjects; however, no cloned subjects as of yet have shown any signs of recognition when introduced to familiar stimuli, such as the donor subject's enclosure.\n\nRefer to Experiment 7D.\n\nRefer to Experiment 7F.";

				// Token: 0x0400A214 RID: 41492
				public static LocString CONTAINER2 = "\nMemories <i>must</i> be embedded within the physical brainmaps of our subjects. The only question that remains is how to activate them. Hormones? Chemical supplements? Situational triggers?\n\nThe Director seems eager to move past this problem, and I am concerned at her willingness to bypass essential stages of the research development process.\n\nWe cannot move on to the fine polish of printing systems until the core processes have been perfected - which they have not.\n\nDr. Broussard, signing off.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D75 RID: 7541
		public class B9_TELEPORTATION
		{
			// Token: 0x040084DA RID: 34010
			public static LocString TITLE = "Memory Breakthrough";

			// Token: 0x040084DB RID: 34011
			public static LocString SUBTITLE = "ENCRYPTION LEVEL: THREE";

			// Token: 0x020024F7 RID: 9463
			public class BODY
			{
				// Token: 0x0400A215 RID: 41493
				public static LocString CONTAINER1 = "<smallcaps>[Voice Recognition Initialized]\n[Subject Identified: A001]</smallcaps>\n\n[LOG BEGINS]\n\nDr. Techna's newest notes on Duplicant memories have revealed some interesting discoveries. It seems memories </i>can</i> be transferred to the cloned subject but it requires the host to be subjected to a machine that performs extremely detailed microanalysis. This in-depth dissection of the subject would produce the results we need but at the expense of destroying the host.\n\nOf course this is not ideal for our current situation. The time and energy it took to recruit Gravitas' highly trained staff would be wasted if we were to extirpate these people for the sake of experimentation. But perhaps we can use our Duplicants as experimental subjects until we perfect the process and look into finding volunteers for the future in order to obtain an ideal specimen. I will have to discuss this with Dr. Techna but I'm sure he would be enthusiastic about such an opportunity to continue his work.\n\nI am also very interested in the commercial opportunities this presents. Off the top of my head I can think of applications in genetics, AI development, and teleportation technology. This could be a significant financial windfall for the company.\n\n[LOG ENDS]";
			}
		}

		// Token: 0x02001D76 RID: 7542
		public class AUTOMATION
		{
			// Token: 0x040084DC RID: 34012
			public static LocString TITLE = UI.FormatAsLink("Automation", "LOGIC");

			// Token: 0x040084DD RID: 34013
			public static LocString HEADER_1 = "Automation";

			// Token: 0x040084DE RID: 34014
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"Automation is a tool for controlling the operation of buildings based on what sensors in the colony are detecting.\n\nA ",
				BUILDINGS.PREFABS.CEILINGLIGHT.NAME,
				" could be configured to automatically turn on when a ",
				BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.NAME,
				" detects a Duplicant in the room.\n\nA ",
				BUILDINGS.PREFABS.LIQUIDPUMP.NAME,
				" might activate only when a ",
				BUILDINGS.PREFABS.LOGICELEMENTSENSORLIQUID.NAME,
				" detects water.\n\nA ",
				BUILDINGS.PREFABS.AIRCONDITIONER.NAME,
				" might activate only when the ",
				BUILDINGS.PREFABS.LOGICTEMPERATURESENSOR.NAME,
				" detects too much heat.\n\n"
			});

			// Token: 0x040084DF RID: 34015
			public static LocString HEADER_2 = "Automation Wires";

			// Token: 0x040084E0 RID: 34016
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"In addition to an ",
				UI.FormatAsLink("electrical wire", "WIRE"),
				", most powered buildings can also have an ",
				BUILDINGS.PREFABS.LOGICWIRE.NAME,
				" connected to them. This wire can signal the building to turn on or off. If the other end of a ",
				BUILDINGS.PREFABS.LOGICWIRE.NAME,
				" is connected to a sensor, the building will turn on and off as the sensor outputs signals.\n\n"
			});

			// Token: 0x040084E1 RID: 34017
			public static LocString HEADER_3 = "Signals";

			// Token: 0x040084E2 RID: 34018
			public static LocString PARAGRAPH_3 = string.Concat(new string[]
			{
				"There are two signals that an ",
				BUILDINGS.PREFABS.LOGICWIRE.NAME,
				" can send: Green and Red. The green signal will usually cause buildings to turn on, and the red signal will usually cause buildings to turn off. Sensors can often be configured to send their green signal only under certain conditions. A ",
				BUILDINGS.PREFABS.LOGICTEMPERATURESENSOR.NAME,
				" could be configured to only send a green signal if detecting temperatures greater than a chosen value.\n\n"
			});

			// Token: 0x040084E3 RID: 34019
			public static LocString HEADER_4 = "Gates";

			// Token: 0x040084E4 RID: 34020
			public static LocString PARAGRAPH_4 = "The signals of sensor wires can be combined using special buildings called \"Gates\" in order to create complex activation conditions.\nThe " + BUILDINGS.PREFABS.LOGICGATEAND.NAME + " can have two automation wires connected to its input slots, and one connected to its output slots. It will send a \"Green\" signal to its output slot only if it is receiving a \"Green\" signal from both its input slots. This could be used to activate a building only when multiple sensors are detecting something.\n\n";
		}

		// Token: 0x02001D77 RID: 7543
		public class DECORSYSTEM
		{
			// Token: 0x040084E5 RID: 34021
			public static LocString TITLE = UI.FormatAsLink("Decor", "DECOR");

			// Token: 0x040084E6 RID: 34022
			public static LocString HEADER_1 = "Decor";

			// Token: 0x040084E7 RID: 34023
			public static LocString PARAGRAPH_1 = "Low Decor can increase Duplicant " + UI.FormatAsLink("Stress", "STRESS") + ". Thankfully, pretty things tend to increase the Decor value of an area. Each Duplicant has a different idea of what is a high enough Decor value. If the average Decor that a Duplicant experiences in a cycle is below their expectations, they will suffer a stress penalty.\n\n";

			// Token: 0x040084E8 RID: 34024
			public static LocString HEADER_2 = "Calculating Decor";

			// Token: 0x040084E9 RID: 34025
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"Many things have an effect on the Decor value of a tile. A building's effect is expressed as a strength value and a radius. Often that effect is positive, but many buildings also lower the decor value of an area too. ",
				UI.FormatAsLink("Plants", "PLANTS"),
				", ",
				UI.FormatAsLink("Critters", "CREATURES"),
				", and ",
				UI.FormatAsLink("Furniture", "BUILDCATEGORYFURNITURE"),
				" often increase decor while industrial buildings and rot often decrease it. Duplicants experience the combined decor of all objects affecting a tile.\n\nThe ",
				CREATURES.SPECIES.PRICKLEGRASS.NAME,
				" has a decor value of ",
				string.Format("{0} and a radius of {1} tiles. ", PrickleGrassConfig.POSITIVE_DECOR_EFFECT.amount, PrickleGrassConfig.POSITIVE_DECOR_EFFECT.radius),
				"\nThe ",
				BUILDINGS.PREFABS.MICROBEMUSHER.NAME,
				" has a decor value of ",
				string.Format("{0} and a radius of {1} tiles. ", MicrobeMusherConfig.DECOR.amount, MicrobeMusherConfig.DECOR.radius),
				"\nThe result of placing a ",
				BUILDINGS.PREFABS.MICROBEMUSHER.NAME,
				" next to a ",
				CREATURES.SPECIES.PRICKLEGRASS.NAME,
				" would be a combined decor value of ",
				(MicrobeMusherConfig.DECOR.amount + PrickleGrassConfig.POSITIVE_DECOR_EFFECT.amount).ToString()
			});
		}

		// Token: 0x02001D78 RID: 7544
		public class EXOBASES
		{
			// Token: 0x040084EA RID: 34026
			public static LocString TITLE = UI.FormatAsLink("Space Travel", "EXOBASES");

			// Token: 0x040084EB RID: 34027
			public static LocString HEADER_1 = "Building Rockets";

			// Token: 0x040084EC RID: 34028
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"Building a rocket first requires constructing a ",
				UI.FormatAsLink("Rocket Platform", "LAUNCHPAD"),
				" and adding modules from the menu. All rockets will require an engine, a nosecone and a Command Module piloted by a Duplicant possessing the ",
				UI.FormatAsLink("Rocket Piloting", "ROCKETPILOTING1"),
				" skill or higher. Note that the ",
				UI.FormatAsLink("Solo Spacefarer Nosecone", "HABITATMODULESMALL"),
				" functions as both a Command Module and a nosecone.\n\n"
			});

			// Token: 0x040084ED RID: 34029
			public static LocString HEADER_2 = "Space Travel";

			// Token: 0x040084EE RID: 34030
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"To scan space and see nearby intersteller destinations a ",
				UI.FormatAsLink("Telescope", "CLUSTERTELESCOPE"),
				" must first be built on the surface of a Planetoid. ",
				UI.FormatAsLink("Orbital Data Collection Lab", "ORBITALRESEARCHCENTER"),
				" in orbit around a Planetoid, and ",
				UI.FormatAsLink("Cartographic Module", "SCANNERMODULE"),
				" attached to a rocket can also reveal places on a Starmap.\n\nAlways check engine fuel to determine if your rocket can reach its destination, keeping in mind rockets can only land on Plantoids with a ",
				UI.FormatAsLink("Rocket Platform", "LAUNCHPAD"),
				" on it although some modules like ",
				UI.FormatAsLink("Rover's Modules", "SCOUTMODULE"),
				" and ",
				UI.FormatAsLink("Trailblazer Modules", "PIONEERMODULE"),
				" can be sent to the surface of a Planetoid from a rocket in orbit.\n\n"
			});

			// Token: 0x040084EF RID: 34031
			public static LocString HEADER_3 = "Space Transport";

			// Token: 0x040084F0 RID: 34032
			public static LocString PARAGRAPH_3 = string.Concat(new string[]
			{
				"Goods can be teleported between worlds with connected Supply Teleporters through ",
				UI.FormatAsLink("Gas", "ELEMENTS_GAS"),
				", ",
				UI.FormatAsLink("Liquid", "ELEMENTS_LIQUID"),
				", and ",
				UI.FormatAsLink("Solid", "ELEMENTS_SOLID"),
				" conduits.\n\nPlanetoids not connected through Supply Teleporters can use rockets to transport goods, either by landing on a ",
				UI.FormatAsLink("Rocket Platform", "LAUNCHPAD"),
				" or a ",
				UI.FormatAsLink("Orbital Cargo Module", "ORBITALCARGOMODULE"),
				" deployed from a rocket in orbit. Additionally, the ",
				UI.FormatAsLink("Interplanetary Launcher", "RAILGUN"),
				" can send ",
				UI.FormatAsLink("Interplanetary Payloads", "RAILGUNPAYLOAD"),
				" full of goods through space but must be opened by a ",
				UI.FormatAsLink("Payload Opener", "RAILGUNPAYLOADOPENER"),
				". A ",
				UI.FormatAsLink("Targeting Beacon", "LANDINGBEACON"),
				" can guide payloads and orbital modules to land at a specific location on a Planetoid surface."
			});
		}

		// Token: 0x02001D79 RID: 7545
		public class GENETICS
		{
			// Token: 0x040084F1 RID: 34033
			public static LocString TITLE = UI.FormatAsLink("Genetics", "GENETICS");

			// Token: 0x040084F2 RID: 34034
			public static LocString HEADER_1 = "Plant Mutations";

			// Token: 0x040084F3 RID: 34035
			public static LocString PARAGRAPH_1 = "Plants exposed to radiation sometimes drop mutated seeds when they are harvested. Each type of mutation has its own efficiencies and trade-offs.\n\nMutated seeds can be planted once they have been analyzed in the " + UI.FormatAsLink("Botanical Analyzer", "GENETICANALYSISSTATION") + ", but the resulting plants will produce no seeds of their own unless they are uprooted.\n\n";

			// Token: 0x040084F4 RID: 34036
			public static LocString HEADER_2 = "Cultivating Mutated Seeds";

			// Token: 0x040084F5 RID: 34037
			public static LocString PARAGRAPH_2 = "Once mutated seeds have been analyzed in the Botanical Analyzer, they are ready to be planted. Continued exposure to naturally occurring radiation or a " + UI.FormatAsLink("Radiation Lamp", "RADIATIONLIGHT") + " is necessary to prevent wilting.\n\n";
		}

		// Token: 0x02001D7A RID: 7546
		public class HEALTH
		{
			// Token: 0x040084F6 RID: 34038
			public static LocString TITLE = UI.FormatAsLink("Health", "HEALTH");

			// Token: 0x040084F7 RID: 34039
			public static LocString HEADER_1 = "Health";

			// Token: 0x040084F8 RID: 34040
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"Duplicants can be physically damaged by some rare circumstances, such as extreme ",
				UI.FormatAsLink("Heat", "HEAT"),
				" or aggressive ",
				UI.FormatAsLink("Critters", "CREATURES"),
				". Damaged Duplicants will suffer greatly reduced athletic abilities, and are at risk of incapacitation if damaged too severely.\n\n"
			});

			// Token: 0x040084F9 RID: 34041
			public static LocString HEADER_2 = "Incapacitation and Death";

			// Token: 0x040084FA RID: 34042
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"Incapacitated Duplicants cannot move or perform errands. They must be rescued by another Duplicant before their health drops to zero. If a Duplicant's health reaches zero they will die.\n\nHealth can be restored slowly over time and quickly through rest at the ",
				BUILDINGS.PREFABS.MEDICALCOT.NAME,
				".\n\n Duplicants are generally more vulnerable to ",
				UI.FormatAsLink("Disease", "DISEASE"),
				" than physical damage.\n\n"
			});
		}

		// Token: 0x02001D7B RID: 7547
		public class HEAT
		{
			// Token: 0x040084FB RID: 34043
			public static LocString TITLE = UI.FormatAsLink("Heat", "HEAT");

			// Token: 0x040084FC RID: 34044
			public static LocString HEADER_1 = "Temperature";

			// Token: 0x040084FD RID: 34045
			public static LocString PARAGRAPH_1 = "Just about everything on the asteroid has a temperature. It's normal for temperature to rise and fall a bit, but extreme temperatures can cause all sorts of problems for a base. Buildings can stop functioning, crops can wilt, and things can even melt, boil, and freeze when they really ought not to.\n\n";

			// Token: 0x040084FE RID: 34046
			public static LocString HEADER_2 = "Wilting, Overheating, and Melting";

			// Token: 0x040084FF RID: 34047
			public static LocString PARAGRAPH_2 = "Most crops require their body temperatures to be within a certain range in order to grow. Values outside of this range are not fatal, but will pause growth. If a building's temperature exceeds its overheat temperature it will take damage and require repair.\nAt very extreme temperatures buildings may melt or boil away.\n\n";

			// Token: 0x04008500 RID: 34048
			public static LocString HEADER_3 = "Thermal Energy";

			// Token: 0x04008501 RID: 34049
			public static LocString PARAGRAPH_3 = "Temperature increase when the thermal energy of a substance increases. The value of temperature is equal to the total Thermal Energy divided by the Specific Heat Capacity of the substance. Because Specific Heat Capacity varies between substances so significantly, it is often the case a substance can have a higher temperature than another despite a lower overall thermal energy. This quality makes Water require nearly four times the amount of thermal energy to increase in temperature compared to Oxygen.\n\n";

			// Token: 0x04008502 RID: 34050
			public static LocString HEADER_4 = "Conduction and Insulation";

			// Token: 0x04008503 RID: 34051
			public static LocString PARAGRAPH_4 = "Thermal energy can be transferred between Buildings, Creatures, World tiles, and other world entities through Conduction. Conduction occurs when two things of different Temperatures are touching. The rate of the energy transfer is the product of the averaged Conductivity values and Temperature difference. Thermal energy will flow slowly between substances with low conductivity values (insulators), and quickly between substances with high conductivity (conductors).\n\n";

			// Token: 0x04008504 RID: 34052
			public static LocString HEADER_5 = "State Changes";

			// Token: 0x04008505 RID: 34053
			public static LocString PARAGRAPH_5 = "Water ice melts into liquid water when its temperature rises above its melting point. Liquid water boils into steam when its temperature rises above its boiling point. Similar transitions in state occur for most elements, but each element has its own threshold temperatures. Sometimes the transitions are not reversible - crude oil boiled into sour gas will not condense back to crude oil when cooled. Instead, the substance might condense into a totally different element with a different utility. \n\n";
		}

		// Token: 0x02001D7C RID: 7548
		public class LIGHT
		{
			// Token: 0x04008506 RID: 34054
			public static LocString TITLE = UI.FormatAsLink("Light", "LIGHT");

			// Token: 0x04008507 RID: 34055
			public static LocString HEADER_1 = "Light";

			// Token: 0x04008508 RID: 34056
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"Most of the asteroid is dark. Light sources such as the ",
				BUILDINGS.PREFABS.CEILINGLIGHT.NAME,
				" or ",
				CREATURES.SPECIES.LIGHTBUG.NAME,
				" improves Decor and gives Duplicants a boost to their productivity. Many plants are also sensitive to the amount of light they receive.\n\n"
			});

			// Token: 0x04008509 RID: 34057
			public static LocString HEADER_2 = "Light Sources";

			// Token: 0x0400850A RID: 34058
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"The ",
				BUILDINGS.PREFABS.FLOORLAMP.NAME,
				" and ",
				BUILDINGS.PREFABS.CEILINGLIGHT.NAME,
				" produce a decent amount of light when powered. The ",
				CREATURES.SPECIES.LIGHTBUG.NAME,
				" naturally emits a halo of light. Strong solar light is available on the surface during daytime.\n\n"
			});

			// Token: 0x0400850B RID: 34059
			public static LocString HEADER_3 = "Measuring Light";

			// Token: 0x0400850C RID: 34060
			public static LocString PARAGRAPH_3 = string.Concat(new string[]
			{
				"The amount of light on a cell is measured in Lux. Lux has a dramatic range - A simple ",
				BUILDINGS.PREFABS.CEILINGLIGHT.NAME,
				" produces ",
				1800.ToString(),
				" Lux, while the sun can produce values as high as ",
				80000.ToString(),
				" Lux. The ",
				BUILDINGS.PREFABS.SOLARPANEL.NAME,
				" generates power proportional to how many Lux it is exposed to.\n\n"
			});
		}

		// Token: 0x02001D7D RID: 7549
		public class MORALE
		{
			// Token: 0x0400850D RID: 34061
			public static LocString TITLE = UI.FormatAsLink("Morale", "MORALE");

			// Token: 0x0400850E RID: 34062
			public static LocString HEADER_1 = "Morale";

			// Token: 0x0400850F RID: 34063
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"Morale describes the relationship between a Duplicant's ",
				UI.FormatAsLink("Skills", "ROLES"),
				" and their Lifestyle. The more skills a Duplicant has, the higher their morale expectation will be. Duplicants with morale below their expectation will experience a ",
				UI.FormatAsLink("Stress", "STRESS"),
				" penalty. Comforts such as quality ",
				UI.FormatAsLink("Food", "FOOD"),
				", nice rooms, and recreation will increase morale.\n\n"
			});

			// Token: 0x04008510 RID: 34064
			public static LocString HEADER_2 = "Recreation";

			// Token: 0x04008511 RID: 34065
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"Recreation buildings such as the ",
				BUILDINGS.PREFABS.WATERCOOLER.NAME,
				" and ",
				BUILDINGS.PREFABS.ESPRESSOMACHINE.NAME,
				" improve a Duplicant's morale when used. Duplicants need downtime time in their schedules to use these buildings.\n\n"
			});

			// Token: 0x04008512 RID: 34066
			public static LocString HEADER_3 = "Overjoyed Responses";

			// Token: 0x04008513 RID: 34067
			public static LocString PARAGRAPH_3 = "If a Duplicant has a very high Morale value, they will spontaneously display an Overjoyed Response. Each Duplicant has a different Overjoyed Behavior - but all overjoyed responses are good. Some will positively affect Building " + UI.FormatAsLink("Decor", "DECOR") + ", others will positively affect Duplicant morale or productivity.\n\n";
		}

		// Token: 0x02001D7E RID: 7550
		public class POWER
		{
			// Token: 0x04008514 RID: 34068
			public static LocString TITLE = UI.FormatAsLink("Power", "POWER");

			// Token: 0x04008515 RID: 34069
			public static LocString HEADER_1 = "Electricity";

			// Token: 0x04008516 RID: 34070
			public static LocString PARAGRAPH_1 = "Electrical power is required to run many of the buildings in a base. Different buildings requires different amounts of power to run. Power can be transferred to buildings that require it using " + UI.FormatAsLink("Wires", "WIRE") + ".\n\n";

			// Token: 0x04008517 RID: 34071
			public static LocString HEADER_2 = "Generators and Batteries";

			// Token: 0x04008518 RID: 34072
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"Several buildings can generate power. Duplicants can run on the ",
				BUILDINGS.PREFABS.MANUALGENERATOR.NAME,
				" to generate clean power. Once generated, power can be consumed by buildings or stored in a ",
				BUILDINGS.PREFABS.BATTERY.NAME,
				" to prevent waste. Any generated power that is not consumed or stored will be wasted. Batteries and Generators tend to produce a significant amount of ",
				UI.FormatAsLink("Heat", "HEAT"),
				" while active.\n\n"
			});

			// Token: 0x04008519 RID: 34073
			public static LocString HEADER_3 = "Measuring Power";

			// Token: 0x0400851A RID: 34074
			public static LocString PARAGRAPH_3 = "Power is measure in Joules when stored in a " + BUILDINGS.PREFABS.BATTERY.NAME + ". Power produced and consumed by buildings is measured in Watts, which are equal to Joules (consumed or produced) per second.\n\nA Battery that stored 5000 Joules could power a building that consumed 240 Watts for about 20 seconds. A generator which produces 480 Watts could power two buildings which consume 240 Watts for as long as it was running.\n\n";

			// Token: 0x0400851B RID: 34075
			public static LocString HEADER_4 = "Overloading";

			// Token: 0x0400851C RID: 34076
			public static LocString PARAGRAPH_4 = string.Concat(new string[]
			{
				"A network of ",
				UI.FormatAsLink("Wires", "WIRE"),
				" can be overloaded if it is consuming too many watts. If the wattage of a wire network exceeds its limits it may break and require repair.\n\n",
				UI.FormatAsLink("Standard wires", "WIRE"),
				" have a ",
				1000.ToString(),
				" Watt limit.\n\n"
			});
		}

		// Token: 0x02001D7F RID: 7551
		public class PRIORITY
		{
			// Token: 0x0400851D RID: 34077
			public static LocString TITLE = UI.FormatAsLink("Priorities", "PRIORITY");

			// Token: 0x0400851E RID: 34078
			public static LocString HEADER_1 = "Errand Priority";

			// Token: 0x0400851F RID: 34079
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"Duplicants prioritize their errands based on several factors. Some of these can be adjusted to affect errand choice, but some errands (such as seeking breathable ",
				UI.FormatAsLink("Oxygen", "OXYGEN"),
				") are so important that they cannot be delayed. Errand priority can primarily be controlled by Errand Type prioritization, and then can be further fine-tuned by the ",
				UI.FormatAsTool("Priority Tool", global::Action.Prioritize),
				".\n\n"
			});

			// Token: 0x04008520 RID: 34080
			public static LocString HEADER_2 = "Errand Type Prioritization";

			// Token: 0x04008521 RID: 34081
			public static LocString PARAGRAPH_2 = "Each errand a Duplicant can perform falls into an Errand Category. These categories can be prioritized on a per-Duplicant basis in the " + UI.FormatAsManagementMenu("Priorities Screen") + ". Entire errand categories can also be prohibited to a Duplicant if they are meant to never perform errands of that variety. A common configuration is to assign errand type priority based on Duplicant attributes.\n\nFor example, Duplicants who are good at Research could be made to prioritize the Researching errand type. Duplicants with poor Athletics could be made to deprioritize the Supplying and Storing errand types.\n\n";

			// Token: 0x04008522 RID: 34082
			public static LocString HEADER_3 = "Priority Tool";

			// Token: 0x04008523 RID: 34083
			public static LocString PARAGRAPH_3 = "The priority of errands can often be modified using the " + UI.FormatAsTool("Priority tool", global::Action.Prioritize) + ". The values applied by this tool are always less influential than the Errand Type priorities described above. If two errands with equal Errand Type Priority are available to a Duplicant, they will choose the errand with a higher priority setting as applied by the tool.\n\n";
		}

		// Token: 0x02001D80 RID: 7552
		public class RADIATION
		{
			// Token: 0x04008524 RID: 34084
			public static LocString TITLE = UI.FormatAsLink("Radiation", "RADIATION");

			// Token: 0x04008525 RID: 34085
			public static LocString HEADER_1 = "Radiation";

			// Token: 0x04008526 RID: 34086
			public static LocString PARAGRAPH_1 = string.Concat(new string[]
			{
				"When transporting radioactive materials such as ",
				UI.FormatAsLink("Uranium Ore", "URANIUMORE"),
				", care must be taken to avoid exposing outside objects to ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATIONSICKNESS"),
				".\n\nUsing proper transportation vessels, such as those which are lined with ",
				UI.FormatAsLink("Lead", "LEAD"),
				", is crucial to ensuring that Duplicants avoid ",
				UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
				"."
			});

			// Token: 0x04008527 RID: 34087
			public static LocString HEADER_2 = "Radiation Sickness";

			// Token: 0x04008528 RID: 34088
			public static LocString PARAGRAPH_2 = string.Concat(new string[]
			{
				"Duplicants who are exposed to ",
				UI.FormatAsLink("Radioactive Contaminants", "RADIATIONSICKNESS"),
				" will need to wear protection or they risk coming down with ",
				UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
				".\n\nSome Duplicants will have more of a natural resistance to radiation, but prolonged exposure will still increase their chances of becoming sick.\n\nConsuming ",
				UI.FormatAsLink("Rad Pills", "BASICRADPILL"),
				" or seafood such as ",
				UI.FormatAsLink("Cooked Seafood", "COOKEDFISH"),
				" or ",
				UI.FormatAsLink("Waterweed", "SEALETTUCE"),
				" increases a Duplicant's radiation resistance, but will not cure a Duplicant's ",
				UI.FormatAsLink("Radiation Sickness", "RADIATIONSICKNESS"),
				" once they have become infected.\n\nOn the other hand, exposure to radiation will kill ",
				UI.FormatAsLink("Food Poisoning", "FOODPOISONING"),
				", ",
				UI.FormatAsLink("Slimelung", "SLIMELUNG"),
				" and ",
				UI.FormatAsLink("Zombie Spores", "ZOMBIESPORES"),
				" on surfaces (including on Duplicants).\n\n"
			});

			// Token: 0x04008529 RID: 34089
			public static LocString HEADER_3 = "Nuclear Energy";

			// Token: 0x0400852A RID: 34090
			public static LocString PARAGRAPH_3 = string.Concat(new string[]
			{
				"A ",
				UI.FormatAsLink("Research Reactor", "NUCLEARREACTOR"),
				" will require ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				" to run. Uranium can be enriched using a ",
				UI.FormatAsLink("Uranium Centrifuge", "URANIUMCENTRIFUGE"),
				".\n\nOnce supplied with ",
				UI.FormatAsLink("Enriched Uranium", "ENRICHEDURANIUM"),
				", a ",
				UI.FormatAsLink("Research Reactors", "NUCLEARREACTOR"),
				" will create an enormous amount of ",
				UI.FormatAsLink("Heat", "HEAT"),
				" which can then be placed under a source of ",
				UI.FormatAsLink("Water", "WATER"),
				" to produce ",
				UI.FormatAsLink("Steam", "STEAM"),
				"and connected to a ",
				UI.FormatAsLink("Steam Turbine", "STEAMTURBINE2"),
				" to produce a considerable source of ",
				UI.FormatAsLink("Power", "POWER"),
				"."
			});
		}

		// Token: 0x02001D81 RID: 7553
		public class RESEARCH
		{
			// Token: 0x0400852B RID: 34091
			public static LocString TITLE = UI.FormatAsLink("Research", "RESEARCH");

			// Token: 0x0400852C RID: 34092
			public static LocString HEADER_1 = "Research";

			// Token: 0x0400852D RID: 34093
			public static LocString PARAGRAPH_1 = "Doing research unlocks new types of buildings for the colony. Duplicants can perform research at the " + BUILDINGS.PREFABS.RESEARCHCENTER.NAME + ".\n\n";

			// Token: 0x0400852E RID: 34094
			public static LocString HEADER_2 = "Research Tasks";

			// Token: 0x0400852F RID: 34095
			public static LocString PARAGRAPH_2 = "A selected research task is completed once enough research points have been generated at the colony's research stations. Duplicants with high 'Science' attribute scores will generate research points faster than Duplicants with lower scores.\n\n";

			// Token: 0x04008530 RID: 34096
			public static LocString HEADER_3 = "Research Types";

			// Token: 0x04008531 RID: 34097
			public static LocString PARAGRAPH_3 = string.Concat(new string[]
			{
				"Advanced research tasks require special research stations to generate the proper kind of research points. These research stations often consume more advanced resources.\n\nUsing higher-level research stations also requires Duplicants to have learned higher level research ",
				UI.FormatAsLink("skills", "ROLES"),
				".\n\n",
				STRINGS.RESEARCH.TYPES.ALPHA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.RESEARCHCENTER.NAME,
				"\n",
				STRINGS.RESEARCH.TYPES.BETA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME,
				"\n",
				STRINGS.RESEARCH.TYPES.GAMMA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.COSMICRESEARCHCENTER.NAME,
				"\n\n"
			});
		}

		// Token: 0x02001D82 RID: 7554
		public class RESEARCHDLC1
		{
			// Token: 0x04008532 RID: 34098
			public static LocString TITLE = UI.FormatAsLink("Research", "RESEARCHDLC1");

			// Token: 0x04008533 RID: 34099
			public static LocString HEADER_1 = "Research";

			// Token: 0x04008534 RID: 34100
			public static LocString PARAGRAPH_1 = "Doing research unlocks new types of buildings for the colony. Duplicants can perform research at the " + BUILDINGS.PREFABS.RESEARCHCENTER.NAME + ".\n\n";

			// Token: 0x04008535 RID: 34101
			public static LocString HEADER_2 = "Research Tasks";

			// Token: 0x04008536 RID: 34102
			public static LocString PARAGRAPH_2 = "A selected research task is completed once enough research points have been generated at the colonies research stations. Duplicants with high 'Science' attribute scores will generate research points faster than Duplicants with lower scores.\n\n";

			// Token: 0x04008537 RID: 34103
			public static LocString HEADER_3 = "Research Types";

			// Token: 0x04008538 RID: 34104
			public static LocString PARAGRAPH_3 = string.Concat(new string[]
			{
				"Advanced research tasks require special research stations to generate the proper kind of research points. These research stations often consume more advanced resources.\n\nUsing higher level research stations also requires Duplicants to have learned higher level research ",
				UI.FormatAsLink("skills", "ROLES"),
				".\n\n",
				STRINGS.RESEARCH.TYPES.ALPHA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.RESEARCHCENTER.NAME,
				"\n",
				STRINGS.RESEARCH.TYPES.BETA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.ADVANCEDRESEARCHCENTER.NAME,
				"\n",
				STRINGS.RESEARCH.TYPES.GAMMA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.ORBITALRESEARCHCENTER.NAME,
				"\n",
				STRINGS.RESEARCH.TYPES.DELTA.NAME,
				" is performed at the ",
				BUILDINGS.PREFABS.NUCLEARRESEARCHCENTER.NAME,
				"\n\n"
			});
		}

		// Token: 0x02001D83 RID: 7555
		public class STRESS
		{
			// Token: 0x04008539 RID: 34105
			public static LocString TITLE = UI.FormatAsLink("Stress", "STRESS");

			// Token: 0x0400853A RID: 34106
			public static LocString HEADER_1 = "Stress";

			// Token: 0x0400853B RID: 34107
			public static LocString PARAGRAPH_1 = "A Duplicant's experiences in the colony affect their stress level. Stress increases when they have negative experiences or unmet expectations. Stress decreases with time if " + UI.FormatAsLink("Morale", "MORALE") + " is satisfied. Duplicant behavior starts to change for the worse when stress levels get too high.\n\n";

			// Token: 0x0400853C RID: 34108
			public static LocString HEADER_2 = "Stress Responses";

			// Token: 0x0400853D RID: 34109
			public static LocString PARAGRAPH_2 = "If a Duplicant has very high stress values they will experience a Stress Response episode. Each Duplicant has a different Stress Behavior - but all stress responses are bad. After the stress behavior episode is done, the Duplicants stress will reset to a lower value. Though, if the factors causing the Duplicant's high stress are not corrected they are bound to have another stress response episode.\n\n";
		}
	}
}
