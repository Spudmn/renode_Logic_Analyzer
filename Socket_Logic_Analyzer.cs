//
// Copyright (c) 2023 Aaron Keith
// Copyright (c) 2010-2022 Antmicro
// Copyright (c) 2011-2015 Realtime Embedded
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//

using System;
using System.Text;
using Antmicro.Renode.Core;
using Antmicro.Renode.Utilities;
using Antmicro.Renode.Logging;
using Antmicro.Renode.Peripherals;
using Antmicro.Renode.Peripherals.Miscellaneous;


using Antmicro.Migrant;
namespace Antmicro.Renode.Backends.Terminals
{
    public static class ServerSocketLogicAnalyzerExtensions
    {
        public static void CreateServerSocketLogicAnalyzer(this Emulation emulation, int port, string name, bool emitConfig = true, bool flushOnConnect = false)
        {
            emulation.ExternalsManager.AddExternal(new ServerSocketLogicAnalyzer(port, emitConfig, flushOnConnect), name);
        }
    }

    [Transient]
    public class ServerSocketLogicAnalyzer : BackendTerminal, IDisposable
    {
        public ServerSocketLogicAnalyzer(int port, bool emitConfigBytes = true, bool flushOnConnect = false)
        {
            server = new SocketServerProvider(emitConfigBytes, flushOnConnect);
            server.DataReceived += b => Rx_From_Client((byte)b);

			this.Log(LogLevel.Info, "ServerSocketLogicAnalyzer started on Port {0}",port);
            server.Start(port);
        }


        public virtual void AttachTo(Logic_Probe Probe)
        {
        
           var StateChangedMethod = (Action<ILed, bool>)
            ((led, currState) =>
            {
        	string Socket_Str =  string.Format("{0:HH:mm:ss.ffff} {1} {2}\n", CustomDateTime.Now,led.GetName(), currState);
        	this.SendString(Socket_Str);
            });
            
        
        	this.Log(LogLevel.Info, "AttachTo {0} ",Probe.GetName());
        	Probe.StateChanged += StateChangedMethod;

        }


        public void Rx_From_Client(byte value)
        {
        this.Log(LogLevel.Info, "Rx_From_Client:  {0}",value);
        //this.WriteChar(value);  //echo test
        }

        public override void WriteChar(byte value)
        {
            server.SendByte(value);
            //this.Log(LogLevel.Info, "****Write  {0}",value);
        }


		protected static readonly Encoding StringEncoding = Encoding.UTF8;
        protected void SendBytes(byte[] bytes)
        {
           foreach(var b in bytes)
                {
                  WriteChar(b);
                }
        }

        protected void SendString(string str)
        {
            SendBytes(StringEncoding.GetBytes(str));
        }


        public void Dispose()
        {
            server.Stop();
        }

        private readonly SocketServerProvider server;
    }
}

