CALL osae_sp_object_type_add ('PHIDGET-RFID','Phidget RFID Plugin','','PLUGIN',1,1,0,1);
CALL osae_sp_object_type_state_add ('ON','Running','PHIDGET-RFID');
CALL osae_sp_object_type_state_add ('OFF','Stopped','PHIDGET-RFID');
CALL osae_sp_object_type_event_add ('ON','Started','PHIDGET-RFID');
CALL osae_sp_object_type_event_add ('OFF','Stopped','PHIDGET-RFID');
CALL osae_sp_object_type_method_add ('ON','Start','PHIDGET-RFID','','','','');
CALL osae_sp_object_type_method_add ('OFF','Stop','PHIDGET-RFID','','','','');
CALL osae_sp_object_type_property_add ('Attached','Boolean','FALSE','PHIDGET-RFID',0);
CALL osae_sp_object_type_property_add ('Name','String','','PHIDGET-RFID',0);
CALL osae_sp_object_type_property_add ('Version','String','','PHIDGET-RFID',0);
CALL osae_sp_object_type_property_add ('Serial','String','','PHIDGET-RFID',0);
CALL osae_sp_object_type_property_add ('Antenna Enabled','Boolean','FALSE','PHIDGET-RFID',0);
CALL osae_sp_object_type_property_add ('LED Enabled','Boolean','TRUE','PHIDGET-RFID',0);
CALL osae_sp_object_type_property_add ('Output 1 ON','Boolean','FALSE','PHIDGET-RFID',0);
CALL osae_sp_object_type_property_add ('Output 2 ON','Boolean','FALSE','PHIDGET-RFID',0);
CALL osae_sp_object_type_property_add ('Last Tag Read','String','','PHIDGET-RFID',0);

CALL osae_sp_object_type_add ('PHIDGET RFID TAG','Phidget RFID Tag','','PHIDGET RFID TAG',0,0,0,0);
CALL osae_sp_object_type_state_add ('ON','Detected','PHIDGET RFID TAG');
CALL osae_sp_object_type_event_add ('ON','Detected','PHIDGET RFID TAG');