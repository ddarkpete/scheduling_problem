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
namespace scheduing_fs_ts
{//trzeba zapisać instancje do pliku , potem nawet wczytać!
 //w zapisie bug , poprawiony dla taskow poprawic dla pauz
    public partial class Form1 : Form
    {
        


        /*
         * 
         *                         OBSŁUGA ZAPISÓW/WCZYTYWAŃ I OKIENKA
         * 
         * 
         */



        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Int32.TryParse(textBox1.Text, out N);
            task_generator();
            pause_generator();
            SortedTasksglobal = Tasks.OrderBy(o => o.start).ToList();
            SortedPauses = Pauses.OrderBy(o => o.p_start).ToList();
            tabu();
            //  System.Console.WriteLine("{0} {1}", pause_instances.Count, task_instances.Count);
            saveFileDialog1.ShowDialog();
            textBox1.Text = "";
            time_mach1 = 0;
            time_mach2 = 0;
            //a to nie powinno być przy wprowadzaniu pliku instancji? czy za jednym zamachem generujemy ,
            //rozwiazujemy i tworzymy plik instancji oraz rozwiązania? chyba powinno być wczytanie instancji tez
            //zeby dr Radom mogl sprawdzic to
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string save_file = saveFileDialog1.FileName;
            save(save_file);
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)//mozna wpisywac liczbe zadan i enter
        {
            if (e.KeyChar == (char)13)
            {
                button1.PerformClick();
                e.Handled = true;//nie ma dzwieku po enterze
            }
        }
        

        private void load_button_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string load_file = openFileDialog1.FileName;
            load(load_file);
        }
    }
}
