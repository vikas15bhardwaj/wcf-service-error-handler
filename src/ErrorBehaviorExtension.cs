//-------------------------------------------------------------------------------------------------------------------
/*
 * Name: ErrorBehaviorExtension.cs
 * Description: This class inherits from BehaviorExtensionElement to extend service behaior via configuration file.
 * This means you are not required to use ErrorBehavior attribute on your service implementation class. You can do
 * all in configuration file. e.g.
 * This class will load ErrorBehaviorAttribute with required type of error handler at run time based on your configuration file
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
using System.ServiceModel.Configuration;
using System.ServiceModel.Dispatcher;
using System.Configuration;
using System.Reflection;

namespace Services.ServiceErrorHandler
{
     /// <summary>
     /// This class inherits from BehaviorExtensionElement to extend service behaior via configuration file.
     /// This means you are not required to use ErrorBehavior attribute on your service implementation class. You can do
     /// all in configuration file. e.g.
     /// 
     /// <system.serviceModel>
     ///   <behaviors>
     ///     <serviceBehaviors>
     ///       <behavior>
     ///         <errorHandlerExtension errorHandlerType="Services.ServiceErrorHandler.CommonErrorHandler, ServiceErrorHandler"/>
     ///       </behavior>
     ///     </serviceBehaviors>
     ///   </behaviors>
     ///   <extensions>
     ///     <behaviorExtensions>
     ///       <add name="errorHandlerExtension" type="Services.ServiceErrorHandler.ErrorBehaviorExtension, ServiceErrorHandler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=cf72db1ac378d881"/>
     ///     </behaviorExtensions>
     ///    </extensions>    
     /// </system.serviceModel>
     ///
     /// This class will load ErrorBehaviorAttribute with required type of error handler at run time based on your configuration file
     /// </summary>
    public class ErrorBehaviorExtension : BehaviorExtensionElement
    {
        [ConfigurationProperty("errorHandlerType", IsRequired = true)]
        public string ErrorHandler
        {
            get
            {
                return (string)base["errorHandlerType"];
            }
            set
            {
                base["errorHandlerType"] = value;
            }
        }

        public override Type BehaviorType
        {
            get { return typeof(ErrorBehaviorAttribute); }
        }

        protected override object CreateBehavior()
        {
            string[] typeDetails = ErrorHandler.Split(',');


            if (typeDetails != null && typeDetails.Length > 0)
            {
                string typeName = typeDetails[0];
                string assembly = string.Empty;
                for (int i = 1; i < typeDetails.Length; i++)
                    assembly += typeDetails[i].Trim() + ", ";

                assembly = assembly.TrimEnd(new char[] { ' ', ',' });


                Type type = Assembly.Load(assembly).GetType(typeName);
                return new ErrorBehaviorAttribute(type);
            }
            else return null;
            
        }
    }
}
