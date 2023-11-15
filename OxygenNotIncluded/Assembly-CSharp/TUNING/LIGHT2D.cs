using System;
using UnityEngine;

namespace TUNING
{
	// Token: 0x02000D99 RID: 3481
	public class LIGHT2D
	{
		// Token: 0x0400500F RID: 20495
		public const int SUNLIGHT_MAX_DEFAULT = 80000;

		// Token: 0x04005010 RID: 20496
		public static readonly Color LIGHT_BLUE = new Color(0.38f, 0.61f, 1f, 1f);

		// Token: 0x04005011 RID: 20497
		public static readonly Color LIGHT_PURPLE = new Color(0.9f, 0.4f, 0.74f, 1f);

		// Token: 0x04005012 RID: 20498
		public static readonly Color LIGHT_YELLOW = new Color(0.57f, 0.55f, 0.44f, 1f);

		// Token: 0x04005013 RID: 20499
		public static readonly Color LIGHT_OVERLAY = new Color(0.56f, 0.56f, 0.56f, 1f);

		// Token: 0x04005014 RID: 20500
		public static readonly Vector2 DEFAULT_DIRECTION = new Vector2(0f, -1f);

		// Token: 0x04005015 RID: 20501
		public const int FLOORLAMP_LUX = 1000;

		// Token: 0x04005016 RID: 20502
		public const float FLOORLAMP_RANGE = 4f;

		// Token: 0x04005017 RID: 20503
		public const float FLOORLAMP_ANGLE = 0f;

		// Token: 0x04005018 RID: 20504
		public const global::LightShape FLOORLAMP_SHAPE = global::LightShape.Circle;

		// Token: 0x04005019 RID: 20505
		public static readonly Color FLOORLAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x0400501A RID: 20506
		public static readonly Color FLOORLAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x0400501B RID: 20507
		public static readonly Vector2 FLOORLAMP_OFFSET = new Vector2(0.05f, 1.5f);

		// Token: 0x0400501C RID: 20508
		public static readonly Vector2 FLOORLAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x0400501D RID: 20509
		public const float CEILINGLIGHT_RANGE = 8f;

		// Token: 0x0400501E RID: 20510
		public const float CEILINGLIGHT_ANGLE = 2.6f;

		// Token: 0x0400501F RID: 20511
		public const global::LightShape CEILINGLIGHT_SHAPE = global::LightShape.Cone;

		// Token: 0x04005020 RID: 20512
		public static readonly Color CEILINGLIGHT_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005021 RID: 20513
		public static readonly Color CEILINGLIGHT_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005022 RID: 20514
		public static readonly Vector2 CEILINGLIGHT_OFFSET = new Vector2(0.05f, 0.65f);

		// Token: 0x04005023 RID: 20515
		public static readonly Vector2 CEILINGLIGHT_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005024 RID: 20516
		public const int CEILINGLIGHT_LUX = 1800;

		// Token: 0x04005025 RID: 20517
		public const int SUNLAMP_LUX = 40000;

		// Token: 0x04005026 RID: 20518
		public const float SUNLAMP_RANGE = 16f;

		// Token: 0x04005027 RID: 20519
		public const float SUNLAMP_ANGLE = 5.2f;

		// Token: 0x04005028 RID: 20520
		public const global::LightShape SUNLAMP_SHAPE = global::LightShape.Cone;

		// Token: 0x04005029 RID: 20521
		public static readonly Color SUNLAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x0400502A RID: 20522
		public static readonly Color SUNLAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x0400502B RID: 20523
		public static readonly Vector2 SUNLAMP_OFFSET = new Vector2(0f, 3.5f);

		// Token: 0x0400502C RID: 20524
		public static readonly Vector2 SUNLAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x0400502D RID: 20525
		public static readonly Color LIGHT_PREVIEW_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x0400502E RID: 20526
		public const float HEADQUARTERS_RANGE = 5f;

		// Token: 0x0400502F RID: 20527
		public const global::LightShape HEADQUARTERS_SHAPE = global::LightShape.Circle;

		// Token: 0x04005030 RID: 20528
		public static readonly Color HEADQUARTERS_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005031 RID: 20529
		public static readonly Color HEADQUARTERS_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005032 RID: 20530
		public static readonly Vector2 HEADQUARTERS_OFFSET = new Vector2(0.5f, 3f);

		// Token: 0x04005033 RID: 20531
		public static readonly Vector2 EXOBASE_HEADQUARTERS_OFFSET = new Vector2(0f, 2.5f);

		// Token: 0x04005034 RID: 20532
		public const float ENGINE_RANGE = 10f;

		// Token: 0x04005035 RID: 20533
		public const global::LightShape ENGINE_SHAPE = global::LightShape.Circle;

		// Token: 0x04005036 RID: 20534
		public const int ENGINE_LUX = 80000;

		// Token: 0x04005037 RID: 20535
		public const float WALLLIGHT_RANGE = 4f;

		// Token: 0x04005038 RID: 20536
		public const float WALLLIGHT_ANGLE = 0f;

		// Token: 0x04005039 RID: 20537
		public const global::LightShape WALLLIGHT_SHAPE = global::LightShape.Circle;

		// Token: 0x0400503A RID: 20538
		public static readonly Color WALLLIGHT_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x0400503B RID: 20539
		public static readonly Color WALLLIGHT_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x0400503C RID: 20540
		public static readonly Vector2 WALLLIGHT_OFFSET = new Vector2(0f, 0.5f);

