using System;
using System.Collections.Generic;
#if !(NET35 || NET20 || WINDOWS_PHONE)
using System.Dynamic;
#endif
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using NUnit.Framework;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Tests.TestObjects;

namespace Newtonsoft.Json.Tests.Serialization
{

	public class JsonListSerializationTests : TestFixtureBase
	{
#if !(NET35 || NET20 || WINDOWS_PHONE)


		[Test]
		public void DeserializeExpandoObjectAsRoot()
		{
			dynamic library = new ExpandoObject();

			library.Name = "1Items";
			library.ArticleArray = new Article[] { new Article("Article1") };
			library.ArticleList = new List<Article>() { new Article("Article1") };
			library.ArticleIList = new List<Article>() { new Article("Article1") };
			library.ArticleCollection = new ArticleCollection() { new Article("Article1") };
			library.StringArray = new[] { "Article1" };

			Assert.AreEqual(1, library.ArticleArray.Length);
			Assert.AreEqual(1, library.ArticleCollection.Count);
			Assert.AreEqual(1, library.ArticleList.Count);
			Assert.AreEqual(1, library.ArticleList.Count);
			Assert.AreEqual(1, library.StringArray.Length);

			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto
			};

			string json = JsonConvert.SerializeObject(library, Formatting.Indented, settings);

			//TODO: Fix this
			//var document = JsonConvert.DeserializeXNode(json, "root", false);

			var result = JsonConvert.DeserializeObject<dynamic>(json, settings);

			Assert.AreEqual(result.Name, library.Name);
			Assert.AreEqual(1, result.ArticleArray.Length);
			Assert.AreEqual(1, result.ArticleCollection.Count);
			Assert.AreEqual(1, result.ArticleList.Count);
			Assert.AreEqual(1, result.ArticleList.Count);
			Assert.AreEqual(1, result.StringArray.Length);
		}

		[Test]
		public void PopulateExpandoObjectAsRoot()
		{
			dynamic library = new ExpandoObject();

			library.Name = "1Items";
			library.ArticleArray = new Article[] { new Article("Article1") };
			library.ArticleList = new List<Article>() { new Article("Article1") };
			library.ArticleIList = new List<Article>() { new Article("Article1") };
			library.ArticleCollection = new ArticleCollection() { new Article("Article1") };
			library.StringArray = new[] { "Article1" };

			Assert.AreEqual(1, library.ArticleArray.Length);
			Assert.AreEqual(1, library.ArticleCollection.Count);
			Assert.AreEqual(1, library.ArticleList.Count);
			Assert.AreEqual(1, library.ArticleList.Count);
			Assert.AreEqual(1, library.StringArray.Length);

			var settings = new JsonSerializerSettings
			{
				TypeNameHandling = TypeNameHandling.Auto
			};

			string json = JsonConvert.SerializeObject(library, Formatting.Indented, settings);

			var expando = new ExpandoObject();
			JsonConvert.PopulateObject(json, expando, settings);
			dynamic result = expando;
			Assert.AreEqual(result.Name, library.Name);
			Assert.AreEqual(1, result.ArticleArray.Length);
			Assert.AreEqual(1, result.ArticleCollection.Count);
			Assert.AreEqual(1, result.ArticleList.Count);
			Assert.AreEqual(1, result.ArticleList.Count);
			Assert.AreEqual(1, result.StringArray.Length);
		}

		public class MagicContainer
		{
			public string Name { get; set; }

			[JsonProperty(TypeNameHandling = TypeNameHandling.Auto)]
			private ExpandoObject Library2 { get; set; }

			[JsonIgnore]
			public dynamic Library
			{
				get { return Library2; }
				set { Library2 = value; }
			}
		}

