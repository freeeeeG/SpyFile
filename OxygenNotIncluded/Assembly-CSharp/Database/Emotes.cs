using System;
using Klei.AI;

namespace Database
{
	// Token: 0x02000CFE RID: 3326
	public class Emotes : ResourceSet<Resource>
	{
		// Token: 0x06006992 RID: 27026 RVA: 0x0028C943 File Offset: 0x0028AB43
		public Emotes(ResourceSet parent) : base("Emotes", parent)
		{
			this.Minion = new Emotes.MinionEmotes(this);
			this.Critter = new Emotes.CritterEmotes(this);
		}

		// Token: 0x06006993 RID: 27027 RVA: 0x0028C96C File Offset: 0x0028AB6C
		public void ResetProblematicReferences()
		{
			for (int i = 0; i < this.Minion.resources.Count; i++)
			{
				Emote emote = this.Minion.resources[i];
				for (int j = 0; j < emote.StepCount; j++)
				{
					emote[j].UnregisterAllCallbacks();
				}
			}
			for (int k = 0; k < this.Critter.resources.Count; k++)
			{
				Emote emote2 = this.Critter.resources[k];
				for (int l = 0; l < emote2.StepCount; l++)
				{
					emote2[l].UnregisterAllCallbacks();
				}
			}
		}

		// Token: 0x04004B8B RID: 19339
		public Emotes.MinionEmotes Minion;

		// Token: 0x04004B8C RID: 19340
		public Emotes.CritterEmotes Critter;

		// Token: 0x02001C2A RID: 7210
		public class MinionEmotes : ResourceSet<Emote>
		{
			// Token: 0x06009C81 RID: 40065 RVA: 0x0034DCF6 File Offset: 0x0034BEF6
			public MinionEmotes(ResourceSet parent) : base("Minion", parent)
			{
				this.InitializeCelebrations();
				this.InitializePhysicalStatus();
				this.InitializeEmotionalStatus();
				this.InitializeGreetings();
			}

