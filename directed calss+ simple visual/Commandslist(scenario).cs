using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO; 
using System.Runtime.Serialization.Formatters.Binary;
using System.Text; 
using System.Windows.Forms;

namespace WindowsFormsApplication1
{




    //class saveable_usecase
    //{
    //    public string name;
    //    public void step1() { }
    //    public void step2() { }
    //    public void continuee()
    //    {

    //    }

    //}

    //class saveable_usecase_undo
    //{
    //    saveable_usecase usecase;
    //    var undolist;


       

    //    public void step1() {  usecase.step1(); push.undolist }
    //    public void step2() { usecase.step2(); push.undolist}
    //    public void continuee()
    //    {
    //        usecase.continuee();
    //    }

    //}

    //class saveable_usecase_undolist_gui
    //{
    //    saveable_usecase_undo usecase_undo;
        

    //    public void step1() { MessageBox.Show(""); usecase_undo.step1(); MessageBox.Show(""); }
    //    void void   step2()    {   MessageBox.Show(""); usecase_undo.step2(); MessageBox.Show(""); }
    //    public void continuee()
    //    {
    //        usecase_undo.continuee();
    //    }

    //}

    //class saveable_usecase
    //{
    //    public string name;

    //    List<Icommand> commands = new List<Icommand>(); //or steps


    //    public void continuee()
    //    {

    //    }

    //}

    //class total_usecase //gui uses this to show liast of continuable usecases
    //{d
    //    List<saveable_usecase> list_usecase = new List<saveable_usecase>();
    //    public void savealltodisk() { }
    //    public void loadallfromdisk() { }

    //    public List<string> getusecaselist() {

    //        List<string> temp = new List<string>();
    //        foreach (var item in list_usecase)
    //        {
    //            temp.Add(item.name);
    //        }
    //        return temp;

    //    }

    //    public void addusecase(saveable_usecase usecase)
    //    {
    //        list_usecase.Add(usecase);
    //    }
    //    public void continueeUsecaseByIndex(int i)
    //    {
    //        //list_usecase[i].loadfromdisk();
    //        list_usecase[i].continuee(); 
    //    }

    //}



    public abstract class Icommand
    {
        //pure inerface ham mishe bashe

        public void log(string msg)
        {
            //Console.Clear()
            //Console.Write(msg +"\r\n");
            //koja log konim?????
            //?matne payam ha che konim baraye central ya edit ya tarjome lazem nist
        }
        public abstract void doo();//age parame haye mokhtalef dahti chi? be constructure midim. constructure chizie mesle frame work khodesh ejra mishe.
       // public abstract void undo(); 
    }

    //????dar command ha hey log tekrary darim mesle refresh bebar dar frame work+mishe mesle dependency injection ek module dige bedanad text log har command chie va command ra pass bede frame oonja.vali bayad gom nshe in ertebat va bedanim command jadid koja edit bayad konim

    class commandslist 
        { 
         public static  AppData appdata; 
          
        public class NoAction : Icommand //????Icommand , command   khode inam inheritance tekrare mishe ba generickard?+mitone ersnabare va log nakone...+ elzam ke hame object ha erse apppolicy hadeagh az: IpolocyCenter ta oon bege az che chizhayee dige bayad ers bebare
        { 
            //cause only refresh
            public NoAction()
            {

            }

            public override void doo()
            {
                base.log("No Actions");
            }


        }
        
        public class reset_target_source_add_relation_request : Icommand
        { 
            public reset_target_source_add_relation_request()
            {

            }

            public override void doo()
            {
                base.log("Reset target and source node. Deselected: "+ appdata.sourcsid.ToString()+", "+ appdata.targetid.ToString());
                appdata.targetid = appdata.sourcsid = -1;
               
            }


        }
        public class deselect_all_things : Icommand
        {
           
            public deselect_all_things()
            {
                
            }

            public override void doo()
            {
                base.log("Deselect all things. Relation: " + appdata.selected_relation_id.ToString() + ", Node: " + appdata.selected_node_id.ToString());
                appdata.selected_relation_id = -1;
                appdata.selected_node_id = -1;              
            } 

        }

        

        public class remove_node : Icommand
        {
            GeneralPolygonBase _selected_node;//aya vaghean refrenece ast????
            List<GeneralPolygonBase> _all_tree_nodes;
            
            public remove_node(GeneralPolygonBase selected_node, List<GeneralPolygonBase> all_tree_nodes)
            {
                this._selected_node = selected_node;
                this._all_tree_nodes = all_tree_nodes;
               
            }

