using System;

namespace App3.Controls
{
	public class Header
	{
		private string _headerText;

		private int _startLocation;

		private string _headerTextInsteadOfTime;

		private DateTime _time;

		public string HeaderText
		{
			get
			{
				return this._headerText;
			}
			set
			{
				this._headerText = value;
			}
		}

		public int StartLocation
		{
			get
			{
				return this._startLocation;
			}
			set
			{
				this._startLocation = value;
			}
		}

		public string HeaderTextInsteadOfTime
		{
			get
			{
				return this._headerTextInsteadOfTime;
			}
			set
			{
				this._headerTextInsteadOfTime = value;
			}
		}

		public DateTime Time
		{
			get
			{
				return this._time;
			}
			set
			{
				this._time = value;
			}
		}
	}
}
