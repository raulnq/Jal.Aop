using System;
using System.Transactions;
using Castle.DynamicProxy;
using Cignium.Framework.Infrastructure.Aop.Aspects.Interface;
using Common.Logging;

namespace Cignium.Framework.Infrastructure.Aop.Aspects.Transaction
{
    public class TransactionAspect : AbstractAspect, IInterceptor
    {
        protected ILog Log;

        protected IRollbackEvaluator RollbackEvaluator;

        public IsolationLevel IsolationLevel { get; set; }

        public TransactionAspect()
        {
            IsolationLevel = IsolationLevel.ReadUncommitted;
        }

        public TransactionAspect(ILog logger, IRollbackEvaluator rollbackEvaluator)
        {
            Log = logger;
            RollbackEvaluator = rollbackEvaluator;
        }

        public void Intercept(IInvocation invocation)
        {
            if (IsMarked(invocation))
            {
                try
                {
                    var attribute = CurrentAttribute(invocation) as TransactionAspect;
                    var transactionOptions = new TransactionOptions
                    {
                        IsolationLevel = attribute.IsolationLevel,
                        Timeout = TransactionManager.DefaultTimeout
                    };
                    using (var scope = new TransactionScope(TransactionScopeOption.Required, transactionOptions))
                    {

                        var tx = System.Transactions.Transaction.Current;
                        Log.Info(string.Format("[{0}.cs, {1}] Open Transaction. CreationTime:{2},LocalIdentifier:{3},DistributedIdentifier:{4},IsolationLevel:{5},Status:{6} ",invocation.TargetType.Name, invocation.Method.Name, tx.TransactionInformation.CreationTime, tx.TransactionInformation.LocalIdentifier, tx.TransactionInformation.DistributedIdentifier, tx.IsolationLevel, tx.TransactionInformation.Status));
                        invocation.Proceed();
                        if (RollbackEvaluator.Evaluate(invocation.ReturnValue))
                        {
                            Log.Info(string.Format("[{0}.cs, {1}] Rollback Transaction", invocation.TargetType.Name, invocation.Method.Name));
                        }
                        else
                        {
                            scope.Complete();
                            Log.Info(string.Format("[{0}.cs, {1}] Commit Transaction. CreationTime:{2},LocalIdentifier:{3},DistributedIdentifier:{4},IsolationLevel:{5},Status:{6} ",
                            invocation.TargetType.Name, invocation.Method.Name, tx.TransactionInformation.CreationTime, tx.TransactionInformation.LocalIdentifier, tx.TransactionInformation.DistributedIdentifier, tx.IsolationLevel, tx.TransactionInformation.Status));
                        }
                    }
                }
                catch (Exception)
                {
                    Log.Info(string.Format("[{0}.cs, {1}] Rollback Transaction", invocation.TargetType.Name, invocation.Method.Name));
                    throw;
                }
                finally
                {
                    Log.Info(string.Format("[{0}.cs, {1}] Close Transaction", invocation.TargetType.Name, invocation.Method.Name));
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
