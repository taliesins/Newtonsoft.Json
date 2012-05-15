using System.Collections.Generic;

namespace Newtonsoft.Json.Tests.TestObjects
{
	public class Library
	{
		public string Name { get; set; }
		public ArticleCollection ArticleCollection { get; set; }
		public List<Article> ArticleList { get; set; }
		public IList<Article> ArticleIList { get; set; }
		public Article[] ArticleArray { get; set; }
		public string[] StringArray { get; set; }
	}
}
