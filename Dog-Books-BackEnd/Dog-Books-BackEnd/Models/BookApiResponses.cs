namespace Dog_Books_BackEnd.Models
{

        public class OpenLibraryBookResponse
        {
            public string Title { get; set; }
            public string FullTitle { get; set; }
            public List<OpenLibraryAuthors> Authors { get; set; }
        }

        public class OpenLibraryAuthors
        {
            public string Key { get; set; }
        }

        public class ParsedBookResponse
        {
            public string Title { get; set; }
            public string FullTitle { get; set; }
            public string AuthorKey { get; set; }
        }

        public class OpenLibraryAuthor
        {
            public string Name { get; set; }
            public string Key { get; set; }
        }

        public class SavedBooks
        {
            public string Title { get; set; }
            public string Author { get; set; }
        }
}
