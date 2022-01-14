using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{

    
    


    [Serializable]
    public class rhombus : GeneralPolygonBase 
    {
       public rhombus(int my_ID, int upperleftx, int upperleftxy) :base(  my_ID,   upperleftx,   upperleftxy){


           

        List<Point> handles = new List<Point>();
        handles.Add(new Point(0, -50));
                handles.Add(new Point(50, 0));
                handles.Add(new Point(0, 50));
                handles.Add(new Point(-50, 0));
            addallpoints(handles);

        }

    }

     
}
