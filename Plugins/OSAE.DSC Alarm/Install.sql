CALL osae_sp_object_type_add ('DSC ALARM','DSC Alarm Panel','','PLUGIN',1,0,0,0);
CALL osae_sp_object_type_state_add ('ON','Running','DSC ALARM');
CALL osae_sp_object_type_state_add ('OFF','Stopped','DSC ALARM');
CALL osae_sp_object_type_event_add ('ON','Started','DSC ALARM');
CALL osae_sp_object_type_event_add ('OFF','Stopped','DSC ALARM');
CALL osae_sp_object_type_property_add ('Software Version','String','','DSC ALARM',0);
CALL osae_sp_object_type_property_add ('Baud','Integer','9600','DSC ALARM',0);
CALL osae_sp_object_type_property_add ('Update Time','Integer','60','DSC ALARM',0);
CALL osae_sp_object_type_property_add ('Code','Integer','0','DSC ALARM',0);
CALL osae_sp_object_type_property_add ('Password','String','','DSC ALARM',0);
CALL osae_sp_object_type_property_add ('Ethernet','Boolean','FALSE','DSC ALARM',0);
CALL osae_sp_object_type_property_add ('Keybus','String','','DSC ALARM',0);

CALL osae_sp_object_type_add ('DSC ALARM PARTITION','DSC Alarm Partition','DSC Alarm','DSC ALARM PARTITION',0,0,0,0);
CALL osae_sp_object_type_state_add ('READY','Ready','DSC ALARM PARTITION');
CALL osae_sp_object_type_state_add ('NOTREADY','Not Ready','DSC ALARM PARTITION');
CALL osae_sp_object_type_state_add ('ARMEDAWAYNODELAY','Armed - Away, No Delay','DSC ALARM PARTITION');
CALL osae_sp_object_type_state_add ('READYFORCEARMING','Ready for Force Arming','DSC ALARM PARTITION');
CALL osae_sp_object_type_state_add ('INALARM','In Alarm','DSC ALARM PARTITION');
CALL osae_sp_object_type_state_add ('DISARMED','Disarmed','DSC ALARM PARTITION');
CALL osae_sp_object_type_state_add ('EXITDELAYINPROGRESS','Exit Delay In Progress','DSC ALARM PARTITION');
CALL osae_sp_object_type_state_add ('ENTRYDELAYINPROGRESS','Entry Delay In Progress','DSC ALARM PARTITION');
CALL osae_sp_object_type_state_add ('ARMEDAWAY','Armed - Away','DSC ALARM PARTITION');
CALL osae_sp_object_type_state_add ('ARMEDSTAY','Armed - Stay','DSC ALARM PARTITION');
CALL osae_sp_object_type_state_add ('ARMEDSTAYNODELAY','Armed - Stay, No Delay','DSC ALARM PARTITION');
CALL osae_sp_object_type_event_add ('INALARM','Partition In Alarm','DSC ALARM PARTITION');
CALL osae_sp_object_type_event_add ('ARMEDAWAY','Partition Armed - Away','DSC ALARM PARTITION');
CALL osae_sp_object_type_event_add ('EXITDELAYINPROGRESS','Partition Exit Delay In Progress','DSC ALARM PARTITION');
CALL osae_sp_object_type_event_add ('ENTRYDELAYINPROGRESS','Partition Entry Delay In Progress','DSC ALARM PARTITION');
CALL osae_sp_object_type_event_add ('ARMEDSTAY','Partition Armed - Stay','DSC ALARM PARTITION');
CALL osae_sp_object_type_event_add ('ARMEDAWAYNODELAY','Partition Armed - Away, No Delay','DSC ALARM PARTITION');
CALL osae_sp_object_type_event_add ('ARMEDSTAYNODELAY','Partition Armed - Stay, No Delay','DSC ALARM PARTITION');
CALL osae_sp_object_type_event_add ('READY','Partition Ready','DSC ALARM PARTITION');
CALL osae_sp_object_type_event_add ('NOTREADY','Partition Not Ready','DSC ALARM PARTITION');
CALL osae_sp_object_type_event_add ('DISARMED','Partition Disarmed','DSC ALARM PARTITION');
CALL osae_sp_object_type_event_add ('READYFORCEARMING','Partition Ready for Forced Arming','DSC ALARM PARTITION');

CALL osae_sp_object_type_add ('DSC ALARM ZONE','DSC Alarm Zone','','DSC ALARM ZONE',0,0,0,0);
CALL osae_sp_object_type_state_add ('OPEN','Open','DSC ALARM ZONE');
CALL osae_sp_object_type_state_add ('CLOSED','Closed','DSC ALARM ZONE');
CALL osae_sp_object_type_event_add ('CLOSED','Closed','DSC ALARM ZONE');
CALL osae_sp_object_type_event_add ('OPEN','Opened','DSC ALARM ZONE');

CALL osae_sp_object_add ('DSC Alarm','DSC Alarm','DSC ALARM','3','SYSTEM',1,@results);
CALL osae_sp_object_property_set ('Software Version','','DSC Alarm','', null);
CALL osae_sp_object_property_set ('Baud','9600','DSC Alarm','', null);
CALL osae_sp_object_property_set ('Update Time','60','DSC Alarm','', null);
CALL osae_sp_object_property_set ('Code','0','DSC Alarm','', null);
CALL osae_sp_object_property_set ('Password','','DSC Alarm','', null);
CALL osae_sp_object_property_set ('Ethernet','FALSE','DSC Alarm','', null);
CALL osae_sp_object_property_set ('Keybus','','DSC Alarm','', null);

CALL osae_sp_object_add ('Alarm Partition 1','DSC Alarm Partion 1','DSC ALARM PARTITION','1','SYSTEM',1,@results);

CALL osae_sp_object_add ('Alarm Zone 01','DSC Alarm Zone 1','DSC ALARM ZONE','01','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 02','DSC Alarm Zone 2','DSC ALARM ZONE','02','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 03','DSC Alarm Zone 3','DSC ALARM ZONE','03','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 04','DSC Alarm Zone 4','DSC ALARM ZONE','04','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 05','DSC Alarm Zone 5','DSC ALARM ZONE','05','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 06','DSC Alarm Zone 6','DSC ALARM ZONE','06','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 07','DSC Alarm Zone 7','DSC ALARM ZONE','07','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 08','DSC Alarm Zone 8','DSC ALARM ZONE','08','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 09','DSC Alarm Zone 9','DSC ALARM ZONE','09','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 10','DSC Alarm Zone 10','DSC ALARM ZONE','10','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 11','DSC Alarm Zone 11','DSC ALARM ZONE','11','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 12','DSC Alarm Zone 12','DSC ALARM ZONE','12','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 13','DSC Alarm Zone 13','DSC ALARM ZONE','13','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 14','DSC Alarm Zone 14','DSC ALARM ZONE','14','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 15','DSC Alarm Zone 15','DSC ALARM ZONE','15','SYSTEM',1,@results);
CALL osae_sp_object_add ('Alarm Zone 16','DSC Alarm Zone 16','DSC ALARM ZONE','16','SYSTEM',1,@results);
