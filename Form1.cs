using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Threading;
using System.Security.Policy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using Microsoft.VisualBasic.ApplicationServices;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Buscador_Archivos
{
    public partial class Form1 : Form
    {
        //https://youtu.be/bHdDrCfD890
        //https://youtu.be/j7beBfVCe6Q

        static StreamReader lecturaTexto, lecturaContador, lecturaTexto2, lecturaContador2;
        static StreamWriter escritura, escritura2;
        string cadenaContador, cadenaContador2;

        String borrarArchivo;
        



        int pausar = 0;

        int numeral = 0;
        int numeral2 = 0;
        int i_prueba = 0;
        int j_prueba = 0;
        int contador = 1;
    

        string original;
        string direccionModif;
        string extencion;
        string nombreArchivoCapeta;

        string pop2, directoryPrueba;

        double archivoSize;
        string sizeStringDato;

        string datos = "";

        public Form1()
        {
            InitializeComponent();
            this.Size = new System.Drawing.Size(1600, 730);
            
                 //textBox2.Size = new System.Drawing.Size(1050, 30);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Maximum = 0;
            progressBar1.Minimum = 0;

            labelCancelado.Text = "Cancel";
            labelCancelado.Visible = false;

        }

        string CarpetaBuscqueda;

        Computer ju = new Computer();
        private void buttonCarpeta_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog carpeta = new FolderBrowserDialog();
            carpeta.RootFolder = Environment.SpecialFolder.MyComputer;

      


            if (carpeta.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CarpetaBuscqueda = carpeta.SelectedPath;
                label1.Text = CarpetaBuscqueda;
                buttonBuscar.Enabled = true;

                pop2 = label1.Text;
                
            }
        }
      
        private void buttonBuscar_Click(object sender, EventArgs e)
        {
            labelConteo.Visible = false;

            BackgroundWorker tarea = new BackgroundWorker();
            tarea.DoWork += HacerAlgo;
            tarea.RunWorkerCompleted += Terminado;
            tarea.RunWorkerAsync();

            //---------------------------------------------------------
            labelCancelado.Visible=false;
            labelAccionProceso.Visible = true;

            labelAccionProceso.Text = "Procesando...";

            contador = 1;
            //listBox1.Items.Clear();
            dataGridView1.Rows.Clear();

            buttonBuscar.Enabled = false;
            button1.Enabled = false;
            buttonCarpeta.Enabled = false;

            button3.Enabled = true;
            //button4.Enabled = true;
            button2.Enabled = true;

            textBox2.Text = "";
            textBox3.Text = "";

            CarpetaBuscqueda = label1.Text;

            progressBar1.Visible = true;
            progressBar1.Maximum = 100;
            progressBar1.Minimum = 1;

        }

        private void HacerAlgo(object sender, DoWorkEventArgs e)
        {



            if (label1.Text == "Seleccione Directorio")
            {
                MessageBox.Show("Debe seleccionar la Carpeta o Directorio");
            }
            else
            {
  
                numeral = Directory.EnumerateFiles(CarpetaBuscqueda).Count();
                numeral2 = Directory.EnumerateDirectories(CarpetaBuscqueda).Count();
                i_prueba = 0;
                j_prueba = 0;
                //------------------------------------------------------------------------------
                ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart(Motor);
                Thread T1 = new Thread(parameterizedThreadStart);

                T1.Start(CarpetaBuscqueda);


                T1.Join();

              

            }

           
        }

        private void Terminado(object sender, RunWorkerCompletedEventArgs e)
        {
            labelAccionProceso.Text = "Listo";

            buttonBuscar.Enabled = true;
            button1.Enabled = true;
            buttonCarpeta.Enabled = true;


            //buttonBuscar.BackColor = Color.FromArgb(0, 192, 0);

            button3.ForeColor = Color.White;
            //button4.ForeColor = Color.White;


            button3.Enabled = false;
            //button4.Enabled = false;
            button2.Enabled = false;

            //Thread.Sleep(1000);
            progressBar1.Maximum = 0;
            progressBar1.Minimum = 0;

            progressBar1.Visible = false;

            labelConteo.Visible = true;

            labelConteo.Location = new Point(782, 20);
           

            labelConteo.Text = dataGridView1.RowCount.ToString() + " Files";

        }

        private delegate void UpdateUi(string name,string ext, double s,string sS,string fn);

        private delegate void UpdateUiDelegateDoSome2();
        private delegate void UpdateUiDelegateDone();

        public void UiDoSome(string name,string ext,double size,string sizeString,string fullname)
        {
           

            dataGridView1.Rows.Add(contador,name,ext.Replace(".",""),size, sizeString,fullname);
            contador++;
        }

      


        public void Motor(object mot)
         {

            string pop = mot.ToString();

         

            try
            {


                if (pop == "C:\\" || pop == "C:\\Users")
                {
                    //MessageBox.Show("hola");
                    return;
                }
                
                pop2 = mot.ToString();

                numeral = Directory.EnumerateFiles(pop).Count();


                int i;


                // ARCHIVOS
                for (i = i_prueba; i < numeral ; i++)
                {
                    FileInfo k = new FileInfo(Directory.GetFiles(pop)[i]);

                        string variable = k.Name.Replace(" ", "").ToUpper();
                        string variable2 = textBox1.Text.ToUpper().Replace(" ","").Replace("  ", "");

                    
                    string kName = k.Name.Replace(".mp4","").Replace(".mkv", "").Replace(".avi", "").Replace(".ia", "");
                    string extencion = k.Extension.ToUpper();

                    if (k.FullName.Contains(".ia"))
                    {
                        extencion = "ia " + extencion.Replace(".","");


                    }

                    if (variable.Contains(variable2))
                    {
                    
                        //listBox1.Items.Add(k.FullName);
                        archivoSize = new FileInfo(k.FullName).Length;

                        if (archivoSize > 1048576)
                        {
                            if (archivoSize > 1073741824)
                            {
                                //GB
                                archivoSize = Math.Round((archivoSize) / 1073741824, 2);
                                sizeStringDato = "GB";

                                dataGridView1.Invoke(new UpdateUi(UiDoSome), kName,extencion, archivoSize , sizeStringDato,k.FullName);
                            }
                            else
                            {
                                //MB
                                archivoSize = Math.Round((archivoSize) / 1048576, 2);
                                sizeStringDato = "MB";
                                dataGridView1.Invoke(new UpdateUi(UiDoSome), kName, extencion, archivoSize, sizeStringDato, k.FullName);
                            }
                        }
                        else
                        {
                            // Bytes
                            sizeStringDato = "Bytes";
                            dataGridView1.Invoke(new UpdateUi(UiDoSome), kName, extencion, archivoSize, sizeStringDato, k.FullName);
                        }

                        
                        //listBox1.Invoke(new UpdateUi(UiDoSome), k.FullName);


                    }
                    else if (textBox1.Text == "")
                    {

                        if (archivoSize > 1048576)
                        {
                            if (archivoSize > 1073741824)
                            {
                                //GB
                                archivoSize = Math.Round((archivoSize) / 1073741824, 2);
                                sizeStringDato = "GB";

                                dataGridView1.Invoke(new UpdateUi(UiDoSome), kName, extencion, archivoSize, sizeStringDato, k.FullName);
                            }
                            else
                            {
                                //MB
                                archivoSize = Math.Round((archivoSize) / 1048576, 2);
                                sizeStringDato = "MB";

                                dataGridView1.Invoke(new UpdateUi(UiDoSome), kName, extencion, archivoSize, sizeStringDato, k.FullName);
                            }
                        }
                        else
                        {
                            // Bytes
                           

                            sizeStringDato = "Bytes";
                            dataGridView1.Invoke(new UpdateUi(UiDoSome), kName, extencion, archivoSize, sizeStringDato, k.FullName);
                        }
                      
                    }
                    
                }

                //// CARPETAS
            

                if (Directory.GetDirectories(pop).Count() > 0)
                {
                    

                    for (int j = j_prueba; j < Directory.GetDirectories(pop).Count(); j++)
                    {

                        string variable = Directory.GetDirectories(pop)[j].ToString().ToLower();
                        string variable2 = textBox1.Text.ToLower().Replace(" ", "").Replace("  ", "");

                        Motor(Directory.GetDirectories(pop)[j]);


                    }

            }


           
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //MessageBox.Show(ex.Message, ex.StackTrace);
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            textBox2.Text = "";
            textBox3.Text = "";

            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                BackgroundWorker tarea = new BackgroundWorker();
            tarea.DoWork += HacerAlgo;
            tarea.RunWorkerCompleted += Terminado;
            tarea.RunWorkerAsync();

            //---------------------------------------------------------
            labelCancelado.Visible=false;
            labelAccionProceso.Visible = true;

            labelAccionProceso.Text = "Procesando...";

            listBox1.Items.Clear();

            buttonBuscar.Enabled = false;
            button1.Enabled = false;
            buttonCarpeta.Enabled = false;

            button3.Enabled = true;
            //button4.Enabled = true;
            button2.Enabled = true;

            

            CarpetaBuscqueda = label1.Text;

            progressBar1.Maximum = 100;
            progressBar1.Minimum = 1;
            }
        }

        private void label1_TextChanged(object sender, EventArgs e)
        {
            string variable;

            if (label1.Text.Contains(".lnk"))
            {
                variable = label1.Text;
                variable = variable.Replace("- Shortcut.lnk", "");

                if (label1.Text.Contains("Desktop"))
                {
                    variable = variable.Replace("Desktop", "source");
                    CarpetaBuscqueda = variable.Trim();
                }
            }
            else
            {
                    CarpetaBuscqueda = label1.Text.Trim();
            }
        }

       

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
          


        }

        private void button5_Click(object sender, EventArgs e)
        {
         


        }


    

        int contador10 = 0;
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            string valor  = "";
            string valor2 = "";

            int rowIndex = dataGridView1.CurrentCell.RowIndex;
            int columnIndex = dataGridView1.CurrentCell.ColumnIndex;

            textBox2.Text = "";
            textBox3.Text = "";


            if (e.RowIndex == -1) return;

            if (contador10 != 0)
            {



                //if (dataGridView1.CurrentCell.ColumnIndex.Equals(1))
                //{
                //dataGridView1.CurrentRow.Selected = true;
                valor = dataGridView1.Rows[e.RowIndex].Cells["Descripcion"].FormattedValue.ToString();

                textBox2.Text = valor;



                if (valor != null && valor2 != null)
                {
                    valor2 = dataGridView1.Rows[e.RowIndex].Cells[5].FormattedValue.ToString();
                    //Clipboard.SetData(DataFormats.StringFormat, valor2);

                    original = valor2;

            }





            textBox2.Text = valor;



                direccionModif = original.Substring(original.LastIndexOf("\\") + 1);
                extencion = original.Substring(original.LastIndexOf(".") + 1);
                nombreArchivoCapeta = direccionModif.Replace("." + extencion, "");

                //Clipboard.SetData(DataFormats.StringFormat, original);


                if (extencion.Contains(direccionModif))
                {
                    textBox2.Text = nombreArchivoCapeta;
                    textBox3.Text = "";
                }
                else
                {
                    textBox2.Text = nombreArchivoCapeta;
                    textBox3.Text = extencion;

                }
                //}
            }
            contador10 = 1;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {          
     
        }

        private void textBoxFiltrar_TextChanged(object sender, EventArgs e)
        {
       
        }


     
        private void button1_Click(object sender, EventArgs e)
        {
            bool variable;

            if (dataGridView1.RowCount != 0)
            {
                //extencion = extencion.ToLower();

                //if (extencion != "ini" && extencion != "lnk")
                //{

                //string valor;

                    if (MessageBox.Show("Desea borrar este archivo?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow Rw in dataGridView1.SelectedRows)
                        {
                            dataGridView1.Rows.Remove(Rw);

                            string varia = Rw.Cells[5].Value.ToString();
                            File.Delete(varia); // Borra archivos de la computadora
                        }
                    }
                       
                                textBox2.Text = "";
                                textBox3.Text = "";


                    }


            }
             
        }


        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {


        }

    

     
        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Estas seguro que quieres detener el proceso?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

            labelAccionProceso.Text = "";

            numeral = 0;
            numeral2 = 0;
            j_prueba = 998877755;

            button3.ForeColor = Color.White;
            //button4.ForeColor = Color.White;

            button3.Enabled = false;
            //button4.Enabled = false;


            button1.Enabled = true;
            buttonCarpeta.Enabled = true;
            buttonBuscar.Enabled = true;

            button2.Enabled = false;

            labelAccionProceso.Visible = false;
            labelCancelado.Visible = true;
            labelCancelado.Text = "Cancel";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            numeral = 0;
            numeral2 = 0;

            Application.DoEvents();
            Thread.Sleep(100);

            Application.Exit();

        }
        private void button3_Click(object sender, EventArgs e)
        {
          

           
        }


        private void button4_Click(object sender, EventArgs e)
        {
        

        }

      

        private void button5_Click_1(object sender, EventArgs e)
        {
            busquedaEnGrid(dataGridView1, 1);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string texto = "";

            if (original != null)
            {
                int rowInt = dataGridView1.CurrentCell.RowIndex;
                int columnInt = dataGridView1.CurrentCell.ColumnIndex;

                texto = dataGridView1.Rows[rowInt].Cells[columnInt].Value.ToString();
                Clipboard.SetData(DataFormats.StringFormat, texto);
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            // CAMBIAR NOMBRE DE ARCHIVO O CARPETA
            nombreArchivoCapeta = textBox2.Text;
            extencion = textBox3.Text;

            if (!textBox2.Text.Contains("?"))
            {
                //string direccionModif = textBox2.Text.Substring(textBox2.Text.LastIndexOf("\\") + 1);

                Process test = new Process();
                test.StartInfo.FileName = "cmd.exe";
                test.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                // uso c para que se cierre la ventana de cmd
                test.StartInfo.Arguments = $@"/c ren {'"' + original + '"'} {'"' + nombreArchivoCapeta + "." + extencion + '"'}";
                //test.StartInfo.Arguments = $@"/k ren {'"' + original + '"'} {'"' + direccionModif + '"'}";

                test.Start();

                //MessageBox.Show("Modificado");


                if (dataGridView1.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow Rw in dataGridView1.SelectedRows)
                    {
                        dataGridView1.Rows.Remove(Rw);

                        //string varia = Rw.Cells[1].Value.ToString();
                        //File.Delete(varia); // Borra archivos de la computadora
                    }
                }


                //listBox1.Items.Remove(original);  // Borra elementos del listbox
                textBox2.Text = "";

                textBox3.Text = "";

            }


        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button9_MouseClick(object sender, MouseEventArgs e)
        {
            //// QUITAR CARACTERES ESPECIALES

            //-----------------------------------------------------------------------------------------

            bool variable;

            if (dataGridView1.RowCount != 0)
            {
                if (textBox2.Text != "")
                {

                

                if (MessageBox.Show("Desea borrar caracteres especiales?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        foreach (DataGridViewRow Rw in dataGridView1.SelectedRows)
                        {

                            FileInfo k = new FileInfo(Rw.Cells[5].Value.ToString());
                            extencion = k.Extension.ToLower();

                            if (textBox4.Text != "")
                            {
                                nombreArchivoCapeta = k.Name.ToString().Replace(textBox4.Text, "");
                            }
                            else
                            {
                                nombreArchivoCapeta = k.Name.ToString().Replace("-------------------------", "").Replace("------------------------", "").Replace("------------", "").Replace("-----------", "").Replace("-----------", "").Replace("----------", "").Replace("---------", "").Replace("--------", "").Replace("-------", "").Replace("------", "").Replace("-----", "").Replace("----", "").Replace("---", "").Replace("--", "").Replace("'", " ").Replace("(", "-").Replace(")", "-").Replace("[", "-").Replace("]", "-").Replace("{", "-").Replace("}", "-").Replace("!", "").Replace("¡", "").Replace("?", " ").Replace("¿", "").Replace("^", "").Replace(",", ".").Replace("%", "").Replace("&", "and").Replace("`", "").Replace("|", "-").Replace("/", "-").Replace("#", "").Replace(":", ".").Replace("´", " ").Replace("¯", " ").Replace("´", " ").Replace("´", " ");

                            }

                            original = k.FullName;

                            Process test = new Process();
                            test.StartInfo.FileName = "cmd.exe";
                            test.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                            // uso c para que se cierre la ventana de cmd
                            test.StartInfo.Arguments = $@"/c ren {'"' + original + '"'} {'"' + nombreArchivoCapeta + '"'}";
                            //test.StartInfo.Arguments = $@"/k ren {'"' + original + '"'} {'"' + direccionModif + '"'}";

                            test.Start();

                            dataGridView1.Rows.Remove(Rw);

                            //string varia = Rw.Cells[5].Value.ToString();

                        }
                    }

                    textBox2.Text = "";
                    textBox3.Text = "";


                }

            }
            }

        }

        private void button6_Click_1(object sender, EventArgs e)
        {

            //MessageBox.Show(k.Directory.ToString());

            if (original != "" && original != null)
            {
                FileInfo k = new FileInfo(original);
                Process.Start(k.Directory.ToString());
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
        
            if (original != null) {

                if (original.Contains(":") && original.Contains("."))
                {
                    Clipboard.SetText(original);

                    Process.Start(original);
                }

                Clipboard.Clear();
            }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {



            //if (textBoxFiltro.Text != "")
            //{
          
              //}
        }

        int contador2 = 1;
        // BUSCAR DATOS DEL DATAGRIDVIEW
        private void busquedaEnGrid(DataGridView d, int col)
        {
            for (int i = 0; i < d.Rows.Count; i++)
            {
                datos = Convert.ToString(d.Rows[i].Cells[col].Value);

               

                contador2 = 1;

                for (int j = 0; j < d.Rows.Count; j++)
                {
                    if (Convert.ToString(d.Rows[i].Cells[col].Value) == Convert.ToString(d.Rows[j].Cells[col].Value))
                    {


                        if (contador2 > 1)
                        {


                            this.dataGridView1.Rows[i].Cells[1].Style.BackColor = Color.FromArgb(48, 246, 0);
                            this.dataGridView1.Rows[j].Cells[1].Style.BackColor = Color.FromArgb(137, 208, 244);
                        }
                    
                        contador2++;


                    }
                    
                }


                }
        }

    }
}
