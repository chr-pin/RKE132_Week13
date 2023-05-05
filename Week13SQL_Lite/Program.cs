using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Xml.Linq;


//ReadData(CreateConnection());
//Insertcustomer(CreateConnection());
//RemoveCustomer(CreateConnection());
//FindCustomer(CreateConnection());
//DisplayProduct(CreateConnection());
//DisplayProductWithCategory(CreateConnection());
InsertCustomer(CreateConnection());


static SQLiteConnection CreateConnection()
{
    SQLiteConnection connection = new SQLiteConnection("Data source=bar.db; Version = 3; New = True; Compress = True");

    try
    {
        connection.Open();
        Console.WriteLine("DB found");
    }
    catch
    {
        Console.WriteLine("Db not found");
    }

    return connection;
}


static void ReadData(SQLiteConnection myConnection)
{
    Console.Clear();
    SQLiteDataReader reader;
    SQLiteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText = "SELECT rowid, * FROM Customer";
    reader = command.ExecuteReader();
    while (reader.Read())
    {
        int readerRowId = reader.GetInt32(0);
        string readerStringFirstname = reader.GetString(1);
        string readerStringLAstname = reader.GetString(2);
        string readerStringStatus = reader.GetString(3);

        Console.WriteLine($"Full name : {readerRowId} {readerStringFirstname} {readerStringLAstname}; Status: {readerStringStatus}");
    }
    myConnection.Close(); 
}

static void Insertcustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;
    string fName, lName, dob;

    Console.WriteLine("Enter first name:");
    fName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    lName = Console.ReadLine();
    Console.WriteLine("Enter date of birth (mm-dd-YYYY:");
    dob = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO customer(firstname, lastName, dateOfBirth) " +
        $"VALUES ('{fName}', '{lName}', '{dob}')";

    int rowInserted = command.ExecuteNonQuery();
    Console.WriteLine($"Row inserted: {rowInserted}");
    
    ReadData(myConnection);

}

static void RemoveCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;
    
    Console.WriteLine("Enter an ide to delete a customer:");
    string idToDelete = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"DELETE FROM customer WHERE rowid = {idToDelete}";
    int rowDeleted = command.ExecuteNonQuery();
    Console.WriteLine($"{rowDeleted} was removed from the table customer.");
    ReadData(myConnection);
}

static void FindCustomer(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;
    string searchName;
    Console.WriteLine("Enter a first name to display customer data:");
    searchName = Console.ReadLine();
    command = myConnection.CreateCommand();
    command.CommandText = $"SELECT customer.rowid, customer.firstName, customer.Lastname, statusType " +
        $"FROM customerStatus " +
        $"JOIN customer ON customer.rowid = customerStatus.customerId " +
        $"JOIN status ON status.rowid = customerStatus.statusId " +
        $"WHERE firstname LIKE '{searchName}'";
    reader = command.ExecuteReader();
    while (reader.Read())
    {
        string readerRowid = reader["rowid"].ToString();
        string readerStringName = reader.GetString(1);
        string readerStringlastName = reader.GetString(2);
        string readerStringStatus = reader.GetString(3);
        Console.WriteLine($"Search result: ID: {readerRowid}. {readerStringName} {readerStringlastName}. Status: {readerStringStatus}");
    }
    myConnection.Close();   
}

static void DisplayProduct(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;
    command = myConnection.CreateCommand();
    command.CommandText = "SELECT rowid, productName, Price FROM product";
    reader = command.ExecuteReader();
    while (reader.Read())
    {
        string readerRowid = reader["rowid"].ToString();
        string readerProductName = reader.GetString(1);
        int readerProductPrice = reader.GetInt32(2);
        Console.WriteLine($"{readerRowid}. {readerProductName}. Price: {readerProductPrice}");

    }
    myConnection.Close();
}

static void DisplayProductWithCategory(SQLiteConnection myConnection)
{
    SQLiteDataReader reader;
    SQLiteCommand command;

    command = myConnection.CreateCommand();
    command.CommandText = "SELECT product.rowid, product.ProductName, ProductCategory.CategoryName FROM product " +
        "JOIN  ProductCategory ON ProductCategory.rowid = Product.CategoryId";
    reader = command.ExecuteReader();
    while(reader.Read())
    {
        string readerRowid = reader["rowid"].ToString();
        string readerProductName = reader.GetString(1);
        string readerProductCategory = reader.GetString(2);

        Console.WriteLine($"{readerRowid}. {readerProductName}. Category: {readerProductCategory}");

    }
    myConnection.Close();
}

static void InsertCustomer(SQLiteConnection myConnection)
{
    SQLiteCommand command;
    string fName, lName;

    Console.WriteLine("First name:");
    fName = Console.ReadLine();

    Console.WriteLine("Last name:");
    lName = Console.ReadLine();

    command = myConnection.CreateCommand();
    command.CommandText = $"INSERT INTO Customer (firstName, lastName) " +
        $"VALUES ('{fName}', '{lName}')";

    int rowsInserted = command.ExecuteNonQuery();
    Console.WriteLine($"{rowsInserted} new row has been inserted.");

    ReadData(myConnection);
}

