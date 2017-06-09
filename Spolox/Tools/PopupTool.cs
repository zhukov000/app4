using App3.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Tools
{
    public class PopupTool : CustomMapTool
    {
        protected override void OnMapControlChanging(CancelEventArgs cea)
        {
            base.OnMapControlChanging(cea);
            if (cea.Cancel) return;

            if (MapControl == null) return;
            MapControl.MouseDown -= HandleMouseDown;
        }

        protected override void OnMapControlChanged(EventArgs e)
        {
            base.OnMapControlChanged(e);
            if (MapControl == null) return;
            MapControl.MouseDown += HandleMouseDown;
        }

        private void HandleMouseDown(GeoAPI.Geometries.Coordinate worldpos, MouseEventArgs imagepos)
        {
            if (!Enabled)
                return;

            MessageBox.Show(string.Format("You clicked at {0}!", worldpos));
        }
    }
}
