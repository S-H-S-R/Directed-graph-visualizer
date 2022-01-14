using DirectedGraphClass;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.IO;

namespace WindowsFormsApplication1
{
    class Key2Commands
    {     
        //-----------communicate with other components----------------
        public Gui gui;
        public AppData appstate;
        
        //------------------------------------------------------------
        
        int i, j;
           
        public int get_selected_relation_id() { return appstate.selected_relation_id;  }
        public int get_selected_node_id()  { return appstate.selected_node_id;   }
        public string get_nested_relation_value() { return appstate.nested_relation_value;  }
        public List<GeneralPolygonBase> GetAllShapes()
        {
           
            return appstate.GetAllShapes();
        }
        public List<directed_relation> GetAllRelations()
        {
            return appstate.GetAllRelations();
        }
        public GeneralPolygonBase FindShapeByIndexNotId(int i)
        {
            return appstate.FindShapeByIndexNotId(i);
        }
        public int GetAllNodecounts()
        {
            return appstate.GetAllNodecounts();
        }
        public GeneralPolygonBase FindShapeById(int node_id)
        {
            return appstate.find_shape_by_id(node_id);
        }
        public List<rrelation> all_directed_relation_between_two_nodes_and_reverse(GeneralPolygonBase node1, GeneralPolygonBase node2)
        {
            return appstate.all_directed_relation_between_two_nodes_both_direction(node1, node2);
        }
        //make response to events
        public Icommand DataChangedForShapeORrelation(string newdata, Dictionary<int, bool> newLabelList, Color color)
        {
            return new commandslist.ChangeSelectedNodeORRelationDataORlabels(newdata, newLabelList, color);
        }

        public bool? AskSaveingCurrentDatabase()
        {
            //-------------safely unload current database----------
            if (appstate.isdatabaseloaded())
            {
                bool? result = gui.AskUserApproveSaveBeforDatabaseUnload(appstate.currentpath);
                if (result == null)
                {
                    //other data base loading canceled
                    return null;
                }
                else if (result == true)
                {   //save changes of curent db before unload it
                    commandslist.trySaveDataWithoutExit cmd = new commandslist.trySaveDataWithoutExit();
                    bool saveresult = cmd.trysave(appstate.currentpath, appstate.getdata());
                    if (!saveresult)
                    {
                        gui.show_message(globalsetting.db_save_problem(appstate.currentpath)); return false;
                    }
                    else
                    {
                        //save was successful
                    }
                }
                else
                {
                    //no need save changes
                }
                
            } //nothing is loaded to ask save
           
            return true;
        }

        internal bool tryrenamelabelbykeyremainunique(int key, string text)
        {
           return appstate.renamelabelbykey(key, text);
        }

        internal void removelabelbykey(int key)
        {

            if(!gui.AskUserApprove(globalsetting.AskSureDeleteLabel())) { return; }
            appstate.removelabelbykey(key);
        }


          
        string getclipartfileaddress()
        {
            IDataObject data_object = Clipboard.GetDataObject();
            string[] files = null; ;
            // Look for a file drop.
            if (data_object.GetDataPresent(DataFormats.FileDrop))
            {
                files = (string[])data_object.GetData(DataFormats.FileDrop);
            }

            return files[0];

        }


        internal Icommand from1loaded()//!!!!bokon ready to render, nabayad bedanad form1 hast+baraye gereftam param hanagoo form2 bego getparamofcircle() ke khdesh be commandline ya form2 tabdil mikoneh baraye in faghat daryaft va eersal param moheme...
        {
                string path = globalsetting.getdefaultdbpath();
                if (path == "" || path == null) return null;//??? empty string in config means not default
                return TryOpenDBAndSetDefault(path);
        }


        internal void updatelabel(int selectedinx, string newlabelvalue)
        {
            appstate.updatelabel(selectedinx , newlabelvalue);
           
        }


