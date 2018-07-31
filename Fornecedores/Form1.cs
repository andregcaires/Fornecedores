using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fornecedores
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            AtualizarGrid();
        }

        #region Validar Campos
        public bool estaInvalido()
        {
            bool erro = false;
            string campos = "";
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                campos += "\n - nome";
                erro = true;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                campos += "\n - email";
                erro = true;
            }
            if (string.IsNullOrWhiteSpace(cbbTipo.Text))
            {
                campos += "\n - tipo";
                erro = true;
            }
            if (string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                campos += "\n - documento";
                erro = true;
            }
            if (string.IsNullOrWhiteSpace(txtTelefone.Text))
            {
                campos += "\n - telefone";
                erro = true;
            }

            if (erro)
            {
                MessageBox.Show("Campos invalidos: " + campos,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return erro;
            }            
            return erro;
        }
        #endregion

        #region Máscara Documento
        private void cbbTipo_Leave(object sender, EventArgs e)
        {
            if(cbbTipo.Text == "Física")
            {
                txtDocumento.Mask = "000,000,000-00";
            }
            else if(cbbTipo.Text == "Jurídica")
            {
                txtDocumento.Mask = "00,000,000/0000-00";
            }
            else
            {
                txtDocumento.Mask = "";
            }
        }
        #endregion

        #region Máscara Telefone
        private void txtTelefone_TextChanged(object sender, EventArgs e)
        {
            string telefone = txtTelefone.Text;
            telefone = telefone.Replace(" ", "");
            telefone = telefone.Replace("-", "");

            if(telefone.Length == 5)
            {
                char[] telarray = txtTelefone.Text.ToCharArray();
                if(telarray[5].ToString() != "9")
                {
                    txtTelefone.Mask = "(99) 0000-0000";
                }
                else
                {
                    txtTelefone.Mask = "(99) 00000-0000";
                }
            }
        }
        #endregion

        #region Limpa Tela
        public void Limpar()
        {
            txtNome.Clear();
            txtCodigo.Clear();
            txtDocumento.Clear();
            txtEmail.Clear();
            txtTelefone.Clear();
            cbbTipo.SelectedIndex = -1;
            txtFiltrar.Clear();
            AtualizarGrid();
        }
        #endregion

        #region Atualizar Grid
        private void AtualizarGrid()
        {
            SqlConnection conn = new SqlConnection();
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();

            da.SelectCommand = new SqlCommand();
            conn.ConnectionString = Dados.StringDeConexao;

            da.SelectCommand.CommandText = "select * from DadosCadastrais";

            conn.Open();
            da.SelectCommand.Connection = conn;
            da.Fill(dt);
            dgvFornecedores.DataSource = dt;
        }
        #endregion

        #region Inserir
        private void btnInserir_Click(object sender, EventArgs e)
        {
            if (estaInvalido())
            {
                return;
            }
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = Dados.StringDeConexao;
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = @"insert into DadosCadastrais (nome, documento, email, telefone, pessoa) values (@nome, @documento, @email, @telefone, @pessoa)";
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@documento", txtDocumento.Text);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
            cmd.Parameters.AddWithValue("@pessoa", cbbTipo.Text);
            cmd.Connection = conn;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item inserido com sucesso!",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }catch(Exception err)
            {
                MessageBox.Show("Erro na comunicação com banco de dados: "+ err.ToString(),
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            Limpar();
        }
        #endregion

        #region Alterar
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Selecione um item para atualizar", 
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = Dados.StringDeConexao;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"update DadosCadastras set nome = @nome, documento = @documento, email = @email, telefone = @telefone, pessoa = @pessoa where codigo = @codigo";
            cmd.Parameters.AddWithValue("@nome", txtNome.Text);
            cmd.Parameters.AddWithValue("@documento", txtDocumento.Text);
            cmd.Parameters.AddWithValue("@email", txtEmail.Text);
            cmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
            cmd.Parameters.AddWithValue("@pessoa", cbbTipo.Text);
            cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item atualizado com sucesso!",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show("Erro na comunicação com banco de dados: " + err.ToString(),
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            Limpar();
        }
        #endregion

        #region Excluir
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Selecione um item para excluir",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = Dados.StringDeConexao;
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = @"delete from DadosCadastrais where codigo = @codigo";
            cmd.Parameters.AddWithValue("@codigo", txtCodigo.Text);
            
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item excluído com sucesso!",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show("Erro na comunicação com banco de dados: " + err.ToString(),
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }

            Limpar();
        }
        #endregion

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFiltrar.Text))
            {
                MessageBox.Show("Informe um nome para filtrar",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }

        private void dgvFornecedores_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCodigo.Text = dgvFornecedores[0, dgvFornecedores.CurrentRow.Index].Value.ToString();
            txtNome.Text = dgvFornecedores[1, dgvFornecedores.CurrentRow.Index].Value.ToString();
            txtEmail.Text = dgvFornecedores[2, dgvFornecedores.CurrentRow.Index].Value.ToString();
            txtTelefone.Text = dgvFornecedores[3, dgvFornecedores.CurrentRow.Index].Value.ToString();
        }
    }
}
