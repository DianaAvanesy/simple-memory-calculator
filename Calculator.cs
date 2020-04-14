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
        String operation = "";  // to catch what arithmetic operation is presed
        bool operation_pressed = false;
        bool isNewEntry = false;
        Double savedValue;
        DataTable dt = new DataTable();

        public Calculator()
        {
            InitializeComponent();
        }

        private void Calculator_Load(object sender, EventArgs e)
        {
            MemoryDispl.Enabled = false;
            backgroud.Enabled = false;
        }
        private void Clear_Click(object sender, EventArgs e)
        {
            txtResults.Clear();
            value = 0;
        } 

        private void NumbButtonClick(object sender, EventArgs e)
        {
            if (isNewEntry)
            {
                txtResults.Text = "0";
                isNewEntry = false;
            }

            if ((txtResults.Text == "0")  || operation_pressed){
                txtResults.Clear();}

            operation_pressed = false;
            Button button = (Button)sender;
            //txtResults.Text = txtResults.Text + button.Text;

            // does number alredy contains the decimal point?
            if (button.Text == ".")
            {
                if (!txtResults.Text.Contains("."))
                    txtResults.Text = txtResults.Text + button.Text;
            }
            else txtResults.Text = txtResults.Text + button.Text; 

        }

        private void OperatorClick(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            // saving operation 
            operation = b.Text;
            //value gets populated with whatever in the box
            value = Double.Parse(txtResults.Text);
            operation_pressed = true;
           // txtResults.Text = txtResults.Text + operation;
        }

        private void Equals_Click(object sender, EventArgs e)
        {
          if (operation_pressed == false)
            {
                // DataTable dt = new DataTable();
                String  v = (dt.Compute(txtResults.Text,string.Empty)).ToString();
                txtResults.Text = v;
            }

            switch (operation)
            {
                case "+":
                    //txtResults.Text = int.TryParse(txtResults.Text, out );
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
                        try
                        {   if (double.IsInfinity(value / Double.Parse(txtResults.Text)))
                                {
                                    throw new DivideByZeroException ("Divided by 0");
                                }
                            txtResults.Text = (value / Double.Parse(txtResults.Text)).ToString();
                            break;
                        }
                        catch (DivideByZeroException)
                        {
                            txtResults.Text = ("Error. Number divided by ziro");
                            break;
                        }
                    }
                default:
                    break;
            }
            operation_pressed = false;
        }

        private void MemoryClear_Click(object sender, EventArgs e)
        {
            savedValue = 0.0d ;
            MemoryDispl.Clear();
        }

        private void MemoryStore_Click(object sender, EventArgs e)
        {
            savedValue = Double.Parse(txtResults.Text);
            MemoryDispl.Text = "M";
        }

        private void MemoryRecall_Click(object sender, EventArgs e)
        {
            txtResults.Text = savedValue.ToString();
        }

        private void MemoryPlus_Click(object sender, EventArgs e)
        {
            savedValue = savedValue + Double.Parse(txtResults.Text);
        }

        private void Back_Click(object sender, EventArgs e)
        {
            if (!(txtResults.Text == ""))
            {
                txtResults.Text = (txtResults.Text.ToString().Remove(txtResults.Text.ToString().Length - 1));
            }
        }

        private void SquareRoot_Click(object sender, EventArgs e)
        {
            txtResults.Text = Math.Sqrt(Double.Parse(txtResults.Text)).ToString();
        }

        private void ChangeArithmeticSign_Click(object sender, EventArgs e)
        {
            if (!txtResults.Text.Contains('-'))
            {
                txtResults.Text = "-" + txtResults.Text ; ;
            }
            else
            {
                txtResults.Text = txtResults.Text.Trim('-');

            }
        }

        private void OneDividedByCurrentEntry_Click(object sender, EventArgs e)
        {
            txtResults.Text = (1 / Double.Parse(txtResults.Text)).ToString();
        }

        private void txtResults_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Equals_Click(sender, e);
            }
        }
    }
}
