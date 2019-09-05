using System;

namespace Aop.demo.AspnetCore.CrossCuttings
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// 提交
        /// </summary>
        void Commit();

        /// <summary>
        /// 事物回滚
        /// </summary>
        void RollBack();
    }

    public class UnitOfWork : IUnitOfWork
    {
        public void BeginTransaction()
        {
            Console.WriteLine("开启事务");
        }

        public void Commit()
        {
            Console.WriteLine("提交事务");
        }

        public void Dispose()
        {
             
        }

        public void RollBack()
        {
            Console.WriteLine("回滚事务");
        }
    }
}
