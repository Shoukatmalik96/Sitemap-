using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Sitemapnews.Helpers
{
    public class RssFeedHelper
    {
		public class RSSFeed
		{
			/// <summary>
			///A custom string writer which outputs in UTF8 rather than UTF16
			///</summary>
			///<remarks></remarks>
			internal class UTF8StringWriter : StringWriter
			{
				public override Encoding Encoding
				{
					get
					{
						return System.Text.Encoding.UTF8;
					}
				}
				public UTF8StringWriter(StringBuilder stringBuilder) : base(stringBuilder)
				{
				}
				public UTF8StringWriter(StringBuilder stringBuilder, System.IFormatProvider format) : base(stringBuilder, format)
				{
				}
			}

			private XmlDocument _document;
			/// <summary>
			///Returns the XML document
			///</summary>
			///<value></value>
			///<returns></returns>
			///<remarks></remarks>
			private XmlDocument Document
			{
				get
				{
					return _document;
				}
			}

			/// <summary>
			///Returns a UTF8 string representation of the XML document
			///</summary>
			///<value></value>
			///<returns></returns>
			///<remarks></remarks>
			public new string ToString
			{
				get
				{
					var b = new StringBuilder();
					using (var w = new UTF8StringWriter(b))
					{
						_document.Save(w);
					}
					return b.ToString();
				}
			}

			/// <summary>
			///Initialise the XML document and create the opening rss tags
			///</summary>
			///<remarks></remarks>
			public RSSFeed()
			{
				_document = new XmlDocument();
				Document.AppendChild(Document.CreateNode(XmlNodeType.XmlDeclaration, null, null));

				// Create the root element and add any name spaces
				var rootelement = Document.CreateElement("rss");
				rootelement.SetAttribute("version", "2.0");
				Document.AppendChild(rootelement);

				// Add the name spaces we use
				AddNamespace("media", "http://search.yahoo.com/mrss/");
				AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");
				//AddNamespace("atom", "http://www.w3.org/2005/Atom");

			}

			/// <summary>
			///Adds a prefix and url as a namespace to the rss root element
			///</summary>
			///<param name="prefix">The prefix for the namespace</param>
			///<param name="url">The reference url</param>
			///<remarks></remarks>
			public void AddNamespace(string prefix, string url)
			{
				Document.DocumentElement.SetAttribute("xmlns:" + prefix, url);
			}

			/// <summary>
			///Creates the channel in the RSS feed
			///</summary>
			///<param name="title">The title</param>
			///<param name="link">The link to the full website</param>
			///<param name="description">A brief description</param>
			///<param name="dateLastChanged">The date/time the channel was last updated</param>
			///<param name="language">The language of the channel</param>
			///<remarks></remarks>
			public void CreateChannel(string lastBuildDate, string title, string description, string link)
			{
				// First check we haven't already created a chnanel, as there should only be one in the feed
				var channels = Document.GetElementsByTagName("channel");

				if (channels.Count > 0)
					throw new ArgumentException("Channel " + channels[0].Name + " has already been created");

				var mainchannel = channels[0];

				// Create the channel element
				mainchannel = Document.CreateElement("channel");
				mainchannel.AppendChild(CreateTextElement("lastBuildDate", lastBuildDate));
				mainchannel.AppendChild(CreateTextElement("title", title));
				mainchannel.AppendChild(CreateTextElement("description", description));
				mainchannel.AppendChild(CreateTextElement("link", link));

				Document.DocumentElement.AppendChild(mainchannel);
			}
			public void CreateChannel(string lastBuildDate, string title, string description, string link, string atomLink)
			{
				// First check we haven't already created a chnanel, as there should only be one in the feed
				var channels = Document.GetElementsByTagName("channel");

				if (channels.Count > 0)
					throw new ArgumentException("Channel " + channels[0].Name + " has already been created");

				var mainchannel = channels[0];

				// Create the channel element
				mainchannel = Document.CreateElement("channel");
				mainchannel.AppendChild(CreateTextElement("lastBuildDate", lastBuildDate));
				mainchannel.AppendChild(CreateTextElement("title", title));
				mainchannel.AppendChild(CreateTextElement("description", description));
				mainchannel.AppendChild(CreateTextElement("link", link));
				var atom = CreateTextElement("link", "atom", "http://www.w3.org/2005/Atom");
				atom.SetAttribute("href", atomLink);
				atom.SetAttribute("rel", "self");
				atom.SetAttribute("type", "application/rss+xml");
				mainchannel.AppendChild(atom);

				Document.DocumentElement.AppendChild(mainchannel);
			}
			/// <summary>
			///Writes an item to the channel
			///</summary>
			///<param name="Title">The title of the item</param>
			///<param name="Link">A link to the full post</param>
			///<param name="Description">A description of the items</param>
			///<remarks></remarks>
			public void WriteRSSItem(string Title, string Link, string Description)
			{
				// First check we haven't already created a chnanel, as there should only be one in the feed
				var channels = Document.GetElementsByTagName("channel");
				if (channels.Count == 0)
					throw new ArgumentException("Please create a channel first by calling CreateChannel");

				var mainchannel = channels[0];

				// Create an item
				var thisitem = Document.CreateElement("item");

				thisitem.AppendChild(CreateTextElement("title", Title));
				thisitem.AppendChild(CreateTextElement("description", Description));
				thisitem.AppendChild(CreateTextElement("link", Link));


				// Append the element
				mainchannel.AppendChild(thisitem);
			}
			public void WriteRSSItem(string Guid, string PubDate, string Title, string Description, string Link)
			{
				// First check we haven't already created a chnanel, as there should only be one in the feed
				var channels = Document.GetElementsByTagName("channel");
				if (channels.Count == 0)
					throw new ArgumentException("Please create a channel first by calling CreateChannel");

				var mainchannel = channels[0];

				// Create an item
				var thisitem = Document.CreateElement("item");

				// Creating Guid 
				var gd = CreateTextElement("guid", Guid);
				gd.SetAttribute("isPermaLink", "false");
				thisitem.AppendChild(gd);

				thisitem.AppendChild(CreateTextElement("pubDate", PubDate));
				thisitem.AppendChild(CreateTextElement("title", Title));
				thisitem.AppendChild(CreateTextElement("description", Description));
				thisitem.AppendChild(CreateTextElement("link", Link));


				// Append the element
				mainchannel.AppendChild(thisitem);
			}
			/// <summary>
			///Creates a simple text element with the given name and inner text value
			///</summary>
			///<param name="Name">The name of the element</param>
			///<param name="Value">The value of it</param>
			///<returns></returns>
			///<remarks></remarks>
			private XmlElement CreateTextElement(string Name, string Value)
			{
				XmlElement CreateTextElement = Document.CreateElement(Name);
				CreateTextElement.InnerText = Value;

				return CreateTextElement;
			}
			private XmlElement CreateTextElement(string Name, string nameSpaceURi, string Value)
			{
				XmlElement CreateTextElement = Document.CreateElement(nameSpaceURi, Name, Value);


				return CreateTextElement;
			}
		}


		public class Sitemapper
		{
			/// <summary>
			///A custom string writer which outputs in UTF8 rather than UTF16
			///</summary>
			///<remarks></remarks>
			internal class UTF8StringWriter : StringWriter
			{
				public override Encoding Encoding
				{
					get
					{
						return System.Text.Encoding.UTF8;
					}
				}
				public UTF8StringWriter(StringBuilder stringBuilder) : base(stringBuilder)
				{
				}
				public UTF8StringWriter(StringBuilder stringBuilder, System.IFormatProvider format) : base(stringBuilder, format)
				{
				}
			}

			private XmlDocument _document;
			/// <summary>
			///Returns the XML document
			///</summary>
			///<value></value>
			///<returns></returns>
			///<remarks></remarks>
			private XmlDocument Document
			{
				get
				{
					return _document;
				}
			}

			/// <summary>
			///Returns a UTF8 string representation of the XML document
			///</summary>
			///<value></value>
			///<returns></returns>
			///<remarks></remarks>
			public new string ToString
			{
				get
				{
					var b = new StringBuilder();
					using (var w = new UTF8StringWriter(b))
					{
						_document.Save(w);
					}
					return b.ToString();
				}
			}

			/// <summary>
			///Initialise the XML document and create the opening rss tags
			///</summary>
			///<remarks></remarks>
			public Sitemapper()
			{
				_document = new XmlDocument();
				Document.AppendChild(Document.CreateNode(XmlNodeType.XmlDeclaration, null, null));

				// Create the root element and add any name spaces
				var rootelement = Document.CreateElement("urlset");
				//rootelement.SetAttribute("version", "1.0");
				Document.AppendChild(rootelement);

				// Add the name spaces we use
				AddNamespace("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
				AddNamespace("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
				AddNamespace("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd");
			}

			/// <summary>
			///Adds a prefix and url as a namespace to the rss root element
			///</summary>
			///<param name="prefix">The prefix for the namespace</param>
			///<param name="url">The reference url</param>
			///<remarks></remarks>
			public void AddNamespace(string prefix, string url)
			{
				Document.DocumentElement.SetAttribute(prefix, url);
			}

			/// <summary>
			///Writes an item to the channel
			///</summary>
			///<param name="location">The title of the item</param>
			///<param name="changeFrequency">A link to the full post</param>
			///<param name="lastModified">A description of the items</param>
			///<remarks></remarks>
			///
			public void WriteItem(string location, string changeFrequency, string lastModified = "")
			{
				// First check we haven't already created a chnanel, as there should only be one in the feed
				var urlSets = Document.GetElementsByTagName("urlset");

				var mainUrlSet = urlSets[0];

				// Create an item
				var thisitem = Document.CreateElement("url");

				thisitem.AppendChild(CreateTextElement("loc", location));
				thisitem.AppendChild(CreateTextElement("changefreq", changeFrequency));

				if (!string.IsNullOrEmpty(lastModified))
				{
					thisitem.AppendChild(CreateTextElement("lastmod", lastModified));
				}

				// Append the element
				mainUrlSet.AppendChild(thisitem);
			}

			/// <summary>
			///Creates a simple text element with the given name and inner text value
			///</summary>
			///<param name="Name">The name of the element</param>
			///<param name="Value">The value of it</param>
			///<returns></returns>
			///<remarks></remarks>
			private XmlElement CreateTextElement(string Name, string Value)
			{
				XmlElement CreateTextElement = Document.CreateElement(Name);
				CreateTextElement.InnerText = Value;

				return CreateTextElement;
			}
		}
	}
}