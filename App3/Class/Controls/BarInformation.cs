using System;
using System.Drawing;

namespace App3.Controls
{
	public class BarInformation
	{
		private string _rowText;

		private DateTime _fromTime;

		private DateTime _toTime;

		private Color _color;

		private Color _hoverColor;

		private int _index;

		public string RowText
		{
			get
			{
				return this._rowText;
			}
			set
			{
				this._rowText = value;
			}
		}

		public DateTime FromTime
		{
			get
			{
				return this._fromTime;
			}
			set
			{
				this._fromTime = value;
			}
		}

		public DateTime ToTime
		{
			get
			{
				return this._toTime;
			}
			set
			{
				this._toTime = value;
			}
		}

		public Color Color
		{
			get
			{
				return this._color;
			}
			set
			{
				this._color = value;
			}
		}

		public Color HoverColor
		{
			get
			{
				return this._hoverColor;
			}
			set
			{
				this._hoverColor = value;
			}
		}

		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		public BarInformation()
		{
		}

		public BarInformation(string rowText, DateTime fromTime, DateTime totime, Color color, Color hoverColor, int index)
		{
			this.RowText = rowText;
			this.FromTime = fromTime;
			this.ToTime = totime;
			this.Color = color;
			this.HoverColor = hoverColor;
			this.Index = index;
		}
	}
}
