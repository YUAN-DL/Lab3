using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;

namespace Lab3
{
    class VMBenchmark
    {
        List<VMtime> results_time;
        List<VMAccuracy> results_accuracy;
        public VMBenchmark()
        {
            results_time = new List<VMtime>();
            results_accuracy = new List<VMAccuracy>();

        }
        public void create(double left_boundary, double right_boundary, int n)
        {
            double[] timeVMS = new double[2];
            double[] timeVMD = new double[2];

            double[] d_x = new double[n];
            float[] f_x = new float[n];
            double[] d_y_HA = new double[n];
            double[] d_y_EP = new double[n];
            float[] f_y_HA = new float[n];
            float[] f_y_EP = new float[n];
            VMtime vMtime1;
            VMAccuracy accuracy;
            vMtime1.lenth_vector = n;
            int ret = -1;
            float scale = (float)(right_boundary - left_boundary) / n;
            f_x[0] =(float) left_boundary;
            for (int i =1 ; i < n; i++)
            {
                f_x[i] =f_x[i-1]+scale;
            }
            VMS_LN(n, f_x, f_y_HA, f_y_EP, timeVMS, ref ret);
            VMD_LN(n, d_x, d_y_HA, d_y_EP, timeVMD, ref ret);
            vMtime1.time_relation_1 = timeVMS[0] /timeVMD[0];
            vMtime1.time_relation_2 = timeVMS[1] / timeVMD[0];
            vMtime1.time_relation_3 = timeVMD[1] / timeVMD[0];
            /*  time_relation_1;  vms_ln(VML_HA)/vmdln(VML_HA)
                time_relation_2;  vms_ln(VML_EP)/vmdln(VML_HA)
                time_relation_3;  vmd_ln(VML_EP)/vmdln(VML_HA)*/
            results_time.Add(vMtime1);
            accuracy.boundary1 = left_boundary;
            accuracy.boundary2 = right_boundary;
            accuracy.len_vec_arg_func = n;
            double max_error_moudle = -1;
            double max_error=0;
            for (int i = 0; i < n; i++)
            {   
                double module1= Math.Abs(f_y_HA[i] - f_y_EP[i]) / Math.Abs(f_y_HA[i]);
                double module2 = Math.Abs(d_y_HA[i] - d_y_EP[i]) / Math.Abs(d_y_HA[i]);
                double error1 = Math.Abs(f_y_HA[i] - f_y_EP[i]);
                double error2 = Math.Abs(d_y_HA[i] - d_y_EP[i]);
                if (module1<module2)
                {
                    if(max_error_moudle < module2)
                    {
                        max_error_moudle = module2;
                    }
                }
                else if(max_error_moudle<module1)
                {
                    max_error_moudle = module1;
                }
                if(error1<error2)
                {
                    if(max_error<error2)
                    {
                        max_error = error2;
                    }
                }
                else if(max_error<error1)
                {
                    max_error = error1;
                }
            }
            accuracy.Max_error_module = max_error_moudle;
            accuracy.MAX_error = max_error;
            results_accuracy.Add(accuracy);
        }
        public override string ToString()
        {
            string str ="";
            for (int i = 0; i < results_time.Count; i++)
            {
                str += $"VMtime {i}:\n";
                str += "Длина вектора: " +results_time[i].lenth_vector.ToString()+"\n";
                str += "Отношение времени vms_ln(VML_HA)/vmdln(VML_HA): " + results_time[i].time_relation_1+"\n";
                str += "Отношение времени vms_ln(VML_EP)/vmdln(VML_HA): " + results_time[i].time_relation_2 + "\n";
                str += "Отношение времени vmd_ln(VML_EP)/vmdln(VML_HA): " + results_time[i].time_relation_3 + "\n";

                str += $"VMAccuracy {i}:"+"\n";
                str += "Границы отрезка: " + results_accuracy[i].boundary1 + " " + results_accuracy[i].boundary2 + "\n";
                str += "Длина вектора: " + results_accuracy[i].len_vec_arg_func.ToString() + "\n";
                str += "Максимальное значение отношения модуля разности значений: " + results_accuracy[i].Max_error_module + "\n";
                str += "Значение функции,которые отличаются максимальнов в режимах  VML_HA и WML_EP: " + results_accuracy[i].MAX_error + "\n";
            }
            return str;
        }
        public bool Write_txt(string filename)
        {

            try
            {
                for (int i = 0; i < results_time.Count; i++)
                {
                    using (StreamWriter sw = File.AppendText(filename))
                    {
                        sw.WriteLine(results_time[i].lenth_vector + " " + results_time[i].time_relation_1+" "+ results_time[i].time_relation_2 +" "+ results_time[i].time_relation_3);
                        sw.WriteLine(results_accuracy[i].boundary1 + " " + results_accuracy[i].boundary2 + " "+ results_accuracy[i].len_vec_arg_func + " "+ results_accuracy[i].Max_error_module + " "+ results_accuracy[i].MAX_error);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"An error occurred while saving data\n{ex.Message}");
            }
            return true;

        }
        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\CPP_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
          void VMS_LN(int n, float[] a, float[] y_HA, float[] y_EP, double[] time, ref int ret);
        [DllImport("..\\..\\..\\..\\x64\\DEBUG\\CPP_DLL.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern
           void VMD_LN(int n, double[] a, double[] y_HA, double[] y_EP, double[] time, ref int ret);
    }

}
