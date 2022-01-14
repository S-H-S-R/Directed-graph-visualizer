using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DirectedGraphClass;

namespace WindowsFormsApplication1
{
    class Gui
    {
        public Form1 form1;
        public Form2 form2;
        public Form3 form3;
               Form4 form4; //??public or private   //instance renew mishe+inja khodesh initialize mikoneh lazem be wiring dar app start nist
        public Key2Commands key2command;

       
        //-------------------------------
        public string get_nested_relation_value() {
            return key2command.get_nested_relation_value();
        }
        public int get_selected_node_id() {
            return key2command.get_selected_node_id();
           }

        public  string SelectDataBaseLocation()
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = globalsetting.openfilter; 
            openFileDialog1.Title = globalsetting.opentitle; 
            DialogResult r = openFileDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                return openFileDialog1.FileName;
            }

            {
                return "";
            }

        }

        internal void loadDBcanceled()
        {
             form4.Hide();
           
        }

        internal void selecanotherDB()
        { 

            string path = SelectDataBaseLocation();

            form4.Dispose();//.Hide(); //!!!!! tartib kheili moheme

            key2command.TryOpenDBAndSetDefault(path);
        }

        internal void tryagainloadDB(string path)
        {
            
            form4.Hide();
            key2command.TryOpenDBAndSetDefault(path);
        }

      

        internal void form2closed(string newdata, Dictionary<int, bool> newlabellist, Color color)
        {
           
            
            Icommand cmd = key2command.DataChangedForShapeORrelation(newdata, newlabellist, color);
            if (cmd != null)
            {  //hamchin commandi application exsit and  rule violation nakarde pas mishe ajra kard
                cmd.doo();
             
                form1.Refresh();
            }
        }

        internal bool tryaddnewuniquelabel(string newvalue)
        {
           return key2command.appstate.addnewsystemlabel(newvalue);
        }

        internal void KeyDown(object sender, KeyEventArgs e)
        {
            Icommand cmd = key2command.KeyDown(sender, e);
            if (cmd!=null)
            {  //hamchin commandi application exsit and rule violation nakarde pas mishe ajra kard
                cmd.doo();
                            
                form1.Refresh();
            }
        }

    
        internal void saveshapelabelstoDB(int shape_id, Dictionary<int, bool> thisshapelabels, string data)
        {
            key2command.appstate.saveshapelabels(shape_id, thisshapelabels, data);
        }

        internal void form3closed(Dictionary<int, string> systemlabels)
        {
            key2command.appstate.updatesystemlabel(systemlabels);
        }

        internal void MouseMove(object sender, MouseEventArgs e)
        {    
            Icommand cmd = key2command.MouseMove(sender, e);
            if (cmd != null)
            {  //hamchin commandi exsit and  rule violation nakarde
                cmd.doo();
              
                form1.Refresh();
            } 
        }

        internal void removesystemlabel(int selectedinx)
        {
            key2command.appstate.removelabel(selectedinx);
        }

        public void MouseDown(object sender, MouseEventArgs e)
        {          
            Icommand cmd =  key2command.MouseDown(sender, e);
            if (cmd != null)
            {  //hamchin commandi application exsit and  rule violation nakarde pas mishe ajra kard
                cmd.doo();
             
         
                form1.Refresh();
            }
        }

        internal void from1loaded()
        {

            Icommand cmd = key2command.from1loaded();
            if (cmd != null)
            {  
                cmd.doo();


                form1.Refresh();
            }

        }

        internal void MouseUp(object sender, MouseEventArgs e)
        { 
            Icommand cmd = key2command.MouseUp(sender, e);
            if (cmd != null)
            {  //hamchin commandi application exsit and  rule violation nakarde pas mishe ajra kard
                cmd.doo();
               
                form1.Refresh();
            }
        }

        internal void updatesystemlabel(int selectedinx, string newlabeltext)
        {
            key2command.updatelabel(selectedinx, newlabeltext);
        }



        internal Color WhichColorRelation(rrelation rel)
        {
            if (rel.getID() == key2command.get_selected_relation_id())
            {
                return SystemColorPolicy.SelectedSateColor;
            }
            else
            {
                return rel.color;
            }
        }

        internal List<rrelation> all_directed_relation_between_two_nodes_and_reverse(GeneralPolygonBase node1, GeneralPolygonBase node2)
        {
            return key2command.all_directed_relation_between_two_nodes_and_reverse(node1, node2);
        }
        internal GeneralPolygonBase FindShapeByIndexNotId(int i)
        {
            return key2command.FindShapeByIndexNotId(i);
        }
        internal GeneralPolygonBase FindShapeById(int i)
        {
            return key2command.FindShapeById(i);
        }
        internal int GetAllNodecounts()
        {
            return key2command.GetAllNodecounts();
        } 
        internal List<GeneralPolygonBase> GetAllShapes()
        {
           return key2command.GetAllShapes();
        } 
        internal void form1Closing(FormClosingEventArgs ee)
        { 
            //khoruj az app ra in module taeen mikoneh
                 

            bool? cmd = key2command.form1Closing(); //oon tasmim migire
            if (cmd==true)     //safe exit
            {
                form1.drawmode = false;
                 Application.Exit();
                //be inja nemirese app  exit mishe on success
                              
            }
            else if (cmd==false) // problem on save
            {
                show_message(globalsetting.db_save_problem(key2command.appstate.currentpath));
                //cmd.doo();  //kari nemikone
                //form1.Refresh(); nemikhad
            }
            else// cmd==null
            {  //exit canceled
                ee.Cancel = true;
            }

        }
        
        public  void show_message(string msg)
        {
            MessageBox.Show(msg);

        }
        internal void show_form2editor(int shapeid,string olddata,  Dictionary<int, string>  systemlabels, Dictionary<int, bool> thisshapelabels, Color color)
        {
            //inham mishod command konim az key2command beporsim vali ui command patttern ya roll back niaz nadare 
           // this.form2 = new Form2();



            //pass data to form
            this.form2.data = olddata;
            this.form2.allsystemlabels = systemlabels;
            this.form2.thisshapelabels = thisshapelabels;
            this.form2.shap_id = shapeid;
            this.form2.color = color;

            form2.ShowDialog();
        }


        internal void show_form3editor(Dictionary<int, string> systemlabels)
        {
            //inham mishod command konim az key2command beporsim vali ui command patttern ya roll back niaz nadare 
            // this.form3 = new Form3();

            
            this.form3.allsystemlabels = systemlabels; 
            form3.ShowDialog();
           


        }


        public rrelation find_relation_by_click_FROM_CACHE(int x, int y)
        {
            //nahve tashkhis relation click: az mahale click ta line kamtarin fasele az minimum kamtar bood oon line select: yani nazdik line relation click konim aan relation select mishe

            //faghat avali peyda return, be mikone overlap kari nadare felan...

            foreach (Cache_computed_relations data in form1.non_hierachial_arc_graphic_data___cashe)
            {
                //mibinim click ba kodom yek az parekhat haye sazandeye relation ha nazdike => ann relation select mishe

                PointF fake = new PointF(0, 0);

                UtilityandGeometryMath geometryMath = new UtilityandGeometryMath();
                double distance_click_to_middlepart =geometryMath.IsPointNearArc(new PointF(x, y), data.middlepoint1, data.middlepoint2, out fake);
                double distance_click_to_leftpart = geometryMath.IsPointNearArc(new PointF(x, y), data.startpoint, data.middlepoint1, out fake);
                double distance_click_to_rightpart = geometryMath.IsPointNearArc(new PointF(x, y), data.middlepoint2, data.endpoint, out fake);
                int distance = 5;//required distance to togh to select it
                //fasele click ba yeki az 3 part togh kamtar bod kole togh entekhab mishe:
                if (distance_click_to_middlepart < distance || distance_click_to_leftpart< distance || distance_click_to_rightpart<distance)
                {
                    return data.relation;
                }
            }
            return null;

        }

        internal int getzoomlevel()
        {
            return key2command.getzoomlevel();
        }

        

        internal bool AskUserApproveDiscardOldData()
        {
            DialogResult result = MessageBox.Show(globalsetting.AskUserApproveDiscardOldData(), globalsetting.ApproveDiscardOldDataCaption(), MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                return true;
            }
          
            else
            {
                return false;
            }
        }

        public GeneralPolygonBase find_shape_by_click_FROM_VISIBLE_SHAPE_CACHE(int x, int y)
        {

            //for optimization return on first answer+ serach from higher z index
             
            //faghat avali jost return mikone be overlap kari nadare

           // List<GeneralPolygonBase> shapes = form1.visible_shape___cache; //not nessseary reference copy

          // for (int i =0 ; i < form1.visible_shape___cache.Count; i++)

           for (int i= form1.visible_shape___cache.Count()-1; i >= 0; i--)            
            {
                if (form1.visible_shape___cache[i].is_point_on_shape(x, y)) { return form1.visible_shape___cache[i]; }
            }

            return null;
        }

        internal List<directed_relation> GetAllRelations()
        {
            return key2command.GetAllRelations();
        }

        internal bool AskUserApprove(string messaage)
        {
            DialogResult result = MessageBox.Show(messaage, ""/* globalsetting.delete_caption()*/, MessageBoxButtons.YesNo);
           
            if (result.ToString()=="Yes") //??button labael baraye farsi ...custom kon
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        internal bool? AskUserApproveSaveBeforDatabaseUnload(string path)
        {
            //??button labael baraye farsi ...custom kon
           
            
            DialogResult result = MessageBox.Show(globalsetting.ask_save_before_unload(path), globalsetting.save_before_unload_caption(), MessageBoxButtons.YesNoCancel);
            if (result == DialogResult.Cancel)
            {
                return null;
            }
            else if (result == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            } 
           
        }
        internal string SelectDataBaseLocation4Save()
        {
            SaveFileDialog savefile = new SaveFileDialog();

            savefile.Filter = globalsetting.savefilter; 
            savefile.Title = globalsetting.saveTitle; 

            System.Windows.Forms.DialogResult r = savefile.ShowDialog();
            if (r == System.Windows.Forms.DialogResult.OK)
            {
                return savefile.FileName;
            }

            {
                return "";
            }
        }

        internal List<GeneralPolygonBase> traverse_tree_and_get_total_childs_with_specific_relationtype(GeneralPolygonBase node, string v)
        {
            return key2command.traverse_tree_and_get_total_childs_with_specific_relationtype(node, v);
        }


        internal bool AskUserApproveGeneralDelete()
        {
           
            DialogResult result = MessageBox.Show(globalsetting.ask_sure_delete(), globalsetting.delete_caption(), MessageBoxButtons.YesNo);

            if (result.ToString() == "Yes") //??button labael baraye farsi ...custom kon
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

        internal void show_form4(string path, string errormessage)
        {


            //form4.Close();
            form4 = new Form4();
          
            form4.gui = this;
            form4.db_path = path;
            form4.errormessage = errormessage;
            //form4.Visible = false;
           
            form4.ShowDialog();
           
        }

      

       

        //internal List<AppData.scenario> scenariolist()
        //{
        //    return key2command.scenariolist();
        //}
    }
     
    public class Cache_computed_relations
    {
         
        public Cache_computed_relations(rrelation myrel, Point mystartpoint, Point mymiddlepoint1, Point mymiddlepoint2, Point myendpoint)
        { 
            this.startpoint = mystartpoint;
            this.middlepoint1 = mymiddlepoint1;
            this.middlepoint2 = mymiddlepoint2;
            this.endpoint = myendpoint; 
            this.relation = myrel;
        }

        
        public Point startpoint;
        public Point middlepoint1;
        public Point middlepoint2;
        public Point endpoint;
        public rrelation relation;
    }
     
}
 
 
  