            public override void doo()
            {
                //chon hierachial darim khode node va all childs ham hazf mishe
                base.log("Remove shape. Shape id: " + _selected_node.GetID().ToString());

                foreach (GeneralPolygonBase a_child in _all_tree_nodes)
                {
                    appdata.graph.RemoveNodeWithRelations((DirectedGraphClass.node)a_child);
                }

                appdata.targetid = appdata.sourcsid = -1;
                appdata.selected_node_id = -1;
                appdata.selected_relation_id = -1;
               

            }
              

        }


        public class remove_hierachial_relation : Icommand
        {
            DirectedGraphClass.directed_relation _todel;//aya vaghean refrenece ast????

            public remove_hierachial_relation(DirectedGraphClass.directed_relation todel)
            {
                this._todel = todel;
            }

            public override void doo()
            {
                base.log("Remove hierachial relation. Relation id: "+ _todel.getID().ToString());

                appdata.graph.RemoveRelation(_todel);
                appdata.selected_node_id = -1;
                appdata.selected_relation_id = -1;
               
            }
              
        }
         
        public class remove_relation : Icommand
        {
            rrelation _relation;//aya vaghean refrenece ast????
            
            public remove_relation(rrelation relation)
            {
                this._relation = relation;
            }

            public override void doo()
            {
                 base.log("Remove relation. Relation id: "+ _relation.getID());
                appdata.graph.RemoveRelation(_relation);
                appdata.selected_node_id = -1;
                appdata.selected_relation_id = -1;
              

            }
             
        }

         
        public class newrelation : Icommand
        {
          
            public newrelation()
            {
                
            }

            public override void doo()
            { 
                int id = appdata.graph.generate_relation_id();
                rrelation temp = new rrelation(id, id.ToString(), appdata.sourcsid, appdata.targetid);
                //todo  ???rel type ra felan id gozashti ta different basehe
                appdata.applycurrentzoomstateONLYNEWRELATION(temp, globalsetting.scalefactor);
                appdata.graph.add_new_directrelation_if_safe(temp);
                //deselect source and target
                appdata.targetid = appdata.sourcsid = -1;
                base.log("New ralation. Id: "+ id.ToString());
            } 

        }

        public class add_hierachial_relation : Icommand
        {
            GeneralPolygonBase _fathernode;
            GeneralPolygonBase _childnode;
            Point _location;
            public add_hierachial_relation(Point location,GeneralPolygonBase childnode, GeneralPolygonBase fathernode)
            {
                
                this._childnode = childnode;
                this._fathernode = fathernode;
                this._location = location;
            }

            public override void doo()
            {
                int id = appdata.graph.generate_relation_id();
                //add child relation to father
                appdata.graph.add_new_directrelation_if_safe(new rrelation(id, appdata.nested_relation_value, appdata.sourcsid, appdata.targetid));
                //find all (deep graph search)childs of this node to also move to  new  location its childs along with father,apply to all nested childrens 
                List<GeneralPolygonBase> all_tree_nodes = appdata.traverse_tree_and_get_total_childs_with_specific_relationtype(_childnode, appdata.nested_relation_value);

                int original_x = _childnode.Locationx;
                int original_y = _childnode.Locationy;
                //child ra mibarim center of father
                _childnode.Locationx = _location.X;
                _childnode.Locationy = _location.Y;
                //farzandane (all deep subchild=nested childs) amigh ra ham mesle child hamoon ghadr move mikonim
                for (int i = 1; i < all_tree_nodes.Count; i++)//faghta childs ,root shamel naknim=> j=1 ...
                {
                    GeneralPolygonBase a_child_node = appdata.find_shape_by_id(all_tree_nodes[i].GetID());//???ba cast nemishod efficent tar
                    a_child_node.Locationx += _childnode.Locationx - original_x;//or newx-original_x
                    a_child_node.Locationy += _childnode.Locationy - original_y;//or newy-original_y
                }


                //done, deselect the selected nodes
                appdata.targetid = appdata.sourcsid = -1;
                //appdata.bringNodeandChildrensToFront(this._fathernode.GetID(), appdata.nested_relation_value);
               
                base.log("Add hierachial realtion. Id: "+ id.ToString());
            }
             
        }
         
      
        public class newnode_rhombus : Icommand
        { 
            public override void doo()
            {
                int id = appdata.graph.generate_node_id();
              
                GeneralPolygonBase temp = new rhombus(id, 50, 200);

                appdata.applycurrentzoomstateONLYNEWSHAPES(temp, globalsetting.scalefactor);
                appdata.graph.add_new_node_if_safe((DirectedGraphClass.node)temp);
                base.log("New rhombus. Id: "+ id.ToString());
            } 
        }

       


        public class newnode_circle : Icommand
        {
            public override void doo()
            {
                int id = appdata.graph.generate_node_id();
               
                GeneralPolygonBase temp = new circle(id, 50, 200,50);
                //  temp.filePath =@"C:\Users\lenovo\Desktop\g.png";
                appdata.applycurrentzoomstateONLYNEWSHAPES(temp, globalsetting.scalefactor);
                appdata.graph.add_new_node_if_safe((DirectedGraphClass.node)temp);
                base.log("New circle. Id: " + id.ToString());
            }
        }

