using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
     
    partial class Form3 : Form
    {
        public Form3()
        {  
            InitializeComponent();
        }
         

        public Gui gui;
        //-----------------------
        //int yani id oon label not index+int is sytem label id
        //pass system labels to this form
        public Dictionary<int, string> allsystemlabels = new Dictionary<int, string>();//label hayee ke darim dar system ke add/delete... khahim kard
        


        private void Form2_Load(object sender, EventArgs e)
        {
            refereshlistbox();

            if (listBox1.Items.Count <= 0)
            {
                button2.Enabled = button3.Enabled = false;
            }

            textBox1.Text = "";
        }

        

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        { 
            gui.form3closed(allsystemlabels);
        }




        private void button1_Click(object sender, EventArgs e)
        {
            //add new label 
            
            if (textBox1.Text=="" || textBox1.Text == null) { return; }
            if (gui.tryaddnewuniquelabel(textBox1.Text))//check label uniqueness
            {             
                
                refereshlistbox();
                button2.Enabled = button3.Enabled = true;
            } 

           
        }




        private void button2_Click(object sender, EventArgs e)
        {
            //remove label
            //todo ??????remove from node? khodesh mishe!!!!????chera???

            int selectedinx = listBox1.SelectedIndex;

            if (selectedinx >= 0)
            {

                listBox1.Items.RemoveAt(selectedinx);
         
                gui.key2command.removelabelbykey(allsystemlabels.ElementAt(selectedinx).Key); ;//dictionary key ra mishnase pas baraye access be sorate index, bayad index ra tabdil be key kard

                refereshlistbox();
            }
            if (listBox1.Items.Count <= 0)
            {
                button2.Enabled = button3.Enabled = false;
            }
        }




       

        private void button3_Click(object sender, EventArgs e)
        {
            //rename
          
            int selectedinx = listBox1.SelectedIndex;
            if (selectedinx >= 0)
            {
                int key = allsystemlabels.ElementAt(listBox1.SelectedIndex).Key;
                if (gui.key2command.tryrenamelabelbykeyremainunique(key, textBox1.Text)) ////check label uniqueness
                {
                    listBox1.Items[listBox1.SelectedIndex] = textBox1.Text;

                    refereshlistbox();
                }
            }
        }


      
        void refereshlistbox()
        { 
            listBox1.Items.Clear();
            foreach (var pair in allsystemlabels)
            {
                listBox1.Items.Add(pair.Value); 
            }

        }
    }
}
