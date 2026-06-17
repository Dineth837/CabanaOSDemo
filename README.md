# CabanaOS

**CabanaOS** is a comprehensive Restaurant Management System developed to streamline operations for restaurant staff and management. Built with C# and WPF, it provides an intuitive interface for managing billing, inventory, staff, and customer reservations.

## Project Status
This project is currently in the **development phase**. We are actively working on the deployment pipeline; therefore, the system is primarily executed directly from the source code via Visual Studio rather than a standalone `.exe` installer.

## Features
* **Billing & Invoicing**: Efficiently calculate bills and manage ongoing and completed orders.
* **Inventory Management**: Track food items, pricing, and quantities.
* **Order Tracking**: Manage food orders with status updates (Ongoing/Complete).
* **Staff Management**: Handle administrative access and employee authentication.
* **Data Visualization**: Integrated charts and summaries for sales performance.

## Technology Stack
* **Language**: C#
* **Framework**: WPF (.NET)
* **Database**: SQLite
* **IDE**: Visual Studio
* **Reporting**: QuestPDF
* **QR Codes**: QRCoder

## How to Run the System
Since the installer is currently under construction, follow these steps to run the application:

1.  **Clone/Download** the repository to your local machine.
2.  Open the solution file (`.sln`) in **Visual Studio**.
3.  Ensure the target configuration is set to **Debug** (or **Release**) recommended **Debug**.
4.  Press **F5** or click the **Start** button in Visual Studio to build and launch the application.
5.  The application will automatically connect to the local `CabanaDatabase.db` file.

## Project Structure
* `CabanaOSDemo`: The core application source code.
* `CabanaDatabase.db`: SQLite database managing all persistent data.
* `CabanaOSInstaller`: Deployment project (in progress) for generating the `.msi` installer. Since this is under construction, the application is currently run directly from Visual Studio. no available installer yet.
## Usage
* **Login**: Use authorized staff credentials to access the management dashboard.
* Default credentials for testing:
  * Username: `admin`
  * Password: `admin`
	* for Employee login and Manager login, you can use the same credentials for testing purposes.
* **Billing**: Navigate to the billing module to create new orders and generate invoices.
* **Dashboard**: Monitor sales analytics and overall restaurant performance.

## License
This project is part of a university group project and is intended for educational purposes.