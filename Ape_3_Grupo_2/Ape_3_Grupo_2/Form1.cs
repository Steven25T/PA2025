using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ape_3_Grupo_2
{
    public partial class Form1 : Form
    {
        private DataGridViewRow filaEditando = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Recoger datos del formulario
            string cedula = txtCedula.Text;
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            string telefono = txtTelefono.Text;
            string correo = txtCorreo.Text;
            DateTime fechaNacimiento = dtpFecha.Value;

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(cedula) ||
                string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(apellido))
            {
                MessageBox.Show("Por favor, complete los campos obligatorios.");
                return;
            }

            // Agregar datos al DataGridView
            dgvResultados.Rows.Add(cedula, nombre, apellido, telefono, fechaNacimiento.ToShortDateString(), correo);

            // Limpiar campos luego de guardar
            LimpiarCampos();

        }
        private void LimpiarCampos()
        {
            txtCedula.Clear();
            txtNombre.Clear();
            txtApellido.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            dtpFecha.Value = DateTime.Now;
            filaEditando = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvResultados.ColumnCount = 6;
            dgvResultados.Columns[0].Name = "Cédula";
            dgvResultados.Columns[1].Name = "Nombre";
            dgvResultados.Columns[2].Name = "Apellido";
            dgvResultados.Columns[3].Name = "Teléfono";
            dgvResultados.Columns[4].Name = "Fecha de Nacimiento";
            dgvResultados.Columns[5].Name = "Correo Electrónico";
        }

        private void txtCedula_TextChanged(object sender, EventArgs e)
        {
            // Eliminar caracteres que no sean dígitos
            string textoFiltrado = new string(txtCedula.Text.Where(char.IsDigit).ToArray());

            // Limitar a 10 dígitos
            if (textoFiltrado.Length > 10)
                textoFiltrado = textoFiltrado.Substring(0, 10);

            // Si hubo cambios, actualiza el texto del textbox
            if (txtCedula.Text != textoFiltrado)
            {
                int pos = txtCedula.SelectionStart - 1;
                txtCedula.Text = textoFiltrado;
                txtCedula.SelectionStart = Math.Max(0, pos);
            }

            // Mostrar error si no hay 10 dígitos (opcional)
            if (txtCedula.Text.Length < 10)
            {
                errorProvider1.SetError(txtCedula, "La cédula debe tener exactamente 10 números.");
            }
            else
            {
                errorProvider1.SetError(txtCedula, "");
            }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            string textoFiltrado = new string(txtNombre.Text.Where(c => char.IsLetter(c) || c == ' ').ToArray());

            // Convertir a título (solo la primera letra de cada palabra será mayúscula)
            textoFiltrado = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textoFiltrado.ToLower());

            // Si el texto filtrado ha cambiado, actualizar el texto en el TextBox
            if (txtNombre.Text != textoFiltrado)
            {
                txtNombre.Text = textoFiltrado;
                // Mantener la posición del cursor
                txtNombre.SelectionStart = txtNombre.Text.Length;
            }

        }

        private void txtApellido_TextChanged(object sender, EventArgs e)
        {
            string textoFiltrado = new string(txtApellido.Text.Where(c => char.IsLetter(c) || c == ' ').ToArray());

            // Convertir a título (solo la primera letra de cada palabra será mayúscula)
            textoFiltrado = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(textoFiltrado.ToLower());

            // Si el texto filtrado ha cambiado, actualizar el texto en el TextBox
            if (txtApellido.Text != textoFiltrado)
            {
                txtApellido.Text = textoFiltrado;
                // Mantener la posición del cursor
                txtApellido.SelectionStart = txtNombre.Text.Length;
            }
        }

        private void txtTelefono_TextChanged(object sender, EventArgs e)
        {
            // Eliminar caracteres que no sean dígitos
            string textoFiltrado = new string(txtTelefono.Text.Where(char.IsDigit).ToArray());

            // Forzar que comience con 0
            if (!string.IsNullOrEmpty(textoFiltrado) && textoFiltrado[0] != '0')
            {
                textoFiltrado = "0" + textoFiltrado;
            }

            // Limitar a 10 dígitos
            if (textoFiltrado.Length > 10)
                textoFiltrado = textoFiltrado.Substring(0, 10);

            // Si hubo cambios, actualiza el texto del textbox
            if (txtTelefono.Text != textoFiltrado)
            {
                int pos = txtTelefono.SelectionStart;
                txtTelefono.Text = textoFiltrado;
                txtTelefono.SelectionStart = Math.Min(txtTelefono.Text.Length, pos);
            }

            // Validación final
            if (txtTelefono.Text.Length < 10)
            {
                errorProvider1.SetError(txtTelefono, "El teléfono debe tener exactamente 10 números y comenzar con 0.");
            }
            else if (txtTelefono.Text[0] != '0')
            {
                errorProvider1.SetError(txtTelefono, "El teléfono debe comenzar con 0.");
            }
            else
            {
                errorProvider1.SetError(txtTelefono, "");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvResultados.SelectedRows.Count == 1)
            {
                DataGridViewRow row = dgvResultados.SelectedRows[0];

                // Llenar campos de texto con los datos de la fila seleccionada
                txtCedula.Text = row.Cells[0].Value.ToString();
                txtNombre.Text = row.Cells[1].Value.ToString();
                txtApellido.Text = row.Cells[2].Value.ToString();
                txtTelefono.Text = row.Cells[3].Value.ToString();
                dtpFecha.Value = DateTime.Parse(row.Cells[4].Value.ToString());
                txtCorreo.Text = row.Cells[5].Value.ToString();

                // Eliminar la fila seleccionada del DataGridView
                dgvResultados.Rows.Remove(row);
            }
            else
            {
                MessageBox.Show("Seleccione una fila para editar.");
            }

        }

        private void txtCorreo_TextChanged(object sender, EventArgs e)
        {
            // Filtrar: solo permitir minúsculas, números y caracteres válidos en correos
            string textoFiltrado = new string(txtCorreo.Text
                .Where(c => char.IsLower(c) || char.IsDigit(c) || c == '@' || c == '.' || c == '-' || c == '_')
                .ToArray());

            // Si el texto cambió, actualizar el contenido
            if (txtCorreo.Text != textoFiltrado)
            {
                int pos = txtCorreo.SelectionStart;
                txtCorreo.Text = textoFiltrado;
                txtCorreo.SelectionStart = Math.Min(txtCorreo.Text.Length, pos);
            }
        }
    }
}