        public Icommand KeyDown(object sender, KeyEventArgs e)
        {//this is like router in php frameworks




            //clone selected shape types
           // if (e.KeyCode == Keys.C)
           // {
                //if (appstate.if_any_node_selected())//first be sure a shape is selected
                //{   //find the selected shape
                //    GeneralPolygonBase selected_node = appstate.find_shape_by_id(appstate.selected_node_id);

                   // return new commandslist.clone(selected_node.GetType());
                    //GeneralPolygonBase temp = new GeneralPolygonBase();
                    //Type type=selected_node.GetType();
                    //GeneralPolygonBase temp = (GeneralPolygonBase)Activator.CreateInstance(type);

                    // MessageBox.Show(type.ToString());
                    // GeneralPolygonBase temp = new type;
                    // return new commandslist.newnode_circle();
                //}
          //  }

















            // scale up only one shape
            if (e.KeyCode == Keys.Multiply)
            {
                if (appstate.if_any_node_selected())//first be sure a shape is selected
                {
                    //find the selected shape
                    GeneralPolygonBase selected_node = appstate.find_shape_by_id(appstate.selected_node_id);
                    //first apply the scaleup   
                    selected_node.scaleup_shape(globalsetting.scalefactor, false/*dont move location*/);

                    

                    //  dar collpase agar small konim chon childrenhash invisible hastan rule nemikhad ke nazane biron
                    if (selected_node.current_collapse_state == true) { return new commandslist.NoAction(); }

                    //child nazane biron az father ....
                    UtilityandGeometryMath geometry = new UtilityandGeometryMath();
                    List<GeneralPolygonBase> all_childs = gui.key2command.appstate.traverse_tree_and_get_total_childs_with_specific_relationtype(selected_node, gui.key2command.get_nested_relation_value());
                    all_childs.RemoveAt(0); //remove the root iteself

                    List<GeneralPolygonBase> fathers = appstate.findFathersOnelevel(selected_node);
                    bool result = geometry.IS_ALL_RULES_OK(selected_node, fathers, all_childs);



                    if (!result)
                    //recover
                    { //delat movement is zero in zoom
                        selected_node.scaledown_shape(globalsetting.scalefactor, false/*dont move location*/);//pas dar command pattern param ha ham hide dar khodesh chon param ha motafavete to do() yeksan dashte bashim+pas param command ba conxtructor pass midim
                                                                                               //rule vioaltion => rollback
                                                                                           //return null;  //rule violation
                    }
                    else
                    {
                        //selected_node.scaleup_shape(globalsetting.scalefactor, false/*dont move location*/); ;//pas dar command pattern param ha ham hide dar khodesh chon param ha motafavete to do() yeksan dashte bashim+pas param command ba conxtructor pass midim


                    }


                    return new commandslist.NoAction();//cause only refresh 
                }

            }
            // scale down only one shape
            if (e.KeyCode == Keys.Divide)
            {
                if (appstate.if_any_node_selected())//first be sure a shape is selected
                {
                    //find the selected shape
                    GeneralPolygonBase selected_node = appstate.find_shape_by_id(appstate.selected_node_id);
                    //first apply the scaleup   
                     selected_node.scaledown_shape(globalsetting.scalefactor, false/*dont move location*/);
                    
                    

                    //  dar collpase agar small konim chon childrenhash invisible hastan rule nemikhad ke nazane biron
                    if (selected_node.current_collapse_state == true) { return new commandslist.NoAction(); }

                    //child nazane biron az father ....
                    UtilityandGeometryMath geometry = new UtilityandGeometryMath();
                    List<GeneralPolygonBase> all_childs = gui.key2command.appstate.traverse_tree_and_get_total_childs_with_specific_relationtype(selected_node, gui.key2command.get_nested_relation_value());
                    all_childs.RemoveAt(0); //remove the root iteself

                    List<GeneralPolygonBase> fathers = appstate.findFathersOnelevel(selected_node);
                    bool result = geometry.IS_ALL_RULES_OK(selected_node, fathers, all_childs);



                    if (!result)
                    //recover
                    { //delat movement is zero in zoom
                        selected_node.scaleup_shape(globalsetting.scalefactor, false/*dont move location*/);//pas dar command pattern param ha ham hide dar khodesh chon param ha motafavete to do() yeksan dashte bashim+pas param command ba conxtructor pass midim
                        //rule vioaltion => rollback
                                            //return null;  //rule violation
                    }
                    else
                    { 
                       // selected_node.scaledown_shape(globalsetting.scalefactor, false/*dont move location*/); ;//pas dar command pattern param ha ham hide dar khodesh chon param ha motafavete to do() yeksan dashte bashim+pas param command ba conxtructor pass midim
                        
                        
                    }


                    return new commandslist.NoAction();//cause only refresh 

                }

            }

            //zoom in all the scene shapes
            if (e.KeyCode == Keys.Add)
            {
                draw_text.systemfontsize *= globalsetting.scalefactor;
                appstate.currentzoomlevel++;//!!!IN  BAYAD AVAL BIAD
                   appstate.scaleupallshapes_ONE_LEVEL();
                   appstate.scaleupallrelations_ONE_LEVEL();
                   return new commandslist.NoAction();//cause only refresh 
                
            }


            //zoom out all the scene shapes
            if (e.KeyCode == Keys.Subtract)
            {

                draw_text.systemfontsize /= globalsetting.scalefactor;
                appstate.currentzoomlevel--;//!!!IN  BAYAD AVAL BIAD
                appstate.scaledownallshapes_ONE_LEVEL();
                appstate.scaledownallrelations_ONE_LEVEL();
                return new commandslist.NoAction();//cause only refresh   
            }


            // Look for Ctrl+V.
            if (e.Control && (e.KeyCode == Keys.V))
            {

                UtilityandGeometryMath.util_folder_or_file util = new UtilityandGeometryMath.util_folder_or_file();
                if (appstate.if_any_node_selected())//or state == s2  or s3
                {
                    GeneralPolygonBase temp = appstate.find_shape_by_id(get_selected_node_id());

                    if (temp.getfilepath() != null && temp.getfilepath() != "" && !gui.AskUserApproveDiscardOldData())
                    {
                        return null;
                    }
                    else
                    {
                            string sourcefilepath = getclipartfileaddress();
                            try
                            {
                                 
                                if (temp.getfilepath() != null && temp.getfilepath() != "") util.deletefileORdirectory(temp.getfilepath());                                
                                string newpath= util.try_copy_folder_or_file(sourcefilepath, Application.StartupPath);
                                if (newpath=="") { gui.show_message(globalsetting.problem_file__or_directiry_copy(sourcefilepath)); return null; }
                                temp.setfilepath(newpath);
                            }
                            catch (Exception ex)
                            {
                                //todo roll back nadarom
                                gui.show_message(globalsetting.problem_file__or_directiry_copy(sourcefilepath));
                            }


                         
                    }
                  

                }
                //if arc selected
                else if (appstate.if_any_relation_selected())//or state == s2  or s3
                {
                    rrelation temp = appstate.find_relation_by_id(get_selected_relation_id());

                    if (temp.getfilepath() != null && temp.getfilepath() != "" && !gui.AskUserApproveDiscardOldData())
                    {
                        return null;

                    }
                    else {
                        
                        {
                            string sourcefilepath = getclipartfileaddress();
                            try
                            {
                                if (temp.getfilepath() != null && temp.getfilepath() != "") util.deletefileORdirectory(temp.getfilepath());
                             
                                string newpath  = util.try_copy_folder_or_file(sourcefilepath, Application.StartupPath);
                                if (newpath=="") { gui.show_message(globalsetting.problem_file__or_directiry_copy(sourcefilepath)); return null; }
                                temp.setfilepath(newpath);
                            }
                            catch (Exception ex)
                            {
                                gui.show_message(globalsetting.problem_file__or_directiry_copy(sourcefilepath));
                                //todo roll back nadarom
                            }

                        }
                    }
                }


                // Get the DataObject.
              
                return new commandslist.NoAction();
            }

            //delete the attachment file of a shape
            if (e.KeyCode == Keys.Delete)
            {
                UtilityandGeometryMath.util_folder_or_file util = new UtilityandGeometryMath.util_folder_or_file();
                string filepath = "";
                //if node selected
                if (appstate.if_any_node_selected())//or state == s2  or s3
                {
                    try
                    {
                        GeneralPolygonBase temp = appstate.find_shape_by_id(get_selected_node_id());
                        filepath = temp.getfilepath();
                        if (filepath != null && filepath != "" && !gui.AskUserApproveDiscardOldData()) { return null; }
                        if (!util.deletefileORdirectory(filepath)) { gui.show_message(globalsetting.problem_file_delete(filepath)); return null; }
                        temp.setfilepath("");
                    }
                    catch (Exception ex)
                    {
                        //todo roll back nadarom
                        gui.show_message(globalsetting.problem_file__or_directiry_copy(filepath));
                    }
                }
                //if arc selected
                else if (appstate.if_any_relation_selected())//or state == s2  or s3
                {
                    try
                    {
                        rrelation temp = appstate.find_relation_by_id(get_selected_relation_id());
                        filepath = temp.getfilepath();
                        if (filepath != null && filepath != "" && !gui.AskUserApproveDiscardOldData()) { return null; }
                        if (!util.deletefileORdirectory(filepath)) { gui.show_message(globalsetting.problem_file_delete(filepath)); return null; }
                        temp.setfilepath("");
                    }
                    catch (Exception ex)
                    {
                        //todo roll back nadarom
                        gui.show_message(globalsetting.problem_file__or_directiry_copy(filepath));
                    }
                } 

                return new commandslist.NoAction();
            }

             

            //lauch the attachment file of a shape
            if (e.KeyCode == Keys.Enter)
            {

                 



                //string[] a=  File.ReadAllLines("A2.1.txt");
                //for (int i = 0; i < 100; i++) {

                //    int id = appstate.graph.generate_node_id();

                //    GeneralPolygonBase temp = new circle(id, 50, 200, 50);

                //    temp.Data = a[i];
                ////  temp.filePath =@"C:\Users\lenovo\Desktop\g.png";
                //appstate.applycurrentzoomstateONLYNEWSHAPES(temp, globalsetting.scalefactor);
                //    appstate.graph.add_new_node_if_safe((DirectedGraphClass.node)temp);



                //}



                string filepath="";
                //if node selected
                if (appstate.if_any_node_selected())//or state == s2  or s3
                {
                    filepath = appstate.find_shape_by_id(get_selected_node_id()).getfilepath();
                    
                }
                //if arc selected
                else if (appstate.if_any_relation_selected())//or state == s2  or s3
                {
                    filepath = appstate.find_relation_by_id(get_selected_relation_id()).getfilepath();
                   
                }


                //------------------
                try
                {
                    if( filepath !=null && filepath!=""  )  Process.Start(filepath);
                }
                catch (Exception ex)
                {
                    gui.show_message(globalsetting.problemfilelaunch(filepath));
                }

                return null;            


            }

             


                //decrease any shape and relation wdth of textarea
              if (e.KeyCode == Keys.Left)
               {
                if (appstate.if_any_node_selected())//first be sure a shape is selected
                {
                    //find the selected shape
                    GeneralPolygonBase selected_node = appstate.find_shape_by_id(appstate.selected_node_id);
                          
                    return new commandslist.decrease_shape_text_width(selected_node);
                }

                else if (appstate.if_any_relation_selected())
                {
                    rrelation selected_rel = appstate.find_relation_by_id(appstate.selected_relation_id);
                    return new commandslist.decrease_relation_text_width(selected_rel);

                }

                    





            }
             //increase any shape and relation wdth of textarea
            if (e.KeyCode == Keys.Right)
                {
                if (appstate.if_any_node_selected())//first be sure a shape is selected
                {
                    //find the selected shape
                    GeneralPolygonBase selected_node = appstate.find_shape_by_id(appstate.selected_node_id);
                    //first apply the zoom                 
                    Icommand increase_width = new commandslist.increase_shape_text_width(selected_node);
                    return increase_width;

                }
                else if (appstate.if_any_relation_selected())
                {
                    rrelation selected_rel = appstate.find_relation_by_id(appstate.selected_relation_id);
                    return new commandslist.increase_relation_text_width(selected_rel);

                }

           
                

                }


            //  //decrease any shape and relation height of textarea
            if (e.KeyCode == Keys.Down)
                {
                if (appstate.if_any_node_selected())//first be sure a shape is selected
                {
                    //find the selected shape
                    GeneralPolygonBase selected_node = appstate.find_shape_by_id(appstate.selected_node_id);
                    //first apply the zoom                 
                    Icommand decrease_height = new commandslist.decrease_shape_text_height(selected_node);
                    return decrease_height;
                }
                else if (appstate.if_any_relation_selected())
                {
                    rrelation selected_rel = appstate.find_relation_by_id(appstate.selected_relation_id);
                    return new commandslist.decrease_relation_text_height(selected_rel);

                }
               
             
                    

                }


             //increase any shape and relation height of textarea
            if (e.KeyCode == Keys.Up)
                {
                if (appstate.if_any_node_selected())//first be sure a shape is selected
                {
                    //find the selected shape
                    GeneralPolygonBase selected_node = appstate.find_shape_by_id(appstate.selected_node_id);
                    //first apply the zoom                 
                    Icommand increase_height = new commandslist.increase_shape__text_height(selected_node);
                    return increase_height;

                }
                else if (appstate.if_any_relation_selected())
                {
                    rrelation selected_rel = appstate.find_relation_by_id(appstate.selected_relation_id);
                    return new commandslist.increase_relation_text_height(selected_rel);

                }

               
                 

            }





                //toggle collpase|expand of shape
                if (e.KeyCode == Keys.Space)
                {
                    if (appstate.if_any_node_selected())
                    {
                        //?????/inhara badan be command tabdil kon
                        GeneralPolygonBase selected_node = appstate.find_shape_by_id(appstate.selected_node_id);
                       

                        if (selected_node.current_collapse_state)
                        { 
                            //firt apply expand
                            selected_node.expand();


                            //check rules: e.g this az fahther biron nare...
                            UtilityandGeometryMath geometry = new UtilityandGeometryMath();
                            List<GeneralPolygonBase> all_childs = gui.key2command.appstate.traverse_tree_and_get_total_childs_with_specific_relationtype(selected_node, gui.key2command.get_nested_relation_value());
                            all_childs.RemoveAt(0); //remove the root iteself

                            List<GeneralPolygonBase> fathers = appstate.findFathersOnelevel(selected_node);
                            bool result = geometry.IS_ALL_RULES_OK(selected_node, fathers, all_childs);


                            if (!result) //rules violated, so rollback
                            {
                                //roleback
                                if (selected_node.current_collapse_state)
                                {

                                    selected_node.expand();
                                }
                                else
                                {
                                    selected_node.collapse();
                                }
                                // return null; //nothing to do or rule violated

                            }
                        }
                        else
                        {
                            selected_node.collapse();
                        }

                        





                  } //only for refresh
                  return new commandslist.NoAction();
              }



                //save current database to disk
                if (e.KeyCode == Keys.S)  //mitone complex event mix condition bashe ghire mostagheem inja bayad khabardar she.+ aya haman event driven mishe?? chon listen(event register) be avamel mojood dar conditional expression kone+har khate barname dar jaye dige yek event tolid mikone ya har action ...ya har rule true shodan...
                {
                    commandslist.trySaveDataWithoutExit savecommand = new commandslist.trySaveDataWithoutExit(); //dasti ham mishod valo command kardim albate be sharte adame niaz be module haye dige
                    bool r = savecommand.trysave(appstate.currentpath, appstate.getdata());
                    if (r)
                    {
                        return savecommand; //not importnant
                    }
                    else
                    {
                        //todo Mohem hatman option bede jaye dige save konim napare
                        gui.show_message(globalsetting.problem_file_delete(appstate.currentpath));
                        return null; //"db-save-problem"

                    }
                }

                 

                //rooye harkodom select bood image az clipboard be onvane data attach va draw mishe
                //inja ham faght ya node ya faghat relation selected bayad bashe           
                //if (e.KeyCode == Keys.I)
                //{
                //    if (Clipboard.GetImage() == null) return null; //common precondition of other actions;nothinf todo

                //    if (appstate.if_any_node_selected())
                //    {
                //        GeneralPolygonBase temp = appstate.find_shape_by_id(appstate.selected_node_id);
                //        return new commandslist.attach_image_to_node(temp, Clipboard.GetImage());

                //    }
                //    else if (appstate.if_any_relation_selected())
                //    {
                //        rrelation temprel = appstate.find_relation_by_id(appstate.selected_relation_id);
                //        return new commandslist.attach_image_to_relation(temprel, Clipboard.GetImage());

                //    }
                //}

                //disconnect a node from father =>then child it remains in place but can be moved out of father space now
                if (e.KeyCode == Keys.J)
                {
                    //if some node selected 
                    if (appstate.if_any_node_selected())
                    {
                        GeneralPolygonBase child_node = appstate.find_shape_by_id(appstate.selected_node_id);
                        //if selected node has father
                        List<GeneralPolygonBase> rel_s_to_father = appstate.find_all_output_shapes_with_specific_relation(child_node, appstate.nested_relation_value);
                        //find nested relation exist between two node
                        directed_relation temp = null;

                        foreach (directed_relation dr in appstate.graph.AllRelations) //che kardi?????
                        {
                            if (rel_s_to_father.Count > 0 && dr.SourceNode_id == child_node.GetID() && dr.ToTargetNode_id == rel_s_to_father[0].GetID())
                            {
                                temp = dr; break; //faghat roo tari????????
                            }
                        }

                        if (temp != null)//faghat bayad 1 peyda shavad chon tebghe ghanoone tak pedari felan
                        {
                            appstate.current_state = AppData.state.s1;
                            return new commandslist.remove_hierachial_relation(temp);

                        }

                    }

                }


                //set hierachial: make some node childeren of other (father) node
                if (e.KeyCode == Keys.H)
                {
                    if (appstate.if_any_node_selected())
                    {
                        //not father or child is selected
                        if (appstate.sourcsid == -1 && appstate.targetid == -1)
                        {
                            appstate.sourcsid = appstate.selected_node_id;
                        }
                        //on child is selected before
                        else if (appstate.sourcsid > -1 && appstate.targetid == -1)
                        {
                            appstate.targetid = appstate.selected_node_id;
                            ////now child and father is selected
                            GeneralPolygonBase father_node = appstate.find_shape_by_id(appstate.targetid);
                            GeneralPolygonBase child_node = appstate.find_shape_by_id(appstate.sourcsid);

                            ///////////PRECONDITION CHECKING SECTION ///////////
                            bool all_preconditions = true;
                            //prevent hierachial arc to itself
                            if (appstate.targetid == appstate.sourcsid)
                            {
                                all_preconditions = false;
                                gui.show_message(globalsetting.cant_hierachial_itself());
                            }





                            //z index precondition== array based location based not a field==this affect rendering sequence
                            //tartib ajza dar list node haman z-index farz mikonim 
                            //!!!!BECAUSE IN THIS VERSION   ANY SHAPE SELECTED,ITS Z-INDEX BECOME TOP=> NO NEED TO THIS RULE: 
                                //if (child_node.GetID() <= father_node.GetID())
                                //{
                                    //all_preconditions = false;
                                //}






                            //ghanoone tak pedari
                            if (appstate.targetid != appstate.sourcsid && appstate.find_all_output_shapes_with_specific_relation(child_node, appstate.nested_relation_value).Count > 0)
                            {
                                all_preconditions = false;

                                gui.show_message(globalsetting.shape_only_single_father());
                            }
                            //-------------------------------------
                            //dar jahat mokhalef father az ghabl nabashe??????
                            List<GeneralPolygonBase> oldfathers = appstate.find_all_input_shapes_with_specific_relation(child_node, appstate.nested_relation_value);
                            foreach (GeneralPolygonBase i in oldfathers)
                            {
                                if (i.GetID() == appstate.targetid)
                                {
                                    all_preconditions = false;
                                    gui.show_message(globalsetting.father_can_not_child_of_his_child());

                                }
                            }
                            //-------------------------------------
                        //child area must be smaller(even not equal) than father to draw in father space
                        UtilityandGeometryMath geometry = new UtilityandGeometryMath();
                        Point? locatiob = geometry.bruteforce_can_firstshape_be_in_secondshape(child_node, father_node);
                        Point location = new Point();
                        if (locatiob == null)
                        {
                            all_preconditions = false;
                        }

                     




                        //-------------------------------------

                        ///////////PRECONDITION CHECKING END ///////////
                        if (all_preconditions) //now all preconditions are ture: 
                            {
                                location.X = locatiob.Value.X;
                                location.Y = locatiob.Value.Y;
                                Icommand cmd =new  commandslist.bringNodeandFathersandChildrensToFront(child_node.GetID(), get_nested_relation_value());
                                cmd.doo();
                                return new commandslist.add_hierachial_relation(location, child_node, father_node);

                                }
                                else
                                {
                                //done, deselect the selected nodes
                                 return new commandslist.reset_target_source_add_relation_request();
                                    //nothing to do
                                }

                        }
                    }

                }


                //load a database from disk
                if (e.KeyCode == Keys.O)
                {
                    //soal mishe save konim ...+ rule hich chizi uload nashe magar soaol
                    bool? asksaving = AskSaveingCurrentDatabase();
                    if (asksaving == null || asksaving == false) { return new commandslist.NoAction(); }

                    string path = gui.SelectDataBaseLocation();
                 

                    return TryOpenDBAndSetDefault(path);
                }

                //create new database
                if (e.KeyCode == Keys.N)
                {
                    //soal mishe save konim ...+ rule hich chizi uload nashe magar soaol
                    bool? asksaving = AskSaveingCurrentDatabase();
                    if (asksaving == null || asksaving == false) { return new commandslist.NoAction(); }

                    string path = gui.SelectDataBaseLocation4Save();
                    if (path == "" || path == null) { gui.show_message(globalsetting.invalid_db_path_to_create(path)); return new commandslist.NoAction(); }
                    if (!appstate.trycreatenedb(path)) { gui.show_message(globalsetting.db_create_problem(path)); return new commandslist.NoAction(); }


                    //load empty created database from disk:
                    if (!appstate.tryopendb(path)) { gui.show_message(globalsetting.db_opening_problem(path)); return new commandslist.NoAction(); }
                    gui.form1.drawmode = true;
                    return new commandslist.NoAction();
                }


                //create new relation
                if (e.KeyCode == Keys.R)
                {
                    if (appstate.if_any_node_selected())
                    {
                        //but not source or dest selected
                        if (appstate.sourcsid == -1 && appstate.targetid == -1)
                        {
                            appstate.sourcsid = appstate.selected_node_id;
                        }
                        //source is selected
                        else if (appstate.sourcsid > -1 && appstate.targetid == -1)
                        {
                            appstate.targetid = appstate.selected_node_id;
                            //now source and dest is selected                           
                            return new commandslist.newrelation();
                        }

                    }

                }




          


            //-------------------
                //create new shape instance from installed shape classes
                if (e.KeyCode == Keys.NumPad1)
                {
                     return new commandslist.newnode_circle();                                   

                }
                if (e.KeyCode == Keys.NumPad2)
                {
                    return new commandslist.newnode_rectangle();
                }
                if (e.KeyCode == Keys.NumPad3)
                {
                     return new commandslist.newnode_rhombus();
                      
                }




                 //----- Reset the scenario animation frame(step) to 0

                 if (e.KeyCode == Keys.NumPad7) {

                    ((rrelation)appstate.GetAllRelations()[appstate.scenarioframeindex]).color = SystemColorPolicy.DefaultUnelectedSateColor;
                    appstate.scenarioframeindex = 0;
                
                 }
    


               //----- Forwald the scenario animation frame(step) one step
                if (e.KeyCode == Keys.NumPad8)
                {
                  int index = appstate.scenarioframeindex;
                  int count=appstate.GetAllRelations().Count;

                 

                    if (index > 0) ((rrelation)appstate.GetAllRelations()[index - 1]).color = SystemColorPolicy.DefaultUnelectedSateColor;
                    ((rrelation)appstate.GetAllRelations()[index]).color = Color.Blue;


                    if(index < count-1) index++;

                    


                appstate.scenarioframeindex=index;
                return    new commandslist.NoAction();


                }


             //----- backward the scenario animation frame(step) one step
            if (e.KeyCode == Keys.NumPad9)
            {
                int index = appstate.scenarioframeindex;
                int count = appstate.GetAllRelations().Count;


                if (index > 0) ((rrelation)appstate.GetAllRelations()[index - 1]).color = Color.Blue; 
                ((rrelation)appstate.GetAllRelations()[index]).color = SystemColorPolicy.DefaultUnelectedSateColor;



                if (index > 0) index--;




                appstate.scenarioframeindex = index;
                return new commandslist.NoAction();


            }




            //delete selected node or an arc
            if (e.KeyCode == Keys.D)
                {
                    //if node selected
                    if (appstate.if_any_node_selected())//or state == s2  or s3.....
                    {
                        appstate.current_state = AppData.state.s1;
                        GeneralPolygonBase todelete = appstate.find_shape_by_id(appstate.selected_node_id);  // inhara ham khode command anjam bede???
                        List<GeneralPolygonBase> all_tree_nodes = appstate.traverse_tree_and_get_total_childs_with_specific_relationtype(todelete, appstate.nested_relation_value);
                        //???chera vasl nist be elate rule "destructive action need confirms" soalkardim+chetor shenasayee shod va confirm haminja ejra ya jaye dige rule ejra va command cancel ya delete nhayee be ma midadand 
                        if (gui.AskUserApproveGeneralDelete())
                        {
                            return new commandslist.remove_node(todelete, all_tree_nodes);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    //if arc selected
                    if (appstate.if_any_relation_selected())//or state == s2  or s3 ....
                    {
                        appstate.current_state = AppData.state.s1;
                        rrelation todel = appstate.find_relation_by_id(appstate.selected_relation_id);
                        if (gui.AskUserApproveGeneralDelete())
                        {
                            return new commandslist.remove_relation(todel);
                        }
                        else
                        {
                            return null;
                        }
                    }
                }

            //add/remove/edit definitions os system labels
            if (e.KeyCode == Keys.L)   
            {

             
                Dictionary<int, string> instaledsystemlabels = appstate.getsystemlabels(); 
                gui.show_form3editor(instaledsystemlabels);

             

               
                
                

            }

 



            //edit data and labels of a node or an relation
            if (e.KeyCode == Keys.E) //only for selected relation or shapes
                {

                   
                  //Dictionary<int, bool> current_selectedlabelsofshape_rel;
                   Dictionary<int, string> instaledsystemlabels = appstate.getsystemlabels();
                   //if node selected
                   if (appstate.if_any_node_selected())//or state == s2  or s3
                    {


                        int id = get_selected_node_id();
                        GeneralPolygonBase temp = appstate.find_shape_by_id(id);                     
                        
                        gui.show_form2editor(id, temp.Data/*old data*/, instaledsystemlabels, temp.getAlllabels(), temp.color);
                    }
                    //if arc selected
                    else if (appstate.if_any_relation_selected())//or state == s2  or s3
                    {
                        int id = get_selected_relation_id();
                        rrelation temp= appstate.find_relation_by_id(id);
                           
                       
                        gui.show_form2editor(id, temp.data /*old data*/, instaledsystemlabels, temp.systemlabelsID, temp.color);
                }

                }

             
                return null;//not recognized command|event|key event or some rule vioalated so command rejected as null
         }

        //internal List<AppData.scenario> scenariolist()
        //{
        //    return appstate.scenariolist();
        //}

       

        public Icommand TryOpenDBAndSetDefault(string path)
        {

           
            //failedload ha ra save nemokonim baraye default load
            if (path == "" || path == null) {
             
                // gui.show_message(globalsetting.invalid_db_path(path));
                //gui.form4.Hide();
                gui.show_form4(path, globalsetting.invalid_db_path(path));
                return new commandslist.NoAction();
            }
           
            if (!appstate.tryopendb(path)) {

                //gui.show_message(globalsetting.db_opening_problem(path));
                //gui.form4.Hide();
                gui.show_form4(path, globalsetting.db_opening_problem(path));
                return new commandslist.NoAction();
            }

            

            gui.form1.drawmode = true;//success db load

            globalsetting.setdefaultdbpath(path); //success db load

            return new commandslist.NoAction();
        }

        internal int getzoomlevel()
        {
            return appstate.currentzoomlevel;
        }

        internal List<GeneralPolygonBase> traverse_tree_and_get_total_childs_with_specific_relationtype(GeneralPolygonBase node, string v)
        {
            return appstate.traverse_tree_and_get_total_childs_with_specific_relationtype(node , v);
        }

        internal bool? form1Closing()
        {
            return AskSaveingCurrentDatabase();
                      
        }

        public Icommand MouseMove(object sender, MouseEventArgs e)
        {  
            switch (appstate.current_state)
            {
                case AppData.state.s1: break;

                case AppData.state.s2: //dragging and moving shape location

                    GeneralPolygonBase selected_node = appstate.find_shape_by_id(appstate.selected_node_id);
                  
                    //back up current shape location before change location
                    int originalx = selected_node.Locationx;
                    int originaly = selected_node.Locationy;
                    //move the shape
                    selected_node.Locationx += e.X - i; //current - old mouse
                    selected_node.Locationy += e.Y - j;
                    //check rules: e.g: child az fahther area biron nare...
                    Icommand cmd = null;
                    bool rules = true;


                    //--------------------------------------------
                    List<GeneralPolygonBase> all_fathers = appstate.findFathersOnelevel(selected_node);
                    UtilityandGeometryMath utilgeometry = new UtilityandGeometryMath();
                    if (!utilgeometry.az_father_biron_nare(selected_node, all_fathers)) ///on  ghanoon digari ke farzand azash nazaneh biron baraye move lazem nist 
                    {
                        rules = false;
                    }
                    //--------------------------------------------

                    if (rules) //rules ok
                    {
                        //khodesh ra move karde boodim , hala nobate bachehash
                        cmd = new commandslist.moveshape(e.X, e.Y, i, j);
                    }
                    else //rules violated, so rollback shape to original locations
                    {
                        //selected_node = (Ishape)backup; //roll back
                        selected_node.Locationx = originalx;
                        selected_node.Locationy = originaly;
                    }
                    i = e.X; j = e.Y;  //akhar bayad set she (after command) chon ghadimi ra niaz dashtand baraye drag
                    return cmd;                    

                    break;
                case AppData.state.s3: break;
                case AppData.state.s4:

                    cmd = new commandslist.movescreen(e.X, e.Y, i, j);
                    i = e.X; j = e.Y;  //akhar bayad set she (after command) chon ghadimi ra niaz dashtand baraye drag
                    return cmd;

                    break;
                case AppData.state.s5: break;
                case AppData.state.s6: break;
            }

            return null;//??????nothing to do or rule violation

        }
        public Icommand MouseDown(object sender, MouseEventArgs e)
        { 

            //single select ast: yek nokte mohem: poosible opverlaps: node on nodes, relation on relation , node and relation.so olaviat shape ast beyne relation va node
            //bar hasbe z index scan mishe, oon ke z-index bishtare selec mishe(dirtaz rasm shode va roo tar ast)
            //z-index ham tartibe create ast. az aval list 0,1,2 ...==jadidtar ha z-index bishtar


            j = e.Y; i = e.X; //faghat baraye baraye drag shape, ke badan dar mouse move azash estefadeh she
           
            //detect if click  is on original_shape
            GeneralPolygonBase selectedshape = gui.find_shape_by_click_FROM_VISIBLE_SHAPE_CACHE(e.X, e.Y);
           
            //detect if click is on relation
            rrelation selectedrelation = gui.find_relation_by_click_FROM_CACHE(e.X, e.Y);
          

            switch (appstate.current_state)
            {
                case AppData.state.s1:

                    if (selectedshape != null)
                    {
                        appstate.current_state = AppData.state.s2;
                        return new commandslist.bringNodeandFathersandChildrensToFront(selectedshape.GetID(), get_nested_relation_value()); 
                    }
                    else if (selectedrelation != null)
                    {
                        appstate.current_state = AppData.state.s5;
                        return new commandslist.select_relation(selectedrelation.getID()); 
                    }

                    else   //click on free blank space
                    {
                        appstate.current_state = AppData.state.s4;
                        return new commandslist.deselect_all_things();   
                    } 

                    break;
                //---------------------------------
                case AppData.state.s2: break;
                //------------------------------------
                case AppData.state.s3:

                   
                    if (selectedshape != null)  //click on a shape
                    {
                        appstate.current_state = AppData.state.s2;
                        return new commandslist.bringNodeandFathersandChildrensToFront(selectedshape.GetID(), get_nested_relation_value());
                    }
                    else if (selectedrelation != null)  //click on a relation
                    {
                        appstate.current_state = AppData.state.s5;
                        return new commandslist.select_relation(selectedrelation.getID()); 
                    }
                    else    //click on free blank space
                    {
                        appstate.current_state = AppData.state.s4;
                        return new commandslist.deselect_all_things();
                        
                    }
                    break;
                //--------------------------
                case AppData.state.s4:  break;

                case AppData.state.s5:
                    if (selectedshape != null)
                    {
                        appstate.current_state = AppData.state.s2;
                        return new commandslist.bringNodeandFathersandChildrensToFront(selectedshape.GetID(), get_nested_relation_value());

                    }
                   
                    else if (selectedrelation != null)
                    {
                       
                    }

                    else   //click on free blank space
                    { 

                    }
                    break;


                case AppData.state.s6:
                    if (selectedshape != null)
                    {
                        appstate.current_state = AppData.state.s2;
                        return new commandslist.bringNodeandFathersandChildrensToFront(selectedshape.GetID(), get_nested_relation_value());

                    }
                  
                    else if (selectedrelation != null)
                    {
                        appstate.current_state = AppData.state.s5;
                        return new commandslist.select_relation(selectedrelation.getID());
                    }

                    else   //click on free blank space
                    {
                        appstate.current_state = AppData.state.s4;
                        return new commandslist.deselect_all_things();
                        
                    }
                    break;

            }

            return null;
             

        }
        public Icommand MouseUp(object sender, MouseEventArgs e)
        {
            switch (appstate.current_state)
            {
                case AppData.state.s1: break;
                case AppData.state.s2:

                    appstate.current_state = AppData.state.s3;
                    return null;

                    break;
                case AppData.state.s3: break;
                case AppData.state.s4:

                    appstate.current_state = AppData.state.s1;
                    return null;

                    break;
                case AppData.state.s5:

                    appstate.current_state = AppData.state.s6;
                    return null;

                    break;
                case AppData.state.s6: break;
            }

            return null;
            


        }
         
    }


     

     
}
