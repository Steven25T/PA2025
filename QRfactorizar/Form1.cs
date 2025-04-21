using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRfactorizar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtFila.Text, out int n) && n > 0 &&
         int.TryParse(txtColumna.Text, out int m) && m > 0)
            {
                dataGridView.Rows.Clear();
                dataGridView.Columns.Clear();
                dataGridView.AllowUserToAddRows = false; // Evita que se agreguen filas automáticamente

                // Crear columnas
                for (int i = 0; i < m; i++)
                {
                    dataGridView.Columns.Add($"col{i}", $"Col {i + 1}");
                    dataGridView.Columns[i].Width = 50;
                }

                // Agregar filas sin permitir que se agreguen solas
                dataGridView.RowCount = n;
            }
            else
            {
                MessageBox.Show("Ingresa valores válidos para filas y columnas.");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int n = dataGridView.RowCount;
            int m = dataGridView.ColumnCount;

            if (n == 0 || m == 0)
            {
                MessageBox.Show("Por favor, ingresa valores en la matriz.");
                return;
            }

            double[,] matriz = new double[n, m];

            try
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        var cellValue = dataGridView.Rows[i].Cells[j].Value?.ToString().Trim();
                        if (string.IsNullOrEmpty(cellValue) || !double.TryParse(cellValue, NumberStyles.Any, CultureInfo.InvariantCulture, out matriz[i, j]))
                        {
                            dataGridView.Rows[i].Cells[j].Style.BackColor = Color.Red; // Marca la celda en rojo
                            MessageBox.Show($"Error en la celda ({i + 1}, {j + 1}). Asegúrate de que sea un número válido.");
                            return;
                        }
                        else
                        {
                            dataGridView.Rows[i].Cells[j].Style.BackColor = Color.White; // Restablece el color si es válido
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error inesperado: {ex.Message}");
                return;
            }

            Stopwatch sw = Stopwatch.StartNew();
            GramSchmidt(matriz, out double[,] Q, out double[,] R);
            sw.Stop();

            MostrarMatrizEnDataGridView(Q, dataGridViewQ);
            MostrarMatrizEnDataGridView(R, dataGridViewR);
            MessageBox.Show($"Cálculo completado en {sw.Elapsed.TotalSeconds:F4} segundos.");
        }
        private void GramSchmidt(double[,] A, out double[,] Q, out double[,] R)
        {
            int m = A.GetLength(0); // Filas
            int n = A.GetLength(1); // Columnas

            // Validar si la matriz es compatible con QR
            if (m < n)
            {
                MessageBox.Show("El número de filas debe ser mayor o igual al número de columnas para la factorización QR.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Q = null;
                R = null;
                return;
            }

            Q = new double[m, n];
            R = new double[n, n];
            double[,] V = new double[m, n];

            // Copiar A en V
            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    V[i, j] = A[i, j];

            for (int k = 0; k < n; k++)
            {
                double norm = 0;

                // Calcular norma de la columna k de V
                for (int i = 0; i < m; i++)
                    norm += V[i, k] * V[i, k];

                norm = Math.Sqrt(norm);

                // Verificar si la columna es casi cero (evitar error de dependencia lineal)
                if (norm < 1e-10)
                {
                    MessageBox.Show($"La columna {k + 1} es linealmente dependiente o demasiado pequeña para continuar la factorización.",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Q = null;
                    R = null;
                    return;
                }

                R[k, k] = norm;

                // Normalizar columna k
                for (int i = 0; i < m; i++)
                    Q[i, k] = V[i, k] / norm;

                // Ortogonalizar las columnas restantes
                for (int j = k + 1; j < n; j++)
                {
                    double dot = 0;
                    for (int i = 0; i < m; i++)
                        dot += Q[i, k] * V[i, j];

                    R[k, j] = dot;

                    for (int i = 0; i < m; i++)
                        V[i, j] -= dot * Q[i, k];
                }
            }

        }
        private void MostrarMatrizEnDataGridView(double[,] matriz, DataGridView dataGridView)
        {
            dataGridView.RowCount = matriz.GetLength(0);
            dataGridView.ColumnCount = matriz.GetLength(1);

            for (int i = 0; i < matriz.GetLength(0); i++)
                for (int j = 0; j < matriz.GetLength(1); j++)
                    dataGridView.Rows[i].Cells[j].Value = matriz[i, j].ToString("F4");
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            double[,] Q = {
       
    }; 
            int m = Q.GetLength(0);
            int n = Q.GetLength(1);

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    double dotProduct = 0;
                    for (int k = 0; k < m; k++)
                        dotProduct += Q[k, i] * Q[k, j];

                    if ((i == j && Math.Abs(dotProduct - 1) > 1e-6) || (i != j && Math.Abs(dotProduct) > 1e-6))
                    {
                        MessageBox.Show("Las columnas de Q no son ortogonales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            MessageBox.Show("La matriz Q es ortogonal.", "Verificación", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dataGridViewQ_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            txtFila.Clear();
            txtColumna.Clear();
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();
            dataGridViewQ.Rows.Clear();
            dataGridViewQ.Columns.Clear();
            dataGridViewR.Rows.Clear();
            dataGridViewR.Columns.Clear();
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
    
}
