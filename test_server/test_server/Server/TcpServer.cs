using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using ShareUtil;
namespace test_server
{
    class TcpServer
    {
        protected TcpListener tcpListener;
        protected Thread m_thread;
        protected bool stillListen;
        protected AsyncCallback acceptCallBack;
        protected AutoResetEvent acceptEvent;
        public TcpServer(){
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            tcpListener = new TcpListener(ip,8080);
            acceptCallBack = new AsyncCallback(AcceptCallBack);
            acceptEvent = new AutoResetEvent(true);
        }
        #region public接口
        public void Start() {
            stillListen = true;
            if (m_thread == null) {
                m_thread = new Thread(MainProc);
                m_thread.Start();
            }
        }
        public void Stop() {
            stillListen = false;
            if (m_thread != null) {
                m_thread.Join();
                m_thread = null;
                Log.Info("process is end");
            }
        }
        #endregion
        #region private接口
        protected void OnAccept(Socket socket) {
            Log.Info("i am receiving a socket and port num is "+ ((IPEndPoint)socket.RemoteEndPoint).Port.ToString());
        }
        protected void StartListen() {
            tcpListener.Start();
        }
        protected void Accept() {
            if (tcpListener == null) {
                return;
            }
            if (acceptEvent.WaitOne(200)) {
                tcpListener.BeginAcceptSocket(acceptCallBack, tcpListener);
            }
        }
        protected void AcceptCallBack(IAsyncResult result) {
            Socket socket = tcpListener.EndAcceptSocket(result);
            OnAccept(socket);
            acceptEvent.Set();
        }
        protected void MainProc()
        {
            Log.Info("process is on");
            while (stillListen)
            {
                StartListen();
                Accept();
            }
        }
        #endregion
    }
}
