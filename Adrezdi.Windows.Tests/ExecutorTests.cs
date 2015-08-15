using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Adrezdi.Windows.Tests
{
    [TestClass]
    public class ExecutorTests
    {
        [TestMethod]
        public void ExecuteOnUiThreadSucceeds()
        {
            // arrange
            var e = new EventWaitHandle(false, EventResetMode.ManualReset);
            var thread = Thread.CurrentThread;
            bool succeeded = false;

            // act
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Executor.OnUIThread(() => succeeded = thread != Thread.CurrentThread);
                e.Set();
            });

            // assert
            e.WaitOne();
            Assert.IsTrue(succeeded);
        }
    }
}
