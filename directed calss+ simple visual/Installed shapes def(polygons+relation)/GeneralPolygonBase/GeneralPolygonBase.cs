using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    [Serializable]
    public class GeneralPolygonBase : DirectedGraphClass.node
    {
        //ba inheritance mishe yek shek jadid sakht va load kard dar ghalebe dll plugin  
        public GeneralPolygonBase(int my_ID, int upperleftx, int upperleftxy) : base(my_ID)
        {



            //TODO + ???? (الزام حداقل 3 تا نقطه  پیاده نکردیم فعلا)


            iconWidth = iconsetting.defalticonwidth;
            iconHeight = iconsetting.defaulticonheight;
            textareaHeight = textareasetting.defaulttextareheight;
            textareaWidth = textareasetting.defaulttextarewidth;
            _collapse_state = false;//first state is expandeded(childrens visible in father)
            color = color = SystemColorPolicy.DefaultUnelectedSateColor; 
        }
         
        public virtual  void  draw(Graphics g, int selected_shape_id)
        {
            //g reference ast
            //?????????in g male form ast behesh midim ta render kone khodesh ra 

         
            //is this node selected
            if (selected_shape_id == this.GetID())
            {
                g.FillPolygon(new SolidBrush(SystemColorPolicy.SelectedSateColor), _handles);
            }
            else
            {
                g.FillPolygon(new SolidBrush(color), _handles);
            }
            //yek line ham doresh mikeshim
            g.DrawPolygon(Pens.Black, _handles);


            // icon_textarea_drawed = true;////baraye inke 2 bar render nashe chon draw_icon_textarea() tavasote child ham mitone call she


            draw_icon_and_textarea(g);//!!!!momkene 2bar call she chon child ham call mikone dar draw overide

        }


     
        protected  void draw_icon_and_textarea(Graphics g)
        {
            //if (icon_textarea_drawed) return;

                //mypoint connection_points
                foreach (Point h in connection_points)
            {
                g.FillEllipse(Brushes.Black, h.X - 3, h.Y - 3, 6, 6);

            }


            //draw string data on node if has

            List<Point> points = draw_text.gettextareapoints(textareaWidth, textareaHeight, Locationx, Locationy);
            TextBox temp= draw_text.gettextboxdimensions(textareaWidth, textareaHeight);
            draw_text.draw_text_if_has(g, temp, points[0].X, points[0].Y, this.Data);

            //Draw icon of attachment file on center of polygon with opacity-----


            drawicon.read_and_draw_icon_with_opacity(g, Locationx, Locationy, iconWidth, iconHeight, filePath);

        }


        public virtual bool is_point_on_shape(int clickx, int clicky)
        {
            UtilityandGeometryMath util = new UtilityandGeometryMath();
            //calling general point to polygon area detection
            return util.is_point_in_polygon(new Point(clickx, clicky), connection_points);
        


        }
       

        int _Locationx;
        public int Locationx
        { //dar movement faghat location meyar ast. va ba tagheer aan baghie naghat bayad move shavad
            get { return _Locationx; }
            set //also move other points
            {
                int dif = value - _Locationx;//differnece the location point moved
                for (int i=0; i<_handles.Count(); i++)
                {
                    _handles[i].X += dif;//applay difference to other polygon points(handles)
                    expand_handles[i].X += dif;//movement also applys to expand and collapse states
                    collapse_handles[i].X += dif;//movement also applys to expand and collapse states


                }
                 _Locationx = value;
            }

        }
        int _Locationy;
        public int Locationy
        {   //dar movement faghat location meyar ast. va ba tagheer aan baghie naghat bayad move shavad
            get { return _Locationy; }
            set//also move other points
            {
                int dif = value - _Locationy;//differnece the location point moved
                for (int i = 0; i < _handles.Count(); i++)
                {
                    _handles[i].Y += dif; //applay difference to other polygon points(handles)
                    expand_handles[i].Y += dif;//movement also applys to expand and collapse states
                    collapse_handles[i].Y += dif;//movement also applys to expand and collapse states

                }
                _Locationy= value; 
            }

        }


        //labels of this shape+ *** int yeni id oon system label
        public Dictionary<int, bool> labels =new Dictionary<int, bool>(); 

        //not used for now
        //private Image image;
        //public Image Image
        //{
        //    get { return image; }
        //    set { image = value; }
        //}
        //----------------
        private string data;
        public string Data
        {
            get { return data; }
            set { data = value; }
        }


        string filePath;

        int iconWidth;
        int iconHeight;
        public void setfilepath(string myfilepath)
        {
            filePath = myfilepath;  
        }
        public string getfilepath()
        {
            return filePath;
        }

        protected void addallpoints(List<Point> all_handles)

        {
            //???? faghat yekbar bayad ejra+dar consructor biarim 

            collapse_handles = expand_handles = _handles = new Point[all_handles.Count];
            //number of point of polygon is dynamic, so any polygon is possible
            for (int i = 0; i < all_handles.Count; i++)
            {

                this._handles[i] = all_handles[i];

            }
            //always collapse and expand and _handles count is the same, and at first content of these 3 is same, (in zoom ,... content bocemoe different)
            UtilityandGeometryMath util = new UtilityandGeometryMath();
            collapse_handles = util.DeepObjectCopy<Point[]>(connection_points.ToArray());
            expand_handles = util.DeepObjectCopy<Point[]>(connection_points.ToArray());


            _Locationx = Locationy = 0;

        }


       protected  Point[] _handles;

        Point[] collapse_handles;// zoom khode shape, zoom kole safhe va screen move inha edit mishe
                                 
        Point[] expand_handles;// zoom khode shape, zoom kole safhe va screen move inha edit mishe
        public List<Point> connection_points
        {
            get
            {
                 return _handles.ToList();
            }
        }
       
          

        //dar vaghe beyn ein 2 size switch mikonim
        bool _collapse_state = false;
        public  bool current_collapse_state { get { return _collapse_state; } }

        //store labels of the shape
        public Dictionary<int, bool> systemlabelsID = new Dictionary<int, bool>();

        public Dictionary<int, bool> getAlllabels()
        {

            return systemlabelsID;

        }

         

        public virtual void expand()
        {   
            
           //collapse feli ra backup mikonim ta badan bargardim be hamin, baad expand mikonim hame hanlde ha ra
            for (int i = 0; i < collapse_handles.Count(); i++)
            {      
                 collapse_handles[i].X = _handles[i].X;
                 collapse_handles[i].Y = _handles[i].Y;
            }
            //apply expand
            for (int i = 0; i < expand_handles.Count(); i++)
            {
                _handles[i].X = expand_handles[i].X;
                _handles[i].Y= expand_handles[i].Y;

            }


          
            _collapse_state = false;
        }
        public virtual void collapse()
        {
            //expand feli ra backup mikonim ta badan bargardim be hamin, baad collpase mikonim hame hanlde ha ra
            for (int i = 0; i < expand_handles.Count(); i++) 
            {
                 expand_handles[i].X= _handles[i].X;
                 expand_handles[i].Y= _handles[i].Y;
            }

            //apply collapse
            for (int i = 0; i < collapse_handles.Count(); i++)
            {
                  _handles[i].X= collapse_handles[i].X;
                  _handles[i].Y= collapse_handles[i].Y;
            }
             
            _collapse_state = true;
        }

        public virtual void scaleup_shape(int scalefactor, bool movelocation_for_screenmove/*true for overla scene zoom*/)
        {    //har 2 collapse va expand ra bayad change konim
             //location shape faghat dar zoom kole screenn avaz mishe



            //aval location ra mibarim be mabda mokhtasat. badan zoom mikonim
            if (!movelocation_for_screenmove)
            {
                for (int i = 0; i < _handles.Count(); i++)
                {
                    _handles[i].X -= Locationx;
                    _handles[i].Y -= Locationy;

                }
            }



                if(movelocation_for_screenmove)             
                for (int i = 0; i < collapse_handles.Count(); i++)
                {
                     _handles[i].X *= scalefactor;
                     _handles[i].Y *= scalefactor;
                     collapse_handles[i].X *= scalefactor;
                     collapse_handles[i].Y *= scalefactor;
                     expand_handles[i].X *= scalefactor;
                     expand_handles[i].Y *= scalefactor;
               } 



            if (!movelocation_for_screenmove)
                if (current_collapse_state)
                {
                    for (int i = 0; i < collapse_handles.Count(); i++)
                    {
                         _handles[i].X *= scalefactor;
                         _handles[i].Y *= scalefactor;
                         collapse_handles[i].X *= scalefactor;
                         collapse_handles[i].Y *= scalefactor;


                    }
                }
                else
                {
                    for (int i = 0; i < expand_handles.Count(); i++)
                    {
                        _handles[i].X *= scalefactor;
                        _handles[i].Y *= scalefactor;
                        expand_handles[i].X *= scalefactor;
                        expand_handles[i].Y *= scalefactor; 

                    }
                }


             



            if (movelocation_for_screenmove)
                {  /*location ham dar overal zoom scene dar factor zarb mishe*/
                    textareaHeight *= scalefactor;
                    textareaWidth *= scalefactor;
                    _Locationx *= scalefactor;
                    _Locationy *= scalefactor;
                    iconWidth *= scalefactor;
                    iconHeight *= scalefactor;
            
               }


            //shape ra az mabda mokhtasat be jaye ghabli mibarim
            if (!movelocation_for_screenmove)
            {

                for (int i = 0; i < _handles.Count(); i++)
                {
                    _handles[i].X += Locationx;
                    _handles[i].Y += Locationy;

                }


            }
        }
        
        public virtual void scaledown_shape(int scalefactor, bool movelocation_for_screenmove/*true for all scene*/)
        { //har 2 collapse va expand ra bayad change konim
            //location shape faghat dar zoom kole screenn avaz mishe

            if (!movelocation_for_screenmove)
            {
                for (int i = 0; i < _handles.Count(); i++)
                {

                    _handles[i].X -= Locationx;
                    _handles[i].Y -= Locationy;


                }
            }

            //aval location ra mibarim be mabda mokhtasat. badan zoom mikonim
            if (movelocation_for_screenmove)
                for (int i = 0; i < collapse_handles.Count(); i++)
                {
                    _handles[i].X /= scalefactor;
                    _handles[i].Y /= scalefactor;
                    collapse_handles[i].X /= scalefactor;
                    collapse_handles[i].Y /= scalefactor;
                    expand_handles[i].X /= scalefactor;
                    expand_handles[i].Y /= scalefactor;
                }



            if (!movelocation_for_screenmove)
                if (current_collapse_state)
                {
                    for (int i = 0; i < collapse_handles.Count(); i++)
                    {
                        _handles[i].X /= scalefactor;
                        _handles[i].Y /= scalefactor;
                        collapse_handles[i].X /= scalefactor;
                        collapse_handles[i].Y /= scalefactor;


                    }
                }
                else
                {
                    for (int i = 0; i < expand_handles.Count(); i++)
                    {
                        _handles[i].X /= scalefactor;
                        _handles[i].Y /= scalefactor;
                        expand_handles[i].X /= scalefactor;
                        expand_handles[i].Y /= scalefactor;

                    }
                }


            if (movelocation_for_screenmove)
            {  /*location ham dar overal zoom scene dar factor taghsim mishe*/
                textareaHeight /= scalefactor;
                textareaWidth /= scalefactor;
                _Locationx /= scalefactor;
                _Locationy /= scalefactor;
                iconWidth /= scalefactor;
                iconHeight /= scalefactor;

            }



            //shape ra az mabda mokhtasat be jaye ghabli mibarim
            if (!movelocation_for_screenmove)
            {
                for (int i = 0; i < _handles.Count(); i++)
                {

                    _handles[i].X += Locationx;
                    _handles[i].Y += Locationy;

                }
            }


        }
        
        //dimention of textarea for display




        int textareaWidth;
        int textareaHeight;
        public Color color ;

        bool istextareainpolygon()
        {

            return true;
            
///?????? 4 noghte text area bayad tooye polygon bashad + vali dorost nist hamishe
                List<Point> points = draw_text.gettextareapoints(textareaWidth,textareaHeight, Locationx, Locationy);
                Point point1 = points[0];
                Point point2 = points[1];
                Point point3 = points[2];
                Point point4 = points[3];

                
                UtilityandGeometryMath util = new UtilityandGeometryMath();
                //check textarea is in collapse state of shape
                bool remain = util.is_point_in_polygon(point1, collapse_handles.ToList())
                && util.is_point_in_polygon(point2, collapse_handles.ToList())
                && util.is_point_in_polygon(point3, collapse_handles.ToList())
                && util.is_point_in_polygon(point4, collapse_handles.ToList());
                //check textarea is in expand state of shape
                //bool inexpand = util.is_point_in_polygon(point1,expand_handles.ToList())
                //&& util.is_point_in_polygon(point2, expand_handles.ToList())
                //&& util.is_point_in_polygon(point3, expand_handles.ToList())
                //&& util.is_point_in_polygon(point4, expand_handles.ToList());


                //textarea bayad ham dar collapse ham dar expand state dakhel shape bashe:


                if (remain)
                {

                    return true;//allow to increase text area
                }
                //else if(!current_collapse_state && inexpand)
                //{

                //    return true;
                //}
                else
                {
                    return false; /*dont increase text area dimension*/

                }


        }


        public virtual void decrease_text_area_width()

        {
            //check ghanoon lazim nist chon size kamtar mikonim 
            //if (istextareainpolygon())
            {
                textarearesize.decrease_text_area_width(ref textareaWidth);
            }
        }
        public virtual void increase_text_area__width()
        {

            if (istextareainpolygon()){

                textarearesize.increase_text_area__width(ref textareaWidth);
            }
        }
        public virtual void decrease_text_area__height()
        {   //check ghanoon lazim nist chon size kamtar mikonim 
           // if (istextareainpolygon())
            {
                textarearesize.decrease_text_area__height(ref textareaHeight);
            }
        }
        public virtual void increase_text_area__height()
        {
            if (istextareainpolygon())
            {
                textarearesize.increase_text_area__height(ref textareaHeight);
            }
        }



    }
}
