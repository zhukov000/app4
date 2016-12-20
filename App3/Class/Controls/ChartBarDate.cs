using System;
using System.Drawing;

namespace App3.Controls
{
	public class ChartBarDate
	{
		internal class Location
		{
			private Point _right = new Point(0, 0);

			private Point _left = new Point(0, 0);

			public Point Right
			{
				get
				{
					return this._right;
				}
				set
				{
					this._right = value;
				}
			}

			public Point Left
			{
				get
				{
					return this._left;
				}
				set
				{
					this._left = value;
				}
			}
		}

		private DateTime _startValue;

		private DateTime _endValue;

		private Color _color;

		private Color _hoverColor;

		private string _text;

		private object _value;

		private int _rowIndex;

		private ChartBarDate.Location _topLocation = new ChartBarDate.Location();

		private ChartBarDate.Location _bottomLocation = new ChartBarDate.Location();

		private bool _hideFromMouseMove;

		public DateTime StartValue
		{
			get
			{
				return this._startValue;
			}
			set
			{
				this._startValue = value;
			}
		}

		public DateTime EndValue
		{
			get
			{
				return this._endValue;
			}
			set
			{
				this._endValue = value;
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

		public string Text
		{
			get
			{
				return this._text;
			}
			set
			{
				this._text = value;
			}
		}

		public object Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this._value = value;
			}
		}

		public int RowIndex
		{
			get
			{
				return this._rowIndex;
			}
			set
			{
				this._rowIndex = value;
			}
		}

		public bool HideFromMouseMove
		{
			get
			{
				return this._hideFromMouseMove;
			}
			set
			{
				this._hideFromMouseMove = value;
			}
		}

		internal ChartBarDate.Location TopLocation
		{
			get
			{
				return this._topLocation;
			}
			set
			{
				this._topLocation = value;
			}
		}

		internal ChartBarDate.Location BottomLocation
		{
			get
			{
				return this._bottomLocation;
			}
			set
			{
				this._bottomLocation = value;
			}
		}
	}
}
