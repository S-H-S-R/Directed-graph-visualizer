using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;

using System.Reflection;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{


   
    static class globalsetting
    {


        //public static void loadappsetting()
        //{
        //     //if (!File.Exists(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)/*!File.Exists("config.xml") /* "|| bad parsing"*/)

        //    // (!System.Configuration.ConfigurationSettings.AppSettings.AllKeys.Contains("defaultdbpath")/*File.Exists("config.xml") && (globalsetting.configdata.defaultDBpath==""|| globalsetting.configdata.defaultDBpath == null)*/)



        //}


        public static string shorten_path(string db_path)
        {

           return Path.GetFileName(db_path);
        }

        public static string getdefaultdbpath()
        {
            return System.Configuration.ConfigurationSettings.AppSettings["defaultdbpath"];
        }


        public static void setdefaultdbpath(string path)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration( Application.ExecutablePath );

            config.AppSettings.Settings.Remove("defaultdbpath");
            config.AppSettings.Settings.Add("defaultdbpath", path);

            config.Save(ConfigurationSaveMode.Modified,true);
            //MessageBox.Show(getdefaltdbpath());
            //System.Configuration.ConfigurationSettings.AppSettings.Add("defaultdbpath", path);
        }


        public static void writedefaultsetting()
        {

            String foo = Properties.Resources.defaultsettings;
            File.WriteAllText( System.Reflection.Assembly.GetExecutingAssembly().Location + ".config", foo);

        }




       


        public static string savefilter= "*.bin Files|*.bin";
        public static string saveTitle = "Save a database File (*.bin)";
        public static string openfilter="*.bin Files|*.bin";
        public static string opentitle= "Select a database File (*.bin)";

       
       


        public static int minimum_height()
        {
            return 100;
        }
 

        public static string ask_sure_delete()
        {
            return "Are you sure to Delete?";
        }


        public static string db_opening_problem(string dbpath)
        {
            return "Problem opening the database: " + shorten_path(dbpath)  + ".";
        }




        public static string db_save_problem(string dbpath)
        {
            return "Problem saving the database file: " + dbpath + ".";
        }

        public static string father_can_not_child_of_his_child()
        {
            return "Father can not be child of his child. Hierachial Relation exist in reverse direction.";
        }

        public static string cant_hierachial_itself()
        {
            return "It's not possible to create hierachial relation to same shape.";
        }

        public static string shape_only_single_father()
        {
            return "Shapes can only have at most one father.";
        }

        internal static string invalid_db_path(string path)
        {
            return "Invalid database path: " + shorten_path( path )+ ".";
        }




        internal static string invalid_db_path_to_create(string path)
        {
            return "Invalid database path to create new one: " + path;
        }

        internal static string db_create_problem(string path)
        {
            return "Create new database problem: " + path + ".";
        }

        internal static string save_before_unload_caption()
        {
            return "Save changes";
        }

        internal static string ask_save_before_unload(string path)
        {
            return "Save changes Before unload database? : " + path + ".";
        }

        internal static string problem_file_delete(string filepath)
        {
            return "Problem file delete from disk: " + filepath + ".";
        }

        internal static string problemfilelaunch(string filepath)
        {
            return "Problem launch file : " + filepath + ".";
        }

        internal static string problem_file__or_directiry_copy(string filepath)
        {
            return "Problem attaching (copy) file or directory to  : " + filepath + ".";
        }


        internal static string AskUserApproveDiscardOldData()
        {
            return "Are you sure to change data?";
        }


        internal static string AskSureDeleteLabel()
        {
            return "Are you sure to delete system label?";
        }

        internal static readonly int scalefactor = 2;

        public static Color ClearScreeColor = Color.Beige;

        internal static string SelectAnotherAction()
        {
            return "Please Select another action:";
        }

        internal static string ask_sure_delete_attachment()
        {
            return "Are you sure to delete attachment?";
        }

        internal static string delete_caption()
        {
            return "Delete";
        }

        internal static string ApproveDiscardOldDataCaption()
        {
            return "Confirm";
        }
    }
}