Comile project and integration into Dynamo: 

1. Clone git
2. open sln 
3. Re-link dependencies if this does not happen automatically

3. Build projects:
* DataLayer
* OffLibrary
* IfcBridgeToolKit
* DynIntegration

Getting Started Sample

Use the following example to test if your Dynamo accepts the IfcBridgeToolKit dll: 

![DynSmallSample](https://gitlab.lrz.de/sebastian.esser/tumcms_ifcbridgetoolkit/-/blob/7fe40df296320d99235f2e087b40de36eb6a63be/GitFigures/SmallSample.png)

These nodes should result in something like: 

'''
ISO-10303-21;
HEADER;
FILE_DESCRIPTION ((''), '2;1');
FILE_NAME ('', '2020-04-21T20:44:10', (''), (''), 'Processor version 5.9999.0.0', 'Xbim.IO.MemoryModel', '');
FILE_SCHEMA (('IFC4X2'));
ENDSEC;
DATA;
#1=IFCPROJECT('3ZdGy3tlX01ujoDPOLIP0V',#2,'testProject',$,$,$,$,$,#7);
#2=IFCOWNERHISTORY(#5,#6,$,.ADDED.,1587501850,$,$,0);
#3=IFCPERSON($,'Esser','Sebastian',$,$,$,$,$);
#4=IFCORGANIZATION($,'Technical University of Munich',$,$,$);
#5=IFCPERSONANDORGANIZATION(#3,#4,$);
#6=IFCAPPLICATION(#4,'1.1','TUM_CMS_IfcBridgeToolkit',$);
#7=IFCUNITASSIGNMENT((#8,#9,#10,#11));
#8=IFCSIUNIT(*,.LENGTHUNIT.,$,.METRE.);
#9=IFCSIUNIT(*,.PLANEANGLEUNIT.,$,.RADIAN.);
#10=IFCSIUNIT(*,.AREAUNIT.,$,.SQUARE_METRE.);
#11=IFCSIUNIT(*,.VOLUMEUNIT.,$,.CUBIC_METRE.);
#12=IFCLOCALPLACEMENT($,#13);
#13=IFCAXIS2PLACEMENT3D(#14,$,$);
#14=IFCCARTESIANPOINT((0.,0.,0.));
#15=IFCGEOMETRICREPRESENTATIONCONTEXT($,'Model',3,1.E-05,#19,#20);
#16=IFCCARTESIANPOINT((0.,0.,0.));
#17=IFCDIRECTION((0.,0.,1.));
#18=IFCDIRECTION((1.,0.,0.));
#19=IFCAXIS2PLACEMENT3D(#16,#17,#18);
#20=IFCDIRECTION((0.,1.));
#21=IFCSITE('2LtjOOZkf7vPkcfuAmfPnv',#2,'BridgeSite','SiteDescription',$,#12,$,$,$,$,$,0.,$,$);
#22=IFCRELAGGREGATES('2r4uy0NDz4iwuT4xzZHxIC',#2,$,$,#1,(#21));
ENDSEC;
END-ISO-10303-21;
'''
