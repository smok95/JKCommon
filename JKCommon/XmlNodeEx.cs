using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JKCommon
{
    /// <summary>
    /// XmlNode 기능확장 클래스
    /// </summary>
    public static class XmlNodeEx
    {
        /// <summary>
        /// 속성값 확인
        /// </summary>
        /// <param name="src"></param>
        /// <param name="name">속성명</param>
        /// <returns>해당 속성값</returns>
        public static string GetAttrValue(this XmlNode src, string name)
        {
            XmlAttribute attr = src.Attributes[name];
            if (attr == null)
                return string.Empty;
            return attr.Value;
        }

        /// <summary>
        /// 신규 속성 추가
        /// </summary>
        /// <param name="src"></param>
        /// <param name="name">신규 속성명</param>
        /// <param name="value">속성값</param>
        /// <returns>추가된 속성</returns>
        public static XmlAttribute AppendAttribute(this XmlNode src, string name, string value)
        {
            XmlAttribute attr = src.OwnerDocument.CreateAttribute(name);
            attr.Value = value;
            return src.Attributes.Append(attr);
        }

        /// <summary>
        /// 자식노드의 값 확인
        /// </summary>
        /// <param name="src"></param>
        /// <param name="name">요소이름</param>
        /// <returns></returns>
        public static string GetChildText(this XmlNode src, string name)
        {
            XmlElement child = src[name];
            if(child == null)
                return string.Empty;
            return child.InnerText;
        }

        /// <summary>
        /// 자식노드 추가
        /// </summary>
        /// <param name="src"></param>
        /// <param name="name">노드명</param>
        /// <param name="value">노드 값</param>
        /// <returns></returns>
        public static XmlNode AppendChild(this XmlNode src, string name, string value)
        {
            XmlNode child = src.OwnerDocument.CreateElement(name);
            if (value != null)
                child.InnerText = value;
            return src.AppendChild(child);
        }
    }
}
