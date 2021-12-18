using System;
using System.Text;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Lab3
{
    struct VMtime
    {
       public double lenth_vector;
       public double time_relation_1; // vms_ln(VML_HA)/vmdln(VML_HA)
       public double time_relation_2; // vms_ln(VML_EP)/vmdln(VML_HA)
       public double time_relation_3; // vmd_ln(VML_EP)/vmdln(VML_HA)
    }
    struct VMAccuracy
    {
        public double boundary1;
        public double boundary2;
        public int len_vec_arg_func;
        public double Max_error_module;
        public double MAX_error;
   
    }

    class Program
    {
        static void Main(string[] args)
        {   
            int n = 10000000;
            //time_VMS_LN_HA, time_VMS_LN_EP, time_VMD_LN_HA, time_VMD_LN_EP;

            VMBenchmark benchmark = new VMBenchmark();
            benchmark.create(234, 2478687, n);
            benchmark.create(354, 2457563, n);
            benchmark.create(1247, 10000000, n);
            benchmark.Write_txt("out.txt");//\bin\Debug\net5.0\out.txt
            Console.WriteLine(benchmark.ToString());

        }
    }
}
