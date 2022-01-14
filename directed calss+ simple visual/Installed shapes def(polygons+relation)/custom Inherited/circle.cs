using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    [Serializable]
    public class circle : GeneralPolygonBase
    {
        public circle(int my_ID, int upperleftx, int upperleftxy, int circle_radious) : base(my_ID, upperleftx, upperleftxy)
        {
            _circle_radious = circle_radious;
           
            List<Point> handles = new List<Point>();
            handles.Add(new Point(0, -50));
            handles.Add(new Point(50, 0));
            handles.Add(new Point(0, 50));
            handles.Add(new Point(-50, 0));
            addallpoints(handles);
        }


        int _circle_radious;
        public override bool is_point_on_shape(int clickx, int clicky)
        {
            int isa = (Locationx - clickx) * (Locationx - clickx) + (Locationy - clicky) * (Locationy - clicky);
            if (isa < _circle_radious * _circle_radious) { return true; }
            return false;
        }

        public override void draw(Graphics g, int selected_shape_id)
        {

           // base.draw(g,selected_shape_id);
            Rectangle rect = get_rectangle_of_circle();
            //is node selected
            if (selected_shape_id == this.GetID())
            {
                g.FillEllipse(new SolidBrush(SystemColorPolicy.SelectedSateColor), rect);
            }
            else
            {

                g.FillEllipse(new SolidBrush(color), rect);
            }
            //yek dayer sadde ham dore node mikeshim
            g.DrawEllipse(Pens.Black, rect);

            //mypoint connection_points
            foreach (Point h in connection_points)
            {
                g.FillEllipse(Brushes.Black, h.X-2, h.Y-2, 4, 4);
                
            }
            draw_icon_and_textarea(g); //!!!!!CHILD must call this to draw text and icons after customized draw
            //draw image data on node if has

            //  resize_and_draw_image_if_has(g);

            //draw string data on node if has

            //   draw_text.draw_text_if_has(g, base.textareaWidth, textareaHeight, this.Data, Locationx, Locationy);

            // Draw icon of attachment file on center of polygon-----

            //  int y = Locationy + (_handles[2].Y - _handles[0].Y) / 2;//;
            //  drawicon.read_and_draw_icon(g, Locationx, Locationy, iconWidth, iconHeight, filePath);

        }



        public override void scaleup_shape(int scalefactor, bool movelocation_for_screenmove/*true for overla scene zoom*/)
        {
            
            base.scaleup_shape(scalefactor, movelocation_for_screenmove);
            _circle_radious *= scalefactor;


        }

        public override void scaledown_shape(int scalefactor, bool movelocation_for_screenmove/*true for overla scene zoom*/)
        {

            base.scaledown_shape(scalefactor, movelocation_for_screenmove);
            _circle_radious /= scalefactor;


        }

        public override void collapse()
        {
             base.collapse();
            {
                _circle_radious =(_handles[2].Y - _handles[0].Y)/2;


            }
        }
        public override void expand()
        {
             base.expand();
            {
                _circle_radious =( _handles[2].Y - _handles[0].Y)/2;


            }
        }
        Rectangle get_rectangle_of_circle()
        {

            int rectx = Locationx - _circle_radious;
            int recty = Locationy - _circle_radious;

            return new Rectangle(rectx, recty, _circle_radious * 2, _circle_radious * 2);
        }
 
         
    }
     
}
