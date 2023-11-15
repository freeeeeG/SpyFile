using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009EC RID: 2540
public class StatusItem : Resource
{
	// Token: 0x06004BC3 RID: 19395 RVA: 0x001A94C4 File Offset: 0x001A76C4
	private StatusItem(string id, string composed_prefix) : base(id, Strings.Get(composed_prefix + ".NAME"))
	{
		this.composedPrefix = composed_prefix;
		this.tooltipText = Strings.Get(composed_prefix + ".TOOLTIP");
	}

	// Token: 0x06004BC4 RID: 19396 RVA: 0x001A9518 File Offset: 0x001A7718
	public StatusItem(string id, string prefix, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, bool showWorldIcon = true, int status_overlays = 129022, Func<string, object, string> resolve_string_callback = null) : this(id, "STRINGS." + prefix + ".STATUSITEMS." + id.ToUpper())
	{
		switch (icon_type)
		{
		case StatusItem.IconType.Info:
			icon = "dash";
			break;
		case StatusItem.IconType.Exclamation:
			icon = "status_item_exclamation";
			break;
		}
		this.iconName = icon;
		this.notificationType = notification_type;
		this.sprite = Assets.GetTintedSprite(icon);
		this.iconType = icon_type;
		this.allowMultiples = allow_multiples;
		this.render_overlay = render_overlay;
		this.showShowWorldIcon = showWorldIcon;
		this.status_overlays = status_overlays;
		this.resolveStringCallback = resolve_string_callback;
		if (this.sprite == null)
		{
			global::Debug.LogWarning("Status item '" + id + "' references a missing icon: " + icon);
		}
	}

	// Token: 0x06004BC5 RID: 19397 RVA: 0x001A95D0 File Offset: 0x001A77D0
	public StatusItem(string id, string name, string tooltip, string icon, StatusItem.IconType icon_type, NotificationType notification_type, bool allow_multiples, HashedString render_overlay, int status_overlays = 129022, bool showWorldIcon = true, Func<string, object, string> resolve_string_callback = null) : base(id, name)
	{
		switch (icon_type)
		{
		case StatusItem.IconType.Info:
			icon = "dash";
			break;
		case StatusItem.IconType.Exclamation:
			icon = "status_item_exclamation";
			break;
		}
		this.iconName = icon;
		this.notificationType = notification_type;
		this.sprite = Assets.GetTintedSprite(icon);
		this.tooltipText = tooltip;
		this.iconType = icon_type;
		this.allowMultiples = allow_multiples;
		this.render_overlay = render_overlay;
		this.status_overlays = status_overlays;
		this.showShowWorldIcon = showWorldIcon;
		this.resolveStringCallback = resolve_string_callback;
		if (this.sprite == null)
		{
			global::Debug.LogWarning("Status item '" + id + "' references a missing icon: " + icon);
		}
	}

	// Token: 0x06004BC6 RID: 19398 RVA: 0x001A9684 File Offset: 0x001A7884
	public void AddNotification(string sound_path = null, string notification_text = null, string notification_tooltip = null)
	{
		this.shouldNotify = true;
		if (sound_path == null)
		{
			if (this.notificationType == NotificationType.Bad)
			{
				this.soundPath = "Warning";
			}
			else
			{
				this.soundPath = "Notification";
			}
		}
		else
		{
			this.soundPath = sound_path;
		}
		if (notification_text != null)
		{
			this.notificationText = notification_text;
		}
		else
		{
			DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
			this.notificationText = Strings.Get(this.composedPrefix + ".NOTIFICATION_NAME");
		}
		if (notification_tooltip != null)
		{
			this.notificationTooltipText = notification_tooltip;
			return;
		}
		DebugUtil.Assert(this.composedPrefix != null, "When adding a notification, either set the status prefix or specify strings!");
		this.notificationTooltipText = Strings.Get(this.composedPrefix + ".NOTIFICATION_TOOLTIP");
	}

	// Token: 0x06004BC7 RID: 19399 RVA: 0x001A9742 File Offset: 0x001A7942
	public virtual string GetName(object data)
	{
		return this.ResolveString(this.Name, data);
	}

	// Token: 0x06004BC8 RID: 19400 RVA: 0x001A9751 File Offset: 0x001A7951
	public virtual string GetTooltip(object data)
	{
		return this.ResolveTooltip(this.tooltipText, data);
	}

