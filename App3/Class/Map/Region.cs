using System;

namespace App3.Class.Map
{
	internal class Region : Layer
	{
		public Region() : base(0, LayerType.Region, "cache")
		{
		}

		public Region(int pRegionId) : base(pRegionId, LayerType.Region, "cache")
		{
		}

		public override void CreateTable()
		{
			if (DataBase.TableExist(base.TableName))
			{
				return;
			}
			string text = "CREATE TABLE {0} AS ";
			if (base.RegionId == 0)
			{
				text += string.Format("(SELECT * FROM regions2map);", new object[0]);
			}
			else
			{
				text += string.Format("(SELECT * FROM regions2map WHERE num = {0});", base.RegionId);
			}
			DataBase.RunCommand(string.Format(text, base.TableName));
			base.CreateTable();
		}

		public override void UpdateTable()
		{
			if (this.need_update)
			{
				if (!DataBase.TableExist(base.TableName))
				{
					this.CreateTable();
					return;
				}
				string format = "DELETE FROM {0}";
				string text = "INSERT INTO {0} ";
				if (base.RegionId == 0)
				{
					text += string.Format("(SELECT * FROM regions2map);", new object[0]);
				}
				else
				{
					text += string.Format("(SELECT * FROM regions2map WHERE num = {0});", base.RegionId);
				}
				DataBase.RunCommand(string.Format(format, base.TableName));
				DataBase.RunCommand(string.Format(text, base.TableName));
				base.UpdateTable();
			}
		}
	}
}
