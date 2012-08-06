using System;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;

namespace Bebox.Toolkit
{
	public class ConfigurationHandler<T> : IConfigurationSectionHandler
	{
		public object Create(object parent, object configContext, XmlNode section)
		{
			var environmentNode = section.SelectSingleNode(typeof(T).Name);
			var xmlSerializer = new XmlSerializer(typeof(T));
			var xmlNodeReader = new XmlNodeReader(environmentNode);
			return xmlSerializer.Deserialize(xmlNodeReader);
		}
	}
}