﻿CALL osae_sp_object_type_add ('APC_UPS','APC UPS Monitor','','PLUGIN',1,0,0,1);
CALL osae_sp_object_type_state_add ('ON','Running','APC_UPS');
CALL osae_sp_object_type_state_add ('OFF','Stopped','APC_UPS');
CALL osae_sp_object_type_event_add ('ON','Started','APC_UPS');
CALL osae_sp_object_type_event_add ('OFF','Stopped','APC_UPS');
CALL osae_sp_object_type_event_add ('COMMFAILURE','Communication Failure','APC_UPS');
CALL osae_sp_object_type_event_add ('COMMOK','Communication OK','APC_UPS');
CALL osae_sp_object_type_event_add ('POWEROUT','Power Out','APC_UPS');
CALL osae_sp_object_type_event_add ('ONBATTERY','On Battery','APC_UPS');
CALL osae_sp_object_type_event_add ('OFFBATTERY','Off Battery','APC_UPS');
CALL osae_sp_object_type_event_add ('MAINSBACK','Mains back on','APC_UPS');
CALL osae_sp_object_type_event_add ('TIMEOUT','Timeout','APC_UPS');
CALL osae_sp_object_type_event_add ('LOADLIMIT','Load limit','APC_UPS');
CALL osae_sp_object_type_event_add ('RUNLIMIT','Run limit','APC_UPS');
CALL osae_sp_object_type_event_add ('FAILING','Failing','APC_UPS');
CALL osae_sp_object_type_event_add ('DOSHUTDOWN','Do Shutdown','APC_UPS');
CALL osae_sp_object_type_event_add ('ANNOYME','Annoy me','APC_UPS');
CALL osae_sp_object_type_event_add ('EMERGENCY','Emergency','APC_UPS');
CALL osae_sp_object_type_event_add ('CHANGEME','Change me','APC_UPS');
CALL osae_sp_object_type_event_add ('REMOTEDOWN','Remote down','APC_UPS');
CALL osae_sp_object_type_event_add ('STARTSELFTEST','Start self test','APC_UPS');
CALL osae_sp_object_type_event_add ('BATTDETACH','Battery detached','APC_UPS');
CALL osae_sp_object_type_event_add ('BATTATTACH','Battery attached','APC_UPS');
CALL osae_sp_object_type_event_add ('ENDSELFTEST','End self test','APC_UPS');
CALL osae_sp_object_type_method_add ('ON','Start','APC_UPS','','','','');
CALL osae_sp_object_type_method_add ('OFF','Stop','APC_UPS','','','','');
CALL osae_sp_object_type_method_add ('PRINTMESSAGE','Print Message from UPS','APC_UPS','','','','');
CALL osae_sp_object_type_method_add ('UPSEVENT','UPS Event','APC_UPS','Event','','','');
CALL osae_sp_object_type_property_add ('Computer Name','String','','APC_UPS',0);
CALL osae_sp_object_type_property_add ('STATUS','String','','APC_UPS',0);
CALL osae_sp_object_type_property_add ('LINEV','Float','','APC_UPS',0);
CALL osae_sp_object_type_property_add ('LOADPCT','Float','','APC_UPS',1);
CALL osae_sp_object_type_property_add ('BCHARGE','Integer','','APC_UPS',0);
CALL osae_sp_object_type_property_add ('TIMELEFT','Integer','','APC_UPS',0);
CALL osae_sp_object_type_property_add ('BATTV','Integer','','APC_UPS',0);
CALL osae_sp_object_type_property_add ('LASTXFER','String','','APC_UPS',0);
CALL osae_sp_object_type_property_add ('Update Interval','Integer','10','APC_UPS',0);
CALL osae_sp_object_type_property_add ('Host','String','localhost','APC_UPS',0);
CALL osae_sp_object_type_property_add ('Port','Integer','3551','APC_UPS',0);
CALL osae_sp_object_type_property_add ('Rated Watts','Integer','','APC_UPS',0);
