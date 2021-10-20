using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LauncherSchool.CustomElements
{
    class CustomPanel : Panel
    {
        private Color colorTop = Color.White;
        private Color colorBottom = Color.White;
        private int alpha1 = 255;
        private int alpha2 = 255;
        private float angle = 0;

        public Color ColorTop
        {
            get { return colorTop; }
            set
            {
                colorTop = value;
            }
        }

        public Color ColorBottom
        {
            get { return colorBottom; }
            set
            {
                colorBottom = value;
            }
        }

        public float Angle
        {
            get { return angle; }
            set
            {
                angle = value;
            }
        }
        public int Alpha1
        {
            get { return alpha1; }
            set
            {
                if(value > 255)
                    MessageBox.Show("Максимальный размер - 255");
                else
                    alpha1 = value;
            }
        }


        public int Alpha2
        {
            get { return alpha2; }
            set
            {
                if (value > 255)
                    MessageBox.Show("Максимальный размер - 255");
                else
                    alpha2 = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush linearGradient = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(this.Alpha1,this.ColorTop), Color.FromArgb(this.Alpha2, this.ColorBottom), this.Angle);
            Graphics g = e.Graphics;
            g.FillRectangle(linearGradient, this.ClientRectangle);
            base.OnPaint(e);
        }
    }
}