	// Token: 0x06004BC9 RID: 19401 RVA: 0x001A9760 File Offset: 0x001A7960
	private string ResolveString(string str, object data)
	{
		if (this.resolveStringCallback != null && (data != null || this.resolveStringCallback_shouldStillCallIfDataIsNull))
		{
			return this.resolveStringCallback(str, data);
		}
		return str;
	}

	// Token: 0x06004BCA RID: 19402 RVA: 0x001A9784 File Offset: 0x001A7984
	private string ResolveTooltip(string str, object data)
	{
		if (data != null)
		{
			if (this.resolveTooltipCallback != null)
			{
				return this.resolveTooltipCallback(str, data);
			}
			if (this.resolveStringCallback != null)
			{
				return this.resolveStringCallback(str, data);
			}
		}
		else
		{
			if (this.resolveStringCallback_shouldStillCallIfDataIsNull && this.resolveStringCallback != null)
			{
				return this.resolveStringCallback(str, data);
			}
			if (this.resolveTooltipCallback_shouldStillCallIfDataIsNull && this.resolveTooltipCallback != null)
			{
				return this.resolveTooltipCallback(str, data);
			}
		}
		return str;
	}

	// Token: 0x06004BCB RID: 19403 RVA: 0x001A97FD File Offset: 0x001A79FD
	public bool ShouldShowIcon()
	{
		return this.iconType == StatusItem.IconType.Custom && this.showShowWorldIcon;
	}

	// Token: 0x06004BCC RID: 19404 RVA: 0x001A9810 File Offset: 0x001A7A10
	public virtual void ShowToolTip(ToolTip tooltip_widget, object data, TextStyleSetting property_style)
	{
		tooltip_widget.ClearMultiStringTooltip();
		string tooltip = this.GetTooltip(data);
		tooltip_widget.AddMultiStringTooltip(tooltip, property_style);
	}

	// Token: 0x06004BCD RID: 19405 RVA: 0x001A9833 File Offset: 0x001A7A33
	public void SetIcon(Image image, object data)
	{
		if (this.sprite == null)
		{
			return;
		}
		image.color = this.sprite.color;
		image.sprite = this.sprite.sprite;
	}

	// Token: 0x06004BCE RID: 19406 RVA: 0x001A9860 File Offset: 0x001A7A60
	public bool UseConditionalCallback(HashedString overlay, Transform transform)
	{
		return overlay != OverlayModes.None.ID && this.conditionalOverlayCallback != null && this.conditionalOverlayCallback(overlay, transform);
	}

	// Token: 0x06004BCF RID: 19407 RVA: 0x001A9886 File Offset: 0x001A7A86
	public StatusItem SetResolveStringCallback(Func<string, object, string> cb)
	{
		this.resolveStringCallback = cb;
		return this;
	}

	// Token: 0x06004BD0 RID: 19408 RVA: 0x001A9890 File Offset: 0x001A7A90
	public void OnClick(object data)
	{
		if (this.statusItemClickCallback != null)
		{
			this.statusItemClickCallback(data);
		}
	}

	// Token: 0x06004BD1 RID: 19409 RVA: 0x001A98A8 File Offset: 0x001A7AA8
	public static StatusItem.StatusItemOverlays GetStatusItemOverlayBySimViewMode(HashedString mode)
	{
		StatusItem.StatusItemOverlays result;
		if (!StatusItem.overlayBitfieldMap.TryGetValue(mode, out result))
		{
			string str = "ViewMode ";
			HashedString hashedString = mode;
			global::Debug.LogWarning(str + hashedString.ToString() + " has no StatusItemOverlay value");
			result = StatusItem.StatusItemOverlays.None;
		}
		return result;
	}

	// Token: 0x0400316D RID: 12653
	public string tooltipText;

	// Token: 0x0400316E RID: 12654
	public string notificationText;

	// Token: 0x0400316F RID: 12655
	public string notificationTooltipText;

	// Token: 0x04003170 RID: 12656
	public string soundPath;

	// Token: 0x04003171 RID: 12657
	public string iconName;

	// Token: 0x04003172 RID: 12658
	public TintedSprite sprite;

	// Token: 0x04003173 RID: 12659
	public bool shouldNotify;

	// Token: 0x04003174 RID: 12660
	public StatusItem.IconType iconType;

	// Token: 0x04003175 RID: 12661
	public NotificationType notificationType;

	// Token: 0x04003176 RID: 12662
	public Notification.ClickCallback notificationClickCallback;

	// Token: 0x04003177 RID: 12663
	public Func<string, object, string> resolveStringCallback;

	// Token: 0x04003178 RID: 12664
	public Func<string, object, string> resolveTooltipCallback;

