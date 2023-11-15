using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000755 RID: 1877
public abstract class ColonyDiagnostic : ISim4000ms
{
	// Token: 0x06003407 RID: 13319 RVA: 0x00116DAC File Offset: 0x00114FAC
	public ColonyDiagnostic(int worldID, string name)
	{
		this.worldID = worldID;
		this.name = name;
		this.id = base.GetType().Name;
		this.IsWorldModuleInterior = ClusterManager.Instance.GetWorld(worldID).IsModuleInterior;
		this.colors = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color>();
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.DuplicantThreatening, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Bad, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Warning, Constants.NEGATIVE_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Concern, Constants.WARNING_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Tutorial, Constants.NEUTRAL_COLOR);
		this.colors.Add(ColonyDiagnostic.DiagnosticResult.Opinion.Good, Constants.POSITIVE_COLOR);
		SimAndRenderScheduler.instance.Add(this, true);
	}

	// Token: 0x170003B8 RID: 952
	// (get) Token: 0x06003408 RID: 13320 RVA: 0x00116ED6 File Offset: 0x001150D6
	// (set) Token: 0x06003409 RID: 13321 RVA: 0x00116EDE File Offset: 0x001150DE
	public int worldID { get; protected set; }

	// Token: 0x170003B9 RID: 953
	// (get) Token: 0x0600340A RID: 13322 RVA: 0x00116EE7 File Offset: 0x001150E7
	// (set) Token: 0x0600340B RID: 13323 RVA: 0x00116EEF File Offset: 0x001150EF
	public bool IsWorldModuleInterior { get; private set; }

	// Token: 0x0600340C RID: 13324 RVA: 0x00116EF8 File Offset: 0x001150F8
	public virtual string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_ALL_VERSIONS;
	}

	// Token: 0x0600340D RID: 13325 RVA: 0x00116EFF File Offset: 0x001150FF
	public void OnCleanUp()
	{
		SimAndRenderScheduler.instance.Remove(this);
	}

	// Token: 0x0600340E RID: 13326 RVA: 0x00116F0C File Offset: 0x0011510C
	public void Sim4000ms(float dt)
	{
		this.SetResult(ColonyDiagnosticUtility.IgnoreFirstUpdate ? ColonyDiagnosticUtility.NoDataResult : this.Evaluate());
	}

	// Token: 0x0600340F RID: 13327 RVA: 0x00116F28 File Offset: 0x00115128
	public DiagnosticCriterion[] GetCriteria()
	{
		DiagnosticCriterion[] array = new DiagnosticCriterion[this.criteria.Values.Count];
		this.criteria.Values.CopyTo(array, 0);
		return array;
	}

	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06003410 RID: 13328 RVA: 0x00116F5E File Offset: 0x0011515E
	// (set) Token: 0x06003411 RID: 13329 RVA: 0x00116F66 File Offset: 0x00115166
	public ColonyDiagnostic.DiagnosticResult LatestResult
	{
		get
		{
			return this.latestResult;
		}
		private set
		{
			this.latestResult = value;
		}
	}

	// Token: 0x06003412 RID: 13330 RVA: 0x00116F6F File Offset: 0x0011516F
	public virtual string GetAverageValueString()
	{
		if (this.tracker != null)
		{
			return this.tracker.FormatValueString(Mathf.Round(this.tracker.GetAverageValue(this.trackerSampleCountSeconds)));
		}
		return "";
	}

	// Token: 0x06003413 RID: 13331 RVA: 0x00116FA0 File Offset: 0x001151A0
	public virtual string GetCurrentValueString()
	{
		return "";
	}

	// Token: 0x06003414 RID: 13332 RVA: 0x00116FA7 File Offset: 0x001151A7
	protected void AddCriterion(string id, DiagnosticCriterion criterion)
	{
		if (!this.criteria.ContainsKey(id))
		{
			criterion.SetID(id);
			this.criteria.Add(id, criterion);
		}
	}

	// Token: 0x06003415 RID: 13333 RVA: 0x00116FCC File Offset: 0x001151CC
	public virtual ColonyDiagnostic.DiagnosticResult Evaluate()
	{
		ColonyDiagnostic.DiagnosticResult diagnosticResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, "", null);
		bool flag = false;
		foreach (KeyValuePair<string, DiagnosticCriterion> keyValuePair in this.criteria)
		{
			if (ColonyDiagnosticUtility.Instance.IsCriteriaEnabled(this.worldID, this.id, keyValuePair.Key))
			{
				ColonyDiagnostic.DiagnosticResult diagnosticResult2 = keyValuePair.Value.Evaluate();
				if (diagnosticResult2.opinion < diagnosticResult.opinion || (!flag && diagnosticResult2.opinion == ColonyDiagnostic.DiagnosticResult.Opinion.Normal))
				{
					flag = true;
					diagnosticResult.opinion = diagnosticResult2.opinion;
					diagnosticResult.Message = diagnosticResult2.Message;
					diagnosticResult.clickThroughTarget = diagnosticResult2.clickThroughTarget;
				}
			}
		}
		return diagnosticResult;
	}

	// Token: 0x06003416 RID: 13334 RVA: 0x001170A4 File Offset: 0x001152A4
	public void SetResult(ColonyDiagnostic.DiagnosticResult result)
	{
		this.LatestResult = result;
	}

	// Token: 0x170003BB RID: 955
	// (get) Token: 0x06003417 RID: 13335 RVA: 0x001170AD File Offset: 0x001152AD
	protected string NO_MINIONS
	{
		get
		{
			return this.IsWorldModuleInterior ? UI.COLONY_DIAGNOSTICS.NO_MINIONS_ROCKET : UI.COLONY_DIAGNOSTICS.NO_MINIONS_PLANETOID;
		}
	}

	// Token: 0x04001F96 RID: 8086
	public string name;

	// Token: 0x04001F97 RID: 8087
	public string id;

	// Token: 0x04001F99 RID: 8089
	public string icon = "icon_errand_operate";

	// Token: 0x04001F9A RID: 8090
	private Dictionary<string, DiagnosticCriterion> criteria = new Dictionary<string, DiagnosticCriterion>();

	// Token: 0x04001F9B RID: 8091
	public ColonyDiagnostic.PresentationSetting presentationSetting;

	// Token: 0x04001F9C RID: 8092
	private ColonyDiagnostic.DiagnosticResult latestResult = new ColonyDiagnostic.DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion.Normal, UI.COLONY_DIAGNOSTICS.NO_DATA, null);

	// Token: 0x04001F9D RID: 8093
	public Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color> colors = new Dictionary<ColonyDiagnostic.DiagnosticResult.Opinion, Color>();

	// Token: 0x04001F9E RID: 8094
	public Tracker tracker;

	// Token: 0x04001F9F RID: 8095
	protected float trackerSampleCountSeconds = 4f;

	// Token: 0x020014E8 RID: 5352
	public enum PresentationSetting
	{
		// Token: 0x040066CA RID: 26314
		AverageValue,
		// Token: 0x040066CB RID: 26315
		CurrentValue
	}

	// Token: 0x020014E9 RID: 5353
	public struct DiagnosticResult
	{
		// Token: 0x06008637 RID: 34359 RVA: 0x0030825F File Offset: 0x0030645F
		public DiagnosticResult(ColonyDiagnostic.DiagnosticResult.Opinion opinion, string message, global::Tuple<Vector3, GameObject> clickThroughTarget = null)
		{
			this.message = message;
			this.opinion = opinion;
			this.clickThroughTarget = null;
		}

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x06008639 RID: 34361 RVA: 0x0030827F File Offset: 0x0030647F
		// (set) Token: 0x06008638 RID: 34360 RVA: 0x00308276 File Offset: 0x00306476
		public string Message
		{
			get
			{
				return this.message;
			}
			set
			{
				this.message = value;
			}
		}

		// Token: 0x0600863A RID: 34362 RVA: 0x00308288 File Offset: 0x00306488
		public string GetFormattedMessage()
		{
			switch (this.opinion)
			{
			case ColonyDiagnostic.DiagnosticResult.Opinion.Bad:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.NEGATIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Warning:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.NEGATIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Concern:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.WARNING_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Suggestion:
			case ColonyDiagnostic.DiagnosticResult.Opinion.Normal:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.WHITE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			case ColonyDiagnostic.DiagnosticResult.Opinion.Good:
				return string.Concat(new string[]
				{
					"<color=",
					Constants.POSITIVE_COLOR_STR,
					">",
					this.message,
					"</color>"
				});
			}
			return this.message;
		}

		// Token: 0x040066CC RID: 26316
		public ColonyDiagnostic.DiagnosticResult.Opinion opinion;

		// Token: 0x040066CD RID: 26317
		public global::Tuple<Vector3, GameObject> clickThroughTarget;

		// Token: 0x040066CE RID: 26318
		private string message;

		// Token: 0x0200216E RID: 8558
		public enum Opinion
		{
			// Token: 0x040095C7 RID: 38343
			Unset,
			// Token: 0x040095C8 RID: 38344
			DuplicantThreatening,
			// Token: 0x040095C9 RID: 38345
			Bad,
			// Token: 0x040095CA RID: 38346
			Warning,
			// Token: 0x040095CB RID: 38347
			Concern,
			// Token: 0x040095CC RID: 38348
			Suggestion,
			// Token: 0x040095CD RID: 38349
			Tutorial,
			// Token: 0x040095CE RID: 38350
			Normal,
			// Token: 0x040095CF RID: 38351
			Good
		}
	}
}
