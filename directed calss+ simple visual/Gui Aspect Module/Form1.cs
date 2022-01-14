using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DirectedGraphClass;


//------------------------------------------------------------------------------------------------
//left click: for selecting any shape type or relation
//press 1,2,3 for new types of node
//press r: select source node then  select target for new relation(realtion to iteself or others) then press: r
//press d for delete the selected shape type or selected relation
//press e for edit the selected shape or selected relation data and labels
//press j for set image to selected shape type or relation (from clipboard if not empty) 

//press h then select source(child) then press h then selct target(father)   for adding a hierchcial relationship from source to target node=?source node become children of target, and drawn only on the target circle surface+we can add more levels and deepen "nested relationship"
//press j for disconnect a child (and its nested child tree) from father (remove hierachial relation) => child now can move out of father surface
//press s for save database current state
//press o for open database from disk
//press n for create new empty database on the disk+unloads current loaded one
//press space for toggle collapse or expand a shape (collpse hides its childrens)

//press downkey to decreasae heigth of a polygon textrea
//press leftkey to decreasae width  of a polygon textrea
//press upkey to increase heigth  of a polygon textrea
//press rightkey to increase width of a polygon textrea

//presee leftmouse key on free space(not shapes...) and drag mouse to move the screen itself
//please do normal copy and paste a file to attach to the selected shape or relation
//press Del button to delete the attachment file of shape
//press Enter button to launch the attachment file of shape
//press + button to zoom in scene (affect all polygons and relations)
//press - button to zoom out the scene (affect all polygons and relations)
//press *(multiply) button to make bigger only selected shape
//press /(divie) button to make smaller only selected shape
//press l for add/remove/edit system label list(tags)


//press 7   Reset the scenario animation frame(step) to 0
//press 8   Forwald the scenario animation frame(step) one step
//press 9   backward the scenario animation frame(step) one step



//------------------------------------------------------------------------------------------------


namespace WindowsFormsApplication1
{
    partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Gui gui;
        //----------------------
        Graphics g;
        UtilityandGeometryMath geometryMath = new UtilityandGeometryMath();
        public bool drawmode=false; //disble / enable draw
      

        public List<Cache_computed_relations> non_hierachial_arc_graphic_data___cashe = new List<Cache_computed_relations>();
        public List<GeneralPolygonBase> visible_shape___cache = new List<GeneralPolygonBase>();



      

        void Form1_Load(object sender, EventArgs e)
        {
           
          
        }
        void Form1_KeyDown(object sender, KeyEventArgs e)
          {
            if (e.KeyCode != Keys.N && e.KeyCode != Keys.O && !drawmode ) return; //means: except new or open database commands
            gui.KeyDown(sender, e);
          }
         

       // int i = 0; 

        void Form1_MouseMove(object sender, MouseEventArgs e)
        {
           
            //i++;
            //if (i>30) { i = 0; } else { return; }

            if (!drawmode) return; //nothing is loaded so dont respond to events
            gui.MouseMove(sender, e);
        }
        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (!drawmode) return; //nothing is loaded so dont respond to events