        //public class clone_circle : Icommand
        //{
        //    circle _target;
        //    public clone_circle(circle target)
        //    {
        //        _target = target;
        //    }
        //    public override void doo()
        //    {
        //        int id = appdata.graph.generate_node_id();

        //        GeneralPolygonBase temp = new circle(id, 50, 200, 50);
        //        temp.connection_points = _target.connection_points;

        //        appdata.applycurrentzoomstateONLYNEWSHAPES(temp, globalsetting.scalefactor);
        //        appdata.graph.add_new_node_if_safe((DirectedGraphClass.node)temp);
        //        base.log("clone circle. Id: " + id.ToString());
        //    }
        //}
        //public class clone : Icommand
        //{
        //    Type _type;
        //    public clone(Type type)
        //    {
        //        _type = type;
        //    }
        //    public override void doo()
        //    {
        //        int id = appdata.graph.generate_node_id();
        //        GeneralPolygonBase temp = new GeneralPolygonBase(id,0,0); 
        //        temp = (GeneralPolygonBase)Activator.CreateInstance(_type);

        //        appdata.applycurrentzoomstateONLYNEWSHAPES(temp, globalsetting.scalefactor);
        //        appdata.graph.add_new_node_if_safe((DirectedGraphClass.node)temp);
        //        base.log("New rectangle. Id: " + id.ToString());
        //    }
        //}

        public class newnode_rectangle : Icommand
        {
            public override void doo()
            {
                int id = appdata.graph.generate_node_id();
               
                GeneralPolygonBase temp = new rectangle(id, 50, 200);
                appdata.applycurrentzoomstateONLYNEWSHAPES(temp, globalsetting.scalefactor);
                appdata.graph.add_new_node_if_safe((DirectedGraphClass.node)temp);
                base.log("New rectangle. Id: "+id.ToString());
            }
        }


        //public class attach_image_to_node : Icommand
        //{
        //    GeneralPolygonBase _selected_node;//aya vaghean refrenece ast????
        //    Image _image;
        //    public attach_image_to_node(GeneralPolygonBase selected_node, Image image)
        //    {
        //        this._selected_node = selected_node;
        //        this._image = image;
        //    }

        //    public override void doo()
        //    {
        //        _selected_node.Image = _image;
        //        base.log("Attach image to node. Node id: "+ _selected_node.GetID().ToString());
        //    }
        //}

        public class  attach_image_to_relation : Icommand
       {
        rrelation _selected_relation;//aya vaghean refrenece ast????
        Image _image;
        public attach_image_to_relation(rrelation selected_relation, Image image)
        {
            this._selected_relation = selected_relation;
            this._image = image;
        }

        public override void doo()
        {
            _selected_relation.image = _image;
             base.log("Attach image to relation. Relation id: "+ _selected_relation.getID().ToString());
            }
             
      } 
        public class moveshape : Icommand
        {
            int _newx;
            int _newy;
            int _originalx;
            int _originaly;
            public moveshape(int newx, int newy,  int originalx, int originaly)
            {

                this._newx= newx;
                this._newy= newy;
                this._originalx= originalx;
                this._originaly= originaly;
            }

            public override void doo()
            {
                //in father movement, find childs of this node to also move to  new  location its childs along with father,apply to all nested childrens (moving)
                List<GeneralPolygonBase> all_childrens_nodes = appdata.traverse_tree_and_get_total_childs_with_specific_relationtype(appdata.find_shape_by_id(appdata.selected_node_id), appdata.nested_relation_value);
                //az root==current node ra ye bar kardim pas az 1 start konim
                for (int i = 1; i < all_childrens_nodes.Count; i++)//root ham, shamel mishe
                {
                    //childeren niz be andaze delata pedar move mishe
                    all_childrens_nodes[i].Locationx += _newx - _originalx;//or newx-original_x
                    all_childrens_nodes[i].Locationy += _newy - _originaly;//or newy-original_y
                }
                
               
                base.log("Drag shape and childs. Id: "+ appdata.selected_node_id.ToString());
            }


        }
     
        public class select_relation : Icommand
        {
            int _selected_relation_id;
            public select_relation(int selected_relation_id)
            {
                this._selected_relation_id = selected_relation_id;

            }

            public override void doo()
            {
                appdata.selected_relation_id = _selected_relation_id;
                appdata.selected_node_id = -1;
                base.log("Select relation. Id: "+ _selected_relation_id.ToString());

            }


        }
        public class bringNodeandFathersandChildrensToFront : Icommand
        {
            int _selected_node_id;
          