		// Token: 0x0400503D RID: 20541
		public static readonly Vector2 WALLLIGHT_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x0400503E RID: 20542
		public const float LIGHTBUG_RANGE = 5f;

		// Token: 0x0400503F RID: 20543
		public const float LIGHTBUG_ANGLE = 0f;

		// Token: 0x04005040 RID: 20544
		public const global::LightShape LIGHTBUG_SHAPE = global::LightShape.Circle;

		// Token: 0x04005041 RID: 20545
		public const int LIGHTBUG_LUX = 1800;

		// Token: 0x04005042 RID: 20546
		public static readonly Color LIGHTBUG_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x04005043 RID: 20547
		public static readonly Color LIGHTBUG_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005044 RID: 20548
		public static readonly Color LIGHTBUG_COLOR_ORANGE = new Color(0.5686275f, 0.48235294f, 0.4392157f, 1f);

		// Token: 0x04005045 RID: 20549
		public static readonly Color LIGHTBUG_COLOR_PURPLE = new Color(0.49019608f, 0.4392157f, 0.5686275f, 1f);

		// Token: 0x04005046 RID: 20550
		public static readonly Color LIGHTBUG_COLOR_PINK = new Color(0.5686275f, 0.4392157f, 0.5686275f, 1f);

		// Token: 0x04005047 RID: 20551
		public static readonly Color LIGHTBUG_COLOR_BLUE = new Color(0.4392157f, 0.4862745f, 0.5686275f, 1f);

		// Token: 0x04005048 RID: 20552
		public static readonly Color LIGHTBUG_COLOR_CRYSTAL = new Color(0.5137255f, 0.6666667f, 0.6666667f, 1f);

		// Token: 0x04005049 RID: 20553
		public static readonly Color LIGHTBUG_COLOR_GREEN = new Color(0.43137255f, 1f, 0.53333336f, 1f);

		// Token: 0x0400504A RID: 20554
		public const int MAJORFOSSILDIGSITE_LAMP_LUX = 1000;

		// Token: 0x0400504B RID: 20555
		public const float MAJORFOSSILDIGSITE_LAMP_RANGE = 3f;

		// Token: 0x0400504C RID: 20556
		public static readonly Vector2 MAJORFOSSILDIGSITE_LAMP_OFFSET = new Vector2(-0.15f, 2.35f);

		// Token: 0x0400504D RID: 20557
		public static readonly Vector2 LIGHTBUG_OFFSET = new Vector2(0.05f, 0.25f);

		// Token: 0x0400504E RID: 20558
		public static readonly Vector2 LIGHTBUG_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x0400504F RID: 20559
		public const int PLASMALAMP_LUX = 666;

		// Token: 0x04005050 RID: 20560
		public const float PLASMALAMP_RANGE = 2f;

		// Token: 0x04005051 RID: 20561
		public const float PLASMALAMP_ANGLE = 0f;

		// Token: 0x04005052 RID: 20562
		public const global::LightShape PLASMALAMP_SHAPE = global::LightShape.Circle;

		// Token: 0x04005053 RID: 20563
		public static readonly Color PLASMALAMP_COLOR = LIGHT2D.LIGHT_PURPLE;

		// Token: 0x04005054 RID: 20564
		public static readonly Color PLASMALAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005055 RID: 20565
		public static readonly Vector2 PLASMALAMP_OFFSET = new Vector2(0.05f, 0.5f);

		// Token: 0x04005056 RID: 20566
		public static readonly Vector2 PLASMALAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x04005057 RID: 20567
		public const int MAGMALAMP_LUX = 666;

		// Token: 0x04005058 RID: 20568
		public const float MAGMALAMP_RANGE = 2f;

		// Token: 0x04005059 RID: 20569
		public const float MAGMALAMP_ANGLE = 0f;

		// Token: 0x0400505A RID: 20570
		public const global::LightShape MAGMALAMP_SHAPE = global::LightShape.Cone;

		// Token: 0x0400505B RID: 20571
		public static readonly Color MAGMALAMP_COLOR = LIGHT2D.LIGHT_YELLOW;

		// Token: 0x0400505C RID: 20572
		public static readonly Color MAGMALAMP_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x0400505D RID: 20573
		public static readonly Vector2 MAGMALAMP_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x0400505E RID: 20574
		public static readonly Vector2 MAGMALAMP_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;

		// Token: 0x0400505F RID: 20575
		public const int BIOLUMROCK_LUX = 666;

		// Token: 0x04005060 RID: 20576
		public const float BIOLUMROCK_RANGE = 2f;

		// Token: 0x04005061 RID: 20577
		public const float BIOLUMROCK_ANGLE = 0f;

		// Token: 0x04005062 RID: 20578
		public const global::LightShape BIOLUMROCK_SHAPE = global::LightShape.Cone;

		// Token: 0x04005063 RID: 20579
		public static readonly Color BIOLUMROCK_COLOR = LIGHT2D.LIGHT_BLUE;

		// Token: 0x04005064 RID: 20580
		public static readonly Color BIOLUMROCK_OVERLAYCOLOR = LIGHT2D.LIGHT_OVERLAY;

		// Token: 0x04005065 RID: 20581
		public static readonly Vector2 BIOLUMROCK_OFFSET = new Vector2(0.05f, 0.33f);

		// Token: 0x04005066 RID: 20582
		public static readonly Vector2 BIOLUMROCK_DIRECTION = LIGHT2D.DEFAULT_DIRECTION;
	}
}
