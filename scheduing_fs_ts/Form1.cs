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
{
    public partial class Form1 : Form
    {
        int instance_nO = 0;
        int N = 30;
        Random rnd = new Random();
        int time_mach1 = 0;
        int time_mach2 = 0;
        public class task
        {
                public int maszyna_op1;
                public int duration_op1;
                public int duration_op2;
                public int maszyna_op2;
                public int start;
                //public int pause;
        };
        private List<task> tasks = new List<task>();
        //private List<List<task>> task_instances = new List<List<task>>();

        class pause
        {
            public int p_id;
            public int p_start;
            public int p_end;
            public int p_duration;

        };
         private List<pause> pauses = new List<pause>();
         //private List<List<pause>> pause_instances = new List<List<pause>>();

        public void task_generator()
        {
            //List<task> tasks = new List<task>();
            for (int i = 0; i < N; i++)//N liczba  zadan
            {
                task new_task = new task();
                new_task.maszyna_op1 = 1;
                new_task.maszyna_op2 = 2;
                if(i % 2 == 0)
                {
                    new_task.duration_op1 = rnd.Next(1, 200);
                    new_task.duration_op2 = rnd.Next(1, 200);
                }
                else
                {
                    new_task.duration_op1 = rnd.Next(1, 50);
                    new_task.duration_op2 = rnd.Next(1, 50);
                }
                
                time_mach1 += new_task.duration_op1;
                time_mach2 += new_task.duration_op2;
                if (i <= N/2)
                {
                    new_task.start = 0;
                }
                
                tasks.Add(new_task);



            }

            for(int j = N/2 + 1; j < N; j++)
            {
                tasks[j].start = rnd.Next(1, (time_mach1 + time_mach2) * 1 / 4);
                
            }
            
         }
        public void pause_generator()
        {
            int pause_number = rnd.Next(N/5,N);
            for (int i = 0; i < pause_number; i++)
            {
                pause temp_pause = new pause();
                temp_pause.p_id = i;
                temp_pause.p_duration = rnd.Next(1, 20);//górna granica czasu?
                temp_pause.p_start = rnd.Next(1, time_mach1 - temp_pause.p_duration - 1);
                temp_pause.p_end = temp_pause.p_start + temp_pause.p_duration;
                pauses.Add(temp_pause);
                
            }
            
        }
        private void save(string path)
        {
            StreamWriter sr = new StreamWriter(path);
            sr.WriteLine("***{0}***", instance_nO);
            sr.WriteLine("{0}", tasks.Count);
            foreach (task taskk in tasks)
                {
                    Console.WriteLine("{0};{1};{2};{3};", taskk.duration_op1, taskk.duration_op2, taskk.maszyna_op1, taskk.maszyna_op2, taskk.start);//czas_operacji1_1; czas_operacji2_1; nr_maszyny_dla_op1_1; nr_maszyny_dla_op1_2; 
                    sr.WriteLine("{0};{1};{2};{3},;", taskk.duration_op1 , taskk.duration_op2,taskk.maszyna_op1,taskk.maszyna_op2,taskk.start);//czas_operacji1_1; czas_operacji2_1; nr_maszyny_dla_op1_1; nr_maszyny_dla_op1_2; 
                }
                
                int x = 0;
                sr.WriteLine("{0}", pauses.Count);
                foreach (pause pausee in pauses)
                {
                    sr.WriteLine("{0};{1};{2};", x, pausee.p_duration, pausee.p_start);
                    x++;
                }
                sr.WriteLine("***EOF***");
            
            sr.Close();
            tasks.Clear();
            pauses.Clear();
        }



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
            saveFileDialog1.ShowDialog();
            textBox1.Text = "";
            time_mach1 = 0;
            time_mach2 = 0;
            
        }

        private void save_button_Click(object sender, EventArgs e)
        {
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string save_file = saveFileDialog1.FileName;
            save(save_file);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                button1.PerformClick();
                e.Handled = true;
            }
        }
    }
}
