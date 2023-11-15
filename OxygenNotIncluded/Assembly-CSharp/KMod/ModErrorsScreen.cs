using System;
using System.Collections.Generic;
using UnityEngine;

namespace KMod
{
	// Token: 0x02000D81 RID: 3457
	public class ModErrorsScreen : KScreen
	{
		// Token: 0x06006BF3 RID: 27635 RVA: 0x002A482C File Offset: 0x002A2A2C
		public static bool ShowErrors(List<Event> events)
		{
			if (Global.Instance.modManager.events.Count == 0)
			{
				return false;
			}
			GameObject parent = GameObject.Find("Canvas");
			ModErrorsScreen modErrorsScreen = Util.KInstantiateUI<ModErrorsScreen>(Global.Instance.modErrorsPrefab, parent, false);
			modErrorsScreen.Initialize(events);
			modErrorsScreen.gameObject.SetActive(true);
			return true;
		}

		// Token: 0x06006BF4 RID: 27636 RVA: 0x002A4880 File Offset: 0x002A2A80
		private void Initialize(List<Event> events)
		{
			foreach (Event @event in events)
			{
				HierarchyReferences hierarchyReferences = Util.KInstantiateUI<HierarchyReferences>(this.entryPrefab, this.entryParent.gameObject, true);
				LocText reference = hierarchyReferences.GetReference<LocText>("Title");
				LocText reference2 = hierarchyReferences.GetReference<LocText>("Description");
				KButton reference3 = hierarchyReferences.GetReference<KButton>("Details");
				string text;
				string toolTip;
				Event.GetUIStrings(@event.event_type, out text, out toolTip);
				reference.text = text;
				reference.GetComponent<ToolTip>().toolTip = toolTip;
				reference2.text = @event.mod.title;
				ToolTip component = reference2.GetComponent<ToolTip>();
				if (component != null)
				{
					ToolTip toolTip2 = component;
					Label mod = @event.mod;
					toolTip2.toolTip = mod.ToString();
				}
				reference3.isInteractable = false;
				Mod mod2 = Global.Instance.modManager.FindMod(@event.mod);
				if (mod2 != null)
				{
					if (component != null && !string.IsNullOrEmpty(mod2.description))
					{
						component.toolTip = mod2.description;
					}
					if (mod2.on_managed != null)
					{
						reference3.onClick += mod2.on_managed;
						reference3.isInteractable = true;
					}
				}
			}
		}

		// Token: 0x06006BF5 RID: 27637 RVA: 0x002A49E0 File Offset: 0x002A2BE0
		protected override void OnActivate()
		{
			base.OnActivate();
			this.closeButtonTitle.onClick += this.Deactivate;
			this.closeButton.onClick += this.Deactivate;
		}

		// Token: 0x04004EEF RID: 20207
		[SerializeField]
		private KButton closeButtonTitle;

		// Token: 0x04004EF0 RID: 20208
		[SerializeField]
		private KButton closeButton;

		// Token: 0x04004EF1 RID: 20209
		[SerializeField]
		private GameObject entryPrefab;

		// Token: 0x04004EF2 RID: 20210
		[SerializeField]
		private Transform entryParent;
	}
}
