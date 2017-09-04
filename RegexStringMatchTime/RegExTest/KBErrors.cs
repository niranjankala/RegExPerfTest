/*****************************************************************************
 * 
 * CONFIDENTIAL TO DIGITAL ALCHEMY, INC.
 * __________________
 * 
 *  Copyright [2009] - [2017] Digital Alchemy, Incorporated 
 *  All Rights Reserved.
 * 
 * NOTICE:  All information contained herein is, and remains the property of 
 * Digital Alchemy, Incorporated and its suppliers, if any.  The intellectual 
 * and technical concepts contained herein are proprietary to Digital Alchemy, 
 * Incorporated and its suppliers and may be covered by U.S. and Foreign 
 * Patents, patents in process, and are protected by trade secret or 
 * copyright law.
 * Dissemination of this information, or reproduction of this material
 * is strictly forbidden unless prior written permission is obtained
 * from Digital Alchemy Incorporated.
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegExTest
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class KBErrors
    {
        private List<KBError> KBErrorsField;
        /// <remarks/>
        [System.Xml.Serialization.XmlElement("KBError", IsNullable = false)]
        public List<KBError> KBErrorsList
        {
            get
            {
                return this.KBErrorsField;
            }
            set
            {
                this.KBErrorsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class KBError
    {
        private string sourceType;
        private string errorType;
        private string errorMessage;
        private string description;
        private string issueResolution;
        private string errorCode;
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string SourceType
        {
            get
            {
                return this.sourceType;
            }
            set
            {
                this.sourceType = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ErrorType
        {
            get
            {
                return this.errorType;
            }
            set
            {
                this.errorType = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
            set
            {
                this.errorMessage = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IssueResolution
        {
            get
            {
                return issueResolution;
            }
            set
            {
                issueResolution = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ErrorCode
        {
            get
            {
                return errorCode;
            }
            set
            {
                errorCode = value;
            }
        }
    }
}