            if (e.Button == MouseButtons.Left)
            {
                gui.MouseDown(sender, e);
            }
        }
        void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!drawmode) return; //nothing is loaded so dont respond to events
            if (e.Button == MouseButtons.Left)
            {
                gui.MouseUp(sender, e);
            }
        }
         
        void ddddraw(Point p1, Point p2)
        {
            Pen p = new Pen(Color.FromArgb(190, Color.Green), 2);
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.ArrowAnchor;
            p.CustomEndCap = new AdjustableArrowCap(9, 9);
            p.DashStyle = DashStyle.Solid;
            p.DashCap = DashCap.Triangle;
            //Graphics graph = this.CreateGraphics();
            g.DrawLine(p, p1, p2 );

        }

        void draw_a_relation(string nested_relation_value, Color statecolor, rrelation rel, bool up, int distace, Point sourcehandle, Point targethandle)
        {
           
            Pen pn = new Pen(statecolor,1);
            //----------------------------------------------------------
            //draw normal directed realtion=not hierachial relation(farzand pedari)
            if (rel.type != nested_relation_value)
            {

                if (rel.ToTargetNode_id == rel.SourceNode_id)   //togh arc=>soyrce and target node are same
                { 

                    Point highest_handle = targethandle;//or source because passed as hiest handle;
                    Point upper_right = new Point(highest_handle.X + distace - 30, highest_handle.Y - distace);//az khodeamn
                    Point upper_left = new Point(highest_handle.X - distace + 30, highest_handle.Y - distace);////az khodeamn
                                                                                                              //togh felan +ye moslas mishe+badan mishe ba bezier smoothesh kard


                    //draw other parts
                    g.DrawLine(pn, upper_right, highest_handle);  //!!!!!!!todo draw curve kon rahat khamideh mishe
                    g.DrawLine(pn, highest_handle, upper_left);
                     // draw(upper_right, upper_left);

                    //draw middle part of togh
                    using (GraphicsPath capPath = new GraphicsPath())
                    {
                         

                        int middleofmiddellineX = (upper_left.X + upper_right.X) / 2;
                        int middleofmiddellineY = (upper_left.Y + upper_right.Y) / 2;
                        Point middleofmiddelline = new Point(middleofmiddellineX, middleofmiddellineY);
                        //A triangle
                        //capPath.AddLine(-5, 0, 5, 0);
                        //capPath.AddLine(-5, 0, 0, 5);
                        //capPath.AddLine(0, 5, 5, 0);

                        g.DrawLine(pn, upper_left, middleofmiddelline);
                        pn.CustomStartCap = new AdjustableArrowCap(rel.width_middle_triangle, rel.heigh_middle_triangle, true);

                        g.DrawLine(pn, middleofmiddelline, upper_right);


                        //--------------------------------------------------------
                        //draw string data on node if has
                        List<Point> points = draw_text.gettextareapoints(rel.textareaWidth, rel.textareaHeight, middleofmiddellineX, middleofmiddellineY);
                        TextBox temp = draw_text.gettextboxdimensions(rel.textareaWidth, rel.textareaHeight);
                        draw_text.draw_text_if_has(g, temp, points[0].X, points[0].Y, rel.data);
                        

                        //--------Draw icon of attachment on center of polygon with opacity-----
                        drawicon.read_and_draw_icon_with_opacity(g, middleofmiddellineX, middleofmiddellineY, rel.iconWidth, rel.iconHeight, rel.getfilepath());
                    }

                    //MOHEM: felan detection ba middle part anjam mishe faghat
                    non_hierachial_arc_graphic_data___cashe.Add(new Cache_computed_relations(rel, highest_handle, upper_left, upper_right, highest_handle));//???/felan faghta ba khate balash detect mikonim+felan faghat togh rooye balatrin hanlde rasm mishe

                }
                else  //non togh arc=> soyrce and target node are different
                {
                    //chon dige multi relation draw support mikonim bayad arc ha az ham  fasele bedim pas vasate har arc badi ra bala tar mibarim ta rooye ham nayoftand
                    //distance fasele az khate mostagheem ast, va manfi boodane va mosbat bodane yani ziresh ya balash bekeshim                   
                    //az samte source be target middle node miravim                    
                    
                    List<Point> mmm = geometryMath.calculate2parralelmiddlepointofsameline(distace, up, sourcehandle, targethandle);
                       g.DrawLine(pn, sourcehandle, mmm[0]);
                       g.DrawLine(pn, mmm[1], targethandle);
                   
                    //----------draw middle arroe part-----------------
                     


                    int middleofmiddellineX = (mmm[0].X + mmm[1].X) / 2;
                    int middleofmiddellineY = (mmm[0].Y + mmm[1].Y) / 2;
                    Point middleofmiddelline = new Point(middleofmiddellineX, middleofmiddellineY);

                    //draw(mmm[0], mmm[1]);

                    using (GraphicsPath capPath = new GraphicsPath())
                    {
                        g.DrawLine(pn, middleofmiddelline, mmm[1]);

                        // A triangle
                        //  capPath.AddLine(-5, 0, 5, 0);
                        //  capPath.AddLine(-5, 0, 0, 5);
                        //  capPath.AddLine(0, 5, 5, 0);
                        pn.CustomStartCap = new AdjustableArrowCap(rel.width_middle_triangle, rel.heigh_middle_triangle, true);

                        //  chera ye jahat mikeshe???
                        g.DrawLine(pn, middleofmiddelline, mmm[0]);


                        //draw text
                        
                        List<Point> points = draw_text.gettextareapoints(rel.textareaWidth, rel.textareaHeight, middleofmiddellineX, middleofmiddellineY);
                        TextBox temp = draw_text.gettextboxdimensions(rel.textareaWidth, rel.textareaHeight);
                        draw_text.draw_text_if_has(g, temp, points[0].X, points[0].Y, rel.data);
                        //Draw icon of attachment on center of polygon with opacity-----
                        drawicon.read_and_draw_icon_with_opacity(g, middleofmiddellineX, middleofmiddellineY, rel.iconWidth, rel.iconHeight, rel.getfilepath());

                      
                        //--------------------------------------------------------

                    }

                    //ooonvari
                    non_hierachial_arc_graphic_data___cashe.Add(new Cache_computed_relations(rel, sourcehandle, mmm[0], mmm[1], targethandle));
                     

                   
                    if (rel.image != null)
                    {
                        UtilityandGeometryMath gm = new UtilityandGeometryMath();
                        Size size = gm.resizeimage(rel.image);
                        //aval size image standard mishe va yeksan baad draw
                        g.DrawImage(rel.image, (sourcehandle.X + targethandle.X) / 2, (sourcehandle.Y + targethandle.Y) / 2, size.Height, size.Width);
                    }
                         // if (n.centerx == n2.centerx) // todo aval va akhare yal rooye ham naiofteh??????????????????????????????
                        //no need to draw relation because center of two node are same , vagrna overflow mishe az nazare rizai taghseem bar sefr mishe
                        //return;
                    
                }
                 

            }
            else //hierachial arc(farzand pedari)
            {
                //draw hierachial directed realtion policy==(if has nested lablel) bedone jahat va dotted line
                pn.SetLineCap(LineCap.NoAnchor, LineCap.NoAnchor, DashCap.Flat);
                pn.DashStyle = DashStyle.Dot;
                g.DrawLine(pn, sourcehandle, targethandle);
              

            }
            //-----------------------------------------------------------------

        }
    
        void draw_all_relation_between_2_nodes_border_point_based(string nested_relation_value, GeneralPolygonBase n, GeneralPolygonBase n2, List<rrelation> all_relation_between_2_point)
        {
            //aval hame yalhaye beyne 2 ta arc rasm ta hame archa beyne in 2 shape rasm ta multi edge shekl girad
            if (all_relation_between_2_point.Count() == 0) return; //todo ???also check for null
                      
            
            //////////////////////////////////////////////////////////////////////////////////
            int start_index = 0;
            int distace = 0;

            /////////////////////////////////darw self arc=toogh//////////////////////////////
            if (n.GetID() == n2.GetID())
            {
                n.connection_points.OrderBy(p => p.Y);
                Point highest_handle = n.connection_points[n.connection_points.Count() - 1]; //??? yekish chera ja  mindaze???????????????


                distace = 50;
                for (int i = start_index; i < all_relation_between_2_point.Count(); i++)
                {
                    distace = distace + 20;


                    draw_a_relation(nested_relation_value, gui.WhichColorRelation(all_relation_between_2_point[i]), all_relation_between_2_point[i], true, distace, highest_handle, highest_handle);

                }

            }
            else
            {
                /////////////////////////////darw arc between two different nodes////////////////////                   
                Point sourcehandle = n.connection_points[0]; //random
                Point targethandle = n2.connection_points[0]; //random
                                                              //double amoodmonasef_slope = -(double)(sourcehandle.X - targethandle.X) / (double)(sourcehandle.Y - targethandle.Y);//aks mikonim chon amood monasef slopesh barakas va manfi ast
                double smallest_distance = geometryMath.GetDistanceBetween2Ppoint(sourcehandle.X, sourcehandle.Y, targethandle.X, targethandle.Y);
                //find smallest distance between two connection_points of two shape

                foreach (Point pp in n.connection_points)
                {
                    foreach (Point pp2 in n2.connection_points)
                    {

                        double temp = geometryMath.GetDistanceBetween2Ppoint(pp.X, pp.Y, pp2.X, pp2.Y);
                        if (temp < smallest_distance)
                        {
                            sourcehandle = pp;
                            targethandle = pp2;
                            smallest_distance = temp;
                        }

                    }
                }
                //draw multi arcs
                /////////////////now nearest handle between these two shape is ready=>draw all relation between these nodes:

                //count_of_relations is zooj now+az 1 start mikonim chon 0 index rasm shode oon bala
                bool up = true;
                for (int i = 0; i < all_relation_between_2_point.Count(); i++)
                {

                    rrelation temp1 = all_relation_between_2_point[i];


                    /////////////////////////////////////////////////////////////////////////////
                    //for one relation//--------------------------------------------------------
                    Point sourcehandle1 = sourcehandle;
                    Point targethandle2 = targethandle;

                    if (temp1.SourceNode_id == n.GetID())
                    {
                        sourcehandle1 = sourcehandle;//same
                        targethandle2 = targethandle;//same

                    }
                    else
                    {
                        sourcehandle1 = targethandle;
                        targethandle2 = sourcehandle;
                    }


                    if (i % 2 == 1)
                    {   // 0, 50,50,100,100,... pattern                        

                        distace = distace + 50;//harchi yall ha ziadtar=> doortar mishe faselash+baraye tafkik arc haye beyne haman 2 node
                        if (i == 0) distace = 0;
                    }
                    up = !up;//true false true false pattern
                      
                    draw_a_relation(nested_relation_value,gui.WhichColorRelation(temp1), temp1, up, distace, sourcehandle1, targethandle2);
                     

                }
                //////////////////////////////////////////////////////////////////////////////////////

            }


        }
        void draw_all_nodes(Graphics g, List<GeneralPolygonBase> shapes, int selected_node_id)
        {
            //harvaght oon error grapghic invalid dad bayad g ra pass bedi chon dipose shode

            //IN MOHEME KE AVAL NODE BAAD RELATION  JOZVE RAHE HAL AST
            //moshkele olavit beine original_shape ke vasate arc va start node miamad ba tarsim aval arc ha hal ke moshkel dovom be vojood avard
            ///////draw nodes itself
            int count = shapes.Count();

            // for (int i = 0; i < count; i++)
           
            for (int i = count - 1; i >= 0; i--)
            {
                shapes[i].draw(g, selected_node_id);

            }

            //if (selected_node_id>=0)
            //{

            //        Ipolygon p = gui.FindShapeById(selected_node_id);
            //        rectangle r = ((rectangle)(p));
            //        // this.richTextBox1.Text = r.Data;
            //        setrectangle(r);

            //}
            //else
            //{
            //    richTextBox1.Hide();
            //    this.Focus();
            //}


        }

        //Form.refresh  monjar be in Form1_Paint mishe = > yeki az rahhaye redraw ast call on refresh()
         
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!drawmode) return; //nothing is loaded so dont respond to events
            gui.form1Closing(e);
        }
        //-------------------------------

        #region    
        

        List<directed_relation> sumofinternals = new List<directed_relation>();

        List<directed_relation> SumOfOuters = new List<directed_relation>();
        void extract_outer_internal_relations__MODIFY_OUTERS(GeneralPolygonBase node, List<directed_relation> CLONE_relations_FOR_VIEW)
        {
            List<rrelation> allrelationonbrothers = new List<rrelation>();//relation beyne farzandanesh = relation beyne frazand ba farzanad+ relation beyne farzand ba biron az father(this node)          
            
            List<GeneralPolygonBase> all_childs = gui.traverse_tree_and_get_total_childs_with_specific_relationtype(node, gui.key2command.get_nested_relation_value()); //kash mishod inam az copy gereft
            //arc haye dakheli beyne khode chiderens ra peyda mikonim dar scope pedar
            all_childs.RemoveAt(0); //remove root itself
            //////////////////////////////////////////////////////////////////////////
            for (int i = 0; i < all_childs.Count(); i++)  
            {
                for (int j = i; j < all_childs.Count(); j++) //togh ham shamel mishe
                {
                    GeneralPolygonBase node1 = gui.FindShapeById(all_childs[i].GetID());
                    GeneralPolygonBase node2 = gui.FindShapeById(all_childs[j].GetID());
                    //?????che in vari che oonvari
                    //from real
                    List<rrelation> all_directed_relation_between_two_nodes = gui.all_directed_relation_between_two_nodes_and_reverse(node1, node2);
                    allrelationonbrothers.AddRange(all_directed_relation_between_two_nodes);

                    sumofinternals.AddRange(all_directed_relation_between_two_nodes);
                }
               
            }
            /////////////////////////////////////////////////////////////////////////
            //find outer relations of childs to out nodes (relation be biron az pedar ya brothers ha)
            List<directed_relation> allsystemrealtions = CLONE_relations_FOR_VIEW;
            List<directed_relation> outerrelations = new List<directed_relation>();

            for (int i = 0; i < allsystemrealtions.Count(); i++)
            {

                bool was = false;
                for (int j = 0; j < allrelationonbrothers.Count(); j++)
                {
                    if (allsystemrealtions[i].getID() == allrelationonbrothers[j].getID())
                    {
                        was = true;
                        break;
                    }
                }

                if (!was && allsystemrealtions[i].type != gui.get_nested_relation_value()) { outerrelations.Add(allsystemrealtions[i]); } //nested ham rad kardim)
                ///*yani ghablan modify nashode vagarna 2 bar hesab mishe*/
            }


            //hala outers hara modify mikonim be pedar nesbat midahim
            //mikhahim modify konim copy ash ra modify mikonim
            List<directed_relation> modifeid_outers = outerrelations;
            for (int i = 0; i < modifeid_outers.Count; i++)
            {
                directed_relation temp = modifeid_outers[i];
                //togh ha chi?
                if (exist_shape_with_id(temp.SourceNode_id, all_childs)) { temp.SourceNode_id = node.GetID(); }
                if (exist_shape_with_id(temp.ToTargetNode_id, all_childs)) { temp.ToTargetNode_id = node.GetID(); }

            }

            //send to out puts
            SumOfOuters.AddRange(modifeid_outers);

        }
        bool exist_shape_with_id(int id, List<GeneralPolygonBase> nodes_collection)
        {
            for (int i = 0; i < nodes_collection.Count; i++)
            {
                if (nodes_collection[i].GetID() == id) { return true; }
            }
            return false;

        }
        bool exist_relation_with_id(int id, List<directed_relation> relation_collection)
        {
            for (int i = 0; i < relation_collection.Count; i++)
            {
                if (relation_collection[i].getID() == id) { return true; }
            }
            return false;

        }
        public void Form1_Paint(object sender, PaintEventArgs e)
        { 
            

            if (!drawmode) return; //no database is loaded so don't need to draw             

            //moteasefane g mipare nemitonim dar yek function dige bebarim ina ra va az shape dige call konim, haminja bayad kar konim bahash
            g = e.Graphics;
            non_hierachial_arc_graphic_data___cashe.Clear();//clear arc cache
            visible_shape___cache.Clear();//clear shape cache
            e.Graphics.Clear(globalsetting.ClearScreeColor);//clear screen
            //clear ha ba har bar Refresh() ya Form1_Paint() anjam va redraw mishe

            //inputhandler.form1loaded();//mohem migim data bede na event loo bedim  yani in ra be an negash va hide mkonim va be zabane khodesh

            UtilityandGeometryMath util = new UtilityandGeometryMath();
            List<directed_relation> CLONE_relations_FOR_DRAW = util.DeepObjectCopy(gui.GetAllRelations());//dige faghat ba in copy modify mikonim baraye draw  (view faghat baraye draw misazim masalan hide kardane barskhi shapes)
            sumofinternals.Clear();//clear temps 
            SumOfOuters.Clear();//clear temps 


            List<GeneralPolygonBase> original_shapes = (gui.GetAllShapes());
            List<GeneralPolygonBase> shapes_tohide = new List<GeneralPolygonBase>();//baraye sakht view inha ra hide mikonim
             
            for (int i = 0; i < gui.GetAllNodecounts(); i++)
            {
                if ((original_shapes[i]).current_collapse_state == true && !exist_shape_with_id(original_shapes[i].GetID(), shapes_tohide)) // hide shode ha ra az list kol dar nazar nemigirim
                {
                    extract_outer_internal_relations__MODIFY_OUTERS(original_shapes[i], CLONE_relations_FOR_DRAW);

                    //UPDATE RELATIONS
                    //-------------------------------------------------------------------------------------------
                    //alan bayad tamame yalhaye (ghire barothery==outer) marboot be gerehaye hide shode midfy be father shavad+brothers hazf, baghie ham bashand

                    //baraye har father hame brothers yalha hazf==be raveshe null based
                    //NULL  BAASE HAST HA HAVASET BASHE
                    CLONE_relations_FOR_DRAW = remove_first_from_second_collection(sumofinternals, CLONE_relations_FOR_DRAW); //togh ham shamel mishe

                    //Remove nulls
                    CLONE_relations_FOR_DRAW = RemoveNullsFromList(CLONE_relations_FOR_DRAW);

                    //hala outers hara modify mikonim

                    //now sumofouters is modified, apply it to relations
                    CLONE_relations_FOR_DRAW = apply_first_to_second_collection_ID_BASED(SumOfOuters, CLONE_relations_FOR_DRAW);
                    //-------------------------------------------------------------------------------------------

                    //in tekrary ast  /to khode extract bezarim???
                    List<GeneralPolygonBase> all_childs = gui.key2command.appstate.traverse_tree_and_get_total_childs_with_specific_relationtype(original_shapes[i], gui.key2command.get_nested_relation_value());
                    all_childs.RemoveAt(0); //remove the root iteself
                    shapes_tohide.AddRange(all_childs);

                }
            }
            //!!!!ACCESS BE APPSATATE RA BEBIN VAAAAAAAAAAAAY
            //   List<Ipolygon> all_childs_of_collapsed_node = gui.key2command.appstate.traverse_tree_and_get_total_childs_with_specific_relationtype(shapes[i], gui.key2command.get_nested_relation_value());

            //dont draw childrens
            //    for (int k = 1; k < all_childs_of_collapsed_node.Count(); k++)//1 to ignore the root


            //khode child ra hide mikonim
            //  if (((rectangle)shapes[i]).collapse_state == true)

            //   shapes_tohides.Add(a_child); //????? badan behseh mikhorim 2 bare va tekrari ham tooshe


            //haminja relation haye oon child ha ra ham modify mikonim be father point she
            //relationha be modify ehtiaje
            //???!!!!!!!!!1deghat shavad arc haye ba biron majmoe!!!brpthers ham na



            // directed_relation temp = outerrelationsANDreverse[r]; //mishod parameter ham dad
            //node haye baghie brothers ham ghate inhast
            //?????!!!!Arc child arc chi mishe????? bayad hide she





            //------------------
            //extract toshow from tohides
            UtilityandGeometryMath utilgeomery = new UtilityandGeometryMath();
            visible_shape___cache = utilgeomery.remove_first_from_second_collection(shapes_tohide, original_shapes);

            //------------------
            //draw only non hides
            draw_all_nodes(g, visible_shape___cache, gui.get_selected_node_id());
            //hatman arc baad node ta maloom bashe nested expanded
            //draw arcs=extract all arcs between 2 nodes:
            //alan dige faght ba ToDraw node haye visible karmiknim

            //draw all relations 
            for (int i = 0; i < visible_shape___cache.Count; i++)
            {
                for (int j = i; j < visible_shape___cache.Count; j++) //togh ham shamel mishe
                {
                    GeneralPolygonBase node1 = visible_shape___cache[i];
                    GeneralPolygonBase node2 = visible_shape___cache[j];
                    List<rrelation> all_directed_relation_between_two_nodes = all_directed_relation_between_two_nodes_and_reverse(CLONE_relations_FOR_DRAW, node1, node2);
                    draw_all_relation_between_2_nodes_border_point_based(gui.get_nested_relation_value(), node1, node2, all_directed_relation_between_two_nodes);

                }
            }



            //
            //List<rrelation> result = new List<rrelation>();
            //foreach (rrelation r in CLONE_relations_FOR_DRAW)
            //{
            //    GeneralPolygonBase node1 = visible_shape___cache[i];
            //    GeneralPolygonBase node2 = visible_shape___cache[j];

            //    draw_all_relation_between_2_nodes_border_point_based(gui.get_nested_relation_value(), node1, node2, all_directed_relation_between_two_nodes);

              

               // r.SourceNode_id == node1.GetID()
                

                //if (( && r.ToTargetNode_id == node2.GetID()) || (r.SourceNode_id == node2.GetID() && r.ToTargetNode_id == node1.GetID()))
                //{
                //    result.Add(r);
                //}
           // }

           



            //moshekele dovom: be insorat hal ke arc az labe original_shape start draw beshe na az center aan, ta rooye shkel ra kharab nakone
            //draw_handle_based_relation_with_policy(g, rel); 

        }
       
        List<directed_relation> remove_first_from_second_collection(List<directed_relation> first, List<directed_relation> second)
        {
            List<directed_relation> temp = new List<directed_relation>();
            for (int i = 0; i < second.Count(); i++)
            {
                bool was = false;
                for (int j = 0; j < first.Count(); j++)
                {
                    if (second[i].getID() == first[j].getID())
                    {
                        was = true;
                        break;
                    }
                }

                if (!was) temp.Add(second[i]);
            }
            return temp;
        }
        List<directed_relation> apply_first_to_second_collection_ID_BASED(List<directed_relation> first, List<directed_relation> second)
        {

            //list avali ra to domomi search mikone age bood replace mikoneh(apply modified element to originals)

            for (int j = 0; j < second.Count; j++) //togh ham shamel mishe
            {

                for (int i = 0; i < first.Count; i++)
                {

                    if (second[j].getID() == first[i].getID())
                    {
                        second[j] = first[i]; //???? membrwise?
                    }
                }

            }
            return second;
        }
        List<rrelation> all_directed_relation_between_two_nodes_and_reverse(List<directed_relation> all_directed_relatios, GeneralPolygonBase node1, GeneralPolygonBase node2)
        {
            //????slef relation chi??felan age node 1 va nodde 2 mosavi bashe shamel mishe??
            List<rrelation> result = new List<rrelation>();
            foreach (rrelation r in all_directed_relatios)
            {
                if ((r.SourceNode_id == node1.GetID() && r.ToTargetNode_id == node2.GetID()) || (r.SourceNode_id == node2.GetID() && r.ToTargetNode_id == node1.GetID()))
                {
                    result.Add(r);
                }
            }

            return result;
        }
        #endregion
        List<directed_relation> RemoveNullsFromList(List<directed_relation> Collection)
        {

            // Remove nulls
            List<directed_relation> temp = new List<directed_relation>();
            for (int i = 0; i < Collection.Count; i++)
            {
                if (Collection[i] != null)
                {
                    temp.Add(Collection[i]);
                }

            }
            return temp;

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            gui.from1loaded();
                       
            //*********!!!!!!!in bayad bere dar command ...
            //foreach (AppData.scenario s in gui.scenariolist())
            //listBox1.Items.Add(s.GetScenarioName());
                
        }

     


      

       
     
    }
}
