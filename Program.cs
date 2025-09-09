
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem
{
    #region 
    enum ItemType { Novel, Magazine, TextBook }

    abstract class LibraryItem
    {
      
        private readonly int _id;
        private readonly string _title;

        public int Id => _id;
        public string Title => _title;
        public ItemType Type { get; }

        protected LibraryItem(int id, string title, ItemType type)
        {
            _id = id;
            _title = title;
            Type = type;
        }

     
        public abstract string GetDetails();
    }
    #endregion

    #region 
    class Novel : LibraryItem
    {
        private readonly string _author;
        public string Author => _author;

        public Novel(int id, string title, string author)
            : base(id, title, ItemType.Novel)
        {
            _author = author;
        }

        public override string GetDetails() => $"[Novel] {Title} by {Author}";
    }

    class Magazine : LibraryItem
    {
        private readonly int _issueNumber;
        public int IssueNumber => _issueNumber;

        public Magazine(int id, string title, int issue)
            : base(id, title, ItemType.Magazine)
        {
            _issueNumber = issue;
        }

        public override string GetDetails() => $"[Magazine] {Title}, Issue #{IssueNumber}";
    }

    class TextBook : LibraryItem
    {
        private readonly string _publisher;
        public string Publisher => _publisher;

        public TextBook(int id, string title, string publisher)
            : base(id, title, ItemType.TextBook)
        {
            _publisher = publisher;
        }

        public override string GetDetails() => $"[TextBook] {Title} published by {Publisher}";
    }
    #endregion

    #region 
    class Member
    {
        private readonly string _name;
        public string Name => _name;

        private readonly List<LibraryItem> _borrowedItems = new();

        public Member(string name)
        {
            _name = name;
        }

        public string BorrowItem(LibraryItem item)
        {
            if (_borrowedItems.Count >= 3)
                return "You cannot borrow more than 3 items.";

            _borrowedItems.Add(item);
            return $"{item.Title} has been added to {Name}'s list of borrowed books.";
        }

        public string ReturnItem(LibraryItem item)
        {
            if (_borrowedItems.Contains(item))
            {
                _borrowedItems.Remove(item);
                return $"{item.Title} has been successfully returned.";
            }
            return $"{item.Title} was not in the list of borrowed items.";
        }

        public IReadOnlyList<LibraryItem> GetBorrowedItems() => _borrowedItems;
    }
    #endregion

    #region 
    class LibraryManager
    {
        private readonly List<LibraryItem> _catalog = new();
        private readonly List<Member> _members = new();

        public void AddItem(LibraryItem item) => _catalog.Add(item);

        public void RegisterMember(Member member) => _members.Add(member);

        public void ShowCatalog()
        {
            Console.WriteLine("--- Library Catalog ---");
            foreach (var item in _catalog)
                Console.WriteLine(item.GetDetails());
           
        }

        public LibraryItem? FindItemById(int id) => _catalog.Find(i => i.Id == id);

        public Member? FindMemberByName(string name) => _members.Find(m => m.Name == name);
    }
    #endregion

    #region 
    internal class Program
    {
        static void Main()
        {
            LibraryManager library = new();

          
            library.AddItem(new Novel(1, "The Hobbit", "J.R.R. Tolkien"));
            library.AddItem(new Magazine(2, "National Geographic", 202));
            library.AddItem(new TextBook(3, "Introduction to Algorithms", "MIT Press"));

        
            Member alice = new("Alice");
            Member bob = new("Bob");
            library.RegisterMember(alice);
            library.RegisterMember(bob);

       
            library.ShowCatalog();

        
            for (int id = 1; id <= 3; id++)
            {
                var item = library.FindItemById(id);
                if (item != null)
                    Console.WriteLine(alice.BorrowItem(item));
            }

           
            var newNovel = new Novel(4, "Dune", "Frank Herbert");
            library.AddItem(newNovel);
            Console.WriteLine(alice.BorrowItem(newNovel));   

           
            Console.WriteLine($"\n{alice.Name} currently borrowed:");
            foreach (var bi in alice.GetBorrowedItems())
                Console.WriteLine($" {bi.GetDetails()}");

      
            Console.WriteLine(alice.ReturnItem(newNovel));   
            var returned = library.FindItemById(2);
            if (returned != null)
                Console.WriteLine(alice.ReturnItem(returned)); 
        }
    }
    #endregion
}