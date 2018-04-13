using SD.Infrastructure.Global;
using SD.IOC.Integration.WebApi;
using System;
using System.Web;

namespace SD.Infrastructure.WebApi.Server
{
    /// <summary>
    /// ��ʼ��HttpModule
    /// </summary>
    internal class InitializationHttpModule : IHttpModule
    {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">Ӧ�ó���������</param>
        public void Init(HttpApplication context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            //��ʼ��SessionId
            Initializer.InitSessionId();

            //��ʼ�����ݿ�
            Initializer.InitDataBase();

            //ע���¼�
            WebApiDependencyResolver.OnGetInstance += this.WebApiDependencyResolver_OnGetInstance;
            WebApiDependencyResolver.OnReleaseInstance += this.WebApiDependencyResolver_OnReleaseInstance;
        }

        /// <summary>
        /// ��ȡ����ʵ���¼�
        /// </summary>
        private void WebApiDependencyResolver_OnGetInstance()
        {
            //��ʼ��SessionId
            Initializer.InitSessionId();
        }

        /// <summary>
        /// ���ٷ���ʵ���¼�
        /// </summary>
        private void WebApiDependencyResolver_OnReleaseInstance()
        {
            //�������ݿ�
            Finalizer.CleanDb();
        }

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose() { }
    }
}