		[Test]
		public void DeserializeExpandoObjectAsAProperty()
		{
			var container = new MagicContainer();
			container.Name = "Container1";
			container.Library = new ExpandoObject();

			container.Library.Name = "1Items";
			container.Library.ArticleArray = new Article[] { new Article("Article1") };
			container.Library.ArticleList = new List<Article>() { new Article("Article1") };
			container.Library.ArticleIList = new List<Article>() { new Article("Article1") };
			container.Library.ArticleCollection = new ArticleCollection() { new Article("Article1") };
			container.Library.StringArray = new[] { "Article1" };

			Assert.AreEqual(1, container.Library.ArticleArray.Length);
			Assert.AreEqual(1, container.Library.ArticleCollection.Count);
			Assert.AreEqual(1, container.Library.ArticleList.Count);
			Assert.AreEqual(1, container.Library.ArticleList.Count);
			Assert.AreEqual(1, container.Library.StringArray.Length);

			var settings = new JsonSerializerSettings { };

			string json = JsonConvert.SerializeObject(container, Formatting.Indented, settings);

			var result = JsonConvert.DeserializeObject<MagicContainer>(json, settings);

			Assert.AreEqual(result.Library.Name, container.Library.Name);
			Assert.AreEqual(1, result.Library.ArticleArray.Length);
			Assert.AreEqual(1, result.Library.ArticleCollection.Count);
			Assert.AreEqual(1, result.Library.ArticleList.Count);
			Assert.AreEqual(1, result.Library.ArticleList.Count);
			Assert.AreEqual(1, result.Library.StringArray.Length);
		}

		[Test]
		public void PopulateExpandoObjectAsAProperty()
		{
			var container = new MagicContainer();
			container.Name = "Container1";
			container.Library = new ExpandoObject();

			container.Library.Name = "1Items";
			container.Library.ArticleArray = new Article[] { new Article("Article1") };
			container.Library.ArticleList = new List<Article>() { new Article("Article1") };
			container.Library.ArticleIList = new List<Article>() { new Article("Article1") };
			container.Library.ArticleCollection = new ArticleCollection() { new Article("Article1") };
			container.Library.StringArray = new[] { "Article1" };

			Assert.AreEqual(1, container.Library.ArticleArray.Length);
			Assert.AreEqual(1, container.Library.ArticleCollection.Count);
			Assert.AreEqual(1, container.Library.ArticleList.Count);
			Assert.AreEqual(1, container.Library.ArticleList.Count);
			Assert.AreEqual(1, container.Library.StringArray.Length);

			var settings = new JsonSerializerSettings { };

			string json = JsonConvert.SerializeObject(container, Formatting.Indented, settings);

			MagicContainer result = new MagicContainer();
			JsonConvert.PopulateObject(json, result, settings);

			Assert.AreEqual(result.Library.Name, container.Library.Name);
			Assert.AreEqual(1, result.Library.ArticleArray.Length);
			Assert.AreEqual(1, result.Library.ArticleCollection.Count);
			Assert.AreEqual(1, result.Library.ArticleList.Count);
			Assert.AreEqual(1, result.Library.ArticleList.Count);
			Assert.AreEqual(1, result.Library.StringArray.Length);
		}
#endif

		[Test]
		public void DeserializeForEmptyStringReturnsNull()
		{
			string json = @"";

			var deserializer = JsonSerializer.Create(new JsonSerializerSettings());

			List<Person> persons;
			using (var stringReader = new StringReader(json))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				persons = deserializer.Deserialize<List<Person>>(jsonReader);
			}