            public bringNodeandFathersandChildrensToFront(int selected_node_id , string  nested_relation_value)
            {
                this._selected_node_id = selected_node_id;

                //bring selected node and its childrens to front
                appdata.bringNodeandFathersandChildrensToFront(selected_node_id, nested_relation_value );
                

            }

            public override void doo()
            {
                appdata.selected_relation_id = -1; //deselect relation
                appdata.selected_node_id = _selected_node_id;
                base.log("Select shape. Id: "+ _selected_node_id.ToString());
            }


        }
         

        public class SaveDataExitApp : Icommand
        { 
            public SaveDataExitApp( )
            {
                 
            }
            public override void doo()
            { 

            }
            public bool trysave(string path, AppData.dataonly data)
            {
                bool result = appdata.savetodisk(path, data); 
                base.log("Tried save data and exit app. result was:  "+ result.ToString());
                return result;


            }
             
        }
         
        public class trySaveDataWithoutExit  : Icommand
        {

            public trySaveDataWithoutExit()
            {

            }
            public override void doo()
            {

            }
            public bool trysave(string path, AppData.dataonly data)
            {
                bool result = appdata.savetodisk(path, data);
                base.log("Tried save data without exit app. result was:  " + result.ToString());
                return result;

            }

        }

        public class ChangeSelectedNodeORRelationDataORlabels : Icommand
        { 
            string _newdata;
            Dictionary<int, bool> _newlist;
            Color _color;
            public ChangeSelectedNodeORRelationDataORlabels(string newdata, Dictionary<int, bool> newlist, Color color)
            { 
                this._newdata = newdata;
                this._newlist = newlist;
                this._color = color;
            }

            public override void doo()
            {
                appdata.ChangeSelectedNodeORRelationDataAndLabels(_newdata, _newlist,_color);
                base.log("Data changed of shape or relation");
            }


        }




        //------------------------------------------------
        internal class decrease_shape_text_width : Icommand
        {
            private GeneralPolygonBase selected_node;

            public decrease_shape_text_width(GeneralPolygonBase selected_node)
            {
                this.selected_node = selected_node;
            }

            public override void doo()
            {
                selected_node.decrease_text_area_width();

            }
        }

        internal class increase_shape_text_width : Icommand
        {
            private GeneralPolygonBase selected_node;

            public increase_shape_text_width(GeneralPolygonBase selected_node)
            {
                this.selected_node = selected_node;
            }
            public override void doo()
            {
                selected_node.increase_text_area__width();

            }

        }

        internal class decrease_shape_text_height : Icommand
        {
            private GeneralPolygonBase selected_node;

            public decrease_shape_text_height(GeneralPolygonBase selected_node)
            {
                this.selected_node = selected_node;
            }

            public override void doo()
            {
                selected_node.decrease_text_area__height();

            }
        }

        internal class increase_shape__text_height : Icommand
        {
            private GeneralPolygonBase selected_node;

            public increase_shape__text_height(GeneralPolygonBase selected_node)
            {
                this.selected_node = selected_node;
            }
            public override void doo()
            {
                selected_node.increase_text_area__height();

            }

        }

        //-----------------------------------------------
        internal class decrease_relation_text_width : Icommand
        {
            private rrelation selected_relation;

            public decrease_relation_text_width(rrelation selected_relation)
            {
                this.selected_relation = selected_relation;
            }

            public override void doo()
            {
                selected_relation.decrease_text_area_width();

            }
        }

        internal class increase_relation_text_width : Icommand
        {
            private rrelation selected_relation;

            public increase_relation_text_width(rrelation selected_node)
            {
                this.selected_relation = selected_node;
            }
            public override void doo()
            {
                selected_relation.increase_text_area__width();

            }

        }

        internal class decrease_relation_text_height : Icommand
        {
            private rrelation selected_relation;

            public decrease_relation_text_height(rrelation selected_relation)
            {
                this.selected_relation = selected_relation;
            }

            public override void doo()
            {
                selected_relation.decrease_text_area__height();

            }
        }

        internal class increase_relation_text_height : Icommand
        {
            private rrelation selected_relation;

            public increase_relation_text_height(rrelation selected_relation)
            {
                this.selected_relation = selected_relation;
            }
            public override void doo()
            {
                selected_relation.increase_text_area__height();

            }

        }
        //----------------------------------------------
        public class movescreen : Icommand
        {
            int _newx;
            int _newy;
            int _originalx;
            int _originaly;
            public movescreen(int newx, int newy, int originalx, int originaly)
            {

                this._newx = newx;
                this._newy = newy;
                this._originalx = originalx;
                this._originaly = originaly;
            }

            public override void doo()
            {

                appdata.movescreen(_newx, _newy, _originalx, _originaly);

                base.log("screen move " + appdata.selected_node_id.ToString());
            }


        }

        

     




    }

}

 