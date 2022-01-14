using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
   
    partial class Form4 : Form
    {
        public Form4()
        {  
            InitializeComponent();
        }
         

        public Gui gui;
        public string db_path;
        public string errormessage;

        private void Form4_Load_1(object sender, EventArgs e)
        {
            string shorten_db_path = Path.GetFileName(db_path);
            label1.Text = errormessage+" " + globalsetting.SelectAnotherAction();
           
        }

        private void button3_Click(object sender, EventArgs e)
        {//cancel
            //???ne khode gui begim hasz kone?? ya kdode hamin dispose|hide
           // this.Close();
            gui.loadDBcanceled();
            //this.Dispose();
           // gui.cancel_loadDB();
        }

        private void button2_Click(object sender, EventArgs e)
        { //try load again
            
            gui.tryagainloadDB(db_path);
            
        }

        private void button1_Click(object sender, EventArgs e)
        { //select another db
            gui.selecanotherDB();
            
        }

        //----------------------- 





















    }
}
