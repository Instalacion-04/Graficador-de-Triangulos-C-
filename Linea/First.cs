using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Linea
{
    public partial class First : Form
    {
        public First()
        {
            InitializeComponent();
        }

        //aqui se crean el objeto qiu dibujara 
        System.Drawing.Pen lapiz1 = new System.Drawing.Pen(System.Drawing.ColorTranslator.FromHtml("#154360")); //la grafica
        System.Drawing.Pen lapiz2 = new System.Drawing.Pen(System.Drawing.ColorTranslator.FromHtml("#85C1E9")); //la grafica


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //Bloque 1 codigo  de  Creacion del plano cartesiano
            int xcentro = pictureBox1.Width - 475;
            int ycentro = pictureBox1.Height - 20;
            e.Graphics.TranslateTransform(xcentro, ycentro); //transformacion de la escala
            e.Graphics.ScaleTransform(2, -2); //grosor de la lineas del plano cartesiaono
            e.Graphics.DrawLine(lapiz1, xcentro * -10, 0, xcentro * 10, 0); //eje x Linea del plano
            e.Graphics.DrawLine(lapiz1, 0, ycentro * 10, 0, ycentro * -10); //eje y Linea del plano
            //trazar lineas de escala
            for (int i = -xcentro; i < 250; i = i + 5)
            {
                e.Graphics.DrawLine(lapiz1, 2, i, -2, i);
                e.Graphics.DrawLine(lapiz1, i, 2, i, -2);
            }
            //Fin del bloque 1
        }

        private void btncalcular_Click(object sender, EventArgs e)
        {
            //Linea A---->B
            //Calculo de valores de la pendiente
            if (string.IsNullOrWhiteSpace(txtx1.Text) || string.IsNullOrWhiteSpace(txty1.Text) ||
                string.IsNullOrWhiteSpace(txtx2.Text) || string.IsNullOrWhiteSpace(txty2.Text) ||
                string.IsNullOrWhiteSpace(txtx3.Text) || string.IsNullOrWhiteSpace(txty3.Text))
            {
                MessageBox.Show("Favor de llenar todos los campos", "Campos vacios", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                txtangulo.Clear();
                txtpendiente.Clear();
                txtlinea.Clear();
                pictureBox1.Refresh();
                this.tabladatos.Items.Clear();

                string lista;
                float pendiente, divisor, dividendo, x1, y1, x2, y2, angulo;
                double tang;

                x1 = (float)Convert.ToDouble(txtx1.Text);
                y1 = (float)Convert.ToDouble(txty1.Text);
                x2 = (float)Convert.ToDouble(txtx2.Text);
                y2 = (float)Convert.ToDouble(txty2.Text);

                divisor = y2 - y1;
                dividendo = x2 - x1;
                pendiente = divisor / dividendo;
                float m = Convert.ToSingle(Math.Round(pendiente, 2));
                float m2 = m;

                txtpendiente.Text = m.ToString();
                pendiente = Math.Abs(m);

                tang = ((Math.Atan(m)) * 180 / Math.PI);
                angulo = Convert.ToSingle(Math.Round(tang, 1));

                if (angulo < 0)
                {
                    angulo = angulo + 180;
                    txtangulo.Text = angulo.ToString() + "%";
                }
                else
                {
                    txtangulo.Text = angulo.ToString() + "%";
                }
                angulo = Math.Abs(angulo);


                //Fin del calculo de valores 
                //Preparacion de dibujo de la linea dentro del plano 
                //dibujar linea recta dentro del plano
                System.Drawing.Graphics ent = this.pictureBox1.CreateGraphics();
                int xcentro = pictureBox1.Width - 475;
                int ycentro = pictureBox1.Height - 20;
                ent.TranslateTransform(xcentro, ycentro);
                ent.ScaleTransform(2, -2);
                ent.DrawLine(lapiz2, x1, y1, x2, y2);


                /* Colorea el triangulo */
                float x3 = (float)Convert.ToDouble(txtx3.Text);
                float y3 = (float)Convert.ToDouble(txty3.Text);

                Point point1 = new Point(Convert.ToInt32(x1), Convert.ToInt32(y1));
                Point point2 = new Point(Convert.ToInt32(x2), Convert.ToInt32(y2));
                Point point3 = new Point(Convert.ToInt32(x3), Convert.ToInt32(y3));

                Point[] rectangulo =
                {
                  point1,
                  point2,
                  point3,
                };

                ent.FillPolygon(Brushes.MediumTurquoise, rectangulo);




                //dibujar los textos de las coordenadas dentro del plano letras A y B
                System.Drawing.Graphics ent2 = this.pictureBox1.CreateGraphics();
                ent2.TranslateTransform(xcentro, ycentro);
                ent2.ScaleTransform(2, -2);
                String cadena = "A";
                Font drawFont = new Font("Microsoft YaHei UI", 6);
                SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#154360"));
                PointF drawpoint = new PointF(x1, y1);
                ent2.DrawString(cadena, drawFont, drawBrush, drawpoint);



                System.Drawing.Graphics ent3 = this.pictureBox1.CreateGraphics();
                ent3.TranslateTransform(xcentro, ycentro);
                ent3.ScaleTransform(2, -2);
                String cadena2 = "B";
                ent3.DrawString(cadena2, drawFont, drawBrush, x2, y2);


                //fin de preparacion de preparacion de dibujo de la linea dentro del plano
                //cuando x1 sea mayor x2 y cuando y1 sea mayor ay2 tengan valores en el textbox dibujara en la tabla los valores
                //y aqui inicia el bloque de pendientes negativas 
                if (pendiente < 1)
                {
                    float pendiente1 = divisor / dividendo;
                    float ya1 = y1;
                    if (x1 > x2 && y1 > y2) //caso:  m+<1 der-izq y arriba y abajo
                    {
                        txtlinea.Text = "Caso Positiva +m<1 DER-IZQ ARR-ABA";
                        for (int i = Convert.ToInt32(txtx1.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatos.Items.Add(lista);
                            y1 = y1 - Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    //cuando x1 sea meno x2 y cuando y1 sea menor ay2 tengan valores en el textbox dibujara en la tabla los valores
                    else if (x1 < x2 && y1 < y2) // caso: m+<1 izq-der  y abajo - arriba
                    {
                        txtlinea.Text = "Caso Positiva +m<1 IZQ-DER ABA-ARR";
                        for (int i = Convert.ToInt32(txtx1.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatos.Items.Add(lista);
                            y1 = y1 + Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    //Pendientes positivas fin del codigo 
                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    else if (x1 > x2 && y1 < y2) //y = y + m -- x = -1  // caso : -m<1 der-izq y abajo-arriba
                    {
                        txtlinea.Text = "Caso Negativa -m<1 DER-IZQ ABA-ARR";
                        for (int i = Convert.ToInt32(txtx1.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatos.Items.Add(lista);
                            y1 = y2 + Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 > y2) //y = y - m -- x = + 1   // caso: -m<1 izq-der y arriba y abajo
                    {
                        txtlinea.Text = "Caso Negativa -m<1 IZQ-DER ARR-ABA";
                        for (int i = Convert.ToInt32(txtx1.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatos.Items.Add(lista);
                            y1 = y1 - Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else
                    {
                        txtlinea.Text = "ERROR";
                    }
                }
                //Fin de la pendiente menor a 1
                //Comienzo de la pendiente mayor a 1
                if (pendiente > 1)
                {
                    float xa1 = x1;
                    if (x1 > x2 && y1 > y2) //  caso: m+>1 der-izq y arriba y abajo
                    {
                        txtlinea.Text = "Caso Positiva +m>1 DER-IZQ ARR-ABA";
                        for (int i = Convert.ToInt32(txty1.Text); i >= y2; i--)
                        {
                            lista = "( " + xa1 + " , " + i + " )";
                            this.tabladatos.Items.Add(lista);
                            x1 = x1 - Convert.ToSingle(1 / pendiente);
                            xa1 = Convert.ToSingle(Math.Round(x1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 < y2) // caso: m +> 1 izq - der y abajo-arriba
                    {
                        txtlinea.Text = "Caso Positiva +m>1 IZQ-DER ABA-ARR";
                        for (int i = Convert.ToInt32(txty1.Text); i <= y2; i++)
                        {
                            lista = "( " + xa1 + " , " + i + " )";
                            this.tabladatos.Items.Add(lista);
                            x1 = x1 + Convert.ToSingle(1 / pendiente);
                            xa1 = Convert.ToSingle(Math.Round(x1, 2));
                        }
                    }
                    else if (x1 > x2 && y1 < y2) // caso: m-> 1 der- izq y abajo-arriba
                    {
                        txtlinea.Text = "Caso Negativa -m>1 DER-IZQ ABA-ARR";
                        for (int i = Convert.ToInt32(txty1.Text); i <= y2; i++)
                        {
                            lista = "( " + xa1 + " , " + i + " )";
                            this.tabladatos.Items.Add(lista);
                            x1 = x2 - Convert.ToSingle(1 / pendiente);
                            xa1 = Convert.ToSingle(Math.Round(x1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 > y2) //caso: m-> 1 izq - der y arriba-abajo
                    {
                        txtlinea.Text = "Caso Negativa -m>1 IZQ-DER ARR-ABA";
                        for (int i = Convert.ToInt32(txty1.Text); i >= y2; i--)
                        {
                            lista = "( " + xa1 + " , " + i + " )";
                            this.tabladatos.Items.Add(lista);
                            x1 = x1 + Convert.ToSingle(1 / pendiente);
                            xa1 = Convert.ToSingle(Math.Round(x1, 2));
                        }
                    }
                    else
                    {
                        txtlinea.Text = "ERROR";
                    }
                }
                //fin de la pendiente > 1
                //comienzo de la pendiente igual a 0
                if (divisor == 0)
                {
                    if (x1 > x2)
                    {
                        txtlinea.Text = "Caso Especial m=0 DER-IZQ ";
                        for (int i = Convert.ToInt32(txtx1.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + y1 + " )";
                            this.tabladatos.Items.Add(lista);
                        }
                    }
                    else
                    {
                        txtlinea.Text = "Caso Especial m=0 IZQ-DER";
                        for (int i = Convert.ToInt32(txtx1.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + y1 + " )";
                            this.tabladatos.Items.Add(lista);
                        }
                    }
                }
                //fin de pendiente = 0
                //inicio de la pendiente inexistente
                if (dividendo == 0)
                {
                    if (y1 > y2)
                    {
                        txtlinea.Text = "Caso Especial m=E ARR-ABA";
                        for (int i = Convert.ToInt32(txty1.Text); i >= y2; i--)
                        {
                            lista = "( " + x1 + " , " + i + " )";
                            this.tabladatos.Items.Add(lista);
                        }
                    }
                    else
                    {
                        txtlinea.Text = "Caso Especial m=E ABA-ARR";
                        for (int i = Convert.ToInt32(txty1.Text); i <= y2; i++)
                        {
                            lista = "( " + x1 + " , " + i + " )";
                            this.tabladatos.Items.Add(lista);
                        }
                    }
                    txtpendiente.Text = "E";
                }
                //fin de pendiente division / 0 que es la pendiente inexistente
                //inicio de la pendiente igual a 1
                if (pendiente == 1)
                {
                    float pendiente1 = divisor / dividendo;
                    float ya1 = y1;
                    if (x1 > x2 && y1 > y2) //
                    {
                        txtlinea.Text = "Caso Especial m=1 DER-IZQ ARR-ABA";
                        for (int i = Convert.ToInt32(txtx1.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatos.Items.Add(lista);
                            y1 = y2 - Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 < y2)
                    {
                        txtlinea.Text = "Caso Especial m=1 IZQ-DER ABA-ARR";
                        for (int i = Convert.ToInt32(txtx1.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatos.Items.Add(lista);
                            y1 = y1 + Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    /*Fin de la pendiente igual a 1*/
                    //comienzo de pendientes negativas iguales a 1 dos casos 
                    // pemdientes negativas igual a 1 -M=1 der-izq abajo-arriba
                    else if (x1 > x2 && y1 < y2) //y = y + m -- x = -1
                    {
                        txtlinea.Text = "Caso Especial m= -1 DER-IZQ ABA-ARR";
                        for (int i = Convert.ToInt32(txtx1.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatos.Items.Add(lista);
                            y1 = y1 + Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 > y2) //y = y - m -- x = + 1 pendientes negativas igual a 1 -M=1 izq-der arriba-abajo
                    {
                        txtlinea.Text = "Caso Especial m= -1 IZQ-DER ARRI-ABA";
                        for (int i = Convert.ToInt32(txtx1.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatos.Items.Add(lista);
                            y1 = y1 - Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else
                    {
                        txtlinea.Text = "ERROR";
                    }
                }
            }


            if (string.IsNullOrWhiteSpace(txtx1.Text) || string.IsNullOrWhiteSpace(txty1.Text) ||
                string.IsNullOrWhiteSpace(txtx2.Text) || string.IsNullOrWhiteSpace(txty2.Text) ||
                string.IsNullOrWhiteSpace(txtx3.Text) || string.IsNullOrWhiteSpace(txty3.Text))
            {
                string a = txtx1.Text;
            }
            else
            {
                txtangulo2.Clear();
                txtpendiente2.Clear();
                txtlinea2.Clear();
               
                this.tabladatosbc.Items.Clear();

                string lista;
                float pendiente, divisor, dividendo, x1, y1, x2, y2, angulo;
                double tang;

                x1 = (float)Convert.ToDouble(txtx2.Text);
                y1 = (float)Convert.ToDouble(txty2.Text);
                x2 = (float)Convert.ToDouble(txtx3.Text);
                y2 = (float)Convert.ToDouble(txty3.Text);

                divisor = y2 - y1;
                dividendo = x2 - x1;
                pendiente = divisor / dividendo;
                float m = Convert.ToSingle(Math.Round(pendiente, 2));
                float m2 = m;

                txtpendiente2.Text = m.ToString();
                pendiente = Math.Abs(m);

                tang = ((Math.Atan(m)) * 180 / Math.PI);
                angulo = Convert.ToSingle(Math.Round(tang, 1));

                if (angulo < 0)
                {
                    angulo = angulo + 180;
                    txtangulo2.Text = angulo.ToString() + "%";
                }
                else
                {
                    txtangulo2.Text = angulo.ToString() + "%";
                }
                angulo = Math.Abs(angulo);


                //Fin del calculo de valores 
                //Preparacion de dibujo de la linea dentro del plano 
                //dibujar linea recta dentro del plano
                System.Drawing.Graphics ent = this.pictureBox1.CreateGraphics();
                int xcentro = pictureBox1.Width - 475;
                int ycentro = pictureBox1.Height - 20;
                ent.TranslateTransform(xcentro, ycentro);
                ent.ScaleTransform(2, -2);
                ent.DrawLine(lapiz2, x1, y1, x2, y2);






                //dibujar los textos de las coordenadas dentro del plano letras A y B
                System.Drawing.Graphics ent2 = this.pictureBox1.CreateGraphics();
                ent2.TranslateTransform(xcentro, ycentro);
                ent2.ScaleTransform(2, -2);
                String cadena = "B";
                Font drawFont = new Font("Microsoft YaHei UI", 6);
                SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#154360"));
                PointF drawpoint = new PointF(x1, y1);
                ent2.DrawString(cadena, drawFont, drawBrush, drawpoint);



                System.Drawing.Graphics ent3 = this.pictureBox1.CreateGraphics();
                ent3.TranslateTransform(xcentro, ycentro);
                ent3.ScaleTransform(2, -2);
                String cadena2 = "C";
                ent3.DrawString(cadena2, drawFont, drawBrush, x2, y2);


                //fin de preparacion de preparacion de dibujo de la linea dentro del plano
                //cuando x1 sea mayor x2 y cuando y1 sea mayor ay2 tengan valores en el textbox dibujara en la tabla los valores
                //y aqui inicia el bloque de pendientes negativas 
                if (pendiente < 1)
                {
                    float pendiente1 = divisor / dividendo;
                    float ya1 = y1;
                    if (x1 > x2 && y1 > y2) //caso:  m+<1 der-izq y arriba y abajo
                    {
                        txtlinea2.Text = "Caso Positiva +m<1 DER-IZQ ARR-ABA";
                        for (int i = Convert.ToInt32(txtx2.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosbc.Items.Add(lista);
                            y1 = y1 - Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    //cuando x1 sea meno x2 y cuando y1 sea menor ay2 tengan valores en el textbox dibujara en la tabla los valores
                    else if (x1 < x2 && y1 < y2) // caso: m+<1 izq-der  y abajo - arriba
                    {
                        txtlinea2.Text = "Caso Positiva +m<1 IZQ-DER ABA-ARR";
                        for (int i = Convert.ToInt32(txtx2.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosbc.Items.Add(lista);
                            y1 = y1 + Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    //Pendientes positivas fin del codigo 
                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    else if (x1 > x2 && y1 < y2) //y = y + m -- x = -1  // caso : -m<1 der-izq y abajo-arriba
                    {
                        txtlinea2.Text = "Caso Negativa -m<1 DER-IZQ ABA-ARR";
                        for (int i = Convert.ToInt32(txtx2.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosbc.Items.Add(lista);
                            y1 = y2 + Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 > y2) //y = y - m -- x = + 1   // caso: -m<1 izq-der y arriba y abajo
                    {
                        txtlinea2.Text = "Caso Negativa -m<1 IZQ-DER ARR-ABA";
                        for (int i = Convert.ToInt32(txtx2.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosbc.Items.Add(lista);
                            y1 = y1 - Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else
                    {
                        txtlinea2.Text = "ERROR";
                    }
                }
                //Fin de la pendiente menor a 1
                //Comienzo de la pendiente mayor a 1
                if (pendiente > 1)
                {
                    float xa1 = x1;
                    if (x1 > x2 && y1 > y2) //  caso: m+>1 der-izq y arriba y abajo
                    {
                        txtlinea2.Text = "Caso Positiva +m>1 DER-IZQ ARR-ABA";
                        for (int i = Convert.ToInt32(txty2.Text); i >= y2; i--)
                        {
                            lista = "( " + xa1 + " , " + i + " )";
                            this.tabladatosbc.Items.Add(lista);
                            x1 = x1 - Convert.ToSingle(1 / pendiente);
                            xa1 = Convert.ToSingle(Math.Round(x1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 < y2) // caso: m +> 1 izq - der y abajo-arriba
                    {
                        txtlinea2.Text = "Caso Positiva +m>1 IZQ-DER ABA-ARR";
                        for (int i = Convert.ToInt32(txty2.Text); i <= y2; i++)
                        {
                            lista = "( " + xa1 + " , " + i + " )";
                            this.tabladatosbc.Items.Add(lista);
                            x1 = x1 + Convert.ToSingle(1 / pendiente);
                            xa1 = Convert.ToSingle(Math.Round(x1, 2));
                        }
                    }
                    else if (x1 > x2 && y1 < y2) // caso: m-> 1 der- izq y abajo-arriba
                    {
                        txtlinea2.Text = "Caso Negativa -m>1 DER-IZQ ABA-ARR";
                        for (int i = Convert.ToInt32(txty2.Text); i <= y2; i++)
                        {
                            lista = "( " + xa1 + " , " + i + " )";
                            this.tabladatosbc.Items.Add(lista);
                            x1 = x2 - Convert.ToSingle(1 / pendiente);
                            xa1 = Convert.ToSingle(Math.Round(x1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 > y2) //caso: m-> 1 izq - der y arriba-abajo
                    {
                        txtlinea2.Text = "Caso Negativa -m>1 IZQ-DER ARR-ABA";
                        for (int i = Convert.ToInt32(txty2.Text); i >= y2; i--)
                        {
                            lista = "( " + xa1 + " , " + i + " )";
                            this.tabladatosbc.Items.Add(lista);
                            x1 = x1 + Convert.ToSingle(1 / pendiente);
                            xa1 = Convert.ToSingle(Math.Round(x1, 2));
                        }
                    }
                    else
                    {
                        txtlinea2.Text = "ERROR";
                    }
                }
                //fin de la pendiente > 1
                //comienzo de la pendiente igual a 0
                if (divisor == 0)
                {
                    if (x1 > x2)
                    {
                        txtlinea2.Text = "Caso Especial m=0 DER-IZQ ";
                        for (int i = Convert.ToInt32(txtx2.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + y1 + " )";
                            this.tabladatosbc.Items.Add(lista);
                        }
                    }
                    else
                    {
                        txtlinea2.Text = "Caso Especial m=0 IZQ-DER";
                        for (int i = Convert.ToInt32(txtx2.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + y1 + " )";
                            this.tabladatosbc.Items.Add(lista);
                        }
                    }
                }
                //fin de pendiente = 0
                //inicio de la pendiente inexistente
                if (dividendo == 0)
                {
                    if (y1 > y2)
                    {
                        txtlinea2.Text = "Caso Especial m=E ARR-ABA";
                        for (int i = Convert.ToInt32(txty2.Text); i >= y2; i--)
                        {
                            lista = "( " + x1 + " , " + i + " )";
                            this.tabladatosbc.Items.Add(lista);
                        }
                    }
                    else
                    {
                        txtlinea2.Text = "Caso Especial m=E ABA-ARR";
                        for (int i = Convert.ToInt32(txty2.Text); i <= y2; i++)
                        {
                            lista = "( " + x1 + " , " + i + " )";
                            this.tabladatosbc.Items.Add(lista);
                        }
                    }
                    txtpendiente2.Text = "E";
                }
                //fin de pendiente division / 0 que es la pendiente inexistente
                //inicio de la pendiente igual a 1
                if (pendiente == 1)
                {
                    float pendiente1 = divisor / dividendo;
                    float ya1 = y1;
                    if (x1 > x2 && y1 > y2) //
                    {
                        txtlinea2.Text = "Caso Especial m=1 DER-IZQ ARR-ABA";
                        for (int i = Convert.ToInt32(txtx2.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosbc.Items.Add(lista);
                            y1 = y2 - Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 < y2)
                    {
                        txtlinea2.Text = "Caso Especial m=1 IZQ-DER ABA-ARR";
                        for (int i = Convert.ToInt32(txtx2.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosbc.Items.Add(lista);
                            y1 = y1 + Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    /*Fin de la pendiente igual a 1*/
                    //comienzo de pendientes negativas iguales a 1 dos casos 
                    // pemdientes negativas igual a 1 -M=1 der-izq abajo-arriba
                    else if (x1 > x2 && y1 < y2) //y = y + m -- x = -1
                    {
                        txtlinea2.Text = "Caso Especial m= -1 DER-IZQ ABA-ARR";
                        for (int i = Convert.ToInt32(txtx2.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosbc.Items.Add(lista);
                            y1 = y1 + Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 > y2) //y = y - m -- x = + 1 pendientes negativas igual a 1 -M=1 izq-der arriba-abajo
                    {
                        txtlinea2.Text = "Caso Especial m= -1 IZQ-DER ARRI-ABA";
                        for (int i = Convert.ToInt32(txtx2.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosbc.Items.Add(lista);
                            y1 = y1 - Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else
                    {
                        txtlinea2.Text = "ERROR";
                    }
                }
            }


            if (string.IsNullOrWhiteSpace(txtx1.Text) || string.IsNullOrWhiteSpace(txty1.Text) ||
     string.IsNullOrWhiteSpace(txtx2.Text) || string.IsNullOrWhiteSpace(txty2.Text) ||
     string.IsNullOrWhiteSpace(txtx3.Text) || string.IsNullOrWhiteSpace(txty3.Text))
            {
                string a = txtx1.Text;
            }
            else
            {
                txtanguloca.Clear();
                txtpendienteca.Clear();
                txtlineaca.Clear();
           
                this.tabladatosca.Items.Clear();

                string lista;
                float pendiente, divisor, dividendo, x1, y1, x2, y2, angulo;
                double tang;

                x1 = (float)Convert.ToDouble(txtx3.Text);
                y1 = (float)Convert.ToDouble(txty3.Text);
                x2 = (float)Convert.ToDouble(txtx1.Text);
                y2 = (float)Convert.ToDouble(txty1.Text);

                divisor = y2 - y1;
                dividendo = x2 - x1;
                pendiente = divisor / dividendo;
                float m = Convert.ToSingle(Math.Round(pendiente, 2));
                float m2 = m;

                txtpendienteca.Text = m.ToString();
                pendiente = Math.Abs(m);

                tang = ((Math.Atan(m)) * 180 / Math.PI);
                angulo = Convert.ToSingle(Math.Round(tang, 1));

                if (angulo < 0)
                {
                    angulo = angulo + 180;
                    txtanguloca.Text = angulo.ToString() + "%";
                }
                else
                {
                    txtanguloca.Text = angulo.ToString() + "%";
                }
                angulo = Math.Abs(angulo);


                //Fin del calculo de valores 
                //Preparacion de dibujo de la linea dentro del plano 
                //dibujar linea recta dentro del plano
                System.Drawing.Graphics ent = this.pictureBox1.CreateGraphics();
                int xcentro = pictureBox1.Width - 475;
                int ycentro = pictureBox1.Height - 20;
                ent.TranslateTransform(xcentro, ycentro);
                ent.ScaleTransform(2, -2);
                ent.DrawLine(lapiz2, x1, y1, x2, y2);






                //dibujar los textos de las coordenadas dentro del plano letras A y B
                System.Drawing.Graphics ent2 = this.pictureBox1.CreateGraphics();
                ent2.TranslateTransform(xcentro, ycentro);
                ent2.ScaleTransform(2, -2);
                String cadena = "C";
                Font drawFont = new Font("Microsoft YaHei UI", 6);
                SolidBrush drawBrush = new SolidBrush(ColorTranslator.FromHtml("#154360"));
                PointF drawpoint = new PointF(x1, y1);
                ent2.DrawString(cadena, drawFont, drawBrush, drawpoint);



                System.Drawing.Graphics ent3 = this.pictureBox1.CreateGraphics();
                ent3.TranslateTransform(xcentro, ycentro);
                ent3.ScaleTransform(2, -2);
                String cadena2 = "A";
                ent3.DrawString(cadena2, drawFont, drawBrush, x2, y2);


                //fin de preparacion de preparacion de dibujo de la linea dentro del plano
                //cuando x1 sea mayor x2 y cuando y1 sea mayor ay2 tengan valores en el textbox dibujara en la tabla los valores
                //y aqui inicia el bloque de pendientes negativas 
                if (pendiente < 1)
                {
                    float pendiente1 = divisor / dividendo;
                    float ya1 = y1;
                    if (x1 > x2 && y1 > y2) //caso:  m+<1 der-izq y arriba y abajo
                    {
                        txtlineaca.Text = "Caso Positiva +m<1 DER-IZQ ARR-ABA";
                        for (int i = Convert.ToInt32(txtx3.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosca.Items.Add(lista);
                            y1 = y1 - Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    //cuando x1 sea meno x2 y cuando y1 sea menor ay2 tengan valores en el textbox dibujara en la tabla los valores
                    else if (x1 < x2 && y1 < y2) // caso: m+<1 izq-der  y abajo - arriba
                    {
                        txtlineaca.Text = "Caso Positiva +m<1 IZQ-DER ABA-ARR";
                        for (int i = Convert.ToInt32(txtx3.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosca.Items.Add(lista);
                            y1 = y1 + Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    //Pendientes positivas fin del codigo 
                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    else if (x1 > x2 && y1 < y2) //y = y + m -- x = -1  // caso : -m<1 der-izq y abajo-arriba
                    {
                        txtlineaca.Text = "Caso Negativa -m<1 DER-IZQ ABA-ARR";
                        for (int i = Convert.ToInt32(txtx3.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosca.Items.Add(lista);
                            y1 = y2 + Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 > y2) //y = y - m -- x = + 1   // caso: -m<1 izq-der y arriba y abajo
                    {
                        txtlineaca.Text = "Caso Negativa -m<1 IZQ-DER ARR-ABA";
                        for (int i = Convert.ToInt32(txtx3.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosca.Items.Add(lista);
                            y1 = y1 - Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else
                    {
                        txtlineaca.Text = "ERROR";
                    }
                }
                //Fin de la pendiente menor a 1
                //Comienzo de la pendiente mayor a 1
                if (pendiente > 1)
                {
                    float xa1 = x1;
                    if (x1 > x2 && y1 > y2) //  caso: m+>1 der-izq y arriba y abajo
                    {
                        txtlineaca.Text = "Caso Positiva +m>1 DER-IZQ ARR-ABA";
                        for (int i = Convert.ToInt32(txty3.Text); i >= y2; i--)
                        {
                            lista = "( " + xa1 + " , " + i + " )";
                            this.tabladatosca.Items.Add(lista);
                            x1 = x1 - Convert.ToSingle(1 / pendiente);
                            xa1 = Convert.ToSingle(Math.Round(x1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 < y2) // caso: m +> 1 izq - der y abajo-arriba
                    {
                        txtlineaca.Text = "Caso Positiva +m>1 IZQ-DER ABA-ARR";
                        for (int i = Convert.ToInt32(txty3.Text); i <= y2; i++)
                        {
                            lista = "( " + xa1 + " , " + i + " )";
                            this.tabladatosca.Items.Add(lista);
                            x1 = x1 + Convert.ToSingle(1 / pendiente);
                            xa1 = Convert.ToSingle(Math.Round(x1, 2));
                        }
                    }
                    else if (x1 > x2 && y1 < y2) // caso: m-> 1 der- izq y abajo-arriba
                    {
                        txtlineaca.Text = "Caso Negativa -m>1 DER-IZQ ABA-ARR";
                        for (int i = Convert.ToInt32(txty3.Text); i <= y2; i++)
                        {
                            lista = "( " + xa1 + " , " + i + " )";
                            this.tabladatosca.Items.Add(lista);
                            x1 = x2 - Convert.ToSingle(1 / pendiente);
                            xa1 = Convert.ToSingle(Math.Round(x1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 > y2) //caso: m-> 1 izq - der y arriba-abajo
                    {
                        txtlineaca.Text = "Caso Negativa -m>1 IZQ-DER ARR-ABA";
                        for (int i = Convert.ToInt32(txty3.Text); i >= y2; i--)
                        {
                            lista = "( " + xa1 + " , " + i + " )";
                            this.tabladatosca.Items.Add(lista);
                            x1 = x1 + Convert.ToSingle(1 / pendiente);
                            xa1 = Convert.ToSingle(Math.Round(x1, 2));
                        }
                    }
                    else
                    {
                        txtlineaca.Text = "ERROR";
                    }
                }
                //fin de la pendiente > 1
                //comienzo de la pendiente igual a 0
                if (divisor == 0)
                {
                    if (x1 > x2)
                    {
                        txtlineaca.Text = "Caso Especial m=0 DER-IZQ ";
                        for (int i = Convert.ToInt32(txtx3.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + y1 + " )";
                            this.tabladatosca.Items.Add(lista);
                        }
                    }
                    else
                    {
                        txtlineaca.Text = "Caso Especial m=0 IZQ-DER";
                        for (int i = Convert.ToInt32(txtx3.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + y1 + " )";
                            this.tabladatosca.Items.Add(lista);
                        }
                    }
                }
                //fin de pendiente = 0
                //inicio de la pendiente inexistente
                if (dividendo == 0)
                {
                    if (y1 > y2)
                    {
                        txtlineaca.Text = "Caso Especial m=E ARR-ABA";
                        for (int i = Convert.ToInt32(txty3.Text); i >= y2; i--)
                        {
                            lista = "( " + x1 + " , " + i + " )";
                            this.tabladatosca.Items.Add(lista);
                        }
                    }
                    else
                    {
                        txtlineaca.Text = "Caso Especial m=E ABA-ARR";
                        for (int i = Convert.ToInt32(txty3.Text); i <= y2; i++)
                        {
                            lista = "( " + x1 + " , " + i + " )";
                            this.tabladatosca.Items.Add(lista);
                        }
                    }
                    txtpendienteca.Text = "E";
                }
                //fin de pendiente division / 0 que es la pendiente inexistente
                //inicio de la pendiente igual a 1
                if (pendiente == 1)
                {
                    float pendiente1 = divisor / dividendo;
                    float ya1 = y1;
                    if (x1 > x2 && y1 > y2) //
                    {
                        txtlineaca.Text = "Caso Especial m=1 DER-IZQ ARR-ABA";
                        for (int i = Convert.ToInt32(txtx3.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosca.Items.Add(lista);
                            y1 = y2 - Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 < y2)
                    {
                        txtlineaca.Text = "Caso Especial m=1 IZQ-DER ABA-ARR";
                        for (int i = Convert.ToInt32(txtx3.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosca.Items.Add(lista);
                            y1 = y1 + Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    /*Fin de la pendiente igual a 1*/
                    //comienzo de pendientes negativas iguales a 1 dos casos 
                    // pemdientes negativas igual a 1 -M=1 der-izq abajo-arriba
                    else if (x1 > x2 && y1 < y2) //y = y + m -- x = -1
                    {
                        txtlineaca.Text = "Caso Especial m= -1 DER-IZQ ABA-ARR";
                        for (int i = Convert.ToInt32(txtx3.Text); i >= x2; i--)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosca.Items.Add(lista);
                            y1 = y1 + Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else if (x1 < x2 && y1 > y2) //y = y - m -- x = + 1 pendientes negativas igual a 1 -M=1 izq-der arriba-abajo
                    {
                        txtlineaca.Text = "Caso Especial m= -1 IZQ-DER ARRI-ABA";
                        for (int i = Convert.ToInt32(txtx3.Text); i <= x2; i++)
                        {
                            lista = "( " + i + " , " + ya1 + " )";
                            this.tabladatosca.Items.Add(lista);
                            y1 = y1 - Math.Abs(pendiente1);
                            ya1 = Convert.ToSingle(Math.Round(y1, 2));
                        }
                    }
                    else
                    {
                        txtlineaca.Text = "ERROR";
                    }
                }
            }

        }


        private void btnlimpiar_Click(object sender, EventArgs e)
        {
            txtangulo.Clear();
            txtangulo2.Clear();
            txtanguloca.Clear();
            txtlinea.Clear();
            txtlinea2.Clear();
            txtlineaca.Clear();
            txtpendiente.Clear();
            txtpendiente2.Clear();
            txtpendienteca.Clear();
            txtx1.Clear();
            txtx2.Clear();
            txtx3.Clear();
            txty1.Clear();
            txty2.Clear();
            txty3.Clear();

            pictureBox1.Refresh();
            this.tabladatos.Items.Clear();
            this.tabladatosbc.Items.Clear();
            this.tabladatosca.Items.Clear();






        }

        private void First_Load(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void txtangulo2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtpendiente2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtlinea2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }
}

    
        
    

