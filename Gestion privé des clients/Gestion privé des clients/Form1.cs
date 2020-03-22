using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestion_privé_des_clients
{
    public partial class Form1 : Form
    {

        Customers model = new Customers();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            
        }

        void Clear()
        {
            textBox1.Name = "";
            btnSave.Text = "Sauvegarder";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.Name = textBox1.Text; // on chope le texte 

            using(DBEntities db = new DBEntities())
            {
                if (model.CustomerID == 0) // Insertion de nouveau
                {
                    db.Customers.Add(model);
                    db.SaveChanges();
                }
                else // Modification du Client
                {
                    db.Entry(model).State = EntityState.Modified; // Etat d'entité
                    db.SaveChanges();
                }
            }
            Clear();
            PopulateDataGridView();
            MessageBox.Show("Enregistré avec succès dans la base de donnée");
        }

        void PopulateDataGridView()
        {
            using(DBEntities db = new DBEntities())
            {
                grid.DataSource = db.Customers.ToList<Customers>();
            }
        }
        private void grid_AllowUserToDeleteRowsChanged(object sender, EventArgs e)
        {

        }

        private void grid_DoubleClick(object sender, EventArgs e)
        {
            if(grid.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(grid.CurrentRow.Cells["CustomerID"].Value);
                using(DBEntities db = new DBEntities())
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    textBox1.Text = model.Name;
                }
                btnSave.Text = "Modifier";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Etes-vous sur de supprimer ce client ?","Gestion des clients",
                                MessageBoxButtons.YesNo)==DialogResult.Yes)
            {
                using(DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(model); // celui qu'on a chope 
                    if(entry.State == EntityState.Detached)
                    {
                        db.Customers.Attach(model); // On le chope de la bdd
                        db.Customers.Remove(model);
                        db.SaveChanges();
                        PopulateDataGridView(); // on réaffiche toute la bdd
                        Clear(); // Nettoyage
                        MessageBox.Show("Supprimé de la base de donnée avec succès");
                    }
                }
            }
        }
    }
}
