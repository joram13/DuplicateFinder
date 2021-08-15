using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DuplicateFinder.Logic.Interface;
using DuplicateFinder.Logic.Model;
using System.Diagnostics;
using System.IO;


namespace DFJE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string folderPath = textBox1.Text;

            if (checkBox1.Checked == false && checkBox2.Checked == false && checkBox3.Checked == false && checkBox4.Checked == false && checkBox5.Checked == false && !Directory.Exists(folderPath))
            {
                label4.Visible = true;
                label5.Visible = true;
                dataGridView1.Visible = false;
            }

            else if (!Directory.Exists(folderPath))
            {
                label4.Visible = true;
                label5.Visible = false;
                dataGridView1.Visible = false;

            }

            else if (checkBox1.Checked == false && checkBox2.Checked == false && checkBox3.Checked == false && checkBox4.Checked == false && checkBox5.Checked == false)
            {
                label5.Visible = false;
                label5.Visible = true;
                dataGridView1.Visible = false;
            }

            else
            {
                label5.Visible = false;
                label5.Visible = false;


                DataTable table = new DataTable();
                table.Columns.Add("Duplicates");
                table.Columns.Add("Criteria");
                dataGridView1.Visible = true;
                //DataRow dr = table.NewRow();
                //dr["Duplicates"] = "test";
                //dr["Criteria"] = "jklj";
                //table.Rows.Add(dr);
                //dataGridView1.DataSource = table;


                var finder = new DuplicateFinder.Logic.DuplicateFinder();




                if (checkBox1.Checked == true || checkBox5.Checked == true)
                {
                    var duplicatesBySize = finder.CollectCandidates(folderPath, CompareMode.Size).ToList();
                    foreach(var duplicate in duplicatesBySize)
                    {
                        DataRow dr = table.NewRow();
                        dr["Duplicates"] = String.Join(", ", duplicate.FilePaths.Select(o => o.ToString()));
                        dr["Criteria"] = "Size";
                        table.Rows.Add(dr);
                    }

                }

                

                if (checkBox2.Checked == true || checkBox5.Checked == true)
                {
                    var duplicatesByName = finder.CollectCandidates(folderPath, CompareMode.Name).ToList();
                    foreach (var duplicate in duplicatesByName)
                    {
                        DataRow dr = table.NewRow();
                        dr["Duplicates"] = String.Join(", ", duplicate.FilePaths.Select(o => o.ToString()));
                        dr["Criteria"] = "Name";
                        table.Rows.Add(dr);
                    }

                }

                if (checkBox3.Checked == true || checkBox5.Checked == true)
                {
                    var duplicatesBySizeAndName = finder.CollectCandidates(folderPath, CompareMode.SizeAndName).ToList();
                    foreach (var duplicate in duplicatesBySizeAndName)
                    {
                        DataRow dr = table.NewRow();
                        dr["Duplicates"] = String.Join(", ", duplicate.FilePaths.Select(o => o.ToString()));
                        dr["Criteria"] = "Size and Name";
                        table.Rows.Add(dr);
                    }

                }

                if (checkBox4.Checked == true || checkBox5.Checked == true)
                {
                    List<IDuplicate> duplicatesBySize = finder.CollectCandidates(folderPath, CompareMode.Size).ToList();
                    var duplicatesByHash = finder.CheckCandidates(duplicatesBySize).ToList();
                    foreach (var duplicate in duplicatesByHash)
                    {
                        DataRow dr = table.NewRow();
                        dr["Duplicates"] = String.Join(", ", duplicate.FilePaths.Select(o => o.ToString()));
                        dr["Criteria"] = "Hash";
                        table.Rows.Add(dr);
                    }

                }
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                dataGridView1.DataSource = table;
                dataGridView1.Columns["Duplicates"].ReadOnly = true;
                dataGridView1.Columns["Criteria"].ReadOnly = true;
                dataGridView1.Columns["Duplicates"].Width = 400;
                dataGridView1.Columns["Criteria"].Width = 400;

                




            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_AutoSizeChanged(object sender, EventArgs e)
        {

        }

        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
