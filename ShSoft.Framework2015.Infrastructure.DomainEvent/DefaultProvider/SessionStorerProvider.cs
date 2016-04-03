﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using ShSoft.Framework2015.Infrastructure.IDomainEvent;

namespace ShSoft.Framework2015.Infrastructure.DomainEvent.DefaultProvider
{
    /// <summary>
    /// 领域事件存储者Session提供者
    /// </summary>
    public class SessionStorerProvider : IDomainEventStorer
    {
        #region # 常量

        /// <summary>
        /// 领域事件存储Session键
        /// </summary>
        private const string EventSessionKey = "DomainEvent";

        /// <summary>
        /// 领域事件源集合
        /// </summary>
        private IList<IDomainEvent.IDomainEvent> _eventSources;

        /// <summary>
        /// 静态构造器
        /// </summary>
        public SessionStorerProvider()
        {
            this._eventSources = new List<IDomainEvent.IDomainEvent>();
        }

        #endregion

        #region # 初始化存储 —— void InitStore()
        /// <summary>
        /// 初始化存储
        /// </summary>
        public void InitStore() { }
        #endregion

        #region # 挂起领域事件 —— void Suspend<T>(T domainSource)
        /// <summary>
        /// 挂起领域事件
        /// </summary>
        /// <typeparam name="T">领域事件源类型</typeparam>
        /// <param name="domainSource">领域事件源</param>
        public void Suspend<T>(T domainSource) where T : class, IDomainEvent.IDomainEvent
        {
            //获取线程缓存
            object eventSources = CallContext.GetData(EventSessionKey);

            //如果缓存不为空，则将事件源队列变量赋值为缓存
            if (eventSources != null)
            {
                this._eventSources = (IList<IDomainEvent.IDomainEvent>)eventSources;
            }

            //将新事件源添加到队列
            this._eventSources.Add(domainSource);

            //将新队列添加到缓存
            CallContext.SetData(EventSessionKey, this._eventSources);
        }
        #endregion

        #region # 处理未处理的领域事件 —— void HandleUncompletedEvents()
        /// <summary>
        /// 处理未处理的领域事件
        /// </summary>
        public void HandleUncompletedEvents()
        {
            //获取线程缓存
            object eventSources = CallContext.GetData(EventSessionKey);

            //如果缓存中没有数据，则终止方法
            if (eventSources == null)
            {
                return;
            }

            //如果缓存不为空，则将事件源队列变量赋值为缓存
            this._eventSources = (IList<IDomainEvent.IDomainEvent>)eventSources;

            //如果有未处理的
            if (this._eventSources.Any(x => !x.Handled))
            {
                foreach (IDomainEvent.IDomainEvent eventSource in this._eventSources.Where(x => !x.Handled))
                {
                    eventSource.Handle();
                }
            }

            //递归
            if (this._eventSources.Any(x => !x.Handled))
            {
                this.HandleUncompletedEvents();
            }

            //处理完毕后置空缓存
            CallContext.FreeNamedDataSlot(EventSessionKey);
        }
        #endregion

        #region # 清空未处理的领域事件 —— void ClearUncompletedEvents()
        /// <summary>
        /// 清空未处理的领域事件
        /// </summary>
        public void ClearUncompletedEvents()
        {
            //置空缓存
            CallContext.FreeNamedDataSlot(EventSessionKey);
        }
        #endregion

        #region # 释放资源 —— void Dispose()
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose() { }
        #endregion
    }
}