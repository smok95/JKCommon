/*
Copyright 2017, Kim Jongkook d0nzs00n@gmail.com

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
associated documentation files (the "Software"), to deal in the Software without restriction, 
including without limitation the rights to use, copy, modify, merge, publish, distribute, 
sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or 
substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT 
NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JKCommon
{
    /// <summary>
    /// 외괵선이 포함된 Label
    /// 
    /// </summary>
    public class OutlineLabel:Label
    {
        #region Variables

        /// <summary>
        /// 글자 외곽선 색상 
        /// http://stackoverflow.com/questions/19842722/setting-a-font-with-outline-color-in-c-sharp
        /// </summary>
        public Color OutlineColor { get; set; }

        /// <summary>
        /// 외곽선 두께
        /// </summary>
        public float OutlineWidth { get; set; }

        #endregion
        public OutlineLabel()
        {
            OutlineColor = Color.White;
            OutlineWidth = 2;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
            using (GraphicsPath gp = new GraphicsPath())
            {
                using (Pen outline = new Pen(OutlineColor, OutlineWidth))
                {
                    outline.LineJoin = LineJoin.Round;

                    using (StringFormat sf = new StringFormat())
                    {
                        using (Brush br = new SolidBrush(ForeColor))
                        {
                            gp.AddString(Text, Font.FontFamily, (int)Font.Style, Font.Size, ClientRectangle, sf);
                            e.Graphics.ScaleTransform(1.3f, 1.35f);
                            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                            e.Graphics.DrawPath(outline, gp);
                            e.Graphics.FillPath(br, gp);
                        }
                    }
                }
            }
        }
    }
}
