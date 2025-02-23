# POS Inventory Management System

A point of sales system with an inventory management system of a retail store made for an advanced relational database project to emulate service and sales transactions.

## Usage
To use the program, open the .sln in Visual Studio to build. If you want to try using your database, open Form1.cs and change the Data Source to the name of your server and Initial Catalog to your specified database.
```csharp
public partial class Form1 : Form
{
  ...
  string connectionString = @"Data Source=Your_Server;Initial Catalog=Your_Database;Integrated Security=true";
  ...
}
```
## Images
1st page of Form1
![image](https://github.com/user-attachments/assets/2dc8b078-e37d-4fae-9677-f0be4aaca8df)<br>
Sales Form
![image](https://github.com/user-attachments/assets/eccb13b8-192d-474a-a856-bba1e5c4ca7a)<br>
Service Form
![image](https://github.com/user-attachments/assets/0d33050b-c2e3-4cbb-a4c0-fcdd43363a89)
