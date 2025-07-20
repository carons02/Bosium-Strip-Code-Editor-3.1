using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Bosium_Strip
{
    public partial class BosiumStripForm : System.Windows.Forms.Form
    {
        // Create a member class variable to track the current file path
        private string m_currentFilePath = string.Empty;
        private string m_currentFileName = string.Empty;

        public BosiumStripForm()
        {
            //This will run the VS code and VS created/wrote.
            InitializeComponent();

            //Created custom styling for the Form after its made
            SetupStyling();

        }

        private void BosiumStripForm_Load(object sender, EventArgs e)
        {
            //Code for setting up dialog box filters for text code files
            openFileDialogMain.Filter = "Text Files (*.txt)|*.txt|C# Console Files(*.cs)|*.cs|Python Files (*.py)|*.py|All Files (*.*)|*.*";
            saveFileDialogMain.Filter = "Text Files (*.txt)|*.txt|C# Console Files(*.cs)|*.cs|Python Files (*.py)|*.py|All Files (*.*)|*.*";

            //Starts the cursor in the text box editor
            TextBoxEditor.Focus();
        }
        //Code area for creating custom styling for form
        private void SetupStyling()
        {
            // Color set for overall Form
            this.BackColor = Color.LightCyan;

            // Color set for the Menu strip, both front and back
            BosiumStripMenu.BackColor = Color.Black;
            BosiumStripMenu.ForeColor = Color.Cyan;
            BosiumStripMenu.Font = new Font("Lucida Console", 9);
            fileToolStripMenuItem.Font = new Font("Lucida Console", 9);
            editToolStripMenuItem.Font = new Font("Lucida Console", 9);

            // Custom styling for the Text Box: colors, font, wordwrap
            TextBoxEditor.BackColor = Color.DarkGray;
            TextBoxEditor.ForeColor = Color.White;
            TextBoxEditor.Font = new Font("Lucida Console", 11, FontStyle.Bold);
            TextBoxEditor.AcceptsTab = true;
            TextBoxEditor.WordWrap = true;

            // Custom styling for the status strip
            MainStatusStrip.BackColor = Color.Silver;
            MainStatusStrip.ForeColor = Color.Blue;
            MainStatusStrip.Font = new Font("Lucida Console", 11);
            MainStatusStrip.Text = "Input your code area...";

            // Expand the window title
            this.Text = "Bosium Strip Editor";
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // The user raised the new event. need to handle it by created a new code file
            // Clear the current text box
            TextBoxEditor.Clear();
            // clear our current file path
            m_currentFilePath = string.Empty;
            // setting the current file
            this.Text = "Bosium Strip Editor - New File";
            //let the user know what is going on
            BosiumStripStatusLabel.Text = "New File Created";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // The user raised the exit event. Need to close the program
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // The user raised the open event. Need to handle the open function
            if (openFileDialogMain.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    //Try to read the user selected file and put it in our editor
                    string fileContent = File.ReadAllText(openFileDialogMain.FileName);
                    TextBoxEditor.Text = fileContent;
                    m_currentFilePath = openFileDialogMain.FileName;
                    this.Text = "Bosium Strip Editor - " + Path.GetFileName(openFileDialogMain.FileName);
                    BosiumStripStatusLabel.Text = "File Opened: " + Path.GetFileName(openFileDialogMain.FileName);
                }
                catch(Exception ex) 
                {
                    //If Trying to open an existing file doesn't work, an event is raised notifying the user which is handled here
                    MessageBox.Show("The file you were trying to open is not available. " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // The user raised the save event. Need to handle saving the file
            //If the current file path is empty, it runs through the script for user action and input
            if(string.IsNullOrEmpty(m_currentFilePath))
            {
                //show a dialog to choose where to save this current code file in the editor.
                if (saveFileDialogMain.ShowDialog() == DialogResult.OK)
                {
                    //sets current file path equal to the file name
                    m_currentFilePath = saveFileDialogMain.FileName;
                }
                else
                {
                    //If we are here, our user cancelled the save and we do nothing
                    return;
                }
            }
            try
            {
                //Try to read the user defined save path to save the file
                File.WriteAllText(m_currentFilePath, TextBoxEditor.Text);
                this.Text = "Bosium Strip Editor - " + Path.GetFileName(m_currentFilePath);
                BosiumStripStatusLabel.Text = "File saved: " + Path.GetFileName(m_currentFilePath);
            }
            // If there is an error raised by the system by attempting to save, it's handled here
            catch (Exception ex)
            {
                //show an messagebox to our user if there is an error with saving the file
                MessageBox.Show("Error saving the text from the editor to a file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // The user raised the saveas event. Need to save the file as the user wishes to name it, both the current name or new name
            try
            {
                //Give the user the option to save the text file in a new or current location
                File.WriteAllText(saveFileDialogMain.FileName, TextBoxEditor.Text);
                m_currentFilePath = saveFileDialogMain.FileName;
                this.Text = "Bosium Strip Editor - " + Path.GetFileName(m_currentFilePath);
                BosiumStripStatusLabel.Text = "File saved as: " + Path.GetFileName(m_currentFilePath);
            }
            // If there is an error raised by the system by attemplting to save as, it's handled here
            catch (Exception ex)
            {
                //show an messagebox to our user if there is an error with saving the file
                MessageBox.Show("Error in save as process from the editor to a file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this will handle the find sercie in our code editor
            // First let's ask our user what text are you looking for?
            string searchText = Microsoft.VisualBasic.Interaction.InputBox("Enter text to find:", "Find", "");

            // This checks if the user entered something to search for
            if (!string.IsNullOrEmpty(searchText))
            {
                // This searches for the text in the rich text box
                int index = TextBoxEditor.Find(searchText, RichTextBoxFinds.None);

                if (index >= 0)
                {
                    // let's highlight the found text
                    TextBoxEditor.SelectionStart = index;
                    TextBoxEditor.SelectionLength = searchText.Length;
                    TextBoxEditor.Focus();
                    TextBoxEditor.Text = "Found: " + searchText;
                }
                else
                {
                    // This tells the user if the text wasn't found
                    BosiumStripStatusLabel.Text = "Text not found: " + searchText;
                    MessageBox.Show("Text not found!", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //This tells the user background information about the code editor.
            MessageBox.Show("Developed in MS539 for use on future coding as needed." +
                Environment.NewLine + Environment.NewLine + "Circa 16 July, 2025.","About", MessageBoxButtons.OK);

        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //This provides information for the user regarding any help they are hoping to get while using the editor.
            MessageBox.Show("Yeah good luck with that right now." + Environment.NewLine + Environment.NewLine +
                "Maybe try Youtube if you need help." + Environment.NewLine + Environment.NewLine +
                "Hopefully in the future there's more here for you", "Get Help", MessageBoxButtons.OK);
        }
    }//this is the of our class scope formMain
}//this is the end of our namespace scope Bosium Strip
