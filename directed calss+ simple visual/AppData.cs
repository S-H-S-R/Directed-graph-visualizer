using DirectedGraphClass;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text; 
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    #region definition
   

    ////////////////////////full app state+visual and graph data serialized////////////
    //we made all appdata serializable 
    [Serializable]
    class AppData
    {
      
        //==========data==========
        //setting: don't change this value unless update db data also
        public string nested_relation_value { get { return "nested"; } }// readonly 


        //todo: integrety must check between these: ke har do baham nabashe chon faghat 1 shape select mitone besehe
        public int selected_relation_id  { get { return DATA.selected_relation_id; } set { DATA.selected_relation_id = value;  } }
        public int selected_node_id { get { return DATA.selected_node_id; } set { DATA.selected_node_id = value; } }


        //parameter buffer techniqe: for relation and add child to father relation (nested) creation and ...== commands that have 2 params
        public int sourcsid  { get { return DATA.sourcsid; } set { DATA.sourcsid = value;  } }
        public int targetid { get { return DATA.targetid; } set { DATA.targetid = value; } }



        public state current_state { get { return DATA.s; } set { DATA.s = value; } }
        public directed_graph graph { get { return DATA.graph; } set { DATA.graph = value; } } //was serializable 

        public int currentzoomlevel { get { return DATA.zoomlevel; } set { DATA.zoomlevel=value; } }  

         dataonly DATA=null;
        
        public enum state { s1, s2, s3, s4, s5, s6 }
     

       
        [Serializable]

        public class dataonly
        {

             //todo uniqe id ra implement kon baraye system labels
            //system label list available for attach to shapes or relations+int is uniqe id
            internal Dictionary<int, string> LabelList = new Dictionary<int, string>();
            //--------------------------
            internal int zoomlevel = 0;
            internal int selected_node_id=-1;
            internal int selected_relation_id=-1;
            internal int sourcsid=-1;
            internal int targetid=-1;
            internal state s= state.s1;
            internal directed_graph graph=new directed_graph();



           

        }


      


        internal bool addnewsystemlabel(string newvalue)
        {

            if (!DATA.LabelList.ContainsValue(newvalue)) //doesn't add if its already there
            {
                DATA.LabelList.Add(DATA.LabelList.Count, newvalue);
                return true;
            }

            return false;


        }

        internal void saveshapelabels(int shape_id, Dictionary<int, bool> thisshapelabels, string data)
        {
            GeneralPolygonBase polgygon = find_shape_by_id(shape_id);
            polgygon.Data = data;
            polgygon.labels = thisshapelabels;
        }


        internal void updatesystemlabel(Dictionary<int, string> newlabels)
        {
            DATA.LabelList = newlabels;


        }

        public Dictionary<int, string> getsystemlabels()
        {

          return  DATA.LabelList;
           
        }
        internal void removelabelbykey(int key)
        {
            DATA.LabelList.Remove(key);

            foreach (rrelation r in graph.AllRelations)
            {
                Dictionary<int, bool> syncedexisted = new Dictionary<int, bool>();
                foreach (var pair in r.systemlabelsID)
                {
                    if (DATA.LabelList.ContainsKey(pair.Key))
                    {
                        syncedexisted.Add(pair.Key, pair.Value);

                    }
                }
                r.systemlabelsID = syncedexisted;


            }



            foreach (GeneralPolygonBase pol in graph.AllNodes)
            {
                Dictionary<int, bool> syncedexisted = new Dictionary<int, bool>();
                foreach (var pair in pol.systemlabelsID)
                {
                    if (DATA.LabelList.ContainsKey(pair.Key))
                    {
                        syncedexisted.Add(pair.Key, pair.Value);

                    }
                }
                pol.systemlabelsID = syncedexisted;
            }


           
        }
        internal void removelabel(int newlabel)
        {
             

            DATA.LabelList.Remove(newlabel);
           
           
        }

        internal bool renamelabelbykey(int key, string newvalue)
        {



            if (!DATA.LabelList.ContainsValue(newvalue)) //doesn't rename if there is another label with this name
            {

                DATA.LabelList[key] = newvalue;





                return true;
            }

            return false;





           
        }
        internal void updatelabel(int selectedinx, string newtext)
        {
            DATA.LabelList[selectedinx] = newtext;
        }
        //===============================






        //===============================
        public string currentpath;
        internal int scenarioframeindex=0;

        public void reloadalldata(dataonly data, string path)
        {            
            this.DATA = data;
            this.currentpath = path;
             
        }

        public bool isdatabaseloaded()
        {
            if (currentpath != null) return true;
            return false;
        }
        public AppData()
        {
            this.DATA = null;
            this.currentpath = null;
        }

        internal void unloaddata()
        {
            this.DATA = null;
            this.currentpath = null;
           
        }

        internal List<directed_relation> GetAllRelations()
        {
            List<directed_relation> temp = new List<directed_relation>();

            foreach (directed_relation pol in graph.AllRelations)
            {
                temp.Add(pol);
            }
            return temp;
        }

       

        internal int GetAllNodecounts()
        {

            return graph.AllNodes.Count();
        }

        internal GeneralPolygonBase FindShapeByIndexNotId(int i)
        {
            return   (GeneralPolygonBase)graph.AllNodes[i];
        }

      
        internal List<GeneralPolygonBase> GetAllShapes()
        {  
           
            List<GeneralPolygonBase> temp = new List<GeneralPolygonBase>();

            foreach (GeneralPolygonBase pol in graph.AllNodes) //oon typesh node vali oon polygon ast
            {
                temp.Add(pol);  
            } 
            return temp;
        }
              

        internal void ChangeSelectedNodeORRelationDataAndLabels(string newdata, Dictionary<int, bool> newlist, Color color)
        { 
            
            GeneralPolygonBase selected_node = find_shape_by_id(selected_node_id);
            rrelation selected_relation = find_relation_by_id(selected_relation_id);
            if (selected_node != null)
            {
                selected_node.Data = newdata;
                selected_node.systemlabelsID= newlist;
                selected_node.color = color;
            }
            if (selected_relation != null)
            {
                selected_relation.data = newdata;
                selected_relation.systemlabelsID= newlist;
                selected_relation.color = color;
            }
 
        }

       
        void savefileNoOverwrite(string sourceFullAdress, string basesavepath, string targetfilename)
        {  //kari kon ba copy ham beshe
            try
            {
                File.Copy(sourceFullAdress, basesavepath + targetfilename, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //File.WriteAllBytes(@"C:\Users\hamid\Desktop\11.txt", e.Data.GetData("FileName"));

        }

        AppData.dataonly loadfromdisk(string path)
        {
            
            
            try
            {
                // if (File.Exists(path))
                {

                    BinaryFormatter binFormat = new BinaryFormatter();

                    using (Stream fStream = File.OpenRead(path))
                    {
                        return (AppData.dataonly)binFormat.Deserialize(fStream);
                    }
                }
              //  else
               
            }
            catch (Exception ex)
            {
                
            }
            //===========================
            return null;

        }
        internal bool tryopendb(string path)
        { 
            AppData.dataonly data = loadfromdisk(path);
            if (data != null) {

                reloadalldata(data, path);
                //globalsetting.configdata.defaultDBpath = path;
                return true;
            }

            return false;

        }

        internal bool trycreatenedb(string path)
        {
            AppData.dataonly data = new dataonly();
            return savetodisk(path, data);
        }

        internal void applycurrentzoomstateONLYNEWRELATION(rrelation newrelation, int scalefactor) //faghat baraye relation haye jadid in call mishe
        {
            for (int i = 0; i < Math.Abs(DATA.zoomlevel); i++)
            {

                if (DATA.zoomlevel < 0)
                {
                    newrelation.scaledown(scalefactor);

                }
                else if (DATA.zoomlevel > 0)
                {

                    newrelation.scaleup(scalefactor);
                }
                else //zoom level is zero
                {
                    //no need apply current zoom level

                }

            }
        }

        public  dataonly getdata()
        {
            return this.DATA;
        }
       

        //=============================================
         

        public bool savetodisk(string path, AppData.dataonly data)
        {
            
             
            //todo ??? owerite mikone: asaan saving risky ast in code khobnis. moshkeli nist!!

            
            try
            {
                //if (File.Exists(filename))
                

                BinaryFormatter binFormat = new BinaryFormatter();
                //Store object in a local file.

                //chon khatarnake va data corrupt momkene beshe filemode.open kardim faghat
                using (Stream fStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    binFormat.Serialize(fStream, data);
                }

            }
            catch (Exception ex)
            {
                return false;

              
            }
            return true;

        }

        public List<rrelation> all_directed_relation_between_two_nodes_both_direction(GeneralPolygonBase node1, GeneralPolygonBase node2)
        {
            //todo : togh self relation children age node 1 va nodde 2 mosavi bashe shamel mishe??
            List<rrelation> result = new List<rrelation>();
            foreach (rrelation r in graph.AllRelations)
            {

                if ((r.SourceNode_id == node1.GetID() && r.ToTargetNode_id == node2.GetID()) || (r.SourceNode_id == node2.GetID() && r.ToTargetNode_id == node1.GetID()))
                {
                    result.Add(r);
                }
            }

            return result;
        }
        public List<GeneralPolygonBase> traverse_tree_and_get_total_childs_with_specific_relationtype(GeneralPolygonBase root_start_node, string relationtype)
        {

            //returns (non recursive based tree traversal) all deep total nodes of a tree: shamele root start ham mishe alave bar childrens of levels to leaves
            //relation hayee ke dar nazar migire faghat hierachil ast (overlay tree)

            List<GeneralPolygonBase> results = new List<GeneralPolygonBase>();
            //todo check not null or exist start node ke nakardi            
            List<GeneralPolygonBase> queue = new List<GeneralPolygonBase>();
            queue.Add(root_start_node);

            //todo rootNode.visited = true;  ???az visited estefadeh nemikonim (az queue hazf mikonim) age tree bashe => farz tree ast vagarna bayad visit estefadeh beshe age cycle dare
            while (queue.Count > 0)
            {
                results.Add(queue[0]);
                //find all hierachilal children of last element of queue and put them in queue
                List<GeneralPolygonBase> all_childrens = find_all_input_shapes_with_specific_relation(queue[0], relationtype);
                foreach (GeneralPolygonBase child in all_childrens)
                {
                    queue.Add(child);
                }
                //kareman ba in node tamoom va hazfesh mikonim az visisted list               
                queue.RemoveAt(0);
            }
            return results;

        }

        //public List<GeneralPolygonBase> traverse_tree_and_get_total_Fathers_with_specific_relationtype(GeneralPolygonBase root_start_node, string relationtype)
        //{

        //    //returns (non recursive based tree traversal) all deep total nodes of a tree: shamele root start ham mishe alave bar childrens of levels to leaves
        //    //relation hayee ke dar nazar migire faghat hierachil ast (overlay tree)

        //    List<GeneralPolygonBase> results = new List<GeneralPolygonBase>();
        //    //todo check not null or exist start node ke nakardi            
        //    List<GeneralPolygonBase> queue = new List<GeneralPolygonBase>();
        //    queue.Add(root_start_node);

        //    //todo rootNode.visited = true;  ???az visited estefadeh nemikonim (az queue hazf mikonim) age tree bashe => farz tree ast vagarna bayad visit estefadeh beshe age cycle dare
        //    while (queue.Count > 0)
        //    {
        //        results.Add(queue[0]);
        //        //find all hierachilal children of last element of queue and put them in queue
        //        List<GeneralPolygonBase> all_childrens = find_all_input_shapes_with_specific_relation(queue[0], relationtype);
        //        foreach (GeneralPolygonBase child in all_childrens)
        //        {
        //            queue.Add(child);
        //        }
        //        //kareman ba in node tamoom va hazfesh mikonim az visisted list               
        //        queue.RemoveAt(0);
        //    }
        //    return results;

        //}

        internal void movescreen(int _newx, int _newy, int _originalx, int _originaly)
        {//todo  momkene slow bashe chon hame shape hara mive mikonim shayd behtar faghat visible ha load bashaad baraye optimization


            List<GeneralPolygonBase> temp = new List<GeneralPolygonBase>();

            foreach (GeneralPolygonBase pol in graph.AllNodes) //oon typesh node vali oon polygon ast
            {
                pol.Locationx += _newx - _originalx;
                pol.Locationy += _newy - _originaly;
            }

        }

        public void applycurrentzoomstateONLYNEWSHAPES(GeneralPolygonBase pol, int scalefactor) //faghat baraye plogon haye jadid in call mishe
        {
            //be tedade martabee(zoomlevel) ke zoom shode app, in shape ham roosh apply mishe
            for (int i = 0; i < Math.Abs(DATA.zoomlevel); i++)
            {

                if (DATA.zoomlevel < 0)
                {
                    pol.scaledown_shape(scalefactor, true);

                }
                else if (DATA.zoomlevel > 0)
                {

                    pol.scaleup_shape(scalefactor, true);
                }
                else //zoom level is zero=default
                {
                    //no need apply current zoom level

                }

            }
        }

        public void movescreenbydelta(int deltax, int deltay)
        {//todo  momkene slow bashe chon hame shape hara mive mikonim shayd behtar faghat visible ha load bashaad baraye optimization


            List<GeneralPolygonBase> temp = new List<GeneralPolygonBase>();

            foreach (GeneralPolygonBase pol in graph.AllNodes) //oon typesh node vali oon polygon ast
            {
                pol.Locationx += deltax;
                pol.Locationy += deltay;
            }

        }
        internal List<GeneralPolygonBase> findFathersOnelevel(GeneralPolygonBase polygon)
        {
            return find_all_output_shapes_with_specific_relation(polygon, nested_relation_value);
        }

        internal List<GeneralPolygonBase> findChildrensOnelevel(GeneralPolygonBase polygon)
        {
            return find_all_input_shapes_with_specific_relation(polygon, nested_relation_value);
        }
        public List<GeneralPolygonBase> find_all_input_shapes_with_specific_relation(GeneralPolygonBase node, string relation_label)
        {
            //todo behtare check konim start node, null nabashe ke nakardim felan
            List<GeneralPolygonBase> result = new List<GeneralPolygonBase>();
            foreach (rrelation rel in graph.InputRelations((node)node))
            {
                if (rel.type == relation_label)
                {
                    result.Add(find_shape_by_id(rel.SourceNode_id));

                }
            }
            return result;
        }
        public List<GeneralPolygonBase> find_all_output_shapes_with_specific_relation(GeneralPolygonBase shape, string relation_label)
        {
            //todo behtare check konim start node , null nabashe ke nakardim felan
            List<GeneralPolygonBase> result = new List<GeneralPolygonBase>();
            foreach (rrelation rel in graph.OutputRelations((node)shape))
            {
                if (rel.type == relation_label)
                {
                    result.Add(find_shape_by_id(rel.ToTargetNode_id));
                }
            }
            return result;
        }

        public rrelation find_relation_by_id(int my_id)
        {

            //todo PRECONDITION  // albate bayad hatman selected rel ha
            foreach (rrelation rel in graph.AllDirectedRelations)
            {
                if (rel.getID() == my_id)
                {
                    return rel;
                }

            }
            return null;


        }
        public bool if_any_node_selected()
        {
            return selected_node_id >= 0;
        }
        public bool if_any_relation_selected()
        {
            return selected_relation_id >= 0;
        }
        public GeneralPolygonBase find_shape_by_id(int my_id)
        { 
            //todo PRECONDITION  // albate bayad hatman selected bashe ha
            return ((GeneralPolygonBase)graph.SearchNodes(my_id)); 
        }        
        public void scaleupallshapes_ONE_LEVEL()
        {//todo  momkene slow bashe chon hame shape hara mive mikonim shayd behtar faghat visible ha load bashaad baraye optimization


            List<GeneralPolygonBase> temp = new List<GeneralPolygonBase>();

            foreach (GeneralPolygonBase pol in GetAllShapes()) //oon typesh node vali oon polygon ast
            {
                pol.scaleup_shape(globalsetting.scalefactor, true/* move location*/); 
                                  // pol.Locationx *= 2;
                                  // pol.Locationy *= 2;
            }

        }
        public void scaledownallshapes_ONE_LEVEL()
        {//todo  momkene slow bashe chon hame shape hara mive mikonim shayd behtar faghat visible ha load bashaad baraye optimization


            List<GeneralPolygonBase> temp = new List<GeneralPolygonBase>();

            foreach (GeneralPolygonBase pol in GetAllShapes()) //oon typesh node vali oon polygon ast
            {
                pol.scaledown_shape(globalsetting.scalefactor, true/* move location*/);//todo go to setting
                              
            }

        }








        public void recursive_tree_SCAN(List<GeneralPolygonBase> array, GeneralPolygonBase start)
        {
             
            array.Remove(start);
            // bring start
            graph.AllNodes.Remove(start);
            graph.add_new_node_if_safe(start);
            //temp.RemoveAt(temp.Count - 1);// ghadimitarin ra hazf


            GeneralPolygonBase directortop = null;
            if (array.Count > 0)
                directortop = array[array.Count - 1]; 

           


            List<GeneralPolygonBase> allchilds = findChildrensOnelevel(start);

            if (allchilds.Count > 0)
                allchilds.Remove(directortop);
             

            

            var sorteddictionary = new SortedDictionary<int, GeneralPolygonBase>();          
            foreach (GeneralPolygonBase child in allchilds)
            {
                sorteddictionary.Add(graph.AllNodes.IndexOf(child),child); 
            }
            //-----------------
            foreach (KeyValuePair<int, GeneralPolygonBase> pair in sorteddictionary)

            {
                
                recursive_tree_SCAN(array, pair.Value); 

            }



           


            if (directortop != null)
            {
              
                recursive_tree_SCAN(array, directortop);
            }
            //--------------


            


        }
        public void bringNodeandFathersandChildrensToFront(int selected_node_id, string nested_relation_value)
        {

            //also returns node itself + jadid tare zirin tare => index=0  nazdiktarin pedar

            //aval father miarim jelooye list chon z-index shan kamtare va bayad zirtar draw beshe
            GeneralPolygonBase mynode = find_shape_by_id(selected_node_id);
            List<GeneralPolygonBase> result = new List<GeneralPolygonBase>();
            result.Add(mynode);

            //tebghe ghanoon tak pedari 1 pedar mide
            List<GeneralPolygonBase> temp = findFathersOnelevel(mynode);
            while (temp.Count >= 1)
            {
                result.Add(temp[0]);
                temp = findFathersOnelevel(temp[0]);
            }



            recursive_tree_SCAN(result, result[result.Count - 1]);

        }
       

       





        public void scaledownallrelations_ONE_LEVEL()
        {
            //todo  momkene slow bashe chon hame shape hara mive mikonim shayd behtar faghat visible ha load bashaad baraye optimization


            List<rrelation> temp = new List<rrelation>();

            foreach (rrelation rel in GetAllRelations()) //oon typesh node vali oon polygon ast
            {
                rel.scaledown(globalsetting.scalefactor);
                // pol.Locationx *= 2;
                // pol.Locationy *= 2;
            }
        }
        public void scaleupallrelations_ONE_LEVEL()
        {
            //todo  momkene slow bashe chon hame shape hara mive mikonim shayd behtar faghat visible ha load bashaad baraye optimization


            List<rrelation> temp = new List<rrelation>();

            foreach (rrelation rel in GetAllRelations()) //oon typesh node vali oon polygon ast
            {
                rel.scaleup(globalsetting.scalefactor);
                // pol.Locationx *= 2;
                // pol.Locationy *= 2;
            }
        }

        //internal List<scenario> scenariolist()
        //{
        //    return DATA.scenario;
        //}
    }
    
    #endregion

}
