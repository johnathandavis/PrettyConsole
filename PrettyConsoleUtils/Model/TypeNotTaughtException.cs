using System;
using System.Runtime.Serialization;

namespace PrettyConsoleUtils.Model
{
    [Serializable]
    public class TypeNotTaughtException : Exception
    {
        public TypeNotTaughtException() { }
        public TypeNotTaughtException(Type duplicateType)
        {
            if (duplicateType == null) throw new ArgumentNullException("The provided type was null.");

            ExistingPrintMethodType = duplicateType;
            printMethodTypeName = duplicateType.AssemblyQualifiedName;
        }
        public TypeNotTaughtException(string message) : base(message) { }
        public TypeNotTaughtException(string message, Exception inner) : base(message, inner) { }
        protected TypeNotTaughtException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

            if (info == null) return;

            printMethodTypeName = info.GetString("printMethodTypeName");

            try
            {
                ExistingPrintMethodType = Type.GetType(printMethodTypeName);
            }
            catch { }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info == null) return;

            info.AddValue("printMethodTypeName", printMethodTypeName);
        }

        private string printMethodTypeName { get; set; }
        public Type ExistingPrintMethodType { get; private set; }
    }
}
