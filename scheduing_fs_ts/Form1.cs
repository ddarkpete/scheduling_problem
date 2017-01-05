using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace scheduing_fs_ts
{
    public partial class Form1 : Form
    {
        static int N = 30;
        Random rnd = new Random();
        int time_mach1 = 0;
        int time_mach2 = 0;
        public struct task
        {
                public int maszyna_op1;
                public int duration_op1;
                public int duration_op2;
                public int maszyna_op2;
                public int start;
                //public int pause;
        };
        private List<task> tasks = new List<task>();

        struct pause
        {
            public int p_start;
            public int p_end;
            public int p_duration;

        };
         private List<pause> pauses = new List<pause>();

        public void task_generator()
        {
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
                task temp_task = new task();
                temp_task.maszyna_op1 = 1;
                temp_task.maszyna_op2 = 2;
                temp_task.duration_op1 = tasks[j].duration_op1;
                temp_task.duration_op2 = tasks[j].duration_op2;
                temp_task.start = rnd.Next(1, (time_mach1 + time_mach2) * 1 / 4);
                tasks[j] = temp_task;
            }
        }
        public void pause_generator()
        {
            int pause_number = rnd.Next(N/5,N);
            for (int i = 0; i < pause_number; i++)
            {
                pause temp_pause = new pause();
                temp_pause.p_duration = rnd.Next(1, 20);//górna granica czasu?
                temp_pause.p_start = rnd.Next(1, time_mach1 - temp_pause.p_duration - 1);
                temp_pause.p_end = temp_pause.p_start + temp_pause.p_duration;
                pauses.Add(temp_pause);
                
            }
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
            foreach (task taskk in tasks)
            {
                System.Console.WriteLine("OP1 TIME :{0} OP2 TIME :{1} START TIME:{2}", taskk.duration_op1 , taskk.duration_op2,taskk.start );
            }
            foreach (pause pausee in pauses)
            {
                System.Console.WriteLine("PAUSE START :{0} PAUSE END :{1} PAUSE DURATION:{2}", pausee.p_start, pausee.p_end, pausee.p_duration);
            }

        }
    }
}
