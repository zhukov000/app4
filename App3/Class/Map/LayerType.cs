using System;

namespace App3.Class.Map
{
	public class LayerType
	{
		public string Value
		{
			get;
			set;
		}

		public static LayerType BigPoly
		{
			get
			{
				return new LayerType("bigpoly");
			}
		}

		public static LayerType Build
		{
			get
			{
				return new LayerType("build");
			}
		}

		public static LayerType BuildBorder
		{
			get
			{
				return new LayerType("buildborder");
			}
		}

		public static LayerType HighWay
		{
			get
			{
				return new LayerType("highway");
			}
		}

		public static LayerType MidPoly
		{
			get
			{
				return new LayerType("midpoly");
			}
		}

		public static LayerType Object
		{
			get
			{
				return new LayerType("object");
			}
		}

		public static LayerType Place
		{
			get
			{
				return new LayerType("place");
			}
		}

		public static LayerType Point
		{
			get
			{
				return new LayerType("point");
			}
		}

		public static LayerType Polygon
		{
			get
			{
				return new LayerType("polygon");
			}
		}

		public static LayerType Region
		{
			get
			{
				return new LayerType("region");
			}
		}

		public static LayerType Road
		{
			get
			{
				return new LayerType("road");
			}
		}

		public static LayerType SmlPoly
		{
			get
			{
				return new LayerType("smlpoly");
			}
		}

		public static LayerType AllRegion
		{
			get
			{
				return new LayerType("regions");
			}
		}

		private LayerType(string value)
		{
			this.Value = value;
		}

		public override string ToString()
		{
			return this.Value;
		}

		public static implicit operator string(LayerType t)
		{
			return t.ToString();
		}
	}
}
