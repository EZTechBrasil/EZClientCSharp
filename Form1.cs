using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EZClientCSharp
{
    public partial class Form1 : Form
    {
        private byte[] lastStates = new byte[100];

        private int MyClientID = 35;


        public Form1()
        {
            InitializeComponent();
        }

        #region Carregamento do form
        private void Form1_Load(object sender, EventArgs e)
        {
            Int32 ct = 0;
            String version = "";

            edServerAddress.Text = "192.168.1.111";

            // Carrega lista de bombas
            for (ct = 1; ct < 32; ct++)
                cbPump.Items.Add(ct);

            cbPump.SelectedIndex = 0;

            // Carrega lista de bicos
            for (ct = 1; ct <= 8; ct++)
                cbHose.Items.Add(ct);

            cbHose.SelectedIndex = 0;

            // Preenche lista de tipos de preset
            cbPresetType.Items.Add("Dinheiro");
            cbPresetType.Items.Add("Volume");
            cbPresetType.SelectedIndex = 0;

            PeriodTypesCB.SelectedIndex = 0;

            // Le versao da EZClient.dll
            EZInterface.DllVersion(ref version);
            WriteMessage(version);
        }

        #endregion

        #region Get CompanyID
        // <summary>
        //    Calcula o codigo referente ao ID do bico nos concentradores CBC da Companytec.
        //    HoseNumber: numero fisico do bico na bomba 
        //    PumpNumber: numero da bomba
        // </summary>
        //
        private String CompanyID(short HoseNumber, short PumpNumber)
        {
            int Offset;

            switch (HoseNumber)
            {
                case 2: Offset = 0x44; break;
                case 3: Offset = 0x84; break;
                case 4: Offset = 0xC4; break;
                default: // Outros valores são tratados como Bico 1
                    Offset = 0x04;
                    break;
            }

            return ((PumpNumber - 1) + Offset).ToString("X2");
        }
        #endregion

        #region Avalia retorno das APIS EZForecourt e gera mensagem de erro
        private Boolean GoodResult(Int32 res)
        {
            string MSG;

            if (res == 0)
                return (true);

            MSG = EZInterface.ResultString(res);

            if (res < 0 && res != (int)EZInterface.Result.INVALID_OBJECT_ID)
            {
                // invalid result fatal error with the connect 
                // best to log off and tehn logon again
                WriteMessage("        *** fatal Error: (" + res + ") " + MSG);
            }
            else
            {
                // logic error 
                WriteMessage("        *** Error: (" + res + ") " + MSG);
            }

            return (false);
        }
        #endregion

        #region Método de Escrever Mensagens no Box
        private void WriteMessage(String msg)
        {
            listMessageBox.Items.Add(msg);
            listMessageBox.SelectedIndex = listMessageBox.Items.Count - 1;
            listMessageBox.SelectedIndex = -1;
        }
        #endregion

        #region Autentificação
        private void btLogon_Click(object sender, EventArgs e)
        {
            short tipoDeCliente;
             IntPtr iprt = new IntPtr(0);
            DateTime dateTime = DateTime.Now;

            if (chProcEvents.Checked)
                tipoDeCliente = 7;
            else
                tipoDeCliente = 1;

            if (edServerAddress.Enabled == true)
            {
                WriteMessage("Conectando no servidor: " + edServerAddress.Text);

                if (GoodResult(EZInterface.ClientLogonEx(MyClientID, tipoDeCliente, edServerAddress.Text, 5123, 5124, 10000, 0, new IntPtr(0), 0)))
                {
                    edServerAddress.Enabled = false;
                    chProcEvents.Enabled = false;
                    btLogon.Text = "Logoff";

                    EZInterface.SetClientType(EZInterface.SINK_MINIMAL_PUMP_EVENT | EZInterface.SINK_FULL_PUMP_EVENT | EZInterface.SINK_DELIVERY_EVENT | EZInterface.SINK_CARD_READ_EVENT | EZInterface.SINK_DB_HOSE_ETOTS_EVENT | EZInterface.SINK_DB_TANK_STATUS_EVENT);

                }
                if (GoodResult(EZInterface.SetDateTime(dateTime)))
                    WriteMessage("Data e Hora do concentrador atualizada com sucesso");

            }
            else
            {

                WriteMessage("Desconectando do servidor: " + edServerAddress.Text);
                GoodResult(EZInterface.ClientLogoff());

                edServerAddress.Enabled = true;
                chProcEvents.Enabled = true;
                btLogon.Text = "Logon";

            }
        }
        #endregion

        #region Limpar Mensagens do Box
        private void btClearMessages_Click(object sender, EventArgs e)
        {
            listMessageBox.Items.Clear();
        }
        #endregion

        #region Liberação pelo Cartão
        private void btReadCards_Click(object sender, EventArgs e)
        {
            int CardID = 0;
            int Number = 0;
            string Name = "";
            int PumpID = 0;
            short CardType = 0;
            int ParentID = 0;
            Int64 Tag = 0;
            DateTime TimeStamp = new DateTime();

            // Verifica conexao
            if (!GoodResult(EZInterface.TestConnection()))
                return;

            while (true)
            {

                // Le ID do primeiro cartao da lista
                if (EZInterface.GetCardReadByOrdinal(1, ref CardID) != 0)
                    break;

                // LE dados do cartao
                if (!GoodResult(EZInterface.GetCardReadProperties(CardID, ref Number, ref Name, ref PumpID, ref CardType, ref ParentID, ref Tag, ref TimeStamp)))
                    break;

                WriteMessage(" --- Cartao Lido: " +
                                          " ID= " + CardID +
                                     ", Numero= " + Number +
                                       ", Nome= " + Name +
                                     ", PumpID= " + PumpID +
                                   ", CardType= " + CardType +
                                   ", ParentID= " + ParentID +
                                        ", Tag= " + Tag.ToString("X10") +
                                  ", TimeStamp= " + TimeStamp);

                // Esta função so funciona se a Autorizacao estiver configurada com um tipo de cartao: 
                //		"Carta/Placa", "Frentista", "Cliente", "Frentista E Cliente", "Frentista OU Cliente"
                if (GoodResult(EZInterface.TagAuthorise(PumpID, Tag, (short)EZInterface.TAllocLimitType.NO_LIMIT_TYPE, 0, 0xFF, 1)))
                    break;

                WriteMessage(" --- Bomba " + PumpID + "Autorizada com Cartao " + Tag);

                // Apaga Cartao da lista
                if (!GoodResult(EZInterface.DeleteCardRead(CardID)))
                    break;

            }

        }
        #endregion

        #region Checando a conexão
        private void btCheckConnection_Click(object sender, EventArgs e)
        {
            if (!GoodResult(EZInterface.TestConnection()))
            {
                edServerAddress.Enabled = true;
                btLogon_Click(this, null);
            }
            else
                WriteMessage("Conexao com EZServer OK!");
        }

        #endregion

        #region Método do Time
        private void timerAppLoop_Tick(object sender, EventArgs e)
        {
            if (chProcEvents.Checked == true)
                InternalProccessEvents();
            else  // Processamento por Pooling
                ReadPumpsStatus();

        }
        #endregion

        #region Lendo o status das bombas
        private void ReadPumpsStatus()
        {
            int PumpsCount = 0;
            String PumpStates = "";
            String CurrentHose = "";
            String DeliveriesCount = "";
            int Idx = 0;
            int CurStatus = 0;
            int CurHose = 0;
            int CurDelv = 0;
            String StrStatus = "";

            byte[] cstatus;
            byte[] chose;
            byte[] cdeliv;

            System.Text.ASCIIEncoding conv = new System.Text.ASCIIEncoding();

            // Verifica se esta conectado ao servidor
            if (EZInterface.TestConnection() == 0)
            {
                // Verifica a quantidade de bombas configuradas
                if (!GoodResult(EZInterface.GetPumpsCount(ref PumpsCount)))
                    return;

                // Le o estado de todas as bombas configuradas
                if (!GoodResult(EZInterface.GetAllPumpStatuses(ref PumpStates, ref CurrentHose, ref DeliveriesCount)))
                    return;

                cstatus = conv.GetBytes(PumpStates);
                chose = conv.GetBytes(CurrentHose);
                cdeliv = conv.GetBytes(DeliveriesCount);

                for (Idx = 1; Idx <= PumpsCount; Idx++)
                {

                    CurStatus = cstatus[Idx - 1] - '0'; // EZClient.TPumpState(Ord(PumpStates[Idx])-Ord('0'));
                    CurHose = chose[Idx - 1] - '0';
                    CurDelv = cdeliv[Idx - 1] - '0';

                    switch ((EZInterface.TPumpState)CurStatus)                                                                                                      // PAM10)
                    {
                        case EZInterface.TPumpState.INVALID_PUMP_STATE: StrStatus = "estado invalido."; break;                                                      // 0 - OFFLINE
                        case EZInterface.TPumpState.NOT_INSTALLED_PUMP_STATE: StrStatus = "nao instalada."; break;                                                  // 6 - CLOSE
                        case EZInterface.TPumpState.NOT_RESPONDING_1_PUMP_STATE: StrStatus = "Bomba nao responde."; break;                                          // 0 - OFFLINE
                        case EZInterface.TPumpState.IDLE_PUMP_STATE: StrStatus = "em espera (desocupada)."; break;                                                  // 1 - IDLE
                        case EZInterface.TPumpState.PRICE_CHANGE_STATE: StrStatus = "troca de preco."; break;                                                       // 1 - IDLE
                        case EZInterface.TPumpState.AUTHED_PUMP_STATE: StrStatus = "Bomba Autorizada"; break;                                                       // 9 - AUTHORIZED
                        case EZInterface.TPumpState.CALLING_PUMP_STATE: StrStatus = "esperando autorizacao."; break;                                                // 5 - CALL
                        case EZInterface.TPumpState.DELIVERY_STARTING_PUMP_STATE: StrStatus = "abastecimeneto iniciando."; break;                                   // 2 - BUSY
                        case EZInterface.TPumpState.DELIVERING_PUMP_STATE: StrStatus = "abastecendo."; break;                                                       // 2 - BUSY
                        case EZInterface.TPumpState.TEMP_STOPPED_PUMP_STATE: StrStatus = "parada temporaria (no meio de uma abastecimento) (STOP)."; break;         // 8 - STOP
                        case EZInterface.TPumpState.DELIVERY_FINISHING_PUMP_STATE: StrStatus = "abastecimento finalizando (fluxo de produto diminuindo)."; break;   // 2 - BUSY
                        case EZInterface.TPumpState.DELIVERY_FINISHED_PUMP_STATE: StrStatus = "abastecimento finalizado (parou de sair combustivel)."; break;       // 2 - BUSY
                        case EZInterface.TPumpState.DELIVERY_TIMEOUT_PUMP_STATE: StrStatus = "abastecimento excedeu tempo maximo."; break;                          // 1 - IDLE
                        case EZInterface.TPumpState.HOSE_OUT_PUMP_STATE: StrStatus = "bico fora do guarda-bico (CALL)."; break;                                     // 5 - CALL
                        case EZInterface.TPumpState.PREPAY_REFUND_TIMEOUT_STATE: StrStatus = "prazo de pre-determinacao esgotado."; break;                          // 1 - IDLE
                        case EZInterface.TPumpState.DELIVERY_TERMINATED_STATE: StrStatus = "abastecimento terminado (EOT)"; break;                                  // 3 - EOT
                        case EZInterface.TPumpState.ERROR_PUMP_STATE: StrStatus = "Erro (resposta de erro da bomba)."; break;                                       // 0 - OFFLINE
                        case EZInterface.TPumpState.NOT_RESPONDING_2_PUMP_STATE: StrStatus = "EZID nao responde."; break;
                        case EZInterface.TPumpState.LAST_PUMP_STATE: StrStatus = "Ultimo estado da bomba?"; break;
                        default:
                            StrStatus = "estado desconhecido = " + CurStatus;
                            break;
                    }

                    if (lastStates[Idx - 1] != cstatus[Idx - 1])
                    {
                        WriteMessage("Bomba " + Idx + ", Bico " + chose[Idx - 1] +
                                                 ", Pendentes " + cdeliv[Idx - 1] +
                                                   ", Status: " + StrStatus);

                        //lastStates.IndexOf(PumpStates[Idx], Idx);

                        lastStates[Idx - 1] = cstatus[Idx - 1];
                    }

                }
            }
        }
        #endregion

        #region Processamento de eventos
        private void InternalProccessEvents()
        {
            int EvtCt = 0;
            short EvtType = 0;

            // Verifica se esta conectado ao servidor
            if (EZInterface.TestConnection() != 0)
                return;

            // Inicia processamento de eventos
            if (!GoodResult(EZInterface.ProcessEvents()))
                return;

            // Le numero de eventos disponiveis
            if (!GoodResult(EZInterface.GetEventsCount(ref EvtCt)))
                return;

            while (true)
            {
                // Le o proximo evento
                if (!GoodResult(EZInterface.GetNextEventType(ref EvtType)))
                    return;

                if (EvtType != 0)
                    WriteMessage(" -> EVENTO GERADO <- : " + EvtType);

                switch ((EZInterface.ClientEvent)EvtType)
                {
                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.PUMP_EVENT:   // Trata Eventos das Bombas
                        EventPump();
                        break;

                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.DELIVERY_EVENT: // Eventos de abastecimento
                        EventDelivery();
                        break;

                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.CARD_READ_EVENT:  // Eventos de leitores de cartoes
                        EventCardRead();
                        break;

                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.DB_LOG_ETOTALS:   // Evento de mudanca de encerrantes
                        EventDbLogETotals();
                        break;

                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.SERVER_EVENT:  // Eventos do servidor
                        EventServer();
                        break;

                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.CLIENT_EVENT:  // Eventos de POS (client)
                        EventClient();
                        break;

                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.ZB2G_STATUS_EVENT: // Eventos de Zigbee
                        EventZB2G();
                        break;

                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.LOG_EVENT_EVENT: // Log eventos
                        EventLog();
                        break;

                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.DB_TANK_STATUS: // Eventos de Tanque
                        EventTank();
                        break;

                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.NO_CLIENT_EVENT:  // Trata Eventos do Cliente
                        return;

                    //---------------------------------------------------------------------
                    default:
                        GoodResult(EZInterface.DiscardNextEvent());
                        WriteMessage("Não há eventos");
                        break;
                }
            }
        }

        #endregion

        #region Evento da Bomba
        private void EventPump()
        {
            int PumpID = 0;
            int PumpNumber = 0;
            short State = 0;
            short ReservedFor = 0;
            int ReservedBy = 0;
            int HoseID = 0;
            int HoseNumber = 0;
            int HosePhysicalNumber = 0;
            int GradeID = 0;
            int GradeNumber = 0;
            short PriceLevel = 0;
            double Price = 0;
            double Volume = 0;
            double Value = 0;
            short StackSize = 0;
            int PhysicalNumber = 0;
            short Side = 0;
            short Address = 0;
            short PriceLevel1 = 0;
            short PriceLevel2 = 0;
            short PumpType = 0;
            int PortID = 0;
            short AuthMode = 0;
            short StackMode = 0;
            short PrepayAllowed = 0;
            short PreauthAllowed = 0;
            short PriceFormat = 0;
            short ValueFormat = 0;
            short VolumeFormat = 0;
            Int64 Tag = 0;
            int AttendantID = 0;
            int AttendantNumber = 0;
            Int64 AttendantTag = 0;
            int CardClientID = 0;
            int CardClientNumber = 0;
            Int64 CardClientTag = 0;
            double CurFlowRate = 0;
            double PeakFlowRate = 0;

            //String cbcID = "";

            String GradeName = "";
            String ShortGradeName = "";
            String PumpName = "";
            String AttendantName = "";
            String CardClientName = "";

            // Verifica se esta conectado ao servidor
            // if (EZInterface.TestConnection() != 0)
            //    return;

            if (GoodResult(EZInterface.GetNextPumpEventEx3(ref PumpID, ref PumpNumber, ref State,
                                                            ref ReservedFor, ref ReservedBy, ref HoseID,
                                                            ref HoseNumber, ref HosePhysicalNumber,
                                                            ref GradeID, ref GradeNumber, ref GradeName,
                                                            ref ShortGradeName, ref PriceLevel, ref Price,
                                                            ref Volume, ref Value, ref StackSize,
                                                            ref PumpName, ref PhysicalNumber, ref Side,
                                                            ref Address, ref PriceLevel1, ref PriceLevel2,
                                                            ref PumpType, ref PortID, ref AuthMode,
                                                            ref StackMode, ref PrepayAllowed, ref PreauthAllowed,
                                                            ref PriceFormat, ref ValueFormat, ref VolumeFormat,
                                                            ref Tag, ref AttendantID, ref AttendantNumber,
                                                            ref AttendantName, ref AttendantTag, ref CardClientID,
                                                            ref CardClientNumber, ref CardClientName, ref CardClientTag,
                                                            ref CurFlowRate, ref PeakFlowRate)))
            {

                WriteMessage("        PumpEvent: " +
                                       " PumpID= " + PumpID +
                                  ", PumpNumber= " + PumpNumber +
                                       ", State= " + State +
                                 ", ReservedFor= " + ReservedFor +
                                  ", ReservedBy= " + ReservedBy +
                                      ", HoseID= " + HoseID +
                                  ", HoseNumber= " + HoseNumber +
                          ", HosePhisicalNumber= " + HosePhysicalNumber +
                                     ", GradeID= " + GradeID +
                                   ", GradeName= " + GradeName +
                                 ", GradeNumber= " + GradeNumber +
                              ", ShortGradeName= " + ShortGradeName +
                                  ", PriceLevel= " + PriceLevel +
                                       ", Price= " + Price +
                                      ", Volume= " + Volume +
                                       ", Value= " + Value +
                                   ", StackSize= " + StackSize +
                                    ", PumpName= " + PumpName +
                              ", PhysicalNumber= " + PhysicalNumber +
                                        ", Side= " + Side +
                                     ", Address= " + Address +
                                 ", PriceLevel1= " + PriceLevel1 +
                                 ", PriceLevel2= " + PriceLevel2 +
                                    ", PumpType= " + PumpType +
                                      ", PortID= " + PortID +
                                    ", AuthMode= " + AuthMode +
                                   ", StackMode= " + StackMode +
                               ", PrepayAllowed= " + PrepayAllowed +
                              ", PreauthAllowed= " + PreauthAllowed +
                                 ", PriceFormat= " + PriceFormat +
                                 ", ValueFormat= " + ValueFormat +
                                ", VolumeFormat= " + VolumeFormat +
                                         ", Tag= " + Tag +
                                 ", AttendantID= " + AttendantID +
                             ", AttendantNumber= " + AttendantNumber +
                               ", AttendantName= " + AttendantName +
                                ", AttendantTag= " + AttendantTag +
                                ", CardClientID= " + CardClientID +
                            ", CardClientNumber= " + CardClientNumber +
                              ", CardClientName= " + CardClientName +
                               ", CardClientTag= " + CardClientTag +
                               ", CurFlowRate= " + CurFlowRate +
                                ", PeakFlowRate= " + PeakFlowRate);

                WriteMessage("             Bico Equivalente CBC: " + CompanyID((short)HoseNumber, (short)PumpNumber));
            }
        }

        #endregion

        #region Evento de Abastecimento
        private void EventDelivery()
        {
            int DeliveryID = 0;
            int HosePhysicalNumber = 0;
            int TankID = 0;
            int TankNumber = 0;
            short DeliveryState = 0;
            short DeliveryType = 0;
            double Volume2 = 0;
            DateTime CompletedDT = new DateTime();
            int LockedBy = 0;
            int Age = 0;
            DateTime ClearedDT = new DateTime();
            double OldVolumeETot = 0;
            double OldVolume2ETot = 0;
            double OldValueETot = 0;
            double NewVolumeETot = 0;
            double NewVolume2ETot = 0;
            double NewValueETot = 0;
            int Duration = 0;

            int PumpID = 0;
            int PumpNumber = 0;
            int HoseID = 0;
            int HoseNumber = 0;
            //short HosePhisicalNumber = 0;
            int GradeID = 0;
            int GradeNumber = 0;

            short PriceLevel = 0;
            double Price = 0;
            double Volume = 0;
            double Value = 0;
            int ReservedBy = 0;

            Int64 Tag = 0;
            int AttendantID = 0;
            int AttendantNumber = 0;
            Int64 AttendantTag = 0;
            int CardClientID = 0;
            int CardClientNumber = 0;
            Int64 CardClientTag = 0;
            double PeakFlowRate = 0;

            short DelState = 0;
            short DelType = 0;

            //String cbcID = "";

            String TankName = "";
            String GradeShortName = "";
            String GradeCode = "";
            String PumpName = "";
            String GradeName = "";
            String AttendantName = "";
            String CardClientName = "";

            // Verifica se esta conectado ao servidor
            //if (EZInterface.TestConnection() != 0)
            //    return;


            if (GoodResult(EZInterface.GetNextDeliveryEventEx3(ref DeliveryID, ref HoseID, ref HoseNumber, ref HosePhysicalNumber, ref PumpID, ref PumpNumber, ref PumpName,
                 ref TankID, ref TankNumber, ref TankName,
                 ref GradeID, ref GradeNumber, ref GradeName, ref GradeShortName, ref GradeCode,
                 ref DelState, ref DelType, ref Volume, ref PriceLevel,
                 ref Price, ref Value, ref Volume2, ref CompletedDT, ref LockedBy,
                 ref ReservedBy, ref AttendantID, ref Age, ref ClearedDT,
                 ref OldVolumeETot, ref OldVolume2ETot, ref OldValueETot,
                 ref NewVolumeETot, ref NewVolume2ETot, ref NewValueETot,
                 ref Tag, ref Duration, ref AttendantNumber, ref AttendantName, ref AttendantTag,
                 ref CardClientID, ref CardClientNumber, ref CardClientName, ref CardClientTag)))
            {



                // Primeiro abastecimento pode ser invalido
                if (DeliveryID > 0)
                {
                    WriteMessage("       DeliveryEvent: " +
                                        " DeliveryID= " + DeliveryID +
                                           ", HoseID= " + HoseID +
                                       ", HoseNumber= " + HoseNumber +
                               ", HosePhysicalNumber= " + HosePhysicalNumber +
                                           ", PumpID= " + PumpID +
                                       ", PumpNumber= " + PumpNumber +
                                         ", PumpName= " + PumpName +
                                           ", TankID= " + TankID +
                                       ", TankNumber= " + TankNumber +
                                         ", TankName= " + TankName +
                                          ", GradeID= " + GradeID +
                                      ", GradeNumber= " + GradeNumber +
                                        ", GradeName= " + GradeName +
                                   ", GradeShortName= " + GradeShortName +
                                        ", GradeCode= " + GradeCode +
                                    ", DeliveryState= " + DeliveryState +
                                     ", DeliveryType= " + DeliveryType +
                                           ", Volume= " + Volume +
                                       ", PriceLevel= " + PriceLevel +
                                            ", Price= " + Price +
                                            ", Value= " + Value +
                                          ", Volume2= " + Volume2 +
                                      ", CompletedDT= " + CompletedDT +
                                         ", LockedBy= " + LockedBy +
                                       ", ReservedBy= " + ReservedBy +
                                      ", AttendantID= " + AttendantID +
                                              ", Age= " + Age +
                                        ", ClearedDT= " + ClearedDT +
                                    ", OldVolumeETot= " + OldVolumeETot +
                                   ", OldVolume2ETot= " + OldVolume2ETot +
                                     ", OldValueETot= " + OldValueETot +
                                    ", NewVolumeETot= " + NewVolumeETot +
                                   ", NewVolume2ETot= " + NewVolume2ETot +
                                     ", NewValueETot= " + NewValueETot +
                                              ", Tag= " + Tag +
                                         ", Duration= " + Duration +
                                  ", AttendantNumber= " + AttendantNumber +
                                    ", AttendantName= " + AttendantName +
                                     ", AttendantTag= " + AttendantTag +
                                     ", CardClientID= " + CardClientID +
                                 ", CardClientNumber= " + CardClientNumber +
                                   ", CardClientName= " + CardClientName +
                                    ", CardClientTag= " + CardClientTag +
                                    ",PeakFlowRate= " + PeakFlowRate);

                    WriteMessage("            Bico Equivalente CBC: " + CompanyID((short)HoseNumber, (short)PumpNumber));

                    if (LockedBy == -1)
                    {

                        if (GoodResult(EZInterface.LockDelivery(DeliveryID)))
                            LockedBy = MyClientID;

                        if ((LockedBy == MyClientID) && (DelState != (short)EZInterface.TDeliveryState.CLEARED))
                            GoodResult(EZInterface.ClearDelivery(DeliveryID, DelType));
                    }


                }
            }
        }

        #endregion

        #region Evento de Leitura de Cartão
        private void EventCardRead()
        {
            int CardReadID = 0;
            int Number = 0;
            short CardType = 0;
            int ParentID = 0;
            DateTime TimeStamp = new DateTime();
            Int64 Tag = 0;
            int PumpID = 0;

            String Name = "";

            // Verifica se esta conectado ao servidor
            //if (EZInterface.TestConnection() != 0)
            //    return;

            WriteMessage("Leitura de cartão solicitada!");

            if (GoodResult(EZInterface.GetNextCardReadEvent(ref CardReadID, ref Number, ref Name,
                                                             ref PumpID, ref CardType, ref ParentID,
                                                             ref Tag, ref TimeStamp)))
            {

                WriteMessage("\n------ CardReadEvent:  CardReadID " + CardReadID +
                                                        ", Number " + Number +
                                                          ", Name " + Name +
                                                        ", PumpID " + PumpID);

                WriteMessage("                         CardType " + CardType +
                                                    ", ParentID " + ParentID +
                                                         ", Tag " + Tag +
                                                  ",  TimeStamp " + TimeStamp);
                // <summary>
                // Rotina utilizada para identificar o status do cartão lido.
                // </summary>
                switch ((EZInterface.TTagType)CardType)
                {
                    case EZInterface.TTagType.ATTENDANT_TAG_TYPE:
                        WriteMessage("           Attendant: " + Name + "  Tag " + Tag);
                        break;

                    case EZInterface.TTagType.BLOCKED_ATTENDANT_TAG_TYPE:
                        WriteMessage("\n           Blocked attendant: " + Name + "  Tag " + Tag);
                        break;

                    case EZInterface.TTagType.WRONG_SHIFT_ATTENDANT_TAG_TYPE:
                        WriteMessage("\n           Wrong shift attendant: " + Name + "  Tag " + Tag);
                        break;

                    case EZInterface.TTagType.CLIENT_TAG_TYPE:
                        WriteMessage("\n           Client: " + Name + "  Tag  " + Tag);
                        break;

                    case EZInterface.TTagType.BLOCKED_CLIENT_TAG_TYPE:
                        WriteMessage("\n           Blocked Client: " + Name + "  Tag " + Tag);
                        break;

                    case EZInterface.TTagType.UNKNOWN_TAG_TYPE:
                        WriteMessage("\n           Unknown Tag read: " + Tag);
                        break;

                    default:
                        WriteMessage("\n           Unknown Tag type: " + CardType + "  Tag " + Tag);
                        break;
                }

                GoodResult(EZInterface.DeleteCardRead(CardReadID));
            }
        }

        #endregion

        #region Evento de mudanca de encerrantes 
        private void EventDbLogETotals()
        {
            int HoseID = 0;
            double Volume = 0;
            double Value = 0;
            double VolumeETot = 0;
            double ValueETot = 0;
            int HoseNumber = 0;
            int HosePhysicalNumber = 0;
            int PumpID = 0;
            int PumpNumber = 0;
            int TankID = 0;
            int TankNumber = 0;
            int GradeID = 0;

            String PumpName = "";
            String TankName = "";
            String GradeName = "";

            // Verifica se esta conectado ao servidor
            //if (EZInterface.TestConnection() != 0)
            //    return;

            if (GoodResult(EZInterface.GetNextDBHoseETotalsEventEx(ref HoseID, ref Volume, ref Value,
                                                                       ref VolumeETot, ref ValueETot,
                                                                    ref HoseNumber, ref HosePhysicalNumber,
                                                                    ref PumpID, ref PumpNumber, ref PumpName,
                                                                    ref TankID, ref TankNumber, ref TankName,
                                                                    ref GradeID, ref GradeName)))
            {
                WriteMessage("------ HoseETotalEvent:  HoseID " + HoseID + ",  Volume " + Volume + "  Value " + Value);
                WriteMessage("             VolumeETot " + VolumeETot + ",  ValueETot " + ValueETot);
                WriteMessage("             HoseNumber " + HoseNumber + ",  HosePhysicalNumber " + HosePhysicalNumber);
                WriteMessage("             PumpID " + PumpID + ",  PumpNumber " + PumpNumber + ",  PumpName " + PumpName);
                WriteMessage("             TankID " + TankID + ",  TankNumber " + TankNumber + ", TankName " + TankName);
                WriteMessage("             GradeID " + GradeID + ",  GradeName " + GradeName);
            }
        }

        #endregion

        #region Evento do EZServer
        private void EventServer()
        {
            int EventID = 0;

            String EventText = "";

            // Verifica se esta conectado ao servidor
            //if (EZInterface.TestConnection() != 0)
            //    return;

            if (GoodResult(EZInterface.GetNextServerEvent(ref EventID, ref EventText)))
                WriteMessage("------ ServerEvent:   EventID " + EventID + ",  EventText " + EventText);

        }
        #endregion

        #region Evento de Client
        private void EventClient()
        {
            int EventID = 0;
            short ClientID = 0;

            String EventText = "";

            // Verifica se esta conectado ao servidor
            //if (EZInterface.TestConnection() != 0)
            //    return;

            if (GoodResult(EZInterface.GetNextClientEvent(ref ClientID, ref EventID, ref EventText)))
                WriteMessage("------ ClientEvent: ClientID " + ClientID + ",  EventID " + EventID + ",  EventText " + EventText);

        }
        #endregion

        #region Evento dos dispositivos ZigBee
        private void EventZB2G()
        {
            Int32 PortID = 0;
            Int64 ZBAddress = 0;
            Int16 LQI = 0;
            Int16 RSSI = 0;
            Int64 ParZBAddress = 0;
            Int16 ZBChannel = 0;
            Int16 MemBlocks = 0;
            Int16 MemFree = 0;

            // Verifica se esta conectado ao servidor
            if (EZInterface.TestConnection() != 0)
                return;

            if (GoodResult(EZInterface.GetNextZB2GStatusEvent(ref PortID, ref ZBAddress, ref LQI, ref RSSI, ref ParZBAddress, ref ZBChannel, ref MemBlocks, ref MemFree)))
                WriteMessage("------ ZigBeeEvent:   PortID " + PortID + ",  Endereço ZigBee " + ZBAddress + ", LQI " + LQI + ", RSSI " + RSSI + ", ParZBAddress " + ParZBAddress + ", Canal " + ZBChannel + ", Memória Bloqueada " + MemBlocks + ", Memória Livre " + MemFree);
        }
        #endregion

        #region Evento Logged
        private void EventLog()
        {
            Int32 LogEventID = 0;
            Int16 DeviceType = 0;
            Int32 DeviceID = 0;
            Int32 DeviceNumber = 0;
            String DeviceName = "";
            Int16 EventLevel = 0;
            Int16 EventType = 0;
            String EventDesc = "";
            DateTime GeneratedDT = new DateTime();
            DateTime ClearedDT = new DateTime();
            Int32 ClearedBy = 0;
            Int32 AckedBy = 0;
            Double Volume = 0;
            Double Value = 0;
            Double ProductVolume = 0;
            Double ProductLevel = 0;
            Double WaterLevel = 0;
            Double Temperature = 0;


            // Verifica se esta conectado ao servidor
            //if (EZInterface.TestConnection() != 0)
            //    return;

            if (GoodResult(EZInterface.GetNextLogEventEvent(ref LogEventID,
                ref DeviceType, ref DeviceID, ref DeviceNumber,
                ref DeviceName, ref EventLevel, ref EventType,
                ref EventDesc, ref GeneratedDT, ref ClearedDT,
                ref ClearedBy, ref AckedBy, ref Volume,
                ref Value, ref ProductVolume, ref ProductLevel,
                ref WaterLevel, ref Temperature)))
            {
                WriteMessage("------ LogEvent:   LogEventID " + LogEventID +
                    ",  DeviceType " + DeviceType + ", DeviceID " + DeviceID +
                    ", DeviceNumber " + DeviceNumber + ", DeviceName " + DeviceName +
                    ", EventLevel " + EventLevel + ", EventType " + EventType +
                    ", EventDesc " + EventDesc + ", GeneratedDT " + GeneratedDT +
                    ", ClearedDT " + ClearedDT + ", ClearedBy " + ClearedBy +
                    ", AckedBy " + AckedBy + ", Volume " + Volume +
                    ", Value " + Value + ", ProductVolume " + ProductVolume +
                    ", ProductLevel " + ProductLevel + ", WaterLevel " + WaterLevel +
                    ", Temperature " + Temperature);
            }
        }
        #endregion

        #region Evento de Tanque
        private void EventTank()
        {
            Int32 TankID = 0;
            Double GaugeVolume = 0;
            Double GaugeTCVolume = 0;
            Double GaugeUllage = 0;
            Double GaugeTemperature = 0;
            Double GaugeLevel = 0;
            Double GaugeWaterVolume = 0;
            Double GaugeWaterLevel = 0;
            Int32 TankNumber = 0;
            String TankName = "";
            Int32 GradeID = 0;
            String GradeName = "";
            Int16 Type = 0;
            Double Capacity = 0;
            Double Diameter = 0;
            Int32 GaugeID = 0;
            Int16 ProbeNo = 0;
            Int16 State = 0;
            Int32 AlarmsMask = 0;

            // Verifica se esta conectado ao servidor
            //if (EZInterface.TestConnection() != 0)
            //    return;

            if (GoodResult(EZInterface.GetNextDBTankStatusEventEx2(ref TankID,
            ref GaugeVolume, ref GaugeTCVolume, ref GaugeUllage,
            ref GaugeTemperature, ref GaugeLevel, ref GaugeWaterVolume,
            ref GaugeWaterLevel, ref TankNumber, ref TankName,
            ref GradeID, ref GradeName, ref Type,
            ref Capacity, ref Diameter, ref GaugeID,
            ref ProbeNo, ref State, ref AlarmsMask)))
            {
                WriteMessage("------ TankEvent:   TankID " + TankID +
                    ",  GaugeVolume " + GaugeVolume + ", GaugeTCVolume " + GaugeTCVolume +
                    ", GaugeUllage " + GaugeUllage + ", GaugeTemperature " + GaugeTemperature +
                    ", GaugeLevel " + GaugeLevel + ", GaugeWaterVolume " + GaugeWaterVolume +
                    ", GaugeWaterLevel " + GaugeWaterLevel + ", TankNumber " + TankNumber +
                    ", TankName " + TankName + ", GradeID " + GradeID +
                    ", GradeName " + GradeName + ", Type " + Type +
                    ", Capacity " + Capacity + ", Diameter " + Diameter +
                    ", GaugeID " + GaugeID + ", ProbeNo " + ProbeNo +
                    ", State " + State + ", AlarmsMask " + AlarmsMask);
            }
        }
        #endregion

        #region Listando todas as configurações.
        private void btLoadConfig_Click(object sender, EventArgs e)
        {
            ListGrades();
            ListTanks();
            ListSensors();
            ListPumps();
            ListZigbee();
            ListHoses();
        }

        #endregion

        #region Lista configuração de combustíveis
        private void ListGrades()
        {
            int Idx = 0;
            int Ct = 0;
            int Id = 0;
            int Number = 0;
            Int16 Type = 0;

            String Name = "";
            String ShortName = "";
            String Code = "";

            //--------------------------------------------------------------------

            // Ler o numero de produtos configurados //
            if (!GoodResult(EZInterface.GetGradesCount(ref Ct)))
                return;

            WriteMessage("[Produtos " + Ct + "]---------------------------------------------------");

            for (Idx = 1; Idx <= Ct; Idx++)
            {

                if (EZInterface.GetGradeByOrdinal(Idx, ref Id) != 0)
                    return;

                if (GoodResult(EZInterface.GetGradePropertiesEx(Id, ref Number, ref Name, ref ShortName, ref Code, ref Type)))
                    WriteMessage("  Grade: " + Number + ",  Nome: " + Name + ",  Abreviado: " + ShortName + ", Codigo: " + Code + ", Tipo: " + Type);
            }

            WriteMessage("");
        }
        #endregion

        #region Listar Tanques
        private void ListTanks()
        {
            int Idx = 0;
            int Ct = 0;
            int Id = 0;
            int Number = 0;
            int GradeID = 0;
            int GradeNumber = 0;
            int GaugeID = 0;
            int GaugeAlarmsMask = 0;
            short TType = 0;
            short State = 0;
            short ProbeNo = 0;
            double Capacity = 0;
            double Diameter = 0;
            double TheoVolume = 0;
            double GaugeVolume = 0;
            double GaugeTCVolume = 0;
            double GaugeUllage = 0;
            double GaugeTemperature = 0;
            double GaugeLevel = 0;
            double GaugeWaterVolume = 0;
            double GaugeWaterLevel = 0;

            String GradeName = "";
            String GradeShortName = "";
            String GradeCode = "";
            String Name = "";

            //--------------------------------------------------------------------
            // Ler o numero de produtos configurados
            if (!GoodResult(EZInterface.GetTanksCount(ref Ct)))
                return;

            WriteMessage("[Tanques = " + Ct + "]---------------------------------------------------");

            //Uso do GetTankByNumber
            //for (Number = 1; Number <= Ct; Number++)
            //{
            //if (!GoodResult(EZInterface.GetTankByNumber(Number, ref Id)))
            //    return;
            //    WriteMessage("Número: " + Number + ", Id: " + Id);
            //}

            //Uso do GetTankByName
            //String TankName = "Tank1";
            //if(!GoodResult(EZInterface.GetTankByName(TankName, ref Id)))
            //    return;
            //WriteMessage("Nome do tanque: " + TankName + ", Id: " + Id);

            //Uso do SetTankProperties(Ex)
            //if(GoodResult(EZInterface.SetTankPropertiesEx(Id, Number, Name, GradeID, TType, 
            //                                                Capacity, Diameter, TheoVolume, GaugeVolume,
            //                                                GaugeTCVolume, GaugeUllage, GaugeTemperature,
            //                                                GaugeLevel, GaugeWaterVolume, GaugeWaterLevel,
            //                                                GaugeID, ProbeNo, GaugeAlarmsMask)))

            //Uso do DeleteTank
            //if(!GoodResult(EZInterface.DeleteTank(Id)))

            for (Idx = 1; Idx <= Ct; Idx++)
            {
                if (EZInterface.GetTankByOrdinal(Idx, ref Id) != 0)
                    return;

                //Uso do GetTankSummary
                //if (GoodResult(EZInterface.GetTankSummaryEx(Id, ref Number, ref Name, ref GradeID,
                //    ref GradeNumber, ref GradeName, ref GradeShortName, ref GradeCode, ref TType, ref Capacity, ref Diameter,
                //    ref TheoVolume, ref GaugeVolume,
                //    ref GaugeTCVolume, ref GaugeUllage,
                //    ref GaugeTemperature, ref GaugeLevel,
                //    ref GaugeWaterVolume, ref GaugeWaterLevel,
                //    ref GaugeID, ref ProbeNo, ref State, ref GaugeAlarmsMask))

                if (GoodResult(EZInterface.GetTankPropertiesEx(Id, ref Number, ref Name, ref GradeID,
                                                              ref TType, ref Capacity, ref Diameter,
                                                              ref TheoVolume, ref GaugeVolume,
                                                              ref GaugeTCVolume, ref GaugeUllage,
                                                              ref GaugeTemperature, ref GaugeLevel,
                                                              ref GaugeWaterVolume, ref GaugeWaterLevel,
                                                              ref GaugeID, ref ProbeNo, ref GaugeAlarmsMask)))
                {
                    WriteMessage("  Tanque: " + Number + ",  Nome: " + Name + ",  Produto: " + GradeID + ",  Tipo: " + TType);
                    WriteMessage("     Capacidade: " + Capacity + "  Diametro: " + Diameter);
                    WriteMessage("     TheoVolume: " + TheoVolume + ",  GaugeVolume: " + GaugeVolume + ",  GaugeTCVolume: " + GaugeTCVolume);
                    WriteMessage("     GaugeUllage: " + GaugeUllage + ",   GaugeTemperature: " + GaugeTemperature + ",  GaugeLevel: " + GaugeLevel);
                    WriteMessage("     GaugeWaterVolume: " + GaugeWaterVolume + ",  GaugeWaterLevel: " + GaugeWaterLevel + ",  GaugeID: " + GaugeID);
                    WriteMessage("     ProbeNo: " + ProbeNo + "   GaugeAlarmsMask: " + GaugeAlarmsMask);
                }

            }

            WriteMessage("");
        }
        #endregion

        #region Listar Sensores
        private void ListSensors()
        {
            int Idx = 0;
            int Ct = 0;
            int Id = 0;
            int Number = 0;
            int PortID = 0;
            Int16 Type = 0;
            Int16 Address = 0;
            Int16 SensorNo = 0;

            String Name = "";

            //--------------------------------------------------------------------
            // Ler o numero de sensores configurados
            if (!GoodResult(EZInterface.GetSensorsCount(ref Ct)))
                return;

            WriteMessage("[Sensores " + Ct + "]---------------------------------------------------");

            for (Idx = 1; Idx <= Ct; Idx++)
            {
                if (EZInterface.GetSensorByOrdinal(Idx, ref Id) != 0)
                    return;

                if (GoodResult(EZInterface.GetSensorProperties(Id, ref Number, ref Name, ref PortID, ref Type, ref Address, ref SensorNo)))
                {
                    WriteMessage("  Sensor: " + Number + ",  Nome: " + Name +
                                    ",  Porta: " + PortID + ",  Tipo: " + Type +
                                    ",  Endereço: " + Address + ",  SensorNo: " + SensorNo);
                }

            }

            WriteMessage("");
        }
        #endregion

        #region Listar Bombas
        private void ListPumps()
        {
            int Idx = 0;
            int Ct = 0;
            int Id = 0;
            int Number = 0;
            short PhysicalNumber = 0;
            short Side = 0;
            short Address = 0;
            short PriceLevel1 = 0;
            short PriceLevel2 = 0;
            short PriceDspFormat = 0;
            short VolumeDspFormat = 0;
            short ValueDspFormat = 0;
            short PType = 0;
            int PortID = 0;
            int AttendantID = 0;
            short AuthMode = 0;
            short StackMode = 0;
            short PrepayAllowed = 0;
            short PreauthAllowed = 0;
            int SlotZigBeeID = 0;
            int MuxSlotZigBeeID = 0;
            short PriceControl = 0;
            short HasPreset = 0;

            String Name = "";

            //--------------------------------------------------------------------
            // Ler o numero de bombas configuradas
            if (!GoodResult(EZInterface.GetPumpsCount(ref Ct)))
                return;

            WriteMessage("[Bombas = " + Ct + "]---------------------------------------------------");

            for (Idx = 1; Idx <= Ct; Idx++)
            {

                if (EZInterface.GetPumpByOrdinal(Idx, ref Id) != 0)
                    return;

                if (GoodResult(EZInterface.GetPumpPropertiesEx(Id, ref Number, ref Name, ref PhysicalNumber,
                                                                    ref Side, ref Address, ref PriceLevel1,
                                                                    ref PriceLevel2, ref PriceDspFormat, ref VolumeDspFormat,
                                                                    ref ValueDspFormat, ref PType, ref PortID,
                                                                    ref AttendantID, ref AuthMode, ref StackMode,
                                                                    ref PrepayAllowed, ref PreauthAllowed, ref SlotZigBeeID,
                                                                    ref MuxSlotZigBeeID, ref PriceControl, ref HasPreset)))
                {
                    WriteMessage("  Bomba: " + Number + ",  Nome: " + Name + ",  PhicalNumber: " + PhysicalNumber + ",  Side: " + Side + ",  Address: " + Address);
                    WriteMessage("     PriceLevel1: " + PriceLevel1 + ",  PriceLevel2: " + PriceLevel2 + ", PriceDspFormat: " + PriceDspFormat);
                    WriteMessage("     PTipe: " + PType + ",  PortID: " + PortID + ",  AttendantID: " + AttendantID + ",  AutoMode: " + AuthMode + ",  StackMode: " + StackMode);
                    WriteMessage("     PrepayAllwed: " + PrepayAllowed + ",  PreauthAllowed: " + PreauthAllowed + ",  SlotZigBeeID: " + SlotZigBeeID + ",  MuxSlotZigBeeID: " + MuxSlotZigBeeID);
                    WriteMessage("     VolumeDspFormat: " + VolumeDspFormat + ", ValueDspFormat: " + ValueDspFormat + ",  PriceControl: " + PriceControl + ",  HasPreset: " + HasPreset);
                }
            }

            WriteMessage("");
        }

        #endregion

        #region Listar ZigBees
        private void ListZigbee()
        {
            int Idx = 0;
            int Ct = 0;
            int Id = 0;
            int Number = 0;
            int PortID = 0;

            Int16 DeviceType = 0;

            String Name = "";
            String SerialNumber = "";
            String NodeIdentifier = "";

            //--------------------------------------------------------------------
            // Ler o numero de EZRemotes configurados
            if (!GoodResult(EZInterface.GetZigBeeCount(ref Ct)))
                return;

            WriteMessage("[EZRemotes " + Ct + "]---------------------------------------------------");

            for (Idx = 1; Idx <= Ct; Idx++)
            {
                if (EZInterface.GetZigBeeByOrdinal(Idx, ref Id) != 0)
                    return;

                if (GoodResult(EZInterface.GetZigBeeProperties(Id, ref Number, ref Name, ref DeviceType,
                    ref SerialNumber, ref NodeIdentifier, ref PortID)))
                {
                    WriteMessage("  Zigbee: " + Number + ",  Nome: " + Name +
                                    ",  DeviceType: " + DeviceType + ",  Número de Série: " + SerialNumber +
                                    ",  NodeIdentifier: " + NodeIdentifier + ",  PortID: " + PortID);
                }

            }

            WriteMessage("");
        }
        #endregion

        #region Listando todos os bicos configurados
        private void ListHoses()
        {
            int Idx = 0;
            int Ct = 0;
            int Id = 0;
            int Number = 0;
            int PumpID = 0;
            int TankID = 0;
            int PhysicalNumber = 0;
            double MtrTheoValue = 0;
            double MtrTheoVolume = 0;
            double MtrElecValue = 0;
            double MtrElecVolume = 0;
            short UVEAntenna = 0;
            double Price1 = 0;
            double Price2 = 0;
            short Enabled = 0;

            //--------------------------------------------------------------------
            // Lê o numero de produtos configurados
            if (!GoodResult(EZInterface.GetHosesCount(ref Ct)))
                return;

            WriteMessage("[Bicos = " + Ct + "]---------------------------------------------------");

            for (Idx = 1; Idx <= Ct; Idx++)
            {

                if (EZInterface.GetHoseByOrdinal(Idx, ref Id) != 0)
                    return;

                if (GoodResult(EZInterface.GetHosePropertiesEx2(Id, ref Number, ref PumpID, ref TankID,
                                                                 ref PhysicalNumber, ref MtrTheoValue,
                                                                 ref MtrTheoVolume, ref MtrElecValue,
                                                                 ref MtrElecVolume, ref UVEAntenna,
                                                                 ref Price1, ref Price2,
                                                                 ref Enabled)))
                {
                    WriteMessage("    Bico: " + Number + ",  PumpID: " + PumpID + ",  TankID: " + TankID + ",  PhisicalNumber: " + PhysicalNumber);
                    WriteMessage("        MtrTheoValue: " + MtrTheoValue + ",  MtrTheoVolume: " + MtrTheoVolume);
                    WriteMessage("        MtrElecValue: " + MtrElecValue + ",  MtrElecVolume: " + MtrElecVolume);
                    WriteMessage("        UVEAntena: " + UVEAntenna + ",  Price1: " + Price1 + ",  Price2: " + Price2 + ",  Enables: " + Enabled);
                }
            }
        }
        #endregion

        #region Leitura de todos os abastecimentos registrados.

        private void GetDelivery(int DelID, bool ClearDel )
        {

            int HoseID = 0;
            short State = 0;
            short DType = 0;
            double Volume = 0;
            short PriceLevel = 0;
            double Price = 0;
            double Value = 0;
            double Volume2 = 0;
            DateTime CompletedDT = new DateTime();
            int LockedBy = 0;
            int ReservedBy = 0;
            int AttendantID = 0;
            int Age = 0;
            DateTime ClearedDT = new DateTime();
            double OldVolumeETot = 0;
            double OldVolume2ETot = 0;
            double OldValueETot = 0;
            double NewVolumeETot = 0;
            double NewVolume2ETot = 0;
            double NewValueETot = 0;
            Int64 Tag = 0;
            int Duration = 0;
            int ClientID = 0;

            if (GoodResult(EZInterface.GetDeliveryPropertiesEx3(DelID, ref HoseID, ref State, ref DType,
                                                                    ref Volume, ref PriceLevel, ref Price,
                                                                    ref Value, ref Volume2, ref CompletedDT,
                                                                    ref LockedBy, ref ReservedBy, ref AttendantID,
                                                                    ref Age, ref ClearedDT, ref OldVolumeETot,
                                                                    ref OldVolume2ETot, ref OldValueETot,
                                                                    ref NewVolumeETot, ref NewVolume2ETot,
                                                                    ref NewValueETot, ref Tag, ref Duration, ref ClientID)))
            {
                WriteMessage("------ Abastecimento: (" + DelID + ") " );
                WriteMessage("           HoseID " + HoseID + ",  State " + State + ",  Type " + DType);
                WriteMessage("           Volume " + Volume + ",  PriceLevel " + PriceLevel + ",  Price " + Price + ",  Value " + Value);
                WriteMessage("           Volume2 " + Volume2 + ",  CompleteDT " + CompletedDT + ",  LockedBy " + LockedBy + ",  ReservedBy " + ReservedBy);
                WriteMessage("           AttendantID " + AttendantID + ",  Age " + Age + ",  ClearedDT " + ClearedDT);
                WriteMessage("           OldVolumeETot " + OldVolumeETot + ",  OldVolume2ETot " + OldVolume2ETot + ",  OldvalueETot " + OldValueETot);
                WriteMessage("           NewVolumeETot " + NewVolumeETot + ",  NewVolume2ETot " + NewVolume2ETot + ",  NewValueETot " + NewValueETot);
                WriteMessage("           Tag " + Tag + ",  Duraction " + Duration + ",   ClientID " + ClientID);
                WriteMessage("");


                if (ClearDel)
                {
                    if (LockedBy != -1)
                        return;

                    if (GoodResult(EZInterface.LockDelivery(DelID)))
                        LockedBy = MyClientID;
                    else
                        return;

                    if ((LockedBy == MyClientID) && (State != (short)EZInterface.TDeliveryState.CLEARED))
                        GoodResult(EZInterface.ClearDelivery(DelID, DType));
                }
            }
        }

        private void btGetAllDeliveries_Click(object sender, EventArgs e)
        {
            int quantidadeDeAbastecimentos = 0;
            int Id = 0;


            // Verifica se esta conectado ao servidor
            if (EZInterface.TestConnection() != 0)
                return;

            // Le o numero de abastecimentos que estão no Ezserver, ou seja, não pegos por nenhum Client.
            if (!GoodResult(EZInterface.GetDeliveriesCount(ref quantidadeDeAbastecimentos)))
                return;

            if (quantidadeDeAbastecimentos > 0)
            {

                WriteMessage("[Abastecimentos " + quantidadeDeAbastecimentos + "]---------------------------------------------------");

#if false
                // new to old
                while ( quantidadeDeAbastecimentos > 0)
                {
                    if (!GoodResult(EZInterface.GetDeliveryByOrdinal(1, ref Id)))
                        return;

                    GetDelivery(Id , true );
                    quantidadeDeAbastecimentos--; 
                }
#else
                // old to new  
                for ( int contador = quantidadeDeAbastecimentos; contador > 0; contador--)
                {

                    if (!GoodResult(EZInterface.GetDeliveryByOrdinal(contador, ref Id)))
                        return;

                    GetDelivery(Id , true );

                }
#endif 

            }
            else
            {
                WriteMessage("Sem abastecimentos até o momento.");
            }

            WriteMessage("------------------------------------------------------------------------");
        }
#endregion

#region Encerrantes
        private void btTotals_Click(object sender, EventArgs e)
        {
            int IdBomba = 0;
            int IdBico = 0;

            int Bomba = 0;
            int Bico = 0;

            int Number = 0;
            int PumpID = 0;
            int TankID = 0;
            int PhysicalNumber = 0;
            double MtrTheoValue = 0;
            double MtrTheoVolume = 0;
            double MtrElecValue = 0;
            double MtrElecVolume = 0;
            short UVEAntenna = 0;
            double Price1 = 0;
            double Price2 = 0;
            short Enabled = 0;

            Bomba = cbPump.SelectedIndex + 1;   // Le o numero da bomba

            // Verifica se esta conectado ao servidor
            if (EZInterface.TestConnection() != 0)
                return;


            // Pega Id da Bomba escolhida
            if (!GoodResult(EZInterface.GetPumpByOrdinal(Bomba, ref IdBomba)))
                return;

            WriteMessage("[ Encerrantes:  Bomba " + Bomba + "]--------------------------------------------------------");

            for (Bico = 1; Bico < 7; Bico++)
            {
                // Le Id do Bico (sem GoodResult() para evitar mensagens no final)
                if (EZInterface.GetPumpHoseByNumber(IdBomba, Bico, ref IdBico) != 0)
                    return;

                // Le dados do Bico
                if (GoodResult(EZInterface.GetHosePropertiesEx2(IdBico, ref Number, ref PumpID, ref TankID,
                                                                         ref PhysicalNumber, ref MtrTheoValue,
                                                                         ref MtrTheoVolume, ref MtrElecValue,
                                                                         ref MtrElecVolume, ref UVEAntenna,
                                                                         ref Price1, ref Price2, ref Enabled)))
                {
                    WriteMessage(" Bico " + Bico + ",  EncVolume " + MtrElecVolume + ",  EncDInheiro " + MtrElecValue + ",  Preco1 " + Price1 + ",  Preco2 " + Price2);
                    WriteMessage("    [ Number " + Number + ",  PumpId " + PumpID + ",  TankID " + TankID + ",  PhisicalNumber " + PhysicalNumber + " ]");
                    WriteMessage("    [ MtrTheoValue " + MtrTheoValue + ", MtrTheoVolume " + MtrTheoVolume + ",  UVAntenna " + UVEAntenna + ",  Enabled " + Enabled + " ]");
                    WriteMessage("");
                }
            }
        }
#endregion

#region Botão Autoriza Bomba
        private void btAuthorize_Click(object sender, EventArgs e)
        {
            int Bomba = 0;
            int IdBomba = 0;

            Bomba = cbPump.SelectedIndex + 1;   // Le o numero da bomba

            // Verifica conexao
            if (!GoodResult(EZInterface.TestConnection()))
                return;

            // Pega Id da Bomba escolhida
            if (!GoodResult(EZInterface.GetPumpByOrdinal(Bomba, ref IdBomba)))
                return;

            // Envia Autorizacao para bomba
            if (GoodResult(EZInterface.Authorise(IdBomba)))
                WriteMessage("--- Bomba " + Bomba + " Autorizada!");

        }
#endregion

#region Botão Bloqueio de Bomba
        private void btLock_Click(object sender, EventArgs e)
        {
            int Bomba = 0;
            int IdBomba = 0;

            Bomba = cbPump.SelectedIndex + 1;   // Le o numero da bomba

            // Verifica conexao
            if (!GoodResult(EZInterface.TestConnection()))
                return;

            // Pega Id da Bomba escolhida
            if (!GoodResult(EZInterface.GetPumpByOrdinal(Bomba, ref IdBomba)))
                return;

            // Envia bloqueio (desautorizacao) para bomba
            if (GoodResult(EZInterface.TempStop(IdBomba)))
                WriteMessage("--- Bomba " + Bomba + " Desautorizada!");


            // Envia bloqueio (desautorizacao) para bomba
            //if (GoodResult(EZInterface.CancelAuthorise(IdBomba)))
            //    WriteMessage("--- Bomba " + Bomba + " Desautorizada!");
        }
#endregion

#region Botão Troca de Preço
        private void btChangePrice_Click(object sender, EventArgs e)
        {
            int Bomba = 0;
            int Bico = 0;
            int IdBico = 0;
            short Duracao = 0;
            short Tipo = 0;
            double Valor1 = 0;
            double Valor2 = 0;

            int Index = 0;
            int Bicos = 0;

            int HNumber = 0;
            int PhysicalNumber = 0;
            int PumpID = 0;
            int PumpNumber = 0;
            int TankID = 0;
            int TankNumber = 0;
            int GradeID = 0;
            int GradeNumber = 0;
            double MtrTheoValue = 0;
            double MtrTheoVolume = 0;
            double trElecValue = 0;
            double MtrElecVolume = 0;
            double Price1 = 0;
            double Price2 = 0;
            short HEnabled = 0;

            String PumpName = "";
            String TankName = "";
            String GradeName = "";
            String GradeShortName = "";
            String GradeCode = "";

            Duracao = (short)EZInterface.TDurationType.MULTIPLE_DURATION_TYPE; // Duracao do preco (Multipos abastecimentos)
            Tipo = (short)EZInterface.TPriceType.FIXED_PRICE_TYPE;  // Tipo de preco (Fixo)

            Bomba = cbPump.SelectedIndex + 1;   // Le o numero da bomba
            Bico = cbHose.SelectedIndex + 1;
            Valor1 = Convert.ToDouble(edPrice1.Text);
            Valor2 = Convert.ToDouble(edPrice2.Text);

            if (Valor2 == 0)
            {
                Valor2 = Valor1;
            }

            WriteMessage("--- Bomba " + Bomba + " - Troca de precos");

            // Verifica conexao
            if (!GoodResult(EZInterface.TestConnection()))
                return;

            // Le o numero de bicos cadastrados
            if (!GoodResult(EZInterface.GetHosesCount(ref Bicos)))
                return;

            for (Index = 1; Index <= Bicos; Index++)
            {

                // Pega o ID do bico
                if (!GoodResult(EZInterface.GetHoseByOrdinal(Index, ref IdBico)))
                    return;

                // Pega os dados do bico
                if (GoodResult(EZInterface.GetHoseSummaryEx(IdBico, ref HNumber, ref PhysicalNumber,
                                                                     ref PumpID, ref PumpNumber, ref PumpName,
                                                                     ref TankID, ref TankNumber, ref TankName,
                                                                     ref GradeID, ref GradeNumber, ref GradeName,
                                                                     ref GradeShortName, ref GradeCode,
                                                                     ref MtrTheoValue, ref MtrTheoVolume,
                                                                     ref trElecValue, ref MtrElecVolume, ref Price1,
                                                                     ref Price2, ref HEnabled)))
                {

                    // Verifica se o ID do bico pertence ao escolhido
                    if ((Bomba == PumpNumber) && (Bico == HNumber))
                    {

                        WriteMessage("        Precos Atual: Bomba " + PumpNumber + ", Bico " + HNumber +
                                                      ", Preco1 R$" + Price1 + ",  Preco2 R$" + Price2);

                        if (GoodResult(EZInterface.SetHosePrices(IdBico, Duracao, Tipo, Valor1, Valor2)))
                        {
                            WriteMessage("        Preco Novo: Bomba " + Bomba + ", Bico " + Bico + ", Preco1 R$" + Valor1 +
                                                        ", Preco2 R$" + Valor2 + ", (Duraticao " + Duracao + ", Tipo " + Tipo + ")");
                        }

                        break;
                    }
                }
            }
        }
#endregion

#region Predeterminação (Preset)
        private void btPreset_Click(object sender, EventArgs e)
        {
            int Bomba = 0;
            int Bico = 0;
            int IdBomba = 0;
            int IdBico = 0;
            short LType = 0;
            double PsValue = 0;

            Bomba = cbPump.SelectedIndex + 1;   // Le o numero da bomba
            Bico = cbHose.SelectedIndex + 1;
            LType = (short)(cbPresetType.SelectedIndex + 2);

            // Verifica se o textbox de Predet. está vazio
            if (edPreset.MaskCompleted)
            {
                PsValue = Convert.ToDouble(edPreset.Text);
            }
            else
            {
                WriteMessage("Valor informado está incorreto.");
            }

            WriteMessage("--- Bomba " + Bomba + " - Preset");

            // Verifica conexao
            if (!GoodResult(EZInterface.TestConnection()))
                return;

            // Pega Id da Bomba escolhida
            if (!GoodResult(EZInterface.GetPumpByOrdinal(Bomba, ref IdBomba)))
                return;

            IdBico = (1 << (Bico - 1));  // Calcula ID do bico escolhido

            // Envia preset para bomba
            if (GoodResult(EZInterface.LoadPreset(IdBomba, LType, PsValue, (short)IdBico, 1)))
                WriteMessage("     Preset Enviado: Bomba " + Bomba + " Bico " + Bico + " Tipo " + LType + " Valor " + PsValue + " Nivel 1");
        }
#endregion

#region Botão Finalizar Abastecimento
        private void button1_Click(object sender, EventArgs e)
        {
            int Bomba;

            Bomba = cbPump.SelectedIndex + 1;

            EZInterface.TempStop(Bomba);
        }
#endregion

#region Botão Desativar Bico
        private void buttonDesativar_Click(object sender, EventArgs e)
        {
            int Bomba = 0;
            int IdBomba = 0;
            int num = 0;

            Bomba = cbPump.SelectedIndex + 1;   // Le o numero da bomba

            // Verifica conexao
            if (!GoodResult(EZInterface.TestConnection()))
                return;

            if (!GoodResult(EZInterface.GetHoseByOrdinal(Bomba, ref IdBomba)))
                return;
            if (!GoodResult(EZInterface.DisablePump(IdBomba)))
                return;
            if (!GoodResult(EZInterface.GetHosesCount(ref num)))
                return;

            //1 Bomba com o Id 1 = total de 4 bicos
            WriteMessage("Quantidade de bicos " + num);
        }
#endregion

#region Ver preço
        private void buttonSeePrice_Click(object sender, EventArgs e)
        {
            int Id = 1;
            int Ct = 0;
            int Number = 0;
            int PumpID = 0;
            int TankID = 0;
            int PhysicalNumber = 0;
            double MtrTheoValue = 0;
            double MtrTheoVolume = 0;
            double MtrElecValue = 0;
            double MtrElecVolume = 0;
            short UVEAntenna = 0;
            double Price1 = 0;
            double Price2 = 0;
            short Enabled = 0;

            WriteMessage("<<<<----------GetHoseProperties ---------->>>>>");

            if (!GoodResult(EZInterface.GetHosesCount(ref Ct)))
                return;

            for (Id = 1; Id <= Ct; Id++)
            {
                if (GoodResult(EZInterface.GetHosePropertiesEx2(Id, ref Number, ref PumpID, ref TankID,
                                                                      ref PhysicalNumber, ref MtrTheoValue,
                                                                      ref MtrTheoVolume, ref MtrElecValue,
                                                                      ref MtrElecVolume, ref UVEAntenna,
                                                                      ref Price1, ref Price2,
                                                                      ref Enabled)))
                {
                    WriteMessage("     ID: " + Id + ", Bico: " + Number + ",  PumpID: " + PumpID + ",  TankID: " + TankID);
                    WriteMessage("        MtrTheoValue: " + MtrTheoValue + ",  MtrTheoVolume: " + MtrTheoVolume + ",  PhisicalNumber: " + PhysicalNumber);
                    WriteMessage("        MtrElecValue: " + MtrElecValue + ",  MtrElecVolume: " + MtrElecVolume);
                    WriteMessage("        UVEAntena: " + UVEAntenna + ",  Price1: R$" + Price1 + ",  Price2: R$" + Price2 + ",  Enables: " + Enabled);
                }
            }
        }
#endregion

#region Consultar Entrega
        private void buttonConsultarEntrega_Click(object sender, EventArgs e)
        {
            Int32 Count = 0;
            Int16 DeviceType = ( Int16 ) EZInterface.TLogEventDeviceType.TANK_ALR; 
            Int32 DeviceId = 1; 
            Int32 DeviceNumber = 0; // tank Number 1
            string DeviceName = "";
            string EventDesc = "";
            Int16 EventLevel = -1;
            Int16 EventType = ( Int16 ) EZInterface.TLogEventType.TANK_DROP_END_TALR;
            Int32 ClearedBy = -2;
            Int32 AckedBy = -2;
            DateTime GeneretedDT = new DateTime();
            DateTime ClearedDT = new DateTime();
            double Value = 0;
            double ProductVolume = 0;
            double ProductLevel = 0;
            double WaterLevel = 0;
            double Temperature = 0;

            int Id = 0;

            //Testando a conexão);
            double Volume = 0;
            if (GoodResult(EZInterface.TestConnection()))
            {
                if (EZInterface.GetLogEventCount(ref Count, DeviceType, DeviceId, EventLevel, EventType, ClearedBy, AckedBy) != 0)
                    return;

                WriteMessage("[Eventos " + Count + "]---------------------------------------------------"); //Vem com o Count preenchido corretamento de acordo com o filtro que passei.

                for (int Index = 1; Index <= Count && Index <= 20 ; Index++)
                {
                    if (EZInterface.GetLogEventByOrdinal(Index, ref Id, DeviceType, DeviceId, EventLevel, EventType, ClearedBy, AckedBy) != 0)
                        return;

                    if (GoodResult(EZInterface.GetLogEventProperties(Id, ref DeviceType, ref DeviceId, ref DeviceNumber,
                                                            ref DeviceName, ref EventLevel, ref EventType, ref EventDesc,
                                                            ref GeneretedDT, ref ClearedDT, ref ClearedBy, ref AckedBy,
                                                            ref Volume, ref Value, ref ProductVolume, ref ProductLevel,
                                                            ref WaterLevel, ref Temperature)))
                    {
                        WriteMessage("Id: " + Id + ", DType: " + DeviceType + ", DId: " + DeviceId +
                                    ", DNumber: " + DeviceNumber + ", DName: " + DeviceName +
                                    ", ELevel: " + EventLevel + ", EType: " + EventType +
                                    ", EDesc: " + EventDesc + ", GDate: " + GeneretedDT +
                                    ", Volume: " + Volume + ", Value: " + Value +
                                    ", PVolume: " + ProductVolume + ", PLevel: " + ProductLevel +
                                    ", WLevel: " + WaterLevel + ", Temp: " + Temperature);
                    }
                }
            }
        }

        private void buttonConsultarHistorico_Click(object sender, EventArgs e)
        {
            Int32 Count = 0;
            Int16 DeviceType = (Int16)EZInterface.TLogEventDeviceType.TANK_ALR;
            Int32 DeviceId = 1;
            Int32 DeviceNumber = 0; // tank Number 1
            string DeviceName = "";
            string EventDesc = "";
            Int16 EventLevel = -1;
            Int16 EventType = (Int16)EZInterface.TLogEventType.TANK_STATE_TALR;
            Int32 ClearedBy = -2;
            Int32 AckedBy = -2;
            DateTime GeneretedDT = new DateTime();
            DateTime ClearedDT = new DateTime();
            double Value = 0;
            double ProductVolume = 0;
            double ProductLevel = 0;
            double WaterLevel = 0;
            double Temperature = 0;

            int Id = 0;

            //Testando a conexão);
            double Volume = 0;
            if (GoodResult(EZInterface.TestConnection()))
            {
                if (EZInterface.GetLogEventCount(ref Count, DeviceType, DeviceId, EventLevel, EventType, ClearedBy, AckedBy) != 0)
                    return;

                WriteMessage("[Eventos " + Count + "]---------------------------------------------------"); //Vem com o Count preenchido corretamento de acordo com o filtro que passei.

                for (int Index = 1; Index <= Count && Index <= 20; Index++)
                {
                    if (EZInterface.GetLogEventByOrdinal(Index, ref Id, DeviceType, DeviceId, EventLevel, EventType, ClearedBy, AckedBy) != 0)
                        return;

                    if (GoodResult(EZInterface.GetLogEventProperties(Id, ref DeviceType, ref DeviceId, ref DeviceNumber,
                                                            ref DeviceName, ref EventLevel, ref EventType, ref EventDesc,
                                                            ref GeneretedDT, ref ClearedDT, ref ClearedBy, ref AckedBy,
                                                            ref Volume, ref Value, ref ProductVolume, ref ProductLevel,
                                                            ref WaterLevel, ref Temperature)))
                    {
                        WriteMessage("Id: " + Id + ", DType: " + DeviceType + ", DId: " + DeviceId +
                                    ", DNumber: " + DeviceNumber + ", DName: " + DeviceName +
                                    ", ELevel: " + EventLevel + ", EType: " + EventType +
                                    ", EDesc: " + EventDesc + ", GDate: " + GeneretedDT +
                                    ", PVolume: " + ProductVolume + ", PLevel: " + ProductLevel +
                                    ", WLevel: " + WaterLevel + ", Temp: " + Temperature);
                    }
                }
            }
        }

#endregion

        private void buttonTeste_Click(object sender, EventArgs e)
        {
            //var teste = EZInterface.SetHosePrices(1, 1, 1, 3.333, 0);
            // WriteMessage("<<<<<------------SetHosePrices------------>>>>>>");

            //EZInterface.LoadPresetWithPrice(1, 1, 0, 1, 1, 8.888);

            //WriteMessage("<<<<<------------LoadPresetWithPrice------------>>>>>>");

            // var teste = EZInterface.PaymentReserve(1, 35, "hash");

            // if (GoodResult(EZInterface.PaymentAuthorise(1, 35, "", -1, -1, -1, -1, 9, 77, 0, 1, 0, 7.777, 2, 30.000, 3, 0, 0, "", "", "", "")))
            // {
            //     WriteMessage("Teste");
            // }

            buttonConsultarEntrega_Click(sender, e);

        }

        private void LerProdutosComplementares_Click(object sender, EventArgs e)
        {
            int quantidadeRegistros = 0;
            int Id = 0;

            // Verifica se esta conectado ao servidor
            if (EZInterface.TestConnection() != 0)
                return;

            // Le o numero de abastecimentos que estão no Ezserver, ou seja, não pegos por nenhum Client.
            var result = EZInterface.GetSaleItemsCount(ref quantidadeRegistros, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -1, -2, -2);

            if (!GoodResult(result))
                return;

            if (quantidadeRegistros != 0)
            {

                WriteMessage("[Produtos " + quantidadeRegistros + "]---------------------------------------------------");

                for (int contador = quantidadeRegistros; contador > 0; contador--)
                {

                    if (!GoodResult(EZInterface.GetSaleItemByOrdinal(contador, ref Id, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -2, -1, -2, -2)))
                        return;


                    //TODO: Coletar e mostrar na tela valores de produto complementar encontrado.

                    if (GoodResult(EZInterface.SaleItemLockAndClear(Id)))
                        WriteMessage("Produto " + Id + " resolvido.");
                    else
                        continue;
                }
            }
            else
            {
                WriteMessage("Sem produtos até o momento.");
            }

            WriteMessage("------------------------------------------------------------------------");
        }

#region Periods

        private bool ClosePeriod(short PeriodType, DateTime CloseDT)
        {

            WriteMessage("[Close report " + PeriodType.ToString() + " --- " + CloseDT.ToString() );
            // verify that all if pumps are idle, and if so stop them all 
            if (!GoodResult(EZInterface.AllStopIfIdle()))
            {
                return false;
            }

            // get the number of hoses
            Int32 HosesCount = 0;
            if (!GoodResult(EZInterface.GetHosesCount(ref HosesCount)))
                return false;

            for (Int32 HoseIndex = 1; HoseIndex <= HosesCount; HoseIndex++)
            {
                Int32 HoseID = -1;
                if (!GoodResult(EZInterface.GetHoseByOrdinal(HoseIndex, ref HoseID)))
                    return false;

                if (!GoodResult(EZInterface.ClosePeriod(HoseID, PeriodType, CloseDT)))
                {
                    return false;
                }

            }

            // unblock all the pumps now that we have finished 
            if (!GoodResult(EZInterface.AllAuthorise()))
            {
                return false;
            }

            return true;
        }

        private DateTime GetPeriodCloseDT(short PeriodType)

        {

            DateTime CloseDT = DateTime.Now;
            Int32 HoseID = -1;
            if (!GoodResult(EZInterface.GetHoseByOrdinal(1, ref HoseID)))
                return CloseDT;

            Double StartVolume = 0;
            Double StartValue = 0;
            Double EndVolume = 0;
            Double EndValue = 0;


            if (!GoodResult(EZInterface.GetPeriod(HoseID, PeriodType, ref StartVolume, ref StartValue, ref CloseDT, ref EndVolume, ref EndValue)))
            {
                return CloseDT;
            }

            return CloseDT;

        }

        private void PrintPeriod(short PeriodType)
        {

            Int32 GradesCount = 0;
            double VolumeTotal = 0;
            double ValueTotal = 0;
            string VolumeUnits = "L";
            string ValueUnits = "$/L";

            WriteMessage("[Period report " + PeriodTypesCB.Items[PeriodType-1].ToString() + " ---------------------------------------------------");

            if (!GoodResult(EZInterface.GetGradesCount(ref GradesCount)))
                return;

            DateTime ReportDate = GetPeriodCloseDT(PeriodType);

            for ( Int32 GradeIndex = 1; GradeIndex <= GradesCount; GradeIndex++ )
            { 
                Int32 GradeID = -1;

                if (!GoodResult(EZInterface.GetGradeByOrdinal(GradeIndex, ref GradeID)))
                    return;

                Int32 GradeNumber = 0;
                string GradeName = "";
                string ShortName = "";
                string Code = "";

                Double GradeVolumeTotal = 0;
                Double GradeValueTotal = 0;
                int HosesForGrade = 0;

                if (!GoodResult(EZInterface.GetGradeProperties(GradeID, ref GradeNumber, ref GradeName, ref ShortName, ref Code)))
                    return;

                Int32 HosesCount = 0;

                if (!GoodResult(EZInterface.GetHosesCount(ref HosesCount)))
                    return;

                for (Int32 HoseIndex = 1; HoseIndex <= HosesCount; HoseIndex++)
                {
                    Int32 HoseID = -1;
                    if (!GoodResult(EZInterface.GetHoseByOrdinal(HoseIndex, ref HoseID)))
                        return;

                    Int32 HoseNumber = 0;
                    Int32 PhysicalNumber = 0;
                    Int32 PumpID = -1;
                    Int32 PumpNumber = 0;
                    string PumpName = "";
                    Int32 TankID = -1;
                    Int32 TankNumber = 0;
                    string TankName = "";
                    Int32 HoseGradeID = 0;
                    Int32 HoseGradeNumber = 0;
                    string HoseGradeName = "";
                    string GradeShortName = "";
                    string GradeCode = "";
                    Double MtrTheoValue = 0;
                    Double MtrTheoVolume = 0;
                    Double MtrElecValue = 0;
                    Double MtrElecVolume = 0;
                    Double Price1 = 0;
                    Double Price2 = 0;
                    Int16 Enabled = 0;

                    if (!GoodResult(EZInterface.GetHoseSummaryEx(HoseID, ref HoseNumber, ref PhysicalNumber,
                                                ref PumpID, ref PumpNumber, ref PumpName,
                                                ref TankID, ref TankNumber, ref TankName,
                                                ref HoseGradeID, ref HoseGradeNumber, ref HoseGradeName, ref GradeShortName, ref GradeCode,
                                                ref MtrTheoValue, ref MtrTheoVolume, ref MtrElecValue, ref MtrElecVolume,
                                                ref Price1, ref Price2, ref Enabled)))
                        return;

                    if (HoseGradeID != GradeID)
                        continue;

                    Double StartVolume = 0;
                    Double StartValue = 0;
                    DateTime CloseDT = DateTime.Now;
                    Double EndVolume = 0;
                    Double EndValue = 0;


                    if (!GoodResult(EZInterface.GetPeriod(HoseID, PeriodType, ref StartVolume, ref StartValue, ref CloseDT, ref EndVolume, ref EndValue)))
                    {
                        return;
                    }

                    if (HosesForGrade == 0)
                    {
                        WriteMessage("[- " + GradeName + "]---------------------------------------------------");
                        GradeVolumeTotal = 0;
                        GradeValueTotal = 0;
                    }


                    Double VolumeDelta = EndVolume - StartVolume;
                    Double ValueDelta = EndValue - StartValue;

                    GradeVolumeTotal = GradeVolumeTotal + VolumeDelta;
                    GradeValueTotal = GradeValueTotal + ValueDelta;

                    VolumeTotal = VolumeTotal + VolumeDelta;
                    ValueTotal = ValueTotal + ValueDelta;


                    HosesForGrade = HosesForGrade + 1;

                    WriteMessage(String.Format("{0,-12} {1,10:N2} - {2,10:N2} = {3,10:N2}{4} {5,10:N2} - {6,10:N2} = {7,10:N2}{8} ",
                                             PumpName + "-" + HoseNumber.ToString(),
                                             EndVolume, StartVolume, VolumeDelta, VolumeUnits,
                                             EndValue, StartValue, ValueDelta, ValueUnits));

                 }

                 if (HosesForGrade > 0)
                 {
                    WriteMessage(String.Format("{0,-12}                           {1,10:N2}{2}                           {3,10:N2}{4} ",
                                             "Total" ,
                                             GradeVolumeTotal, VolumeUnits,
                                             GradeValueTotal, ValueUnits));

                 }

            }

            WriteMessage(String.Format("{0,-12}                           {1,10:N2}{2}                           {3,10:N2}{4} ",
                                            "Grand total", 
                                            VolumeTotal, VolumeUnits,
                                            ValueTotal, ValueUnits));


        }

#endregion

        private void PrintPeriodBN_Click(object sender, EventArgs e)
        {
            PrintPeriod((short)(PeriodTypesCB.SelectedIndex+1));
        }

        private void ClosePeriodBN_Click(object sender, EventArgs e)
        {
            DateTime now = System.DateTime.Now;

            ClosePeriod((short)(PeriodTypesCB.SelectedIndex+1), now);
        }

        private void buttonReadTanks_Click(object sender, EventArgs e)
        {
            ListTanks();
        }
    }
}
