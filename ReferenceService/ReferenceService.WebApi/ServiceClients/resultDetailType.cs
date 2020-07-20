    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://server.ru/service/result")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://server.ru/service/result", IsNullable = false)]
    public class resultDetail
    {
        private processNotices processNoticesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public processNotices processNotices
        {
            get
            {
                return this.processNoticesField;
            }
            set
            {
                this.processNoticesField = value;
            }
        }
    }

    /// <remarks/>
    public class processNotices
    {
        private object attributeField;

        private string descriptionField;

        private string severityField;

        /// <remarks/>
       [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public object attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string severity
        {
            get
            {
                return this.severityField;
            }
            set
            {
                this.severityField = value;
            }
        }
    }
