//-------------------------------------------------------------------------------------------------------------------
/*
 * Name: ErrorHandler.cs
 * Description: This class implements WCF IErrorHandler interface to catch and log any uncaught exception within WCF
 * workspace and to log it and return a uniform fault exception from it.
 * See ErrorBehaviourAttribute class to use this error handler.
 * Once used this class HandlerError is called first and then ProvideFault.
 * Created By: VB
 * Created Date (month, year): Dec 2011
 * Modified Date:
 * Modified Reason:
*/
//-------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using ServiceErrorHandler.Properties;
using Utilities;

namespace Services.ServiceErrorHandler
{
    
    public sealed class CommonErrorHandler : ErrorHandler
    { 
    }

    /// <summary>
    /// Description: This class implements WCF IErrorHandler interface to catch and log any uncaught exception within WCF
    /// workspace and to log it and return a uniform fault exception from it.
    /// See ErrorBehaviourAttribute class to use this error handler.
    /// Once used this class HandlerError is called first and then ProvideFault.
    /// </summary>
    public abstract class ErrorHandler : IErrorHandler
    {
        public virtual bool HandleError(Exception error)
        {
            //log the error only if its of our FaultException and it is unhandled, rest other errors are logged from ProvideFault
            if (error is FaultException<ServiceException>)
            {
                ServiceException se = (error as FaultException<ServiceException>).Detail;
                if (se != null && se.IsUnhandledException)
                    Trace.Write(TraceType.Error, se.ToString());
            }
            return true;
        }

        public virtual void ProvideFault(Exception error, System.ServiceModel.Channels.MessageVersion version, ref System.ServiceModel.Channels.Message fault)
        {
            if (!(error is FaultException<ServiceException>))
            {
                FaultException<ServiceException> newFault = ServiceException.HandleException<object>(error, null);
                //logging this error message here and not in handleerror because need have additional id
                if (newFault != null && newFault.Detail != null)
                {
                    Trace.Write(TraceType.Error, newFault.Detail.ToString()); ;
                    //create fault message
                    fault = Message.CreateMessage(version, newFault.CreateMessageFault(), newFault.Action);
                }
            }
        }

    }
}
