using System;
using System.Collections.Generic;
using System.Text;

namespace DamirM.CommonLibrary
{
    public struct NameValueDataStruct
    {
        private string name;
        private object value;
        private object data;

        public NameValueDataStruct(string name, object value)
        {
            this.name = name;
            this.value = value;
            this.data = null;
        }
        public NameValueDataStruct(string name, object value, object data)
        {
            this.name = name;
            this.value = value;
            this.data = data;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public object Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }
        public override string ToString()
        {
            return name;
        }
    }
}
