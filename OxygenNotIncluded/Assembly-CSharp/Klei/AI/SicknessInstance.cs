using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DE4 RID: 3556
	[SerializationConfig(MemberSerialization.OptIn)]
	public class SicknessInstance : ModifierInstance<Sickness>, ISaveLoadable
	{
		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06006D6A RID: 28010 RVA: 0x002B21CC File Offset: 0x002B03CC
		public Sickness Sickness
		{
			get
			{
				return this.modifier;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06006D6B RID: 28011 RVA: 0x002B21D4 File Offset: 0x002B03D4
		public float TotalCureSpeedMultiplier
		{
			get
			{
				AttributeInstance attributeInstance = Db.Get().Attributes.DiseaseCureSpeed.Lookup(this.smi.master.gameObject);
				AttributeInstance attributeInstance2 = this.modifier.cureSpeedBase.Lookup(this.smi.master.gameObject);
				float num = 1f;
				if (attributeInstance != null)
				{
					num *= attributeInstance.GetTotalValue();
				}
				if (attributeInstance2 != null)
				{
					num *= attributeInstance2.GetTotalValue();
				}
				return num;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06006D6C RID: 28012 RVA: 0x002B2248 File Offset: 0x002B0448
		public bool IsDoctored
		{
			get
			{
				if (base.gameObject == null)
				{
					return false;
				}
				AttributeInstance attributeInstance = Db.Get().Attributes.DoctoredLevel.Lookup(base.gameObject);
				return attributeInstance != null && attributeInstance.GetTotalValue() > 0f;
			}
		}

		// Token: 0x06006D6D RID: 28013 RVA: 0x002B2293 File Offset: 0x002B0493
		public SicknessInstance(GameObject game_object, Sickness disease) : base(game_object, disease)
		{
		}

		// Token: 0x06006D6E RID: 28014 RVA: 0x002B229D File Offset: 0x002B049D
		[OnDeserialized]
		private void OnDeserialized()
		{
			this.InitializeAndStart();
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06006D6F RID: 28015 RVA: 0x002B22A5 File Offset: 0x002B04A5
		// (set) Token: 0x06006D70 RID: 28016 RVA: 0x002B22AD File Offset: 0x002B04AD
		public SicknessExposureInfo ExposureInfo
		{
			get
			{
				return this.exposureInfo;
			}
			set
			{
				this.exposureInfo = value;
				this.InitializeAndStart();
			}
		}

		// Token: 0x06006D71 RID: 28017 RVA: 0x002B22BC File Offset: 0x002B04BC
		private void InitializeAndStart()
		{
			Sickness disease = this.modifier;
			Func<List<Notification>, object, string> tooltip = delegate(List<Notification> notificationList, object data)
			{
				string text = "";
				for (int i = 0; i < notificationList.Count; i++)
				{
					Notification notification = notificationList[i];
					string arg = (string)notification.tooltipData;
					text += string.Format(DUPLICANTS.DISEASES.NOTIFICATION_TOOLTIP, notification.NotifierName, disease.Name, arg);
					if (i < notificationList.Count - 1)
					{
						text += "\n";
					}
				}
				return text;
			};
			string name = disease.Name;
			string title = name;
			NotificationType type = (disease.severity <= Sickness.Severity.Minor) ? NotificationType.BadMinor : NotificationType.Bad;
			object sourceInfo = this.exposureInfo.sourceInfo;
			this.notification = new Notification(title, type, tooltip, sourceInfo, true, 0f, null, null, null, true, false, false);
			this.statusItem = new StatusItem(disease.Id, disease.Name, DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.TEMPLATE, "", (disease.severity <= Sickness.Severity.Minor) ? StatusItem.IconType.Info : StatusItem.IconType.Exclamation, (disease.severity <= Sickness.Severity.Minor) ? NotificationType.BadMinor : NotificationType.Bad, false, OverlayModes.None.ID, 129022, true, null);
			this.statusItem.resolveTooltipCallback = new Func<string, object, string>(this.ResolveString);
			if (this.smi != null)
			{
				this.smi.StopSM("refresh");
			}
			this.smi = new SicknessInstance.StatesInstance(this);
			this.smi.StartSM();
		}

		// Token: 0x06006D72 RID: 28018 RVA: 0x002B23D4 File Offset: 0x002B05D4
		private string ResolveString(string str, object data)
		{
			if (this.smi == null)
			{
				global::Debug.LogWarning("Attempting to resolve string when smi is null");
				return str;
			}
			KSelectable component = base.gameObject.GetComponent<KSelectable>();
			str = str.Replace("{Descriptor}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DESCRIPTOR, Strings.Get("STRINGS.DUPLICANTS.DISEASES.SEVERITY." + this.modifier.severity.ToString().ToUpper()), Strings.Get("STRINGS.DUPLICANTS.DISEASES.TYPE." + this.modifier.sicknessType.ToString().ToUpper())));
			str = str.Replace("{Infectee}", component.GetProperName());
			str = str.Replace("{InfectionSource}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.INFECTION_SOURCE, this.exposureInfo.sourceInfo));
			if (this.modifier.severity <= Sickness.Severity.Minor)
			{
				str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1", false)));
			}
			else if (this.modifier.severity == Sickness.Severity.Major)
			{
				str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1", false)));
				if (!this.IsDoctored)
				{
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.BEDREST);
				}
				else
				{
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTORED);
				}
			}
			else if (this.modifier.severity >= Sickness.Severity.Critical)
			{
				if (!this.IsDoctored)
				{
					str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.FATALITY, GameUtil.GetFormattedCycles(this.GetFatalityTimeRemaining(), "F1", false)));
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTOR_REQUIRED);
				}
				else
				{
					str = str.Replace("{Duration}", string.Format(DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DURATION, GameUtil.GetFormattedCycles(this.GetInfectedTimeRemaining(), "F1", false)));
					str = str.Replace("{Doctor}", DUPLICANTS.DISEASES.STATUS_ITEM_TOOLTIP.DOCTORED);
				}
			}
			List<Descriptor> symptoms = this.modifier.GetSymptoms();
			string text = "";
			foreach (Descriptor descriptor in symptoms)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				text = text + "    • " + descriptor.text;
			}
			str = str.Replace("{Symptoms}", text);
			str = Regex.Replace(str, "{[^}]*}", "");
			return str;
		}

		// Token: 0x06006D73 RID: 28019 RVA: 0x002B269C File Offset: 0x002B089C
		public float GetInfectedTimeRemaining()
		{
			return this.modifier.SicknessDuration * (1f - this.smi.sm.percentRecovered.Get(this.smi)) / this.TotalCureSpeedMultiplier;
		}

		// Token: 0x06006D74 RID: 28020 RVA: 0x002B26D2 File Offset: 0x002B08D2
		public float GetFatalityTimeRemaining()
		{
			return this.modifier.fatalityDuration * (1f - this.smi.sm.percentDied.Get(this.smi));
		}

		// Token: 0x06006D75 RID: 28021 RVA: 0x002B2701 File Offset: 0x002B0901
		public float GetPercentCured()
		{
			if (this.smi == null)
			{
				return 0f;
			}
			return this.smi.sm.percentRecovered.Get(this.smi);
		}

		// Token: 0x06006D76 RID: 28022 RVA: 0x002B272C File Offset: 0x002B092C
		public void SetPercentCured(float pct)
		{
			this.smi.sm.percentRecovered.Set(pct, this.smi, false);
		}

		// Token: 0x06006D77 RID: 28023 RVA: 0x002B274C File Offset: 0x002B094C
		public void Cure()
		{
			this.smi.Cure();
		}

		// Token: 0x06006D78 RID: 28024 RVA: 0x002B2759 File Offset: 0x002B0959
		public override void OnCleanUp()
		{
			if (this.smi != null)
			{
				this.smi.StopSM("DiseaseInstance.OnCleanUp");
				this.smi = null;
			}
		}

		// Token: 0x06006D79 RID: 28025 RVA: 0x002B277A File Offset: 0x002B097A
		public StatusItem GetStatusItem()
		{
			return this.statusItem;
		}

		// Token: 0x06006D7A RID: 28026 RVA: 0x002B2782 File Offset: 0x002B0982
		public List<Descriptor> GetDescriptors()
		{
			return this.modifier.GetSicknessSourceDescriptors();
		}

		// Token: 0x04005212 RID: 21010
		[Serialize]
		private SicknessExposureInfo exposureInfo;

		// Token: 0x04005213 RID: 21011
		private SicknessInstance.StatesInstance smi;

		// Token: 0x04005214 RID: 21012
		private StatusItem statusItem;

		// Token: 0x04005215 RID: 21013
		private Notification notification;

		// Token: 0x02001F65 RID: 8037
		private struct CureInfo
		{
			// Token: 0x04008E03 RID: 36355
			public string name;

			// Token: 0x04008E04 RID: 36356
			public float multiplier;
		}

		// Token: 0x02001F66 RID: 8038
		public class StatesInstance : GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.GameInstance
		{
			// Token: 0x0600A24F RID: 41551 RVA: 0x00363C76 File Offset: 0x00361E76
			public StatesInstance(SicknessInstance master) : base(master)
			{
			}

			// Token: 0x0600A250 RID: 41552 RVA: 0x00363C80 File Offset: 0x00361E80
			public void UpdateProgress(float dt)
			{
				float delta_value = dt * base.master.TotalCureSpeedMultiplier / base.master.modifier.SicknessDuration;
				base.sm.percentRecovered.Delta(delta_value, base.smi);
				if (base.master.modifier.fatalityDuration > 0f)
				{
					if (!base.master.IsDoctored)
					{
						float delta_value2 = dt / base.master.modifier.fatalityDuration;
						base.sm.percentDied.Delta(delta_value2, base.smi);
						return;
					}
					base.sm.percentDied.Set(0f, base.smi, false);
				}
			}

			// Token: 0x0600A251 RID: 41553 RVA: 0x00363D34 File Offset: 0x00361F34
			public void Infect()
			{
				Sickness modifier = base.master.modifier;
				this.componentData = modifier.Infect(base.gameObject, base.master, base.master.exposureInfo);
				if (PopFXManager.Instance != null)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Negative, string.Format(DUPLICANTS.DISEASES.INFECTED_POPUP, modifier.Name), base.gameObject.transform, 1.5f, true);
				}
			}

			// Token: 0x0600A252 RID: 41554 RVA: 0x00363DB8 File Offset: 0x00361FB8
			public void Cure()
			{
				Sickness modifier = base.master.modifier;
				base.gameObject.GetComponent<Modifiers>().sicknesses.Cure(modifier);
				modifier.Cure(base.gameObject, this.componentData);
				if (PopFXManager.Instance != null)
				{
					PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Plus, string.Format(DUPLICANTS.DISEASES.CURED_POPUP, modifier.Name), base.gameObject.transform, 1.5f, true);
				}
				if (!string.IsNullOrEmpty(modifier.recoveryEffect))
				{
					Effects component = base.gameObject.GetComponent<Effects>();
					if (component)
					{
						component.Add(modifier.recoveryEffect, true);
					}
				}
			}

			// Token: 0x0600A253 RID: 41555 RVA: 0x00363E71 File Offset: 0x00362071
			public SicknessExposureInfo GetExposureInfo()
			{
				return base.master.ExposureInfo;
			}

			// Token: 0x04008E05 RID: 36357
			private object[] componentData;
		}

		// Token: 0x02001F67 RID: 8039
		public class States : GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance>
		{
			// Token: 0x0600A254 RID: 41556 RVA: 0x00363E80 File Offset: 0x00362080
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.infected;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.infected.Enter("Infect", delegate(SicknessInstance.StatesInstance smi)
				{
					smi.Infect();
				}).DoNotification((SicknessInstance.StatesInstance smi) => smi.master.notification).Update("UpdateProgress", delegate(SicknessInstance.StatesInstance smi, float dt)
				{
					smi.UpdateProgress(dt);
				}, UpdateRate.SIM_200ms, false).ToggleStatusItem((SicknessInstance.StatesInstance smi) => smi.master.GetStatusItem(), (SicknessInstance.StatesInstance smi) => smi).ParamTransition<float>(this.percentRecovered, this.cured, GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.IsGTOne).ParamTransition<float>(this.percentDied, this.fatality_pre, GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.IsGTOne);
				this.cured.Enter("Cure", delegate(SicknessInstance.StatesInstance smi)
				{
					smi.master.Cure();
				});
				this.fatality_pre.Update("DeathByDisease", delegate(SicknessInstance.StatesInstance smi, float dt)
				{
					DeathMonitor.Instance smi2 = smi.master.gameObject.GetSMI<DeathMonitor.Instance>();
					if (smi2 != null)
					{
						smi2.Kill(Db.Get().Deaths.FatalDisease);
						smi.GoTo(this.fatality);
					}
				}, UpdateRate.SIM_200ms, false);
				this.fatality.DoNothing();
			}

			// Token: 0x04008E06 RID: 36358
			public StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.FloatParameter percentRecovered;

			// Token: 0x04008E07 RID: 36359
			public StateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.FloatParameter percentDied;

			// Token: 0x04008E08 RID: 36360
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State infected;

			// Token: 0x04008E09 RID: 36361
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State cured;

			// Token: 0x04008E0A RID: 36362
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State fatality_pre;

			// Token: 0x04008E0B RID: 36363
			public GameStateMachine<SicknessInstance.States, SicknessInstance.StatesInstance, SicknessInstance, object>.State fatality;
		}
	}
}
