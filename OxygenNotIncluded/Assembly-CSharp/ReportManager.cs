using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020004F5 RID: 1269
[AddComponentMenu("KMonoBehaviour/scripts/ReportManager")]
public class ReportManager : KMonoBehaviour
{
	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06001DC6 RID: 7622 RVA: 0x0009E4F5 File Offset: 0x0009C6F5
	public List<ReportManager.DailyReport> reports
	{
		get
		{
			return this.dailyReports;
		}
	}

	// Token: 0x06001DC7 RID: 7623 RVA: 0x0009E4FD File Offset: 0x0009C6FD
	public static void DestroyInstance()
	{
		ReportManager.Instance = null;
	}

	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x0009E505 File Offset: 0x0009C705
	// (set) Token: 0x06001DC9 RID: 7625 RVA: 0x0009E50C File Offset: 0x0009C70C
	public static ReportManager Instance { get; private set; }

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06001DCA RID: 7626 RVA: 0x0009E514 File Offset: 0x0009C714
	public ReportManager.DailyReport TodaysReport
	{
		get
		{
			return this.todaysReport;
		}
	}

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06001DCB RID: 7627 RVA: 0x0009E51C File Offset: 0x0009C71C
	public ReportManager.DailyReport YesterdaysReport
	{
		get
		{
			if (this.dailyReports.Count <= 1)
			{
				return null;
			}
			return this.dailyReports[this.dailyReports.Count - 1];
		}
	}

	// Token: 0x06001DCC RID: 7628 RVA: 0x0009E546 File Offset: 0x0009C746
	protected override void OnPrefabInit()
	{
		ReportManager.Instance = this;
		base.Subscribe(Game.Instance.gameObject, -1917495436, new Action<object>(this.OnSaveGameReady));
		this.noteStorage = new ReportManager.NoteStorage();
	}

	// Token: 0x06001DCD RID: 7629 RVA: 0x0009E57B File Offset: 0x0009C77B
	protected override void OnCleanUp()
	{
		ReportManager.Instance = null;
	}

	// Token: 0x06001DCE RID: 7630 RVA: 0x0009E583 File Offset: 0x0009C783
	[CustomSerialize]
	private void CustomSerialize(BinaryWriter writer)
	{
		writer.Write(0);
		this.noteStorage.Serialize(writer);
	}

	// Token: 0x06001DCF RID: 7631 RVA: 0x0009E598 File Offset: 0x0009C798
	[CustomDeserialize]
	private void CustomDeserialize(IReader reader)
	{
		if (this.noteStorageBytes == null)
		{
			global::Debug.Assert(reader.ReadInt32() == 0);
			BinaryReader binaryReader = new BinaryReader(new MemoryStream(reader.RawBytes()));
			binaryReader.BaseStream.Position = (long)reader.Position;
			this.noteStorage.Deserialize(binaryReader);
			reader.SkipBytes((int)binaryReader.BaseStream.Position - reader.Position);
		}
	}

	// Token: 0x06001DD0 RID: 7632 RVA: 0x0009E603 File Offset: 0x0009C803
	[OnDeserialized]
	private void OnDeserialized()
	{
		if (this.noteStorageBytes != null)
		{
			this.noteStorage.Deserialize(new BinaryReader(new MemoryStream(this.noteStorageBytes)));
			this.noteStorageBytes = null;
		}
	}

	// Token: 0x06001DD1 RID: 7633 RVA: 0x0009E630 File Offset: 0x0009C830
	private void OnSaveGameReady(object data)
	{
		base.Subscribe(GameClock.Instance.gameObject, -722330267, new Action<object>(this.OnNightTime));
		if (this.todaysReport == null)
		{
			this.todaysReport = new ReportManager.DailyReport(this);
			this.todaysReport.day = GameUtil.GetCurrentCycle();
		}
	}

	// Token: 0x06001DD2 RID: 7634 RVA: 0x0009E683 File Offset: 0x0009C883
	public void ReportValue(ReportManager.ReportType reportType, float value, string note = null, string context = null)
	{
		this.TodaysReport.AddData(reportType, value, note, context);
	}

