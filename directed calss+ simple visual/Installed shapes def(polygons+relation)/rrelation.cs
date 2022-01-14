using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using System.Drawing;
 
namespace WindowsFormsApplication1
{

    

    [Serializable]
    public class rrelation : DirectedGraphClass.directed_relation
    {    //ba inheritance be oon node ya relation dll library , filed data structure ... ezaf mikonim 

        //felan custome nadarim systemi va fix ast 
        public rrelation(int myid, string mytype, int source_id, int target_id) : base(myid, mytype, source_id, target_id)
        {
            width_middle_triangle = 10; //!!!TODO set va get kon
            heigh_middle_triangle = 10;//default


            iconWidth = iconsetting.defalticonwidth;
            iconHeight = iconsetting.defaulticonheight;
            textareaHeight = textareasetting.defaulttextareheight;
            textareaWidth = textareasetting.defaulttextarewidth;
            color = SystemColorPolicy.DefaultUnelectedSateColor;



        }
        public string data;
        public Image image;


        public int width_middle_triangle = 10; //!!!TODO set va get kon
        public int heigh_middle_triangle = 10;//default





        string filePath;
        public void setfilepath(string myfilepath)
        {
            filePath = myfilepath;

            

        }
        public string getfilepath()
        {
            return filePath;
        }




        public int iconWidth; //todo  don make public
        public int iconHeight;




        public void scaledown(int factor)
        {
            heigh_middle_triangle /= factor;
            width_middle_triangle /= factor;
            iconWidth /= factor;
            iconHeight /= factor;
            textareaHeight /= factor;
            textareaWidth /= factor;
        }

        public void scaleup(int factor)
        {

            width_middle_triangle *= factor;
            heigh_middle_triangle *= factor;
            iconWidth *= factor;
            iconHeight *= factor;
            textareaHeight *= factor;
            textareaWidth *= factor;
        }



        //store labels of the shape+int is label id in system labels definition
        public Dictionary<int, bool> systemlabelsID =new Dictionary<int, bool>();
        
        public int textareaWidth;
        public int textareaHeight;
        public Color color;

        public void decrease_text_area_width()
        {
            textarearesize.decrease_text_area_width(ref textareaWidth);

        }
        public void increase_text_area__width()
        {

            textarearesize.increase_text_area__width(ref textareaWidth);


        }
        public void decrease_text_area__height()
        {

            textarearesize.decrease_text_area__height(ref textareaHeight);


        }
        public void increase_text_area__height()
        {

            textarearesize.increase_text_area__height(ref textareaHeight);


        }


    }



}
