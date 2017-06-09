using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class.Static
{
    public static class GeoStyles
    {
        public static SharpMap.Styles.VectorStyle ThemeObject(SharpMap.Data.FeatureDataRow row)
        {
            SharpMap.Styles.VectorStyle style = new SharpMap.Styles.VectorStyle();
            style.PointColor = new SolidBrush(System.Drawing.ColorTranslator.FromHtml(row["color"].ToString()));
            return style;
        }

        public static SharpMap.Styles.VectorStyle ThemeBuilding(SharpMap.Data.FeatureDataRow row)
        {
            SharpMap.Styles.VectorStyle style = new SharpMap.Styles.VectorStyle();
            style.EnableOutline = true;
            style.Outline = new Pen(Color.Black);
            style.Fill = new SolidBrush(Color.DarkGray);
            return style;
        }

        public static SharpMap.Styles.VectorStyle ThemeRegion(SharpMap.Data.FeatureDataRow row)
        {
            SharpMap.Styles.VectorStyle style = new SharpMap.Styles.VectorStyle();
            style.EnableOutline = true;
            style.Outline = new Pen(Color.FromArgb(167, 232, 232), 2);
            style.Outline.DashStyle = DashStyle.Dot;
            style.Fill = new SolidBrush(System.Drawing.ColorTranslator.FromHtml(row["color"].ToString()));
            return style;
        }

        public static SharpMap.Styles.LabelStyle ThemeLabel(SharpMap.Data.FeatureDataRow row)
        {
            string s = row["name"].ToString();
            if (s.Length > 10)
            {
                row["name"] += "_"; // Fix Bug with reder label
            }
            SharpMap.Styles.LabelStyle Style = new SharpMap.Styles.LabelStyle
            {
                ForeColor = Color.DarkBlue,
                Font = new Font(FontFamily.GenericSansSerif, 11),
                HorizontalAlignment = SharpMap.Styles.LabelStyle.HorizontalAlignmentEnum.Center,
                VerticalAlignment = SharpMap.Styles.LabelStyle.VerticalAlignmentEnum.Middle,
                CollisionBuffer = new SizeF(5, 5),
                Halo = new Pen(Color.Azure, 2),
                CollisionDetection = true,
                MaxVisible = 90,
                MinVisible = 30
            };
            return Style;
        }
    }
}