			// Token: 0x06009C82 RID: 40066 RVA: 0x0034DD1C File Offset: 0x0034BF1C
			public void InitializeCelebrations()
			{
				this.ClapCheer = new Emote(this, "ClapCheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "clapcheer_pre"
					},
					new EmoteStep
					{
						anim = "clapcheer_loop"
					},
					new EmoteStep
					{
						anim = "clapcheer_pst"
					}
				}, "anim_clapcheer_kanim");
				this.Cheer = new Emote(this, "Cheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "cheer_pre"
					},
					new EmoteStep
					{
						anim = "cheer_loop"
					},
					new EmoteStep
					{
						anim = "cheer_pst"
					}
				}, "anim_cheer_kanim");
				this.ProductiveCheer = new Emote(this, "Productive Cheer", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "productive"
					}
				}, "anim_productive_kanim");
				this.ResearchComplete = new Emote(this, "ResearchComplete", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_research_complete_kanim");
				this.ThumbsUp = new Emote(this, "ThumbsUp", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_thumbsup_kanim");
			}

			// Token: 0x06009C83 RID: 40067 RVA: 0x0034DE5C File Offset: 0x0034C05C
			private void InitializePhysicalStatus()
			{
				this.CloseCall_Fall = new Emote(this, "Near Fall", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_floor_missing_kanim");
				this.Cold = new Emote(this, "Cold", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "andim_idle_cold_kanim");
				this.Cough = new Emote(this, "Cough", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_slimelungcough_kanim");
				this.Cough_Small = new Emote(this, "Small Cough", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_small"
					}
				}, "anim_slimelungcough_kanim");
				this.FoodPoisoning = new Emote(this, "Food Poisoning", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_contaminated_food_kanim");
				this.Hot = new Emote(this, "Hot", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_hot_kanim");
				this.IritatedEyes = new Emote(this, "Irritated Eyes", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "irritated_eyes"
					}
				}, "anim_irritated_eyes_kanim");
				this.MorningStretch = new Emote(this, "Morning Stretch", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_morning_stretch_kanim");
				this.Radiation_Glare = new Emote(this, "Radiation Glare", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_radiation_glare"
					}
				}, "anim_react_radiation_kanim");
				this.Radiation_Itch = new Emote(this, "Radiation Itch", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_radiation_itch"
					}
				}, "anim_react_radiation_kanim");
				this.Sick = new Emote(this, "Sick", Emotes.MinionEmotes.DEFAULT_IDLE_STEPS, "anim_idle_sick_kanim");
				this.Sneeze = new Emote(this, "Sneeze", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "sneeze"
					},
					new EmoteStep
					{
						anim = "sneeze_pst"
					}
				}, "anim_sneeze_kanim");
				this.Sneeze_Short = new Emote(this, "Short Sneeze", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "sneeze_short"
					},
					new EmoteStep
					{
						anim = "sneeze_short_pst"
					}
				}, "anim_sneeze_kanim");
			}

			// Token: 0x06009C84 RID: 40068 RVA: 0x0034E090 File Offset: 0x0034C290
			private void InitializeEmotionalStatus()
			{
				this.Concern = new Emote(this, "Concern", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_concern_kanim");
				this.Cringe = new Emote(this, "Cringe", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "cringe_pre"
					},
					new EmoteStep
					{
						anim = "cringe_loop"
					},
					new EmoteStep
					{
						anim = "cringe_pst"
					}
				}, "anim_cringe_kanim");
				this.Disappointed = new Emote(this, "Disappointed", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_disappointed_kanim");
				this.Shock = new Emote(this, "Shock", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_shock_kanim");
				this.Sing = new Emote(this, "Sing", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_singer_kanim");
			}

			// Token: 0x06009C85 RID: 40069 RVA: 0x0034E170 File Offset: 0x0034C370
			private void InitializeGreetings()
			{
				this.FingerGuns = new Emote(this, "Finger Guns", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_fingerguns_kanim");
				this.Wave = new Emote(this, "Wave", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_wave_kanim");
				this.Wave_Shy = new Emote(this, "Shy Wave", Emotes.MinionEmotes.DEFAULT_STEPS, "anim_react_wave_shy_kanim");
			}

			// Token: 0x04007FB8 RID: 32696
			private static EmoteStep[] DEFAULT_STEPS = new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "react"
				}
			};

			// Token: 0x04007FB9 RID: 32697
			private static EmoteStep[] DEFAULT_IDLE_STEPS = new EmoteStep[]
			{
				new EmoteStep
				{
					anim = "idle_pre"
				},
				new EmoteStep
				{
					anim = "idle_default"
				},
				new EmoteStep
				{
					anim = "idle_pst"
				}
			};

			// Token: 0x04007FBA RID: 32698
			public Emote ClapCheer;

			// Token: 0x04007FBB RID: 32699
			public Emote Cheer;

			// Token: 0x04007FBC RID: 32700
			public Emote ProductiveCheer;

			// Token: 0x04007FBD RID: 32701
			public Emote ResearchComplete;

			// Token: 0x04007FBE RID: 32702
			public Emote ThumbsUp;

			// Token: 0x04007FBF RID: 32703
			public Emote CloseCall_Fall;

			// Token: 0x04007FC0 RID: 32704
			public Emote Cold;

			// Token: 0x04007FC1 RID: 32705
			public Emote Cough;

			// Token: 0x04007FC2 RID: 32706
			public Emote Cough_Small;

			// Token: 0x04007FC3 RID: 32707
			public Emote FoodPoisoning;

			// Token: 0x04007FC4 RID: 32708
			public Emote Hot;

			// Token: 0x04007FC5 RID: 32709
			public Emote IritatedEyes;

			// Token: 0x04007FC6 RID: 32710
			public Emote MorningStretch;

			// Token: 0x04007FC7 RID: 32711
			public Emote Radiation_Glare;

			// Token: 0x04007FC8 RID: 32712
			public Emote Radiation_Itch;

			// Token: 0x04007FC9 RID: 32713
			public Emote Sick;

			// Token: 0x04007FCA RID: 32714
			public Emote Sneeze;

			// Token: 0x04007FCB RID: 32715
			public Emote Sneeze_Short;

			// Token: 0x04007FCC RID: 32716
			public Emote Concern;

			// Token: 0x04007FCD RID: 32717
			public Emote Cringe;

			// Token: 0x04007FCE RID: 32718
			public Emote Disappointed;

			// Token: 0x04007FCF RID: 32719
			public Emote Shock;

			// Token: 0x04007FD0 RID: 32720
			public Emote Sing;

			// Token: 0x04007FD1 RID: 32721
			public Emote FingerGuns;

			// Token: 0x04007FD2 RID: 32722
			public Emote Wave;

			// Token: 0x04007FD3 RID: 32723
			public Emote Wave_Shy;
		}

		// Token: 0x02001C2B RID: 7211
		public class CritterEmotes : ResourceSet<Emote>
		{
			// Token: 0x06009C87 RID: 40071 RVA: 0x0034E253 File Offset: 0x0034C453
			public CritterEmotes(ResourceSet parent) : base("Critter", parent)
			{
				this.InitializePhysicalState();
				this.InitializeEmotionalState();
			}

			// Token: 0x06009C88 RID: 40072 RVA: 0x0034E270 File Offset: 0x0034C470
			private void InitializePhysicalState()
			{
				this.Hungry = new Emote(this, "Hungry", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_hungry"
					}
				}, null);
			}

			// Token: 0x06009C89 RID: 40073 RVA: 0x0034E2B0 File Offset: 0x0034C4B0
			private void InitializeEmotionalState()
			{
				this.Angry = new Emote(this, "Angry", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_angry"
					}
				}, null);
				this.Happy = new Emote(this, "Happy", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_happy"
					}
				}, null);
				this.Idle = new Emote(this, "Idle", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_idle"
					}
				}, null);
				this.Sad = new Emote(this, "Sad", new EmoteStep[]
				{
					new EmoteStep
					{
						anim = "react_sad"
					}
				}, null);
			}

			// Token: 0x04007FD4 RID: 32724
			public Emote Hungry;

			// Token: 0x04007FD5 RID: 32725
			public Emote Angry;

			// Token: 0x04007FD6 RID: 32726
			public Emote Happy;

			// Token: 0x04007FD7 RID: 32727
			public Emote Idle;

			// Token: 0x04007FD8 RID: 32728
			public Emote Sad;
		}
	}
}
