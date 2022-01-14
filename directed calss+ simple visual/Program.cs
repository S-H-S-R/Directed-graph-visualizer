using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {




        /// <summary>
        /// The main entry mypoint for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //----------------





            Gui gui = new Gui();
            Form1 form1 = new Form1();
            Form2 form2 = new Form2();
            Form3 form3 = new Form3();
            //Form4 form4 = new Form4();

            Key2Commands key2comand = new Key2Commands();

            //commandslist commandlist = new commandslist();                     

            AppData appdata = new AppData();

            //==================================================
            //wiring:

            key2comand.gui = gui;
            key2comand.appstate = appdata;


            gui.key2command = key2comand;
            gui.form1 = form1;
            gui.form2 = form2;
            gui.form3 = form3;
            //gui.form4 = form4;


            form1.gui = gui;
            form2.gui = gui;
            form3.gui = gui; //vali badan replace mishe in form ha!???
            //form4.gui = gui;

            //globalsetting class static ast khodesh hast va instanciate nemikhad va intialize va load va wiring ...

            commandslist.appdata = appdata;

             
            //---------------
         
            Application.Run(form1); 


            //haman:  Application.Run(form1) ast!!!
            //!!!in ra kahar nayarim code haye ghabli run neishe contriol miofteh dasteh form va dar exit code ha ejra mishe
            //form1.Show();
            //Application.Run(form1);

            

        }


         

    }
}
