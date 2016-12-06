using Xunit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Inventory
{
  public class InventoryTest : IDisposable
  {
    public InventoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Book.GetAll().Count;
      Assert.Equal(0, result); // Currently there are 8 books that were added via Powershell when DB was created, 4 books from Shakespeare and 4 books from Lovecraft
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      Book firstBook = new Book("The Shadow Out of Time", 2);
      Book secondBook = new Book("The Shadow Out of Time", 2);
      Assert.Equal(firstBook, secondBook);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      Book testBook = new Book("Much Ado About Nothing", 1);
      testBook.Save();
      List<Book> result = Book.GetAll();
      List<Book> testList = new List<Book>{testBook};
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      Book testBook = new Book("Much Ado About Nothing", 1);
      testBook.Save();
      Book savedBook = Book.GetAll()[0];
      int result = savedBook.GetBookId();
      int testId = testBook.GetBookId();
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      Book testBook = new Book("Much Ado About Nothing", 1);
      testBook.Save();

      Book foundBook = Book.Find(testBook.GetBookId());
      Assert.Equal(testBook, foundBook);
    }

    public void Dispose()
    {
      Book.DeleteAll();
    }
  }
}
