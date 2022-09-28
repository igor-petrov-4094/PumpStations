using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using App.PumpFactsMobile.ServiceDataModels;

namespace App.PumpFactsMobile.ViewModels
{
    public class PumpStationLeftPageViewModel : PumpStationDetailPageViewModel
    {
        public ParamValueInfo pch_freq_output                                     { get { return pvByKey("pch.freq_output"); } }
        public ParamValueInfo pch_speed_fact                                      { get { return pvByKey("pch.speed_fact"); } }
        public ParamValueInfo pch_motor_current                                   { get { return pvByKey("pch.motor_current"); } }
        public ParamValueInfo pch_motor_voltage                                   { get { return pvByKey("pch.motor_voltage"); } }
        public ParamValueInfo pch_state_failure                                   { get { return pvByKey("pch.state.failure"); } }
        public ParamValueInfo pch_state_malfunction                               { get { return pvByKey("pch.state.malfunction"); } }
        public ParamValueInfo pch_state_inprocess                                 { get { return pvByKey("pch.state.inprocess"); } }
        public ParamValueInfo pch_state_ready                                     { get { return pvByKey("pch.state.ready"); } }
        public ParamValueInfo protection_dry_run_pump_unit                        { get { return pvByKey("protection.dry_run_pump_unit"); } }
        public ParamValueInfo protection_pump_unit_current_excess                 { get { return pvByKey("protection.pump_unit_current_excess"); } }
        public ParamValueInfo protection_voltage_disappearance                    { get { return pvByKey("protection.voltage_disappearance"); } }
        public ParamValueInfo locks_bore_debet_excess                             { get { return pvByKey("locks.bore_debet_excess"); } }
        public ParamValueInfo locks_bore_consumption_excess                       { get { return pvByKey("locks.bore_consumption_excess"); } }
        public ParamValueInfo signalling_pump_aggregate_stopping                  { get { return pvByKey("signalling.pump_aggregate_stopping"); } }
        public ParamValueInfo signalling_bore_debet_elevation                     { get { return pvByKey("signalling.bore_debet_elevation"); } }
        public ParamValueInfo signalling_pump_aggregate_current_elevation         { get { return pvByKey("signalling.pump_aggregate_current_elevation"); } }
        public ParamValueInfo signalling_bore_level_decrease                      { get { return pvByKey("signalling.bore_level_decrease"); } }
        public ParamValueInfo signalling_no_pch_controller_interchange            { get { return pvByKey("signalling.no_pch_controller_interchange"); } }
        public ParamValueInfo signalling_door_is_open                             { get { return pvByKey("signalling.door_is_open"); } }
        public ParamValueInfo signalling_volume_defection                         { get { return pvByKey("signalling.volume_defection"); } }
        public ParamValueInfo signalling_general_failure                          { get { return pvByKey("signalling.general_failure"); } }
        public ParamValueInfo indication_manual                                   { get { return pvByKey("indication.manual"); } }
        public ParamValueInfo indication_off                                      { get { return pvByKey("indication.off"); } }
        public ParamValueInfo indication_automate                                 { get { return pvByKey("indication.automate"); } }
        public ParamValueInfo control_pump                                        { get { return pvByKey("control.pump"); } }
        public ParamValueInfo control_general_run                                 { get { return pvByKey("control.general.run"); } }
        public ParamValueInfo control_general_stop                                { get { return pvByKey("control.general.stop"); } }
        public ParamValueInfo params_pressure_given                               { get { return pvByKey("params.pressure.given"); } }
        public ParamValueInfo params_pressure_fact                                { get { return pvByKey("params.pressure.fact"); } }
        public ParamValueInfo params_level_above_pump                             { get { return pvByKey("params.level_above_pump"); } }
        public ParamValueInfo params_consumption                                  { get { return pvByKey("params.consumption"); } }
        public ParamValueInfo D216                                                { get { return pvByKey("D216"); } }
        public ParamValueInfo params_computable_level_ex                          { get { return pvByKey("params.computable.level_ex"); } }

        public ParamValueInfo pvByKey(string key)
        {
            if (getItemByKey(key, out ParamValueInfo paramValueInfo))
                return paramValueInfo;
            else
                return null;
        }
    }
}
