📌 Project Overview

Stock ROI Analyzer is a C# Windows Forms application designed to calculate and analyze the Return on Investment (ROI) of stocks from five different companies.
The application allows users to enter investment details, calculate profit or loss after applicable taxes, compare multiple companies, and identify the company with the highest ROI.

🛠️ Technologies & Environment
Language: C#
Framework: .NET Windows Forms
IDE: Visual Studio
Programming Approach: Object-Oriented Programming (OOP)
UI Components
DataGridView
ComboBox
RadioButtons
CheckedListBox
MenuStrip

✨ Key Features
📊 ROI Calculation
The application automatically calculates:
Gross Profit/Loss
Company Tax
Government Tax
Total Tax
Net Profit/Loss
ROI Percentage

🏢 Company-Specific Tax Rates
Company	Tax Rate
Company 1	5%
Company 2	5.5%
Company 3	7.5%
Company 4	6.3%
Company 5	9.9%
🧾 Government Tax

Government tax is calculated according to the investor's status:
Investor Status	Government Tax
Filer	2%
Non-Filer	4%
🔄 Company Comparison

Users can select multiple companies and compare their investment results side-by-side.
The application automatically identifies and highlights the company with the highest calculated ROI.

✅ Input Validation
The application validates user input and handles situations such as:
Empty fields
Invalid characters
Negative numbers
Invalid numerical values
Zero or invalid investment amounts
Break-even situations
Loss scenarios
If an invalid value is entered, the user receives an appropriate message and is asked to enter a valid value.

📜 ROI History
The application maintains a history of ROI calculations performed during the current session, allowing users to review previous calculations.

🎉 Profit Notification
When an investment results in a profit, the application displays a congratulations popup.

📋 Interactive Menu
The application includes an easy-to-use menu with:
File
Investment
View
Help

🚀 How to Run
Clone or download this repository.
Open StockROIAnalyzer3.slnx in Visual Studio.
Build the solution using Ctrl + Shift + B.
Run the application using F5 or click the green Start button in Visual Studio.

💻 How to Use
1. Select a Company
Select a company from the dropdown menu.
2. Select Investor Status

Choose one of the following:
Filer — 2% government tax
Non-Filer — 4% government tax
3. Enter Investment Details

Enter:
Investment Amount
Purchase Price
Selling Price
4. Calculate ROI
Click the CALCULATE ROI button.
The application will display the complete investment result, including profit/loss, taxes, net profit/loss, and ROI percentage.

5. Compare Companies
Click the COMPARE button to select multiple companies.
The application will display the selected companies side-by-side and highlight the company with the highest calculated ROI.

🎯 Project Objective
The objective of Stock ROI Analyzer is to provide a simple and interactive tool for analyzing stock investment returns while considering both company-specific and government taxes.
The project demonstrates the practical use of C#, Windows Forms, Object-Oriented Programming, data validation, financial calculations, and interactive user interface components.

👨‍💻 Built With
C# • .NET Windows Forms • Visual Studio • OOP
