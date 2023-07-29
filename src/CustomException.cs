using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Utilities
{
    [DataContract(Name = "Error", Namespace = "http://services/servicefault/contract/1.0")]
    public sealed class Error
    {
        [DataMember(Name = "code", IsRequired = true)]
        public string Code { get; set; }
        [DataMember(Name = "message", IsRequired = true)]
        public string Message { get; set; }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append("Code: ");
            s.AppendLine(Code);
            s.Append("Message: ");
            s.AppendLine(Message);

            return s.ToString();

        }
    }

    [CollectionDataContract(Name = "ErrorList", ItemName = "error", Namespace = "http://services/servicefault/contract/1.0")]
    public sealed class ErrorList : List<Error>
    {

    }

    public class CustomException : Exception, IList<Error>
    {
        private ErrorList _errorList = new ErrorList();

        public virtual ErrorList ErrorList
        {
            get
            {
                return _errorList;
            }
            set
            {
                _errorList = value;
            }
        }

        public CustomException()
            : base()
        {
            
        }
        public CustomException(string code, string message)
            : base(message)
        {
            _errorList.Add(new Error { Code = code, Message = message });
        }

        public CustomException(ErrorList errorList)
            : base()
        {
            _errorList = errorList;
        }

        #region IList members...
        public int IndexOf(Error item)
        {
            return _errorList.IndexOf(item);
        }

        public void Insert(int index, Error item)
        {
            _errorList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _errorList.RemoveAt(index);
        }

        public Error this[int index]
        {
            get
            {
                return _errorList[index];
            }
            set
            {
                _errorList[index] = value;
            }
        }

        public void Add(Error item)
        {
            _errorList.Add(item);
        }

        public void Clear()
        {
            _errorList.Clear();
        }

        public bool Contains(Error item)
        {
            return _errorList.Contains(item);
        }

        public void CopyTo(Error[] array, int arrayIndex)
        {
            _errorList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _errorList.Count(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Error item)
        {
            return _errorList.Remove(item);
        }

        public IEnumerator<Error> GetEnumerator()
        {
            return _errorList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _errorList.GetEnumerator();
        }

        #endregion
    }



}
