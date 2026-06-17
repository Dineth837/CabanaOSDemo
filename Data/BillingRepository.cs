using CabanaOSDemo.Models;
using CabanaOSDemo.Utils;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace CabanaOSDemo.Data
{
    public static class BillingRepository
    {
        
        public static SuiteMaster[] MasterSuites { get; set; } = new SuiteMaster[22];
        public static TableMaster[] MasterTables { get; set; } = new TableMaster[22];

        public static List<Customer> AllCustomers = new List<Customer>();

        
        public static ObservableCollection<RestaurantBillingRecord> RestaurantInvoices { get; set; }
        public static ObservableCollection<RoomBillingRecord> RoomInvoices { get; set; }

        
        private static readonly string DbConnectionString = "Data Source=CabanaDatabase.db";

        
        static BillingRepository()
        {
            RestaurantInvoices = new ObservableCollection<RestaurantBillingRecord>();
            RoomInvoices = new ObservableCollection<RoomBillingRecord>();

            InitializeDatabase();

            if (!LoadAllDataFromFiles())
            {
                SeedPhysicalResortAssets();
                SeedInitialInvoices();
                SaveAllDataToFiles();
            }
        }

        private static void InitializeDatabase()
        {
            SqliteConnection dbConnection;
            SqliteCommand dbCommand;
            string createAllTablesQuery;

            createAllTablesQuery = @"
                CREATE TABLE IF NOT EXISTS Customers (CustomerID TEXT PRIMARY KEY, FullName TEXT, NICNumber TEXT, CustomerType TEXT);
                CREATE TABLE IF NOT EXISTS RoomInvoices (BookingID TEXT PRIMARY KEY, RoomNumber TEXT, CustomerName TEXT, Category TEXT, Date TEXT, States TEXT, TotalDue TEXT);
                CREATE TABLE IF NOT EXISTS RestaurantInvoices (OrderID TEXT PRIMARY KEY, TableNumber TEXT, CustomerName TEXT, Date TEXT, States TEXT, TotalBill TEXT);
                CREATE TABLE IF NOT EXISTS MasterTables (TableID TEXT PRIMARY KEY, Zone TEXT, Type TEXT, States TEXT);
                CREATE TABLE IF NOT EXISTS MasterSuites (RoomID TEXT PRIMARY KEY, Tier TEXT, Price REAL, Type TEXT, States TEXT);
                CREATE TABLE IF NOT EXISTS FoodOrders (OrderID TEXT, ItemName TEXT, Price REAL, Quantity INTEGER);
                CREATE TABLE IF NOT EXISTS SystemUsers (Username TEXT PRIMARY KEY, PasswordHash TEXT, FullName TEXT, Role TEXT, IsActive INTEGER);
                CREATE TABLE IF NOT EXISTS TotalRevenue (revID TEXT PRIMARY KEY, month TEXT NOT NULL, year INTEGER NOT NULL, totalRoomRevenue REAL DEFAULT 0.0, totalRestaurantRevenue REAL DEFAULT 0.0);";

            dbConnection = new SqliteConnection(DbConnectionString);
            using (dbConnection)
            {
                dbConnection.Open();
                dbCommand = new SqliteCommand(createAllTablesQuery, dbConnection);
                using (dbCommand)
                {
                    dbCommand.ExecuteNonQuery();
                }
            }
        }

        private static void SeedPhysicalResortAssets()
        {
            MasterSuites = new SuiteMaster[22];
            MasterTables = new TableMaster[22];

            var suiteList = new List<SuiteMaster> {
                // Standard Couple
                new SuiteMaster { RoomID = "ST01",     Tier = "Standard", Price = 8500.0,  Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "ST02",     Tier = "Standard", Price = 8500.0,  Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "ST03",     Tier = "Standard", Price = 8500.0,  Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "ST04",     Tier = "Standard", Price = 8500.0,  Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "ST05",     Tier = "Standard", Price = 8500.0,  Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "ST06",     Tier = "Standard", Price = 8500.0,  Type = "Couple", States = "CheckedOut" },
                // Standard Family
                new SuiteMaster { RoomID = "Fam ST07", Tier = "Standard", Price = 15000.0, Type = "Family", States = "CheckedOut" },
                new SuiteMaster { RoomID = "Fam ST08", Tier = "Standard", Price = 15000.0, Type = "Family", States = "CheckedOut" },
                new SuiteMaster { RoomID = "Fam ST09", Tier = "Standard", Price = 15000.0, Type = "Family", States = "CheckedOut" },
                new SuiteMaster { RoomID = "Fam ST010",Tier = "Standard", Price = 15000.0, Type = "Family", States = "CheckedOut" },
                // Superior Couple
                new SuiteMaster { RoomID = "SUP01",    Tier = "Superior", Price = 12500.0, Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "SUP02",    Tier = "Superior", Price = 12500.0, Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "SUP03",    Tier = "Superior", Price = 12500.0, Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "SUP04",    Tier = "Superior", Price = 12500.0, Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "SUP05",    Tier = "Superior", Price = 12500.0, Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "SUP06",    Tier = "Superior", Price = 12500.0, Type = "Couple", States = "CheckedOut" },
                // Deluxe Couple
                new SuiteMaster { RoomID = "DLX01",    Tier = "Deluxe",   Price = 20000.0, Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "DLX02",    Tier = "Deluxe",   Price = 20000.0, Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "DLX03",    Tier = "Deluxe",   Price = 20000.0, Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "DLX04",    Tier = "Deluxe",   Price = 20000.0, Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "DLX05",    Tier = "Deluxe",   Price = 20000.0, Type = "Couple", States = "CheckedOut" },
                new SuiteMaster { RoomID = "DLX06",    Tier = "Deluxe",   Price = 20000.0, Type = "Couple", States = "CheckedOut" }
            };
            for (int i = 0; i < suiteList.Count; i++) MasterSuites[i] = suiteList[i];

            var tableList = new List<TableMaster> {
                // Standard Tables
                new TableMaster { TableID = "ST C01",  Zone = "Standard", Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "ST C02",  Zone = "Standard", Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "ST C03",  Zone = "Standard", Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "ST C04",  Zone = "Standard", Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "ST C05",  Zone = "Standard", Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "ST C06",  Zone = "Standard", Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "ST C07",  Zone = "Standard", Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "ST C08",  Zone = "Standard", Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "ST C09",  Zone = "Standard", Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "ST C10",  Zone = "Standard", Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "ST F01",  Zone = "Standard", Type = "Family", States = "Complete" },
                new TableMaster { TableID = "ST F02",  Zone = "Standard", Type = "Family", States = "Complete" },
                new TableMaster { TableID = "ST F03",  Zone = "Standard", Type = "Family", States = "Complete" },
                new TableMaster { TableID = "ST F04",  Zone = "Standard", Type = "Family", States = "Complete" },
                // Deluxe Tables
                new TableMaster { TableID = "DLX C01", Zone = "Deluxe",   Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "DLX C02", Zone = "Deluxe",   Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "DLX C03", Zone = "Deluxe",   Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "DLX C04", Zone = "Deluxe",   Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "DLX C05", Zone = "Deluxe",   Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "DLX C06", Zone = "Deluxe",   Type = "Couple", States = "Complete" },
                new TableMaster { TableID = "DLX F01", Zone = "Deluxe",   Type = "Family", States = "Complete" },
                new TableMaster { TableID = "DLX F02", Zone = "Deluxe",   Type = "Family", States = "Complete" }
            };
            for (int i = 0; i < tableList.Count; i++) MasterTables[i] = tableList[i];

            SaveAllDataToFiles();
        }

        private static void SeedInitialInvoices()
        {
            // Left intentionally empty.
            // On Day 1, a real hotel has 0 active bills and 0 occupied rooms.
        }

        

        // UPDATE TABLE STATUS (For Restaurant)
        public static void UpdateTableStatus(string tableId, string newStatus)
        {
            using (var connection = new SqliteConnection(DbConnectionString))
            {
                connection.Open();
                var command = new SqliteCommand("UPDATE MasterTables SET States = @status WHERE TableID = @id", connection);
                command.Parameters.AddWithValue("@status", newStatus);
                command.Parameters.AddWithValue("@id", tableId);
                command.ExecuteNonQuery();
            }
            var table = MasterTables.FirstOrDefault(t => t != null && t.TableID == tableId);
            if (table != null) table.States = newStatus;
        }

        // UPDATE SUITE STATUS (For Rooms)
        public static void UpdateSuiteStatus(string roomId, string newStatus)
        {
            // Update Memory
            var suite = MasterSuites.FirstOrDefault(s => s != null && s.RoomID == roomId);
            if (suite != null) suite.States = newStatus;

            // Update Database Atomically
            using (var connection = new SqliteConnection(DbConnectionString))
            {
                connection.Open();
                var command = new SqliteCommand("UPDATE MasterSuites SET States = @status WHERE RoomID = @id", connection);
                command.Parameters.AddWithValue("@status", newStatus);
                command.Parameters.AddWithValue("@id", roomId);
                command.ExecuteNonQuery();
            }
        }

        
        public static bool EnsureCustomerRegistered(string fullName, string nicNumber, string customerType, string customerID)
        {
            using (var connection = new SqliteConnection(DbConnectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM Customers WHERE NICNumber = @NIC";
                using (var cmd = new SqliteCommand(checkQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@NIC", nicNumber);
                    long count = (long)cmd.ExecuteScalar();
                    if (count > 0) return false;
                }

                string insertQuery = @"INSERT INTO Customers (CustomerID, FullName, NICNumber, CustomerType)
                                       VALUES (@ID, @Name, @NIC, @Type)";
                using (var cmd = new SqliteCommand(insertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", customerID);
                    cmd.Parameters.AddWithValue("@Name", fullName);
                    cmd.Parameters.AddWithValue("@NIC", nicNumber);
                    cmd.Parameters.AddWithValue("@Type", customerType);
                    cmd.ExecuteNonQuery();
                }

                AllCustomers.Add(new Customer
                {
                    CustomerID = customerID,
                    FullName = fullName,
                    NICNumber = nicNumber,
                    CustomerType = customerType
                });

                return true;
            }
        }

        public static void SaveAllDataToFiles()
        {
            SqliteConnection dbConnection;
            SqliteCommand dbCommand;
            SqliteCommand insertCmd;
            SqliteTransaction dbTransaction;
            string clearTablesQuery;

            clearTablesQuery = "DELETE FROM RestaurantInvoices; DELETE FROM RoomInvoices; DELETE FROM MasterSuites; DELETE FROM MasterTables;";

            try
            {
                dbConnection = new SqliteConnection(DbConnectionString);
                using (dbConnection)
                {
                    dbConnection.Open();
                    dbTransaction = dbConnection.BeginTransaction();
                    using (dbTransaction)
                    {
                        dbCommand = new SqliteCommand(clearTablesQuery, dbConnection, dbTransaction);
                        using (dbCommand)
                        {
                            dbCommand.ExecuteNonQuery();
                        }

                        // Insert Restaurant Invoices
                        foreach (var r in RestaurantInvoices)
                        {
                            insertCmd = new SqliteCommand("INSERT INTO RestaurantInvoices VALUES (@id, @table, @cust, @date, @states, @bill)", dbConnection, dbTransaction);
                            using (insertCmd)
                            {
                                insertCmd.Parameters.AddWithValue("@id", r.OrderID);
                                insertCmd.Parameters.AddWithValue("@table", r.TableNumber);
                                insertCmd.Parameters.AddWithValue("@cust", r.CustomerName ?? "");
                                insertCmd.Parameters.AddWithValue("@date", r.Date ?? "");
                                insertCmd.Parameters.AddWithValue("@states", r.States ?? "");
                                insertCmd.Parameters.AddWithValue("@bill", r.TotalBill ?? "Rs. 00.00");
                                insertCmd.ExecuteNonQuery();
                            }
                        }

                        // Insert Room Invoices
                        foreach (var r in RoomInvoices)
                        {
                            string query = "INSERT INTO RoomInvoices (BookingID, RoomNumber, CustomerName, Category, Date, States, TotalDue) " +
                                           "VALUES (@bid, @room, @cust, @cat, @date, @states, @due)";
                            insertCmd = new SqliteCommand(query, dbConnection, dbTransaction);
                            using (insertCmd)
                            {
                                insertCmd.Parameters.AddWithValue("@bid", r.BookingID ?? "N/A");
                                insertCmd.Parameters.AddWithValue("@room", r.RoomNumber);
                                insertCmd.Parameters.AddWithValue("@cust", r.CustomerName ?? "");
                                insertCmd.Parameters.AddWithValue("@cat", r.Category ?? "");
                                insertCmd.Parameters.AddWithValue("@date", r.Date ?? "");
                                insertCmd.Parameters.AddWithValue("@states", r.States ?? "");
                                insertCmd.Parameters.AddWithValue("@due", r.TotalDue ?? "Rs. 00.00");
                                insertCmd.ExecuteNonQuery();
                            }
                        }

                        // Insert Master Tables
                        foreach (var t in MasterTables.Where(x => x != null))
                        {
                            insertCmd = new SqliteCommand("INSERT INTO MasterTables VALUES (@id, @zone, @type, @states)", dbConnection, dbTransaction);
                            using (insertCmd)
                            {
                                insertCmd.Parameters.AddWithValue("@id", t.TableID);
                                insertCmd.Parameters.AddWithValue("@zone", t.Zone ?? "");
                                insertCmd.Parameters.AddWithValue("@type", t.Type ?? "");
                                insertCmd.Parameters.AddWithValue("@states", t.States ?? "");
                                insertCmd.ExecuteNonQuery();
                            }
                        }

                        // Insert Master Suites
                        foreach (var s in MasterSuites.Where(x => x != null))
                        {
                            insertCmd = new SqliteCommand("INSERT INTO MasterSuites VALUES (@id, @tier, @price, @type, @states)", dbConnection, dbTransaction);
                            using (insertCmd)
                            {
                                insertCmd.Parameters.AddWithValue("@id", s.RoomID);
                                insertCmd.Parameters.AddWithValue("@tier", s.Tier ?? "");
                                insertCmd.Parameters.AddWithValue("@price", s.Price);
                                insertCmd.Parameters.AddWithValue("@type", s.Type ?? "");
                                insertCmd.Parameters.AddWithValue("@states", s.States ?? "");
                                insertCmd.ExecuteNonQuery();
                            }
                        }

                        dbTransaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Database Write Failure: {ex.Message}", "Storage Error",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public static bool LoadAllDataFromFiles()
        {
            SqliteConnection dbConnection;
            SqliteCommand dbCommand;
            SqliteDataReader reader;
            RestaurantBillingRecord loadedRestRecord;
            RoomBillingRecord loadedRoomRecord;
            TableMaster loadedTable;
            SuiteMaster loadedSuite;
            bool hasData;
            int tableIndex;
            int suiteIndex;

            hasData = false;
            tableIndex = 0;
            suiteIndex = 0;

            try
            {
                dbConnection = new SqliteConnection(DbConnectionString);
                using (dbConnection)
                {
                    dbConnection.Open();

                    // Load Restaurant Invoices
                    dbCommand = new SqliteCommand("SELECT * FROM RestaurantInvoices", dbConnection);
                    using (dbCommand)
                    {
                        reader = dbCommand.ExecuteReader();
                        using (reader)
                        {
                            RestaurantInvoices.Clear();
                            while (reader.Read())
                            {
                                hasData = true;
                                loadedRestRecord = new RestaurantBillingRecord();
                                loadedRestRecord.OrderID = reader.GetString(0);
                                loadedRestRecord.TableNumber = reader.GetString(1);
                                loadedRestRecord.CustomerName = reader.GetString(2);
                                loadedRestRecord.Date = reader.GetString(3);
                                loadedRestRecord.States = reader.GetString(4);
                                loadedRestRecord.TotalBill = BillingRepository.CalculateTotalBillAll(loadedRestRecord.OrderID).ToString("F2");
                                RestaurantInvoices.Add(loadedRestRecord);
                            }
                        }
                    }

                    
                    dbCommand = new SqliteCommand("SELECT * FROM RoomInvoices", dbConnection);
                    using (dbCommand)
                    {
                        reader = dbCommand.ExecuteReader();
                        using (reader)
                        {
                            RoomInvoices.Clear();
                            while (reader.Read())
                            {
                                hasData = true;
                                loadedRoomRecord = new RoomBillingRecord();
                                loadedRoomRecord.BookingID = reader.GetString(0);
                                loadedRoomRecord.RoomNumber = reader.GetString(1);
                                loadedRoomRecord.CustomerName = reader.GetString(2);
                                loadedRoomRecord.Category = reader.GetString(3);
                                loadedRoomRecord.Date = reader.GetString(4); 
                                loadedRoomRecord.States = reader.GetString(5);
                                loadedRoomRecord.TotalDue = reader.GetString(6);
                                RoomInvoices.Add(loadedRoomRecord);
                            }
                        }
                    }

                    // Load Master Tables
                    dbCommand = new SqliteCommand("SELECT * FROM MasterTables", dbConnection);
                    using (dbCommand)
                    {
                        reader = dbCommand.ExecuteReader();
                        using (reader)
                        {
                            while (reader.Read() && tableIndex < 22)
                            {
                                loadedTable = new TableMaster();
                                loadedTable.TableID = reader.GetString(0);
                                loadedTable.Zone = reader.GetString(1);
                                loadedTable.Type = reader.GetString(2);
                                loadedTable.States = reader.GetString(3);
                                MasterTables[tableIndex] = loadedTable;
                                tableIndex++;
                            }
                        }
                    }

                    // Load Master Suites
                    dbCommand = new SqliteCommand("SELECT * FROM MasterSuites", dbConnection);
                    using (dbCommand)
                    {
                        reader = dbCommand.ExecuteReader();
                        using (reader)
                        {
                            while (reader.Read() && suiteIndex < 22)
                            {
                                loadedSuite = new SuiteMaster();
                                loadedSuite.RoomID = reader.GetString(0);
                                loadedSuite.Tier = reader.GetString(1);
                                loadedSuite.Price = reader.GetDouble(2);
                                loadedSuite.Type = reader.GetString(3);
                                loadedSuite.States = reader.GetString(4);
                                MasterSuites[suiteIndex] = loadedSuite;
                                suiteIndex++;
                            }
                        }
                    }
                }
                return hasData;
            }
            catch
            {
                return false;
            }
        }

        public static double GetTotalRoomRevenue()
        {
            double total = 0;
            using (var connection = new SqliteConnection("Data Source=CabanaDatabase.db"))
            {
                connection.Open();

                
                string query = "SELECT SUM(CAST(TotalDue AS REAL)) FROM RoomInvoices WHERE States = 'CheckedOut'";

                using (var cmd = new SqliteCommand(query, connection))
                {
                    object result = cmd.ExecuteScalar();

                    
                    if (result != null && result != DBNull.Value)
                    {
                        total = Convert.ToDouble(result);
                    }
                }
            }
            return total;
        }

        public static double GetTotalRestaurantRevenue()
        {
            double total = 0;
            using (var connection = new SqliteConnection("Data Source=CabanaDatabase.db"))
            {
                connection.Open();

                
                string query = "SELECT SUM(CAST(TotalBill AS REAL)) FROM RestaurantInvoices WHERE States = 'Complete'";

                using (var cmd = new SqliteCommand(query, connection))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        total = Convert.ToDouble(result);
                    }
                }
            }
            return total;
        }




        public static double CalculateTotalBill(string orderId)
        {
            double total = 0;

            using (var connection = new SqliteConnection("Data Source=CabanaDatabase.db"))
            {
                connection.Open();

                
                string query = "SELECT SUM(Price * Quantity) FROM FoodOrders WHERE [OrderID] = @orderId AND [States] = 'Ongoing'";

                using (var cmd = new SqliteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    object result = cmd.ExecuteScalar();

                    if (result != DBNull.Value && result != null)
                    {
                        total = Convert.ToDouble(result);
                    }
                }
            }
            return total;
        }


        
        public static List<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();

            using (var connection = new SqliteConnection(DbConnectionString))
            {
                connection.Open();
                string query = "SELECT CustomerID, FullName, NICNumber, CustomerType FROM Customers";

                using (var cmd = new SqliteCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Customer
                            {
                                CustomerID = reader.GetString(0),
                                FullName = reader.GetString(1),
                                NICNumber = reader.GetString(2),
                                CustomerType = reader.IsDBNull(3) ? "" : reader.GetString(3)
                            });
                        }
                    }
                }
            }
            return customers;
        }
        public static double CalculateTotalBillAll(string orderId)
        {
            double total = 0;
            using (var connection = new SqliteConnection("Data Source=CabanaDatabase.db"))
            {
                connection.Open();
                
                string query = "SELECT SUM(Price * Quantity) FROM FoodOrders WHERE OrderID = @orderId";

                using (var cmd = new SqliteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    

                    object result = cmd.ExecuteScalar();

                    

                    if (result != DBNull.Value)
                    {
                        total = Convert.ToDouble(result);
                    }
                }
            }
            return total;
        }

        public static void AddRestaurantInvoice(RestaurantBillingRecord record)
        {
            // Add to the in-memory ObservableCollection for UI updates
            RestaurantInvoices.Add(record);

            // Perform a targeted SQL Insert
            using (var connection = new SqliteConnection(DbConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO RestaurantInvoices (OrderID, TableNumber, CustomerName, Date, States, TotalBill) " +
                               "VALUES (@id, @table, @cust, @date, @states, @bill)";

                using (var cmd = new SqliteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@id", record.OrderID);
                    cmd.Parameters.AddWithValue("@table", record.TableNumber);
                    cmd.Parameters.AddWithValue("@cust", record.CustomerName ?? "");
                    cmd.Parameters.AddWithValue("@date", record.Date ?? "");
                    cmd.Parameters.AddWithValue("@states", record.States ?? "");
                    cmd.Parameters.AddWithValue("@bill", record.TotalBill ?? "Rs. 0.00");

                    cmd.ExecuteNonQuery();
                }
            }
        }




        public static (double RoomRevenue, double RestaurantRevenue) GetTotalRevenueFromTable(int month, int year)
        {
            double roomRev = 0, restRev = 0;

            // Open connection to your existing database
            using (var connection = new SqliteConnection(DbConnectionString))
            {
                connection.Open();

                // Select the totals for the requested month and year
                string query = "SELECT totalRoomRevenue, totalRestaurantRevenue FROM TotalRevenue WHERE month = @m AND year = @y";

                using (var cmd = new SqliteCommand(query, connection))
                {
                    
                    cmd.Parameters.AddWithValue("@m", System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month));
                    cmd.Parameters.AddWithValue("@y", year);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            roomRev = reader.GetDouble(0);
                            restRev = reader.GetDouble(1);
                        }
                    }
                }
            }
            return (roomRev, restRev);
        }

        public static List<RevenueRecord> LoadAllRevenueHistory()
        {
            var history = new List<RevenueRecord>();

            using (var connection = new SqliteConnection("Data Source=CabanaDatabase.db"))
            {
                connection.Open();
                string query = "SELECT month, year, totalRoomRevenue, totalRestaurantRevenue FROM TotalRevenue ORDER BY year, month";

                using (var cmd = new SqliteCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        history.Add(new RevenueRecord
                        {
                            Month = reader.GetString(0),
                            Year = reader.GetInt32(1),
                            RoomRevenue = reader.GetDouble(2),
                            RestaurantRevenue = reader.GetDouble(3)
                        });
                    }
                }
            }
            return history;
        }


        public static void SyncRoomPayment(double roomAmount)
        {
            DateTime now = DateTime.Now;
            var current = GetTotalRevenueFromTable(now.Month, now.Year);
            UpdateTotalRevenueTable(current.RoomRevenue + roomAmount, current.RestaurantRevenue);
        }

        
        public static void SyncRestaurantPayment(double restAmount)
        {
            DateTime now = DateTime.Now;
            var current = GetTotalRevenueFromTable(now.Month, now.Year);
            UpdateTotalRevenueTable(current.RoomRevenue, current.RestaurantRevenue + restAmount);
        }

        public static void CompleteFoodOrder(string orderId)
        {
            using (var connection = new SqliteConnection("Data Source=CabanaDatabase.db"))
            {
                connection.Open();

                
                string query = "UPDATE FoodOrders SET States = 'Complete' WHERE OrderID = @orderId AND States = 'Ongoing'";

                using (var cmd = new SqliteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static void UpdateTotalRevenueTable(double roomRev, double restRev)
        {
            DateTime now = DateTime.Now;

            using (var connection = new SqliteConnection(DbConnectionString))
            {
                connection.Open();
                string upsertQuery = @"
            INSERT OR REPLACE INTO TotalRevenue (revID, month, year, totalRoomRevenue, totalRestaurantRevenue)
            VALUES (@id, @m, @y, @rr, @rest);";

                using (var cmd = new SqliteCommand(upsertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@id",IdGenerator.GenerateRevID());
                    cmd.Parameters.AddWithValue("@m", now.ToString("MMMM")); 
                    cmd.Parameters.AddWithValue("@y", now.Year);             
                    cmd.Parameters.AddWithValue("@rr", roomRev);
                    cmd.Parameters.AddWithValue("@rest", restRev);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateRoomRevenue(double roomAmountToAdd)
        {
            DateTime now = DateTime.Now;
            string month = now.ToString("MMMM");
            int year = now.Year;

            using (var connection = new SqliteConnection(DbConnectionString))
            {
                connection.Open();

                
                string upsertQuery = @"
            INSERT INTO TotalRevenue (revID, month, year, totalRoomRevenue, totalRestaurantRevenue)
            VALUES (@id, @m, @y, @rr, 0)
            ON CONFLICT(revID) DO UPDATE SET 
            totalRoomRevenue = totalRoomRevenue + @rr;";

                

                using (var cmd = new SqliteCommand(upsertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@id", IdGenerator.GenerateRevID());
                    cmd.Parameters.AddWithValue("@m", month);
                    cmd.Parameters.AddWithValue("@y", year);
                    cmd.Parameters.AddWithValue("@rr", roomAmountToAdd);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateRestaurantRevenue(double restAmountToAdd)
        {
            DateTime now = DateTime.Now;
            string month = now.ToString("MMMM");
            int year = now.Year;

            using (var connection = new SqliteConnection(DbConnectionString))
            {
                connection.Open();

                
                string upsertQuery = @"
            INSERT INTO TotalRevenue (revID, month, year, totalRoomRevenue, totalRestaurantRevenue)
            VALUES (@id, @m, @y, 0, @rest)
            ON CONFLICT(revID) DO UPDATE SET 
            totalRestaurantRevenue = totalRestaurantRevenue + @rest;";

                using (var cmd = new SqliteCommand(upsertQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@id", IdGenerator.GenerateRevID());
                    cmd.Parameters.AddWithValue("@m", month);
                    cmd.Parameters.AddWithValue("@y", year);
                    cmd.Parameters.AddWithValue("@rest", restAmountToAdd);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        

        public static (double TotalRevenue, double TotalDues, int Available, int Occupied) GetRestaurantMetrics()
        {
            double revenue = 0;
            double dues = 0;
            int occupied = 0;
            int available = 0;
            string todayStr = DateTime.Now.ToString("yyyy.MM.dd");
            double amount;

            foreach (var record in RestaurantInvoices)
            {
                if (record.Date == todayStr)
                {
                    amount = CleanAndParseAmount(record.TotalBill);
                    if (record.States?.Equals("Complete", StringComparison.OrdinalIgnoreCase) == true)
                        revenue += amount;
                    else if (record.States?.Equals("Ongoing", StringComparison.OrdinalIgnoreCase) == true)
                        dues += amount;
                }
            }

            foreach (var table in MasterTables)
            {
                if (table != null)
                {
                    if (table.States?.Equals("Ongoing", StringComparison.OrdinalIgnoreCase) == true) occupied++;
                    else if (table.States?.Equals("Complete", StringComparison.OrdinalIgnoreCase) == true) available++;
                }
            }
            return (revenue, dues, available, occupied);
        }

        public static (double TotalRevenue, double TotalDues, int Available, int Occupied) GetRoomMetrics()
        {
            double revenue = 0;
            double dues = 0;
            int occupied = 0;
            int available = 0;
            string todayStr = DateTime.Now.ToString("yyyy.MM.dd");
            double amount;

            foreach (var record in RoomInvoices)
            {
                if (record.Date == todayStr)
                {
                    amount = CleanAndParseAmount(record.TotalDue);
                    if (record.States?.Equals("CheckedOut", StringComparison.OrdinalIgnoreCase) == true)
                        revenue += amount;
                    else if (record.States?.Equals("CheckedIn", StringComparison.OrdinalIgnoreCase) == true)
                        dues += amount;
                }
            }

            foreach (var suite in MasterSuites)
            {
                if (suite != null)
                {
                    if (suite.States?.Equals("CheckedIn", StringComparison.OrdinalIgnoreCase) == true) occupied++;
                    else if (suite.States?.Equals("CheckedOut", StringComparison.OrdinalIgnoreCase) == true) available++;
                }
            }
            return (revenue, dues, available, occupied);
        }

        private static double CleanAndParseAmount(string? rawAmount)
        {
            if (string.IsNullOrWhiteSpace(rawAmount)) return 0;
            try
            {
                string clean = rawAmount.Replace("Rs.", "").Replace(" ", "").Replace(",", "").Trim();
                return double.TryParse(clean, out double result) ? result : 0;
            }
            catch
            {
                return 0;
            }
        }

        public static (double StdCouple, double StdFamily, double Superior, double Deluxe) GetRoomCategoryDistribution()
        {
            double stdCouple = 0; double stdFamily = 0; double superior = 0; double deluxe = 0;

            foreach (var suite in MasterSuites)
            {
                if (suite != null &&
                    (suite.States?.Equals("CheckedIn", StringComparison.OrdinalIgnoreCase) == true ||
                     suite.States?.Equals("UnderCleaning", StringComparison.OrdinalIgnoreCase) == true))
                {
                    if (suite.Tier?.Equals("Standard", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        if (suite.Type?.Equals("Couple", StringComparison.OrdinalIgnoreCase) == true) stdCouple++;
                        else if (suite.Type?.Equals("Family", StringComparison.OrdinalIgnoreCase) == true) stdFamily++;
                    }
                    else if (suite.Tier?.Equals("Superior", StringComparison.OrdinalIgnoreCase) == true) superior++;
                    else if (suite.Tier?.Equals("Deluxe", StringComparison.OrdinalIgnoreCase) == true) deluxe++;
                }
            }
            return (stdCouple, stdFamily, superior, deluxe);
        }

        public static (double DlxFamily, double StdFamily, double DlxCouple, double StdCouple) GetRestaurantCategoryDistribution()
        {
            double dlxFamily = 0; double stdFamily = 0; double dlxCouple = 0; double stdCouple = 0;

            foreach (var table in MasterTables)
            {
                if (table != null && table.States?.Equals("Ongoing", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (table.Zone?.Equals("Deluxe", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        if (table.Type?.Equals("Family", StringComparison.OrdinalIgnoreCase) == true) dlxFamily++;
                        else if (table.Type?.Equals("Couple", StringComparison.OrdinalIgnoreCase) == true) dlxCouple++;
                    }
                    else if (table.Zone?.Equals("Standard", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        if (table.Type?.Equals("Family", StringComparison.OrdinalIgnoreCase) == true) stdFamily++;
                        else if (table.Type?.Equals("Couple", StringComparison.OrdinalIgnoreCase) == true) stdCouple++;
                    }
                }
            }
            return (dlxFamily, stdFamily, dlxCouple, stdCouple);
        }

        
        public static double GetMonthlyRevenue(int month, string serviceType)
        {
            double total = 0;
            string[] parts;
            int parsedMonth;

            if (serviceType.Equals("Restaurant", StringComparison.OrdinalIgnoreCase))
            {
                foreach (var record in RestaurantInvoices)
                {
                    
                    if (record.States?.Equals("Complete", StringComparison.OrdinalIgnoreCase) != true)
                        continue;

                    if (!string.IsNullOrEmpty(record.Date) && record.Date.Length >= 7)
                    {
                        parts = record.Date.Split('.');
                        if (parts.Length >= 2 && int.TryParse(parts[1], out parsedMonth) && parsedMonth == month)
                        {
                            total += CleanAndParseAmount(record.TotalBill);
                        }
                    }
                }
            }
            else 
            {
                foreach (var record in RoomInvoices)
                {
                    
                    if (record.States?.Equals("CheckedOut", StringComparison.OrdinalIgnoreCase) != true)
                        continue;

                    if (!string.IsNullOrEmpty(record.Date) && record.Date.Length >= 7)
                    {
                        parts = record.Date.Split('.');
                        if (parts.Length >= 2 && int.TryParse(parts[1], out parsedMonth) && parsedMonth == month)
                        {
                            total += CleanAndParseAmount(record.TotalDue);
                        }
                    }
                }
            }
            return total;
        }
    }
}