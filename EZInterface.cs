using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace EZClientCSharp
{
    public class EZInterface
    {
        public enum EClientType : short
        {
            SIMPLE_CLIENT_TYPE = 0,
            CALLS_CLIENT_TYPE = 1,
            EVENTS_CLIENT_TYPE = 2,
            DB_CLIENT_TYPE = 4
        }

        public enum ClientEvent : short
        {
            NO_CLIENT_EVENT,
            PUMP_EVENT,
            DELIVERY_EVENT,
            SERVER_EVENT,
            CLIENT_EVENT,
            DB_LOG_EVENT,
            DB_LOG_DELIVERY,
            DB_CLEAR_DELIVERY,
            DB_STACK_DELIVERY,
            DB_LOG_ETOTALS,
            DB_TRIGGER,
            DB_ATTENDANT_LOGON_EVENT,
            DB_ATTENDANT_LOGOFF_EVENT,
            DB_TANK_STATUS,
            SERIAL_PORT_EVENT,
            ZIGBEE_EVENT,
            UVE_EVENT,
            ZERO_DELIVERY_EVENT,
            ZB_STATUS_EVENT,
            ZB_PAN_EVENT,
            ZIGBEE_CMD_EVENT,
            ZIGBEE_RAW_EVENT,
            CARD_READ_EVENT ,
            ZB2G_STATUS_EVENT ,
            LOG_EVENT_EVENT
        }

        public enum ClientType : short
        {
            SIMPLE_CLIENT_TYPE = 0x00,
            CALLS_CLIENT_TYPE = 0x01,
            EVENTS_CLIENT_TYPE = 0x02,
            DB_CLIENT_TYPE = 0x04
        }

        public enum Result : int
        {
            INVALID_HEADER_VERSION = -1,
            INVALID_INTERFACE_VERSION = -2,
            INVLAID_INTERFACE_ID = -3,
            INVALID_FUNCTION_ID = -4,
            INVALID_SOURCE_ID = -5,
            INVALID_DESTINATION_ID = -6,
            INVALID_OBJECT_ID = -7,
            INVALID_SEQUENCE_NO = -8,
            MSG_IN_BUFFER_OVERFLOW = -9,
            MSG_OUT_BUFFER_OVERFLOW = -10,
            PARAMETER_TYPE_MISMATCH = -11,
            PARAMETER_COUNT_MISMATCH = -12,
            SERVER_TIMEOUT = -13,
            CONNECTION_BROKEN = -14,
            SOCKET_READ_ERROR = -15,
            NO_MSG_ERROR = -16,
            SOCKET_WRITE_ERROR = -17,
            SERVER_NONASYNC_CALL = -18,
            SOCKET_NOT_CONNECTED = -19,
            CLIENT_NOT_CONNECTED = -20,
            OCX_NOT_CONNECTED = -21,
            INVALID_CLIENT_TYPE = -22,
            INTERNAL_SERVER_ERROR = -23,
            CALL_SOCKET_CLOSED_RESULT = -24,
            OK_RESULT = 0,
            OBJECT_EXISTS_RESULT,                             // 01
            OBJECT_DOES_NOT_EXIST_RESULT,                     // 02
            OBJECT_HAS_DEPENDANCIES_RESULT,                   // 03
            INVALID_INTERFACE_RESULT,                         // 04
            INVALID_EVENTS_SOCKET_RESULT,                     // 05
            INVALID_OBJECT_LINK_RESULT,                       // 06
            INVALID_OBJECT_PARAMETER_RESULT,                  // 07
            NOT_LOGGED_ON_RESULT,                             // 08
            ALREADY_LOGGED_ON_RESULT,                         // 09
            INVALID_LOGON_RESULT,                             // 10
            INVALID_CLIENT_TYPE_RESULT,                       // 11
            PUMP_NOT_RESPONDING_RESULT,                       // 12
            PUMP_IN_USE_RESULT,                               // 13
            PUMP_ALREADY_RESERVED_RESULT,                     // 14
            PUMP_NOT_AVAILABLE_RESULT,                        // 15
            PUMP_NOT_RESERVED_RESULT,                         // 16
            PUMP_NOT_RESERVED_FOR_PREPAY_RESULT,              // 17
            PUMP_NOT_RESERVED_BY_YOU_RESULT,                  // 18
            INVALID_PRESET_TYPE_RESULT,                       // 19
            INVALID_HOSE_MASK_RESULT,                         // 20
            PUMP_NOT_RESERVED_FOR_PREAUTH_RESULT,             // 21
            PREPAYS_NOT_PREMITTED_RESULT,                     // 22
            PREAUTHS_NOT_PREMITTED_RESULT,                    // 23
            PUMP_CANNOT_BE_AUTHED_RESULT,                     // 24
            PUMP_NOT_AUTHED_RESULT,                           // 25
            NO_DELIVERY_AVAILABLE_RESULT,                     // 26
            STACK_IS_DISABLED_RESULT,                         // 27
            NO_CURRENT_DELIVERY_RESULT,                       // 28
            STACK_FULL_RESULT,                                // 29
            PUMP_NOT_STOPPED_RESULT,                          // 30
            PUMP_NOT_DELIVERING_RESULT,                       // 31
            INVALID_PRESET_AMOUNT_RESULT,                     // 32
            PUMP_IS_STOPPED_RESULT,                           // 33
            DELIVERY_ALREADY_LOCKED_RESULT,                   // 34
            DELIVERY_IS_RESERVED_RESULT,                      // 35
            DELIVERY_NOT_LOCKED_RESULT,                       // 36
            DELIVERY_NOT_LOCKED_BY_YOU_RESULT,                // 37
            DELIVERY_TYPE_CANNOT_BE_STACKED_RESULT,           // 38
            DELIVERY_CANNOT_BE_CLEARED_AS_THIS_TYPE_RESULT,   // 39
            DELIVERY_NOT_CURRENT_RESULT,                      // 40
            INVALID_CLIENT_ID_RESULT,                         // 41
            DELIVERY_TERMINATED_RESULT,                       // 42
            HAS_CURRENT_DELIVERY_RESULT,                      // 43
            ATTENDANT_NOT_LOGGED_ON_RESULT,                   // 44
            ATTENDANT_ALREADY_LOGGED_ON_RESULT,               // 45
            PUMP_IN_WRONG_AUTH_MODE_RESULT,                   // 46
            PUMP_HAS_DELIVERIES_RESULT,                       // 47
            SERVER_NOT_LICENSED_RESULT,                       // 48
            NO_EZMOD_RESULT,                                  // 49
            LICENSE_EXPIRED_RESULT,                           // 50
            CTF_NOT_PREMITTED_RESULT,                         // 51
            PUMP_NOT_RESERVED_FOR_CTF_RESULT,                 // 52
            ZIGBEE_MODULE_TYPE_ERROR_RESULT,                  // 53 
            DELAY_LOGON_RESULT,                               // 54 
            STANDALONE_AUTHMODE_ERROR_RESULT,                 // 55 
            SERVER_CLIENT_INCOMPATIBLE_ERROR_RESULT,          // 56 
            TAG_ALREADY_IN_USE_ERROR_RESULT,                  // 57 
            LOG_EVENT_ALREADY_ACKED_RESULT,                   // 
            LAST_RESULT // must be last
        }

        public enum TPumpState : short
        {
            INVALID_PUMP_STATE = 0,
            NOT_INSTALLED_PUMP_STATE,
            NOT_RESPONDING_1_PUMP_STATE,
            IDLE_PUMP_STATE,
            PRICE_CHANGE_STATE,
            AUTHED_PUMP_STATE,
            CALLING_PUMP_STATE,
            DELIVERY_STARTING_PUMP_STATE,
            DELIVERING_PUMP_STATE,
            TEMP_STOPPED_PUMP_STATE,
            DELIVERY_FINISHING_PUMP_STATE,
            DELIVERY_FINISHED_PUMP_STATE,
            DELIVERY_TIMEOUT_PUMP_STATE,
            HOSE_OUT_PUMP_STATE,
            PREPAY_REFUND_TIMEOUT_STATE,
            DELIVERY_TERMINATED_STATE,
            ERROR_PUMP_STATE,
            NOT_RESPONDING_2_PUMP_STATE,
            LAST_PUMP_STATE
        }

        public enum TPumpReserve : short
        {
            UNKNOWN_RESERVE,
            NOT_RESERVED,
            RESERVED_FOR_PREPAY,
            AUTHED_FOR_PREPAY,
            RESERVED_FOR_PREAUTH,
            AUTHED_FOR_PREAUTH,
            RESERVED_FOR_CTF,
            AUTHED_FOR_CTF,
            RESERVED_FOR_PAYMENT,
            AUTHED_FOR_PAYMENT,
        }

        public enum TAuthMode : short
        {
            INVALID_AUTH_MODE,
            NOT_AUTHABLE,
            COMP_AUTH,
            AUTO_AUTH,
            MONITOR_AUTH,
            ATTENDANT_AUTH,
            ATTENDANT_MONITOR_AUTH,
            CTF_AUTH,
            TAG_AUTH,
            OFFLINE_AUTH,
            ATTENDANT_TAG_AUTH,
            CLIENT_TAG_AUTH,
            ATTENDANT_AND_CLIENT_TAG_AUTH,
            ATTENDANT_OR_CLIENT_TAG_AUTH,
            EXT_AUTH,
        }

        public enum TTagType : short
        {
            INVALID_TAG_TYPE,
            ATTENDANT_TAG_TYPE,
            BLOCKED_ATTENDANT_TAG_TYPE,
            WRONG_SHIFT_ATTENDANT_TAG_TYPE,
            CLIENT_TAG_TYPE,
            BLOCKED_CLIENT_TAG_TYPE,
            UNKNOWN_TAG_TYPE
        }

        public enum TStackMode : short
        {
            INVALID_STACK_MODE,
            STACK_DISABLED,
            STACK_MANUAL,
            STACK_AUTO
        }

        public enum TDeliveryState : short
        {
            UNKNOWN_DEL_STATE,
            CURRENT,
            STACKED,
            CLEARED
        }

        public enum TDeliveryType : short
        {
            UNKNOWN_DEL_TYPE,
            POSTPAY,
            PREPAY,
            PREPAY_REFUND,
            PREAUTH,
            MONITOR,
            TEST,
            DRIVEOFF,
            OFFLINE,
            CTF,
            CARD_CLIENT,
            PAYMENT,

        }


        public enum TPriceType : short
        {
            UNKNOW_PRICE_TYPE,
            FIXED_PRICE_TYPE,
            DISCOUNT_PRICE_TYPE,
            SURCHARGE_PRICE_TYPE,
        }

        public enum TDurationType : short
        {
            UNKNOWN_DURATION_TYPE,
            SINGLE_DURATION_TYPE,
            MULTIPLE_DURATION_TYPE,
        }


        public enum TAllocLimitType : short
        {
            INVALID_LIMIT_TYPE = 0,
            NO_LIMIT_TYPE,
            DOLLAR_LIMIT_TYPE,
            VOLUME_LIMIT_TYPE
        }


        // const string DllName = "\\Work200\\EZClientDLL\\x64\\Release\\EZClient64.DLL";
        const string DllName = "C:\\EZForecourt\\EZClient64.dll";

        //--------------------------------- Connection -----------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 ClientLogon(Int32 ClientID, Int16 ClientType, Int32 EventHandle, System.IntPtr hWnd, Int32 wMsg);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 ClientLogonEx(Int32 ClientID, Int16 ClientType, [MarshalAs(UnmanagedType.BStr)] string Server, Int16 CallPortNo, Int16 EventsPortNo, Int32 CallTimeout, Int32 EventHandle, System.IntPtr hWnd, Int32 wMsg);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DllVersion([MarshalAs(UnmanagedType.BStr)] ref string VersionStr);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 ServerVersion([MarshalAs(UnmanagedType.BStr)] ref string VersionStr);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 ClientLogoff();

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 ClientStatus(ref Int16 PumpsReserved, ref Int16 DeliveriesLocked);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 TestConnection();

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 LicenseStatus();

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetLicenseType(ref Int16 LicenseType);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetIniValue([MarshalAs(UnmanagedType.BStr)] string Section, [MarshalAs(UnmanagedType.BStr)] string Key, [MarshalAs(UnmanagedType.BStr)] ref string Value);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetIniValue([MarshalAs(UnmanagedType.BStr)] string Section, [MarshalAs(UnmanagedType.BStr)] string Key, [MarshalAs(UnmanagedType.BStr)] string Value);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetClientsCount(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetDateTime(DateTime DateTime);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDateTime(ref DateTime DateTime);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.BStr)]
        internal static extern string ResultString(Int32 Res);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 CheckSocketClosed(Int32 lParam);

        //--------------------------------- Events --------------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 ProcessEvents();

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetEventsCount(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextEventType(ref Int16 Type);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DiscardNextEvent();

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextPumpEvent(ref Int32 PumpID, ref Int16 PumpNumber, ref Int16 State, ref Int16 ReservedFor, ref Int32 ReservedBy, ref Int32 HoseID, ref Int16 HoseNumber, ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price, ref Double Volume, ref Double Value, ref Int16 StackSize);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextPumpEventEx(ref Int32 PumpID, ref Int16 PumpNumber, ref Int16 State, ref Int16 ReservedFor, ref Int32 ReservedBy, ref Int32 HoseID,
            ref Int16 HoseNumber, ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price, ref Double Volume,
            ref Double Value, ref Int16 StackSize, [MarshalAs(UnmanagedType.BStr)] ref string PumpName, ref Int16 PhysicalNumber, ref Int16 Side, ref Int16 Address,
            ref Int16 PriceLevel1, ref Int16 PriceLevel2, ref Int16 Type, ref Int32 PortID, ref Int16 AuthMode, ref Int16 StackMode, ref Int16 PrepayAllowed,
            ref Int16 PreauthAllowed, ref Int16 PriceFormat, ref Int16 ValueFormat, ref Int16 VolumeFormat);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextPumpEventEx2(ref Int32 PumpID, ref Int32 PumpNumber, ref Int16 State, ref Int16 ReservedFor, ref Int32 ReservedBy, ref Int32 HoseID,
            ref Int32 HoseNumber, ref Int32 HosePhysicalNumber, ref Int32 GradeID, ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price, ref Double Volume,
            ref Double Value, ref Int16 StackSize, [MarshalAs(UnmanagedType.BStr)] ref string PumpName, ref Int32 PhysicalNumber, ref Int16 Side, ref Int16 Address,
            ref Int16 PriceLevel1, ref Int16 PriceLevel2, ref Int16 Type, ref Int32 PortID, ref Int16 AuthMode, ref Int16 StackMode, ref Int16 PrepayAllowed,
            ref Int16 PreauthAllowed, ref Int16 PriceFormat, ref Int16 ValueFormat, ref Int16 VolumeFormat, ref Int64 Tag,
            ref Int32 AttendantID, ref Int32 AttendantNumber, [MarshalAs(UnmanagedType.BStr)] ref string AttendantName, ref Int64 AttendantTag,
            ref Int32 CardClientID, ref Int32 CardClientNumber, [MarshalAs(UnmanagedType.BStr)] ref string CardClientName, ref Int64 CardClientTag);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextPumpEventEx3(ref Int32 PumpID, ref Int32 PumpNumber, ref Int16 State, ref Int16 ReservedFor, ref Int32 ReservedBy, ref Int32 HoseID,
            ref Int32 HoseNumber, ref Int32 HosePhysicalNumber, ref Int32 GradeID, ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price, ref Double Volume,
            ref Double Value, ref Int16 StackSize, [MarshalAs(UnmanagedType.BStr)] ref string PumpName, ref Int32 PhysicalNumber, ref Int16 Side, ref Int16 Address,
            ref Int16 PriceLevel1, ref Int16 PriceLevel2, ref Int16 Type, ref Int32 PortID, ref Int16 AuthMode, ref Int16 StackMode, ref Int16 PrepayAllowed,
            ref Int16 PreauthAllowed, ref Int16 PriceFormat, ref Int16 ValueFormat, ref Int16 VolumeFormat, ref Int64 Tag,
            ref Int32 AttendantID, ref Int32 AttendantNumber, [MarshalAs(UnmanagedType.BStr)] ref string AttendantName, ref Int64 AttendantTag,
            ref Int32 CardClientID, ref Int32 CardClientNumber, [MarshalAs(UnmanagedType.BStr)] ref string CardClientName, ref Int64 CardClientTag,
            ref Double CurFlowRate, ref Double PeakFlowRate);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.BStr)] internal static extern Int32 GetNextDeliveryEvent(ref Int32 DeliveryID, ref Int32 PumpID, ref Int16 PumpNumber, ref Int32 HoseID, ref Int16 HoseNumber, ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price, ref Double Volume, ref Double Value, ref Int16 DeliveryState, ref Int16 DeliveryType, ref Int32 LockedBy, ref Int32 ReservedBy, ref Int32 Age, ref DateTime CompletedDT, ref Int32 AttendantID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDeliveryEventEx(ref Int32 DeliveryID, ref Int32 PumpID, ref Int16 PumpNumber, ref Int32 HoseID, ref Int16 HoseNumber,
            ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price,
            ref Double Volume, ref Double Value, ref Int16 DeliveryState,
            ref Int16 DeliveryType, ref Int32 LockedBy, ref Int32 ReservedBy, ref Int32 Age, ref DateTime CompletedDT,
            ref Int32 AttendantID, ref Double VolumeETot, ref Double Volume2ETot, ref Double ValueETot);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDeliveryEventEx2(ref Int32 DeliveryID, ref Int32 PumpID, ref Int16 PumpNumber, ref Int32 HoseID, ref Int16 HoseNumber,
            ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price,
            ref Double Volume, ref Double Value, ref Int16 DeliveryState,
            ref Int16 DeliveryType, ref Int32 LockedBy, ref Int32 ReservedBy, ref Int32 Age, ref DateTime CompletedDT, ref Int32 AttendantID,
            ref Double OldVolumeETot, ref Double OldVolume2ETot, ref Double OldValueETot,
            ref Double NewVolumeETot, ref Double NewVolume2ETot, ref Double NewValueETot,
            ref Int64 Tag, ref Int32 Duration);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDeliveryEventEx3(ref Int32 DeliveryID, ref Int32 HoseID, ref Int32 HoseNumber, ref Int32 HosePhysicalNumber,
            ref Int32 PumpID, ref Int32 PumpNumber, [MarshalAs(UnmanagedType.BStr)] ref string PumpName,
            ref Int32 TankID, ref Int32 TankNumber, [MarshalAs(UnmanagedType.BStr)] ref string TankName,
            ref Int32 GradeID, ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string GradeShortName, [MarshalAs(UnmanagedType.BStr)] ref string GradeCode,
            ref Int16 State, ref Int16 Type, ref Double Volume, ref Int16 PriceLevel,
            ref Double Price, ref Double Value, ref Double Volume2, ref DateTime CompletedDT, ref Int32 LockedBy,
            ref Int32 ReservedBy, ref Int32 AttendantID, ref Int32 Age, ref DateTime ClearedDT,
            ref Double OldVolumeETot, ref Double OldVolume2ETot, ref Double OldValueETot,
            ref Double NewVolumeETot, ref Double NewVolume2ETot, ref Double NewValueETot,
            ref Int64 Tag, ref Int32 Duration, ref Int32 AttendantNumber, [MarshalAs(UnmanagedType.BStr)] ref string AttendantName, ref Int64 AttendantTag,
            ref Int32 CardClientID, ref Int32 CardClientNumber, [MarshalAs(UnmanagedType.BStr)] ref string CardClientName, ref Int64 CardClientTag);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDeliveryEventEx4(ref Int32 DeliveryID, ref Int32 HoseID, ref Int32 HoseNumber, ref Int32 HosePhysicalNumber,
            ref Int32 PumpID, ref Int32 PumpNumber, [MarshalAs(UnmanagedType.BStr)] ref string PumpName,
            ref Int32 TankID, ref Int32 TankNumber, [MarshalAs(UnmanagedType.BStr)] ref string TankName,
            ref Int32 GradeID, ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string GradeShortName, [MarshalAs(UnmanagedType.BStr)] ref string GradeCode,
            ref Int16 State, ref Int16 Type, ref Double Volume, ref Int16 PriceLevel,
            ref Double Price, ref Double Value, ref Double Volume2, ref DateTime CompletedDT, ref Int32 LockedBy,
            ref Int32 ReservedBy, ref Int32 AttendantID, ref Int32 Age, ref DateTime ClearedDT,
            ref Double OldVolumeETot, ref Double OldVolume2ETot, ref Double OldValueETot,
            ref Double NewVolumeETot, ref Double NewVolume2ETot, ref Double NewValueETot,
            ref Int64 Tag, ref Int32 Duration, ref Int32 AttendantNumber, [MarshalAs(UnmanagedType.BStr)] ref string AttendantName, ref Int64 AttendantTag,
            ref Int32 CardClientID, ref Int32 CardClientNumber, [MarshalAs(UnmanagedType.BStr)] ref string CardClientName, ref Int64 CardClientTag, ref Double PeakFlowRate);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextServerEvent(ref Int32 EventID, [MarshalAs(UnmanagedType.BStr)]ref string EventText);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextClientEvent(ref Int16 ClientID, ref Int32 EventID, [MarshalAs(UnmanagedType.BStr)] ref string EventText);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 FireClientEvent(Int32 EventID, [MarshalAs(UnmanagedType.BStr)] string EventStr);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDBLogEvent(ref Int32 EventType, ref Int32 DeviceID, ref DateTime EventDT, [MarshalAs(UnmanagedType.BStr)] ref string EventText);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDBLogDeliveryEvent(ref Int32 DeliveryID, ref Int32 HoseID, ref Int16 DeliveryState, ref Int16 DeliveryType, ref Double Volume, ref Int16 PriceLevel, ref Double Price, ref Double Value, ref Double Volume2, ref Int32 ReservedBy, ref Int32 AttendantID, ref DateTime DeliveryDT);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextZB2GStatusEvent(ref Int32 PortID, ref Int64 ZBAddress, ref Int16 LQI, ref Int16 RSSI, ref Int64 ParZBAddress, ref Int16 ZBChannel, ref Int16 MemBlocks, ref Int16 MemFree);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDBClearDeliveryEvent(ref Int32 DeliveryID, ref Int16 DeliveryType, ref Int32 ClearedBY, ref DateTime ClearedDT, ref Int32 AttendantID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDBStackDeliveryEvent(ref Int32 DeliveryID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDBHoseETotalsEvent(ref Int32 HoseID, ref Double Volume, ref Double Value, ref Double VolumeETot, ref Double ValueETot);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDBHoseETotalsEventEx(ref Int32 HoseID, ref Double Volume, ref Double Value, ref Double VolumeETot, ref Double ValueETot,
            ref Int32 HoseNumber, ref Int32 HosePhysicalNumber, ref Int32 PumpID, ref Int32 PumpNumber, [MarshalAs(UnmanagedType.BStr)] ref string PumpName,
            ref Int32 TankID, ref Int32 TankNumber, [MarshalAs(UnmanagedType.BStr)] ref string TankName, ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDBTriggerEvent(ref Int32 TableID, ref Int32 RowID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextAttendantLogonEvent(ref Int32 AttendantID, ref Int32 PumpID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextAttendantLogoffEvent(ref Int32 AttendantID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDBTankStatusEvent(ref Int32 TankID, ref Double GaugeVolume, ref Double GaugeTCVolume, ref Double GaugeUllage, ref Double GaugeTemperature, ref Double GaugeLevel, ref Double GaugeWaterVolume, ref Double GaugeWaterLevel);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDBTankStatusEventEx(ref Int32 TankID, ref Double GaugeVolume, ref Double GaugeTCVolume, ref Double GaugeUllage,
            ref Double GaugeTemperature, ref Double GaugeLevel, ref Double GaugeWaterVolume, ref Double GaugeWaterLevel,
            ref Int32 TankNumber, [MarshalAs(UnmanagedType.BStr)] ref string TankName, ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, ref Int16 Type,
            ref Double Capacity, ref Double Diameter, ref Int32 GaugeID, ref Int16 ProbeNo);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextDBTankStatusEventEx2(ref Int32 TankID, ref Double GaugeVolume, ref Double GaugeTCVolume, ref Double GaugeUllage,
            ref Double GaugeTemperature, ref Double GaugeLevel, ref Double GaugeWaterVolume, ref Double GaugeWaterLevel,
            ref Int32 TankNumber, [MarshalAs(UnmanagedType.BStr)] ref string TankName, ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, ref Int16 Type,
            ref Double Capacity, ref Double Diameter, ref Int32 GaugeID, ref Int16 ProbeNo, ref Int16 State, ref Int32 AlarmsMask);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextCardReadEvent(ref Int32 CardReadID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, ref Int32 PumpID, ref Int16 Type, ref Int32 ParentID, ref Int64 Tag, ref DateTime TimeStamp);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextLogEventEvent(ref Int32 LogEventID, ref Int16 DeviceType, ref Int32 DeviceID, ref Int32 DeviceNumber, [MarshalAs(UnmanagedType.BStr)] ref string DeviceName, ref Int16 EventLevel,
            ref Int16 EventType, [MarshalAs(UnmanagedType.BStr)] ref string EventDesc, ref DateTime GeneratedDT, ref DateTime ClearedDT, ref Int32 ClearedBy, ref Int32 AckedBy, ref Double Volume,
            ref Double Value, ref Double ProductVolume, ref Double ProductLevel, ref Double WaterLevel, ref Double Temperature);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetNextZeroDeliveryEvent(ref Int32 PumpID, ref Int32 PumpNumber, ref Int32 HoseID, ref Int32 HoseNumber);

        //--------------------------------- Pumps ---------------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpsCount(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpByNumber(Int32 Number, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpByName([MarshalAs(UnmanagedType.BStr)] string Name, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpByOrdinal(Int32 Index, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpProperties(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, ref Int16 PhysicalNumber, ref Int16 Side, ref Int16 Address, ref Int16 PriceLevel1, ref Int16 PriceLevel2, ref Int16 PriceDspFormat, ref Int16 VolumeDspFormat, ref Int16 ValueDspFormat, ref Int32 Type, ref Int32 PortID, ref Int32 AttendantID, ref Int16 AuthMode, ref Int16 StackMode, ref Int16 PrepayAllowed, ref Int16 PreauthAllowed);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpPropertiesEx(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, ref Int16 PhysicalNumber, ref Int16 Side, ref Int16 Address, ref Int16 PriceLevel1,
            ref Int16 PriceLevel2, ref Int16 PriceDspFormat, ref Int16 VolumeDspFormat, ref Int16 ValueDspFormat, ref Int16 Type,
            ref Int32 PortID, ref Int32 AttendantID, ref Int16 AuthMode, ref Int16 StackMode, ref Int16 PrepayAllowed, ref Int16 PreauthAllowed,
            ref Int32 SlotZigBeeID, ref Int32 MuxSlotZigBeeID, ref Int16 PriceControl, ref Int16 HasPreset);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetPumpProperties(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, Int16 PhysicalNumber, Int16 Side, Int16 Address, Int16 PriceLevel1, Int16 PriceLevel2, Int16 PriceDspFormat, Int16 VolumeDspFormat, Int16 ValueDspFormat, Int32 Type, Int32 PortID, Int32 AttendantID, Int16 AuthMode, Int16 StackMode, Int16 PrepayAllowed, Int16 PreauthAllowed);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetPumpPropertiesEx(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, Int16 PhysicalNumber, Int16 Side, Int16 Address, Int16 PriceLevel1,
            Int16 PriceLevel2, Int16 PriceDspFormat, Int16 VolumeDspFormat, Int16 ValueDspFormat, Int16 Type,
            Int32 PortID, Int32 AttendantID, Int16 AuthMode, Int16 StackMode, Int16 PrepayAllowed, Int16 PreauthAllowed,
            Int32 SlotZigBeeID, Int32 MuxSlotZigBeeID, Int16 PriceControl, Int16 HasPreset);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DeletePump(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpHosesCount(Int32 ID, ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpHoseByNumber(Int32 ID, Int32 Number, ref Int32 HoseID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpStatus(Int32 ID, ref Int16 Status, ref Int16 ReservedFor, ref Int32 ReservedBy, ref Int32 HoseID, ref Int16 HoseNumber, ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price, ref Double Volume, ref Double Value, ref Int16 StackSize);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpStatusEx(Int32 ID, ref Int32 PumpNumber, [MarshalAs(UnmanagedType.BStr)] ref string PumpName, ref Int32 PhysicalNumber, ref Int16 Status, ref Int16 ReservedFor, ref Int32 ReservedBy, ref Int32 HoseID, ref Int16 HoseNumber, ref Int32 HosePhysicalNumber, ref Int32 GradeID, ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price, ref Double Volume, ref Double Value, ref Int16 StackSize,
                       ref Int64 Tag, ref Int32 AttendantID, ref Int32 AttendantNumber, [MarshalAs(UnmanagedType.BStr)] ref string AttendantName, ref Int64 AttendantTag, ref Int32 CardClientID, ref Int32 CardClientNumber, [MarshalAs(UnmanagedType.BStr)] ref string CardClientName, ref Int64 CardClientTag);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpStatusEx2(Int32 ID, ref Int32 PumpNumber, [MarshalAs(UnmanagedType.BStr)] ref string PumpName, ref Int32 PhysicalNumber, ref Int16 Status, ref Int16 ReservedFor, ref Int32 ReservedBy, ref Int32 HoseID, ref Int16 HoseNumber, ref Int32 HosePhysicalNumber, ref Int32 GradeID, ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price, ref Double Volume, ref Double Value, ref Int16 StackSize,
                       ref Int64 Tag, ref Int32 AttendantID, ref Int32 AttendantNumber, [MarshalAs(UnmanagedType.BStr)] ref string AttendantName, ref Int64 AttendantTag, ref Int32 CardClientID, ref Int32 CardClientNumber, [MarshalAs(UnmanagedType.BStr)] ref string CardClientName, ref Int64 CardClientTag, ref Double CurFlowRate , ref Double PeakFlowRate );

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.BStr)]
        internal static extern string PumpStateString(Int16 State);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 EnablePump(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DisablePump(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetPumpDefaultPriceLevel(Int32 ID, Int16 Level);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDensity(Int32 ID, ref Double Density);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 ScheduleBeep(Int32 ID, Int16 Pitch1, Int16 Duration1, Int16 Pitch2, Int16 Duration2, Int16 Pitch3, Int16 Duration3, Int16 Pitch4, Int16 Duration4, Int16 Pitch5, Int16 Duration5);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 FlashLEDS(Int32 ID, Int16 Side, Int16 PeriodMs, Int16 Cycles);

        //--------------------------- Pump prepay deliveries ----------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 PrepayReserve(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 PrepayCancel(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 PrepayAuthorise(Int32 ID, Int16 LimitType, Double Value, Int16 Hose, Int16 PriceLevel);

        //--------------------------- Pump preauth deliveries ---------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 PreauthReserve(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 PreauthCancel(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 PreauthAuthorise(Int32 ID, Int16 LimitType, Double Value, Int16 Hose, Int16 PriceLevel);

        //--------------------------- Pump payment deliveries ---------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 PaymentReserve(Int32 ID, Int32 TermID, [MarshalAs(UnmanagedType.BStr)] string TermHash);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 PaymentCancel(Int32 ID, Int32 TermID, [MarshalAs(UnmanagedType.BStr)] string TermHash);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 PaymentAuthorise(Int32 ID, Int32 TermID, [MarshalAs(UnmanagedType.BStr)] string TermHash,
           Int32 AttendantID, Int64 AttendantTag,
           Int32 CardClientID, Int64 CardClientTag,
           short AuthType, Int64 ExtTag,
           Int16 GradeType, Int16 PriceType, Int16 PriceLevel, double Price,
           Int16 PresetType, Double Value, Int16 Hose,
           Double Odometer, Double Odometer2, [MarshalAs(UnmanagedType.BStr)] string Plate,
           [MarshalAs(UnmanagedType.BStr)] string ExtTransactionID, [MarshalAs(UnmanagedType.BStr)] string DriverID, [MarshalAs(UnmanagedType.BStr)] string AuthorisationID);

        //------------------------------ Pump authorization ------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 AttendantAuthorise(Int32 ID, Int32 AttendantID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 Authorise(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 CancelAuthorise(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 TempStop(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 ReAuthorise(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 TerminateDelivery(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 LoadPreset(Int32 ID, Int16 LimitType, Double Value, Int16 Hose, Int16 PriceLevel);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 LoadPresetWithPrice(Int32 ID, Int16 LimitType, Double Value, Int16 Hose, Int16 PriceLevel, Double Price);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 TagAuthorise(Int32 ID, Int64 Tag, Int16 LimitType, Double Value, Int16 Hose, Int16 PriceLevel);

        //-------------------------------- Global functions ------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 AllStop();

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 AllAuthorise();

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 AllReAuthorise();

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 AllStopIfIdle();

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 ReadAllTanks();

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetAllPumpStatuses([MarshalAs(UnmanagedType.BStr)] ref string States, [MarshalAs(UnmanagedType.BStr)] ref string CurrentHoses, [MarshalAs(UnmanagedType.BStr)] ref string DeliveriesCount);

        //------------------------------------ Deliveries --------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliveriesCount(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliveryByOrdinal(Int32 Index, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliveryProperties(Int32 ID, ref Int32 HoseID, ref Int16 State, ref Int16 Type, ref Double Volume, ref Int16 PriceLevel, ref Double Price, ref Double Value, ref Double Volume2, ref DateTime CompletedDT, ref Int32 LockedBy, ref Int32 ReservedBy, ref Int32 AttendantID, ref Int32 Age);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliveryPropertiesEx(Int32 ID, ref Int32 HoseID, ref Int16 State, ref Int16 Type, ref Double Volume, ref Int16 PriceLevel,
            ref Double Price, ref Double Value, ref Double Volume2, ref DateTime CompletedDT, ref Int32 LockedBy, ref Int32 ReservedBy, ref Int32 AttendantID, ref Int32 Age,
            ref DateTime ClearedDT, ref Double VolumeETot, ref Double Volume2ETot, ref Double ValueETot);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliveryPropertiesEx2(Int32 ID, ref Int32 HoseID, ref Int16 State, ref Int16 Type, ref Double Volume, ref Int16 PriceLevel,
            ref Double Price, ref Double Value, ref Double Volume2, ref DateTime CompletedDT, ref Int32 LockedBy, ref Int32 ReservedBy, ref Int32 AttendantID, ref Int32 Age,
            ref DateTime ClearedDT, ref Double OldVolumeETot, ref Double OldVolume2ETot, ref Double OldValueETot, ref Double NewVolumeETot, ref Double NewVolume2ETot, ref Double NewValueETot, ref Int64 Tag, ref Int32 Duration);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliveryPropertiesEx3(Int32 ID, ref Int32 HoseID, ref Int16 State, ref Int16 Type, ref Double Volume, ref Int16 PriceLevel,
            ref Double Price, ref Double Value, ref Double Volume2, ref DateTime CompletedDT, ref Int32 LockedBy, ref Int32 ReservedBy, ref Int32 AttendantID, ref Int32 Age,
            ref DateTime ClearedDT, ref Double OldVolumeETot, ref Double OldVolume2ETot, ref Double OldValueETot, ref Double NewVolumeETot, ref Double NewVolume2ETot, ref Double NewValueETot, ref Int64 Tag, ref Int32 Duration, ref Int32 CardClientID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliveryPropertiesEx4(Int32 ID, ref Int32 HoseID, ref Int16 State, ref Int16 Type, ref Double Volume, ref Int16 PriceLevel,
            ref Double Price, ref Double Value, ref Double Volume2, ref DateTime CompletedDT, ref Int32 LockedBy, ref Int32 ReservedBy, ref Int32 AttendantID, ref Int32 Age,
            ref DateTime ClearedDT, ref Double OldVolumeETot, ref Double OldVolume2ETot, ref Double OldValueETot, ref Double NewVolumeETot, ref Double NewVolume2ETot, ref Double NewValueETot, ref Int64 Tag, ref Int32 Duration, ref Int32 CardClientID, ref Double PeakFlowRate);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetDeliveryProperties(Int32 ID, Int32 HoseID, Int16 State, Int16 Type, Double Volume, Int16 PriceLevel, Double Price, Double Value, Double Volume2, DateTime CompletedDT, Int32 LockedBy, Int32 ReservedBy, Int32 AttendantID, Int32 Age);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetDeliveryPropertiesEx(Int32 ID, Int32 HoseID, Int16 State, Int16 Type, Double Volume, Int16 PriceLevel,
            Double Price, Double Value, Double Volume2, DateTime CompletedDT, Int32 LockedBy,
            Int32 ReservedBy, Int32 AttendantID, Int32 Age, DateTime ClearedDT, Double VolumeETot, Double Volume2ETot, Double ValueETot);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetDeliveryPropertiesEx2(Int32 ID, Int32 HoseID, Int16 State, Int16 Type, Double Volume, Int16 PriceLevel,
            Double Price, Double Value, Double Volume2, DateTime CompletedDT, Int32 LockedBy,
            Int32 ReservedBy, Int32 AttendantID, Int32 Age, DateTime ClearedDT, Double OldVolumeETot, Double OldVolume2ETot, Double OldValueETot,
            Double NewVolumeETot, Double NewVolume2ETot, Double NewValueETot,
            Int64 Tag, Int32 Duration);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetDeliveryPropertiesEx3(Int32 ID, Int32 HoseID, short State, short Type, Double Volume, short PriceLevel,
           Double Price, Double Value, Double Volume2, DateTime CompletedDT, Int32 LockedBy,
           Int32 ReservedBy, Int32 AttendantID, Int32 Age, DateTime ClearedDT,
           Double OldVolumeETot, Double OldVolume2ETot, Double OldValueETot,
           Double NewVolumeETot, Double NewVolume2ETot, Double NewValueETot,
           Int64 Tag, Int32 Duration, Int32 CardClientID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetDeliveryPropertiesEx4(Int32 ID, Int32 HoseID, short State, short Type, Double Volume, short PriceLevel,
           Double Price, Double Value, Double Volume2, DateTime CompletedDT, Int32 LockedBy,
           Int32 ReservedBy, Int32 AttendantID, Int32 Age, DateTime ClearedDT,
           Double OldVolumeETot, Double OldVolume2ETot, Double OldValueETot,
           Double NewVolumeETot, Double NewVolume2ETot, Double NewValueETot,
           Int64 Tag, Int32 Duration, Int32 CardClientID, Double PeakFlowRate);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 LockDelivery(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 UnlockDelivery(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 ClearDelivery(Int32 ID, Int16 Type);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 LockAndClearDelivery(Int32 ID, Int16 Type);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetNextDeliveryID(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 AckDeliveryDBLog(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliveryIDByOrdinalNotLogged(Int32 Ordinal, ref Int32 pID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliveriesCountNotLogged(ref Int32 pCount);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 AckDeliveryVolLog(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliveryIDByOrdinalNotVolLogged(Int32 Ordinal, ref Int32 pID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliveriesCountNotVolLogged(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetAllDeliveriesCount( ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetAllDeliveryByOrdinal( Int32 Index, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliverySummary(Int32 ID, ref Int32 HoseID, ref Int32 HoseNumber, ref Int32 HosePhysicalNumber,
            ref Int32 PumpID, ref Int32 PumpNumber, [MarshalAs(UnmanagedType.BStr)] ref string PumpName,
            ref Int32 TankID, ref Int32 TankNumber, [MarshalAs(UnmanagedType.BStr)] ref string TankName,
            ref Int32 GradeID, ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string GradeShortName, [MarshalAs(UnmanagedType.BStr)] ref string GradeCode,
            ref Int16 State, ref Int16 Type, ref Double Volume, ref Int16 PriceLevel,
            ref Double Price, ref Double Value, ref Double Volume2, ref DateTime CompletedDT, ref Int32 LockedBy,
            ref Int32 ReservedBy, ref Int32 AttendantID, ref Int32 Age, ref DateTime ClearedDT,
            ref Double VolumeETot, ref Double Volume2ETot, ref Double ValueETot);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliverySummaryEx(Int32 ID, ref Int32 HoseID, ref Int32 HoseNumber, ref Int32 HosePhysicalNumber,
            ref Int32 PumpID, ref Int32 PumpNumber, [MarshalAs(UnmanagedType.BStr)] ref string PumpName,
            ref Int32 TankID, ref Int32 TankNumber, [MarshalAs(UnmanagedType.BStr)] ref string TankName,
            ref Int32 GradeID, ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string GradeShortName, [MarshalAs(UnmanagedType.BStr)] ref string GradeCode,
            ref Int16 State, ref Int16 Type, ref Double Volume, ref Int16 PriceLevel,
            ref Double Price, ref Double Value, ref Double Volume2, ref DateTime CompletedDT, ref Int32 LockedBy,
            ref Int32 ReservedBy, ref Int32 AttendantID, ref Int32 Age, ref DateTime ClearedDT,
            ref Double OldVolumeETot, ref Double OldVolume2ETot, ref Double OldValueETot,
            ref Double NewVolumeETot, ref Double NewVolume2ETot, ref Double NewValueETot,
            ref Int64 Tag, ref Int32 Duration);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliverySummaryEx2(Int32 ID, ref Int32 HoseID, ref Int32 HoseNumber, ref Int32 HosePhysicalNumber,
            ref Int32 PumpID, ref Int32 PumpNumber, [MarshalAs(UnmanagedType.BStr)] ref string PumpName,
            ref Int32 TankID, ref Int32 TankNumber, [MarshalAs(UnmanagedType.BStr)] ref string TankName,
            ref Int32 GradeID, ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string GradeShortName, [MarshalAs(UnmanagedType.BStr)] ref string pGradeCode,
            ref Int16 State, ref Int16 Type, ref Double Volume, ref Int16 PriceLevel,
            ref Double Price, ref Double Value, ref Double Volume2, ref DateTime CompletedDT, ref Int32 LockedBy,
            ref Int32 ReservedBy, ref Int32 AttendantID, ref Int32 Age, ref DateTime ClearedDT,
            ref Double OldVolumeETot, ref Double OldVolume2ETot, ref Double OldValueETot,
            ref Double NewVolumeETot, ref Double NewVolume2ETot, ref Double NewValueETot,
            ref Int64 Tag, ref Int32 Duration, ref Int32 AttendantNumber, [MarshalAs(UnmanagedType.BStr)] ref string AttendantName, ref Int64 AttendantTag,
            ref Int32 CardClientID, ref Int32 CardClientNumber, [MarshalAs(UnmanagedType.BStr)] ref string CardClientName, ref Int64 CardClientTag);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliverySummaryEx3(Int32 ID, ref Int32 HoseID, ref Int32 HoseNumber, ref Int32 HosePhysicalNumber,
            ref Int32 PumpID, ref Int32 PumpNumber, [MarshalAs(UnmanagedType.BStr)] ref string PumpName,
            ref Int32 TankID, ref Int32 TankNumber, [MarshalAs(UnmanagedType.BStr)] ref string TankName,
            ref Int32 GradeID, ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string GradeShortName, [MarshalAs(UnmanagedType.BStr)] ref string pGradeCode,
            ref Int16 State, ref Int16 Type, ref Double Volume, ref Int16 PriceLevel,
            ref Double Price, ref Double Value, ref Double Volume2, ref DateTime CompletedDT, ref Int32 LockedBy,
            ref Int32 ReservedBy, ref Int32 AttendantID, ref Int32 Age, ref DateTime ClearedDT,
            ref Double OldVolumeETot, ref Double OldVolume2ETot, ref Double OldValueETot,
            ref Double NewVolumeETot, ref Double NewVolume2ETot, ref Double NewValueETot,
            ref Int64 Tag, ref Int32 Duration, ref Int32 AttendantNumber, [MarshalAs(UnmanagedType.BStr)] ref string AttendantName, ref Int64 AttendantTag,
            ref Int32 CardClientID, ref Int32 CardClientNumber, [MarshalAs(UnmanagedType.BStr)] ref string CardClientName, ref Int64 CardClientTag, ref Double pPeakFlowRate);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetDeliveryExt(Int32 ID, [MarshalAs(UnmanagedType.BStr)] string Plate,
            Double Odometer, Double Odometer2, [MarshalAs(UnmanagedType.BStr)] string ExtTransactionID,
            [MarshalAs(UnmanagedType.BStr)] string DriverID, [MarshalAs(UnmanagedType.BStr)] string AuthID,
            Int16 AuthType);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeliveryExt(Int32 ID, [MarshalAs(UnmanagedType.BStr)] ref string Plate,
            ref Double Odometer, ref Double Odometer2, [MarshalAs(UnmanagedType.BStr)] ref string ExtTransactionID, 
            [MarshalAs(UnmanagedType.BStr)] ref string DriverID, [MarshalAs(UnmanagedType.BStr)] ref string AuthID,
            ref Int16 AuthType);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpDeliveryProperties(Int32 ID, Int16 Index, ref Int32 DeliveryID, ref Int16 Type, ref Int16 State, ref Int32 HoseID, ref Int16 HoseNum, ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price, ref Double Volume, ref Double Value, ref Int32 LockedBy, ref Int32 ReservedBy, ref Int32 Age, ref DateTime CompletedDT, ref Int32 AttendantID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpDeliveryPropertiesEx(Int32 ID, Int16 Index, ref Int32 DeliveryID, ref Int16 Type, ref Int16 State, ref Int32 HoseID, ref Int16 HoseNum, ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price, ref Double Volume, ref Double Value, ref Int32 LockedBy, ref Int32 ReservedBy, ref Int32 Age, ref DateTime CompletedDT, ref Int32 AttendantID,
                                        ref Double VolumeETot, ref Double Volume2ETot, ref Double ValueETot, ref Int64 Tag);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpDeliveryPropertiesEx2(Int32 ID, Int16 Index, ref Int32 DeliveryID, ref Int16 Type, ref Int16 State, ref Int32 HoseID, ref Int16 HoseNum, ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price, ref Double Volume, ref Double Value, ref Int32 LockedBy, ref Int32 ReservedBy, ref Int32 Age, ref DateTime CompletedDT, ref Int32 AttendantID,
                                        ref Double OldVolumeETot, ref Double OldVolume2ETot, ref Double OldValueETot,
                                        ref Double NewVolumeETot, ref Double NewVolume2ETot, ref Double NewValueETot,
                                        ref Int64 Tag, ref Int32 Duration);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpDeliveryPropertiesEx3(Int32 ID, Int16 Index, ref Int32 DeliveryID, ref Int16 Type, ref Int16 State, ref Int32 HoseID, ref Int16 HoseNum, ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName, ref Int16 PriceLevel, ref Double Price, ref Double Volume, ref Double Value, ref Int32 LockedBy, ref Int32 ReservedBy, ref Int32 Age, ref DateTime CompletedDT, ref Int32 AttendantID,
                                       ref Double OldVolumeETot, ref Double OldVolume2ETot, ref Double OldValueETot,
                                       ref Double NewVolumeETot, ref Double NewVolume2ETot, ref Double NewValueETot,
                                       ref Int64 Tag, ref Int32 Duration, ref Int32 CardClientID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPumpDeliveryPropertiesEx4(ref Int32 ID, ref Int16 Index, ref Int32 DeliveryID, ref Int16 Type, ref Int16 State, ref Int32 HoseID, ref Int16 HoseNum, ref Int32 GradeID, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string ShortGradeName,
                                        ref Int16 PriceLevel, ref Double Price, ref Double Volume, ref Double Value, ref Int32 LockedBy,
                                        ref Int32 ReservedBy, ref Int32 Age, ref DateTime CompletedDT, ref Int32 AttendantID,
                                        ref Double OldVolumeETot, ref Double OldVolume2ETot, ref Double OldValueETot,
                                        ref Double NewVolumeETot, ref Double NewVolume2ETot, ref Double NewValueETot,
                                        ref Int64 Tag, ref Int32 Duration, ref Int32 CardClientID, ref Double PeakFlowRate);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.BStr)]
        internal static extern string ReserveTypestring(Int16 Type);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDuration(Int32 ID, ref Int32 Duration);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 StackCurrentDelivery(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.BStr)]
        internal static extern string DeliveryStatestring(Int16 State);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.BStr)]
        internal static extern string DeliveryTypestring(Int16 Type);

        //-------------------------------------- Hoses -----------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetHosesCount(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetHoseByOrdinal(Int32 Index, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetHoseProperties(Int32 ID, ref Int32 Number, ref Int32 PumpID, ref Int32 TankID, ref Int32 PhysicalNumber, ref Double MtrTheoValue, ref Double MtrTheoVolume, ref Double MtrElecValue, ref Double MtrElecVolume);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetHosePropertiesEx(Int32 ID, ref Int32 Number, ref Int32 PumpID, ref Int32 TankID, ref Int32 PhysicalNumber,
                              ref Double MtrTheoValue, ref Double MtrTheoVolume, ref Double MtrElecValue,
                              ref Double MtrElecVolume, ref Int16 UVEAntenna );

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetHosePropertiesEx2(Int32 ID, ref Int32 Number, ref Int32 PumpID, ref Int32 TankID, ref Int32 PhysicalNumber,
                              ref Double MtrTheoValue, ref Double MtrTheoVolume, ref Double MtrElecValue,
                              ref Double MtrElecVolume, ref Int16 UVEAntenna, ref Double Price1, ref Double Price2, ref Int16 Enabled);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetHoseProperties(Int32 ID, Int32 Number, Int32 PumpID, Int32 TankID, Int32 PhysicalNumber, Double MtrTheoValue, Double MtrTheoVolume, Double MtrElecValue, Double MtrElecVolume);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetHosePropertiesEx(Int32 ID, Int32 Number, Int32 PumpID, Int32 TankID, Int32 PhysicalNumber,
                              Double MtrTheoValue, Double MtrTheoVolume, ref Double MtrElecValue,
                              Double MtrElecVolume, Int16 UVEAntenna);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetHosePropertiesEx2(Int32 ID, Int32 Number, Int32 PumpID, Int32 TankID, Int32 PhysicalNumber,
                              Double MtrTheoValue, Double MtrTheoVolume, Double MtrElecValue,
                              Double MtrElecVolume, Int16 UVEAntenna, Double Price1, Double Price2, Int16 Enabled);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DeleteHose(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetHosePrices(Int32 ID, ref Int16 DurationType, ref Int16 PriceType, ref Double Price1, ref Double Price2);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetHoseSummary(Int32 ID, ref Int32 Number, ref Int32 PhysicalNumber,
            ref Int32 PumpID, ref Int32 PumpNumber, [MarshalAs(UnmanagedType.BStr)] ref string PumpName,
            ref Int32 TankID, ref Int32 TankNumber, [MarshalAs(UnmanagedType.BStr)] ref string TankName,
            ref Int32 GradeID, ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string GradeShortName, [MarshalAs(UnmanagedType.BStr)] ref string GradeCode,
            ref Double MtrTheoValue, ref Double MtrTheoVolume, ref Double MtrElecValue, ref Double MtrElecVolume);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetHoseSummaryEx(Int32 ID, ref Int32 Number, ref Int32 PhysicalNumber,
            ref Int32 PumpID, ref Int32 PumpNumber, [MarshalAs(UnmanagedType.BStr)] ref string PumpName,
            ref Int32 TankID, ref Int32 TankNumber, [MarshalAs(UnmanagedType.BStr)] ref string TankName,
            ref Int32 GradeID, ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string GradeShortName, [MarshalAs(UnmanagedType.BStr)] ref string GradeCode,
            ref Double MtrTheoValue, ref Double MtrTheoVolume, ref Double MtrElecValue, ref Double MtrElecVolume,
            ref Double Price1, ref Double Price2, ref Int16 Enabled);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetHoseETotals(Int32 ID, Double Volume, Double Value);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetHosePrices(Int32 id, Int16 DurationType, Int16 PriceType, Double Price1, Double Price2);

        //-------------------------------------- Grades ---------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetGradesCount(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetGradeByNumber(Int32 Number, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetGradeByName([MarshalAs(UnmanagedType.BStr)] string Name, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetGradeByOrdinal(Int32 Index, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetGradeProperties(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, [MarshalAs(UnmanagedType.BStr)] ref string ShortName, [MarshalAs(UnmanagedType.BStr)] ref string Code);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetGradePropertiesEx(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, [MarshalAs(UnmanagedType.BStr)] ref string ShortName, [MarshalAs(UnmanagedType.BStr)] ref string Code, ref Int16 Type);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetGradeProperties(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.BStr)] string ShortName, [MarshalAs(UnmanagedType.BStr)] string Code);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetGradePropertiesEx(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.BStr)] string ShortName, [MarshalAs(UnmanagedType.BStr)] string Code, Int16 Type);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DeleteGrade(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetGradePrice(Int32 ID, Int16 Level, Double Price);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetGradePrice(Int32 ID, Int16 Level, ref Double Price);

        //-------------------------------------- Tanks ----------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetTanksCount(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetTankByNumber(Int32 Number, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetTankByName([MarshalAs(UnmanagedType.BStr)] string Name, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetTankByOrdinal(Int32 Index, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetTankProperties(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, ref Int32 GradeID, ref Int16 Type, ref Double Capacity, ref Double Diameter, ref Double TheoVolume, ref Double GaugeVolume, ref Double GaugeTCVolume, ref Double GaugeUllage, ref Double GaugeTemperature, ref Double GaugeLevel, ref Double GaugeWaterVolume, ref Double GaugeWaterLevel, ref Int32 GaugeID, ref Int16 ProbeNo);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetTankPropertiesEx(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, ref Int32 GradeID, ref Int16 Type, ref Double Capacity, ref Double Diameter,
            ref Double TheoVolume, ref Double GaugeVolume, ref Double GaugeTCVolume, ref Double GaugeUllage, ref Double GaugeTemperature,
            ref Double GaugeLevel, ref Double GaugeWaterVolume, ref Double GaugeWaterLevel, ref Int32 GaugeID, ref Int16 ProbeNo, ref Int32 GaugeAlarmsMask);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetTankProperties(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, Int32 GradeID, Int16 Type, Double Capacity, Double Diameter, Double TheoVolume, Double GaugeVolume, Double GaugeTCVolume, Double GaugeUllage, Double GaugeTemperature, Double GaugeLevel, Double GaugeWaterVolume, Double GaugeWaterLevel, Int32 GaugeID, Int16 ProbeNo);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetTankPropertiesEx(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, Int32 GradeID, Int16 Type, Double Capacity, Double Diameter,
            Double TheoVolume, Double GaugeVolume, Double GaugeTCVolume, Double GaugeUllage, Double GaugeTemperature,
            Double GaugeLevel, Double GaugeWaterVolume, Double GaugeWaterLevel, Int32 GaugeID, Int16 ProbeNo, Int32 GaugeAlarmsMask);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DeleteTank(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetTankSummary(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, ref Int32 GradeID,
            ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string GradeShortName, [MarshalAs(UnmanagedType.BStr)] ref string GradeCode,
            ref Int16 Type, ref Double Capacity, ref Double Diameter,
            ref Double TheoVolume, ref Double GaugeVolume, ref Double GaugeTCVolume, ref Double GaugeUllage, ref Double GaugeTemperature,
            ref Double GaugeLevel, ref Double GaugeWaterVolume, ref Double GaugeWaterLevel, ref Int32 GaugeID, ref Int16 ProbeNo);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetTankSummaryEx(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, ref Int32 GradeID,
            ref Int32 GradeNumber, [MarshalAs(UnmanagedType.BStr)] ref string GradeName, [MarshalAs(UnmanagedType.BStr)] ref string GradeShortName, [MarshalAs(UnmanagedType.BStr)] ref string GradeCode,
            ref Int16 Type, ref Double Capacity, ref Double Diameter,
            ref Double TheoVolume, ref Double GaugeVolume, ref Double GaugeTCVolume, ref Double GaugeUllage, ref Double GaugeTemperature,
            ref Double GaugeLevel, ref Double GaugeWaterVolume, ref Double GaugeWaterLevel, ref Int32 GaugeID, ref Int16 ProbeNo,
            ref Int16 State, ref Int32 GaugeAlarmsMask);

        //-------------------------------------- Ports ----------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPortsCount(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPortByNumber(Int32 Number, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPortByName([MarshalAs(UnmanagedType.BStr)] string Name, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPortByOrdinal(Int32 Index, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetPortProperties(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, ref Int32 ProtocolID, ref Int16 DeviceType, [MarshalAs(UnmanagedType.BStr)] ref string SerialNo);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetPortProperties(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, Int32 ProtocolID, Int16 DeviceType, [MarshalAs(UnmanagedType.BStr)] string SerialNo);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 RemovePort(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetZB2GConfig(Int32 ID, ref Int64 PanID, ref Int32 Channels, ref Int64 KeyA, ref Int64 KeyB);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetSerialNo(Int32 ID, [MarshalAs(UnmanagedType.BStr)] ref string SerialNo);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetDeviceDetails(Int32 ID, Int32 ZBID, [MarshalAs(UnmanagedType.BStr)] ref string SerialNo, [MarshalAs(UnmanagedType.BStr)] ref string BootVersion, [MarshalAs(UnmanagedType.BStr)] ref string FirmwareVersion);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 ResetDevice(Int32 ID, Int32 ZBID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 RequestVersion(Int32 ID, Int32 ZBID);

        //-------------------------------------- Attendants -----------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetAttendantsCount(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetAttendantByNumber(Int32 Number, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetAttendantByName([MarshalAs(UnmanagedType.BStr)] string Name, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetAttendantByOrdinal(Int32 Index, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetAttendantProperties(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, [MarshalAs(UnmanagedType.BStr)] ref string ShortName, [MarshalAs(UnmanagedType.BStr)] ref string Password, [MarshalAs(UnmanagedType.BStr)] ref string Tag);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetAttendantPropertiesEx(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, [MarshalAs(UnmanagedType.BStr)] ref string ShortName, [MarshalAs(UnmanagedType.BStr)] ref string Password, [MarshalAs(UnmanagedType.BStr)] ref string Tag,
            ref Int16 ShiftAStart, ref Int16 ShiftAEnd, ref Int16 ShiftBStart, ref Int16 ShiftBEnd, ref Int16 Enabled);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetAttendantProperties(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.BStr)] string ShortName, [MarshalAs(UnmanagedType.BStr)] string Password, [MarshalAs(UnmanagedType.BStr)] string Tag);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetAttendantPropertiesEx(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.BStr)] string ShortName, [MarshalAs(UnmanagedType.BStr)] string Password, [MarshalAs(UnmanagedType.BStr)] string Tag,
            short ShiftAStart, short ShiftAEnd, short ShiftBStart, short ShiftBEnd, short Enabled);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetAttendantState(Int32 ID, ref Int16 Type, ref Int16 LoggedOn);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DeleteAttendant(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 AttendantLogon(Int32 ID, Int32 PumpID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 AttendantLogoff(Int32 ID);

        //------------------------------ Card Clients -----------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardClientsCount(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardClientByNumber(Int32 Number, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardClientByName([MarshalAs(UnmanagedType.BStr)] string Name, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardClientByOrdinal(Int32 Index, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardClientProperties(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, [MarshalAs(UnmanagedType.BStr)] ref string Tag, ref Int16 Enabled);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardClientPropertiesEx(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, [MarshalAs(UnmanagedType.BStr)] ref string Tag, ref Int16 Enabled, ref Int16 PriceLevel, [MarshalAs(UnmanagedType.BStr)] ref string Plate);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardClientPropertiesEx2(Int32 ID, ref Int32 pNumber, [MarshalAs(UnmanagedType.BStr)] ref string pName, [MarshalAs(UnmanagedType.BStr)] ref string pTag, ref Int16 pEnabled, ref Int16 pPriceLevel, [MarshalAs(UnmanagedType.BStr)] ref string pPlate, ref Int16 pGradeType);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetCardClientProperties(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.BStr)] string Tag, short Enabled);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetCardClientPropertiesEx(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.BStr)] string Tag, Int16 Enabled, Int16 PriceLevel, [MarshalAs(UnmanagedType.BStr)] string Plate);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetCardClientPropertiesEx2(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, [MarshalAs(UnmanagedType.BStr)] string Tag, Int16 Enabled, Int16 PriceLevel, [MarshalAs(UnmanagedType.BStr)] string Plate, Int16 GradeType);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DeleteCardClient(Int32 ID);

        //------------------------------ Card Reads -----------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardReadsCount(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardReadByNumber(Int32 Number, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardReadByName([MarshalAs(UnmanagedType.BStr)] string Name, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardReadByOrdinal(Int32 Index, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardReadProperties(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, ref Int32 PumpID, ref Int16 Type, ref Int32 ParentID, ref Int64 Tag, ref DateTime TimeStamp);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetCardReadProperties(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, Int32 PumpID, short Type, Int32 ParentID, Int64 Tag, DateTime TimeStamp);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DeleteCardRead(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetCardType([MarshalAs(UnmanagedType.BStr)] string Tag, 
            ref Int32 pTagType, ref Int32 pID, [MarshalAs(UnmanagedType.BStr)] 
            ref string pName, ref Int32 pNumber);

        //------------------------------ ZigBee devices ---------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetZigBeeCount(ref Int32 Count);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetZigBeeByNumber(Int32 Number, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetZigBeeByName([MarshalAs(UnmanagedType.BStr)] string Name, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetZigBeeByOrdinal(Int32 Index, ref Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetZigBeeProperties(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, ref Int16 DeviceType, [MarshalAs(UnmanagedType.BStr)] ref string SerialNumber, [MarshalAs(UnmanagedType.BStr)] ref string NodeIdentifier, ref Int32 PortID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetZigBeeProperties(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, Int16 DeviceType, [MarshalAs(UnmanagedType.BStr)] string SerialNumber, [MarshalAs(UnmanagedType.BStr)] string NodeIdentifier, Int32 PortID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DeleteZigBee(Int32 ID);

        //------------------------------ Sensors ----------------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetSensorsCount( ref Int32 Count ) ;

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetSensorByNumber(Int32 Number, ref Int32 ID);
        
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetSensorByName([MarshalAs(UnmanagedType.BStr)] string Name, ref Int32 ID);
        
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetSensorByOrdinal(Int32 Index, ref Int32 ID);
        
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetSensorProperties(Int32 ID, ref Int32 Number, [MarshalAs(UnmanagedType.BStr)] ref string Name, ref Int32 PortID, ref Int16 Type, ref Int16 Address, ref Int16 SensorNo);
        
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetSensorProperties(Int32 ID, Int32 Number, [MarshalAs(UnmanagedType.BStr)] string Name, Int32 PortID, Int16 Type, Int16 Address, Int16 SensorNo);
        
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetSensorStatus(Int32 ID, Int16 State, Int16 IsResponding);
        
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetSensorStatus(Int32 ID, ref Int16 State, ref Int16 IsResponding);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DeleteSensor(Int32 ID);

        //------------------------------ Logged events -----------------------------------------//

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetLogEventCount(ref Int32 Count, Int16 DeviceType, Int32 DeviceID, Int16 EventLevel, Int16 EventType, Int32 ClearedBy, Int32 AckedBy);
        
        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetLogEventByOrdinal(Int32 Index, ref Int32 ID, Int16 DeviceType, Int32 DeviceID, Int16 EventLevel, Int16 EventType, Int32 ClearedBy, Int32 AckedBy);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 GetLogEventProperties(Int32 ID, ref Int16 DeviceType, ref Int32 DeviceID, ref Int32 DeviceNumber, [MarshalAs(UnmanagedType.BStr)] ref string DeviceName, ref Int16 EventLevel,
           ref Int16 EventType, [MarshalAs(UnmanagedType.BStr)] ref string EventDesc, ref DateTime GeneratedDT, ref DateTime ClearedDT, ref Int32 ClearedBy, ref Int32 AckedBy, ref Double Volume,
           ref Double Value, ref Double ProductVolume, ref Double ProductLevel, ref Double WaterLevel, ref Double Temperature);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 SetLogEventProperties(Int32 ID, Int16 DeviceType, Int32 DeviceID, Int32 DeviceNumber, [MarshalAs(UnmanagedType.BStr)] string DeviceName, Int16 EventLevel,
           Int16 EventType, [MarshalAs(UnmanagedType.BStr)] string EventDesc, DateTime GeneratedDT, DateTime ClearedDT, Int32 ClearedBy, Int32 AckedBy, Double Volume,
           Double Value, Double ProductVolume, Double ProductLevel, Double WaterLevel, Double Temperature);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 DeleteLogEvent(Int32 ID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 ClearLogEvent(Int32 ID, Int32 ClientID);

        [DllImport(DllName, CharSet = CharSet.Unicode)]
        internal static extern Int32 AckLogEvent(Int32 ID, Int32 ClientID);

        }
}
