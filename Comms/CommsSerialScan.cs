using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MissionPlanner.Comms;

namespace MissionPlanner.Comms
{
    public class CommsSerialScan
    {
        static public bool foundport = false;
        static public ICommsSerial portinterface;

        static object runlock = new object();
        public static int run = 0;
        public static int running = 0;
        static bool connect = false;

        static int[] bauds = new int[] {115200, 57600, 38400, 19200, 9600};

        static public void Scan(bool connect = false, bool scan_wifi = false)
        {
            foundport = false;
            portinterface = null;
            lock (runlock)
            {
                run = 0;
                running = 0;
            }
            CommsSerialScan.connect = connect;

            List<MAVLinkInterface> scanports = new List<MAVLinkInterface>();

            string[] portlist = SerialPort.GetPortNames();

            foreach (string port in portlist)
            {
                scanports.Add(new MAVLinkInterface()
                {
                    BaseStream = new SerialPort() {PortName = port, BaudRate = bauds[0]}
                });
            }

            // AB ZhaoYJ@2017-02-23 for auto-conn FC
            // add UDP to autoscan list
            // AB ZhaoYJ@2017-04-18 for auto-conn FC
            // add TCP to autoscan list
            // ========= start =========
            if(scan_wifi)
            {
                scanports.Add(new MAVLinkInterface()
                {
                    BaseStream = new TcpSerial()
                });
            }

            // string pname = scanports[2].BaseStream.PortName;
            // ========= end =========

            foreach (MAVLinkInterface inter in scanports)
            {
                if(!foundport)
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(doread, inter);
                }
                else
                {
                    break;
                }
            }
        }

        static void doread(object o)
        {
            lock (runlock)
            {
                run++;
                running++;
            }

            MAVLinkInterface port = (MAVLinkInterface) o;

            try
            {
                port.BaseStream.Open();

                int baud = 0;

                redo:

                lock (runlock)
                {
                    if (run == 0)
                        return;
                }

                DateTime deadline = DateTime.Now.AddSeconds(5);

                while (DateTime.Now < deadline)
                {
                    Console.WriteLine("Scan port {0} at {1}", port.BaseStream.PortName, port.BaseStream.BaudRate);

                    MAVLink.MAVLinkMessage packet;

                    try
                    {
                        packet = port.readPacket();
                    }
                    catch
                    {
                        continue;
                    }

                    if (packet.Length > 0)
                    {
                        port.BaseStream.Close();

                        Console.WriteLine("Found Mavlink on port {0} at {1}", port.BaseStream.PortName,
                            port.BaseStream.BaudRate);

                        foundport = true;
                        portinterface = port.BaseStream;

                        if (CommsSerialScan.connect)
                        {
                            MainV2.comPort.BaseStream = port.BaseStream;

                            doconnect();
                        }

                        lock (runlock)
                        {
                            running--;
                        }

                        return;
                    }

                    if (foundport)
                        break;
                }


                if (!foundport && port.BaseStream.BaudRate > 0)
                {
                    baud++;
                    if (baud < bauds.Length)
                    {
                        port.BaseStream.BaudRate = bauds[baud];
                        goto redo;
                    }
                }

                try
                {
                    port.BaseStream.Close();
                }
                catch
                {
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                lock (runlock)
                {
                    running--;
                }
            }

            Console.WriteLine("Scan port {0} Finished!!", port.BaseStream.PortName);
        }

        static void doconnect()
        {
            if (MainV2.instance == null)
            {
                MainV2.comPort.Open(false);
            }
            else
            {
                if (MainV2.instance.InvokeRequired)
                {
                    MainV2.instance.BeginInvoke(
                        (System.Windows.Forms.MethodInvoker) delegate() { MainV2.comPort.Open(true); });
                }
                else
                {
                    MainV2.comPort.Open(true);
                }
            }
        }
    }
}