using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    class UtilityandGeometryMath
    {
       
        public  T DeepObjectCopy<T>(T item)
        {
            //memory based deep clone
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Seek(0, SeekOrigin.Begin);
            T result = (T)formatter.Deserialize(stream);
            stream.Close();

            if (Object.ReferenceEquals(item, result))
            {
                MessageBox.Show("References are the same.");
            }

            return result;
        }


        public double GetDistanceBetween2Ppoint(int x1, int y1, int x2, int y2)
        {
            //???double bashe behtare ke nakardim
            return (int)Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
        }

        public List<Point> calculate2parralelmiddlepointofsameline(int distance, bool up, Point sourcehandle, Point targethandle)
        {
            checked
            {
                List<Point> online = calculate2middlepointofsameline(sourcehandle, targethandle);
                Point paramiddle1 = new Point(online[0].X, online[0].Y);
                Point paramiddle2 = new Point(online[1].X, online[1].Y);
                //???check nashod ke same nabashe ya null, ya hadeaghal distance ....
                List<Point> result = new List<Point>();


                int dr = distance;
                double soorat = (double)(targethandle.Y - sourcehandle.Y);
                double makhraj = (double)(targethandle.X - sourcehandle.X);
                //aval samte sourcehandle node baad samte targethandle ra bar migardone
                double temp = soorat / makhraj;
                double slope = -1 / temp;//prepreculdar slop
                                         // slope = Math.Abs(slope);           
                double dx = dr / Math.Sqrt((slope * slope) + 1);
                double dy = dx * slope;
                dx = Math.Abs(dx);
                dy = Math.Abs(dy);



                if (double.IsInfinity(temp))
                {
                    //line is vertical
                    if (up)
                    {
                        paramiddle1.X -= (int)dr;
                        //paramiddle1.Y -= (int)dr; //no change
                        paramiddle2.X -= (int)dr;
                        //paramiddle2.Y -= (int)dri; //y no change
                    }
                    else
                    {
                        paramiddle1.X += (int)dr;
                        // paramiddle1.Y += (int)dy;//no change
                        paramiddle2.X += (int)dr;
                        //paramiddle2.Y += (int)dy;//no change
                    }

                }
                else
                {

                    if (temp > 0)
                    {

                        if (up)
                        {
                            paramiddle1.X -= (int)dx;
                            paramiddle1.Y += (int)dy;
                            paramiddle2.X -= (int)dx;
                            paramiddle2.Y += (int)dy;
                        }
                        else
                        {



                            paramiddle1.X += (int)dx;
                            paramiddle1.Y -= (int)dy;
                            paramiddle2.X += (int)dx;
                            paramiddle2.Y -= (int)dy;


                        }
                    }
                    else if (temp < 0)
                    {

                        if (up)
                        {
                            paramiddle1.X -= (int)dx;
                            paramiddle1.Y -= (int)dy;
                            paramiddle2.X -= (int)dx;
                            paramiddle2.Y -= (int)dy;

                        }
                        else
                        {

                            paramiddle1.X += (int)dx;
                            paramiddle1.Y += (int)dy;
                            paramiddle2.X += (int)dx;
                            paramiddle2.Y += (int)dy;

                        }


                    }
                    else if (temp == 0)
                    {
                        //horizal line
                        if (up)
                        {
                            //paramiddle1.X -= (int)dx; //nochange
                            paramiddle1.Y += (int)dr;
                            //paramiddle2.X -= (int)dx;
                            paramiddle2.Y += (int)dr;
                        }
                        else
                        {
                            //paramiddle1.X += (int)dx; //nochange
                            paramiddle1.Y -= (int)dr;
                            //paramiddle2.X += (int)dx; //nochange
                            paramiddle2.Y -= (int)dr;
                        }

                    }
                }

                 

                result.Add(paramiddle1);
                result.Add(paramiddle2);
                return result;
            }

        }

        internal Image GetIconOfFile(string filePath)
        {
            //draw icon in center
            if (filePath != null && filePath != "")
            {

                //string extention = (new FileInfo(filePath)).Extension;
                try
                {
                    System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(filePath);
                 
                    
                    return icon.ToBitmap();
                    
                }
                catch
                {
                    return null;
                    
                }


            }
            else
            {
                return null;
            }
        }

        public Size resizeimage(Image image)
        {  
            //if height biashtare 100 kari mione ke 100 besh ba hefze aspect ration
            //age height kamtare 100 khodesh rasm mikoneh
            float height = image.Height;
            float width = image.Width;
            if (image.Height > globalsetting.minimum_height())
            {
                float ration = image.Height / globalsetting.minimum_height();
                height = (int)(image.Height / ration);
                width = image.Width / ration;

            }
            return new Size((int)height, (int)width);
        }

         List<Point> calculate2middlepointofsameline(Point sourcehandle, Point targethandle)
        {

            checked
            { 
                //???check nashod ke same nabashe ya null, ya hadeaghal distance ....
                List<Point> result = new List<Point>();

                //aval samte sourcehandle node baad samte targethandle ra bar migardone
                double slope = (double)(targethandle.Y - sourcehandle.Y) / (double)(targethandle.X - sourcehandle.X);
                slope = Math.Abs(slope);

                int dr = 10;//toole 2 parekhat kenari 
                double dx = dr / Math.Sqrt((slope * slope) + 1);
                double dy = dx * slope;
                dx = Math.Abs(dx);
                dy = Math.Abs(dy);

                 
                Point middlepoint1 = new Point(sourcehandle.X, sourcehandle.Y);
                Point middlepoint2 = new Point(targethandle.X, targethandle.Y);
                if (double.IsInfinity(slope))
                {
                    //vertical line


                    if (targethandle.Y > sourcehandle.Y)
                    {
                        middlepoint1.Y += (int)dr;
                        middlepoint2.Y -= (int)dr;
                        //x bi tagheer
                    }
                    else
                    {
                        middlepoint1.Y -= (int)dr;
                        middlepoint2.Y += (int)dr;
                        //x bi tagheer
                    }


                }
                else
                {  //not infinity
                    if (slope < 0 || slope > 0)
                    {

                        if (targethandle.X > sourcehandle.X)
                        {
                            middlepoint1.X += (int)dx;
                            middlepoint2.X -= (int)dx;
                        }
                        else
                        {
                            middlepoint1.X -= (int)dx;
                            middlepoint2.X += (int)dx;

                        }

                        if (targethandle.Y > sourcehandle.Y)
                        {
                            middlepoint1.Y += (int)dy;
                            middlepoint2.Y -= (int)dy;
                        }
                        else
                        {
                            middlepoint1.Y -= (int)dy;
                            middlepoint2.Y += (int)dy;
                        }

                    }
                    else if (slope == 0)
                    {

                        if (targethandle.X > sourcehandle.X)
                        {
                            middlepoint1.X += (int)dr;
                            middlepoint2.X -= (int)dr;
                            //y bi tagheer
                        }
                        else
                        {
                            middlepoint1.X -= (int)dr;
                            middlepoint2.X += (int)dr;
                            //y bi tagheer
                        }
                    }



                }


                result.Add(middlepoint1);
                result.Add(middlepoint2);
                return result;

            }
        }
        public double IsPointNearArc(PointF pt, PointF p1, PointF p2, out PointF closest)
        {
             
            // Calculate the distance between
            // mypoint pt and the segment p1 --> p2  yani arc

            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;
            if ((dx == 0) && (dy == 0))
            {
                // It's a mypoint not a line segment.
                closest = p1;
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
                return Math.Sqrt(dx * dx + dy * dy);
            }

            // Calculate the t that minimizes the distance.
            float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) /
                (dx * dx + dy * dy);

            // See if this represents one of the segment's
            // end points or a mypoint in the middle.
            if (t < 0)
            {
                closest = new PointF(p1.X, p1.Y);
                dx = pt.X - p1.X;
                dy = pt.Y - p1.Y;
            }
            else if (t > 1)
            {
                closest = new PointF(p2.X, p2.Y);
                dx = pt.X - p2.X;
                dy = pt.Y - p2.Y;
            }
            else
            {
                closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
                dx = pt.X - closest.X;
                dy = pt.Y - closest.Y;
            }

            return Math.Sqrt(dx * dx + dy * dy);
        }

        public Rectangle get_rectnagle_area_of_polygon(List<Point> polygon_points)
        {
            // shape rectangle  area
            int minX1 = polygon_points.Min(p => p.X);
            int minY1 = polygon_points.Min(p => p.Y);
            int maxX1 = polygon_points.Max(p => p.X);
            int maxY1 = polygon_points.Max(p => p.Y);

            return new Rectangle(minX1, minY1, maxX1 - minX1, maxY1 - minY1);


        }

         
        public Point? bruteforce_can_firstshape_be_in_secondshape(GeneralPolygonBase first, GeneralPolygonBase second ) {

            int minX2 = second.connection_points.Min(p => p.X);
            int maxX2 = second.connection_points.Max(p => p.X);
            int minY2 = second.connection_points.Min(p => p.Y);
            int maxY2 = second.connection_points.Max(p => p.Y);

            int original_first_Locationx = first.Locationx;
            int original_first_Locationy = first.Locationy;
           

            ////todo brute force approach +tozih ravesh brute
            for (int x = minX2; x < maxX2; x++)
            {
                for (int y = minY2; y < maxY2; y++)
                {

                    first.Locationx = x;
                    first.Locationy = y;
                    if (is_firstshape_in_secondshape_completely(first.connection_points, second.connection_points, first.Locationx, first.Locationy))
                    {
                        first.Locationx = original_first_Locationx;//restore
                        first.Locationy = original_first_Locationy;
                        return new Point(x, y); //??faghat avlin havab ra bar migardane
                    }
                }

            }
            //reset modufied coordinats
            first.Locationx = original_first_Locationx;
            first.Locationy = original_first_Locationy;

            return null;
        }

        public Point? can_firstshape_be_in_secondshape(List<Point> first_polygon_points, List<Point> second_polygon_points, int first_locationx, int first_locationy)
        {  
           //tartib ahamiat dare first check miokone ke dar second  original_shape bashe
           //hata overlap ham reject mishe, balke kamel bayad first in second dakhel bashe ta true bede
          
          
           //aval check mikonim
           //???GHALATE , handle na, balke noghate mohiti bayad chek ashavad
           //mypoint ke first shape bezarim, dar second shape ja mishe  return mikonim+felan rectangle area of shape
           
            Rectangle first_rectangle = get_rectnagle_area_of_polygon(first_polygon_points);
            Rectangle second_rectangle = get_rectnagle_area_of_polygon(second_polygon_points);
            
            if (second_rectangle.Width > first_rectangle.Width && second_rectangle.Height > first_rectangle.Height)
            {

                int minX1 = first_polygon_points.Min(p => p.X);
                int minY1 = first_polygon_points.Min(p => p.Y);
                int maxX1 = first_polygon_points.Max(p => p.X);
                int maxY1 = first_polygon_points.Max(p => p.Y);

                int minX2 = second_polygon_points.Min(p => p.X);
                int minY2 = second_polygon_points.Min(p => p.Y);
                int maxX2 = second_polygon_points.Max(p => p.X);
                int maxY2 = second_polygon_points.Max(p => p.Y);

                //ye jori midim ke rectange in biofte too oon kamel
                return new Point(second_rectangle.X + (first_locationx - minX1), second_rectangle.Y + ((first_locationy - minY1)));

            }
            else
            {
                return null;

            }
             
          
        }

      public  bool is_point_in_polygon(Point point, List<Point> polygon_points)
        {   

            List<Point> second_shape_connection_points = polygon_points;
            //Ray-cast algorithm is here onward
            int k, j = second_shape_connection_points.Count - 1;
            bool oddNodes = false; //to check whether number of intersections is odd
            for (k = 0; k < second_shape_connection_points.Count; k++)
            {
                //fetch adjucent points of the second_shape_connection_points
                PointF polyK = second_shape_connection_points[k];
                PointF polyJ = second_shape_connection_points[j];

                //check the intersections
                if (((polyK.Y > point.Y) != (polyJ.Y > point.Y)) &&
                 (point.X < (polyJ.X - polyK.X) * (point.Y - polyK.Y) / (polyJ.Y - polyK.Y) + polyK.X))
                    oddNodes = !oddNodes; //switch between odd and even
                j = k;
            }

            //if odd number of intersections
            if (oddNodes)
            {
                //mouse mypoint is inside the second_shape_connection_points
                return true;
            }
            else //if even number of intersections
            {
                //mouse mypoint is outside the second_shape_connection_points so deselect the second_shape_connection_points
                return false;
            }


        }


        //---------------------------------------

        public bool az_father_biron_nare(GeneralPolygonBase new_setting, List<GeneralPolygonBase> fathers)
        {
           
            //this az fatheresh nazane biron
            //farz mikonim 1 father dare (invariant)  be allate tak pedari
            if (fathers != null && fathers.Count > 0)
            {
                if (!is_firstshape_in_secondshape_completely(new_setting.connection_points, fathers[0].connection_points, new_setting.Locationx, new_setting.Locationy))
                {
                  return false;

                }
            }
            return true;
        }
        bool childrenhash_azash_nazaneh_biron(GeneralPolygonBase new_setting, List<GeneralPolygonBase> all_childs) {
         
            if (!new_setting.current_collapse_state)
            { //==expand
              //children az this nazanrh biron:

                foreach (GeneralPolygonBase ch in all_childs)
                {
                    if (!is_firstshape_in_secondshape_completely(ch.connection_points, new_setting.connection_points, ch.Locationx, ch.Locationy))
                    {
                       return false;
                        
                    }
                }
            }
            return true;

        }
        public bool IS_ALL_RULES_OK(GeneralPolygonBase new_setting, List<GeneralPolygonBase> fathers, List<GeneralPolygonBase> all_childs)
        {  //yek node yek seri relatiob nesbat be father va kseri be farzand ha 
           //rule ha ba proposed new_setting state check mishe age ok bood asli ham hamintori mishe vagran rollback, albate bazihash visible nist


            bool result1 = az_father_biron_nare(new_setting, fathers);
            bool result2 = childrenhash_azash_nazaneh_biron(new_setting, all_childs);
             
            return result1 &&  result2;
            
        }
        bool is_firstshape_in_secondshape_completely(List<Point> first_polygon_points, List<Point> second_polygon_points, int first_locationx, int first_locationy)
        {
            //tartib ahamit dare: first ra dar second check mikone +  //tartib ahamiat dare first  check miokone ke dar second  original_shape bashe
            //in ravesh albate dodrost nist  paraye polygon haye pichide
            //ye rahesh checking polygin inside othere ya bouncing rect/circle

            if (can_firstshape_be_in_secondshape(first_polygon_points, second_polygon_points, first_locationx, first_locationy) == null) { return false; }
            Rectangle first_rectangle = get_rectnagle_area_of_polygon(first_polygon_points);
            Rectangle second_rectangle = get_rectnagle_area_of_polygon(second_polygon_points);

            if (!second_rectangle.Contains(first_rectangle)) { return false; };

            //TARKIBESH KON BA IN ZIRIE!!!!!!!!!!!!!!  

           
            //hata overlap ham reject mishe, balke kamel bayad first in second dakhel bashe ta true bede
            foreach (Point p in first_polygon_points)
            {///???GHALATE , handle na, balke noghate mohiti bayad chek ashavad
                if (!is_point_in_polygon(p, second_polygon_points))
                    return false;
            }

            return true;
        }

        public List<GeneralPolygonBase> remove_first_from_second_collection(List<GeneralPolygonBase> first, List<GeneralPolygonBase> second)
        {
            List<GeneralPolygonBase> temp = new List<GeneralPolygonBase>();
            for (int i = 0; i < second.Count(); i++)
            {
                bool was = false;
                for (int j = 0; j < first.Count(); j++)
                {
                    if (second[i].GetID() == first[j].GetID())
                    {
                        was = true;
                        break;
                    }
                }

                if (!was) temp.Add(second[i]);
            }
            return temp;
        }

        public class util_folder_or_file
        {

          void CopyDirectory(string mySource, string myDestination)
        {

            DirectoryInfo Source = new DirectoryInfo(mySource);
            DirectoryInfo Destination = new DirectoryInfo(myDestination);

            foreach (FileInfo f in Source.GetFiles())
            {
                f.CopyTo(Destination + @"\" + f.Name);
            }

            foreach (DirectoryInfo dS in Source.GetDirectories())
            {
                string newDirPart = dS.FullName.Replace(Source.FullName, "");
                string newDestinationPath = Destination + newDirPart;
                DirectoryInfo dD = new DirectoryInfo(newDestinationPath);


                //if (!CountOnly)
                {
                    dD.Create();
                    CopyDirectory(dS.FullName, dD.FullName);

                }
            }

        }
          string trycopydirectory(string source, string destination)
        {
            try
            {

                CopyDirectory(source, destination);
                double sorceLength = GetDirectorySize(source);
                double destinationLength = GetDirectorySize(destination);
                if (sorceLength == destinationLength)
                {
                    return destination;

                }
                else
                {
                    return "";
                }


            }
            catch (Exception ex)
            {

                return "";
            }

        }

          string copyfile(string source, string destination)
        {
            try
            {
                FileInfo f = new FileInfo(source);
                string realdest = destination + @"\" + f.Name;
                File.Copy(source, realdest);
                double sorceLength = new FileInfo(source).Length;
                double destinationLength = new FileInfo(realdest).Length;
                if (sorceLength == destinationLength)
                {
                    return realdest;

                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return "";
            }

        }

          public  string try_copy_folder_or_file(string source, string destination)
        {
                //detect it is file or directory to call appropriate function



            if (File.Exists(source))
            {
                return copyfile(source, destination);
            }
            else if (Directory.Exists(source))
            {

                string realdestination = destination + @"\" + new DirectoryInfo(source).Name;
                Directory.CreateDirectory(realdestination);
                return trycopydirectory(source, realdestination);
            }
            else
            {
                return "";

            }

        }
          public  bool deletefileORdirectory(string filepath)
            {
                try
                {
                    if (Directory.Exists(filepath))
                    {
                        Directory.Delete(filepath, true);
                        if (!Directory.Exists(filepath)) throw new Exception(); //kamel delete nashodeh
                        return true;
                    }
                    else
                    {
                        //az aval ham nabood
                        return true;
                    }

                }
                catch (Exception ex)
                {
                    return false;
                }

            }
          long GetDirectorySize(string p)
            {
                // 1.
                // Get array of all file names.
                string[] a = Directory.GetFiles(p, "*.*");

                // 2.
                // Calculate total bytes of all files in a loop.
                long b = 0;
                foreach (string name in a)
                {
                    // 3.
                    // Use FileInfo to get length of each file.
                    FileInfo info = new FileInfo(name);
                    b += info.Length;
                }
                // 4.
                // Return total size
                return b;
            }

            //void resize_and_draw_image_if_has(Graphics g)
            //{
            //    //aval size image standard mishe va yeksan



            //    if (image != null)
            //    {
            //        UtilityandGeometryMath gm = new UtilityandGeometryMath();
            //        Size size = gm.resizeimage(image);
            //        g.DrawImage(image, Locationx, Locationy, size.Height, size.Width);
            //    }


            //}

        }





    }
}