			Assert.IsNull(persons);
		}

		[Test]
		public void DeserializeForNullReturnsNull()
		{
			string json = @"null";

			var deserializer = JsonSerializer.Create(new JsonSerializerSettings());

			List<Person> persons;
			using (var stringReader = new StringReader(json))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				persons = deserializer.Deserialize<List<Person>>(jsonReader);
			}

			Assert.IsNotNull(persons);
			Assert.AreEqual(1, persons.Count);
			Assert.IsNull(persons[0]);
		}

		[Test]
		public void DeserializeForDefaultObjectReturnsListWithDefaultObject()
		{
			string json = @"{}";

			var deserializer = JsonSerializer.Create(new JsonSerializerSettings());

			List<Person> persons;
			using (var stringReader = new StringReader(json))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				persons = deserializer.Deserialize<List<Person>>(jsonReader);
			}

			Assert.IsNotNull(persons);
			Assert.AreEqual(1, persons.Count);
			Assert.IsNotNull(persons[0]);
		}

		[Test]
		public void DeserializeForEmptyListReturnsEmptyList()
		{
			string json = @"[]";

			var deserializer = JsonSerializer.Create(new JsonSerializerSettings());

			List<Person> persons;
			using (var stringReader = new StringReader(json))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				persons = deserializer.Deserialize<List<Person>>(jsonReader);
			}

			Assert.IsNotNull(persons);
			Assert.AreEqual(0, persons.Count);
		}


		[Test]
		public void DeserializeForListWithNullReturnsListWithANullValue()
		{
			string json = @"[null]";

			var deserializer = JsonSerializer.Create(new JsonSerializerSettings());

			List<Person> persons;
			using (var stringReader = new StringReader(json))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				persons = deserializer.Deserialize<List<Person>>(jsonReader);
			}

			Assert.IsNotNull(persons);
			Assert.AreEqual(1, persons.Count);
			Assert.IsNull(persons[0]);
		}

		[Test]
		public void DeserializeForNullReturnsListWithADefaultValue()
		{
			string json = @"[{}]";

			var deserializer = JsonSerializer.Create(new JsonSerializerSettings());

			List<Person> persons;
			using (var stringReader = new StringReader(json))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				persons = deserializer.Deserialize<List<Person>>(jsonReader);
			}

			Assert.IsNotNull(persons);
			Assert.AreEqual(1, persons.Count);
			Assert.IsNotNull(persons[0]);
		}

		[Test]
		public void JsonArraySpecifiedToJson0ItemInChildCollection()
		{
			var jsonText2 = @"{
  ""Name"": ""0Items"",
  ""ArticleCollection"": [],
  ""ArticleList"": [],
  ""ArticleIList"": [],
  ""ArticleArray"": [],
  ""StringArray"": []
}";

			var deserializer = JsonSerializer.Create(new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				ContractResolver = new DefaultContractResolver
				{
					DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
				},
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,

				NullValueHandling = NullValueHandling.Ignore,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				ObjectCreationHandling = ObjectCreationHandling.Auto,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
			});

			Library result;
			using (var stringReader = new StringReader(jsonText2))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				result = deserializer.Deserialize<Library>(jsonReader);
			}

			Assert.AreEqual(0, result.ArticleArray.Count());
			Assert.AreEqual(0, result.ArticleCollection.Count());
			Assert.AreEqual(0, result.ArticleList.Count());
			Assert.AreEqual(0, result.ArticleList.Count());
			Assert.AreEqual(0, result.StringArray.Count());
		}

		[Test]
		public void JsonArraySpecifiedToJson1ItemInChildCollection()
		{
			var jsonText2 = @"{
  ""Name"": ""1Items"",
  ""ArticleCollection"": [{
    ""Name"": ""Article1""
  }],
  ""ArticleList"": [{
    ""Name"": ""Article1""
  }],
  ""ArticleIList"": [{
    ""Name"": ""Article1""
  }],
  ""ArticleArray"": [{
    ""Name"": ""Article1""
  }],
  ""StringArray"": [""Article1""]
}";

			var deserializer = JsonSerializer.Create(new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				ContractResolver = new DefaultContractResolver
				{
					DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
				},
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,

				NullValueHandling = NullValueHandling.Ignore,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				ObjectCreationHandling = ObjectCreationHandling.Auto,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
			});

			Library result;
			using (var stringReader = new StringReader(jsonText2))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				result = deserializer.Deserialize<Library>(jsonReader);
			}

			Assert.AreEqual(1, result.ArticleArray.Count());
			Assert.AreEqual(1, result.ArticleCollection.Count());
			Assert.AreEqual(1, result.ArticleList.Count());
			Assert.AreEqual(1, result.ArticleList.Count());
			Assert.AreEqual(1, result.StringArray.Count());
		}

		[Test]
		public void JsonArraySpecifiedToJson2ItemInChildCollection()
		{
			var jsonText2 = @"{
  ""Name"": ""2Items"",
  ""ArticleCollection"": [{""Name"": ""Article1""},{""Name"": ""Article2""}],
  ""ArticleList"": [{""Name"": ""Article1""},{""Name"": ""Article2""}],
  ""ArticleIList"": [{""Name"": ""Article1""},{""Name"": ""Article2""}],
  ""ArticleArray"": [{""Name"": ""Article1""},{""Name"": ""Article2""}],
  ""StringArray"": [""Article1"",""Article2""]
}";

			var deserializer = JsonSerializer.Create(new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				ContractResolver = new DefaultContractResolver
				{
					DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
				},
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,

				NullValueHandling = NullValueHandling.Ignore,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				ObjectCreationHandling = ObjectCreationHandling.Auto,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
			});

			Library result;
			using (var stringReader = new StringReader(jsonText2))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				result = deserializer.Deserialize<Library>(jsonReader);
			}

			Assert.AreEqual(2, result.ArticleArray.Count());
			Assert.AreEqual(2, result.ArticleCollection.Count());
			Assert.AreEqual(2, result.ArticleList.Count());
			Assert.AreEqual(2, result.ArticleList.Count());
			Assert.AreEqual(2, result.StringArray.Count());
		}

		[Test]
		public void XmlNodeWithWriteArrayAttributeTextToJson0ItemInChildCollection()
		{
			var deserializer = JsonSerializer.Create(new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				ObjectCreationHandling = ObjectCreationHandling.Auto,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
				Converters = new List<JsonConverter>(new[]
							{
								new XmlNodeConverter
									{
										DeserializeRootElementName = "root",
										WriteArrayAttribute = true,
										OmitRootObject = true,
									}
							})
			});

			var jsonText1 = @"{
  ""Name"": ""1Items"",
  ""StringArray"": []
}";

			var json = new StringBuilder(jsonText1);
			var jsonText2 = string.Empty;
			using (var stringReader = new StringReader(json.ToString()))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				var document = (XDocument)deserializer.Deserialize(jsonReader, typeof(XDocument));
				jsonText2 = JsonConvert.SerializeXNode(document, Formatting.Indented, true);
			}

			Library result;
			using (var stringReader = new StringReader(jsonText2))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				result = deserializer.Deserialize<Library>(jsonReader);
			}
			Assert.IsNull(result.StringArray);
		}


		[Test]
		public void XmlNodeWithWriteArrayAttributeTextToJson1ItemInChildCollection()
		{
			var deserializer = JsonSerializer.Create(new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				ObjectCreationHandling = ObjectCreationHandling.Auto,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
				Converters = new List<JsonConverter>(new[]
							{
								new XmlNodeConverter
									{
										DeserializeRootElementName = "root",
										WriteArrayAttribute = true,
										OmitRootObject = true,
									}
							})
			});

			var jsonText1 = @"{
  ""Name"": ""1Items"",
  ""StringArray"": [""Article1""]
}";

			var json = new StringBuilder(jsonText1);
			var jsonText2 = string.Empty;
			using (var stringReader = new StringReader(json.ToString()))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				var document = (XDocument)deserializer.Deserialize(jsonReader, typeof(XDocument));
				jsonText2 = JsonConvert.SerializeXNode(document, Formatting.Indented, true);
			}

			Library result;
			using (var stringReader = new StringReader(jsonText2))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				result = deserializer.Deserialize<Library>(jsonReader);
			}
			Assert.IsNotNull(result.StringArray);
			Assert.AreEqual(1, result.StringArray.Count());
		}

		[Test]
		public void XmlNodeWithWriteArrayAttributeTextToJson2ItemInChildCollection()
		{
			var deserializer = JsonSerializer.Create(new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				ObjectCreationHandling = ObjectCreationHandling.Auto,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
				Converters = new List<JsonConverter>(new[]
							{
								new XmlNodeConverter
									{
										DeserializeRootElementName = "root",
										WriteArrayAttribute = true,
										OmitRootObject = true,
									}
							})
			});

			var jsonText1 = @"{
  ""Name"": ""1Items"",
  ""StringArray"": [""Article1"", ""Article2""]
}";

			var json = new StringBuilder(jsonText1);
			var jsonText2 = string.Empty;
			using (var stringReader = new StringReader(json.ToString()))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				var document = (XDocument)deserializer.Deserialize(jsonReader, typeof(XDocument));
				jsonText2 = JsonConvert.SerializeXNode(document, Formatting.Indented, true);
			}

			Library result;
			using (var stringReader = new StringReader(jsonText2))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				result = deserializer.Deserialize<Library>(jsonReader);
			}
			Assert.IsNotNull(result.StringArray);
			Assert.AreEqual(2, result.StringArray.Count());
		}

		[Test]
		public void ToXmlThenToJson1ItemInChildCollection()
		{
			var library = new Library();

			library.Name = "1Items";
			library.ArticleArray = new Article[] { new Article("Article1") };
			library.ArticleList = new List<Article>() { new Article("Article1") };
			library.ArticleIList = new List<Article>() { new Article("Article1") };
			library.ArticleCollection = new ArticleCollection() { new Article("Article1") };
			library.StringArray = new[] { "Article1" };

			var jsonText = JsonConvert.SerializeObject(library, Formatting.Indented);
			var xmlDocument = JsonConvert.DeserializeXmlNode(jsonText, "root");
			var jsonText2 = JsonConvert.SerializeXmlNode(xmlDocument, Formatting.Indented, true);

			var deserializer = JsonSerializer.Create(new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				ContractResolver = new DefaultContractResolver
				{
					DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
				},
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,

				NullValueHandling = NullValueHandling.Ignore,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				ObjectCreationHandling = ObjectCreationHandling.Auto,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
			});

			Library result;
			using (var stringReader = new StringReader(jsonText2))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				result = deserializer.Deserialize<Library>(jsonReader);
			}

			Assert.AreEqual(library.ArticleArray.Count(), result.ArticleArray.Count());
			Assert.AreEqual(library.ArticleCollection.Count(), result.ArticleCollection.Count());
			Assert.AreEqual(library.ArticleList.Count(), result.ArticleList.Count());
			Assert.AreEqual(library.ArticleIList.Count(), result.ArticleList.Count());
			Assert.AreEqual(library.StringArray.Count(), result.StringArray.Count());
		}


		[Test]
		public void ToXmlThenToJson2ItemInChildCollection()
		{
			var library = new Library();

			library.Name = "2Items";
			library.ArticleArray = new Article[] { new Article("Article1"), new Article("Article2") };
			library.ArticleList = new List<Article>() { new Article("Article1"), new Article("Article2") };
			library.ArticleIList = new List<Article>() { new Article("Article1"), new Article("Article2") };
			library.ArticleCollection = new ArticleCollection() { new Article("Article1"), new Article("Article2") };
			library.StringArray = new[] { "Article1", "Article2" };

			var jsonText = JsonConvert.SerializeObject(library, Formatting.Indented);
			var xmlDocument = JsonConvert.DeserializeXmlNode(jsonText, "root");
			var jsonText2 = JsonConvert.SerializeXmlNode(xmlDocument, Formatting.Indented, true);

			var deserializer = JsonSerializer.Create(new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				ContractResolver = new DefaultContractResolver
				{
					DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
				},
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,

				NullValueHandling = NullValueHandling.Ignore,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				ObjectCreationHandling = ObjectCreationHandling.Auto,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
			});

			Library result;
			using (var stringReader = new StringReader(jsonText2))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				result = deserializer.Deserialize<Library>(jsonReader);
			}

			Assert.AreEqual(library.ArticleArray.Count(), result.ArticleArray.Count());
			Assert.AreEqual(library.ArticleCollection.Count(), result.ArticleCollection.Count());
			Assert.AreEqual(library.ArticleList.Count(), result.ArticleList.Count());
			Assert.AreEqual(library.ArticleIList.Count(), result.ArticleList.Count());
			Assert.AreEqual(library.StringArray.Count(), result.StringArray.Count());
		}

		[Test]
		public void ToXmlThenToJson0ItemInChildCollection()
		{
			var library = new Library();

			library.Name = "0Items";
			library.ArticleArray = new Article[] { };
			library.ArticleList = new List<Article>() { };
			library.ArticleIList = new List<Article>() { };
			library.ArticleCollection = new ArticleCollection() { };
			library.StringArray = new string[] { };

			var jsonText = JsonConvert.SerializeObject(library, Formatting.Indented);
			var xmlDocument = JsonConvert.DeserializeXmlNode(jsonText, "root");
			var jsonText2 = JsonConvert.SerializeXmlNode(xmlDocument, Formatting.Indented, true);

			var deserializer = JsonSerializer.Create(new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				ContractResolver = new DefaultContractResolver
				{
					DefaultMembersSearchFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
				},
				TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,

				NullValueHandling = NullValueHandling.Ignore,
				DefaultValueHandling = DefaultValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore,
				ObjectCreationHandling = ObjectCreationHandling.Auto,
				ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
			});

			Library result;
			using (var stringReader = new StringReader(jsonText2))
			using (var jsonReader = new JsonTextReader(stringReader))
			{
				result = deserializer.Deserialize<Library>(jsonReader);
			}

			Assert.IsNull(result.ArticleArray);
			Assert.IsNull(result.ArticleCollection);
			Assert.IsNull(result.ArticleList);
			Assert.IsNull(result.ArticleIList);
			Assert.IsNull(result.StringArray);
		}
	}
}