//-------------------------------------------------------------------------------------------------------------------
/*
 * Name: ServiceException.cs
 * Description: This file defines the class which is used to return FaultException for WCF service. All services must
 * use this class to return faults or any other error to its consumers. please note this class does not derive from
 * Exception class, so it can not be thrown directly. It has to be part of FaultException<T>
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
using Utilities;
using System.Runtime.Serialization;
using System.ServiceModel;
using ServiceErrorHandler.Properties;

namespace Services.ServiceErrorHandler
{
    public enum FaultCodeType
    { 
        Client,
        Server
    }

    /// <summary>
    ///  This class defines the class which is used to return FaultException for WCF service. All services must
    ///  use this class to return faults or any other error to its consumers. please note this class does not derive from
    ///  Exception class, so it can not be thrown directly. It has to be part of FaultException
    /// </summary>
    [DataContract(Name = "ServiceException", Namespace = "http:///services/servicefault/contract/1.0")]
    public class ServiceException
    {
        [DataMember(Name = "id", Order = 0, IsRequired = false)]
        public string ExceptionId { get; set; }

        [DataMember(Name = "errors", Order = 1, IsRequired = false)]
        public ErrorList ErrorList { get; set; }

        public string RequestMessage { get; set; }

        public bool IsUnhandledException { get; set; }

        public string UnhandledExceptionDetail { get; set; }

        public ServiceException()
        {
            ErrorList = new ErrorList();
        }

        public ServiceException(ErrorList errorList)
        {
            ErrorList = errorList;
        }

        public ServiceException(string code, string message)
        {
            ErrorList = new ErrorList();
            ErrorList.Add(new Error { Code = code, Message = message });
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append("ExceptionId: ");
            s.AppendLine(ExceptionId);
            
            s.Append("IsUnhandledException: ");
            s.AppendLine(IsUnhandledException.ToString());

            s.Append("UnhandledExceptionDetail: ");
            s.AppendLine(UnhandledExceptionDetail);

            s.Append("ErrorList: ");
            if (ErrorList != null && ErrorList.Count > 0)
            {
                foreach (Error e in ErrorList)
                {
                    s.AppendLine(e.ToString());
                }
            }

            s.Append("RequestMessage: ");
            s.AppendLine(RequestMessage);

            return s.ToString();
        }

        /// <summary>
        /// This function handles the exception, logs it if required, and returns the FaultException<typeparamref name="ServiceException"/>
        /// This function should always be used to get a faultexception from any exception
        /// </summary>
        /// <typeparam name="T">Type T is a class parameter, and is supposed to be your request object which can be used to log details to
        /// </typeparam>
        /// <param name="ex">Exception caught from which FaultException to be created</param>
        /// <param name="requestType">the request object of type T</param>
        /// <returns>returns FaultException object</returns>
        public static FaultException<ServiceException> HandleException<T>(Exception ex, T requestType) where T : class
        {
            ServiceException serviceException = new ServiceException();
            serviceException.RequestMessage = requestType == null ? String.Empty : requestType.ToString();
            serviceException.ExceptionId = Guid.NewGuid().ToString();

            FaultReason faultReason = null;
            FaultCode faultCode = null;

            if (ex is CustomException)
            {
                serviceException.ErrorList = (ex as CustomException).ErrorList;
                faultReason = new FaultReason(Resources.Client);
                faultCode = new FaultCode(FaultCodeType.Client.ToString());
            }
            else
            {
                serviceException.ErrorList.Add(new Error { Code = "UNKNOWN_EXCEPTION", Message = Resources.UNKNOWN_EXCEPTION});
                serviceException.IsUnhandledException = true;
                serviceException.UnhandledExceptionDetail = ex.ToString();
                faultReason = new FaultReason(Resources.Server);
                faultCode = new FaultCode(FaultCodeType.Server.ToString());
            }

            FaultException<ServiceException> fault =
                new FaultException<ServiceException>(serviceException, faultReason, faultCode);

            return fault;

        }

        public static FaultException<ServiceException> GetClientFault(string code, string message)
        {
            return new FaultException<ServiceException>(new ServiceException(code, message),
                                                        new FaultReason(Resources.Client),
                                                        new FaultCode(FaultCodeType.Client.ToString()));
        }

    }
}
