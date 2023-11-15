using System;
using Klei.AI;
using KSerialization;
using UnityEngine;

// Token: 0x02000704 RID: 1796
[SerializationConfig(MemberSerialization.OptIn)]
public class RoverModifiers : Modifiers, ISaveLoadable
{
	// Token: 0x06003165 RID: 12645 RVA: 0x00106D84 File Offset: 0x00104F84
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributes.Add(Db.Get().Attributes.Construction);
		this.attributes.Add(Db.Get().Attributes.Digging);
		this.attributes.Add(Db.Get().Attributes.Strength);
	}

	// Token: 0x06003166 RID: 12646 RVA: 0x00106DE8 File Offset: 0x00104FE8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (base.GetComponent<ChoreConsumer>() != null)
		{
			base.Subscribe<RoverModifiers>(-1988963660, RoverModifiers.OnBeginChoreDelegate);
			Vector3 position = base.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			base.transform.SetPosition(position);
			base.gameObject.layer = LayerMask.NameToLayer("Default");
			this.SetupDependentAttribute(Db.Get().Attributes.CarryAmount, Db.Get().AttributeConverters.CarryAmountFromStrength);
		}
	}

	// Token: 0x06003167 RID: 12647 RVA: 0x00106E7C File Offset: 0x0010507C
	private void SetupDependentAttribute(Klei.AI.Attribute targetAttribute, AttributeConverter attributeConverter)
	{
		Klei.AI.Attribute attribute = attributeConverter.attribute;
		AttributeInstance attributeInstance = attribute.Lookup(this);
		AttributeModifier target_modifier = new AttributeModifier(targetAttribute.Id, attributeConverter.Lookup(this).Evaluate(), attribute.Name, false, false, false);
		this.GetAttributes().Add(target_modifier);
		attributeInstance.OnDirty = (System.Action)Delegate.Combine(attributeInstance.OnDirty, new System.Action(delegate()
		{
			target_modifier.SetValue(attributeConverter.Lookup(this).Evaluate());
		}));
	}

	// Token: 0x06003168 RID: 12648 RVA: 0x00106F10 File Offset: 0x00105110
	private void OnBeginChore(object data)
	{
		Storage component = base.GetComponent<Storage>();
		if (component != null)
		{
			component.DropAll(false, false, default(Vector3), true, null);
		}
	}

	// Token: 0x04001DA0 RID: 7584
	private static readonly EventSystem.IntraObjectHandler<RoverModifiers> OnBeginChoreDelegate = new EventSystem.IntraObjectHandler<RoverModifiers>(delegate(RoverModifiers component, object data)
	{
		component.OnBeginChore(data);
	});
}