	// Token: 0x06001DD3 RID: 7635 RVA: 0x0009E698 File Offset: 0x0009C898
	private void OnNightTime(object data)
	{
		this.dailyReports.Add(this.todaysReport);
		int day = this.todaysReport.day;
		ManagementMenuNotification notification = new ManagementMenuNotification(global::Action.ManageReport, NotificationValence.Good, null, string.Format(UI.ENDOFDAYREPORT.NOTIFICATION_TITLE, day), NotificationType.Good, (List<Notification> n, object d) => string.Format(UI.ENDOFDAYREPORT.NOTIFICATION_TOOLTIP, day), null, true, 0f, delegate(object d)
		{
			ManagementMenu.Instance.OpenReports(day);
		}, null, null, true);
		if (this.notifier == null)
		{
			global::Debug.LogError("Cant notify, null notifier");
		}
		else
		{
			this.notifier.Add(notification, "");
		}
		this.todaysReport = new ReportManager.DailyReport(this);
		this.todaysReport.day = GameUtil.GetCurrentCycle() + 1;
	}

	// Token: 0x06001DD4 RID: 7636 RVA: 0x0009E760 File Offset: 0x0009C960
	public ReportManager.DailyReport FindReport(int day)
	{
		foreach (ReportManager.DailyReport dailyReport in this.dailyReports)
		{
			if (dailyReport.day == day)
			{
				return dailyReport;
			}
		}
		if (this.todaysReport.day == day)
		{
			return this.todaysReport;
		}
		return null;
	}

