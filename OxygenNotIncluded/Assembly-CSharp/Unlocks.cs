using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using ProcGen;
using STRINGS;
using UnityEngine;

// Token: 0x02000A0D RID: 2573
[AddComponentMenu("KMonoBehaviour/scripts/Unlocks")]
public class Unlocks : KMonoBehaviour
{
	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x06004CF9 RID: 19705 RVA: 0x001AF5E3 File Offset: 0x001AD7E3
	private static string UnlocksFilename
	{
		get
		{
			return System.IO.Path.Combine(global::Util.RootFolder(), "unlocks.json");
		}
	}

	// Token: 0x06004CFA RID: 19706 RVA: 0x001AF5F4 File Offset: 0x001AD7F4
	protected override void OnPrefabInit()
	{
		this.LoadUnlocks();
	}

	// Token: 0x06004CFB RID: 19707 RVA: 0x001AF5FC File Offset: 0x001AD7FC
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UnlockCycleCodexes();
		GameClock.Instance.Subscribe(631075836, new Action<object>(this.OnNewDay));
		base.Subscribe<Unlocks>(-1277991738, Unlocks.OnLaunchRocketDelegate);
		base.Subscribe<Unlocks>(282337316, Unlocks.OnDuplicantDiedDelegate);
		base.Subscribe<Unlocks>(-818188514, Unlocks.OnDiscoveredSpaceDelegate);
		Components.LiveMinionIdentities.OnAdd += this.OnNewDupe;
	}

	// Token: 0x06004CFC RID: 19708 RVA: 0x001AF67A File Offset: 0x001AD87A
	public bool IsUnlocked(string unlockID)
	{
		return !string.IsNullOrEmpty(unlockID) && (DebugHandler.InstantBuildMode || this.unlocked.Contains(unlockID));
	}

	// Token: 0x06004CFD RID: 19709 RVA: 0x001AF69B File Offset: 0x001AD89B
	public IReadOnlyList<string> GetAllUnlockedIds()
	{
		return this.unlocked;
	}

	// Token: 0x06004CFE RID: 19710 RVA: 0x001AF6A3 File Offset: 0x001AD8A3
	public void Lock(string unlockID)
	{
		if (this.unlocked.Contains(unlockID))
		{
			this.unlocked.Remove(unlockID);
			this.SaveUnlocks();
			Game.Instance.Trigger(1594320620, unlockID);
		}
	}

	// Token: 0x06004CFF RID: 19711 RVA: 0x001AF6D8 File Offset: 0x001AD8D8
	public void Unlock(string unlockID, bool shouldTryShowCodexNotification = true)
	{
		if (string.IsNullOrEmpty(unlockID))
		{
			DebugUtil.DevAssert(false, "Unlock called with null or empty string", null);
			return;
		}
		if (!this.unlocked.Contains(unlockID))
		{
			this.unlocked.Add(unlockID);
			this.SaveUnlocks();
			Game.Instance.Trigger(1594320620, unlockID);
			if (shouldTryShowCodexNotification)
			{
				MessageNotification messageNotification = this.GenerateCodexUnlockNotification(unlockID);
				if (messageNotification != null)
				{
					base.GetComponent<Notifier>().Add(messageNotification, "");
				}
			}
		}
		this.EvalMetaCategories();
	}

	// Token: 0x06004D00 RID: 19712 RVA: 0x001AF750 File Offset: 0x001AD950
	private void EvalMetaCategories()
	{
		foreach (Unlocks.MetaUnlockCategory metaUnlockCategory in this.MetaUnlockCategories)
		{
			string metaCollectionID = metaUnlockCategory.metaCollectionID;
			string mesaCollectionID = metaUnlockCategory.mesaCollectionID;
			int mesaUnlockCount = metaUnlockCategory.mesaUnlockCount;
			int num = 0;
			foreach (string unlockID in this.lockCollections[mesaCollectionID])
			{
				if (this.IsUnlocked(unlockID))
				{
					num++;
				}
			}
			if (num >= mesaUnlockCount)
			{
				this.UnlockNext(metaCollectionID);
			}
		}
	}

	// Token: 0x06004D01 RID: 19713 RVA: 0x001AF7F8 File Offset: 0x001AD9F8
	private void SaveUnlocks()
	{
		if (!Directory.Exists(global::Util.RootFolder()))
		{
			Directory.CreateDirectory(global::Util.RootFolder());
		}
		string s = JsonConvert.SerializeObject(this.unlocked);
		bool flag = false;
		int num = 0;
		while (!flag && num < 5)
		{
			try
			{
				Thread.Sleep(num * 100);
				using (FileStream fileStream = File.Open(Unlocks.UnlocksFilename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
				{
					flag = true;
					byte[] bytes = new ASCIIEncoding().GetBytes(s);
					fileStream.Write(bytes, 0, bytes.Length);
				}
			}
			catch (Exception ex)
			{
				global::Debug.LogWarningFormat("Failed to save Unlocks attempt {0}: {1}", new object[]
				{
					num + 1,
					ex.ToString()
				});
			}
			num++;
		}
	}

	// Token: 0x06004D02 RID: 19714 RVA: 0x001AF8C0 File Offset: 0x001ADAC0
	public void LoadUnlocks()
	{
		this.unlocked.Clear();
		if (!File.Exists(Unlocks.UnlocksFilename))
		{
			return;
		}
		string text = "";
		bool flag = false;
		int num = 0;
		while (!flag && num < 5)
		{
			try
			{
				Thread.Sleep(num * 100);
				using (FileStream fileStream = File.Open(Unlocks.UnlocksFilename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					flag = true;
					ASCIIEncoding asciiencoding = new ASCIIEncoding();
					byte[] array = new byte[fileStream.Length];
					if ((long)fileStream.Read(array, 0, array.Length) == fileStream.Length)
					{
						text += asciiencoding.GetString(array);
					}
				}
			}
			catch (Exception ex)
			{
				global::Debug.LogWarningFormat("Failed to load Unlocks attempt {0}: {1}", new object[]
				{
					num + 1,
					ex.ToString()
				});
			}
			num++;
		}
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		try
		{
			foreach (string text2 in JsonConvert.DeserializeObject<string[]>(text))
			{
				if (!string.IsNullOrEmpty(text2) && !this.unlocked.Contains(text2))
				{
					this.unlocked.Add(text2);
				}
			}
		}
		catch (Exception ex2)
		{
			global::Debug.LogErrorFormat("Error parsing unlocks file [{0}]: {1}", new object[]
			{
				Unlocks.UnlocksFilename,
				ex2.ToString()
			});
		}
	}

	// Token: 0x06004D03 RID: 19715 RVA: 0x001AFA30 File Offset: 0x001ADC30
	public string UnlockNext(string collectionID)
	{
		foreach (string text in this.lockCollections[collectionID])
		{
			if (string.IsNullOrEmpty(text))
			{
				DebugUtil.DevAssertArgs(false, new object[]
				{
					"Found null/empty string in Unlocks collection: ",
					collectionID
				});
			}
			else if (!this.IsUnlocked(text))
			{
				this.Unlock(text, true);
				return text;
			}
		}
		return null;
	}

	// Token: 0x06004D04 RID: 19716 RVA: 0x001AFA94 File Offset: 0x001ADC94
	private MessageNotification GenerateCodexUnlockNotification(string lockID)
	{
		string entryForLock = CodexCache.GetEntryForLock(lockID);
		if (string.IsNullOrEmpty(entryForLock))
		{
			return null;
		}
		string text = null;
		if (CodexCache.FindSubEntry(lockID) != null)
		{
			text = CodexCache.FindSubEntry(lockID).title;
		}
		else if (CodexCache.FindSubEntry(entryForLock) != null)
		{
			text = CodexCache.FindSubEntry(entryForLock).title;
		}
		else if (CodexCache.FindEntry(entryForLock) != null)
		{
			text = CodexCache.FindEntry(entryForLock).title;
		}
		string text2 = UI.FormatAsLink(Strings.Get(text), entryForLock);
		if (!string.IsNullOrEmpty(text))
		{
			ContentContainer contentContainer = CodexCache.FindEntry(entryForLock).contentContainers.Find((ContentContainer match) => match.lockID == lockID);
			if (contentContainer != null)
			{
				foreach (ICodexWidget codexWidget in contentContainer.content)
				{
					CodexText codexText = codexWidget as CodexText;
					if (codexText != null)
					{
						text2 = text2 + "\n\n" + codexText.text;
					}
				}
			}
			return new MessageNotification(new CodexUnlockedMessage(lockID, text2));
		}
		return null;
	}

	// Token: 0x06004D05 RID: 19717 RVA: 0x001AFBC0 File Offset: 0x001ADDC0
	private void UnlockCycleCodexes()
	{
		foreach (KeyValuePair<int, string> keyValuePair in this.cycleLocked)
		{
			if (GameClock.Instance.GetCycle() + 1 >= keyValuePair.Key)
			{
				this.Unlock(keyValuePair.Value, true);
			}
		}
	}

	// Token: 0x06004D06 RID: 19718 RVA: 0x001AFC30 File Offset: 0x001ADE30
	private void OnNewDay(object data)
	{
		this.UnlockCycleCodexes();
	}

	// Token: 0x06004D07 RID: 19719 RVA: 0x001AFC38 File Offset: 0x001ADE38
	private void OnLaunchRocket(object data)
	{
		this.Unlock("surfacebreach", true);
		this.Unlock("firstrocketlaunch", true);
	}

	// Token: 0x06004D08 RID: 19720 RVA: 0x001AFC52 File Offset: 0x001ADE52
	private void OnDuplicantDied(object data)
	{
		this.Unlock("duplicantdeath", true);
		if (Components.LiveMinionIdentities.Count == 1)
		{
			this.Unlock("onedupeleft", true);
		}
	}

	// Token: 0x06004D09 RID: 19721 RVA: 0x001AFC79 File Offset: 0x001ADE79
	private void OnNewDupe(MinionIdentity minion_identity)
	{
		if (Components.LiveMinionIdentities.Count >= Db.Get().Personalities.GetAll(true, false).Count)
		{
			this.Unlock("fulldupecolony", true);
		}
	}

	// Token: 0x06004D0A RID: 19722 RVA: 0x001AFCA9 File Offset: 0x001ADEA9
	private void OnDiscoveredSpace(object data)
	{
		this.Unlock("surfacebreach", true);
	}

	// Token: 0x06004D0B RID: 19723 RVA: 0x001AFCB8 File Offset: 0x001ADEB8
	public void Sim4000ms(float dt)
	{
		int x = int.MinValue;
		int num = int.MinValue;
		int x2 = int.MaxValue;
		int num2 = int.MaxValue;
		foreach (MinionIdentity minionIdentity in Components.MinionIdentities.Items)
		{
			if (!(minionIdentity == null))
			{
				int cell = Grid.PosToCell(minionIdentity);
				if (Grid.IsValidCell(cell))
				{
					int num3;
					int num4;
					Grid.CellToXY(cell, out num3, out num4);
					if (num4 > num)
					{
						num = num4;
						x = num3;
					}
					if (num4 < num2)
					{
						x2 = num3;
						num2 = num4;
					}
				}
			}
		}
		if (num != -2147483648)
		{
			int num5 = num;
			for (int i = 0; i < 30; i++)
			{
				num5++;
				int cell2 = Grid.XYToCell(x, num5);
				if (!Grid.IsValidCell(cell2))
				{
					break;
				}
				if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(cell2) == SubWorld.ZoneType.Space)
				{
					this.Unlock("nearingsurface", true);
					break;
				}
			}
		}
		if (num2 != 2147483647)
		{
			int num6 = num2;
			for (int j = 0; j < 30; j++)
			{
				num6--;
				int num7 = Grid.XYToCell(x2, num6);
				if (!Grid.IsValidCell(num7))
				{
					break;
				}
				if (global::World.Instance.zoneRenderData.GetSubWorldZoneType(num7) == SubWorld.ZoneType.ToxicJungle && Grid.Element[num7].id == SimHashes.Magma)
				{
					this.Unlock("nearingmagma", true);
					return;
				}
			}
		}
	}

	// Token: 0x04003233 RID: 12851
	private const int FILE_IO_RETRY_ATTEMPTS = 5;

	// Token: 0x04003234 RID: 12852
	private List<string> unlocked = new List<string>();

	// Token: 0x04003235 RID: 12853
	private List<Unlocks.MetaUnlockCategory> MetaUnlockCategories = new List<Unlocks.MetaUnlockCategory>
	{
		new Unlocks.MetaUnlockCategory("dimensionalloreMeta", "dimensionallore", 4)
	};

	// Token: 0x04003236 RID: 12854
	public Dictionary<string, string[]> lockCollections = new Dictionary<string, string[]>
	{
		{
			"emails",
			new string[]
			{
				"email_thermodynamiclaws",
				"email_security2",
				"email_pens2",
				"email_atomiconrecruitment",
				"email_devonsblog",
				"email_researchgiant",
				"email_thejanitor",
				"email_newemployee",
				"email_timeoffapproved",
				"email_security3",
				"email_preliminarycalculations",
				"email_hollandsdog",
				"email_temporalbowupdate",
				"email_retemporalbowupdate",
				"email_memorychip",
				"email_arthistoryrequest",
				"email_AIcontrol",
				"email_AIcontrol2",
				"email_friendlyemail",
				"email_AIcontrol3",
				"email_AIcontrol4",
				"email_engineeringcandidate",
				"email_missingnotes",
				"email_journalistrequest",
				"email_journalistrequest2"
			}
		},
		{
			"journals",
			new string[]
			{
				"journal_timesarrowthoughts",
				"journal_A046_1",
				"journal_B835_1",
				"journal_sunflowerseeds",
				"journal_B327_1",
				"journal_B556_1",
				"journal_employeeprocessing",
				"journal_B327_2",
				"journal_A046_2",
				"journal_elliesbirthday1",
				"journal_B835_2",
				"journal_ants",
				"journal_pipedream",
				"journal_B556_2",
				"journal_movedrats",
				"journal_B835_3",
				"journal_A046_3",
				"journal_B556_3",
				"journal_B327_3",
				"journal_B835_4",
				"journal_cleanup",
				"journal_A046_4",
				"journal_B327_4",
				"journal_revisitednumbers",
				"journal_B556_4",
				"journal_B835_5",
				"journal_elliesbirthday2",
				"journal_B111_1",
				"journal_revisitednumbers2",
				"journal_timemusings",
				"journal_evil",
				"journal_timesorder",
				"journal_inspace",
				"journal_mysteryaward",
				"journal_courier"
			}
		},
		{
			"researchnotes",
			new string[]
			{
				"notes_clonedrats",
				"notes_agriculture1",
				"notes_husbandry1",
				"notes_hibiscus3",
				"notes_husbandry2",
				"notes_agriculture2",
				"notes_geneticooze",
				"notes_agriculture3",
				"notes_husbandry3",
				"notes_memoryimplantation",
				"notes_husbandry4",
				"notes_agriculture4",
				"notes_neutronium",
				"notes_firstsuccess",
				"notes_neutroniumapplications",
				"notes_teleportation",
				"notes_AI",
				"cryotank_warning"
			}
		},
		{
			"misc",
			new string[]
			{
				"misc_newsecurity",
				"misc_mailroometiquette",
				"misc_unattendedcultures",
				"misc_politerequest",
				"misc_casualfriday",
				"misc_dishbot"
			}
		},
		{
			"dimensionallore",
			new string[]
			{
				"notes_clonedrabbits",
				"notes_clonedraccoons",
				"journal_movedrabbits",
				"journal_movedraccoons",
				"journal_strawberries",
				"journal_shrimp"
			}
		},
		{
			"dimensionalloreMeta",
			new string[]
			{
				"log9"
			}
		},
		{
			"space",
			new string[]
			{
				"display_spaceprop1",
				"notice_pilot",
				"journal_inspace",
				"notes_firstcolony"
			}
		},
		{
			"storytraits",
			new string[]
			{
				"story_trait_critter_manipulator_initial",
				"story_trait_critter_manipulator_complete",
				"storytrait_crittermanipulator_workiversary",
				"story_trait_mega_brain_tank_initial",
				"story_trait_mega_brain_tank_competed",
				"story_trait_fossilhunt_initial",
				"story_trait_fossilhunt_poi1",
				"story_trait_fossilhunt_poi2",
				"story_trait_fossilhunt_poi3",
				"story_trait_fossilhunt_complete"
			}
		}
	};

	// Token: 0x04003237 RID: 12855
	public Dictionary<int, string> cycleLocked = new Dictionary<int, string>
	{
		{
			0,
			"log1"
		},
		{
			3,
			"log2"
		},
		{
			15,
			"log3"
		},
		{
			1000,
			"log4"
		},
		{
			1500,
			"log4b"
		},
		{
			2000,
			"log5"
		},
		{
			2500,
			"log5b"
		},
		{
			3000,
			"log6"
		},
		{
			3500,
			"log6b"
		},
		{
			4000,
			"log7"
		},
		{
			4001,
			"log8"
		}
	};

	// Token: 0x04003238 RID: 12856
	private static readonly EventSystem.IntraObjectHandler<Unlocks> OnLaunchRocketDelegate = new EventSystem.IntraObjectHandler<Unlocks>(delegate(Unlocks component, object data)
	{
		component.OnLaunchRocket(data);
	});

	// Token: 0x04003239 RID: 12857
	private static readonly EventSystem.IntraObjectHandler<Unlocks> OnDuplicantDiedDelegate = new EventSystem.IntraObjectHandler<Unlocks>(delegate(Unlocks component, object data)
	{
		component.OnDuplicantDied(data);
	});

	// Token: 0x0400323A RID: 12858
	private static readonly EventSystem.IntraObjectHandler<Unlocks> OnDiscoveredSpaceDelegate = new EventSystem.IntraObjectHandler<Unlocks>(delegate(Unlocks component, object data)
	{
		component.OnDiscoveredSpace(data);
	});

	// Token: 0x02001898 RID: 6296
	private class MetaUnlockCategory
	{
		// Token: 0x0600922F RID: 37423 RVA: 0x0032B828 File Offset: 0x00329A28
		public MetaUnlockCategory(string metaCollectionID, string mesaCollectionID, int mesaUnlockCount)
		{
			this.metaCollectionID = metaCollectionID;
			this.mesaCollectionID = mesaCollectionID;
			this.mesaUnlockCount = mesaUnlockCount;
		}

		// Token: 0x0400726D RID: 29293
		public string metaCollectionID;

		// Token: 0x0400726E RID: 29294
		public string mesaCollectionID;

		// Token: 0x0400726F RID: 29295
		public int mesaUnlockCount;
	}
}
