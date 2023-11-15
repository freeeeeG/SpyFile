using System;
using System.Collections.Generic;

// Token: 0x0200041D RID: 1053
public class ClosestEdibleSensor : Sensor
{
	// Token: 0x06001640 RID: 5696 RVA: 0x00074AC7 File Offset: 0x00072CC7
	public ClosestEdibleSensor(Sensors sensors) : base(sensors)
	{
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x00074AD0 File Offset: 0x00072CD0
	public override void Update()
	{
		HashSet<Tag> forbiddenTagSet = base.GetComponent<ConsumableConsumer>().forbiddenTagSet;
		Pickupable pickupable = Game.Instance.fetchManager.FindEdibleFetchTarget(base.GetComponent<Storage>(), forbiddenTagSet, GameTags.Edible);
		bool flag = this.edibleInReachButNotPermitted;
		Edible x = null;
		bool flag2 = false;
		if (pickupable != null)
		{
			x = pickupable.GetComponent<Edible>();
			flag2 = true;
			flag = false;
		}
		else
		{
			flag = (Game.Instance.fetchManager.FindEdibleFetchTarget(base.GetComponent<Storage>(), new HashSet<Tag>(), GameTags.Edible) != null);
		}
		if (x != this.edible || this.hasEdible != flag2)
		{
			this.edible = x;
			this.hasEdible = flag2;
			this.edibleInReachButNotPermitted = flag;
			base.Trigger(86328522, this.edible);
		}
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x00074B8D File Offset: 0x00072D8D
	public Edible GetEdible()
	{
		return this.edible;
	}

	// Token: 0x04000C67 RID: 3175
	private Edible edible;

	// Token: 0x04000C68 RID: 3176
	private bool hasEdible;

	// Token: 0x04000C69 RID: 3177
	public bool edibleInReachButNotPermitted;
}
