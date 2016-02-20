﻿using System;
using PostSharp.Aspects;
using ShSoft.Framework2015.AOP.Aspects.ForAny;
using ShSoft.Framework2015.Infrastructure.CustomExceptions;

namespace ShSoft.Framework2015.Infrastructure.AOP.Aspects.ForAny
{
    /// <summary>
    /// Business层异常AOP特性类
    /// </summary>
    [Serializable]
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class BusinessExceptionAspect : ExceptionAspect
    {
        /// <summary>
        /// 异常过滤器
        /// </summary>
        /// <param name="eventArgs">方法元数据</param>
        public override void OnException(MethodExecutionArgs eventArgs)
        {
            //抛出异常
            throw new BusinessException(eventArgs.Exception.Message, eventArgs.Exception);
        }
    }
}