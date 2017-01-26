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
            Int32.TryParse(textBox1.Text, out Backend.Instance.N);
            Backend.Instance.task_generator();
            Backend.Instance.pause_generator();
            Backend.Instance.SortedTasksglobal = Backend.Instance.Tasks.OrderBy(o => o.start).ToList();
            Backend.Instance.SortedPauses = Backend.Instance.Pauses.OrderBy(o => o.p_start).ToList();
          //  Backend.Instance.count_time(Backend.Instance.SortedTasksglobal, this);// tabu dopisać 
           // Backend.Instance.tabu(this);
            //  System.Console.WriteLine("{0} {1}", pause_instances.Count, task_instances.Count);
            saveFileDialog1.ShowDialog();
            Backend.Instance.SortedTasksglobal.Clear();
            Backend.Instance.SortedPauses.Clear();
            Backend.Instance.Tasks.Clear();
            Backend.Instance.Pauses.Clear();
            textBox1.Text = "";
            //Backend.Instance.time_mach1 = 0;
            //Backend.Instance.time_mach2 = 0;
            //a to nie powinno być przy wprowadzaniu pliku instancji? czy za jednym zamachem generujemy ,
            //rozwiazujemy i tworzymy plik instancji oraz rozwiązania? chyba powinno być wczytanie instancji tez
            //zeby dr Radom mogl sprawdzic to
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string save_file = saveFileDialog1.FileName;
            Backend.Instance.save(save_file);
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
            Console.Write("cycki");
            openFileDialog1.ShowDialog();
            Console.Write("cycki");
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string load_file = openFileDialog1.FileName;
            Backend.Instance.load(load_file);
        }

        private void tabu_button_Click(object sender, EventArgs e)
        {
            Backend.Instance.tabu(this);
            Backend.Instance.SortedTasksglobal.Clear();
            Backend.Instance.SortedPauses.Clear();
            Backend.Instance.Tasks.Clear();
            Backend.Instance.Pauses.Clear();
            Backend.Instance.time_mach1 = 0;
            Backend.Instance.time_mach2 = 0;

        }
    }
}
