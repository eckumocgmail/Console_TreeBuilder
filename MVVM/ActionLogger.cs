using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfGrid.MVVM
{
    internal class ActionLogger
    {

        public event EventHandler<string> OnMessage = (sender, evt) => {
            Debug.WriteLine(evt);
        };

        internal void WriteSuccessAction(string actionName)
            => OnMessage.Invoke(this,actionName);
        internal void WriteErrorAction(string actionName)
            => OnMessage.Invoke(this, actionName);

        internal void WriteAsyncOperationCompleted(object result)
        {
            throw new NotImplementedException();
        }
    }
}
