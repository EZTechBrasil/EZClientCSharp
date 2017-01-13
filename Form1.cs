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

        public Form1()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            Int32 ct = 0;
            String version = "";
            
            edServerAddress.Text = "localhost";

            // Carrega lista de bombas
            for( ct=1; ct<32; ct++)
                cbPump.Items.Add( ct );

            cbPump.SelectedIndex = 0;

            // Carrega lista de bicos
            for (ct = 1; ct<=8; ct++)
                cbHose.Items.Add(ct);

            cbHose.SelectedIndex = 0;

            // Preenche lista de tipos de preset
            cbPresetType.Items.Add("Dinheiro");
            cbPresetType.Items.Add("Volume");
            cbPresetType.SelectedIndex = 0;

            // Le versao da EZClient.dll
            EZInterface.DllVersion(ref version);
            WriteMessage(version);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
        }

        //------------------------------------------------------------------------------
        // Calcula o codigo referente ao ID do bico nos concentradores CBC da Companytec.
        //    HoseNumber: numero fisico do bico na bomba 
        //    PumpNumber: numero da bomba
        //
        private String CompanyID(short HoseNumber, short PumpNumber)
        {
            int Offset;

            switch( HoseNumber )
            {
               case 2: Offset = 0x44;  break;
               case 3: Offset = 0x84;  break;
               case 4: Offset = 0xC4;  break;
               default: // Outros valores são tratados como Bico 1
                Offset = 0x04;
                break;
            }

            return ((PumpNumber-1)+Offset).ToString("X2");
        }

        //----------------------------------------------------------------------------
        // Avalia retorno das APIS EZForecourt e gera mensagem de erro
        private Boolean GoodResult(Int32 res)
        {
            string MSG;

            if(res==0)
                return(true);

            MSG = EZInterface.ResultString(res);

            WriteMessage( "        *** Error: (" + res + ") " + MSG );

            return (false);
        }

        //---------------------------------------------------------------------
        private void WriteMessage(String msg)
        {
            listMessageBox.Items.Add(msg);
            listMessageBox.SelectedIndex = listMessageBox.Items.Count - 1;
            listMessageBox.SelectedIndex = -1;
        }
  
        //---------------------------------------------------------------------
        private void chProcEvents_CheckedChanged(object sender, EventArgs e)
        {

        }

        //---------------------------------------------------------------------
        private void btLogon_Click(object sender, EventArgs e)
        {
            short iTmp;
            IntPtr iprt = new IntPtr(0);
            DateTime dateTime = DateTime.Now ;

            if (chProcEvents.Checked)
                iTmp = 7;
            else
                iTmp = 1;

           if( edServerAddress.Enabled == true )
           {
              WriteMessage("Conectando no servidor: " + edServerAddress.Text);

               if (GoodResult(EZInterface.ClientLogonEx(35, iTmp, edServerAddress.Text, 5123, 5124, 10000, 0, new IntPtr(0), 0)))
              {
                edServerAddress.Enabled = false;
                chProcEvents.Enabled = false;
                btLogon.Text = "Logoff";
              }
               if (GoodResult(EZInterface.SetDateTime(dateTime)))
                   WriteMessage("Data e Hora do concentrador atualizada com sucesso");
              
           }
           else
           {

              WriteMessage("Desconectando do servidor: " + edServerAddress.Text);
              GoodResult( EZInterface.ClientLogoff() );

              edServerAddress.Enabled = true;
              chProcEvents.Enabled = true;
              btLogon.Text = "Logon";

           }
        }

        private void btClearMessages_Click(object sender, EventArgs e)
        {
            listMessageBox.Items.Clear();
        }

        //---------------------------------------------------------------------
        private void btReadCards_Click(object sender, EventArgs e)
        {
            int      CardID   = 0;
            int      Number   = 0;
            string   Name     = "";
            int      PumpID   = 0;
            short    CardType = 0;
            int      ParentID = 0;
            Int64    Tag      = 0;
            DateTime TimeStamp = new DateTime();

            // Verifica conexao
            if( !GoodResult( EZInterface.TestConnection() ) )
              return;

            while( true )
            {

              // Le ID do primeiro cartao da lista
              if( EZInterface.GetCardReadByOrdinal(1, ref CardID)!=0 )
                break;

              // LE dados do cartao
              if( !GoodResult( EZInterface.GetCardReadProperties(CardID, ref Number, ref Name, ref PumpID, ref CardType, ref ParentID, ref Tag, ref TimeStamp) ) )
                break;

              WriteMessage(" --- Cartao Lido: " +
                                        " ID= " + CardID +
                                   ", Numero= " + Number +
                                     ", Nome= " + Name +
                                   ", PumpID= " + PumpID +
                                 ", CardType= " + CardType +
                                 ", ParentID= " + ParentID +
                                      ", Tag= " + Tag.ToString("X10") +
                                ", TimeStamp= " + TimeStamp );

	          // Esta função so funciona se a Autorizacao estiver configurada com um tipo de cartao: 
	          //		"Carta/Placa", "Frentista", "Cliente", "Frentista E Cliente", "Frentista OU Cliente"
              if (GoodResult(EZInterface.TagAuthorise(PumpID, Tag, (short)EZInterface.TAllocLimitType.NO_LIMIT_TYPE, 0, 0xFF, 1)))
                break;

              WriteMessage(" --- Bomba " + PumpID + "Autorizada com Cartao " + Tag );

              // Apaga Cartao da lista
              if( !GoodResult( EZInterface.DeleteCardRead(CardID) ) )
                break;

            }

        }

        //----------------------------------------------------------------------------
        private void btCheckConnection_Click(object sender, EventArgs e)
        {
            if( !GoodResult( EZInterface.TestConnection() ) )
            {
                edServerAddress.Enabled = true;
                btLogon_Click(this, null);
            }
            else
                WriteMessage("Conexao com EZServer OK!");
        }

        //----------------------------------------------------------------------------
        private void timerAppLoop_Tick(object sender, EventArgs e)
        {
            if( chProcEvents.Checked == true )
               InternalProccessEvents();
            else  // Procssamento por Pooling
               ReadPumpsStatus();

        }

        //----------------------------------------------------------------------------
        private void ReadPumpsStatus()
        {
            int    PumpsCount = 0;
            String PumpStates = "";
            String CurrentHose = "";
            String DeliveriesCount = "";
            int    Idx = 0;
            int    CurStatus = 0;
            int    CurHose = 0;
            int    CurDelv = 0;
            String StrStatus = "";

            byte[] cstatus;
            byte[] chose;
            byte[] cdeliv;

            System.Text.ASCIIEncoding conv = new System.Text.ASCIIEncoding();

            // Verifica se esta conectado ao servidor
            if( EZInterface.TestConnection() == 0)
            {
              // Verifica se esta conectado ao servidor
              if( !GoodResult( EZInterface.GetPumpsCount( ref PumpsCount ) ) )
                  return;

              // Le o estado de todas as bombas configuradas
              if (!GoodResult(EZInterface.GetAllPumpStatuses(ref PumpStates, ref CurrentHose, ref DeliveriesCount)))
                  return;

              cstatus = conv.GetBytes(PumpStates);
              chose   = conv.GetBytes(CurrentHose);
              cdeliv  = conv.GetBytes(DeliveriesCount);

              for( Idx=1; Idx <= PumpsCount; Idx++ )
              {

                CurStatus = cstatus[Idx - 1] - '0'; // EZClient.TPumpState(Ord(PumpStates[Idx])-Ord('0'));
                CurHose   = chose[Idx-1]-'0';
                CurDelv   = cdeliv[Idx-1]-'0';

                switch( (EZInterface.TPumpState)CurStatus )  																		    						// PAM10)
                {
                    case EZInterface.TPumpState.INVALID_PUMP_STATE: StrStatus = "estado invalido."; break;										            	// 0 - OFFLINE
                    case EZInterface.TPumpState.NOT_INSTALLED_PUMP_STATE: StrStatus = "nao instalada."; break;											        // 6 - CLOSE
                    case EZInterface.TPumpState.NOT_RESPONDING_1_PUMP_STATE: StrStatus = "Bomba nao responde."; break;									    	// 0 - OFFLINE
                    case EZInterface.TPumpState.IDLE_PUMP_STATE: StrStatus = "em espera (desocupada)."; break;								                	// 1 - IDLE
                    case EZInterface.TPumpState.PRICE_CHANGE_STATE: StrStatus = "troca de preco."; break;											            // 1 - IDLE
                    case EZInterface.TPumpState.AUTHED_PUMP_STATE: StrStatus = "Bomba Autorizada"; break;											            // 9 - AUTHORIZED
                    case EZInterface.TPumpState.CALLING_PUMP_STATE: StrStatus = "esperando autorizacao."; break;									            // 5 - CALL
                    case EZInterface.TPumpState.DELIVERY_STARTING_PUMP_STATE: StrStatus = "abastecimeneto iniciando."; break;									// 2 - BUSY
                    case EZInterface.TPumpState.DELIVERING_PUMP_STATE: StrStatus = "abastecendo."; break;												        // 2 - BUSY
                    case EZInterface.TPumpState.TEMP_STOPPED_PUMP_STATE: StrStatus = "parada temporaria (no meio de uma abastecimento) (STOP)."; break;     	// 8 - STOP
                    case EZInterface.TPumpState.DELIVERY_FINISHING_PUMP_STATE: StrStatus = "abastecimento finalizando (fluxo de produto diminuindo)."; break;	// 2 - BUSY
                    case EZInterface.TPumpState.DELIVERY_FINISHED_PUMP_STATE: StrStatus = "abastecimento finalizado (parou de sair combustivel)."; break;		// 2 - BUSY
                    case EZInterface.TPumpState.DELIVERY_TIMEOUT_PUMP_STATE: StrStatus = "abastecimento excedeu tempo maximo."; break;						    // 1 - IDLE
                    case EZInterface.TPumpState.HOSE_OUT_PUMP_STATE: StrStatus = "bico fora do guarda-bico (CALL)."; break;					            		// 5 - CALL
                    case EZInterface.TPumpState.PREPAY_REFUND_TIMEOUT_STATE: StrStatus = "prazo de pre-determinacao esgotado."; break;					        // 1 - IDLE
                    case EZInterface.TPumpState.DELIVERY_TERMINATED_STATE: StrStatus = "abastecimento terminado (EOT)"; break;							    	// 3 - EOT
                    case EZInterface.TPumpState.ERROR_PUMP_STATE: StrStatus = "Erro (resposta de erro da bomba)."; break;				            			// 0 - OFFLINE
                    case EZInterface.TPumpState.NOT_RESPONDING_2_PUMP_STATE: StrStatus = "EZID nao responde."; break;
                    case EZInterface.TPumpState.LAST_PUMP_STATE: StrStatus = "Ultimo estado da bomba?"; break;
                    default: 
                        StrStatus = "estado desconhecido = " + CurStatus;         
                        break;
                }

                if (lastStates[Idx-1] != cstatus[Idx - 1])
                {
                    WriteMessage("Bomba " + Idx + ", Bico " + chose[Idx - 1] +
                                             ", Pendentes " + cdeliv[Idx - 1] +
                                               ", Status: " + StrStatus );

                  //lastStates.IndexOf(PumpStates[Idx], Idx);

                  lastStates[Idx-1] = cstatus[Idx-1];
                }

              }
            }
        }

        //----------------------------------------------------------------------------
        private void InternalProccessEvents()
        {
            int EvtCt = 0;
            short EvtType = 0;

            // Verifica se esta conectado ao servidor
            if( EZInterface.TestConnection() != 0 )
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
                        break ;

                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.LOG_EVENT_EVENT: // Log eventos
                        EventLog();
                        break;

                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.DB_TANK_STATUS: // eventos de Tanque
                        EventTank();
                        break;

                    //---------------------------------------------------------------------
                    case EZInterface.ClientEvent.NO_CLIENT_EVENT:  // Trata Eventos do Cliente
                        return ;

                    //---------------------------------------------------------------------
                    default:
                        GoodResult(EZInterface.DiscardNextEvent());
                        break;
                }
            }
        }

        //-----------------------------------------------------------------------------
        private void EventPump()
        {
            int    PumpID = 0;
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
            if (EZInterface.TestConnection() != 0)
                return;

	        if( GoodResult( EZInterface.GetNextPumpEventEx3(ref PumpID,           ref PumpNumber,         ref State,
													        ref ReservedFor,      ref ReservedBy,         ref HoseID,
													        ref HoseNumber,       ref HosePhysicalNumber,
													        ref GradeID,          ref GradeNumber,        ref GradeName,
													        ref ShortGradeName,   ref PriceLevel,         ref Price,
													        ref Volume,           ref Value,              ref StackSize,
													        ref PumpName,         ref PhysicalNumber,     ref Side,
													        ref Address,          ref PriceLevel1,        ref PriceLevel2,
													        ref PumpType,         ref PortID,             ref AuthMode,
													        ref StackMode,        ref PrepayAllowed,      ref PreauthAllowed,
													        ref PriceFormat,      ref ValueFormat,        ref VolumeFormat,
													        ref Tag,              ref AttendantID,        ref AttendantNumber,
													        ref AttendantName,    ref AttendantTag,       ref CardClientID,
													        ref CardClientNumber, ref CardClientName,	  ref CardClientTag,
                                                            ref CurFlowRate,      ref PeakFlowRate) ) )
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
                           ", CurFlowRate= "   + CurFlowRate +
                            ", PeakFlowRate= " + PeakFlowRate );

            WriteMessage("             Bico Equivalente CBC: " + CompanyID((short)HoseNumber, (short)PumpNumber));
	        }
        }

        //-----------------------------------------------------------------------------
        private void EventDelivery()
        {
            int    DeliveryID = 0;
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

            //String cbcID = "";

            String TankName = "";
            String GradeShortName = "";
            String GradeCode = "";
            String PumpName = "";
            String GradeName = "";
            String AttendantName = "";
            String CardClientName = "";

            // Verifica se esta conectado ao servidor
            if (EZInterface.TestConnection() != 0)
                return;

            if( GoodResult( EZInterface.GetNextDeliveryEventEx4(ref DeliveryID,         ref HoseID,           ref HoseNumber,
												                ref HosePhysicalNumber, ref PumpID,           ref PumpNumber,
												                ref PumpName,           ref TankID,           ref TankNumber,    ref TankName,
												                ref GradeID,            ref GradeNumber,      ref GradeName,     ref GradeShortName,
												                ref GradeCode,          ref DeliveryState,    ref DeliveryType,  ref Volume,
												                ref PriceLevel,         ref Price,            ref Value,         ref Volume2, ref CompletedDT,
												                ref LockedBy,           ref ReservedBy,       ref AttendantID,   ref Age,     ref ClearedDT,
												                ref OldVolumeETot,      ref OldVolume2ETot,   ref OldValueETot,
												                ref NewVolumeETot,      ref NewVolume2ETot,   ref NewValueETot,  ref Tag,
												                ref Duration,           ref AttendantNumber,  ref AttendantName, ref AttendantTag,
												                ref CardClientID,       ref CardClientNumber, ref CardClientName,
												                ref CardClientTag,      ref PeakFlowRate ) ) )
            {

	            // Primeiro abastecimento pode ser invalido
	            if( DeliveryID>0 )
	            {
                    WriteMessage("       DeliveryEvent: " +
                                        " DeliveryID= " + DeliveryID +
                                           ", HoseID= " + HoseID +
                                       ", HoseNumber= " + HoseNumber +
                               ", HosePhysicalNumber= " + HosePhysicalNumber +
                                           ", PumpID= " + PumpID +
                                       ", PumpNumber= " + PumpNumber +
                                         ", PumpName= " + PumpName +
                                           ", TankID= " + TankID+
                                       ", TankNumber= " + TankNumber+
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
                                    ",PeakFlowRate= " + PeakFlowRate );

                    WriteMessage("            Bico Equivalente CBC: " + CompanyID((short)HoseNumber, (short)PumpNumber));


		            if( LockedBy==-1 )
		            {
			            if( GoodResult( EZInterface.LockDelivery( DeliveryID ) ) )
				            LockedBy=1;

                        if ((LockedBy == 1) && (DeliveryState != (short)EZInterface.TDeliveryState.CLEARED))
			              GoodResult( EZInterface.ClearDelivery( DeliveryID , DeliveryType ) ) ;
		            }
	            }
            }
        }

        //-----------------------------------------------------------------------------
        private void EventCardRead()
        {
            int       CardReadID = 0;
            int       Number = 0;
            short     CardType = 0;
            int       ParentID = 0;
            DateTime  TimeStamp = new DateTime();
	        Int64     Tag = 0;
	        int       PumpID = 0;

	        String    Name = "";

            // Verifica se esta conectado ao servidor
            if (EZInterface.TestConnection() != 0)
                return;

	        if( GoodResult( EZInterface.GetNextCardReadEvent(ref CardReadID, ref Number,   ref Name,
												             ref PumpID,     ref CardType, ref ParentID,
												             ref Tag,        ref TimeStamp) ) )
	        {

                WriteMessage("\n------ CardReadEvent:  CardReadID " + CardReadID +
                                                        ", Number " + Number +
                                                          ", Name " + Name +
                                                        ", PumpID " + PumpID);

                WriteMessage("                         CardType " + CardType +
                                                    ", ParentID " + ParentID +
                                                         ", Tag " + Tag +
                                                  ",  TimeStamp " + TimeStamp );

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

		        GoodResult( EZInterface.DeleteCardRead( CardReadID )) ;
	        }
        }

        //-----------------------------------------------------------------------------
        private void EventDbLogETotals()
        {
	        int    HoseID = 0;
	        double Volume = 0;
            double Value = 0;
            double VolumeETot = 0;
            double ValueETot = 0;
            int    HoseNumber = 0;
            int    HosePhysicalNumber = 0;
            int    PumpID = 0;
            int    PumpNumber = 0;
	        int    TankID = 0;
            int    TankNumber = 0;
            int    GradeID = 0;

            String   PumpName  = "";
            String   TankName  = "";
            String   GradeName = "";

            // Verifica se esta conectado ao servidor
            if (EZInterface.TestConnection() != 0)
                return;

	        if( GoodResult( EZInterface.GetNextDBHoseETotalsEventEx(ref HoseID,     ref Volume,     ref Value,
   													                ref VolumeETot, ref ValueETot,
													                ref HoseNumber, ref HosePhysicalNumber,
													                ref PumpID,     ref PumpNumber, ref PumpName,
													                ref TankID,     ref TankNumber, ref TankName,
													                ref GradeID,    ref GradeName) ) )
	        {
		        WriteMessage("------ HoseETotalEvent:  HoseID " + HoseID + ",  Volume " + Volume + "  Value " + Value);
		        WriteMessage("             VolumeETot " + VolumeETot + ",  ValueETot " + ValueETot);
		        WriteMessage("             HoseNumber " + HoseNumber + ",  HosePhysicalNumber " + HosePhysicalNumber);
		        WriteMessage("             PumpID " + PumpID + ",  PumpNumber " + PumpNumber + ",  PumpName " + PumpName);
		        WriteMessage("             TankID " + TankID + ",  TankNumber " + TankNumber + ", TankName " +  TankName);
		        WriteMessage("             GradeID " + GradeID + ",  GradeName " + GradeName);
	        }
        }

        //-----------------------------------------------------------------------------
        private void EventServer()
        {
	        int EventID = 0;

	        String EventText = "";

            // Verifica se esta conectado ao servidor
            if (EZInterface.TestConnection() != 0)
                return;

	        if( GoodResult( EZInterface.GetNextServerEvent(ref EventID, ref EventText) ) )
		        WriteMessage("------ ServerEvent:   EventID " + EventID + ",  EventText " + EventText);

        }

        //-----------------------------------------------------------------------------
        private void EventClient()
        {
	        int EventID = 0;
	        short ClientID = 0;

	        String EventText = "";

            // Verifica se esta conectado ao servidor
            if (EZInterface.TestConnection() != 0)
                return;

	        if( GoodResult( EZInterface.GetNextClientEvent(ref ClientID, ref EventID, ref EventText) ) )
		        WriteMessage("------ ClientEvent: ClientID " + ClientID + ",  EventID " + EventID + ",  EventText " + EventText);

        }

        //-----------------------------------------------------------------------------

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
                WriteMessage("------ ZigBeeEvent:   PortID " + PortID + ",  Endereço ZigBee " + ZBAddress + ", LQI " + LQI + ", RSSI " + RSSI + ", ParZBAddress " + ParZBAddress + ", Canal " + ZBChannel + ", Memória Bloqueada " + MemBlocks + ", Memória Livre " + MemFree );
        }

        //-----------------------------------------------------------------------------

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
            if (EZInterface.TestConnection() != 0)
                return;

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
                    ", Temperature " + Temperature );
            }
        }

        //-----------------------------------------------------------------------------

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
            String TankName	= "";
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
            if (EZInterface.TestConnection() != 0)
                return;

            if (GoodResult(EZInterface.GetNextDBTankStatusEventEx2(ref TankID ,
            ref GaugeVolume , ref GaugeTCVolume , ref GaugeUllage ,
            ref GaugeTemperature , ref GaugeLevel , ref GaugeWaterVolume ,
            ref GaugeWaterLevel , ref TankNumber , ref TankName	,
            ref GradeID , ref GradeName , ref Type ,	
            ref Capacity , ref Diameter , ref GaugeID ,	
            ref ProbeNo , ref State , ref AlarmsMask )))
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

        //-----------------------------------------------------------------------------

        private void btLoadConfig_Click(object sender, EventArgs e)
        {
            ListGrades();
            ListTanks();
            ListSensors();
            ListPumps();
            ListZigbee();
            ListHoses();
        }

        //----------------------------------------------------------------------------
        // Lista configuracao de produtos
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
            if( !GoodResult( EZInterface.GetGradesCount( ref Ct ) ) )
              return;

            WriteMessage( "[Produtos" + Ct + "]---------------------------------------------------");

            for( Idx = 0 ; Idx < Ct ; Idx++ )
	        {

               if( EZInterface.GetGradeByOrdinal( Idx+1, ref Id ) != 0 )
                  return;

               if( GoodResult( EZInterface.GetGradePropertiesEx(Id, ref Number, ref Name, ref ShortName, ref Code, ref Type) ) )
                   WriteMessage("  Grade: " + Number + ",  Nome: " + Name + ",  Abreviado: " + ShortName + ", Codigo: " + Code + ", Tipo: " + Type);
	        }

	        WriteMessage( "");
        }

        //----------------------------------------------------------------------------
        private void ListTanks()
        {
            int    Idx = 0;
            int    Ct = 0;
            int    Id = 0;
            int    Number = 0;
            int    GradeID = 0;
            short  TType = 0;
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
            int    GaugeID = 0;
            short  ProbeNo = 0;
            int GaugeAlarmsMask = 0;

            String   Name = "";

            //--------------------------------------------------------------------
            // Ler o numero de produtos configurados
            if( !GoodResult( EZInterface.GetTanksCount( ref Ct ) ) )
              return;

	        WriteMessage("[Tanques " + Ct + "]---------------------------------------------------");

            for( Idx = 0 ; Idx < Ct ; Idx++ )
	        {
               if( EZInterface.GetTankByOrdinal( Idx, ref Id )!=0 )
                  return;

               if (GoodResult(EZInterface.GetTankPropertiesEx(Id, ref Number, ref Name, ref GradeID,
                                                                 ref TType, ref Capacity, ref Diameter,
                                                                 ref TheoVolume, ref GaugeVolume,
                                                                 ref GaugeTCVolume, ref GaugeUllage,
                                                                 ref GaugeTemperature, ref GaugeLevel,
                                                                 ref GaugeWaterVolume, ref GaugeWaterLevel,
                                                                 ref GaugeID, ref ProbeNo, ref GaugeAlarmsMask)))
	           {
                  WriteMessage("  Tanque: " + Number + ",  Nome: " + Name + ",  Produto: " + GradeID  + ",  Tipo: " + TType);
                  WriteMessage("     Capacidade: " + Capacity + "  Diametro: " + Diameter );
                  WriteMessage("     TheoVolume: " + TheoVolume + ",  GaugeVolume: " + GaugeVolume + ",  GaugeTCVolume: " + GaugeTCVolume);
                  WriteMessage("     GaugeUllage: " + GaugeUllage + ",   GaugeTemperature: " + GaugeTemperature + ",  GaugeLevel: " + GaugeLevel);
                  WriteMessage("     GaugeWaterVolume: " + GaugeWaterVolume + ",  GaugeWaterLevel: " + GaugeWaterLevel + ",  GaugeID: " + GaugeID);
                  WriteMessage("     ProbeNo: " + ProbeNo + "   GaugeAlarmsMask: " + GaugeAlarmsMask );
	           }

	        }

            WriteMessage("");
        }

        //----------------------------------------------------------------------------

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

            for (Idx = 0; Idx < Ct; Idx++)
            {
                if (EZInterface.GetSensorByOrdinal(Idx, ref Id) != 0)
                    return;

                if (GoodResult(EZInterface.GetSensorProperties( Id, ref Number, ref Name, ref PortID, ref Type, ref Address, ref SensorNo)))
                {
                    WriteMessage(   "  Sensor: " + Number + ",  Nome: " + Name + 
                                    ",  Porta: " + PortID + ",  Tipo: " + Type + 
                                    ",  Endereço: " + Address + ",  SensorNo: " + SensorNo);
                }

            }

            WriteMessage("");
        }

        //----------------------------------------------------------------------------

        private void ListPumps()
        {
            int   Idx = 0;
            int   Ct = 0;
            int   Id = 0;
            int   Number = 0;
            short PhysicalNumber = 0;
            short Side = 0;
            short Address = 0;
            short PriceLevel1 = 0;
            short PriceLevel2 = 0;
            short PriceDspFormat = 0;
            short VolumeDspFormat = 0;
            short ValueDspFormat = 0;
            short PType = 0;
            int   PortID = 0;
            int   AttendantID = 0;
            short AuthMode = 0;
            short StackMode = 0;
            short PrepayAllowed = 0;
            short PreauthAllowed = 0;
            int   SlotZigBeeID = 0;
            int   MuxSlotZigBeeID = 0;
            short PriceControl = 0;
            short HasPreset = 0;

            String   Name = "";

            //--------------------------------------------------------------------
            // Ler o numero de produtos configurados
            if( !GoodResult( EZInterface.GetPumpsCount( ref Ct ) ) )
              return;

	        WriteMessage( "[Bombas " + Ct + "]---------------------------------------------------");

            for ( Idx = 0; Idx < Ct; Idx++ )
	        {

               if( EZInterface.GetPumpByOrdinal( Idx, ref Id )!=0 )
                  return;

               if( GoodResult( EZInterface.GetPumpPropertiesEx(Id, ref Number,          ref Name,           ref PhysicalNumber,
                                                                   ref Side,            ref Address,        ref PriceLevel1,
                                                                   ref PriceLevel2,     ref PriceDspFormat, ref VolumeDspFormat,
														           ref ValueDspFormat,  ref PType,          ref PortID,
														           ref AttendantID,     ref AuthMode,       ref StackMode,
														           ref PrepayAllowed,   ref PreauthAllowed, ref SlotZigBeeID,
                                                                   ref MuxSlotZigBeeID, ref PriceControl,   ref HasPreset) ) )
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

        //----------------------------------------------------------------------------

        private void ListZigbee()
        {
            int Idx = 0;
            int Ct = 0;
            int Id = 0;
            int Number = 0;
            int PortID = 0;
            
            Int16 DeviceType = 0;

            String Name = "";
            String SerialNumber ="";
            String NodeIdentifier ="";

            //--------------------------------------------------------------------
            // Ler o numero de EZRemotes configurados
            if (!GoodResult(EZInterface.GetZigBeeCount(ref Ct)))
                return;

            WriteMessage("[EZRemotes " + Ct + "]---------------------------------------------------");

            for (Idx = 0; Idx < Ct; Idx++)
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

        //----------------------------------------------------------------------------
        private void ListHoses()
        {
            int    Idx = 0;
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
            // Ler o numero de produtos configurados
            if( !GoodResult( EZInterface.GetHosesCount( ref Ct ) ) )
              return;

            WriteMessage("[Bicos " + Ct + "]---------------------------------------------------");

            for (Idx = 0; Idx < Ct; Idx++)
	        {

               if( EZInterface.GetHoseByOrdinal( Idx, ref Id )!=0 )
                  return;

               if( GoodResult( EZInterface.GetHosePropertiesEx2( Id, ref Number, ref PumpID, ref TankID,
                                                                ref PhysicalNumber, ref MtrTheoValue,
                                                                ref MtrTheoVolume, ref MtrElecValue,
                                                                ref MtrElecVolume, ref UVEAntenna,
                                                                ref Price1, ref Price2,
                                                                ref Enabled ) ) )
	           {
                   WriteMessage("    Bico: " + Number + ",  PumpID: " + PumpID + ",  TankID: " + TankID + ",  PhisicalNumber: " + PhysicalNumber);
                   WriteMessage("        MtrTheoValue: " + MtrTheoValue + ",  MtrTheoVolume: " +MtrTheoVolume);
                   WriteMessage("        MtrElecValue: " + MtrElecValue + ",  MtrElecVolume: " + MtrElecVolume);
                   WriteMessage("        UVEAntena: " + UVEAntenna + ",  Price1: " + Price1 + ",  Price2: " + Price2 + ",  Enables: " + Enabled);
	           }
	        }
        }

        //---------------------------------------------------------------------
        private void btGetAllDeliveries_Click(object sender, EventArgs e)
        {
            int  Idx = 0;
            int  Ct = 0;
            int  Id = 0;

            int    HoseID = 0;
            short  State = 0;
            short  DType = 0;
            double Volume = 0;
            short  PriceLevel = 0;
            double Price = 0;
            double Value = 0;
            double Volume2 = 0;
            DateTime CompletedDT = new DateTime();
            int    LockedBy = 0;
            int    ReservedBy = 0;
            int    AttendantID = 0;
            int    Age = 0;
            DateTime ClearedDT = new DateTime();
            double OldVolumeETot = 0;
            double OldVolume2ETot = 0;
            double OldValueETot = 0;
            double NewVolumeETot = 0;
            double NewVolume2ETot = 0;
            double NewValueETot = 0;
            Int64  Tag = 0;
            int    Duration = 0;
            int    ClientID = 0;

            // Verifica se esta conectado ao servidor
            if (EZInterface.TestConnection() != 0)
                return;

            //--------------------------------------------------------------------
            // Le o numero de produtos configurados
            if( !GoodResult( EZInterface.GetDeliveriesCount( ref Ct ) ) )
              return;

	        WriteMessage("[Abastecimentos " + Ct + "]---------------------------------------------------");

            for(Idx=Ct; Idx>0; Idx--)
	        {

               if( !GoodResult( EZInterface.GetDeliveryByOrdinal( Idx, ref Id ) ) )
                  return;

               if( GoodResult( EZInterface.GetDeliveryPropertiesEx3(Id, ref HoseID, ref State, ref DType,
                                                                        ref Volume, ref PriceLevel, ref Price,
                                                                        ref Value, ref Volume2, ref CompletedDT,
                                                                        ref LockedBy, ref ReservedBy, ref AttendantID,
                                                                        ref Age, ref ClearedDT, ref OldVolumeETot,
                                                                        ref OldVolume2ETot, ref OldValueETot,
                                                                        ref NewVolumeETot, ref NewVolume2ETot,
                                                                        ref NewValueETot, ref Tag, ref Duration, ref ClientID ) ) )
	           {
                  WriteMessage( "------ Abastecimento: (" + Idx + ") " + Id);
                  WriteMessage( "           HoseID " + HoseID + ",  State " + State + ",  Type " + DType);
                  WriteMessage( "           Volume " + Volume + ",  PriceLevel " + PriceLevel + ",  Price " + Price + ",  Value " + Value);
		          WriteMessage( "           Volume2 " + Volume2 + ",  CompleteDT " + CompletedDT + ",  LockedBy " + LockedBy + ",  ReservedBy " + ReservedBy);
		          WriteMessage( "           AttendantID " + AttendantID + ",  Age " + Age + ",  ClearedDT " + ClearedDT);
                  WriteMessage( "           OldVolumeETot " + OldVolumeETot + ",  OldVolume2ETot " + OldVolume2ETot + ",  OldvalueETot " + OldValueETot);
		          WriteMessage( "           NewVolumeETot " + NewVolumeETot + ",  NewVolume2ETot " + NewVolume2ETot + ",  NewValueETot " + NewValueETot);
		          WriteMessage( "           Tag " + Tag + ",  Duraction " + Duration + ",   ClientID " + ClientID);
		          WriteMessage( "");

                  if( LockedBy != -1 )
                    continue;

                  if( GoodResult( EZInterface.LockDelivery( Id ) ) )
			        LockedBy = 1;
                  else
                    continue;

                  if ((LockedBy == 1) && (State != (short)EZInterface.TDeliveryState.CLEARED))
                    GoodResult( EZInterface.ClearDelivery( Id , DType ) ) ;

	           }
	        }

	        WriteMessage("------------------------------------------------------------------------");
        }

        private void btTotals_Click(object sender, EventArgs e)
        {
            int  IdBomba = 0;
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
                if (EZInterface.GetPumpHoseByNumber(IdBomba, Bico,  ref IdBico) != 0)
                    return;

                // Le dados do Bico
                if (GoodResult(EZInterface.GetHosePropertiesEx2(IdBico, ref Number, ref PumpID, ref TankID,
                                                                         ref PhysicalNumber, ref MtrTheoValue,
                                                                         ref MtrTheoVolume, ref MtrElecValue,
                                                                         ref MtrElecVolume, ref UVEAntenna,
                                                                         ref Price1, ref Price2, ref Enabled)))
                {
                    WriteMessage(" Bico " + Bico +",  EncVolume " + MtrElecVolume +",  EncDInheiro " + MtrElecValue +",  Preco1 " + Price1 +",  Preco2 " + Price2);
                    WriteMessage("    [ Number " + Number +",  PumpId " + PumpID +",  TankID " + TankID +",  PhisicalNumber " + PhysicalNumber +" ]");
                    WriteMessage("    [ MtrTheoValue " + MtrTheoValue + ", MtrTheoVolume " + MtrTheoVolume + ",  UVAntenna " + UVEAntenna + ",  Enabled " + Enabled + " ]");
                    WriteMessage("");
                }
            }
        }

        //---------------------------------------------------------------------
        private void btAuthorize_Click(object sender, EventArgs e)
        {
            int Bomba = 0;
            int IdBomba = 0;

            Bomba = cbPump.SelectedIndex + 1;   // Le o numero da bomba

            // Verifica conexao
            if( !GoodResult( EZInterface.TestConnection() ) )
              return;

            // Pega Id da Bomba escolhida
            if( !GoodResult( EZInterface.GetPumpByOrdinal(Bomba, ref IdBomba) ) )
              return;

            // Envia Autorizacao para bomba
            if( GoodResult( EZInterface.Authorise(IdBomba) ) )
                WriteMessage("--- Bomba " + Bomba + " Autorizada!");

        }

        //---------------------------------------------------------------------
        private void btLock_Click(object sender, EventArgs e)
        {
            int Bomba = 0;
            int IdBomba = 0;

            Bomba = cbPump.SelectedIndex + 1;   // Le o numero da bomba

            // Verifica conexao
            if( !GoodResult( EZInterface.TestConnection() ) )
              return;

            // Pega Id da Bomba escolhida
            if( !GoodResult( EZInterface.GetPumpByOrdinal(Bomba, ref IdBomba) ) )
              return;

            // Envia bloqueio (desautorizacao) para bomba
            if( GoodResult( EZInterface.CancelAuthorise(IdBomba) ) )
              WriteMessage("--- Bomba " + Bomba + " Desautorizada!");
        }

        //---------------------------------------------------------------------
        private void btChangePrice_Click(object sender, EventArgs e)
        {
            int    Bomba = 0;
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

            String   PumpName = "";
            String TankName = "";
            String GradeName = "";
            String GradeShortName = "";
            String GradeCode = "";

            Duracao = (short)EZInterface.TDurationType.MULTIPLE_DURATION_TYPE; // Duracao do preco (Multipos abastecimentos)
            Tipo    = (short)EZInterface.TPriceType.FIXED_PRICE_TYPE;  // Tipo de preco (Fixo)

            Bomba   = cbPump.SelectedIndex + 1;   // Le o numero da bomba
            Bico = cbHose.SelectedIndex + 1;
            Valor1 = Convert.ToDouble(edPrice1.Text);
            Valor2 = Convert.ToDouble(edPrice2.Text);

            WriteMessage("--- Bomba " + Bomba + " - Troca de precos");

            // Verifica conexao
            if (!GoodResult(EZInterface.TestConnection()))
                return;

            // Le o numero de bicos cadastrados
            if( !GoodResult( EZInterface.GetHosesCount(ref Bicos) ) )
              return;

            for(Index=1; Index<=Bicos; Index++)
	        {

              // Pega o ID do bico
              if( !GoodResult( EZInterface.GetHoseByOrdinal(Index, ref IdBico) ) )
                return;

              // Pega os dados do bico
              if( GoodResult( EZInterface.GetHoseSummaryEx(IdBico, ref HNumber,        ref PhysicalNumber,
                                                                   ref PumpID,         ref PumpNumber,  ref PumpName,
                                                                   ref TankID,         ref TankNumber,  ref TankName,
                                                                   ref GradeID,        ref GradeNumber, ref GradeName,
                                                                   ref GradeShortName, ref GradeCode,
                                                                   ref MtrTheoValue,   ref MtrTheoVolume,
                                                                   ref trElecValue,    ref MtrElecVolume, ref Price1,
                                                                   ref Price2,         ref HEnabled) ) )
              {

                // Verifica se o ID do bico pertence ao escolhido
                if( (Bomba==PumpNumber) && (Bico==HNumber) )
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

            // Faz ajuste do preço na bomba
        }

        //---------------------------------------------------------------------
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
            LType = (short) (cbPresetType.SelectedIndex + 2);
            PsValue = Convert.ToDouble(edPreset.Text);

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

        private void listMessageBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbHose_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}
