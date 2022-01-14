using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace WindowsFormsApplication1
{
    static class drawicon
    {

        static public Image SetImageOpacity(Image image, float opacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                Bitmap bmp = new Bitmap(image.Width, image.Height);

                //create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {

                    //create a color matrix object  
                    ColorMatrix matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create image attributes  
                    ImageAttributes attributes = new ImageAttributes();

                    //set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        static public void read_and_draw_icon_with_opacity(Graphics g, int x_darwing_start, int y_darwing_start, int iconWidth, int iconHeight, string filepath)
        {


            UtilityandGeometryMath util = new UtilityandGeometryMath();
            Image img = util.GetIconOfFile(filepath);


            if (img != null)
            {
                    img = SetImageOpacity(img, 0.5F);
                    if (img != null) {
                    x_darwing_start = x_darwing_start - iconWidth / 2;//bring the icon exactly to the center of shape
                    y_darwing_start = y_darwing_start - iconHeight / 2;
                    g.DrawImage(img, x_darwing_start, y_darwing_start, iconWidth, iconHeight);

                }
            }



        }


    }

    static class textarearesize
    {



        static public void decrease_text_area_width(ref int textareaWidth)
        {  //???? az minimum size 
            if (textareaWidth > 1)
                textareaWidth-=20;           

        }
        static public void increase_text_area__width(ref int textareaWidth)
        { //???? az minimum size

            //   textarea Width++;
            textareaWidth += 20;




        }
         static public void increase_text_area__height(ref int textareaHeight)
        { //???? az minimum size


            // textareaHeight++;
            textareaHeight += 20;

        }
        static public void decrease_text_area__height(ref int textareaHeight)
        { //???? az minimum size

            if (textareaHeight > 1)
                textareaHeight -= 20;
        }
       


    }




    static class draw_text
    {
        static public int systemfontsize=16;

        static public List<Point> gettextareapoints(int textareaWidth, int textareaHeight, int Locationx, int Locationy)
        {
            //compute four point coordinate of textarea rectangle
            List<Point> result = new List<Point>();

            TextBox temp = draw_text.gettextboxdimensions(textareaWidth, textareaHeight);
            int upleftx = Locationx - temp.Width / 2;
            int uplefty = Locationy - temp.Height / 2;
            result.Add(new Point(upleftx, uplefty));
            result.Add(new Point(upleftx + temp.Width, uplefty));
            result.Add(new Point(upleftx + temp.Width, uplefty + temp.Height));
            result.Add(new Point(upleftx, uplefty + temp.Height));

            return result;


        }
        static public TextBox gettextboxdimensions(int textareaWidth, int textareaHeight)
        {

            TextBox temp = new TextBox();
            temp.Multiline = true;
            temp.Width = textareaWidth;
            temp.Height = textareaHeight;
          //  temp.BackColor = globalsetting.ClearScreeColor;
            temp.BorderStyle = BorderStyle.None;
            return temp;

        }
        static public void draw_text_if_has(Graphics g, TextBox temp , int upleftx, int uplefty,  string data)
        { //rooye yek bitmap text draw mikonim baad rooye graphic paint mikonim



            if (data == "" || data == null) return;


            temp.Text = data;
            Bitmap bitmap = new Bitmap(temp.Width, temp.Height);

           // W_parvaz
            temp.Font = new System.Drawing.Font("B Nazanin", systemfontsize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            temp.RightToLeft = RightToLeft.Yes;
            temp.WordWrap = true;
            temp.DrawToBitmap(bitmap, new Rectangle(0, 0, temp.Width, temp.Height));
            g.SmoothingMode = SmoothingMode.HighSpeed;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault; 

            g.DrawImage(bitmap, upleftx, uplefty+40);
            // g.DrawImage(CreateBitmapImage(data), upleftx, uplefty);
            // g.DrawString(Data, new Font("B zar", 7), Brushes.Black, new Point(Locationx, Locationy), new StringFormat(StringFormatFlags.DirectionRightToLeft));



        }


      


    }
}
