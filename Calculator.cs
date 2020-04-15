using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _200421248A2
{
    public partial class Calculator : Form
    {
        Double value = 0;
        String operation = "";  
        bool operationPressed = false;
        Double savedValue;
        DataTable dt = new DataTable();

        public Calculator()
        {
            InitializeComponent();
        }

        private void Calculator_Load(object sender, EventArgs e)
        {
            // Making MemoryDispl unavailible for user to change
            txtMemoryDispl.Enabled = false;
            backgroud.Enabled = false;
        }

        /// <summary>
        /// Clears txtResults values and resets memory to the default state 
        /// </summary>
        private void Clear_Click(object sender, EventArgs e)
        {
            txtResults.Clear();
            value = 0;
            MemoryClear_Click(sender, e);
        }

        /// <summary>
        /// Function for numeric buttons that adds number/dot that been presed to the display
        /// </summary>
        private void NumbButtonClick(object sender, EventArgs e)
        {
            // If number button was click after operation been presseed
            if (operationPressed) 
                {
                    //clean txtResult from previos number
                    txtResults.Clear(); 
                }
            //after we dealt with cleaning txtResults, set opperation_pressed flag to false
            operationPressed = false;
            Button button = (Button)sender;

            // If button that been presed is a dot
            if (button.Text == ".")
            {
                if (txtResults.Text != "")
                {
                    // make sure that its a first dot in the numberand add dot to the txtResults
                    if (!txtResults.Text.Contains("."))
                        txtResults.Text = txtResults.Text + button.Text;
                }
                // if txtResults is empty, add 0 with a dot
                else txtResults.Text = "0.";
            }
            // Otherwise add number to the txtResults
            else txtResults.Text = txtResults.Text + button.Text;
        }

        /// <summary>
        /// Function for operators buttons that saves operation for future calculations
        /// </summary>
        private void OperatorClick(object sender, EventArgs e)
        {
            // saving operation for later
            Button b = (Button)sender;
            operation = b.Text;
            //value gets populated with the contents of the txtResult box
            value = Double.Parse(txtResults.Text);
            // set operationPressed flag to true
            operationPressed = true;
        }

        /// <summary>
        /// Function that being called when equals button been pressed,
        /// Calculating result based on the operator or formula that been input by user
        /// </summary>
        private void Equals_Click(object sender, EventArgs e)
        {
            // Scenario 1:  if prior to that operation button wasnt pressed, that means that user input a formula using keyboard
            if (operationPressed == false)
            {
                // Calculate formula using DataTable.Compute()
                // Credit: https://docs.microsoft.com/en-us/dotnet/api/system.data.datatable.compute?view=netframework-4.8
                String v = (dt.Compute(txtResults.Text, string.Empty)).ToString();
                txtResults.Text = Double.Parse(v.Trim().ToString()).ToString().Trim();
            }

            // Scenario 2: if operation button been pressed
            // calculate result based on the operator
            switch (operation)
            {
                case "+":
                    txtResults.Text = (value + Double.Parse(txtResults.Text)).ToString();
                    break;
                case "-":
                    txtResults.Text = (value - Double.Parse(txtResults.Text)).ToString();
                    break;
                case "*":
                    txtResults.Text = (value * Double.Parse(txtResults.Text)).ToString();
                    break;
                case "/":
                    {
                        // try to devide numbers if operator is devision
                        // if result will equal to infinity throw DivideByZeroException
                        try
                        {
                            if (double.IsInfinity(value / Double.Parse(txtResults.Text)))
                            {
                                throw new DivideByZeroException("Divided by 0");
                            }
                            txtResults.Text = (value / Double.Parse(txtResults.Text)).ToString();
                            break;
                        }
                        // catch exeption if one been trown and print error message in txtResults
                        catch (DivideByZeroException)
                        {
                            txtResults.Text = ("Error. Number divided by ziro");
                            break;
                        }
                    }
                default:
                    break;
            }
            // unset flag 
            operationPressed = false;
        }

        /// <summary>
        /// Function that clears memory 
        /// </summary>
        private void MemoryClear_Click(object sender, EventArgs e)
        {
            savedValue = 0.0d;
            txtMemoryDispl.Clear();
        }

        /// <summary>
        /// Function allow user store number in memory
        /// </summary>
        private void MemoryStore_Click(object sender, EventArgs e)
        {
            // Store in memory only if user input a number
            if (txtResults.Text != "")
            {
                savedValue = Double.Parse(txtResults.Text);
                txtMemoryDispl.Text = "M";
            }
        }

        /// <summary>
        /// Function takes memory number that user set before and displaying it in the txtResult textbox 
        /// </summary>
        private void MemoryRecall_Click(object sender, EventArgs e)
        {
            txtResults.Text = savedValue.ToString();
        }

        /// <summary>
        /// Function that takes number from txtResult textbox and adding it to the memory value
        /// </summary>
        private void MemoryPlus_Click(object sender, EventArgs e)
        {
            savedValue = savedValue + Double.Parse(txtResults.Text);
        }

        /// <summary>
        ///  Function that beeing called when back button clicked, allow user to delete last entered number
        /// </summary>
        private void Back_Click(object sender, EventArgs e)
        {
            //  check if there is something in a txtResults textbox
            if (!(txtResults.Text == ""))
            {
                txtResults.Text = (txtResults.Text.ToString().Remove(txtResults.Text.ToString().Length - 1)).Trim();
            }
        }

        /// <summary>
        /// Function calculate Square Root on a input number
        /// If no number been input dont do anything
        /// </summary>
        private void SquareRoot_Click(object sender, EventArgs e)
        {
            if (txtResults.Text != "")
                txtResults.Text = Math.Sqrt(Double.Parse(txtResults.Text)).ToString();
        }

        /// <summary>
        /// Function allows user to change arithmetic sign of the number in txtResult textbox
        /// </summary>
        private void ChangeArithmeticSign_Click(object sender, EventArgs e)
        {
            // if number dont already contain minus add it in the begining
            if (!txtResults.Text.Contains('-'))
            {
                txtResults.Text = "-" + txtResults.Text.Trim();
            }
            // else, delete minus from the string
            else
            {
                txtResults.Text = txtResults.Text.Trim('-');
            }
        }

        /// <summary>
        /// Function perfomes 1/x functionality, where x is number provided by user
        /// </summary>
        private void OneDividedByCurrentEntry_Click(object sender, EventArgs e)
        {
            if (txtResults.Text != "")
            txtResults.Text = (1 / Double.Parse(txtResults.Text)).ToString();
        }

        /// <summary>
        /// In txtResults textbox, presing Enter will triger Equals_Click function 
        /// </summary>
        private void txtResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Equals_Click(sender, e);
            }
        }

        /// <summary>
        /// Input validation in txtResult textbox, that dont allow user input letters and numbers with multiple dots
        /// </summary>
        private void txtResultsKeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
             if ((ch == '.' && txtResults.Text.Contains(".")) || char.IsLetter(ch)){
                e.Handled = true;
             }
        }
    }
}