	// Token: 0x06001DD5 RID: 7637 RVA: 0x0009E7D4 File Offset: 0x0009C9D4
	public ReportManager()
	{
		Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> dictionary = new Dictionary<ReportManager.ReportType, ReportManager.ReportGroup>();
		dictionary.Add(ReportManager.ReportType.DuplicantHeader, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.DUPLICANT_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.CaloriesCreated, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedCalories(v, GameUtil.TimeSlice.None, true), true, 1, UI.ENDOFDAYREPORT.CALORIES_CREATED.NAME, UI.ENDOFDAYREPORT.CALORIES_CREATED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CALORIES_CREATED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.StressDelta, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v, GameUtil.TimeSlice.None), true, 1, UI.ENDOFDAYREPORT.STRESS_DELTA.NAME, UI.ENDOFDAYREPORT.STRESS_DELTA.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.STRESS_DELTA.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DiseaseAdded, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.DISEASE_ADDED.NAME, UI.ENDOFDAYREPORT.DISEASE_ADDED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.DISEASE_ADDED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DiseaseStatus, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedDiseaseAmount((int)v, GameUtil.TimeSlice.None), true, 1, UI.ENDOFDAYREPORT.DISEASE_STATUS.NAME, UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP, UI.ENDOFDAYREPORT.DISEASE_STATUS.TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.LevelUp, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.LEVEL_UP.NAME, UI.ENDOFDAYREPORT.LEVEL_UP.TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ToiletIncident, new ReportManager.ReportGroup(null, false, 1, UI.ENDOFDAYREPORT.TOILET_INCIDENT.NAME, UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP, UI.ENDOFDAYREPORT.TOILET_INCIDENT.TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ChoreStatus, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.CHORE_STATUS.NAME, UI.ENDOFDAYREPORT.CHORE_STATUS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CHORE_STATUS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.DomesticatedCritters, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NAME, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NUMBER_OF_DOMESTICATED_CRITTERS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.WildCritters, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NAME, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NUMBER_OF_WILD_CRITTERS.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.RocketsInFlight, new ReportManager.ReportGroup(null, true, 1, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NAME, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.ROCKETS_IN_FLIGHT.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.TimeSpentHeader, new ReportManager.ReportGroup(null, true, 2, UI.ENDOFDAYREPORT.TIME_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.WorkTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.WORK_TIME.NAME, UI.ENDOFDAYREPORT.WORK_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.TravelTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.TRAVEL_TIME.NAME, UI.ENDOFDAYREPORT.TRAVEL_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.PersonalTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.PERSONAL_TIME.NAME, UI.ENDOFDAYREPORT.PERSONAL_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.IdleTime, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedPercent(v / 600f * 100f, GameUtil.TimeSlice.None), true, 2, UI.ENDOFDAYREPORT.IDLE_TIME.NAME, UI.ENDOFDAYREPORT.IDLE_TIME.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.NONE, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, (float v, float num_entries) => GameUtil.GetFormattedPercent(v / 600f * 100f / num_entries, GameUtil.TimeSlice.None)));
		dictionary.Add(ReportManager.ReportType.BaseHeader, new ReportManager.ReportGroup(null, true, 3, UI.ENDOFDAYREPORT.BASE_DETAILS_HEADER, "", "", ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order.Unordered, true, null));
		dictionary.Add(ReportManager.ReportType.OxygenCreated, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), true, 3, UI.ENDOFDAYREPORT.OXYGEN_CREATED.NAME, UI.ENDOFDAYREPORT.OXYGEN_CREATED.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.OXYGEN_CREATED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.EnergyCreated, new ReportManager.ReportGroup(new ReportManager.FormattingFn(GameUtil.GetFormattedRoundedJoules), true, 3, UI.ENDOFDAYREPORT.ENERGY_USAGE.NAME, UI.ENDOFDAYREPORT.ENERGY_USAGE.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.ENERGY_USAGE.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.EnergyWasted, new ReportManager.ReportGroup(new ReportManager.FormattingFn(GameUtil.GetFormattedRoundedJoules), true, 3, UI.ENDOFDAYREPORT.ENERGY_WASTED.NAME, UI.ENDOFDAYREPORT.NONE, UI.ENDOFDAYREPORT.ENERGY_WASTED.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ContaminatedOxygenToilet, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), false, 3, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NAME, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_TOILET.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		dictionary.Add(ReportManager.ReportType.ContaminatedOxygenSublimation, new ReportManager.ReportGroup((float v) => GameUtil.GetFormattedMass(v, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"), false, 3, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NAME, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.POSITIVE_TOOLTIP, UI.ENDOFDAYREPORT.CONTAMINATED_OXYGEN_SUBLIMATION.NEGATIVE_TOOLTIP, ReportManager.ReportEntry.Order.Descending, ReportManager.ReportEntry.Order.Descending, false, null));
		this.ReportGroups = dictionary;
		this.dailyReports = new List<ReportManager.DailyReport>();
		base..ctor();
	}

	// Token: 0x0400108F RID: 4239
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04001090 RID: 4240
	private ReportManager.NoteStorage noteStorage;

	// Token: 0x04001091 RID: 4241
	public Dictionary<ReportManager.ReportType, ReportManager.ReportGroup> ReportGroups;

	// Token: 0x04001092 RID: 4242
	[Serialize]
	private List<ReportManager.DailyReport> dailyReports;

	// Token: 0x04001093 RID: 4243
	[Serialize]
	private ReportManager.DailyReport todaysReport;

	// Token: 0x04001094 RID: 4244
	[Serialize]
	private byte[] noteStorageBytes;

	// Token: 0x02001199 RID: 4505
	// (Invoke) Token: 0x06007A1E RID: 31262
	public delegate string FormattingFn(float v);

	// Token: 0x0200119A RID: 4506
	// (Invoke) Token: 0x06007A22 RID: 31266
	public delegate string GroupFormattingFn(float v, float numEntries);

	// Token: 0x0200119B RID: 4507
	public enum ReportType
	{
		// Token: 0x04005CC1 RID: 23745
		DuplicantHeader,
		// Token: 0x04005CC2 RID: 23746
		CaloriesCreated,
		// Token: 0x04005CC3 RID: 23747
		StressDelta,
		// Token: 0x04005CC4 RID: 23748
		LevelUp,
		// Token: 0x04005CC5 RID: 23749
		DiseaseStatus,
		// Token: 0x04005CC6 RID: 23750
		DiseaseAdded,
		// Token: 0x04005CC7 RID: 23751
		ToiletIncident,
		// Token: 0x04005CC8 RID: 23752
		ChoreStatus,
		// Token: 0x04005CC9 RID: 23753
		TimeSpentHeader,
		// Token: 0x04005CCA RID: 23754
		TimeSpent,
		// Token: 0x04005CCB RID: 23755
		WorkTime,
		// Token: 0x04005CCC RID: 23756
		TravelTime,
		// Token: 0x04005CCD RID: 23757
		PersonalTime,
		// Token: 0x04005CCE RID: 23758
		IdleTime,
		// Token: 0x04005CCF RID: 23759
		BaseHeader,
		// Token: 0x04005CD0 RID: 23760
		ContaminatedOxygenFlatulence,
		// Token: 0x04005CD1 RID: 23761
		ContaminatedOxygenToilet,
		// Token: 0x04005CD2 RID: 23762
		ContaminatedOxygenSublimation,
		// Token: 0x04005CD3 RID: 23763
		OxygenCreated,
		// Token: 0x04005CD4 RID: 23764
		EnergyCreated,
		// Token: 0x04005CD5 RID: 23765
		EnergyWasted,
		// Token: 0x04005CD6 RID: 23766
		DomesticatedCritters,
		// Token: 0x04005CD7 RID: 23767
		WildCritters,
		// Token: 0x04005CD8 RID: 23768
		RocketsInFlight
	}

	// Token: 0x0200119C RID: 4508
	public struct ReportGroup
	{
		// Token: 0x06007A25 RID: 31269 RVA: 0x002DB1B4 File Offset: 0x002D93B4
		public ReportGroup(ReportManager.FormattingFn formatfn, bool reportIfZero, int group, string stringKey, string positiveTooltip, string negativeTooltip, ReportManager.ReportEntry.Order pos_note_order = ReportManager.ReportEntry.Order.Unordered, ReportManager.ReportEntry.Order neg_note_order = ReportManager.ReportEntry.Order.Unordered, bool is_header = false, ReportManager.GroupFormattingFn group_format_fn = null)
		{
			ReportManager.FormattingFn formattingFn;
			if (formatfn == null)
			{
				formattingFn = ((float v) => v.ToString());
			}
			else
			{
				formattingFn = formatfn;
			}
			this.formatfn = formattingFn;
			this.groupFormatfn = group_format_fn;
			this.stringKey = stringKey;
			this.positiveTooltip = positiveTooltip;
			this.negativeTooltip = negativeTooltip;
			this.reportIfZero = reportIfZero;
			this.group = group;
			this.posNoteOrder = pos_note_order;
			this.negNoteOrder = neg_note_order;
			this.isHeader = is_header;
		}

		// Token: 0x04005CD9 RID: 23769
		public ReportManager.FormattingFn formatfn;

		// Token: 0x04005CDA RID: 23770
		public ReportManager.GroupFormattingFn groupFormatfn;

		// Token: 0x04005CDB RID: 23771
		public string stringKey;

		// Token: 0x04005CDC RID: 23772
		public string positiveTooltip;

		// Token: 0x04005CDD RID: 23773
		public string negativeTooltip;

		// Token: 0x04005CDE RID: 23774
		public bool reportIfZero;

		// Token: 0x04005CDF RID: 23775
		public int group;

		// Token: 0x04005CE0 RID: 23776
		public bool isHeader;

		// Token: 0x04005CE1 RID: 23777
		public ReportManager.ReportEntry.Order posNoteOrder;

		// Token: 0x04005CE2 RID: 23778
		public ReportManager.ReportEntry.Order negNoteOrder;
	}

	// Token: 0x0200119D RID: 4509
	[SerializationConfig(MemberSerialization.OptIn)]
	public class ReportEntry
	{
		// Token: 0x06007A26 RID: 31270 RVA: 0x002DB234 File Offset: 0x002D9434
		public ReportEntry(ReportManager.ReportType reportType, int note_storage_id, string context, bool is_child = false)
		{
			this.reportType = reportType;
			this.context = context;
			this.isChild = is_child;
			this.accumulate = 0f;
			this.accPositive = 0f;
			this.accNegative = 0f;
			this.noteStorageId = note_storage_id;
		}

		// Token: 0x1700083A RID: 2106
		// (get) Token: 0x06007A27 RID: 31271 RVA: 0x002DB28C File Offset: 0x002D948C
		public float Positive
		{
			get
			{
				return this.accPositive;
			}
		}

		// Token: 0x1700083B RID: 2107
		// (get) Token: 0x06007A28 RID: 31272 RVA: 0x002DB294 File Offset: 0x002D9494
		public float Negative
		{
			get
			{
				return this.accNegative;
			}
		}

		// Token: 0x1700083C RID: 2108
		// (get) Token: 0x06007A29 RID: 31273 RVA: 0x002DB29C File Offset: 0x002D949C
		public float Net
		{
			get
			{
				return this.accPositive + this.accNegative;
			}
		}

		// Token: 0x06007A2A RID: 31274 RVA: 0x002DB2AB File Offset: 0x002D94AB
		[OnDeserializing]
		private void OnDeserialize()
		{
			this.contextEntries.Clear();
		}

		// Token: 0x06007A2B RID: 31275 RVA: 0x002DB2B8 File Offset: 0x002D94B8
		public void IterateNotes(Action<ReportManager.ReportEntry.Note> callback)
		{
			ReportManager.Instance.noteStorage.IterateNotes(this.noteStorageId, callback);
		}

		// Token: 0x06007A2C RID: 31276 RVA: 0x002DB2D0 File Offset: 0x002D94D0
		[OnDeserialized]
		private void OnDeserialized()
		{
			if (this.gameHash != -1)
			{
				this.reportType = (ReportManager.ReportType)this.gameHash;
				this.gameHash = -1;
			}
		}

		// Token: 0x06007A2D RID: 31277 RVA: 0x002DB2F0 File Offset: 0x002D94F0
		public void AddData(ReportManager.NoteStorage note_storage, float value, string note = null, string dataContext = null)
		{
			this.AddActualData(note_storage, value, note);
			if (dataContext != null)
			{
				ReportManager.ReportEntry reportEntry = null;
				for (int i = 0; i < this.contextEntries.Count; i++)
				{
					if (this.contextEntries[i].context == dataContext)
					{
						reportEntry = this.contextEntries[i];
						break;
					}
				}
				if (reportEntry == null)
				{
					reportEntry = new ReportManager.ReportEntry(this.reportType, note_storage.GetNewNoteId(), dataContext, true);
					this.contextEntries.Add(reportEntry);
				}
				reportEntry.AddActualData(note_storage, value, note);
			}
		}

		// Token: 0x06007A2E RID: 31278 RVA: 0x002DB37C File Offset: 0x002D957C
		private void AddActualData(ReportManager.NoteStorage note_storage, float value, string note = null)
		{
			this.accumulate += value;
			if (value > 0f)
			{
				this.accPositive += value;
			}
			else
			{
				this.accNegative += value;
			}
			if (note != null)
			{
				note_storage.Add(this.noteStorageId, value, note);
			}
		}

		// Token: 0x06007A2F RID: 31279 RVA: 0x002DB3CE File Offset: 0x002D95CE
		public bool HasContextEntries()
		{
			return this.contextEntries.Count > 0;
		}

		// Token: 0x04005CE3 RID: 23779
		[Serialize]
		public int noteStorageId;

		// Token: 0x04005CE4 RID: 23780
		[Serialize]
		public int gameHash = -1;

		// Token: 0x04005CE5 RID: 23781
		[Serialize]
		public ReportManager.ReportType reportType;

		// Token: 0x04005CE6 RID: 23782
		[Serialize]
		public string context;

		// Token: 0x04005CE7 RID: 23783
		[Serialize]
		public float accumulate;

		// Token: 0x04005CE8 RID: 23784
		[Serialize]
		public float accPositive;

		// Token: 0x04005CE9 RID: 23785
		[Serialize]
		public float accNegative;

		// Token: 0x04005CEA RID: 23786
		[Serialize]
		public ArrayRef<ReportManager.ReportEntry> contextEntries;

		// Token: 0x04005CEB RID: 23787
		public bool isChild;

		// Token: 0x0200209F RID: 8351
		public struct Note
		{
			// Token: 0x0600A643 RID: 42563 RVA: 0x0036DA1F File Offset: 0x0036BC1F
			public Note(float value, string note)
			{
				this.value = value;
				this.note = note;
			}

			// Token: 0x040091BE RID: 37310
			public float value;

			// Token: 0x040091BF RID: 37311
			public string note;
		}

		// Token: 0x020020A0 RID: 8352
		public enum Order
		{
			// Token: 0x040091C1 RID: 37313
			Unordered,
			// Token: 0x040091C2 RID: 37314
			Ascending,
			// Token: 0x040091C3 RID: 37315
			Descending
		}
	}

	// Token: 0x0200119E RID: 4510
	public class DailyReport
	{
		// Token: 0x06007A30 RID: 31280 RVA: 0x002DB3E0 File Offset: 0x002D95E0
		public DailyReport(ReportManager manager)
		{
			foreach (KeyValuePair<ReportManager.ReportType, ReportManager.ReportGroup> keyValuePair in manager.ReportGroups)
			{
				this.reportEntries.Add(new ReportManager.ReportEntry(keyValuePair.Key, this.noteStorage.GetNewNoteId(), null, false));
			}
		}

		// Token: 0x1700083D RID: 2109
		// (get) Token: 0x06007A31 RID: 31281 RVA: 0x002DB464 File Offset: 0x002D9664
		private ReportManager.NoteStorage noteStorage
		{
			get
			{
				return ReportManager.Instance.noteStorage;
			}
		}

		// Token: 0x06007A32 RID: 31282 RVA: 0x002DB470 File Offset: 0x002D9670
		public ReportManager.ReportEntry GetEntry(ReportManager.ReportType reportType)
		{
			for (int i = 0; i < this.reportEntries.Count; i++)
			{
				ReportManager.ReportEntry reportEntry = this.reportEntries[i];
				if (reportEntry.reportType == reportType)
				{
					return reportEntry;
				}
			}
			ReportManager.ReportEntry reportEntry2 = new ReportManager.ReportEntry(reportType, this.noteStorage.GetNewNoteId(), null, false);
			this.reportEntries.Add(reportEntry2);
			return reportEntry2;
		}

		// Token: 0x06007A33 RID: 31283 RVA: 0x002DB4CC File Offset: 0x002D96CC
		public void AddData(ReportManager.ReportType reportType, float value, string note = null, string context = null)
		{
			this.GetEntry(reportType).AddData(this.noteStorage, value, note, context);
		}

		// Token: 0x04005CEC RID: 23788
		[Serialize]
		public int day;

		// Token: 0x04005CED RID: 23789
		[Serialize]
		public List<ReportManager.ReportEntry> reportEntries = new List<ReportManager.ReportEntry>();
	}

	// Token: 0x0200119F RID: 4511
	public class NoteStorage
	{
		// Token: 0x06007A34 RID: 31284 RVA: 0x002DB4E4 File Offset: 0x002D96E4
		public NoteStorage()
		{
			this.noteEntries = new ReportManager.NoteStorage.NoteEntries();
			this.stringTable = new ReportManager.NoteStorage.StringTable();
		}

		// Token: 0x06007A35 RID: 31285 RVA: 0x002DB504 File Offset: 0x002D9704
		public void Add(int report_entry_id, float value, string note)
		{
			int note_id = this.stringTable.AddString(note, 6);
			this.noteEntries.Add(report_entry_id, value, note_id);
		}

		// Token: 0x06007A36 RID: 31286 RVA: 0x002DB530 File Offset: 0x002D9730
		public int GetNewNoteId()
		{
			int result = this.nextNoteId + 1;
			this.nextNoteId = result;
			return result;
		}

		// Token: 0x06007A37 RID: 31287 RVA: 0x002DB54E File Offset: 0x002D974E
		public void IterateNotes(int report_entry_id, Action<ReportManager.ReportEntry.Note> callback)
		{
			this.noteEntries.IterateNotes(this.stringTable, report_entry_id, callback);
		}

		// Token: 0x06007A38 RID: 31288 RVA: 0x002DB563 File Offset: 0x002D9763
		public void Serialize(BinaryWriter writer)
		{
			writer.Write(6);
			writer.Write(this.nextNoteId);
			this.stringTable.Serialize(writer);
			this.noteEntries.Serialize(writer);
		}

		// Token: 0x06007A39 RID: 31289 RVA: 0x002DB590 File Offset: 0x002D9790
		public void Deserialize(BinaryReader reader)
		{
			int num = reader.ReadInt32();
			if (num < 5)
			{
				return;
			}
			this.nextNoteId = reader.ReadInt32();
			this.stringTable.Deserialize(reader, num);
			this.noteEntries.Deserialize(reader, num);
		}

		// Token: 0x04005CEE RID: 23790
		public const int SERIALIZATION_VERSION = 6;

		// Token: 0x04005CEF RID: 23791
		private int nextNoteId;

		// Token: 0x04005CF0 RID: 23792
		private ReportManager.NoteStorage.NoteEntries noteEntries;

		// Token: 0x04005CF1 RID: 23793
		private ReportManager.NoteStorage.StringTable stringTable;

		// Token: 0x020020A1 RID: 8353
		private class StringTable
		{
			// Token: 0x0600A644 RID: 42564 RVA: 0x0036DA30 File Offset: 0x0036BC30
			public int AddString(string str, int version = 6)
			{
				int num = Hash.SDBMLower(str);
				this.strings[num] = str;
				return num;
			}

			// Token: 0x0600A645 RID: 42565 RVA: 0x0036DA54 File Offset: 0x0036BC54
			public string GetStringByHash(int hash)
			{
				string result = "";
				this.strings.TryGetValue(hash, out result);
				return result;
			}

			// Token: 0x0600A646 RID: 42566 RVA: 0x0036DA78 File Offset: 0x0036BC78
			public void Serialize(BinaryWriter writer)
			{
				writer.Write(this.strings.Count);
				foreach (KeyValuePair<int, string> keyValuePair in this.strings)
				{
					writer.Write(keyValuePair.Value);
				}
			}

			// Token: 0x0600A647 RID: 42567 RVA: 0x0036DAE4 File Offset: 0x0036BCE4
			public void Deserialize(BinaryReader reader, int version)
			{
				int num = reader.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					string str = reader.ReadString();
					this.AddString(str, version);
				}
			}

			// Token: 0x040091C4 RID: 37316
			private Dictionary<int, string> strings = new Dictionary<int, string>();
		}

		// Token: 0x020020A2 RID: 8354
		private class NoteEntries
		{
			// Token: 0x0600A649 RID: 42569 RVA: 0x0036DB28 File Offset: 0x0036BD28
			public void Add(int report_entry_id, float value, int note_id)
			{
				Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary;
				if (!this.entries.TryGetValue(report_entry_id, out dictionary))
				{
					dictionary = new Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>(ReportManager.NoteStorage.NoteEntries.sKeyComparer);
					this.entries[report_entry_id] = dictionary;
				}
				ReportManager.NoteStorage.NoteEntries.NoteEntryKey noteEntryKey = new ReportManager.NoteStorage.NoteEntries.NoteEntryKey
				{
					noteHash = note_id,
					isPositive = (value > 0f)
				};
				if (dictionary.ContainsKey(noteEntryKey))
				{
					Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary2 = dictionary;
					ReportManager.NoteStorage.NoteEntries.NoteEntryKey key = noteEntryKey;
					dictionary2[key] += value;
					return;
				}
				dictionary[noteEntryKey] = value;
			}

			// Token: 0x0600A64A RID: 42570 RVA: 0x0036DBA4 File Offset: 0x0036BDA4
			public void Serialize(BinaryWriter writer)
			{
				writer.Write(this.entries.Count);
				foreach (KeyValuePair<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>> keyValuePair in this.entries)
				{
					writer.Write(keyValuePair.Key);
					writer.Write(keyValuePair.Value.Count);
					foreach (KeyValuePair<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> keyValuePair2 in keyValuePair.Value)
					{
						writer.Write(keyValuePair2.Key.noteHash);
						writer.Write(keyValuePair2.Key.isPositive);
						writer.WriteSingleFast(keyValuePair2.Value);
					}
				}
			}

			// Token: 0x0600A64B RID: 42571 RVA: 0x0036DC94 File Offset: 0x0036BE94
			public void Deserialize(BinaryReader reader, int version)
			{
				if (version < 6)
				{
					OldNoteEntriesV5 oldNoteEntriesV = new OldNoteEntriesV5();
					oldNoteEntriesV.Deserialize(reader);
					foreach (OldNoteEntriesV5.NoteStorageBlock noteStorageBlock in oldNoteEntriesV.storageBlocks)
					{
						for (int i = 0; i < noteStorageBlock.entryCount; i++)
						{
							OldNoteEntriesV5.NoteEntry noteEntry = noteStorageBlock.entries.structs[i];
							this.Add(noteEntry.reportEntryId, noteEntry.value, noteEntry.noteHash);
						}
					}
					return;
				}
				int num = reader.ReadInt32();
				this.entries = new Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>>(num);
				for (int j = 0; j < num; j++)
				{
					int key = reader.ReadInt32();
					int num2 = reader.ReadInt32();
					Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary = new Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>(num2, ReportManager.NoteStorage.NoteEntries.sKeyComparer);
					this.entries[key] = dictionary;
					for (int k = 0; k < num2; k++)
					{
						ReportManager.NoteStorage.NoteEntries.NoteEntryKey key2 = new ReportManager.NoteStorage.NoteEntries.NoteEntryKey
						{
							noteHash = reader.ReadInt32(),
							isPositive = reader.ReadBoolean()
						};
						dictionary[key2] = reader.ReadSingle();
					}
				}
			}

			// Token: 0x0600A64C RID: 42572 RVA: 0x0036DDC8 File Offset: 0x0036BFC8
			public void IterateNotes(ReportManager.NoteStorage.StringTable string_table, int report_entry_id, Action<ReportManager.ReportEntry.Note> callback)
			{
				Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> dictionary;
				if (this.entries.TryGetValue(report_entry_id, out dictionary))
				{
					foreach (KeyValuePair<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float> keyValuePair in dictionary)
					{
						string stringByHash = string_table.GetStringByHash(keyValuePair.Key.noteHash);
						ReportManager.ReportEntry.Note obj = new ReportManager.ReportEntry.Note(keyValuePair.Value, stringByHash);
						callback(obj);
					}
				}
			}

			// Token: 0x040091C5 RID: 37317
			private static ReportManager.NoteStorage.NoteEntries.NoteEntryKeyComparer sKeyComparer = new ReportManager.NoteStorage.NoteEntries.NoteEntryKeyComparer();

			// Token: 0x040091C6 RID: 37318
			private Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>> entries = new Dictionary<int, Dictionary<ReportManager.NoteStorage.NoteEntries.NoteEntryKey, float>>();

			// Token: 0x02002F5B RID: 12123
			public struct NoteEntryKey
			{
				// Token: 0x0400C19C RID: 49564
				public int noteHash;

				// Token: 0x0400C19D RID: 49565
				public bool isPositive;
			}

			// Token: 0x02002F5C RID: 12124
			public class NoteEntryKeyComparer : IEqualityComparer<ReportManager.NoteStorage.NoteEntries.NoteEntryKey>
			{
				// Token: 0x0600C6F4 RID: 50932 RVA: 0x003C1C1B File Offset: 0x003BFE1B
				public bool Equals(ReportManager.NoteStorage.NoteEntries.NoteEntryKey a, ReportManager.NoteStorage.NoteEntries.NoteEntryKey b)
				{
					return a.noteHash == b.noteHash && a.isPositive == b.isPositive;
				}

				// Token: 0x0600C6F5 RID: 50933 RVA: 0x003C1C3B File Offset: 0x003BFE3B
				public int GetHashCode(ReportManager.NoteStorage.NoteEntries.NoteEntryKey a)
				{
					return a.noteHash * (a.isPositive ? 1 : -1);
				}
			}
		}
	}
}
