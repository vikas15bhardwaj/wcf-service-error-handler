//-------------------------------------------------------------------------------------------------------------------
/*
 * Name: ErrorBehaviorAttribute.cs
 * Description: This class implements IServiceBehaviour interface and inherits from Attribute class. This class makes
 * it easy to use ErrorHandler in service implementation declartively. e.g.
 * 
 * [ErrorBehavior(typeof(Services.ServiceErrorHandler.CommonErrorHandler))]
 * [ServiceBehavior(Namespace = "http://services/ebankservice/1")]
 * public class EBankService : IEBank
 * 
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
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Services.ServiceErrorHandler
{
    /// <summary>
    ///  This class implements IServiceBehaviour interface and inherits from Attribute class. This class makes
    ///  it easy to use ErrorHandler in service implementation declartively. e.g.
    ///  [ErrorBehavior(typeof(Services.ServiceErrorHandler.CommonErrorHandler))]
    ///  [ServiceBehavior(Namespace = "http://services/ebankservice/1")]
    ///  public class EBankService : IEBank
    /// </summary>
    public class ErrorBehaviorAttribute : Attribute, IServiceBehavior
    {
        Type errorHandlerType;

        public ErrorBehaviorAttribute(Type errorHandlerType)
        {
            this.errorHandlerType = errorHandlerType;
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            IErrorHandler errorHandler;

            try
            {
                errorHandler = (IErrorHandler)Activator.CreateInstance(errorHandlerType);
            }
            catch (MissingMethodException e)
            {
                throw new ArgumentException("The errorHandlerType specified in the ErrorBehaviorAttribute constructor must have a public empty constructor.", e);
            }
            catch (InvalidCastException e)
            {
                throw new ArgumentException("The errorHandlerType specified in the ErrorBehaviorAttribute constructor must implement System.ServiceModel.Dispatcher.IErrorHandler.", e);
            }

            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                channelDispatcher.ErrorHandlers.Add(errorHandler);
            }   

        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            
        }
    }
}
