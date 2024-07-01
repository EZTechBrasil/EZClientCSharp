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
            this.label3 = new System.Windows.Forms.Label();
            this.buttonTankDrops = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.PeriodTypesCB = new System.Windows.Forms.ComboBox();
            this.PrintPeriodBN = new System.Windows.Forms.Button();
            this.ClosePeriodBN = new System.Windows.Forms.Button();
            this.buttonTankHistory = new System.Windows.Forms.Button();
            this.buttonReadTanks = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "EZForecourt";
            // 
            // edServerAddress
            // 
            this.edServerAddress.Location = new System.Drawing.Point(126, 6);
            this.edServerAddress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edServerAddress.Name = "edServerAddress";
            this.edServerAddress.Size = new System.Drawing.Size(469, 26);
            this.edServerAddress.TabIndex = 1;
            this.edServerAddress.Text = "localhost";
            // 
            // chProcEvents
            // 
            this.chProcEvents.AutoSize = true;
            this.chProcEvents.Location = new System.Drawing.Point(627, 11);
            this.chProcEvents.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chProcEvents.Name = "chProcEvents";
            this.chProcEvents.Size = new System.Drawing.Size(195, 24);
            this.chProcEvents.TabIndex = 2;
            this.chProcEvents.Text = "Processar por Eventos";
            this.chProcEvents.UseVisualStyleBackColor = true;
            // 
            // timerAppLoop
            // 
            this.timerAppLoop.Enabled = true;
            this.timerAppLoop.Interval = 300;
            this.timerAppLoop.Tick += new System.EventHandler(this.timerAppLoop_Tick);
            // 
            // btLogon
            // 
            this.btLogon.Location = new System.Drawing.Point(19, 49);
            this.btLogon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btLogon.Name = "btLogon";
            this.btLogon.Size = new System.Drawing.Size(180, 35);
            this.btLogon.TabIndex = 3;
            this.btLogon.Text = "Logon";
            this.btLogon.UseVisualStyleBackColor = true;
            this.btLogon.Click += new System.EventHandler(this.btLogon_Click);
            // 
            // btCheckConnection
            // 
            this.btCheckConnection.Location = new System.Drawing.Point(207, 49);
            this.btCheckConnection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btCheckConnection.Name = "btCheckConnection";
            this.btCheckConnection.Size = new System.Drawing.Size(180, 35);
            this.btCheckConnection.TabIndex = 4;
            this.btCheckConnection.Text = "Verificar Conexão";
            this.btCheckConnection.UseVisualStyleBackColor = true;
            this.btCheckConnection.Click += new System.EventHandler(this.btCheckConnection_Click);
            // 
            // btLoadConfig
            // 
            this.btLoadConfig.Location = new System.Drawing.Point(396, 49);
            this.btLoadConfig.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btLoadConfig.Name = "btLoadConfig";
            this.btLoadConfig.Size = new System.Drawing.Size(180, 35);
            this.btLoadConfig.TabIndex = 5;
            this.btLoadConfig.Text = "Ler Configuração";
            this.btLoadConfig.UseVisualStyleBackColor = true;
            this.btLoadConfig.Click += new System.EventHandler(this.btLoadConfig_Click);
            // 
            // btGetAllDeliveries
            // 
            this.btGetAllDeliveries.Location = new System.Drawing.Point(585, 49);
            this.btGetAllDeliveries.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btGetAllDeliveries.Name = "btGetAllDeliveries";
            this.btGetAllDeliveries.Size = new System.Drawing.Size(180, 35);
            this.btGetAllDeliveries.TabIndex = 6;
            this.btGetAllDeliveries.Text = "Ler Abastecimentos";
            this.btGetAllDeliveries.UseVisualStyleBackColor = true;
            this.btGetAllDeliveries.Click += new System.EventHandler(this.btGetAllDeliveries_Click);
            // 
            // btClearMessages
            // 
            this.btClearMessages.Location = new System.Drawing.Point(774, 49);
            this.btClearMessages.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btClearMessages.Name = "btClearMessages";
            this.btClearMessages.Size = new System.Drawing.Size(180, 35);
            this.btClearMessages.TabIndex = 7;
            this.btClearMessages.Text = "Limpar Mensagens";
            this.btClearMessages.UseVisualStyleBackColor = true;
            this.btClearMessages.Click += new System.EventHandler(this.btClearMessages_Click);
            // 
            // cbPump
            // 
            this.cbPump.FormattingEnabled = true;
            this.cbPump.Location = new System.Drawing.Point(132, 97);
            this.cbPump.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbPump.Name = "cbPump";
            this.cbPump.Size = new System.Drawing.Size(179, 28);
            this.cbPump.TabIndex = 8;
            // 
            // cbHose
            // 
            this.cbHose.FormattingEnabled = true;
            this.cbHose.Location = new System.Drawing.Point(585, 94);
            this.cbHose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbHose.Name = "cbHose";
            this.cbHose.Size = new System.Drawing.Size(179, 28);
            this.cbHose.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 99);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "Bomba / PA";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 140);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "Preço 1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(510, 141);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 20);
            this.label5.TabIndex = 15;
            this.label5.Text = "Preço 2";
            // 
            // btChangePrice
            // 
            this.btChangePrice.Location = new System.Drawing.Point(774, 129);
            this.btChangePrice.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btChangePrice.Name = "btChangePrice";
            this.btChangePrice.Size = new System.Drawing.Size(180, 35);
            this.btChangePrice.TabIndex = 16;
            this.btChangePrice.Text = "Trocar Preço";
            this.btChangePrice.UseVisualStyleBackColor = true;
            this.btChangePrice.Click += new System.EventHandler(this.btChangePrice_Click);
            // 
            // edPrice1
            // 
            this.edPrice1.Location = new System.Drawing.Point(130, 135);
            this.edPrice1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edPrice1.Mask = "#.000";
            this.edPrice1.Name = "edPrice1";
            this.edPrice1.Size = new System.Drawing.Size(179, 26);
            this.edPrice1.TabIndex = 17;
            this.edPrice1.Text = "0000";
            this.edPrice1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // edPrice2
            // 
            this.edPrice2.Location = new System.Drawing.Point(584, 137);
            this.edPrice2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edPrice2.Mask = "#.000";
            this.edPrice2.Name = "edPrice2";
            this.edPrice2.Size = new System.Drawing.Size(179, 26);
            this.edPrice2.TabIndex = 18;
            this.edPrice2.Text = "0000";
            this.edPrice2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cbPresetType
            // 
            this.cbPresetType.FormattingEnabled = true;
            this.cbPresetType.Location = new System.Drawing.Point(129, 175);
            this.cbPresetType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbPresetType.Name = "cbPresetType";
            this.cbPresetType.Size = new System.Drawing.Size(179, 28);
            this.cbPresetType.TabIndex = 19;
            // 
            // edPreset
            // 
            this.edPreset.Location = new System.Drawing.Point(584, 177);
            this.edPreset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edPreset.Mask = "##0.000";
            this.edPreset.Name = "edPreset";
            this.edPreset.Size = new System.Drawing.Size(179, 26);
            this.edPreset.TabIndex = 20;
            this.edPreset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btPreset
            // 
            this.btPreset.Location = new System.Drawing.Point(774, 175);
            this.btPreset.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btPreset.Name = "btPreset";
            this.btPreset.Size = new System.Drawing.Size(180, 35);
            this.btPreset.TabIndex = 21;
            this.btPreset.Text = "Predeterminação";
            this.btPreset.UseVisualStyleBackColor = true;
            this.btPreset.Click += new System.EventHandler(this.btPreset_Click);
            // 
            // btAuthorize
            // 
            this.btAuthorize.Location = new System.Drawing.Point(18, 225);
            this.btAuthorize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btAuthorize.Name = "btAuthorize";
            this.btAuthorize.Size = new System.Drawing.Size(180, 35);
            this.btAuthorize.TabIndex = 22;
            this.btAuthorize.Text = "Autoriza Bomba";
            this.btAuthorize.UseVisualStyleBackColor = true;
            this.btAuthorize.Click += new System.EventHandler(this.btAuthorize_Click);
            // 
            // btLock
            // 
            this.btLock.Location = new System.Drawing.Point(207, 225);
            this.btLock.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btLock.Name = "btLock";
            this.btLock.Size = new System.Drawing.Size(180, 35);
            this.btLock.TabIndex = 23;
            this.btLock.Text = "Bloqueia Bomba";
            this.btLock.UseVisualStyleBackColor = true;
            this.btLock.Click += new System.EventHandler(this.btLock_Click);
            // 
            // btTotals
            // 
            this.btTotals.Location = new System.Drawing.Point(396, 225);
            this.btTotals.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btTotals.Name = "btTotals";
            this.btTotals.Size = new System.Drawing.Size(180, 35);
            this.btTotals.TabIndex = 24;
            this.btTotals.Text = "Ler Encerrantes";
            this.btTotals.UseVisualStyleBackColor = true;
            this.btTotals.Click += new System.EventHandler(this.btTotals_Click);
            // 
            // btReadCards
            // 
            this.btReadCards.Location = new System.Drawing.Point(585, 225);
            this.btReadCards.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btReadCards.Name = "btReadCards";
            this.btReadCards.Size = new System.Drawing.Size(180, 35);
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
            this.listMessageBox.ItemHeight = 20;
            this.listMessageBox.Location = new System.Drawing.Point(17, 269);
            this.listMessageBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listMessageBox.Name = "listMessageBox";
            this.listMessageBox.Size = new System.Drawing.Size(1101, 484);
            this.listMessageBox.TabIndex = 26;
            // 
            // buttonFinalizarAbastecimento
            // 
            this.buttonFinalizarAbastecimento.Location = new System.Drawing.Point(774, 225);
            this.buttonFinalizarAbastecimento.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonFinalizarAbastecimento.Name = "buttonFinalizarAbastecimento";
            this.buttonFinalizarAbastecimento.Size = new System.Drawing.Size(180, 35);
            this.buttonFinalizarAbastecimento.TabIndex = 27;
            this.buttonFinalizarAbastecimento.Text = "Finalizar Abast.";
            this.buttonFinalizarAbastecimento.UseVisualStyleBackColor = true;
            this.buttonFinalizarAbastecimento.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonDesativar
            // 
            this.buttonDesativar.Location = new System.Drawing.Point(969, 42);
            this.buttonDesativar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonDesativar.Name = "buttonDesativar";
            this.buttonDesativar.Size = new System.Drawing.Size(150, 35);
            this.buttonDesativar.TabIndex = 28;
            this.buttonDesativar.Text = "Desativar Bico";
            this.buttonDesativar.UseVisualStyleBackColor = true;
            this.buttonDesativar.Click += new System.EventHandler(this.buttonDesativar_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(534, 99);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 20);
            this.label3.TabIndex = 30;
            this.label3.Text = "Bico";
            // 
            // buttonTankDrops
            // 
            this.buttonTankDrops.Location = new System.Drawing.Point(969, 120);
            this.buttonTankDrops.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonTankDrops.Name = "buttonTankDrops";
            this.buttonTankDrops.Size = new System.Drawing.Size(150, 35);
            this.buttonTankDrops.TabIndex = 31;
            this.buttonTankDrops.Text = "Entregas";
            this.buttonTankDrops.UseVisualStyleBackColor = true;
            this.buttonTankDrops.Click += new System.EventHandler(this.buttonConsultarEntrega_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(842, 90);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 35);
            this.button1.TabIndex = 32;
            this.button1.Text = "Ver Preco";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonSeePrice_Click);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(969, 196);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(148, 63);
            this.button2.TabIndex = 33;
            this.button2.Text = "Ler Produtos Complementares";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.LerProdutosComplementares_Click);
            // 
            // PeriodTypesCB
            // 
            this.PeriodTypesCB.FormattingEnabled = true;
            this.PeriodTypesCB.Items.AddRange(new object[] {
            "Shift",
            "Day",
            "Month"});
            this.PeriodTypesCB.Location = new System.Drawing.Point(329, 97);
            this.PeriodTypesCB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PeriodTypesCB.Name = "PeriodTypesCB";
            this.PeriodTypesCB.Size = new System.Drawing.Size(179, 28);
            this.PeriodTypesCB.TabIndex = 34;
            // 
            // PrintPeriodBN
            // 
            this.PrintPeriodBN.Location = new System.Drawing.Point(329, 129);
            this.PrintPeriodBN.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PrintPeriodBN.Name = "PrintPeriodBN";
            this.PrintPeriodBN.Size = new System.Drawing.Size(180, 35);
            this.PrintPeriodBN.TabIndex = 35;
            this.PrintPeriodBN.Text = "Print period";
            this.PrintPeriodBN.UseVisualStyleBackColor = true;
            this.PrintPeriodBN.Click += new System.EventHandler(this.PrintPeriodBN_Click);
            // 
            // ClosePeriodBN
            // 
            this.ClosePeriodBN.Location = new System.Drawing.Point(329, 172);
            this.ClosePeriodBN.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ClosePeriodBN.Name = "ClosePeriodBN";
            this.ClosePeriodBN.Size = new System.Drawing.Size(180, 35);
            this.ClosePeriodBN.TabIndex = 36;
            this.ClosePeriodBN.Text = "Close period";
            this.ClosePeriodBN.UseVisualStyleBackColor = true;
            this.ClosePeriodBN.Click += new System.EventHandler(this.ClosePeriodBN_Click);
            // 
            // buttonTankHistory
            // 
            this.buttonTankHistory.Location = new System.Drawing.Point(967, 159);
            this.buttonTankHistory.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonTankHistory.Name = "buttonTankHistory";
            this.buttonTankHistory.Size = new System.Drawing.Size(150, 35);
            this.buttonTankHistory.TabIndex = 37;
            this.buttonTankHistory.Text = "Tank history";
            this.buttonTankHistory.UseVisualStyleBackColor = true;
            this.buttonTankHistory.Click += new System.EventHandler(this.buttonConsultarHistorico_Click);
            // 
            // buttonReadTanks
            // 
            this.buttonReadTanks.Location = new System.Drawing.Point(969, 80);
            this.buttonReadTanks.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonReadTanks.Name = "buttonReadTanks";
            this.buttonReadTanks.Size = new System.Drawing.Size(150, 35);
            this.buttonReadTanks.TabIndex = 38;
            this.buttonReadTanks.Text = "Ler Tanques";
            this.buttonReadTanks.UseVisualStyleBackColor = true;
            this.buttonReadTanks.Click += new System.EventHandler(this.buttonReadTanks_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 781);
            this.Controls.Add(this.buttonReadTanks);
            this.Controls.Add(this.buttonTankHistory);
            this.Controls.Add(this.ClosePeriodBN);
            this.Controls.Add(this.PrintPeriodBN);
            this.Controls.Add(this.PeriodTypesCB);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonTankDrops);
            this.Controls.Add(this.label3);
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
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
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
        //private System.Windows.Forms.Label label3;
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonTankDrops;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ComboBox PeriodTypesCB;
        private System.Windows.Forms.Button PrintPeriodBN;
        private System.Windows.Forms.Button ClosePeriodBN;
        private System.Windows.Forms.Button buttonTankHistory;
        private System.Windows.Forms.Button buttonReadTanks;
    }
}