	// Token: 0x04003179 RID: 12665
	public bool resolveStringCallback_shouldStillCallIfDataIsNull;

	// Token: 0x0400317A RID: 12666
	public bool resolveTooltipCallback_shouldStillCallIfDataIsNull;

	// Token: 0x0400317B RID: 12667
	public bool allowMultiples;

	// Token: 0x0400317C RID: 12668
	public Func<HashedString, object, bool> conditionalOverlayCallback;

	// Token: 0x0400317D RID: 12669
	public HashedString render_overlay;

	// Token: 0x0400317E RID: 12670
	public int status_overlays;

	// Token: 0x0400317F RID: 12671
	public Action<object> statusItemClickCallback;

	// Token: 0x04003180 RID: 12672
	private string composedPrefix;

	// Token: 0x04003181 RID: 12673
	private bool showShowWorldIcon = true;

	// Token: 0x04003182 RID: 12674
	public const int ALL_OVERLAYS = 129022;

	// Token: 0x04003183 RID: 12675
	private static Dictionary<HashedString, StatusItem.StatusItemOverlays> overlayBitfieldMap = new Dictionary<HashedString, StatusItem.StatusItemOverlays>
	{
		{
			OverlayModes.None.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.Power.ID,
			StatusItem.StatusItemOverlays.PowerMap
		},
		{
			OverlayModes.Temperature.ID,
			StatusItem.StatusItemOverlays.Temperature
		},
		{
			OverlayModes.ThermalConductivity.ID,
			StatusItem.StatusItemOverlays.ThermalComfort
		},
		{
			OverlayModes.Light.ID,
			StatusItem.StatusItemOverlays.Light
		},
		{
			OverlayModes.LiquidConduits.ID,
			StatusItem.StatusItemOverlays.LiquidPlumbing
		},
		{
			OverlayModes.GasConduits.ID,
			StatusItem.StatusItemOverlays.GasPlumbing
		},
		{
			OverlayModes.SolidConveyor.ID,
			StatusItem.StatusItemOverlays.Conveyor
		},
		{
			OverlayModes.Decor.ID,
			StatusItem.StatusItemOverlays.Decor
		},
		{
			OverlayModes.Disease.ID,
			StatusItem.StatusItemOverlays.Pathogens
		},
		{
			OverlayModes.Crop.ID,
			StatusItem.StatusItemOverlays.Farming
		},
		{
			OverlayModes.Rooms.ID,
			StatusItem.StatusItemOverlays.Rooms
		},
		{
			OverlayModes.Suit.ID,
			StatusItem.StatusItemOverlays.Suits
		},
		{
			OverlayModes.Logic.ID,
			StatusItem.StatusItemOverlays.Logic
		},
		{
			OverlayModes.Oxygen.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.TileMode.ID,
			StatusItem.StatusItemOverlays.None
		},
		{
			OverlayModes.Radiation.ID,
			StatusItem.StatusItemOverlays.Radiation
		}
	};

	// Token: 0x0200186E RID: 6254
	public enum IconType
	{
		// Token: 0x040071ED RID: 29165
		Info,
		// Token: 0x040071EE RID: 29166
		Exclamation,
		// Token: 0x040071EF RID: 29167
		Custom
	}

	// Token: 0x0200186F RID: 6255
	[Flags]
	public enum StatusItemOverlays
	{
		// Token: 0x040071F1 RID: 29169
		None = 2,
		// Token: 0x040071F2 RID: 29170
		PowerMap = 4,
		// Token: 0x040071F3 RID: 29171
		Temperature = 8,
		// Token: 0x040071F4 RID: 29172
		ThermalComfort = 16,
		// Token: 0x040071F5 RID: 29173
		Light = 32,
		// Token: 0x040071F6 RID: 29174
		LiquidPlumbing = 64,
		// Token: 0x040071F7 RID: 29175
		GasPlumbing = 128,
		// Token: 0x040071F8 RID: 29176
		Decor = 256,
		// Token: 0x040071F9 RID: 29177
		Pathogens = 512,
		// Token: 0x040071FA RID: 29178
		Farming = 1024,
		// Token: 0x040071FB RID: 29179
		Rooms = 4096,
		// Token: 0x040071FC RID: 29180
		Suits = 8192,
		// Token: 0x040071FD RID: 29181
		Logic = 16384,
		// Token: 0x040071FE RID: 29182
		Conveyor = 32768,
		// Token: 0x040071FF RID: 29183
		Radiation = 65536
	}
}
