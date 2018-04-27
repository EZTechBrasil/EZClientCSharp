namespace EZClientCSharp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.edServerAddress = new System.Windows.Forms.TextBox();
            this.chProcEvents = new System.Windows.Forms.CheckBox();
            this.timerAppLoop = new System.Windows.Forms.Timer(this.components);
            this.btLogon = new System.Windows.Forms.Button();
            this.btCheckConnection = new System.Windows.Forms.Button();
            this.btLoadConfig = new System.Windows.Forms.Button();
            this.btGetAllDeliveries = new System.Windows.Forms.Button();
            this.btClearMessages = new System.Windows.Forms.Button();
            this.cbPump = new System.Windows.Forms.ComboBox();
            this.cbHose = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btChangePrice = new System.Windows.Forms.Button();
            this.edPrice1 = new System.Windows.Forms.MaskedTextBox();
            this.edPrice2 = new System.Windows.Forms.MaskedTextBox();
            this.cbPresetType = new System.Windows.Forms.ComboBox();
            this.edPreset = new System.Windows.Forms.MaskedTextBox();
            this.btPreset = new System.Windows.Forms.Button();
            this.btAuthorize = new System.Windows.Forms.Button();
            this.btLock = new System.Windows.Forms.Button();
            this.btTotals = new System.Windows.Forms.Button();
            this.btReadCards = new System.Windows.Forms.Button();
            this.listMessageBox = new System.Windows.Forms.ListBox();
            this.buttonFinalizarAbastecimento = new System.Windows.Forms.Button();
            this.buttonDesativar = new System.Windows.Forms.Button();
            this.teste = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "EZForecourt";
            // 
            // edServerAddress
            // 
            this.edServerAddress.Location = new System.Drawing.Point(84, 4);
            this.edServerAddress.Name = "edServerAddress";
            this.edServerAddress.Size = new System.Drawing.Size(314, 20);
            this.edServerAddress.TabIndex = 1;
            this.edServerAddress.Text = "localhost";
            this.edServerAddress.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // chProcEvents
            // 
            this.chProcEvents.AutoSize = true;
            this.chProcEvents.Location = new System.Drawing.Point(418, 7);
            this.chProcEvents.Name = "chProcEvents";
            this.chProcEvents.Size = new System.Drawing.Size(133, 17);
            this.chProcEvents.TabIndex = 2;
            this.chProcEvents.Text = "Processar por Eventos";
            this.chProcEvents.UseVisualStyleBackColor = true;
            this.chProcEvents.CheckedChanged += new System.EventHandler(this.chProcEvents_CheckedChanged);
            // 
            // timerAppLoop
            // 
            this.timerAppLoop.Enabled = true;
            this.timerAppLoop.Interval = 300;
            this.timerAppLoop.Tick += new System.EventHandler(this.timerAppLoop_Tick);
            // 
            // btLogon
            // 
            this.btLogon.Location = new System.Drawing.Point(12, 32);
            this.btLogon.Name = "btLogon";
            this.btLogon.Size = new System.Drawing.Size(120, 23);
            this.btLogon.TabIndex = 3;
            this.btLogon.Text = "Logon";
            this.btLogon.UseVisualStyleBackColor = true;
            this.btLogon.Click += new System.EventHandler(this.btLogon_Click);
            // 
            // btCheckConnection
            // 
            this.btCheckConnection.Location = new System.Drawing.Point(138, 32);
            this.btCheckConnection.Name = "btCheckConnection";
            this.btCheckConnection.Size = new System.Drawing.Size(120, 23);
            this.btCheckConnection.TabIndex = 4;
            this.btCheckConnection.Text = "Verificar Conexão";
            this.btCheckConnection.UseVisualStyleBackColor = true;
            this.btCheckConnection.Click += new System.EventHandler(this.btCheckConnection_Click);
            // 
            // btLoadConfig
            // 
            this.btLoadConfig.Location = new System.Drawing.Point(264, 32);
            this.btLoadConfig.Name = "btLoadConfig";
            this.btLoadConfig.Size = new System.Drawing.Size(120, 23);
            this.btLoadConfig.TabIndex = 5;
            this.btLoadConfig.Text = "Ler Configuração";
            this.btLoadConfig.UseVisualStyleBackColor = true;
            this.btLoadConfig.Click += new System.EventHandler(this.btLoadConfig_Click);
            // 
            // btGetAllDeliveries
            // 
            this.btGetAllDeliveries.Location = new System.Drawing.Point(390, 32);
            this.btGetAllDeliveries.Name = "btGetAllDeliveries";
            this.btGetAllDeliveries.Size = new System.Drawing.Size(120, 23);
            this.btGetAllDeliveries.TabIndex = 6;
            this.btGetAllDeliveries.Text = "Ler Abastecimentos";
            this.btGetAllDeliveries.UseVisualStyleBackColor = true;
            this.btGetAllDeliveries.Click += new System.EventHandler(this.btGetAllDeliveries_Click);
            // 
            // btClearMessages
            // 
            this.btClearMessages.Location = new System.Drawing.Point(516, 32);
            this.btClearMessages.Name = "btClearMessages";
            this.btClearMessages.Size = new System.Drawing.Size(120, 23);
            this.btClearMessages.TabIndex = 7;
            this.btClearMessages.Text = "Limpar Mensagens";
            this.btClearMessages.UseVisualStyleBackColor = true;
            this.btClearMessages.Click += new System.EventHandler(this.btClearMessages_Click);
            // 
            // cbPump
            // 
            this.cbPump.FormattingEnabled = true;
            this.cbPump.Location = new System.Drawing.Point(137, 61);
            this.cbPump.Name = "cbPump";
            this.cbPump.Size = new System.Drawing.Size(121, 21);
            this.cbPump.TabIndex = 8;
            // 
            // cbHose
            // 
            this.cbHose.FormattingEnabled = true;
            this.cbHose.Location = new System.Drawing.Point(390, 61);
            this.cbHose.Name = "cbHose";
            this.cbHose.Size = new System.Drawing.Size(121, 21);
            this.cbHose.TabIndex = 9;
            this.cbHose.SelectedIndexChanged += new System.EventHandler(this.cbHose_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(91, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Bomba";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(356, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Bico";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(91, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Preço 1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(340, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Preço 2";
            // 
            // btChangePrice
            // 
            this.btChangePrice.Location = new System.Drawing.Point(516, 84);
            this.btChangePrice.Name = "btChangePrice";
            this.btChangePrice.Size = new System.Drawing.Size(120, 23);
            this.btChangePrice.TabIndex = 16;
            this.btChangePrice.Text = "Trocar Preço";
            this.btChangePrice.UseVisualStyleBackColor = true;
            this.btChangePrice.Click += new System.EventHandler(this.btChangePrice_Click);
            // 
            // edPrice1
            // 
            this.edPrice1.Location = new System.Drawing.Point(136, 88);
            this.edPrice1.Mask = "#.000";
            this.edPrice1.Name = "edPrice1";
            this.edPrice1.Size = new System.Drawing.Size(121, 20);
            this.edPrice1.TabIndex = 17;
            this.edPrice1.Text = "0000";
            this.edPrice1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // edPrice2
            // 
            this.edPrice2.Location = new System.Drawing.Point(389, 89);
            this.edPrice2.Mask = "#.000";
            this.edPrice2.Name = "edPrice2";
            this.edPrice2.Size = new System.Drawing.Size(121, 20);
            this.edPrice2.TabIndex = 18;
            this.edPrice2.Text = "0000";
            this.edPrice2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbPresetType
            // 
            this.cbPresetType.FormattingEnabled = true;
            this.cbPresetType.Location = new System.Drawing.Point(135, 114);
            this.cbPresetType.Name = "cbPresetType";
            this.cbPresetType.Size = new System.Drawing.Size(121, 21);
            this.cbPresetType.TabIndex = 19;
            // 
            // edPreset
            // 
            this.edPreset.Location = new System.Drawing.Point(389, 115);
            this.edPreset.Mask = "##0.000";
            this.edPreset.Name = "edPreset";
            this.edPreset.Size = new System.Drawing.Size(121, 20);
            this.edPreset.TabIndex = 20;
            this.edPreset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btPreset
            // 
            this.btPreset.Location = new System.Drawing.Point(516, 114);
            this.btPreset.Name = "btPreset";
            this.btPreset.Size = new System.Drawing.Size(120, 23);
            this.btPreset.TabIndex = 21;
            this.btPreset.Text = "Predeterminação";
            this.btPreset.UseVisualStyleBackColor = true;
            this.btPreset.Click += new System.EventHandler(this.btPreset_Click);
            // 
            // btAuthorize
            // 
            this.btAuthorize.Location = new System.Drawing.Point(12, 146);
            this.btAuthorize.Name = "btAuthorize";
            this.btAuthorize.Size = new System.Drawing.Size(120, 23);
            this.btAuthorize.TabIndex = 22;
            this.btAuthorize.Text = "Autoriza Bomba";
            this.btAuthorize.UseVisualStyleBackColor = true;
            this.btAuthorize.Click += new System.EventHandler(this.btAuthorize_Click);
            // 
            // btLock
            // 
            this.btLock.Location = new System.Drawing.Point(138, 146);
            this.btLock.Name = "btLock";
            this.btLock.Size = new System.Drawing.Size(120, 23);
            this.btLock.TabIndex = 23;
            this.btLock.Text = "Bloqueia Bomba";
            this.btLock.UseVisualStyleBackColor = true;
            this.btLock.Click += new System.EventHandler(this.btLock_Click);
            // 
            // btTotals
            // 
            this.btTotals.Location = new System.Drawing.Point(264, 146);
            this.btTotals.Name = "btTotals";
            this.btTotals.Size = new System.Drawing.Size(120, 23);
            this.btTotals.TabIndex = 24;
            this.btTotals.Text = "Ler Encerrantes";
            this.btTotals.UseVisualStyleBackColor = true;
            this.btTotals.Click += new System.EventHandler(this.btTotals_Click);
            // 
            // btReadCards
            // 
            this.btReadCards.Location = new System.Drawing.Point(390, 146);
            this.btReadCards.Name = "btReadCards";
            this.btReadCards.Size = new System.Drawing.Size(120, 23);
            this.btReadCards.TabIndex = 25;
            this.btReadCards.Text = "Ler Cartões";
            this.btReadCards.UseVisualStyleBackColor = true;
            this.btReadCards.Click += new System.EventHandler(this.btReadCards_Click);
            // 
            // listMessageBox
            // 
            this.listMessageBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listMessageBox.FormattingEnabled = true;
            this.listMessageBox.HorizontalScrollbar = true;
            this.listMessageBox.Location = new System.Drawing.Point(11, 175);
            this.listMessageBox.Name = "listMessageBox";
            this.listMessageBox.Size = new System.Drawing.Size(751, 316);
            this.listMessageBox.TabIndex = 26;
            this.listMessageBox.SelectedIndexChanged += new System.EventHandler(this.listMessageBox_SelectedIndexChanged);
            // 
            // buttonFinalizarAbastecimento
            // 
            this.buttonFinalizarAbastecimento.Location = new System.Drawing.Point(516, 146);
            this.buttonFinalizarAbastecimento.Name = "buttonFinalizarAbastecimento";
            this.buttonFinalizarAbastecimento.Size = new System.Drawing.Size(120, 23);
            this.buttonFinalizarAbastecimento.TabIndex = 27;
            this.buttonFinalizarAbastecimento.Text = "Finalizar Abast.";
            this.buttonFinalizarAbastecimento.UseVisualStyleBackColor = true;
            this.buttonFinalizarAbastecimento.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonDesativar
            // 
            this.buttonDesativar.Location = new System.Drawing.Point(645, 59);
            this.buttonDesativar.Name = "buttonDesativar";
            this.buttonDesativar.Size = new System.Drawing.Size(100, 23);
            this.buttonDesativar.TabIndex = 28;
            this.buttonDesativar.Text = "Desativar Bico";
            this.buttonDesativar.UseVisualStyleBackColor = true;
            this.buttonDesativar.Click += new System.EventHandler(this.buttonDesativar_Click);
            // 
            // teste
            // 
            this.teste.Location = new System.Drawing.Point(657, 91);
            this.teste.Name = "teste";
            this.teste.Size = new System.Drawing.Size(75, 23);
            this.teste.TabIndex = 29;
            this.teste.Text = "teste";
            this.teste.UseVisualStyleBackColor = false;
            this.teste.Click += new System.EventHandler(this.teste_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 508);
            this.Controls.Add(this.teste);
            this.Controls.Add(this.buttonDesativar);
            this.Controls.Add(this.buttonFinalizarAbastecimento);
            this.Controls.Add(this.listMessageBox);
            this.Controls.Add(this.btReadCards);
            this.Controls.Add(this.btTotals);
            this.Controls.Add(this.btLock);
            this.Controls.Add(this.btAuthorize);
            this.Controls.Add(this.btPreset);
            this.Controls.Add(this.edPreset);
            this.Controls.Add(this.cbPresetType);
            this.Controls.Add(this.edPrice2);
            this.Controls.Add(this.edPrice1);
            this.Controls.Add(this.btChangePrice);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbHose);
            this.Controls.Add(this.cbPump);
            this.Controls.Add(this.btClearMessages);
            this.Controls.Add(this.btGetAllDeliveries);
            this.Controls.Add(this.btLoadConfig);
            this.Controls.Add(this.btCheckConnection);
            this.Controls.Add(this.btLogon);
            this.Controls.Add(this.chProcEvents);
            this.Controls.Add(this.edServerAddress);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "EZClient C# Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox edServerAddress;
        private System.Windows.Forms.CheckBox chProcEvents;
        private System.Windows.Forms.Timer timerAppLoop;
        private System.Windows.Forms.Button btLogon;
        private System.Windows.Forms.Button btCheckConnection;
        private System.Windows.Forms.Button btLoadConfig;
        private System.Windows.Forms.Button btGetAllDeliveries;
        private System.Windows.Forms.Button btClearMessages;
        private System.Windows.Forms.ComboBox cbPump;
        private System.Windows.Forms.ComboBox cbHose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btChangePrice;
        private System.Windows.Forms.MaskedTextBox edPrice1;
        private System.Windows.Forms.MaskedTextBox edPrice2;
        private System.Windows.Forms.ComboBox cbPresetType;
        private System.Windows.Forms.MaskedTextBox edPreset;
        private System.Windows.Forms.Button btPreset;
        private System.Windows.Forms.Button btAuthorize;
        private System.Windows.Forms.Button btLock;
        private System.Windows.Forms.Button btTotals;
        private System.Windows.Forms.Button btReadCards;
        private System.Windows.Forms.ListBox listMessageBox;
        private System.Windows.Forms.Button buttonFinalizarAbastecimento;
        private System.Windows.Forms.Button buttonDesativar;
        private System.Windows.Forms.Button teste;
    }
}

