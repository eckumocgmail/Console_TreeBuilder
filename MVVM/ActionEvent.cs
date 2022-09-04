using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfGrid.MVVM
{
    public class ActionEvent: EventArgs
    {
        public ActionEvent( string ActionName, bool ActionResult )
        {
            this.ActionName = ActionName;
            if( (this.ActionResult = MethodResult.FromResult(ActionResult)) == null)
            {
                this.ActionResult = new MethodResult();
            }

        }


        public ActionEvent(string ActionName, MethodResult ActionResult)
        {
            this.ActionName = ActionName;
            if ((this.ActionResult = ActionResult) == null)
            {
                this.ActionResult = new MethodResult();
            }

        }

        public string ActionName { get; }
        public MethodResult ActionResult { get; }


        public T Get<T>() => (T)ActionResult.Result;

    }
}
