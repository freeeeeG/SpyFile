using System;
using UnityEngine;

// Token: 0x02000651 RID: 1617
public class MeterController
{
	// Token: 0x170002B9 RID: 697
	// (get) Token: 0x06002A9B RID: 10907 RVA: 0x000E306F File Offset: 0x000E126F
	// (set) Token: 0x06002A9C RID: 10908 RVA: 0x000E3077 File Offset: 0x000E1277
	public KBatchedAnimController meterController { get; private set; }

	// Token: 0x06002A9D RID: 10909 RVA: 0x000E3080 File Offset: 0x000E1280
	public MeterController(KMonoBehaviour target, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, params string[] symbols_to_hide)
	{
		string[] array = new string[symbols_to_hide.Length + 1];
		Array.Copy(symbols_to_hide, array, symbols_to_hide.Length);
		array[array.Length - 1] = "meter_target";
		KBatchedAnimController component = target.GetComponent<KBatchedAnimController>();
		this.Initialize(component, "meter_target", "meter", front_back, user_specified_render_layer, Vector3.zero, array);
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x000E30D7 File Offset: 0x000E12D7
	public MeterController(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, params string[] symbols_to_hide)
	{
		this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, Vector3.zero, symbols_to_hide);
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x000E30F3 File Offset: 0x000E12F3
	public MeterController(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, Vector3 tracker_offset, params string[] symbols_to_hide)
	{
		this.Initialize(building_controller, meter_target, meter_animation, front_back, user_specified_render_layer, tracker_offset, symbols_to_hide);
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x000E310C File Offset: 0x000E130C
	private void Initialize(KAnimControllerBase building_controller, string meter_target, string meter_animation, Meter.Offset front_back, Grid.SceneLayer user_specified_render_layer, Vector3 tracker_offset, params string[] symbols_to_hide)
	{
		if (building_controller.HasAnimation(meter_animation + "_cb") && !GlobalAssets.Instance.colorSet.IsDefaultColorSet())
		{
			meter_animation += "_cb";
		}
		string name = building_controller.name + "." + meter_animation;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Assets.GetPrefab(MeterConfig.ID));
		gameObject.name = name;
		gameObject.SetActive(false);
		gameObject.transform.parent = building_controller.transform;
		this.gameObject = gameObject;
		gameObject.GetComponent<KPrefabID>().PrefabTag = new Tag(name);
		Vector3 position = building_controller.transform.GetPosition();
		switch (front_back)
		{
		case Meter.Offset.Infront:
			position.z -= 0.1f;
			break;
		case Meter.Offset.Behind:
			position.z += 0.1f;
			break;
		case Meter.Offset.UserSpecified:
			position.z = Grid.GetLayerZ(user_specified_render_layer);
			break;
		}
		gameObject.transform.SetPosition(position);
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		component.AnimFiles = new KAnimFile[]
		{
			building_controller.AnimFiles[0]
		};
		component.initialAnim = meter_animation;
		component.fgLayer = Grid.SceneLayer.NoLayer;
		component.initialMode = KAnim.PlayMode.Paused;
		component.isMovable = true;
		component.FlipX = building_controller.FlipX;
		component.FlipY = building_controller.FlipY;
		if (Meter.Offset.UserSpecified == front_back)
		{
			component.sceneLayer = user_specified_render_layer;
		}
		this.meterController = component;
		KBatchedAnimTracker component2 = gameObject.GetComponent<KBatchedAnimTracker>();
		component2.offset = tracker_offset;
		component2.symbol = new HashedString(meter_target);
		gameObject.SetActive(true);
		building_controller.SetSymbolVisiblity(meter_target, false);
		if (symbols_to_hide != null)
		{
			for (int i = 0; i < symbols_to_hide.Length; i++)
			{
				building_controller.SetSymbolVisiblity(symbols_to_hide[i], false);
			}
		}
		this.link = new KAnimLink(building_controller, component);
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x000E32DC File Offset: 0x000E14DC
	public MeterController(KAnimControllerBase building_controller, KBatchedAnimController meter_controller, params string[] symbol_names)
	{
		if (meter_controller == null)
		{
			return;
		}
		this.meterController = meter_controller;
		this.link = new KAnimLink(building_controller, meter_controller);
		for (int i = 0; i < symbol_names.Length; i++)
		{
			building_controller.SetSymbolVisiblity(symbol_names[i], false);
		}
		this.meterController.GetComponent<KBatchedAnimTracker>().symbol = new HashedString(symbol_names[0]);
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x000E3342 File Offset: 0x000E1542
	public void SetPositionPercent(float percent_full)
	{
		if (this.meterController == null)
		{
			return;
		}
		this.meterController.SetPositionPercent(percent_full);
	}

	// Token: 0x06002AA3 RID: 10915 RVA: 0x000E335F File Offset: 0x000E155F
	public void SetSymbolTint(KAnimHashedString symbol, Color32 colour)
	{
		if (this.meterController != null)
		{
			this.meterController.SetSymbolTint(symbol, colour);
		}
	}

	// Token: 0x06002AA4 RID: 10916 RVA: 0x000E3381 File Offset: 0x000E1581
	public void SetRotation(float rot)
	{
		if (this.meterController == null)
		{
			return;
		}
		this.meterController.Rotation = rot;
	}

	// Token: 0x040018E5 RID: 6373
	public GameObject gameObject;

	// Token: 0x040018E6 RID: 6374
	private KAnimLink link;
}
