using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    
   
    [Serializable]
    public class rectangle: GeneralPolygonBase
    {

        public rectangle(int my_ID, int upperleftx, int upperleftxy) :base(  my_ID,   upperleftx,   upperleftxy)
        {
            List<Point> handles = new List<Point>();
            handles.Add(new Point(100, 0));
            handles.Add(new Point(200, 0));
            handles.Add(new Point(200, 50));
            handles.Add(new Point(100, 50));
            addallpoints(handles);

         

        }


        //public override void draw(Graphics g, int selected_shape_id)
        //{

            
        //    if (selected_shape_id == this.GetID())
        //    {

        //      //  _handles.ToList().Add(new Point(0,0));
        //        g.FillPolygon(new SolidBrush(SystemColorPolicy.SelectedSateColor), connection_points.ToArray());
        //    }
        //    else
        //    {
        //       // _handles.ToList().Add(new Point(0, 0));
        //        g.FillPolygon(new SolidBrush(color), connection_points.ToArray());
        //    }
        //    //yek dayer sadde ham dore node mikeshim
        //  //  g.FillPolygon(Pens.Black, connection_points.ToArray());

        //    //mypoint connection_points
        //    foreach (Point h in connection_points)
        //    {
        //        g.FillEllipse(Brushes.Black, h.X - 2, h.Y - 2, 4, 4);

        //    }
        //    draw_icon_and_textarea(g); //!!!!!CHILD must call this to draw text and icons after customized draw
          

        //}


    }
}
