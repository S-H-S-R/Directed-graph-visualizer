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
   



    partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
         

        public Gui gui;
        //-----------------------
        public string data;//pass shape|relation data to this form

        //int yani id oon label not index
        //pass labels of system to this form
        public Dictionary<int, string> allsystemlabels = new Dictionary<int, string>();//label hayee ke darim dar system va azash entekhab mikonim

        //int id hayee az systemlabel ast va bool ke mige aya checked hast ya kheir=yani entesab label be in shape
        //shape|relations
        public Dictionary<int, bool> thisshapelabels = new Dictionary<int, bool>();



        //color of the node
        public Color color = new Color();
        public int shap_id;//or relation

         

        void reDrawCheckedListBox()
        {
            //hame label haye system ra list mikonimm va anhayee ke in shape darad ra tick mizanim
            checkedListBox1.Items.Clear();
            foreach (var pair in allsystemlabels)
            {
                checkedListBox1.Items.Add(pair.Value);
                if (thisshapelabels.ContainsKey(pair.Key)){//yani label  male in shape ast pas dar listbox ham tick mizanim

                    checkedListBox1.SetItemChecked(checkedListBox1.Items.Count - 1, true);

                }
            }

        }

        internal void copychecklistboxtodictionary()
        {

            thisshapelabels.Clear();
           
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    int id = allsystemlabels.ElementAt(i).Key;
                    thisshapelabels.Add(id, true);

                }

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
           richTextBox1.Text = this.data;
           reDrawCheckedListBox();
            button1.BackColor = color;


        }

        

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
        
            copychecklistboxtodictionary();

            this.data = richTextBox1.Text;
            richTextBox1.Text = "";
            gui.form2closed(this.data, thisshapelabels, color);//send data and edited labels  to save

           
           
          
        }

        private void linkLabel1_MouseClick(object sender, MouseEventArgs e)
        {
            Dictionary<int, string> instaledsystemlabels = gui.key2command.appstate.getsystemlabels(); //??aya in masir dorost ast violate nashode encapsulation???
            gui.show_form3editor(instaledsystemlabels);

            this.allsystemlabels = gui.key2command.appstate.getsystemlabels(); ;
            reDrawCheckedListBox();

        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.ShowDialog();
            this.color = cd.Color;
            button1.BackColor = cd.Color;
        }
    }
}
