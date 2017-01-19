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
        string loaded_instance_id;
        int instance_nO = 0;
        int N = 30;
        Random rnd = new Random();
        int time_mach1 = 0;
        int time_mach2 = 0;
        public class Task
        {
            public int maszyna_op1;
            public int duration_op1;
            public int duration_op2;
            public int maszyna_op2;
            public int start;
            //public int pause;
        };
        private List<Task> Tasks = new List<Task>();
        List<Task> SortedTasksglobal = new List<Task>();
        public class Pause
        {
            public int p_id;
            public int p_start;
            public int p_end;
            public int p_duration;
        };
        private List<Pause> Pauses = new List<Pause>();
        private List<Pause> SortedPauses = new List<Pause>();
        public void task_generator()
        {

            for (int i = 0; i < N; i++)//N liczba  zadan
            {
                Task New_task = new Task();
                New_task.maszyna_op1 = 1;
                New_task.maszyna_op2 = 2;
                if (i % 2 == 0)
                {
                    New_task.duration_op1 = rnd.Next(1, 200);
                    New_task.duration_op2 = rnd.Next(1, 200);
                }
                else
                {
                    New_task.duration_op1 = rnd.Next(1, 50);
                    New_task.duration_op2 = rnd.Next(1, 50);
                }

                time_mach1 += New_task.duration_op1;
                time_mach2 += New_task.duration_op2;
                if (i <= N / 2)
                {
                    New_task.start = 0;
                }

                Tasks.Add(New_task);
            }
            for (int j = N / 2 + 1; j < N; j++)
            {
                Tasks[j].start = rnd.Next(1, (time_mach1 + time_mach2) * 1 / 4);

            }
            //buffer_task = tasks;
            /*     task_instances.Add(tasks);
                 System.Console.WriteLine("{0}", task_instances[task_instances.Count - 1].Count);
                 //tasks.Clear();
                 System.Console.WriteLine("{0}", task_instances[task_instances.Count - 1].Count);*/
        }
        public void pause_generator()
        {

            int pause_number = rnd.Next(N / 5, N);
            for (int i = 0; i < pause_number; i++)
            {
                Pause Temp_pause = new Pause();
                Temp_pause.p_id = i;
                Temp_pause.p_duration = rnd.Next(1, 20);//górna granica czasu?
                Temp_pause.p_start = rnd.Next(1, time_mach1 - Temp_pause.p_duration - 1);
                Temp_pause.p_end = Temp_pause.p_start + Temp_pause.p_duration;
                Pauses.Add(Temp_pause);

            }

        }
       

        public int count_time(List<Task> SortedTasks)// JESZCZE NIE OK 
        {
            
            int m1_time = 0;
            int m2_time = 0;
            List<Task> copy = SortedTasks.ToList();
            List<int> end_op1 = new List<int>();
            List<int> end_op2 = new List<int>();
            //List<bool> scheduled_op1
            for (int i = 0; i < SortedTasks.Count; i++)
            {
                end_op1.Add(0);
                end_op2.Add(0);
            }
            bool scheduled = false;
            while (scheduled == false)
            {
                //Console.WriteLine("twoja stara ty ry ty ry");
                if (end_op1.Contains(0))//to chyba jednak nie jest najlepszy pomysł bo przy szeregowaniu op2
                {               //korzystamy z indeksów a jak zaczniemy usuwać to się zaburzy struktura indeksow
                                //moznaby użyć metody List.Contains + lista bool czy uszeregowane
                   /* foreach ( int end in end_op1)
                    {
                        Console.Write("{0} ",end);
                    }*/
                

                
                    for (int i = 0; i < copy.Count; i++)
                    {
                        if (copy[i].start <= m1_time && end_op1[i] == 0)
                        {
                            int p_counter = 0;

                            // taskBox.Text += m1_time + System.Environment.NewLine;
                            foreach (Pause pause in Pauses)
                            {
                                if (m1_time <= pause.p_start && pause.p_start < (m1_time + copy[i].duration_op1))
                                {
                                    Console.WriteLine("jebs w pauze");
                                    int before_pause = pause.p_start - m1_time;
                                    m1_time += before_pause + pause.p_duration + copy[i].duration_op1;
                                    p_counter++;

                                }

                            }
                            if (p_counter == 0)
                            {
                                m1_time += copy[i].duration_op1;
                                end_op1[i] = m1_time;
                                break;
                            }
                            else
                            {
                                m1_time += copy[i].duration_op1;
                                end_op1[i] = m1_time;
                                break;
                            }


                        }
                        else
                        {
                            m1_time++;
                            Console.WriteLine("nie pasuje w chuj");

                        }
                      }
                    // taskBox.Text += "Operacje 2" + System.Environment.NewLine;
                    for (int i = 0; i < copy.Count; i++)
                    {

                        if (i > 0)
                        {
                            if (end_op1[i] != 0 && end_op2[i - 1] != 0)
                            {

                                // taskBox.Text += m2_time + System.Environment.NewLine;
                                if (end_op2[i - 1] > end_op1[i])
                                {
                                    m2_time = end_op2[i - 1];
                                    m2_time += copy[i].duration_op2;
                                    end_op2[i] = m2_time;
                                }

                                else if (end_op1[i] > end_op2[i - 1])
                                {
                                    m2_time = end_op1[i];
                                    m2_time += copy[i].duration_op2;
                                    end_op2[i] = m2_time;
                                }
                            }
                        }
                        else
                        {
                            if (end_op1[0] != 0)
                            {
                                m2_time = end_op1[0];
                                m2_time += copy[0].duration_op2;
                                end_op2[0] = m2_time;
                            }

                        }
                    }
                }
                else { scheduled = true; }
            }
           // taskBox.Text += "Time of machine 1: " + m1_time + System.Environment.NewLine;
            taskBox.Text += "Total time: " + m2_time + System.Environment.NewLine;
            return m2_time; 
        }

        public void tabu()
        {
            count_time(SortedTasksglobal);


        }


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
        private void load(string path)
        {
            int tasks_count;
            int pauses_count;
            
            List<Task> loaded_tasks = new List<Task>();
            List<Pause> loaded_pauses = new List<Pause>();
            StreamReader sr = new StreamReader(path);
            loaded_instance_id = sr.ReadLine();
            Int32.TryParse(sr.ReadLine(), out tasks_count);
           for (int i = 0; i < tasks_count; i++)
            {
                string loaded_task = sr.ReadLine();
                string[] split = loaded_task.Split(';');
                Console.WriteLine("{0}", split.Length);
                Console.WriteLine("{0};{1};{2};{3};{4};", split[0], split[1], split[2], split[3], split[4]);
                Task TempLoad_task = new Task();//czas_operacji1_1; czas_operacji2_1; nr_maszyny_dla_op1_1; nr_maszyny_dla_op1_2; 
                Int32.TryParse(split[0], out TempLoad_task.duration_op1);
                Int32.TryParse(split[1], out TempLoad_task.duration_op2);
                Int32.TryParse(split[2], out TempLoad_task.maszyna_op1);
                Int32.TryParse(split[3], out TempLoad_task.maszyna_op2);
                Int32.TryParse(split[4], out TempLoad_task.start);
                loaded_tasks.Add(TempLoad_task);
            }
            Int32.TryParse(sr.ReadLine(), out pauses_count);
            for(int j =0; j < pauses_count;j++)
            {
                Pause Temp_pause = new Pause();
                string loaded_pause = sr.ReadLine();
                string[] split = loaded_pause.Split(';');
                Int32.TryParse(split[0], out Temp_pause.p_id);
                Int32.TryParse(split[1], out Temp_pause.p_duration);
                Int32.TryParse(split[2], out Temp_pause.p_start);
                Temp_pause.p_end = Temp_pause.p_start + Temp_pause.p_duration;
                loaded_pauses.Add(Temp_pause);

            }

        }

        private void save(string path)
        {
            StreamWriter sr = new StreamWriter(path);
            sr.WriteLine("***{0}***", instance_nO); //tu numer instancj8i zrob PIt
            instance_nO++;
            sr.WriteLine("{0}", Tasks.Count);
            foreach (Task taskk in SortedTasksglobal)
            {
                Console.WriteLine("{0};{1};{2};{3};{4};", taskk.duration_op1, taskk.duration_op2, taskk.maszyna_op1, taskk.maszyna_op2, taskk.start);//czas_operacji1_1; czas_operacji2_1; nr_maszyny_dla_op1_1; nr_maszyny_dla_op1_2; 
                sr.WriteLine("{0};{1};{2};{3};{4};", taskk.duration_op1, taskk.duration_op2, taskk.maszyna_op1, taskk.maszyna_op2, taskk.start);//czas_operacji1_1; czas_operacji2_1; nr_maszyny_dla_op1_1; nr_maszyny_dla_op1_2; 
                                                                                                                                                // 
            }

            int x = 0;
            sr.WriteLine("{0}", Pauses.Count);
            foreach (Pause pause in Pauses)
            {
                sr.WriteLine("{0};{1};{2};", x, pause.p_duration, pause.p_start);
                x++;
            }

            sr.WriteLine("***EOF***");
            sr.Close();
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
