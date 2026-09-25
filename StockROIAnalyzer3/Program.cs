
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace StockROIAnalyzer
{
    // =========================================================
    // PROGRAM ENTRY POINT
    // =========================================================

    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());
        }
    }


    // =========================================================
    // MAIN FORM
    // =========================================================

    public class Form1 : Form
    {
        // =========================================================
        // COLORS
        // =========================================================

        private readonly Color Dark =
            Color.FromArgb(23, 23, 23);

        private readonly Color Gray =
            Color.FromArgb(68, 68, 68);

        private readonly Color Red =
            Color.FromArgb(218, 0, 55);

        private readonly Color Light =
            Color.FromArgb(237, 237, 237);

        private readonly Color White =
            Color.White;

        private readonly Color Green =
            Color.FromArgb(20, 130, 80);

        private readonly Color Orange =
            Color.DarkOrange;


        // =========================================================
        // CONTROLS
        // =========================================================

        private ComboBox cmbCompany;

        private RadioButton rdoFiler;
        private RadioButton rdoNonFiler;

        private TextBox txtInvestment;
        private TextBox txtPurchasePrice;
        private TextBox txtSellingPrice;

        private Button btnCalculate;
        private Button btnClear;
        private Button btnCompare;
        private Button btnExit;

        private Label lblGrossProfit;
        private Label lblCompanyTax;
        private Label lblGovernmentTax;
        private Label lblTotalTax;
        private Label lblNetProfit;
        private Label lblROI;
        private Label lblStatus;

        private Label lblSelectedCompany;
        private Label lblTaxRate;

        private Panel pnlResults;

        private bool allowClosing = false;


        // =========================================================
        // COMPANY DATA
        // =========================================================

        private Dictionary<string, decimal> companyTaxes =
            new Dictionary<string, decimal>()
            {
                { "Company 1", 0.05m },
                { "Company 2", 0.055m },
                { "Company 3", 0.075m },
                { "Company 4", 0.063m },
                { "Company 5", 0.099m }
            };


        // =========================================================
        // ROI HISTORY
        // =========================================================

        private List<ROIHistory> roiHistory =
            new List<ROIHistory>();


        // =========================================================
        // CONSTRUCTOR
        // =========================================================

        public Form1()
        {
            InitializeForm();
            CreateMenu();
            CreateInterface();
        }


        // =========================================================
        // FORM SETTINGS
        // =========================================================

        private void InitializeForm()
        {
            this.Text = "Stock ROI Analyzer";

            this.StartPosition =
                FormStartPosition.CenterScreen;

            this.Size =
                new Size(1180, 760);

            this.MinimumSize =
                new Size(1050, 680);

            this.BackColor =
                Light;

            this.Font =
                new Font("Segoe UI", 10F);

            this.FormClosing +=
                Form1_FormClosing;
        }


        // =========================================================
        // MENU
        // =========================================================

        private void CreateMenu()
        {
            MenuStrip menu =
                new MenuStrip();

            menu.BackColor =
                Dark;

            menu.ForeColor =
                White;

            menu.Font =
                new Font(
                    "Segoe UI",
                    10F);


            // =====================================================
            // FILE MENU
            // =====================================================

            ToolStripMenuItem fileMenu =
                new ToolStripMenuItem("File");

            ToolStripMenuItem newCalculation =
                new ToolStripMenuItem(
                    "New Calculation");

            ToolStripMenuItem clearMenu =
                new ToolStripMenuItem(
                    "Clear");

            ToolStripMenuItem exitMenu =
                new ToolStripMenuItem(
                    "Exit");


            newCalculation.Click +=
                (s, e) =>
                {
                    ClearForm();
                };


            clearMenu.Click +=
                (s, e) =>
                {
                    ClearForm();
                };


            exitMenu.Click +=
                (s, e) =>
                {
                    ExitApplication();
                };


            fileMenu.DropDownItems.Add(
                newCalculation);

            fileMenu.DropDownItems.Add(
                clearMenu);

            fileMenu.DropDownItems.Add(
                new ToolStripSeparator());

            fileMenu.DropDownItems.Add(
                exitMenu);


            // =====================================================
            // INVESTMENT MENU
            // =====================================================

            ToolStripMenuItem investmentMenu =
                new ToolStripMenuItem(
                    "Investment");

            ToolStripMenuItem calculateMenu =
                new ToolStripMenuItem(
                    "Calculate ROI");

            ToolStripMenuItem compareMenu =
                new ToolStripMenuItem(
                    "Compare Companies");


            calculateMenu.Click +=
                (s, e) =>
                {
                    CalculateROI();
                };


            compareMenu.Click +=
                (s, e) =>
                {
                    CompareCompanies();
                };


            investmentMenu.DropDownItems.Add(
                calculateMenu);

            investmentMenu.DropDownItems.Add(
                compareMenu);


            // =====================================================
            // VIEW MENU
            // =====================================================

            ToolStripMenuItem viewMenu =
                new ToolStripMenuItem(
                    "View");

            ToolStripMenuItem resultsMenu =
                new ToolStripMenuItem(
                    "Results");

            ToolStripMenuItem companyInfoMenu =
                new ToolStripMenuItem(
                    "Company Information");

            ToolStripMenuItem historyMenu =
                new ToolStripMenuItem(
                    "ROI History");


            resultsMenu.Click +=
                (s, e) =>
                {
                    if (pnlResults != null)
                    {
                        pnlResults.Focus();
                    }
                };


            companyInfoMenu.Click +=
                (s, e) =>
                {
                    ShowCompanyInformation();
                };


            historyMenu.Click +=
                (s, e) =>
                {
                    ShowHistory();
                };


            viewMenu.DropDownItems.Add(
                resultsMenu);

            viewMenu.DropDownItems.Add(
                companyInfoMenu);

            viewMenu.DropDownItems.Add(
                historyMenu);


            // =====================================================
            // HELP MENU
            // =====================================================

            ToolStripMenuItem helpMenu =
                new ToolStripMenuItem(
                    "Help");

            ToolStripMenuItem aboutMenu =
                new ToolStripMenuItem(
                    "About");


            aboutMenu.Click +=
                (s, e) =>
                {
                    ShowAbout();
                };


            helpMenu.DropDownItems.Add(
                aboutMenu);


            // =====================================================
            // ADD MENUS
            // =====================================================

            menu.Items.Add(fileMenu);
            menu.Items.Add(investmentMenu);
            menu.Items.Add(viewMenu);
            menu.Items.Add(helpMenu);


            this.MainMenuStrip =
                menu;

            this.Controls.Add(menu);
        }


        // =========================================================
        // MAIN INTERFACE
        // =========================================================

        private void CreateInterface()
        {
            // =====================================================
            // HEADER
            // =====================================================

            Panel header =
                new Panel();

            header.BackColor =
                Dark;

            header.Location =
                new Point(0, 24);

            header.Size =
                new Size(
                    this.ClientSize.Width,
                    100);

            header.Anchor =
                AnchorStyles.Top |
                AnchorStyles.Left |
                AnchorStyles.Right;


            Label title =
                new Label();

            title.Text =
                "STOCK ROI ANALYZER";

            title.ForeColor =
                White;

            title.Font =
                new Font(
                    "Segoe UI",
                    24F,
                    FontStyle.Bold);

            title.Location =
                new Point(35, 18);

            title.AutoSize =
                true;


            Label subtitle =
                new Label();

            subtitle.Text =
                "Investment & Return Analysis";

            subtitle.ForeColor =
                Color.LightGray;

            subtitle.Font =
                new Font(
                    "Segoe UI",
                    11F);

            subtitle.Location =
                new Point(38, 60);

            subtitle.AutoSize =
                true;


            header.Controls.Add(title);
            header.Controls.Add(subtitle);

            this.Controls.Add(header);


            // =====================================================
            // INPUT CARD
            // =====================================================

            Panel inputCard =
                CreateCard(
                    new Point(30, 145),
                    new Size(510, 430));


            Label inputTitle =
                CreateSectionTitle(
                    "INVESTMENT DETAILS",
                    new Point(25, 20));

            inputCard.Controls.Add(
                inputTitle);


            // =====================================================
            // COMPANY
            // =====================================================

            Label lblCompany =
                CreateLabel(
                    "Company",
                    new Point(25, 75));


            cmbCompany =
                new ComboBox();

            cmbCompany.DropDownStyle =
                ComboBoxStyle.DropDownList;

            cmbCompany.Font =
                new Font(
                    "Segoe UI",
                    10F);

            cmbCompany.Location =
                new Point(190, 70);

            cmbCompany.Size =
                new Size(280, 30);


            foreach (
                string company
                in companyTaxes.Keys)
            {
                cmbCompany.Items.Add(
                    company);
            }


            cmbCompany.SelectedIndexChanged +=
                CmbCompany_SelectedIndexChanged;


            inputCard.Controls.Add(
                lblCompany);

            inputCard.Controls.Add(
                cmbCompany);


            // =====================================================
            // TAX RATE
            // =====================================================

            lblTaxRate =
                CreateLabel(
                    "Tax Rate: —",
                    new Point(190, 105));

            lblTaxRate.ForeColor =
                Red;

            lblTaxRate.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);


            inputCard.Controls.Add(
                lblTaxRate);


            // =====================================================
            // INVESTOR STATUS
            // =====================================================

            Label lblInvestorStatus =
                CreateLabel(
                    "Investor Status",
                    new Point(25, 145));


            rdoFiler =
                new RadioButton();

            rdoFiler.Text =
                "Filer (2%)";

            rdoFiler.Location =
                new Point(190, 142);

            rdoFiler.AutoSize =
                true;


            rdoNonFiler =
                new RadioButton();

            rdoNonFiler.Text =
                "Non-Filer (4%)";

            rdoNonFiler.Location =
                new Point(295, 142);

            rdoNonFiler.AutoSize =
                true;


            inputCard.Controls.Add(
                lblInvestorStatus);

            inputCard.Controls.Add(
                rdoFiler);

            inputCard.Controls.Add(
                rdoNonFiler);


            // =====================================================
            // INVESTMENT AMOUNT
            // =====================================================

            Label lblInvestment =
                CreateLabel(
                    "Investment Amount",
                    new Point(25, 195));


            txtInvestment =
                CreateTextBox(
                    new Point(190, 190));


            inputCard.Controls.Add(
                lblInvestment);

            inputCard.Controls.Add(
                txtInvestment);


            // =====================================================
            // PURCHASE PRICE
            // =====================================================

            Label lblPurchase =
                CreateLabel(
                    "Purchase Price",
                    new Point(25, 245));


            txtPurchasePrice =
                CreateTextBox(
                    new Point(190, 240));


            inputCard.Controls.Add(
                lblPurchase);

            inputCard.Controls.Add(
                txtPurchasePrice);


            // =====================================================
            // SELLING PRICE
            // =====================================================

            Label lblSelling =
                CreateLabel(
                    "Selling Price",
                    new Point(25, 295));


            txtSellingPrice =
                CreateTextBox(
                    new Point(190, 290));


            inputCard.Controls.Add(
                lblSelling);

            inputCard.Controls.Add(
                txtSellingPrice);


            // =====================================================
            // CALCULATE BUTTON
            // =====================================================

            btnCalculate =
                CreateButton(
                    "CALCULATE ROI",
                    new Point(25, 350),
                    new Size(445, 45),
                    Red);


            btnCalculate.Click +=
                (s, e) =>
                {
                    CalculateROI();
                };


            inputCard.Controls.Add(
                btnCalculate);


            this.Controls.Add(
                inputCard);


            // =====================================================
            // RESULTS CARD
            // =====================================================

            pnlResults =
                CreateCard(
                    new Point(565, 145),
                    new Size(570, 430));


            Label resultTitle =
                CreateSectionTitle(
                    "INVESTMENT SUMMARY",
                    new Point(25, 20));


            pnlResults.Controls.Add(
                resultTitle);


            // =====================================================
            // COMPANY RESULT
            // =====================================================

            lblSelectedCompany =
                CreateResultLabel(
                    "Company: —",
                    new Point(25, 70));


            pnlResults.Controls.Add(
                lblSelectedCompany);


            // =====================================================
            // GROSS PROFIT
            // =====================================================

            lblGrossProfit =
                CreateResultLabel(
                    "Gross Profit/Loss: —",
                    new Point(25, 115));


            pnlResults.Controls.Add(
                lblGrossProfit);


            // =====================================================
            // COMPANY TAX
            // =====================================================

            lblCompanyTax =
                CreateResultLabel(
                    "Company Tax: —",
                    new Point(25, 150));


            pnlResults.Controls.Add(
                lblCompanyTax);


            // =====================================================
            // GOVERNMENT TAX
            // =====================================================

            lblGovernmentTax =
                CreateResultLabel(
                    "Government Tax: —",
                    new Point(25, 185));


            pnlResults.Controls.Add(
                lblGovernmentTax);


            // =====================================================
            // TOTAL TAX
            // =====================================================

            lblTotalTax =
                CreateResultLabel(
                    "Total Tax: —",
                    new Point(25, 220));


            pnlResults.Controls.Add(
                lblTotalTax);


            // =====================================================
            // NET PROFIT
            // =====================================================

            lblNetProfit =
                CreateResultLabel(
                    "Net Profit/Loss: —",
                    new Point(25, 255));


            pnlResults.Controls.Add(
                lblNetProfit);


            // =====================================================
            // ROI CAPTION
            // =====================================================

            Label roiCaption =
                new Label();

            roiCaption.Text =
                "RETURN ON INVESTMENT";

            roiCaption.ForeColor =
                Gray;

            roiCaption.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);

            roiCaption.Location =
                new Point(315, 75);

            roiCaption.AutoSize =
                true;


            pnlResults.Controls.Add(
                roiCaption);


            // =====================================================
            // ROI VALUE
            // =====================================================

            lblROI =
                new Label();

            lblROI.Text =
                "—";

            lblROI.ForeColor =
                Red;

            lblROI.Font =
                new Font(
                    "Segoe UI",
                    30F,
                    FontStyle.Bold);

            lblROI.Location =
                new Point(310, 100);

            lblROI.AutoSize =
                true;


            pnlResults.Controls.Add(
                lblROI);


            // =====================================================
            // STATUS CAPTION
            // =====================================================

            Label statusCaption =
                new Label();

            statusCaption.Text =
                "STATUS";

            statusCaption.ForeColor =
                Gray;

            statusCaption.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);

            statusCaption.Location =
                new Point(315, 180);

            statusCaption.AutoSize =
                true;


            pnlResults.Controls.Add(
                statusCaption);


            // =====================================================
            // STATUS
            // =====================================================

            lblStatus =
                new Label();

            lblStatus.Text =
                "WAITING";

            lblStatus.ForeColor =
                Gray;

            lblStatus.Font =
                new Font(
                    "Segoe UI",
                    18F,
                    FontStyle.Bold);

            lblStatus.Location =
                new Point(310, 205);

            lblStatus.AutoSize =
                true;


            pnlResults.Controls.Add(
                lblStatus);


            // =====================================================
            // EXPLANATION
            // =====================================================

            Label explanation =
                new Label();

            explanation.Text =
                "Government tax is calculated at 2% for Filer\n" +
                "or 4% for Non-Filer when a profit is made.\n\n" +
                "Company tax applies only when there is profit.";

            explanation.ForeColor =
                Gray;

            explanation.Font =
                new Font(
                    "Segoe UI",
                    9F);

            explanation.Location =
                new Point(310, 260);

            explanation.AutoSize =
                true;


            pnlResults.Controls.Add(
                explanation);


            // =====================================================
            // CLEAR BUTTON
            // =====================================================

            btnClear =
                CreateButton(
                    "CLEAR",
                    new Point(25, 345),
                    new Size(135, 45),
                    Gray);


            btnClear.Click +=
                (s, e) =>
                {
                    ClearForm();
                };


            pnlResults.Controls.Add(
                btnClear);


            // =====================================================
            // COMPARE BUTTON
            // =====================================================

            btnCompare =
                CreateButton(
                    "COMPARE",
                    new Point(175, 345),
                    new Size(135, 45),
                    Dark);


            btnCompare.Click +=
                (s, e) =>
                {
                    CompareCompanies();
                };


            pnlResults.Controls.Add(
                btnCompare);


            // =====================================================
            // EXIT BUTTON
            // =====================================================

            btnExit =
                CreateButton(
                    "EXIT",
                    new Point(325, 345),
                    new Size(135, 45),
                    Red);


            btnExit.Click +=
                (s, e) =>
                {
                    ExitApplication();
                };


            pnlResults.Controls.Add(
                btnExit);


            this.Controls.Add(
                pnlResults);


            // =====================================================
            // COMPARISON AREA
            // =====================================================

            Panel comparisonCard =
                CreateCard(
                    new Point(30, 595),
                    new Size(1105, 100));


            comparisonCard.Anchor =
                AnchorStyles.Left |
                AnchorStyles.Right |
                AnchorStyles.Bottom;


            Label comparisonTitle =
                CreateSectionTitle(
                    "COMPANY COMPARISON",
                    new Point(20, 15));


            comparisonCard.Controls.Add(
                comparisonTitle);


            Label comparisonText =
                new Label();

            comparisonText.Text =
                "Click Compare to select the companies you want to compare.";

            comparisonText.ForeColor =
                Gray;

            comparisonText.Location =
                new Point(20, 55);

            comparisonText.AutoSize =
                true;


            comparisonCard.Controls.Add(
                comparisonText);


            this.Controls.Add(
                comparisonCard);
        }


        // =========================================================
        // CREATE CARD
        // =========================================================

        private Panel CreateCard(
            Point location,
            Size size)
        {
            Panel panel =
                new Panel();

            panel.BackColor =
                White;

            panel.Location =
                location;

            panel.Size =
                size;

            return panel;
        }


        // =========================================================
        // CREATE SECTION TITLE
        // =========================================================

        private Label CreateSectionTitle(
            string text,
            Point location)
        {
            Label label =
                new Label();

            label.Text =
                text;

            label.ForeColor =
                Dark;

            label.Font =
                new Font(
                    "Segoe UI",
                    13F,
                    FontStyle.Bold);

            label.Location =
                location;

            label.AutoSize =
                true;

            return label;
        }


        // =========================================================
        // CREATE LABEL
        // =========================================================

        private Label CreateLabel(
            string text,
            Point location)
        {
            Label label =
                new Label();

            label.Text =
                text;

            label.ForeColor =
                Gray;

            label.Font =
                new Font(
                    "Segoe UI",
                    10F);

            label.Location =
                location;

            label.AutoSize =
                true;

            return label;
        }


        // =========================================================
        // CREATE RESULT LABEL
        // =========================================================

        private Label CreateResultLabel(
            string text,
            Point location)
        {
            Label label =
                new Label();

            label.Text =
                text;

            label.ForeColor =
                Gray;

            label.Font =
                new Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Bold);

            label.Location =
                location;

            label.AutoSize =
                true;

            return label;
        }


        // =========================================================
        // CREATE TEXTBOX
        // =========================================================

        private TextBox CreateTextBox(
            Point location)
        {
            TextBox textBox =
                new TextBox();

            textBox.Location =
                location;

            textBox.Size =
                new Size(280, 30);

            textBox.Font =
                new Font(
                    "Segoe UI",
                    10F);

            textBox.BorderStyle =
                BorderStyle.FixedSingle;

            return textBox;
        }


        // =========================================================
        // CREATE BUTTON
        // =========================================================

        private Button CreateButton(
            string text,
            Point location,
            Size size,
            Color backColor)
        {
            Button button =
                new Button();

            button.Text =
                text;

            button.Location =
                location;

            button.Size =
                size;

            button.BackColor =
                backColor;

            button.ForeColor =
                White;

            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize =
                0;

            button.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);

            button.Cursor =
                Cursors.Hand;

            return button;
        }


        // =========================================================
        // COMPANY SELECTION
        // =========================================================

        private void CmbCompany_SelectedIndexChanged(
            object sender,
            EventArgs e)
        {
            if (cmbCompany.SelectedItem == null)
            {
                lblTaxRate.Text =
                    "Tax Rate: —";

                return;
            }


            string company =
                cmbCompany.SelectedItem.ToString();


            decimal taxRate =
                companyTaxes[company];


            lblTaxRate.Text =
                "Tax Rate: " +
                (taxRate * 100)
                    .ToString("0.0") +
                "%";
        }


        // =========================================================
        // VALIDATE DECIMAL INPUT
        // =========================================================

        private bool TryGetAmount(
            TextBox textBox,
            string fieldName,
            bool mustBeGreaterThanZero,
            out decimal value)
        {
            value = 0;


            string input =
                textBox.Text.Trim();


            // -----------------------------------------------------
            // EMPTY INPUT
            // -----------------------------------------------------

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show(
                    fieldName +
                    " cannot be empty.",
                    "Input Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                textBox.Focus();

                return false;
            }


            // -----------------------------------------------------
            // INVALID NUMBER
            // -----------------------------------------------------

            if (!decimal.TryParse(
                input,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out value))
            {
                MessageBox.Show(
                    "Please enter a valid numeric value for " +
                    fieldName + ".",
                    "Invalid Input",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                textBox.Focus();

                textBox.SelectAll();

                return false;
            }


            // -----------------------------------------------------
            // NEGATIVE VALUE
            // -----------------------------------------------------

            if (value < 0)
            {
                MessageBox.Show(
                    fieldName +
                    " cannot be negative.",
                    "Invalid Value",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                textBox.Focus();

                textBox.SelectAll();

                return false;
            }


            // -----------------------------------------------------
            // ZERO CHECK
            // -----------------------------------------------------

            if (mustBeGreaterThanZero &&
                value <= 0)
            {
                MessageBox.Show(
                    fieldName +
                    " must be greater than zero.",
                    "Invalid Value",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                textBox.Focus();

                textBox.SelectAll();

                return false;
            }


            return true;
        }


        // =========================================================
        // VALIDATE INVESTMENT RELATIONSHIPS
        // =========================================================

        private bool ValidateInvestmentRelationships(
            decimal investment,
            decimal purchasePrice,
            decimal sellingPrice)
        {
            // -----------------------------------------------------
            // PURCHASE PRICE CANNOT BE GREATER THAN INVESTMENT
            // -----------------------------------------------------

            if (purchasePrice > investment)
            {
                MessageBox.Show(
                    "Purchase Price cannot be greater than " +
                    "the Investment Amount.\n\n" +
                    "Please enter a Purchase Price that is " +
                    "less than or equal to the Investment Amount.",
                    "Invalid Investment",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPurchasePrice.Focus();
                txtPurchasePrice.SelectAll();

                return false;
            }


            // -----------------------------------------------------
            // SELLING PRICE BELOW PURCHASE PRICE
            // -----------------------------------------------------

            if (sellingPrice < purchasePrice)
            {
                DialogResult result =
                    MessageBox.Show(
                        "Selling Price is below Purchase Price.\n\n" +
                        "This will result in a loss.\n\n" +
                        "Do you want to continue?",
                        "Loss Warning",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);


                if (result == DialogResult.No)
                {
                    txtSellingPrice.Focus();
                    txtSellingPrice.SelectAll();

                    return false;
                }
            }


            // -----------------------------------------------------
            // SELLING PRICE EQUAL TO PURCHASE PRICE
            // -----------------------------------------------------

            if (sellingPrice == purchasePrice)
            {
                DialogResult result =
                    MessageBox.Show(
                        "Selling Price is equal to Purchase Price.\n\n" +
                        "The investment will result in a " +
                        "break-even situation.\n\n" +
                        "Do you want to continue?",
                        "Break-Even Warning",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);


                if (result == DialogResult.No)
                {
                    txtSellingPrice.Focus();
                    txtSellingPrice.SelectAll();

                    return false;
                }
            }


            return true;
        }


        // =========================================================
        // CALCULATE ROI
        // =========================================================

        private void CalculateROI()
        {
            // -----------------------------------------------------
            // COMPANY VALIDATION
            // -----------------------------------------------------

            if (cmbCompany.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a company.",
                    "Company Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCompany.Focus();

                return;
            }


            // -----------------------------------------------------
            // FILER VALIDATION
            // -----------------------------------------------------

            if (!rdoFiler.Checked &&
                !rdoNonFiler.Checked)
            {
                MessageBox.Show(
                    "Please select Filer or Non-Filer.",
                    "Investor Status Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }


            // -----------------------------------------------------
            // INVESTMENT
            // -----------------------------------------------------

            decimal investment;

            if (!TryGetAmount(
                txtInvestment,
                "Investment Amount",
                true,
                out investment))
            {
                return;
            }


            // -----------------------------------------------------
            // PURCHASE PRICE
            // -----------------------------------------------------

            decimal purchasePrice;

            if (!TryGetAmount(
                txtPurchasePrice,
                "Purchase Price",
                true,
                out purchasePrice))
            {
                return;
            }


            // -----------------------------------------------------
            // SELLING PRICE
            // -----------------------------------------------------

            decimal sellingPrice;

            if (!TryGetAmount(
                txtSellingPrice,
                "Selling Price",
                false,
                out sellingPrice))
            {
                return;
            }


            // -----------------------------------------------------
            // RELATIONSHIP VALIDATION
            // -----------------------------------------------------

            if (!ValidateInvestmentRelationships(
                investment,
                purchasePrice,
                sellingPrice))
            {
                return;
            }


            // -----------------------------------------------------
            // CALCULATIONS
            // -----------------------------------------------------

            decimal grossProfit =
                sellingPrice -
                purchasePrice;


            string company =
                cmbCompany.SelectedItem.ToString();


            decimal companyTaxRate =
                companyTaxes[company];


            decimal governmentTaxRate =
                rdoFiler.Checked
                    ? 0.02m
                    : 0.04m;


            decimal companyTax = 0;
            decimal governmentTax = 0;


            if (grossProfit > 0)
            {
                companyTax =
                    grossProfit *
                    companyTaxRate;

                governmentTax =
                    grossProfit *
                    governmentTaxRate;
            }


            decimal totalTax =
                companyTax +
                governmentTax;


            decimal netProfit =
                grossProfit -
                totalTax;


            decimal roi =
                (netProfit / investment) *
                100;


            // -----------------------------------------------------
            // DISPLAY RESULTS
            // -----------------------------------------------------

            lblSelectedCompany.Text =
                "Company: " +
                company;


            lblGrossProfit.Text =
                "Gross Profit/Loss: $" +
                grossProfit.ToString("N2");


            lblCompanyTax.Text =
                "Company Tax: $" +
                companyTax.ToString("N2");


            lblGovernmentTax.Text =
                "Government Tax: $" +
                governmentTax.ToString("N2");


            lblTotalTax.Text =
                "Total Tax: $" +
                totalTax.ToString("N2");


            lblNetProfit.Text =
                "Net Profit/Loss: $" +
                netProfit.ToString("N2");


            lblROI.Text =
                roi.ToString("0.00") +
                "%";


            // -----------------------------------------------------
            // STATUS
            // -----------------------------------------------------

            string status;


            if (grossProfit > 0)
            {
                status =
                    "PROFIT";

                lblStatus.Text =
                    status;

                lblStatus.ForeColor =
                    Green;

                lblNetProfit.ForeColor =
                    Green;
            }
            else if (grossProfit == 0)
            {
                status =
                    "BREAK-EVEN";

                lblStatus.Text =
                    status;

                lblStatus.ForeColor =
                    Orange;

                lblNetProfit.ForeColor =
                    Orange;
            }
            else
            {
                status =
                    "LOSS";

                lblStatus.Text =
                    status;

                lblStatus.ForeColor =
                    Red;

                lblNetProfit.ForeColor =
                    Red;
            }


            // -----------------------------------------------------
            // ROI COLOR
            // -----------------------------------------------------

            if (roi > 0)
            {
                lblROI.ForeColor =
                    Green;
            }
            else if (roi == 0)
            {
                lblROI.ForeColor =
                    Orange;
            }
            else
            {
                lblROI.ForeColor =
                    Red;
            }


            // -----------------------------------------------------
            // SAVE TO HISTORY
            // -----------------------------------------------------

            roiHistory.Add(
                new ROIHistory
                {
                    Date =
                        DateTime.Now,

                    Company =
                        company,

                    Investment =
                        investment,

                    PurchasePrice =
                        purchasePrice,

                    SellingPrice =
                        sellingPrice,

                    GrossProfit =
                        grossProfit,

                    TotalTax =
                        totalTax,

                    NetProfit =
                        netProfit,

                    ROI =
                        roi,

                    Status =
                        status
                });


            // -----------------------------------------------------
            // CONGRATULATIONS POPUP
            // -----------------------------------------------------

            if (grossProfit > 0 &&
                roi > 0)
            {
                ShowCongratulations(
                    company,
                    roi,
                    netProfit);
            }
        }


        // =========================================================
        // COMPARE COMPANIES
        // =========================================================

        private void CompareCompanies()
        {
            // -----------------------------------------------------
            // INVESTOR STATUS
            // -----------------------------------------------------

            if (!rdoFiler.Checked &&
                !rdoNonFiler.Checked)
            {
                MessageBox.Show(
                    "Please select Filer or Non-Filer before comparing companies.",
                    "Investor Status Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }


            // -----------------------------------------------------
            // INVESTMENT
            // -----------------------------------------------------

            decimal investment;

            if (!TryGetAmount(
                txtInvestment,
                "Investment Amount",
                true,
                out investment))
            {
                return;
            }


            // -----------------------------------------------------
            // PURCHASE PRICE
            // -----------------------------------------------------

            decimal purchasePrice;

            if (!TryGetAmount(
                txtPurchasePrice,
                "Purchase Price",
                true,
                out purchasePrice))
            {
                return;
            }


            // -----------------------------------------------------
            // SELLING PRICE
            // -----------------------------------------------------

            decimal sellingPrice;

            if (!TryGetAmount(
                txtSellingPrice,
                "Selling Price",
                false,
                out sellingPrice))
            {
                return;
            }


            // -----------------------------------------------------
            // RELATIONSHIP VALIDATION
            // -----------------------------------------------------

            if (!ValidateInvestmentRelationships(
                investment,
                purchasePrice,
                sellingPrice))
            {
                return;
            }


            // -----------------------------------------------------
            // SELECT COMPANIES
            // -----------------------------------------------------

            List<string> selectedCompanies =
                ShowCompanySelection();


            if (selectedCompanies == null ||
                selectedCompanies.Count == 0)
            {
                return;
            }


            // -----------------------------------------------------
            // GOVERNMENT TAX
            // -----------------------------------------------------

            decimal governmentTaxRate =
                rdoFiler.Checked
                    ? 0.02m
                    : 0.04m;


            List<CompanyResult> results =
                new List<CompanyResult>();


            // -----------------------------------------------------
            // CALCULATE SELECTED COMPANIES
            // -----------------------------------------------------

            foreach (
                string company
                in selectedCompanies)
            {
                decimal companyTaxRate =
                    companyTaxes[company];


                decimal grossProfit =
                    sellingPrice -
                    purchasePrice;


                decimal companyTax = 0;
                decimal governmentTax = 0;


                if (grossProfit > 0)
                {
                    companyTax =
                        grossProfit *
                        companyTaxRate;

                    governmentTax =
                        grossProfit *
                        governmentTaxRate;
                }


                decimal totalTax =
                    companyTax +
                    governmentTax;


                decimal netProfit =
                    grossProfit -
                    totalTax;


                decimal roi =
                    (netProfit / investment) *
                    100;


                results.Add(
                    new CompanyResult
                    {
                        Company =
                            company,

                        TaxRate =
                            companyTaxRate,

                        Investment =
                            investment,

                        PurchasePrice =
                            purchasePrice,

                        SellingPrice =
                            sellingPrice,

                        GrossProfit =
                            grossProfit,

                        TotalTax =
                            totalTax,

                        NetProfit =
                            netProfit,

                        ROI =
                            roi
                    });
            }


            // -----------------------------------------------------
            // FIND HIGHEST ROI
            // -----------------------------------------------------

            CompanyResult highest =
                results
                    .OrderByDescending(
                        x => x.ROI)
                    .First();


            ShowComparisonForm(
                results,
                highest);
        }


        // =========================================================
        // COMPANY SELECTION FORM
        // =========================================================

        private List<string> ShowCompanySelection()
        {
            Form selectionForm =
                new Form();


            selectionForm.Text =
                "Select Companies";


            selectionForm.StartPosition =
                FormStartPosition.CenterParent;


            selectionForm.Size =
                new Size(520, 430);


            selectionForm.BackColor =
                Light;


            selectionForm.FormBorderStyle =
                FormBorderStyle.FixedDialog;


            selectionForm.MaximizeBox =
                false;


            selectionForm.MinimizeBox =
                false;


            selectionForm.Font =
                new Font(
                    "Segoe UI",
                    10F);


            // =====================================================
            // HEADER
            // =====================================================

            Panel header =
                new Panel();

            header.BackColor =
                Dark;

            header.Dock =
                DockStyle.Top;

            header.Height =
                75;


            Label title =
                new Label();

            title.Text =
                "SELECT COMPANIES";

            title.ForeColor =
                White;

            title.Font =
                new Font(
                    "Segoe UI",
                    18F,
                    FontStyle.Bold);

            title.Location =
                new Point(25, 20);

            title.AutoSize =
                true;


            header.Controls.Add(
                title);

            selectionForm.Controls.Add(
                header);


            // =====================================================
            // INSTRUCTION
            // =====================================================

            Label instruction =
                new Label();

            instruction.Text =
                "Choose the companies you want to compare:";

            instruction.ForeColor =
                Gray;

            instruction.Location =
                new Point(25, 95);

            instruction.AutoSize =
                true;


            selectionForm.Controls.Add(
                instruction);


            // =====================================================
            // CHECKED LIST
            // =====================================================

            CheckedListBox companyList =
                new CheckedListBox();

            companyList.Location =
                new Point(25, 125);

            companyList.Size =
                new Size(450, 160);

            companyList.BorderStyle =
                BorderStyle.FixedSingle;

            companyList.Font =
                new Font(
                    "Segoe UI",
                    10F);


            foreach (
                KeyValuePair<string, decimal> item
                in companyTaxes)
            {
                companyList.Items.Add(
                    item.Key +
                    "    (" +
                    (item.Value * 100)
                        .ToString("0.0") +
                    "%)",
                    true);
            }


            selectionForm.Controls.Add(
                companyList);


            // =====================================================
            // SELECT ALL
            // =====================================================

            Button selectAllButton =
                CreateButton(
                    "SELECT ALL",
                    new Point(25, 300),
                    new Size(130, 40),
                    Green);


            selectAllButton.Click +=
                (s, e) =>
                {
                    for (
                        int i = 0;
                        i < companyList.Items.Count;
                        i++)
                    {
                        companyList.SetItemChecked(
                            i,
                            true);
                    }
                };


            selectionForm.Controls.Add(
                selectAllButton);


            // =====================================================
            // CLEAR ALL
            // =====================================================

            Button clearAllButton =
                CreateButton(
                    "CLEAR ALL",
                    new Point(170, 300),
                    new Size(130, 40),
                    Gray);


            clearAllButton.Click +=
                (s, e) =>
                {
                    for (
                        int i = 0;
                        i < companyList.Items.Count;
                        i++)
                    {
                        companyList.SetItemChecked(
                            i,
                            false);
                    }
                };


            selectionForm.Controls.Add(
                clearAllButton);


            // =====================================================
            // COMPARE BUTTON
            // =====================================================

            Button compareButton =
                CreateButton(
                    "COMPARE",
                    new Point(315, 300),
                    new Size(160, 40),
                    Red);


            List<string> selectedCompanies =
                new List<string>();


            compareButton.Click +=
                (s, e) =>
                {
                    if (
                        companyList.CheckedItems.Count == 0)
                    {
                        MessageBox.Show(
                            "Please select at least one company.",
                            "Select Company",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return;
                    }


                    selectedCompanies.Clear();


                    foreach (
                        object item
                        in companyList.CheckedItems)
                    {
                        string text =
                            item.ToString();


                        string companyName =
                            text.Split('(')[0]
                                .Trim();


                        selectedCompanies.Add(
                            companyName);
                    }


                    selectionForm.DialogResult =
                        DialogResult.OK;

                    selectionForm.Close();
                };


            selectionForm.Controls.Add(
                compareButton);


            // =====================================================
            // CANCEL BUTTON
            // =====================================================

            Button cancelButton =
                CreateButton(
                    "CANCEL",
                    new Point(25, 350),
                    new Size(450, 35),
                    Dark);


            cancelButton.Click +=
                (s, e) =>
                {
                    selectedCompanies.Clear();

                    selectionForm.Close();
                };


            selectionForm.Controls.Add(
                cancelButton);


            selectionForm.ShowDialog(
                this);


            return selectedCompanies;
        }


        // =========================================================
        // COMPARISON FORM
        // =========================================================

        private void ShowComparisonForm(
            List<CompanyResult> results,
            CompanyResult highest)
        {
            Form comparisonForm =
                new Form();


            comparisonForm.Text =
                "Company Comparison";


            comparisonForm.StartPosition =
                FormStartPosition.CenterParent;


            comparisonForm.Size =
                new Size(1100, 580);


            comparisonForm.BackColor =
                Light;


            comparisonForm.Font =
                new Font(
                    "Segoe UI",
                    9F);


            // =====================================================
            // HEADER
            // =====================================================

            Panel header =
                new Panel();

            header.BackColor =
                Dark;

            header.Dock =
                DockStyle.Top;

            header.Height =
                80;


            Label title =
                new Label();

            title.Text =
                "COMPANY ROI COMPARISON";

            title.ForeColor =
                White;

            title.Font =
                new Font(
                    "Segoe UI",
                    20F,
                    FontStyle.Bold);

            title.Location =
                new Point(25, 20);

            title.AutoSize =
                true;


            header.Controls.Add(
                title);

            comparisonForm.Controls.Add(
                header);


            // =====================================================
            // DATA GRID
            // =====================================================

            DataGridView dgvComparison =
                new DataGridView();


            dgvComparison.Location =
                new Point(20, 100);


            dgvComparison.Size =
                new Size(1040, 330);


            dgvComparison.BackgroundColor =
                White;


            dgvComparison.BorderStyle =
                BorderStyle.None;


            dgvComparison.AllowUserToAddRows =
                false;


            dgvComparison.AllowUserToDeleteRows =
                false;


            dgvComparison.ReadOnly =
                true;


            dgvComparison.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;


            dgvComparison.RowHeadersVisible =
                false;


            dgvComparison.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;


            dgvComparison.EnableHeadersVisualStyles =
                false;


            dgvComparison.ColumnHeadersDefaultCellStyle.BackColor =
                Dark;


            dgvComparison.ColumnHeadersDefaultCellStyle.ForeColor =
                White;


            dgvComparison.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);


            dgvComparison.DefaultCellStyle.BackColor =
                White;


            dgvComparison.DefaultCellStyle.ForeColor =
                Dark;


            dgvComparison.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(
                    255,
                    225,
                    232);


            dgvComparison.DefaultCellStyle.SelectionForeColor =
                Dark;


            // =====================================================
            // COLUMNS
            // =====================================================

            dgvComparison.Columns.Add(
                "Company",
                "Company");


            dgvComparison.Columns.Add(
                "TaxRate",
                "Tax Rate");


            dgvComparison.Columns.Add(
                "Investment",
                "Investment");


            dgvComparison.Columns.Add(
                "Purchase",
                "Purchase Price");


            dgvComparison.Columns.Add(
                "Sale",
                "Sale Price");


            dgvComparison.Columns.Add(
                "Gross",
                "Gross Profit");


            dgvComparison.Columns.Add(
                "Tax",
                "Total Tax");


            dgvComparison.Columns.Add(
                "Net",
                "Net Profit");


            dgvComparison.Columns.Add(
                "ROI",
                "ROI");


            // =====================================================
            // ADD ROWS
            // =====================================================

            foreach (
                CompanyResult result
                in results)
            {
                int rowIndex =
                    dgvComparison.Rows.Add(
                        result.Company,

                        (result.TaxRate * 100)
                            .ToString("0.0") +
                        "%",

                        "$" +
                        result.Investment
                            .ToString("N2"),

                        "$" +
                        result.PurchasePrice
                            .ToString("N2"),

                        "$" +
                        result.SellingPrice
                            .ToString("N2"),

                        "$" +
                        result.GrossProfit
                            .ToString("N2"),

                        "$" +
                        result.TotalTax
                            .ToString("N2"),

                        "$" +
                        result.NetProfit
                            .ToString("N2"),

                        result.ROI
                            .ToString("0.00") +
                        "%"
                    );


                if (
                    result.Company ==
                    highest.Company)
                {
                    dgvComparison.Rows[rowIndex]
                        .DefaultCellStyle
                        .BackColor =
                        Color.FromArgb(
                            255,
                            235,
                            240);


                    dgvComparison.Rows[rowIndex]
                        .DefaultCellStyle
                        .Font =
                        new Font(
                            "Segoe UI",
                            9F,
                            FontStyle.Bold);
                }
            }


            comparisonForm.Controls.Add(
                dgvComparison);


            // =====================================================
            // HIGHEST ROI INFORMATION
            // =====================================================

            Label highestLabel =
                new Label();


            highestLabel.Text =
                "HIGHEST CALCULATED ROI: " +
                highest.Company +
                "  —  " +
                highest.ROI
                    .ToString("0.00") +
                "%";


            highestLabel.ForeColor =
                Red;


            highestLabel.Font =
                new Font(
                    "Segoe UI",
                    14F,
                    FontStyle.Bold);


            highestLabel.Location =
                new Point(25, 450);


            highestLabel.AutoSize =
                true;


            comparisonForm.Controls.Add(
                highestLabel);


            // =====================================================
            // CLOSE BUTTON
            // =====================================================

            Button closeButton =
                CreateButton(
                    "CLOSE",
                    new Point(900, 445),
                    new Size(140, 42),
                    Dark);


            closeButton.Click +=
                (s, e) =>
                {
                    comparisonForm.Close();
                };


            comparisonForm.Controls.Add(
                closeButton);


            comparisonForm.ShowDialog(
                this);
        }


        // =========================================================
        // CONGRATULATIONS POPUP
        // =========================================================

        private void ShowCongratulations(
            string company,
            decimal roi,
            decimal netProfit)
        {
            Form popup =
                new Form();


            popup.Text =
                "Congratulations!";


            popup.StartPosition =
                FormStartPosition.CenterParent;


            popup.Size =
                new Size(430, 330);


            popup.FormBorderStyle =
                FormBorderStyle.FixedDialog;


            popup.MaximizeBox =
                false;


            popup.MinimizeBox =
                false;


            popup.BackColor =
                White;


            popup.Font =
                new Font(
                    "Segoe UI",
                    10F);


            // =====================================================
            // EMOJI
            // =====================================================

            Label emoji =
                new Label();

            emoji.Text =
                "🎉";


            emoji.Font =
                new Font(
                    "Segoe UI Emoji",
                    40F);


            emoji.Location =
                new Point(175, 10);


            emoji.AutoSize =
                true;


            popup.Controls.Add(
                emoji);


            // =====================================================
            // TITLE
            // =====================================================

            Label title =
                new Label();

            title.Text =
                "Congratulations!";


            title.ForeColor =
                Green;


            title.Font =
                new Font(
                    "Segoe UI",
                    20F,
                    FontStyle.Bold);


            title.Location =
                new Point(90, 80);


            title.AutoSize =
                true;


            popup.Controls.Add(
                title);


            // =====================================================
            // MESSAGE
            // =====================================================

            Label message =
                new Label();

            message.Text =
                "Your investment generated a profit!\n\n" +
                "Company: " +
                company +
                "\n" +
                "Net Profit: $" +
                netProfit.ToString("N2") +
                "\n" +
                "ROI: " +
                roi.ToString("0.00") +
                "%";


            message.ForeColor =
                Gray;


            message.Font =
                new Font(
                    "Segoe UI",
                    10F);


            message.TextAlign =
                ContentAlignment.MiddleCenter;


            message.Location =
                new Point(55, 120);


            message.Size =
                new Size(
                    320,
                    105);


            popup.Controls.Add(
                message);


            // =====================================================
            // BUTTON
            // =====================================================

            Button okButton =
                CreateButton(
                    "GREAT! 🎉",
                    new Point(135, 245),
                    new Size(140, 40),
                    Green);


            okButton.Click +=
                (s, e) =>
                {
                    popup.Close();
                };


            popup.Controls.Add(
                okButton);


            popup.ShowDialog(
                this);
        }


        // =========================================================
        // SHOW HISTORY
        // =========================================================

        private void ShowHistory()
        {
            Form historyForm =
                new Form();


            historyForm.Text =
                "ROI Calculation History";


            historyForm.StartPosition =
                FormStartPosition.CenterParent;


            historyForm.Size =
                new Size(1100, 600);


            historyForm.BackColor =
                Light;


            historyForm.Font =
                new Font(
                    "Segoe UI",
                    9F);


            // =====================================================
            // HEADER
            // =====================================================

            Panel header =
                new Panel();


            header.BackColor =
                Dark;


            header.Dock =
                DockStyle.Top;


            header.Height =
                80;


            Label title =
                new Label();


            title.Text =
                "ROI CALCULATION HISTORY";


            title.ForeColor =
                White;


            title.Font =
                new Font(
                    "Segoe UI",
                    20F,
                    FontStyle.Bold);


            title.Location =
                new Point(25, 20);


            title.AutoSize =
                true;


            header.Controls.Add(
                title);


            historyForm.Controls.Add(
                header);


            // =====================================================
            // HISTORY GRID
            // =====================================================

            DataGridView dgvHistory =
                new DataGridView();


            dgvHistory.Location =
                new Point(20, 100);


            dgvHistory.Size =
                new Size(
                    1040,
                    380);


            dgvHistory.BackgroundColor =
                White;


            dgvHistory.BorderStyle =
                BorderStyle.None;


            dgvHistory.AllowUserToAddRows =
                false;


            dgvHistory.AllowUserToDeleteRows =
                false;


            dgvHistory.ReadOnly =
                true;


            dgvHistory.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;


            dgvHistory.RowHeadersVisible =
                false;


            dgvHistory.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;


            dgvHistory.EnableHeadersVisualStyles =
                false;


            dgvHistory.ColumnHeadersDefaultCellStyle.BackColor =
                Dark;


            dgvHistory.ColumnHeadersDefaultCellStyle.ForeColor =
                White;


            dgvHistory.ColumnHeadersDefaultCellStyle.Font =
                new Font(
                    "Segoe UI",
                    9F,
                    FontStyle.Bold);


            // =====================================================
            // COLUMNS
            // =====================================================

            dgvHistory.Columns.Add(
                "Date",
                "Date / Time");


            dgvHistory.Columns.Add(
                "Company",
                "Company");


            dgvHistory.Columns.Add(
                "Investment",
                "Investment");


            dgvHistory.Columns.Add(
                "Purchase",
                "Purchase Price");


            dgvHistory.Columns.Add(
                "Selling",
                "Selling Price");


            dgvHistory.Columns.Add(
                "Gross",
                "Gross Profit");


            dgvHistory.Columns.Add(
                "Net",
                "Net Profit/Loss");


            dgvHistory.Columns.Add(
                "ROI",
                "ROI");


            dgvHistory.Columns.Add(
                "Status",
                "Status");


            // =====================================================
            // NO HISTORY
            // =====================================================

            if (roiHistory.Count == 0)
            {
                Label noHistory =
                    new Label();


                noHistory.Text =
                    "No ROI calculations have been recorded yet.";


                noHistory.ForeColor =
                    Gray;


                noHistory.Font =
                    new Font(
                        "Segoe UI",
                        11F);


                noHistory.Location =
                    new Point(
                        25,
                        110);


                noHistory.AutoSize =
                    true;


                historyForm.Controls.Add(
                    noHistory);
            }


            // =====================================================
            // ADD HISTORY
            // =====================================================

            foreach (
                ROIHistory item
                in roiHistory
                    .AsEnumerable()
                    .Reverse())
            {
                int rowIndex =
                    dgvHistory.Rows.Add(
                        item.Date.ToString(
                            "dd/MM/yyyy HH:mm"),

                        item.Company,

                        "$" +
                        item.Investment
                            .ToString("N2"),

                        "$" +
                        item.PurchasePrice
                            .ToString("N2"),

                        "$" +
                        item.SellingPrice
                            .ToString("N2"),

                        "$" +
                        item.GrossProfit
                            .ToString("N2"),

                        "$" +
                        item.NetProfit
                            .ToString("N2"),

                        item.ROI
                            .ToString("0.00") +
                        "%",

                        item.Status);


                if (
                    item.Status ==
                    "PROFIT")
                {
                    dgvHistory.Rows[rowIndex]
                        .DefaultCellStyle
                        .ForeColor =
                        Green;
                }
                else if (
                    item.Status ==
                    "LOSS")
                {
                    dgvHistory.Rows[rowIndex]
                        .DefaultCellStyle
                        .ForeColor =
                        Red;
                }
                else
                {
                    dgvHistory.Rows[rowIndex]
                        .DefaultCellStyle
                        .ForeColor =
                        Orange;
                }
            }


            historyForm.Controls.Add(
                dgvHistory);


            // =====================================================
            // CLOSE BUTTON
            // =====================================================

            Button closeButton =
                CreateButton(
                    "CLOSE",
                    new Point(900, 500),
                    new Size(140, 40),
                    Dark);


            closeButton.Click +=
                (s, e) =>
                {
                    historyForm.Close();
                };


            historyForm.Controls.Add(
                closeButton);


            historyForm.ShowDialog(
                this);
        }


        // =========================================================
        // CLEAR FORM
        // =========================================================

        private void ClearForm()
        {
            cmbCompany.SelectedIndex =
                -1;


            rdoFiler.Checked =
                false;


            rdoNonFiler.Checked =
                false;


            txtInvestment.Clear();


            txtPurchasePrice.Clear();


            txtSellingPrice.Clear();


            lblTaxRate.Text =
                "Tax Rate: —";


            lblSelectedCompany.Text =
                "Company: —";


            lblGrossProfit.Text =
                "Gross Profit/Loss: —";


            lblCompanyTax.Text =
                "Company Tax: —";


            lblGovernmentTax.Text =
                "Government Tax: —";


            lblTotalTax.Text =
                "Total Tax: —";


            lblNetProfit.Text =
                "Net Profit/Loss: —";


            lblROI.Text =
                "—";


            lblROI.ForeColor =
                Red;


            lblStatus.Text =
                "WAITING";


            lblStatus.ForeColor =
                Gray;


            lblNetProfit.ForeColor =
                Gray;
        }


        // =========================================================
        // EXIT APPLICATION
        // =========================================================

        private void ExitApplication()
        {
            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to exit?",
                    "Exit Application",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);


            if (
                result ==
                DialogResult.Yes)
            {
                allowClosing =
                    true;

                this.Close();
            }
        }


        // =========================================================
        // FORM CLOSING
        // =========================================================

        private void Form1_FormClosing(
            object sender,
            FormClosingEventArgs e)
        {
            if (allowClosing)
            {
                return;
            }


            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to exit?",
                    "Exit Application",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);


            if (
                result ==
                DialogResult.No)
            {
                e.Cancel =
                    true;
            }
        }


        // =========================================================
        // COMPANY INFORMATION
        // =========================================================

        private void ShowCompanyInformation()
        {
            string information =
                "COMPANY TAX INFORMATION\n\n" +

                "Company 1     5.0%\n" +
                "Company 2     5.5%\n" +
                "Company 3     7.5%\n" +
                "Company 4     6.3%\n" +
                "Company 5     9.9%\n\n" +

                "Government Tax:\n\n" +

                "Filer          2%\n" +
                "Non-Filer      4%";


            MessageBox.Show(
                information,
                "Company Information",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }


        // =========================================================
        // ABOUT
        // =========================================================

        private void ShowAbout()
        {
            string about =
                "STOCK ROI ANALYZER\n\n" +

                "A Windows Forms application for\n" +
                "investment and ROI analysis.\n\n" +

                "Features:\n\n" +

                "• Profit / Loss calculation\n" +
                "• Company tax calculation\n" +
                "• Government tax calculation\n" +
                "• ROI calculation\n" +
                "• ROI calculation history\n" +
                "• Company comparison\n" +
                "• Select companies for comparison\n" +
                "• Highest calculated ROI identification\n" +
                "• Input validation\n" +
                "• Loss warning\n" +
                "• Break-even warning\n" +
                "• Investment validation\n" +
                "• Congratulations popup";


            MessageBox.Show(
                about,
                "About Stock ROI Analyzer",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }


    // =========================================================
    // COMPANY RESULT CLASS
    // =========================================================

    public class CompanyResult
    {
        public string Company
        {
            get;
            set;
        }


        public decimal TaxRate
        {
            get;
            set;
        }


        public decimal Investment
        {
            get;
            set;
        }


        public decimal PurchasePrice
        {
            get;
            set;
        }


        public decimal SellingPrice
        {
            get;
            set;
        }


        public decimal GrossProfit
        {
            get;
            set;
        }


        public decimal TotalTax
        {
            get;
            set;
        }


        public decimal NetProfit
        {
            get;
            set;
        }


        public decimal ROI
        {
            get;
            set;
        }
    }


    // =========================================================
    // ROI HISTORY CLASS
    // =========================================================

    public class ROIHistory
    {
        public DateTime Date
        {
            get;
            set;
        }


        public string Company
        {
            get;
            set;
        }


        public decimal Investment
        {
            get;
            set;
        }


        public decimal PurchasePrice
        {
            get;
            set;
        }


        public decimal SellingPrice
        {
            get;
            set;
        }


        public decimal GrossProfit
        {
            get;
            set;
        }


        public decimal TotalTax
        {
            get;
            set;
        }


        public decimal NetProfit
        {
            get;
            set;
        }
         

        public decimal ROI
        {
            get;
            set;
        }


        public string Status
        {
            get;
            set;
        }
    }
}

